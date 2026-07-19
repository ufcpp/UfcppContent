---
title: "ピックアップRoslyn 7/30: C# 7.1の先の計画＆ .NET でのメモリ手動管理"
source_url: "https://ufcpp.net/blog/2017/7/pickuproslyn0730/"
content_type: "BlogEntry"
published_at: "2017-07-30T14:48:12"
updated_at: "2017-07-30T14:48:12"
tags: []
umbraco_id: 2076
parent_id: 2075
sort_order: 0
aliases: []
---

# ピックアップRoslyn 7/30: C# 7.1の先の計画＆ .NET でのメモリ手動管理

## C# 7.1の先の計画

C# 7.1 がらみの作業も落ち着いたところで、
その先の優先度付け的な Design Notes がアップロードされていました。

- [C# Language Design Notes for Jul 5, 2017](https://github.com/dotnet/csharplang/blob/master/meetings/2017/LDM-2017-07-05.md)

概ね以下のような感じ。

- 構造体の活用(特に ref がらみ)は C# 7.2 でまとめて実装
- [Utf8String](http://www.buildinsider.net/column/iwanaga-nobuyuki/007) は C# 8.0 に回りそう
- [static delegate](https://github.com/dotnet/csharplang/blob/master/proposals/static-delegates.md) とかいう話が出てる(C# 8.0向け)
  - static なメソッドは関数ポインターで直接管理したいという話
  - static なメソッドしか参照しないデリゲートのために `Delegate` 型インスタンス(ヒープを使う)を作りたくない
- その他、バグっぽい挙動に関して
  - 自動実装プロパティで、バッキングフィールドに対して属性を適用できなかった問題(いつでも修正する用意あり)
  - long tuple (`System.ValueTuple`型の arity を超える8要素以上のタプル)に対するオーバーロード解決が怪しい問題(今のままにしたい)

## .NET でのメモリ手動管理

これは .NET チームとか C# チームでの動きじゃなくて、マイクロソフト リサーチによる論文なんですが、
以下のようなものが出ています。

- [Project Snowflake: Non-blocking Safe Manual Memory
Management in .NET](https://www.microsoft.com/en-us/research/wp-content/uploads/2017/07/snowflake-extended.pdf)

用途を絞らない汎用のメモリ利用に関してはガベージ コレクション(GC)の生産性を下げてまで手動管理を頑張ってもそれほど効果はないんですが、
一方で、手動管理が割かししやすくて、かつ、手動管理できれば数倍程度の高速化が見込めるような状況もあります。
そこで上記論文は  .NET における手動メモリ管理に関する研究なんですが、以下のような内容。

- GC と共存
  - 手動管理下にあるオブジェクトが、GC 管理下のオブジェクトの参照を持てる
  - GC に追加の負担を掛けない
- マルチスレッド環境でも高速
  - lock なしでスレッド安全な手動メモリ管理を行う手法の導入
  - メモリの「owner」(メモリ解放の責任を負っていているただ1つのオブジェクト)と「shield」(一時的にメモリ参照権限を借り受けるオブジェクト)を用意して管理
- (C# とかの)プログラミング言語との連携
  - 言語レベルで変数の「コピー不能性」を保証してもらえれば、低負担なメモリ管理アルゴリズムが使える

メモリ管理のアルゴリズムについては参考文献をいろいろ引用していて、
Rust とかでもやっている手法っぽい感じ。
GC との共存ができるという点と、実際に [CoreCLR](https://github.com/dotnet/coreclr)をベースに実装してみて、
計測してみたというあたりがこの論文のポイントになりそう。

owner (所有者。解放責任者)がはっきりしないとこの手法は使えないんで、用途は限られるんですが、
例えば以下のような用途に使えます。

- `List<T>` とかの内部で要素の保存に使っている配列
- キャッシュ管理
- LINQ の `GroupBy` とかの中で使う一時的なハッシュテーブル

はまる用途に関しては2倍とか3倍とかの性能改善が期待できるという計測結果も出ています。

まあ、あくまでリサーチの段階なので、
実際に .NET に取り込まれるとしても結構先になる感じはあります。
