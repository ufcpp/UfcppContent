---
title: "構造体"
source_url: "https://ufcpp.net/study/csharp/resource/rm_struct/"
content_type: "Article"
published_at: "2015-07-15T00:00:00"
updated_at: "2017-11-04T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 6.0"
umbraco_id: 1773
parent_id: 1286
sort_order: 1
aliases:
  - "/csharp/resource/rm_struct/"
---

# 構造体

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[データの構造化](../structured/st_struct.md)」で少し触れて以来、ずっとクラスだけを使って説明してきましたが、ここで、C#における「もう1つの複合型」である構造体について説明します。

根本的な差は、次項で説明する「[値型](oo_reference.md#valtype)」か「[参照型](oo_reference.md#reftype)」かの違いに起因します。構造体は値型で、そのためにクラスと比べていくつか用途に制限がかかります。

##<a id="sec-generated-title-2"></a> <a id="restriction"></a>構造体の制限
とりあえず、クラスと構造体を並べて書いてみましょう。

#### <a id="sec-generated-title-3"></a>構造体
<pre class="source" title="">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">SampleStruct</span> : <span class="type">InterfaceA</span>, <span class="type">InterfaceB</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> A { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> B { <span class="reserved">get</span>; }

    <span class="reserved">public</span> SampleStruct(<span class="reserved">int</span> a, <span class="reserved">int</span> b) { A = a; B = b; }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">SampleStruct</span> <span class="reserved">operator</span>-(<span class="type">SampleStruct</span> x)
        =&gt; <span class="reserved">new</span> <span class="type">SampleStruct</span>(-x.A, -x.B);
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">InterfaceA</span> { <span class="reserved">int</span> A { <span class="reserved">get</span>; } }
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">InterfaceB</span> { <span class="reserved">int</span> B { <span class="reserved">get</span>; } }
</code></pre>

#### <a id="sec-generated-title-4"></a>クラス
<pre class="source" title="">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">SampleClass</span> : <span class="type">BaseClass</span>, <span class="type">InterfaceA</span>, <span class="type">InterfaceB</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> A { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> B { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> SampleClass() { }
    <span class="reserved">public</span> SampleClass(<span class="reserved">int</span> a, <span class="reserved">int</span> b) { A = a; B = b; }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">SampleClass</span> <span class="reserved">operator</span> -(<span class="type">SampleClass</span> x)
        =&gt; <span class="reserved">new</span> <span class="type">SampleClass</span>(-x.A, -x.B);

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> X() { }

    ~SampleClass() { }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">BaseClass</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> X() { }
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">StaticClass</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span> Hex(<span class="reserved">int</span> x) =&gt; x.ToString(<span class="string">"X"</span>);
}
</code></pre>

単純に、クラスの方ができることは多いです。

クラスにしかできないことは以下の通り。

- 他のクラスから派生(他のクラスを継承)する
  - 継承に関連する修飾子(`abstract`, `sealed`, `virtual`, `override`など)を使えるもクラスだけ
