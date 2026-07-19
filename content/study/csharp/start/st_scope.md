---
title: "[雑記] 識別子のスコープとオブジェクトの寿命"
source_url: "https://ufcpp.net/study/csharp/start/st_scope/"
content_type: "Article"
published_at: "2016-01-14T00:00:00"
updated_at: "2023-11-15T21:24:13"
tags:
  - "Ver. 7.0"
umbraco_id: 1859
parent_id: 1190
sort_order: 17
aliases:
  - "/csharp/start/st_scope/"
---

# \[雑記\] 識別子のスコープとオブジェクトの寿命

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ローカル変数、メンバー名(メソッドなどの名前)、型名など、開発者が自由につけれる名前のことを<strong id="identifier" class="keyword">識別子</strong>(identifier)と言います。「識別」(identify)の名のとおり、一意に区別するためにつける名前なので、基本的には複数のものに同じ名前は付けれません。

ただし、識別子には有効は範囲があります。この範囲を識別子の<strong id="scope" class="keyword">スコープ</strong>(scope)と言い、スコープ内では識別子名は一意でなければならず、逆に、スコープが違えば、別のものに同じ名前を付けることができます。

また、スコープと関連して、以下のようなものがあります。

- スコープ: 別のものに同じ名前を付けられない範囲
  - 基本的には、その識別子を囲うブロック内がスコープです
