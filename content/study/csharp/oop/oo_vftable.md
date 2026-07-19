---
title: "[雑記] 仮想関数テーブル"
source_url: "https://ufcpp.net/study/csharp/oop/oo_vftable/"
content_type: "Article"
published_at: "2008-07-21T00:00:00"
updated_at: "2021-02-21T18:01:59"
tags: []
umbraco_id: 1272
parent_id: 1248
sort_order: 18
aliases:
  - "/csharp/oo_vftable"
  - "/csharp/oo_vftable.html"
  - "/csharp/oop/oo_vftable/"
  - "/study/csharp/oo_vftable"
  - "/study/csharp/oo_vftable.html"
---

# \[雑記\] 仮想関数テーブル

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[仮想メソッド](oo_polymorphism.md#virtual_method)」というものがどういう仕組みで実現されているのかを説明します。

（C / C++ の知識がある程度必要なので、
その辺りが全く分からない場合には内容が高度すぎるので読み飛ばし推奨。）

「[.NET Framework](../abstract/ab_dotnet.md#dotnet)」 の 「[IL](../abstract/ab_dotnet.md#il)」 は、
仮想メソッド呼び出し用の命令を持っていたりします。
ですが、一般的な PC に使われている CPU が「仮想メソッド呼出し命令」みたいなものを持っているわけではなく、
.NET Framework が適切な命令に置き換えて仮想メソッド呼び出しを実現してくれています。

（要するに、C# プログラマにとっては気にする必要のない部分です。
このページの内容は、「でも中身の分からないものを使うのはなんとなく不安」という人向けです。）

一般に、仮想メソッド呼び出し（C++ など、言語によっては仮想関数呼び出しという言い方をします）は、
<strong id="vftable" class="keyword">仮想関数テーブル</strong>（virtual function table）というものを用いて実現されています。

ここでは、C++ の仮想関数呼び出しを、
それとほぼ透過な（非オブジェクト指向言語の） C 言語コードに置き換えることで、
仮想関数テーブルの実装方法を示します。
（C# と C だとちょっと「遠い親戚」過ぎるので、このページのサンプルには C++ を使います。）

↓サンプルプログラムのソース。

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/VFTable.zip)




##<a id="sec-generated-title-2"></a> <a id="theme"></a>題材と元ソース
ここでは、「[多態性](oo_polymorphism.md)」の演習問題と同じような題材
（Shape クラスを継承した Rectangle と Circle クラスを作る）
で説明をします。

まず、元となる C++ のソースを示すと以下のような感じ。

<pre class="source" title="ShapeCpp.h" lang="">
<code><span class="reserved">#pragma</span> once

<span class="reserved">class</span> Shape
{
<span class="reserved">public</span>:
  <span class="reserved">virtual double</span> GetArea() = 0;
  <span class="reserved">virtual double</span> GetPerimeter() = 0;
};

<span class="reserved">class</span> Rectangle : <span class="reserved">public</span> Shape
{
<span class="reserved">public</span>:
  Rectangle(<span class="reserved">double</span> w, <span class="reserved">double</span> h);
  <span class="reserved">virtual double</span> GetArea();
  <span class="reserved">virtual double</span> GetPerimeter();

<span class="reserved">private</span>:
  <span class="reserved">double</span> width;
  <span class="reserved">double</span> height;
};

<span class="reserved">class</span> Circle : <span class="reserved">public</span> Shape
{
<span class="reserved">public</span>:
  Circle(<span class="reserved">double</span> r);
  <span class="reserved">virtual double</span> GetArea();
  <span class="reserved">virtual double</span> GetPerimeter();

<span class="reserved">private</span>:
  <span class="reserved">double</span> radius;
};
</code></pre>


<pre class="source" title="ShapeCpp.cpp" lang="">
<code><span class="reserved">#include</span> <span class="literal">"ShapeCpp.h"</span>

Rectangle::Rectangle(<span class="reserved">double</span> w, <span class="reserved">double</span> h)
{
  <span class="reserved">this</span>-&gt;width = w;
  <span class="reserved">this</span>-&gt;height = h;
}

<span class="reserved">double</span> Rectangle::GetArea()
{
  <span class="reserved">return this</span>-&gt;width * <span class="reserved">this</span>-&gt;height;
}

<span class="reserved">double</span> Rectangle::GetPerimeter()
{
  <span class="reserved">return</span> 2 * (<span class="reserved">this</span>-&gt;width + <span class="reserved">this</span>-&gt;height);
}

Circle::Circle(<span class="reserved">double</span> r)
{
  <span class="reserved">this</span>-&gt;radius = r;
}

<span class="reserved">double</span> Circle::GetArea()
{
  <span class="reserved">return</span> 3.14159265358979 * <span class="reserved">this</span>-&gt;radius * <span class="reserved">this</span>-&gt;radius;
}

<span class="reserved">double</span> Circle::GetPerimeter()
{
  <span class="reserved">return</span> 2 * 3.14159265358979 * <span class="reserved">this</span>-&gt;radius;
}
</code></pre>


要するに、Shape クラスは図形を表していて、面積と周囲を求めるメソッドを持っています。
Rectangle、Circle はそれぞれ、矩形・円を表すクラスです。


##<a id="sec-generated-title-3"></a> <a id="method"></a>メンバー関数と仮想関数テーブル
まず、Shape クラスの宣言に相当する C 言語コードを作ってみます。

<pre class="source" title="Shape クラスの宣言に相当する C 言語コード" lang="">
<code><span class="comment">//----------------------------------------------------------------
// class Shape に相当
</span>
<span class="reserved">typedef struct</span> TagShape
{
  <span class="reserved">void</span>** vftable;
} Shape;

<span class="reserved">#define</span> VF_GetArea 1
<span class="reserved">#define</span> VF_GetPerimeter 2

<span class="reserved">typedef double</span> TypeGetArea(Shape* this);
<span class="reserved">typedef double</span> TypeGetPerimeter(Shape* this);

<span class="reserved">extern void</span>* ShapeVftable[];
<span class="reserved">void</span> ShapeCtor(Shape* this);
<span class="reserved">void</span> ShapeDtor(Shape* this);
</code></pre>


まず、非 OOP 言語にはメンバー関数（メソッド）なんてものはありません。
C++ で、

<pre class="source" title="メンバー関数" lang="">
<code><span class="reserved">class</span> Person
{
<span class="reserved">public</span>:
  <span class="reserved">int</span> GetAge();
};

Person p;
p.GetAge();
</code></pre>


と言うように書いていたものは、
C 言語では、

<pre class="source" title="メンバー関数" lang="">
<code><span class="reserved">typedef struct</span> TagPerson
{
} Person;

<span class="reserved">int</span> PersonGetAge(Person* p);

Person p;
PersonGetAge(&amp;p);
</code></pre>


と書く必要があります。
（typedef してるのは、C 言語と C++ の仕様の違いのためで、
このページの内容とはあまり関係ないので説明は割愛。）

で、ShapeVftable というのが、仮想関数を実現するためのキモである仮想関数テーブルというやつです。
実体は以下のようになっています。

<pre class="source" title="Shape クラスの仮想関数テーブル" lang="">
<code><span class="reserved">void</span>* ShapeVftable[] = 
{
  <span class="literal">"class Shape"</span>,
  0,
  0
};
</code></pre>


void* の配列になっていて、
配列の1番目はクラスの型情報、
2番目、3番目が GetArea, GetPerimeter メンバー関数の実体をさすポインターです。
今回の場合、Shape の GetArea, GetPerimeter は純粋仮想関数
（C# でいうところの「[抽象メソッド](oo_abstract.md#abmethod)」 ）なので、
0（ヌルポインター、実体がないことを示す）になっています。


##<a id="sec-generated-title-4"></a> <a id="ctor"></a>コンストラクタ
ShapeCtor, ShapeDtor はそれぞれコンストラクタ、デストラクタに相当する関数です。
（当然、Ctor, Dtor は Constructor, Destructor の略。）
C++ とは違って、これらを自動的に読んでくれる仕組みは持っていないので、
自分で呼び出してやる必要があります。
例えば、C++ の、

<pre class="source" title="Shape を new" lang="">
<code><span class="comment">// 実際には、Shape は抽象クラスなので new できないけども。
</span>
s = <span class="reserved">new</span> Shape();

<span class="reserved">delete</span> s;
</code></pre>


と同じ事をしようと思うと、以下のような書き方が必要になります。

<pre class="source" title="C 言語で new, delete 相当のコード" lang="">
<code>s = (Shape*)malloc(<span class="reserved">sizeof</span>(Shape));
ShapeCtor(s);

ShapeDtor(s);
free(s);
</code></pre>


ちなみに、元の Shape クラスがコンストラクタ・デストラクタで特に何もしていないので、
ShapeCtor, ShapeDtor の中身もほぼ空っぽになります。
ただし、ShapeCtor の中では1つだけやっておかないといけないことがあります。
前節で説明した仮想関数テーブルの実体 ShapeVftable を、
Shape の vftable メンバー変数に代入します。

<pre class="source" title="ShapeCtor, ShapeDtor の実体" lang="">
<code><span class="reserved">void</span> ShapeCtor(Shape* this)
{
  this-&gt;vftable = ShapeVftable;
}

<span class="reserved">void</span> ShapeDtor(Shape* this)
{
}
</code></pre>


この vftable は、仮想関数呼び出しの際に利用します。


##<a id="sec-generated-title-5"></a> <a id="inherit"></a>クラスの継承
続いて、Rectangle クラスの宣言に相当する C 言語コードを示します。

<pre class="source" title="Rectangle クラスの宣言に相当する C 言語コード" lang="">
<code><span class="comment">//----------------------------------------------------------------
// class Rectangle に相当
</span>
<span class="reserved">typedef struct</span> TagRectangle
{
  Shape base;

  <span class="reserved">double</span> width;
  <span class="reserved">double</span> height;
} Rectangle;

<span class="reserved">extern void</span>* RectangleVftable[];
<span class="reserved">void</span> RectangleCtor(Rectangle* this, <span class="reserved">double</span> w, <span class="reserved">double</span> h);
<span class="reserved">void</span> RectangleDtor(Rectangle* this);
<span class="reserved">double</span> RectangleGetArea(Rectangle* this);
<span class="reserved">double</span> RectangleGetPerimeter(Rectangle* this);
</code></pre>


Shape のときと同じく、
仮想関数テーブル RectangleVftable、
コンストラクタ RectangleCtor、
デストラクタ RectangleDtor を持っています。
それに加え、
GetArea, GetPerimiter メンバー関数に相当する、
RectangleGetArea, RectangleGetPerimeter があります。

クラスのメンバー変数（Rectangle の場合は width と height）は、
そのまま構造体のメンバー変数になります。

問題は「[継承](oo_inherit.md#derive)」なんですが、
これは、単に、親クラスを1つ目のメンバー変数として持つことによって実現します。
例えば、

<pre class="source" title="基底クラスのポインター変数に代入" lang="">
<code>Rectangle* r = (Rectangle*)malloc(<span class="reserved">sizeof</span>(Rectangle));
Shape* s = (Shape*)r;

<span class="reserved">if</span> (s-&gt;vftable == r-&gt;base.vftable)
  printf(<span class="literal">"true"</span>);
</code></pre>


というようなコードを書いた場合、
ちゃんと、 true という文字列が出力されるはずです。
（&amp;r と &amp;r-&gt;base が同じアドレスを表している。）


##<a id="sec-generated-title-6"></a> <a id="inherit_impl"></a>派生クラスの実装
Rectangle クラスの仮想関数テーブルの実体 RectangleVftable は以下のようになります。

<pre class="source" title="Rectangle クラスの仮想関数テーブル" lang="">
<code><span class="reserved">void</span>* RectangleVftable[] =
{
  <span class="literal">"class Rectangle"</span>,
  RectangleGetArea,
  RectangleGetPerimeter
};
</code></pre>


Shape のときと同じく、
配列の1つ目がクラスに関する情報で、
2つ目、3つ目がそれぞれ GetArea, GetPerimeter に相当する関数へのポインターです。
Shape のときと違って、GetArea, GetPerimeter が実体を持っているので、
0 以外のちゃんとした値がセットされています。

コンストラクタに相当する関数 RectangleCtor で、
vftable メンバーに RectangleVftable を代入します。
ここで、1つ注意が必要なのは、
C++ と違って基底クラスのコンストラクタを自動的に呼んでくれるような機能はないので、
プログラマが明示的に ShapeCtor を呼び出す必要があります。
（デストラクタも同様。）

<pre class="source" title="Rectangle のコンストラクタ・デストラクタ" lang="">
<code><span class="reserved">void</span> RectangleCtor(Rectangle* this, <span class="reserved">double</span> w, <span class="reserved">double</span> h)
{
  ShapeCtor(&amp;this-&gt;base);

  this-&gt;base.vftable = RectangleVftable;

  this-&gt;width = w;
  this-&gt;height = h;
}

<span class="reserved">void</span> RectangleDtor(Rectangle* this)
{
  ShapeDtor(&amp;this-&gt;base);
}
</code></pre>


ちなみに、RectangleGetArea, RectangleGetPerimeter の実体は以下のような感じ。
元の C++ の Rectangle クラスの GetArea, GetPerimeter とほぼ同じです。
（関数の引数に Rectangle* this が増えているだけ。）

<pre class="source" title="RectangleGetArea, RectangleGetPerimeter" lang="">
<code><span class="reserved">double</span> RectangleGetArea(Rectangle* this)
{
  <span class="reserved">return</span> this-&gt;width * this-&gt;height;
}

<span class="reserved">double</span> RectangleGetPerimeter(Rectangle* this)
{
  <span class="reserved">return</span> 2 * (this-&gt;width + this-&gt;height);
}
</code></pre>



##<a id="sec-generated-title-7"></a> <a id="call"></a>仮想関数の呼び出し
Circle クラスの実装は Rectangle とほぼ同様なので説明は省略。
次は、仮想関数呼び出しの C 言語化を行います。

C++ で、以下のようなコードを考えます。

<pre class="source" title="仮想関数呼び出し" lang="">
<code><span class="reserved">void</span> print(Shape* s)
{
  printf(<span class="literal">"%s\n%f\n%f\n\n"</span>,
    <span class="reserved">typeid</span>(*s).name(),
    s-&gt;GetArea(),
    s-&gt;GetPerimeter());
}

<span class="reserved">void</span> TestCpp()
{
  Shape* s;
  
  s = <span class="reserved">new</span> Rectangle(2, 3);
  print(s);
  <span class="reserved">delete</span> s;

  s = <span class="reserved">new</span> Circle(1.41421356);
  print(s);
  <span class="reserved">delete</span> s;
}
</code></pre>


Rectangle, Circle のインスタンスそれぞれについて、
クラス名、面積、周囲を求めて表示しています。

これに相当する C 言語コードは以下のようになります。

<pre class="source" title="仮想関数呼び出しに相当する C 言語コード" lang="">
<code><span class="reserved">void</span> print(Shape* s)
{
  printf(<span class="literal">"%s\n%f\n%f\n\n"</span>,
    (<span class="reserved">char</span>*)s-&gt;vftable[0],
    ((TypeGetArea*)s-&gt;vftable[VF_GetArea])(s),
    ((TypeGetPerimeter*)s-&gt;vftable[VF_GetPerimeter])(s));
}

<span class="reserved">void</span> TestC(<span class="reserved">void</span>)
{
  Shape* s;

  s = (Shape*)malloc(<span class="reserved">sizeof</span>(Rectangle));
  RectangleCtor((Rectangle*)s, 2, 3);
  print(s);
  RectangleDtor((Rectangle*)s);
  free(s);

  s = (Shape*)malloc(<span class="reserved">sizeof</span>(Circle));
  CircleCtor((Circle*)s, 1.41421356);
  CircleDtor((Circle*)s);
  print(s);
  free(s);
}
</code></pre>


これで、先ほどの C++ コードと同じ出力が得られます。

このままだと分かりにくいので、
仮想関数呼び出しと、型情報の取得の部分だけを取り出してみましょう。
まずは C++。

<pre class="source" title="仮想関数呼び出し" lang="">
<code><span class="reserved">typeid</span>(*s).name(),
s-&gt;GetArea(),
s-&gt;GetPerimeter());
</code></pre>


続いて C 言語版。

<pre class="source" title="仮想関数呼び出しに相当する C 言語コード" lang="">
<code>(<span class="reserved">char</span>*)s-&gt;vftable[0],
((TypeGetArea*)s-&gt;vftable[VF_GetArea])(s),
((TypeGetPerimeter*)s-&gt;vftable[VF_GetPerimeter])(s));
</code></pre>


