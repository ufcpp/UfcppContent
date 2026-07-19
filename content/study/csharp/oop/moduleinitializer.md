---
title: "モジュール初期化子"
source_url: "https://ufcpp.net/study/csharp/oop/moduleinitializer/"
content_type: "Article"
published_at: "2021-02-21T00:00:00"
updated_at: "2021-02-21T21:06:34"
tags: []
umbraco_id: 2329
parent_id: 1248
sort_order: 6
aliases:
  - "/csharp/oop/moduleinitializer/"
---

# モジュール初期化子

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version9">Ver. 9</h5>

プログラムの実行時、最初に1回だけ呼び出したい処理が必要になることがあります。
「[静的コンストラクター](oo_static.md#ctor)」で説明しているように、この静的コンストラクターという機能を使っても「最初に1回だけ呼ばれる」ということができますが、C# 9.0 ではモジュール初期化子という書き方もできるようになりました。

## <a id="sec-generated-title-2"></a> <a id="module-initializer">モジュール初期化子</a>

「[静的コンストラクター](oo_static.md#ctor)」を使うとプログラム中で1回だけ呼び出される処理を書くことができます。
静的コンストラクターが呼び出されるタイミングは、そのクラスのなんらかのメンバーに初めてアクセスしたときです。

C# 9.0 では、もう1種類、「最初に1回だけ呼ばれる」という性質の処理の書き方ができるようになりました。
以下のように、`ModuleInitilizer` 属性(`System.Runtime.CompilerServices` 名前空間)を付けた[静的メソッド](oo_static.md#stmethod)を書くと、それが必ず1回呼び出されるようになります。

<pre class="source" title="ModuleInitialize 属性">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;必ず1回だけ呼ばれる&quot;</span>);
    }
}
</code></pre>

これを<strong id="key-module-initializer" class="keyword">モジュール初期化子</strong>(module initializer)と呼びます。

静的コンストラクターとの差は以下の通りです。

- 1つのクラスに複数のモジュール初期化子を書ける
  - [`partial`](oo_class.md#partial) を使って複数のファイルに分かれていても全部呼ばれる
- そのクラスを含んでいるモジュールを読み込んだ時点で呼ばれる

静的コンストラクターの呼び出しには「そのクラスのなんらかのメンバーにアクセス」という条件が付くので、確実に呼び出される保証が実はなかったりします。
モジュール初期化子の呼び出しも「モジュールを読み込む」(モジュールに含まれているなんらかの型に触れる)という条件は付くんですが、静的コンストラクターと比べればだいぶ確実に呼ばれます。
(一切何の型も使わないモジュールを参照すること自体がほとんどないので、実質的には「確実」と行ってしまっても構わないと思います。)

### <a id="sec-generated-title-3"></a> <a id="module-initializer-impl">モジュール初期化子の実装方法</a>

「モジュール読み込み時に必ず呼ばれる」というもの自体は .NET Framework 1.0 の頃から実はありました(単に C# から使う手段がなかっただけ)。
当初から、「`<Module>` という特殊な名前のクラスの静的コンストラクターは、モジュール読み込み時に必ず1回呼ばれる」という仕様があります。
`<>` を含む名前なので通常の C# コードで書くことはできませんし、C# 8.0 まではこの型の静的コンストラクターを書き出す手段もありませんでした。

C# 9.0 のモジュール初期化子がやっていることはこの「`<Module>` クラスの静的コンストラクターの生成」です。
例えば以下のようなコードを書いたとすると、

<pre class="source" title="モジュール初期化子(&lt;Module&gt; クラスの生成元)">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">class</span> <span class="type">C1</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init1</span>() { }
 
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init2</span>() { }
}
 
<span class="reserved">class</span> <span class="type">C2</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init1</span>() { }
 
}
</code></pre>

以下のようなコードに相当するものがコンパイラーによって追加されます。

<pre class="source" title="モジュール初期化子から生成される &lt;Module&gt; クラス">
<code><span class="reserved">class</span> <span class="type">&lt;Module&gt;</span>
{
    <span class="reserved">static</span> <span class="type">&lt;Module&gt;</span>()
    {
        <span class="type">C1</span>.<span class="method">Init1</span>();
        <span class="type">C1</span>.<span class="method">Init2</span>();
        <span class="type">C2</span>.<span class="method">Init1</span>();
    }
}
</code></pre>

1つの静的コンストラクターの中に単なるメソッド呼び出しが並べられているだけの状態になります。
したがって、以下のような性質があります。

- トータルでの呼び出しコストは静的コンストラクターをたくさん並べるよりも軽い
  - (静的コンストラクターは「[マルチスレッド](../async/sp_thread.md)実行時でも1回限り呼ぶ」という処理が必要で、通常のメソッド呼び出しよりも少し負担が大きい。モジュール初期化子はその負担が1回だけで済む)
- (意図せずコードを残してしまうと)本当は不要であっても必ず呼ばれる
- 呼び出しの負荷がモジュール読み込み時に集中する

### <a id="sec-generated-title-4"></a> <a id="module-initialize-usage">モジュール初期化子の用途</a>

