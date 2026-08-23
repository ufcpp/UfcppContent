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
aliases: []
---

# エントリー ポイント

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

実行可能なプログラムを書くとき、最初に呼び出される処理を<strong id="key-entry-point" class="keyword">エントリー ポイント</strong>(entry point: 入場地点、入り口)と言います。
C# の場合、通常、`Main` という名前の静的メソッドを1個だけ書くことで、このメソッドがエントリー ポイントになります。

また、複数の `Main` メソッドを書いてそのうちの1つをエントリー ポイントに選ぶ方法があったり、
C# 9.0 からはトップ レベル ステートメントという書き方でエントリー ポイントを作れたりします。

本項ではこの C# のエントリー ポイントに関する仕様について説明します。

## <a id="sec-generated-title-2"></a> <a id="entry-point-in-csharp"></a>C# のエントリー ポイント

C# 関連のチュートリアルでのサンプル コードや、
テンプレート通りに C# プログラムを新規作成すると以下のような内容になっていることが多いと思います。

```csharp {title="よくあるチュートリアル・テンプレート通りの C# コード"}
using System;
 
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```

C# の仕様上、実行可能プログラムを C# で書きたい場合、どこかに1つ、`Main` という名前のメソッドが必要です。
(後述しますが、C# 9.0 からは別の方法も追加されました。)

名前空間は必須ではありません。クラス名も何でも構いません。
例えば以下のようなコードでも、`Main` メソッドがエントリー ポイントになります。

```csharp {title="名前空間はなくてもいい。クラス名も任意"}
using System;
 
class X
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}
```

通常、エントリー ポイントとして使うためには、`Main` メソッドに以下のような制限があります。

- [静的](../oop/oo_static.md)である(`static` 修飾子が付いてる)必要がある
- 引数はなしか、`string[]` のどちらか
- 戻り値はなし(`void`)か、`int` のどちらか
  - [C# 7.1](../cheatsheet/ap_ver7_1.md#async-Main) 以降は追加で `Task` か `Task<int>` も OK

つまり、C# 7.0 以前だと以下の4つのうちのいずれかが、

```csharp {title="エントリー ポイントとして許される Main メソッドの書き方"}
static void Main()
static void Main(string[] args)
static int Main()
static int Main(string[] args)
```

加えて、C# 7.1 以降だと以下の4つのうちのいずれかが認められます。

```csharp {title="エントリー ポイントとして許される Main メソッドの書き方 (C# 7.1 以降)"}
using System.Threading.Tasks;
 
static Task Main()
static Task Main(string[] args)
static Task<int> Main()
static Task<int> Main(string[] args)
```

## <a id="sec-generated-title-3"></a> <a id="entry-point-in-dotnet"></a>.NET のエントリー ポイント

前述の `Main` という名前が必須なのは C# の仕様上の話で、
その下層、 .NET ランタイムにはそういう制限はありません。
`.entrypoint` ディレクティブで修飾したメソッドがエントリー ポイントになります。

例えば、以下のような .NET IL アセンブラー コードを書けば、`A` というクラス内の `B` というメソッドをエントリー ポイントにできます。

```cil {highlight-text=".entrypoint"}
.class public auto ansi beforefieldinit A
       extends [mscorlib]System.Object
{
  .method public hidebysig static void B(string[] args) cil managed
  {
    .entrypoint
    .maxstack  8
    IL_0000:  ldstr      "Hello World!"
    IL_0005:  call       void [mscorlib]System.Console::WriteLine(string)
    IL_000a:  nop
    IL_000b:  ret
  }
}
```

逆に .NET ランタイム的には `Task` 戻り値のエントリー ポイントを認めていなくて、
[C# 7.1 の非同期 `Main`](../cheatsheet/ap_ver7_1.md#async-Main) は、C# コンパイラーが以下のようなコードに相当する IL を生成しています。

```csharp {title="非同期メインから生成される実際のエントリー ポイント"}
using System.Threading.Tasks;
 
class Program
{
    // C# 7.1 以降書ける「非同期 Main」。
    static async Task Main()
    {
    }
 
    // 非同期 Main から C# コンパイラーが自動生成するメソッド。
    // これに .entrypoint ディレクティブが付く。
    static void <Main>()
    {
        Main().GetAwaiter().GetResult();
    }
}
```

## <a id="sec-generated-title-4"></a> <a id="startup-option"></a>複数の Main メソッドからエントリー ポイントを選択

C# で複数のクラスに `Main` メソッドを書くこともできますが、
素の状態ではコンパイル エラーになります。
(エラー内容は「複数のエントリー ポイントが定義されています」。)

```csharp
class A
{
    static void Main()
    {
    }
}
 
class B
{
    static void Main()
    {
    }
}
```

ただ、オプションによってこのうちのどれをエントリー ポイントにするかを指定する方法があります。
csc (C# コンパイラー)を直接呼び出す場合は `-main` オプションを、

```shell {title="よくあるチュートリアル・テンプレート通りの C# コード" highlight-text="-main:A"}
csc -main:A
```

csproj (プロジェクト)に設定を書く場合は `StartupObject` タグでクラス名を指定します。

```xml {highlight-lines="6"}
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <StartupObject>A</StartupObject>
  </PropertyGroup>
 
</Project>
```

この例の場合は、この書き方で、`A.Main` の方がエントリー ポイントになります。

## <a id="sec-generated-title-5"></a> <a id="top-level-statements"></a>トップ レベル ステートメント

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 から、トップ レベル(top-leve: クラスや名前空間よりも外側、ファイル直下)に[ステートメント](../start/st_variable.md#statement)を直接書けるようになりました。

例えば前述の「Hello World」であれば、単に以下のように書けるようになります。

```csharp {title="トップ レベルに直接「Hello World」"}
using System;
Console.WriteLine("Hello World!");
```

この機能を<strong id="key-top-level-statements" class="keyword">トップ レベル ステートメント</strong>(top-level statements)と言います。

挙動としては、`Main`に相当するメソッドの自動生成になります。
上記の例の場合、以下のようなコードが生成された上で、`$Main` メソッドに `.entrypoint` が付きます。

```csharp {title="トップ レベル ステートメントから生成されるエントリー ポイント"}
using System;
 
class <Program>$
{
    static void $Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}
```

クラス名もメソッド名も、通常の C# コードでは定義できない・呼び出しできない名前で生成されます<sup>※</sup>。
名前も決まった名前にはなっていません(今現在の実装が `$Main` という名前で生成しているからといって、将来ずっとこの名前とは限らない)。

<sup>※</sup> [C# 10.0 以降は、クラス名に関しては `Program` という普通の名前に変更されました](../../../blog/2021/11/top-level-csharp10/index.md)。
メソッド名の方は `<Main>$` になっています。

### <a id="sec-generated-title-6"></a> <a id="top-level-statement-restriction"></a>ステートメントを書ける場所

トップ レベル ステートメントを書ける場所には少し制約があります。

- プロジェクト全体で1ファイルだけがトップ レベル ステートメントを持てる
- クラスや名前空間よりも上にだけトップ レベル ステートメントを書ける

要は、実行順序で迷いそうになったり、
不慮の事故で予定外の処理を足してしまったりすることがないように、
書ける場所を1か所に絞っています。

例えば以下のようなコードはコンパイル エラーになります。

```csharp {title="クラスよりも下にステートメントを書くことは認められていない"}
using System;
 
// ここにステートメントを書くのは OK。
Console.WriteLine("above class");
 
class Program
{
    void M() { }
}
 
// ここにステートメントを書くのはダメ。
Console.WriteLine("below class");
```

### <a id="sec-generated-title-7"></a> <a id="top-level-method"></a>トップ レベルにメソッド記述

トップ レベルにはメソッドを書くこともできます。
これは扱いとしては、生成される `Main` (相当の)メソッドのローカル関数になります。

例えば以下のようなコードを書いた場合、

```csharp {title="トップ レベルでメソッドを定義"}
void m(string s) => System.Console.WriteLine(s);
 
m("abc");
m("123");
```

コンパイラーが生成するコードは以下のような感じになります。

```csharp {title="トップ レベルのメソッドは、生成される Main メソッドのローカル関数扱い"}
class <Program>$
{
    static void $Main(string[] args)
    {
        void m(string s) => System.Console.WriteLine(s);
 
        m("abc");
        m("123");
    }
}
```

ただ、定義したメソッドの名前はプロジェクト全域に影響を及ぼします。
以下のように、「メソッドがあることは全域で見えているけども、使ってはいけない」という扱いを受けます。

```csharp {title="トップ レベルのメソッドの扱い(名前は全域から見えてるけど、使っちゃダメ)"}
void m(string s) => System.Console.WriteLine(s);
 
m("abc");
m("123");
 
class Program
{
    static void M()
    {
        // ここはエラーになるものの、エラー内容は
        // 「m が見つからない」ではなく、
        // 「トップ レベルで定義した m をここから使うことはできない」になる。
        m("Program.M");
    }
}
```

(将来的に、トップ レベルで定義したメソッドを、ローカル関数扱いからグローバル関数(どこからでも参照できる静的メソッド)扱いに変更する可能性もなくはなく、その場合、C# 9.0 時点ではこの例のようなエラーにしておく方が将来の憂いがないみたいです。)

### <a id="sec-generated-title-8"></a> <a id="top-level-vs-script"></a>トップ レベル ステートメントとスクリプト実行

今の C# には[スクリプト実行用の文法](../cheatsheet/apscripting.md)もあったりするんですが、
それとトップ レベル ステートメントは微妙に仕様が違っていたりします。

スクリプト実行の場合にはクラスの後ろにもステートメントを書けます。
また、[`#r` や `#load` など](../cheatsheet/apscripting.md#directive)、一部のディレクティブはスクリプト実行専用です。
スクリプト実行では、`;` なしで式を書くと、その値をREPL(Read Eval Print Loop: 1行式を評価しては、即座にその値を画面に表示する)実行できたりします。

例えば、以下のコードはスクリプト実行では有効ですが、トップ レベル ステートメントとしてはコンパイル エラーになります。

```csharp {title="スクリプト実行でだけ有効な C# コード"}
struct Point
{
    public int X;
    public int Y;
}
 
var p = new Point { X = 1, Y = 2 };
 
p.X
p.Y
```

一方で、スクリプト実行では名前空間を書けないので、例えば以下のコードはトップ レベル ステートメントでだけコンパイルできます。

```csharp {title="トップ レベル ステートメントでだけ有効な C# コード"}
var p = new App1.Point { X = 1, Y = 2 };
 
namespace App1
{
    struct Point
    {
        public int X;
        public int Y;
    }
}
```

### <a id="sec-generated-title-9"></a> <a id="args-returns"></a>コマンドライン引数と戻り値

トップ レベル ステートメントを使う場合、暗黙的に `args` という名前の変数が定義されていて、
この変数にはコマンドライン引数(`Main` メソッドを書いた時、`string[]` 引数に入っているのと同じもの)が入っています。

また、トップ レベル ステートメントには `return` を書くことができますが、`int` 戻り値の `Main` メソッドと同じ意味になります(プログラムの終了コードになる)。

例えば以下のようなトップ レベル ステートメントを書けます。

```csharp {title="トップ レベル ステートメントにおけるコマンドライン引数と終了コード"}
if (args.Length == 0)
{
    System.Console.WriteLine("コマンドライン引数が必要です");
    return 1;
}
else
{
    System.Console.WriteLine(args[0]);
    return 0;
}
```

このコードは以下のような意味で解釈されます。

```csharp {title="トップ レベル ステートメントから生成される Main メソッド"}
class <Program>$
{
    static int $Main(string[] args)
    {
        if (args.Length == 0)
        {
            System.Console.WriteLine("コマンドライン引数が必要です");
            return 1;
        }
        else
        {
            System.Console.WriteLine(args[0]);
            return 0;
        }
    }
}
```

`return` がない時には `void Main(string[] args)` で、あるときには `int Main(string[] args)` 相当のコードが生成されます。

ちなみに、トップ レベル ステートメント中には `await` を書けます。
`await` があるときに限って、`Task Main(string[] args)` 、 `Task<int> Main(string[] args)` 相当のコード生成になります。
