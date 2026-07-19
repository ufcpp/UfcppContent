---
title: "抽象メソッド、抽象クラス"
source_url: "https://ufcpp.net/study/csharp/oop/oo_abstract/"
content_type: "Article"
published_at: "2015-05-06T14:09:53"
updated_at: "2021-02-21T18:01:58"
tags: []
umbraco_id: 1267
parent_id: 1248
sort_order: 15
aliases:
  - "/csharp/oo_abstract"
  - "/csharp/oo_abstract.html"
  - "/csharp/oop/oo_abstract/"
  - "/study/csharp/oo_abstract"
  - "/study/csharp/oo_abstract.html"
---

# 抽象メソッド、抽象クラス

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
抽象メソッドとは、実装を持たず、メソッドの意味（規約）だけを定義したメソッドです。
抽象メソッドの実装は基底クラスでは行わず、派生クラスで行います。

また、抽象クラスとは、
インスタンスを生成出来ないクラスのことで、
継承して使うことを前提としたクラスのことです。


##### <a id="sec-generated-title-2"></a>ポイント
* 抽象メソッド: 基底クラスでは実装せず、メソッドの意味（規約）だけを定義して、派生クラスで具体的な実装を行うようなメソッド。

* （C++ では純粋仮想関数と呼ばれていたものです。）

* 抽象メソッドを1個でも持つクラス（抽象クラス）は、インスタンスを生成することができません。

* クラスやメソッドの前に abstract キーワードを付ける。



##<a id="sec-generated-title-3"></a> <a id="abstraction"></a>抽象化
「[多態性](oo_polymorphism.md)」で、
仮想メソッドの利用例として <code>Person</code> クラスを挙げました。
この <code>Person</code> 基底クラスには、
<code>Age</code> というプロパティがありますが、
このプロパティ自体は意味のある値を返さず、
実装は派生クラスの <code>Age</code> プロパティで行っていました。

<pre class="source" title="人間の基底クラス" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="comment">// ここではあんまり関係ないんで name は省略。</span>
  <span class="reserved">protected int</span> age;

  <span class="reserved">public</span> Person(<span class="reserved">int</span> age){<span class="reserved">this</span>.age  = age;}

  <span class="reserved">public virtual int</span> Age
  {
    <span class="comment">// 基底クラスでは特に意味のない値を返す。</span>
    <span class="comment">// 意味のある実装は派生クラスで行います。</span>
    <span class="reserved">get</span>
    {
      <span class="reserved">return</span> 0;
    }
  }
}
</code></pre>


しかし、<code>Person</code> クラスのように、
意味のない値を返すメソッドを持つクラスのインスタンスが生成されてしまうというのはあまり好ましいことではありません。

この問題を解決するためには2つの方法があります。
1つは基底クラスにデフォルトの動作を定める方法です。
すなわち、
性善説を信じて <code>Person</code> がデフォルトで正直な答えを返すようにするか、
性悪説を信じて <code>Person</code> がデフォルトで鯖を読むようにするか、
とにかく、<code>Person</code> の <code>Age</code> プロパティが何らかの意味を持つ値を返すようにします。

<pre class="source" title="性善説を信じた人間クラス" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">protected int</span> age;

  <span class="reserved">public</span> Person(<span class="reserved">int</span> age){<span class="reserved">this</span>.age  = age;}

  <span class="reserved">public virtual int</span> Age
  {
    <span class="comment">// 性善説を信じてみる。</span>
    <span class="comment">// 普通の人はみんな正直に年齢を答えてくれるに違いない。</span>
    <span class="reserved">get</span>
    {
      <span class="reserved">return</span> <span class="reserved">this</span>.age;
    }
  }
}
</code></pre>


そして、もう1つの方法は、<code>Person</code> クラスのインスタンスを生成出来ないようにすることです。
例えば、<code>Person</code> クラスのコンストラクタを protected にしてしまえば、<code>Person</code> クラスのインスタンスは外部から生成できなくなります。

<pre class="source" title="Person クラスのインスタンスを生成不能に" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">protected int</span> age;

  <span class="comment">// ↓ protected なので外部からコンストラクタを呼べない。</span>
  <span class="comment">//    Person は継承して使う専用のクラスになります。</span>
  <em><span class="reserved">protected</span></em> Person(<span class="reserved">int</span> age){<span class="reserved">this</span>.age  = age;}

  <span class="reserved">public virtual int</span> Age{<span class="reserved">get</span>{<span class="reserved">return</span> 0;}}
}
</code></pre>


これで <code>Person</code> クラスのインスタンスが作られることはなくなるんですが、
まだ <code>Person</code> クラスに意味のないメソッドの実装が残っています。
これは意味のないものをわざわざ書かなくてはいけないので無駄になりますし、
サブクラスでちゃんとオーバーライドしなければ無意味な値が返されてしまうという問題があります。

