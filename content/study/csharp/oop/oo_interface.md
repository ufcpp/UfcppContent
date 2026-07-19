---
title: "インターフェース"
source_url: "https://ufcpp.net/study/csharp/oop/oo_interface/"
content_type: "Article"
published_at: "2002-10-05T00:00:00"
updated_at: "2019-05-05T00:00:00"
tags: []
umbraco_id: 1269
parent_id: 1248
sort_order: 16
aliases:
  - "/csharp/oo_interface"
  - "/csharp/oo_interface.html"
  - "/csharp/oop/oo_interface/"
  - "/study/csharp/oo_interface"
  - "/study/csharp/oo_interface.html"
---

# インターフェース

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

インターフェース(interface)という言葉の意味は直訳すると「境界面」になります。
すなわち、物と物との間の仲介をする部分のことです。

例えば、PC と周辺機器をつなぐ場合、
どのような物理媒体を用いて、どういう信号を送るかといった規約を定める必要があります。
このような約束事に基づいて作られたケーブルやコネクタのことをインターフェースと呼ぶわけです。

オブジェクト指向プログラミングの世界においては、
インターフェースとはクラスが実装すべき規約（どういうメソッドにどういう引数を渡すかなど）を定めるものです。
すなわち、クラス設計者とクラス利用者の間の仲介役を担うのがインターフェースです。


##### <a id="sec-generated-title-2"></a>ポイント

* インターフェース: クラス外部からみた規約だけを定めるもの。「クラスの内外の境界」という意味。

* public な抽象メソッドだけを持つクラスのようなもの。
    * C# 8.0 で緩和されて、「フィールドを持てない代わりに多重継承できる」くらいの差に縮まっています

* 抽象クラスと違って、複数のインターフェースを継承できる。

* class キーワードの代わりに interface キーワードを使う。

##### <a id="sec-generated-title-3"></a>サンプル

-[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Oop/InterfaceSample](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Oop/InterfaceSample)

## <a id="sec-generated-title-4"></a> <a id="contract"></a>メソッドの規約と実装

メソッドを設計する場合、規約の決定と実装という2つの段階を経ることになります。

<strong id="contract" class="keyword">規約</strong>あるいは<em>契約</em>（contract）とは、
クラス外部からみたクラス・メソッドの仕様のことで、
メソッドを設計する際、まずは規約を定める必要があります。
すなわち、規約とは「そのメソッドが何を出来るのか」、
「そのメソッドを呼び出すことで何が起こるのか」ということです。

そしてその後、定まった規約を満たすようにメソッド内部の<strong id="implementation" class="keyword">実装</strong>（implementation）を行います。
通常、規約と実装は切り離して考えるべきです。
クラス利用側からすると、
実際にメソッドの内部実装がどうなっているかはどうでもよくて、
外部仕様さえ分かればクラスを利用できるからです。

通常のメソッドは規約と実装を同時に定めますが、
「[抽象メソッド](oo_abstract.md#abmethod)」抽象メソッドは規約のみを定め、実装は派生クラスで行うことになります。

ここで注意しなければいけないのは、複数のクラスが同じ規約を満たす場合もあるということです。
また、同じ規約であっても、クラスが異なればその実装方法も異なります。
抽象メソッドの実装は派生クラスで行いますが、
派生クラスごとに実装方法が異なります。

例えば、
「[抽象メソッド、抽象クラス](oo_abstract.md)」で説明した <code>Person</code> クラスでは、
「<code>Age</code> プロパティが呼ばれたら年齢を答える」という規約を定めています。
<code>Person</code> の派生クラスではこの規約に従って <code>Age</code> プロパティを実装します。
クラスによって正直に答えたり、鯖を読んだりと、その実装方法は異なりますが、
「年齢を答える」という規約は満たされています。


## <a id="sec-generated-title-5"></a> <a id="interface"></a>C# のインターフェース

インターフェースとは、規約のみを定めるものです。
上述したように、C# では抽象メソッドを用いることでメソッドの規約のみを定めることが出来ます。
つまり、C# の<strong id="interface" class="keyword">インターフェース</strong>（interface）とは、抽象メソッドのみを持つ抽象クラスだと考えることが出来ます。

<figure>
	[![インターフェース](../../../../assets/media/ufcpp2000/csharp/fig/if0.png)](../../../../assets/media/ufcpp2000/csharp/fig/if0.png)
	<figcaption>インターフェース</figcaption>
</figure>


C# のインターフェースの定義は以下のようにして行います。

<pre class="source" title="インターフェース定義のしかた" lang="">
<code><span class="reserved">interface</span> <span class="input">インターフェース名</span>
{
  <span class="input">メソッド・プロパティの宣言</span>
}
</code></pre>


インターフェースの実装はクラスの継承と同じ構文で行います。

<pre class="source" title="インターフェースの実装" lang="">
<code><span class="reserved">class</span> <span class="input">クラス名</span> : <span class="input">インターフェース名</span>
{
  <span class="input">クラスの定義</span>
}
</code></pre>


クラスとよく似ていますが、インターフェースには以下に挙げるような特徴があります。

* メンバー変数(フィールド)を持つことが出来ない。

* static メソッドを持つことが出来ない。

* 宣言したメソッド・プロパティはすべて<code>public abstract</code>になる。

* 1つのクラスが複数のインターフェースを実装(多重継承)できる。

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、制限がいくつか緩和されています。
[後述](#dim)しますが、機能面で言うと、クラス(特に抽象クラス)との差は「フィールドを持てない代わりに多重継承できる」くらいの差になっています。

<!-- original-page-break -->

## <a id="sec-generated-title-6"></a> <a id="lib"></a>標準クラスライブラリ中のインターフェース

.NET Framework の標準クラスライブラリでは、汎用性の高いいくつかのインターフェースを標準で用意しています。
ここでは、そのうちのいくつかを紹介します。

### <a id="sec-generated-title-7"></a> <a id="IComparable"></a>IComparable

`IComparable<T>`インターフェイス(`System`名前空間)は、順序比較ができるものを表します。
配列の整列などに使います。

<pre class="source" title="IComparableの例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> 2次元上の点。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;see cref="</span><span class="type">IComparable</span>{<span class="type">T</span>}<span class="inactive">"/&gt;</span><span class="comment"> を実装している = 順序をつけられる。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">Point2D</span> : <span class="type">IComparable</span>&lt;<span class="type">Point2D</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">double</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Y { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Point2D(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
    {
        X = x;
        Y = y;
    }

    <span class="reserved">public</span> <span class="reserved">double</span> Radius =&gt; <span class="type">Math</span>.Sqrt(X * X + Y * Y);
    <span class="reserved">public</span> <span class="reserved">double</span> Angle =&gt; <span class="type">Math</span>.Atan2(Y, X);

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> 距離で順序を決める。</span>
    <span class="inactive">///</span><span class="comment"> 距離が全く同じなら偏角で順序付け。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;param name="</span>other<span class="inactive">"&gt;&lt;/param&gt;</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;returns&gt;&lt;/returns&gt;</span>
    <span class="reserved">public</span> <span class="reserved">int</span> CompareTo(<span class="type">Point2D</span> other)
    {
        <span class="reserved">var</span> r = Radius.CompareTo(other.Radius);
        <span class="reserved">if</span> (r != 0) <span class="reserved">return</span> r;
        <span class="reserved">return</span> Angle.CompareTo(other.Angle);
    }
}


<span class="reserved">class</span> <span class="type">IComparableSample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">const</span> <span class="reserved">int</span> N = 5;
        <span class="reserved">var</span> rand = <span class="reserved">new</span> <span class="type">Random</span>();
        <span class="reserved">var</span> data = <span class="type">Enumerable</span>.Range(0, N).Select(_ =&gt; <span class="reserved">new</span> <span class="type">Point2D</span>(rand.NextDouble(), rand.NextDouble())).ToArray();

        <span class="type">Console</span>.WriteLine(<span class="string">"元:"</span>);
        <span class="reserved">foreach</span> (<span class="reserved">var</span> p <span class="reserved">in</span> data) WriteLine(p);

        <span class="comment">// 並べ替えの順序に使える</span>
        <span class="type">Console</span>.WriteLine(<span class="string">"整列済み:"</span>);
        <span class="reserved">foreach</span> (<span class="reserved">var</span> p <span class="reserved">in</span> data.OrderBy(x =&gt; x)) WriteLine(p);
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> WriteLine(<span class="type">Point2D</span> p)
    {
        <span class="type">Console</span>.WriteLine(<span class="string">$"(</span>{p.X:<span class="string">N3</span>}<span class="string">, </span>{p.Y:<span class="string">N3</span>}<span class="string">), radius = </span>{p.Radius:<span class="string">N3</span>}<span class="string">, angle = </span>{p.Angle:<span class="string">N3</span>}<span class="string">"</span>);
    }
}
</code></pre>

### <a id="sec-generated-title-8"></a> <a id="collection"></a>コレクション

コレクション(参考: 「[コレクション概要](../../algorithm/collection/collection.md)」)には、
同じ操作ができる様々な実装方法があります(それぞれにメリット・デメリット、適切な利用場面があります)。

