---
title: "エントリー ポイント"
source_url: "https://ufcpp.net/study/csharp/misc/miscentrypoint/"
content_type: "Article"
published_at: "2020-07-05T00:00:00"
updated_at: "2024-08-31T17:24:49"
tags:
  - "Ver. 9.0"
umbraco_id: 2301
parent_id: 1338
sort_order: 8
aliases:
  - "/csharp/misc/miscentrypoint/"
---

# エントリー ポイント

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
実行可能なプログラムを書くとき、最初に呼び出される処理を<strong id="key-entry-point" class="keyword">エントリー ポイント</strong>(entry point: 入場地点、入り口)と言います。
C# の場合、通常、`Main` という名前の静的メソッドを1個だけ書くことで、このメソッドがエントリー ポイントになります。

また、複数の `Main` メソッドを書いてそのうちの1つをエントリー ポイントに選ぶ方法があったり、
C# 9.0 からはトップ レベル ステートメントという書き方でエントリー ポイントを作れたりします。

本項ではこの C# のエントリー ポイントに関する仕様について説明します。

##<a id="sec-generated-title-2"></a> <a id="entry-point-in-csharp"></a>C# のエントリー ポイント
C# 関連のチュートリアルでのサンプル コードや、
テンプレート通りに C# プログラムを新規作成すると以下のような内容になっていることが多いと思います。

<pre class="source" title="よくあるチュートリアル・テンプレート通りの C# コード">
<code><span class="reserved">using</span> System;
 
<span class="reserved">namespace</span> ConsoleApp1
{
    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>(<span class="reserved">string</span>[] args)
        {
            <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
        }
    }
}
</code></pre>

C# の仕様上、実行可能プログラムを C# で書きたい場合、どこかに1つ、`Main` という名前のメソッドが必要です。
(後述しますが、C# 9.0 からは別の方法も追加されました。)

名前空間は必須ではありません。クラス名も何でも構いません。
例えば以下のようなコードでも、`Main` メソッドがエントリー ポイントになります。

<pre class="source" title="名前空間はなくてもいい。クラス名も任意">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>(<span class="reserved">string</span>[] args)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
    }
}
</code></pre>

通常、エントリー ポイントとして使うためには、`Main` メソッドに以下のような制限があります。

- [静的](../oop/oo_static.md)である(`static` 修飾子が付いてる)必要がある
- 引数はなしか、`string[]` のどちらか
- 戻り値はなし(`void`)か、`int` のどちらか
  - [C# 7.1](../cheatsheet/ap_ver7_1.md#async-Main) 以降は追加で `Task` か `Task<int>` も OK

つまり、C# 7.0 以前だと以下の4つのうちのいずれかが、

<pre class="source" title="エントリー ポイントとして許される Main メソッドの書き方">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>(<span class="reserved">string</span>[] args)
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Main</span>()
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">Main</span>(<span class="reserved">string</span>[] args)
</code></pre>

加えて、C# 7.1 以降だと以下の4つのうちのいずれかが認められます。

<pre class="source" title="エントリー ポイントとして許される Main メソッドの書き方 (C# 7.1 以降)">
<code><span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">static</span> <span class="type">Task</span> <span class="method">Main</span>()
<span class="reserved">static</span> <span class="type">Task</span> <span class="method">Main</span>(<span class="reserved">string</span>[] args)
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; <span class="method">Main</span>()
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; <span class="method">Main</span>(<span class="reserved">string</span>[] args)
</code></pre>

##<a id="sec-generated-title-3"></a> <a id="entry-point-in-dotnet"></a>.NET のエントリー ポイント
前述の `Main` という名前が必須なのは C# の仕様上の話で、
その下層、 .NET ランタイムにはそういう制限はありません。
`.entrypoint` ディレクティブで修飾したメソッドがエントリー ポイントになります。

例えば、以下のような .NET IL アセンブラー コードを書けば、`A` というクラス内の `B` というメソッドをエントリー ポイントにできます。

<pre class="source" title="">
<code>.class public auto ansi beforefieldinit A
       extends [mscorlib]System.Object
{
  .method public hidebysig static void B(string[] args) cil managed
  {
    <em>.entrypoint</em>
    .maxstack  8
    IL_0000:  ldstr      "Hello World!"
    IL_0005:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_000a:  nop
    IL_000b:  ret
  }
}
</code></pre>

逆に .NET ランタイム的には `Task` 戻り値のエントリー ポイントを認めていなくて、
[C# 7.1 の非同期 `Main`](../cheatsheet/ap_ver7_1.md#async-Main) は、C# コンパイラーが以下のようなコードに相当する IL を生成しています。

<pre class="source" title="非同期メインから生成される実際のエントリー ポイント">
<code><span class="reserved">using</span> System.Threading.Tasks;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// C# 7.1 以降書ける「非同期 Main」。</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">Main</span>()
    {
    }
 
    <span class="comment">// 非同期 Main から C# コンパイラーが自動生成するメソッド。</span>
    <span class="comment">// これに .entrypoint ディレクティブが付く。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">&lt;Main&gt;</span>()
    {
        <span class="type">Main</span>().<span class="type">GetAwaiter</span>().<span class="type">GetResult</span>();
    }
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="startup-option"></a>複数の Main メソッドからエントリー ポイントを選択
C# で複数のクラスに `Main` メソッドを書くこともできますが、
素の状態ではコンパイル エラーになります。
(エラー内容は「複数のエントリー ポイントが定義されています」。)

<pre class="source" title="">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="error"><span class="method">Main</span></span>()
    {
    }
}
 
<span class="reserved">class</span> <span class="type">B</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
    }
}
</code></pre>

