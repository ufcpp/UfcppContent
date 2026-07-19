---
title: "継承"
source_url: "https://ufcpp.net/study/csharp/oop/oo_inherit/"
content_type: "Article"
published_at: "2002-08-05T00:00:00"
updated_at: "2019-05-05T00:00:00"
tags: []
umbraco_id: 1262
parent_id: 1248
sort_order: 9
aliases:
  - "/csharp/oo_inherit"
  - "/csharp/oo_inherit.html"
  - "/csharp/oop/oo_inherit/"
  - "/study/csharp/oo_inherit"
  - "/study/csharp/oo_inherit.html"
---

# 継承

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="derive" class="keyword">継承</strong>（inheritance）とはオブジェクト指向の中核を担う概念で、
あるクラスから性質を受け継いだ新しいクラスを作ることです。
継承は<em>派生</em>（derivation）とも呼ばれます。


##### <a id="sec-generated-title-2"></a>ポイント

* オブジェクト指向の中核概念その2: 継承。

* 「人間」⊃「学生」のように、包含関係のあるものを表現する方法。

* 「学生は人間を継承する」、「学生は人間から派生する」などと言う。

* class Person { ... } に対して、class Student : Person { ... } と書く。



## <a id="sec-generated-title-3"></a> <a id="about"></a>継承関係とは

継承関係の例として、「人間」と「学生」という2つのクラスについて考えて見ましょう。

「学生」は「人間」の一部です。
すなわち、「学生」ならば必ず「人間」としての特徴を備えています。
それとは逆に「人間」だからといって必ずしも「学生」であるとはいえません。
つまり、「学生」は「人間」の特別な場合である(「人間⊃学生」という包含関係が成り立つ)といえます。

例えば、「人間」には「名前」、「年齢」などの属性があります。
(ここでは簡単化のためこの2つの属性のみを考えます。)
「学生」は人間の一部分ですから、当然この2つの属性を備えています。
それに加え、「学生」は「学籍番号」という属性を持っています。

このように、あるクラス A がクラス B を包含するような関係にあるとき、
この関係を<em>継承関係</em>と呼び、
「B は A を継承する」とか「B は A から派生する(導出される)」といいます。
また、このとき、クラス A のことを「<strong id="supclass" class="keyword">基底クラス</strong>（base class）」
または「<em>スーパークラス</em>（super class）」と呼び、
クラス B のことを「<strong id="subclass" class="keyword">派生クラス</strong>（derived class）」
または「<em>サブクラス</em>（sub class）」と呼びます。

<figure>
	[![「人間」と「学生」の包含関係](../../../../assets/media/ufcpp2000/csharp/fig/inheritance.png)](../../../../assets/media/ufcpp2000/csharp/fig/inheritance.png)
	<figcaption>「人間」と「学生」の包含関係</figcaption>
</figure>



## <a id="sec-generated-title-4"></a> <a id="inherit"></a>クラスの継承

C# を始めとするオブジェクト指向言語では、
このような継承関係を表現するため、
あるクラスが他のクラスを継承するための構文が用意されています。
C# でクラスの継承を行うためには、クラス定義の際に以下のように書きます。

<pre class="source" title="クラスの継承" lang="">
<code><span class="reserved">class</span> <span class="input">派生クラス名</span> : <span class="input">基底クラス名</span>
{
  <span class="input">派生クラスの定義</span>
}
</code></pre>


クラスの継承の例として、先ほどの「人間」と「学生」にあたるクラス
<code>Person</code> と <code>Student</code> を
C# でクラス化すると以下のようになります。

<pre class="source" title="継承の例。人間と学生。" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span>    age;  <span class="comment">// 年齢</span>
}

<em><span class="reserved">class</span> Student : Person</em>
{
  <span class="reserved">public int</span>    id;   <span class="comment">// 学籍番号</span>
}
</code></pre>


クラス利用側のコードは以下のようになります。

<pre class="source" title="" lang="">
<code>Person p1 = <span class="reserved">new</span> Person();
p1.name = <span class="literal">"天野舞耶"</span>;
p1.age  = 23;

Student s1 = <span class="reserved">new</span> Student();
s1.name = <span class="literal">"周防達也"</span>; <span class="comment">// Person のメンバーをそのまま利用出来る</span>
s1.age  = 18;
s1.id   = 50012;