- 静的クラス
- 引数なしのコンストラクターの定義(C# 9.0 まで)
- ファイナライザーの定義

一方、クラス・構造体どちらでもできることは以下のとおりです。

- 引数なしコンストラクターとファイナライザー以外のメンバー定義
- インターフェイスの実装(複数も可)
- (構造体自身には`static`修飾子を付けれないものの)静的メンバーの定義自体は可能

##<a id="sec-generated-title-5"></a> <a id="usage"></a>構造体の用途
「できること」でいうと、構造体はクラスの完全下位互換で、メリットがないように見えます。構造体の利点を理解するには、次項の[値型と参照型](oo_reference.md)についての知識が必要になります。

おおまかに言うと、

- クラスと構造体ではメモリの使い方が違う
- 小さなデータ構造に対しては構造体が有利
  - 使い方にもよりますが、大まかなガイドラインとしては16バイト程度が境界と言われています

というものです。

この性質と、前節で説明した制限とを併せて考えると、構造体の利用を検討するのは、

- データ構造が16バイト未満
- 継承が必要ない

という状況下になります。

##<a id="sec-generated-title-6"></a> <a id="default"></a>構造体の既定値
これも[値型](oo_reference.md#valtype)の性質になりますが、
クラス(`new`するまでメモリ領域を確保しない)と違って、
構造体は宣言した時点でデータを記録するためのメモリ領域が確保されます。

クラス型のフィールドの場合は、`new`するなり他のインスタンスを代入するなりして初期化するまでの間、
`null` (何のインスタンスも指していない状態)が入ります。

一方、構造体の場合、いわゆる「0初期化」状態になっています。
全てのメンバーに対して、0、もしくはそれに類する以下のような値が入ります。

- 数値型(`int`, `double`など)の場合は0
  - 列挙型も、0 に相当する値
- `bool` 型の場合は `false`
- 参照型(`string`、配列、クラス、デリゲートなど)や[Null許容型](sp2_nullable.md#nullableType)の場合は `null`

これら、0初期化状態にある値を、<em>構造体の既定値</em>(default value)と呼びます。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> I;
    <span class="reserved">public</span> <span class="reserved">double</span> D;
    <span class="reserved">public</span> <span class="reserved">bool</span> B;
    <span class="reserved">public</span> <span class="reserved">string</span> S;
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="type">Sample</span> s;

    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="type">Console</span>.WriteLine(s.I);
        <span class="type">Console</span>.WriteLine(s.D);
        <span class="type">Console</span>.WriteLine(s.B);
        <span class="type">Console</span>.WriteLine(s.S);
    }
}
</code></pre>

<pre class="console" title="">
<code>0
0
False

</code></pre>

##<a id="sec-generated-title-7"></a> <a id="parameterless-ctor"></a>引数なしコンストラクター
C# 9.0 まで、構造体のメンバーとして引数なしのコンストラクターを書くことはできませんでした。
これは、`new T()`を[既定値](rm_default.md#default-keyword)(0初期化)として使っていたせいです。

例えば以下のコードでは、`Point`クラスには引数なしのコンストラクターを定義していませんが、
`new Point()`という書き方で 0 初期化を行っています。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { X = x; Y = y; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; <span class="string">$"(</span>{X}<span class="string">, </span>{Y}<span class="string">)"</span>;
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> p1 = <span class="reserved">new</span> <span class="type">Point</span>(); <span class="comment">// 既定値、つまり、「XもYも0に初期化」という意味で使われる</span>
        <span class="reserved">var</span> p2 = <span class="reserved">new</span> <span class="type">Point</span>(10, 20);
        <span class="reserved">var</span> p3 = <span class="reserved">default</span>(<span class="type">Point</span>); <span class="comment">// C# 2.0～9.0 まで、p1と同じ意味</span>

        <span class="type">Console</span>.WriteLine(p1);
        <span class="type">Console</span>.WriteLine(p2);
        <span class="type">Console</span>.WriteLine(p3);
    }
}
</code></pre>

<pre class="console" title="">
<code>(0, 0)
(10, 20)
(0, 0)
</code></pre>

<h5 class="version version2">Ver. 2.0</h5>

ちなみに、C# 2.0 以降では、構造体の既定値は、`new T()`という書き方の他に、`default(T)`という書き方もできます。
(主に[ジェネリック](../oop/sp2_generics.md)のために導入された構文です。)

既定値について、詳しくは別項「[既定値](rm_default.md#default-constructor)」で説明します。

<h5 class="version version10">Ver. 10</h5>

C# 2.0 で `default(T)` を使った既定値(0初期化)ができるようになって、
「`new T()` と書く場合は引数なしコンストラクターを呼ぶ」という仕様に変えたい
(構造体にも引数なしコンストラクターを書けるようにして、`new T()` と `default(T)` を区別する)
という話は前々からありました。

C# 10.0 で、ついにその案が採用されることになり、
引数なしコンストラクターを書けるようになりました。
例えば以下のようなコードが書けるようになります。

<pre class="source" title="構造体の引数なしコンストラクターの例">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="type">A</span>() =&gt; X = 1;
}
</code></pre>

これで、`new A()` で `X` が1になります。

###<a id="sec-generated-title-8"></a> <a id="new-or-default"></a>new() と default
背景説明の通り、`new()` と `default` の意味が変わったので注意が必要です。
この例の構造体 `A` の場合、以下のような挙動になります。

<pre class="source" title="new A() と default(A)">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">A</span>().X); <span class="comment">// コンストラクターが呼ばれて、X == 1 になってる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">default</span>(<span class="type">A</span>).X); <span class="comment">// コンストラクターも呼ばれず 0 初期化で、X == 0 になってる。</span>
</code></pre>

