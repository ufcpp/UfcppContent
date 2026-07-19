---
title: "C# 8.0 小さな修正"
source_url: "https://ufcpp.net/blog/2018/12/cs8misc/"
content_type: "BlogEntry"
published_at: "2018-12-06T21:09:56"
updated_at: "2018-12-06T21:09:56"
tags: []
umbraco_id: 2186
parent_id: 2177
sort_order: 5
aliases: []
---

# C# 8.0 小さな修正

Visual Studio 2019 Preview 1 が出て、
さすがに C# 8.0 に入る機能・入らない機能がある程度見えてきたので、
今日からしばらくその辺りの紹介をしていこうかと。

とりあえず今日は、「1記事使うほどでもないような小さい奴」をまとめて紹介。

1. 文字列補完、`$` と `@` の順序緩和
1. `??=` (null 合体代入)演算子
1. 構造体の宣言時、`ref`と`partial`の順序緩和
1. 分解の右辺に `default` 式
1. 入れ子の`{}`内での `stackalloc`
1. `unmanaged` 制約付きの型引数に、ジェネリックな型を渡す

ちなみに、VS 2019 Preview 1 で実装されているのは上の2つだけです。

## 順序緩和

C# のキーワードには、並び順を自由に変えられるものがいくつかあります。
代表的なのはクラスやメソッドに対する修飾子ですが、例えば以下の3行は全く同じ意味になります。

```csharp
static public readonly int x;
public readonly static int x;
readonly static public int x;
```

