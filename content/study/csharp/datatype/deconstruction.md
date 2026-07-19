---
title: "複合型の分解"
source_url: "https://ufcpp.net/study/csharp/datatype/deconstruction/"
content_type: "Article"
published_at: "2016-08-22T00:00:00"
updated_at: "2021-09-20T15:39:50"
tags: []
umbraco_id: 1944
parent_id: 1940
sort_order: 1
aliases:
  - "/csharp/data/deconstruction"
  - "/csharp/data/deconstruction/"
  - "/csharp/datatype/deconstruction/"
  - "/study/csharp/data/deconstruction"
  - "/study/csharp/data/deconstruction/"
---

# 複合型の分解

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<h5 class="version version7">Ver. 7</h5>

[タプル](tuples.md#key-tuple)から値を取り出す際には、メンバーを直接、それぞれバラバラに受け取りたくなることがあります。

「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
メンバー名だけ見ればその型が何を意味するか分かるからこそ型に名前が付かないわけです。
このとき、その型を受け取る変数にも、よい名前が浮かばなくなるはずです。

そこでC# 7では、タプルと同時に、分解(deconstruction)のための構文が追加されました。

## <a id="sec-generated-title-2"></a> <a id="deconstruction"></a>分解

以下のような、整数列の個数(count)と和(sum)を同時に計算するメソッドがあったとします。
「[名前のない複合型](../structured/st_anonymoustype.md)」で説明したように、
戻り値の型として「個数と和」みたいな名前(`CountAndSum`とか)しか思い浮かばないようなものです。

<pre class="source" title="個数と和を返すメソッド">
<code><span class="reserved">static</span> (<span class="reserved">int</span> count, <span class="reserved">int</span> sum) Tally(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="reserved">var</span> count = 0;
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
    {
        sum += x;
        count++;
    }

    <span class="reserved">return</span> (count, sum);
}
</code></pre>

そうなると、この結果を受け取る変数名も、「個数と和」以上の名前はつかないでしょう。
通常、ローカル変数であれば適当な名前でもそこまで問題ではないので、
`x`とか`y`とか、本当に意味がない名前を付けることになると思います。

<pre class="source" title="個数と和の受け取り">
<code><span class="reserved">var</span> x = Tally(<span class="reserved">new</span>[] { 1, 2, 3, 4, 5 });
<span class="type">Console</span>.WriteLine(x.count);
<span class="type">Console</span>.WriteLine(x.sum);
</code></pre>

実際にほしい名前は`count`と`sum`だけです。
であれば、最初から`count`変数と`sum`変数に分解して受け取りたいと思うでしょう。
要するに、以下のようなことを1行で書ける構文がほしいです。

<pre class="source" title="タプルの分解">
<code><comment></span><span class="comment">// この3行に相当する構文がほしい</span>
<span class="reserved">var</span> x = Tally(<span class="reserved">new</span>[] { 1, 2, 3, 4, 5 });
<span class="reserved">var</span> count = x.count;
<span class="reserved">var</span> sum = x.sum;
<span class="comment">// 以後、もう x は使わない</span>

<span class="type">Console</span>.WriteLine(count);
<span class="type">Console</span>.WriteLine(sum);
</code></pre>

タプルのような名前の決まらない型は、この例のように分解して使うのが前提と言えます。

そこで、C# 7では、タプルと一緒に、以下のような分解のための構文を追加しました。

<pre class="source" title="分解代入構文">
<code>(<span class="reserved">var</span> count, <span class="reserved">var</span> sum) = Tally(<span class="reserved">new</span>[] { 1, 2, 3, 4, 5 });
<span class="type">Console</span>.WriteLine(count);
<span class="type">Console</span>.WriteLine(sum);
</code></pre>

ちなみに、この分解構文は、タプルか、後述する`Deconstruct`メソッドを持つ任意の型に対して使えます。

### <a id="sec-generated-title-3"></a> <a id="deconstruction-declaration"></a>分解宣言

以下のような書き方で、分解と同時に変数を宣言できます。
これを分解宣言(deconstruction declaration)と言います。

<pre class="source" title="分解宣言">
<code><span class="comment">// count, sum を宣言しつつ、タプルを分解</span>
(<span class="reserved">int</span> count, <span class="reserved">int</span> sum) = Tally(items);

<span class="comment">// ↓こう書くとタプル型の変数の宣言</span>
<span class="comment">// (int count, int sum) t = Tally(items);</span>
</code></pre>

この例の後半のコメントのように、分解宣言はタプルの型宣言の書き方によく似ています。
ただ、タプルの型宣言と違って、型推論の`var`が使えます。

<pre class="source" title="var での型推論付きの分解宣言">
<code><span class="comment">// 型推論で count, sum を宣言しつつ、タプルを分解</span>
(<span class="reserved">var</span> count, <span class="reserved">var</span> sum) = Tally(items);

<span class="comment">// ↓タプルだと var は使えない。これはコンパイル エラー</span>
<span class="comment">// (var count, var sum) t = Tally(items);</span>
</code></pre>

このとき、部分的に型推論(`var`)を使うこともできます。

<pre class="source" title="部分的に var を使う">
<code><span class="comment">// 部分的に var を使う</span>
(<span class="reserved">var</span> count, <span class="reserved">long</span> sum) = Tally(items);
</code></pre>

一方で、宣言したいすべての変数を型推論する場合であれば、先頭に1つだけ `var` キーワードを書く以下のような書き方もできます。

<pre class="source" title="var + 変数リスト">
<code><span class="comment">// 「var + 変数リスト」でタプルを分解</span>
<span class="reserved">var</span> (count, sum) = Tally(items);
</code></pre>

この書き方は、`foreach`、`for`などでの変数宣言でも使えます。

<pre class="source" title="foreachやforの中で分解宣言">
<code>(<span class="reserved">int</span> x, <span class="reserved">int</span> y)[] array = <span class="reserved">new</span>[] { (1, 2), (3, 4) };

<span class="reserved">foreach</span> (<span class="reserved">var</span> (x, y) <span class="reserved">in</span> array)
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);
}

