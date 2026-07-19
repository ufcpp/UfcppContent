<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document"
  >

<!--
ドキュメントルート document の処理。
-->

<xsl:import href="index.xsl"    /> <!-- インデックスページ用 -->
<xsl:import href="section.xsl"  /> <!-- 章の定義、参照 -->
<xsl:import href="silverlight.xsl" />

<!-- ********** パラメータ ********** -->
<xsl:param name="DocumentMenu">yes</xsl:param>
<xsl:param name="DocumentIndex">yes</xsl:param>
<xsl:param name="DocumentKeyword">yes</xsl:param>

<xsl:variable name="index" select="document(concat($dir,'/index.xml'))/ufcpp:index"/>

<!-- ********** ドキュメントルート ********** -->
<xsl:template match="/ufcpp:document">
<xsl:variable name="pageTitle" select="@title" />
<xsl:variable name="pageId"><xsl:value-of select="$index//ufcpp:document[document(concat($dir,'/',@href,'.xml'))/ufcpp:document/@title=$pageTitle]/@href"/></xsl:variable>
<xsl:variable name="pageUrl"><xsl:value-of select="$rootUrl" /><xsl:value-of select="$dir" />/<xsl:value-of select="$pageId" />.html</xsl:variable>

<html lang="ja-JP">
<head>
<meta http-equiv="Content-Language" content="ja-JP" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<meta http-equiv="Content-Script-Type" content="text/javascript" />
<meta name="Author" content="IWANAGA Nobuyuki" />

<link rel="stylesheet" href="../main.css" />
<link rel="stylesheet" href="../document.css" />

<!-- 必要な CSS スタイルシートの読み込み。 -->
<xsl:for-each select="document('main.xsl')/xsl:stylesheet/xsl:import">
 <xsl:for-each select="document(@href)//xsl:variable[@name='css']">
  <link rel="stylesheet" href="../{@select}.css"/>
 </xsl:for-each>
</xsl:for-each>

<!-- ドキュメントごとのスタイルシート指定 -->
<xsl:for-each select="ufcpp:css">
  <link rel="stylesheet" href="{@name}.css"/>
</xsl:for-each>

<!-- 内部スタイル -->
<xsl:if test="ufcpp:style!=''">
<style>
<xsl:value-of select="style"/>
</style>
</xsl:if>

<!-- meta タグ(keywords)にキーワード一覧を記載 -->
<meta name="keywords">
<xsl:attribute name="content">
<xsl:for-each select="//ufcpp:keyword"><xsl:value-of select="./text()" />,</xsl:for-each><xsl:value-of select="$index/@keyword" />
</xsl:attribute>
</meta>

<!-- ページタイトル -->
<title>
<xsl:value-of select="/ufcpp:document/@title" />
(<xsl:value-of select="$index/@title" />)
</title>

  <xsl:call-template name="SilverlightHeader" />

  <link type="text/css" href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/themes/base/jquery-ui.css" rel="stylesheet" />

  <script src="http://www.google.com/jsapi"></script>
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" type="text/javascript"></script>
  <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.13/jquery-ui.min.js" type="text/javascript"></script>

  <!--<script src="http://ajax.microsoft.com/ajax/jquery/jquery-1.6.min.js" type="text/javascript" />-->

</head>
<body>
<xsl:if test="$DocumentMenu='yes'">
<xsl:attribute name="class"><xsl:text>Menu</xsl:text></xsl:attribute>
</xsl:if>

<!-- ↓メイン（ヘッダ、本文、フッタ） -->
<div>
<xsl:if test="$DocumentMenu='yes'">
<xsl:attribute name="class">Main</xsl:attribute>
</xsl:if>

<!-- ↓全ページに共通のヘッダ -->
<div class="CommonHeader">
  <xsl:variable name="commonh" select="document('common/header')" />
  <xsl:apply-templates select="$commonh/content/*"/>
</div>
<!-- ↑全ページに共通のヘッダ -->

