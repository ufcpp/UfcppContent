---
title: "ref 構造体のインターフェイス実装 / 型引数での使用"
source_url: "https://ufcpp.net/blog/2024/2/ref-struct-interface/"
content_type: "BlogEntry"
published_at: "2024-02-11T11:53:20"
updated_at: "2024-02-11T11:53:20"
tags: []
umbraco_id: 2483
parent_id: 2480
sort_order: 2
aliases: []
---

# ref 構造体のインターフェイス実装 / 型引数での使用

[ref 構造体](../../../../study/csharp/resource/refstruct.md)で説明しているように、
[`Span<T>`](../../../../study/csharp/resource/span.md) 型など一部の型は「スタック上にないといけない」という強い制約があります。

この制約を守るため、これまで、ref 構造体は

* インターフェイスを実装できなかった
* ジェネリック型引数に使えなかった

という制限が掛かっていました。

C# 13 では、この制限を緩和するため、
ジェネリック型引数に「`allows ref struct`」という「アンチ制約」を追加する予定です。

こういう案自体は [ref フィールド](../../../../study/csharp/resource/refstruct.md#ref-field)が追加された C# 11 (2022年)の頃から温められてはいたんですが、
いよいよ C# 13 で本格的に取り組むみたいです。
.NET 8/C# 12 がリリースされた後くらいからちらほら提案ドキュメントの更新あり。

* [Add draft for demonstrating ref-struct-constraint soundness](https://github.com/dotnet/csharplang/pull/7769)
* [Update ref struct interfaces based on LDM discussions](https://github.com/dotnet/csharplang/pull/7865)
* [ref struct interfaces updates](https://github.com/dotnet/csharplang/pull/7911)

ちなみに、ランタイム側はその2022年頃に対応すでに入っているみたいです。

* [Design to support ByRefLike types in Generics](https://github.com/dotnet/runtime/pull/67129)
* [Support ByRefLike types as Generic parameters](https://github.com/dotnet/runtime/pull/67783)

## ref 構造体の制限緩和の要求

わかりやすい例でいうと、`Span<T>` は `IEnumerable<T>` であってほしいというものです。
C# 12 時点だと、以下のような2重実装を余儀なくされています。

<pre class="source" title="C# 12 時点では IEnumerable と Span の2重実装が必須">
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="type">MyMath</span></span><span class="operator">.</span><span class="static"><span class="method">Sum</span></span>(<span class="variable">list</span>));
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="type"><span class="static">MyMath</span></span><span class="operator">.</span><span class="static"><span class="method">Sum</span></span>(<span class="variable">span</span>));

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">MyMath</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">Sum</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">numbers</span>)
    {
        <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="number">0</span>;
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">numbers</span>) <span class="variable">sum</span><span class="operator">+=</span> <span class="variable">x</span>;
        <span class="control">return</span> <span class="variable">sum</span>;
    }

    <span class="comment">// メソッドの中身全く同じ。</span>
    <span class="comment">// Span/ReadOnlySpan が IEnumerable じゃないので別メソッドでの実装が必須。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Sum</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">numbers</span>)
    {
        <span class="comment">// 実装的に、numbers をボックス化したり、ref フィールドを外に漏らしたりもしてない。</span>
        <span class="comment">// IEnumerable に対する実装をそのまま使って何も問題ない。</span>
        <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="number">0</span>;
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">numbers</span>) <span class="variable">sum</span> <span class="operator">+=</span> <span class="variable">x</span>;
        <span class="control">return</span> <span class="variable">sum</span>;
    }
}
</pre>

ref 構造体にインターフェイス実装を持たせること自体はそこまで問題ではありません。
問題は、以下のように、「インターフェイス型の変数に直接代入してしまうとボックス化を起こしてまずい」という点です。

<pre class="source" title="Span をインターフェイス型変数に代入しちゃダメ">
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];

<span class="comment">// たとえ、Span が IEnumerable&lt;T&gt; を実装していたとしても、</span>
<span class="comment">// 以下のようなコードを書くとこの時点でボックス化が起きる。</span>
<span class="comment">// span がヒープに漏れてしまうのでまずい。</span>
<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">e</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS0029">span</span></span>;
</pre>

じゃあどうすべきかというと、ジェネリクスを介します。

<pre class="source" title="ジェネリクスを介すればいい">
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];

<span class="comment">// ジェネリクスを介すれば、ボックス化を起こさずにインターフェイスのメンバーを呼べる。</span>
<span class="comment">// (前述の問題はクリア。)</span>
<span class="reserved">static</span> <span class="type param">T</span> <span class="method"><span class="static">Sum</span></span>&lt;<span class="type param">T</span>, <span class="type param">TEnumerable</span>&gt;(<span class="type param">TEnumerable</span> <span class="variable local">list</span>)
    <span class="reserved">where</span> <span class="type param">TEnumerable</span> : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// 省略</span>
    <span class="control">return</span> <span class="reserved">default</span><span class="operator">!</span>; <span class="comment">// 仮</span>
}

