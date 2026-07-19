---
title: "プラットフォーム呼び出し"
source_url: "https://ufcpp.net/study/csharp/interop/sp_pinvoke/"
content_type: "Article"
published_at: "2009-01-25T00:00:00"
updated_at: "2015-08-15T00:00:00"
tags: []
umbraco_id: 1324
parent_id: 1321
sort_order: 2
aliases:
  - "/csharp/interop/sp_pinvoke/"
  - "/csharp/sp_pinvoke"
  - "/csharp/sp_pinvoke.html"
  - "/study/csharp/sp_pinvoke"
  - "/study/csharp/sp_pinvoke.html"
---

# プラットフォーム呼び出し

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
.NET Frameworkには豊富なライブラリが提供されていて、C#やVisual Basicなどの.NET Framework上で動くプログラミング言語だけを使ってたいていのことができます。
しかし、その他のプログラミング言語との相互運用をしたい場面も出てくるでしょう。

特に、OSに深く食い込むような機能はいわゆるネイティブ コードで書かれたネイティブ ライブラリです。
.NET Frameworkはネイティブ ライブラリ中の機能を呼び出すための機能を備えていて、
これを<strong id="pinvoke" class="keyword">P/Invoke</strong> (Platform Invoke: プラットフォーム呼び出し)と呼びます。

ここでは、C#から、このP/Invokeを使う(ネイティブ コードを呼び出す)方法について説明します。

##### <a id="sec-generated-title-2"></a>ポイント
* .NET Framework はネイティブ ライブラリ呼び出し用の命令を持っている。
* C# でネイティブコード呼び出しをするには、DllImport 属性とかを使う。

##### <a id="sec-generated-title-3"></a>サンプル
- [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Interop/NativeInterop](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Interop/NativeInterop)

##<a id="sec-generated-title-4"></a> <a id="native"></a>ネイティブ コード
C# から呼び出せるネイティブ コードには以下のようなものがあります。

- C-Style 関数
- COM オブジェクト
- WinRT コンポーネント

##<a id="sec-generated-title-5"></a> <a id="c-style-function"></a>C-Style 関数
C-Style 関数は、C言語で書いた関数や、C++ で「`extern "C"`」内に書いた関数です。Unix系OSのAPIや、初期のWindows API (Win32 APIと呼ばれています)はこの形式で提供されています。

###<a id="sec-generated-title-6"></a> <a id="dllimport"></a>DllImport
C# から C-Stlye 関数を呼び出すには、`DllImport`属性(`System.Runtime.InteropServices`名前空間)を使います。
例えば、以下のように書きます。

<pre class="source" title="DllImport を使って C-Style の Windows API を呼び出す">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

<span class="reserved">namespace</span> NativeInterop
{
    <span class="reserved">class</span> <span class="type">DllImportSample</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="type">SYSTEMTIME</span> t;
            GetLocalTime(<span class="reserved">out</span> t);

            <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{t.wYear}<span class="string">/</span>{t.wMonth}<span class="string">/</span>{t.wDay}<span class="string"> </span>{t.wHour}<span class="string">:</span>{t.wMinute}<span class="string">:</span>{t.wSecond}<span class="string">"</span>);
        }

        [<span class="type">DllImport</span>(<span class="string">"kernel32.dll"</span>)]
        <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> GetLocalTime(<span class="reserved">out</span> <span class="type">SYSTEMTIME</span> lpSystemTime);
    }

    [<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential, Pack = 2)]
    <span class="reserved">struct</span> <span class="type">SYSTEMTIME</span>
    {
        <span class="reserved">public</span> <span class="reserved">ushort</span> wYear;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wMonth;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wDayOfWeek;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wDay;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wHour;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wMinute;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wSecond;
        <span class="reserved">public</span> <span class="reserved">ushort</span> wMilliseconds;
    }
}
</code></pre>

<pre class="console"><code>2015/8/15 1:42:37
</code></pre>

