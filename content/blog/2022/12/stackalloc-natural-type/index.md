---
title: "stackalloc の自然な型"
source_url: "https://ufcpp.net/blog/2022/12/stackalloc-natural-type/"
content_type: "BlogEntry"
published_at: "2022-12-05T22:24:10"
updated_at: "2022-12-05T22:24:10"
tags: []
umbraco_id: 2442
parent_id: 2438
sort_order: 1
aliases: []
---

# stackalloc の自然な型

今日は
`stackalloc T[N]` と `(stackalloc T[N])` に差があるとか、
`(stackalloc T[N]).M()` が許されるとか、
そんな感じの話。

## <a id="natural-type">ターゲット型推論と自然な型</a>

C# の文法の中には、「基本的にはターゲットを見て型決定するけども、別にターゲットがなくても型決定できる」ような文法がいくつかあります。
例えば整数リテラルがそうなんですが、以下のように、ターゲット(左辺)の型が決まっていても決まっていなくても大丈夫です。

<pre class="source" title="ターゲットからの型推論もできるし、推論できなかった時の自然な型も決まってる">
<span class="comment">// ターゲット(左辺)の型に合わせて「100」の型を決めてる。</span>
<span class="reserved">byte</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">100</span>;
<span class="reserved">short</span> <span class="variable">y</span> <span class="operator">=</span> <span class="number">100</span>;
<span class="reserved">int</span> <span class="variable">z</span> <span class="operator">=</span> <span class="number">100</span>;

<span class="comment">// 一方で、var だとターゲットからは型決定できない。</span>
<span class="comment">// そういう場合の 100 は int になる。</span>
<span class="reserved">var</span> <span class="variable">v</span> <span class="operator">=</span> <span class="number">100</span>;
</pre>

ちなみに、`var v = 100;` みたいに「普段ターゲットから型を決めている式が、決めれないときにデフォルトで何の型になるか」を指して「自然な型」(natural type)と言います。
上述の場合、「整数リテラルの自然な型は `int` 」ということになります。

他だと、補間文字列リテラルも「ターゲット型推論 + 自然な型持ち」です。

<pre class="source" title="補間文字列の自然な型">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">100</span>;

<span class="comment">// ターゲット(左辺)の型に合わせて「$&quot;abc{x}&quot;」の型を決めてる。</span>
<span class="reserved">string</span> <span class="variable">s</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">abc</span>{<span class="variable">x</span>}<span class="string">&quot;</span>;
<span class="type">IFormattable</span> <span class="variable">f</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">abc</span>{<span class="variable">x</span>}<span class="string">&quot;</span>;
<span class="type struct">DefaultInterpolatedStringHandler</span> <span class="variable">h</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">abc</span>{<span class="variable">x</span>}<span class="string">&quot;</span>;

<span class="comment">// 一方で、こちらはターゲットからは型決定できない。</span>
<span class="comment">// そういう場合の $&quot;abc{x}&quot; は string になる。</span>
<span class="reserved">var</span> <span class="variable">v</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">abc</span>{<span class="variable">x</span>}<span class="string">&quot;</span>;
</pre>

## <a id="stackalloc">stackalloc</a>

`stackalloc` は元々 [`unsafe`](../../../../study/csharp/interop/sp_unsafe.md) 限定機能で、
当然利用者も少ない機能でした。

ところが C# 7.2 で [`Span<T>` 構造体](../../../../study/csharp/resource/span.md)とか[安全な `stackalloc`](../../../../study/csharp/interop/sp_unsafe.md#safe-stackalloc)とか、
安全性を犠牲にせずにパフォーマンスを向上させれる文法が追加されて、
利用範囲が急に広がりました。

そして、安全な `stackalloc` の方が後入りなのもあって、`stackalloc` の自然な型はポインターのままです。

