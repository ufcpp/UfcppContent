---
title: "多態性"
source_url: "https://ufcpp.net/study/csharp/oop/oo_polymorphism/"
content_type: "Article"
published_at: "2015-05-06T14:09:43"
updated_at: "2007-10-06T00:00:00"
tags: []
umbraco_id: 1263
parent_id: 1248
sort_order: 12
aliases:
  - "/csharp/oo_polymorphism"
  - "/csharp/oo_polymorphism.html"
  - "/csharp/oop/oo_polymorphism/"
  - "/study/csharp/oo_polymorphism"
  - "/study/csharp/oo_polymorphism.html"
---

# 多態性

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

多態性(polymorphism: ポリモーフィズム)とは、
同じメソッド呼び出し(オブジェクト指向用語的には「メッセージ」という)に対して異なるオブジェクトが異なる動作をすることを言います。

（
ちなみに、polymorphism は多相性とか多様性と訳す場合もあります。
「poly（多）＋morphism（射：形を変えるみたいな意味） → いろいろな姿を映し出す」という意味。
）

オブジェクト指向プログラミング言語には、
多態性を実現するために、仮想メソッドというものが用意されています。


##### <a id="sec-generated-title-2"></a>ポイント

* オブジェクト指向の中核概念その3: 多態性。

* 同じ名前のメソッドを呼び出しで、異なる振る舞いをすること。

* 特に重要なのは、仮想関数を使った動的多態性。インスタンスの動的な型に応じて異なる振る舞いをする。

* （メソッドのオーバーロードも多態性の一種（静的多態性）。）



## <a id="sec-generated-title-3"></a> <a id="type"></a>静的な型、動的な型

「[継承](oo_inherit.md)」で説明したとおり、
派生クラスのインスタンスは基底クラスの変数に格納することが出来ます。
このとき、変数の型を<strong id="statictype" class="keyword">静的な型</strong>といい、
実際に格納されているインスタンスの型を<strong id="dynamictype" class="keyword">動的な型</strong>といいます。

<pre class="source" title="派生クラスのインスタンスを基底クラスの変数に格納" lang="">
<code><span class="reserved">class</span> Base{}
<span class="reserved">class</span> Derived : Base{}

<span class="reserved">class</span> DynamicTypeTest
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// 変数の型
    // ｜         実際に格納するインスタンスの型
    // ｜         ｜
    // ↓         ↓              静的な型, 動的な型</span>
    Base    a = <span class="reserved">new</span> Base();    <span class="comment">// Base    , Base</span>
    Base    b = <span class="reserved">new</span> Derived(); <span class="comment">// Base    , Derived</span>
    Derived c = <span class="reserved">new</span> Derived(); <span class="comment">// Derived , Derived</span>
  }
}
</code></pre>


ここでいう“静的”とはコンパイル時に型が確定するという意味です。
変数（new で生成されるインスタンスではなく、単なる入れ物）の型は、
宣言時に決まっていますので、静的な型になります。
つまり、実行時に型が変わるということはありません。

静的な型の情報は以下のように <strong id="typeof" class="keyword">typeof 演算子</strong>を用いて取得することが出来ます。
typeof 演算子は <code>System.Type</code> というクラスのインスタンスを返します。

<pre class="source" title="静的型情報 typeof" lang="">
<code><span class="reserved">typeof</span>(<span class="input">クラス名</span>)
</code></pre>


逆に、“動的”とはコンパイル時には型が確定せず、
実行時に変化する可能性のあるもののことを指します。
（なので、動的な型のことを実行時型（run-time type）とも言います。）
（単なる入れ物である）変数とは異なり、
（実行時に new で生成される）インスタンスの型は実行時に決まります。

動的な型の情報は以下のように <code>GetType</code> メソッドを用いて取得します。

<pre class="source" title="動的型情報 GetType" lang="">
<code><span class="input">変数名</span>.GetType()
</code></pre>



##### <a id="sec-generated-title-4"></a>サンプル

<pre class="source" title="動的型情報のサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base{}
<span class="reserved">class</span> Derived : Base{}