<span class="reserved">for</span> ((<span class="reserved">int</span> i, <span class="reserved">int</span> j) = (0, 0); i &lt; 10; i++, j--)
{
    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{i}<span class="string">, </span>{j}<span class="string">"</span>);
}
</code></pre>

(仕様書状はクエリ式の`let`、`from` でも使えることになっているものの、プレビュー版である現在は未実装。)

### <a id="sec-generated-title-4"></a> <a id="deconstruction-assignment"></a>分解代入

既存の変数を使って分解することもできます。
こちらは分解代入(deconstruction assignment)といいます。

<pre class="source" title="分解代入">
<code><span class="reserved">int</span> x, y;

<span class="comment">// 既存の変数を使って分解</span>
(x, y) = Tally(items);
</code></pre>

文法説明のために簡素化したものとはいえ、この例では分解宣言で十分で、
再代入(既存の変数`x`、`y`の書き換え)の必要性があまりありません。
実際は、以下の例のように、ループで書き換えたりすることになるでしょう。

<pre class="source" title="分解代入で変数を書き換え">
<code><span class="reserved">var</span> x = 1.0;
<span class="reserved">var</span> y = 5.0;

<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 100; i++)
{
    (x, y) = ((x + y) / 2, <span class="type">Math</span>.Sqrt(x * y));
}
</code></pre>

分解代入の左辺には、書き換え可能なものであれば何でも書けます。
例えば、配列アクセスや参照戻り値などを分解代入の左辺に書けます。

<pre class="source" title="配列アクセスや参照戻り値を使って分解代入">
<code><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> DeconstractionAssingment()
{
    <span class="reserved">var</span> a = <span class="reserved">new</span>[] { 1, 2 };

    <span class="comment">// 配列アクセス</span>
    <span class="reserved">var</span> b = <span class="reserved">new</span> <span class="reserved">int</span>[a.Length];
    (b[1], b[0]) = (a[0], a[1]);

    <span class="comment">// 参照戻り値</span>
    <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="reserved">int</span>[a.Length];
    (Mod(c, 5), Mod(c, 8)) = (a[0], a[1]);

    <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, b));
    <span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, c));
}

<span class="reserved">static</span> <span class="reserved">ref</span> <span class="type">T</span> Mod&lt;<span class="type">T</span>&gt;(<span class="type">T</span>[] array, <span class="reserved">int</span> index) =&gt; <span class="reserved">ref</span> array[index % array.Length];
</code></pre>