- 変数に格納したオブジェクトの寿命
  - 基本的に、変数のスコープを外れれば、そのオブジェクトは不要([GC](../resource/rm_gc.md#garbage-collection)の対象)になります
  - ただし、[ラムダ式](../functional/sp_delegate.md#anonymous)や[イテレーター](../data/sp2_iterator.md#complied)、[非同期メソッド](../async/sp5_awaitable.md)など、オブジェクトの寿命を延ばしてしまう構文がいくつかあります
- 変数を使える範囲:
  - スコープ内で、かつ、変数宣言より下でだけ変数を使えます
  - さらに、変数に格納した値を読み出すためには、確実に初期化してからでなければいけません

本稿では、これらについて説明して行きます。

## <a id="sec-generated-title-2"></a> <a id="scope"></a>識別子のスコープ

C#の識別子のスコープは、原則として、<em>その識別子の定義場所を囲むブロック内</em>です。例えば以下のようになります。

![識別子のスコープ = 囲むブロック内](../../../../assets/media/1059/scope1.png)

この範囲では、基本的に同じ名前は使えないということになります。

### <a id="sec-generated-title-3"></a> <a id="nested-block"></a>入れ子のブロック

スコープの範囲は、ブロックが入れ子になっている個所も含めます。
すなわち、以下のようなコードはコンパイル エラーになります。

<pre class="source" title="入れ子のブロックにもスコープは及ぶ">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M()
{
    <span class="reserved">int</span> x = 10;

    {
        <span class="reserved">int</span> x = 20; <span class="comment">// ここでエラー</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="type">Console</span>.WriteLine(x);
}
</code></pre>

この例では`x`という名前の変数が2つあります。1つ目の`x`(10を代入している方)のスコープはメソッド`M`全体になります。2つ目の`x`(20の方)のスコープはそれよりも1回り小さい内側のブロック内になりますが、この範囲は1つ目の`x`のスコープ内でもあります。
プログラミング言語によっては、この「入れ子のレベル違い」の同名識別子を認めているものもありますが、C#では認めません。
C#は、原則として<em>スコープ内で識別子の意味を変えない・上書かない</em>という方針をとっています。

逆に、以下のようなコードであれば、2つの`x`がそれぞれ直近のブロック内だけをスコープにしているので、エラーにはなりません。

<pre class="source" title="2つの独立したブロックは別スコープ">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M()
{
    {
        <span class="reserved">int</span> x = 10;
        <span class="type">Console</span>.WriteLine(x);
    }

    {
        <span class="comment">// 別ブロック = 別スコープ。↑のxとは完全に別物</span>
        <span class="reserved">string</span> x = <span class="string">"a"</span>;
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

もう1つ注意が必要なのは、変数の定義位置がどこであろうと、スコープは直近のブロック全体になるということです。
例えば以下のコードを見てください。

<pre class="source" title="スコープはあくまで直近のブロック全体">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M3()
{
    {
        <span class="comment">// 下で定義されている string の方の x と名前被り</span>
        <span class="reserved">int</span> x = 20; <span class="comment">// コンパイル エラー</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="comment">// string の方の x はここから下でしか使えない</span>
    <span class="comment">// にも関わらず、x のスコープはメソッド内全体</span>
    <span class="reserved">string</span> x = <span class="string">"a"</span>;
    <span class="type">Console</span>.WriteLine(x);
}
</code></pre>

2つ目の`x`(`string`の方)は下の方で定義されていますが、これのスコープはブロックの先頭からになります。
その結果、1つ目の`x`は「スコープ被り」で、同名が許されず、コンパイル エラーになります。

### <a id="sec-generated-title-4"></a> <a id="member-local"></a>例外1: メンバーとローカル変数

「入れ子のもの含めて、スコープ内では同名不可」の原則には例外もあります。
1つは、以下のように、メンバーとローカル変数には同じ名前をつけれるということです。

<pre class="source" title="メンバー名とローカル変数名は同じものを付けれる">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">int</span> x = 20;

    <span class="reserved">public</span> <span class="reserved">void</span> M()
    {
        <span class="reserved">int</span> x = 10;

        <span class="type">Console</span>.WriteLine(x);      <span class="comment">// ローカル変数の方の x = 10</span>
        <span class="type">Console</span>.WriteLine(<span class="reserved">this</span>.x); <span class="comment">// フィールドの方の x = 20</span>
    }
}
</code></pre>

この場合、ローカル変数側が優先されます。フィールドの方を使うためには`this.`を付けるのが必須になります。

### <a id="sec-generated-title-5"></a> <a id="type-member"></a>例外2: 型と名前空間

もう1つの例外は、型と名前空間です。外で定義された型の名前と同名のメンバーやローカル変数が作れます。

<pre class="source" title="型や名前空間と同じ名前のフィールド・ローカル変数">
<code><reserved></span><span class="reserved">namespace</span> Color
{
    <span class="reserved">public</span> <span class="reserved">enum</span> <span class="type">Color</span>
    {
        Green,
        Yellow,
        Red,
    }

    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
    {
        <span class="reserved">public</span> <span class="type">Color</span> Color { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <span class="reserved">public</span> <span class="reserved">void</span> M()
        {
            <span class="type">Color</span> Color = <span class="type">Color</span>.Red;
        }
    }
}
</code></pre>

この場合、どの識別子かを明確化するには、完全修飾名を使うことになります。

<pre class="source" title="完全修飾名で識別子を参照">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">namespace</span> Color
{
    <span class="reserved">public</span> <span class="reserved">enum</span> <span class="type">Color</span>
    {
        Green,
        Yellow,
        Red,
    }

    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
    {
        <span class="reserved">public</span> <span class="reserved">global</span>::Color.<span class="type">Color</span> Color { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <span class="reserved">public</span> <span class="reserved">void</span> M()
        {
            <span class="reserved">global</span>::Color.<span class="type">Color</span> Color = <span class="reserved">global</span>::Color.<span class="type">Color</span>.Red;

            <span class="type">Console</span>.WriteLine(Color);
            <span class="type">Console</span>.WriteLine(<span class="reserved">this</span>.Color);
        }
    }
}
</code></pre>

ちなみに、これは、あくまで型が外側のスコープで定義されている場合だけです。
以下のように、まったく同じスコープ内で定義する場合は、型名とメンバー名を同じにすることはできなくなります。

<pre class="source" title="同スコープ内での同名の型とメンバー定義">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">enum</span> <span class="type">Color</span>
    {
        Green,
        Yellow,
        Red,
    }

    <span class="comment">// enum の Color と同じスコープ内でプロパティの Color を作ろうとしていて</span>
    <span class="comment">// この場合はコンパイル エラーになる</span>
    <span class="reserved">public</span> <span class="type">Color</span> Color { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="parameter"></a>引数

メソッドの引数のスコープは、そのメソッド本体内全域です。ほぼ、ローカル変数と扱いは一緒です。
メソッド内で、引数と同名のローカル変数は作れません。

<pre class="source" title="引数の扱いはローカル変数と同じ">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x)
{
    <span class="reserved">int</span> x = 10; <span class="comment">// コンパイル エラー</span>
    <span class="type">Console</span>.WriteLine(x);
}
</code></pre>

ローカル変数と同じくスコープの例外として、メンバーと同じ名前を付けることができます。
極端な話、以下のように、メソッドと同名の引数を使うこともできます。

<pre class="source" title="メソッド名と同名の引数が利用可能">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> X(<span class="reserved">int</span> X)
    {
        <span class="reserved">if</span> (X &lt;= 1) <span class="reserved">return</span> 1;
        <span class="reserved">else</span> <span class="reserved">return</span> <span class="type">Sample</span>.X(X - 1);
    }
}
</code></pre>

### <a id="sec-generated-title-7"></a> <a id="loop"></a>ループ変数

`for`ステートメントや、`foreach`ステートメントの場合、ループ変数があります。ループ変数のスコープはステートメントの内側になります。

<pre class="source" title="ループ変数のスコープ">
<code><reserved></span><span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
{
    <span class="comment">// for の i のスコープはこのブロック内</span>
    <span class="type">Console</span>.WriteLine(i);
}

<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> <span class="type">Enumerable</span>.Range(0, 5))
{
    <span class="comment">// foreach の i のスコープはこのブロック内</span>
    <span class="comment">// for の方の i とは別物</span>
    <span class="type">Console</span>.WriteLine(i);
}
</code></pre>

## <a id="sec-generated-title-8"></a>変数を使える範囲

変数を使える範囲は、スコープよりもやや厳しくなります。
前節の通り、スコープは、その識別子を囲うブロック全体になりますが、
変数の場合はそのブロック全体でから使えるわけではありません。

まず、変数は、変数宣言よりも前では使えません。

<pre class="source" title="変数は、宣言より前では使えない">
<code><span class="comment">// 宣言より後なのでコンパイル エラー</span>
x = 10;

<span class="reserved">int</span> x; <span class="comment">// 変数宣言</span>

<span class="comment">// 宣言より後なので OK</span>
x = 20;
</code></pre>

また、変数に格納された値を読み出すためには、それよりも前に確実に初期化を行っている必要があります。

<pre class="source" title="読み出す前に初期化が必要">
<code>{
    <span class="reserved">int</span> x; <span class="comment">// 未初期化変数</span>

    <span class="comment">// 初期化前には読めない。コンパイル エラー</span>
    <span class="type">Console</span>.WriteLine(x);
}

{
    <span class="reserved">int</span> y; <span class="comment">// 未初期化変数</span>

    y = 10; <span class="comment">// ここで初期化</span>

    <span class="comment">// これならOK</span>
    <span class="type">Console</span>.WriteLine(y);
}
</code></pre>

C#では、変数が確実に初期化されているかどうかを結構真面目に判定しています。
例えば、以下のように、if ステートメントでは真偽両方で初期化されているかまで見ています。
(これを、「確実な代入ルール」(definite assignment rule)と呼んで、結構事細かにルールが決まっています。)

<pre class="source" title="if ステートメントの中まで追って、変数の初期化を確認">
<code>{
    <span class="reserved">int</span> x; <span class="comment">// 未初期化変数</span>

    <span class="reserved">if</span> (<span class="type">Console</span>.ReadKey().Key == <span class="type">ConsoleKey</span>.A)
    {
        x = 10;
    }

    <span class="comment">// 条件を満たさない時に x が初期化されない。コンパイル エラー</span>
    <span class="type">Console</span>.WriteLine(x);
}

{
    <span class="reserved">int</span> y; <span class="comment">// 未初期化変数</span>

    <span class="reserved">if</span> (<span class="type">Console</span>.ReadKey().Key == <span class="type">ConsoleKey</span>.A)
    {
        y = 10;
    }
    <span class="reserved">else</span>
    {
        y = 20;
    }

    <span class="comment">// これならOK</span>
    <span class="type">Console</span>.WriteLine(y);
}
</code></pre>

<!-- original-page-break -->


## <a id="sec-generated-title-9"></a> <a id="lifetime"></a>オブジェクトの寿命

オブジェクトは、誰からも参照されなくなったら[ガベージ コレクション](../resource/rm_gc.md#garbage-collection)の対象になります。この時点をもって、オブジェクトの寿命は尽きていると考えます。

この「誰かが参照している」というのは、以下のように判定します。

1. 何もしなければ識別子のスコープを抜けた時点で参照が外れたことになる
1. 明示的に別の値やnullを代入すれば、その時点で参照が外れたことになる

1つ目の制限 があるので、基本的に、識別子のスコープが、オブジェクトの寿命の最大範囲です。
例えば以下のようなコードから、変数のスコープ = オブジェクトの寿命になっていることが分かります。

<pre class="source" title="変数のスコープとオブジェクトの寿命">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> Sample()
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"Sampleが作られました"</span>);
    }
    ~Sample()
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"SampleがGCされました"</span>);
    }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M()
    {
        {
            <span class="type">Console</span>.WriteLine(<span class="string">"Scope開始"</span>);
            <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>();

            <span class="comment">// この時点ではまだ生きているので、GC しても無駄</span>
            <span class="type">GC</span>.Collect();

            <span class="type">Console</span>.WriteLine(<span class="string">"Scope終了"</span>);
        }

        <span class="comment">// この時点で s に入っていた Sample インスタンスは寿命迎えてる</span>
        <span class="comment">// GC を強制起動すると回収されるはず</span>
        <span class="type">GC</span>.Collect();
    }
}
</code></pre>

<pre class="console" title="実行結果">
<code>Scope開始
Sampleが作られました
Scope終了
SampleがGCされました
</code></pre>

### <a id="sec-generated-title-10"></a> <a id="closure"></a>ラムダ式と変数の昇格

通常、ローカル変数に格納したオブジェクトの寿命は非常に短いです。戻り値で返したりしない限り、ブロック内だけで寿命を終えます。
ただ、C#にはいくつか、ただのローカル変数を、もう少し寿命の長いものに「昇格」(elevation)させてしまう構文があります。

その1つが[匿名関数](../functional/sp_delegate.md#anonymous)です。匿名関数は、外側のローカル変数を取り込んでしまえる(補足(capture)できる)機能を持っています。この場合、取り込んだローカル変数に入っているインスタンスの寿命が延びます。

<pre class="source" title="ローカル変数の補足とオブジェクトの寿命">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Sample(<span class="reserved">int</span> value)
    {
        Value = value;
    }
    ~Sample()
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"SampleがGCされました"</span>);
    }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; M()
    {
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f;
        {
            <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>(1);
            f = () =&gt; s.Value;
            <span class="comment">// 変数 s のスコープはここまで</span>
        }

        <span class="comment">// でも、f が内部で s を参照しているので、インスタンスの寿命が延びる</span>
        <span class="comment">// 変数 s のスコープを超えて、f のスコープ内でずっと生き残る</span>
        <span class="comment">// GC 起動しても回収されず</span>
        <span class="type">GC</span>.Collect();

        <span class="reserved">return</span> f;
    }
}
</code></pre>

