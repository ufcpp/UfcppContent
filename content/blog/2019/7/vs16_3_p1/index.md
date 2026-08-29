---
title: "Visual Studio 16.2 GA と 16.3 Preview 1"
source_url: "https://ufcpp.net/blog/2019/7/vs16_3_p1/"
content_type: "BlogEntry"
published_at: "2019-07-27T15:50:55"
updated_at: "2019-07-27T15:50:55"
tags: []
umbraco_id: 2257
parent_id: 2256
sort_order: 0
aliases: []
---

# Visual Studio 16.2 GA と 16.3 Preview 1

一昨日、Visual Studio 2019 16.2 の Generally Available と 16.3 の Preview 1 が出ました。
あと、.NET Core 3.0 Preview 7 も出ました。

- [Visual Studio 2019 version 16.2 Generally Available and 16.3 Preview 1](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-version-16-2-generally-available-and-16-3-preview-1/)
- [Announcing .NET Core 3.0 Preview 7](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-7/)

16.2 の機能:

- テスト エクスプローラーの UI が見やすくなった

16.3 Preview 1 の機能:

- 起動画面でのプロジェクト検索や、プロジェクト テンプレートの検索がしやすくなった
- .NET Core 3.0 や C# 8.0 のサポート追加
  - `LangVersion` を `8.0` や `preview` にしなくても C# 8.0 が有効

.NET Core 3.0 Preview 7

- Go Live (自己責任で、製品環境で使ってもいい状態)になった
- インストール サイズ改善
  - インストーラーの状態で3割減、インストール後のディスク サイズで 75%減
  - Alpine の Docker イメージのサイズが 148MB に

## .NET Core 3.0/C# 8.0

ようやく、.NET Core 3.0/C# 8.0 が最終形になってきました。
前述の通り、Visual Studio 16.3 ではもう C# 8.0 が default です。
`LangVersion`の明示は要らなくなりました。

.NET Core 3.0 が Go Live になったので、今後、Generally Available になるまであまり変化はないはずです。

素直に「もう完成してる」「バグ修正を除けばもう変化はない」と言えればすっきりするんですけどね…
[null 許容参照型](https://github.com/ufcpp/UfcppSample/issues/255)がらみがどうもまだリリースされ切っていないようで。

(まあ、null 許容参照型の変化は文法には関係なくて、「C# コンパイラーが属性をどう扱うか」で「警告の有無が変わる」というものです。
一応、文法自体はもうさすがに今後 C# 8.0 の正式リリースまでに変化することはないと思います。)

### null 許容参照型

ブログ的に Visual Studio 16.2 Preview 4 (7月19日に出てた)の話はすっ飛ばしちゃいましたが、
16.2 Preview 4 辺りでだいぶ null 許容参照型の実装は進んでいました。
ただ、ちゃんと実装されているようなされていないような…

例えば以下のようなコード。

```csharp {title="MabyNull/NotNull 属性" warning-ranges="10:27-10:28,18:32-18:39,21:47-21:54"}
using System;
using System.Diagnostics.CodeAnalysis;
 
public class Program
{
    static void Main()
    {
        // MaybeNull が付いているので、string だけど null が返ることがある
        var a = MaybeNull<string>();
        Console.WriteLine(a.Length); // 警告
 
        // NotNull が付いているので、string? だけど null は返ってこない
        var b = NotNull<string?>();
        Console.WriteLine(b.Length);
    }
 
    [return: MaybeNull]
    static T MaybeNull<T>() => default; // ただ、ここで警告出ちゃう(出ないのが正しいはず)
 
    [return: NotNull]
    static T NotNull<T>() where T : class? => default;
}
```

`MabyNull`/`NotNull` 属性、使う側は対応しているけど、メソッド定義側が対応していなかったり。
この辺りは Visual Studio 16.3 Preview 1 でも変化なし。

[C# によるプログラミング入門に null 許容参照型のページを追加](../../../../study/csharp/resource/nullablereferencetype.md)し始めてるんですけど、
まだリリースに含まれていない挙動が多くて書きづらい…
(先週こっそり書き始めてこっそりもうページはあるんですけど、背景説明だけで力尽きてる。)

[roslyn 上の issue](https://github.com/dotnet/roslyn/issues/35816) からたどるに、
実装自体はあって、結構 merge 済みのものも多いんですけども。
まだリリースには反映されていないようです。

あと、[今からマイルストーンを 3.0 に変えて大丈夫なの？という issue](https://github.com/dotnet/coreclr/issues/25488)もあったり。
(型引数に対して属性を付けたい、付けれないと null 許容参照型で困るという話。)
