---
title: "拡張メソッド"
source_url: "https://ufcpp.net/study/csharp/functional/sp3_extension/"
content_type: "Article"
published_at: "2008-08-15T00:00:00"
updated_at: "2018-03-25T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1284
parent_id: 1275
sort_order: 10
aliases:
  - "/csharp/functional/sp3_extension/"
  - "/csharp/sp3_extension"
  - "/csharp/sp3_extension.html"
  - "/study/csharp/sp3_extension"
  - "/study/csharp/sp3_extension.html"
---

# 拡張メソッド

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

拡張メソッドは、静的メソッドをインスタンスメソッドと同じ形式で呼び出せるようにできるものです。
すなわち、
今までなら、

<pre class="source" title="静的メソッド" lang="">
<code><span class="reserved">int</span> x = <span class="reserved">int</span>.Parse(<span class="literal">"1"</span>);      
</code></pre>


と書いていたものを、

<pre class="source" title="拡張メソッドの定義" lang="">
<code><span class="reserved">static class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public static int</span> Parse(<span class="reserved">this string</span> str)
    {
        <span class="reserved">return int</span>.Parse(str);
    }
}
</code></pre>


というような静的メソッドを用意することで、
以下のような構文で呼び出せるようになります。

<pre class="source" title="拡張メソッドの利用" lang="">
<code><span class="reserved">int</span> x = <span class="literal">"1"</span>.Parse();
</code></pre>



##### <a id="sec-generated-title-2"></a>ポイント

* 拡張メソッド： 静的メソッドをインスタンスメソッドと同じ書式で呼び出せるようにすることで、 あたかもクラスに新しいメソッドを追加したかのように見せかける仕組みです。

* 単に、静的メソッドを後置き記法で呼び出せるようになっただけとも考えることができます。

* 定義側： 第1引数の前に this を付けます。

* 利用側： インスタンスメソッドと同じ書き方をします。



## <a id="sec-generated-title-3"></a> <a id="extension"></a>拡張メソッド

C# 2.0 までの常識で言うと、
既存のクラスの機能拡張（＝メソッドの追加）をしたければ、
そのクラスを継承したりなどして、新しいクラスを作るしかありませんでした。

これに対して、C# 3.0 では、後述する方法で、
既存のクラスにメソッドを追加できます。
（正確には、インスタンスメソッドの“ようなもの”。インスタンスメソッドと同じ構文で呼べるだけ。）
このような、後から追加するメソッドのことを<strong id="exmethod" class="keyword">拡張メソッド</strong>（extension method）と呼びます。

まず、拡張メソッドの定義の仕方ですが、
以下のように、
<em>「[静的クラス](../oop/oo_static.md#stclass)」中に、
      第一引数に this キーワードを修飾子として付けた static メソッドを書きます</em>。

<pre class="source" title="拡張メソッドの定義" lang="">
<code><span class="reserved">static class</span> <span class="type">StringExtensions</span>
{
  <span class="reserved">public static string</span> ToggleCase(<span class="reserved"><em>this</em> string</span> s)
  <span class="input">中身省略</span>
}
</code></pre>


このようにして定義したメソッドは、
通常通り、静的メソッドとして呼び出すこともできますが、
あたかも string 型のインスタンスメソッドであるかのように呼び出せるようになります。

<pre class="source" title="拡張メソッドの呼び出し" lang="">
<code><span class="reserved">string</span> s = <span class="literal">"This Is a Test String."</span>;
<span class="reserved">string</span> s1 = StringExtensions.ToggleCase(s); <span class="comment">// 通常の呼び出し方。</span>
<span class="reserved">string</span> s1 = <em>s.ToggleCase()</em>;                 <span class="comment">// 拡張メソッド呼び出し。</span>
</code></pre>


上述のような拡張メソッドの利用例のソース全てを以下に示します。

<pre class="source" title="拡張メソッドの例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">namespace</span> ConsoleApplication1
{
  <span class="reserved">static class</span> <span class="type">StringExtensions</span>
  {
    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> 文字列の大文字と小文字を入れ替える。</span>
    <span class="inactive">/// &lt;/summary&gt;
    /// &lt;param name="s"&gt;</span><span class="comment">変換元</span><span class="inactive">&lt;/param&gt;
    /// &lt;returns&gt;</span><span class="comment">変換結果</span><span class="inactive">&lt;/returns&gt;</span>
    <span class="reserved">public static string</span> ToggleCase(<span class="reserved">this string</span> s)
    {
      System.Text.<span class="type">StringBuilder</span> sb = <span class="reserved">new</span> System.Text.<span class="type">StringBuilder</span>();
      <span class="reserved">foreach</span>(<span class="reserved">char</span> c <span class="reserved">in</span> s)
      {
        <span class="reserved">if</span>(<span class="reserved">char</span>.IsUpper(c))
          sb.Append(<span class="reserved">char</span>.ToLower(c));
        <span class="reserved">else if</span>(<span class="reserved">char</span>.IsLower(c))
          sb.Append(<span class="reserved">char</span>.ToUpper(c));
        <span class="reserved">else</span>
          sb.Append(c);
      }
      <span class="reserved">return</span> sb.ToString();
    }
  }

  <span class="reserved">class</span> <span class="type">ExtensionMethodTest</span>
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      <span class="reserved">string</span> s = <span class="literal">"This Is a Test String."</span>;
      <span class="type">Console</span>.Write(s.ToggleCase());
    }
  }
}
</code></pre>


