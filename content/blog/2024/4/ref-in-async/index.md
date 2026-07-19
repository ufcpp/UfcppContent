---
title: "ref/ref struct 変数を非同期メソッド中で使えるように"
source_url: "https://ufcpp.net/blog/2024/4/ref-in-async/"
content_type: "BlogEntry"
published_at: "2024-04-04T23:30:21"
updated_at: "2024-04-04T23:30:21"
tags: []
umbraco_id: 2498
parent_id: 2496
sort_order: 1
aliases: []
---

# ref/ref struct 変数を非同期メソッド中で使えるように

[前回の `Lock` クラスの話](../lock-class/index.md)を見てから、とりあえず以下のコードを見てほしい。

<pre class="source" title="非同期メソッド中でエラーに">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>Versioning;

[<span class="reserved">module</span>: <span class="type">RequiresPreviewFeatures</span>]

<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="reserved">object</span> <span class="field"><span class="static">_syncObj</span></span> <span class="operator">=</span> <span class="reserved">new</span>();
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">Lock</span> <span class="field"><span class="static">_syncLock</span></span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method"><span class="static">MIterator</span></span>()
    {
        <span class="reserved">lock</span> (<span class="field"><span class="static">_syncObj</span></span>) { } <span class="comment">// 旧来 lock。</span>
        <span class="reserved">lock</span> (<span class="field"><span class="static">_syncLock</span></span>) { } <span class="comment">// 新しい lock (VS 17.10p2 以降)。</span>

        <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="method"><span class="static">MAsync</span></span>()
    {
        <span class="reserved">lock</span> (<span class="static"><span class="field">_syncObj</span></span>) { }
        <span class="reserved">lock</span> (<span class="static"><span class="error" title="CS9217"><span class="field">_syncLock</span></span></span>) { } <span class="comment">// これだけダメ(VS 17.10p2 以降)。</span>

        <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>();
    }
}
</pre>

おそらく C# 13 正式リリースまでには直ると思うんですが、
どうしてこうなるのかと、どう対処する予定なのかという話になります。

ちなみに、単に `Lock` クラスに対して特殊処理をするという話ではなく、
もう少し汎用に「非同期メソッド中で ref ローカル変数を使えるようにする」という対処になります。


## lock の展開

[前回の話]で、今回関係するのは、`Lock` インスタンスに対する `lock` ステートメントが `using (x.EnterScope())` み化けるという点。
で、さらにいうと、`using` は以下のように展開されます。

<pre class="source" title="lock → using → try-finally">
<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">Lock</span> <span class="field"><span class="static">_syncLock</span></span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="comment">// 元コード。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">A</span></span>()
    {
        <span class="reserved">lock</span> (<span class="field"><span class="static">_syncLock</span></span>) { }
    }

    <span class="comment">// lock → using。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">B</span></span>()
    {
        <span class="reserved">using</span> (<span class="static"><span class="field">_syncLock</span></span><span class="operator">.</span><span class="method">EnterScope</span>()) { }
    }

    <span class="comment">// using → try-finally。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">C</span></span>()
    {
        <span class="type">Lock</span><span class="operator">.</span><span class="type struct">Scope</span> <span class="variable">scope</span> <span class="operator">=</span> <span class="field"><span class="static">_syncLock</span></span><span class="operator">.</span><span class="method">EnterScope</span>();
        <span class="control">try</span>
        {
        }
        <span class="control">finally</span>
        {
            <span class="variable">scope</span><span class="operator">.</span><span class="method">Dispose</span>();
        }
    }
}
</pre>

ここで、`Lock.Scope` は [ref struct](../../../../study/csharp/resource/refstruct.md) になっています。
これが先ほどのコードで非同期メソッド中の `lock (_syncLock)` がエラーになる原因です。
問題の本質としては以下のようなコードと同じ。

<pre class="source" title="非同期メソッド中では ref struct を使えない">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method"><span class="static">MIterator</span></span>()
    {
        <span class="comment">// イテレーター中では ref strcut を使える。</span>
        <span class="comment">// (ただし、yield をまたがない場合のみ。)</span>
        <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">1</span>];

        <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="method"><span class="static">MAsync</span></span>()
    {
        <span class="comment">// こちらはダメ。</span>
        <span class="error" title="CS4012"><span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;</span> <span class="variable">span</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">int</span>[<span class="number">1</span>];

        <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>();
    }
}
</pre>

イテレーターと非同期メソッドって、仕組みがかなり似ていて、「イテレーターでできて非同期メソッドでできない」ということは原理的にはあまりないんですが。
実際、上記の挙動は単に実装都合で、コストさえかければ「非同期メソッド中でも ref struct のローカル変数を書けるようにする」というのは可能です。

## イテレーターの中断と再開

「[イテレーターのコンパイル結果](../../../../study/csharp/data/sp2_iterator.md#complied)」辺りで書いてるんですが、
イテレーターは「中断と再開」をするようなコードが生成されます。

例えば以下のようなコードを書いたとき、

<pre class="source" title="イテレーターの例">
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="method">M</span>())
{
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span>);
}

