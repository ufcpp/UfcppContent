---
title: "タプル"
source_url: "https://ufcpp.net/study/csharp/datatype/tuples/"
content_type: "Article"
published_at: "2016-08-20T00:00:00"
updated_at: "2016-10-25T00:00:00"
tags: []
umbraco_id: 1941
parent_id: 1940
sort_order: 0
aliases:
  - "/csharp/data/tuples"
  - "/csharp/data/tuples/"
  - "/csharp/datatype/tuples/"
  - "/study/csharp/data/tuples"
  - "/study/csharp/data/tuples/"
---

# タプル

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<h5 class="version version7">Ver. 7</h5>

「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
型には常によい名前が付くわけではなく、名無しにしておきたいことがあります。
そういう場合に使うもののうちの1つがC# 7で導入されたタプルです。

タプルの最大の用途は[多値戻り値](../structured/st_anonymoustype.md#multiple-returns)です。
関数の戻り値は引数と対になるものなので、タプルの書き心地は引数に近くなるように設計されています。

#### <a id="sec-generated-title-2"></a>ポイント

- `(int x, int y)`というような、引数みたいな書き方で「名前のない型」を作れます
- この書き方をタプルと呼びます

## <a id="sec-generated-title-3"></a> <a id="tuple"></a>タプル

C# 7で導入された<strong id="key-tuple" class="keyword">タプル</strong>(tuple)は、
`(int x, int y)`というような、引数みたいな書き方で「名前のない型」を作る機能です。

※ タプルの利用には、`ValueTuple`構造体という型が必要になります。
この型が標準ライブラリに取り込まれるのは .NET Framework 4.7、.NET Standard 1.7を予定しています。
それ以前のバージョンでタプルを使いたい場合には、以下のパッケージを参照する必要があります。

- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)


### <a id="sec-generated-title-4"></a> <a id="name"></a>タプルという名前

最初に例を挙げた`(int x, int y)`という書き方は、2つの`int`の値`x`と`y`を並べたものなわけですが、こういう「データを複数並べたもの」を意味する単語がタプルです。

英語では倍数を「double, triple, quadruple, ...」などという単語で表しますが、これを一般化して n-tuple (nは0以上の任意の整数)と書くことがあり、これがタプルの語源です。
n倍、n重、n連結というような意味しかなく、まさに「名前のない複合型」にピッタリの単語です。

### <a id="sec-generated-title-5"></a> <a id="denotation"></a>型の明示

`(int x, int y)`みたいな書き方で、1つの型を表します。
タプルの型の書き方はメソッドの仮引数リスト(引数を受け取る側の書き方)に似ていて、`()`の中に「型名 メンバー名」を `,` 区切りで並べます。

これは、型を書ける場所であれば概ねどこにでもこの「型」を書けます。
まず、以下のように、フィールドや戻り値などの型にできます。

<pre class="source" title="フィールドや戻り値の型にタプルを使う">
<code><span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">private</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) value;
    <span class="reserved">public</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) GetValue() =&gt; value;
}
</code></pre>

以下のように、ローカル変数の型としても明示できます。

<pre class="source" title="明示的にローカル変数の型をタプル型にする">
<code><reserved></span><span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>();
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) t = s.GetValue();
</code></pre>

もちろん、`var`を使った型推論も効きます。

![varで型推論](../../../../assets/media/1091/tuplelocalinference.png)

また、ジェネリックな型の型引数にも使えます。

<pre class="source" title="型引数にタプルを使う">
<code><span class="reserved">var</span> dic = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;(<span class="reserved">string</span> s, <span class="reserved">string</span> t), (<span class="reserved">int</span> x, <span class="reserved">int</span> y)&gt;
{
    { (<span class="string">"a"</span>, <span class="string">"b"</span>), (1, 2) },
    { (<span class="string">"x"</span>, <span class="string">"y"</span>), (4, 8) },
};

<span class="type">Console</span>.WriteLine(dic[(<span class="string">"a"</span>, <span class="string">"b"</span>)]); <span class="comment">// (1, 2)</span>
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="denotation-disallowed"></a>制限事項

ただ、いくつか、通常の型であれば書ける場所で、タプルのこの記法を使えないところがあります。
以下の3つです。

- `new`演算子
- `is`演算子 ([C# 8.0 以降は使えるように](patterns.md#positional))
- `using`ディレクティブ ([C# 12 以降は使えるように](../structured/sp_namespace.md#using-any-type))

例えば以下のコードはコンパイル エラーを起こします。

<pre class="source" title="タプル型を掛けない場所">
<code><span class="comment">// using でエイリアスを付けることはできない(C# 11 以前)</span>
<span class="reserved">using</span> T = (<span class="reserved">int</span> x, <span class="reserved">int</span> y);

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// var t = new T(1, 2); みたいなのと同じノリでは書けない</span>
        <span class="reserved">var</span> t1 = <span class="reserved">new</span> <span class="error">(<span class="reserved">int</span> x, <span class="reserved">int</span> y)</span>(1, 2);
        <span class="reserved">var</span> t2 = <span class="reserved">new</span> <span class="error">(<span class="reserved">int</span> x, <span class="reserved">int</span> y)</span> { x = 1, y = 2 };
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">object</span> obj)
    {
        <span class="comment">// (C# 7.3 までは) is 演算子には使えない</span>
        <span class="reserved">if</span>(obj <span class="reserved">is</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y))
        {
        }
    }
}
</code></pre>

ただし、以下のように、配列やnull許容型を作る場合には`new`を使えます。

<pre class="source" title="">
<code><span class="reserved">var</span> a = <span class="reserved">new</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y)[10]; <span class="comment">// OK</span>
<span class="reserved">var</span> n = <span class="reserved">new</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y)?();  <span class="comment">// OK</span>
</code></pre>

`new (int x, int y)`という書き方は、将来的な言語拡張の予定と被る(被ってしまったら将来の拡張ができない)ため禁止しているようです。
`is`演算子は、C# 8.0で入った[位置パターン](patterns.md#positional)との競合を懸念して、C# 8.0までは認めていませんでした。

