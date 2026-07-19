---
title: "C# 8.0 Ranges"
source_url: "https://ufcpp.net/blog/2018/12/cs8ranges/"
content_type: "BlogEntry"
published_at: "2018-12-07T11:44:46"
updated_at: "2018-12-07T11:49:36"
tags: []
umbraco_id: 2188
parent_id: 2177
sort_order: 6
aliases: []
---

# C# 8.0 Ranges

今日もC# 8.0の新機能の話で、今日のはすでに Visual Studio 2019 Preview 1に入っているやつです。

Ranges and Indicesと呼ばれていて、配列などに対して、
`a[^i]`で「後ろからi番目」とか、
`a[i..j]`で「i番目からj番目の範囲」とかを取り出せるようにする機能です。

正確にいうと、`^i`とか`i..j`とかの部分がC#の新機能で、
これらはそれぞれ`Index`型、`Range`型になります。
`Index`、`Range`を受け取るインデクサーやメソッドはライブラリ側の機能です。
(ただし、配列だけは言語レベルで処理している模様。)

## 背景1: 統一ルールが欲しい

一旦先ほどの説明は忘れてまっさらな状態で、
例えば「3..5」と言われると何を思い浮かべるでしょう。
文脈次第だとは思いますが、以下のようなものがあり得ます。

- 3, 4, 5 (5も含む)
- 3, 4 (5は含まない)
- 3, 4, 5, 6, 7 (3から初めて5つ)

どれがいいかは用途次第で、実際、どれもあり得ます。
例えば、.NET でも、以下のようなメソッドがあります。

