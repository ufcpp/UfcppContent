---
title: "params コレクション"
source_url: "https://ufcpp.net/blog/2024/3/params-collections/"
content_type: "BlogEntry"
published_at: "2024-03-02T17:44:41"
updated_at: "2024-03-02T17:44:41"
tags: []
umbraco_id: 2491
parent_id: 2490
sort_order: 0
aliases: []
---

# params コレクション

[ほぼ1年ぶり](../../../2023/2/params-ros/index.md)の [params](../../../../study/csharp/structured/sp_params.md#params) の話。

params を配列以外のコレクションに対して使えるようにするという話ですが、
雰囲気的に C# 13 でついに 入りそうです。
なので、最近そこそこ高頻度で Language Design Meeting の議題に上がっています。

* [Params Collection](https://github.com/dotnet/csharplang/issues/7700)
* C# Language Design Meeting
  * [November 15th, 2023](https://github.com/dotnet/csharplang/blob/main/meetings/2023/LDM-2023-11-15.md#params-improvements)
  * [January 29th, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-01-29.md#params-collections)
  * [January 31st, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-01-31.md#params-collections-evaluation-orders)
  * [February 21st, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md#params-collections)

まあ、割かしもう詳細を詰めている感じの話題が多めですね。

## params ‘コレクション’

去年には「`ReadOnlySpan<T>` 以外需要低め」、「他は[コレクション式](../../../../study/csharp/cheatsheet/ap_ver12.md#collection-expression)を使って `M([a, b, c])` でいいのでは」などという話も出ていましたが。
コレクション式を実装した今改めて検討して、
むしろ「コレクション式とそろえるのがいいのではないか」という感じに変わったみたいです。

<pre class="source" title="params コレクション(案)">
<span class="comment">// ReadOnlySpan を優先するようになる予定。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 今でも書ける。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">_</span>) { }

    <span class="comment">// 新規に書けるようになる予定。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved"><span class="error" title="CS0225">params</span></span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved"><span class="error" title="CS0225">params</span></span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

## params ‘ref struct’

params に配列以外の型を認めたいという話の前提には、パフォーマンスを改善したいという要求があります。
なので、`Span` や `ReadOnlySpan` をはじめとした [ref struct](../../../../study/csharp/resource/refstruct.md) を使いたいです(ref struct 自体がパフォーマンス改善のために導入された概念)。

で、ref struct にはスコープの概念があって、引数や変数を [`scoped`](../../../../study/csharp/resource/refstruct.md#scoped) で修飾するかどうかでちょっと挙動が変わります。

<pre class="source" title="scoped の有無">
<span class="static"><span class="method">M</span></span>(<span class="reserved">true</span>);

<span class="reserved">static</span> <span class="type struct">S</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">bool</span> <span class="variable local">b</span>)
{
    <span class="control">if</span>(<span class="variable local">b</span>)
    {
        <span class="comment">// [] が作る Span が S に伝搬してて、外に漏らせないので return に渡すとエラー。</span>
        <span class="control">return</span> <span class="error" title="CS8347"><span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">Unscoped</span></span>(<span class="error" title="CS9203">[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</span>)</span>;
    }
    <span class="control">else</span>
    {
        <span class="comment">// こちらは Span が伝搬しないので return できる。</span>
        <span class="control">return</span> <span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">Scoped</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
    }
}

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// span の寿命が S に伝搬する。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">S</span> <span class="static"><span class="method">Unscoped</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">span</span>);

    <span class="comment">// span の寿命を外に漏らさない。</span>
    <span class="comment">// なので、S に直接伝搬できない。</span>
    <span class="comment">// new(span.ToArray()) とかする必要がある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">S</span> <span class="static"><span class="method">Scoped</span></span>(<span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="error" title="CS8347"><span class="reserved">new</span>(<span class="error" title="CS8352"><span class="variable local">span</span></span>)</span>;
}

<span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>)
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">Span</span> <span class="operator">=</span> <span class="variable local">span</span>;
}
</pre>

で、ここに params をつけれるようになった場合にどうするかという話になります。

まあ、現状出ている用途を考えると「scoped じゃない params を必要とする場面はなく、scoped な params を必要とする場面はある」とのことで、「params が付いている時点で暗黙的に scoped にする」という判断になりそうです。

こうなるともう1つ問題が、オーバーライドをどうするかという話があるみたいです。
というのも、params 配列の場合、実はオーバーライド側には params 修飾を付けなくてもいいそうで。

<pre class="source" title="params 配列のオーバーライドには params 修飾不要">
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) { }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// params 配列の場合、派生側で params を付けなくても別にいい。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }
}
</pre>

ところがまあ、「params ref strcut は暗黙的に scoped」みたいな暗黙の挙動があるので、
「何もつけてないのになぜか scoped」みたいな挙動は避けたいでしょう。
なので、この場合は「オーバーライド側にも params を必須にしたい」とのこと。

(けど、[LDM に挙げられている例](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md#params-and-scoped-across-overrides)を見るに、戻り値があるときにだけこれを求められていそう…)

## オーバーロード解決

現在の params (配列の `params T[]`)とコレクション式は、ちょっとオーバーロード解決の仕組みが違います。
なので、「params の部分を `[]` で覆っても同じ結果になる」というのは**成り立たない**ことになります。
例えば以下のようなもの。

<pre class="source" title="params と []">
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]); <span class="comment">// こちらは解決できなくてエラーに。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>); <span class="comment">// こちらは int[] 側に解決。</span>

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">params</span> <span class="reserved">long</span>[] <span class="variable local">_</span>) { }
}
</pre>

で、params コレクションに関してですが、「既存の params 配列に沿う」案で行くみたいです。

## 引数の評価順

引数に副作用のある式を渡さない限り問題になることは少ないので忘れがちですが、
引数をどういう順で評価するかは決めておかないと混乱のもとです。
C# は基本的に「呼び出し側で並べた順」で、例えば名前付き引数を使うと順序を変えることができたりします。

<pre class="source" title="引数の評価は並べた順">
<span class="method"><span class="static">Test</span></span>(<span class="static"><span class="method">GetA</span></span>(), <span class="static"><span class="method">GetB</span></span>()); <span class="comment">// A → B</span>
<span class="method"><span class="static">Test</span></span>(<span class="variable local">b</span>: <span class="static"><span class="method">GetB</span></span>(), <span class="variable local">a</span>: <span class="static"><span class="method">GetA</span></span>()); <span class="comment">// B → A</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Test</span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">b</span>) { }

<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">GetA</span></span>() { <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;A&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">GetB</span></span>() { <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;B&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
</pre>

で、名前付き引数を使うと params 引数の場所も末尾以外に移せたり。

<pre class="source" title="params 引数を真ん中に。GetC の評価順も真ん中に">
<span class="static"><span class="method">Test</span></span>(<span class="variable local">b</span>: <span class="static"><span class="method">GetB</span></span>(), <span class="variable local">c</span>: <span class="method"><span class="static">GetC</span></span>(), <span class="variable local">a</span>: <span class="method"><span class="static">GetA</span></span>()); <span class="comment">// B → C → A</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Test</span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">b</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">c</span>) { }

<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">GetA</span></span>() { <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;A&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">GetB</span></span>() { <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;B&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">GetC</span></span>() { <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;C&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
</pre>

ちなみにこの時、`params int[] c` のための配列は、`Test` を呼ぶ直前になるそうです。
ということで、展開結果は以下のような感じ。

<pre class="source" title="先ほどのコードの展開結果">
<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="static"><span class="method">GetB</span></span>();
<span class="reserved">var</span> <span class="variable">c</span> <span class="operator">=</span> <span class="static"><span class="method">GetC</span></span>();
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="static"><span class="method">GetA</span></span>();
<span class="reserved">var</span> <span class="variable">paramsC</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="variable">c</span> };
<span class="static"><span class="method">Test</span></span>(<span class="variable">a</span>, <span class="variable">b</span>, <span class="variable">paramsC</span>);
</pre>

ところが、params コレクションとなるとどうなるべきかという話になります。
コレクションのインスタンスはいつ作られるべきなのか。

<pre class="source" title="params を自作の型に変更">
<span class="method"><span class="static">Test</span></span>(<span class="variable local">b</span>: <span class="static"><span class="method">GetB</span></span>(), <span class="variable local">c</span>: <span class="method"><span class="static">GetC</span></span>(), <span class="variable local">a</span>: <span class="static"><span class="method">GetA</span></span>());

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Test</span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">b</span>, <span class="reserved">params</span> <span class="type">MyCollection</span> <span class="variable local">c</span>) { }

<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">GetA</span></span>() { <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;A&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">GetB</span></span>() { <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;B&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">GetC</span></span>() { <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;C&quot;</span>); <span class="control">return</span> <span class="number">0</span>; }

<span class="reserved">class</span> <span class="type">MyCollection</span> : <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">public</span> <span class="type">MyCollection</span>() { <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;MyCollection Construcotr&quot;</span>); }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    System<span class="operator">.</span>Collections<span class="operator">.</span><span class="type">IEnumerator</span> System<span class="operator">.</span>Collections<span class="operator">.</span><span class="type">IEnumerable</span><span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
}
</pre>

こんな副作用の起こし方をするコードはめったに書かないでしょうけども、
"MyCollection Construcotr" はいつ表示されるべきでしょう？

とりあえず現状は、B → MyCollection → C → A の順で考えているそうです。
引数 `c:` の場所で生成。`GetC` を呼ぶよりも前。
要するに、以下のように展開したいんでしょうね。

<pre class="source" title="params 部分の展開の例">
<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="static"><span class="method">GetB</span></span>();
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="method"><span class="static">GetA</span></span>();
<span class="reserved">var</span> <span class="variable">paramsC</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">MyCollection</span>();
<span class="variable">paramsC</span><span class="operator">.</span><span class="method">Add</span>(<span class="method"><span class="static">GetC</span></span>());
<span class="method"><span class="static">Test</span></span>(<span class="variable">a</span>, <span class="variable">b</span>, <span class="variable">paramsC</span>);
</pre>

## メタデータ

今ある params 配列のコンパイル結果には `System.ParamArrayAttribute` が付きます。
で、C# 13 で考えている params コレクションでも、別にこの属性を使いまわすこともできるそうです。

ただ1点懸念は、C# 以外のコンパイラーが誤動作しないかどうか。
新しい属性であれば「未対応なので無視」でいいわけですが、
既存の属性を使いまわすと「`ParamArray` 属性が付いているのであれば配列でないとダメ」というコンパイル エラーを起こす可能性が高いです。

ということで、新しい params コレクションについては新しい属性として `System.Runtime.CompilerServices.ParamCollectionAttribute` を用意するそうです。