<pre class="source" title="将来的な拡張予定">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> ticks = 100000;
        <span class="comment">// (予定。C#7 ではできない) C# 8?</span>
        <span class="type">DateTime</span> d = <span class="reserved">new</span>(ticks); <span class="comment">// 左辺から型推論して、new DateTime(ticks) が呼ばれる</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">object</span> obj)
    {
        <span class="comment">// C# 8.0 で入った構文</span>
        <span class="comment">// is T 扱いじゃなくて、位置パターンで obj を x, y に分解</span>
        <span class="reserved">if</span> (obj <span class="reserved">is</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y))
        {
            <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);
        }
    }
}
</code></pre>

また、タプルのメンバーは2つ以上である必要があります。`()`や`(int x)`というようなタプルは現在の仕様では作れません。

<pre class="source" title="0-tuple, 1-tuple は書けない">
<code>() noneple;     <span class="comment">// ダメ</span>
(<span class="reserved">int</span> x) oneple; <span class="comment">// ダメ</span>

<span class="comment">// タプル構文で書けるのは2つ以上だけ</span>
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) twople; <span class="comment">// OK</span>

<span class="comment">// タプル構文でなければ、0-tuple, 1-tuple も作れる</span>
<span class="type">ValueTuple</span> none;     <span class="comment">// OK</span>
<span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>&gt; one; <span class="comment">// OK</span>
</code></pre>

### <a id="sec-generated-title-7"></a> <a id="literal"></a>タプル リテラル

タプルは`(1, 2)`というような書き方で[リテラル](../start/st_variable.md#literal)を書くことができます。
タプル リテラルは実引数リスト(引数を渡す側の書き方)に似ています。

<pre class="source" title="タプル リテラル">
<code><span class="comment">// メソッド呼び出し時の F(1, 2); みたいなノリ</span>
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) t1 = (1, 2);

<span class="comment">// メソッド呼び出し時の F(x: 1, y: 2); みたいなノリ</span>
<span class="reserved">var</span> t2 = (x: 1, y: 2);
</code></pre>

`null`のように単体では型が決まらないものも、左辺に型があれば推論が効きます。
一方で、左辺も`var`等になっていて型が決まらない場合、コンパイル エラーになります。

<pre class="source" title="">
<code><span class="comment">// これは左辺から型推論が聞くので、null も書ける</span>
(<span class="reserved">string</span> s, <span class="reserved">int</span> i) t1 = (<span class="reserved">null</span>, 1);

<span class="comment">// これはダメ。null の型が決まらない。</span>
<span class="reserved">var</span> t2 = (<span class="reserved">null</span>, 1); <span class="comment">// コンパイル エラー</span>
</code></pre>

### <a id="sec-generated-title-8"></a> <a id="member-access"></a>メンバー参照

メンバーの参照の仕方は普通の型と変わりません。`(int x, int y)`であれば、`x`、`y`という名前でアクセスできます。
ちなみに、タプルのメンバーは書き換え可能です。

<pre class="source" title="タプルのメンバー参照">
<code><span class="reserved">var</span> t = (x: 1, y: 2);
<span class="type">Console</span>.WriteLine(t.x); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(t.y); <span class="comment">// 2</span>

<span class="comment">// メンバーごとに書き換え可能</span>
t.x = 10;
t.y = 20;
<span class="type">Console</span>.WriteLine(t.x); <span class="comment">// 10</span>
<span class="type">Console</span>.WriteLine(t.y); <span class="comment">// 20</span>

<span class="comment">// タプル自身も書き換え可能</span>
t = (100, 200);
<span class="type">Console</span>.WriteLine(t.x); <span class="comment">// 100</span>
<span class="type">Console</span>.WriteLine(t.y); <span class="comment">// 200</span>
</code></pre>

ちなみに、タプルのメンバーはフィールドになっています
(プロパティではない)。
フィールドになっているということは、例えば、[参照引数(`ref`)](../resource/sp_ref.md#sec-byref)に直接渡せます
(これが、プロパティだと無理)。

例えば以下のようなメソッドがあったとします。

<pre class="source" title="Swapメソッド">
<code><span class="reserved">static</span> <span class="reserved">void</span> Swap&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> x, <span class="reserved">ref</span> <span class="type">T</span> y)
{
    <span class="reserved">var</span> t = x;
    x = y;
    y = t;
}
</code></pre>

このとき、以下のようにタプルのメンバーを渡せます。

<pre class="source" title="タプルのメンバーを参照引数に渡す">
<code><span class="reserved">var</span> t = (x: 1, y: 2);
Swap(<span class="reserved">ref</span> t.x, <span class="reserved">ref</span> t.y);
<span class="type">Console</span>.WriteLine(t.x); <span class="comment">// 2</span>
<span class="type">Console</span>.WriteLine(t.y); <span class="comment">// 1</span>
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="deconstruction"></a>タプルの分解

タプルは、各メンバーを分解して、それぞれ別の変数に受けて使うことができます。

<pre class="source" title="タプルの分解">
<code><span class="reserved">var</span> t = (x: 1, y: 2);

<span class="comment">// 分解宣言1</span>
(<span class="reserved">int</span> x1, <span class="reserved">int</span> y1) = t; <span class="comment">// x1, y1 を宣言しつつ、ｔ を分解</span>
<span class="comment">// 分解宣言2</span>
<span class="reserved">var</span> (x2, y2) = t; <span class="comment">// 分解宣言の簡易記法</span>

<span class="comment">// 分解代入</span>
<span class="reserved">int</span> x, y;
(x, y) = t; <span class="comment">// 分解結果を既存の変数に代入</span>
</code></pre>

この分解は、タプル以外の型に対しても使えるものです。
詳しくは「[複合型の分解](deconstruction.md)」で説明します。

### <a id="sec-generated-title-10"></a> <a id="conversion"></a>タプル間の変換

タプル間の代入は、一定の条件下では暗黙的変換が掛かります。

#### <a id="sec-generated-title-11"></a> <a id="different-names"></a>名前違いのタプル

タプル間の代入は、メンバーの宣言位置に基づいて行われます。
逆に言うと、名前は無関係で、メンバーの型の並びだけ一致していれば代入できます。

例えば以下のように書くと、1番目同士(`x` → `s`)、2番目同士(`y` → `t`)で値が代入されます。