型情報の取得は簡単ですね。
仮想関数テーブルの先頭に型情報を入れたので、それを取り出すだけです。

仮想関数の呼び出しは少々面倒なんですが、
要するに、

* 仮想関数テーブルには関数ポインターが入っているので、それを取り出す。

* その関数ポインターを介して、メンバー関数の実体を呼ぶ。


ということをしています。

vftable はコンストラクタに相当する関数
ShapeCtor, RectangleCtor, CircleCtor の中で、
それぞれ ShapeVftable, RectangleVftable, CircleVftable に初期化されています。
なので、

<pre class="source" title="仮想関数呼び出しに相当する C 言語コード" lang="">
<code>Shape* s = (Shape*)malloc(<span class="reserved">sizeof</span>(Rectangle));
RectangleCtor((Rectangle*)s, 2, 3);

((TypeGetArea*)s-&gt;vftable[VF_GetArea])(s),
</code></pre>


というコードでは、
s は Shape のポインター型の変数ですが、
正しく RectangleGetArea を呼び出すことができます。


##<a id="sec-generated-title-8"></a> <a id="cost"></a>仮想関数呼び出しのコスト
###<a id="sec-generated-title-9"></a> <a id="computationalcost"></a>演算コスト
ここで、通常の関数呼び出しと仮想関数呼び出しの比較をしてみましょう。

