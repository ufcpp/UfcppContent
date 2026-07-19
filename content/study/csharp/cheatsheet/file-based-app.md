---
title: "ファイル ベース実行"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/file-based-app/"
content_type: "Article"
published_at: "2025-10-11T16:20:45"
updated_at: "2025-10-11T16:20:45"
tags: []
umbraco_id: 2515
parent_id: 1174
sort_order: 24
aliases:
  - "/csharp/cheatsheet/file-based-app/"
---

# ファイル ベース実行

# <a id="sec-generated-title-1"></a>ファイル ベース実行
##<a id="sec-generated-title-2"></a> <a id="abstract">概要</a>
.NET 10 (C# 14 と同世代)で単独の `.cs` ファイルだけで C# プログラムを実行できるようになりました。
例えば、`app1.cs` という名前で保存した C# ファイルを `dotnet app1.cs` という1コマンドだけで実行できます。
それに伴って、C# 14 で `#!` と `#:` (無視ディレクティブ)という機能が追加されています。

(C# 言語の新文法というよりは、C# コンパイラーの1機能という感じのものです。
バージョン的にも C# 14 である必要はなくて、.NET 10 以降付属の C# コンパイラーであれば言語バージョン問わず `#!` と `#:` を認識します。)

本項ではこの「単独のファイルでの実行」(ファイル ベース実行)の話と、
C# 14 の `#!` と `#:` (無視ディレクティブ)について説明します。

サンプル コード: [FileBaseApp](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/FileBaseApp)

##<a id="sec-generated-title-3"></a> <a id="file-based-app">ファイル ベース実行</a>
改めて、 .NET 10 で C# ファイルを直接1コマンドで実行できるようになりました。
例えば、以下の1行だけ書いたファイル `app1.cs` を用意して、

<pre class="source" title="1行だけの .cs ファイル">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;🐈&quot;</span>);
</pre>

以下のようなコマンドを打つと、この C# ファイルを単独で実行できます。

<pre class="source" title="app1.cs ファイルを直接実行する">
<span class="prompt">&gt;</span> dotnet app1.cs
🐈
</pre>

これは[スクリプト実行](apscripting.md)ではなく、通常の<sup>[※脚注](#non-scripting)</sup> C# 実行になります。
この仕組みをファイル ベース実行(file-based execution) と言い、これを使って書かれた C# プログラムをファイル ベース アプリ(file-based app)と言ったりします。

この機能の追加に伴い、これまでであれば[プロジェクト](../devenv/vs_project.md) (実体は拡張子 `.csproj` の XML ファイル)に書いていた設定の類を C# 中に直接書けるようになりました。
以下の2つが追加されています。

* `#!` : いわゆる [shebang](https://ja.wikipedia.org/wiki/%E3%82%B7%E3%83%90%E3%83%B3_(Unix))
* `#:` : プロジェクト設定の類を書くための[ディレクティブ](../misc/sp_preprocess.md#preprocess)で、C# コンパイラーにとっては「単に無視」になる

ちなみに、普通に `.csproj` ファイルを使って C# プロジェクトをコンパイルする際には、`#!` や `#:` があるとコンパイル エラーになります。
ただし、`.csproj` ファイル中に `<Features>FileBasedProgram</Features>` オプションを書いておくとコンパイルでき、この場合、`#!` や `#:` から始まる行は単に無視されます。
C# コンパイラーからすると「単に無視するもの」なので、無視ディレクティブ(ignored directive)と呼ばれます。

<sup><a id="non-scripting">※</a></sup> スクリプト実行が「それ専用の構文がいくつかある」状態なのに対して、
ファイル ベース実行は本当に普通の C# です。
スクリプト実行みたいに「1行1行追加で実行」みたいなことができない一方で、
「コードが多くなってきたから `.csproj` 形式の通常の C# プロジェクトに切り替えたい」というときにスムーズに移行できます。
移行を自動化するための `dotnet project convert` というコマンドも用意されています。


##<a id="sec-generated-title-4"></a> <a id="shebang">shebang</a>
`#!` (通称 shebang。 sharp + bang が由来)は主に Unix のスクリプト言語で使われるもので、
ソースコードの先頭にこの記号から始まる行を入れると「何を使ってこのスクリプトを実行するか」を指定できます。

C# 14 で、C# にもこの1行を入れることができるようになりました。
例えば前節の `app1.cs` ファイルにちょっと手を加えて以下のような内容にします。

<pre class="source" title="shebang 入り .cs ファイル">
<span class="comment">#!/usr/bin/env dotnet</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;🐈&quot;</span>);
</pre>

このファイルは [bash](https://ja.wikipedia.org/wiki/Bash) などの Unix 系シェルで `./app1.cs` みたいに直接実行できるようになります。
(実行権限が必要なので、最初に1回 `chmod +x` などの操作が必要。)

<pre class="source" title="bash 上で app1.cs を直接実行する">
<span class="prompt">$</span> ls
app1.cs
<span class="prompt">$</span> chmod +x app1.cs
<span class="prompt">$</span> ./app1.cs
🐈
</pre>

用途的に、`#!` はファイルの先頭にのみ書けます。
`#!` の前には改行はもちろんのこと、空白文字や [BOM](https://ja.wikipedia.org/wiki/%E3%83%90%E3%82%A4%E3%83%88%E9%A0%86%E3%83%9E%E3%83%BC%E3%82%AF) を入れることもできません。

##<a id="sec-generated-title-5"></a> <a id="ignored-directive">: 無視ディレクティブ</a>
`#:` から始まる行は `dotnet` コマンドがプロジェクト設定として解釈するために使い、
C# 上は単に無視されます。


例えば、以下のような `.cs` ファイルをファイル ベース実行するのは、

<pre class="source" title="#: を使ったファイル ベース実行の例">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">property</span><span class="string"> InvariantGlobalization=true</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">new</span> <span class="type struct">DateTime</span>(<span class="number">2000</span>, <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>));
</pre>

以下のような2ファイルを使って既存の `.csproj` ベースの `dotnet run` をするのとほぼ同じ意味になります。

`app1.csproj`:

<pre class="xml" title="既存の .csproj ベース実行の例(app1.csproj)">
&lt;Project Sdk=&quot;Microsoft.NET.Sdk&quot;&gt;
    &lt;PropertyGroup&gt;
    &lt;OutputType&gt;Exe&lt;/OutputType&gt;
    &lt;TargetFramework&gt;net10.0&lt;/TargetFramework&gt;
    &lt;ImplicitUsings&gt;enable&lt;/ImplicitUsings&gt;
    &lt;Nullable&gt;enable&lt;/Nullable&gt;
    &lt;/PropertyGroup&gt;
    <em>&lt;InvariantGlobalization&gt;true&lt;/InvariantGlobalization&gt;</em>
&lt;/Project&gt;
</pre>

`app1.cs`:

<pre class="source" title="既存の .csproj ベース実行の例(app1.cs)">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">new</span> <span class="type struct">DateTime</span>(<span class="number">2000</span>, <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span>));
</pre>

(ちなみに `InvariantGlobalization` を指定すると書式が北米フォーマットになるので、出力される結果は `01/02/2000 03:04:05` (MM/dd/yyyy)になります。)

`#:` で始まる無視ディレクティブは [shebang](#shebang) とコメントを除いて、ファイルの先頭に置く必要があります。
例えば以下のコードでは、5行目(`LangVersion` の行)は問題なく、
9行目(`ImplicitUsings` の行)でだけコンパイル エラーを起こします。

<pre class="source" title="">
<span class="comment">#!/usr/bin/env dotnet</span>

<span class="comment">// コメントはあってもいい。</span>

<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">property</span><span class="string"> LangVersion=13</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;🐈&quot;</span>);

<span class="preprocess">#</span><span class="preprocess"><span class="error" title="CS9297">:</span></span><span class="preprocess">property</span><span class="string"> ImplicitUsings=disable</span>
</pre>

.NET 10 時点で、`dotnet` コマンドは以下のディレクティブを解釈できます。

| ディレクティブ | 意味 | `.csproj` での書き方 |
| --- | --- | --- |
| `#:sdk` | [プロジェクト SDK](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/overview) を指定 | `<Project Sdk="これ">` |
| `#:property` | [プロパティ要素](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/propertygroup-element-msbuild) | `<PropertyGroup>` の子要素 |
| `#:package` | [パッケージ参照](https://learn.microsoft.com/ja-jp/nuget/consume-packages/package-references-in-project-files) | `<PackageReference>` 要素 |
| `#:project` | [プロジェクト参照](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/common-msbuild-project-items#projectreference) | `<ProjectReference>` 要素 |

###<a id="sec-generated-title-6"></a> <a id="sdk-directive">sdk ディレクティブ</a>
`#:sdk` は、 `.csproj` では `<Project Sdk="Identifier">` と書いていたものです。
省略した場合は `Microsoft.NET.Sdk` (ライブラリやコンソール プログラムで使う一番シンブルな SDK)になります。 
実質的には「ASP.NET プログラムを書きたいときに `Microsoft.NET.Sdk.Web` にするもの」です。

例えば、以下のようなコードで ASP.NET なコードをファイル ベース実行できます。

<pre class="source" title="ファイル ベース ASP.NET コード">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">sdk</span><span class="string"> Microsoft.NET.Sdk.Web</span>

<span class="reserved">var</span> <span class="variable">app</span> <span class="operator">=</span> <span class="type">WebApplication</span><span class="operator">.</span>CreateBuilder(<span class="reserved">args</span>)<span class="operator">.</span>Build();
<span class="variable">app</span><span class="operator">.</span>MapGet(<span class="string">&quot;/&quot;</span>, () <span class="operator">=&gt;</span> <span class="string">&quot;Hello World!&quot;</span>);
<span class="variable">app</span><span class="operator">.</span>Run();
</pre>

###<a id="sec-generated-title-7"></a> <a id="property-directive">property ディレクティブ</a>
`#:property` は、 `.csproj` では `<PropertyGroup>` の子要素として書いていたものです。
`.csproj` の `<Tag>Value</Tag>` 要素が `#:property Tag=Value` という書き方になります。

[無視ディレクティブの節](#ignored-directive)の冒頭の `InvariantGlobalization` の例もこれになります。
その他、例えば [unsafe ブロック](../interop/sp_unsafe.md)はオプションを指定しないと使えない構文なわけですが、以下のように書くことでそのオプションを指定できます。

<pre class="source" title="AllowUnsafeBlocks=true">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">property</span><span class="string"> AllowUnsafeBlocks=true</span>

<span class="comment">// unsafe ブロックはオプションをつけないと使えない構文。</span>
<span class="reserved">unsafe</span>
{
    <span class="reserved">int</span> <span class="variable">n</span> <span class="operator">=</span> <span class="number">1</span>;
    <span class="reserved">int</span><span class="operator">*</span> <span class="variable">pn</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="variable">n</span>;
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span>{(<span class="reserved">nint</span>)<span class="variable">pn</span>:<span class="string">x</span>}<span class="string">&quot;</span>);
}
</pre>

###<a id="sec-generated-title-8"></a> <a id="package-directive">package ディレクティブ</a>
`#:package` は、 `.csproj` では `<PackageReference>` 要素で書いていたものです。
`.csproj` の `<PackageReference Include="PackageName" Version="x.y.z" />` 要素が `#:package PackageName@x.y.z` という書き方になります。

例として `Microsoft.CodeAnalysis.CSharp` パッケージ(C# 中から C# コンパイラー自身を呼ぶためのライブラリ)を参照したコードを書くと以下のようになります。
(ちなみに、4.14.0 は C# 13 当時のバージョンです。)

<pre class="source" title="Microsoft.CodeAnalysis.CSharp パッケージを参照する例">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">package</span><span class="string"> Microsoft.CodeAnalysis.CSharp@4.14.0</span>

<span class="reserved">using</span> Microsoft<span class="operator">.</span>CodeAnalysis<span class="operator">.</span>CSharp;

<span class="reserved">var</span> <span class="variable">tree</span> <span class="operator">=</span> <span class="type">CSharpSyntaxTree</span><span class="operator">.</span><span class="method"><span class="static">ParseText</span></span>(<span class="string">&quot;class Class1;&quot;</span>);
<span class="reserved">var</span> <span class="variable">root</span> <span class="operator">=</span> <span class="control">await</span> <span class="variable">tree</span><span class="operator">.</span><span class="method">GetRootAsync</span>();
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">root</span><span class="operator">.</span><span class="method">GetFirstToken</span>()<span class="operator">.</span><span class="property">Text</span>);
</pre>

###<a id="sec-generated-title-9"></a> <a id="project-directive">project ディレクティブ</a>
`#:project` は、 `.csproj` では `<ProjectReference>` 要素で書いていたものです。
`.csproj` の `<ProjectReference Include="path" />` 要素が `#:project path` という書き方になります。

例えば以下のような書き方で、`.cs` のある場所からの相対パスで `Lib/Lib.csproj` プロジェクトを参照できます。

<pre class="source" title="プロジェクトを参照する例">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">project</span><span class="string"> Lib/Lib.csproj</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(Lib<span class="operator">.</span><span class="type">Class1</span><span class="operator">.</span><span class="property"><span class="static">Name</span></span>);
</pre>

###<a id="sec-generated-title-10"></a> <a id="unknown-directive">未対応のディレクティブ</a>
未対応の `#:` ディレクティブは、ファイル ベース実行するとエラーを起こします。
例えば以下のようなコードを書いて `dotnet app1.cs` コマンド実行すると、
「認識されないディレクティブ ' aaa' です。」というエラーが出ます。

<pre class="source" title="わざと変な無視ディレクティブを書いた例">
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">aaa</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;🐈&quot;</span>);
</pre>

ちなみにこのエラーを出すのはあくまで `dotnet` コマンドであって、
C# コンパイラー的には「`#:` で始まるディレクティブはすべて無視」という挙動になっています。
`<Features>FileBasedProgram</Features>` オプションを書いた `.csproj` ファイルを用意して、
旧来方式でコンパイルすると `#:aaa` の行のエラーは出ません。
