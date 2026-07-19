---
title: "[雑記] オーバーロード解決"
source_url: "https://ufcpp.net/study/csharp/structured/miscoverloadresolution/"
content_type: "Article"
published_at: "2018-04-15T00:00:00"
updated_at: "2024-11-14T00:00:00"
tags: []
umbraco_id: 2147
parent_id: 1217
sort_order: 8
aliases:
  - "/csharp/structured/miscoverloadresolution/"
---

# \[雑記\] オーバーロード解決

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[関数](st_function.md#overload)で説明しましたが、
C# では[関数メンバー](st_function.md#function-member)に対して、
同名で引数リストだけが違う物を定義でき、これをオーバーロードと呼びます。

同名の関数がいくつかあるので、`M(0)` などと書いた時、実際には「どの`M`が呼ばれるか」という検索処理が必要になります。
このような同名の関数のうちどれを呼ぶか探す処理を<strong id="overload-resolution" class="keyword">オーバーロード解決</strong>(overload resolution)と呼びます。

本項では、C# がどういうルールでオーバーロード解決を行っているのかについて説明して行きます。

## <a id="sec-generated-title-2"></a> <a id="betterness-rule"></a>「より一致度の高いものを選ぶ」ルール

オーバーロード解決は、基本方針だけを一言でいうとシンプルで、
「より一致度の高いものを選ぶ」という方針になっています。
詳しくは後々説明して行くことになりますが、例えば以下のようなルールになっています。

- 型変換なしで引数に渡せるなら、それを優先的に呼ぶ
- 引数の数がピッタリ一致している方を優先的に呼ぶ

### <a id="sec-generated-title-3"></a> <a id="parameter-type"></a>引数の型

引数の型は、以下のリストの上の方ほど「一致度が高い」と判断されます。

- ぴったり一致する型
- [ジェネリック](../oop/sp2_generics.md)な型
- 親クラス
  - 多段に派生している場合、近い方ほど優先
- 暗黙的に変換できる型
  - その型が実装しているインターフェイス
  - [ユーザー定義の型変換](../functional/fun_whyextensions.md#cast)がある場合
- `object`

型変換なしで渡せるものほど「一致」、
いろんな型を受け付けるものほど「不一致」です。

例えば以下のようなメソッド `M` を書いた場合、
上の方に書いたものほど優先的に呼ばれます。

<pre class="source" title="引数の型の「一致度」の高さ">
<code><span class="reserved">using</span> System;

<span class="comment">// A → B → C の型階層</span>
<span class="comment">// IDisposable インターフェイスを実装</span>
<span class="comment">// C には int への暗黙的型変換あり</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IDisposable</span> { <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { } }
<span class="reserved">class</span> <span class="type">B</span> : A, <span class="type">IDisposable</span> { }
<span class="reserved">class</span> <span class="type">C</span> : B, <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="reserved">int</span>(C x) =&gt; 0;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// M のオーバーロードがいくつかある中、C を引数にして呼び出す</span>
        M(<span class="reserved">new</span> C());
    }

    <span class="comment">// 上から順に候補になる。</span>
    <span class="comment">// 上の方を消さないと、下の方が呼ばれることはない。</span>

    <span class="comment">// 「そのもの」が当然1番一致度高い</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">C</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"C"</span>);

    <span class="comment">// 次がジェネリックなやつ。型変換が要らないので一致度が高いという扱い。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"generic"</span>);

    <span class="comment">// 基底クラスは、階層が近い方が優先。この場合 B が先で、A が後</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">B</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"B"</span>);

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">A</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"A"</span>);

    <span class="comment">// 次に、インターフェイス、暗黙的型変換が同率。</span>
    <span class="comment">// (構造体の時の ValueType と違って、クラスは明確に基底クラスが上。)</span>
    <span class="comment">// この2つが同時に候補になってると ambiguous エラー</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">IDisposable</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"IDisposable"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int"</span>);

    <span class="comment">// 最後が object。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">object</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"object"</span>);
}
</code></pre>

型変換に関しては、候補が複数ある場合は、どちらを呼ぶべきか不明瞭なためコンパイル エラーになります。
例えば以下のコードはコンパイルできません。

<pre class="source" title="不明瞭でオーバーロード解決できない例">
<code><span class="reserved">using</span> System;

<span class="comment">// インターフェイス実装とユーザー定義の型変換を持つ</span>
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="reserved">int</span>(A x) =&gt; 0;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">IDisposable</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"IDisposable"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int"</span>);

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// インターフェイスへの変換と、ユーザー定義の型変換は同列</span>
        <span class="comment">// どちらを呼ぶべきか、このコードでは解決できない</span>
        <span class="error">M</span>(<span class="reserved">new</span> A());

        <span class="comment">// 明示的にキャストを書けば大丈夫</span>
        M((<span class="type">IDisposable</span>)<span class="reserved">new</span> A());
        M((<span class="reserved">int</span>)<span class="reserved">new</span> A());
    }
}
</code></pre>

型の派生に関してはクラスのみです。
C# では、任意の[値型](../resource/oo_reference.md#valtype)は `System.ValueType` クラスから派生、任意の[列挙型](st_enum.md)は`System.Enum`クラスから派生しているように振る舞いますが、
これらはあくまで「それっぽく振る舞うようにコンパイラーが特殊対応している」というだけで、
実際には型変換の一種です。
そのため、以下のようなコードはコンパイル エラーになります。

<pre class="source" title="ValueType への変換はインターフェイスへの変換と同列">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">S</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// S は ValueType から派生しているかのように振る舞うものの、これはあくまで ValueType への型変換になる</span>
        <span class="comment">// インターフェイスへの変換と同列なので、以下の呼び出しは不明瞭</span>
        M(<span class="reserved">new</span> S());
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">IDisposable</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"IDisposable"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">ValueType</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"ValueType"</span>);
}
</code></pre>

### <a id="sec-generated-title-4"></a> <a id="generic-method"></a>ジェネリック メソッド

C# では、「ジェネリックかどうか」だけの差があるメソッド オーバーロードも可能です。
この場合、非ジェネリックな方が優先的に呼ばれます。

