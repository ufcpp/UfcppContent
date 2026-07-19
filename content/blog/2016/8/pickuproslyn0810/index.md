---
title: "ピックアップRoslyn 8/10"
source_url: "https://ufcpp.net/blog/2016/8/pickuproslyn0810/"
content_type: "BlogEntry"
published_at: "2016-08-09T15:37:28"
updated_at: "2016-08-09T15:37:28"
tags: []
umbraco_id: 1937
parent_id: 1932
sort_order: 2
aliases: []
---

# ピックアップRoslyn 8/10

7月のデザインノートが2件ほど。

- [C# Language Design Notes for Jul 12, 2016](https://github.com/dotnet/roslyn/issues/13022)
- [C# Language Design Notes for Jul 13, 2016](https://github.com/dotnet/roslyn/issues/13015)

これ関連の作業がひと段落したところでまとめて清書して表に出したって感じですかねぇ。
この辺りの話、かなりの割合がもう実装されててマージされてたりします。

先週、dots.をお借りしてこんなイベントやってたわけですが

- [roslyn (C# 7) もくもく会](http://eventdots.jp/event/594178)

最新のmasterブランチの取ってきてビルドして実行してみると、大体この仕様通りになってる感じ。

さて、どんな感じの仕様かというと…

## タプル型のメンバー名は省略・名前付きの混在可能

こんなコードでOKですって。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> t = (<span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">1</span>, y: <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">2</span>); <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// infers (int, int y)</span>
(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span>) t = (<span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">1</span>, <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">2</span>);
</code></pre>

ちなみに、名前を省略したところは、`ValueTuple`がたの本来のメンバーである `x.Item1` とかの名前で参照できます。

## ITuple

タプル型みたいな「単に複数のデータを寄せ集めただけ」な型に対して、インデックスでメンバー参照したくなることがあります。
`ValueTuple`型はそのために、以下のようなインターフェイスを実装すべきじゃないかという話に。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">interface</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">ITuple</span>
{
    <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> Size;
    object this[int i] { <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">get</span>; }
}
</code></pre>

タプル型の分解に使いたいそうで。

`ValueTuple`型はこれを実装すべきだとは思うものの、名前にはまだ議論の余地あり。
インターフェイス名も`IDeconstructable`とかがいいかもしれないし。
要素数のプロパティも`Length`とか`Count`とかもあり得るし。

## var型がある場合。

C#のvarは、文脈キーワード(特定の文脈でだけキーワード扱いされる)です。`var`って名前のクラスがあると、クラス名として認識される。

で、タプル型の分解構文で以下のような書き方を認めることになるわけですが、

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> (x, y) = e;
</code></pre>

ここで、`var`クラスがあった場合どうなるべきか。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">var</span> {}
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> (x, y) = e;
</code></pre>

ちなみに、世の中には、わざわざこういう`var`クラスを用意しておくことで、型推論のvarを使わせない(コンパイル エラーにさせる)トリッキーな運用をしている人もいるそうで。C#チーム的には「(その良し悪しは置いといて)そういう運用も認めるべきでしょう」という感じ。
そういう背景もあって、タプル型の分解におけるvarでも、`var`クラスがあったらコンパイル エラーにするみたい。

## var メソッド

じゃあ、メソッドの場合はどうか。分解代入の構文、メソッド呼び出しに似ているので、以下のような書き方ができてしまいます。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">ref int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> y);
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span>(x, y) = e; <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// deconstruction or call?</span>
</code></pre>

参照戻り値なメソッドへの代入(参照先への代入)か、分解代入か、どちらにするべきか。

常に分解代入の方を選ぶそうです。メソッドの方を呼びたい場合は `@var` って書けばできます。

## partialクラスでのインターフェイス

partialクラスの場合、複数の宣言で、同じインターフェイスを継承できたりします。
ここで、じゃあ、メンバー名違いの同じ型のインターフェイスを継承してしまった場合はどうするべきか。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">partial</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">C</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">IEnumerable</span>&lt;(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">name</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">age</span>)&gt; { ... }
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">partial</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">C</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">IEnumerable</span>&lt;(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">fullname</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span>)&gt; { ... } 
</code></pre>

タプル型は、内部的には全部`ValueTuple`構造体に変換されます。
名前は属性に残るだけ。
で、じゃあ、上記の名前違いのインターフェイスは別の型なのか同じ型なのかよくわからず。
紛らわしいのでコンパイル エラーにすべきでしょう。
逆に、メンバー名も含めて全一致している場合だけは、複数のpartial宣言に書いても大丈夫。

もう少し面倒なケースは、多重継承(インターフェイスであればC#でも多重継承が可能)。
以下の場合はどうすべきか。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">interface</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">I1</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">IEnumerable</span>&lt;(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">a</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">b</span>)&gt; {}
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">interface</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">I2</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">IEnumerable</span>&lt;(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">c</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">d</span>)&gt; {}
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">interface</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">I3</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">I1</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">I2</span> {} <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// what comes out when you enumerate?</span>
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">C</span> : <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">I1</span> { <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> IEnumerator&lt;(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> e, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> f)&gt; <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">GetEnumerator</span>() {} } <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// what comes out when you enumerate?</span>
</code></pre>

現状、これもコンパイル エラーにする案で進めてるみたい。
できてそこまで大きなメリットもなさそうなので、複雑化させない方向に倒すという感じ。
もし、将来的にこれを認めたくなるような重要な利用シナリオが見つかったりした場合、それはその時に考える。

## タプル リテラルの分解

null (全ての参照型に代入可能)とか、1 (`int`、`short`, `byte` 辺りのどれか不明瞭)とか、リテラルの場合、型があいまいなものがあります。
その分解はちゃんと働くべきか。

<pre class="source" title="">
<code>(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">byte</span> y, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> z) = (<span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">null</span>, <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">1</span>, <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">2</span>);
</code></pre>

できるべきだろうとのこと。

各要素ごとに並べて書いた時と同じ挙動になるべき。上記コードであれば、まあ、↓みたいなのと同じ解釈をすべき。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> x = <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">null</span>;
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">byte</span> y = <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">1</span>;
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> z) = <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">2</span>;
</code></pre>

ただし、これが逐次実行されるわけじゃなくて、一斉に代入が起きる。つまり、swapに使っても差し支えないようなにはなってる。

<pre class="source" title="">
<code>(x, y) = (y, x); <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// swap!</span>
</code></pre>

## タプル型の中のvar

「タプル型の変数宣言」と「分解代入」は非常に似た構文になるわけですが。

<pre class="source" title="">
<code>(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> y) = GetTuple(); <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// 分解</span>
(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">int</span> y) t = GetTuple(): <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// タプル型の変数宣言</span>
</code></pre>

じゃあ、以下の構文(これも似て非なるもの)の場合はどうなるべきか。

<pre class="source" title="">
<code>(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> y) = GetTuple(); <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// これは分解代入時の型推論</span>
(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> x, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">var</span> y) t = GetTuple(): <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// varなタプル型。これは認めるべき？</span>
</code></pre>

で、結論的には、この後者は認めないとのこと。

## 分解代入の戻り値の型は void？

C#では、代入は式です。どこにでも書けます…

<pre class="source" title="">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = (x = 2) * x;
</code></pre>

まあ、ろくでもないんですが。副作用を伴う式とか割かし害悪。C言語を参考にしすぎたところですね。とはいえ、今更変更できません。

例えばの話、forステートメントの中には式を書くことになっているので、以下のようなコードを書きたければ、タプルの分解代入も式でないといけないそうです。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">for</span> (... ;; (current, next) = (next, next.Next)) { ... }
</code></pre>

とはいえ、実のところ、「戻り値がvoidの式」という扱いにすれば、forステートメントの中で使えつつ、さっきのろくでもない`y = (x = 2) * x` みたいなコードをなくせたりします。

ということで、voidであるべき？

まあ、これも、既存の代入式との一貫性がなくなるので、voidではなく、タプル型を返すべきだと思ってるみたいです。
C# 7では実装しなさそうだけど、後々は、分解代入の結果を、再度タプル構築して戻り値に返すべきだと思っているとのこと。

### 参考までに: Swift

ちなみに、Swiftはほんとに、代入は戻り値がvoidの式みたいです。
`y = (x = 2) * x` なんていうクソコードは認めません。

その割にインクリメント・デクリメントがあった `y = ++x * x` とか書けたわけですが。
そりゃ、forステートメントもインクリメントもなくしたくもなります(Swift 3で破壊的変更してまでなくす予定)。

## 分解を変換として、変換を分解として

分解代入と型変換はある程度似た構文です。分解は、タプル型への変換的な雰囲気があります。似てるのあれば、いっそある程度統一性を持たせるべき？

まあ、そうしない方がよさそう。分解(コンパイル結果的には`Deconstruct`メソッドの呼び出し)は型変換的に扱われるべきじゃない。

## 匿名型

匿名型(`{ X = 1, Y = "a" }` みたいなやつ)は`Deconstruct`メソッドや`ITuple`インターフェイス実装を持つべき？

そうでもなさそう。実装しても、今のところ有用な利用シナリオが思い当たらないとのこと。
欲しくなる場面もなくはないけど、そういう場面では大体タプル型を使えば解決しそう。

## 分解代入時のワイルドカード

ワイルドカードってのは、要するに、要らない部分を読み飛ばす機能。

<pre class="source" title="">
<code>(<span class="reserved">var</span> x, <span class="reserved">var</span> y, *) = (1, 2, 3);
</code></pre>

こういうコードで、3を読み飛ばすために使いもしないダミー変数を用意する必要はありません。

C#的に、こういう機能を入れるべきだろうとは思ってるみたい。
ただし、たぶん、C# 8になる(7には入らない。パターン マッチングと同時期に入る予定)。

あと、ワイルドカードのために使う記号はたぶん `*`。
関数型言語の類だと `_` を使うことが多いんですが、C#では `_` が有効な識別子になっちゃうので。
既存コードの意味を変えてまではこの記号は使わないみたい(コード解析をきっちりやれば不可能ではないけど、そうまでするかという話)。

## double型に対するswitch

パターン マッチングが入った暁には、`double`型の変数もswitchに使えるわけですが。
ここで問題になるのは、`double`型の等値判定。

NaN(Not a Number)の扱いどうするの？とか、実は`==`と`Equals`でNaNとの比較結果が違ったりするけどどうする？とか。

`==`と`Equals`の違いというと、`int`の1と`double`の1.0が等値判定とかも。前者はtrueになるけど、後者はfalse。

`Equals`の側を使いそう。
