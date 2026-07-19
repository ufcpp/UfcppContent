---
title: "実装の隠蔽"
source_url: "https://ufcpp.net/study/csharp/oop/oo_conceal/"
content_type: "Article"
published_at: "2015-05-06T14:09:26"
updated_at: "2021-10-31T17:38:06"
tags: []
umbraco_id: 1254
parent_id: 1248
sort_order: 3
aliases:
  - "/csharp/oo_conceal"
  - "/csharp/oo_conceal.html"
  - "/csharp/oop/oo_conceal/"
  - "/study/csharp/oo_conceal"
  - "/study/csharp/oo_conceal.html"
---

# 実装の隠蔽

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[オブジェクト指向とは](oo_about.md)」で「オブジェクトは内部の実装がどうなっているのかを隠蔽し、可能な操作と属性のみを公開する」と書きました。
しかし、今までのサンプルではまず、クラスの定義の仕方などを覚えてもらうためにこのような実装の隠蔽については何も説明していませんでした。

ここでは、
クラスの内部実装を隠蔽するためにクラスのメンバー変数やメソッドにアクセシビリティを設定する方法を説明し、
なぜクラスの内部実装を隠蔽する必要があるのかを説明します。


##### <a id="sec-generated-title-2"></a>ポイント

* オブジェクト指向の中核概念その1: 実装の隠蔽（カプセル化）。

* 外（クラス利用側）から見た振る舞いと中身（実装側）はわけて考える。

* 中身は隠す（利用者に見せない）。

* 目的：
    * 不正な書き換えを防止する。

    * 実装を変更したときに、利用者側まで変更する必要をなくす。





## <a id="sec-generated-title-3"></a> <a id="level"></a>アクセシビリティ

クラスのメンバー変数やメソッドには<strong id="level" class="keyword">アクセシビリティ</strong>（Accessibility: アクセスできる度合い）というものがあります。
アクセシビリティとは、変数やメソッドに対して、どこからアクセスできるかという制限の度合いのことで、
以下のようなものがあります。

