---
title: "null 許容参照型"
source_url: "https://ufcpp.net/study/csharp/resource/nullablereferencetype/"
content_type: "Article"
published_at: "2019-08-11T00:00:00"
updated_at: "2020-06-13T00:00:00"
tags:
  - "Ver. 8.0"
umbraco_id: 2255
parent_id: 1286
sort_order: 11
aliases:
  - "/csharp/resource/nullablereferencetype/"
---

# null 許容参照型

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<h5 class="version version8">Ver. 8.0</h5>

C# くらいの世代(1990年代後半～2000年代前半)のプログラミング言語では、
[参照型](oo_reference.md#reftype)には [null](oo_reference.md#null) が「つきもの」で、不可避なものでした。
(参考: 「[null参照問題](https://www.buildinsider.net/column/iwanaga-nobuyuki/011)」。)

ただ、2010年代ともなると、「つきもの」として惰性で null を認めるのはよくないとされています。
C# でも、少なくとも「意図して null を使っているかどうか」を区別できる必要性が生まれました。

そこで C# 8.0 では、以下のような機能を提供することにしました。

- 参照型でも単に型 `T` と書くと null を認めない型になる
- `T?` と書くと null を代入できる型になる

C# 7.X の頃と 8.0 で何が変わったかというと、
「参照型でも null を拒否できるようになった」ということになります。
ただ、「`T?` と書いたときに null 許容」という方式なのと、値型との対比として、
この機能は<strong id="key-nrt" class="keyword">null許容参照型</strong>(nullable reference type)と呼びます(略してNRTと言うことも)。

構文的には C# 2.0 からあった[null許容値型](sp2_nullable.md)と極力そろうように作られています。

ただ、後入りな機能なので、以下のような制約が掛かります。

- opt-in (オプションを明示しないと有効にならない)方式
  - `T` の意味が変わるので、opt-in にしないと既存のコードがコンパイルできなくなる
- 警告のみ
  - `T` 型の変数に null を代入しても警告だけで、エラーにはならない
- 値型と参照型で、`T?` の挙動が違う
  - 参照型の `T` と `T?` はアノテーション<sup>※</sup>だけの差で、内部的には差がない
  - 値型の場合は `T?` と書くと実体は `Nullable<T>` という `T` と明確に異なる型になる
  - 特に、[ジェネリクス](../oop/sp2_generics.md)を使うときに困る

<sup>※</sup> annotation。「単なる注釈」という意味で、この場合は「コンパイラーがソースコード解析するために使うヒントとなる情報」くらいの意味合い。

##<a id="sec-generated-title-2"></a> <a id="opt-in"></a>null許容参照型の有効化
無条件に「参照型でも null を拒否する」としてしまうと、既存の C# コードの挙動を壊します。

<pre class="source" title="opt-in した瞬間に警告">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// NRT を opt-in した時点で警告が出るようになる</span>
        <span class="reserved">string</span> <span class="variable">s</span> = <span class="warning"><span class="reserved">null</span></span>; <span class="comment">// string (非 null)に null を入れちゃダメ</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable"><span class="warning">s</span></span>.Length); <span class="comment">// null の可能性があるものを null チェックせずに使っちゃダメ</span>
    }
}
</code></pre>

警告だから追加してもいいということにはなりません。
警告を残すのは作法的によくないことですし、
なので、C# には[「警告をすべてエラー扱いする」というオプション](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/compiler-options/warnaserror-compiler-option)もあります。
警告の追加も破壊的変更の一種になります。

C# は「既存のソースコードがコンパイルできなくなる」というのをかなり慎重に避けている言語なので、null許容参照型は無条件に入れられる機能ではありません。
そのため、明示的な有効化(opt-in)が必要になります。

有効化された状態かどうかを指して、<strong id="nullable-context" class="keyword">null 許容コンテキスト</strong>(nullable context)と言います。
(有効・無効を切り替えることを「null 許容コンテキストの切り替え」とか言ったりします。)

null 許容コンテキストの切り替え方は2通りあります。

- ソースコード中の行単位での切り替え … `#nullable` ディレクティブ
- プロジェクト全体での切り替え … `Nullable` オプション

また、単純な有効・無効以外に、後述する warnings/annotations (それぞれ警告のみ、アノテーションのみの有効・無効化)というモードもあります。

ちなみに、C# は本来、オプションでのオン/オフ切り替えなど、
「文法の分岐」に対してもかなり消極的な言語です。
opt-in 方式で `T` の意味が変わるnull許容参照型もだいぶ悩んだ末の苦渋の決断で、
それだけnull参照問題が深刻だということです。
おそらく、C# 史上最初で最後の大きな「分岐」になると思われます。

###<a id="sec-generated-title-3"></a> <a id="nullable-directive"></a>#nullable ディレクティブ
それなりの規模のソースコードを保守している場合、いきなりnull許容参照型を全面的に有効化してしまうと結構大変なことになります。
(筆者の経験的な話で言うと、少なくとも50行に1個くらいは警告が出ます。何万行ものソースコードを持っている場合、とてもじゃないけど直して回れるものではありません。)

そのため、[プリプロセッサー](../misc/sp_preprocess.md)的に、書いたその行以降の opt-in/opt-out をする `#nullable` ディレクティブが用意されています。
([`#pragma warning`](../misc/sp_preprocess.md#pragma)と似たような使い方をします。)

以下のような書き方をします。

<pre class="source" title="nullable ディレクティブ">
<code><span class="inactive">#nullable</span> <span class="input">enable|disable|restore</span> <span class="input">[warnings|annotations]</span>
</code></pre>

null 許容参照型を有効にしたければ`#nullable enable`、
無効にしたければ`#nullable disable`と書きます。
`#nullable restore`は「1つ前のコンテキストに戻す」という処理になります。
`warnings`と`annotations`については後述しますが、省略可能で、省略した場合は「両方をオン・オフ」になります。

<pre class="source" title="null 許容コンテキストの切り替え例">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
        <span class="method">E1</span>(<span class="warning"><span class="reserved">null</span></span>); <span class="comment">// 警告が出る</span>
 
<span class="inactive">#nullable</span> <span class="inactive">disable</span>
        <span class="method">E1</span>(<span class="reserved">null</span>); <span class="comment">// 警告が出ない</span>
    }
 
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
    <span class="comment">// 有効化したのでここでは string で非 null、string? で null 許容。</span>
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">E1</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="variable">s</span>.Length;
    <span class="reserved">static</span> <span class="reserved">int</span>? <span class="method">E2</span>(<span class="reserved">string</span>? <span class="variable">s</span>) =&gt; <span class="variable">s</span>?.Length;
 
<span class="inactive">#nullable</span> <span class="inactive">disable</span>
    <span class="comment">// 無効化したので string に null が入っている可能性あり。</span>
    <span class="comment">// string? とは書けない(書くだけで警告になる)。</span>
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">D1</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="variable">s</span>.Length;
 
<span class="inactive">#nullable</span> <span class="inactive">restore</span>
    <span class="comment">// 1つ前のコンテキストに戻す。</span>
    <span class="comment">// この場合、disable から enable に戻る。</span>
    <span class="reserved">static</span> <span class="reserved">int</span>? <span class="method">R1</span>(<span class="reserved">string</span>? <span class="variable">s</span>) =&gt; <span class="variable">s</span>?.Length;
}
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="nullable-option"></a>Nullable オプション
一方で、これから新規に作成するプログラムの場合、最初から全部null許容参照型を有効化してしまう方がいいでしょう。
そのくらい、null参照問題は避けたいものです。

プロジェクト全体で null 許容コンテキストを切り替えるには、コンパイラー オプションを指定します。
`csc` (C# コンパイラー)コマンドを直接使う場合は `/nullable` オプションで指定します。

<pre class="console" title="csc の /nullable オプション">
<code>csc <span class="input">source.cs</span> <em>/nullable:enable</em> /langversion:8
</code></pre>

csproj (C# プロジェクト)ファイル中でオプション指定する場合、`<Nullable>` タグを使います。

<pre class="xsource" title="csproj の Nullable オプション">
<code><span class="attvalue">&lt;</span><span class="element">Project</span><span class="attvalue"> </span><span class="attribute">Sdk</span><span class="attvalue">=</span>&quot;<span class="attvalue">Microsoft.NET.Sdk</span>&quot;<span class="attvalue">&gt;</span>
 
<span class="attvalue">  &lt;</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>Exe<span class="attvalue">&lt;/</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>netcoreapp3.0<span class="attvalue">&lt;/</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>
<span class="attvalue">    <em>&lt;</span><span class="element">Nullable</span><span class="attvalue">&gt;</span>enable<span class="attvalue">&lt;/</span><span class="element">Nullable</span><span class="attvalue">&gt;</span></em>
<span class="attvalue">  &lt;/</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
 
<span class="attvalue">&lt;/</span><span class="element">Project</span><span class="attvalue">&gt;</span>
</code></pre>

指定できる値は `enable`(有効)、`disable` (無効)、`warnings` (警告のみ有効)、`annotations` (アノテーションのみ有効)の4種類です。
`warnings` と `annotations` については次節で説明します。

###<a id="sec-generated-title-5"></a> <a id="nullable-directive"></a>warnings/annotations
null 許容参照型には以下の2つの側面があります。

- アノテーション: 型に `?` を付けて null 許容か非 null かを明示する
- 警告: アノテーションを見て、適切な null チェックが行われてるかどうかを調べて警告を出す

![warnings/annotations](../../../../assets/media/1177/annotation_warning.png)

既存コードを null 許容参照型に段階的に対応させていくにあたって、
これら2つは別々に有効化・無効化できます。
以下のような状況を想定しています。

- 差し当たってアノテーションだけは付けたいけど、中身の警告を全部消す作業まで手が回らない
- 差し当たって警告は出してほしいけど、自分が公開している API にまでは責任を持てないのでアノテーションは付けたくない

アノテーションを付けるかどうかだけを切り替えるのが `annotations` で、
警告の有無だけを切り替えるのが `warnings` です。

例えば、元々以下のようなコードがあったとします。

<pre class="xsource" title="既存コード(null 許容参照型に未対応)">
<code><span class="inactive"><span class="attvalue">string</span> <span class="method">NotNull</span>() =&gt; <span class="element">&quot;&quot;</span>;
<span class="attvalue">string</span> <span class="method">MaybeNull</span>() =&gt; <span class="attvalue">null</span>;
 
<span class="attvalue">int</span> <span class="method">M</span>(<span class="attvalue">string</span> <span class="variable">s</span>)
{
    <span class="attvalue">var</span> <span class="variable">s1</span> = <span class="method">NotNull</span>();
    <span class="attvalue">var</span> <span class="variable">s2</span> = <span class="method">MaybeNull</span>();
    <span class="control">return</span> <span class="variable">s</span>.Length + <span class="variable">s1</span>.Length + <span class="variable">s2</span>.Length;
}
</code></pre>

これに対して、単に `#nullable enable` を付けるとアノテーションも警告も有効になります。

<pre class="xsource" title="enable のみ指定(アノテーションも警告も有効化)">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="attvalue">string</span> <span class="method">NotNull</span>() =&gt; <span class="element">&quot;&quot;</span>;
<span class="attvalue">string</span>? <span class="method">MaybeNull</span>() =&gt; <span class="attvalue">null</span>; <span class="comment">// 戻りに ? を追加</span>
 
<span class="attvalue">int</span> <span class="method">M</span>(<span class="attvalue">string</span> <span class="variable">s</span>) <span class="comment">// この s は非 null の意味になる</span>
{
    <span class="attvalue">var</span> <span class="variable">s1</span> = <span class="method">NotNull</span>();
    <span class="attvalue">var</span> <span class="variable">s2</span> = <span class="method">MaybeNull</span>();
    <span class="control">return</span> <span class="variable">s</span>.Length + <span class="variable">s1</span>.Length + <span class="variable">s2</span>.Length; <span class="comment">// s2 のところに警告が出る</span>
}
</code></pre>

`#nullable enable warnings` とすると警告のみ有効化できます。
この場合、引数の `string` は「C# 7.3 以前と同じ扱い」で、null 許容かどうか「未指定」になります。

<pre class="xsource" title="警告のみ有効化">
<code><span class="comment">// 警告のみ有効化</span>
<span class="inactive">#nullable</span> <span class="inactive">enable</span> <span class="inactive">warnings</span>
<span class="attvalue">int</span> <span class="method">M</span>(<span class="attvalue">string</span> <span class="variable">s</span>) <span class="comment">// この s は null 許容かどうか「未指定」</span>
{
    <span class="attvalue">var</span> <span class="variable">s1</span> = <span class="method">NotNull</span>();
    <span class="attvalue">var</span> <span class="variable">s2</span> = <span class="method">MaybeNull</span>();
    <span class="control">return</span> <span class="variable">s</span>.Length + <span class="variable">s1</span>.Length + <span class="variable">s2</span>.Length; <span class="comment">// s2 のところに警告が出る</span>
}
</code></pre>

一方、`#nullable enable annotations` とするとアノテーションのみが有効化されます。
null のチェック漏れがあっても警告は出ない状態です。

<pre class="xsource" title="">
<code><span class="comment">// アノテーションのみ有効化</span>
<span class="inactive">#nullable</span> <span class="inactive">enable</span> <span class="inactive">annotations</span>
<span class="attvalue">int</span> <span class="method">M</span>(<span class="attvalue">string</span> <span class="variable">s</span>) <span class="comment">// この s は非 null</span>
{
    <span class="attvalue">var</span> <span class="variable">s1</span> = <span class="method">NotNull</span>();
    <span class="attvalue">var</span> <span class="variable">s2</span> = <span class="method">MaybeNull</span>();
    <span class="control">return</span> <span class="variable">s</span>.Length + <span class="variable">s1</span>.Length + <span class="variable">s2</span>.Length; <span class="comment">// 警告は出ない</span>
}
</code></pre>

##<a id="sec-generated-title-6"></a> <a id="flow-analysis"></a>フロー解析
null 許容参照型は、フロー解析(flow analysis)で成り立っています。
フロー解析というのは、コードの流れ(flow)を追って、
「使っている場所より前で正しく代入・チェックが行われるか」を C# コンパイラーが調べるものです。

例えば以下のように、変数 `s` に何を代入したかによって、それ以降、`s.Length` というようなメンバー アクセス時に警告が出たり出なかったりします。

<pre class="source" title="null 許容参照型はフロー解析で null チェックをしてる">
<code><span class="comment">// null 許容で宣言されていても、</span>
<span class="reserved">string</span>? <span class="variable">s</span>;
 
<span class="comment">// ちゃんと有効な値を代入すれば</span>
<span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
 
<span class="comment">// 警告は出なくなる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
 
<span class="comment">// 逆に null を代入すると、</span>
<span class="variable">s</span> = <span class="reserved">null</span>;
 
<span class="comment">// それ以降警告が出る。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">s</span></span>.Length);
</code></pre>

分岐などもきっちり調べられます。

<pre class="source" title="フロー解析は分岐もちゃんと調べる">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">flag</span>)
{
    <span class="reserved">string</span>? <span class="variable">s</span>;
 
    <span class="comment">// 分岐の1つでも null があれば、その後ろでは警告が出る。</span>
    <span class="control">if</span> (<span class="variable">flag</span>) <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
    <span class="control">else</span> <span class="variable">s</span> = <span class="reserved">null</span>;
 
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">s</span></span>.Length);
 
    <span class="comment">// 分岐の全部で非 null なら、その後ろでは警告が出ない。</span>
    <span class="control">if</span> (<span class="variable">flag</span>) <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>;
    <span class="control">else</span> <span class="variable">s</span> = <span class="string">&quot;123&quot;</span>;
 
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
}
</code></pre>

非 null (`?` が付いていない)変数・引数には null を渡した時点で警告が出て、
null 許容(`?` が付いてる)変数・引数の場合はメンバー アクセスの時点で警告が出ます。
また、null 代入の有無の他、`is null` や `== null` での null チェックをすれば、それ以降の警告は消えます。

<pre class="source" title="警告の出方">
<code><span class="reserved">using</span> System;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
    <span class="comment">// enable なコンテキストでは、string と書くと非 null、string? と書くと null 許容。</span>
    <span class="reserved">string</span> <span class="method">NotNull</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="variable">s</span>;
    <span class="reserved">string</span>? <span class="method">MaybeNull</span>(<span class="reserved">string</span>? <span class="variable">s</span>) =&gt; <span class="variable">s</span>;
 
    <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// 非 null。</span>
        <span class="reserved">var</span> <span class="variable">n</span> = <span class="method">NotNull</span>(<span class="warning"><span class="reserved">null</span></span>); <span class="comment">// 引数に null を渡した時点で警告。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">n</span>.Length);
 
        <span class="comment">// null 許容。</span>
        <span class="reserved">var</span> <span class="variable">m</span> = <span class="method">MaybeNull</span>(<span class="reserved">null</span>);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">m</span></span>.Length); <span class="comment">// 戻り値の null チェックをしなかった時点で警告。</span>
 
        <span class="control">if</span> (<span class="variable">m</span> <span class="reserved">is</span> <span class="reserved">null</span>) <span class="control">return</span>;
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">m</span>.Length); <span class="comment">// 前の行で null チェックしたのでもう警告にならない。</span>
    }
}
</code></pre>

ちなみに、一度何らかのメンバー アクセスをした時点で「null チェックした」扱いを受けます。
「null 許容型を null チェックなしで使ってる」警告が出るのは最初の1個だけになります。

<pre class="source" title="メンバー アクセスを持って null チェック扱い">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">x</span>)
{
    <span class="comment">// null チェックせずに使ったので警告。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">x</span></span>[0]);
 
    <span class="comment">// ただ、2重には警告がでない。警告が出るのは↑の行だけ。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>.Length);
}
</code></pre>

他の変数との比較でも null チェックになることがあります。
例えば以下のように、非 null な変数 `x` と一致したら null 許容な変数 `y` も null ではないことが確定します。
これもちゃんとフロー解析の対象になっています。

<pre class="source" title="他の変数との比較で null チェック">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span>? <span class="variable">y</span>)
{
    <span class="comment">// 非 null な x との比較で y が null じゃないことがわかる。</span>
    <span class="control">if</span> (<span class="variable">x</span> == <span class="variable">y</span>)
    {
        <span class="comment">// こっちは y が非 null なことがわかるので警告が出ない。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">y</span>.Length);
    }
    <span class="control">else</span>
    {
        <span class="comment">// こっちは null な可能性が残るので警告が出る。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">y</span></span>.Length);
    }
}
</code></pre>