そして、C#では、操作の種類ごとにインターフェイスが標準で用意されていて、コレクションはそれらのインターフェイスを実装します。
以下の表示いくつか例を挙げます(いずれも`System.Collections.Generic`名前空間)。
(詳しくは[MSDN](https://msdn.microsoft.com/ja-jp/library/system.collections.generic.aspx)をご覧ください。)

<table>
<tr>
<th>インターフェイス</th>
<th>説明</th>
</tr>
<tr>
<td Markdown="1"> `IEnumerable<T>`</td>
<td Markdown="1">要素の列挙ができる。`foreach`ステートメントや、[LINQ](../data/sp3_linq.md#linq) to Objects で使える。</td>
</tr>
<tr>
<td Markdown="1">`ICollection<T>`</td>
<td Markdown="1">`IEnumerable<T>`に加えて、要素の追加(`Add`)、削除(`Remove`)などができたり、要素の個数が取れる。</td>
</tr>
<tr>
<td Markdown="1">`IList<T>`</td>
<td Markdown="1">`ICollection<T>`に加えて、[インデクサー](oo_indexer.md)を使った要素の読み書きができる。</td>
</tr>
<tr>
<td Markdown="1">`IDictionary<TKey, TValue> `</td>
<td Markdown="1">辞書アクセス(キーを使った値の検索)しての値の読み書きができる。</td>
</tr>
<tr>
<td Markdown="1">`IReadOnlyCollection<T>`<sup>※</sup></td>
<td Markdown="1">`IEnumerable<T>`に加えて、要素の個数が取れる。読み取り専用なので[共変](sp4_variance.md#covariance)。</td>
</tr>
<tr>
<td Markdown="1">`IReadOnlyList<T>`<sup>※</sup></td>
<td Markdown="1">`IReadOnlyCollection<T>`に加えて、[インデクサー](oo_indexer.md)を使った要素の読み取りができる。読み取り専用なので[共変](sp4_variance.md#covariance)。</td>
</tr>
<tr>
<td Markdown="1">`IReadOnlyDictionary<TKey, TValue>`<sup>※</sup></td>
<td Markdown="1">辞書アクセス(キーを使った値の検索)しての値の読み取りができる。</td>
</tr>
</table>

<h5 class="version version5">Ver. 5.0</h5>
<sup>※</sup> 読み取り専用系のインターフェイスは .NET Framework 4.5 (C# 5.0と同時期)で追加されました。

このうち、`IEnumerable`と`IReadIReadOnlyList`の例を挙げておきます。

<pre class="source" title="IEnumerableの例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> 連結リスト。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;see cref="</span><span class="type">IEnumerable</span>{<span class="type">T</span>}<span class="inactive">"/&gt;</span><span class="comment"> を実装している = データの列挙ができる。複数のデータを束ねてる。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;typeparam name="</span><span class="type">T</span><span class="inactive">"&gt;&lt;/typeparam&gt;</span>
<span class="reserved">class</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; Next { <span class="reserved">get</span>; }

    <span class="reserved">public</span> LinkedList(<span class="type">T</span> value) : <span class="reserved">this</span>(value, <span class="reserved">null</span>) { }
    <span class="reserved">private</span> LinkedList(<span class="type">T</span> value, <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; next) { Value = value; Next = next; }

    <span class="reserved">public</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; Add(<span class="type">T</span> value) =&gt; <span class="reserved">new</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt;(value, <span class="reserved">this</span>);

    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt; GetEnumerator()
    {
        <span class="reserved">if</span>(Next != <span class="reserved">null</span>)
            <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> Next)
                <span class="reserved">yield</span> <span class="reserved">return</span> x;
        <span class="reserved">yield</span> <span class="reserved">return</span> Value;
    }

    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span>.GetEnumerator() =&gt; GetEnumerator();
}

<span class="reserved">class</span> <span class="type">IEnumerableSample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">LinkedList</span>&lt;<span class="reserved">int</span>&gt;(1);
        <span class="reserved">var</span> b = a.Add(2).Add(3).Add(4);

        <span class="comment">// foreach で使える(これは IEnumerable 必須ではない)</span>
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> b)
            <span class="type">Console</span>.WriteLine(x);

        <span class="comment">// string.Join で使える</span>
        <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, b));

        <span class="comment">// LINQ で使える</span>
        <span class="type">Console</span>.WriteLine(b.Sum());
    }
}
</code></pre>

<pre class="source" title="IReadOnlyListの例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;
<span class="reserved">using</span> System.Collections.Generic;

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> 4次元上の点。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;see cref="</span><span class="type">IReadOnlyList</span>{<span class="type">T</span>}<span class="inactive">"/&gt;</span><span class="comment"> を実装している = </span><span class="inactive">&lt;see cref="</span><span class="type">IEnumerable</span>{<span class="type">T</span>}<span class="inactive">"/&gt;</span><span class="comment">に加えて、インデックス指定で値を読める。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">Point4D</span> : <span class="type">IReadOnlyList</span>&lt;<span class="reserved">double</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">double</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Z { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> W { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Point4D(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z, <span class="reserved">double</span> w) { X = x; Y = y; Z = z; W = w; }

    <span class="reserved">public</span> <span class="reserved">double</span> <span class="reserved">this</span>[<span class="reserved">int</span> index]
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">switch</span> (index)
            {
                <span class="reserved">default</span>:
                <span class="reserved">case</span> 0: <span class="reserved">return</span> X;
                <span class="reserved">case</span> 1: <span class="reserved">return</span> Y;
                <span class="reserved">case</span> 2: <span class="reserved">return</span> Z;
                <span class="reserved">case</span> 3: <span class="reserved">return</span> W;
            }
        }
    }

    <span class="reserved">public</span> <span class="reserved">int</span> Count =&gt; 4;

    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="reserved">double</span>&gt; GetEnumerator()
    {
        <span class="reserved">yield</span> <span class="reserved">return</span> X;
        <span class="reserved">yield</span> <span class="reserved">return</span> Y;
        <span class="reserved">yield</span> <span class="reserved">return</span> Z;
        <span class="reserved">yield</span> <span class="reserved">return</span> W;
    }

    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span>.GetEnumerator() =&gt; GetEnumerator();
}

<span class="reserved">class</span> <span class="type">IReadOnlyListSample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p1 = <span class="reserved">new</span> <span class="type">Point4D</span>(1, 2, 3, 4);
        <span class="reserved">var</span> p2 = <span class="reserved">new</span> <span class="type">Point4D</span>(3, 7, 5, 11);

        <span class="comment">// X, Y, Z, W の代わりに 0, 1, 2, 3 のインデックスで値を読み出し</span>
        <span class="reserved">var</span> innerProduct = 0.0;
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 4; i++)
            innerProduct += p1[i] * p2[i];

        <span class="type">Console</span>.WriteLine(innerProduct);
    }
}
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="IDisposable"></a>IDisposable

`IDisposable`インターフェイス(`System`名前空間)は、[ガベージ コレクション](../resource/rm_gc.md#garbage-collection)任せではなく、
明示的なタイミングで破棄処理を行いたいものに使います。詳細は「[リソースの破棄](../resource/oo_dispose.md)」で説明します。

<pre class="source" title="IDisposableの例">
<coe><reserved></span><span class="reserved">using</span> System;

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;see cref="</span><span class="type">IDisposable</span><span class="inactive">"/&gt;</span><span class="comment"> を実装している = 使い終わったら明示的に Dispose を呼ぶ必要がある。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">Stopwatch</span> : <span class="type">IDisposable</span>
{
    System.Diagnostics.<span class="type">Stopwatch</span> _s = <span class="reserved">new</span> System.Diagnostics.<span class="type">Stopwatch</span>();

    <span class="reserved">public</span> Stopwatch() { _s.Start(); }

    <span class="reserved">public</span> <span class="reserved">void</span> Dispose()
    {
        _s.Stop();
        <span class="type">Console</span>.WriteLine(_s.Elapsed);
    }
}

<span class="reserved">class</span> <span class="type">IDisposableSample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// using ブロックを抜けたら自動的に Dispose が呼ばれる</span>
        <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">Stopwatch</span>())
        {
            <span class="reserved">var</span> t = T(12, 6, 0);
        }
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> T(<span class="reserved">int</span> x, <span class="reserved">int</span> y, <span class="reserved">int</span> z) =&gt; x &lt;= y ? y : T(T(x - 1, y, z), T(y - 1, z, x), T(z - 1, x, y));
}
</code></pre>

<!-- original-page-break -->

## <a id="sec-generated-title-10"></a> <a id="multiple"></a>複数のインターフェイスを実装

C#は多重継承を認めていません(1つのクラスしか[継承](oo_inherit.md)できない)。この制約はクラスに対してのみかかります。すなわち、インターフェイスは複数実装できます。

例えば、以下のような型を作れます。

<pre class="source" title="">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Id</span> : <span class="type">IComparable</span>&lt;<span class="type">Id</span>&gt;, <span class="type">IEquatable</span>&lt;<span class="type">Id</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">int</span> CompareTo(<span class="type">Id</span> other) =&gt; Value.CompareTo(other.Value);

    <span class="reserved">public</span> <span class="reserved">bool</span> Equals(<span class="type">Id</span> other) =&gt; Value == other.Value;
}
</code></pre>

## <a id="sec-generated-title-11"></a> <a id="orverload"></a>型引数違いのジェネリック インターフェイス

C#では、[オーバーロード](../structured/st_function.md#overload)解決ができる限り、同名のメンバーを持つインターフェイスを複数、普通に実装することができます(オーバーロード解決できない場合には、次節の[明示的実装](#explicit-impl)が必要になります)。

