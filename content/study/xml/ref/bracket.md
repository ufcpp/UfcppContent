---
title: "括弧"
source_url: "https://ufcpp.net/study/xml/ref/bracket/"
content_type: "Article"
published_at: "2015-05-06T14:24:41"
updated_at: "2015-05-06T14:24:41"
tags: []
umbraco_id: 1667
parent_id: 1661
sort_order: 5
aliases:
  - "/ref/bracket"
  - "/ref/bracket.html"
  - "/study/ref/bracket"
  - "/study/ref/bracket.html"
  - "/xml/ref/bracket/"
---

# 括弧

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

括弧の表示。
(a), {a}, &lt;a&gt;, [a], |a|, ||a|| など、全て bracket にまとめました。
（短縮形 bra）
（type: paren (round), brace (curl), angle, square(sq), abs, norm, ceil, floor）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<bracket size="括弧の大きさ" type="type">括弧内の式</bracket>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<bracket>x</bracket> = <inv>N</inv><Sigma><sub>i</sub><sup>N</sup></Sigma>x<sub>i</sub>
```
<div class="math"><span class="paren" style="font-size:em;">〈</span>x<span class="paren" style="font-size:em;">〉</span> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table>x<sub>i</sub>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:math//ufcpp:bracket|ufcpp:math//ufcpp:bra|ufcpp:Math//ufcpp:bracket|ufcpp:Math//ufcpp:bra">
<xsl:variable name="t"><xsl:choose><xsl:when test="@type != ''"><xsl:value-of select="@type"/></xsl:when><xsl:otherwise><xsl:value-of select="@t"/></xsl:otherwise></xsl:choose></xsl:variable>

<xsl:variable name="l">
 <xsl:choose>
  <xsl:when test="$t = 'p'">(</xsl:when>
  <xsl:when test="$t = 'paren'">(</xsl:when>
  <xsl:when test="$t = 'r'">(</xsl:when>
  <xsl:when test="$t = 'round'">(</xsl:when>
  <xsl:when test="$t = 'c'">{</xsl:when>
  <xsl:when test="$t = 'curl'">{</xsl:when>
  <xsl:when test="$t = 'b'">{</xsl:when>
  <xsl:when test="$t = 'brace'">{</xsl:when>
  <xsl:when test="$t = 'angle'">&#9001;</xsl:when>
  <xsl:when test="$t = 'square'">[</xsl:when>
  <xsl:when test="$t = 'sq'">[</xsl:when>
  <xsl:when test="$t = 'abs'">|</xsl:when>
  <xsl:when test="$t = 'norm'">||</xsl:when>
  <xsl:when test="$t = 'ceil'">&#8968;</xsl:when>
  <xsl:when test="$t = 'floor'">&#8970;</xsl:when>
  <xsl:otherwise>&#9001;</xsl:otherwise>
 </xsl:choose>
</xsl:variable>

<xsl:variable name="r">
 <xsl:choose>
  <xsl:when test="$t = 'p'">)</xsl:when>
  <xsl:when test="$t = 'paren'">)</xsl:when>
  <xsl:when test="$t = 'r'">)</xsl:when>
  <xsl:when test="$t = 'round'">)</xsl:when>
  <xsl:when test="$t = 'c'">}</xsl:when>
  <xsl:when test="$t = 'curl'">}</xsl:when>
  <xsl:when test="$t = 'b'">}</xsl:when>
  <xsl:when test="$t = 'brace'">}</xsl:when>
  <xsl:when test="$t = 'angle'">&#9002;</xsl:when>
  <xsl:when test="$t = 'square'">]</xsl:when>
  <xsl:when test="$t = 'sq'">]</xsl:when>
  <xsl:when test="$t = 'abs'">|</xsl:when>
  <xsl:when test="$t = 'norm'">||</xsl:when>
  <xsl:when test="$t = 'ceil'">&#8969;</xsl:when>
  <xsl:when test="$t = 'floor'">&#8971;</xsl:when>
  <xsl:otherwise>&#9002;</xsl:otherwise>
 </xsl:choose>
</xsl:variable>

  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    <xsl:value-of select="$l"/>
  </span>
  <xsl:apply-templates/>
  <span class="paren">
    <xsl:attribute name="style">font-size:<xsl:value-of select="@size"/>em;</xsl:attribute>
    <xsl:value-of select="$r"/>
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
```