<!-- ↓現ページのヘッダ -->
<div class="Header">

  <p>
    <a href="../../index.html">Top</a> ≫ <a href="../index.html">総合 目次</a> ≫ <a href="index.html"><xsl:value-of select="$index/@title" /></a>
  </p>

  <h1 id="pagetitle"><xsl:value-of select="/ufcpp:document/@title" /></h1>

  <!-- いいね！ -->
  <p>
    <!-- hatena -->
    <a href="http://b.hatena.ne.jp/entry/{$pageUrl}" class="hatena-bookmark-button" data-hatena-bookmark-layout="standard" title="このエントリーをはてなブックマークに追加">
      <img src="http://b.st-hatena.com/images/entry-button/button-only.gif" alt="このエントリーをはてなブックマークに追加" width="20" height="20" style="border: none;" />
    </a>
    <script type="text/javascript" src="http://b.st-hatena.com/js/bookmark_button.js" charset="utf-8" async="async"></script>

    <!-- twitter -->
    <a href="http://twitter.com/share" class="twitter-share-button" data-url="{$pageUrl}" data-text="{$pageTitle}" data-count="horizontal" data-lang="ja">Tweet</a>
    <script type="text/javascript" src="http://platform.twitter.com/widgets.js"></script>

    <!-- facebook -->
    <iframe src="http://www.facebook.com/plugins/like.php?href={$pageUrl}&amp;layout=button_count&amp;show_faces=false&amp;width=50&amp;action=like&amp;colorscheme=light&amp;height=21" scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:105px; height:21px;" allowTransparency="true"></iframe>
    
    <!-- google +1 -->
    <script type="text/javascript" src="https://apis.google.com/js/plusone.js"></script>
    <g:plusone xmlns:g="http://base.google.com/ns/1.0" size="small"></g:plusone>
  </p>

<!-- DocumentIndex が yes のときのみ目次を表示 -->
<xsl:if test="$DocumentIndex='yes'">
<h4>目次</h4>
<xsl:call-template name="MakeIndex">
 <xsl:with-param name="root" select="/ufcpp:document"/>
</xsl:call-template>
</xsl:if>

<!-- DocumentKeyword が yes のときのみキーワードを表示 -->
<xsl:if test="$DocumentKeyword='yes'">
<h4>キーワード</h4>
<ul>
<xsl:for-each select="//ufcpp:keyword">
	<xsl:variable name="id">
		<xsl:choose>
		<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
		<xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<li><a href="#{$id}"><xsl:value-of select="./text()" /></a></li>
</xsl:for-each>
</ul>
</xsl:if>

<!-- ↓メニュー下、本文前の欄 -->
<div class="CommonHeader">
  <xsl:variable name="commonum" select="document('common/undermenu')" />
  <xsl:apply-templates select="$commonum/content/*"/>
</div>

</div>
<!-- ↑現ページのヘッダ -->

<div class="Middle">

<!-- ↓本文 -->
<div class="Body">
<xsl:apply-templates select="/ufcpp:document/*"/>
</div>
<!-- ↑本文 -->

<!-- ↓広告欄 -->
<div class="Ad">
  <xsl:variable name="ads" select="document('common/ads')" />
  <xsl:apply-templates select="$ads/content/*"/>
  <xsl:apply-templates select="$index/ufcpp:Ads/*"/>
  <xsl:apply-templates select="/document/ufcpp:Ads/*"/>
</div>
<!-- ↑広告欄 -->

</div>

<!-- ↓現ページのフッタ -->
<div class="Footer">

  <!-- ↓本分下広告 -->
  <div class="CommonHeader">
    <xsl:variable name="commonum" select="document('common/underbody')" />
    <xsl:apply-templates select="$commonum/content/*"/>
  </div>
  
  <p>
<xsl:if test="/ufcpp:document/@since!=''">
Since: <xsl:value-of select="/ufcpp:document/@since" />.
</xsl:if>
<xsl:if test="/ufcpp:document/@update!=''">
 Last Update: <xsl:value-of select="/ufcpp:document/@update" />.
</xsl:if>
</p>

<p>
<a href="#pagetitle">このページの先頭に戻る</a>
</p>
<p>
<a href="index.html" accesskey="i"><xsl:value-of select="$index/@title" /> 目次に戻る(<span class="accesskey">i</span>)</a>
</p>

