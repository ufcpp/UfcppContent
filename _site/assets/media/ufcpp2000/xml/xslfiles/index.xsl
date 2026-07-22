<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document"
  >

<!--
インデックスページのドキュメントルート index の処理。
-->
<xsl:variable name="indexTitle"><xsl:value-of select="/ufcpp:index/@title" /></xsl:variable>
<xsl:variable name="indexUrl"><xsl:value-of select="$rootUrl" /><xsl:value-of select="$dir" />/</xsl:variable>

  <!-- ********** インデックス用 ********** -->
<xsl:template match="/ufcpp:index">
<html lang="ja-JP">

<head>
<meta http-equiv="Content-Language" content="ja-JP" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<meta http-equiv="Content-Script-Type" content="text/javascript" />
<meta name="Author" content="IWANAGA Nobuyuki" />

<xsl:if test="@keyword!=''">
  <meta name="keywords" content="{@keyword}"/>
</xsl:if>
<xsl:if test="@description!=''">
  <meta name="description" content="{@description}"/>
</xsl:if>

<meta http-equiv="Content-Style-Type" content="text/css" />
<meta http-equiv="Content-Script-Type" content="text/javascript" />

<link rel="stylesheet" href="../main.css" />
<link rel="stylesheet" href="../index.css" />

<!-- ドキュメントごとのスタイルシート指定 -->
<xsl:for-each select="ufcpp:css">
  <link rel="stylesheet">
  <xsl:attribute name="href"><xsl:value-of select="@name"/>.css</xsl:attribute>
  </link>
</xsl:for-each>

<title><xsl:value-of select="$indexTitle" /></title>

  <script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.6.js" type="text/javascript" />
</head>
<body>

<!-- ↓インデックスページヘッダ -->
<div class="CommonHeader">
  <xsl:variable name="commonih" select="document('common/iheader')" />
  <xsl:apply-templates select="$commonih/content/*"/>
</div>
<!-- ↑インデックスページヘッダ -->

<h1><xsl:value-of select="$indexTitle" /></h1>

  <!-- いいね！ -->
  <p>
    <!-- hatena -->
    <a href="http://b.hatena.ne.jp/entry/{$indexUrl}" class="hatena-bookmark-button" data-hatena-bookmark-layout="standard" title="このエントリーをはてなブックマークに追加">
      <img src="http://b.st-hatena.com/images/entry-button/button-only.gif" alt="このエントリーをはてなブックマークに追加" width="20" height="20" style="border: none;" />
    </a>
    <script type="text/javascript" src="http://b.st-hatena.com/js/bookmark_button.js" charset="utf-8" async="async"></script>

    <!-- twitter -->
    <a href="http://twitter.com/share" class="twitter-share-button" data-url="{$indexUrl}" data-text="{$indexTitle}" data-count="horizontal" data-lang="ja">Tweet</a>
    <script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>

    <!-- facebook -->
    <iframe src="http://www.facebook.com/plugins/like.php?href={$indexUrl}&amp;layout=button_count&amp;show_faces=false&amp;width=50&amp;action=like&amp;colorscheme=light&amp;height=21" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:105px; height:21px;" allowTransparency="true"></iframe>

    <!-- google +1 -->
    <script type="text/javascript" src="https://apis.google.com/js/plusone.js"></script>
    <g:plusone xmlns:g="http://base.google.com/ns/1.0" size="small"></g:plusone>
  </p>

  <xsl:apply-templates select="ufcpp:summary"/>

<xsl:call-template name="MakeSectionIndex">
  <xsl:with-param name="root" select="/ufcpp:index"/>
</xsl:call-template>

  <!-- ↓メニュー下、本文前の欄 -->
  <div class="CommonHeader">
    <xsl:variable name="commonum" select="document('common/undermenu')" />
    <xsl:apply-templates select="$commonum/content/*"/>
  </div>
  <!-- ↑メニュー下、本文前の欄 -->

  <div class="Index">
<xsl:call-template name="MakeDocIndex">
<xsl:with-param name="root" select="/ufcpp:index"/>
</xsl:call-template>
</div>

