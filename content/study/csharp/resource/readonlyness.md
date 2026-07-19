---
title: "readonly の注意点"
source_url: "https://ufcpp.net/study/csharp/resource/readonlyness/"
content_type: "Article"
published_at: "2017-11-04T00:00:00"
updated_at: "2023-04-01T20:34:20"
tags: []
umbraco_id: 2095
parent_id: 1286
sort_order: 3
aliases:
  - "/csharp/resource/readonlyness/"
---

# readonly の注意点

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[定数](../start/sp_const.md#readonly)」で、読み取り専用のフィールドが作れるという話をしました。
この時点ではまだ[クラス](../oop/oo_class.md)や[構造体](rm_struct.md)、[値型と参照型の違い](oo_reference.md)などについて触れていなかったので`readonly`修飾子の簡単な紹介だけに留めましたが、
本項で改めて`readonly`について説明します。

整数などの基本的な型に対して使う分には特に問題は起きないんですが、構造体やクラスなど、複合型に対して使うときには注意が必要です。

##<a id="sec-generated-title-2"></a> <a id="class-readonly"></a>参照型のフィールドに対して readonly
`readonly`に関して最も注意が必要な点は、`readonly`は再帰的には働かないという点です。
`readonly`を付けたその場所だけが読み取り専用になり、参照先などについては書き換えが可能です。

例えば以下のコードを見てください。`Program`クラスのフィールド`c`には`readonly`が付いていますが、
`c`が普通に書き換え可能なクラスのフィールドなので、クラスの中身は自由に書き換えられます。

<pre class="source" title="参照型のフィールドに対してreadonlyを付ける例">
<code><span class="comment">// 書き換え可能なクラス</span>
<span class="reserved">class</span> <span class="type">MutableClass</span>
{
    <span class="comment">// フィールドを直接公開</span>
    <span class="reserved">public</span> <span class="reserved">int</span> X;

    <span class="comment">// 書き換え可能なプロパティ</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// フィールドの値を書き換えるメソッド</span>
    <span class="reserved">public</span> <span class="reserved">void</span> M(<span class="reserved">int</span> value) =&gt; X = value;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">MutableClass</span> c = <span class="reserved">new</span> <span class="type">MutableClass</span>();

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// これは許されない。c は readonly なので、c 自体の書き換えはできない</span>
        <span class="error">c</span> = <span class="reserved">new</span> <span class="type">MutableClass</span>();

        <span class="comment">// けども、c の中身までは保証してない</span>
        <span class="comment">// 書き換え放題</span>
        c.X = 1;
        c.Y = 2;
        c.M(3);
    }
}
</code></pre>

![参照型のフィールドに対してreadonlyを付ける例](../../../../assets/media/1145/mutableclass.png)

