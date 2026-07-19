---
title: "行列"
source_url: "https://ufcpp.net/study/xml/ref/matrix/"
content_type: "Article"
published_at: "2015-05-06T14:25:09"
updated_at: "2015-05-06T14:25:09"
tags: []
umbraco_id: 1681
parent_id: 1661
sort_order: 19
aliases:
  - "/ref/matrix"
  - "/ref/matrix.html"
  - "/study/ref/matrix"
  - "/study/ref/matrix.html"
  - "/xml/ref/matrix/"
---

# 行列

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

テーブル状の行列を表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;matrix size="行列の高さ"&gt;
  &lt;row&gt;&lt;elem&gt;1,1成分&lt;/elem&gt;...&lt;elem&gt;1,n成分&lt;/elem&gt;
  .
  .
  .
  &lt;row&gt;&lt;elem&gt;m,1成分&lt;/elem&gt;...&lt;elem&gt;m,n成分&lt;/elem&gt;
&lt;/matrix&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;vervec size="3"&gt;&lt;elem&gt;x'&lt;/elem&gt;&lt;elem&gt;y'&lt;/elem&gt;&lt;elem&gt;z'&lt;/elem&gt;&lt;/vervec&gt; = 
&lt;matrix size="3"&gt;
&lt;row&gt;&lt;elem&gt;a&lt;/elem&gt;&lt;elem&gt;b&lt;/elem&gt;&lt;elem&gt;c&lt;/elem&gt;&lt;/row&gt;
&lt;row&gt;&lt;elem&gt;d&lt;/elem&gt;&lt;elem&gt;e&lt;/elem&gt;&lt;elem&gt;f&lt;/elem&gt;&lt;/row&gt;
&lt;row&gt;&lt;elem&gt;g&lt;/elem&gt;&lt;elem&gt;h&lt;/elem&gt;&lt;elem&gt;i&lt;/elem&gt;&lt;/row&gt;
&lt;/matrix&gt;
&lt;vervec size="3"&gt;&lt;elem&gt;x&lt;/elem&gt;&lt;elem&gt;y&lt;/elem&gt;&lt;elem&gt;z&lt;/elem&gt;&lt;/vervec&gt;
</pre><div class="math"><span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="vector"><tr><td>x'</td></tr><tr><td>y'</td></tr><tr><td>z'</td></tr></table><span class="paren" style="font-size:3em;">]</span> = 
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="matrix"><tr><td>a</td><td>b</td><td>c</td></tr><tr><td>d</td><td>e</td><td>f</td></tr><tr><td>g</td><td>h</td><td>i</td></tr></table><span class="paren" style="font-size:3em;">]</span>
<span class="paren" style="font-size:3em;">[</span><table class="matrix" summary="vector"><tr><td>x</td></tr><tr><td>y</td></tr><tr><td>z</td></tr></table><span class="paren" style="font-size:3em;">]</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:matrix"&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;table class="matrix" summary="matrix"&gt;
    &lt;xsl:apply-templates select="ufcpp:row"/&gt;
  &lt;/table&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size" /&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:array"&gt;
  &lt;table class="matrix" summary="array"&gt;
    &lt;xsl:apply-templates select="ufcpp:row"/&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:matrix/ufcpp:row|ufcpp:array/ufcpp:row"&gt;
  &lt;tr&gt;&lt;xsl:apply-templates select="ufcpp:elem"/&gt;&lt;/tr&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:matrix/ufcpp:row/ufcpp:elem|ufcpp:array/ufcpp:row/ufcpp:elem"&gt;
  &lt;td&gt;&lt;xsl:apply-templates/&gt;&lt;/td&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>table.matrix
{
  display:inline;
  font-style:italic;
  text-align:center;
  vertical-align:bottom;
  vertical-align:middle;
}

span.paren
{
  font-style:normal;
  vertical-align:middle;
}

</pre>
