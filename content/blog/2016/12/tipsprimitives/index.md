---
title: "小ネタ プリミティブ型"
source_url: "https://ufcpp.net/blog/2016/12/tipsprimitives/"
content_type: "BlogEntry"
published_at: "2016-12-19T21:05:28"
updated_at: "2016-12-22T13:29:56"
tags: []
umbraco_id: 2002
parent_id: 1969
sort_order: 18
aliases: []
---

# 小ネタ プリミティブ型

.NETには「プリミティブ型」とかいうものがあるんですが、
何をもってプリミティブと言えるのか、
どういう型がプリミティブ型なのかというと、
なんかよくわからない存在です。

`Type`型に`IsPrimitive`というプロパティがあって、こいつが`true`を返すものがプリミティブ型なんですが。
以下のコードを見ての通り、どういう基準なのかがパッと見でわからず。

<pre class="source" title="IsPrimitiveの結果">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">int</span>).IsPrimitive);     <span class="comment">// true</span>
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">bool</span>).IsPrimitive);    <span class="comment">// true</span>
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">double</span>).IsPrimitive);  <span class="comment">// true</span>
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">object</span>).IsPrimitive);  <span class="comment">// false!</span>
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">string</span>).IsPrimitive);  <span class="comment">// false!</span>
        WriteLine(<span class="reserved">typeof</span>(<span class="reserved">decimal</span>).IsPrimitive); <span class="comment">// false!</span>
        WriteLine(<span class="reserved">typeof</span>(System.<span class="type">IntPtr</span>).IsPrimitive); <span class="comment">// true!</span>
    }
}
</code></pre>

## primitive

primitiveという単語の意味は、原始的とか基本的とかそんな意味なわけですが。
C#とかのプログラミング言語において「原始的」っていうのは、内部的に専用命令などを持っていて、
ユーザー定義のクラスや構造体ではできない何らかの特別扱いを受けているという意味になります。

特にC#の場合には、C#のレベルではなく、.NETランタイム的に、専用のIL (中間言語、Intermediate Language)命令を持っているかどうかが1つの基準なんですが…
それにしても分類は結構あいまいで、よくわからなかったりします。

## 元々はJavaから

元々はJavaから来ているんですかね。Javaの場合は割かしプリミティブ型がはっきりしています。
以下の3つが一致していて、これこそがプリミティブ型です。

- `int`とか`boolean`みたいに、専用のキーワードがある
- 言語的に許される唯一の値型
- 中間言語(bytecode)的に専用命令を持ってる

C#というか.NETでよくわからなくなる理由は、

- `object`、`string`、`decimal`にも専用のキーワードがある
- 構造体や列挙型があるのでいくらでも値型を作れる
- 専用命令を持っているって意味では`string`もちょっとだけ命令を持っている

というあたり。

## .NETのプリミティブ事情

.NET だと、

- `IsPrimitive`が`true`な型は以下の通り
  -  `Boolean`, `Byte`, `SByte`, `Int16`, `UInt16`, `Int32`, `UInt32`, `Int64`, `UInt64`, `IntPtr`, `UIntPtr`, `Char`, `Double`, `Single`
  - C#のキーワードになってる型から、
      - `decimal`, `object`, `string`は除く
      - `IntPtr`, `UIntPtr`を加える
- `object`、`string`はキーワードになってるけど参照型
- 構造体や列挙型があるので、いくらでも値型を作れる
- `string`はプリミティブ型とかと比べると大して専用命令ない
  - けども、ないわけじゃない
  - 命令ではなくメモリの確保の仕方で言うと、`string`は他のクラスと比べて相当特殊
      - この意味で言うと配列もかなり特殊
- `decimal`に至っては全く専用命令ない
  - `decimal`が特別なのはC#的にリテラルがあることくらい
  - IL 的にはリテラルすらない、完全に普通の構造体

ということで、何が何やら。まあ大まかに言うと、プリミティブ型 = 「中間言語(IL)的に専用を持っていて、かつ、値型」ですかね。

## IL的な扱い

せっかくなので、IL 的な扱いも見てみますか。
以下のようなコードをコンパイルしてみます。

<pre class="source" title="2つの引数の + 演算子呼び出しするだけの例">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        M(1, 2);
        M(<span class="string">"a"</span>, <span class="string">"b"</span>);
        M(1.23m, 2.71m);
    }

    <span class="reserved">static</span> <span class="reserved">int</span> M(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; x + y;
    <span class="reserved">static</span> <span class="reserved">decimal</span> M(<span class="reserved">decimal</span> x, <span class="reserved">decimal</span> y) =&gt; x + y;
    <span class="reserved">static</span> <span class="reserved">string</span> M(<span class="reserved">string</span> x, <span class="reserved">string</span> y) =&gt; x + y;
}
</code></pre>

