---
title: "コンストラクター"
source_url: "https://ufcpp.net/study/csharp/oop/oo_construct/"
content_type: "Article"
published_at: "2015-05-06T14:09:22"
updated_at: "2020-07-04T00:00:00"
tags: []
umbraco_id: 1252
parent_id: 1248
sort_order: 2
aliases:
  - "/csharp/oo_construct"
  - "/csharp/oo_construct.html"
  - "/csharp/oop/oo_construct/"
  - "/study/csharp/oo_construct"
  - "/study/csharp/oo_construct.html"
---

# コンストラクター

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

オブジェクトを作成するためには、オブジェクトを正しく初期化してやる必要があります。
そのために、オブジェクトの構築のためのコンストラクターと呼ばれる特殊なメソッドが用意されています。


##### <a id="sec-generated-title-2"></a>ポイント

* コンストラクターで初期化
    * new したときに呼び出される特殊なメソッド。
    * 型名と同じ名前で定義する。
* 例えば、class Person { public Person(string name) { ... } ... }

## <a id="sec-generated-title-3"></a> <a id="ctor"></a>コンストラクター

コンストラクターはインスタンスを正しく初期化するための特別なメソッドです。
コンストラクターは以下のように、型名と同じ名前のメソッドを書くことで定義できます。

<pre class="source" title="コンストラクターの例" lang="">
<code><span class="reserved">class</span> SampleClass
{
  <span class="comment">// ↓これがコンストラクター</span>
  SampleClass()
  {
    <span class="comment">// インスタンスの初期化用のコードを書く</span>
  }
}
</code></pre>


他のメソッドと異なり、戻り値の型は書きません(コンストラクターは戻り値を返すことは出来ません)。

例えば、名簿作成のために個人情報を表す <code>Person</code> というクラスを作ったとします。
説明を簡単にするために、この名簿では名前と年齢だけを管理することにします。
そのため、<code>Person</code> は <code>name</code> と <code>age</code> という2つのメンバーのみを定義します。

<pre class="source" title="Person クラスその1" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢</span>
}
</code></pre>


ここで、<code>Person</code>クラスのインスタンスを生成する際、
名前を <code>""</code> (空の文字列)で、年齢を <code>0</code> で初期化したいとします。
そのためには以下のようなコンストラクターを作成します。

<pre class="source" title="Person クラスその2" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢</span>

  <span class="comment">// ↓これが Person クラスのコンストラクター</span>
  <span class="reserved">public</span> Person()
  {
    name = "";
    age  = 0;
  }
}
</code></pre>


コンストラクターは <code>new</code> を用いてインスタンスを作成する際に呼び出されます。
例えば、下記のようなコードを実行した場合、

<pre class="source" title="コンストラクターが呼び出されるタイミング" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Test
{
  <span class="reserved">public</span> Test()
  {
    Console.Write(<span class="literal">"Test クラスのコンストラクターが呼ばれました\n"</span>);
  }
}

<span class="reserved">class</span> ConstructorSample
{
  <span class="reserved">static void</span> Main()
  {
    Console.Write(<span class="literal">"Main の先頭\n"</span>);

    Test t = <span class="reserved">new</span> Test(); <span class="comment">// ここで Test のコンストラクターが呼ばれる</span>

    Console.Write(<span class="literal">"Main の末尾\n"</span>);
  }
}
</code></pre>


以下のような出力が得られます。

<pre class="console" title="">
Main の先頭
Sample クラスのコンストラクターが呼ばれました
Main の末尾
</pre>


また、コンストラクターには引数を与えることもできます。
例えば、先ほどの <code>Person</code> クラスで、
インスタンスの作成時に名前と年齢の値を設定したい場合、
以下のようなコンストラクターを作成します。

<pre class="source" title="Person クラスその3" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢</span>

  <span class="comment">// ↓引数つきの Person クラスのコンストラクター</span>
  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }
}
</code></pre>


この例で使われている <em>
        <code>this</code>
      </em> というキーワードは、
作成するインスタンス自身を格納する特別な変数です。
そのため、この例では <code>this.name</code> は <code>Person</code> クラス内で定義された <code>name</code> のことになります。
一方、<code>this</code> の付いていない方の <code>name</code> は、コンストラクターの引数として定義した <code>name</code> のことです。

引数つきのコンストラクターを呼び出すためには、<code>new</code> を使ってインスタンスを生成する際に、以下のようにして引数を渡します。

<pre class="source" title="引数つきコンストラクターの呼び出し" lang="">
<code><span class="input">型名</span> <span class="input">変数名</span> = <span class="reserved">new</span> <span class="input">型名</span>(<span class="input">引数リスト</span>);
</code></pre>

