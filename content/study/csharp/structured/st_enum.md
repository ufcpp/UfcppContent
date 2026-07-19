---
title: "列挙型"
source_url: "https://ufcpp.net/study/csharp/structured/st_enum/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2008-01-05T00:00:00"
tags: []
umbraco_id: 1241
parent_id: 1217
sort_order: 13
aliases:
  - "/csharp/st_enum"
  - "/csharp/st_enum.html"
  - "/csharp/structured/st_enum/"
  - "/study/csharp/st_enum"
  - "/study/csharp/st_enum.html"
---

# 列挙型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# では、<strong id="enum" class="keyword">列挙型</strong>（enumeration type）と呼ばれるものを利用することで、曜日などの特定の値しかとらないデータを表現することが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント

* 列挙型: 特定の値しか取らないようなもの（例えば曜日など）に対して使う型

* enum DayOfWeek { Monday, Tuesday, ... }



## <a id="sec-generated-title-3"></a> <a id="about"></a>列挙型とは

例えば、曜日は月・火・水・木・金・土・日の7つの値しか取りませんし、
英語の月は January, February, March, April, May, June, July, August, September, October, November, December の12個の値しか取りません。
その他にも、
飛行機の乗車クラス(エコノミー・ビジネス・ファースト)、
日本の年号(明治・大正・昭和・平成)、
性別(男・女)など、特定の値しか取らないものはたくさんあります。
C# で、このような特定の値しか取らない型を表現するためには列挙型というものを使います。

列挙型は以下のようにして定義します。

<pre class="source" title="列挙型の定義" lang="">
<code><span class="reserved">enum</span> <span class="input">列挙型名</span>
{
  <span class="input">メンバー1</span>, <span class="input">メンバー2</span>, …, <span class="input">メンバーn</span>
}
</code></pre>


列挙型を利用する側では以下のようにします。

<pre class="source" title="列挙型の利用" lang="">
<code><span class="input">列挙型名</span>.<span class="input">メンバー名</span>
</code></pre>


また、列挙型の値を <code>Console.Write</code> などに渡して表示すると、
メンバー名がそのまま表示されます。
例えば、和暦の年号を列挙型として定義すると以下のようになります。

<pre class="source" title="列挙型の例(年号)" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">enum</span> 年号
{
  明治, 大正, 昭和, 平成
}

<span class="reserved">class</span> EnumSample
{
  <span class="comment">/// &lt;summary&gt;
  /// 和暦を西暦に変換する
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> Main()
  {
    年号[] era = <span class="reserved">new</span> 年号[5]{年号.昭和, 年号.大正, 年号.明治, 年号.平成, 年号.昭和};
    <span class="reserved">int</span>[] j_year = <span class="reserved">new int</span>[5]{33, 12, 20, 10, 54};
    <span class="reserved">int</span>[] year = <span class="reserved">new int</span>[5];

    Console.Write(<span class="literal">"和暦      西暦\n"</span>);
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;5; ++i)
    {
      <span class="reserved">switch</span>(era[i])
      {
      <span class="reserved">case</span> 年号.明治: year[i] = j_year[i] + 1863; <span class="reserved">break</span>;
      <span class="reserved">case</span> 年号.大正: year[i] = j_year[i] + 1911; <span class="reserved">break</span>;
      <span class="reserved">case</span> 年号.昭和: year[i] = j_year[i] + 1925; <span class="reserved">break</span>;
      <span class="reserved">case</span> 年号.平成: year[i] = j_year[i] + 1988; <span class="reserved">break</span>;
      }

      Console.Write(<span class="literal">"{0}{1:d2}年  {2:d4}年\n"</span>, era[i], j_year[i], year[i]);
    }
  }
}
</code></pre>


<pre class="console" title="">
和暦      西暦
昭和33年  1958年
大正12年  1923年
明治20年  1883年
平成10年  1998年
昭和54年  1979年
</pre>



## <a id="sec-generated-title-4"></a> <a id="value"></a>列挙型の値

列挙型はプログラムの内部では整数として扱われていて、
整数型に変換することでその値を取り出すことが出来ます。
特に値や型を指定しなければ、列挙型は <code>int</code> として扱われ、
各メンバーは宣言した順番に 0, 1, 2, …, n となります。

例えば以下のような列挙型を定義すると、
<code>Mon, Tue, Wed, Thu, Fri, Sat, Sun</code>
の値はそれぞれ 0, 1, 2, 3, 4, 5, 6 になります。

<pre class="source" title="曜日をあらわす列挙型" lang="">
<code><span class="reserved">enum</span> DayOfWeek
{
  Mon, Tue, Wed, Thu, Fri, Sat, Sun
}
</code></pre>


列挙型の型や値は以下のようにすることで明示的に指定することも出来ます。

