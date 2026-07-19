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

<pre class="source" title="string interpolation の例">
<code><span class="reserved">var</span> <span class="variable">s</span> = <span class="string">$&quot;(</span>{<span class="variable">a</span>}<span class="string">, </span>{<span class="variable">b</span>}<span class="string">)&quot;</span>;
</code></pre>

これは、以下のように展開されます。

<pre class="source" title="上記コードの展開結果">
<code><span class="reserved">var</span> <span class="variable">s</span> = <span class="reserved">string</span>.<span class="method">Format</span>(<span class="string">&quot;({0}, {1})&quot;</span>, <span class="variable">a</span>, <span class="variable">b</span>);
</code></pre>

これがパフォーマンス的にあんまりよろしくなくて…

特に、[冒頭の提案ドキュメント](../../../../study/csharp/start/st_string.md#string-interpolation)にもある通り、ロギング用途との相性が最悪で、
[`ILogger`](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger.log?WT.mc_id=DT-MVP-4028921&view=dotnet-plat-ext-5.0)のメソッドがなかなか使いにくそうな感じの引数になっています。

<pre class="source" title="ILogger.Log メソッドの引数が意味不明な件">
<code><span class="reserved">void</span> <span class="method">Log</span>&lt;<span class="type">TState</span>&gt;(<span class="type">LogLevel</span> <span class="variable">logLevel</span>, <span class="type">EventId</span> <span class="variable">eventId</span>, <span class="type">TState</span> <span class="variable">state</span>, <span class="type">Exception</span> <span class="variable">exception</span>, <span class="type">Func</span>&lt;<span class="type">TState</span>, <span class="type">Exception</span>, <span class="reserved">string</span>&gt; <span class="variable">formatter</span>);
</code></pre>

「`formatter` でラムダ式を渡して、その中で文字列化」みたいなことをしないといけなくて、結構面倒です。

もちろんこのまま使うのは大変なので `LogDebug` とか `LogTrace` とかの拡張メソッドには素直に `string` を引数として受け取るオーバーロードもあったりするんですが、
それがまた罠というか、パフォーマンスにシビアな場面で使ってしまうと露骨に遅くなるという問題が。

遅くなる原因はいくつかあって、

- 引数(上記の例でいうと `a` と `b`)を `object` で受け取ってしまう。引数が値型の時に[ボックス化](../../../../study/csharp/resource/rmboxing.md)を起こす
- 引数の数が多いと [`params`](../../../../study/csharp/structured/sp_params.md#params) 扱いになって配列の確保も起きる
- 即時評価なので、実際には不要なものも(ログレベル的に無視する文字列であっても)必ず文字列化される
- [`Span<T>`](../../../../study/csharp/resource/span.md) みたいな、C# 7.2 以降、パフォーマンスが重要な場面で多用することになった型を使えない

例えば以下のようなコード(一部仮想コードですが)があった場合、

<pre class="source" title="呼ばれ方としてまずいロギング処理">
<code><span class="reserved">using</span> System;
 
<span class="method">Log</span>(<span class="string">$&quot;</span>{<span class="method">DiagnosticMetric</span>()}<span class="string">, </span>{<span class="method">DiagnosticMetric</span>()}<span class="string">, </span>{<span class="method">DiagnosticMetric</span>()}<span class="string">, </span>{<span class="method">DiagnosticMetric</span>()}<span class="string">&quot;</span>);
 
<span class="reserved">string</span> <span class="method">DiagnosticMetric</span>()
{
    <span class="comment">// 診断専用で、日常的に読むには少々重たい値がなにかあるとして</span>
    <span class="control">return</span> その値を返す;
}
 
<span class="reserved">void</span> <span class="method">Log</span>(<span class="reserved">string</span> <span class="variable">message</span>)
{
    <span class="comment">// LogLevel はコンパイル時に確定しない設定ファイルとかから読んだりする想定で</span>
    <span class="control">if</span> (LogLevel &lt; 1) <span class="control">return</span>;
 
    <span class="comment">// もし、たいていの場面では LogLevel 0 で運用してるとここにはほとんど来ない。</span>
    <span class="comment">// 実際には message を読む必要がない。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">message</span>);
}
</code></pre>

以下のように展開されて処理されます。

<pre class="source" title="string interpolation の展開結果">
<code><span class="comment">// ただでさえ「必要な時にだけ呼びたい」というつもりのメソッドが無条件に呼ばれる。</span>
<span class="reserved">object</span> <span class="variable">tmp1</span> = <span class="method">DiagnosticMetric</span>(); <span class="comment">// int → object に代入しててボックス化。</span>
<span class="reserved">object</span> <span class="variable">tmp2</span> = <span class="method">DiagnosticMetric</span>();
<span class="reserved">object</span> <span class="variable">tmp3</span> = <span class="method">DiagnosticMetric</span>();
<span class="reserved">object</span> <span class="variable">tmp4</span> = <span class="method">DiagnosticMetric</span>();
 
<span class="comment">// params 用の配列が作られる。</span>
<span class="reserved">var</span> <span class="variable">paramsArray</span> = <span class="reserved">new</span> <span class="reserved">object</span>[] { <span class="variable">tmp1</span>, <span class="variable">tmp2</span>, <span class="variable">tmp3</span>, <span class="variable">tmp4</span> };
 
<span class="comment">// こういう文字列リテラルもプログラム中に埋め込まれて {0} とかの部分が無駄と言えば無駄。</span>
<span class="reserved">var</span> <span class="variable">format</span> = <span class="string">&quot;{0}, {1}, {3}, {4}&quot;</span>;
 
<span class="comment">// これも必要性の有無にかかわらず必ず string 生成。</span>
<span class="reserved">var</span> <span class="variable">message</span> = <span class="reserved">string</span>.<span class="method">Format</span>(<span class="variable">format</span>, <span class="variable">paramsArray</span>);
 
<span class="comment">// 作ったはいいけど、 Log の中で、LogLevel 的に使われない。</span>
<span class="method">Log</span>(<span class="variable">message</span>);
</code></pre>

[`IFormattable` で受け取ると `string` 生成は遅らせれる](../../../../study/csharp/start/st_string.md#FormattableString)仕様はあるんですが、
あんまりカスタマイズ性もなくて、ボックス化とか `params` 同様の配列の生成は避けれません。

## <a id="csharp-next">提案仕様</a>

ということで、以下のように「特定パターンを満たす builder を作って、それの `TryFormat` メソッドを1個1個呼ぶ」みたいな形に展開できるようにしたいそうです。

<pre class="source" title="builder.TryFormat に展開">
<code><span class="type">Builder</span>.<span class="method">GetInterpolatedStringBuilder</span>(<span class="variable">baseLength</span>: 6, <span class="variable">formatHoleCount</span>: 4, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">builder</span>);
<span class="reserved">_</span> = <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="method">DiagnosticMetric</span>())
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="string">&quot;, &quot;</span>)
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="method">DiagnosticMetric</span>())
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="string">&quot;, &quot;</span>)
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="method">DiagnosticMetric</span>())
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="string">&quot;, &quot;</span>)
    &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="method">DiagnosticMetric</span>())
    ;
