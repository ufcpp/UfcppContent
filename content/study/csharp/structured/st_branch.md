---
title: "条件分岐"
source_url: "https://ufcpp.net/study/csharp/structured/st_branch/"
content_type: "Article"
published_at: "2015-05-06T14:08:20"
updated_at: "2021-01-02T00:00:00"
tags: []
umbraco_id: 1220
parent_id: 1217
sort_order: 2
aliases:
  - "/csharp/st_branch"
  - "/csharp/st_branch.html"
  - "/csharp/structured/st_branch/"
  - "/study/csharp/st_branch"
  - "/study/csharp/st_branch.html"
---

# 条件分岐

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
プログラム中で、ある条件を満たしたときだけ処理の流れを変えたい場合があります。
このような処理を<strong id="branch" class="keyword">条件分岐</strong>といい、
C#では条件分岐のために <code>if</code>、<code>else</code>、<code>switch</code> などのキーワードを用意しています。


##### <a id="sec-generated-title-2"></a>ポイント
* if(条件式) 真のとき

* if(条件式) 真のとき else 偽のとき

* switch(条件) { case 値: ... }

* goto Label;



##<a id="sec-generated-title-3"></a> <a id="if"></a>if 文
<strong id="if" class="keyword">if</strong> 文は以下のような書き方をします。

<pre class="source" title="if 文の書式" lang="">
<code><span class="reserved">if</span>(<span class="input">条件式</span>)
  <span class="input">文1</span> <span class="comment">// 条件式が真のときに実行される</span>
<span class="reserved">else</span>
  <span class="input">文2</span> <span class="comment">// 条件式が偽のときに実行される</span>
</code></pre>


英文法に近い書き方ですね。
if A, B, else C （もし A ならば B、さもなくば C）。

if 文は <code>if</code> の後の括弧内に書かれた条件式の真偽によって処理の流れを変えます。
条件式が真のときには 文1 が、偽のときには 文2 が実行されます。
また、<code>else</code> から後ろの部分は省略することができます。


##### <a id="sec-generated-title-4"></a>サンプル
<pre class="source" title="if 文の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> IfSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 整数を入力してもらう</span>
    <span class="reserved">int</span> x;
    Console.Write(<span class="literal">"整数を入力してください : "</span>);
    x = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="reserved">if</span>(x == 0)
    {
      <span class="comment">// 0が入力された場合、エラーメッセージだけ表示</span>
      Console.Write(<span class="literal">"0が入力されました"</span>);
    }
    <span class="reserved">else</span>
    {
      <span class="comment">// 0以外が入力された場合、入力された数値の逆数を求めて表示</span>
      <span class="reserved">double</span> x_inv = 1.0 / x;
      Console.Write(<span class="literal">"1/{0} = {1}"</span>, x, x_inv);
    }
  }
}
</code></pre>


<pre class="console" title="">
整数を入力してください : <span class="input">4</span>
1/4 = 0.25
</pre>


<pre class="console" title="">
整数を入力してください : <span class="input">0</span>
0が入力されました
</pre>

###<a id="sec-generated-title-5"></a> <a id="conditional-operator"></a>条件演算子
「[組み込み演算子](../start/st_operator.md#condition)」で紹介した条件演算子`?:`は、「`if`文の[式](miscexpressions.md#term)版」とも言える機能です。
式なので戻り値が必須ですが、以下のように、条件を満たすときと満たさないときの両方で同じ型の値を返す場合には条件演算子を使った方がすっきり書けることが多いです。

<pre class="source" title="if 文と条件演算子">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">num</span> = <span class="reserved">int</span>.<span class="method">Parse</span>(<span class="type">Console</span>.<span class="method">ReadLine</span>());
 
        <span class="comment">// if で偶奇判定</span>
        <span class="reserved">string</span> <span class="variable">parity1</span>;
        <span class="control">if</span> (<span class="variable">num</span> % 2 == 1) <span class="variable">parity1</span> = <span class="string">&quot;odd&quot;</span>;
        <span class="control">else</span> <span class="variable">parity1</span> = <span class="string">&quot;even&quot;</span>;
 
        <span class="comment">// 条件演算子で偶奇判定</span>
        <span class="reserved">var</span> <span class="variable">parity2</span> = <span class="variable">num</span> % 2 == 1 ? <span class="string">&quot;odd&quot;</span> : <span class="string">&quot;even&quot;</span>;
    }
}
</code></pre>

