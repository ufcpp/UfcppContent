---
title: "キーワードの参照"
source_url: "https://ufcpp.net/study/xml/summary/keyword/"
content_type: "Article"
published_at: "2015-05-06T14:24:18"
updated_at: "2015-07-07T18:23:42"
tags: []
umbraco_id: 1655
parent_id: 1650
sort_order: 4
aliases:
  - "/study/testxsl/keyword"
  - "/study/testxsl/keyword.html"
  - "/testxsl/keyword"
  - "/testxsl/keyword.html"
  - "/xml/summary/keyword/"
---

# キーワードの参照

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
[<code>keyword.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/keyword.xsl) には、キーワードの強調、一覧表示のための template が記述されています。

各ドキュメントの先頭には、目次に続き、キーワードの一覧が表示されます。

また、他のドキュメントからキーワードを参照することも出来ます。


##<a id="sec-generated-title-2"></a> <a id="source"></a>ソース
<pre class="xsource" title="">
<code><span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">keyword</span> <span class="attribute">id</span><span class="attvalue">="tag"</span><span class="bracket">&gt;</span>keyword タグ<span class="bracket">&lt;/</span><span class="element">keyword</span><span class="bracket">&gt;</span> でキーワードの強調表示が出来ます。
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
  refkey タグを使うと、keyword タグで囲ったキーワードを参照出来ます。
  例 → <span class="bracket">&lt;</span><span class="element">refkey</span> <span class="attribute">doc</span><span class="attvalue">="keyword"</span> <span class="attribute">id</span><span class="attvalue">="tag"</span> <span class="bracket">/&gt;</span>。
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
</code></pre>

##<a id="sec-generated-title-3"></a> <a id="result"></a>結果
<strong id="tag" class="keyword">keyword タグ</strong> でキーワードの強調表示が出来ます。

refkey タグを使うと、keyword タグで囲ったキーワードを参照出来ます。
例 → 「[keyword タグ](#tag)」。