<span class="reserved">class</span> DynamicTypeTest
{
  <span class="reserved">static void</span> Main()
  {
    ShowDynamicType(<span class="reserved">new</span> Base());
    ShowDynamicType(<span class="reserved">new</span> Derived());
  }

  <span class="comment">// Base 型の変数 b に格納されているインスタンスの動的な型の名前を表示する。</span>
  <span class="reserved">static void</span> ShowDynamicType(Base b)
  {
    Type t = b.GetType();
    Console.Write(t.Name + <span class="literal">"\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
Base
Derived
</pre>



## <a id="sec-generated-title-5"></a> <a id="downcast"></a>ダウンキャスト

基底クラスの変数に派生クラスの変数を渡すことを<strong id="upcast" class="keyword">アップキャスト</strong>（upcast）と呼び、
それとは逆に、
派生クラスの変数に基底クラスの変数を渡すことを<strong id="downcast" class="keyword">ダウンキャスト</strong>（downcast）と呼びます。

基底クラスの変数に派生クラスのインスタンスを格納することは何の問題もありませんので、
アップキャストは常に安全に行うことが出来ます。
ところが、ダウンキャストの場合は必ずしも安全には行うことが出来ません。
以下に危険なダウンキャストの例を挙げます。

<pre class="source" title="" lang="">
<code><span class="reserved">class</span> Base{}
<span class="reserved">class</span> Derived1 : Base{}
<span class="reserved">class</span> Derived2 : Base{}

<span class="reserved">class</span> DowncastTest
{
  <span class="reserved">static void</span> Main()
  {
    Derived1 d1 = <span class="reserved">new</span> Derived1(); <span class="comment">// 当然、合法。</span>
    Derived2 d2 = <span class="reserved">new</span> Derived2(); <span class="comment">// 同じく、合法。</span>

    Base b;
    Derived1 d;

    b = d1;          <span class="comment">// アップキャストは常に合法。明示的なキャスト不要。</span>
    d = (Derived1)b; <span class="comment">// ダウンキャストは明示的なキャストが必要。
    // Derived1 の変数に Derived1 のインスタンスを格納しているので、これはOK。</span>

    b = d2;          <span class="comment">// 同じ事を今度は d2 の方で繰り返す。</span>
    d = (Derived1)b;
    <span class="comment">// Derived1 の変数に Derived2 のインスタンスを格納しているので、これは問題あり。
    // コンパイルは通るが、実行時エラーになる。</span>
  }
}
</code></pre>


このプログラムを実行すると <code>InvalidCastException</code> という例外が発生します。
(例外については「[例外処理](../structured/oo_exception.md)」で説明します。)

このような問題があるため、ダウンキャストを行う際には動的な型情報を取得する必要があります。
そのための構文として C# には <strong id="is-operator" class="keyword">is 演算子</strong>と <strong id="as-operator" class="keyword">as 演算子</strong>があります。

is 演算子はキャスト可能かどうかを調べるための演算子で以下のようにして使用します。

<pre class="source" title="is 演算子" lang="">
<code><span class="input">変数名</span> <span class="reserved">is</span> <span class="input">型名</span>
</code></pre>


is 演算子を適用した結果は bool 型になり、
左辺の変数が右辺の型にキャスト可能ならば true を、不能ならば false を返します。

<pre class="source" title="is 演算子の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base{}
<span class="reserved">class</span> Derived1 : Base{}
<span class="reserved">class</span> Derived2 : Base{}

<span class="reserved">class</span> DowncastTest
{
  <span class="reserved">static void</span> Main()
  {
    Base b;

    b = <span class="reserved">new</span> Derived1();
    <span class="reserved">if</span>(<em>b <span class="reserved">is</span> Derived1</em>)
      Console.Write(<span class="literal">"b = new Derived1();\n"</span>);

    b = <span class="reserved">new</span> Derived2();
    <span class="reserved">if</span>(<em>b <span class="reserved">is</span> Derived1</em>)
      Console.Write(<span class="literal">"b = new Derived2();\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
b = new Derived1();
</pre>


as 演算子はキャストと同じような働きをする演算子で、以下のようにして使用します。

<pre class="source" title="as 演算子" lang="">
<code><span class="input">変換先の変数</span> = <span class="input">変換元の変数</span> <span class="reserved">as</span> <span class="input">型名</span>
</code></pre>


キャストとの違いは、
もし型変換が出来ない場合には結果が null になるということです。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base{}
<span class="reserved">class</span> Derived1 : Base{}
<span class="reserved">class</span> Derived2 : Base{}

<span class="reserved">class</span> DowncastTest
{
  <span class="reserved">static void</span> Main()
  {
    Base b;
    Derived1 d;

    b = <span class="reserved">new</span> Derived1();
    d = <em>b <span class="reserved">as</span> Derived1</em>;
    <span class="reserved">if</span>(d != <span class="reserved">null</span>)
      Console.Write(<span class="literal">"b = new Derived1();\n"</span>);

    b = <span class="reserved">new</span> Derived2();
    d = <em>b <span class="reserved">as</span> Derived1</em>;
    <span class="reserved">if</span>(d != <span class="reserved">null</span>)
      Console.Write(<span class="literal">"b = new Derived2();\n"</span>);
  }
}
</code></pre>


<pre class="console" title="">
b = new Derived1();
</pre>


### <a id="sec-generated-title-6"></a> <a id="type-switch"></a>is演算子の拡張

<h5 class="version version7">Ver. 7</h5>

C# 7では、`is`演算子で以下のような書き方ができるようになりました。

<pre class="source" title="is 演算子の拡張" lang="">
<code><span class="input">変数名</span> <span class="reserved">is</span> <span class="input">型名</span> <span class="input">新しい変数名</span>
</code></pre>

演算子の結果はこれまで通り`bool`で、左辺の変数の中身が右辺の型にキャストできるなら`true`、できないなら`false`を返します。
そして、キャストできるとき、そのキャスト結果が新しい変数に入ります。
例えば、以下のような書き方ができます。

<pre class="source" title="C# 7の新しいis演算子の例">
<code><span class="reserved">static</span> <span class="reserved">void</span> TypeSwitch(<span class="reserved">object</span> obj)
{
    <span class="comment">// C# 7での新しい書き方</span>
    <span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">string</span> s)
    {
        <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + s.Length);
    }
}
</code></pre>

詳しくは、「[型スイッチ](../datatype/typeswitch.md#is)」で説明します。

## <a id="sec-generated-title-7"></a> <a id="virtual"></a>仮想メソッド

C# では、何も指定しない通常のメソッド呼び出し時、
基底クラスと派生クラスに同名のメソッドがある場合、
どちらのメソッドが呼び出されるかは静的な型によって決定されます。

<pre class="source" title="静的型情報に基づいたメソッドが呼び出し" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base
{
  <span class="reserved">public void</span> Test(){Console.Write(<span class="literal">"Base.Test()\n"</span>);}
}

<span class="reserved">class</span> Derived : Base
{
  <span class="reserved">public new void</span> Test(){Console.Write(<span class="literal">"Derived.Test()\n"</span>);}
}

<span class="reserved">class</span> NonVirtualTest
{
  <span class="reserved">static void</span> Main()
  {
    Base    a = <span class="reserved">new</span> Base();
    a.Test(); <span class="comment">// Base の Test が呼ばれる。</span>

    Base    b = <span class="reserved">new</span> Derived();
    b.Test(); <span class="comment">// Base の Test が呼ばれる。</span>

    Derived c = <span class="reserved">new</span> Derived();
    c.Test(); <span class="comment">// Derived の Test が呼ばれる。</span>
  }
}
</code></pre>


<pre class="console" title="">
Base.Test()
Base.Test()
Derived.Test()
</pre>


しかし、動的な型に基づいて呼び出されるメソッドを決定したい場合があります。
（というより、ほとんどの場合、メソッド呼び出しは動的に決定した方が都合がいい。）
動的な型に基づいて呼び出されるメソッドを選びたい場合、
以下のように、
メソッドに <em>virtual</em> という修飾子を付けます。

<pre class="source" title="動的型情報に基づいたメソッドが呼び出し" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base
{
  <span class="reserved">public <em>virtual</em> void</span> Test(){Console.Write(<span class="literal">"Base.Test()\n"</span>);}
}

<span class="reserved">class</span> Derived : Base
{
  <span class="reserved">public <em>override</em> void</span> Test(){Console.Write(<span class="literal">"Derived.Test()\n"</span>);}
}

<span class="reserved">class</span> VirtualTest
{
  <span class="reserved">static void</span> Main()
  {
    Base    a = <span class="reserved">new</span> Base();
    a.Test(); <span class="comment">// Base の Test が呼ばれる。</span>

    Base    b = <span class="reserved">new</span> Derived();
    b.Test(); <span class="comment">// Derived の Test が呼ばれる。</span>

    Derived c = <span class="reserved">new</span> Derived();
    c.Test(); <span class="comment">// Derived の Test が呼ばれる。</span>
  }
}
</code></pre>


<pre class="console" title="">
Base.Test()
Derived.Test()
Derived.Test()
</pre>


このような virtual 修飾子をつけたメソッドのことを<strong id="virtual_method" class="keyword">仮想メソッド</strong>（virtual method）と呼びます。

また、仮想メソッドを派生クラスで再定義することをメソッドの<strong id="override" class="keyword">オーバーライド</strong>(override: 上に重なる)と言います。
オーバーロード(「[関数](../structured/st_function.md)」のところにある「関数のオーバーロード」を参照)と混乱しそうになる名前ですが、別物です。

さらに、C#では、「[基底クラスのメンバーの隠蔽](oo_inherit.md#conceal)」と同様に、
プログラマの意図しないところでメソッドがオーバーライドされてしまうのを防ぐため、
メソッドをオーバーライドする際には <em>
        <code>override</code> 修飾子
      </em>を明示的に付ける必要があります。


## <a id="sec-generated-title-8"></a> <a id="usage"></a>仮想メソッドの利用例

仮想メソッド、すなわち、メソッドの動的呼び出しを用いると、
どのようなことが出来るのかを説明します。

ここではまた、例として Person クラスを使いましょう。
人間と一口に言ってもいろいろな人がいます。
例えば、年齢を聞いても、
正直に答える人、
鯖を読む人、
大体の年齢しか答えない人とさまざまなタイプの人がいます。

このようなさまざまなタイプの人をクラスで表現してみましょう。
まずは共通部分をまとめた基底クラス(<code>Person</code>)を定義します。
年齢を取得するプロパティ <code>Age</code> は、virtual にしておいて、
とりあえず意味のない値を返しておきます。

<pre class="source" title="人間の基底クラス" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">protected string</span> name;
  <span class="reserved">protected int</span> age;

  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }

  <span class="reserved">public string</span> Name{<span class="reserved">get</span>{<span class="reserved">return this</span>.name;}}
  <span class="reserved">public virtual int</span> Age{<span class="reserved">get</span>{<span class="reserved">return</span> 0;}} <span class="comment">// 基底クラスでは特に意味のない値を返す。</span>
}
</code></pre>


次に正直者を表すクラス(<code>Truepenny</code>)を定義します。
<code>Truepenny</code> の <code>Age</code> プロパティでは実年齢をそのまま返します。

<pre class="source" title="正直者クラス" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 正直者。
/// 年齢を偽らない。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Truepenny : Person
{
  <span class="reserved">public</span> Truepenny(<span class="reserved">string</span> name, <span class="reserved">int</span> age) : <span class="reserved">base</span>(name, age){}

  <span class="reserved">public override int</span> Age
  {
    <span class="reserved">get</span>
    {
      <span class="comment">// 実年齢をそのまま返す。</span>
      <span class="reserved">return this</span>.age;
    }
  }
}
</code></pre>


次は嘘つき(<code>Liar</code>)クラスの定義です。
<code>Liar</code> の <code>Age</code> プロパティでは、
歳を取るにつれ大幅に鯖を読んだ値を返します。

<pre class="source" title="嘘つきクラス" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 嘘つき。
/// 鯖を読む(しかも、歳取るにつれ大幅に)。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Liar : Person
{
  <span class="reserved">public</span> Liar(<span class="reserved">string</span> name, <span class="reserved">int</span> age) : <span class="reserved">base</span>(name, age){}

  <span class="reserved">public override int</span> Age
  {
    <span class="reserved">get</span>
    {
      <span class="comment">// 年齢を偽る。</span>
      <span class="reserved">if</span>(<span class="reserved">this</span>.age &lt; 20) <span class="reserved">return this</span>.age;
      <span class="reserved">if</span>(<span class="reserved">this</span>.age &lt; 25) <span class="reserved">return this</span>.age - 1;
      <span class="reserved">if</span>(<span class="reserved">this</span>.age &lt; 30) <span class="reserved">return this</span>.age - 2;
      <span class="reserved">if</span>(<span class="reserved">this</span>.age &lt; 35) <span class="reserved">return this</span>.age - 3;
      <span class="reserved">if</span>(<span class="reserved">this</span>.age &lt; 40) <span class="reserved">return this</span>.age - 4;
      <span class="reserved">return this</span>.age - 5;
    }
  }
}
</code></pre>


次はいい加減な人(<code>Equivocator</code>)クラスの定義です。
<code>Equivocator</code> の <code>Age</code> プロパティでは、
実年齢を四捨五入した値を返します。

<pre class="source" title="いい加減な人のクラス" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// いいかげん。
/// 大体の歳しか答えない。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Equivocator : Person
{
  <span class="reserved">public</span> Equivocator(<span class="reserved">string</span> name, <span class="reserved">int</span> age) : <span class="reserved">base</span>(name, age){}

  <span class="reserved">public override int</span> Age
  {
    <span class="reserved">get</span>
    {
      <span class="comment">// 年齢を四捨五入した値を返す。</span>
      <span class="reserved">return</span> ((<span class="reserved">this</span>.age + 5) / 10) * 10;
    }
  }
}
</code></pre>


おまけで永遠の17歳。

<pre class="source" title="永遠の17歳" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// いくつになったって気持ちは17歳。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Seventeenist : Person
{
  <span class="reserved">public</span> Seventeenist(<span class="reserved">string</span> name, <span class="reserved">int</span> age) : <span class="reserved">base</span>(name, age) { }

  <span class="reserved">public override int</span> Age
  {
    <span class="reserved">get</span>
    {
      <span class="reserved">return</span> 17;
    }
  }
}
</code></pre>


最後に、これらのクラスを利用したプログラムを作ってみます。
以下の例では、<code>Person</code> クラスを引数とし、
その人の自己紹介文を画面に表示するメソッドを用意し、
正直者、嘘つき、いい加減な人のそれぞれに自己紹介をしてもらいます。

<pre class="source" title="Person クラスとその派生クラスの利用例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> PolymorphismTest
{
  <span class="reserved">static void</span> Main()
  {
    Introduce(<span class="reserved">new</span> Truepenny   (<span class="literal">"Ky Kiske"</span>  , 24)); <span class="comment">// 正直者のカイさん24歳。</span>
    Introduce(<span class="reserved">new</span> Liar        (<span class="literal">"Axl Low"</span>   , 24)); <span class="comment">// 嘘つきのアクセルさん24歳。</span>
    Introduce(<span class="reserved">new</span> Equivocator (<span class="literal">"Sol Badguy"</span>, 24)); <span class="comment">// いい加減なソルさん24歳。</span>
    Introduce(<span class="reserved">new</span> Seventeenist(<span class="literal">"Ino"</span>       , 24)); <span class="comment">// 時空を超えるイノさん24歳。</span>
  }

  <span class="comment">/// &lt;summary&gt;
  /// p さんの自己紹介をする。
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> Introduce(Person p)
  {
    Console.Write(<span class="literal">"My name is {0}.\n"</span>, p.Name);
    Console.Write(<span class="literal">"I'm {0} years old.\n\n"</span>, p.Age);
  }
}
</code></pre>


<pre class="console" title="">
My name is Ky Kiske.
I'm 24 years old.

My name is Axl Low.
I'm 23 years old.

My name is Sol Badguy.
I'm 20 years old.

My name is Ino.
I'm 17 years old.
</pre>


正直者、嘘つき、いい加減な人はいずれも実年齢24歳にしてあります。
しかし、画面に表示される自己紹介文では異なる年齢が表示されています。

<code>Introduce</code> メソッド中では、
<code>Person</code> の <code>Age</code> プロパティが呼び出されていますが、
実際には、動的型情報に基づき、
<code>Truepenny</code>、<code>Liar</code>、<code>Equivocator</code> の
<code>Age</code> プロパティが呼び出されます。


## <a id="sec-generated-title-9"></a> <a id="polymorphism"></a>多態性とは

仮想メソッドの利用例のところで示したとおり、
仮想メソッドを用いると、同じメソッドを呼び出しても、
変数に格納されているインスタンスの型によって異なる動作をします。
このように、同じメッセージ(メソッド呼び出し)に対し、
異なるオブジェクトが異なる動作をすることを<strong id="polymorphism" class="keyword">多態性</strong>（polymorphism: ポリモーフィズム）と呼びます。

仮想メソッド呼び出しの他にも、
メソッドのオーバーロード
(同じ名前のメソッドでも、引数が異なれば動作も異なる)
なども多態性の一種であると考えられます。
しかし、メソッドのオーバーロードはその動作がコンパイル時に決定しますが、
仮想メソッド呼び出しの動作は実行時に決定するという違いがあります。
(前者を静的多態性、後者を動的多態性と言って区別する場合もあります。)

## <a id="sec-generated-title-10"></a> <a id="covariance">戻り値の共変性</a>

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 (.NET 5.0)から、仮想メソッドの戻り値に共変性が認められるようになりました。
(機能名の俗称としては、「クラスの共変戻り値」と言ったりします。)

例えば以下のようなコードを書けるようになります。

<pre class="source" title="仮想メソッド戻り値の共変性">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">Base</span> <span class="method">Clone</span>() =&gt; <span class="reserved">new</span> <span class="type">Base</span>();
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// これの戻り値が Base じゃなくてもよくなった。</span>
    <span class="comment">// Derived は常に Base に安全に変換可能なので、 Base Clone() の override として Derived Clone() を使える。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">Derived</span> <span class="method">Clone</span>() =&gt; <span class="reserved">new</span> <span class="type">Derived</span>();
}
</code></pre>

get のみのプロパティでも同様に、共変なオーバーライドができます。

<pre class="source" title="get のみのプロパティの共変戻り値">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">Base</span> P { <span class="reserved">get</span>; }
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// get のみの時は OK。</span>
    <span class="comment">// set を書いちゃうとコンパイル エラー。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="type">Derived</span> P { <span class="reserved">get</span>; }
}
</code></pre>

### <a id="sec-generated-title-11"></a> <a id="runtime-feature">ランタイム側の修正</a>

[デリゲート](../functional/sp_delegate.md#co-contra)や[ジェネリクス](sp4_variance.md)では元々できていたことなので、今までできなかったことの方が不思議なくらいです。
(実際、似たような言語でいうと、Java は JDK 5.0 以降で共変戻り値をサポートしています。)

[インターフェイスのデフォルト実装](oo_interface.md#runtime-feature)が C# 8.0 でやっと実装されたのと同様で、 .NET ランタイム側の修正が必要なためこれまで未実装でした。

ランタイム側の修正が必要ということは、古いランタイムでは動かせません。
[言語バージョン](../cheatsheet/langversionoption.md)で `LangVersion` 9.0 を明示的に指定していても、ターゲット フレームワークが .NET 5.0 (`net5.0`)以降でないとコンパイルできません。

ランタイム側の修正に関しては、以前書いたブログ「[RuntimeFeature クラス](../../../blog/2018/12/runtimefeature/index.md)」で説明しています。
(.NET 5.0 で `RuntimeFeature` クラスに `CovariantReturnsOfClasses` が追加されています。)

### <a id="sec-generated-title-12"></a> <a id="interface-covariant-returns">注意: インターフェイスの共変戻り値(C# 9.0 時点で未対応)</a>

C# 9.0 時点では共変戻り値を使えるのはクラスの仮想メソッド・仮想プロパティのみです。
将来的にはインターフェイスに対しても共変戻り値のサポートを考えているようですが、後回しにしたそうです。

例えば以下のようなコードはおそらく書きたい意図とは異なる挙動になると思います。

<pre class="source" title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例1">
<code><span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="type">IA</span> <span class="method">M</span>();
}
 
<span class="reserved">interface</span> <span class="type">IB</span> : <span class="type">IA</span>
{
    <span class="comment">// 以下の行は override 扱いを受けない。</span>
    <span class="comment">// 「IA.M を隠してしまう(別メソッド扱いされる)」という警告が出る。</span>
    <span class="type">IB</span> <span class="method"><span class="warning">M</span></span>();
}
</code></pre>

以下のようなコードはコンパイル エラーになります。

<pre class="source" title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例2">
<code><span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">public</span> <span class="type">IA</span> <span class="method">M</span>() =&gt; <span class="reserved">null</span>;
}
 
<span class="reserved">interface</span> <span class="type">IB</span> : <span class="type">IA</span>
{
    <span class="comment">// コンパイル エラー(IA.M と一致しない)</span>
    <span class="type">IB</span> <span class="type">IA</span>.<span class="method"><span class="error">M</span></span>() =&gt; <span class="reserved">null</span>;
}
</code></pre>

以下のような実装クラスもコンパイル エラーになります。

<pre class="source" title="インターフェイスの共変戻り値は C# 9.0 時点ではないという例3">
<code><span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="type">IA</span> <span class="method">M</span>();
}
 
<span class="reserved">class</span> <span class="type">ImpleA</span> : <span class="type"><span class="error">IA</span></span>
{
    <span class="comment">// コンパイル エラー(IA.M を実装していない)</span>
    <span class="reserved">public</span> <span class="type">ImpleA</span> <span class="method">M</span>() =&gt; <span class="reserved">this</span>;
}
</code></pre>
## <a id="exercise"></a>演習問題

### <a id="exercise-polim1"></a>問題 1


[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Triangle</code> クラスを元に、
以下のような継承構造を持つクラスを作成せよ。

まず、三角形や円等の共通の基底クラスとなる <code>Shape</code> クラスを以下のように作成。

<pre class="source" title="Shape" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Shape
{
  <span class="reserved">virtual public double</span> GetArea() { <span class="reserved">return</span> 0; }
  <span class="reserved">virtual public double</span> GetPerimeter() { <span class="reserved">return</span> 0; }
}
</code></pre>


そして、<code>Shape</code> クラスを継承して、
三角形 <code>Triangle</code> クラスと
円 <code>Circle</code> クラスを作成。

<pre class="source" title="Triangle" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
</code></pre>


<pre class="source" title="Circle" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
</code></pre>



#### 解答例 1


<pre class="source" title="Shape、Triangle、Circle" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">double</span> y; <span class="comment">// y 座標</span>

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// x 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> X
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// y 座標。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Y
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.y; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.y = value; }
  }

  <span class="reserved">#endregion
  #region</span> 演算子

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル和
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> +(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x + b.x, a.y + b.y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// ベクトル差
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;和&lt;/returns&gt;</span>
  <span class="reserved">public static</span> Point <span class="reserved">operator</span> -(Point a, Point b)
  {
    <span class="reserved">return new</span> Point(a.x - b.x, a.y - b.y);
  }

  <span class="reserved">#endregion</span>

  <span class="comment">/// &lt;summary&gt;
  /// A-B 間の距離を求める。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;点A&lt;/param&gt;
  /// &lt;param name="b"&gt;点B&lt;/param&gt;
  /// &lt;returns&gt;距離AB&lt;/returns&gt;</span>
  <span class="reserved">public static double</span> GetDistance(Point a, Point b)
  {
    <span class="reserved">double</span> x = a.x - b.x;
    <span class="reserved">double</span> y = a.y - b.y;
    <span class="reserved">return</span> Math.Sqrt(x * x + y * y);
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の基底クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Shape
{
  <span class="reserved">virtual public double</span> GetArea() { <span class="reserved">return</span> 0; }
  <span class="reserved">virtual public double</span> GetPerimeter() { <span class="reserved">return</span> 0; }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の円をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Circle : Shape
{
  Point center;
  <span class="reserved">double</span> radius;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 半径を指定して初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="r"&gt;半径。&lt;/param&gt;</span>
  <span class="reserved">public</span> Circle(Point center, <span class="reserved">double</span> r)
  {
    <span class="reserved">this</span>.center = center;
    <span class="reserved">this</span>.radius = r;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 円の中心。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point Center
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.center; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.center = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の半径。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public double</span> Radius
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.radius; }
    <span class="reserved">set</span> { <span class="reserved">this</span>.radius = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 円の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetArea()
  {
    <span class="reserved">return</span> Math.PI * <span class="reserved">this</span>.radius * <span class="reserved">this</span>.radius;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 円の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetPerimeter()
  {
    <span class="reserved">return</span> 2 * Math.PI * <span class="reserved">this</span>.radius;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (c = {0}, r = {1})"</span>,
      <span class="reserved">this</span>.center, <span class="reserved">this</span>.radius);
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわすクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle : Shape
{
  Point a;
  Point b;
  Point c;

  <span class="reserved">#region</span> 初期化

  <span class="comment">/// &lt;summary&gt;
  /// 3つの頂点の座標を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="a"&gt;頂点A&lt;/param&gt;
  /// &lt;param name="b"&gt;頂点B&lt;/param&gt;
  /// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
  <span class="reserved">public</span> Triangle(Point a, Point b, Point c)
  {
    <span class="reserved">this</span>.a = a;
    <span class="reserved">this</span>.b = b;
    <span class="reserved">this</span>.c = c;
  }

  <span class="reserved">#endregion
  #region</span> プロパティ

  <span class="comment">/// &lt;summary&gt;
  /// 頂点A。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point A
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> a; }
    <span class="reserved">set</span> { a = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点B。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point B
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> b; }
    <span class="reserved">set</span> { b = value; }
  }

  <span class="comment">/// &lt;summary&gt;
  /// 頂点C。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Point C
  {
    <span class="reserved">get</span> { <span class="reserved">return</span> c; }
    <span class="reserved">set</span> { c = value; }
  }

  <span class="reserved">#endregion
  #region</span> 面積・周

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetArea()
  {
    Point ab = b - a;
    Point ac = c - a;
    <span class="reserved">return</span> 0.5 * Math.Abs(ab.X * ac.Y - ac.X * ab.Y);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の周の長さを求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;周&lt;/returns&gt;</span>
  <span class="reserved">public override double</span> GetPerimeter()
  {
    <span class="reserved">double</span> l = Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.b);
    l += Point.GetDistance(<span class="reserved">this</span>.a, <span class="reserved">this</span>.c);
    l += Point.GetDistance(<span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
    <span class="reserved">return</span> l;
  }

  <span class="reserved">#endregion

  public override string</span> ToString()
  {
    <span class="reserved">return string</span>.Format(
      <span class="literal">"Circle (a = {0}, b = {1}, c = {2})"</span>,
      <span class="reserved">this</span>.a, <span class="reserved">this</span>.b, <span class="reserved">this</span>.c);
  }
}

<span class="comment">/// &lt;summary&gt;
/// Class1 の概要の説明です。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Class1
{
  <span class="reserved">static void</span> Main()
  {
    Triangle t = <span class="reserved">new</span> Triangle(
      <span class="reserved">new</span> Point(0, 0),
      <span class="reserved">new</span> Point(3, 4),
      <span class="reserved">new</span> Point(4, 3));

    Circle c = <span class="reserved">new</span> Circle(
      <span class="reserved">new</span> Point(0, 0), 3);

    Show(t);
    Show(c);
  }

  <span class="reserved">static void</span> Show(Shape f)
  {
    Console.Write(<span class="literal">"{0}\n"</span>, f);
    Console.Write(<span class="literal">"{0}\n"</span>, f.GetArea());
    Console.Write(<span class="literal">"{0}\n"</span>, f.GetPerimeter());
  }
}
</code></pre>
