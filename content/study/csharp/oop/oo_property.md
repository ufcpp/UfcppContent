---
title: "プロパティ"
source_url: "https://ufcpp.net/study/csharp/oop/oo_property/"
content_type: "Article"
published_at: "2015-05-06T14:09:28"
updated_at: "2022-09-22T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 3.0"
  - "Ver. 6.0"
umbraco_id: 1255
parent_id: 1248
sort_order: 4
aliases:
  - "/csharp/oo_property"
  - "/csharp/oo_property.html"
  - "/csharp/oop/oo_property/"
  - "/study/csharp/oo_property"
  - "/study/csharp/oo_property.html"
---

# プロパティ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="property" class="keyword">プロパティ</strong>（property：所有物、特性）とは、JavaやC++にはない(Visual Basicにはある)機能で、
クラス外部から見るとメンバー変数のように振る舞い、
クラス内部から見るとメソッドのように振舞うものです。

JavaやC++がこの機能を持ってないことからも分かると思いますが、
プロパティはオブジェクト指向言語に必須の機能ではありません。
しかし、これから説明していくように、あると便利なものです。


##### <a id="sec-generated-title-2"></a>ポイント

* プロパティ: 中（実装側）からはメソッドのように扱え、外（利用側）からはメンバー変数のように見えるもの。

* 実装の隠蔽（カプセル化）の原則を崩すことなく、 アクセサー関数の煩雑さを解消。



## <a id="sec-generated-title-3"></a> <a id="about"></a>プロパティとは

「[実装の隠蔽](oo_conceal.md)」で、
メンバー変数はクラス外部から直接アクセス出来ないようにして、
オブジェクトの状態の変更はすべてメソッドを通して行うべきだと書きました。
これを忠実に実行すると、クラスを利用する側のコードは以下の例のように少々見栄えの悪いものになってしまいます。

<pre class="source" title="「実装の隠蔽」で作った複素数クラスその2の利用例" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// 「実装の隠蔽」で作った複素数クラス</span>
<span class="reserved">class</span> Complex
{
  <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
  <span class="reserved">private double</span> re; <span class="comment">// 実部を記憶しておく</span>
  <span class="reserved">private double</span> im; <span class="comment">// 虚部を記憶しておく</span>

  <span class="reserved">public double</span> Re(){<span class="reserved">return this</span>.re;}    <span class="comment">// 実部を取り出す</span>
  <span class="reserved">public void</span> Re(<span class="reserved">double</span> x){<span class="reserved">this</span>.re = x;} <span class="comment">// 実部を書き換え</span>

  <span class="reserved">public double</span> Im(){<span class="reserved">return this</span>.im;}    <span class="comment">// 虚部を取り出す</span>
  <span class="reserved">public void</span> Im(<span class="reserved">double</span> y){<span class="reserved">this</span>.im = y;} <span class="comment">// 虚部を書き換え</span>

  <span class="reserved">public double</span> Abs(){<span class="reserved">return</span> Math.Sqrt(re*re + im*im);}  <span class="comment">// 絶対値を取り出す</span>
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> ConcealSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// x = 5 + 1i</span>
    Complex x = <span class="reserved">new</span> Complex();
    x.Re(5);  <span class="comment">// x.re = 5</span>
    x.Im(1);  <span class="comment">// x.im = 1

    // y = -2 + 3i</span>
    Complex y = <span class="reserved">new</span> Complex();
    y.Re(-2); <span class="comment">// y.re = -2</span>
    y.Im( 3); <span class="comment">// y.im =  3</span>

    Complex z = <span class="reserved">new</span> Complex();
    z.Re(x.Re() + y.Re()); <span class="comment">// z.re = x.re + y.re</span>
    z.Im(x.Im() + y.Im()); <span class="comment">// z.im = x.im + y.im</span>

    Console.Write(<span class="literal">"|{0} + {1}i| = {2}\n"</span>, z.Re(), z.Im(), z.Abs());
    <span class="comment">// |3 + 4i| = 5 と表示される</span>
  }
}
</code></pre>


<code>void Re(double x)</code>、<code>double Re()</code>などの、
メンバー変数の値の取得・変更を行うためのメソッドのことを<strong id="accessor" class="keyword">アクセサー</strong>(accessor)といいます。
C++やJavaなどの言語では、下手をすると<em>メンバー変数の数だけアクセサーが存在する</em>という状態になることもあります。
C++やJavaではアクセサーのメソッド名は<code>void SetRe(double x)</code>、<code>double GetRe()</code>というように、メンバー変数名に Set/Get をつけた物を使うことが多く、<em>メンバ変数の数だけ Set/Get で始まるメソッドのペアができ、ちょっと見苦しいものになります</em>。
（参考： 「[Set / Get とプロパティ](../../miscprog/list/accessor.md)」）

また、クラス作成側からすると、オブジェクトの状態の取得・変更はすべてメソッドを通して行ったほうがいいのですが、
クラス利用側からすると、メンバー変数に値を直接代入するほうが見た目がすっきりします。

このような理由から、
C#では
<em>
        クラス内部から見るとメソッドのように振る舞い、
        クラス利用側から見るとメンバー変数のように振舞う
      </em>
プロパティという機能を用意しました。
プロパティの定義の仕方は以下のような書式になります。

<pre class="source" title="" lang="">
<code><span class="input">アクセスレベル</span> <span class="input">型名</span> <span class="input">プロパティ名</span>
{
    <span class="reserved">set</span>
    {
        <span class="comment">// setアクセサー（setter とも言う）
        //  ここに値の変更時の処理を書く。
        //  value という名前の変数に代入された値が格納される。</span>
    }
    <span class="reserved">get</span>
    {
        <span class="comment">// getアクセサー （getter とも言う）
        //  ここに値の取得時の処理を書く。
        //  メソッドの場合と同様に、値はreturnキーワードを用いて返す。</span>
    }
}
</code></pre>


set 以降のブロックに値の変更用の処理を、
get 以降のに値の取得用の処理を書きます。
これらを、set アクセサー、get アクセサーと呼びます。
あるいは、通称では <strong id="setter" class="keyword">setter</strong>、<strong id="getter" class="keyword">getter</strong> と呼んだりします。

例えば先ほどの複素数クラスのアクセサーをプロパティを使って書き換えると以下のようになります。

<pre class="source" title="複素数クラス その3" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
    <span class="reserved">private double</span> re; <span class="comment">// 実部を記憶しておく</span>
    <span class="reserved">private double</span> im; <span class="comment">// 虚部を記憶しておく

    // 実部の取得・変更用のプロパティ</span>
<em>    <span class="reserved">public double</span> Re</em>
    {
        <span class="reserved">set</span> { <span class="reserved">this</span>.re = <span class="reserved">value</span>; }
        <span class="reserved">get</span> { <span class="reserved">return this</span>.re; }
    }
    <span class="comment">/* ↑のコードは意味的には以下のコードと同じ。
    public void SetRe(double value){this.re = value;}
    public double GetRe(){return this.re;}
    メソッドと同じ感覚で使える。
    */

    // 実部の取得・変更用のプロパティ</span>