<pre class="source" title="">
<code>(<span class="reserved">int</span> s, <span class="reserved">int</span> t) t1 = (x: 1, y: 2);
<span class="type">Console</span>.WriteLine(t1.s); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(t1.t); <span class="comment">// 2</span>
</code></pre>

同名であっても、位置が優先です。以下のような書き方をすると、`x`、`y`が入れ替わります。

<pre class="source" title="">
<code>(<span class="reserved">int</span> y, <span class="reserved">int</span> x) t2 = (x: 1, y: 2);
<span class="type">Console</span>.WriteLine(t2.x); <span class="comment">// 2</span>
<span class="type">Console</span>.WriteLine(t2.y); <span class="comment">// 1</span>
</code></pre>

#### <a id="sec-generated-title-12"></a> <a id="different-types"></a>型違いのタプル

タプルのメンバーの型が違う場合、メンバーごとに調べて、すべてのメンバーで暗黙的な変換がかかる場合に限り、
タプル間の暗黙的変換ができます。

例えば以下の場合、`x`も`y`も`z`も、それぞれが型変換できるので、タプルの暗黙的型変換が掛かります。

<pre class="source" title="タプル間の暗黙の型変換">
<code><span class="reserved">object</span> x = <span class="string">"abc"</span>; <span class="comment">// string → object は OK</span>
<span class="reserved">long</span> y = 1; <span class="comment">// int → long は OK</span>
<span class="reserved">int</span>? z = 2; <span class="comment">// int → int? は OK</span>
<span class="comment">// ↓</span>
(<span class="reserved">object</span> x, <span class="reserved">long</span> y, <span class="reserved">int</span>? z) t = (<span class="string">"abc"</span>, 1, 2); <span class="comment">// OK</span>
</code></pre>

逆に、以下の場合はコンパイル エラーになります。この例では全部のメンバーが変換不能ですが、全部でなくても、どれか1つでも変換できないと、タプル自体の変換もエラーになります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">string</span> x = 1; <span class="comment">// int → string は NG</span>
<span class="reserved">int</span> y = 1L; <span class="comment">// long → int は NG</span>
<span class="reserved">int</span> z = <span class="reserved">default</span>(<span class="reserved">int</span>?); <span class="comment">// int? → int は NG</span>
<span class="comment">// ↓</span>
(<span class="reserved">string</span> x, <span class="reserved">int</span> y, <span class="reserved">int</span> z) t = (1, 1L, <span class="reserved">default</span>(<span class="reserved">int</span>?)); <span class="comment">// NG</span>
</code></pre>

#### <a id="sec-generated-title-13"></a> <a id="extensions"></a>拡張メソッドの解決

前節のような型違いのタプル間の変換は、拡張メソッドのオーバーロード解決の際にも働きます。

例えば以下のように、配列×2のタプルに対して、`IEnumerable`×2のタプルの拡張メソッドを呼べます。
(配列から`IEnumerable`への変換は暗黙的に行えるので、このタプル間の変換も暗黙的に行えます。)

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">int</span>[] a1 = <span class="reserved">new</span>[] { 1, 2, 3 };
        <span class="reserved">string</span>[] a2 = <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"b"</span>, <span class="string">"c"</span> };

        <span class="comment">// 配列 ×2のタプルに対して、IEnumerable ×2のタプルの拡張メソッドを呼べる</span>
        <span class="reserved">foreach</span> (<span class="reserved">var</span> (i, s) <span class="reserved">in</span> (a1, a2).Zip())
        {
            <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{i}<span class="string">: </span>{s}<span class="string">"</span>);
        }
    }
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">TupelExtensions</span>
{
    <span class="comment">// IEnumerable ×2 に対する拡張メソッド</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;(<span class="type">T1</span> x1, <span class="type">T2</span> x2)&gt; Zip&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt;(<span class="reserved">this</span> (<span class="type">IEnumerable</span>&lt;<span class="type">T1</span>&gt; items1, <span class="type">IEnumerable</span>&lt;<span class="type">T2</span>&gt; items2) t)
        =&gt; t.items1.Zip(t.items2, (x1, x2) =&gt; (x1, x2));
}
</code></pre>


### <a id="sec-generated-title-14"></a> <a id="nest"></a>タプルの入れ子

タプルは入れ子にできます。

<pre class="source" title="タプルの入れ子">
<code><comment></span><span class="comment">// タプルの入れ子</span>
(<span class="reserved">string</span> a, (<span class="reserved">int</span> x, <span class="reserved">int</span> y) b) t1 = (<span class="string">"abc"</span>, (1, 2));
<span class="type">Console</span>.WriteLine(t1.a);   <span class="comment">// abc</span>
<span class="type">Console</span>.WriteLine(t1.b.x); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(t1.b.y); <span class="comment">// 2</span>

<span class="comment">// 型推論も可能</span>
<span class="reserved">var</span> t2 = (a: <span class="string">"abc"</span>, b: (x: 1, y: 2));
</code></pre>


### <a id="sec-generated-title-15"></a> <a id="anonymous-member"></a>メンバー名も匿名

タプルは、メンバー名もなくして、完全に匿名(名無し)にすることもできます。
この場合、メンバーを使う際には`Item1`、`Item2`、…というような名前で参照します。

<pre class="source" title="メンバー名も匿名なタプル">
<code><reserved></span><span class="reserved">var</span> t1 = (1, 2);
<span class="type">Console</span>.WriteLine(t1.Item1); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(t1.Item2); <span class="comment">// 2</span>
</code></pre>

`Item1`、`Item2`、… という名前は、後述する`ValueTuple`構造体のメンバー名です。

冒頭や「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
「メンバー名だけ見れば十分」だから型名を省略するのであって、
メンバー名まで省略するのとさすがにプログラムが読みづらくなります。
メンバー名も持っていない完全な匿名タプルは、おそらくかなり短い寿命でしか使わないでしょう。
例えば、すぐに別の(メンバー名のある)タプル型に代入したり、分解して変数に受けて使うことになります。

### <a id="sec-generated-title-16"></a> <a id="overload"></a>オーバーロード

型違いのタプルを使うのであれば、オーバーロードに使えます。
例えば、以下のメソッド`F`は、`y`の型が違うのでオーバーロード可能です。