詳細は「[匿名デリゲートのコンパイル結果](../functional/sp2_anonymousmethod.md)」で説明していますが、匿名関数から外部のローカル変数を参照すると、実際にはクラスが自動生成されて、フィールドが作られます。すなわち、ローカル変数だったものがフィールドに昇格します。この昇格により、格納されているインスタンスの寿命が延びます。

### <a id="sec-generated-title-11"></a> <a id="for-loop-variable"></a>forステートメントのループ変数

ラムダ式の外部変数補足と合わせると、ループ変数のスコープに関して注意が必要になります。

まず、`for`ステートメントですが、これのループ変数は、全ループで1つ、同じ変数扱いになります。
例えば、以下の2つのループ(`for`ステートメントと、その下の`while`ステートメントを使ったもの)は同じ意味になります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">public static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> n)
{
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; n; i++)
    {
        <span class="type">Console</span>.WriteLine(i);
    }

    {
        <span class="reserved">int</span> i = 0;
        <span class="reserved">while</span>(i &lt; n)
        {
            <span class="type">Console</span>.WriteLine(i);
            i++;
        }
    }
}
</code></pre>

`while`に書き換えたものを見てのとおり、ループの外側に1つの変数があり、それがずっと使いまわされます。

<pre class="source" title="forのループ変数はループ全体で共有">
<code><type></span><span class="type">Action</span> a = <span class="reserved">null</span>;

