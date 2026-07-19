---
title: "配列のダウンキャスト"
source_url: "https://ufcpp.net/blog/2018/12/arraydowncast/"
content_type: "BlogEntry"
published_at: "2018-12-15T12:21:16"
updated_at: "2018-12-15T12:24:23"
tags: []
umbraco_id: 2196
parent_id: 2177
sort_order: 14
aliases: []
---

# 配列のダウンキャスト

今日は [`Unsafe` クラス](../unsafe/index.md)を使った配列の最適化の話。

## `object[]`

.NET Framework 1.x 時代からある古い API の中にはいくつか、
本当は `T` 型の配列なのに `object[]` で戻り値を返してくるようなメソッドがいくつかあります。
`object[]`とまでは言わないものの、基底クラスの配列で返すメソッドは多いです。

1.x 時代には[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)がなかったせいなんですが、今となっては不便ではあります。

### 例: マルチキャスト デリゲート

例を1個。
C# のデリゲートは、複数のメソッドを `+=` で繋いで、一斉に呼び出すという機能があり、
これを[マルチキャスト デリゲート](../../../../study/csharp/functional/sp_delegate.md#malticast)と言います。
例えば以下のコードは、

```csharp
Action f = null;
 
foreach (var i in new[] { 1, 2, 3, 4, 5 })
{
    f += () => Console.WriteLine($"lambda {i} invoked");
}
 
f();
```

以下のような結果を出力します。

```csharp
lambda 1 invoked
lambda 2 invoked
lambda 3 invoked
lambda 4 invoked
lambda 5 invoked
```

基本的には[イベント](../../../../study/csharp/functional/sp_event.md)のための機能で、
戻り値は想定していません。`void`戻り値以外のメソッドに使おうとするとトラブります。
以下のようなコードを書いたとすると、

```csharp
Func<int> f = null;
 
foreach (var i in new[] { 1, 2, 3, 4, 5 })
{
    f += () =>
    {
        Console.WriteLine($"lambda {i} invoked");
        return i;
    };
}
 
Console.WriteLine($"f returns {f()}");
```

最後の行の出力は

```csharp
f returns 5
```

になります。要するに、最後の1個の戻り値以外は消えてなくなります。
全ての戻り値を取りたければ以下のように、
個々のデリゲートを配列で受け取って、1つ1つ呼び出すようなコードを書きます。

```csharp
Delegate[] list = f.GetInvocationList();
foreach (Func<int> item in list)
    Console.WriteLine($"f returns {item()}");
```

`f` が `Func<int>` なんだから、`Func<int>[]` で一覧を取りたいところなんですが、
残念ながら `GetInvocationList` の結果は `Delegate[]` で帰ってきます。
それを再び `Func<int>` にダウンキャストして使うことになります。

特に、[`Task`戻り値のデリゲートを `await` したいとき](https://gist.github.com/ufcpp/a72f63d11962f7a5a9a5981b6be31f74)とかに必須の手段です。

## 配列ダウンキャスト

そしてここからが本題。

基底クラスの配列から、元の派生クラスの要素を列挙したい場合、どうするのが最速でしょうか。
`string[]` と `object[]` でベンチマークを取ってみます。

ベンチマーク全体は [Gist](https://gist.github.com/ufcpp/efb726420adc6f8183a5c7a92ff17a61) に置いておきます。
要点を抜き出すと…

比較用データ: 同じ `string` 配列を、`string[]` のフィールドと `object[]` にフィールドに格納して使います。

```csharp
string[] _stringData = new string[] { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg" };
object[] _objectData = new string[] { "a", "ab", "abc", "abcd", "abcde", "abcdef", "abcdefg" };
```

これを、以下の3パターン(+ 参考までに1パターン)のコードに与えてみます。

(1) MemberwiseCast: 要素ごとにダウンキャスト

```csharp
foreach (string s in _objectData)
    sum += s.Length;
```

(2) ArrayCast: 最初に配列自体をダウンキャスト

```csharp
var data = (string[])_objectData;
foreach (var s in data)
    sum += s.Length;
```

(3) UnsafeStructCast: 謎の最適化

```csharp
public struct Wrap<T> { public T Value; }
```
```csharp
var data = Unsafe.As<object[], Wrap<string>[]>(ref _objectData);
foreach (var s in data)
    sum += s.Value.Length;
```

(参考) Static: 最初から `string[]` の方を列挙

```csharp
foreach (var s in _stringData)
    sum += s.Length;
```

比較の結果、以下のような感じになります。

|           Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
|----------------- |---------:|----------:|----------:|-------:|---------:|
|   MemberwiseCast | 8.552 ns | 0.1170 ns | 0.1094 ns |   2.56 |     0.04 |
|        ArrayCast | 7.074 ns | 0.0952 ns | 0.0844 ns |   2.11 |     0.03 |
| UnsafeStructCast | 3.589 ns | 0.0200 ns | 0.0167 ns |   1.07 |     0.01 |
|           Static | 3.346 ns | 0.0249 ns | 0.0233 ns |   1.00 |     0.00 |

お分かりいただけるだろうか。
1要素ごとにキャストのオーバーヘッドが掛かっていそうな (1) が遅いのは当然として。
最初に1回だけオーバーヘッドが掛かってあとは大丈夫そうに見える (2) がだいぶ遅いという。
そして、「謎の最適化」を自称しているだけあって (3) が倍くらい速い。

## 謎の最適化 `Wrap<T>`

ということで、この謎の最適化が速くなる原理について。

[.NET の配列には共変性](../../../../study/csharp/oop/sp4_variance.md#covariant-array)があります。

```csharp
string[] derivedItems = { "Aleph", "Beth", "Gimel" };
object[] baseItems = derivedItems; // この代入は明示的なキャストなしでできる
```

だいたいこいつが犯人。

この状況で、`baseItems[i] = 10;` とか書いてしまうとまずいことになります。
なので、`baseItems[i]` に対していちいち型チェックが挿入されていて、
本来の型と違う型の値を代入しようとすると例外が飛びます。
その型チェックのコストが、前節の (2) が遅くなる原因。

ちなみに、共変性は参照型にしか働かないので、例えば以下のようなコードはコンパイル エラーになります。`int` は値型なので、共変ではなくなります。

```csharp
int[] derivedItems = { 1, 2, 3 };
object[] baseItems = derivedItems; // この代入は(キャストの有無によらず)認められない
```

謎の最適化 (3) が速くなる理由はここにあります。
`Wrap<T>`構造体を介することで、共変性がなくなっています。
そして、共変じゃないことがわかっているので、型チェックが挟まらなくなる。
結果的に速い。
ただし、本来変換できないはずの `object[]` から `Wrap<string>[]` への嘘ダウンキャストが必要になるので、`Unsafe` クラスが必須です。

(ちなみに、`Wrap<T>`構造体は`T`型のフィールド1つだけの構造体なので、
`object`と`Wrap<object>`のメモリ上での構造は全く同じになります。
なので、今回のような嘘ダウンキャストしても「メモリ上の配置が同じだから同じコードで動く」みたいな感じになります。)

割かしひどい話です。
`Unsafe` クラスを避けれるなら避けたいわけでして。
最初から「共変性は使わないから型チェックしないでくれ」と指示できるような、
配列に代わる手段が求められます。
ということで出ている提案が以下のようなもの。

- [Invariant Arrays](https://github.com/dotnet/corefx/issues/23689)

まあ、採用される気配ないんですけども…
普通に coreclr 内にさっきの「謎の最適化」と同種のコード入ってたりするんですけども。