<pre class="console" title="拡張メソッドの例">
tHIS iS A tEST sTRING.
</pre>



## <a id="sec-generated-title-4"></a> <a id="using"></a>using ディレクティブによる拡張メソッドのインポート

通常、静的メソッドは「クラス名.メソッド名」という記法で呼び出します。
ところが、拡張メソッドでは、「クラス名」の部分をさぼって書けるようになっています。

じゃあ、どうやって「どのメソッドが呼ばれるか」を決定しているかというと、
<em>「[using ディレクティブ](../structured/sp_namespace.md#using)」で指定した名前空間中のにある拡張メソッドが参照される</em>ようになっています。

そのため、同じ名前空間内に2つ以上同名の拡張メソッドを定義してはいけません。

<pre class="source" title="同名の拡張メソッドがあるせいでエラーに" lang="">
<code><span class="reserved">namespace</span> ConsoleApplication1
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static void</span> Main()
        {
            <span class="type">Console</span>.Write(<span class="literal">1</span>.Square()); <span class="comment">// エラーになる</span>
        }
    }

    <span class="reserved">static class</span> <span class="type">Extensions1</span>
    {
        <span class="reserved">public static int</span> Square(<span class="reserved">this int</span> x)
        {
            <span class="reserved">return</span> x * x;
        }
    }

    <span class="reserved">static class</span> <span class="type">Extensions2</span>
    {
        <span class="reserved">public static int</span> Square(<span class="reserved">this int</span> x) <span class="comment">// エラーの原因</span>
        {
            <span class="reserved">return</span> x * x;
        }
    }
}
</code></pre>


同名の拡張メソッドが定義されている名前空間を同時に using するのもご法度です。

<pre class="source" title="using でどのメソッドが呼ばれるかが決まる" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">namespace</span> ConsoleApplication1
{
    <span class="reserved">using</span> NamespaceA;
    <span class="comment">//using NamespaceB;
    // ↑
    // ここのコメントを外してもやっぱりエラー。
    // using NamespaceA をコメントアウトして、
    // 代りに using NamespaceB するなら OK（表示結果が変わる）。</span>

    <span class="reserved">class</span> <span class="type">Program</span>   
    {
        <span class="reserved">static void</span> Main()
        {
            <span class="literal">1</span>.WriteToConsole();
            <span class="comment">// ↑
            // NamespaceA.Extensions.WriteToConsole が呼ばれる</span>
        }
    }
}

<span class="reserved">namespace</span> NamespaceA
{
    <span class="reserved">static class</span> <span class="type">Extensions</span>
    {
        <span class="reserved">public static void</span> WriteToConsole(<span class="reserved">this int</span> x)
        {
            <span class="type">Console</span>.Write(<span class="literal">"A {0}"</span>, x);
        }
    }
}

<span class="reserved">namespace</span> NamespaceB
{
    <span class="reserved">static class</span> <span class="type">Extensions</span>
    {
        <span class="reserved">public static void</span> WriteToConsole(<span class="reserved">this int</span> x)
        {
            <span class="type">Console</span>.Write(<span class="literal">"B {0}"</span>, x);
        }
    }
}
</code></pre>



## <a id="sec-generated-title-5"></a> <a id="priority"></a>優先順位

