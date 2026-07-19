---
title: "(没) 数学準拠な剰余演算子"
source_url: "https://ufcpp.net/blog/2024/12/euclidean-mod/"
content_type: "BlogEntry"
published_at: "2024-12-25T21:02:17"
updated_at: "2024-12-25T21:07:43"
tags: []
umbraco_id: 2504
parent_id: 2501
sort_order: 2
aliases: []
---

# (没) 数学準拠な剰余演算子

[こないだ](../u8-string-interpolation/index.md)につづき、C# 言語機能としては没ネタ。
最終的な結論が「ライブラリでやれ」 → 「.NET 10 でメソッド追加を検討」です。

## <a id="mod-usecase">剰余の利用例</a>

剰余演算(C# だと `%` 演算子)の用途として、
「配列の範囲内に収めるために `index % array.Length` する」とかがあると思います。
例えば以下のような感じ。

<pre class="source" title="配列のインデックスを i % Length">
<span class="reserved">var</span> <span class="variable">table</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Table</span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>]);

<span class="control">for</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">5</span>; <span class="variable">i</span><span class="operator">++</span>)
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">table</span><span class="operator">.</span><span class="method">Next</span>());

<span class="comment">// 引数で受け取った配列の中身を1個ずつ返す。</span>
<span class="reserved">struct</span> <span class="type struct">Table</span>(<span class="reserved">int</span>[] <span class="variable local">values</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_index</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">Next</span>()
    {
        <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="field">_index</span>;
        <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="variable local">values</span>[<span class="variable">i</span>];
        <span class="field">_index</span> <span class="operator">=</span> (<span class="variable">i</span> <span class="operator">+</span> <span class="number">1</span>) <span class="operator">%</span> <span class="variable local">values</span><span class="operator">.</span><span class="property">Length</span>; <span class="comment">// % Length することで範囲内に収める。 </span>
        <span class="control">return</span> <span class="variable">x</span>;
    }
}
</pre>

この状態なら、単純に +1 でやっていて、 `i` が常に0以上なので特に問題は起きません。

で、これにちょこっと、「ステップ幅」みたいなのを足したとします。
インデックスを何個飛ばしするか。

<pre class="source" title="(i + 1) % Length を (i + step) % Length に変更">
<span class="reserved">var</span> <span class="variable">table</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Table</span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>], <span class="number">3</span>);

<span class="comment">// 3個飛ばしにしたので 1, 4, 2, 5, 3 と出力される。</span>
<span class="control">for</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">5</span>; <span class="variable">i</span><span class="operator">++</span>)
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">table</span><span class="operator">.</span><span class="method">Next</span>());

<span class="reserved">struct</span> <span class="type struct">Table</span>(<span class="reserved">int</span>[] <span class="variable local">values</span>, <span class="reserved">int</span> <span class="variable local">step</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_index</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">Next</span>()
    {
        <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="field">_index</span>;
        <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="variable local">values</span>[<span class="variable">i</span>];
        <span class="field">_index</span> <span class="operator">=</span> (<span class="variable">i</span> <span class="operator">+</span> <span class="variable local">step</span>) <span class="operator">%</span> <span class="variable local">values</span><span class="operator">.</span><span class="property">Length</span>; <span class="comment">// +1 を +step に変更。 </span>
        <span class="control">return</span> <span class="variable">x</span>;
    }
}
</pre>

そしてうっかり、`step` を負にして `IndexOutOfRange`…

<pre class="source" title="負のステップ">
<span class="reserved">var</span> <span class="variable">table</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Table</span>([<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>], <span class="operator">-</span><span class="number">1</span>);

<span class="comment">// IndexOutOfRange で止まる。</span>
<span class="control">for</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="number">5</span>; <span class="variable">i</span><span class="operator">++</span>)
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">table</span><span class="operator">.</span><span class="method">Next</span>());

<span class="reserved">struct</span> <span class="type struct">Table</span>(<span class="reserved">int</span>[] <span class="variable local">values</span>, <span class="reserved">int</span> <span class="variable local">step</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_index</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">Next</span>()
    {
        <span class="reserved">var</span> <span class="variable">i</span> <span class="operator">=</span> <span class="field">_index</span>;
        <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="variable local">values</span>[<span class="variable">i</span>];
        <span class="field">_index</span> <span class="operator">=</span> (<span class="variable">i</span> <span class="operator">+</span> <span class="variable local">step</span>) <span class="operator">%</span> <span class="variable local">values</span><span class="operator">.</span><span class="property">Length</span>; <span class="comment">// 負 % 正 の結果は負です。 </span>
        <span class="control">return</span> <span class="variable">x</span>;
    }
}
</pre>

