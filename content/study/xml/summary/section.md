---
title: "章の参照"
source_url: "https://ufcpp.net/study/xml/summary/section/"
content_type: "Article"
published_at: "2015-05-06T14:24:16"
updated_at: "2015-07-07T18:21:46"
tags: []
umbraco_id: 1654
parent_id: 1650
sort_order: 3
aliases:
  - "/study/testxsl/section"
  - "/study/testxsl/section.html"
  - "/testxsl/section"
  - "/testxsl/section.html"
  - "/xml/summary/section/"
---

# 章の参照

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
[<code>section.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/section.xsl) には、章の表示（サブサブセクションまで階層的に表示）や、
章の参照、ドキュメントへのリンクのための template が記述されています。


##<a id="sec-generated-title-2"></a> <a id="source"></a>ソース
<pre class="xsource" title="ソース">
<code><span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="結果"</span> <span class="attribute">id</span><span class="attvalue">="result"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    section タグでセクションを作ります。
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="サブセクション1"</span> <span class="attribute">id</span><span class="attvalue">="sub1"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      section タグの中に section タグを入れ子にすると、
      サブセクションになります。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="サブセクション2"</span> <span class="attribute">id</span><span class="attvalue">="sub2"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      サブセクション2
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      ref タグで、他のセクションを参照することが出来ます。
      例 → <span class="bracket">&lt;</span><span class="element">ref</span> <span class="attribute">doc</span><span class="attvalue">="section"</span> <span class="attribute">id</span><span class="attvalue">="result"</span> <span class="bracket">/&gt;</span>。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      また、link タグで、他のドキュメントへのリンクを作れます。
      例 → <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">doc</span><span class="attvalue">="document"</span> <span class="bracket">/&gt;</span>。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="サブサブセクション"</span> <span class="attribute">id</span><span class="attvalue">="subsub"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
        さらに入れ子にすることで、サブサブセクションまで表示できます。
      <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
</code></pre>
id 属性を省略すると、ランダムな ID を割り当ててくれる機能もありますが、非推奨です。
（[XSD](../../../../assets/media/ufcpp2000/xsd/xsd.zip) では id 属性を必須属性にしてあります。）


##<a id="sec-generated-title-3"></a> <a id="result"></a>結果
section タグでセクションを作ります。


###<a id="sec-generated-title-4"></a> <a id="sub1"></a>サブセクション1
section タグの中に section タグを入れ子にすると、
サブセクションになります。


###<a id="sec-generated-title-5"></a> <a id="sub2"></a>サブセクション2
サブセクション2

ref タグで、他のセクションを参照することが出来ます。
例 → 「[結果](#result)」。

また、link タグで、他のドキュメントへのリンクを作れます。
例 → 「[ドキュメント](document.md)」。


####<a id="sec-generated-title-6"></a> <a id="subsub"></a>サブサブセクション
さらに入れ子にすることで、サブサブセクションまで表示できます。
