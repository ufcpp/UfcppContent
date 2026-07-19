---
title: "偏角"
source_url: "https://ufcpp.net/study/xml/ref/arg/"
content_type: "Article"
published_at: "2015-05-06T14:24:35"
updated_at: "2015-05-06T14:24:35"
tags: []
umbraco_id: 1664
parent_id: 1661
sort_order: 2
aliases:
  - "/ref/arg"
  - "/ref/arg.html"
  - "/study/ref/arg"
  - "/study/ref/arg.html"
  - "/xml/ref/arg/"
---

# 偏角

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
偏角を表す記号を表示


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;arg/&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;arg/&gt;z = nπi
</pre><div class="math"><span class="normal">arg</span>z = nπi
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:arg"&gt;
  &lt;span class="normal"&gt;arg&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
