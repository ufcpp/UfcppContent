---
title: "コレクション式"
source_url: "https://ufcpp.net/study/csharp/datatype/collection-expression/"
content_type: "Article"
published_at: "2023-10-24T00:00:00"
updated_at: "2023-10-24T22:46:16"
tags: []
umbraco_id: 2475
parent_id: 1940
sort_order: 6
aliases:
  - "/csharp/datatype/collection-expression/"
---

# コレクション式

##<a id="sec-generated-title-1"></a> <a id="abstract">概要</a>
<h5 class="version version12">Ver. 12</h5>

C# 12 で、`[]` 記号を使って配列などの初期化ができるようになりました。
配列だけではなく、コレクション(`List<T>` 型など)、`Span<T>` なども全く同じ書き方で初期化できます。
これをコレクション式(collection expression)と言います。

<pre class="source" title="コレクション式">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> <em>[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</em>;
</pre>

また、コレクション式中では、`..` を使うことで「別のコレクションの中身の展開」ができます。
これを スプレッド (spread)演算子と言います。

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">array1</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="reserved">int</span>[] <span class="variable">array2</span> <span class="operator">=</span> [<span class="number">4</span>, <span class="number">5</span>, <span class="number">6</span>];

<span class="comment">// 0, 1, 2, 3, 4, 5, 6, 7</span>
<span class="reserved">int</span>[] <span class="variable">combined</span> <span class="operator">=</span> [<span class="number">0</span>, <em>..</em><span class="variable">array1</span>, <em>..</em><span class="variable">array2</span>, <span class="number">7</span>];
</pre>

##<a id="sec-generated-title-2"></a> <a id="background">背景: これまでのコレクションの初期化</a>
これまでだと、以下のように型に応じて書き方を変える必要がありました。

<pre class="source" title="これまでの書き方いろいろ">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="reserved">int</span>[] <span class="variable">array1</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="reserved">int</span>[] <span class="variable">array2</span> <span class="operator">=</span> { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="reserved">stackalloc</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> <span class="type"><span class="static">ImmutableArray</span></span><span class="operator">.</span><span class="method"><span class="static">Create</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
</pre>

1つずつ、もう少し補足を加えます。

#### <a id="sec-generated-title-3"></a>配列(1)
`new[] { }` という書き方で配列を作れます。

<pre class="source" title="配列の new">
<span class="reserved">int</span>[] <span class="variable">array1</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

正確にはこれは省略形で、本来の書き方では、`new T[length]` という書き方で配列を作ります。
ただし、`{}` (配列初期化子といいます)がついている場合、中身の数から長さが確定するので `new T[] { ... }` というように長さを省略できます。
また、`{}` の中身から型を推論できる場合には `T` の部分を省略できて、`new[] { ... }` と書けます。

`new[] { ... }`の型推論は「要素の中身から型決定」なので、例えば以下のようなコードはエラーになります。

<pre class="source" title="要素の型からの型推論">
<span class="reserved">byte</span>[] <span class="variable">array</span> <span class="comment">// byte[] 型</span>
    <span class="operator">=</span> <span class="error" title="CS0029"><span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> }</span>; <span class="comment">// 数値リテラルはデフォルトでは int 型。int からの型推論で int[] 型に。</span>
</pre>

また、「要素の中身から型決定」だと、ブログ「[共変配列事故](../../../blog/2022/11/covariantarrayincident/index.md)」で書いたような問題を踏むことがあります。

#### <a id="sec-generated-title-4"></a>配列(2)
配列の変数宣言時に限り、以下のように `{}` だけで初期化できます。

<pre class="source" title="配列初期化子">
<span class="reserved">int</span>[] <span class="variable">array2</span> <span class="operator">=</span> { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

宣言と初期化を別の行に分けてしまうと `{}` を使えなくなります。(`new[] { }` なら使えます)。

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">array</span>; <span class="comment">// 宣言</span>

<span class="variable">array</span> <span class="operator">=</span> <span class="error" title="CS1525"><span class="error" title="CS1002">{</span></span> <span class="number">1</span><span class="error" title="CS1513"><span class="error" title="CS1002">,</span></span> <span class="number">2</span><span class="error" title="CS1513"><span class="error" title="CS1002">,</span></span> <span class="number">3</span> <span class="error" title="CS1002">}</span>; <span class="comment">// 宣言と別の行ではこの書き方はできない</span>
</pre>

#### <a id="sec-generated-title-5"></a>コレクション初期化子
所定の条件を満たす型に対して、`new T() { }` という書き方で初期化ができます。
これを[コレクション初期化子](../functional/sp3_lambda.md#collectioninit)と言います。

これと、[ターゲットからの型推論](../cheatsheet/ap_ver9.md#target-typed-new)を組み合わせると、以下のように `new() { }` という書き方で初期化できます。

<pre class="source" title="">
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

配列の場合は `new[] { }` なのに対して、その他のコレクションは `new() { }` になります。
このせいで、「配列とその他のコレクションを切り替えて使う」みたいなことがちょっと面倒になっています。

<pre class="source" title="配列とコレクションの切り替えが難しい">
<span class="preprocess">#</span><span class="preprocess">if</span> WPF
<span class="excluded">using System.Collections.ObjectModel;

// WPF の時だけ ObservableCollection を使う。
ObservableCollection&lt;int&gt;
</span><span class="preprocess">#</span><span class="preprocess">else</span>
<span class="comment">// 他はただの配列。</span>
<span class="reserved">int</span>[]
<span class="preprocess">#</span><span class="preprocess">endif</span>
    <span class="variable">list</span> <span class="operator">=</span> <span class="error" title="CS8752"><span class="reserved">new</span>() { <span class="number"><span class="error" title="CS1061">1</span></span>, <span class="number"><span class="error" title="CS1061">2</span></span>, <span class="number"><span class="error" title="CS1061">3</span></span> }</span>; <span class="comment">// () と [] が違うのでコード共通化が難しい。</span>
</pre>

#### <a id="sec-generated-title-6"></a>stackalloc
パフォーマンス的に配列を使いたくない場面があり、
そういう場合 [stackalloc](../interop/sp_unsafe.md#safe-stackalloc)というものが使えることがあります。

元々は unsafe な機能でめったに使うものではなかったんですが、
C# 7.2 で、[`Span<T>` 構造体](../resource/span.md)の導入とともに safe な構文になりました。

<pre class="source" title="stackalloc">
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="reserved">stackalloc</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

これも本来の書き方は `stackalloc T[length]` で、長さ・型の推論が働くことで `stackalloc[] { }`という書き方ができています。

ちなみに、`stackalloc` は参照型を含められないという問題があって、
例えば以下のようなコードはコンパイル エラーになります。

<pre class="source" title="参照型に対する stackalloc はエラーに">
<span class="type struct">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="error" title="CS0208"><span class="reserved">stackalloc</span>[] { <span class="string">&quot;abc&quot;</span> }</span>;
</pre>


####<a id="sec-generated-title-7"></a> <a id="static-data">静的データ最適化</a>
`ReadOnlySpan<T>` 型に対して配列を渡すと、
最適化で配列が消えてくれて、stackalloc を使うよりもパフォーマンスがよくなることがあります。

<pre class="source" title="静的データ最適化">
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

詳しくはブログ「[静的なデータの ReadOnlySpan 最適化](../../../blog/2018/12/staticdatareadonlyspan/index.md)」に書いたことがあるのでそちらを参照してください。
(C# 12 からは `byte`, `sbyte` 以外の型に対してもこの最適化がかかります。)

この最適化は、「条件がそろえば配列が消える」というのが上級者にしかわからないという問題があります。
ぱっと見はパフォーマンスが悪そうに見えますし、
古い C# コンパイラーで同様のコードをコンパイルすると実際パフォーマンスが悪いので、
結構な混乱を招いています。

#### <a id="sec-generated-title-8"></a>ImmutableArray
[`ImmutableArray<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.immutablearray-1) という型に対しては初期化子の類が使えません。
以下のように地道に `Create` メソッドを呼ぶ必要があります。

<pre class="source" title="ImmutableArray.Create">
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> <span class="type"><span class="static">ImmutableArray</span></span><span class="operator">.</span><span class="method"><span class="static">Create</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>);
</pre>

ところが質が悪いことに、`ImmutableArray<T>` 型はコレクション初期化子を使える要件を満たしてしまっています。
以下のようなコードは「コンパイルはできてしまうけど、実行すると必ず例外が起こる」という、かなりつらい状態になります。

<pre class="source" title="ImmutableArray に対してコレクション初期化子">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="comment">// コンパイルはできてしまう。</span>
<span class="comment">// ところが実行すると必ず例外。</span>
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> <span class="reserved">new</span>() { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };
</pre>

##<a id="sec-generated-title-9"></a> <a id="collection-expr">コレクション式</a>
前節で説明したような「型によって初期化の方法が違う」という問題と、補足で書いた諸問題に対処するため、
C# 12 で<strong id="key-collection-expr" class="keyword">コレクション式</strong>(collection expression)というものを導入することになりました。

概要でも書いたように、`[]` 記号を使って配列などを初期化します。

<pre class="source" title="コレクション式">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="reserved">int</span>[] <span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
</pre>

コレクション式は以下のような型に対して使えます。

* 配列
* `Span<T>`, `ReadOnlySpan<T>`
* 配列が実装している `IEnumerable<T>`, `IReadOnlyList<T>`, `IList<T>` などのインターフェイス
* [`CollectionBuilder` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.collectionbuilderattribute)が付いている型
* [コレクション初期化子](../functional/sp3_lambda.md#collectioninit)の条件を満たす型

配列初期化子と違って、`[]` はどこにでも書けます。

<pre class="source" title="宣言以外でも書ける">
<span class="reserved">int</span>[] <span class="variable">array</span>;

<span class="comment">// OK</span>
<span class="variable">array</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
</pre>

stackalloc と違って、参照型の `Span<T>` に対しても使えます。
ちゃんと、(配列を作るよりも)パフォーマンスのいいコードになります。
(内部的には[InlineArray](inline-array.md)を使っています。)

<pre class="source" title="参照型の Span">
<span class="comment">// OK</span>
<span class="type struct">Span</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="string">&quot;abc&quot;</span>];
</pre>

`ReadOnlySpan<T>` に対しては前述の[静的データ最適化](#static-data)がかかるわけですが、
「配列を new してそうに見えるコード」がないだけ混乱が少ないです。

<pre class="source" title="ReadOnlySpan の静的データ最適化">
<span class="comment">// 以下のコードはちゃんと静的データ最適化がかかる。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
</pre>

コレクション初期化子の条件よりも、`CollectionBuilder` 属性の方が優先されます。
[`ImmutableArray<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.immutablearray-1) には `CollectionBuilder` 属性が付いていることによってコレクション式を使えます。

<pre class="source" title="ImmutableArray に対するコレクション式">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="type struct">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// CollectionBuilder 属性の情報をもとに、C# コンパイラーが以下のようなコードに展開する。</span>
<span class="comment">//</span>
<span class="comment">//ReadOnlySpan&lt;int&gt; temp = [1, 2, 3];</span>
<span class="comment">//ImmutableArray&lt;int&gt; immutable = ImmutableArray.Create(temp);</span>
</pre>

ちなみに、`CollectionBuilder` 属性はインターフェイスにも付けることができます。
[`IImmutableList<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.iimmutablelist-1)が一例で、以下のようなコードを書けます。

<pre class="source" title="CollectionBuilder 属性はインターフェイス可">
<span class="reserved">using</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Immutable;

<span class="type">IImmutableList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// IImmutableList インターフェイスについている CollectionBuidler 属性では、</span>
<span class="comment">// ImmutableList.Create メソッドを使うよう指定されているので、</span>
<span class="comment">// おおむね以下のようなコードと同じ意味。</span>
<span class="comment">//</span>
<span class="comment">//ReadOnlySpan&lt;int&gt; span = [1, 2, 3];</span>
<span class="comment">//IImmutableList&lt;int&gt; immutable = ImmutableList.Create(span);</span>
</pre>

`List<T>` は「コレクション初期化子の条件」に該当していて、コレクション初期化子の時と同じコードに展開されます。

<pre class="source" title="">
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// コレクション初期化子 new() { 1, 2, 3 } と同じ結果。</span>
<span class="comment">//</span>
<span class="comment">//List&lt;int&gt; list = new();</span>
<span class="comment">//list.Add(1);</span>
<span class="comment">//list.Add(2);</span>
<span class="comment">//list.Add(3);</span>
</pre>

`IEnumerable<T>` などのインターフェイスに対しては、
将来の最適化の余地を残すため、実際に何の型が使われるかは仕様化されていません。
参考までに、「C# 12 リリース時点」では以下のような実装になっています。

<pre class="source" title="">
<span class="comment">//※ C# 12 時点の実装。将来、最適化で変更する余地あり。</span>

<span class="comment">// 長さが既知で読み取り専用 → ReadOnlyArray みたいな型をコンパイラーが生成して使う。</span>
<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">x1</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type">IReadOnlyList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">x2</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type">IReadOnlyCollection</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">x3</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// 書き換え・追加・削除可能なもの → new List&lt;T&gt; を使用。</span>
<span class="type">ICollection</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">x4</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">x5</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="method"><span class="static">Twice</span></span>(<span class="variable">x1</span>);

<span class="comment">// 長さが未知の場合も new List&lt;T&gt; を使用。</span>
<span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method"><span class="static">Twice</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">e</span>) <span class="operator">=&gt;</span> [.. <span class="variable local">e</span>, .. <span class="variable local">e</span>];

<span class="comment">// 空っぽの時は Array.Empty&lt;T&gt;() を使用。</span>
<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">empty</span> <span class="operator">=</span> [];
</pre>

いずれにせよ、ターゲットとする型から最適なものを選んで展開されると思ってください。
コレクション式は「書きやすい」だけではなく、「<em>一番パフォーマンスがいい</em>」を目標としています。

(おおむね目標は達成しているので、以前の書き方を積極的に選ぶ理由はありません。
配列やコレクションの初期化は、C# 12 以降を使えるのであればすべて `[]` を使っていいと思います。)

###<a id="sec-generated-title-10"></a> <a id="square-bracket">余談: [] 括弧</a>
C# ではこれまで、
配列の `new[] { }` にしろ、コレクション初期化子の `new() { }` にしろ、
コレクション系の構文には `{}` を使うものが多かったわけですが、
C# 12 のコレクション式は `[]` になりました。

その理由として、`{}` は用途が多すぎてこれ以上新しい構文を兼ねることが難しかったというのがあります。

<pre class="source" title="">
<span class="comment">// 普通に単独で書ける。</span>
<span class="comment">// これは「空のブロック」。</span>
<span class="comment">// 要は、if とかの後ろにある { } と同じものを単独で書いてる。</span>
{ }

<span class="comment">// ラムダ式の後ろとか、</span>
<span class="reserved">var</span> <span class="variable">action</span> <span class="operator">=</span> () <span class="operator">=&gt;</span> { };

<span class="comment">// メソッドの後ろとかにも普通にブロックを書く。</span>
<span class="reserved">void</span> <span class="method">localFunc</span>() { }

<span class="comment">// 初期化子に限っても、</span>
<span class="comment">// コレクション初期化子(Add メソッド呼び出しになる)と、</span>
<span class="reserved">var</span> <span class="variable">list</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };

<span class="comment">// オブジェクト初期化子(プロパティの初期化になる)があるし、</span>
<span class="reserved">var</span> <span class="variable">obj</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">EnumerationOptions</span> { <span class="property">BufferSize</span> <span class="operator">=</span> <span class="number">1024</span> };

<span class="comment">// 匿名クラスとかにも使う。</span>
<span class="reserved">var</span> <span class="variable">anonymous</span> <span class="operator">=</span> <span class="reserved">new</span> { <span class="property">X</span> <span class="operator">=</span> <span class="number">1</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">2</span> };

<span class="comment">// 将来候補として、「ブロック式」を導入したいという話もある。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> { <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">123</span>; <span class="variable">i</span> <span class="operator">*</span> <span class="variable">i</span> };
</pre>

特に、オブジェクト初期化子とコレクション初期化子が同じ記号を使っていて、混在不可なので以下のようなことが起こります。

<pre class="source" title="2つの {} 初期化子">
<span class="comment">// オブジェクト初期化子(プロパティの値指定)とコレクション初期化子(Add)の混在不可。</span>
<span class="reserved">var</span> <span class="variable">list1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { <span class="property">Capacity</span> <span class="operator">=</span> <span class="number">1014</span>, <span class="error" title="CS0747"><span class="number">1</span></span>, <span class="number"><span class="error" title="CS0747">2</span></span>, <span class="number"><span class="error" title="CS0747">3</span></span> };

<span class="comment">// ちなみに、[0] = 1 みたいな書き方はオブジェクト初期化子の範疇。</span>
<span class="reserved">var</span> <span class="variable">list2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;
{
    <span class="property">Capacity</span> <span class="operator">=</span> <span class="number">1014</span>,
    [<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>, <span class="comment">// 実行時例外にはなるけど、コンパイルは通っちゃう。</span>
};

<span class="comment">// なので「混在不可」のせいで以下のコードもエラー。</span>
<span class="reserved">var</span> <span class="variable">dictionary</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;
{
    <span class="error" title="CS0747">{ <span class="number">1</span>, <span class="number">2</span> }</span>,
    [<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>,
};
</pre>

この問題は C# 11 で[リスト パターン](patterns.md#list)を導入する際にも問題になりました。
C# 8 の頃からある[プロパティ パターン](patterns.md#property)との区別のためには `{}` を使えませんでした。
例えば以下のようなコードで、「空っぽのリスト」の意味で `list is {}` とは書けないという問題があったりします。

<pre class="source" title="{} の「兼用」は難しい">
<span class="reserved">var</span> <span class="variable">obj</span> <span class="operator">=</span> <span class="reserved">new</span> { <span class="property">X</span> <span class="operator">=</span> <span class="number">1</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">2</span> };
<span class="reserved">var</span> <span class="variable">list</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> };

<span class="comment">// プロパティ パターン。</span>
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">obj</span> <span class="reserved">is</span> { <span class="property">X</span>: <span class="number">1</span> };

<span class="comment">// (当初検討にあった)リスト パターンの候補としての {}。</span>
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">list</span> <span class="reserved">is</span> { <span class="number">1</span>, .. } ;

<span class="comment">// プロパティ パターンで {} は「単に null チェック」になる。</span>
<span class="reserved">var</span> <span class="variable">isNotNull</span> <span class="operator">=</span> <span class="variable">obj</span> <span class="reserved">is</span> { };

<span class="comment">// これは C# 11 以前から有効な文法(「null じゃない」の意味)なので、</span>
<span class="comment">// リスト パターンとして {} を使おうとすると「空リストとマッチ」にはできなくなる。</span>
<span class="reserved">var</span> <span class="variable">isEmpty</span> <span class="operator">=</span> <span class="variable">list</span> <span class="reserved">is</span> { };
</pre>

そこで C# 11 では最終的にリスト パターンに `[]` を採用したわけですが、
だったら「リスト構築の方でも `[]` を使った方がきれい」という話になりました。

<pre class="source" title="コレクション式とリスト パターンが対">
<span class="comment">// () コンストラクター/タプル構築と位置パターンが対。</span>
<span class="reserved">var</span> <span class="variable">obj</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">DateOnly</span>(<span class="number">2021</span>, <span class="number">1</span>, <span class="number">1</span>);
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">obj</span> <span class="reserved">is</span> (<span class="number">2021</span>, <span class="reserved">_</span>, <span class="reserved">_</span>);

<span class="reserved">var</span> <span class="variable">tuple</span> <span class="operator">=</span> (<span class="number">1</span>, <span class="number">2</span>);
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">tuple</span> <span class="reserved">is</span> (<span class="number">1</span>, <span class="reserved">_</span>);

<span class="comment">// {} オブジェクト初期化子/匿名クラスとプロパティ パターンが対。</span>
<span class="reserved">var</span> <span class="variable">prop</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">EnumerationOptions</span> { <span class="property">BufferSize</span> <span class="operator">=</span> <span class="number">1024</span> };
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">prop</span> <span class="reserved">is</span> { <span class="property">BufferSize</span>: <span class="number">1024</span> };

<span class="reserved">var</span> <span class="variable">anon</span> <span class="operator">=</span> <span class="reserved">new</span> { <span class="property">X</span> <span class="operator">=</span> <span class="number">1</span>, <span class="property">Y</span> <span class="operator">=</span> <span class="number">2</span> };
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">anon</span> <span class="reserved">is</span> { <span class="property">X</span>: <span class="number">1</span> };

<span class="comment">// [] コレクション式とリスト パターンが対。</span>
<span class="reserved">int</span>[] <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">list</span> <span class="reserved">is</span> [<span class="number">1</span>, ..];
</pre>

###<a id="sec-generated-title-11"></a> <a id="null-conditional-foreach">余談: null 条件 foreach</a>
「C# に追加して欲しい機能」としてそこそこ高頻度で挙がるものの1つに、
「null 条件 foreach」があったりします。
これは要するに、以下のようなコードを、

<pre class="source" title="null があり得る foreach">
<span class="method"><span class="static">Print</span></span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]);
<span class="method"><span class="static">Print</span></span>(<span class="reserved">null</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">list</span>)
{
    <span class="comment">// null が来た時に普通にぬるぽ。</span>
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local"><span class="warning" title="CS8602">list</span></span>)
        <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">item</span>}<span class="string"> </span><span class="string">&quot;</span>);
}
</pre>

以下のように直すのが嫌で、

<pre class="source" title="null チェックで1段インデントが下がる">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">list</span>)
{
    <span class="comment">// null チェックを1行足せばいいだけの話なものの、1段インデントが下がるのが嫌。</span>
    <span class="control">if</span> (<span class="variable local">list</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local">list</span>)
            <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">item</span>}<span class="string"> </span><span class="string">&quot;</span>);
}
</pre>

以下のように `foreach?` という構文を追加してもらえないかという提案です。

<pre class="source" title="foreach?">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">list</span>)
{
    <span class="comment">// foreach? 構文を足すのはどうだろう？</span>
    <span class="control">foreach</span>? (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local">list</span>)
        <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">item</span>}<span class="string"> </span><span class="string">&quot;</span>);
}
</pre>

確かに時々そういう機能が欲しくなるものの、
新文法を導入するほどのものかと言われると悩ましく、
長らく塩漬けになっている提案です。

そしてこの度コレクション式が入ったことで、
これと同等のことが以下のコードで実現できるようになりました。

<pre class="source" title="??[]">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Print</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">list</span>)
{
    <span class="comment">// ?? [] の4文字を追加すれば null チェック代わりになる。</span>
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local">list</span> <span class="operator">??</span> [])
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">item</span>}<span class="string"> </span><span class="string">&quot;</span>);
}
</pre>

これはまあ、以下のようなコードとほぼ同等です。

<pre class="source" title="??[] と同等のコード">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">list</span>)
{
    <span class="control">if</span> (<span class="variable local">list</span> <span class="reserved">is</span> <span class="reserved">null</span>) <span class="variable local">list</span> <span class="operator">=</span> <span class="type">Array</span><span class="operator">.</span><span class="static"><span class="method">Empty</span></span>&lt;<span class="reserved">int</span>&gt;();

    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local">list</span>)
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">Write</span></span>(<span class="string">$&quot;</span>{<span class="variable">item</span>}<span class="string"> </span><span class="string">&quot;</span>);
}
</pre>

`Array.Empty<int>()` を挟んでいるので無駄そうに見えるわけですが、
.NET 8 では[かなり強力な最適化がかかるようになっていて](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#collections)、
`Array.Empty<int>()` に対する foreach のコストはかなり低く抑えられます。

(これが十分に便利なので、`foreach?` 提案が通る可能性はかなり低くなりました。)

##<a id="sec-generated-title-12"></a> <a id="spread">スプレッド</a>
コレクション式の中では、`..` を使って「他のコレクションの中身を展開」みたいなことができます。
これを<strong id="key-spread" class="keyword">スプレッド</strong>(spread: 広げる、伸ばす、まき散らす)演算子と言います。

<pre class="source" title="..">
<span class="reserved">int</span>[] <span class="variable">a</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>];
<span class="reserved">int</span>[] <span class="variable">b</span> <span class="operator">=</span> [<span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// これで 0, 1, 2, 3, 4, 5 になる。</span>
<span class="reserved">int</span>[] <span class="variable">c</span> <span class="operator">=</span> [<span class="number">0</span>, ..<span class="variable">a</span>, ..<span class="variable">b</span>, <span class="number">5</span>];
</pre>

これは、`List<T>` でいう [`AddRange` メソッド](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.addrange)であったり、
[LINQ でいう `Concat` メソッド](../data/sp3_stdqueryo.md#concat)みたいな物です。

<pre class="source" title="AddRange と Concat">
<span class="reserved">int</span>[] <span class="variable">a</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>];
<span class="reserved">int</span>[] <span class="variable">b</span> <span class="operator">=</span> [<span class="number">3</span>, <span class="number">4</span>];

<span class="comment">// コレクション式では ..</span>
<span class="reserved">int</span>[] <span class="variable">expression</span> <span class="operator">=</span> [<span class="number">0</span>, ..<span class="variable">a</span>, ..<span class="variable">b</span>, <span class="number">5</span>];

<span class="comment">// List&lt;T&gt; でいう AddRange</span>
<span class="reserved">var</span> <span class="variable">list</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
<span class="variable">list</span><span class="operator">.</span><span class="method">Add</span>(<span class="number">0</span>);
<span class="variable">list</span><span class="operator">.</span><span class="method">AddRange</span>(<span class="variable">a</span>);
<span class="variable">list</span><span class="operator">.</span><span class="method">AddRange</span>(<span class="variable">b</span>);
<span class="variable">list</span><span class="operator">.</span><span class="method">Add</span>(<span class="number">5</span>);

<span class="comment">// LINQ でいう Concat</span>
<span class="reserved">var</span> <span class="variable">linq</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">0</span> }
    <span class="operator">.</span><span class="method">Concat</span>(<span class="variable">a</span>)
    <span class="operator">.</span><span class="method">Concat</span>(<span class="variable">b</span>)
    <span class="operator">.</span><span class="method">Append</span>(<span class="number">5</span>);
</pre>

ちなみに[先ほど](#square-bracket)、コレクション式とリスト パターンが対という話をしましたが、

<pre class="source" title="コレクション式とリスト パターンが対">
<span class="comment">// [] コレクション式とリスト パターンが対。</span>
<span class="reserved">int</span>[] <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">list</span> <span class="reserved">is</span> [<span class="number">1</span>, ..];
</pre>

スプレッドも[スライス パターン](patterns.md#slice-pattern)と対になっています。

<pre class="source" title="スプレッドとスライス パターンが対">
<span class="reserved">int</span>[] <span class="variable">list</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>];

<span class="comment">// スライス: コレクションの一部分を切り出して新しい変数に代入。</span>
<span class="control">if</span> (<span class="variable">list</span> <span class="reserved">is</span> [<span class="reserved">var</span> <span class="variable">first</span>, ..<span class="reserved">var</span> <span class="variable">middle</span>, <span class="reserved">var</span> <span class="variable">last</span>])
{
    <span class="comment">// スプレッド: コレクションの中身を展開して、1つのコレクションに結合。</span>
    <span class="reserved">int</span>[] <span class="variable">list2</span> <span class="operator">=</span> [<span class="variable">first</span>, ..<span class="variable">middle</span>, <span class="variable">last</span>];
}
</pre>


##<a id="sec-generated-title-13"></a> <a id="type-inference">型推論・オーバーロード解決</a>
元々あった配列やコレクション初期化子の構文では、`new T[]` や `new List<T>()` などというように、型名を明示できます。
それに対して、コレクション式は `[]` しか書かないので何の型になるかは完全に推論だよりになります。

ちなみに、[後述](#after12)しますが、C# 12 時点では以下のような「`var` との組み合わせ」はできません。
C# 13 以降で検討中です。

<pre class="source" title="C# 12 時点では型決定できない var">
<span class="reserved">var</span> <span class="variable">list</span> <span class="operator">=</span> <span class="error" title="CS9176">[<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>]</span>;</pre>

###<a id="sec-generated-title-14"></a> <a id="target-typed">ターゲットの型から</a>
基本的にコレクション式の型はターゲット(代入先の変数・引数の型)から決定します。

この点は `new[] {}` の場合と異なります。
`new[] {}` は「中身の型からの推論優先」で、コレクション式 `[]` は「ターゲットからの推論優先」です。
例えば、以下のようなコードでは、`x` と `y` に代入されるインスタンスの型が異なります。

<pre class="source" title="new[]{} は要素からの推論なので時々困る">
<span class="comment">// new[]{} は要素からの型推論。</span>
<span class="comment">// x には string[] が入る。</span>
<span class="reserved">object</span>[] <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="string">&quot;a&quot;</span> };

<span class="comment">// [] はターゲットからの型推論。</span>
<span class="comment">// y には object[] が入る。</span>
<span class="reserved">object</span>[] <span class="variable">y</span> <span class="operator">=</span> [<span class="string">&quot;a&quot;</span>];

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="method">GetType</span>()<span class="operator">.</span><span class="property">Name</span>); <span class="comment">// String[]</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">y</span><span class="operator">.</span><span class="method">GetType</span>()<span class="operator">.</span><span class="property">Name</span>); <span class="comment">// Object[]</span>

<span class="variable">y</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>; <span class="comment">// OK。</span>
<span class="variable">x</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="number">1</span>; <span class="comment">// 例外が出る(C# 1.0 からある嫌な仕様)。</span>
</pre>

(この例の最後の行はかなり[奇妙で安全性に欠ける仕様ですが、大昔の名残りで今更変更できない](../../../blog/2022/11/covariantarrayincident/index.md)そうです。)

コレクション式の「ターゲットからの型推論」は、型や式が入れ子になっていてもちゃんと働きます。
以下のように、[タプル](tuples.md)中にコレクションがあって、条件演算子や [switch 式](typeswitch.md#switch-expression)を経由していても正しく型推論されます。

<pre class="source" title="入れ子でもちゃんと型推論される例">
<span class="reserved">bool</span> <span class="variable">b</span> <span class="operator">=</span> <span class="reserved">true</span>;
<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="comment">// 条件演算子 x ? y : z とか、</span>
<span class="comment">// switch 式 x switch { ... } とか、</span>
<span class="comment">// タプルを使った入れ子とかあっても、ちゃんと型推論される。</span>
(<span class="reserved">byte</span>, (<span class="reserved">byte</span>, <span class="reserved">byte</span>[] z)[])[] <span class="variable">x</span> <span class="operator">=</span> <span class="variable">b</span> <span class="operator">?</span> [] <span class="operator">:</span> <span class="variable">i</span> <span class="control">switch</span>
{
    <span class="reserved">_</span> <span class="operator">=&gt;</span> [
        (<span class="number">1</span>, [(<span class="number">2</span>, [<span class="number">3</span>, <span class="number">4</span>])]) <span class="comment">// [3, 4] の部分、ちゃんと byte[] になる。</span>
    ]
};
</pre>

###<a id="sec-generated-title-15"></a> <a id="from-element">要素の型から</a>
一方で、メソッドのオーバーロード解決などが絡む場合、
コレクション式の中身からの型解決も働きます。

<pre class="source" title="要素の型からオーバーロード解決">
<span class="static"><span class="method">Print</span></span>([<span class="number">1</span>, <span class="number">2</span>]);     <span class="comment">// Print&lt;int&gt;</span>
<span class="method"><span class="static">Print</span></span>([<span class="number">1.1</span>, <span class="number">2.2</span>]); <span class="comment">// Print&lt;double&gt;</span>
<span class="method"><span class="static">Print</span></span>([<span class="string">&quot;a&quot;</span>, <span class="string">&quot;b&quot;</span>]); <span class="comment">// Print&lt;string&gt;</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span>[] <span class="variable local">args</span>) { <span class="comment">/* 省略 */</span> }
</pre>

ただ、スプレッドが絡むとき、スプレッドの中身の優先度は低いそうです。
(実装が大変なわりに需要が少ないという判断。)

<pre class="source" title="スプレッドが絡むとき">
<span class="reserved">byte</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>];

<span class="comment">// ..x しかない場合には x の型から byte[] に決定。</span>
<span class="method"><span class="static">Print</span></span>([.. <span class="variable">x</span>]);     <span class="comment">// Print&lt;byte&gt;</span>

<span class="comment">// ただ、その横に整数リテラルが1個でも並ぶと…</span>
<span class="comment">// 3 (int) につられて int[] に決定。</span>
<span class="static"><span class="method">Print</span></span>([.. <span class="variable">x</span>, <span class="number">3</span>]);  <span class="comment">// Print&lt;int&gt;</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Print</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span>[] <span class="variable local">args</span>) { }
</pre>

###<a id="sec-generated-title-16"></a> <a id="priority">オーバーロード解決の優先度</a>
`[]` は配列や `List<T>`, `Span<T>` など、いろいろな型になるわけですが、
では、そのいずれも候補になるオーバーロードの場合はどれが優先されるでしょうか。

これまでの C# では、「元の型に近いほど優先度が高い」というのがオーバーロード解決の基本ルールで、
具体的には以下のような順(上ほど優先)になっています。

1. ぴったり一致する型
2. 基底クラス(階層が近いほど優先)
3. 実装しているインターフェイス(階層が近いほど優先)
4. 暗黙の型変換できる型

このルールに沿うなら、配列(`[]` で作るのではなく `new T[]` で作る)の場合、
一例をあげると以下のような優先度になります。

1. `T[]`
2. `IList<T>`
3. `IEnumerable<T>`
4. `Span<T>` もしくは `ReadOnlySpan<T>` (両方あるとエラー)

一方で、コレクション式 `[]` の場合、「パフォーマンスがいいものを優先する」という目標のため、
`Span<T>` と `ReadOnlySpan<T>` を特別扱いして優先度を上げています(ぴったり一致する型よりも優先度が上)。
そのため、以下のような優先度になっています。

(※ .NET 8 RC 2 時点では `ReadOnlySpan<T>` の優先度がちょっと低いですが、正式リリースまでに変更される予定です。)

1. `ReadOnlySpan<T>`
2. `Span<T>`
3. `T[]`
4. `IList<T>`
5. `IEnumerable<T>`

以下に例を挙げます。

<pre class="source" title="具象型優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// int[]</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 具象型優先。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

<pre class="source" title="具象型優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// List&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 具象型優先。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

<pre class="source" title="具象型に近い方優先 (派生側優先)">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// IList&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 具象型に近い方優先 (派生側優先)。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

<pre class="source" title="Span 優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// Span&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// Span 優先(コレクション式のみの特殊動作)。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">_</span>) { } <span class="comment">// 普通は具象型優先。</span>
}
</pre>

<pre class="source" title="Span 優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// Span&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// Span 優先(コレクション式のみの特殊動作)。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// 普通は 派生 &gt; 型変換</span>
}
</pre>

<pre class="source" title="ReadOnlySpan 優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// ReadOnlySpan&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// ReadOnlySpan 優先(コレクション式のみの特殊動作)。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IReadOnlyList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// 普通は 派生 &gt; 型変換</span>
}
</pre>

<pre class="source" title="ReadOnlySpan 優先">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>([]); <span class="comment">// ReadOnlySpan&lt;int&gt;</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// ReadOnlySpan 優先(コレクション式のみの特殊動作)。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { } <span class="comment">// (.NET 8 RC 2 時点ではまだこっちが優先されてる。変更予定)</span>
}
</pre>

ちなみに、以下のような場合には(普通のオーバーロード解決でも、コレクション式でも)不明瞭(オーバーロード解決不能)でコンパイル エラーになります。

<pre class="source" title="具象型同士は同列">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>([]); <span class="comment">// コンパイル エラー</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 具象型同士は同列。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

<pre class="source" title="派生関係のないインターフェイスは同列">
<span class="type">A</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>([]); <span class="comment">// コンパイル エラー</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 派生関係のないインターフェイスは同列。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">IReadOnlyList</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

##<a id="sec-generated-title-17"></a> <a id="after12">将来計画(C# 13 以降)</a>
スケジュールの関係で C# 12 からは外れて、
C# 13 以降で実装される予定になっている機能がいくつかあります。
詳細は実装されてから追記しますが、
簡単に紹介だけしておきます。

#### <a id="sec-generated-title-18"></a>自然な型
`var x = [1, 2];` みたいに、`var` と組み合わせでは「ターゲットからの型推論」ができないわけですが、
この時にデフォルトで何の型になるかは C# 12 時点では決めかねました。

使い勝手を考えると(機能が多い) `List<T>` がいいですし、
一方で、パフォーマンスを考えると `Span<T>` がいいです(検討中)。

#### <a id="sec-generated-title-19"></a>拡張メソッドからの型推論
単純に `var x = [1, 2];` とは書けない一方で、
C# 12 時点でも、`var x = (int[])[1, 2];` みたいにキャストを挟めば型を決定できます。

ただ、キャストは「なんか書きにくい」
(キャストは型推論が効かないとか、`()` がタイピングしにくいとか、コード補完の都合で後置きの方が書き心地がいいとか)
という問題があって、
以下のように、拡張メソッドからの型推論が効くようにしてほしいという話があります。

<pre class="source" title="拡張メソッドからの型推論">
<span class="comment">// (List&lt;int&gt;)[1, 2] よりも、拡張メソッド形式の方が書き心地がいい。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="error" title="CS9176">[<span class="number">1</span>, <span class="number">2</span>]</span><span class="operator">.</span>AsList();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">List</span>&lt;<span class="type param">T</span>&gt; <span class="method"><span class="static">AsList</span></span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">this</span> <span class="type">List</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">list</span>) <span class="operator">=&gt;</span> <span class="variable local">list</span>;
}
</pre>

この話はコレクション式に限らず、
[デリゲート](../functional/sp_delegate.md#natural-type)や[文字列補間](../start/improvedinterpolatedstring.md)でもそうなんですが、
C# 13 以降で取り組むそうです。

#### <a id="sec-generated-title-20"></a>Dictionary 式
`Dictionary<TKey, TValue>` などのキーを持つタイプのコレクションに対して、
以下のような構文で初期化できるようにしたいという案があって、これも C# 13 で検討中です。

<pre class="source" title="">
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">map</span> <span class="operator">=</span>
[
    <span class="string">&quot;one&quot;</span>:</span> <span class="number">1</span>,</span>
    <span class="string">&quot;two&quot;</span>: <span class="number">2</span>,
];
</pre>

#### <a id="sec-generated-title-21"></a>非ジェネリック コレクション
C# 12 では以下のようなコードに対応しなかったんですが、これも C# 13 で検討中です。

<pre class="source" title="非ジェネリック コレクション">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="type">ICollection</span> <span class="variable">c</span> <span class="operator">=</span> <span class="error" title="CS9174">[<span class="string">&quot;a&quot;</span>, <span class="number">2</span>, <span class="reserved">null</span>]</span>;
</pre>
