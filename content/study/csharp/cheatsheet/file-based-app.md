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
aliases: []
---

# ファイル ベース実行

# <a id="sec-generated-title-1"></a>ファイル ベース実行

## <a id="sec-generated-title-2"></a> <a id="abstract">概要</a>

.NET 10 (C# 14 と同世代)で単独の `.cs` ファイルだけで C# プログラムを実行できるようになりました。
例えば、`app1.cs` という名前で保存した C# ファイルを `dotnet app1.cs` という1コマンドだけで実行できます。
それに伴って、C# 14 で `#!` と `#:` (無視ディレクティブ)という機能が追加されています。

(C# 言語の新文法というよりは、C# コンパイラーの1機能という感じのものです。
バージョン的にも C# 14 である必要はなくて、.NET 10 以降付属の C# コンパイラーであれば言語バージョン問わず `#!` と `#:` を認識します。)

本項ではこの「単独のファイルでの実行」(ファイル ベース実行)の話と、
C# 14 の `#!` と `#:` (無視ディレクティブ)について説明します。

サンプル コード: [FileBaseApp](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/FileBaseApp)

## <a id="sec-generated-title-3"></a> <a id="file-based-app">ファイル ベース実行</a>

改めて、 .NET 10 で C# ファイルを直接1コマンドで実行できるようになりました。
例えば、以下の1行だけ書いたファイル `app1.cs` を用意して、

```csharp {title="1行だけの .cs ファイル"}
Console.WriteLine("🐈");
```

以下のようなコマンドを打つと、この C# ファイルを単独で実行できます。

```console {title="app1.cs ファイルを直接実行する"}
> dotnet app1.cs
🐈
```

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


## <a id="sec-generated-title-4"></a> <a id="shebang">shebang</a>

`#!` (通称 shebang。 sharp + bang が由来)は主に Unix のスクリプト言語で使われるもので、
ソースコードの先頭にこの記号から始まる行を入れると「何を使ってこのスクリプトを実行するか」を指定できます。

C# 14 で、C# にもこの1行を入れることができるようになりました。
例えば前節の `app1.cs` ファイルにちょっと手を加えて以下のような内容にします。

```csharp {title="shebang 入り .cs ファイル"}
#!/usr/bin/env dotnet
Console.WriteLine("🐈");
```

このファイルは [bash](https://ja.wikipedia.org/wiki/Bash) などの Unix 系シェルで `./app1.cs` みたいに直接実行できるようになります。
(実行権限が必要なので、最初に1回 `chmod +x` などの操作が必要。)

```console {title="bash 上で app1.cs を直接実行する"}
$ ls
app1.cs
$ chmod +x app1.cs
$ ./app1.cs
🐈
```

用途的に、`#!` はファイルの先頭にのみ書けます。
`#!` の前には改行はもちろんのこと、空白文字や [BOM](https://ja.wikipedia.org/wiki/%E3%83%90%E3%82%A4%E3%83%88%E9%A0%86%E3%83%9E%E3%83%BC%E3%82%AF) を入れることもできません。

## <a id="sec-generated-title-5"></a> <a id="ignored-directive">: 無視ディレクティブ</a>

`#:` から始まる行は `dotnet` コマンドがプロジェクト設定として解釈するために使い、
C# 上は単に無視されます。


例えば、以下のような `.cs` ファイルをファイル ベース実行するのは、

```csharp {title="#: を使ったファイル ベース実行の例"}
#:property InvariantGlobalization=true
Console.WriteLine(new DateTime(2000, 1, 2, 3, 4, 5));
```

以下のような2ファイルを使って既存の `.csproj` ベースの `dotnet run` をするのとほぼ同じ意味になります。

`app1.csproj`:

```xml {title="既存の .csproj ベース実行の例(app1.csproj)" highlight-text="&lt;InvariantGlobalization&gt;true&lt;/InvariantGlobalization&gt;"}
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    </PropertyGroup>
    <InvariantGlobalization>true</InvariantGlobalization>
</Project>
```

`app1.cs`:

```xml {title="既存の .csproj ベース実行の例(app1.cs)"}
Console.WriteLine(new DateTime(2000, 1, 2, 3, 4, 5));
```

(ちなみに `InvariantGlobalization` を指定すると書式が北米フォーマットになるので、出力される結果は `01/02/2000 03:04:05` (MM/dd/yyyy)になります。)

`#:` で始まる無視ディレクティブは [shebang](#shebang) とコメントを除いて、ファイルの先頭に置く必要があります。
例えば以下のコードでは、5行目(`LangVersion` の行)は問題なく、
9行目(`ImplicitUsings` の行)でだけコンパイル エラーを起こします。

```csharp {error-ranges="sha256:0f676e14a6df2a426f564b2d53fc5dee7b671f4734a76fbf0ba433d11c4b0bb5;9:2-9:3" error-diagnostics="sha256:0f676e14a6df2a426f564b2d53fc5dee7b671f4734a76fbf0ba433d11c4b0bb5;CS9297@9:2-9:3"}
#!/usr/bin/env dotnet

// コメントはあってもいい。

#:property LangVersion=13

Console.WriteLine("🐈");

#:property ImplicitUsings=disable
```

.NET 10 時点で、`dotnet` コマンドは以下のディレクティブを解釈できます。

| ディレクティブ | 意味 | `.csproj` での書き方 |
| --- | --- | --- |
| `#:sdk` | [プロジェクト SDK](https://learn.microsoft.com/ja-jp/dotnet/core/project-sdk/overview) を指定 | `<Project Sdk="これ">` |
| `#:property` | [プロパティ要素](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/propertygroup-element-msbuild) | `<PropertyGroup>` の子要素 |
| `#:package` | [パッケージ参照](https://learn.microsoft.com/ja-jp/nuget/consume-packages/package-references-in-project-files) | `<PackageReference>` 要素 |
| `#:project` | [プロジェクト参照](https://learn.microsoft.com/ja-jp/visualstudio/msbuild/common-msbuild-project-items#projectreference) | `<ProjectReference>` 要素 |

### <a id="sec-generated-title-6"></a> <a id="sdk-directive">sdk ディレクティブ</a>

`#:sdk` は、 `.csproj` では `<Project Sdk="Identifier">` と書いていたものです。
省略した場合は `Microsoft.NET.Sdk` (ライブラリやコンソール プログラムで使う一番シンブルな SDK)になります。 
実質的には「ASP.NET プログラムを書きたいときに `Microsoft.NET.Sdk.Web` にするもの」です。

例えば、以下のようなコードで ASP.NET なコードをファイル ベース実行できます。

```csharp {title="ファイル ベース ASP.NET コード"}
#:sdk Microsoft.NET.Sdk.Web

var app = WebApplication.CreateBuilder(args).Build();
app.MapGet("/", () => "Hello World!");
app.Run();
```

### <a id="sec-generated-title-7"></a> <a id="property-directive">property ディレクティブ</a>

`#:property` は、 `.csproj` では `<PropertyGroup>` の子要素として書いていたものです。
`.csproj` の `<Tag>Value</Tag>` 要素が `#:property Tag=Value` という書き方になります。

[無視ディレクティブの節](#ignored-directive)の冒頭の `InvariantGlobalization` の例もこれになります。
その他、例えば [unsafe ブロック](../interop/sp_unsafe.md)はオプションを指定しないと使えない構文なわけですが、以下のように書くことでそのオプションを指定できます。

```csharp {title="AllowUnsafeBlocks=true"}
#:property AllowUnsafeBlocks=true

// unsafe ブロックはオプションをつけないと使えない構文。
unsafe
{
    int n = 1;
    int* pn = &n;
    Console.WriteLine($"{(nint)pn:x}");
}
```

### <a id="sec-generated-title-8"></a> <a id="package-directive">package ディレクティブ</a>

`#:package` は、 `.csproj` では `<PackageReference>` 要素で書いていたものです。
`.csproj` の `<PackageReference Include="PackageName" Version="x.y.z" />` 要素が `#:package PackageName@x.y.z` という書き方になります。

例として `Microsoft.CodeAnalysis.CSharp` パッケージ(C# 中から C# コンパイラー自身を呼ぶためのライブラリ)を参照したコードを書くと以下のようになります。
(ちなみに、4.14.0 は C# 13 当時のバージョンです。)

```csharp {title="Microsoft.CodeAnalysis.CSharp パッケージを参照する例"}
#:package Microsoft.CodeAnalysis.CSharp@4.14.0

using Microsoft.CodeAnalysis.CSharp;

var tree = CSharpSyntaxTree.ParseText("class Class1;");
var root = await tree.GetRootAsync();
Console.WriteLine(root.GetFirstToken().Text);
```

### <a id="sec-generated-title-9"></a> <a id="project-directive">project ディレクティブ</a>

`#:project` は、 `.csproj` では `<ProjectReference>` 要素で書いていたものです。
`.csproj` の `<ProjectReference Include="path" />` 要素が `#:project path` という書き方になります。

例えば以下のような書き方で、`.cs` のある場所からの相対パスで `Lib/Lib.csproj` プロジェクトを参照できます。

```csharp {title="プロジェクトを参照する例"}
#:project Lib/Lib.csproj
Console.WriteLine(Lib.Class1.Name);
```

### <a id="sec-generated-title-10"></a> <a id="unknown-directive">未対応のディレクティブ</a>

未対応の `#:` ディレクティブは、ファイル ベース実行するとエラーを起こします。
例えば以下のようなコードを書いて `dotnet app1.cs` コマンド実行すると、
「認識されないディレクティブ ' aaa' です。」というエラーが出ます。

```csharp {title="わざと変な無視ディレクティブを書いた例"}
#:aaa
Console.WriteLine("🐈");
```

ちなみにこのエラーを出すのはあくまで `dotnet` コマンドであって、
C# コンパイラー的には「`#:` で始まるディレクティブはすべて無視」という挙動になっています。
`<Features>FileBasedProgram</Features>` オプションを書いた `.csproj` ファイルを用意して、
旧来方式でコンパイルすると `#:aaa` の行のエラーは出ません。