クラスを書き換えできないように作る場合、クラス自体を書き換え不能に作りましょう。
(クラスの方で、フィールドを`readonly`にしたり、プロパティを[get-only](../oop/oo_property.md#get-only)にします。)

##<a id="sec-generated-title-3"></a> <a id="struct-readonly"></a>値型のフィールドに対して readonly
クラス(参照型)とは対照的に、構造体(値型)の場合はデータを直接持ちます。
そのため、構造体のフィールドに対して`readonly`を付けると、構造体の中身も読み取り専用になります。
ただし、メソッドの呼び出しなどを行う際、コピーが発生するという別の注意が必要です。

例えば以下のように、`readonly`が付いたフィールド`c`自体に加えて、`c`のフィールドも書き換えできません。

<pre class="source" title="値型のフィールドに対してreadonlyを付ける例">
<code><span class="reserved">using</span> System;

<span class="comment">// 書き換え可能な構造体</span>
<span class="reserved">struct</span> <span class="type">MutableStruct</span>
{
    <span class="comment">// フィールドを直接公開</span>
    <span class="reserved">public</span> <span class="reserved">int</span> X;

    <span class="comment">// フィールドの値を書き換えるメソッド</span>
    <span class="reserved">public</span> <span class="reserved">void</span> M(<span class="reserved">int</span> value) =&gt; X = value;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">readonly</span>  <span class="type">MutableStruct</span> c = <span class="reserved">new</span>  <span class="type">MutableStruct</span>();

    <span class="reserved">static</span> <span class="reserved">void</span> Main() =&gt; Allowed();

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> NotAllowed()
    {
        <span class="comment">// これはもちろん許されない。c は readonly なので、c 自体の書き換えはできない</span>
        <span class="error">c</span> = <span class="reserved">new</span>  <span class="type">MutableStruct</span>();

        <span class="comment">// 構造体の場合、フィールドに関しては readonly な性質を引き継ぐ</span>
        <span class="error">c.X</span> = 1;
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> Allowed()
    {
        <span class="comment">// でも、メソッドは呼べてしまう</span>
        c.M(3); <span class="comment">// X を 3 で上書きしているはず？</span>

        <span class="type">Console</span>.WriteLine(c.X); <span class="comment">// でも、X は 0 のまま</span>

        <span class="comment">//↑のコードは、実はコピーが発生している</span>
        <span class="comment">// 以下のコードと同じ意味になる</span>

        <span class="reserved">var</span> local = c;
        local.M(3);

        <span class="type">Console</span>.WriteLine(c.X); <span class="comment">// 書き換わってるのは local (コピー)の方なので、c は書き換わらない(0)</span>

        <span class="type">Console</span>.WriteLine(local.X); <span class="comment">// もちろんこっちは書き換わってる(3)</span>
    }
}
</code></pre>

![値型のフィールドに対してreadonlyを付ける例](../../../../assets/media/1146/mutablestruct.png)

この例の後半を見ての通り、メソッドは呼べてしまいます。
フィールド`X`は書き換えれないはずなのに、その`X`を書き換えているメソッド`M`を呼んでもエラーになりません。
C# では、こういう場合に、`readonly`であることを保証しつつメソッドを呼び出せるように、フィールドを一度コピーしてから、そのコピーに対してメソッドを呼ぶということをしています。

このコピーは、万が一に備えて防衛的にコピー(defensive copy)するものです。
実際にコピーが必要かどうか(実際にメソッド内で書き換えをしているかどうか)に関わらず、常にコピーが発生します。
ソースコード上は目に見えないコピーなので、<strong id="hidden-copy" class="keyword">隠れたコピー</strong>(hidden copy)と呼ばれたりもします。

すなわち、コピーが発生してまずいような場合(例えば構造体のサイズが大きくてコピーにコストが掛かるとか)には、`readonly`なフィールドを使うことで問題が発生することがあります。
この問題は、[`in`引数](sp_ref.md#in)などでも発生しまえます。
後述する[`readonly struct`](#readonly-struct)や[readonly 関数メンバー](#readonly-member)を使えばこの問題は少し緩和するので、そちらも参照してください。

##<a id="sec-generated-title-4"></a> <a id="this-rewrite"></a>構造体の this 書き換え
C# の`readonly`フィールドには少し片手落ちなところがあって、実は、構造体の場合にちょっとした問題を起こせたりします。

構造体のメソッドの中では`this`が「自分自身の参照」の意味なんですが、この`this`参照は書き換えできてしまいます。
そのため、以下のように、`readonly`で一見書き換えができなさそうなフィールドを書き換えてしまうことができます。

<pre class="source" title="構造体の this 書き換えの例">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="comment">// フィールドに readonly を付けているものの…</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Y;

    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// this の書き換えができてしまうので、実は X, Y の書き換えが可能</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Set(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
    {
        <span class="comment">// X = x; Y = y; とは書けない</span>
        <span class="comment">// でも、this 自体は書き換えられる</span>
        <span class="reserved">this</span> = <span class="reserved">new</span> <span class="type">Point</span>(x, y);
    }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);

        <span class="comment">// p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる</span>

        <span class="comment">// でも、このメソッドは呼べるし、X, Y が書き換わる</span>
        p.Set(3, 4);

        <span class="type">Console</span>.WriteLine(p.X); <span class="comment">// 3</span>
        <span class="type">Console</span>.WriteLine(p.Y); <span class="comment">// 4</span>
    }
}
</code></pre>

わざわざこんな紛らわしいことをしようとは思わないのでめったに問題になることはないんですが、一応は注意が必要です。
また、この問題は、次節で説明する通り、C# 7.2で少し緩和されます。

##<a id="sec-generated-title-5"></a> <a id="readonly-struct"></a>readonly struct
<h5 class="version version7">Ver. 7.2</h5>

C# 7.2で、構造体自体に`readonly`修飾を付けられるようになりました。
`readonly`を付けた構造体は以下のような状態になります。

- 全てのフィールドに対して `readonly` を付けなければならなくなる
  - [get-onlyプロパティ](../oop/oo_property.md#get-only)は使えます(自動生成されるフィールドが`readonly`なので問題ない)
- `this`参照も`readonly`扱いされる

`this`が`readonly`扱いになるので、前節のような`this`書き換えの問題は起きません。

<pre class="source" title="readonly struct の例">
<code><span class="reserved">using</span> System;

<span class="comment">// 構造体自体に readonly を付ける</span>
<span class="reserved"><em>readonly</em></span> <span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="comment">// フィールドには readonly が必須</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Y;

    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);

    <span class="comment">// readonly を付けない場合と違って、以下のような this 書き換えも不可</span>
    <span class="comment">//public void Set(int x, int y) =&gt; this = new <span class="type">Point</span>(x, y);</span>
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);

        <span class="comment">// p.X = 0; とは書けない。これはちゃんとコンパイル エラーになる</span>
        <span class="comment">// p.Set(3, 4); みたいなのもダメ</span>

        <span class="type">Console</span>.WriteLine(p.X); <span class="comment">// 1 しかありえない</span>
        <span class="type">Console</span>.WriteLine(p.Y); <span class="comment">// 2 しかありえない</span>
    }
}
</code></pre>

