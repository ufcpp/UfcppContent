---
title: "ピックアップRoslyn 10/4: C# 9.0, パターン追加、switch 式ステートメント、共変戻り値"
source_url: "https://ufcpp.net/blog/2019/10/pickuproslyn1004/"
content_type: "BlogEntry"
published_at: "2019-10-04T22:36:29"
updated_at: "2019-10-04T22:43:31"
tags: []
umbraco_id: 2269
parent_id: 2268
sort_order: 0
aliases: []
---

# ピックアップRoslyn 10/4: C# 9.0, パターン追加、switch 式ステートメント、共変戻り値

何件か、C# 9.0 向けに提案されている機能のドラフト仕様が出てきました。

- [Proposed changes for Pattern Matching in C# 9.0 - Draft Specification #2850](https://github.com/dotnet/csharplang/issues/2850)
- [Draft Spec for Switch Expression as a Statement Expression in C# 9.0 #2860](https://github.com/dotnet/csharplang/issues/2860)
- [Covariant Return Types - Draft Specification #2844](https://github.com/dotnet/csharplang/issues/2844)

## パターン マッチ

- [Pattern Matching - Draft Specification #2850](https://github.com/dotnet/csharplang/issues/2850)

[C# 8.0 でもずいぶんとパターンが増えました](../../../../study/csharp/cheatsheet/ap_ver8.md#recursive-pattern)が、9.0 でも追加が出そうです。

- 複数のパターンを `and` や `or` でつないだり、`!(x is pattern)` と書かなくても `x is not pattern` と書けるようにしたり
- `and` や `or` があるなら優先度を付けるために、パターンを `()` で囲えるようにしたり
- `x is >= min and <= max` みたいに、比較パターンを入れたり

## switch 式を式ステートメントに

- [Switch Expression as a Statement Expression #2860](https://github.com/dotnet/csharplang/issues/2860)

メソッド呼び出しなど、いくつかの式は、式を単体で `M();` みたいに書いてステートメント化できます。(こういうのを「式ステートメントと言います。)

[`switch` 式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)でも、以下のような書き方に需要があるので、式ステートメント化をしたいという話は前々からあります。

<pre class="source" title="switch 式ステートメントの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">A</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A&quot;</span>);
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">B</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B&quot;</span>);
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">C</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;C&quot;</span>);
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span>? <span class="variable">state</span>)
{
    <span class="variable">state</span> <span class="control">switch</span>
    {
        <span class="reserved">true</span> =&gt; <span class="method">A</span>(),
        <span class="reserved">false</span> =&gt; <span class="method">B</span>(),
        <span class="reserved">null</span> =&gt; <span class="method">C</span>(),
    };
}
</code></pre>

C# 8.0 には間に合わなかったので、9.0 での提案に。

### 共変戻り値

- [Covariant Return Types - Draft Specification #2844](https://github.com/dotnet/csharplang/issues/2844)

これは要するに以下のような奴。

<pre class="source" title="共変戻り値の例">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span> { }
 
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">Base</span> <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="comment">// 戻り値が Base じゃなくて Derived。</span>
    <span class="comment">// 原理的には問題ないはずだけど、今までの .NET ではできなかった。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">Derived</span> <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
</code></pre>

これはずっと「C# 上の構文糖衣ではなく、ランタイムに手を入れた方がいいので難しめ」ということでなかなか手付かずだったやつです。

C# 上の構文糖衣で何とかごまかせないかという検討もしていたんですが、
結局、ランタイム(.NET の型システム自体)の修正込みでやろいうという流れになっています。

[インターフェイスのデフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)に続く2例目の「ランタイムを選ぶ新機能」になります。