<pre class="source" title="stackalloc は基本的にはポインター">
<span class="reserved">unsafe</span>
{
    <span class="comment">// stackalloc の昔からの用法。</span>
    <span class="comment">// 元々がこういう文法なので、 stackalloc の結果は T* (ポインター)。</span>
    <span class="reserved">int</span><span class="operator">*</span> <span class="variable">i1</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>];

    <span class="comment">// 型推論でも T* 扱い。</span>
    <span class="comment">// ↓の i2 は int* になる。</span>
    <span class="reserved">var</span> <span class="variable">i2</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>];
}

<span class="comment">// C# 7.2 から</span>
<span class="comment">// ターゲットが Span のときに限り、safe コンテキストで stackalloc が使える。</span>
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>];

<span class="comment">// ところが、stackalloc の自然な型はポインターのまま。</span>
<span class="comment">// 以下の行は「safe コンテキストでポインターは使えません」エラー。</span>
<span class="reserved">var</span> <span class="variable">p</span> <span class="operator">=</span> <span class="error" title="CS0214"><span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]</span>;
</pre>

その後、C# 8.0 で、式の途中に `stackalloc` を書けるようになりました。
(C# 8.0 未満では、ここまで上げてきた例のように、変数に直接代入する場所にしか書けませんでした。)

<pre class="source" title="式の途中で stackalloc">
<span class="comment">// C# 8.0 未満でも書けた書き方:</span>
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>];

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">s</span>) { }

<span class="comment">// こういう書き方は C# 8.0 以降でだけ書ける。</span>
<span class="method"><span class="static">M</span></span>(<span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]);
</pre>

こういう歴史的な流れから、現状の `stackalloc` がどうなっているかというと…

## <a id="stackalloc-anywhere">式の途中の stackalloc</a>

C# 8.0 のとき、「式の途中に `stackalloc` を書いた場合に限り、自然な型を `Span<T>` にする」という決定をしていたりします。

例えば、以下のようなコードを書くと、`M(int*)` と `M(Span<int>)` の呼び分けが掛かります。

<pre class="source" title="式の途中かどうかで自然な型が違う stackalloc">
<span class="reserved">unsafe</span>
{
    <span class="comment">// こちらは昔ながらの型決定で、 stackalloc の自然な型はポインター。</span>
    <span class="reserved">var</span> <span class="variable">p</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]; <span class="comment">// int* 扱い。</span>
    <span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">p</span>); <span class="comment">// M(int*) の方が呼ばれる。</span>

    <span class="comment">// こちらは「式の途中」ということで、C# 8.0 以降のルールで、自然な型が Span&lt;T&gt; に。</span>
    <span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]); <span class="comment">// M(Span&lt;int&gt;) の方が呼ばれる。(なので実は unsafe 不要。)</span>
}

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">int</span><span class="operator">*</span> <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

で、この「式の途中なら `Span<T>`」な仕様を使うと、以下のようなこともできたりします。

* `var` + `stackalloc` の自然な型を `Span<T>` にする
* `stackalloc` に対して拡張メソッドを呼ぶ

### <a id="natural-type-span">var + stackalloc を Span に</a>

式の途中なら自然な型が `Span<T>` になるということは…
実は `()` の有無で自然な型を変えれます。
`()` を付ければ safe。

<pre class="source" title="() を付ければ safe">
<span class="comment">// 前述のとおり、自然な型が int* で、unsafe 必須。</span>
<span class="comment">// (今は unsafe を付けていないのでコンパイル エラー。)</span>
<span class="reserved">var</span> <span class="variable">p</span> <span class="operator">=</span> <span class="error" title="CS0214"><span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]</span>;

<span class="comment">// こっちは自然な型が Span&lt;int&gt;。</span>
<span class="comment">// var に対して使っても Span&lt;int&gt; になるので safe。</span>
<span class="reserved">var</span> <span class="variable">s</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]);
</pre>

