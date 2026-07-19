---
title: "ピックアップRoslyn 3/18: インターフェイスのデフォルト実装"
source_url: "https://ufcpp.net/blog/2017/3/pickuproslyn0318/"
content_type: "BlogEntry"
published_at: "2017-03-18T21:00:51"
updated_at: "2017-03-18T21:00:51"
tags: []
umbraco_id: 2048
parent_id: 2047
sort_order: 0
aliases: []
---

# ピックアップRoslyn 3/18: インターフェイスのデフォルト実装

C#でもインターフェイスのデフォルト実装(まんま、Javaのデフォルト メソッドと同じ機能)の実装始めたって。

- [Enable a default implementation of an interface method to be provided as part of its declaration. #17927](https://github.com/dotnet/roslyn/pull/17927)

結構びっくり…

驚いてるって言っても、この機能が革新的だとかそういう驚きではなくてですね。
この機能、.NET ランタイムに手を入れないと実現できない機能なので、「今から実装始めれるんだ？！」という驚きになります。

## デフォルト実装

機能的にはそんなに驚くこともなく。
というか、本当にJavaのデフォルト メソッドそのままなので。
まあ、今からC#にデフォルト実装の機能を入れても、そこまで大きなインパクトはないと思います。

メリットは、

- インターフェイスに後からメンバーを追加しても、利用側コードを壊さずに済むようになる
- `IEnumerable.GetEnumerator`みたいな、1.0時代の互換性のためだけに現存しているものの実装が楽になる

というくらい。後者の方は要するに、以下のような話。

```csharp
namespace System.Collections.Generic
{
    public interface IEnumerable<out T> : IEnumerable
    {
        IEnumerator<T> GetEnumeartor();

        // こんな風に、デフォルト実装を与えておいてもらう
        IEnumerator IEnumerable.GetEnumeartor() => GetEnumerator();
    }
}

namespace ConsoleApp1
{
    using System.Collections.Generic;

    class MyEnumerable : IEnumerable<int>
    {
        IEnumerator<int> GetEnumerator()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        // IEnumerator IEnumerable.GetEnumeartor の方は書かなくてもいい
    }
}
```

あったらあったで便利なんですが、ランタイム側の修正が必要となるとなかなか。

## .NET ランタイムに手を入れる

C# の機能って、C# 3～7までの間は、コンパイラーが頑張って実現しているような機能がほとんどで、
.NET ランタイム的にいうと .NET Framework 2.0上でも動くものばかりでした。

(参考: [C#の言語バージョンと.NET Frameworkバージョン](../../../../study/csharp/cheatsheet/listfxlangversion.md)
[async/await](../../../../study/csharp/async/sp5_async.md)が[`Task`クラス](https://msdn.microsoft.com/ja-jp/library/system.threading.tasks.task(v=vs.110).aspx)に依存していたり、ライブラリを必要とする機能はちらほらありましたが、
ランタイムに関しては.NET Framework 2.0以降何も変化していません。
)

とはいえ、さすがに、ランタイムに手を入れないとなぁという感じはしてきています。
いくつか、「ランタイムによってコンパイラーの進化が妨げられている」みたいなリストもできていたりします。

- [Places where runtime bugs interfere with compiler or language evolution #251](https://github.com/dotnet/csharplang/issues/251)

見るからに「バグだから直して」って感じのもありますが、
他にも例えば、以下のようなものが挙がっています。

- 構造体に対して引数なしのコンストラクターを認めたい(`Activator.CreateInstance`のバグで今はそれができない)
- 属性にジェネリックなものを認めたい
- インターフェイスにデフォルト実装を認めたい

需要があるんだからランタイムの修正してくれよって話ではあるんですが、
ここ数年の.NETって、クロスプラットフォーム化とかで忙しくて、新機能を追加する余裕なんて全然なかったんですよね。
そりゃまあ、デフォルト実装を認めるよりも先にXamarin (iOS/Android)とか.NET Core (Linux/Mac)とかの安定化の方が先だろと。

ってことで、個人的な予想としては、この手のランタイム修正が必要な機能の実装は.NET Core 2.0とかよりさらに後だろうとか思ってたんですが。
デフォルト実装も、当初予定では[C# 8.0 milestone](https://github.com/dotnet/csharplang/milestone/8)に並べられてたはずですし。
なので、冒頭の「今？！」という驚きにつながるわけです。

今作業が始まったってことは、.NET Core 2.0(一瞬、「今年の春には」とか言う話も出ていましたけど。今年中には出るのかなぁ…出るはずよなぁ…きっと？)とかでひょっとしてランタイムの修正もするのかな？とか期待に胸を膨らませたり。
