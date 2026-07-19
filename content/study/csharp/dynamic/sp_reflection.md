---
title: "実行時型情報"
source_url: "https://ufcpp.net/study/csharp/dynamic/sp_reflection/"
content_type: "Article"
published_at: "2015-05-06T14:11:35"
updated_at: "2016-05-08T03:27:39"
tags: []
umbraco_id: 1313
parent_id: 1312
sort_order: 0
aliases:
  - "/csharp/dynamic/sp_reflection/"
  - "/csharp/sp_reflection"
  - "/csharp/sp_reflection.html"
  - "/study/csharp/sp_reflection"
  - "/study/csharp/sp_reflection.html"
---

# 実行時型情報

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
クラスを他のプログラムから利用できるようにするため、
プログラムやライブラリ中にはクラス名やメンバー名、それらのアクセスレベル等の情報が格納されています。
これらの情報は<strong id="metadata" class="keyword">メタデータ</strong>と呼ばれ、
プログラムの実行時にメタデータを取り出すための機能を<strong id="reflection" class="keyword">リフレクション</strong>（reflection）と呼びます。

(プログラムが自分自身の情報を調べることができる機能なので、reflection(鏡映、反射)と呼ぶわけです。)

さらに、C# ではクラスのインスタンスから実行時に型情報を取得したり、
リフレクション機能を利用して型情報からメタデータを取り出したりする機能が用意されています。
このようにして実行時に得られる型に関する情報を<strong id="rtti" class="keyword">実行時型情報</strong>（runtime type information）といいます。


##### <a id="sec-generated-title-2"></a>ポイント
* 構造体/クラス名やメンバー名などの情報は、プログラムを実行するだけなら不要な情報です。

* ですが、C# には、クラス名やメンバー名などの情報を実行時に取り出したり、 あるいは、クラス名の文字列からクラスのインスタンスを動的に生成したりする機能（リフレクション）があります。



##<a id="sec-generated-title-3"></a> <a id="gettype"></a>実行時型情報とは
実は、プログラムを実行するだけなら、実行時型情報は不要な情報です。

例えば、以下のような構造体を考えます。

<pre class="source" title="Rect 構造体" lang="">
<code><span class="reserved">struct</span> Rect
{
  <span class="reserved">public int</span> Width;
  <span class="reserved">public int</span> Height;
}
</code></pre>


で、以下のように、Rect 構造体のメンバーにアクセスすることを考えます。

<pre class="source" title="Rect 構造体の利用" lang="">
<code>Rect x = <span class="reserved">new</span> Rect();
x.Width = 3;
x.Height = 4;
</code></pre>


で、Rect 構造体は、コンピュータ内部では、表1のようなレイアウトになっています。
（実際にどうなるかは環境依存なんですが、32ビット CPU の場合は大体表1のようになります。）

<table summary="Rect 構造体のレイアウト">
	<caption>
		Rect 構造体のレイアウト
	</caption>
	<tr>
		<th>オフセット</th>
		<th>メンバー名</th>
	</tr>
	<tr>
		<td markdown="1">0</td>
		<td markdown="1" rowspan="4">Width</td>
	</tr>
	<tr>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<td markdown="1">4</td>
		<td markdown="1" rowspan="4">Height</td>
	</tr>
	<tr>
		<td markdown="1">5</td>
	</tr>
	<tr>
		<td markdown="1">6</td>
	</tr>
	<tr>
		<td markdown="1">7</td>
	</tr>
</table>


で、実行時には、Width とか Height とかのメンバー名を知る必要はなく、
このレイアウトさえ分かっていればメンバーにアクセスできます。
x.Width にアクセスしたければ変数 x の格納されている場所の先頭を、
x.Height ならば x から4バイト目を見ればいいことになります。

要するに、Rect 構造体のメンバーへのアクセスは、
実行時には、以下のような（C 言語風の）コードと同じような扱いになっています。

<pre class="source" title="C 言語的に書くと" lang="">
<code><span class="comment">// Rect x;</span>
char x[8];

<span class="comment">// x.Width = 3;</span>
*((int *)(x + 0)) = 3;

<span class="comment">// y.Height = 4;</span>
*((int *)(x + 4)) = 4;
</code></pre>


実行時型情報をサポートしない言語では、
実行時に不要な
「Rect 構造体は Width とか Height という名前のメンバーを持っている」
というような情報は削除されてしまいます。

一方、C# などのような、実行時型情報をサポートする言語では、
実行には直接不要ではあっても、
「Rect 構造体は Width とか Height という名前のメンバーを持っている」
という情報を持っていて、
実行時にこういった情報を引き出せるようになっています。


##<a id="sec-generated-title-4"></a> <a id="type"></a>実行時型情報の取得
C# では、System.Type クラスというものを使って実行時型情報の取得できます。

例として、以下のようなコードを考えてみます。
（あんまり意味のあるコードではないですけども、例ということで。）

<pre class="source" title="Rect 型のメンバーアクセス（通常のコード）" lang="">
<code>Rect x = <span class="reserved">new</span> Rect();
x.Width = 3;
x.Height = 4;
<span class="reserved">int</span> w = x.Width;
<span class="reserved">int</span> h = x.Height;
<span class="reserved">int</span> area = w * h;

Console.Write(<span class="literal">"{0} × {1} ＝ {2}\n"</span>, x.Width, x.Height, area);
</code></pre>


