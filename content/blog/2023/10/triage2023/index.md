---
title: "C# 13 向けトリアージ"
source_url: "https://ufcpp.net/blog/2023/10/triage2023/"
content_type: "BlogEntry"
published_at: "2023-10-18T21:14:51"
updated_at: "2023-10-18T21:51:26"
tags: []
umbraco_id: 2474
parent_id: 2473
sort_order: 0
aliases: []
---

# C# 13 向けトリアージ

.NET 8 も RC 2 な段階になって、ここから GA までの間に仕様が変わるということはほとんどなくなってきました。
となると、話題はもうその次。来年向け(C# 13 / .NET 9 ターゲット)の話が出てきます。
C# Design Meeting でも、13向けのトリアージがちらほら始まりました。

とりあえず現状、2件。

* [LDM Notes for October 9th, 2023](https://github.com/dotnet/csharplang/discussions/7587)
* [LDM Notes for October 16th, 2023](https://github.com/dotnet/csharplang/discussions/7603)

以下のようなものがトリアージされました。

## 10/9 議事録

### ReadOnlySpan initialization from static data 5295

[#5295](https://github.com/dotnet/csharplang/issues/5295)

C# 7.2 辺りから、以下のような「配列のアロケーションを消す」最適化が掛かります。

<pre class="source" title="配列が消える">
<span class="comment">// 定数だけで構成された byte 配列は最適化で消える。</span>
<span class="comment">// new ReadOnlySpan&lt;byte&gt;(静的データのポインター, 4) みたいなコードに展開される。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">data1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> };

<span class="comment">// .NET 7 までは byte, sbyte のみだったけど、 .NET 8 からはそれ以外の整数にも最適化がかかるように。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">data2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">int</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> };
</pre>

けども、見た目はどう見ても配列を作っているので、たびたび「この配列を new するのもったいなくない？」という突っ込みが入りがちです。

そこで、以下のような「ReadOnlySpan の初期化構文が欲しい」という話がありました。

<pre class="source" title="Span 初期化構文(案)">
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">data1</span> <span class="operator">=</span> { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> };

<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">data2</span> <span class="operator">=</span> { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span> };
</pre>

ですが、C# 12 で入る予定の[コレクション式](../../1/collection-literal/index.md)がこれを兼ねるので、この `{}` を使った書き方はリジェクトになりました。

### Embedded Language Indicators for raw string literals 6247

[#6247](https://github.com/dotnet/csharplang/issues/6247)

<pre class="source" title="インジケーター案">
<span class="comment">// こんな風に、raw string の先頭行に「文字列リテラルの中身が何か」を示すインジケーターを書きたいという案。</span>
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="string">&quot;&quot;&quot;regex
    \s+
    &quot;&quot;&quot;</span>;

<span class="comment">// ちなみに今も、以下のように「文字列リテラル直前のコメントに lang = を付ける」という手段でインジケーターを書ける。</span>
<span class="comment">// Visual Studio はこれを認識して色付けしたり補間したりしてくれる。</span>

<span class="comment">// lang=regex</span>
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="string">&quot;&quot;&quot;
    \s+
    &quot;&quot;&quot;</span>;
</pre>


優先度付くほど強いモチベーションがなさげ。
Backlog (過去ログ行き)。

### list-patterns on enumerables

[#6574](https://github.com/dotnet/csharplang/issues/6574)


`IEnumerable` に対して `x is []` とか書けるようにしたいというやつ。

時間がなくて12でも入らなかっただけ。
Working set (作業中)。

### Make generated `Program`` for top-level statements public by default

[#6769](https://github.com/dotnet/csharplang/issues/6769)

[トップ レベル ステートメント](../../../../study/csharp/cheatsheet/ap_ver9.md#top-level-statements)から生成される Program クラスを public にしたいという話。

一番のモチベはテストだけど。
テスト関連、もっと広く要件調査必要。

### CallerCharacterNumberAttribute

[#3992](https://github.com/dotnet/csharplang/issues/3992)

[Caller Info 属性](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)に追加で「ソースコードの何列目か」を取れるものを足したいという話。
(今、CallerLineNumber で行番号は取れるけども、列を取る手段がない。)

[Interceptor](https://github.com/ufcpp/UfcppSample/issues/456)と一環としてやる。

### Add private and namespace accessibility modifiers for top-level types

[#6794](https://github.com/dotnet/csharplang/issues/6794)

「同一の名前空間内限定でアクセスできる」というアクセスレベルの新設。
[file](../../../../study/csharp/cheatsheet/ap_ver11.md#file-local) は狭すぎるし、[internal](../../../../study/csharp/oop/oo_conceal.md#protected-internal) は広すぎる。

やる気になってるっぽい(Working set)。

### Require await to apply nullable postconditions to task-returning calls

[#6888](https://github.com/dotnet/csharplang/issues/6888)

非同期メソッドが絡んだ時に `MemberNotNull`` とかがちゃんと働かない問題。

作業中。もらったフィードバックに対処が必要。

### is expression evaluating const expression should be considered constant

[#6926](https://github.com/dotnet/csharplang/issues/6926)

<pre class="source" title="const is expression">
<span class="reserved">const</span> <span class="reserved">int</span> <span class="constant"><span class="static">x</span></span> <span class="operator">=</span> <span class="number">123</span>;
<span class="reserved">const</span> <span class="reserved">bool</span> <span class="static"><span class="constant">y</span></span> <span class="operator">=</span> <span class="constant">x</span> <span class="operator">==</span> <span class="number">0</span>; <span class="comment">// これは OK。const 同士に対する式の結果は const。</span>
<span class="reserved">const</span> <span class="reserved">bool</span> <span class="static"><span class="constant">z</span></span> <span class="operator">=</span> <span class="error" title="CS0133"><span class="constant">x</span> <span class="reserved">is</span> <span class="number">0</span></span>; <span class="comment">// 今ダメ。 == が行けるんなら is も行けていいんじゃない？</span>
</pre>

Any time (C# チーム内ではやらないけど、コミュニティ貢献受付はできる)。
実際、[コミュニティ実装が始まってそう](https://github.com/dotnet/csharplang/pull/7589)。

## 10/16 議事録

### Breaking change warnings

[#7189](https://github.com/dotnet/csharplang/issues/7189)

「C# 14 で破壊的変更になる予定だから注意してね」警告みたいなやつを C# 13 以下に対して出そうかという話。
(主に、field キーワード導入がモチベ。)

これについて書いたブログ: [C# での破壊的変更の今後の扱い(案)](../../3/csharp-breaking-change/index.md)

普通に作業中。
10/9 のミーティングでは取り上げ忘れてただけ。
Working set。

### Determine natural type of method group by looking scope-by-scope

[#7364](https://github.com/dotnet/csharplang/issues/7364)

[#7429](https://github.com/dotnet/csharplang/issues/7429) との重複扱いで close。

### u8 string interpolation

[#7072](https://github.com/dotnet/csharplang/issues/7072)

`$"直接 UTF-8 で書き込まれる文字列補間 {x} {y}"u8` (u8 接尾辞) みたいなのが欲しいという話は上がってたんだけど。

.NET 8 の並々ならぬ努力の結果、JIT 最適化がだいぶ賢くなった。

<pre class="source" title=".NET 8 の Utf8.TryWrite は UTF-16 → UTF-8 変換がほぼノーコスト">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>Unicode;

<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
<span class="reserved">int</span> <span class="variable">y</span> <span class="operator">=</span> <span class="number">456</span>;
<span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">dest</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">byte</span>[<span class="number">100</span>];

<span class="static"><span class="type">Utf8</span></span><span class="operator">.</span><span class="method"><span class="static">TryWrite</span></span>(<span class="variable">dest</span>, <span class="string">$&quot;</span><span class="string">UTF-8 補間 </span>{<span class="variable">x</span>}<span class="string"> </span>{<span class="variable">y</span>}<span class="string">&quot;</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">written</span>);
<span class="comment">// ↑ 普通の(UTF-16 な)文字列補間だけど、JIT の努力によって UTF-16 → UTF-8 への変換がほぼノーコストに最適化される。</span>
</pre>

その結果、 `$""u8` の要求減った。
Backlog 行き。

### Lock statement pattern

[#7104](https://github.com/dotnet/csharplang/issues/7104)

`lock (obj)` もパターンベースにしたいという話。

.NET の「任意の object を lock に使える」、「オブジェクトヘッダーに lock 用の syncblock って領域を持ってる」という仕様、オーバーヘッドが大きいので、ちゃんと [`Lock`` 型](https://github.com/dotnet/runtime/issues/34812)みたいなのを用意してそれを使って lock したい。

この `Lock` 型インスタンスに対して `lock (_lock)` されたときに、syncblock 使わず、パターンベースで `Lock.TryEnter` が呼ばれるようにしたい。

.NET 9 マイルストーンで Working set に。

### String/Character escape sequence \e as a short-hand for \u001b ()

[#7400](https://github.com/dotnet/csharplang/issues/7400)

エスケープ文字(U+001B)に対するエスケープシーケンス `\e` を導入したい。

Any time に(今、[提案者に対してコミュニティ実装するか聞いてる](https://github.com/dotnet/csharplang/issues/7400#issuecomment-1765085640)ところ)。

### New operator %% for canonical Modulus operations

[#7599](https://github.com/dotnet/csharplang/issues/7599)

C# の `%` 演算子 (というか、大体の CPU の div rem 命令) は、オペランドの符号によっては 0～n-1 にならない。
それに対して、`array[x % array.Length]` みたいな用途では 0～n-1 になってほしい。

という、需要はあるものの、C# 言語組み込みでやるべきかどうかは疑問。
div rem にはいろいろ種類があるんで、メソッド名とかメソッド引数で「どの div か」を明示すべきという話に支持が集まってる。

なので C# としてはやらない(likely never)。

代わりにライブラリ追加の提案が runtime の方で進みそう ([#93568](https://github.com/dotnet/runtime/issues/93568))。
↓提案内容。

<pre class="source" title="">
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
    <span class="comment">// Existing:</span>
    <span class="reserved">static</span> <span class="reserved">virtual</span> (<span class="type param">TSelf</span> Quotient, <span class="type param">TSelf</span> Remainder) <span class="method"><span class="static">DivRem</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>);

    <span class="comment">// Proposed:</span>
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">TSelf</span> <span class="static"><span class="method">Divide</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
    <span class="reserved">static</span> <span class="reserved">virtual</span> (<span class="type param">TSelf</span> Quotient, <span class="type param">TSelf</span> Remainder) <span class="static"><span class="method">DivRem</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
    <span class="reserved">static</span> <span class="reserved">virtual</span> <span class="type param">TSelf</span> <span class="method"><span class="static">Remainder</span></span>(<span class="type param">TSelf</span> <span class="variable local">left</span>, <span class="type param">TSelf</span> <span class="variable local">right</span>, <span class="type">DivisionRounding</span> <span class="variable local">mode</span>);
}
</pre>
