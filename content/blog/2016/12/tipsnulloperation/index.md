---
title: "小ネタ null関係の演算子"
source_url: "https://ufcpp.net/blog/2016/12/tipsnulloperation/"
content_type: "BlogEntry"
published_at: "2016-12-13T00:37:03"
updated_at: "2016-12-27T14:33:44"
tags: []
umbraco_id: 1994
parent_id: 1969
sort_order: 12
aliases: []
---

# 小ネタ null関係の演算子

今日は、`?.`とか`??`での、nullの判定方法について。

C# 6で導入されたnull条件演算子(`?.`)ですが、以下の2つの式が**ほぼ**同じ意味になります。

```csharp
x != null ? x.M() : null
```

```csharp
x ?.M()
```

「ほぼ」であって「完全に同じ」と言えないのは、`==`演算子を呼ぶか呼ばないかが変わってしまうせいです。
前者(自分で`==`を呼んでいるやつ)はオーバーロードされた`==`を呼び出しますが、
後者(`?.`を利用)は呼びません(直接nullかどうか調べます)。

例えば、以下のように、本当はnullじゃないのにnullを自称する(`x == null`がtrueになる)クラスを作ると、ちょっと変な挙動になります。

```csharp
using static System.Console;

class NonDefault<T>
{
    public T Value { get; }
    public NonDefault(T value) { Value = value; }

    public override string ToString() => Value.ToString();

    // Value が既定値のときに null と同値扱いする
    // null でないものとの x == null が true になることがある
    public static bool operator ==(NonDefault<T> x, NonDefault<T> y) =>
        ReferenceEquals(x, null) ? ReferenceEquals(y, null) || Equals(y.Value, default(T)) :
        ReferenceEquals(y, null) ? ReferenceEquals(x, null) || Equals(x.Value, default(T)) :
        Equals(x.Value, y.Value);

    public static bool operator !=(NonDefault<T> x, NonDefault<T> y) => !(x == y);
}

class Program
{
    // null の時には "null" と表示する ToString
    static string A(NonDefault<int> x) => (x != null ? x.ToString() : null) ?? "null";
    // A とほぼ同じ意味に見えて…
    static string B(NonDefault<int> x) => x?.ToString() ?? "null";

    static void Main()
    {
        WriteLine(A(new NonDefault<int>(1))); // 1
        WriteLine(B(new NonDefault<int>(1))); // 1

        WriteLine(A(null));                   // null
        WriteLine(B(null));                   // null

        // == を呼ぶ呼ばないことによる差がここで出る
        WriteLine(A(new NonDefault<int>(0))); // null
        WriteLine(B(new NonDefault<int>(0))); // 0
    }
}
```

まあ、普通、こんな`==`演算子オーバーロードの仕方はしないんですが。
というか、参照型に対する`==`オーバーロード自体めったにしないんですが。
(通常、`==`演算子を使うのは、`Dictionary`のキーにしたい不変なクラスくらいです。)

ちなみに、このメソッド`A`、`B`のコンパイル結果はそれぞれ以下のようになります。
比較のために表にして命令ごとに並べてみましょう。

A | B
---- | ----
`ldarg.0`                            | `ldarg.0`
`ldnull`                             | `brtrue.s IL_0006`
`call     NonDefault::op_Inequality` |
`brtrue.s IL_000c`                   |
`ldnull`                             | `ldnull`
`br.s     IL_0012`                   | `br.s     IL_000c`
`ldarg.0`                            | `ldarg.0`
`callvirt Object::ToString`          | `callvirt Object::ToString`
`dup`                                | `dup`
`brtrue.s IL_001b`                   | `brtrue.s IL_0015`
`pop`                                | `pop`
`ldstr    "null"`                    | `ldstr    "null"`
`ret`                                | `ret`

nullの判定方法(2行目～4行目)だけが違って、残りは全く同じです。
`==`演算子を呼ばずに直接nullを調べるなら`brtrue`命令1個でできます。

ちなみに、`brtrue`は"branch if true"の略で、
「直前の結果がtrueだったらジャンプする」という命令になります。
整数の0とか、参照型のnullとかはfalse扱い。

この挙動は[null合体演算子](../../../../study/csharp/resource/sp2_nullable.md#coalescing)(`??`)でも同様です。

## おまけ: `throw null`

話題は変わりますが、`?.`の中身をILレベルで覗いたついでと言ってはなんですが、ちょっとしたおまけ。

時々、「`throw null`と書くと、`throw new NullReferenceException()`と同じ意味になる」的な誤解(？)を見かけたりします。
コンパイル結果的には当然、全然違うんですよね。

以下のように書いた場合、

```csharp
static void X() { throw null; }
static void Y() { throw new NullReferenceException(); }
```

コンパイル結果は以下の通り。

```cil
.method private hidebysig static void  X() cil managed
{
  // コード サイズ       2 (0x2)
  .maxstack  8
  IL_0000:  ldnull
  IL_0001:  throw
} // end of method Program::X

.method private hidebysig static void  Y() cil managed
{
  // コード サイズ       6 (0x6)
  .maxstack  8
  IL_0000:  newobj     instance void [mscorlib]System.NullReferenceException::.ctor()
  IL_0005:  throw
} // end of method Program::Y
```

割かしそのまんまなILコードです。
nullをロード(`ldnull`)して、`throw`命令を実行。
要するに、前者は(`throw`命令の実行に失敗して)実行エンジンが`NullReferenceException`を作って投げていて、
後者は自分自身で作った`NullReferenceException`を投げている。
まあ、結果的には同じような挙動をするんですが。
