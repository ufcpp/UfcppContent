---
title: "ピックアップRoslyn 4/4: static virtual/abstract members"
source_url: "https://ufcpp.net/blog/2021/4/staticvirtual/"
content_type: "BlogEntry"
published_at: "2021-04-04T17:07:36"
updated_at: "2021-04-04T17:07:36"
tags: []
umbraco_id: 2347
parent_id: 2346
sort_order: 0
aliases: []
---

# ピックアップRoslyn 4/4: static virtual/abstract members

インターフェイスの静的メソッドを virtual/abstract 指定できるようにする話が出ています。

- [[Proposal]: Static abstract members in interfaces #4436](https://github.com/dotnet/csharplang/issues/4436)

主な用途は、

- ファクトリ
- 比較 (`Equatable` とか `Comparable`)
- 数値計算

とかになると思います。
一番求められている用途は数値計算で、要は [NumPy](https://ja.wikipedia.org/wiki/NumPy) みたいなことを C# でも苦痛なく、かつ、パフォーマンスを損なうことなく実現したいというものです。

## <a id="factory">ファクトリ</a>

数値計算に特化した仕様かと言うとそんなこともないので、先に他の用途について触れておきます。

ジェネリックなメソッドを作るとき、`new()` 制約を付けることで引数なしのコンストラクターなら呼び出せるんですが…

<pre class="source" title="new() 制約">
<code><span class="reserved">void</span> <span class="method">m</span>&lt;<span class="type">T</span>&gt;() <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>()
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">T</span>(); <span class="comment">// OK</span>
}
</code></pre>

ところが、この `new` には引数を渡せません。

<pre class="source" title="new(X) は書けない">
<code><span class="reserved">void</span> <span class="method">m</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">int</span> <span class="variable">i</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>(<span class="reserved">int</span>) <span class="comment">// こう書きたい(ダメ)</span>
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> T(i); <span class="comment">// ダメ</span>
}
</code></pre>

これを例えば以下のように書けるようにすることで代替できるようになります。

<pre class="source" title="new(X) の代替で T.New(X)">
<code><span class="reserved">void</span> <span class="method">m</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">int</span> <span class="variable">i</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IConvartibleFromInt</span> <span class="comment">// 普通のインターフェイス制約</span>
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="type">T</span>.New(<span class="variable">i</span>); <span class="comment">// こう書けるようにする</span>
}

<span class="reserved">interface</span> <span class="type">IConvartibleFromInt</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="type">IConvartibleFromInt</span> <span class="method">New</span>(<span class="reserved">int</span> <span class="variable">i</span>);
}
</code></pre>

## <a id="generic-math">generic math</a>