#### <a id="sec-generated-title-7"></a>注意: 別スレッドでの書き換え
フィールドやプロパティに対するフロー解析では、利便性を優先して、シングルスレッド動作を前提としたフロー解析をしています。
例えば、以下のように、マルチスレッド動作をしていて、他のスレッドで書き換えられてしまうと、本来 null が来るはずがなく警告も出ない場面で null 参照例外が起こることがあります。

<pre class="source" title="別スレッドで null を代入することで不整合が起こる例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span>? S;
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetNull</span>()
    {
        S = <span class="reserved">null</span>;
    }
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetNonNull</span>()
    {
        <span class="control">if</span> (S <span class="reserved">is</span> <span class="reserved">null</span>) S = <span class="string">&quot;&quot;</span>;
 
        <span class="type">Thread</span>.<span class="method">Sleep</span>(200);
 
        <span class="comment">// 警告はでない。 S = &quot;&quot; しているので非 null 扱い。</span>
        <span class="comment">// 単一スレッド実行の場合はおかしくはない。</span>
        <span class="comment">// でも、Sleep 中に SetNull を呼ばれると null 参照例外になる。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(S.Length);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Program</span>();
        <span class="type">Task</span>.<span class="method">Run</span>(<span class="variable">p</span>.<span class="method">SetNonNull</span>);
        <span class="type">Thread</span>.<span class="method">Sleep</span>(100);
        <span class="type">Task</span>.<span class="method">Run</span>(<span class="variable">p</span>.<span class="method">SetNull</span>);
 
        <span class="type">Thread</span>.<span class="method">Sleep</span>(300);
    }
}
</code></pre>

