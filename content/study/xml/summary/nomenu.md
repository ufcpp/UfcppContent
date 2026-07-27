---
title: "ドキュメントのパラメータ"
source_url: "https://ufcpp.net/study/xml/summary/nomenu/"
content_type: "Article"
published_at: "2015-05-06T14:24:11"
updated_at: "2015-07-07T18:18:14"
tags: []
umbraco_id: 1652
parent_id: 1650
sort_order: 1
aliases:
  - "/study/testxsl/nomenu.html"
---

# ドキュメントのパラメータ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<code>document.xsl</code> のパラメータの例です。

このページだけ、他のページと異なり、
メニュー非表示パラメータを設定したスタイルシート [<code>nomenu.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/nomenu.xsl) を利用しています。
（他のページは [<code>main.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/main.xsl) を利用。）

<code>nomenu.xsl</code> には、以下のようなパラメータが設定されています。
これで、目次も索引もキーワードリストも出なくなります。


```xml
<xsl:param name="DocumentMenu">no</xsl:param>
<xsl:param name="DocumentIndex">no</xsl:param>
<xsl:param name="DocumentKeyword">no</xsl:param>
```
