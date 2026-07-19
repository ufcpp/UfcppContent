---
title: "[雑記] 匿名関数のコンパイル結果"
source_url: "https://ufcpp.net/study/csharp/functional/sp2_anonymousmethod/"
content_type: "Article"
published_at: "2008-03-09T00:00:00"
updated_at: "2017-12-03T00:00:00"
tags:
  - "Ver. 2.0"
umbraco_id: 1279
parent_id: 1275
sort_order: 5
aliases:
  - "/csharp/functional/sp2_anonymousmethod/"
  - "/csharp/sp2_anonymousmethod"
  - "/csharp/sp2_anonymousmethod.html"
  - "/study/csharp/sp2_anonymousmethod"
  - "/study/csharp/sp2_anonymousmethod.html"
---

# \[雑記\] 匿名関数のコンパイル結果

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

本項では、
[匿名関数](sp_delegate.md#anonymous)が内部的にどう実現されているかについて説明します。

匿名関数は、C# コンパイラーによって普通のメソッドに展開されます。
単にメソッドが1つ生成されるだけの場合もあれば、クラスを丸ごと生成する場合もあります。

## <a id="sec-generated-title-2"></a> <a id="compile_anonymous"></a>匿名関数のコンパイル結果

例えば、以下のようなコードは、

<pre class="source" title="匿名関数の例1" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
      <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = () => 0;
      f1();
  }
}
</code></pre>

以下のコードと同じ意味になります。
(※ 古いC#コンパイラーの実装の場合だけです。現在は、静的メソッドの場合もう少し複雑なコード生成にした方がパフォーマンスがいいらしく、変換結果が変わっています。現在の実装については[後述](#static))

<pre class="source" title="例1のコンパイル結果" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static int</span> AnonymousMethod1()
    {
        <span class="reserved">return</span> 0;
    }

    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = AnonymousMethod1;
        f1();
    }
}
</code></pre>

この例の場合は、クラスのフィールドの使わず、ローカル変数の捕獲もしていないので、静的メソッドに変換されます。

ただし、`AnonymousMethod1` の部分は、
実際には `<Main>b__0` とかいうような、
C# では通常記述できないような特殊な名前になっていて、
プログラマが明示的に参照することはできません。


### <a id="sec-generated-title-3"></a> <a id="instance-member"></a>メンバー変数を参照する場合

匿名関数内で、クラスのメンバー変数を参照するような場合には、
インスタンス メソッド（非 static なメソッド）が自動生成されます。

例えば、以下のようなコードは、

<pre class="source" title="匿名関数の例2" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">int</span> member = 0;

  <span class="reserved">void</span> Method()
  {
    <span class="comment">// 2. メンバー変数を参照する匿名関数</span>
    <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f2 = () => <span class="reserved">this</span>.member;
    f2();
  }
}
</code></pre>

以下のように展開されます。

<pre class="source" title="例2のコンパイル結果" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">int</span> AnonymousMethod2()
  {
    <span class="reserved">return this</span>.member;
  }

  <span class="reserved">int</span> member = 0;

  <span class="reserved">void</span> Method()
  {
    <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f2 = AnonymousMethod2;
    f2();
  }
}
</code></pre>

### <a id="sec-generated-title-4"></a> <a id="closure"></a>クロージャ(ローカル変数を参照する)の場合

ローカル変数を参照するような匿名関数(クロージャ)を書いた場合、
クラスまで自動生成されます。

例えば、以下のようなコードは、

<pre class="source" title="匿名関数の例3" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="comment">// 3. ローカル変数を参照する匿名関数</span>
    <span class="reserved">int</span> x = 0;
    <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f3 = () => ++x;
    f3();

    <span class="type">Console</span>.Write(x);
  }
}
</code></pre>

コンパイル時に以下のようなクラスを生成したうえで、実行時にそのインスタンスが作られます。