(後述しますが、[C# 9.0 からは `new` の後ろの型名を省略できることがあります](#target-typed-new)。)

例えば、先ほど定義した<code>Person</code>クラスのコンストラクターを呼び出すためには以下のようにします。

<pre class="source" title="引数つきコンストラクターの例" lang="">
<code>Person p = <span class="reserved">new</span> Person(<span class="literal">"ビスケット・クルーガー"</span>, 57);
Console.Write(p.age); <span class="comment">// 57 と表示される</span>
</code></pre>


また、コンストラクターはオーバーロードすることができます。
例えば、<code>Person</code> クラスに、名前と年齢を引数として与えるコンストラクターと、何も引数を与えないコンストラクターの両方を定義することができます。

<pre class="source" title="Person クラスその4" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢</span>

  <span class="comment">// ↓引数なしの Person クラスのコンストラクター</span>
  <span class="reserved">public</span> Person()
  {
    <span class="reserved">this</span>.name = "";
    <span class="reserved">this</span>.age  = 0;
  }

  <span class="comment">// ↓引数つきの Person クラスのコンストラクター</span>
  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }
}
</code></pre>



##### <a id="sec-generated-title-4"></a>サンプル

<pre class="source" title="コンストラクターのサンプル" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 名簿用の個人情報記録用のクラス。
/// とりあえず、名前と年齢のみ。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Person
{
  <span class="comment">// public なフィールド</span>
  <span class="reserved">public string</span> name; <span class="comment">// 氏名</span>
  <span class="reserved">public int</span>    age;  <span class="comment">// 年齢

  // 定数</span>
  <span class="reserved">const int</span> UNKNOWN = -1;
  <span class="reserved">const string</span> DEFAULT_NAME = <span class="literal">"デフォルトの名無しさん"</span>;

  <span class="comment">/// &lt;summary&gt;
  /// 名前と年齢を初期化
  /// 与えられた年齢が負のときは年齢不詳とみなす
  /// &lt;/summary&gt;
  /// &lt;param name="name"&gt;氏名&lt;/param&gt;
  /// &lt;param name="age"&gt;年齢&lt;/param&gt;</span>
  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age &gt; 0 ? age : UNKNOWN;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 名前のみを初期化
  /// 年齢は不詳とする
  /// &lt;/summary&gt;
  /// &lt;param name="name"&gt;氏名&lt;/param&gt;</span>
  <span class="reserved">public</span> Person(<span class="reserved">string</span> name) : <span class="reserved">this</span>(name, UNKNOWN)
  {
  }

  <span class="comment">/// &lt;summary&gt;
  /// デフォルトコンストラクター
  /// 氏名・年齢ともに不詳
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> Person() : <span class="reserved">this</span>(<span class="reserved">null</span>, UNKNOWN)
  {
  }

  <span class="comment">/// &lt;summary&gt;
  /// 文字列化
  /// 氏名が不詳のときには NONAME に設定された名前を返す
  /// 年齢が不詳の時には名前のみを返す
  /// 氏名・年齢が分かっているときには「名前(xx歳)」という形の文字列を返す
  /// &lt;/summary&gt;</span>
  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">if</span>(name == <span class="reserved">null</span>)
      <span class="reserved">return</span> DEFAULT_NAME;

    <span class="reserved">if</span>(age == UNKNOWN)
      <span class="reserved">return</span> name;

    <span class="reserved">return</span> name + <span class="literal">"("</span> + age + <span class="literal">"歳)"</span>;
  }
}<span class="comment">//class Person</span>

<span class="comment">//----------------------------------------------------
// メインプログラム</span>
<span class="reserved">class</span> ConstructorSample
{
  <span class="reserved">static void</span> Main()
  {
    Person p1 = <span class="reserved">new</span> Person(<span class="literal">"ちゆ"</span>, 12);
    Person p2 = <span class="reserved">new</span> Person(<span class="literal">"澪"</span>);
    Person p3 = <span class="reserved">new</span> Person();

    Console.Write(<span class="literal">"{0}\n{1}\n{2}\n"</span>, p1, p2, p3);
  }
}</code></pre>


<pre class="console" title="">
ちゆ(12歳)
澪
デフォルトの名無しさん
</pre>


## <a id="sec-generated-title-5"></a> <a id="variable-initializer"></a>フィールド初期化子

フィールドに初期値を与えるだけなら、
コンストラクターを使わなくても、以下の様な書き方で初期化できます。

<pre class="source" title="フィールドに対するフィールド初期化子" lang="">
<code><span class="reserved">class</span> Person
{
<em>    <span class="reserved">public string</span> name = <span class="literal">""</span>;
    <span class="reserved">public int</span> age = 0;</em>
}
</code></pre>

こういう書き方をフィールド初期化子（variable initializer）と言います。フィールド初期化子は、フィールドと定数に対して付けることができます。

