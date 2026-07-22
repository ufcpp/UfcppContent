<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
章立て、章参照関係の xsl:template の定義

section … セクション、サブセクション、サブサブセクション。
ref     … 章の参照。
link    … ドキュメントの参照。

-->

<!-- セクション～サブサブセクション作成用の共通テンプレート -->
<xsl:template name="MakeSection">
 <xsl:param name="tag">h2</xsl:param>

	<xsl:variable name="id">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

 <xsl:element name="{$tag}"><a id="{$id}"><xsl:value-of select="@title" /></a></xsl:element>

 <xsl:apply-templates select="*"/>
</xsl:template>

<!-- ********** section ********** -->
<!-- セクション -->
<xsl:template match="ufcpp:document//ufcpp:section">
 <xsl:call-template name="MakeSection"/>
</xsl:template>

<!-- ********** section/section ********** -->
<!-- サブセクション -->
<xsl:template match="ufcpp:document//ufcpp:section/ufcpp:section">
 <xsl:call-template name="MakeSection">
  <xsl:with-param name="tag">h3</xsl:with-param>
 </xsl:call-template>
</xsl:template>

<!-- ********** section/section/section ********** -->
<!-- サブサブセクション -->
<xsl:template match="ufcpp:document//ufcpp:section/ufcpp:section/ufcpp:section">
 <xsl:call-template name="MakeSection">
  <xsl:with-param name="tag">h4</xsl:with-param>
 </xsl:call-template>
</xsl:template>

<!-- ********** ref ********** -->
<!-- 章の参照 -->
<xsl:template match="*/ufcpp:ref">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="filename" select="concat($area,@doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,@doc,'.html')" />
	<xsl:variable name="id" select="@id" />
	<xsl:variable name="section" select="document($filename)//ufcpp:section[@id=$id]" />

	「<a>
		<xsl:attribute name="href"><xsl:value-of select="$htmlname" />#<xsl:value-of select="$id" /></xsl:attribute>
		<xsl:value-of select="$section/@title" />
	</a>」
</xsl:template>

<!-- ********** link ********** -->
<!-- ドキュメントの参照 -->
<xsl:template match="*/ufcpp:link">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="filename" select="concat($area,@doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,@doc,'.html')" />

	<xsl:variable name="doctitle" select="document($filename)//ufcpp:document/@title" />
	<xsl:variable name="indextitle" select="document($filename)//ufcpp:index/@title" />
	<xsl:variable name="title"><xsl:choose><xsl:when test="$doctitle!=''"><xsl:value-of select="$doctitle" /></xsl:when><xsl:otherwise><xsl:value-of select="$indextitle" /></xsl:otherwise></xsl:choose></xsl:variable>
	「<a>
		<xsl:attribute name="href"><xsl:value-of select="$htmlname" /></xsl:attribute>
		<xsl:value-of select="$title" />
	</a>」
</xsl:template>

<!-- ********** head ********** -->
<!-- 小見出し -->
<xsl:template match="ufcpp:section//ufcpp:head">
<h5><xsl:value-of select="text()" /></h5>
</xsl:template>

<!-- ********** sample ********** -->
<!-- サンプル -->
<xsl:template match="ufcpp:sample">
  <h4 class="sample">サンプル</h4>
</xsl:template>

</xsl:stylesheet>
