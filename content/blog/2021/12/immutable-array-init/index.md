---
title: "ImmutableArray に対してコレクション初期化子"
source_url: "https://ufcpp.net/blog/2021/12/immutable-array-init/"
content_type: "BlogEntry"
published_at: "2021-12-30T18:31:17"
updated_at: "2021-12-30T18:31:17"
tags: []
umbraco_id: 2398
parent_id: 2375
sort_order: 11
aliases: []
---

# ImmutableArray に対してコレクション初期化子

Immutable コレクションは現状いろんな使いにくさがあって悪名高いわけですが。
今日は「[リスト パターンの回に出した `[]` リテラルの話](../list-pattern/index.md#collection-literal)」と絡むので、特に `ImmutableArray` とかに対して[コレクション初期化子](../../../2016/12/tipscollectioninitializer/index.md)が使えないという話をします。

## クイズ

以下のようなコード。

<pre class="source" title="ImmutableArray にコレクション初期化子">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>() { 1, 2 };

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">a</span>)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
}
</code></pre>

どういう結果になるでしょう？

* 1, 2, 3 が表示される
* 何も表示されない
* `foreach` のところで例外が出る
* `new` のところで例外が出る
* コンパイルできない

ちなみに、[C# ライブ配信中に参加者みんな間違えてました](https://youtu.be/AzJd1tAZdfo?t=2466)。

## 答え

`new` のところで例外です。
しかもぬるぽ。

<pre>
Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at System.Collections.Immutable.ImmutableArray`1.get_Length()
   at System.Collections.Immutable.ImmutableArray`1.Add(T item)
   at Program.<Main>$(String[] args) in C:\source\repos\ConsoleApp1\ConsoleApp1\Program.cs:line 4
</pre>

そして、どうしてこうなるかの説明にも何段階かの変形が必要という…

とりあえず、`foreach` のところは無罪というか、その行までたどり着かないのでいったん削除。
(ちなみに、もしたどり着けた場合、`foreach` でも例外が出ます。)

<pre class="source" title="問題のコード">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>() { 1, 2 };
</code></pre>

コレクション初期化子は、以下のように、`new()` の後に `Add` メソッドを呼ぶという展開のされ方になります。

<pre class="source" title="コレクション初期化子を展開">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>();
<span class="variable">a</span>.<span class="method">Add</span>(1); <span class="comment">// ぬるぽるのはこの行になる。</span>
<span class="variable">a</span>.<span class="method">Add</span>(2);
</code></pre>

まあ、この時点ですでに問題の原因が分かってくる頃かと思いますが、一応もう1段。
`ImmutableArray` は構造体で、C# 10.0 より前には構造体に引数なしコンストラクターがなかったので、これは以下のコードと同じ意味になります。

<pre class="source" title="new() の顛末">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved"><em>default</em></span>;
<span class="variable">a</span>.<span class="method">Add</span>(1); <span class="comment">// ぬるぽるのはこの行。</span>
<span class="variable">a</span>.<span class="method">Add</span>(2);
</code></pre>

[構造体の `default`](../../../../study/csharp/resource/rm_default.md#sec-default-value) は「全ビットを0にする」みたいな扱いで、
参照型の場合には `null` が入ります。

要するに、実質以下のコードと同じような挙動になります。

<pre class="source" title="説明用: 実質同じ挙動のコード">
<code><span class="comment">// 実質これと同じ結果</span>
<span class="reserved">using</span> System.Collections.Immutable;

<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="type">ImmutableArray</span>.<span class="method">Create</span>&lt;<span class="reserved">int</span>&gt;(<span class="variable">items</span>: <span class="reserved">null</span>);
<span class="variable">a</span>.<span class="method">Add</span>(1); <span class="comment">// ぬるぽる原因は items: null なせい。</span>
<span class="variable">a</span>.<span class="method">Add</span>(2);
</code></pre>

ちなみに、「既存の構造体に引数なしコンストラクターを足すのは破壊的変更」なので、破壊的変更を極力割けて通っている .NET 的に、追加されることはないと思います。なのでたぶん、`ImmutableArray<T>` がこんな変な挙動から解放される未来もないと思われます。

また、以下のようにぬるぽ回避コードを入れても、おそらくほとんどの人にとって所望の結果にはならないと思います。

<pre class="source" title="ぬるぽ回避">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="comment">// ぬるぽ回避</span>
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="type">ImmutableArray</span>.<span class="method">Create</span>(<span class="type">Array</span>.<span class="method">Empty</span>&lt;<span class="reserved">int</span>&gt;());
<span class="variable">a</span>.<span class="method">Add</span>(1); <span class="comment">// 無事通過。</span>
<span class="variable">a</span>.<span class="method">Add</span>(2);

<span class="comment">// ただし、a の中身は Empty のまま。</span>
<span class="comment">// まあ、immutable ですからね。初期状態から変わるはずないですよね。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">a</span>)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
}
</code></pre>

ええ、immutable ですから。
`Add` は「元の配列に1要素 append した新しい配列を作って返す」という仕様。
自身の書き換えはしません。

## 課題

2つの問題が重なってこんなことになっています。

* コレクション初期化子に適さないのに、C# の構文上はコレクション初期化子が使える条件を満たしてしまっている
* `default` が不正な状態になる構造体を作ってしまっている

と言うことで、また日を改めて書くと思うんですが、それぞれ以下のような解決策が考えられています。

* 今後追加予定の新構文の `[]` リテラルでは、`ImmutableArray` でも使えるような展開の仕方をする
* 基本的に `default` 禁止なアノテーション([null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)の構造体版)を作る