このコードで、`kernel32.dll` という Windows のネイティブ ライブラリ中にある[`GetLocalTime`](https://msdn.microsoft.com/ja-jp/library/Cc429760.aspx)という関数を呼び出せます。

上記の例を見ての通り、結構なコードを書く必要があります。以下のようなものが必要です。

- `DllImport`属性の引数に、参照したいネイティブ ライブラリの名前を書く
- メソッド名は、呼び出したい関数の名前をそのまま付ける
- C# 側の型とネイティブ側の型には対応関係があるので、適切に置き換えて戻り値や引数を並べる
  - 対応関係についてはMSDNを参照: [プラットフォーム呼び出しによるデータのマーシャリング](https://msdn.microsoft.com/ja-jp/library/fzhhdwae.aspx)
- 引数で構造体が使われている場合、同じレイアウトの構造体を C# コード中に書く必要がある

使いたい関数に対して一つ一つこの作業が必要ですが、さすがにめんどくさいので、Win32 API名からP/Invoke用のコードを検索できるサイトがあったりします。

- [http://pinvoke.net/](http://pinvoke.net/)

###<a id="sec-generated-title-7"></a> <a id="extern-modifier"></a>extern 修飾子
ちなみに、この`DllImport`では、「実装が外にあるメソッド」を書くことになります。
この例の場合、`GetLocalTime`には実装(メソッドの本体)がない代わりに、<strong id="extern-modifier" class="keyword">`extern`修飾子</strong>がついています。
`extern`は実装が外にあることを示すための修飾子で、P/Invoke 用の機能です。

###<a id="sec-generated-title-8"></a> <a id="marshaling"></a>マーシャリング
`DllImport`属性を使った P/Invoke の手順の中に、
「C# 側の型とネイティブ側の型には対応関係があるので、適切に置き換えて戻り値や引数を並べる」
というものがありました。

この、C# 側の型とネイティブ側の型の対応関係に基づいて型を置き換える処理のことを<strong id="key-marshaling" class="keyword">マーシャリング</strong> (marshalling: 整列)といいます。

marshal という単語には、整列の他に、元帥(軍事司令官)、(パレードなどの)式典担当という意味があります。
つまり、marshal は、同じ整列でも sort や order よりも「統括者がいて、責任をもって並べる」というような意味合いが強くなります。
C# とネイティブの境界では、そういう「誰かが責任を持った整列」が行われないと危険ということです。

C# とネイティブの間に限らず、C# 同士であっても、セキュリティ的に隔離したい2つのプログラムの間では、同様にマーシャリングと呼ばれる過程(誰かが責任をもってデータを受け渡しする)を通したデータの受け渡しが必要になります。

#### <a id="sec-generated-title-9"></a>サンプル
- [KeyLogger](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/KeyLogger)

キー入力を全部記録して、それをマクロ的に再生するプログラム。
昔、ブラウザー ゲームでボット プレイするために作ったもの。

`SetWindowsHookEx`とか`SendInput`とかの Win32 API を使っています。
このあたりのAPIの使い方は、以下のページを参考にして作りました。

- （[Processing Global Mouse and Keyboard Hooks in C#](http://www.codeproject.com/KB/cs/globalhook.aspx)）

保守していないし今ちゃんと動く保証なし。

<!-- original-page-break -->

##<a id="sec-generated-title-10"></a> <a id="COM"></a><a id="com"></a>COM オブジェクト
COM (Component Object Model)は、かなり端折って言うと、プログラミング言語をまたいでクラスやメソッドを使うための規格です。
マイクロソフトが作った規格で、ほぼWindows用(規格はオープンだし、Unix上での利用ガイドもあるものの、Windows以外ではあまり使われない)です。
DirectXなど新し目のWindows APIはCOMで実装されています。また、OfficeやInternet ExplorerなどのWindowsアプリはCOMを介して、自作のプログラムからアプリ中の機能を呼び出すことができます。

.NET Frameworkの型システムは、このCOMの発展形です。

###<a id="sec-generated-title-11"></a> <a id="com-reference"></a>COM 参照
C# からの COM 利用は、Visual Studio を使えば簡単にできます。
Visual Studio 上で、下図のように、「参照の追加」→「COM」→参照したいDLLを選んで「OK」という手順を踏みます。

![COM参照](../../../../assets/media/1030/comreference.png)

この図の例の場合、MSXML2 という COM ライブラリを参照します。
これで、例えば以下のように、MSXML2 中のクラス(この例では`DOMDocument60`クラス)を使えます。

<pre class="source" title="COMの参照">
<code><reserved></span><span class="reserved">using</span> MSXML2;
<span class="reserved">using</span> System;

<span class="reserved">namespace</span> NativeInterop
{
    <span class="reserved">class</span> <span class="type">ComImportSample</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="reserved">var</span> doc = <span class="reserved">new</span> <span class="type">DOMDocument60</span>();

            <span class="reserved">if</span> (doc.load(<span class="string">"Sample.xml"</span>))
            {
                <span class="reserved">var</span> s = doc.documentElement;

                <span class="reserved">foreach</span> (<span class="type">IXMLDOMElement</span> item <span class="reserved">in</span> s.getElementsByTagName(<span class="string">"Item"</span>))
                {
                    <span class="reserved">var</span> name = item.getAttribute(<span class="string">"Name"</span>);
                    <span class="reserved">var</span> value = item.getAttribute(<span class="string">"Value"</span>);

                    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{name}<span class="string"> = </span>{value}<span class="string">"</span>);
                }
            }
        }
    }
}
</code></pre>

見ての通り、C#からCOMオブジェクトは、普通にC#のクラスっぽく見えます。
造りが古臭いせいで面倒になりがちですが、そこまで違和感なく使えます。

ここで、このコードに対して与えるデータ(`Sample.xml`)として以下のようなものを用意したとすると、

<pre class="xsource" title="Sample.xml">
<code><attvalue></span><span class="attvalue">&lt;?</span><span class="element">xml</span><span class="attvalue"> </span><span class="attribute">version</span><span class="attvalue">=</span>"<span class="attvalue">1.0</span>"<span class="attvalue"> </span><span class="attribute">encoding</span><span class="attvalue">=</span>"<span class="attvalue">utf-8</span>"<span class="attvalue"> ?&gt;</span>
<span class="attvalue">&lt;</span><span class="element">Sample</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Item</span><span class="attvalue"> </span><span class="attribute">Name</span><span class="attvalue">=</span>"<span class="attvalue">a</span>"<span class="attvalue"> </span><span class="attribute">Value</span><span class="attvalue">=</span>"<span class="attvalue">1</span>"<span class="attvalue">/&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Item</span><span class="attvalue"> </span><span class="attribute">Name</span><span class="attvalue">=</span>"<span class="attvalue">b</span>"<span class="attvalue"> </span><span class="attribute">Value</span><span class="attvalue">=</span>"<span class="attvalue">2</span>"<span class="attvalue">/&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Item</span><span class="attvalue"> </span><span class="attribute">Name</span><span class="attvalue">=</span>"<span class="attvalue">c</span>"<span class="attvalue"> </span><span class="attribute">Value</span><span class="attvalue">=</span>"<span class="attvalue">3</span>"<span class="attvalue">/&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Item</span><span class="attvalue"> </span><span class="attribute">Name</span><span class="attvalue">=</span>"<span class="attvalue">d</span>"<span class="attvalue"> </span><span class="attribute">Value</span><span class="attvalue">=</span>"<span class="attvalue">4</span>"<span class="attvalue">/&gt;</span>
<span class="attvalue">&lt;/</span><span class="element">Sample</span><span class="attvalue">&gt;</span>
</code></pre>

以下の結果が得られます。

<pre class="console"><code>a = 1
b = 2
c = 3
d = 4
</code></pre>

###<a id="sec-generated-title-12"></a> <a id="rcw-ccw"></a>RCW と CCW
前節の「COM参照」をすると、コンパイラーが以下のようなクラスを生成します。

<pre class="source" title="「COM参照」でで生成されるクラス">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Runtime.InteropServices;
<span class="reserved">namespace</span> MSXML2
{
    [<span class="type">CompilerGenerated</span>, <span class="type">CoClass</span>(<span class="reserved">typeof</span>(<span class="reserved">object</span>)), <span class="type">Guid</span>(<span class="string">"2933BF96-7B36-11D2-B20E-00C04F983E60"</span>), <span class="type">TypeIdentifier</span>]
    [<span class="type">ComImport</span>]
    <span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">DOMDocument60</span> : <span class="type">IXMLDOMDocument3</span>, <span class="type">XMLDOMDocumentEvents_Event</span>
    {
    }
}
</code></pre>

<pre class="source" title="">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Runtime.InteropServices;
<span class="reserved">namespace</span> MSXML2
{
    [<span class="type">CompilerGenerated</span>, <span class="type">Guid</span>(<span class="string">"2933BF86-7B36-11D2-B20E-00C04F983E60"</span>), <span class="type">TypeIdentifier</span>]
    [<span class="type">ComImport</span>]
    <span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IXMLDOMElement</span> : <span class="type">IXMLDOMNode</span>
    {
        <span class="reserved">void</span> _VtblGap1_37();
        [<span class="type">DispId</span>(99)]
        [<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.InternalCall)]
        [<span class="reserved">return</span>: <span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span>.Struct)]
        <span class="reserved">object</span> getAttribute([<span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span>.BStr)] [<span class="type">In</span>] <span class="reserved">string</span> name);
        <span class="reserved">void</span> _VtblGap2_5();
        [<span class="type">DispId</span>(105)]
        [<span class="type">MethodImpl</span>(<span class="type">MethodImplOptions</span>.InternalCall)]
        [<span class="reserved">return</span>: <span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span>.Interface)]
        <span class="type">IXMLDOMNodeList</span> getElementsByTagName([<span class="type">MarshalAs</span>(<span class="type">UnmanagedType</span>.BStr)] [<span class="type">In</span>] <span class="reserved">string</span> tagName);
    }
}
</code></pre>