説明は後程になりますが、[プロパティ](oo_property.md#get-only)に対しても同様の初期化を行うことができ、こちらは「プロパティ初期化子」と呼びます。
(初期化する対象の名前が違うだけで、ほぼ同じものです。)


## <a id="sec-generated-title-6"></a> <a id="initializer"></a>コンストラクター初期化子

場合によっては、あるコンストラクターから別のコンストラクターを呼びだしたいことがあります。
このような場合に、以下のような書き方で、別のコンストラクターを呼び出すことができます。

<pre class="source" title="コンストラクター初期化子" lang="">
<code><span class="reserved">class</span> Person
{
    <span class="reserved">public string</span> name;
    <span class="reserved">public int</span> age;

    <span class="reserved">public</span> Person()
        <em>: <span class="reserved">this</span>(<span class="literal">""</span>, 0)</em> <span class="comment">// ↓のPerson(string, int) が呼ばれる。</span>
    {
    }

    <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
    {
        <span class="reserved">this</span>.name = name;
        <span class="reserved">this</span>.age = age;
    }
}
</code></pre>


この書き方をコンストラクター初期化子（constructor initializer）と言います。
（[別項](oo_inherit.md#base_ctor)で説明する`base`と区別してthis初期化子と言うこともあります。）

### <a id="sec-generated-title-7"></a> <a id="initializer-order">初期化子の呼ばれる順序</a>

ちなみに、フィールド初期化子やコンストラクターの実行順序は以下のようになります。

1. コンストラクター初期化子に渡す引数の評価
2. フィールド初期化子
    * フィールドが複数ある場合、上から順
3. 呼び先のコンストラクター
4. 呼び元のコンストラクター

<pre class="source" title="初期化子やコンストラクターの呼び出し順序の例">
<span class="comment">// コンストラクターを空呼び。</span>
<span class="reserved">_</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 呼び出される順序を確認するために呼ぶメソッド。</span>
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">string</span> <span class="variable local">message</span>)
    {
        <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable local">message</span>);
        <span class="control">return</span> <span class="number">0</span>;
    }

    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_member1</span> <span class="operator">=</span> <span class="static"><span class="method">M</span></span>(<span class="string">&quot;フィールド初期化子 1&quot;</span>);
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_member2</span> <span class="operator">=</span> <span class="method"><span class="static">M</span></span>(<span class="string">&quot;フィールド初期化子 2&quot;</span>);

    <span class="reserved">public</span> <span class="type">A</span>() : <span class="reserved">this</span>(<span class="method"><span class="static">M</span></span>(<span class="string">&quot;コンストラクター初期化子引数&quot;</span>))
    {
        <span class="static"><span class="method">M</span></span>(<span class="string">&quot;コンストラクター()&quot;</span>);
    }

    <span class="reserved">public</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">_</span>)
    {
        <span class="static"><span class="method">M</span></span>(<span class="string">&quot;コンストラクター(int)&quot;</span>);
    }
}
</pre>

<pre class="console" title="実行結果">
コンストラクター初期化子引数
フィールド初期化子 1
フィールド初期化子 2
コンストラクター(int)
コンストラクター()
</pre>

この初期化の順序との兼ね合いで、フィールド初期化子ではインスタンス メソッドを呼ぶことができません。
例えば以下のようなコードを認めてしまうと、「まだ初期化していないフィールドを読んでしまう」問題が起きます。

<pre class="source" title="初期化子内ではインスタンス メソッドを呼んではいけない">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// ここで M を呼べてしまうと、未初期化の _otherField を読んでしまう。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_someField</span> <span class="operator">=</span> <span class="method"><span class="error" title="CS0236">M</span></span>();

    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field"><span class="warning" title="CS0649">_otherField</span></span>;
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="field">_otherField</span>;
}
</pre>

## <a id="sec-generated-title-8"></a> <a id="member_initializer"></a>オブジェクト初期化子

<h5 class="version version3">Ver. 3.0</h5>

C# 3.0 から、以下のような記法でメンバーを初期化できるようになりました。

<pre class="source" title="オブジェクト初期化子" lang="">
<code>Point p = <span class="reserved">new</span> Point{ X = 0, Y = 1 };
</code></pre>


ちなみに、このコードの実行結果は以下のようなコードと等価です。

<pre class="source" title="オブジェクト初期化子" lang="">
<code>Point p = <span class="reserved">new</span> Point();
p.X = 0;
p.Y = 1;
</code></pre>


