---
title: "微分のd"
source_url: "https://ufcpp.net/study/xml/ref/d/"
content_type: "Article"
published_at: "2015-05-06T14:24:46"
updated_at: "2015-05-06T14:24:46"
tags: []
umbraco_id: 1670
parent_id: 1661
sort_order: 8
aliases:
  - "/ref/d"
  - "/ref/d.html"
  - "/study/ref/d"
  - "/study/ref/d.html"
  - "/xml/ref/d/"
---

# 微分のd

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

微分記号のdを表示する(微分のdはブロック体で表記すべき)


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;d/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;d/&gt;x = &lt;cos/&gt;t&lt;d/&gt;t
</pre><div class="math"><span class="normal">d</span>x = <span class="normal">cos</span>t<span class="normal">d</span>t
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:d"&gt;
  &lt;span class="normal"&gt;d&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