<em>    <span class="reserved">public double</span> Im</em>
    {
        <span class="reserved">set</span> { <span class="reserved">this</span>.im = <span class="reserved">value</span>; }
        <span class="reserved">get</span> { <span class="reserved">return this</span>.im; }
    }

    <span class="comment">// 絶対値の取得用のプロパティ</span>
    <span class="reserved">public double</span> Abs
    {
        <span class="comment">// 読み取り専用プロパティ。
        // setブロックを書かない。</span>
        <span class="reserved">get</span> { <span class="reserved">return</span> <span class="type">Math</span>.Sqrt(re * re + im * im); }
    }
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> <span class="type">PropertySample</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="type">Complex</span> c = <span class="reserved">new</span> <span class="type">Complex</span>();
        c.Re = 4; <span class="comment">// Reプロパティのsetアクセサーが呼び出される。</span>
        c.Im = 3; <span class="comment">// Imプロパティのsetアクセサーが呼び出される。</span>
        <span class="type">Console</span>.Write(<span class="literal">"|{0} + "</span>, c.Re); <span class="comment">// Reプロパティのgetアクセサーが呼び出される。</span>
        <span class="type">Console</span>.Write(<span class="literal">"{0}i| ="</span>, c.Im); <span class="comment">// Imプロパティのgetアクセサーが呼び出される。</span>
        <span class="type">Console</span>.Write(<span class="literal">" {0}\n"</span>, c.Abs); <span class="comment">// Absプロパティのgetアクセサーが呼び出される。</span>
    }
}
</code></pre>


「[実装の隠蔽](oo_conceal.md)」のときと同様に、
このコードの実装方法を
「実部と虚部をメンバー変数に記憶しておく」方法から
「絶対値と偏角をメンバー変数に記憶しておく」方法に変更しても、
以下のように、クラス利用側のコードに手を加える必要は一切ありません。

<pre class="source" title="複素数クラスその3の実装を変更" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
    <span class="reserved">private double</span> abs; <span class="comment">// 絶対値を記憶しておく</span>
    <span class="reserved">private double</span> arg; <span class="comment">// 偏角を記憶しておく

    // 実部の取得・変更用のプロパティ</span>
    <span class="reserved">public double</span> Re
    {
        <span class="reserved">set</span>
        {
            <span class="reserved">double</span> im = <span class="reserved">this</span>.abs * <span class="type">Math</span>.Sin(<span class="reserved">this</span>.arg);
            <span class="reserved">this</span>.abs = <span class="type">Math</span>.Sqrt(<span class="reserved">value</span> * <span class="reserved">value</span> + im * im);
            <span class="reserved">this</span>.arg = <span class="type">Math</span>.Atan2(im, <span class="reserved">value</span>);
        }
        <span class="reserved">get</span>
        {
            <span class="reserved">return this</span>.abs * <span class="type">Math</span>.Cos(<span class="reserved">this</span>.arg);
        }
    }

    <span class="comment">// 実部の取得・変更用のプロパティ</span>
    <span class="reserved">public double</span> Im
    {
        <span class="reserved">set</span>
        {
            <span class="reserved">double</span> re = <span class="reserved">this</span>.abs * <span class="type">Math</span>.Cos(<span class="reserved">this</span>.arg);
            <span class="reserved">this</span>.abs = <span class="type">Math</span>.Sqrt(<span class="reserved">value</span> * <span class="reserved">value</span> + re * re);
            <span class="reserved">this</span>.arg = <span class="type">Math</span>.Atan2(<span class="reserved">value</span>, re);
        }
        <span class="reserved">get</span>
        {
            <span class="reserved">return this</span>.abs * <span class="type">Math</span>.Sin(<span class="reserved">this</span>.arg);
        }
    }

    <span class="comment">// 絶対値の取得用のプロパティ</span>
    <span class="reserved">public double</span> Abs
    {
        <span class="reserved">get</span> { <span class="reserved">return this</span>.abs; }
    }
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> <span class="type">PropertySample</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="type">Complex</span> c = <span class="reserved">new</span> <span class="type">Complex</span>();
<em>        c.Re = 4; <span class="comment">// クラス利用側は一切変更せず</span>
        c.Im = 3;</em>
        <span class="type">Console</span>.Write(<span class="literal">"|{0} + "</span>, c.Re);
        <span class="type">Console</span>.Write(<span class="literal">"{0}i| ="</span>, c.Im);
        <span class="type">Console</span>.Write(<span class="literal">" {0}\n"</span>, c.Abs);
    }
}
</code></pre>



## <a id="sec-generated-title-4"></a> <a id="level"></a>set/get で異なるアクセスレベルを設定

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 の新機能で、
プロパティの set/get アクセサーそれぞれ異なるアクセスレベルを設定できるようになりました。

<pre class="source" title="異なるアクセスレベル" lang="">
<code><span class="reserved">class</span> A
{
  <span class="reserved">private int</span> n;

  <span class="reserved">public int</span> N
  {
    <span class="reserved">get</span>{ <span class="reserved">return this</span>.n; }
    <span class="reserved"><em>protected</em> set</span>{ <span class="reserved">this</span>.n = value; }
  }
}
</code></pre>



## <a id="sec-generated-title-5"></a> <a id="auto"></a>自動プロパティ

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、プロパティの get/set の中身の省略もできるようになりました。
この機能を<strong id="auto_prop" class="keyword">自動プロパティ</strong>（auto-property, auto-implemented property）といいます。

例えば、

<pre class="source" title="プロパティの set/get の省略" lang="">
<code><span class="reserved">public string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
</code></pre>


というように、
<code>get; set;</code> とだけ書いておくと、

<pre class="source" title="set/get の自動生成結果" lang="">
<code><span class="reserved">private string</span> __name;
<span class="reserved">public string</span> Name
{
  <span class="reserved">get</span> { <span class="reserved">return this</span>.__name; }
  <span class="reserved">set</span> { <span class="reserved">this</span>.__name = value; }
}
</code></pre>


というようなコードに相当するものが自動的に生成されます。
（説明のため <code>__name</code> という名前で書いていますが、
実際のコンパイル結果はプログラマが参照できない記号入りの名前で生成されます。）
ちなみに、このコンパイラーによって生成されるフィールド(この例で言うと __name)は、バッキング フィールド(baking field: 裏打ち、裏付け、後援みたいな意味)と呼ばれます。

C# プログラミングでは、
この手のコード（メンバー変数 name をプロパティ Name で覆う）は定型文的によく使います。
また、クラス内からであっても、private のメンバー変数には直接アクセスせず、
プロパティを通してアクセスする方が後々の保守がしやすかったりします。
ということで、自動プロパティのような省略記法が導入されました。

複素数の例でも、直交座標による実装のものは、以下のようにだいぶシンプルに書けるようになります。

