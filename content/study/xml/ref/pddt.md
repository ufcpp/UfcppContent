---
title: "時間偏微分"
source_url: "https://ufcpp.net/study/xml/ref/pddt/"
content_type: "Article"
published_at: "2015-05-06T14:25:19"
updated_at: "2015-05-06T14:25:19"
tags: []
umbraco_id: 1685
parent_id: 1661
sort_order: 23
aliases:
  - "/ref/pddt"
  - "/ref/pddt.html"
  - "/study/ref/pddt"
  - "/study/ref/pddt.html"
  - "/xml/ref/pddt/"
---

# 時間偏微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

時間微分記号∂/∂tを表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;pddt/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;pddt/&gt;T = α&lt;nabra/&gt;&lt;sup&gt;2&lt;/sup&gt;T
</pre><div class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂t</td></tr></table>T = α<span class="vector">∇</span><sup>2</sup>T
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:pddt"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;∂&lt;xsl:choose&gt;&lt;xsl:when test="@var != ''"&gt;&lt;xsl:value-of select="@var"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@v != ''"&gt;&lt;xsl:value-of select="@v"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;∂t&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:math//ufcpp:pdd|ufcpp:Math//ufcpp:pdd"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;∂&lt;xsl:choose&gt;&lt;xsl:when test="@num != ''"&gt;&lt;xsl:value-of select="@num"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@n != ''"&gt;&lt;xsl:value-of select="@n"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:num"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;∂&lt;xsl:choose&gt;&lt;xsl:when test="@denom != ''"&gt;&lt;xsl:value-of select="@denom"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@d != ''"&gt;&lt;xsl:value-of select="@d"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:denom"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>table.frac
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

</pre>