<p>
<xsl:if test="/ufcpp:index/@since!=''">
Since: <xsl:value-of select="/ufcpp:index/@since" />.
</xsl:if>
<xsl:if test="/ufcpp:index/@update!=''">
 Last Update: <xsl:value-of select="/ufcpp:index/@update" />.
</xsl:if>
</p>

<p class="footer">
<a href="../index.html" accesskey="i">勉強用ページ 目次に戻る(<span class="accesskey">i</span>)</a>
</p>

<!-- ↓インデックスページフッタ -->
<div class="CommonHeader">
  <xsl:apply-templates select="/ufcpp:index/ufcpp:IAds/*"/>
  <xsl:variable name="commonif" select="document('common/ifooter')" />
  <xsl:apply-templates select="$commonif/content/*"/>
</div>
<!-- ↑インデックスページフッタ -->

<script src="http://www.google-analytics.com/urchin.js" type="text/javascript">
<xsl:text> </xsl:text>
</script>
<script type="text/javascript">
<![CDATA[
_uacct = "UA-887343-1";
urchinTracker();
]]>
</script>

</body>
</html>
</xsl:template>

<!-- この分野のサマリー -->
<xsl:template match="/ufcpp:index/ufcpp:summary">
<xsl:apply-templates select="*"/>
</xsl:template>

  <!-- セクション見出し作成 -->
  <xsl:template name="MakeSectionIndex">
    <xsl:param name="root" select="/ufcpp:index"/>

    <xsl:if test="./ufcpp:section!=''">
      <ul>
        <xsl:for-each select="$root/ufcpp:section">
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

          <li>
            <a href="#index-{$id}">
              <xsl:value-of select="@title" />
            </a>
            <xsl:call-template name="MakeIndex">
              <xsl:with-param name="root" select="."/>
            </xsl:call-template>
          </li>

        </xsl:for-each>
      </ul>
    </xsl:if>
  </xsl:template>

  <!-- インデックス作成 -->
  <xsl:template name="MakeDocIndex">
    <xsl:param name="root" select="/ufcpp:index"/>
    <xsl:param name="class" select="'index'"/>

    <ul class="documentIndex">
      <xsl:attribute name="class">
        <xsl:value-of select="$class"/>
      </xsl:attribute>
      <xsl:apply-templates select="$root/ufcpp:document|$root/ufcpp:section|$root/ufcpp:extern"/>
    </ul>
  </xsl:template>

<xsl:template match="ufcpp:index//ufcpp:document">
<xsl:variable name="xmlname" select="concat($dir,'/',@href,'.xml')" />
<xsl:variable name="htmlname" select="concat('../',$dir,'/',@href,'.html')" />

  <li><a href="{$htmlname}">
    <xsl:if test="@indexid!=''">
      <xsl:attribute name="id">index-<xsl:value-of select="@indexid"/></xsl:attribute>
    </xsl:if><xsl:value-of select="document($xmlname)/ufcpp:document/@title" /></a></li>
</xsl:template>

<xsl:template match="ufcpp:index//ufcpp:refindex">
<xsl:variable name="xmlname" select="concat($dir,'/',@href,'.xml')" />
<xsl:variable name="htmlname" select="concat('../',$dir,'/',@href,'.html')" />

  <li><a href="{$htmlname}"><xsl:value-of select="document($xmlname)/ufcpp:index/@title" /></a></li>
</xsl:template>

<xsl:template match="ufcpp:index//ufcpp:section">
<li>
<h2 onClick="$('#indexlist-{@id}').slideToggle();" class="indexSection">
  <a id="index-{@id}">▶ <xsl:value-of select="@title" /></a>
</h2><ul id="indexlist-{@id}" class="index"><xsl:apply-templates /></ul></li>
</xsl:template>

<xsl:template match="ufcpp:index//ufcpp:extern">
<li><a href="{@href}"><xsl:value-of select="@title" /></a></li>
</xsl:template>

</xsl:stylesheet>