<pre class="source" title="型違いのタプルでのオーバーロードは可能">
<code><span class="comment">// 型違いのタプルでのオーバーロードは可能</span>
<span class="reserved">void</span> F((<span class="reserved">int</span> x, <span class="reserved">int</span> y) t) { }
<span class="reserved">void</span> F((<span class="reserved">int</span> x, <span class="reserved">string</span> y) t) { }
</code></pre>

一方、型が一緒で名前だけが違うタプルではオーバーロードできません。
以下のメソッド`G`は、同じものが2つあるのでコンパイル エラーを起こします。

<pre class="source" title="">
<code><span class="comment">// 型が一緒で名前だけ違うタプルでのオーバーロードはダメ。コンパイル エラー</span>
<span class="reserved">void</span> G((<span class="reserved">int</span> x, <span class="reserved">int</span> y) t) { }
<span class="reserved">void</span> G((<span class="reserved">int</span> a, <span class="reserved">int</span> b) t) { }
</code></pre>

こういう仕様になっている理由は2つあります。
1つは、次節で説明するように、内部実装的に名前だけ違うタプルを区別できないという、技術的な理由。
もう1つは、[引数でのオーバーロード](../structured/st_function.md#overload)が名前を見ていない(引数の型だけがシグネチャに含まれる)のだから、引数に倣って設計されているタプルでも、メンバー名は区別しないのが自然という理由です。

### <a id="sec-generated-title-17"></a> <a id="infer-tuple-name"></a>タプル要素名の推論

<h5 class="version version7_1">Ver. 7.1</h5>

C# 7.1から、タプル構築時に渡した変数からタプルの要素名を推論できるようになりました。
例えば以下のように、`(x, y)` と書くだけで、1要素目に`x`、2要素目に `y` という名前が付きます。
(これまでだと、`(x: x, y: y)` と書く必要がありました。)

<pre class="source" title="タプル要素名の推論の例">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
<span class="reserved">var</span> t = (x, y);

<span class="comment">// C# 7.0。t の要素には名前が付かない</span>
<span class="type">Console</span>.WriteLine(t.Item1);
<span class="type">Console</span>.WriteLine(t.Item2);

<span class="comment">// C# 7.1。(x, y) で (x: x, y: y) 扱い</span>
<span class="comment">// t の要素に x, y という名前が付く</span>
<span class="type">Console</span>.WriteLine(t.x);
<span class="type">Console</span>.WriteLine(t.y);
</code></pre>

以下のように、部分的な適用もされます。

<pre class="source" title="タプル要素名の部分的な推論">
<code><span class="reserved">var</span> y = 2;
<span class="reserved">var</span> t = (1, y);
<span class="type">Console</span>.WriteLine(t.Item1); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(t.y);     <span class="comment">// 2</span>
</code></pre>

ただし、名前に被りがあるときには推論が働きません。

<pre class="source" title="名前被りでタプル要素名の推論ができない例">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> t = (x, x);
<span class="type">Console</span>.WriteLine(t.Item1); <span class="comment">// t.x とは書けない</span>
<span class="type">Console</span>.WriteLine(t.Item2); <span class="comment">// こっちも t.x とは書けない</span>

<span class="reserved">var</span> u = (x: 0, x);
<span class="type">Console</span>.WriteLine(u.x); <span class="comment">// u.x というと Item1 の方</span>
<span class="type">Console</span>.WriteLine(u.Item2); <span class="comment">// Item2 の方は x とは書けない</span>
</code></pre>

名前がないので当然ですが、リテラルからは要素名の推論はできません

<pre class="source" title="リテラルからは推論不可">
<code><span class="reserved">var</span> t = (1, 2);
<span class="type">Console</span>.WriteLine(t.Item1); <span class="comment">// さすがに t.1 とかは書けない</span>
</code></pre>

また、メソッド名からは推論されません。
一方で、プロパティ名からは推論されます。
プロパティやフィールドの場合、インスタンス メンバーへのアクセスでも推論されます
(`t.x`とかなら、タプル要素名は`x`になります。`t?.x`でも可)。

<pre class="source" title="メソッド不可、プロパティ可。インスタンス メンバー アクセス可。null 条件演算子可">
<code><span class="reserved">int</span> F() =&gt; 1;
<span class="reserved">var</span> s = <span class="string">"abc"</span>;

<span class="reserved">var</span> t = (F(), s?.Length);
<span class="type">Console</span>.WriteLine(t.Item1); <span class="comment">// メソッド名からは推論されない(t.F はダメ)</span>
<span class="type">Console</span>.WriteLine(t.Length); <span class="comment">// プロパティ名からは推論される( . でも ?. でも OK)</span>
</code></pre>

### <a id="sec-generated-title-18"></a> <a id="equality"></a>==、!= での比較

<h5 class="version version7">Ver. 7.3</h5>

C# 7.3で、タプル同士を `==`、`!=` 演算子で比較できるようになりました。

これは、後述する[`ValueTuple`](#tuple-ValueTuple)の演算子が呼ばれるわけではなく、
コンパイラーによる特別な処理が入ります。

タプルに対する`==`比較は、以下のように、メンバーごとの`==`を[`&&`](../start/st_operator.md#short-circuit)で繋いだものに展開されます。

<pre class="source" title="タプル ==">
<code><span class="reserved">void</span> M((<span class="reserved">int</span> a, (<span class="reserved">int</span> x, <span class="reserved">int</span> y) b) t)
{
    <span class="comment">// このタプル == 比較は、</span>
    <span class="type">Console</span>.WriteLine(t == (1, (2, 3)));
    <span class="comment">// こんな感じで、メンバーごとの == を &amp;&amp; で繋いだものに展開される。</span>
    <span class="type">Console</span>.WriteLine(t.a == 1 &amp;&amp; t.b.x == 2 &amp;&amp; t.b.y == 3);
}
</code></pre>

同様に、`!=`は以下のように、メンバーごとの`!=`を[`||`](../start/st_operator.md#short-circuit)で繋いだものになります。

<pre class="source" title="タプル !=">
<code><span class="reserved">void</span> N((<span class="reserved">int</span> a, (<span class="reserved">int</span> x, <span class="reserved">int</span> y) b) t)
{
    <span class="comment">// 同じく != 比較は、</span>
    <span class="type">Console</span>.WriteLine(t != (1, (2, 3)));
    <span class="comment">// こんな感じで、メンバーごとの != を || で繋いだものに展開される。</span>
    <span class="type">Console</span>.WriteLine(t.a != 1 || t.b.x != 2 || t.b.y != 3);
}
</code></pre>

`ValueTuple`の`==`演算子や`Equals`メソッドではなくこういうコンパイラーによる処理が入っているのは、
「[タプル間の変換](#conversion)」で説明したような、メンバーごとの型変換を考慮してのことです。
例えば、以下のように、暗黙的型変換ができるもの同士の比較ができます。

<pre class="source" title="">
<code>(<span class="reserved">long</span> a, (<span class="reserved">double</span> x, <span class="reserved">decimal</span> y) b) t = (1, (2, 3));

<span class="comment">// byte → long</span>
<span class="comment">// float → double</span>
<span class="comment">// short → decimal</span>
<span class="comment">// という、暗黙的型変換ができるもの同士の比較</span>
<span class="type">Console</span>.WriteLine(t == ((<span class="reserved">byte</span>)1, ((<span class="reserved">float</span>)2, (<span class="reserved">short</span>)3)));
</code></pre>

ちなみに、[ユーザー定義](../oop/oo_operator.md)の`==`、`!=`演算子を持っている場合、そのユーザー定義のものが呼ばれます。
また、ユーザー定義であれば`==`が`bool`以外の型を返すこともありますが、
その場合も、[`true`、`false`演算子](../oop/oo_operator.md#true-false)があれば比較できます。

<pre class="source" title="ユーザー定義の ==, !=, true, false が呼ばれる例">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">MyBool</span>
{
    <span class="reserved">public</span> <span class="reserved">bool</span> Value;
    <span class="reserved">public</span> <span class="type">MyBool</span>(<span class="reserved">bool</span> value) =&gt; Value = value;

    <span class="comment">// 何が呼ばれてるかがわかるように WriteLine を挟む</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">true</span>(<span class="type">MyBool</span> x) { <span class="type">Console</span>.WriteLine(<span class="string">"MyBool.true"</span>); <span class="reserved">return</span> x.Value; }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">false</span>(<span class="type">MyBool</span> x) { <span class="type">Console</span>.WriteLine(<span class="string">"MyBool.false"</span>); <span class="reserved">return</span> !x.Value; }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">MyBool</span>(<span class="reserved">bool</span> b) =&gt; <span class="reserved">new</span> <span class="type">MyBool</span>(b);
}

<span class="reserved">struct</span> <span class="type">MyInt</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value;
    <span class="reserved">public</span> MyInt(<span class="reserved">int</span> value) =&gt; Value = value;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">MyBool</span> <span class="reserved">operator</span> ==(<span class="type">MyInt</span> x, <span class="type">MyInt</span> y) =&gt; x.Value == y.Value;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">MyBool</span> <span class="reserved">operator</span> !=(<span class="type">MyInt</span> x, <span class="type">MyInt</span> y) =&gt; x.Value != y.Value;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">MyInt</span>(<span class="reserved">int</span> b) =&gt; <span class="reserved">new</span> <span class="type">MyInt</span>(b);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">bool</span> Equals(<span class="reserved">object</span> obj) =&gt; obj <span class="reserved">is</span> <span class="type">MyInt</span> x &amp;&amp; Value == x.Value;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> GetHashCode() =&gt; Value.GetHashCode();
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        (<span class="type">MyInt</span> a, (<span class="type">MyInt</span> x, <span class="type">MyInt</span> y) b) t = (1, (2, 3));

        <span class="comment">// MyInt の == に展開されるので、MyBool が得られる。</span>
        <span class="comment">// MyBool 同士の &amp;&amp; で、MyBool の false 演算子が呼ばれる。</span>
        <span class="comment">// (この例の場合、"MyBool.false" が3回表示される。)</span>
        <span class="comment">// (false の方が呼ばれるのは C# の &amp;&amp; の仕様。)</span>
        <span class="type">Console</span>.WriteLine(t == (1, (2, 3)));
    }
}
</code></pre>


<!-- original-page-break -->

## <a id="sec-generated-title-19"></a> <a id="internal"></a>タプルの内部実装

タプルがどういうコードに展開されるかについても話しておきましょう。

タプルを使ったコードを古いバージョンの.NET上で動かしたり、
タプルを使ったライブラリを古いバージョンのC#から参照したり、
別のプログラミング言語から参照したい場合もあります。
そのために、タプルは、`ValueTuple`という構造体に展開されます。

### <a id="sec-generated-title-20"></a> <a id="tuple-ValueTuple"></a>ValueTuple構造体への展開

タプルは、コンパイルの結果としては`ValueTuple`構造体(`System`名前空間)に展開されます。

例えば、以下のようなコードを考えます。

<pre class="source" title="ローカルでのタプル利用">
<code><span class="reserved">var</span> t = (x: 3, y: 5);
<span class="reserved">var</span> p = t.x * t.y;
<span class="reserved">var</span> (x, y) = t;
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string"> × </span>{y}<span class="string"> = </span>{p}<span class="string">"</span>);
</code></pre>

以下のようなコードに展開されます。

<pre class="source" title="ローカルでのタプルの展開結果">
<code><span class="reserved">var</span> t = <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(3, 5); <span class="comment">// (x: 3, y: 5)</span>
<span class="reserved">var</span> p = t.Item1 * t.Item2; <span class="comment">// t.x * t.y</span>
<span class="reserved">var</span> x = t.Item1;
<span class="reserved">var</span> y = t.Item2;
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string"> × </span>{y}<span class="string"> = </span>{p}<span class="string">"</span>);
</code></pre>

元々の`x`や`y`という名前は、内部的には残っていません。`ValueTuple`構造体のメンバーである`Item1`や`Item2`に展開されます。

特に、一度`object`や`dynamic`を経由すると、名前を完全に紛失します。
以下のコードでは、`x`や`y`が見つからず、実行時エラーを起こします。

<pre class="source" title="タプル型は名前を紛失する">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Dynamic()
{
    <span class="comment">// 匿名型は名前が残る</span>
    <span class="reserved">var</span> a = <span class="reserved">new</span> { x = 3, y = 5 };
    <span class="reserved">var</span> s1 = Sum(a); <span class="comment">// 大丈夫</span>
    <span class="type">Console</span>.WriteLine(s1);

    <span class="comment">// タプル型は名前を紛失する</span>
    <span class="reserved">var</span> t = (x: 3, y: 5);
    <span class="reserved">var</span> s2 = Sum(t); <span class="comment">// x, yという名前が実行時になくてエラーに</span>
    <span class="type">Console</span>.WriteLine(s2);
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">dynamic</span> Sum(<span class="reserved">dynamic</span> d) =&gt; d.x + d.y;
</code></pre>

### <a id="sec-generated-title-21"></a> <a id="TupleElementNames"></a>TupleElementNames属性

とはいえ、名前をどこにも残さないと、ライブラリをまたいだ時に`x`、`y`などの名前が使えなくて困ります。
そこで、クラスのメンバーにタプルを使う場合には、`TupleElementNames`属性(`System.Runtime.CompilerServices`名前空間)を付けて、
C#コンパイラーには名前がわかるようにしています。

例えば、以下のような引数も戻り値もタプルなメソッドを書いたとします。

<pre class="source" title="引数も戻り値もタプルなメソッド">
<code><span class="reserved">public</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) F((<span class="reserved">int</span> a, <span class="reserved">int</span> b) t) =&gt; (t.a + t.b, t.a - t.b);
</code></pre>

このメソッドは、以下のように展開されます。タプルが`ValueTuple`構造体に化けますが、`TupleElementNames`属性を付けて名前を残します。

<pre class="source" title="">
<code>[<span class="reserved">return</span>: <span class="type">TupleElementNames</span>(<span class="reserved">new</span>[] { <span class="string">"x"</span>, <span class="string">"y"</span> })]
<span class="reserved">public</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; F([<span class="type">TupleElementNames</span>(<span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"b"</span> })] <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; t)
    =&gt; <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(t.Item1 + t.Item2, t.Item1 - t.Item2);
</code></pre>

C#コンパイラーは、この情報を元に、タプルの名前を復元します。

### <a id="sec-generated-title-22"></a> <a id="ValueTuple-definition"></a>ValueTuple構造体の中身

タプルの展開結果にあたる`ValueTuple`は、型引数が0～8個の合計9個の構造体があります。
例えば、型引数2個のものは以下のような定義になっています。

<pre class="source" title="ValueTuple構造体">
<code>[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Auto)]
<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">ValueTuple</span>&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt;
    : <span class="type">IEquatable</span>&lt;<span class="type">ValueTuple</span>&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt;&gt;, <span class="type">IStructuralEquatable</span>, <span class="type">IStructuralComparable</span>, <span class="type">IComparable</span>, <span class="type">IComparable</span>&lt;<span class="type">ValueTuple</span>&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt;&gt;
{
    <span class="reserved">public</span> <span class="type">T1</span> Item1;
    <span class="reserved">public</span> <span class="type">T2</span> Item2;

    <span class="reserved">public</span> ValueTuple(<span class="type">T1</span> item1, <span class="type">T2</span> item2)
    {
        Item1 = item1;
        Item2 = item2;
    }

    <span class="comment">// 後略、インターフェイスのメンバー定義</span>
}
</code></pre>