<pre class="source" title="自動プロパティを使った複素数クラス定義" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public double</span> Re { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public double</span> Im { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public double</span> Abs
    {
        <span class="reserved">get</span> { <span class="reserved">return</span> <span class="type">Math</span>.Sqrt(Re * Re + Im * Im); }
    }
}
</code></pre>


ちなみに、元々 C# 2.0 以前でも、
「プロパティの「[デリゲート](../functional/sp_delegate.md#delegate)」版」にあたる「[イベント](../functional/sp_event.md#event)」では自動プロパティを同じような省略が可能でした。
（デリゲート、イベントについては後述。
参考： 「[デリゲート](../functional/sp_delegate.md)」、「[イベント](../functional/sp_event.md)」。）
その省略機能がプロパティにも実装されたということになります。


## <a id="sec-generated-title-6"></a> <a id="get-only"></a>get-only プロパティ

<h5 class="version version6">Ver. 6</h5>

C# 6 では、get アクセサーだけのプロパティを定義できるようになりました。
この場合、コンストラクターでだけ値を代入できて、以降は書き換え不能になります。

<pre class="source" title="get-only なプロパティ" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public double</span> Re { <em><span class="reserved">get</span>;</em> }
    <span class="reserved">public double</span> Im { <em><span class="reserved">get</span>;</em> }

    <span class="reserved">public</span> Complex(<span class="reserved">double</span> re, <span class="reserved">double</span> im)
    {
        <span class="comment">// コンストラクター内でだけ代入可能。</span>
        Re = re;
        Im = im;
    }
}
</code></pre>

このように `get` アクセサーのみを持つプロパティは通称 <strong id="key-get-only" class="keyword">get-only プロパティ</strong>(get-only property)と呼ばれています。

「コンストラクターでだけ値を代入できる」という挙動は [readonly フィールド](../start/sp_const.md#readonly)と同じです。
実際、上記の get-only プロパティからは以下のように、readonly なバッキング フィールドが作られます。

<pre class="source" title="get-only なプロパティから生成されるコード" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public double</span> Re { <span class="reserved">get</span> { <span class="reserved">return</span> _re; } }
    <span class="reserved">private readonly double</span> _re;
    <span class="reserved">public double</span> Im { <span class="reserved">get</span> { <span class="reserved">return</span> _im; } }
    <span class="reserved">private readonly double</span> _im;

    <span class="reserved">public</span> Complex(<span class="reserved">double</span> re, <span class="reserved">double</span> im)
    {
        <span class="comment">// コンストラクター内でだけ代入可能。</span>
        _re = re;
        _im = im;
    }
}
</code></pre>

## <a id="sec-generated-title-7"></a> <a id="property-initializer"></a>プロパティ初期化子

<h5 class="version version6">Ver. 6</h5>

同じくC# 6.0から、自動プロパティに対して初期化子を与えられるようになりました。

<pre class="source" title="">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }<em> = 10;</em>
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; } <em>= 20;</em>
}
</code></pre>

これで、コンストラクターを書かなくてもプロパティに対して初期値を与えることができます。

## <a id="sec-generated-title-8"></a> <a id="expression-bodied"></a>expression-bodied なプロパティ

