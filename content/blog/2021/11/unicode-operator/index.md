---
title: "Unicode 演算子 (∑Σ とか ∫ʃ とか)"
source_url: "https://ufcpp.net/blog/2021/11/unicode-operator/"
content_type: "BlogEntry"
published_at: "2021-11-28T14:10:33"
updated_at: "2021-11-28T14:11:58"
tags: []
umbraco_id: 2374
parent_id: 2363
sort_order: 6
aliases: []
---

# Unicode 演算子 (∑Σ とか ∫ʃ とか)

C# ライブ配信をしていて、「[括弧用の記号の種類が少なすぎる](https://youtu.be/7-E3GYiQU9o?t=6523)」みたいな話題から、
「あるよ、括弧。Unicode には」みたいな話になり、
「Swift ではマジでいろんな記号が使える」という話に脱線したときの話。

配信では「括弧がたくさんある」という話と「Swift では演算子にいろんな文字が使える」という話が混ざっていて、
実際に Swift で色々使えるのは括弧ではないんですけど、演算子の方は本当に Swift で使えるものがかなり自由が効く仕様になっていまして。
例えば以下のコードはコンパイルして実行できます。
(1から5の和で、15が出力されます。)

```swift {title="Swift の Unicode 演算子"}
prefix operator ∑

prefix func ∑ (x: [Int]) -> Int {
  var sum = 0
  for i in x {
    sum += i
  }
  return sum
}

let Σ = [ 1, 2, 3, 4, 5 ]

print(∑Σ)
```

`∑Σ`。

シグマシグマ。

ちなみにこれ、同じ記号に見えて実は違う文字でして。

* (1個目の)∑ : U+2211。数学記号の「和」の意味のシグマ。
* (2個目の)Σ : U+03A3。ギリシャ文字大文字のシグマ。

Swift は、

* 変数名に使える文字も[演算子に使える文字](https://github.com/apple/swift/blob/08e7963/include/swift/AST/Identifier.h#L87-L121)も許可リスト方式
* [ギリシャ文字](https://en.wikipedia.org/wiki/Greek_and_Coptic)の letter は変数名に使える
* [数学記号](https://en.wikipedia.org/wiki/Mathematical_operators_and_symbols_in_Unicode)は演算子に使える

という仕様になっているので、上記のようなコードが合法になります。

## シグマとシグマ

まあ、「なんで文字コード分けた」という話ではあります。
Unicode、同じような意味、読み、形の文字は統合する方針じゃないの？
シグマとシグマを分けるんだったら、日中韓漢字統合(いわゆる[中華フォント問題](https://www.itmedia.co.jp/news/articles/2110/29/news136.html))とかやめてよ…

そしてまあ、同じようなことができる組み合わせは他にもいろいろとあります。
ぱっと思いつくのは以下のようなもの。

パイ:

* ∏ : U+220F。数学記号の「積」の意味のパイ。
* Π : U+03A0。ギリシャ文字大文字のパイ。

long S:

* ∫ : U+222B。積分記号。積分記号自体が「S を引き延ばしたもの」が由来。
* ʃ : U+0283。発音記号の "esh"。S 亜種の発音。
  * letter 扱いらしく、変数名に使える。

Union:

* ∪: U+222A。Union (和集合)の数学記号。
* U: U+0055。ローマ字の(ASCII コード中にある) U の大文字。

c の丸囲み: 

* © : U+00A9。コピーライト記号。演算子にできるらしい…
* c⃝: U+0063 U+20DD。c に combining enclosing circle を付けて丸囲み。
  * サポートしているフォントは少ないものの、ものによっては © と似たような描画をされるはず。
  * letter + diacritical mark なので変数名に使える。

他に ℵ(U+2135。ヘブライ文字を元にした数学記号のアレフ)と א (U+05D0。元々のヘブライ文字のアレフ)とかも思いついたんですけど、これは両方後も letter (変数名に使えて、演算子には使えない文字)でした。
ちなみにこのアレフに至っては、数学記号の方は L to R (左書き)、ヘブライ文字の方は R to L (右書き)の文字で、後ろに続く文字のレンダリングに影響を及ぼしたりします。