基本的には、publicなフィールドだけを持つ構造体です。
それに、値の比較用の各種インターフェイスが実装されています。

#### <a id="sec-generated-title-23"></a> <a id="long-tuple"></a>メンバーが9個以上のタプル

最初に言った通り、`ValueTuple`構造体の型引数は、最大のものでも8個です。
では、メンバーが9個以上のタプルを作るとどうなるかというと、入れ子の`ValueTuple`構造体が作られます。

例えば、以下のようなコードを書いたとします。
メンバー名も匿名で作ったので `ItemN`(`N`は正の整数)といったような名前でメンバーを読み書きすることになります。
C#上は、8番目以降のメンバーに対しても、`Item8`、`Item9`というような名前で参照できます。

<pre class="source" title="メンバーが9個のタプル">
<code><span class="reserved">var</span> t = (1, 2, 3, 4, 5, 6, 7, 8, 9);
<span class="type">Console</span>.WriteLine(t.Item9);
</code></pre>

このコードは、以下のように展開されます。

<pre class="source" title="メンバーが9個のタプルの展開結果">
<code><span class="reserved">var</span> t = <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>, <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt;(
    1, 2, 3, 4, 5, 6, 7, <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(8, 9));
<span class="type">Console</span>.WriteLine(t.Rest.Item2);
</code></pre>

