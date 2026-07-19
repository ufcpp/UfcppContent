---
title: "ニコタッチ方式"
source_url: "https://ufcpp.net/study/misc/list/nikotouch/"
content_type: "Article"
published_at: "2015-05-06T14:19:12"
updated_at: "2015-05-06T14:19:12"
tags: []
umbraco_id: 1540
parent_id: 1534
sort_order: 5
aliases:
  - "/misc/list/nikotouch/"
  - "/misc/nikotouch"
  - "/misc/nikotouch.html"
  - "/study/misc/nikotouch"
  - "/study/misc/nikotouch.html"
---

# ニコタッチ方式

##<a id="sec-generated-title-1"></a> <a id="intro"></a>概要
松下製の携帯電話に載っている文字入力方式「ニコタッチ」、
かなり優秀な入力方式だと思うのでちょっと布教活動をしてみようか、
というのがこのページの目的。

きっかけは、
SHARP の [W-ZERO3[es]](http://www.willcom-inc.com/ja/lineup/ws/007sh/index.html) を使ってみたことにあります。
フルキーボードで文字を打ってみたところ、
テンキーでニコタッチ方式で打つ方が速くて、
携帯電話における文字入力の中で、
現状最速で打てるのはこの方式なんじゃないかという気がしてきました
（2006年8月）。

いい方式だと思うんですが、
特定メーカの独自方式なので、
「こんな機能使っちゃうと機種変更できなくなる」という理由で敬遠されたりするのが残念です。
独自方式だから敬遠されるなら、
この方式がテンキー日本語入力のデファクトスタンダードになればいいんじゃね？
という妄想気味の淡い期待をこめて、
ちょっとこの方式の布教活動をしてみようかと。

追記：
W-ZERO3[es] をニコタッチ方式に対応させることの出来るプログラム（ctrlswapmini）があるようです。
「[関連リンク](#link)」参照。


##<a id="sec-generated-title-2"></a> <a id="mobile"></a>携帯電話の入力方式
携帯電話で文字を打とうと思うと、
テンキーで入力することになります。
ほとんど全ての携帯電話では、
標準の入力方式は「かな方式」というやつで、
ボタンを押すたびに文字が「あ → い → う → え → お → ・・・」と変わっていくタイプのものです。

（
2タッチ方式だって「かな」を入力するわけで、
実際には「かな入力」という呼び方はおかしいんですけどね。
2タッチ方式と区別してトグル入力と呼んでる人もいますが、
メーカ付属の説明書なんかでは「かな入力」と書かれている場合が多いので、
ここでもかな入力と呼んでおきます。
）

この方式、「あ段」の多い文章ならいいんですが、
「お段」の文字を出すには5回もボタンを押す必要があります。
[ローマ字方式でのアルファベット出現頻度](http://121ware.com/apinfo1/content/mworld/1-3.htm)を元に、
ボタンを押さなければいけない回数の期待値を求めると2.9回くらいなんですよね。
濁音・半濁音を出そうと思うと、もう少しタッチ数が増えます。

対して、
ポケベルなんかで使われていた2タッチ方式だと、
ボタンを押す回数は2回。
濁音を出すのに余計に1回、半濁音に2回のタッチが必要だと仮定しても、
平均2.2回のタッチで日本語を打てます。

あと、かな方式は同じ行の音が続くときにちょっと困りますね。
「あいおい」とか打とうと思うと、
「あ」を打って [→] ボタン、「い」を打って [→] ボタン・・・
みたいな打ち方が必要です。
これも、ベル打ちだと [1][1][1][2][1][5][1][2] で打てます。
ワーストケースな文章として、
「すももももももももはもも」とか打つとかなり悲惨な差が出ますね。

また、かな方式では、
かなとアルファベットは切り替えボタンを押して入力方式を切り替える必要があります。
これに対して、2タッチ方式では、
ボタンに余裕があって、
方式を切り替えなくてもかなとアルファベット、さらには数字も入力することができます。
したがって、英単語が出てくるような文章ではかな方式はさらに不利になります。
「今日、SHARP が出してる WILLCOM の  PHS の W-ZERO3 を買ってみた。」とかいう文章を打とうと思うと、
相当な差が生じます。


##<a id="sec-generated-title-3"></a> <a id="twotouch"></a>2タッチ方式（ベル打ち）
というわけで、2タッチ方式。
2タッチ方式というと、
全てではないですが結構多くの携帯電話にポケベル方式（通称：ベル打ち）が載っています。
ベル打ちの方が文字入力が速いというのは割とよく言われる話で、
実際、早打ちコンテストとかすると、
上位はベル打ちの人で占められるらしいです。

ですが、そんなベル打ちにもちょっと問題が。
「っ」「ゃ」「ゅ」「ょ」が打ちにくいのと、
数字とアルファベットの配置が覚えられない。

通常、ベル打ちの変換表は以下のようになっています。
（<span style="background:#008000;">　</span> はスペース文字。）

<table summary="ベル打ち（大文字）">
	<caption>
		ベル打ち（大文字）
	</caption>
	<tr>
		<td markdown="1" colspan="2" rowspan="2"></td>
		<th colspan="10">2桁目</th>
	</tr>
	<tr>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
		<th>5</th>
		<th>6</th>
		<th>7</th>
		<th>8</th>
		<th>9</th>
		<th>0</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="10">1<br></br>桁<br></br>目</td>
		<th>1</th>
		<td markdown="1">あ</td>
		<td markdown="1">い</td>
		<td markdown="1">う</td>
		<td markdown="1">え</td>
		<td markdown="1">お</td>
		<td markdown="1">A</td>
		<td markdown="1">B</td>
		<td markdown="1">C</td>
		<td markdown="1">D</td>
		<td markdown="1">E</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">か</td>
		<td markdown="1">き</td>
		<td markdown="1">く</td>
		<td markdown="1">け</td>
		<td markdown="1">こ</td>
		<td markdown="1">F</td>
		<td markdown="1">G</td>
		<td markdown="1">H</td>
		<td markdown="1">I</td>
		<td markdown="1">J</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">さ</td>
		<td markdown="1">し</td>
		<td markdown="1">す</td>
		<td markdown="1">せ</td>
		<td markdown="1">そ</td>
		<td markdown="1">K</td>
		<td markdown="1">L</td>
		<td markdown="1">M</td>
		<td markdown="1">N</td>
		<td markdown="1">O</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">た</td>
		<td markdown="1">ち</td>
		<td markdown="1">つ</td>
		<td markdown="1">て</td>
		<td markdown="1">と</td>
		<td markdown="1">P</td>
		<td markdown="1">Q</td>
		<td markdown="1">R</td>
		<td markdown="1">S</td>
		<td markdown="1">T</td>
	</tr>
	<tr>
		<th>5</th>
		<td markdown="1">な</td>
		<td markdown="1">に</td>
		<td markdown="1">ぬ</td>
		<td markdown="1">ね</td>
		<td markdown="1">の</td>
		<td markdown="1">U</td>
		<td markdown="1">V</td>
		<td markdown="1">W</td>
		<td markdown="1">X</td>
		<td markdown="1">Y</td>
	</tr>
	<tr>
		<th>6</th>
		<td markdown="1">は</td>
		<td markdown="1">ひ</td>
		<td markdown="1">ふ</td>
		<td markdown="1">へ</td>
		<td markdown="1">ほ</td>
		<td markdown="1">Z</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>7</th>
		<td markdown="1">ま</td>
		<td markdown="1">み</td>
		<td markdown="1">む</td>
		<td markdown="1">め</td>
		<td markdown="1">も</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
	</tr>
	<tr>
		<th>8</th>
		<td markdown="1">や</td>
		<td markdown="1">　</td>
		<td markdown="1">ゆ</td>
		<td markdown="1">　</td>
		<td markdown="1">よ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>9</th>
		<td markdown="1">ら</td>
		<td markdown="1">り</td>
		<td markdown="1">る</td>
		<td markdown="1">れ</td>
		<td markdown="1">ろ</td>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">4</td>
		<td markdown="1">5</td>
	</tr>
	<tr>
		<th>0</th>
		<td markdown="1">わ</td>
		<td markdown="1">を</td>
		<td markdown="1">ん</td>
		<td markdown="1">゛</td>
		<td markdown="1">゜</td>
		<td markdown="1">6</td>
		<td markdown="1">7</td>
		<td markdown="1">8</td>
		<td markdown="1">9</td>
		<td markdown="1">0</td>
	</tr>
</table>


<table summary="ベル打ち（小文字）">
	<caption>
		ベル打ち（小文字）
	</caption>
	<tr>
		<td markdown="1" colspan="2" rowspan="2"></td>
		<th colspan="10">2桁目</th>
	</tr>
	<tr>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
		<th>5</th>
		<th>6</th>
		<th>7</th>
		<th>8</th>
		<th>9</th>
		<th>0</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="10">1<br></br>桁<br></br>目</td>
		<th>1</th>
		<td markdown="1">ぁ</td>
		<td markdown="1">ぃ</td>
		<td markdown="1">ぅ</td>
		<td markdown="1">ぇ</td>
		<td markdown="1">ぉ</td>
		<td markdown="1">a</td>
		<td markdown="1">b</td>
		<td markdown="1">c</td>
		<td markdown="1">d</td>
		<td markdown="1">e</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">f</td>
		<td markdown="1">g</td>
		<td markdown="1">h</td>
		<td markdown="1">i</td>
		<td markdown="1">j</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">k</td>
		<td markdown="1">l</td>
		<td markdown="1">m</td>
		<td markdown="1">n</td>
		<td markdown="1">o</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">っ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">p</td>
		<td markdown="1">q</td>
		<td markdown="1">r</td>
		<td markdown="1">s</td>
		<td markdown="1">t</td>
	</tr>
	<tr>
		<th>5</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">u</td>
		<td markdown="1">v</td>
		<td markdown="1">w</td>
		<td markdown="1">x</td>
		<td markdown="1">y</td>
	</tr>
	<tr>
		<th>6</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">z</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>7</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
	</tr>
	<tr>
		<th>8</th>
		<td markdown="1">ゃ</td>
		<td markdown="1">　</td>
		<td markdown="1">ゅ</td>
		<td markdown="1">　</td>
		<td markdown="1">ょ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>9</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>0</th>
		<td markdown="1">ゎ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">、</td>
		<td markdown="1">。</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
</table>


アルファベットは、
[1][6] ～ [1][0] で a, b, c, d, e、
[2][6] ～ [2][0] で f, g, h, i, j
というように、
5文字ずつ [6]～[0] のボタンに詰め込んで配置されています。
ですが、アルファベットの何文字目が何かなんて、
ぱっと思いつきません。
[3][2] と打てば12番目のアルファベットが出ると言うことが分かっていても、
12番目のアルファベットが何なのかが分からないと打ちようがありません。
「12番目のアルファベットは何？」と聞かれて即答できる日本人はほとんどいないと思います。


##<a id="sec-generated-title-4"></a> <a id="niko"></a>ニコタッチ方式
ニコタッチ方式は、
松下の独自方式で、DoCoMo の P シリーズなんかに載っているんですが、
ポイントは2点。
ガイド機能と、ベル打ちの不満解消です。

1つ目は、1桁目のボタンを押した時点で、
2桁目に何を押せばどの文字が出るかガイドが出ること。
2タッチ方式は、覚えてしまえば速いとはいえ、
慣れるまで多少時間がかかります。
特に、「、」「。」等の記号は、分かりやすい規則がないので、
覚えるまでかなり悩むと思います。

そこで、ニコタッチ方式では、1桁目を押した時点でガイドを表示します。
うろ覚えで、
「確か記号は [1] か [0] から始まる」ということだけ覚えていれば、
「、」「。」等もそこまで悩まず打つことが出来ます。

まあ、この点に関しては、覚えてしまえは不要な機能なんですが、
重要なのはもう1つの方、
前述のベル打ちの不満点の解消です。

ニコタッチ方式の変換表は以下のようになっています。
（<span style="background:#008000;">　</span> はスペース文字。）

<table summary="ニコタッチ（大文字）">
	<caption>
		ニコタッチ（大文字）
	</caption>
	<tr>
		<td markdown="1" colspan="2" rowspan="2"></td>
		<th colspan="10">2桁目</th>
	</tr>
	<tr>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
		<th>5</th>
		<th>6</th>
		<th>7</th>
		<th>8</th>
		<th>9</th>
		<th>0</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="10">1<br></br>桁<br></br>目</td>
		<th>1</th>
		<td markdown="1">あ</td>
		<td markdown="1">い</td>
		<td markdown="1">う</td>
		<td markdown="1">え</td>
		<td markdown="1">お</td>
		<td markdown="1">.</td>
		<td markdown="1">-</td>
		<td markdown="1">@</td>
		<td markdown="1">_</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">か</td>
		<td markdown="1">き</td>
		<td markdown="1">く</td>
		<td markdown="1">け</td>
		<td markdown="1">こ</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
		<td markdown="1">a</td>
		<td markdown="1">b</td>
		<td markdown="1">c</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">さ</td>
		<td markdown="1">し</td>
		<td markdown="1">す</td>
		<td markdown="1">せ</td>
		<td markdown="1">そ</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
		<td markdown="1">d</td>
		<td markdown="1">e</td>
		<td markdown="1">f</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">た</td>
		<td markdown="1">ち</td>
		<td markdown="1">つ</td>
		<td markdown="1">て</td>
		<td markdown="1">と</td>
		<td markdown="1">っ</td>
		<td markdown="1">g</td>
		<td markdown="1">h</td>
		<td markdown="1">i</td>
		<td markdown="1">4</td>
	</tr>
	<tr>
		<th>5</th>
		<td markdown="1">な</td>
		<td markdown="1">に</td>
		<td markdown="1">ぬ</td>
		<td markdown="1">ね</td>
		<td markdown="1">の</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
		<td markdown="1">j</td>
		<td markdown="1">k</td>
		<td markdown="1">l</td>
		<td markdown="1">5</td>
	</tr>
	<tr>
		<th>6</th>
		<td markdown="1">は</td>
		<td markdown="1">ひ</td>
		<td markdown="1">ふ</td>
		<td markdown="1">へ</td>
		<td markdown="1">ほ</td>
		<td markdown="1"><span style="background:#008000;">　</span></td>
		<td markdown="1">m</td>
		<td markdown="1">n</td>
		<td markdown="1">o</td>
		<td markdown="1">6</td>
	</tr>
	<tr>
		<th>7</th>
		<td markdown="1">ま</td>
		<td markdown="1">み</td>
		<td markdown="1">む</td>
		<td markdown="1">め</td>
		<td markdown="1">も</td>
		<td markdown="1">p</td>
		<td markdown="1">q</td>
		<td markdown="1">r</td>
		<td markdown="1">s</td>
		<td markdown="1">7</td>
	</tr>
	<tr>
		<th>8</th>
		<td markdown="1">や</td>
		<td markdown="1">ゆ</td>
		<td markdown="1">よ</td>
		<td markdown="1">ゃ</td>
		<td markdown="1">ゅ</td>
		<td markdown="1">ょ</td>
		<td markdown="1">t</td>
		<td markdown="1">u</td>
		<td markdown="1">v</td>
		<td markdown="1">8</td>
	</tr>
	<tr>
		<th>9</th>
		<td markdown="1">ら</td>
		<td markdown="1">り</td>
		<td markdown="1">る</td>
		<td markdown="1">れ</td>
		<td markdown="1">ろ</td>
		<td markdown="1">w</td>
		<td markdown="1">x</td>
		<td markdown="1">y</td>
		<td markdown="1">z</td>
		<td markdown="1">9</td>
	</tr>
	<tr>
		<th>0</th>
		<td markdown="1">わ</td>
		<td markdown="1">を</td>
		<td markdown="1">ん</td>
		<td markdown="1">、</td>
		<td markdown="1">。</td>
		<td markdown="1">ー</td>
		<td markdown="1">・</td>
		<td markdown="1">？</td>
		<td markdown="1">！</td>
		<td markdown="1">0</td>
	</tr>
</table>


<table summary="ニコタッチ（小文字）">
	<caption>
		ニコタッチ（小文字）
	</caption>
	<tr>
		<td markdown="1" colspan="2" rowspan="2"></td>
		<th colspan="10">2桁目</th>
	</tr>
	<tr>
		<th>1</th>
		<th>2</th>
		<th>3</th>
		<th>4</th>
		<th>5</th>
		<th>6</th>
		<th>7</th>
		<th>8</th>
		<th>9</th>
		<th>0</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="10">1<br></br>桁<br></br>目</td>
		<th>1</th>
		<td markdown="1">ぁ</td>
		<td markdown="1">ぃ</td>
		<td markdown="1">ぅ</td>
		<td markdown="1">ぇ</td>
		<td markdown="1">ぉ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>2</th>
		<td markdown="1">ヵ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">ヶ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">A</td>
		<td markdown="1">B</td>
		<td markdown="1">C</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>3</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">D</td>
		<td markdown="1">E</td>
		<td markdown="1">F</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>4</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">っ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">つ</td>
		<td markdown="1">G</td>
		<td markdown="1">H</td>
		<td markdown="1">I</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>5</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">J</td>
		<td markdown="1">K</td>
		<td markdown="1">L</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>6</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">M</td>
		<td markdown="1">N</td>
		<td markdown="1">O</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>7</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">P</td>
		<td markdown="1">Q</td>
		<td markdown="1">R</td>
		<td markdown="1">S</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>8</th>
		<td markdown="1">ゃ</td>
		<td markdown="1">ゅ</td>
		<td markdown="1">ょ</td>
		<td markdown="1">や</td>
		<td markdown="1">ゆ</td>
		<td markdown="1">よ</td>
		<td markdown="1">T</td>
		<td markdown="1">U</td>
		<td markdown="1">V</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>9</th>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">W</td>
		<td markdown="1">X</td>
		<td markdown="1">Y</td>
		<td markdown="1">Z</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<th>0</th>
		<td markdown="1">ゎ</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
		<td markdown="1">　</td>
	</tr>
</table>


ちなみに、濁点・半濁点は、
清音で文字を入力した後に [*] ボタン。
また、文字入力後に [電話] ボタンを押すことで小文字・大文字の切り替えも出来ます。


###<a id="sec-generated-title-5"></a> <a id="alphabet"></a>アルファベット
まずはアルファベット入力に関して。
携帯電話のキーには
[2] ～ [9] のボタンにアルファベットが印字されていますね。

<table summary="携帯電話のボタン">
	<caption>
		携帯電話のボタン
	</caption>
	<tr>
		<td markdown="1">1 あ</td>
		<td markdown="1">2 か<br></br>ABC</td>
		<td markdown="1">3 さ<br></br>DEF</td>
	</tr>
	<tr>
		<td markdown="1">4 た<br></br>GHI</td>
		<td markdown="1">5 な<br></br>JKL</td>
		<td markdown="1">6 は<br></br>MNO</td>
	</tr>
	<tr>
		<td markdown="1">7 ま<br></br>PQRS</td>
		<td markdown="1">8 や<br></br>TUV</td>
		<td markdown="1">9 ら<br></br>WXYZ</td>
	</tr>
	<tr>
		<td markdown="1">　</td>
		<td markdown="1">0 わ<br></br>をん</td>
		<td markdown="1">　</td>
	</tr>
</table>


元々はアメリカの電話機に印字されていたものです。
英語では、日本みたいに語呂合わせで番号を覚えられないので、
このボタンに印字された番号を使って、
社名に対応する電話番号をつけたりします。
例えば、
Microsoft なら、表3から、
m → 6、i → 4、・・・なので、
642767638 になります。
（注： 実際にこれが Microsoft の電話番号というわけではないです。）

ニコタッチ方式では、
このボタンに印字された文字に沿ってアルファベットを入力します。
1桁目はボタンに印字された通りに打ち、
2桁目は [6]～[9] を使います。
（1ボタンに3文字のものは [7]～[9]、4文字のものは [6]～[9]。）
a, b, c はそれぞれ [2][7], [2][8], [2][9]、
p, q, r, s はそれぞれ [7][6], [7][7], [7][8], [7][9] という具合です。

変換表を覚えていなくても、ボタンを見ればアルファベットの打ち方が分かります。

また、
日本語大文字モードのときに、アルファベットの小文字が出るようになっています。
よく考えてみたら、この方が自然なんですね。
日本語はほとんど大文字なのに対して、
英語の文章は文頭を除いて基本的に全部小文字で打ちますから。
よく打つもの同士を同じ設定で打てるようにするのが自然です。


###<a id="sec-generated-title-6"></a> <a id="small"></a>小文字
アルファベットをぎちぎちに詰め込むのを辞めたので、
2桁目 [6] ボタンが余っています。
そこで、ニコタッチではこの部分に「っ」等の小文字を入れています。
要するに、[4][6] で「っ」を入力できます。
これを覚えると、小さい「っ」入力するのに、[4] ボタンを6回も押すのが馬鹿馬鹿しくなります。

また、「ゃ」「ゅ」「ょ」も、
[8][1], [8][2], [8][3]、[8][4], [8][5], [8][6] でそれぞれ
「や」「ゆ」「よ」「ゃ」「ゅ」「ょ」が出るようになっています。
（ただ、個人的には、これは
「や」「ゃ」「ゆ」「ゅ」「よ」「ょ」という順序にして欲しかったです。
そうすると、「ゆ」「よ」の部分で2桁目 [3], [5] は母音の「う」「お」というルールが一貫するので。）


###<a id="sec-generated-title-7"></a> <a id="digit"></a>数字
数字の入力もかなり覚えやすく出来ています。
1桁目は数字そのもの、2桁目は [0]。
以上です。

「8月15日0時0分」とか言うように、数字はせいぜい1・2桁しか入力しないような場合には、
入力方式を切り替えるのと比べて [0] をはさむ方が楽です。
日付やら時間やら、メールの文面でに出てくる数字は、
かなりの割合で数桁程度です。


###<a id="sec-generated-title-8"></a> <a id="symbol"></a>記号
携帯電話でよく使う記号は何か考えてみてください。
日本語文章でよく使うのは、
句読点・感嘆符「、。！？」、のばし棒「ー」、ドット「・」辺りですね。
また、ネットやメールが使える最近の携帯では、
メアドや URL によく使う「. - @ _ 」等を打つ頻度は結構高いです。

ということで、ニコタッチ方式では、
アルファベットの割り当たっていない [1] と [0] にこれらの記号を割り当てています。
1桁目 [1]、2桁目 [6]～[9] に 「. - @ _」、
1桁目 [0]、2桁目 [4]～[9] に「、。ー・！？」が割り当たっています。
空白文字は、残った部分 [2][6]、[3][6]、[5][6]、[6][6] などに割り当たっています。
個人的には、[6][6] が覚えやすいので、これで打っています。

その他の記号は流石に2タッチでは打てず、
[#] ボタンを何度か押して切り替えたり、
日本語変換機能で「かっこ」を「()」に変換したりして出します。


##<a id="sec-generated-title-9"></a> <a id="summary"></a>まとめ
ニコタッチ方式は、特定メーカの独自方式にしておくには惜しいんじゃないかな。

松下が自社ウェブサイトでプロモーションしてる様子がないので、
ここで布教活動を。

* 2タッチ入力。日本語文章を平均 2.2 タッチで打てる。

* （ガイド機能付き。1桁目を入力した時点で、どのボタンでどの文字が出るか分かる。）

* アルファベット、数字の打ち方を覚えやすい。ボタンに印字されてる通りに入力。

* よく使う記号も2タッチで出せる。ネット・メール用に「. - @ _」、日本語文章用に「、。ー・！？」。

* アルファベット、数字の入力方式切り替え不要。「SHARP の W-ZERO3 の新機種が7月27日から販売開始。」とか全部切り替えなし、2タッチで打てる。



##<a id="sec-generated-title-10"></a> <a id="link"></a>関連リンク
[ポケベル入力愛好会](http://ikamtls.hp.infoseek.co.jp/)
: 名前どおり、ポケベル入力愛好者用のサイト。

[フルパワー全開 WindowsCE のページ](http://hp.vector.co.jp/authors/VA004474/wince/wince.html)
: W-ZERO3[es] の文字入力方式を改善するためのツール「ctrlswapmini」を公開しているページ。 ctrlswapmini は、元々は ctrl キーと caps キーを入れ替えるためのソフトだったようですが、 現在ではテンキーまで含めて全部自由に入力方式を設定できるようになっています。 テンキーのキーマップは自由に変更できて、 P902i 風（ニコタッチ方式含む）や Vodafone 905SH 風の入力も実現できます。
