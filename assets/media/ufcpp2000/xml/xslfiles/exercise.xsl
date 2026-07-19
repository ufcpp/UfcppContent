<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
演習問題の xsl:template の定義

exercise … 演習問題
	question … 問題
	answer   … 解答例

refex    … 演習問題の参照

exercise-list … 問題の一覧表示。
-->

<xsl:variable name="index" select="document(concat($dir,'/index.xml'))/index"/>

<!-- ********** exercise ********** -->
<!-- 演習問題 -->
<xsl:template match="ufcpp:exercise">

<h3><a id="ex_{@id}">問題 <xsl:number value="position()" format="1"/></a></h3>
<xsl:apply-templates select="ufcpp:question"/>
<p>
<a href="exercise.html#ex_{@id}">解答</a>
</p>

</xsl:template>

<!-- ********** refex ********** -->
<!-- 演習問題の参照 -->
<xsl:template match="ufcpp:refex">
	<xsl:variable name="area"><xsl:choose><xsl:when test="@area!=''"><xsl:value-of select="@area" /></xsl:when><xsl:otherwise><xsl:value-of select="$dir" /></xsl:otherwise></xsl:choose>/</xsl:variable>

	<xsl:variable name="filename" select="concat($area,@doc,'.xml')" />
	<xsl:variable name="htmlname" select="concat('../',$area,@doc,'.html')" />
	<xsl:variable name="pos"><xsl:number value="@pos" format="1"/></xsl:variable>
	<xsl:variable name="title" select="document($filename)/ufcpp:document/@title" />
	<xsl:variable name="exercise" select="document($filename)//ufcpp:exercise[position()=$pos]" />
	<xsl:variable name="id" select="$exercise/@id" />

<a href="{$htmlname}"><xsl:value-of select="$title" /></a>
<xsl:text>の</xsl:text>
<a href="{$htmlname}#ex_{$id}">問題 <xsl:value-of select="$pos" /></a>

</xsl:template>

<!-- ********** exercise-list ********** -->
<!-- 分野中の演習問題の一覧表示(解答込み) -->
<xsl:template match="ufcpp:exercise-list">

<xsl:for-each select="$index//ufcpp:document">
 <xsl:variable name="xmlname" select="concat($dir,'/',@href,'.xml')" />
 <xsl:variable name="document" select="document($xmlname)/ufcpp:document"/>

 <xsl:if test="$document//ufcpp:exercise!=''">
 <h2><xsl:value-of select="$document/@title"/></h2>
 </xsl:if>

 <xsl:for-each select="$document//ufcpp:exercise">
  <h3><a id="ex_{@id}">問題</a></h3>
  <xsl:apply-templates select="ufcpp:question"/>

   <xsl:for-each select="ufcpp:answer">
    <h4>解答例 <xsl:number value="position()" format="1"/></h4>
    <xsl:apply-templates select="*"/>
   </xsl:for-each>
 </xsl:for-each>

</xsl:for-each>

</xsl:template>

</xsl:stylesheet>