<pre class="source" title="非ジェネリックな方優先">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// M(string) の方が呼ばれる</span>
        M(<span class="string">"abc"</span>);

        <span class="comment">// M&lt;T&gt;(string) の方が呼ばれる</span>
        M&lt;<span class="reserved">int</span>&gt;(<span class="string">"abc"</span>);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">string</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"M"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="reserved">string</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"M&lt;T&gt;"</span>);
}
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="optional"></a>オプション引数・可変長引数

C# には[オプション引数](sp4_optional.md#optional)と[可変長引数](sp_params.md)という、引数を省略できる仕組みが2つあります。
この場合、以下のリストの上の方ほど「一致度が高い」と判断されます。

- 省略なくぴったり引数の数が一致しているもの
- オプション引数による省略
- 可変長引数による省略

<pre class="source" title="引数の省略">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        M();
    }

    <span class="comment">// これが最優先</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"void"</span>);

    <span class="comment">// 次がこれ。既定値を与えたもの</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x = 0) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int x = 0"</span>);

    <span class="comment">// 最後がこれ。params</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">params</span> <span class="reserved">int</span>[] x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"params int[]"</span>);
}
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="instance"></a>インスタンス メソッド優先

C# には[拡張メソッド](../functional/sp3_extension.md)という、
インスタンス メソッドと同じ書き方で静的メソッドを呼べます。
正確にはオーバーロードとは言わないんですが、
インスタンス メソッドと同名の拡張メソッドも定義できるので、
オーバーロードと同種の「解決」が必要になります。

この場合、インスタンス メソッドの方が優先です。
拡張メソッドの方を呼びたければ、本来の静的メソッドとして呼ぶ必要があります。

<pre class="source" title="拡張メソッド">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> M() =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"instance"</span>);
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">this</span> A a) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"extension"</span>);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// instance の方が呼ばれる</span>
        <span class="reserved">new</span> A().M();

        <span class="comment">// A 自身が M を持っている以上、↑の書き方で拡張メソッドの方は呼べない</span>
        <span class="comment">// 以下のように、普通に静的メソッドとして呼ぶ必要がある</span>
        <span class="type">Extensions</span>.M(<span class="reserved">new</span> A());
    }
}
</code></pre>

## <a id="sec-generated-title-7"></a> <a id="inference"></a>型推論とオーバーロード解決

C# の構文にはいくつか、左辺値からの型推論をするものがあります。

- [ラムダ式](../functional/sp3_lambda.md)
  - どのデリゲート型かの決定
  - デリゲートと、[式ツリー](../functional/sp3_lambda.md#expression)
- [文字列補間](../start/st_string.md#string-interpolation)
- [`default` 式](../resource/rm_default.md#default-expr)

推論に推論を重ねることになるので、これらの型を引数にした場合、オーバーロード解決ができない場合が増えます。

<pre class="source" title="型推論が働かなくなる例">
<code><span class="reserved">using</span> System;

<span class="comment">// 引数が完全に一致しているデリゲート型を2個用意</span>
<span class="reserved">delegate</span> <span class="reserved">int</span> <span class="type">A</span>(<span class="reserved">int</span> x);
<span class="reserved">delegate</span> <span class="reserved">int</span> <span class="type">B</span>(<span class="reserved">int</span> x);

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 2個以上候補があるときに default は使えない</span>
        <span class="error">M</span>(<span class="reserved">default</span>);

        <span class="comment">// 型推論とはちょっと違うものの、null (型がない。どの型にでも代入可)でも同様</span>
        <span class="error">M</span>(<span class="reserved">null</span>);

        <span class="comment">// 型指定ありの default なら大丈夫</span>
        M(<span class="reserved">default</span>(<span class="type">A</span>));

        <span class="comment">// A なのか B なのか区別がつかない</span>
        <span class="error">M</span>(x =&gt; x);

        <span class="comment">// キャストがあれば大丈夫</span>
        <span class="comment">// new でも可</span>
        M((<span class="type">A</span>)(x =&gt; x));
        M(<span class="reserved">new</span> <span class="type">A</span>(x =&gt; x));
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">A</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"A"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">B</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"B"</span>);
}
</code></pre>

文字列補完では、`string`型で受け取る場合と`FormattableString`で受け取る場合で異なる挙動になりますが、
`var`を使った暗黙的変数宣言では自動的に`string`扱いされます。
そのため、オーバーロード解決でも特にキャストがない場合、`string`が優先されます。

<pre class="source" title="文字列補間">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> (a, b) = (1, 2);

        <span class="comment">// M(string) の方が呼ばれる</span>
        M(<span class="string">$"</span>{a}<span class="string">, </span>{b}<span class="string">"</span>);

        <span class="comment">// こう書けば M(FormattableString) の方</span>
        M((<span class="type">FormattableString</span>)<span class="string">$"</span>{a}<span class="string">, </span>{b}<span class="string">"</span>);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="reserved">string</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"string"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">FormattableString</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"FormattableString"</span>);
}
</code></pre>

同様に、ラムダ式は、デリゲート型で受け取る場合と式ツリーで受け取る場合で異なる挙動になります。
こちらは推論は効かず、オーバーロード解決もできなくなります。

<pre class="source" title="式ツリー">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq.Expressions;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        M(x =&gt; x);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Func"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Expression"</span>);
}
</code></pre>

ただし、次節で説明しますが、ラムダ式の型推論は結構優秀で、
ちゃんと推論が働きつつ、オーバーロード解決できる場合も多いです。

## <a id="sec-generated-title-8"></a> <a id="lambda"></a>ラムダ式

ラムダ式の型推論は相当優秀で、結構複雑なオーバーロード解決もできたりします。
例えば、以下の `M(x => x)` はちゃんとコンパイルできます。

<pre class="source" title="ラムダ式とオーバーロード解決">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// x の素通し = 引数と戻り値が一致 = Fucn&lt;int, int&gt; の方だけなのでそっちが選ばれる</span>
        <span class="comment">// x の型は int に</span>
        M(x =&gt; x);

        <span class="comment">// 明示的に double を返すと Func&lt;int, double&gt; の方が選ばれる</span>
        <span class="comment">// x の型は int に</span>
        M(x =&gt; (<span class="reserved">double</span>)x);

        <span class="comment">// この場合、引数と戻り値が一致してるという条件では int なのか string なのか区別できなくてエラー</span>
        <span class="error">N</span>(x =&gt; x);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int → int"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">double</span>&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int → double"</span>);

    <span class="reserved">static</span> <span class="reserved">void</span> N(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int → int"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> N(<span class="type">Func</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int → int"</span>);
}
</code></pre>