詳細は「[初期化子](../functional/sp3_lambda.md#init)」で説明します。

## <a id="sec-generated-title-9"></a> <a id="dtor"></a>コンストラクターの逆操作

詳しくは後々説明していきますが、コンストラクターと逆の操作を行うものが2つあります。

1つは、ファイナライザー(destructor)です。
プログラムを書く上で、「確保したら必ず後片付けが必要なリソース」と言うものが存在します。
コンストラクターでリソースを確保したら、セットで後片付けを書く場所がファイナライザーです。

<pre class="source" title="コンストラクターの逆操作2: ファイナライザー">
<code><span class="reserved">using</span> System.Buffers;

<span class="reserved">class</span> <span class="type">Resource</span>
{
    <span class="reserved">private</span> <span class="reserved">byte</span>[] _rentalArray;

    <span class="comment">// コンストラクターで「借りてくる」</span>
    <span class="reserved">public</span> Resource() =&gt; _rentalArray = ArrayPool&lt;<span class="reserved">byte</span>&gt;.Shared.Rent(100);

    <span class="comment">// 借りたものは返さないといけない。そのために使うのがファイナライザー</span>
    ~Resource() =&gt; ArrayPool&lt;<span class="reserved">byte</span>&gt;.Shared.Return(_rentalArray);
}
</code></pre>

詳しくは「[ファイナライザー](../resource/rm_destructor.md)」で説明します。

もう1つは、分解(deconstruct)です。
コンストラクターは複数の値を1つの複合型にまとめる操作でもあります。
この意味でのコンストラクターにあたるのが分解処理です。

<pre class="source" title="コンストラクターの逆操作2: 分解">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;

    <span class="comment">// 複数の値を組み合わせる</span>
    <span class="reserved">public</span> Point(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// 複数の値にばらす</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">int</span> x, <span class="reserved">out</span> <span class="reserved">int</span> y) =&gt; (x, y) = (X, Y);
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 組み合わせる</span>
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);

        <span class="comment">// ばらす</span>
        <span class="reserved">var</span> (x, y) = p;
    }
}
</code></pre>

詳しくは「[複合型の分解](../datatype/deconstruction.md)」で説明します。


## <a id="sec-generated-title-10"></a> <a id="target-typed-new"></a>ターゲットからの new 型推論

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 から、状況によっては `new 型名()` の `型名` の部分を省略できるようになりました。
[ターゲット型](../start/misctyperesolution.md#target-type)からの推論が効くことが条件で、
例えば、以下のような書き方をできます。
(この機能を target-typed new と呼んだりします)。

<pre class="source" title="ターゲットからの new 型推論">
<code><span class="comment">// new Person(17, new DateTime(1964, 9, 25)) と同じ意味</span>
<span class="type">Person</span> p = <span class="reserved">new</span>(17, <span class="reserved">new</span>(1964, 9, 25));
 
<span class="reserved">record</span> Person(<span class="reserved">int</span> Age, <span class="type">DateTime</span> Birthday);
</code></pre>

1つ目の `new` は左辺の `Person p` から、2つ目の `new` はコンストラクター引数の `DateTime Birthday` から型を推論できるので、自動的に `Person`、`DateTime` に型を決定します。

ローカル変数の場合には [`var`](../start/sp3_inference.md#type-inference) が使えるのでそれほど便利ではないんですが、[フィールド初期化子](#variable-initializer)やメソッドの引数などでは便利です。

<pre class="source" title="フィールド初期化子で特に便利">
<code><span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="comment">// フィールドに対しては var が使えない。
    // 代わりに new 型推論を使うと便利なことがある(特に、型名が長い時)。</span>
    <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="type">List</span>&lt;(<span class="reserved">int</span> x, <span class="reserved">int</span> y)&gt;&gt; _cache = <span class="reserved">new</span>();
}
</code></pre>

<pre class="source" title="メソッドの引数でも便利">
<code><span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>&gt; options) { }
 
<span class="method">m</span>(<span class="reserved">new</span>()
{
    { <span class="string">&quot;define&quot;</span>, <span class="string">&quot;DEBUG&quot;</span> },
    { <span class="string">&quot;o&quot;</span>, <span class="string">&quot;true&quot;</span> },
    { <span class="string">&quot;w&quot;</span>, <span class="string">&quot;4&quot;</span> },
});
</code></pre>

型名の省略をできるだけの機能で、
元々 `new T(a, b, ...)` みたいに書けて、型 `T` を推論できるのであれば、`new(a, b, ...)` と書くことができます。

<pre class="source" title="型名省略前からダメなものはダメ">
<code><span class="reserved">using</span> System.Globalization;
 
<span class="comment">// new UnicodeCategory() とは元々書けるので、new() と省略可能。</span>
<span class="type">UnicodeCategory</span> c1 = <span class="reserved">new</span>();
 
<span class="comment">// new UnicodeCategory(1) とは元々書けないので、new(1) もダメ。</span>
<span class="type">UnicodeCategory</span> c2 = <span class="error"><span class="reserved">new</span>(1)</span>;
 
