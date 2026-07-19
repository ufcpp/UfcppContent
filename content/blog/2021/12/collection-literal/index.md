---
title: "【C# 11 候補】コレクション リテラル"
source_url: "https://ufcpp.net/blog/2021/12/collection-literal/"
content_type: "BlogEntry"
published_at: "2021-12-31T19:42:44"
updated_at: "2021-12-31T19:42:44"
tags: []
umbraco_id: 2399
parent_id: 2375
sort_order: 12
aliases: []
---

# 【C# 11 候補】コレクション リテラル

今日は[リスト パターンの回でちょこっと出て来た `[]` リテラルの話](../list-pattern/index.md#collection-literal)。

逆に、リスト パターン側でも `{}` ではなく `[]` を使う決断に至った理由でもあります。

もう実装があるリスト パターンと違って、こちらはまだ案が出たてで、
もしかしたら C# 11 よりもさらに後になるかもしれないです。

## <a id="collection-literal">[] リテラルの導入</a>

元々、C# よりも後に世に出たり、大幅改修したことがあるプログラミング言語には結構「コレクション リテラル」系の文法があります。
で、多くの場合、`[ 1, 2, 3 ]` みたいに角括弧を利用。

そして現在の C# には `new[] { 1, 2, 3 }` みたいな書き方はあるにはあるものの、いろんなコレクション型があって、それぞれ書き方に統一感がない状態。

<pre class="source" title="C# のコレクションあれこれ">
<code><span class="comment">// 型を明示、かつ、配列の時に限り {} だけで OK。</span>
<span class="reserved">int</span>[] <span class="variable">array1</span> = { 1, 2, 3 };

<span class="comment">// 型推論を使いたければ new[] {}。</span>
<span class="reserved">var</span> <span class="variable">array2</span> = <span class="reserved">new</span>[] { 1, 2, 3 };

<span class="comment">// Target-typed new + コレクション初期化子。 () は省略不可。</span>
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list1</span> = <span class="reserved">new</span>() { 1, 2, 3 };

<span class="comment">// 通常の new + コレクション初期化子。こっちの場合は () 省略 OK。</span>
<span class="reserved">var</span> <span class="variable">list2</span> = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { 1, 2, 3 };

<span class="comment">// Span にはまあ、new で配列を割り当ててもいいものの、</span>
<span class="comment">// パフォーマンス的には stackalloc を使った方が大体の場合有利。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[] { 1, 2 };

<span class="comment">// ReadOnlySpan も同様。</span>
<span class="comment">// あと、stackalloc の後ろは型推論で省略可能。</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> = <span class="reserved">stackalloc</span>[] { 1, 2, 3 };

<span class="comment">// new() もコレクション初期化子も使えないかわいそうな型あり。</span>
<span class="reserved">var</span> <span class="variable">immutable</span> = System.Collections.Immutable.<span class="type">ImmutableArray</span>.<span class="method">Create</span>(1, 2, 3);
</code></pre>

C# でももう少し統一感あるコレクション リテラルがあった方がいいし、
だったら他の言語に倣って `[]` を使った新文法を導入でいいのではないかという話になります。

<pre class="source" title="[] をもっていろんなコレクションを初期化したい">
<code><span class="comment">// ぜんぶ [] にしたい。</span>
<span class="reserved">int</span>[] <span class="variable">array1</span> = [ 1, 2, 3 ];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list1</span> = [ 1, 2, 3 ];
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = [ 1, 2, 3 ];
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> = [ 1, 2 ];
System.Collections.Immutable.<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> = [ 1, 2, 3 ];
</code></pre>

そしてこっち(リテラル側)でも `[]` を使うのであれば、
[パターンの方](../list-pattern/index.md)で `{}` (プロパティ パターンと区別が付かない)とか `[]{}` (`new[]{}` との対称性はいいかもしれないもののキモい)とか考えず、そっちも素直に `[]` を使えばいいということに。

## <a id="spread">[] リテラル中の .. (spread 演算)</a>

[パターンの方](../list-pattern/index.md)で「`[1, ..[2, 3, 4], 5]` と `[1, 2, 3, 4, 5]` が同じ意味になる」と書きましたが、コレクション リテラル中でも同じく「入れ子のコレクションを展開」みたいな仕様があります。

<pre class="source" title=".. で入れ子のコレクションを展開">
<code><span class="reserved">int</span>[] <span class="variable">array</span> = [ 1, 2, 3 ];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> = [ 0, ..<span class="variable">array</span>, 4 ]; <span class="comment">// 0, 1, 2, 3, 4</span>
</code></pre>

他の言語で unpacking とか splat (* 記号が一部の人にそう呼ばれていて、この機能に * を使ってる言語ではこう呼ぶ)とか spread (拡散)演算子とか呼ばれているやつです。

C# ではまあ、LINQ の `Concat`, `Append`, `Prepend` とかを使って同様のものは書けていましたが、煩雑、かつ、パフォーマンスはいまいちでした。

<pre class="source" title="Concat, Append, Prepend">
<code><span class="reserved">int</span>[] <span class="variable">array1</span> = { 1, 2, 3 };
<span class="reserved">int</span>[] <span class="variable">array2</span> = { 4, 5, 6 };

<span class="comment">// enumerator のインスタンスが余計に new されたりで遅い。</span>
<span class="reserved">var</span> <span class="variable">linq</span> = <span class="variable">array1</span>.<span class="method">Concat</span>(<span class="variable">array2</span>).<span class="method">Prepend</span>(0).<span class="method">Append</span>(7);

<span class="comment">// 列挙も結構遅い。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">linq</span>)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
}

