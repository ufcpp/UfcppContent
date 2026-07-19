---
title: "リソースの破棄"
source_url: "https://ufcpp.net/study/csharp/resource/oo_dispose/"
content_type: "Article"
published_at: "2002-11-02T00:00:00"
updated_at: "2007-06-30T00:00:00"
tags: []
umbraco_id: 1295
parent_id: 1286
sort_order: 13
aliases:
  - "/csharp/oo_dispose"
  - "/csharp/oo_dispose.html"
  - "/csharp/resource/oo_dispose/"
  - "/study/csharp/oo_dispose"
  - "/study/csharp/oo_dispose.html"
---

# リソースの破棄

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
ファイルや周辺機器などのリソース(OSが管理している資源)を使用する場合、
まずリソースを使用する権利を取得し、
リソースに対する操作(ファイルの読み書きなど)を行った後、
リソース使用権を破棄する必要があります。

メモリは .NET Framework の「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」機能が自動的に管理していて、
プログラマが明示的に破棄してやる必要はないのですが、
ファイルなどは「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」の管理対象外で、
明示的な破棄が必要です。

リソースの破棄を怠ると操作が正しく完了しなかったり、
他のプログラムからそのリソースを使用できなくなったりします。
（例えば、ファイルにロックが掛かったままになって、ファイルの読み書きがしばらくできなくなったり。）
そのため、リソースの破棄は確実に行う必要があるのですが、
これは意外に面倒な作業だったりします。


##### <a id="sec-generated-title-2"></a>ポイント
* .NET Framework でメモリー管理は自動化されたけど、 管理外のリソース（たとえば、ファイルIO）もある。

* 管理外のリソースは明示的に破棄が必要。

* 例外が発生した場合でも正しくリソース破棄ができるように、try-catch-finally や using を使いましょう。

##<a id="sec-generated-title-3"></a> <a id="ex"></a>リソース破棄の例
例えば、ファイルの読み書きを行う場合、
まずファイルを開いて、読み書きを行った後、ファイルを閉じる必要があります。
以下に簡単な例を示します。

<pre class="source" title="リソースの破棄の例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> <span class="type">DisposeTest</span>
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="type">FileStream</span> reader = <span class="reserved">new</span> <span class="type">FileStream</span>(args[<span class="literal">0</span>], <span class="type">FileMode</span>.Open);

        <span class="comment">// 先頭のNバイトを読み出して画面に表示</span>
        <span class="reserved">const int</span> N = <span class="literal">32</span>;
        <span class="reserved">byte</span>[] buf = <span class="reserved">new byte</span>[N];
        reader.Read(buf, <span class="literal">0</span>, N);
        <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; N; ++i)
        {
            <span class="type">Console</span>.Write(<span class="literal">"{0,4}"</span>, (<span class="reserved">int</span>)buf[i]);
            <span class="reserved">if</span> (i % <span class="literal">8</span> == <span class="literal">7</span>) <span class="type">Console</span>.Write(<span class="literal">'\n'</span>);
        }

        reader.Close(); <span class="comment">// ファイルを閉じる(リソースの破棄)</span>
    }
}
</code></pre>


この例のようなリソース破棄の仕方には実は問題があります。
この例のコードでは<em>例外が発生したときに <code>Close</code> メソッドが呼ばれない</em>ため、
リソースの開放が出来なくなります。
例外が発生した場合にも <code>Close</code> メソッドが呼ばれるようにするためには、
以下のように <em>try-catch-finally ステートメントを用います</em>。