フィールドに対しても使えるので、
例えば以下のように、コンストラクターを記述を簡素にできたりもします。

<pre class="source" title="分解代入を使ったコンストラクターの簡素化の例">
<code><span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;
    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (X, Y);
}
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="deconstruction-expression"></a>タプル構築と分解の混在

タプルを作る構文と分解代入の構文は似ているわけですが、これらは、以下のようにつなげて書くこともできます。

<pre class="source" title="分解、かつ、タプル構築">
<code><span class="reserved">int</span> x, y;
<span class="reserved">var</span> t = (x, y) = (1, 2);
</code></pre>

これは、以下のように、分解後に改めてタプルを作るのと同じ意味になります。

<pre class="source" title="分解 → タプル構築">
<code><span class="reserved">int</span> x, y;
(x, y) = (1, 2); <span class="comment">// 分解代入</span>
<span class="reserved">var</span> t = (x, y);  <span class="comment">// 改めてタプルを構築</span>
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="mixed-deconstruction"></a>分解宣言と分解代入の混在

<h5 class="version version10">Ver. 10</h5>

C# 10.0 では以下のように、分解代入と分解宣言の混在もできるようになりました。

<pre class="source" title="分解宣言と分解代入の混在">
<code><span class="reserved">int</span> x;
(x, <em><span class="reserved">var</span> u</em>) = (1, 2);
</code></pre>

ただし、式の途中に分解宣言 (var 付きの宣言) が来るようなコードは C# 10.0 でも書けません。

<pre class="source" title="ただし、分解宣言は式の途中には書けない">
<code><span class="reserved">int</span> x, y;
(x, <span class="reserved">var</span> u) = (<span class="error"><span class="reserved">var</span> v</span>, y) = (1, 2);
</code></pre>

## <a id="sec-generated-title-7"></a> <a id="conversion"></a>分解時の型変換