####<a id="sec-generated-title-6"></a> <a id="terget-typed-conditional"></a>条件演算子のターゲット型推論
<h5 class="version version9">Ver. 9</h5>

C# 9.0 から条件演算子に[ターゲット型](../start/misctyperesolution.md#target-type)からの型推論が働くようになりました。

これまで、条件演算子の結果は第2項・第3項から判別できる共通の型で決めていました。
(ソース型からの推論(例えば [`var`](../start/sp3_inference.md#implicit) など)とターゲット型からの推論は両立できないので、`var` などを使う場合には C# 9.0 でもこれまで通り、共通の型で決まります。)
共通の型が判別できないときにはコンパイル エラーになります。

例えば以下のようなコードはコンパイルできません。

<pre class="source" title="共通型を判別できない例">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="comment">// C# では整数型と null の共通型判定ができない。</span>
    <span class="comment">// 自動的に int? になってくれたりはしない(int? が後入り機能なせい)。</span>
    <span class="reserved">var</span> <span class="variable">i</span> = <span class="error"><span class="variable">b</span> ? 1 : <span class="reserved">null</span></span>;
 
    <span class="comment">// C# では「共通の基底クラスを探す」とかの処理はやらない。</span>
    <span class="comment">// インターフェイスは多重継承が可能で、共通基底を探す処理はかなりの時間を要することがあって、意図的に避けている。</span>
    <span class="reserved">var</span> <span class="variable">c</span> = <span class="error"><span class="variable">b</span> ? <span class="reserved">new</span> <span class="type">A</span>() : <span class="reserved">new</span> <span class="type">B</span>()</span>;
}
 
<span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span> { }
</code></pre>

これが、C# 9.0 から、ターゲット型を指定することでコンパイルできるようになります。

<pre class="source" title="条件演算子に対するターゲット型推論">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="comment">// var をやめて、int? を明示。</span>
    <span class="reserved">int</span>? <span class="variable">i</span> = <span class="variable">b</span> ? 1 : <span class="reserved">null</span>;
 
    <span class="comment">// var をやめて、Base を明示。</span>
    <span class="type">Base</span> <span class="variable">c</span> = <span class="variable">b</span> ? <span class="reserved">new</span> <span class="type">A</span>() : <span class="reserved">new</span> <span class="type">B</span>();
}
</code></pre>

##<a id="sec-generated-title-7"></a> <a id="switch"></a>switch 文
<strong id="switch" class="keyword">switch</strong> 文は以下のような書き方をします。

<pre class="source" title="switch文の書式" lang="">
<code><span class="reserved">switch</span>(<span class="input">変数</span>)
{
  <span class="reserved">case</span> <span class="input">値1</span>:
    <span class="input">いくつかの文1</span> <span class="comment">// 変数の値 == 値1 のとき実行される</span>
    <span class="reserved">break</span>;
  <span class="reserved">case</span> <span class="input">値2</span>:
    <span class="input">いくつかの文2</span> <span class="comment">// 変数の値 == 値2 のとき実行される</span>
    <span class="reserved">break</span>;
      ・
      ・
      ・
  <span class="reserved">default</span>:
    <span class="input">いくつかの文</span> <span class="comment">// 変数の値がどの値とも異なるとき実行される</span>
    <span class="reserved">break</span>;
}
</code></pre>


<code>switch</code> の後ろの括弧に書かれた変数の値によって処理の流れを変えます。
<code>switch</code> 中で使える変数は、整数型もしくは文字列型の変数のみです。

そして、<code>case</code> の後ろに条件となる値を書きます。
変数の値が <code>case</code> で指定されたどの値とも異なる場合、
<code>default</code> というラベルのついた場所に処理の流れが移ります。
<code>break</code> は switch 文から抜けるために使います。

###<a id="sec-generated-title-8"></a> <a id="type-switch"></a>型による分岐
<h5 class="version version7">Ver. 7</h5>

C# 6までは、`case`に書ける条件は値のみでした。その値と一致したときにだけ、`case`以下の文が実行されます。

一方、C# 7からは、型による分岐ができるようになりました。例えば以下のような書き方ができます。

<pre class="source" title="switchで型による分岐">
<code><span class="reserved">static</span> <span class="reserved">void</span> TypeSwitch(<span class="reserved">object</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="reserved">int</span> n:
            <span class="type">Console</span>.WriteLine(<span class="string">"整数 "</span> + n);
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="reserved">string</span> s:
            <span class="type">Console</span>.WriteLine(<span class="string">"文字列 "</span> + s);
            <span class="reserved">break</span>;
        <span class="reserved">default</span>:
            <span class="type">Console</span>.WriteLine(<span class="string">"その他"</span>);
            <span class="reserved">break</span>;
    }
}
</code></pre>