<span class="comment">// new (int x, int y)(1, 2) とは書けないんだけど、</span>
<span class="comment">// new ValueTuple&lt;int, int&gt;(1, 2) とは書けて、new(1, 2) はこの意味になる。</span>
(<span class="reserved">int</span> x, <span class="reserved">int</span> y) t = <span class="reserved">new</span>(1, 2);
 
<span class="comment">// 配列とか dynamic は元々 new int[]() とか new dynamic() と書けないので、new() もダメ</span>
<span class="reserved">int</span>[] a = <span class="error"><span class="reserved">new</span>()</span>;
<span class="reserved">dynamic</span> d = <span class="error"><span class="reserved">new</span>()</span>;
</code></pre>

ちなみに、[null 許容型](../resource/sp2_nullable.md) に対する `new()` は、元となる型(`T?` に対する `T` 型) の方の意味になります。

<pre class="source" title="null 許容型に対する new()">
<code><span class="reserved">using</span> System;
 
<span class="reserved">void</span> <span class="method">m</span>(<span class="type">DateTime</span>? d) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(d);
 
<span class="method">m</span>(<span class="reserved">default</span>); <span class="comment">// これは null の意味になる。何も表示されない。</span>
<span class="method">m</span>(<span class="reserved">new</span>()); <span class="comment">// これは new DateTime() の意味になる。 0001/01/01/ 0:00:00</span>
</code></pre>

また、`throw new()` は `throw new Exception()` の意味になったりします。

## <a id="sec-generated-title-11"></a> <a id="primary-constructor">プライマリ コンストラクター</a>

<h5 class="version version12">Ver. 12</h5>

C# 12 から、クラス名の直後に `()` を付けることでコンストラクターを簡素に書けるようになりました。
これを<strong id="key-primary-constructor" class="keyword">プライマリ コンストラクター</strong>(primary constructor: 主要な、第1のコンストラクター)と言います。


例えば、これまで以下のように書いていたコードがあったとします。

<pre class="source" title="既存のコンストラクター">
<span class="reserved">class</span> <span class="type">Person</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="field">Name</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Age</span>;

    <span class="reserved">public</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">name</span>, <span class="reserved">int</span> <span class="variable local">age</span>)
    {
        <span class="field">Name</span> <span class="operator">=</span> <span class="variable local">name</span>;
        <span class="field">Age</span> <span class="operator">=</span> <span class="variable local">age</span>;
    }
}
</pre>

これをプライマリ コンストラクターを使って書きなおすと以下のようになります。

<pre class="source" title="">
<span class="reserved">class</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">name</span>, <span class="reserved">int</span> <span class="variable local">age</span>)
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="field">Name</span> <span class="operator">=</span> <span class="variable local">name</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Age</span> <span class="operator">=</span> <span class="variable local">age</span>;
}
</pre>

プライマリ コンストラクターは、
名前にプライマリ(主要、第1)と付く程度には特別な地位にあります。
構文的に1つしか持てないのはもちろんのこと、
他のコンストラクターから必ず呼び出す必要があります。

例えば以下のコードはコンパイル エラーになりますが、

<pre class="source" title="プライマリ コンストラクターを呼ばないとエラー">
<span class="reserved">class</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">name</span>, <span class="reserved">int</span> <span class="variable local">age</span>)
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="field">Name</span> <span class="operator">=</span> <span class="variable local">name</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Age</span> <span class="operator">=</span> <span class="variable local">age</span>;

    <span class="comment">// プライマリ コンストラクター以外にもコンストラクターを書けるものの、</span>
    <span class="comment">// : this(...) でプライマリ コンストラクターを呼び出す必要がある。</span>
    <span class="reserved">public</span> <span class="type"><span class="error" title="CS8862">Person</span></span>() { } <span class="comment">// このコードでは呼んでいないのでコンパイル エラーを起こす。</span>
}
</pre>

以下のようなコードなら大丈夫です。

<pre class="source" title="他のコンストラクターからプライマリ コンストラクターを呼び出す例">
<span class="reserved">class</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">name</span>, <span class="reserved">int</span> <span class="variable local">age</span>)
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="field">Name</span> <span class="operator">=</span> <span class="variable local">name</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Age</span> <span class="operator">=</span> <span class="variable local">age</span>;

    <span class="reserved">const</span> <span class="reserved">int</span> <span class="static"><span class="constant">UNKNOWN</span></span> <span class="operator">=</span> <span class="operator">-</span><span class="number">1</span>;
    <span class="reserved">const</span> <span class="reserved">string</span> <span class="static"><span class="constant">DEFAULT_NAME</span></span> <span class="operator">=</span> <span class="string">&quot;デフォルトの名無しさん&quot;</span>;

    <span class="reserved">public</span> <span class="type">Person</span>() : <span class="reserved">this</span>(<span class="static"><span class="constant">DEFAULT_NAME</span></span>, <span class="constant"><span class="static">UNKNOWN</span></span>) { }
    <span class="reserved">public</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">name</span>) : <span class="reserved">this</span>(<span class="variable local">name</span>, <span class="constant"><span class="static">UNKNOWN</span></span>) { }
}
</pre>

