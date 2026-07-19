---
title: "∑"
source_url: "https://ufcpp.net/study/xml/ref/sigma/"
content_type: "Article"
published_at: "2015-05-06T14:25:32"
updated_at: "2015-05-06T14:25:32"
tags: []
umbraco_id: 1690
parent_id: 1661
sort_order: 28
aliases:
  - "/ref/Sigma"
  - "/ref/Sigma.html"
  - "/study/ref/Sigma"
  - "/study/ref/Sigma.html"
  - "/xml/ref/sigma/"
---

# ∑

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
和の記号∑を表示


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;Sigma&gt;&lt;sub&gt;∑の下にくる式&lt;/sub&gt;&lt;sup&gt;∑の上にくる式&lt;/sup&gt;&lt;/Sigma&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>f(a) = 
&lt;Sigma&gt;&lt;sub&gt;n=0&lt;/sub&gt;&lt;sup&gt;∞&lt;/sup&gt;&lt;/Sigma&gt;
&lt;frac&gt;&lt;num&gt;f&lt;sup&gt;(n)&lt;/sup&gt;(a)&lt;/num&gt;&lt;denom&gt;n&lt;factorial/&gt;&lt;/denom&gt;&lt;/frac&gt;
&lt;paren&gt;z-a&lt;/paren&gt;&lt;sup&gt;n&lt;/sup&gt;
</pre><div class="math">f(a) = 
<table class="sigma" summary="sum"><tr><td class="sigmasub">∞</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">n=0</td></tr></table>
<table class="frac" summary="fraction"><tr><td class="num">f<sup>(n)</sup>(a)</td></tr><tr><td>n<span class="normal">!</span></td></tr></table>
<span class="paren" style="font-size:em;">(</span>z-a<span class="paren" style="font-size:em;">)</span><sup>n</sup>
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:Sigma"&gt;
  &lt;table class="sigma" summary="sum"&gt;
    &lt;tr&gt;&lt;td class="sigmasub"&gt;&lt;xsl:apply-templates select="ufcpp:sup"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="sigma"&gt;&amp;#8721;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="sigmasub"&gt;&lt;xsl:apply-templates select="ufcpp:sub"/&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:Sigma/ufcpp:sup|ufcpp:Pi/ufcpp:sup|ufcpp:Sigma/ufcpp:sub|ufcpp:Pi/ufcpp:sub"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre>table.sigma
{
  display:inline;
  text-align:center;
  vertical-align:middle;
  font-style:italic;
}

td.sigma
{
  font-style:normal;
  font-size:120%;
}

td.sigmasub
{
  font-size:70%;
}

</pre>
