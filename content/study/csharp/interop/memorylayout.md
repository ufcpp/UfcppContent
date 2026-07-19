---
title: "複合型のレイアウト"
source_url: "https://ufcpp.net/study/csharp/interop/memorylayout/"
content_type: "Article"
published_at: "2016-06-14T00:00:00"
updated_at: "2025-02-15T15:59:00"
tags: []
umbraco_id: 1915
parent_id: 1321
sort_order: 4
aliases:
  - "/csharp/interop/memorylayout/"
---

# 複合型のレイアウト

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
複合型(クラスや構造体)では、フィールドをメモリ上にどうレイアウト(layout: 配置)するかという問題があります。

通常、メモリ上のレイアウトがどうなっているのかをプログラマーが気にする必要はありません。
大体はコンパイラーが最適な仕事をしてくれます。

それでも、時々、レイアウト方法を明示的に指定したい場合があります
(おそらく、そのほとんどはC++などで書かれたネイティブ コードとの相互運用です)。
そこで、プログラミング言語によってはレイアウトをカスタマイズするための機能を提供しているものもあります。

C#では、クラスと構造体に対してレイアウトのカスタマイズ機能を提供しています。
`StructLayout`属性を付けることでカスタマイズ可能です。

##<a id="sec-generated-title-2"></a> <a id="alignment"></a>アラインメント
「最適なレイアウト」について説明するためには、まず、メモリの<strong id="key-alignment" class="keyword">アラインメント</strong>(alignment: 整列、調整)について知る必要があります。

一般に、メモリの読み書きは、[アドレス](../../computer/general/memory.md#address)が4の倍数や8の倍数になっている方が高速です。
(1命令で読み出せるのは倍数ピッタリの場所だけなCPUもあります。
最悪、2命令で隣り合った場所を読み込んで、繋ぎなおすような処理が必要になります。)

そこで、多くのプログラミング言語で、アドレスがきれいな倍数になるように、フィールドとフィールドの間に隙間を空けたり、フィールドを並べ替えたりしています。この、所定の倍数の位置にフィールドを並べる処理をアラインメントと呼びます。

例えば、以下のような構造体を書いたとします。A, C (`byte`型)が1バイト、B (`long`型)が8バイトのデータです。

<pre class="source" title="アラインメント説明用のサンプル構造体">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}
</code></pre>

アラインメントの間隔はコンパイラーやCPUによって変わりますが、一例として、この構造体のメモリ レイアウトは以下のようになります。

![Sample構造体のレイアウト](../../../../assets/media/1082/sequentiallayout.png)

フィールドすべてが8の倍数アドレスに並ぶように、8バイト間隔でフィールドが並びます。
また、末尾にも、全体が8の倍数になるように未使用領域が追加されます。

##<a id="sec-generated-title-3"></a> <a id="inspection"></a>C#でレイアウトを調べてみる
C#でも、[unsafe](sp_unsafe.md#unsafe)コードを使えば、構造体のレイアウトを調べることができます。
以下のように、ポインターを使って、構造体の先頭と、各フィールドのアドレスの差を見れば、レイアウトがわかります。

<pre class="source" title="ポインターを使ってレイアウトを調べるコード">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = <span class="reserved">default</span>(<span class="type">Sample</span>);
        <span class="reserved">var</span> p = &amp;a;
        <span class="reserved">var</span> pa = &amp;a.A;
        <span class="reserved">var</span> pb = &amp;a.B;
        <span class="reserved">var</span> pc = &amp;a.C;

        <span class="type">Console</span>.WriteLine(<span class="string">$@"サイズ: </span>{<span class="reserved">sizeof</span>(<span class="type">Sample</span>)}
<span class="string">A: </span>{(<span class="reserved">long</span>)pa - (<span class="reserved">long</span>)p}
<span class="string">B: </span>{(<span class="reserved">long</span>)pb - (<span class="reserved">long</span>)p}
<span class="string">C: </span>{(<span class="reserved">long</span>)pc - (<span class="reserved">long</span>)p}
<span class="string">"</span>);
    }
}
</code></pre>

ただし、1つ注意があります。C#では、たとえunsafeコード内であっても、参照型のアドレスは取れないようになっています。
そのため、参照型や、参照型を含んだ構造体の場合はレイアウトを調べられません。

