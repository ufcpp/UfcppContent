---
title: "[雑記] オーバーロード解決"
source_url: "https://ufcpp.net/study/csharp/structured/miscoverloadresolution/"
content_type: "Article"
published_at: "2018-04-15T00:00:00"
updated_at: "2024-11-14T00:00:00"
tags: []
umbraco_id: 2147
parent_id: 1217
sort_order: 8
aliases: []
---

# \[雑記\] オーバーロード解決

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[関数](st_function.md#overload)で説明しましたが、
C# では[関数メンバー](st_function.md#function-member)に対して、
同名で引数リストだけが違う物を定義でき、これをオーバーロードと呼びます。

同名の関数がいくつかあるので、`M(0)` などと書いた時、実際には「どの`M`が呼ばれるか」という検索処理が必要になります。
このような同名の関数のうちどれを呼ぶか探す処理を<strong id="overload-resolution" class="keyword">オーバーロード解決</strong>(overload resolution)と呼びます。

本項では、C# がどういうルールでオーバーロード解決を行っているのかについて説明して行きます。

## <a id="sec-generated-title-2"></a> <a id="betterness-rule"></a>「より一致度の高いものを選ぶ」ルール

オーバーロード解決は、基本方針だけを一言でいうとシンプルで、
「より一致度の高いものを選ぶ」という方針になっています。
詳しくは後々説明して行くことになりますが、例えば以下のようなルールになっています。

- 型変換なしで引数に渡せるなら、それを優先的に呼ぶ
- 引数の数がピッタリ一致している方を優先的に呼ぶ

### <a id="sec-generated-title-3"></a> <a id="parameter-type"></a>引数の型

引数の型は、以下のリストの上の方ほど「一致度が高い」と判断されます。

- ぴったり一致する型
- [ジェネリック](../oop/sp2_generics.md)な型
- 親クラス
  - 多段に派生している場合、近い方ほど優先
- 暗黙的に変換できる型
  - その型が実装しているインターフェイス
  - [ユーザー定義の型変換](../functional/fun_whyextensions.md#cast)がある場合
- `object`

型変換なしで渡せるものほど「一致」、
いろんな型を受け付けるものほど「不一致」です。

例えば以下のようなメソッド `M` を書いた場合、
上の方に書いたものほど優先的に呼ばれます。

```csharp {title="引数の型の「一致度」の高さ"}
using System;

// A → B → C の型階層
// IDisposable インターフェイスを実装
// C には int への暗黙的型変換あり
class A : IDisposable { public void Dispose() { } }
class B : A, IDisposable { }
class C : B, IDisposable
{
    public static implicit operator int(C x) => 0;
}

class Program
{
    static void Main()
    {
        // M のオーバーロードがいくつかある中、C を引数にして呼び出す
        M(new C());
    }

    // 上から順に候補になる。
    // 上の方を消さないと、下の方が呼ばれることはない。

    // 「そのもの」が当然1番一致度高い
    static void M(C x) => Console.WriteLine("C");

    // 次がジェネリックなやつ。型変換が要らないので一致度が高いという扱い。
    static void M<T>(T x) => Console.WriteLine("generic");

    // 基底クラスは、階層が近い方が優先。この場合 B が先で、A が後
    static void M(B x) => Console.WriteLine("B");

    static void M(A x) => Console.WriteLine("A");

    // 次に、インターフェイス、暗黙的型変換が同率。
    // (構造体の時の ValueType と違って、クラスは明確に基底クラスが上。)
    // この2つが同時に候補になってると ambiguous エラー
    static void M(IDisposable x) => Console.WriteLine("IDisposable");
    static void M(int x) => Console.WriteLine("int");

    // 最後が object。
    static void M(object x) => Console.WriteLine("object");
}
```

型変換に関しては、候補が複数ある場合は、どちらを呼ぶべきか不明瞭なためコンパイル エラーになります。
例えば以下のコードはコンパイルできません。

```csharp {title="不明瞭でオーバーロード解決できない例" error-ranges="sha256:83f09632948caaee51220b1e7be94561cd81d0215957f4643a76f6d38d3456f9;19:9-19:10"}
using System;

// インターフェイス実装とユーザー定義の型変換を持つ
class A : IDisposable
{
    public void Dispose() { }
    public static implicit operator int(A x) => 0;
}

class Program
{
    static void M(IDisposable x) => Console.WriteLine("IDisposable");
    static void M(int x) => Console.WriteLine("int");

    static void Main()
    {
        // インターフェイスへの変換と、ユーザー定義の型変換は同列
        // どちらを呼ぶべきか、このコードでは解決できない
        M(new A());

        // 明示的にキャストを書けば大丈夫
        M((IDisposable)new A());
        M((int)new A());
    }
}
```

型の派生に関してはクラスのみです。
C# では、任意の[値型](../resource/oo_reference.md#valtype)は `System.ValueType` クラスから派生、任意の[列挙型](st_enum.md)は`System.Enum`クラスから派生しているように振る舞いますが、
これらはあくまで「それっぽく振る舞うようにコンパイラーが特殊対応している」というだけで、
実際には型変換の一種です。
そのため、以下のようなコードはコンパイル エラーになります。

```csharp {title="ValueType への変換はインターフェイスへの変換と同列"}
using System;

struct S : IDisposable
{
    public void Dispose() { }
}

class Program
{
    static void Main()
    {
        // S は ValueType から派生しているかのように振る舞うものの、これはあくまで ValueType への型変換になる
        // インターフェイスへの変換と同列なので、以下の呼び出しは不明瞭
        M(new S());
    }

    static void M(IDisposable x) => Console.WriteLine("IDisposable");
    static void M(ValueType x) => Console.WriteLine("ValueType");
}
```

### <a id="sec-generated-title-4"></a> <a id="generic-method"></a>ジェネリック メソッド

C# では、「ジェネリックかどうか」だけの差があるメソッド オーバーロードも可能です。
この場合、非ジェネリックな方が優先的に呼ばれます。

```csharp {title="非ジェネリックな方優先"}
using System;

class Program
{
    static void Main()
    {
        // M(string) の方が呼ばれる
        M("abc");

        // M<T>(string) の方が呼ばれる
        M<int>("abc");
    }

    static void M(string x) => Console.WriteLine("M");
    static void M<T>(string x) => Console.WriteLine("M<T>");
}
```

### <a id="sec-generated-title-5"></a> <a id="optional"></a>オプション引数・可変長引数

C# には[オプション引数](sp4_optional.md#optional)と[可変長引数](sp_params.md)という、引数を省略できる仕組みが2つあります。
この場合、以下のリストの上の方ほど「一致度が高い」と判断されます。

- 省略なくぴったり引数の数が一致しているもの
- オプション引数による省略
- 可変長引数による省略

```csharp {title="引数の省略"}
using System;

class Program
{
    static void Main()
    {
        M();
    }

    // これが最優先
    static void M() => Console.WriteLine("void");

    // 次がこれ。既定値を与えたもの
    static void M(int x = 0) => Console.WriteLine("int x = 0");

    // 最後がこれ。params
    static void M(params int[] x) => Console.WriteLine("params int[]");
}
```

### <a id="sec-generated-title-6"></a> <a id="instance"></a>インスタンス メソッド優先

C# には[拡張メソッド](../functional/sp3_extension.md)という、
インスタンス メソッドと同じ書き方で静的メソッドを呼べます。
正確にはオーバーロードとは言わないんですが、
インスタンス メソッドと同名の拡張メソッドも定義できるので、
オーバーロードと同種の「解決」が必要になります。

この場合、インスタンス メソッドの方が優先です。
拡張メソッドの方を呼びたければ、本来の静的メソッドとして呼ぶ必要があります。

```csharp {title="拡張メソッド"}
using System;

class A
{
    public void M() => Console.WriteLine("instance");
}

static class Extensions
{
    public static void M(this A a) => Console.WriteLine("extension");
}

class Program
{
    static void Main()
    {
        // instance の方が呼ばれる
        new A().M();

        // A 自身が M を持っている以上、↑の書き方で拡張メソッドの方は呼べない
        // 以下のように、普通に静的メソッドとして呼ぶ必要がある
        Extensions.M(new A());
    }
}
```

## <a id="sec-generated-title-7"></a> <a id="inference"></a>型推論とオーバーロード解決

C# の構文にはいくつか、左辺値からの型推論をするものがあります。

- [ラムダ式](../functional/sp3_lambda.md)
  - どのデリゲート型かの決定
  - デリゲートと、[式ツリー](../functional/sp3_lambda.md#expression)
- [文字列補間](../start/st_string.md#string-interpolation)
- [`default` 式](../resource/rm_default.md#default-expr)

推論に推論を重ねることになるので、これらの型を引数にした場合、オーバーロード解決ができない場合が増えます。

```csharp {title="型推論が働かなくなる例" error-ranges="sha256:0d1c1f7fbcf384652559ef35d4c1db585938cb1b5a831e74cb43375e1df65f02;12:9-12:10,15:9-15:10,21:9-21:10"}
using System;

// 引数が完全に一致しているデリゲート型を2個用意
delegate int A(int x);
delegate int B(int x);

class Program
{
    static void Main()
    {
        // 2個以上候補があるときに default は使えない
        M(default);

        // 型推論とはちょっと違うものの、null (型がない。どの型にでも代入可)でも同様
        M(null);

        // 型指定ありの default なら大丈夫
        M(default(A));

        // A なのか B なのか区別がつかない
        M(x => x);

        // キャストがあれば大丈夫
        // new でも可
        M((A)(x => x));
        M(new A(x => x));
    }

    static void M(A x) => Console.WriteLine("A");
    static void M(B x) => Console.WriteLine("B");
}
```

文字列補完では、`string`型で受け取る場合と`FormattableString`で受け取る場合で異なる挙動になりますが、
`var`を使った暗黙的変数宣言では自動的に`string`扱いされます。
そのため、オーバーロード解決でも特にキャストがない場合、`string`が優先されます。

```csharp {title="文字列補間"}
using System;

class Program
{
    static void Main()
    {
        var (a, b) = (1, 2);

        // M(string) の方が呼ばれる
        M($"{a}, {b}");

        // こう書けば M(FormattableString) の方
        M((FormattableString)$"{a}, {b}");
    }

    static void M(string x) => Console.WriteLine("string");
    static void M(FormattableString x) => Console.WriteLine("FormattableString");
}
```

同様に、ラムダ式は、デリゲート型で受け取る場合と式ツリーで受け取る場合で異なる挙動になります。
こちらは推論は効かず、オーバーロード解決もできなくなります。

```csharp {title="式ツリー"}
using System;
using System.Linq.Expressions;

class Program
{
    static void Main()
    {
        M(x => x);
    }

    static void M(Func<int, int> f) => Console.WriteLine("Func");
    static void M(Expression<Func<int, int>> f) => Console.WriteLine("Expression");
}
```

ただし、次節で説明しますが、ラムダ式の型推論は結構優秀で、
ちゃんと推論が働きつつ、オーバーロード解決できる場合も多いです。

## <a id="sec-generated-title-8"></a> <a id="lambda"></a>ラムダ式

ラムダ式の型推論は相当優秀で、結構複雑なオーバーロード解決もできたりします。
例えば、以下の `M(x => x)` はちゃんとコンパイルできます。

```csharp {title="ラムダ式とオーバーロード解決" error-ranges="sha256:8ae0f8cc379f2179f8d70a06ea3e0f2d0f7ace3b39943958587bae01b4b5e008;16:9-16:10"}
using System;

class Program
{
    static void Main()
    {
        // x の素通し = 引数と戻り値が一致 = Fucn<int, int> の方だけなのでそっちが選ばれる
        // x の型は int に
        M(x => x);

        // 明示的に double を返すと Func<int, double> の方が選ばれる
        // x の型は int に
        M(x => (double)x);

        // この場合、引数と戻り値が一致してるという条件では int なのか string なのか区別できなくてエラー
        N(x => x);
    }

    static void M(Func<int, int> x) => Console.WriteLine("int → int");
    static void M(Func<int, double> x) => Console.WriteLine("int → double");

    static void N(Func<int, int> x) => Console.WriteLine("int → int");
    static void N(Func<string, string> x) => Console.WriteLine("int → int");
}
```

<h5 class="version version6">Ver. 6.0</h5>

ちなみに、ラムダ式がらみの型推論/オーバーロード解決は、C# 6.0 で少し改良がありました。
以下のように、多段のラムダ式でちゃんとオーバーロード解決できるようになったのは C# 6.0 からです。
また、「匿名メソッド式はラムダ式と違って式ツリーにならない」という条件が加味されたのも C# 6.0 からです。

```csharp {title="多段のラムダ式など"}
using System;
using System.Linq.Expressions;

class Program
{
    static void Main()
    {
        // M(() => { }) だと Action か Expression<Action> か区別つかないものの
        // 匿名メソッド式の場合は式ツリー化できない仕様なので、M(Action) で確定
        // なのに以前はこれもエラーになってた(C# 6.0 からは M(Action) が呼ばれる)
        M(delegate () { });

        // 以下のような、多段のラムダ式でちゃんとオーバーロード解決できるのは C# 6.0 から
        // Func<int, Func<int>> の方
        M(() => () => 1);
        // Func<int, Func<double>> の方
        M(() => () => 1.0);
    }

    // ラムダ式だと区別できないものの、匿名メソッド式なら Action で確定
    static void M(Actionx) => Console.WriteLine("Action");
    static void M(Expression<Action> x) => Console.WriteLine("Expression");

    // () => () => 1 みたいな、多段のラムダ式
    static void M(Func<Func<int>> x) => Console.WriteLine("() → () → int");
    static void M(Func<Func<double>> x) => Console.WriteLine("() → () → int");
}
```


<!-- original-page-break -->


## <a id="sec-generated-title-9"></a> <a id="remove-redundant"></a>オーバーロード候補の絞り込み

<h5 class="version version7">Ver. 7.3</h5>

 C# 7.3で、オーバーロード解決の改善がありました。
以下の3つの改善があります。

- 静的メソッドかインスタンス メソッドかの違いで解決できるようになった
- ジェネリック型制約の違いで解決できるようになった
- [メソッド グループ](st_function.md#key-method-group)を引数にするとき、メソッドの戻り値を見るようになった

実のところ、これらの改善は「処理手順の順序変更」だそうです。
(今までも、これからも)オーバーロード解決に際して、C# コンパイラーは以下の2つの処理を行っていますが、
この順序を入れ替えることで上記のような区別がつくようになります。

1. 前述のような、引数の数や型の一致度を調べて最も一致するものを探す
1. 本当にそのメソッドを呼べるかどうかを調べる(上記の、静的/インスタンスの差や、型制約を調べる)

例えば、以下のコードを見てください。
同名の静的メソッドとインスタンス メソッドを1つずつ定義していますが、
間違った引数で呼び出しています。

```csharp {title="同名の静的メソッドとインスタンス メソッド" error-ranges="sha256:68f99ed3f1f7ad2e0b5b9721263f67a9c780021e8cb83218b2a688304a9bfb8d;16:9-16:18,20:9-20:24"}
using System;

struct Static { }
struct Instance { }

class Program
{
    // 同名で、片方は静的メソッドで、もう片方はインスタンス メソッド。
    static void M(Static x) => Console.WriteLine("Static");
    void M(Instance x) => Console.WriteLine("Instance");

    static void Main()
    {
        // 型名.M() で呼べるのは静的メソッドだけのはず。
        // でも、エラー メッセージとしては「M(Instance) を呼ぶにはインスタンスが必要」の類。
        Program.M(new Instance());

        // インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。
        // でも、エラー メッセージとしては「M(Static) を呼ぶにはインスタンス越しじゃダメ」の類。
        new Program().M(new Static());

        // つまり、引数の型でのオーバーロード解決を先にやって、その後、静的/インスタンスの区別を調べてる。
    }
}
```

静的かインスタンスかの差をよりも先に、引数の型だけでオーバーロード解決しています。
なので、`Program.M(new Instance())`と呼ぼうが、`M(Instance x)`の方がまず選ばれます。
そして、「`M(Instance x)`はインスタンス メソッドなので、`型名.M`では呼べない」というエラーになります。

C# 7.3でこの順を逆にして、引数の型でオーバーロード解決をする前に、静的かインスタンスかなどの条件を先に見るようになりました。
呼べないことがわかるんだったら最初からオーバーロード解決候補から外して欲しいわけで、
ある意味当然の変更でしょう。

### <a id="sec-generated-title-10"></a> <a id="static-instance"></a>静的メソッドかインスタンス メソッドか

前節の例に、引数の既定値を足してみましょう。
2つのメソッド`M`が、どちらも`M()`で呼べるようになります。
C# 7.3からは、これらの呼び分けができるようになりました。

```csharp {title="静的メソッドかインスタンス メソッドかでオーバーロード解決" error-ranges="sha256:d9fcbd536be4e7be0d85b2edc72d19fe1a07e835f2fa6b750a4b7f78389fdd3f;33:9-33:10"}
using System;

struct Static { }
struct Instance { }

class Program
{
    // 既定値が入っているのでどちらも M() で呼べる。
    // 片方は静的メソッドで、もう片方はインスタンス メソッド。
    static void M(Static x = default) => Console.WriteLine("Static");
    void M(Instance x = default) => Console.WriteLine("Instance");

    static void Main()
    {
        // 型名.M() で呼べるのは静的メソッドだけのはず。
        // でも、これまでは、M(Static) か M(Instance) かの区別がつかなかった。
        // C# 7.3 では M(Static) が選ばれるように。
        Program.M();

        // インスタンス.M() で呼べるのはインスタンス メソッドだけのはず。
        // 同上。
        // C# 7.3 では M(Instance) が選ばれるように。
        new Program().M();

        // Main が静的メソッドなので、何もつけない場合、この M() も静的な方が呼ばれる。
        M();
    }

    void InstanceMethod()
    {
        // でも、これはダメ。
        // 静的な方もインスタンスの方も M() で呼べるので不明瞭。
        M();

        // これなら OK。
        // this. が付いているのでインスタンス メソッドに絞られる。
        this.M();
    }
}
```

#### <a id="sec-generated-title-11"></a> <a id="color-color"></a>余談: Color Color 問題

C# では、型名とプロパティ名が同じプロパティを作ることができます。
もっともありがちな例が「`Color`構造体型の`Color`プロパティ」なので、「Color Color問題」と呼ばれます。

C# 7.3での静的メソッドとインスタンス メソッドの呼び分けによって、
Color Color問題下においても呼び分けできるようになったものもあります。
しかし、C# 7.3でも解決できないものもあります。

例えば以下の例の通りです。
末尾の2つはC# 7.3でだけコンパイルできるコード、
真ん中の `Color.M()` はC# 7.3でもコンパイルできないコードになります。

```csharp {error-ranges="sha256:7eb8adf628e555e2e41f9eb7a17e81781106e4152bb7c3295685c1d5786ab6b2;33:15-33:16"}
using System;

struct Color
{
    public byte R;
    public byte G;
    public byte B;

    // どちらも M() で呼べるメソッド。
    public void M(int x = 0) => Console.WriteLine("Instance");
    public static void M(Color c = default) => Console.WriteLine("static");

    // 参考までに、オーバーロードがない場合。
    public void Instance() { }
    public static void Static() { }
}

class Program
{
    // C# では、型名とプロパティ名が同じプロパティを作れる。
    static Color Color { get; set; }

    static void Main()
    {
        // これは「プロパティのColor」(C# 7.2以前でも行ける)。
        Color.Instance();

        // これが「型のColor」(C# 7.2以前でも行ける)。
        Color.Static();

        // これだと、この Color が型名かプロパティかが区別できない。
        // C# 7.3 でも不明瞭エラー。
        Color.M();

        // C# 7.3 なら、以下の書き方で呼び分け可能(これまでは不明瞭エラー)。
        // 「プロパティのColor」。
        Program.Color.M();
        // 「型のColor」。
        global::Color.M();
    }
}
```

### <a id="sec-generated-title-12"></a> <a id="constraints"></a>ジェネリック型制約

ジェネリック メソッドで、型制約だけが違うメソッドのオーバーロード解決ができるようにもなりました。

```csharp {title="型制約での呼び分け"}
using System;

// オーバーロード用のダミー型
struct A { }
struct B { }

// IDisposable, IComparable な型を用意
struct Disposable : IDisposable { public void Dispose() { } }
struct Comparable : IComparable { public int CompareTo(object x) => 0; }

class Program
{
    // M(x) で呼べるメソッドが2つ。
    // 差は、T の型制約のみ。
    static void M<T>(T x, A _ = default) where T : IDisposable { }
    static void M<T>(T x, B _ = default) where T : IComparable { }

    static void Main()
    {
        // C# 7.3 からこの呼び出し方ができるように。
        M(new Disposable());
        M(new Comparable());

        // この書き方も C# 7.3 から。
        M(new Disposable(), default); // default は default(A) に推論される
        M(new Comparable(), default); // default は default(B) に推論される

        // C# 7.2 以前の場合、こう書くのが必須。
        M(new Disposable(), default(A));
        M(new Comparable(), default(B));
    }
}
```

特に、参照型(class)か値型(struct)かによるオーバーロード解決は便利そうです。
例えば、「条件を満たさなければnullを返す」みたいなメソッドを書きたい場合、
値型の時だけ[null許容型](../resource/sp2_nullable.md)にして、`?`を付ける必要があります。
この呼び分けが、これまでだとなかなか難しかったですが、C# 7.3ではできるようになります。

```csharp {title="class 制約と struct 制約の呼び分け"}
using System.Collections.Generic;
using System.Linq;

static class ClassExtensions
{
    // クラスの場合は LINQ の FirstOrDefault そのまま。
    public static T FirstOrNull<T>(this IEnumerable<T> source)
        where T : class
        => source.FirstOrDefault();
}

static class StructExtensions
{
    // 構造体の場合は null 許容型に変える必要がある。
    public static T? FirstOrNull<T>(this IEnumerable<T> source)
        where T : struct
        => source.Select(x => (T?)x).FirstOrDefault();
}

class Program
{
    static void Main()
    {
        // ClassExtensions の方のが呼ばれる。
        new[] { "a", "b", "c" }.FirstOrNull();

        // StructExtensions の方のが呼ばれる。
        new[] { 1, 2, 3 }.FirstOrNull();
    }
}
```

### <a id="sec-generated-title-13"></a> <a id="method-return"></a>メソッドの戻り値

C# (というか、.NET)のメソッドは、戻り値の型を[シグネチャ](st_function.md#key-signature)に含みません。
基本的に、戻り値だけが違うメソッドは定義できませんし、呼び分けもできません。

ただ、これまでの例でもたびたび出てきたように、引数の規定値を与えることで戻り値だけが違う「っぽく見える」メソッド オーバーロードはできます。
また、以下のように、「戻り値違いのデリゲートを受け取るメソッド」は作れます。

```csharp {title="戻り値違いのデリゲートを受け取るメソッド オーバーロード"}
static void M(Func<int> f) => Console.WriteLine("int");
static void M(Func<string> f) => Console.WriteLine("string");
```

[前述の通り](#lambda)、
ラムダ式であれば、ラムダ式の型推論が賢くて、この2つのメソッドの呼び分けができました。

```csharp {title="ラムダ式は賢い"}
M(() => 0); // int の方
M(() => "abc"); // string の方
```

しかし、メソッド グループを引数に渡した場合、これまではオーバーロード解決できませんでした。
それが、以下のように、C# 7.3からはオーバーロード解決できるようになります。

```csharp {title="メソッドの戻り値でオーバーロード解決"}
using System;

class Program
{
    static void M(Func<int> f) => Console.WriteLine("int");
    static void M(Func<string> f) => Console.WriteLine("string");

    static int IntReturn() => 0;
    static string StringReturn() => "";

    static void Main()
    {
        // ラムダ式賢い。
        M(() => 0); // int の方
        M(() => "abc"); // string の方

        // こういう書き方なら C# 7.2 まででもできた。
        M(() => IntReturn());
        M(() => StringReturn());

        // なのに、以下のような書き方はこれまでできなかった。
        // C# 7.3 からできるように。
        M(IntReturn);
        M(StringReturn);
    }
}
```

### <a id="sec-generated-title-14"></a> <a id="signature-trick"></a>余談: 同一シグネチャのメソッド オーバーロード

ここで説明してきたように、C# 7.3から静的メソッドかインスタンス メソッドかや、
ジェネリック型制約の差でオーバーロード解決できるようになりました。

呼び分けできるようになったんなら、そもそもオーバーロードもできていいはずではあります。
しかし、静的/インスタンス違いや型制約違いでオーバーロードを作れないのは、
C# ではなく、.NET 型システムの制約です。
単に C# コンパイラーだけの仕事ではないので、これを修正するのは少し難しいです。
そのため、これは引き続き認められていません。

```csharp {title="制約違いのオーバーロードは不可" error-ranges="sha256:3fb66cf60cb9d87d3b05329842059faa2042a0f60f521afc06cb2ee3fbf13ad9;4:13-4:14"}
// 以下の2つは呼び分けできるようになった。
// なのに、定義はできない(C# コンパイラーだけの問題じゃないので直せない)。
static void M<T>(T x) where T : struct { }
static void M<T>(T x) where T : class { }
```

ただし、これまで挙げてきた例で少し出てきていますが、
「ごまかす」方法がいくつかあります。

1つは[オプション引数](sp4_optional.md#optional)(引数の規定値)や[可変長引数](sp_params.md)を使う方法で、以下のような書き方で「違うオーバーロードなんだけど、実質的には同じ呼び方ができる」と言うようなメソッドを定義できます。

```csharp {title="オプション引数をダミーにして疑似的に同シグネチャ オーバーロードを実現"}
class Program
{
    // 呼び分け用のダミー型
    struct Struct { }
    struct Class { }

    // ダミー引数を足すことでオーバーロードする。
    static void M<T>(T x, Struct _ = default) where T : struct { }
    static void M<T>(T x, Class _ = default) where T : class { }

    static void Main()
    {
        M(1);     // M(T, Struct) が呼ばれる
        M("abc"); // M(T, Class) が呼ばれる
    }
}
```

もう1つは拡張メソッドを使う方法です。
拡張メソッドであれば、別のクラス中で定義してやれば、同じ型を対象とした全く同じシグネチャのメソッドを定義できます。

```csharp {title="拡張メソッドで同シグネチャ オーバーロードを実現"}
using System.Collections.Generic;
using System.Linq;

static class ClassExtensions
{
    public static T FirstOrNull<T>(this IEnumerable<T> source)
        where T : class
        => source.FirstOrDefault();
}

static class StructExtensions
{
    public static T? FirstOrNull<T>(this IEnumerable<T> source)
        where T : struct
        => source.Select(x => (T?)x).FirstOrDefault();
}
```

また、`ref`の有無が違うだけの拡張メソッドでもオーバーロード可能です。

```csharp {title="ref の有無でオーバーロード"}
static class Extensions
{
    // ref の有無の差 + 型制約
    public static void M<T>(this ref T x) where T : struct { }
    public static void M<T>(this T x) where T : class { }
}

class Program
{
    static void Main()
    {
        "abc".M();

        var x = 123;
        x.M();
        // ただ、ref 拡張メソッドの性質上、123.M() とは呼べない(リテラルがダメ)
        // また、DateTime.Now.M() とかもダメ(プロパティ越しがダメ)
    }
}
```

いずれも疑似的なもので、ダミーなしのオーバーロードと比べると利便性は下がりますが、
C# 7.3で呼び分けができるようになったことで、少し使い勝手はよくなりました。


<!-- original-page-break -->


## <a id="sec-generated-title-15"></a> <a id="overload-resolution-priority">OverloadResolutionPriority 属性</a>

C# 13 で、オーバーロードの解決優先度を属性を付けて明示できる機能が入りました。
`OverloadResolutionPriority` 属性(`System.Runtime.CompilerServices` 名前空間)を使います。
名前通り優先度を指定できて、正の整数を指定すると優先度が上がって、負の整数なら下がります。

```csharp {title="オーバーロード解決の優先度を変更する例"}
using System.Runtime.CompilerServices;

// IEnumerable<char> の方が選ばれる。
C.M1("");
C.M2("");

class C
{
    // 通常、インターフェイスよりも具体的な型の方が優先。
    public static void M1(string _) { }

    // 属性を付けて優先度を上げる。
    [OverloadResolutionPriority(1)]
    public static void M1(IEnumerable<char> _) { }

    // 属性を付けて優先度を下げる。
    [OverloadResolutionPriority(-1)]
    public static void M2(string _) { }

    public static void M2(IEnumerable<char> _) { }
}
```

ちなみに、オーバーロードできないメンバーにこの属性を付けるとコンパイル エラーになります。

```csharp {title="オーバーロードできないメンバーに OverloadResolutionPriority を付けるとコンパイラーに怒られる" error-ranges="sha256:cf0cf662abb807dfdf285ce290bb048d1274124847a633074457dc5f9c88500d;16:6-16:35,19:6-19:35,22:6-22:35,25:6-25:35" error-diagnostics="sha256:cf0cf662abb807dfdf285ce290bb048d1274124847a633074457dc5f9c88500d;CS9262@16:6-16:35,CS9262@19:6-19:35,CS9262@22:6-22:35,CS9262@25:6-25:35"}
using System.Runtime.CompilerServices;

namespace System.Runtime.CompilerServices
{
    // .NET 標準ライブラリ中の OverloadResolutionPriorityAttribute には
    // AttributeTargets.Method | Constructor | Property がついてる。
    // ここではあえてターゲットの制限を外した同名・同名前空間の型を定義。
    public sealed class OverloadResolutionPriorityAttribute(int priority) : Attribute
    {
        public int Priority => priority;
    }
}

class C
{
    [OverloadResolutionPriority(0)]
    static C() { }

    [OverloadResolutionPriority(0)]
    ~C() { }

    [OverloadResolutionPriority(0)]
    public int X { get; }

    [OverloadResolutionPriority(0)]
    public static implicit operator int(C x) => default!;
}
```


### <a id="sec-generated-title-16"></a> <a id="binary-compat">互換性問題</a>

C# の言語機能が増えるにつれて、例えば「`IEnumerable<T>` よりも、`ReadOnlySpan<T>` 引数を使いたい」みたいなことが多々あります。
しかし、以前からあるメソッドを消すことができなくて、それは残したまま新しいオーバーロードを追加することになったりします。
(ライブラリ作者、特に、プラグイン提供するような場合、バイナリ互換(ソースコードの再コンパイルなしでも動く保証)を残すため、メソッドの削除はできなくなります。)
ところが、互換性のために消すに消せない方のメソッドが、優先度が高すぎて困ったり、
オーバーロード解決できなくなって困るということが起こるようになってきました。

`IEnumerable<T>` と `ReadOnlySpan<T>` の場合、C# 13 時点ではオーバーロード解決できなくなって困ります。
(この2者の問題であれば、C# 14 で `Span<T>`/`ReadOnlySpan<T>` の特別扱いが入って問題解消する予定です。)

```csharp {error-ranges="sha256:5449863ce64b16ff4d6ec80a53a509cc39b1617a9fc408be56ecf94cfd7d8cb1;2:3-2:4" error-diagnostics="sha256:5449863ce64b16ff4d6ec80a53a509cc39b1617a9fc408be56ecf94cfd7d8cb1;CS0121@2:3-2:4"}
// C# 13 時点だと IEnumerable と ReadOnlySpan を選べなくてコンパイル エラーになる。
C.M(new int[1]);

class C
{
    public static void M(IEnumerable<int> _) { }

    // ReadOnlySpan は C# 7.2 / .NET Core 2.1 / 2017年頃に入った。
    // パフォーマンス的に有利なので IEnumerable を置き換えたいことがある。
    public static void M(ReadOnlySpan<int> _) { }
}
```

他に、デフォルト引数が絡んだ場合に困ったりします。
具体的には、`Debug.Assert` や文字列がらみで困っているみたいです。

`Debug.Assert` は、C# 10 で導入された [`CallerArgumentExpression`](../cheatsheet/ap_ver10.md#CallerArgumentExpression) を使いたいものの、既存のオーバーロードに阻害されて呼びようがないという問題が出ています。

```csharp {title="CallerArgumentExpression 付きのオーバーロードを呼べない問題"}
var x = int.Parse(Console.ReadLine());

// Debug.Assert(x > 0, "x > 0") になってほしいのに、1引数の方が呼ばれちゃう。
Debug.Assert(x > 0);

// System.Diagnostics.Debug からの抜粋
class Debug
{
    // 元々 bool 1引数のオーバーロードがある。
    public static void Assert(bool condition) { }

    // C# 10 で導入された CallerArgumentExpression を使いたい。
    // けど、 Assert(condition) では1引数オーバーロードの方が優先されて、CallerArgumentExpression が役に立たない。
    public static void Assert(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null) { }
}
```

文字列がらみは、
.NET の負の遺産として有名なカルチャー依存問題(参考: [遅い](../../../blog/2023/3/string-order/index.md)、[環境依存](../../../blog/2020/11/net5_0ga/index.md))への対処として、`IndexOf` などのメソッドにデフォルト引数 `StringComparison comparisonType = StringComparison.Ordinal` を付けて、無指定の時の挙動を `Ordinal` に変えたいという話があります。
しかしこれも、1引数オーバーロードの方が優先度が高くてうまく働きません。

```csharp
// IndexOf(value, StringComparison.Ordinal) で呼ばれてほしいけど、
// 残念ながら IndexOf(value) にしかならない。
String.IndexOf("àèò", "a");

// 本来は string クラスのインスタンスメソッド。デモ用に静的メソッド。
static class String
{
    // 1引数オーバーロードがいるので…
    public static void IndexOf(this string s, string value) => s.IndexOf(value);

    // デフォルト引数を付けたところで IndexOf(string value) の方が優先される。
    public static void IndexOf(
        this string s, string value,
        StringComparison comparisonType = StringComparison.Ordinal) // Ordinal をデフォルトに変えたい。
        => s.IndexOf(value, comparisonType);
}
```

これらの問題に `OverloadResolutionPriority` 属性が使えます。

```csharp {title="IEnumerable の優先度を下げる"}
using System.Runtime.CompilerServices;

C.M(new int[1]); // 無事、ReadOnlySpan の方が選ばれる。

class C
{
    [OverloadResolutionPriority(-1)]
    public static void M(IEnumerable<int> _) { }

    public static void M(ReadOnlySpan<int> _) { }
}
```

```csharp {title="1引数オーバーロードの優先度を下げる"}
using System.Runtime.CompilerServices;

var x = int.Parse(Console.ReadLine());

// 無事、 Debug.Assert(x > 0, "x > 0") で呼ばれる。
Debug.Assert(x > 0);

class Debug
{
    [OverloadResolutionPriority(-1)]
    public static void Assert(bool condition) { }

    public static void Assert(bool condition, [CallerArgumentExpression(nameof(condition))] string? message = null) { }
}
```

```csharp {title="1引数オーバーロードの優先度を下げる"}
using System.Runtime.CompilerServices;

// 無事、IndexOf(value, StringComparison.Ordinal) で呼ばれる。
String.IndexOf("àèò", "a");

static class String
{
    [OverloadResolutionPriority(-1)]
    public static void IndexOf(this string s, string value) => s.IndexOf(value);

    public static void IndexOf(
        this string s, string value,
        StringComparison comparisonType = StringComparison.Ordinal) // Ordinal をデフォルトに変えたい。
        => s.IndexOf(value, comparisonType);
}
```

ちなみに、`OverloadResolutionPriority` で優先度を下げたメソッドを呼び出すのはかなり困難になったりします。
場合によっては真っ当な方法で呼ぶ手段がなく、リフレクションや unsafe な手段でしか呼べなくなります。

```csharp {title="優先度を下げたせいで真っ当な手段では呼べず &amp; 真っ当じゃない手段で呼ぶ例"}
using System.Runtime.CompilerServices;

// OverloadResolutionPriority(-1) のせいで、真っ当な方法ではどうやっても M(string) の方を呼べない。
C.M((string)"");

// リフレクションとか Unsafe な手段を使えば一応呼べなくはない。
[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = nameof(C.M))]
static extern void M(C? c, string _);
M(default, "");

class C
{
    public static void M(object _) => Console.WriteLine("object");

    [OverloadResolutionPriority(-1)]
    public static void M(string _) => Console.WriteLine("string");
}
```

### <a id="sec-generated-title-17"></a> <a id="in-a-type">同一クラス内でのみ有効</a>

`OverloadResolutionPriority` 属性による優先度の変更は、同一クラス内においてのみ有効です。
なので、以下のようなことは<em>できません</em>。

* 拡張メソッドでインスタンス メソッドを乗っ取り
* 自作の拡張メソッドで他人の拡張メソッドを乗っ取り
* 派生クラス内のオーバーロードで基底クラスのメソッドを乗っ取り

例えば以下のような所業はできません。

```csharp {title="Linq 乗っ取りを画策"}
using System.Runtime.CompilerServices;

// わざと System.Linq.Enumerable と競合するようにして、
namespace System.Linq;

static class FakeLinq
{
    // 優先度を最大限引き上げ。
    [OverloadResolutionPriority(int.MaxValue)]
    public static IEnumerable<TResult> Select<TSource, TResult>(
        this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        => throw new Exception("Select は乗っ取った");
}
```

```csharp {title="ただし、実際にやってみるとうまくいかない(当然)" error-text="Select" error-diagnostics="sha256:121f610bf8281f50ca9f3639df7c5bd659e20b03c8c6a29b793cd18af4c67d26;CS0121@3:7-3:13"}
// FakeLinq の方が優先されたりはしない。
// 単に「Enumerable と FakeLinq 間で不明瞭」エラーに。
"abc".Select(c => (int)c);
```

また、`OverloadResolutionPriority` を付けることで逆にオーバーロード解決できなくなるようなこともありえます。

例えば、以下のように複数のクラスで複数の拡張メソッドが定義されていて、
全体でみれば1つだけ優先度が高くてオーバーロード解決できる場合を考えます。

```csharp {title="複数のクラスの複数の拡張メソッドから1つ選ばれる例"}
// A.M(string), A.M(string, int), B.M(string, int) が同列で比較されて、
// デフォルト引数なしの A.M(string) が勝つ。
"".M();

static class A
{
    public static void M(this string s) => Console.WriteLine($"A.M({s})");
    public static void M(this string s, int i = 0) => Console.WriteLine($"A.M({s}, {i})");
}

static class B
{
    public static void M(this string s, int i = 0) => Console.WriteLine($"B.M({s}, {i})");
}
```

ここで、`A.M` のうちの1つに `OverloadResolutionPriority` を付けて優先度を変えてみます。
`OverloadResolutionPriority` は1つのクラス内でしか働かないので、`A` の中のどの `M` が選ばれるかにだけ影響します。
その結果、以下のように別のクラスの `M` と競合する可能性があります。

```csharp {title="OverloadResolutionPriority を付けたことで他のクラスのメンバーと競合するようになる例" error-ranges="sha256:b2e474f0df2c07544ec4371f4f98d174bf0c31fe82ea28f93b2e9bb09cee7eec;6:4-6:5" error-diagnostics="sha256:b2e474f0df2c07544ec4371f4f98d174bf0c31fe82ea28f93b2e9bb09cee7eec;CS0121@6:4-6:5"}
using System.Runtime.CompilerServices;

// OverloadResolutionPriority を付けたことで、A.M の中では A.M(string, int) が選ばれる。
// B.M は元々 B.M(string, int) しかない。
// A.M(string, int) と B.M(string, int) が競合してオーバーロード解決できなくなる。
"".M();

static class A
{
    public static void M(this string s) => Console.WriteLine($"A.M({s})");

    [OverloadResolutionPriority(1)]
    public static void M(this string s, int i = 0) => Console.WriteLine($"A.M({s}, {i})");
}

static class B
{
    public static void M(this string s, int i = 0) => Console.WriteLine($"B.M({s}, {i})");
}
```

### <a id="sec-generated-title-18"></a> <a id="overload-by-return">余談: (疑似)戻り値オーバーロード</a>

C# では戻り値だけが異なるオーバーロードを認めていません。
例えば以下のコードはコンパイル エラーになります。

```csharp {title="戻り値だけが違うオーバーロードの追加はできない" error-ranges="sha256:4ac088f41f67af8c137adae628025cfe058f953fe77848d6d829abc0e89b7e60;7:35-7:41" error-diagnostics="sha256:4ac088f41f67af8c137adae628025cfe058f953fe77848d6d829abc0e89b7e60;CS0111@7:35-7:41"}
class C
{
    public static async Task MAsync() { await Task.Yield(); }

    // Task を ValueTask に変更したいとして、互換性のために Task MAsync() を残すと…
    // 戻り値だけが違うオーバーロードは認められない。
    public static async ValueTask MAsync() { await Task.Yield(); }
}
```

ちょっと気持ち悪い回避策になりますが、デフォルト引数を悪用することでオーバーロードもどきを作れたりはします。
ところが、「引数なし」と「デフォルト引数持ち」なら前者の方が優先されるため、
追加した新しいオーバーロードもどきが呼ばれることはありません。

```csharp {title="オーバーロードもどき(おしい)"}
// 残念ながら Task MAsync() の方しか呼ばれない。
await C.MAsync();

// もちろんこうすれば ValueTask の方が呼ばれるものの、不格好すぎる。
await C.MAsync(default);

class C
{
    public static async Task MAsync() { await Task.Yield(); }

    // オーバーロードもどきとして、適当に使わないデフォルト値付きの引数を追加。
    public static async ValueTask MAsync(int _ = 0) { await Task.Yield(); }
}
```

これも一応、`OverloadResolutionPriority` 属性で解消できます。

```csharp {title="OverloadResolutionPriority でごり押し"}
using System.Runtime.CompilerServices;

// ValueTask 戻り値の方が呼ばれるように。
await C.MAsync();

class C
{
    [OverloadResolutionPriority(-1)]
    public static async Task MAsync() { await Task.Yield(); }

    public static async ValueTask MAsync(int _ = 0) { await Task.Yield(); }
}
```
