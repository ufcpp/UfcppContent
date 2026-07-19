---
title: "文字列の書式設定"
source_url: "https://ufcpp.net/study/dotnet/bcl/bcl_format/"
content_type: "Article"
published_at: "2012-01-23T00:00:00"
updated_at: "2015-05-06T14:14:08"
tags: []
umbraco_id: 1388
parent_id: 1385
sort_order: 2
aliases:
  - "/dotnet/bcl/bcl_format/"
  - "/dotnet/bcl_format"
  - "/dotnet/bcl_format.html"
  - "/study/dotnet/bcl_format"
  - "/study/dotnet/bcl_format.html"
---

# 文字列の書式設定

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

数値を整形して表示したいことがあります。
例えば、19800 という数値に対して、

* 数字のみ: 19800

* 3ケタごとにコンマ区切り: 19,800

* 指数表記: 1.98e4


など、いろんな表示の仕方があります。

.NET では、ToString メソッドや、string.Format 静的メソッドなどに対して、書式を与えることで、数値の表示の仕方を変えることができます。
また、WPF や Silverlight のデータ バインディングでも、書式設定ができます。

<figure>
	[![データ バインディングにおける書式設定。](../../../../assets/media/ufcpp2000/dotnet/fig/BindingStringFormat.png)](../../../../assets/media/ufcpp2000/dotnet/fig/BindingStringFormat.png)
	<figcaption>データ バインディングにおける書式設定。</figcaption>
</figure>


参考:

* [MSDN: 型の書式設定](http://msdn.microsoft.com/ja-jp/library/26etazsy.aspx)



## <a id="sec-generated-title-2"></a> <a id="ToString"></a>ToString メソッド

C#では、数値などから文字列への型変換は、そのままではできません。しかし、objectクラスがToStringというメソッドを持っていて、これで文字列化できます。

自作の型を文字列化したい場合は、以下のように、ToStringメソッドをオーバーライドします。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>C++</li>
</ul>
<div>

<pre class="source" title="ToString をオーバーライド" lang="C#">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString()
    {
        <span class="reserved">return</span> <span class="literal">"("</span> + X + <span class="literal">", "</span> + Y + <span class="literal">")"</span>;
    }
}
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="reserved">Class</span> <span class="type">Point</span>
    <span class="reserved">Public</span> <span class="reserved">Property</span> X <span class="reserved">As</span> <span class="reserved">Integer</span>
    <span class="reserved">Public</span> <span class="reserved">Property</span> Y <span class="reserved">As</span> <span class="reserved">Integer</span>

    <span class="reserved">Public</span> <span class="reserved">Overrides</span> <span class="reserved">Function</span> ToString() <span class="reserved">As</span> <span class="reserved">String</span>
        <span class="reserved">Return</span> <span class="literal">"("</span> &amp; X &amp; <span class="literal">", "</span> &amp; Y &amp; <span class="literal">")"</span>
    <span class="reserved">End</span> <span class="reserved">Function</span>
<span class="reserved">End</span> <span class="reserved">Class</span>
</code></pre>


</div>
<div>

<pre class="source" title="" lang="C++">
<code><span class="reserved">ref</span> <span class="reserved">class</span> Point
{
<span class="reserved">public</span>:
  <span class="reserved">property</span> <span class="reserved">int</span> X;
  <span class="reserved">property</span> <span class="reserved">int</span> Y;

  <span class="reserved">virtual</span> String^ ToString() <span class="reserved">override</span>
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + X + <span class="literal">", "</span> + Y + <span class="literal">")"</span>;
  }
};
</code></pre>


</div>
</div>


以下のように利用できます。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>C++</li>
</ul>
<div>

<pre class="source" title="" lang="C#">
<code><span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span> { X = 10, Y = 20 };
<span class="type">Console</span>.WriteLine(p);
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="reserved">Dim</span> p = <span class="reserved">New</span> <span class="type">Point</span> <span class="reserved">With</span> {.X = 10, .Y = 20}
<span class="type">Console</span>.WriteLine(p)
</code></pre>


</div>
<div>

<pre class="source" title="" lang="C++">
<code><span class="reserved">auto</span> p = <span class="reserved">gcnew</span> Point();
p-&gt;X = 10;
p-&gt;Y = 20;
Console::WriteLine(p);
</code></pre>


</div>
</div>


<pre class="console" title="">
(10, 20)
</pre>



## <a id="sec-generated-title-3"></a> <a id="ToString-format"></a>書式設定付きの ToString メソッド

