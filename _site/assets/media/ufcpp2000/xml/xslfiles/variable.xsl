<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
変数参照系の xsl:template の定義

var-group … 変数一覧の表示。
variable  … 変数定義。
use       … 変数の参照。

-->

<!-- ********** var-group ********** -->
<!-- 変数グループ定義 -->
<xsl:template match="ufcpp:var-group">
<!--
<xsl:variable name="css" select="figure" />
↑figure.xsl を使わなくて、この variable.xsl だけ場合、
この行をコメントから出して。
-->
<p>
<table class="border">
<tr class="border">
<th class="border">変数名</th>
<th class="border" colspan="2">意味</th>
<th class="border">単位</th>
<th class="border">概要</th>
</tr>

<xsl:apply-templates select="ufcpp:variable"/>

</table>
</p>
</xsl:template>

<!-- ********** variable ********** -->
<!-- 変数定義 -->
<xsl:template match="ufcpp:variable">
<tr class="border">
<td class="border">
	<a>
	<xsl:attribute name="id"><xsl:value-of select="@id" /></xsl:attribute>
	<span class="math"><nobr><xsl:apply-templates select="ufcpp:letter" /></nobr></span>
	</a>
</td>
<td class="border"><xsl:value-of select="ufcpp:name" /></td>
<td class="border"><xsl:value-of select="ufcpp:ename" /></td>
<td class="border"><xsl:apply-templates select="ufcpp:unit" /></td>
<td class="border"><xsl:apply-templates select="ufcpp:summary" /></td>
</tr>
</xsl:template>

<xsl:template match="ufcpp:variable//ufcpp:letter">
<xsl:apply-templates select="*|text()" />
</xsl:template>

<!-- ********** use ********** -->
<!-- 変数の参照 -->
<xsl:template match="*/ufcpp:use">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="doc"><xsl:choose><xsl:when test="@doc!=''"><xsl:value-of select="@doc" /></xsl:when><xsl:otherwise>variable</xsl:otherwise></xsl:choose></xsl:variable>

	<xsl:variable name="filename" select="concat($area,$doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,$doc,'.html')" />
	<xsl:variable name="id" select="@id" />

	<xsl:variable name="var" select="document($filename)//ufcpp:variable[@id=$id]" />

	<a>
		<xsl:attribute name="href"><xsl:value-of select="$htmlname" />#<xsl:value-of select="$id" /></xsl:attribute>
		<xsl:attribute name="title"><xsl:value-of select="$var/ufcpp:name/text()" /></xsl:attribute>
		<xsl:apply-templates select="$var/ufcpp:letter" />
	</a>
</xsl:template>

</xsl:stylesheet>