### <a id="sec-generated-title-12"></a> <a id="vs-record">補足: レコード型との差</a>

C# 9 で[レコード型](../datatype/record.md)が導入された際、
普通のクラスや構造体よりも先にレコード型に対してだけプライマリ コンストラクターが書けました。
順序的に紛らわしくなっていますが、
プロパティの自動生成をしてくれるのはレコード型だけです。

例えば以下のような(通常の)クラスとレコードがあったとして、

<pre class="source" title="プライマリ コンストラクター持ちのクラスとレコード">
<span class="reserved">class</span> <span class="type">Class</span>(<span class="reserved">int</span> <span class="variable local"><span class="warning" title="CS9113">X</span></span>, <span class="reserved">int</span> <span class="variable local"><span class="warning" title="CS9113">Y</span></span>);

<span class="reserved">record</span> <span class="type">Record</span>(<span class="reserved">int</span> <span class="variable local">X</span>, <span class="reserved">int</span> <span class="variable local">Y</span>);
</pre>

これらの型は以下のような感じに展開されます。

<pre class="source" title="コンパイラーが生成するコードの例">
<span class="reserved">class</span> <span class="type">Class</span>
{
    <span class="comment">// 空っぽのコンストラクターができるだけ(引数未使用)。</span>
    <span class="reserved">public</span> <span class="type">Class</span>(<span class="reserved">int</span> <span class="variable local">X</span>, <span class="reserved">int</span> <span class="variable local">Y</span>) { }
}

<span class="reserved">class</span> <span class="type">Record</span>
{
    <span class="comment">// レコード型の場合はコンパイラーがいろいろと生成する。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">init</span>; }

    <span class="reserved">public</span> <span class="type">Record</span>(<span class="reserved">int</span> <span class="variable local">X</span>, <span class="reserved">int</span> <span class="variable local">Y</span>)
    {
        <span class="reserved">this</span><span class="operator">.</span><span class="property">X</span> <span class="operator">=</span> <span class="variable local">X</span>;
        <span class="reserved">this</span><span class="operator">.</span><span class="property">Y</span> <span class="operator">=</span> <span class="variable local">Y</span>;
    }

    <span class="comment">// その他、Equals などもコンパイラーが生成。</span>
}
</pre>

### <a id="sec-generated-title-13"></a> <a id="empty-body">括弧省略</a>

レコード型では、以下のように `{}` を省略可能でした。
(ただし、その場合、`;` を付ける必要があります。)

<pre class="source" title="{} を省略したレコード型">
<span class="comment">// プライマリ コンストラクターだけ持つレコード。</span>
<span class="comment">// 「X 以外にメンバーは不要」みたいなことは多々あり、{} 省略にはそれなりの需要あり。</span>
<span class="reserved">record</span> <span class="type">R1</span>(<span class="reserved">int</span> <span class="variable local">X</span>);

<span class="comment">// プライマリ コンストラクターは引数なしでも OK。</span>
<span class="comment">// なんならプライマリ コンストラクターすらなくても {} 省略可能。</span>
<span class="comment">// あんまり使わないとしても、わざわざ禁止する理由もないので。</span>
<span class="reserved">record</span> <span class="type">R2</span>();
<span class="reserved">record</span> <span class="type">R3</span>;
</pre>

C# 12 で、普通のクラスに対してもプライマリ コンストラクターを書けるようにするにあたって、
この `{}` を省略できる仕様も引き継がれました。
そして、コンストラクターを必要とするクラスと構造体だけではなく、
インターフェイスと列挙型に対しても同様に `{}` 省略を認めることになりました。
(`{}` が `;` に変わるだけなのでたかだか1文字差ですが。)

<pre class="source" title="いろんな型の {} 省略">
<span class="comment">// クラス、構造体、インターフェイス、列挙型で {} 省略が可能に。</span>
<span class="reserved">class</span> <span class="type">C</span>;
<span class="reserved">struct</span> <span class="type struct">S</span>;
<span class="reserved">interface</span> <span class="type">I</span>;
<span class="reserved">enum</span> <span class="type">E</span>;
</pre>

レコード型と比べると用途は少ないですが、例えば、
コード生成前提で「手書きでは何も書くものがない」というような場合に使えなくもないです。
実際例えば、[`JsonSerializable` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.text.json.serialization.jsonserializableattribute)を使うときにそういうコードになったりします。

<pre class="source" title="コード生成だよりで中身空っぽのクラスの例">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>Json<span class="operator">.</span>Serialization;