<pre class="source" title="例3のコンパイル結果" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">class</span> <span class="type">AnonymousClass</span>
  {
    <span class="reserved">public int</span> x;

    <span class="reserved">public int</span> AnonymousMethod()
    {
      <span class="reserved">return</span> ++<span class="reserved">this</span>.x;
    }
  }

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">var</span> temp = <span class="reserved">new</span> <span class="type">AnonymousClass</span>();
    temp.x = 0;
    <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f3 = temp.AnonymousMethod;
    f3();

    <span class="type">Console</span>.Write(temp.x);
  }
}
</code></pre>

ローカル変数の変わりに、自動生成されたクラスのメンバー変数アクセスになっています。

呼び出し元とクロージャ側とで、ローカル変数`x`の書き換え結果が共有される(実行結果で 1 が表示される)のは、このコード生成のおかげです。
例えば以下のように、ローカル変数を書き換えるコードを書いたとします。

<pre class="source" title="例4： 匿名関数で参照している変数の書き換え" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">int</span> x = 0;
    <span class="type">Action</span> f = () => <span class="type">Console</span>.Write(x);

    x = 1;
    f();
  }
}
</code></pre>

このコードは以下のように展開されます。

<pre class="source" title="例4のコンパイル結果" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">class</span> <span class="type">AnonymousClass</span>
  {
    <span class="reserved">public int</span> x;

    <span class="reserved">public void</span> AnonymousMethod()
    {
      <span class="type">Console</span>.Write(<span class="reserved">this</span>.x);
    }
  }

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="reserved">var</span> temp = <span class="reserved">new</span> <span class="type">AnonymousClass</span>();
    temp.x = 0;
    <span class="type">Action</span> f = temp.AnonymousMethod;

    temp.x = 1;
    f();
  }
</code></pre>

すなわち、元々のコードでローカル変数だったものは、クラスのフィールドになっています。
これを、「ローカル変数がフィールドに昇格(elevate)した」と言ったりします。
「昇格」と言っても、えらくなったわけでなくて、むしろ、実行性能上はペナルティになります。
クラスのインスタンスが1つ余計に作られる分、ちょっとした負担が発生しています。

### <a id="sec-generated-title-5"></a> <a id="closure-local-function"></a>ローカル関数かつクロージャの場合

前述の通り、クロージャにはローカル変数の昇格と、それに伴う余計なインスタンス生成が伴います。
これに対して、状況が許せばその余計なインスタンス生成を避けるような最適化ができます。
最適化できる状況は、以下の通りです。

- ローカル関数でクロージャを作っている(匿名関数ではない)
- デリゲートに代入したりせず、直接関数呼び出ししている

