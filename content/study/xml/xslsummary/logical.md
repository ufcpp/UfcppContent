---
title: "論理マークアップとデザインの変更"
source_url: "https://ufcpp.net/study/xml/xslsummary/logical/"
content_type: "Article"
published_at: "2015-05-06T14:23:58"
updated_at: "2015-05-06T14:23:58"
tags: []
umbraco_id: 1647
parent_id: 1645
sort_order: 1
aliases:
  - "/study/testxsl/logical"
  - "/study/testxsl/logical.html"
  - "/testxsl/logical"
  - "/testxsl/logical.html"
  - "/xml/xslsummary/logical/"
---

# 論理マークアップとデザインの変更

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
HTML を直接書くのではなく、
一度 XML で書いてから XSL で変換をかける利点の1つは、
論理マークアップと視覚デザインを分離できることです。

論理と視覚が分離されているので、視覚デザインの変更も容易です。


##<a id="sec-generated-title-2"></a> <a id="separation"></a>論理と視覚の分離
例として更新履歴を見てみましょう。

勉強ページの更新履歴の元データは以下のような XML になっています。


<pre class="xsource" title="whatsnew.xml">
<code><span class="bracket">&lt;</span><span class="element">whatsnew</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">new</span> <span class="attribute">year</span><span class="attvalue">="2000"</span> <span class="attribute">month</span><span class="attvalue">="12"</span> <span class="attribute">day</span><span class="attvalue">="24"</span>
  <span class="attribute">url</span><span class="attvalue">="../csharp/index.html"</span><span class="bracket">&gt;</span>
  C#の解説ページ作りました
<span class="bracket">&lt;/</span><span class="element">new</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">new</span> <span class="attribute">year</span><span class="attvalue">="2000"</span> <span class="attribute">month</span><span class="attvalue">="12"</span> <span class="attribute">day</span><span class="attvalue">="23"</span> <span class="attribute">url</span>=""<span class="bracket">&gt;</span>
  ホームページ開設
<span class="bracket">&lt;/</span><span class="element">new</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">whatsnew</span><span class="bracket">&gt;</span>
</code></pre>
更新履歴に必要な情報というと、更新日時と更新したページのURLと更新内容だけあれば十分なわけです。

こういう情報をブラウザで表示したい場合、テーブルか何かを使って一覧表示したいですよね。
こんな感じで↓