これは特に、[ジェネリック](sp2_generics.md#generics)なインターフェイスを、型引数違いで複数実装する際に有効です。

例えば、標準ライブラリの`IEquatable<T>`インターフェイス(`System`名前空間)について、異なる型引数で複数実装できます。
`A`と`B`という2つのクラスがあったとして、`IEquatable<A>`と`IEquatable<B>`という2つの実装を持てます。

具体的な用途としては、例えば、以下のような場面で有効です。

- 図形全般を表す`Shape`型がある
- `Shape`から派生した、矩形型`Rectangle`がある
  - `Rectangle`は、幅と高さの両方の比較で等値判定する
- `Shape`から派生した、円型`Circle`がある
  - `Circle`は、半径の比較で等値判定する
- `Shape`は、矩形同士、円同士でだけ等値判定をする。型が違う場合はその時点で不一致

この条件下では、それぞれのクラスに以下のようにインターフェイスを持てます。

- `Shape`は他の`Shape`と比較できるので、`IEquatable<Shape>`を実装できる
- `Rectangle`は他の`Rectangle`と比較できるので、`IEquatable<Rectangle>`を実装できる
  - `Rectangle`は`Shape`から派生しているので、`IEquatable<Shape>`でもある
- `Circle`は他の`Circle`と比較できるので、`IEquatable<Circle>`を実装できる
  - `Circle`は`Shape`から派生しているので、`IEquatable<Shape>`でもある

これを、以下のようなコードで実装できます。

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">Shape</span> : <span class="type">IEquatable</span>&lt;<span class="type">Shape</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">bool</span> Equals(<span class="type">Shape</span> other);
}

<span class="reserved">class</span> <span class="type">Rectangle</span> : <span class="type">Shape</span>, <span class="type">IEquatable</span>&lt;<span class="type">Rectangle</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">double</span> Width { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Height { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">bool</span> Equals(<span class="type">Shape</span> other) =&gt; Equals(other <span class="reserved">as</span> <span class="type">Rectangle</span>);

    <span class="reserved">public</span> <span class="reserved">bool</span> Equals(<span class="type">Rectangle</span> other)
        =&gt; other != <span class="reserved">null</span> &amp;&amp; Width == other.Width &amp;&amp; Height == other.Height;
}

<span class="reserved">class</span> <span class="type">Circle</span> : <span class="type">Shape</span>, <span class="type">IEquatable</span>&lt;<span class="type">Circle</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">double</span> Radius { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">bool</span> Equals(<span class="type">Shape</span> other) =&gt; Equals(other <span class="reserved">as</span> <span class="type">Circle</span>);

    <span class="reserved">public</span> <span class="reserved">bool</span> Equals(<span class="type">Circle</span> other)
        =&gt; other != <span class="reserved">null</span> &amp;&amp; Radius == other.Radius;
}
</code></pre>

## <a id="sec-generated-title-12"></a> <a id="explicit-impl"></a>明示的実装

インターフェイスの場合、1つのクラスで複数のインターフェイスを実装することができます。
このとき、複数のインターフェイスに同名・同引数のメソッドがあった場合、衝突が起こりえます。

例えば以下の例を見てください。`IAccumulator`インターフェイスと`IGroup<T>`インターフェイスがどちらも`Add`メソッドを持っていて、それを両方実装している`ImplicitImplementation`クラスは、1つの`Add`メソッドが2つの役割を兼ねることになります。

<pre class="source" title="複数のインターフェイスの実装">
<code><reserved></span><span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">interface</span> <span class="type">IAccumulator</span>
{
<em>    <span class="reserved">void</span> Add(<span class="reserved">int</span> value);</em>
    <span class="reserved">int</span> Sum { <span class="reserved">get</span>; }
}

<span class="reserved">interface</span> <span class="type">IGroup</span>&lt;<span class="type">T</span>&gt;
{
<em>    <span class="reserved">void</span> Add(<span class="type">T</span> item);</em>
    <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; Items { <span class="reserved">get</span>; }
}

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> 1つの</span><span class="inactive">&lt;see cref="</span>Add(<span class="reserved">int</span>)<span class="inactive">"/&gt;</span><span class="comment">で、2つのインターフェイスの実装を担うんであれば特に問題は出ない。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">ImplicitImplementation</span> : <span class="type">IAccumulator</span>, <span class="type">IGroup</span>&lt;<span class="reserved">int</span>&gt;
{
<em>    <span class="reserved">public</span> <span class="reserved">void</span> Add(<span class="reserved">int</span> x)
    {
        Sum += x;
        _items.Add(x);
    }</em>

    <span class="reserved">public</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Items =&gt; _items;
    <span class="reserved">private</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; _items = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();

    <span class="reserved">public</span> <span class="reserved">int</span> Sum { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
}
</code></pre>

元々役割を兼ねたい場合はこれでいいんですが、そうでないこともあります。
こういう時に使うのが、<strong id="explicit-interface-method" class="keyword">インターフェイスの明示的実装</strong>です。
メンバーを定義する際に、メンバー名の前に「インターフェイス名 + `.`」を加えます。
例えば、メソッドの場合は以下のように書きます。

<pre class="source" title="関数の書式" lang="">
<code><span class="input">戻り値の型</span> <em><span class="input">インターフェイス名</span></em>.<span class="input">メソッド名</span>(<span class="input">引数一覧</span>)
{
    <span class="input">メソッド本体(具体的な処理)</span>
}
</code></pre>

この場合、アクセス修飾子(`public`や`private`などは付けれません。)

これを使って、先ほどの2つのインターフェイスの`Add`メソッドに対して別実装を与えてみましょう。
以下のようになります。

<pre class="source" title="インターフェイスの明示的実装の例">
<code><inactive></span><span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;see cref="</span><span class="type">IAccumulator</span>.Add(<span class="reserved">int</span>)<span class="inactive">"/&gt;</span><span class="comment">と、</span><span class="inactive">&lt;see cref="</span><span class="type">IGroup</span>{<span class="reserved">int</span>}.Add(<span class="reserved">int</span>)<span class="inactive">"/&gt;</span><span class="comment">が完全に被るので、</span>
<span class="inactive">///</span><span class="comment"> 別の実装を与えたければ明示的実装が必要。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">ExplicitImplementation</span> : <span class="type">IAccumulator</span>, <span class="type">IGroup</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="reserved">void</span> <span class="type">IAccumulator</span>.Add(<span class="reserved">int</span> value) =&gt; Sum += value;

    <span class="reserved">void</span> <span class="type">IGroup</span>&lt;<span class="reserved">int</span>&gt;.Add(<span class="reserved">int</span> item) =&gt; _items.Add(item);

    <span class="reserved">public</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Items =&gt; _items;
    <span class="reserved">private</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; _items = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();

    <span class="reserved">public</span> <span class="reserved">int</span> Sum { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
}
</code></pre>

この例のように、明示的実装はメンバー単位で切り替えれます。
この例の場合は、`Add`だけが明示的実装で、残りの`Sum`や`Items`は通常の(暗黙的な)実装です。

ちなみに、明示的実装をしたメンバーは、そのクラスの変数から直接は利用できなくなります。
一度インターフェイスのキャストしてから呼び出すことになります。

<pre class="source" title="明示的実装したインターフェイスの呼び出し例">
<code><reserved></span><span class="reserved">using</span> System;

<reserved></span><span class="reserved">class</span> <span class="type">ExpliciteImplementationSample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 1つのAddで両方の債務を担ってるので2重集計される</span>
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">ImplicitImplementation</span>();
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
        {
            Accumulate(a, i);
            AddItem(a, i);

            <span class="comment">// 通常の実装なので、普通に Add(i) を呼ぶことも可能</span>
            <span class="comment">//a.Add(i);</span>
        }
        <span class="type">Console</span>.WriteLine(<span class="string">$"sum = </span>{a.Sum}<span class="string">, items = </span>{<span class="reserved">string</span>.Join(<span class="string">", "</span>, a.Items)}<span class="string">"</span>);

        <span class="comment">// 明示的実装を使って2つのAddを別実装したので個別集計される。</span>
        <span class="reserved">var</span> b = <span class="reserved">new</span> <span class="type">ExplicitImplementation</span>();
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
        {
            Accumulate(b, i);
            AddItem(b, i);

            <span class="comment">// 明示的実装の場合、一度インターフェイスにキャストしてからでないと Add(i) は呼べない。</span>
            <span class="comment">// 例えば以下のコメントを外すとコンパイル エラー。</span>
            <span class="comment">//b.Add(i);</span>
        }
        <span class="type">Console</span>.WriteLine(<span class="string">$"sum = </span>{b.Sum}<span class="string">, items = </span>{<span class="reserved">string</span>.Join(<span class="string">", "</span>, b.Items)}<span class="string">"</span>);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Accumulate(<span class="type">IAccumulator</span> x, <span class="reserved">int</span> value) =&gt; x.Add(value);

    <span class="reserved">static</span> <span class="reserved">void</span> AddItem&lt;<span class="type">T</span>&gt;(<span class="type">IGroup</span>&lt;<span class="type">T</span>&gt; g, <span class="type">T</span> item) =&gt; g.Add(item);
}
</code></pre>

まとめると、インターフェイスの明示的実装を使うと、以下のような状態になります。

- 同じ名前のメンバーを持ったインターフェイスを複数同時に実装できる
- 明示的実装したメンバーは、いったんインターフェイス型にキャストしてからでないと呼べなくなる


<!-- original-page-break -->

## <a id="sec-generated-title-13"></a> <a id="usage"></a>インターフェイスの明示的実装の用途

もう少し具体的に、インターフェイスの明示的実装の用途をいくつか紹介しましょう。

インターフェイスの明示的実装は、同じ名前のメンバーを持ったインターフェイスを複数同時に実装できるようにするための機能です。
では、それが必要になる場面というのは具体的にはどういう状況でしょう。
また、メンバーをいったんインターフェイス型にキャストしてからでないと呼べなくなるという性質も、有効に使える場面があります。

### <a id="sec-generated-title-14"></a> <a id="legacy-member"></a>消したいけど消せないメソッドを隠す

