---
title: "小ネタ 小さなオブジェクトのスタック割り当て"
source_url: "https://ufcpp.net/blog/2016/12/tipssmallheapallocation/"
content_type: "BlogEntry"
published_at: "2016-12-26T00:21:00"
updated_at: "2016-12-26T03:01:23"
tags: []
umbraco_id: 2011
parent_id: 1969
sort_order: 25
aliases: []
---

# 小ネタ 小さなオブジェクトのスタック割り当て

今回は、C#や.NETの現在ある機能でも、近い将来入りそうな機能でもないんですが、ちょっとした最適化の話。小ネタというか、いつものピックアップRoslynの亜種というか。「Javaでやってるんだし.NETでも」的な要望にし対して、.NETでは事情が変わるよ、というネタ紹介。

小さくて、かつ、短い期間使われないオブジェクトは、たとえ参照型(通常はヒープ上にメモリ確保)であってもスタック上にメモリ確保してはどうかという案があったりします。

- [CLR/JIT should optimize "alloc temporary small object" to "alloc on stack" automatically #1784](https://github.com/dotnet/coreclr/issues/1784)

例えば、以下のようなコードを考えます。非常に小さい型`Distance`があって、ループの中でそのインスタンスが`new`されています。

```csharp
using System;
using System.Linq;

// ただの double なんだけど、次元をはっきりさせたいために専用の型を作る的なやつ
class Distance
{
    public double Value { get; }
    public Distance(double value) { Value = value; }
}

class Program
{
    static void Main()
    {
        var r = new Random();
        var segments = Enumerable.Range(0, 1000).Select(_ => new Distance(r.NextDouble())).ToArray();

        var sum = new Distance(0);
        foreach (var s in segments)
        {
            // ループの中で new されるのがまずい。
            // ヒープ確保が頻繁すぎる。
            sum = new Distance(sum.Value + s.Value);
        }

        Console.WriteLine($"{sum.Value}");
    }
}
```

Javaでは起こりうる問題で、この、ループの中の`new`で、オブジェクトをヒープではなくスタック上に確保できれば結構なパフォーマンス改善が見込めます。で、そのためコード解析手法(Escape Analysisと言います)は論文になっているし、実際、Java 6で採用されているみたいです。

- Java HotSpot™ Virtual Machine Performance Enhancements: [Escape Analysis](https://docs.oracle.com/javase/8/docs/technotes/guides/vm/performance-enhancements-7.html#escapeAnalysis)

ということで、この最適化は.NETでも有効かどうか。

とりあえず、coreclrチームの中の人曰く「気づいているし、議論もしている。real-worldな事例を挙げてもらえると優先度が上がるかも。人工的なサンプルであれば持っている。」だそうです。

まあ、.NETだとそんな頑張らなくても簡単な回避策があるんで。上記の例で言うと、`Distance`クラスを構造体に変えるだけ。小さくて、かつ、短い期間しか使われないものは構造体にしますからね、最初から。なので、よっぽど大きなインパクトがあるreal-world事例がないと動いてもらえないかも、という感じです。

一応、上記提案ページで挙がった「事例」だと、以下のようなものがあります。

1. enumerator (主にLINQ)
2. 文字列処理
3. immutableデータの構築
4. `new FileInfo(path).Exists`みたいなの

どうかなぁ…これ…

1のLINQに関しては、そもそもEscape Analysisの対象にできないみたいです。メソッドをまたぐとダメ。インライン展開が掛からない限りは解析できないそうですが、LINQはインライン展開される類の処理ではないので無理。

2の文字列処理に関しては、[`Utf8String`](https://github.com/dotnet/corefxlab/tree/master/src/System.Text.Primitives/System/Text/Encoding/Utf8)のような、低アロケーションな文字列型が開発中で、これが入れば負担が激減するかもしれません。
現状の`string`型に対しては効き目があっても、将来的には要らなかったということになる可能性が高いです。

3は、多くが「構造体にすれば解決」になる気もします。

ってことで、この事例の中だと4くらいですかね、効き目がありそうなのは。
Escape Analysisをするにもオーバーヘッドは結構あるわけで、ちょっとメリットを得にくそう。