### int

まずは`int`の場合。
メソッド`M(int, int)`の中身が

<pre class="source" title="int の +">
<code>  IL_0000:  ldarg.0
  IL_0001:  ldarg.1
  IL_0002:  add
</code></pre>

`M(int, int)`を呼び出す側が

<pre class="source" title="M(int, int) の呼び出し">
<code>  IL_0000:  ldc.i4.1
  IL_0001:  ldc.i4.2
  IL_0002:  call       <span class="reserved">int32</span> Program::M(<span class="reserved">int32</span>,
                                        <span class="reserved">int32</span>)
</code></pre>

という感じです。
足し算用に`add`という専用命令があったり、
定数読み込みのために`ldc.i4.1` (load constant 4バイト整数の1という意味)という命令があったりします。

### string

続いて`string`
メソッド`M(string, string)`の中身が

<pre class="source" title="string の +">
<code>  IL_0000:  ldarg.0
  IL_0001:  ldarg.1
  IL_0002:  call       <span class="reserved">string</span> [mscorlib]System.String::Concat(<span class="reserved">string</span>,
                                                              <span class="reserved">string</span>)
</code></pre>

`M(string, string)`を呼び出す側が

<pre class="source" title="M(string, string) の呼び出し">
<code>  IL_0008:  ldstr      <span class="string">"a"</span>
  IL_000d:  ldstr      <span class="string">"b"</span>
  IL_0012:  call       <span class="reserved">string</span> Program::M(<span class="reserved">string</span>,
                                         <span class="reserved">string</span>)
</code></pre>

です。
連結のためには特に命令を持っているわけではなく、`+`演算子は`Concat`メソッド呼び出しに置き換わります。
一方で、値の読み込みのために`ldstr` (load stringの意味)命令は持っています。

微妙なライン…
C#的には組み込み型(`string`っていうキーワードがあり、リテラルとかが用意されてる特別な型)だし、
IL的にはプリミティブ型ではない、という割には`ldstr`命令とか持ってる…

### decimal

最後に`decimal`。
メソッド`M(decimal, decimal)`の中身が

<pre class="source" title="decimal の +">
<code>  IL_0000:  ldarg.0
  IL_0001:  ldarg.1
  IL_0002:  call       <span class="reserved">valuetype</span> [mscorlib]System.Decimal [mscorlib]System.Decimal::op_Addition(<span class="reserved">valuetype</span> [mscorlib]System.Decimal,
                                                                                                <span class="reserved">valuetype</span> [mscorlib]System.Decimal)
</code></pre>

`M(decimal, decimal)`を呼び出す側が

<pre class="source" title="M(decimal, decimal)の呼び出し">
<code>  IL_0018:  ldc.i4.s   123
  IL_001a:  ldc.i4.0
  IL_001b:  ldc.i4.0
  IL_001c:  ldc.i4.0
  IL_001d:  ldc.i4.2
  IL_001e:  newobj     <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Decimal::<span class="reserved">.ctor</span>(<span class="reserved">int32</span>,
                                                                     <span class="reserved">int32</span>,
                                                                     <span class="reserved">int32</span>,
                                                                     <span class="reserved">bool</span>,
                                                                     uint8)
  IL_0023:  ldc.i4     0x10f
  IL_0028:  ldc.i4.0
  IL_0029:  ldc.i4.0
  IL_002a:  ldc.i4.0
  IL_002b:  ldc.i4.2
  IL_002c:  newobj     <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Decimal::<span class="reserved">.ctor</span>(<span class="reserved">int32</span>,
                                                                     <span class="reserved">int32</span>,
                                                                     <span class="reserved">int32</span>,
                                                                     <span class="reserved">bool</span>,
                                                                     uint8)
  IL_0031:  call       <span class="reserved">valuetype</span> [mscorlib]System.Decimal Program::M(<span class="reserved">valuetype</span> [mscorlib]System.Decimal,
                                                                     <span class="reserved">valuetype</span> [mscorlib]System.Decimal)
</code></pre>

です。
どこにも専用命令がないどころか、リテラルの`1.23m`や`2.71m`すらも、
`new decimal(123, 0, 0, false, 2)`の意味のコンストラクター呼び出しに置き換わっています。
加算も、`op_Addition`メソッド呼び出しです。

正直なところ、`decimal`がC#的に特別扱いを受ける理由はリテラルだけだったりします。
あくまでC#上の特別扱いであって、IL上は他の構造体の扱いとまったく同じです。
なので、`decimal`はプリミティブではない。

もしかすると、C++みたいに[ユーザー定義リテラル](http://cpprefjp.github.io/lang/cpp11/user_defined_literals.html)が書ければ、
`decimal`なんていう組み込み型は要らなかったかもしれません。
