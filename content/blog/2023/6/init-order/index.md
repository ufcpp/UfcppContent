---
title: "モジュール初期化子が呼ばれる順"
source_url: "https://ufcpp.net/blog/2023/6/init-order/"
content_type: "BlogEntry"
published_at: "2023-06-24T21:57:33"
updated_at: "2023-06-24T21:57:33"
tags: []
umbraco_id: 2468
parent_id: 2463
sort_order: 1
aliases: []
---

# モジュール初期化子が呼ばれる順

[前回のブログ](../ca-specify-culture/index.md)に続き、
[先日の C# 配信](https://www.youtube.com/watch?v=M5weHOCzJ6E)で出てたネタ。

まあ、今回のは知ったところで誰が助かるということもないようなトリビア的な話です。

## 後だし優先で上書き

その C# 配信内で、
「[`CultureInfo.DefaultThreadCurrentCulture` を上書きすれば対処はできるけども](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/73#issuecomment-1595764176)」
みたいな話が出まして。

ただ、まあ、こういう「グローバルに影響がある静的プロパティの書き換え」は決してお行儀はよくないじゃないですか。
誰でも、いつでも上書き可能。
すぐに競合しかねません。

例えばここで出した
`DefaultThreadCurrentCulture` だと、
人それぞれ、以下のようなバラバラの主張が混ざったとします。

* 常に `InvariantCulture` にしたい
* 常に ja-jp カルチャーにしたい
* 基本、`InvariantCulture` でいいものの、日付の書式だけは `MM-dd-yyyy` が許せないので上書きする

そしてしかも、これを「ソース ジェネレーターとかを使って裏でこっそり書き換えておきたい」とかやったときに、誰の主張が通ってしまうでしょうという問題。

```csharp
using System.Globalization;
using System.Runtime.CompilerServices;

// file: A.cs
class Aさんの主張
{
    [ModuleInitializer]
    public static void Init()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
    }
}

// file: B.cs
class Bさんの主張
{
    [ModuleInitializer]
    public static void Init()
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ja-JP");
    }
}

// file: C.cs
class Cさんの主張
{
    [ModuleInitializer]
    public static void Init()
    {
        var c = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        c.DateTimeFormat.LongDatePattern = "yyyy'-'MM'-'dd";
        c.DateTimeFormat.LongTimePattern = "HH':'mm':'ss";
        c.DateTimeFormat.MonthDayPattern = "MM'-'dd";
        c.DateTimeFormat.YearMonthPattern = "yyyy'-'MM";
        c.DateTimeFormat.ShortDatePattern = "yyyy'-'MM'-'dd";
        c.DateTimeFormat.ShortTimePattern = "HH':'mm':'ss";
        CultureInfo.DefaultThreadCurrentCulture = c;
    }
}
```

そしてこの話題から、「後だし優先といわれても、
じゃあ、[モジュール初期化子](../../../../study/csharp/oop/moduleinitializer.md)の実行順序は決まっているの？」という話題になります。

## 呼び出し順序

「[モジュール初期化子の実装方法](../../../../study/csharp/oop/moduleinitializer.md#module-initializer-impl)」のところで書いてるんですが、
C# 的に「1つのモジュールに複数のモジュール初期化子がある」とき、
コンパイル結果的には「本当のモジュール初期化子は1つで、その中で複数のメソッドを呼ぶ」みたいな実装になっています。
問題はこの「複数のメソッド呼び出し」がどういう順序で並ぶか。

C# の仕様書上は「reserved, but deterministic order」
(詳細は明言せず将来変更の余地を残す、ただし決定論的な順序)
となっているみたいです。
こういう場合、同じ環境(同じツールの同じバージョン)でコンパイルする限りは、
同じソースコードからは同じ実行ファイルができます。
その一方で、環境が変わると結果が変わる可能性あり。

ここでは現時点での実装がどうなっているかという話もしておきましょう。

### 同じファイルの中

まずは同じファイルの中に複数のモジュール初期化子を書いた場合。
これはまあ結構単純で、上から順番です。

メタデータ的に、同じ名前空間内のクラスは書いた順に並ぶし、
クラスのメンバーも書いた順に並びます。
モジュール初期化子はその順で呼ばれます。

例えば以下のようなコードを1つのファイルに書いた場合、

```csharp
using System.Runtime.CompilerServices;

class Z
{
    [ModuleInitializer]
    public static void Init1() { }

    [ModuleInitializer]
    public static void Init2() { }
}

class Y
{
    [ModuleInitializer]
    public static void Init2() { }

    [ModuleInitializer]
    public static void Init1() { }
}

class A
{
    [ModuleInitializer]
    public static void Init() { }
}
```

呼ばれるのは `Z.Init1`, `Z.Init2`, `Y.Init2`, `Y.Init1`, `A.Init` の順です。

### ファイルが分かれている場合

問題は複数のファイルが分かれている場合。

[調べた感じ](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2023/MemberOrder)、
`<Compile>` タグの手書きで順序を明示することもできるけども、
`*.cs` 指定にした場合はビルド ツール依存
(MSBuild.exe と dotnet build で微妙に結果に差あり)
みたいです。

とはいえ、まあ、おおむね、ファイル名でソートしているみたいです。
補足として、

* Ordinal 比較でソート
    * 安心してください、カルチャーには依存しません
    * a と b の間に À が来たりしません。文字コード順
* MSBuild.exe は UTF-16 比較、dotnet build は UTF-8 比較してそう

となります。

例えば以下のような2つのファイルを同じプロジェクトに含めた場合、

```csharp
// このコードを A.cs に書く
using System.Runtime.CompilerServices;

class Z
{
    [ModuleInitializer]
    public static void Init() { }
}
```

```csharp
// このコードを Z.cs に書く
using System.Runtime.CompilerServices;

class A
{
    [ModuleInitializer]
    public static void Init() { }
}
```

(型名とファイル名が逆なことに注意。)

呼ばれるのは `Z.Init`, `A.Init` の順です。

### 一番最後に並ぶもの

ここで「後だし優先」の話に戻ります。

これまでに話した仕様に沿って、極力「後だし」になるためにやれそうな工夫としては、

* 文字コード的に後ろの方の文字、例えば半角カナとかのファイル名を付ける
* `<Compile Include=""/>`タグを明示的に後ろの方に書く
* .targets ファイル(.csproj の末尾に import されるファイル)に `<Compile>` タグを書く

とかですかね。

まあ、モジュール初期化子の実行順序に依存するようなコードを書くのは邪悪なのであんまりおすすめはしませんが。