この問題を解決するため、
C# にはインスタンスを作成できないクラスや、
実装のない(派生クラスで必ずオーバーライドしなければならない)メソッドを定義するための構文が用意されています。

インスタンスを作成できないクラスは<strong id="abclass" class="keyword">抽象クラス</strong>（abstract class）と呼ばれています。
抽象クラスを作成するには、クラスの定義時に <em>
        <code>abstract</code>
      </em> 修飾子を付けます。

<pre class="source" title="抽象クラスの定義" lang="">
<code><span class="reserved"><em>abstract</em> class</span> Person
{
  <span class="reserved">protected int</span> age;

  <span class="comment">// 抽象クラスなので、コンストラクタが public であってもインスタンスは生成できない。</span>
  <span class="reserved">public</span> Person(<span class="reserved">int</span> age){<span class="reserved">this</span>.age  = age;}

  <span class="reserved">public virtual int</span> Age{<span class="reserved">get</span>{<span class="reserved">return</span> 0;}}
}
</code></pre>


また、実体を持たず、意味だけを定義し、実装は派生クラスで行うメソッドは<strong id="abmethod" class="keyword">抽象メソッド</strong>（abstract method）と呼ばれています。
抽象メソッドを作成するには、メソッドの定義時に <code>abstract</code> 修飾子を付けます。
抽象メソッドは抽象クラス中でしか定義できません。

ちなみに、「[プロパティ](oo_property.md#property)」も、内部的に見るとメソッドのようなものなので、
abstract を付けて抽象プロパティにすることができます。

<pre class="source" title="抽象メソッドの定義" lang="">
<code><span class="reserved">abstract class</span> Person
{
  <span class="reserved">protected int</span> age;

  <span class="reserved">public</span> Person(<span class="reserved">int</span> age){<span class="reserved">this</span>.age  = age;}

  <span class="reserved">public <em>abstract</em> int</span> Age{<span class="reserved">get</span>;} <span class="comment">// 抽象メソッドや抽象プロパティには定義は要らない</span>
}
</code></pre>



##### <a id="sec-generated-title-4"></a>サンプル
いままで例に挙げてきた <code>Person</code> クラスの最終形です。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

abstract <span class="reserved">class</span> Person
{
  <span class="reserved">protected string</span> name;
  <span class="reserved">protected int</span> age;

  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }

  <span class="reserved">public string</span> Name{<span class="reserved">get</span>{<span class="reserved">return this</span>.name;}}
  <span class="reserved">public</span> abstract <span class="reserved">int</span> Age{<span class="reserved">get</span>;} <span class="comment">// 抽象メソッドには定義は要らない</span>
}

<span class="comment">/// &lt;summary&gt;
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

<span class="comment">/// &lt;summary&gt;
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

<span class="comment">/// &lt;summary&gt;
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

<span class="comment">/// &lt;summary&gt;
/// いくつになったって気持ちは17歳。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Seventeenist : Person
{
  <span class="reserved">public</span> Seventeenist(<span class="reserved">string</span> name, <span class="reserved">int</span> age) : <span class="reserved">base</span>(name, age) { }

  <span class="reserved">public override int</span> Age
  {
    <span class="reserved">get</span>
    {
      <span class="comment">// 「おいおい」って突っ込み入れてあげてね。</span>
      <span class="reserved">return</span> 17;
    }
  }
}

<span class="reserved">class</span> PolymorphismTest
{
  <span class="reserved">static void</span> Main()
  {
    Introduce(<span class="reserved">new</span> Truepenny  (<span class="literal">"Ky Kiske"</span>  , 24)); <span class="comment">//正直者のカイさん24歳。</span>
    Introduce(<span class="reserved">new</span> Liar       (<span class="literal">"Axl Low"</span>   , 24)); <span class="comment">//嘘つきのアクセルさん24歳。</span>
    Introduce(<span class="reserved">new</span> Equivocator(<span class="literal">"Sol Badguy"</span>, 24)); <span class="comment">//いい加減なソルさん24歳。</span>
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


## <a id="exercise"></a>演習問題

### <a id="exercise-abst1"></a>問題 1


[多態性](oo_polymorphism.md)の[問題 1](oo_polymorphism.md#exercise-polim1)の <code>Shape</code> クラスを抽象クラス化せよ。


#### 解答例 1


必要な箇所（Shape クラスの部分）だけ抜粋。

<pre class="source" title="Shape" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 2次元空間上の図形を表すクラス。
/// 三角形や円等の共通の抽象基底クラス。
/// &lt;/summary&gt;</span>
abstract <span class="reserved">class</span> Shape
{
  <span class="reserved">public</span> abstract <span class="reserved">double</span> GetArea();
  <span class="reserved">public</span> abstract <span class="reserved">double</span> GetPerimeter();
}
</code></pre>