これを、リフレクション（実行時型情報の取得）機能を使って同じことをしようと思うと以下のようになります。

<pre class="source" title="Rect 型のメンバーアクセス（リフレクション版）" lang="">
<code>Type t = Type.GetType(<span class="literal">"Rect"</span>);

<span class="reserved">object</span> o = Activator.CreateInstance(t);

t.GetField(<span class="literal">"Width"</span>).SetValue(o, 3);
t.GetField(<span class="literal">"Height"</span>).SetValue(o, 4);

<span class="reserved">int</span> w = (<span class="reserved">int</span>)t.GetField(<span class="literal">"Width"</span>).GetValue(o);
<span class="reserved">int</span> h = (<span class="reserved">int</span>)t.GetField(<span class="literal">"Height"</span>).GetValue(o);
<span class="reserved">int</span> area = w * h;

Rect x = (Rect)o;
Console.Write(<span class="literal">"{0} × {1} ＝ {2}\n"</span>, x.Width, x.Height, area);
</code></pre>


見てのとおり、型名 Rect も、メンバー名 Width, Height も、文字列になっています。
実行時に、文字列からインスタンスを動的に生成しています。

この例では Type を文字列から生成していますが、
「[多態性](../oop/oo_polymorphism.md)」のところで説明したように、
GetType() メソッドや typeof 演算子を用いることでも Type型のインスタンスを取得することが出来ます。

リフレクション機能を利用するためのクラスは、
この例にも出てきた Type 型、Activator 型の他にもいろいろあって、
主に System.Reflection 名前空間内に定義されています。
例えば、上記の例の、t.GetField() メソッドの戻り値は System.Reflection.FieldInfo 型です。


##### <a id="sec-generated-title-5"></a>ポインター版
参考までに、
逆に 「[unsafe](../interop/sp_unsafe.md#unsafe)」 機能・ポインターを使って書くと、
以下のようになります。

<pre class="source" title="Rect 型のメンバーアクセス（ポインター版）" lang="">
<code><span class="comment">// 環境依存だし、ほんとはこんなコード書いちゃ駄目</span>

<span class="reserved">byte</span>* p = <span class="reserved">stackalloc byte</span>[<span class="reserved">sizeof</span>(Rect)];
*(<span class="reserved">int</span>*)(p + 0) = 3;
*(<span class="reserved">int</span>*)(p + 4) = 4;
<span class="reserved">int</span> w = *(<span class="reserved">int</span>*)(p + 0);
<span class="reserved">int</span> h = *(<span class="reserved">int</span>*)(p + 4);
<span class="reserved">int</span> area = w * h;

Rect x = *(Rect*)p;
Console.Write(<span class="literal">"{0} × {1} ＝ {2}\n"</span>, x.Width, x.Height, area);
</code></pre>



##### <a id="sec-generated-title-6"></a>実行速度
リフレクション機能を使うと、
例えば、テキスト形式で書かれた設定ファイルを読み込んで、
動的にインスタンスを生成したりといった面白いこともできるんですが、
実行速度は圧倒的に遅くなります。
例えば、ここで例示したようなコードの場合、
リフレクション版は通常版の数千倍くらい低速です。


##### <a id="sec-generated-title-7"></a>サンプル
リフレクションを使って、XML ファイルから動的に（実行時に）インスタンスを生成する簡単なプログラムを作りました。


[ソース一式](../../../../assets/media/ufcpp2000/csharp/source/Reflection.zip)


以下に、利用例をあげます。

<pre class="source" title="XML からインスタンス生成" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Xml.Linq;

<span class="reserved">namespace</span> Reflection
{
  <span class="reserved">class</span> Program
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      Load(
<span class="literal">@"
&lt;Triangle xmlns='Shape'&gt;
  &lt;P1&gt;&lt;Point&gt;&lt;X&gt;0&lt;/X&gt;&lt;Y&gt;0&lt;/Y&gt;&lt;/Point&gt;&lt;/P1&gt;
  &lt;P2&gt;&lt;Point&gt;&lt;X&gt;1&lt;/X&gt;&lt;Y&gt;0&lt;/Y&gt;&lt;/Point&gt;&lt;/P2&gt;
  &lt;P3&gt;&lt;Point&gt;&lt;X&gt;0&lt;/X&gt;&lt;Y&gt;2&lt;/Y&gt;&lt;/Point&gt;&lt;/P3&gt;
&lt;/Triangle&gt;
"</span>);

      Load(
<span class="literal">@"
&lt;Rectangle xmlns='Shape'&gt;
  &lt;Width&gt;3&lt;/Width&gt;
  &lt;Height&gt;4&lt;/Height&gt;
&lt;/Rectangle&gt;
"</span>);

      Load(
<span class="literal">@"
&lt;Circle xmlns='Shape'&gt;
  &lt;Radius&gt;2&lt;/Radius&gt;
&lt;/Circle&gt;
"</span>);
    }

    <span class="reserved">static void</span> Load(<span class="reserved">string</span> xml)
    {
      <span class="reserved">var</span> doc = XDocument.Parse(xml);
      <span class="reserved">var</span> p = (Shape.IShape)Loader.LoadFromXml(doc);
      Console.Write(<span class="literal">"{0}, {1}\n"</span>, p, p.GetArea());
    }
  }
}
</code></pre>
