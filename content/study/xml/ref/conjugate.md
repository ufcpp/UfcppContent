---
title: "複素共役"
source_url: "https://ufcpp.net/study/xml/ref/conjugate/"
content_type: "Article"
published_at: "2015-05-06T14:24:44"
updated_at: "2015-05-06T14:24:44"
tags: []
umbraco_id: 1669
parent_id: 1661
sort_order: 7
aliases:
  - "/ref/conjugate"
  - "/ref/conjugate.html"
  - "/study/ref/conjugate"
  - "/study/ref/conjugate.html"
  - "/xml/ref/conjugate/"
---

# 複素共役

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
共役複素数(右上に*を付ける)


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;conjugate&gt;共役にしたい式&lt;/conjugate&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>z=a+ib, &lt;conjugate&gt;z&lt;/conjugate&gt;=a&amp;#x2212;ib
</pre><div class="math">z=a+ib, z<sup>*</sup>=a−ib
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:conjugate"&gt;
  &lt;xsl:apply-templates/&gt;&lt;sup&gt;*&lt;/sup&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre></pre>