`ValueTuple`構造体には`Item8`、`Item9`という名前のメンバーはありません。
型引数の数が最大のもので8メンバーで、その8つ目のメンバーの名前は`Rest` (残り)です。
そして、以下のように、C#上`Item9`であれば展開結果的には`Rest`のさらに`Item2`というように、入れ子のメンバー参照に展開されます。

C# 上 | コンパイル結果
---- | ----
`Item8` | `Rest.Item1`
`Item9` | `Rest.Item2`
… | …
`Item15` | `Rest.Rest.Item1`
`Item16` | `Rest.Rest.Item2`
… | …

#### <a id="sec-generated-title-24"></a> <a id="nupkg"></a>ValueTuple構造体の定義場所

C# 7のリリースに合わせて、`ValueTuple`構造体は標準ライブラリに取り込まれる予定です。

一方で、古い.NET (.NET Framework 4.6.2以前、.NET Standard 1.6以前)上でタプルを使いたい場合、
以下のライブラリを参照します。この中に`ValueTuple`構造体や、`TupleElementNames`属性が定義されています。

- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)

### <a id="sec-generated-title-25"></a> <a id="0-tuple"></a>型引数0、1のValueTuple

前述の通り、タプルのメンバーは2つ以上な必要があって、`()`や`(int x)`というようなタプルは作れません。
一方で、`ValueTuple`構造体には、型引数0個と1個のものが存在します。

