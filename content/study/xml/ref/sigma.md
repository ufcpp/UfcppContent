---
title: "∑"
source_url: "https://ufcpp.net/study/xml/ref/sigma/"
content_type: "Article"
published_at: "2015-05-06T14:25:32"
updated_at: "2015-05-06T14:25:32"
tags: []
umbraco_id: 1690
parent_id: 1661
sort_order: 28
aliases:
  - "/study/ref/Sigma.html"
---

# ∑

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

和の記号∑を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<Sigma><sub>∑の下にくる式</sub><sup>∑の上にくる式</sup></Sigma>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
f(a) = 
<Sigma><sub>n=0</sub><sup>∞</sup></Sigma>
<frac><num>f<sup>(n)</sup>(a)</num><denom>n<factorial/></denom></frac>
<paren>z-a</paren><sup>n</sup>
```
<div class="math">f(a) =
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=0</td></tr></table>
<table class="frac" summary="fraction"><tr><td class="num">f<sup>(n)</sup>(a)</td></tr><tr><td>n<span class="normal">!</span></td></tr></table>
<span class="paren" style="font-size:em;">(</span>z-a<span class="paren" style="font-size:em;">)</span><sup>n</sup>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:Sigma">
  <table class="sigma" summary="sum">
    <tr><td class="sigmasub"><xsl:apply-templates select="ufcpp:sup"/></td></tr>
    <tr><td class="sigma">&#8721;</td></tr>
    <tr><td class="sigmasub"><xsl:apply-templates select="ufcpp:sub"/></td></tr>
  </table>
</xsl:template>

<xsl:template match="ufcpp:Sigma/ufcpp:sup|ufcpp:Pi/ufcpp:sup|ufcpp:Sigma/ufcpp:sub|ufcpp:Pi/ufcpp:sub">
  <xsl:apply-templates/>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.sigma
{
  display:inline;
  text-align:center;
  vertical-align:middle;
  font-style:italic;
}

td.sigma
{
  font-style:normal;
  font-size:120%;
}

td.sigmasub
{
  font-size:70%;
}
```