<pre class="source" title="finally を用いたリソースの破棄" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> <span class="type">DisposeTest</span>
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="type">FileStream</span> reader = <span class="reserved">new</span> <span class="type">FileStream</span>(args[<span class="literal">0</span>], <span class="type">FileMode</span>.Open);

        <span class="reserved">try</span>
        {
            <span class="comment">// 先頭のNバイトを読み出して画面に表示</span>
            <span class="reserved">const int</span> N = <span class="literal">32</span>;
            <span class="reserved">byte</span>[] buf = <span class="reserved">new byte</span>[N];
            reader.Read(buf, <span class="literal">0</span>, N);
            <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; N; ++i)
            {
                <span class="type">Console</span>.Write(<span class="literal">"{0,4}"</span>, (<span class="reserved">int</span>)buf[i]);
                <span class="reserved">if</span> (i % <span class="literal">8</span> == <span class="literal">7</span>) <span class="type">Console</span>.Write(<span class="literal">'\n'</span>);
            }
        }
        <span class="reserved">catch</span> (<span class="type">Exception</span>)
        {
            <span class="comment">// 例外処理を行う</span>
        }
        <span class="reserved">finally</span>
        {
            <span class="comment">// 例外が発生しようがしまいが finally ブロックは必ず実行される。
            // リソースの破棄は finally ブロックで行う。</span>
            <span class="reserved">if</span> (reader != <span class="reserved">null</span>)
                reader.Close();
        }
    }
}
</code></pre>



##<a id="sec-generated-title-4"></a> <a id="using"></a>using ステートメント
リソースの破棄の手順をまとめると以下のようになります。
(ただし、<code>Resource</code> はリソース管理用クラスで、
<code>Dispose</code> メソッドによりリソースの破棄を行うものとする。)

<pre class="source" title="リソース破棄の手順" lang="">
<code><span class="type">Resource</span> r = <span class="reserved">new</span> <span class="type">Resource</span>();
<span class="reserved">try</span>
{
  <span class="input">リソースに対する操作</span>
}
<span class="reserved">finally</span>
{
  <span class="reserved">if</span>(r != <span class="reserved">null</span>)
    r.Dispose();
}
</code></pre>


リソースの破棄は必ずこの手順で行います
（「Dispose パターン」という呼び名もついてる定型パターン）。
しかし、毎回同じ手順を繰り返すのは面倒です。
そこで、C#ではこの手順を自動的に行ってくれる構文が用意されています。
この構文は <strong id="using" class="keyword">using ステートメント</strong>と呼ばれ、以下のようにして用います。

<pre class="source" title="using ステートメント" lang="">
<code><span class="reserved">using</span>(<span class="type">Resource</span> r = <span class="reserved">new</span> <span class="type">Resource</span>())
{
  <span class="input">リソースに対する操作</span>
}
</code></pre>


using ステートメントを用いると、
コンパイラが自動的に上述のリソース破棄用のコードに展開してくれます。
ただし、using ステートメントで使うリソース管理用クラスは <em>
        <code>System.IDisposable</code> インターフェース
      </em>を実装している必要があります。
(<code>FileStream</code> などのクラスライブラリ中のクラスは <code>System.IDisposable</code> インターフェースを実装しています。)

using ステートメントを用いて上述の例を書き直したものを以下に示します。

<pre class="source" title="using を用いたリソースの破棄" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> <span class="type">DisposeTest</span>
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">using</span> (<span class="type">FileStream</span> reader = <span class="reserved">new</span> <span class="type">FileStream</span>(args[<span class="literal">0</span>], <span class="type">FileMode</span>.Open))
        {
            <span class="comment">// 先頭のNバイトを読み出して画面に表示</span>

            <span class="reserved">const int</span> N = <span class="literal">32</span>;
            <span class="reserved">byte</span>[] buf = <span class="reserved">new byte</span>[N];
            reader.Read(buf, <span class="literal">0</span>, N);
            <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; N; ++i)
            {
                <span class="type">Console</span>.Write(<span class="literal">"{0,4}"</span>, (<span class="reserved">int</span>)buf[i]);
                <span class="reserved">if</span> (i % <span class="literal">8</span> == <span class="literal">7</span>) <span class="type">Console</span>.Write(<span class="literal">'\n'</span>);
            }
        }
    }
}
</code></pre>



