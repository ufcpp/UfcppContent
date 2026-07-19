---
title: "MemberNotNull (値型) 判定"
source_url: "https://ufcpp.net/blog/2022/1/member-not-null/"
content_type: "BlogEntry"
published_at: "2022-01-11T20:34:58"
updated_at: "2022-01-11T20:34:58"
tags: []
umbraco_id: 2408
parent_id: 2401
sort_order: 3
aliases: []
---

# MemberNotNull (値型) 判定

こないだ、[null フロー解析]と似たノリで、[構造体の default フロー解析が必要](../defaultable/index.md)という話をしました。

まあ、難航しそうではあるんですが…

とはいえ実は現在でも、「null チェックといいつつ、構造体に対しても働くフロー解析」があったりします。

## MemberNotNull

[nullable enable](../../../../study/csharp/resource/nullablereferencetype.md#opt-in) のとき、
非 null 参照型のフィールドやプロパティは、
コンストラクター内でちゃんと初期化する必要があります。

例えば以下のコードはプロパティ定義の行に警告。

```csharp
class C
{
    public string S { get; } // CS8618 警告
}
```

以下のようにコンストラクターを足すと、今度はコンストラクターの行に警告。

```csharp
class C
{
    public string S { get; }
    public C() { } // CS8618 警告
}
```

以下のように書くと警告は消えるんですが、

```csharp
class C
{
    public string S { get; }
    public C()
    {
        S = "値は適当"; // これで警告が消える。
    }
}
```

これをメソッド抽出してしまうと再び警告が出ます。

```csharp
class C
{
    public string S { get; private set; }

    public C() // 再び CS8618
    {
        Initialize();
    }

    private void Initialize()
    {
        S = "値は適当";
    }
}
```

null 許容参照型の初期リリースではこの問題を回避する手段はなかったんですが、後々、[`MemberNotNull`](../../../../study/csharp/resource/nullablereferencetype.md#MemberNotNull) という属性が追加されていて、
以下のように書けば警告をなくすことができるようになりました。

```csharp
using System.Diagnostics.CodeAnalysis;

class C
{
    public string S { get; private set; }

    public C()
    {
        Initialize();
    }

    [MemberNotNull(nameof(S))]
    private void Initialize()
    {
        S = "値は適当"; // 逆に、この行を消すと CS8774 警告。
    }
}
```

## 値型に対して MemberNotNull

そしてここでようやく本題。

`MemberNotNull` なんて名前をしていますが、
実際には「値を代入したかどうか」を見ているようで、
値型に対しても使えたりします。

```csharp
using System.Diagnostics.CodeAnalysis;

class C
{
    public DateOnly D { get; private set; }
    public C() = > Initialize();

    [MemberNotNull(nameof(D))]
    private void Initialize()
    {
    } // CS8774
    // member not "null" と言いつつ、非 null が確定している値型に対してもフロー解析してる。
}
```

「代入したかどうか」しか調べてない雰囲気？

代入さえされていれば `D = default;` でも警告が消えたりします。
(C# 10.0 時点では。)

```csharp
using System.Diagnostics.CodeAnalysis;

class C
{
    public DateOnly D { get; private set; }
    public C() => Initialize();

    [MemberNotNull(nameof(D))]
    private void Initialize()
    {
        D = default; // これでも OK。
    }
}
```

ということで、[defaultable value type](../defaultable/index.md) の仕様が入るまではまだ機能不足ではあるんですが。
とりあえず、`MemberNotNull` に対して値型のプロパティを渡せなくするみたいな処理をあえて入れたりはしていないようです。
将来的に defaultable value type のフロー解析もあるだろう見込みがあるのではじかないようにしてあるんじゃないかなぁと思います。
