---
title: "演算子"
source_url: "https://ufcpp.net/study/xml/ref/operator/"
content_type: "Article"
published_at: "2015-05-06T14:25:13"
updated_at: "2015-05-06T14:25:13"
tags: []
umbraco_id: 1683
parent_id: 1661
sort_order: 21
aliases:
  - "/ref/operator"
  - "/ref/operator.html"
  - "/study/ref/operator"
  - "/study/ref/operator.html"
  - "/xml/ref/operator/"
---

# 演算子

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

演算子。
（短縮形: o）


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;operator name="name"&gt;operator&lt;/operator&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>x &lt;o&gt;+&lt;/o&gt; y,
x &lt;o name="pm"/&gt; y,
x &lt;o n="mp"/&gt; y
</pre><div class="math">x <span class="normal">+</span> y,
x <span class="normal">±</span> y,
x <span class="normal">∓</span> y
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:math//ufcpp:o|ufcpp:math//ufcpp:operator|ufcpp:Math//ufcpp:o|ufcpp:Math//ufcpp:operator"&gt;
&lt;xsl:variable name="n"&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@name != ''"&gt;&lt;xsl:value-of select="@name"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:value-of select="@n"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/xsl:variable&gt;

&lt;span class="normal"&gt;
&lt;xsl:choose&gt;
 &lt;xsl:when test="$n = 'in'"&gt;&amp;#8712;&lt;/xsl:when&gt;&lt;!--∈--&gt;
 &lt;xsl:when test="$n = 'nin'"&gt;&amp;#8713;&lt;/xsl:when&gt;&lt;!--∉--&gt;
 &lt;xsl:when test="$n = 'ni'"&gt;&amp;#8715;&lt;/xsl:when&gt;&lt;!--∋--&gt;
 &lt;xsl:when test="$n = 'nni'"&gt;&amp;#8716;&lt;/xsl:when&gt;&lt;!--∌--&gt;

 &lt;xsl:when test="$n = 'wedge'"&gt;&amp;#8743;&lt;/xsl:when&gt;&lt;!--∧--&gt;
 &lt;xsl:when test="$n = 'vee'"&gt;&amp;#8744;&lt;/xsl:when&gt;&lt;!--∨--&gt;
 &lt;xsl:when test="$n = 'cap'"&gt;&amp;#8745;&lt;/xsl:when&gt;&lt;!--∩--&gt;
 &lt;xsl:when test="$n = 'cup'"&gt;&amp;#8746;&lt;/xsl:when&gt;&lt;!--∪--&gt;

 &lt;xsl:when test="$n = 'sub'"&gt;&amp;#8834;&lt;/xsl:when&gt;&lt;!--⊂--&gt;
 &lt;xsl:when test="$n = 'sup'"&gt;&amp;#8835;&lt;/xsl:when&gt;&lt;!--⊃--&gt;
 &lt;xsl:when test="$n = 'nsub'"&gt;&amp;#8836;&lt;/xsl:when&gt;&lt;!--⊄--&gt;
 &lt;xsl:when test="$n = 'nsup'"&gt;&amp;#8837;&lt;/xsl:when&gt;&lt;!--⊅--&gt;
 &lt;xsl:when test="$n = 'sube'"&gt;&amp;#8838;&lt;/xsl:when&gt;&lt;!--⊆--&gt;
 &lt;xsl:when test="$n = 'supe'"&gt;&amp;#8839;&lt;/xsl:when&gt;&lt;!--⊇--&gt;

 &lt;xsl:when test="$n = 'perp'"&gt;&amp;#8869;&lt;/xsl:when&gt;&lt;!--⊥--&gt;
 &lt;xsl:when test="$n = 'para'"&gt;&amp;#8514;&lt;/xsl:when&gt;&lt;!--∥--&gt;

 &lt;xsl:when test="$n = 'eq'"&gt;=&lt;/xsl:when&gt;&lt;!--=--&gt;
 &lt;xsl:when test="$n = 'sim'"&gt;&amp;#8764;&lt;/xsl:when&gt;&lt;!--∼--&gt;
 &lt;xsl:when test="$n = 'approx'"&gt;&amp;#8773;&lt;/xsl:when&gt;&lt;!--≅--&gt;
 &lt;xsl:when test="$n = 'asymp'"&gt;&amp;#8776;&lt;/xsl:when&gt;&lt;!--≈--&gt;
 &lt;xsl:when test="$n = 'ne'"&gt;&amp;#8800;&lt;/xsl:when&gt;&lt;!--≠--&gt;
 &lt;xsl:when test="$n = 'equiv'"&gt;&amp;#8801;&lt;/xsl:when&gt;&lt;!--≡--&gt;
 &lt;xsl:when test="$n = 'prop'"&gt;&amp;#8733;&lt;/xsl:when&gt;&lt;!--∝--&gt;

 &lt;xsl:when test="$n = 'lt'"&gt;&amp;lt;&lt;/xsl:when&gt;&lt;!--&lt;--&gt;
 &lt;xsl:when test="$n = 'gt'"&gt;&amp;gt;&lt;/xsl:when&gt;&lt;!--&gt;--&gt;
 &lt;xsl:when test="$n = 'le'"&gt;&amp;#8804;&lt;/xsl:when&gt;&lt;!--≤--&gt;
 &lt;xsl:when test="$n = 'ge'"&gt;&amp;#8805;&lt;/xsl:when&gt;&lt;!--≥--&gt;
 &lt;xsl:when test="$n = 'lE'"&gt;&amp;#8806;&lt;/xsl:when&gt;&lt;!--≦--&gt;
 &lt;xsl:when test="$n = 'gE'"&gt;&amp;#8807;&lt;/xsl:when&gt;&lt;!--≧--&gt;
 &lt;xsl:when test="$n = 'lnE'"&gt;&amp;#8808;&lt;/xsl:when&gt;&lt;!--≨--&gt;
 &lt;xsl:when test="$n = 'gnE'"&gt;&amp;#8809;&lt;/xsl:when&gt;&lt;!--≩--&gt;
 &lt;xsl:when test="$n = 'Lt'"&gt;&amp;#8810;&lt;/xsl:when&gt;&lt;!--≪--&gt;
 &lt;xsl:when test="$n = 'Gt'"&gt;&amp;#8811;&lt;/xsl:when&gt;&lt;!--≫--&gt;

 &lt;xsl:when test="$n = 'p'"&gt;&amp;#43;&lt;/xsl:when&gt;&lt;!--+--&gt;
 &lt;xsl:when test="$n = 'plus'"&gt;&amp;#43;&lt;/xsl:when&gt;&lt;!--+--&gt;
 &lt;xsl:when test="$n = 'm'"&gt;&amp;#8722;&lt;/xsl:when&gt;&lt;!--−--&gt;
 &lt;xsl:when test="$n = 'minus'"&gt;&amp;#8722;&lt;/xsl:when&gt;&lt;!--−--&gt;
 &lt;xsl:when test="$n = 'times'"&gt;&amp;#215;&lt;/xsl:when&gt;&lt;!--×--&gt;
 &lt;xsl:when test="$n = 'div'"&gt;&amp;#247;&lt;/xsl:when&gt;&lt;!--÷--&gt;
 &lt;xsl:when test="$n = 'slash'"&gt;/&lt;/xsl:when&gt;&lt;!--/--&gt;
 &lt;xsl:when test="$n = 'mp'"&gt;&amp;#8723;&lt;/xsl:when&gt;&lt;!--∓--&gt;
 &lt;xsl:when test="$n = 'pm'"&gt;&amp;#177;&lt;/xsl:when&gt;&lt;!--±--&gt;

 &lt;xsl:when test="$n = 'oplus'"&gt;&amp;#8853;&lt;/xsl:when&gt;&lt;!--⊕--&gt;
 &lt;xsl:when test="$n = 'ominus'"&gt;&amp;#8854;&lt;/xsl:when&gt;&lt;!--⊖--&gt;
 &lt;xsl:when test="$n = 'otimes'"&gt;&amp;#8855;&lt;/xsl:when&gt;&lt;!--⊗--&gt;
 &lt;xsl:when test="$n = 'bs'"&gt;&amp;#8726;&lt;/xsl:when&gt;&lt;!--∖--&gt;

 &lt;xsl:when test="$n = 'dot'"&gt;&amp;#8901;&lt;/xsl:when&gt;&lt;!--⋅--&gt;
 &lt;xsl:when test="$n = 'cross'"&gt;&amp;#215;&lt;/xsl:when&gt;&lt;!--×--&gt;
 &lt;xsl:when test="$n = 'wedge'"&gt;&amp;#8743;&lt;/xsl:when&gt;&lt;!--∧--&gt;
 &lt;xsl:when test="$n = 'ring'"&gt;&amp;#8728;&lt;/xsl:when&gt;&lt;!--∘--&gt;
 &lt;xsl:when test="$n = 'aster'"&gt;&amp;#8727;&lt;/xsl:when&gt;&lt;!--∗--&gt;
 &lt;xsl:when test="$n = 'star'"&gt;&amp;#8902;&lt;/xsl:when&gt;&lt;!--⋆--&gt;
 &lt;xsl:when test="$n = 'not'"&gt;&amp;#172;&lt;/xsl:when&gt;&lt;!--¬--&gt;

 &lt;xsl:when test="$n = 'larr'"&gt;&amp;#8592;&lt;/xsl:when&gt;&lt;!--←--&gt;
 &lt;xsl:when test="$n = 'uarr'"&gt;&amp;#8593;&lt;/xsl:when&gt;&lt;!--↑--&gt;
 &lt;xsl:when test="$n = 'rarr'"&gt;&amp;#8594;&lt;/xsl:when&gt;&lt;!--→--&gt;
 &lt;xsl:when test="$n = 'darr'"&gt;&amp;#8595;&lt;/xsl:when&gt;&lt;!--↓--&gt;
 &lt;xsl:when test="$n = 'harr'"&gt;&amp;#8596;&lt;/xsl:when&gt;&lt;!--↔--&gt;

 &lt;xsl:when test="$n = 'lArr'"&gt;&amp;#8656;&lt;/xsl:when&gt;&lt;!--⇐--&gt;
 &lt;xsl:when test="$n = 'uArr'"&gt;&amp;#8657;&lt;/xsl:when&gt;&lt;!--⇑--&gt;
 &lt;xsl:when test="$n = 'rArr'"&gt;&amp;#8658;&lt;/xsl:when&gt;&lt;!--⇒--&gt;
 &lt;xsl:when test="$n = 'dArr'"&gt;&amp;#8659;&lt;/xsl:when&gt;&lt;!--⇓--&gt;
 &lt;xsl:when test="$n = 'hArr'"&gt;&amp;#8660;&lt;/xsl:when&gt;&lt;!--⇔--&gt;

 &lt;xsl:when test="$n = 'prec'"&gt;&amp;#x227A;&lt;/xsl:when&gt;&lt;!--≺--&gt;
 &lt;xsl:when test="$n = 'succ'"&gt;&amp;#x227B;&lt;/xsl:when&gt;&lt;!--≻--&gt;

 &lt;xsl:when test="$n = 'cr'"&gt;&amp;#8629;&lt;/xsl:when&gt;&lt;!--↵--&gt;

 &lt;xsl:otherwise&gt;
  &lt;xsl:value-of select="text()"/&gt;
 &lt;/xsl:otherwise&gt;
&lt;/xsl:choose&gt;
&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
