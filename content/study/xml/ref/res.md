---
title: "留数"
source_url: "https://ufcpp.net/study/xml/ref/res/"
content_type: "Article"
published_at: "2015-05-06T14:25:30"
updated_at: "2015-05-06T14:25:30"
tags: []
umbraco_id: 1689
parent_id: 1661
sort_order: 27
aliases:
  - "/ref/Res"
  - "/ref/Res.html"
  - "/study/ref/Res"
  - "/study/ref/Res.html"
  - "/xml/ref/res/"
---

# 留数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

留数を表す記号Resを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<Res>留数を求めたい関数,極</Res>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<int><sub>C</sub></int>f(z)<d/>z = 2πi<Sigma><sub>i</sub></Sigma><Res>f,a<sub>i</sub></Res>
```
<div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f(z)<span class="normal">d</span>z = 2πi<table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table><span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,a<sub>i</sub><span class="paren" style="font-size:em;">]</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:Res">
  <span class="normal">Res</span>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    [
  </span>
  <xsl:apply-templates/>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    ]
  </span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.paren
{
  font-style:normal;
  vertical-align:middle;
}

span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
