---
title: "ピックアップRoslyn 1/13: null 許容参照型改善、式ブロック"
source_url: "https://ufcpp.net/blog/2020/1/pickuproslyn0113/"
content_type: "BlogEntry"
published_at: "2020-01-13T11:23:44"
updated_at: "2020-01-13T11:23:44"
tags: []
umbraco_id: 2280
parent_id: 2279
sort_order: 0
aliases: []
---

# ピックアップRoslyn 1/13: null 許容参照型改善、式ブロック

1件、Design Note 追加。

- [C# Language Design Meeting for Jan. 6, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-01-06.md)

C# 9.0 に向けた [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)の改善と、「式ブロック」の話。

## null 許容参照型

### 属性のメソッド内への適用

C# 8.0 だと、以下のような感じのコードの null 警告は [`!` 演算子](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving) で無視する以外に消す方法がありません。

```csharp {title="属性がメソッド内に効いてない" warning-ranges="3:9-3:13,9:12-9:16"}
bool TryGetValue<T>([NotNullWhen(true)]out T t) where T: class
{
    t = null; // 今、警告が出る
    return false;
}
[return: MaybeNull]
T GetFirstOrDefault<T>() where T : class
{
    return null; // 今、警告が出る
}
```

C# 8.0 ではスケジュール都合で放置(属性をメソッドの中にまで反映させるのは結構大変＆これを使う人(= ライブラリ作者側 << 利用側)は少ない)されてたやつです。
機能要望どころか、バグだと思われてバグ報告がたびたび入ります(重複 issue がたくさんある)し、9.0 で再検討。
今回の Design Meeting でも「有益度合いの調査しないとね。有益そうなら実装したい」みたいな雰囲気。

### Task-like の変性

Task-like (= 要は非同期メソッドの戻り値)の[変性](../../../../study/csharp/oop/sp4_variance.md)も不便な場面がよくあります。

```csharp {title="Task の null 許容共変性" warning-ranges="2:22-2:25"}
Task<string> A() => Task.FromResult("");
Task<string?> B() => A(); // async/await が付いていればOKなものの、この書き方だと警告
```

これも要望が多いわけですが…
というか、これに関してはそもそも `Task<object> x = new Task<string>();` 的な、(null 許容性関係ない)一般の共変性を認めてほしいという話もありましが、
そっちは「クラスの変性を認めるのは大変」ということでここではあくまで `?` の有無(null 許容性違いの変性)だけの話です。

Task-like (`Task` とか `ValueTask` とか、非同期メソッドの戻り値に使うもの)だけ特別扱いするのも気持ち悪い話なんですが、
特別扱いというなら今、どうもそもそも、`IEnumerable<T>` だけ特別扱いしているそうなので今更とのこと。

```csharp {warning-ranges="11:21-11:23"}
interface I<in T> { }
class C<T> : I<T> { }
 
static void Main()
{
    // string は string? に代入可能
    string s1 = "";
    string? s2 = s1;
 
    I<string> x1 = new C<string>();
    I<string?> x2 = x1; // 一般にはここで警告。in が付いてても ? 違いの共変性はない
 
    // でも、IEnumerable だけは特別扱いして共変らしい
    IEnumerable<string> e1 = new string[1];
    IEnumerable<string?> e2 = e1;
}
```

なので、Task-like を特別扱いするの自体はありだろうという雰囲気。
ただ、「オーバーロード解決とかで問題起こさないかとか要検証」という感じ。

### キャスト

今、以下のようなコードは警告が出るようになっています。非 null 型にキャストしたければそれより前に null チェックが必須。

```csharp {title="null チェックなしで非 null な型にキャスト" warning-text="(string)nullable"}
static void M(string? nullable)
{
    string nonNull = (string)nullable;
}
```

これも意図的にそうしてるんですが、「うっとおしいと思う場面と有益だと思う場面、どちらが多いか」ということで再検討。
例えば Roslyn (C# コンパイラー自身のコード)内では `(object)x == null` という書き方をよくするそうです。
ユーザー定義演算子を無視して確実に参照比較で null チェックしたいというもの。
ここで null 警告が出ちゃうので結構うっとおしいとのこと。
ただ、Roslyn 内では多くても、一般にこの書き方が多いかといわれるとそんなことはなくて少数派。

結論としては今のまま、警告は出す方向でいきたいとのこと。

##  式ブロック

前々から、式の中に書けるものを増やしたいという話はたびたびあります。
例えば、[宣言式](https://github.com/dotnet/csharplang/issues/973) (`var x = F()` みたいな変数宣言を式中に書けるようにする話)と、
[シーケンス式](https://github.com/dotnet/csharplang/issues/377) (`(var x = F(); x * x)` みたいな `;` 区切りで複数の式をつないで、最後の1個の値を返す)提案が出ていたりします。

C# 8.0 で [`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)が入ったことで、その枝(`x switch { c1 => e1, _ => e2 }` とかの `e1`、`e2` の部分)にちょっと凝ったものを書きたいという欲求も増えたので、「[Block-bodied switch expression arms](https://github.com/dotnet/csharplang/issues/377)」なんていう提案も出ています。

今回の Design Meeting では、この辺りの似て非なる提案はまとめて検討して、統合、もしくは、少なくとも一貫性のある文法を考えないとまずいだろうという話。

それに伴って、`switch` 式の枝に限らず式中のどこにでもステートメント(`if` とか `for` とか)を書けるようにする構文として「式ブロック」が提案されました。

- [Proposal: Expression blocks #3086](https://github.com/dotnet/csharplang/issues/3086)

`{ var x = F(); x * x }` みたいに、`{}` 中の最後の1個を式にすることで、ブロック全体を「最後の式の値を返す式」にするというもの。

末尾のたった1個の `;` の有無で意味が変わるというのはだいぶ気持ち悪いんですが… 他に適切な記号もなさげ。

```csharp {title="; の有無で意味が違う"}
f = () => { F(); G(); }; // block body
f = () => { F(); G() };  // expression body
```

`switch` 式の枝の方の話だけに留めるか、式ブロックの方までやるべきか、とりあえず C# 9.0 で何かしら実装する前提で調査は始めようという感じみたいです。
