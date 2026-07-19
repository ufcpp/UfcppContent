---
title: "Visual Studio 16.1.0 & 16.2 Preview 1"
source_url: "https://ufcpp.net/blog/2019/5/vs16_2_p1/"
content_type: "BlogEntry"
published_at: "2019-05-22T20:35:13"
updated_at: "2019-05-23T11:41:56"
tags: []
umbraco_id: 2244
parent_id: 2241
sort_order: 2
aliases: []
---

# Visual Studio 16.1.0 & 16.2 Preview 1

Visual Studio 16.1 のリリースと、16.2 の Preview 1 が来ていますね。

- [Visual Studio 2019 version 16.1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes#16.1.0)
- [Visual Studio 2019 version 16.2 Preview 1](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.2.0-pre.1.0)

## 16.1

16.1 の方は、[こないだの Preview 3](../build2019/index.md)からそんなに変わってなくて、割かし「リリースされました」という感じ。

[C# 8.0](../../../../study/csharp/cheatsheet/ap_ver8.md) 的には、

- [Ranges](../../../2018/12/cs8ranges/index.md)は たぶん、今の挙動で確定
- [`switch`式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)
  - 優先度が `+` よりも上になってる
  - [Target-typed switch](https://github.com/dotnet/csharplang/issues/2389) は 8.0 候補になってて、かつ、未実装
- [非同期イテレーター](../../../2018/12/cs8asyncstreams/index.md)
  - `EnumeratorCancellation ` 属性を付けた引数に `CancellationToken` が渡るように
  - 変更が確定していて、16.2 Preview 1 ですでに挙動が違う(後述)
- [インターフェイスのデフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)も 16.2 Preview 1 で変更あり(後述)
- [null 許容参照型](../../../2018/12/cs8nrt/index.md)は相変わらず作業真っ最中

という感じ。

## 16.2 Preview 1

IDE 的には以下のようなものが増えてるみたいです。

- プロジェクトの新規作成で、作成したいアプリのタイプで検索できるように
- テスト エクスプローラーがだいぶ見やすく

あと、Developer PowerShell (開発ツールがらみの環境変数とかパスが通った状態の PowerShell)が追加されたみたいです。
これまでもあった [Developer Command Prompt](https://docs.microsoft.com/ja-jp/dotnet/framework/tools/developer-command-prompt-for-vs) の PowerShell 版。

<em>ちょっと C# コンパイラーに致命的なバグがありそうなので注意</em>。
[安全な `stackalloc`](../../../../study/csharp/resource/span.md#safe-stackalloc)を使うと不正なコードを生成して、プログラムが起動できなくなります。
(正確に言うと、`stackalloc` を書いたメソッドを呼んだ瞬間、`InvalidProgram`例外発生。)
修正済みっぽいんですけど、16.2 Preview 1 には反映されていない状態。

- [Invalid IL generated for stackalloc assigned to Span #35764](https://github.com/dotnet/roslyn/issues/35764)

C# 8.0 的には、16.1 の方と以下の差があります。

- [非同期イテレーター](../../../2018/12/cs8asyncstreams/index.md)の`EnumeratorCancellation ` 属性の仕様変更
- [`base(T)`](../../../../study/csharp/oop/oo_inherit.md#non-virtual-base-access) 削除
- [stackalloc in nested expressions](https://github.com/dotnet/csharplang/issues/1412) の追加

あと、null許容参照型をプロジェクト単位で有効化するための設定も、`<Nullable>enable</Nullable>` に変わっています(16.1 までは `NullableContextOptions`)。

### 非同期イテレーターの仕様変更

`X(ct1).WithCancellation(ct2)` みたいなのを書いたときの挙動が変わります。

```csharp
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
 
class Program
{
    static async Task Main()
    {
        var c1 = new CancellationTokenSource();
        var c2 = new CancellationTokenSource();
 
        // キャンセルなし
        await foreach (var x in X()) ;
 
        // AscynEnumerable 生成時に c1 が渡る
        await foreach (var x in X(c1.Token)) ;
 
        // GetAsyncEnumerator 時に c2 が渡る
        await foreach (var x in X().WithCancellation(c2.Token)) ;
 
        // 旧挙動: c2 だけが渡る
        // 新挙動: c1, c2 の両方が渡る。内部で CreateLinkedTokenSource
        await foreach (var x in X(c1.Token).WithCancellation(c2.Token)) ;
    }
 
    // 新挙動: EnumeratorCancellation 属性付きの引数は1個に限る
    static async IAsyncEnumerable<int> X([EnumeratorCancellation]CancellationToken ct = default)
    {
        await Task.Yield();
        yield break;
    }
}
```

### base(T) 削除

[base(T) アクセス](../../../../study/csharp/oop/oo_inherit.md#non-virtual-base-access)、いったん取りやめになりました。
(書いた記事どうしよう… 消すか、「今後入る予定です」に変えるか…)

C# コンパイラーだけでできる実装方法だと不満だそうで、 .NET Core ランタイム側も合せて修正変更したいそうです。
結果的に C# 8.0 には間に合わず、ランタイム修正ありなものをマイナー リリースするとは思えないので 9.0 以降での実装になります。

### stackalloc in nested expressions

式のど真ん中に `stackalloc` を書けるようになりました。

```csharp
using System;
using System.Threading.Tasks;
 
class Program
{
    static int M(Span<int> span) => 0;
 
    static void Main()
    {
        // 引数にも書けたり
        M(stackalloc int[1]);
 
        // 式のどこにでも書ける
        if (stackalloc int[1] == stackalloc int[1]) { }
    }
 
    // フィールド初期化子内にも書けたり
    int x = M(stackalloc int[1]);
 
    static async Task Async()
    {
        // 式中に書くなら、非同期メソッド内でも stackalloc が書ける
        M(stackalloc int[1]);
 
        await Task.Yield();
    }
}
```

ぶっちゃけ、[再帰パターン](../../../../study/csharp/cheatsheet/ap_ver8.md#recursive-pattern)のついでだそうです。
再帰パターンの導入で[参照として返せるものの判定](../../../../study/csharp/resource/sp_ref.md#flow-analysis)が複雑になったらしく、
ちゃんとした判定に書き換えたらついでに `stackalloc` を書ける場所も増えたとのこと。