<pre class="source" title="参照型のアドレスは取れないので、レイアウトも調べられない">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> I;
    <span class="reserved">public</span> <span class="reserved">string</span> S;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = <span class="reserved">default</span>(<span class="type">Sample</span>);
        <span class="reserved">var</span> p = &amp;a;    <span class="comment">// コンパイル エラー: 参照型を含んだ構造体はアドレス取れない</span>
        <span class="reserved">var</span> pi = &amp;a.I;
        <span class="reserved">var</span> ps = &amp;a.S; <span class="comment">// コンパイル エラー: 参照型メンバーのアドレスは取れない</span>

        <span class="type">Console</span>.WriteLine((<span class="reserved">long</span>)pi - (<span class="reserved">long</span>)p);
        <span class="type">Console</span>.WriteLine((<span class="reserved">long</span>)ps - (<span class="reserved">long</span>)p);
    }
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="layout-kind"></a>レイアウトの指定
C#では、`StructLayout`属性(`System.Runtime.InteropServices`名前空間)を付けることで、レイアウト方式をカスタマイズ可能です。

以下の3種類のレイアウト方式を選択できます。

- Sequential: フィールドを宣言した順番通りに並べる
- Auto: コンパイラー裁量で並び替えを認める
- Explicit: 複合型の作者が明示的に位置を指定する

ちなみに、何も指定しない場合、構造体はSequentialレイアウト、クラスはAutoレイアウトになります。

また、フィールドとフィールドに何バイトの間隔を空けるか(Pack)を指定することもできます。

###<a id="sec-generated-title-5"></a> <a id="sequential-layout"></a>Sequentialレイアウト
Sequentialレイアウトでは、複合型のフィールドは宣言した順序通りにレイアウトされます。
`StructLayout`属性の引数に、`LayoutKind.Sequential`を渡します。

<pre class="source" title="Sequentialレイアウトの例">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential)]
<span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}
</code></pre>

アラインメントの説明で挙げたのと同じ絵になりますが、このコードは以下のようなレイアウトになります。

![Sequentialレイアウトの例](../../../../assets/media/1082/sequentiallayout.png)

構造体では、特に何も指定しないとSequentialレイアウトになります。
順序通りに並べるとコンパイラーごとの差異が生まれにくく、相互運用がしやすいからでしょう
(構造体はネイティブコードとの相互運用に使うことが結構多い)。

###<a id="sec-generated-title-6"></a> <a id="pack"></a>Pack指定
間隔の開け方(Pack)は、通常は、32ビットCPUであれば4 (4バイト = 32ビット)、64ビットCPUであれば8 (8バイト = 64ビット)です
(それが一番高速になる可能性が高い)。

Packを明示的に指定したい場合には、以下のように、`StructLayout`属性の`Pack`プロパティに数値を与えます。

<pre class="source" title="StructLayout属性のPackプロパティを指定">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 8)]
<span class="reserved">struct</span> <span class="type">Pack8</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 4)]
<span class="reserved">struct</span> <span class="type">Pack4</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 2)]
<span class="reserved">struct</span> <span class="type">Pack2</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 1)]
<span class="reserved">struct</span> <span class="type">Pack1</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}
</code></pre>

これで、以下のようなレイアウトになります。

![Pack指定した結果のレイアウト](../../../../assets/media/1083/structpack.png)

###<a id="sec-generated-title-7"></a> <a id="auto-layout"></a>Autoレイアウト
Autoレイアウトでは、コンパイラー裁量でフィールドの順序変更を許します。
`StructLayout`属性の引数に、`LayoutKind.Auto`を渡します。

通常、`int`型(4バイト)は4の倍数に、`long`型(8バイト)は8の倍数位置に並ぶようにしつつ、
隙間をより小さい型で埋めたりして、型のサイズが最小になるように並び替えが行われます。

例えば、以下のような構造体を書いた場合を考えます。

<pre class="source" title="Autoレイアウトの例">
<code><reserved></span><span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Auto, Pack = 8)]
<span class="reserved">struct</span> <span class="type">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}
</code></pre>

この場合、
`byte`型のフィールド2つを固めて後ろに持っていくことで、`long`型のBのアラインメントは揃えつつ、構造体のサイズを小さくします。
結果、以下のような12バイトのレイアウトになります。

![Autoレイアウトの例](../../../../assets/media/1084/autolayout.png)

(ただし、32ビット CPU の場合。これが64ビットCPUの場合はアラインメントが8バイト単位になって、 `Sample` 構造体は16バイトになります。)

