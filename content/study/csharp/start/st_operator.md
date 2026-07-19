---
title: "組込み演算子"
source_url: "https://ufcpp.net/study/csharp/start/st_operator/"
content_type: "Article"
published_at: "2015-05-06T14:07:44"
updated_at: "2008-03-09T00:00:00"
tags: []
umbraco_id: 1203
parent_id: 1190
sort_order: 11
aliases:
  - "/csharp/st_operator"
  - "/csharp/st_operator.html"
  - "/csharp/start/st_operator/"
  - "/study/csharp/st_operator"
  - "/study/csharp/st_operator.html"
---

# 組込み演算子

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
＋、－、×、÷のように、
いくつかの変数に対して何らかの処理を加えるもののことを<strong id="operator" class="keyword">演算子</strong>(operator)と呼びます。
また、演算の対象となるもの(x+y の x や y)のことを<strong id="operand" class="keyword">オペランド</strong>(operand: 被演算子)と呼びます。

-x のように、1つのオペランドを必要とする演算子のことを<em>単項演算子</em>(unary operator)、
x+y のように、2つのオペランドを必要とする演算子のことを<em>2項演算子</em>(binary operator)と呼びます。
また、2項演算において、
演算子の左側にあるオペランドのことを<em>左オペランド</em>(left hand side operand)、
演算子の右側にあるオペランドのことを<em>右オペランド</em>(right hand side operand)といいます。

C# では、算術演算や論理演算を行うための演算子が用意されています。


##### <a id="sec-generated-title-2"></a>ポイント
* 演算子: 加減乗除など、数学で出てくるような演算子がいろいろと。

* x = y は、数学と違って代入なので注意。
    * 数学の場合: 「x は y と等しい」あるいは「x と y が等しくなるように値を決める」。

    * プログラミングの場合: 「x に y を代入する」あるいは「y の値を改めて x と書く」。




