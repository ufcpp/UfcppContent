---
title: "C# 2.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver2/"
content_type: "Article"
published_at: "2015-05-06T14:06:40"
updated_at: "2025-01-01T12:53:56"
tags:
  - "Ver. 2.0"
umbraco_id: 1178
parent_id: 1174
sort_order: 4
aliases:
  - "/csharp/ap_ver2"
  - "/csharp/ap_ver2.html"
  - "/csharp/cheatsheet/ap_ver2/"
  - "/study/csharp/ap_ver2"
  - "/study/csharp/ap_ver2.html"
---

# C# 2.0 の新機能

##<a id="sec-generated-title-1"></a> <a id="ver2"></a>C# 2.0
<div class="version version2">Ver. 2.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2005/10</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2005</li>
<li>.NET Framework 2.0</li>
<li>Visual Basic 8</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>ジェネリック</li>
<li>イテレーター</li>
</ul>
</td>
</tr>
</table>

2005年、Visual Studio 2005 の発売に合わせ、
.NET Framework および C# がメジャーバージョンアップして、
.NET Framework 2.0、C# 2.0 になりました。

バージョン 1.0 から 1.1 へのバージョンアップはほとんどバグフィックスのみで、
機能的な変更は小さなものでした。
これに対し、2.0 へのメジャーバージョンアップでは、
少し大きな機能追加がありました。

ここでは、その追加機能、すなわち、C# 2.0 の新機能について説明します。


##<a id="sec-generated-title-2"></a> <a id="generics"></a>Generics
C++ で言うところの template。
（ただし、template とは実装の方式が違います。）
以下のような感じで、“型をパラメータに持つ型”を作ることが出来ます。

<pre class="source" title="Generics の例" lang="">
<code><span class="reserved">public class</span> Stack<em>&lt;T&gt;</em>
{
	<em>T</em>[] items;
	<span class="reserved">int</span> count;
	<span class="reserved">public void</span> Push(<em>T</em> item) {...}
	<span class="reserved">public</span> <em>T</em> Pop() {...}
}
</code></pre>


Generics には、
コンパイル時に型のチェックが可能、ボクシング・ダウンキャストが不要などという利点があり、
開発の効率およびプログラムの実行効率が期待できます。

Generics はクラス、構造体、インターフェース、デリゲート、メソッドに対して適用可能です。

詳細は「[ジェネリック](../oop/sp2_generics.md)」にて説明します。


##<a id="sec-generated-title-3"></a> <a id="anonymous"></a>匿名メソッド
匿名メソッドとは、インラインに(コード中に直に)メソッドを記述できる機能です。
例えば、今までなら、イベントハンドラを定義するときに、
以下のように1度メソッドを定義してからデリゲートにそのメソッドを渡していました。

<pre class="source" title="イベントハンドラ(今までの書き方)" lang="">
<code><span class="reserved">class</span> InputForm: Form
{
  ...
  <span class="reserved">public</span> InputForm()
  {
    ...
    addButton.Click += <span class="reserved">new</span> EventHandler(AddClick);
  }

  <span class="reserved">void</span> AddClick(<span class="reserved">object</span> sender, EventArgs e)
  {
    listBox.Items.Add(textBox.Text);
  }
}
</code></pre>


それに対して、匿名メソッドを使った書き方では、
以下のようにコード中に直接メソッドを書くことが出来ます。

<pre class="source" title="イベントハンドラ(匿名メソッドを使った書き方)" lang="">
<code><span class="reserved">class</span> InputForm: Form
{
  ...
  <span class="reserved">public</span> InputForm()
  {
    ...
<em>    addButton.Click += <span class="reserved">delegate</span>
    {
      listBox.Items.Add(textBox.Text);
    };</em>
    <span class="comment">// ↑デリゲートの型は自動的に判別されます。</span>
    <span class="comment">/*
     * 引数つきの匿名デリゲートも定義できます。
     * ↑の例の場合、最初の行は
     * addButton.Click += delegate(object sender, EventArgs e)
     * と書くことも出来ます。
     */</span>
  }
}
</code></pre>


それから、匿名メソッドとは直接関係のない話ですが、
以下のように、メソッドを暗黙的にデリゲートに変換することが出来るようになりました。