ちなみに、クラスでは、特に何も指定しないとAutoレイアウトになります。

###<a id="sec-generated-title-8"></a> <a id="explicit-layout"></a>Explicitレイアウト
`StructLayout`属性の引数に、`LayoutKind.Explicit`を指定して、
フィールドに`FieldOffset`属性を付けることで、
フィールドの位置を明示的に指定することができます。

例えば、以下のような構造体を書いた場合を考えます。

<pre class="source" title="Explicitレイアウトの例">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Explicit)]
<span class="reserved">struct</span> <span class="type">Sample</span>
{
    [<span class="type">FieldOffset</span>(1)]
    <span class="reserved">public</span> <span class="reserved">byte</span> A;
    [<span class="type">FieldOffset</span>(4)]
    <span class="reserved">public</span> <span class="reserved">long</span> B;
    [<span class="type">FieldOffset</span>(15)]
    <span class="reserved">public</span> <span class="reserved">byte</span> C;
}
</code></pre>

以下のような変な隙間が空いたレイアウトになります。

![Explicitレイアウトの例](../../../../assets/media/1085/explicitlayout.png)

####<a id="sec-generated-title-9"></a> <a id="union"></a>フィールドの位置を重ねる
Explicitレイアウトを使うと、複数のフィールドの位置を重ねることもできます。
すなわち、C言語のunionのようなことができます。

例えば、以下のようなことができます。

<pre class="source" title="union的な使い方の例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Explicit)]
<span class="reserved">struct</span> <span class="type">Union</span>
{
    [<span class="type">FieldOffset</span>(0)]
    <span class="reserved">public</span> <span class="reserved">byte</span> A;

    [<span class="type">FieldOffset</span>(1)]
    <span class="reserved">public</span> <span class="reserved">byte</span> B;

    [<span class="type">FieldOffset</span>(2)]
    <span class="reserved">public</span> <span class="reserved">byte</span> C;

    [<span class="type">FieldOffset</span>(3)]
    <span class="reserved">public</span> <span class="reserved">byte</span> D;

    [<span class="type">FieldOffset</span>(0)] <span class="comment">// A と一緒</span>
    <span class="reserved">public</span> <span class="reserved">int</span> N;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Union</span> { N = 0x12345678 };
        <span class="type">Console</span>.WriteLine(x.A.ToString(<span class="string">"x"</span>)); <span class="comment">// 78</span>
        <span class="type">Console</span>.WriteLine(x.B.ToString(<span class="string">"x"</span>)); <span class="comment">// 56</span>
        <span class="type">Console</span>.WriteLine(x.C.ToString(<span class="string">"x"</span>)); <span class="comment">// 34</span>
        <span class="type">Console</span>.WriteLine(x.D.ToString(<span class="string">"x"</span>)); <span class="comment">// 12</span>
    }
}
</code></pre>

<pre class="console" title="実行結果">
<code>78
56
34
12
</code></pre>

`int`型のフィールド`N`に書き込んだ結果を、1バイト1バイト、個別に取り出しています。

ちなみに、下位バイト(この例では78)が先頭(フィールド`A`)に来るか末尾(フィールド`D`)に来るかはCPUによります(「エンディアン(endian)」と言います)。
Intel CPUの場合は先頭に来ます(little endianと言います。この逆はbig endian)。

