---
title: "ピックアップ Roslyn 3/3: タプル ベースのposition-to-propertyマッチ"
source_url: "https://ufcpp.net/blog/2016/3/pickuproslyn0303/"
content_type: "BlogEntry"
published_at: "2016-03-03T03:42:50"
updated_at: "2016-03-03T03:42:50"
tags: []
umbraco_id: 1879
parent_id: 1877
sort_order: 1
aliases: []
---

# ピックアップ Roslyn 3/3: タプル ベースのposition-to-propertyマッチ

[一昨日の](../pickuproslyn0301/index.md)の補足。

先日は以下のようなLanguage Design Notesが出てたわけですが。

- [C# Design Notes - catch up edition, Feb 29, 2016 (deconstruction and immutable object creation) #9330](https://github.com/dotnet/roslyn/issues/9330)

コンストラクター引数とプロパティの名前の一致を見て、何番目の引数がどのプロパティに対応するかを調べる(position-to-propertyマッチする)方針を使おうという話。
もちろんいろんな対案あった中で、今、こういう方針に傾いてるという話なんですが、そういう過程抜きに「名前の一致を見る」の話だけしたので多少炎上中。

ということで、対案の1つ、[タプル](http://www.buildinsider.net/column/iwanaga-nobuyuki/003)を使ったパターンの話も公開されました。

- [Proposal: Tuple-based construction and deconstruction of immutable types #9411](https://github.com/dotnet/roslyn/issues/9411)

以下のようなメソッドを用意することで、position-to-propertyマッチしようというもの。
拡張メソッドでもいいことにしておけば、既存のクラスの拡張もできます(ただし、その拡張メソッドを書くのは手動。大変めんどいはず)。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">Person</span>
{
  ...
  <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> (<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> FirstName, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> LastName) <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">GetValues</span>() { ... }
  <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> Person <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">With</span>((<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-smi" style="box-sizing: border-box; color: rgb(51, 51, 51);">FirstName</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-smi" style="box-sizing: border-box; color: rgb(51, 51, 51);">LastName</span>) builder) { ... }
}
</code></pre>

with式みたいなものの実現には、既存の言語構文だけでやるならいわゆる「ビルダー パターン」を使ったりします。
そのためにはビルダー用のクラスを1個余計に作らないと行けなくて、余計なメモリ アロケーションが発生したり、余計なクラスを書く手間が掛かったり。

でも、[タプル型](http://www.buildinsider.net/column/iwanaga-nobuyuki/003)を使えば、タプルはmutableな構造体なので余計なアロケーションは起きない。
それに、新しいクラスの追加も必要なくて、手間は多少マシになる。
タプルは元々、引数位置と名前の対応関係を持っているので、黒魔術的な特殊処理なしでposition-to-propertyマッチできるはず、ということになります。

とはいえ、以下のように、悪い面もあります。

- 既存の型に対してそのままでは使えない。`GetValues`や`With`などの追加(拡張メソッドでもいいけど、手動での追加)が必要
- オブジェクトの分解(`GetValues`)やwith式(`With`)はインスタンス メソッドになるのでvirtualにできても、新規インスタンス作成ではできない
- (レコード型などの新構文で)コンパイラーが自動生成するコードが増える
- タプル型という、別の新構文に強く依存することになる(複雑度が増す)

というような話があっての、先日の「[名前で解決](../pickuproslyn0301/index.md)」の流れになったという話です。
