---
title: "lim"
source_url: "https://ufcpp.net/study/xml/ref/limit/"
content_type: "Article"
published_at: "2015-05-06T14:25:05"
updated_at: "2015-05-06T14:25:05"
tags: []
umbraco_id: 1679
parent_id: 1661
sort_order: 17
aliases:
  - "/ref/limit"
  - "/ref/limit.html"
  - "/study/ref/limit"
  - "/study/ref/limit.html"
  - "/xml/ref/limit/"
---

# lim

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

極限記号limを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;lim&gt;lim記号の下に来る式&lt;/lim&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;frac&gt;&lt;num&gt;&lt;d/&gt;f&lt;/num&gt;&lt;denom&gt;&lt;d/&gt;x&lt;/denom&gt;&lt;/frac&gt; = 
&lt;lim&gt;Δx→0&lt;/lim&gt;&lt;frac&gt;&lt;num&gt;f(x+Δx) &amp;#x2212; f(x)&lt;/num&gt;&lt;denom&gt;Δx&lt;/denom&gt;&lt;/frac&gt;
</pre><div class="math"><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">d</span>f</td></tr><tr><td><span class="normal">d</span>x</td></tr></table> = 
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">Δx→0</td></tr></table><table class="frac" summary="fraction"><tr><td class="num">f(x+Δx) − f(x)</td></tr><tr><td>Δx</td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:lim"&gt;
  &lt;table class="sigma" summary="limitation"&gt;
    &lt;tr&gt;&lt;td&gt;&lt;span class="normal"&gt;lim&lt;/span&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="sigmasub"&gt;&lt;xsl:apply-templates/&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>table.sigma
{
  display:inline;
  text-align:center;
  vertical-align:middle;
  font-style:italic;
}

td.sigmasub
{
  font-size:70%;
}

</pre>
