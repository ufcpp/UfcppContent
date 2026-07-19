---
title: "括弧"
source_url: "https://ufcpp.net/study/xml/ref/bracket/"
content_type: "Article"
published_at: "2015-05-06T14:24:41"
updated_at: "2015-05-06T14:24:41"
tags: []
umbraco_id: 1667
parent_id: 1661
sort_order: 5
aliases:
  - "/ref/bracket"
  - "/ref/bracket.html"
  - "/study/ref/bracket"
  - "/study/ref/bracket.html"
  - "/xml/ref/bracket/"
---

# 括弧

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
括弧の表示。
(a), {a}, &lt;a&gt;, [a], |a|, ||a|| など、全て bracket にまとめました。
（短縮形 bra）
（type: paren (round), brace (curl), angle, square(sq), abs, norm, ceil, floor）


##<a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法
<pre>&lt;bracket size="括弧の大きさ" type="type"&gt;括弧内の式&lt;/bracket&gt;
</pre>

##<a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル
<pre>&lt;bracket&gt;x&lt;/bracket&gt; = &lt;inv&gt;N&lt;/inv&gt;&lt;Sigma&gt;&lt;sub&gt;i&lt;/sub&gt;&lt;sup&gt;N&lt;/sup&gt;&lt;/Sigma&gt;x&lt;sub&gt;i&lt;/sub&gt;
</pre><div class="math"><span class="paren" style="font-size:em;">〈</span>x<span class="paren" style="font-size:em;">〉</span> = <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>N</td></tr></table><table class="sigma" summary="sum"><tr><td class="sigmasub">N</td></tr><tr><td class="sigma">∑</td></tr><tr><td class="sigmasub">i</td></tr></table>x<sub>i</sub>
</div>

##<a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template
<pre>&lt;xsl:template match="ufcpp:math//ufcpp:bracket|ufcpp:math//ufcpp:bra|ufcpp:Math//ufcpp:bracket|ufcpp:Math//ufcpp:bra"&gt;
&lt;xsl:variable name="t"&gt;&lt;xsl:choose&gt;&lt;xsl:when test="@type != ''"&gt;&lt;xsl:value-of select="@type"/&gt;&lt;/xsl:when&gt;&lt;xsl:otherwise&gt;&lt;xsl:value-of select="@t"/&gt;&lt;/xsl:otherwise&gt;&lt;/xsl:choose&gt;&lt;/xsl:variable&gt;

&lt;xsl:variable name="l"&gt;
 &lt;xsl:choose&gt;
  &lt;xsl:when test="$t = 'p'"&gt;(&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'paren'"&gt;(&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'r'"&gt;(&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'round'"&gt;(&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'c'"&gt;{&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'curl'"&gt;{&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'b'"&gt;{&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'brace'"&gt;{&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'angle'"&gt;&amp;#9001;&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'square'"&gt;[&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'sq'"&gt;[&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'abs'"&gt;|&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'norm'"&gt;||&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'ceil'"&gt;&amp;#8968;&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'floor'"&gt;&amp;#8970;&lt;/xsl:when&gt;
  &lt;xsl:otherwise&gt;&amp;#9001;&lt;/xsl:otherwise&gt;
 &lt;/xsl:choose&gt;
&lt;/xsl:variable&gt;

&lt;xsl:variable name="r"&gt;
 &lt;xsl:choose&gt;
  &lt;xsl:when test="$t = 'p'"&gt;)&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'paren'"&gt;)&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'r'"&gt;)&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'round'"&gt;)&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'c'"&gt;}&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'curl'"&gt;}&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'b'"&gt;}&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'brace'"&gt;}&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'angle'"&gt;&amp;#9002;&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'square'"&gt;]&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'sq'"&gt;]&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'abs'"&gt;|&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'norm'"&gt;||&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'ceil'"&gt;&amp;#8969;&lt;/xsl:when&gt;
  &lt;xsl:when test="$t = 'floor'"&gt;&amp;#8971;&lt;/xsl:when&gt;
  &lt;xsl:otherwise&gt;&amp;#9002;&lt;/xsl:otherwise&gt;
 &lt;/xsl:choose&gt;
&lt;/xsl:variable&gt;

  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    &lt;xsl:value-of select="$l"/&gt;
  &lt;/span&gt;
  &lt;xsl:apply-templates/&gt;
  &lt;span class="paren"&gt;
    &lt;xsl:attribute name="style"&gt;font-size:&lt;xsl:value-of select="@size"/&gt;em;&lt;/xsl:attribute&gt;
    &lt;xsl:value-of select="$r"/&gt;
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