<pre class="source" title="">
<code><span class="reserved">var</span> r = <span class="reserved">new</span> <span class="type">Random</span>();
<span class="reserved">var</span> a = <span class="reserved">new</span>[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
 
<span class="reserved">var</span> x = r.Next(3, 5); <span class="comment">// 3, 4 (5を含まない)</span>
<span class="reserved">var</span> s = a.AsSpan(3, 5); <span class="comment">// 3, 4, 5, 6, 7 (3から始めて5つ)</span>
</code></pre>

ちょっとでもわかりやすくしたければ、以下のように名前付き引数にすべきかもしれません。

<pre class="source" title="">
<code><span class="reserved">var</span> x = r.Next(minValue: 3, maxValue: 5); <span class="comment">// 「5つ」でないことは明確なものの、5を含むかどうかわからず</span>
<span class="reserved">var</span> s = a.AsSpan(start: 3, length: 5); <span class="comment">// これなら割とわかりやすく「3から始めて5つ」</span>
</code></pre>

`Random.Next`の例のように、名前が「max」だけで、「含むかどうか」がわからないAPIも多いです。
この区別のために、`Parallel.For`なんかは引数名が`fromInclusive`、`toExclusive`とかになっていたりします。
しかし、どんどん名前が長くなって書きづらい上に、
所詮は命名規約なので規約が守られない場合だってあり得ます。

さらにいうと、多次元データになるともっとしんどくなります。

<pre class="source" title="">
<code><span class="reserved">var</span> m = <span class="reserved">new</span>[,]
{
    { 1, 2, 3, 4 },
    { 5, 6, 7, 8 },
    { 9, 10, 11, 12 },
};
 
<span class="comment">// (x, y) が (1, 2) ～ (3, 4) の範囲？</span>
<span class="comment">// x が 1～2、y が 3～4 の範囲？</span>
<span class="comment">// 2, 4 は含む？含まない？</span>
<span class="reserved">var</span> n = m.Slice(1, 2, 3, 4);

</code></pre>

ということで、範囲を表す専用の文法が欲しいという話になります。

## 背景2: インデックス用途

その両端を含むか含まないか問題ですが、どちらがいいかは正直用途によります。

例えば、`x in 1..3` みたいに「`x` がその範囲に入るかどうか」(マッチング用途)の場合、
大体は「3も含む」の方にしたいという要望が多いです。
一方で、`x[1..3]`みたいに「`x`の1番から3番の要素」(インデックス用途)の場合、
「3を含まない」にした方が都合がよかったりします。
インデックス用途における「含まない」の利点は以下のようなもの。

- 実装上、パフォーマンス的に有利
  - 長さを `length = maxExclusive - minInclusive` で計算できる(`+1`が要らない)
  - ループが `for (var i = minInclusive; i < maxExclusive; i++)` になる(`<=` だと `int.MaxValue`に対する特別扱いが必要)
- `i..i`が空(0要素)範囲になる。「含む」の方だと空範囲が`i..i-1`になってちょっとキモい

C# 8.0で導入される範囲構文は、後者のインデックス用途を狙ったもので、「末尾は含まない」の方になります。

ちなみに、「範囲に入るかどうか」の方は別途[パターン マッチング](../../../../study/csharp/datatype/patterns.md)の一種(range pattern)として提供される可能性はあるんですが、
おそらく別の文法(`x in 1 to 3`みたいな)になりそうです。

一方、インデックス用途に絞ったことで、
「配列の末尾からi番目」を表したいという別の要望も出てきます。
そこで、`^`演算子を導入して、`^i`で「末尾からi番目」を表すことになりました。

## 文法

ということで、C# 8.0で導入されるのは以下のような文法です。

- `^i` 演算子で「末尾からi番目」を表す `Index`型を作る
  - 正確には「`Length - i`」を表す。`^0`は`Length`番目なので、`array[^0]`は OutOfRange。
- `i..j` 演算子で、「i番目からj番目」を表す`Range`型を作る
  - 開始の方(`i`)は含む、末尾の方(`j`)は含まない
  - 両端は省略可能。`i..`なら「iから末尾」、`..j`なら「先頭からj」、`..`なら「配列全体」
  - `Index`を受け付ける。`^3..`なら「末尾から3要素」

ちなみに、`Range`、`Index`はいずれも`System`名前空間の構造体です。

例えば以下のように書けます。

<pre class="source" title="">
<code><span class="reserved">var</span> data = <span class="reserved">new</span>[] { 0, 1, 2, 3, 4, 5 };
 
<span class="comment">// 1～2要素目。2 は exclusive。なので、表示されるのは 1 だけ。</span>
Write(data[1..2]);
 
<span class="comment">// 先頭から1～末尾から1。 1, 2, 3, 4</span>
Write(data[1..^1]);
 
<span class="comment">// 先頭～末尾から1。 0, 1, 2, 3, 4</span>
Write(data[..^1]);
 
<span class="comment">// 先頭から1～末尾。 1, 2, 3, 4, 5</span>
Write(data[1..]);
 
<span class="comment">// 全体。0, 1, 2, 3, 4, 5</span>
Write(data[..]);
</code></pre>

![範囲構文](../../../../../assets/media/1167/ranges.png)

## 内部実装

実装としては以下のようになります。

- `^i` は`new Index(i, true)`になる(第2引数の`true`が「末尾から」の意味)
- 整数から `Index` へは暗黙の型変換がある
- `i..j`は`Range.Create(i, j)`になる
- `i..`は`Range.FromStart(i)`になる
- `..j`は`Range.ToEnd(j)`になる
- `..`は`Range.All()`になる

<pre class="source" title="">
<code><span class="reserved">var</span> r1 = <span class="type">Range</span>.Create(1, 2);                  <span class="comment">// 1..2</span>
<span class="reserved">var</span> r2 = <span class="type">Range</span>.Create(1, <span class="reserved">new</span> <span class="type">Index</span>(1, <span class="reserved">true</span>)); <span class="comment">// 1..^1</span>
<span class="reserved">var</span> r3 = <span class="type">Range</span>.ToEnd(<span class="reserved">new</span> <span class="type">Index</span>(1, <span class="reserved">true</span>));     <span class="comment">// ..^1</span>
<span class="reserved">var</span> r4 = <span class="type">Range</span>.FromStart(1);                  <span class="comment">// 1..</span>
<span class="reserved">var</span> r5 = <span class="type">Range</span>.All();                         <span class="comment">// ..</span>
</code></pre>

ちなみに、`Range`、`Index`はそれぞれ、

- `Index` … `int`を1つだけ持つ構造体
  - .NET の配列は負のインデックスを想定していないので、負の数を使って「末尾から」を表現
- `Range` … `Index`を2つ持つ構造体

になっています。

また、構文上は、`^`の方は単なる単項演算子、
`..`の方は専用の構文(オペランドを省略可能というのが特殊なので、単なる2項演算子扱いにはできない)だそうです。

## Rangeを受け付けるインデクサー

配列に対して `a[i..j]` と書いた時の挙動はちょっとまだもめているみたいです。
要は以下のどちらにすべきか。

- 配列からは配列で「subarray」を返すべきではないか
  - 新しい配列のアロケーションとコピーが発生
- アロケーションを避けるために `Span<T>` で返すべきではないか

Visual Studio 2019 Preview 1 での実装は前者になっていて、
`new T[]`と`Array.Copy`が生成されます。
パフォーマンスを気にするなら`a.AsSpan()[i..j]`と書く必要があります。
