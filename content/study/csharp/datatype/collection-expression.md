---
title: "コレクション式"
source_url: "https://ufcpp.net/study/csharp/datatype/collection-expression/"
content_type: "Article"
published_at: "2023-10-24T00:00:00"
updated_at: "2023-10-24T22:46:16"
tags: []
umbraco_id: 2475
parent_id: 1940
sort_order: 6
aliases: []
---

# コレクション式

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

<h5 class="version version12">Ver. 12</h5>

C# 12 で、`[]` 記号を使って配列などの初期化ができるようになりました。
配列だけではなく、コレクション(`List<T>` 型など)、`Span<T>` なども全く同じ書き方で初期化できます。
これをコレクション式(collection expression)と言います。

```csharp {title="コレクション式" highlight-ranges="sha256:f910249d2a264e3d542e6b64cb3d32585edd0d2a69bea57c15c964163f3ebd18;3:15-3:24,4:18-4:27,5:18-5:27,6:25-6:34,7:33-7:42"}
using System.Collections.Immutable;

int[] array = [1, 2, 3];
List<int> list = [1, 2, 3];
Span<int> span = [1, 2, 3];
ReadOnlySpan<int> ros = [1, 2, 3];
ImmutableArray<int> immutable = [1, 2, 3];
```

また、コレクション式中では、`..` を使うことで「別のコレクションの中身の展開」ができます。
これを スプレッド (spread)演算子と言います。

```csharp {highlight-ranges="sha256:69f2146a3dd2229ba74d69539a785a324542af8ee84d16dd1e8f9e876feb4108;5:22-5:24,5:32-5:34"}
int[] array1 = [1, 2, 3];
int[] array2 = [4, 5, 6];

// 0, 1, 2, 3, 4, 5, 6, 7
int[] combined = [0, ..array1, ..array2, 7];
```

## <a id="sec-generated-title-2"></a> <a id="background">背景: これまでのコレクションの初期化</a>

これまでだと、以下のように型に応じて書き方を変える必要がありました。

```csharp {title="これまでの書き方いろいろ"}
using System.Collections.Immutable;

int[] array1 = new[] { 1, 2, 3 };
int[] array2 = { 1, 2, 3 };
List<int> list = new() { 1, 2, 3 };
Span<int> span = stackalloc[] { 1, 2, 3 };
ReadOnlySpan<int> ros = new[] { 1, 2, 3 };
ImmutableArray<int> immutable = ImmutableArray.Create(1, 2, 3);
```

1つずつ、もう少し補足を加えます。

#### <a id="sec-generated-title-3"></a>配列(1)

`new[] { }` という書き方で配列を作れます。

```csharp {title="配列の new"}
int[] array1 = new[] { 1, 2, 3 };
```

正確にはこれは省略形で、本来の書き方では、`new T[length]` という書き方で配列を作ります。
ただし、`{}` (配列初期化子といいます)がついている場合、中身の数から長さが確定するので `new T[] { ... }` というように長さを省略できます。
また、`{}` の中身から型を推論できる場合には `T` の部分を省略できて、`new[] { ... }` と書けます。

`new[] { ... }`の型推論は「要素の中身から型決定」なので、例えば以下のようなコードはエラーになります。

```csharp {title="要素の型からの型推論"}
byte[] array // byte[] 型
    = new[] { 1, 2, 3 }; // 数値リテラルはデフォルトでは int 型。int からの型推論で int[] 型に。
```

また、「要素の中身から型決定」だと、ブログ「[共変配列事故](../../../blog/2022/11/covariantarrayincident/index.md)」で書いたような問題を踏むことがあります。

#### <a id="sec-generated-title-4"></a>配列(2)

配列の変数宣言時に限り、以下のように `{}` だけで初期化できます。

```csharp {title="配列初期化子"}
int[] array2 = { 1, 2, 3 };
```

宣言と初期化を別の行に分けてしまうと `{}` を使えなくなります。(`new[] { }` なら使えます)。