ちなみに、いくつかの演算子は、組み込み型に対するものだけでなく、
後述する「[クラス](../oop/oo_class.md#class)」や構造体などのユーザー定義型では自作することができます。
（参考: 「[演算子のオーバーロード](../oop/oo_operator.md)」。）


##<a id="sec-generated-title-3"></a> <a id="arithmetic"></a>算術演算子
<code>+, -, *, /</code> を用いて加減乗除算を行えます。
また、<code>+, -</code> を用いて数値の符号を反転することが出来ます。

整数型の除算は余り切り捨てとなります。剰余を求めたい場合は <code>%</code> 演算子を用います。

これらの算術演算子はすべての数値型に対して利用できます。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x + y</code></td>
		<td markdown="1">x と y を足す</td>
		<td markdown="1"><code>
            <span class="reserved">byte</span> a = 11 + 92; <span class="comment">// a は 103 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x - y</code></td>
		<td markdown="1">x から y を引く</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 9 - 4; <span class="comment">// a は 5 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x * y</code></td>
		<td markdown="1">x と y を掛ける</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 3 * 7; <span class="comment">// a は 21 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x / y</code></td>
		<td markdown="1">x を y で割る</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 9 / 2; <span class="comment">// 整数の場合、あまり切り捨て。 a は 4 になる。</span>
          </code><br></br><code>
            <span class="reserved">double</span> x = 9.0 / 2.0; <span class="comment">// a は 4.5 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x % y</code></td>
		<td markdown="1">x を y で割った余り</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 9 % 2; <span class="comment">// a は 1 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>+x</code></td>
		<td markdown="1">x の値そのまま</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = +1; <span class="comment">// a = 1 と同じ</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>-x</code></td>
		<td markdown="1">符号反転</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 1;
          </code><br></br><code>
            <span class="reserved">int</span> b = -a; <span class="comment">// b は -1 になる。</span>
          </code></td>
	</tr>
</table>


注意1：
整数の除算 <code>x / y</code> は、0 に向かって丸められます。
すなわち、<code>x / y</code> の結果が正の場合は切捨て、
負の場合は切上げになります。

注意2：
剰余演算 <code>x % y</code> は、
整数の場合は <code>x - (x / y) * y</code>、
浮動小数点数の場合は <code>x - Math.Truncate(x / y) * y</code> と同じ値になります。


##<a id="sec-generated-title-4"></a> <a id="inc"></a>インクリメント・デクリメント
<code>++, --</code> を用いてインクリメント、デクリメント演算を行うことが出来ます。
インクリメント演算を行うとオペランドは 1 ずつ増加（<code>++x</code> は <code>x = x + 1</code> と同じ）し、
デクリメント演算を行うとオペランドは 1 ずつ減少（<code>--x</code> は <code>x = x - 1</code> と同じ）します。

インクリメント・デクリメント演算には前置き(<code>++x</code>という形式)と後置き(<code>x++</code>という形式)があります。
前置き演算の演算結果は、インクリメント・デクリメントが行われた後のオペランドの値になり、
後置き演算の演算結果は、インクリメント・デクリメントが行われる前のオペランドの値になります。

これらの算術演算子はすべての数値型に対して利用できます。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>++x</code></td>
		<td markdown="1">前置きインクリメント</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5;
          </code><br></br><code>
            <span class="reserved">int</span> b = ++a; <span class="comment">// a も b も 6 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x++</code></td>
		<td markdown="1">後置きインクリメント</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5;
          </code><br></br><code>
            <span class="reserved">int</span> b = a++; <span class="comment">// a は 6 に、b は 5 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>--x</code></td>
		<td markdown="1">前置きデクリメント</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5;
          </code><br></br><code>
            <span class="reserved">int</span> b = --a; <span class="comment">// a も b も 4 になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x--</code></td>
		<td markdown="1">後置きデクリメント</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5;
          </code><br></br><code>
            <span class="reserved">int</span> b = a--; <span class="comment">// a は 4 に、b は 5 になる。</span>
          </code></td>
	</tr>
</table>



##<a id="sec-generated-title-5"></a> <a id="shift"></a>シフト
<code>&lt;&lt;</code> は左シフトを、
<code>&gt;&gt;</code> は右シフトを行う演算子です。

シフト演算子は左側のオペランドを右側のオペランド分だけ左または右にシフトします。
左オペランドには <code>int, uint, long, ulong</code> のみを、
右オペランドには <code>int</code> のみを取ることが出来ます。

左オペランドが符号付き整数の場合、右シフトは算術シフト演算になり、
符号無し整数の場合、右シフトは論理シフト演算になります。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x&lt;&lt;i</code></td>
		<td markdown="1">左シフト</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 51 &lt;&lt; 2 ; <span class="comment">// a は 204 になる。</span>
          </code><br></br>(0011 0011 &lt;&lt; 2 = 1100 1100)</td>
	</tr>
	<tr>
		<td markdown="1"><code>x&gt;&gt;i</code></td>
		<td markdown="1">右シフト</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 51 &gt;&gt; 1 ; <span class="comment">// a は 25 になる。</span>
          </code><br></br>(0011 0011 &gt;&gt; 1 = 0001 1001)</td>
	</tr>
</table>

###<a id="sec-generated-title-6"></a> <a id="unsigned-right-shift"></a>符号なし右シフト
<h5 class="version version11*">Ver. 11</h5>

C# では長らく、

* 符号<em>付き</em>整数の右シフトは符号<em>付き</em>右シフト(算術シフト)
* 符号<em>なし</em>整数の右シフトは符号<em>なし</em>右シフト(論理シフト)

という方式で右シフトの方式を切り替えていました。

これに対して、C# 11 では、`>>>` という演算子で「型によらず常に符号なし右シフト」ができるようになりました。

詳しくは「[【Generic Math】 C# 11 での演算子の新機能](../oop/generic-math-operators.md#unsigned-right-shift)」で説明します。

##<a id="sec-generated-title-7"></a> <a id="concat"></a>文字列連結
文字列に対して <code>+</code> 演算子を用いることで文字列の連結を行えます。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x+y</code></td>
		<td markdown="1">文字列連結</td>
		<td markdown="1"><code>
            <span class="reserved">string</span> s = <span class="string">"abc"</span> + <span class="string">"def"</span>; <span class="comment">// s は "abcdef" になる。</span>
          </code></td>
	</tr>
</table>



##<a id="sec-generated-title-8"></a> <a id="logical"></a>論理演算子
AND, OR, XOR などの論理演算を行います。

<code>&amp;, |, ^</code> はそれぞれ AND, OR, XOR を行う演算子です。
これらの演算子は整数型および <code>bool</code> 型に対して利用できます。
整数型に対してこれらの演算子を用いた場合、ビットごとの論理演算を行います。

<code>!</code> は論理否定を行う演算子です。
この演算子は <code>bool</code> 型に対してのみ利用できます。

<code>~</code> はビットごとの補数演算(各ビットの 0/1 を反転する)を行う演算子です。
この演算子は <code>int, uint, long, ulong</code> に対してのみ利用できます。

<code>&amp;&amp;, ||</code> は条件 AND, OR 演算子で、
その演算結果は <code>bool</code> に対する <code>&amp;, |</code> の演算結果と同じものになります。
<code>&amp;, |</code> の演算との違いは、後述する「[短絡評価](#shortcircuit)」を行うかどうかです。
この短絡評価版の論理演算子は <code>bool</code> 型に対してのみ利用できます。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x &amp; y</code></td>
		<td markdown="1">x と y の論理積を計算</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = <span class="reserved">true</span> &amp; <span class="reserved">false</span>; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">byte</span> a = 201 &amp; 92; <span class="comment">// a は 72になる。</span>
          </code><br></br>(1100 1001 AND 0101 1100 = 0100 1000)</td>
	</tr>
	<tr>
		<td markdown="1"><code>x | y</code></td>
		<td markdown="1">x と y の論理和を計算</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = <span class="reserved">true</span> | <span class="reserved">false</span>; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">byte</span> a = 201 | 92; <span class="comment">// a は 221になる。</span>
          </code><br></br>(1100 1001 OR 0101 1100 = 1101 1101)</td>
	</tr>
	<tr>
		<td markdown="1"><code>x ^ y</code></td>
		<td markdown="1">x と y の排他的論理和を計算</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = <span class="reserved">true</span> ^ <span class="reserved">true</span>; <span class="comment">// a は falseになる。</span>
          </code><br></br><code>
            <span class="reserved">byte</span> a = 201 ^ 92; <span class="comment">// a は 149になる。</span>
          </code><br></br>(1100 1001 XOR 0101 1100 = 1001 0101)</td>
	</tr>
	<tr>
		<td markdown="1"><code>!x</code></td>
		<td markdown="1">x の論理否定を計算</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = !<span class="reserved">true</span>; <span class="comment">// a は false になる。</span>
          </code><br></br></td>
	</tr>
	<tr>
		<td markdown="1"><code>~x</code></td>
		<td markdown="1">x の補数を計算</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = ~201; <span class="comment">// a は -202 になる。</span>
          </code><br></br>~(0000 0000 1100 1001) = 1111 1111 0011 0110</td>
	</tr>
</table>



##<a id="sec-generated-title-9"></a> <a id="relation"></a>関係演算
<code>==, !=</code> を用いてオペランドの等値性を判断できます。
<code>==</code> は2つのオペランドが等しければ <code>true</code> を、
<code>!=</code> は2つのオペランドが等しくなければ <code>true</code> を返します。
これらの演算子はすべての組込み型に対して利用でき、
数値型の場合はその値の比較、文字列型の場合はすべての文字が一致しているかどうか、
オブジェクト型の場合はオブジェクトの参照先が同じかどうかを調べます。

<code>&lt;, &gt;, &lt;=, &gt;=</code> はオペランドの大小比較を行います。
これらの演算子は数値型に対して利用できます。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x == y</code></td>
		<td markdown="1">x が y と等しいかどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = "abc" == "abc"; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 == 0; <span class="comment">// a は false になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x != y</code></td>
		<td markdown="1">x が y と異なるかどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = "abc" != "abc"; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 != 0; <span class="comment">// a は true になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x &lt; y</code></td>
		<td markdown="1">x が y より小さいかどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = 1 &lt; 2; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &lt; 1; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &lt; 0; <span class="comment">// a は false になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x &gt; y</code></td>
		<td markdown="1">x が y より大さいかどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = 1 &gt; 2; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &gt; 1; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &gt; 0; <span class="comment">// a は true になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x &lt;= y</code></td>
		<td markdown="1">x が y 以下かどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = 1 &lt;= 2; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &lt;= 1; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &lt;= 0; <span class="comment">// a は false になる。</span>
          </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>x &gt;= y</code></td>
		<td markdown="1">x が y 以上かどうか</td>
		<td markdown="1"><code>
            <span class="reserved">bool</span> a = 1 &gt;= 2; <span class="comment">// a は false になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &gt;= 1; <span class="comment">// a は true になる。</span>
          </code><br></br><code>
            <span class="reserved">bool</span> a = 1 &gt;= 0; <span class="comment">// a は true になる。</span>
          </code></td>
	</tr>
</table>



##<a id="sec-generated-title-10"></a> <a id="substitute"></a>代入演算
`=` を用いて代入を行えます。<code>=</code> は、右オペランドの値を左オペランドに代入します。

（
数学の場合、<code>=</code> は比較演算（C# の場合、<code>==</code> 演算子を使う）ですが、
C# では代入になります。
）

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x = y</code></td>
		<td markdown="1">x に y を代入します</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5; <span class="comment">// a は 5 になる。</span>
          </code></td>
	</tr>
</table>

###<a id="sec-generated-title-11"></a> <a id="compound-assignment"></a>複合代入演算子
<code>+=, -=, *=, /=, %=, &amp;=, |=, ^=, &lt;&lt;=, &gt;&gt;=</code> など、
2項演算子の後ろに `=` を付けることで複合代入(compound assignment)というものになります。

例えば <code>a += b;</code> であれば <code>a = a + b;</code> と同じ意味になります。
<code>-=, *=, /=, %=, &amp;=, |=, ^=, &lt;&lt;=, &gt;&gt;=</code> も同様です。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x += y</code></td>
		<td markdown="1"><code>x = x + y</code>と同じ結果が得られる</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = 5;
          </code><br></br><code>
            a += 10; <span class="comment">// a は 15 になる。</span>
          </code></td>
	</tr>
</table>

####<a id="sec-generated-title-12"></a> <a id="null-coalescing-assignment"></a>null 合体代入
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、[null合体演算子](../resource/sp2_nullable.md#coalescing) (`??`)も複合代入に使えるようになりました(`??=`)。

例えば以下のような書き方ができます。

<pre class="source" title="null 合体代入">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">string</span> s = <span class="reserved">null</span>)
{
    s <em>??=</em> <span class="string">"default string"</span>;
    <span class="type">Console</span>.WriteLine(s);
}
</code></pre>

意味としては、`if (s == null) s = ...;` と同じになります。


##<a id="sec-generated-title-13"></a> <a id="condition"></a>条件演算子
<code>?:</code> 演算子、は C# における唯一の3項演算子(trinary operator: オペランドが3つの演算子)で、
1つ目のオペランドの結果に応じて2つ目か3つ目のどちらかのオペランドの値を返します。
例えば、<code>cond ? x : y;</code> は
<code>cond</code> が <code>true</code> ならば <code>x</code> を、
<code>cond</code> が <code>false</code> ならば <code>y</code> を返します。

1つ目のオペランドは <code>bool</code> 型でなければなりません。
また、2つ目と3つ目のオペランドにはすべての型を利用できますが、
両方が同じ型である必要があります。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>意味</th>
		<th>例</th>
	</tr>
	<tr>
		<td markdown="1"><code>c ? x : y</code></td>
		<td markdown="1">c が true ならば x を、<br></br>false ならば y を返します</td>
		<td markdown="1"><code>
            <span class="reserved">int</span> a = (x &gt; 5) ? 10 : 0;
          </code><br></br><code>
            <span class="comment">// a は、x が 5 より大きければ 10 に、</span>
          </code><br></br><code>
            <span class="comment">// さもなくば 0 になります。</span>
          </code></td>
	</tr>
</table>



##<a id="sec-generated-title-14"></a> <a id="null"></a>null 合体演算子
<h5 class="version version2">Ver. 2.0</h5>

無効な値（null）を許容する型（「[参照型](../resource/oo_reference.md#reftype)」 もしくは 「[Nullable 型](../resource/sp2_nullable.md#nullableType)」）に対して、
「値が無効な時、デフォルト値を代入しなおしたい」ということが多々あります。
すなわち、以下のような条件演算を結構よく利用します。

<pre class="source" title="" lang="">
<code><span class="reserved">string</span> str = <span class="reserved">null</span>;

<span class="reserved">string</span> nonNullStr = str != <span class="reserved">null</span> ? str : <span class="literal">"default string"</span>;
</code></pre>


そこで、C# 2.0 では、この条件演算に相当する処理を簡潔に書くために、null 合体演算子（null coalescing operator） ?? というものが導入されました。
上記の例は、以下の書くことができます。

<pre class="source" title="" lang="">
<code><span class="reserved">string</span> str = <span class="reserved">null</span>;

<span class="reserved">string</span> nonNullStr = str ?? <span class="literal">"default string"</span>;
</code></pre>



##<a id="sec-generated-title-15"></a> <a id="sizeof"></a>sizeof 演算子
sizeof 演算子は、他の演算子と比べると少し特殊で、
以下のように、型に対して用います。

<pre class="source" title="sizeof 演算子" lang="">
<code>Console.Write(<span class="literal">"{0}, {1}\n"</span>, <span class="reserved">sizeof</span>(<span class="reserved">int</span>), <span class="reserved">sizeof</span>(<span class="reserved">byte</span>));
</code></pre>


<pre class="console" title="sizeof 演算子">
4, 1
</pre>


sizeof 演算子は、その型が何バイトのメモリを占めるかを返します。
int（32ビット整数）なら4バイト、
byte（8ビット符号なし整数）なら1バイトなので、
sizeof(int), sizeof(byte) はそれぞれ 4, 1 を返します。

通常、sizeof 演算子の引数として与えられる型は、int や byte など、C# の規格上、サイズが決まっている数値型のみです。
（unsafe コードと呼ばれる特殊な状況下でのみ、もう少し広い範囲の型のサイズを取得できます。参考: 「[unsafe](../interop/sp_unsafe.md)」。）


##<a id="sec-generated-title-16"></a> <a id="short-circuit"></a>短絡評価
条件 AND <code>&amp;&amp;</code>、
条件 OR <code>||</code>、
条件演算子 <code>?:</code>、および、null 合体演算子 <code>??</code> は
<strong id="shortcircuit" class="keyword">短絡評価</strong>（short circuit evaluation）と呼ばれる挙動をします。

短絡評価は、左辺の結果によっては右辺が評価されない（関数などを呼ぼうとしても呼ばれない）というものです。

<table summary="">

	<tr>
		<th>演算子</th>
		<th>挙動</th>
	</tr>
	<tr>
		<td markdown="1">条件 AND<code>&amp;&amp;</code></td>
		<td markdown="1">左オペランドが<code>false</code>の場合、右オペランドは評価されません。</td>
	</tr>
	<tr>
		<td markdown="1">条件 OR<code>||</code></td>
		<td markdown="1">左オペランドが<code>true</code>の場合、右オペランドは評価されません。</td>
	</tr>
	<tr>
		<td markdown="1">条件演算子<code>?:</code></td>
		<td markdown="1">第1項が<code>true</code>なら第2項のみ、<code>false</code>なら第3項のみが評価されます。</td>
	</tr>
	<tr>
		<td markdown="1">null 合体演算子<code>??</code></td>
		<td markdown="1">左オペランドが<code>null</code>ではない場合、右オペランドは評価されません。</td>
	</tr>
</table>


例えば、<code>|</code> 演算子と <code>||</code> 演算子の挙動の差を見てみましょう。

<pre class="source" title="|| 演算子の短絡評価" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">string</span> Echo(<span class="reserved">string</span> message)
    {
        <span class="type">Console</span>.WriteLine(message);
        <span class="reserved">return</span> message;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Console</span>.WriteLine(<span class="literal">"短絡評価なし"</span>);
        <span class="reserved">var</span> x = Echo(<span class="literal">"a"</span>) == <span class="literal">"a"</span> | Echo(<span class="literal">"b"</span>) == <span class="literal">"b"</span>; <span class="comment">// a、b 両方出力。</span>

        <span class="type">Console</span>.WriteLine(<span class="literal">"短絡評価あり"</span>);
        <span class="reserved">var</span> y = Echo(<span class="literal">"a"</span>) == <span class="literal">"a"</span> || Echo(<span class="literal">"b"</span>) == <span class="literal">"b"</span>; <span class="comment">// a のみ出力</span>
    }
}
</code></pre>


<pre class="console" title="">
短絡評価なし
a
b
短絡評価あり
a
</pre>


また、条件演算子の短絡評価の例を示すと、以下のようになります。

<pre class="source" title="条件演算子の短絡評価" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">string</span> Echo(<span class="reserved">string</span> message)
    {
        <span class="type">Console</span>.WriteLine(message);
        <span class="reserved">return</span> message;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">true</span> ? Echo(<span class="literal">"第2項"</span>) : Echo(<span class="literal">"第3項"</span>); <span class="comment">// 第2項だけ表示される</span>
        <span class="reserved">var</span> y = <span class="reserved">false</span> ? Echo(<span class="literal">"第2項"</span>) : Echo(<span class="literal">"第3項"</span>); <span class="comment">// 第3項だけ表示される</span>
    }
}
</code></pre>


<pre class="console" title="">
第2項
第3項
</pre>



##<a id="sec-generated-title-17"></a> <a id="other"></a>その他の式
このページで紹介したような、数学でも出てくるような演算子は、
C# の構文上は式（expression）と呼ばれるものの一種です。
式には、その他にもいろいろな種類のものがあり、おいおい説明してくことになります。
（参考: 「[C# の式と文の一覧](../cheatsheet/list_expression.md)」）      


##<a id="sec-generated-title-18"></a> <a id="priority"></a>演算子の優先順位
演算子には以下に示す優先順位があります。

<table summary="">

	<tr>
		<th>分類</th>
		<th>式/演算子</th>
	</tr>
	<tr>
		<td markdown="1">基本式</td>
		<td markdown="1"><code>x++,  x--</code></td>
	</tr>
	<tr>
		<td markdown="1">単項式</td>
		<td markdown="1"><code>+,  -,  !,  ~,  ++x,  --x</code></td>
	</tr>
	<tr>
		<td markdown="1">乗法式</td>
		<td markdown="1"><code>*,  /,  % </code></td>
	</tr>
	<tr>
		<td markdown="1">加法式</td>
		<td markdown="1"><code>+,  - </code></td>
	</tr>
	<tr>
		<td markdown="1">シフト</td>
		<td markdown="1"><code>&lt;&lt;,  &gt;&gt; </code></td>
	</tr>
	<tr>
		<td markdown="1">関係式</td>
		<td markdown="1"><code>&lt;,  &gt;,  &lt;=,  &gt;=</code></td>
	</tr>
	<tr>
		<td markdown="1">等値式</td>
		<td markdown="1"><code>==,  !=</code></td>
	</tr>
	<tr>
		<td markdown="1">論理 AND</td>
		<td markdown="1"><code>&amp; </code></td>
	</tr>
	<tr>
		<td markdown="1">論理 XOR</td>
		<td markdown="1"><code>^ </code></td>
	</tr>
	<tr>
		<td markdown="1">論理 OR</td>
		<td markdown="1"><code>| </code></td>
	</tr>
	<tr>
		<td markdown="1">条件 AND</td>
		<td markdown="1"><code>&amp;&amp; </code></td>
	</tr>
	<tr>
		<td markdown="1">条件 OR</td>
		<td markdown="1"><code>|| </code></td>
	</tr>
	<tr>
		<td markdown="1">条件</td>
		<td markdown="1"><code>?: </code></td>
	</tr>
	<tr>
		<td markdown="1">null 合体</td>
		<td markdown="1"><code>?? </code></td>
	</tr>
	<tr>
		<td markdown="1">代入</td>
		<td markdown="1"><code>=,  *=, /=, %=, +=, -=, &lt;&lt;=, &gt;&gt;=, &amp;=, ^=, |= </code></td>
	</tr>
</table>


優先順位の高いものから順番に計算が行われます。
また、優先順位が同じ場合、代入演算では右から、それ以外の演算では左から順に計算が行われます。

他の式も含めた全体の優先順位は「[C# の式と文の一覧](../cheatsheet/list_expression.md)」を参照ください。
## <a id="exercise"></a>演習問題

### <a id="exercise-ope1"></a>問題 1


2つの整数を入力し、
その整数の四則演算(＋, －, ×, ÷)結果を表示するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="整数の四則演算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"input a: "</span>);
    <span class="reserved">int</span> a = <span class="reserved">int</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"input b: "</span>);
    <span class="reserved">int</span> b = <span class="reserved">int</span>.Parse(Console.ReadLine());

    Console.Write(<span class="literal">"{0} + {1} = {2}\n"</span>, a, b, a + b);
    Console.Write(<span class="literal">"{0} - {1} = {2}\n"</span>, a, b, a - b);
    Console.Write(<span class="literal">"{0} * {1} = {2}\n"</span>, a, b, a * b);
    Console.Write(<span class="literal">"{0} / {1} = {2}\n"</span>, a, b, a / b);
  }
}
</code></pre>



### <a id="exercise-ope2"></a>問題 2


前問の「整数の四則演算」の、 double, short 等の他の型を用いた物を作成せよ。


#### 解答例 1


例として double 版を掲載。

<pre class="source" title="実数の四則演算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"input a: "</span>);
    <span class="reserved">double</span> a = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"input b: "</span>);
    <span class="reserved">double</span> b = <span class="reserved">double</span>.Parse(Console.ReadLine());

    Console.Write(<span class="literal">"{0} + {1} = {2}\n"</span>, a, b, a + b);
    Console.Write(<span class="literal">"{0} - {1} = {2}\n"</span>, a, b, a - b);
    Console.Write(<span class="literal">"{0} * {1} = {2}\n"</span>, a, b, a * b);
    Console.Write(<span class="literal">"{0} / {1} = {2}\n"</span>, a, b, a / b);
  }
}
</code></pre>



