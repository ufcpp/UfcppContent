---
title: "ローカル関数と匿名関数"
source_url: "https://ufcpp.net/study/csharp/functional/fun_localfunctions/"
content_type: "Article"
published_at: "2016-07-17T00:00:00"
updated_at: "2023-07-29T00:00:00"
tags: []
umbraco_id: 1929
parent_id: 1275
sort_order: 4
aliases:
  - "/csharp/functional/fun_localfunctions/"
---

# ローカル関数と匿名関数

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
C# には、関数内に関数を書く方法として、ローカル関数と匿名関数という2つの機能があります。

いずれも共通して、以下のような性質があります。

- 定義している関数の中でしか使えない
- 周りの(定義している関数側にある)ローカル変数を取り込める

ローカル関数の方ができることは多いですが、書ける場所は少なくなります。
匿名関数はその逆で、できることに少し制限がある代わりに、どこにでも書けます。

サンプル コード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Functional/LocalFunctions](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Functional/LocalFunctions)

##<a id="sec-generated-title-2"></a> <a id="local-function"></a>ローカル関数
<h5 class="version version7">Ver. 7</h5>

C# 7では、関数の中で別の関数を定義して使うことができます。
関数の中でしか使えないため、<strong id="key-local">ローカル関数</strong>(local function: その場所でしか使えない関数)と呼びます。

例えば以下のように書けます。

<pre class="source" title="ローカル関数の例">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// Main 関数の中で、ローカル関数 f を定義</span>
        <em><span class="reserved">int</span> f(<span class="reserved">int</span> n) =&gt; n &gt;= 1 ? n * f(n - 1) : 1;</em>

        <span class="type">Console</span>.WriteLine(f(10));
    }
}
</code></pre>

ローカル関数(この例でいう `f`)は、定義した関数(この例でいう `Main`メソッド)の中でしか使えません。

ローカル関数は、通常のメソッドでできることであれば概ね何でもできます。例えば、以下のようなこともできます。

- 再帰呼び出し
- イテレーター
- 非同期メソッド

また、メソッド内に限らず、[関数メンバー](../structured/st_function.md#sec-function-member)ならどれの中でも定義できます。

<pre class="source" title="メソッドに限らず、プロパティやコンストラクター、演算子等の中でローカル関数を定義する例">
<code><span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> Sample()
    {
        <span class="reserved">int</span> f(<span class="reserved">int</span> n) =&gt; n * n;
    }

    <span class="reserved">public</span> <span class="reserved">int</span> Property
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">int</span> f(<span class="reserved">int</span> n) =&gt; n * n;
            <span class="reserved">return</span> f(10);
        }
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Sample</span> <span class="reserved">operator</span>+(<span class="type">Sample</span> x)
    {
        <span class="reserved">int</span> f(<span class="reserved">int</span> n) =&gt; n * n;
        <span class="reserved">return</span> <span class="reserved">null</span>;
    }
}
</code></pre>

###<a id="sec-generated-title-3"></a> <a id="local-function-attribute"></a>ローカル関数への属性適用
<h5 class="version version9">Ver. 9.0</h5>

ローカル関数の追加当初、ローカル関数には属性を付けれなかったんですが、C# 9.0 でできるようになりました。

<pre class="source" title="ローカル関数に属性を付ける">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Diagnostics.CodeAnalysis;
 
<span class="method">m</span>(<span class="string">&quot;&quot;</span>, <span class="string">&quot;&quot;</span>);
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span>? <span class="variable">a</span>, <span class="reserved">string</span>? <span class="variable">b</span>)
{
    <span class="comment">// C# 9.0 からローカル関数に属性を付けれる。</span>
    <span class="comment">// C# 8.0 の null 許容参照型がらみで特に有用。</span>
    [<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="string">&quot;s&quot;</span>)]
    <span class="reserved">string</span>? <span class="method">toLower</span>(<span class="reserved">string</span>? <span class="variable">s</span>) =&gt; <span class="variable">s</span>?.<span class="method">ToLower</span>();
 
    <span class="control">if</span> (<span class="variable">a</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span> &amp;&amp; <span class="variable">b</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
    {
        <span class="comment">// a, b の null 許容性が、NotNullIfNotNull 属性のおかげで al, bl に伝搬。</span>
        <span class="reserved">string</span> <span class="variable">al</span> = <span class="method">toLower</span>(<span class="variable">a</span>);
        <span class="reserved">string</span> <span class="variable">bl</span> = <span class="method">toLower</span>(<span class="variable">a</span>);
 
        <span class="comment">// a, b が非 null なので、al, bl は非 null で確定済み。改めてのチェック不要。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">al</span>.<span class="method">GetHashCode</span>());
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">bl</span>.<span class="method">GetHashCode</span>());
    }
}
</code></pre>

ローカル関数が追加された C# 7.0 時点で特に属性を付けれない積極的な理由はなく、
9.0 で入ったのは単に実装都合です。
(メソッド本体(`{}` の中身)内で属性を使えるような文法がこれまで全くなくて、
新たに書かないといけないコードが案外多く、
単純な割には実装コストが高くて後回しになっていただけ。
C# 8.0 の [null 許容参照型](../resource/nullablereferencetype.md)がらみでローカル関数にも属性を付けたい需要が急激に増えたので実装優先度が上がったみたいです。)

###<a id="sec-generated-title-4"></a> <a id="local-function-usage"></a>ローカル関数の使い道
ローカル関数を使いたくなる一番の動機は、定義した関数内からだけ使えるというになるでしょう。

あるメソッド`M`の中から、その`M`でしか使わないメソッドを呼び出したい場面が時々あります。
このとき、ローカル関数を使わないと、`M`でしか使わないメソッドに`MInternal`など、あまり意味のない名前を付ける羽目になり、不格好です。

<pre class="source" title="不格好な Internal メソッド">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">void</span> M()
{
    <span class="comment">// 何らかの前準備とか</span>
    MInternal();
}

<span class="reserved">static</span> <span class="reserved">void</span> MInternal()
{
    <span class="comment">// 実際の処理はこちらで</span>
}
</code></pre>

名前が不格好な程度ならそれほど大きな問題ではないんですが、
この`MInternal`は、`M`以外のメソッドからも呼べてしまうという問題が発生します。
こういう場合に、ローカル関数を使えば、以下のように書くことができ、呼びたい場所からだけ呼べるようになります。

<pre class="source" title="ローカル関数を使って呼べる場所をメソッド内に限定">
<code><span class="reserved">static</span> <span class="reserved">void</span> M()
{
    <span class="comment">// 何らかの前準備とか</span>

    <span class="reserved">void</span> m()
    {
        <span class="comment">// 実際の処理はこちらで</span>
    }

    m();
}
</code></pre>

####<a id="sec-generated-title-5"></a> <a id="iterator"></a>例1: イテレーターの引数チェック
例えば、[イテレーター](../data/sp2_iterator.md#iterator)の引数チェックではこういうコードが必要になりがちです。

例として、標準ライブラリ中の処理を1つ自作してみましょう。`Enumerable`クラス(`System.Linq`名前空間)の`Where`メソッドをまねてみます。
まず、単純な書き方をしてみましょう。この書き方には、コメントに書いてあるように、少し欠陥があります。

<pre class="source" title="Whereをまねたもの(欠陥あり)">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; Where&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="reserved">bool</span>&gt; predicate)
    {
        <span class="comment">// イテレーター中のコードは、最初に列挙した(foreach などに渡す)時に初めて実行される</span>
        <span class="comment">// このメソッドを呼んだ時点では、↓この引数チェックが働かない</span>
        <span class="reserved">if</span> (source == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(source));
        <span class="reserved">if</span> (predicate == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(predicate));

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> source)
            <span class="reserved">if</span> (predicate(x))
                <span class="reserved">yield</span> <span class="reserved">return</span> x;
    }
}
</code></pre>

