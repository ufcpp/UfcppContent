---
title: "ピックアップRoslyn: C# 10.0 での低水準機能改善(ref フィールドなど)"
source_url: "https://ufcpp.net/blog/2020/10/pickuproslynlowlevel10/"
content_type: "BlogEntry"
published_at: "2020-10-11T22:30:21"
updated_at: "2020-10-11T22:30:21"
tags: []
umbraco_id: 2313
parent_id: 2311
sort_order: 1
aliases: []
---

# ピックアップRoslyn: C# 10.0 での低水準機能改善(ref フィールドなど)

[前回書いたの](../pickuproslynrecord10/index.md)の続き。

ここ数週間くらいの Language Design Meeting 議事録は C# 10.0 (今年11月に正式リリースされるのが 9.0 で、すでにその次のデザイン)話になっていています。

- [C# Language Design Meeting for September 23rd, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-23.md)
- [C# Language Design Meeting for September 28th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-28.md)
- [C# Language Design Meeting for September 30th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-30.md)
- [C# Language Design Meeting for October 5th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-05.md)
- [C# Language Design Meeting for October 7th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md)

このうち、[前回](../pickuproslynrecord10/index.md)はレコード型関連の話を書きましたが、
今日は低水準機能の改善の話です。([9/23 議事録分](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-23.md))

(残り、細かいトリアージ話もあるんですが、それはまたさらに回を改めて。)

## 低水準 (low level) 機能

.NET Core 2.1 くらいの世代で .NET のパフォーマンスが劇的に向上したわけですが、その原動力になっているのは[`Span<T>`構造体](../../../../study/csharp/resource/span.md)です。
関連する C# の言語機能としては以下のものがあります。

- [参照戻り値と参照ローカル変数](../../../../study/csharp/cheatsheet/ap_ver7.md#ref-returns)
- [ref 構造体](../../../../study/csharp/cheatsheet/ap_ver7_2.md#span-safety)

C# のような「生産性と安全性重視」なプログラミング言語にとっては珍しく、パフォーマンス優先の機能です。
C# だと普段あまり意識しなくていいはずのメモリ管理を強く意識した機能なので、低水準(low-level: ハードウェア寄りという意味)機能というくくりになります。

今回、C# 10.0 向けに検討されているのもこの手の低水準機能で、以下のようなものです。

- ref 構造体が ref フィールドを持てるようにする
- `ByReferenct<T>` という特殊な型を使うのをやめて、ref フィールドに移行する
- 構造体が、そのフィールドを ref 戻り値で返せるようにする
- safe な文脈で、[managed な型](../../../../study/csharp/interop/sp_unsafe.md#unmanaged-types)に対しても[固定長バッファー](../../../../study/csharp/interop/sp_unsafe.md#fixed-buffer)を使えるようにする

## Span 構造体の内部と「ref フィールド」

[`Span<T>`構造体](../../../../study/csharp/resource/span.md)は、<em>論理的には</em>以下のような構造体だと説明されます。

```csharp
struct Span<T>
{
    ref T _pointer;
    int _length;
}
```

フィールドとして `T` への参照と長さを持っています。
この「フィールドとして `T` への参照を持っている」(以下、これを「ref フィールド」と呼びます)というのが `Span<T>` 構造体の肝で、
パフォーマンス向上のポイントになっています。

ただ、「論理的には」と書いたのは、これまでの .NET (.NET 5.0/ C# 9.0 時点でも)にはこの ref フィールド機能がなくて、 .NET Core 2.1 の `Span<T>` 実装当時には、以下のような特殊処理をすることにしました。

```csharp
struct ByReference<T>
{
    // .NET ランタイムが特別扱いする前提なので、C# では書けない
}
 
struct Span<T>
{
    ByReference<T> _pointer;
    int _length;
}
```

`ByReference<T>` がやりたいことはまさに ref フィールドなんですが、
.NET に本格的に ref フィールドを導入するよりは、この特殊処理で実装した方が楽だったそうです。

そして、ref フィールドが欲しくなるような状況の大半は `Span<T>` がカバーしているので、
この時点では以下のような方針になりました。

- 将来、本格的に ref フィールドを導入するときまで `ByReference<T>` は public にしない
- `Span<T>` に関する escape analysis (メソッド外に漏らしてまずいものを return してないかのフロー解析)だけ実装する

そして、この先送りにしていた ref フィールドの話が本格的に検討される段階になったみたいです。
.NET ランタイムにも手を入れる必要がありますし、
C# コンパイラー的にも escape analysis の改善が必要で、
[9/23 の議事録](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-23.md)では ref フィールドに対する escape analysis の案が書かれています。

当然、ref フィールドが入れば、`ByReference<T>` という特殊な構造体は必要なくなるので、
`Span<T>` も素直に ref フィールドで実装したいという話にもなります。

```csharp
readonly ref struct Span<T>
{
    ref readonly T _field;
    readonly int _length;
 
    // 今までありそうでなかったコンストラクター。
    // 今回の提案はこれをできるようにするのも目標の1つ。
    public Span(ref T value)
    {
        ref _field = ref value;
        _length = 1;
    }
}
```

## 構造体のフィールドを ref 戻り値で返す

[ref 戻り値](../../../../study/csharp/resource/sp_ref.md#ref-returns)では、構造体のフィールドの参照を返せなかったりします。
例えば、以下のコードはコンパイル エラーになります。

```csharp
struct S
{
    int _field;
    public ref int Prop => ref _field; // _field の参照を返せない
}
```

以下のような、インターフェイス実装とジェネリックなメソッド呼び出しをしたときに、
外に漏れてはいけない参照を漏らしてしまうことがあるので禁止されています。

```csharp
interface I1
{
    ref int Prop { get; }
}
 
struct S1 : I1
{
    int _field;
    public ref int Prop => ref _field;
 
    // p の寿命は M 内で閉じてるはずなものの、その p の中身が ref 戻り値で返ってしまう。
    static ref int M<T>(T p) where T : I1 => ref p.Prop;
}
```

これに対する対処は、結局、

- フィールドの参照を返すメソッドには `ThisRefEscapes` 属性を付ける
  - 専用の C# 文法を用意してコンパイラー生成にするか、明示的にこの属性を書かせるかはまだ要検討
- この属性が付いているメソッドには制限を掛ける
  - インターフェイス実装できなくする
  - 以下のようなメソッド呼び出しも制限する

```csharp
struct S1
{
    public ref int GetValue() => ...
}
 
class Example
{
    ref int M()
    {
        // このコードは今現在有効 (破壊的変更したくないので今後も有効)
        S1 local = default;
        return ref local.GetValue();
    }
}
```

## safe な固定長バッファー

`Span<T>` が出た当初にも、この構造体があれば[固定長バッファー](../../../../study/csharp/interop/sp_unsafe.md#fixed-buffer)を safe に実装できるんじゃないかという話は出ていたんですが。

実際には、固定長バッファーを safe にしたいときに問題になるのは前節の「構造体のフィールドの参照を返す」の方なので、これまで実装が止まっていました。
前述の `ThisRefEscapes` 属性があれば問題が解決するので、一緒に検討に上がっているみたいです。
