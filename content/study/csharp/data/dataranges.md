---
title: "インデックス/範囲処理"
source_url: "https://ufcpp.net/study/csharp/data/dataranges/"
content_type: "Article"
published_at: "2019-06-02T00:00:00"
updated_at: "2023-03-05T15:39:08"
tags: []
umbraco_id: 2246
parent_id: 1298
sort_order: 14
aliases:
  - "/csharp/data/dataranges/"
---

# インデックス/範囲処理

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、配列などに対して以下のような書き方をできるようになります。

- `a[^i]` で「後ろからi番目の要素」を参照
- `a[i..j]` で「i番目からj番目の範囲」を参照

例えば、以下のような書き方で配列の前後1要素ずつを削ったものを得ることができます。

<pre class="source" title=".. 構文">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
 
        <span class="comment">// 前後1要素ずつ削ったもの</span>
        <span class="reserved">var</span> <span class="variable">middle</span> = <span class="variable">a</span>[1..^1];
 
        <span class="comment">// 2, 3, 4 が表示される</span>
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">middle</span>)
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>);
        }
    }
}
</code></pre>

ちなみに、`i..j` は「iは含んでjは含まない」という範囲になります。
`for (var x = i; x < j; ++x)` のイメージ。

より細かく言うと、以下のような機能の組み合わせになります。

- `^i` で「後ろからi番目」を表す `Index` 型の値を得る
- `i..j` で「i番目からj番目」を表す `Range` 型の値を得る
- 所定の条件を満たす型に対して `Index`/`Range` を渡すと、所定のパターンに展開する

いくつかのプログラミング言語で似たような構文があり、
多くの場合は range (範囲)構文と呼ばれます。
C# 8.0 で導入されたものは配列などのインデックス用途に特化していて、
`Index`型と`Range`型からなるので、index/range (インデックス/範囲)構文と言ったりもします。

##<a id="sec-generated-title-2"></a> <a id="background"></a>背景
###<a id="sec-generated-title-3"></a> <a id="span"></a>Span
C# 7.2 で、[`Span<T>` 構造体](../resource/span.md)が導入されました。
配列や文字列中の一定範囲を抜き出して効率的に読み書きするための型です。
(単純な機能なのでもっと昔からあってもよさそうなものですが、
[ガベージ コレクション](../../computer/essential-software/memorymanagement.md#garbage-collection)があっても[安全かつ高速に](../resource/span.md#two-implementations)動くようにするのが意外と大変で、C# 7.2 まで導入が見送られていました。)

「[配列のインデクサー](../../../blog/2018/12/arrayindexer/index.md)」というブログで書いたことがあるんですが、
`Span<T>` 構造体は特別な最適化の対象になっていて、非常に高速です。
例えば以下の2つのメソッドでは、`Span<T>` を使った `Sum2` の方が高速です。

<pre class="source" title="Span を使うと速い">
<code><span class="comment">// i番目からj番目までの和。</span>
[<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.NoInlining)]
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Sum1</span>(<span class="reserved">int</span>[] <span class="variable">array</span>, <span class="reserved">int</span> <span class="variable">i</span>, <span class="reserved">int</span> <span class="variable">j</span>)
{
    <span class="reserved">var</span> <span class="variable">sum</span> = 0;
    <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">x</span> = <span class="variable">i</span>; <span class="variable">x</span> &lt; <span class="variable">j</span>; <span class="variable">x</span>++) <span class="variable">sum</span> += <span class="variable">array</span>[<span class="variable">x</span>];
    <span class="control">return</span> <span class="variable">sum</span>;
}
 
<span class="comment">// Sum1 と同じ処理を Span を使って書く。</span>
<span class="comment">// Sum1 よりこっちの方が速い。</span>
[<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.NoInlining)]
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Sum2</span>(<span class="reserved">int</span>[] <span class="variable">array</span>, <span class="reserved">int</span> <span class="variable">i</span>, <span class="reserved">int</span> <span class="variable">j</span>)
{
    <span class="reserved">var</span> <span class="variable">sum</span> = 0;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">array</span>.<span class="method">AsSpan</span>()[<span class="variable">i</span>..<span class="variable">j</span>]) <span class="variable">sum</span> += <span class="variable">x</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="unification"></a>範囲のルール統一
配列の一定範囲を抜き出すという処理は、`array.AsSpan(x, y)` というように、単なるメソッド呼び出しでもできます。
ただ、ここで問題となるのは、引数の意味がメソッドによってぶれている点です。
`x`、`y` にそれぞれ3、5を渡した場合、どういう意味になるでしょう。
例えば、以下のようなパターンが考えられます。

- (1) 3, 4, 5 (3から5まで、3も5も含む)
- (2) 3, 4 (3から5まで、5は含まない)
- (3) 3, 4, 5, 6, 7 (3から5要素)

実際、 .NET の標準ライブラリ中でもぶれています。
例えば、[Parallel.For](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.tasks.parallel.for?view=netstandard-2.1#System_Threading_Tasks_Parallel_For_System_Int32_System_Int32_System_Action_System_Int32__)や[`Random.Next`](https://docs.microsoft.com/ja-jp/dotnet/api/system.random.next?view=netframework-4.8#System_Random_Next_System_Int32_System_Int32_) は (2) の意味ですが、[`Substring`](https://docs.microsoft.com/ja-jp/dotnet/api/system.string.substring?view=netstandard-2.1#System_String_Substring_System_Int32_System_Int32_)や[`AsSpan`](https://docs.microsoft.com/ja-jp/dotnet/api/system.memoryextensions.asspan?view=netstandard-2.1#System_MemoryExtensions_AsSpan__1___0___System_Int32_System_Int32_)は (3) の意味です。

<pre class="source" title="範囲指定の引数のぶれ">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// この2つは 1から3 (3は含まない) = 1, 2の意味</span>
        <span class="type">Parallel</span>.<span class="method">For</span>(1, 3, <span class="variable">i</span> =&gt; { });
        <span class="reserved">var</span> <span class="variable">v</span> = <span class="reserved">new</span> <span class="type">Random</span>().<span class="method">Next</span>(1, 3);
 
        <span class="comment">// この2つは 1から3要素 = 1, 2, 3 の意味</span>
        <span class="reserved">var</span> <span class="variable">span</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 }.<span class="method">AsSpan</span>(1, 3);
        <span class="reserved">var</span> <span class="variable">substr</span> = <span class="string">&quot;abcde&quot;</span>.<span class="method">Substring</span>(1, 3);
    }
}
</code></pre>

