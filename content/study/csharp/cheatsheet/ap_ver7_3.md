---
title: "C# 7.3 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver7_3/"
content_type: "Article"
published_at: "2018-04-14T00:00:00"
updated_at: "2025-01-01T18:46:38"
tags: []
umbraco_id: 2146
parent_id: 1174
sort_order: 12
aliases:
  - "/csharp/cheatsheet/ap_ver7_3/"
---

# C# 7.3 の新機能

<div class="version version7_1">Ver. 7.3</div>

<table>
<tr>
<th>リリース時期</th>
<td>2018/5</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2017 15.7</li>
<li>.NET Core 2.1</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>C# 7.0～7.2のちょっとした改善</li>
</ul>
</td>
</tr>
</table>

C# 7.0 以降の「小数点リリース」も3つ目となりました。
これまでのC# 7系リリースで追加されてきた、
[タプル](ap_ver7.md#tuple)や[構造体と参照の活用](ap_ver7_2.md#ref)、[式中での変数宣言](ap_ver7.md#var-expressions)になどに関する改善が含まれています。

## <a id="sec-generated-title-1"></a> <a id="tuple-equality"></a>タプルの ==, != 比較

タプル同士を `==`、`!=` 演算子で比較できるようになりました。
以下のように、メンバーごとの`==`を[`&&`](../start/st_operator.md#short-circuit)で繋いだものに展開されます。

```csharp
void M((int a, (int x, int y) b) t)
{
    // このタプル == 比較は、
    Console.WriteLine(t == (1, (2, 3)));
    // こんな感じで、メンバーごとの == を && で繋いだものに展開される。
    Console.WriteLine(t.a == 1 && t.b.x == 2 && t.b.y == 3);
}
```

詳しくは「[==、!= での比較](../datatype/tuples.md#equality)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="ref-reassignment"></a>ref 再代入

参照引数、参照ローカル変数に対して、
参照先の値の書き換えではなく、「どこを参照しているか」自体を書き換えることができるようになりました。

```csharp
int x = 1;
int y = 2;

// x を参照。
ref var r = ref x;

// このとき、r に対する代入は x に反映される。
r = 10; // x が 10 になる。

// これが ref 再代入。
// r が y を参照するようになる。
r = ref y;

// 今度は、r に対する代入が y に反映される。
r = 20; // y が 20 になる。

Console.WriteLine((x, y)); // (10, 20)
```

また、同時に、`for`ステートメントと`foreach`ステートメントのループ変数を参照ローカル変数にできるようになりました。

詳しくは
「[ref再代入](../resource/sp_ref.md#ref-reassignment)」、
「[for/foreach のループ変数を参照に](../resource/sp_ref.md#ref-for)」で説明します。


## <a id="sec-generated-title-3"></a> <a id="var-expressions"></a>式中での変数宣言(使える場所の拡充)

C# 7.0から式中で、
[is 演算子](../datatype/typeswitch.md#is)や[出力変数宣言](../resource/sp_ref.md#out-var)を使って、
式中でも変数宣言できるようになりましたが、
いくつか制限がありました。
C# 7.3で、これまではできなかった以下の個所でも変数宣言ができるようにありました。

- [クエリ式](../start/st_scope.md#query-expression)
- [初期化子](../start/st_scope.md#initializer)

```csharp
var q =
    from s in new[] { "a", "abc", "112", "132", "451", null }
    where s is string x && x.Length > 1
    where int.TryParse(s, out var x) && (x % 3) == 0
    select s;
```

```csharp
using System;

class Derived : base
{
    public Derived(string s) : this(int.TryParse(s, out var x) ? x : -1)
    {
        // コンストラクター初期化子中で宣言した x は、コンストラクター本体内で利用可能。
        Console.WriteLine(x);
    }

    public Derived(int a) : base(out var x)
    {
        // base の場合でも同様。
        Console.WriteLine(x);
    }

    // フィールド初期化子、プロパティ初期化子中で宣言した x は、その初期化子内でのみ有効。
    public int Field = int.TryParse("123", out var x) ? x : -1;
    public int Property{ get; set; } = int.TryParse("123", out var x) ? x : -1;
}
```

詳しくは「[C# 7での新しいスコープ ルール](../start/st_scope.md#csharp7)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="constraints"></a>ジェネリック型引数に対する Enum、Delegate、unmanaged 制約

3つほど指定できる制約が増えました。

<table summary="型引数に対する制約条件(C# 7.2まで)">
	<caption>
		型引数に対する制約条件
	</caption>
	<tr>
		<th>制約の与え方</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1"><code>where T : unmanaged</code></td>
		<td markdown="1">型<code>T</code>は「[アンマネージ型](../interop/sp_unsafe.md#unmanaged-types)」である</td>
	</tr>
	<tr>
		<td markdown="1"><code>where T : Enum</code></td>
		<td markdown="1">型<code>T</code>は「[列挙型](../structured/st_enum.md)」である</td>
	</tr>
	<tr>
		<td markdown="1"><code>where T : Delegate</code></td>
		<td markdown="1">型<code>T</code>は「[デリゲート型](../functional/sp_delegate.md)」である</td>
	</tr>
</table>

詳しくは「[ジェネリック](../oop/sp2_generics.md#cs7.3)」、
「[unsafe](../interop/sp_unsafe.md#unmanaged-constraints)」、
「[[余談] 暗黙的な派生](../oop/miscimplictinherit.md#constraints)」などで説明します。

## <a id="sec-generated-title-5"></a> <a id="overload-resolution"></a>オーバーロード解決の改善

オーバーロード解決が少し賢くなって、
これまでは呼び分けできなかったようなオーバーロードを呼び分けれるようになりました。

以下のようなものがあります。

- 静的メソッドかインスタンス メソッドかの違いで解決できるようになった
- ジェネリック型制約の違いで解決できるようになった
- &nbsp; [メソッド グループ](../structured/st_function.md#key-method-group)を引数にするとき、メソッドの戻り値を見るようになった

例えば、型制約だと、以下のような拡張メソッドの呼び分けができるようになりました。

```csharp
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

詳しくは「[[雑記]オーバーロード解決](../structured/miscoverloadresolution.md)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="stackalloc-initializer"></a>stackalloc 初期化子

`stackalloc`に対して、配列と同じような初期化子を使えるようになりました。
配列同様、初期化子中の要素の型からの推論も効きます。

```csharp
// 初期化子。{ } を使って初期値を与えられる。
Span<int> x1 = stackalloc int[3] { 0xEF, 0xBB, 0xBF };

// 初期化子があるとき、サイズは省略可能。
Span<int> x2 = stackalloc int[] { 0xEF, 0xBB, 0xBF };

// 初期化子から推論できるときは型名も省略可能。
Span<int> x3 = stackalloc[] { 0xEF, 0xBB, 0xBF };
```

## <a id="sec-generated-title-7"></a> <a id="custom-fixed"></a>ユーザー定義型の fixed ステートメント利用

所定のパターンを満たす型に対して `fixed` ステートメントが使えるようになりました。
以下のように、`GetPinnableReference`という名前のメソッドを用意すれば使えます。

```csharp
readonly struct Array<T>
{
    private readonly T[] _array;
    public Array(int length) => _array = new T[length];
    public ref T this[int index] => ref _array[index];
    public int Length => _array.Length;

    // このメソッドがあれば fixed ステートメントを使えるようになる
    public ref T GetPinnableReference() => ref _array[0];
}

class Program
{
    static void Main(string[] args)
    {
        var a = new Array<int>(5);

        unsafe
        {
            // fixed (int* p = &a.GetPinnableReference()) に展開される。
            fixed (int* p = a)
            {
                for (int i = 0; i < 5; i++)
                    p[i] = i;
            }
        }

        for (int i = 0; i < 5; i++)
            System.Console.WriteLine(a[i]);
    }
}
```

詳しくは「[ユーザー定義型の fixed ステートメント利用](../interop/sp_unsafe.md#custom-fixed)」で説明します。

## <a id="sec-generated-title-8"></a> <a id="others"></a>その他

その他、ほぼ「バグ修正」レベルの改善が2点あります。

### <a id="sec-generated-title-9"></a> <a id="field-attribute-on-auto-property"></a>自動プロパティのバック フィールドに対する field 属性指定

前者は、[自動プロパティ](../oop/oo_property.md#auto)に対して `field` 指定の属性が付けられるようになりました。

```csharp
using System;

class XAttribute : Attribute { }

class Sample
{
    [field:X] // 自動実装で生成されるフィールドに対する属性の指定
    public int AutoProperty { get; }
```

詳しくは「[プロパティ、イベントと属性の対象](../dynamic/sp_attribute.md#auto-impl)」で説明します。

### <a id="sec-generated-title-10"></a> <a id="movable-fixed-buffer"></a>固定長バッファーの読み書きで、fixed ステートメント不要に

[固定長バッファー](../interop/sp_unsafe.md#fixed-buffer)の読み書きをする際、
[`fixed`ステートメント](../interop/sp_unsafe.md#fixed)が不要になる場面が増えたそうです。

```csharp
unsafe struct Buffer
{
    public fixed byte A[8];
}

class Program
{
    static Buffer _buffer;

    unsafe static void Main()
    {
        var buffer = new Buffer();
        buffer.A[0] = 1; // 元々 OK
        _buffer.A[0] = 2; // C# 7.3 から OK

        RefFixedBuffer(ref buffer);

        System.Console.WriteLine(buffer.A[0]);  // 元々 OK
        System.Console.WriteLine(_buffer.A[0]); // C# 7.3 から OK
    }

    unsafe static void RefFixedBuffer(ref Buffer buffer)
    {
        buffer.A[1] = 3; // C# 7.3 から OK
    }
}
```

[提案文書](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-7.3/indexing-movable-fixed-fields.md)にすら、「言語仕様上どうしてこの条件緩和が許されるのかを説明するのが難しい」とか書かれる始末な機能です…

本来はポインター操作になるので`fixed`ステートメントが必須なんですが、
C# コンパイラー的には[参照ローカル変数](../resource/sp_ref.md#ref-returns)と同じようなコード生成するらしく、
だったら`fixed`がなくても平気なはず、と言うことらしいです。
