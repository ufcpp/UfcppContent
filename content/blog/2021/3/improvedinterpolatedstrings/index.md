---
title: "ピックアップRoslyn: Improved Interpolated Strings"
source_url: "https://ufcpp.net/blog/2021/3/improvedinterpolatedstrings/"
content_type: "BlogEntry"
published_at: "2021-03-20T20:47:11"
updated_at: "2021-03-20T20:47:11"
tags: []
umbraco_id: 2339
parent_id: 2336
sort_order: 2
aliases: []
---

# ピックアップRoslyn: Improved Interpolated Strings

[string interplation](../../../../study/csharp/start/st_string.md#string-interpolation) の改善するって。

## <a id="csharp-6">現行仕様</a>

C# 6.0 から以下のようなコードで `string.Format` 相当のことができるようになったわけですが。

```csharp
var s = $"({a}, {b})";
```

これは、以下のように展開されます。

```csharp
var s = string.Format("({0}, {1})", a, b);
```

これがパフォーマンス的にあんまりよろしくなくて…

特に、[冒頭の提案ドキュメント](../../../../study/csharp/start/st_string.md#string-interpolation)にもある通り、ロギング用途との相性が最悪で、
[`ILogger`](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger.log?WT.mc_id=DT-MVP-4028921&view=dotnet-plat-ext-5.0)のメソッドがなかなか使いにくそうな感じの引数になっています。

```csharp
void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
```

「`formatter` でラムダ式を渡して、その中で文字列化」みたいなことをしないといけなくて、結構面倒です。

もちろんこのまま使うのは大変なので `LogDebug` とか `LogTrace` とかの拡張メソッドには素直に `string` を引数として受け取るオーバーロードもあったりするんですが、
それがまた罠というか、パフォーマンスにシビアな場面で使ってしまうと露骨に遅くなるという問題が。

遅くなる原因はいくつかあって、

- 引数(上記の例でいうと `a` と `b`)を `object` で受け取ってしまう。引数が値型の時に[ボックス化](../../../../study/csharp/resource/rmboxing.md)を起こす
- 引数の数が多いと [`params`](../../../../study/csharp/structured/sp_params.md#params) 扱いになって配列の確保も起きる
- 即時評価なので、実際には不要なものも(ログレベル的に無視する文字列であっても)必ず文字列化される
- [`Span<T>`](../../../../study/csharp/resource/span.md) みたいな、C# 7.2 以降、パフォーマンスが重要な場面で多用することになった型を使えない

例えば以下のようなコード(一部仮想コードですが)があった場合、

```csharp
using System;
 
Log($"{DiagnosticMetric()}, {DiagnosticMetric()}, {DiagnosticMetric()}, {DiagnosticMetric()}");
 
string DiagnosticMetric()
{
    // 診断専用で、日常的に読むには少々重たい値がなにかあるとして
    return その値を返す;
}
 
void Log(string message)
{
    // LogLevel はコンパイル時に確定しない設定ファイルとかから読んだりする想定で
    if (LogLevel < 1) return;
 
    // もし、たいていの場面では LogLevel 0 で運用してるとここにはほとんど来ない。
    // 実際には message を読む必要がない。
    Console.WriteLine(message);
}
```

以下のように展開されて処理されます。

```csharp
// ただでさえ「必要な時にだけ呼びたい」というつもりのメソッドが無条件に呼ばれる。
object tmp1 = DiagnosticMetric(); // int → object に代入しててボックス化。
object tmp2 = DiagnosticMetric();
object tmp3 = DiagnosticMetric();
object tmp4 = DiagnosticMetric();
 
// params 用の配列が作られる。
var paramsArray = new object[] { tmp1, tmp2, tmp3, tmp4 };
 
// こういう文字列リテラルもプログラム中に埋め込まれて {0} とかの部分が無駄と言えば無駄。
var format = "{0}, {1}, {3}, {4}";
 
// これも必要性の有無にかかわらず必ず string 生成。
var message = string.Format(format, paramsArray);
 
// 作ったはいいけど、 Log の中で、LogLevel 的に使われない。
Log(message);
```

[`IFormattable` で受け取ると `string` 生成は遅らせれる](../../../../study/csharp/start/st_string.md#FormattableString)仕様はあるんですが、
あんまりカスタマイズ性もなくて、ボックス化とか `params` 同様の配列の生成は避けれません。

## <a id="csharp-next">提案仕様</a>

ということで、以下のように「特定パターンを満たす builder を作って、それの `TryFormat` メソッドを1個1個呼ぶ」みたいな形に展開できるようにしたいそうです。

```csharp
Builder.GetInterpolatedStringBuilder(baseLength: 6, formatHoleCount: 4, out var builder);
_ = builder.TryFormat(DiagnosticMetric())
    && builder.TryFormat(", ")
    && builder.TryFormat(DiagnosticMetric())
    && builder.TryFormat(", ")
    && builder.TryFormat(DiagnosticMetric())
    && builder.TryFormat(", ")
    && builder.TryFormat(DiagnosticMetric())
    ;
```

`&&` でつないでいるので、1個目で `false` を返せばもう2個目以降は呼ばれないという実装。
`TryFormat` にちゃんとしたオーバーロードを増やせば「`object` を介するせいでボックス化」も避けれます。

「ログレベルに応じて即 `false` を返す」みたいなのも、以下のような実装でできるようにしたいみたいです。

まず、`Logger` 自体の定義。
`LogTrace` メソッドの引数を「特定パターンを満たす builder」にします(この例の場合 `TraceLoggerParamsBuilder` 型)。

```csharp
public class Logger
{
    // どこかで設定
    public LogLevel EnabledLevel;
 
    // TraceLoggerParamsBuilder の作りは後述。TryFormat とかを持ってる型
    public void LogTrace(TraceLoggerParamsBuilder builder)
    {
        // TraceLoggerParamsBuilder から文字列を取り出してログ取りする。
    }
}
```

これで、以下のようなコードを書いたとして、

```csharp
Logger logger = GetLogger(LogLevel.Info);
logger.LogTrace($"{"this"} will never be printed because info is < trace!");
```

`logger.LogTrace` の行は以下のように展開するそうです。

```csharp
var receiverTemp = logger;
TraceLoggerParamsBuilder.GetInterpolatedStringBuilder(baseLength: 47, formatHoleCount: 1, receiverTemp, out var builder);
_ = builder.TryFormat("this") && builder.TryFormat(" will never be printed because info is < trace!");
receiverTemp.LogTrace(builder);
```

ログレベルを伝搬できるように、`Logger` のインスタンスも `GetInterpolatedStringBuilder` メソッド(builder のファクトリメソッド)に渡せるようにするとのこと。

`TraceLoggerParamsBuilder` 型は最低ライン以下のように作ります。

```csharp
public struct TraceLoggerParamsBuilder
{
    bool _logLevelEnabled;
 
    internal static void GetInterpolatedStringBuilder(int baseLength, int formatHoleCount, Logger logger, out TraceLoggerParamsBuilder builder)
    {
        // 実際は baseLength, formatHoleCount とかも使って初期サイズを決定したバッファーとかも作る想定。
        // とりあえず「レベルが合わないログは無視」のためのコードのみ例示。
        builder = new TraceLoggerParamsBuilder { _logLevelEnabled = logger.EnabledLevel <= LogLevel.Trace };
    }
 
    public bool TryFormat(string message)
    {
        if (!_logLevelEnabled) return false;
 
        // バッファーへの文字列書き込み
 
        return true;
    }
}
```

### <a id="overload">オーバーロード解決</a>

`$""` を渡すときに限り、`string` のオーバーロードよりも、「特定のパターンを満たす builder」型の方の優先度を高くするそうです。
しかも、`$""` がリテラルに展開されい場合だけ。
以下のような挙動になります。

```csharp
void Log(string s) { ... }
void Log(TraceLoggerParamsBuilder p) { ... }
 
Log($"test"); // {} を含んでないので $ が付かない "test"と同じ扱い → Log(string) の方が呼ばれる
Log($"{"test"}"); // {} の中身が文字列定数なのでコンパイル時に "test" に展開される → Log(string)
Log($"{1}"); // コンパイル時の展開が利かない文字列補間 → Log(TraceLoggerParamsBuilder) 扱いで TryFormat に展開
```

### <a id="InterpolatedStringBuilder">InterpolatedStringBuilder</a>

[`Span<T>`](../../../../study/csharp/resource/span.md) と [`ArrayPool`](https://github.com/dotnet/runtime/blob/79ae74f5ca5c8a6fe3a48935e85bd7374959c570/src/libraries/System.Private.CoreLib/src/System/Buffers/ArrayPool.cs) ベースでパフォーマンスが出るように作った builder を標準提供したいそうです。

現状、`InterpolatedStringBuilder` という名前で提案されています。

で、`string.Format` にも以下のオーバーロードを追加。

```csharp
public class String
{
    public static string Format(InterpolatedStringBuilder builder) => builder.ToString();
}
```

これで、通常の `var s = $"{x}, {y}";` みたいな string interpolation も `InterpolatedStringBuilder` に対する `TryFormat` に展開されるようになるとのこと。

### <a id="other">その他、考慮する点</a>

その他、以下のような話も。

- builder 自体、キャッシュしたインスタンスを使いまわすことを考慮してコンストラクターにはしない (`GetInterpolatedStringBuilder` メソッドを介する)
- `bool TryFormat` だけじゃなくて `void Format` も認めるかどうか
- `stackalloc` を使ってバッファーでもヒープ アロケーションを完全になくす案
- `Utf8Formatter` みたいにそもそも書き込み先を `Span<byte>` にする案
- パフォーマンスを考えると builder は [`ref struct`](../../../../study/csharp/resource/refstruct.md) になるはずで、だったら非同期メソッド内での利用に制限がかかりそう

## <a id="conclusion">まとめ</a>

文字列処理、やればやるほど「[`StringBuilder.Append` 直呼びするしかない…](https://github.com/ufcpp/StringLiteralGenerator/blob/master/src/StringLiteralGenerator/Utf8StringLiteralGenerator.cs)」みたいな気持ちになることが多々あるんですが、それがだいぶ解消されそうです。

そこそこ複雑な仕様になっていますが、
現状の [`ILogger`](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger.log?WT.mc_id=DT-MVP-4028921&view=dotnet-plat-ext-5.0) の `Log` メソッドの実装のしにくさを考えるとだいぶマシかなという感じ。
