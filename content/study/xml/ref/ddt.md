---
title: "時間微分"
source_url: "https://ufcpp.net/study/xml/ref/ddt/"
content_type: "Article"
published_at: "2015-05-06T14:24:48"
updated_at: "2015-05-06T14:24:48"
tags: []
umbraco_id: 1671
parent_id: 1661
sort_order: 9
aliases:
  - "/ref/ddt"
  - "/ref/ddt.html"
  - "/study/ref/ddt"
  - "/study/ref/ddt.html"
  - "/xml/ref/ddt/"
---

# 時間微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

時間微分記号d/dtを表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;ddt/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;ddt/&gt;x = -kx
</pre><div class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x = -kx
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:ddt"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;&lt;span class="normal"&gt;d&lt;/span&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@var != ''"&gt;&lt;xsl:value-of select="@var"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@v != ''"&gt;&lt;xsl:value-of select="@v"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;&lt;span class="normal"&gt;d&lt;/span&gt;t&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:math//ufcpp:dd|ufcpp:Math//ufcpp:dd"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;&lt;span class="normal"&gt;d&lt;/span&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@num != ''"&gt;&lt;xsl:value-of select="@num"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@n != ''"&gt;&lt;xsl:value-of select="@n"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:num"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;&lt;span class="normal"&gt;d&lt;/span&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@denom != ''"&gt;&lt;xsl:value-of select="@denom"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@d != ''"&gt;&lt;xsl:value-of select="@d"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:denom"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
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

span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