<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10; i++)
{
    a += () =&gt; <span class="type">Console</span>.WriteLine(i); <span class="comment">// この i はずっと共有</span>
}
<span class="comment">// ループを抜けたときには、i の値は 10 に置き換わってる</span>

<span class="comment">// 結果、10が10回表示される</span>
a();
</code></pre>

この結果(10が10回表示される)は意図通りでしょうか。0～9までの数字が1回ずつ表示される方を期待したいところですが、残念ながらそうはなりません。「0～9まで1回ずつ」という挙動を得るためには以下のように書く必要があります。

<pre class="source" title="ループ1回1回で分けたい場合は別の変数が必要">
<code><type></span><span class="type">Action</span> a = <span class="reserved">null</span>;

<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10; i++)
{
    <span class="reserved">var</span> j = i;
    a += () =&gt; <span class="type">Console</span>.WriteLine(j); <span class="comment">// この j は1回1回別</span>
}

<span class="comment">// 結果、0～9が1回ずつ表示される</span>
a();
</code></pre>

### <a id="sec-generated-title-12"></a> <a id="foreach-loop-variable"></a>foreachステートメントのループ変数

<h5 class="version version5">Ver. 5.0</h5>

同様の件について、`foreach`ステートメントでは、C# 5.0を境に仕様変更がありました。

C# 4.0以前では、`for`ステートメントと同じで、ループ変数がループ全体で共有されていました。
一方、C# 5.0以降では、ループ1回1回別扱いされるように変更されています。
すなわち、`while`を使って書き直すなら以下のようになります。

<pre class="source" title="foreachのループ変数は4.0以前と5.0以降で挙動が異なる">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; list)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> list)
    {
        <span class="type">Console</span>.WriteLine(i);
    }

    {
        <span class="comment">// C# 4.0 以前</span>
        <span class="reserved">var</span> e = list.GetEnumerator();
        <span class="reserved">using</span> (e <span class="reserved">as</span> <span class="type">IDisposable</span>)
        {
            <span class="reserved">int</span> i; <span class="comment">// ループの外</span>
            <span class="reserved">while</span> (e.MoveNext())
            {
                i = e.Current;
                <span class="type">Console</span>.WriteLine(i);
            }
        }
    }

    {
        <span class="comment">// C# 5.0 以降</span>
        <span class="reserved">var</span> e = list.GetEnumerator();
        <span class="reserved">using</span> (e <span class="reserved">as</span> <span class="type">IDisposable</span>)
        {
            <span class="reserved">while</span> (e.MoveNext())
            {
                <span class="reserved">var</span> i = e.Current; <span class="comment">// ループの中</span>
                <span class="type">Console</span>.WriteLine(i);
            }
        }
    }
}
</code></pre>

当然、以下のように、匿名関数で変数を取り込んだ際の挙動が変わります。

<pre class="source" title="ラムダ式で変数補足した場合の挙動">
<code><type></span><span class="type">Action</span> a = <span class="reserved">null</span>;

<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> <span class="type">Enumerable</span>.Range(0, 10))
{
    <span class="comment">// C# 4.0 以前: この i はずっと共有</span>
    <span class="comment">// C# 5.0 以降: この i は1回1回別</span>
    a += () =&gt; <span class="type">Console</span>.WriteLine(i);
}

<span class="comment">// C# 4.0 以前: 9が10回表示される</span>
<span class="comment">// C# 5.0 以降: 0～9が1回ずつ表示される</span>
a();
</code></pre>

便利になる方向への変更なので概ね問題は起こしませんが、もしも、C# 4.0以前を使う必要がある場合には注意が必要です。
最新のコンパイラーと同じ感覚で上記のようなコードを書くと、C# 4.0以前のコンパイラーではバグになったりします。

### <a id="sec-generated-title-13"></a> <a id="iterator"></a>イテレーターと非同期メソッド