</code></pre>

`&&` でつないでいるので、1個目で `false` を返せばもう2個目以降は呼ばれないという実装。
`TryFormat` にちゃんとしたオーバーロードを増やせば「`object` を介するせいでボックス化」も避けれます。

「ログレベルに応じて即 `false` を返す」みたいなのも、以下のような実装でできるようにしたいみたいです。

まず、`Logger` 自体の定義。
`LogTrace` メソッドの引数を「特定パターンを満たす builder」にします(この例の場合 `TraceLoggerParamsBuilder` 型)。

<pre class="source" title="想定している Logger の作り方">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Logger</span>
{
    <span class="comment">// どこかで設定</span>
    <span class="reserved">public</span> <span class="type">LogLevel</span> EnabledLevel;
 
    <span class="comment">// TraceLoggerParamsBuilder の作りは後述。TryFormat とかを持ってる型</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">LogTrace</span>(<span class="type">TraceLoggerParamsBuilder</span> <span class="variable">builder</span>)
    {
        <span class="comment">// TraceLoggerParamsBuilder から文字列を取り出してログ取りする。</span>
    }
}
</code></pre>

これで、以下のようなコードを書いたとして、

<pre class="source" title="logger.LogTrace 利用例">
<code><span class="type">Logger</span> <span class="variable">logger</span> = GetLogger(<span class="type">LogLevel</span>.Info);
<span class="variable">logger</span>.LogTrace(<span class="string">$&quot;</span>{<span class="string">&quot;this&quot;</span>}<span class="string"> will never be printed because info is &lt; trace!&quot;</span>);
</code></pre>

`logger.LogTrace` の行は以下のように展開するそうです。