<h5 class="version version6">Ver. 6.0</h5>

ちなみに、ラムダ式がらみの型推論/オーバーロード解決は、C# 6.0 で少し改良がありました。
以下のように、多段のラムダ式でちゃんとオーバーロード解決できるようになったのは C# 6.0 からです。
また、「匿名メソッド式はラムダ式と違って式ツリーにならない」という条件が加味されたのも C# 6.0 からです。

<pre class="source" title="多段のラムダ式など">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq.Expressions;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// M(() =&gt; { }) だと Action か Expression&lt;Action&gt; か区別つかないものの</span>
        <span class="comment">// 匿名メソッド式の場合は式ツリー化できない仕様なので、M(Action) で確定</span>
        <span class="comment">// なのに以前はこれもエラーになってた(C# 6.0 からは M(Action) が呼ばれる)</span>
        M(<span class="reserved">delegate</span> () { });

        <span class="comment">// 以下のような、多段のラムダ式でちゃんとオーバーロード解決できるのは C# 6.0 から</span>
        <span class="comment">// Func&lt;int, Func&lt;int&gt;&gt; の方</span>
        M(() =&gt; () =&gt; 1);
        <span class="comment">// Func&lt;int, Func&lt;double&gt;&gt; の方</span>
        M(() =&gt; () =&gt; 1.0);
    }

    <span class="comment">// ラムダ式だと区別できないものの、匿名メソッド式なら Action で確定</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Action</span>x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Action"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Expression</span>&lt;<span class="type">Action</span>&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Expression"</span>);

    <span class="comment">// () =&gt; () =&gt; 1 みたいな、多段のラムダ式</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt;&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"() → () → int"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">double</span>&gt;&gt; x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"() → () → int"</span>);
}
</code></pre>


<!-- original-page-break -->


## <a id="sec-generated-title-9"></a> <a id="remove-redundant"></a>オーバーロード候補の絞り込み

<h5 class="version version7">Ver. 7.3</h5>

 C# 7.3で、オーバーロード解決の改善がありました。
以下の3つの改善があります。

- 静的メソッドかインスタンス メソッドかの違いで解決できるようになった
- ジェネリック型制約の違いで解決できるようになった
- [メソッド グループ](st_function.md#key-method-group)を引数にするとき、メソッドの戻り値を見るようになった

実のところ、これらの改善は「処理手順の順序変更」だそうです。
(今までも、これからも)オーバーロード解決に際して、C# コンパイラーは以下の2つの処理を行っていますが、
この順序を入れ替えることで上記のような区別がつくようになります。

1. 前述のような、引数の数や型の一致度を調べて最も一致するものを探す
1. 本当にそのメソッドを呼べるかどうかを調べる(上記の、静的/インスタンスの差や、型制約を調べる)

例えば、以下のコードを見てください。
同名の静的メソッドとインスタンス メソッドを1つずつ定義していますが、
間違った引数で呼び出しています。

<pre class="source" title="同名の静的メソッドとインスタンス メソッド">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Static</span> { }
<span class="reserved">struct</span> <span class="type">Instance</span> { }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 同名で、片方は静的メソッドで、もう片方はインスタンス メソッド。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Static</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Static"</span>);
    <span class="reserved">void</span> M(<span class="type">Instance</span> x) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Instance"</span>);

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 型名.M() で呼べるのは静的メソッドだけのはず。</span>
        <span class="comment">// でも、エラー メッセージとしては「M(Instance) を呼ぶにはインスタンスが必要」の類。</span>
        <span class="error"><span class="type">Program</span>.M</span>(<span class="reserved">new</span> <span class="type">Instance</span>());

        <span class="comment">// インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。</span>
        <span class="comment">// でも、エラー メッセージとしては「M(Static) を呼ぶにはインスタンス越しじゃダメ」の類。</span>
        <span class="error"><span class="reserved">new</span> <span class="type">Program</span>().M</span>(<span class="reserved">new</span> <span class="type">Static</span>());

        <span class="comment">// つまり、引数の型でのオーバーロード解決を先にやって、その後、静的/インスタンスの区別を調べてる。</span>
    }
}
</code></pre>

静的かインスタンスかの差をよりも先に、引数の型だけでオーバーロード解決しています。
なので、`Program.M(new Instance())`と呼ぼうが、`M(Instance x)`の方がまず選ばれます。
そして、「`M(Instance x)`はインスタンス メソッドなので、`型名.M`では呼べない」というエラーになります。

C# 7.3でこの順を逆にして、引数の型でオーバーロード解決をする前に、静的かインスタンスかなどの条件を先に見るようになりました。
呼べないことがわかるんだったら最初からオーバーロード解決候補から外して欲しいわけで、
ある意味当然の変更でしょう。

### <a id="sec-generated-title-10"></a> <a id="static-instance"></a>静的メソッドかインスタンス メソッドか

前節の例に、引数の既定値を足してみましょう。
2つのメソッド`M`が、どちらも`M()`で呼べるようになります。
C# 7.3からは、これらの呼び分けができるようになりました。

<pre class="source" title="静的メソッドかインスタンス メソッドかでオーバーロード解決">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Static</span> { }
<span class="reserved">struct</span> <span class="type">Instance</span> { }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 既定値が入っているのでどちらも M() で呼べる。</span>
    <span class="comment">// 片方は静的メソッドで、もう片方はインスタンス メソッド。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Static</span> x = <span class="reserved">default</span>) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Static"</span>);
    <span class="reserved">void</span> M(<span class="type">Instance</span> x = <span class="reserved">default</span>) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Instance"</span>);

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// 型名.M() で呼べるのは静的メソッドだけのはず。</span>
        <span class="comment">// でも、これまでは、M(Static) か M(Instance) かの区別がつかなかった。</span>
        <span class="comment">// C# 7.3 では M(Static) が選ばれるように。</span>
        <span class="type">Program</span>.M();

        <span class="comment">// インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。</span>
        <span class="comment">// 同上。</span>
        <span class="comment">// C# 7.3 では M(Instance) が選ばれるように。</span>
        <span class="reserved">new</span> <span class="type">Program</span>().M();

        <span class="comment">// Main が静的メソッドなので、何もつけない場合、この M() も静的な方が呼ばれる。</span>
        M();
    }

    <span class="reserved">void</span> InstanceMethod()
    {
        <span class="comment">// でも、これはダメ。</span>
        <span class="comment">// 静的な方もインスタンスの方も M() で呼べるので不明瞭。</span>
        <span class="error">M</span>();

        <span class="comment">// これなら OK。</span>
        <span class="comment">// this. が付いているのでインスタンス メソッドに絞られる。</span>
        <span class="reserved">this</span>.M();
    }
}
</code></pre>

