---
title: "null の取り扱い"
source_url: "https://ufcpp.net/study/csharp/resource/rm_nullusage/"
content_type: "Article"
published_at: "2014-10-06T00:00:00"
updated_at: "2025-12-20T00:00:00"
tags:
  - "Ver. 6.0"
umbraco_id: 1294
parent_id: 1286
sort_order: 12
aliases:
  - "/csharp/resource/rm_nullusage/"
  - "/csharp/rm_nullusage"
  - "/csharp/rm_nullusage.html"
  - "/study/csharp/rm_nullusage"
  - "/study/csharp/rm_nullusage.html"
---

# null の取り扱い

## <a id="sec-generated-title-1"></a> <a id="abst">概要</a>

null が来た時にできる対処はいくつかあります。

- null が来たら単に null を返す (対処は他の誰かに委ねる)
- null が来たら何か適当な有効な値で埋める
- null が来たら何も処理しない
- null を完全に認めない

ここでは、それぞれについて C# での書き方について説明して行きます。

## <a id="sec-generated-title-2"></a> <a id="null">null</a>

C# の[参照型](oo_reference.md#reftype)には null (無効な値、何もない、ゼロ)という、無効な参照を表す特別な値があります。
また、[null許容型](sp2_nullable.md)を使うことで、
本来は無効な値を持たない[値型](oo_reference.md#valtype)に対しても無効な状態を表すことができます。

詳しくは外部で書いた記事「[nullが生まれた背景と現在のnullの問題点](https://www.buildinsider.net/column/iwanaga-nobuyuki/011)」で書いていますが、null はちょっと妥協の産物で、今となっては無い方がいいとも言われます。
例えば2010年代以降に生まれたプログラミング言語であれば、

- 既定では「無効」という状態を認めない
  - 必ず有効な値での初期化を求める
- 「無効」が欲しいなら、別途 `Optional<T>` と言うような、「無効な値、もしくは、`T` 型の有効な値」を表す型を使う

となっているものが増えてきています。

C# では、値型 `T` の場合にはこれと同じような状態になっています。

- 値型では「`T` の無効な値」を表すものがない
- 「無効」が欲しいなら `?` を付けて、[null許容型](sp2_nullable.md)にする
  - 「無効」を表すのに `T?` の null を使う

一方で参照型の場合、
C# 8.0 以前は意図して null (無効)を認めているのかどうかがわからないという問題がありました。
このせいで、「メソッド実装側は null を想定してないのに、呼び出し側が null を渡してしまった」などの齟齬が起こっていました。

この参照型の問題に対して C# 8.0 では[ null 許容参照型](nullablereferencetype.md)というものを導入しました。
null 自体はなくせないものの、少なくとも「意図して null を使っているかどうか」だけは表せるようになっています:

- 参照型でも単に型 `T` と書くと null を認めない
- `T?` と書いた場合だけ null を認める

いずれにせよ、C# では、null を「無効な値」として使われます。

そして、概要でも書きましたが、null が来た時にできる対処はいくつかあります。

- null が来たら単に null を返す (対処は他の誰かに委ねる)
- null が来たら何か適当な有効な値で埋める
- null が来たら何も処理しない
- null を完全に認めない

## <a id="sec-generated-title-3"></a> <a id="for-instance">例題</a>

最初に、本項の残りの部分の例として使うクラスを1セット用意しておきましょう。

例えば、ゲームで使いそうなデータ構造で考えてみます。
「無効な値」というか、null を「空欄」的な意味で使うことを考えます。

- 武器の装備欄は固定で4つある
- 「1つ目と3つ目に武器を持っていて、2つ目と4つ目には何も持っていない」みたいに、歯抜けがあって、かつ、何番目かの順序も保つ
- 以下のように異なる種類の画面がある
    - 空欄は飛ばして詰めて表示したい画面
    - 空欄には空欄画像を出したい画面

これを、以下のように表現してみましょう。

```csharp
// 武器装備欄
class WeaponSlots
{
    // 空欄のところには null を入れる
    public Weapon? Weapon1 { get; }
    public Weapon? Weapon2 { get; }
    public Weapon? Weapon3 { get; }
    public Weapon? Weapon4 { get; }
}

// 武器
class Weapon
{
    // 基礎攻撃力
    public int Attack { get; }

    // 画像の URL
    public string? ImagePath { get; }

    // パラメーターを for 列挙できるように
    public int this[int parameterIndex] => parameterIndex switch
    {
        0 => Attack,
        // 実際は他のパラメーター種別もあるとして…
        _ => throw new IndexOutOfRangeException(),
    };
}
```



この例では、何も装備していない欄を表すのに null を使うことにします。

そして、装備確認・変更画面を作ることを考えます。
これを以下ことなどを考えてみましょう。

- `Weapon`から画像URLを得る
- 画像URLを渡して、その画像ロードする
- ロードした画像を表示する

## <a id="sec-generated-title-4"></a> <a id="null-conditional">null 条件演算子(null が来たら null を返す)</a>

<h5 class="version version6">Ver. 6.0</h5>

まず、`Weapon`から画像URL (`string`)を得る部分だけを見てましょう。
この時点では、null(空欄)だったらnull(無効なURL)を返すことにしましょう。
(もちろん実装によっては、この時点で「空欄だったら空欄画像を表すURLを返す」という仕様もあるかもしれませんが、ここではとりあえずこの仕様でいきます。)

この処理は、以下のように書くこともできます。

```csharp
static string? M(Weapon? w)
{
    if (w == null) return null;
    else return w.ImagePath;
}
```

あるいはこれと全く同じコードを条件演算子を使って以下のように書いたりします。

```csharp
static string? M(Weapon? w)
{
    return w == null ? null : w.ImagePath;
}
```

この類の「null が来たら null を返す」という処理はそれなりに頻出します。
そこで、もっと楽に書けるように、C# 6.0 で<strong id="key-null-conditional" class="keyword">null条件演算子</strong>(null conditional operator)と言うものが導入されました。
null条件演算子は、メンバー アクセスのための `.` の代わりに `?.` を使うことで「null が来たら null を返す」という挙動をします。
すなわち、以下のコードで、先ほどと同じ挙動をします。

```csharp
static string M(Weapon? w) => w?.ImagePath;
```

### <a id="sec-generated-title-5"></a> <a id="null-conditional-indexer">インデクサーに対するnull条件演算子</a>

インデクサーの前にも、`?`を付けることでnull条件付きにできます。

```csharp
static int? M(WeaponSlots w) => w.Weapon1?[0];
```

これは以下のようなコードとほぼ同じ意味になります。

```csharp
static int? M(WeaponSlots w)
{
    var w1 = w.Weapon1;
    if (w1 == null) return null;
    else return w1[0];
}
```

### <a id="sec-generated-title-6"></a> <a id="null-conditional-to-nullable">補足: null許容型に対するnull条件演算子</a>

null 条件演算子 `?.` を使えば、[null許容型](sp2_nullable.md)のメンバー アクセスが少し楽になります。
例えば以下のコードでは、`x` の行はコンパイル エラーになりますが、`y` の行は OK です。

```csharp
// さっきと違って Weapon が構造体
struct Weapon
{
    // 基礎攻撃力
    public int Attack { get; }

    // 画像の URL
    public string ImagePath { get; }
}

class Program
{
    // Weapon を構造体にしたので、null が使いたければ null 許容型にする(? を付ける)
    static void M(Weapon? w)
    {
        // null 許容型に対して直接 . でメンバー アクセスはできない。
        // (. でアクセスできるのは Nullable<T> 構造体の HasValue や Value などのメンバーだけ)
        var x = w.ImagePath;

        // ?. なら使える。
        var y = w?.ImagePath;
    }
}
```

### <a id="sec-generated-title-7"></a> <a id="void-null-conditional">null じゃないときだけメソッド呼び出し</a>

null 条件演算子 `?.` は戻り値がない(戻り値が `void` の)メソッドに対しても使えます。
この場合、`?.` の結果も「戻り値がない」(`void`)扱いです。

例えば、`WeaponSlots` にも `Weapon` にも `Dispose` メソッドを用意したとして、
`WeaponSlots` は `Weapon1` などが null じゃないときだけその `Dispose` を呼ぶとしたい場合、以下のように書けます。

```csharp
public void Dispose()
{
    Weapon1?.Dispose();
    Weapon2?.Dispose();
    Weapon3?.Dispose();
    Weapon4?.Dispose();
}
```

これは以下のようなコードとほぼ同じ意味です。

```csharp
public void Dispose()
{
    if (Weapon1 != null) Weapon1.Dispose();
    if (Weapon2 != null) Weapon2.Dispose();
    if (Weapon3 != null) Weapon3.Dispose();
    if (Weapon4 != null) Weapon4.Dispose();
}
```

戻り値はないので、以下のようなコードは書けません。

```csharp
// void の ?. 結果は void。
// 何の値も返って来ず、変数に受けたりはできない。
var x = Weapon1?.Dispose();
```

### <a id="sec-generated-title-8"></a> <a id="null-conditional-delegate">補足: デリゲートの呼び出し</a>

`?[]` が行けるのなら、デリゲート呼び出し時に `?()` も行けそうに思えますが、
これは認められていません。
条件演算子 `? :` との弁別が少し面倒で、需要の割に実装するリスクが大きいとのことで認めていないようです。

ただ、デリゲートは `d()` のような呼び方の他に、`d.Invoke()` と言う呼び方もできるので、
こちらなら null 条件演算子 `?.` が使えます。

```csharp
using System.ComponentModel;

class Bindable : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(PropertyChangedEventArgs args)
        => PropertyChanged?.Invoke(this, args);
}
```

### <a id="sec-generated-title-9"></a> <a id="null-conditional-assignment">null じゃないときだけ代入</a>

<h5 class="version version14">Ver. 14</h5>

C# 14 では、[代入](../start/st_operator.md#substitute)
([複合代入](../start/st_operator.md#compound-assignment)も含む)の左側に `?.` を書くことで「null じゃないときだけ代入」ができるようになりました。
これを null 条件代入(null conditional assignment)といいます。

例えば以下のようなコードでは、
「`Weapon1` が null じゃないときだけ `Attack` の値を10に更新」と、
「`Weapon2` が null じゃないときだけ `Weapon1.Attack` の値を加える」という処理になります。

```csharp
static void UpdateWaepon(WeaponSlots slots)
{
    slots.Weapon1?.Attack = 10;
    slots.Weapon2?.Attack += slots.Weapon1?.Attack ?? 0;
}
```

`a?.X = value;` みたいに書くと、
ぱっと見では `(a?.X) = value;` みたいな意味合いにも見えて変な感じ(この解釈だと `null = value;` みたいな結果になってしまうので違う)なので賛否両論はある文法なんですが、
「null じゃないときだけ代入」自体は割と需要があったので C# 14 でついに実装されました。

(ちなみに、`a?.X = value;` の解釈は、模式的には `a? (.X = value);` みたいなくくり方の方が近いです(`?` の手前が null じゃないときだけ `()` の中身を実行という意味で)。)

いくつか例を挙げるために以下のようなクラスを考えてみます。

```csharp
class A
{
    public A? X { get; set; }
}
```

まずはシンプルな1段だけの例で、`a?.X = new();` を考えてみます。
以下のコードは、

```csharp
static void M(A? a)
{
    a?.X = new();
}
```

以下のコードとほぼ同じ意味になります。

```csharp
M(null);

static void M(A? a)
{
    if (a != null)
    {
        a.X = new();
    }
}
```

`?.` の段数を増やすと単純に `if` の段数が増えます。
例えば以下のコードは、

```csharp
static void M(A? a)
{
    a?.X?.X?.X = new();
}
```

以下のような意味になります。

```csharp
M(null);

static void M(A? a)
{
    if (a != null)
    {
        var a1 = a.X;
        if (a1 != null)
        {
            var a2 = a1.X;
            if (a2 != null)
            {
                a2.X = new();
            }
        }
    }
}
```

代入が複数並んでいる場合も考えてみます。
対比として先に通常の代入の例を書きますが、例えば以下のようなコードを書いた場合、

```csharp
static void M(A a, A b)
{
    a.X = b.X = new();
}
```

おおむね以下のようなコードと同じように、`=` の左側の代入(この例の場合、`b.X` の方への代入)が先に実行されます。

```csharp
static void M(A a, A b)
{
    var a1 = new A();
    b.X = a1;
    a.X = a1;
}
```

これに対して、null 条件代入の例として以下のようなコードを書いた場合、

```csharp
static void M(A? a, A? b)
{
    a?.X = b?.X = new();
}
```

おおむね以下のような意味になります
(「`a` を null チェック → `b` を null チェック → `b.X` への代入 → `a.X` への代入」みたいな順)。

```csharp
static void M(A? a, A? b)
{
    if (a != null) // a の null チェック
    {
        A? a1;

        if (b == null) // b の null チェック
        {
            a1 = null; 
        }
        else
        {
            a1 = new A();
            b.X = a1; // b への代入
        }

        a.X = a1; // a への代入
    }
}
```

ちなみに、null 条件代入は[インデクサー](../oop/oo_indexer.md)や[イベント](../functional/sp_event.md)に対しても使えます。

```csharp
static void M(A? a)
{
    // if (a != null) a[0] = 10; とほぼ同じ。
    a?[0] = 10;

    // if (a != null) a.Event += () => { }; とほぼ同じ。
    a?.Event += () => { };
}

class A
{
    public int this[int index]
    {
        get => 0;
        set { }
    }

    public event Action? Event;
}
```


## <a id="sec-generated-title-10"></a> <a id="null-coalesce">null合体演算子(null が来たら何か適当な有効な値で埋める)</a>

<h5 class="version version2">Ver. 2.0</h5>

次に、画像URLを渡して、その画像ロードする部分を考えましょう。
この段階で、「空欄(`ImagePath`としてもnullが渡ってくる)の時には空欄画像を読む」という処理を入れてみます。

例えば以下のように書けるでしょう。
(ここでは`LoadImage(string path)`という名前で画像を読み込むメソッドがあるものして説明します。)

```csharp
const string EmptyWeaponSlotImagePath = "EmptyWeaponSlot.png";

static Image LoadWeaponImage(string? imagePath)
{
    string path;
    if (imagePath == null) path = EmptyWeaponSlotImagePath;
    else path = imagePath;

    return LoadImage(path);
}

static Image LoadImage(string path)
{
    // 画像読み込み処理(省略、ここでは仮に new Image() を返す)
    return new Image();
}
```

前節と同様、この「null の時に所定の値に差し替える」と言う処理も頻出です。
こちらは C# 2.0で、<strong id="key-null-coalesce" class="keyword">null合体演算子</strong>(null coalescing operator)と言うものが導入されました。
以下のように、`??`で、左側に元の値、右側に差し替えたい値を書きます。

```csharp
static Image LoadWeaponImage(string? imagePath)
{
    return LoadImage(imagePath ?? EmptyWeaponSlotImagePath);
}
```

ちなみに、[別のページ](sp2_nullable.md#coalesce-translation)でも書いていますが、coalesce を「合体」と訳すのはちょっとわかりにくいかもしれません。
coalesce には「(折れた骨が)融合・癒着する」と言うような意味があって、
例えば欠けた素材をパテなどで穴埋めするようなときにも使うようです。
「null coalescing」 も null で欠けた部分を穴埋めすると言うようなニュアンスです。

### <a id="sec-generated-title-11"></a> <a id="short-circuit">補足: null条件演算子とnull合体演算子の短絡評価</a>

null条件演算子とnull合体演算子はいわゆる[短絡評価](../start/st_operator.md#shortcircuit)になっています。
null条件演算子の場合は左側がnullだったら、
null合体演算子の場合は左側がnullでなかったら、右側を評価する必要がなくなるので、全く評価しません。

例えば、プロパティやメソッドがどこまで呼ばれたのかを確認するためのログ表示を仕込んだ以下のようなクラスを用意します。

```csharp
static class Extension
{
    // null な変数に対しても a.M(i) で例外を起こさず呼べる拡張メソッド。
    public static void M(this A? s, int i)
    {
        Console.WriteLine("A.M(int)");
    }

    public static int M(this int i)
    {
        Console.WriteLine("int.M()");
        return i;
    }
}

class A
{
    public A? X
    {
        get
        {
            // プロパティが読まれたことを確認するためだけのログ表示。
            Console.WriteLine("X");
            return field;
        }
        set;
    }
}
```

これに対して3通りの呼び出し方をしてみましょう。
まず、非 null しかない場合、`?.` から先がすべて呼ばれます。

```csharp
// 変数も、その X も非 null の場合
var a1 = new A { X = new() };

// X も呼ばれ、M も呼ばれる。
// X, int.M(), A.M(int) の3行表示される。
a1?.X?.M(1.M());
```

```console
X
int.M()
A.M(int)
```

続いて、変数は非 null、その `X` は null の場合、
`X?.` の後ろが呼ばれなくなります。
この時、引数の評価(この例の場合、`1.M()` の部分)も消えます。

```csharp
// 変数は非 null、その X は null の場合
var a2 = new A { X = null };

// a1 は 非 null → X は呼ばれる
// その X は null → M は呼ばれない
// M を呼ばなくていいならその引数の 1.M() 自体呼ばれない
// X の1行だけ表示される。
a2?.X?.M(1.M());
```

```console
X
```

最後に、根本がすでに null の場合、すべて呼ばれなくなります。

```csharp
// 変数自体が null の場合
A? a3 = null;

// a3 が null の時点で X もその先も呼ばれない
// 何も表示されない。
a3?.X?.M(1.M());
```

```console

```



### <a id="sec-generated-title-12"></a> <a id="cache">余談: キャッシュ用途</a>

null を使う場面の例としてよく挙げられるものの1つに、キャッシュ用途もあります。
ここでいうキャッシュは、

- クラスのコンストラクターの時点では計算できない、もしくは、計算したくない
  - 「計算自体にそこそこコストが掛かるので、計算は1回限りにしたい」など
- 1度計算してしまえばその後値は変化しない
- 未計算の状態として null を使って、「null の時だけ計算」というような処理を書く

というものです。

例えば以下のように書いたりします。
[リフレクション](../dynamic/sp_reflection.md)を使った例ですが、リフレクションは重たいので取得した値はキャッシュしておきたいです。

```csharp
using System;
using System.ComponentModel;
using System.Reflection;

// System.Type から、自分のプログラムで使う属性とかを抽出するためのクラス
class TypeInfo
{
    private readonly Type  _type;
    public TypeInfo(Type type) => _type = type;

    // 必ずしも使わないものとする。使うときにだけ属性を読みたい。
    // リフレクションは重たいので、1回呼んだらキャッシュしておきたい。
    public string Description
    {
        get
        {
            if (_description == null)
            {
                var desc = _type.GetCustomAttribute<DescriptionAttribute>();
                _description = desc?.Description ?? "";
            }
            return _description;
        }
    }
    private string? _description;
}
```

こういう場合、以下のように、 `??` を使ってもっと短縮して書くこともできます。
1行だけにできるので、[`=>`](../cheatsheet/ap_ver7.md#throw-expression) を使えたりもします。

```csharp
public string Description => _description = _description ?? _type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
```

ただ、この例はちょっと1行に詰め込みすぎではあるので、`??`から後ろは別途メソッド化する方が読みやすくていいでしょう。

```csharp
public string Description => _description = _description ?? GetDescription();
private string GetDescription() => _type.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "";
```

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 で入った[`??=` 演算子](sp2_nullable.md#null-coalescing-assignment)は、こういうキャッシュ用途で使うのに特に便利です。
上記の例は以下のように書くことができます。

```csharp
public string Description => _description ??= GetDescription();
```

<h5 class="version version14">Ver. 14</h5>

ちなみに、この手のコードに対しては C# 14 で導入された [field キーワード](../oop/oo_property.md#field-keyword)が有効で、
C# 14 以降では以下のような書き方ができます。

```csharp
// (_description フィールドを用意する必要なし。)
public string Description => field ??= GetDescription();
```

## <a id="sec-generated-title-13"></a> <a id="null-branch">nullを読み飛ばす</a>

続いて、ロードした画像の表示を考えます。
今回の例では画像を表示する画面には2種類あって、「空欄は飛ばして詰めて表示したい画面」という仕様のものもあります。

単純に null が来たら飛ばすだけでいいので、要は、以下のような `if` を書きます。

```csharp
void ShowImage(Weapon? w)
{
    var imageUrl = w?.ImagePath;

    if (imageUrl != null)
    {
        Canvas.Draw(LoadImage(imageUrl));
    }
}
```

<h5 class="version version7">Ver. 7.0</h5>

また、C# 7.0で導入された[パターン マッチング](../datatype/typeswitch.md#null-check)は、この手の null 判定のためにも使えます。
例えば先ほどのコードは以下のように書くこともできます。

```csharp
void ShowImage(Weapon? w)
{
    if (w?.ImagePath is string imageUrl)
    {
        Canvas.Draw(LoadImage(imageUrl));
    }
}
```

ちなみに、`is var` ([`var`パターン](../datatype/patterns.md#var)と言って、[`is T`](../datatype/patterns.md#declaration) とは別扱い)を使った場合、nullチェックはされません。
`var` は何でも受け取れる構文で、null も受け付けます。

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、[再帰パターン](../datatype/patterns.md#recursive) の `{}` が暗黙的に null チェックも含んでいることを使って、手短に null チェックができます
(参考: [非 null マッチング](../datatype/patterns.md#non-null))。

```csharp
string? s = null;
 
if (s is var _) Console.WriteLine("ここは通る");
if (s is { }) Console.WriteLine("ここは通らない");
```

## <a id="sec-generated-title-14"></a> <a id="non-null">null を完全に認めない</a>

今回の例では、画面に2種類の仕様がありますが、

- 前節の「空欄は飛ばして詰める」というものでは、`if`ステートメントで null を読み飛ばしているので、`Draw`の行には絶対にnullが来ない
- 「空欄画像を表示する」という仕様でも、`LoadWeaponImage` の時点で有効な空欄画像を読んでいるはずなので、`Draw`の行には絶対にnullが来ない

と言うように、画像の表示メソッド`Canvas.Draw`に対しては絶対にnullが渡らないはずです。
ここにnullが来てしまうということは、何らかのバグがあるということです。
しっかりとテストをして、そういうことが起こらないようにデバッグすべきものです。

[最初に説明](#null)した通り、本来、「型を`T`とだけ書けばnullを絶対に受け付けない。nullを受け付けたければ`T?`と書く」とすべきです。
C# 8.0 でこの仕様が入る予定ですが、現時点(C# 7.3)では残念ながら、これができるのは[値型](oo_reference.md#valtype)だけです。

C# 7.3 以前の場合、せめて、引数に対してnull判定をして、nullだったら例外を出すということをよくやります。

```csharp
class Canvas
{
    public void Draw(Image image)
    {
        if (image == null)
        {
            throw new ArgumentNullException(nameof(image));
        }

        // 描画処理
    }
}
```

<h5 class="version version7">Ver. 7.0</h5>

ちなみに、C# 7.0では[`throw`式](../structured/oo_exception.md#throwexpr)といって、`??`の右側に`throw`を書けるようになったので、以下のような書き方でnull判定を行うこともできます。

```csharp
class Canvas
{
    public void Draw(Image image)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));

        // 描画処理
    }
}
```

## <a id="sec-generated-title-15"></a> <a id="user-defined-null">余談: 自称 null</a>

混乱の元なのでおすすめはしませんが、[演算子を自作](../oop/oo_operator.md)して、「null を自称できる型」を作ることができます。
例えば以下のようなものです。

```csharp
// null じゃないのに this == null が成り立ってしまうかなりタチが悪いクラス
class FalseNullable
{
    // 動作確認用
    public string? Name { get; }

    // 自身が null でなくても、中身が null だったら null を自称する
    public bool IsNull => Name == null;

    // 自称 null
    public static readonly FalseNullable Null = new FalseNullable();

    public FalseNullable() => Name = null;
    public FalseNullable(string name) => Name = name;

    // IsNull が true のとき、null とも一致
    public static bool operator ==(FalseNullable? x, FalseNullable? y)
        => ReferenceEquals(x, y)
        || (y is null && x.IsNull)
        || (x is null && y.IsNull);
    public static bool operator !=(FalseNullable? x, FalseNullable? y) => !(x == y);

    // 自称 null のときは "null" と表示
    public override string? ToString() => IsNull ? "null" : Name;
}
```

タチが悪いことに、この型には「真の null」(`null`)と「自称 null」(`FalseNullable.Null`)があります。
真のnullと自称nullで、`is`演算子や`??`演算子の挙動が変わります。
例えば、上記のクラスに対して以下のような処理を書いたとします。

```csharp
static void Write(FalseNullable? x)
{
    Console.WriteLine(x);

    // == 演算子呼び出し。自称 null でも true になる。
    Console.WriteLine(x == null);

    // これは == を呼ばない。真の null の時だけ true になる。
    Console.WriteLine(x is null);

    // == 呼ばない。自称 null の時には "null" と出る。
    Console.WriteLine(x ?? new FalseNullable("coalescing value"));

    // わざと null 参照。これも、例外になるのは真の null の時だけ
    try { Console.WriteLine(x.Name); }
    catch { Console.WriteLine("NullReferenceException"); }
}
```


`==` では、ユーザー定義の`==`演算子が呼ばれて、自称nullが`x == null`を満たします。
一方、`is`では、真のnullしか`x is null`になりません。
また、`??` で右辺の値が選ばれるのも真のnullの時だけです。
`x.Name`がnull参照例外になるのも真のnullの時だけになります。
例えば以下のような呼び出しをすると、

```csharp
Console.WriteLine("=== 真の null ===");
Write(null);

Console.WriteLine("=== 自称 null ===");
Write(FalseNullable.Null);

Console.WriteLine("=== 非 null ===");
Write(new FalseNullable("non-null")); 
```

以下のような結果になります。
```console
=== 真の null ===

True
True
coalscing value
NullReferenceException
=== 自称 null ===
null
True
False
null

=== 非 null ===
non-null
False
False
non-null
non-null
```

かなり気持ち悪い挙動を起こしますので、「null を自称できる型」を作るのはよっぽどのことがない限り辞めた方がいいでしょう。