ちなみに、この書き方の場合、各`case`に対してさらに`when`句で条件を付けることができます。
この書き方では条件が被ることもありますが、そのときは書いた順に上から調べて最初に条件を満たした`case`が実行されます。

<pre class="source" title="when句付きのcase">
<code><span class="reserved">static</span> <span class="reserved">int</span> TypeSwitch(<span class="reserved">object</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="reserved">int</span> n <span class="reserved">when</span> n &lt; 1: <span class="reserved">return</span> 0;
        <span class="reserved">case</span> <span class="reserved">int</span> n <span class="reserved">when</span> n &lt; 10: <span class="reserved">return</span> 1;
        <span class="reserved">case</span> <span class="reserved">int</span> n <span class="reserved">when</span> n &lt; 100: <span class="reserved">return</span> 2;
        <span class="reserved">case</span> <span class="reserved">int</span> n <span class="reserved">when</span> n &lt; 1000: <span class="reserved">return</span> 3;
        <span class="reserved">case</span> <span class="reserved">int</span> n: <span class="reserved">return</span> (<span class="reserved">int</span>)Math.Log10(n);
        <span class="reserved">case</span> <span class="reserved">int</span>[] a: <span class="reserved">return</span> a.Length;
        <span class="reserved">default</span>: <span class="reserved">return</span> -1;
    }
}
</code></pre>

