---
title: "nullable 警告もみ消し(来年までの我慢)の手段"
source_url: "https://ufcpp.net/blog/2022/1/nullable-property-disable/"
content_type: "BlogEntry"
published_at: "2022-01-28T20:49:46"
updated_at: "2022-01-28T20:50:52"
tags: []
umbraco_id: 2410
parent_id: 2401
sort_order: 5
aliases: []
---

# nullable 警告もみ消し(来年までの我慢)の手段

今日はとあるアンケートの結果を乗せておこう的な話。

## 背景: 非 null プロパティの初期化

[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)の仕様が入って以来、以下のようなコードに警告が出るようになりました。

```csharp
class A
{
    public string X;
    public string Y { get; set; }
    public string Z { get; init; }
}
```

C# 10.0 現在、この警告を回避する唯一の方法は「ちゃんとコンストラクターで初期化すること」です。

```csharp
class A
{
    public string X;
    public string Y { get; set; }
    public string Z { get; init; }

    public A(string x, string y, string z) => (X, Y, Z) = (x, y, z);
}
```

困るのが、「このプロパティは `new() { X = "", Y = "", Z = "" }` みたいに初期化子で初期化したい」という場面。
結構あると思うんですよね、コンストラクターを定義したくない・できないとき。
今のところいい解決策がない状態です。

### 【将来予定】 required

一応補足。
将来的には解消する予定です。
今のところ C# 11.0 目標で、`required` 修飾子を付けるという案が進められています。

```csharp
class A
{
    public required string X;
    public required string Y { get; set; }
    public required string Z { get; init; }
}
```

これが付いていると、オブジェクト初期化子で非 null な値を渡すことを義務付けられるようになるので、クラス定義側には警告が出なくなります。

```csharp
var a1 = new A(); // required プロパティ/フィールドに値を与えていないので警告

var a2 = new A()
{
    X = null, // null を与えたので警告
};

// required プロパティ/フィールド全てにちゃんと値を与えたのでOKに
var a3 = new A()
{
    X = "",
    Y = "",
    Z = "",
};
```

## 現状の回避策

ということで、`required` によって来年には根本解決の当てがあるわけですが。
そうなると、今現在 `A` の作者が頑張って回避策を取る必要もないよなぁ…
ということになって、
「来年まではやっつけ対処でもみ消ししとこう」という発想になります。

ただ、
こういうやっつけほど具体的にどう対処しようか迷います。
また、もみ消しにいくつかの手段があるのでその点もちょっと迷うポイント。

ということでアンケート。

### 選択肢

選択肢1. 該当行を `nullable disable`

```csharp
class A
{
#nullable disable warnings
    public string X;
    public string Y { get; set; }
    public string Z { get; init; }
#nullable restore warnings
}
```

選択肢2. 該当行を `pragma warning disable`

```csharp
class A
{
#pragma warning disable CS8618
    public string X;
    public string Y { get; set; }
    public string Z { get; init; }
#pragma warning restore CS8618
}
```

選択肢3. とりあえず `default!` や `null!` を代入

```csharp
class A
{
    public string X = null!;
    public string Y { get; set; } = null!;
    public string Z { get; init; } = null!;
}
```

選択肢4. ノーガード。警告出っぱなしなのをあきらめる

```csharp
class A
{
    public string X;
    public string Y { get; set; }
    public string Z { get; init; }
}
```

### 結果

C# 配信中にこの話題になり、
配信真っ最中に Twitter アンケートを作って投票してもらったり。

[https://twitter.com/ufcpp/status/1434168597060853760](https://twitter.com/ufcpp/status/1434168597060853760)

`!` でもみ消し派が35%くらいでちょっと多めですね。
まあ思ったよりは差が広がらず。
