---
title: "特殊記号の入力"
source_url: "https://ufcpp.net/study/misc/list/symbol/"
content_type: "Article"
published_at: "2015-05-06T14:19:05"
updated_at: "2015-05-06T14:19:05"
tags: []
umbraco_id: 1536
parent_id: 1534
sort_order: 1
aliases:
  - "/misc/list/symbol/"
  - "/misc/symbol"
  - "/misc/symbol.html"
  - "/study/misc/symbol"
  - "/study/misc/symbol.html"
---

# 特殊記号の入力

## <a id="sec-generated-title-1"></a> <a id="d23e4"></a>特殊記号の入力

電磁理論のところとかで∫とか∂とか多用しています。
JISコードにはこの手の記号がちゃんと用意されてますから、
MS IME2000だと「いんてぐらる」とか「きごう」って入力して変換すればこの手の記号を書けるわけで、
僕はそうやってこの手の記号を入力しています。


このページの場合、どうせ全文日本語ですし、この手の記号もIME使って変換するのが一番手っ取り早いんですが、じゃあ、IMEなんて物使ってない国の人らはどうやって入力すればいいのかというと、
<em>&amp;#****; (****のところにはUNICODEを10進数で書く)</em>
か、もしくは
<em>&amp;#x****; (****のところにはUNICODEを16進数で書く)</em>
とすることで、UNICODEで定義されている文字なら何でも入力することが出来ます。
特によく使う記号にはentity名が付いていて、
<em>&amp;****; (****のところにはentity名を書く)</em>
とすることで入力することも出来ます。
(例えば、&amp;int;(∫)や&amp;minus;(−)(普通に-を書くとハイフンになる)や&amp;part;(∂)などがあります)


いつつか例を挙げると、


<table summary="">

	<tr>
		<th>文字</th>
		<th>10進</th>
		<th>16進</th>
		<th>entity名</th>
	</tr>
	<tr>
		<td markdown="1">∫</td>
		<td markdown="1">&amp;#8747;</td>
		<td markdown="1">&amp;#x222B;</td>
		<td markdown="1">&amp;int;</td>
	</tr>
	<tr>
		<td markdown="1">∂</td>
		<td markdown="1">&amp;#8706;</td>
		<td markdown="1">&amp;#x2202;</td>
		<td markdown="1">&amp;part;</td>
	</tr>
	<tr>
		<td markdown="1">∀</td>
		<td markdown="1">&amp;#8704;</td>
		<td markdown="1">&amp;#x2201;</td>
		<td markdown="1">&amp;forall;</td>
	</tr>
	<tr>
		<td markdown="1">∃</td>
		<td markdown="1">&amp;#8707;</td>
		<td markdown="1">&amp;#x2203;</td>
		<td markdown="1">&amp;exist;</td>
	</tr>
	<tr>
		<td markdown="1">∇</td>
		<td markdown="1">&amp;#8711;</td>
		<td markdown="1">&amp;#x2207;</td>
		<td markdown="1">&amp;nabla;</td>
	</tr>
</table>



といったものがあります。
あと、同じ方法でアラビア文字とかハングル文字とかJISコードに含まれていない漢字も表示できたりもします。


でも、こういう風に特殊文字をUNICODEで直接書くためにはUNICODEのコード番号を調べないと書けません。
幸い、UNICODEの一覧を書いてくれているページもありますのでこちらを参照してください。
[Unicode and Multilingual Support in HTML, Fonts, Web Browsers and Other Applications](http://www.alanwood.net/unicode/)

余談なんですが、コード番号が分かっても即表示できるとは限りません。
コード番号があっても、フォントが用意されていない記号が多々あるからです。
例えば、&amp;#x22BB; ←XOR 記号とか、IE 5.0 では表示できません。
「⊻」←多分、「□」とか「・」とか「?」が表示されてると思います。


あと、限られた状況下でのみ表示できる文字なんかもあったりします。
例えば、集合論でよく使われる記号「‭א」（アレフ、ヘブライ文字、アルファベットのAに相当、某真理教の改名後の名前の元になった記号）とか、
IEで標準で使われている「MS P ゴシック」フォントでは表示できません。
でも、文字コードがUNICODEのときには文字の言語/分類に応じて適切なフォントを自動選択してくれるようで、アレフを表示できるようです。
（このHTMLファイルはUNICODEで書いているので、IEで見れば表示できているはず。）
