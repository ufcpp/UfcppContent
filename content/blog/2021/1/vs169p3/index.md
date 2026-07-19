---
title: "Visual Studio 16.9 Preview 3 (C# Next チョットある)"
source_url: "https://ufcpp.net/blog/2021/1/vs169p3/"
content_type: "BlogEntry"
published_at: "2021-01-23T14:25:22"
updated_at: "2021-01-23T14:45:19"
tags: []
umbraco_id: 2322
parent_id: 2321
sort_order: 0
aliases: []
---

# Visual Studio 16.9 Preview 3 (C# Next チョットある)

Visual Studio 16.9 Preview 3 が出たということでライブ配信をしていました。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/xS_AlnXMFwo" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

冒頭で話しているんですが、Preview 1 は 16.8 正式リリースと同時だったので 16.8 の方を紹介、
Preview 2 はそんなに大きな変化もなかったので、Preview 3 で初めて 16.9 の話です。

## ポロリ

YouTube 配信前に作ってある[お品書き issue](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/24)のタイトルには「C# Next ポロリもあるよ」とか書いているんですが。
まあ変なタイトルを付けたかっただけで、ポロリといっても「出ちゃいけないものが出ちゃった」みたいなことはないです。

- C# にもちょっとした新 preview 機能追加あったよ
- 9.0 には間に合わなかったものがさらっと merge されたよ

くらいの意味でポロリと言っています。

昨年、C# 9.0 のときは最初に Preview 機能追加があったのは4月のことだったので、小さな機能追加といってもずいぶんフットワークが軽くなったなぁと思います。
ほんとにもうできたものから Preview リリースしていくんだ、と。

### LangVersion Preview