まず一般論として、public なものは、足すより消す方が難しいです。他人の作ったライブラリを使っていて、ある日突然、自分の使っているメソッドが消えたらどうでしょう。自分は何もしていないのに、自分の書いたコードがコンパイルできなくなります。

この問題はライブラリが広く使われれば使われるほど影響範囲が広がります。標準ライブラリに至っては、まず削除はできないものだと思ってください。

その結果、.NETの標準ライブラリには、いくつか、消したくても消せないものがあります。代表例として、以下のようなものがあります。

- 非ジェネリック版の`IEnumerable`インターフェイス(`System.Collections`名前空間)
  - ジェネリック版の`IEnumerable<T>`(`System.Collections.Generic`名前空間)が、この非ジェネリック版から派生している
- `ICollection<T>`インターフェイス(`System.Collections.Generic`名前空間)の`IsReadOnly`

これらを「消したい」理由については後で補足しますが、とりあえず、消したくても消してはいけません。

これらのインターフェイスを実装する際、その消したいけど消せないメソッドも一緒に実装させられるという苦行が待っています。
せめて、そんなもうあまり使わなくなったメンバーはpublicにしたくないわけです。
そこで、明示的実装の、メンバーを隠せる性質が使えます。

例として`IEnumerable`インターフェイスを隠す方法を示しましょう。というか、すでに[前述](#collection)の例で使っていたりします。再掲すると以下の通りです。

<pre class="source" title="IEnumerableの例">
<span class="reserved">using</span> System.Collections;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; Next { <span class="reserved">get</span>; }

    <span class="reserved">public</span> LinkedList(<span class="type">T</span> value) : <span class="reserved">this</span>(value, <span class="reserved">null</span>) { }
    <span class="reserved">private</span> LinkedList(<span class="type">T</span> value, <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; next) { Value = value; Next = next; }

    <span class="reserved">public</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt; Add(<span class="type">T</span> value) =&gt; <span class="reserved">new</span> <span class="type">LinkedList</span>&lt;<span class="type">T</span>&gt;(value, <span class="reserved">this</span>);

    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt; GetEnumerator()
    {
        <span class="reserved">if</span>(Next != <span class="reserved">null</span>)
            <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> Next)
                <span class="reserved">yield</span> <span class="reserved">return</span> x;
        <span class="reserved">yield</span> <span class="reserved">return</span> Value;
    }

    <span class="comment">// 明示的実装。こいつは、IEnumerableを介さない限り見えなくなる</span>
    <em><span class="type">IEnumerator</span> <span class="type">IEnumerable</span>.GetEnumerator() =&gt; GetEnumerator();</em>
}
</code></pre>

#### <a id="sec-generated-title-15"></a> <a id="legacy-nongeneric"></a>補足1： 非ジェネリック インターフェイス