get-only のプロパティに限りますが、他のいくつかの関数メンバーと同様に、expression-bodied (本体が式の)形式でプロパティを定義できます。
(参考: 「[expression-bodied な関数](../structured/st_function.md#sec-expression-bodied)」)

先ほどから例に挙げている複素数クラスでいうと、Abs プロパティの定義が楽になります。

<pre class="source" title="" lang="">
<code><span class="reserved">using static</span> System.<span class="type">Math</span>;

<span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public double</span> Re { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public double</span> Im { <span class="reserved">get</span>; <span class="reserved">set</span>; }

<em>    <span class="reserved">public double</span> Abs =&gt; Sqrt(Re * Re + Im * Im);</em>
}
</code></pre>



## <a id="sec-generated-title-9"></a> <a id="indexed"></a>余談: C# にインデックス付きプロパティはありません

VB にはある「インデックス付きプロパティ」は、C# にはありません。
C# の流儀的には、「インデックス付きプロパティ」よりも、「コレクションクラスを返す普通のプロパティ」推奨です。
（その方が、foreach が使えたり、色々便利だから。）

<pre class="source" title="ダメな例： インデックス付きプロパティ" lang="">
<code><span class="reserved">int</span>[] x;
<span class="comment">// ↓これは文法違反。</span>
<span class="reserved">public int</span> X[<span class="reserved">int</span> i]
{
    <span class="reserved">get</span> { <span class="reserved">return</span> x[i]; }
    <span class="reserved">private set</span> { x[i] = value; }
}
</code></pre>


<pre class="source" title="一応、可能： 配列を返すプロパティ" lang="">
<code><span class="reserved">int</span>[] x;
<span class="comment">// ↓これなら OK。</span>
<span class="reserved">public int</span>[] X
{
    <span class="reserved">get</span> { <span class="reserved">return</span> x; }
}
</code></pre>


C# 2.0 や C# 3.0 を見こすなら、以下のように、配列や ICollection ではなく、IEnumerable を返すようにする方がいいかもしれません。
（詳細は「[イテレーター](../data/sp2_iterator.md)」参照。）

<pre class="source" title="C# 2.0 的には： イテレーターを使って IEnumerable で返す" lang="">
<code><span class="reserved">int</span>[] x;
<span class="reserved">public</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; X
{
    <span class="reserved">get</span> { <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> x) <span class="reserved">yield return</span> item; }
}
</code></pre>


ちなみに、VB にはあることからわかるように、.NET 的にはインデックス付きプロパティを認めています。
C# から呼び出す場合は、get_*** というような名前のメソッド呼び出しになります。
例えば、VB で X と言う名前で、int を引数にとるインデックス付きプロパティを定義した場合、
C# からは get_X(0) というように呼び出します。

さらに特殊事情として、対 COM の場合だけ、普通に X[0] というような呼び出し方が認められます。
詳しくは「[COM 相互運用時の特別処理](../interop/sp4_cominterop.md)」を参照。

## <a id="sec-generated-title-10"></a> <a id="init-only"></a>init-only プロパティ

<h5 class="version version9">Ver. 9</h5>

C# 9.0 では、`set` に代わって、`init` という名前のアクセサーを定義できるようになりました。
例えば以下のように書けます(ちなみに、`set` と `init` は同時には書けません。排他です)。

<pre class="source" title="init アクセサー">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> Re { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Im { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
}
</code></pre>

`init` アクセサーを持っているプロパティは通称 <strong id="key-get-only" class="keyword">init-only プロパティ</strong>(init-only property)と呼ばれます。

用途としては [get-only プロパティ](#get-only) や [`readonly` フィールド](../start/sp_const.md#readonly)とほとんど同じです。
ただ、`readonly` の制限が厳しすぎるので、問題ない範囲でちょっとだけ制限を緩めたものが `init` アクセサーです。
(歴史的経緯で `init` という新キーワードが使われていますが、もし C# をフルスクラッチで作り直せるなら `readonly` が最初から `init` 相当の仕様になっていたと思います。)

まず、`readonly` と同じ点として、コンストラクター内での書き換えはできます。

<pre class="source" title="init はコンストラクター内から書き換え可能">
<code><span class="reserved">class</span> <span class="type">Complex</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> Re { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Im { <span class="reserved">get</span>; <span class="reserved">init</span>; }
 
    <span class="reserved">public</span> <span class="type">Complex</span>(<span class="reserved">double</span> <span class="variable">re</span>, <span class="reserved">double</span> <span class="variable">im</span>)
    {
        <span class="comment">// この2行は OK。</span>
        Re = <span class="variable">re</span>;
        Im = <span class="variable">im</span>;
    }
}
</code></pre>

一方、`readonly` では認められてないことで、`init` であればできることが3つあります。

- [オブジェクト初期化子](oo_construct.md#member_initializer)での書き換え
- 他の `init` アクセサー内での書き換え
- [`with` 式での書き換え](../datatype/record.md#with)

例えば、以下のコード(get-only プロパティを利用)はコンパイルできませんが、

<pre class="source" title="get-only プロパティはオブジェクト初期化子を使えない">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span> { <span class="error">X</span> = 1, <span class="error">Y</span> = 2 };
 
<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
}
</code></pre>

以下のように init-only プロパティに書き換えるとコンパイルできます。

<pre class="source" title="init-only プロパティならオブジェクト初期化子を使える">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
 
<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved"><em>init</em></span>; }
}
</code></pre>

初期化子の外で書き換えようとすると、`readonly`と同じくコンパイル エラーになります。

<pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
<span class="error"><span class="variable">p</span>.X</span> = 3; <span class="comment">// ダメ。</span>
</code></pre>

`with` 式については別途解説予定(トラッキング issue: [C# 9.0](https://github.com/ufcpp/UfcppSample/issues/297))ですが、
例えば以下のようなコードが書けます。

<pre class="source" title="with 式で init-only プロパティを書き換え">
<code><span class="reserved">var</span> <span class="variable">p0</span> = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
<span class="reserved">var</span> <span class="variable">p1</span> = <span class="variable">p0</span> <span class="reserved">with</span> { X = 3 }; <span class="comment">// p0 のクローンを作った上で、X だけ 3 で上書き。</span>
 
<span class="reserved">record</span> <span class="type">Point</span>(<span class="reserved">int</span> <span class="variable">X</span>, <span class="reserved">int</span> <span class="variable">Y</span>);
</code></pre>

他の `init` アクセサーからの書き換えは、例えば以下のようなコードを書けます。

<pre class="source" title="他の init アクセサーからの書き換え">
<code><span class="reserved">using</span> System;
 
<span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">Squared</span> { ValueSquared = 4 };
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>.Value); <span class="comment">// 2</span>
 
<span class="reserved">class</span> <span class="type">Squared</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> Value { <span class="reserved">get</span>; <span class="reserved">init</span>; }
 
    <span class="reserved">public</span> <span class="reserved">double</span> ValueSquared
    {
        <span class="reserved">get</span> =&gt; Value * Value;
        <span class="reserved">init</span> =&gt; Value = <span class="type">Math</span>.<span class="method">Sqrt</span>(<span class="reserved">value</span>);
    }
}
</code></pre>

ちなみに、`init` アクセサー内では `readonly` フィールドも書き換え可能です。

<pre class="source" title="init アクセサー内で readonly フィールドを書き換え">
<code><span class="reserved">class</span> <span class="type">Squared</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">double</span> Value;
 
    <span class="reserved">public</span> <span class="reserved">double</span> ValueSquared
    {
        <span class="reserved">get</span> =&gt; Value * Value;
        <span class="reserved">init</span> =&gt; Value = <span class="type">Math</span>.<span class="method">Sqrt</span>(<span class="reserved">value</span>); <span class="comment">// OK。</span>
    }
}
</code></pre>

### <a id="sec-generated-title-11"></a> <a id="init-only-internal">init-only プロパティの中身</a>

ちなみに、init-only プロパティコンパイル結果としては単に `public` な `set` アクセサーと `readonly` フィールドになっています。
C# コンパイラーのレベルで「初期化子など以外からの書き換えを禁止する」というような解析をしています。

この解析に対応していない古い C# コンパイラーから `set` を呼ばれるとかなりまずい(本来書き換えられないはずの `readonly` フィールドが書き換わる)ので、それを禁止するために modreq という修飾機能を使っています。

modreq については別途説明予定です。トラッキング issue:

- [新機能の実装方法(modreq + RuntimeFeature)](https://github.com/ufcpp/UfcppSample/issues/295)
- [modreq って何？](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/4)


<!-- original-page-break -->

## <a id="sec-generated-title-12"></a> <a id="required"></a>required メンバー

<h5 class="version version11">Ver. 11</h5>

C# 11 でプロパティとフィールドに対する `required` 修飾子というものが追加されました。
これを使うと、[オブジェクト初期化子](oo_construct.md#member_initializer)で何らかの値を代入することを義務付けられます。
例えば以下のようなコードを書いたとき、`a1` 以外の `new A` はエラーになります。
(警告ではなくエラーにします。)

<pre class="source" title="required 修飾子">
<span class="reserved">var</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span> { <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span> };

<span class="reserved">var</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035">A</span></span> { <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span> }; <span class="comment">// Y を代入していないのでエラー。</span>
<span class="reserved">var</span> <span class="variable">a3</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035">A</span></span> { <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span> };   <span class="comment">// X を代入していないのでエラー。</span>
<span class="reserved">var</span> <span class="variable">a4</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035"><span class="error" title="CS9035">A</span></span></span>();             <span class="comment">// X も Y も代入していないのでエラー。</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <em><span class="reserved">required</span></em> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <em><span class="reserved">required</span></em> <span class="reserved">int</span> <span class="property">Y</span>;
}
</pre>

この機能を指して、<strong id="key-required" class="keyword">required メンバー</strong> (required members)と言います。

### <a id="sec-generated-title-13"></a> <a id="required-needs">required の必要性</a>

C# のオブジェクトの初期化には以下の2種類の構文があります。

* `new A(x, y)`: コンストラクターに引数で値を与える
    * 引数を並べる順序に意味があって、渡す先に仮引数名は指定しないので「位置指定」(positional)初期化と呼ぶ
* `new A { X = x, Y = y }`: オブジェクト初期化子でプロパティに値を与える
    * 順序に意味がなくて、プロパティ名は指定するので「名前指定」(nominal)初期化と呼ぶ

元々の C# にはコンストラクター(位置指定初期化)しかなかったのに対して、C# 3 でオブジェクト初期化子が導入されて名前指定初期化ができるようになりました。
C# 3 当時は名前指定初期化という考え方もなくて、あくまでコンストラクターの補助的な立ち位置でしたが、今となってはコンストラクターと対を成すような扱いを受けています。

クラスを作っている側で手間を惜しまないのであれば、普通にコンストラクターがある方が、使う側にとっては便利なことが多かったりします。
ただ、作る側の面倒は結構多いです。

まず、単にコンストラクターが増えるだけで手間。
よく言われる話ですが、プロパティ1個に対して同じような文字列を4回は繰り返す必要が出ます。

<pre class="source" title="コンストラクターを用意する手間">
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="string">&quot;abc&quot;</span>, <span class="number">123</span>); <span class="comment">// 使う側は簡潔。</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; } <span class="comment">// ここに X を書いて</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; }

    <span class="reserved">public</span> <span class="type">A</span>(<span class="reserved">string</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>) <span class="comment">// ここにも x</span>
    {
        <span class="property">X</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// ここに至っては2個の X</span>
        <span class="property">Y</span> <span class="operator">=</span> <span class="variable local">y</span>;
    }
}
</pre>

さらに、このクラス `A` を継承して、もう1個 `Z` プロパティを持った型 `B` を作ることを考えます。
以下のように、さらに追加で2か所同じ文字列を追加する必要があります。

<pre class="source" title="継承するとさらにかかる手間">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// A の中身はさっきと一緒。</span>
}

<span class="comment">// 派生クラスで1プロパティ増やしたくなった時</span>
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="property">Z</span> { <span class="reserved">get</span>; }

    <span class="reserved">public</span> <span class="type">B</span>(<span class="reserved">string</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>, <span class="reserved">bool</span> <span class="variable local">z</span>) <span class="comment">// さらにここと、</span>
        : <span class="reserved">base</span>(<span class="variable local">x</span>, <span class="variable local">y</span>) <span class="comment">// ここにも x が必要。</span>
    {
        <span class="property">Z</span> <span class="operator">=</span> <span class="variable local">z</span>;
    }
}
</pre>

