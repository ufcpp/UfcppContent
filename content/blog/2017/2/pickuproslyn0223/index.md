---
title: "ピックアップRoslyn 2/23: Shapes and Extensions"
source_url: "https://ufcpp.net/blog/2017/2/pickuproslyn0223/"
content_type: "BlogEntry"
published_at: "2017-02-23T01:24:33"
updated_at: "2017-02-23T01:24:33"
tags: []
umbraco_id: 2045
parent_id: 2036
sort_order: 4
aliases: []
---

# ピックアップRoslyn 2/23: Shapes and Extensions

久しぶりに面白そうな話が。

- [Exploration: Shapes and Extensions #164](https://github.com/dotnet/csharplang/issues/164)

2つほど原案があって、組み合わせて結構よさげな機能案ができたので詳細を詰めていきたいという感じの話。
元になっているのは以下の2つ。

- [Extension everything](https://github.com/dotnet/roslyn/issues/11159)
  - メソッドだけじゃなくて、プロパティとかインデクサーとかあらゆるものを「拡張」定義したい
  - 静的メソッドも(インスタンスじゃなくて型に対して)「拡張」したい
  - インターフェイスの後刺しとかもしたい
- [Type Classes](https://github.com/dotnet/csharplang/issues/164)
  - Haskellの型クラス的なもの、 .NET ランタイムに手を入れなくてもちょっとした「値型ジェネリック」を使ったトリックで実現できそうという話
  - [MS Researchの人と、インターンで来た人の成果っぽい](https://github.com/MattWindsor91/roslyn/blob/master/concepts/docs/csconcepts.md)
  - 型クラスってのはどういうのかというと:
      - 静的メソッドやコンストラクターを含めて、その型が持つべきメンバーを規定するもの
      - 「型、インスタンスどちらにも有効なインターフェイス」みたいな感じ

## Shapes (型クラス)

今のところ型クラスの定義を Shapes って呼びたいみたい。
要するに、どういうメンバーを持っているべきかという、型の「形状」(shape)を規定するもの。

例として挙げてるのが、零元と加算演算子を持つ型。
数学で「[群](../../../../study/math/group/group.md)」(group)って呼んでるやつです。
(正確にはこれだと[モノイド](../../../../study/math/group/group.md#monoid)なんですけど)

```csharp {title="群を表すShape"}
public shape SGroup<T>
{
    static T Zero { get; }
    static T operator +(T t1, T t2);
}
```

概ねインターフェイスと似たような書き方なわけですが、以下の点が異なります。

- `interface`キーワードの代わりに`shape`キーワードを書く
- 静的メンバーを持てる

静的メンバーが持てるので、ジェネリックの残念な点の1つとしてよく上がる「演算子を使えない」問題が解消します。

わざわざShapes(インターフェイス的なもの)をかますのは、いろんな型、いろんな演算子がこの条件を満たすからです。
例えば:

1. 整数の加法群: 整数に対して、零元 = 0、演算子 = +
2. 整数の乗法群: 整数に対して、零元 = 1、演算子 = ×
3. 文字列に対して、零元 = 空文字、演算子 = 文字列結合

これらに対して、共通ロジックで「総和」を取ったりできるわけです。

```csharp {title="Shapeに対する共通ロジック"}
public static T AddAll<T>(T[] ts) where T : SGroup<T> // shape 制約
{
    var result = T.Zero;                   // 静的プロパティから零元を取得
    foreach (var t in ts) { result += t; } // + 演算子の呼び出し
    return result;
}
```

例えば、

- 1, 2, 3, 4 に対して上記1の意味で総和(普通に足し算) → 10
- 1, 2, 3, 4 に対して上記2の意味で総和(全部を掛け算) → 24
- "ab", "c", "de" に対して上記3の意味で総和(Concat) → "abcde"

とかになります。

## Extensions

で、拡張(extensions)。
拡張メソッドだけじゃなくて、プロパティだろうがインデクサーだろうが、静的メソッドだろうがコンストラクターだろうが、なんでも拡張できるようにしたいという話が元々ありまして。
この「拡張」で、Shapesを実装できるようにするみたいです。

例えば上記1の意味のShape (整数の加法群)であれば、以下のように書けます(構文は「仮」なもの。特に `of` の辺りが今後どうなるか怪しい)。

```csharp {title="「群」Shapeに対する「整数の加法群」実装"}
public extension IntGroup of int : SGroup<int>
{
    public static int Zero => 0;
    public static int operator +(int t1, int t2) => t1 + t2;
}
```

2のやつ(乗法群)であれば以下の通り。

```csharp {title="「群」Shapeに対する「整数の乗法群」実装"}
public extension IntMulGroup of int : SGroup<int>
{
    public static int Zero => 1;
    public static int operator +(int t1, int t2) => t1 * t2;
}
```

呼び出し側は以下のように書けます。

```csharp {title="共通ロジック呼び出し"}
// 全部を足し算。sum == 10
var sum = AddAll<IntGroup>(new[] { 1, 2, 3, 4 });

// 全部を掛け算。prod == 24
var prod = AddAll<IntMulGroup>(new[] { 1, 2, 3, 4 });
```

## 展開

構文的には割かしよさそうです。
で、気になるのはどう実現するか。
メタデータ的に(リフレクションや旧バージョンのコンパイラーから見たの時の挙動が)どうなるのかとか、
動的リンクできるのか(コンパイル時にベタ展開しちゃうと、変更時に利用側の再コンパイルが必要)とか、
パフォーマンス的に大丈夫かとか。

とりあえず、予定されている展開結果をお見せしましょう。
これまで挙げてきたコードは、以下のように展開されます。
(比較のために元のコードを並べつつ)

### Shapes

元:

```csharp {title="群を表すShape"}
public shape SGroup<T>
{
    static T Zero { get; }
    static T operator +(T t1, T t2);
}
```

展開結果:

```csharp {title="SGroup shape の展開結果"}
// shape はべたにインターフェイス化
// 静的なものもインスタンス メンバーに変更
public interface SGroup<T>
{
    T Zero { get; }
    T op_Addition(T t1, T t2); // 演算子は所定の命名ルールでメソッド化
}
```

### Extensions

元:

```csharp {title="「群」Shapeに対する「整数の加法群」・「整数の乗法群」実装"}
public extension IntGroup of int : SGroup<int>
{
    public static int Zero => 0;
    public static int operator +(int t1, int t2) => t1 + t2;
}

public extension IntMulGroup of int : SGroup<int>
{
    public static int Zero => 1;
    public static int operator +(int t1, int t2) => t1 * t2;
}
```

展開結果:

```csharp {title="IntGroup/IntMulGroup extensionsの展開結果"}
// extension による shape 実装は、構造体でのインターフェイス実装に
public struct IntGroup : SGroup<int>
{
    public int Zero => 0;
    public int op_Addition(int t1, int t2) => t1 + t2;
}

public struct IntMulGroup : SGroup<int>
{
    public int Zero => 1;
    public int op_Addition(int t1, int t2) => t1 * t2;
}
```

### Shapes に対する共通ロジック

元:

```csharp {title="Shapeに対する共通ロジック"}
public static T AddAll<T>(T[] ts) where T : SGroup<T> // shape 制約
{
    var result = T.Zero;                   // 静的プロパティから零元を取得
    foreach (var t in ts) { result += t; } // + 演算子の呼び出し
    return result;
}
```

展開結果:

```csharp {title="AddAll メソッドの展開結果"}
// 型引数を1個追加。そこに Shape を渡す想定
public static T AddAll<T, TShape>(T[] ts)
    where TShape : SGroup<T> // shape 
{
    var tShape = default(TShape); // 静的メソッド呼び出しだったものは、構造体の既定値に対するメソッド呼び出しに展開
    var result = tShape.Zero;
    foreach (var t in ts) { result = tShape.op_Addition(result, t); }
    return result;
}
```

### メソッド呼び出し

元:

```csharp {title="Shape呼び出し"}
// 全部を足し算。sum == 10
var sum = AddAll<IntGroup>(new[] { 1, 2, 3, 4 });

// 全部を掛け算。prod == 24
var prod = AddAll<IntMulGroup>(new[] { 1, 2, 3, 4 });
```

展開結果:

```csharp {title="AddAll メソッド呼び出しの展開結果"}
// 型引数を1個追加
var sum = AddAll<int, IntGroup>(new[] { 1, 2, 3, 4 });
var prod = AddAll<int, IntMulGroup>(new[] { 1, 2, 3, 4 });
```

## 展開結果から言えること

### メタデータ

展開してみればそこまで変なコードではないので、リフレクションや、古いコンパイラーから使う場合でもそんなにおかしなコードにはならないと思います。

### 動的リンク

結局はただのインターフェイスなので、ちゃんと動的リンクできます。
要するに、

- ライブラリの実装側を修正する
- 利用側は再コンパイル不要
- JIT 実行するとちゃんと修正結果が反映される

という状態になります。

### パフォーマンス

構造体＋ジェネリックに展開されるのがポイントでして。
ちょうど、ついこないだ以下のような文章を書いたんですが、これと同じ状態になっています。

- [静的メソッド代わり](../../../../study/csharp/oop/sp2_generics.md#pseudo-static)

値型に対するジェネリックは、JIT コンパイル時に静的に展開されます。
元々インターフェイスの仮想メソッド呼び出しだったものも、通常のメソッド呼び出しに展開されます。
仮想呼び出しが消えることで、インライン展開すらされます。

もしインライン展開までされれば、きわめて高パフォーマンスに実行できます。
ここまで例示してきたような`SGroup`程度、
すなわち、0/1を返したり`+`/`*`を計算するだけのものなら、
リリース(最適化オプション付き)ビルドすれば確実にインライン展開されます。

結果的に、以下のようなコードとほとんど変わらない状態に最適化されます。

```csharp {title="値型ジェネリックの展開 ＋ インライン展開の結果"}
public static int AddAll_IntGroup(int[] ts)
{
    var result = 0;
    foreach (var t in ts) { result += t; }
    return result;
}

public static int AddAll_IntMulGroup(int[] ts)
{
    var result = 1;
    foreach (var t in ts) { result *= t; }
    return result;
}
```

## まとめ

なんか急にきれいにまとまった感じ。

これまで「なんでも拡張」(Extension everything)をどう実現しようか迷っていたところに、
低コストで型クラス(Type Classes)を実現できそうという案が降ってきたことで、
合わせるといい感じに。

実装的には値型ジェネリックを使ったちょっとしたトリックになっています。
値型ジェネリックに対して .NET の JIT が結構いい最適化を掛けてくれることによって、

- リフレクションや古いコンパイラーから見てもそこまで苦しくなく
- 動的リンク可能で
- きわめて低コスト(静的メソッドとほとんど変わらない高パフォーマンス)

が実現できそうです。

このブログでは触れませんでしたが、[元 issue](https://github.com/dotnet/csharplang/issues/164)では、インターフェイスの後刺しとかについても書かれています。
