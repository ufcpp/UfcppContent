---
title: "論理演算と算術演算"
source_url: "https://ufcpp.net/study/computer/old/logic/"
content_type: "Article"
published_at: "2007-05-08T00:00:00"
updated_at: "2015-05-06T14:06:19"
tags: []
umbraco_id: 1168
parent_id: 1166
sort_order: 1
aliases:
  - "/study/computer/logic.html"
---

# 論理演算と算術演算

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

コンピュータ内での演算の基本は論理演算です。

論理演算ができる電子回路（論理回路）があれば、
加減乗除などの算術演算も実現可能です。

ということで、ここでは、論理回路の話を少しつまむ程度に話をしておきます。
例として、加算器の作り方や、負の数の表現方法を説明します。


## <a id="sec-generated-title-2"></a> <a id="logical"></a>論理演算

「[n 進数](n_adic.md)」で、
コンピュータの中では 0, 1 の2進で数値を表しているという説明をしました。

0, 1 は電圧の高低で表されているわけで、high / low と言ってもいいし、
on / off と言ってもいい。
0 が偽（false）で 1 が真（true）を表していると考えて、
真偽値（boolean）とか論理値（logical value）とか言ったりもします。

で、2進数の 0, 1 を false / true の真偽値だとみなしていろいろ演算することを、
論理演算（logical operation）とかブール演算（boolean operation）といいます。
（ちなみに、ブール（George Boole）は人名。論理演算を考えた人。）

論理演算というと、分かりやすいのは AND, OR, NOT の3つです。
それぞれ、「a かつ b」、「a または b」、「a の逆」です（表1）。

<table summary="AND, OR, NOT">
	<caption>
		AND, OR, NOT
	</caption>
	<tr>
		<td markdown="1" colspan="2">　</td>
		<td markdown="1">AND</td>
		<td markdown="1">OR</td>
		<td markdown="1">NOT</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">日本語名称</td>
		<td markdown="1">論理積</td>
		<td markdown="1">論理和</td>
		<td markdown="1">否定</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">論理演算子</td>
		<td markdown="1"><span class="math">
            a <span class="normal">∧</span> b
          </span></td>
		<td markdown="1"><span class="math">
            a <span class="normal">∨</span> b
          </span></td>
		<td markdown="1"><span class="math">
            <span class="normal">¬</span> a
          </span></td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">代数的記法</td>
		<td markdown="1"><span class="math">ab</span></td>
		<td markdown="1"><span class="math">
            a <span class="normal">+</span> b
          </span></td>
		<td markdown="1"><span class="math">
            <span class="bar">a</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">読み方</td>
		<td markdown="1">a かつ b</td>
		<td markdown="1">a または b</td>
		<td markdown="1">a の否定、非 a</td>
	</tr>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">
            a <span class="normal">∧</span> b
          </span></th>
		<th><span class="math">
            a <span class="normal">∨</span> b
          </span></th>
		<th><span class="math">
            <span class="normal">¬</span> a
          </span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
</table>


その他、NAND, NOR, XOR, XNOR (EQ) なんてものもあります。
NAND, NOR は NOT AND, NOT OR の略で、
名前どおり、AND と OR を否定したもの。
XOR は a と b が違うとき真、
XNOR (EQ) は a と b が同じとき真になる2項演算です（表2）。

<table summary="NAND, NOR, XOR, XNOR">
	<caption>
		NAND, NOR, XOR, XNOR
	</caption>
	<tr>
		<td markdown="1" colspan="2">　</td>
		<td markdown="1">NAND</td>
		<td markdown="1">NOR</td>
		<td markdown="1">XOR</td>
		<td markdown="1">XNOR (EQ)</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">日本語名称</td>
		<td markdown="1">否定論理積</td>
		<td markdown="1">否定論理和</td>
		<td markdown="1">排他的論理和</td>
		<td markdown="1">XOR の否定（等価）</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">論理演算子</td>
		<td markdown="1"><span class="math">
            <span class="normal">¬</span>
            <span class="paren" style="font-size:em;">(</span>
              a <span class="normal">∧</span> b
            <span class="paren" style="font-size:em;">)</span>
          </span></td>
		<td markdown="1"><span class="math">
            <span class="normal">¬</span>
            <span class="paren" style="font-size:em;">(</span>
              a <span class="normal">∨</span> b
            <span class="paren" style="font-size:em;">)</span>
          </span></td>
		<td markdown="1"><span class="math">
            a <span class="normal">⊕</span> b
          </span></td>
		<td markdown="1"><span class="math">
            a <span class="normal">≡</span> b
          </span></td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">代数的記法</td>
		<td markdown="1"><span class="math">
            <span class="bar">ab</span>
          </span></td>
		<td markdown="1"><span class="math">
            <span class="bar">
              a <span class="normal">+</span> b
            </span>
          </span></td>
		<td markdown="1"><span class="math">
            <span class="bar">a</span>b <span class="normal">+</span> a<span class="bar">b</span>
          </span></td>
		<td markdown="1"><span class="math">
            <span class="bar">ab</span>
            <span class="normal">+</span> ab
          </span></td>
	</tr>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">
            <span class="normal">¬</span>
            <span class="paren" style="font-size:em;">(</span>
              a <span class="normal">∧</span> b
            <span class="paren" style="font-size:em;">)</span>
          </span></th>
		<th><span class="math">
            <span class="normal">¬</span>
            <span class="paren" style="font-size:em;">(</span>
              a <span class="normal">∨</span> b
            <span class="paren" style="font-size:em;">)</span>
          </span></th>
		<th><span class="math">
            a <span class="normal">⊕</span> b
          </span></th>
		<th><span class="math">
            a <span class="normal">≡</span> b
          </span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
