---
title: "フーリエ変換など"
source_url: "https://ufcpp.net/study/xml/ref/fourier/"
content_type: "Article"
published_at: "2015-05-06T14:24:58"
updated_at: "2015-05-06T14:24:58"
tags: []
umbraco_id: 1676
parent_id: 1661
sort_order: 14
aliases:
  - "/ref/Fourier"
  - "/ref/Fourier.html"
  - "/study/ref/Fourier"
  - "/study/ref/Fourier.html"
  - "/xml/ref/fourier/"
---

# フーリエ変換など

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

フーリエ変換、ラプラス変換、Z変換の記号。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;Fourier&gt;変換元&lt;/Fourier&gt;
&lt;Laplace&gt;変換元&lt;/Laplace&gt;
&lt;Z&gt;変換元&lt;/Z&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;Fourier&gt;f&lt;paren&gt;t&lt;/paren&gt;&lt;/Fourier&gt;&lt;paren&gt;ω&lt;/paren&gt;, 
&lt;Laplace&gt;f&lt;paren&gt;t&lt;/paren&gt;&lt;/Laplace&gt;&lt;paren&gt;s&lt;/paren&gt;, 
&lt;Z&gt;f&lt;paren&gt;t&lt;/paren&gt;&lt;/Z&gt;&lt;paren&gt;z&lt;/paren&gt;
</pre><div class="math"><span class="normal">ℱ</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>ω<span class="paren" style="font-size:em;">)</span>, 
<span class="normal">ℒ</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>s<span class="paren" style="font-size:em;">)</span>, 
<span class="script">Z</span><span class="paren" style="font-size:em;">[</span>f<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:em;">]</span><span class="paren" style="font-size:em;">(</span>z<span class="paren" style="font-size:em;">)</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:Fourier"&gt;
  &lt;span class="normal"&gt;&amp;#x2131;&lt;/span&gt;
  &lt;xsl:if test="@inv!=''"&gt;
  &lt;sup&gt;�|1&lt;/sup&gt;
  &lt;/xsl:if&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:Laplace"&gt;
  &lt;span class="normal"&gt;&amp;#x2112;&lt;/span&gt;
  &lt;xsl:if test="@inv!=''"&gt;
  &lt;sup&gt;�|1&lt;/sup&gt;
  &lt;/xsl:if&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:Z"&gt;
  &lt;span class="script"&gt;
    Z
  &lt;/span&gt;
  &lt;xsl:if test="@inv!=''"&gt;
  &lt;sup&gt;�|1&lt;/sup&gt;
  &lt;/xsl:if&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.cursive
{
  font-family:cursive;
  font-style:italic;
  padding-right:0.2em;
}

span.paren
{
  font-style:normal;
  vertical-align:middle;
}

</pre>
