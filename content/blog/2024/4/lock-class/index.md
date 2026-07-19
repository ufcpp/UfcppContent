---
title: "Lock クラス"
source_url: "https://ufcpp.net/blog/2024/4/lock-class/"
content_type: "BlogEntry"
published_at: "2024-04-04T00:22:28"
updated_at: "2024-04-04T00:22:28"
tags: []
umbraco_id: 2497
parent_id: 2496
sort_order: 0
aliases: []
---

# Lock クラス

今日は、
.NET 9 で [`Lock` クラス](https://learn.microsoft.com/ja-jp/dotnet/api/system.threading.lock)というのが入る予定で、
それに伴って C# コンパイラーにも対応が必要そうという話。

一応雰囲気的には C# 13 に入りそう。

## 任意のオブジェクトを lock

C# は**なぜか**任意のオブジェクト インスタンスを使って排他制御ができます。
ロックを掛けるために以下のようなコードを書くことになります。

<pre class="source" title="ロック用のオブジェクトをわざわざ用意">
<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">object</span> <span class="field">_syncObj</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">object</span>();

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="reserved">lock</span> (<span class="field">_syncObj</span>)
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }
}
</pre>

Java からの習慣(= 1995年頃の発想)ですかね。
Java の `synchronized` ブロックも同じ仕様のはず。

本来の思想としては「`lock()` の `()` 内には同時に操作されるとまずいリソースを書く」という感じのはず。
そういわれると、`lock (任意のオブジェクト)` に正当性があるように感じます。

<pre class="source" title="lock (任意オブジェクト) の本来の意図">
<span class="reserved">class</span> <span class="type">Resource</span>;

<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Resource</span> <span class="field">_someResource</span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="reserved">lock</span> (<span class="field">_someResource</span>)
        {
            <span class="comment">// _someResource に対する操作をする。</span>
            <span class="comment">// _someResource を同時に操作されると困るんだから、「_someResource を lock」。</span>
        }
    }
}
</pre>

ですがまあ、実際のところこんなにきれいに `lock (x) { x に対する操作 }` になることはなく、
大体は先ほどのように「`lock` のためだけに1個追加で `object _syncObj` みたいなフィールドを用意」みたいなことになります。

これがめんどくさく…
とはいえ、面倒だからといって以下のようなことは**してはいけない**とされています。

<pre class="source" title="ダメ！絶対！">
<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="comment">// ✖</span>
        <span class="comment">// 任意のオブジェクトでロックできるということは、this でも行ける！</span>
        <span class="reserved">lock</span> (<span class="reserved">this</span>)
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticRun</span></span>()
    {
        <span class="comment">// ✖</span>
        <span class="comment">// 静的メソッド内では this がない…</span>
        <span class="comment">// そうだ、Type 型もオブジェクトじゃん！</span>
        <span class="reserved">lock</span> (<span class="reserved">typeof</span>(<span class="type">MultiThreadCode</span>))
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }
}
</pre>

「外に漏れるインスタンスでロックを取ってはいけない」というお作法があるからです。
以下のようなコードを書かれる可能性があって困ります。

<pre class="source" title="ダメな lock の例">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">MultiThreadCode</span>();

<span class="comment">// ここの lock と、MultiThreadCode.Run 内の lock (this) が同じオブジェクトをロックする。</span>
<span class="comment">// 意図しない挙動のはず。</span>
<span class="reserved">lock</span> (<span class="variable">x</span>)
{
}

<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="reserved">lock</span> (<span class="reserved">this</span>)
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }
}
</pre>

さらにいうと、外に漏れてダメなら以下のようなコードもダメになると。

<pre class="source" title="これもダメ">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">MultiThreadCode</span>();

<span class="comment">// ここの lock と、MultiThreadCode.Run 内の lock (_items) が同じオブジェクトをロックする。</span>
<span class="reserved">lock</span> (<span class="variable">x</span><span class="operator">.</span><span class="property">Items</span>)
{
}

<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="comment">// private だから一見外に漏れてない。</span>
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_items</span> <span class="operator">=</span> [];

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="reserved">lock</span> (<span class="field">_items</span>)
        {
            <span class="comment">// _items に Add/Remove とかしたり。</span>
        }
    }

    <span class="comment">// List としては公開していないものの、</span>
    <span class="comment">// インスタンス自体は _items そのままなので…</span>
    <span class="reserved">public</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="property">Items</span> <span class="operator">=&gt;</span> <span class="field">_items</span>;
}
</pre>

なのでまあ、元の話の戻りますが、結局は「`_items` とは別に
`object _syncObj = new();` を用意」みたいなことになります。

## .NET のオブジェクト ヘッダー

「任意のオブジェクトに対して `lock` を掛けれるという仕様は意外とオーバーヘッドが大きい」という話題があったりします。
なので、「ロック専用のクラスがあった方がいい」という話も。