拡張メソッドのせいで、
同じ名前のメソッドがいくつか同時に定義されてしまう可能性があります。
その場合、どのメソッドが呼ばれるか優先順位が決まっています。

### <a id="sec-generated-title-6"></a> <a id="instance-over-extension"></a>インスタンス メソッド優先

まず、拡張メソッドよりも通常のインスタンスメソッドの方が優先されます。

<pre class="source" title="インスタンスメソッド優先" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="type">Console</span>.Write(<span class="literal">1</span>.ToString());
        <span class="comment">// ↑
        // Extensions.ToString ではなく、
        // int.ToString が呼ばれる。</span>
    }
}

<span class="reserved">static class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public static string</span> ToString(<span class="reserved">this int</span> x)
    {
        <span class="reserved">return</span> <span class="literal">"dummy data"</span>;
    }
}
</code></pre>

#### <a id="sec-generated-title-7"></a> <a id="overload"></a>オーバーロード解決ルールより、インスタンス メソッド優先が強い

通常、オーバーロードが複数ある場合は一番引数の一致度が高いものが呼ばれます。
例えば、以下のコードの場合は、`object`引数のものより`string`引数のものがまず優先、`string`に合わない場合だけ`object`のものが呼ばれます。

<pre class="source" title="オーバーロード解決">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> F(<span class="reserved">object</span> x) =&gt; WriteLine(<span class="string">$"object </span>{x}<span class="string">"</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> F(<span class="reserved">string</span> x) =&gt; WriteLine(<span class="string">$"string </span>{x}<span class="string">"</span>);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">X</span>();
        x.F(<span class="string">"abc"</span>); <span class="comment">// string のが呼ばれる</span>
        x.F(10);    <span class="comment">// int のオーバーロードがないので object のが呼ばれる</span>
    }
}
</code></pre>
<pre class="console" title="実行結果">
<code>string abc
object 10
</code></pre>

ここで、`int`引数の拡張メソッドを足してみましょう。
しかし、拡張メソッドよりもインスタンス メソッドの方が優先的に呼ばれます。
引数の一致度が高くても、拡張メソッドの方は呼ばれません。

<pre class="source" title="インスタンス メソッドと拡張メソッドの混在">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> F(<span class="reserved">object</span> x) =&gt; WriteLine(<span class="string">$"object </span>{x}<span class="string">"</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> F(<span class="reserved">string</span> x) =&gt; WriteLine(<span class="string">$"string </span>{x}<span class="string">"</span>);
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">XExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="type">X</span> @this, <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">$"int </span>{x}<span class="string">"</span>);

}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">X</span>();
        x.F(<span class="string">"abc"</span>); <span class="comment">// string のが呼ばれる</span>
        x.F(10);    <span class="comment">// int な拡張が増えたものの、インスタンス メソッド優先で object のが呼ばれる</span>
    }
}
</code></pre>
<pre class="console" title="実行結果">
<code>string abc
object 10
</code></pre>


### <a id="sec-generated-title-8"></a> <a id="namespace"></a>名前空間の優先度

名前空間違いで複数の拡張メソッドを定義することもできます。
この場合、優先度付けは名前空間の仕様に準じます:

- [名前空間 > 名前解決の優先度](../structured/sp_namespace.md#priority)

特に、拡張メソッドを拡張メソッドとして呼びたい場合、完全修飾名は使えません。
上記ページの優先度付けが唯一の呼び分け手段になります。
以下のように、使う場所に近いほど優先、直接的なものほど優先で呼べます。
同優先度のものが複数ある場合はコンパイル エラーになります。

<pre class="source" title="複数の名前空間にある拡張メソッドの呼び分け">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;
<span class="reserved">using</span> A;

<span class="reserved">using</span> <span class="type">Lib</span> = C.<span class="type">Lib</span>;
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">"global"</span>); }

<span class="reserved">namespace</span> MyApp
{
    <span class="reserved">using</span> B;

    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">"MyApp"</span>); }

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="comment">// F 拡張メソッドは5つある</span>
            <span class="comment">// この場合 MyApp.Lib.F が使われる</span>
            <span class="comment">// 優先度 高 MyApp &gt; B &gt; global = C &gt; A 低</span>
            10.F();

            <span class="comment">// ちゃんと呼び分けたければ拡張メソッドとして使うことをあきらめる</span>
            <span class="comment">// 完全修飾名を使って、普通の静的メソッドとして呼ぶ</span>
            A.<span class="type">Lib</span>.F(10);
            B.<span class="type">Lib</span>.F(10);
            C.<span class="type">Lib</span>.F(10);
            MyApp.<span class="type">Lib</span>.F(10);
            <span class="reserved">global</span>::<span class="type">Lib</span>.F(10);
        }
    }
}

