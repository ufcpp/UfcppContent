---
title: "Unsafe クラス(保証外)"
source_url: "https://ufcpp.net/blog/2018/12/unsafenogarantee/"
content_type: "BlogEntry"
published_at: "2018-12-27T09:53:16"
updated_at: "2018-12-27T09:53:16"
tags: []
umbraco_id: 2210
parent_id: 2177
sort_order: 27
aliases: []
---

# Unsafe クラス(保証外)

今日は `Unsafe` クラスがらみの話で、
特にきわどい(動作保証外)やつ。
.NET Core 2.0 ～ 2.1 くらいでは動くことを確認していますが、
仕様として保証がなく、古いランタイムや将来、また、Mono などの他の .NET 環境で動く保証がないものです。

## メモリレイアウトが同じもの

まず、元々 unsafe コードを使ってできるし、
`Unsafe`クラスを使っても動作保証があるものから説明。

ポインターを使ったり、`Unsafe.As`メソッドを使うと、
全然違う型・C# では本来変換できない型同士の間で強制変換ができます。
強制しているだけなので、それがちゃんと意味あるコードになるかどうかは unsafe、
すなわち、書いている人の責任の範疇になります。

どういう場合なら大丈夫かというと、要するに、
メモリ上でのフィールドなどのレイアウトが同じ場合です。
例えば、以下のような、サイズが同じで参照型を含まない構造体同士は強制変換しても大丈夫です。

<pre class="source" title="レイアウトがわかっている構造体に対して Unsafe">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Runtime.InteropServices;
 
<span class="comment">// 構造体サイズが4バイトになるようにフィールドを並べる</span>
<span class="comment">// この場合は別に StructLayout 属性がなくても4バイトになるものの、</span>
<span class="comment">// サイズをピッタリ調整したい場合には明示した方がいいかも。</span>
[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 1)]
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> X;
    <span class="reserved">public</span> <span class="reserved">byte</span> Y;
    <span class="reserved">public</span> <span class="reserved">short</span> Z;
}
 
<span class="comment">// int 1個なので当然4バイト。</span>
<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// サイズが同じで参照型を含まない構造体間での強制変換は、</span>
        <span class="comment">// 普通にポインターを使ってできる操作なので</span>
        <span class="comment">// unsafe ではあってもまだ動作保証はある。</span>
        <span class="type">B</span> b = <span class="reserved">new</span> <span class="type">B</span> { X = 0x01020304 };
        <span class="type">A</span> a = <span class="type">Unsafe</span>.As&lt;<span class="type">B</span>, <span class="type">A</span>&gt;(<span class="reserved">ref</span> b);
 
        <span class="comment">// 4, 3, 102</span>
        <span class="type">Console</span>.WriteLine(<span class="string">$&quot;</span>{a.X}<span class="string">, </span>{a.Y}<span class="string">, </span>{a.Z:<span class="string">x</span>}<span class="string">&quot;</span>);
    }
}
</code></pre>

## 保証外な利用方法

`Unsafe.As`メソッドを使うと、
こういった強制型変換を参照型に対しても行えます。

ただ、これは動作保証がないようです。
少なくとも .NET Core 2.1 では動いているんですが、
将来にわたってもそのまま動くかと言われると何も保証されていません。

### 共変クラス

C# の[変性](../../../../study/csharp/oop/sp4_variance.md)はインターフェイスとデリゲートに対してしか働かないわけですが、それを強制的にクラスに対しても適用できたりします。

<pre class="source" title="共変クラス(動作保証なし)">
<code><span class="comment">// string → object の代入が合法なんだったら…</span>
<span class="reserved">string</span> s = <span class="string">&quot;abc&quot;</span>;
<span class="reserved">object</span> o = s;
 
<span class="comment">// Task&lt;string&gt; → Task&lt;object&gt; も OK にしてほしい</span>
<span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; ts = <span class="type">Task</span>.FromResult(<span class="string">&quot;abc&quot;</span>);
 
<span class="comment">// 実際は無理</span>
<span class="comment">// Task&lt;object&gt; to = ts;</span>
 
<span class="comment">// が、Unsafe.As ならできてしまう。</span>
<span class="type">Task</span>&lt;<span class="reserved">object</span>&gt; to = <span class="type">Unsafe</span>.As&lt;<span class="type">Task</span>&lt;<span class="reserved">string</span>&gt;, <span class="type">Task</span>&lt;<span class="reserved">object</span>&gt;&gt;(<span class="reserved">ref</span> ts);
 
<span class="comment">// await でちゃんと &quot;abc&quot; が取れる</span>
<span class="reserved">var</span> result = <span class="reserved">await</span> to;
<span class="type">Console</span>.WriteLine(result);
</code></pre>

ただ、これは `Task<TResult>`クラス(`System.Threading.Tasks`名前空間)の`TResult`が戻り値にしか使われていないから大丈夫なのであって、
例えば以下のように、読み書き両方できるとまずいです。

