---
title: "C# 8.0 null許容参照型"
source_url: "https://ufcpp.net/blog/2018/12/cs8nrt/"
content_type: "BlogEntry"
published_at: "2018-12-08T19:48:11"
updated_at: "2018-12-08T19:48:11"
tags: []
umbraco_id: 2189
parent_id: 2177
sort_order: 7
aliases: []
---

# C# 8.0 null許容参照型

今日も C# 8.0 の新機能の話。
C# 8.0 の中でおそらく一番の目玉機能扱いになると思われる
null許容参照型の話です。

## 参照型でもそのままでは null を認めない

要は、参照型に対しても、単に`T`と書くとnullを認めない型になり、
null許容にしたければ`T?`と書くようにするという機能です。

```csharp
#nullable enable
    // string には null が来ない
    // null が来ないなら s.Length で OK
    static int M1(string s) => s.Length;
 
    // string? には null が来る
    // null が来るのに s.Length (null チェックしてない)はダメ
    static int M2(string? s) => s.Length;
 
    // string? には null が来る
    // null が来ても ?. や ?? を駆使すれば OK
    static int M3(string? s) => s?.Length ?? 0;
```

### null-forgiving

原理的に null を消せない場合もあります。
一例としては循環参照を作りたいときとかなんですが、
例えば以下のような感じで一時的に有効な値を持てない場合があり得ます。

```csharp
class Node
{
    public Node Next { get; private set; }
    public Node(Node next) => Next = next;
 
    public (Node a, Node b) CircularDependency()
    {
        // 参照に循環があるとき、どうしても片方は最初から有効な参照にできない
        var a = new Node(null); // やむなくいったん null
        var b = new Node(a);
        a.Next = b;
 
        // メソッドを抜けるまでには有効な値を入れておくのでどうかご容赦願いたい…
        return (a, b);
    }
}
```

こういうとき、警告をもみ消す処理があると問題を回避できます。
そのための演算子が後置きの`!`。

```csharp
// 非 null なところに null を渡すのを容赦してもらう
var a = new Node(null!);
var b = new Node(a);
a.Next = b;
```

今のところ、この演算子は null-forgiving 演算子と呼ばれています。
(日本語だとどうするといいんだろう。直訳だと「null容赦」。)
(ちなみに、口頭だとたぶん`!`をそのまま読んで、「びっくり演算子」(英語でも"bang"とか)呼ばれると思います。)

null-forgiving演算子はあくまで「警告になるコードを無視してもらう」という処理です
(そこが許容(able)と容赦(forgive)の差)。
コンパイル結果には何も影響を及ぼさないので、
間違ったコードを書くと普通に`NullReferenceException`が出るようになります。
(実行時のチェック処理とかは別に何も挿入されません。)

## 途中入り

機能名が「null許容参照型」になっている(非null参照型じゃない)のは、
何もつけない`T`が非nullで、null許容の方に`?`を付けるという文法を選んだからです。

ということは、
C# 7.x 以前であれば参照型`T`はnullを許容していたわけで、
これまでと`T`の意味が変わります。
そのせいで、最初から`T?`を考えて言語設計していたなら必要なかったであろう苦労が少しあります。
具体的には、以下のような感じになっています。

- 有効にするにはオプション指定が必要
- エラーではなく警告ベース
- 値型の`T?`と扱いが違う
- ジェネリクスに対して使いづらい
- 漏れがあり得る

### オプション指定

何も指定しないと、C# 7.x までの動きと同様になります。
参照型 `T` には null があり得るし、`T?` とは書けません
(警告が出るだけですが)。

null 許容参照型を有効にするには、以下の2通りのオプション指定の方法があります。

- `#nullable`ディレクティブ … ファイルの行単位でオン/オフ切り替え
- `NullableReferenceTypes` タグ … プロジェクト全体でオン/オフ切り替え

前者は、`#if`や`#pragma`などと同じ[プリプロセス命令](../../../../study/csharp/misc/sp_preprocess.md)です。
以下のように、`#nullable`を書いた行から先がオン/オフ切り替わります。

```csharp
#nullable enable
    static void M1(string s)
    {
        // enable 時に string に null を代入したら警告
        s = null;
    }
 
#nullable disable
    static void M2(string s)
    {
        // disable にしたので string に null を代入しても何も言われない
        s = null;
    }
```