###<a id="sec-generated-title-5"></a> <a id="expression-using"></a>式だけの using ステートメント
ちなみに、using() の中身は変数宣言だけではなく、式にすることもできます。

<pre class="source" title="using(式)" lang="">
<code><span class="reserved">using</span>(<span class="input">式</span>)
{
  <span class="input">リソースに対する操作</span>
}
</code></pre>


これで、以下のようなコードと同等な処理になります。

<pre class="source" title="using(式)" lang="">
<code><span class="reserved">using</span>(<span class="type">IDisposable</span> r = <span class="input">式</span>)
{
  <span class="input">リソースに対する操作</span>
}
</code></pre>


さらに展開すると、以下のような意味です。

<pre class="source" title="using(式) の解釈" lang="">
<code><span class="type">Resource</span> r = <span class="input">式</span>;
<span class="reserved">try</span>
{
  <span class="input">リソースに対する操作</span>
}
<span class="reserved">finally</span>
{
  <span class="reserved">if</span>(r != <span class="reserved">null</span>)
    r.Dispose();
}
</code></pre>


用途としては例えば、以下の「[ジェネリック](../oop/sp2_generics.md#generics)」を使ったメソッドのように、
T が IDispose を実装している時だけ Dispose を呼び出したい場合などに便利です。

<pre class="source" title="IDispose を実装している時だけ Dispose を呼び出し" lang="">
<code><span class="reserved">static void</span> GenericMethod&lt;T&gt;(T obj)
{
    <span class="reserved">using</span> (obj <span class="reserved">as</span> <span class="type">IDisposable</span>)
    {
       <span class="input">obj に対する操作</span>
    }
}
</code></pre>

###<a id="sec-generated-title-6"></a> <a id="dtor"></a>Dispose とファイナライザー
ちなみに、確実に解放しなければならないリソースは、`Dispose`メソッドだけでなく、ファイナライザーも使って破棄処理を行うべきです。
詳しくは「[IDisposable インターフェイスの実装](rm_disposable.md#idisposable)」で説明します。

##<a id="sec-generated-title-7"></a> <a id="using-declaration"></a>using 変数宣言
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、変数宣言に対して `using` 修飾を付けることで、
その変数のスコープに紐づいて `using` ステートメントと同じ効果を得られるようになりました。
これを `using` 変数宣言(using declaration)と呼びます。

例えば以下のように書きます。

<pre class="source" title="using 変数宣言">
<code><span class="reserved">using</span> System;
 
<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">DeferredMessage</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">string</span> _message;
    <span class="reserved">public</span> <span class="type">DeferredMessage</span>(<span class="reserved">string</span> <span class="variable">message</span>) =&gt; _message = <span class="variable">message</span>;
 
    <span class="comment">// Dispose 時にメッセージ表示</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(_message);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// using var で、変数のスコープに紐づいた using になる。</span>
        <span class="comment">// スコープを抜けるときに Dispose が呼ばれる。</span>
        <span class="reserved">using</span> <span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span> <span class="type">DeferredMessage</span>(<span class="string">&quot;a&quot;</span>);
        <span class="reserved">using</span> <span class="reserved">var</span> <span class="variable">b</span> = <span class="reserved">new</span> <span class="type">DeferredMessage</span>(<span class="string">&quot;b&quot;</span>);
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;c&quot;</span>);
 
        <span class="comment">// c, b, a の順でメッセージが表示される</span>
    }
}
</code></pre>

`Main` メソッド内は以下のコードと同じ意味になります。

<pre class="source" title="using 変数宣言の展開">
<code><span class="comment">// using var で、変数のスコープに紐づいた using になる。</span>
<span class="comment">// スコープを抜けるときに Dispose が呼ばれる。</span>
<span class="reserved">using</span> (<span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span> <span class="type">DeferredMessage</span>(<span class="string">&quot;a&quot;</span>))
{
    <span class="reserved">using</span> (<span class="reserved">var</span> <span class="variable">b</span> = <span class="reserved">new</span> <span class="type">DeferredMessage</span>(<span class="string">&quot;b&quot;</span>))
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;c&quot;</span>);
    }
}
</code></pre>