要は、C-Style 関数の時に`DllImport`属性を使ったように、
COM オブジェクトに対しては `ComImport` という属性を使います。

このような、C# から COM オブジェクトを呼び出すためのラッパー クラスを<strong id="rcw" class="keyword">RCW</strong> (Runtime Callable Wrapper)と呼びます。

逆に、詳細はここでは省略しますが、C# で書いたクラスを COM 側から使う手段もあって、そちらは<strong id="ccw" class="keyword">CCW</strong> (COM Callable Wrapper)と呼ばれます。

####<a id="sec-generated-title-13"></a> <a id="no-pia"></a>No PIA
<h5 class="version version4">Ver. 4.0</h5>

.NET Framework 3.5以前では、RCW を介してのCOM呼び出しに少し問題がありました。

先ほど例示したように、COM参照すると RCW と呼ばれるラッパークラスが生成されます。
プログラム中で使っている型・メソッドだけが残されていて、不要なメンバーはありません。

ここで問題になるのは、複数のライブラリ中にそれぞれ別個に RCW が生成された場合です。
.NET では、「アセンブリ＋名前」の組み合わせで型の所在を検索するので、同名・同機能であっても、別のライブラリ中に定義されていたら別の型扱いになります。

そこで、複数のライブラリからCOM参照したい場合、.NET Framework 3.5以前では、PIA (Primary Interop Assembly: プライマリ相互運用アセンブリ)といって、RCW 専用のCOMオブジェクト中の全メンバーを参照したライブラリを作って使う必要がありました(参考: [PIA に関するドキュメント](https://msdn.microsoft.com/ja-jp/library/Aa302338.aspx))。

PIAは、まじめに作ると結構馬鹿でかいファイルになってしまいます。
それが嫌で、.NET Framework 4からは、アセンブリやメンバー定義が違っていても、RCW の GUIDが同じなら同じ型とみなして扱うという特殊処理が入りました(この処理は No PIA と呼ばれています)。
この処理によって、PIAなしでも複数のライブラリからCOMオブジェクトの参照ができるようになりました。

##<a id="sec-generated-title-14"></a> <a id="dynamic"></a>dynamic と COM 呼び出し
<h5 class="version version4">Ver. 4.0</h5>

C# 4.0の `dynamic` (参考: 「[動的型付け変数](../dynamic/sp4_dynamic.md#dynamic)」)を使えば、「COM 参照」すらなしで COM オブジェクトを呼び出せます。

例えば、先ほどのコードは以下のように書き換えることもできます。

<pre class="source" title="dynamic を使ったCOM呼び出し">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">namespace</span> NativeInterop
{
    <span class="reserved">class</span> <span class="type">ComLateBindingSample</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            <span class="reserved">var</span> t = <span class="type">Type</span>.GetTypeFromProgID(<span class="string">"MSXML2.DOMDocument"</span>);
            <em><span class="reserved">dynamic</span> doc = <span class="type">Activator</span>.CreateInstance(t);</em>

            <span class="reserved">if</span> (doc.load(<span class="string">"Sample.xml"</span>))
            {
                <span class="reserved">var</span> s = doc.documentElement;

                <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> s.getElementsByTagName(<span class="string">"Item"</span>))
                {
                    <span class="reserved">var</span> name = item.getAttribute(<span class="string">"Name"</span>);
                    <span class="reserved">var</span> value = item.getAttribute(<span class="string">"Value"</span>);

                    <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{name}<span class="string"> = </span>{value}<span class="string">"</span>);
                }
            }
        }
    }
}
</code></pre>

