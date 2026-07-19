---
title: "ピックアップRoslyn 5/12"
source_url: "https://ufcpp.net/blog/2016/5/pickuproslyn0512/"
content_type: "BlogEntry"
published_at: "2016-05-11T22:54:29"
updated_at: "2016-05-11T22:54:29"
tags: []
umbraco_id: 1898
parent_id: 1890
sort_order: 1
aliases: []
---

# ピックアップRoslyn 5/12

## なんでも拡張

- [Language Feature: Extension Everything #11159](https://github.com/dotnet/roslyn/issues/11159)

拡張メソッドは便利な構文なわけですが、インスタンス メソッドしか拡張できないのが残念なところです。拡張プロパティとかも作りたいことがあるし、静的な拡張(静的メソッドも既存のクラスに対して追加したように見える構文)もほしかったりします。という、なんでも拡張できる構文の案。

今のところ、以下のような構文で検討中。

<pre class="source" title="">
<code><span class="reserved">extension class</span> <span class="type">クラス名</span> : <span class="type">拡張したい型</span>
{
    <span class="comment">// ここにメンバーは、拡張したい型の拡張メソッド、拡張プロパティなどになる</span>
}
</code></pre>

## C# Design Notes (タプル型関連 再び)

- [C# Design Notes for May 3-4, 2016 #11205](https://github.com/dotnet/roslyn/issues/11205)

[前回](../pickuproslyn0504/index.md)の[C# Design Notes for Apr 12-22, 2016](https://github.com/dotnet/roslyn/issues/11031)に引き続き、タプル型関連。

- 分解
  - `(x, y) = tuple;` みたいなのを、`tuple.Deconstruct(out x, out y);` みたいなメソッド呼び出しとして解釈したい
  - タプル型専用の特別な構文にしないために、インスタンス or 拡張メソッドを通したい
  - ちょっと前まで`GetValue`って名前を検討してたけど、一般的過ぎてすでに使われてそうなので、`Deconstruct`メソッドにする
  - 要素の値を返すのはout引数にする。タプル型を分解するのにタプル型を返すわけにもいかないので
- switchステートメントでの変換
  - 既存のswitchだとintとかstringしか受け付けないので、intとかへの暗黙的型変換できる型は変換結果で解釈されてた
  - 破壊的変更にならないようにするために、暗黙的型変換を定義した型は、今後も変換結果で解釈する
  - なので、そういう型(例えばintへの暗黙的型変換を持った`Const`型)は `case Const(int i):` みたいなcaseにマッチしない(`case 0:`とかにはマッチする)
  - あんまり起きない状況だし、破壊的変更を避ける方を優先
- タプル型間の変換
  - 「要素数が一致していて、各要素が暗黙的型変換できるなら、タプル型間でも暗黙的型変換できる」ってルールにするっぽい
- タプル風のインスタンス生成
  - 任意の型に対して、左辺から型が推論できるなら、`new (x, y)`みたいな書き方でインスタンス生成できるようにする

最後の「タプル風インスタンス生成」はちょっと補足。

元々の発想は、「分解の構文を`Deconstruct`メソッドを通すことで汎用化したんだし、構築の構文も汎用化すべき」というもの。タプル構築と、コンストラクター呼び出しを紐づけたいということになります。

が、例えば、その発想で行くと、以下のように、なんか直観にそぐわない「タプル型リテラルからの構築」ができてしまう。

<pre class="source" title="">
<code><type></span><span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">string</span>&gt; d = (16, <span class="type">EqualityComparer</span>&lt;<span class="reserved">int</span>&gt;.Default); <span class="comment">// さすがにこれは気持ち悪い</span>
</code></pre>

代わりと言ってはなんだけど、以下のような、`new`の後ろの型の省略を認めようという感じになってるみたい。

<pre class="source" title="">
<code><type></span><span class="type">Point</span> p = <span class="reserved">new</span> (3, 4); <span class="comment">// new Point(3, 4) と同じ</span>
<span class="type">List</span>&lt;<span class="reserved">string</span>&gt; l1 = <span class="reserved">new</span> (10); <span class="comment">// 引数0個 or 1個でも大丈夫</span>
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; l2 = <span class="reserved">new</span> () { 3, 4, 5 }; <span class="comment">// コレクション初期化子との併用もできるけど、() は省略できない</span>
</code></pre>

要するに、結局、左辺からの型推論みたいな構文を追加することになりそう。