intやDateTimeなど、主要な型には、書式設定が可能なバージョンのToStringメソッドが提供されています。書式を、ToStringの引数として渡します。

<pre class="source" title="" lang="">
<code><span class="reserved">var</span> n = 1980;
<span class="type">Console</span>.WriteLine(n.ToString(<span class="literal">"d"</span>)); <span class="comment">// 1980</span>
<span class="type">Console</span>.WriteLine(n.ToString(<span class="literal">"x"</span>)); <span class="comment">// 7bc</span>
 
<span class="reserved">var</span> x = 0.12;
<span class="type">Console</span>.WriteLine(x.ToString(<span class="literal">"f"</span>)); <span class="comment">// 0.12</span>
<span class="type">Console</span>.WriteLine(x.ToString(<span class="literal">"e"</span>)); <span class="comment">// 1.200000e-001</span>
</code></pre>


<code>"d"</code> などが書式です。
書式の書き方については後程改めて説明します。


## <a id="sec-generated-title-4"></a> <a id="string-format"></a>複合書式（string.Format）

stringクラスのFormat静的メソッドで、複数の値をまとめて書式設定することができます。

<pre class="source" title="string.Format 静的メソッド" lang="">
<code><span class="reserved">var</span> x = 7;
<span class="reserved">var</span> y = 13;
<span class="reserved">var</span> line = <span class="reserved">string</span>.Format(<span class="literal">"{0} × {1} = {2}"</span>, x, y, x * y);
<span class="type">Console</span>.WriteLine(line); <span class="comment">// 7 × 13 = 91</span>
</code></pre>


1つ目の引数が書式で、2つ目以降の引数を、それぞれ、<code>{0}</code>、<code>{1}</code>、<code>{2}</code> の部分に展開します。
<code>{}</code> 内の数字は、何番目の引数を参照するかのインデックス（0 始まり）を表します。

Console.Writeや、StreamWriter.Writeなど、内部的にstring.Formatを呼び出してくれる（＝文字列整形の挙動は string.Format と同じ）ものもあります。

<pre class="source" title="Console.WriteLine は string.Format と同じ書式設定ができる" lang="">
<code><span class="type">Console</span>.WriteLine(<span class="literal">"({0}, {1})"</span>, 1, 2); <span class="comment">// (1, 2)</span>
</code></pre>


インデックスに続けて、<code>,</code>（コンマ）で区切って幅を指定することもできます。この時、正の数を指定すると右詰め、負の数を指定すると左詰めになります。

<pre class="source" title="複合書式中での幅指定" lang="">
<code><span class="type">Console</span>.WriteLine(<span class="literal">"({0,-5}) ({1,5})"</span>, 1, 1); <span class="comment">// (1    ) (    1)</span>
</code></pre>


また、インデックスに続けて、<code>:</code> （コロン）で区切って、個別の書式（＝ ToString メソッドに渡す書式）を指定できます。

<pre class="source" title="個別書式指定" lang="">
<code><span class="type">Console</span>.WriteLine(<span class="literal">"{0:x}, {1:c}"</span>, 123, 123); <span class="comment">// 7b, \123</span>
<span class="comment">//↑ "{0}, {1}", 123.ToString("x"), 123.ToString("c") と同じ扱い</span>
</code></pre>


それでは、個別の書式についてみていきましょう。


## <a id="sec-generated-title-5"></a> <a id="num-format-std"></a>数値書式（標準）

##### <a id="sec-generated-title-6"></a>整数

dは10進数、xは16進数を表します。xを大文字にするか小文字にするかで、16進数のa～fの大小を選べます。

<pre class="source" title="標準の整数書式" lang="">
<code><span class="comment">// d：10進数、0詰め桁数指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:d}, {0:d4}"</span>, 5); <span class="comment">// 5, 0005</span>
<span class="comment">// x: 16進数、0詰め桁数指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:x}, {0:X}, {0:x4}, {0:X4}"</span>, 140); <span class="comment">// 8c, 8C, 008c, 008C</span>
</code></pre>



##### <a id="sec-generated-title-7"></a>浮動小数点数

fで固定小数点表示、eで指数表記を表します。また、gで、fとeのどちらか、簡潔な方を自動選択してくれます。

<pre class="source" title="標準の浮動小数点数書式" lang="">
<code><span class="comment">// f: 小数点、小数点以下の桁数指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:f}, {0:f5}"</span>, 0.1234); <span class="comment">// 0.12, 0.12340</span>
<span class="comment">// e: 指数表記、精度指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:e}, {0:e2}, {0:E2}"</span>, 0.1234); <span class="comment">// 1.234000e-001, 1.23e-001, 1.23E-001</span>
<span class="comment">// g: f か e かを自動選択</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:g}, {1:g}"</span>, 1200000000000000.0, 0.12); <span class="comment">// 1.2e+15, 0.12</span>
</code></pre>