もし、GetArea が仮想関数ではなかった場合、
（C 言語版の）RectangleGetArea の呼び出しは以下のようになります。

<pre class="source" title="通常のメンバー関数の C 言語化" lang="">
<code>Shape* s = (Shape*)malloc(<span class="reserved">sizeof</span>(Rectangle));
RectangleCtor((Rectangle*)s, 2, 3);

RectangleGetArea((Rectangle*)s);
</code></pre>


一方、仮想関数呼び出しは以下のようになります。

<pre class="source" title="仮想関数呼び出しの C 言語化" lang="">
<code>Shape* s = (Shape*)malloc(<span class="reserved">sizeof</span>(Rectangle));
RectangleCtor((Rectangle*)s, 2, 3);

((TypeGetArea*)s-&gt;vftable[VF_GetArea])(s),
</code></pre>


その差は、仮想関数テーブル vftable の参照を行うかどうかということになります。
要するに、
「<em>仮想関数呼び出しの演算コストはテーブルの参照1回分</em>」
ということです。

このコストを小さいと見るか大きいと見るかは状況次第ですが、
当たり障りのない言い方をすると、
「<em>微々たるコストだけども、避けれるなら避けたい</em>」
といった所です。

ちなみに、メンバー関数に virtual キーワードが付いていても、
必ずしも仮想関数呼び出しになるわけではありません。
例えば、以下のようなコード（要するに、ポインターや参照を使っていない）では、
コンパイル時にどのメンバー関数を呼び出せばいいのかが確定するので、
通常のメンバー関数呼び出しになります。