この展開結果からもわかるように、複数の `using` 変数宣言が並んでいた場合、
`Dispose` メソッドの呼び出しは宣言の逆順で行われます。
この例では、変数は `a`、`b` の順で宣言しているので、
`Dispose` は `b`、`a` の順になります。

###<a id="sec-generated-title-8"></a> <a id="using-declaration-pitfall"></a>using 変数宣言の注意点
`using` 変数宣言は両手放し喜べる機能ではありません。
`Dispose` が呼ばれるタイミングを伸ばしてしまって、パフォーマンスに悪影響を及ぼす可能性があります。
例えば以下のコードを考えます。

<pre class="source" title="using 変数宣言に単純置き換えしない方がいい例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Threading;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">string</span> <span class="variable">content</span>;
        <span class="reserved">using</span> (<span class="reserved">var</span> <span class="variable">s</span> = <span class="reserved">new</span> <span class="type">StreamReader</span>(<span class="string">&quot;sample.txt&quot;</span>))
        {
            <span class="variable">content</span> = <span class="variable">s</span>.<span class="method">ReadToEnd</span>();
        }
        <span class="comment">// s.Dispose はここで呼ばれる。</span>
 
        <span class="comment">// すごく長い処理。ここでは Sleep で代用。</span>
        <span class="type">Thread</span>.<span class="method">Sleep</span>(5000);
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">content</span>);
    }
}
</code></pre>

ファイルからの内容読み込み後、少し長い処理が挟まってからその内容を使います。
これを単純に `using` 変数宣言に置き換えたとしましょう。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Threading;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">using</span> <span class="reserved">var</span> <span class="variable">s</span> = <span class="reserved">new</span> <span class="type">StreamReader</span>(<span class="string">&quot;sample.txt&quot;</span>);
        <span class="reserved">var</span> <span class="variable">content</span> = <span class="variable">s</span>.<span class="method">ReadToEnd</span>();
 
        <span class="comment">// すごく長い処理。ここでは Sleep で代用。</span>
        <span class="type">Thread</span>.<span class="method">Sleep</span>(5000);
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">content</span>);
        <span class="comment">// s.Dispose はここで呼ばれる。</span>
    }
}
</code></pre>

この例では、`Dispose` が呼ばれるタイミングが5秒、無駄に遅れることになります。
5秒もファイルを開きっぱなしになるのでだいぶ悪影響があります。
なので、この場合は単純置き換えがダメだということです。

ただ、実用的には、この手の処理は必要な部分だけメソッドに切り出すことが多いです。
この例でも、実際には以下のように書くべきでしょう。
これならまさに `using` 変数宣言がふさわしい書き方です。

<pre class="source" title="using が必要な範囲だけメソッド抽出">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Threading;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">content</span> = <span class="method">ReadToEnd</span>(<span class="string">&quot;sample.txt&quot;</span>);
 
        <span class="comment">// すごく長い処理。ここでは Sleep で代用。</span>
        <span class="type">Thread</span>.<span class="method">Sleep</span>(5000);
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">content</span>);
    }
 
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="method">ReadToEnd</span>(<span class="reserved">string</span> <span class="variable">path</span>)
    {
        <span class="reserved">using</span> <span class="reserved">var</span> <span class="variable">s</span> = <span class="reserved">new</span> <span class="type">StreamReader</span>(<span class="variable">path</span>);
        <span class="control">return</span> <span class="variable">s</span>.<span class="method">ReadToEnd</span>();
        <span class="comment">// s.Dispose はここで呼ばれる。</span>
    }
}
</code></pre>

