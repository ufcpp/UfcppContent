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
  - "/study/testxsl/math.html"
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


```html {title="分数っぽく見える HTML 記述"}
<table style="
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;text-align:center;">
<tr><td style="
  border-bottom:#000000 1pt solid;">
x
</td></tr>
<tr><td>
y
</td></tr>
</table>
```
となるわけです。
ちなみに、こんな感じの見た目になります↓

<blockquote markdown="1"><div class="math">
        <table class="frac" summary="fraction"><tr><td class="num">x</td></tr><tr><td>y</td></tr></table>
      </div>
</blockquote>
さすがにこんなもんがそこら中にちりばめられるのは、書くのも見るのも辛いです。
同じことを LaTeX で書こうと思うと、

```csharp {title="TeX なら"}
\begin{math}
\frac{x}{y}
\end{math}
```


これだけですむわけです。
せめて XML を使ってこんな風↓に書ければいくらか書きやすくなりますよね。


```xml {title="独自に分数用 XML を定義"}
<math>
 <frac>
  <num>x</num>
  <denom>y</denom>
 </frac>
</math>
```
そこで、XSL を使ってこの XML を先ほど書いたような HTML に変換するルールを書いてやることにしました。 
スタイルも CSS 使って指定するようにしました。


```xml {title="分数用 XML → HTML に変換"}
<xsl:template match="math">
 <span class="math"><nobr><xsl:apply-templates/></nobr></span>
</xsl:template>

<xsl:template match="frac">
 <table class="frac">
  <tr><td class="num"><xsl:apply-templates select="num"/></td></tr>
  <tr><td><xsl:apply-templates select="denom"/></td></tr>
 </table>
</xsl:template>

<xsl:template match="num"><xsl:apply-templates/></xsl:template>
<xsl:template match="denom"><xsl:apply-templates/></xsl:template>
```
```csharp
span.math{font-style:italic;font-family: serif;}
table.frac{
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;
  text-align:center;
}
td.num{border-bottom:#000000 1pt solid;}
```


同様にしてベクトル（太字イタリック体にする）や積分（∫を120%拡大して左右のマージンを調整）、
微分（frac タグと同じ原理で d/dt を表現）なども定義しました。

こうして出来上がったのが「[勉強用ページの XSL](../index.md)」で説明しているようなXSLスタイルシートなわけです。
