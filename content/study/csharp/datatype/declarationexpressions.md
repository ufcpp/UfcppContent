---
title: "特殊な変数宣言"
source_url: "https://ufcpp.net/study/csharp/datatype/declarationexpressions/"
content_type: "Article"
published_at: "2016-12-24T00:00:00"
updated_at: "2020-05-12T00:00:00"
tags: []
umbraco_id: 2009
parent_id: 1940
sort_order: 4
aliases:
  - "/csharp/datatype/declarationexpressions/"
---

# 特殊な変数宣言

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
C# 7.0～9.0 に掛けて、
[パターン マッチング](patterns.md)をはじめとして、
[変数宣言](../start/st_variable.md#var-decl)を拡張するような機能が入っています。

- [型スイッチ](typeswitch.md)
  - [is演算子の拡張](typeswitch.md#is)
  - [switchステートメントの拡張](typeswitch.md#switch)
- [出力変数宣言](../resource/sp_ref.md#out-var)
- [分解代入](deconstruction.md#deconstruction-assignment)

C# 6.0 までの変数宣言と違って、以下のような性質があります。

- 式の途中でも変数宣言できる
- 複数の値のうち一部だけを受け取り、残りを破棄したいことがある

##<a id="sec-generated-title-2"></a> <a id="declaration-expression"></a>式中の変数宣言
C# 7.0 以降の構文に特有な点の1つとして、式の途中で変数を宣言できるようになるという点があります。

<pre class="source" title="式中の変数宣言">
<code><span class="comment">// C# 6.0 以前は、この x のように単独の変数宣言しかなかった。</span>
<span class="reserved">object</span> <span class="variable">x</span> = 1;
 
<span class="comment">// C# 7.0 以降、この y とか z とかのように式の途中で宣言される変数が増えた。</span>
<span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">int</span> <span class="variable">y</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">y</span>);
<span class="control">if</span> (<span class="reserved">int</span>.<span class="method">TryParse</span>(<span class="string">&quot;1&quot;</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">z</span>)) <span class="type">Console</span>.<span class="method">WriteLine</span>( <span class="variable">z</span>);
</code></pre>

ちなみに、案としてはここからさらに発展して、任意の式の中で変数を宣言できるような話も出ています。
この機能を<strong id="key-declaration-expression" class="keyword">変数宣言式</strong>(variable declaration expression)といいます。
例えば以下のように書けるようになるかもしれません。
(優先度低めとされていて、この機能が入る期待はそれほどしない方がいいです。
代わりに、[Expression blocks](https://github.com/dotnet/csharplang/issues/3086)のような機能が入るみたいな話もありますが、こちらもそれほど高い優先度は付いていません。)

<pre class="source" title="宣言式の例">
<code><span class="comment">// (草案。このままの文法が採用されるとは限らない) </span>
<span class="reserved">static</span> <span class="reserved">int</span> X(<span class="reserved">string</span> s) =&gt; (<span class="reserved">int</span> x = <span class="reserved">int</span>.Parse(s)) * x;
</code></pre>

`(int x = int.Parse(s))` の部分の戻り値は、`x`に代入された値です。結局、以下のコードと同じ意味ですが、これが「式」として書けます。

<pre class="source" title="宣言式の展開結果">
<code><span class="reserved">static</span> <span class="reserved">int</span> X(<span class="reserved">string</span> s)
{
    <span class="reserved">int</span> x = <span class="reserved">int</span>.Parse(s);
    <span class="reserved">return</span> x * x;
}
</code></pre>

式中で変数宣言があり得ることによって、
変数のスコープに関するルールがいくつか追加されています。
詳しくは「[C# 7での新しいスコープ ルール](../start/st_scope.md#csharp7)」で説明します。

##<a id="sec-generated-title-3"></a> <a id="discards"></a>値の破棄
型スイッチや分解では、変数を宣言しつつ何らかの値を受け取るわけですが、
特に受け取る必要のない余剰の値が生まれたりします。

例えば、分解の場合、複数の値のうち、1つだけを受け取りたい場合があったとします。
そういう場面が複数並んでしまった場合、以下のようなコードになりがちです。

<pre class="source" title="要らない値を無視するための適当な変数">
<code><span class="reserved">static</span> <span class="reserved">void</span> Deconstruct()
{
    <span class="comment">// 商と余りを計算するメソッドがあるけども、ここでは商しか要らない</span>
    <span class="comment">// 要らないので適当な変数 x とかで受ける</span>
    <span class="reserved">var</span> (q, x) = DivRem(123, 11);

    <span class="comment">// 逆に、余りしか要らない</span>
    <span class="comment">// 要らないから再び適当な変数 x で受けたいけども、x はもう使ってる</span>
    <span class="comment">// <em>しょうがないから x1 とかにしとくか…</em></span>
    <span class="reserved">var</span> (<em>x1</em>, r) = DivRem(123, 11);
}

<span class="reserved">static</span> (<span class="reserved">int</span> quotient, <span class="reserved">int</span> remainder) DivRem(<span class="reserved">int</span> dividend, <span class="reserved">int</span> divisor)
    =&gt; (<span class="type">Math</span>.DivRem(dividend, divisor, <span class="reserved">out</span> <span class="reserved">var</span> remainder), remainder);
</code></pre>

「しょうがないから」感がひどく、どう見ても不格好です。

こういう時に使うのが、値の<strong id="discard" class="keyword">破棄</strong>(discard)です。
以下のように、`_`を書くことで値を無視できます。

<pre class="source" title="_ で値の破棄">
<code>{
    <span class="comment">// _ を書いたところでは、値を受け取らずに無視する</span>
    <span class="reserved">var</span> (q, <span class="reserved"><em>_</em></span>) = DivRem(123, 11);

    <span class="comment">// _ は変数にはならないので、スコープを汚さない。別の場所でも再び _ を書ける</span>
    <span class="comment">// また、本来「var x」とか変数宣言を書くべき場所にも _ だけを書ける</span>
    (<span class="reserved"><em>_</em></span>, <span class="reserved">var</span> r) = DivRem(123, 11);
}
</code></pre>

1つ目の例では一見、`_`という名前の変数を定義しているようにも見えますが、別の挙動になります。
変数は作らず、スコープ内の別の場所でも再び`_`を使うことができます(先ほどの例みたいに`_1`みたいな変な名前を作らなくて済む)。

また、2つ目の例のように、「型名 変数名」みたいに書くべき場所でも、`var _`ではなく、`_`だけでOKです。

同様に、出力変数宣言でも`_`を破棄の意味で使えます。

<pre class="source" title="out 引数で、_ で値を破棄">
<code><span class="comment">// 欲しいのは戻り値だけであって、out 引数で受け取った値は要らない</span>
<span class="reserved">static</span> <span class="reserved">bool</span> CanParse(<span class="reserved">string</span> s) =&gt; <span class="reserved">int</span>.TryParse(s, <span class="reserved">out</span> _);
</code></pre>

型スイッチでも同様です。

<pre class="source" title="型スイッチで、_ で値を破棄">
<code><span class="reserved">static</span> <span class="reserved">int</span> TypeSwitch(<span class="reserved">object</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="reserved">int</span>[] x: <span class="reserved">return</span> x.Length;
        <span class="reserved">case</span> <span class="reserved">long</span>[] x: <span class="reserved">return</span> 2 * x.Length;
        <span class="comment">// int でさえあれば値は問わない</span>
        <span class="reserved">case</span> <span class="reserved">int</span> <span class="reserved">_</span>: <span class="reserved">return</span> 1;
        <span class="comment">// 同、long</span>
        <span class="reserved">case</span> <span class="reserved">long</span> <span class="reserved">_</span>: <span class="reserved">return</span> 2;
        <span class="reserved">case</span> <span class="reserved">null</span>: <span class="reserved">return</span> 0;
        <span class="comment">// 以下の行をコメントアウトするとエラーに</span>
        <span class="comment">// 今のところ、case _ は未実装(将来的に予定はあり)</span>
        <span class="comment">//case _:</span>
        <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentOutOfRangeException</span>();
    }
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="underscore"></a>_ が破棄の意味になる場合
`_`という記号は、元々のC#では[識別子](../start/misc_identifier.md)として有効な名前です。
すなわち、以下のコードは有効なC#コードです。

<pre class="source" title="_ は有効な識別子">
<code><span class="reserved">var</span> _ = 10;
<span class="type">Console</span>.WriteLine(_); <span class="comment">// 10 が表示される</span>
</code></pre>

`_`を破棄の意味で使うということは、`_`の使い方を変えるということになります。
なので、以下のように、文脈によって `_` の意味が変わります。

- C# 7から導入される新しい構文の中では、`_`が常に破棄の意味になる
- それ以前の構文では、1つも参照がなかった場合だけ`_`を破棄の意味で扱う(予定)

分解、出力引数宣言、型スイッチなど、C# 7から導入された構文の中では、
`_`が常に破棄の意味になります。
`_`という名前の変数は作られません。

<pre class="source" title="新構文における _">
<code><span class="reserved">static</span> <span class="reserved">void</span> Deconstruct1()
{
    <span class="comment">// 要らないので適当な変数 x とかで受ける</span>
    <span class="reserved">var</span> (q, x) = DivRem(123, 11);

    <span class="comment">// 要らないと言いつつ、参照できてしまう</span>
    <span class="type">Console</span>.WriteLine(x);

    <span class="comment">// 要らないものは _ で破棄</span>
    <span class="reserved">var</span> (<span class="reserved">_</span>, r) = DivRem(123, 11);

    <span class="comment">// 分解の中に書いた _ は変数にはならない</span>
    <span class="comment">// 以下の行でコンパイル エラーになる(_ は存在しない)</span>
    <span class="type">Console</span>.WriteLine(_);
}
</code></pre>

ちなみに、既存の構文に対しては破棄は使えません。
`_`は普通に変数扱いされます。

例えば、引数に対して `_` を使っても破棄の意味にはなりません。
以下のコードはコンパイル エラーになります。
(同名の引数が2つある状態。)

<pre class="source" title="引数の _ は破棄の意味にならない">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">_</span>, <span class="reserved">int</span> <span class="variable"><span class="error">_</span></span>)
{
}
</code></pre>

###<a id="sec-generated-title-5"></a> <a id="lambda-discard"></a>ラムダ式の引数
<h5 class="version version9">Ver. 9</h5>

既存の構文で破棄を使いたいものの代表例は、ラムダ式の引数でしょう。
C# 8.0 までは破棄の意味で`_`を使えず、「`_1`」みたいな名前が必要でした。

<pre class="source" title="C# 8.0時点では使えない _ 破棄">
<code><span class="reserved">static</span> <span class="reserved">void</span> Subscribe(<span class="type">INotifyPropertyChanged</span> source)
{
    <span class="comment">// C# 8.0 以前、2個目の _ が「同じ名前被ってる」エラーになる</span>
    source.PropertyChanged += (_, <span class="error">_</span>) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"property changed"</span>);
}
</code></pre>

C# 9.0 でこの場合に対応しました。
ただし、既存コードを壊さないように、2個以上の引数を `_` にした時だけ破棄の意味になるようにしています。

すなわち、以下のようなコードが書ける予定です。

<pre class="source" title="ラムダ式の引数で _ を破棄扱い">
<code><span class="reserved">static</span> <span class="reserved">void</span> Subscribe(<span class="type">INotifyPropertyChanged</span> source)
{
    <span class="comment">// 2回以上 _ を使かったら破棄扱い</span>
    source.PropertyChanged += (<span class="reserved">_</span>, <span class="reserved">_</span>) =&gt; { };

    <span class="comment">// _ が1回だけの場合は引数扱い。この場合普通に変数参照できる</span>
    source.PropertyChanged += (<span class="variable">_</span>, <span class="variable">_1</span>) =&gt; <span class="type">Console</span>.WriteLine(<span class="variable">_</span>);
}
</code></pre>
