---
title: "[雑記] 明確な代入ルール"
source_url: "https://ufcpp.net/study/csharp/start/definiteassignment/"
content_type: "Article"
published_at: "2023-04-15T16:19:17"
updated_at: "2023-04-15T16:19:17"
tags: []
umbraco_id: 2462
parent_id: 1190
sort_order: 20
aliases:
  - "/csharp/start/definiteassignment/"
---

# \[雑記\] 明確な代入ルール

##<a id="sec-generated-title-1"></a> <a id="abstract">概要</a>
C# には「明確な代入(definite assignment)ルール」と呼ばれる、未初期化変数を避ける仕組みがあります。

##<a id="sec-generated-title-2"></a> <a id="undefined">未定義動作問題</a>
大昔のプログラミング言語では、
変数に対して誰も何の値も代入していないことで、不定な値が返ってくるということがありました。
不定な値が得られてしまうことで、[未定義な動作](../resource/rm_default.md#uninitialized)になります。
特にまずいのは、「テストの時にはたまたまうまくいっていた(うまくいく値が返っていた)けども、本番でだけ失敗する」みたいな状況です。

この未定義動作はかなりまずい状態なので、
最近のプログラミング言語では大体これを防いでいます。
大体以下のいずれかの手段を取ります。

* 既定値: ある決まった値([C# の場合は 0 や null](../resource/rm_default.md))を自動的に代入する
* 明確な代入: 開発者が明示的な代入をすることを義務付ける

C# では、[クラス](../oop/oo_class.md)のフィールドや[配列](../structured/st_array.md)の中身については前者の「既定値による初期化」を行っていて、ローカル変数については後者の「代入の義務付け」を行っています。
この「代入の義務付け」が「明確な代入ルール」です。

##<a id="sec-generated-title-3"></a> <a id="rule">ルールの例</a>
まずわかりやすい例から見ていきましょう。
分岐も何もなければ簡単です。以下のようなコードはコンパイル エラーになります。

<pre class="source" title="未初期化変数を使おうとしてエラーになる例">
<span class="reserved">int</span> <span class="variable">x</span>;

<span class="comment">// x に何も代入しないまま値を取り出そうとした。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="error" title="CS0165"><span class="variable">x</span></span>);
</pre>

解決策は当然「ちゃんと代入すること」(definitely assigned)なんですが、
変数の宣言と同時に初期値を与えるのでもいいですし、
後からの代入でも構いません。

<pre class="source" title="ちゃんと代入">
<span class="comment">// 変数宣言と同時に初期値を与える。</span>
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="reserved">int</span> <span class="variable">y</span>;

<span class="comment">// ここで y を使うとまずいけども…</span>

<span class="variable">y</span> <span class="operator">=</span> <span class="number">2</span>;

<span class="comment">// 値の代入後なら大丈夫。</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">y</span>);
</pre>

C# では、この明確な代入を判定する際、分岐も見てくれます。
全ての分岐先でちゃんと代入していれば OK です。

<pre class="source" title="if-else 両方で代入">
<span class="comment">// 大丈夫な例: if-else 両方で代入。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;

    <span class="control">if</span> (<span class="variable local">condition</span>)
    {
        <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
    }
    <span class="control">else</span>
    {
        <span class="variable">x</span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>;
    }

    <span class="comment">// 大丈夫。</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
}
</pre>

<pre class="source" title="if でだけ代入">
<span class="comment">// ダメな例: if でだけ代入。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;

    <span class="control">if</span> (<span class="variable local">condition</span>)
    {
        <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
    }

    <span class="comment">// エラー。</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable"><span class="error" title="CS0165">x</span></span>);
}
</pre>

`if` だけではなく、`switch` でも判定してくれます。

<pre class="source" title="case が全ての値を網羅">
<span class="comment">// 大丈夫な例: case が全ての値を網羅しているなら大丈夫。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">byte</span> <span class="variable local">condition</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;

    <span class="control">switch</span> (<span class="variable local">condition</span>)
    {
        <span class="control">case</span> <span class="number">0</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>; <span class="control">break</span>;
        <span class="control">case</span> <span class="number">1</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>; <span class="control">break</span>;
        <span class="control">default</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">break</span>; <span class="comment">// default は必須。</span>
    }

    <span class="comment">// 大丈夫。</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span>);
}
</pre>

<pre class="source" title="case に漏れ">
<span class="comment">// ダメな例: case に漏れがあるとダメ。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">byte</span> <span class="variable local">condition</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;

    <span class="control">switch</span> (<span class="variable local">condition</span>)
    {
        <span class="control">case</span> <span class="number">0</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>; <span class="control">break</span>;
        <span class="control">case</span> <span class="number">1</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>; <span class="control">break</span>;
        <span class="control">case</span> <span class="operator">&lt;</span> <span class="number">255</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>; <span class="control">break</span>;
        <span class="comment">// この条件だと、condition が 255 の時が漏れてる。</span>
    }

    <span class="comment">// エラー。</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="error" title="CS0165"><span class="variable">x</span></span>);
}
</pre>