<pre class="source" title="メソッド→デリゲートの変換" lang="">
<code>  <span class="reserved">static double</span>[] Apply(<span class="reserved">double</span>[] a, Function f) { ... }

    Apply(a, <span class="reserved">new</span> Function(Math.Sin)); <span class="comment">// 今までの書き方</span>
    Apply(a, Math.Sin);               <span class="comment">// Ver. 2.0 から</span>
</code></pre>


<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 では、「[ラムダ式](../functional/sp3_lambda.md#lambda)」という記法を使って匿名デリゲートを書けるようになりました。
こちらの記法の方が簡便なため、delegate キーワードを使った匿名デリゲートの書き方はもう使われなくなると思われます。


##<a id="sec-generated-title-4"></a> <a id="interator"></a>イテレータ
C# の foreach 構文は、コレクションクラスの利用者側から見ると非常に便利な機能です。
しかしながら、実装側から見た場合、<code>IEnumerable</code>や<code>IEnumerator</code>インターフェース実装する必要があり、結構面倒な作業が必要でした。

この実装側の労力を軽減するために、C# 2.0ではイテレータ構文というものが追加されました。
イテレータ構文は、コレクションクラスから要素を得る(yield: 産出する、利益を生む)ための構文で、以下のような書き方をします。

<pre class="source" title="イテレータ(GetEnumerator)" lang="">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">public class</span> Stack&lt;T&gt;: IEnumerable&lt;T&gt;
{
  T[] items;
  <span class="reserved">int</span> count;
  <span class="reserved">public void</span> Push(T data) {...}
  <span class="reserved">public</span> T Pop() {...}
  <span class="reserved">public</span> IEnumerator&lt;T&gt; GetEnumerator()
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i = count - 1; i &gt;= 0; --i)
      <span class="reserved"><em>yield</em> return</span> items[i];
  }
}
</code></pre>


<code>GetEnumerator()</code> メソッド中で、
<code>yield</code> というキーワードを用いて値を返すことで、
自動的に <code>IEnumerator</code> インターフェース実装するクラスを生成してくれます。
また、イテレータは以下のように、<code>IEnumerable</code> を返すメソッド/プロパティとしても定義することが出来ます。

<pre class="source" title="イテレータ(IEnumerable を返すメソッド)" lang="">
<code>  <span class="reserved">public</span> <em>IEnumerable&lt;T&gt;</em> BottomToTop
  {
    <span class="reserved">get</span>
    {
      <span class="reserved">for</span>(<span class="reserved">int</span> i = 0; i &lt; count; i++)
        <span class="reserved"><em>yield</em> return</span> items[i];
    }
  }
</code></pre>


利用者側では以下のようにして使用します。

<pre class="source" title="イテレータ(利用者側のコード)" lang="">
<code>    Stack&lt;<span class="reserved">int</span>&gt; stack = <span class="reserved">new</span> Stack&lt;<span class="reserved">int</span>&gt;();
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10; i++) stack.Push(i);
    <span class="reserved">foreach</span> (<span class="reserved">int</span> i <em><span class="reserved">in</span> stack</em>) Console.Write(<span class="literal">"{0} "</span>, i);
    Console.WriteLine();
    <span class="reserved">foreach</span> (<span class="reserved">int</span> i <em><span class="reserved">in</span> stack.BottomToTop</em>) Console.Write(<span class="literal">"{0} "</span>, i);
    Console.WriteLine();
</code></pre>


詳細は「[イテレーター](../data/sp2_iterator.md)」にて説明します。


##<a id="sec-generated-title-5"></a> <a id="partial"></a>Partial Type
C# 2.0 では、クラスや構造体などの型を複数のソースファイルに分けて記述できるようになりました。
分けて記述したい型には、以下のように <code>partial</code> キーワードを付けます。