<xsl:variable name="thisTitle" select="/ufcpp:document/@title"/>
<xsl:for-each select="$index//ufcpp:document">
  <xsl:variable name="xmlname" select="concat($dir,'/',@href,'.xml')" />
  <xsl:variable name="title" select="document($xmlname)/ufcpp:document/@title"/>

  <xsl:if test="$title=$thisTitle">
    <xsl:variable name="prehref" select="preceding::ufcpp:document[position()=1]/@href"/>
    <xsl:variable name="prename" select="concat($dir,'/',$prehref,'.xml')"/>
    <xsl:if test="$prehref!=''">
      <xsl:variable name="pre" select="document($prename)"/>
      <p>
      <a href="{$prehref}.html" accesskey="p">＜＜ 前(<span class="accesskey">p</span>) 「<xsl:value-of select="$pre/ufcpp:document/@title"/>」</a>
      </p>
    </xsl:if>

    <xsl:variable name="folhref" select="following::ufcpp:document[position()=1]/@href"/>
    <xsl:variable name="folname" select="concat($dir,'/',$folhref,'.xml')"/>
    <xsl:if test="$folhref!=''">
      <xsl:variable name="fol" select="document($folname)"/>
      <p>
      <a href="{$folhref}.html" accesskey="n">＞＞ 次(<span class="accesskey">n</span>) 「<xsl:value-of select="$fol/ufcpp:document/@title"/>」</a>
      </p>
    </xsl:if>
  </xsl:if>
</xsl:for-each>
</div>
<!-- ↑現ページのフッタ -->

<!-- ↓全ページに共通のフッタ -->
<div class="CommonHeader">
  <xsl:variable name="commonf" select="document('common/footer')" />
  <xsl:apply-templates select="$commonf/content/*"/>
</div>
<!-- ↑全ページに共通のフッタ -->

</div>
<!-- ↑メイン（ヘッダ、本文、フッタ） -->

<!-- ↓メニュー -->
<xsl:if test="$DocumentMenu='yes'">

<div class="MenuList">

<!--<div class="GeneralIndex">
<ul>
<li>≫ <a href="../../index.html">Top</a></li>
<li>≫ <a href="../index.html">総合 目次</a></li>
<li>≫ <a href="index.html"><xsl:value-of select="$index/@title" /></a></li>
</ul>
</div>-->

<!-- ↓全ページに共通のヘッダ in メニュー -->
<div class="CommonMenu">
  <xsl:variable name="commonmh" select="document('common/mheader')" />
  <xsl:apply-templates select="$commonmh/content/*"/>
</div>
<!-- ↑全ページに共通のヘッダ in メニュー -->

<div class="MenuIndex">
<xsl:call-template name="MakeDocIndex">
  <xsl:with-param name="root" select="$index"/>
  <xsl:with-param name="class" select="'documentIndex'"/>
</xsl:call-template>
</div>

<!-- ↓全ページに共通のフッタ in メニュー -->
<div class="CommonMenu">
  <xsl:variable name="commonmf" select="document('common/mfooter')" />
  <xsl:apply-templates select="$commonmf/content/*"/>
</div>
<!-- ↑全ページに共通のフッタ in メニュー -->

</div>
</xsl:if>
<!-- ↑メニュー -->

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

<!-- 目次作成 -->
<xsl:template name="MakeIndex">
<xsl:param name="root" select="/ufcpp:document"/>

<xsl:if test="./ufcpp:section!=''">
	<ul>
	<xsl:for-each select="$root/ufcpp:section">
		<xsl:variable name="id">
			<xsl:choose>
			<xsl:when test="@id!=''"><xsl:value-of select="@id"/></xsl:when>
			<xsl:otherwise><xsl:value-of select="generate-id()"/></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<li>
			<a href="#{$id}"><xsl:value-of select="@title" /></a>
			<xsl:call-template name="MakeIndex">
				<xsl:with-param name="root" select="."/>
			</xsl:call-template>
		</li>

	</xsl:for-each>
	</ul>
</xsl:if>
</xsl:template>

  <xsl:template match="ufcpp:expand">
    <p>
      <span title="展開/折畳" style="background-color:#ddddff;" onClick="$('#{@id}').slideToggle()">&#xA0;⊞&#xA0;</span> 
      （<xsl:value-of select="@description" />）
    </p>
    <div id="{@id}" style="display:none;">
      <xsl:apply-templates select="*|text()"/>
    </div>
  </xsl:template>

</xsl:stylesheet>
