---
title: "partial メソッドの拡張 (C# 9.0 候補機能)"
source_url: "https://ufcpp.net/blog/2020/6/extendingpartialmethod/"
content_type: "BlogEntry"
published_at: "2020-06-08T21:39:51"
updated_at: "2020-06-08T21:39:51"
tags:
  - "C# 9.0"
umbraco_id: 2298
parent_id: 2297
sort_order: 0
aliases: []
---

# partial メソッドの拡張 (C# 9.0 候補機能)

もう1週間近く経過しちゃってるんですけども、Visual Studio 16.7 Preview 2 が出ています。

- [Visual Studio 2019 v16.7 Preview 2 Available Today!](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-v16-7-preview-2/)

で、今日はこの Preview 2 で追加された C# 9.0 候補の話です。
[先月追加された Srouce Generator](../../5/sourcegenerator/index.md)関連の機能で、
[partial メソッド](../../../../study/csharp/oop/oo_class.md#partial_method)の亜種が追加されました。

[先月、Design Notesの時点で軽く触れていた機能](../../5/pickuproslyn0503/index.md)が、この度実際に触れる状態になっています。

## (既存の) partial メソッド

意外と知られていない機能みたいなので改めて、既存の [partial メソッド](../../../../study/csharp/oop/oo_class.md#partial_method)自体についても説明。

一番の用途としては、自動生成されているコードに、一部分だけ手動で処理をカスタマイズしたいみたいなときに使います。
例えば、以下のようなソースコードを T4 テンプレートなどを使って生成していると考えてください。

```csharp
// T4 テンプレートとか XAML とかから自動生成されている想定のコード
partial class Sample
{
    public int X
    {
        get => _x;
        set
        {
            OnXChanging();
            _x = value;
            OnXChanged();
        }
    }
    private int _x;
 
    // 既定動作としては何もしたくない。
    // 人手で、何かしら _x = value; の前後に処理を挟みたいときにはこれに実装を持たせる。
    partial void OnXChanging();
    partial void OnXChanged();
}
```

一部分だけカスタマイズしたいからといって、このコードに手作業で修正を加えてしまうと、
再度自動生成がかかったタイミングで上書きされて消えてしまいます。
そこで、カスタマイズする可能性のある部分に partial メソッド(`partial` 修飾子を付けて、中身を持たない空っぽのメソッド)を挟んでおきます。

もしも、手作業カスタマイズが必要なら、以下のように、別ファイルで partial メソッドに実装を与えます。

```csharp
// 手書きする想定のコード
partial class Sample
{
    partial void OnXChanged()
    {
        System.Console.WriteLine("X: " + X);
    }
}
```

今回上げた例では、`OnXChanging` と `OnXChanged` という2つの partial メソッドがありますが、
そのうち `OnXChanged` の方にだけ実装を与えています。
`virtual` とか `abstract` とかとは違って、以下のような挙動をします。

- 必要ないなら実装を与えなくていい
- 実装を与えていない場合、コンパイル結果から完全に消滅する
  - メタデータ(リフレクションで取れるメソッド情報)すら残さないのでノーコスト
- 実装を与えた場合でも、通常のメソッド扱い
  - `virtual` を付けた場合と違って、インライン展開が効いたりして負担が少ない

C# 2.0 の頃からある機能なんですけども、
言われてみると利用場面が少ないというか。
T4 なりなんなり、コード生成を多用している人にしか目につかないのかなと思います。
Entity Framework の Scaffolding とかで使われているはず…

ただ、まあ、[dotnet/runtime](https://github.com/dotnet/runtime)とかで用例を探してみたものの、テストを除けば数十件くらいしか出てこないんですよね。確かにレア機能。
しかも、自動生成のコードと手書きコードの橋渡しと言うよりも、プラットフォーム依存な処理の分離に使われてることの方が多そう。

```csharp
partial class Sample
{
    public void M()
    {
        OnMBegin();
 
        // 全プラットフォーム共通処理
    }
 
    partial void OnMBegin();
}
 
// Sample.Windows.cs みたいなファイル
partial class Sample
{
    partial void OnMBegin()
    {
        // Windows 限定処理
    }
}
```

## 新 partial メソッド

で、C# 9.0 で追加されるのは逆向きの用途で使うものです。
手書きの方が先にあって、その実装は Source Generator に埋めてもらうという想定。

例えば以下のような書き方をします。

```csharp
// 手書きコード側
using System;
 
partial class Sample
{
    [Utf8("abcd")]
    public partial ReadOnlySpan<byte> M();
}
```

2.0 の頃からある partial との違いは以下の通りです。

- [アクセシビリティ](../../../../study/csharp/oop/oo_conceal.md#level)の指定をする
  - アクセシビリティの有無でどちらの partial メソッドなのかの分岐をしてる
- 戻り値が `void` でなくてもいい
  - アクセシビリティなしの partial メソッドは `void` しか受け付けない
- [`ref` 引数](../../../../study/csharp/resource/sp_ref.md#sec-byref)や [`out` 引数](../../../../study/csharp/resource/sp_ref.md#out)を持てる
  - 同上
- 必ず1個、実装を与えないといけない
  - 手書きの C# コードを起点にして、Source Generator で実装を生成する想定

例えば上記の例は、Source Generator を使って以下のようなコードを生成する想定で書いています。

```csharp
// Source Generator で生成する想定のコード
partial class Sample
{
    public partial ReadOnlySpan<byte> M()
        => new byte[] { 0x41, 0x42, 0x43, 0x44, };
}
```

アクセシビリティの有無で挙動が違うっていうのはそこそこ気持ち悪くはあるんですが。
元々の partial メソッド自体がほとんど使われていませんし、
このためだけに新しいキーワードを追加するほどではないのかなと思います。

## おまけ

ちなみに、今回例に挙げている Source Generator のコードなんですが、
実際に作ってみたもので、
文字列を .NET の文字列リテラル(UTF-16 になる)ではなくて、UTF-8 のバイト列としてプログラムに埋め込むための Source Generator です。
(ちょっとコード整理して、GitHub に上げたら改めてブログに書こうかと思っています。)

昨日、こいつの紹介でちょっと動画配信したりしてたんですけども。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/1Ic2VN3I5vI" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

三人寄れば文殊の知恵というか、主に[あえとすさん](https://twitter.com/aetos382)がすごい気もするものの、
ライブ配信中にこの新 partial メソッドの[バグを見つけたり](https://github.com/dotnet/roslyn/issues/44930)。

みんなで寄ってたかって新機能を試してみるっていうのはライブ配信に対して求めていたことの1つなので、
非常によい回になったのではないかと思います。