名前付き引数を使えば、多少混乱を予防することはできます。

<pre class="source" title="名前付き引数で混乱を予防">
<code><span class="type">Parallel</span>.<span class="method">For</span>(<span class="variable">fromInclusive</span>: 1, <span class="variable">toExclusive</span>: 3, <span class="variable">i</span> =&gt; { });
<span class="reserved">var</span> <span class="variable">v</span> = <span class="reserved">new</span> <span class="type">Random</span>().<span class="method">Next</span>(<span class="variable">minValue</span>: 1, <span class="variable">maxValue</span>: 3);
<span class="reserved">var</span> <span class="variable">span</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 }.<span class="method">AsSpan</span>(<span class="variable">start</span>: 1, <span class="variable">length</span>: 3);
<span class="reserved">var</span> <span class="variable">substr</span> = <span class="string">&quot;abcde&quot;</span>.<span class="method">Substring</span>(<span class="variable">startIndex</span>: 1, <span class="variable">length</span>: 3);
</code></pre>

ただ、名前付き引数を使っても以下の問題があります。

- コードがとにかく長くなる
- `Random.Next` のように「含むか含まないか」を明示していないやつがいる
- あくまで実装者の良心頼みになっている
- 多次元データだと `matrix[1, 3, 1, 3]` みたいにさらにわかりにくい

そこで、範囲を表す専用の構文が欲しいわけです。
構文になっていれば意味がぶれることがなくなります。
C# では、`i..j` で「i番目からj番目(j は含まない)」となる構文を採用しました。

###<a id="sec-generated-title-5"></a> <a id="index-usage"></a>インデックス用途
`i..j` と書いたとき、j を含むかどうかは難しい問題です。
実際、あるプログラミング言語では j を含みますし、別のある言語では含みません。
`..=` や `..<` などで含む・含まないを選ぶようになっている言語もありますが、
`..` だけを書く構文もあったりして、その `..` の意味は言語ごとにまちまちです。

用途次第でもあります。
「この範囲に入っているかどうかを判定」みたいな用途(要するに[パターン マッチング](../datatype/patterns.md))だと、末尾も含んでくれている方がわかりやすいです。
一方で、`Span`や`Substring`のように、配列や文字列から一定範囲を抜き出す用途(インデックス用途)では、末尾を含まない方が使いやすかったりします。

インデックス用途での「末尾を含まない」には以下のようなメリットがあります。

- `j - i` だけで長さを計算できる
- ループで使いやすい
  - ループでは `for (int x = i; x < j; ++x)` というように `<` で条件判定することが多い
