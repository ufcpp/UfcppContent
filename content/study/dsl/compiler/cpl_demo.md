---
title: "デモアプリ"
source_url: "https://ufcpp.net/study/dsl/compiler/cpl_demo/"
content_type: "Article"
published_at: "2009-05-10T00:00:00"
updated_at: "2015-05-06T14:15:43"
tags: []
umbraco_id: 1436
parent_id: 1434
sort_order: 1
aliases:
  - "/dsl/compiler/cpl_demo/"
  - "/dsl/cpl_demo"
  - "/dsl/cpl_demo.html"
  - "/study/dsl/cpl_demo"
  - "/study/dsl/cpl_demo.html"
---

# デモアプリ

##<a id="sec-generated-title-1"></a> <a id="demo"></a>デモ
<div class="silverlightControlHost" style="margin:1em;height:600;"><object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="800" height="600"><param name="source" value="/media/ufcpp2000/dsl/ClientBin/StackMachine.xap"></param><param name="onerror" value="onSilverlightError"></param><param name="background" value="white"></param><param name="minRuntimeVersion" value="2.0"></param><param name="autoUpgrade" value="false"></param><a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;"><img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Microsoft Silverlight プラグインを入れてね" style="border-style: none"></img></a></object><iframe style="visibility:hidden;height:0;width:0;border:0px"></iframe></div>

##### <a id="sec-generated-title-2"></a>このデモでやっていること
* デモ用に簡単な言語を作りました。（仕様は後述）

* Silverlight 上でソースコードの編集ができます。 （デモ中の左上の部分。）

* コンパイルの中間生成物である抽象構文木を視覚化。 （デモ中の左下の部分。）

* コンパイル結果の仮想マシン語コードを表示。 （デモ中、真ん中よりちょっと右の部分。）

* 動作の様子。 スタックの内部状態とかを表示。 （デモ中の右側の部分。）



##### <a id="sec-generated-title-3"></a>独自言語の仕様
* 扱えるのは整数のみ。型なんて概念ないよ。

* 加減乗除、比較、条件演算子くらいは使える。

* 関数を定義できる（再帰呼び出しも可）。

* 詳しい文法：
[stack.mg](../../../../assets/media/ufcpp2000/dsl/source/stack.mg)
（MGrammer 形式）



##<a id="sec-generated-title-4"></a> <a id="source"></a>付属品
* 独自言語
    * 文法ファイル：
[stack.mg](../../../../assets/media/ufcpp2000/dsl/source/stack.mg)


    * サンプルソース：
[sample.dsl](../../../../assets/media/ufcpp2000/dsl/source/sample.dsl)




* ソース一式：
[StackMachine.zip](../../../../assets/media/ufcpp2000/dsl/source/StackMachine.zip)




##<a id="sec-generated-title-5"></a> <a id="plan"></a>予定
<pre>
目的
• 関数呼び出し（特に再帰）を理解する
• MSILとかJavaバイトコードとかの理解深める


書くこと
• スタックマシン
	○ スタックのイメージ
	○ スタックマシンの場合、スタックの一番上に乗ってる数個の値をオペランドにする
• コンパイラ
	○ 字句解析 → 構文解析 → 意味解析
	○ 今回は構文解析と意味解析は同時にやっちゃってる
	○ MGrammerは構文解析だけやる
• 字句解析
	○ 正規表現でやる
• 構文解析
	○ まず式から
	○ 式木
	○ 前置き記法、中置き記法、後置き記法
	○ 意味解析も一緒にやっちゃってる
		§ 分ける・わけないの差は：
			□ 「xは変数でfは関数」とかの情報をパース途中で使えるか否か
• BNF
	○ MGrammer
	○ BNFを元にC#化

注意
• 構文解析と意味解析は同時にやっちゃってる
• コード最適化とかはやらない
• レジスタマシンへの変換とかもここでは説明しない

Further Reading
• レジスタマシン
	○ 仮想マシンはスタック型なことが多いけど、実際のCPUはレジスタ型がほとんど
		§ Java VMもMSILもスタックマシン
	○ レジスタ型は実装依存しやすい。VMの場合はスタック型で設計しておいて、JIT時にレジスタコードに。
• いろんな意味にとれる文
	○ G(F&lt;x, y&gt;(z + w))
		§ ジェネリック関数Fとも読めるし、比較演算結果2個を引数に取る関数ともとれるし
		§ 解決策
			□ 意味解析しながら
			□ 優先順位を付ける
• x(i)は、xが変数ならインデクサ、xが関数なら関数呼び出し
	○ この場合、構文解析だけ先にやるなら、(i)を後置き演算子とでもしておけば、できないこともない

参考
• コンパイラ入門
	○ http://www.atmarkit.co.jp/fjava/rensai4/compiler01/compiler01_02.html
• 文法


</pre>