</table>


表2の「代数的記法」の行のように、NAND, NOR, XOR, EQ は、
AND, OR, NOT を使って表すことができます。
さらに言うと、AND と NOT があれば OR も作れます。
あと、a NAND a が NOT a と同じになるので、
NAND 1個だけでも他の演算を全部表現できます。

NAND 演算を電子回路を使って実現するのは（少なくともトランジスタの動作が分かれば）割りと簡単で、
よくある実現方法として
CMOS NAND 回路というのがあります。
ここではあまり詳しくは触れませんが、興味があればこの単語をキーワードに検索してみてください。


## <a id="sec-generated-title-3"></a> <a id="adder"></a>加算器

前節の最後でちょこっとだけ触れましたが、
論理演算は電子回路を用いて簡単に実現できます。
（これを論理回路と呼びます。）

では、2進数の算術演算（加減乗除とか）はどうでしょうか。
実は、算術演算も、論理演算を使って実現することができます。
ということで、ここでは、例として加算器の作り方を説明します。

まあ、まずは1桁だけの場合を考えて見ましょう。
<span class="math">a</span> も <span class="math">b</span> も1ビット（1桁）の論理値として、
（論理和ではなくて）数値としての和 <span class="math">
        a <span class="normal">+</span> b
      </span> を求めることを考えます。
（なお、本節では、
算術和と論理和の区別のために、論理積/和は
<span class="math">
        a <span class="normal">∧</span> b
      </span>,
<span class="math">
        a <span class="normal">∨</span> b
      </span>
で書き表します。）

1桁同士の足し算なので、結果は2桁（以下）になるはずです。
足し算結果を表にすると、表3のようになります。

<table summary="1桁の2進数の足し算">
	<caption>
		1桁の2進数の足し算
	</caption>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">
            a <span class="normal">+</span> b
          </span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">10</td>
	</tr>
</table>


これを、1桁目の足し算結果 <span class="math">s</span> と、2桁目への繰り上がり <span class="math">c</span> に分けて書くと、
表4のようになります。

<table summary="1桁の2進数の足し算（s と c）">
	<caption>
		1桁の2進数の足し算（s と c）
	</caption>
	<tr>
		<th><span class="math">a</span></th>
		<th><span class="math">b</span></th>
		<th><span class="math">c</span></th>
		<th><span class="math">s</span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
</table>


前節の表と見比べてみると、
<span class="math">
        c <span class="normal">=</span> a <span class="normal">∧</span> b
      </span>,
<span class="math">
        s <span class="normal">=</span> a <span class="normal">⊕</span> b
      </span>
になっていることが分かります。

それでは、桁数を増やしてみましょう。
数値 <span class="math">a, b</span> の
2進数 <span class="math">n</span> 桁目のビットをそれぞれ
<span class="math">
        a<sub>n</sub>, b<sub>n</sub>
      </span> で表してみます。
1ビット目と違って、下の桁からの繰り上がり<span class="math">
        c<sub>n</sub>
      </span>も考えて、
<span class="math">n</span> ビット目の足し算結果 <span class="math">
        s<sub>n</sub>
      </span>
上の桁への繰り上がり <span class="math">
        c<sub>
          n <span class="normal">+</span><span class="normal">1</span>
        </sub>
      </span> は表5のようになります。

