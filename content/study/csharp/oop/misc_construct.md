---
title: "[雑記] コンストラクター内の仮想メソッド呼び出し"
source_url: "https://ufcpp.net/study/csharp/oop/misc_construct/"
content_type: "Article"
published_at: "2007-10-06T00:00:00"
updated_at: "2023-11-04T00:00:00"
tags: []
umbraco_id: 1266
parent_id: 1248
sort_order: 14
aliases:
  - "/csharp/misc_construct"
  - "/csharp/misc_construct.html"
  - "/csharp/oop/misc_construct/"
  - "/study/csharp/misc_construct"
  - "/study/csharp/misc_construct.html"
---

# \[雑記\] コンストラクター内の仮想メソッド呼び出し

## <a id="sec-generated-title-1"></a> <a id="abst">概要</a>

継承構造を持つクラスのコンストラクターの挙動と注意点の話を少々。


## <a id="sec-generated-title-2"></a> <a id="ctor-order">コンストラクターの実行順序</a>

派生クラスのインスタンスが生成される際、
派生クラスのコンストラクターの前に、基底クラスのコンストラクターが呼び出されます。

<pre class="source" title="基底クラスのコンストラクターが呼ばれる">
<span class="comment">// コンストラクター呼び出し。</span>
<span class="reserved">new</span> <span class="type">D</span>();

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;base&quot;</span>);
}

<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">D</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;derived&quot;</span>);
}
</pre>


<pre class="console" title="実行結果">
base
derived
</pre>


なので、派生クラスのコンストラクター内では、
基底クラスのメンバーはちゃんと初期化済みだと思って使えます。

<pre class="source" title="コンストラクター内で基底クラスのメンバーを使用">
<span class="reserved">var</span> <span class="variable">d</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">D</span>();
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">d</span><span class="operator">.</span><span class="field">Y</span>); <span class="comment">// 25</span>

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> <span class="field">X</span>;
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="field">X</span> <span class="operator">=</span> <span class="number">5</span>;
}

<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> <span class="field">Y</span>;

    <span class="comment">// B() の実行が先。X は 5 になってる。</span>
    <span class="comment">// ↓ ちゃんと y == 25 になる。</span>
    <span class="reserved">public</span> <span class="type">D</span>() <span class="operator">=&gt;</span> <span class="field">Y</span> <span class="operator">=</span> <span class="field">X</span> <span class="operator">*</span> <span class="field">X</span>;
}
</pre>


<pre class="console" title="実行結果">
25
</pre>



## <a id="sec-generated-title-3"></a> <a id="virtual">仮想メソッド呼び出し</a>

「派生クラスのコンストラクターの前に基底クラスのコンストラクターが呼ばれる」というルールは、
たいていどの言語でも同じルールです。
C++ でも Java でもそういうルールでコンストラクターを呼び出します。

でも、1つだけ注意すべき点があります。
コンストラクター中の仮想メソッド呼び出しの扱いに関して。
例えば、以下のような感じ。

<pre class="source" title="基底クラスのコンストラクターで仮想メソッドを呼ぶ">
<span class="reserved">new</span> <span class="type">D</span>(); <span class="comment">// C# のルールだと &quot;derived&quot; の方が表示される。</span>

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method">Name</span>());

    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="string">&quot;base&quot;</span>;
}

<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="string">&quot;derived&quot;</span>;
}
</pre>


この類のコードの挙動は C++ と C# で違います。
C++ で、このコードに相当するものを書いて実行すると、
base と表示されます。
派生クラス D のインスタンスを生成しているにもかかわらず、
基底クラス B の Name メソッドが呼ばれます。

<pre class="console" title="C++ で同様のプログラムを書いて実行させた結果">
base
</pre>


一方、C# では、以下のように、派生クラスの Name メソッドが呼ばれます。

<pre class="console" title="C# での実行結果">
derived
</pre>


仮想メソッド（あるいは、C++ では仮想関数と呼ぶ）の呼び出しは、
仮想メソッドテーブル（仮想関数テーブル）というものを通して行います。
Name というメソッドが呼ばれたときに、
実際にはどのメソッド（D.Name なのか B.Name なのか）を呼べばいいか、
テーブル中に参照情報が書かれていて、
それを見て実際のメソッド呼び出しが行われます。

で、C++ では、コンストラクターの頭で仮想関数テーブルが更新されます。
基底クラスのコンストラクター内では、まだ仮想関数テーブルが派生クラスのものに更新されていません。

一方、C# では、仮想メソッドテーブルの更新だけは先にして、
それから基底クラスのコンストラクター → 派生クラスのコンストラクターの順で処理が行われます。


### <a id="sec-generated-title-4"></a> <a id="order">余談： 初期化子とコンストラクターの実行順序</a>