コメント中に「メソッドを呼んだ時点では引数チェックが働かない」とありますが、使う側のコードも書いてみると問題がよりはっきりするでしょう。
以下のように、期待されるのと異なるタイミングで例外が起きます。

<pre class="source" title="欠陥版の問題点の例">
<code><reserved></span><span class="reserved">using</span> Iterator1;
<span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">IEnumerable</span>&lt;<span class="reserved">string</span>&gt; input = <span class="reserved">null</span>;

        <span class="comment">// input が null なので例外を投げてほしい</span>
        <span class="comment">// 多くの人がそれを期待する</span>
        <span class="reserved">var</span> output = input.Where(x =&gt; x.Length &lt; 10);

        <span class="type">Console</span>.WriteLine(<span class="string">"ここが表示されるとおかしい"</span>); <span class="comment">// でも表示される</span>

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> output) <span class="comment">// 実際に例外が出るのはこの行</span>
        {
            <span class="type">Console</span>.WriteLine(x);
        }
    }
}
</code></pre>

そこで、よく以下のような書き方をします。

<pre class="source" title="Whereをまねたもの(実物に近い書き方)">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; Where&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="reserved">bool</span>&gt; predicate)
    {
        <span class="comment">// イテレーターではなくなった(イテレーターなのは WhereInternal の方)ので、ちゃんと呼ばれた時点でチェックが走る</span>
        <span class="reserved">if</span> (source == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(source));
        <span class="reserved">if</span> (predicate == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(predicate));

        <span class="reserved">return</span> WhereInternal(source, predicate);
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; WhereInternal&lt;<span class="type">T</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="reserved">bool</span>&gt; predicate)
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> source)
            <span class="reserved">if</span> (predicate(x))
                <span class="reserved">yield</span> <span class="reserved">return</span> x;
    }
}
</code></pre>

こういう場面こそ、ローカル関数の出番です。
以下のように書き直すことができます。

<pre class="source" title="Whereをまねたもの(ローカル関数を使った実装)">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; Where&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="reserved">bool</span>&gt; predicate)
    {
        <span class="comment">// イテレーターではなくなった(イテレーターなのは WhereInternal の方)ので、ちゃんと呼ばれた時点でチェックが走る</span>
        <span class="reserved">if</span> (source == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(source));
        <span class="reserved">if</span> (predicate == <span class="reserved">null</span>) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(predicate));

        <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; f()
        {
            <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> source)
                <span class="reserved">if</span> (predicate(x))
                    <span class="reserved">yield</span> <span class="reserved">return</span> x;
        }

        <span class="reserved">return</span> f();
    }
}
</code></pre>

####<a id="sec-generated-title-6"></a> <a id="ToArray"></a>例2: イテレーターをToArrayしてから返す
[イテレーター](../data/sp2_iterator.md#iterator)を使って書きたいものの、
遅延実行(foreachで列挙されて初めて実行される)ではなく即座に実行するために、`ToArray`メソッド(`System.Enumerable`クラスの拡張メソッド)を掛けてから返したい場合があります。

この場合も、1つのメソッドからしか呼ばれないメソッドが作られがちです。
例えば以下のようなコードになります。

<pre class="source" title="ToArrayするためだけに作られるメソッド">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">U</span>[] SelectToArray&lt;<span class="type">T</span>, <span class="type">U</span>&gt;(<span class="reserved">this</span> <span class="type">T</span>[] array, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="type">U</span>&gt; selector)
    {
        <span class="reserved">return</span> Select(array, selector).ToArray();
    }

    <span class="comment">// SelectToArray からしか呼ばれない</span>
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type">U</span>&gt; Select&lt;<span class="type">T</span>, <span class="type">U</span>&gt;(<span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="type">U</span>&gt; selector)
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> source)
            <span class="reserved">yield</span> <span class="reserved">return</span> selector(x);
    }
}
</code></pre>

これも、以下のように書き直せます。

<pre class="source" title="ローカル関数を使って書き直し">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyEnumerable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">U</span>[] SelectToArray&lt;<span class="type">T</span>, <span class="type">U</span>&gt;(<span class="reserved">this</span> <span class="type">T</span>[] array, <span class="type">Func</span>&lt;<span class="type">T</span>, <span class="type">U</span>&gt; selector)
    {
        <span class="type">IEnumerable</span>&lt;<span class="type">U</span>&gt; inner()
        {
            <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
                <span class="reserved">yield</span> <span class="reserved">return</span> selector(x);
        }

        <span class="reserved">return</span> inner().ToArray();
    }
}
</code></pre>

####<a id="sec-generated-title-7"></a> <a id="async-task"></a>例3: 非同期メソッドのキャッシュ
最後の例は、非同期メソッドで作った`Task`のキャッシュです。

非同期メソッドを呼び出すと、呼び出すたびに`Task`クラス(`System.Threading.Tasks`名前空間)のインスタンスが作られます。
しかし、これを、1度だけ呼んで、2度目以降はキャッシュして持っている`Task`を返したいことがあります。

<pre class="source" title="Taskをキャッシュする例">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> MainAsync()
{
    <span class="comment">// 何度か呼ぶけども、キャッシュされているので通信は1回きり</span>
    <span class="type">Console</span>.WriteLine(<span class="reserved">await</span> LoadAsync());
    <span class="type">Console</span>.WriteLine(<span class="reserved">await</span> LoadAsync());
    <span class="type">Console</span>.WriteLine(<span class="reserved">await</span> LoadAsync());
}

<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; LoadAsync()
{
    _loadCache = _loadCache ?? LoadAsyncInternal();
    <span class="reserved">return</span> _loadCache;
}
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; _loadCache;