<span class="comment">// LINQ のよりも速い実装になる予定(後述)。</span>
<span class="comment">// かつ、Preapend よりはだいぶわかりやすい。</span>
<span class="reserved">var</span> <span class="variable">spread</span> = [ 0, .. <span class="variable">array1</span>, .. <span class="variable">array2</span>, 7 ];
</code></pre>

## <a id="brace">おまけ: {} 案</a>

一時期はパターンの方も `is {}` にしたいみたいな話もあったんですが。
元々配列初期化子が `{}` ですし、コレクション初期化子も `{}` になる案もなくはなかったです。

ただ、`{}` の用途としては他に [Expression blocks](https://github.com/dotnet/csharplang/issues/3086) という提案も出ていて、それとの弁別が無理そうということで没。

## <a id="lowering">展開結果</a>

展開結果、基本的には「前から順に詰める」です。
配列の場合だと割かしシンプルで、例えば以下のような感じ。

<pre class="source" title="配列に対するコレクション リテラルの展開結果">
<code><span class="reserved">int</span>[] <span class="variable">array1</span> = { 1, 2, 3 };
<span class="reserved">int</span>[] <span class="variable">array2</span> = { 4, 5, 6 };

<span class="comment">// var spread = [ 0, .. array1, .. array2, 7 ];</span>

<span class="reserved">var</span> <span class="variable">len</span> = 1 + <span class="variable">array1</span>.Length + <span class="variable">array2</span>.Length + 1;
<span class="reserved">var</span> <span class="variable">spread</span> = <span class="reserved">new</span> <span class="reserved">int</span>[<span class="variable">len</span>];

<span class="reserved">var</span> <span class="variable">i</span> = 0;
<span class="variable">spread</span>[<span class="variable">i</span>++] = 0;
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">j</span> = 0; <span class="variable">j</span> &lt; <span class="variable">array1</span>.Length; <span class="variable">j</span>++, <span class="variable">i</span>++) <span class="variable">spread</span>[<span class="variable">i</span>] = <span class="variable">array1</span>[<span class="variable">j</span>];
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">j</span> = 0; <span class="variable">j</span> &lt; <span class="variable">array2</span>.Length; <span class="variable">j</span>++, <span class="variable">i</span>++) <span class="variable">spread</span>[<span class="variable">i</span>] = <span class="variable">array2</span>[<span class="variable">j</span>];
<span class="variable">spread</span>[<span class="variable">i</span>] = 7;
</code></pre>

`Span<T>` の場合には `new T[]` のところを `stackalloc T[]` に変更。
`ReadOnlySpan<T>` の場合はいったん `Span<T>` と同じ処理でデータを書き込んでから、最後に `ReadOnlySpan<T>` に変換。

それ以外の型については「所定のパターンを満たすコンストラクターと `Init` メソッドを呼ぶ」と言うことになっています。

* `capacity` という名前の引数があるコンストラクターがある場合はそれを、なければ引数なしコンストラクターを呼ぶ
* `void Init(T1)` があって、`T1` が `T[]` なら `new[]` で、`T1` が `Span<T>`, `ReadOnlySpan<T>` なら `stackalloc[]` で一時バッファーを作ってから `Init` メソッドに渡す

例えば `Init(int[])` だけ持っている型だと以下のような感じ。

<pre class="source" title="一時 new int[] が作られるパターン">
<code><span class="comment">// A a = [ 1, 2, 3 ];</span>
<span class="reserved">int</span>[] <span class="variable">tempA</span> = { 1, 2, 3 };
<span class="type">A</span> <span class="variable">a</span> = <span class="reserved">new</span>();
<span class="variable">a</span>.<span class="method">Init</span>(<span class="variable">tempA</span>);

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Init</span>(<span class="reserved">int</span>[] <span class="variable">items</span>) { }
}
</code></pre>