[9:28～](https://www.youtube.com/watch?t=568&v=xS_AlnXMFwo&feature=youtu.be)

復活の [LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) Preview。

今年はほんとに `<LangVersion>latest</LangVersion>` (とか `default`) とは短いおつきあい(2か月ほど)でした。

### Constant Interpolated Strings

[10:18～](https://www.youtube.com/watch?t=618&v=xS_AlnXMFwo&feature=youtu.be)

入ったのは [Constant  Interpolated Strings](https://github.com/dotnet/csharplang/issues/2951)([文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)の const 扱い)です。

```csharp
class Sample
{
    public int A { get; }
    public int B { get; }
    public const string S = $"{nameof(A)} {nameof(B)}";
}
```

文字列補間の `$"{}"` の `{}` の中が全部定数の時、`$"{}"` も定数扱いされるという仕様になりました。

[10:39～](https://www.youtube.com/watch?t=1119&v=xS_AlnXMFwo&feature=youtu.be)

ちなみに、`{}` の中には、たとえ [const](../../../../study/csharp/start/sp_const.md#const) であっても数値とかは受け付けず、const string の入れ子だけを受け付けます。

```csharp
public const string S = $"{123}";
```

[23:00～](https://www.youtube.com/watch?t=1380&v=xS_AlnXMFwo&feature=youtu.be)

なんでかというと、文字列以外の文字列補間は `ToString` を経由していて、その `ToString` はカルチャー依存だから…
コンパイル時に定数化できません。

カルチャー依存の例:

```csharp
using System;
using System.Globalization;
 
var ja = CultureInfo.GetCultureInfo("ja-jp");
var fr = CultureInfo.GetCultureInfo("fr-fr");
//Console.WriteLine(double.Parse(1.234.ToString(ja), fr)); // 例外
Console.WriteLine(double.Parse(1.234.ToString(fr), ja)); // 1234 扱い…
 
Console.WriteLine(double.Parse(1.234.ToString(fr))); // CurrentCulture...
 
System.Threading.Thread.CurrentThread.CurrentCulture = fr;
Console.WriteLine(double.Parse(1.234.ToString(fr))); // CurrentCulture...
```

```console
1234
1234
1,234
```

## IDE Productivity

[33:56～](https://www.youtube.com/watch?t=2036&v=xS_AlnXMFwo&feature=youtu.be)

`#if` にコード補完がかかるように。

[1:13:46～](https://www.youtube.com/watch?t=4426&v=xS_AlnXMFwo&feature=youtu.be)

警告等のツールチップヒントからヘルプページに飛べるように。

[1:36:53～](https://www.youtube.com/watch?t=5813&v=xS_AlnXMFwo&feature=youtu.be)

Source Generator の生成物のファイルが Solution Explorer 上に表示されるように。

[1:39:2～](https://www.youtube.com/watch?t=5942&v=xS_AlnXMFwo&feature=youtu.be)

同じく Source Generator の生成物のファイル、Ctrl + , とかの検索結果に現れるように。

[2:47:00～](https://www.youtube.com/watch?t=10020&v=xS_AlnXMFwo&feature=youtu.be)

XAML からの ViewModel C# コード生成ができるように。

## OR_GREATER

[44:23～](https://www.youtube.com/watch?t=2663&v=xS_AlnXMFwo&feature=youtu.be)

OR_GREATER シンボル…

OR_GREATER に関するアナウンスは特に大々的に行われておらず、
[`#if` のコード補完の話のスクショ](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-v16-9-preview-3/#net-productivity)でしれっとお披露目…

「あるバージョンより新しいものだけで」みたいな条件コンパイルをしたいという話は昔からありまして。
それがついに入りました。

当初予定では「`NET5_0` だけで .NET 5 以降」という扱いで行くつもりだったものの、
.NET 5 リリース直前でダメだしを受けて急遽取りやめ。
その代案として出て来た話がこの「`_OR_GREATER` を付ける」でした。

[52:32～](https://www.youtube.com/watch?t=3152&v=xS_AlnXMFwo&feature=youtu.be)

`NETSTANDARD` には `OR_GREATER` シンボルはないとか…

[56:38～](https://www.youtube.com/watch?t=3398&v=xS_AlnXMFwo&feature=youtu.be)

`NET4` 系(要するに .NET Framework) と `NET5` 系(".NET" に統合後)は不連続なので注意とか…

[59:00～](https://www.youtube.com/watch?t=3540&v=xS_AlnXMFwo&feature=youtu.be)

大人は嘘つきではないのです。間違いをするだけなのです。

## CsWin32

[1:43:10～](https://www.youtube.com/watch?t=6190&v=xS_AlnXMFwo&feature=youtu.be)

ちょっと本題(VS 16.9 Preview 3、C# 10.0)から離れましたが、Win32 P/Invoke の話題でも盛り上がったり…

ちょうどこの配信の日に [win32metadata](https://github.com/microsoft/win32metadata) とか [CsWin32](https://github.com/microsoft/CsWin32/) とかの告知が出ていて、「Twitter とかでみんな盛り上がってるなぁ」とか思っていたら、自分のライブ配信でも盛り上がるなど。

Win32 API の P/Invoke 用のコードを Source Generator で生成してくれるものです。

[2:34:41](https://www.youtube.com/watch?t=9281&v=xS_AlnXMFwo&feature=youtu.be)

Linux マシン上とかでもちゃんとソースコード生成からビルドまでできてちょっと盛り上がりました。
(さすがに実行はできなくて、DllNotFound。)

[2:02:42～](https://www.youtube.com/watch?t=7362&v=xS_AlnXMFwo&feature=youtu.be)

C# の `string` とかからネイティブの `char*` とかへの変換などのいわゆるマーシャリング処理は今まで .NET Runtime 内で動的にやっていたみたいなんですが、この度 Source Generator 化されたことでパフォーマンス的にもちょっとうれしいはずです。
これまでも .NET Native とかではビルド時にやっていたそうなので、それが .NET Native (UWP 向けの AOT)という特定環境向けのビルド ツールに限らない汎用的なものになりました。
