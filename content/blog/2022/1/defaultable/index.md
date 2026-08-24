---
title: "【C# 11 候補】defaultable value type"
source_url: "https://ufcpp.net/blog/2022/1/defaultable/"
content_type: "BlogEntry"
published_at: "2022-01-07T21:48:20"
updated_at: "2022-01-07T21:48:20"
tags: []
umbraco_id: 2403
parent_id: 2401
sort_order: 1
aliases: []
---

# 【C# 11 候補】defaultable value type

[`ImmutableArray` に対してコレクション初期化子は使えないという話](../../../2021/12/immutable-array-init/index.md)でちょっと出しましたが、この問題の原因の1つは「[既定値](../../../../study/csharp/resource/rm_default.md)(`default`、0初期化)のまま放置してはいけない型がある」というものです。

`default` 放置問題は「null を null のまま放置してはいけない」という問題に直結するので、
[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)とも関連します。

ということで「[クラスの `null` 解析と同様に、構造体の `default` に関するフロー解析を行う](https://github.com/dotnet/csharplang/discussions/5337)」という提案が前々からあるんですが。
状況としては「提案のドラフトは書いてみたけど、まだ思い悩んでる点があって、Design Meeting に議題を上げる段階にない」みたいな感じです。

## default 放置問題

C# 8.0 で[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)(nullable reference type、通称 NRT)が入って、以下のように、null 参照例外が出そうな箇所にはコンパイル時に警告を出してくれるようになりました。

```csharp {title="C# 8.0 の NRT" warning-ranges="sha256:e3984a0b5135427427982b763c4668c86ec79b6bf61b562b34ea2b836e144653;4:12-4:16,7:19-7:20,13:19-13:20"}
#nullable enable

// 警告: ? が付いてない変数に null を渡してる。
string s = null;

// この行でも警告: s に null が入ってることを認識してる。
Console.WriteLine(s.Length);

// OK
string? n = null;

// 警告: null かもしれないもののメンバー参照してる。
Console.WriteLine(n.Length);

// これなら OK: not null 判定してるのでメンバー参照してももう大丈夫。
if (n is not null) Console.WriteLine(n.Length);
```

この解析は「できる範囲で、できることからやる」みたいな感じなので結構判定漏れもあるんですが。
その判定漏れの中で特に深刻なのが、構造体の `default` を挟んだ場合。

例えば以下のようなコードで、簡単に判定から漏れた null を残せます。

```csharp {title="default を介して null が紛れ込む例"}
#nullable enable

// これは警告にしてもらえる: 非 null な S に null を渡した。
A a1 = new();
Console.WriteLine(a1.S.Length); // OK

// これだと警告が出ない: default に対する解析がまだない(提案段階)。
A a = default;
Console.WriteLine(a.S.Length); // OK じゃないんだけど OK になる

// S は非 null なはず。
record struct A(string S);
```

この問題を一番深刻に踏み抜いてるのが、
最近のブログで何度か出て来た [`ImmutableArray`](../../../2021/12/immutable-array-init/index.md) なわけです。

```csharp {title="ImmutableArray の default によるぬるぽ"}
#nullable enable
using System.Collections.Immutable;

var a = new ImmutableArray<int>();

// コードのぱっと見の印象からすると 0 とか返ってきて欲しい。
// 実際にはぬるぽ発生。
// ぬるぽるんだったら、NRT 警告みたいなの出してほしい(これが課題)。
Console.WriteLine(a.Length);
```

## defaultable value type

この問題に対する解決策、方向性としてはシンプルで、
「参照型に対して null を認めないようにフロー解析する」というのと同じノリで、「値型に対して `default` を認めないようにフロー解析する」というやり方で解決できるはずです。
それが今回説明する[defaultable value type](https://github.com/dotnet/csharplang/discussions/5337) (default 許容値型)。
nullable reference type (null 許容参照型)との対比でこんな名前になっています。

要は、

* `ImmutableArray` みたいな型に対して `default` を渡しているところには警告を出す
* あえて `default` を渡したい箇所には、NRT の `T?` に類する何か(仮に `T~` とか書く)みたいなアノテーションを付ける
  * これが defaultable value type

というもの。

### nullable と defaultable

ただ、まあ、ちょっとややこしいのが nullable と defaultable があるという点。
C# 2.0 の頃から null 許容値型があるので、
null → default → 有効な値 みたいな「2段の無効な値」ができてしまうという問題があります。

```csharp {title="2段の無効な値"}
using System.Collections.Immutable;

// null
// Nullable<T>.HasValue で null 判定。
ImmutableArray<int>? a1 = null;
ImmutableArray<int>? a2 = default; // これは null になる

Console.WriteLine(a1.HasValue); // false

// default
// HasValue は true。
// ImmutableArray.IsDefault みたいな別判定が必要。
ImmutableArray<int>? a3 = new();
ImmutableArray<int> a4 = new();
ImmutableArray<int> a5 = default; // これは new() になる

Console.WriteLine(a3.HasValue); // true
Console.WriteLine(a4.IsDefault); // true

// 有効な値
ImmutableArray<int> a6 = ImmutableArray.Create<int>();

Console.WriteLine(a6.IsDefault); // false
```

これがあるので、defaltable value type に対して `T?` という記法は使えません。
なので提案では<em>仮に</em> `T~` としています。
当初は `T??` みたいな案も出ていたんですが、
[null 合体演算の `??`](../../../../study/csharp/resource/sp2_nullable.md#coalescing)との弁別が(構文解析が重たくなるという意味で)難しいとのこと。

この仮の `~` を使って話を進めると、とりあえず書きたいコードは以下のようなものになります。

```csharp {title="defaultable value type の例 (~ 案)" highlight-text="~" warning-ranges="sha256:c364fcfe5748da99f0aa6b0d647e5d919a51fdf4d35fb2b0ae699778f3dd930a;3:4-3:11,16:23-16:24"}
using System.Collections.Immutable;

m1(default); // 警告
m1(ImmutableArray.Create<int>()); // OK
m2(default); // OK

void m1(ImmutableArray<int> a)
{
    // a に default が入ることはなく、a.Length が有効。
    Console.WriteLine(a.Length);
}

void m2(ImmutableArray<int>~ a)
{
    // a に default が入る可能性があり、a.Length のところに警告を出したい。
    Console.WriteLine(a.Length);

    // 非 default を保証するような仕組みも欲しい。
    if (!a.IsDefault)
    {
        Console.WriteLine(a.Length); // これは OK にしたい。
    }
}
```

### 参照型フィールドで自動判定

この defaultable value types の最大の目的は `ImmutableArray` みたいな、内部に参照型フィールドを持っている場合の null 解析です。

なので、

* 非 null 参照型フィールドを1つでも持っていると「`default` のまま放置してはいけない型」判定になる
* 非 null 参照型フィールドをすべて非 null 初期化した時点で「`default` 状態から脱した」判定になる

という判定を自動的にする予定です。

```csharp {title="非 null 参照型フィールドで自動判定" warning-ranges="sha256:4193c3a87f09bf6c5ad1611012ccffc1442bd30201d63fc7423997387ef032fa;4:19-4:20"}
A a = default;

// 警告: default のまま使った。
Console.WriteLine(a.S);

// OK: S が非 null になった時点で a は非 default。
a.S = "";
Console.WriteLine(a.S);

record struct A(string S);
```

### opt-in

上記の通り、非 null 参照型フィールドを持っている値型は自動的に `default` 解析の対象になるわけですが、
それ以外の構造体でも「`default` 放置するとまずい」というものはあります。

例として挙がってるのは "ハンドル" の類ですが、
要は、ポインターに類する値を `int` とか `IntPtr` で持っているような構造体。
昔からの習慣で、null と同じく「0 なら無効なハンドル値」とすることが多いです。
こういう型は null 許容参照型とほぼ同じが扱いが必要。

こういう型に対して何らかの新構文を追加すべきか、
それとも属性か何かでアノテーションを付けるかはまだ検討の余地がありますが、
仮に属性を使う案でいうと以下のような感じになります。

```csharp {title="属性を使って defaultable value type opt-in" warning-ranges="sha256:fb2c3cf9ab513789108e47098af11c07bbbf462c7547574ce61c6d4689591987;25:4-25:11"}
[MaybeDefault] // 「default 放置はダメ」を表す何らかの属性
public struct BlobHandle
{
    private readonly nuint _value;

    [AllowDefault] // 「このプロパティが true なら非 default」を表す何らかの属性
    public bool IsNil => _value != 0;

    public byte Read() => // ...
}

void M1(BlobHandle~ handle)
{
    if (!handle.IsNil)
    {
        handle.Read(); // ok
    }
}
M1(default); // ok

void M2(BlobHandle handle)
{
    handle.Read();
}
M2(default); // warning
```

ちなみに、属性はこれ専用のものを用意すべきか、
それとも null 許容参照型で使っている `MaybeNull` などの属性をそのまま流用すべきかみたいな点も検討途中です。

### default 演算子

前述の `IsDefault` (`ImmutableArray` が今持ってるやつ)とか `IsNil` (前節の例に挙げた `BlobHandle` のやつ)とかじゃなくて、
`default` 判定専用の演算子定義も必要なんじゃないかという話もあります。

というのも、以下のようなコード(また `ImmutableArray` が起こす問題)を考えます。

```csharp {title="ImmutableArray に対してパターン マッチングでぬるぽる"}
using System.Collections.Immutable;

void m(ImmutableArray<int> a)
{
    // ImmutableArray に対してリスト パターンを使う。
    // パターンマッチングは暗黙的に非 null 判定を含んでいて、たいていの型に対してはぬるぽを起こさない。
    // ところが…
    Console.WriteLine(a is [1, ..]);
}

// こういうのは大丈夫。
m(ImmutableArray.Create(1)); // true
m(ImmutableArray.Create(2)); // false

// これが例外を起こす。
// null チェックに代わる「default チェック」が必要…
m(default);
```

こんな感じで「`default` を放置しちゃダメ」な型に対するパターン マッチングをするにあたって、「null チェック代わりに何か `default` チェックを挟みたい」という要件があります。

で、「何か特定のプロパティを呼ぶ」とかよりは、以下のように、`operator default` みたいなものを書けるようにした方がいいのではないかという案も出ています。

```csharp {title="operator default"}
public struct ImmutableArray<T>
{
    public static bool operator default(ImmutableArray<T> arr) => arr._array is null;
}
```

## 課題

NRT で問題を起こしている以上、defaultable value type みたいなフロー解析が必要なこと自体はもう分かっているわけですが。
話が進まないのはまだ悩ましい点が残っているから。

特に悩ましいとされるのが2点あって、以下のようなものです。

* プロパティはどうするか
* NRT 並みに「既存コードの移行作業」に手間が掛かる

### プロパティ

[`record struct`](../../../../study/csharp/cheatsheet/ap_ver10.md#record-struct)では、メンバーは(フィールドではなく)プロパティで作られます。
例えば、`record struct A(string S);` と書くと、`S` はプロパティです。

この場合、「すべての非 null 参照型フィールドを初期化していれば非 `default`」の判定をどうするかという問題があります。
プロパティ `S` 越しにそのバッキングフィールドを初期化することになるわけですが、プロパティとフィールドの紐づけができないとフロー解析できません。

### 既存コードの移行

null 許容参照型を導入するときもかなり苦労しました。
.NET の標準ライブラリに null アノテーションを付けて回る作業には2年くらい掛かっています。

しかも、既存コードを壊さないように、「null 解析をするかどうか」は [opt-in](../../../../study/csharp/resource/nullablereferencetype.md#opt-in) (明示的にオプション指定しない限り有効化されない)になっていて、「オプションの有無で2種類の C# がある」といってもいいような状況になっています。
(C# チームもこれを好ましいとは思っていないので、
null 許容参照型はそれだけ「無理してでも必要」とされる唯一の機能です。)

defaultable value type ではこの「アノテーション追加」と「opt-in」をもう1度やる必要があります。