<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; LoadAsyncInternal()
{
    <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">HttpClient</span>();
    <span class="reserved">var</span> res = <span class="reserved">await</span> c.GetAsync(<span class="string">"http://ufcpp.net"</span>);
    <span class="reserved">var</span> content = <span class="reserved">await</span> res.Content.ReadAsStringAsync();

    <span class="reserved">return</span> <span class="type">Regex</span>.Match(content, @"\&lt;title\&gt;(.*?)\&lt;").Groups[1].Value;
}
</code></pre>

これも、以下のように書き直せます。

<pre class="source" title="ローカル関数を使って書き直し">
<code><span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; LoadAsync()
{
    <span class="reserved">async</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; inner()
    {
        <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">HttpClient</span>();
        <span class="reserved">var</span> res = <span class="reserved">await</span> c.GetAsync(<span class="string">"http://ufcpp.net"</span>);
        <span class="reserved">var</span> content = <span class="reserved">await</span> res.Content.ReadAsStringAsync();

        <span class="reserved">return</span> <span class="type">Regex</span>.Match(content, @"\&lt;title\&gt;(.*?)\&lt;").Groups[1].Value;
    }

    _loadCache = _loadCache ?? inner();
    <span class="reserved">return</span> _loadCache;
}
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">string</span>&gt; _loadCache;
</code></pre>

##<a id="sec-generated-title-8"></a> <a id="anonymous-function"></a>匿名関数 (ラムダ式)
<h5 class="version version2">Ver. 2.0</h5>
<h5 class="version version3">Ver. 3.0</h5>

C# 2.0では[匿名メソッド式](sp_delegate.md#anonymous-method)、C# 3.0では[ラムダ式](sp_delegate.md#lambda)という構文が入り、これらを合わせて<strong id="key-anonymous" class="keyword">匿名関数</strong>と呼びます。

(ラムダ式は匿名メソッド式のほぼ上位互換です。
C#開発者も、「ラムダ式が最初からあれば、匿名メソッド式の構文はC#には不要だった」と言っています。
そのため、匿名メソッド式はC# 2.0時代の互換性を保つためだけの機能だと考えて差し支えないです。
こういう背景から、匿名関数という名前が使われることはあまりなく、
<strong id="key-lambda" class="keyword">ラムダ式</strong>(lambda expression)という言葉の方がよく目にすることになると思います。
本節でも、以下の説明はラムダ式でのみ行います。)

ラムダ式は、以下の例のように、引数リストと関数本体を `=>`でつないで書きます。

<pre class="source" title="ラムダ式の例1">
<code>(<span class="reserved">int</span> x) =&gt;
{
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; x; i++)
        sum += i;
    <span class="reserved">return</span> sum;
}
</code></pre>

この例を見ての通り、関数名がありません。これが「匿名」と呼ばれる理由です。

`=>` は、矢印のように見えることからアロー演算子(arrow operator)と呼ばれたり、
その矢印を「行先」に見立ててgoes to演算子と呼ばれたりします。
(実際、`x => 2 * x`を x goes to 2x (xが2xに行く)と読むと、英語的に案外しっくり来るそうです。)

`=>` の後ろの関数本体の部分は、式が1つだけの場合、`{}`と`return`を省略して、以下のように書くことができます。

<pre class="source" title="ラムダ式の例2 (本体が式1つだけの場合)">
<code>(<span class="reserved">int</span> x) =&gt; x * x
</code></pre>

また、`=>`の前の引数リストでは、引数の型を推論できる場合には型を省略できます。
このとき、引数が1つだけであれば、`()`も省略できます。

<pre class="source" title="ラムダ式の例3 (引数の型の省略)">
<code>(x, y) =&gt; x * y
</code></pre>

<pre class="source" title="ラムダ式の例3 (引数の型の省略)">
<code>x =&gt; x * x
</code></pre>

例えば、以下のような使い方ができます。

<pre class="source" title="匿名関数の例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> input = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
        <span class="reserved">var</span> output = input
            .Where(<em>n =&gt; n &gt; 3</em>)
            .Select(<em>n =&gt; n * n</em>);

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> output)
        {
            <span class="type">Console</span>.WriteLine(x);
        }
    }
}
</code></pre>

強調表示している部分が匿名関数です。
匿名関数の引数(`n`)の型は、渡す先（`Where`や`Select`)から推論されます。

##<a id="sec-generated-title-9"></a> <a id="pros-cons"></a>ローカル関数と匿名関数のそれぞれの利点
前節の例のように、匿名関数は式(この例では`Where`メソッドや`Select`メソッドの引数)の中に書くことができます。
ここがローカル関数との最大の違いになります。
ローカル関数の場合は、関数(この場合`Main`メソッド)直下にしか書けません。

匿名関数はどこにでも書けるという利点がある一方で、以下のような制限があります。

- 再帰呼び出しが素直にはできない
- イテレーターにできない
- ジェネリックにできない
- 引数の既定値を持てない

<pre class="source" title="匿名関数の再帰呼び出しは面倒">
<code><span class="comment">// ローカル関数は素直に再帰を書ける</span>
<span class="reserved">int</span> f1(<span class="reserved">int</span> n) =&gt; n &gt;= 1 ? n * f1(n - 1) : 1;

<span class="comment">// 匿名関数はひと手間必要</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f2 = <span class="reserved">null</span>;
f2 = n =&gt; n &gt;= 1 ? n * f2(n - 1) : 1;
</code></pre>

<pre class="source" title="匿名関数はイテレーターにできない">
<code><span class="comment">// ローカル関数ならイテレーターにできる</span>
<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; g1(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
        <span class="reserved">yield</span> <span class="reserved">return</span> 2 * x;
}

<span class="comment">// 匿名関数ではコンパイル エラー</span>
<span class="type">Func</span>&lt;<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;, <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;&gt; g2 = items =&gt;
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
        <span class="reserved">yield</span> <span class="reserved">return</span> 2 * x;
}
</code></pre>

<pre class="source" title="匿名関数はジェネリックにできない">
<code><span class="comment">// ローカル関数ならジェネリックに使える</span>
<span class="reserved">bool</span> eq1&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">T</span> y) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IComparable</span>&lt;<span class="type">T</span>&gt; =&gt; x.CompareTo(y) == 0;
<span class="type">Console</span>.WriteLine(eq1(1, 2));
<span class="type">Console</span>.WriteLine(eq1(<span class="string">"aaa"</span>, <span class="string">"aaa"</span>));

<span class="comment">// 匿名関数はジェネリックにならない</span>
<span class="comment">// Func&lt;T, T, bool&gt; の時点でコンパイル エラー</span>
<span class="comment">// where 制約を付ける構文もない</span>
<span class="type">Func</span>&lt;T, T, <span class="reserved">bool</span>&gt; eq2 = (x, y) =&gt; x.CompareTo(y) == 0;
<span class="comment">// 当然、呼べない</span>
<span class="type">Console</span>.WriteLine(eq2(1, 2));
<span class="type">Console</span>.WriteLine(eq2(<span class="string">"aaa"</span>, <span class="string">"aaa"</span>));
</code></pre>

