---
title: "bool 型の false, true, それ以外"
source_url: "https://ufcpp.net/blog/2019/1/falsetrueother/"
content_type: "BlogEntry"
published_at: "2019-01-28T14:50:18"
updated_at: "2019-01-28T15:02:17"
tags: []
umbraco_id: 2219
parent_id: 2216
sort_order: 2
aliases: []
---

# bool 型の false, true, それ以外

これまで(C# 7.3 まで)、C# の `switch` ステートメントで `bool` 型を使う場合、以下のように、`default` 句が必須になることが多々ありました。

<pre class="source" title="true, false, default...">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">X</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="control">switch</span> (<span class="variable">b</span>)
    {
        <span class="control">case</span> <span class="reserved">false</span>: <span class="control">return</span> 0;
        <span class="control">case</span> <span class="reserved">true</span>: <span class="control">return</span> 1;
        <span class="control">default</span>: <span class="control">return</span> -1;
    }
}
</code></pre>

`bool` 型には `false` と `true` しかないはずなのにこれはおかしいと言われ続けていたんですが、C# 8.0 では `default` 句が要らなくなるというか、`default` 句を絶対に通らなくなるよう、コード生成の仕方を変更するみたいです。

今日はこの辺りの、要するに「`false` でも `true` でもない `bool` 値」の話。

サンプルコード: [BoolExhaustiveness](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2019/BoolExhaustiveness)

## bool とは

### ドキュメント上

まず、ドキュメント上、`bool` がどうなっているかというと…

- [C# Language Reference での `bool` の説明](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/bool)
  - `System.Boolean` 型のエイリアスで、真偽値、すなわち、`true` か `false` を格納できる
- [`Boolean` 構造体の説明](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)
  - `true` か `false` のいずれかの2値を取れる型

大体は2つの値だけを取れる型として説明されています。

### 実装上: Boolean 構造体

その `Boolean` 構造体(`System` 名前空間)の内部実装がどうなっているかというと、

- 1バイトの構造体
- `true` の内部表現は 1
- `false` の内部表現は 0

です。
1バイトだけども0と1しか必要としないので、残り254個の値は基本的には使われません。

## 0 でも 1 でもない bool を作る

普通にリテラルの `true`, `false` や、`==` などの条件式から `bool` 値を得る限り、本当に0と1以外の値は発生しません。

ただ、C# は unsafe な手段を使って任意に値を書き換えれちゃうので、無理やりやると 0 でも 1 でもない `bool` 値を作れます。

具体的にはいくつか書き方があるんですが、1つ目は素直にポインターを使うもの。

<pre class="source" title="ポインターを使って変な bool を作る">
<code><span class="reserved">unsafe</span> <span class="reserved">bool</span> toBool(<span class="reserved">byte</span> b) =&gt; *((<span class="reserved">bool</span>*)&amp;b);
Console.WriteLine(toBool(2));
</code></pre>

もう1つは、[`Unsafe` クラス](../../../2018/12/unsafe/index.md)を使う書き方。
これもまあ、書き方が違うだけでポインターと大差ないです。

<pre class="source" title="Unsafe クラスを使って変な bool を作る">
<code><span class="reserved">bool</span> toBool(<span class="reserved">byte</span> b) =&gt; Unsafe.As&lt;<span class="reserved">byte</span>, <span class="reserved">bool</span>&gt;(<span class="reserved">ref</span> b);
Console.WriteLine(toBool(2));
</code></pre>

最後に、`StructLayout` を使う(C 言語の union 風な使い方する)方法。
`LayoutKind.Explicit` は、ポインター並みに変なことができちゃう機能なので、
そもそも unsafe コードなしで使えること自体が疑問視されていたりもします。
要するに、実質 unsafe。

<pre class="source" title="LayoutKind.Explicit を使って変な bool を作る">
<code><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="reserved">bool</span> toBool(<span class="reserved">byte</span> b)
    {
        Union u = <span class="reserved">default</span>;
        u.Byte = b;
        <span class="reserved">return</span> u.Boolean;
    }

    Console.WriteLine(toBool(2));
}

