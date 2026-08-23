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
  - "/study/testxsl/document.html"
---

# ドキュメント

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

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


## <a id="sec-generated-title-2"></a> <a id="source"></a>ソース

このドキュメント自体のソースは以下のようになっています。


```xml {title="ソース"}
<document title="ドキュメント" xmlns="http://ufcpp.net/study/document">
  <section title="概要" id="abst">
    <p>
      <a href="../document.xsl">
        <code>document.xsl</code>
      </a> および <a href="../index.xsl">
        <code>index.xsl</code>
      </a> は、ドキュメント表示用の template が記述されている、
      必須 xsl です。
    </p>
    <p>
      <code>document.xsl</code> を利用する際には、<a href="../section.xsl">
        <code>section.xsl</code>
      </a> および <a href="../general.xsl">
        <code>general.xsl</code>
      </a> も必要です。
      （参考： <link doc="section" />、<link doc="general" />）
    </p>
    <p>
      <code>document.xsl</code> では、パラメータを変更することで、
      メニュー、目次、キーワードリストの表示・非表示を切り替え可能です。
      （パラメータは <code>main.xsl</code> に記述します。）
    </p>
    <ul>
      <li>
        <code>DocumentMenu   </code> … 左側にメニューを表示するかどうか。
      </li>
      <li>
        <code>DocumentIndex  </code> … 目次を表示するかどうか。
      </li>
      <li>
        <code>DocumentKeyword</code> … キーワードリストを表示するかどうか。
      </li>
    </ul>
    <p>
      メニュー、目次、キーワードリストを非表示にした例 → <link doc="nomenu" />。
    </p>
  </section>
  <!-- 再帰になっちゃうのでこのセクションは省略 -->
</document>
```
