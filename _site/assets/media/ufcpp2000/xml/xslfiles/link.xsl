<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
リンク集。

links
	site … 各ページ
		@url  … サイトの URL
		@name … サイトの名前
		.     … サイトの説明
-->

<!-- ********** links ********** -->
<!-- リンク集 -->
<xsl:template match="ufcpp:links">
<xsl:variable name="css" select="link" />

<xsl:call-template name="MakeLinks"/>
</xsl:template>

<!-- ********** reflinks ********** -->
<!-- 他のファイル中のリンク集をまんま取り込み -->
<xsl:template match="ufcpp:reflinks">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="filename" select="concat($area,@doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,@doc,'.html')" />
	<xsl:variable name="id" select="@id" />
	<xsl:variable name="link" select="document($filename)//ufcpp:links[@id=$id]" />

 <xsl:call-template name="MakeLinks">
  <xsl:with-param name="tag" select="$link"/>
 </xsl:call-template>

</xsl:template>

<!-- links, reflinks 両用のテンプレート -->
<xsl:template name="MakeLinks">
<xsl:param name="tag" select="."/>

<dl class="link">

<xsl:for-each select="$tag/ufcpp:site">
	<dt><a href="{@url}"><xsl:value-of select="@name"/></a></dt>
	<dd><xsl:apply-templates select="*|text()"/></dd>
</xsl:for-each>

</dl>
</xsl:template>


</xsl:stylesheet>