そしてまあ、型推論推進派(左辺と右辺で2度同じ型名を書きたくない)にとっては、
安全な `stackalloc` を使いつつも型推論を掛けるための回避策になります。

<pre class="source" title="左右に同じ型名を2度も書きたくない">
<span class="comment">// こう書いてもいいけども…</span>
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">s1</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>];

<span class="comment">// こっちの方が短いという。</span>
<span class="reserved">var</span> <span class="variable">s2</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>]);

<span class="comment">// まして、型名が長いときは… だいぶ差が大きい。</span>
<span class="type struct">Span</span>&lt;<span class="type struct">LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm</span>&gt; <span class="variable">s3</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="type struct">LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm</span>[<span class="number">4</span>];
<span class="reserved">var</span> <span class="variable">s4</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="type struct">LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm</span>[<span class="number">4</span>]);

<span class="reserved">struct</span> <span class="type struct">LongLongStructName1234567890qwertyuiopasdfghjklzxcvbnm</span> { }
</pre>

### <a id="extension-method">stackalloc に対して拡張メソッドを呼ぶ</a>

そして、拡張メソッドも呼べるみたいですよ。

<pre class="source" title="(stackalloc) なら拡張メソッドも呼べる">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">4</span>])<span class="operator">.</span><span class="method">M</span>(<span class="number">123</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">C</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="type param">T</span>&gt; <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>, <span class="type param">T</span> <span class="variable local">value</span>)
    {
        <span class="variable local">span</span><span class="operator">.</span><span class="method">Fill</span>(<span class="variable local">value</span>);
        <span class="control">return</span> <span class="variable local">span</span>;
    }
}
</pre>

できる気はしていたものの、ほんとにできた…

というか、以下のようなコードを書いててふと思いつき。

<pre class="source" title="&quot;&quot;u8 拡張メソッド">
<span class="reserved">using</span> System<span class="operator">.</span>Text;

<span class="comment">// u8 リテラルの自然な型は ReadOnlySpan&lt;byte&gt; だったはず。</span>
<span class="comment">// なら拡張メソッド M も呼べるはず。</span>
<span class="string">&quot;abcあいう&quot;<span class="reserved">u8</span></span><span class="operator">.</span><span class="method">M</span>();

<span class="comment">// そういや stackalloc にも自然な型あるはずよな…?</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">C</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">span</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">span</span>)
        {
            <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">x</span>:<span class="string">X2</span>}<span class="string"> </span><span class="string">&quot;</span>);
        }
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>();
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="type">Encoding</span><span class="operator">.</span><span class="property"><span class="static">UTF8</span></span><span class="operator">.</span><span class="method">GetString</span>(<span class="variable local">span</span>));
    }
}
</pre>

ちなみに、拡張メソッド解決の仕様的に、以下のようなコードだとダメ(コンパイル エラー)だったりします。
`Span<T>` から `ReadOnlySpan<T>` への暗黙の型変換は、拡張メソッド解決の際には使われません。

<pre class="source" title="ReadOnlySpan の拡張メソッドに対しては使えない">
<span class="reserved">using</span> System<span class="operator">.</span>Text;

<span class="comment">// これは呼べない。</span>
<span class="comment">// Span&lt;byte&gt; → ReadOnlySpan&lt;byte&gt; には暗黙の型変換があるものの、</span>
<span class="comment">// 拡張メソッド解決の際に暗黙の型変換を挟むことは許容していない。</span>
(<span class="error" title="CS1929"><span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="number">4</span>]</span>)<span class="operator">.</span>M();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">C</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">span</span>)
    {
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">span</span>)
        {
            <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">x</span>:<span class="string">X2</span>}<span class="string"> </span><span class="string">&quot;</span>);
        }
        <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>();
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="type">Encoding</span><span class="operator">.</span><span class="static"><span class="property">UTF8</span></span><span class="operator">.</span><span class="method">GetString</span>(<span class="variable local">span</span>));
    }
}
</pre>