<span class="reserved">namespace</span> A
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">"A"</span>); }
}
<span class="reserved">namespace</span> B
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">"B"</span>); }
}
<span class="reserved">namespace</span> C
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Lib</span> { <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; WriteLine(<span class="string">"C"</span>); }
}
</code></pre>


## <a id="sec-generated-title-9"></a> <a id="interface"></a>インターフェースに拡張メソッドを追加

拡張メソッドでは、1つ、通常のインスタンスメソッドにはできないことができます。
それは、「[インターフェース](../oop/oo_interface.md#interface)」に対して、
インスタンスメソッド風のメソッドを定義できると言うことです。

通常、「[インターフェース](../oop/oo_interface.md#interface)」は、メソッドの外部仕様のみを定義でき、
実装は定義できません。
しかしながら、拡張メソッドを利用することで、
インスタンスメソッド定義っぽいことが実現できます。

<pre class="source" title="インターフェースに対する拡張メソッド定義" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;

<span class="reserved">static class</span> <span class="type">Extensions</span>
{
  <span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;T&gt; Duplicate&lt;T&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;T&gt; list)
  {
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> list)
    {
      <span class="reserved">yield return</span> x;
      <span class="reserved">yield return</span> x;
    }
  }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; data = <span class="reserved">new int</span>[]{ <span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">3</span> };

    <span class="comment">// ↓インターフェースに対してメソッドを追加できる</span>
    data = data.Duplicate();

    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> data)
      <span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, x);
  }
}
</code></pre>


C# 3.0 では、IEnumerable インターフェースなどに、
拡張メソッドとして Where や Select などのメソッド（「[標準クエリ演算子](../data/sp3_linq.md#std_query_op)」）が定義されています。


## <a id="sec-generated-title-10"></a> <a id="problem"></a>拡張メソッドの問題点

ちなみに、インスタンス メソッドでも拡張メソッドでもどちらでもいい場合、拡張メソッドの濫用は避けた方がいいでしょう。
拡張メソッドの濫用には不便な点もありますし、
いくつか問題を起こす可能性があります。

##### <a id="sec-generated-title-11"></a>実体はあくまで静的メソッド

拡張メソッドは、
呼び出し側だけ見ると、一見、クラスにメソッドが追加されたように思えますが、
その実態はあくまで静的メソッドです。
それも、元のクラス中ではなく、別の静的クラスの中で定義された静的メソッドです。

元のクラスからみれば当然「外部」なので、
拡張メソッドから private / protected メンバーにアクセスすることはできません。


##### <a id="sec-generated-title-12"></a>定義場所がどこかわからなくなる

クラス本体と別の場所にメソッド定義があるため、
定義された場所を探すのに苦労する可能性があります。

しかも、using 文を使ってインポートするため、
using 文1つでどの静的メソッドが呼ばれるのかが切り替わって、
なおのことどこに定義があるのかわかりにくくなっています。


## <a id="sec-generated-title-13"></a> <a id="significance"></a>拡張メソッドの意義

前節の通り、実を言うと、拡張メソッドは両手ばなしによろこべる機能ではなかったりします。
インスタンス メソッドでの実装が可能ならば素直にクラスのインスタンス メソッドとして定義すべきです。

拡張メソッドは、「クラスを作った人とは全くの別人がメソッドを足せる」という点が最大のメリットです。
このメリットは、特にインターフェイスに対して需要があります。
多くの場合、インターフェイスを作る人と、そのインターフェイスを使った処理を書く人は別です。
通常、この「インターフェイスを使った処理」は静的メソッドになりがちです。
そして、拡張メソッドの真骨頂は「<em>（本来は前置き記法である）静的メソッドを後置き記法で書ける</em>」という部分にあると思っています。

例えば、下図のような、データ列に対するパイプライン処理を考えてみます。

<figure>
	[![パイプライン処理](../../../../assets/media/ufcpp2000/csharp/fig/extension01.png)](../../../../assets/media/ufcpp2000/csharp/fig/extension01.png)
	<figcaption>パイプライン処理</figcaption>
</figure>


まず、条件付けや値の加工のために以下のような静的メソッドを用意します。

<pre class="source" title="データ列の選択・加工用のメソッド" lang="">
<code><span class="reserved">static class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Where(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; array, <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; pred)
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
            <span class="reserved">if</span> (pred(x))
                <span class="reserved">yield return</span> x;
    }

    <span class="reserved">public static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Select(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; array, <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; filter)
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
            <span class="reserved">yield return</span> filter(x);
    }
}
</code></pre>


