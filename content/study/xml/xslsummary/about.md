---
title: "概要"
source_url: "https://ufcpp.net/study/xml/xslsummary/about/"
content_type: "Article"
published_at: "2015-05-06T14:23:56"
updated_at: "2015-05-06T14:23:56"
tags: []
umbraco_id: 1646
parent_id: 1645
sort_order: 0
aliases:
  - "/study/testxsl/about"
  - "/study/testxsl/about.html"
  - "/testxsl/about"
  - "/testxsl/about.html"
  - "/xml/xslsummary/about/"
---

# 概要

## <a id="sec-generated-title-1"></a> <a id="term"></a>用語

<table summary="">

	<tr>
		<th>XML</th>
		<td markdown="1">
Extensible Markup Language。

HTML と似たような構文（&lt;&gt; を使ったマークアップ）で階層的な構造を記述するための言語。

「人も読めるけども、プログラムからも読みやすく」というのが目標なので、
「タグは必ず閉じなければならない」など、HTML と比べると少し厳格なルールがあります。

XML の定めるところは、あくまで「&lt;&gt; を使ったマークアップタグを書く」というような部分で、
「どういうタグを定義できるか」は利用者が決めることになります。
（ちなみに、「どういうタグを定義できるか」の部分を定めるための言語は XSD。）
</td>
	</tr>
	<tr>
		<th>XSL</th>
		<td markdown="1">
Extensible Stylesheet Language。

XML を画面表示や印刷に適した形式に変換するための、スタイルシート定義用言語。
XSL 自体も XML 形式で記述します。
</td>
	</tr>
	<tr>
		<th>XSLT</th>
		<td markdown="1">
Extensible Stylesheet Language Transformations。

XSL のうち、
XML → XML の変換ルールの部分だけを取り出して規格化したもの。
XSL の他の部分（印刷用にはどういうタグを使うかなど）の標準化は難航しているようで、
現状、XSL というとこれのこと。

独自の XML に対して、
XSLT をかけて HTML 化してブラウザで表示したりといった用途に使います。
</td>
	</tr>
	<tr>
		<th>XSD</th>
		<td markdown="1">
XML Schema Definition。

例えば HTML なら「body タグの直下にはブロック要素が書ける」「ブロック要素とは div, p, table などで・・・」というような、
タグの構造の決まりがあります。
XSD というのは、このようなタグ構造を定義するための言語です。
これも XML 形式で記述します。

多くの XML 編集ソフト（例えば Visual Studio も XML の編集機能があります）では、
XSD があれば XML を書いている途中にタグの補完機能が働くようになります。
</td>
	</tr>
</table>



## <a id="sec-generated-title-2"></a> <a id="xml"></a>XML の利用

このサイトではそこら中でXMLを使っています。 勉強ページは全域、XML で書いて XSLT をかけてからアップロードしています。

XML を使う利点は以下のような感じ。

* HTML よりも論理マークアップしやすい

* サイト全体を通して、ページの見た目を一貫性あるものに保つことができる
    * デザインを変えたくなったとき、XSL を修正するだけで勉強ページ全域を一括変更できる



* 冗長な記述が必要ない
    * 各ページの目次や索引を自動生成できる




C# などのプログラミング言語で変換プログラムを書いているわけではなく、 XSL しか使っていません。
なぜかというと、

* XSL だけでも十分な表現力がある。
    * 条件分岐、反復、再帰呼び出しと、ちょっとしたプログラミング言語並の機能がある。

    * 他の XML ドキュメントの中身の参照もできる。



* ウェブブラウザ（IE でも Opera、Firefox でも）直接表示できる。
    * いちいち変換プログラムを起動しなくても、XML ファイルのダブルクリックだけで表示できる。

    * 更新も [F5] キーを押すだけ。

    * IIS や Apache などのサーバを立てる必要もない。




難点は、独自に定義した XML タグを覚えていないと使えないことですが、 
XSD（XML Schema Definition）を書けば XML エディタ（XML notepad や Visual Studio などの XML 編集機能）の補完機能が効くようになるので、 
XSD も書くことでタグを覚える面倒さは軽減されます。


## <a id="sec-generated-title-3"></a> <a id="example"></a>具体例

具体的に例をあげると、 以下のような XML を書いて、


<pre class="xsource" title="勉強用ページ内の XML の例">
<code><span class="bracket">&lt;?</span><span class="element">xml</span> <span class="attribute">version</span><span class="attvalue">="1.0"</span> <span class="attribute">encoding</span><span class="attvalue">="utf-8"</span><span class="bracket">?&gt;</span>

<span class="bracket">&lt;</span><span class="element">document</span> <span class="attribute">title</span><span class="attvalue">="概要"</span> <span class="attribute">xmlns</span><span class="attvalue">="http://ufcpp.net/study/document"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="XML の利用"</span> <span class="attribute">id</span><span class="attvalue">="xml"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      このサイトではそこら中でXMLを使っています。
      勉強ページは全域、XML で書いて XSLT をかけてからアップロードしています。
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">document</span><span class="bracket">&gt;</span>
</code></pre>
以下のような HTML に変換しています。


