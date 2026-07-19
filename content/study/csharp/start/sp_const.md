---
title: "定数"
source_url: "https://ufcpp.net/study/csharp/start/sp_const/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-09-18T00:00:00"
tags: []
umbraco_id: 1214
parent_id: 1190
sort_order: 14
aliases:
  - "/csharp/sp_const"
  - "/csharp/sp_const.html"
  - "/csharp/start/sp_const/"
  - "/study/csharp/sp_const"
  - "/study/csharp/sp_const.html"
---

# 定数

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
全く変化しない値を、異なる場所で何度も使いたい事があります。
このような場合、「[リテラル](st_variable.md#literal)」を何箇所にも分散させて書くのではなく、<code>const</code> というキーワードを用いて定義した定数を使うべきです。


##### <a id="sec-generated-title-2"></a>ポイント
* const キーワードを使って、定数（値が絶対に変わらない / 変えれない変数）を定義できます。
    * 見た目は変数と同じように使えますが、値の変更はできません。

    * コンパイル結果は「[リテラル](st_variable.md#literal)」を直接書いた場合と同様になります。



* 定数は、宣言時に値をリテラルで初期化できるものにしか使えません。（new できない。）

* より柔軟な初期化ができる readonly （読み取り専用）変数というものもあります。



##<a id="sec-generated-title-3"></a> <a id="constant"></a>変化しない値
例えば、以下のようなコードを見てください。

<pre class="source" title="リテラルをちりばめたコード" lang="">
<code><span class="reserved">int</span>[] array = <span class="reserved">new int</span>[<em>5</em>];

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;<em>5</em>; ++i)
  array[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());

<span class="reserved">int</span> sum = 0;
<span class="reserved">int</span> sq_sum = 0;

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;5; ++i)
{
  <span class="reserved">int</span> n = array[i];
  sum += n;
  sq_sum += n*n;
}

<span class="reserved">double</span> mean = sum / <em>5</em>;
<span class="reserved">double</span> var  = sq_sum / <em>5</em> - mean*mean;

Console.Write(<span class="literal">"平均: {0}\n分散: {1}\n"</span>, mean, var);
</code></pre>


値を5つ入力してもらって、その平均と分散を求めるものです。
5 という「[リテラル](st_variable.md#literal)」が4箇所出てきていますね。

さて、ではここで、データの数を5つではなくて6つに変更することになったとします。
5 というリテラルを全部 6 に置き換える必要があるわけですが、
たった4つでも結構面倒です。
まして、もっと数が多かったことを考えてみましょう。
数が増えるにつれて、忘れず全部修正するのが困難になります。

なので、普通はリテラルを直接使うということはしません。
例えば、以下のように、5 と言う値を一度変数に代入して使うことを考えます。

<pre class="source" title="一度変数に代入" lang="">
<code><em><span class="reserved">int</span> NUM = 5;</em>
<span class="reserved">int</span>[] array = <span class="reserved">new int</span>[NUM];

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;NUM; ++i)
  array[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());

<span class="reserved">int</span> sum = 0;
<span class="reserved">int</span> sq_sum = 0;

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;NUM; ++i)
{
  <span class="reserved">int</span> n = array[i];
  sum += n;
  sq_sum += n*n;
}

<span class="reserved">double</span> mean = sum / (<span class="reserved">double</span>)NUM;
<span class="reserved">double</span> var  = sq_sum / (<span class="reserved">double</span>)NUM - mean*mean;

Console.Write(<span class="literal">"平均: {0}\n分散: {1}\n"</span>, mean, var);
</code></pre>


これで、もしデータの個数を変更する必要が生じても、
<code>int NUM = 5;</code> の1行だけの修正で解決します。


##<a id="sec-generated-title-4"></a> <a id="const"></a>const
ところが、このコードにもちょっとだけ問題があります。
1つは、定数なのか、途中で値が変わるものなのかが分からないことです。
変数なので、途中で値が書き換えられてしまってもエラーにはなりません。
また、ソースファイルの見易さの観点からも、
定数は定数であることが一目で分かる方が好ましいです。

2つ目の問題は、効率面にあります。
書き換える必要のある変数よりも、
その必要のない定数の方が、プログラムの実行効率が高くなります。
したがって、上述のような方法（一度変数に値を格納）すると、
多少ですが実行効率が悪くなるという欠点があります。