`capacity` コンストラクターと `Init(ReadOnlySpan<int>)` を持つ型だと以下のような感じ。

<pre class="source" title="一時 stackalloc int[] が作られるパターン">
<code><span class="comment">// A a = [ 1, 2, 3 ];</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">tempA</span> = <span class="reserved">stackalloc</span>[] { 1, 2, 3 };
<span class="type">A</span> <span class="variable">a</span> = <span class="reserved">new</span>(3);
<span class="variable">a</span>.<span class="method">Init</span>(<span class="variable">tempA</span>);

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable">capacity</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Init</span>(<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">items</span>) { }
}
</code></pre>

## immutable コレクション初期化

ちょっと別の機能追加も必要なのでさらに不透明なんですが、
この `[]` リテラルは前に話した [`ImmutableArray` の初期化問題](../immutable-array-init/index.md)の解決策としても期待されています。

とりあえず、`ImmutableArray` についても前節と同じルールで初期化を掛けることを考えます。

<pre class="source" title="ImmutableArray.Init">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="comment">// ImmutableArray&lt;int&gt; a = [ 1, 2, 3 ];</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">tempA</span> = <span class="reserved">stackalloc</span>[] { 1, 2, 3 };
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> = <span class="reserved">new</span>();
<span class="variable">a</span>.Init(<span class="variable">tempA</span>); <span class="comment">// こういうメソッドを足したいという話。今はない。</span>
</code></pre>

こういう `Init` メソッドを足せればいいわけですが、
immutable を名乗る以上、`new()` とは別に呼ばれるとまずいという話になります。

で、そこは[init-only プロパティ](../../../../study/csharp/oop/oo_property.md#init-only-internal)と同じ方式で乗り切りたいとのこと。

任意のメソッドに対して、`new()` 中、もしくは、直後にしか呼ばない・呼ばれない保証をコンパイラーがするような仕様(メソッドに対する `init` 修飾)があればいいわけで、そういう仕様も模索中とのこと。

<pre class="source" title="init 修飾子">
<code><span class="reserved">struct</span> <span class="type">ImmutableArray</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">readonly</span> <span class="type">T</span>[] _items;

    <span class="comment">// init 修飾を付けたメソッドは new() 内、もしくは、直後でしか呼べないように、</span>
    <span class="comment">// コンパイラーが呼び出し箇所をチェックする。</span>
    <span class="reserved">public</span> <span class="reserved">init</span> <span class="reserved">void</span> <span class="method">Init</span>(<span class="type">ReadOnlySpan</span>&lt;<span class="type">T</span>&gt; <span class="variable">items</span>)
    {
        <span class="comment">// 本来、コンストラクター内でしか書き換えてはいけないはずのフィールドを、</span>
        <span class="comment">// init 修飾子が付いたメソッド内に限り書き換え可能にする。</span>
        _items = <span class="variable">items</span>.<span class="method">ToArray</span>();
    }
}
</code></pre>
