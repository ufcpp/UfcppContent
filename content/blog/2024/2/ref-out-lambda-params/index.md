---
title: "ラムダ式の引数で、型名を省略して ref, out などだけを指定"
source_url: "https://ufcpp.net/blog/2024/2/ref-out-lambda-params/"
content_type: "BlogEntry"
published_at: "2024-02-25T22:53:14"
updated_at: "2024-02-25T22:53:14"
tags: []
umbraco_id: 2489
parent_id: 2480
sort_order: 8
aliases: []
---

# ラムダ式の引数で、型名を省略して ref, out などだけを指定

ラムダ式で、`ref` 引数などに対して `ref x => { }` みたいに書けるようにしたいという話が出ています。

## ラムダ式での ref 引数、out 引数

ラムダ式は、状況が許すなら、`x => { }` などといったように非常に簡素に書けます。
ところが、[`ref`](../../../../study/csharp/resource/sp_ref.md#sec-byref) や [`out`](../../../../study/csharp/resource/sp_ref.md#out) が絡むとそうもいかなくて、型推論が効く状況でも型名を省略できません。

<pre class="source" title="ref, out などが絡むと型名の省略ができない">
<span class="comment">// 通常、ラムダ式は型推論が効く限り、引数の型を省略できる。</span>
<span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">=&gt;</span> { };

<span class="comment">// ところが ref, out などの修飾が付いた引数は省略不可。</span>

<span class="comment">// これなら OK。</span>
<span class="type">RefAction</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">r</span> <span class="operator">=</span> (<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> { };
<span class="type">OutFunc</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">o</span> <span class="operator">=</span> (<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="comment">// ダメ。CS1676 エラー。</span>
<span class="type">RefAction</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">r1</span> <span class="operator">=</span> <span class="error" title="CS1676"><span class="variable local">x</span></span> <span class="operator">=&gt;</span> { };
<span class="type">OutFunc</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">o1</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS1676">x</span></span> <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">RefAction</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">ref</span> <span class="type param">T</span> <span class="variable local">arg</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">OutFunc</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">arg</span>);
</pre>

特に、「他にも引数が多かったり、他の引数のどれかに型名が長くて書きたくない引数がある」みたいな状況では相当に不便です。

<pre class="source" title="引数が多くて型の省略したい…">
<span class="comment">// 全部の引数に型の明示が必要。</span>
<span class="type">ManyParams</span> <span class="variable">a</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>, <span class="reserved">int</span> <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>) <span class="operator">=&gt;</span> { };

<span class="comment">// r の型は省略できない。</span>
<span class="type">ManyParams</span> <span class="variable">a1</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="variable local">z</span>, <span class="error" title="CS1676"><span class="variable local">r</span></span>) <span class="operator">=&gt;</span> { };

<span class="comment">// 「部分的に型を明示」というのも書けない。</span>
<span class="type">ManyParams</span> <span class="variable">a2</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="reserved"><span class="error" title="CS0748">int</span></span> <span class="variable local"><span class="error" title="CS1676">r</span></span>) <span class="operator">=&gt;</span> { };

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">ManyParams</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>, <span class="reserved">int</span> <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>);
</pre>

<pre class="source" title="型名が長くて省略したい…">
<span class="comment">// 全部の引数に型の明示が必要。</span>
<span class="type">LongTypeName</span> <span class="variable">a</span> <span class="operator">=</span> (<span class="type">IReadOnlyDictionary</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y), <span class="type">List</span>&lt;<span class="reserved">string</span>[,]&gt;&gt; <span class="variable local">x</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>) <span class="operator">=&gt;</span> { };

<span class="comment">// r の型は省略できない。</span>
<span class="type">LongTypeName</span> <span class="variable">a1</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="variable local"><span class="error" title="CS1676">r</span></span>) <span class="operator">=&gt;</span> { };