<em>
        そこで、C# では、const というキーワードを用いることで、
        変数のように扱える定数を定義することが出来ます。
      </em>
通常、<strong id="constant" class="keyword">定数</strong>（constant）とだけいうと、
「[リテラル](st_variable.md#literal)」ではなく、
こちらのことを指します。
（リテラルの方は直定数と訳す。）

<pre class="source" title="定数定義" lang="">
<code><span class="reserved"><em>const</em> int</span> NUM = 5;
<span class="reserved">int</span>[] array = <span class="reserved">new int</span>[NUM];

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;NUM; ++i)
  array[i] = <span class="reserved">int</span>.Parse(Console.ReadLine());

<span class="reserved">int</span> sum = 0;
<span class="reserved">int</span> sq_sum = 0;

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;NUM; ++i)
{
  <span class="reserved">int</span> n = array[i];
  sum += n;
  sq_sum += n*n;
}

<span class="reserved">double</span> mean = sum / (<span class="reserved">double</span>)NUM;
<span class="reserved">double</span> var  = sq_sum / (<span class="reserved">double</span>)NUM - mean*mean;

Console.Write(<span class="literal">"平均: {0}\n分散: {1}\n"</span>, mean, var);
</code></pre>


const を付けて宣言された定数は、宣言文中における初期化時にのみ値を代入できます。
定数というくらいですから、当然、
その他の場所で値を書き換えることは出来ません。

<pre class="source" title="定数に値を代入（エラー）" lang="">
<code><span class="reserved">const int</span> NUM = 5; <span class="comment">// 宣言時の初期化のみ可能</span>
NUM = 6; <span class="comment">// ここでエラーになる</span>
</code></pre>


また、const を付けた定数を用いたソースコードは、
リテラルを使ったソースコードと同等のコンパイル結果になります。
従って、リテラルの直書きと比べて効率が落ちることはありません。

ただし、const キーワードは、int などの数値型、string 型、または列挙型に対してのみ使用できます。
（あと、値が null 限定で参照型にも使える。）
インスタンスを new キーワードで生成するようなものには const キーワードは使えません。