C# 7.1/9.0 で、`new()` や `default` に[ターゲット型からの推論](../start/misctyperesolution.md#target-type)が働くようになったので、以下のようにも書けます。

<pre class="source" title="new() と default">
<code><span class="type">A</span> a = <span class="reserved">new</span>();
<span class="type">Console</span>.<span class="method">WriteLine</span>(a.X); <span class="comment">// 1</span>

a = <span class="reserved">default</span>;
<span class="type">Console</span>.<span class="method">WriteLine</span>(a.X); <span class="comment">// 0</span>
</code></pre>

`default` を書く以外に、配列の要素も既定値(0初期化)になるので注意が必要です。

<pre class="source" title=" 配列の要素は暗黙的に default">
<code><span class="comment">// 配列の要素は暗黙的に default…</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>((<span class="reserved">new</span> <span class="type">A</span>[1])[0].X); <span class="comment">// default(A) と同じ扱いで、X == 0 になってる。</span>
</code></pre>

ちなみに、ジェネリクス越しでも `new()` と `default` の呼び分けが掛かります。

<pre class="source" title="ジェネリクス越しの new() と default">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">New</span>&lt;<span class="type">A</span>&gt;().X); <span class="comment">// 1</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">Default</span>&lt;<span class="type">A</span>&gt;().X); <span class="comment">// 0</span>

<span class="reserved">static</span> <span class="type">T</span> <span class="method">New</span>&lt;<span class="type">T</span>&gt;() <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>() =&gt; <span class="reserved">new</span>();
<span class="reserved">static</span> <span class="type">T</span>? <span class="method">Default</span>&lt;<span class="type">T</span>&gt;() =&gt; <span class="reserved">default</span>;
</code></pre>

また、これまで `default` と同じ意味だった `new()` が、引数なしコンストラクターの有無で違う意味になるのでこの点にも注意が必要です。
例えば、一般の構造体で[オプション引数](../structured/st_function.md#default-parameter)を使いたい場合、
既定値しか使えません。
引数なしコンストラクターがない場合には `new()` も既定値扱いですが、
ある場合には `new()` を渡せなくなります。

<pre class="source" title="引数なしコンストラクターの有無で new() の意味が変わる例">
<code><span class="reserved">void</span> <span class="method">m</span>(
    <span class="type">NoCtor</span> n1 = <span class="reserved">new</span>(),
    <span class="type">NoCtor</span> n2 = <span class="reserved">default</span>,
    <span class="type">Ctor</span> c1 = <span class="error"><span class="reserved">new</span>()</span>, <span class="comment">// この行だけコンパイル エラー</span>
    <span class="type">Ctor</span> c2 = <span class="reserved">default</span>
    )
{ }

<span class="reserved">struct</span> <span class="type">NoCtor</span> { }
<span class="reserved">struct</span> <span class="type">Ctor</span> { <span class="reserved">public</span> <span class="type">Ctor</span>() { } }
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="field-initialize"></a>フィールド初期化子
C# 10.0 で構造体に引数なしコンストラクターが使えるようになったことに伴って、
フィールド初期化子も使えるようになりました。
以下のようなコードは C# 10.0 から書けるようになります。

<pre class="source" title="構造体のフィールド初期化子の例">
<code><span class="reserved">struct</span> <span class="type struct">FieldInitializer</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X</span> <span class="operator">=</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Y</span> <span class="operator">=</span> <span class="number">2</span>;

    <span class="reserved">public</span> <span class="type struct">FieldInitializer</span>() { }
}
</code></pre>

`new()` だけで、`X`、`Y` の値がそれぞれ1、2に初期化されます。

<pre class="source" title="引数なしコンストラクターでフィールド初期化子が呼ばれる例">
<code><span class="reserved">var</span> f = <span class="reserved">new</span> <span class="type">FieldInitializer</span>();
<span class="type">Console</span>.<span class="method">WriteLine</span>(f.X); <span class="comment">// 1</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(f.Y); <span class="comment">// 2
</code></pre>

(※ 初期案では、明示的なコンストラクター定義もなしでフィールド初期化子を書けるようにする予定でした。
この際、フィールド初期化子を書くとコンパイラーが引数なしコンストラクターを生成していました。
C# 10 リリース当初はその案に基づいた実装になっていましたが、
ちょっと問題があって撤回され、明示的にコンストラクターを書かなければならなくなりました。)

