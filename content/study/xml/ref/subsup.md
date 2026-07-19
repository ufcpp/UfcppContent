---
title: "上付き・下付き文字"
source_url: "https://ufcpp.net/study/xml/ref/subsup/"
content_type: "Article"
published_at: "2015-05-06T14:25:37"
updated_at: "2015-05-06T14:25:37"
tags: []
umbraco_id: 1693
parent_id: 1661
sort_order: 31
aliases:
  - "/ref/subsup"
  - "/ref/subsup.html"
  - "/study/ref/subsup"
  - "/study/ref/subsup.html"
  - "/xml/ref/subsup/"
---

# 上付き・下付き文字

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

上付き・下付き文字を表示する。


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

<pre>&lt;sup&gt;上付き&lt;/sup&gt;
&lt;sub&gt;下付き&lt;/sub&gt;
&lt;subsup&gt;&lt;sub&gt;下&lt;/sub&gt;&lt;sup&gt;上&lt;/sup&gt;&lt;/subsup&gt;
</pre>

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

<pre>x&lt;sup&gt;2&lt;/sup&gt;, 
a&lt;sub&gt;0&lt;/sub&gt; , 
p&lt;subsup&gt;&lt;sub&gt;1&lt;/sub&gt;&lt;sup&gt;2&lt;/sup&gt;&lt;/subsup&gt;
</pre><div class="math">x<sup>2</sup>, 
a<sub>0</sub> , 
p<table class="subsup" summary="sub / sup"><tr><td>2</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td>1</td></tr></table>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

<pre>
&lt;xsl:template match="ufcpp:sup0"&gt;&lt;sup&gt;&lt;span class="normal"&gt;0&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup1"&gt;&lt;sup&gt;&lt;span class="normal"&gt;1&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup2"&gt;&lt;sup&gt;&lt;span class="normal"&gt;2&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup2"&gt;&lt;sup&gt;&lt;span class="normal"&gt;2&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup3"&gt;&lt;sup&gt;&lt;span class="normal"&gt;3&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup4"&gt;&lt;sup&gt;&lt;span class="normal"&gt;4&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup5"&gt;&lt;sup&gt;&lt;span class="normal"&gt;5&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup6"&gt;&lt;sup&gt;&lt;span class="normal"&gt;6&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup7"&gt;&lt;sup&gt;&lt;span class="normal"&gt;7&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup8"&gt;&lt;sup&gt;&lt;span class="normal"&gt;8&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup9"&gt;&lt;sup&gt;&lt;span class="normal"&gt;9&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sup10"&gt;&lt;sup&gt;&lt;span class="normal"&gt;10&lt;/span&gt;&lt;/sup&gt;&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:sub0"&gt;&lt;sub&gt;&lt;span class="normal"&gt;0&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub1"&gt;&lt;sub&gt;&lt;span class="normal"&gt;1&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub2"&gt;&lt;sub&gt;&lt;span class="normal"&gt;2&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub2"&gt;&lt;sub&gt;&lt;span class="normal"&gt;2&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub3"&gt;&lt;sub&gt;&lt;span class="normal"&gt;3&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub4"&gt;&lt;sub&gt;&lt;span class="normal"&gt;4&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub5"&gt;&lt;sub&gt;&lt;span class="normal"&gt;5&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub6"&gt;&lt;sub&gt;&lt;span class="normal"&gt;6&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub7"&gt;&lt;sub&gt;&lt;span class="normal"&gt;7&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub8"&gt;&lt;sub&gt;&lt;span class="normal"&gt;8&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub9"&gt;&lt;sub&gt;&lt;span class="normal"&gt;9&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;
&lt;xsl:template match="ufcpp:sub10"&gt;&lt;sub&gt;&lt;span class="normal"&gt;10&lt;/span&gt;&lt;/sub&gt;&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:supinv"&gt;
  &lt;sup&gt;&lt;span class="normal"&gt;&amp;#8722;1&lt;/span&gt;&lt;/sup&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:subsup"&gt;
  &lt;table class="subsup" summary="sub / sup"&gt;
    &lt;tr&gt;&lt;td&gt;&lt;xsl:apply-templates select="ufcpp:sup"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td style="font-size:30%;"&gt;&amp;#xA0;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td&gt;&lt;xsl:apply-templates select="ufcpp:sub"/&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:subsup/ufcpp:sub|ufcpp:subsup/ufcpp:sup"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

</pre>

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

<pre>table.subsup
{
  display:inline;
  vertical-align:middle;
  font-size:80%;
  font-style:italic;
  padding-left:1em;
}


</pre>