<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="method">M</span>()
{
    <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
    <span class="control">yield</span> <span class="control">return</span> <span class="variable">x</span> <span class="operator">*</span> <span class="variable">x</span>;

    <span class="comment">// 式は適当。</span>
    <span class="comment">// ここで重要なのは、y は yield をまたがないということ。</span>
    <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="operator">++</span><span class="variable">x</span> <span class="operator">*</span> <span class="variable">x</span>;
    <span class="variable">y</span> <span class="operator">*=</span> <span class="variable">y</span>;

    <span class="control">yield</span> <span class="control">return</span> <span class="variable">y</span>;

    <span class="comment">// 同、z は yield をまたがない。</span>
    <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="operator">++</span><span class="variable">x</span>;
    <span class="variable">z</span> <span class="operator">*=</span> (<span class="number">2</span> <span class="operator">*</span> <span class="variable">x</span> <span class="operator">+</span> <span class="number">1</span>);

    <span class="control">yield</span> <span class="control">return</span> <span class="variable">z</span>;
}
</pre>

おおむね、以下のようなクラスが生成されます。
(簡単化のためちょこっとさぼっています。要点のみ。)

<pre class="source" title="上記イテレーターの解釈結果">
<span class="reserved">var</span> <span class="variable">e</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">MImpl</span>();
<span class="control">while</span> (<span class="variable">e</span><span class="operator">.</span><span class="method">MoveNext</span>())
{
    <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">e</span><span class="operator">.</span><span class="property">Current</span>);
}

<span class="reserved">class</span> <span class="type">MImpl</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_i</span> <span class="operator">=</span> <span class="number">0</span>;
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span> <span class="operator">=</span> <span class="number">1</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Current</span> { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>()
    {
        <span class="control">if</span> (<span class="field">_i</span> <span class="operator">==</span> <span class="number">0</span>)
        {
            <span class="property">Current</span> <span class="operator">=</span> <span class="field">_x</span> <span class="operator">*</span> <span class="field">_x</span>;
        }
        <span class="control">else</span> <span class="control">if</span> (<span class="field">_i</span> <span class="operator">==</span> <span class="number">1</span>)
        {
            <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="operator">++</span><span class="field">_x</span> <span class="operator">*</span> <span class="field">_x</span>;
            <span class="variable">y</span> <span class="operator">*=</span> <span class="variable">y</span>;
            <span class="property">Current</span> <span class="operator">=</span> <span class="variable">y</span>;
        }
        <span class="control">else</span> <span class="control">if</span> (<span class="field">_i</span> <span class="operator">==</span> <span class="number">2</span>)
        {
            <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="operator">++</span><span class="field">_x</span>;
            <span class="variable">z</span> <span class="operator">*=</span> (<span class="number">2</span> <span class="operator">*</span> <span class="field">_x</span> <span class="operator">+</span> <span class="number">1</span>);
            <span class="property">Current</span> <span class="operator">=</span> <span class="variable">z</span>;
        }
        <span class="control">else</span>
        {
            <span class="control">return</span> <span class="reserved">false</span>;
        }

        <span class="field">_i</span><span class="operator">++</span>;
        <span class="control">return</span> <span class="reserved">true</span>;
    }
}
</pre>

ここで重要なのは以下の点。

* `yield` をまたいで使う変数はフィールドに昇格する
* そうでないものはローカル変数のまま

つまり、「`yield` さえまたがなければ、ローカル変数に制限を掛ける必要はない」ということになります。
ここではイテレーターで話しましたが、非同期メソッドもほぼ同様で、
「`await` さえまたがなければ、ローカル変数に制限を掛ける必要はない」といえたりします。

ただまあ、これはあくまで「原理的には」という話であって、じゃあ、現在の実装がどうなっているかというと…
C# 12 時点では以下のような感じ。

<pre class="source" title="C# 12 時点での、ref/ref struct のイテレーター/非同期メソッド中での挙動">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>()
    {
        <span class="type struct">RefStruct</span> <span class="variable">rs</span> <span class="operator">=</span> <span class="reserved">new</span>();

        <span class="reserved">using</span> (<span class="variable">rs</span>) { }
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="variable">rs</span>) ;

        <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
        <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>;
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method"><span class="static">MIterator</span></span>()
    {
        <span class="type struct">RefStruct</span> <span class="variable">rs</span> <span class="operator">=</span> <span class="reserved">new</span>();

        <span class="reserved">using</span> (<span class="variable">rs</span>) { }
        <span class="error" title="CS8344"><span class="control">foreach</span></span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="variable">rs</span>) ; <span class="comment">// ダメ。</span>

        <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
        <span class="reserved">ref</span> <span class="reserved">int</span> <span class="error" title="CS8176"><span class="variable">r</span></span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>; <span class="comment">// ダメ。</span>

        <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="static"><span class="method">MAsync</span></span>()
    {
        <span class="type struct"><span class="error" title="CS4012">RefStruct</span></span> <span class="variable">rs</span> <span class="operator">=</span> <span class="reserved">new</span>(); <span class="comment">// 非同期メソッドだとこの時点でダメ。</span>

        <span class="reserved">using</span> (<span class="variable"><span class="error" title="CS9104">rs</span></span>) { } <span class="comment">// ダメ。</span>
        <span class="control"><span class="error" title="CS8344">foreach</span></span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="variable">rs</span>) ; <span class="comment">// ダメ。</span>

        <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
        <span class="reserved">ref</span> <span class="reserved">int</span> <span class="error" title="CS8177"><span class="variable">r</span></span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>; <span class="comment">// ダメ。</span>

        <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="static"><span class="method">Yield</span></span>();
    }
}

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">RefStruct</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }

    <span class="reserved">public</span> <span class="type struct">RefStruct</span> <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="reserved">this</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Current</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() <span class="operator">=&gt;</span> <span class="reserved">false</span>;
}
</pre>