これを、静的メソッド呼び出しの構文で書くと以下のようになります。

<pre class="source" title="静的メソッドによるデータ列のパイプライン処理" lang="">
<code><span class="reserved">var</span> input = <span class="reserved">new</span>[] { <span class="literal">8</span>, <span class="literal">9</span>, <span class="literal">10</span>, <span class="literal">11</span>, <span class="literal">12</span>, <span class="literal">13</span> };

<span class="reserved">var</span> output =
    <span class="type">Extensions</span>.Select(
        <span class="type">Extensions</span>.Where(
            input,
            x =&gt; x &gt; <span class="literal">10</span>),
        x =&gt; x * x);
</code></pre>


やりたいパイプライン処理の順序と、語順が逆になります。
また、「Where とそれに対する条件式 x &gt; 10」や
「Select とそれに対する加工式 x * x」の位置が離れてしまいます。

これに対して、拡張メソッド構文を使うと、以下のようになります。

<pre class="source" title="拡張メソッドによるデータ列のパイプライン処理" lang="">
<code><span class="reserved">var</span> input = <span class="reserved">new</span>[] { <span class="literal">8</span>, <span class="literal">9</span>, <span class="literal">10</span>, <span class="literal">11</span>, <span class="literal">12</span>, <span class="literal">13</span> };

<span class="reserved">var</span> output = input
    .Where(x =&gt; x &gt; <span class="literal">10</span>)
    .Select(x =&gt; x * x);
</code></pre>


ただ語順が違うだけなんですが、
こちらの方がやりたいことの意図が即座に伝わります。
すなわち、パイプライン処理（フィルタリング処理）は、
後置きの語順が好ましい処理です。

というように、
語順的に後置きの方がしっくりくる場合に
（というか、むしろその場合のみに）、
静的メソッドを拡張メソッド化することをお勧めします。


## <a id="sec-generated-title-14"></a> <a id="delegate"></a>拡張メソッドのデリゲートへの代入

拡張メソッドは、インスタンスメソッドと同じ構文で静的メソッドを呼べるものなわけですが、
デリゲートへの代入時にも、インスタンスメソッドと同じ構文で書けたりします。
（ただし、少々制約あり。）

すなわち、以下のようなコードは合法です。

<pre class="source" title="拡張メソッドのデリゲートへの代入" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">namespace</span> ConsoleApplication1
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static void</span> Main()
        {
            <span class="type">Func</span>&lt;<span class="reserved">string</span>&gt; f = <span class="literal">"test"</span>.Duplicate;
            <span class="comment">// ↑
            // 実行結果的には
            // Func&lt;string&gt; f = () =&gt; Extensions.Duplicate("test");
            // と同じ。
            // コンパイル結果的には、こんな余計な匿名デリゲートはできないらしい。
            // 直接 f に Extensions.Duplicate("test") が代入されるようなイメージ。</span>
        }
    }

    <span class="reserved">static class</span> <span class="type">Extensions</span>
    {
        <span class="reserved">public static string</span> Duplicate(<span class="reserved">this string</span> x)
        {
            <span class="reserved">return</span> x + x;
        }
    }
}
</code></pre>

