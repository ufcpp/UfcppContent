---
title: "指数・対数"
source_url: "https://ufcpp.net/study/xml/ref/log/"
content_type: "Article"
published_at: "2015-05-06T14:25:07"
updated_at: "2015-05-06T14:25:07"
tags: []
umbraco_id: 1680
parent_id: 1661
sort_order: 18
aliases:
  - "/ref/log"
  - "/ref/log.html"
  - "/study/ref/log"
  - "/study/ref/log.html"
  - "/xml/ref/log/"
---

# 指数・対数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

log,expの表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<log/> <exp/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<exp/>At = <Sigma><sub>n=0</sub><sup>∞</sup></Sigma><inv>n<factorial/></inv>A<sup>n</sup>t<sup>n</sup>,
<log/>z = <log/><abs>z</abs> + i<arg/>z
```
<div class="math"><span class="normal">exp</span>At = <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n<span class="normal">!</span></td></tr></table>A<sup>n</sup>t<sup>n</sup>,
<span class="normal">log</span>z = <span class="normal">log</span><span class="normal">|</span>z<span class="normal">|</span> + i<span class="normal">arg</span>z
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:log">
  <span class="normal">log</span>
</xsl:template>

<xsl:template match="ufcpp:exp">
  <span class="normal">exp</span>
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