これに対して、名前指定初期化の場合はプロパティだけ書けばいいのでずいぶんと楽です。

<pre class="source" title="名前指定初期化はクラス定義側が楽">
<span class="comment">// 使う側は多少長いものの、名前を明示してる分読みやすいかも。</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">B</span>
{
    <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>,
    <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span>,
    <span class="property">Z</span> <span class="operator">=</span> <span class="reserved">true</span>,
};

<span class="comment">// クラス定義側は簡素に。</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property"><span class="warning" title="CS8618">X</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}

<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="property">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</pre>

ところがこれには1つ問題があります。
このコードの例で、`X` プロパティのところに警告(CS8618)が出てしまっています。
この警告は [null 許容参照型](../resource/nullablereferencetype.md)を有効化してるときにだけ発生するんですが、要するに、
「`X` の型は (非 null な) `string` なのに、有効な初期値を与えていない」というものです。
非 null な以上、何も値を与えない(勝手に null に初期化される)わけにはいきません。

そこで `required` が導入されました。
「名前指定にはしたいけど、明示的な初期化も義務付けたい」という要件です。

<pre class="source" title="名前指定にはしたいけど、明示的な初期化も義務付けたいときには required">
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>
{
    <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>, <span class="comment">// 非 null に初期化される保証がこの行でできる.</span>
    <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span>,
};

<span class="comment">// 明示的な初期化を義務付けたいプロパティ/フィールドには required を付ける。</span>
<span class="comment">// これを使えば null 許容参照型での問題も回避可能。</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</pre>

ちなみに、null 許容参照型は「わかりやすい需要の例」ではありますが、
別にその他の場面でも `required` は使えます。
とにかく「初期化を明示させたい」というものなので、値型や null 許容型でも使えます。

<pre class="source" title="とにかく「初期化を明示させたい」">
<span class="comment">// 全部 0 か null なので、別に new A() でも結果は同じものの、明示させたいという意図があるなら required。</span>
<span class="reserved">var</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span> { <span class="property">X</span> <span class="operator">=</span> <span class="reserved">null</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">0</span>, <span class="property">Z</span> <span class="operator">=</span> <span class="reserved">null</span> };

<span class="reserved">var</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type"><span class="error" title="CS9035">A</span></span> { <span class="property">X</span> <span class="operator">=</span> <span class="reserved">null</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">0</span> }; <span class="comment">// Z がないのでエラー。</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// default 値(0 や null)でもいいけども、とにかく明示はさせたい。</span>
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">int</span><span class="operator">?</span> <span class="property">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</pre>

### <a id="sec-generated-title-14"></a> <a id="applicable">required の適用範囲</a>

`required` は、`virtual` や `abstract` なプロパティに対しても使えます。
ただし、基底クラス側が `required` なものは派生クラス側にも `required` を付ける必要があります。

<pre class="source" title="派生と required">
<span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">abstract</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="property">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}

<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="comment">// 基底クラス側が required なら、こっちも required でないとダメ。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    <span class="comment">// 逆は大丈夫。基底クラスになくても、派生クラス側だけ required を足すことはできる。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">Z</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}

<span class="reserved">class</span> <span class="type">C</span> : <span class="type">A</span>
{
    <span class="comment">// 派生側で required を取ってしまうとコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9030">X</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</pre>

そして、`required` はオブジェクト初期化で使うことが前提なので、
`new` できないインターフェイスに対しては使えません。

<pre class="source" title="インターフェイスには required を使えない">
<span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// エラー。</span>
    <span class="reserved">required</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS0106">X</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}</pre>

また、オブジェクト初期化子で値を渡せるように、
プロパティ/フィールドのアクセシビリティは、それを含む型よりも広い必要があります。
例えば、`internal` クラスの `internal` プロパティには使えますが、
`public` クラスの `protected` プロパティには使えません。

<pre class="source" title="required メンバーのアクセシビリティの制限">
<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// internal クラスの internal プロパティなので OK。</span>
    <span class="reserved">internal</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">B</span>
{
    <span class="comment">// public 未満のアクセシビリティは全部不可。以下は全部エラー。</span>
    <span class="reserved">protected</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9032">X1</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">internal</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9032">X2</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">internal</span> <span class="reserved">protected</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9032">X3</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">protected</span> <span class="reserved">private</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS9032">X4</span></span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">private</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="field"><span class="error" title="CS9032">X5</span></span>;
}
</pre>

### <a id="sec-generated-title-15"></a> <a id="SetsRequiredMembers">SetsRequiredMembers</a>

`required` メンバーをコンストラクター内で初期化するのであれば、
呼び出し元のオブジェクト初期化子では必ずしも初期化の必要がない場合があります。
こういう場合にエラーを出されても困るので、
`SetsRequiredMembers` という属性(`System.Diagnostics.CodeAnalysis` 名前空間)を使って「このコンストラクターを呼んだ場合は `required` メンバーの初期化をする必要はない」
という指定もできます。

<pre class="source" title="SetsRequiredMembers 属性の例">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// required メンバーは A() (引数なしコンストラクター)で初期化するので、</span>
<span class="comment">// この場合は { X = &quot;&quot; } とかがなくてもエラーにならない。</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    [<span class="type">SetsRequiredMembers</span>]
    <span class="reserved">public</span> <span class="type">A</span>()
    {
        <span class="property">X</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>;
        <span class="property">Y</span> <span class="operator">=</span> <span class="number">123</span>;
    }
}
</pre>