Person p2 = s1; <span class="comment">// Student は Person として扱うことが出来る。</span>

Student s2 = p1; <span class="comment">// でも、Person は Student として扱っちゃ駄目。
//↑この1行はエラーになる。</span>
</code></pre>


C# では、派生クラスのインスタンスは基底クラスの変数に代入することが出来ます。
これは、例えば、学生ならば必ず人間であるため、「学生は人間として扱うことができる」ということです。
それとは逆に、基底クラスのインスタンスを派生クラスの変数に代入することは出来ません。
すなわち、すべて人間が学生というわけではないですから、「人間を無条件に学生として扱ってはいけない」ということです。


## <a id="sec-generated-title-5"></a> <a id="object"></a>object型

C# では、基底クラスを指定せずに作成した型は全て自動的に <code>object</code> 型を継承することになります。
(構造体等の値型は明示的に他の型を継承できないので、必ず <code>object</code> を継承します。)
つまり、C# における全ての型は <code>object</code> の派生クラスになります。

<code>object</code> 型には <code>Equals</code> (他のインスタンスとの比較)や <code>ToString</code> (インスタンスを文字列化する)等の機能があります。


## <a id="sec-generated-title-6"></a> <a id="ctor"></a>基底クラスのコンストラクタ呼び出し

派生クラスのインスタンスが生成される際、
派生クラスのコンストラクタが呼び出される前に
基底クラスのコンストラクタが呼び出されます。

例えば、以下のようなコードを実行すると、
まず、<code>Base</code> クラスのコンストラクタが呼ばれ、
その後 <code>Derived</code> クラスのコンストラクタが呼ばれます。

<pre class="source" title="呼び出し順序">
<span class="reserved">_</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span>();

<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">Base</span>()
    {
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Base&quot;</span>);
    }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">Derived</span>()
    {
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Derived&quot;</span>);
    }
}
</pre>

<pre class="console" title="呼び出し順序">
Base
Derived
</pre>

一方で、フィールド初期化子の呼び出し順序は逆で、派生クラス側の初期化子の方が先に実行されます。
結果的に、実行順序は以下の順序になります。

1. 派生クラスのフィールド初期化子
2. 基底クラスのフィールド初期化子
3. 基底クラスのコンストラクター
4. 派生クラスのコンストラクター

<pre class="source" title="フィールド初期化子を含む場合の実行順序">
<span class="reserved">_</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span>();

<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="comment">// 呼び出される順序を確認するために呼ぶメソッド。</span>
    <span class="reserved">protected</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">string</span> <span class="variable local">message</span>)
    {
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">message</span>);
        <span class="control">return</span> <span class="number">0</span>;
    }

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X</span> <span class="operator">=</span> <span class="static"><span class="method">M</span></span>(<span class="string">&quot;Base フィールド初期化子&quot;</span>);

    <span class="reserved">public</span> <span class="type">Base</span>()
    {
        <span class="method"><span class="static">M</span></span>(<span class="string">&quot;Base コンストラクター&quot;</span>);
    }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Y</span> <span class="operator">=</span> <span class="method"><span class="static">M</span></span>(<span class="string">&quot;Derived フィールド初期化子&quot;</span>);

    <span class="reserved">public</span> <span class="type">Derived</span>()
    {
        <span class="static"><span class="method">M</span></span>(<span class="string">&quot;Derived コンストラクター&quot;</span>);
    }
}
</pre>

<pre class="console" title="フィールド初期化子を含む場合の実行順序">
Derived フィールド初期化子
Base フィールド初期化子
Base コンストラクター
Derived コンストラクター
</pre>

あと、「[コンストラクター](oo_construct.md#initializer-order)」で説明している「初期化の順序との兼ね合い」も改めて問題になります。
フィールド初期化子でインスタンス メソッドを呼べてしまうと、
以下のように「基底クラスの未初期化のフィールドを読めてしまう」ということが起きます。
(クラスが分かれているので、派生がない場合よりも深刻です。)

<pre class="source" title="初期化子内ではインスタンス メソッドを呼んではいけない">
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field"><span class="warning" title="CS0649">BaseField</span></span>;

    <span class="reserved">protected</span> <span class="reserved">int</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="field">BaseField</span>;
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// ここで M を呼べてしまうと、未初期化の BaseField を読んでしまう。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">DerivedField</span> <span class="operator">=</span> <span class="method"><span class="error" title="CS0236">M</span></span>();
}
</pre>