特に、[ジェネリック](sp2_generics.md#generics)関連に多いです。
ジェネリックが.NET 1.0には間に合わず、2.0からの追加だったので、多くのインターフェイスで非ジェネリック版と、ジェネリック版が2重保守されています。

`IEnumerable`もその例の1つで、.NET 1.0時代に非ジェネリック版が、2.0でジェネリック版が入りました。2.0で入ったジェネリック版は、1.0時代のコードとの互換性のために非ジェネリック版から派生しています。もし、最初から.NETにジェネリックがあれば、非ジェネリック版の機能は不要でした。

#### <a id="sec-generated-title-16"></a> <a id="legacy-isreadonly"></a>補足2: IsReadOnly

インターフェイスが増えるというのはそれなりのコストがかかるそうで、.NETリリース初期の頃は、インターフェイスを減らす方向で設計を進めたそうです。`ICollection<T>`インターフェイスが`IsReadOnly`というプロパティを持っているのはその頃の名残です。しかし今となっては、インターフェイスが増えてもいいからちゃんと「読み取り専用なコレクション」と「書き換え可能なコレクション」は別インターフェイスに分けるべきだということになっています(そのため、.NET 4.5で、`IReadOnlyCollection<T>`インターフェイスが(`System.Collections.Generic`名前空間)が追加されました)。

つまり、今と昔で以下のような思想の差があります。

- 昔: インターフェイスを増やしたくないので、コレクションが読み取り専用か書き換え可能かはプロパティで返していた
- 今: 読み取り専用なら`IReadOnlyCollection<T>`インターフェイスを、書き換え可能なら`ICollection<T>`インターフェイスを使う

こうなると、`IsReadOnly`プロパティははっきり言って邪魔です。`ICollection<T>`を選んだ時点で書き換え可能にしたいんだから、おそらくは常にtrueを返すだけになるでしょう。

### <a id="sec-generated-title-17"></a> <a id="access-restriction"></a>メンバーのアクセスを制限する

(書きかけ)

- internal set 隠し
- internal interface 実装できるのとの組み合わせ

### <a id="sec-generated-title-18"></a>ジェネリック版とobject版

(書きかけ)

ときどき、「特定のインターフェイスを実装している時だけ特別な動作を挟む」みたいな処理を書きたい場合があります。

- この as 判定用に `interface IX { object X { get; } }`
- でも、人手で使うとき用にジェネリック版を用意して `interface IX<T> : IX { new T X { get; } }`


<!-- original-page-break -->


## <a id="sec-generated-title-19"></a> <a id="dim"></a>インターフェイスのデフォルト実装

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 (.NET Core 3.0)で、インターフェイスの制限が緩和されました。
以下のようになります。

- メソッド、[プロパティ](oo_property.md)、[インデクサー](oo_indexer.md)、[イベント](../functional/sp_event.md)のアクセサーの実装を持てるようになった
- [アクセシビリティ](oo_conceal.md#level)を明示的に指定できるようになった
- [静的メンバー](oo_static.md)を持てるようになった
  - [入れ子](../package/toplevelaccessibility.md#key-nested)の型も含む

これら指して「インターフェイスのデフォルト実装」(default implementations of interfaces)と呼びます<sup>※</sup>。
(1番目の「インターフェイスが関数メンバーの実装を持てる」というのを主目的に検討されたもので、
言葉の意味だけからすると、狭義にはこの1番目の機能こそが「デフォルト実装」です。
ただ、これのついでに実装されたものなので2番目、3番目には具体的な名前がついていません。)

このようにインターフェイスに対する制限を減らすのであれば、
「クラス(特に[抽象クラス](oo_abstract.md#abclass))との区別が今でも必要なのかどうか」
というような議論もありました。
今、1から文法を決めれるとしても残したい区別は、
「フィールドを持てない代わりに多重継承できる」という点くらいで、
他の差は「歴史的経緯に由来するもの」という側面が強いです。
(インターフェイスでのフィールド定義は、多重継承、特に、[ひし形継承](https://ja.wikipedia.org/wiki/%E8%8F%B1%E5%BD%A2%E7%B6%99%E6%89%BF%E5%95%8F%E9%A1%8C)との相性が悪く、複雑度のわりにメリットが少ないです。)

歴史的経緯に由来して、以下のような挙動はクラスと揃えることができませんでした。

- アクセシビリティ未指定のときなど、既定の挙動が違う
- 派生インターフェイスでの[オーバーライド](oo_polymorphism.md#override)は明示的実装が必須
- デフォルト実装を持っているメンバーは、派生クラス・派生インターフェイスからは直接呼べない(親へのキャストが必要)

ここでいう「歴史的経緯」は、
既存機能・既存コードへの影響を最小限にとどめるためや、
.NET ランタイム側の修正が簡単な範囲に収めるために残ってしまった差です。

<sup>※</sup> Java 由来で、「インターフェイスのデフォルト メソッド」(default interface method、略して DIM)と呼ばれたりもします。

### <a id="sec-generated-title-20"></a> <a id="runtime-feature"></a>ランタイム側の修正

インターフェイスのデフォルト実装は C# コンパイラー上のトリックだけでは実装できず、 .NET ランタイム側の対応が必要な機能です。
C# 8.0 以降を使っていても、ターゲットとなるランタイム(TargetFramework)が古いと使えません。
.NET Core 3.0 (かそれと同世代)以降のランタイムである必要があります。
.NET Framework 側では対応予定はない(.NET Core 3.0 と同世代な .NET Framework 4.8 でも未対応)です。

詳しくは以前書いたブログ「[RuntimeFeature クラス](../../../blog/2018/12/runtimefeature/index.md)」で説明しています。

### <a id="sec-generated-title-21"></a> <a id="dim-motivation"></a>導入の動機

この制限緩和には、以下のような動機ががあります。

- 既存のインターフェイスにメンバーを追加しても破壊的変更にならない
- 同様の機能を持っている Android (Java (8以降))や iOS (Swift)との相互運用
- [トレイト](https://ja.wikipedia.org/wiki/%E3%83%88%E3%83%AC%E3%82%A4%E3%83%88)的にも使える

#### <a id="sec-generated-title-22"></a> <a id="breaking-change"></a>メンバー追加による破壊的変更

最大の動機は1番目の「破壊的変更にならない」という部分です。
抽象メンバーは派生クラスでの実装が必須で、実装しなければコンパイル エラーを起こします。
その結果、「後から追加したら派生クラスがコンパイル エラーを起こす」という状態になります。

<pre class="source" title="抽象メンバーの追加は破壊的変更">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">X</span>();
 
    <span class="comment">// 後から追加したものとする</span>
    <span class="reserved">void</span> <span class="method">Y</span>();
}
 
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">I</span>
{
    <span class="comment">// X は実装してある</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">X</span>() { }
 
    <span class="comment">// C が I を実装するコードを書いたころには Y がなかったので OK。</span>
    <span class="comment">// Y を追加したことでコンパイル エラーに。</span>
}
</code></pre>

この問題を回避するには、これまでは抽象クラスを使うしかありませんでした。
抽象クラスは抽象クラスで、多重継承ができないという別の制限があるので完全な回避策にはなりません。

(あるいは、語尾にExとか2とか3とかが付いた新しいインターフェイスを作ったり、
ユーザーに破壊的変更を受け入れてもらうという手もありますが、
どちらもかなり最終手段です。)

そこで、C# 8.0 ではインターフェイスも実装を持てるようにしました。
Java 8 の同様の機能も同じ動機に基づいています。
機能名が「デフォルト実装」(default = de(脱) + fault(不備))なのもこのためです。
「本来なくてはならない実装がない」という状態(fault)に対して既定動作を与えることで、エラーを回避します。

「規約だけを定める」というクリーンさを犠牲にしてでも、このメリットは大きいです。

この観点で言うと、インターフェイスのデフォルト実装はライブラリ作者のための機能になります。
特に、広く使われているライブラリほど破壊的変更はできないものなので、
一番恩恵を受けるのは[corefx](https://github.com/dotnet/corefx/) (.NET Core の標準ライブラリ部分)チームだったりします。

(小さい規模だと、自分たちで作ったインターフェイスを自分たちで使うということが多くなりますし、
その場合は別に破壊的変更が気になること自体あまりありません。)

#### <a id="sec-generated-title-23"></a> <a id="trait"></a>トレイト用途

[トレイト](https://ja.wikipedia.org/wiki/%E3%83%88%E3%83%AC%E3%82%A4%E3%83%88)的な用途としては、フィールドを持てないなどの制限があるので、恩恵は限定的です。
C# の場合には[拡張メソッド](../functional/sp3_extension.md)でも似たようなことができるので、特に恩恵は少なめです。

「拡張メソッドでもできなくはないけども、[virtual](oo_polymorphism.md#virtual_method) な実装方法を取りたい」みたいな場合に使います。

よく例に上がるのが [LINQ](../data/sp3_linq.md) to Object の `Count` メソッドです。
`IEnumerable<T>`(`System.Collections.Generic`名前空間) に対する `Count`(含まれている要素数を数える)は、汎用的に書くなら以下のように書くしかありません。

<pre class="source" title="汎用的な Count">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Count</span>&lt;<span class="type">T</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; <span class="variable">source</span>)
{
    <span class="reserved">var</span> <span class="variable">count</span> = 0;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="variable">source</span>) ++<span class="variable">count</span>;
    <span class="control">return</span> <span class="variable">count</span>;
}
</code></pre>

ただ、配列や`List<T>`など、元々長さを持っている型であれば、この `foreach` は全くの無駄で、できれば元々フィールドとして持っている長さを直接返したいです。
そのため、[実際の `Count` の実装](https://source.dot.net/#System.Linq/System/Linq/Count.cs)には `is` 演算子による分岐が挟まっています。
この分岐をするくらいなら、拡張メソッドではなく、インターフェイスのデフォルト実装としてトレイト的に実装する方が素直(virtual なので `ICollection` 側でオーバーライドできる)です。

### <a id="sec-generated-title-24"></a> <a id="function-implementation"></a>実装を持つ関数メンバー

ということで、インターフェイスが実装を持てるようになりました。

<pre class="source" title="デフォルト実装">
<code><span class="reserved">using</span> System;
 
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">X</span>();
 
    <span class="comment">// 後から追加しても、デフォルト実装を持っているので平気</span>
    <span class="reserved">void</span> <span class="method">Y</span>() { }
}
 
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">I</span>
{
    <span class="comment">// X だけ実装していればとりあえず大丈夫</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">X</span>() { }
}
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">I</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">X</span>() { }
 
    <span class="comment">// Y も実装。I 越しでもちゃんとこれが呼ばれる。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Y</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>() =&gt; <span class="method">M</span>(<span class="reserved">new</span> <span class="type">B</span>());
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">I</span> <span class="variable">i</span>) =&gt; <span class="variable">i</span>.<span class="method">Y</span>();
}
</code></pre>

<pre class="console" title="デフォルト実装">
<code>B
</code></pre>

ただし、以下の制限は残っています。

- インスタンス [フィールド](../structured/st_struct.md#field) は持てない
- インスタンス [コンストラクター](oo_construct.md)、[ファイナライザー](../resource/rm_destructor.md)は持てない

主目的(新規メンバー追加での破壊的変更の回避)のためにはインスタンス メンバーだけ実装を持てればいいわけですが、ついでにいろいろと緩和されています。

#### <a id="sec-generated-title-25"></a> <a id="static-member"></a>静的メンバー

静的メンバーも持てるようになりました。
インスタンス メンバーと違って、静的コンストラクターや静的フィールドは持てます。
[定数](../start/sp_const.md)や、[演算子](oo_operator.md)、[入れ子](../package/toplevelaccessibility.md#key-nested)の型も持てます。

<pre class="source" title="">
<code>

</code></pre>

<pre class="source" title="インターフェイスの静的メンバー">
<code><span class="reserved">using</span> System;
 
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">static</span> <span class="type">I</span>() { }
    <span class="reserved">static</span> <span class="reserved">int</span> _field;
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Method</span>() =&gt; ++_field;
    <span class="reserved">const</span> <span class="reserved">int</span> Constant = 1;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">I</span> <span class="reserved">operator</span> +(<span class="type">I</span> x) =&gt; x;
    <span class="reserved">class</span> <span class="type">Inner</span> { }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">I</span>.<span class="method">Method</span>());
        <span class="type">I</span>.<span class="type">Inner</span> <span class="variable">inner</span>;
    }
}
</code></pre>

次節で説明する通り、アクセシビリティは特に指定しなければ `public` です。
明示すれば `protected` や `private` にすることもできます。

#### <a id="sec-generated-title-26"></a> <a id="accessibility"></a>アクセシビリティ

C# 7.3 までは、インターフェイスのメンバーは常に `public` で `virtual` でした。
C# 8.0 からは、明示的に指定することでクラスと同じく、`protected` などのアクセシビリティを指定できます。

<pre class="source" title="インターフェイスのメンバーにアクセシビリティを明示">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// 未指定の挙動は今まで通り、public virtual。</span>
    <span class="reserved">void</span> <span class="method">Public</span>()
    {
        <span class="method">Private</span>();
    }
 
    <span class="comment">// 明示することでそれ以外のアクセシビリティを指定できるように。</span>
    <span class="comment">// protected なら派生クラス・派生インターフェイスから、</span>
    <span class="comment">// private なら自分自身からのみ呼び出し可能。</span>
    <span class="reserved">protected</span> <span class="reserved">void</span> <span class="method">Protected</span>() { }
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Private</span>() { }
}
 
<span class="reserved">interface</span> <span class="type">IDerived</span> : <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="method">Public</span>();
        <span class="method">Protected</span>();
        <span class="comment">// Private(); はダメ</span>
    }
}
</code></pre>

ちなみに、省略時の挙動は今まで通り `public virtual` です。
クラスの場合の省略時は `private` なので、クラスとは挙動が異なります。

また、[後述しますが](#mics-restriction)、`protected` なメンバーにアクセスできるのは派生インターフェイスからだけです。
クラスの場合、派生(実装)しているクラスであっても `protected` メンバーは見えません。

#### <a id="sec-generated-title-27"></a> <a id="default-virtual"></a>既定で仮想

アクセシビリティを明示して `protected` や `internal` などを付けても、`protected virtual` や `internal virtual` の意味になります。
仮想呼び出しになる方が既定動作です。
これも、クラスとは既定動作が違います。
C# のクラスは何も指定しなければ仮想関数にはなりません。

`private` か、あるいは明示的に `sealed` を指定した時だけ、非仮想になります。

<pre class="source" title="インターフェイスは既定で virtual">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// 未指定の挙動は今まで通り、public virtual。</span>
    <span class="reserved">void</span> <span class="method">Public</span>() { }
 
    <span class="comment">// これも実際には protected virtual。</span>
    <span class="reserved">protected</span> <span class="reserved">void</span> <span class="method">Protected</span>() { }
 
    <span class="comment">// private メンバーは派生側から呼ばれないので virtual である必要がない。</span>
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Private</span>() { }
 
    <span class="comment">// sealed を明示すれば virtual ではなくせる。</span>
    <span class="reserved">sealed</span> <span class="reserved">void</span> <span class="method">Sealed</span>() { }
}
</code></pre>

ちなみに、派生インターフェイスで基底インターフェイスの `virtual` なメンバーに `sealed` を付けることはできません。
一度 `virtual` になったものはずっと `virtual` です。

<pre class="source" title="">
<code><span class="reserved">interface</span> <span class="type">IDerived</span> : <span class="type">I</span>
{
    <span class="comment">// 基底側で virtual なものを派生側で sealed に変えることはできない。</span>
    <span class="comment">// コンパイル エラーになる。</span>
    <span class="reserved">sealed</span> <span class="reserved">void</span> <span class="type">I</span>.<span class="error"><span class="method">Protected</span></span>() { }
}
</code></pre>

(多重継承があり得るインターフェイスでは、ある経路で `sealed` を付けてオーバーライドを禁止しても、別のある経路では `sealed` が付いていないなど、不整合があるため認められません。)

#### <a id="sec-generated-title-28"></a> <a id="multiple-inheritance"></a>多重継承

クラスとの最大の差は多重継承ができる点です。

デフォルト実装があっても、
フィールドさえ持たなければ多重継承の実装はそれほど難しいものではないので、
パフォーマンスなどへの悪影響はありません。
(参考: [「インターフェースのデフォルト実装」の導入（前編）](https://www.buildinsider.net/column/iwanaga-nobuyuki/013))

ただ、「別経路で同じメソッドに別実装が与えられている」という場合があって、
そこでの呼び分けが問題になることがあります。

例えば以下のようなコードでは、どの実装を使いたいのか不明瞭なので、コンパイル エラーを起こします。

<pre class="source" title="実装が不明瞭な場合はコンパイル エラーに">
<code><span class="reserved">using</span> System;
 
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A.M&quot;</span>);
}
 
<span class="reserved">interface</span> <span class="type">IB</span> : <span class="type">IA</span>
{
    <span class="reserved">void</span> <span class="type">IA</span>.<span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B.M&quot;</span>);
}
 
<span class="reserved">interface</span> <span class="type">IC</span> : <span class="type">IA</span>
{
    <span class="reserved">void</span> <span class="type">IA</span>.<span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;C.M&quot;</span>);
}
 
<span class="comment">// IB にも IC にも M の実装があって、どちらを使いたいのか不明瞭(コンパイル エラー)。</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="error"><span class="type">IB</span></span>, <span class="type">IC</span>
{
}
</code></pre>

ちなみに、「コンパイルするときには `IB` にしか `M` の実装がなかったからコンパイルできたけど、後から `IC` に `M` を追加した状態のライブラリに差し替えた」というような状況もあり得て、この場合は実行時エラーになります。
`AmbiguousImplementationException`(`System.Runtime` 名前空間)が throw されます。

もちろん、自分自身が実装を持てばそれが優先されるので、この「不明瞭」エラーは起きません。

<pre class="source" title="不明瞭エラーの回避">
<code><span class="reserved">class</span> <span class="type">C</span> : <span class="type">IB</span>, <span class="type">IC</span>
{
    <span class="comment">// これなら IB.M でも IC.M でもなく、この M が呼ばれるので明瞭</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;new implementation&quot;</span>);
}
</code></pre>

「どうしても `IB.M` を呼びたい」というように、特定の実装を明示的に呼び出したい場合もあるかと思います。
そういうときのために、[`base` キーワード](oo_inherit.md#base-access)に特定の型を指定できる機能も追加される予定<sup>※</sup>です。
`base(T)` というように書きます。

<pre class="source" title="">
<code><span class="reserved">class</span> <span class="type">C</span> : <span class="type">IB</span>, <span class="type">IC</span>
{
    <span class="comment">// これなら IB.M を明示的に呼べる。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="reserved">base</span>(<span class="type">IB</span>).<span class="method">M</span>();
}
</code></pre>

<sup>※</sup> 元々 C# 8.0 に入る予定で一時的には実装されていましたが、
最終的には 8.0 から外れて、9.0 で取り組みなおすことになりました。

ちなみに、将来的にはこの書き方も認めたいという計画はあります
(参考: 「[base(T) アクセス](oo_inherit.md#non-virtual-base-access)」)。

### <a id="sec-generated-title-29"></a>#<a id="reabstraction"></a>再抽象化

デフォルト実装を持つメンバーを、派生インターフェイス側で再び抽象メンバーに戻すこともできます。
以下のように、明示的実装っぽい書き方の前に `abstract` 修飾を付けます。

<pre class="source" title="再抽象化">
<code><span class="reserved">using</span> System;
 
<span class="reserved">interface</span> <span class="type">A</span>
{
    <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;default implementation&quot;</span>);
}
 
<span class="reserved">interface</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="comment">// 実装を持っているメソッドを abstract に変更。</span>
    <em><span class="reserved">abstract</span> <span class="reserved">void</span> <span class="type">A</span>.<span class="method">M</span>();</em>
}
 
<span class="comment">// M の実装が必須になる(コンパイル エラー)。</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">B</span>
{
}
</code></pre>

メソッド `M` が抽象メンバーになったので、インターフェイス`B`を実装するクラスには `M` の実装が必須になります。

この機能を再抽象化(re-abstraction)と言います。

#### <a id="sec-generated-title-30"></a> <a id="mics-restriction"></a>その他の制限

主に既存の(C# 7.3 以前の)コードを壊さないようにするためですが、
その他にもいくつか制限が掛かっています。
派生クラスと派生インターフェイスで挙動が変わったりもするので注意が必要です。

まず、派生インターフェイスでは、オーバーライドは常に[明示的実装](#explicit-impl)が必要です。

<pre class="source" title="オーバーライドには明示的実装が必須">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">M</span>() { }
}
 
<span class="reserved">interface</span> <span class="type">IDerived</span> : <span class="type">I</span>
{
    <span class="comment">// オーバーライドには明示的実装が必須。</span>
    <span class="reserved">void</span> <span class="type">I</span>.<span class="method">M</span>() { }
 
    <span class="comment">// 単に M と書くと、別物になる。</span>
    <span class="comment">// 「別物で基底の M を隠したければ new 修飾を付けろ」と警告が出る。</span>
    <span class="reserved">void</span> <span class="warning"><span class="method">M</span></span>() { }
}

<span class="reserved">class</span> <span class="type">C</span> : <span class="type">I</span>
{
    <span class="comment">// クラスの場合は別にそんな制限はなくて、public な同名のメソッドを書けば I.M として使える。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() { }
}
</code></pre>

基底インターフェイスのメンバーの呼び出しは、
派生側もインターフェイスの場合にはクラス → クラスの時と同じような感覚です。
普通に呼べるし、`proteted` なものに触れます。

一方、派生側がクラスの場合、デフォルト実装しかない(自分自身はオーバーライドしていない)時にはそのメンバーを直接呼べません。
また、`protected` なものには触れません。

<pre class="source" title="派生側での扱い">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">Abstract</span>();
    <span class="reserved">void</span> <span class="method">Default</span>() { }
 
    <span class="reserved">protected</span> <span class="reserved">void</span> <span class="method">Protected</span>() { }
}
 
<span class="reserved">interface</span> <span class="type">IDerived</span> : <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// クラス → クラスの派生と同じ感覚。</span>
        <span class="comment">// public, protected メソッドを呼べるし、デフォルト実装の有無も関係なく呼べる。</span>
        <span class="method">Abstract</span>();
        <span class="method">Default</span>();
        <span class="method">Protected</span>();
    }
}
 
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">I</span>
{
    <span class="comment">// デフォルト実装がないものは実装が必須</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Abstract</span>() { }
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// これは、自身も実装を持っているので呼べる。</span>
        <span class="method">Abstract</span>();
 
        <span class="comment">// これはコンパイル エラーになる。</span>
        <span class="error">Default</span>();
 
        <span class="comment">// 呼びたければ1段キャストが必要。</span>
        ((<span class="type">I</span>)<span class="reserved">this</span>).<span class="method">Default</span>();
 
        <span class="comment">// protected なものは呼べない。コンパイル エラーに。</span>
        ((<span class="type">I</span>)<span class="reserved">this</span>).<span class="error">Protected</span>();
    }
}
</code></pre>


<!-- original-page-break -->

## <a id="sec-generated-title-31"></a> <a id="static-abstract"></a>インターフェイスの静的抽象メンバー

<h5 class="version version11">Ver. 11.0</h5>

C# 11 (.NET 7) で、インターフェイスの静的メンバーを abstract/virtual にできるようになりました。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Buffers<span class="operator">.</span>Text;
<span class="reserved">using</span> System<span class="operator">.</span>Text;

<span class="reserved">interface</span> <span class="type">IUtf8Parsable</span><<span class="type param">T</span>>
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IUtf8Parsable</span><<span class="type param">T</span>>
{
    <span class="comment">// 静的メンバーにしたいもの筆頭が、ファクトリメソッドの類。</span>
    <span class="comment">// この例では Parse (文字列から T のインスタンスを作る)にしているものの、</span>
    <span class="comment">// 例えば static T Create(); みたいなものの需要も結構高いはず。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="type param">T</span> <span class="static"><span class="method">Parse</span></span>(<span class="type struct">ReadOnlySpan</span><<span class="reserved">byte</span>> <span class="variable local">utf8</span>);

    <span class="comment">// virtual にもできる。</span>
    <span class="comment">// デフォルト実装を持ちつつ、必要であればクラス側で別実装を書ける。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">T</span> <span class="static"><span class="method">Parse</span></span>(<span class="reserved">string</span> <span class="variable local">s</span>)
    {
        <span class="reserved">var</span> <span class="variable">buffer</span> <span class="operator">=</span> (<span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="variable local">s<span class="operator"></span>.</span><span class="property">Length</span>]);
        <span class="reserved">var</span> <span class="variable">read</span> <span class="operator">=</span> <span class="type">Encoding</span><span class="operator">.<span class="static"><span class="property"></span>ASCII</span><span class="operator"></span>.</span><span class="method">GetBytes</span>(<span class="variable local">s</span>, <span class="variable">buffer</span>);
        <span class="control">return</span> <span class="type param">T<span class="operator"></span>.</span><span class="static"><span class="method">Parse</span></span>(<span class="variable">buffer</span>[..<span class="variable">read</span>]);
    }
}