ローカル変数がフィールドに昇格してしまうものがあと2つあります。[イテレーター](../data/sp2_iterator.md#complied)と[非同期メソッド](../async/sp5_awaitable.md)です。

これらは、結構大々的なクラスの自動生成を行っていて、ローカル変数がフィールドに格上げされます。
例えば、以下のようなコードを実行すると、`Sample`のインスタンスはプログラム終了直前まで回収されません。

<pre class="source" title="イテレーターと非同期メソッドでのローカル変数の昇格">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Sample</span>
{
    ~Sample()
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"SampleがGCされました"</span>);
    }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M()
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> Iterator()) ;
        AsyncMethod().Wait();
    }

    <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Iterator()
    {
        <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>();
        <span class="reserved">yield</span> <span class="reserved">return</span> 1;
        <span class="type">Console</span>.WriteLine(<span class="string">"1"</span>);

        <span class="comment">// s はずっと生き残ってる。回収されない</span>
        <span class="type">GC</span>.Collect();

        <span class="reserved">yield</span> <span class="reserved">return</span> 2;
        <span class="type">Console</span>.WriteLine(<span class="string">"2"</span>);

        <span class="comment">// 同上。回収されない</span>
        <span class="type">GC</span>.Collect();

        <span class="reserved">yield</span> <span class="reserved">return</span> 3;
        <span class="type">Console</span>.WriteLine(<span class="string">"3"</span>);
    }

    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> AsyncMethod()
    {
        <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>();
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
        <span class="type">Console</span>.WriteLine(<span class="string">"1"</span>);

        <span class="comment">// s はずっと生き残ってる。回収されない</span>
        <span class="type">GC</span>.Collect();

        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
        <span class="type">Console</span>.WriteLine(<span class="string">"2"</span>);

        <span class="comment">// 同上。回収されない</span>
        <span class="type">GC</span>.Collect();

        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1);
        <span class="type">Console</span>.WriteLine(<span class="string">"3"</span>);
    }
}
</code></pre>


<pre class="console" title="実行結果">
<code>1
2
3
1
2
3
SampleがGCされました
SampleがGCされました
</code></pre>

<h5 class="version version6">Ver. 6</h5>

C# 5.0以前の場合、すべてのローカル変数が問答無用で軒並みフィールドに昇格していました。
元々、昇格が必要な理由は`yield return`や`await`をまたいで使うためです。
にもかかわらず、たとえ`yield return`や`await`をまたいでなくてもすべて昇格します。
これは、デバッグ実行時に変数の中身を覗けるようにするためです。

しかし、デバッグ実行のためというなら、デバッグ ビルドの際だけでいいはずです。
そこで、C# 6ではそう変更しました。リリース ビルドすると、`yield return`や`await`をまたがないものは通常のローカル変数にとどまります。
昇格が起きない分、オブジェクトの寿命が短くなります。
例えば、先ほどのコードですが、まったく同じものを、C# 6以降のコンパイラーを使って、リリース設定でコンパイルすると、結果は以下のように変わります。

<pre class="console" title="実行結果(C# 6以降、リリース設定)">
<code>1
2
SampleがGCされました
3
1
SampleがGCされました
2
3
</code></pre>

<!-- original-page-break -->

## <a id="sec-generated-title-14"></a> <a id="csharp7"></a>C# 7での新しいスコープ ルール

<h5 class="version version7">Ver. 7</h5>