## <a id="sec-generated-title-7"></a> <a id="base_ctor"></a>基底クラスのコンストラクタを明示的に呼び出す

先ほど行ったように、派生クラスのインスタンスを生成する際、
自動的に基底クラスのコンストラクタも呼び出されます。
しかし、この際、呼び出されるコンストラクタは引数なしのコンストラクタになります。

基底クラスの引数つきのコンストラクタを呼び出すためには、
以下のように自分でコードを書いて明示的に基底クラスのコンストラクタを呼び出す必要があります。

<pre class="source" title="基底クラスのコンストラクタ呼び出し" lang="">
<code><span class="input">派生クラスのコンストラクタ</span>(<span class="input">引数</span>) : <span class="reserved">base</span>(<span class="input">基底クラスに渡したい引数</span>)
{
}
</code></pre>


例として、先ほどの <code>Person</code> クラスと <code>Student</code> クラスにコンストラクタを追加してみましょう。
ついでに実装の隠蔽も行った結果を以下に示します。

<pre class="source" title="Person と Student にコンストラクタを追加" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">private string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">private int</span>    age;  <span class="comment">// 年齢</span>

  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age;
  }

  <span class="reserved">public string</span> Name
  {
    <span class="reserved">set</span>{<span class="reserved">this</span>.name = value;}
    <span class="reserved">get</span>{<span class="reserved">return this</span>.name;}
  }

  <span class="reserved">public int</span> Age
  {
    <span class="reserved">set</span>{<span class="reserved">this</span>.age = value;}
    <span class="reserved">get</span>{<span class="reserved">return this</span>.age;}
  }
}

<span class="reserved">class</span> Student : Person
{
  <span class="reserved">private int</span>    id;   <span class="comment">// 学籍番号</span>

  <span class="reserved">public</span> Student(<span class="reserved">string</span> name, <span class="reserved">int</span> age, <span class="reserved">int</span> id) : <span class="reserved">base</span>(name, age)
  {
    <span class="reserved">this</span>.id   = id;
  }

  <span class="reserved">public int</span> Id
  {
    <span class="reserved">set</span>{<span class="reserved">this</span>.id = value;}
    <span class="reserved">get</span>{<span class="reserved">return this</span>.id;}
  }
}
</code></pre>