<table summary="繰り上がり付きの足し算">
	<caption>
		繰り上がり付きの足し算
	</caption>
	<tr>
		<th><span class="math">
            a<sub>n</sub>
          </span></th>
		<th><span class="math">
            b<sub>n</sub>
          </span></th>
		<th><span class="math">
            c<sub>n</sub>
          </span></th>
		<th><span class="math">
            c<sub>
              n <span class="normal">+</span><span class="normal">1</span>
            </sub>
          </span></th>
		<th><span class="math">
            s<sub>n</sub>
          </span></th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
		<td markdown="1">1</td>
		<td markdown="1">0</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
		<td markdown="1">1</td>
	</tr>
</table>


だいぶ複雑に見えますが、
論理回路に関して勉強すれば、
この表を論理演算で表現する方法も分かります。
（詳しくは「論理回路」あたりをキーワードに検索を。）

とりあえずここでは、算術演算も論理演算の組み合わせで実現できるということを示したいだけなので、
結論だけ書きますが、
<span class="math">
        c<sub>
          n <span class="normal">+</span><span class="normal">1</span>
        </sub>
      </span>、
<span class="math">
        s<sub>n</sub>
      </span> は以下のような論理式で表されます。
<div class="math">
      c<sub>
        n <span class="normal">+</span><span class="normal">1</span>
      </sub><span class="normal">=</span><span class="paren" style="font-size:em;">(</span>
        a<sub>n</sub><span class="normal">∧</span> b<sub>n</sub>
      <span class="paren" style="font-size:em;">)</span><span class="normal">∨</span><span class="paren" style="font-size:em;">(</span>
        b<sub>n</sub><span class="normal">∧</span> c<sub>n</sub>
      <span class="paren" style="font-size:em;">)</span><span class="normal">∨</span><span class="paren" style="font-size:em;">(</span>
        c<sub>n</sub><span class="normal">∧</span> a<sub>n</sub>
      <span class="paren" style="font-size:em;">)</span>
    </div><div class="math">
      s<sub>n</sub><span class="normal">=</span>
      a<sub>n</sub><span class="normal">⊕</span>
      b<sub>n</sub><span class="normal">⊕</span>
      c<sub>n</sub>
    </div>
ちなみに、
<span class="math">
        c<sub>
          n <span class="normal">+</span><span class="normal">1</span>
        </sub>
      </span> は、
<span class="math">a, b, c</span> の3個中2個以上が 1 のときに結果が 1 になるので、
多数決回路と呼ばれています。
また、<span class="math">
        s<sub>n</sub>
      </span> の方は、
1 の数が奇数個のときに結果が 1 になっています。

この式が分かれば加算器を作るのは簡単で、
この論理式を <span class="math">a, b</span> の桁数分並べれば OK。
（
そういう単純な方式の加算器を桁上げ伝搬加算器（ripple carry adder、ripple は波紋の意味）と呼びます。
あんまり動作は高速じゃなくて、
実際は何桁か先の繰り上がりを別に計算する桁上げ先見加算器なんてものも考えられていて、
こちらの方が賢い実装方法です。
）

ということで、少し飛ばし飛ばしの説明でしたが、
要するに、
「論理演算ができる回路があれば、算術演算も回路化できる」、
「コンピュータ内の演算の基本は論理回路」というのがポイントです。


## <a id="sec-generated-title-4"></a> <a id="negative"></a>負の数

次は、負の数の表現の仕方を考えてみます。
符号と絶対値を別々に持つとか、いろいろ考えられるんですけども、
よく使われるのは「2の補数表現」と呼ばれる表現方法です。

2の補数は、
<span class="math">a</span> に対して、各ビットを 0, 1 反転させた上で1を足すことで作ります。
例えば、4ビット（2進数4桁）で表される数値の場合、
2の補数は表6のようになります。

<table summary="2の補数（4ビット）">
	<caption>
		2の補数（4ビット）
	</caption>
	<tr>
		<th colspan="2"><span class="math">a</span></th>
		<th rowspan="2"><span class="math">
            <span class="normal">−</span>a
          </span></th>
	</tr>
	<tr>
		<th>10進数</th>
		<th>2進数</th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0000</td>
		<td markdown="1">0000</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0001</td>
		<td markdown="1">1111</td>
	</tr>
	<tr>
		<td markdown="1">2</td>
		<td markdown="1">0010</td>
		<td markdown="1">1110</td>
	</tr>
	<tr>
		<td markdown="1">3</td>
		<td markdown="1">0011</td>
		<td markdown="1">1101</td>
	</tr>
	<tr>
		<td markdown="1">4</td>
		<td markdown="1">0100</td>
		<td markdown="1">1100</td>
	</tr>
	<tr>
		<td markdown="1">5</td>
		<td markdown="1">0101</td>
		<td markdown="1">1011</td>
	</tr>
	<tr>
		<td markdown="1">6</td>
		<td markdown="1">0110</td>
		<td markdown="1">1010</td>
	</tr>
	<tr>
		<td markdown="1">7</td>
		<td markdown="1">0111</td>
		<td markdown="1">1001</td>
	</tr>
