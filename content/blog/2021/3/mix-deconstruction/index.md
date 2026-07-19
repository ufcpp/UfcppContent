---
title: "ピックアップRoslyn: 分解時の宣言と変数の混在"
source_url: "https://ufcpp.net/blog/2021/3/mix-deconstruction/"
content_type: "BlogEntry"
published_at: "2021-03-05T21:47:26"
updated_at: "2021-03-05T21:55:39"
tags: []
umbraco_id: 2337
parent_id: 2336
sort_order: 0
aliases: []
---

# ピックアップRoslyn: 分解時の宣言と変数の混在

[.NET 6 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-net-6-preview-1/)とか [Visual Studio 16.9 正式版＆ 16.10 Preview 1](https://devblogs.microsoft.com/visualstudio/vs2019-v16-9-and-v16-10-preview-1/)とかが出ましたね。

というの、ライブ配信はしてたんですが。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/y7kqEYov5ro" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/99ek2n6F_1U" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

その中で、今日は C# 10.0 候補で、すでに Visual Studio 16.10 にマージ済みの機能の紹介。
以下のようなコードがコンパイルできるようになっています。

```csharp
int x;
(x, var y) = (1, "abc");
```

配信では言ってるんですが、 .NET 6 Preview 1 が出た時点で、コマンドライン (dotnet コマンド)ではコンパイルできていました。
「今回、.NET SDK と Visual Studio で2週間くらいリリースタイミング違うんですね」とか「Visual Studio は [Ignite](https://myignite.microsoft.com/home) のために取っといたんですかね」とかいう感じ。

で、Visual Studio の方は、16.10 の方で「[LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#langversion)」にした時だけ上記コードがコンパイルできます。

## <a id="deconstruction">分解(C# 7.0)</a>

[分解](../../../../study/csharp/datatype/deconstruction.md)という機能自体は C# 7.0 の頃に入っています。
以下のようなコード、どれも C# 7.0 として有効。

1つ目。`()` 内で変数宣言。

```csharp
(int x, string y) = (1, "abc");
```

2つ目。これを型推論(var)で書いたもの。型推論してる点以外は1つ目のコードと同じ。コンパイラーの解釈結果は全く同じです。

```csharp
(var x, var y) = (1, "abc");
```

3つ目。タプル変数宣言。頭に1個だけ `var` を書いて、複数の変数の宣言をまとめてやる構文。

```csharp
var (x, y) = (1, "abc");
```

4つ目。既存の変数を使って分解。

```csharp
int x;
string y;
(x, y) = (1, "abc");
```

## <a id="mix-deconstruction">混在分解(C# 10.0)</a>

で、これの実装時点で、変数宣言と既存変数の混在についても検討はされていました。
「何か地雷を踏みそうで怖い」みたいな感じで「後でやる」扱い。

それが今回、16.10 Preview 1 でマージされました。
以下のコードが通ります。

```csharp
int x;
(x, string y) = (1, "abc");
```

変数宣言には `var` も使えて、それが冒頭のコードになります。

```csharp
int x;
(x, var y) = (1, "abc");
```

欲しいかと言われると微妙なライン… と感じますが、
実装負担がほとんどなかったみたいですね。

「あえてエラーにしてたけど、そのあえてエラーにする行を消すだけで動く」というレベルだったみたいで。
コミュニティ(C# チームの外の人)から「[エラー行を消して、テストを足しといたよ](https://github.com/dotnet/roslyn/pull/44476)」っていう pull request が出ていました。

pull request をみるに、式ステートメントと `for` の初期化式(1項目)中でだけ認めるみたいです。

```csharp
int x;
 
// OK な例1
(x, string y1) = (1, "abc");
 
// OK な例2
for ((x, string y2) = (1, "abc"); false;)
{
}
 
// ダメな例1
var t = (x, string y3) = (1, "abc");
 
// ダメな例2
m(out (x, string y4));
void m(out (int, string) t) => t = (1, "abc");
```

ちなみに、コミュニティ貢献であってもレビューのコストは掛かるわけで、
この手の pull request が常にうまくいくわけではないんですが。
今回に関しては [pull request 作者さん](https://github.com/YairHalberstadt)が元々 [charplang](https://github.com/dotnet/csharplang/)/[roslyn](https://github.com/dotnet/roslyn) への貢献が大きい人なのと、
本当に「ほぼテストを足しただけ」レベルの修正だったからあっさりと通ったんじゃないかなという感じはします。
