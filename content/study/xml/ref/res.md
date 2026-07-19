---
title: "留数"
source_url: "https://ufcpp.net/study/xml/ref/res/"
content_type: "Article"
published_at: "2015-05-06T14:25:30"
updated_at: "2015-05-06T14:25:30"
tags: []
umbraco_id: 1689
parent_id: 1661
sort_order: 27
aliases:
  - "/ref/Res"
  - "/ref/Res.html"
  - "/study/ref/Res"
  - "/study/ref/Res.html"
  - "/xml/ref/res/"
---

# 留数

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
留数を表す記号Resを表示


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;Res&gt;留数を求めたい関数,極&lt;/Res&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;int&gt;&lt;sub&gt;C&lt;/sub&gt;&lt;/int&gt;f(z)&lt;d/&gt;z = 2πi&lt;Sigma&gt;&lt;sub&gt;i&lt;/sub&gt;&lt;/Sigma&gt;&lt;Res&gt;f,a&lt;sub&gt;i&lt;/sub&gt;&lt;/Res&gt;
</pre><div class="math"><span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">C</td></tr></table>f(z)<span class="normal">d</span>z = 2πi<table class="sigma" summary="sum"><tr><td class="sigmasub"></td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table><span class="normal">Res</span><span class="paren" style="font-size:em;">[</span>f,a<sub>i</sub><span class="paren" style="font-size:em;">]</span>
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:Res"&gt;
  &lt;span class="normal"&gt;Res&lt;/span&gt;
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

span.normal
{
  font-weight:normal;
  font-style:normal;
}

</pre>