<pre class="source" title="クロージャが最適かできるかどうかの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> M1(<span class="reserved">int</span> m, <span class="reserved">int</span> n)
{
    <span class="comment">// <em>最適化できる状況: ローカル関数を直接呼出し</em></span>
    <span class="reserved">int</span> f(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; m * x + n * y;
    <span class="reserved">var</span> r = f(3, 4);
}

<span class="reserved">static</span> <span class="reserved">void</span> M2(<span class="reserved">int</span> m, <span class="reserved">int</span> n)
{
    <span class="comment">// できない状況1: デリゲート越しに使っている</span>
    <span class="reserved">int</span> f(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; m * x + n * y;
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; func = f;
    <span class="reserved">var</span> r2 = func(3, 4);
}

<span class="reserved">static</span> <span class="reserved">void</span> M3(<span class="reserved">int</span> m, <span class="reserved">int</span> n)
{
    <span class="comment">// できない状況2: 匿名関数を使っている</span>
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; f3 = (x, y) =&gt; m * x + n * y;
    <span class="reserved">var</span> r3 = f3(3, 4);
}
</code></pre>

最適化できる状況、例えばこの例の`M1`の場合、以下のようなコードに展開されます。

<pre class="source" title="クロージャに伴うインスタンス生成を避ける最適化">
<code><reserved></span><span class="reserved">struct</span> <span class="type">State</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> m;
    <span class="reserved">public</span> <span class="reserved">int</span> n;
}

<span class="reserved">static</span> <span class="reserved">int</span> Anonymous(<span class="reserved">int</span> x, <span class="reserved">int</span> y, <span class="reserved">ref</span> <span class="type">State</span> state)
{
    <span class="reserved">return</span> state.m * x + state.n * y;
}

<span class="reserved">static</span> <span class="reserved">void</span> M1(<span class="reserved">int</span> m, <span class="reserved">int</span> n)
{
    <span class="comment">// 最適化できる状況: ローカル関数を直接呼出し</span>
    <span class="reserved">var</span> state = <span class="reserved">new</span> <span class="type">State</span> { m = m, n = n };
    <span class="reserved">var</span> r = Anonymous(3, 4, <span class="reserved">ref</span> state);
}
</code></pre>

この違いは構造体とクラス(値型と参照型)の差によります。
詳しくは「[値型と参照型](../resource/oo_reference.md)」で説明していますが、
参照型を使うとヒープの確保という少し重たい処理が必要になります。
状況が許すなら値型を使って性能改善ができる場合があり、本節で説明しているクロージャの最適化はまさにその場合に当てはまります。

## <a id="sec-generated-title-6"></a> <a id="static"></a>補足: 静的メソッドにできる場合でも静的メソッドにしない

冒頭の例のように、
インスタンス メンバーもローカル変数使っていないような場合、匿名関数は静的メソッドとして実装してもよいはずです。
実際、昔の C# コンパイラーは静的メソッドを生成していました。

しかし、C# 6.0の頃から、静的メソッドは使わなくなりました。
例えば、冒頭の例を改めて使いますが、以下の例の場合、

<pre class="source" title="匿名関数の例1" lang="">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
      <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = () => 0;
      f1();
  }
}
</code></pre>

C# 5.0までは静的メソッドが生成されていましたが、
現在は以下のように展開されます。

<pre class="source" title="例1の現在の展開結果">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">class</span> <span class="type">AnonymousClass</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">AnonymousClass</span>Singleton = <span class="reserved">new</span> <span class="type">AnonymousClass</span>();
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; Cache1;

        <span class="reserved">internal</span> <span class="reserved">int</span> AnonymousMethod1()
        {
            <span class="reserved">return</span> 0;
        }
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">if</span> (<span class="type">AnonymousClass</span>.Cache1 == <span class="reserved">null</span>)
        {
            <span class="type">AnonymousClass</span>.Cache1 = <span class="type">AnonymousClass</span>.Singleton.AnonymousMethod1;
        }
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f1 = <span class="type">AnonymousClass</span>.Cache1;
        f1();
    }
}
</code></pre>

変更の理由は、この方がパフォーマンスがいいからです。
これでパフォーマンスが改善する理由は主に以下の2つです。

- キャッシュ: 作ったデリゲートはキャッシュできる
- デリゲートの性質: デリゲートがそもそも静的メソッドに対してパフォーマンスが良くない

##### <a id="sec-generated-title-7"></a>キャッシュ

デリゲートに対して`Action f = temp.AnonymousMethod;` と言うようにメソッドを代入するとき、
実際には`Action f = new Action(temp.AnonymousMethod);` というような`new`が挟まります。
この`new`の負担は大したものではないですが、なくて済むならない方がいい程度には、無視できない負担になります。

インスタンスが毎回変わる場合には、デリゲートも毎回`new`する必要がありますが、
ここで説明している例の場合は常に同じインスタンス(`AnonymousClass.Singleton`)が相手なので、
デリゲートも1インスタンスあれば十分です。

そこで、デリゲート自体をキャッシュする(`AnonymousClass.Cache`に持つ)ことでパフォーマンスが向上します。

##### <a id="sec-generated-title-8"></a>デリゲートの性質

