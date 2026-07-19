---
title: "params コレクション"
source_url: "https://ufcpp.net/blog/2024/3/params-collections/"
content_type: "BlogEntry"
published_at: "2024-03-02T17:44:41"
updated_at: "2024-03-02T17:44:41"
tags: []
umbraco_id: 2491
parent_id: 2490
sort_order: 0
aliases: []
---

# params コレクション

[ほぼ1年ぶり](../../../2023/2/params-ros/index.md)の [params](../../../../study/csharp/structured/sp_params.md#params) の話。

params を配列以外のコレクションに対して使えるようにするという話ですが、
雰囲気的に C# 13 でついに 入りそうです。
なので、最近そこそこ高頻度で Language Design Meeting の議題に上がっています。

* [Params Collection](https://github.com/dotnet/csharplang/issues/7700)
* C# Language Design Meeting
  * [November 15th, 2023](https://github.com/dotnet/csharplang/blob/main/meetings/2023/LDM-2023-11-15.md#params-improvements)
  * [January 29th, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-01-29.md#params-collections)
  * [January 31st, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-01-31.md#params-collections-evaluation-orders)
  * [February 21st, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md#params-collections)

まあ、割かしもう詳細を詰めている感じの話題が多めですね。

## params ‘コレクション’

去年には「`ReadOnlySpan<T>` 以外需要低め」、「他は[コレクション式](../../../../study/csharp/cheatsheet/ap_ver12.md#collection-expression)を使って `M([a, b, c])` でいいのでは」などという話も出ていましたが。
コレクション式を実装した今改めて検討して、
むしろ「コレクション式とそろえるのがいいのではないか」という感じに変わったみたいです。

```csharp
// ReadOnlySpan を優先するようになる予定。
C.M(1, 2, 3);

class C
{
    // 今でも書ける。
    public static void M(params int[] _) { }

    // 新規に書けるようになる予定。
    public static void M(params List<int> _) { }
    public static void M(params ReadOnlySpan<int> _) { }
}
```

## params ‘ref struct’

params に配列以外の型を認めたいという話の前提には、パフォーマンスを改善したいという要求があります。
なので、`Span` や `ReadOnlySpan` をはじめとした [ref struct](../../../../study/csharp/resource/refstruct.md) を使いたいです(ref struct 自体がパフォーマンス改善のために導入された概念)。

で、ref struct にはスコープの概念があって、引数や変数を [`scoped`](../../../../study/csharp/resource/refstruct.md#scoped) で修飾するかどうかでちょっと挙動が変わります。

```csharp
M(true);

static S M(bool b)
{
    if(b)
    {
        // [] が作る Span が S に伝搬してて、外に漏らせないので return に渡すとエラー。
        return C.Unscoped([1, 2, 3]);
    }
    else
    {
        // こちらは Span が伝搬しないので return できる。
        return C.Scoped([1, 2, 3]);
    }
}

class C
{
    // span の寿命が S に伝搬する。
    public static S Unscoped(Span<int> span) => new(span);

    // span の寿命を外に漏らさない。
    // なので、S に直接伝搬できない。
    // new(span.ToArray()) とかする必要がある。
    public static S Scoped(scoped Span<int> span) => new(span);
}

readonly ref struct S(Span<int> span)
{
    public readonly Span<int> Span = span;
}
```

で、ここに params をつけれるようになった場合にどうするかという話になります。

まあ、現状出ている用途を考えると「scoped じゃない params を必要とする場面はなく、scoped な params を必要とする場面はある」とのことで、「params が付いている時点で暗黙的に scoped にする」という判断になりそうです。

こうなるともう1つ問題が、オーバーライドをどうするかという話があるみたいです。
というのも、params 配列の場合、実はオーバーライド側には params 修飾を付けなくてもいいそうで。

```csharp
class Base
{
    public virtual void M(params int[] x) { }
}

class Derived : Base
{
    // params 配列の場合、派生側で params を付けなくても別にいい。
    public override void M(int[] x) { }
}
```

ところがまあ、「params ref strcut は暗黙的に scoped」みたいな暗黙の挙動があるので、
「何もつけてないのになぜか scoped」みたいな挙動は避けたいでしょう。
なので、この場合は「オーバーライド側にも params を必須にしたい」とのこと。

(けど、[LDM に挙げられている例](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-21.md#params-and-scoped-across-overrides)を見るに、戻り値があるときにだけこれを求められていそう…)

## オーバーロード解決

現在の params (配列の `params T[]`)とコレクション式は、ちょっとオーバーロード解決の仕組みが違います。
なので、「params の部分を `[]` で覆っても同じ結果になる」というのは**成り立たない**ことになります。
例えば以下のようなもの。

```csharp
C.M([1, 2, 3]); // こちらは解決できなくてエラーに。
C.M(1, 2, 3); // こちらは int[] 側に解決。

class C
{
    public static void M(params int[] _) { }
    public static void M(params long[] _) { }
}
```

で、params コレクションに関してですが、「既存の params 配列に沿う」案で行くみたいです。

## 引数の評価順

引数に副作用のある式を渡さない限り問題になることは少ないので忘れがちですが、
引数をどういう順で評価するかは決めておかないと混乱のもとです。
C# は基本的に「呼び出し側で並べた順」で、例えば名前付き引数を使うと順序を変えることができたりします。

```csharp
Test(GetA(), GetB()); // A → B
Test(b: GetB(), a: GetA()); // B → A

static void Test(int a, int b) { }

static int GetA() { Console.WriteLine("A"); return 0; }
static int GetB() { Console.WriteLine("B"); return 0; }
```

で、名前付き引数を使うと params 引数の場所も末尾以外に移せたり。

```csharp
Test(b: GetB(), c: GetC(), a: GetA()); // B → C → A

static void Test(int a, int b, params int[] c) { }

static int GetA() { Console.WriteLine("A"); return 0; }
static int GetB() { Console.WriteLine("B"); return 0; }
static int GetC() { Console.WriteLine("C"); return 0; }
```

ちなみにこの時、`params int[] c` のための配列は、`Test` を呼ぶ直前になるそうです。
ということで、展開結果は以下のような感じ。

```csharp
var b = GetB();
var c = GetC();
var a = GetA();
var paramsC = new[] { c };
Test(a, b, paramsC);
```

ところが、params コレクションとなるとどうなるべきかという話になります。
コレクションのインスタンスはいつ作られるべきなのか。

```csharp
Test(b: GetB(), c: GetC(), a: GetA());

static void Test(int a, int b, params MyCollection c) { }

static int GetA() { Console.WriteLine("A"); return 0; }
static int GetB() { Console.WriteLine("B"); return 0; }
static int GetC() { Console.WriteLine("C"); return 0; }

class MyCollection : IEnumerable<int>
{
    public MyCollection() { Console.WriteLine("MyCollection Construcotr"); }
    public void Add(int _) { }
    public IEnumerator<int> GetEnumerator() => throw new NotImplementedException();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => throw new NotImplementedException();
}
```

こんな副作用の起こし方をするコードはめったに書かないでしょうけども、
"MyCollection Construcotr" はいつ表示されるべきでしょう？

とりあえず現状は、B → MyCollection → C → A の順で考えているそうです。
引数 `c:` の場所で生成。`GetC` を呼ぶよりも前。
要するに、以下のように展開したいんでしょうね。

```csharp
var b = GetB();
var a = GetA();
var paramsC = new MyCollection();
paramsC.Add(GetC());
Test(a, b, paramsC);
```

## メタデータ

今ある params 配列のコンパイル結果には `System.ParamArrayAttribute` が付きます。
で、C# 13 で考えている params コレクションでも、別にこの属性を使いまわすこともできるそうです。

ただ1点懸念は、C# 以外のコンパイラーが誤動作しないかどうか。
新しい属性であれば「未対応なので無視」でいいわけですが、
既存の属性を使いまわすと「`ParamArray` 属性が付いているのであれば配列でないとダメ」というコンパイル エラーを起こす可能性が高いです。

ということで、新しい params コレクションについては新しい属性として `System.Runtime.CompilerServices.ParamCollectionAttribute` を用意するそうです。