##### <a id="sec-generated-title-8"></a>その他

適宜桁区切り、通貨記号などをはさんでくれるn、cや、精度を自動判定してくれるr、パーセント化してくれるpなども利用できます。

<pre class="source" title="その他の数値書式" lang="">
<code><span class="comment">// n: 適宜、桁区切りなどを挿入、小数点以下の桁数指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:n}, {0:n0}"</span>, 1234567); <span class="comment">// 1,234,567.00, 1,234,567</span>
<span class="comment">// c: 通貨</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:c}"</span>, 1234567); <span class="comment">// \1,234,567</span>
<span class="comment">// r: 復元するのに十分な桁数で出力</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:r}"</span>, 0.1234567890123456789f); <span class="comment">// 0.123456791</span>
<span class="comment">// p: パーセント表示、小数点以下の桁数指定</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:p1}"</span>, 0.1234); <span class="comment">// 12.30%</span>
</code></pre>


* 参考:[標準の数値書式指定文字列](http://msdn.microsoft.com/ja-jp/library/dwhawy9k.aspx)



## <a id="sec-generated-title-9"></a> <a id="num-format-custom"></a>数値書式（カスタム）

数値は、0や#（ナンバー記号）などを使って、かなり自由な書式を作れます。

<pre class="source" title="カスタム数値書式" lang="">
<code><span class="comment">// 桁数を明示。0. の 0 は省略</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:#.##}"</span>, 0.2345); <span class="comment">// .23</span>
<span class="comment">// 0詰め4ケタ.4ケタ</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:0000.0000}"</span>, 1.23); <span class="comment">// 0001.2300</span>
<span class="comment">// 3ケタ区切り、小数点以下0詰め2ケタ</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"{0:#,#.00}"</span>, 1234567); <span class="comment">// 1,234,567.00</span>
</code></pre>


* 参考:[カスタム数値書式指定文字列](http://msdn.microsoft.com/ja-jp/library/0c899ak8.aspx)



## <a id="sec-generated-title-10"></a> <a id="datetime-format"></a>日付の書式

DateTime 型、DateTimeOffset 型に対しても、標準書式（<code>"d"</code>など）や、カスタム書式（<code>"y/M/d"</code> など）を設定できます。

<pre class="source" title="標準の日付書式" lang="">
<code><span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">DateTime</span>(2008, 5, 4, 8, 30, 0);
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"d"</span>)); <span class="comment">// 2008/05/04</span>
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"D"</span>)); <span class="comment">// 2008年5月4日</span>
</code></pre>


* 参考:[標準の日付と時刻の書式指定文字列](http://msdn.microsoft.com/ja-jp/library/az4se3k1.aspx)


<pre class="source" title="" lang="">
<code><span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">DateTime</span>(2008, 5, 4, 8, 30, 0);
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"y/M/d h:m:s"</span>)); <span class="comment">// 8/5/4 8:30:0</span>
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"hh:mm:ss"</span>));    <span class="comment">// 08:30:00</span>
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"yy/MM/dd"</span>));    <span class="comment">// 08/05/04 8:30:0</span>
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"yyyy/MM/dd"</span>));  <span class="comment">// 2008/12/04</span>
<span class="type">Console</span>.WriteLine(d.ToString(<span class="literal">"ddd dddd"</span>));    <span class="comment">// 日 日曜日</span>
</code></pre>


