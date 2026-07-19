---
title: "C# 9.0 in .NET 5 Preview 4 (Build でのリリース)"
source_url: "https://ufcpp.net/blog/2020/5/cs9net5p4/"
content_type: "BlogEntry"
published_at: "2020-05-22T20:54:03"
updated_at: "2020-05-22T20:54:03"
tags: []
umbraco_id: 2296
parent_id: 2290
sort_order: 4
aliases: []
---

# C# 9.0 in .NET 5 Preview 4 (Build でのリリース)

一昨日、C# 9.0 の話を動画配信してたわけですが。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/fZxRZgWYqq0" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

[Microsoft Build](https://mybuild.microsoft.com/) に合わせていろいろとブログ公開が公開されてました。

- [Announcing .NET 5 Preview 4 and our journey to one .NET](https://devblogs.microsoft.com/dotnet/announcing-net-5-preview-4-and-our-journey-to-one-net/)
- [Releasing Today! Visual Studio 2019 v16.6 & v16.7 Preview](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-v16-6-and-v16-7-preview-1-ship-today/)
  - [16.6 Release Notes](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes#16.6.0)
  - [16.7 Preview 1 Release Notes](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.7.0-pre.1.0)
- [Introducing .NET Multi-platform App UI](https://devblogs.microsoft.com/dotnet/introducing-net-multi-platform-app-ui/)
- [Windows Forms Designer for .NET Core Released](https://devblogs.microsoft.com/dotnet/windows-forms-designer-for-net-core-released/)
- [Announcing Entity Framework Core 5.0 Preview 4](https://devblogs.microsoft.com/dotnet/announcing-entity-framework-core-5-0-preview-4/)
- [ML.NET Model Builder is now a part of Visual Studio](https://devblogs.microsoft.com/dotnet/ml-net-model-builder-is-now-a-part-of-visual-studio/)
- [Windows Terminal 1.0](https://devblogs.microsoft.com/commandline/windows-terminal-1-0/)
- [Windows Package Manager Preview](https://devblogs.microsoft.com/commandline/windows-package-manager-preview/)
- [Introducing WinUI 3 Preview 1](https://blogs.windows.com/windowsdeveloper/2020/05/19/introducing-winui-3-preview-1/)
- [The Windows Subsystem for Linux BUILD 2020 Summary](https://devblogs.microsoft.com/commandline/the-windows-subsystem-for-linux-build-2020-summary/)
- [DirectX is coming to the Windows Subsystem for Linux](https://devblogs.microsoft.com/directx/directx-heart-linux/)
- [Expanding Visual Studio 2019 support for Visual Studio Codespaces](https://devblogs.microsoft.com/visualstudio/expanding-visual-studio-2019-support-for-visual-studio-codespaces/)
- [Live Share, now with chat and audio support!](https://devblogs.microsoft.com/visualstudio/live-share-now-with-chat-and-audio-support/)

結構いろんな方向に手を出してるなぁという感じで、あんまり事細かに全部は見れていないんですが。
というか、全部見てるといくらでも時間が溶けるというか。
C# 9.0の話だけでもお腹いっぱいというか。
動画配信でも当然のようにほぼ C# 9.0 の話だけで2時間くらいになっています。

C# チームによる C# 9.0 のブログも投稿されています。

- [Welcome to C# 9.0](https://devblogs.microsoft.com/dotnet/welcome-to-c-9-0/)

ちなみに、C# チームのブログは「C# 9.0 は最終的にこうなる予定」という内容で、
今現在は動かない機能が結構あります。
(というか、9.0 の主役なのでブログのボリュームを割かれている Records が、
まだまだ絶賛作業中で仕様レベルでも完成形になっていないです。)

一方で、配信で話した＆今日ここで書くのは .NET 5 Preview 4 に merge された新機能についてです:

- native int
- target-typed new
- pattern V3

これらを動かすためには相変わらず [`LangVersion`](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) preview 指定が必要になります。
(9.0 指定はまだできません。)

配信前に用意したコード:

<div>
<script src="https://gist.github.com/ufcpp/3b0f78de250dc94243bd8015250779b0.js"></script>
</div>

[配信中に書きちらかした結果](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/7#issuecomment-631462252)

## native int

native int は、
32ビットCPUでは32ビット整数(`int`, `uint`)、
64ビットCPUでは64ビット整数(`long`, `ulong`)
になる整数型です。

これまでも `IntPtr` と `UIntPtr` がそういう性質を持つ型だったんですが、
名前通り[ポインター用](../../../../study/csharp/interop/sp_unsafe.md#about-pointer)であって、
普通の整数演算(加減乗除とか)には使いにくかったです。
それが C# 9.0 で、

- `nint`、`nuint` というキーワードを用意
- このキーワードを使って作った変数の場合、`int` とかと同じ整数演算が使えるようになる

という状態になります。
ちなみに、実体(コンパイル結果)としては `IntPtr` と `UIntPtr` に翻訳されるみたいです。

まあ、CPU の違いを意識したコードを書かないといけない人と言うのはあまり多くないはずなので、
`nint`、`nuint` もあまり多くの人が使う機能ではないと思います。

## target-typed new

ターゲットの型から推論できる場合、`new T()` の `T` を省略できて、`new()` と書ける機能です。
ちょっとした機能ですが、もしかするとこれからお世話になる度合いでいうと C# 9.0 の中で一番多いかもしれません。
(C# 7 世代でも、個人的には[target-typed default](../../../../study/csharp/resource/rm_default.md#default-expr)の利用頻度が非常に高かったです。)

`var` (ソース側からの推論)よりもターゲット側からの推論が有効なのは以下のような場面です。

1つはフィールドとかプロパティの初期化子。

```csharp
using System.Collections.Generic;
 
class Sample
{
    private static Dictionary<int, string> _cache = new();
}
```

もう1つはメソッドの引数。

```csharp
static void M(Dictionary<string, string> options) { }
 
static void Main()
{
    M(new()
    {
        { "define", "DEBUG" },
        { "o", "true" },
        { "w", "4" },
    });
}
```

特に、ジェネリックな型でフルネームを書くと長ったらしくなるものに対して有効で、
ここで挙げた例みたいに `Dictionary` に対して使うことが多くなるんじゃないかなと思います。

## pattern V3

C# 7.0 から脈々と、ちょっとずつ拡充されてきた[パターン マッチング](../../../../study/csharp/datatype/patterns.md)なんですが、一応、9.0 で最終形になります。

まあ、7.0 と 8.0 では見送られる程度には<em>複雑な割に需要が低い</em>パターンなので、
そんなに使う機会はないかもしれません。

そもそも、パターン自体が Roslyn チームの「内需」なんじゃないかという感じもありますし。
(コンパイラーとか書いてるとパターン マッチが欲しい場面が多々あります。
`is T` 分岐だらけですし、[`A`～`z`の `case` が並んでいる嫌な感じの `switch`] とかが平然と出てくるので。)

ただ、C# 9.0 の主役である [Records](https://github.com/dotnet/csharplang/issues/39) の発展形として、

- [Discriminated Unions](https://github.com/dotnet/csharplang/issues/113)
- ["Closed" type hierarchies](https://github.com/dotnet/csharplang/issues/485)

みたいな話があって、これが入るとパターン マッチングの需要がちょっと上がるかもしれません。

また、複雑なパターンはたぶん書かないという人にとっても、以下の2つのパターンは便利だと思います。

- simplified type pattern
- not pattern

前者は、以下のように、`_` なしで型パターンを使えるというもの。

```csharp
static int M(object obj) => obj switch
{
    // C# 8.0 までだと _ が必須だった。
    // 型名だけだと定数パターンとの区別がつかないため。
    short _ => 1,
    int _ => 1,
    long _ => 1,
 
    // C# 9.0 では型名だけで型パターンにできるようになった。
    // 文脈を読んでくれてる(型名だったら型パターン、型がなければ定数パターンとして解釈)。
    ushort => 2,
    uint => 2,
    ulong => 2,
};
```

後者は名前通り「パターンを満たさないとき」用の構文です。
たぶん、`not null` が一番使うと思います。

```csharp
static int M(object obj) => obj switch
{
    // string 型のインスタンスじゃないとき
    not string => 1,
    // null じゃないとき
    not null => 2,
};
```

ちなみに、「こんな変な文法を追加しなくても、既存の構文で `if` とか `when` とか並べればいいんじゃないのか？」と思うかもしれませんが、パターン マッチングは結構賢くて、
ちゃんと条件が網羅的かどうかの判定をやってくれます。

```csharp
static int Invalid(object obj)
{
    if (!(obj is string)) return 1;
    if (!(obj is null)) return 2;
 
    // null の時は1つ目の if に引っかかっているはずで、
    // 2つめ if と合わせると全条件網羅してる。
    // なので、ここには到達できないはずだけど、if だと到達判定が効かない。
    // このメソッドはコンパイル エラーを起こす。
}
 
// パターンを使うと網羅性の判定が正しく動く。
// このメソッドはエラーにもならないし警告も出ない。
// 逆に、網羅できてない場合は警告が出る。
static int Valid(object obj) => obj switch
{
    not string => 1,
    not null => 2,
};
```

あと、数値の範囲を表すパターンとして `min..max` を使いたいという意見も結構あるんですが…
「両端を含む・含まない」の区別が紛らわしすぎるということで愚直に `<`、`<=`、`>`、`>=` を使うということになりました。

```csharp
static int M(byte b) => b switch
{
    >= 0 and <= 10 => 1, // 0 も 10 も含んで [0, 10] の範囲
    > 10 and <= 20 => 2, // 10 は含まず 20 は含んで (10, 20] の範囲
    > 20 and < 30 => 3, // 20 も 30 も含まず (20, 30) の範囲
    >= 30 and < 40 => 4, // 30 は含んで 40 は含まず [30, 40) の範囲
    _ => 0,
};
```

ちなみに、比較パターンでも網羅性のチェックが働いています。

```csharp
// これは無警告
static int M(byte b) => b switch
{
    >= 0 and <= 250 => 1,
    251 or 252 or 253 or 254 => 2,
    255 => 3,
};
 
// 例えば以下の3つには警告が出る
static int M1(byte b) => b switch
{
    >= 0 and < 250 => 1, // <= と間違えて < を書いて、250 が漏れてる
    251 or 252 or 253 or 254 => 2,
    255 => 3,
};
static int M2(byte b) => b switch
{
    >= 0 and <= 250 => 1,
    251 or 253 or 254 => 2, // 252 が漏れてる 
    255 => 3,
};
static int M3(byte b) => b switch
{
    >= 0 and <= 250 => 1,
    251 or 252 or 253 or 254 => 2,
    // 255 が漏れてる
};
```

この辺りのパターンの最適化の掛け方とか網羅性のチェックは、
先達となるプログラミング言語があって割と十分に検証されているらしく、
「需要は高くないけど、検討コストも低いから go サインが出た」という類になります。