分解時には、[タプル間の型変換](tuples.md#conversion)と同じルールで暗黙の型変換が働きます。
すなわち、宣言位置で分解されます(メンバー名は見ない)し、メンバーごとに暗黙的型変換が効くなら分解でも暗黙的型変換が効きます。

<pre class="source" title="分解時の型変換">
<code><span class="comment">// Tally の戻り値は (count, sum) の順</span>
<span class="reserved">var</span> t = Tally(<span class="reserved">new</span>[] { 1, 2, 3, 4, 5 });

<span class="comment">// sum = t.count, count = t.sum の意味になるので注意が必要</span>
(<span class="reserved">int</span> sum, <span class="reserved">int</span> count) = t;
<span class="type">Console</span>.WriteLine(sum);   <span class="comment">// 5</span>
<span class="type">Console</span>.WriteLine(count); <span class="comment">// 15</span>

<span class="comment">// int → object も int → long も暗黙的に変換可能</span>
<span class="comment">// なので、分解もでもこの変換が暗黙的に可能</span>
(<span class="reserved">object</span> x, <span class="reserved">long</span> y) = t;
</code></pre>

## <a id="sec-generated-title-8"></a> <a id="arbitrary-types"></a>任意の型を分解

C#の言語機能としてのタプルの他にも、
タプルに類する型はあります。
すなわち、意味のある変数が作れず、分解して使う前提の型です。

代表例は`KeyValuePair`構造体(`System.Collections.Generic`名前空間)でしょう。
`key`と`value`という変数で分解して受け取りたいです。

また、C#の構文としてタプルが導入される以前に使っていた型ですが、
`Tuple`クラス(`System`名前空間)というものがあります。
メンバー名まで紛失してしまうので使い勝手はよくありませんが、
「型名がうまく付けられない時に使う型」です。

これらの型に対しても分解構文を使いたいです。
そこで、C# 7では、`Deconstruct`という名前のインスタンス メソッド、もしくは、拡張メソッドさえ持っていれば、
どんな型でも分解構文使えるようにしました。
例として`KeyValuePair`と`Tuple`に対する`Deconstruct`の書き方を示しましょう。
以下のような拡張メソッドがあれば分解できます。

<pre class="source" title="KeyValuePairとTupleの分解用のDeconstructメソッド">
<code><span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Deconstruct&lt;<span class="type">T</span>, <span class="type">U</span>&gt;(<span class="reserved">this</span> <span class="type">KeyValuePair</span>&lt;<span class="type">T</span>, <span class="type">U</span>&gt; pair, <span class="reserved">out</span> <span class="type">T</span> key, <span class="reserved">out</span> <span class="type">U</span> value)
    {
        key = pair.Key;
        value = pair.Value;
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Deconstruct&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt;(<span class="reserved">this</span> <span class="type">Tuple</span>&lt;<span class="type">T1</span>, <span class="type">T2</span>&gt; x, <span class="reserved">out</span> <span class="type">T1</span> item1, <span class="reserved">out</span> <span class="type">T2</span> item2)
    {
        item1 = x.Item1;
        item2 = x.Item2;
    }
}
</code></pre>

(ちなみに、.NET Core 2.0 以降か .NET Standard 2.1 以降であれば、`KeyValuePair` にはインスタンス メソッドとして標準で`Deconstruct`メソッドが追加されています。
`Tuple` の方は .NET Standard 2.0 以降であれば拡張メソッドとして`Deconstruct`メソッドがあります。)

これで、`KeyValuePair`と`Tuple`に対して分解構文が使えます。以下のようなコードが書けます。

<pre class="source" title="任意の型に対する分解宣言">
<code><span class="reserved">var</span> pair = <span class="reserved">new</span> <span class="type">KeyValuePair</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;(<span class="string">"one"</span>, 1);
<span class="reserved">var</span> (k, v) = pair;
<span class="comment">// 以下のようなコードに展開される</span>
<span class="comment">// string k;</span>
<span class="comment">// int v;</span>
<span class="comment">// pair.Deconstruct(out k, out v);</span>

<span class="reserved">var</span> tuple = <span class="type">Tuple</span>.Create(<span class="string">"abc"</span>, 100);
<span class="reserved">var</span> (x, y) = tuple;
<span class="comment">// 以下のようなコードに展開される</span>
<span class="comment">// string x;</span>
<span class="comment">// int y;</span>
<span class="comment">// tuple.Deconstruct(out x, out y);</span>
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="deconstruct-overload"></a>引数の数が同じオーバーロード不可

分解構文では、引数の数が同じ`Deconstruct`メソッドを呼び分けることができません。
例えば以下の例のように、引数の型が`double, double`のものと、`double, Radian`のものという2つの`Deconstruct`メソッドを定義してしまうと、2変数の分解ができなくなります。

<pre class="source" title="Deconstructメソッドの呼び分けができない(引数の数が同じ)例">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Math</span>;

<span class="reserved">struct</span> <span class="type">Radian</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Radian(<span class="reserved">double</span> value) =&gt; Value = value;
}

<span class="reserved">struct</span> <span class="type">Vector2D</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Y { <span class="reserved">get</span>; }

    <span class="comment">// コンストラクターは当然、個数が同じでも、型が違えば呼び分けができる</span>
    <span class="reserved">public</span> Vector2D(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; (X, Y) = (x, y);
    <span class="reserved">public</span> Vector2D(<span class="reserved">double</span> radius, <span class="type">Radian</span> angle)
        : <span class="reserved">this</span>(radius * Cos(angle.Value), radius * Sin(angle.Value)) { }

    <span class="comment">// 引数の数が同じ Deconstruct が2つある</span>
    <span class="comment">// 片方だけならいいけど、2つあると分解ができなくなる</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">double</span> x, <span class="reserved">out</span> <span class="reserved">double</span> y) =&gt; (x, y) = (X, Y);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">double</span> radius, <span class="reserved">out</span> <span class="type">Radian</span> angle)
        =&gt; (radius, angle) = (Sqrt(X * X + Y * Y), <span class="reserved">new</span> <span class="type">Radian</span>(Atan2(Y, X)));
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// コンストラクターの呼び分け</span>
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Vector2D</span>(1, 2);
        <span class="reserved">var</span> q = <span class="reserved">new</span> <span class="type">Vector2D</span>(10, <span class="reserved">new</span> <span class="type">Radian</span>(PI / 5));

        <span class="comment">// 分解は呼び分けできない</span>
        (<span class="reserved">double</span> x, <span class="reserved">double</span> y) = <span class="error">q</span>; <span class="comment">// コンパイル エラー</span>
        (<span class="reserved">double</span> r, <span class="type">Radian</span> a) = <span class="error">p</span>; <span class="comment">// コンパイル エラー</span>
    }
}
</code></pre>

