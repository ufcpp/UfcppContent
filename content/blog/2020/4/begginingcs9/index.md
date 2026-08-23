---
title: ".NET 5 Preview 2 と C# Next"
source_url: "https://ufcpp.net/blog/2020/4/begginingcs9/"
content_type: "BlogEntry"
published_at: "2020-04-06T20:30:07"
updated_at: "2020-04-06T20:30:07"
tags: []
umbraco_id: 2287
parent_id: 2286
sort_order: 0
aliases: []
---

# .NET 5 Preview 2 と C# Next

先週末に .NET 5 Preview 2 が出ています。

- [Announcing .NET 5.0 Preview 2](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-2/)

昨日、これ絡みでまたライブ配信したりしてました。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/JXa0JbfiYpw" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

ブログ的にはあんまり長くなってもしんどいので2回に分けようかと思います。
今日は前半の話で、「ひそかに C# Next がちょっと動いてた」という話になります。

## .NET 5 Preview 2

[アナウンス](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-2/)上は、Preview 1 との差分がほぼパフォーマンス改善だけになっています。

が、まあ、アナウンスするほどでもない細かい修正はいろいろあると思います。
ひそかに C# コンパイラーも更新されてるみたいですね。

[Visual Studio 16.6 の方は2週間くらい前に Preview 2 になってる](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes-preview)んですけど、
最近の C# コンパイラーは .NET SDK に同梱されているものが使われているようなので、
更新が掛かるとしたら .NET SDK の方に合わせて。
で、大体は、roslyn リポジトリ中の [Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)のページの State が Merged になっているものは動くはずなので、
物は試しにやってみたらやっぱり入ってたという感じ。

まあ、アナウンスに含まれないくらいなのでお察しだとは思いますが、小さな機能が3つほど追加されただけです。

## LangVersion preview

ちなみに、まだ一応正式には「C# 9.0」とは言っていません。

まあ、Pull Request タイトルとかにはすでに普通に 9.0 の文字が入っていたりする
(例: [#42368](https://github.com/dotnet/roslyn/issues/42368))ので、
雰囲気的にはもう次のバージョンは 9.0 になるかと思いますが、正式決定ではないです。

なので、コンパイラー オプション的にもまだ LangVersion `9` とか `9.0` というものはない状態です。
一方で、[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion)に`preview`を指定すると、今日話すような新機能が使えるようになります。

ちなみに、この`preview`指定ができるようになったことが、7.X 世代みたいな細かいアップデートをしなくなった要因の1つだと思われます。
「新しい機能を早期に試してほしい」というのが目的だったわけで、そのためにはマイナーバージョンを積み重ねるよりも、単にpreviewチャンネルを用意する方が好ましいということなんだと思います。

まあ、現状、9.0 らしい機能は1つも入ってないんですが。
今入っている3つの機能は「8.0 の時に間に合わなかっただけ」みたいな感じのものです。
ちなみに、本格的に 9.0 っぽい機能が入り出すのは、[マイルストーンを覗き見してる感じでは Visual Studio 16.7 のタイミング](https://github.com/dotnet/roslyn/milestone/67)みたいです。

## .NET 5 Preview 2 での C# 新機能

ということで、今回入った機能は以下の3つ。

- Lambda discard parameters
- Attributes on local functions
- Skip locals init

### Lambda discard parameters

C# 8.0 では [discard (値の破棄)](../../../../study/csharp/datatype/patterns.md#discard)という機能が入ったわけですが、
これを使える場所がちょっと増えて、ラムダ式の引数でも使えるようになっています。

```csharp {title="discard をラムダ式の引数に使う"}
Func<int, int, int> f = (_, _) => 0;
```

ぱっと見ではわかりにくいかもしれませんが、2つある引数が両方同じ `_` です。
普通の変数だと `(x, x) => 0` みたいな書き方はできないんですが(名前被りはダメ)、
`_` は「一切使われない引数」という意味になって、
2か所以上の場所でも使えるようになります。
当然、一切使われないことが前提なので、`(_, _) => _` みたいなコードを書くとコンパイル エラーになります。

ちなみに、C# 8.0 以前のコードが壊れないように、「1引数」の場合には discard ではなく通常の変数扱いになります。
要するに、`_ => _` というコードは今まで通り合法です。

### Attributes on local functions

これも名前通りで、ローカル関数に属性を付けれるようになります。

```csharp {title="ローカル関数に属性を付ける"}
static void Main()
{
    [return: NotNullIfNotNull("s")]
    int? f(string? s) => s?.Length;
 
    // f(null).Value だと警告が出る
    Console.WriteLine(f("").Value);
}
```

メソッドの内側に属性を書く構文がこれまでの C# にはなかったので、
[ローカル関数](../../../../study/csharp/functional/fun_localfunctions.md#local-function)の実装当初(C# 7.0 時点)では保留になっていました。

C# 8.0 で [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)が追加されたことで、ローカル関数に対しても属性を付けたいという欲求が急に膨れ上がったので、9.0 で実装することにしたみたいです。

### Skip locals init

最後は [unsafe](../../../../study/csharp/interop/sp_unsafe.md) 限定機能です。
[値が不定になることをほとんど認めない](../../../../study/csharp/resource/rm_default.md#uninitialized) C# にしては珍しく、
初期化をさぼるための機能。
`SkipLocalsInit` という属性をメソッドに付けることで、
その中ではローカル変数の0初期化が行われなくなります。
影響があるのは [`stackalloc`](../../../../study/csharp/interop/sp_unsafe.md#safe-stackalloc) を使うときくらいです。

```csharp {title="0 初期化のスキップ"}
void safe()
{
    Span<byte> span = stackalloc byte[4];
 
    // 通常、 stackalloc した時点で確保した領域は 0 初期化される。
    // なので、以下のコードは常に 0。
    Console.WriteLine(span[0]);
}
 
// unsafe 限定
[System.Runtime.CompilerServices.SkipLocalsInit]
void skipInit()
{
    Span<byte> span = stackalloc byte[4];
 
    // SkipLocalsInit 属性を付けた場合、0 初期化を行わない。
    // span の中身はプログラマーが責任を持って初期化しないとダメ。
    // 今、初期化をさぼってしまったので、以下のコードは不定な値を表示する。
    Console.WriteLine(span[0]);
}
```

不定な値を返すというのはそれだけで安全性を脅かすので unsafe オプション必須です。

0初期化をさぼりたいというのはパフォーマンス改善のためです。
プログラマーが責任を持ってちゃんと初期化するのであれば、
コンパイラーが自動的に挿入する 0 初期化コードは無駄にしかならないので。
その安全性を得るためのちょっとしたペナルティすら避けたい場合にこの属性を使います。