たびたび出てくる要望として、 `+`, `-`, `*`, `/` をジェネリックな型で使いたいというものがあります。
わかりやすい例だと「[`Enumerable.Sum`](https://source.dot.net/#System.Linq/System/Linq/Sum.cs,17ae8142727f08ee) の実装何個あるんだ」って話で。
中身はほぼ定型文で、以下のようなコードのコピペが何個も並んでいます。

<pre class="source" title="Sum">
<code><span class="control">foreach</span> (<span class="reserved">int</span> <span class="variable">v</span> <span class="control">in</span> <span class="variable">source</span>)
{
    <span class="variable">sum</span> += <span class="variable">v</span>;
}
</code></pre>

コピペせざるを得ないのはジェネリックな型に対して `+` を使えないからです。

業務アプリ開発とかでは大体 `int` か `double`、せいぜい `decimal` を使っておけばいいのでジェネリックじゃなくてもそこまで困らないんですが、
汎用数学ライブラリみたいなのを作ろうとすると結構困ります。
[NumPy](https://ja.wikipedia.org/wiki/NumPy) みたいなものの利用者を取り込みたいし、この問題を解決したいという流れ。

現状の C# で汎用数学処理を書こうとするとどうなるかと言うと、以下のような感じ(3年位前のブログ):

- [C# にも型クラス(Shapes)が欲しい…](../../../2018/5/metricspace/index.md)

ブログタイトルが「型クラス」となっていますが、まあ、それが今回出ている「[static virtual 提案](https://github.com/dotnet/csharplang/issues/4436)」の原型。

- [Shapes and Extensions](../../../2017/2/pickuproslyn0223/index.md)

この「Shapes」というやつは結構込み入った仕様なんですが、
いったんこのうちの一部分というか、既存の文法からそう大きく外れない範囲でできるものが
「インターフェイスの [static](../../../../study/csharp/oop/oo_static.md) メソッドに [virtual](../../../../study/csharp/oop/oo_polymorphism.md#virtual)/[abstract](../../../../study/csharp/oop/oo_abstract.md) を認めよう」というものです。

上記の `Sum` であれば、「0 を取得」と「足し算」の2つがあれば書けるので、まず以下のようなインターフェイスを用意。

<pre class="source" title="static virtual / static abstract の宣言">
<code><span class="reserved">interface</span> <span class="type">IAddable</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IAddable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type">T</span> Zero { <span class="reserved">get</span>; } =&gt; <span class="reserved">default</span>(<span class="type">T</span>);
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="type">T</span> <span class="reserved">operator</span> +(<span class="type">T</span> <span class="variable">t1</span>, <span class="type">T</span> <span class="variable">t2</span>);
}
</code></pre>

これが入るのであれば、標準の `int` 型(`Int32` 構造体(`System` 名前空間))に以下のような実装も足されることになります。

<pre class="source" title="static virtual / static abstract の実装">
<code><span class="reserved">struct</span> <span class="type">Int32</span> : …, <span class="type">IAddable</span>&lt;<span class="type">Int32</span>&gt;
{
    <span class="reserved">static</span> <span class="type">Int32</span> I.<span class="reserved">operator</span> +(<span class="type">Int32</span> x, <span class="type">Int32</span> y) =&gt; x + y; <span class="comment">// Explicit</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> Zero =&gt; 0;                          <span class="comment">// Implicit</span>
}
</code></pre>

これを使って `Sum` メソッドを書くと以下のようになります。

<pre class="source" title="static virtual / static abstract の利用">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> <span class="method">Sum</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>[] <span class="variable">ts</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IAddable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span> <span class="variable">result</span> = <span class="type">T</span>.Zero;                   <span class="comment">// Call static operator</span>
    <span class="control">foreach</span> (<span class="type">T</span> <span class="variable">t</span> <span class="control">in</span> <span class="variable">ts</span>) { <span class="variable">result</span> <span class="method">+=</span> <span class="variable">t</span>; } <span class="comment">// Use `+`</span>
    <span class="control">return</span> <span class="variable">result</span>;
}
</code></pre>

これ、下手な実装をするとパフォーマンスを著しく損ねます。
`+` なんてネイティブコード化されると CPU の1命令だったりするわけですが、
そこに、インターフェイスが挟まることで仮想関数呼び出しが挟まったり、インライン展開阻害が起きたりして数倍～1桁遅くなります。

とはいえ、[前述の3年前のブログ](../../../2018/5/metricspace/index.md)でやっているような「値型ジェネリクスを使った黒魔術」でパフォーマンスは解決できるんですが、型引数が余分に1個増えたり、演算子を使えなかったり、だいぶ使い勝手は悪いです。

<pre class="source" title="これまでの黒魔術的な回避策">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> <span class="method">Sum</span>&lt;<span class="type">T</span>, <span class="type">TAddable</span>&gt;(<span class="type">T</span>[] <span class="variable">ts</span>) <span class="reserved">where</span> <span class="type">TAddable</span> : <span class="type">IAddable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">T</span> <span class="variable">result</span> = <span class="reserved">default</span>(<span class="type">TAddable</span>).Zero;
    <span class="control">foreach</span> (<span class="type">T</span> <span class="variable">t</span> <span class="control">in</span> <span class="variable">ts</span>) { <span class="variable">result</span> = <span class="reserved">default</span>(<span class="type">TAddable</span>).Add(<span class="variable">result</span>, <span class="variable">t</span>); }
    <span class="control">return</span> <span class="variable">result</span>;
}
</code></pre>

## <a id="type-param">型引数による分岐</a>

普通の、既存の virtual/abstract メソッドの場合、
実際にどのメソッドが呼び出されるかはインスタンスの実行時の型によって決まります。

<pre class="source" title="通常の virtual/abstract は実行時の型によって呼び出し先が決定される">
<code><span class="reserved">using</span> System;
 
<span class="comment">// 型引数が何だろうと、インスタンスが A なので表示されるのは &quot;A&quot;。</span>
<span class="method">m</span>&lt;<span class="type">I</span>&gt;(<span class="reserved">new</span> <span class="type">A</span>());
<span class="method">m</span>&lt;<span class="type">A</span>&gt;(<span class="reserved">new</span> <span class="type">A</span>());
 
<span class="comment">// 型引数が何だろうと、インスタンスが B なので表示されるのは &quot;B&quot;。</span>
<span class="method">m</span>&lt;<span class="type">I</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>());
<span class="method">m</span>&lt;<span class="type">A</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>());
<span class="method">m</span>&lt;<span class="type">B</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>());
 