インスタンス `doc` を作るところが `new` から `CreateInstance` に代わって、変数の型が `dynamic` になっただけで、そこから先のコードはほぼ同じです。
これで、「COM 参照」は必要なくなり、RCW は実行時に動的に作られます。

この機能は、RCW が不要になる分、プログラムのサイズが縮むというメリットがあります。
一方、開発時には、どのクラスにどういうメンバーがあるといった情報が得られる、コード補完が効かなくなるというデメリットがあります。
なので、開発時には「COM参照」を使って開発して、
最後に`dynamic`に置き換えて、「COM参照」を消してからコンパイルするという手順を踏むとよいかもしれません。

<!-- original-page-break -->

##<a id="sec-generated-title-15"></a> <a id="WinRT"></a>WinRT コンポーネント
WinRT (Windows Runtime)は、Windows 8以降で実装された、新しいWindows APIです。
WinRTコンポーネント(WinRT APIが提供するクラスなど)は、COMの進化版(COMの上位互換)に、
.NET Frameworkの型情報を加えたような形式になっています。

WinRT コンポーネントは、Windows 8世代、つまり、.NET Frameworkよりもだいぶ後に作られただけあって、
C# からの参照はかなり簡単になっています。
C# で書かれたライブラリとほとんど区別なく WinRT コンポーネントを参照できます。
よっぽど意識しない限り、ネイティブ ライブラリを参照しているとは感じないでしょう。

