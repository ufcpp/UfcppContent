---
title: "記号"
source_url: "https://ufcpp.net/study/xml/ref/symbol/"
content_type: "Article"
published_at: "2015-05-06T14:25:39"
updated_at: "2015-05-06T14:25:39"
tags: []
umbraco_id: 1694
parent_id: 1661
sort_order: 32
aliases:
  - "/ref/symbol"
  - "/ref/symbol.html"
  - "/study/ref/symbol"
  - "/study/ref/symbol.html"
  - "/xml/ref/symbol/"
---

# 記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

記号類。
（短縮形: sym）
主に、MS IME の変換で出せない文字。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<symbol name="name"></symbol>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<symbol n="forall"/>x s.t. <symbol n="vtheta"/><paren>x</paren> <op>=</op> 0
```
<div class="math"><span class="normal">∀</span>x s.t. <span class="normal">ϑ</span><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <op>=</op> 0

</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:math//ufcpp:symbol|ufcpp:math//ufcpp:sym|ufcpp:Math//ufcpp:symbol|ufcpp:Math//ufcpp:sym">
<xsl:variable name="n"><xsl:choose><xsl:when test="@name != ''"><xsl:value-of select="@name"/></xsl:when><xsl:otherwise><xsl:value-of select="@n"/></xsl:otherwise></xsl:choose></xsl:variable>

<span class="normal">
<xsl:choose>
 <xsl:when test="$n = 'forall'">&#8704;</xsl:when><!--∀-->
 <xsl:when test="$n = 'exist'">&#8707;</xsl:when><!--∃-->
 <xsl:when test="$n = 'partial'">&#8706;</xsl:when><!--∂-->
 <xsl:when test="$n = 'part'">&#8706;</xsl:when><!--∂-->
 <xsl:when test="$n = 'nabla'">&#8711;</xsl:when><!--∇-->
 <xsl:when test="$n = 'infinity'">&#8734;</xsl:when><!--∞-->
 <xsl:when test="$n = 'infty'">&#8734;</xsl:when><!--∞-->
 <xsl:when test="$n = 'infin'">&#8734;</xsl:when><!--∞-->
 <xsl:when test="$n = 'ang'">&#8736;</xsl:when><!--∠-->
 <xsl:when test="$n = 'therefore'">&#8756;</xsl:when><!--∴-->
 <xsl:when test="$n = 'because'">&#8757;</xsl:when><!--∵-->

 <xsl:when test="$n = 'empty'">&#8709;</xsl:when><!--∅-->
 <xsl:when test="$n = 'weierp'">&#8472;</xsl:when><!--℘-->
 <xsl:when test="$n = 'image'">&#8465;</xsl:when><!--ℑ-->
 <xsl:when test="$n = 'real'">&#8476;</xsl:when><!--ℜ-->
 <xsl:when test="$n = 'alef'">&#8501;</xsl:when><!--ℵ-->
 <xsl:when test="$n = 'planck'">&#x210F;</xsl:when><!--ℏ-->

 <!-- ellipsis -->
 <xsl:when test="$n = 'dots'">&#x22EF;</xsl:when><!--⋯-->
 <xsl:when test="$n = 'vdots'">&#x22EE;</xsl:when><!--⋮-->
 <xsl:when test="$n = 'updots'">&#x22F0;</xsl:when><!--⋰-->
 <xsl:when test="$n = 'downdots'">&#x22F1;</xsl:when><!--⋱-->

 <!--ギリシャ文字異字体-->
 <xsl:when test="$n = 'vbeta'">&#976;</xsl:when>
 <xsl:when test="$n = 'vepsilon'">&#8714;</xsl:when>
 <xsl:when test="$n = 'vtheta'">&#977;</xsl:when>
 <xsl:when test="$n = 'vkappa'">&#1008;</xsl:when>
 <xsl:when test="$n = 'vpi'">&#982;</xsl:when>
 <xsl:when test="$n = 'vrho'">&#1009;</xsl:when>
 <xsl:when test="$n = 'vsigma'">&#962;</xsl:when>
 <xsl:when test="$n = 'vphi'">&#981;</xsl:when>

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