<span class="comment">// JsonSerializable 属性を付けていると、シリアライズ処理に必要なメンバーをコード生成する。</span>
<span class="comment">// 手書きでは何もする必要がないので空っぽ。</span>
[<span class="type">JsonSerializable</span>(<span class="reserved">typeof</span>(<span class="type">Person</span>))]
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">MyJsonContext</span> : <span class="type">JsonSerializerContext</span>;

<span class="reserved">record</span> <span class="type">Person</span>(<span class="reserved">string</span> <span class="variable local">FirstName</span>, <span class="reserved">string</span> <span class="variable local">LastName</span>);
</pre>

### <a id="sec-generated-title-14"></a> <a id="pc-paramter">プライマリ コンストラクター引数</a>

プライマリ コンストラクターの引数は、クラス内の全域で参照できます。

<pre class="source" title="プライマリ コンストラクターの引数を参照">
<span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Fiedl</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// フィールド初期化子で使う。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span> { <span class="reserved">get</span>; } <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// プロパティ初期化子で使う。</span>

    <span class="comment">// どこでも、何度でも使える。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X2</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X3</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="variable local">x</span>;
}
</pre>

なんなら [`partial`](oo_class.md#partial)で複数のファイルに分割されていても参照できます。

<pre class="source" title="partial で分かれてても参照可">
<span class="comment">// C1.cs</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
}

<span class="comment">// C2.cs</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// OK</span>
}
</pre>

プライマリ コンストラクターの引数を初期化にだけ使っている分には、
通常のコンストラクター引数とほぼ同じです。

例えば、以下のコードの `C1` と `C2` には差がありません。
(クラスの継承が絡まない限りは同じで、[継承があった場合でも初期化の実行順](misc_construct.md#primary-constructor)にちょっと影響があるだけです。)

<pre class="source" title="プライマリ コンストラクターと通常のコンストラクターの比較例">
<span class="reserved">class</span> <span class="type">C1</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;
}

<span class="reserved">class</span> <span class="type">C2</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">_x</span>;

    <span class="reserved">public</span> <span class="type">C2</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
    {
        <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;
    }
}
</pre>

#### <a id="sec-generated-title-15"></a> <a id="capture">キャプチャ</a>

プライマリ コンストラクター引数を初期化時以外でも使う場合には少し事情が変わってきます。

例えば以下のように、メソッドや[プロパティ](oo_property.md)の中で参照した場合、
コンパイラーがフィールドを生成します。

<pre class="source" title="メソッドやプロパティの中でプライマリ コンストラクター引数を参照する例">
<span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="comment">// = (代入)じゃなくて =&gt; (式形式のプロパティ)。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="variable local">x</span>;

    <span class="comment">// 他に、メソッドの中でも参照。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Increment</span>() <span class="operator">=&gt;</span> <span class="operator">++</span><span class="variable local">x</span>;
}
</pre>

こういう操作をキャプチャ(capture: 捕獲)と言います。
「`Count` プロパティや `Increment` メソッドに引数 `x` が捕まる」という意味です。

この例の場合、以下のようなコードと同じ意味になります。

<pre class="source" title="キャプチャの展開結果の例">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// コンパイラー生成のフィールドは実際には &lt;x&gt;P みたいな、通常の C# では書けない名前になる。</span>
    <span class="comment">// かつ、この名前はコンパイラーのバージョンによって変わる可能性あり。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;

    <span class="reserved">public</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="field">_x</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Increment</span>() <span class="operator">=&gt;</span> <span class="operator">++</span><span class="field">_x</span>;
}
</pre>

#### <a id="sec-generated-title-16"></a> <a id="double-field">注意: 2重フィールド生成</a>

ちょっと注意が必要なのは、以下のようなコードを書いてしまうと(おそらく意図せず)フィールドが2重に生成されることがあるという点です。

<pre class="source" title="2重にフィールド生成がかかってしまう例">
<span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="comment">// こちらは「キャプチャ」。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X1</span> <span class="operator">=&gt;</span> <span class="variable local">x</span>;

    <span class="comment">// こちらは自動プロパティの初期化。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X2</span> { <span class="reserved">get</span>; } <span class="operator">=</span> <span class="variable local"><span class="warning" title="CS9124">x</span></span>;
}
</pre>

(ちゃんと警告が出るようになっています。`X2` の行の `= x` のところに警告が出ます。)

このコードはおおむね以下のような意味になります。

<pre class="source" title="2重にフィールド生成がかかった結果の例">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// キャプチャに対応するため、「引数 x に対応するフィールド」を生成。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;

    <span class="comment">// 自動プロパティに対応するため、「プロパティ X2 に対応するフィールド」を生成。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x2</span>;

    <span class="reserved">public</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
    {
        <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">x</span>;
        <span class="field">_x2</span> <span class="operator">=</span> <span class="variable local">x</span>;
    }

    <span class="comment">// 「キャプチャ」だったもの。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X1</span> <span class="operator">=&gt;</span> <span class="field">_x</span>;

    <span class="comment">// 「自動プロパティ」だったもの。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X2</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_x2</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_x2</span> <span class="operator">=</span> <span class="reserved">value</span>; }
}
</pre>