<span class="comment">// 実装例:</span>
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">Point</span>(<span class="reserved">int</span> <span class="variable local">X</span>, <span class="reserved">int</span> <span class="variable local">Y</span>) : <span class="type">IUtf8Parsable</span><<span class="type struct">Point</span>>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Point</span> <span class="method"><span class="static">Parse</span></span>(<span class="type struct">ReadOnlySpan</span><<span class="reserved">byte</span>> <span class="variable local">utf8</span>)
    {
        <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="variable local">utf8</span><span class="operator">.</span><span class="method">IndexOf</span>((<span class="reserved">byte</span>)<span class="string">','</span>);
        <span class="reserved">var</span> <span class="variable">xs</span> <span class="operator">=</span> <span class="variable local">utf8</span>[..<span class="variable">i</span>];
        <span class="reserved">var</span> <span class="variable">ys</span> <span class="operator">=</span> <span class="variable local">utf8</span>[(<span class="variable">i</span> <span class="operator">+</span> <span class="number">1</span>)..];

        <span class="type"><span class="static">Utf8Parser</span></span><span class="operator">.</span><span class="method"><span class="static">TryParse</span></span>(<span class="variable">xs</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">_</span>);
        <span class="type"><span class="static">Utf8Parser</span></span><span class="operator">.</span><span class="method"><span class="static">TryParse</span></span>(<span class="variable">ys</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>, <span class="reserved">out</span> <span class="reserved">_</span>);

        <span class="control">return</span> <span class="reserved">new</span>(<span class="variable">x</span>, <span class="variable">y</span>);
    }
}
</pre>

