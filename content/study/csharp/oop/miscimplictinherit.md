---
title: "[余談] 暗黙的な派生"
source_url: "https://ufcpp.net/study/csharp/oop/miscimplictinherit/"
content_type: "Article"
published_at: "2018-04-21T00:00:00"
updated_at: "2021-02-21T18:01:58"
tags: []
umbraco_id: 2149
parent_id: 1248
sort_order: 10
aliases:
  - "/csharp/oop/miscimplictinherit/"
---

# \[余談\] 暗黙的な派生

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
C# では「すべての[値型](../resource/oo_reference.md#valtype)は`ValueType`クラス(`System`名前空間)から派生する」というような、暗黙的な派生があります。

また、通常、値型(組み込み型や、構造体、列挙型)は他の型からの派生、他の型への派生ができませんが、
本項の「暗黙的な派生」だけは許されています。
ただ、内部的には、派生しているように見せかけるための特殊な変換処理が掛かっています。

##### <a id="sec-generated-title-2"></a>ポイント
- 全ての型は `Object` である
- 全ての値型は `ValueType` である
- 全ての列挙型は `Enum` である
- 全てのデリゲートは `Delegate` である
- 全ての配列は `Array` である

##<a id="sec-generated-title-3"></a> <a id="special-types"></a>特殊な型
以下の型は、.NET/C# にとって特別な意味を持ちます。
いずれも `System` 名前空間中のクラスです。

| 型名 | 役割 |
| --- | --- |
| `Object` | 全ての型の共通の最上位の基底クラス。C# のキーワードの `object` はこのクラスの別名になっている。 |
| `ValueType` | 全ての値型([プリミティブ型](../../../blog/2016/12/tipsprimitives/index.md)、[構造体](../resource/rm_struct.md)、[列挙型](../structured/st_enum.md))の共通基底クラス。 |
| `Enum` | 全ての[列挙型](../structured/st_enum.md)の共通基底クラス。`ValueType` クラスから派生。 |
| `Delegate` | 全ての[デリゲート](../functional/sp_delegate.md)の共通基底クラス。 |
| `Array` | 全ての[配列](../structured/st_array.md)の共通基底クラス。 |

これらの型は「共通基底」として働きます。
基底クラスになっているので、派生しているどんな型でも受け取れる変数が作れます。

<pre class="source" title="いろいろな型を受け付ける基底クラスの変数">
<code><span class="comment">// 整数でも DateTime 構造体でも UriKind 列挙型でも入る変数</span>
<span class="type">ValueType</span> x;
x = 1;
x = <span class="type">DateTime</span>.Now;
x = <span class="type">PlatformID</span>.Unix;

<span class="comment">// どんな型の配列でも入る変数</span>
<span class="type">Array</span> array;
array = <span class="reserved">new</span>[] { 1, 2, 3 };
array = <span class="reserved">new</span>[] { 1.2, 2.5, 3.9 };
array = <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"b"</span>, <span class="string">"c"</span> };
</code></pre>

また、これらのクラスのメンバーは、派生型から呼べます。
例えば、`Enum`クラスが持っている`HasFlag`メソッドは任意の列挙型に対して使えます。
(ただし、この`HasFlag`の利用には、後述するパフォーマンス上の注意点があります。)

<pre class="source" title="">
<code><span class="reserved">using</span> System;

[<span class="type">Flags</span>]
<span class="reserved">enum</span> <span class="type">Flag</span>
{
    X = 1,
    Y = 2,
    Z = 4,
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Flag</span> f = <span class="type">Flag</span>.X | <span class="type">Flag</span>.Y;
        <span class="reserved">if</span> (f.HasFlag(<span class="type">Flag</span>.X)) <span class="comment">// Flag 型に対して、Enum.HasFlag を呼んでる</span>
        {
            <span class="comment">// ...</span>
        }
    }
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="boxing"></a>ValueType, Enum とボックス化
前述の `ValueType` や `Enum` はクラス(もちろん参照型)です。
これらに対して、値型である整数や列挙型の値を代入するとボックス化が起こります。

ボックス化については詳しくは「[ボックス化](../resource/rmboxing.md)」で説明しているのでそちらをご覧ください。
簡単にいうと、本来値の持ち方が全然違う型に対して、内部的な変換処理が働いています。
この変換処理はそれなりに重たい処理で、パフォーマンス的には避けたいものです。
(元から参照型な`Array`(配列)や`Delegate`(デリゲート型)の場合は特に問題になりません。
`ValueType`と`Enum`だけの問題です。)

例えば以下のようなコードは、似たようなことに対して2つの書き方をしているだけですが、
パフォーマンス的にはだいぶ差があります。

<pre class="source" title="ValueType への代入でボックス化">
<code><span class="comment">// 値型だけを受け付けたいとき、ValueType で引数を受け取るとボックス化が起きる</span>
<span class="reserved">static</span> <span class="reserved">void</span> A(ValueType value) { }

<span class="comment">// ボックス化を避けたければこう書く</span>
<span class="comment">// where T : struct 制約付きのジェネリック メソッドを用意</span>
<span class="reserved">static</span> <span class="reserved">void</span> B&lt;<span class="type">T</span>&gt;(T value) <span class="reserved">where</span> T : <span class="reserved">struct</span> { }