「負 % 正」の結果は負になるので、`values[i]` が例外を起こすようになります。

何か多少気持ち悪いですよね。
この類の `(i + step) % Length` って、
意味的には「配列の末尾を超えたら先頭に戻る」なわけです。
だったら逆に、「配列の先頭を超えたら末尾に戻る」もあってもいいわけで。
この例(`Length` が5)でいうと、`values[0]` の次は `values[4]` に行ってほしい。

## <a id="divrem-sign">剰余の符号</a>

前節みたいな動作になるのは、「大体の CPU の剰余命令がそういう実装だから」です。
C# を始めとして、そこそこパフォーマンスを気にするプログラミング言語では、だいたい「ハードウェア的に一番速い奴を選ぶ」な方針になります。

そもそも、整数の剰余自体、ちょっと定義がぶれていまして。
「以下の条件を満たす」というところまではほとんどの環境で合意が得られてる感じではあるんですが:

> a を n で割った商(quotient )を q、余り(remainder )を r として、
>
> * a = nq + r
> * |r| < |n|

商 q は (実数で計算した場合の) a ÷ n に近い整数ではあるんですが、丸め方によって値が1ずれます。
その結果、余り r の符号も変わります。

例えば、以下のコードで得られる `q` と `r` はどの丸め方でも必ず `a == n * q + r`、`Abs(r) < Abs(n)` の条件を満たします。