<pre class="source" title="partial クラス" lang="">
<code><span class="reserved">public partial class</span> Customer
{
  <span class="reserved">private int</span> id;
  <span class="reserved">private string</span> name;
  <span class="reserved">private string</span> address;
  <span class="reserved">private</span> List&lt;Order&gt; orders;
  <span class="reserved">public</span> Customer() { ... }
}
<span class="comment">// ↑のクラスと↓のクラスは別ファイルに記述可能。</span>
<span class="reserved">public partial class</span> Customer
{
  <span class="reserved">public void</span> SubmitOrder(Order order) { orders.Add(order); }
  <span class="reserved">public bool</span> HasOutstandingOrders() { <span class="reserved">return</span> orders.Count &gt; 0; }
}
</code></pre>


この2つのクラスを記述したファイルを一緒にコンパイルすることで、1つのクラスに結合することが出来ます。
クラスの結合はコンパイル時に行う(DLL 参照時にはできない)ので、Partial Type の全ての部分を一緒にしてコンパイルする必要があります。


##<a id="sec-generated-title-6"></a> <a id="nullable"></a>Nullable 型
Nullable 型は、値型の型名の後ろに <code>?</code> を付ける事で、元の型の値または <code>null</code> の値を取れる型になるというものです。
<code>int</code> 型で例に取ると、以下のような書き方が出来ます。

<pre class="source" title="Nullable 型の例" lang="">
<code><span class="reserved">int</span>? x = 123;
<span class="reserved">int</span>? y = <span class="reserved">null</span>;
<span class="reserved">if</span> (x.HasValue) Console.WriteLine(x.Value); 
<span class="reserved">if</span> (y.HasValue) Console.WriteLine(y.Value);
</code></pre>


上述の例のよう、<code>int?</code> 型は、
整数値または <code>null</code> 値の代入および、値を持つかどうかの判別が出来る型になります。
また、以下のように、Nullable 型同士の演算を行う際には、値が <code>null</code> かを自動的に判別してくれます。

<pre class="source" title="Nullable 型同士の演算" lang="">
<code><span class="reserved">int</span>? x, y;
....
<span class="reserved">int</span>? z = x + y;
</code></pre>


上述のようなコードを書いた場合、以下のコードと同じ意味合いになります。

<pre class="source" title="Nullable 型同士の演算(等価なコード)" lang="">
<code><span class="reserved">int</span>? z = x.HasValue &amp;&amp; y.HasValue ? x.Value + y.Value : (<span class="reserved">int</span>?)<span class="reserved">null</span>;
</code></pre>


さらに、Nullable 型に対する特別な演算子として、<code>??</code> 演算子というものが追加されました。
（null coalescing operator といいます。
訳すなら、null 結合演算子。）
<code>??</code> 演算子は、値が <code>null</code> かどうかを判別し、<code>null</code> の場合には別の値を割り当てる演算子です。

<pre class="source" title="?? 演算子" lang="">
<code><span class="comment">// x, y は int? 型の変数</span>
<span class="reserved">int</span>? z = x ?? y; <span class="comment">// x != null ? x : y</span>
<span class="reserved">int</span> i = z ?? -1; <span class="comment">// z != null ? z.Value : -1</span>
</code></pre>


詳細は「[Nullable 型](../resource/sp2_nullable.md)」にて説明します。


##<a id="sec-generated-title-7"></a> <a id="level"></a>アクセサのアクセスレベル
以下に示すように、プロパティの set/get アクセサ別個のアクセスレベルが設定可能になりました。

<pre class="source" title="set/get のアクセスレベルを別個に設定" lang="">
<code><span class="reserved">public class</span> A
{
  <span class="reserved">public int</span> P
  {
    <span class="reserved">protected set</span> {...}
    <span class="reserved">get</span> {...}
  }
}
</code></pre>



##<a id="sec-generated-title-8"></a> <a id="static"></a>static クラス
static メンバーのみを持ち、インスタンスの作成が不可能なクラスを作りたいことがしばしばあります。
C# 1.0 では、private なコンストラクタを持つ sealed クラスとしてこのようなクラスを作成していました。
このような方法で、「インスタンスが作成不可能」という制約は満たすことが出来ますが、
非 static なメンバーを定義することができてしまうという問題がありました。
(決してアクセスすることの出来ない無駄なメンバーになってしまいます。)

それに対して、C# 2.0 では、
クラス定義時に <code>static</code> をつけることで、
static メンバーしか定義できないクラスを作ることが出来ます。


