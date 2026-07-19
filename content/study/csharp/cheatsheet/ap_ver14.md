---
title: "C# 14.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver14/"
content_type: "Article"
published_at: "2025-08-31T00:00:00"
updated_at: "2025-12-20T20:47:29"
tags: []
umbraco_id: 2514
parent_id: 1174
sort_order: 19
aliases:
  - "/csharp/cheatsheet/ap_ver14/"
---

# C# 14.0 の新機能

<div class="version version14">Ver. 14.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2025/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 10.0</li>
<li>Visual Studio 2022 18.0</li>
</td>
</tr>
</table>

執筆予定: [C# 14.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/487)

## <a id="sec-generated-title-1"></a> <a id="field-keyword">field キーワード</a>

`field` という文脈キーワードが追加されました。
プロパティの `get`/`set` の中に `field` と書くと、
バッキング フィールドを生成した上で、そのフィールドの読み書きができます。
例えば前述の例を `field` を使って書き直すと以下のようになります。

```csharp
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

class FieldBackedProperties : INotifyPropertyChanged
{
    // 遅延初期化: 最初のプロパティ アクセス時にインスタンスを生成。
    public string X => field ??= "";

    // set 側だけ null 許容(get 側で ?? で非 null 化)。
    [AllowNull]
    public string Y
    {
        get => field ?? "";
        set;
    }

    // INotifyPropertyChanged の実装: get 側だけ素通し。
    public string? Z
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                PropertyChanged?.Invoke(this, new(nameof(Z)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
```

詳しくは「[field キーワード](../oop/oo_property.md#field-keyword)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="null-conditional-assignment">null 条件代入</a>

代入演算の左側で `?.` や `?[]` を書くことで「null じゃないときだけ代入」ができるようになりました。
これを null 条件代入(null conditional assignment)といいます。

```csharp
static void M(A? a)
{
    // if (a != null) a.X = 10; とほぼ同じ。
    a?.X = 10;

    // if (a != null) a[0] = 10; とほぼ同じ。
    a?[0] = 10;

    // if (a != null) a.Event += () => { }; とほぼ同じ。
    a?.Event += () => { };
}

class A
{
    public int X { get; set; }

    public int this[int index]
    {
        get => 0;
        set { }
    }

    public event Action? Event;
}
```

詳しくは「[null の取り扱い - null じゃないときだけ代入](../resource/rm_nullusage.md#null-conditional-assignment)」で説明します。

## <a id="sec-generated-title-3"></a> <a id="first-class-span">First-class Span</a>

`Span<T>`/`ReadOnlySpan<T>` 構造体を言語構文的に特別扱いするようなりました。

詳しくは「[First-class Span](../resource/span.md#first-class-span)」で説明します。
## <a id="sec-generated-title-4"></a> <a id="overload-compound">複合代入演算子のオーバーロード</a>

複合代入演算子を直接オーバーロードできるようになりました。

```csharp
record struct X(int Value)
{
    public void operator +=(int value) => Value += value;
    public void operator -=(int value) => Value -= value;
    public void operator *=(int value) => Value *= value;
    public void operator /=(int value) => Value /= value;
    public void operator %=(int value) => Value %= value;
    public void operator &=(int value) => Value &= value;
    public void operator |=(int value) => Value |= value;
    public void operator ^=(int value) => Value ^= value;
    public void operator <<=(int value) => Value <<= value;
    public void operator >>=(int value) => Value >>= value;
    public void operator >>>=(int value) => Value >>>= value;
    public void operator checked +=(int value) { checked { Value += value; }; }
    public void operator checked -=(int value) { checked { Value += value; }; }
    public void operator checked *=(int value) { checked { Value += value; }; }
    public void operator checked /=(int value) { checked { Value += value; }; }
    public void operator ++() => Value++;
    public void operator --() => Value--;
    public void operator checked ++() { checked { Value++; } }
    public void operator checked --() { checked { Value--; } }
}
```

以前から二項演算子(`+` など)のオーバーロードをすることで、それに対応する複合代入(`+=` など)ができていましたが、この実装だとコピーのコストが不可避でした。
複合代入演算子を直接オーバーロードすることでこのコストを削減できます。

詳しくは「[複合代入演算子のオーバーロード](../oop/oo_operator.md#overload-compound)」で説明します。

## <a id="sec-generated-title-5"></a> <a id="simple-param-with-modifier">修飾子付きの引数の型名省略</a>

`ref` や `out` などの修飾子が必須の引数でも、ラムダ式引数の型名を省略できるようになりました。

```csharp
// C# 13 までは型名省略不可で、(string text, out int result) のように書く必要があった。
TryParse<int> m = (text, out result) => { result = 0; return true; };

delegate bool TryParse<T>(string text, out T result);
```

詳しくは「[修飾子付きの引数の型名省略](../functional/fun_localfunctions.md#simple-param-with-modifier)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="others">その他</a>

### <a id="sec-generated-title-7"></a> <a id="partial-event">部分イベントと部分コンストラクター</a>

[部分プロパティ](../misc/partial-type.md#partial_property) (C# 13)に続いて、
C# 14 では[イベント](../functional/sp_event.md)と[コンストラクター](../oop/oo_construct.md)も部分定義できるようになりました。

```csharp
// 元コード(手書き想定)。
partial class PartialClass
{
    // 部分イベント。
    public partial event Action<int>? PartialEvent;

    // 部分コンストラクター。
    public partial PartialClass();
}

// コード生成で作ってもらう前提のコード。
partial class PartialClass
{
    private Action<int>? _partialEvent;
    public partial event Action<int>? PartialEvent
    {
        add => _partialEvent += value;
        remove => _partialEvent -= value;
    }

    public partial PartialClass() { }
}
```

### <a id="sec-generated-title-8"></a> <a id="unbount-type-in-nameof">unbound な型に対する nameof</a>

`T<>` みたいに型引数を埋めていないジェネリック型(これを unbound (未束縛)とか open (開きっぱなし) な型といいます)に対して `nameof` 演算子を使えるようになりました。

```csharp
Console.WriteLine(nameof(List<>)); // "List"
Console.WriteLine(nameof(Dictionary<,>.Keys)); // "Keys"
Console.WriteLine(nameof(List<>.Enumerator.MoveNext)); // "MoveNext"
```

詳しくは「[unbound な型に対する nameof](../start/st_string.md#unbount-type-in-nameof)」で説明します。

### <a id="sec-generated-title-9"></a> <a id="file-based-app">ファイル ベース実行</a>

.NET 10 (C# 14 と同世代)で単独の `.cs` ファイルだけで C# プログラムを実行できるようになりました。

それに伴って、C# 的にも `#!` と `#:` (無視ディレクティブ)という機能が追加されています。
例えば以下のようなコードが書けて、
Unix 系シェルの [shebang](https://ja.wikipedia.org/wiki/%E3%82%B7%E3%83%90%E3%83%B3_(Unix)) を書けたり、これまでであればプロジェクト(`.csproj` ファイル中)に書いていた設定の類を C# ソースコード中に書けるようになっています。

```csharp
#!/usr/bin/env dotnet
#:sdk Microsoft.NET.Sdk.Web

var app = WebApplication.CreateBuilder(args).Build();
app.MapGet("/", () => "Hello World!");
app.Run();
```

詳しくは「[ファイル ベース実行](file-based-app.md)」で説明します。