#### <a id="sec-generated-title-11"></a> <a id="color-color"></a>余談: Color Color 問題

C# では、型名とプロパティ名が同じプロパティを作ることができます。
もっともありがちな例が「`Color`構造体型の`Color`プロパティ」なので、「Color Color問題」と呼ばれます。

C# 7.3での静的メソッドとインスタンス メソッドの呼び分けによって、
Color Color問題下においても呼び分けできるようになったものもあります。
しかし、C# 7.3でも解決できないものもあります。

例えば以下の例の通りです。
末尾の2つはC# 7.3でだけコンパイルできるコード、
真ん中の `Color.M()` はC# 7.3でもコンパイルできないコードになります。

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Color</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> R;
    <span class="reserved">public</span> <span class="reserved">byte</span> G;
    <span class="reserved">public</span> <span class="reserved">byte</span> B;

    <span class="comment">// どちらも M() で呼べるメソッド。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> M(<span class="reserved">int</span> x = 0) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"Instance"</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Color</span> c = <span class="reserved">default</span>) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"static"</span>);

    <span class="comment">// 参考までに、オーバーロードがない場合。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Instance() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Static() { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// C# では、型名とプロパティ名が同じプロパティを作れる。</span>
    <span class="reserved">static</span> Color Color { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// これは「プロパティのColor」(C# 7.2以前でも行ける)。</span>
        Color.Instance();

        <span class="comment">// これが「型のColor」(C# 7.2以前でも行ける)。</span>
        <span class="type">Color</span>.Static();

        <span class="comment">// これだと、この Color が型名かプロパティかが区別できない。</span>
        <span class="comment">// C# 7.3 でも不明瞭エラー。</span>
        Color.<span class="error">M</span>();

        <span class="comment">// C# 7.3 なら、以下の書き方で呼び分け可能(これまでは不明瞭エラー)。</span>
        <span class="comment">// 「プロパティのColor」。</span>
        <span class="type">Program</span>.Color.M();
        <span class="comment">// 「型のColor」。</span>
        <span class="reserved">global</span>::<span class="type">Color</span>.M();
    }
}
</code></pre>

### <a id="sec-generated-title-12"></a> <a id="constraints"></a>ジェネリック型制約

ジェネリック メソッドで、型制約だけが違うメソッドのオーバーロード解決ができるようにもなりました。

<pre class="source" title="型制約での呼び分け">
<code><span class="reserved">using</span> System;

<span class="comment">// オーバーロード用のダミー型</span>
<span class="reserved">struct</span> <span class="type">A</span> { }
<span class="reserved">struct</span> <span class="type">B</span> { }

<span class="comment">// IDisposable, IComparable な型を用意</span>
<span class="reserved">struct</span> <span class="type">Disposable</span> : <span class="type">IDisposable</span> { <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { } }
<span class="reserved">struct</span> <span class="type">Comparable</span> : <span class="type">IComparable</span> { <span class="reserved">public</span> <span class="reserved">int</span> CompareTo(<span class="reserved">object</span> x) =&gt; 0; }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// M(x) で呼べるメソッドが2つ。</span>
    <span class="comment">// 差は、T の型制約のみ。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">A</span> _ = <span class="reserved">default</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IDisposable</span> { }
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">B</span> _ = <span class="reserved">default</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IComparable</span> { }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// C# 7.3 からこの呼び出し方ができるように。</span>
        M(<span class="reserved">new</span> <span class="type">Disposable</span>());
        M(<span class="reserved">new</span> <span class="type">Comparable</span>());

        <span class="comment">// この書き方も C# 7.3 から。</span>
        M(<span class="reserved">new</span> <span class="type">Disposable</span>(), <span class="reserved">default</span>); <span class="comment">// default は default(A) に推論される</span>
        M(<span class="reserved">new</span> <span class="type">Comparable</span>(), <span class="reserved">default</span>); <span class="comment">// default は default(B) に推論される</span>

        <span class="comment">// C# 7.2 以前の場合、こう書くのが必須。</span>
        M(<span class="reserved">new</span> <span class="type">Disposable</span>(), <span class="reserved">default</span>(<span class="type">A</span>));
        M(<span class="reserved">new</span> <span class="type">Comparable</span>(), <span class="reserved">default</span>(<span class="type">B</span>));
    }
}
</code></pre>

特に、参照型(class)か値型(struct)かによるオーバーロード解決は便利そうです。
例えば、「条件を満たさなければnullを返す」みたいなメソッドを書きたい場合、
値型の時だけ[null許容型](../resource/sp2_nullable.md)にして、`?`を付ける必要があります。
この呼び分けが、これまでだとなかなか難しかったですが、C# 7.3ではできるようになります。

<pre class="source" title="class 制約と struct 制約の呼び分け">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">ClassExtensions</span>
{
    <span class="comment">// クラスの場合は LINQ の FirstOrDefault そのまま。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
        =&gt; source.FirstOrDefault();
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">StructExtensions</span>
{
    <span class="comment">// 構造体の場合は null 許容型に変える必要がある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span>? FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>
        =&gt; source.Select(x =&gt; (<span class="type">T</span>?)x).FirstOrDefault();
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ClassExtensions の方のが呼ばれる。</span>
        <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"b"</span>, <span class="string">"c"</span> }.FirstOrNull();

        <span class="comment">// StructExtensions の方のが呼ばれる。</span>
        <span class="reserved">new</span>[] { 1, 2, 3 }.FirstOrNull();
    }
}
</code></pre>

