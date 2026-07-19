---
title: "ピックアップRoslyn 12/1"
source_url: "https://ufcpp.net/blog/2018/12/pickuproslyn1201/"
content_type: "BlogEntry"
published_at: "2018-12-01T20:35:39"
updated_at: "2018-12-01T20:36:54"
tags: []
umbraco_id: 2178
parent_id: 2177
sort_order: 0
aliases: []
---

# ピックアップRoslyn 12/1

10月31日～11月28日当たりの Design Notes 追加。

- [Added: LDM notes for Oct. 31, Nov. 5, and Nov. 14 #2032](https://github.com/dotnet/csharplang/issues/2032)
- [Added: LDM Notes for Nov. 28, 2018 #2033](https://github.com/dotnet/csharplang/issues/2033)

ちなみにこの辺りの実装、pull request を探してみたら Milestone が [16.0.P2](https://github.com/dotnet/roslyn/milestone/36)になっているものが結構あって、近々出てくれそうな Visual Studio 16 Preview 1 ではまだ入ってなさそうなもの多めです。

## Nullable Reference Types

今回の Design Notes の半分くらいは null 許容参照型がらみ。

あと、書きかけではありますが、やっと null 許容参照型の提案ドキュメントがアップロードされたみたいです。

- [Nullable Reference Types Specification](https://github.com/dotnet/csharplang/blob/master/proposals/nullable-reference-types-specification.md)

(これまで、Design Notes に検討事項が散見されていただけで、まとまったページが全くなかったという…)

[10月31日のやつ](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-10-31.md)は、なんか問題提起だけで結論が入ってなくて、なんかふんわりとした感じ。

- 明示的な null チェック
  - 8.0 というバージョンから急に null 許容参照型を追加する都合で、`string x` は「古いコード由来だとnull許容なんだけど、8.0以降＋`#nullable enable`の時には非nullとして扱う」みたいな挙動になる
  - `string x` に対して`if (x == null)` があるとき、「明示的に null チェックがあるということは、`x` 自体は null があり得る」ということなので、`string` (`?` が付かない)であっても null 許容扱いする
  - みたいなの、やるべきかどうか。TypeScript はやってる
- リファクタリング
  - null かどうかのフロー解析は、リファクタリングの影響を受けるという話
  - 例えば、`if` の中身だけを「メソッド抽出」リファクタリングすると、`if` の条件式内でやった null チェックの情報を失って(メソッドをまたいだ解析はしないので)、警告が出る
- `!` 演算子
  - 場所によって意味変わりすぎ… (前に置くと否定、後ろに置くて「null forgiving」(null チェックを意図的にやらない)アノテーション)
- ラムダ式中での代入
  - ラムダ式でキャプチャした変数までフロー解析するべきかどうか

[11月5日の](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-11-05.md):

- `#nullable` の影響する範囲はどこまでか
  - 例えば以下のようなコードを書いた場合、disable になるのは第2型引数の`string`と、`Dictionary` 自体(`Dictionary`のシグネチャの一部分である`>`を範囲に含んでいるので)

<pre class="source">
<code><span class="type">Dictionary</span>&lt;<span class="reserved">string</span>,
<span class="inactive">#nullable</span> disable
    <span class="reserved">string</span>&gt;
<span class="inactive">#</span> <span class="inactive">nullable</span> enable
</code></pre>

- null 許容参照型
  - `using`ディレクティブ中では認めない？ → そのつもり
  - `typeof`、`nameof`は？ → 値型の nullable と同じ挙動にしたい。なので、`typeof`では認めて、`nameof`では認めない

[11月14日の](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-11-14.md):

- (同じく C# 8.0 で入る) `switch`式の網羅性(exhaustiveness)とnull
  - 「非 null 参照型だから null は来ないはず」判定のときに、実際には(フロー解析漏れ/C# 7.X 以前のコード由来のせいで) null が来た時どうするか？
  - → `MatchFailureException`例外を投げることにする。`MatchFailureException`の基底は`InvalidOperationException` にする

[11月28日の](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-11-28.md):

- 多重配列問題
  - `string[]?[]`と`string[][]?`、「null 許容配列の非 null 配列」と「非 null 配列の null 許容配列」どっちがどっち？
  - → 現状、`string[]?[]` の方が「非 null 配列の null 許容配列」
  - 多重配列の順序は元々ややこしい…

## パターン マッチング

- `x is (a, b)` みたいなパターン
  - x がタプル(`ValueTupe<T>`構造体等)なら普通にタプル扱いで分解(要素ごとにマッチング)
  - `Deconstruct`メソッドがあれば(拡張メソッドでの実装を含めて)それを使って分解
  - `ITuple` インターフェイスを実装していたら、そのインデクサーとかを使って分解
  - (優先度は上から順。`ITuple`インターフェイスを実装していて、かつ、`Deconstrcut` メソッドも持っていたら`Deconstruct`の方優先)
- 0, 1 要素
  - `x is ()` 認めてくれるらしい(0引数の `Deconstruct` 呼び出し)
  - `x is (1)` とかも認めてくれるらしい(1引数の`Decontruct`呼び出し)
  - これは、パターン マッチング(`is`と`switch`-`case`)の時だけ働く。既存の分解代入・分解変数宣言では、キャストとかとの弁別ができないので残念ながら認められない

## インターフェイスのデフォルト実装

C# では、`base.Member` みたいな書き方で、基底クラスのメンバーを呼べます。
(override していても、この書き方なら基底クラス側の実装が呼ばれる。)

で、C# 8.0でインターフェイスのデフォルト実装が入ることで、
「メソッドの実装の多重継承」ができるようになってしまい、
`base.Member`だけだと「どっちの基底だよ」と不明瞭に。

そこで、「どの基底インターフェイスのメンバーか」を指定するための構文が必要になります。
候補はたくさん並んでいますが、採用するのは `base(BaseInterface).Member` みたいな書き方にしたいそうです。

## 非同期ストリームのキャンセル

非同期ストリーム(`IAsyncEnumerable<T>`インターフェイス、非同期 `foreach`、`yield`と`await`の混在)に `CancellationToken` を渡せるようにするという話。
元々「できないとまずいよね」という検討はされてるんですが、やっと「どうするか」まで決めたみたいです。

まず、インターフェイス。`GetAsyncEnumerator`の引数に`CancellationToken`を足すみたいです。

<pre class="source" title="">
<code><span class="reserved">interface</span> <span class="type">IAsyncEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">IAsyncEnumerator</span>&lt;<span class="type">T</span>&gt; GetAsyncEnumerator(<span class="type">CancellationToken</span> token = <span class="reserved">default</span>);
</code></pre>

あと、拡張メソッドで `IAsyncEnumerable<T> WithCancellationToken<T>(this IAsyncEnumerable<T> e, CancellationToken token)` も用意。

非同期 `foreach` と`yield`と`await`の混在については、
細かい単位で`CancellationToken`を渡せるような構文を用意するかどうかという検討はしたものの、
とりあえず現状はそういう特別なことはしないという結論に。
