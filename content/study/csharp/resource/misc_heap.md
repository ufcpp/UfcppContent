---
title: "[雑記] スタックとヒープ"
source_url: "https://ufcpp.net/study/csharp/resource/misc_heap/"
content_type: "Article"
published_at: "2009-04-26T00:00:00"
updated_at: "2017-11-18T14:50:26"
tags: []
umbraco_id: 1291
parent_id: 1286
sort_order: 8
aliases:
  - "/csharp/misc_heap"
  - "/csharp/misc_heap.html"
  - "/csharp/resource/misc_heap/"
  - "/study/csharp/misc_heap"
  - "/study/csharp/misc_heap.html"
---

# \[雑記\] スタックとヒープ

##<a id="sec-generated-title-1"></a> <a id="point"></a>ポイント
* メモリにはスタックとヒープの2種類の使い方がある



##<a id="sec-generated-title-2"></a> <a id="abst"></a>概要
（書きかけ）

「スタックとは」「ヒープとは」の説明を入れる予定。
とりあえず、デモ用のアプリだけ先に公開。

「[値型と参照型](oo_reference.md)」の理解の手助け用。


##<a id="sec-generated-title-3"></a> <a id="emulate"></a>模擬的に視覚化
スタックとヒープの挙動を模擬的にデモするような Silverlight アプリを作ってみました。
<div class="silverlightControlHost" style="margin:1em;height:480;"><object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="752" height="480"><param name="source" value="/media/ufcpp2000/csharp/ClientBin/MemoryImage.xap"></param><param name="onerror" value="onSilverlightError"></param><param name="background" value="white"></param><param name="minRuntimeVersion" value="2.0"></param><param name="autoUpgrade" value="false"></param><a href="http://go.microsoft.com/fwlink/?LinkID=124807" style="text-decoration: none;"><img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Microsoft Silverlight プラグインを入れてね" style="border-style: none"></img></a></object><iframe style="visibility:hidden;height:0;width:0;border:0px"></iframe></div>
以下、使い方の説明。


##### <a id="sec-generated-title-4"></a>疑似コード（左半分）
疑似コードで記述されたプログラム。
コードの先頭から選択した行までの実行結果が右側に表示されます。

疑似コードの書き方は、
変数の宣言とインスタンスの生成、参照関係だけを記述する独自言語。

<table summary="">

	<tr>
		<th>疑似コード</th>
		<th>説明</th>
		<th>対応する C# の例</th>
	</tr>
	<tr>
		<td markdown="1"><code>x = 100</code></td>
		<td markdown="1">新しい変数 x を用意して、値 100 を格納。</td>
		<td markdown="1"><code>
<span class="reserved">int</span> x = 100;
</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>a = new[8]</code></td>
		<td markdown="1">サイズ 8 バイトのインスタンスを生成して変数 a に格納。</td>
		<td markdown="1"><code>Point a = <span class="reserved">new</span> Point();</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>c = new[8] { a b }</code></td>
		<td markdown="1">インスタンスを生成して c に格納。<br></br>このインスタンスは別のインスタンス a と b から参照されてる。</td>
		<td markdown="1"><code>
          Point c = <span class="reserved">new</span>Point();<br></br>
          Line a = <span class="reserved">new</span>Line();<br></br>
          a.Origin = c;<br></br>
          Circle b = <span class="reserved">new</span>Circle();<br></br>
          b.Center = c;
      </code></td>
	</tr>
	<tr>
		<td markdown="1"><code>
          {<br></br>
          　　a = new[8]<br></br>
          }
        </code></td>
		<td markdown="1">スコープ内で新しい変数 a を作成。<br></br>スコープを抜けると変数 a の持つ参照は無効になる。</td>
		<td markdown="1"><code>
          {<br></br>
          　　Point a = <span class="reserved">new</span> Point();<br></br>
          }
        </code></td>
	</tr>
</table>


左下の編集ボタンを押せば、疑似コードの編集ができます。
エラーチェックとかまるでしてなくて、文法違反なコードを書いたらその部分まるごと無視します。


##### <a id="sec-generated-title-5"></a>スタック（真ん中辺りの青いところ）
スタックを模したもの。
変数の中身はここに格納されます。

ちなみに、このアプリ上ではスタックの深さは20。


##### <a id="sec-generated-title-6"></a>ヒープ（右側の黄色いところ）
ヒープを模したもの。
new したインスタンスはここに作成されます。

スタックからたどれるインスタンスはカラーで、
誰からも参照されなくなったインスタンスはグレーで表示。

Java や C# などのように、ガベージコレクションを持つ言語では、
誰からの参照されなくなったインスタンスもしばらくヒープ上に残ります。
ヒープの空き領域がなくなった時点でゴミ掃除が行われて、
誰も参照していない場所が解放されます。

このアプリ上ではヒープのサイズは400。


##### <a id="sec-generated-title-7"></a>関連物ダウンロード
* 
[ソース一式（ZIP 形式圧縮）](../../../../assets/media/ufcpp2000/csharp/source/MemoryImage.zip)


* 
[疑似コードの文法（MGrammer 形式）](../../../../assets/media/ufcpp2000/csharp/source/MemoryImage.mg)



ちなみに、
疑似コードの構文解析は M を使ってるわけではなくて、
Silverlight 中では正規表現を使って実装しています。
MGrammer はお試しで書いたもの。