* [Add first class System.Threading.Lock type](https://github.com/dotnet/runtime/issues/34812)

ここにこんな説明があります:

> Locking on any class has overhead from the dual role of the syncblock as both lock field and hashcode et al.
>
> (任意のクラスに対するロック操作は、ロック用の値とハッシュ値とか、syncblock に複数の役割を持たせていることによるオーバーヘッドを持つ。)

syncblock が何かという話は以下の英語の記事がわかりやすそう。

* [Object header get complicated](https://mycodingplace.wordpress.com/2018/01/10/object-header-get-complicated/)

ここの図を見ての通り、27ビット目の値によって、下位ビットをハッシュ値として使うか、ロック用に使うか分岐させています。

ところがまあ、[これのせいで分岐予測をミスりまくって、結構ペナルティ](https://github.com/dotnet/runtime/issues/34800)になるみたいです。
言われてみればそりゃそう。
`GetHashCode` と `lock` だったら `GetHashCode` の方が圧倒的に利用頻度高いでしょうから。
いざ `lock` しようとすると分岐予測当たらないのもしょうがなく。

(あと、`lock` 中のオブジェクトに対して override してない `object.GetHashCode` を呼ぶと遅くなります。)

で、ここで、前節の「どうせロック専用のインスタンスを作ることが多い」話と合わせると、
「だったらロック専用の `Lock` クラスを作って `private readonly Lock _syncObj = new();` しようよ」ということになったりします。

## System.Threading.Lock クラス

ということで、 .NET 9 では `Lock` クラス(`System.Threading` 名前空間)を追加するみたいです。
現状 (.NET 9 Preview 1 とか Preview 2 時点)では、
プレビュー扱いで `RequiresPreviewFeatures` 属性が付いていますが、
一応今でも実装が入っています。

C# の `lock` ステートメントをどうするかはいったん置いておいて(後述)、
以下のような使い方を想定しているクラスです。

<pre class="source" title="Lock クラス導入">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>Versioning;

<span class="comment">// 今のペースなら、.NET 9 正式リリースまでには外れる気はする。</span>
[<span class="reserved">module</span>: <span class="type">RequiresPreviewFeatures</span>]

<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Lock</span> <span class="field">_syncObj</span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="comment">// C# コンパイラーに手を入れないとしたらこんな使い方に。</span>
        <span class="comment">// lock じゃなくて using。</span>
        <span class="reserved">using</span> (<span class="field">_syncObj</span><span class="operator">.</span><span class="method">EnterScope</span>())
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }
}
</pre>

`Lock` クラスが何をやっているかというと、おおむね「`lock` が内部で使っている C++ コード([`AwareLock`](https://github.com/dotnet/runtime/blob/e5cf6905f6065b45f32f8780fe9645969e836ecf/src/coreclr/vm/syncblk.h#L157))を [C# に移植](https://github.com/dotnet/runtime/pull/87672)」です。
本当に、「オブジェクト ヘッダーの syncblock を使うのが高コスト」を避けるためのクラスという感じです。

## lock ステートメントで Lock インスタンス

ここで問題になるのが、じゃあ、`Lock` インスタンスに対して `lock` ステートメントを使うとどうなるの？というお話。
「`Lock` の時には `lock (x)` じゃなくて `using (x.EnterScope())` にしようね」とか言われても割と困るかと思います。
知らなきゃ確実に `lock (x)` と書くでしょうし、
知ってたって `lock (x)` をやらかす自信があります。

なので、C# 言語のレベルでも何らかの対処は必要だろうという話になります。
(おそらくその辺りが `RequiresPreviewFeatures` 属性付きになっている理由。)

* [`Lock` object](https://github.com/dotnet/csharplang/blob/main/proposals/lock-object.md)

検討段階では「`lock (x)` すると警告を出すみたいなのだけでもいいかもしれない」なんて話もありましたが、
まあ、「`lock (x)` と書いたらコンパイラーが `using (x.EnterScope())` に置き換える」路線で行くことになりました。

この実装、 Visual Studio 17.10.0 Preview 2.0 ([3週間くらい前](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/88))の時点で入ってるみたいです。
以下のコードを書いて、ILSpy とかでコンパイル結果の中身を覗くと `using (_syncObj.EnterScope())` に置き換わっています。

<pre class="source" title="Lock インスタンスに対する lock ステートメント">
<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Lock</span> <span class="field">_syncObj</span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Run</span>()
    {
        <span class="comment">// C# コンパイラーが特殊対応することになったので、lock で OK に。</span>
        <span class="reserved">lock</span> (<span class="field">_syncObj</span>)
        {
            <span class="comment">// いろんなスレッドから同時に呼ばれるコード。</span>
        }
    }
}
</pre>

ちなみに、現状は `Lock` クラス専用です。
珍しくパターン ベースでなく、`Lock` でないと認識せず。
まあ、需要がないんでしょうね。

<pre class="source" title="Lock 専用なので、自作はできず">
<span class="comment">// これは現状、既存の lock (Monitor.TryEnter を使ったコード)になる。 </span>
<span class="reserved">lock</span> (<span class="reserved">new</span> <span class="type">MyLock</span>())
{
}

<span class="comment">// System.Threading.Lock と同じパターンのメソッド持ちの自作クラス。</span>
<span class="reserved">class</span> <span class="type">MyLock</span>
{
    <span class="reserved">public</span> <span class="type struct">Scope</span> <span class="method">EnterScope</span>() <span class="operator">=&gt;</span> <span class="reserved">default</span>;

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Scope</span>
    {
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
    }
}
</pre>