ただ、この `SetsRequiredMembers` は、利用側(呼び出した側)のエラーはなくしてくれる一方で、
作る側(コンストラクターの実装側)では特に何もしてくれません。
単にエラーを消します。

<pre class="source" title="自称 SetsRequiredMembers">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// 自称 SetsRequiredMembers を信じてエラーは出さない。</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span><span class="operator">.</span><span class="property">X</span>); <span class="comment">// null</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">string</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    [<span class="type">SetsRequiredMembers</span>]
    <span class="reserved">public</span> <span class="type"><span class="warning" title="CS8618">A</span></span>()
    {
        <span class="comment">// 「requierd メンバーをセットする」と自称しているくせに、実際は何もしない。</span>
        <span class="comment">// X に関しては nullability のフロー解析で、null 許容参照型警告が出るけども、全くの別件。</span>
        <span class="comment">// Y に関しては一切何もチェックが働かない。</span>
        <span class="comment">// 少なくとも C# 11 リリース時点では「仕様」(問題はわかっているものの、実装が大変なので妥協)。</span>
        <span class="comment">// 現状の SetsRequiredMembers は「使う側はコンパイラーが守るけど、作る側は自分で頑張って」という姿勢。</span>
    }
}
</pre>

### <a id="sec-generated-title-16"></a> <a id="required-internal">required メンバーの中身</a>

required メンバーを含む型は、内部的には属性を付けて表現しているようです。
例えば、以下のようなクラスがあったとします。

<pre class="source" title="シンプルな required メンバーの例">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</pre>

これをコンパイルすると、以下のようなコードに展開されます。

<pre class="source" title="上記の例の展開結果">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

[<span class="type">RequiredMember</span>]
<span class="reserved">class</span> <span class="type">A</span>
{
    [<span class="type">RequiredMember</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    [<span class="type">Obsolete</span>(<span class="string">&quot;Constructors of types with required members are not supported in this version of your compiler.&quot;</span>, <span class="reserved">true</span>)]
    [<span class="type">CompilerFeatureRequired(<span class="string">&quot;RequiredMembers&quot;</span>)</span>]
    <span class="reserved">public</span> <span class="type">A</span>() { }
}
</pre>

型と、required メンバー自体には `RequiredMember` 属性(`System.Runtime.CompilerServices` 名前空間)が付いていて、これで required かどうかを判断しています。

そして、引数なしコンストラクターが追加されて、
そこに `Obsolete` と `CompilerFeatureRequired` 属性が付きます。
これらは required メンバーに未対応の古いコンパイラーでこのクラスを使ったときにエラーにするための属性です。
これは本来どちらか片方でいいんですが、それぞれ以下のような用途です。

* 既存の仕組みでエラーにできるように `Obsolete` 属性を付けている
    * required メンバーに対応しているコンパイラーの場合、「所定のメッセージの場合は無視してエラーにしない」みたいな特殊対応をしている
* `Obsolete` による対処は気持ち悪いので、「未対応ならエラー」のために新しい `CompilerFeatureRequired` 属性を作った
    * こちらは素直に、`featureName` 引数に与えた文字列を見て対応できるかどうかを判定
    * `CompilerFeatureRequired` に対応していないコンパイラーのサポートが切れるくらいの頃に `Obsolete` は消したい

[`init` の場合](#init-only-internal)とは違って、modreq (属性よりも強い制約でコンパイル エラーにできる機構)は使わない方針です。
以下のような状況を考えると、制約が強い modreq は使いにくいそうです。
(不意に、コンパイラーが裏で勝手に作るコンストラクターが増えることがある。
不意に増えるものに使うには modreq は強すぎる。)

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">required</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    <span class="comment">// SetsRequiredMembers なコンストラクターを明示。</span>
    <span class="comment">// この場合、Obsolete, CompilerFeatureRequired 付きのコンストラクターはコンパイラー生成されない。</span>
    <span class="comment">// もし、このコンストラクターを消すと…</span>
    <span class="comment">// コンパイラーが裏で Obsolete, CompilerFeatureRequired 付きを作ってしまう。</span>
    [<span class="type">SetsRequiredMembers</span>]
    <span class="reserved">public</span> <span class="type">A</span>() { }
}
</pre>

## <a id="sec-generated-title-17"></a> <a id="field-keyword">field キーワード</a>

<h5 class="version version14">Ver. 14</h5>

[自動プロパティ](#auto)ではバッキング フィールドへの値の素通しが行われます。
これに対して、ちょこっとだけ実装をいじりたいことが結構あります。
特によくあるのが「バッキング フィールドの生成は自動でやってほしいけど、`get`/`set` の中身は自分で書きたい」という状況で、例えば下のような例があります。

<pre class="source" title="惜しくも自動にならないプロパティ">
<span class="reserved">using</span> System<span class="operator">.</span>ComponentModel;
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">FieldBackedProperties</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="comment">// 遅延初期化: 最初のアクセス時にインスタンスを生成。</span>
    <span class="reserved">private</span> <span class="reserved">string</span><span class="operator">?</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">??=</span> <span class="string">&quot;&quot;</span>;

    <span class="comment">// set 側だけ null 許容(get 側で ?? で非 null 化)。</span>
    <span class="reserved">private</span> <span class="reserved">string</span><span class="operator">?</span> <span class="field">_y</span>;

    [<span class="type">AllowNull</span>]
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Y</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }

    <span class="comment">// INotifyPropertyChanged の実装: get 側だけ素通し。</span>
    <span class="reserved">private</span> <span class="reserved">string</span><span class="operator">?</span> <span class="field">_z</span>;

    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Z</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_z</span>;
        <span class="reserved">set</span>
        {
            <span class="control">if</span> (<span class="field">_x</span> <span class="operator">!=</span> <span class="reserved">value</span>)
            {
                <span class="field">_z</span> <span class="operator">=</span> <span class="reserved">value</span>;
                PropertyChanged<span class="operator">?</span><span class="operator">.</span><span class="method">Invoke</span>(<span class="reserved">this</span>, <span class="reserved">new</span>(<span class="reserved">nameof</span>(<span class="property">Z</span>)));
            }
        }
    }

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">PropertyChangedEventHandler</span><span class="operator">?</span> PropertyChanged;
}
</pre>

これに対して C# 14 では、 `field` キーワードというものを追加しました。
プロパティの `get`/`set` の中に `field` と書くと、
バッキング フィールドを生成した上で、そのフィールドの読み書きができます。
例えば前述の例を `field` を使って書き直すと以下のようになります。

