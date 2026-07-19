---
title: "未使用ジェネリック型引数で TypeLoadException が起こる問題"
source_url: "https://ufcpp.net/blog/2022/12/unused-generic-type-parameter/"
content_type: "BlogEntry"
published_at: "2022-12-11T18:17:58"
updated_at: "2023-08-03T22:39:37"
tags: []
umbraco_id: 2444
parent_id: 2438
sort_order: 3
aliases: []
---

# 未使用ジェネリック型引数で TypeLoadException が起こる問題

今日は C# の構造体がらみで、
できそうでできない、
できてもいいはずだけど直されない、
コンパイルまでは通るのに実行時にエラーになってしまう制限の話。

※(2023/8/3追記) .NET 8 で直るそうです。

## 入れ子の構造体

C# で、構造体の中にその構造体自身のフィールドを持つことはできません。
レイアウトの決定が無限再帰を起こすので、これはダメで当然。

<pre class="source" title="構造体の入れ子はダメ">
<span class="reserved">struct</span> <span class="type struct">S</span> { <span class="type struct">S</span> <span class="field"><span class="error" title="CS0523">_nested</span></span>; }
</pre>

これはそもそもコンパイル エラーになります。
当然。

## 使ってないジェネリック型引数でも TypeLoadExcpetion

問題は以下のような場合。
現在の C# (というか .NET の型システム)では、以下のような型はコンパイルはできるものの、実行してみようとすると実行時例外を起こします。
(構造体 `S` のメンバーに初めて触れた瞬間に `TypeLoadException` が飛ぶ。)

<pre class="source" title="疑惑の判定">
<span class="reserved">struct</span> <span class="type struct">S</span> { <span class="type struct">Empty</span>&lt;<span class="type struct">S</span>&gt; <span class="field">_empty</span>; }
<span class="reserved">struct</span> <span class="type struct">Empty</span>&lt;<span class="type param">T</span>&gt; { }
</pre>

`Empty<T>` の側が `T` のフィールドを持っていないので
(というか空っぽなので、`T` が何かによらずサイズ1で固定)、
レイアウト決定で無限再帰は起こさないはずです。
実際、これは、

* 原理的にはできてもいい
* C# は禁止していない
* CLI (.NET のランタイム仕様)でも禁止は名言されていない
* 現在の .NET のランタイムの実装が過剰防衛している

という状態。
[C# コンパイラー チームの人がそれを指摘する issue](https://github.com/dotnet/runtime/issues/6924) も立っていたりします。

### 実用例

まあ、`Empty` みたいな無意味なコードは誰も書かないとしても、
例えば、以下のようなシナリオでなら似たようなことをしたくなる人はいるはずです。

まず、以下のように、構造体の配列で木構造を表現する例を考えます。

<pre class="source" title="配列に Parent と Next を持たせた型を入れて木構造を表現">
<span class="comment">// 配列に Parent と Next を持たせた型を入れて木構造を表現。</span>
<span class="comment">// A も B もツリー。</span>
<span class="comment">// A からは B も参照。</span>
<span class="reserved">class</span> <span class="type">Tree</span>
{
    <span class="type struct">A</span>[] <span class="field">A</span>;
    <span class="type struct">B</span>[] <span class="field">B</span>;
}

<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">int</span> <span class="field">Parent</span>;
    <span class="reserved">int</span> <span class="field">Next</span>;
    <span class="reserved">int</span> <span class="field">BIndex</span>;
}

<span class="reserved">struct</span> <span class="type struct">B</span>
{
    <span class="reserved">int</span> <span class="field">Parent</span>;
    <span class="reserved">int</span> <span class="field">Next</span>;
}
</pre>

実際にはさらに、「インデックスとは関係ない別の `int` も持ちたくなったりするはずで、なおのこと「この `int` は何？」みたいになると思います。

<pre class="source" title="この int は何？">
<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="comment">// 木とは別に持ちたいデータ。</span>
    <span class="reserved">int</span> <span class="field">Value</span>;
    <span class="reserved">int</span> <span class="field">Length</span>;
    <span class="input">...</span>

    <span class="comment">// 木構造表現用。</span>
    <span class="reserved">int</span> <span class="field">Parent</span>;
    <span class="reserved">int</span> <span class="field">Next</span>;

    <span class="comment">// 別の木を参照</span>
    <span class="reserved">int</span> <span class="field">BIndex</span>;
}
</pre>