<pre class="source" title="丸め方の違いでバリエーションがある商と余り">
<span class="comment">// 一度 double で計算して、商の丸め方式をいろいろ変えてみる。</span>
<span class="comment">// 余りは常に r = a - q * n で計算。</span>
<span class="reserved">static</span> (<span class="reserved">int</span> q, <span class="reserved">int</span> r) <span class="method"><span class="static">DivRem</span></span>(<span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">n</span>, <span class="type">MidpointRounding</span> <span class="variable local">mode</span>)
{
    <span class="reserved">var</span> <span class="variable">q</span> <span class="operator">=</span> (<span class="reserved">int</span>)<span class="static"><span class="type">Math</span></span><span class="operator">.</span><span class="static"><span class="method">Round</span></span>((<span class="reserved">double</span>)<span class="variable local">a</span> <span class="operator">/</span> <span class="variable local">n</span>, <span class="variable local">mode</span>);
    <span class="reserved">var</span> <span class="variable">r</span> <span class="operator">=</span> <span class="variable local">a</span> <span class="operator">-</span> <span class="variable">q</span> <span class="operator">*</span> <span class="variable local">n</span>;
    <span class="control">return</span> (<span class="variable">q</span>, <span class="variable">r</span>);
}
</pre>

一応、これでどういう結果が得られるかも例示しておきます:

<pre class="source" title="DivRem の実行例">
<span class="method"><span class="static">ShowDivRem</span></span>(<span class="number">5</span>, <span class="number">3</span>);
<span class="method"><span class="static">ShowDivRem</span></span>(<span class="operator">-</span><span class="number">5</span>, <span class="number">3</span>);
<span class="method"><span class="static">ShowDivRem</span></span>(<span class="number">5</span>, <span class="operator">-</span><span class="number">3</span>);
<span class="method"><span class="static">ShowDivRem</span></span>(<span class="operator">-</span><span class="number">5</span>, <span class="operator">-</span><span class="number">3</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">ShowDivRem</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>, <span class="reserved">int</span> <span class="variable local">y</span>)
{
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">DivRem(</span>{<span class="variable local">x</span>}<span class="string">, </span>{<span class="variable local">y</span>}<span class="string">)</span><span class="string">&quot;</span>);
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">  to 0  </span>{<span class="method"><span class="static">DivRem</span></span>(<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="type">MidpointRounding</span><span class="operator">.</span>ToZero)}<span class="string">&quot;</span>);
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">  floor </span>{<span class="method"><span class="static">DivRem</span></span>(<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="type">MidpointRounding</span><span class="operator">.</span>ToNegativeInfinity)}<span class="string">&quot;</span>);
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">  ceil  </span>{<span class="method"><span class="static">DivRem</span></span>(<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="type">MidpointRounding</span><span class="operator">.</span>ToPositiveInfinity)}<span class="string">&quot;</span>);
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">  away  </span>{<span class="method"><span class="static">DivRem</span></span>(<span class="variable local">x</span>, <span class="variable local">y</span>, <span class="type">MidpointRounding</span><span class="operator">.</span>AwayFromZero)}<span class="string">&quot;</span>);
}
</pre>
<pre class="console" title="DivRem の実行例">
DivRem(5, 3)
  to 0  (1, 2)
  floor (1, 2)
  ceil  (2, -1)
  away  (2, -1)
DivRem(-5, 3)
  to 0  (-1, -2)
  floor (-2, 1)
  ceil  (-1, -2)
  away  (-2, 1)
DivRem(5, -3)
  to 0  (-1, 2)
  floor (-2, -1)
  ceil  (-1, 2)
  away  (-2, -1)
DivRem(-5, -3)
  to 0  (1, -2)
  floor (1, -2)
  ceil  (2, 1)
  away  (2, 1)
</pre>

C# の整数の `/` と `%` (= 大体の CPU の剰余命令の結果)は、この例でいうと `ToZero` と同じです。
例を見ての通り、余り `r`の符号は被除数 `a` と一致します。

一方、[前節](#mod-usecase)みたいなものを含め、アルゴリズム的に使いやすいのは `ToNegativeInfinity` (floor)かもしれません。
正負反転のタイミングで不連続がないのと、r が正負問わず 0～n の範囲に収まるので。

ちなみに、数学では[ユークリッド除法](https://ja.wikipedia.org/wiki/%E9%99%A4%E6%B3%95%E3%81%AE%E5%8E%9F%E7%90%86)を根拠として、

* 余り r が常に正になるようにする
* それに合わせて q を (a - r) ÷ n で計算

とすることが多いそうです。

## <a id="euclidean-mod-operator">没案: %% 演算子</a>

「[いろんな剰余があり得る](#divrem-sign)」という話をしたところで、
[最初の例](#mod-usecase)に戻りましょう。
最初の例では、要するに「`(i + step) % Length` の結果は正が良かったな」という話になります。

ということで出てきたのが以下の提案。

* [Proposal: New operator %% for canonical Modulus operations #7599](https://github.com/dotnet/csharplang/issues/7599)

ここでいう canonical (聖典・規範に準ずる)というのは、「数学の定義に準ずる」みたいな意味で言っていて、
前節の「ユークリッド除法を根拠にした、余り r を常に正する」方式です。
この定義の余りを返す演算子として `%%` 演算子を足すのはどうかという案。

結果的にリジェクトされてるんですけども、理由はまあ、前節の説明の後だとわかりやすいですね。
以下のようなことを考えた時、本当に演算子として追加するのが望ましいのかどうかというと微妙。

* 剰余にもいろいろある
  * 例えば今の場合、`Length` が正なことはわかっているので、`ToNegativeInfinity` 丸めでもいい
* 商と余りはセットで考えないとダメ
  * `%%` と対になる除算も必要？

まあ、言語組み込みの演算子ではなく、
ライブラリ提供のメソッドでやるべきでしょうね。
以下の方法で上記の問題は解決できるので。

* `Divide`、`Remainder`、`DivRem` メソッドをセットで提供
* 丸め方式は引数で明示

## <a id="divrem-rounding">DivRem メソッド</a>

ということで、dotnet/runtime 側に以下の提案が出ています。

* [Provide support for alternative rounding modes for division and remainder of division. #93568](https://github.com/dotnet/runtime/issues/93568)

現状の案では以下のようなメソッドの追加になります。

<pre class="source" title="丸め方式指定付きの DivRem">
<span class="reserved">namespace</span> System<span class="operator">.</span>Numerics;

<span class="reserved">public</span> <span class="reserved">enum</span> <span class="type">DivisionRounding</span>
{
    Truncate <span class="operator">=</span> <span class="number">0</span>,        <span class="comment">// Towards Zero</span>
    Floor <span class="operator">=</span> <span class="number">1</span>,           <span class="comment">// Towards -Infinity</span>
    Ceiling <span class="operator">=</span> <span class="number">2</span>,         <span class="comment">// Towards +Infinity</span>
    AwayFromZero <span class="operator">=</span> <span class="number">3</span>,    <span class="comment">// Away from Zero</span>
    Euclidean <span class="operator">=</span> <span class="number">4</span>,       <span class="comment">// floor(x / abs(n)) * sign(n)</span>
}

<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">interface</span> <span class="type">IBinaryInteger</span>&lt;<span class="type param">TSelf</span>&gt;
{
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">TSelf</span> <span class="method"><span class="static">Divide</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
    <span class="reserved">static</span> <span class="reserved">virtual</span> (<span class="type param">TSelf</span> Quotient, <span class="type param">TSelf</span> Remainder) <span class="method"><span class="static">DivRem</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">TSelf</span> <span class="static"><span class="method">Remainder</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
}
</pre>

おそらくこれらは .NET 10 で入ります。
(デザイン レビューで承認済みで、すでに[実装作業も始まって Pull Request が出ている](https://github.com/dotnet/runtime/pull/104589)状態。)
