---
title: "静的メンバー"
source_url: "https://ufcpp.net/study/csharp/oop/oo_static/"
content_type: "Article"
published_at: "2015-05-06T14:09:31"
updated_at: "2015-01-18T00:00:00"
tags:
  - "Ver. 2.0"
  - "Ver. 3.0"
  - "Ver. 6.0"
umbraco_id: 1257
parent_id: 1248
sort_order: 5
aliases:
  - "/csharp/oo_static"
  - "/csharp/oo_static.html"
  - "/csharp/oop/oo_static/"
  - "/study/csharp/oo_static"
  - "/study/csharp/oo_static.html"
---

# 静的メンバー

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<strong id="static-member" class="keyword">静的メンバー</strong>（static member）とは、
特定のインスタンスにではなく、クラスに属するフィールドやメソッドのことです。
そのため、静的変数のとこを<em>クラス メンバー</em>とも呼びます。
(クラス変数という呼び名の方が意味合い的には正しいのですが、
C言語から派生したというC#の歴史的な背景のため、静的変数という呼び方をします。)

「静的」という言葉は、各種メンバー（フィールド、メソッド、プロパティなど）それぞれに対して、<strong id="stfield" class="keyword">静的フィールド</strong>、<strong id="stmethod" class="keyword">静的メソッド</strong>、静的プロパティ、… などという使い方もします。
また、静的メンバーとの区別を明確にしたい場合には、通常のメンバー変数のことを<em>インスタンス メンバー</em>と呼びます。


##### <a id="sec-generated-title-2"></a>ポイント
* 静的メンバー: この呼び方は歴史的なもので、実際にはクラス メンバー（クラス メソッド、クラス フィールド）と呼ぶ方がいいかも。
    * static キーワードをつけると静的メンバーになる。



* クラス メンバー: クラスに属するもの。全インスタンスで共有されるもの。

* インスタンス メンバー: 今まで説明してきたメンバー。インスタンスごとに別の値になる。



##<a id="sec-generated-title-3"></a> <a id="use"></a>静的メンバーの使い方
クラスのメンバー（フィールドやメソッド）を定義する際に、<em><code>static</code></em>キーワードを付けることで、
その変数は静的メンバー変数・静的メソッドになります。
例えば、静的フィールドであれば以下のように書きます。

<pre class="source" title="static 変数の定義" lang="">
<code><span class="reserved">static</span> <span class="input">型名</span> <span class="input">フィールド名</span>
</code></pre>


静的メンバーはクラスごとに唯一つの実体を持ち、すべてのインスタンスの間で共有されます。

例として、人間について考えてみましょう。
この場合、特定のインスタンスとは個人個人のこと、
クラスとは人間という種別そのもののことになるわけですが、
名前や年齢などは各個人ごとに異なります。
一方で、人という種の学名「Homo sapiens」などのように個体によらない共通のものもあります。
したがって、人間をあらわす<code>Person</code>というクラスを作成した場合、
<code>name</code>(名前)や<code>age</code>(年齢)といったメンバー変数を作りたい場合はインスタンス フィールドに、
<code>scientificName</code>(学名)などのクラス全体で共有すべき変数を作りたい場合は静的フィールドにすべきです。
(実際には学名などの普遍な値は定数(<code>const</code>)として定義すべきですが、
ここでは説明のためということでご容赦を。
定数の定義については後ほど説明します。)

<pre class="source" title="インスタンス フィールドと静的フィールドの例" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前。個体ごとに違うので、インスタンス フィールドに。</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢。同上、インスタンス フィールドに。</span>

  <span class="reserved">public <em>static</em> string</span> scientificName;
  <span class="comment">// 学名。個体じゃなくて種によって決まるものなので、静的フィールドに。</span>
}
</code></pre>


静的メンバーはクラスに属する値なので、値を参照するには、変数を介してではなく、以下のようにします。

