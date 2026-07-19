---
title: "ピックアップRoslyn 10/9: base(T), UTF-8 String, Discard parameters"
source_url: "https://ufcpp.net/blog/2019/10/pickuproslyn1029/"
content_type: "BlogEntry"
published_at: "2019-10-29T21:42:13"
updated_at: "2019-10-29T21:42:13"
tags: []
umbraco_id: 2272
parent_id: 2268
sort_order: 3
aliases: []
---

# ピックアップRoslyn 10/9: base(T), UTF-8 String, Discard parameters

数日前、いくつかの新機能について、仕様書のドラフト案が上がっていました。

- [base(T) - Draft Specification #2910](https://github.com/dotnet/csharplang/issues/2910)
- [UTF8 String Literals - Draft Specification #2911](https://github.com/dotnet/csharplang/issues/2911)

どちらも、これまであった Design Meeting の議事録通りな感じ。

あと、ちょこっと変更が検討されて、結局元さやに納まったものが1件。

- [Champion "Lambda discard parameters" #111](https://github.com/dotnet/csharplang/issues/111)

## base(T)

- [base(T) - Draft Specification #2910](https://github.com/dotnet/csharplang/issues/2910)

これは、C# によるプログラミング入門に説明を書いた直後に「やっぱり C# 8.0 ではやめておく」となってしまったやつ。
(しょうがないんで「[C# 8.0 から外れました](../../../../study/csharp/oop/oo_inherit.md#non-virtual-base-access)」って書き足してそのまま残してあったり。)

まあ、.NET ランタイムのレベルで対応してもらう予定だそうです。
`base(T).M()` と書いたとき、`T` 自体に `M` の実装がなくても基底クラスをたどって最初に見つかった `M` を呼んでもらえるという仕様。

## UTF-8 String Literals

- [UTF8 String Literals - Draft Specification #2911](https://github.com/dotnet/csharplang/issues/2911)

待望の。

といっても、`Utf8String` 自体についてはまだいくつか悩ましいポイントがあり…

- `string` (今は UTF-16)自体を UTF-8 に切り替えるオプションがやっぱりほしい
  - とはいえ、インデクサー(UTF-16 での i 文字目を `s[i]` で定数時間アクセスできる)を期待しているコードが壊れる
  - unsafe に `fixed (char* p = s)` しているコードも壊れる
- `System.String` に対して C# キーワードの `string` があるけど `Utf8String` に対してはどうするべきか
  - キーワード足さない？
  - `ustring`？
  - `utf8`？
- クラス名自体まだ悩ましい
  - 今回のドラフト案も冒頭に「corert 側が今のところその名前になってるのでそれに従う」との注釈がまだ必要な段階

というのもあって、C# vNext で検討されているのは本当に「手始め」という感じのものだけ。

- (今ある) [文字列リテラル](../../../../study/csharp/start/st_embeddedtype.md#charl)をそのまま使う
- 以下のようなルールだけ C# に追加
  - 文字列リテラルから `Utf8String` への暗黙の型変換を認める
  - `Utf8String` は `const` にできて、それに渡せるのは `const` な文字列のみ
  - `+` 演算子は `Utf8String.Concat` として解釈する
- コンパイル結果としても `string` (UTF-16) のままプログラムにデータを埋め込む
  - 読み込み時に .NET ランタイム組み込みのヘルパー関数を呼んで UTF-8 に変換する
  - 将来的には直接 UTF-8 なデータをプログラムに埋め込めるように改修する可能性はあり

まあ、`Utf8String` クラスが標準ライブラリに入ってくれるだけでも随分助かりはするんですが…
既存のコードベースが `string` だらけなのがだいぶやっぱりネックになりそうな感じ。

## Discard parameters

- [Champion "Lambda discard parameters" #111](https://github.com/dotnet/csharplang/issues/111)

この issue の趣旨自体は、`Action<T, T> a = (_. _) => { };` みたいな書き方を認めたいというもの。
2個以上の引数が `_` の時、それは[discard](../../../../study/csharp/cheatsheet/ap_ver7.md#discard)扱いにします。
ラムダ式の中で `_` を普通の変数のように触ろうとするとコンパイル エラー。
一方で、1引数の `_` は今現在有効は引数名として使えてしまっているので、破壊的変更を避けるために今のまま。

で、ここ数日で、一瞬、「ラムダ式以外、普通のメソッドの引数やローカル関数にも適用してもいいんじゃないか」というのが議題に上がりました。
でも、[名前付き引数](../../../../study/csharp/structured/sp4_optional.md#named)がある以上、引数の名前は API の一部分であり、メソッドの外から見えてしまう情報になります。
そこを省略するのはあんまりお行儀がよくない。

なので結局、当初予定通り[匿名関数](../../../../study/csharp/functional/fun_localfunctions.md#anonymous-function)(ラムダ式と匿名メソッド式)でだけ、`_` のdiscard扱いをしたいという結論に。