ただ、オプションによってこのうちのどれをエントリー ポイントにするかを指定する方法があります。
csc (C# コンパイラー)を直接呼び出す場合は `-main` オプションを、

<pre class="source" title="よくあるチュートリアル・テンプレート通りの C# コード">
<code>csc <em>-main:A</em>
</code></pre>

csproj (プロジェクト)に設定を書く場合は `StartupObject` タグでクラス名を指定します。

<pre class="xsource" title="">
<code><span class="attvalue">&lt;</span><span class="element">Project</span><span class="attvalue"> </span><span class="attribute">Sdk</span><span class="attvalue">=</span>&quot;<span class="attvalue">Microsoft.NET.Sdk</span>&quot;<span class="attvalue">&gt;</span>
 
<span class="attvalue">  &lt;</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>Exe<span class="attvalue">&lt;/</span><span class="element">OutputType</span><span class="attvalue">&gt;</span>
<span class="attvalue">    &lt;</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>net5.0<span class="attvalue">&lt;/</span><span class="element">TargetFramework</span><span class="attvalue">&gt;</span>
<em><span class="attvalue">    &lt;</span><span class="element">StartupObject</span><span class="attvalue">&gt;</span>A<span class="attvalue">&lt;/</span><span class="element">StartupObject</span><span class="attvalue">&gt;</span></em>
<span class="attvalue">  &lt;/</span><span class="element">PropertyGroup</span><span class="attvalue">&gt;</span>
 
<span class="attvalue">&lt;/</span><span class="element">Project</span><span class="attvalue">&gt;</span>
</code></pre>

この例の場合は、この書き方で、`A.Main` の方がエントリー ポイントになります。

##<a id="sec-generated-title-5"></a> <a id="top-level-statements"></a>トップ レベル ステートメント
<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 から、トップ レベル(top-leve: クラスや名前空間よりも外側、ファイル直下)に[ステートメント](../start/st_variable.md#statement)を直接書けるようになりました。

例えば前述の「Hello World」であれば、単に以下のように書けるようになります。

<pre class="source" title="トップ レベルに直接「Hello World」">
<code><span class="reserved">using</span> System;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
</code></pre>

この機能を<strong id="key-top-level-statements" class="keyword">トップ レベル ステートメント</strong>(top-level statements)と言います。

挙動としては、`Main`に相当するメソッドの自動生成になります。
上記の例の場合、以下のようなコードが生成された上で、`$Main` メソッドに `.entrypoint` が付きます。

<pre class="source" title="トップ レベル ステートメントから生成されるエントリー ポイント">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">&lt;Program&gt;$</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">$Main</span>(<span class="reserved">string</span>[] args)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
    }
}
</code></pre>

