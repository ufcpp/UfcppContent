---
title: "積分記号"
source_url: "https://ufcpp.net/study/xml/ref/int/"
content_type: "Article"
published_at: "2015-05-06T14:25:03"
updated_at: "2015-05-06T14:25:03"
tags: []
umbraco_id: 1678
parent_id: 1661
sort_order: 16
aliases:
  - "/ref/int"
  - "/ref/int.html"
  - "/study/ref/int"
  - "/study/ref/int.html"
  - "/xml/ref/int/"
---

# 積分記号

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
積分記号を表示する


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;int&gt;&lt;sub&gt;積分記号の下に来る文字&lt;/sub&gt;&lt;sup&gt;積分記号の上に来る文字&lt;/sup&gt;&lt;/int&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;int&gt;&lt;sub&gt;a&lt;/sub&gt;&lt;sup&gt;b&lt;/sup&gt;&lt;/int&gt; f(x) &lt;d/&gt;x
</pre><div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> b</td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">a</td></tr></table> f(x) <span class="normal">d</span>x
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:int"&gt;
  &lt;span class="integral"&gt;∫&lt;/span&gt;
  &lt;table class="integral" summary="integral"&gt;
    &lt;tr&gt;&lt;td class="intsup"&gt;&amp;#xA0;&lt;xsl:apply-templates select="ufcpp:sup"/&gt;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td style="font-size:30%;"&gt;&amp;#xA0;&lt;/td&gt;&lt;/tr&gt;
    &lt;tr&gt;&lt;td class="intsub"&gt;&lt;xsl:apply-templates select="ufcpp:sub"/&gt;&lt;/td&gt;&lt;/tr&gt;
  &lt;/table&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:oint/ufcpp:sup|ufcpp:int/ufcpp:sup|ufcpp:doubleint/ufcpp:sup|ufcpp:tripleint/ufcpp:sup"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

&lt;xsl:template match="ufcpp:oint/ufcpp:sub|ufcpp:int/ufcpp:sub|ufcpp:doubleint/ufcpp:sub|ufcpp:tripleint/ufcpp:sub"&gt;
  &lt;xsl:apply-templates/&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre>span.integral
{
  font-size:140%;
  font-style:normal;
  vertical-align:middle;
  margin-right:-0.1em;
}
span.ointegral
{
  font-size:140%;
  font-style:normal;
  vertical-align:middle;
  margin-right:-0.4em;
}

table.integral
{
  display:inline;
  vertical-align:middle;
  font-size:80%;
  font-style:italic;
  padding-right:0.3em;
  padding-left:0.1em;
}

td.intsup
{
  text-align:right;
  margin:0;
  padding:0;
}

table.integral td.intsub
{
  text-align:left;
  margin:0;
  padding:0;
}

</pre>