###<a id="sec-generated-title-10"></a> <a id="accessibility"></a>引数なしコンストラクターのアクセシビリティ
`new()` が `default` と同じ意味になるのか、
それとも引数なしコンストラクターの呼び出しになるのか紛らわしくなるので、
構造体の引数なしコンストラクターは public 以外を認めていません。

<pre class="source" title="private、internal な引数なしコンストラクターはエラーになる">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">private</span> <span class="error"><span class="type">A</span></span>() =&gt; X = 0; <span class="comment">// エラー</span>
}

<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">internal</span> <span class="error"><span class="type">B</span></span>() =&gt; X = 0; <span class="comment">// エラー</span>
}
</code></pre>

##<a id="sec-generated-title-11"></a> <a id="definite-assignment"></a>確実な初期化
※ C# 10 までの仕様になります。

`new T()` や`default(T)`で作る「既定値」とは違って、
引数付きのコンストラクターを使う場合は、コンストラクター内で全てのメンバーをきっちり自分の手で初期化する必要がありました。

例えば、以下のコードは、コンストラクター内で `_z` の初期化を忘れているのでコンパイル エラーになっていました。

<pre class="source" title="_z の初期化忘れ">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">int</span> _x;
    <span class="reserved">int</span> _y;
    <span class="reserved">int</span> _z;

    <span class="reserved">public</span> Sample(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        _x = x;
        _y = y;
        <span class="comment">// C# 10 以前はコンパイル エラー</span>
    }
}
</code></pre>

(クラスの場合はこういう制限はなく、明示的に初期化しなかったフィールドは既定値(0)で初期化されます。)

また、全てのフィールドを初期化するまで、プロパティやメソッドなどの関数メンバーを呼べないという制約もありました。

<pre class="source" title="">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">int</span> _x;
    <span class="reserved">int</span> _y;

    <span class="reserved">public</span> Sample(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        M(); <span class="comment">// エラー: _x, _y の初期化より前に呼んじゃダメ。</span>
        _x = x;
        _y = y;
        M(); <span class="comment">// この順ならOK。</span>
    }

    <span class="reserved">void</span> M() { }
}
</code></pre>

(同じくクラスの場合は制限はなし。既定値(0)が使われるだけ。)

###<a id="sec-generated-title-12"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>
<h5 class="version version11">Ver. 11.0</h5>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
(クラスと構造体の差が1つなくなりました。)

前節のコードとほぼ同じですが、 C# 11 にすれば以下のようなコードがコンパイルできるようになります。

<pre class="source" title="構造体のフィールドが自動的に 0 初期化されるように">
<code><span class="reserved">struct</span> <span class="type struct">Sample</span>
{
    <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">int</span> <span class="field">_y</span>;
    <span class="reserved">int</span> <span class="field">_z</span>;

    <span class="reserved">public</span> <span class="type struct">Sample</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>)
    {
        <span class="method">M</span>(); <span class="comment">// C# 11 では初期化よりも先に読んでも平気。_x, _y にもこの時点でいったん 0 が入ってる。</span>

        <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;
        <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
        <span class="comment">// C# 11 では _z に 0 が自動で入る。</span>
    }

    <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="field">_x</span>}<span class="string">, </span>{<span class="field">_y</span>}<span class="string">, </span>{<span class="field">_z</span>}<span class="string">&quot;</span>);
}
</code></pre>


###<a id="sec-generated-title-13"></a> <a id="auto-property"></a>自動プロパティの扱い変更
<h5 class="version version6">Ver. 6</h5>

前節の「確実な初期化」と絡んで、C# 5.0までのC#では、自動プロパティの初期化が非常に面倒でした。

C# 5.0 以前の場合、以下のコードはコンパイル エラーを起こします。

<pre class="source" title="C# 5.0まではエラーになるコード">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }

    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        <span class="comment">// C# 5.0 まではエラーになる</span>
        X = x;
        Y = y;
    }
}
</code></pre>

エラーを起こす原因は、以下の組み合わせのせいです。

- 自動プロパティを定義すると、コンパイラーが対応するフィールド(バック フィールド)を作る
- 構造体の制約のせいで、バック フィールドが初期化されるまで、プロパティの読み書きできない
- でも、自動プロパティの場合、プロパティを介さずにバック フィールドを初期化する方法がない

このせいで、構造体と自動プロパティは相性が悪く、以下のように、自動プロパティを使わない書き方に書き換える必要がありました。