詳しくは「[型スイッチ](../datatype/typeswitch.md#switch)」で説明します。

###<a id="sec-generated-title-9"></a> <a id="tuple-switch"></a>複数の値で switch
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 からは、以下のように、複数の値をまとめて `switch` 文に掛けれるようになりました。

<pre class="source" title="複数の値に対する switch">
<code><span class="reserved">static</span> <span class="reserved">string</span> <span class="method">Color</span>(<span class="reserved">bool</span> <span class="variable">r</span>, <span class="reserved">bool</span> <span class="variable">g</span>, <span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="control">switch</span> (<span class="variable">r</span>, <span class="variable">g</span>, <span class="variable">b</span>)
    {
        <span class="control">case</span> (<span class="reserved">false</span>, <span class="reserved">false</span>, <span class="reserved">false</span>): <span class="control">return</span> <span class="string">&quot;black&quot;</span>;
        <span class="control">case</span> (<span class="reserved">true</span>, <span class="reserved">false</span>, <span class="reserved">false</span>): <span class="control">return</span> <span class="string">&quot;red&quot;</span>;
        <span class="control">case</span> (<span class="reserved">false</span>, <span class="reserved">true</span>, <span class="reserved">false</span>): <span class="control">return</span> <span class="string">&quot;green&quot;</span>;
        <span class="control">case</span> (<span class="reserved">false</span>, <span class="reserved">false</span>, <span class="reserved">true</span>): <span class="control">return</span> <span class="string">&quot;blue&quot;</span>;
        <span class="control">case</span> (<span class="reserved">false</span>, <span class="reserved">true</span>, <span class="reserved">true</span>): <span class="control">return</span> <span class="string">&quot;cyan&quot;</span>;
        <span class="control">case</span> (<span class="reserved">true</span>, <span class="reserved">false</span>, <span class="reserved">true</span>): <span class="control">return</span> <span class="string">&quot;magenta&quot;</span>;
        <span class="control">case</span> (<span class="reserved">true</span>, <span class="reserved">true</span>, <span class="reserved">false</span>): <span class="control">return</span> <span class="string">&quot;yellow&quot;</span>;
        <span class="control">case</span> (<span class="reserved">true</span>, <span class="reserved">true</span>, <span class="reserved">true</span>): <span class="control">return</span> <span class="string">&quot;white&quot;</span>;
    }
}
</code></pre>

正確に言うと、これは「[タプル](../datatype/tuples.md)に対する[位置パターン](../datatype/patterns.md#positional)」だったりします。
詳しくは「[タプル switch](../datatype/patterns.md#tuple-switch)」で説明します。

###<a id="sec-generated-title-10"></a> <a id="fallthrough"></a>フォールスルーの禁止
C# の先祖に当たる C 言語や C++ 言語では、
以下のようなコードが許されていました。

<pre class="source" title="C/C++ では許されたコード" lang="">
<code>swicth(x)
{
case 1:
  printf("x == 1 のときに実行される\n"); // (1)
case 2:
  printf("x == 1 でも x == 2 でも実行される\n"); // (2)
}
</code></pre>


変数 <code>x</code> が 1 のとき、(1) と (2) の両方の行が実行されます。
<code>x</code> が 2 のときには (2) だけが実行され、
それ以外の場合は何も実行されません。
すなわち、C/C++ では、switch 文中の case ラベルを超えてコードが実行され、
このような動作を<em>フォールスルー</em>（fall through）と呼びます。

ですが、実際にプログラムを作る際、多くの場合では、
<code>x</code> が 1 のときと 2 のときで、
全く別の処理をしたい、すなわち、
フォールスルーして欲しくない場合がほとんどで、
以下のように、<code>braek</code> を挿入して、
case ラベルを超えてコードが実行されないようにします。

<pre class="source" title="C/C++ では許されたコード" lang="">
<code>swicth(x)
{
case 1:
  printf("x == 1 のときだけ実行される\n");
  <em>break;</em>
case 2:
  printf("x == 2 のときだけ実行される\n");
  <em>break;</em>
}
</code></pre>


で、C/C++ では、
「フォールスルーして欲しくないのに、ついうっかり break を忘れる」
というバグが結構頻繁に起こりました。
そのため、<em>C# ではフォールスルーを禁止しています</em>。
すなわち、C# では、
case ラベル毎に必ず、break, 「[goto](#goto)」, 「[戻り値return](st_function.md#return)」 のいずれかを記述する必要があります。

毎回いちいち break を書くのが面倒ですが、必ず書く必要があります。
C/C++ 時代の名残です。

（「どうせフォールスルーできないんだから、break を明示的に書かなくてもフォールスルーしない仕様にして欲しい」という要望も多かったりします。
C# は C/C++ からの移行を意識して作られたので、
C/C++ プログラマの混乱を避けるために break を付ける構文になったんだと思います。
最初から C# でプログラミングを学び始める人もかなり出きた今となっては少々気持ち悪い構文です。
）

ただし、C# でも、以下のように、case ラベルが連続している場合に限りフォールスルー可能で、
break 等が必須ではありません。

<pre class="source" title="case ラベルの連続" lang="">
<code><span class="reserved">switch</span>(x)
{
  <span class="reserved">case</span> 1:
  <span class="reserved">case</span> 2:
    Console.Write(<span class="literal">"x == 1 か x == 2 のときに実行される\n"</span>);
    <span class="reserved">break</span>;
    <span class="comment">// case ラベルが連続している場合のみ OK。</span>
    <span class="comment">// case 1: と case 2: の間にコードを書いては駄目。</span>
}
</code></pre>



##### <a id="sec-generated-title-11"></a>サンプル
<pre class="source" title="switch文の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> SwitchSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 整数を2つ入力してもらう</span>
    <span class="reserved">int</span> x, y;
    Console.Write(<span class="literal">"1つ目の整数を入力してください : "</span>);
    x = <span class="reserved">int</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"2つ目の整数を入力してください : "</span>);
    y = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="comment">// + - / * のいずれかを入力してもらう</span>
    <span class="reserved">char</span> op;
    Console.Write(<span class="literal">"行いたい操作を入力してください(+ - / *) : "</span>);
    op = Console.ReadLine()[0];

    <span class="reserved">switch</span>(op)
    {
      <span class="reserved">case</span> <span class="literal">'+'</span>:
        Console.Write(<span class="literal">"{0} + {1} = {2}"</span>, x, y, x+y);
        <span class="reserved">break</span>;
      <span class="reserved">case</span> <span class="literal">'-'</span>:
        Console.Write(<span class="literal">"{0} - {1} = {2}"</span>, x, y, x-y);
        <span class="reserved">break</span>;
      <span class="reserved">case</span> <span class="literal">'*'</span>:
        Console.Write(<span class="literal">"{0} × {1} = {2}"</span>, x, y, x*y);
        <span class="reserved">break</span>;
      <span class="reserved">case</span> <span class="literal">'/'</span>:
        <span class="reserved">if</span>(y != 0)
          Console.Write(<span class="literal">"{0} ÷ {1} = {2} … {3}"</span>, x, y, x/y, x%y);
        <span class="reserved">break</span>;
      <span class="reserved">default</span>:
        Console.Write(<span class="literal">"対応していない操作です"</span>);
        <span class="reserved">break</span>;
    }
  }
}
</code></pre>


<pre class="console" title="">
1つ目の整数を入力してください : <span class="input">5</span>
2つ目の整数を入力してください : <span class="input">7</span>
行いたい操作を入力してください(+ - / *) : <span class="input">+</span>
5 + 7 = 12
</pre>


<pre class="console" title="">
1つ目の整数を入力してください : <span class="input">11</span>
2つ目の整数を入力してください : <span class="input">3</span>
行いたい操作を入力してください(+ - / *) : <span class="input">/</span>
11 ÷ 3 = 3 … 2
</pre>


<pre class="console" title="">
1つ目の整数を入力してください : <span class="input">1</span>
2つ目の整数を入力してください : <span class="input">1</span>
行いたい操作を入力してください(+ - / *) : <span class="input">0</span>
対応していない操作です
</pre>

###<a id="sec-generated-title-12"></a> <a id="switch-expression"></a>switch 式
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0では、`switch`の[式](miscexpressions.md#term)版が追加されました。
以下のような書き方をします。

<pre class="source" title="switch式の書式">
<code><span class="input">変数</span> <span class="control">switch</span>
{
    <span class="input">パターン1</span> =&gt; <span class="input">式1</span>,
    <span class="input">パターン2</span> =&gt; <span class="input">式2</span>,
      ・
      ・
      ・
}
</code></pre>

詳しくは「[`switch` 式](../datatype/typeswitch.md#switch-expression)」で説明します。

##<a id="sec-generated-title-13"></a> <a id="goto"></a>goto 文
<strong id="goto" class="keyword">goto</strong> 文は if 文や switch 文と異なり、無条件に処理の流れを変えるものです。
例えば以下のように使います。

<pre class="source" title="goto 文の例" lang="">
<code>START: <span class="comment">// ジャンプ先を示すラベル</span>
Console.Write("gotoの例");
<span class="reserved">goto</span> START;<span class="comment">// START: というラベルのある位置に処理の流れを移す</span>
</code></pre>


この例では、<code>Console.Write("gotoの例");</code>が何度も繰り返し実行されます。
(プログラムを強制終了するしか止める方法がないので注意)

goto 文を使用するとプログラムの処理の流れを追いづらくなるので、あまり使うのは好ましくないとされています。
そのため、通常は goto 文を使うのは以下のような場合に限られます。

1つは以下のように、switch 文で、<code>x</code> の値が1のときも2の時も同じ処理を行いたいといった場合に使います。

<pre class="source" title="switch 文中で goto を使う例" lang="">
<code><span class="reserved">switch</span>(x)
{
  <span class="reserved">case</span> 1:
    <span class="reserved">goto</span> <span class="reserved">case</span> 2; <span class="comment">// gotoを使って処理を移す</span>
  <span class="reserved">case</span> 2:
    <span class="comment">// x の値が1か2だった場合の処理</span>
    <span class="reserved">break</span>;
  <span class="reserved">case</span> 3:
    <span class="comment">// x の値が3だった場合の処理</span>
    <span class="reserved">break</span>;
  <span class="reserved">default</span>:
    <span class="comment">// そのほかの場合の処理</span>
    <span class="reserved">break</span>;
}
</code></pre>


もう1つ、以下のように多重ループ(ループについては「[反復処理](st_loop.md)」で説明します)から抜け出すときにも使います。

<pre class="source" title="多重ループから抜けるための goto の例" lang="">
<code><span class="reserved">while</span>(x != 0)
{
  <span class="reserved">while</span>(y != 0)
  {
    <span class="comment">// 繰り返し行いたい処理</span>

    <span class="reserved">if</span>(x == y)
      <span class="reserved">goto</span> LOOPEND; <span class="comment">// break では while(y != 0) の方のループしか抜けられない</span>
  }
}
LOOPEND:
;
</code></pre>
## <a id="exercise"></a>演習問題

### <a id="exercise-brancheo"></a>問題 1


ユーザから入力された整数が奇数か偶数か判定するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="奇数・偶数の判定" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"整数を入力してください: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="reserved">if</span> (n % 2 == 0) Console.Write(<span class="literal">"{0} は偶数です\n"</span>, n);
    <span class="reserved">else</span>            Console.Write(<span class="literal">"{0} は奇数です\n"</span>, n);
  }
}
</code></pre>



### <a id="exercise-branch0"></a>問題 2


[組込み演算子](../start/st_operator.md)の[問題 5](../start/st_operator.md#exercise-ope5)のプログラムを修正し、
BMI 値から体型(やせ型、普通、やや肥満、肥満、高度肥満)を判定し、
表示するプログラムを作成せよ。


#### 解答例 1


<pre class="source" title="BMI 値の計算と体型の判定" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"身長[cm]: "</span>);
    <span class="reserved">double</span> height = <span class="reserved">double</span>.Parse(Console.ReadLine()) * 0.01;
    Console.Write(<span class="literal">"体重[kg]: "</span>);
    <span class="reserved">double</span> weight = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> bmi = weight / (height * height);
    Console.Write(<span class="literal">"BMI = {0}\n"</span>, bmi);

    <span class="reserved">if</span>(bmi &lt; 19.8)      Console.Write(<span class="literal">"やせ型"</span>);
    <span class="reserved">else if</span>(bmi &lt; 24.2) Console.Write(<span class="literal">"普通"</span>);
    <span class="reserved">else if</span>(bmi &lt; 26.4) Console.Write(<span class="literal">"やや肥満（過体重）"</span>);
    <span class="reserved">else if</span>(bmi &lt; 35.0) Console.Write(<span class="literal">"肥満"</span>);
    <span class="reserved">else</span>                Console.Write(<span class="literal">"高度肥満（要治療）"</span>);
    Console.Write(<span class="literal">"です\n"</span>);
  }
}
</code></pre>