### <a id="sec-generated-title-13"></a> <a id="method-return"></a>メソッドの戻り値

C# (というか、.NET)のメソッドは、戻り値の型を[シグネチャ](st_function.md#key-signature)に含みません。
基本的に、戻り値だけが違うメソッドは定義できませんし、呼び分けもできません。

ただ、これまでの例でもたびたび出てきたように、引数の規定値を与えることで戻り値だけが違う「っぽく見える」メソッド オーバーロードはできます。
また、以下のように、「戻り値違いのデリゲートを受け取るメソッド」は作れます。

<pre class="source" title="戻り値違いのデリゲートを受け取るメソッド オーバーロード">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int"</span>);
<span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">string</span>&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"string"</span>);
</code></pre>

[前述の通り](#lambda)、
ラムダ式であれば、ラムダ式の型推論が賢くて、この2つのメソッドの呼び分けができました。

<pre class="source" title="ラムダ式は賢い">
<code>M(() =&gt; 0); <span class="comment">// int の方</span>
M(() =&gt; <span class="string">"abc"</span>); <span class="comment">// string の方</span>
</code></pre>

しかし、メソッド グループを引数に渡した場合、これまではオーバーロード解決できませんでした。
それが、以下のように、C# 7.3からはオーバーロード解決できるようになります。

<pre class="source" title="メソッドの戻り値でオーバーロード解決">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"int"</span>);
    <span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">Func</span>&lt;<span class="reserved">string</span>&gt; f) =&gt; <span class="type">Console</span>.WriteLine(<span class="string">"string"</span>);

    <span class="reserved">static</span> <span class="reserved">int</span> IntReturn() =&gt; 0;
    <span class="reserved">static</span> <span class="reserved">string</span> StringReturn() =&gt; <span class="string">""</span>;

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ラムダ式賢い。</span>
        M(() =&gt; 0); <span class="comment">// int の方</span>
        M(() =&gt; <span class="string">"abc"</span>); <span class="comment">// string の方</span>

        <span class="comment">// こういう書き方なら C# 7.2 まででもできた。</span>
        M(() =&gt; IntReturn());
        M(() =&gt; StringReturn());

        <span class="comment">// なのに、以下のような書き方はこれまでできなかった。</span>
        <span class="comment">// C# 7.3 からできるように。</span>
        M(IntReturn);
        M(StringReturn);
    }
}
</code></pre>

### <a id="sec-generated-title-14"></a> <a id="signature-trick"></a>余談: 同一シグネチャのメソッド オーバーロード

ここで説明してきたように、C# 7.3から静的メソッドかインスタンス メソッドかや、
ジェネリック型制約の差でオーバーロード解決できるようになりました。

呼び分けできるようになったんなら、そもそもオーバーロードもできていいはずではあります。
しかし、静的/インスタンス違いや型制約違いでオーバーロードを作れないのは、
C# ではなく、.NET 型システムの制約です。
単に C# コンパイラーだけの仕事ではないので、これを修正するのは少し難しいです。
そのため、これは引き続き認められていません。

<pre class="source" title="制約違いのオーバーロードは不可">
<code><span class="comment">// 以下の2つは呼び分けできるようになった。</span>
<span class="comment">// なのに、定義はできない(C# コンパイラーだけの問題じゃないので直せない)。</span>
<span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span> { }
<span class="reserved">static</span> <span class="reserved">void</span> <span class="error">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span> { }
</code></pre>

ただし、これまで挙げてきた例で少し出てきていますが、
「ごまかす」方法がいくつかあります。

1つは[オプション引数](sp4_optional.md#optional)(引数の規定値)や[可変長引数](sp_params.md)を使う方法で、以下のような書き方で「違うオーバーロードなんだけど、実質的には同じ呼び方ができる」と言うようなメソッドを定義できます。

<pre class="source" title="オプション引数をダミーにして疑似的に同シグネチャ オーバーロードを実現">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 呼び分け用のダミー型</span>
    <span class="reserved">struct</span> <span class="type">Struct</span> { }
    <span class="reserved">struct</span> <span class="type">Class</span> { }

    <span class="comment">// ダミー引数を足すことでオーバーロードする。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">Struct</span> _ = <span class="reserved">default</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span> { }
    <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x, <span class="type">Class</span> _ = <span class="reserved">default</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span> { }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        M(1);     <span class="comment">// M(T, Struct) が呼ばれる</span>
        M(<span class="string">"abc"</span>); <span class="comment">// M(T, Class) が呼ばれる</span>
    }
}
</code></pre>

もう1つは拡張メソッドを使う方法です。
拡張メソッドであれば、別のクラス中で定義してやれば、同じ型を対象とした全く同じシグネチャのメソッドを定義できます。

<pre class="source" title="拡張メソッドで同シグネチャ オーバーロードを実現">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">ClassExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
        =&gt; source.FirstOrDefault();
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">StructExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span>? FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>
        =&gt; source.Select(x =&gt; (<span class="type">T</span>?)x).FirstOrDefault();
}
</code></pre>

また、`ref`の有無が違うだけの拡張メソッドでもオーバーロード可能です。

<pre class="source" title="ref の有無でオーバーロード">
<code><span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Extensions</span>
{
    <span class="comment">// ref の有無の差 + 型制約</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="reserved">ref</span> <span class="type">T</span> x) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span> { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">T</span> x) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span> { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="string">"abc"</span>.M();

        <span class="reserved">var</span> x = 123;
        x.M();
        <span class="comment">// ただ、ref 拡張メソッドの性質上、123.M() とは呼べない(リテラルがダメ)</span>
        <span class="comment">// また、DateTime.Now.M() とかもダメ(プロパティ越しがダメ)</span>
    }
}
</code></pre>

いずれも疑似的なもので、ダミーなしのオーバーロードと比べると利便性は下がりますが、
C# 7.3で呼び分けができるようになったことで、少し使い勝手はよくなりました。


<!-- original-page-break -->


## <a id="sec-generated-title-15"></a> <a id="overload-resolution-priority">OverloadResolutionPriority 属性</a>

