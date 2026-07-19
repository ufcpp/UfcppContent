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
  - "/study/testxsl/nomenu"
  - "/study/testxsl/nomenu.html"
  - "/testxsl/nomenu"
  - "/testxsl/nomenu.html"
  - "/xml/summary/nomenu/"
---

# ドキュメントのパラメータ

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<code>document.xsl</code> のパラメータの例です。

このページだけ、他のページと異なり、
メニュー非表示パラメータを設定したスタイルシート [<code>nomenu.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/nomenu.xsl) を利用しています。
（他のページは [<code>main.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/main.xsl) を利用。）

<code>nomenu.xsl</code> には、以下のようなパラメータが設定されています。
これで、目次も索引もキーワードリストも出なくなります。


<pre class="xsource" title="パラメータ">
<code><span class="bracket">&lt;</span><span class="element">xsl:param</span> <span class="attribute">name</span><span class="attvalue">="DocumentMenu"</span><span class="bracket">&gt;</span>no<span class="bracket">&lt;/</span><span class="element">xsl:param</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">xsl:param</span> <span class="attribute">name</span><span class="attvalue">="DocumentIndex"</span><span class="bracket">&gt;</span>no<span class="bracket">&lt;/</span><span class="element">xsl:param</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">xsl:param</span> <span class="attribute">name</span><span class="attvalue">="DocumentKeyword"</span><span class="bracket">&gt;</span>no<span class="bracket">&lt;/</span><span class="element">xsl:param</span><span class="bracket">&gt;</span>
</code></pre>