##<a id="sec-generated-title-5"></a> <a id="const_member"></a>const メンバー
const を使った定数は、
メソッド中（ローカル）だけでなく、
クラスのメンバーにする事も出来ます。
（「[クラス](../oop/oo_class.md#class)」に関しては別章参照: 「[クラス](../oop/oo_class.md)」。）

<pre class="source" title="クラスの const メンバー" lang="">
<code><span class="reserved">class</span> Math
{
  <span class="reserved">public const double</span> PI = 3.1415926535897932;
}
</code></pre>


const メンバーはクラスに属します。
（「[静的メンバー](../oop/oo_static.md#static-member)」と同じ扱い。
<code>ClassName.Member</code> という形式で参照。）
例えば、上述の例、<code>PI</code> の場合、<code>Math.PI</code> という形式で参照します。


###<a id="sec-generated-title-6"></a> <a id="versioning"></a>const のバージョニング問題
ちなみに、private な場合は const メンバー変数で問題ないのですが、
public にする場合にはあまり const メンバー変数は使わない方がいいです。

単一のプロジェクトで使っている分には何の問題もありませんが、
もし、ライブラリで定数を定義して、別プロジェクトのアプリから参照するような場合には注意が必要です。
定数は、コンパイル時にリテラルと全く同じように値が展開されてしまうため、
定数を定義しているライブラリの方だけでなく、参照しているアプリ側も再コンパイルしないと、値の変化が反映されません。

このような問題を、バージョン アップしたときの挙動が怪しくなるという意味で、
バージョニング問題（versioning problem）と呼びます。

したがって、
数学の定数である π のように、
値が変わること自体まずありえないような定数なら全然問題ないのですが、
もしも変更がありうる場合には、
たとえ定数であっても static な「[プロパティ](../oop/oo_property.md#property)」にしておく方がいいです。
でないと、値が変わったときに、利用側でも再コンパイルが必要になってしまいます。


##<a id="sec-generated-title-7"></a> <a id="readonly"></a>readonly
クラスのメンバーに対しては、
const 以外に、もう1つ定数のようなものを実現する方法があります。
readonly というキーワードを用いて、<strong id="ro" class="keyword">読取り専用</strong>（read only）の変数を定義できます。
const との違いは以下のようになります。

<table summary="">

	<tr>
		<th>const</th>
		<th>readonly</th>
	</tr>
	<tr>
		<td markdown="1">ローカル変数にも使える</td>
		<td markdown="1">クラスのメンバー変数のみ。</td>
	</tr>
	<tr>
		<td markdown="1">常に静的変数と同じ扱い。</td>
		<td markdown="1">static の有無を変えられる。</td>
	</tr>
	<tr>
		<td markdown="1">宣言時にのみ初期化可能。</td>
		<td markdown="1">コンストラクタ内で値を書き換え可能。</td>
	</tr>
	<tr>
		<td markdown="1">コンパイル結果はリテラルと同等。</td>
		<td markdown="1">コンパイル結果は変数と同等。</td>
	</tr>
	<tr>
		<td markdown="1">インスタンスを new で生成するようなものには使えない。</td>
		<td markdown="1">new 可能。</td>
	</tr>
</table>


<pre class="source" title="readonly メンバー" lang="">
<code><span class="reserved">class</span> A
{
  <em><span class="reserved">readonly int</span> num;</em>

  <span class="reserved">public</span> A(<span class="reserved">int</span> num)
  {
    <span class="reserved">this</span>.num = num; <span class="comment">// コンストラクタ内では書き換え可能。</span>
  }

  <span class="reserved">public void</span> Method(<span class="reserved">int</span> num)
  {
    <span class="reserved">int</span> x = <span class="reserved">this</span>.num; <span class="comment">// 読み取りは可能。</span>
    <span class="reserved">this</span>.num = num;   <span class="comment">// 書き込み不可。エラー！</span>
  }
}
</code></pre>

###<a id="sec-generated-title-8"></a> <a id="prefer-readonly"></a>注意: const の問題とreadonlyやプロパティ
ちなみに、const を使った定数は、
（コンパイル結果がリテラルを使った結果と同じく）
プログラム中に直接値が埋め込まれてしまうため、
値を変更した際には、参照側（クラス利用側）のコードも再コンパイルする必要が生じます。
なので、<code>Math.PI</code>（数学定数π）のように、本当に不変で、
絶対に変わることのない値以外は public const なメンバー変数にすべきではありません。
（private なものや、ローカル変数に対する const は OK。）

その代替として、`readonly`なフィールドや、[get-onlyなプロパティ](../oop/oo_property.md#get-only)が使えます。

<pre class="source" title="const の代替としての readonly">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// const はあんまり public にしたくない</span>
    <span class="comment">// 今後絶対に値を変更しないという自信がない限りは使わない方がいい</span>
    <span class="reserved">public</span> <span class="reserved">const</span> <span class="reserved">int</span> X = 1;

    <span class="comment">// readonly フィールドや、get-only プロパティ越しに公開することを推奨</span>
    <span class="comment">// (プロパティの方がより推奨)</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Y = 1;
    <span class="reserved">public</span> <span class="reserved">int</span> Z =&gt; 1;
}
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="struct-class-readonly"></a>readonly の注意点
現時点では触れられませんが、クラスや構造体を説明した後に、改めて`readonly`に関する注意点があります。
詳細は「[readonly の注意点](../resource/readonlyness.md)」を参照してください。

##<a id="sec-generated-title-10"></a> <a id="constant-expressions"></a>定数にできるもの
C# で `const` を使った定数にできるものには以下のようなものがあります。

* [リテラル](st_embeddedtype.md#literal)
* [列挙型](../structured/st_enum.md)の値
* 他の `const` メンバー
* 上記同士の[式](st_variable.md#expression)(四則演算とか)

例えば、以下のように複数のリテラル、定数を組み合わせたものも再び定数にできます。

<pre class="source" title="const にできるもの">
<code><span class="comment">// enum</span>
<span class="reserved">const</span> <span class="type">DayOfWeek</span> D1 = <span class="type">DayOfWeek</span>.Friday;
<span class="reserved">const</span> <span class="type">DayOfWeek</span> D2 = <span class="type">DayOfWeek</span>.Wednesday;

<span class="comment">// int のリテラル同士の掛け算。</span>
<span class="reserved">const</span> <span class="reserved">int</span> I = 3 * 5;

<span class="comment">// 他の定数を参照。</span>
<span class="reserved">const</span> <span class="reserved">int</span> J = 2 * I;

<span class="comment">// 参照しているものが全部定数なら多少複雑な式でも OK。</span>
<span class="reserved">const</span> <span class="reserved">double</span> X = (<span class="reserved">int</span>)(J % 2 == 0 ? D1 : D2) * 1.25;

<span class="comment">// 文字列も const にできる。</span>
<span class="reserved">const</span> <span class="reserved">string</span> S = <span class="string">"abc"</span> + <span class="string">"def"</span>;

<span class="comment">// その他の型では、null だけ const にできる。</span>
<span class="reserved">const</span> <span class="reserved">object</span> N = <span class="reserved">null</span>;
</code></pre>

ちなみに、特殊なものでは [`nameof` 演算子](st_string.md#nameof-operator)は完全にリテラルと同列の扱いを受けるので、`const` に使えます。

<pre class="source" title="nameof は const に使える">
<code><span class="reserved">const</span> <span class="reserved">string</span> A = <span class="reserved">nameof</span>(A); <span class="comment">// "A" と同じ結果になる。</span>
<span class="reserved">const</span> <span class="reserved">string</span> B = A + <span class="reserved">nameof</span>(B); <span class="comment">// 他の const + リテラル という扱い。</span>
</code></pre>

###<a id="sec-generated-title-11"></a> <a id="constant-string-interpolation"></a>const 文字列補間
<h5 class="version version10">Ver. 10</h5>

C# 10.0 からは、[文字列補間](st_string.md#string-interpolation)でも、`{}` の中身が `const` 文字列な場合に限り、補完結果も `const` にできます。
例えば以下のような `const` 文字列を作れます。

<pre class="source" title="const 文字列補間">
<code><span class="reserved">const</span> <span class="reserved">string</span> A = <span class="string">"Abc"</span>;
<span class="reserved">const</span> <span class="reserved">string</span> B = <span class="string">"Xyz"</span>;
<span class="reserved">const</span> <span class="reserved">string</span> C = <span class="string">$"</span>{<span class="reserved">nameof</span>(A)}<span class="string">: </span>{A}<span class="string">, </span>{<span class="reserved">nameof</span>(B)}<span class="string">: </span>{B}<span class="string">"</span>; <span class="comment">// "A: Abc, B: Xyz"</span>
</code></pre>

この例のように `nameof` との組み合わせはそれなりに需要があるかと思います。

一方で、`{}` の中身が文字列でない場合、たとえ `const` であっても文字列補間結果は `const` にできなくなります。

<pre class="source" title="補完結果を const 文字列にできない例">
<code><span class="reserved">const</span> <span class="reserved">int</span> A = 1;
<span class="reserved">const</span> <span class="reserved">string</span> C = <span class="error"><span class="string">$"</span>{A}<span class="string">"</span></span>; <span class="comment">// A が文字列じゃないので $"" の結果を const にできない。</span>
</code></pre>

第一印象としては `const` にできてもよさげに見えるんですが…
これは、文字列補間の結果が一定にならないためです。
例えば、以下のように、浮動小数点数を文字列化した結果は国によって異なります。

<pre class="source" title="C# で数値を文字列化した結果は国によって異なる">
<code><span class="reserved">using</span> System.Globalization;

<span class="comment">// 東南アジアの多くの国は . を小数点に使う。</span>
<span class="type">Thread</span>.CurrentThread.CurrentCulture = <span class="reserved">new</span> <span class="type">CultureInfo</span>(<span class="string">"ja-jp"</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(1.234);

<span class="comment">// 大陸ヨーロッパの多くの国は , を小数点に使う。</span>
<span class="type">Thread</span>.CurrentThread.CurrentCulture = <span class="reserved">new</span> <span class="type">CultureInfo</span>(<span class="string">"fr-fr"</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(1.234);
</code></pre>

<pre class="source" title="C# で数値を文字列化した結果は国によって異なる">
<code>1.234
1,234
</code></pre>
