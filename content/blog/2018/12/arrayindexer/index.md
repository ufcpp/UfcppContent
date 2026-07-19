---
title: "配列のインデクサー"
source_url: "https://ufcpp.net/blog/2018/12/arrayindexer/"
content_type: "BlogEntry"
published_at: "2018-12-13T10:52:27"
updated_at: "2018-12-13T10:52:27"
tags: []
umbraco_id: 2194
parent_id: 2177
sort_order: 12
aliases: []
---

# 配列のインデクサー

C# 8.0 がらみの話も一段落してしまったので、
今日からしばらく予告通り、Gist に書き捨ててたもののブログ化になります。
ぶっちゃけ、在庫一掃処分セールみたいなものなので過度な期待はしないでください。

今日は C# コンパイラーと JIT レベルの最適化の話。

## 配列の範囲チェック

.NET の配列は、バッファオーバーランとかのメモリ破壊を避けるべく、範囲チェックがかかっています。

```csharp
using System;
 
class Program
{
    static void Main()
    {
        var a = new int[4];
        var x = a[5]; // 範囲外なのでここで IndexOutOfRangeException が飛ぶ
        Console.WriteLine(x);
    }
}
```

`a[5]` のところのコンパイル結果は以下のようになります。

IL:

```cil
IL_0006: ldc.i4.5   // 5 をロード
IL_0007: ldelem.i4  // 配列要素の読み込み命令
```

IL には配列の要素を読み書きする命令があります。
この時点では `ldelem` 命令が出力されているだけで、
例外を飛ばすコードはありません。
範囲チェックが挿入されるのはその次の、JIT でネイティブ コード化される段階になります。

x86 コード:

```csharp
L000f: cmp dword [eax+0x4], 0x5 ; 配列長と 5 を比較
L0013: jbe L001e                ; 例外を投げるコードにジャンプ
L0015: mov ecx, [eax+0x1c]      ; a[5] の場所のデータを読み込み
```

元のコードにはない比較・ジャンプ命令が挟まっています。

## 配列の列挙

今度は、全要素列挙することを見てみましょう。
以下のようなコードを考えます。

```csharp
void M(int[] array)
{
    for (var i = 0; i < array.Length; ++i)
    {
        var x = array[i];
        Console.WriteLine(x);
    }
}
```

この場合は、ループ付近が以下のようにコンパイルされます。

IL:

```cil
IL_0004: ldarg.1
IL_0005: ldloc.0
IL_0006: ldelem.i4                  // array[i]
IL_0007: call void WriteLine(int32)
IL_000c: ldloc.0
IL_000d: ldc.i4.1
IL_000e: add
IL_000f: stloc.0
IL_0010: ldloc.0
IL_0011: ldarg.1
IL_0012: ldlen
IL_0013: conv.i4                    // i < Length
IL_0014: blt.s IL_0004
```

x86 コード:

```csharp
L0008: xor esi, esi
L000a: mov ebx, [edi+0x4]
L000d: test ebx, ebx
L000f: jle L001f
L0011: mov ecx, [edi+esi*4+0x8]  ; array[i]
L0015: call WriteLine(Int32)
L001a: inc esi
L001b: cmp ebx, esi              ; i < Length
L001d: jg L0011
```

`for` ステートメント中の `i < Length` 相当のコードはありますが、
その他の比較はありません。
要するに、単品だと`array[i]`のところに挟まっていた範囲チェックが、このループでは消えています。

これは JIT が行っている最適化で、要するに、

- 何もしなければ、`array[i]` のところに暗黙的な範囲チェックを追加する
- 明示的な範囲チェックがあれば、余計な範囲チェックの追加はしない

という挙動になります。
なので、安全性は保たれつつ、ループの速度は落としません。

## foreach 最適化

次に以下のコードを考えます。
先ほどの `for` を使ったコードとやっていることは全く一緒です。
配列の全要素の列挙。