### <a id="exercise-branch1"></a>問題 3


switch 文を使って150以下の平方数(4＝2×2、9＝3×3、16＝4×4というように、ある整数の二乗になっている数)を判別するプログラムを作成せよ。
ユーザに整数値を1つ入力してもらい、
判別結果を出力するものとする。

ヒント：
要するに、ユーザからの入力が 1, 4, 9, 16, ・・・になっているかどうかを switch 文で判別します。


#### 解答例 1


<pre class="source" title="平方数の判別" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"整数を入力してください: "</span>);
    <span class="reserved">int</span> n = <span class="reserved">int</span>.Parse(Console.ReadLine());

    <span class="reserved">switch</span> (n)
    {
      <span class="reserved">case</span> 1:
        Console.Write(<span class="literal">"{0} は平方数です。\n"</span>, n);
        <span class="reserved">break</span>;
      <span class="reserved">case</span> 2 * 2: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 3 * 3: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 4 * 4: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 5 * 5: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 6 * 6: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 7 * 7: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 8 * 8: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 9 * 9: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 10 * 10: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 11 * 11: <span class="reserved">goto case</span> 1;
      <span class="reserved">case</span> 12 * 12: <span class="reserved">goto case</span> 1;
      <span class="reserved">default</span>:
        Console.Write(<span class="literal">"{0} は平方数ではないか、150以上です\n"</span>, n);
        <span class="reserved">break</span>;
    }
  }
}
</code></pre>



