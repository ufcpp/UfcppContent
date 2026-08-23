---
title: "file ローカル型"
source_url: "https://ufcpp.net/study/csharp/misc/file-local/"
content_type: "Article"
published_at: "2022-08-25T00:00:00"
updated_at: "2024-08-31T17:24:49"
tags: []
umbraco_id: 2431
parent_id: 1338
sort_order: 10
aliases: []
---

# file ローカル型

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`file` という修飾子を使って「書いたファイル内からだけアクセスできる型」を作れるようになりました。
これを <strong id="branch" class="key-file-local-type">file ローカル型</strong> (file-local type)と言います。

例えば、あるファイルに以下のようなコードを書いたとします。

```csharp {title="file 修飾付きの型を使う例" highlight-text="file"}
1.M();

file static class Extensions
{
    public static void M(this int x) => Console.WriteLine(x);
}
```

これと同じプロジェクト内の別のファイルに以下のようなコードを書いてもエラーにはなりません。

```csharp {title="別のファイルに同名の file 修飾付きの型を定義" highlight-ranges="sha256:bd3266b004cefb39a4c35ea1400844736d98b67545f72862bcc78688a36870d9;1:1-1:5"}
file static class Extensions
{
    public static void M(this int x) => Console.WriteLine("別ファイルの file-local Extensions");
}
```

通常、global な場所(どの名前空間にも属さない場所)に、`Extensions` なんていうよくありそうな名前のクラスを作るとすぐに名前が衝突しますが、
`file` が付いていることによって、全くの同名の型があってもコンパイルできるようになります。

## <a id="sec-generated-title-1"></a> <a id="vs-internal">private や internal と比べて</a>

この手の「見える範囲を制限する」系の処理の用途の1つとして、
「派生クラス・インターフェイス実装クラスを隠す」というのがあります。

例えば、以下のようなコードを書いて、
`Disposable.FromAction` 越しに `IDisposable` でインスタンスを返し、
実装クラスである `ActionDisposable` は直接は使わせないというようなことがしたいことがあります。

```csharp {title="実装クラスを隠す例"}
// file 修飾子を付けると、このファイル内からしかアクセスできない。
file class ActionDisposable : IDisposable
{
    private Action _disposer;
    public ActionDisposable(Action disposer) => _disposer = disposer;
    public void Dispose() => _disposer();
}

// public クラスの、
public static class Disposable
{
    // public メソッドで、
    // 戻り値は public interface なので大丈夫。
    // 内部でだけ file-local な型を使う。
    public static IDisposable FromAction(Action disposer) => new ActionDisposable(disposer);
}
```

こういう「隠す」用途であれば、
これまでも、`internal` や `private` でもある程度代用できました。

`private` の例:

```csharp {title="private で実装を隠す例"}
public static class Disposable
{
    // private にしておけば Disposable クラスの外からは触れない。
    private class ActionDisposable : IDisposable
    {
        private Action _disposer;
        public ActionDisposable(Action disposer) => _disposer = disposer;
        public void Dispose() => _disposer();
    }

    public static IDisposable FromAction(Action disposer) => new ActionDisposable(disposer);
}
```

`internal` の例:

```csharp {title="internal で実装を隠す例"}
// internal にしておけば別プロジェクトからは触れない。
internal class ActionDisposable : IDisposable
{
    private Action _disposer;
    public ActionDisposable(Action disposer) => _disposer = disposer;
    public void Dispose() => _disposer();
}

public static class Disposable
{
    public static IDisposable FromAction(Action disposer) => new ActionDisposable(disposer);
}
```

多くの場合はこれらで十分ですし、
C# 10 以前ではこれでしのいできました。

