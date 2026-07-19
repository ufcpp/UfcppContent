---
title: "C#スクリプト実行"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/apscripting/"
content_type: "Article"
published_at: "2016-01-16T00:00:00"
updated_at: "2025-08-31T15:05:57"
tags: []
umbraco_id: 1865
parent_id: 1174
sort_order: 23
aliases:
  - "/csharp/cheatsheet/apscripting/"
---

# C#スクリプト実行

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

2015年末頃、ついにC#をスクリプト言語的に実行したり、インタラクティブに実行したりできるようになりました。すなわち、以下のようなことができるようになりました。

- アプリへの組み込み
  - アプリに組み込んで、そのアプリ用のマクロ言語としてC#を使う
  - アプリを実行したままC#スクリプトを読み直して、動的にアプリの挙動を変える
- REPL(Read Eval Print Loop)実行
  - 1行1行、都度(インタラクティブに)結果を見ながらC#を書く
- スクリプト実行
  - コマンド ライン ツールにC#スクリプト ファイルを渡して実行する
  - `class Program { static void Main() { } }`みたいなノイズなしに、1行目から式やステートメントを書ける

以下、これらを総称して「スクリプト実行」と呼びます。

通常の(コンパイルして使う)C#で書けるものは大体はスクリプト実行できます。また、スクリプト実行時にのみ許される構文や、スクリプト実行時特有の動作がいくつかあります。

## <a id="sec-generated-title-2"></a> <a id="variety"></a>いくつかの実行形態

概要で一覧を出したように、いくつかの方法でC#スクリプト実行できます。

### <a id="sec-generated-title-3"></a> <a id="hosting"></a>アプリへの組み込み

[Microsoft.CodeAnalysis.CSharp.Scripting](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp.Scripting)ライブラリを参照することで、自作のアプリにC#スクリプトを組み込めます。
例えば、以下のようなコードが書けます。

サンプル コード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Scripting/src/Scripting](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Scripting/src/Scripting)

<pre class="source" title="C#スクリプト ライブラリの利用例">
<code><reserved></span><span class="reserved">using</span> Microsoft.CodeAnalysis.CSharp.Scripting;
<span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        MainAsync().Wait();
    }

    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> MainAsync()
    {
        <span class="reserved">var</span> result = <span class="reserved">await</span> <span class="type">CSharpScript</span>.EvaluateAsync&lt;<span class="reserved">int</span>&gt;(<span class="string">@"
var x = 1;
var y = 2;
x + y
"</span>);
        <span class="type">Console</span>.WriteLine(result);
    }
}
</code></pre>

#### <a id="sec-generated-title-4"></a> <a id="script-globals"></a>スクリプトとアプリとのやり取り

アプリに組み込む以上は、アプリに対する命令みたいなものをスクリプトに対して公開する必要があるわけですが、
それはこの`EvaluateAsync`などのメソッドの引数の`globals`に対してオブジェクトを渡すことで実現できます。

例えば、以下のようなクラスを用意します。

<pre class="source" title="globalsに渡す用のコマンド発行クラス">
<code><inactive></span><span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;summary&gt;</span>
<span class="inactive">///</span><span class="comment"> コマンド発行クラス。</span>
<span class="inactive">///</span><span class="comment"> C# スクリプトのglobalsとして渡して、スクリプトからコマンドを発行するのに使う。</span>
<span class="inactive">///</span><span class="comment"> </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Commander</span>
{
    <span class="comment">// 中略</span>

    <span class="reserved">public</span> <span class="reserved">void</span> walk(<span class="reserved">double</span> distance) =&gt; _queue.Enqueue(<span class="type">Command</span>.Walk(distance));
    <span class="reserved">public</span> <span class="reserved">void</span> turn(<span class="reserved">double</span> angle) =&gt; _queue.Enqueue(<span class="type">Command</span>.Turn(angle));
    <span class="reserved">public</span> <span class="reserved">void</span> speed(<span class="reserved">double</span> speedDotPerSecond) =&gt; _queue.Enqueue(<span class="type">Command</span>.Speed(speedDotPerSecond));
    <span class="reserved">public</span> <span class="reserved">void</span> clear() =&gt; _queue.Enqueue(<span class="type">Command</span>.Clear());
}
</code></pre>

これを、`EvaluateAsync`や`RunAsync`などのスクリプトAPIの`globals`引数に渡すことで、
C#スクリプト側から、`walk`, `turn`, `speed`, `clear`などのメソッドを呼び出せるようになります。

<pre class="source" title="globalsへのオブジェクトの受け渡し">
<code>_state = <span class="reserved">await</span> CSharpScript.RunAsync(s, globals: ViewModel.Commander);
</code></pre>

ちなみに、このコードは、C#スクリプトを使ってタートル グラフィックスをやってみるサンプル プログラムの一部です。
コード全体は、GitHubで公開しています。

サンプルコード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Scripting/TurtleGraphics](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Scripting/TurtleGraphics)

実際に動かしている様子は以下の通りです。

