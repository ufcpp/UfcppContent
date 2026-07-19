---
title: "指数・対数"
source_url: "https://ufcpp.net/study/xml/ref/log/"
content_type: "Article"
published_at: "2015-05-06T14:25:07"
updated_at: "2015-05-06T14:25:07"
tags: []
umbraco_id: 1680
parent_id: 1661
sort_order: 18
aliases:
  - "/ref/log"
  - "/ref/log.html"
  - "/study/ref/log"
  - "/study/ref/log.html"
  - "/xml/ref/log/"
---

# 指数・対数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

log,expの表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;log/&gt; &lt;exp/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;exp/&gt;At = &lt;Sigma&gt;&lt;sub&gt;n=0&lt;/sub&gt;&lt;sup&gt;∞&lt;/sup&gt;&lt;/Sigma&gt;&lt;inv&gt;n&lt;factorial/&gt;&lt;/inv&gt;A&lt;sup&gt;n&lt;/sup&gt;t&lt;sup&gt;n&lt;/sup&gt;,
&lt;log/&gt;z = &lt;log/&gt;&lt;abs&gt;z&lt;/abs&gt; + i&lt;arg/&gt;z
</pre><div class="math"><span class="normal">exp</span>At = <table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>n<span class="normal">!</span></td></tr></table>A<sup>n</sup>t<sup>n</sup>,
<span class="normal">log</span>z = <span class="normal">log</span><span class="normal">|</span>z<span class="normal">|</span> + i<span class="normal">arg</span>z
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:log"&gt;
  &lt;span class="normal"&gt;log&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:exp"&gt;
  &lt;span class="normal"&gt;exp&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