<pre class="source" title="C# 5.0までで正しくコンパイルできるようにするには">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _x;
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span> { <span class="reserved">return</span> _x; } }

    <span class="reserved">private</span> <span class="reserved">int</span> _y;
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span> { <span class="reserved">return</span> _y; } }

    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        _x = x;
        _y = y;
    }
}
</code></pre>

これに対して、C# 6では、最初のコードがコンパイルできるようになっています。
C#の仕様書に以下の1文が追加されたことによります。

- 自動プロパティを型の中から使う場合、そのバック フィールドに対する読み書きに置き換える

この仕様が追加されたことで、先ほどのコードはバック フィールドの初期化と見なされ、構造体の制約に引っかからなくなりました。

ちなみに、C# 6の場合は get のみの自動プロパティ(get-only auto-property)という構文が追加されて、先ほどのコードはさらに、以下のように書けるようになりました。

<pre class="source" title="C# 6のget-only自動プロパティ">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        X = x;
        Y = y;
    }
}
</code></pre>

##<a id="sec-generated-title-14"></a> <a id="memberwise"></a>メンバー毎コピー、メンバー毎比較
構造体の変数への代入は、全メンバーのコピーになります。
また、構造体には自動的に`Equals`メソッドが実装されて、メンバー毎の比較(全メンバー一致の場合に一致)になります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Point</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
        <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }

        <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) { X = x; Y = y; }
        <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; <span class="string">$"(</span>{X}<span class="string">, </span>{Y}<span class="string">)"</span>;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
        <span class="reserved">var</span> y = x;

        <span class="type">Console</span>.WriteLine(y); <span class="comment">// x のメンバー毎コピー = (1, 2)</span>

        <span class="comment">// メンバー毎比較(全メンバー一致なら一致)</span>
        <span class="type">Console</span>.WriteLine(x.Equals(<span class="reserved">new</span> <span class="type">Point</span>(1, 2))); <span class="comment">// true</span>
        <span class="type">Console</span>.WriteLine(x.Equals(<span class="reserved">new</span> <span class="type">Point</span>(1, 3))); <span class="comment">// false</span>
    }
}
</code></pre>

##<a id="sec-generated-title-15"></a> <a id="struct-modifier"></a>構造体に対する特別な修飾子
ここでは紹介だけになりますが、構造体にだけ付けることができる特別な修飾子があります。

- [readonly](readonlyness.md#readonly-struct)
- [ref](refstruct.md)

詳細についてはそれぞれリンク先を参照してください。

ちなみに、現状では、`ref` には語順に制約があって、
`struct`もしくは`partial`の直前に来る必要があります(緩和も検討されています)。
要するに、`readonly ref struct`はOKですが、`ref readonly struct`はエラーになります。

いくつか実例を挙げます。

<pre class="source" title="ref の語順の例">
<code><span class="comment">// OK</span>
<span class="reserved">readonly</span> <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">Ok1</span> { }
<span class="reserved">readonly</span> <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">Ok2</span> { }
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">Ok3</span> { }

<span class="comment">// コンパイル エラー</span>
<span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved"><span class="error">struct</span></span> <span class="type">Ng1</span> { }
<span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved"><span class="error">public</span></span> <span class="reserved">struct</span> <span class="type">Ng2</span> { }
<span class="reserved">readonly</span> <span class="reserved">public</span> <span class="error">partial</span> <span class="reserved"><span class="error">ref</span></span> <span class="reserved">struct</span> <span class="type">Ng3</span> { }
<span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved"><span class="error">partial</span></span> <span class="reserved">struct</span> <span class="type">Ng4</span> { }
<span class="reserved">public</span> <span class="reserved">ref</span> <span class="error">partial</span> <span class="reserved"><span class="error">readonly</span></span> <span class="reserved">struct</span> <span class="type">Ng5</span> { }
</code></pre>

おそらく、以下のような型の入れ子とメソッド定義の区別を楽にするための制限(あくまでコンパイラー都合)と思われます。

<pre class="source" title="ref の語順に制限がある理由">
<code><span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="comment">// 以下のエラー行、エラー内容は「readonly の後ろには型名が必要」になる</span>
    <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved"><span class="error">struct</span></span> <span class="type">InnerStruct</span> { }
    <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Method(<span class="reserved">in</span> <span class="reserved">int</span> x) =&gt; <span class="reserved">ref</span> x;
}
</code></pre>
