---
title: "【C# 12 候補】コレクション リテラル"
source_url: "https://ufcpp.net/blog/2023/1/collection-literal/"
content_type: "BlogEntry"
published_at: "2023-01-29T16:24:10"
updated_at: "2023-01-29T16:24:10"
tags: []
umbraco_id: 2454
parent_id: 2449
sort_order: 4
aliases: []
---

# 【C# 12 候補】コレクション リテラル

今回はコレクション リテラルの話。

・提案 issue: [[Proposal]: Collection literals #5354](https://github.com/dotnet/csharplang/issues/5354)

今日の話も、提案自体は[去年から結構前向きに検討されてたもの](../../../2021/12/collection-literal/index.md)です。
リスト パターンの実装の過程で出てきた案で、元から「C# 11 には間に合わないかも」みたいな空気感だったもの。
昨年11月に C# 11 が世に出た後、改めて進捗が出始めたので、今日はその辺りの話になります。

ちなみに、[Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) で、[最近 "C# Next" の欄に並びました](https://github.com/dotnet/roslyn/commit/0e17a88c3b34f7c7f995c5e29207d2bfe0095fb9)。
実装もちらほら始まっているので、割かし C# 12 入りが有望だと思います。

## コレクション リテラルおさらい

[去年](../../../2021/12/collection-literal/index.md)から大体決まってそうなところをおさらい。

文法的には `[]` を使う案が有力です。

<pre class="source" title="[] リテラル案">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="comment">// いろんなコレクション型に対して共通して使える。</span>
<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1, 2, 3</span>];
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = [<span class="number">1, 2, 3</span>];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> = [<span class="number">1, 2, 3</span>];
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> = [<span class="number">1, 2, 3</span>];
</pre>

また、同時に、いわゆる "spread" と呼ばれる操作も導入されます。

<pre class="source" title="コレクションの spread">
<span class="comment">// いろんなコレクション型に対して共通して使える。</span>
<span class="reserved">int</span>[] <span class="variable">a</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>];
<span class="reserved">int</span>[] <span class="variable">b</span> <span class="operator">=</span> [<span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// これだと、2重配列の [ [1, 2], [3, 4] ] になる。</span>
<span class="reserved">int</span>[][] <span class="variable">nested</span> <span class="operator">=</span> [<span class="variable">a</span>, <span class="variable">b</span>];

<span class="comment">// これが &quot;spread&quot;。各コレクションを展開して、concat 的な操作をする。</span>
<span class="comment">// [ 1, 2, 3, 4] になる。</span>
<span class="reserved">int</span>[] <span class="variable">spread</span> <span class="operator">=</span> [..<span class="variable">a</span>, ..<span class="variable">b</span>];

<span class="comment">// もちろん混在もあり得る。</span>
<span class="comment">// [ [1, 2], 3, 4] になる。</span>
<span class="reserved">object</span>[] <span class="variable">spread</span> <span class="operator">=</span> [<span class="variable">a</span>, ..<span class="variable">b</span>];
</pre>

導入の動機は以下のようなものです。

* 現在だと `new T[] { ... }`, `new T { ... }`, `stackalloc T[] { ... }`, `T.Create(...)` みたいにバラバラな書き方になるものを簡潔な書き方に統一する
    * 特に、[`stackalloc`](../../../2022/12/stackalloc-natural-type/index.md) みたいな不自然な構文の必要性を減らす
* [リスト パターン](../../../../study/csharp/datatype/patterns.md#list)と対称な構文を用意する
* コレクション初期化周りのパフォーマンス改善
    * 現状書くとすると `a.Concat(b).Append(c).ToArray()` は書くのが煩雑な上にパフォーマンスが悪い
    * [ReadOnlySpan 最適化](../../../2022/2/span-optimization/index.md)みたいな知らないとまず書けない最適化を減らしたい
* 型推論フレンドリーにしたい
    * [現在の事故例](../../../2022/11/covariantarrayincident/index.md)

## その後

最近の C# チームは、有望そうな機能ごとに、その機能を専門に検討する「Working Group」という単位を作って作業をしています。
コレクション リテラルにも Working Group があって、2回ほど Working Group 内でのミーティング議事録が公開されています。

* [Collection literals working group meeting for October 6th, 2022](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/collection-literals/CL-2022-10-06.md)
* [Collection literals working group meeting for October 14th, 2022](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/collection-literals/CL-2022-10-14.md)
* [Collection literals working group meeting for October 21st, 2022](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/collection-literals/CL-2022-10-21.md)

以下、これらの議事録から[去年](../../../2021/12/collection-literal/index.md)からの進捗を拾って列挙していこうかと思います。

* 自然な型 (`var x = [a];` のときの `x` の型)は `List<T>` がよさそう
    * `var a = [x]; a.Add(y);` ってやりたい
    * `let` (readonly local) が入るんなら、`let x = [a];` の自然な型は `ImmutableArray<T>` がよさげ。Swift がそう
* アロケーションを減らせるように、長さが既知なら `new List<T>(length)` みたいに capacity を渡せるようにしたい
    * `IEnumerable<T>` に対しても、`Enumerable.TryGetNonEnumeratedCount` を使って事前に長さをとれないか試みる
* 当初予定では `var x = new(length); x.Init(buffer);` みたいなコード生成のつもりでいたけども、このメソッドは `Init` よりも `Construct` という名前の方がいいかも
* 長さが未知の `[..enumarable]` を `ImmutableArray<T>` みたいな `Add` が使えない型に対しても使えるようにしたい
* lazy な enumerable に対して `[..a, ..b]` するとき、これも lazy であってほしいと望む人もいそうだけど、たぶん、(即時評価で)新しいコレクション作る
* 新しい文法がが「一番パフォーマンスいい」状態にはしたい
    * (`ImmutableArray<T>` に対する `[..enumerableOfT]` は `ImmutableArray.CreateRange<T>` よりも遅くなってはいけない。)
* `[..a]` は `a` のクローン手段として使えていい
* `[]` は `null` と同様に、「どんな型のコレクションにでも代入できる特殊なリテラル」であるべき
* `var list = cond ? [str] : [obj];` の型はどう決める？ target-typed? それか、common-base-type?
* 一緒に Dictionary リテラルもやる。最有力候補は `[key:value]`という書き方
    * `key:value` の部分で `KeyValuePair<TKey, TValue>` なリテラルになりそう
    * `List<T>`, `Dictionary<TKey, TValue>`, `KeyValuePair<TKey, TValue>` の3つの型は first-class になりそう

### Dictionary リテラル

特に、Dictionary リテラルの話は[去年](../../../2021/12/collection-literal/index.md)はまだなかったですかね、確か。
候補となる文法は以下のようなもの。
(このうち、`[key: value]` が有力。`[ [key] = value ]` もありかも。)

<pre class="source" title="Dictionary リテラルの文法候補">
<span class="reserved">var</span> <span class="variable">dict1 <span class="operator">=</span> { <span class="string">&quot;key1&quot;</span></span>: <span class="string">&quot;value1&quot;</span>, <span class="string">&quot;key2&quot;</span>: <span class="string">&quot;value2&quot;</span> };
<span class="reserved">var</span> <span class="variable">dict2</span> <span class="operator">=</span> [<span class="string">&quot;key1&quot;</span>: <span class="string">&quot;value1&quot;</span>, <span class="string">&quot;key2&quot;</span>: <span class="string">&quot;value2&quot;</span> ];
<span class="reserved">var</span> <span class="variable">dict3</span> <span class="operator">=</span> [ [<span class="string">&quot;key1&quot;</span>] <span class="operator">=</span> <span class="string">&quot;value1&quot;</span>, [<span class="string">&quot;key2&quot;</span>] <span class="operator">=</span> <span class="string">&quot;value2&quot;</span> ];
<span class="reserved">var</span> <span class="variable">dict4</span> <span class="operator">=</span> [<span class="string">&quot;key1&quot;</span> <span class="operator">=&gt;</span> <span class="string">&quot;value1&quot;</span>, <span class="string">&quot;key2&quot;</span> <span class="operator">=&gt;</span> <span class="string">&quot;value2&quot;</span>];
</pre>

Dictionary リテラルをやるのであれば、一緒に「Dictionary パターン」もやりたいそうです。

<pre class="source" title="Dictionary パターンの文法候補">
<span class="reserved">var</span> <span class="variable">dict</span> = [ <span class="string">"key1"</span>: <span class="string">"value1"</span>, <span class="string">"key2"</span>: <span class="string">"value2"</span> ];

<span class="reserved">if</span> (<span class="variable">dict</span> <span class="reserved">is</span> [ <span class="string">"key1"</span>: <span class="reserved">var</span> <span class="variable">value</span> ])
{
}
</pre>

`[key: value]` の場合には `x.Add(new(key, value))` (もしくは前述の `Construct`)扱いで、
`[ [key] = value ]` の場合には `x[key] = value` 扱いという区別で両方認める可能性もあります。
この場合、「パターン」の方も、以下のような別パターンを考えます。

<pre class="source" title="Dictionary パターンの文法候補">
<span class="reserved">var</span> <span class="variable">dict</span> = [ [<span class="string">"key1"</span>] = <span class="string">"value1"</span>, [<span class="string">"key2"</span>] = <span class="string">"value2"</span> ];

<span class="reserved">if</span> (<span class="variable">dict</span> <span class="reserved">is</span> [ [<span class="string">"key1"</span>]: <span class="reserved">var</span> <span class="variable">value</span> ])
{
}
</pre>

他に、以下のような話あり。

* spread の併用で `[..dict, prop: a]` とか書くのも OK
* Swift は `[:]` で空辞書リテラル作れる。C# もやる？
* `init void Init(KeyValuePair<TKey, TValue>[] values)` で実装
* コレクション インデクサー(`List<string> list = [0: "first", 1: "second"];` みたいなの)も認める？
* Dictionary 2個連結した `[..dict1, ..dict2]` は Dictionary であるべき