<pre class="source" title="静的変数の参照" lang="">
<code>Person p = <span class="reserved">new</span> Person()

p.name = <span class="literal">"野上冴子"</span>; <span class="comment">// インスタンス フィールドは [インスタンス名.フィールド名] で参照する。</span>
p.age  = 40;

Person.scientificName = <span class="literal">"Homo Sapiens"</span>;
<span class="comment">// <em>静的フィールドは [クラス名.フィールド名] で参照する。</em></span>
</code></pre>


また、メソッドに対して static を付けると、
クラスに属するメソッドになります。
（静的メンバーにしかアクセスできなくなります。
メソッドからインスタンス フィールドなどにアクセスする必要が特にない場合には、静的メソッドにしておく方が実行効率がいい。）

数学関数や数学定数などのように、そもそもインスタンスを持つ必要のないものもあります。
この場合にも、静的メソッド・静的フィールド（あるいは別項で説明する「定数」）を使います。

<pre class="source" title="インスタンスを持たない関数の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> MyMath
{
  <span class="comment">// sin x を求める関数。</span>
  <span class="reserved">public</span> <span class="reserved"><em>static</em> double</span> Sin(<span class="reserved">double</span> x)
  {
    <span class="reserved">double</span> xx = -x * x;
    <span class="reserved">double</span> fact = 1;
    <span class="reserved">double</span> sin = x;
    <span class="reserved">for</span>(<span class="reserved">int</span> i=2; i&lt;100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    <span class="reserved">return</span> sin;
  }
}

<span class="reserved">class</span> StaticSample
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(MyMath.Sin(1));
  }
}
</code></pre>


標準ライブラリの <code>Math.Sin</code> や <code>Console.Write</code> などは静的メソッドです。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="reserved">var</span> pi = 2 * <span class="type">Math</span>.Asin(1); <span class="comment">// 静的クラス Math の静的メソッド Asin を参照</span>
        <span class="type">Console</span>.WriteLine(<span class="type">Math</span>.PI == pi); <span class="comment">// 静的クラス Math の定数 PI を参照</span>
    }
}
</code></pre>



##### <a id="sec-generated-title-4"></a>補足: 関数
オブジェクト指向ではない（クラス的な概念を持たない）プログラミング言語でいう「関数」に一番近いのは、この静的メソッドです。

ちなみに、「[関数メンバー](../structured/st_function.md#function-member)」で説明していますが、
C# には「関数」的な動作をするメンバーとして、コンストラクター、プロパティ、インデクサーなどがあって、
これらの総称として「関数メンバー」という呼び方をします。


##<a id="sec-generated-title-5"></a> <a id="ctor"></a>静的コンストラクター
静的フィールドの初期化には、通常のコンストラクターではなく、<strong id="stconst" class="keyword">静的コンストラクター</strong>（static constructor）というものを使います。
静的コンストラクターの定義の仕方は、コンストラクターの前に <code>static</code> キーワードを付ける以外は通常のコンストラクターの定義の仕方と同じです。
例えば、先ほどの <code>Person</code> クラスを例に挙げると以下のようになります。

<pre class="source" title="静的コンストラクターの例" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">string</span> name; <span class="comment">// 名前。インスタンス フィールド。</span>
  <span class="reserved">int</span> age;     <span class="comment">// 年齢。インスタンス フィールド。</span>

  <span class="reserved">static string</span> scientificName; <span class="comment">// 学名。静的フィールド。

  // 通常のコンストラクター</span>
  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }

  <span class="comment">// 静的コンストラクター</span>
  <span class="reserved"><em>static</em></span> Person()
  {
    Person.scientificName = <span class="literal">"Homo sapiens"</span>;
  }
}
</code></pre>


通常のコンストラクターが新しいインスタンスが生成されるたびに呼び出されるのに対して、
静的コンストラクターは1度だけ呼び出されます
（呼び出されるタイミングは、そのクラスの何らかのメンバーに初めてアクセスしたときです）。