<pre class="source" title="型引数0個と1個のValueTuple">
<code><span class="comment">// メンバー0個、1個のものは、構造体はあるけど、タプル構文は使えない</span>
<span class="reserved">var</span> noneple = <span class="reserved">new</span> <span class="type">ValueTuple</span>();
<span class="reserved">var</span> oneple = <span class="reserved">new</span> <span class="type">ValueTuple</span>&lt;<span class="reserved">int</span>&gt;(1);

<span class="comment">// メンバー2個以上はタプル構文を使える</span>
<span class="reserved">var</span> twople = (1, 2); <span class="comment">// new ValueTuple&lt;int, int&gt;(1, 2);</span>
<span class="reserved">var</span> threeple = (1, 2, 3); <span class="comment">// new ValueTuple&lt;int, int, int&gt;(1, 2, 3);</span>
</code></pre>

型引数0個の`ValueTuple`(0-tuple)は、いわゆる[Unit型](../structured/st_function.md#unit)です。
`void`の代わりにこの型を使うことで、戻り値がある場合とない場合のコードを統一的に書けてうれしい場合があります。
一方、型引数1個のもの(1-tuple)も、用途としては0-tupleと同じです。
型引数2個以上のものと並べて、戻り値や引数の個数違いを統一的に書けます。

例えば、以下の2つのコードはどちらの方が統一性があっていいかという話になります。

<pre class="source" title="タプルでは0、1は書けない">
<code><span class="comment">// タプルでは0、1は書けない</span>
<span class="reserved">async</span> <span class="type">Task</span> F0() { }
<span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; F1() =&gt; 1;
<span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x1, <span class="reserved">int</span> x2)&gt; F2() =&gt; (1, 2);
<span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x1, <span class="reserved">int</span> x2, <span class="reserved">int</span> x3)&gt; F3() =&gt; (1, 2, 3);
</code></pre>

<pre class="source" title="こう書けると統一性があってきれい">
<code><span class="comment">// こう書けると統一性があってきれい(C# 7では書けない)</span>
<span class="reserved">async</span> <span class="type">Task</span>&lt;()&gt; F0() { }
<span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x1)&gt; F1() =&gt; (1);
<span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x1, <span class="reserved">int</span> x2)&gt; F2() =&gt; (1, 2);
<span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x1, <span class="reserved">int</span> x2, <span class="reserved">int</span> x3)&gt; F3() =&gt; (1, 2, 3);
</code></pre>

特に、ソースコード生成などでまとめて、個数違いのメソッドを生成したい場合などには、0-tupleや1-tupleがほしくなります。
0個と1個の時だけ特別扱いが必要になるかどうかという問題です。
0-tupleと1-tupleがあれば、特別扱いなしでソースコード生成ができて楽です。

ということで、0-tuple、1-tupleの需要はあるんですが、問題があって構文を提供できていません。
1-tupleになるであろう構文は`(1)`というような形になるはずですが、
これが、C#の既存の構文ですでに、単に`1`と同じ意味で解釈されるため、1-tupleを作れません。
0-tupleの方の`()`は、これまでは書けなかった書き方なので別にC# 7で追加できますが、
1-tupleだけ飛ばして「0か2以上のみ」とするのも変な話です。

<!-- original-page-break -->

## <a id="sec-generated-title-26"></a> <a id="related"></a>関連

タプルには、毛色の似た機能が2つあります。