<pre class="source" title="logger.LogTrace の展開結果">
<code><span class="reserved">var</span> <span class="variable">receiverTemp</span> = <span class="variable">logger</span>;
<span class="type">TraceLoggerParamsBuilder</span>.<span class="method">GetInterpolatedStringBuilder</span>(<span class="variable">baseLength</span>: 47, <span class="variable">formatHoleCount</span>: 1, <span class="variable">receiverTemp</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">builder</span>);
<span class="reserved">_</span> = <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="string">&quot;this&quot;</span>) &amp;&amp; <span class="variable">builder</span>.<span class="method">TryFormat</span>(<span class="string">&quot; will never be printed because info is &lt; trace!&quot;</span>);
<span class="variable">receiverTemp</span>.<span class="method">LogTrace</span>(<span class="variable">builder</span>);
</code></pre>

ログレベルを伝搬できるように、`Logger` のインスタンスも `GetInterpolatedStringBuilder` メソッド(builder のファクトリメソッド)に渡せるようにするとのこと。

`TraceLoggerParamsBuilder` 型は最低ライン以下のように作ります。

<pre class="source" title="ログレベルに応じて必要な時だけ文字列書き込みする builder の例">
<code><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">TraceLoggerParamsBuilder</span>
{
    <span class="reserved">bool</span> _logLevelEnabled;
 
    <span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">GetInterpolatedStringBuilder</span>(<span class="reserved">int</span> <span class="variable">baseLength</span>, <span class="reserved">int</span> <span class="variable">formatHoleCount</span>, <span class="type">Logger</span> <span class="variable">logger</span>, <span class="reserved">out</span> <span class="type">TraceLoggerParamsBuilder</span> <span class="variable">builder</span>)
    {
        <span class="comment">// 実際は baseLength, formatHoleCount とかも使って初期サイズを決定したバッファーとかも作る想定。</span>
        <span class="comment">// とりあえず「レベルが合わないログは無視」のためのコードのみ例示。</span>
        <span class="variable">builder</span> = <span class="reserved">new</span> <span class="type">TraceLoggerParamsBuilder</span> { _logLevelEnabled = <span class="variable">logger</span>.EnabledLevel &lt;= <span class="type">LogLevel</span>.Trace };
    }
 
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">TryFormat</span>(<span class="reserved">string</span> <span class="variable">message</span>)
    {
        <span class="control">if</span> (!_logLevelEnabled) <span class="control">return</span> <span class="reserved">false</span>;
 
        <span class="comment">// バッファーへの文字列書き込み</span>
 
        <span class="control">return</span> <span class="reserved">true</span>;
    }
}
</code></pre>

### <a id="overload">オーバーロード解決</a>

`$""` を渡すときに限り、`string` のオーバーロードよりも、「特定のパターンを満たす builder」型の方の優先度を高くするそうです。
しかも、`$""` がリテラルに展開されい場合だけ。
以下のような挙動になります。

<pre class="source" title="オーバーロード解決">
<code><span class="reserved">void</span> <span class="method">Log</span>(<span class="reserved">string</span> <span class="variable">s</span>) { ... }
<span class="reserved">void</span> <span class="method">Log</span>(<span class="type">TraceLoggerParamsBuilder</span> <span class="variable">p</span>) { ... }
 
<span class="method">Log</span>(<span class="string">$&quot;test&quot;</span>); <span class="comment">// {} を含んでないので $ が付かない &quot;test&quot;と同じ扱い → Log(string) の方が呼ばれる</span>
<span class="method">Log</span>(<span class="string">$&quot;</span>{<span class="string">&quot;test&quot;</span>}<span class="string">&quot;</span>); <span class="comment">// {} の中身が文字列定数なのでコンパイル時に &quot;test&quot; に展開される → Log(string)</span>
<span class="method">Log</span>(<span class="string">$&quot;</span>{1}<span class="string">&quot;</span>); <span class="comment">// コンパイル時の展開が利かない文字列補間 → Log(TraceLoggerParamsBuilder) 扱いで TryFormat に展開</span>
</code></pre>

### <a id="InterpolatedStringBuilder">InterpolatedStringBuilder</a>

[`Span<T>`](../../../../study/csharp/resource/span.md) と [`ArrayPool`](https://github.com/dotnet/runtime/blob/79ae74f5ca5c8a6fe3a48935e85bd7374959c570/src/libraries/System.Private.CoreLib/src/System/Buffers/ArrayPool.cs) ベースでパフォーマンスが出るように作った builder を標準提供したいそうです。

現状、`InterpolatedStringBuilder` という名前で提案されています。

で、`string.Format` にも以下のオーバーロードを追加。

<pre class="source" title="string.Format(InterpolatedStringBuilder)">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">String</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="method">Format</span>(<span class="type">InterpolatedStringBuilder</span> <span class="variable">builder</span>) =&gt; <span class="variable">builder</span>.<span class="method">ToString</span>();
}
</code></pre>

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