##### <a id="sec-generated-title-6"></a>サンプル
<pre class="source" title="静的フィールドのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// 1台ごとに固有のIDが振られるような何らかの製品。</span>
<span class="reserved">class</span> Product
{
  <span class="reserved">static int</span> id_generator;
  <span class="reserved">int</span> id;

  <span class="reserved">static</span> Product()
  {
    <span class="comment">// 最初に1度だけ呼ばれ、id_generator を 0 に初期化。</span>
    id_generator = 0;
  }

  <span class="reserved">public</span> Product()
  {
    <span class="comment">// 新しい製品が製造されるたびに新しい id を振る。</span>
    id = id_generator;
    id_generator++;
  }

  <span class="comment">/// &lt;summary&gt;
  /// その製品のIDを取得する。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public int</span> ID
  {
    <span class="reserved">get</span>{<span class="reserved">return</span> id;}
  }
}

<span class="reserved">class</span> StaticSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;10; i++)
    {
      Product p = <span class="reserved">new</span> Product();

      Console.Write(<span class="literal">"ID: {0}\n"</span>, p.ID);
    }
  }
}
</code></pre>


<pre class="console" title="">
ID: 0
ID: 1
ID: 2
ID: 3
ID: 4
ID: 5
ID: 6
ID: 7
ID: 8
ID: 9
</pre>



##<a id="sec-generated-title-7"></a> <a id="class"></a>静的クラス
<h5 class="version version2">Ver. 2.0</h5>

標準ライブラリ中の <code>Math</code> クラスのように、
静的なメンバーしか持たないクラスがあります。
<code>Math</code> クラスに限らず、
static メンバーのみを持ち、インスタンスの作成が不可能なクラスを作りたいことがしばしばあります。

C# 1.0 では、private なコンストラクタを持つ sealed クラスとしてこのようなクラスを作成していました。
このような方法で、「インスタンスが作成不可能」という制約は満たすことが出来ますが、
非 static なメンバーを定義することができてしまうという問題がありました。
(決してアクセスすることの出来ない無駄なメンバーになってしまいます。)

それに対して、C# 2.0 では、
クラス定義時に <code>static</code> をつけることで、
静的メンバーしか定義できないクラスを作ることが出来ます。
このようなクラスを<strong id="stclass" class="keyword">静的クラス</strong>（static class）と呼びます。

<pre class="source" title="静的クラスの例" lang="">
<code><span class="reserved"><em>static</em> class</span> MyMath
{
  <span class="comment">// double x; というような、非 static な変数・メソッドは定義できない。

  // sin x を求める関数。</span>
  <span class="reserved">public static double</span> Sin(<span class="reserved">double</span> x)
  {
    <span class="reserved">double</span> xx = -x * x;
    <span class="reserved">double</span> fact = 1;
    <span class="reserved">double</span> sin = x;
    <span class="reserved">for</span>(<span class="reserved">int</span> i=2; i&lt;100;)
    {
      fact *= i; ++i; fact *= i; ++i;
      x *= xx;
      sin += x / fact;
    }
    <span class="reserved">return</span> sin;
  }
}
</code></pre>


ちなみに、他のプログラミング言語には、こういう静的メンバーしか定義しない型のことを「モジュール」(module)と呼んでクラスと区別するものもあったりします。
（例えば Visual Basic なんかがそう。）


##<a id="sec-generated-title-8"></a> <a id="extension"></a>拡張メソッド
<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、（本来、前置き記法である）静的メソッドを、
インスタンスメソッドと同様に後置き記法で書くことのできる、
拡張メソッドという機能が追加されました。

すなわち、
今までなら、

<pre class="source" title="静的メソッド" lang="">
<code><span class="reserved">int</span> x = <span class="reserved">int</span>.Parse(<span class="literal">"1"</span>); <span class="comment">// "1" よりも Parse が前</span>
</code></pre>


