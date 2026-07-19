---
title: "ベクトル解析用記号"
source_url: "https://ufcpp.net/study/xml/ref/va/"
content_type: "Article"
published_at: "2015-05-06T14:25:43"
updated_at: "2015-05-06T14:25:43"
tags: []
umbraco_id: 1696
parent_id: 1661
sort_order: 34
aliases:
  - "/ref/va"
  - "/ref/va.html"
  - "/study/ref/va"
  - "/study/ref/va.html"
  - "/xml/ref/va/"
---

# ベクトル解析用記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

線素、面素、体積素。
勾配、発散、回転。
ナブラ記号。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<dV/>
<dS/>
<dl/>
<gradient/>
<divergence/>
<rotation/>
<textgrad/>
<textdiv/>
<textrot/>
<nabra/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<int><sub>V</sub></int> <divergence/><vec>f</vec> <dV/>
＝
<oint><sub>∂V</sub></oint> <vec>f</vec>・<dS/>
,
<int><sub>S</sub></int> <rotation/><vec>f</vec>・<dS/>
＝
<oint><sub>∂S</sub></oint> <vec>f</vec>・<dl/>
,
<d/>f
＝
<gradient/>f
・<dl/>,
<gradient/> ＝ <textgrad/>, 
<divergence/> ＝ <textdiv/>, 
<rotation/> ＝ <textrot/>, 
<nabra/>
```
<div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table> <span class="vector">∇</span>・<span class="vector">f</span> <span class="normal">d</span>V
＝
<span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table> <span class="vector">f</span>・<span class="normal">d</span><span class="vector">S</span>
,
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table> <span class="vector">∇</span>×<span class="vector">f</span>・<span class="normal">d</span><span class="vector">S</span>
＝
<span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂S</td></tr></table> <span class="vector">f</span>・<span class="normal">d</span><span class="vector">l</span>
,
<span class="normal">d</span>f
＝
<span class="vector">∇</span>f
・<span class="normal">d</span><span class="vector">l</span>,
<span class="vector">∇</span> ＝ <span class="normal">grad</span>, 
<span class="vector">∇</span>・ ＝ <span class="normal">div</span>, 
<span class="vector">∇</span>× ＝ <span class="normal">rot</span>, 
<span class="vector">∇</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:dV">
  <span class="normal">d</span>V
</xsl:template>

<xsl:template match="ufcpp:dS">
  <span class="normal">d</span><span class="vector">S</span>
</xsl:template>

<xsl:template match="ufcpp:dl">
  <span class="normal">d</span><span class="vector">l</span>
</xsl:template>

<xsl:template match="ufcpp:gradient">
  <span class="vector">∇</span>
</xsl:template>

<xsl:template match="ufcpp:divergence">
  <span class="vector">∇</span>・
</xsl:template>

<xsl:template match="ufcpp:rotation">
  <span class="vector">∇</span>×
</xsl:template>

<xsl:template match="ufcpp:textgrad">
  <span class="normal">grad</span>
</xsl:template>

<xsl:template match="ufcpp:textdiv">
  <span class="normal">div</span>
</xsl:template>

<xsl:template match="ufcpp:textrot">
  <span class="normal">rot</span>
</xsl:template>

<xsl:template match="ufcpp:nabra">
  <span class="vector">∇</span>
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
