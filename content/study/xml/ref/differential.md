---
title: "微分"
source_url: "https://ufcpp.net/study/xml/ref/differential/"
content_type: "Article"
published_at: "2015-05-06T14:24:50"
updated_at: "2015-05-06T14:24:50"
tags: []
umbraco_id: 1672
parent_id: 1661
sort_order: 10
aliases:
  - "/ref/differential"
  - "/ref/differential.html"
  - "/study/ref/differential"
  - "/study/ref/differential.html"
  - "/xml/ref/differential/"
---

# 微分

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

d/dx や ∂/∂x 等を表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;dd var="x" func="y"/&gt; &lt;pdd var="x" func="y/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;dd var="s"/&gt;f
＝
&lt;pdd var="x" func="f"/&gt;&lt;d/&gt;x
＋
&lt;pdd var="y" func="f"/&gt;&lt;d/&gt;y
＋
&lt;pdd var="y" func="f"/&gt;&lt;d/&gt;x
</pre><div class="math"><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span></td></tr></table>f
＝
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>x
＋
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>y
＋
<table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂</td></tr></table><span class="normal">d</span>x
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:dd"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;&lt;span class="normal"&gt;d&lt;/span&gt;&lt;xsl:value-of select="@func"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;&lt;span class="normal"&gt;d&lt;/span&gt;
    &lt;xsl:choose&gt;
      &lt;xsl:when test="@var != ''"&gt;
        &lt;xsl:value-of select="@var"/&gt;
      &lt;/xsl:when&gt;
      &lt;xsl:otherwise&gt;
        &lt;xsl:apply-templates/&gt;
      &lt;/xsl:otherwise&gt;
    &lt;/xsl:choose&gt;
    &lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:pdd"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;��&lt;xsl:value-of select="@func"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;��
    &lt;xsl:choose&gt;
      &lt;xsl:when test="@var != ''"&gt;
        &lt;xsl:value-of select="@var"/&gt;
      &lt;/xsl:when&gt;
      &lt;xsl:otherwise&gt;
        &lt;xsl:apply-templates/&gt;
      &lt;/xsl:otherwise&gt;
    &lt;/xsl:choose&gt;
    &lt;/td&gt;&lt;/tr&gt;
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
