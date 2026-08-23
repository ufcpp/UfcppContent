---
title: "Visual Studio 2019 Preview 2"
source_url: "https://ufcpp.net/blog/2019/1/vs2019p2/"
content_type: "BlogEntry"
published_at: "2019-01-24T19:58:52"
updated_at: "2019-01-24T19:58:52"
tags: []
umbraco_id: 2218
parent_id: 2216
sort_order: 1
aliases: []
---

# Visual Studio 2019 Preview 2

なんか、[Visual Studio 2019 Preview 2](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#VS2019_Preview2)が出てますね。

リリースノート上は、.NET 関連はまた「[リファクタリング機能が増えたよ](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#net-productivity)」みたいな感じのアナウンス。

あとは、自分が手元で確認してみた感じ、[Preview 1](../../../2018/12/vs2019p1/index.md)の頃から3つほど C# 8.0 の実装が増えてました。

- 再帰パターン
- using の改善
- 静的ローカル関数

動作確認で使ったコード: [Demo/2019/Csharp80/Preview2](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2019/Csharp80/Preview2)

## 再帰パターン

これは Preview 1 で入ると思ってたのに入らなかったというくらいなので、
前に、[sharplab.io](https://sharplab.io/)で動作確認しながら書いた以下の2つのブログほぼそのまま。

- [再帰パターン](../../../2018/12/cs8patterns/index.md)
- [switch 式](../../../2018/12/cs8switchexpr/index.md)

一応、0引数・1引数での `Deconstruct` ができるようになったりしているみたいです。

```csharp {title="0, 1引数 Deconstructと再帰パターン"}
using System;
 
struct X
{
    public void Deconstruct() { }
    public void Deconstruct(out int x) => x = 0;
    public void Deconstruct(out int x, out int y) => (x, y) = (0, 0);
}
 
class Program
{
    static void Main()
    {
        var x = new X();
        Console.WriteLine(x is ());      // 0引数
        Console.WriteLine(x is var (_)); // 1引数のだけは、() 式とかキャストとかとの弁別のために var 必須
        Console.WriteLine(x is (_, _));  // 2引数
    }
}
```

## using の改善

2つほど。

- `ref struct` に限り、`IDisposable` インターフェイスを実装していなくても、パターン ベースで`Dispose`メソッドを呼んでくれるようになった
- `using var` で、ローカル変数のスコープに紐づいたリソースの破棄(`Dispose` メソッド呼び出し)ができるようになった

はい、残念なお知らせ。パターン ベースでの`Dispose`呼び出しが[`ref struct`](../../../../study/csharp/resource/refstruct.md)限定になりました。
そうしないと破壊的変更を起こす可能性があってやむなく限定したそうです。

```csharp {title="パターン ベースの using"}
using System;
 
// インターフェイスなし、ref なし
struct A { public void Dispose() { } }
 
// インターフェイスあり
struct B : IDisposable { public void Dispose() { } }
 
// ref あり
ref struct C { public void Dispose() { } }
 
class Program
{
    static void Main()
    {
        using var a = new A(); // ダメ
        using var b = new B(); // 元々 OK
        using var c = new C(); // C# 8.0 で OK に
    }
}
```

## 静的ローカル関数

ローカル関数に `static` 修飾を付けることで、[ローカル変数のキャプチャ](../../../../study/csharp/functional/sp2_anonymousmethod.md#closure)をしないということを明示できるようになります。

```csharp
// ローカル関数に static を付けると、ローカル変数をキャプチャできなくなる。
static int a(int x) => 2 * x;
 
// 以下のコードは2行目の n のところでエラーに。
int n = 0;
static int b(int x) => n * x;
```

## Preview 1 からのその他の修正

[Async streams](../../../2018/12/cs8asyncstreams/index.md)はいまだに動きません…
これは、たぶん、 .NET Core 3.0 の方の Preview 2が来れば解消される気がします。

あと、[null許容参照型](../../../2018/12/cs8nrt/index.md)は、以下のような変更が掛かってそう

- プロジェクト全体に対して null 解析をオンにするためのオプションが以下のように変更されてそう
  - 旧: `<NullableReferenceTypes>true</NullableReferenceTypes>`
  - 新: `<NullableContextOptions>Enable</NullableContextOptions>`
- 解析が走るタイミングが変わっていそう(たぶん)
  - 旧: 常時
  - 新: ファイルを開いているとき
