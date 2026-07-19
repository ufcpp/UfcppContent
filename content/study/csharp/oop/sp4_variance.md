---
title: "ジェネリクスの共変性・反変性"
source_url: "https://ufcpp.net/study/csharp/oop/sp4_variance/"
content_type: "Article"
published_at: "2009-05-24T00:00:00"
updated_at: "2016-06-12T00:00:00"
tags:
  - "Ver. 4.0"
umbraco_id: 1274
parent_id: 1248
sort_order: 20
aliases:
  - "/csharp/oop/sp4_variance/"
  - "/csharp/sp4_variance"
  - "/csharp/sp4_variance.html"
  - "/study/csharp/sp4_variance"
  - "/study/csharp/sp4_variance.html"
---

# ジェネリクスの共変性・反変性

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 で、ジェネリクスの型引数に共変性・反変性を持たせることが可能になりました。
（共変性・反変性という言葉の意味は「[covariance と contravariance](../functional/sp_delegate.md#co-contra)」参照。）


## <a id="sec-generated-title-2"></a> <a id="variance"></a>ジェネリックの共変性・反変性

ジェネリクスの共変性・反変性というものがどういうものかというのを説明する前に、まず背景を。
ジェネリックコレクションに関して、昔から以下のようなことをしたいという要望がありました。

<pre class="source" title="string のリストを object のリストに代入" lang="">
<code>List&lt;<span class="reserved">string</span>&gt; strings = {<span class="literal">"aa"</span>, <span class="literal">"bb"</span>, <span class="literal">"cc"</span>};
List&lt;<span class="reserved">object</span>&gt; objs = strings;
</code></pre>


これを認めてしまうと何がまずいかというと、
以下のような不正な値の書き換えが起こり得る。

<pre class="source" title="不正な書き換え" lang="">
<code><span class="comment">// strings と objs は同じオブジェクト</span>
objs[0] = 5; <span class="comment">// int に書き換えられたらまずい</span>
<span class="reserved">string</span> str = strings[0];
</code></pre>


この問題が起きる原因がどこにあるかというと、
List が set も get も可能なインデクサーを持っていることです。

get しかない場合なら、ここで挙げたような不正な書き換えは起こらないわけです。
戻り値（あるいは get）でしか使わない型の場合、

<pre class="source" title="string の列挙子を object の列挙子に代入" lang="">
<code>IEnumerable&lt;<span class="reserved">string</span>&gt; strings = <span class="reserved">new</span>[] {<span class="literal">"aa"</span>, <span class="literal">"bb"</span>, <span class="literal">"cc"</span>};
IEnumerable&lt;<span class="reserved">object</span>&gt; objs = strings;
<span class="comment">// foreach (object x in strings) ってやっても問題ないんだから、
// objs に strings を代入しても OK。</span>
</code></pre>


みたいな事が出来ても問題ないはず。
（こういうのを<strong id="covariance" class="keyword">共変性</strong>（covariance）と言います。）

逆に、引数（あるいは set）でしか使わない場合も、

<pre class="source" title="object 引数の Action を string 引数の Action に代入。" lang="">
<code>Action&lt;<span class="reserved">object</span>&gt; objAction = x =&gt; { Console.Write(x); };
Action&lt;<span class="reserved">string</span>&gt; strAction = objAction;
<span class="comment">// objAction("string"); ってやっても問題ないんだから、
// strAction に objAction を代入しても OK。</span>
</code></pre>


みたいな事をして大丈夫。
（こういうのを<strong id="contravariance" class="keyword">反変性</strong>（contravariance）といいます。）

![ジェネリックの共変性・反変性](../../../../assets/media/1081/genericvariance.png)

## <a id="sec-generated-title-3"></a> <a id="in_out"></a>in/out 修飾子

ということで、C# 4.0 から、ジェネリックなインターフェース、もしくは、デリゲートに対して、
共変性・反変性を実現するための仕組みが追加されました。

共変性のためには「型を出力（戻り値、get）にしか使わない」、
反変性のためには「型を入力（引数、set）にしか使わない」という保証があればいいので、
それぞれ、ジェネリクスの型引数に out と in という修飾子を付けることでこれを保証します。
（ちなみに、この out と in 修飾子のことを<strong id="variance-annotation" class="keyword">変性注釈</strong>（variance annotation）と呼ぶそうです。）

まず、出力（メソッドの戻り値、プロパティの get）にしか使わない型には out という修飾子を指定します。
例えば、.NET Framework 4.0 では、IEnumerator の型引数に out が付きました。

<pre class="source" title="IEnumerator に out が付きました" lang="">
<code><span class="reserved">public interface</span> IEnumerator&lt;<span class="reserved">out</span> T&gt;
{
  T Current { <span class="reserved">get</span>; } <span class="comment">// get しかない ＝ 出力のみ</span>
  <span class="reserved">bool</span> MoveNext();
  <span class="reserved">void</span> Reset();
}
</code></pre>


こうすることで、共変性が認められます。

<pre class="source" title="out 型引数の共変性" lang="">
<code>IEnumerator&lt;<span class="reserved">string</span>&gt; strEnum = <span class="reserved">new</span> Enumerator&lt;<span class="reserved">string</span>&gt;();
IEnumerator&lt;<span class="reserved">object</span>&gt; objEnum = strEnum;
</code></pre>


一方、入力（メソッドの引数、プロパティの set）にしか使わない型には in という修飾子を指定します。
例えば、IComparer の型引数に in が付きました。

<pre class="source" title="IComparer に　in が付きました" lang="">
<code><span class="reserved">public interface</span> IComparer&lt;<span class="reserved">in</span> T&gt;
{
  <span class="reserved">int</span> Compare(T a, T b); <span class="comment">// T は引数としてしか使われない</span>
}
</code></pre>


こうすることで、今度は反変性が認められます。

<pre class="source" title="out 型引数の反変性" lang="">
<code>IComparer&lt;<span class="reserved">object</span>&gt; objComp = <span class="reserved">new</span> Comparer&lt;<span class="reserved">object</span>&gt;();
IComparer&lt;<span class="reserved">string</span>&gt; strComp = objComp;
</code></pre>


当然、in/out の組み合わせもあり得ます。

<pre class="source" title="Func には in/out 両方付いてます" lang="">
<code><span class="reserved">public delegate</span> TResult Func&lt;<span class="reserved">in</span> T1, <span class="reserved">in</span> T2, <span class="reserved">out</span> TResult&gt;(T1 arg1, T2 arg2);
</code></pre>


<pre class="source" title="共変性・反変性両方使う" lang="">
<code>Func&lt;<span class="reserved">object</span>, <span class="reserved">object</span>, <span class="reserved">string</span>&gt; f1 = (x, y) =&gt; <span class="reserved">string</span>.Format(<span class="literal">"({0}, {1})"</span>, x, y);
Func&lt;<span class="reserved">string</span>, <span class="reserved">string</span>, <span class="reserved">object</span>&gt; f2 = f1;
</code></pre>



## <a id="sec-generated-title-4"></a> <a id="implementation"></a>余談1： in/out の内部実装

型引数の in/out のような仕組みの実現には 「[IL](../abstract/ab_dotnet.md#il)」 レベルでの対応が必要になります。
というか、IL レベルでは、.NET Framework 2.0 の時点で in/out 相当のフラグを設定する機能がありました。
（今回、C# からそのフラグを立てれるようになっただけ。）

例えば、C# 4.0 で以下のようなソースを書いて、

<pre class="source" title="in/out 付きのインターフェース定義" lang="">
<code><span class="reserved">namespace</span> ConsoleApplication1
{
    <span class="reserved">public interface</span> IEnumerator&lt;<span class="reserved">out</span> T&gt;
    {
        T Current { <span class="reserved">get</span>; }
        <span class="reserved">bool</span> MoveNext();
    }
    <span class="reserved">public interface</span> IComparable&lt;<span class="reserved">in</span> T&gt;
    {
        <span class="reserved">int</span> CompareTo(T x);
    }
}
</code></pre>


一度コンパイルしたものを .NET Framework 2.0 付属の IL Disasm（.NET Framework 付属の IL 逆アセンブラー）で開いてみると、
型引数 T の前に + や - が付いていることを確認できます。

<figure>
	[![in/out 付きインターフェースのコンパイル結果](../../../../assets/media/ufcpp2000/csharp/fig/variance.png)](../../../../assets/media/ufcpp2000/csharp/fig/variance.png)
	<figcaption>in/out 付きインターフェースのコンパイル結果</figcaption>
</figure>


仕組みとしては .NET Framework 2.0 の頃からあったので、
IL アセンブラーを使ってこの +/- フラグを立ててやれば、
C# 3.0 以前でも共変性・反変性を使えたりします。
（一度 object にしてから無理やりキャストする必要はある。）


## <a id="sec-generated-title-5"></a> <a id="value"></a>余談2： 値型は　invariant

ちなみに、値型（int とかの組み込み整数型や、struct、enum）には共変性・反変性は使えません。
（「[IL](../abstract/ab_dotnet.md#il)」 の実装上の制約。）

<pre class="source" title="値型は共変性・反変性を使えない" lang="">
<code>IEnumerable&lt;<span class="reserved">object</span>&gt; e1 = <span class="reserved">new</span>[] { <span class="literal">"abc"</span>, <span class="literal">"def"</span> }; <span class="comment">// こっちは OK。</span>
IEnumerable&lt;<span class="reserved">object</span>&gt; e2 = <span class="reserved">new</span>[] { 1, 2 };         <span class="comment">// でも、これは不可。int が値型だから。</span>
</code></pre>


<!-- original-page-break -->


## <a id="sec-generated-title-6"></a> <a id="covariant-array"></a>余談3: C#の配列は共変

C#の配列には共変性があります。つまり、以下のコードがコンパイルできます。

<pre class="source" title="C#の配列は共変">
<code><reserved></span><span class="reserved">string</span>[] derivedItems = { <span class="string">"Aleph"</span>, <span class="string">"Beth"</span>, <span class="string">"Gimel"</span> };
<span class="reserved">object</span>[] baseItems = derivedItems;

<span class="comment">// 読み出し(戻り値側、out、共変)は常に安全</span>
<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; baseItems.Length; i++)
{
    <span class="type">Console</span>.WriteLine(baseItems[i]);
}
</code></pre>

逆向き(反変な代入)はできません。

<pre class="source" title="反変ではない">
<code><reserved></span><span class="reserved">object</span>[] baseItems = { 1, 2, 3 };
<span class="reserved">string</span>[] derivedItems = baseItems; <span class="comment">// コンパイル エラー</span>
</code></pre>

C#の配列が共変なのは、ジェネリックがなかった時代(C# 1.0の頃)の名残です。
本当は認めるべきではありません。

共変性は、本来、出力(読み出し)になる型にしか認められません。
しかし、配列は、同じ型に対して入力(書き込み)もできます。
配列に対して特別に共変性を認めてしまっているので、以下のような問題が起きます。

<pre class="source" title="配列に対する不正な操作">
<code><reserved></span><span class="reserved">string</span>[] derivedItems = { <span class="string">"Aleph"</span>, <span class="string">"Beth"</span>, <span class="string">"Gimel"</span> };
<span class="reserved">object</span>[] baseItems = derivedItems;

<span class="comment">// 書き込み(引数側、in、反変)は本当はやっちゃいけない</span>
<span class="comment">// でも、コンパイルが成功する。実行時エラーが出る</span>
baseItems[1] = 100;
</code></pre>

本当はコンパイル自体できてはいけないコードですが、実行してみるまでエラーになりません。
`IEnumerable<T>`や`IReadOnlyCollection<T>`などのジェネリックなインターフェイスを介してのアクセスであれば、こういう問題のあるコードは書けません。

## <a id="sec-generated-title-7"></a> <a id="paramter-delegate"></a>引数でインターフェイスやデリゲートを受け取る場合

ジェネリックなインターフェイスやデリゲートを引数として渡す場合、in/outの向きが逆転します。
(戻り値の場合は逆転しません。)
例えば以下のようになります。

<pre class="source" title="in/out、引数/戻り値の逆転">
<code><span class="comment">// 標準ライブラリの System.Func</span>
<span class="reserved">public</span> <span class="reserved">delegate</span> <span class="type">TResult</span> <span class="type">Func</span>&lt;<span class="reserved">in</span> <span class="type">T</span>, <span class="reserved">out</span> <span class="type">TResult</span>&gt;(<span class="type">T</span> arg);

<span class="comment">// 引数の Func の TIn と TOut が逆</span>
<span class="reserved">delegate</span> <span class="type">Func</span>&lt;<span class="type">TIn</span>, <span class="type">TOut</span>&gt; <span class="type">F</span>&lt;<span class="reserved">in</span> <span class="type">TIn</span>, <span class="reserved">out</span> <span class="type">TOut</span>&gt;(<em><span class="type">Func</span>&lt;<span class="type">TOut</span>, <span class="type">TIn</span>&gt;</em> x);
</code></pre>

in/out 注釈は、値を受け取る(in)か渡す(out)かの区別です。
引数で受け取ったインターフェイスやデリゲートの場合、「戻り値から値を受け取る」、「引数に値を渡す」ということになるので、こういう逆転が起きます。

<pre class="source" title="戻り値から値を受け取る、引数に値を渡す">
<code><reserved></span><span class="reserved">interface</span> <span class="type">INestedVariance</span>&lt;<span class="reserved">in</span> <span class="type">TIn</span>, <span class="reserved">out</span> <span class="type">TOut</span>&gt;
{
    <span class="type">TOut</span> F(<span class="type">TIn</span> x, <span class="type">Func</span>&lt;<span class="type">TOut</span>, <span class="type">TIn</span>&gt; f);
}

<span class="reserved">class</span> <span class="type">NestedVariance</span>&lt;<span class="type">TIn</span>, <span class="type">TOut</span>&gt; : <span class="type">INestedVariance</span>&lt;<span class="type">TIn</span>, <span class="type">TOut</span>&gt;
{
    <span class="reserved">public</span> <span class="type">TOut</span> F(<span class="type">TIn</span> x, <span class="type">Func</span>&lt;<span class="type">TOut</span>, <span class="type">TIn</span>&gt; f)
    {
        <span class="comment">// f の戻り値から値を受け取る = in</span>
        <span class="type">TIn</span> in1 = f(<span class="reserved">default</span>(<span class="type">TOut</span>));

        <span class="comment">// f の引数にはこちらから値を渡す = out</span>
        <span class="type">TOut</span> out1 = <span class="reserved">default</span>(<span class="type">TOut</span>);
        <span class="reserved">var</span> r = f(out1);

        <span class="comment">// 引数から受け取る = in</span>
        <span class="type">TIn</span> in2 = x;

        <span class="comment">// 戻り値を返す = out</span>
        <span class="type">TOut</span> out2 = <span class="reserved">default</span>(<span class="type">TOut</span>);
        <span class="reserved">return</span> out2;
    }
}
</code></pre>

実用例の代表は、`IObserver<T>`インターフェイスと`IObservable<T>`インターフェイス(どちらも標準ライブラリの`System`名前空間に含まれるインターフェイス)でしょう。
以下のようなインターフェイスになっています。

<pre class="source" title="in/outが逆になる実用例">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IObserver<em></span>&lt;<span class="reserved">in</span> <span class="type">T</span>&gt;</em>
{
    <span class="reserved">void</span> OnCompleted();
    <span class="reserved">void</span> OnError(<span class="type">Exception</span> error);
    <span class="reserved">void</span> OnNext(<span class="type">T</span> value);
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IObservable<em></span>&lt;<span class="reserved">out</span> <span class="type">T</span>&gt;</em>
{
    <span class="type">IDisposable</span> Subscribe(<em><span class="type">IObserver</span>&lt;<span class="type">T</span>&gt;</em> observer);
}
</code></pre>