<table summary="アクセシビリティに関する修飾子">

	<tr>
		<th>アクセシビリティ</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">public</td>
		<td markdown="1">どこからでもアクセス可能</td>
	</tr>
	<tr>
		<td markdown="1">protected</td>
		<td markdown="1">クラス内部と、派生クラスの内部からのみアクセス可能</td>
	</tr>
	<tr>
		<td markdown="1">internal</td>
		<td markdown="1">同一プロジェクト内のクラスからのみアクセス可能</td>
	</tr>
	<tr>
		<td markdown="1">protected internal</td>
		<td markdown="1">同一プロジェクト内のクラス内部、または、派生クラスの内部からのみアクセス可能</td>
	</tr>
	<tr>
		<td markdown="1">private protected</td>
		<td markdown="1">(C# 7.2 以降)同一プロジェクト内のクラス内部、かつ、派生クラスの内部からのみアクセス可能</td>
	</tr>
	<tr>
		<td markdown="1">private</td>
		<td markdown="1">クラス内部からのみアクセス可能</td>
	</tr>
</table>

![アクセシビリティに関する修飾子](../../../../assets/media/1141/accessibility.png)

以下のように変数の前にキーワードを付けることでアクセシビリティを制御することが出来ます。

<pre class="source" title="アクセシビリティの指定" lang="">
<code><span class="input">アクセシビリティ</span> <span class="input">変数宣言やメソッド定義</span>
</code></pre>


派生クラスについては後ほど「[継承](oo_inherit.md#subclass)」で説明します。
また、アセンブリについては「[プロジェクトの分割](../package/project.md#assembly)」で説明します。

アクセス権限のない場所からクラスのメンバーにアクセスしようとするとエラーになります。
例えば、アクセシビリティをprivateにした変数に、クラスの外部からアクセスしようとするとエラーになります。
とりあえず、今のところは<em>クラスの外部に公開したいものはpublicに、そうでないものはprivateにする</em>とだけ覚えておいてください。

ちなみに、アクセシビリティを明示的に指定しなかった場合、private (一番厳しい制限)扱いされます。
後述しますが、むやみに広い範囲からアクセスできると後々の修正が大変になることがあるので、
可能な限り狭い範囲からだけアクセスできるようにすることをお勧めします。
迷うようなら、最初はprivateで作って、必要になったときに必要な分だけ制限を緩めるのがいいでしょう。

また、別項([トップ レベルのアクセシビリティ](../package/toplevelaccessibility.md))で説明しますが、(トップ レベルにある)クラス自身に対するアクセシビリティは public もしくは internal のみになります。

##### <a id="sec-generated-title-4"></a>サンプル

<pre class="source" title="アクセシビリティのサンプル" lang="">
<code><span class="reserved">class</span> A
{
  <span class="reserved"><em>public</em>    int</span> pub; <span class="comment">// どこからでもアクセス可能</span>
  <span class="reserved"><em>protected</em> int</span> pro; <span class="comment">// クラス内部と派生クラス内部からアクセス可能</span>
  <span class="reserved"><em>private</em>   int</span> pri; <span class="comment">// クラス内部からのみアクセス可能</span>

  <span class="reserved">public void</span> function1()
  {
    <span class="comment">// クラス内部</span>
    pub = 1; <span class="comment">// OK</span>
    pro = 2; <span class="comment">// OK</span>
    pri = 3; <span class="comment">// OK</span>
  }
}

<span class="reserved">class</span> B : A
{
  <span class="reserved">public void</span> function2()
  {
    <span class="comment">// 派生クラス内部</span>
    pub = 1; <span class="comment">// OK</span>
    pro = 2; <span class="comment">// OK</span>
    pri = 3; <span class="comment">// エラー</span>
  }
}

<span class="reserved">class</span> AccessibilitySample
{
  <span class="reserved">static void</span> Main()
  {
    A a = <span class="reserved">new</span> A();
    <span class="comment">// クラス A の外部</span>
    a.pub = 1; <span class="comment">// OK</span>
    a.pro = 2; <span class="comment">// エラー</span>
    a.pri = 3; <span class="comment">// エラー</span>
  }
}
</code></pre>


このソースをコンパイルしようとすると、以下のようなエラーが出ます。

<pre class="console" title="">
test.cs(23,3): error CS0122: 'A.pri' is inaccessible due to its protection level
test.cs(34,3): error CS0122: 'A.pro' is inaccessible due to its protection level
test.cs(35,3): error CS0122: 'A.pri' is inaccessible due to its protection level
</pre>



## <a id="sec-generated-title-5"></a> <a id="conceal"></a>実装の隠蔽

通常、内部の実装がどうなっているのかを隠蔽（要するに private にする）し、可能な操作のみを公開(public)することが望ましいとされています。
簡単に言うと、<em>メンバー変数はクラス外部から直接アクセス出来ないようにして、オブジェクトの状態の変更はすべてメソッドを通して行うべきだということです</em>。

例として、「[クラス](oo_class.md)」で作った複素数クラスについて考えてみましょう。
以前は実装の隠蔽は行っていませんでしたが、
ちゃんと実装を隠蔽するように作り直して見ましょう。

<pre class="source" title="複素数クラス その2" lang="">
<code><span class="reserved">class</span> Complex
{
  <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
  <span class="reserved">private double</span> re; <span class="comment">// 実部を記憶しておく</span>
  <span class="reserved">private double</span> im; <span class="comment">// 虚部を記憶しておく</span>

  <span class="comment">// 実部を取り出す</span>
  <span class="reserved">public double</span> Re(){<span class="reserved">return this</span>.re;}

  <span class="comment">// 実部を書き換え</span>
  <span class="reserved">public void</span> Re(<span class="reserved">double</span> x){<span class="reserved">this</span>.re = x;}

  <span class="comment">// 虚部を取り出す</span>
  <span class="reserved">public double</span> Im(){<span class="reserved">return this</span>.im;}

  <span class="comment">// 虚部を書き換え</span>
  <span class="reserved">public void</span> Im(<span class="reserved">double</span> y){<span class="reserved">this</span>.im = y;}

  <span class="comment">// 絶対値を取り出す</span>
  <span class="reserved">public double</span> Abs()
  {
    <span class="reserved">return</span> Math.Sqrt(re*re + im*im);<span class="comment">// Math.Sqrt は平方根を求める関数</span>
  }
}
</code></pre>


見ての通り、以前のものと比べてかなり回りくどくて面倒くさいものになっています。
なぜこのようにわざわざ回りくどい書き方をしなければいけないのか疑問に感じるかと思いますが、
クラスの内部実装を隠蔽する意義は、大きく分けて以下の2つがあります。

* オブジェクトの不正な書き換えを防止する。

* クラスの実装を変更した際、利用側のコードを修正する必要をなくす

ちなみに、パフォーマンスに関しては心配する必要はありません。
[インライン展開](../structured/miscinlining.md)という最適化が掛かるので、
元々のフィールドを直接公開するコードと大差ない速度で実行できます。

##### <a id="sec-generated-title-6"></a>オブジェクトの不正な書き換え防止する

「[コンストラクタ](../../../../assets/oo_construct.html)」で、 <code>Person</code> というクラスを作りました。
ここで、年齢が負の数になるのはおかしいので、
コンストラクタで年齢が負の数にならないようにチェックを行うように改良してみましょう。

<pre class="source" title="Person クラスその1" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">public int</span> age;     <span class="comment">// 年齢</span>

  <span class="reserved">public</span> Person()
  {
    <span class="reserved">this</span>.name = "";
    <span class="reserved">this</span>.age  = 0;
  }

  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    <span class="reserved">this</span>.age  = age &gt; 0 ? age : 0; <span class="comment">// age が負だった場合、0歳にしておく</span>
  }
}
</code></pre>


しかし、現時点ではクラスの外部から<code>Person</code>クラスのメンバー<code>age</code>を直接書き換えれてしまうため、
年齢が負の数にならないように強制することは無理です。
例えば、以下のサンプルのようにすると無理やり年齢を負の数に設定することができます。

<pre class="source" title="メンバー変数に直接アクセスができる場合の問題点" lang="">
<code>Person p = <span class="reserved">new</span> Person(<span class="literal">"範馬刃牙"</span>, -5); <span class="comment">// 年齢に負の値を設定しようとしても</span>
Console.Write(<span class="literal">"{0}は{1}歳です。\n"</span>,  <span class="comment">// 0歳に修正されている</span>
              p.name, p.age);        <span class="comment">// (「範馬刃牙は0歳です」と表示される)</span>

p.age = -5;                          <span class="comment">// でも、ageを直接書き換えてしまえば</span>
Console.Write(<span class="literal">"{0}は{1}歳です。\n"</span>,  <span class="comment">// 負の年齢になってしまう</span>
              p.name, p.age);        <span class="comment">// (「範馬刃牙は-5歳です」と表示される)</span>
</code></pre>


この問題を解決するためには、メンバー変数<code>age</code>は外部からは直接アクセスできないようにして、メソッドを通して<code>age</code>の値を設定、取得する必要があります。

<pre class="source" title="Person クラスその2" lang="">
<code><span class="reserved">class</span> Person
{
  <span class="reserved">public string</span> name; <span class="comment">// 名前</span>
  <span class="reserved">private int</span> age;    <span class="comment">// 年齢</span>

  <span class="reserved">public</span> Person()
  {
    <span class="reserved">this</span>.name = "";
    <span class="reserved">this</span>.age  = 0;
  }

  <span class="reserved">public</span> Person(<span class="reserved">string</span> name, <span class="reserved">int</span> age)
  {
    <span class="reserved">this</span>.name = name;
    SetAge(age);
  }

  <span class="reserved">public int</span> GetAge()
  {
    <span class="reserved">return this</span>.age;
  }

  <span class="reserved">public void</span> SetAge(int age)
  {
    <span class="reserved">this</span>.age  = age &gt; 0 ? age : 0; <span class="comment">// age が負だった場合、0歳にしておく</span>
  }
}
</code></pre>



##### <a id="sec-generated-title-7"></a>クラスの実装を変更した際、利用側のコードを修正する必要をなくす

クラスの実装を隠蔽しない場合、どのような不具合が生じるかを説明するため、
まず、以下のコードについて考えてみましょう。

<pre class="source" title="複素数クラスその1の利用" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> Complex
{
  <span class="reserved">public double</span> re; <span class="comment">// 実部を記憶しておく(外部からの読み出し・書き換えも可能)</span>
  <span class="reserved">public double</span> im; <span class="comment">// 虚部を記憶しておく(外部からの読み出し・書き換えも可能)</span>

  <span class="comment">// 絶対値を取り出す</span>
  <span class="reserved">public double</span> Abs()
  {
    <span class="reserved">return</span> Math.Sqrt(re*re + im*im);<span class="comment">// Math.Sqrt は平方根を求める関数</span>
  }
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> ConcealSample
{
  <span class="reserved">static void</span> Main()
  {
    Complex c = <span class="reserved">new</span> Complex();
    c.re = 4; <span class="comment">// メンバー変数に直接アクセス</span>
    c.im = 3; <span class="comment">// メンバー変数に直接アクセス</span>
    Console.Write(<span class="literal">"|c| = {0}\n"</span>, c.Abs());
  }
}
</code></pre>


「[クラス](oo_class.md)」で説明しましたが、複素数クラスの実装方法には、
上述のコードのような「実部と虚部をメンバー変数に記憶しておく」方法のほかに、
「絶対値と偏角をメンバー変数に記憶しておく」方法があります。
そして、加減算を行う回数よりも乗除算を行う回数のほうがはるかに多い場合、
後者のほうが計算量が少なくなります。

例えば、この複素数クラスを利用するプログラムがあったとして、
そのプログラムでは加減算よりも乗除算の回数のほうがはるかに多いため、
後者の方式に変更したくなったとします。
この場合、以下のようにクラスの側だけでなく、クラスの利用側のコードも修正する必要があります。

<pre class="source" title="複素数クラスその1の仕様変更時" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> Complex
{
  <span class="reserved">public double</span> abs; <span class="comment">// 絶対値を記憶しておく(外部からの読み出し・書き換えも可能)</span>
  <span class="reserved">public double</span> arg; <span class="comment">// 偏角を記憶しておく(外部からの読み出し・書き換えも可能)</span>

  <span class="comment">// 実部・虚部を書き換え</span>
  <span class="reserved">public void</span> Set(<span class="reserved">double</span> x, <span class="reserved">double</span> y)
  {
    this.abs = Math.Sqrt(x*x + y*y);
    this.arg = Math.Atan2(y, x);
  }
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> ConcealSample
{
  <span class="reserved">static void</span> Main()
  {
    Complex c = <span class="reserved">new</span> Complex();
<em>    c.Set(4, 3); <span class="comment">// クラス利用側のコードも修正が必要</span></em>
    Console.Write(<span class="literal">"|c| = {0}\n"</span>, c.abs);
  }
}
</code></pre>


このように、
クラスの実装方法を変更するたびに、利用側のコードまで修正する必要があると、
プログラムを作るのも保守するのも大変になります。

このような問題は、以下のように実装を隠蔽することで避けることができます。

<pre class="source" title="複素数クラスその2の利用" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> Complex
{
  <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
  <span class="reserved">private double</span> re; <span class="comment">// 実部を記憶しておく</span>
  <span class="reserved">private double</span> im; <span class="comment">// 虚部を記憶しておく</span>

  <span class="reserved">public double</span> Re(){<span class="reserved">return this</span>.re;}    <span class="comment">// 実部を取り出す</span>
  <span class="reserved">public void</span> Re(<span class="reserved">double</span> x){<span class="reserved">this</span>.re = x;} <span class="comment">// 実部を書き換え</span>

  <span class="reserved">public double</span> Im(){<span class="reserved">return this</span>.im;}    <span class="comment">// 虚部を取り出す</span>
  <span class="reserved">public void</span> Im(<span class="reserved">double</span> y){<span class="reserved">this</span>.im = y;} <span class="comment">// 虚部を書き換え</span>

  <span class="reserved">public double</span> Abs(){<span class="reserved">return</span> Math.Sqrt(re*re + im*im);}  <span class="comment">// 絶対値を取り出す</span>
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> ConcealSample
{
  <span class="reserved">static void</span> Main()
  {
    Complex c = <span class="reserved">new</span> Complex();
    c.Re(4); <span class="comment">// メソッドを通してオブジェクトの状態を変更</span>
    c.Im(3);
    Console.Write(<span class="literal">"|c| = {0}\n"</span>, c.Abs());
  }
}
</code></pre>


このコードの実装方法を
「実部と虚部をメンバー変数に記憶しておく」方法から
「絶対値と偏角をメンバー変数に記憶しておく」方法に変更する場合、
以下のように、クラス利用側のコードに手を加える必要は一切ありません。

<pre class="source" title="複素数クラスその2の仕様変更時" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">// クラス定義</span>
<span class="reserved">class</span> Complex
{
  <span class="comment">// 実装は外部から隠蔽(privateにしておく)</span>
  <span class="reserved">private double</span> abs; <span class="comment">// 絶対値を記憶しておく</span>
  <span class="reserved">private double</span> arg; <span class="comment">// 偏角を記憶しておく</span>

  <span class="comment">// 実部を取り出す</span>
  <span class="reserved">public double</span> Re()
  {
    <span class="reserved">return this</span>.abs * Math.Cos(<span class="reserved">this</span>.arg);
  }

  <span class="comment">// 実部を書き換え</span>
  <span class="reserved">public void</span> Re(<span class="reserved">double</span> x)
  {
    <span class="reserved">double</span> im = <span class="reserved">this</span>.abs * Math.Sin(<span class="reserved">this</span>.arg);
    <span class="reserved">this</span>.abs = Math.Sqrt(x*x + im*im);
    <span class="reserved">this</span>.arg = Math.Atan2(im, x);
  }

  <span class="comment">// 虚部を取り出す</span>
  <span class="reserved">public double</span> Im(){<span class="reserved">return this</span>.abs * Math.Sin(<span class="reserved">this</span>.arg);}

  <span class="comment">// 虚部を書き換え</span>
  <span class="reserved">public void</span> Im(<span class="reserved">double</span> y)
  {
    <span class="reserved">double</span> re = <span class="reserved">this</span>.abs * Math.Cos(<span class="reserved">this</span>.arg);
    <span class="reserved">this</span>.abs = Math.Sqrt(y*y + re*re);
    <span class="reserved">this</span>.arg = Math.Atan2(y, re);
  }

  <span class="reserved">public double</span> Abs(){<span class="reserved">return this</span>.abs;}  <span class="comment">// 絶対値を取り出す</span>
}

<span class="comment">// クラス利用側</span>
<span class="reserved">class</span> ConcealSample
{
  <span class="reserved">static void</span> Main()
  {
    Complex c = <span class="reserved">new</span> Complex();
    c.Re(4); <span class="comment">// クラス利用側は一切変更せず</span>
    c.Im(3);
    Console.Write(<span class="literal">"|c| = {0}\n"</span>, c.Abs());
  }
}
</code></pre>

## <a id="sec-generated-title-8"></a> <a id="protected-internal"></a>protected、internal、protected internal と private protected

`protected`や`internal`が必要になるのは[派生クラス](oo_inherit.md#subclass)や[アセンブリ](../package/project.md#assembly)が必要になってからですが、一応ここである程度説明しておきます。

まず、1つの[プロジェクト](../package/project.md#project)内ではアクセシビリティに応じて以下のような制限がかかります。

<pre class="source" title="同一プロジェクト内でのアクセス制限">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Public { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// どこからでも</span>
    <span class="reserved">protected</span> <span class="reserved">int</span> Protected { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// 派生クラスからだけ</span>
    <span class="reserved">internal</span> <span class="reserved">int</span> Internal { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// 同一アセンブリ(同一 exe/同一 dll)内からだけ</span>
    <span class="reserved">protected</span> <span class="reserved">internal</span> <span class="reserved">int</span> ProtectedInternal { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// 派生クラス "もしくは" 同一アセンブリ内 から</span>
    <span class="reserved">private</span> <span class="reserved">protected</span> <span class="reserved">int</span> PrivateProtected { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// 派生クラス "かつ" 同一アセンブリ内 から(C# 7.2 以降)</span>
    <span class="reserved">private</span> <span class="reserved">int</span> Private { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="comment">// クラス内からだけ</span>

    <span class="reserved">public</span> <span class="reserved">void</span> Method()
    {
        <span class="comment">// 同一クラス内</span>
        <span class="comment">// 全部 OK</span>
        Public = 0;
        Protected = 0;
        Internal = 0;
        ProtectedInternal = 0;
        Private = 0;
        PrivateProtected = 0;
    }
}

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">Derived</span> : Base
{
    <span class="reserved">public</span> <span class="reserved">void</span> MethodInDerived()
    {
        <span class="comment">// 同一アセンブリ内の派生クラス</span>
        <span class="comment">// コメントアウトしてないやつだけ OK</span>
        Public = 0;
        Protected = 0;
        Internal = 0;
        ProtectedInternal = 0;
        <span class="comment">//Private = 0;</span>
        PrivateProtected = 0;
    }
}

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">OtherClass</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> Method()
    {
        <span class="comment">// 同一アセンブリ内の他のクラス</span>
        <span class="comment">// コメントアウトしてないやつだけ OK</span>
        <span class="reserved">var</span> x = <span class="reserved">new</span> Base();

        x.Public = 0;
        <span class="comment">//x.Protected = 0;</span>
        x.Internal = 0;
        x.ProtectedInternal = 0;
        <span class="comment">//x.Private = 0;</span>
        <span class="comment">//x.PrivateProtected = 0;</span>
    }
}
</code></pre>

このコードとは別のプロジェクト内では、以下のような制限がかかります。

<pre class="source" title="他のプロジェクト内でのアクセス制限">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Derived</span> : ClassLibrary1.Base
{
    <span class="reserved">public</span> <span class="reserved">void</span> MethodInDerived()
    {
        <span class="comment">// 他のアセンブリ内の派生クラス</span>
        <span class="comment">// コメントアウトしてないやつだけ OK</span>

        Public = 0;
        Protected = 0;
        <span class="comment">//Internal = 0;</span>
        ProtectedInternal = 0;
        <span class="comment">//Private = 0;</span>
        <span class="comment">//PrivateProtected = 0; // ここが protected internal との差</span>
    }
}

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">OtherClass</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> Method()
    {
        <span class="comment">// 他のアセンブリ内の他のクラス</span>
        <span class="comment">// public 以外全滅</span>

        <span class="reserved">var</span> x = <span class="reserved">new</span> ClassLibrary1.Base();

        x.Public = 0;
        <span class="comment">//x.Protected = 0;</span>
        <span class="comment">//x.Internal = 0;</span>
        <span class="comment">//x.ProtectedInternal = 0;</span>
        <span class="comment">//x.Private = 0;</span>
        <span class="comment">//x.PrivateProtected = 0;</span>
    }
}
</code></pre>

ちなみに、`protected internal` と `private protected` では、語順は自由です。
`protected internal`と`internal protected`、`private protected`と`protected private`はそれぞれ同じ意味になります。

<pre class="source" title="protected internal と private protected の語順は自由">
<code><span class="comment">// どちらの順序でも同じ意味</span>
<span class="reserved">protected</span> <span class="reserved">internal</span> <span class="reserved">int</span> A1;
<span class="reserved">internal</span> <span class="reserved">protected</span> <span class="reserved">int</span> A2;

<span class="reserved">private</span> <span class="reserved">protected</span> <span class="reserved">int</span> B1;
<span class="reserved">protected</span> <span class="reserved">private</span> <span class="reserved">int</span> B2;
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="private-protected"></a>余談: private protected は C# コンパイラー上だけの問題

<h5 class="version version7_1">Ver. 7.2</h5>

余談となりますが、`private protected`相当のアクセシビリティは、[IL](../abstract/ab_dotnet.md#il)レベルでは 1.0 の頃からずっとあります。

C# | IL
--- | ---
public | public
protected | family
internal | assembly
protected internal | famorassem
private protected | famandassem
private | private

protectedを指してfamily、internalを指してassemblyと、別の単語を使っていますが意味は同じです。
famorassem、famandassemはそれぞれfamily <em>or</em> assembly、family <em>and</em> assemblyの意味です。

当初、fam<em>and</em>assem相当のアクセシビリティの需要を甘く見ていて、
`protected internal`をfam<em>or</em>assemの意味で用い、fam<em>and</em>assemは用意しませんでした。

元々あるものなので、`private protected`の追加は大して難しい作業ではありません。
しかし、キーワードをどうするかでかなり悩みました。
最初に追加することを考えたのは C# 6.0 の頃ですが、結局、C# 7.2まで延びました。

確かに、`private protected`と言われて「`protected` かつ `internal`」とは想像しにくいです。
一応、「`private`が混ざってるからより厳しい方」 = 「かつ」と覚えてください。

他のキーワードを導入するとか、`protected and internal`や`protected & internal`みたいな書き方も検討されましたが、
新しいキーワードの追加やこれ専用の文法の追加はコスト的に見合わないということで見送られました。
