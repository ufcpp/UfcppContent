---
title: "null かどうかを判定"
source_url: "https://ufcpp.net/blog/2019/1/fasternullchecks/"
content_type: "BlogEntry"
published_at: "2019-01-04T19:54:25"
updated_at: "2019-01-04T19:54:25"
tags: []
umbraco_id: 2217
parent_id: 2216
sort_order: 0
aliases: []
---

# null かどうかを判定

C# では、無効な値として [null](../../../../study/csharp/resource/rm_nullusage.md) をよく使うので、
「値が有効かどうか」「無効だったら何もしない」みたいな判定のために null チェックを結構書きます。
で、C# の null は「全ビットが0」で表されるので、通常、null チェックは非常に軽い処理(単なる 0 との比較)になります。

通常は。

問題は、`==` 演算子のオーバーロードがある場合。
その場合、`x == null` は、演算子(を表すメソッド)の呼び出しになります。
もしその `==` 演算子が[インライン展開](../../../../study/csharp/structured/miscinlining.md)できないものだった場合、「単なる0比較」と比べると大幅に遅いコードになります。

なので元々、`==` 演算子のオーバーロードがある場合には、`x == null` ではなくて、
`(object)x == null` や、`ReferenceEquals(x, null)` を使えという話もあったりします。
ですが、これらはちょっと長ったらしくてつらい。

一方、C# 7.0 で[パターン マッチング](../../../../study/csharp/datatype/typeswitch.md)が入って、定数パターンを使った `x is null` であれば、
たとえ演算子オーバーロードがあったとしても、必ず「単なる0比較」で null チェックが掛かります。

そこで最近、`==` 演算子をオーバーロードしちゃっているクラスに対して `x == null` しているところを、`x is null` に置き換える最適化を掛けることが多かったりします。

そして、[coreclr](https://github.com/dotnet/coreclr)内で大々的にこの最適化をやろうとした痕跡が。

- [Faster null checks #21736](https://github.com/dotnet/coreclr/pull/21736)

日付的に、ホリデーシーズンの余暇でやってみたんですかね。

ただ、あまりにも数が多過ぎてちまちま置き換える作業は結局断念したみたいです。
400ファイル、4千行とかですからね、差分。

で、別の解決策として挙がっているのがこちら。

- [Support faster null checks #21765](https://github.com/dotnet/coreclr/pull/21765)

`==` 演算子オーバーロードの実装の方に手を入れて、

- インライン展開が掛かるサイズに収める
- `x == null` (右辺が null )の時には `x is null` だけが残る構造にする

となるようにしたという修正。
`==` 演算子の実装は、具体的には概ね以下のような構造です。

<pre class="source" title="インライン展開可能、かつ、== null の最適化が掛かるように == を実装">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> ==(<span class="type">MyClass</span> left, <span class="type">MyClass</span> right)
    =&gt; (right <span class="reserved">is</span> <span class="reserved">null</span>) ? (left <span class="reserved">is</span> <span class="reserved">null</span>) : right.Equals(left);
</code></pre>

最後が `right is null` を先に判定して、後ろが `right.Equals(left)` なのがポイント。
`null == x` だと最適化は掛からない構造。
(C# ではあまり見ませんけども、「リテラルは左辺にあるべき」派の人はご注意を。)

あと、「[null じゃないのに null を自称する邪悪なクラス](../../../../study/csharp/resource/rm_nullusage.md#user-defined-null)」なんかにはこの最適化は使えないので、[そういう残念なやつ](https://docs.unity3d.com/ja/2017.4/ScriptReference/Object.html)はあきらめましょう…
