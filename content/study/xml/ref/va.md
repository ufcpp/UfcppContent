---
title: "ベクトル解析用記号"
source_url: "https://ufcpp.net/study/xml/ref/va/"
content_type: "Article"
published_at: "2015-05-06T14:25:43"
updated_at: "2015-05-06T14:25:43"
tags: []
umbraco_id: 1696
parent_id: 1661
sort_order: 34
aliases:
  - "/ref/va"
  - "/ref/va.html"
  - "/study/ref/va"
  - "/study/ref/va.html"
  - "/xml/ref/va/"
---

# ベクトル解析用記号

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

線素、面素、体積素。
勾配、発散、回転。
ナブラ記号。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;dV/&gt;
&lt;dS/&gt;
&lt;dl/&gt;
&lt;gradient/&gt;
&lt;divergence/&gt;
&lt;rotation/&gt;
&lt;textgrad/&gt;
&lt;textdiv/&gt;
&lt;textrot/&gt;
&lt;nabra/&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>&lt;int&gt;&lt;sub&gt;V&lt;/sub&gt;&lt;/int&gt; &lt;divergence/&gt;&lt;vec&gt;f&lt;/vec&gt; &lt;dV/&gt;
＝
&lt;oint&gt;&lt;sub&gt;∂V&lt;/sub&gt;&lt;/oint&gt; &lt;vec&gt;f&lt;/vec&gt;・&lt;dS/&gt;
,
&lt;int&gt;&lt;sub&gt;S&lt;/sub&gt;&lt;/int&gt; &lt;rotation/&gt;&lt;vec&gt;f&lt;/vec&gt;・&lt;dS/&gt;
＝
&lt;oint&gt;&lt;sub&gt;∂S&lt;/sub&gt;&lt;/oint&gt; &lt;vec&gt;f&lt;/vec&gt;・&lt;dl/&gt;
,
&lt;d/&gt;f
＝
&lt;gradient/&gt;f
・&lt;dl/&gt;,
&lt;gradient/&gt; ＝ &lt;textgrad/&gt;, 
&lt;divergence/&gt; ＝ &lt;textdiv/&gt;, 
&lt;rotation/&gt; ＝ &lt;textrot/&gt;, 
&lt;nabra/&gt;
</pre><div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table> <span class="vector">∇</span>・<span class="vector">f</span> <span class="normal">d</span>V
＝
<span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂V</td></tr></table> <span class="vector">f</span>・<span class="normal">d</span><span class="vector">S</span>
,
<span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">S</td></tr></table> <span class="vector">∇</span>×<span class="vector">f</span>・<span class="normal">d</span><span class="vector">S</span>
＝
<span class="ointegral">∮</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">∂S</td></tr></table> <span class="vector">f</span>・<span class="normal">d</span><span class="vector">l</span>
,
<span class="normal">d</span>f
＝
<span class="vector">∇</span>f
・<span class="normal">d</span><span class="vector">l</span>,
<span class="vector">∇</span> ＝ <span class="normal">grad</span>, 
<span class="vector">∇</span>・ ＝ <span class="normal">div</span>, 
<span class="vector">∇</span>× ＝ <span class="normal">rot</span>, 
<span class="vector">∇</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>&lt;xsl:template match="ufcpp:dV"&gt;
  &lt;span class="normal"&gt;d&lt;/span&gt;V
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:dS"&gt;
  &lt;span class="normal"&gt;d&lt;/span&gt;&lt;span class="vector"&gt;S&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:dl"&gt;
  &lt;span class="normal"&gt;d&lt;/span&gt;&lt;span class="vector"&gt;l&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:gradient"&gt;
  &lt;span class="vector"&gt;∇&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:divergence"&gt;
  &lt;span class="vector"&gt;∇&lt;/span&gt;・
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:rotation"&gt;
  &lt;span class="vector"&gt;∇&lt;/span&gt;×
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:textgrad"&gt;
  &lt;span class="normal"&gt;grad&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:textdiv"&gt;
  &lt;span class="normal"&gt;div&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:textrot"&gt;
  &lt;span class="normal"&gt;rot&lt;/span&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:nabra"&gt;
  &lt;span class="vector"&gt;∇&lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
