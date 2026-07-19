---
title: "三角関数"
source_url: "https://ufcpp.net/study/xml/ref/sin/"
content_type: "Article"
published_at: "2015-05-06T14:25:34"
updated_at: "2015-05-06T14:25:34"
tags: []
umbraco_id: 1691
parent_id: 1661
sort_order: 29
aliases:
  - "/ref/sin"
  - "/ref/sin.html"
  - "/study/ref/sin"
  - "/study/ref/sin.html"
  - "/xml/ref/sin/"
---

# 三角関数

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
sin,cos,tanの表示


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;sin/&gt; &lt;cos/&gt; &lt;tan/&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;sin/&gt;x = &lt;cos/&gt;(x-&lt;frac&gt;&lt;num&gt;π&lt;/num&gt;&lt;denom&gt;2&lt;/denom&gt;&lt;/frac&gt;),
&lt;tan/&gt;x = &lt;frac&gt;&lt;num&gt;&lt;sin/&gt;x&lt;/num&gt;&lt;denom&gt;&lt;cos/&gt;x&lt;/denom&gt;&lt;/frac&gt;
</pre><div class="math"><span class="normal">sin</span>x = <span class="normal">cos</span>(x-<table class="frac" summary="fraction"><tr><td class="num">π</td></tr><tr><td>2</td></tr></table>),
<span class="normal">tan</span>x = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">sin</span>x</td></tr><tr><td><span class="normal">cos</span>x</td></tr></table>
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:sin"&gt;
  &lt;span class="normal"&gt;sin&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:cos"&gt;
  &lt;span class="normal"&gt;cos&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:tan"&gt;
  &lt;span class="normal"&gt;tan&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