####<a id="sec-generated-title-10"></a> <a id="abuse-union"></a>余談: Explicitレイアウトの悪用例
C# 8.0 で挙動が変わったんですが、昔の C# では、「true でも false でもない bool 型」を作ることができました。
(詳しくは「[余談: bool の網羅性](../datatype/typeswitch.md#bool-exhaustiveness)」で説明しています。)

以前の C# では、Explicit レイアウトを悪用して、([unsafe](sp_unsafe.md) すら要らずに)この「true でも false でもない bool 型」を作れて、switch ステートメントで変な挙動を起こしていました。

<span class="expand-button" title="展開/折畳">（古い C# での挙動）</span>
<div class="expand-panel" markdown="1" title="（古い C# での挙動）">

Explicitレイアウトでフィールドの位置を重ねられることで、
結構たちが悪い悪用ができたりします。
例えば、本来あり得ない値を作ったりできます。

通常、C#の`bool`型が`true`か`false`の2つの値しかとりません。
しかし、別の型のフィールドと重ねて、無理やり上書きすることで、他の値にできます。
例えば以下のようなコードが書けます。

<pre class="source" title="Explicitレイアウトの悪用例">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Explicit)]
<span class="reserved">struct</span> <span class="type">Union</span>
{
    [<span class="type">FieldOffset</span>(0)]
    <span class="reserved">public</span> <span class="reserved">bool</span> Bool;

    [<span class="type">FieldOffset</span>(0)] <span class="comment">// Bool と同じ場所</span>
    <span class="reserved">public</span> <span class="reserved">byte</span> Byte;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        Write(<span class="reserved">false</span>);   <span class="comment">// False</span>
        Write(<span class="reserved">true</span>);    <span class="comment">// True</span>

        Write(Bool(0)); <span class="comment">// False … false と一緒</span>
        Write(Bool(1)); <span class="comment">// True … true と一緒</span>
        Write(Bool(2)); <span class="comment">// Other!</span>
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">bool</span> Bool(<span class="reserved">byte</span> value)
    {
        <span class="reserved">var</span> union = <span class="reserved">new</span> <span class="type">Union</span>();
        union.Byte = value;
        <span class="reserved">return</span> union.Bool;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Write(<span class="reserved">bool</span> x)
    {
        <span class="reserved">switch</span> (x)
        {
            <span class="reserved">case</span> <span class="reserved">true</span>:
                <span class="type">Console</span>.WriteLine(<span class="string">"True"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="reserved">false</span>:
                <span class="type">Console</span>.WriteLine(<span class="string">"False"</span>);
                <span class="reserved">break</span>;
            <span class="reserved">default</span>:
                <span class="type">Console</span>.WriteLine(<span class="string">"Other!"</span>);
                <span class="reserved">break</span>;
        }
    }
}
</code></pre>

<pre class="console" title="実行結果">
<code>False
True
False
True
Other!
</code></pre>

C#の`true`、`false`は、内部的にはそれぞれ1, 0の数値になっていることが分かります。
そして、それ以外の数値を指定すると、`switch`の飛び先が変わります。

ちなみに、0以外の数値は、真偽判定(`if`ステートメントの条件式内とか)で使うと`true`扱いになります。
`ToString`の結果も`True`です。
`switch`ステートメントでだけ変な結果を起こせます。

</div>

####<a id="sec-generated-title-11"></a> <a id="illegal-layout"></a>余談: 値と参照を重ねる
Explicitレイアウトには1つ制限があります。
値型のフィールドと参照型のフィールドを同じ位置に重ねてレイアウトすることはできません。

ただし、コンパイル エラーにはならず、実行時エラーです。
(C#の制限ではなくて、.NETランタイムの制限なので、実行時にしかエラーを拾えない。)
例えば、以下のようなコードを書くと、`TypeLoadException`が発生します。

<pre class="source" title="Explicitレイアウトで値と参照を重ねる例">
<code><span class="reserved">using</span> System.Runtime.InteropServices;

[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Explicit)]
<span class="reserved">struct</span> <span class="type">Sample</span>
{
    [<span class="type">FieldOffset</span>(0)]
    <span class="reserved">public</span> <span class="reserved">int</span> A;

    <span class="comment">// 値と参照を同じ場所にレイアウト</span>
    <span class="comment">// コンパイル エラーにはならない</span>
    [<span class="type">FieldOffset</span>(0)]
    <span class="reserved">public</span> <span class="reserved">object</span> B;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// Sample 型に触れた瞬間、実行時エラーになる</span>
        <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">Sample</span>();
    }
}
</code></pre>

この制限は、[ガベージ コレクション](../../computer/essential-software/memorymanagement.md#garbage-collection)の都合です。
ガベージ コレクションは、参照をたどって誰からも参照されていないオブジェクトを探索するわけですが、
ここで、本当に参照なのかどうかがわからないものがあると探索に支障をきたします。
値と参照が重なっていると、この状態が生まれます。

ちなみに、プログラミング言語によっては、同じメモリ領域に値と参照をどちらでも格納できるものもあります。
そういう言語の場合は、以下のいずれかの処理を行っています。

- ガベージ コレクションを持たない
- すべて参照扱いにしてガベージ コレクションを走らせる
  - もちろん、本来参照じゃない場所まで参照扱いするので、回収漏れがあり得ます
- 値か参照かを弁別のために、メモリ領域の最上位ビットとかをフラグとして使う
  - こういう言語では、整数が31ビットしか使えなかったりします(C#みたいな言語の半分の大きさ)