* 参考:[カスタムの日付と時刻の書式指定文字列](http://msdn.microsoft.com/ja-jp/library/8kb3ddd4.aspx)


カスタム書式で、 <code>/</code> や <code>:</code> などの記号は自由な位置に挿入できます。
その他、以下の文字は特別な意味を持ちます。

<table summary="">

	<tr>
		<th>記号</th>
		<th>意味</th>
	</tr>
	<tr>
		<td markdown="1">y, yy, yyyy</td>
		<td markdown="1">年。それぞれ、下2桁（2桁目が0なら1ケタ）、下2桁（2桁目は0詰め）、4ケタ表示。</td>
	</tr>
	<tr>
		<td markdown="1">M, MM</td>
		<td markdown="1">月。2文字並べた場合、0を挿入して2ケタにする（以下の、dd などでも同様）。</td>
	</tr>
	<tr>
		<td markdown="1">d, dd</td>
		<td markdown="1">日。</td>
	</tr>
	<tr>
		<td markdown="1">h, hh</td>
		<td markdown="1">時（12時間形式）。</td>
	</tr>
	<tr>
		<td markdown="1">H, HH</td>
		<td markdown="1">時（24時間形式）。</td>
	</tr>
	<tr>
		<td markdown="1">m, mm</td>
		<td markdown="1">分。</td>
	</tr>
	<tr>
		<td markdown="1">s, ss</td>
		<td markdown="1">秒。</td>
	</tr>
	<tr>
		<td markdown="1">f</td>
		<td markdown="1">秒の小数点以下。欲しい桁数分、f を並べる。</td>
	</tr>
	<tr>
		<td markdown="1">ddd, dddd</td>
		<td markdown="1">曜日。ddd が省略名（mon とか 月 とか）、dddd が完全名（Monday とか 月曜日 とか）。</td>
	</tr>
	<tr>
		<td markdown="1">MMM, MMMM</td>
		<td markdown="1">月名。MMM が省略名（Jun とか 1 とか）、MMMM が完全名（Junuary とか 5月 とか）。</td>
	</tr>
	<tr>
		<td markdown="1">t, tt</td>
		<td markdown="1">AM か PM か。日本語カルチャーで t （省略名）を使うと残念なことに（午前でも午後でも「午」と表示）。</td>
	</tr>
	<tr>
		<td markdown="1">g</td>
		<td markdown="1">年号。</td>
	</tr>
	<tr>
		<td markdown="1">K</td>
		<td markdown="1">タイム ゾーン。</td>
	</tr>
</table>



## <a id="sec-generated-title-11"></a> <a id="culture"></a>書式とカルチャー

注意点として、文字列の書式設定の結果は、カルチャーに依存します。

例えば、金額表示（通貨書式 <code>"c"</code> を使う）を考えてみましょう。
世界各国の通販サイトでも覗いていただけるとわかるんですが、以下のような部分が、国によってすべて異なります。

* 小数点以下の有無

* 小数点に使う記号

* 3ケタずつの区切りに使う記号

* 通貨記号

* 負の数の表し方


<code>"c"</code> 書式を使うと、金額に対して、カルチャーごとに最適な整形を掛けてくれます。

<pre class="source" title="カルチャーごとの通貨書式" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Globalization;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> cultures = <span class="reserved">new</span>[] { <span class="literal">"ja-jp"</span>, <span class="literal">"zh-cn"</span>, <span class="literal">"en-us"</span>, <span class="literal">"en-gb"</span>, <span class="literal">"fr-fr"</span>, <span class="literal">"de-de"</span>, <span class="literal">"pt-br"</span>, <span class="literal">"tr-tr"</span>, <span class="literal">"he-il"</span> };
        <span class="reserved">var</span> price = 9800;

        <span class="reserved">foreach</span> (<span class="reserved">var</span> c <span class="reserved">in</span> cultures)
        {
            <span class="reserved">var</span> culture = <span class="reserved">new</span> <span class="type">CultureInfo</span>(c);
            <span class="reserved">var</span> plus = price.ToString(<span class="literal">"c"</span>, culture);
            <span class="reserved">var</span> minus = (-price).ToString(<span class="literal">"c"</span>, culture);
            <span class="type">Console</span>.WriteLine(<span class="literal">"{0,-11} / {1,-12} ({2})"</span>, plus, minus, culture.DisplayName);
        }
    }
}
</code></pre>


<pre class="console" title="実行結果">
¥9,800      / -¥9,800      (日本語 (日本))
￥9,800.00   / ￥-9,800.00   (中国語 (中華人民共和国))
$9,800.00   / ($9,800.00)  (英語 (米国))
£9,800.00   / -£9,800.00   (英語 (英国))
9 800,00 €  / -9 800,00 €  (フランス語 (フランス))
9.800,00 €  / -9.800,00 €  (ドイツ語 (ドイツ))
R$ 9.800,00 / -R$ 9.800,00 (ポルトガル語 (ブラジル))
9.800,00 TL / -9.800,00 TL (トルコ語 (トルコ))
₪ 9,800.00  / ₪-9,800.00   (ヘブライ語 (イスラエル))
</pre>


ちなみに、特にカルチャーを指定しなかった場合、OS 設定のカルチャー（日本語 Windows を使っているなら、デフォルトでは当然日本語）に基づいて整形します。

通貨に限らず、小数点や区切り文字、日付の書式などは文化の影響を受けます。
