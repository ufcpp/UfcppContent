---
title: "ref構造体"
source_url: "https://ufcpp.net/study/csharp/resource/refstruct/"
content_type: "Article"
published_at: "2017-11-18T00:00:00"
updated_at: "2024-06-22T00:00:00"
tags: []
umbraco_id: 2107
parent_id: 1286
sort_order: 7
aliases: []
---

# ref構造体

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[前項](span.md)では、C# 7.2 の新機能と深くかかわる `Span<T>` 構造体という型を紹介しました。
この型は、論理的には `(ref T Reference, int Length)` というような、「参照フィールド」と長さのペアを持つ構造体です。
「参照」を持っているので、参照戻り値や参照ローカル変数と同種の「出所の保証」が必要です。
また`Span<T>` には「[スタック](misc_heap.md)上に置かれている必要がある」(ヒープに置けない)という制限が必要です。

さらに、`Span<T>` に制限が掛かっている以上、「`Span<T>`を持つ型」にも再帰的に制限が掛かります。
「`Span<T>` を持つか持たないか」だけで挙動が変わるのでは影響範囲が大きすぎるため、
「`Span<T>` を持ちたければ `ref` という修飾が必要」という制約もあります。

ここでは、これらの `Span<T>` の「スタック上に置かれている必要がある」という制約や、「`ref` 構造体」について説明していきます。
(`ref`構造体という機能ではありますが、主用途が`Span<T>`に関するものなので、span safety ruleと呼ばれたりもします。)

## <a id="sec-generated-title-2"></a> <a id="ref-struct"></a>ref 構造体

`Span<T>` には制限が必要といっても、C# コンパイラーとしては `Span<T>` だけを特別扱いしたくはありません。
そこで、<strong id="key-refstruct" class="keyword">`ref`構造体</strong> (`ref struct`)というものを導入しました。

`ref`構造体は、名前通り、`ref` 修飾子が付いた構造体です。
`Span<T>` 構造体自身にも `ref` 修飾子がついています。
そして、`ref`構造体をフィールドとして持てるのは`ref`構造体だけです。

```csharp {title="ref構造体を持てるのはref構造体だけ"}
// Span<T> は ref 構造体になっている
public readonly ref struct Span<T> { ... }

// ref 構造体を持てるのは ref 構造体だけ
ref struct RefStruct
{
    private Span<int> _span; //OK
}
```

逆に言うと、`ref` 修飾子がついていない構造体や、クラスは`ref`構造体をフィールドとして持てません。

```csharp
// NG。構造体以外を「ref 型」にはできない
ref class InvalidClass { }

// ref がついていない普通の構造体は ref 構造体を持てない
struct NonRefStruct
{
    private Span<int> _span; //NG
}
```

そして、以下で説明する制約は、`Span<T>` 構造体だけでなく、すべての `ref` 構造体に対して掛かります。

## <a id="sec-generated-title-3"></a> <a id="flow-analysis"></a>戻り値で返せるもの

