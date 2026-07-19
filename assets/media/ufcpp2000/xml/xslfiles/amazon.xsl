<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

<!--
Amazon アフィリエイト 個別商品リンク作成。

amazon
	@asin … ASIN 番号
-->

<!-- Amazon アフィリエイト 個別商品リンク作成 -->
<xsl:template match="ufcpp:amazon">
<iframe style="width:120px;height:240px;" scrolling="no" marginwidth="0" marginheight="0" frameborder="0"><xsl:attribute name="src"><xsl:text>http://rcm-jp.amazon.co.jp/e/cm?t=cunflc-22&amp;o=9&amp;p=8&amp;l=as1&amp;asins=</xsl:text><xsl:value-of select="@asin" /><xsl:text>&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;lc1=0000ff&amp;bc1=000000&amp;bg1=ffffff&amp;f=ifr</xsl:text></xsl:attribute>
asin 番号: <xsl:value-of select="@asin" />
</iframe>
</xsl:template>

<xsl:template match="ufcpp:amazontext">
<a name="amazletlink" target="_blank"><xsl:attribute name="href"><xsl:text>http://www.amazon.co.jp/exec/obidos/ASIN/</xsl:text><xsl:value-of select="@asin" /><xsl:text>/cunflc-22/ref=nosim/</xsl:text></xsl:attribute><xsl:value-of select="text()" /></a>
</xsl:template>

</xsl:stylesheet>