[C# 8 のときのデフォルト実装](#dim)と同じく、ランタイム側の修正が必要な機能で、C# バージョンだけを 11 に上げても、古い .NET をターゲットにしていると利用できません。

### <a id="sec-generated-title-32"></a> <a id="static-abstract-declaration">静的抽象メンバーの宣言</a>

文法的には割かし素直で、 `abstract`/`virtual` と `static` を併用できるようになりました。

<pre class="source" title="">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <em><span class="reserved">static</span> <span class="reserved">abstract</span></em> <span class="reserved">void</span> <span class="method"><span class="static">StaticAbstract</span></span>();
    <em><span class="reserved">static</span> <span class="reserved">virtual</span></em> <span class="reserved">void</span> <span class="method"><span class="static">StaticVirtual</span></span>() { }
}
</pre>

このまま「`abstract`/`virtual` と `static` を同時に指定できるようになっただけです」と簡単に済ませられればいいんですが、C# 11 にもなって後付けしている経緯から、
ちょっと他の文法との整合性が悪かったりします。

以下のように、インスタンス メンバーと静的メンバーで、何も修飾子を付けないときの挙動が異なります。

<pre class="source" title="インスタンス メンバーと静的メンバーの挙動の違い">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="comment">// インスタンス メンバーの場合、abstract 修飾を付けなくても元から abstract。</span>
    <span class="reserved">void</span> <span class="method">Instance</span>();

    <span class="comment">// C# 8</span>
    <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method">InstanceAbstract</span>();
    <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">InstanceVirtual</span>() { }

    <span class="comment">// C# 8</span>
    <span class="comment">// 静的メンバーの場合、何も修飾しないときは non-virtual。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>() { }

    <span class="comment">// C# 11</span>
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="static"><span class="method">StaticAbstract</span></span>();
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticVirtual</span></span>() { }
}
</pre>

ちなみに、この C# 8 の頃からの「何も付けないと non-virtual」の仕様があるのでわざわざ付ける意味はないんですが、一応、`sealed` 修飾子を付けれるようになっています。

<pre class="source" title="sealed の明示も OK">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="comment">// 何もつけない = non-virtual。</span>
    <span class="reserved">void</span> <span class="method">Static</span>() { }

    <span class="comment">// わざわざつける意味はない(元から sealed)けども、一応、明示的に sealed を付けることは認められてる。</span>
    <span class="reserved">sealed</span> <span class="reserved">void</span> <span class="method">StaticSealed</span>() { }
}
</pre>

### <a id="sec-generated-title-33"></a> <a id="static-abstract-implementation">静的抽象メンバーの実装</a>

インターフェイスの静的メンバーの実装方法はインスタンス メンバーの場合とそれほど変わりません。
以下のように、`public` で同名のメソッドを定義する(暗黙的実装)か、
`インターフェイス名.` で実装する(明示的実装)かです。

<pre class="source" title="静的メンバーの実装例">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method">Instance</span>();
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>();
}

<span class="reserved">class</span> <span class="type">Implicit</span> : <span class="type">IA</span>
{
    <span class="comment">// 暗黙的実装。</span>
    <span class="comment">// public にする必要あり。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Instance</span>() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>() { }
}

<span class="reserved">class</span> <span class="type">Explicit</span> : <span class="type">IA</span>
{
    <span class="comment">// 明示的実装。</span>
    <span class="comment">// アクセシビリティは書けない(public と付けちゃダメ)。</span>
    <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.</span><span class="method">Instance</span>() { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.</span><span class="method"><span class="static">Static</span></span>() { }
}
</pre>

ただ、静的メンバーを `virtual` / `abstract` にできるのはインターフェイスだけなので、
この点はインスタンス メンバーと同じというわけにはいきません。
以下のようなコードはエラーになります。

<pre class="source" title="クラスでは static virtual とは書けない">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method">Instance</span>();
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>();
}

<span class="reserved">class</span> <span class="type">Virtual</span> : <span class="type">IA</span>
{
    <span class="comment">// これは書ける(元々)。</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">Instance</span>() { }

    <span class="comment">// こうは書けない。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method"><span class="static"><span class="error" title="CS0112">Static</span></span></span>() { }
}
</pre>


### <a id="sec-generated-title-34"></a> <a id="static-abstract-invocation">静的抽象メンバーの呼び出し</a>

インターフェイスの静的抽象メンバーは、[ジェネリック型引数](sp2_generics.md#typeparam)越しにしか呼べません。

例えば前節で例に挙げた `IA` インターフェイスの場合、以下のような呼び出し方になります。

<pre class="source" title="">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;()
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IA</span>
{
    <span class="comment">// non-virtual の場合、インターフェイス名. 開始。</span>
    <span class="comment">// T.Static(); とは書けない。</span>
    <span class="type">IA</span><span class="operator">.</span><span class="method"><span class="static">Static</span></span>();

    <span class="comment">// virtual/abstract の場合、型引数. 開始。</span>
    <span class="comment">// IA.StaticAbstract(); IA.StaticVirtual(); とは書けない。</span>
    <span class="type param">T</span><span class="operator">.<span class="static"></span><span class="static"><span class="method">StaticAbstract</span></span></span>();
    <span class="type param">T</span><span class="operator">.</span><span class="method"><span class="static">StaticVirtual</span></span>();
}

<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="comment">// non-virtual。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>() { }

    <span class="comment">// virtual/abstract</span>
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticAbstract</span></span>();
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticVirtual</span></span>() { }
}</pre>

#### <a id="sec-generated-title-35"></a> <a id="type-class">注意: 静的抽象メンバー呼び出しは静的な型に紐づく</a>

インスタンス メンバーと違って、
静的抽象メンバーの呼び出しは静的な型に紐づきます。

以下のように、`M<T>()` 内で `T.Static()` と呼び出したとき、
メソッド `M` を `M<A>()` で呼び出した場合に常に `A.Static` が呼ばれます。

<pre class="source" title="静的な型に紐づいてメソッドが呼ばれる例">
<span class="comment">// 静的な型(変数/引数の型)とインスタンスの型(変数に格納した値の型)が一致してるときはそんなに変な挙動はしない。</span>

<span class="method"><span class="static">M</span></span>(<span class="reserved">new</span> <span class="type">ABase</span>()); <span class="comment">// Base Instance / Base Static</span>
<span class="static"><span class="method">M</span></span>(<span class="reserved">new</span> <span class="type">ADerived</span>()); <span class="comment">// Derived Instance / Derived Static</span>

<span class="comment">// 問題は、それが違うとき。</span>

<span class="type">ABase</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">ADerived</span>();
<span class="static"><span class="method">M</span></span>(<span class="variable">a</span>); <span class="comment">// Derived Instance / Base Static</span>

<span class="static"><span class="method">M</span></span>&lt;<span class="type">ABase</span>&gt;(<span class="reserved">new</span> <span class="type">ADerived</span>()); <span class="comment">// Derived Instance / Base Static</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">x</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IA</span>
{
    <span class="variable local">x</span><span class="operator">.</span><span class="method">Instance</span>();
    <span class="type param">T</span><span class="operator">.</span><span class="method"><span class="static">Static</span></span>();
}
 
<span class="comment">// static abstract (実装を持っていない)メンバーがあるとと M&lt;IA&gt;() と書けなくなる。</span>
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method">Instance</span>();
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method"><span class="static">Static</span></span>();
}

<span class="reserved">class</span> <span class="type">ABase</span> : <span class="type">IA</span>
{
    <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.</span><span class="method">Instance</span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Base Instance&quot;</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.</span><span class="static"><span class="method">Static</span></span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Base Static&quot;</span>);
}

<span class="reserved">class</span> <span class="type">ADerived</span> : <span class="type">ABase</span>, <span class="type">IA</span>
{
    <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.<span class="method"></span>Instance</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Derived Instance&quot;</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="type">IA</span><span class="operator">.<span class="static"></span><span class="method">Static</span></span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Derived Static&quot;</span>);
}
</pre>

これまでのインターフェイスの「インスタンスの型に紐づいて動的な呼び出しが行われる」という感覚とずれるので注意が必要です。

このことを指して、他のプログラミングの機能名と照らし合わせて、
「インターフェイスの静的抽象メンバーは、インターフェイスというよりも型クラス(type class)だ」と説明する人もいるくらいです。

#### <a id="sec-generated-title-36"></a> <a id="type-argument">注意: 静的抽象メンバーを持っていると型実引数に渡せない</a>

前節で説明したように、静的な型に紐づく以上、
`abstract` な(実装を持っていない)型を型引数にすることはできません。

以下のように、`virtual` (実装を持っている)であれば問題ありません。

<pre class="source" title="virtual なら実装を持っているので困らない">
<span class="method"><span class="static">M</span></span>&lt;<span class="type">IA</span>&gt;();

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;()
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IA</span>
    <span class="operator">=&gt;</span> <span class="type param">T</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>();