この構文は[コンストラクター初期化子](oo_construct.md#initializer)の一種です。
`this`の方と区別して base 初期化子と呼ぶ場合もあります。

## <a id="sec-generated-title-8"></a> <a id="protected"></a>protected

「[実装の隠蔽](oo_conceal.md)」で、クラスのメンバーのアクセスレベルについて説明しました。その際、public と private については説明しましたが、ここでは継承と関係の深い protected について説明します。

public はクラス内外とわずどこからでもアクセス可能なレベルで、
private はクラス内部からのみアクセス可能なレベルです。
これらに対し、protected はクラスとそのクラスを継承する派生クラス内からアクセス可能なレベルです(private は派生クラス内からアクセスできない)。
以下に例を挙げます。

<pre class="source" title="protected" lang="">
<code><span class="reserved">class</span> Base
{
  <span class="reserved">public    int</span> public_val;
  <span class="reserved">protected int</span> protected_val;
  <span class="reserved">private   int</span> private_val;

  <span class="reserved">void</span> BaseTest()
  {
    public_val    = 0; <span class="comment">// OK</span>
    protected_val = 0; <span class="comment">// OK</span>
    private_val   = 0; <span class="comment">// OK</span>
  }
}

<span class="reserved">class</span> Derived : Base
{
  <span class="reserved">void</span> DerivedTest()
  {
    public_val    = 0; <span class="comment">// OK</span>
    <em>protected_val = 0; <span class="comment">// OK   (protected は派生クラスからアクセス可能)</span></em>
    private_val   = 0; <span class="comment">// エラー(private   は派生クラスからアクセス不能)</span>
  }
}

<span class="reserved">class</span> Test
{
  <span class="reserved">static void</span> Main()
  {
    Base b = <span class="reserved">new</span> Base();

    b.public_val    = 0; <span class="comment">// OK</span>
    b.protected_val = 0; <span class="comment">// エラー(protected は外部からアクセス不能)</span>
    b.private_val   = 0; <span class="comment">// エラー(private   は外部からアクセス不能)</span>
  }
}
</code></pre>



## <a id="sec-generated-title-9"></a> <a id="conceal"></a>基底クラスのメンバーの隠蔽

派生クラスには自由にメンバーを追加することが出来ますが、
基底クラスの public メンバーと同名のメンバーを再定義してしまうと
基底クラスのメンバーが新しく追加されたメンバーに隠れてしまいます。
このような状態を「基底クラスのメンバーを隠蔽する」といいます。

<pre class="source" title="基底クラスのメンバーの再定義" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> Base
{
  <span class="reserved">public void</span> Test()
  {
    Console.Write(<span class="literal">"Base.Test()\n"</span>);
  }
}

<span class="reserved">class</span> Derived : Base
{
  <span class="reserved">public void</span> Test() <span class="comment">//基底クラスの Test() と同名のメソッド</span>
  {
    Console.Write(<span class="literal">"Derived.Test()\n"</span>);
  }
}

<span class="reserved">class</span> Test
{
  <span class="reserved">static void</span> Main()
  {
    Base b = <span class="reserved">new</span> Base();
    b.Test(); <span class="comment">// Base の Test が呼ばれる</span>

    Derived d = <span class="reserved">new</span> Derived();
    d.Test(); <span class="comment">// Derived の Test が呼ばれる</span>

    ((Base)d).Test();
    <span class="comment">// Base に キャストしてから Test を呼ぶと Base の Test が呼ばれる</span>
  }
}
</code></pre>


<pre class="console" title="">
Base.Test()
Derived.Test()
Base.Test()
</pre>


ここで、プログラマが意図して基底クラスのメンバーの隠蔽を行う分には何の問題もないんですが、
基底クラスに同名のメソッドがあることに気づかずにメソッドを追加してしまうと、
意図しない動作を引き起こしてしまうことがあります。
そこで、C#では基底クラスのメンバーの隠蔽を行う場合、メソッドにnew修飾子を付ける必要があります。
(new修飾子を付けていない場合、コンパイラが警告を出します。)

<pre class="source" title="new修飾子" lang="">
<code><span class="reserved">class</span> Derived : Base
{
  <span class="comment">//基底クラスのメンバーを隠蔽するには new を付ける必要がある。</span>
  <span class="reserved">public <em>new</em> void</span> Test()
  {
    Console.Write(<span class="literal">"Derived.Test()\n"</span>);
  }
}
</code></pre>

### <a id="sec-generated-title-10"></a> <a id="base-access"></a>base アクセス

ちなみに、`base` キーワードを使って基底クラスのメンバーを参照できます。
この機能を使って、以下のように、隠蔽されたメンバーを呼び出すこともできます。

<pre class="source" title="隠蔽された基底クラスのメンバー呼び出し" lang="">
<code><span class="reserved">class</span> Base
{
  <span class="reserved">public void</span> Test()
  {
    Console.Write(<span class="literal">"Base.Test()\n"</span>);
  }
}

<span class="reserved">class</span> Derived : Base
{
  <span class="reserved">public new void</span> Test() <span class="comment">//基底クラスの Test() と同名のメソッド</span>
  {
    Console.Write(<span class="literal">"Derived.Test()\n"</span>);
  }

  <span class="reserved">public void</span> Test2()
  {
<em>    <span class="reserved">this</span>.Test(); <span class="comment">// Derived の Test が呼ばれる。</span>
    <span class="reserved">base</span>.Test(); <span class="comment">// Base の Test が呼ばれる。</span></em>
  }
}
</code></pre>

ちなみに、[`this`アクセス](oo_class.md#this-access)と同様に、`base`アクセスでも[インデクサー](oo_indexer.md)にアクセスできます。
(一方で、[拡張メソッド](../functional/sp3_extension.md)の呼び出しには使えません。)

<pre class="source" title="base を使ってインデクサーにアクセスする例">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable">i</span>] =&gt; <span class="variable">i</span>;
}
 
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable">i</span>] =&gt; <span class="reserved">base</span>[<span class="variable">i</span>]; <span class="comment">// Base のインデクサーが呼ばれる</span>
}
</code></pre>