と書いていたものを、

<pre class="source" title="拡張メソッドの定義" lang="">
<code><span class="reserved">static class</span> Extensions
{
    <span class="reserved">public static int</span> Parse(<span class="reserved">this string</span> str)
    {
        <span class="reserved">return int</span>.Parse(str);
    }
}
</code></pre>


というような静的メソッドを用意することで、
以下のような構文で呼び出せるようになります。

<pre class="source" title="拡張メソッドの利用" lang="">
<code><span class="reserved">int</span> x = <span class="literal">"1"</span>.Parse(); <span class="comment">// Parse が後に</span>
</code></pre>


詳細は「[拡張メソッド](../functional/sp3_extension.md)」で説明します。


##<a id="sec-generated-title-9"></a> <a id="using-static"></a>using static
<h5 class="version version6">Ver. 6</h5>

名前空間(参考: 「[名前空間](../structured/sp_namespace.md)」)に対しては、「[using ディレクティブ](../structured/sp_namespace.md#using)」を使うことで、
利用側で長い名前空間を省略して書けるようになります。

C# 6 で、これと同じようなことが、静的メソッドに対してもできるようになりました。
<strong id="key-using-static" class="keyword">using static</strong> ディレクティブを書くことで、クラス名を省略して、直接静的メソッドを呼べるようになります。
例えば、Math クラス(System 名前空間)中のメソッド呼び出しであれば、以下のように書けます。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;
<em><span class="reserved">using static</span> System.<span class="type">Math</span></em>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="reserved">var</span> pi = 2 * <em>Asin(1)</em>;
        <span class="type">Console</span>.WriteLine(<em>PI</em> == pi);
    }
}
</code></pre>

ちなみに、using static は任意のクラスに対して使えます(静的クラスでないとダメとかの制限はありません)。
たとえば以下の例では、`TimeSpan`構造体や`Task`クラスを using static していますが、これらは static 修飾子がついていない普通のクラスです。

<pre class="source" title="static 修飾子がつかないクラスを using static">
<code><reserved></span><span class="reserved">using</span> System.Threading.Tasks;
<span class="reserved">using</span> <span class="reserved">static</span> System.Threading.Tasks.<span class="type">Task</span>;
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">TimeSpan</span>;

<span class="reserved">class</span> <span class="type">UsingStaticNormalClass</span>
{
    <span class="reserved">public</span> <span class="reserved">async</span> <span class="type">Task</span> XAsync()
    {
        <span class="comment">// TimeSpan.FromSeconds</span>
        <span class="reserved">var</span> sec = FromSeconds(1);

        <span class="comment">// Task.Delay</span>
        <span class="reserved">await</span> Delay(sec);
    }
}
</code></pre>

###<a id="sec-generated-title-10"></a> <a id="using-static-enum"></a>using staticと列挙型
列挙型のメンバーも静的なので、using staticを使って、型名を省略して参照できます。

<pre class="source" title="using static と列挙型">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> <span class="type">Color</span>;

<span class="reserved">class</span> <span class="type">UsingStaticEnum</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> X()
    {
        <span class="comment">// enum のメンバーも using static で参照できる</span>
        <span class="reserved">var</span> cyan = Blue | Green;
        <span class="reserved">var</span> purple = Red | Blue;
        <span class="reserved">var</span> yellow = Red | Green;
    }
}

<span class="reserved">enum</span> <span class="type">Color</span>
{
    Red = 1,
    Green = 2,
    Blue = 4,
}
</code></pre>

###<a id="sec-generated-title-11"></a> <a id="using-static-extensions"></a>using staticと拡張メソッド
using static を使う場合でも、そのクラス中の[拡張メソッド](../functional/sp3_extension.md)はあくまで拡張メソッドとしてだけ使えます。
using static だけでは、拡張メソッドを普通の静的メソッドと同じ呼び方で呼べません。