<pre class="source" title="field キーワードを使って書き直し">
<span class="reserved">using</span> System<span class="operator">.</span>ComponentModel;
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">FieldBackedProperties</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="comment">// 遅延初期化: 最初のプロパティ アクセス時にインスタンスを生成。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="string">&quot;&quot;</span>;

    <span class="comment">// set 側だけ null 許容(get 側で ?? で非 null 化)。</span>
    [<span class="type">AllowNull</span>]
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Y</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>;
        <span class="reserved">set</span>;
    }

    <span class="comment">// INotifyPropertyChanged の実装: get 側だけ素通し。</span>
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Z</span>
    {
        <span class="reserved">get</span>;
        <span class="reserved">set</span>
        {
            <span class="control">if</span> (<span class="reserved">field</span> <span class="operator">!=</span> <span class="reserved">value</span>)
            {
                <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
                PropertyChanged<span class="operator">?</span><span class="operator">.</span><span class="method">Invoke</span>(<span class="reserved">this</span>, <span class="reserved">new</span>(<span class="reserved">nameof</span>(<span class="property">Z</span>)));
            }
        }
    }

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">PropertyChangedEventHandler</span><span class="operator">?</span> PropertyChanged;
}
</pre>

`field` キーワードには以下のようなメリットがあります。

* 重複を避けれる
  * この例の場合は `_x` みたいな短い名前なものの、プロパティ名はもっと長いことが多いので繰り返したくない
  * プロパティの型も、型名が長いことが多々ある
* 他のプロパティから参照されるのを避けれる
  * ほとんどの場合「`_x` は `X` 内でしか使わない」みたいなことになるのに、`_x` が他のメソッドやプロパティから見えてしまっていた

ちなみに(この例で既に使っていますが)、自動実装(空っぽの `get`/`set`)との併用もできます。
`get;` は `get => field;` と、
`set;` は `set => field = value;` と同じ意味になります。

### <a id="sec-generated-title-18"></a> <a id="field-backed-property">自動プロパティとの共通点</a>

既存の自動プロパティと、 C# 14 で追加された `field` キーワードを使ったプロパティは
「バッキング フィールドが自動生成される」という意味で共通しているわけですが、
これらを合わせて field-baked プロパティ(フィールドで裏付けされたプロパティ)と呼びます。
ひとくくりにする言葉が用意されているくらいにはこの2つは扱われ方が似ています。

以下は一例ですが、「`get` だけ書くと [get-only プロパティ](#get-only)になる」という挙動は完全に一致します。

<pre class="source" title="field-backed プロパティの get-only 化">
<span class="reserved">class</span> <span class="type">GetOnly</span>
{
    <span class="comment">// 元々ある get-only プロパティ。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; }

    <span class="comment">// get =&gt; field; と get; は全く同じ意味で、これも get-only プロパティになる。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>; }

    <span class="comment">// 何ならこれも get =&gt; field; の省略形なので get-only プロパティになる。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Z</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>;

    <span class="comment">// 中身をカスタマイズしても、field キーワードを使っている時点で get-only プロパティ。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">W</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">+</span> <span class="number">1</span>;

    <span class="reserved">public</span> <span class="type">GetOnly</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>, <span class="reserved">int</span> <span class="variable local">z</span>, <span class="reserved">int</span> <span class="variable local">w</span>)
    {
        <span class="comment">// なので set; を省略していても、コンストラクター内に限り値の代入が可能。</span>
        <span class="comment">// (バッキング フィールドへの直代入扱い。)</span>
        <span class="property">X</span> <span class="operator">=</span> <span class="variable local">x</span>;
        <span class="property">Y</span> <span class="operator">=</span> <span class="variable local">y</span>;
        <span class="property">Z</span> <span class="operator">=</span> <span class="variable local">z</span>;
        <span class="property">W</span> <span class="operator">=</span> <span class="variable local">w</span>;
    }
}
</pre>

他の例として、`ref` 付きのバッキング フィールドは作れないという制限も共通です。

<pre class="source" title="ref 付きのプロパティは field-backed プロパティにできない">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">RefField</span>
{
    <span class="comment">// ref 付きのプロパティは自動実装にできない。</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="error" title="CS8145"><span class="property">X</span></span> { <span class="reserved">get</span>; }

    <span class="comment">// 同じく field キーワードは使えない。</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="property"><span class="error" title="CS8145">Y</span></span> <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="reserved">field</span>;

    <span class="comment">// 参考: これなら書ける。(警告は別件。)</span>
    <span class="reserved">private</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field"><span class="warning" title="CS9265">_z</span></span>;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="property">Z</span> <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="field">_z</span>;
}
</pre>

### <a id="sec-generated-title-19"></a> <a id="field-contextual-keyword">文脈キーワード</a>

`field` 「キーワード」とは言っていますが、
他の例にもれず `field` は[文脈キーワード](../misc/ap_compatibility.md#contextual-keyword)です。
プロパティの `get`/`set` 内でだけキーワード扱いされます。

<pre class="source" title="field は文脈キーワード">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// これは普通にフィールド。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field"><span class="warning" title="CS0169">field</span></span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">M</span>()
    {
        <span class="comment">// これは普通にローカル変数。</span>
        <span class="reserved">var</span> <span class="variable">field</span> <span class="operator">=</span> <span class="number">123</span>;
        <span class="control">return</span> <span class="variable">field</span>;
    }

    <span class="comment">// これは文脈キーワードの field。</span>
    <span class="comment">// (ちなみにこの例では「同名のフィールドがあるけど大丈夫？」と警告される。)</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="reserved"><span class="warning" title="CS9258">field</span></span>;
}

<span class="comment">// これも警告は出るものの合法。普通に型名。</span>
<span class="comment">// (「小文字アルファベット始まるの型名は将来の文脈キーワードと被る可能性が高いからやめてほしい」という警告。)</span>
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8981">field</span></span>;

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="comment">// こんなのすら合法。</span>
    <span class="reserved">public</span> <span class="type">field</span> <span class="method">field</span>(<span class="type">field</span> <span class="variable local">field</span>) <span class="operator">=&gt;</span> <span class="variable local">field</span>;
}
</pre>

この例のような「`field` という名前のフィールド」は元々書けていたわけで、
`field` キーワードの追加はたとえ文脈キーワードだとしても破壊的変更です。
以下のコードは C# 13 と 14 で解釈が異なります。

<pre class="source" title="field キーワードの追加は破壊的変更">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field"><span class="warning" title="CS0649">field</span></span>;

    <span class="comment">// C# 13: field フィールドを参照。</span>
    <span class="comment">// C# 14: X のバッキング フィールドが自動生成されて、それを参照。</span>
    <span class="comment">//        (field フィールドとは別のフィールドが生成される。)</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="reserved"><span class="warning" title="CS9258">field</span></span>;

    <span class="comment">// 以前の挙動を得るためには:</span>

    <span class="comment">// @ を付けるとキーワードではなくなる。この名前のフィールドを参照。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> <span class="operator">=&gt;</span> <span class="field">@field</span>;

    <span class="comment">// this. を付けてもフィールド参照にできる。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Z</span> <span class="operator">=&gt;</span> <span class="reserved">this</span><span class="operator">.</span><span class="field">field</span>;
}
</pre>