`ref` 構造体を戻り値として使いたい場合、
[`ref` 戻り値・`ref` ローカル変数](sp_ref.md#ref-returns)と同様に、大元をたどって調べて(フロー解析して)、返していいものかどうかを判定します。
以下のようなルールがあります([`ref`戻り値と同じルール](sp_ref.md#flow-analysis)です)。

- 引数で受け取ったものは戻り値に返せます
- ローカルで確保したものは返せません
- 引数などを介して多段に参照している場合、コードをたどって大元が安全かまで調べます

```csharp {title="戻り値に返せるかどうか"}
// 引数で受け取ったものは戻り値で返せる
private static Span<int> Success(Span<int> x) => x;

// ローカルで確保したもの変数はダメ
private static Span<int> Error()
{
    Span<int> x = stackalloc int[1];
    return x;
}

// 多段の場合も元をたどって出所を調べてくれる
private static Span<int> Success(Span<int> x, Span<int> y)
{
    var r1 = x;
    var r2 = y;
    var r3 = r1.Length >= r2.Length ? r1 : r2;

    // r3 は出所をたどると引数の x か y
    // x も y も引数なので大丈夫
    return r3;
}

private static Span<int> Error(Span<int> x, int n)
{
    var r1 = x;
    Span<int> r2 = stackalloc int[n];
    var r3 = r1.Length >= r2.Length ? r1 : r2;

    // r2 がローカルなのでダメ
    return r3;
}
```

ちなみに、上記の`Error`と似たようなコードでも、以下のコードはコンパイルできます。
ちゃんと「メモリ確保があったかどうか」を見ていて、「`default`であれば何も確保していない」という判定もしています。

```csharp {title="default は何も確保しない"}
// ちゃんと「メモリ確保」があったかどうかを見てる
// 同じようなコードでもこれは OK (default だと何も確保しない)
private static Span<int> Success1()
{
    Span<int> x = default;
    return x;
}
```

このルールは、`ref`構造体と、`ref`引数・`ref`戻り値の間でも働きます。
例えば、引数由来の `Span<T>`から得た`ref T`な参照は戻り値にできますが、ローカル由来のものはできません。

```csharp {title="Span&gt;T&lt;とref T"}
// 引数で受け取った Span 由来の ref 戻り値は返せる
private static ref int Success(Span<int> x) => ref x[0];

// ローカルで確保した Span 由来の ref 戻り値はダメ
private static ref int Error()
{
    Span<int> x = stackalloc int[1];
    return ref x[0];
}
```

### <a id="sec-generated-title-4"></a> <a id="readonly-ref"></a>readonly ref

C# 7.2 で追加された構造体がらみの修飾子には[`readonly`](readonlyness.md#readonly-struct)というものもあります。
`readonly`修飾は、一見、参照がらみの機能とは無関係に見えますが、実はこれも「参照として返せるかどうか」の判定に関係しています。

例えば以下のコードを見てください。

```csharp {title="readonly修飾とref構造体"}
using System;

// ref だけ
ref struct RefToSpan
{
    private readonly Span<int> _span;
    public RefToSpan(Span<int> span) => _span = span;

    // 例え _span に readonly が付いていても、this 書き換えが可能
    public void Method(Span<int> span) { this = new RefToSpan(span); }
}

// readonly ref
readonly ref struct RORefToSpan
{
    private readonly Span<int> _span;
    public void Method(Span<int> span) { }
}

class Program
{
    public static void LocalToRef(RefToSpan r)
    {
        Span<int> local = stackalloc int[1];
        r.Method(local); // ここでエラーになる。r の中身が書き換えられることで、local が外に漏れる可能性を危惧

        // 注: この例の場合は実際には漏れはしないものの、RefToSpan の作り次第なので保証はできない
    }

    public static void LocalToRORef(RORefToSpan r)
    {
        Span<int> local = stackalloc int[1];
        r.Method(local); // readonly ref に対してなら OK
    }
}
```

ローカルで定義した`Span<T>`を、引数で渡ってきた`ref`構造体のメソッドに対して渡しています。
この場合、`readonly`がついている場合にだけコンパイルできます。
`readonly`がついていない方では、メソッドの中で`r`が書き換わる可能性があります。
その結果「ローカルの`Span<T>`が外に漏れる可能性がある」という判定を受けるため、コンパイル エラーになります。
`readonly`がついている方では「書き換えがあり得ない」ということで、「外にも漏れない」という判定になります。

### <a id="sec-generated-title-5"></a> <a id="unsafe"></a>余談: さすがに unsafe までは追えない

参照がらみのフロー解析は、あくまで`ref`ローカル変数や、`ref`構造体に対してだけ働きます。
`unsafe`を使って、ポインターなどを介するとさすがに追跡できません。

例えば、以下のコードは不正で、実行時エラーであったり、予期しない動作を招く可能性があります。
しかし、コンパイラーが不正を判定できず、コンパイル時にエラーにすることができません。

```csharp {title="unsafe な手段までは追えない"}
unsafe static Span<int> X()
{
    // ローカル
    int x = 10;

    // unsafe な手段でローカルなものの参照を作って返す
    // これをやってしまうとまずいものの、コンパイル時にはエラーにできない
    return new Span<int>(&x, 1);
}
```

## <a id="sec-generated-title-6"></a> <a id="stack-only"></a>「スタックのみ」制約

`ref`構造体はスタック上に置かれている必要があります。
この性質から、`ref`構造体は「stack-only 型」と呼ばれることもあります。
この制限が必要になるのは以下の2つの理由からです。

- そもそも参照自体がスタック上でしか働かない
- マルチスレッド動作時に安全性を保証できない

まず、`ref` 構造体以前に、参照自体がスタック上でしか使えません。
参照は、常にその参照の出所をトラッキングする必要があります。
例えば、出所がクラス(.NET の[ガベージ コレクション](rm_gc.md#garbage-collection)の管理下)の場合、
それを参照する方もガベージ コレクションのトラッキングの対象になります。
このトラッキング処理を低コストで行うためには、参照がスタック上になければなりません。

次に、マルチスレッド動作に関してですが、
`Span<T>` の中身が論理的には `(ref T Reference, int Length)` という2要素からなることによります。
安全に使うには、この2つが[アトミック](../async/sp_thread.md#lock)に読み書きされなければなりません。
もし、`Reference` だけが書き換わり、`Length` がまだ書き換わっていないタイミングで参照先を読み書きされてしまうと、
範囲チェックが正しく働かず、不正な領域を読み書きしてしまう危険性が出てきます。

ということで、「スタック上に置かれている必要がある」という制約が掛かります。
具体的には、以下のような制限があります。

- クラスのフィールドとして持てない(クラスに `ref` 修飾子を付けれない理由はこれ)
- [クラスのフィールドに昇格](../functional/sp2_anonymousmethod.md)する可能性があることができない
  - [ローカル関数](../functional/fun_localfunctions.md#key-local)や[ラムダ式](../functional/fun_localfunctions.md#key-anonymous)で[キャプチャ](../functional/fun_localfunctions.md#capture-local)できない
  - [イテレーター](../data/sp2_iterator.md)の引数には使えない
  - イテレーター内では、`yield return` をまたいで使えない
  - [非同期メソッド](../async/sp5_async.md)に対しては引数にもローカル変数にも使えない
      - ([C# 13 で緩和](../cheatsheet/ap_ver13.md#ref-in-async)。C# 13 からは、`await` をまたがない限り、ローカル変数に使えます)
- [ボックス化](rmboxing.md)できない
  - `object`や`dynamic`、インターフェイス型の変数に代入できない
  - `ToString` など、`object` 型のメソッドを呼べない
- ジェネリック型引数として使えない

```csharp {title="ref構造体は stack-only"}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

//❌ そもそもクラスに ref を付けれないのも stack-only を保証するため
ref class Class { }

//❌ インターフェイス実装
ref struct RefStruct : IDisposable { public void Dispose() { } }

class Program
{
    //❌ 非同期メソッドの引数
    static async Task Async(Span<int> x)
    {
        //❌ 非同期メソッドのローカル変数
        Span<int> local = stackalloc int[10];
    }

    //❌ イテレーターの引数
    static IEnumerable<int> Iterator(Span<int> x)
    {
        Span<int> local = stackalloc int[10];
        local[0] = 1; //⭕ yield return をまたがないならOK
        yield return local[0];
        //❌ yield をまたいだ読み書き
        local[0] = 2; // ダメ
    }

    static void Main()
    {
        Span<int> local = stackalloc int[1];

        //❌ box 化
        object obj = local;

        //❌ object のメソッド呼び出し
        var str = local.ToString();

        //❌ クロージャ
        Func<int> a1 = () => local[0];
        int F() => local[0];

        //❌ 型引数にも渡せない
        List<Span<int>> list;
    }
}
```


### <a id="sec-generated-title-7"></a> <a id="TypedReference"></a>余談: TypedReference

「[型付き参照](../interop/sp_makeref.md)」で説明している`TypedReference`型も、内部的に参照を持っている型の1つです。
`TypedReference` は ref 構造体の仕様よりも古くからあって、昔はこの型だけに対して特殊対応をしていました。

その昔からある `TypedReference` に対する特殊対応は、本項で説明している C# 7.2 から入った ref 構造体に対する制約よりもだいぶ緩くて、実は「スタック上に置かれている必要がある」制約から割かし簡単に外れることができました。

ちなみに、C# 7.2 で ref 構造体を導入後、
.NET Core 2.1 からは `TypedReference` に対する特殊対応は止めて、単に `TypedReference` を ref 構造体に変更したようです。
結果的に元よりも制約が厳しくなっていて、昔は(バグっている可能性が非常に高いものの)一応コンパイルできていたコードがコンパイル エラーになる可能性があります。
(ただ、`TypedReference` 自体利用頻度が非常に低いので問題にはなっていません。)

## <a id="sec-generated-title-8"></a> <a id="ref-field">ref フィールド</a>

<h5 class="version version11">Ver. 11</h5>

C# 11 で、[ref 構造体](#key-refstruct)のフィールドを [`ref` (参照渡し)](sp_ref.md#byref)で持てるようになりました。
これを <strong id="key-ref-field" class="keyword">ref フィールド</strong>(ref field)と言います。

ref フィールドの書き方は参照引数や参照戻り値と同じく、型の前に `ref` 修飾を付けます。

```csharp {title="ref フィールド"}
ref struct ByReference<T>
{
    public ref T Value;
}
```

C# 7.2 に頃に [`Span<T>` 構造体の内部的な話](span.md#fast-span)で、「`Span<T>` はランタイム側で特殊処理を入れている」というような話を書いていましたが、
ref フィールドが入ったことで、通常の C# コードで同様のことができるようになりました。
実際、.NET 7 からはそういう実装に置き換わっていて、`Span<T>` の内部は晴れて以下のようなコードに変更されています。

```csharp {title=".NET 7 での Span の中身"}
ref struct Span<T>
{
    internal readonly ref T _reference;
    private readonly int _length;
}
```

ちなみに、ref フィールドを持てるのは ref 構造体だけです。
以下のコードはコンパイル エラーになります。

```csharp
class A
{
    ref int _x; // class 中はダメ。
}

struct B
{
    ref int _x; // struct も ref がついてないものの中はダメ。
}
```

### <a id="sec-generated-title-9"></a> <a id="readonly-ref">readonly ref</a>

C# 7.2 の頃に [`ref readonly`](sp_ref.md#ref-readonly) というものがありました。
これは、「参照先の値の変更不可」というものです。
一方で、ref フィールドになると、`ref readonly` と `readonly ref` の2種類の readonly ができます(あるいは両方付けて `readonly ref readonly` もできます)。

比較のためにまず、どちらの readonly もついていない状態ですが、
当然、「どこを参照するか変更」と「参照先の値の変更」のどちらもできます。

```csharp {title="✔「どこを参照するか変更」と✔「参照先の値の変更」" highlight-ranges="sha256:488405be4ed0d9ef67529c31ae732689273f536f96f8f0ce8c6dcbe6ce77cb4c;10:12-10:15"}
scoped var a = new A();

int x1 = 0;
a.X = ref x1; // どこを参照するかを変更。

a.X = 2; // 参照先の値を変更

ref struct A
{
    public ref int X;
}
```

で、`ref readonly` の方は C# 7.2 の頃からある意味と同じで、「参照先の値の変更不可」です。

```csharp {title="✔「どこを参照するか変更」と✖「参照先の値の変更」" highlight-text="ref readonly"}
scoped var a = new A();

int x1 = 0;
a.X = ref x1; // どこを参照するかを変更。

a.X = 2; // エラー: 参照先の値を変更不可。

ref struct A
{
    public ref readonly int X;
}
```

一方、C# 11 から書ける `readonly ref` は、要は、ref フィールド `ref T X` を readonly にするという意味なので、「どこを参照するか変更」の方ができなくなります。

```csharp {title="✖「どこを参照するか変更」と✔「参照先の値の変更」" highlight-text="readonly ref"}
int x0 = 0;

// readonly フィールドはコンストラクターでしか初期化できないので引数で渡す。
scoped var a = new A(ref x0);

int x1 = 1;
a.X = ref x1; // エラー: どこを参照するかを変更不可。

a.X = 2; // 参照先の値を変更はできる。

ref struct A
{
    public readonly ref int X;
    public A(ref int x) => X = ref x;
}
```

当然、両方の `readonly` を付けると両方不可です。

```csharp {title="✖「どこを参照するか変更」と✖「参照先の値の変更」"}
int x0 = 0;

// readonly フィールドはコンストラクターでしか初期化できないので引数で渡す。
scoped var a = new A(ref x0);

int x1 = 1;
a.X = ref x1; // エラー: どこを参照するかを変更不可。

a.X = 2; // エラー: 参照先の値を変更不可。

ref struct A
{
    public readonly ref readonly int X;
    public A(ref int x) => X = ref x;
}
```

## <a id="sec-generated-title-10"></a> <a id="escape-analysis">エスケープ解析</a>

参照を使う上では、「漏らしてはいけないものを漏らさない」ということが必要になります。
簡単に言うと、メソッド内のローカル変数はメソッドを抜けると消えるので、
その参照は外に漏らしてはいけません。

```csharp {title="ローカル変数への参照は外に漏らせない"}
static ref int M()
{
    int x = 123; // メソッド内の変数はメソッド抜けると消える。
    return ref x; // エラー: 消えるものと外には漏らせない。
}
```

こういう「漏れている」状態を「エスケープ(escape: 脱走)している」と言います。

上記の例の場合は単純ですが、
参照変数などがあるため、間接的に何段も追いかける必要があります。

```csharp {title="エスケープ阻止のため、多段に追う必要あり"}
static ref int M()
{
    int x = 123; // メソッド内の変数はメソッド抜けると消える。
    ref var y = ref x;
    ref var z = ref y;
    return ref z; // エラー: 間に2段挟まっているものの、元は x なので外に漏らせない。
}
```

このように、間に何段か挟まっていようと、大本をたどってエスケープを避ける処理を「<strong id="key-escape-analysis" class="keyword">エスケープ解析</strong>」(escape analysis)と呼びます。

C# 7.2 で ref 構造体が、
C# 11 で ref フィールドが入ったわけですが、
エスケープ解析はこれらも考慮する必要があります。

例えばわざとちょっと複雑なことをすると、以下のように、いろいろなところに参照が伝搬するコードが書けます。

```csharp {title="参照がいろんなところに伝搬する例"}
static void M(out Span<int> result)
{
    int x = 123;
    var span = new Span<int>(ref x); // x が span から参照される状態。
    scoped var r = new R();

    var ret = r.M(span, out var y); // x がいろんなところに伝搬。

    result = r.Span; // エラー: x が r.Span に伝搬してるかもしれないのでダメ。
    result = y;      // エラー: x が y に伝搬してるかもしれないのでダメ。
    result = ret;    // エラー: x が ret に伝搬してるかもしれないのでダメ。
}

ref struct R
{
    public Span<int> Span;

    public Span<int> M(Span<int> x, out Span<int> y)
    {
        Span = x; // フィールドにも、
        y = x;    // out 引数にも、
        return x; // 戻り値にも x (が持ってる参照)が伝搬。
    }
}
```

コスト度外視でよければ、
「どの引数・フィールドが、他のどの引数・フィールド・戻り値に伝搬するか」を事細かに指定することで厳密なエスケープ解析ができます。
(C# では採用しなかったため)仮定的なコードにはなりますが、
先ほどのコードを以下のように書けるようにするという案はなくはないです。

```csharp {title="(仮定的なコードで) 参照の伝搬をすべて明示" highlight-ranges="sha256:939ca15f191bf8a9793bfe3a18d6e661142296486b487edbe642187936068d9e;18:21-18:23,20:21-20:23,20:35-20:37,20:54-20:56"}
static void M(out Span<int> result)
{
    int x = 123;
    var span1 = new Span<int>(ref x); // x が span から参照される状態。
    var span2 = new int[1];           // こちらは配列を参照しているので外に漏らしても大丈夫。

    var r = new R { Span = span1 };

    var ret = r.M(span2, out var y); // span2 → y, span1 → r.Span → ret と伝搬。

    result = y;      // 出どころが y → span2 → 配列 なので外に漏らして大丈夫。
    result = ret;    // 出どころが ret → r.Span → span1 → x なのでダメ。
}

// 仮定的な文法: ` で、参照の伝搬先を表現。
ref struct R
{
    public Span<int>`A Span;

    public Span<int>`A M(Span<int>`B x, out Span<int>`B y)
    {
        // 伝搬先の指定が違うので、以下のコードはダメ。
        // Span = x;
        // return x;
        y = x;       // `B 間の伝搬は OK。
        return Span; // `A 間の伝搬は OK。
    }
}
```

### <a id="sec-generated-title-11"></a> <a id="scoped"></a><a id="scoped-modifier">scoped 修飾子</a>

ただ、ここまで細かい指定に需要があるかというと微妙です。
そこで C# 11 では、以下の2種類だけに絞ることにしました。

* scoped: どこにも漏らさない。メソッドの中でだけ使う。
* unscoped: どこかに漏らす。

ref 構造体(`Span<T>` など)に関しては実際にこの2択で、
何もつかなかった場合は unscoped 扱いで、`scoped` という新しい修飾子を付けると scoped 扱いになります。

一方で、`ref T` (`ref` 引数・`ref` 変数)に関しては、
既存コードを壊さないように、何もつけないと「引数から戻り値への伝搬だけ認める」(通称 return-only)というわかりにくいルールになっています。
そして、`UnscopedRef` 属性(`System.Diagnostics.CodeAnalysis` 名前空間)を付けると unscoped 扱い、
`scoped` 修飾子を付けると scoped 扱いになります。
(またちょっとややこしいことに、コンストラクターの引数の場合だけ、`ref T` でも unscoped 扱いみたいです。)

実際のコードを見てみましょう。
まず、何もつけない場合(`ref T` は return-only、ref 構造体は unscoped):

```csharp {title="何もつけない: ref T は return-only、ref 構造体は unscoped"}
ref struct Default
{
    private ref int _x;
    private Span<int> _y;

    // OK なやつ。
    public Default(ref int x) => _x = ref x;
    public ref int ReturnRef(ref int x) => ref x;
    public ref int GetRef() => ref _x;
    public void UseRef(ref int x) { }

    public Default(Span<int> y) => _y = y;
    public Span<int> ReturnSpan(Span<int> y) => y;
    public Span<int> GetSpan() => _y;
    public void SetSpan(Span<int> y) => _y = y;
    public void UseSpan(Span<int> y) { }

    // エラーになるやつ。
    // 引数 → フィールドへの伝搬だけ、ref T と Span<T> の挙動が違う。
    // ref T は「引数 → 戻り値 だけは OK」(return-only)。
    public void SetRef(ref int x) => _x = ref x;
}
```

続いて、`scoped` 修飾子を付けた場合(いずれも scoped 扱い)、たいていのものがダメになります:

```csharp {title="scoped 修飾子を付けた場合"}
ref struct Scoped
{
    private ref int _x;
    private Span<int> _y;

    // OK なやつ。
    // フィールドにも戻りにも伝搬しない場合だけ OK。
    public void UseRef(scoped ref int x) { }
    public void UseSpan(scoped Span<int> y) { }

    // エラーになるやつ。
    // たいていダメ。
    public Scoped(scoped ref int x) => _x = ref x;
    public ref int ReturnRef(scoped ref int x) => ref x;
    public void SetRef(scoped ref int x) => _x = ref x;

    public Scoped(scoped Span<int> y) => _y = y;
    public Span<int> ReturnSpan(scoped Span<int> y) => y;
    public void SetSpan(scoped Span<int> y) => _y = y;
}
```

最後に、`UnscopedRef` 属性を付けた場合、たいていのものが OK になります
(ただし、ref 構造体は何も付けなくても unscoped 扱いなので、追加で属性を付けようとするとエラーになります):

```csharp
using System.Diagnostics.CodeAnalysis;

ref struct Unscoped
{
    private ref int _x;
    private Span<int> _y;

    // OK なやつ。
    // UnscopedRef 属性を付けるとなんでも OK に。
    // (といっても差が出るのは SetRef だけ。)
    public Unscoped([UnscopedRef] ref int x) => _x = ref x;
    public ref int ReturnRef([UnscopedRef] ref int x) => ref x;
    public void SetRef([UnscopedRef] ref int x) => _x = ref x;
    public void UseRef([UnscopedRef] ref int x) { }

    // Span の方は「デフォルトで UnscopedRef だから属性付けるな」とエラーになる。
    public Unscoped([UnscopedRef] Span<int> y) => _y = y;
    public Span<int> ReturnSpan([UnscopedRef] Span<int> y) => y;
    public void SetSpan([UnscopedRef] Span<int> y) => _y = y;
    public void UseSpan([UnscopedRef] Span<int> y) { }
}
```

### <a id="sec-generated-title-12"></a> <a id="caller">呼び出し元の挙動</a>

この手の機能は、
「メソッド内でできることを制限する代わりに、呼び出し元でできることを増やす」というものです。

例えば、unscoped (何も修飾子を付けていない ref 構造体)の場合、以下のように、
`Builder.Replace` の中で制限がない代わり、それを呼んでいる場所でのエラーが増えます。

```csharp {title="unscoped な挙動"}
var builder = new Builder();

Replace(ref builder);

static void Replace(ref Builder builder)
{
    Span<char> newBuffer = stackalloc char[3];
    builder.Replace(newBuffer); // ダメ。stackalloc したものが builder 越しに外に漏れる。
}

ref struct Builder(Span<char> initialBuffer)
{
    private Span<char> _buffer = initialBuffer;

    public void Replace(Span<char> value)
    {
        // 参照先自体を書き換え。
        // 引数からフィールドに参照が伝搬。
        _buffer = value;
    }
}
```

一方、scoped (`scoped` 修飾子を付けている)の場合、以下のように、
`Builder.Replace` の中で制限が掛かる代わり、それを呼んでいる場所でのエラーがなくなります。

```csharp
var builder = new Builder();

Append(ref builder);

static void Append(ref Builder builder)
{
    Span<byte> buffer = [0x61, 0x62, 0x63];
    builder.Append(buffer); // 同じようなことをしていてもこれは OK。
}


ref struct Builder(Span<char> initialBuffer)
{
    private Span<char> _buffer = initialBuffer;

    public void Append(scoped ReadOnlySpan<byte> utf8)
    {
        // 中身を書き換え。参照先自体は元のまま。
        // 引数の参照はどこにも漏らさない。
        System.Text.Encoding.UTF8.GetChars(utf8, _buffer);
    }
}
```

ちなみに、内部的には `scoped` 修飾子の方も属性で表現されています。
`scoped` 修飾子を付けた引数には `ScopedRef` 属性が付きます。
(ユーザーが自分の手でこの属性を付けることは認められていません。)

### <a id="sec-generated-title-13"></a> <a id="ref-this">構造体の this</a>

構造体の `this` は参照になっています。
この参照はデフォルトで scoped 扱いになっていて、外に漏らすことができません。

```csharp {title="this は scoped 扱い"}
using System.Diagnostics.CodeAnalysis;

struct S
{
    private int _x;

    public ref S RefThis() => ref this;

    public ref int RefX() => ref _x;
}
```

この挙動を変えるのにも `UnscopedRef` 属性が使えます。
メソッド自身に `UnscopedRef` 属性を付けることで、`this` が unscoped 扱いになります。

```csharp {title="this を unscoped 扱いに変更"}
using System.Diagnostics.CodeAnalysis;

struct S
{
    private int _x;

    [UnscopedRef]
    public ref S RefThis() => ref this;

    [UnscopedRef]
    public ref int RefX() => ref _x;
}
```

## <a id="sec-generated-title-14"></a> <a id="ref-struct-interface">ref 構造体のインターフェイス実装</a>

<h5 class="version version13">Ver. 13</h5>

C# 13 で、ref 構造体にインターフェイスを実装できるようになりました。
例えば以下のようなコードを書いてもエラーを起こしません。

```csharp {title="ref 構造体にインターフェイスを実装する例"}
ref struct S : IFormattable
{
    public string ToString(string? format, IFormatProvider? formatProvider) => "";
}
```

ただ、前述の[「スタックのみ」制約](#stack-only)のせいで直接インターフェイス型の変数に代入することは C# 13 でもできません。
以下のコードは引き続きエラーになります。

```csharp {title="インターフェイスを実装できるようになったのに、インターフェイスに代入できない"}
IFormattable f = new S();

ref struct S : IFormattable
{
    public string ToString(string? format, IFormatProvider? formatProvider) => "";
}
```

[ボックス化](rmboxing.md#boxing)を起こさないようにインターフェイス活用しようと思うと[ジェネリクス](../oop/sp2_generics.md)が必要になります。

```csharp {title="ジェネリクスでボックス化回避"}
int x = 123; // int は IFormattable を実装してる。

// これはボックス化を起こす。
IFormattable f = x;
f.ToString("X", null);

// ジェネリックメソッドを介して、
static void M<T>(T f) where T : IFormattable
    => f.ToString("X", null);

// こうやって IFormattable.ToString を呼べばボックス化を回避できる。
M(x);
```

したがって、この機能の肝は「ref 構造体をジェネリクスで使えるようにする」ということになります。

### <a id="sec-generated-title-15"></a> <a id="allows-ref-struct">allows ref struct</a>

ref 構造体に課せられている「ボックス化できない」などの制限は、C# のジェネリクスにとっては後付けなので、
そのままでは「ref 構造体の制限を満たしている」ということを保証できません。
例えば以下のコードは C# 2 以来ずっと合法なわけですが、
ボックス化を起こすコードなので ref 構造体に適しません。

```csharp
static void M<T>(T f) where T : IFormattable
{
    // object に代入するとボックス化。
    object o = f;

    // WriteLine(object) なので、これも「object への変換」でボックス化。
    Console.WriteLine(f);

    // 何ならインターフェイスへの代入でもボックス化。
    IFormattable f1 = f;
}

M(123);
```

そこで C# 13 で、`allows ref struct` というものが追加されました。
型制約の `where` 句にこの条件を書くと、型引数に ref 構造体を渡せるようになります。

```csharp {highlight-text="allows ref struct"}
static void M<T>() where T : allows ref struct
{
}

// これまで使えていた型は引き続き使える。
M<string>();
M<int>();

// これまで使えなかった ref 構造体にも使えるようになる。
M<Span<int>>();
M<ReadOnlySpan<char>>();
```

その代わり、`allows ref struct` を付けると、メソッド内でボックス化を起こすようなコードを書けなくなります。

```csharp {title="allows ref struct な型の変数はボックス化できない"}
static void M<T>() where T : allows ref struct
{
    // 先ほどのボックス化を起こすコードはすべてエラーに。
    object o = f;
    Console.WriteLine(f);
    IFormattable f1 = f;
}
```

ちなみに、通常の制約が「メソッド内でできることが増える代わりに、渡せる型が減る」というものなのに対して、
`allows ref struct` は「メソッド内でできることを減らす代わりに、渡せるが型が増える」ものになっていて、
これを「[アンチ制約](../oop/sp2_generics.md#anti-constraint)」と呼びます。

これで、ボックス化を起こさないようにインターフェイスのメンバーを呼べるようになったので、
ref 構造体のインターフェイス実装を活用できるようになります。

```csharp {title="allows ref struct なジェネリック メソッドを介して、ref 構造体のインターフェイス実装を呼ぶ"}
S x = new(); // S は IFormattable を実装してる。

// これはボックス化を起こすから C# 13 でもエラーになる。
IFormattable f = x;
f.ToString("X", null);

// allows ref struct なジェネリックメソッドを介して、
static void M<T>(T f) where T : IFormattable, allows ref struct
    => f.ToString("X", null);

// こうやって IFormattable.ToString を呼べば大丈夫になった。
M(x);

ref struct S : IFormattable
{
    public string ToString(string? format, IFormatProvider? formatProvider) => "";
}
```

#### <a id="sec-generated-title-16"></a> <a id="bcl-allows-ref-struct">標準ライブラリ中の allows ref struct</a>

C# 13 で `allows ref struct` が追加されると同時に、
.NET 9 では、標準ライブラリ中のジェネリックなデリゲート型の大部分と、一部のインターフェイスの型引数に `allows ref struct` が付きました。
以下のようなコードが書けるようになっています。

```csharp {title="多くのデリゲート、一部のインターフェイスに allows ref struct"}
using System.Diagnostics.CodeAnalysis;

// 多くのデリゲートの型引数に allows ref struct が付いた。
Action<Span<char>> a = x => "123".TryCopyTo(x);
Func<Span<char>, int> b = x => x.IndexOf('1');
Predicate<Span<char>> c = x => x.Contains('1');
Comparison<Span<char>> d = (x, y) => x.SequenceCompareTo(y);
Converter<Span<char>, ReadOnlySpan<char>> e = x => x;

// 比較系のインターフェイスには大体 allows ref struct が付いた。
class C : IComparer<Span<char>>, IEqualityComparer<Span<char>>
{
    public int Compare(Span<char> x, Span<char> y) => 0;
    public bool Equals(Span<char> x, Span<char> y) => true;
    public int GetHashCode([DisallowNull] Span<char> obj) => 0;
}

ref struct S : IEquatable<S>, IComparable<S>
{
    public int CompareTo(S other) => 0;
    public bool Equals(S other) => true;
}
```

##### <a id="sec-generated-title-17"></a> <a id="ref-struct-delegate">余談: ref 構造体引数のデリゲートの自然な型</a>

C# 10 の頃にデリゲートに[自然な型](../functional/sp_delegate.md#natural-type)が入りましたが、
「可能であれば `Action`、`Action<T>`、`Func<T>` を使う」という仕様になっています。
これに対して、.NET 9 でこれらのデリゲートに `allows ref strcut` が付いたことで、「可能であれば」の範囲が広がっています。
これまでだと匿名のデリゲート型になっていたものが、`Action` や `Func` に変わることがあります。

```csharp {title=".NET 8 から 9 で型が変わる例"}
var a = (Span<char> s) => { };

// .NET 8 以前だと: <>f__AnonymousDelegate0
// .NET 9 以降だと: Action`1
Console.WriteLine(a.GetType().Name);
```

#### <a id="sec-generated-title-18"></a> <a id="ienumerable-not-allow">余談: IEnumerable 問題</a>

ref 構造体がらみで非常に多い要望の1つに、`Span<T>`、`ReadOnlySpan<T>` に対して LINQ を使いたいというものがあります。
しかし、ref 構造体にインターフェイスを実装できるようになっても、`Span<T>` に `IEnumerable<T>` は実装できなくて、この要望はかないません。
問題は、以下のように、`IEnumerator<T>` インターフェイスを戻り値に返す部分が ref 構造体と合いません。

```csharp {title="ref 構造体は IEnumerable と相性がよくない"}
using System.Collections;

ref struct Span<T> : IEnumerable<T>
{
    // res 構造体に IEnumerator を実装するのは可能。
    ref struct Enumerator(Span<T> span) : IEnumerator<T>
    {
        private readonly Span<T> _span = span;
        public T Current => default!;
        object IEnumerator.Current => null!;
        public void Dispose() { }
        public bool MoveNext() => false;
        public void Reset() { }
    }

    // 問題はここ。
    // (ジェネリックを介さず) 直接 IEnumerator<T> インターフェイスを返す必要があって、ref 構造体に合わない。
    public IEnumerator<T> GetEnumerator() => new Enumerator(this);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
```

`IEnumerator<T>` の方であれば問題なく実装できるので、`IEnumerator<T>` 版の LINQ を用意した方がいいのかという話題も出ていたりします。
