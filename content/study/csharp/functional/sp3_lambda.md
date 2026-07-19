---
title: "ラムダ式"
source_url: "https://ufcpp.net/study/csharp/functional/sp3_lambda/"
content_type: "Article"
published_at: "2009-04-29T00:00:00"
updated_at: "2023-10-25T21:55:23"
tags:
  - "Ver. 3.0"
umbraco_id: 1280
parent_id: 1275
sort_order: 6
aliases:
  - "/csharp/functional/sp3_lambda/"
  - "/csharp/sp3_lambda"
  - "/csharp/sp3_lambda.html"
  - "/study/csharp/sp3_lambda"
  - "/study/csharp/sp3_lambda.html"
---

# ラムダ式

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<h5 class="version version3">Ver. 3.0</h5>

<strong id="lambda" class="keyword">ラムダ式</strong>（lambda expression）と言うのは、
関数型言語と呼ばれるような種類のプログラミング言語における用語なのですが、
関数（メソッド）を整数などの変数と全く同列に扱う手法のことです。

C# 3.0 で導入されたラムダ式は、
以下のようなものだと思ってください。

1. 「[デリゲート](sp_delegate.md#delegate)」に対して代入すると、「[匿名メソッド式](sp_delegate.md#anonymous)」と同じ扱いになる。

2. Expression 型の変数に代入すると、式木（expression tree）データになる。



##### <a id="sec-generated-title-2"></a>ポイント
* C# 3.0 で導入されたラムダ式には、2通りの意味があります。
    * 匿名メソッドを 2.0 の頃の記法より簡単に書ける。

    * 上述の匿名メソッドと同じ記法で式木を作れる。



* 例：<code>Func&lt;int, int&gt; square = x =&gt; x * x;</code>



##<a id="sec-generated-title-3"></a> <a id="anonymous"></a>匿名メソッドの記法の簡略化
まず、1つ目。
ラムダ式は、
C# 2.0 の匿名メソッドをさらに簡便な記法で書けるようにするものとして使われます。
先に概要を書いてしまうと、以下のような感じ。

* 匿名メソッドの定義から、delegate とか { return } とかの記述を省略できる。

* 型推論機構が働く。


（2.0 の）匿名メソッド構文でできることはラムダ式でもできます。
C# 3.0 の開発者も、「もし、ラムダ式が先に導入されていれば、匿名メソッドの構文は不要だった」と言っています。

C# 2.0 までの匿名メソッドは、例えば、以下のような書き方をしていました。

<pre class="source" title="C# 2.0 の匿名メソッド" lang="">
<code><span class="reserved">delegate</span>(<span class="reserved">int</span> n)
{
  <span class="reserved">return</span> n &gt; <span class="literal">0</span>;
}
</code></pre>


この匿名メソッドをラムダ式を使って書き直すと、以下のようになります。

<pre class="source" title="ラムダ式" lang="">
<code>(<span class="reserved">int</span> n) =&gt; { <span class="reserved">return</span> n &gt; <span class="literal">0</span>; };
</code></pre>


{} の中身が単文の場合には、{} と return も省略できます。

<pre class="source" title="ラムダ式" lang="">
<code>(<span class="reserved">int</span> n) =&gt; n &gt; <span class="literal">0</span>;
</code></pre>


要するに、記法としては、以下のようになります。

<pre class="source" title="ラムダ式の記法" lang="">
<code><span class="input">引数リスト</span> =&gt; <span class="input">式</span>
</code></pre>


ちなみに、文脈的に引数の型が明らかな場合、
型は省略できます。
（var と似たような型推定機能が働く。）
例えば、以下のようなデリゲートがあるとき、

<pre class="source" title="デリゲート Pred" lang="">
<code><span class="reserved">delegate bool</span> <span class="type">Pred</span>(<span class="reserved">int</span> n);
</code></pre>


このデリゲートに対する代入式中にラムダ式を書く場合、
引数の型は int であることが明らかなので、
int を省略して以下のように書くことができます。
（n の型はコンパイラが推論してくれます。）

<pre class="source" title="引数の型推定" lang="">
<code><span class="type">Pred</span> p = <em>n =&gt; n &gt; <span class="literal">0</span></em>;
</code></pre>


あと、いちいちデリゲートを定義するのは面倒なので、
.NET Framework 3.5 では、Func という名前のデリゲートが標準で用意されています。
Func は「[ジェネリック](../oop/sp2_generics.md#generics)」を使って定義されていて、
例えば、上述の例の Pred デリゲートのように、
int 型の引数を1つとって、bool 型を返すようなデリゲートを以下のように表現できます。

<pre class="source" title="Func デリゲート" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt;
</code></pre>


式が複数になる場合は省略せずに {} でくくります。
（この場合は、{} の中身は匿名デリゲートと同じ書き方をする。return も書く必要あり。）

<pre class="source" title="ラムダ式（複文）" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt; f =
  (x, y) =&gt;
  {
    <span class="reserved">int</span> sum = x + y;
    <span class="reserved">int</span> prod = x * y;
    <span class="reserved">return</span> sum * prod;
  };
</code></pre>



##<a id="sec-generated-title-4"></a> <a id="expression"></a>式木
一方、2つ目に関してですが、こちらは完全に新機能で、ラムダ式特有のものです。
匿名メソッドと違って、
ラムダ式は本当に<strong id="exp_tree" class="keyword">式木</strong>（expression tree）データとして扱うこともできます。

上述の例の
<code>Pred p = n =&gt; n &gt; 0;</code> のように、
デリゲートに代入する場合には、
ラムダ式は匿名メソッドと同じ扱い、
すなわち、コンパイル後には実行コードの状態になっています。

これに対して、ラムダ式を
Expression 型の変数に代入すると、式木データとして扱うことができ、
以下のように式中の項を取り出したりといった操作が可能です。

<pre class="source" title="ラムダ式をデータとして扱う" lang="">
<code><span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt;&gt; e = n =&gt; n &gt; <span class="literal">0</span>;
<span class="type">BinaryExpression</span> lt = (<span class="type">BinaryExpression</span>)e.Body;
<span class="type">ParameterExpression</span> en = (<span class="type">ParameterExpression</span>)lt.Left;
<span class="type">ConstantExpression</span> zero = (<span class="type">ConstantExpression</span>)lt.Right;
</code></pre>


インタプリタ型の関数型言語には、実行コードとデータを区別しないものがあって、
ラムダ式をあるときには実行コードとして、またあるときにはデータとして利用するということができました。
C# では「実行コードとデータを区別しない」というわけにはいかないですし、
デリゲートに代入するか Expression 型に代入するかによってコンパイル結果を変えることで、
関数型言語と似たような動作を実現しています。

ただし、ラムダ式をデリゲートに代入する場合と違って、
式木には少し制約があります。
式木にできるのは、単文の（{} を使わない）ラムダ式だけです。
以下の例では、1つ目のラムダ式はコンパイル可能ですが、2つ目はエラーになります。

<pre class="source" title="{} を使って書いたラムダ式は式木にできない" lang="">
<code><span class="comment">// ↓ これは OK</span>
<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt;&gt; p = n =&gt; n &gt; <span class="literal">0</span>;

<span class="comment">// ↓ これは「式木に変換できません」と怒られる</span>
<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt; f =
  (x, y) =&gt;
  {
      <span class="reserved">int</span> sum = x + y;
      <span class="reserved">int</span> prod = x * y;
      <span class="reserved">return</span> sum * prod;
  };
</code></pre>


要するに、単文で書けるものしか式木にできません。
したがって、四則演算やメソッドコールは式木にできるんですが、
for や while などの制御構文は式木にできません。
（Expression 型にも、for や while に相当するノードはない。）

ちなみに、LINQ to SQL では、
このラムダ式を式木として扱う機能を使って、LINQ クエリ式の条件式などを式木データとして受け取って、
それを SQL クエリに変換してデータベースに問い合わせをかけるというようなことをしているようです。

例えば、以下のようなクエリ式を書いたとすると、

<pre class="source" title="LINQ to SQL の例" lang="">
<code><span class="reserved">var</span> q =
  <span class="reserved">from</span> c <span class="reserved">in</span> db
  <span class="reserved">where</span> c.City == <span class="literal">"London"</span>
  <span class="reserved">select new</span> {c.City};

<span class="reserved">foreach</span> (<span class="reserved">var</span> city <span class="reserved">in</span> q)
  <span class="input">...</span>
</code></pre>


db.Where や db.Select では、
データベースサーバに対して以下のような SQL を発行するしくみになっています。

<pre class="source" title="上述のクエリ式から作られる SQL 文" lang="">
<code>SELECT TOP 1 [t0].[City]
FROM [Customers] AS [t0]
WHERE [t0].[City] = @p0
</code></pre>


こういう動作は、<code>c.City == "London"</code> の部分をデリゲート（要するに実行コード）として受け取っていてはできません。式木データとして受け取って、その中身を見ながら SQL 文を作ります。


##<a id="sec-generated-title-5"></a> <a id="init"></a>初期化子
###<a id="sec-generated-title-6"></a> <a id="object-initializer"></a>オブジェクト初期化子
C# 3.0 では、オブジェクトの初期化を以下のような記法でできるようになりました。
このような記法を<strong id="objectinit" class="keyword">オブジェクト初期化子</strong> （object initializer）と呼びます。

<pre class="source" title="オブジェクト初期化子" lang="">
<code><span class="type">Point</span> p = <span class="reserved">new</span> <span class="type">Point</span>{ X = <span class="literal">0</span>, Y = <span class="literal">1</span> };
</code></pre>


ちなみに、このコードの実行結果は以下のようなコードと等価です。

<pre class="source" title="オブジェクト初期化子" lang="">
<code><span class="type">Point</span> p = <span class="reserved">new</span> <span class="type">Point</span>();
p.X = <span class="literal">0</span>;
p.Y = <span class="literal">1</span>;
</code></pre>


この等価なコードを見ればわかると思いますが、
オブジェクト初期化子で指定できるのは public なメンバー変数またはプロパティのみです。
（初期化子を書く場所によっては protected や internal も可。
とにかく、初期化子を書いた場所からアクセスできる変数・プロパティのみ。）

ただし、初期化子を使うと、
プロパティへの値の代入を単文で書けるようになります。
これで何が嬉しいかというと、<em>クラスのメンバー変数の初期化や、式木への代入が可能になります</em>。

<pre class="source" title="式木への代入時のオブジェクト初期化子" lang="">
<code><span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="type">Point</span>&gt;&gt; f = () =&gt; <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">0</span> };
<span class="comment">// ↑式木には単文のラムダ式しか代入できない。

// 要するに、以下のような書き方はコンパイルエラーになる。</span>
<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="type">Point</span>&gt;&gt; f = () =&gt;
{
  <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>();
  p.X = <span class="literal">0</span>;
  p.Y = <span class="literal">0</span>;
  <span class="reserved">return</span> p;
}
</code></pre>


<pre class="source" title="クラスのメンバー変数初期化時のオブジェクト初期化子" lang="">
<code><span class="reserved">class</span> <span class="type">Triangle</span>
{
    <span class="reserved">public</span> <span class="type">Point</span> A = <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">0</span> };
    <span class="reserved">public</span> <span class="type">Point</span> B = <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">0</span> };
    <span class="reserved">public</span> <span class="type">Point</span> C = <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">1</span> };
    <span class="comment">// ↑メンバー変数の初期化に複文は書けないの。</span>
}
</code></pre>

####<a id="sec-generated-title-7"></a> <a id="trailing-comma"></a>末尾コンマ
オブジェクト初期化子では、[配列の初期化子](../structured/st_array.md#use)と同様に、末尾のコンマはあってもなくてもかまいません。
以下の2行は同じ意味になります。

<pre class="source" title="初期化子の末尾コンマ">
<code><span class="reserved">var</span> <span class="variable">p1</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 0, Y = 1 };
<span class="reserved">var</span> <span class="variable">p2</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 0, Y = 1<em>,</em> };
</code></pre>

これは、後述するコレクション初期化子やインデックス初期化子でも同様です。

###<a id="sec-generated-title-8"></a> <a id="collection-initializer"></a>コレクション初期化
また、コレクションの初期化を以下のような記法でできるようになりました。
こちらは<strong id="collectioninit" class="keyword">コレクション初期化子</strong>（collection initializer）と呼びます。

<pre class="source" title="コレクション初期化子" lang="">
<code>List&lt;<span class="reserved">int</span>&gt; list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; {<span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">3</span>};
</code></pre>


要するに、配列と同じような初期化記法を、任意のコレクションクラス（System.Collections.IEnumerable インターフェースを実装していて、Add メソッドを持つクラス）に対して行うことができます。
ちなみに、このコードは以下のようなコードと等価です。

<pre class="source" title="コレクション初期化子" lang="">
<code><span class="type">List</span>&lt;<span class="reserved">int</span>&gt; list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
list.Add(<span class="literal">1</span>);
list.Add(<span class="literal">2</span>);
list.Add(<span class="literal">3</span>);
</code></pre>


このようなリスト型のコレクションだけでなく、
IDictionary&lt;TKey,TValue&gt; のような辞書クラスに対しても、
以下のような記法で初期化ができます。
（この場合、2引数の Add メソッドが呼ばれます。）

<pre class="source" title="コレクション初期化子" lang="">
<code><span class="reserved">var</span> map = Dictionary&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;
{
  { <span class="literal">"One"</span>, <span class="literal">1</span> },
  { <span class="literal">"Two"</span>, <span class="literal">2</span> },
  { <span class="literal">"Three"</span>, <span class="literal">3</span> },
  { <span class="literal">"Four"</span>, <span class="literal">4</span> },
};
</code></pre>

<h5 class="version version12">Ver. 12</h5>

C# 12 からはコレクション初期化子に代わって、以下のようにコレクションを作ることができるようになりました。
これをコレクション式といいます。

<pre class="source" title="">
<span class="reserved">int</span>[] <span class="variable">a</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">3</span>, <span class="number">5</span>, <span class="number">7</span>, <span class="number">9</span>];
</pre>

コレクション初期化子との差や、コレクション式のメリットなどは「[コレクション式](../datatype/collection-expression.md)」で説明します。



###<a id="sec-generated-title-9"></a> <a id="index-initializer"></a>インデックス初期化
<h5 class="version version6">Ver. 6.0</h5>

C# 6.0 から、[オブジェクト初期化子](#object-initializer)に、インデクサーを混ぜれるようになりました。
これを<strong id="key-index-initializer" class="keyword">インデックス初期化子</strong>(index initializer)といいます。

例えば `Dictionary`(`System.Collections.Generic`名前空間)に対して以下のような書き方ができます。

<pre class="source" title="インデックス初期化子の例">
<code><span class="reserved">var</span> <span class="variable">dic</span> = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;
{
    [<span class="string">&quot;one&quot;</span>] = 1,
    [<span class="string">&quot;two&quot;</span>] = 2,
};
</code></pre>

プロパティへの代入とインデクサーへの代入を混在させることもできます。

<pre class="source" title="初期化子内でのプロパティとインデクサーの混在">
<code><span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">string</span> <span class="variable">key</span>]
    {
        <span class="reserved">get</span> { <span class="control">return</span> 0; }
        <span class="reserved">set</span> { }
    }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">s</span> = <span class="reserved">new</span> <span class="type">Sample</span>
        {
            Name = <span class="string">&quot;sample&quot;</span>,
            [<span class="string">&quot;X&quot;</span>] = 1,
            [<span class="string">&quot;Y&quot;</span>] = 2,
        };
    }
}
</code></pre>