これら`enable`、`disable`に加えて、
リリース版までには`restore`と`safeonly`というオプションも入るそうです。
(`restore`は名前通り前の状態に戻すもの。
`safeonly`の方はちょっと[仕様(書きかけ)](https://github.com/dotnet/csharplang/blob/master/proposals/nullable-reference-types-specification.md)を読んだだけだとピンとこなかったので、
実装されたら改めて確認します…)

後者は、csproj に対して設定を書きます。
(最終的にはVisual Studio上の設定画面からもオン/オフができると思いますが、
現状ではcsprojを手書きする必要があります。)

```xml {highlight-lines="7"}
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <LangVersion>8.0</LangVersion>
    <NullableReferenceTypes>true</NullableReferenceTypes>
  </PropertyGroup>
 
</Project>
```

完全に1から書くプロジェクトの場合ならオンになっていて特に困ることもないので、
積極的にこのオプションを指定するといいと思います。
[先日書きましたが](../directorybuild/index.md)、`Directory.Build.props`に書くのもありかもしれません。

### 警告ベース

null 許容参照型がらみの違反は、全て警告になっていて、エラーにはなりません。
オプションでオン/オフできるとは言え既存のC#とは`T`の意味が変わるものなので、エラーにするのは怖いというのがあると思います。
また、後述するように漏れがあり得るので、
もしかするとエラーにしてしまうとまずい状況もあり得るかもしれません。

まあ、C# には「警告をエラーとして扱う」というオプションもあるので、
「null は絶対に許さない。慈悲はない」という方はcsprojに`TreatWarningsAsErrors`を加えるといいと思います。

```xml {highlight-lines="7-8"}
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>netcoreapp3.0</TargetFramework>
    <LangVersion>8.0</LangVersion>
    <NullableReferenceTypes>true</NullableReferenceTypes>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
  </PropertyGroup>
 
</Project>
```

ちなみに、「全部警告にするにしてもオプション指定必須、既定動作ではオフ」なのもこいつのせいです。
例え警告であっても、既存コードに対して警告を起こす変更は破壊的変更になります。

### null 許容“値型”との差

`T?`といえば、C# には、2.0 の頃からnull許容型というものがあります。
本来絶対にnullがない構造体`T`に対して、`T?`と書くことでnull許容にできる機能です。
C# 8.0で「null許容参照型」が入ってしまったので、
これまでの「null許容型」は区別のために「null許容“値型”」と呼び変えるようになると思います。

見た目はどちらも「`T`に対して`T?`でnull許容」ですが、
結構扱いに差があります。

まず根本的な差として、内部的な実装方法が全然異なります。
値型の場合は`T`と`T?`が明確に違う型ですが、
参照型の場合は`T?`も内部的に`T`になっていて、単に C# コンパイラーがフロー解析を頑張るだけになっています。

```csharp
// null 許容値型は Nullable<T> 構造体が作られる
// T と T? は型システムのレベルで別の型
// なので、typeof の結果もことなる
Console.WriteLine(typeof(int) == typeof(int?)); // false
 
// null 許容参照型は C# がフロー解析するだけ
// T? と書いても、内部的には T のまま
// なので、typeof の結果は同じ
Console.WriteLine(typeof(string) == typeof(string?)); // true
```

この余波なんですが、例えば以下のような差が出ます。

```csharp
static void M(string? x)
{
    // 参照型の場合、フロー解析で保証をしている
    // この if を抜けた時点で、x が null でないことが保証される
    if (x == null) return;
 
    // なので、? の付かない string に代入できるようになる
    string y = x;
}
 
static void M(int? x)
{
    // 値型の場合、型自体が違う
    // この if を抜けても x はあくまで Nullable<int> 型
    if (x == null) return;
 
    // なので、「int? を int に暗黙的に変換できません」となる
    int y = x;
}
```

```csharp
// int と int? は別の型なので、オーバーロード可能
void M(int x) { }
void M(int? x) { }
 
// string と string? 型システム上は同じ型なので、オーバーロードできない
void M(string x) { }
void M(string? x) { }
```

### ジェネリクス

ある程度はジェネリックな型に対しても使えます。
例えば、null 許容参照型を表す `class?` 制約というものも追加されます。

```csharp
struct A<T> where T : class
{
    public T Value;
}
 
struct B<T> where T : class?
{
    public T Value;
}
 
class Program
{
    static void Main(string[] args)
    {
        var a1 = new A<string> { Value = null }; // null ダメ
        var a2 = new A<string?> { Value = null }; // string? を渡しちゃダメ
        var b1 = new B<string> { Value = null }; // null ダメ
        var b2 = new B<string?> { Value = null }; // これなら警告が出ない
    }
}
```

しかし、「null許容参照型でもnull許容値型でもいいので、とにかくnull許容 or 非 null」みたいな指定ができません。

```csharp
struct A<T> where T : class // 非 null 参照型
{
    public T NonNull;
    public T? Nullable; // OK
}
 
struct B<T> where T : struct // 非 null 値型
{
    public T NonNull;
    public T? Nullable; // OK
}
 
struct C<T> // 「非 null」だけを指定する手段はない
{
    public T NonNull;
    public T? Nullable; // これが無理
}
```

### 漏れがあり得る

フロー解析には漏れがあり得るそうです。
元々 null だらけな言語をいきなり完全に null のない言語に変えるのは無理でして。

例えば、現状だと以下のようなものすら漏れます。

```csharp
using System;
 
class Program
{
    static void Main()
    {
        // new string[] { null } みたいなのはちゃんと警告になるものの、
        // new string[1] は通っちゃう。
        // 「既定値」が使われるので、null が入る。
        M(new string[1]);
    }
 
    static void M(string[] x)
    {
        foreach (var item in x)
        {
            // 本来は null は絶対来ないはずなものの…
            Console.WriteLine(item.Length); // ぬるぽ
        }
    }
}
```

この例は、将来的には「治る」可能性が高いです。
(徐々にフロー解析を賢くしたい意志はあるし、
この配列の既定値問題は既知の問題。
もしかしたら、リリース版までにはちゃんと警告が出るかもしれません。)

しかし、根本的に拾えないもの、
例えば unsafe コードや native 相互運用が絡むとどうしても解析しきれなくなります。

ということで、
`#nullable`を有効にしたうえで警告をすべて取り切っても、
`NullReferenceException`が出るときは出ます。

## 試してみた感じ

とまあ、完全ではない感じの C# 8.0 の null 解析ですが、
ここからは個人の感想。

実際、職場のコードも含めて、自分の持っているコードに対して `#nullable` を有効にして試してみました。

まあ、とりあえず感想としては、

- 完全ではないとしても元よりは絶対マシ
- オン/オフ混在させてもそんなに違和感はなさそう
  - ちょっとずつ対応させていけばいい感じはちゃんとある

という感じ。

### 警告が出た量

ちなみに、いきなりプロジェクト全体に対して`NullableReferenceTypes`をtrueにしてやった場合どうなるかですが、
大体50～100行に1個くらい警告が出ます。
23万行ほどあるリポジトリに対してやってみたところ、
警告が3800個出ました。

ほとんどは、元々nullを意図していたコードに対してちまちまと`?`を付けて回る簡単なお仕事です。
今までさぼっていた人だと、`if (x != null)` や `?.` を付けて回るお仕事も待っていると思います。

### 普通にやって取れなかった警告

何十個に1個かくらい、真っ当な方法では警告が取れなくて、`!`演算子でご容赦を願ったところもあったりはします。
(いくつかはバグだと思うので、リリースまでには治ることを期待。)

前述のジェネリクスの問題は本当にどうしようもなかったです。
例えば以下のような感じのやつ。

```csharp
class MyDictionary<T>
{
    // 制約なしの T は T? にできないので…
    public T GetValueOrDefault(int key)
    {
        //if (keyで検索) return 見つかったら値を返す;
        return default!; // ! を付けないと警告
    }
}
```

あとは、「null は素通し」形のメソッド。
以下のような感じのコードがあって、結局は `!` に頼りました。

```csharp
class Program
{
    // 引数が null の場合に限り、戻り値も null
    // (例として素通ししているものの、実際のコードは多少の変換コードあり。
    //  ただし、メソッドの先頭で if (s == null) return null;)
    static string? M(string? s) => s;
 
    static void Main()
    {
        // null を与えて null が返ってくるのは想定通り。
        string? s1 = M(null);
 
        // 自分は、この場合に M が null を返さないことを知っているものの…
        // コンパイラーにはわからないので警告が出る。
        string s2 = M("abc");
    }
}
```