C# 13 で、オーバーロードの解決優先度を属性を付けて明示できる機能が入りました。
`OverloadResolutionPriority` 属性(`System.Runtime.CompilerServices` 名前空間)を使います。
名前通り優先度を指定できて、正の整数を指定すると優先度が上がって、負の整数なら下がります。

<pre class="source" title="オーバーロード解決の優先度を変更する例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// IEnumerable&lt;char&gt; の方が選ばれる。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M1</span></span>(<span class="string">&quot;&quot;</span>);
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M2</span></span>(<span class="string">&quot;&quot;</span>);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 通常、インターフェイスよりも具体的な型の方が優先。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M1</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) { }

    <span class="comment">// 属性を付けて優先度を上げる。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }

    <span class="comment">// 属性を付けて優先度を下げる。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M2</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

ちなみに、オーバーロードできないメンバーにこの属性を付けるとコンパイル エラーになります。

<pre class="source" title="オーバーロードできないメンバーに OverloadResolutionPriority を付けるとコンパイラーに怒られる">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">namespace</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices
{
    <span class="comment">// .NET 標準ライブラリ中の OverloadResolutionPriorityAttribute には</span>
    <span class="comment">// AttributeTargets.Method | Constructor | Property がついてる。</span>
    <span class="comment">// ここではあえてターゲットの制限を外した同名・同名前空間の型を定義。</span>
    <span class="reserved">public</span> <span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">OverloadResolutionPriorityAttribute</span>(<span class="reserved">int</span> <span class="variable local">priority</span>) : <span class="type">Attribute</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Priority</span> <span class="operator">=&gt;</span> <span class="variable local">priority</span>;
    }
}

<span class="reserved">class</span> <span class="type">C</span>
{
    [<span class="error" title="CS9262"><span class="type">OverloadResolutionPriority</span>(<span class="number">0</span>)</span>]
    <span class="reserved">static</span> <span class="static"><span class="type">C</span></span>() { }

    [<span class="error" title="CS9262"><span class="type">OverloadResolutionPriority</span>(<span class="number">0</span>)</span>]
    <span class="operator">~</span><span class="type">C</span>() { }

    [<span class="error" title="CS9262"><span class="type">OverloadResolutionPriority</span>(<span class="number">0</span>)</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; }

    [<span class="error" title="CS9262"><span class="type">OverloadResolutionPriority</span>(<span class="number">0</span>)</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="reserved">int</span>(<span class="type">C</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">default</span><span class="operator">!</span>;
}
</pre>


### <a id="sec-generated-title-16"></a> <a id="binary-compat">互換性問題</a>

C# の言語機能が増えるにつれて、例えば「`IEnumerable<T>` よりも、`ReadOnlySpan<T>` 引数を使いたい」みたいなことが多々あります。
しかし、以前からあるメソッドを消すことができなくて、それは残したまま新しいオーバーロードを追加することになったりします。
(ライブラリ作者、特に、プラグイン提供するような場合、バイナリ互換(ソースコードの再コンパイルなしでも動く保証)を残すため、メソッドの削除はできなくなります。)
ところが、互換性のために消すに消せない方のメソッドが、優先度が高すぎて困ったり、
オーバーロード解決できなくなって困るということが起こるようになってきました。

`IEnumerable<T>` と `ReadOnlySpan<T>` の場合、C# 13 時点ではオーバーロード解決できなくなって困ります。
(この2者の問題であれば、C# 14 で `Span<T>`/`ReadOnlySpan<T>` の特別扱いが入って問題解消する予定です。)

<pre class="source" title="">
<span class="comment">// C# 13 時点だと IEnumerable と ReadOnlySpan を選べなくてコンパイル エラーになる。</span>
<span class="type">C</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121"><span class="static">M</span></span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">1</span>]);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }

    <span class="comment">// ReadOnlySpan は C# 7.2 / .NET Core 2.1 / 2017年頃に入った。</span>
    <span class="comment">// パフォーマンス的に有利なので IEnumerable を置き換えたいことがある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

他に、デフォルト引数が絡んだ場合に困ったりします。
具体的には、`Debug.Assert` や文字列がらみで困っているみたいです。

`Debug.Assert` は、C# 10 で導入された [`CallerArgumentExpression`](../cheatsheet/ap_ver10.md#CallerArgumentExpression) を使いたいものの、既存のオーバーロードに阻害されて呼びようがないという問題が出ています。

<pre class="source" title="CallerArgumentExpression 付きのオーバーロードを呼べない問題">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="static"><span class="type">Console</span><span class="operator">.</span><span class="static"><span class="method">ReadLine</span></span>()</span>);

<span class="comment">// Debug.Assert(x &gt; 0, &quot;x &gt; 0&quot;) になってほしいのに、1引数の方が呼ばれちゃう。</span>
<span class="type">Debug</span><span class="operator">.</span><span class="static"><span class="method">Assert</span></span>(<span class="variable">x</span> <span class="operator">&gt;</span> <span class="number">0</span>);

<span class="comment">// System.Diagnostics.Debug からの抜粋</span>
<span class="reserved">class</span> <span class="type">Debug</span>
{
    <span class="comment">// 元々 bool 1引数のオーバーロードがある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Assert</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>) { }

    <span class="comment">// C# 10 で導入された CallerArgumentExpression を使いたい。</span>
    <span class="comment">// けど、 Assert(condition) では1引数オーバーロードの方が優先されて、CallerArgumentExpression が役に立たない。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Assert</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>, [<span class="type">CallerArgumentExpression</span>(<span class="reserved">nameof</span>(<span class="variable local">condition</span>))] <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">message</span> <span class="operator">=</span> <span class="reserved">null</span>) { }
}
</pre>

文字列がらみは、
.NET の負の遺産として有名なカルチャー依存問題(参考: [遅い](../../../blog/2023/3/string-order/index.md)、[環境依存](../../../blog/2020/11/net5_0ga/index.md))への対処として、`IndexOf` などのメソッドにデフォルト引数 `StringComparison comparisonType = StringComparison.Ordinal` を付けて、無指定の時の挙動を `Ordinal` に変えたいという話があります。
しかしこれも、1引数オーバーロードの方が優先度が高くてうまく働きません。