- `i..i` (先頭と末尾が同じ)が不正にならない(単に長さ0の範囲になる)
  - 逆に「j を含む」を採用する場合、長さ0の範囲は `i..(i-1)` と書く必要がある

C# の `i..j` で「j は含まない」の方を採用したのは、明確にインデックス用途を意図したものです。

##<a id="sec-generated-title-6"></a> <a id="index"></a>Index
配列や文字列からの一定範囲の抜き出しではよく「末尾から i 番目」という場所を取りたいことがあります。
C# 8.0 では、そのために単項 `^` 演算子を使います。

<pre class="source" title="^ 演算子">
<code><span class="reserved">var</span> <span class="variable">i</span> = ^1; <span class="comment">// Length - 1 の場所</span>
 
<span class="reserved">var</span> <span class="variable">value</span> = 1;
<span class="reserved">var</span> <span class="variable">j</span> = ^<span class="variable">value</span>; <span class="comment">// 変数に対しても ^ を使える</span>
</code></pre>

単項 `^` 演算子はオペランドに `int` (か `int` に暗黙に変換できる型)しか受け付けません。
また、戻り値は `Index` 構造体(`System` 名前空間)になります。
`Index` は、以下のようなプロパティ・メソッドを持つ構造体です。

<pre class="source" title="Index 構造体">
<code><span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Index</span>
{
    <span class="reserved">public</span> <span class="type">Index</span>(<span class="reserved">int</span> <span class="variable">value</span>, <span class="reserved">bool</span> <span class="variable">fromEnd</span> = <span class="reserved">false</span>);
    <span class="reserved">public</span> <span class="reserved">bool</span> IsFromEnd { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetOffset</span>(<span class="reserved">int</span> <span class="variable">length</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">Index</span>(<span class="reserved">int</span> <span class="variable">value</span>)
}
</code></pre>

`^i` は `new Index(i, true)` に展開されます
(第2引数の `true` が「末尾から」の意味です)。
`int` からの暗黙的な変換もあって、それは素直に「先頭から i 番目」の意味になります。

###<a id="sec-generated-title-7"></a> <a id="non-negative"></a>補足: インデックスは0以上の整数
C# では、[配列のインデックスは0以上(非負)という前提](../../../blog/2018/12/arrayindex/index.md)があります。
なので、`Index` 構造体も以下のような作りになっています。

- コンストラクターに負の整数を渡すと `IndexOutOfRange` 例外が発生する
  - `^-1` みたいな書き方は文法的には認められるものの、実行時に例外発生
- 内部的には `int` 1つだけ持っていて、負の数を「末尾から」の意味で使っている
  - 構造体のサイズは `int` と同じ4バイト

##<a id="sec-generated-title-8"></a> <a id="range"></a>Range
C# 8.0 で `..` という新しい構文が追加されました。

<pre class="source" title=".. 構文">
<code><span class="reserved">var</span> <span class="variable">r1</span> = 1..^1;
<span class="reserved">var</span> <span class="variable">r2</span> = 1..;
<span class="reserved">var</span> <span class="variable">r3</span> = ..^1;
<span class="reserved">var</span> <span class="variable">r4</span> = ..;
 
<span class="reserved">var</span> <span class="variable">i</span> = 1;
<span class="reserved">var</span> <span class="variable">j</span> = ^1;
<span class="reserved">var</span> <span class="variable">r</span> = <span class="variable">i</span>..<span class="variable">j</span>;
</code></pre>

他の2項演算子と違って、`i..` や `..j`、`..` というようにオペランドを省略できます。
オペランドは `Index` 型か、(`int` を含む) `Index` 型に暗黙的に変換できる型である必要があります。
戻り値は `Range` 型(`System` 名前空間)になります。
`Range` は、以下のようなプロパティ・メソッドを持つ構造体です。

<pre class="source" title="Range 構造体">
<code><span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Range</span>
{
    <span class="reserved">public</span> <span class="type">Range</span>(<span class="type">Index</span> <span class="variable">start</span>, <span class="type">Index</span> <span class="variable">end</span>);
    <span class="reserved">public</span> <span class="type">Index</span> Start { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Index</span> End { <span class="reserved">get</span>; }
    <span class="reserved">public</span> (<span class="reserved">int</span> Offset, <span class="reserved">int</span> Length) <span class="method">GetOffsetAndLength</span>(<span class="reserved">int</span> <span class="variable">length</span>);
}
</code></pre>

左オペランドの省略時は先頭から、右オペランドの省略時は末尾までの意味になります。
すなわち、`i..` は `i..^0` と、`..j` は `0..j` と、`..` は `0..^0` と同じ意味です。
また、`i..j` は `new Range(i, j)` に展開されます。

名前通り、`Start` が開始位置で、`End` が末尾位置です。
コンストラクターの引数は、第1、第2引数がそれぞれ `Start`、`End` と対応しています。
これまでの説明通り、`Start` は「含む」、`End`は「含まない」という扱いです。

この辺りは言葉で説明してもわかりにくいと思うので、以下の図を参考にしてください。

![Index/Range の意味](../../../../assets/media/1174/ranges.png)

`i..^j` で、先頭からi要素、末尾からj要素を削った範囲になります。

ちなみに、演算子の優先順位は結構高いです。
2項演算(乗除算含む)や [`switch` 式](../datatype/typeswitch.md#switch-expression)よりも上になります。

<pre class="source" title=".. の優先順位">
<code>_ = <span class="error">2 * 3..4</span>; <span class="comment">// 2 * (3..4) の意味。そんな掛け算はできないのでコンパイル エラーに。</span>
_ = 2..3 <span class="control">switch</span> <span class="comment">// 2..3 という Range が switch 式の引数になる</span>
{
    <span class="type">Range</span> <span class="reserved">_</span> =&gt; 4,
};
_ = (1 + 2)..(3 + 4); <span class="comment">// 足し算とかを優先したければ () 必須</span>
</code></pre>

##<a id="sec-generated-title-9"></a> <a id="indexer"></a>Index/Range とインデクサー
`Index`/`Range`型に対するインデクサーは、
以下で説明するように、
一定のパターンで `int` に対するインデクサーや`Slice`メソッドに展開されます。

(当初予定では、`^i`から`Index`型を、`i..j`から`Range`型を作るところまでだけが C# コンパイラーの仕事で、それを使ったインデクサーは使う側(配列や `List<T>`などのコレクションの側)の仕事にする予定でした。
それだとあらゆるコレクションに対して1個1個インデクサーのオーバーロードを追加する作業が大変なのと、最適化が掛けにくいという理由で、現状のパターン ベースな方式に変更されました。)

`Index`型の `i` に対するインデクサー `a[i]` は基本的に以下のように展開されます。

<pre class="source" title="Index 型インデクサーの展開結果">
<code><span class="reserved">int</span> <span class="variable">offset</span> = <span class="variable">i</span>.<span class="method">GetOffset</span>(<span class="variable">a</span>.Length);
<span class="variable">a</span>[<span class="variable">offset</span>];
</code></pre>

また、`Range` 型の `r` に対するインデクサー `a[r]` は基本的に以下のように展開されます。

<pre class="source" title="Range 型インデクサーの展開結果">
<code><span class="reserved">var</span> <span class="variable">offset1</span> = <span class="variable">r</span>.Start.<span class="method">GetOffset</span>(<span class="variable">a</span>.Length);
<span class="reserved">var</span> <span class="variable">offset2</span> = <span class="variable">r</span>.End.<span class="method">GetOffset</span>(<span class="variable">a</span>.Length);
<span class="variable">a</span>.Slice(<span class="variable">offset1</span>, <span class="variable">offset2</span> - <span class="variable">offset1</span>);
</code></pre>

`a` の型によって多少バリエーションがあります。
C# のコレクションは長さを `Length` で取るものと `Count` で取るものの両方あるので、
そのどちらにも対応しています。`Length` がなくて `Count` がある場合それを使います
(`Length` があるならそっちが優先)。

<pre class="source" title="Index 型インデクサーの展開結果(Count)">
<code><span class="reserved">int</span> <span class="variable">offset</span> = <span class="variable">i</span>.<span class="method">GetOffset</span>(<span class="variable">a</span>.<em>Count</em>);
<span class="variable">a</span>[<span class="variable">offset</span>];
</code></pre>

<pre class="source" title="Range 型インデクサーの展開結果(Count)">
<code><span class="reserved">var</span> <span class="variable">offset1</span> = <span class="variable">r</span>.Start.<span class="method">GetOffset</span>(<span class="variable">a</span>.<em>Count</em>);
<span class="reserved">var</span> <span class="variable">offset2</span> = <span class="variable">r</span>.End.<span class="method">GetOffset</span>(<span class="variable">a</span>.<em>Count</em>);
<span class="variable">a</span>.Slice(<span class="variable">offset1</span>, <span class="variable">offset2</span> - <span class="variable">offset1</span>);
</code></pre>

また、`Range` 型インデクサーには、配列と文字列の場合だけ特別扱いがあります。
`Slice` メソッドではなく、それぞれ `GetSubArray`、`Substring` メソッドが呼ばれます
(`GetSubArray`は`RuntimeHelpers`クラス(`System.Runtime.CompilerServices` 名前空間)の静的メソッド)。

###<a id="sec-generated-title-10"></a> <a id="avoid-copy"></a>コピーの回避
配列と文字列に対する `Range`型インデクサー `a[i..j]` 
(展開結果的には `GetSubArray` と `Substring`)は、
それぞれ配列、文字列を返します。
この際、新しい配列・文字列を確保してコピーするコストが発生します。

<pre class="source" title="Range型インデクサーでコピー発生">
<code><span class="reserved">var</span> <span class="variable">array</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
<span class="reserved">var</span> <span class="variable">str</span> = <span class="string">&quot;abcde&quot;</span>;
 
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 100; <span class="variable">i</span>++)
{
    <span class="comment">// こういう書き方をすると、ループのたびに new int[], new string が発生。</span>
    <span class="comment">// だいぶ重たい。</span>
    <span class="reserved">var</span> <span class="variable">subarray</span> = <span class="variable">array</span>[1..^1];
    <span class="reserved">var</span> <span class="variable">substr</span> = <span class="variable">str</span>[1..^1];
}
</code></pre>

これらはそれなりに重たい処理なので、パフォーマンスにシビアな状況での利用には注意が必要です。

コピーを発生させたくない場合、[`Span<T>`](../resource/span.md)を経由します。
要するに、`AsSpan()` や `AsMemory()` を挟めばコピーを回避できます。

<pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">array</span> = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
<span class="reserved">var</span> <span class="variable">str</span> = <span class="string">&quot;abcde&quot;</span>;
 
<span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; 100; <span class="variable">i</span>++)
{
    <span class="comment">// 以下の書き方をすれば Span&lt;int&gt;/ReadOnlySpan&lt;char&gt; の Slice が呼ばれるようになる。</span>
    <span class="comment">// これならコピーは発生せず、軽い。</span>
    <span class="reserved">var</span> <span class="variable">subarray</span> = <span class="variable">array</span><em>.<span class="method">AsSpan</span>()</em>[1..^1];
    <span class="reserved">var</span> <span class="variable">substr</span> = <span class="variable">str</span><em>.<span class="method">AsSpan</span>()</em>[1..^1];
}
</code></pre>

##### <a id="sec-generated-title-11"></a>サンプル
「一定範囲を抜き出す」という処理は、テキスト処理でよく使います。

例として、書式が決まっているテキストの中から一部分を取り出してみましょう。
今回は「1行1項目で、`:` 区切りでキーと値が並んでいる」というような書式を考えます。
この書式のテキストの中からキーだけを取り出すようなコードを以下のように書けます。

<pre class="source" title="書式が決まったテキストから一部分を抜き出す例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">testData</span> = <span class="string">@&quot;longitude: 139.8803943
latitude: 35.6328964
postal code: 279-0031
&quot;</span>;
 
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">key</span> <span class="control">in</span> <span class="method">GetKeys</span>(<span class="variable">testData</span>))
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">key</span>);
        }
    }
 
    <span class="comment">// 行頭から : までの間の文字列だけを抜き出す</span>
    <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">ReadOnlyMemory</span>&lt;<span class="reserved">char</span>&gt;&gt; <span class="method">GetKeys</span>(<span class="reserved">string</span> <span class="variable">content</span>)
    {
        <span class="reserved">var</span> <span class="variable">start</span> = 0;
        <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; <span class="variable">content</span>.Length; <span class="variable">i</span>++)
        {
            <span class="reserved">var</span> <span class="variable">c</span> = <span class="variable">content</span>[<span class="variable">i</span>];
            <span class="control">if</span> (<span class="variable">c</span> == <span class="string">&#39;:&#39;</span>)
            {
                <span class="control">yield</span> <span class="control">return</span> <span class="variable">content</span>.<span class="method">AsMemory</span>()[<span class="variable">start</span>..<span class="variable">i</span>];
            }
            <span class="control">else</span> <span class="control">if</span> (<span class="variable">c</span> == <span class="string">&#39;\n&#39;</span>)
            {
                <span class="variable">start</span> = <span class="variable">i</span> + 1;
            }
        }
    }
}
</code></pre>
<pre class="console" title="書式が決まったテキストから一部分を抜き出す例">
<code>longitude
latitude
postal code
</code></pre>

例なのでシンプルな書式にしましたが、もうちょっと実用的な、例えば JSON 形式からのキーの取り出しなども、こういうコードの延長線上になります。