<span class="reserved">void</span> <span class="method">m</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">I</span> =&gt; <span class="variable">x</span>.<span class="method">M</span>();
 
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">M</span>();
}
 
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">I</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B&quot;</span>);
}
</code></pre>

一方、static virtual/abstract の場合は型引数を見ます。
コンパイル時に決定済み。
abstract なままのもの(実態がないもの)を使うとコンパイル自体できません。

<pre class="source" title="static virtual/abstract はコンパイル時に渡した型引数で決定される">
<code><span class="reserved">using</span> System;
 
<span class="comment">// static virtual/abstract の場合は型引数の方で呼び出し先が決まる。</span>
<span class="method">m</span>&lt;<span class="type">I</span>&gt;(<span class="reserved">new</span> <span class="type">A</span>()); <span class="comment">// コンパイル エラー。 I.M に実装がない。</span>
<span class="method">m</span>&lt;<span class="type">A</span>&gt;(<span class="reserved">new</span> <span class="type">A</span>()); <span class="comment">// &quot;A&quot;</span>
 
<span class="method">m</span>&lt;<span class="type">I</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>()); <span class="comment">// コンパイル エラー。 I.M に実装がない。</span>
<span class="method">m</span>&lt;<span class="type">A</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>()); <span class="comment">// &quot;A&quot;</span>
<span class="method">m</span>&lt;<span class="type">B</span>&gt;(<span class="reserved">new</span> <span class="type">B</span>()); <span class="comment">// &quot;B&quot;</span>
 
<span class="reserved">void</span> <span class="method">m</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">I</span> =&gt; <span class="type">T</span>.M();
 
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>();
}
 
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">I</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B&quot;</span>);
}
</code></pre>

## <a id="runtime-mod">型システムの修正</a>

これ、C# コンパイラーのレベルで実現しようと思うと、多分前述の「黒魔術的な構造体ジェネリクス」みたいなコードを生成することになります。
さすがにちょっと「コンパイラーが裏でこっそり生成するコード」にするのもためらわれる(型引数の個数が変わるとかだいぶつらい)レベルです。

なので、.NET ランタイムの型システム自体に手を入れる必要がありました。
実際、型システムに手を入れる(.NET 5 以前では使えない機能になる)方向で実装を進めるそうです。

C# 8.0 くらいから、こういう「古いランタイムでは動かない機能」がちらほら入ってきています。

- C# 8.0 の[インターフェイスのデフォルト実装](../../../../study/csharp/oop/oo_interface.md#dim): .NET Core 3.0 以降でだけ動く
- C# 9.0 の[共変戻り値](../../../../study/csharp/oop/oo_polymorphism.md#covariance): .NET 5.0 以降でだけ動く

(ちなみに、この辺りの一定バージョン以上のランタイムでしか動かない機能については「[RuntimeFeature](../../../2018/12/runtimefeature/index.md)」でちょっと書いています。)

とはいえ、デフォルト実装とか共変戻り値と比べても、static virtual/abstract は実装が難しめの機能になります。

結構な大事なんですが、[Miguel de Icaza](https://github.com/migueldeicaza) ([Mono](https://ja.wikipedia.org/wiki/Mono_(%E3%82%BD%E3%83%95%E3%83%88%E3%82%A6%E3%82%A7%E3%82%A2)) 創設者)が[プロトタイプ](https://github.com/partydonk/partydonk/)を作っていて、これをベースに話が進んでいるみたいです。
