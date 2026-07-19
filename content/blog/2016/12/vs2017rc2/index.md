---
title: "Updating Visual Studio 2017 Release Candidate"
source_url: "https://ufcpp.net/blog/2016/12/vs2017rc2/"
content_type: "BlogEntry"
published_at: "2016-12-14T00:02:38"
updated_at: "2016-12-13T15:08:32"
tags: []
umbraco_id: 1995
parent_id: 1969
sort_order: 13
aliases: []
---

# Updating Visual Studio 2017 Release Candidate

先月出たVisual Studio 2017 RC、ちょこっとアップデートが掛かりました。

- [Updating Visual Studio 2017 Release Candidate](https://blogs.msdn.microsoft.com/visualstudio/2016/12/12/updating-visual-studio-2017-release-candidate/)

小ネタ集？そんなの休み休み。

ちなみに、MSDNブログには「前のRCを入れてる人はアップデートの通知が出るはずだからそこからアップデートできる」って書いてあるんですが、うちではなんかトラブって、結局アンインストールからのインストールしなおしでした。
(ASP.NET がらみのVSIXパッケージが1個インストール失敗してた。)

## 新旧 csproj

概ねバグ修正っぽいんですが、個人的にうれしいのは、ようやくだいぶ .NET Core Tooling周りがマシになってきたこと。
昔ながらのcsproj (普通に .NET Framework向けライブラリ作ったり、旧Portable Class Libraryを作ったりしたときのやつ)と新csproj (元々xproj + project.jsonだったもの。 .NET Standardライブラリ作ったり、.NET Coreアプリ書いたりするときのやつ)の混在がやっとまっとうに。
.NET Standardライブラリのプロジェクトを、.NET Frameworkなコンソール アプリとかからプロジェクト参照できるように。

## 言語パック

あと、表示言語を切り替えれるように。最近もうすっかり、普段は英語UIで使っていて、記事を書くときのスクリーンショットとかのためだけに日本語に切り替えたりしてるんですけども、それが前のRCまではできなかったという。しょうがないからCommunity(日本語)とEnterprise(英語)をside by sideに入れてたんですが、それはそれで、スタートメニューにはそのうち片方(後からインストールした方)しか出てこないとか言う別の不具合が。それもやっと解消。

## C# 7

で、C# 7。先月のRCリリースの時点でC# 7の全機能そろったといったな。あれは嘘だ。

えーっと、「C# 7に間に合わせて入れるかどうかまだ迷ってるけども…」と言われていた機能が1個、入ることに決まったようです。
Wildcards(ワイルドカード、万能札)と言われてたものが、Discards(破棄、捨て札)という呼び名に変わりましたが、機能的には同じものです。

要するに、[分解](../../../../study/csharp/datatype/deconstruction.md)や[out var](../../../../study/csharp/resource/sp_ref.md#out-var)で、要らない値のところに `_` を書けば無視できるという機能。
以下のようなコードが書けます。

```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        // 分解と out var では _ が discard 使いになったっぽい
        var (_, sum) = Tally(new[] { 1, 2, 3, 4, 5 });
        (_, var sum2) = Tally(new[] { 2, 3, 5, 7, 11 });
        var (count, _) = Tally(new[] { 1, 3, 5 });

        Console.WriteLine((count, sum, sum2));

        if (int.TryParse("123", out _) && int.TryParse("456", out _))
            Console.WriteLine("successfully parsed");

        // 既存文法に対しては未対応。
        // 計画上は出てたはずだけど、いつ対応するかは不明。
        Func<int, int, int> f = (_, _) => 1;
    }

    static (int count, int sum) Tally(IEnumerable<int> items)
    {
        var count = 0;
        var sum = 0;
        foreach (var x in items)
        {
            count++;
            sum += x;
        }
        return (count, sum);
    }
}
```

これ、今までのC#だと、`_`も有効な識別子なので、1度どこかで使ってしまうと、他の場所では二重定義扱いになってコンパイル エラーになります。
それが、とりあえず分解とout varというC# 7からの新構文な場所では、識別子ではなくてdiscards扱いされるようになりました。

ラムダ式の引数など、既存の文脈でどうするかってのはちょっと悩んでたみたいですけども、
とりあえず、「一度でも値の読み出しがあるなら変数扱い、なければdiscards扱い」みたいにしたいそうです。
ちょっと最終的な判断どうなっているのかまでは把握してないんですが、C# 7リリースまでには入るかも。
