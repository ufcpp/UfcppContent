---
title: "Hardware Intrinsics"
source_url: "https://ufcpp.net/blog/2018/12/hdintrinsic/"
content_type: "BlogEntry"
published_at: "2018-12-31T10:17:34"
updated_at: "2018-12-31T10:17:34"
tags: []
umbraco_id: 2214
parent_id: 2177
sort_order: 31
aliases: []
---

# Hardware Intrinsics

今日は、おそらく .NET Core 3.0 で正式リリースとなるであろう最適化の話。
Hardware Intrinsics といって、特定 CPU の専用命令を利用するための機能の話になります。

元々は .NET Core 2.1 の頃に作業が始まっているんですが、2.1 リリースのタイミングには間に合いませんでした。
しかし、内部的な対応はすでに入っていて、[daily ビルドなパッケージ](https://dotnet.myget.org/feed/dotnet-core/package/nuget/System.Runtime.Intrinsics.Experimental)を参照すれば、今現在の .NET Core 2.1 でも利用可能です。
というか、[ドキュメントはすでにあります](https://docs.microsoft.com/ja-jp/dotnet/api/system.runtime.intrinsics.x86?view=dotnet-plat-ext-2.1)。

## CPU 専用命令

いろいろなプログラミング言語で書かれたプログラムを比較したとき、
傾向として言うと、C 言語や C++ で書かれたものが最速です。

C# は、これら C や C++ と比較してどこがボトルネックでしょう。
印象としては[ガベージ コレクション](../../../../study/computer/essential-software/memorymanagement.md#garbage-collection)が遅そうに思われるかもしれませんが、案外、別のところにも原因があります。
(C# でもヒープ アロケーションを避けるコードは書けます。
それに、ヒープをどうしても避けれない場合だけに限定していうと、
ガベージ コレクションによるヒープ管理はものすごく高速です。)

高速化の行き着く先は、特定の CPU の専用命令をどれだけうまく使えるかになったりします。

例えば、32ビット整数の中から、特定のビットだけを抜き出すことを考えてみます。
普通に C# で書くと以下のような感じ。

<pre class="source" title="uint 中の特定のビットを抜き出し">
<code><span class="reserved">struct</span> <span class="type">SingleView</span>
{
    <span class="reserved">public</span> <span class="reserved">uint</span> Value;
 
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
    <span class="inactive">///</span><span class="comment"> Value のうち、23～31ビット目の値を抜き出す。</span>
    <span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/</span><span class="inactive">summary</span><span class="inactive">&gt;</span>
    <span class="reserved">public</span> <span class="reserved">uint</span> Exponent
    {
        <span class="reserved">get</span> =&gt; (Value &amp; 0x7F800000) &gt;&gt; 23;
        <span class="reserved">set</span> =&gt; Value = (<span class="reserved">uint</span>)((Value &amp; ~0x7F800000) | ((<span class="reserved">value</span> &lt;&lt; 23) &amp; 0x7F800000));
    }
}
</code></pre>

AND とか OR とかシフト演算がいくつか必要です。

ところが、これ、たいていの CPU で1命令で実行できる命令があったりします。
[x86 CPU だと BEXTR 命令](http://www.felixcloutier.com/x86/BEXTR.html)、
[ARM だと UBFX 命令](http://infocenter.arm.com/help/index.jsp?topic=/com.arm.doc.dui0489f/Cjahjhee.html)というのがそれです。

理想をいうと、先ほどの AND とシフトな C# コードから、ちゃんと最適化でこれらの専用命令に翻訳されてほしいんですが、そんなにうまく行かないことが多いです。

そこで行き着く先は、「[インライン アセンブラ](https://ja.wikipedia.org/wiki/%E3%82%A4%E3%83%B3%E3%83%A9%E3%82%A4%E3%83%B3%E3%82%A2%E3%82%BB%E3%83%B3%E3%83%96%E3%83%A9)を書かせろ」となったりします。
実際、速いといわれている C/C++ コードは、CPU 専用命令を使ってガチガチに最適化していたりします。

実のところ、C/C++ と比べたときに C# (や、Java, Go, Swift 辺りの「そこそこ速い」言語)が遅い理由の結構な割合が、こういう専用命令利用に関する部分だったりします。

## Intrinsics

ということで、C# 内にもインライン アセンブラを書きたいという要望はあるんですが。

しかし、「C# の中で別の言語を保守する」というのは、コンパイラーを作る側にとっても使う側にとってもかなりのハードル・足かせになります。
そこで最近よく取られる手法が、「intrinsic 関数の提供」です。

「[JIT Intrinsics](../jitintrinsics/index.md)」でも書きましたが、
intrinsic というのは固有の、内在的な、内因的な、本質的なという意味の単語で、
概ね、「内部的に特別扱いして最適化しているもの」という意味で使われています。

そして、intrinsic 関数(あるいは単に intrinsics)というのは、

- 普通に関数(C# だと静的メソッド)としてライブラリ提供する
- その関数を見たら特定の CPU 命令に置き換える

というようなもののことです。

例えば C++ でも、有名なものでは、[Intel Intrinsics](https://software.intel.com/sites/landingpage/IntrinsicsGuide/) というものがあります。
名前通り Intel CPU 向けのものですが、Visual C++, GCC, clang など、Intel 製以外の C/C++ コンパイラーでも大体利用できます。
`mmintrin.h` とかで検索してもらうとサンプル コードがすぐに見つかると思います。
以下のような感じで、普通の C++ コードを書くと、それが特定の Intel CPU 命令に置き換わります。

<pre class="source" title="C++ での Intel Intrisics">
<code><span class="reserved">#include</span> <span class="string">&lt;immintrin.h&gt;</span>
<span class="comment">// 中略</span>
<span class="type">__m128</span> c = _mm_mul_ps(a, b);
</code></pre>

いわゆる SIMD 演算というやつで、
複数の積和演算を1命令で実行するので、うまく使えば数値計算が4～8倍速くなったりします。

ただし、注意点もあります。
特定 CPU の専用命令を使うための手法なので当然なんですが、
特定の CPU に依存します。
上記の Intel Intrinsics であれば当然 Intel CPU でしか動きません。
同じ Intel 系の CPU でも、世代を追うごとに命令がどんどん追加されているわけで、
古い CPU では対応していない命令が大量にあります。

その結果どうなるかというと、ガチガチに最適化するなら `#ifdef` だらけになります。
例え古い CPU のサポートを切ったとしても、Intel 系と ARM 系の2種類は保守が必要になったりします。

## .NET でも Hardware Intrinsics

ということで、 .NET Core 2.1 くらいの頃から、.NET にも Hardware Intrinsics を入れたいという話が出ます。

実際、実は内部的にはもうその対応が入っていて、以下のパッケージを参照すれば .NET Core 2.1 で Hardware Intrinsics を使えます。

- [System.Runtime.Intrinsics.Experimental](https://dotnet.myget.org/feed/dotnet-core/package/nuget/System.Runtime.Intrinsics.Experimental)

現状は、nuget.org からは落とせません。
MyGet (daily ビルド用の CI サーバー)からのみ取得できます。
また、正式リリースされた暁には Experimental が外れて、System.Runtime.Intrinsics パッケージになると思われます(もしかしたら、X86 と Arm で別パッケージになるかも)。

例えば、最初に出した「特定のビットだけを抜き出す」コードは以下のように書けます。

<pre class="source" title="System.Runtime.Intrinsics">
<code><span class="reserved">using</span> System.Runtime.Intrinsics.X86;
 
<span class="reserved">struct</span> <span class="type">SingleView</span>
{
    <span class="reserved">public</span> <span class="reserved">uint</span> Value;
 
    <span class="reserved">public</span> <span class="reserved">uint</span> Exponent
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">if</span> (<span class="type">Bmi1</span>.IsSupported) <span class="reserved">return</span> <span class="type">Bmi1</span>.BitFieldExtract(Value, 23, 8);
            <span class="reserved">else</span> <span class="reserved">return</span> (Value &amp; 0x7F800000) &gt;&gt; 23;
        }
        <span class="comment">// set 割愛</span>
    }
}
</code></pre>

他にも、先ほど挙げた Intel Intrinsics 相当のメソッドもあります。

ちなみに、ここで出てくる `IsSupported` プロパティは JIT 時定数になります。
このコードは、JIT が掛かるタイミングで、
この CPU 命令セットを持っている環境なら `if` 側、
持っていない環境なら `else` 側だけが残ります。

なのでパフォーマンス的にはかなりいいものに仕上がるんですが、
見ての通り、同じ意味のコードを2回書く必要があります。
もちろん、ARM 系 CPU にも対応したければ3回に。

要するに、C/C++ でよくある「ガチガチに最適化するなら `#ifdef` だらけ」が、C# でも書けるようになります…
大変さと引き換えに、数倍高速なコードが手に入ります。