こういうように、メソッドの引数を何らかの値で束縛して、新しいデリゲートを作ることをカリー化（currying）といいます。
また、上述のようなデリゲートの作り方をカリー化デリゲート（curried delegate）というそうです
（curry は人名に由来する単語らしくて、他に意味はない）。
詳細は「[カリー化デリゲート](miscdelegateinternal.md#curried-delegate)」で説明します。

ただし、カリー化デリゲートが作れるのは参照型の変数のみです。
値型の場合にはエラーになります。

## <a id="sec-generated-title-15"></a> <a id="ref-extensions"></a>参照渡しの拡張メソッド

<h5 class="version version7">Ver. 7.2</h5>

C# 7.2 から、拡張メソッドの第1引数(`this`が付いている引数)を参照渡し([`ref`](../resource/sp_ref.md#sec-byref)もしくは[`in`](../resource/sp_ref.md#in))で渡せるようになりました。
(ただし、構造体に対してのみです。クラスの場合は今まで通り、値渡ししかできません。)

以下のように書けます。`ref`引数の拡張メソッドで構造体の書き換えができたり、コピー除けのために`in`引数が使えます。

<pre class="source" title="参照渡しの拡張メソッドの例">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">QuaternionExtensions</span>
{
    <span class="comment">// 構造体の書き換えを拡張メソッドでやりたい場合に ref 引数が使える</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Conjugate(<em><span class="reserved">ref</span> <span class="reserved">this</span></em> <span class="type">Quaternion</span> q)
    {
        <span class="reserved">var</span> norm = q.W * q.W + q.X * q.X + q.Y * q.Y + q.Z * q.Z;
        q.W = q.W / norm;
        q.X = -q.X / norm;
        q.Y = -q.Y / norm;
        q.Z = -q.Z / norm;
    }

    <span class="comment">// コピーを避けたい場合に in 引数が使える</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Quaternion</span> Rotate(<em><span class="reserved">in</span> <span class="reserved">this</span></em> <span class="type">Quaternion</span> p, <span class="reserved">in</span> <span class="type">Quaternion</span> q)
    {
        <span class="reserved">var</span> qc = q;
        qc.Conjugate();
        <span class="reserved">return</span> q * p * qc;
    }
}

<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Quaternion</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> W;
    <span class="reserved">public</span> <span class="reserved">double</span> X;
    <span class="reserved">public</span> <span class="reserved">double</span> Y;
    <span class="reserved">public</span> <span class="reserved">double</span> Z;
    <span class="reserved">public</span> Quaternion(<span class="reserved">double</span> w, <span class="reserved">double</span> x, <span class="reserved">double</span> y, <span class="reserved">double</span> z) =&gt; (W, X, Y, Z) = (w, x, y, z);

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Quaternion</span> <span class="reserved">operator</span> *(<span class="reserved">in</span> <span class="type">Quaternion</span> a, <span class="reserved">in</span> <span class="type">Quaternion</span> b)
        =&gt; <span class="reserved">new</span> <span class="type">Quaternion</span>(
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
            a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
}
</code></pre>

ちなみに、
古いバージョンの[コンパイラー](https://www.nuget.org/packages/Microsoft.Net.Compilers/)(バージョン2.6)では、
修飾子の順序が`ref this`、`in this`の順でないと受け付けないという挙動でした。
2.7 以降では逆(`this ref`、`this in`)の順でも大丈夫です。

### <a id="sec-generated-title-16"></a> <a id="only-struct"></a>補足: 構造体のみ

すでに触れてはいますが、参照渡しで拡張メソッドを作れるのは[構造体](../resource/rm_struct.md)(値型)だけです。
以下のように、クラスではできません。また、ジェネリックな型の場合、[`struct`制約](../oop/sp2_generics.md#where)が必要です(ただし、それでも`in`引数は不可)。

<pre class="source" title="参照渡しの拡張メソッドを作れるのは構造体だけ">
<code><span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="comment">// 構造体(値型)は OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">ref</span> <span class="reserved">this</span> <span class="reserved">int</span> x) { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> MI(<span class="reserved">in</span> <span class="reserved">this</span> <span class="reserved">int</span> x) { }

    <span class="comment">// クラス(参照型)はダメ。コンパイル エラー</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="error">M</span>(<span class="reserved">ref</span> <span class="reserved">this</span> <span class="reserved">string</span> x) { }

    <span class="comment">// 制約が付いていないとダメ。コンパイル エラー</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="error">M1</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="reserved">this</span> T x) { }

    <span class="comment">// ref の場合、struct 制約が付いていれば OK</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M2&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="reserved">this</span> T x) <span class="reserved">where</span> T : <span class="reserved">struct</span> { }

    <span class="comment">// in の場合、struct 制約が付いてもダメ</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="error">M3</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">in</span> <span class="reserved">this</span> T x) <span class="reserved">where</span> T : <span class="reserved">struct</span> { }
}
</code></pre>

こういう仕様になっている理由ですが、
まず、クラスについては拡張メソッドの中で参照を書き換えられることを心配してのことだそうです。
通常の[参照引数](../resource/sp_ref.md#sec-byref)の場合は呼ぶ側で`M(ref s)`と言うように`ref`を付ける必要があるので、
`s`が書き換わる可能性があることが呼ぶ側でもわかりやすいです。
一方で、拡張メソッドの場合は`ref`を付けない仕様なので、知らないうちに書き換わる可能性があり、これを禁止したかったわけです。

<pre class="source" title="クラスの引数を ref this にできない理由">
<code><span class="comment">// (もしもこれをコンパイル エラーにしなかった場合)</span>
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">ref</span> <span class="reserved">this</span> <span class="reserved">string</span> s)
{
    <span class="comment">// 拡張メソッドの中で参照を書き換える</span>
    s = <span class="reserved">null</span>;
}

<span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">var</span> s = <span class="string">"abc"</span>;
    s.M(); <span class="comment">// M の中で s = null される</span>
    Console.WriteLine(s); <span class="comment">// null になってる</span>
}
</code></pre>

`in`引数では`struct`制約付きのジェネリック型も認めていない理由については、
コピー発生を避けることができなくて、`in`引数である意味が全くなくなるからだそうです。
詳しくは「[参照渡し](../resource/sp_ref.md#in-copy)」の項で説明しますが、
`in`引数はパフォーマンス改善を目的とした機能ですが、
正しく使わないとかえってパフォーマンスを損ねます。
ジェネリックな構造体に対する`in`引数はまさにパフォーマンスを損ねるため、最初から禁止することにしました。

<pre class="source" title="ジェネリックな構造体を in this にできない理由">
<code><span class="comment">// (もしもこれをコンパイル エラーにしなかった場合)</span>
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="reserved">in</span> <span class="reserved">this</span> T s)
    <span class="reserved">where</span> T : IDisposable
{
    <span class="comment">// 結局、この Dispose 呼び出しのところでコピーが起こる</span>
    <span class="comment">// コピーを避けるためには T が readonly struct でないとダメ</span>
    <span class="comment">// インターフェイス越しなので readonly struct かどうかの判定が不可能</span>
    s.Dispose();
    <span class="comment">// しかも、メソッドを呼ぶたびにコピー</span>
    s.Dispose();
}
</code></pre>

### <a id="sec-generated-title-17"></a> <a id="struct-field"></a>構造体のフィールドの参照

「[参照渡し](../resource/sp_ref.md#struct-this)」で振れていますが、構造体のインスタンス メソッドでは、その構造体のフィールドの参照を返せません
(その方が都合のいい場面がある)。

この制約に対する救済策として、`ref`引数の拡張メソッドが使えます。
例えば以下のように、インスタンス メソッドではコンパイル エラーになる`ref`戻り値が、拡張メソッドではコンパイルできます。

<pre class="source" title="拡張メソッドならフィールドを ref で返せる">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;
    <span class="reserved">public</span> <span class="reserved">int</span> Z;

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> At(<span class="reserved">int</span> index)
    {
        <span class="reserved">switch</span> (index)
        {
            <span class="comment">// インスタンス メソッド(プロパティ、インデクサー)では以下の ref が認められていない(コンパイル エラー)</span>
            <span class="reserved">case</span> 0: <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">X</span>;
            <span class="reserved">case</span> 1: <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">Y</span>;
            <span class="reserved">case</span> 2: <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">Z</span>;
            <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> IndexOutOfRangeException();
        }
    }
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">PointExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> At(<span class="reserved">ref</span> <span class="reserved">this</span> Point p, <span class="reserved">int</span> index)
    {
        <span class="reserved">switch</span> (index)
        {
            <span class="comment">// インスタンス メソッド版とやっていることは同じでも、こちらは OK</span>
            <span class="reserved">case</span> 0: <span class="reserved">return</span> <span class="reserved">ref</span> p.X;
            <span class="reserved">case</span> 1: <span class="reserved">return</span> <span class="reserved">ref</span> p.Y;
            <span class="reserved">case</span> 2: <span class="reserved">return</span> <span class="reserved">ref</span> p.Z;
            <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> IndexOutOfRangeException();
        }
    }
}
</code></pre>
