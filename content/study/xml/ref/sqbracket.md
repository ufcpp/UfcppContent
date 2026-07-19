---
title: "[]括弧"
source_url: "https://ufcpp.net/study/xml/ref/sqbracket/"
content_type: "Article"
published_at: "2015-05-06T14:25:35"
updated_at: "2015-05-06T14:25:35"
tags: []
umbraco_id: 1692
parent_id: 1661
sort_order: 30
aliases:
  - "/ref/sqbracket"
  - "/ref/sqbracket.html"
  - "/study/ref/sqbracket"
  - "/study/ref/sqbracket.html"
  - "/xml/ref/sqbracket/"
---

# \[\]括弧

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
[]の表示
（obsolete。bracket に移行。）


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;sqbracket size="括弧の大きさ"&gt;括弧内の式&lt;/sqbracket&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;sqbracket&gt;H,G&lt;/sqbracket&gt; = HG &amp;#x2212; GH
</pre><div class="math"><span class="paren" style="font-size:em;">[</span>H,G<span class="paren" style="font-size:em;">]</span> = HG − GH
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:sqbracket"&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    [
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    ]
  &lt;/span&gt;
&lt;/xsl:template&gt;

</pre>

##<a id="sec-generated-title-5"></a> <a id="css"></a>style sheet
<pre>span.paren
{
  font-style:normal;
  vertical-align:middle;
}

</pre>