<pre class="source" title="">
<span class="comment">// IndexOf(value, StringComparison.Ordinal) で呼ばれてほしいけど、</span>
<span class="comment">// 残念ながら IndexOf(value) にしかならない。</span>
<span class="type"><span class="static">String</span></span><span class="operator">.</span><span class="method"><span class="static">IndexOf</span></span>(<span class="string">&quot;àèò&quot;</span>, <span class="string">&quot;a&quot;</span>);

<span class="comment">// 本来は string クラスのインスタンスメソッド。デモ用に静的メソッド。</span>
<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">String</span></span>
{
    <span class="comment">// 1引数オーバーロードがいるので…</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">IndexOf</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">string</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="variable local">s</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="variable local">value</span>);

    <span class="comment">// デフォルト引数を付けたところで IndexOf(string value) の方が優先される。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">IndexOf</span></span>(
        <span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">string</span> <span class="variable local">value</span>,
        <span class="type">StringComparison</span> <span class="variable local">comparisonType</span> <span class="operator">=</span> <span class="type">StringComparison</span><span class="operator">.</span>Ordinal) <span class="comment">// Ordinal をデフォルトに変えたい。</span>
        <span class="operator">=&gt;</span> <span class="variable local">s</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="variable local">value</span>, <span class="variable local">comparisonType</span>);
}
</pre>

これらの問題に `OverloadResolutionPriority` 属性が使えます。

<pre class="source" title="IEnumerable の優先度を下げる">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">1</span>]); <span class="comment">// 無事、ReadOnlySpan の方が選ばれる。</span>

<span class="reserved">class</span> <span class="type">C</span>
{
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">_</span>) { }
}
</pre>

<pre class="source" title="1引数オーバーロードの優先度を下げる">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="static"><span class="type">Console</span><span class="operator">.</span><span class="method"><span class="static">ReadLine</span></span>()</span>);

<span class="comment">// 無事、 Debug.Assert(x &gt; 0, &quot;x &gt; 0&quot;) で呼ばれる。</span>
<span class="type">Debug</span><span class="operator">.</span><span class="method"><span class="static">Assert</span></span>(<span class="variable">x</span> <span class="operator">&gt;</span> <span class="number">0</span>);

<span class="reserved">class</span> <span class="type">Debug</span>
{
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Assert</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Assert</span></span>(<span class="reserved">bool</span> <span class="variable local">condition</span>, [<span class="type">CallerArgumentExpression</span>(<span class="reserved">nameof</span>(<span class="variable local">condition</span>))] <span class="reserved">string</span><span class="operator">?</span> <span class="variable local">message</span> <span class="operator">=</span> <span class="reserved">null</span>) { }
}
</pre>

<pre class="source" title="1引数オーバーロードの優先度を下げる">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// 無事、IndexOf(value, StringComparison.Ordinal) で呼ばれる。</span>
<span class="static"><span class="type">String</span></span><span class="operator">.</span><span class="static"><span class="method">IndexOf</span></span>(<span class="string">&quot;àèò&quot;</span>, <span class="string">&quot;a&quot;</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">String</span></span>
{
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">IndexOf</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">string</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="variable local">s</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="variable local">value</span>);

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">IndexOf</span></span>(
        <span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">string</span> <span class="variable local">value</span>,
        <span class="type">StringComparison</span> <span class="variable local">comparisonType</span> <span class="operator">=</span> <span class="type">StringComparison</span><span class="operator">.</span>Ordinal) <span class="comment">// Ordinal をデフォルトに変えたい。</span>
        <span class="operator">=&gt;</span> <span class="variable local">s</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="variable local">value</span>, <span class="variable local">comparisonType</span>);
}
</pre>

ちなみに、`OverloadResolutionPriority` で優先度を下げたメソッドを呼び出すのはかなり困難になったりします。
場合によっては真っ当な方法で呼ぶ手段がなく、リフレクションや unsafe な手段でしか呼べなくなります。

<pre class="source" title="優先度を下げたせいで真っ当な手段では呼べず &amp; 真っ当じゃない手段で呼ぶ例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// OverloadResolutionPriority(-1) のせいで、真っ当な方法ではどうやっても M(string) の方を呼べない。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>((<span class="reserved">string</span>)<span class="string">&quot;&quot;</span>);

<span class="comment">// リフレクションとか Unsafe な手段を使えば一応呼べなくはない。</span>
[<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>StaticMethod, <span class="property">Name</span> <span class="operator">=</span> <span class="reserved">nameof</span>(<span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">M</span></span>))]
<span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">C</span><span class="operator">?</span> <span class="variable local">c</span>, <span class="reserved">string</span> <span class="variable local">_</span>);
<span class="static"><span class="method">M</span></span>(<span class="reserved">default</span>, <span class="string">&quot;&quot;</span>);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">object</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;object&quot;</span>);

    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;string&quot;</span>);
}
</pre>

### <a id="sec-generated-title-17"></a> <a id="in-a-type">同一クラス内でのみ有効</a>

`OverloadResolutionPriority` 属性による優先度の変更は、同一クラス内においてのみ有効です。
なので、以下のようなことは<em>できません</em>。

* 拡張メソッドでインスタンス メソッドを乗っ取り
* 自作の拡張メソッドで他人の拡張メソッドを乗っ取り
* 派生クラス内のオーバーロードで基底クラスのメソッドを乗っ取り

例えば以下のような所業はできません。

<pre class="source" title="Linq 乗っ取りを画策">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// わざと System.Linq.Enumerable と競合するようにして、</span>
<span class="reserved">namespace</span> System<span class="operator">.</span>Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">FakeLinq</span></span>
{
    <span class="comment">// 優先度を最大限引き上げ。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="reserved">int</span><span class="operator">.</span><span class="static"><span class="constant">MaxValue</span></span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="type param">TResult</span>&gt; <span class="method"><span class="static">Select</span></span>&lt;<span class="type param">TSource</span>, <span class="type param">TResult</span>&gt;(
        <span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type param">TSource</span>&gt; <span class="variable local">source</span>, <span class="type">Func</span>&lt;<span class="type param">TSource</span>, <span class="type param">TResult</span>&gt; <span class="variable local">selector</span>)
        <span class="operator">=&gt;</span> <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>(<span class="string">&quot;Select は乗っ取った&quot;</span>);
}
</pre>