###<a id="sec-generated-title-6"></a> <a id="avoid-copy"></a>readonly struct によるコピー回避
[前述](#struct-readonly)の通り、(無印の)構造体の`readonly`フィールドに対してメソッドを呼ぶと防衛的コピーが発生するという問題があります。
これに対して、`readonly struct`であれば、このコピーを回避できます。

例えば以下のように、ほぼ同じ構造・どちらも書き換え不能な構造体を作ったとして、`readonly struct`になっているかどうかでコピー発生の有無が変わります。

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="comment">// 作りとしては readonly を意図しているので、何も書き換えしない</span>
<span class="comment">// でも、struct 自体には readonly が付いていない</span>
<span class="reserved">struct</span> <span class="type">NoReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">void</span> M() { }
}

<span class="comment">// <span class="type">NoReadOnly</span> と作りは同じ</span>
<span class="comment">// ちゃんと readonly struct</span>
<span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">ReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">void</span> M() { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">NoReadOnly</span> nro;
    <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">ReadOnly</span> ro;

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// readonly を付けなかった場合</span>
        <span class="comment">// フィールド参照(読み取り)は問題ない</span>
        <span class="type">Console</span>.WriteLine(nro.X);

        <span class="comment">// メソッド呼び出しが問題。ここでコピー発生</span>
        <span class="comment">// (呼び出し側では、「M の中で特に何も書き換えていない」というのを知るすべがないので、防衛的にコピーが発生)</span>
        nro.M();

        <span class="comment">// readonly を付けた場合</span>
        <span class="comment">// これなら、M をそのまま呼んでも何も書き換わらない保証があるので、コピーは起きない</span>
        ro.M();
    }

    <span class="comment">// これも問題あり(コピー発生)</span>
    <span class="comment">// in を付けたので readonly 扱い → M を呼ぶ際にコピー発生</span>
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">in</span> <span class="type">NoReadOnly</span> x) =&gt; x.M();

    <span class="comment">// こちらも、readonly struct であれば問題なし(コピー回避)</span>
    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">in</span> <span class="type">ReadOnly</span> x) =&gt; x.M();
}
</code></pre>

C# 7.2 以降では、書き換えを意図していない構造体に対しては`readonly`修飾を付けるのが無難でしょう。

また、「フィールド直接参照なら大丈夫だけど、メソッドを(プロパティも)呼ぶとコピー発生」という性質上、
書き換えを最初から意図している構造体の場合は、プロパティよりも、フィールドを直接`public`にしてしまう方が都合がいいことがあります。

