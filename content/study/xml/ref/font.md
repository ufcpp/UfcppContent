---
title: "書体"
source_url: "https://ufcpp.net/study/xml/ref/font/"
content_type: "Article"
published_at: "2015-05-06T14:24:55"
updated_at: "2015-05-06T14:24:55"
tags: []
umbraco_id: 1675
parent_id: 1661
sort_order: 13
aliases:
  - "/ref/font"
  - "/ref/font.html"
  - "/study/ref/font"
  - "/study/ref/font.html"
  - "/xml/ref/font/"
---

# 書体

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

通常の文字列（text）、
太字（bold）、
筆記体（cursive）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;text&gt;通常の文字列&lt;/text&gt; &lt;bold&gt;太字&lt;/bold&gt; &lt;cursive&gt;筆記体&lt;/cursive&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>α ∈ &lt;bold&gt;C&lt;/bold&gt;,
&lt;cursive&gt;Re&lt;/cursive&gt;&lt;paren&gt;α&lt;/paren&gt; ∈ &lt;bold&gt;R&lt;/bold&gt;
&lt;text&gt;（C や R は太字、Re は筆記体で書く。）&lt;/text&gt;
</pre><div class="math">α ∈ <span class="bold">C</span>,
<span class="cursive">Re</span><span class="paren" style="font-size:em;">(</span>α<span class="paren" style="font-size:em;">)</span> ∈ <span class="bold">R</span>
<span class="normal">（C や R は太字、Re は筆記体で書く。）</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:text"&gt;
  &lt;span class="normal"&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:bold"&gt;
  &lt;span class="bold"&gt;&lt;xsl:apply-templates/&gt;&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:cursive"&gt;
  &lt;span class="cursive"&gt;&lt;xsl:apply-templates/&gt;&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>
span.bold
{
  font-weight:bold;
  font-style:normal;
}


</pre>