[StructLayout(LayoutKind.Explicit)]
<span class="reserved">private</span> <span class="reserved">struct</span> <span class="type">Union</span>
{
    [FieldOffset(0)]
    <span class="reserved">public</span> <span class="reserved">byte</span> Byte;
    [FieldOffset(0)]
    <span class="reserved">public</span> <span class="reserved">bool</span> Boolean;
}
</code></pre>

## 0 でも 1 でもない bool を使うとどうなるか

x86 などの CPU では、条件分岐命令が以下のような方法で実現されています。

- 直前の命令の結果が 0 になったら立つフラグが CPU 内に存在する
- そのフラグを見て分岐する

要するに、「0 かどうか」しか見ません。
この意味では、「true とは 0 以外の全ての値を指す」と言えます。

### C# の if ステートメント

.NET の中間言語もそういう挙動をします。
[brtrue 命令](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.brtrue)ってのを持ってるんですが、
こいつは「value が 0 でなければ分岐」という挙動。

C# の `if` ステートメントはこの命令(もしくはその逆の [brfalse](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.brfalse))に変換されるので、
「0 以外の値」は全て `true` 扱いになります。
実際、前述の方法で作った「中身が2の`bool`値」を `if` に渡すと `true` 側に分岐します。

<pre class="source" title="中身が2のboolは、if 中では true 扱い">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Pointer</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">unsafe</span> <span class="reserved">bool</span> toBool(<span class="reserved">byte</span> b) =&gt; *((<span class="reserved">bool</span>*)&amp;b);

        Branch(<span class="reserved">false</span>);     <span class="comment">// if (false)</span>
        Branch(<span class="reserved">true</span>);      <span class="comment">// if (true)</span>
        Branch(toBool(2)); <span class="comment">// if (true)</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Branch(<span class="reserved">bool</span> b)
    {
        <span class="reserved">if</span> (b) Console.WriteLine(<span class="string">"if (true)"</span>);
        <span class="reserved">else</span> Console.WriteLine(<span class="string">"if (false)"</span>);
    }
}
</code></pre>

<pre class="console" title="中身が2のboolは、if 中では true 扱い">
<code>if (false)
if (true)
if (true)
</code></pre>

### C# 7.3 までの switch ステートメント

問題はここからなんですが…