```csharp
void M(int[] array)
{
    foreach (var x in array)
    {
        Console.WriteLine(x);
    }
}
```

こいつは以下のようにコンパイルされます。

IL:

```cil
IL_0006: ldloc.0
IL_0007: ldloc.1
IL_0008: ldelem.i4                  // array[i]
IL_0009: call void WriteLine(int32)
IL_000e: ldloc.1
IL_000f: ldc.i4.1
IL_0010: add
IL_0011: stloc.1
IL_0012: ldloc.1
IL_0013: ldloc.0
IL_0014: ldlen
IL_0015: conv.i4                    // i < Length
IL_0016: blt.s IL_0006
```

`ldloc` (ローカル変数読み込み)の後ろの番号とかが違うだけで、
他は先ほどの `for` のコードと全く同じです。
(要するに、「変数名が違うけど同じロジック」程度の差です。)

`GetEnumerator`とか`MoveNext`、`Current`は一切出てきません。
代わりにインデックスの `++i` とか `array[i]` 相当のコードが出てきます。

要するに、C# コンパイラーは、配列の `foreach` を見たら `for (var i ...` 相当のコードに変換します。

## 配列の一部分を列挙

配列全体の列挙に対して結構よい最適化がかかっていることはわかりました。
次は、一部分だけ列挙することを考えます。

```csharp
void M(int[] array, int start, int count)
{
    var end = start + count;
    for (var i = start; i < end; ++i)
    {
        var x = array[i];
        Console.WriteLine(x);
    }
}
```

これを同じようにコンパイルすると…
ループ近辺だけ抜き出すと以下のようになります。

x86 コード:

```csharp
L0017: mov eax, [edi+0x4]
L001a: mov [ebp-0x10], eax
L001d: mov eax, [ebp-0x10]
L0020: cmp esi, eax        ; ここの比較は array[i] に対して暗黙的に追加されるもの
L0022: jae L003a           ; 例外を投げるコードへのジャンプ
L0024: mov ecx, [edi+esi*4+0x8]
L0028: call System.Console.WriteLine(Int32)
L002d: inc esi
L002e: cmp esi, ebx        ; ここの比較は i < end
L0030: jl L001d            ; これはループを抜ける・抜けないの分岐
```

さすがに、`array.Length` 以外のものまで見て最適化は掛けてくれないみたいです。
ちなみに、事前に `start`、`end`の範囲チェックをしてもダメ。

## そこで `Span<T>`

C# 7.2 では、[`Span<T>`](../../../../study/csharp/resource/span.md)がらみの対応がいろいろ入ったわけですが、このタイミングで、`Span<T>`に対する列挙の最適化も入っています。

まず、配列同様、`Span<T>`に対する`foreach`は`for`に最適化されます。
例えば、以下の2つのメソッドはほぼ同じ IL にコンパイルされます。

```csharp
public void M1(Span<int> array)
{
    for (var i = 0; i < array.Length; ++i)
    {
        var x = array[i];
        Console.WriteLine(x);
    }
}
 
public void M2(Span<int> array)
{
    foreach (var x in array)
    {
        Console.WriteLine(x);
    }
}
```

また、JIT 時の最適化で、「暗黙の範囲チェック」も消えてくれるようです。
要するに、先ほどの `for (var i = start; i < end; ++i)` なループよりも、
以下のような、`Span<T>`を介したコードの方が最適化がかかりやすいです。

```csharp
public void M2(int[] array, int start, int count)
{
    foreach (var x in array.AsSpan(start, count))
    {
        Console.WriteLine(x);
    }
}
```

ちなみに、今のところこういう最適化がかかるのは配列と `Span<T>` だけです。
`Span<T>` だけ特別扱いするのもちょっと嫌な話で、
もっと汎用的に、所定のパターンを満たした型なら `foreach` を `for (var i ...` に変換できるような仕組みも一応検討はされています。
(`Span<T>`以外に対する需要はそこまで高くないので、優先度は低め。)
