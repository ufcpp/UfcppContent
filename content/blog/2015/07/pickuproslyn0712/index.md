---
title: "ピックアップRoslyn 7/12"
source_url: "https://ufcpp.net/blog/2015/07/pickuproslyn0712/"
content_type: "BlogEntry"
published_at: "2015-07-12T05:55:05"
updated_at: "2015-07-12T06:13:32"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1769
parent_id: 1756
sort_order: 1
aliases: []
---

# ピックアップRoslyn 7/12

Mads、怒涛のClose祭り。

久しぶりに、C# Design Notesが投稿されてました。というか、5月のNotesが今投稿されているなど。
要するに、Visual Studio 2015関連作業がやっと落ち着いたってことなんですかね、きっと。

## C# Language Design Review, Apr 22, 2015 #3910

[https://github.com/dotnet/roslyn/issues/3910](https://github.com/dotnet/roslyn/issues/3910)

### 式ツリー

式ツリー化できる構文とできない構文の差が大きくなりすぎてる。
全部考えると非常に労力がかかって、その努力に見合うと思えない。

ただ、`?.` と string interpolation に関しては、新しいノードを導入しなくてもできそう。それくらいは考えてもよさそう。

### nullable 参照型

互換性問題があるので、以下の2つのアプローチが考えられる。

- two-type: 既存の`T`の意味を変えて、これを非nullにする。nullableは`T?`
- three-type: 既存コードを壊さないように`T`は残す。非nullが`T!`、nullableが`T?`

もちろん互換性の問題をクリアできるんなら two-type が理想的。nullチェックに対して、何らかのopt-in (コンパイラーの規定動作じゃなくて拡張にするとか、アセンブリ レベルの属性で`T`の意味を新旧切り替えるとか)を提供する必要がある。

### Wire format

Wire format(JSONとか、要するに通信用に使う形式)問題。どうやったらJSON化とかの定義を楽にできるか、特に、シリアライズのレイヤーを緩い型付けのままどうやって強い型付けのロジックとつなぎこむか。

考えうるやり方は以下のようなもの。

- 実行時に、辞書式のデータとインターフェイスとの相互変換する機能の類を提供
- TypeScript みたいな「コンパイル時のみの型」(コンパイル結果からは型情報を消して辞書式アクセスに置き換え)
- F# みたいな「型プロバイダー」の提供

## C# Design Notes for May 20, 2015 #3911

[https://github.com/dotnet/roslyn/issues/3911](https://github.com/dotnet/roslyn/issues/3911)

ローカル関数、つまり、関数(メソッドとか演算子とか)の中で別の関数を定義したいという話。

まあたまにある、「メソッド X の中で、X からしか使わないメソッドを作りたい」という要件。これに対して、現状は無理で、代わりに XInternal みたいなメソッドを用意したり。あるいは、ラムダ式で代用するわけですが、ラムダ式はいくつかの制限が面倒なので改善したいと。

ラムダ式を拡張するか、ローカル関数用の構文を追加するかは検討中な物の、現状は後者、つまりローカル関数の追加で話が進んでいそう。というか、Design Notes 公開前に、[pull-req とかブランチが見えて](../../6/pickuproslyn0610/index.md)るんですが。

ちなみに、現状のラムダ式との違いは、

- ジェネリックを使える
- 再帰呼び出しもできる
- イテレーターにもできる(yield return 書ける)
- デリゲートの型を明示する面倒がいらない(ラムダ式は `Func<int>` みたいなやつの明示が必須)
- というかデリゲートを介さない(オーバーヘッドが減る)

という感じ。

紹介されている用途の例:

- イテレーターに引数チェックを追加
- クイックソートとかの再帰アルゴリズムの実装
- キャッシュ(2度目以降の呼び出しは辞書に格納した値を返す)
- 特に、`Task`のキャッシュ

## C# Design Notes for May 25, 2015 #3912

[https://github.com/dotnet/roslyn/issues/3912](https://github.com/dotnet/roslyn/issues/3912)

いくつか、提案 issue に対してやるかどうかの判定。これに伴って、いくつかの issue が閉じられました。

- [XML リテラル](https://github.com/dotnet/roslyn/issues/1746): ない。特定のデータ形式をC#に組み込むことはない。
- [ループの最初の1回だけ特別扱いする文法](https://github.com/dotnet/roslyn/issues/131): ない。そういうシナリオは確かにあるけども、専用構文を追加するほどの価値は見いだせない。
- [ラムダ式に対する型推論](https://github.com/dotnet/roslyn/issues/14): やらないけども、ローカル関数が代わりになる。
- [さらなる型推論](https://github.com/dotnet/roslyn/issues/17): ない。ここまで急進的な型推論をC#に入れたくはない。
- [多値戻り値](https://github.com/dotnet/roslyn/issues/102): タプル構文の一部になるはず。

その他、いくつか簡単な検討あり。

- `params` IEnumerable: 性能的に、単に配列使うんじゃなく、もっといい実装を検討したほうがよさそう。
- immutable な型に対する初期化子: 「wither」と一緒に検討したい。
- `vararg` 呼び出し: `params` IEnumerable と一緒に検討したい。

## C# Design Notes for Jul 1, 2015 #3913

[https://github.com/dotnet/roslyn/issues/3913](https://github.com/dotnet/roslyn/issues/3913)

Tuple がらみ、その後の状況(まだこれが最終決定じゃなく、いったんコミュニティからの感想受け付け用)。

### 名前関連

まず名前関連。あまり強く型付けしすぎないというのと、「C#のTupleはメソッド引数の延長」という発想の元、

- 名前なし(匿名)Tuple作れるようにする
- 同じ位置に同じ型が来ているTupleは、名前が違っても同じTuple型とみなす
  - ただし、`(first: "John", doe: "Doe")`みたいな、名前を明示する場合は順序の入れ替えあり
  - 名前違い同型のTupleで配列を作ると、型推論結果は匿名Tupleの配列

とかに。

### エンコーディング

具体的な実装と、Tupleに対応していない他の言語・古いC#との繋ぎ。

たぶん、

<pre><code>
public struct ValueTuple<T1, T2, T3>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
}
</code></pre>

みたいな型と属性で実現することになるはず。他の言語・古いC#からは`Items1`とかでメンバーアクセスすることになるけども、C# 7.0からはどう見えるべきか。混乱を避けるには`Items1`でのアクセスは隠すべきだろう。

### 分解(deconstruction)

↓こういう分解用の構文を用意すべきかどうか。

<pre><code>
(int sum, int count) = Tally(...);   
</code></pre>

用意するならいくつか検討すべき疑問が残るものの、今のところその疑問を避けるべく、分解用の構文追加はしないでいる。

### その他の課題

今のところ arity 0、1のTuple は考えていない。これらがあると便利だと思う(特に、コード生成の観点で)。でも、おそらく多くの疑問も起こすことになる。

その他いくつか、型変換(例えば、covariantな変換とか)も考えるに値するけども、今のところその検討は置いておく。
