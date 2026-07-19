---
title: "Π"
source_url: "https://ufcpp.net/study/xml/ref/pi/"
content_type: "Article"
published_at: "2015-05-06T14:25:26"
updated_at: "2015-05-06T14:25:26"
tags: []
umbraco_id: 1687
parent_id: 1661
sort_order: 25
aliases:
  - "/ref/Pi"
  - "/ref/Pi.html"
  - "/study/ref/Pi"
  - "/study/ref/Pi.html"
  - "/xml/ref/pi/"
---

# Π

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

積の記号Πを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;Pi&gt;&lt;sub&gt;Πの下にくる式&lt;/sub&gt;&lt;sup&gt;Πの上にくる式&lt;/sup&gt;&lt;/Pi&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>n&lt;factorial/&gt; = &lt;Pi&gt;&lt;sub&gt;k=0&lt;/sub&gt;&lt;sup&gt;n&lt;/sup&gt;&lt;/Pi&gt;k
</pre><div class="math">n<span class="normal">!</span> = <table class="sigma" summary="product"><tr><td class="sigmasub">n</td></tr><tr><td class="sigma">∏</td></tr><tr><td class="sigmasub">k=0</td></tr></table>k
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:Pi"&gt;
  &lt;table class="sigma" summary="product"&gt;
    &lt;tr&gt;&lt;td class="sigmasub"&gt;&lt;xsl:apply-templates select="ufcpp:sup"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="sigma"&gt;&amp;#8719;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="sigmasub"&gt;&lt;xsl:apply-templates select="ufcpp:sub"/&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:Sigma/ufcpp:sup|ufcpp:Pi/ufcpp:sup|ufcpp:Sigma/ufcpp:sub|ufcpp:Pi/ufcpp:sub"&gt;
  &lt;xsl:apply-templates/&gt;
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