##<a id="sec-generated-title-9"></a> <a id="interface-required"></a>IDispose インターフェイス必須
C# の構文の多くは、C# コンパイラーによる簡単な置き換え
(いわゆる構文糖衣(syntax sugar))になっています。
例えば、[`foreach`ステートメント](../data/sp_foreach.md#foreach)の場合は `GetEnumerator`、`MoveNext`、`Current` などのメソッド/プロパティ呼び出しに置き換えられます。

この際、C# の大体の構文糖衣は
「ある特定の名前のメソッドさえ持っていれば使える」
という割と緩い条件になっていて、
これをパターン ベース(pattern-based)な構文と呼びます。

そんな中、`using` ステートメントだけは `IDisposable` インターフェイス(`System` 名前空間)の実装が必須です。
[次節で説明する](#pattern-based-using)ように、C# 8.0 で少しだけ条件緩和されましたが、
既存のコードを壊さないようにするためにはかなり限定的にせざるを得なかったらしく、基本的にはインターフェイス実装が必須です。

<pre class="source" title="using ステートメントの利用には IDisposable インターフェイスの実装が(ほぼ)必須">
<code><span class="reserved">using</span> System;
 
<span class="comment">// using で使える型。</span>
<span class="reserved">class</span> <span class="type">Disposable</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="comment">// 残念ながら IDisposable を実装していないと using で使えない。</span>
<span class="reserved">class</span> <span class="type">NonDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// こっちは OK。</span>
        <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">Disposable</span>()) { }
 
        <span class="comment">// こっちはコンパイル エラーに。</span>
        <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">NonDisposable</span>()) { }
    }
}
</code></pre>

一方で、C# 8.0 で新規導入する非同期 `using` ステートメントの場合は、
既存コードのことを心配する必要がないため、元からパターン ベースにしてあります。
すなわち、別に `IAsyncDisposable` インターフェイスの実装は必要ありません。

<pre class="source" title="非同期 using はパターン ベース">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading.Tasks;
 
<span class="comment">// 非同期 using は別に IAsyncDisposable インターフェイスの実装を求めない。</span>
<span class="reserved">class</span> <span class="type">AsyncDisposable</span>
{
    <span class="comment">// ちゃんと await using のブロックの最後で呼ばれる。</span>
    <span class="comment">// 戻り値の型が Task や ValueTask である必要もない。</span>
    <span class="reserved">public</span> <span class="type">MyAwaitable</span> <span class="method">DisposeAsync</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;disposed async&quot;</span>);
        <span class="control">return</span> <span class="reserved">default</span>;
    }
}
 
<span class="reserved">struct</span> <span class="type">MyAwaitable</span>&lt;<span class="type">T</span>&gt; { <span class="reserved">public</span> <span class="type">ValueTaskAwaiter</span>&lt;<span class="type">T</span>&gt; <span class="method">GetAwaiter</span>() =&gt; <span class="reserved">default</span>; }
<span class="reserved">struct</span> <span class="type">MyAwaitable</span> { <span class="reserved">public</span> <span class="type">ValueTaskAwaiter</span> <span class="method">GetAwaiter</span>() =&gt; <span class="reserved">default</span>; }
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
        <span class="reserved">await</span> <span class="reserved">using</span>(<span class="reserved">new</span> <span class="type">AsyncDisposable</span>())
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;iside using&quot;</span>);
        }
    }
}
</code></pre>

<pre class="console" title="非同期 using はパターン ベース">
<code>iside using
disposed async
</code></pre>

###<a id="sec-generated-title-10"></a> <a id="pattern-based-using"></a>パターン ベースな using
<h5 class="version version8">Ver. 8.0</h5>

`using` ステートメントで使うのにインターフェイスの実装が必須となると、
C# 7.2 で導入された[ref 構造体](refstruct.md)で困ることになりました。

ref 構造体を使いたいような場面では `Dispose` したいリソースを握ることもあり、
`using` ステートメントを使いたい動機があります。
しかし、ref 構造体にはインターフェイスが実装できません。

ということで、`using` ステートメントもパターン ベースにしてしまおうということになりました。
ところが、無条件に変更すると既存のコードを壊しかねない懸念があって断念されました。