[C# 7](../cheatsheet/ap_ver7.md)では、新機能の導入に伴って、それ以前にはなかったスコープ関連のルールが発生しています。

- [is 演算子の拡張](../datatype/typeswitch.md#is)と[出力変数宣言](../resource/sp_ref.md#out-var)が入ったので、式の途中で変数宣言できるようになりました
- [ローカル関数](../structured/st_function.md#sec-local)が入りましたが、ローカル変数とはちょっと違うルールになっています

<h5 class="version version7">Ver. 7.3</h5>

ちなみに、C# 7.0の時点では、「式中での変数宣言」が使えるのは、関数本体(メソッドなどの`{}`の中や`=>`の後ろの部分)の中の式だけでした。
また、[クエリ式](../data/sp3_linq.md#query)内では変数宣言できませんでした。

これに対して、C# 7.3からはこの制限がなくなり、
クエリ式や[コンストラクター初期化子](../oop/oo_construct.md#initializer)などの中でも変数宣言できるようになりました。

### <a id="sec-generated-title-15"></a> <a id="declaration-expressions"></a>式中での変数宣言

C# 6以前であれば、変数の宣言は宣言ステートメントでしかできませんでした。
そして、その宣言ステートメントを囲うブロックが、変数のスコープになります。

ちなみに、ブロックを持たない宣言ステートメントは書けません。
「ブロックを持たない」というのは、例えば、if ステートメントや foreach ステートメント直下です。
以下のようなコードはコンパイル エラーになります。

<pre class="source" title="ifやforeach直下には変数宣言を書けない">
<code><span class="reserved">if</span> (<span class="reserved">true</span>)
    <span class="reserved">int</span> x = 10; <span class="comment">// コンパイル エラー</span>

<span class="reserved">if</span> (<span class="reserved">true</span>)
{
    <span class="reserved">int</span> x = 10; <span class="comment">// これなら OK</span>
}

<span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> <span class="reserved">new</span>[] { 1 })
    <span class="reserved">int</span> x = 10; <span class="comment">// コンパイル エラー</span>

<span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> <span class="reserved">new</span>[] { 1 })
{
    <span class="reserved">int</span> x = 10; <span class="comment">// これなら OK</span>
}
</code></pre>

このifやforeach直下の部分を、構文上は埋め込みステートメント(embedded statement)と呼びます。
つまり、変数宣言ステートメントは、埋め込みステートメントに含まれていません。

ということで、C# 6までは「変数のスコープと言えばそれを囲うブロック内」というシンプルなルールで説明が付きました。

ところが、C# 7で導入された[is 演算子の拡張](../datatype/typeswitch.md#is)と[出力変数宣言]では、式の中で変数宣言ができます。
式は割かしどこにでも書けるものなので、実質的に、ほぼどこででも変数宣言できるようになりました。

<pre class="source" title="宣言をどこにでも書けるようになった例">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">object</span> obj)
{
    <span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">int</span> x1) <span class="comment">// 条件式内</span>
        ;

    <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> obj <span class="reserved">is</span> <span class="reserved">int</span> x2 ? <span class="string">"a"</span> : <span class="string">"b"</span>) <span class="comment">// foreach の () 内</span>
        ;

    <span class="reserved">for</span> (<span class="reserved">var</span> n = 0; obj <span class="reserved">is</span> <span class="reserved">int</span> x3 ? n &lt; x3 : <span class="reserved">false</span>; n++) <span class="comment">// for の () 内</span>
        ;

    <span class="reserved">if</span> (<span class="reserved">true</span>)
        <span class="type">Console</span>.WriteLine(obj <span class="reserved">is</span> <span class="reserved">int</span> x4 ? 1 : 2); <span class="comment">// 埋め込みステートメント内</span>

    <span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> <span class="string">"a"</span>)
        <span class="type">Console</span>.WriteLine(obj <span class="reserved">is</span> <span class="reserved">int</span> x5 ? 1 : 2); <span class="comment">// 埋め込みステートメント内</span>
}
</code></pre>

そうなると問題は、式中で宣言した変数のスコープがどうなるかです。
これには、仕様を決める段階で紆余曲折あったんですが、「式を囲うブロック、埋め込みステートメント、while、for、foreach、using、 case内」ということになりました。

<pre class="source" title="">
<code><span class="reserved">if</span> (<span class="reserved">true</span>)
{
    <span class="type">Console</span>.WriteLine(obj <span class="reserved">is</span> <span class="reserved">int</span> x ? 1 : 2); <span class="comment">// もちろん、ブロック内がスコープ</span>
    x = 1; <span class="comment">// これは OK</span>
}

<span class="reserved">if</span> (<span class="reserved">true</span>)
    <span class="type">Console</span>.WriteLine(obj <span class="reserved">is</span> <span class="reserved">int</span> x ? 1 : 2); <span class="comment">// 埋め込みステートメント内がスコープ</span>

<span class="reserved">foreach</span> (<span class="reserved">var</span> n <span class="reserved">in</span> obj <span class="reserved">is</span> <span class="reserved">int</span> x ? <span class="string">"a"</span> : <span class="string">"b"</span>) <span class="comment">// foreach 内がスコープ</span>
    ;

<span class="reserved">for</span> (<span class="reserved">var</span> n = 0; obj <span class="reserved">is</span> <span class="reserved">int</span> x ? n &lt; x : <span class="reserved">false</span>; n++) <span class="comment">// for 内がスコープ</span>
    ;

<span class="reserved">while</span> (obj <span class="reserved">is</span> <span class="reserved">int</span> x) <span class="comment">// while 内がスコープ</span>
{
    obj = <span class="string">""</span>;
}

<span class="reserved">using</span> (obj <span class="reserved">is</span> <span class="type">IDisposable</span> x ? x : <span class="reserved">null</span>) <span class="comment">// using 内がスコープ</span>
    ;

<span class="comment">// どの x ももうスコープ外。コンパイル エラー</span>
<span class="error">x</span> = 10;
</code></pre>

特に、forステートメントの更新式の部分で宣言された変数のスコープは、更新式内だけになります。
(ループ本体の中からすら参照できない。)

<pre class="source" title="for ステートメントの更新式のスコープ">
<code><span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 100; i += obj <span class="reserved">is</span> <span class="reserved">int</span> x ? x : 1) <span class="comment">// この x はこの式内でだけ使える</span>
{
    <span class="reserved">var</span> x = <span class="string">"別の値"</span>; <span class="comment">// OK。更新式内の x とは別物</span>
}
</code></pre>

また、switch-case では以下のような書き方もできます。

<pre class="source" title="caseごとにスコープが分かれる">
<code><span class="reserved">switch</span> (obj)
{
    <span class="reserved">case</span> <span class="reserved">int</span> x: <span class="reserved">return</span> x;
    <span class="reserved">case</span> <span class="reserved">string</span> x: <span class="reserved">return</span> x.Length; <span class="comment">// int x の方とは別になる</span>
    <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>();
}
</code></pre>

一方で、if ステートメントの条件式ではスコープが区切られません。そのifを囲うブロックがスコープになります。

<pre class="source" title="if, while はスコープを区切らない">
<code><span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">int</span> x1) <span class="comment">// 条件式内</span>
{
}
<span class="reserved">else</span>
{
    x1 = 10; <span class="comment">// ここも x1 のスコープ</span>
}