### <a id="sec-generated-title-11"></a> <a id="non-virtual-base-access"></a>base(T) アクセス

<h5 class="version version8">Ver. 未定</h5>

※ 本節の `base(T)` の機能は、元々 C# 8.0 で入る予定だったものが、9.0 以降で改めて検討しなおすことになったものです。
C# 8.0 のプレビュー版で一時的に使える時期はありましたが、リリース版ではいったん削除されています。

前節の `base` アクセスでは、継承に階層があるとき、特定のクラスの実装を呼び分けるということはできません。
常に、「一番近いもの」が選ばれて呼び出されます。

これに対して、将来的には、`base(T)` という形で、特定のクラスを明示的に指定できるようになりました。
(主に[インターフェイスのデフォルト実装](oo_interface.md#dim)のための機能でしたが、クラスに対しても認められています。)

<pre class="source" title="base(T) アクセスの例">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A.M&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;B.M&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">B</span>
{
    <span class="comment">// 今までであれば、必ず「自分に近い方の M」が呼ばれる。</span>
    <span class="comment">// この場合は B.M。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="reserved">base</span>.<span class="method">M</span>();
 
    <span class="comment">// この書き方なら絶対に A.M が呼ばれる。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M1</span>() =&gt; <em><span class="reserved">base</span>(<span class="type">A</span>)</em>.<span class="method">M</span>();
 
    <span class="comment">// この書き方なら絶対に B.M が呼ばれる。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M2</span>() =&gt; <em><span class="reserved">base</span>(<span class="type">B</span>)</em>.<span class="method">M</span>();
}
</code></pre>

C# 8.0 から外れたのは、以下のような `base` の方との挙動の差が問題になったからです。

前述の通り `base` は「一番近いものを自動的に選ぶ」という性質があります。
これは、
「コンパイルした時には `B.M` はなかった/あったけど、
実行時には `B.M` が追加/削除されている」
というような状況でも問題なく実行できます。
`B.M` があればそれが呼ばれるし、なければ `A.M` を探しに行きます。

一方、C# 8.0 に間に合うようにこの `base(T)` アクセスを実装しようと思うと、「基底をたどって探す」という挙動ができませんでした。
`base(B).M()` と書くと、クラス `B` 自体しか見ません。
コンパイル時に `B` 自体に `M` がなければコンパイル エラーになりますし、
先ほどの「コンパイルした時にはあったものが削除された」みたいなシチュエーションでは実行時に例外が発生します。

この挙動の差を埋めようと思うと .NET ランタイム自体にそこそこ大変な修正が必要で、
後々改めて取り組むことになりました。
(その後結局あまり進んでいなくて、.NET 7 / C# 11 の時点でも未実装です。)

## <a id="sec-generated-title-12"></a> <a id="sealed"></a>sealed

C# のクラスは基本的に常に継承して派生クラスを作ることができるのですが、
場合によっては絶対に継承されたくないと言うこともあります。
このような場合、クラス定義時に sealed （封印された）というキーワードをつけることで、
継承を禁止することができます。

<pre class="source" title="sealed クラス" lang="">
<code><span class="reserved"><em>sealed</em> class</span> SealedClass { }

<span class="reserved">class</span> Derived : SealedClass <span class="comment">// SealedClass は継承不可なので、エラーになる。</span>
{
}
</code></pre>

## <a id="sec-generated-title-13"></a> <a id="single"></a>単一継承

C#のクラス継承では、1つのクラスしか継承できません。これを単一継承(single inheritance)と呼びます。
(逆を意味するのは多重継承(multiple inheritance)で、「C#では多重継承を認めていない」などと言ったりもします。)
つまり、以下のように、2つ以上のクラスを継承しようとするとコンパイル エラーになります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">class</span> <span class="type">Base1</span> { }
<span class="reserved">class</span> <span class="type">Base2</span> { }
<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base1</span>, <span class="type">Base2</span> { }
</code></pre>

別項で説明する[インターフェイス](oo_interface.md)であればこの制限はなく、いくつでも実装できます。