</table>


要するに、<span class="math">n</span> ビットの2進数 <span class="math">a</span> に対して、
<span class="math">
        a <span class="normal">+</span> b <span class="normal">=</span><span class="normal">2</span><sup>n</sup>
      </span>
になる数 <span class="math">b</span> が <span class="math">a</span> の2の補数です。

4ビットの例、すなわち表6の例では、
表の2列目と3列目の2進数を足すと、どの行も 10000 になります。
これを4ビット＋4ビット → 4ビットの演算と考えるなら、
5ビット目の1は無視されて（というか、わざと無視して） 0 になるというわけです。

「負の数」 ＝ 「足して 0 になる数」ということで、
この2の補数を負の数の表現として使うといろいろと都合がいい。
具体的には、
正の数同士の加算器とかをそのまま使って正負問わず整数の加算・減算ができるようになります。
あと、最上位の1ビットを見れば符号が分かる（0 なら正、1 なら負）というのも利点。

ちなみに、2の補数という言葉は、
「足して <span class="math">
        <span class="normal">2</span>
        <sup>n</sup>
      </span> になる数」
という意味なので、本当は <span class="math">
        <span class="normal">2</span>
        <sup>n</sup>
      </span> の補数という方がいいのかもしれない。
まあ、習慣的に2の補数と呼びます。

ただし、2の補数表現では、<span class="math">
        <span class="normal">2</span>
        <sup>
          n <span class="normal">−</span><span class="normal">1</span>
        </sup>
      </span> の扱い
（4ビットの場合は 8）にだけは気をつけないといけません。
8 は2進数で書くと 1000 なわけですが、
これの2の補数は 1000 で元のままです。
最上位ビットが 1 なので、2の補数表現としてはこれは －8 とみなすべきなんですが、
要するに、＋8 は4ビットでは表現できないし、
－8 は －1 をかけたつもりでも －8 のままになってしまいます。

ちなみに、あんまり使われることはないんですが、
符号絶対値表現とか、1の補数表現なんてものもあるので、
参考程度に表7にまとめておきます。

<table summary="符号絶対値、1の補数、2の補数（4ビット）">
	<caption>
		符号絶対値、1の補数、2の補数（4ビット）
	</caption>
	<tr>
		<th colspan="2"><span class="math">a</span></th>
		<th colspan="3"><span class="math">
            <span class="normal">−</span>a
          </span></th>
	</tr>
	<tr>
		<th rowspan="2">10進数</th>
		<th rowspan="2">2進数</th>
		<th>符号絶対値表現</th>
		<th>1の補数</th>
		<th>2の補数</th>
	</tr>
	<tr>
		<td markdown="1">最上位1ビットだけ 0, 1 反転</td>
		<td markdown="1">全ビット 0, 1 反転</td>
		<td markdown="1">1の補数 ＋ 1</td>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1">0000</td>
		<td markdown="1">1000</td>
		<td markdown="1">1111</td>
		<td markdown="1">0000</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">0001</td>
		<td markdown="1">1001</td>
		<td markdown="1">1110</td>
		<td markdown="1">1111</td>
	</tr>
	<tr>
		<td markdown="1">2</td>
		<td markdown="1">0010</td>
		<td markdown="1">1010</td>
		<td markdown="1">1101</td>
		<td markdown="1">1110</td>
	</tr>
	<tr>
		<td markdown="1">3</td>
		<td markdown="1">0011</td>
		<td markdown="1">1011</td>
		<td markdown="1">1100</td>
		<td markdown="1">1101</td>
	</tr>
	<tr>
		<td markdown="1">4</td>
		<td markdown="1">0100</td>
		<td markdown="1">1100</td>
		<td markdown="1">1011</td>
		<td markdown="1">1100</td>
	</tr>
	<tr>
		<td markdown="1">5</td>
		<td markdown="1">0101</td>
		<td markdown="1">1101</td>
		<td markdown="1">1010</td>
		<td markdown="1">1011</td>
	</tr>
	<tr>
		<td markdown="1">6</td>
		<td markdown="1">0110</td>
		<td markdown="1">1110</td>
		<td markdown="1">1001</td>
		<td markdown="1">1010</td>
	</tr>
	<tr>
		<td markdown="1">7</td>
		<td markdown="1">0111</td>
		<td markdown="1">1111</td>
		<td markdown="1">1000</td>
		<td markdown="1">1001</td>
	</tr>
</table>
