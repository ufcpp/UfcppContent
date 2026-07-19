---
title: "型推論(暗黙的型付け)と匿名型"
source_url: "https://ufcpp.net/study/csharp/start/sp3_inference/"
content_type: "Article"
published_at: "2009-04-29T00:00:00"
updated_at: "2021-09-22T16:44:12"
tags:
  - "Ver. 3.0"
umbraco_id: 1215
parent_id: 1190
sort_order: 15
aliases:
  - "/csharp/sp3_inference"
  - "/csharp/sp3_inference.html"
  - "/csharp/start/sp3_inference/"
  - "/study/csharp/sp3_inference"
  - "/study/csharp/sp3_inference.html"
---

# 型推論(暗黙的型付け)と匿名型

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
（※修正予定: 
型推論だけに絞って、「変数と式」の直後にでも移動。
匿名型の話は「クラス」の辺りか、「メソッド指向」か「データ処理」の辺りに移動。

<h5 class="version version3">Ver. 3.0</h5>

C# 2.0 以前、「静的型付け言語は冗長な記述が多くてめんどくさい」などと言われることがありました。
例えば、以下の例について考えてみてください。

<pre class="source" title="" lang="">
<code>System.Collections.Generic.<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; list =
  <span class="reserved">new</span> System.Collections.Generic.<span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
</code></pre>


「なんでこんな長ったらしい型名を左辺と右辺の両方で書かなきゃいけないんだ、
どっちか片方書けば、もう片方は推論できるだろう」という話です。

これに対して、C# 3.0 では、可能な限り型推論を行うような構文が追加されています。


##### <a id="sec-generated-title-2"></a>ポイント
* var： 変数の型を推論してくれる。<code>var x = 1;</code>なら x は int になる。

* 暗黙的配列：<code>new int[] { 1, 2, 3 }</code>を<code>new[] { 1, 2, 3 }</code>と書けるようになりました。

* 匿名型：<code>var anonymous = new { X = 1, Y = 2 };</code>みたいに、匿名のクラスを作ることができるようになりました。



##<a id="sec-generated-title-3"></a> <a id="implicit"></a>変数の型推論(変数の暗黙的型付け)
var キーワードを用いて、<strong id="type-inference" class="keyword">型推論</strong>（type inference）して、
暗黙的に型付けされたローカル変数（Implicitly typed local variables）を定義できるようになりました。

<pre class="source" title="var" lang="">
<code><span class="reserved">var</span> n = <span class="literal">1</span>;
<span class="reserved">var</span> x = <span class="literal">1.0</span>;
<span class="reserved">var</span> s = <span class="literal">"test"</span>;
</code></pre>


var を用いる際には、必ず初期値を伴う必要があります。
そして、初期値から、変数の型を自動判別（型推論）してくれます。
上記の例では、
<code>n</code> は <code>int</code>、
<code>x</code> は <code>double</code>、
<code>s</code> は <code>string</code> 型の変数になります。

注意すべき点は、
あくまで型の自動判別・推論であって、
<em>任意の型の値を代入できる万能な変数を作れるわけではない</em>ということです。
したがって、以下のように、初期値を伴わない宣言は（型の推論ができないので）エラーになります。

<pre class="source" title="var（間違い）" lang="">
<code><span class="reserved">var</span> n; <span class="comment">// エラー。初期値が必要。</span>
</code></pre>


<code>TypeName x = new TypeName();</code> というように、
式の両辺に型名を書かないといけないのは冗長ではあります。
var は、この冗長さを省くため、左辺側の型名を省略できる機能だと思ってください。

ただし、冗長性がエラー耐性（2か所とも間違っていないとコンパイル エラーになって間違いに気付く）になっている場合もあるので、
<code>TypeName x = new TypeName();</code> という冗長な書き方も悪いことばかりではありません。


##<a id="sec-generated-title-4"></a> <a id="anonymous"></a>匿名型
C# 3.0 では<strong id="anonytype" class="keyword">匿名型</strong>（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

<pre class="source" title="匿名型" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> { FamilyName = <span class="literal">"糸色"</span>, FirstName=<span class="literal">"望"</span>};
</code></pre>


このようなコードから、自動的に、以下のような型が生成されます。

<pre class="source" title="匿名型によって自動生成されるクラス" lang="">
<code><span class="comment">// ↓この __Anonymous という名前はプログラマが参照できるわけではない。</span>
<span class="reserved">class</span> <span class="type">__Anonymous1</span>
{
  <span class="reserved">private string</span> f1;
  <span class="reserved">private string</span> f2;
  
  <span class="reserved">public</span> __Anonymous1(<span class="reserved">string</span> f1, <span class="reserved">string</span> f2)
  {
    <span class="reserved">this</span>.f1 = f1;
    <span class="reserved">this</span>.f2 = f2;
  }

  <span class="reserved">public string</span> FamilyName
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.f1}
  };
  <span class="reserved">public string</span> FirstName
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>.f2}
  };
  
  <span class="comment">// あと、Equals, GetHashCode, ToString も実装</span>
}
</code></pre>