ということで、`Parent` や `Next` が「配列 `A[]` のインデックス」であることが一目でわかるようにしたくなったりします。
よくやるのが、以下のように「`int` をラップした構造体を用意」みたいな手段。

<pre class="source" title="「配列 T[] のインデックス」用の int のラッパー構造体">
<span class="reserved">struct</span> <span class="type struct">Index</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type struct">Index</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">Index</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">value</span>);
}
</pre>

この型を使って先ほどの `Tree`, `A`, `B` を書き換えると以下のような感じになります。

<pre class="source" title="Index 構造体の導入">
<span class="comment">// 配列に Parent と Next を持たせた型を入れて木構造を表現。</span>
<span class="comment">// A も B もツリー。</span>
<span class="comment">// A からは B も参照。</span>
<span class="reserved">class</span> <span class="type">Tree</span>
{
    <span class="type struct">A</span>[] <span class="field">A</span>;
    <span class="type struct">B</span>[] <span class="field">B</span>;
}

<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">int</span> <span class="field">Value</span>;
    <span class="reserved">int</span> <span class="field">Length</span>;
    <span class="type struct">Index</span>&lt;<span class="type struct">A</span>&gt; <span class="field">Parent</span>;
    <span class="type struct">Index</span>&lt;<span class="type struct">A</span>&gt; <span class="field">Next</span>;
    <span class="type struct">Index</span>&lt;<span class="type struct">B</span>&gt; <span class="field">BIndex</span>;
}

<span class="reserved">struct</span> <span class="type struct">B</span>
{
    <span class="type struct">Index</span>&lt;<span class="type struct">B</span>&gt; <span class="field">Parent</span>;
    <span class="type struct">Index</span>&lt;<span class="type struct">B</span>&gt; <span class="field">Next</span>;
}
</pre>

**便利！**

と思ったところで、冒頭の `Empty<T>` の例と同じ理屈の過剰防衛で、
`TypeLoadException` を起こします…

## 回避策

ちょっと不格好でもよければ解決方法は簡単で、
1段ダミーのクラスを挟むだけだったり。

<pre class="source" title="ダミーのクラスを1個用意">
<span class="reserved">struct</span> <span class="type struct">Index</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type struct">Index</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">Index</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">value</span>);
}

<span class="comment">// Index&lt;Dummy&lt;T&gt;&gt; とか Index&lt;Empty&lt;T&gt;&gt; よりは Index&lt;Of&lt;T&gt;&gt; の方がマシかなと…</span>
<span class="reserved">class</span> <span class="type">Of</span>&lt;<span class="type param">T</span>&gt; { }
</pre>

<pre class="source" title="やむなく Index&lt;Of&lt;T&gt;&gt;">
<span class="reserved">class</span> <span class="type">Tree</span>
{
    <span class="type struct">A</span>[] <span class="field">A</span>；
    <span class="type struct">B</span>[] <span class="field">B</span>;
}

<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">int</span> <span class="field">Value</span>;
    <span class="reserved">int</span> <span class="field">Length</span>;
    <span class="type struct">Index</span>&lt;<span class="type">Of</span>&lt;<span class="type struct">A</span>&gt;&gt; <span class="field">Parent</span>;
    <span class="type struct">Index</span>&lt;<span class="type">Of</span>&lt;<span class="type struct">A</span>&gt;&gt; <span class="field">Next</span>;
    <span class="type struct">Index</span>&lt;<span class="type">Of</span>&lt;<span class="type struct">B</span>&gt;&gt; <span class="field">BIndex</span>;
}

<span class="reserved">struct</span> <span class="type struct">B</span>
{
    <span class="type struct">Index</span>&lt;<span class="type">Of</span>&lt;<span class="type struct">B</span>&gt;&gt; <span class="field">Parent</span>;
    <span class="type struct">Index</span>&lt;<span class="type">Of</span>&lt;<span class="type struct">B</span>&gt;&gt; <span class="field">Next</span>;
}
</pre>

だいぶ不格好で嫌なので、
[件の issue](https://github.com/dotnet/runtime/issues/6924) の優先度を上げてもらえるように👍を付けまくってもらえたりすると大変うれしかったりは…

この issue は2016年からずっと「Future」(いつかね、いつか)なんですよね。
これ、C# 6.0 (つまり、Roslyn 化/C# への移植)の頃に始めて報告されたというだけで、
実際には .NET Framework が生まれてこの方ずっとかも。

※(2023/8/3追記) .NET 8 で直るそうです。C# にジェネリクスが導入されて以来の20年越しの修正に。
