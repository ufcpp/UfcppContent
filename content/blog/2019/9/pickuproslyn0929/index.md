---
title: "ピックアップRoslyn 9/29: Utf8String など、C# 9.0 がらみ"
source_url: "https://ufcpp.net/blog/2019/9/pickuproslyn0929/"
content_type: "BlogEntry"
published_at: "2019-09-29T13:55:28"
updated_at: "2019-09-29T20:00:12"
tags: []
umbraco_id: 2267
parent_id: 2264
sort_order: 1
aliases: []
---

# ピックアップRoslyn 9/29: Utf8String など、C# 9.0 がらみ

C# 8.0 もリリースされたわけですし、
(リリース直前に 8.0 に関して修正も聞かないので、今月に入ったくらいから)
C# 9.0 に向けた Design Meeting が開かれているみたいです。
[.NET Conf 2019](https://www.dotnetconf.net/) も落ち着いたところで、議事録も公開。

- [LDM notes for 2019-09-16 and 2019-09-18 #2826](https://github.com/dotnet/csharplang/issues/2826)

雰囲気的には

- .NET 5 向け
- C# 9.0 を想定

みたいな感じなので、たぶん来年のリリースですね。
(.NET 5 は 2020年11月と明言されてる。
C# を .NET のリリースとどう同期させるかは明言されていないものの、
議事録の雰囲気からすると .NET 5 時点で C# 9.0。
.NET Core 3.1 (今年11月)時点で何か更新しそうな気配もなし。)

議題については

- Utf8String
- すでに 9.0 に向けてやると決まってたことについて再確認
  - 特に、ローカル関数への属性適用と、target-typed new
- いくつかトリアージ
  - [Language Feature Status の C# vNext に5つほど項目追加](https://github.com/dotnet/roslyn/commit/0056d765287bdc6717d38be515d3026792771f60?short_path=62d2c4c#diff-62d2c4ce577204aa80f67774438c2beb)

という感じです。

Design Notes とは別に、以下のドキュメントにも更新 Pull Request あり。

- [Native-sized integers proposal #2833](https://github.com/dotnet/csharplang/pull/2833)
  - 内部的には `IntPtr`、`UIntPtr` をそのまま使って、コンパイラーのレベルでだけ `nint`、`nuint` という別の型に見せかける実装にするみたい

## Utf8String

UTF-8 なバイト列を直接読み書きしたいという要望はかねてからあるというか、
C# で Web なことをするときに一番ネックになっていたのが UTF-8 (Web 上ではもうほぼこのエンコード)から UTF-16 (.NET の string の内部表現)への変換です。
なので、UTF-8 を直接読み書きするための型として `Utf8String` を追加したいという話は3・4年前くらいから出ていますし、プロトタイプ実装はすでにあります。
この型の正式リリースは .NET 5 にしたいということで、それに対する C# 言語上のサポートも C# 9.0 がターゲットになっています。

この `Utf8String` に関して、今回の議題は以下のようなもの。

- リテラルを exe/dll にどう埋め込むか
  - いったんは既存の `string` リテラルの仕組みをそのまま使って UTF-16 の状態で記録して、ランタイムが読み込み時に UTF-8 変換する方式をとる
  - ただ、どう考えても UTF-8 データを生で埋め込める方が効率的なので、将来的にその方式に変更するときに困らないかだけは要確認
      - 今のところそういう内部的な変更が C# の破壊的変更になりそうなものは思いつかない(ので大丈夫)
- 列挙をどうするか
  - UTF-8 な byte 列、Unicode コードポイント(あと、もしかしたら[書記素クラスター](../../../2018/12/unicodecategory/index.md)も)をそれぞれ列挙するためのプロパティはある
  - (プロパティを介さず) `Utf8String` 自体を `foreach` に渡したいか？渡すなら何が列挙されるべきか？
      - プロパティを介さない列挙は認めない方が無難そう
- C# 言語上の特殊対応は必要か。以下の2点は特殊対応する利点がありそう
  - `Concat` の最適化(n 個の文字列の `Concat` に対して O(n) 処理にする)
  - リテラル
- (今の `string` リテラルと同じ) `""` リテラルに対して Target-Typed な型判定をすべきか
  - 「UTF-8 リテラル」用の追加構文を用意しなくていいという利点はあるけども、オーバーロードで困る
      - `M(string s)` と `(M Utf8String s)` があるとき `M("literal")` の解決ができない
      - 破壊的変更にならないようにするためには、`string` 優先にせざるを得ない
  - リテラルを Target-Typed にするのはやめた方がよさそう
  - `u""` みたいな構文を足すことになる
- キーワード
   - `System.String` が `string` という C# キーワードになってるのと同様、`Utf8String` に対して `ustring` などのキーワードがあった方がいいか？
  - `u` だけだと `UTF-16` だって `Unicode` だって頭文字 u だから避けたいけど、他にもっと使いたい名前もない
  - リテラルとかで言語的な特殊対応をするんだから、キーワードを与えて primitive な地位を与えることはおかしくはない
  - まだ迷い気味

## ローカル関数への属性適用

[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)とか[非同期ストリーム](../../../../study/csharp/async/asyncstream.md)とか、ローカル関数でも使えて、かつ、属性を付けれないと困りそうな機能がすでにいくつかあります。
なので、ローカル関数への属性適用をできるようにすること自体は確定事項。

今回の懸念としては、ローカル関数は C# コンパイラーが何らかの通常のメソッドに変換するわけですが、
その変換結果から属性を取れるという保証をするかどうか。
現在のコンパイラー実装では、どこかしらには絶対属性が残るようになっているものの、どこからどうすればその属性を取れるか言語仕様で保証まではしない(という提案が今回あって、その方向で行くことに決定)とのこと。

## Target-Typed 型推論

[Target-Typed new](https://github.com/dotnet/csharplang/issues/100)は以前にもやる気になっていて、その時にデザインも終えてる。その頃から気がわかってないかだけ改めて確認。
やる気は変わらず、デザインのレビュー待ち状態に。

その他、Target-Typed な機能というと

- [`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression) (C# 8.0 で実装済み)は最初から Target-Typed な型推論を持ってる
- 「`switch` でやるのなら」ということで [?? 演算子](https://github.com/dotnet/csharplang/issues/2473)、[?: 演算子] (https://github.com/dotnet/csharplang/issues/2460)でも検討中
- それとは別(ただし同じ課題に対する解決策)に、「[Common Type 推論の改善](https://github.com/dotnet/csharplang/issues/33)」がある

Target-Typed 型推論と Common Type 推論の改善はどちらも、
`flag ? 1 : null` みたいな式の型決定(`int?` になってほしい。今はコンパイル エラーを起こす)に対する解決策ですが、
優先度を決めないと競合します。
`M(int?)` と `M(short?)` みたいなのがあるときに、`M(flag ? 1 : null)` みたいなのがどちらのオーバーロードを呼ぶべきかが変わります。

Target-Typed 型推論の方が「ターゲットさえはっきりしていればいろんな型に対応できる」という利点があるものの、
「ターゲットがはっきりしない」ということも多いので Common Type 推論の方が使える場面は多くなります。

破壊的変更になることを避けるために、C# 8.0 の新機能の `switch` 式にだけ Target-Typed 型推論が入りました。
一方でこれから Common Type 推論も入れたいわけですが、`switch` 式とその他で違う挙動にせざるを得ないかもしれません。

Target-Typed 型推論に対する破壊的変更にならないように Common Type 推論を実現できないかは検討はしたい。
もし `switch` 式に対して破壊的変更が起きそうなら、その影響範囲は調査しておきたい(場合によっては破壊的変更を認めるかも)とのこと。

## その他のトリアージ

- [Null parameter checking](https://github.com/dotnet/csharplang/issues/2145)
  - すぐにでも。9.0 に入れる
- 修飾子の順序緩和。特に、[ref と partial の順序](https://github.com/dotnet/csharplang/issues/946)
  - 9.0
- [0要素、1要素のタプル](https://github.com/dotnet/csharplang/issues/883)
  - 決めかねてる。 X.X (いつになるかわからない)行き
- [分解](../../../../study/csharp/datatype/deconstruction.md)での[変数と宣言の混在](https://github.com/dotnet/csharplang/issues/125)
  - あると良いことはわかっているけど急ぎではない
  - Any Time (いつでもいい)行き
- [ラムダ式の引数での discard](https://github.com/dotnet/csharplang/issues/111)
  - Any Time
  - といいつつ、[実装始まってたりする](https://github.com/dotnet/roslyn/pull/38786)。おそらく、「やってみて案外低コストだったら 9.0 入りを考えてもいい」みたいな状態かと