<pre class="source" title="ただし、実際にやってみるとうまくいかない(当然)">
<span class="comment">// FakeLinq の方が優先されたりはしない。</span>
<span class="comment">// 単に「Enumerable と FakeLinq 間で不明瞭」エラーに。</span>
<span class="string">&quot;abc&quot;</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121">Select</span></span>(<span class="variable local">c</span> <span class="operator">=&gt;</span> (<span class="reserved">int</span>)<span class="variable local">c</span>);
</pre>

また、`OverloadResolutionPriority` を付けることで逆にオーバーロード解決できなくなるようなこともありえます。

例えば、以下のように複数のクラスで複数の拡張メソッドが定義されていて、
全体でみれば1つだけ優先度が高くてオーバーロード解決できる場合を考えます。

<pre class="source" title="複数のクラスの複数の拡張メソッドから1つ選ばれる例">
<span class="comment">// A.M(string), A.M(string, int), B.M(string, int) が同列で比較されて、</span>
<span class="comment">// デフォルト引数なしの A.M(string) が勝つ。</span>
<span class="string">&quot;&quot;</span><span class="operator">.</span><span class="method">M</span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">A</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">A.M(</span>{<span class="variable local">s</span>}<span class="string">)</span><span class="string">&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">int</span> <span class="variable local">i</span> <span class="operator">=</span> <span class="number">0</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">A.M(</span>{<span class="variable local">s</span>}<span class="string">, </span>{<span class="variable local">i</span>}<span class="string">)</span><span class="string">&quot;</span>);
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">B</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">int</span> <span class="variable local">i</span> <span class="operator">=</span> <span class="number">0</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">B.M(</span>{<span class="variable local">s</span>}<span class="string">, </span>{<span class="variable local">i</span>}<span class="string">)</span><span class="string">&quot;</span>);
}
</pre>

ここで、`A.M` のうちの1つに `OverloadResolutionPriority` を付けて優先度を変えてみます。
`OverloadResolutionPriority` は1つのクラス内でしか働かないので、`A` の中のどの `M` が選ばれるかにだけ影響します。
その結果、以下のように別のクラスの `M` と競合する可能性があります。

<pre class="source" title="OverloadResolutionPriority を付けたことで他のクラスのメンバーと競合するようになる例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// OverloadResolutionPriority を付けたことで、A.M の中では A.M(string, int) が選ばれる。</span>
<span class="comment">// B.M は元々 B.M(string, int) しかない。</span>
<span class="comment">// A.M(string, int) と B.M(string, int) が競合してオーバーロード解決できなくなる。</span>
<span class="string">&quot;&quot;</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121">M</span></span>();

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">A</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">A.M(</span>{<span class="variable local">s</span>}<span class="string">)</span><span class="string">&quot;</span>);

    [<span class="type">OverloadResolutionPriority</span>(<span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">int</span> <span class="variable local">i</span> <span class="operator">=</span> <span class="number">0</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">A.M(</span>{<span class="variable local">s</span>}<span class="string">, </span>{<span class="variable local">i</span>}<span class="string">)</span><span class="string">&quot;</span>);
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">B</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">string</span> <span class="variable local">s</span>, <span class="reserved">int</span> <span class="variable local">i</span> <span class="operator">=</span> <span class="number">0</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">$&quot;</span><span class="string">B.M(</span>{<span class="variable local">s</span>}<span class="string">, </span>{<span class="variable local">i</span>}<span class="string">)</span><span class="string">&quot;</span>);
}
</pre>

### <a id="sec-generated-title-18"></a> <a id="overload-by-return">余談: (疑似)戻り値オーバーロード</a>

C# では戻り値だけが異なるオーバーロードを認めていません。
例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="戻り値だけが違うオーバーロードの追加はできない">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method"><span class="static">MAsync</span></span>() { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); }

    <span class="comment">// Task を ValueTask に変更したいとして、互換性のために Task MAsync() を残すと…</span>
    <span class="comment">// 戻り値だけが違うオーバーロードは認められない。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="error" title="CS0111"><span class="method"><span class="static">MAsync</span></span></span>() { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); }
}
</pre>

ちょっと気持ち悪い回避策になりますが、デフォルト引数を悪用することでオーバーロードもどきを作れたりはします。
ところが、「引数なし」と「デフォルト引数持ち」なら前者の方が優先されるため、
追加した新しいオーバーロードもどきが呼ばれることはありません。

<pre class="source" title="オーバーロードもどき(おしい)">
<span class="comment">// 残念ながら Task MAsync() の方しか呼ばれない。</span>
<span class="reserved">await</span> <span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">MAsync</span></span>();

<span class="comment">// もちろんこうすれば ValueTask の方が呼ばれるものの、不格好すぎる。</span>
<span class="reserved">await</span> <span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">MAsync</span></span>(<span class="reserved">default</span>);

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="static"><span class="method">MAsync</span></span>() { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="static"><span class="method">Yield</span></span>(); }

    <span class="comment">// オーバーロードもどきとして、適当に使わないデフォルト値付きの引数を追加。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="static"><span class="method">MAsync</span></span>(<span class="reserved">int</span> <span class="variable local">_</span> <span class="operator">=</span> <span class="number">0</span>) { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); }
}
</pre>

これも一応、`OverloadResolutionPriority` 属性で解消できます。

<pre class="source" title="OverloadResolutionPriority でごり押し">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="comment">// ValueTask 戻り値の方が呼ばれるように。</span>
<span class="reserved">await</span> <span class="type">C</span><span class="operator">.</span><span class="method"><span class="static">MAsync</span></span>();

<span class="reserved">class</span> <span class="type">C</span>
{
    [<span class="type">OverloadResolutionPriority</span>(<span class="operator">-</span><span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method"><span class="static">MAsync</span></span>() { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type struct">ValueTask</span> <span class="static"><span class="method">MAsync</span></span>(<span class="reserved">int</span> <span class="variable local">_</span> <span class="operator">=</span> <span class="number">0</span>) { <span class="reserved">await</span> <span class="type">Task</span><span class="operator">.</span><span class="method"><span class="static">Yield</span></span>(); }
}
</pre>
