<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

  <!-- HTMLタグをそのまま表示できるように -->
  <xsl:template match="*">
    <xsl:element name="{local-name()}">
      <xsl:for-each select="@*">
        <xsl:copy/>
      </xsl:for-each>
      <xsl:apply-templates/>
    </xsl:element>
  </xsl:template>
	
  <xsl:template match="text()"><xsl:copy/></xsl:template>

  <!-- スペース -->
  <xsl:template match="ufcpp:sp">
    <xsl:text> </xsl:text>
  </xsl:template>

  <!-- 色つきスペース -->
  <xsl:template match="ufcpp:csp">
    <span class="color">&#x00A0;</span>
  </xsl:template>

  <!-- ○× -->
  <xsl:template match="ufcpp:pros">
    <span style="color:#0000FF;">○</span>
  </xsl:template>
  <xsl:template match="ufcpp:cons">
    <span style="color:#FF4040">×</span>
  </xsl:template>

</xsl:stylesheet>