<pre class="source" title="仮想関数呼び出し" lang="">
<code>Rectangle r(2, 3);
r.GetArea();
</code></pre>


当然、仮想関数であることのメリットも一切受けないことになるので、
状況による使い分けが必要です。


###<a id="sec-generated-title-10"></a> <a id="memorycost"></a>メモリコスト
メモリの観点から見ると、
仮想関数を使うためには仮想関数テーブル分のメモリが必要になります。
具体的なメモリの量は、

* 仮想関数1つに付き、関数ポインター1つ分。

* インスタンス1つに付き、vftable 分（これもポインター1つ分）。


となります。
ポインターのサイズは、処理系によりますが、
今だと大体は4バイトもしくは8バイトです。
クラス自体のサイズの大小に関係なく、常にこの4～8バイト分のサイズ増加があります。
なので、小さいクラスほど、相対的に vftable のコストが大きくなります。

C++ の仕様では、クラス中に1つでも仮想関数があると、
vftable が自動生成されます。
逆に、1つも仮想関数がなければ vftable は生成されません。
（したがって、型情報（typeid）も使えなくなります。）
「必要がなければ（特に小さいクラスでは）仮想関数は使うな」ということなんですが、
「すでに仮想関数が1つあるのに、2つ目の仮想関数の追加をためらう理由はそれほどない」
と言えます。


###<a id="sec-generated-title-11"></a> <a id="csharp"></a>C# や Java では
C# や Java では、
型情報の取得のために、仮想メソッド（C++ でいうと仮想関数）が1つもない場合でも、
有無を言わせず vftable 相当の物が自動生成されます。
Java ではこのコストを避ける方法はありません。

C# の場合には、
1つも仮想関数が必要ないのなら、class ではなくて struct にすることで、
vftable 分のメモリを節約することができます。
（値型は継承不可、仮想メソッド定義不可。
したがって、仮想関数テーブル相当の物は必要ない。）
（「1つも必要ない」というよりは、
「将来的にも絶対に1つも必要としない自信がある」場合に struct を使います。）

（struct を含む）値型を object 型の変数に代入すると、
ちゃんと ToString などの仮想メソッド呼び出しができるわけですが、
これは object への代入の際に仮想関数テーブルに相当する情報を追加する処理が行われるためです。
この処理を boxing と呼びます。