<pre class="source" title="匿名関数の引数には既定値を与えられない">
<code><comment></span><span class="comment">// ローカル関数の引数には既定値を与えられる</span>
<span class="reserved">int</span> f1(<span class="reserved">int</span> n = 0) =&gt; 2 * n;
<span class="type">Console</span>.WriteLine(f1());
<span class="type">Console</span>.WriteLine(f1(5));

<span class="comment">// 匿名関数は無理</span>
<span class="comment">// この時点でコンパイル エラー</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f2 = (<span class="reserved">int</span> n = 0) =&gt; 2 * n;
<span class="comment">// 当然、呼べない</span>
<span class="type">Console</span>.WriteLine(f2());
<span class="type">Console</span>.WriteLine(f2(5));
</code></pre>

すなわち、以下のことが言えます。

- ローカル関数は書ける場所が限られるものの、機能的には通常のメソッドと同程度に何でも書ける
- 逆に、匿名関数はどこにでも書ける代わりに、いくつか機能的に制限がある

また、詳しくは「[[雑記] 匿名関数のコンパイル結果](sp2_anonymousmethod.md#closure-local-function)」で説明しますが、
多少、実行性能にも差があります。
呼び出し方次第ではありますが、ローカル関数の方が高速になる場合があります。

###<a id="sec-generated-title-10"></a> <a id="background-local-function"></a>余談: 経緯
ちなみに、C# 7でローカル関数が導入されるに至った経緯としては、匿名関数の制限を緩和してほしいという要望から始まっています。
すなわち、前述の、「匿名関数はイテレーター化できない、再帰呼び出しが大変」という問題の解決策がローカル関数です。

書ける場所にも違いがあるので、この要望が完全に満たされたわけではありません。
しかし、「イテレーター化」あるいは「再帰呼び出し」をしたい場面を改めて考えてみたところ、
「別に式中に書きたいわけじゃない」、「ローカル関数で十分」、「ローカル関数の方が実行性能的にお得になる場面もある」となったみたいです。

##<a id="sec-generated-title-11"></a> <a id="capture-local"></a>ローカル変数の捕獲
ローカル関数でも匿名関数でも、周りの(定義している関数内の)ローカル変数や引数を取り込んで使うことができます。
例えば以下のようなコードが書けます。

<pre class="source" title="ローカル変数の取り込みの例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ユーザーからの入力をローカル変数に記録</span>
        <span class="reserved">var</span> m = <span class="reserved">int</span>.Parse(<span class="type">Console</span>.ReadLine());
        <span class="reserved">var</span> n = <span class="reserved">int</span>.Parse(<span class="type">Console</span>.ReadLine());

        <span class="reserved">var</span> input = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        <span class="comment">// ユーザーの入力 m よりも大きいか判定</span>
        <span class="reserved">bool</span> filter(<span class="reserved">int</span> x) =&gt; x &gt; <em>m</em>;

        <span class="reserved">var</span> output = input
            .Where(filter)
            .Select(x =&gt; <em>n</em> * x); <span class="comment">// ユーザーの入力 n を掛ける</span>

        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> output)
        {
            <span class="type">Console</span>.WriteLine(x);
        }
    }
}
</code></pre>

こういう処理を、ローカル変数の捕獲(capture)と言います(カタカナ言葉で「キャプチャする」ともよく言います)。
また、ローカル変数を捕獲しているローカル関数や匿名関数を<strong id="closure" class="keyword">クロージャ</strong>(closure: 囲い込み)と呼んだりします。

捕獲したローカル変数は書き換えることもできます。

<pre class="source" title="捕獲したローカル変数をクロージャ内で書き換える例">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = 1;

        <span class="comment">// ローカル関数内で変数xを書き換え</span>
        <span class="reserved">void</span> f(<span class="reserved">int</span> n) =&gt; x = n;

        <span class="type">Console</span>.WriteLine(x); <span class="comment">// 1</span>

        f(2);
        <span class="type">Console</span>.WriteLine(x); <span class="comment">// 2</span>
    }
}
</code></pre>

