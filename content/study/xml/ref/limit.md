---
title: "lim"
source_url: "https://ufcpp.net/study/xml/ref/limit/"
content_type: "Article"
published_at: "2015-05-06T14:25:05"
updated_at: "2015-05-06T14:25:05"
tags: []
umbraco_id: 1679
parent_id: 1661
sort_order: 17
aliases:
  - "/ref/limit"
  - "/ref/limit.html"
  - "/study/ref/limit"
  - "/study/ref/limit.html"
  - "/xml/ref/limit/"
---

# lim

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

極限記号limを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<lim>lim記号の下に来る式</lim>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<frac><num><d/>f</num><denom><d/>x</denom></frac> = 
<lim>Δx→0</lim><frac><num>f(x+Δx) &#x2212; f(x)</num><denom>Δx</denom></frac>
```
<div class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>f</td></tr><tr><td><span class="normal">d</span>x</td></tr></table> =
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δx→0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">f(x+Δx) − f(x)</td></tr><tr><td>Δx</td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:lim">
  <table class="sigma" summary="limitation">
    <tr><td><span class="normal">lim</span></td></tr>
    <tr><td class="sigmasub"><xsl:apply-templates/></td></tr>
  </table>
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

td.sigmasub
{
  font-size:70%;
}
```
