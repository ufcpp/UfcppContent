<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
	version="2.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:ufcpp="http://ufcpp.net/study/document">

  <!--
外部スタイルのインポート。
不要なものはコメントアウトしてください。
-->
<xsl:import href="general.xsl"  /> <!-- スタイルシートで未定義のタグをそのままコピー -->
<xsl:import href="mathenv.xsl"  /> <!-- 数式利用用 -->
<xsl:import href="keyword.xsl"  /> <!-- キーワードの定義、参照 -->
<xsl:import href="variable.xsl" /> <!-- 変数の定義、参照 -->
<xsl:import href="figure.xsl"   /> <!-- 図表環境 -->
<xsl:import href="source.xsl"   /> <!-- ソースファイルの表示 -->
<xsl:import href="exercise.xsl" /> <!-- 演習問題 -->
<xsl:import href="link.xsl"     /> <!-- リンク集 -->
<xsl:import href="qanda.xsl"    /> <!-- Q＆A -->
<xsl:import href="amazon.xsl"   /> <!-- Amazon 広告 -->
<xsl:import href="document.xsl" /> <!-- ドキュメントルート -->

  <xsl:variable name="rootUrl">http://ufcpp.net/study/</xsl:variable>

  <!--
document のパラメータ設定。
いずれも、デフォルトでは yes
DocumentMenu    … 左側にメニューを表示するかどうか。
DocumentIndex   … 目次を表示するかどうか。
DocumentKeyword … キーワードリストを表示するかどうか。
-->
<!-- xsl:param name="DocumentMenu">no</xsl:param -->
<!-- xsl:param name="DocumentIndex">no</xsl:param -->
<!-- xsl:param name="DocumentKeyword">no</xsl:param -->

</xsl:stylesheet>