### <a id="exercise-ope3"></a>問題 3


複素数 x + iy の逆数を求めるプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"実部を入力してください: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"虚部を入力してください: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> norm = x * x + y * y;

    Console.Write(<span class="literal">"{0} + i({1}) の逆数は {2} + i({3})\n)"</span>,
      x, y,
      x / norm, -y / norm);
  }
}
</code></pre>



### <a id="exercise-ope4"></a>問題 4


半径を入力し、その半径の円の面積を求めるプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="円の面積を求める" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span> r; <span class="comment">// 半径</span>

    Console.Write(<span class="literal">"半径を入力してください: "</span>);
    r = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> area = r * r * 3.1415926535897932;
    Console.Write(<span class="literal">"面積 = {0}\n"</span>, area);
  }
}
</code></pre>



### <a id="exercise-ope5"></a>問題 5


体重と身長を入力し、BMIを求めるプログラムを作成せよ。

<blockquote markdown="1">
BMIは、WHO（世界保健機関）が推奨しているもので、Body Mass Indexの略称で、肥満度指数とも呼ばれています。 BMIは肥満度の基準として、広く使用されている測定方法です。
計算式は、下記のとおりで比較的簡単に計算できることも特徴です。

BMI = 体重(kg)÷{身長(m)×身長(m)}