##<a id="sec-generated-title-7"></a> <a id="ref-readonly"></a>readonly参照と不変性
[`in`引数](sp_ref.md#in)や[`ref readonly`](sp_ref.md#ref-readonly)で、読み取り専用の参照を作れます。
この読み取り専用参照は、「そのメソッド内で書き換えない」、「その引数・変数を通した書き換えをしない」という意思表明としては非常に有用です。
その一方で、「外で書き換わる」、「参照元の値が書き換わる」という意味で、不変性(immutability)の保証はありません。

例えば以下の例を見てください。

<pre class="source" title="in/ref readonly で保証できる範囲">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        _value = 0;
        ByVal(_value); <span class="comment">// 0, 0</span>

        _value = 0;
        ByRef(_value); <span class="comment">// 0, 1</span>
    }

    <span class="comment">// 書き換えできるフィールド</span>
    <span class="reserved">static</span> <span class="reserved">int</span> _value;

    <span class="comment">// 値渡し = コピー なので、 _value 書き換えの影響は受けない</span>
    <span class="reserved">static</span> <span class="reserved">void</span> ByVal(<span class="reserved">int</span> value)
    {
        <span class="type">Console</span>.WriteLine(value);
        _value++;
        <span class="type">Console</span>.WriteLine(value);
    }

    <span class="comment">// 参照渡しなので、 _value 書き換えの影響を受ける</span>
    <span class="comment">// in (ref readonly) であっても、immutable ではない</span>
    <span class="comment">// value を通して書き換えない保証があるだけで、別経路で書き換わることに対しては無力</span>
    <span class="reserved">static</span> <span class="reserved">void</span> ByRef(<span class="reserved">in</span> <span class="reserved">int</span> value)
    {
        <span class="type">Console</span>.WriteLine(value);
        _value++;
        <span class="type">Console</span>.WriteLine(value);
    }
}
</code></pre>

メソッドの中身としては全く同じメソッドが2つありますが、片方(`ByVal`)は値渡しで、もう片方(`ByRef`)は `in` 引数で整数値を受け取っています。
`ByVal`では、`value`は値のコピーを受け取っているので、元の値の出どころとは無縁になっています。
一方、`ByRef`の方では`value`自身は`in`が付いていて書き換えられませんが、その参照元になっている`_value` の方が書き換わると、`value`の値も一緒に変化します。
書き換え不能(readonly)だからと言って、値の不変性(immutable)の保証はなく、こうして値が変化する場合があります。