- [匿名型](../start/sp3_inference.md#anonymous) … タプルと同様に、名前がない型
- [出力引数](../resource/sp_ref.md#out) … 複数の戻り値を返すのに使える

これらとの関連・使い分けについても話しておきましょう。

### <a id="sec-generated-title-27"></a> <a id="anonymous-type"></a>匿名型との比較

タプルは、名前がない型という観点で言うと、[匿名型](../start/sp3_inference.md#anonymous)と似ています。
しかし、「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
出自・用途の違いから、内部実装は結構異なります。

以下の表のようになります。

| | タプル | 匿名型 |
|---|---|---|
|主な用途|[多値戻り値](../structured/st_anonymoustype.md#multiple-returns)|[部分的なメンバー抜き出し](../structured/st_anonymoustype.md#projection)|
|展開結果| `ValueTuple`構造体＋属性 | クラスの生成 |
|型の種類|値型|参照型|
|見た目|引数の書き方に似ている|オブジェクト初期化子の書き方に似ている|

展開結果の差は用途の差から来ています。
タプルは戻り値として使います。publicなメンバーの型にも使うことになるので、ライブラリ間をまたげる必要があります。
`ValueTuple`構造体に展開することで、ライブラリをまたいでも同じ構造体を参照する状態になります。

一方、匿名型は、ライブラリごとにそれぞれクラスを生成します(「[匿名型](../start/sp3_inference.md#anonymous)」参照)。
同じ型に見えて、ライブラリをまたぐと別クラスになってしまいます。
このことから、匿名型は、メソッドの戻り値など、publicになりうる場所には書けません。
メソッド内のローカルな部分で完結して使う必要があります。

とはいえ、`ValueTuple`構造体に展開では、前節での説明の通り、実行時に名前を紛失します。
[`dynamic`](../dynamic/sp4_dynamic.md)や、[式木](../dynamic/sp3_expression.md)での利用にはタプルは向きません。この用途なら匿名型の方が向いています。

値型か参照型かも実装が異なりますが、これも、戻り値として使う、その後すぐに[分解](#deconstruction)して使うという想定だと、値型の方が実行性能的に有利だからです。
用途が変われば最適な実装は変わります。

### <a id="sec-generated-title-28"></a> <a id="out-params"></a>出力引数との比較

多値戻り値という用途だと、[出力引数](../resource/sp_ref.md#out)という手段もあります。
一般的に言うと、多値戻り値には今後タプルを使うのがおすすめです。
出力引数の方が煩雑な書き方になりがちだからです。

比較のために簡単な例を挙げてみましょう。まず、C# 6以前の出力引数を使ったものです。

<pre class="source" title="出力引数(C# 6)版">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="type">Point</span> p)
{
    <span class="comment">// 事前に変数を用意しないといけない/var 不可</span>
    <span class="reserved">int</span> x, y;
    <span class="comment">// 1個1個 out を付けないといけない</span>
    Deconstruct(p, <span class="reserved">out</span> x, <span class="reserved">out</span> y);
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);

    <span class="comment">//非同期メソッドには使えない</span>
}

<span class="comment">// 1個1個 out を付けないといけない</span>
<span class="reserved">static</span> <span class="reserved">void</span> Deconstruct(<span class="type">Point</span> p, <span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y)
{
    <span class="comment">// 1個1個代入</span>
    x = p.X;
    y = p.Y;
}
</code></pre>

1個1個`out`修飾子を付けて回るのは結構な煩雑さです。
呼び出す前に別途変数宣言が必要なのも面倒です。
これらは単に煩雑なだけなので我慢すれば何とかなりますが、
致命的なのは非同期メソッドで使えないことです。

ちなみに、煩雑さはC# 7で多少マシになりました。[出力変数宣言](../resource/sp_ref.md#out-var)という構文が追加されて、以下のように書けます。

<pre class="source" title="出力引数(C# 7)版">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="type">Point</span> p)
{
    <span class="comment">// 変数の事前準備は不要に</span>
    <span class="comment">// でも1個1個 out を付けないといけない</span>
    Deconstruct(p, <em><span class="reserved">out</span> <span class="reserved">var</span> x, <span class="reserved">out</span> <span class="reserved">var</span> y</em>);
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);

    <span class="comment">//非同期メソッドには相変わらず使えない</span>
}

<span class="comment">// 1個1個 out を付けないといけない</span>
<span class="reserved">static</span> <span class="reserved">void</span> Deconstruct(<span class="type">Point</span> p, <span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (p.X, p.Y);
</code></pre>

でも、相変わらず長くなりがちです。
また、非同期メソッドで使えない点は変わりません。

タプルを使えばこの問題は解決です。

<pre class="source" title="タプル版">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> F(<span class="type">Point</span> p)
{
    <span class="comment">// 1個の var で受け取れる</span>
    <span class="reserved">var</span> t1 = Deconstruct(p);
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{t1.x}<span class="string">, </span>{t1.y}<span class="string">"</span>);

    <span class="comment">// 何なら分解と併せればもっと書き心地よく書ける</span>
    <span class="reserved">var</span> (x, y) = Deconstruct(p);
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);

    <span class="comment">// 非同期メソッドで使えるのはタプルだけ</span>
    <span class="reserved">var</span> t2 = <span class="reserved">await</span> DeconstructAsync(p);
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{t2.x}<span class="string">, </span>{t2.y}<span class="string">"</span>);
}

<span class="reserved">static</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) Deconstruct(<span class="type">Point</span> p) =&gt; (p.X, p.Y); <span class="comment">// 1個の式で書けて楽</span>
<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y)&gt; DeconstructAsync(<span class="type">Point</span> p) =&gt; (p.X, p.Y);
</code></pre>

一方で、出力引数を使いたくなる場面も残っています。

- `TryParse`のように、`bool`値を返して`if`ステートメントなどの条件式内で使いたい場合
- オーバーロードを呼び分けたい場合

`if`内で使いたい場合は、例えば以下のようなコードになります。

<pre class="source" title="if 内で使うなら bool 1個の戻り値の方が使いやすい">
<code><span class="reserved">static</span> <span class="reserved">void</span> TryPattern()
{
    <span class="reserved">var</span> s = <span class="type">Console</span>.ReadLine();
    <span class="reserved">if</span> (<span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> <span class="reserved">var</span> x)) <span class="type">Console</span>.WriteLine(x);
}
</code></pre>

これはさすがにタプルを使う方が煩雑です。

<pre class="source" title="if 内で使うならタプルの方が煩雑">
<code><span class="reserved">static</span> <span class="reserved">void</span> TuplePattern()
{
    <span class="reserved">var</span> s = <span class="type">Console</span>.ReadLine();
    <span class="reserved">var</span> (success, x) = Parse(s);
    <span class="reserved">if</span> (success) <span class="type">Console</span>.WriteLine(x);
}

<span class="reserved">static</span> (<span class="reserved">bool</span> success, <span class="reserved">int</span> value) Parse(<span class="reserved">string</span> s) =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> <span class="reserved">var</span> x) ? (<span class="reserved">true</span>, x) : (<span class="reserved">false</span>, 0);
</code></pre>

もっとも、C# 7では、以下のような `is` 演算子を使った`null`チェックで同様のことをすると言う手もあります。
この書き方を型スイッチと呼びます(説明ページ準備中。でき次第リンク)。

<pre class="source" title="C# 7の is を使って、int? の null チェック">
<code><span class="reserved">static</span> <span class="reserved">void</span> NullCheckPattern()
{
    <span class="reserved">var</span> s = <span class="type">Console</span>.ReadLine();
    <span class="reserved">if</span> (ParseOrDefault(s) <em><span class="reserved">is</span> <span class="reserved">int</span> x</em>) <span class="type">Console</span>.WriteLine(x);
}

<span class="reserved">static</span> <span class="reserved">int</span>? ParseOrDefault(<span class="reserved">string</span> s) =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> <span class="reserved">var</span> x) ? x : <span class="reserved">default</span>(<span class="reserved">int</span>?);
</code></pre>

もう1つ、[オーバーロード](../structured/st_function.md#overload)ですが、C#では(というか.NETでは)、引数でのオーバーロードはできますが、戻り値でのオーバーロードはできません。
そこで、以下のように、オーバーロードに関しては出力引数の方が有利になります。

<pre class="source" title="オーバーロードの可否">
<code><span class="comment">// これはオーバーロード可能</span>
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (1, 2);
<span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">out</span> <span class="reserved">int</span> id, <span class="reserved">out</span> <span class="reserved">string</span> name) =&gt; (id, name) = (1, <span class="string">"abc"</span>);

<span class="comment">// 戻り値でのオーバーロードはできない</span>
<span class="comment">// コンパイル エラーに</span>
<span class="reserved">static</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) F() =&gt; (1, 2);
<span class="reserved">static</span> (<span class="reserved">int</span> id, <span class="reserved">string</span> name) F() =&gt; (1, <span class="string">"abc"</span>);
</code></pre>
