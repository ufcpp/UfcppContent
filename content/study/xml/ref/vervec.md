---
title: "縦ベクトル"
source_url: "https://ufcpp.net/study/xml/ref/vervec/"
content_type: "Article"
published_at: "2015-05-06T14:25:47"
updated_at: "2015-05-06T14:25:47"
tags: []
umbraco_id: 1698
parent_id: 1661
sort_order: 36
aliases:
  - "/ref/vervec"
  - "/ref/vervec.html"
  - "/study/ref/vervec"
  - "/study/ref/vervec.html"
  - "/xml/ref/vervec/"
---

# 縦ベクトル

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

縦ベクトルを表示する


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;vervec size="ベクトルの要素数"&gt;&lt;elem&gt;要素1&lt;/elem&gt;...&lt;elem&gt;要素n&lt;elem&gt;&lt;/vervec&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;vec&gt;r&lt;/vec&gt; = &lt;vervec size="2"&gt;&lt;elem&gt;x&lt;/elem&gt;&lt;elem&gt;y&lt;/elem&gt;&lt;/vervec&gt;
</pre><div class="math"><span class="vector">r</span> = <span class="paren" style="font-size:2em;">[</span><table class="matrix" summary="vector"><tr><td>x</td></tr><tr><td>y</td></tr></table><span class="paren" style="font-size:2em;">]</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:vervec"&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;table class="matrix" summary="vector"&gt;
    &lt;xsl:apply-templates select="ufcpp:elem"/&gt;
  &lt;/table&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size" /&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:vervec/ufcpp:elem"&gt;
  &lt;tr&gt;&lt;td&gt;&lt;xsl:apply-templates/&gt;&lt;/td&gt;&lt;/tr&gt;
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