<span class="comment">// static abstract (実装を持っていない)メンバーがいないときは、M&lt;IA&gt;() と書ける。</span>
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;IA.M&quot;</span>);
}
</pre>

一方で、以下のように `abstract` (実装を持っていない)だとコンパイル エラーになります。

<pre class="source" title="abstract は実装を持っていないので呼べない">
<span class="method"><span class="static"><span class="error" title="CS8920">M</span></span>&lt;<span class="type">IA</span>&gt;</span>(); <span class="comment">// ここでエラーに。</span>

<span class="method"><span class="static">M</span></span>&lt;<span class="type">A</span>&gt;(); <span class="comment">// これ(実装クラス)ならOK。</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;()
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IA</span>
    <span class="operator">=&gt;</span> <span class="type param">T</span><span class="operator">.<span class="static"><span class="method"></span>M</span></span>();

<span class="comment">// static abstract (実装を持っていない)メンバーがあると M&lt;IA&gt;() と書けなくなる。</span>
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>();
}

<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IA</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>() { }
}
</pre>

### <a id="sec-generated-title-37"></a> <a id="interface-operator">演算子</a>

静的メンバーを `virtual` / `abstract` にできて一番うれしいのは、
演算子を定義できることでしょう。

例えばこれまで、以下のようなメソッドすらジェネリックな実装を持てませんでした。

<pre class="source" title="+ 演算子の例">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="method">Sum</span></span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> }));

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="method">Sum</span></span>(<span class="error" title="CS1503"><span class="reserved">new</span> <span class="reserved">float</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> }</span>)); <span class="comment">// こう書きたいのにエラーに…</span>

<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Sum</span></span>(<span class="reserved">int</span>[] <span class="variable local">items</span>) <span class="comment">// Sum&lt;T&gt;(T[]) にしてしまうと += が書けない。</span>
{
    <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="number">0</span>;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">items</span>) <span class="variable">sum</span> <span class="operator">+=</span> <span class="variable">x</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}
</pre>

C# 11 でインターフェイスに `virtual` / `abstract` な演算子を持てるようになったことに伴って、
.NET 7 で標準ライブラリに以下のようなインターフェイスが用意されました。

<pre class="source" title="+ 演算子を持つインターフェイスが標準ライブラリ入り">
<span class="reserved">namespace</span> System<span class="operator">.</span>Numerics;

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IAdditionOperators</span>&lt;<span class="type param">TSelf</span>, <span class="type param">TOther</span>, <span class="type param">TResult</span>&gt;
    <span class="reserved">where</span> <span class="type param">TSelf</span> : <span class="type">IAdditionOperators&lt;<span class="type param">TSelf</span>, <span class="type param">TOther</span>, <span class="type param">TResult</span>&gt;</span><span class="operator">?</span>
{
    <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="type param">TResult</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TOther</span> <span class="variable local">right</span>);
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">TResult</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">+</span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TOther</span> <span class="variable local">right</span>) <span class="operator">=&gt;</span> <span class="variable local">left</span> + <span class="variable local">right</span>;
}
</pre>

`int` や `float` などの組み込みの数値型は一通りこのインターフェイスを実装しています。
(さらにいうと、この手のインターフェイスをまとめた `INumeber<T>` というインターフェイスを実装しています。)
その結果、本節冒頭で挙げたような `Sum` メソッドをジェネリックに書けるようになりました。

<pre class="source" title="ジェネリックな Sum メソッド">
<span class="reserved">using</span> System<span class="operator">.</span>Numerics;

<span class="comment">// よくある「和を取るコード」なものですら、これまでだとジェネリックに書く手段がなかった。</span>
<span class="comment">// C# 11 で可能に。</span>
<span class="reserved">static</span> <span class="type param">T</span> <span class="static"><span class="method">Sum</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">items</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">INumber</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="type param">T</span><span class="operator">.</span><span class="property"><span class="static">Zero</span></span>;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">items</span>) <span class="variable">sum</span> += <span class="variable">x</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}

<span class="comment">// いろんな型に対して sum&lt;T&gt; を呼ぶ。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="method">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">float</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="static"><span class="type">Console<span class="operator"></span></span>.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">double</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method"><span class="static">Sum</span></span>(<span class="reserved">new</span> <span class="reserved">decimal</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }));
</pre>

#### <a id="sec-generated-title-38"></a> <a id="generic-math">Generic Math</a>

加減乗除や論理演算はもちろん、`float`, `double` などの一部の型は `Math.Sin` などの数学関数も使えます。
コンセプト的に、この新機能を使ったジェネリックな数値処理の事を通称 Generic Math と呼んでいたりします。

また、 .NET 5 以降、数値関連の型がいくつか追加されています。

* [`Half`](https://docs.microsoft.com/ja-jp/dotnet/api/system.half?WT.mc_id=DT-MVP-4028921): 16ビット浮動小数点数
* [`Int128`, `UInt128`](https://github.com/dotnet/runtime/issues/67151): 128ビットの整数
* [`CLong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.clong?WT.mc_id=DT-MVP-4028921), [`CULong`](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.interopservices.culong?WT.mc_id=DT-MVP-4028921): C/C++ との相互運用のために使う、環境によってビット幅が違う整数
* [`nint`, `nuint`](../cheatsheet/ap_ver9.md#nint): CPU 依存幅の整数

これらの新しい数値型も、Generic Math の対象で、`INumber<T>` などのインターフェイスを実装しています。
## <a id="exercise"></a>演習問題

### <a id="exercise-if1"></a>問題 1


[多態性](oo_polymorphism.md)の[問題 1](oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスをインターフェース化せよ。

<code>Triangle</code> や <code>Shape</code> 関係の例題は一応、これで完成形。

余力があれば、楕円、長方形、平行四辺形、（任意の頂点の）多角形等、さまざまな図形クラスを作成せよ。
また、これらの図形の面積と周の比を計算するプログラムを作成せよ。


#### 解答例 1


三角形、円に加え、多角形を実装した物を示します。

<pre class="source" title="さまざまな図形" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion
  #region</span> 演算子

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル和
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x + b.x, a.y + b.y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル差
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x - b.x, a.y - b.y);
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">interface</span> Shape
{
  <span class="reserved">double</span> GetArea();
  <span class="reserved">double</span> GetPerimeter();
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
{
  Point center;
  <span class="reserved">double</span> radius;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 半径を指定して初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="r"&gt;半径。&lt;/param&gt;</span>
  <span class="reserved">public</span> Circle(Point center, <span class="reserved">double</span> r)
  {
    <span class="reserved">this</span>.center = center;
    <span class="reserved">this</span>.radius = r;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 円の中心。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point Center
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.center; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.center = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の半径。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Radius
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.radius; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.radius = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 円の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">return</span> Math.PI * <span class="reserved">this</span>.radius * <span class="reserved">this</span>.radius;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">return</span> 2 * Math.PI * <span class="reserved">this</span>.radius;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (c = {0}, r = {1})"</span>,
      <span class="reserved">this</span>.center, <span class="reserved">this</span>.radius);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    <span class="reserved">return</span> 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (a = {0}, b = {1}, c = {2})"</span>,
      <span class="reserved">this</span>.a, <span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 自由多角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Polygon : Shape
{
  Point[] verteces; <span class="comment">// 頂点</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="verteces"&gt;頂点の座標の入った配列&lt;/param&gt;</span>
  <span class="reserved">public</span> Polygon(<span class="reserved">params</span> Point[] verteces)
  {
    <span class="reserved">this</span>.verteces = verteces;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点の集合。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point[] Verteces
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.verteces; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.verteces = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> area = 0;
    Point p = <span class="reserved">this</span>.verteces[<span class="reserved">this</span>.verteces.Length - 1];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      Point q = <span class="reserved">this</span>.verteces[i];
      area += p.X * q.Y - q.X * p.Y;
      p = q;
    }
    <span class="reserved">return</span> 0.5 * Math.Abs(area);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetPerimeter()
  {
    <span class="reserved">double</span> perimeter = 0;
    Point p = <span class="reserved">this</span>.verteces[<span class="reserved">this</span>.verteces.Length - 1];
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      Point q = <span class="reserved">this</span>.verteces[i];
      perimeter += Point.GetDistance(p, q);
      p = q;
    }
    <span class="reserved">return</span> perimeter;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    System.Text.StringBuilder sb = <span class="reserved">new</span> System.Text.StringBuilder();
    sb.AppendFormat(<span class="literal">"Polygon ({0}"</span>, <span class="reserved">this</span>.verteces[0]);
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt; <span class="reserved">this</span>.verteces.Length; ++i)
    {
      sb.AppendFormat(<span class="literal">", {0}"</span>, <span class="reserved">this</span>.verteces[i]);
    }
    sb.Append(<span class="literal">")"</span>);

    <span class="reserved">return</span> sb.ToString();
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Circle c = <span class="reserved">new</span> Circle(
      <span class="reserved">new</span> Point(0, 0), 3);

    Polygon p1 = <span class="reserved">new</span> Polygon(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Polygon p2 = <span class="reserved">new</span> Polygon(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(0, 2),
      <span class="reserved">new</span> Point(2, 2),
      <span class="reserved">new</span> Point(2, 0));

    Show(t);
    Show(c);
    Show(p1);
    Show(p2);
  }

  <span class="reserved">static void</span> Show(Shape f)
  {
    Console.Write(<span class="literal">"図形 {0}\n"</span>, f);
    Console.Write(<span class="literal">"面積/周 = {0}\n"</span>, f.GetArea() / f.GetPerimeter());
  }
}
</code></pre>
