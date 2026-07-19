---
title: "XML で数式を書こう"
source_url: "https://ufcpp.net/study/xml/xslsummary/math/"
content_type: "Article"
published_at: "2015-05-06T14:24:00"
updated_at: "2015-07-07T18:38:58"
tags: []
umbraco_id: 1648
parent_id: 1645
sort_order: 2
aliases:
  - "/study/testxsl/math"
  - "/study/testxsl/math.html"
  - "/testxsl/math"
  - "/testxsl/math.html"
  - "/xml/xslsummary/math/"
---

# XML で数式を書こう

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

勉強用ページにある数式は全て、
何らかのプラグインを使っているわけでも画像になっているわけでもなく、
HTML で表示しています。

数式表示のためには、
[mathenv.xsl](../../../../assets/media/ufcpp2000/xml/xslfiles/mathenv.xsl) という XSL を使っています。
mathenv.xsl の詳細は、「[勉強用ページの XSL](../index.md)」を参照してください。

ちなみに、XSD もあります → [math.xsd](../../../../assets/media/ufcpp2000/xsd/math.xsd)。


## <a id="sec-generated-title-2"></a> <a id="byHtml"></a>HTMLで数式を書こう

このサイトでは勉強用ページと称して数学等の解説を書いているわけですが、
そこでは意地でも数式を HTML で書いています。

なぜかというと、
LaTeX2HTML とかのツールを使うと数式は jpeg 画像にされちゃって、
背景色を変えたときに数式の部分だけ浮くのが嫌だし、
こういう変換ソフト使うとスタイルシート非対応なHTMLを吐かれたりするのも嫌だし。

MathML という数式を表現するための XML 仕様もあるのですが、
これを HTML 中に埋め込んで表示できるようなブラウザが少ない
（IE が標準で対応していないので話にならない）ので利用は断念。

それで意地でもHTMLで数式を書くことにしたわけですが、
本来、HTML に数式を表示するような機能はないので、
よく人から「どうやって書いてるの？」と聞かれます。

種を明かすと、全部 table と CSS を使って書いています。
が、以下の例を見てもらえるとわかる通り、
やっぱり HTML は数式を書くのには全然適していません。
分数一つ書くにしても、


<pre class="xsource" title="分数っぽく見える HTML 記述">
<code><span class="bracket">&lt;</span><span class="element">table</span> <span class="attribute">style</span><span class="attvalue">="
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;text-align:center;"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">td</span> <span class="attribute">style</span><span class="attvalue">="
  border-bottom:#000000 1pt solid;"</span><span class="bracket">&gt;</span>
x
<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>
y
<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
</code></pre>
となるわけです。
ちなみに、こんな感じの見た目になります↓

<blockquote markdown="1"><div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">x</td></tr><tr><td>y</td></tr></table>
      </div>
</blockquote>
さすがにこんなもんがそこら中にちりばめられるのは、書くのも見るのも辛いです。
同じことを LaTeX で書こうと思うと、

<pre class="source" title="TeX なら" lang="">
<code>\begin{math}
\frac{x}{y}
\end{math}
</code></pre>


これだけですむわけです。
せめて XML を使ってこんな風↓に書ければいくらか書きやすくなりますよね。


<pre class="xsource" title="独自に分数用 XML を定義">
<code><span class="bracket">&lt;</span><span class="element">math</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">frac</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">num</span><span class="bracket">&gt;</span>x<span class="bracket">&lt;/</span><span class="element">num</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">denom</span><span class="bracket">&gt;</span>y<span class="bracket">&lt;/</span><span class="element">denom</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;/</span><span class="element">frac</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">math</span><span class="bracket">&gt;</span>
</code></pre>
そこで、XSL を使ってこの XML を先ほど書いたような HTML に変換するルールを書いてやることにしました。 
スタイルも CSS 使って指定するようにしました。


<pre class="xsource" title="分数用 XML → HTML に変換">
<code><span class="bracket">&lt;</span><span class="element">xsl:template</span> <span class="attribute">match</span><span class="attvalue">="math"</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="math"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">nobr</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">xsl:apply-templates</span><span class="bracket">/&gt;</span><span class="bracket">&lt;/</span><span class="element">nobr</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">xsl:template</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">xsl:template</span> <span class="attribute">match</span><span class="attvalue">="frac"</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">table</span> <span class="attribute">class</span><span class="attvalue">="frac"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">td</span> <span class="attribute">class</span><span class="attvalue">="num"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">xsl:apply-templates</span> <span class="attribute">select</span><span class="attvalue">="num"</span><span class="bracket">/&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">xsl:apply-templates</span> <span class="attribute">select</span><span class="attvalue">="denom"</span><span class="bracket">/&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">xsl:template</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">xsl:template</span> <span class="attribute">match</span><span class="attvalue">="num"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">xsl:apply-templates</span><span class="bracket">/&gt;</span><span class="bracket">&lt;/</span><span class="element">xsl:template</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">xsl:template</span> <span class="attribute">match</span><span class="attvalue">="denom"</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">xsl:apply-templates</span><span class="bracket">/&gt;</span><span class="bracket">&lt;/</span><span class="element">xsl:template</span><span class="bracket">&gt;</span>
</code></pre>
<pre class="source" title="" lang="">
<code>span.math{font-style:italic;font-family: serif;}
table.frac{
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;
  text-align:center;
}
td.num{border-bottom:#000000 1pt solid;}
</code></pre>


同様にしてベクトル（太字イタリック体にする）や積分（∫を120%拡大して左右のマージンを調整）、
微分（frac タグと同じ原理で d/dt を表現）なども定義しました。

こうして出来上がったのが「[勉強用ページの XSL](../index.md)」で説明しているようなXSLスタイルシートなわけです。