BMIの値が22のときに病気になる可能性が最も低く、BMIが26を超えると糖尿病など生活習慣病になるリスクが高まると言われています。

<table summary="">

	<tr>
		<td markdown="1">BMI 値</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1">19.8未満</td>
		<td markdown="1">やせ型</td>
	</tr>
	<tr>
		<td markdown="1">19.8～24.2未満</td>
		<td markdown="1">普通</td>
	</tr>
	<tr>
		<td markdown="1">24.2～26.4未満</td>
		<td markdown="1">やや肥満（過体重）</td>
	</tr>
	<tr>
		<td markdown="1">26.4～35.0未満</td>
		<td markdown="1">肥満</td>
	</tr>
	<tr>
		<td markdown="1">35.0以上</td>
		<td markdown="1">高度肥満（要治療）</td>
	</tr>
</table>


</blockquote>
以下にプログラムの実行結果の例を示す。

<pre class="console" title="結果の例">
身長[cm] = <span class="input">175.5</span>
体重[kg] = <span class="input">52.4</span>
BMI = 17.0128489216808
</pre>



#### 解答例 1


<pre class="source" title="BMI 値の計算" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">double</span> height; <span class="comment">// 身長[cm]</span>
    <span class="reserved">double</span> weight; <span class="comment">// 体重[kg]</span>

    Console.Write(<span class="literal">"身長[cm]: "</span>);
    height = <span class="reserved">double</span>.Parse(Console.ReadLine());
    height *= 0.01; <span class="comment">// cm → m</span>

    Console.Write(<span class="literal">"体重[kg]: "</span>);
    weight = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> bmi = weight / (height * height);
    Console.Write(<span class="literal">"BMI = {0}\n"</span>, bmi);
  }
}
</code></pre>