##<a id="sec-generated-title-9"></a> <a id="alias"></a>namespace alias qualifier
C# では、基本的に、名前空間に対しても、クラスに対しても、
<code>using</code> 文で作ったエイリアスに対しても、
全て <code>.</code> 修飾子(qualifier: 限定子とも訳す)を用いて名前を繋いでいました。

<pre class="source" title=". 修飾子" lang="">
<code><span class="reserved">namespace</span> Namespace
{
  <span class="reserved">public class</span> A{}
  <span class="reserved">namespace</span> Namespace2{ <span class="reserved">public class</span> A{} }
}
<span class="reserved">public class</span> Class
{
  <span class="reserved">public class</span> A{}
}

<span class="reserved">class</span> X
{
  <span class="reserved">using</span> Alias = Namespace.Namespace2; <span class="comment">// エイリアスを付ける。</span>

  Namespace.A a1; <span class="comment">// Namespace(名前空間) . A(クラス)</span>
  Class.A     a2; <span class="comment">// Class(クラス)       . A(クラス)</span>
  Alias.A     a3; <span class="comment">// Alias(エイリアス)   . A(クラス)</span>
                  <span class="comment">// ↑全部 .</span>
}
</code></pre>


もし、同一プロジェクト内の他の場所で Alias という名前のクラスまたは名前空間を追加作成すると、
このコードはエラーを起こしてしまいます。

<pre class="source" title=". 修飾子の問題点" lang="">
<code><span class="reserved">class</span> Alias{ <span class="reserved">public class</span> A{} }
<span class="comment">// ↑プロジェクト内のどこか他の場所にこの1行を追加すると・・・</span>

<span class="reserved">class</span> X
{
  <span class="reserved">using</span> Alias = Namespace.Namespace2;

  Alias.A a3; <span class="comment">// エラー。クラスの Alias？それともエイリアスの方？</span>
}
</code></pre>


そこで、C# 2.0 では、<code>::</code> 修飾子というものが追加されました。
<code>::</code> 修飾子は、<code>.</code> 修飾子とは異なり、
using 文もしくは後述する extern alias という構文を使って作成したエイリアスのみを参照します。
したがって、後々になって、プロジェクトにエイリアスと同じ名前を持つクラスや名前空間を追加してもエラーにはなりません。

<pre class="source" title="" lang="">
<code><span class="reserved">namespace</span> N
{
  <span class="reserved">public class</span> A {}
  <span class="reserved">public class</span> B {}
}
<span class="reserved">namespace</span> N
{
  <span class="reserved">using</span> A = System.IO; <span class="comment">// using 文でエイリアス作成。</span>
  <span class="reserved">class</span> X
  {
    A.Stream s1;  <span class="comment">// エラー。class A なのか A = System.IO なのか分からない。</span>
    A::Stream s2; <span class="comment">// OK。using 文で作ったエイリアス(A = System.IO)しか参照しない。</span>
  }
}
</code></pre>



##<a id="sec-generated-title-10"></a> <a id="extern"></a>extern alias
コンパイル時に、<code>/r</code> オプションで DLL の参照を指定する際に、
<code>/r:X=xxx.dll</code> というようにエイリアスを付けることが出来るようになりました。

<pre class="console" title="">
csc /r:X=xxx.dll /Y:yyy.dll test.cs
</pre>


<pre class="source" title="外部エイリアス" lang="">
<code><span class="reserved">extern alias</span> X; <span class="comment">// コンパイル時にオプションで指定したエイリアス</span>
<span class="reserved">extern alias</span> Y;
<span class="reserved">class</span> Test
{
  X::N.A a;  <span class="comment">// xxx.dll 内の N.A</span>
  X::N.B b1; <span class="comment">// xxx.dll 内の N.B</span>
  Y::N.B b2; <span class="comment">// yyy.dll 内の N.B</span>
  Y::N.C c;  <span class="comment">// yyy.dll 内の N.C</span>
}
</code></pre>



##<a id="sec-generated-title-11"></a> <a id="pragma"></a>#pragma
pragma プリプロセッサ命令が追加されました。
warning メッセージの抑止などが出来ます。

