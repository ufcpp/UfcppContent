---
title: "絶対値"
source_url: "https://ufcpp.net/study/xml/ref/abs/"
content_type: "Article"
published_at: "2015-05-06T14:24:31"
updated_at: "2015-05-06T14:24:31"
tags: []
umbraco_id: 1662
parent_id: 1661
sort_order: 0
aliases:
  - "/study/ref/abs.html"
---

# 絶対値

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

絶対値記号||の表示
（obsolete。bracket に移行。）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<abs>絶対値記号内の式</abs>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<int/><inv>x</inv><d/>x = <log/><abs>x</abs> + C
```
<div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table><span class="normal">d</span>x = <span class="normal">log</span><span class="normal">|</span>x<span class="normal">|</span> + C
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:abs">
  <span class="normal">|</span>
  <xsl:apply-templates/>
  <span class="normal">|</span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