その結果、C# 8.0 では、ref 構造体に対してだけパターン ベースでの `using` ステートメントを認めることにしました。
以下のようになります。

<pre class="source" title="ref 構造体に対するパターン ベース using">
<code><span class="reserved">using</span> System;
 
<span class="comment">// これまで通り、using で使える型。</span>
<span class="reserved">struct</span> <span class="type">Disposable</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="comment">// 残念ながら IDisposable を実装していないと using で使えない。</span>
<span class="reserved">struct</span> <span class="type">NonDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="comment">// となると、インターフェイスを実装できない ref struct で困っていた。</span>
<span class="comment">// ref struct の場合、IDisposable なしでも Dispose メソッドさえあれば using で使えるようになった。</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// この行は元々 OK。</span>
        <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">Disposable</span>()) { }
 
        <span class="comment">// 残念ながら今でもコンパイル エラーに。</span>
        <span class="reserved">using</span> (<span class="error"><span class="reserved">new</span> <span class="type">NonDisposable</span>()</span>) { }
 
        <span class="comment">// C# 8.0 で、これは OK になった。</span>
        <span class="reserved">using</span> (<span class="reserved">new</span> <span class="type">RefDisposable</span>()) { }
    }
}
</code></pre>

この変更は、`foreach` ステートメントに対しても適用されます。
`foreach` ステートメントは、列挙対象が `IDisposable` だった場合に `Dispose` メソッドを呼び出す仕様になっています。

<pre class="source" title="foreach 最後の Dispose 呼び出しがパターン ベースに">
<code><span class="reserved">using</span> System;
 
<span class="comment">// GetEnumerator/MoveNext/Current は元々パターン ベース。</span>
<span class="comment">// ただ、Dispose の呼び出しだけは IDisposable の実装が必須だった。</span>
<span class="comment">// C# 8.0 で、ref struct の場合はパターン ベースで Dispose メソッドを呼んでもらえるように。</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefEnumerable</span>
{
    <span class="reserved">public</span> <span class="type">RefEnumerable</span> <span class="method">GetEnumerator</span>() =&gt; <span class="reserved">this</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; 0;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() =&gt; <span class="reserved">false</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;ref disposed&quot;</span>);
}
 
<span class="comment">// RefEnumerable と比べて、 ref を取っただけ。</span>
<span class="reserved">struct</span> <span class="type">BrokenEnumerable</span>
{
    <span class="reserved">public</span> <span class="type">BrokenEnumerable</span> <span class="method">GetEnumerator</span>() =&gt; <span class="reserved">this</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; 0;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() =&gt; <span class="reserved">false</span>;
 
    <span class="comment">// この Dispose は呼ばれない。</span>
    <span class="comment">// ref struct でない場合、IDisposable インターフェイスの実装が必須。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;broken disposed&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// ref disposed は表示される。</span>
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">RefEnumerable</span>()) ;
 
        <span class="comment">// broken disposed は表示されない。</span>
        <span class="comment">// コンパイル エラーにはならないので特に注意。</span>
        <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">_</span> <span class="control">in</span> <span class="reserved">new</span> <span class="type">BrokenEnumerable</span>()) ;
    }
}
</code></pre>

<pre class="console" title="非同期 using はパターン ベース">
<code>ref disposed
</code></pre>

##<a id="sec-generated-title-11"></a> <a id="await-using"></a>非同期using
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0で非同期版の`using`が追加されました。
`await using`という構文で、`IAsyncDisposable`インターフェイス(`System`名前空間)か、
それと同じ[パターン](../async/asyncstream.md#await-foreach)を満たす型の列挙ができます。

<pre class="source" title="非同期using">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncUsing</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IAsyncDisposable</span>
{
    <span class="reserved">await</span> <span class="reserved">using</span> (<span class="variable">x</span>)
    {
        <span class="comment">// x を破棄する前にやっておきたい処理</span>
    }
}
</code></pre>

詳しくは「[非同期using](../async/asyncstream.md#await-using)」で説明します。