「[コンストラクター初期化子](oo_construct.md#initializer)」で説明したように、
初期化の順序は、

1. 派生クラスのメンバー初期化子
2. 基底クラスのメンバー初期化子
3. 基底クラスのコンストラクター本体
4. 派生クラスのコンストラクター本体

という順序になります。
以下のようなコードを書くと実行順序がはっきりします。

<pre class="source" title="初期化の実行順序">
<span class="comment">// コンストラクター呼び出し。</span>
<span class="reserved">new</span> <span class="type">Derived</span>();

<span class="reserved">class</span> <span class="type">Member</span>
{
    <span class="reserved">public</span> <span class="type">Member</span>(<span class="reserved">string</span> <span class="variable local">s</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">Member </span>{<span class="variable local">s</span>}<span class="string">&quot;</span>);
}

<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">Member</span> <span class="field">X</span> <span class="operator">=</span> <span class="reserved">new</span>(<span class="string">&quot;base&quot;</span>); <span class="comment">// 2.</span>

    <span class="reserved">public</span> <span class="type">Base</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Base()&quot;</span>); <span class="comment">// 3.</span>
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">Member</span> <span class="field">Y</span> <span class="operator">=</span> <span class="reserved">new</span>(<span class="string">&quot;derived&quot;</span>); <span class="comment">// 1.</span>

    <span class="reserved">public</span> <span class="type">Derived</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Derived()&quot;</span>); <span class="comment">// 4.</span>
}
</pre>


<pre class="console" title="実行結果">
Member derived
Member base
Base()
Derived()
</pre>


で、メンバー変数初期化子を使って値を設定した変数は、
基底クラスのコンストラクターが呼ばれた時点ですでにきちんと初期化済みな事が保証されます。
基底クラスから仮想メソッドを呼び出す場合、
このことに留意してコードを書くとトラブルになりにくいです。


## <a id="sec-generated-title-5"></a> <a id="problem">コンストラクター中での仮想メソッド呼び出しの問題点</a>

基底クラスのコンストラクター内から仮想メソッドを呼んだとき、
ちゃんと動的な型に基づいて派生クラスのメソッドが呼ばれるわけですが、
この動作には1つ問題があります。
基底関数のコンストラクター内で仮想メソッドが呼ばれた時点では、
派生クラスのメンバー変数は初期化されていない（派生クラスのコンストラクターはまだ呼ばれてない）んですよね。
例えば、以下のコードを見てください。

<pre class="source" title="初期化されるまえにメンバー変数を参照してしまう">
<span class="comment">// コンストラクター呼び出し。</span>
<span class="reserved">new</span> <span class="type">D</span>(<span class="string">&quot;derived&quot;</span>);

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="method">Name</span>()); <span class="comment">// D() の中身より先に実行される。</span>

    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="string">&quot;anonymous&quot;</span>;
}

<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>
{
    <span class="reserved">private</span> <span class="reserved">string</span> <span class="field">_name</span>;

    <span class="reserved">public</span> <span class="type">D</span>(<span class="reserved">string</span> <span class="variable local">name</span>) <span class="operator">=&gt;</span> <span class="field">_name</span> <span class="operator">=</span> <span class="variable local">name</span>;

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="field">_name</span>; <span class="comment">// D() 実行前に呼ばれるとまだ _name の初期化が終わってない。</span>
}
</pre>


前節の内容と比べて何が違うかというと、D.Name メソッド内で派生クラスのメンバー変数である name の値を読み出しています。

B のコンストラクター内で Name メソッドが呼ばれた時点では、
まだ D のコンストラクターは実行されていません。
したがって、name 変数はまだ初期化されていない（null になっている）状態で、
結局、このコードの実行結果は何も出力されません。

### <a id="sec-generated-title-6"></a> <a id="primary-constructor">プライマリ コンストラクターでの解決</a>

<h5 class="version version12">Ver. 12</h5>

ちなみにこの問題はプライマリ コンストラクターで解決できたりします。

「メンバー初期化子は派生クラスの方が先に実行される」という仕様で、
プライマリ コンストラクターの場合はメンバー初期化子を使うことになるので、
初期化処理が実行されるタイミングが早くなります。

<pre class="source" title="プライマリ コンストラクターを使うと初期化タイミングが早い">
<span class="comment">// コンストラクター呼び出し。</span>
<span class="reserved">new</span> <span class="type">D</span>(<span class="string">&quot;derived&quot;</span>);

<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">B</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="method">Name</span>()); <span class="comment">// D のメンバー初期化子よりは後に実行される。</span>

    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="string">&quot;anonymous&quot;</span>;
}

<span class="comment">// 先ほどのコードのコンストラクターをプライマリ コンストラクター形式に変更。</span>
<span class="reserved">class</span> <span class="type">D</span>(<span class="reserved">string</span> <span class="variable local">name</span>) : <span class="type">B</span>
{
    <span class="reserved">private</span> <span class="reserved">string</span> <span class="field">_name</span> <span class="operator">=</span> <span class="variable local">name</span>; <span class="comment">// フィールド初期化子になったことで、実行タイミングが早くなる。</span>

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">Name</span>() <span class="operator">=&gt;</span> <span class="field">_name</span>; <span class="comment">// B() 実行前に _name = name が呼ばれてて、期待通りの動作になる。</span>
}
</pre>

<pre class="console" title="実行結果">
derived
</pre>


## <a id="sec-generated-title-7"></a> <a id="summary">まとめ</a>

C# では、コンストラクター内での仮想メソッド呼び出しは、
動的な型に基づいて呼び出されます。

ただし、メンバー変数を読み出すような仮想メソッドをコンストラクター内から呼び出すと、
正しい値が読めないので注意が必要です。
（メンバー変数にアクセスしない場合や、値を書き込む方は OK。）

C# 12 で追加されたプライマリ コンストラクターでは、通常のコンストラクターと比べてメンバーの初期化タイミングが早くなる点にも注意が必要かもしれません。
