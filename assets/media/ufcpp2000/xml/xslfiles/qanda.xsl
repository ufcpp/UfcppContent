<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
・ソースファイルの表示。
source   … ソースファイル用の pre。
reserved … 言語の予約語（キーワード）。
string   … 文字列。
comment  … ソース中のコメント。
operator … 演算子。

・コンソール画面っぽいのの表示。
console … コンソール画面っぽい pre。
input   … ユーザからの入力の部分。
comment … (実際にはコンソール上には表示されてない)コメント。
prompt  … プロンプト。
-->

<!-- ********** source ********** -->
<!-- ソースファイル用の pre -->
<xsl:template match="ufcpp:QandAList">
<xsl:variable name="css" select="qanda" />

	<xsl:variable name="qaid">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<!-- 項目一覧 -->
	<ul>
	<xsl:for-each select="ufcpp:item">
	<xsl:variable name="itemid">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:number count="ufcpp:item" format="01" /></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<li>
		<a href="#{$qaid}_{$itemid}"><xsl:value-of select="@name" /></a>
	</li>
	</xsl:for-each>
	</ul>

	<xsl:for-each select="ufcpp:item">
	<xsl:variable name="itemid">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:number count="ufcpp:item" format="01" /></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<div class="qanda">
		<h2 class="q">
		<a id="{$qaid}_{$itemid}"><xsl:value-of select="@name" /></a>
		</h2>
		<div class="a"><xsl:apply-templates select="." /></div>
	</div>
	</xsl:for-each>
</xsl:template>

</xsl:stylesheet>