Visual Studio 上では、下図のように、「参照の追加」→「Windows」→参照したいコンポーネントを選んで「OK」という手順を踏みます。

![WinRTコンポーネントの参照](../../../../assets/media/1031/winrtreference.png)

###<a id="sec-generated-title-16"></a> <a id="universal-windows"></a>Windows アプリ
WinRT は前述のとおり、Windows 8世代の新APIです。
Windows 8から Windows 10にかけて紆余曲折ありましたが、要は、以下のタイプのアプリから使う前提のものです。

- Windows ストア アプリ
  - 「Modern アプリ」とか「メトロ スタイル」とか呼ばれていた時期もあります
  - これを単に「Windows アプリ」と呼んで、これまでの Win32 API ベースのアプリは「従来のデスクトップ アプリ」と呼びたがっている(呼ばれるようになってほしい)節もあります
- Universal Windows Platform (UWP)アプリ

これらに関連したプロジェクトを作ると、標準の状態で WinRT コンポーネントの参照ができます。

###<a id="sec-generated-title-17"></a> <a id="classic-desktop"></a>従来のデスクトップ アプリ
標準の状態では無理ですが、少し手を入れることで、従来のデスクトップ アプリからもWinRTコンポーネントを参照できます。

csproj を手書きで書き換える必要があります。以下のように、`TargetPlatformVersion`というタグを1行追加します。