.NET 6.0 では iOS や [WebAssembly](https://ja.wikipedia.org/wiki/WebAssembly) 上での実行のサポートが入ります。
(iOS や WebAssembly 上で C# が動くという状況はもっと前からあったんですが、
Windows で動いていた .NET とは別系統で保守されていました。
それが、 .NET 6.0 で統合されて1つの「.NET」になりました。)

用途が増えると、これまでの用途では動いていたものが新しい用途では動かせない・動いても効率が悪いということがあります。
実際、iOS や WebAssembly 環境では[リフレクション](../dynamic/sp_reflection.md)を使いづらいです。

例えば以下のように、文字列で型名を指定して、その型のインスタンスを生成するということを考えてみます。
(こういうコードをそのまま書くことはないですが、JSON などにシリアライズ・デシリアライズしたりするときにこれに類する処理が内部的に行われたりします。)

<pre class="source" title="文字列で型名を指定してインスタンス生成">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Reflection;
 
<span class="comment">// リフレクションを使えば文字列からその名前の型のインスタンスを作れる。</span>
<span class="comment">// ただ、パフォーマンスはあんまりよくない。</span>
<span class="reserved">object</span>? <span class="method">CreateInstance</span>(<span class="reserved">string</span> <span class="variable">typeName</span>)
{
    <span class="control">if</span> (<span class="type">Assembly</span>.<span class="method">GetExecutingAssembly</span>().<span class="method">GetType</span>(<span class="variable">typeName</span>) <span class="reserved">is</span> { } t) <span class="control">return</span> <span class="type">Activator</span>.<span class="method">CreateInstance</span>(<span class="variable">t</span>);
    <span class="control">else</span> <span class="control">return</span> <span class="reserved">null</span>;
}
 
<span class="comment">// ただ、 &quot;A&quot;, &quot;B&quot; という文字列が型名を指しているかどうかはコンパイラーが関知することではなく、</span>
<span class="comment">// 「クラス A, B は誰も使っていない」誤判定を受けることがある。</span>
<span class="comment">// AOT (事前ネイティブコード化)実行環境だと A, B が消し去られて、上記 GetType に失敗しうる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">CreateInstance</span>(<span class="string">&quot;A&quot;</span>));
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">CreateInstance</span>(<span class="string">&quot;B&quot;</span>));
 
<span class="reserved">class</span> <span class="type">A</span>
{
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
}
</code></pre>

このコードは直接的にクラス `A`、`B` を使っているコードがどこにもありません。
かなり頑張ってコードを追えば、`"A"` という文字列が `A` というクラスを指していて、
それをリフレクション(`Activator.CreateInstance`)越しに使っていることがわからなくはないんですが、コンパイラーが機械的に判定すると「`A` も `B` も使われていない」という判定を受けます。

一方で、C# 9.0 の世代では [source generator](../misc/analyzer-generator.md) (ソースコード生成)の仕組みが導入されました。
source generator 導入の動機の1つに「これまでリフレクションでやっていたような処理をコンパイル時にやりたい」というものがあります。
先ほどの `Activator.CreateInstance` を使っていた処理も、source generator を使って、「最初に1回どこかで初期化処理をする」みたいなものに置き換えることが考えられます。
例えば、以下のように、`CreateInstance` 的な処理を自前管理することを考えます。

<pre class="source" title="リフレクションをなくすために、自前で CreateInstance 的なものを管理">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="comment">// どこか必ず1回呼ばれる保証のあるものを使って、事前に string → Func&lt;object&gt; な辞書を作っておくという発想。</span>
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">TypeRepository</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="type">Func</span>&lt;<span class="reserved">object</span>&gt;&gt; _factories = <span class="reserved">new</span>();
 
    <span class="comment">// 型名からインスタンスを作る。Register がどこかで呼ばれる前提。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">object</span>? <span class="method">CreateInstance</span>(<span class="reserved">string</span> <span class="variable">typeName</span>) =&gt; _factories.<span class="method">TryGetValue</span>(<span class="variable">typeName</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">f</span>) ? <span class="variable">f</span>() : <span class="reserved">null</span>;
 
    <span class="comment">// 型名 → インスタンス生成デリゲートを登録。</span>
    <span class="comment">// 静的コンストラクターで呼んでもらう想定だと破綻気味だったけど、モジュール初期化子なら割と成立する。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Register</span>(<span class="reserved">string</span> <span class="variable">typeName</span>, <span class="type">Func</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">factory</span>) =&gt; _factories.<span class="method">Add</span>(<span class="variable">typeName</span>, <span class="variable">factory</span>);
}

</code></pre>

ここで静的コンストラクターだと「呼ばれる保証がない」という点が問題になります。
例えば以下のコードのように変な挙動をしたりします。

<pre class="source" title="静的コンストラクターだと呼ばれないことがあるので困る例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="comment">// 後述するように、静的コンストラクターはこの用途だと呼ばれない。</span>
<span class="comment">// なので、Register が呼ばれてなくて、CreateInstance が null を返す。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;A&quot;</span>)); <span class="comment">// null</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;B&quot;</span>)); <span class="comment">// null</span>
 