<span class="comment">// なので残る問題はこっち。</span>
<span class="comment">// ref 構造体を型引数に渡したい。</span>
<span class="error" title="CS0306"><span class="static"><span class="method">Sum</span></span>&lt;<span class="reserved">int</span>, <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt;</span>(<span class="variable">span</span>);
</pre>

ということで次節で説明する「アンチ制約」が必要になります。

## アンチ制約

ジェネリック型制約(`where T :` みたいなやつ)は、普通、制限を掛けることで、

* メソッド内で `T`に対して できること(呼べるメソッドとか)が増える
* その代わり、呼び出し側で `T` に対して渡せる型が減る

というものになります。

<pre class="source" title="型制約">
<span class="comment">// 制限なし。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>&lt;<span class="type param">T</span>&gt;() { }

<span class="comment">// 何の型でも渡せる。</span>
<span class="static"><span class="method">M1</span></span>&lt;<span class="reserved">int</span>&gt;();
<span class="static"><span class="method">M1</span></span>&lt;<span class="reserved">string</span>&gt;();
<span class="static"><span class="method">M1</span></span>&lt;<span class="reserved">object</span>&gt;();

<span class="comment">// 制限あり。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M2</span></span>&lt;<span class="type param">T</span>&gt;() <span class="reserved">where</span> <span class="type param">T</span>:<span class="type">ISpanParsable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// 呼べるメソッドが増える。</span>
    <span class="type param">T</span> <span class="variable">value</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="string">&quot;123&quot;</span>, <span class="reserved">null</span>);
}

<span class="comment">// 渡せる型が減る。</span>
<span class="static"><span class="method">M2</span></span>&lt;<span class="reserved">int</span>&gt;();
<span class="method"><span class="static">M2</span></span>&lt;<span class="reserved">string</span>&gt;();
<span class="error" title="CS0311"><span class="method"><span class="static">M2</span></span>&lt;<span class="reserved">object</span>&gt;</span>(); <span class="comment">// コンパイルエラー。</span>
</pre>

ところが今回、「ref 構造体を渡せるようにしたい」という逆の要件なので、「制約」ではなく「アンチ制約(制約の撤回)」が必要になります。

[2年くらい前のブログ](../../../2022/2/ref-generic-arguments/index.md)でちょこっと触れていますが、
逆のことをするのに `where T : ref struct` とは書かせたくないようで、ちょっと別文法を模索していました。
当初案だと `allow T : ref struct` とかも検討されていたんですが、
結局は `where T : allows ref struct` (where はそのまま。制約の前に allows)になりそうです。

<pre class="source" title="">
<span class="comment">// allows で制限を緩める。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M3</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">x</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="reserved">allows</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="comment">// アンチ制約。</span>
{
    <span class="comment">// メソッド内でできることが減る。</span>
    <span class="reserved">object</span> <span class="variable">obj</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// box 化ダメ。エラーにする予定。</span>
}

<span class="comment">// 渡せる型が増える。</span>
<span class="static"><span class="method">M3</span></span>&lt;<span class="reserved">int</span>&gt;();
<span class="static"><span class="method">M3</span></span>&lt;<span class="reserved">string</span>&gt;();
<span class="static"><span class="method">M3</span></span>&lt;<span class="reserved">object</span>&gt;();
<span class="static"><span class="method">M3</span></span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt;(); <span class="comment">// allows ref struct がないと呼べない。</span>
</pre>

ちなみに、`where T : IDisposable, allows ref struct` みたいに、制約とアンチ制約は並べて書けます。