###<a id="sec-generated-title-8"></a> <a id="initialize-field"></a>フィールドやプロパティの初期化
非 null 型のフィールドやプロパティは、コンストラクター内で必ず初期化しなければなりません。
例えば以下のコードはフィールド `X`、プロパティ `Y` のところに警告が出ます。

<pre class="source" title="非 null なフィールド・プロパティを初期化しないと警告が出る">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">X</span>;
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">Y</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

以下のように、コンストラクターを追加すれば警告が消えます。

<pre class="source" title="初期化コードを足すことで警告が消える">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> X;
    <span class="reserved">public</span> <span class="reserved">string</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="type">A</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span> <span class="variable">y</span>) =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
}
</code></pre>

ちなみに、コンストラクターは書いたものの初期化を忘れると、
フィールド・プロパティの方だけではなく、コンストラクターの方にも警告が出ます。

<pre class="source" title="初期か忘れ警告">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="warning">X</span>;
 
    <span class="comment">// X を初期化していないのでコンストラクターにも警告が出る</span>
    <span class="reserved">public</span> <span class="warning"><span class="type">A</span></span>() { }
}
</code></pre>

ちなみに、最終的には非 null になるものの、コンストラクターの時点ではどうしても一時的に null を入れておかないといけない場面というものもあったりします。
そういうときの回避策として、後述する [`!` 演算子](#null-forgiving)というものもあります。

<pre class="source" title="null をあえて見逃すための ! 演算子">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 一時的に null になってしまうことを強制的に容認</span>
    <span class="reserved">public</span> <span class="reserved">string</span> X = <span class="reserved">null</span><em>!</em>;
}
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="oblivious"></a>oblivious
opt-in にしたので、null 許容(nullable)、非 null (non-nullable, not null)の他に、
「アノテーションが付いていない、未指定」という状態があり得ます。
この未指定状態を oblivious (忘れてる、気づかない)と呼びます。

要するに、C# 7.3 以前で書かれたコードや、`#nullable enable annotations`になっていない場所で書かれたコードの型が oblivious です。
oblivious な型の変数は一切フロー解析の対象になりません。

<pre class="source" title="oblivious な変数">
<code><span class="reserved">using</span> System;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
<span class="inactive">#nullable</span> <span class="inactive">disable</span>
    <span class="comment">// C# 7.3 以前でコンパイルされたものや、disable なコンテキストで定義されると</span>
    <span class="comment">// アノテーション「未指定」(oblivious)という扱いになる。</span>
    <span class="reserved">string</span> <span class="method">Oblivious</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="variable">s</span>;
 
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
    <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// 未指定。</span>
        <span class="comment">// null チェックの対象にならない(警告出ない)。</span>
        <span class="reserved">var</span> <span class="variable">o</span> = <span class="method">Oblivious</span>(<span class="reserved">null</span>);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">o</span>.Length);
 
        <span class="comment">// たとえ明示的な型で受けても、もうこの変数は oblivious 扱いでチェック対象にならない(警告出ない)。</span>
        <span class="reserved">string</span>? <span class="variable">o1</span> = <span class="method">Oblivious</span>(<span class="reserved">null</span>);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">o1</span>.Length);
    }
}
</code></pre>

###<a id="sec-generated-title-10"></a> <a id="nvt-defference"></a>null 許容値型との違い
null 許容<em>参照</em>型は、
`?` を使う文法こそ[null 許容<em>値</em>型](sp2_nullable.md)と同じですが、
内部的にはだいぶ違う実装になっています。
null 許容参照型の `?` は単なるアノテーション(フロー解析のためのヒント)で、実装上、`T`と`T?`が本質的には同じ型です。
一方で、null 許容値型の `?` は明確に別の型になります(`T?` と書くと`Nullable<T>`型になります)。

この実装上の差から、使い勝手にも差が出てきます。
まず、以下のように、`T` と `T?` でオーバーロードできるのは値型だけです。

<pre class="source" title="オーバーロードの可否">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="comment">// 参照型の場合、アノテーションだけが違うオーバーロードは作れない。</span>
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>) { }
<span class="reserved">void</span> <span class="error"><span class="method">M</span></span>(<span class="reserved">string</span>? <span class="variable">x</span>) { }
 
<span class="comment">// 値型の場合、? が付くと別の型なのでオーバーロードできる。</span>
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>) { }
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>? <span class="variable">x</span>) { }
</code></pre>

また、null チェック後の挙動が違います。
参照型の場合は null チェックさえ挟めば以後「null ではない」という扱いを受けますが、
値型の場合は null チェックを挟んでも `Nullable<T>` は `Nullable<T>` のままです。

<pre class="source" title="null チェック後の挙動">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="comment">// 参照型の場合</span>
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">x</span>)
{
    <span class="comment">// null チェックさえすれば</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>) <span class="control">return</span>;
    <span class="comment">// 警告が消える。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>.Length);
}
 
<span class="comment">// 値型の場合</span>
<span class="reserved">void</span> <span class="method">M</span>(<span class="type">DateTime</span>? <span class="variable">x</span>)
{
    <span class="comment">// null チェックしても</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>) <span class="control">return</span>;
    <span class="comment">// こういう書き方はできない(x?.Minute や x.Value.Minute なら大丈夫)。</span>
    <span class="type">Console</span>.WriteLine(<span class="variable">x</span>.<span class="error">Minute</span>);
}
</code></pre>

null 許容参照型は `typeof` 演算子に対しても使えません。
`T` と `T?` が内部的には同じ型なのに、`typeof(T?)` を認めると混乱の元です。
以下のコードはコンパイル エラーになります。

<pre class="source" title="null 許容参照型に対して typeof を使うとコンパイル エラー">
<code><span class="reserved">var</span> <span class="variable">t</span> = <span class="error"><span class="reserved">typeof</span>(<span class="reserved">string</span>?)</span>;
</code></pre>


<!-- original-page-break -->


##<a id="sec-generated-title-11"></a> <a id="compile"></a>アノテーションのコンパイル結果
null 許容参照型のアノテーションのコンパイル結果は、
`NullableContext`と`Nullable` という2つの属性(いずれも`System.Runtime.CompilerServices`名前空間)を使って表現されます。

2つの属性を使い分けるのはプログラムのサイズを小さくするためです。
属性は付けば付くだけ少しずつプログラムを大きくするため、ちょっとでも付く量を減らす工夫をしています。
例えば以下のようなメソッドを考えます。
引数が4つあって、非nullとnull許容がそれぞれ2つずつになっています。

<pre class="source" title="非null引数が2つ、null許容引数が2つのメソッド">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>, <span class="reserved">string</span> <span class="variable">c</span>, <span class="reserved">string</span>? <span class="variable">d</span>) { }
</code></pre>

初期の案では `Nullable` 属性だけを使って、以下のようにコンパイルする予定でした。

<pre class="source" title="初期案(Nullable のみ)">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>([<span class="type">Nullable</span>(1)]<span class="reserved">string</span> <span class="variable">a</span>, [<span class="type">Nullable</span>(2)]<span class="reserved">string</span> <span class="variable">b</span>, [<span class="type">Nullable</span>(1)]<span class="reserved">string</span> <span class="variable">c</span>, [<span class="type">Nullable</span>(2)]<span class="reserved">string</span> <span class="variable">d</span>) { }
</code></pre>

これだとすべての引数に属性が付くことになります。
その後、少しでも属性の数を減らすために、`NullableContext` 属性が追加され、
以下のようにコンパイルされる仕様になりました。

<pre class="source" title="NullableContext の導入">
<code>[<span class="type">NullableContext</span>(1)]
<span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">a</span>, [<span class="type">Nullable</span>(2)]<span class="reserved">string</span> <span class="variable">b</span>, <span class="reserved">string</span> <span class="variable">c</span>, [<span class="type">Nullable</span>(2)]<span class="reserved">string</span> <span class="variable">d</span>) { }
</code></pre>

`NullableContext` は、クラス内やメソッド内で、`Nullable` 属性が付いていない引数・戻り値をどう扱うかを示しています。
(前述の「[null 許容コンテキスト](#nullable-context)」とは微妙に違う意味で context (文脈)という単語を使ってしまっていますが、
まあどちらも「前後のコードの意味を変える」という意味で「文脈」です。)

この例でいうと、「メソッドに1と付いているので、引数 `a`、`c` は1扱い」ということになります。
メソッドに対する属性が1個増えた代わりに、引数に対する属性が2個減って、全体では属性の数が減りました。

ちなみに、属性の引数になっている1とか2とかの数値は以下の意味になります。
(`Nullable`も`NullableContext`も付いていない場合は0、すなわち oblivious 扱いになります。)

| 値 | 意味 |
| --- | --- |
| 0 | oblivious |
| 1 | 非 null |
| 2 | null 許容 |

属性は、総数が極力少なくなるように付きます。
例えば以下のような2つのメソッドを考えます。

<pre class="source" title="非 null、null 許容の引数の数">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 非 null が2個、null 許容が1個</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>, <span class="reserved">string</span>? <span class="variable">c</span>) { }
 
    <span class="comment">// 非 null が1個、null 許容が2個</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>, <span class="reserved">string</span>? <span class="variable">c</span>) { }
}
</code></pre>

