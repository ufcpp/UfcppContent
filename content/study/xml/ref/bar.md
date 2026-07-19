---
title: "バー"
source_url: "https://ufcpp.net/study/xml/ref/bar/"
content_type: "Article"
published_at: "2015-05-06T14:24:37"
updated_at: "2015-05-06T14:24:37"
tags: []
umbraco_id: 1665
parent_id: 1661
sort_order: 3
aliases:
  - "/ref/bar"
  - "/ref/bar.html"
  - "/study/ref/bar"
  - "/study/ref/bar.html"
  - "/xml/ref/bar/"
---

# バー

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

変数を上線付きにする


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;bar&gt;上線つきにしたい式&lt;/bar&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>a XOR b = a&lt;bar&gt;b&lt;/bar&gt;+&lt;bar&gt;a&lt;/bar&gt;b
</pre><div class="math">a XOR b = a<span class="bar">b</span>+<span class="bar">a</span>b
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:bar"&gt;
  &lt;span class="bar"&gt;
    &lt;xsl:apply-templates/&gt;
  &lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.bar
{
  display:inline-block;
  border-top:1pt solid #000000;
}

</pre>
