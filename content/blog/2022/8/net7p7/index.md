---
title: ".NET 7 Preview 7 で、C# 11 の機能が一通りそろったみたい"
source_url: "https://ufcpp.net/blog/2022/8/net7p7/"
content_type: "BlogEntry"
published_at: "2022-08-13T11:56:03"
updated_at: "2022-08-13T11:56:04"
tags: []
umbraco_id: 2430
parent_id: 2429
sort_order: 0
aliases: []
---

# .NET 7 Preview 7 で、C# 11 の機能が一通りそろったみたい

久々のブログになります。
C# 11 の機能追加があるたびに [YouTube 配信](https://www.youtube.com/c/ufcppdotnet)ではちょくちょく紹介していましたが、
こっちではかなりの久々。

そういえば去年とかは新しい Preview が出るたびに「今回はこの機能が実装されたよ」一覧くらいはブログに書いてたなと思いつつ。
まあ、今年は早い段階から「[C# 11.0 の新機能](../../../../study/csharp/cheatsheet/ap_ver11.md)」の方を埋める作業をしているので、何もしてなかったわけでもないんですが。
ちなみに、「[C# 11.0 の新機能](../../../../study/csharp/cheatsheet/ap_ver11.md)」の方は現在、
[進捗 12/19](https://github.com/ufcpp/UfcppSample/issues/387) です。

## .NET 7 Preview 7 での C#

しばらくブログとしては書いてなかった Preview 版の紹介を、今回久々に書いているのは、
.NET 7 Preview 7 で、

* 予定されている C# 11 の機能が一通り全部入った。今ないものは RC/GA でもない
* `LangVersion` に `preview` を指定しなくても (net7.0 ターゲットならデフォルトで) C# 11 が使えるようになった

という2点から。

去年どうでしたっけ。.NET 6 では RC 1 のタイミングで `preview` が外れてたような…

### C# 11 最終版

とりあえず、[Language Feature Status では一通り「11」の機能がそろいました](https://github.com/dotnet/roslyn/pull/63010)。
まあ、一部、ちょっと修正が入りそうな部分はありますが、大まかな機能としてはもう変更はなさそうな雰囲気です。

「ちょっとした修正」も、例えば以下のようなバグの修正みたいなレベルの話です。

```csharp {title="Preview 7 だと挙動が怪しいやつの例"}
unsafe
{
    // ref フィールドを持ってる構造体、 .NET 7 Preview 7 では managed 扱いされないバグがあるみたい。
    // (ガベコレ的にまずいコード。)
    var a = stackalloc RefInt[4];
}

ref struct RefInt
{
    public ref int Reference;
}
```

### preview 外れた

とりあえず、`TargetFramework` が `net7.0` なプロジェクトは `LangVersion` を書かなくても C# 11 になります。
あと、`LangVersion` を `latest` とかにしているプロジェクトでも C# 11 になります。

注意点として、今回、「`net6.0` と C# 11」みたいな組み合わせにするとちょっと問題を起こすような破壊的変更があったりします。
以下のようなやつで、まあ、めったに踏むようなコードでもないとは思いますが、一応。

```csharp {title="net6.0 + C# 11 でだけ問題を起こすコードの例"}
using System.Runtime.InteropServices;

public struct Buffer
{
    private int _item0;

    // net7.0 なら問題なくコンパイルできる。
    // net6.0 + C# 10 でも問題なくコンパイルできる。
    // net6.0 + C# 11 だとコンパイルエラーになるので注意。
    public Span<int> AsSpan() => MemoryMarshal.CreateSpan(ref _item0, 1);
}
```