### <a id="sec-generated-title-20"></a> <a id="field-keyword-initializer">プロパティ初期化子</a>

プロパティ初期化子を使う場合ちょっと注意が必要になります。
初期化子で値を渡す場合、プロパティの `set` アクセサー呼び出しではなく、バッキング フィールドへの直代入になります。

<pre class="source" title="プロパティ初期化子では set が呼ばれない">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">PropertyInitializer</span>(<span class="number">10</span>);

<span class="comment">// x.X は 10 になる。</span>
<span class="comment">// set が呼ばれていなくて、バッキング フィールドに直接 10 が渡る。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="property">X</span>);

<span class="reserved">class</span> <span class="type">PropertyInitializer</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span> <span class="operator">+</span> <span class="number">1</span>; <span class="comment">// 値を1ずらす</span>
    } <span class="operator">=</span> <span class="variable local">x</span>;
}
</pre>

コンストラクターの場合はこんなことはなくて、ちゃんと `set` アクセサーが呼ばれます。

<pre class="source" title="コンストラクター内で初期化するとちゃんと set が呼ばれる">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Constructor</span>(<span class="number">10</span>);

<span class="comment">// x.X は 11 になる。</span>
<span class="comment">// ちゃんと set 経由でバッキング フィールドの初期化が行われる。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="property">X</span>);

<span class="reserved">class</span> <span class="type">Constructor</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span> <span class="operator">+</span> <span class="number">1</span>; <span class="comment">// 値を1ずらす</span>
    }

    <span class="reserved">public</span> <span class="type">Constructor</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
    {
        <span class="property">X</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// この場合は set アクセサーが呼ばれる。</span>
    }
}
</pre>

変な挙動ではありますが、これは初期化子やコンストラクターの実行順序に関係しています。
「[コンストラクター](oo_construct.md#initializer-order)」や
「[継承](oo_inherit.md#ctor)」で説明していますが、フィールド初期化子やプロパティ初期化子でインスタンス メソッドを呼べてしまうと、未初期化のフィールドを読んでしまう可能性があります。
プロパティのアクセサーの実態はメソッドとほぼ同じなので同様の問題があり得て、
初期化子で `set` アクセサーは呼んではいけないということになります。
そのため仕方なく、プロパティ初期化子ではフィールドへの直代入する仕様になっています。

### <a id="sec-generated-title-21"></a> <a id="backing-field-nullability">バッキング フィールドの null 許容性</a>

プロパティが参照型のとき、そのバッキング フィールドの [null 許容性](../resource/nullablereferencetype.md)はどうあるべきでしょうか？
本節冒頭の例でも挙げたように、`field` キーワードの用途の1つに遅延初期化があります。
この場合、「`T` 型のプロパティのバッキング フィールドは `T?` の方が都合がいい」ということになります。

<pre class="source" title="T 型の遅延初期化では T? が都合がいい">
<span class="reserved">class</span> <span class="type">LazyInit</span>
{
    <span class="comment">// field は string? でも大丈夫。</span>
    <span class="comment">// 一方で、field が string だとすると「コンストラクターで非 null に初期化しろ」警告が出るはず。</span>
    <span class="comment">// つまり、field は string? の方が都合がいい。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>;
}
</pre>

かといって常に `T?` にすればいいというものでもなく、`T` でないとまずい場合もあります。 
ちょっと複雑な例ですが、以下のコードを見てください。

<pre class="source" title="string プロパティのバッキング フィールドは string か string? か">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">AllowNullSetter</span>
{
    <span class="comment">// AllowNull を付けると set 側だけ nullable になる。</span>
    <span class="comment">// obj.X = null; を渡せて、でも、var x = obj.X; は null にならない。</span>

    <span class="comment">// フィールドは string? であってほしい例: </span>
    [<span class="type">AllowNull</span>]
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>; <span class="comment">// こっちで非 null を保証。</span>
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }

    <span class="comment">// フィールドは string であってほしい例: </span>
    [<span class="type">AllowNull</span>]
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Y</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>; <span class="comment">// こっちで非 null を保証。</span>
    } <span class="operator">=</span> <span class="string">&quot;&quot;</span>;
}
</pre>

これをコンパイラーが正しく判断できるように、`get`/`set` 両方合わせてフロー解析する仕様になっています
(通常、null 許容性のフロー解析は2つ以上のメソッドをまたいで行いません。
`get`/`set` の中身はそれぞれ独立したメソッドなので、ここだけの特殊処理になります)。
`get` 側で `field` が `T?` だと思ってフロー解析してみて警告にならなかった場合、
`set` 側も `field` が `T?` かもしれない前提でフロー解析します。

<pre class="source" title="get の解析結果を踏まえて set をフロー解析">
<span class="reserved">class</span> <span class="type">Nullability</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>; <span class="comment">// field は string? でも問題ない。</span>
        <span class="reserved">set</span>
        {
            <span class="comment">// string? 扱いでフロー解析。</span>
            <span class="reserved">string</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved"><span class="warning" title="CS8600">field</span></span>; <span class="comment">// ここで警告。</span>
        }
    }

    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property"><span class="warning" title="CS9264">Y</span></span> <span class="comment">// ここに「非 null 初期化しろ」警告が出る。</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>; <span class="comment">// field は string でないとおかしい。</span>
        <span class="reserved">set</span>
        {
            <span class="comment">// string 扱いでフロー解析。</span>
            <span class="reserved">string</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">field</span>; <span class="comment">// 警告なし。</span>
        }
    }

    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Z</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>;
        <span class="reserved">set</span>
        {
            <span class="comment">// string? 扱いでフロー解析するとしても、</span>
            <span class="comment">// value が string なのでここより後ろでは field は非 null。</span>
            <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
            <span class="reserved">string</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">field</span>; <span class="comment">// 警告なし。</span>
        }
    }

    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">W</span>
    {
        <span class="reserved">set</span>
        {
            <span class="comment">// ちなみに get を省略すると field は string? 扱いになる。</span>
            <span class="reserved">string</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved"><span class="warning" title="CS8600">field</span></span>; <span class="comment">// ここで警告。</span>
        }
    }
}
</pre>

ちなみにこの挙動はあくまで [null 許容参照型](../resource/nullablereferencetype.md)に対するものです。
[null 許容値型](../resource/sp2_nullable.md)の場合は「`T` 型プロパティのバッキング フィールドは常に `T`」になります。
`int X => field ??= 1;` などと書くとエラー(`field` は `int?` にはならず `int`。`int` に対して `??` は使えない)になります。
## <a id="exercise"></a>演習問題

### <a id="exercise-prop1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスの各メンバー変数に対して、
プロパティを使って実装の隠蔽を行え。


#### 解答例 1


<pre class="source" title="Point/Triangle" lang="">
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

  public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
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

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.X - a.X;
    aby = b.Y - a.Y;
    acx = c.X - a.X;
    acy = c.Y - a.Y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
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

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
  }
}
</code></pre>