注意点として、詳しくは「[[雑記] 匿名関数のコンパイル結果](sp2_anonymousmethod.md#closure)」で説明しますが、
ローカル変数の取り込みには少々ペナルティがかかります。
実行性能への要求が極めて高い場合には、避けれるなら避けるべきです
(ペナルティは小さいので、ボトルネックになっていない場所でまで無理に頑張る必要はありません)。

##<a id="sec-generated-title-12"></a> <a id="avoid-capture"></a>ローカル変数捕獲の禁止
前節での説明の通り、外部の変数を捕獲してしまうと少々ペナルティが掛かります。
意図してやっているのならいいんですが、無自覚にやってしまうのは避けたいです。

そこで、C# 8.0 では静的ローカル関数、C# 9.0 では静的匿名関数という仕様が入りました。

###<a id="sec-generated-title-13"></a> <a id="static-local-function"></a>静的ローカル関数
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 から、外部の変数を捕獲しないことを明示するため、
ローカル関数に `static` 修飾を付けれるようになりました。
この機能を<strong id="key-static-local-function" class="keyword">静的ローカル関数</strong>(static local function)と呼びます。

<pre class="source" title="静的ローカル関数の例">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">a</span>)
{
    <span class="comment">// 外部の変数(引数)を捕獲(クロージャ化)。</span>
    <span class="reserved">int</span> <span class="method">f</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">a</span> * <span class="variable">x</span>;
 
    <span class="comment">// static を付けて、クロージャ化を禁止。</span>
    <span class="comment">// a を使っているところでコンパイル エラーになる。</span>
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">g</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="error"><span class="variable">a</span></span> * <span class="variable">x</span>;
}
</code></pre>

「[匿名関数のコンパイル結果](sp2_anonymousmethod.md#compile_anonymous)」で説明していますが、
こういう何も捕獲していないローカル関数は、静的メソッドに展開されます。
なので、`static` 修飾子を使って、静的ローカル関数と呼びます。

ちなみに、「静的」の名前が示す通り、インスタンス メンバーの参照もできません。

<pre class="source" title="静的ローカル関数はインスタンス メンバーに触れない">
<code><span class="reserved">class</span> <span class="type">LocalFunction</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> StaticProperty { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> InstanceProperty { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// これは OK。</span>
        <span class="reserved"><em>static</em></span> <span class="reserved">int</span> <span class="method">f1</span>() =&gt; StaticProperty;
 
        <span class="comment">// これはコンパイル エラー。</span>
        <span class="reserved"><em>static</em></span> <span class="reserved">int</span> <span class="method">f2</span>() =&gt; <span class="error">InstanceProperty</span>;
    }
}
</code></pre>

ちなみに、定数や `nameof` であれば外側のスコープにあるものに触ることができます。
例えば以下のコードはコンパイルできます。

<pre class="source" title="定数なのでセーフ">
<code><span class="reserved">using</span> System;
 
<span class="reserved">const</span> <span class="reserved">string</span> s = <span class="string">&quot;bc&quot;</span>;
<span class="reserved">int</span> <span class="variable">a</span> = 0;
 
<span class="comment">// a を使っているように見えて、nameof(a) は単に &quot;a&quot; に展開されるのでセーフ。</span>
<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">m</span>() =&gt; <span class="reserved">nameof</span>(<span class="variable">a</span>) + s;
 
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">m</span>());
</code></pre>

###<a id="sec-generated-title-14"></a> <a id="static-anonymous-function"></a>静的匿名関数
<h5 class="version version9">Ver. 9.0</h5>

同様に、C# 9.0 では匿名関数に対しても `static` 修飾子を付けれるようになりました。
意味的には[静的ローカル関数](#static-local-function)と全く同じで、「外部の変数を捕獲しない」という宣言になります。
ラムダ式、匿名メソッド式ともに、式の前に `static` を付けます。

<pre class="source" title="静的匿名関数">
<code><span class="reserved">using</span> System;
 
<span class="reserved">int</span> <span class="variable">a</span> = 0;
 
<span class="comment">// 以下の2行は自身の引数しか使っていないので static にしても怒られない。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ok1</span> = <span class="reserved"><em>static</em></span> <span class="variable">x</span> =&gt; <span class="variable">x</span> * <span class="variable">x</span>;
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ok2</span> = <span class="reserved"><em>static</em></span> <span class="reserved">delegate</span> (<span class="reserved">int</span> <span class="variable">x</span>) { <span class="control">return</span> <span class="variable">x</span> * <span class="variable">x</span>; };
 
<span class="comment">// 以下の2行は外側のローカル変数 a を使ってしまったのでコンパイル エラー。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ng1</span> = <span class="reserved"><em>static</em></span> <span class="variable">x</span> =&gt; <span class="variable"><span class="error">a</span></span> * <span class="variable">x</span>;
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">ng2</span> = <span class="reserved"><em>static</em></span> <span class="reserved">delegate</span> (<span class="reserved">int</span> <span class="variable">x</span>) { <span class="control">return</span> <span class="variable"><span class="error">a</span></span> * <span class="variable">x</span>; };
</code></pre>

静的ローカル関数がある時点で匿名関数でも同様のことができてしかるべきもので、
ただちょっと構文解析が大変なので後回しになっていたものです。
順当に「1バージョン遅れで実装」となりました。

###<a id="sec-generated-title-15"></a> <a id="not-pure"></a>注意: 純粋関数(副作用なしのメソッド)ではない
静的ローカル関数にしても静的匿名関数にしても、ローカル変数の捕獲(によるパフォーマンスのペナルティ)は避けることができますが、静的フィールドの読み書きは普通にできます。
例えば以下のコードは有効な C# 8.0 コードになります。

<pre class="source" title="副作用がある静的ローカル関数の例">
<code><span class="reserved">class</span> <span class="type">StaticLocalFunction</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> _count;
 
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// ローカル関数内から外の変数を読み書きしてる。</span>
        <span class="comment">// _count が static なのでコンパイル可能。</span>
        <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">local</span>() =&gt; ++_count;
 
        System.<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">local</span>());
    }
}
</code></pre>

`static` を付けてもいわゆる純粋関数(pure function、同じ引数で呼べば必ず同じ戻り値が得られる)にはならないので注意してください。

##<a id="sec-generated-title-16"></a> <a id="shadowing"></a>変数のシャドーイング
<h5 class="version version8">Ver. 8.0</h5>

前節の静的ローカル関数に伴って新たに認められた機能に、変数の<strong id="key-shadowing" class="keyword">シャドーイング</strong>(shadowing)というものがあります。
ローカル関数内で、外側にすでに存在している変数や引数と同じ名前で、
新たに変数・引数を定義できる機能です。
外側のものを「影で覆い隠す」という意味で shadowing と呼びます。

<pre class="source" title="シャドーイングの例">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">a</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;
 
    <span class="reserved">int</span> <span class="method">f</span>(<span class="reserved">int</span> <span class="variable">a</span>) <span class="comment">// この a は M(int a) の a とは別物</span>
    {
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">a</span>; <span class="comment">// この x も外側の x とは別物</span>
        <span class="control">return</span> <span class="variable">x</span>;
    }
}
</code></pre>