`if` ステートメントとは違って、(C# 7.3 までの) `switch` ステートメントは中身の値を見ます。
すなわち、普通の `true` と、「中身が2の`bool`値」は別の値という扱い。

これが、冒頭のコードで `default` 句が必須になる理由です。
[実際、`case true` を通らないようなコード](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2019/BoolExhaustiveness/BoolOtherThan01/Program.cs)を書けます。

<pre class="source" title="case false も case true も通らない bool 値">
<code><span class="reserved">static</span> <span class="reserved">void</span> Main()
{
    <span class="comment">// 0 → false</span>
    <span class="comment">// 1 → true</span>
    <span class="comment">// それ以外 → if (b) は通るんだけど、switch (b) { case true: } は通らない(C# 7.3 までは)変な値になる。</span>
    <span class="reserved">for</span> (<span class="reserved">byte</span> i = 0; i &lt; 3; i++)
    {
        Console.WriteLine(<span class="string">$"value = </span>{i}<span class="string">"</span>);
        Branch(Pointer(i));
        Branch(UnsafeAs(i));
        Branch(UnionStruct(i));
    }
}

<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> false (0) の時は何も表示されない。</span>
<span class="inactive">///</span><span class="comment"> true (1) の時は if(b) switch(b) の両方が表示される。</span>
<span class="inactive">///</span><span class="comment"> 「それ以外の値」を作って渡すと、if(b) だけが表示される。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">static</span> <span class="reserved">void</span> Branch(<span class="reserved">bool</span> b)
{
    <span class="reserved">if</span> (b) Console.WriteLine(<span class="string">"    if(b)"</span>);
    <span class="reserved">switch</span> (b) { <span class="reserved">case</span> <span class="reserved">true</span>: Console.WriteLine(<span class="string">"    switch(b)"</span>); <span class="reserved">break</span>; }
}
</code></pre>

### 型 switch

ちなみにこの「中身の値を見て分岐」挙動は、`case` が全部定数の場合(= 古き良き昔からある `switch`) の場合だけの挙動です。

C# 7.0 から入った、[パターン マッチングを使った `switch`](../../../../study/csharp/datatype/typeswitch.md#switch)(いやゆる「型 switch」)の場合には brtrue 命令が使われるようになって、[`if` ステートメントと同じ挙動になります](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2019/BoolExhaustiveness/BoolOtherThan01/TypeSwitch.cs)。

<pre class="source" title="型 switch は brtrue と同じ挙動">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">TypeSwitch</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        Branch(0);
        Branch(1);
        Branch(2);
    }

    <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Branch(<span class="reserved">byte</span> x)
    {
        <span class="reserved">var</span> b = *((<span class="reserved">bool</span>*)&amp;x);

        Console.WriteLine(<span class="string">$"value = </span>{x}<span class="string">"</span>);
        Console.Write(<span class="string">"    traditional switch: "</span>);
        <span class="reserved">switch</span> (b)
        {
            <span class="reserved">case</span> <span class="reserved">false</span>:
                Console.WriteLine(<span class="string">"false"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="reserved">true</span>:
                Console.WriteLine(<span class="string">"true"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">default</span>:
                <span class="comment">// 0でも1でもないbool値の時にここに来る</span>
                Console.WriteLine(<span class="string">"other"</span>);
                <span class="reserved">break</span>;
        }

        Console.Write(<span class="string">"    type switch: "</span>);
        <span class="reserved">switch</span> (b)
        {
            <span class="reserved">case</span> <span class="reserved">false</span> <em><span class="reserved">when</span> <span class="reserved">true</span></em>:
                Console.WriteLine(<span class="string">"false"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="reserved">true</span>:
                Console.WriteLine(<span class="string">"true"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">default</span>:
                <span class="comment">// 絶対ここは通らない</span>
                Console.WriteLine(<span class="string">"other"</span>);
                <span class="reserved">break</span>;
        }
    }
}
</code></pre>

<pre class="console" title="型 switch は brtrue と同じ挙動">
<code>value = 0
    traditional switch: false
    type switch: false
value = 1
    traditional switch: true
    type switch: true
value = 2
    traditional switch: <em>other</em>
    type switch: true
</code></pre>


### マーシャリング

ちなみに、[P/Invoke](../../../../study/csharp/interop/sp_pinvoke.md#pinvoke)を使う際には、マーシャリング時に「0でも1でもない`bool`値」を`true`(内部的に1の`bool`値)に置き換える処理が掛かるみたいです。

例えば、以下のような Rust コードを lib.dll 中で定義しておいて、

<pre class="source" title="8ビット整数値を素通しする Rust 関数">
<code>#[no_mangle]
<span class="reserved">pub extern fn</span> <span class="method">id</span>(<span class="variable">x</span>: <span class="type">i8</span>) -> <span class="type">i8</span> { <span class="variable">x</span> }
</code></pre>

これを C# 側から以下のように呼び出します。

<pre class="source" title="id 関数を C# から呼び出し">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="comment">// 素通し。当然、2。</span>
        <span class="reserved">byte</span> a = Id(2);
        Console.WriteLine(a);

        <span class="comment">// 素通しじゃなくて、bool で値を受け取り。true。</span>
        <span class="reserved">bool</span> b = ToBool(2);
        Console.WriteLine(b);

        <span class="reserved">unsafe</span>
        {
            <span class="comment">// 内部表現を見てみると、1 になってる。</span>
            <span class="reserved">byte</span> b1 = *(<span class="reserved">byte</span>*)&amp;b;
            Console.WriteLine(b1);
        }
    }

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> rust 側の id 関数は i8 を素通しするだけ。</span>
    <span class="inactive">///</span><span class="comment"> それを DllImport で呼んでるので、このメソッドも素通し。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    [DllImport(<span class="string">"lib.dll"</span>, EntryPoint = <span class="string">"id"</span>)]
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">byte</span> Id(<span class="reserved">byte</span> x);

    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
    <span class="inactive">///</span><span class="comment"> マーシャリングで、byte な戻り値を bool で受け取ることができる。</span>
    <span class="inactive">///</span><span class="comment"> ただ、この場合、素通しではなくて、ちゃんと 戻り値 != 0 で bool に変換されているみたい。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
    [DllImport(<span class="string">"lib.dll"</span>, EntryPoint = <span class="string">"id"</span>)]
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">bool</span> ToBool(<span class="reserved">byte</span> x);
}
</code></pre>

`id`関数の戻り値は `i8` (C# でいう `sbyte`)ですが、マーシャリング時に `bool` への変換をしてくれます。
変換の仕方は、`!= 0` になっているみたいで、「0 でない値」だったら普通の `true` (内部的に1の`bool`値)が返ってきます。

### C# 8.0 での switch ステートメントの変更

まあ、要するに、`switch` ステートメントだけがきもいです。

たびたび「`case false` と `case true` があれば `default` 要らないだろ」と言われ続け、
そのたびに「内部的に `false` でも `true` でもない値があり得るから」という回答が返って来続けていたんですが。

この度、「ドキュメント上も 『`true` と `false` の2値』と明記されているんだから、それ以外の値を想定して非効率なコードを生成するのはおかしいだろ」という突っ込みがあって、「それは確かに」的な空気になったみたいです。

また、C# 8.0 では [`switch` 式](../../../2018/12/cs8switchexpr/index.md)も入るので、網羅性のチェック(「`true` と `false` で全パターン網羅している」という判定)をしたい需要が高まったので、ついに折れて、`bool` に対する `switch` の挙動を変えることにしたみたいです。

<pre class="source" title="bool に対する switch の仕様変更">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        Console.WriteLine(X(<span class="reserved">false</span>)); <span class="comment">// -1</span>
        Console.WriteLine(X(<span class="reserved">true</span>)); <span class="comment">// 1</span>

        <span class="reserved">unsafe</span>
        {
            <span class="reserved">byte</span> x = 2;
            <span class="reserved">bool</span> y = *(<span class="reserved">bool</span>*)&amp;x;
            Console.WriteLine(X(y)); <span class="comment">// C# 7.0 までは 0 だった。C# 8.0 で 1 になるように。</span>
        }
    }

    <span class="reserved">static</span> <span class="reserved">int</span> X(<span class="reserved">bool</span> b)
    {
        <span class="reserved">switch</span> (b)
        {
            <span class="reserved">case</span> <span class="reserved">false</span>: <span class="reserved">return</span> -1;
            <span class="reserved">case</span> <span class="reserved">true</span>: <span class="reserved">return</span> 1;
            <span class="reserved">default</span>: <span class="reserved">return</span> 0;     <span class="comment">// C# 7.0 までは何も言われなかった。C# 8.0 で「到達できないコード」警告出るように。</span>
        }
    }
}
</code></pre>

内部的には `if` 相当のコードへの置き換えです。

ちなみに、Visual Studio 2019 Preview 2だと、「[LangVersion を 7.3 以下にしてても新しい方の挙動になってしまう](https://github.com/dotnet/roslyn/issues/32806)」というバグがあったりします。
バグ認定はされていて、正式版までには「C# 8.0 以上にした場合だけ新しい挙動になる」に変更されるはずです。
