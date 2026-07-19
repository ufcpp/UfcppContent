---
title: "Set / Get とプロパティ"
source_url: "https://ufcpp.net/study/miscprog/list/accessor/"
content_type: "Article"
published_at: "2007-05-15T00:00:00"
updated_at: "2015-05-06T14:19:20"
tags: []
umbraco_id: 1544
parent_id: 1542
sort_order: 1
aliases:
  - "/miscprog/accessor"
  - "/miscprog/accessor.html"
  - "/miscprog/list/accessor/"
  - "/study/miscprog/accessor"
  - "/study/miscprog/accessor.html"
---

# Set / Get とプロパティ

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[プロパティ](../../csharp/oop/oo_property.md)」で説明しているように、
C# にはプロパティという便利な機能が備わっています。
 
ここでは、その C# が出てくる以前、
C++ ではどうやって実装の隠蔽をしていたかについて説明したいと思います。
（ちょっと記憶があいまいだけど、
確か Effective C++ か More Effective C++ 辺りで読んだ話。）

「[C# によるプログラミング入門](../../csharp/index.md)」では、
名前をあらわす name と、
年齢をあらわす age をメンバーとして持つクラス Person を使って説明をしましたので、
ここでも Person クラスの age を例として説明します。


##<a id="sec-generated-title-2"></a> <a id="setter"></a>Set / Get
一番簡単なのは、public にしたいメンバー変数の数だけ、
Set変数名 / Get変数名 という名前のメンバー関数を用意する方法。

age なら、SetAge と GetAge というのを Person クラス内に作る。

<pre class="source" title="SetAge, GetAge" lang="">
<code><span class="reserved">#include</span>&lt;iostream&gt;

<span class="reserved">class</span> Person
{
<span class="reserved">private</span>:
  <span class="reserved">int</span> age;
<span class="reserved">public</span>:
  <span class="reserved">void</span> SetAge(<span class="reserved">int</span> a)
  {
    <span class="reserved">if</span>(a &lt; 0) <span class="reserved">return</span>;
    <span class="reserved">this</span>-&gt;age = a;
  }

  <span class="reserved">int</span> GetAge()
  {
    <span class="reserved">return this</span>-&gt;age;
  }

  Person() { <span class="reserved">this</span>-&gt;SetAge(0); }
  Person(<span class="reserved">int</span> a) { <span class="reserved">this</span>-&gt;SetAge(a); }
};

<span class="reserved">int</span> main()
{
  Person p;

  p.SetAge(20);
  std::cout &lt;&lt; p.GetAge();

  <span class="reserved">return</span> 0;
}
</code></pre>


まあ、これが C++ における実装の隠蔽の基本です。
クラス中 Set/Get だらけになるのがあたりまえ。
 
ちなみに、「[プロパティ](../../csharp/oop/oo_property.md)」でも書いていますが、
こういうのを <strong id="setter" class="keyword">setter</strong> / <strong id="getter" class="keyword">getter</strong> と呼びます。
また、setter / getter をあわせて <strong id="accessor" class="keyword">accessor</strong> と呼んだりします。
 
プログラミングになれた人なら accessor をきっちり書くんですが、
初心者はめんどくさがって accessor を書く癖をなかなか付けてくれなかったりします。


##<a id="sec-generated-title-3"></a> <a id="overload"></a>オーバーロードで Set / Get を省略
で、Set / Get だらけになるのを嫌って、
これを省略する人もいます。
Set / Get だらけだと、
Visual Studio のインテリセンスなどの入力支援（変数名を途中まで書けば残りの部分を補間してくれたり）も働かなくなりますし（Set までタイピングしても、どの Set なのか分からない）。
 
C++ は、引数が異なる同名の関数を定義（オーバーロード）できるので、
void SetAge(int a) と int GetAge() の両方を Age という名前にしてしまっても問題ありません。

<pre class="source" title="void Age(int a), int Age()" lang="">
<code><span class="reserved">#include</span>&lt;iostream&gt;

<span class="reserved">class</span> Person
{
<span class="reserved">private</span>:
  <span class="reserved">int</span> age;
<span class="reserved">public</span>:
  <span class="reserved">void</span> Age(<span class="reserved">int</span> a)
  {
    <span class="reserved">if</span>(a &lt; 0) <span class="reserved">return</span>;
    <span class="reserved">this</span>-&gt;age = a;
  }

  <span class="reserved">int</span> Age()
  {
    <span class="reserved">return this</span>-&gt;age;
  }

  Person() { <span class="reserved">this</span>-&gt;Age(0); }
  Person(<span class="reserved">int</span> a) { <span class="reserved">this</span>-&gt;Age(a); }
};

<span class="reserved">int</span> main()
{
  Person p;

  p.Age(20);
  std::cout &lt;&lt; p.Age();

  <span class="reserved">return</span> 0;
}
</code></pre>


まあ、Set / Get がなくなって、タイピングしやすくはなりました。
でも、それだけです。
C# の「[プロパティ](../../csharp/oop/oo_property.md#property)」のように、
利用側では変数のように扱えたりはしません。


##<a id="sec-generated-title-4"></a> <a id="proxy"></a>プロキシ
実は、C++ でも、かなり無理やりですが、（見た目だけは）プロパティのようなことができたりします。
とりあえず、百聞は一見にしかずということで、以下の例を見てください。

<pre class="source" title="proxy" lang="">
<code><span class="reserved">#include</span>&lt;iostream&gt;

<span class="reserved">class</span> Person
{
<span class="reserved">private</span>:
  <span class="reserved">int</span> age;
<span class="reserved">public</span>:
  <span class="reserved">class</span> AgeProxy
  {
    Person&amp; p;
  <span class="reserved">public</span>:
    AgeProxy(Person&amp; p0) : p(p0) {}

    AgeProxy&amp; <span class="reserved">operator</span>= (<span class="reserved">int</span> a)
    {
      <span class="reserved">if</span>(a &gt;= 0)
        <span class="reserved">this</span>-&gt;p.age = a;
      <span class="reserved">return</span> *<span class="reserved">this</span>;
    }

    <span class="reserved">operator int</span>()
    {
      <span class="reserved">return this</span>-&gt;p.age;
    }
  } Age;

  <span class="reserved">friend class</span> AgeProxy;

  Person() : Age(*<span class="reserved">this</span>) { <span class="reserved">this</span>-&gt;Age = 0; }
  Person(<span class="reserved">int</span> a) : Age(*<span class="reserved">this</span>) { <span class="reserved">this</span>-&gt;Age = a; }
};

<span class="reserved">int</span> main()
{
  Person p;

  p.Age = 20;
  std::cout &lt;&lt; (<span class="reserved">int</span>)p.Age;

  <span class="reserved">return</span> 0;
}
</code></pre>


Person の中身はちょっと変な感じになっていますが、
利用側、すなわち、main の中では、
まるで普通の変数に対する代入・参照であるかのようなコードになっています。
 
でも、単なる変数への代入とは違って、
ちゃんと、p.Age = 20 のところで、
年齢 age が負にならないようにチェックが行われます。
 
このからくりは、
age の読み書きに、AgeProxy という名前の別のクラスを介することで実現します。
Age は AgeProxy 型の変数です。
AgeProxy の代入演算子（operator =）と int 型へのキャスト（operator int）を通して、
Person クラスの age 変数の読み書きをします。
 
ちなみに、こういう例のように、いったん別のクラスを通して値を読み書きしたりする方法を、
<strong id="proxy" class="keyword">プロキシ</strong>（proxy: 代理）と呼びます。
 
まあ、このパターンは、利用側の見た目は綺麗になりますが、
実装は面倒ですし、実行効率もあまりよいとはいえません。
さらに言うと、プロパティを virtual 化しようとすると、
この例よりもさらに複雑な実装が必要になります。
 
こういう感じの話を振り返った上で、
改めて C# の「[プロパティ](../../csharp/oop/oo_property.md#property)」機能を見ると、
便利な機能だなぁとつくづく思います。