一方で、引数の数が違えば複数の`Deconstruct`メソッドがあっても大丈夫です。
例えば以下のようなコードであれば、ちゃんと分解が使えます。

<pre class="source" title="Deconstructメソッドの呼び分けができる(引数の数が違う)例">
<code><span class="reserved">struct</span> <span class="type">Vector3D</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">double</span> Z { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Vector3D(<span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) =&gt; (X, Y, Z) = (x, y, z);

    <span class="comment">// 引数の数が違えば大丈夫</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">double</span> x, <span class="reserved">out</span> <span class="reserved">double</span> y, <span class="reserved">out</span> <span class="reserved">double</span> z) =&gt; (x, y, z) = (X, Y, Z);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">double</span> first, <span class="reserved">out</span> <span class="type">Vector2D</span> rest) =&gt; (first, rest) = (X, <span class="reserved">new</span> <span class="type">Vector2D</span>(Y, Z));
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Vector3D</span>(1, 2, 3);

        <span class="comment">// 分解可能</span>
        <span class="reserved">var</span> (first, rest) = p;
        <span class="reserved">var</span> (x, y, z) = p;
    }
}
</code></pre>

### <a id="sec-generated-title-10"></a> <a id="tuple-optimization"></a>タプルの構築や分解の最適化

分解構文は、基本的には`Deconstruct`メソッドの呼び出しに展開されます。
しかし、タプルに対しては、`Deconstruct`メソッドやコンストラクター呼び出しをなくす最適化が掛かります。

例えば以下のようなコード(いわゆるSwap処理)を書いたとします。

<pre class="source" title="タプル構築後にすぐに分解する例(swap)">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
(x, y) = (y, x);
</code></pre>