<span class="comment">// これが例えば、どこでもいいから1度 A のメンバーを空呼びすると上記コードがちゃんと new A(), new B() を返すようになる。</span>
<span class="comment">// 静的コンストラクターが呼ばれるタイミングは「その型のメンバーを最初に使った直後」</span>
<span class="reserved">_</span> = <span class="reserved">new</span> <span class="type">A</span>(); <span class="comment">// このタイミングで A の静的コンストラクターが呼ばれる</span>
<span class="reserved">_</span> = <span class="reserved">new</span> <span class="type">B</span>(); <span class="comment">// このタイミングで B の静的コンストラクターが呼ばれる</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;A&quot;</span>)); <span class="comment">// A</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;B&quot;</span>)); <span class="comment">// B</span>
 
<span class="comment">// 手書きはあまりしたくないものの、Source Generator がある今、</span>
<span class="comment">// 必要な型に対して以下のようなコード生成をするのは十分現実的。</span>
<span class="comment">// ただ、静的コンストラクターは呼ばれるタイミングに問題があって…</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">static</span> <span class="type">A</span>() =&gt; <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">nameof</span>(<span class="type">A</span>), () =&gt; <span class="reserved">new</span> <span class="type">A</span>());
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">static</span> <span class="type">B</span>() =&gt; <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">nameof</span>(<span class="type">B</span>), () =&gt; <span class="reserved">new</span> <span class="type">B</span>());
}
</code></pre>

モジュール初期化子なら確実に呼ばれる保証が強いのでこの問題を解決できます。
以下のコードであれば意図した挙動になります。

<pre class="source" title="モジュール初期化子なら呼び出される保証が強いので楽という例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="comment">// モジュール初期化子の場合、その型を含むモジュール(dll とか exe とか)がロードされた直後に必ず呼ばれる。</span>
<span class="comment">// 静的コンストラクターの「型に触れた瞬間」よりは確実に呼ばれる保証あり。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;A&quot;</span>)); <span class="comment">// A</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="type">TypeRepository</span>.<span class="method">CreateInstance</span>(<span class="string">&quot;B&quot;</span>)); <span class="comment">// B</span>
 
<span class="comment">// 静的コンストラクターだと呼ばれるタイミングが不定で問題があったけど、モジュール初期化子なら大丈夫。</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init</span>() =&gt; <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">nameof</span>(<span class="type">A</span>), () =&gt; <span class="reserved">new</span> <span class="type">A</span>());
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init</span>() =&gt; <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">nameof</span>(<span class="type">B</span>), () =&gt; <span class="reserved">new</span> <span class="type">B</span>());
}
</code></pre>

## <a id="sec-generated-title-5"></a> <a id="generics"></a>ジェネリックな型

逆に静的コンストラクターでないと書けないものもあります。
ジェネリックな型に対してはモジュール初期化子を定義できません。

<pre class="source" title="ジェネリックな型に対するモジュール初期化はコンパイル エラーになる">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Generic</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// これはコンパイル エラー。</span>
    <span class="comment">// 静的コンストラクターなら、 Generic&lt;int&gt; みたいな具象化した型ごとに呼ばれるけど、</span>
    <span class="comment">// モジュール初期化のタイミングでは何の型で具象化されるかわからなくて呼びようがない。</span>
    [<span class="error"><span class="type">ModuleInitializer</span></span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init1</span>() { }
}
</code></pre>

[前節](#module-initialize-usage)で書いたような用途でモジュール初期化をジェネリック型に対して使いたい場合、
以下のように、非ジェネリックな型を1つ用意して、その中で想定しうるすべての型を列挙するなどの対処が必要になります。

<pre class="source" title="非ジェネリックな型のモジュール初期化に初期化処理を集約する必要あり">
<code><span class="comment">// 前節のようなことをジェネリックな型に対してしようとすると…</span>
<span class="reserved">class</span> <span class="type">Generic</span>&lt;<span class="type">T</span>&gt;
{
}
 
<span class="comment">// 非ジェネリックなものを1個用意して、</span>
<span class="reserved">class</span> <span class="type">Generic</span>
{
    [<span class="type">ModuleInitializer</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Init</span>()
    {
        <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">typeof</span>(<span class="type">Generic</span>&lt;&gt;) + <span class="string">&quot;&lt;int&gt;&quot;</span>, () =&gt; <span class="reserved">new</span> <span class="type">Generic</span>&lt;<span class="reserved">int</span>&gt;());
        <span class="type">TypeRepository</span>.<span class="method">Register</span>(<span class="reserved">typeof</span>(<span class="type">Generic</span>&lt;&gt;) + <span class="string">&quot;&lt;string&gt;&quot;</span>, () =&gt; <span class="reserved">new</span> <span class="type">Generic</span>&lt;<span class="reserved">string</span>&gt;());
        <span class="comment">// 以下、使うことがわかっている限りの具象型を並べる必要がある。</span>
    }
}
</code></pre>
