---
title: "()括弧"
source_url: "https://ufcpp.net/study/xml/ref/paren/"
content_type: "Article"
published_at: "2015-05-06T14:25:16"
updated_at: "2015-05-06T14:25:16"
tags: []
umbraco_id: 1684
parent_id: 1661
sort_order: 22
aliases:
  - "/ref/paren"
  - "/ref/paren.html"
  - "/study/ref/paren"
  - "/study/ref/paren.html"
  - "/xml/ref/paren/"
---

# ()括弧

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
()の表示
obsolete


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;paren size="括弧の大きさ"&gt;括弧内の式&lt;/paren&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>f&lt;sup&gt;(n)&lt;/sup&gt; = &lt;paren size="2"&gt;&lt;ddt/&gt;&lt;/paren&gt;&lt;sup&gt;n&lt;/sup&gt;f
</pre><div class="math">f<sup>(n)</sup> = <span class="paren" style="font-size:2em;">(</span><table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table><span class="paren" style="font-size:2em;">)</span><sup>n</sup>f
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:paren"&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    (
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    )
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