<iframe width="420" height="315" src="https://www.youtube.com/embed/uex74qGWLxE" frameborder="0" allowfullscreen></iframe>

### <a id="sec-generated-title-5"></a> <a id="interactive-window"></a>C# インタラクティブ ウィンドウ

Visual Studio 2015 Update 1から、C#をREPL実行できる「C# インタラクティブ」というウィンドウが追加されました。

Visual Studioのメニューから下図のようにたどるか、
Visual Studio右上にある「クイック起動」欄に下図のように「C#」と打って検索することでウィンドウを開けます。

![メニューから、C#インタラクティブ ウィンドウを開く](../../../../assets/media/1063/vs-menu-csi.png)

![クイック起動から、C#インタラクティブ ウィンドウを開く](../../../../assets/media/1064/vs-quick-csi.png)

C#インタラクティブ ウィンドウ内では、下図のようにコード ハイライトやコード補完が効きます。

![コードのハイライトや補完](../../../../assets/media/1065/csi-code-completion.png)

REPL(Read Eval Print Loop)なので、1行1行コードを読んで(read)、評価して(eval)、その結果を出力(print)することができます。

![C#インタラクティブ ウィンドウを使ったREPL実行](../../../../assets/media/1066/csi-repl.png)

### <a id="sec-generated-title-6"></a> <a id="dotnet-cli"></a>dotnetコマンド

[dotnetコマンド](../devenv/ab_devenv.md#dotnetcli)の1機能として、REPL実行やスクリプト実行ができます。

下図のように、`dotnet repl`というサブコマンドを使うことでREPLが起動します。

![dotnet replサブコマンド](../../../../assets/media/1067/dotnet-repl.png)

ちなみに、`dotnet repl`は、既定動作がC# REPLの起動というだけで、引数で他の.NET言語も選べます。
(といっても、2016年1月時点ではC#のみに対応。計画としてはVisual BasicとF#への対応も考えている模様。Visual Basicはその作業真っ最中。)

1行1行書けるコードは[C#インタラクティブ ウィンドウ](#interactive-window)と同じです。
ただ、C#インタラクティブ ウィンドウと違って、コード補完などは掛かりません。
Visual Studio 2015 Update 1以降を使えるのであれば、C#インタラクティブ ウィンドウを使う方が便利でしょう。
`dotnet`コマンドはクロスプラットフォームなコマンド ライン ツールなので、GUIのない環境でも使えるという利点はあります。

REPLで1行1行実行する他に、スクリプト ファイルを与えて実行するモードがあります。
下図のように、`dotnet repl`サブコマンドの引数にファイル名を指定します。

![dotnet replサブコマンドにスクリプト ファイルを与えて実行](../../../../assets/media/1068/dotnet-csx.png)

ちなみに第1引数はどの言語を使うかを指定します。(前述の通り2016年1月時点ではC#のみ。csiかcsharpを入力。)
そして、第2引数が実行したいC#スクリプトのファイル名です。

この例では、以下のようなC#スクリプトを与えています。

<pre class="source" title="C#スクリプトの例">
<code><reserved></span><span class="reserved">using</span> System;

<span class="type">Console</span>.WriteLine(<span class="type">DateTime</span>.Now);
</code></pre>

見てのとおり、通常の(コンパイルして使う)C#と違って、トップ レベルにステートメントを書いて実行できます。
`class Program`や`static void Main()`などのクラス/メソッドは必ずしも必要ありません。

## <a id="sec-generated-title-7"></a> <a id="script-syntax"></a>スクリプト実行用の構文

通常の(コンパイルして使う)C#の機能はほぼ全て使えます。
例えば以下のように、通常のC#コードをそのままC#インタラクティブ ウィンドウに張り付けて実行できます。

<pre class="source" title="通常のC#コードをC#インタラクティブに貼り付け">
<code>&gt; <span class="reserved">using</span> System;
. 
. <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
. {
.     <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
.     {
.         <span class="type">Console</span>.WriteLine(<span class="string">"Hello World!"</span>);
.     }
. }
. 
&gt; <span class="type">Program</span>.Main()
Hello World!
</code></pre>

一方で、スクリプト実行でだけできる書き方がいくつかあります。

### <a id="sec-generated-title-8"></a> <a id="print-expression"></a>結果の出力

式を1つだけ書いて、`;`も入力せずに改行すると、その式の結果を出力します。
例えば、以下のようなコードでは、1行目は普通のC#と同じく代入ステートメント、2行目は`x * x`という式の計算結果の出力になります。

<pre class="source" title="式の計算結果の出力">
<code>&gt; <span class="reserved">var</span> x = 10;
&gt; x * x
100
</code></pre>

一方で、例えば以下のような書き方はできません。
`;` を付けたことで通常のC#構文として解釈されますが、式 + `;` という構文はC#にはないのでエラーになります。

<pre class="source" title="式の後ろには ; は付けちゃダメ">
<code>&gt; x * x<em>;</em>
(1,1): error CS0201: Only assignment, call, increment, decrement, and new object expressions can be used as a statement
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="top-level"></a>トップ レベル

通常のC#では、トップ レベル(ソースコードの一番上)に書けるものがかなり限られています。

- [プリプロセス ディレクティブ](../misc/sp_preprocess.md)
- [using ディレクティブ](../structured/sp_namespace.md#using)
- [クラス](../oop/oo_class.md)
- [名前空間](../structured/sp_namespace.md#namespace)
- [アセンブリに対する属性](../dynamic/sp_attribute.md#target)

このうち、名前空間とアセンブリに対する属性は、スクリプト実行では使えません。

<pre class="source" title="スクリプト実行で使えない構文">
<code>&gt; <span class="reserved">namespace</span> Sample { }
(1,1): error CS7021: スクリプト コードで名前空間を宣言することはできません
&gt; [<span class="reserved">assembly</span>:System.Reflection.<span class="type">AssemblyTitle</span>(<span class="string">"test"</span>)]
(1,2): error CS7026: アセンブリ属性とモジュール属性は、このコンテキストでは許可されていません。
</code></pre>

一方、スクリプト実行時には、トップ レベルに以下のようなものが書けます。

- ステートメント
- 式(結果の値が出力される)
- クラスのメンバー(メソッド、プロパティなど)

例えば以下のようなコードが書けます。

<pre class="source" title="トップ レベルのステートメントやメンバーの例">
<code>&gt; <span class="reserved">var</span> x = 10;
&gt; <span class="reserved">var</span> y = 20;
&gt; <span class="reserved">int</span> Product =&gt; x * y;
&gt; Product
200
&gt; x = 15;
&gt; y = 25;
&gt; Product
375
</code></pre>

トップ レベルで定義した変数は特殊なスコープを持ちます。
上記の例のように、トップ レベルに書いたメンバー内では参照(この例だと`Product`プロパティ内で、変数`x`, `y`を参照)できますが、
クラスを書くと、その中からは参照できません。

<pre class="source" title="">
<code>&gt; <span class="reserved">var</span> x = 10;
&gt; <span class="reserved">int</span> X =&gt; x; <span class="comment">// これはOK</span>
&gt; <span class="reserved">class</span> <span class="type">C</span> { <span class="reserved">int</span> X =&gt; x; } <span class="comment">// クラス内からは x を使えない</span>
(1,20): error CS0120: 静的でないフィールド、メソッド、またはプロパティ 'x' で、オブジェクト参照が必要です
</code></pre>

ちなみに、トップ レベルに拡張メソッドも書けます。

<pre class="source" title="トップ レベルの拡張メソッド">
<code>&gt; <span class="reserved">static</span> <span class="reserved">int</span> Square(<span class="reserved">this</span> <span class="reserved">int</span> x) =&gt; x * x;
&gt; 10.Square()
100
</code></pre>

また、トップ レベルは、通常のC#でいうところの[非同期メソッド](../async/sp5_async.md)と同じ状態になっていて、常に`await`演算子が使えます。

<pre class="source" title="トップ レベルはawaitを使える">
<code>&gt; <span class="reserved">#r</span> <span class="string">"System.Net.Http"</span>
&gt; <span class="reserved">using</span> System.Net.Http;
&gt; <span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">HttpClient</span>();
&gt; <span class="reserved">var</span> res = <span class="reserved">await</span> c.GetAsync(<span class="string">"http://ufcpp.net"</span>);
&gt; <span class="reserved">var</span> content = <span class="reserved">await</span> res.Content.ReadAsStringAsync();
&gt; content.Substring(0, 50)
"\r\n&lt;!DOCTYPE html&gt;\r\n&lt;html lang=\"ja\" xmlns=\"http://w"
</code></pre>

### <a id="sec-generated-title-10"></a> <a id="directive"></a>スクリプト用ディレクティブ

スクリプト実行時にだけ使えるものとして、[プリプロセス ディレクティブ](../misc/sp_preprocess.md)と同じ `#` から始まるいくつかのディレクティブがあります。

現状では以下のようなものがあります。

| ディレクティブ | 説明 |
| --- | --- |
| `#help` | ヘルプを表示します。 |
| `#cls`, `#clear` | ウィンドウ内のテキストをクリアします。 |
| `#reset` | コンテキスト(定義した変数やメンバーなど)をクリアします。 |
| `#r` | アセンブリを読み込みます。 |
| `#load` | スクリプト ファイルを読み込みます。 |

例えば、`a.csx`という名前で以下のようなファイルがあったとします。

<pre class="source" title="a.csx スクリプト ファイル">
<code><reserved></span><span class="reserved">var</span> x = 10;
</code></pre>

この状況下で、以下のようなスクリプトを実行できます。

<pre class="source" title="a.csxをloadするスクリプト">
<code>&gt; <span class="reserved">#load</span> <span class="string">"a.csx"</span>
&gt; x
10
</code></pre>

ディレクティブは、これからいくつか追加も予定されています。
`#help`と打つことでヘルプが表示されるので詳しくはそれを読んでみてください。