これは、以下のようなコードにコンパイルされます。
要するに、多い方が「context」になることで、属性が必要な引数が減ります。

<pre class="source" title="多い方を Context で指定">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 非 null が多いので NullableContext(1)</span>
    [<span class="type">NullableContext</span>(1)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>, [<span class="type">Nullable</span>(2)] <span class="reserved">string</span> <span class="variable">c</span>) { }
 
    <span class="comment">// null 許容が多いので NullableContext(2)</span>
    [<span class="type">NullableContext</span>(2)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>([<span class="type">Nullable</span>(1)] <span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>, <span class="reserved">string</span> <span class="variable">c</span>) { }
}
</code></pre>

(ちなみに、数が同じ場合は2よりも1を、1よりも0を優先するようです。)

型自体に `NullableContext` が付く例も見てみましょう。
以下のような2つの型を考えます。

<pre class="source" title="型に NullableContext が付く例">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">string</span>? <span class="variable">a</span>) { }
 
    <span class="comment">// 非 null なメソッドが多い</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N1</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N2</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N3</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="comment">// M1, M2 は A と同じ</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">string</span>? <span class="variable">a</span>) { }
 
    <span class="comment">// null 許容なメソッドが多い</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N1</span>(<span class="reserved">string</span>? <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N2</span>(<span class="reserved">string</span>? <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N3</span>(<span class="reserved">string</span>? <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>) { }
}
</code></pre>

この場合、メソッドに付く属性が減るように、クラスに `NullableContext` 属性が付きます。
以下のようなコンパイル結果になります。

<pre class="source" title="型に NullableContext が付いた結果">
<code>[<span class="type">NullableContext</span>(1)]
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
    [<span class="type">NullableContext</span>(2)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N1</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N2</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N3</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
}
 
[<span class="type">NullableContext</span>(2)]
<span class="reserved">class</span> <span class="type">B</span>
{
    [<span class="type">NullableContext</span>(1)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N1</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N2</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">N3</span>(<span class="reserved">string</span> <span class="variable">a</span>, <span class="reserved">string</span> <span class="variable">b</span>) { }
}
</code></pre>

###<a id="sec-generated-title-12"></a> <a id="generic-annotation"></a>型引数に対するアノテーション
[ジェネリクス](../oop/sp2_generics.md)が絡むともう少し複雑になります。
[`dynamic`型の場合](../dynamic/sp4_callsite.md#DynamicAttribute)と同じなんですが、
`Nullable`属性の引数が配列になります。
例えば以下のようなメソッドを考えます。

<pre class="source" title="引数がジェネリックな型の場合">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>?&gt; <span class="variable">a</span>,
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>?&gt;? <span class="variable">b</span>,
    (<span class="reserved">string</span>, <span class="reserved">string</span>, <span class="reserved">string</span>?) <span class="variable">c</span>
    ) { }
</code></pre>

`Dictionary`型やタプルの型引数1個1個で null 許容性が違います。
また、「`Dictionary` 自体」と「`Dictionary` の型引数」でも null 許容性が違っています。
こういう場合には、以下のような属性が付きます。

<pre class="source" title="引数がジェネリックな型の場合の Nullable 属性">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(
    [<span class="type">Nullable</span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { 1, 1, 2 })]
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>?&gt; <span class="variable">a</span>,
    [<span class="type">Nullable</span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { 2, 1, 2 })]
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>?&gt;? <span class="variable">b</span>,
    [<span class="type">Nullable</span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { 0, 1, 1, 2 })]
    (<span class="reserved">string</span>, <span class="reserved">string</span>, <span class="reserved">string</span>?) <span class="variable">c</span>
    ) { }
</code></pre>

配列の最初の要素が型自体で、2個目以降が型引数の null 許容性を表しています。

ちなみに、この他いくつか細かい条件を上げると以下のようなものがあります
(公式ドキュメント: [Nullable Metadata](https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md))。

- 非ジェネリックな値型には属性は付けない
- ジェネリックな値型の場合、0 に続けて型引数の値を並べる
- 型引数が値型のところはスキップ
- 配列中のすべて要素が同じ値のとき、配列ではなく1要素に置き換える
- タプルには元となる`ValueTuple`構造体に準じた属性を付ける

###<a id="sec-generated-title-13"></a> <a id="reflection"></a>Nullable 属性とリフレクション
これで、プログラムのサイズはだいぶ小さくなっています。
しかし、すでに察している人もいるかもしれませんが、
その分、[リフレクション](../dynamic/sp_reflection.md)で null 許容かどうかを取るのがだいぶ面倒になります。

例えば、前述のクラス `A`、`B` のメソッド `M1` の引数を調べたい場合を考えます。
(`M1` に関連する部分を抜粋して再掲します。)

<pre class="source" title="型に NullableContext が付いた結果(M1 がらみを抜粋)">
<code>[<span class="type">NullableContext</span>(1)]
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
}
 
[<span class="type">NullableContext</span>(2)]
<span class="reserved">class</span> <span class="type">B</span>
{
    [<span class="type">NullableContext</span>(1)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">a</span>) { }
}
</code></pre>

ここで、引数 `a` が null 許容かどうか調べようとするとき、

- どちらも引数 `a` 自体には属性が付いていない
- メソッドには `B` の `M1` にだけ属性が付いている
- `A` の場合は型までたどらないと引数 `a` の null 許容性がわからない

ということになります。


<!-- original-page-break -->


##<a id="sec-generated-title-14"></a> <a id="null-forgiving"></a>! 演算子
null 許容なものを、`is null` や `== null` などによるチェック抜きで、
強制的に非 null 扱いしたい場合があります。
原因としては2つあって、以下のような場面で「強制非 null 扱い」が必要になります。

- コンストラクターの時点では非 null 保証が絶対にできない(後からの初期化が必須になる)場合がある
- フロー解析の未熟さからコンパイラーが判定しきれない場合がある

前者のわかりやすい例は循環参照がある場合です。
お互いにインスタンスを持ち合う必要がある場面では、どちらか片方は絶対にコンストラクターよりも後でないとインスタンスを渡せません。

<pre class="source" title="循環参照があるとき、コンストラクターでは非 null 保証ができない">
<code><span class="reserved">class</span> <span class="type">PairedNode</span>
{
    <span class="comment">// このプロパティに対する警告が消せない。</span>
    <span class="reserved">public</span> <span class="type">PairedNode</span> <span class="warning">Pairing</span> { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
 
    <span class="reserved">public</span> <span class="reserved">static</span> (<span class="type">PairedNode</span> a, <span class="type">PairedNode</span> b) <span class="method">Create</span>()
    {
        <span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span> <span class="type">PairedNode</span>();
 
        <span class="comment">// 後から作る方は new の時点でインスタンスを受け取れる。</span>
        <span class="comment">// なのでやろうと思えばコンストラクターにも渡せる。</span>
        <span class="reserved">var</span> <span class="variable">b</span> = <span class="reserved">new</span> <span class="type">PairedNode</span> { Pairing = <span class="variable">a</span> };
 
        <span class="comment">// でも、先に作った方にはどうしても後からの指しなおしが必要。</span>
        <span class="variable">a</span>.Pairing = <span class="variable">b</span>;
 
        <span class="control">return</span> (<span class="variable">a</span>, <span class="variable">b</span>);
    }
}
</code></pre>

後者の例は、例えば `ReferenceEquals` とかです。
null に関するフロー解析は結構ぎりぎりまで作業をしているようで、
`ReferenceEquals` に関する解析は Visual Studio 16.3 Preview 1 (2019年7月)時点では未対応、
Preview 2 (同8月) 時点で初めて対応しました。

<pre class="source" title="ReferenceEquals でも等価チェックになるはずなのに">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span>? <span class="variable">y</span>)
{
    <span class="control">if</span> (<span class="method">ReferenceEquals</span>(<span class="variable">x</span>, <span class="variable">y</span>))
    {
        <span class="comment">// x == y なら警告が消えるのに、ReferenceEquals だと残ってた。</span>
        <span class="comment">// 16.3 Preview 1 の時点では警告あり、Preview 2 から消える。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">y</span></span>.Length);
    }
}
</code></pre>

この例はまだ需要もあって対処も楽な類なので対応されましたが、
もっとレアだったり、対処にコストがかかりすぎる場合は対応してもらえない可能性が高いです。

要するに、null がらみのフロー解析には無理なもの・やっても割に合わないものがざらにあるので、
フロー解析をあえて抑止するような手段が必要になります。

そこで用意されているのが後置き `!` 演算子です。
`a!` というように、式の後ろに `!` を付けると、式 `a` の null 許容性は無視して常に非 null 扱いになります。

<pre class="source" title="! を付けて強制非 null 扱い">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">PairedNode</span>
{
    <span class="comment">// null を無理やり非 null 扱いにして警告を消す。</span>
    <span class="comment">// (省略したものの前述の) Create の中で自己責任で非 null を保証してるので大丈夫。</span>
    <span class="reserved">public</span> <span class="type">PairedNode</span> Pairing { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; } = <em><span class="reserved">null</span>!</em>;
}
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span>? <span class="variable">y</span>)
    {
        <span class="control">if</span> (<span class="method">ReferenceEquals</span>(<span class="variable">x</span>, <span class="variable">y</span>))
        {
            <span class="comment">// string? だけども気にせずメンバー アクセスする。</span>
            <span class="comment">// コンパイラーにはわからないかもしれないけども、人間はこの時点で y が非 null なことを知っている。</span>
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">y</span><em>!</em>.Length);
        }
    }
}
</code></pre>

この `!` 演算子は null forgiving (null に寛大)演算子とか、
null suppression (null 抑止) 演算子などと呼ばれています。
コンパイラーが厳しく(ただ、過剰に)チェックしてくれているものを、あえて緩めておおらかにコードを書く「回避策」的なものなのでこんな呼び名になっています。