<pre class="source" title="結構ちゃんと網羅性をチェックしてる">
<span class="comment">// 大丈夫な例: 結構ちゃんと網羅性をチェックしてる。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">m</span></span>(<span class="reserved">sbyte</span> <span class="variable local">condition</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;

    <span class="control">switch</span> (<span class="variable local">condition</span>)
    {
        <span class="control">case</span> <span class="operator">&lt;</span> <span class="number">0</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>; <span class="control">break</span>;
        <span class="control">case</span> <span class="number">0</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">break</span>;
        <span class="control">case</span> <span class="operator">&gt;</span> <span class="number">0</span>: <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>; <span class="control">break</span>;
        <span class="comment">// 負、0、正 で全ての値を網羅。</span>
    }

    <span class="comment">// 大丈夫。</span>
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span>);
}
</pre>

ループも結構ちゃんと判定します。
例えば、`while (false)` や、`break` なども追ってくれます。


<pre class="source" title="通らないループ">
<span class="comment">// ダメな例: 通らないループ。</span>
<span class="reserved">int</span> <span class="variable">x</span>;

<span class="control">while</span> (<span class="reserved">false</span>)
{
    <span class="comment">// ここを通らないこともちゃんと判定される。</span>
    <span class="variable"><span class="warning" title="CS0162">x</span></span> <span class="operator">=</span> <span class="number">1</span>;
}

<span class="comment">// エラー。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable"><span class="error" title="CS0165">x</span></span>);
</pre>

<pre class="source" title="早すぎる break">
<span class="comment">// ダメな例: 早すぎる break。</span>
<span class="reserved">int</span> <span class="variable">x</span>;

<span class="control">while</span> (<span class="reserved">true</span>)
{
    <span class="control">break</span>;
    <span class="comment">// ここを通らないこともちゃんと判定される。</span>
    <span class="variable"><span class="warning" title="CS0162">x</span></span> <span class="operator">=</span> <span class="number">1</span>;
}

<span class="comment">// エラー。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="error" title="CS0165"><span class="variable">x</span></span>);
</pre>

<pre class="source" title="break 前に代入">
<span class="comment">// 大丈夫な例: break 前に代入。</span>
<span class="reserved">int</span> <span class="variable">x</span>;

<span class="control">while</span> (<span class="reserved">true</span>)
{
    <span class="comment">// これならここを通る。</span>
    <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
    <span class="control">break</span>;
}

<span class="comment">// 大丈夫。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);</pre>

<pre class="source" title="永久ループ">
<span class="comment">// 大丈夫な例: 永久ループの下。</span>
<span class="reserved">int</span> <span class="variable">x</span>;

<span class="control">while</span> (<span class="reserved">true</span>)
{
}

<span class="comment">// 永久ループの下には来ないので、この行自体呼ばれない。</span>
<span class="comment">// その場合、「代入してない」エラーにはならない。</span>
<span class="comment">// 別途「絶対に通らない」警告は出る。</span>
<span class="type"><span class="static"><span class="warning" title="CS0162">Console</span></span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
</pre>

##<a id="sec-generated-title-4"></a> <a id="improved-rule">ルールの改善</a>
<h5 class="version version10">Ver. 10</h5>

長らく、`?.` や `??` が絡んだ時の明確な代入の判定はあまり賢くありませんでした。
明確に代入されているケースでも、判定漏れでコンパイル エラーになっていました。
(厳しめにエラーになっているので、未定義動作問題は起きません。不便なだけです。)

それが C# 10 で改善されました。
例えば以下のコードは C# 10 以降でだけコンパイルできます。

<pre class="source" title="?. == true">
<span class="comment">// C# 10 から大丈夫な例: ?. == true。</span>
<span class="reserved">void</span> <span class="method">m</span>(<span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">d</span>)
{
    <span class="control">if</span> (<span class="variable local">d</span><span class="operator">?</span><span class="operator">.</span><span class="method">TryGetValue</span>(<span class="number">123</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">x</span>) <span class="operator">==</span> <span class="reserved">true</span>)
    {
        <span class="comment">// C# 10 から大丈夫になった。</span>
        <span class="comment">// (前までは ?. からの == true は判定漏れでエラー。)</span>
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
    }
}
</pre>

<pre class="source" title="?. ??">
<span class="comment">// C# 10 から大丈夫な例: ?. ??。</span>
<span class="reserved">void</span> <span class="method">m</span>(<span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="variable local">d</span>)
{
    <span class="control">if</span> (<span class="variable local">d</span><span class="operator">?</span><span class="operator">.</span><span class="method">TryGetValue</span>(<span class="number">123</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">x</span>) <span class="operator">??</span> <span class="reserved">false</span>)
    {
        <span class="comment">// C# 10 から大丈夫になった。</span>
        <span class="comment">// (前までは ?. からの ?? も同様。)</span>
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span>);
    }
}
</pre>
