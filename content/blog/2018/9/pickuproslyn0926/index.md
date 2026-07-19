---
title: "ピックアップRoslyn 9/26"
source_url: "https://ufcpp.net/blog/2018/9/pickuproslyn0926/"
content_type: "BlogEntry"
published_at: "2018-09-26T23:15:26"
updated_at: "2018-09-26T23:15:26"
tags: []
umbraco_id: 2170
parent_id: 2168
sort_order: 1
aliases: []
---

# ピックアップRoslyn 9/26

Design Notes 2件追加。

- [Added: LDM notes for 2018-09-10 #1878](https://github.com/dotnet/csharplang/issues/1878)
    1. Nullability of constraints in inheritance and interface implementation
    2. Static local functions
- [Added: LDM Notes for September 19th, 2018 #1889](https://github.com/dotnet/csharplang/issues/1889)
    1. XML doc comment features
    2. New `foreach` pattern using `Length` and indexer
    3. Object initializers for `readonly` members
    4. `readonly` struct methods
    5. `params Span<T>`
    6. Nullable reference type features on `Nullable<T>`

## オーバーライド時の nullability

C# 8.0では、参照型に対して単に`T`と書くと非null、`T?`と書いた時だけnullを許容するようにしたいわけですが。
十数年の資産がある中、破壊的変更にならないように、属性を使ってnullチェックの有無を切り替えれるようになる予定です。
ちなみに、以下のような選択肢があります。

- null チェックをしない
- null チェックをする
    - チェックしていないクラスを参照した場合、`T` をnull許容扱いする
    - チェックしていないクラスを参照した場合、`T` を非null扱いする

で、基底クラスと派生クラスで選んだ選択肢が違う場合にどう扱うべきかという話題が。

## static local functions

[ローカル関数](../../../../study/csharp/cheatsheet/ap_ver7.md#local-functions)に対して、意図せずローカル変数をキャプチャしないように、「`static`修飾子を付けたらキャプチャ不可」という修飾をできるようにしようというのが静的ローカル関数(static local functions)。
これに対して以下のような話題。

- 全部キャプチャするか、全部しないかだけ？キャプチャ リスト(「この変数だけキャプチャする」みたいな指定を明示的に行う機能)とかも用意する？ → しない。全部するかしないかの2択
- ローカル関数への属性指定はできるようにする？ → する予定
- ローカル関数内で、その外と同名の変数を使える(shadowing、外側のやつを隠す処理)ようにする？ → 考えているけど、staticなものだけに対してやるってのは嫌。既存の、普通のローカル関数でもできるようにしたいし、将来的にはラムダ式でも考えたい
- ジェネリック関数内の静的ローカル関数でも、型引数はキャプチャする？ → Yes
- 「static ラムダ」はやる？ → やらない。ローカル関数と比べると短く使うことが多いし、式に埋め込んで書くものなので。

## XML Doc コメント

[XML Doc コメント](../../../../study/csharp/misc/sp_xmldoc.md)に対して、いろいろタグを追加したいとかいう話もかなり積もっていたりします。

Doc コメントは、Visual Studio上でのツールチップ表示とか、[Webページ](https://docs.microsoft.com/ja-jp/dotnet/)出力したりとか、ツールの対応が必要になります。
チーム外との折衝が必要なので、C# チーム的には常に腰が重いようで、ほんとに要望がたまりにたまっていたり。

で、今回のDesign Noteでようやく検討したみたいなんですが、「やるなら一気にやらないと」、「ツールのバージョンアップのタイミングでやりたいので、C# 的にもメジャー リリースのタイミングで対応したい」とのこと。
(といっても、マイルストーンは「X.0」。少なくとも8.0ではやらない模様。)

## foreach のインデックス アクセス最適化

C# は昔から、配列に対して `foreach (var x in array)` と書くと、`for (var i = 0; i < array.Length; i++) { var x = array[i] }` 相当のコードに最適化されます。
C# 7.2では、[`Span<T>`](../../../../study/csharp/resource/span.md)に対しても同様の最適化を掛けるようにしました。

で、ここでの議題は、配列と`Span<T>`だけを特別扱いするのか、それとももっと汎用な仕組みを提供するのか。

- [Champion: `foreach` using `Length` and indexer #1424](https://github.com/dotnet/csharplang/issues/1424)

今回のDesign Meetingの結果、この提案の形ではやらないけども、この手の`foreach`の最適化自体は引き続き何かしら検討したいとのこと。

## readonly object initializer

`readonly`なフィールドに対しても[オブジェクト初期子](../../../../study/csharp/oop/oo_construct.md#member_initializer)を使えるようにしたいという提案がありまして。

- [Champion: readonly object initializers #1684](https://github.com/dotnet/csharplang/issues/1684)

[Record 型の提案](https://github.com/dotnet/csharplang/issues/39)とかなり被ってるところがあるし、今出ている提案の形で進む前に、もっと汎用な仕組みができないか検討したいとのこと。

## readonly functions on structs

[構造体なフィールドに`readonly`を付けると、かえってコピーが多発してパフォーマンスを落とす場合が](../../../../study/csharp/resource/readonlyness.md#avoid-copy)あったりします。
これを避けるためのreadonly structなんですが、
メソッド単位でも「このメソッド内ではフィールドの書き換えはしません」という修飾がしたいという提案があります。

- [Champion: Readonly members on structs #1710](https://github.com/dotnet/csharplang/issues/1710)

興味はあって、引き続き、今の提案の形で進めていくとのこと(今すぐにやるとは言っていない)。

## params Span

今のC#は、[可変長引数](../../../../study/csharp/structured/sp_params.md)で使えるのは配列だけなわけですが。
`Span<T>`で可変長引数を受けたいという提案あり。

- [Champion "params Span<T>" #1757](https://github.com/dotnet/csharplang/issues/1757)

ただ、これを制限なく実装したければ、.NETランタイムのレベルでの修正が必要です。
([`stackalloc`](../../../../study/csharp/resource/span.md#safe-stackalloc)を使って可変長引数を渡したいものの、現状、ランタイムの制限で、`stackalloc`が使えるのは[`unmanaged`な型](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-constraints)だけです。)

とりあえずC# 8.0目標で進めたいものの、ちょっとやるべきことが多い状態。

## nullable reference types 関連の機能を nullable value types にも

1. nullable value types → 要するに C# 2.0 からある[null許容型](../../../../study/csharp/resource/sp2_nullable.md)のこと
2. nullable reference types → C# 8.0 で入る予定の null チェック機構

1の方は、`Nullable<T>`構造体を使った実装で、`T` と `T?` が実際に違う型で、`T?` から `T` を取り出すときには `Value`プロパティとか`GetValueOrDefault()`メソッドを介します。

それに対して2の方は、nullを追跡するためのアノテーションであって、`T` も `T?` もどちらも実行時には同じ型になっていて、コンパイラーのnullチェックが掛かるだけです。

まあ、nullable 絡みは途中のバージョンから追加するには結構厳しい機能で、
特に2の方はずっと無理だといわれ続けた結果8.0まで入らなかったくらいなので、
ちょっと無理があるのはしょうがなく、1と2で結構な差が生じています。

とはいえ、差を縮めるように1の方に手を入れることは考えてもよさげ。

- [Proposal: Adding nullable reference type features to nullable value types #1865](https://github.com/dotnet/csharplang/issues/1865)

要するに、`Nullable<T>`構造体に対して、例えば以下のようなものも検討したいという話。

- nullチェックをしないまま`Value`プロパティにアクセスさせない(したら警告)
- nullチェック後であれば、`T?`のままで(`?.`でなく`.`で)`T`のメンバーにアクセスできるようにする
- nullチェック後であれば、`T`を引数に取るメソッドに対して`T?`の変数でオーバーロード解決できるようにする

いずれは実装したいみたいですけども、C# 8.0時点では「将来これを実装する段階になって破壊的変更にならないかだけ注意する」という扱い。