<pre class="source" title="ほんとに挙動が壊れるダメな Unsafe">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Box</span>&lt;<span class="type">T</span>&gt; { <span class="reserved">public</span> <span class="type">T</span> Value; }
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// string → object の代入が合法なんだったら…</span>
        <span class="type">Box</span>&lt;<span class="reserved">string</span>&gt; s = <span class="reserved">new</span> <span class="type">Box</span>&lt;<span class="reserved">string</span>&gt; { Value = <span class="string">&quot;abc&quot;</span> };
        <span class="type">Box</span>&lt;<span class="reserved">object</span>&gt; o = <span class="type">Unsafe</span>.As&lt;<span class="type">Box</span>&lt;<span class="reserved">string</span>&gt;, <span class="type">Box</span>&lt;<span class="reserved">object</span>&gt;&gt;(<span class="reserved">ref</span> s);
 
        <span class="comment">// 読み出しはまだ大丈夫。&quot;abc&quot; が表示される。</span>
        <span class="type">Console</span>.WriteLine(o.Value);
 
        <span class="comment">// 書き込みはアウト。</span>
        o.Value = 10;
        <span class="comment">// ダメなことをやっちゃったあとなので、何か動作がおかしい。</span>
        <span class="comment">// 最悪の場合死に至るのでダメ、絶対！</span>
        <span class="type">Console</span>.WriteLine(o.Value);
    }
}
</code></pre>

また、`string` → `object` が大丈夫だから `Task<string>` → `Task<object>` も大丈夫だったのであって、互換性がない型同士での `Task<T>` 間の変換はもちろんダメです。

<pre class="source" title="ヤバい(ヤバい)">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="comment">// 無関係のクラス</span>
<span class="reserved">class</span> <span class="type">C1</span> { }
<span class="reserved">class</span> <span class="type">C2</span> { }
 
<span class="comment">// A, B は同じ4バイト</span>
<span class="comment">// C は1バイト</span>
<span class="reserved">struct</span> <span class="type">A</span> { <span class="reserved">public</span> <span class="reserved">int</span> X; }
<span class="reserved">struct</span> <span class="type">B</span> { <span class="reserved">public</span> <span class="reserved">short</span> X, Y; }
<span class="reserved">struct</span> <span class="type">C</span> { <span class="reserved">public</span> <span class="reserved">byte</span> X; }
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ヤバい(無関係のクラス)</span>
        <span class="type">Task</span>&lt;<span class="type">C1</span>&gt; c1 = <span class="type">Task</span>.FromResult&lt;<span class="type">C1</span>&gt;(<span class="reserved">null</span>);
        <span class="type">Task</span>&lt;<span class="type">C2</span>&gt; c2 = <span class="type">Unsafe</span>.As&lt;<span class="type">Task</span>&lt;<span class="type">C1</span>&gt;, <span class="type">Task</span>&lt;<span class="type">C2</span>&gt;&gt;(<span class="reserved">ref</span> c1);
 
        <span class="comment">// 保証外だけどギリ動く(サイズが同じ)</span>
        <span class="type">Task</span>&lt;<span class="type">A</span>&gt; a = <span class="type">Task</span>.FromResult(<span class="reserved">new</span> <span class="type">A</span>());
        <span class="type">Task</span>&lt;<span class="type">B</span>&gt; b = <span class="type">Unsafe</span>.As&lt;<span class="type">Task</span>&lt;<span class="type">A</span>&gt;, <span class="type">Task</span>&lt;<span class="type">B</span>&gt;&gt;(<span class="reserved">ref</span> a);
 
        <span class="comment">// ヤバい(サイズが違う)</span>
        <span class="type">Task</span>&lt;<span class="type">C</span>&gt; c = <span class="type">Unsafe</span>.As&lt;<span class="type">Task</span>&lt;<span class="type">A</span>&gt;, <span class="type">Task</span>&lt;<span class="type">C</span>&gt;&gt;(<span class="reserved">ref</span> a);
    }
}
</code></pre>

### シグネチャが同じデリゲート

デリゲートは、引数・戻り値の型が完全に一致していても、
別個に定義したものは別の型扱いを受けます。

そして、引数・戻り値の型が完全に一致しているデリゲート型は山ほどあります。
例えば以下のような。