そして、変数 x に対して、
2つのプロパティ FamilyName と FirstName が使えます。

<pre class="source" title="匿名型の変数" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> { FamilyName = <span class="literal">"糸色"</span>, FirstName=<span class="literal">"望"</span>};

<span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, <em>x.FamilyName, x.FirstName</em>);
</code></pre>



##### <a id="sec-generated-title-5"></a>不変性
自動生成されたクラスを見てのとおり、自動実装されたプロパティには set アクセサーがありません。
要するに、読み取り専用（immutable: 不変）になります。

通常の「[オブジェクト初期化子](../functional/sp3_lambda.md#objectinit)」では、public な set アクセサーを持つプロパティしか初期化できませんでしたが、
匿名型の場合には、コンストラクター呼び出しに置き換えられます。

<pre class="source" title="" lang="">
<code><span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">2</span> };
<span class="comment">// Point p = new Point();
// p.X = 1;
// p.Y = 2;
// と同じ意味。</span>

<span class="reserved">var</span> anonymous = <span class="reserved">new</span> { X = <span class="literal">1</span>, Y = <span class="literal">2</span> };
<span class="comment">// __Anonymous anonymous = new __Anonymous(1, 2);
// みたいなコードが生成される。</span>
</code></pre>



##### <a id="sec-generated-title-6"></a>プロパティ名の省略
ちなみに、以下のように、他のクラスのプロパティを初期化子に渡す場合には、
「プロパティ名 =」の部分を省略することもできます。
（初期化子で渡したプロパティの名前がそのまま匿名クラスでも使われます。）

<pre class="source" title="プロパティ名の省略" lang="">
<code><span class="reserved">struct</span> <span class="type">A</span>
{
  <span class="reserved">public int</span> X { <span class="reserved">set</span>; <span class="reserved">get</span>; }
  <span class="reserved">public int</span> Y { <span class="reserved">set</span>; <span class="reserved">get</span>; }
  <span class="reserved">public int</span> Z { <span class="reserved">set</span>; <span class="reserved">get</span>; }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    A a = <span class="reserved">new</span> A { X = <span class="literal">0</span>, Y = <span class="literal">1</span>, Z = <span class="literal">2</span>};
    <em><span class="reserved">var</span> b = <span class="reserved">new</span> { a.X, a.Y };</em>
    <span class="comment">//↑ new { X = a.X, Y = a.Y } と同じ意味。</span>
    <span class="type">Console</span>.Write(<span class="literal">"{0}, {1}\n"</span>, b.X, b.Y);
  }
}
</code></pre>



##### <a id="sec-generated-title-7"></a>LINQ との組み合わせ
まあ、匿名クラスは、その場限りの使い捨てなクラスになるわけで、
普通はあまり使うような機能ではありません。
基本的には、「[LINQ](../data/sp3_linq.md#linq)」 のための機能だと思っていいでしょう。
例えば、後述するクエリ式中で、以下のように利用します。

<pre class="source" title="匿名型の利用" lang="">
<code><span class="reserved">var</span> list1 =
  <span class="reserved">from</span> p <span class="reserved">in</span> list
  <span class="reserved">where</span> p.id &lt;= <span class="literal">15</span>
  <span class="reserved">orderby</span> p.id
  <em><span class="reserved">select new</span> { p.FamilyName, p.FirstName }</em>;
</code></pre>



##<a id="sec-generated-title-8"></a> <a id="impl_array"></a>暗黙型付け配列
new で配列を作成する際、
型を省略できるようになりました。

<pre class="source" title="配列の暗黙的型付け" lang="">
<code><span class="reserved">int</span>[] array = <em><span class="reserved">new</span>[] {<span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">3</span>, <span class="literal">4</span>}</em>;
</code></pre>


見ての通り、
new の後ろの型を省略しています。
配列の型は、{} の中身の型から推定されます。
この例の場合、中身が 1, 2, 3, 4 といずれも int 型なので、
配列は int[] 型になります。

まあ、これだけだと、
ちょっとタイピングをサボれる程度ですが、
var および「[匿名型](#anonytype)」と組み合わせることによって、
真価が発揮されます。

<pre class="source" title="var と匿名型との組み合わせ" lang="">
<code><span class="reserved">var</span> array = <span class="reserved">new</span>[]
  {
    <span class="reserved">new</span> {X =  <span class="literal">0</span>, Y =  <span class="literal">1</span>},
    <span class="reserved">new</span> {X =  <span class="literal">3</span>, Y = -<span class="literal">1</span>},
    <span class="reserved">new</span> {X =  <span class="literal">7</span>, Y =  <span class="literal">3</span>},
    <span class="reserved">new</span> {X = <span class="literal">13</span>, Y = -<span class="literal">5</span>},
  };

<span class="reserved">foreach</span>(<span class="reserved">var</span> p <span class="reserved">in</span> array) Console.Write(<span class="literal">"{0}\n"</span>, p);
</code></pre>


配列宣言の中身が匿名なんだから、
new の後ろにどういう型名を書いたらいいかわかるはずがないですからね。