<span class="comment">// 「部分的に型を明示」というのも書けない。</span>
<span class="type">LongTypeName</span> <span class="variable">a2</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="reserved">ref</span> <span class="reserved"><span class="error" title="CS0748">int</span></span> <span class="variable local"><span class="error" title="CS1676">r</span></span>) <span class="operator">=&gt;</span> { };

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">LongTypeName</span>(<span class="type">IReadOnlyDictionary</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y), <span class="type">List</span>&lt;<span class="reserved">string</span>[,]&gt;&gt; <span class="variable local">x</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>);
</pre>

これに対して、`ref x => { }` みたいな書き方は認めてもいいんじゃない？という話があります。

<pre class="source" title="ref x, out x なら型名省略できてもいいのでは？">
<span class="comment">// 現状ダメ。でも、これくらいはできてもいいのでは？</span>
<span class="type">RefAction</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">r</span> <span class="operator">=</span> (<span class="reserved">ref</span> <span class="error" title="CS0246">x</span><span class="error" title="CS1001">)</span> <span class="operator">=&gt;</span> { };
<span class="type">OutFunc</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">o</span> <span class="operator">=</span> (<span class="reserved">out</span> <span class="error" title="CS0246">x</span><span class="error" title="CS1001">)</span> <span class="operator">=&gt;</span> <span class="error" title="CS0177">x <span class="operator">=</span> <span class="number">1</span></span>;
<span class="type">ManyParams</span> <span class="variable">m</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="error" title="CS0118"><span class="variable">r</span></span>) <span class="operator">=&gt;</span> { };
<span class="type">LongTypeName</span> <span class="variable">l</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="reserved">ref</span> <span class="variable"><span class="error" title="CS0118">r</span></span>) <span class="operator">=&gt;</span> { };

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">RefAction</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">ref</span> <span class="type param">T</span> <span class="variable local">arg</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">OutFunc</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">arg</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">ManyParams</span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>, <span class="reserved">int</span> <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">LongTypeName</span>(<span class="type">IReadOnlyDictionary</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y), <span class="type">List</span>&lt;<span class="reserved">string</span>[,]&gt;&gt; <span class="variable local">x</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">r</span>);
</pre>

## コミュニティ提案

実際アイディア自体は2015年くらいからずっとあります。