詳細は「[[雑記] デリゲートの内部](miscdelegateinternal.md#static-method)」で説明しますが、
デリゲートの内部の仕組み上、
静的メソッドからデリゲートを作るとそれだけで遅かったりします。

これは、匿名クラスのインスタンスが1つ余計に作られる負担を差し引いてもおつりが来るくらい遅いです。
したがって、匿名関数の生成結果はインスタンス メソッドにした方が速くなります。

## <a id="sec-generated-title-9"></a> <a id="multiple-functions"></a>補足: 同じスコープに複数の匿名関数がある場合

同じスコープに複数の匿名関数がある場合、1つのクラスにまとめてメソッドが生成されます。
例えば以下のコードの場合、

<pre class="source" title="同じスコープに複数の匿名関数がある例">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> m)
    {
        <span class="comment">// ローカル関数かラムダ式か匿名デリゲート式かは無関係</span>
        <span class="reserved">void</span> a(<span class="reserved">int</span> x) =&gt; Console.WriteLine(<span class="string">"A "</span> + m * x);
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; b = x =&gt; Console.WriteLine(<span class="string">"B "</span> + m * x);
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; c = <span class="reserved">delegate</span> (<span class="reserved">int</span> x) { Console.WriteLine(<span class="string">"C "</span> + m * x); };

        Invoke(a, b, c);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Invoke(<span class="reserved">params</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;[] list)
    {
        <span class="reserved">foreach</span> (var item <span class="reserved">in</span> list) item(1);
    }
}
</code></pre>

以下のように展開されます。

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="comment">// a, b, c いずれも1つの型にまとまる</span>
<span class="reserved">class</span> <span class="type">AnonymousClass</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> m;
    <span class="reserved">internal</span> <span class="reserved">void</span> A(<span class="reserved">int</span> x) =&gt; Console.WriteLine(<span class="string">"A "</span> + x);
    <span class="reserved">internal</span> <span class="reserved">void</span> B(<span class="reserved">int</span> x) =&gt; Console.WriteLine(<span class="string">"B "</span> + x);
    <span class="reserved">internal</span> <span class="reserved">void</span> C(<span class="reserved">int</span> x) =&gt; Console.WriteLine(<span class="string">"C "</span> + x);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span> m)
    {
        <span class="comment">// 作られるインスタンスは1つだけ</span>
        <span class="reserved">var</span> anonymous = <span class="reserved">new</span> <span class="type">AnonymousClass</span>();
        anonymous.m = m;

        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; a = anonymous.A;
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; b = anonymous.B;
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; c = anonymous.C;

        Invoke(a, b, c);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Invoke(<span class="reserved">params</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;[] list)
    {
        <span class="reserved">foreach</span> (var item <span class="reserved">in</span> list) item(1);
    }
}
</code></pre>

コンパイラーによって作られるインスタンスが1つで済むという意味ではこの作りはお得です。

その一方で、この作りには、キャプチャしたローカル変数の寿命が一蓮托生になるという欠点があります。
寿命を変えるべきものは、同じスコープでキャプチャしないようにしましょう。
例えば以下のようなコードを書いてしまうと、
短寿命でガベージ コレクションされて欲しい大きなデータがいつまでたっても回収されないという問題が起こります。

<pre class="source" title="寿命が一蓮托生になって困る例">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> Main()
    {
        <span class="comment">// この2つの配列の寿命は一蓮托生になる</span>
        <span class="reserved">var</span> smallData = <span class="reserved">new</span> <span class="reserved">int</span>[5];
        <span class="reserved">var</span> bigData = <span class="reserved">new</span> <span class="reserved">int</span>[10000];

        <span class="comment">// 小さいデータしか握っていないので長寿でもそこまで問題のないデリゲート</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f1 = i =&gt; smallData[i];

        <span class="comment">// 大きめのデータを握っていて、長寿だと問題の出るデリゲート</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f2 = i =&gt; bigData[i];

        <span class="comment">// f1, f2 を使う何か</span>
        f1(0);
        f2(0);

        <span class="comment">// f2 の寿命が長いと問題なので用が済み次第消す</span>
        f2 = <span class="reserved">null</span>;

        <span class="reserved">await</span> <span class="type">Task</span>.Delay(<span class="type">TimeSpan</span>.FromHours(10));

        <span class="comment">// f1 は後で使いたい</span>
        <span class="comment">// f1 が生きている限り、f2 を消しても結局 bigData は残る</span>
        f1(0);
    }
}
</code></pre>