- [`IValueTaskSource`](https://source.dot.net/#System.Runtime/System.Runtime.cs,e847f170291a7c6f,references) … `Action<object>` を使用。`object`引数、`void`戻り値。
- [`Timer`](https://source.dot.net/#System.Private.CoreLib/src/System/Threading/Timer.cs,814) … `TimerCallback` を使用。`object`引数、`void`戻り値。
- [`SynchronizationContext`](https://source.dot.net/#System.Private.CoreLib/src/System/Threading/SynchronizationContext.cs,98) … `SendOrPostCallback` を使用。`object`引数、`void`戻り値。

そして、これらのデリゲート間の変換では、以下のように `new` が挟まってしまって、無駄にメモリを食います。

<pre class="source" title="デリゲートの残念さ">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks.Sources;
 
<span class="reserved">class</span> <span class="type">MyValueTaskSource</span> : <span class="type">IValueTaskSource</span>
{
    <span class="reserved">private</span> <span class="type">SynchronizationContext</span> _context;
    <span class="reserved">public</span> <span class="reserved">void</span> GetResult(<span class="reserved">short</span> token) { }
    <span class="reserved">public</span> <span class="type">ValueTaskSourceStatus</span> GetStatus(<span class="reserved">short</span> token) =&gt; <span class="type">ValueTaskSourceStatus</span>.Succeeded;
    <span class="reserved">public</span> <span class="reserved">void</span> OnCompleted(<span class="type">Action</span>&lt;<span class="reserved">object</span>&gt; continuation, <span class="reserved">object</span> state, <span class="reserved">short</span> token, <span class="type">ValueTaskSourceOnCompletedFlags</span> flags)
    {
        <span class="comment">// こういう書き方は無理。</span>
        <span class="comment">// _context.Post(continuation, state);</span>
 
        <span class="comment">// こうなる。</span>
        _context.Post(continuation.Invoke, state);
 
        <span class="comment">// ↑これは意味的には↓と同じ。1段 new が挟まってて、ヒープも確保される。</span>
        <span class="comment">// _context.Post(new SendOrPostCallback(continuation.Invoke), state);</span>
    }
}
</code></pre>

でも、`Unsafe.As`メソッドを使えば無駄な `new` なしで強制変換できます。

<pre class="source" title="Unsafe で無理やり変換(動作保証なし)">
<code><span class="comment">// でも、これで行けたりする。</span>
_context.Post(Unsafe.As&lt;<span class="type">Action</span>&lt;<span class="reserved">object</span>&gt;, <span class="type">SendOrPostCallback</span>&gt;(<span class="reserved">ref</span> continuation), state);
</code></pre>

引数・戻り値の型が一致している限りには、
少なくとも .NET Core 2.1 とかでは動きます
(再三いうけども動作保証があるわけじゃない)。

<pre class="source" title="Unsafe で無理やり変換(動作保証なし)">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading;
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Action</span>&lt;<span class="reserved">object</span>&gt; action = x =&gt; <span class="type">Console</span>.WriteLine(x);
        <span class="type">SendOrPostCallback</span> callback = <span class="type">Unsafe</span>.As&lt;<span class="type">Action</span>&lt;<span class="reserved">object</span>&gt;, <span class="type">SendOrPostCallback</span>&gt;(<span class="reserved">ref</span> action);
 
        callback(<span class="string">&quot;abc&quot;</span>); <span class="comment">// ちゃんと Console.WriteLine(&quot;abc&quot;) が呼ばれる</span>
    }
}
</code></pre>

## 静的な型と動的な型

ちなみに、`Unsafe.As`メソッドでの共生型変換を、互いに無関係なクラスでやってしまうと結構変な動作になります。
以下のように、全然無関係なメソッドが呼ばれてしまうことがあり得ます。
([仮想呼び出し](../../../../study/csharp/oop/oo_vftable.md)が狂います。本来参照すべきものと違う仮想テーブルをひいちゃうので当然。)

<pre class="source" title="Unsafe.As の強制変換をすると仮想呼び出しが狂う">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> M() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">&quot;A non-virtual M&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> X() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">&quot;A virtual X&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> M() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">&quot;B non-virtual M&quot;</span>);
 
    <span class="comment">// 仮想テーブル的に、A.X と同じ場所にポインターが入る</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> Y() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">&quot;B virtual Y&quot;</span>);
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">A</span> a = <span class="reserved">new</span> <span class="type">A</span>();
        <span class="type">B</span> b = <span class="type">Unsafe</span>.As&lt;<span class="type">A</span>, <span class="type">B</span>&gt;(<span class="reserved">ref</span> a);
 
        <span class="comment">// non-virtual なメソッドは静的な型(B)に基づいて呼ばれる。</span>
        <span class="comment">// なので、これは普通に B.M が呼ばれる</span>
        b.M(); <span class="comment">// B non-virtual M</span>
 
        <span class="comment">// virtual なメソッドは動的な型(A)に基づいて呼ばれる。</span>
        <span class="comment">// 型の強制変換のせいで変な挙動に。</span>
        <span class="comment">// 仮想テーブル上、B.Y と同じ位置に A.X のポインターがあるので、</span>
        <span class="comment">// B.Y を呼んだつもりが A.X が呼ばれる。</span>
        b.Y(); <span class="comment">// A virtual X</span>
    }
}
</code></pre>

この例ではたまたまクラッシュせずに動作しますが
(というか、ならないように気を使って書いています)、
無神経にやるとまずクラッシュします。