* [Declaration of ref/out parameters in lambdas without typename #338](https://github.com/dotnet/csharplang/issues/338)

`ref` 引数ラムダ式とか自体が使用頻度低めなのでそれほど優先度はついておらず、
ずっと「Any Time」(C# チーム自らはやらず、「コミュニティ貢献お待ちしております」状態)でした。

これに対して、去年くらいに実際、コミュニティからの提案ドキュメントが上がっていました。

* [Declaration of lambda parameters with modifiers without type name](https://github.com/dotnet/csharplang/blob/main/proposals/ref-out-lambda-params.md)

[履歴](https://github.com/dotnet/csharplang/commits/main/proposals/ref-out-lambda-params.md))を見るに、2023年7～8月くらいにコミュニティから提案されていて、C# 12 作業中は進捗なし。
今月に入ってから C# チームの中の人が引き取って検討を始めていそうな感じですね。

そして[数日前の Language Design Meeting](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md) で議題に。
とりあえず提案は承認されたみたいです。

## その他検討事項

Desing Meeting では対案も2点ほど検討されたんですがそちらはリジェクト。
元の提案の方向で受け付けるみたいです。

対案その1は、`x => { }` だけで `ref`/`out` も「推論」してもいいのでは？という案。
ただ、C# の `ref` 引数、`out` 引数は、呼び出し元にも `ref`/`out` の明示を求めるくらいなので、さすがに `x => { }` というような書き方はちょっと C# 的には違和感があります(なのでリジェクト)。

<pre class="source" title="呼び出し元にも ref/out の明示が必須">
<span class="type">RefAction</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">r</span> <span class="operator">=</span> (<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> { };
<span class="type">OutFunc</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">o</span> <span class="operator">=</span> (<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="comment">// ref, out 引数は呼び出し側にも ref, out を書く必要があるくらい明示を求められる。</span>
<span class="comment">// 呼び出し先で書き変わるのは明示されないと怖い。</span>
<span class="reserved">int</span> <span class="variable">local</span>;
<span class="variable">o</span>(<span class="reserved">out</span> <span class="variable">local</span>);
<span class="variable">r</span>(<span class="reserved">ref</span> <span class="variable">local</span>);

<span class="comment">// なのでラムダ式側でも ref, out は書かないと違和感。</span>
<span class="comment">// 以下のような書き方は今後も乗り気ではない。</span>
<span class="type">RefAction</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">r1</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS1676">x</span></span> <span class="operator">=&gt;</span> { };
<span class="type">OutFunc</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">o1</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS1676">x</span></span> <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">RefAction</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">ref</span> <span class="type param">T</span> <span class="variable local">arg</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">OutFunc</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">arg</span>);
</pre>

対案その2は前述の `ManyParams` とか `LongTypeName` とかの例で書いたような、「引数の一部分の型名を省略、一部分を明示」です。
ただ、これは「`r` に指定した型から `x` の型を推論」みたいな別の要望が加わるだろうことと、
それをやると[部分型推論](../partial-inference/index.md)の話と同様、
推論を頑張ろうとすると指数的なコンパイル時間になってしまう可能性があってちょっと怖いそうです(なのでリジェクト、やるとしても部分型推論と一緒に)。

<pre class="source" title="ラムダ式引数の部分型指定は型推論が複雑になりそう">
<span class="comment">// ラムダ式引数の部分型指定 + 型引数の推論。</span>
<span class="comment">// 結構推論機構が複雑になるはず。</span>
<span class="reserved">static</span> <span class="type">ManyParams</span>&lt;<span class="type param">T</span>&gt; <span class="method"><span class="warning" title="CS8321"><span class="static">Create</span></span></span>&lt;<span class="type param">T</span>&gt;(<span class="type">ManyParams</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">a</span>) <span class="operator">=&gt;</span> <span class="variable local">a</span>;
<span class="reserved">var</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="static"><span class="method"><span class="error" title="CS0411">Create</span></span></span>((<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="error" title="CS0748"><span class="reserved">int</span></span> <span class="variable local">r</span>) <span class="operator">=&gt;</span> { });

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">ManyParams</span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">x</span>, <span class="type param">T</span> <span class="variable local">y</span>, <span class="type param">T</span> <span class="variable local">z</span>, <span class="reserved">ref</span> <span class="type param">T</span> <span class="variable local">r</span>);
</pre>

あと、元の提案に残っていた「属性や、引数のデフォルト値はどうしよう？」という未解決の議題についても「大変そうなわりに需要がない」ということで、やらないことになりそうです。

<pre class="source" title="属性やデフォルト値が付いているときの型省略">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// C# 10 と 12 で、こんな感じで属性を付けたりデフォルト値を指定できるようになった。</span>
<span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> <span class="operator">=</span> <span class="warning" title="CS8622">([<span class="type">MaybeNull</span>] <span class="reserved">string</span> <span class="variable local"><span class="warning" title="CS9099">s</span></span> <span class="operator">=</span> <span class="warning" title="CS8625"><span class="reserved">null</span></span>) <span class="operator">=&gt;</span></span> <span class="variable local">s</span><span class="operator">?</span><span class="operator">.</span><span class="property">Length</span> <span class="operator">??</span> <span class="number">0</span>;

<span class="comment">// これに対して型名省略したい？</span>
<span class="comment">// (そんなに需要なさそうな割に、これを実装するのは大変そう。)</span>
<span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">f1</span> <span class="operator">=</span> <span class="warning" title="CS8622">([<span class="type">MaybeNull</span>] <span class="variable local"><span class="error" title="CS9098">s</span></span> <span class="operator">=</span> <span class="reserved">null</span>) <span class="operator">=&gt;</span></span> <span class="variable local">s</span><span class="operator">?</span><span class="operator">.</span><span class="property">Length</span> <span class="operator">??</span> <span class="number">0</span>;
</pre>