もしタプルが一般の型と同列に扱われるのなら、
「[ValueTuple構造体への展開](tuples.md#tuple-ValueTuple)」で説明した内容や、
前述の`Deconstruct`に展開される仕様を考えると、
これは以下のような意味にとることができます。

<pre class="source" title="(一般の型の分解と同列に考える場合の)タプル構築と分解の展開結果">
<code><span class="reserved">var</span> t = <span class="reserved">new</span> ValueTuple&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;(y, x);
t.Deconstruct(<span class="reserved">out</span> x, <span class="reserved">out</span> y);
</code></pre>

しかし、タプルに限り、単なる一時変数の追加やメンバーアクセスに展開され得ます<sup>※</sup>。
上記の `(x, y) = (y, x)` は、以下のように展開できます。

<pre class="source" title="タプルの場合は構築も分解も最適化で消える">
<code><span class="reserved">var</span> t1 = y; <span class="comment">// この t1 の方はさらに最適化で消える可能性あり</span>
<span class="reserved">var</span> t2 = x;
x = t1;
y = t2;
</code></pre>

<sup>※</sup>実際にどこまで最適化されるかは実装依存です。
例えば、C# 7.0の頃には `new ValueTuple<int, int>(x, y)` が一度作られていましたし、
現在の実装では `t1` も消えて `var t = x; x = y; y = t;` 相当のコードが出力されます。

### <a id="sec-generated-title-11"></a> <a id="ValueTuple"></a>余談: System.ValueTuple 構造体を要求される

タプルによる分解を使う場合、C# コンパイラーは常に`ValueTuple`構造体を要求します([System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/)パッケージの参照が必要)。

「常に」というところが少し曲者です。
例えば以下のような2つのステートメントを考えます。

<pre class="source" title="タプル構築と、タプル構築＋分解">
<code><span class="comment">// タプルの仕様上、ValueTuple&lt;int, int&gt; 構造体が作られる</span>
<span class="reserved">var</span> t = (1, 2);

<span class="comment">// 前述の通り、最適化が掛かるので ValueTuple は不要なはず</span>
<span class="reserved">var</span> (x, y) = (1, 2);
</code></pre>

前者は実際に`ValueTuple`構造体を必要としているので問題はありません。必要なものの参照を要求しているだけです。
一方、後者は`ValueTuple`構造体を使わないにも関わらず、C# コンパイラーは`ValueTuple`構造体の参照を求めます。

このコードから「すぐに分解するから最適化で消える」というの判定するのはコンパイラーにとっては意外と大変らしく、
「頑張っても見合わない」とのことで、この仕様を変えるつもりは今のところないようです。

## <a id="sec-generated-title-12"></a> <a id="evaluation"></a>分解の評価のされ方

分解構文では、メンバーごとにそれぞれ代入するような結果を生みます。
このとき、以下のようなルールが働きます。

- メンバーの評価は左から順
- メンバーの書き換えは同時に起こる

単純な場合、例えば`(a, b) = (x, y);`のような時にはこんなルールを気にするまでもなく、`a = x; b = y;`と同じ結果になります。
ここで、もう少し複雑な場合を考えてみましょう。

まず、左右で同じ変数が出てくる場合についてです。
分解構文では、各メンバーへの代入が同時に行われるかのような結果を生みます。
例えば、`x`と`y`という2つの変数の値を入れ替え(swap)ようとするとき、逐次実行であれば、以下のような書き方は間違いです。

<pre class="source" title="逐次実行でのswap">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;

y = x;
x = y; <span class="comment">// 上の行で y が書き換わっているので、値の入れ替えにはならない</span>

<span class="type">Console</span>.WriteLine(x); <span class="comment">// 1</span>
<span class="type">Console</span>.WriteLine(y); <span class="comment">// 1</span>

<span class="comment">// 正しくは以下のように書く</span>
<span class="comment">// var temp = y;</span>
<span class="comment">// y = x;</span>
<span class="comment">// x = temp;</span>
</code></pre>

これが、分解代入を使って以下のように書くと、正しく値が入れ替わります。

<pre class="source" title="分解代入を使ったswap">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;

<span class="comment">// 分解代入であれば、値の書き換えは同時に起こる</span>
(y, x) = (x, y);

<span class="type">Console</span>.WriteLine(x); <span class="comment">// 2</span>
<span class="type">Console</span>.WriteLine(y); <span class="comment">// 1</span>
</code></pre>

値が並行して同時に書き換わっているような結果を得るために、一時変数が挟まります。

<pre class="source" title="実際の評価のされ方">
<code><span class="comment">// 左辺の (y, x) を受け取る一時変数をまず用意</span>
<span class="reserved">var</span> t1 = y;
<span class="reserved">var</span> t2 = x;
<span class="comment">// 一時変数から改めて代入</span>
x = t1;
y = t2;
</code></pre>

さらに複雑になるのは、式が副作用を持つ場合です。
例として、分解代入の両辺に、悪名高いインクリメント演算を混ぜてみましょう。
各メンバーは、左から順に評価されます。

<pre class="source" title="分解代入の両辺にインクリメントを混ぜてみる">
<code><span class="reserved">var</span> a = <span class="reserved">new</span>[] { 0, 1, 2, 3 };
<span class="reserved">var</span> i = 0;

(a[i++], a[i++]) = (a[i++], a[i++]);

<span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Join(<span class="string">", "</span>, a)); <span class="comment">// 2, 3, 2, 3</span>
<span class="comment">// つまり、以下の評価を受けてる</span>
<span class="comment">// (a[0], a[1]) = (a[2], a[3]);</span>
</code></pre>

これと同じ動作をタプルと分解なしで書くと、以下のようなコードになります。

<pre class="source" title="左から順に評価するため、一時変数が挟まる">
<code><span class="reserved">var</span> a = <span class="reserved">new</span>[] { 0, 1, 2, 3 };
<span class="reserved">var</span> i = 0;

<span class="reserved">ref</span> <span class="reserved">var</span> l1 = <span class="reserved">ref</span> a[i++];
<span class="reserved">ref</span> <span class="reserved">var</span> l2 = <span class="reserved">ref</span> a[i++];
<span class="reserved">var</span> r1 = a[i++];
<span class="reserved">var</span> r2 = a[i++];

l1 = r1;
l2 = r2;
</code></pre>２