<span class="type">Console</span>.WriteLine(x1); <span class="comment">// ここも x1 のスコープ</span>
</code></pre>

これは、いわゆる「early return」(`if (条件) { 長い処理 }` の代わりに、`if (!条件) return;` で処理を打ち切ってしまうパターン)で変数宣言をしたいという要件が多いからだそうです。

<pre class="source" title="early return と if の条件式中での変数宣言">
<code><span class="reserved">void</span> M(<span class="reserved">string</span> s)
{
    <span class="reserved">if</span> (!<span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> var x)) <span class="reserved">return</span>;

    <span class="comment">// x を使った長い処理</span>
}
</code></pre>

### <a id="sec-generated-title-16"></a> <a id="lambda"></a>ラムダ式

[ラムダ式](../functional/sp3_lambda.md)では、ブロックを使った `() => { }` というようなものと、
`=>` に続けて式を直接書く `() => x` というようなものの2パターンの記法が使えます。
後者であっても、この中で宣言した変数のスコープはラムダ式内に限られます。
(要するに、`() => x` みたいなのの`x`の部分は、前述の「埋め込みステートメント」と同じ扱いになっています。)

<pre class="source" title="ラムダ式中の変数宣言">
<code><span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; f = s =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> var x) ? x : -1;
f(<span class="string">"123"</span>);
<span class="type">Console</span>.WriteLine(<span class="error">x</span>); <span class="comment">// ここで x は使えない</span>
</code></pre>

### <a id="sec-generated-title-17"></a> <a id="is-operator"></a>余談: is 演算子で新しい変数を導入

Swift など、他のプログラミング言語の一部では、(C#風に書くと)以下のような構文を持っているものがあります。

<pre class="source" title="is 演算子">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived1</span> : <span class="type">Base</span> { <span class="reserved">public</span> <span class="reserved">int</span> Id =&gt; 1; }
<span class="reserved">class</span> <span class="type">Derived2</span> : <span class="type">Base</span> { <span class="reserved">public</span> <span class="reserved">string</span> Name =&gt; <span class="string">"2"</span>; }

<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Base</span> x)
    {
        <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="type">Derived1</span>)
        {
            <span class="comment">// この中では、x を Derived1 として扱える</span>
            <span class="type">Console</span>.WriteLine(x.Id);
        }
        <span class="reserved">else</span> <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="type">Derived2</span>)
        {
            <span class="comment">// この中では、x を Derived2 として扱える</span>
            <span class="type">Console</span>.WriteLine(x.Name);
        }
    }
}
</code></pre>

is演算子の拡張は、C# 7でもこういう「型による分岐」機能がほしいということで入った機能です。
しかし、Swiftのような構文だと、「スコープ内で識別子の意味を変えない・上書かない」という原則に反します。
`x`は最初に`Base`型として定義した以上、ずっと`Base`型のままにしたいということです。

結局、is演算子の拡張は以下のように、式の中で新しい変数を導入する構文になっています。

<pre class="source" title="C# 7のis演算子">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Base</span> x)
{
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="type">Derived1</span> d1)
    {
        <span class="comment">// x の型が Derived1 だった場合だけ、キャスト結果が d1 に入る</span>
        <span class="type">Console</span>.WriteLine(d1.Id);
    }
    <span class="reserved">else</span> <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="type">Derived2</span> d2)
    {
        <span class="comment">// x の型が Derived2 だった場合だけ、キャスト結果が d2 に入る</span>
        <span class="type">Console</span>.WriteLine(d2.Name);
    }
}
</code></pre>

### <a id="sec-generated-title-18"></a> <a id="local-functions"></a>ローカル関数を使える範囲

[ローカル関数](../structured/st_function.md#sec-local)はどう扱うべきでしょうか。
ローカル変数のようなものだと考えると、宣言より前では使えないはずです。
一方で、メソッドのようなものだと考えると、通常、メソッドは宣言よりも前で使えます。

<pre class="source" title="ローカル関数はローカル変数的であるべきか、メソッド的であるべきか">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ローカル関数は、こういうローカル変数的な扱いすべき？</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f = x =&gt; x * x;

        <span class="comment">// もしローカル変数的に扱うなら、f はこの後ろでしか使えない</span>
        <span class="reserved">var</span> y = f(2);

        <span class="comment">// それとも、メソッドと同じような扱いにすべき？</span>
        <span class="comment">// メソッドなら、宣言よりも前でも使える</span>
        <span class="reserved">var</span> z = M(2);
    }

    <span class="comment">// メソッドであれば、宣言が後ろにあってもいい</span>
    <span class="reserved">static</span> <span class="reserved">int</span> M(<span class="reserved">int</span> x) =&gt; x * x;
}
</code></pre>

これは結局、後者が選ばれました。すなわち、メソッド的に、宣言よりも前で使えます。

<pre class="source" title="ローカル関数は宣言より前で使える">
<code><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="comment">// ローカル関数は宣言より前で使える</span>
    <span class="reserved">var</span> y = f(2);

    <span class="reserved">int</span> f(<span class="reserved">int</span> x) =&gt; x * x;
}
</code></pre>

もう1つ、ローカル関数が絡むと、「確実な代入ルール」も少々複雑です。
ローカル関数が周りのローカル変数をキャプチャする際、
その変数は、初めてローカル関数を呼び出すまでに初期化すればよいということになっています。

