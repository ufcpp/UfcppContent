---
title: "静的な typeof/sizeof"
source_url: "https://ufcpp.net/blog/2018/12/statictypeof/"
content_type: "BlogEntry"
published_at: "2018-12-24T10:43:48"
updated_at: "2018-12-24T10:43:48"
tags: []
umbraco_id: 2206
parent_id: 2177
sort_order: 23
aliases: []
---

# 静的な typeof/sizeof

[JIT Intrinsics](../jitintrinsics/index.md)で少し触れましたが、
.NET Core 2.1では`Enum.HasFlag`に対する最適化が掛かります。
.NET Core 2.0と2.1で`Enum.HasFlag`の実行速度が1桁違うわけですが、
古いランタイムでも何とかする手段がなくもないです(ただし、`Unsafe`)。

今日はそんな、.NET Core 2.0以前でも使える最適化の話。

## 定数最適化

例えば、以下のようなコードを考えます。

<pre class="source" title="if (true) は else 側が消える">
<code><span class="reserved">static</span> <span class="reserved">int</span> X()
{
    <span class="reserved">if</span> (<span class="reserved">true</span>) <span class="reserved">return</span> 1;
    <span class="reserved">else</span> <span class="reserved">return</span> 0;
}
</code></pre>

`if` の条件式が定数なので、これは C# のコンパイル時に最適化が掛かって、 `return 1`だけが残ります。`if`相当のコードは出力されません。
このように、コンパイル時に確定している値や条件分岐などは、きれいさっぱり消えることがあります。

## JIT 時定数

中には、C# コンパイル結果としては定数にならないものの、
JIT のタイミングでは定数と判明して、最適化が掛かるものがあります。

ジェネリック型引数に対する`typeof(T)`や`sizeof(T)`はまさにそういう「JIT 時に定数になるもの」です。
例えば以下のようなコードは C# コンパイラーは条件分岐を生成しますが、
 JIT 時の最適化が掛かって、条件式が一致している行だけを残して消えてくれます。

<pre class="source" title="静的に解決できる typeof は JIT 時に消える">
<code><span class="reserved">static</span> <span class="reserved">long</span> MaxValue&lt;<span class="type">T</span>&gt;()
{
    <span class="reserved">if</span> (<span class="reserved">typeof</span>(<span class="type">T</span>) == <span class="reserved">typeof</span>(<span class="reserved">byte</span>)) <span class="reserved">return</span> <span class="reserved">byte</span>.MaxValue;
    <span class="reserved">else</span> <span class="reserved">if</span> (<span class="reserved">typeof</span>(<span class="type">T</span>) == <span class="reserved">typeof</span>(<span class="reserved">short</span>)) <span class="reserved">return</span> <span class="reserved">short</span>.MaxValue;
    <span class="reserved">else</span> <span class="reserved">if</span> (<span class="reserved">typeof</span>(<span class="type">T</span>) == <span class="reserved">typeof</span>(<span class="reserved">int</span>)) <span class="reserved">return</span> <span class="reserved">int</span>.MaxValue;
    <span class="reserved">else</span> <span class="reserved">if</span> (<span class="reserved">typeof</span>(<span class="type">T</span>) == <span class="reserved">typeof</span>(<span class="reserved">long</span>)) <span class="reserved">return</span> <span class="reserved">long</span>.MaxValue;
    <span class="comment">// お好みで、sbyte, ushort, uint, ulong もどうぞ</span>
    <span class="reserved">else</span> <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
}
</code></pre>

## Enum.HasFlag の代わり

ということで、この手の分岐を書いて、ジェネリックな `HasFlag` を書いてみましょう。

<pre class="source" title="sizeof が JIT 時定数なのを利用した HasFlag 最適化">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">EnumExtensions</span>
{
    [<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.AggressiveInlining)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> UnsafeHasFlag&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">T</span> y)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">unmanaged</span>, <span class="type">Enum</span>
    {
        <span class="reserved">if</span> (<span class="type">Unsafe</span>.SizeOf&lt;<span class="type">T</span>&gt;() == 1) <span class="reserved">return</span> (<span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">byte</span>&gt;(<span class="reserved">ref</span> x) &amp; <span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">byte</span>&gt;(<span class="reserved">ref</span> y)) != 0;
        <span class="reserved">else</span> <span class="reserved">if</span> (<span class="type">Unsafe</span>.SizeOf&lt;<span class="type">T</span>&gt;() == 2) <span class="reserved">return</span> (<span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">ushort</span>&gt;(<span class="reserved">ref</span> x) &amp; <span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">ushort</span>&gt;(<span class="reserved">ref</span> y)) != 0;
        <span class="reserved">else</span> <span class="reserved">if</span> (<span class="type">Unsafe</span>.SizeOf&lt;<span class="type">T</span>&gt;() == 4) <span class="reserved">return</span> (<span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">uint</span>&gt;(<span class="reserved">ref</span> x) &amp; <span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">uint</span>&gt;(<span class="reserved">ref</span> y)) != 0;
        <span class="reserved">else</span> <span class="reserved">if</span> (<span class="type">Unsafe</span>.SizeOf&lt;<span class="type">T</span>&gt;() == 8) <span class="reserved">return</span> (<span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">ulong</span>&gt;(<span class="reserved">ref</span> x) &amp; <span class="type">Unsafe</span>.As&lt;<span class="type">T</span>, <span class="reserved">ulong</span>&gt;(<span class="reserved">ref</span> y)) != 0;
        <span class="reserved">else</span> { Throw(); <span class="reserved">return</span> <span class="reserved">default</span>; }
    }
 
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Throw() =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
}
</code></pre>

このコードで、非ジェネリックな場合の、

<pre class="source" title="参考: 非ジェネリック実装">
<code><span class="reserved">static</span> <span class="reserved">bool</span> HasFlag(A x, A y) =&gt; (((<span class="reserved">int</span>)x) &amp; ((<span class="reserved">int</span>)y)) != 0;
</code></pre>

みたいなコードとそこまで差がない性能が出ます。
さすがに最適化をちょっと阻害されるみたいで全く同じとは行きませんが、
少なくとも `Enum.HasFlag` みたいに1桁遅くなることはありません。
せいぜい数割差です。

`Unsafe.SizeOf<T>()` は内部的に `sizeof(T)` を呼んでいるだけです。
単にジェネリック型引数に対して掛けるようにしただけ。
(C# 7.3 移行で unsafe コードであれば、普通にジェネリック型引数に対しても `sizeof(T)` を掛けるようになりました。一方、C# 7.2 以前だと `Unsafe.SizeOf<T>` メソッドが必須です。)

先ほどの説明の通り、`sizeof(T)`はJIT時定数になるので、
この`UnsafeHasFlag`メソッドは、ちゃんと1行だけ残して残りのサイズが違うコードはきれいさっぱり消えます。
この最適化は結構昔から掛かっているものなので、.NET Core 2.0以前でも働きます。
(と言っても、[`Unsafe`クラス](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)が対応している必要があるので、.NET Framework 3.5とかでは動かせません。
`Unsafe.As`相当のILコードを自分で書けば使えますが…)

ちなみに、`Unsafe.As<T, byte>(ref x)` の方は変数の型を無理やり変更するもので、通常の C# ではどうやっても書けません。
メソッドの中身は IL で書かれています。
(また、`Intrinsic`属性が付いているので、おそらく .NET Core 2.1移行ではJITレベルでの最適化も何か掛けていそうです。)
