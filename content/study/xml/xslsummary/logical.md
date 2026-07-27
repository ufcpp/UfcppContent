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
  - "/study/testxsl/logical.html"
---

# 論理マークアップとデザインの変更

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

HTML を直接書くのではなく、
一度 XML で書いてから XSL で変換をかける利点の1つは、
論理マークアップと視覚デザインを分離できることです。

論理と視覚が分離されているので、視覚デザインの変更も容易です。


## <a id="sec-generated-title-2"></a> <a id="separation"></a>論理と視覚の分離

例として更新履歴を見てみましょう。

勉強ページの更新履歴の元データは以下のような XML になっています。


```xml
<whatsnew>
<new year="2000" month="12" day="24"
  url="../csharp/index.html">
  C#の解説ページ作りました
</new>
<new year="2000" month="12" day="23" url="">
  ホームページ開設
</new>
</whatsnew>
```
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


```html
<table>
<tr>
 <td>2000年12月24日</td>
 <td>
 <a href="../programming/csharp/index.html">
   C#の解説ページ作りました
 </a>
 </td>
</tr>
<tr>
 <td>2000年12月23日</td>
 <td>ホームページ開設</td>
</tr>
</table>
```
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


```xml
<p>
2000年12月24日 <a target="_top"
  href="../csharp/index.html">
  C#の解説ページ作りました
</a><br>
2000年12月23日 ホームページ開設<br>
</p>
```
こんな感じ↑に変更したくなったときに、いちいち更新履歴全体を書き換えるのはかなり面倒なわけで、
こういう視覚デザイン的な情報は更新履歴という論理的な情報とは分離しておいた方が変更作業が楽になるわけです。


## <a id="sec-generated-title-3"></a> <a id="xsl"></a>XSL による論理と視覚の分離

XSL というのは、まさに「論理と視覚の分離」のためにあります。
XML には論理的な情報を書き、
XSL によって視覚的な情報を持たせます。

具体的な例をあげると、
更新履歴の XML からテーブルを生成するには以下のような XSL を書きます。


```xml
<?xml version="1.0" encoding="Shift_JIS"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
  <xsl:output method="html" encoding="Shift_JIS"/>

  <xsl:template match="/">

    <!-- 中略 -->

    <table>
      <xsl:for-each select="whatsnew/new"
        order-by="-number(@year);-number(@month);-number(@day)">
      <tr>
        <td>
          <xsl:value-of select="@year" />年
          <xsl:value-of select="@month" />月
          <xsl:value-of select="@day" />日
        </td>
        <td>
          <xsl:choose>
            <xsl:when test=".[@url!='']">
              <a target="_top">
              <xsl:attribute name="href">
                <xsl:value-of select="@url" />
                </xsl:attribute>
              <xsl:value-of select="." />
              </a>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="." />
            </xsl:otherwise>
          </xsl:choose>
        </td>
      </tr>
      </xsl:for-each>
    </table>

    <!-- 中略 -->

  </xsl:template>
</xsl:stylesheet>
```
こうすることで、ページのデザインを変えたくなったときにはこのXSLを修正するだけですむようになるわけです。
例えば、先ほどのようにテーブルを使うのをやめたくなったときには、
XSL の table の部分を以下のように修正するだけでデザインの変更が出来ます。


```xml
<p>
  <xsl:for-each select="whatsnew/new"
    order-by="-number(@year);-number(@month);-number(@day)">
      <xsl:value-of select="@year" />年
      <xsl:value-of select="@month" />月
      <xsl:value-of select="@day" />日
  <xsl:choose>
    <xsl:when test=".[@url!='']">
      <a target="_top">
      <xsl:attribute name="href">
        <xsl:value-of select="@url" />
      </xsl:attribute>
      <xsl:value-of select="." />
      </a>
    </xsl:when>
    <xsl:otherwise>
      <xsl:value-of select="." />
    </xsl:otherwise>
  </xsl:choose>
  <br />
  </xsl:for-each>
</p>
```
ちなみにこの XSL では、ついでだから更新日時でソートして、URLが指定されているときに限って&lt;a&gt;タグをつけるように条件分岐するようにしています。