一見するとこれらと同じように順序不問そうに見えるのに、なぜか順序に厳しいものもあります。
やむを得ない理由があってそうなっているものもあるんですが、
例えば、[部分クラス](../../../../study/csharp/oop/oo_class.md#partial_class)の`partial`は、
「C# 2.0 から追加したキーワードなので、1.0 時代のコードを壊さないように、順序を厳しくした」という理由で「`class`または`struct`の直前でないといけない」という制限が付いています。C# 7.2 で入った[ref構造体](../../../../study/csharp/resource/refstruct.md)も同様に、`ref`キーワードは`struct`の直前にないといけません。

しかし、いくつかは理不尽、あるいは、過剰で、

- [参照引数な拡張メソッド](../../../../study/csharp/functional/sp3_extension.md#ref-extensions)は`ref this T`でないとダメだった
- 構造体に対する `ref` と `partial` の両方付けたければ`ref partial struct`の順でないとダメ
  - `partial ref struct` でもいいはず
- [文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation) の(`$`)と[逐語的リテラル](../../../../study/csharp/start/st_embeddedtype.md#verbatim-string)の(`@`)を同時に指定したければ`$@`の順でないとダメ
  - `@$` でもいいはず

とかいうものもあったりします。
こいつらはほんとにどっちが正しいのかわからず、よく間違います。

拡張メソッドの `ref this`と`this ref`は、
今は順序緩和されていてどちらでも使えます。
しかもこの修正、パッチ リリースでこっそりと入っていたり。

ということで、この度、C# 8.0 では後者2つも順序緩和されるみたいです。
どちらも最初から認めてくれててもいいレベルなんですけどね…

## null 合体代入

「null だったら何か適当な既定値で上書き」みたいな処理は結構頻出かと思います。

```csharp
static void M(string x = null)
{
    if (x == null) x = "default string";
    // x に対して何か処理
}
```

あるいは、遅延初期化のために、「初期値にnullを入れておいて、初回アクセス時に有効な値で上書き」みたいなことも結構書きます。

```csharp
public string Name => _name ?? (_name = GetName());
private string _name = null;
```

後者の例では
[null 合体演算子](../../../../study/csharp/resource/rm_nullusage.md#key-null-coalesce) `??` と代入 `=` を組み合わせていますが、まあ、まさにやりたいことはこれ。
`+` に対する `+=` のように、`??` に対する `??=` が欲しいという要望は結構あります。

ということで、その`??=`演算子が C# 8.0で入ります。

```csharp
static void M(string x = null)
{
    x ??= "default string";
    // x に対して何か処理
}
 
public string Name => _name ??= GetName();
private string _name = null;
```

これ、VS 2019 Preview 1ですでに実装されていますけども、
取り組むことになったの、そこそこ最近なんですよね。
あと、地味な機能なのでそんなに話題にも登らず、アピールもされず。
なんか気が付いたら決まっていて、
気が付いたら実装されてて、
気が付いたらマージされた印象。

大した機能じゃなくて実装が簡単とは言え、ちょっとびっくり…

## 分解の右辺に `default` 式

C# 7.1 で [`default`式](../../../../study/csharp/cheatsheet/ap_ver7_1.md#default-expr)ってのが入ったわけですが。
要は、左辺から推論が効く限りには、`default(T)`の`(T)`を省略して`default`だけで掛けるようになるというやつ。

この`default`の型推論、C# 7.x までは、以下のような状況では利きませんでした。

```csharp
(int x1, int y1) = default; // ダメ
(int x2, int y2) = default((int, int)); // これならOK
(int x3, int y3) = (default, default); // これでもOK
```

この、1行目の「ダメ」ってなっている方を、C# 8.0からはOKにするみたいです。

確かに、なんかきわどい…
`(int x, int y)` に対して分解代入できる型は別にタプルに限らないわけで、
じゃあ、この`default`は何に推論されたのか…的な不思議さは一瞬ちょっと感じます。
(まあ、でも、便利さ優先でほしい機能。)

## 入れ子の`{}`内での `stackalloc`

C# 7.2 で[安全に使える `stackalloc`](../../../../study/csharp/resource/span.md#safe-stackalloc)が入りました。
ですが、[ref構造体](../../../../study/csharp/resource/refstruct.md)の制限から、非同期メソッド内ではこの機能が使えませんでした。

```csharp
Span<int> x = stackalloc int[32];
 
// ここで x を使うのは安全なはずだけど、今は問答無用でエラー。
 
await Task.Delay(1);
 
// await をまたいで stackalloc を使おうとするのは明確にまずい。
// これは制限されていてもしょうがない。
```

これに対して、C# 8.0では、以下のように一段`{}`でくくればOKになります。
要するに、`{}`でくくることによって、絶対に`await`をまたがないことが保証されれば(`{}`内に`await`がなければ)認めても安全ということです。

```csharp
{
    // {} でくくったのでこれが書けるようになる。
    Span<int> x = stackalloc int[32];
}
await Task.Delay(1);
```

## `unmanaged` 制約付きの型引数に、ジェネリックな型を渡す

C# 7.3 で[`unmanaged`制約](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-constraints)が入りましたが、微妙に使いにくい点がありました。

```csharp
Unmanaged<int> x; // int は unmanaged なので OK
 
// 以下のものは C# 7.3 ではダメ
Unmanaged<(int, int)> y; // int しか含まないはずなのに…
Unmanaged<Unmanaged<int>> z; // 再帰的に unmanaged 制約を満たしてそうなのに…
```

要は、「ジェネリック型は問答無用ではじく」という状態です。

ちなみに、同様の事情は[ref構造体](../../../../study/csharp/resource/refstruct.md)にもありまして。
ただ、ref構造体の方は、かなり厳密にチェックしてはじかないとまずい(セキュリティ ホールの原因になりかねない危険性あり)ので、
こちらは絶対にジェネリック型を使えないそうです。

そして、C# の仕様書上、[ポインターにも同様の制限](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-types)があります。
C# 2.0の頃からずっと、ジェネリックな型に対してポインターを使えませんでした。

しかしどうも、ポインターに関しては別にこの制限は要らなかったらしいです。
あくまで、「1つでも参照型を含んでいたらダメ」にすべきで、
再帰的に`unmanaged`制約を満たしているのならジェネリックかどうかは関係ないはずです。

なので、C# 8.0で、`unmanaged`制約でのジェネリック型の利用制限は撤廃するし、
[仕様書](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/unsafe-code)のアンマネージ型に関する記述も修正すべきという話になっています。