### <a id="exercise-branch2"></a>問題 4


数値を3つ入力してもらい、
その3つの値の中の最大値、最小値を求めるプログラムを作成せよ。


#### 解答例 1


単純な条件分岐による方法。

<pre class="source" title="最大値、最小値" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"値1: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値2: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値3: "</span>);
    <span class="reserved">double</span> z = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">if</span> (x &gt; y)
    {
      <span class="reserved">if</span> (x &gt; z)
      {
        <span class="reserved">if</span> (y &gt; z) Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, x, y, z);
        <span class="reserved">else</span>       Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, x, z, y);
      }
      <span class="reserved">else</span> Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, z, x, y);
    }
    <span class="reserved">else</span>
    {
      <span class="reserved">if</span> (y &gt; z)
      {
        <span class="reserved">if</span> (x &gt; z) Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, y, x, z);
        <span class="reserved">else</span>       Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, y, z, x);
      }
      <span class="reserved">else</span> Console.Write(<span class="literal">"最大 {0}, 中間 {1}, 最小 {2}\n"</span>, z, y, x);
    }
  }
}
</code></pre>



#### 解答例 2


3つの数値をあらかじめ整列してしまう方法。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Exercise
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"値1: "</span>);
    <span class="reserved">double</span> x = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値2: "</span>);
    <span class="reserved">double</span> y = <span class="reserved">double</span>.Parse(Console.ReadLine());
    Console.Write(<span class="literal">"値3: "</span>);
    <span class="reserved">double</span> z = <span class="reserved">double</span>.Parse(Console.ReadLine());

    <span class="reserved">double</span> tmp;

    <span class="reserved">if</span> (y &lt; z) { tmp = y; y = z; z = tmp; }
    <span class="reserved">if</span> (x &lt; y) { tmp = x; x = y; y = tmp; }
    <span class="reserved">if</span> (y &lt; z) { tmp = y; y = z; z = tmp; }

    Console.Write(<span class="literal">"最大 {0}, 中間{1}, 最小 {2}\n"</span>, x, y, z);
  }
}</code></pre>