##<a id="sec-generated-title-8"></a> <a id="readonly-member"></a>readonly 関数メンバー
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で、[関数メンバー](../structured/st_function.md#sec-function-member)単位で「フィールドを書き換えてない」ということを保証できるようになりました。
構造体全体を `readonly struct` にしなくても、[隠れたコピー](#hidden-copy)問題を避けられる機会が増えます。

以下のように、関数メンバーに `readonly` 修飾を付けます。

<pre class="source" title="readonly 関数メンバーの例">
<code><span class="comment">// 構造体自体は readonly にしない。</span>
<span class="comment">// フィールドは書き換えたい</span>
<span class="reserved">struct</span> <span class="type">NonReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">float</span> X;
    <span class="reserved">public</span> <span class="reserved">float</span> Y;
 
    <span class="comment">// でも、このプロパティ内ではフィールドを書き換えない</span>
    <span class="reserved">public</span> <span class="reserved">float</span> LengthSquared =&gt; X * X + Y * Y;
}
 
<span class="comment">// NonReadOnly との差は LengthSquared の readonly の有無だけ</span>
<span class="reserved">struct</span> <span class="type">ReadOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">float</span> X;
    <span class="reserved">public</span> <span class="reserved">float</span> Y;
 
    <span class="comment">// readonly 修飾でフィールドを書き換えないことを明示</span>
    <span class="reserved">public</span> <span class="reserved"><em>readonly</em></span> <span class="reserved">float</span> LengthSquared =&gt; X * X + Y * Y;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// こっちは、LengthSquared 内での X, Y の書き換えを恐れて隠れたコピーが発生する。</span>
    <span class="reserved">static</span> <span class="reserved">float</span> <span class="method">M</span>(<span class="reserved">in</span> <span class="type">NonReadOnly</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>.LengthSquared;
 
    <span class="comment">// こっちは、LengthSquared に readonly が付いているのでコピー発生しない。</span>
    <span class="reserved">static</span> <span class="reserved">float</span> <span class="method">M</span>(<span class="reserved">in</span> <span class="type">ReadOnly</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span>.LengthSquared;
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>(<span class="reserved">string</span>[] <span class="variable">args</span>)
    {
        <span class="method">M</span>(<span class="reserved">new</span> <span class="type">NonReadOnly</span> { X = 1, Y = 2 });
        <span class="method">M</span>(<span class="reserved">new</span> <span class="type">ReadOnly</span> { X = 1, Y = 2 });
    }
}
</code></pre>

隠れたコピー問題はソースコードの見た目に現れず、気づきにくい問題なので、
関数内でフィールドを書き換えていないなら積極的に `readonly` 修飾を付けておくべきでしょう。

ちなみに、逆に、`readonly` 関数メンバー内から、`readonly` ではないものを触ろうとしても隠れたコピーが発生します。
例えば以下のコードでは、`A`のフィールドを書き換える`Increment`メソッドを、
`readonly` なメソッドとそうでないメソッドから呼び出してみています。

<pre class="source" title="readonly 関数メンバーから、非 readonly な構造体フィールドに触る">
<code><span class="reserved">using</span> System;
 
<span class="reserved">struct</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Increment</span>() =&gt; Value++;
}
 
<span class="reserved">struct</span> <span class="type">B</span>
{
    <span class="reserved">public</span> <span class="type">A</span> A;
 
    <span class="comment">// A の非 readonly メンバーを呼ぶ。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Mutable</span>() =&gt; A.<span class="method">Increment</span>();
 
    <span class="comment">// Mutable との差は readonly 修飾が付いてるだけ。</span>
    <span class="comment">// this が書き換わらないように、A のコピーが作られる。A 自体には変化が起きない。</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">void</span> <span class="method">Immutable</span>() =&gt; A.<span class="method">Increment</span>();
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">b</span> = <span class="reserved">new</span> <span class="type">B</span>();
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">b</span>.A.Value); <span class="comment">// 初期状態: 0</span>
 
        <span class="variable">b</span>.<span class="method">Mutable</span>();
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">b</span>.A.Value); <span class="comment">// 意図通りの書き換え: 1</span>
 
        <span class="variable">b</span>.<span class="method">Immutable</span>();
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">b</span>.A.Value); <span class="comment">// 書き換わらない: 1 (Immutable の中で A のコピーが発生)</span>
    }
}
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="similar-but-different"></a>注意: 似て非なるもの(ref readonly)
この `readonly` 関数メンバーは、構文上、[`ref readonly`](sp_ref.md#ref-readonly)と似ているのでちょっと注意が必要かもしれません。

<pre class="source" title="readonly ref との兼ね合い">
<code><span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span>[] _value;
 
    <span class="comment">// これは、読み取り専用参照を返すという意味。</span>
    <span class="comment">// _value 配列の中身が書き換わってもらっては困る。</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> X =&gt; <span class="reserved">ref</span> _value[0];
 
    <span class="comment">// これは、S 内のフィールド(この場合 _value) を書き換えないという意味。</span>
    <span class="comment">// _value 配列の中身が書き換わろうと知ったことではない。</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">int</span> Y =&gt; <span class="reserved">ref</span> _value[0];
 
    <span class="comment">// これは、上記2つの両方の意味。</span>
    <span class="comment">// _value 自体も書き換わらないし、_value の中身を書き換えてもらっても困るとき用。</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> Z =&gt; <span class="reserved">ref</span> _value[0];
}
</code></pre>

ちなみに、プロパティの場合は `get`/`set` それぞれ別に `readonly` 指定ができます。
当然ですが、ほとんどの場合は「`get` だけが `readonly`」になると思われます。

<pre class="source" title="プロパティの get にだけ readonly 修飾">
<code><span class="reserved">struct</span> <span class="type">X</span>
{
    <span class="reserved">int</span> _value;
 
    <span class="reserved">public</span> <span class="reserved">int</span> Value
    {
        <span class="reserved">readonly</span> <span class="reserved">get</span> =&gt; _value;
        <span class="reserved">set</span> =&gt; _value = <span class="reserved">value</span>;
    }
}
</code></pre>