クラス名もメソッド名も、通常の C# コードでは定義できない・呼び出しできない名前で生成されます<sup>※</sup>。
名前も決まった名前にはなっていません(今現在の実装が `$Main` という名前で生成しているからといって、将来ずっとこの名前とは限らない)。

<sup>※</sup> [C# 10.0 以降は、クラス名に関しては `Program` という普通の名前に変更されました](../../../blog/2021/11/top-level-csharp10/index.md)。
メソッド名の方は `<Main>$` になっています。

###<a id="sec-generated-title-6"></a> <a id="top-level-statement-restriction"></a>ステートメントを書ける場所
トップ レベル ステートメントを書ける場所には少し制約があります。

- プロジェクト全体で1ファイルだけがトップ レベル ステートメントを持てる
- クラスや名前空間よりも上にだけトップ レベル ステートメントを書ける

要は、実行順序で迷いそうになったり、
不慮の事故で予定外の処理を足してしまったりすることがないように、
書ける場所を1か所に絞っています。

例えば以下のようなコードはコンパイル エラーになります。

<pre class="source" title="クラスよりも下にステートメントを書くことは認められていない">
<code><span class="reserved">using</span> System;
 
<span class="comment">// ここにステートメントを書くのは OK。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;above class&quot;</span>);
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">void</span> <span class="method">M</span>() { }
}
 
<span class="comment">// ここにステートメントを書くのはダメ。</span>
<span class="error"><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;below class&quot;</span>);</span>
</code></pre>

###<a id="sec-generated-title-7"></a> <a id="top-level-method"></a>トップ レベルにメソッド記述
トップ レベルにはメソッドを書くこともできます。
これは扱いとしては、生成される `Main` (相当の)メソッドのローカル関数になります。

例えば以下のようなコードを書いた場合、

<pre class="source" title="トップ レベルでメソッドを定義">
<code><span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> s) =&gt; System.<span class="type">Console</span>.<span class="method">WriteLine</span>(s);
 
<span class="method">m</span>(<span class="string">&quot;abc&quot;</span>);
<span class="method">m</span>(<span class="string">&quot;123&quot;</span>);
</code></pre>

コンパイラーが生成するコードは以下のような感じになります。

<pre class="source" title="トップ レベルのメソッドは、生成される Main メソッドのローカル関数扱い">
<code><span class="reserved">class</span> <span class="type">&lt;Program&gt;$</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">$Main</span>(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> s) =&gt; System.<span class="type">Console</span>.<span class="method">WriteLine</span>(s);
 
        <span class="method">m</span>(<span class="string">&quot;abc&quot;</span>);
        <span class="method">m</span>(<span class="string">&quot;123&quot;</span>);
    }
}
</code></pre>

ただ、定義したメソッドの名前はプロジェクト全域に影響を及ぼします。
以下のように、「メソッドがあることは全域で見えているけども、使ってはいけない」という扱いを受けます。

<pre class="source" title="トップ レベルのメソッドの扱い(名前は全域から見えてるけど、使っちゃダメ)">
<code><span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> s) =&gt; System.<span class="type">Console</span>.<span class="method">WriteLine</span>(s);
 
<span class="method">m</span>(<span class="string">&quot;abc&quot;</span>);
<span class="method">m</span>(<span class="string">&quot;123&quot;</span>);
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// ここはエラーになるものの、エラー内容は</span>
        <span class="comment">// 「m が見つからない」ではなく、</span>
        <span class="comment">// 「トップ レベルで定義した m をここから使うことはできない」になる。</span>
        <span class="method">m</span>(<span class="string">&quot;Program.M&quot;</span>);
    }
}
</code></pre>