ただ、問題になったのが [Source Generator](analyzer-generator.md#analyzer) によるコード生成です。
コード生成でクラスを生成したい場合、

* 複数の Source Generator によって「名前の取り合い」が起きかねない
    * 1つのクラスに対して複数の Source Generator を掛けるとき、たとえ `private` でコード生成しても名前が被る可能性がある
    * もし異なる作者の Source Generator で名前が被った場合、解決のしようがない
* Source Generator のアップデート時にクラス名を変えたり、クラス自体を消したりしたいことがある

という懸念・要求が出てきました。
`file` 修飾子によって得られるのは、この「他とは絶対に名前の取り合いにならない型名」になります。

ということで、`file` 修飾子があって一番うれしい用途は Source Generator です。
実際これは、.NET 6 で追加された `Regex` の Source Generator 対応(`GeneratedRegex`)から出て来た要望で、
`GeneratedRegex` は .NET 7 で早速この `file` 修飾子を使ったコード生成をするようになりました。

```csharp {title="GeneratedRegex の例"}
using System.Text.RegularExpressions;

namespace FileLocal;

internal partial class R
{
    // file 修飾子、Source Generator で使う需要が高い。
    // 例えば、GeneratedRegex は早速(.NET 7 から)使ってる。
    [GeneratedRegex(@"\d+")]
    public static partial Regex M();

    // ↑このメソッドから、
    // file sealed class M_0 : Regex { } みたいなクラスが作られてる。
}
```

## <a id="sec-generated-title-2"></a> <a id="applicable">適用範囲</a>

`file` 修飾子は型にのみ適用できます。
以下のように、フィールドやメソッドなどに使おうとするとコンパイル エラーになります。

```csharp {title="file は型のみ"}
class A
{
    file int _x;

    file int M() => _x;
}
```

一方、型であれば何でもよくて、インターフェイス、列挙型、デリゲートなどにも使えます。
以下のコードはいずれも問題なくコンパイルできます。

```csharp {title="型なら何でも file を付けれる"}
file interface IA { }
file enum E { }
file delegate void D();
file struct S { }
file record R;
file record struct RS;
```

インターフェイスであれば、file ローカルなインターフェイスを `public` な型で実装することもできます。
これを使って、「file ローカルなメソッド」の代用にはなったりします。

```csharp {title="file ローカルなインターフェイスを public なクラスで実装する例"}
// file ローカルなインターフェイスも OK だし、
// それを public な型で実装するのも OK。

public class CX : IX // OK
{
    // file ローカルなインターフェイス で明示的実装しておけば実質 file ローカルなメソッドになる。
    // (ちなみに、別に明示的実装でなく普通に実装しても OK)。
    void IX.M() { }
}

file interface IX
{
    void M();
}
```

また、`file` 修飾子は[アクセシビリティ修飾子](../oop/oo_conceal.md#level)と同時に使うことはできません。
例えば以下のコードはコンパイル エラーになります。

```csharp {title="アクセシビリティとの併用不可" highlight-text="internal file"}
internal file static class X
{
}
```

さらに、 `file` 修飾子は top-level (global な場所、もしくは、名前空間直下)の型にしか使えません。
言い換えると、入れ子の型は file ローカルにできません。
以下のコードはコンパイル エラーになります。

```csharp {title="入れ子の型不可" highlight-text="file"}
class A
{
    file class NestedFileClass
    {
    }
}
```

## <a id="sec-generated-title-3"></a> <a id="implementation">内部実装</a>

file ローカルな型のコンパイル結果は、
C# にはよくある「通常の C# からは参照できない名前」(unspeakable name)に変換されます。
名前付けのルールは仕様化されていなくて、「常に同じ名前で生成される保証はない」とされています。
(この辺りは unspeakable name を生成する他の言語機能も同じです。)

現在の file ローカル型の名前付けでは、
ファイル名と連番が入った「`<file_name>f1_ClassName`」みたいな名前で生成されています。
file ローカル型の存在意義的に、「プロジェクト全体で一意な名前」であれば十分なはずで、
連番だけでも目的は果たせていそうです。
型名にファイル名が入ってるのはおそらくデバッグ時にスタックトレースを見やすくするためなど、付加的な目的だと思われます。
