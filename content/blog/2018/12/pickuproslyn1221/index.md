---
title: "ピックアップRoslyn 12/21 & Connect() Japan フォローアップ"
source_url: "https://ufcpp.net/blog/2018/12/pickuproslyn1221/"
content_type: "BlogEntry"
published_at: "2018-12-21T15:22:53"
updated_at: "2018-12-21T15:22:53"
tags: []
umbraco_id: 2203
parent_id: 2177
sort_order: 20
aliases: []
---

# ピックアップRoslyn 12/21 & Connect() Japan フォローアップ

昨日、[Connect(); Japan 2018](https://connpass.com/event/111254/)でちょっとだけですけども、C# 8.0の話をしたりしました。
7分(ちょっと超過したけど)だとあんまり大したことを話せず…

とりあえず、昨日やったデモは、1機能1コミットでプルリクを作って GitHub においてあるのでそちらも参照してみてください。

- [C# 8.0 デモ用](https://github.com/ufcpp/connect-japan-2018/pull/2) … 昨日やれたデモ。Preview 1 で入った機能の紹介。
- [UfcppSample](https://github.com/ufcpp/connect-japan-2018/pull/1) … [C# によるプログラミング入門](../../../../study/csharp/index.md)で書いてるコードに対して NullableReferenceTypes true にするとどうなるか、どう書き換えるかというデモ。時間なかった。

で、今日は、[Visual Studio 2019 Preview 1](../vs2019p1/index.md)のその後のピックアップRoslynでパターン マッチングがらみの話が1件と、
機能やれなかった[UfcppSample](https://github.com/ufcpp/connect-japan-2018/pull/1)デモのフォローアップ。

## パターン マッチング

v2。

- [Open LDM Issues in Pattern-Matching (v2)](https://github.com/dotnet/csharplang/issues/2095)

[元々あった issue](https://github.com/dotnet/csharplang/issues/1054)が長大になりすぎたので、今残ってる作業だけを抜き出して新しくissueを立てた模様。

[今月1回書いてますけども](../cs8patterns/index.md)、
パターン マッチングは Preview 1 に入ると思ってたけど入ってなかったって感じなんですが。
上記 issue はその現状で残ってる課題の一覧。

- switch 式を、void も認めて、「式ステートメント」も認めたい
  - `void M1()`、`void M2()` に対して、`x switch { 1 => M1(), 2 => M2() };` みたいなのを認めたい
- switch 式、末尾 `,` を認めたい
  - 今の実装だと `x switch { 1 => M1(), 2 => M2(), }` (`M2()`の後ろの`,`) を書くとエラー
- 0, 1要素分解を認めたい
  - `if (o is (3) _)` みたいなの
  - キャスト+定数パターン `o is (int)0` みたいなのとの弁別で悩み中
- 名前付き引数でのオーバーロード解決を認めるかどうか
  - `Deconstruct(int X, int Y)`と`Deconstruct(double Angle, double Length)`があるとき、`p is (X: 3, Y: 4)`で前者を呼べるようにするかどうか
- プロパティ パターンで、インデクサーとかイベントとかを認めるか
- ref構造体のトラッキングがバグってる
  - 今、パターン マッチングを使うと、本来返せないはずの `Span<T>` を返せちゃうバグあり
- `ITuple`インターフェイス越しの分解と、`Deconstruct`メソッド越しの分解の優先度をどうするか

## UfcppSample に対して NullableReferenceTypes true

[null許容参照型](../cs8nrt/index.md)は待望の機能なわけですが、
1つ懸念としては、既存コードに対して適用するとどうなるかでしょう。
一応は、既存コードを壊さないようにopt-in (明示的にオプション指定しないと有効にならない)になっているわけですが、
「問答無用に全体に opt-in してしまうとどうなるか」は気になるところだと思います。

ということで、昨日は、時間が許せば[C# によるプログラミング入門](../../../../study/csharp/index.md)で書いてるコードに対して opt-in してみる話もしたかったんですが。
特に、うちのサイトは結構 C# 1.0 とか 2.0 の頃からある古いコードも残っていますし。
それに対して opt-in してみようと。

まあ、時間的に無理だったのでここで改めて。

## 普通な範囲

大半は、「意図して null を受け付けているところにちまちまと `?` を付けていく作業になります。

- [割と納得いく範囲で直せるやつ](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/bc8ea6661a38fde76fcc4930719cb693786f3c63)

これで、51件あった警告が、28件減って23件に。

## ジェネリクス

Preview 1の実装では、結構ジェネリクス周りの実装が抜けています。
これに関しては、最近、Roslyn 上で generics がどうこうみたいなプルリクをよく見かけるので、Preview 2までにはだいぶ改善するかもしれません。

とりあえず、今はあきらめて(Preview 2で良くなることを祈って)、無視します。

- [ジェネリクスがらみはdefault!でごまかすしかなさそう](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/5de947a08873d0c420f48dbaacc781e24efe904d)

基本的に、後置き`!`演算子を付けると、forgiving (警告もみ消しを容赦してもらう)になります。
ジェネリクスがらみにはこいつを使って対処。

## ローカル関数に変更 

ラムダ式に対して再帰したり、自分自身を参照したりするとき、以下のように、デリゲートをいったん空初期化した上で改めてラムダ式を代入する必要があります。

```csharp
Func<int, int> f = null;
f = x => x <= 1 ? 1 : f(x - 1);
```

この、最初の `= null` がよくない。

で、これは単に、ラムダ式をローカル関数に書き換えるだけで解消します。

- [そもそもラムダ式の限界。ローカル関数に変える](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/0ea3ca59b868c36fca1a4ea7d1b08d5fa1082de6)

## あと片付け

[ガベコレ](../../../../study/computer/essential-software/memorymanagement.md)で少しでも早く不要メモリを回収してもらうために、もう要らない変数に null を代入することもあったりします。

これに関しては、

- 要らなくなる(Dispose する)までは絶対に null にならないので、`T` で使いたい
- 要らなくなった後だけのために `T?` に変えるのはちょっと嫌

という感じ…

ちょっと迷ったんですが、結局は `!` に頼ることにしました。

- [こういう後処理のためのnull代入はnull!でいい気がする](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/ad94f956e75f74da18d7e220e9c4e253aedfc981)

## バグ

まあ、バグっててどうしようもない奴は `#pragma warning disable` で黙殺。

- [バグってて対処不能なやつ](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/1fa623ba4494ace9e6d7fb5a29ce09e614ad8421)

バグ報告済みなので、Preview 2までに治ってるといいなぁ…

ちなみに、このバグは Visual Studio 自体を落とします。

```csharp
static class Ex
{
    // こういう、カリー化デリゲート(拡張メソッドを使ったデリゲート構築)に対する null 検証がバグってる。
    // 非 null なインスタンスを渡していても、なぜか null 警告が出る。
    // バグを黙殺するために ! を付けようとすると Visual Studio が落ちる。
    public static Action a = new object().M;
    public static void M(this object x) { }
}
```

## デモ都合

[C# によるプログラミング入門](../../../../study/csharp/index.md)内には、「null がダメなのは百も承知で、もしそれでも null を渡してしまったらどうなるか」を示すデモがいくつかあります。

百も承知でわざとやってるんだからうるせー(おもむろに `#pragma warning disable`)。

- [デモ都合で無視したい奴](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/5b021ecdb380d1f721ee8aec2ff69e09037cd65d)

## どうしていいのかわからなかった奴…

で、6件ほど、ほんとにどう対処すべきなのかわからなくてとりあえず `!` とか `#pragma warning disable` とかでやっつけたのが6件ほど。

- [デリゲートの += はどう対処すべきなんだろう](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/06138cf31e78b7cc701b8c5faa32357193213a27)
- [Rx はほんとどうしたらいいのかわからない](https://github.com/ufcpp/connect-japan-2018/pull/1/commits/2047730635b94cf3551141c818e6aa1dde1080ec)

デリゲートがらみはほんとに鬼門かも…