<pre class="source" title="pragma プリプロセッサ" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">class</span> Program
{
  [Obsolete]
  <span class="reserved">static void</span> Foo() {}
  <span class="reserved">static void</span> Main() {
<span class="comment">// 612番の警告(Obsolete メソッドを使用)を出さないようにする。</span>
<span class="reserved">#pragma</span> warning disable 612
  Foo();
<span class="comment">// 612番の警告を出すように戻す。</span>
<span class="reserved">#pragma</span> warning restore 612
  }
}
</code></pre>



##<a id="sec-generated-title-12"></a> <a id="conditional"></a>Conditional 属性
属性クラスに対して Conditional 属性を付けることで、
一定条件化でのみ適用される属性を作ることが可能になりました。

<pre class="source" title="" lang="">
<code><span class="reserved">#define</span> DEBUG
<span class="reserved">using</span> System;
<span class="reserved">using</span> System.Diagnostics;

<span class="comment">// ↓属性クラスに対して Conditional 属性を付ける。</span>
[Conditional(<span class="literal">"DEBUG"</span>)]
<span class="reserved">public class</span> TestAttribute : Attribute {}

<span class="comment">// ↓DEBUG シンボルが定義されているときのみ Test 属性が付く。</span>
[Test]
<span class="reserved">class</span> C {}
</code></pre>



##<a id="sec-generated-title-13"></a> <a id="fixed"></a>固定長配列
unsafe コード内限定で、
<code>fixed int fixedArray[128];</code> というように固定長配列が使用可能になりました。


##<a id="sec-generated-title-14"></a> <a id="co-contra"></a>デリゲートの Covariance/Contravariance
Covariance … 戻り値の型が、デリゲートの戻り値の型の派生クラスになっていても OK。

<pre class="source" title="Covariance" lang="">
<code><span class="reserved">class</span> Mammal {}       <span class="comment">// 哺乳類。</span>
<span class="reserved">class</span> Dog : Mammal {} <span class="comment">// 犬。</span>

<span class="reserved">class</span> Program
{
  <span class="comment">// Mammal（哺乳類）型を返すデリゲートを定義。</span>
  <span class="reserved">public delegate</span> Mammal MyHandlerMethod();

  <span class="reserved">public static</span> Mammal MammalHandler(){ <span class="reserved">return</span> null; }
  <span class="reserved">public static</span> Dog DogHandler(){ <span class="reserved">return</span> null; }

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {

    MyHandlerMethod handler_1 = <span class="reserved">new</span> MyHandlerMethod(MammalHandler);

    <span class="comment">// Covariance によって、以下のメソッドもデリゲート化可能。</span>
    <span class="comment">// Dog 型の変数を Mammal 型に渡すのは OK なので、戻り値は暗黙的にキャストされる。</span>
    MyHandlerMethod handler_2 = <span class="reserved">new</span> MyHandlerMethod(DogHandler);

  }
}
</code></pre>


Contravariance … 引数の型が、デリゲートの引数の型の基底クラスになっていても OK。

<pre class="source" title="Contravariance" lang="">
<code><span class="reserved">class</span> Mammal {}       <span class="comment">// 哺乳類。</span>
<span class="reserved">class</span> Dog : Mammal {} <span class="comment">// 犬。</span>

<span class="reserved">class</span> Program
{
  <span class="comment">// Dog（犬）型を受け取るデリゲートを定義。</span>
  <span class="reserved">public delegate void</span> MyHandlerMethod(Dog dog);

  <span class="reserved">public static</span> void MammalHandler(Mammal elephant){}
  <span class="reserved">public static</span> void DogHandler(Dog sheepdog){}

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {

    <span class="comment">// Contravariance によって、以下のメソッドもデリゲート化可能。</span>
    <span class="comment">// Dog 型の変数を Mammal 型に渡すのは OK なので、引数は暗黙的にキャストされる。</span>
    MyHandlerMethod handler_1 = <span class="reserved">new</span> MyHandlerMethod(MammalHandler);

    MyHandlerMethod handler_2 = <span class="reserved">new</span> MyHandlerMethod(DogHandler);

  }
}
</code></pre>


数学用語的には、Covariance … 共変性、Contravariance … 反変性。
プログラミングの分野でもこの訳語で定着したようです。
