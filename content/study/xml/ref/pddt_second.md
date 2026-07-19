---
title: "2階時間偏微分"
source_url: "https://ufcpp.net/study/xml/ref/pddt_second/"
content_type: "Article"
published_at: "2015-05-06T14:25:23"
updated_at: "2015-05-06T14:25:23"
tags: []
umbraco_id: 1686
parent_id: 1661
sort_order: 24
aliases:
  - "/ref/pddt_second"
  - "/ref/pddt_second.html"
  - "/study/ref/pddt_second"
  - "/study/ref/pddt_second.html"
  - "/xml/ref/pddt_second/"
---

# 2階時間偏微分

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
2階時間微分記号(∂/∂t)<sup>2</sup>を表示する


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;pddt_second/&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;pddt_second/&gt;φ = c&lt;sup&gt;2&lt;/sup&gt;&lt;nabra/&gt;&lt;sup&gt;2&lt;/sup&gt;φ
</pre><div class="math"><table class="frac" summary="differential"><tr><td class="num">∂<sup>2</sup></td></tr><tr><td>∂t<sup>2</sup></td></tr></table>φ = c<sup>2</sup><span class="vector">∇</span><sup>2</sup>φ
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:pddt_second"&gt;
  &lt;table class="frac" summary="differential"&gt;
    &lt;tr&gt;&lt;td class="num"&gt;∂&lt;sup&gt;2&lt;/sup&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;∂t&lt;sup&gt;2&lt;/sup&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
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

</pre>
