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

<!-- ********** sources ********** -->
<!-- jQuery UI Tabs で多言語対応 source -->
<xsl:template match="ufcpp:sources">
<xsl:variable name="css" select="source" />
  <xsl:variable name="id">
    <xsl:choose>
      <xsl:when test="@id!=''">
        <xsl:value-of select="@id"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="generate-id()"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <script type="text/javascript">
  <xsl:text><![CDATA[google.setOnLoadCallback(function(){ $("#]]></xsl:text><xsl:value-of select="$id"/><xsl:text><![CDATA[").tabs() });]]></xsl:text>
  </script>

  <div id="{$id}">
    <ul>
      <xsl:for-each select="*">
        <xsl:variable name="num"><xsl:value-of select="$id"/>_<xsl:number level="single" format="1"/></xsl:variable>
        <li>
          <a href="#{$num}"><xsl:value-of select="@header"/></a>
        </li>
      </xsl:for-each>
    </ul>
    <xsl:for-each select="*">
      <xsl:variable name="num">
        <xsl:value-of select="$id"/>_<xsl:number level="single" format="1"/>
      </xsl:variable>
      <div id="{$num}">
        <xsl:apply-templates select="." />
      </div>
    </xsl:for-each>
    </div>
</xsl:template>

    <!-- ********** source ********** -->
<!-- ソースファイル用の pre -->
<xsl:template match="ufcpp:source">
<xsl:variable name="css" select="source" />

<pre class="source"><xsl:for-each select="@*"><xsl:copy/></xsl:for-each><xsl:apply-templates select="*|text()"/></pre>
</xsl:template>

<xsl:template match="ufcpp:source//ufcpp:reserved"><span class="reserved"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:comment"><span class="comment"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:string"><span class="literal"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:literal"><span class="literal"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:type"><span class="type"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:inactive"><span class="inactive"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:operator"><span class="operator"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:source//ufcpp:input"><span class="input"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:code">
<xsl:variable name="css" select="source" />

<code><xsl:for-each select="@*"><xsl:copy/></xsl:for-each><xsl:apply-templates select="*|text()"/></code>
</xsl:template>
  
<xsl:template match="ufcpp:code//ufcpp:reserved"><span class="reserved"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:code//ufcpp:comment"><span class="comment"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:code//ufcpp:string"><span class="string"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:code//ufcpp:operator"><span class="operator"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:code//ufcpp:input"><span class="input"><xsl:apply-templates select="*|text()"/></span></xsl:template>

  <!-- ********** xsource ********** -->
<!-- XML ソースファイル用の pre -->
<xsl:template match="ufcpp:xsource">

<pre class="xsource">
<xsl:for-each select="@*">
<xsl:copy/>
</xsl:for-each>
<xsl:apply-templates select="*|text()"/>
</pre>
</xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:lt"><span class="bracket">&lt;</span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:gt"><span class="bracket">&gt;</span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:symbol"><span class="bracket"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:element"><span class="element"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:comment"><span class="comment"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:attribute"><span class="attribute"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:attvalue"><span class="attvalue"><xsl:apply-templates select="*|text()"/></span></xsl:template>

<xsl:template match="ufcpp:xsource//ufcpp:input"><span class="input"><xsl:apply-templates select="*|text()"/></span></xsl:template>
<xsl:template match="ufcpp:xsource//ufcpp:inactive"><span class="inactive"><xsl:apply-templates select="*|text()"/></span></xsl:template>


<!-- ********** console ********** -->
<!-- コンソール画面っぽい pre -->
<xsl:template match="ufcpp:console">

<pre class="console">
<xsl:for-each select="@*">
<xsl:copy/>
</xsl:for-each>
<xsl:apply-templates select="*|text()"/>
</pre>
</xsl:template>

<xsl:template match="ufcpp:console//ufcpp:input"><span class="input"><xsl:value-of select="text()"/></span></xsl:template>

<xsl:template match="ufcpp:console//ufcpp:comment">
<span class="floatcomment">
<xsl:if test="@left!=''">
  <xsl:attribute name="style">
    left:<xsl:value-of select="@left"/>em;
  </xsl:attribute>
</xsl:if>
← <xsl:value-of select="text()"/>
</span>
</xsl:template>

<xsl:template match="ufcpp:console//ufcpp:prompt">
	<xsl:variable name="char"><xsl:choose><xsl:when test="@char!=''"><xsl:value-of select="@char" /></xsl:when><xsl:otherwise>&gt;</xsl:otherwise></xsl:choose></xsl:variable>

<span class="prompt">
<xsl:value-of select="$char"/><xsl:text> </xsl:text>
</span>
</xsl:template>

<!-- ********** refsource ********** -->
<!-- 外部ソースファイルの参照 -->
<xsl:template match="ufcpp:refsource">
<a href="{@src}"><xsl:value-of select="@name"/></a>
</xsl:template>

<!-- ********** sourcelist ********** -->
<!-- 外部ソースファイルの一覧作成 -->
<xsl:template match="ufcpp:sourcelist">
<h3>ソースファイルリスト</h3>
<ul>
<xsl:for-each select="//ufcpp:refsource">
<li><a href="{@src}"><xsl:value-of select="@name"/></a></li>
</xsl:for-each>
</ul>
</xsl:template>

</xsl:stylesheet>