<pre class="source" title="ローカル関数を呼ぶまでに初期化すればOK">
<code><span class="reserved">static</span> <span class="reserved">void</span> SuccessfulSample()
{
    <span class="reserved">int</span> a; <span class="comment">// 未初期化</span>
    <span class="reserved">int</span> f(<span class="reserved">int</span> x) =&gt; a * x; <span class="comment">// (この時点で)未初期化変数 a 参照</span>
    a = 10; <span class="comment">// ここで初期化</span>
    <span class="reserved">var</span> y = f(2); <span class="comment">// OK</span>
}

<span class="reserved">static</span> <span class="reserved">void</span> ErroneousSample()
{
    <span class="reserved">int</span> a; <span class="comment">// 未初期化</span>
    <span class="reserved">int</span> f(<span class="reserved">int</span> x) =&gt; a * x; <span class="comment">// 未初期化変数 a 参照</span>
    <span class="comment">// a を初期化しない！</span>
    <span class="reserved">var</span> y = f(2); <span class="comment">// コンパイル エラー</span>
}
</code></pre>

### <a id="sec-generated-title-19"></a> <a id="query-expression"></a>クエリ式

<h5 class="version version7">Ver. 7.3</h5>

C# 7.3までは、クエリ式中では式中での変数宣言ができませんでした。
(変数のスコープをどうするかがちょっと悩ましく、7.0時点では「先送り」していました。)
C# 7.3で、これが許されるようになりました。

<pre class="source" title="クエリ式中での変数宣言">
<code><span class="reserved">var</span> q =
    <span class="reserved">from</span> s <span class="reserved">in</span> <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"abc"</span>, <span class="string">"112"</span>, <span class="string">"132"</span>, <span class="string">"451"</span>, <span class="reserved">null</span> }
    <span class="reserved">where</span> s <span class="reserved">is</span> <span class="reserved">string</span> <em>x</em> &amp;&amp; x.Length &gt; 1
    <span class="reserved">where</span> <span class="reserved">int</span>.TryParse(s, <span class="reserved">out var</span> <em>x</em>) &amp;&amp; (x % 3) == 0
    <span class="reserved">select</span> s;
</code></pre>

ちなみに、この場合、変数のスコープは「句の中のみ」に限られます
(`where`とか`select`とかによってスコープが区切られます)。
上記の例の場合、1つ目の`where`中の`x`と、2つ目の`where`中の`x`はそれぞれ別変数になります。

これは、クエリ式が実際には以下のようなメソッド チェーンに展開されるためです。

<pre class="source" title="クエリ式のメソッド チェーンへの展開">
<code><span class="reserved">var</span> q =
    <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"abc"</span>, <span class="string">"112"</span>, <span class="string">"132"</span>, <span class="string">"451"</span>, <span class="reserved">null</span> }
    .Where(s =&gt; s <span class="reserved">is</span> <span class="reserved">string</span> <em>x</em> &amp;&amp; x.Length &gt; 1)
    .Where(s =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out var</span> <em>x</em>) &amp;&amp; (x % 3) == 0);
</code></pre>

前述の通り、ラムダ式内で変数宣言した場合、その変数のスコープはラムダ式内に限られます。
クエリ式は句ごとに1つのラムダ式が作られるので、それとの整合性を取った結果が「句ごとに別スコープ」です。
句をまたいだ変数を宣言したい場合は[`let`句](../data/sp3_stdquery.md#let)を使ってください。

### <a id="sec-generated-title-20"></a> <a id="initializer"></a>コンストラクター初期子、フィールド初期化子、プロパティ初期化子

<h5 class="version version7">Ver. 7.3</h5>

ラムダ式同様、スコープをどうするか悩ましくて保留になっていたものに初期化子があります。
C# 7.3で、以下のように、初期化子内でも変数宣言できるようになりました。

<pre class="source" title="初期化子内での変数宣言">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> Derived(<span class="reserved">string</span> s) : <span class="reserved">this</span>(<span class="reserved">int</span>.TryParse(s, <span class="reserved">out var</span> <em>x</em>) ? x : -1)
    {
        <span class="comment">// コンストラクター初期化子中で宣言した x は、コンストラクター本体内で利用可能。</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="reserved">public</span> Derived(<span class="reserved">int</span> a) : <span class="reserved">base</span>(<span class="reserved">out var</span> <em>x</em>)
    {
        <span class="comment">// base の場合でも同様。</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="comment">// フィールド初期化子、プロパティ初期化子中で宣言した x は、その初期化子内でのみ有効。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Field = <span class="reserved">int</span>.TryParse(<span class="string">"123"</span>, <span class="reserved">out var</span> <em>x</em>) ? x : -1;
    <span class="reserved">public</span> <span class="reserved">int</span> Property{ <span class="reserved">get</span>; <span class="reserved">set</span>; } = <span class="reserved">int</span>.TryParse(<span class="string">"123"</span>, <span class="reserved">out var</span> <em>x</em>) ? x : -1;
}
</code></pre>

ちなみに、コンストラクター初期化子内で宣言した変数のスコープはそのコンストラクター全体、
フィールド初期化子・プロパティ初期化子中のものはその初期化子の中限定です。
