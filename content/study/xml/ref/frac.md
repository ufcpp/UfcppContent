---
title: "分数"
source_url: "https://ufcpp.net/study/xml/ref/frac/"
content_type: "Article"
published_at: "2015-05-06T14:25:00"
updated_at: "2015-05-06T14:25:00"
tags: []
umbraco_id: 1677
parent_id: 1661
sort_order: 15
aliases:
  - "/ref/frac"
  - "/ref/frac.html"
  - "/study/ref/frac"
  - "/study/ref/frac.html"
  - "/xml/ref/frac/"
---

# 分数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

分数を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;frac&gt;
  &lt;num&gt;分子&lt;/num&gt;
  &lt;denom&gt;分母&lt;/denom&gt;
&lt;/frac&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;frac&gt;&lt;num&gt;f(b)&amp;#x2212;f(a)&lt;/num&gt;&lt;denom&gt;b&amp;#x2212;a&lt;/denom&gt;&lt;/frac&gt; = f'(c)
</pre><div class="math"><table class="frac" summary="fraction"><tr><td class="num">f(b)−f(a)</td></tr><tr><td>b−a</td></tr></table> = f'(c)
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:frac"&gt;

  &lt;table class="frac" summary="fraction"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@num != ''"&gt;&lt;xsl:value-of select="@num"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@n != ''"&gt;&lt;xsl:value-of select="@n"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:num"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@denom != ''"&gt;&lt;xsl:value-of select="@denom"/&gt;&lt;/xsl:when&gt;&lt;xsl:when test="@d != ''"&gt;&lt;xsl:value-of select="@d"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:apply-templates select="ufcpp:denom"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:frac/ufcpp:num"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:frac/ufcpp:denom"&gt;
  &lt;xsl:apply-templates/&gt;
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
