<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
図表環境の xsl:template の定義

tabular … 表。キャプションとかの機能付き。
figure  … 図。キャプションとかの機能付き。

-->

<!-- ********** figure ********** -->
<!-- 図 -->
<xsl:template match="ufcpp:figure">
<xsl:variable name="css" select="figure" />

<div class="fig">
  <xsl:apply-templates />
</div>
</xsl:template>

  <xsl:template match="ufcpp:image">
    <p class="fig">
      <a>
        <xsl:attribute name="href">
          <xsl:value-of select="@src"/>
        </xsl:attribute>
        <img>
          <xsl:attribute name="src">
            <xsl:value-of select="@src"/>
          </xsl:attribute>
          <xsl:attribute name="alt">
            <xsl:value-of select="text()"/>
          </xsl:attribute>
          <xsl:attribute name="width">
            <xsl:value-of select="@width" />
          </xsl:attribute>
          <xsl:attribute name="height">
            <xsl:value-of select="@height"/>
          </xsl:attribute>
        </img>
      </a>
    </p>
  </xsl:template>

  <xsl:template match="ufcpp:legend">
    <p class="caption">
      図<xsl:number level="any" count="ufcpp:figure//ufcpp:legend" format="1"/>: <xsl:apply-templates />
    </p>
  </xsl:template>

<!-- ********** table ********** -->
<!-- 表 -->
<xsl:template match="ufcpp:table">
<div class="fig">
  <xsl:apply-templates select="ufcpp:caption"/>

  <table class="border" cellspacing="0" cellpadding="0">
    <xsl:attribute name="summary">
      <xsl:choose>
        <xsl:when test="ufcpp:caption/text() != ''">
          <xsl:value-of select="ufcpp:caption/text()"/>
        </xsl:when>
        <xsl:when test="@caption != ''">
          <xsl:value-of select="@caption"/>
        </xsl:when>
      </xsl:choose>
    </xsl:attribute>
    <xsl:apply-templates select="ufcpp:tr|ufcpp:thead|ufcpp:tbody|ufcpp:tfoot"/>
  </table>
</div>
</xsl:template>

  <xsl:template match="ufcpp:caption">
    <p class="caption">
      表<xsl:number level="any" count="ufcpp:table//ufcpp:caption" format="1"/>: <xsl:apply-templates />
    </p>
  </xsl:template>

<xsl:template match="ufcpp:table//ufcpp:tr">
<tr class="border">
  <xsl:apply-templates select="ufcpp:td|ufcpp:th"/>
</tr>
</xsl:template>

<xsl:template match="ufcpp:table//ufcpp:td">
<td class="border">
<xsl:for-each select="@*">
<xsl:copy/>
</xsl:for-each>
  <xsl:apply-templates select="*|text()"/>
</td>
</xsl:template>

<xsl:template match="ufcpp:table//ufcpp:th">
<th class="border">
<xsl:for-each select="@*">
<xsl:copy/>
</xsl:for-each>
  <xsl:apply-templates select="*|text()"/>
</th>
</xsl:template>

  <!-- ********** layouttable ********** -->
  <!-- （あんまり推奨されることではないけど）レイアウト用のテーブル -->
  <xsl:template match="ufcpp:layouttable">
    <table class="layout" summary="レイアウト用テーブル" cellspacing="0" cellpadding="0">
      <xsl:choose>
        <xsl:when test="@class!=''">
          <xsl:attribute name ="class">
            <xsl:value-of select="@class"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name ="class">
            <xsl:text>layout</xsl:text>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>

      <xsl:apply-templates select="*"/>
    </table>
  </xsl:template>

</xsl:stylesheet>