C# 8.0 以降であれば、普通のローカル関数でも使えます。
ただ、外側の変数を捕獲したものなのか、ローカル関数側でシャドーイングしたものなのかの区別がわかりにくくなるという問題があるので、静的ローカル関数と同時(C# 8.0)に認められました。
静的ローカル関数でだけ認めるのも気持ち悪く、普通のローカル関数でも認めるようにしたそうです。

##<a id="sec-generated-title-17"></a> <a id="lambda-csharp10"></a>ラムダ式の戻り値の明示と属性
<h5 class="version version10">Ver. 10</h5>

C# 10.0 で、ラムダ式の戻り値を明示できるようになりました。
また、属性も付けられるようになりました。
例えば以下のようなコードが書けます。

<pre class="source" title="ラムダ式の戻り値の明示と属性の追加">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> =
    <em>[<span class="type">A</span>]</em>
    <em><span class="reserved">int</span></em> (<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>)
    =&gt; <span class="variable">x</span> + <span class="variable">y</span>;

<span class="reserved">class</span> <span class="type">AAttribute</span> : <span class="type">Attribute</span> { }
</code></pre>

これだけ見るとあまり使い道がなさそうな機能ですが、
同時に入る[デリゲートの自然な型決定](sp_delegate.md#natural-type)と併せるとそれなりに意味を持ちます。
「[自然な型](sp_delegate.md#natural-type)」の方でも書いていますが、
 .NET 6.0 (C# 10.0 と同世代)の Web アプリ テンプレートで作られるコードは以下のようになっています。

<pre class="source" title="Web アプリの .NET 6 新テンプレート">
<code><span class="reserved">var</span> <span class="variable">builder</span> = <span class="type">WebApplication</span>.<span class="method">CreateBuilder</span>(<span class="variable">args</span>);
<span class="reserved">var</span> <span class="variable">app</span> = <span class="variable">builder</span>.<span class="method">Build</span>();

<span class="variable">app</span>.<span class="method">MapGet</span>(<span class="string">&quot;/&quot;</span>, () =&gt; <span class="string">&quot;Hello World!&quot;</span>);

<span class="variable">app</span>.<span class="method">Run</span>();
</code></pre>

`MapGet` にラムダ式を渡すことで Web API を簡潔に書けるようになりました。
この書き方がそのまま大規模開発に向いているかというと微妙ですが、
少なくとも入門用のコードとしてはこれくらいの簡潔さが求められています。

この例では、HTTP GET で `/` (Web サイトのルート)にアクセスすると、`Hellow World!` という文字列を返します。
ここで、`/` アクセス時に色々と凝ったことをしようと思うと、属性や戻り値の型を指定したくなります。

###<a id="sec-generated-title-18"></a> <a id="lambda-explicit-return"></a>戻り値の型の明示
ラムダ式に戻り値の型を明示できるようになりました。
戻り値の型は、引数の `()` の前に書きます。
例えば以下のような書き方ができます。

<pre class="source" title="ラムダ式の戻り値の型を明示する例">
<code><span class="comment">// 新文法。</span>
<span class="comment">// ラムダ式に戻り値の型を明示。</span>
<span class="comment">// (引数も明示。)</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f1</span> = <em><span class="reserved">int</span></em> (<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>;

<span class="comment">// 元々の文法。</span>
<span class="comment">// 引数の型の方を明示。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f2</span> = (<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>;

<span class="comment">// 新文法。</span>
<span class="comment">// 戻り値の型だけ明示。 () が必要。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f3</span> = <em><span class="reserved">int</span></em> (<span class="variable">x</span>) =&gt; <span class="variable">x</span>;

<span class="comment">// これはエラーになる。</span>
<span class="comment">// int が引数に掛かっているのか戻り値に掛かっているのか不明瞭。</span>
<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f4</span> = <span class="error"><span class="reserved">int</span> x</span> =&gt; x;
</code></pre>

たとえ[自然な型決定](sp_delegate.md#natural-type)と組み合わせたとしても、
たいていの場合は引数だけ型を明示すれば戻り値の型は決定できたりするので、
必要になる場面はそう多くないかもしれません。
以下のようなコード(右辺のラムダ式の部分は C# 9.0 でも有効)でも問題なく自然な型決定ができます。

<pre class="source" title="引数の型だけでも自然な型決定がたいてい可能">
<code><span class="comment">// 引数の int から、戻り値の型が int に決定する。</span>
<span class="comment">// その後、ラムダ式の型は Func&lt;int, int&gt; として決定できる。</span>
<span class="reserved">var</span> <span class="variable">f</span> = (<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>;
</code></pre>

(おそらく、後述する属性のついでで実装された(ついででやれたから手間が掛かっていない)機能だと思われます。)

戻り値の型の明示が有効なのは、
例えば、
ラムダ式の中身自体がターゲット型推論に依存している場合などです。
サンプル コードとして[条件演算子のターゲット型推論](../cheatsheet/ap_ver9.md#target-typed-conditional)を使いますが、以下のような式は後者のみ有効になります。

<pre class="source" title="ラムダ式の中身にターゲット型推論を含む場合の例">
<code><span class="comment">// 条件演算子だけでは int と null の共通型が決定できなくて、戻り値の型が決まらない。</span>
<span class="comment">// (条件演算子の後方互換性のために掛かってる制限。)</span>
<span class="reserved">var</span> <span class="variable">f1</span> = <span class="error">(<span class="reserved">bool</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; <span class="variable">x</span> ? <span class="variable">y</span> : <span class="reserved">null</span></span>;

<span class="comment">// 一方で、これなら、戻り値の型からのターゲット型推論で条件演算子を書ける。</span>
<span class="comment">// f2 の自然な型決定もできるようになる (Func&lt;bool, int, int?&gt; になる)。</span>
<span class="reserved">var</span> <span class="variable">f2</span> = <span class="reserved">int</span>? (<span class="reserved">bool</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; <span class="variable">x</span> ? <span class="variable">y</span> : <span class="reserved">null</span>;
</code></pre>

ちなみに、[静的匿名関数](#static-local-function)の `static` と併用する場合、戻り値の型を書く場所は `static` の後ろです。
(通常のメソッドと同じ。)

<pre class="source" title="static の後ろに戻り値の型">
<code><span class="comment">// 戻り値の型を各場所は static の後ろ。</span>
<span class="reserved">var</span> <span class="variable">f</span> = <span class="reserved">static</span> <span class="reserved">int</span> (<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>;
</code></pre>

また、明示した戻り値の型からラムダ式の引数の型を推論することはできません。

<pre class="source" title="戻り値の型から引数の型の推論はできない">
<code><span class="comment">// 戻り値の型から引数の型の推論はできない。</span>
<span class="comment">// 結果的に、Func&lt;T, int&gt; への代入はできても、自然な型決定(var などへの代入)はできない。</span>
<span class="reserved">var</span> <span class="variable">f6</span> = <span class="error"><span class="reserved">int</span> (<span class="variable">x</span>) =&gt; <span class="variable">x</span></span>;
</code></pre>

###<a id="sec-generated-title-19"></a> <a id="lambda-attribute"></a>属性
同じくラムダ式に属性を付けれるようになりました。

<pre class="source" title="ラムダ式に対する属性付与">
<code><span class="reserved">var</span> <span class="variable">f</span> =
    <em>[<span class="type">A</span>]</em>
    <em>[<span class="reserved">return</span>: <span class="type">B</span>]</em>
    <span class="reserved">static</span> <span class="reserved">int</span> (<em>[<span class="type">C</span>]</em> <span class="reserved">int</span> <span class="variable">x</span>)
    =&gt; <span class="variable">x</span>;

[<span class="type">AttributeUsage</span>(<span class="type">AttributeTargets</span>.Method)]
<span class="reserved">class</span> <span class="type">AAttribute</span> : <span class="type">Attribute</span> { }

[<span class="type">AttributeUsage</span>(<span class="type">AttributeTargets</span>.ReturnValue)]
<span class="reserved">class</span> <span class="type">BAttribute</span> : <span class="type">Attribute</span> { }

[<span class="type">AttributeUsage</span>(<span class="type">AttributeTargets</span>.Parameter)]
<span class="reserved">class</span> <span class="type">CAttribute</span> : <span class="type">Attribute</span> { }
</code></pre>

属性を書く位置は通常のメソッドと同じです。
ラムダ式(メソッド)自体、引数、戻り値が対象になります。
また、([静的匿名関数](#static-local-function)と併用するなら)ラムダ式全体(メソッド全体)や戻り値に属性を付けたい場合は `static` よりも前に書きます。

これも、 .NET 6 の新しい Web テンプレートで使います。
`MapGet` などのメソッドでは、引数などに属性を付けて Web API の挙動をカスタマイズできます。
例えば以下のような書き方ができます。

<pre class="source" title="新 Web テンプレートに対して属性で挙動を制御する例">
<code><span class="reserved">using</span> Microsoft.AspNetCore.Mvc;

<span class="reserved">var</span> <span class="variable">builder</span> = <span class="type">WebApplication</span>.<span class="method">CreateBuilder</span>(<span class="variable">args</span>);

<span class="comment">// テンプレに1行を追加。DI 用。</span>
<span class="variable">builder</span>.Services.<span class="method">AddSingleton</span>&lt;<span class="type">Counter</span>&gt;();

<span class="reserved">var</span> <span class="variable">app</span> = <span class="variable">builder</span>.<span class="method">Build</span>();

<span class="comment">// テンプレを1行書き換え。引数を DI で受け取ったり、クエリ文字列から受け取ったり。</span>
<span class="comment">// counter: ページをリロードするたびに +1。</span>
<span class="comment">// value: クエリ文字列で数値を指定。</span>
<span class="comment">// その2つの値から何らかの計算して返す。</span>
<span class="variable">app</span>.<span class="method">MapGet</span>(<span class="string">&quot;/&quot;</span>, ([<span class="type">FromServices</span>] <span class="type">Counter</span> <span class="variable">counter</span>, [<span class="type">FromQuery</span>] <span class="reserved">int</span>? <span class="variable">value</span>) =&gt; <span class="variable">counter</span>.Count * (<span class="variable">value</span> ?? 1));

<span class="variable">app</span>.<span class="method">Run</span>();

<span class="comment">// テンプレに1クラス追加。上記 DI で渡すデモ用の型。</span>
<span class="reserved">class</span> <span class="type">Counter</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _count;
    <span class="reserved">public</span> <span class="reserved">int</span> Count { <span class="reserved">get</span> =&gt; _count++; }
}
</code></pre>

##<a id="sec-generated-title-20"></a> <a id="lambda-default">ラムダ式のオプション引数(既定値)と params 引数</a>
<h5 class="version version12">Ver. 12</h5>

C# 12 でラムダ式の引数に[オプション引数](../structured/sp4_optional.md#optional)にできる(既定値を与えられる)ようになりました。
また、[params 引数](../structured/sp_params.md)も使えるようになりました。

<pre class="source" title="ラムダ式の引数の既定値と params 引数">
<span class="comment">// オプション引数(既定値値指定)。</span>
<span class="reserved">var</span> <span class="variable">f1</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="comment">// params 引数。</span>
<span class="reserved">var</span> <span class="variable">f2</span> <span class="operator">=</span> (<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="comment">// 混在も OK。</span>
<span class="reserved">var</span> <span class="variable">f3</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>, <span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
</pre>

前節の[戻り値の型の指定や属性付与](#lambda-csharp10)と同様、
[デリゲートの自然な型](sp_delegate.md#natural-type)との併用で使ったり、
リフレクションを使って情報を取得するために使います。

自然な型決定(要するに `var` への代入)した場合、
匿名デリゲート型が生成されて、既定値や params の情報が残ります。
例えば `(int x = 1) => x` であれば `delegate int F(int x = 1)` 相当の匿名デリゲート型が生成されます。

<pre class="source" title="既定値、params の情報が残る例">
<span class="comment">// 引数にデフォルト値指定。</span>
<span class="comment">// delegate int &lt;anonymous&gt;(int x = 1); みたいな匿名デリゲート型になる。</span>
<span class="reserved">var</span> <span class="variable">f1</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="variable">f1</span>(); <span class="comment">// f1(1) と同じ。</span>

<span class="comment">// params 引数。</span>
<span class="comment">// delegate int &lt;anonymous&gt;(params int[] x); みたいな匿名デリゲート型になる。</span>
<span class="reserved">var</span> <span class="variable">f2</span> <span class="operator">=</span> (<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="variable">f2</span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>); <span class="comment">// f2(new int[] { 1, 2, 3 }) と同じ。</span>
</pre>

一方で、既定値違い、params 違いのデリゲート型への代入もできてしまいます。
この場合、既定値などの情報は消えます。
(ちょっと罠なので、一応、警告はしてくれます。)

<pre class="source" title="既定値違い、params 違いのデリゲート型への代入">
<span class="comment">// 既定値の情報がないデリゲート型に代入。</span>
<span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">f1</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="warning" title="CS9099"><span class="variable local">x</span></span> <span class="operator">=</span> <span class="number">1</span>) <span class="operator">=&gt;</span> { };

<span class="variable"><span class="error" title="CS7036">f1</span></span>(); <span class="comment">// エラー。 f1(1) と書かないとダメ。</span>

<span class="comment">// params の情報がないデリゲート型に代入。</span>
<span class="type">Action</span>&lt;<span class="reserved">int</span>[]&gt; <span class="variable">f2</span> <span class="operator">=</span> (<span class="reserved">params</span> <span class="reserved">int</span>[] <span class="variable local"><span class="warning" title="CS9100">x</span></span>) <span class="operator">=&gt;</span> { };

<span class="variable"><span class="error" title="CS1593">f2</span></span>(<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>); <span class="comment">// エラー。 f2(new int[] { 1, 2, 3 }) と書かないとダメ。</span>
</pre>

この点についてもう少し踏み込んで注意すると、
ラムダ式の側とデリゲート型の側で異なる既定値を与えたとき、
リフレクションで値を取るときに変なことが起きたりもします。
`Delegate.Method` で取る情報(ラムダ式側)と、`Type.GetMethod` で取る情報(デリゲート型型)が食い違います。

<pre class="source" title="異なる既定値が取れちゃう例">
<span class="reserved">using</span> System<span class="operator">.</span>Reflection;

<span class="comment">// ラムダ式としては既定値 2。</span>
<span class="comment">// ちゃんと警告にはなるものの、無視してしまうと…</span>
<span class="type">A</span> <span class="variable">a</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local"><span class="warning" title="CS9099">x</span></span> <span class="operator">=</span> <span class="number">2</span>) <span class="operator">=&gt;</span> { };

<span class="type">MethodInfo</span> <span class="variable">m1</span> <span class="operator">=</span> <span class="variable">a</span><span class="operator">.</span><span class="property">Method</span>; <span class="comment">// ラムダ式側の情報が取れる。</span>
<span class="type">MethodInfo</span> <span class="variable">m2</span> <span class="operator">=</span> <span class="variable">a</span><span class="operator">.</span><span class="method">GetType</span>()<span class="operator">.</span><span class="method">GetMethod</span>(<span class="string">&quot;Invoke&quot;</span>)<span class="operator">!</span>; <span class="comment">// デリゲート型側の情報が取れる。</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">m1</span><span class="operator">.</span><span class="method">GetParameters</span>()[<span class="number">0</span>]<span class="operator">.</span><span class="property">DefaultValue</span>); <span class="comment">// 2</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">m2</span><span class="operator">.</span><span class="method">GetParameters</span>()[<span class="number">0</span>]<span class="operator">.</span><span class="property">DefaultValue</span>); <span class="comment">// 1</span>

<span class="comment">// デリゲート型としては既定値 1。</span>
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">1</span>);
</pre>

##<a id="sec-generated-title-21"></a> <a id="simple-param-with-modifier">修飾子付きの引数の型名省略</a>
<h5 class="version version14">Ver. 14.0</h5>

ラムダ式には導入以来ずっと、「型を推論できる限り、引数の型は省略できる」という仕様があります。
ところが、`ref` や `out` などの修飾子が必須の引数の場合、C# 13 までは型名省略できませんでした。
これが C# 14 で改善されました。

例えば、`int` などをはじめ多くの型が以下のような `TryParse` メソッドを持っています。


<pre class="source" title="int などが持っている TryParse メソッド">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type struct">Int32</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="method"><span class="static">TryParse</span></span>([<span class="type">NotNullWhen</span>(<span class="reserved">true</span>)] <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">s</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">result</span>)
    {
        <span class="comment">// ...</span>
    }
}
</pre>

以下のように、この類の処理用のデリゲート型があったとして、

<pre class="source" title="int.TryParse などを受け取るためのデリゲート型">
<span class="reserved">delegate</span> <span class="reserved">bool</span> <span class="type">TryParse</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">string</span> <span class="variable local">text</span>, <span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">result</span>);
</pre>

C# 13 までは以下のように書くことができませんでした。

<pre class="source" title="C# 13 までは (out x) みたいな型名省略ができない">
<span class="comment">// C# 13 までは書けなかった。</span>
<span class="type">TryParse</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">m</span> <span class="operator">=</span> (<span class="variable local">text</span>, <span class="error" title="CS9260"><span class="reserved">out</span> <span class="variable local">result</span></span>) <span class="operator">=&gt;</span> { <span class="variable local">result</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">return</span> <span class="reserved">true</span>; };

<span class="comment">// out や ref がないならこう書けるのに…</span>
<span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> <span class="operator">=</span> (<span class="variable local">text</span>, <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;

<span class="comment">// out が付いた瞬間、型名が必須だった(これなら C# 13 でもコンパイル可能)。</span>
<span class="type">TryParse</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">m13</span> <span class="operator">=</span> (<span class="reserved">string</span> <span class="variable local">text</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">result</span>) <span class="operator">=&gt;</span> { <span class="variable local">result</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">return</span> <span class="reserved">true</span>; };

<span class="reserved">delegate</span> <span class="reserved">bool</span> <span class="type">TryParse</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">string</span> <span class="variable local">text</span>, <span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">result</span>);
</pre>

これが C# 14 では認められます。

<pre class="source" title="C# 14 で (out x) の類が可能に">
<span class="comment">// C# 14 で可能に。</span>
<span class="type">TryParse</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">m</span> <span class="operator">=</span> (<span class="variable local">text</span>, <em><span class="reserved">out</span> <span class="variable local">result</span></em>) <span class="operator">=&gt;</span> { <span class="variable local">result</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">return</span> <span class="reserved">true</span>; };
</pre>

対象となる修飾子は `out`、`ref`、`in`、`ref readonly`、`scoped` などです。

<pre class="source" title="">
<span class="type">M</span> <span class="variable">m</span> <span class="operator">=</span> (<span class="reserved">in</span> <span class="variable local">a</span>, <span class="reserved">ref</span> <span class="variable local">b</span>, <span class="reserved">out</span> <span class="variable local">c</span>, <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="variable local">d</span>, <span class="reserved">scoped</span> <span class="variable local">e</span>) <span class="operator">=&gt;</span> <span class="variable local">c</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// C# 13 以前だと以下のように書く必要あり。</span>
<span class="type">M</span> <span class="variable">m13</span> <span class="operator">=</span> (<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">b</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">c</span>, <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="variable local">d</span>, <span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">e</span>) <span class="operator">=&gt;</span> <span class="variable local">c</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">M</span>(<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">a</span>, <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">b</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable local">c</span>, <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="variable local">d</span>, <span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">e</span>);
</pre>

ちなみに、これが認められているのは引数リストを `()` でくくっている場合だけです。
ラムダ式は、引数が1つだけの時は `x => { }` というように引数リストの `()` も省略できるわけですが、
この場合は `ref x => { }` みたいな書き方はできません(というか元々、`int x => { }` みたいな型名指定も許されていません)。

<pre class="source" title="修飾子をつけたい場合、() は必須">
<span class="comment">// 修飾子をつけたい場合、() は必須。</span>
<span class="type">In</span> <span class="variable">m1</span> <span class="operator">=</span> (<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">a</span>) <span class="operator">=&gt;</span> { };
<span class="type">In</span> <span class="variable">m2</span> <span class="operator">=</span> (<span class="reserved">in</span> <span class="variable local">a</span>) <span class="operator">=&gt;</span> { };

<span class="comment">// () 省略不可でエラーに。</span>
<span class="type">In</span> <span class="variable">m3</span> <span class="operator">=</span> <span class="error" title="CS1003"><span class="error" title="CS1525"><span class="reserved">in</span></span></span> <span class="reserved">int</span> a <span class="operator">=&gt;</span> { <span class="error" title="CS1022"><span class="error" title="CS1002">}</span></span>;
<span class="type">In</span> <span class="variable">m4</span> <span class="operator">=</span> <span class="reserved"><span class="error" title="CS1525"><span class="error" title="CS1003">in</span></span></span> a <span class="operator">=&gt;</span> { <span class="error" title="CS1022"><span class="error" title="CS1002">}</span></span>;

<span class="comment">// ちなみに、in を抜こうとすると型が合わなくてエラーになる。</span>
<span class="type">In</span> <span class="variable">m5</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="error" title="CS1676"><span class="variable local">a</span></span>) <span class="operator">=&gt;</span> { };
<span class="type">In</span> <span class="variable">m6</span> <span class="operator">=</span> (<span class="variable local"><span class="error" title="CS1676">a</span></span>) <span class="operator">=&gt;</span> { };
<span class="type">In</span> <span class="variable">m7</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS1676">a</span></span> <span class="operator">=&gt;</span> { };

<span class="comment">// 参考: 修飾子がない場合:</span>
<span class="type">Value</span> <span class="variable">v1</span> <span class="operator">=</span> <span class="variable local">a</span> <span class="operator">=&gt;</span> { };
<span class="type">Value</span> <span class="variable">v2</span> <span class="operator">=</span> (<span class="variable local">a</span>) <span class="operator">=&gt;</span> { };
<span class="type">Value</span> <span class="variable">v3</span> <span class="operator">=</span> (<span class="reserved">int</span> <span class="variable local">a</span>) <span class="operator">=&gt;</span> { };
<span class="comment">// Value v4 = int a =&gt; { }; はこっちでもダメ。コンパイル エラーに。</span>

<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">Value</span>(<span class="reserved">int</span> <span class="variable local">a</span>);
<span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">In</span>(<span class="reserved">in</span> <span class="reserved">int</span> <span class="variable local">a</span>);
</pre>