(ただ、最近、C# のドキュメントは結構ぎりぎりになるまで正式な用語決定をしないので、
この呼称も最後までこのままかどうかはわかりません。「通称」になる可能性あり。)

ちなみに、`!` 演算子は英語で口頭だと bang operator とか言ったりもするみたいです。
(bang は破裂音の擬音語。「バンと音を立ててびっくりさせる」から、ビックリマークのことを bang と読んだりするそうです。)

(他のプログラミング言語では、「(コンパイラーには無理な) null 判定をプログラマーが明示する」という意味で not-null assertion (非 null 表明)と言ったり、
「強制的に非 null にしてしまう」という意味で force unwrap (強制アンラップ)と言ったりします。)

`!` 演算子を使うと本当に自己責任になります。
フロー解析の対象から外れて、`NullReferenceException` を起こす可能性が出てきます。
また、`!` を書いた地点には特に何も実行時チェックが入りません。
実際に `NullReferenceException` を起こすのはメンバー アクセスした瞬間です。
問題の真の原因と、例外が発生する場所がずれるので注意が必要です。

<pre class="source" title="! 演算子を誤用するとそれなりに面倒事を起こす例">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 悪用して、本当に null を渡してはいけないところに null を渡す。</span>
        <span class="comment">// この時点では例外が出ない。</span>
        <span class="method">M</span>(<span class="reserved">null</span>!);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>)
    {
        <span class="comment">// 実際に NullReferenceException を起こすのは以下の行。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>.Length);
    }
}
</code></pre>

ちなみに、2重に `!` を付けようとするとコンパイル エラーになります。
例えば以下のコードは`x!!` のところでコンパイル エラーが出ます。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">x</span>)
{
    <span class="reserved">var</span> <span class="variable">y</span> = <span class="error"><span class="variable">x</span></span>!!;
}
</code></pre>

##<a id="sec-generated-title-15"></a> <a id="type-constraints"></a>ジェネリクス
[前述の通り](#nvt-defference)、
null 許容型の `T?` は参照型と値型でだいぶ実装方法が違います。
これで特に問題になるのは[ジェネリクス](../oop/sp2_generics.md)です。
型引数には参照型が渡される場合も値型が渡される場合もあって、
そういうときに `T?` の扱いに困ります。

扱いに困るというか、C# 8.0 では制約なしでは `T?` とは書けませんでした。
以下のコードはコンパイル エラーになります。
(後述しますが、C# 9.0 でもこの書き方には注意が必要です。)

<pre class="source" title="制約なしの型引数 T に対して T? は使えない">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">class</span> <span class="type">Generic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// T? と書くと C# 8.0 ではコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="error"><span class="type">T</span></span>? <span class="method">M</span>() =&gt; <span class="reserved">default</span>;
}
</code></pre>

一方、`struct` 制約や `class` 制約、基底クラス制約を付けると `T?` と書けるようになります。
`struct` 制約は [null 許容値型](sp2_nullable.md)の仕様によるもので、C# 2.0 の頃から書けます。
「制約に単に `class` と書くと非 null の意味になる」というのが新仕様になります。

<pre class="source" title="制約を付けて T? を使えるようにできる例">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
 
<span class="comment">// struct 制約を付けると null 許容&quot;値型&quot;を使えるようになる。</span>
<span class="reserved">class</span> <span class="type">StructConstraint</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>
{
    <span class="reserved">public</span> <span class="type">T</span>? <span class="method">M</span>() =&gt; <span class="reserved">default</span>;
}
 
<span class="comment">// class 制約は「非 null 参照型」の意味の制約になる。</span>
<span class="comment">// なので T? と書いて null 許容&quot;参照&quot;型を作れるようになる。</span>
<span class="reserved">class</span> <span class="type">ClassConstraint</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
{
    <span class="reserved">public</span> <span class="type">T</span>? <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="comment">// 基底クラス制約も「非 null」扱い。</span>
<span class="reserved">class</span> <span class="type">BaseTypeConstarint</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Exception</span>
{
    <span class="reserved">public</span> <span class="type">T</span>? <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// class 制約を満たしてる。</span>
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">ClassConstraint</span>&lt;<span class="reserved">string</span>&gt;();
 
        <span class="comment">// class 制約は「非 null」扱いなので以下のコードには警告あり。</span>
        <span class="reserved">var</span> <span class="variable">y</span> = <span class="reserved">new</span> <span class="type">ClassConstraint</span>&lt;<span class="warning"><span class="reserved">string</span>?</span>&gt;();
    }
}
</code></pre>

その代わり、`class`、基底クラス制約に `?` を付けることで null 許容参照型を受け付けることができます。

<pre class="source" title="">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
 
<span class="comment">// class? 制約で「null 許容参照型」を表す。</span>
<span class="reserved">class</span> <span class="type">ClassConstraint</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>?
{
    <span class="comment">// class? な型 T をさらに T? にはできず、コンパイル エラーになる。</span>
    <span class="reserved">public</span> <span class="error"><span class="type">T</span></span>? <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="comment">// 基底クラス制約でも ? を使って null 許容にできる。</span>
<span class="reserved">class</span> <span class="type">BaseTypeConstarint</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Exception</span>?
{
    <span class="comment">// この行がコンパイル エラーになるのは class? 制約と同じ。</span>
    <span class="reserved">public</span> <span class="error"><span class="type">T</span></span>? <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// class? 制約なので特に警告なし。</span>
        <span class="reserved">var</span> <span class="variable">y</span> = <span class="reserved">new</span> <span class="type">ClassConstraint</span>&lt;<span class="reserved">string</span>?&gt;();
    }
}
</code></pre>

###<a id="sec-generated-title-16"></a> <a id="notnull"></a>notnull 制約
また、新たに `notnull` 制約というものが追加されて、
非 null 参照型もしくは非 null 値型のみを受け付けることができます。

<pre class="source" title="notnull 制約">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">class</span> <span class="type">NotNullConstraint</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">notnull</span>
{
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// この2行は OK。</span>
        <span class="reserved">var</span> <span class="variable">ok1</span> = <span class="reserved">new</span> <span class="type">NotNullConstraint</span>&lt;<span class="reserved">int</span>&gt;();
        <span class="reserved">var</span> <span class="variable">ok2</span> = <span class="reserved">new</span> <span class="type">NotNullConstraint</span>&lt;<span class="reserved">string</span>&gt;();
 
        <span class="comment">// この2行には警告が出る。</span>
        <span class="reserved">var</span> <span class="variable">ng1</span> = <span class="reserved">new</span> <span class="type">NotNullConstraint</span>&lt;<span class="warning"><span class="reserved">int</span>?</span>&gt;();
        <span class="reserved">var</span> <span class="variable">ng2</span> = <span class="reserved">new</span> <span class="type">NotNullConstraint</span>&lt;<span class="warning"><span class="reserved">string</span>?</span>&gt;();
    }
}
</code></pre>

例えば、`Dictionary<TKey, TValue>` (`System.Collections.Generic`名前空間)のキーは元々 null を受け付けていません。`d[null] = 0` みたいな書き方をすると null 参照例外が発生します。
なので、.NET Core 3.0 の `Dictionary` の `TKey` には `notnull` 制約が付いています。
`new Dicitionary<int?, string>()` みたいに書くと警告が出るようになります。