<pre class="xsource" title="">
<code><attvalue></span><span class="attvalue">&lt;?</span><span class="element">xml</span><span class="attvalue"> </span><span class="attribute">version</span><span class="attvalue">=</span>"<span class="attvalue">1.0</span>"<span class="attvalue"> </span><span class="attribute">encoding</span><span class="attvalue">=</span>"<span class="attvalue">utf-8</span>"<span class="attvalue">?&gt;</span>
<span class="attvalue">&lt;</span><span class="element">Project</span><span class="attvalue"> </span><span class="attribute">ToolsVersion</span><span class="attvalue">=</span>"<span class="attvalue">14.0</span>"<span class="attvalue"> </span><span class="attribute">DefaultTargets</span><span class="attvalue">=</span>"<span class="attvalue">Build</span>"<span class="attvalue"> </span><span class="attribute">xmlns</span><span class="attvalue">=</span>"<span class="attvalue">http://schemas.microsoft.com/developer/msbuild/2003</span>"<span class="attvalue">&gt;</span>
<span class="attvalue">  &lt;</span><span class="element">Import</span><span class="attvalue"> </span><span class="attribute">Project</span><span class="attvalue">=</span>"<span class="attvalue">$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props</span>"<span class="attvalue"> </span><span class="attribute">Condition</span><span class="attvalue">=</span>"<span class="attvalue">Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')</span>"<span class="attvalue"> /&gt;</span>
<span class="attvalue">  &lt;</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Configuration</span><span class="attvalue"> </span><span class="attribute">Condition</span><span class="attvalue">=</span>"<span class="attvalue"> '$(Configuration)' == '' </span>"<span class="attvalue">&gt;</span>Debug<span class="attvalue">&lt;/</span><span class="element">Configuration</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">Platform</span><span class="attvalue"> </span><span class="attribute">Condition</span><span class="attvalue">=</span>"<span class="attvalue"> '$(Platform)' == '' </span>"<span class="attvalue">&gt;</span>AnyCPU<span class="attvalue">&lt;/</span><span class="element">Platform</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">ProjectGuid</span><span class="attvalue">&gt;</span>{F404E6CA-F7FD-4AB8-A531-D8203BCC3F70}<span class="attvalue">&lt;/</span><span class="element">ProjectGuid</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>Exe<span class="attvalue">&lt;/</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">AppDesignerFolder</span><span class="attvalue">&gt;</span>Properties<span class="attvalue">&lt;/</span><span class="element">AppDesignerFolder</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">RootNamespace</span><span class="attvalue">&gt;</span>NativeInterop<span class="attvalue">&lt;/</span><span class="element">RootNamespace</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">AssemblyName</span><span class="attvalue">&gt;</span>NativeInterop<span class="attvalue">&lt;/</span><span class="element">AssemblyName</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">TargetFrameworkVersion</span><span class="attvalue">&gt;</span>v4.6<span class="attvalue">&lt;/</span><span class="element">TargetFrameworkVersion</span><span class="attvalue">&gt;</span>
<em><span class="attvalue">    &lt;</span><span class="element">TargetPlatformVersion</span><span class="attvalue">&gt;</span>10.0.10240.0<span class="attvalue">&lt;/</span><span class="element">TargetPlatformVersion</span><span class="attvalue">&gt;</span></em>
<span class="attvalue">    &lt;</span><span class="element">FileAlignment</span><span class="attvalue">&gt;</span>512<span class="attvalue">&lt;/</span><span class="element">FileAlignment</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">AutoGenerateBindingRedirects</span><span class="attvalue">&gt;</span>true<span class="attvalue">&lt;/</span><span class="element">AutoGenerateBindingRedirects</span><span class="attvalue">&gt;</span>
<span class="attvalue">  &lt;/</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="inactive">...</span>
</code></pre>

`TargetPlatformVersion`タグの中身には、`8.0`, `8.1`, `10.0` など、Windows のバージョンを書きます。

これで、例えば、以下のようなコンソール アプリで、WinRT コンポーネントを使えます。

<pre class="source" title="WinRT コンポーネントをコンソール アプリから利用">
<code><reserved></span><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Threading.Tasks;
<span class="reserved">using</span> Windows.Foundation;
<span class="reserved">using</span> Windows.System;

<span class="reserved">namespace</span> NativeInterop
{
    <span class="reserved">class</span> <span class="type">WinRtSample</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
        {
            MainAsync().Wait();
        }

        <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> MainAsync()
        {
            <span class="reserved">var</span> allUsers = <span class="reserved">await</span> <span class="type">User</span>.FindAllAsync();

            <span class="reserved">foreach</span> (<span class="reserved">var</span> user <span class="reserved">in</span> allUsers)
            {
                <span class="type">Console</span>.WriteLine(user.NonRoamableId);
            }
        }
    }

    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">WinRtExtensions</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">TaskAwaiter</span>&lt;<span class="type">T</span>&gt; GetAwaiter&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IAsyncOperation</span>&lt;<span class="type">T</span>&gt; t) =&gt; t.AsTask().GetAwaiter();

        <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="type">T</span>&gt; AsTask&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IAsyncOperation</span>&lt;<span class="type">T</span>&gt; t)
        {
            <span class="reserved">var</span> tcs = <span class="reserved">new</span> <span class="type">TaskCompletionSource</span>&lt;<span class="type">T</span>&gt;();
            t.Completed += (info, state) =&gt;
            {
                <span class="reserved">try</span>
                {
                    tcs.TrySetResult(info.GetResults());
                }
                <span class="reserved">catch</span>(<span class="type">Exception</span> ex)
                {
                    tcs.TrySetException(ex);
                }
            };
            <span class="reserved">return</span> tcs.Task;
        }
    }
}
</code></pre>
