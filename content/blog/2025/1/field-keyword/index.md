---
title: "field キーワード"
source_url: "https://ufcpp.net/blog/2025/1/field-keyword/"
content_type: "BlogEntry"
published_at: "2025-01-02T22:47:21"
updated_at: "2025-01-02T22:47:21"
tags: []
umbraco_id: 2508
parent_id: 2506
sort_order: 1
aliases: []
---

# field キーワード

「Rosly の [Language Feature Status](https://github.com/dotnet/roslyn/blob/main/docs/Language%20Feature%20Status.md) にこの1・2か月で結構更新かかったね」という話題もたびたびあり、その辺りの話を。

Language Feature Status に並んでいるもののうち、いくつかは preview として現時点でもうすでに取り込まれています。

* field キーワード ← 今日はこれ
* First-class Span
* nameof(T<>)

今(執筆時、Visual Studio 17.13.0 Preview 2.1)の時点でも、
[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) に `preview` を指定すれば利用可能です。

最初は3つまとめて1ブログにしようかと思ってたんですが、
案外長くなったので個別に。
今日は field キーワードの話になります。
([昔のブログ](../../../2023/1/semi-auto-property/index.md)を参照して「やっと入ったよ」だけ書いて終わりかと思ったら案外新規に書くことがあり。)

### <a id="field">field キーワード</a>

プロパティ内において [`field` をキーワード扱い](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md)して、
「プロパティのバッキング フィールドを表す変数」にしようという案があって、
今は機能名としても「field キーワード」と呼ばれています。

```csharp {title="field キーワード"}
class A(int x)
{
    // (既存の)自動プロパティ。
    public int X1 { get; set; }

    // X1 と同じ意味になる「field キーワード持ち」のプロパティ。
    public int X2
    {
        get => field;
        set => field = value;
    }

    // 片方を自動、片方を field 持ちにもできる。
    public int X3
    {
        get; // 自動。
        set => field = value; // field 持ち。
    }

    // 自動プロパティでできたことは一通りこっちでもできる。
    // (イニシャライザーも持てたり、get-only とか init とかも。)
    // (というか、扱いは完全に自動プロパティと同じ。)
    public int X4 { get => field; } = x;

    // get 省略形の => 内でも field が使える。
    // int X5 { get; } と全く同じ。(コンストラクターで初期化可能。)
    public int X5 => field;
}
```

[2年前にすでに「場合によっては C# 11 に入っていたかも」と言っていたもの](../../../2023/1/semi-auto-property/index.md)がようやく C# 14 で入ります。
当時は「半自動プロパティ」(semi-auto properties)とか呼んでいましたが、
結局「field キーワード」で行こうという感じになっているみたいです。

Visual Studio 17.12 Preview 3 / .NET 9 RC 2 の頃にはすでに merge されています。
つまり、C# 13 正式リリース(.NET 9)よりも前に、
すでに C# 14 の preview 機能が取り込まれている状態。
結構長いこと検討していて実装もあるものの、いくつか懸念があって延びに延びていて、
ようやく preview として世に出すことに。

懸念の1つは、これが「そこそこありえる」頻度の破壊的変更になることです。
「`field` という名前のフィールドがあって、`this.` は付けずに、プロパティの中で参照している」という状況が破壊的変更になります。
(「そこまで多くはないけど、まあそういう人も一定数いる」レベル。)

```csharp {title="field キーワードにはそこそこありえる破壊的変更"}
class A
{
    int field;

    public int Field
    {
        // C# 13 まで: field フィールドの参照。
        // C# 14 から: field キーワード。
        //             field フィールドはノータッチになる。
        //             field フィールドを参照したければ @field とか this.field にする。
        get => field;
        set => field = value;
    }
}
```

このコードは一応、C# 14 では警告になる予定です。
「`field` キーワードが `field` フィールドを隠してるけども意図通りか？」と怒られて、`@field` への書き換えを推奨されます。
もしかすると、今年のうち(C# 13 の間)に、「今のうちから `@field` に書き換えておいてくれ」アナライザーが提供されるかもしれません。

ちなみに、プロパティ内において、`field` は完全にキーワードになっています。
当初は「既存のコードを壊さない限りにキーワード扱いする」みたいな努力をするかどうかという話もあったんですが、複雑すぎるので断念しています。
例えば、`field` という名前のローカル変数があったとしてもキーワード扱いです。

```csharp {title="ローカル変数があっても field はキーワード扱い" warning-ranges="sha256:087d924f19fed5037e0159db9583e4951160111eefd2d38b81cc1a7b3936c7b6;7:17-7:22,8:20-8:25"}
class A
{
    public int X
    {
        get
        {
            var field = 1;
            return field; // これは field キーワード。フィールドの場合と同じく警告あり。
        }
    }
}
```

`nameof(field)` もエラーになります。
`nameof(int)` とかがエラーなのと同じ。

```csharp {title="nameof(field) はダメ" error-text="field" warning-text="X"}
class A
{
    public string X
    {
        get => nameof(field); // ダメ。
    }
}
```

(余談で、[`value` もキーワードに変えちゃうか](../../../2024/2/value-as-context-keyword/index.md)という話もあったんですが、これは没になりました。)

これと関連して、以下のようなコードを書くと、タプル要素名のやつだけエラーを起こします。

```csharp {title="タプル要素名とか、匿名型のプロパティとか" error-ranges="sha256:51415403246e24404becd011774f40a5f7fe3780c31658211c488dd69d01ea94;7:27-7:28,7:30-7:31,7:33-7:34"}
class A
{
    public int X
    {
        get
        {
            var x = (field: 1, 2); // タプル要素名 (これだけコンパイル エラー)
            var y = new { field = 1 }; // 匿名型のプロパティ
            var z = new Foo() { field = 1 }; // オブジェクト初期化子でのフィールド/プロパティ参照
            if (y is { field: 1 }) { } // プロパティ パターンでのフィールド/プロパティ参照

            return field;
        }
    }

    class Foo
    {
        public int field;
    }
}
```

最後にもう1つ、[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)のフロー解析の問題があります。
プロパティが `T` のとき、そのバッキング フィールド(`field` キーワードの実体)は `T` であるべきか、`T?` であるべきか。

例えば以下のような `??=` を使った遅延初期化コードはよく書くと思います。

```csharp {title="??= で遅延初期化" warning-ranges="sha256:67771b771afcd9e05fda48d08b1a8c2bd59ec902353b4a3f5612c0bd9f41be07;4:19-4:23"}
class A(Type type)
{
    // Type.Name のキャッシュ。
    public string Name
    {
        // 遅延初期化にしたいので field ??= で代入。
        get => field ??= type.Name;
    }
}
```

現状(Visual Studio 17.13.0 Preview 2.1 時点)、「プロパティが `T` なら `field` も `T`」です。
この例の場合、`string` (not null)。
「not null なフィールドがあるのに、コンストラクターで初期化していない」という警告が出ます。

[解決策](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md#nullability)は検討さいれているんですが、短期的には `MaybeNull` 属性を使って回避してくれと言われています。

```csharp {title="当面、MaybeNull で回避"}
using System.Diagnostics.CodeAnalysis;

class A(Type type)
{
    [field: MaybeNull] // この属性によって、field が string? 扱いになる。
    public string Name
    {
        get => field ??= type.Name;
    }
}
```

上記[解決策](https://github.com/dotnet/csharplang/blob/main/proposals/field-keyword.md#nullability)が間に合うなら、
「いったん `field` が `T?` と仮定してフロー解析して nullable 警告を起こすかどうか」をみてバッキング フィールドが `T` か `T?` かを決定するとのこと。
これが入れば `MaybeNull` を付ける前のコードでも警告が出なくなる予定です。
