<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
キーワード関係系の xsl:template の定義

keyword … キーワードの定義。
refkey  … キーワードの参照。

-->

<!-- ********** keyword ********** -->
<!-- キーワード -->
<xsl:template match="ufcpp:keyword">
	<xsl:variable name="id">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<a id="{$id}"><em><xsl:value-of select="./text()" /></em></a>
</xsl:template>

<!-- ********** refkey ********** -->
<!-- キーワードの参照 -->
<xsl:template match="*/ufcpp:refkey">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="doc"><xsl:choose><xsl:when test="@doc!=''"><xsl:value-of select="@doc" /></xsl:when><xsl:otherwise>variable</xsl:otherwise></xsl:choose></xsl:variable>

	<xsl:variable name="filename" select="concat($area,$doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,$doc,'.html')" />
	<xsl:variable name="id" select="@id" />
	<xsl:variable name="keyword" select="document($filename)//ufcpp:keyword[@id=$id]" />

	<a>
		<xsl:attribute name="href"><xsl:value-of select="$htmlname" />#<xsl:value-of select="$id" /></xsl:attribute>
		<xsl:value-of select="$keyword/text()" />
	</a>
</xsl:template>

<!-- ********** keyword-list ********** -->
<!-- 分野中のキーワードの一覧表示 -->
<xsl:template match="ufcpp:keyword-list">

<xsl:for-each select="$index//ufcpp:document">
 <xsl:variable name="xmlname" select="concat($dir,'/',@href,'.xml')" />
 <xsl:variable name="htmlname" select="concat(@href,'.html')" />
 <xsl:variable name="document" select="document($xmlname)/ufcpp:document"/>

 <xsl:if test="$document//ufcpp:keyword!=''">
 <h2><xsl:value-of select="$document/@title"/></h2>
 </xsl:if>

<ul>
<xsl:text> </xsl:text>
 <xsl:for-each select="$document//ufcpp:keyword">
  <li><a>
   <xsl:attribute name="href"><xsl:value-of select="$htmlname" /><xsl:if test="@id!=''">#<xsl:value-of select="@id" /></xsl:if></xsl:attribute>
   <xsl:value-of select="text()"/>
  </a></li>
 </xsl:for-each>
</ul>
</xsl:for-each>

</xsl:template>

</xsl:stylesheet>