<pre class="source" title="拡張メソッドと using static">
<code><reserved></span><span class="reserved">using</span> <span class="reserved">static</span> System.Linq.<span class="type">Enumerable</span>;

<span class="reserved">class</span> <span class="type">UsingStaticSample</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> X()
    {
        <span class="comment">// 普通の静的メソッド</span>
        <span class="comment">// Enumerable.Range が呼ばれる</span>
        <span class="reserved">var</span> input = Range(0, 10);

        <span class="comment">// 拡張メソッド</span>
        <span class="comment">// Enumerable.Select が呼ばれる</span>
        <span class="reserved">var</span> output1 = input.Select(x =&gt; x * x);

        <span class="comment">// 拡張メソッドを普通の静的メソッドとして呼ぼうとすると</span>
        <span class="comment">// コンパイル エラー</span>
        <span class="reserved">var</span> output2 = Select(input, x =&gt; x * x);
    }
}
</code></pre>

### <a id="sec-generated-title-12"></a>補足: 名前空間の using と違う理由
ちなみに、名前空間の using ディレクティブと区別を付けるために、「using static クラス名;」というように、static キーワードが必要です。
名前空間の using と静的クラスの using の区別がつかないと結構ひどいコードが書けてしまう問題があったので、この文法に落ち着きました。
static キーワードを付けなくてよい場合、以下のように、名前空間と同名のクラスを後から足すことで、既存のコードを壊せます。

<pre class="source" title="using クラス名; ではなく、using static クラス名; な理由" lang="">
<code><span class="comment">// 正式な C# 6 ではコンパイルできない
// プレビュー版のころにコンパイルできて問題になったコード</span>

<span class="reserved">using</span> System;
<span class="reserved">using</span> System.<span class="type">Linq</span>;

<span class="comment">// ↑ 静的クラスの方の Linq が参照される。
// 本来の LINQ (System.Linq.Enumerable クラス内の拡張メソッド)は呼べなくなるわ、
// nameof の意味が下記の Linq クラスの nameof 静的メソッドで上書きされてしまうわ、
// ろくなことにならない。</span>

<span class="reserved">public class</span> <span class="type">Program</span>
{
    <span class="reserved">public static void</span> Main()
    {
        <span class="reserved">var</span> name = <span class="reserved">nameof</span>(Main); <span class="comment">// 下記の System.Linq クラスの nameof 静的メソッドが呼ばれる。</span>
        Console.WriteLine(name);
    }
}

<span class="reserved">namespace</span> System
{
    <span class="reserved">public static class</span> <span class="type">Linq</span>
    {
        <span class="reserved">public static string</span> nameof(Action x) =&gt; <span class="literal">""</span>;
    }
}
</code></pre>


nameof も C# 6 で追加された新文法です。詳しくは「[特殊な文字列リテラル](../start/st_string.md)」を参照。
## <a id="exercise"></a>演習問題

### <a id="exercise-static1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体に、
2点間の距離を求める static メソッド <code>GetDistance</code> を追加せよ。

<pre class="source" title="GetDistance" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// A-B 間の距離を求める。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;点A&lt;/param&gt;
/// &lt;param name="b"&gt;点B&lt;/param&gt;
/// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
<span class="reserved">public static double</span> GetDistance(Point a, Point b)
</code></pre>


また、<code>GetDistance</code> を用いて、
<code>Triangle</code> クラスに三角形の周を求めるメソッド
<code>GetPerimeter</code> を追加せよ。

<pre class="source" title="GetPerimeter" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 三角形の周の長さを求める。
/// &lt;/summary&gt;
/// &lt;returns&gt;周&lt;/returns&gt;</span>
<span class="reserved">public double</span> GetPerimeter()
</code></pre>



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
    Console.Write(<span class="literal">"{0}\n"</span>, t.GetPerimeter());
  }
}
</code></pre>
