---
title: "条件分岐"
source_url: "https://ufcpp.net/study/xml/ref/branch/"
content_type: "Article"
published_at: "2015-05-06T14:24:42"
updated_at: "2015-05-06T14:24:42"
tags: []
umbraco_id: 1668
parent_id: 1661
sort_order: 6
aliases:
  - "/ref/branch"
  - "/ref/branch.html"
  - "/study/ref/branch"
  - "/study/ref/branch.html"
  - "/xml/ref/branch/"
---

# 条件分岐

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

条件によって値の違う式を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;branch size="条件の数"&gt;
  &lt;case&gt;&lt;equ&gt;式1&lt;/equ&gt;&lt;cond&gt;条件1&lt;/cond&gt;&lt;/case&gt;
  &lt;case&gt;&lt;equ&gt;式2&lt;/equ&gt;&lt;cond&gt;条件2&lt;/cond&gt;&lt;/case&gt;
  .
  .
  .
  &lt;case&gt;&lt;equ&gt;式n&lt;/equ&gt;&lt;cond&gt;条件n&lt;/cond&gt;&lt;/case&gt;
&lt;/branch&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;abs&gt;x&lt;/abs&gt;=
&lt;branch size="2"&gt;
  &lt;case&gt;&lt;equ&gt;x&lt;/equ&gt;&lt;cond&gt;&lt;math&gt;x≧0&lt;/math&gt;&lt;/cond&gt;&lt;/case&gt;
  &lt;case&gt;&lt;equ&gt;&amp;#x2212;x&lt;/equ&gt;&lt;cond&gt;&lt;math&gt;x&amp;lt;0&lt;/math&gt;&lt;/cond&gt;&lt;/case&gt;
&lt;/branch&gt;
</pre><div class="math"><span class="normal">|</span>x<span class="normal">|</span>=
<span class="paren" style="font-size:2em;">{</span><table class="branch" summary="conditional"><tr><td><span class="math">x</span>  </td><td><span class="paren">(</span><span class="math"><span class="math">x≧0</span></span><span class="paren">)</span></td></tr><tr><td><span class="math">−x</span>  </td><td><span class="paren">(</span><span class="math"><span class="math">x&lt;0</span></span><span class="paren">)</span></td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:branch"&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    {
  &lt;/span&gt;
  &lt;table class="branch" summary="conditional"&gt;
    &lt;xsl:for-each select="ufcpp:case"&gt;
      &lt;tr&gt;
        &lt;td&gt;&lt;span class="math"&gt;&lt;xsl:apply-templates select="ufcpp:equ"/&gt;&lt;/span&gt;&amp;#xA0;&amp;#xA0;&lt;/td&gt;
        &lt;td&gt;
          &lt;span class="paren"&gt;(&lt;/span&gt;
          &lt;span class="math"&gt;&lt;xsl:apply-templates select="ufcpp:cond"/&gt;&lt;/span&gt;
          &lt;span class="paren"&gt;)&lt;/span&gt;
        &lt;/td&gt;
      &lt;/tr&gt;
    &lt;/xsl:for-each&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:branch/ufcpp:case/ufcpp:equ"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:branch/ufcpp:case/ufcpp:cond"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>table.branch
{
  display:inline;
  font-style:italic;
  vertical-align:bottom;
  vertical-align:middle;
}

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

</pre>
