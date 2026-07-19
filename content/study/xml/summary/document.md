---
title: "ドキュメント"
source_url: "https://ufcpp.net/study/xml/summary/document/"
content_type: "Article"
published_at: "2015-05-06T14:24:08"
updated_at: "2015-07-07T18:06:55"
tags: []
umbraco_id: 1651
parent_id: 1650
sort_order: 0
aliases:
  - "/study/testxsl/document"
  - "/study/testxsl/document.html"
  - "/testxsl/document"
  - "/testxsl/document.html"
  - "/xml/summary/document/"
---

# ドキュメント

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
[<code>document.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/document.xsl) および [<code>index.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/index.xsl) は、ドキュメント表示用の template が記述されている、
必須 xsl です。

<code>document.xsl</code> を利用する際には、[<code>section.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/section.xsl) および [<code>general.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/general.xsl) も必要です。
（参考： 「[章の参照](section.md)」、「[未定義タグ](general.md)」）

<code>document.xsl</code> では、パラメータを変更することで、
メニュー、目次、キーワードリストの表示・非表示を切り替え可能です。
（パラメータは <code>main.xsl</code> に記述します。）

* <code>DocumentMenu   </code>… 左側にメニューを表示するかどうか。

* <code>DocumentIndex  </code>… 目次を表示するかどうか。

* <code>DocumentKeyword</code>… キーワードリストを表示するかどうか。


メニュー、目次、キーワードリストを非表示にした例 → 「[ドキュメントのパラメータ](nomenu.md)」。


##<a id="sec-generated-title-2"></a> <a id="source"></a>ソース
このドキュメント自体のソースは以下のようになっています。


<pre class="xsource" title="ソース">
<code><span class="bracket">&lt;</span><span class="element">document</span> <span class="attribute">title</span><span class="attvalue">="ドキュメント"</span> <span class="attribute">xmlns</span><span class="attvalue">="http://ufcpp.net/study/document"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="概要"</span> <span class="attribute">id</span><span class="attvalue">="abst"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../document.xsl"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>document.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span> および <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../index.xsl"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>index.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span> は、ドキュメント表示用の template が記述されている、
      必須 xsl です。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>document.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> を利用する際には、<span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../section.xsl"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>section.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span> および <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../general.xsl"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>general.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span> も必要です。
      （参考： <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">doc</span><span class="attvalue">="section"</span> <span class="bracket">/&gt;</span>、<span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">doc</span><span class="attvalue">="general"</span> <span class="bracket">/&gt;</span>）
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>document.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> では、パラメータを変更することで、
      メニュー、目次、キーワードリストの表示・非表示を切り替え可能です。
      （パラメータは <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>main.xsl<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> に記述します。）
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">ul</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>DocumentMenu   <span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> … 左側にメニューを表示するかどうか。
      <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>DocumentIndex  <span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> … 目次を表示するかどうか。
      <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">code</span><span class="bracket">&gt;</span>DocumentKeyword<span class="bracket">&lt;/</span><span class="element">code</span><span class="bracket">&gt;</span> … キーワードリストを表示するかどうか。
      <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      メニュー、目次、キーワードリストを非表示にした例 → <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">doc</span><span class="attvalue">="nomenu"</span> <span class="bracket">/&gt;</span>。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
  <span class="comment">&lt;!-- 再帰になっちゃうのでこのセクションは省略 --&gt;</span>
<span class="bracket">&lt;/</span><span class="element">document</span><span class="bracket">&gt;</span>
</code></pre>