<pre class="source" title="列挙型の型と値の指定" lang="">
<code><span class="reserved">enum</span> <span class="input">列挙型名</span> : <span class="input">内部的な型</span>
{
  <span class="input">メンバー1</span> = <span class="input">メンバー1の値</span>,
  <span class="input">メンバー2</span> = <span class="input">メンバー2の値</span>,
   …,
  <span class="input">メンバーn</span> = <span class="input">メンバーnの値</span>
}
</code></pre>


また、1つ目のメンバーだけに値を指定すると、残りのメンバーの値は1つ目のメンバーの値から1ずつ増加した値になります。

例えば、<code>byte</code> 型で、値が1から始まる列挙型を定義したければ以下のようにします。

<pre class="source" title="列挙型の型と値を指定する例" lang="">
<code><span class="reserved">enum</span> Month<em> : byte</em>
{
  January<em> = 1</em>, February, March, April,
  May, June, July, August,
  September, October, November, December
}

<span class="reserved">class</span> EnumSample
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">for</span>(<span class="reserved">int</span> i=1; i&lt;12; ++i)
      Console.Write(<span class="literal">"{0}月  {1}\n"</span>, i, (Month)i);
  }
}
</code></pre>


<pre class="console" title="">
1月  January
2月  February
3月  March
4月  April
5月  May
6月  June
7月  July
8月  August
9月  September
10月  October
11月  November
</pre>


ちなみに、この例から分かるように、
列挙型を文字列化（ToString）すると、列挙型のメンバー名が表示されます。


## <a id="sec-generated-title-5"></a> <a id="flag"></a>フラグ

ときには、以下のような定数を定義したい場合もあります。

* 条件が n 個ある（例えば X, Y, Z の3つ）

* 「X かつ Y」とか「Y かつ Z」というような条件もありうる


こういう場合、列挙型を以下のように使って実現したりします。

<pre class="source" title="フラグとしての列挙体" lang="">
<code><span class="reserved">enum</span> Xyz
{
  X = 1, <span class="comment">// 001</span>
  Y = 2, <span class="comment">// 010</span>
  Z = 4, <span class="comment">// 100</span>
}

<span class="reserved">class</span> Program
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    Xyz xy = Xyz.X | Xyz.Y; <span class="comment">// 011</span>
    <span class="input">...</span>
  }
</code></pre>


列挙型の値を2の累乗にして、OR 演算をとります。

ただし、このままだと、Console.Write を使って表示するときに少し困ります。
以下の例の場合、X | Y は 3 になるわけですが、値が3のメンバーは Xyz 列挙型には定義されていないので、
表示結果は数値の3がそのまま表示されます。

<pre class="source" title="X | Y" lang="">
<code><span class="reserved">enum</span> Xyz
{
  X = 1, <span class="comment">// 001</span>
  Y = 2, <span class="comment">// 010</span>
  Z = 4, <span class="comment">// 100</span>
}

<span class="reserved">class</span> Program
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    Console.Write(<span class="literal">"{0}\n"</span>, Xyz.X);
    Console.Write(<span class="literal">"{0}\n"</span>, Xyz.Y);
    Console.Write(<span class="literal">"{0}\n"</span>, Xyz.Z);

    Xyz xy = Xyz.X | Xyz.Y;
    Console.Write(<span class="literal">"{0}\n"</span>, xy);
  }
}
</code></pre>


<pre class="console" title="結果">
X
Y
Z
3
</pre>


これに対して、列挙型に Flags 属性を付けると、以下のような表示結果が得られるようになります。
（属性に関しては「[属性](../dynamic/sp_attribute.md)」を参照。）

<pre class="source" title="Flags 属性を付ける" lang="">
<code>[Flags]
<span class="reserved">enum</span> Xyz
{
  X = 1, <span class="comment">// 001</span>
  Y = 2, <span class="comment">// 010</span>
  Z = 4, <span class="comment">// 100</span>
}

<span class="reserved">class</span> Program
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    Xyz xy = Xyz.X | Xyz.Y;
    Console.Write(<span class="literal">"{0}\n"</span>, xy);

    Xyz yz = Xyz.Y | Xyz.Z;
    Console.Write(<span class="literal">"{0}\n"</span>, yz);

    Xyz zx = Xyz.Z | Xyz.X;
    Console.Write(<span class="literal">"{0}\n"</span>, zx);

    Xyz xyz = Xyz.X | Xyz.Y | Xyz.Z;
    Console.Write(<span class="literal">"{0}\n"</span>, xyz);
  }
}
</code></pre>


<pre class="console" title="結果">
X, Y
Y, Z
X, Z
X, Y, Z
</pre>



## <a id="sec-generated-title-6"></a> <a id="plan"></a>追加予定

System.Enum クラスから派生。
特別扱い（他の値型同様）。

いくつかメソッド紹介。
インスタンス: ToString, HasFlag。
静的: IsDefined, GetName, GetNames, GetValues, TryParse
