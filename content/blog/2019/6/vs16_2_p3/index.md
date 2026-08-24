---
title: "Visual Studio 16.2 Preview 3、notnull 制約"
source_url: "https://ufcpp.net/blog/2019/6/vs16_2_p3/"
content_type: "BlogEntry"
published_at: "2019-06-26T20:09:16"
updated_at: "2019-06-27T23:31:23"
tags: []
umbraco_id: 2252
parent_id: 2250
sort_order: 1
aliases: []
---

# Visual Studio 16.2 Preview 3、notnull 制約

[Visual Studio 16.1.4](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes#16.1.4) と [Visual Studio 16.2 Preview 3](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.2.0-pre.3.0)が出たみたいです。

見た感じ、大半が不具合の修正っぽい雰囲気。

C# 的には、[16.2.P3 マイルストーン](https://github.com/dotnet/roslyn/milestone/48?closed=1)の履歴的に、そろそろ [null 許容参照型](../../../2018/12/cs8nrt/index.md)の作業が本格化していそうで、「[C# によるプログラミング入門](../../../../study/csharp/index.md)」の対応作業もそろそろやらなきゃ… と身構えていたんですが。
実際に 16.2 Preview 3 を触ってみると、あんまり入ってなさげ。Preview 4 に繰り越されたみたいです。

唯一、動作確認が取れたのが以下の機能。notnull 制約。

```csharp {title="notnull 制約" highlight-text="notnull"}
using System;
 
class C<T>
    where T : notnull
{
    public T Value { get; }
 
    public C(T value) => Value = value;
}
 
public class Program
{
    static void Main()
    {
        var c = new C<string>("");
        Console.WriteLine(c.Value.Length);
 
        var c1 = new C<int>(1);
        var c2 = new C<int?>(1); // 警告あり
        var c3 = new C<string?>(""); // 警告あり
        var c4 = new C<string>(null); // 警告あり
    }
}
```

これも、Preview 3 で対応するつもりがそんなになかったのか、
コンパイルはできるものの Visual Studio 上は未対応(コード補完もハイライトも効かない)な状態です。
(一方で、特にコンパイル エラーになったりはしない。)

## notnull 制約

そういえば、この notnull 制約の話はあんまりこのブログで取り上げていなかったはず。
null 許容参照型周りはちょっと目を離すと結構実装が変わっているんで…

見たまんま、「この型引数は null を認めない」の意味です。
`int` や `string` は受け付けるけども、`int?` や `string?` は受け付けない(警告のみですが)という型制約になります。
null 許容<em>値型</em>とnull 許容<em>参照型</em>を統一的に扱いたいがための仕様。

ただ、以下のようなコードは今のところ受け付けません。
notnull とは… (仕組み上しょうがなさそう。これを受け付けるためには .NET ランタイム側での対応が必要そうで結構な手間。)

```csharp {title="T? とは書けない問題"}
class C<T>
    where T : notnull
{
    // せっかく notnull にしても、T? とは書けない。
    // [return: MaybeNull] という属性ベースの回避策を取る予定。
    public T? X() => default;
}
```

元々は、`where T : object` (`object` に `?` がついてないんだから非 null 扱い)でいいんじゃないかって言われてたんですが、「`object` だと参照型っぽくて値型に使えなさそうな印象がある」という理由で新規キーワード追加になりました。

さらに言うと[当初予定](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-05-15.md) では nonnull だったのが、今回の実装だと notnull (non と not の差)になっていたり。

C# ってあんまりこういう、2単語(not null)をつないだキーワードを採用することが少ないので、ちょっと最終的にもこのまま進むのかわからなかったりはします。荒れそう…
