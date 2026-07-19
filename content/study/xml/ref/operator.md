---
title: "演算子"
source_url: "https://ufcpp.net/study/xml/ref/operator/"
content_type: "Article"
published_at: "2015-05-06T14:25:13"
updated_at: "2015-05-06T14:25:13"
tags: []
umbraco_id: 1683
parent_id: 1661
sort_order: 21
aliases:
  - "/ref/operator"
  - "/ref/operator.html"
  - "/study/ref/operator"
  - "/study/ref/operator.html"
  - "/xml/ref/operator/"
---

# 演算子

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

演算子。
（短縮形: o）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<operator name="name">operator</operator>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
x <o>+</o> y,
x <o name="pm"/> y,
x <o n="mp"/> y
```
<div class="math">x <span class="normal">+</span> y,
x <span class="normal">±</span> y,
x <span class="normal">∓</span> y
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:math//ufcpp:o|ufcpp:math//ufcpp:operator|ufcpp:Math//ufcpp:o|ufcpp:Math//ufcpp:operator">
<xsl:variable name="n"><xsl:choose><xsl:when test="@name != ''"><xsl:value-of select="@name"/></xsl:when><xsl:otherwise><xsl:value-of select="@n"/></xsl:otherwise></xsl:choose></xsl:variable>

<span class="normal">
<xsl:choose>
 <xsl:when test="$n = 'in'">&#8712;</xsl:when><!--∈-->
 <xsl:when test="$n = 'nin'">&#8713;</xsl:when><!--∉-->
 <xsl:when test="$n = 'ni'">&#8715;</xsl:when><!--∋-->
 <xsl:when test="$n = 'nni'">&#8716;</xsl:when><!--∌-->

 <xsl:when test="$n = 'wedge'">&#8743;</xsl:when><!--∧-->
 <xsl:when test="$n = 'vee'">&#8744;</xsl:when><!--∨-->
 <xsl:when test="$n = 'cap'">&#8745;</xsl:when><!--∩-->
 <xsl:when test="$n = 'cup'">&#8746;</xsl:when><!--∪-->

 <xsl:when test="$n = 'sub'">&#8834;</xsl:when><!--⊂-->
 <xsl:when test="$n = 'sup'">&#8835;</xsl:when><!--⊃-->
 <xsl:when test="$n = 'nsub'">&#8836;</xsl:when><!--⊄-->
 <xsl:when test="$n = 'nsup'">&#8837;</xsl:when><!--⊅-->
 <xsl:when test="$n = 'sube'">&#8838;</xsl:when><!--⊆-->
 <xsl:when test="$n = 'supe'">&#8839;</xsl:when><!--⊇-->

 <xsl:when test="$n = 'perp'">&#8869;</xsl:when><!--⊥-->
 <xsl:when test="$n = 'para'">&#8514;</xsl:when><!--∥-->

 <xsl:when test="$n = 'eq'">=</xsl:when><!--=-->
 <xsl:when test="$n = 'sim'">&#8764;</xsl:when><!--∼-->
 <xsl:when test="$n = 'approx'">&#8773;</xsl:when><!--≅-->
 <xsl:when test="$n = 'asymp'">&#8776;</xsl:when><!--≈-->
 <xsl:when test="$n = 'ne'">&#8800;</xsl:when><!--≠-->
 <xsl:when test="$n = 'equiv'">&#8801;</xsl:when><!--≡-->
 <xsl:when test="$n = 'prop'">&#8733;</xsl:when><!--∝-->

 <xsl:when test="$n = 'lt'">&lt;</xsl:when><!--<-->
 <xsl:when test="$n = 'gt'">&gt;</xsl:when><!-->-->
 <xsl:when test="$n = 'le'">&#8804;</xsl:when><!--≤-->
 <xsl:when test="$n = 'ge'">&#8805;</xsl:when><!--≥-->
 <xsl:when test="$n = 'lE'">&#8806;</xsl:when><!--≦-->
 <xsl:when test="$n = 'gE'">&#8807;</xsl:when><!--≧-->
 <xsl:when test="$n = 'lnE'">&#8808;</xsl:when><!--≨-->
 <xsl:when test="$n = 'gnE'">&#8809;</xsl:when><!--≩-->
 <xsl:when test="$n = 'Lt'">&#8810;</xsl:when><!--≪-->
 <xsl:when test="$n = 'Gt'">&#8811;</xsl:when><!--≫-->

 <xsl:when test="$n = 'p'">&#43;</xsl:when><!--+-->
 <xsl:when test="$n = 'plus'">&#43;</xsl:when><!--+-->
 <xsl:when test="$n = 'm'">&#8722;</xsl:when><!--−-->
 <xsl:when test="$n = 'minus'">&#8722;</xsl:when><!--−-->
 <xsl:when test="$n = 'times'">&#215;</xsl:when><!--×-->
 <xsl:when test="$n = 'div'">&#247;</xsl:when><!--÷-->
 <xsl:when test="$n = 'slash'">/</xsl:when><!--/-->
 <xsl:when test="$n = 'mp'">&#8723;</xsl:when><!--∓-->
 <xsl:when test="$n = 'pm'">&#177;</xsl:when><!--±-->

 <xsl:when test="$n = 'oplus'">&#8853;</xsl:when><!--⊕-->
 <xsl:when test="$n = 'ominus'">&#8854;</xsl:when><!--⊖-->
 <xsl:when test="$n = 'otimes'">&#8855;</xsl:when><!--⊗-->
 <xsl:when test="$n = 'bs'">&#8726;</xsl:when><!--∖-->

 <xsl:when test="$n = 'dot'">&#8901;</xsl:when><!--⋅-->
 <xsl:when test="$n = 'cross'">&#215;</xsl:when><!--×-->
 <xsl:when test="$n = 'wedge'">&#8743;</xsl:when><!--∧-->
 <xsl:when test="$n = 'ring'">&#8728;</xsl:when><!--∘-->
 <xsl:when test="$n = 'aster'">&#8727;</xsl:when><!--∗-->
 <xsl:when test="$n = 'star'">&#8902;</xsl:when><!--⋆-->
 <xsl:when test="$n = 'not'">&#172;</xsl:when><!--¬-->

 <xsl:when test="$n = 'larr'">&#8592;</xsl:when><!--←-->
 <xsl:when test="$n = 'uarr'">&#8593;</xsl:when><!--↑-->
 <xsl:when test="$n = 'rarr'">&#8594;</xsl:when><!--→-->
 <xsl:when test="$n = 'darr'">&#8595;</xsl:when><!--↓-->
 <xsl:when test="$n = 'harr'">&#8596;</xsl:when><!--↔-->

 <xsl:when test="$n = 'lArr'">&#8656;</xsl:when><!--⇐-->
 <xsl:when test="$n = 'uArr'">&#8657;</xsl:when><!--⇑-->
 <xsl:when test="$n = 'rArr'">&#8658;</xsl:when><!--⇒-->
 <xsl:when test="$n = 'dArr'">&#8659;</xsl:when><!--⇓-->
 <xsl:when test="$n = 'hArr'">&#8660;</xsl:when><!--⇔-->

 <xsl:when test="$n = 'prec'">&#x227A;</xsl:when><!--≺-->
 <xsl:when test="$n = 'succ'">&#x227B;</xsl:when><!--≻-->

 <xsl:when test="$n = 'cr'">&#8629;</xsl:when><!--↵-->

 <xsl:otherwise>
  <xsl:value-of select="text()"/>
 </xsl:otherwise>
</xsl:choose>
</span>
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