こんな風にフィールドが2個できることは望ましくないので、ちゃんと警告は取りましょう。

#### <a id="sec-generated-title-17"></a> <a id="mutable">注意: 書き換え可能</a>

プライマリ コンストラクターの引数は、
あくまで引数です。
キャプチャが発生すると実質的にはフィールドみたいなものですが、
それでも扱いとしては引数です。

現状、C# には引数を[readonly](../start/sp_const.md#readonly)にする手段がないので、
プライマリ コンストラクター引数は常に書き換え可能です。

<pre class="source" title="プライマリ コンストラクター引数は常に書き換え可能">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="operator">++</span><span class="variable local">x</span>; <span class="comment">// x を書き換え放題。</span>
}

<span class="comment">// 別ファイル</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">0</span>; <span class="comment">// 何だったらだいぶ遠い場所で書き換え可能。</span>
}
</pre>

これが嫌なら、一度 readonly フィールドで受けましょう。

<pre class="source" title="一度 readonly フィールドで受け取る">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
{
    <span class="comment">// フィールドで受け取る。</span>
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">_x</span> <span class="operator">=</span> <span class="warning" title="CS9124"><span class="variable local">x</span></span>;
}

<span class="comment">// 別ファイル</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>() <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">=</span> <span class="number">0</span>; <span class="comment">// これは「2重フィールド警告」が出る(警告を取れば問題を避けれる)。</span>

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>() <span class="operator">=&gt;</span> <span class="field"><span class="error" title="CS0191">_x</span></span> <span class="operator">=</span> <span class="number">0</span>; <span class="comment">// これは「readonly フィールドを書き換えちゃダメ」エラーになる。</span>
}
</pre>
## <a id="exercise"></a>演習問題

### <a id="exercise-str1"></a>問題 1


前節[クラス](oo_class.md)の[問題 1](oo_class.md#exercise-str1)の <code>Point</code> 構造体および <code>Triangle</code> クラスに、
以下のようなコンストラクターを追加せよ。

<pre class="source" title="Point クラスコンストラクター" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 座標値 (x, y) を与えて初期化。
/// &lt;/summary&gt;
/// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
/// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
<span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
</code></pre>


<pre class="source" title="Triangle クラスコンストラクター" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 3つの頂点の座標を与えて初期化。
/// &lt;/summary&gt;
/// &lt;param name="a"&gt;頂点A&lt;/param&gt;
/// &lt;param name="b"&gt;頂点B&lt;/param&gt;
/// &lt;param name="c"&gt;頂点C&lt;/param&gt;</span>
<span class="reserved">public</span> Triangle(Point a, Point b, Point c)
</code></pre>



#### 解答例 1


<pre class="source" title="Point/Triangle クラス" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// 2次元の点をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">struct</span> Point
{
  <span class="reserved">public double</span> x; <span class="comment">// x 座標</span>
  <span class="reserved">public double</span> y; <span class="comment">// y 座標

  /// &lt;summary&gt;
  /// 座標値 (x, y) を与えて初期化。
  /// &lt;/summary&gt;
  /// &lt;param name="x"&gt;x 座標値&lt;/param&gt;
  /// &lt;param name="y"&gt;y 座標値&lt;/param&gt;</span>
  <span class="reserved">public</span> Point(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    <span class="reserved">this</span>.x = x;
    <span class="reserved">this</span>.y = y;
  }

  <span class="reserved">public override string</span> ToString()
  {
    <span class="reserved">return</span> <span class="literal">"("</span> + x + <span class="literal">", "</span> + y + <span class="literal">")"</span>;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 2次元空間上の三角形をあらわす構造体
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Triangle
{
  <span class="reserved">public</span> Point a;
  <span class="reserved">public</span> Point b;
  <span class="reserved">public</span> Point c;

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

  <span class="comment">/// &lt;summary&gt;
  /// 三角形の面積を求める。
  /// &lt;/summary&gt;
  /// &lt;returns&gt;面積&lt;/returns&gt;</span>
  <span class="reserved">public double</span> GetArea()
  {
    <span class="reserved">double</span> abx, aby, acx, acy;
    abx = b.x - a.x;
    aby = b.y - a.y;
    acx = c.x - a.x;
    acy = c.y - a.y;
    <span class="reserved">return</span> 0.5 * Math.Abs(abx * acy - acx * aby);
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

    Console.Write(<span class="literal">"{0}\n"</span>, t.GetArea());
  }
}
</code></pre>