(将来的に、トップ レベルで定義したメソッドを、ローカル関数扱いからグローバル関数(どこからでも参照できる静的メソッド)扱いに変更する可能性もなくはなく、その場合、C# 9.0 時点ではこの例のようなエラーにしておく方が将来の憂いがないみたいです。)

###<a id="sec-generated-title-8"></a> <a id="top-level-vs-script"></a>トップ レベル ステートメントとスクリプト実行
今の C# には[スクリプト実行用の文法](../cheatsheet/apscripting.md)もあったりするんですが、
それとトップ レベル ステートメントは微妙に仕様が違っていたりします。

スクリプト実行の場合にはクラスの後ろにもステートメントを書けます。
また、[`#r` や `#load` など](../cheatsheet/apscripting.md#directive)、一部のディレクティブはスクリプト実行専用です。
スクリプト実行では、`;` なしで式を書くと、その値をREPL(Read Eval Print Loop: 1行式を評価しては、即座にその値を画面に表示する)実行できたりします。

例えば、以下のコードはスクリプト実行では有効ですが、トップ レベル ステートメントとしてはコンパイル エラーになります。

<pre class="source" title="スクリプト実行でだけ有効な C# コード">
<code><span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X;
    <span class="reserved">public</span> <span class="reserved">int</span> Y;
}
 
<span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
 
p.X
p.Y
</code></pre>

一方で、スクリプト実行では名前空間を書けないので、例えば以下のコードはトップ レベル ステートメントでだけコンパイルできます。

<pre class="source" title="トップ レベル ステートメントでだけ有効な C# コード">
<code><span class="reserved">var</span> p = <span class="reserved">new</span> App1.<span class="type">Point</span> { X = 1, Y = 2 };
 
<span class="reserved">namespace</span> App1
{
    <span class="reserved">struct</span> <span class="type">Point</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> X;
        <span class="reserved">public</span> <span class="reserved">int</span> Y;
    }
}
</code></pre>

###<a id="sec-generated-title-9"></a> <a id="args-returns"></a>コマンドライン引数と戻り値
トップ レベル ステートメントを使う場合、暗黙的に `args` という名前の変数が定義されていて、
この変数にはコマンドライン引数(`Main` メソッドを書いた時、`string[]` 引数に入っているのと同じもの)が入っています。

また、トップ レベル ステートメントには `return` を書くことができますが、`int` 戻り値の `Main` メソッドと同じ意味になります(プログラムの終了コードになる)。

例えば以下のようなトップ レベル ステートメントを書けます。

<pre class="source" title="トップ レベル ステートメントにおけるコマンドライン引数と終了コード">
<code><span class="reserved">if</span> (args.Length == 0)
{
    System.<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;コマンドライン引数が必要です&quot;</span>);
    <span class="reserved">return</span> 1;
}
<span class="reserved">else</span>
{
    System.<span class="type">Console</span>.<span class="method">WriteLine</span>(args[0]);
    <span class="reserved">return</span> 0;
}
</code></pre>

このコードは以下のような意味で解釈されます。

<pre class="source" title="トップ レベル ステートメントから生成される Main メソッド">
<code><span class="reserved">class</span> <span class="type">&lt;Program&gt;$</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">$Main</span>(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">if</span> (args.Length == 0)
        {
            System.<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;コマンドライン引数が必要です&quot;</span>);
            <span class="reserved">return</span> 1;
        }
        <span class="reserved">else</span>
        {
            System.<span class="type">Console</span>.<span class="method">WriteLine</span>(args[0]);
            <span class="reserved">return</span> 0;
        }
    }
}
</code></pre>

`return` がない時には `void Main(string[] args)` で、あるときには `int Main(string[] args)` 相当のコードが生成されます。

ちなみに、トップ レベル ステートメント中には `await` を書けます。
`await` があるときに限って、`Task Main(string[] args)` 、 `Task<int> Main(string[] args)` 相当のコード生成になります。
