---
title: "2階時間偏微分"
source_url: "https://ufcpp.net/study/xml/ref/pddt_second/"
content_type: "Article"
published_at: "2015-05-06T14:25:23"
updated_at: "2015-05-06T14:25:23"
tags: []
umbraco_id: 1686
parent_id: 1661
sort_order: 24
aliases:
  - "/study/ref/pddt_second.html"
---

# 2階時間偏微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

2階時間微分記号(∂/∂t)<sup>2</sup>を表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<pddt_second/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<pddt_second/>φ = c<sup>2</sup><nabra/><sup>2</sup>φ
```
<div class="math"><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table>φ = c<sup>2</sup><span class="vector">∇</span><sup>2</sup>φ
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:pddt_second">
  <table class="frac" summary="differential">
    <tr><td class="num">∂<sup>2</sup></td></tr>
    <tr><td>∂t<sup>2</sup></td></tr>
  </table>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
table.frac
{
  display:inline;
  vertical-align:middle;
  font-style:italic;
  font-size:90%;
  text-align:center;
}

td.num
{
  border-bottom:#000000 1pt solid;
}
```