```csharp
int[] array; // 宣言

array = { 1, 2, 3 }; // 宣言と別の行ではこの書き方はできない
```

#### <a id="sec-generated-title-5"></a>コレクション初期化子

所定の条件を満たす型に対して、`new T() { }` という書き方で初期化ができます。
これを[コレクション初期化子](../functional/sp3_lambda.md#collectioninit)と言います。

これと、[ターゲットからの型推論](../cheatsheet/ap_ver9.md#target-typed-new)を組み合わせると、以下のように `new() { }` という書き方で初期化できます。

```csharp
List<int> list = new() { 1, 2, 3 };
```

配列の場合は `new[] { }` なのに対して、その他のコレクションは `new() { }` になります。
このせいで、「配列とその他のコレクションを切り替えて使う」みたいなことがちょっと面倒になっています。

```csharp {title="配列とコレクションの切り替えが難しい"}
#if WPF
using System.Collections.ObjectModel;

// WPF の時だけ ObservableCollection を使う。
ObservableCollection<int>
#else
// 他はただの配列。
int[]
#endif
    list = new() { 1, 2, 3 }; // () と [] が違うのでコード共通化が難しい。
```

#### <a id="sec-generated-title-6"></a>stackalloc

パフォーマンス的に配列を使いたくない場面があり、
そういう場合 [stackalloc](../interop/sp_unsafe.md#safe-stackalloc)というものが使えることがあります。

元々は unsafe な機能でめったに使うものではなかったんですが、
C# 7.2 で、[`Span<T>` 構造体](../resource/span.md)の導入とともに safe な構文になりました。

```csharp {title="stackalloc"}
Span<int> span = stackalloc[] { 1, 2, 3 };
```

これも本来の書き方は `stackalloc T[length]` で、長さ・型の推論が働くことで `stackalloc[] { }`という書き方ができています。

ちなみに、`stackalloc` は参照型を含められないという問題があって、
例えば以下のようなコードはコンパイル エラーになります。

```csharp {title="参照型に対する stackalloc はエラーに"}
Span<string> span = stackalloc[] { "abc" };
```


#### <a id="sec-generated-title-7"></a> <a id="static-data">静的データ最適化</a>

`ReadOnlySpan<T>` 型に対して配列を渡すと、
最適化で配列が消えてくれて、stackalloc を使うよりもパフォーマンスがよくなることがあります。

```csharp {title="静的データ最適化"}
ReadOnlySpan<int> ros = new[] { 1, 2, 3 };
```

詳しくはブログ「[静的なデータの ReadOnlySpan 最適化](../../../blog/2018/12/staticdatareadonlyspan/index.md)」に書いたことがあるのでそちらを参照してください。
(C# 12 からは `byte`, `sbyte` 以外の型に対してもこの最適化がかかります。)

この最適化は、「条件がそろえば配列が消える」というのが上級者にしかわからないという問題があります。
ぱっと見はパフォーマンスが悪そうに見えますし、
古い C# コンパイラーで同様のコードをコンパイルすると実際パフォーマンスが悪いので、
結構な混乱を招いています。

#### <a id="sec-generated-title-8"></a>ImmutableArray

[`ImmutableArray<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.immutablearray-1) という型に対しては初期化子の類が使えません。
以下のように地道に `Create` メソッドを呼ぶ必要があります。

```csharp {title="ImmutableArray.Create"}
ImmutableArray<int> immutable = ImmutableArray.Create(1, 2, 3);
```

ところが質が悪いことに、`ImmutableArray<T>` 型はコレクション初期化子を使える要件を満たしてしまっています。
以下のようなコードは「コンパイルはできてしまうけど、実行すると必ず例外が起こる」という、かなりつらい状態になります。

```csharp {title="ImmutableArray に対してコレクション初期化子"}
using System.Collections.Immutable;

// コンパイルはできてしまう。
// ところが実行すると必ず例外。
ImmutableArray<int> immutable = new() { 1, 2, 3 };
```

## <a id="sec-generated-title-9"></a> <a id="collection-expr">コレクション式</a>

前節で説明したような「型によって初期化の方法が違う」という問題と、補足で書いた諸問題に対処するため、
C# 12 で<strong id="key-collection-expr" class="keyword">コレクション式</strong>(collection expression)というものを導入することになりました。

概要でも書いたように、`[]` 記号を使って配列などを初期化します。

```csharp {title="コレクション式"}
using System.Collections.Immutable;

int[] array = [1, 2, 3];
List<int> list = [1, 2, 3];
Span<int> span = [1, 2, 3];
ReadOnlySpan<int> ros = [1, 2, 3];
ImmutableArray<int> immutable = [1, 2, 3];
```

コレクション式は以下のような型に対して使えます。

* 配列
* `Span<T>`, `ReadOnlySpan<T>`
* 配列が実装している `IEnumerable<T>`, `IReadOnlyList<T>`, `IList<T>` などのインターフェイス
* [`CollectionBuilder` 属性](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.compilerservices.collectionbuilderattribute)が付いている型
* [コレクション初期化子](../functional/sp3_lambda.md#collectioninit)の条件を満たす型

配列初期化子と違って、`[]` はどこにでも書けます。

```csharp {title="宣言以外でも書ける"}
int[] array;

// OK
array = [1, 2, 3];
```

stackalloc と違って、参照型の `Span<T>` に対しても使えます。
ちゃんと、(配列を作るよりも)パフォーマンスのいいコードになります。
(内部的には[InlineArray](inline-array.md)を使っています。)

```csharp {title="参照型の Span"}
// OK
Span<string> span = ["abc"];
```

`ReadOnlySpan<T>` に対しては前述の[静的データ最適化](#static-data)がかかるわけですが、
「配列を new してそうに見えるコード」がないだけ混乱が少ないです。

```csharp {title="ReadOnlySpan の静的データ最適化"}
// 以下のコードはちゃんと静的データ最適化がかかる。
ReadOnlySpan<int> span = [1, 2, 3];
```

コレクション初期化子の条件よりも、`CollectionBuilder` 属性の方が優先されます。
[`ImmutableArray<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.immutablearray-1) には `CollectionBuilder` 属性が付いていることによってコレクション式を使えます。

```csharp {title="ImmutableArray に対するコレクション式"}
using System.Collections.Immutable;

ImmutableArray<int> immutable = [1, 2, 3];

// CollectionBuilder 属性の情報をもとに、C# コンパイラーが以下のようなコードに展開する。
//
//ReadOnlySpan<int> temp = [1, 2, 3];
//ImmutableArray<int> immutable = ImmutableArray.Create(temp);
```

ちなみに、`CollectionBuilder` 属性はインターフェイスにも付けることができます。
[`IImmutableList<T>`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.immutable.iimmutablelist-1)が一例で、以下のようなコードを書けます。

```csharp {title="CollectionBuilder 属性はインターフェイス可"}
using System.Collections.Immutable;

IImmutableList<int> immutable = [1, 2, 3];

// IImmutableList インターフェイスについている CollectionBuidler 属性では、
// ImmutableList.Create メソッドを使うよう指定されているので、
// おおむね以下のようなコードと同じ意味。
//
//ReadOnlySpan<int> span = [1, 2, 3];
//IImmutableList<int> immutable = ImmutableList.Create(span);
```

`List<T>` は「コレクション初期化子の条件」に該当していて、コレクション初期化子の時と同じコードに展開されます。

```csharp
List<int> list = [1, 2, 3];

// コレクション初期化子 new() { 1, 2, 3 } と同じ結果。
//
//List<int> list = new();
//list.Add(1);
//list.Add(2);
//list.Add(3);
```

`IEnumerable<T>` などのインターフェイスに対しては、
将来の最適化の余地を残すため、実際に何の型が使われるかは仕様化されていません。
参考までに、「C# 12 リリース時点」では以下のような実装になっています。

```csharp
//※ C# 12 時点の実装。将来、最適化で変更する余地あり。

// 長さが既知で読み取り専用 → ReadOnlyArray みたいな型をコンパイラーが生成して使う。
IEnumerable<int> x1 = [1, 2, 3];
IReadOnlyList<int> x2 = [1, 2, 3];
IReadOnlyCollection<int> x3 = [1, 2, 3];

// 書き換え・追加・削除可能なもの → new List<T> を使用。
ICollection<int> x4 = [1, 2, 3];
IEnumerable<int> x5 = [1, 2, 3];

Twice(x1);

// 長さが未知の場合も new List<T> を使用。
static IEnumerable<int> Twice(IEnumerable<int> e) => [.. e, .. e];

// 空っぽの時は Array.Empty<T>() を使用。
IEnumerable<int> empty = [];
```

いずれにせよ、ターゲットとする型から最適なものを選んで展開されると思ってください。
コレクション式は「書きやすい」だけではなく、「<em>一番パフォーマンスがいい</em>」を目標としています。

(おおむね目標は達成しているので、以前の書き方を積極的に選ぶ理由はありません。
配列やコレクションの初期化は、C# 12 以降を使えるのであればすべて `[]` を使っていいと思います。)

### <a id="sec-generated-title-10"></a> <a id="square-bracket">余談: [] 括弧</a>

C# ではこれまで、
配列の `new[] { }` にしろ、コレクション初期化子の `new() { }` にしろ、
コレクション系の構文には `{}` を使うものが多かったわけですが、
C# 12 のコレクション式は `[]` になりました。

その理由として、`{}` は用途が多すぎてこれ以上新しい構文を兼ねることが難しかったというのがあります。

```csharp
// 普通に単独で書ける。
// これは「空のブロック」。
// 要は、if とかの後ろにある { } と同じものを単独で書いてる。
{ }

// ラムダ式の後ろとか、
var action = () => { };

// メソッドの後ろとかにも普通にブロックを書く。
void localFunc() { }

// 初期化子に限っても、
// コレクション初期化子(Add メソッド呼び出しになる)と、
var list = new List<int> { 1, 2, 3 };

// オブジェクト初期化子(プロパティの初期化になる)があるし、
var obj = new EnumerationOptions { BufferSize = 1024 };

// 匿名クラスとかにも使う。
var anonymous = new { X = 1, Y = 2 };

// 将来候補として、「ブロック式」を導入したいという話もある。
var x = { var i = 123; i * i };
```

特に、オブジェクト初期化子とコレクション初期化子が同じ記号を使っていて、混在不可なので以下のようなことが起こります。

```csharp {title="2つの {} 初期化子"}
// オブジェクト初期化子(プロパティの値指定)とコレクション初期化子(Add)の混在不可。
var list1 = new List<int> { Capacity = 1014, 1, 2, 3 };

// ちなみに、[0] = 1 みたいな書き方はオブジェクト初期化子の範疇。
var list2 = new List<int>
{
    Capacity = 1014,
    [0] = 1, // 実行時例外にはなるけど、コンパイルは通っちゃう。
};

// なので「混在不可」のせいで以下のコードもエラー。
var dictionary = new Dictionary<int, int>
{
    { 1, 2 },
    [0] = 1,
};
```

この問題は C# 11 で[リスト パターン](patterns.md#list)を導入する際にも問題になりました。
C# 8 の頃からある[プロパティ パターン](patterns.md#property)との区別のためには `{}` を使えませんでした。
例えば以下のようなコードで、「空っぽのリスト」の意味で `list is {}` とは書けないという問題があったりします。

```csharp {title="{} の「兼用」は難しい"}
var obj = new { X = 1, Y = 2 };
var list = new[] { 1, 2, 3 };

// プロパティ パターン。
_ = obj is { X: 1 };

// (当初検討にあった)リスト パターンの候補としての {}。
_ = list is { 1, .. } ;

// プロパティ パターンで {} は「単に null チェック」になる。
var isNotNull = obj is { };

// これは C# 11 以前から有効な文法(「null じゃない」の意味)なので、
// リスト パターンとして {} を使おうとすると「空リストとマッチ」にはできなくなる。
var isEmpty = list is { };
```

そこで C# 11 では最終的にリスト パターンに `[]` を採用したわけですが、
だったら「リスト構築の方でも `[]` を使った方がきれい」という話になりました。

```csharp {title="コレクション式とリスト パターンが対"}
// () コンストラクター/タプル構築と位置パターンが対。
var obj = new DateOnly(2021, 1, 1);
_ = obj is (2021, _, _);

var tuple = (1, 2);
_ = tuple is (1, _);

// {} オブジェクト初期化子/匿名クラスとプロパティ パターンが対。
var prop = new EnumerationOptions { BufferSize = 1024 };
_ = prop is { BufferSize: 1024 };

var anon = new { X = 1, Y = 2 };
_ = anon is { X: 1 };

// [] コレクション式とリスト パターンが対。
int[] list = [1, 2, 3];
_ = list is [1, ..];
```

### <a id="sec-generated-title-11"></a> <a id="null-conditional-foreach">余談: null 条件 foreach</a>

「C# に追加して欲しい機能」としてそこそこ高頻度で挙がるものの1つに、
「null 条件 foreach」があったりします。
これは要するに、以下のようなコードを、

```csharp {title="null があり得る foreach"}
Print([1, 2, 3]);
Print(null);

static void Print(IEnumerable<int>? list)
{
    // null が来た時に普通にぬるぽ。
    foreach (var item in list)
        Console.Write($"{item} ");
}
```

以下のように直すのが嫌で、

```csharp {title="null チェックで1段インデントが下がる"}
static void Print(IEnumerable<int>? list)
{
    // null チェックを1行足せばいいだけの話なものの、1段インデントが下がるのが嫌。
    if (list is not null)
        foreach (var item in list)
            Console.Write($"{item} ");
}
```

以下のように `foreach?` という構文を追加してもらえないかという提案です。

```csharp {title="foreach?"}
static void Print(IEnumerable<int>? list)
{
    // foreach? 構文を足すのはどうだろう？
    foreach? (var item in list)
        Console.Write($"{item} ");
}
```

確かに時々そういう機能が欲しくなるものの、
新文法を導入するほどのものかと言われると悩ましく、
長らく塩漬けになっている提案です。

そしてこの度コレクション式が入ったことで、
これと同等のことが以下のコードで実現できるようになりました。

```csharp {title="??[]"}
static void Print(IEnumerable<int>? list)
{
    // ?? [] の4文字を追加すれば null チェック代わりになる。
    foreach (var item in list ?? [])
        Console.Write($"{item} ");
}
```

これはまあ、以下のようなコードとほぼ同等です。

```csharp {title="??[] と同等のコード"}
static void Print(IEnumerable<int>? list)
{
    if (list is null) list = Array.Empty<int>();

    foreach (var item in list)
        Console.Write($"{item} ");
}
```

`Array.Empty<int>()` を挟んでいるので無駄そうに見えるわけですが、
.NET 8 では[かなり強力な最適化がかかるようになっていて](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-8/#collections)、
`Array.Empty<int>()` に対する foreach のコストはかなり低く抑えられます。

(これが十分に便利なので、`foreach?` 提案が通る可能性はかなり低くなりました。)

## <a id="sec-generated-title-12"></a> <a id="spread">スプレッド</a>

コレクション式の中では、`..` を使って「他のコレクションの中身を展開」みたいなことができます。
これを<strong id="key-spread" class="keyword">スプレッド</strong>(spread: 広げる、伸ばす、まき散らす)演算子と言います。

```csharp {title=".."}
int[] a = [1, 2];
int[] b = [3, 4];

// これで 0, 1, 2, 3, 4, 5 になる。
int[] c = [0, ..a, ..b, 5];
```

これは、`List<T>` でいう [`AddRange` メソッド](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.addrange)であったり、
[LINQ でいう `Concat` メソッド](../data/sp3_stdqueryo.md#concat)みたいな物です。

```csharp {title="AddRange と Concat"}
int[] a = [1, 2];
int[] b = [3, 4];

// コレクション式では ..
int[] expression = [0, ..a, ..b, 5];

// List<T> でいう AddRange
var list = new List<int>();
list.Add(0);
list.AddRange(a);
list.AddRange(b);
list.Add(5);

// LINQ でいう Concat
var linq = new[] { 0 }
    .Concat(a)
    .Concat(b)
    .Append(5);
```

ちなみに[先ほど](#square-bracket)、コレクション式とリスト パターンが対という話をしましたが、

```csharp {title="コレクション式とリスト パターンが対"}
// [] コレクション式とリスト パターンが対。
int[] list = [1, 2, 3];
_ = list is [1, ..];
```

スプレッドも[スライス パターン](patterns.md#slice-pattern)と対になっています。

```csharp {title="スプレッドとスライス パターンが対"}
int[] list = [1, 2, 3, 4, 5];

// スライス: コレクションの一部分を切り出して新しい変数に代入。
if (list is [var first, ..var middle, var last])
{
    // スプレッド: コレクションの中身を展開して、1つのコレクションに結合。
    int[] list2 = [first, ..middle, last];
}
```


## <a id="sec-generated-title-13"></a> <a id="type-inference">型推論・オーバーロード解決</a>

元々あった配列やコレクション初期化子の構文では、`new T[]` や `new List<T>()` などというように、型名を明示できます。
それに対して、コレクション式は `[]` しか書かないので何の型になるかは完全に推論だよりになります。

ちなみに、[後述](#after12)しますが、C# 12 時点では以下のような「`var` との組み合わせ」はできません。
C# 13 以降で検討中です。

```csharp {title="C# 12 時点では型決定できない var"}
var list = [1, 2, 3];
```

### <a id="sec-generated-title-14"></a> <a id="target-typed">ターゲットの型から</a>

基本的にコレクション式の型はターゲット(代入先の変数・引数の型)から決定します。

この点は `new[] {}` の場合と異なります。
`new[] {}` は「中身の型からの推論優先」で、コレクション式 `[]` は「ターゲットからの推論優先」です。
例えば、以下のようなコードでは、`x` と `y` に代入されるインスタンスの型が異なります。

```csharp {title="new[]{} は要素からの推論なので時々困る"}
// new[]{} は要素からの型推論。
// x には string[] が入る。
object[] x = new[] { "a" };

// [] はターゲットからの型推論。
// y には object[] が入る。
object[] y = ["a"];

Console.WriteLine(x.GetType().Name); // String[]
Console.WriteLine(y.GetType().Name); // Object[]

y[0] = 1; // OK。
x[0] = 1; // 例外が出る(C# 1.0 からある嫌な仕様)。
```

(この例の最後の行はかなり[奇妙で安全性に欠ける仕様ですが、大昔の名残りで今更変更できない](../../../blog/2022/11/covariantarrayincident/index.md)そうです。)

コレクション式の「ターゲットからの型推論」は、型や式が入れ子になっていてもちゃんと働きます。
以下のように、[タプル](tuples.md)中にコレクションがあって、条件演算子や [switch 式](typeswitch.md#switch-expression)を経由していても正しく型推論されます。

```csharp {title="入れ子でもちゃんと型推論される例"}
bool b = true;
int i = 1;

// 条件演算子 x ? y : z とか、
// switch 式 x switch { ... } とか、
// タプルを使った入れ子とかあっても、ちゃんと型推論される。
(byte, (byte, byte[] z)[])[] x = b ? [] : i switch
{
    _ => [
        (1, [(2, [3, 4])]) // [3, 4] の部分、ちゃんと byte[] になる。
    ]
};
```

### <a id="sec-generated-title-15"></a> <a id="from-element">要素の型から</a>

一方で、メソッドのオーバーロード解決などが絡む場合、
コレクション式の中身からの型解決も働きます。

```csharp {title="要素の型からオーバーロード解決"}
Print([1, 2]);     // Print<int>
Print([1.1, 2.2]); // Print<double>
Print(["a", "b"]); // Print<string>

static void Print<T>(T[] args) { /* 省略 */ }
```

ただ、スプレッドが絡むとき、スプレッドの中身の優先度は低いそうです。
(実装が大変なわりに需要が少ないという判断。)

```csharp {title="スプレッドが絡むとき"}
byte[] x = [1, 2];

// ..x しかない場合には x の型から byte[] に決定。
Print([.. x]);     // Print<byte>

// ただ、その横に整数リテラルが1個でも並ぶと…
// 3 (int) につられて int[] に決定。
Print([.. x, 3]);  // Print<int>

static void Print<T>(T[] args) { }
```

### <a id="sec-generated-title-16"></a> <a id="priority">オーバーロード解決の優先度</a>

`[]` は配列や `List<T>`, `Span<T>` など、いろいろな型になるわけですが、
では、そのいずれも候補になるオーバーロードの場合はどれが優先されるでしょうか。

これまでの C# では、「元の型に近いほど優先度が高い」というのがオーバーロード解決の基本ルールで、
具体的には以下のような順(上ほど優先)になっています。

1. ぴったり一致する型
2. 基底クラス(階層が近いほど優先)
3. 実装しているインターフェイス(階層が近いほど優先)
4. 暗黙の型変換できる型

このルールに沿うなら、配列(`[]` で作るのではなく `new T[]` で作る)の場合、
一例をあげると以下のような優先度になります。

1. `T[]`
2. `IList<T>`
3. `IEnumerable<T>`
4. `Span<T>` もしくは `ReadOnlySpan<T>` (両方あるとエラー)

一方で、コレクション式 `[]` の場合、「パフォーマンスがいいものを優先する」という目標のため、
`Span<T>` と `ReadOnlySpan<T>` を特別扱いして優先度を上げています(ぴったり一致する型よりも優先度が上)。
そのため、以下のような優先度になっています。

(※ .NET 8 RC 2 時点では `ReadOnlySpan<T>` の優先度がちょっと低いですが、正式リリースまでに変更される予定です。)

1. `ReadOnlySpan<T>`
2. `Span<T>`
3. `T[]`
4. `IList<T>`
5. `IEnumerable<T>`

以下に例を挙げます。

```csharp {title="具象型優先"}
A.M([]); // int[]

class A
{
    // 具象型優先。
    public static void M(int[] _) { }
    public static void M(IEnumerable<int> _) { }
}
```

```csharp {title="具象型優先"}
A.M([]); // List<int>

class A
{
    // 具象型優先。
    public static void M(List<int> _) { }
    public static void M(IList<int> _) { }
}
```

```csharp {title="具象型に近い方優先 (派生側優先)"}
A.M([]); // IList<int>

class A
{
    // 具象型に近い方優先 (派生側優先)。
    public static void M(IList<int> _) { }
    public static void M(IEnumerable<int> _) { }
}
```

```csharp {title="Span 優先"}
A.M([]); // Span<int>

class A
{
    // Span 優先(コレクション式のみの特殊動作)。
    public static void M(Span<int> _) { }
    public static void M(int[] _) { } // 普通は具象型優先。
}
```

```csharp {title="Span 優先"}
A.M([]); // Span<int>

class A
{
    // Span 優先(コレクション式のみの特殊動作)。
    public static void M(Span<int> _) { }
    public static void M(IList<int> _) { } // 普通は 派生 > 型変換
}
```

```csharp {title="ReadOnlySpan 優先"}
A.M([]); // ReadOnlySpan<int>

class A
{
    // ReadOnlySpan 優先(コレクション式のみの特殊動作)。
    public static void M(ReadOnlySpan<int> _) { }
    public static void M(IReadOnlyList<int> _) { } // 普通は 派生 > 型変換
}
```

```csharp {title="ReadOnlySpan 優先"}
A.M([]); // ReadOnlySpan<int>

class A
{
    // ReadOnlySpan 優先(コレクション式のみの特殊動作)。
    public static void M(ReadOnlySpan<int> _) { }
    public static void M(Span<int> _) { } // (.NET 8 RC 2 時点ではまだこっちが優先されてる。変更予定)
}
```

ちなみに、以下のような場合には(普通のオーバーロード解決でも、コレクション式でも)不明瞭(オーバーロード解決不能)でコンパイル エラーになります。

```csharp {title="具象型同士は同列"}
A.M([]); // コンパイル エラー

class A
{
    // 具象型同士は同列。
    public static void M(int[] _) { }
    public static void M(List<int> _) { }
}
```

```csharp {title="派生関係のないインターフェイスは同列"}
A.M([]); // コンパイル エラー

class A
{
    // 派生関係のないインターフェイスは同列。
    public static void M(IList<int> _) { }
    public static void M(IReadOnlyList<int> _) { }
}
```

## <a id="sec-generated-title-17"></a> <a id="after12">将来計画(C# 13 以降)</a>

スケジュールの関係で C# 12 からは外れて、
C# 13 以降で実装される予定になっている機能がいくつかあります。
詳細は実装されてから追記しますが、
簡単に紹介だけしておきます。

#### <a id="sec-generated-title-18"></a>自然な型

`var x = [1, 2];` みたいに、`var` と組み合わせでは「ターゲットからの型推論」ができないわけですが、
この時にデフォルトで何の型になるかは C# 12 時点では決めかねました。

使い勝手を考えると(機能が多い) `List<T>` がいいですし、
一方で、パフォーマンスを考えると `Span<T>` がいいです(検討中)。

#### <a id="sec-generated-title-19"></a>拡張メソッドからの型推論

単純に `var x = [1, 2];` とは書けない一方で、
C# 12 時点でも、`var x = (int[])[1, 2];` みたいにキャストを挟めば型を決定できます。

ただ、キャストは「なんか書きにくい」
(キャストは型推論が効かないとか、`()` がタイピングしにくいとか、コード補完の都合で後置きの方が書き心地がいいとか)
という問題があって、
以下のように、拡張メソッドからの型推論が効くようにしてほしいという話があります。

```csharp {title="拡張メソッドからの型推論"}
// (List<int>)[1, 2] よりも、拡張メソッド形式の方が書き心地がいい。
var x = [1, 2].AsList();

static class Extensions
{
    public static List<T> AsList<T>(this List<T> list) => list;
}
```

この話はコレクション式に限らず、
[デリゲート](../functional/sp_delegate.md#natural-type)や[文字列補間](../start/improvedinterpolatedstring.md)でもそうなんですが、
C# 13 以降で取り組むそうです。

#### <a id="sec-generated-title-20"></a>Dictionary 式

`Dictionary<TKey, TValue>` などのキーを持つタイプのコレクションに対して、
以下のような構文で初期化できるようにしたいという案があって、これも C# 13 で検討中です。

```csharp
Dictionary<string, int> map =
[
    "one": 1,
    "two": 2,
];
```

#### <a id="sec-generated-title-21"></a>非ジェネリック コレクション

C# 12 では以下のようなコードに対応しなかったんですが、これも C# 13 で検討中です。

```csharp {title="非ジェネリック コレクション"}
using System.Collections;

ICollection c = ["a", 2, null];
```