<blockquote markdown="1">
<table summary="">

	<tr>
		<td markdown="1">2000年12月24日</td>
		<td markdown="1">[C#の解説ページ作りました](../../csharp/index.md)</td>
	</tr>
	<tr>
		<td markdown="1">2000年12月23日</td>
		<td markdown="1">ホームページ開設</td>
	</tr>
</table>


</blockquote>
HTMLのソースはこんな感じです↓


<pre class="xsource" title="更新履歴をテーブルで表示">
<code><span class="bracket">&lt;</span><span class="element">table</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>2000年12月24日<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../programming/csharp/index.html"</span><span class="bracket">&gt;</span>
   C#の解説ページ作りました
 <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>2000年12月23日<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>ホームページ開設<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
</code></pre>
元の XML と変換後の HTML には以下のような決定的な違いがあります。

* 元の XML:
    * データの意味（この数字は日付だとか、この項目は URL だとか）がわかる。

    * 視覚的な配置情報は一切ない。



* 変換後の HTML:
    * 意味は失われている。

    * 視覚的な情報（どこに何を配置するか）が含まれている。




この「データの意味」の方を<strong id="logic" class="keyword">論理デザイン</strong>（logical design）、
「視覚的な配置情報」の方を<strong id="visual" class="keyword">視覚デザイン</strong>（visual design）と言います。

要するに、XML の方では論理的なタグの中に情報がありますが、
HTML の方では視覚的なタグの中に情報が埋まっています。

「別に HTML で視覚デザイン中に直接データを埋め込んじゃってもいいじゃないか」と思うかもしれませんが、
そうすると、デザインを変更したいときにかなり大変になります。
例えば、テーブルを使うのをやめて、


<pre class="xsource" title="whatsnew.xml を単なる p タグで表示">
<code><span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
2000年12月24日 <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">target</span><span class="attvalue">="_top"</span>
  <span class="attribute">href</span><span class="attvalue">="../csharp/index.html"</span><span class="bracket">&gt;</span>
  C#の解説ページ作りました
<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">br</span><span class="bracket">&gt;</span>
2000年12月23日 ホームページ開設<span class="bracket">&lt;</span><span class="element">br</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
</code></pre>
こんな感じ↑に変更したくなったときに、いちいち更新履歴全体を書き換えるのはかなり面倒なわけで、
こういう視覚デザイン的な情報は更新履歴という論理的な情報とは分離しておいた方が変更作業が楽になるわけです。


##<a id="sec-generated-title-3"></a> <a id="xsl"></a>XSL による論理と視覚の分離
XSL というのは、まさに「論理と視覚の分離」のためにあります。
XML には論理的な情報を書き、
XSL によって視覚的な情報を持たせます。

具体的な例をあげると、
更新履歴の XML からテーブルを生成するには以下のような XSL を書きます。


<pre class="xsource" title="whatsnew.xml をテーブル化する XSL">
<code><span class="bracket">&lt;?</span><span class="element">xml</span> <span class="attribute">version</span><span class="attvalue">="1.0"</span> <span class="attribute">encoding</span><span class="attvalue">="Shift_JIS"</span><span class="bracket">?&gt;</span>
<span class="bracket">&lt;</span><span class="element">xsl:stylesheet</span> <span class="attribute">xmlns:xsl</span><span class="attvalue">="http://www.w3.org/1999/XSL/Transform"</span> <span class="attribute">version</span><span class="attvalue">="2.0"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">xsl:output</span> <span class="attribute">method</span><span class="attvalue">="html"</span> <span class="attribute">encoding</span><span class="attvalue">="Shift_JIS"</span><span class="bracket">/&gt;</span>

  <span class="bracket">&lt;</span><span class="element">xsl:template</span> <span class="attribute">match</span><span class="attvalue">="/"</span><span class="bracket">&gt;</span>

    <span class="comment">&lt;!-- 中略 --&gt;</span>

    <span class="bracket">&lt;</span><span class="element">table</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">xsl:for-each</span> <span class="attribute">select</span><span class="attvalue">="whatsnew/new"</span>
        <span class="attribute">order-by</span><span class="attvalue">="-number(@year);-number(@month);-number(@day)"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@year"</span> <span class="bracket">/&gt;</span>年
          <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@month"</span> <span class="bracket">/&gt;</span>月
          <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@day"</span> <span class="bracket">/&gt;</span>日
        <span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">xsl:choose</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">xsl:when</span> <span class="attribute">test</span><span class="attvalue">=".[@url!='']"</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">target</span><span class="attvalue">="_top"</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">xsl:attribute</span> <span class="attribute">name</span><span class="attvalue">="href"</span><span class="bracket">&gt;</span>
                <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@url"</span> <span class="bracket">/&gt;</span>
                <span class="bracket">&lt;/</span><span class="element">xsl:attribute</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="."</span> <span class="bracket">/&gt;</span>
              <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;/</span><span class="element">xsl:when</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">xsl:otherwise</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="."</span> <span class="bracket">/&gt;</span>
            <span class="bracket">&lt;/</span><span class="element">xsl:otherwise</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">xsl:choose</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">xsl:for-each</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>

    <span class="comment">&lt;!-- 中略 --&gt;</span>

  <span class="bracket">&lt;/</span><span class="element">xsl:template</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">xsl:stylesheet</span><span class="bracket">&gt;</span>
</code></pre>
こうすることで、ページのデザインを変えたくなったときにはこのXSLを修正するだけですむようになるわけです。
例えば、先ほどのようにテーブルを使うのをやめたくなったときには、
XSL の table の部分を以下のように修正するだけでデザインの変更が出来ます。


<pre class="xsource" title="whatsnew.xml を p タグに展開">
<code><span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">xsl:for-each</span> <span class="attribute">select</span><span class="attvalue">="whatsnew/new"</span>
    <span class="attribute">order-by</span><span class="attvalue">="-number(@year);-number(@month);-number(@day)"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@year"</span> <span class="bracket">/&gt;</span>年
      <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@month"</span> <span class="bracket">/&gt;</span>月
      <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@day"</span> <span class="bracket">/&gt;</span>日
  <span class="bracket">&lt;</span><span class="element">xsl:choose</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">xsl:when</span> <span class="attribute">test</span><span class="attvalue">=".[@url!='']"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">target</span><span class="attvalue">="_top"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">xsl:attribute</span> <span class="attribute">name</span><span class="attvalue">="href"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="@url"</span> <span class="bracket">/&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">xsl:attribute</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="."</span> <span class="bracket">/&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">xsl:when</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">xsl:otherwise</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">xsl:value-of</span> <span class="attribute">select</span><span class="attvalue">="."</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">xsl:otherwise</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">xsl:choose</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">br</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">xsl:for-each</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
</code></pre>
ちなみにこの XSL では、ついでだから更新日時でソートして、URLが指定されているときに限って&lt;a&gt;タグをつけるように条件分岐するようにしています。
