---
title: "C# サンプルプログラム"
source_url: "https://ufcpp.net/study/csharp/sample/ap_sample/"
content_type: "Article"
published_at: "2015-05-06T14:13:09"
updated_at: "2015-05-06T14:13:09"
tags: []
umbraco_id: 1360
parent_id: 1359
sort_order: 0
aliases:
  - "/csharp/ap_sample"
  - "/csharp/ap_sample.html"
  - "/csharp/sample/ap_sample/"
  - "/study/csharp/ap_sample"
  - "/study/csharp/ap_sample.html"
---

# C# サンプルプログラム

## <a id="sec-generated-title-1"></a> <a id="licence"></a>ライセンスに関して

置いてあるサンプルは [MIT/X ライセンス](http://www.opensource.gr.jp/licenses/mit-license.html)に準拠ということでお願いします。


## <a id="sec-generated-title-2"></a> <a id="other"></a>他のページのサンプル

サイト内の他のページにもいくつか C# サンプルプログラムがあります。

* 「[クラスライブラリ](../../dotnet/index.md)」→「[サンプルプログラム](../../dotnet/appendix/sample.md)」

* 「[アルゴリズムとデータ構造](../../algorithm/index.md)」

* 「[信号処理](../../sp/index.md)」



## <a id="sec-generated-title-3"></a> <a id="xslt"></a>XSL 変換一斉適用

フォルダ中にある XML ファイルに一斉に XSL 変換をかけます。

[ソースファイル(zip形式書庫)](../../../../assets/sample/ApplyXsl.zip)


## <a id="sec-generated-title-4"></a> <a id="reversi"></a>オセロ

名前の通り、オセロです。
今のところローカルコンピュータ上での人対人のみで、ネットワーク対戦やコンピュータ戦は出来ません。

[ソースファイル(zip形式書庫)](../../../../assets/sample/reversi.zip)


## <a id="sec-generated-title-5"></a> <a id="complex"></a>複素数クラス

複素数をクラス化してみました。
実用品ではなくて、実装の隠蔽・抽象基底クラスからの継承のサンプルとして作りました。
以下のような2つの方法で実装しています。

* 実部・虚部をメンバーとして持つ複素数クラス<code>CartesianComplex</code>

* 絶対値・偏角をメンバーとして持つ複素数クラス<code>PolarComplex</code>


また、これらのクラスを <code>Complex</code> という抽象基底クラスから派生させています。

[ソースファイル(zip形式書庫)](../../../../assets/sample/Complex.zip)


## <a id="sec-generated-title-6"></a> <a id="lineart"></a>ラインアート

僕はGUI開発環境の提供されているプログラミング言語を新しく覚えるたびに
ラインアートを作っています。
ほとんど同じプログラムを作ることでその言語の善し悪しを見比べているのですが、
今まで作った中でC#はもっとも作成が容易でした。

作るの楽だし、ちょっと凝ったものを作ってみようということで、
右クリックメニューで設定画面開けるようにしたり、
設定(線の本数、頂点の数、画面サイズ等)を XML で保存して、
次回起動時に設定を読み出すようにしてみました。

[ソースファイル(zip形式書庫)](../../../../assets/sample/LineArt.zip)


## <a id="sec-generated-title-7"></a> <a id="bitfield"></a>ビットフィールド

研究室の課題で Verilog HDL でソース書いてたんですが、
途中で、エラーチェック甘いし動作の重たいシミュレーションツールにぶち切れて、
ソフトウェアでアルゴリズムのチェックをしてから HDL 記述を書くことにしました。
その際に作ったのが、Verilog の変数みたいなビット操作を行うことの出来るビットフィールドクラスです。

このビットフィールドクラスの例を以下にあげます。

<table summary="">

	<tr>
		<th>Verilog風記述</th>
		<th><code>BitField</code>クラス</th>
	</tr>
	<tr>
		<td markdown="1"><code>wire [31:0]w;</code></td>
		<td markdown="1"><code>BitField w = BitField.Create(31, 0);</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>assign w = x[4:0];</code></td>
		<td markdown="1"><code>w.Assign(x[4, 0]);</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>assign w = {x, y, z};</code></td>
		<td markdown="1"><code>w.Assign(BitField.Concat(x, y, z));</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>assign w = {w[0], w[31:1]};</code></td>
		<td markdown="1"><code>w.Assign(BitField.Concat(w[0], w[31, 1]));</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>assign w[0] = x[0] &amp; y[0];</code></td>
		<td markdown="1"><code>w[0] = x[0] &amp; y[0];</code></td>
	</tr>
	<tr>
		<td markdown="1"><code>assign w[4:0] = x[4:0] &amp; y[4:0];</code></td>
		<td markdown="1"><code>w[4, 0].Assign(x[4, 0] &amp; y[4, 0])</code></td>
	</tr>
</table>


[ソースファイル(zip形式書庫)](../../../../assets/sample/BitField.zip)


## <a id="sec-generated-title-8"></a> <a id="wcf_demo"></a>WCF デモ

* [ソース一式（zip圧縮）](../../../../assets/source/WcfGameSample.zip)

* [PowerPoint（OpenXML）](../../../../assets/slide/WcfDemo.pptx)

* [XPS](../../../../assets/slide/WcfDemo.xps)
