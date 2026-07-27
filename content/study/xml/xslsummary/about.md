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
  - "/study/testxsl/about.html"
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


```xml
<?xml version="1.0" encoding="utf-8"?>

<document title="概要" xmlns="http://ufcpp.net/study/document">
  <section title="XML の利用" id="xml">
    <p>
      このサイトではそこら中でXMLを使っています。
      勉強ページは全域、XML で書いて XSLT をかけてからアップロードしています。
    </p>
  </section>
</document>
```
以下のような HTML に変換しています。


```html
<html lang="ja-JP" xmlns:ufcpp="http://ufcpp.net/study/document">
  <head>
  <head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Language" content="ja-JP" />
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta http-equiv="Content-Script-Type" content="text/javascript" />
    <meta name="Author" content="IWANAGA Nobuyuki"/>
    <link rel="stylesheet" href="../main.css"/>
    <link rel="stylesheet" href="../document.css"/>
    <link rel="stylesheet" href="../mathstyle.css"/>
    <link rel="stylesheet" href="../figure.css"/>
    <link rel="stylesheet" href="../source.css"/>
    <link rel="stylesheet" href="../link.css"/>
    <link rel="stylesheet" href="../qanda.css"/>
    <meta name="keywords" content=""/>
    <title>
      概要(このページの XSL)
    </title>
  </head>
  <body class="Menu">
    <div class="Main">
      <div class="CommonHeader">
        <p class="head">
          <a href="../../index.html">
            <img src="../common/sitelogo.jpg" width="450" height="65"
                 alt="++C++; // 未確認飛行 C" />
          </a>
        </p>
      </div>

      <div class="Header">
        <h1 id="pagetitle">概要</h1>
        <h4>目次</h4>
        <ul>
          <li>
            <a href="#xml">XML の利用</a>
          </li>
        </ul>
        <h4>キーワード</h4>
        <ul></ul>
      </div>

      <div class="Middle">
        <div class="Body">
          <h2>
            <a id="xml">XML の利用</a>
          </h2>
          <p>
            このサイトではそこら中でXMLを使っています。
            勉強ページは全域、XML で書いて XSLT をかけてからアップロードしています。
          </p>
        </div>
      </div>

      <div class="Footer">
        <p></p>
        <p>
          <a href="#pagetitle">このページの先頭に戻る</a>
        </p>
        <p>
          <a href="index.html" accesskey="i">
            インデックスページに戻る(<span class="accesskey">i</span>)
          </a>
        </p>
        <p>
          <a href="document.html" accesskey="n">
            ＞＞ 次(<span class="accesskey">n</span>) 「ドキュメント」
          </a>
        </p>
      </div>
    </div>

    <div class="MenuList">
      <div class="GeneralIndex">
        <ul>
          <li>
            ≫ <a href="../../index.html">Top</a>
          </li>
          <li>
            ≫ <a href="../index.html">総合インデックス</a>
          </li>
          <li>
            ≫ <a href="index.html">このページの XSL</a>
          </li>
        </ul>
      </div>
      <div class="CommonMenu"></div>
      <div class="MenuIndex">
        <ul class="documentIndex">
          <li class="indexDoc">
            <a href="../testxsl/about.html">概要</a>
          </li>
          <li class="indexSection">
            <h2 class="indexSection">
              <a id="summary">スタイルシートの説明</a>
            </h2>
            <ul class="index">
              <li class="indexDoc">
                <a href="../testxsl/document.html">ドキュメント</a>
              </li>
            </ul>
          </li>
        </ul>
      </div>
    </div>
  </body>
</html>
```