<span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="comment">// 同じような呼び方をしていても、</span>
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10000; i++)
    {
        <span class="comment">// こっちの方が倍は遅い</span>
        A(1);
        A(<span class="type">TimeSpan</span>.FromSeconds(1));
        A(<span class="type">PlatformID</span>.Unix);
    }

    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10000; i++)
    {
        B(1);
        B(<span class="type">TimeSpan</span>.FromSeconds(1));
        B(<span class="type">PlatformID</span>.Unix);
    }
}
</code></pre>

[単にこのメソッドを呼び出すだけのベンチマーク](https://gist.github.com/ufcpp/0fd6fa47d05a21ad7fb559a615cd0460)を取ると、2倍～3倍程度の差が付きます。
(手元の環境では、`A`の方が167μ秒、`B`の方が66μ秒でした。)

できる限りはジェネリクスを使う方がいいでしょう。

###<a id="sec-generated-title-5"></a> <a id="enum-hasflag"></a>Enum.HasFlag でのボックス化
ちなみに、列挙型は、`Enum`型変数を経由しなくても、`HasFlag`メソッドを呼んだ時点でボックス化します。
なので長らく、このメソッドは「地雷」として有名だったんですが、
.NET Core 2.1では「`Enum.HasFlag`を見たら特別扱いして置き換える」と言うような最適化が掛かるようになったそうです。

例えば以下のようなコードは、[.NET Core 2.0以前と2.1以降で実行速度に20倍以上の差](https://gist.github.com/ufcpp/79283511d2a10afb34f8c5c837dce1a6)があります。

<pre class="source" title="">
<code>[<span class="type">Flags</span>]
<span class="reserved">enum</span> <span class="type">X</span>
{
    A = 1,
    B = 2,
    C = 4,
}

<span class="reserved">int</span> Count(<span class="type">X</span> x)
{
    <span class="reserved">var</span> count = 0;
    <span class="reserved">if</span> (x.HasFlag(<span class="type">X</span>.A)) count++;
    <span class="reserved">if</span> (x.HasFlag(<span class="type">X</span>.B)) count++;
    <span class="reserved">if</span> (x.HasFlag(<span class="type">X</span>.C)) count++;
    <span class="reserved">return</span> count;
}
</code></pre>

##<a id="sec-generated-title-6"></a> <a id="constraints"></a>Enum制約とDelegate制約
<h5 class="version version7">Ver. 7.3</h5>

本項で紹介しているような「特殊な基底クラス」は、これまでジェネリクスの型制約には指定できませんでした。

<pre class="source" title="Array は型制約には使えない">
<code><span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;()
    <span class="reserved">where</span> T : <span class="error">System.<span class="type">Array</span></span> <span class="comment">// エラーになる</span>
{ }
</code></pre>

C# 7.3 からはこの制限が少しだけ緩和されて、`Enum`と`Delegate`の2つは制約にできるようになりました。

`Enum`制約を付けると[列挙型](../structured/st_enum.md)だけを受け取れるジェネリック型・ジェネリック メソッドを作れます。

<pre class="source" title="Enum制約">
<code>        <span class="reserved">static</span> <span class="reserved">void</span> EnumConstraint&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">T</span> y)
            <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>, <span class="type">Enum</span>
        {
            <span class="type">Console</span>.WriteLine(x.HasFlag(y));

            <span class="comment">// ちなみに、 == はダメ。Enum クラスに == はない。<span>
        }
</code></pre>

[前述の.NET Core 2.1での最適化](#enum-hasflag)の話と併せると、
.NET Core 2.1以降では有用な機能でしょう。

一方、`Delegate`制約の方は[デリゲート](../functional/sp_delegate.md)だけを受け取れます。
ちなみに、`Delegate`クラスにはさらに`MulticastDelegate`クラス(これも`System`名前空間)という派生クラスがいますが、この型も型制約として使えます。

<pre class="source" title="Delegate/MulticastDelegate制約">
<code><span class="reserved">static</span> <span class="reserved">bool</span> M&lt;<span class="type">A</span>&gt;(<span class="type">A</span> a, <span class="type">A</span> b)
    <span class="reserved">where</span> <span class="type">A</span> : <span class="type">MulticastDelegate</span>
{
    <span class="comment">// Delegate は == 演算子を持ってる</span>
    <span class="reserved">return</span> a == b;
}

<span class="reserved">static</span> <span class="reserved">object</span> Invoke&lt;<span class="type">A</span>&gt;(<span class="type">A</span> a)
    <span class="reserved">where</span> <span class="type">A</span> : <span class="type">Delegate</span>
{
    <span class="comment">// Delegate.DynamicInvoke を呼ぶ</span>
    <span class="reserved">return</span> a.DynamicInvoke();
}
</code></pre>

デリゲートの方は、`Enum.HasFlag`のような特殊な最適化が関わるわけではなく、
そこまで使い勝手は良くありません。
[`Expression.Lambda<TDelegate>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.linq.expressions.expression.lambda?view=netframework-4.7.2#System_Linq_Expressions_Expression_Lambda_System_Linq_Expressions_Expression_System_Collections_Generic_IEnumerable_System_Linq_Expressions_ParameterExpression__)のような一部のメソッドでのみ有効そうです。