<pre class="xsource" title="XML の変換結果">
<code><span class="bracket">&lt;</span><span class="element">html</span> <span class="attribute">lang</span><span class="attvalue">="ja-JP"</span> <span class="attribute">xmlns:ufcpp</span><span class="attvalue">="http://ufcpp.net/study/document"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">http-equiv</span><span class="attvalue">="Content-Type"</span> <span class="attribute">content</span><span class="attvalue">="text/html; charset=utf-8"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">http-equiv</span><span class="attvalue">="Content-Language"</span> <span class="attribute">content</span><span class="attvalue">="ja-JP"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">http-equiv</span><span class="attvalue">="Content-Style-Type"</span> <span class="attribute">content</span><span class="attvalue">="text/css"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">http-equiv</span><span class="attvalue">="Content-Script-Type"</span> <span class="attribute">content</span><span class="attvalue">="text/javascript"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">name</span><span class="attvalue">="Author"</span> <span class="attribute">content</span><span class="attvalue">="IWANAGA Nobuyuki"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../main.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../document.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../mathstyle.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../figure.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../source.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../link.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">href</span><span class="attvalue">="../qanda.css"</span><span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">meta</span> <span class="attribute">name</span><span class="attvalue">="keywords"</span> <span class="attribute">content</span>=""<span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>
      概要(このページの XSL)
    <span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">body</span> <span class="attribute">class</span><span class="attvalue">="Menu"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="Main"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="CommonHeader"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">p</span> <span class="attribute">class</span><span class="attvalue">="head"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../../index.html"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">img</span> <span class="attribute">src</span><span class="attvalue">="../common/sitelogo.jpg"</span> <span class="attribute">width</span><span class="attvalue">="450"</span> <span class="attribute">height</span><span class="attvalue">="65"</span>
                 <span class="attribute">alt</span><span class="attvalue">="++C++; // 未確認飛行 C"</span> <span class="bracket">/&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="Header"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">h1</span> <span class="attribute">id</span><span class="attvalue">="pagetitle"</span><span class="bracket">&gt;</span>概要<span class="bracket">&lt;/</span><span class="element">h1</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">h4</span><span class="bracket">&gt;</span>目次<span class="bracket">&lt;/</span><span class="element">h4</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ul</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="#xml"</span><span class="bracket">&gt;</span>XML の利用<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">h4</span><span class="bracket">&gt;</span>キーワード<span class="bracket">&lt;/</span><span class="element">h4</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ul</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="Middle"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="Body"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">h2</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">id</span><span class="attvalue">="xml"</span><span class="bracket">&gt;</span>XML の利用<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">h2</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
            このサイトではそこら中でXMLを使っています。
            勉強ページは全域、XML で書いて XSLT をかけてからアップロードしています。
          <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="Footer"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="#pagetitle"</span><span class="bracket">&gt;</span>このページの先頭に戻る<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="index.html"</span> <span class="attribute">accesskey</span><span class="attvalue">="i"</span><span class="bracket">&gt;</span>
            インデックスページに戻る(<span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="accesskey"</span><span class="bracket">&gt;</span>i<span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>)
          <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="document.html"</span> <span class="attribute">accesskey</span><span class="attvalue">="n"</span><span class="bracket">&gt;</span>
            ＞＞ 次(<span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="accesskey"</span><span class="bracket">&gt;</span>n<span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>) 「ドキュメント」
          <span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

    <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="MenuList"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="GeneralIndex"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ul</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
            ≫ <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../../index.html"</span><span class="bracket">&gt;</span>Top<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
            ≫ <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../index.html"</span><span class="bracket">&gt;</span>総合インデックス<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span><span class="bracket">&gt;</span>
            ≫ <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="index.html"</span><span class="bracket">&gt;</span>このページの XSL<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="CommonMenu"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="MenuIndex"</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">ul</span> <span class="attribute">class</span><span class="attvalue">="documentIndex"</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span> <span class="attribute">class</span><span class="attvalue">="indexDoc"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../testxsl/about.html"</span><span class="bracket">&gt;</span>概要<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;</span><span class="element">li</span> <span class="attribute">class</span><span class="attvalue">="indexSection"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">h2</span> <span class="attribute">class</span><span class="attvalue">="indexSection"</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">id</span><span class="attvalue">="summary"</span><span class="bracket">&gt;</span>スタイルシートの説明<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;/</span><span class="element">h2</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">ul</span> <span class="attribute">class</span><span class="attvalue">="index"</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;</span><span class="element">li</span> <span class="attribute">class</span><span class="attvalue">="indexDoc"</span><span class="bracket">&gt;</span>
                <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../testxsl/document.html"</span><span class="bracket">&gt;</span>ドキュメント<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
              <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
          <span class="bracket">&lt;/</span><span class="element">li</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">ul</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>