どれも、「ref struct のローカル変数が認められるのであれば書けてもいいはずのコード」になります。
ところが、大丈夫なものとコンパイル エラーになるものがまちまち。

## ref/ref struct 変数を非同期メソッド中で使えるように

まあ既知の問題ではあったんですが。
これまで、需要がそこまでないからか、ずっと放置されていました。
ところが、今回「`Lock` クラスに対する `lock` ステートメント」問題が出たからか、急に対処することになったみたいです。

* [Add proposal for "ref and unsafe in iterators and async" #7994](https://github.com/dotnet/csharplang/pull/7994)

先ほどの、以下のようなコード、すべて「`yield`/`await` さえまたがなければ認める」ということになりそうです。

<pre class="source" title="C# 12 時点での、ref/ref struct のイテレーター/非同期メソッド中での挙動">
<span class="type struct">RefStruct</span> <span class="variable">rs</span> <span class="operator">=</span> <span class="reserved">new</span>();

<span class="reserved">using</span> (<span class="variable">rs</span>) { }
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="variable">rs</span>) ;

<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>;
</pre>

* ref ローカル変数
* ref struct のローカル変数
  * ref struct に対する `using` ステートメント
  * ref struct に対する `foreach` ステートメント

付随して、同じく「`yield`/`await` さえまたがなければ認める」という条件で、
[`unsafe` ブロック](../../../../study/csharp/interop/sp_unsafe.md#unsafe)も認めるそうです。

### lock 中の yield

逆に、「これまで書けちゃっていたけども、実はまずかった」というものに警告を出そうという話もあります。
それが「`lock` ステートメント中の `yield`」です。

<pre class="source" title="まずそうなコード: lock 中の yield">
<span class="reserved">class</span> <span class="type">MultiThreadCode</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="reserved">object</span> <span class="static"><span class="field">_syncObj</span></span> <span class="operator">=</span> <span class="reserved">new</span>();

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method"><span class="static">MIterator</span></span>()
    {
        <span class="reserved">lock</span> (<span class="field"><span class="static">_syncObj</span></span>)
        {
            <span class="comment">// これが書けちゃう。使い方によってはまずい。</span>
            <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
        }
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="method"><span class="static">MAsync</span></span>()
    {
        <span class="reserved">lock</span> (<span class="field"><span class="static">_syncObj</span></span>)
        {
            <span class="comment">// 非同期メソッドの場合、コンパイル エラーになるので大丈夫。</span>
            <span class="error" title="CS1996"><span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>()</span>;
        }
    }
}
</pre>

.NET の実装では、
「ロックの開始と終了(内部的には `Monitor.Enter` と `Monitor.Exit`)は同じスレッドでやらないといけない」という制限がありまして。
非同期メソッドの方はわかりやすく「`await` をまたぐと別スレッド」感があるのでコンパイルの時点でエラーにしています。

で、イテレーターの方も使い方によっては「`yield` をまたぐと別スレッドになることがある」という意味では危険で、
例えば、以下のようなコードを書くと実行時に `SynchronizationLockException` 例外が出ます。

<pre class="source" title="lock 中 yield で例外を起こす例">
<span class="reserved">object</span> <span class="variable">syncObj</span> <span class="operator">=</span> <span class="reserved">new</span>();

<span class="type">IEnumerable</span>&lt;<span class="reserved">object</span><span class="operator">?</span>&gt; <span class="method">M</span>()
{
    <span class="reserved">lock</span> (<span class="variable">syncObj</span>)
    {
        <span class="comment">// これが書けちゃう。使い方によってはまずい。</span>
        <span class="control">yield</span> <span class="control">return</span> <span class="reserved">null</span>;
    }
}

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="method">M</span>())
{
    <span class="comment">// M 内に非同期コードがなくても、利用側が非同期だった時点でアウト。</span>
    <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>();
}
</pre>

ということで、この「`lock` 中での `yield`」も警告を足すことになりそうです。
(いきなりエラーにすると破壊的変更になるのでとりあえず警告。
何バージョンかかけてエラーに変更する可能性はあり。)

(※ 「スレッドをまたいだ `lock` を書けるようにする」みたいなことはしません。)