ただ、C# 8.0 では `notnull` 制約を付けてもなお、`T?` とは書けません。
(参照型と値型での null 許容の仕様の差が大きすぎてちょっと難しいようです。
もし実現しようと思うなら、C# コンパイラーのレベルでは無理で、.NET ランタイムの型システム レベルでの改修が必要。)

<pre class="source" title="notnull を付けても T? とは書けない">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">class</span> <span class="type">NotNullConstraint</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">notnull</span>
{
    <span class="comment">// 以下の2行はコンパイル エラーになる。</span>
    <span class="error"><span class="type">T</span>?</span> <span class="method">M</span>() =&gt; <span class="error"><span class="reserved">null</span></span>;
    <span class="reserved">int</span> <span class="method">M</span>(<span class="error"><span class="type">T</span>?</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span> ? 0 : <span class="variable">x</span>.<span class="method">GetHashCode</span>();
}
</code></pre>

一応、[次節](#annotation-attributes)で説明する属性を使ってある程度の問題回避はできます。

<pre class="source" title="アノテーション属性(次節で説明)で問題回避">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System.Diagnostics.CodeAnalysis;
 
<span class="reserved">class</span> <span class="type">NotNullConstraint</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">notnull</span>
{
    <span class="comment">// T? と書けないことに対する代替手段。</span>
    [<span class="reserved">return</span>: <span class="type">MaybeNull</span>] <span class="reserved">public</span> <span class="type">T</span> <span class="method">M</span>() =&gt; <span class="reserved">default</span>!;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">M</span>([<span class="type">AllowNull</span>] <span class="type">T</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span> ? 0 : <span class="variable">x</span>.<span class="method">GetHashCode</span>();
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">NotNullConstraint</span>&lt;<span class="reserved">string</span>&gt;();
        <span class="reserved">string</span>? <span class="variable">nullable</span> = <span class="variable">x</span>.<span class="method">M</span>(); <span class="comment">// string M() だけど null が返ってくる。</span>
        <span class="variable">x</span>.<span class="method">M</span>(<span class="reserved">null</span>); <span class="comment">// M(string) だけど null を渡せる。</span>
    }
}
</code></pre>

###<a id="sec-generated-title-17"></a> <a id="unconstrained-generics"></a>制約なしジェネリック型引数
<h5 class="version version9">Ver. 9</h5>

C# 9.0 で、制約なしのジェネリック型引数 `T` に対して `T?` と書けるようになりました。
ジェネリクスの話の冒頭で「C# 8.0 ではエラーになる」と説明した以下のコードが C# 9.0 では有効です。

<pre class="source" title="C# 9.0 で有効になったコード">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">class</span> <span class="type">Generic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// C# 9.0 では一応 T? と書ける。</span>
    <span class="reserved">public</span> <span class="type">T</span>? <span class="method">M</span>() =&gt; <span class="reserved">default</span>;
}
</code></pre>

「一応」と言っているのは、この `T?` にはちょっと注意が必要だからです。
前述のとおり、`T?` は内部実装的に、値型(構造体など)と参照型(クラスなど)とで結構差があって、
その影響で素直に「nullable (null 許容)」と言えるものになっていません。

どちらかというと「defaultable ([規定値](rm_struct.md#default)になる可能性がある)」というべきで、
以下のように、`T?` であっても null にはならない(規定値の 0 になる)ことがあります。

<pre class="source" title="ジェネリックな T? はどちらかというと「defaultable」">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">using</span> System;
 
<span class="comment">// この2つに関しては default == null なので変なことにはならない。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>&lt;<span class="reserved">string</span>?&gt;()); <span class="comment">// null</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>&lt;<span class="reserved">string</span>&gt;()); <span class="comment">// null</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>&lt;<span class="reserved">int</span>?&gt;()); <span class="comment">// null</span>
 
<span class="comment">// 問題が非 null 値型で、この場合 default != null なのでちょっと変。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>&lt;<span class="reserved">int</span>&gt;()); <span class="comment">// 0</span>
 
<span class="comment">// ジェネリックな T? は nullable じゃなくて defaultable。</span>
<span class="comment">// default を渡しても警告にならない。 </span>
<span class="reserved">static</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;() =&gt; <span class="reserved">default</span>;
</code></pre>

これはちょっと罠になるので、検討当初は `T??` みたいな文法で「nullable」と「defaultable」を区別しようかという案も出ていました。
ただ、これはこれで、[`??` 演算子](rm_nullusage.md#null-coalesce)との区別が付かなくて困る場面があるということで断念されました。
他に新しい記号を導入するのも微妙で、結局、「`T?` で defaultable 扱い」という決定が下りました。

##<a id="sec-generated-title-18"></a> <a id="default-constraint"></a>default 制約
<h5 class="version version9">Ver. 9</h5>

[前節の制約なし型引数](#unconstrained-generics)のせいなんですが、
ちょっと限定的な状況でだけ必要となる制約として、`default` 制約というものも増えました。

`default` 制約が必要になるのは以下のような状況です。

<pre class="source" title="default 制約">
<code><span class="inactive">#nullable</span> <span class="inactive">disable</span>
 
<span class="comment">// さかのぼること、null 許容参照型導入前にから以下のような書き方ができた。</span>
<span class="reserved">class</span> <span class="type">Csharp7</span>
{
    <span class="comment">// これは Nullable&lt;T&gt; の意味に。</span>
    <span class="reserved">public</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">x</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span> =&gt; <span class="reserved">null</span>;
 
    <span class="comment">// T と Nullable&lt;T&gt; は別の型扱いなのでオーバーロード可能。</span>
    <span class="reserved">public</span> <span class="type">T</span> <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) =&gt; <span class="reserved">default</span>;
}
 
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="comment">// ここで、null 許容参照型を有効化。</span>
<span class="comment">// 特に、C# 9.0 では制約なし型引数に対して T? と書けるようになったので…</span>
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="comment">// これは Nullable&lt;T&gt; の意味に。</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">t</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span> =&gt; <span class="reserved">null</span>;
 
    <span class="comment">// これは C# 9.0 の制約なし型引数に対する null 許容(正確には default 許容)アノテーション。</span>
    <span class="comment">// T と Nullable&lt;T&gt; 違いのオーバーロードという扱いになる。</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">t</span>) =&gt; <span class="reserved">default</span>;
}
 
<span class="comment">// さらに紛らわしいのが↑を override したときで…</span>
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// これ、実は Nullable&lt;T&gt; の意味。</span>
    <span class="comment">// 親クラス側の where T : struct 制約を自動的に引き継いでしまう。</span>
    <span class="comment">// こうしないと C# 8.0 以前との整合性が取れないとのこと。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">t</span>) =&gt; <span class="reserved">null</span>;
 
    <span class="comment">// ということで、制約なし T? の方を参照するために別の制約が必要になったという経緯があり。</span>
    <span class="comment">// override 時に限り、where T : struct じゃない方に、逆に where T : default という制約を書く必要がある。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">t</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">default</span> =&gt; <span class="reserved">default</span>;
}
</code></pre>

まとめると、

- 古いバージョンとの互換性のため、ジェネリック型引数に対して `T` と `T?` は別の型になっている
- 基底クラス側で `where T : struct` と書いているものは、派生クラスでは改めて `where T : struct` と書かなくてもいい仕様だった
- C# 9.0 で制約なし型引数に対しても `T?` と書けるようになったことで、派生クラス側の挙動が怪しくなった
- この問題を回避するため、派生クラス側には `where T : default` という制約を書く必要がある

という感じです。
前節で説明した通り、制約なしの型引数に対する `T?` は「null 許容」というよりは「default 許容」(defaultable)なので、`where T : default` というキーワードを用います。

<!-- original-page-break -->

##<a id="sec-generated-title-19"></a> <a id="annotation-attributes"></a>アノテーション属性
[前節](#type-constraints)のジェネリクスの問題を筆頭に、
いくつか、`T?` という記法だけでは解決できない問題があります。
ジェネリックな型でなくても例えば以下のような場合に、`?` の有無だけではフロー解析がうまく働きません。

- プロパティの get と set で null 許容性が違う場合がある
- [参照引数](sp_ref.md#sec-byref)で、「null が渡ってきてもいいけど、非 null な値で必ず上書きする」みたいな挙動があり得る
- `TryGetValue` のように、戻り値が true の時だけ非 null な値を返す[出力引数](sp_ref.md#out)がある
- 「引数が null の場合に限り戻り値も null」みたいな場合がある

こういう場合への対処としていくつか、[属性](../dynamic/sp_attribute.md)によってフロー解析を制御する手段が用意されています。
いずれの属性も`System.Diagnostics.CodeAnalysis`名前空間で定義されています。

<table>
<caption>.NET Core 3.0 からあるもの</caption>
<tr>
<th>分類</th><th>属性名</th><th>概要</th>
</tr>
<tr>
<td rowspan="2">事前条件</td>
<td><code>AllowNull</code></td>
<td>(<code>T</code> であっても)入力として null を受け付ける</td>
</tr>
<tr>
<td><code>DisallowNull</code></td>
<td>(<code>T?</code> であっても)入力として null を受け付けない</td>
</tr>
<tr>
<td rowspan="2">事後条件</td>
<td><code>MaybeNull</code></td>
<td>(<code>T</code> であっても)出力として null を返す</td>
</tr>
<tr>
<td><code>NotNull</code></td>
<td>(<code>T?</code> であっても)出力として null を返さない<sup>※</sup></td>
</tr>
<tr>
<td rowspan="2">条件付き<br/>事後条件</td>
<td><code>MaybeNullWhen</code></td>
<td>戻り値が true/false どちらかの時だけ <code>MaybeNull</code> 使い</td>
</tr>
<tr>
<td><code>NotNullWhen</code></td>
<td>戻り値が true/false どちらかの時だけ <code>NotNull</code> 使い</td>
</tr>
<tr>
<td>null 依存性</td>
<td><code>NotNullIfNotNull</code></td>
<td>引数が null の時に限り戻り値が null</td>
</tr>
<tr>
<td rowspan="2">フロー</td>
<td><code>DoesNotReturn</code></td>
<td>このメソッドを呼んだらもう戻ってこないという意味で、それ以降のフロー解析をしない</td>
</tr>
<tr>
<td><code>DoesNotReturnIf</code></td>
<td>引数が true/false どちらかの時だけ <code>DoesNotReturn</code> 扱い</td>
</tr>
</table>

<table>
<caption>.NET 5 からあるもの</caption>
<tr>
<th>分類</th><th>属性名</th><th>概要</th>
</tr>
<tr>
<td rowspan="2">他のメンバー</td>
<td><code>MemberNotNull</code></td>
<td>この属性が付いたメンバーを呼んだ時点で、他のメンバーの非 null が確定する</td>
</tr>
<tr>
<td><code>MemberNotNullWhen</code></td>
<td>この属性が付いたメンバーを呼ばれて、かつ、戻り値が特定の値だった時点で、他のメンバーの非 null が確定する</td>
</tr>
</table>

<sup>※</sup> [`out`引数](sp_ref.md#out)に対しては「メソッド内で非 null な値を代入している」、
通常の引数や[`in`引数](sp_ref.md#in)に対しては「もし null が渡ってきたら例外を出すなど、それ以降の処理を続行させない」という扱い。

###<a id="sec-generated-title-20"></a> <a id="attribute-usage"></a>アノテーション属性の利用例
これらの属性が必要になる具体例をいくつか紹介していきましょう。

#### <a id="sec-generated-title-21"></a>Array.Resize (NotNull)
まず、[`Array.Resize`](https://docs.microsoft.com/ja-jp/dotnet/api/system.array.resize) は配列の長さを変更するメソッドですが、参照引数で null を受け付けはするものの、絶対に非 null なインスタンスを作って渡します。そこで、以下のように、`NotNull` 属性が付いています。

<pre class="source" title="ref の入力と出力で null 許容性が違う例">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Array</span>
{
    <span class="comment">// null を受け付けるけど、返しはしない。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Resize</span>&lt;<span class="type">T</span>&gt;([<span class="type">NotNull</span>] <span class="reserved">ref</span> <span class="type">T</span>[]? <span class="variable">array</span>, <span class="reserved">int</span> <span class="variable">newSize</span>);
}
</code></pre>

その結果、以下のようなコードが書けます。

<pre class="source" title="Array.Resize の AllowNull の効果">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// null を渡せる。</span>
        <span class="reserved">int</span>[]? <span class="variable">array</span> = <span class="reserved">null</span>;
        <span class="type">Array</span>.<span class="method">Resize</span>(<span class="reserved">ref</span> <span class="variable">array</span>, 4);
 
        <span class="comment">// でも、呼び出し後は非 null 保証がある。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">array</span>.Length); <span class="comment">// 警告なし</span>
    }
}
</code></pre>

#### <a id="sec-generated-title-22"></a>TextWriter.NewLine (AllowNull)
[`TextWriter.NewLine`](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.textwriter.newline) は get で null を返すことはありません。
しかし、「null を set すると [`Environment.NewLine`](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.textwriter.newline) を使う」という仕様があって、set だけが null 許容です。
そこで、以下のように、`AllowNull` が付いています。
(`AllowNull` は意味としては「入力(引数とか)に `null` を許す」なので、プロパティに付けると `set` の `value` が nullable の意味になるみたいです。)

<pre class="source" title="set と get で null 許容性が違う例">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">TextWriter</span>
{
    [<span class="type">AllowNull</span>] <span class="comment">// set だけ null 許容</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">string</span> NewLine
    {
        <span class="reserved">get</span> =&gt; ...
        <span class="reserved">set</span> =&gt; ...
    }
}
</code></pre>

#### <a id="sec-generated-title-23"></a>ジェネリック型引数に対するアノテーション (MeybeNull)
ジェネリクス都合で `T?` と書けない問題を `MaybeNull` 属性で回避している例としては
[`StrongBox<T>.Value`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.strongbox-1.value)や[`ThreadLocal<T>.Value`](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.threadlocal-1.value)などがあります。

<pre class="source" title="ジェネリクス都合で MaybeNull">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">StrongBox</span>&lt;<span class="type">T</span>&gt;
{
    [<span class="type">MaybeNull</span>] <span class="reserved">public</span> <span class="type">T</span> Value =&gt; ...
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">ThreadLocal</span>&lt;<span class="type">T</span>&gt;
{
    [<span class="type">MaybeNull</span>] <span class="reserved">public</span> <span class="type">T</span> Value =&gt; ...
}
</code></pre>

#### <a id="sec-generated-title-24"></a>Try メソッド (NotNullWhen)
.NET には名前が `Try` から始まって、処理の成否を `bool` で返すメソッドが結構多いですが、
こういう場合「戻り値が true の時だけ null でない値を取れる」ということが多いです。
例えば、[Version.TryParse](https://docs.microsoft.com/ja-jp/dotnet/api/system.version.tryparse)が該当します。
また、[`string.IsNullEmpty`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.isnullorempty) のように、他の処理と兼ねて null チェックしているものがあります。
こういう場合に `NotNullWhen` などの条件付き事後条件を使います。

<pre class="source" title="条件付き事後条件の例">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Version</span>
{
    <span class="comment">// 戻り値が true の時には非 null 値を version 変数に入れて返す。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">TryParse</span>(
        <span class="reserved">string</span>? <span class="variable">input</span>,
        [<span class="type">NotNullWhen</span>(<span class="reserved">true</span>)] <span class="reserved">out</span> <span class="type">Version</span>? <span class="variable">version</span>);
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">String</span>
{
    <span class="comment">// 中で null チェックをしているので、true を返すなら value は非 null とわかる。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">IsNullOrEmpty</span>([<span class="type">NotNullWhen</span>(<span class="reserved">false</span>)] <span class="reserved">string</span>? <span class="variable">value</span>);
}
</code></pre>

#### <a id="sec-generated-title-25"></a>null 伝搬 (NotNullIfNotNull)
[Path.GetFileName](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.path.getfilename)など、単純に null を伝搬する(null が来たら素通しで null を返す)ようなメソッドも多いです。
また、[Volatile.Read](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.volatile.read)/[Write](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.volatile.write)のように、引数の値を戻り値や他の参照引数に伝搬するものがあって、値の伝搬によって null 許容性も伝搬します。
こういう場合に使うのが `NotNullIfNotNull` 属性です。

<pre class="source" title="null 許容性の伝搬">
<code><span class="reserved">class</span> <span class="type">Path</span>
{
    <span class="comment">// 引数が null のとき、戻り値に null を素通しする仕様。</span>
    [<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="string">&quot;path&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span>? <span class="method">GetFileName</span>(<span class="reserved">string</span>? <span class="variable">path</span>);
}
 
<span class="reserved">class</span> <span class="type">Volatile</span>
{
    <span class="comment">// location に value を書き込むメソッドなので、value の null 判定が location に伝搬。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Write</span>&lt;<span class="type">T</span>&gt;([<span class="type">NotNullIfNotNull</span>(<span class="string">&quot;value&quot;</span>)] <span class="reserved">ref</span> <span class="type">T</span> <span class="variable">location</span>, <span class="type">T</span> <span class="variable">value</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>?;
 
    <span class="comment">// location に入っている値をそのまま返すメソッドなので、location の null 判定が戻り値に伝搬。</span>
    [<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="string">&quot;location&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> <span class="method">Read</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> <span class="variable">location</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>?;
}
</code></pre>

(ちなみに、この例の `"path"` や `"location"` は `nameof(path)`、`nameof(location)` と書きたいところですが、[`nameof` 演算子](../start/st_string.md#nameof-operator)の仕様上、メソッドの外から引数を参照することは残念ながらできません。
この `NotNullIfNotNull` 属性によってそれなりに強い需要が生じてしまったので修正が入る可能性はありますが、破壊的変更になりそうなのであんまり期待はできません。)

#### <a id="sec-generated-title-26"></a>FailFast (DoesNotReturn)
一部のメソッドは、そのメソッドを呼んだら最後、もう絶対に正常には戻ってこないものがあります。例えば[Environment.FailFast](https://docs.microsoft.com/ja-jp/dotnet/api/system.environment.failfast)はプログラムを即座に止めてしまう(おかしな状態のままプログラムが進むよりは、一思いにクラッシュした方がマシな場面で使う)メソッドなので、このメソッドの呼び出しから後ろが実行されることは絶対にありません。
こういう場合、フロー解析もそのメソッドまでで止めてしまいたく、そのために使う属性が `DoesNotReturn` です。

<pre class="source" title="呼んだら最後、絶対に戻ってこないメソッド">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Environment</span>
{
    [<span class="type">DoesNotReturn</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">FailFast</span>(<span class="reserved">string</span> <span class="variable">message</span>);
}
</code></pre>

これは以下のような使い方を想定しています。

<pre class="source" title="DoesNotReturn 付きメソッドの利用例">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">null</span>)
    {
        <span class="type">Environment</span>.<span class="method">FailFast</span>(<span class="string">&quot;null は許さない。絶対にだ！&quot;</span>);
    }
 
    <span class="comment">// null だったら FailFast 行きで、FailFast は DoesNotReturn なので、</span>
    <span class="comment">// ここに来た時点で s は非 null な保証がある。</span>
    <span class="control">return</span> <span class="variable">s</span>.Length;
}
</code></pre>

プログラムのクラッシュの他、絶対に例外を出すことがわかっているメソッドにも `DoesNotReturn` 属性が使えます。

<pre class="source" title="絶対に例外を出すメソッドにも DoesNotReturn が使える">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">null</span>)
    {
        <span class="method">Throw</span>(<span class="reserved">nameof</span>(<span class="variable">s</span>));
    }
 
    <span class="control">return</span> <span class="variable">s</span>.Length;
}
 
<span class="comment">// throw はインライン展開を阻害するのでここだけメソッドを分離</span>
[<span class="type">DoesNotReturn</span>]
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Throw</span>(<span class="reserved">string</span> <span class="variable">name</span>) =&gt; <span class="control">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="variable">name</span>);
</code></pre>

#### <a id="sec-generated-title-27"></a>Assert (DoesNotReturnIf)
同じプログラムのクラッシュでも、条件付きな場合があります。
[`Debug.Assert`](https://docs.microsoft.com/ja-jp/dotnet/api/system.diagnostics.debug.assert)がわかりやすいでしょう。
このメソッドは引数が false の時に限ってプログラムを止めます。
こういうメソッドに対して使うがの `DoesNotReturnIf` 属性です。

<pre class="source" title="条件次第で戻ってこなくなるメソッドの例">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Debug</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Assert</span>([<span class="type">DoesNotReturnIf</span>(<span class="reserved">false</span>)] <span class="reserved">bool</span> <span class="variable">condition</span>);
}
</code></pre>

ちなみに、「絶対に戻ってこないからフロー解析をしなくていい」という処理は、
null 許容性の他に[確実な初期化](rm_struct.md#definite-assignment)でも使いたいものです。
ただ、`DoesNotReturn`/`DoesNotReturnIf` 属性は null に関してしか働きません。
(確実な初期化の方がシビアな判定をすべき(でないとセキュリティ ホールになりえる)もので、
C# コンパイラーのフロー解析だけじゃなく .NET ランタイムのレベルでも検証をしたいけど、そこまで実装する余裕がないからという理由。)

##<a id="sec-generated-title-28"></a> <a id="special-treatment"></a>特殊扱いされるメソッド
前節で紹介した属性を使うことで、いろいろな状況に対応可能です。
しかし、「属性を使って汎用的に解決するほどの需要がない」ということで、
1つ1つ特別扱いすることでフロー解析しているメソッドがいくつかあります。

以下のようなものが該当します(要するに、`==` の代用になる類のメソッドです)。

- [`object.Equals`](https://docs.microsoft.com/ja-jp/dotnet/api/system.object.equals)
- [`object.ReferenceEquals`](https://docs.microsoft.com/ja-jp/dotnet/api/system.object.referenceequals)
- [`IEqualityComparer<T>.Equals`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.iequalitycomparer-1.equals)
- [`IEquatable<T>.Equals`](https://docs.microsoft.com/ja-jp/dotnet/api/system.iequatable-1.equals)
- [`Interlocked.CompareExchange`](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.interlocked.compareexchange)

これらはちゃんと、`==` 演算子と同様、null 許容性を伝搬します。
例えば以下のように、`EqualityComparer<T>.Default.Euqlas` を使って null チェックができます。

<pre class="source" title="">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">EqualityComaprerEquals</span>(<span class="reserved">string</span> <span class="variable">x</span>, <span class="reserved">string</span>? <span class="variable">y</span>)
{
    <span class="comment">// IEqualityComparer.Equals は == と同じ扱いを受ける。</span>
    <span class="control">if</span> (<span class="type">EqualityComparer</span>&lt;<span class="reserved">string</span>&gt;.Default.<span class="method">Equals</span>(<span class="variable">x</span>, <span class="variable">y</span>))
    {
        <span class="comment">// こっちは y が非 null なことがわかるので警告が出ない。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">y</span>.Length);
    }
    <span class="control">else</span>
    {
        <span class="comment">// こっちは null な可能性が残るので警告が出る。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">y</span>.Length);
    }
}
</code></pre>

##<a id="sec-generated-title-29"></a> <a id="gradual"></a>段階的な改善
null 許容参照型はそれなりの期間を掛けて徐々に完成していく予定です。
以下の2つの意味で、少しずつ警告が増えたり減ったりします。

- C# コンパイラーのフロー解析の精度が上がる
- .NET Core の基本ライブラリに正しくアノテーション属性が付く

[`!` 演算子](#null-forgiving)の説明でも出てきましたが、
フロー解析はそれなりに労力がかかり、完璧なものは作れません。
バージョンアップとともに少しずつ精度が上がっていくものと思われます。

ほとんどの場合は「過剰に警告が出てしまっていて、それを `!` 演算子で抑止している状態」が解消できるもので、
精度が上がれるほど警告が減る方に変化すると思われます。

###<a id="sec-generated-title-30"></a> <a id="array-element"></a>配列の要素のフロー解析
しかし一部は、もしかすると<em>警告が増える</em>ことが考えられます。

例えば今「抜け穴になっていることはわかっているけど見逃している」状態なのが配列の要素の初期化です。
以下のコードは、フロー解析の漏れであって、可能であれば警告を出したいコードです。
(コンストラクター内で全要素に対して 非 null 初期化しているかどうかまで解析したい。)
しかし、少なくとも C# 8.0 時点では警告を出せません。

<pre class="source" title="C# 8.0 時点でのフロー解析の不足の例">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">ArrayInit</span>
{
    <span class="reserved">string</span>[] _buffer;
 
    <span class="reserved">public</span> <span class="type">ArrayInit</span>()
    {
        <span class="comment">// _buffer 自体には new string[] を代入したけど、その要素には何も代入していない。</span>
        <span class="comment">// C# の仕様上、_buffer[0] は null になってる(おかしい)。</span>
        <span class="comment">// string (? を付けていない)なので null になってはいけないはず。</span>
        _buffer = <span class="reserved">new</span> <span class="reserved">string</span>[1];
    }
 
    <span class="comment">// string[] からの要素の取り出しなので、string (非 null)のはず。</span>
    <span class="comment">// 警告は出ない。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> Value =&gt; _buffer[0];
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">ArrayInit</span>();
        <span class="reserved">string</span> <span class="variable">s</span> = <span class="variable">x</span>.Value;
 
        <span class="comment">// どこにも警告が出ないものの、実行するとここで null 参照例外発生。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
    }
}
</code></pre>

###<a id="sec-generated-title-31"></a> <a id="patch-version-up"></a>C# バージョン変更なしでのフロー解析の改善
フロー解析の改善は、
C# の文法に追加があるわけではなく単に警告の増減なこともあって、
C# のバージョン変更なし(パッチ バージョンアップ)で機能が増えたりします。

####<a id="sec-generated-title-32"></a> <a id="attribute-affect"></a>アノテーション属性のメソッド内への影響
C# 8.0 のリリース直後の時点では、
null 許容性に関する属性はメソッドの外に対してだけ影響を及ぼしていました。
以下のように、メソッド内ではフロー解析に寄与していませんでした。

<pre class="source" title="アノテーション属性の影響はメソッド内部には及んでなかった(リリース当初)">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
<span class="reserved">using</span> System.Diagnostics.CodeAnalysis;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// メソッドを作る側(メソッドの中)には影響していない。</span>
    [<span class="reserved">return</span>: <span class="type">MaybeNull</span>]
    <span class="reserved">static</span> <span class="reserved">string</span> <span class="method">M</span>() =&gt; <span class="warning"><span class="reserved">null</span></span>; <span class="comment">// ここで警告が出る。</span>
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// メソッドを使う側(メソッドの外)にはちゃんと影響してる。</span>
        <span class="reserved">var</span> <span class="variable">s</span> = <span class="method">M</span>();
 
        <span class="comment">// MaybeNull なのに null チェックしていないのでここで警告。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="warning"><span class="variable">s</span>.Length</span>);
    }
}
</code></pre>

外から見た都合(メソッドを使う側)の方が大事なので優先的に実装された結果です。
当初、`null` 戻り値のところに [`!` 演算子](#null-forgiving)を付けて警告を回避するしかありませんでした。

この挙動は Visual Studio 16.6 (2020年5月リリース)で改善されていて、今はもうメソッド `M` の定義側の警告は出ません
(ちゃんと、`MaybeNull` 属性を解釈して `null` 戻り値を許す)。
「C# 8.1」になったとかではなく、「C# 8.0」のまま、フロー解析だけ改善されています。

####<a id="sec-generated-title-33"></a> <a id="MemberNotNull"></a>MemberNotNull 属性の追加
`MemberNotNull`と `MemberNotNullWhen` 属性のフロー解析も Visual Studio 16.6 (2020年5月リリース)で追加されています。

`MemberNotNull` 属性は、あるメンバー(メソッドやプロパティ)を呼んだ時点で、
別のメンバーが非 null であることを決定するための属性です。

例えば以下のような状況を考えます
(実際、標準ライブラリの [`DeflateStream`](https://docs.microsoft.com/ja-jp/dotnet/api/system.io.compression.deflatestream)クラスに似たようなコードが入っています)。

<pre class="source" title="間接的な初期化をしているフィールド">
<code><span class="reserved">class</span> <span class="type">DeflateStream</span>
{
    <span class="reserved">private</span> <span class="type">Stream</span> _stream; <span class="comment">// コンストラクターで初期化していないので警告が出る。</span>
 
    <span class="reserved">public</span> <span class="warning"><span class="type">DeflateStream</span></span>(<span class="type">Stream</span> stream)
    {
        <span class="method">Initialize</span>(stream);
    }
 
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>(<span class="type">Stream</span> stream)
    {
        _stream = stream;
    }
}
</code></pre>

`Initialize` メソッドを介して間接的には非 null なフィールドをちゃんと初期化しているんですが、
これまでだとこの状況を正しくフロー解析する手段がありませんでした。
これに対して、`MemberNotNull` 属性が追加されたことで以下のように書けるようになりました。

<pre class="source" title="MemberNotNull で警告消し">
<code><span class="reserved">class</span> <span class="type">DeflateStream</span>
{
    <span class="reserved">private</span> <span class="type">Stream</span> _stream; <span class="comment">// Initialize 内で初期化される。</span>
 
    <span class="reserved">public</span> <span class="type">DeflateStream</span>(<span class="type">Stream</span> stream)
    {
        <span class="comment">// Initialize 内で _stream が初期化されることがわかるので警告が消える。</span>
        <span class="method">Initialize</span>(stream);
    }
 
    <span class="comment">// この属性によって正しくフロー解析できるようになってる。</span>
    [<span class="type">MemberNotNull</span>(<span class="reserved">nameof</span>(_stream))]
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Initialize</span>(<span class="type">Stream</span> stream)
    {
        _stream = stream;
    }
}
</code></pre>


###<a id="sec-generated-title-34"></a> <a id="over-a-period"></a>移行期間
.NET Core 側としても、基本クラス ライブラリに膨大な数のクラス、メソッドがあり、
1度のリリースですべてにアノテーションを付けることは不可能です。
なので、段階的にアノテーションが増える予定です。

実際例えば、LINQ to Object (`Enumerable`クラス(`System.Linq` 名前空間の各種拡張メソッド)には .NET Core 3.0 (C# 8.0 と同世代)時点では[アノテーション属性](#annotation-attributes)が付いていません。

<pre class="source" title=".NET Core 3.0 時点のアノテーション不足の例">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
<span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 以下のコードは null 参照例外を起こすんだから、ToDictionary には DisallowNull 属性が付くべき。</span>
        _ = <span class="reserved">new</span>[] { <span class="string">&quot;&quot;</span>, <span class="reserved">null</span> }.<span class="method">ToDictionary</span>(<span class="variable">x</span> =&gt; <span class="variable">x</span>);
 
        <span class="comment">// 以下のコードは null を返してくるんだから、FirstOrDefault には MaybeNull 属性が付くべき。</span>
        <span class="reserved">string</span> <span class="variable">s</span> = <span class="reserved">new</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;b&quot;</span> }.<span class="method">FirstOrDefault</span>(<span class="variable">x</span> =&gt; <span class="variable">x</span>.Length &gt; 2);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
    }
}
</code></pre>

これらについては、後からアノテーションが増える予定です。

フロー解析の発達にしろアノテーションの追加にしろ、
いずれもあとから警告が増える可能性があるという点に注意してください。
しばらくの間、「移行期だから仕方がない」と受け入れてもらうしかなさそうです。

(通常、C# は警告の追加すらも「破壊的変更になるから」という理由で避ける文化のプログラミング言語です。
[opt-inであること](#opt-in)と同様、段階移行も苦渋の選択です。)
