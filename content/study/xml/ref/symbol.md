---
title: "記号"
source_url: "https://ufcpp.net/study/xml/ref/symbol/"
content_type: "Article"
published_at: "2015-05-06T14:25:39"
updated_at: "2015-05-06T14:25:39"
tags: []
umbraco_id: 1694
parent_id: 1661
sort_order: 32
aliases:
  - "/ref/symbol"
  - "/ref/symbol.html"
  - "/study/ref/symbol"
  - "/study/ref/symbol.html"
  - "/xml/ref/symbol/"
---

# 記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

記号類。
（短縮形: sym）
主に、MS IME の変換で出せない文字。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;symbol name="name"&gt;&lt;/symbol&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;symbol n="forall"/&gt;x s.t. &lt;symbol n="vtheta"/&gt;&lt;paren&gt;x&lt;/paren&gt; &lt;op&gt;=&lt;/op&gt; 0

</pre><div class="math"><span class="normal">∀</span>x s.t. <span class="normal">ϑ</span><span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span> <op>=</op> 0

</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:math//ufcpp:symbol|ufcpp:math//ufcpp:sym|ufcpp:Math//ufcpp:symbol|ufcpp:Math//ufcpp:sym"&gt;
&lt;xsl:variable name="n"&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@name != ''"&gt;&lt;xsl:value-of select="@name"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:value-of select="@n"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/xsl:variable&gt;

&lt;span class="normal"&gt;
&lt;xsl:choose&gt;
 &lt;xsl:when test="$n = 'forall'"&gt;&amp;#8704;&lt;/xsl:when&gt;&lt;!--∀--&gt;
 &lt;xsl:when test="$n = 'exist'"&gt;&amp;#8707;&lt;/xsl:when&gt;&lt;!--∃--&gt;
 &lt;xsl:when test="$n = 'partial'"&gt;&amp;#8706;&lt;/xsl:when&gt;&lt;!--∂--&gt;
 &lt;xsl:when test="$n = 'part'"&gt;&amp;#8706;&lt;/xsl:when&gt;&lt;!--∂--&gt;
 &lt;xsl:when test="$n = 'nabla'"&gt;&amp;#8711;&lt;/xsl:when&gt;&lt;!--∇--&gt;
 &lt;xsl:when test="$n = 'infinity'"&gt;&amp;#8734;&lt;/xsl:when&gt;&lt;!--∞--&gt;
 &lt;xsl:when test="$n = 'infty'"&gt;&amp;#8734;&lt;/xsl:when&gt;&lt;!--∞--&gt;
 &lt;xsl:when test="$n = 'infin'"&gt;&amp;#8734;&lt;/xsl:when&gt;&lt;!--∞--&gt;
 &lt;xsl:when test="$n = 'ang'"&gt;&amp;#8736;&lt;/xsl:when&gt;&lt;!--∠--&gt;
 &lt;xsl:when test="$n = 'therefore'"&gt;&amp;#8756;&lt;/xsl:when&gt;&lt;!--∴--&gt;
 &lt;xsl:when test="$n = 'because'"&gt;&amp;#8757;&lt;/xsl:when&gt;&lt;!--∵--&gt;

 &lt;xsl:when test="$n = 'empty'"&gt;&amp;#8709;&lt;/xsl:when&gt;&lt;!--∅--&gt;
 &lt;xsl:when test="$n = 'weierp'"&gt;&amp;#8472;&lt;/xsl:when&gt;&lt;!--℘--&gt;
 &lt;xsl:when test="$n = 'image'"&gt;&amp;#8465;&lt;/xsl:when&gt;&lt;!--ℑ--&gt;
 &lt;xsl:when test="$n = 'real'"&gt;&amp;#8476;&lt;/xsl:when&gt;&lt;!--ℜ--&gt;
 &lt;xsl:when test="$n = 'alef'"&gt;&amp;#8501;&lt;/xsl:when&gt;&lt;!--ℵ--&gt;
 &lt;xsl:when test="$n = 'planck'"&gt;&amp;#x210F;&lt;/xsl:when&gt;&lt;!--ℏ--&gt;

 &lt;!-- ellipsis --&gt;
 &lt;xsl:when test="$n = 'dots'"&gt;&amp;#x22EF;&lt;/xsl:when&gt;&lt;!--⋯--&gt;
 &lt;xsl:when test="$n = 'vdots'"&gt;&amp;#x22EE;&lt;/xsl:when&gt;&lt;!--⋮--&gt;
 &lt;xsl:when test="$n = 'updots'"&gt;&amp;#x22F0;&lt;/xsl:when&gt;&lt;!--⋰--&gt;
 &lt;xsl:when test="$n = 'downdots'"&gt;&amp;#x22F1;&lt;/xsl:when&gt;&lt;!--⋱--&gt;

 &lt;!--ギリシャ文字異字体--&gt;
 &lt;xsl:when test="$n = 'vbeta'"&gt;&amp;#976;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vepsilon'"&gt;&amp;#8714;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vtheta'"&gt;&amp;#977;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vkappa'"&gt;&amp;#1008;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vpi'"&gt;&amp;#982;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vrho'"&gt;&amp;#1009;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vsigma'"&gt;&amp;#962;&lt;/xsl:when&gt;
 &lt;xsl:when test="$n = 'vphi'"&gt;&amp;#981;&lt;/xsl:when&gt;

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