###<a id="sec-generated-title-10"></a> <a id="recursive"></a>再帰初期化
ちなみに、再帰的な構造を持ったクラスの初期化もできます。

<pre class="source" title="再帰的なオブジェクト初期化子" lang="">
<code><span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public double</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public double</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">class</span> <span class="type">Color</span>
{
    <span class="reserved">public byte</span> R { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public byte</span> G { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public byte</span> B { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">class</span> <span class="type">Geometry</span>
{
    <span class="reserved">public</span> <span class="type">List</span>&lt;<span class="type">Point</span>&gt; Vertices = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="type">Point</span>&gt;();
    <span class="reserved">public</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; Indices = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
}

<span class="reserved">class</span> <span class="type">Model</span>
{
    <span class="reserved">public</span> <span class="type">Geometry</span> Geometry = <span class="reserved">new</span> <span class="type">Geometry</span>();
    <span class="reserved">public</span> <span class="type">Color</span> Color = <span class="reserved">new</span> <span class="type">Color</span>();
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="type">Model</span> m = <span class="reserved">new</span> <span class="type">Model</span>
        {
            Color = { R = <span class="literal">128</span>, G = <span class="literal">128</span>, B = <span class="literal">128</span> },
            Geometry =
            {
                Vertices =
                {
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">0</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">0</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">1</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">1</span>},
                },
                Indices = { <span class="literal">0</span>, <span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">0</span>, <span class="literal">2</span>, <span class="literal">3</span> },
            },
        };

        <span class="comment">//Model m = new Model();
        //m.Color.R = 128;</span>
    }
}
</code></pre>


ただし、再帰的な初期化をするためには、メンバーが参照型（class）である必要があります。
例えば、上記の例で、Color が class ではなく struct だった場合、
コンパイルエラーになります。

また、この記法ででの初期化は、以下のようなコードと等価で、Color、Geometry、Indices などに対してインスタンスを new してくれたりはしないので注意が必要です。
コンストラクターもしくはメンバー初期化子での初期化が必要です。

<pre class="source" title="再起初期化子の解釈結果" lang="">
<code>        <span class="type">Model</span> m = <span class="reserved">new</span> <span class="type">Model</span>();
        <span class="comment">// ↓ m = new Model() の時点で Color が初期化されていないと NullReferenceException。</span>
        m.Color.R = <span class="literal">128</span>;
        m.Color.G = <span class="literal">128</span>;
        m.Color.B = <span class="literal">128</span>;
        m.Geometry.Vertices.Add(<span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">0</span> });
        m.Geometry.Vertices.Add(<span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">0</span> });
        m.Geometry.Vertices.Add(<span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">1</span> });
        m.Geometry.Vertices.Add(<span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">1</span> });
        m.Geometry.Indices.Add(<span class="literal">0</span>);
        m.Geometry.Indices.Add(<span class="literal">1</span>);
        m.Geometry.Indices.Add(<span class="literal">2</span>);
        m.Geometry.Indices.Add(<span class="literal">0</span>);
        m.Geometry.Indices.Add(<span class="literal">2</span>);
        m.Geometry.Indices.Add(<span class="literal">3</span>);
</code></pre>


さもなくば、以下のように、おとなしく new を書きましょう。

<pre class="source" title="おとなしく new を明示的に書く" lang="">
<code>        <span class="type">Model</span> m = <span class="reserved">new</span> <span class="type">Model</span>
        {
            Color = <span class="reserved">new</span> <span class="type">Color</span> { R = <span class="literal">128</span>, G = <span class="literal">128</span>, B = <span class="literal">128</span> },
            Geometry = <span class="reserved">new</span> <span class="type">Geometry</span>
            {
                Vertices = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="type">Point</span>&gt;
                {
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">0</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">0</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">1</span>, Y = <span class="literal">1</span>},
                    <span class="reserved">new</span> <span class="type">Point</span> { X = <span class="literal">0</span>, Y = <span class="literal">1</span>},
                },
                Indices = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { <span class="literal">0</span>, <span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">0</span>, <span class="literal">2</span>, <span class="literal">3</span> },
            },
        };
</code></pre>
