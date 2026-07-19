---
title: "ピックアップRoslyn 8/29"
source_url: "https://ufcpp.net/blog/2015/08/pickuproslyn0829/"
content_type: "BlogEntry"
published_at: "2015-08-29T03:33:11"
updated_at: "2015-08-29T03:33:11"
tags: []
umbraco_id: 1785
parent_id: 1775
sort_order: 4
aliases: []
---

# ピックアップRoslyn 8/29

## Anonymous function that captures no outer variables is not static #4793

[https://github.com/dotnet/roslyn/issues/4793](https://github.com/dotnet/roslyn/issues/4793)

[匿名関数](../../../../study/csharp/functional/sp_delegate.md#anonymous-func) (ラムダ式とか匿名デリゲート式)の生成結果が変わったという話。

- 旧コンパイラー: 外部のローカル変数をキャプチャした時だけインスタン スメソッドに、キャプチャしていなければ静的メソッドになってた
- Roslyn: 無条件にインスタンス メソッドになる

理由は、性能測定してみたらそっちの方が早かったかららしい。
インスタンス メソッドの方が `this` ポインター分、余計なものを渡してるのでこっちの方が遅そうなのに。
実際には、分岐する方が高コストなのか、CLRレベルでの最適化のせいなのか、インスタンス メソッドの方がよいとのこと
(StackOverflow 上での説明によれば、ネイティブ コード化するときの呼び出し規約の決定のコストがどうとか書いてある)。

## Discussion: Scope of pattern variables, and tuple decomposition #4781

[https://github.com/dotnet/roslyn/issues/4781](https://github.com/dotnet/roslyn/issues/4781)

現状、パターンマッチングは `x is pattern` という構文で bool 値を返す想定で作っているものの、

- コンパイル時に成功が確定するはずで、コンパイル時に成功していることをチェックしてほしい場合がある
- その時に `if (x is pattern) { }` とか書きたくない
- pattern 中の分解して得られる変数のスコープをどうするかで困る

という場面がある。例えばタプルの分解で、いちいち

<pre><code>
var tuple = (12, "foo")
tuple is (int x, string y) ? (x, y を使った式) : (来ないはず);
</code></pre>

とか書くのも何かおかしい。

ということで、コンパイル時チェック付きの別構文、例えば、

<pre><code>
var tuple = (12, "foo")
(int x, string y) := tuple;
// 以後、x, y を使える
</code></pre>

とかいうのを導入するのはどうだろうか、というディスカッションの場。

コンパイル時にチェックできるのって実質タプルの分解くらいかもしれず、タプル分解用の構文としてはなんかいまいちに思えるあたりが悩ましい感じ。

## Proposal: An await operator similar to ?. #4714

[https://github.com/dotnet/roslyn/issues/4714](https://github.com/dotnet/roslyn/issues/4714)

`await null;` がヌルポになるの何とかならない？ メソッド呼び出しに `?.` (null 条件演算子)が導入されたんだし、null 条件 await できない？という提案。

むっちゃほしい。

というか最近何個か、

<pre><code>
var t = XAsync();
if (t != null) await t;
</code></pre>

とかいう嫌なコード書いたところ…

## Proposal: A bottom type for C# #4843

[https://github.com/dotnet/roslyn/issues/4843](https://github.com/dotnet/roslyn/issues/4843)

型理論でいうところの bottom type、Haskelでいう`undefined`、Scalaでいう`Nothing`が欲しいとのことで、そこそこ議論が起きた後で、「それ、もうあるよ [#1226](https://github.com/dotnet/roslyn/issues/4843)」(今のところ Never 型として提案中)。

「例外を出すから絶対に戻り値返さないよ」型。こういう型があれば、

- エラー チェックして例外を throw するだけのメソッドを作りやすい/呼びやすい
- `Never X() => throw new Exception()` みたいに、ラムダ形式のメソッドで例外投げれる
- `Never` は任意の型に変換可能で、`TryParse(s, out var x) ? x : throw new Exctption()` みたいな、他の型との混在ができる

というもの。

bottom, undefined, nothing, never… まあ、そりゃ検索できなくて重複提案出ますわ。
