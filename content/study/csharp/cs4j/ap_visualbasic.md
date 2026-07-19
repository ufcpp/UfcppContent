---
title: "C# と Visual Basic"
source_url: "https://ufcpp.net/study/csharp/cs4j/ap_visualbasic/"
content_type: "Article"
published_at: "2013-12-08T00:00:00"
updated_at: "2015-05-06T14:13:39"
tags: []
umbraco_id: 1376
parent_id: 1372
sort_order: 3
aliases:
  - "/csharp/ap_visualbasic"
  - "/csharp/ap_visualbasic.html"
  - "/csharp/cs4j/ap_visualbasic/"
  - "/study/csharp/ap_visualbasic"
  - "/study/csharp/ap_visualbasic.html"
---

# C# と Visual Basic

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

(書きかけ)

Visual Basic 7 以降、つまり、.NET が出てからの VB は、持っている機能的には C# とあまり変わりありません。
特に、C# 3.0 / VB 9 以降はコンパイラー開発チームも統合され、機能差が縮まっています。
現在では、その差は単なる構文（syntax）的なもの（ブロックに {} を使うか then/end などを使うか程度の差）であって、意味論（semantics）的にはかなり近くなっています。

なので、割かし簡単に C# ⇔ VB 相互変換ができたりします。
実際、そういう機能を提供するサイトがあったり。

* [Convert C# to VB.NET](http://www.developerfusion.com/tools/convert/csharp-to-vb/)


とはいえ、

* 差がこの程度しかないということを示す

* 似て非なるところがかえって厄介なので気を付ける


という意味で、C# と VB の現状の差を紹介しましょう。


## <a id="sec-generated-title-2"></a> <a id="unsafe"></a>unsafe コード

C# にしかない機能の筆頭は unsafe


## <a id="sec-generated-title-3"></a> <a id="xml-literal"></a>XML リテラル

VB にしかない機能筆頭。

あんまり XML が使われなくなってきてる現状としては…


## <a id="sec-generated-title-4"></a> <a id="iterator"></a>イテレーター

VB は 10 までなかった。

一方で、VB 10 では、匿名関数もイテレーター化できるように（C# は無理）。


## <a id="sec-generated-title-5"></a> <a id="collection-inilializer"></a>コレクション初期化子

Add メソッドがあればコレクション初期化子を使えるわけだけども。

VB の場合は拡張メソッドでも OK（C# は通常のメソッドとして持たなきゃダメ）。


## <a id="sec-generated-title-6"></a> <a id="dynamic"></a>dynamic

dynamic 関連の仕様が結構違う。
単なる late-binding か、Dynamic Callsite か。


## <a id="sec-generated-title-7"></a> <a id="overload-resolution"></a>オーバーロードの解決ルール

メソッドの似たようなオーバーロードがあるとき、どれが呼ばれるかを決めるルールは結構複雑。
C# と VB で意外と違うので、
パッと見で同じになるように単純に翻訳してしまうと挙動が変わる。

C# ⇔ VB の自動変換をやろうとすると一番はまる部分。


## <a id="sec-generated-title-8"></a> <a id="misc"></a>他

With とか Handles とか On Error とか。
要するに、旧 VB の名残。

引数付きプロパティ。
C# では「foreach に使えないのが不便なだけ。コレクション型のプロパティにすべき」として不採用。
一応、C# でも、対 COM 用（RCW の呼び出し）限定で引数付きプロパティを呼ぶ機能はある（VB で作ったやつは呼べないし、C# で作ることはできない）。

大文字小文字の区別。
VB だとクラスと同名のメソッド作れる（C# はコンストラクターとかぶるので作れない）。
ちなみに、キーワードの差に関しては @ つければ解消するので問題なし。
^ 演算子はどのみち Math.Pow とかへの翻訳。
