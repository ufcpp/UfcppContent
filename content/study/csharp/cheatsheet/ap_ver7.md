---
title: "C# 7 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver7/"
content_type: "Article"
published_at: "2016-05-22T00:00:00"
updated_at: "2016-10-30T00:00:00"
tags:
  - "Ver. 7.0"
umbraco_id: 1899
parent_id: 1174
sort_order: 9
aliases: []
---

# C# 7 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver7"></a>C# 7

<div class="version version7">Ver. 7</div>

<table>
<tr>
<th>リリース時期</th>
<td>2017/3</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2017</li>
<li>Visual Basic 15</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>タプル型</li>
<li>パターン マッチング</li>
</ul>
</td>
</tr>
</table>

C# 6からは[C#コンパイラーがオープンソース化](https://github.com/dotnet/roslyn)されたわけですが、
C# 6の言語仕様自体はオープン化前から大筋が決まっていました。
C# 7は、仕様を決めるかなり早い段階からすべてがオープンとなる初めてのバージョンになります。

C# 7以降のC#の大きなテーマとしては以下のようなものがあります。

- データ中心の設計
- パフォーマンスや信頼性の向上

C# 7にはその最初の一歩となる機能がいろいろと追加されています。
また、この大きなテーマ以外にも、こまごまとして改善が何点かあります。

## <a id="sec-generated-title-2"></a> <a id="data-centric"></a>データ中心の設計

伝統的なオブジェクト指向的な発想は多くの場面で有用ですが、別の発想を持つ方が好ましい場面もあります。
オブジェクト指向では、具体的なデータは隠蔽し、メソッド越しにデータを操作します。
振る舞い中心(behavior-centric)な設計になります。

一方で、関数型言語では、純粋なデータ(C#でいうとpublicなフィールドだけの構造体とか)を最初に用意して、
そのデータを変化させる関数を作ったりします。
データ中心(data-centric)な設計です。

この2者は相補的なものです。
C#はまだまだ前者(振る舞い中心)に寄っているので、もっと後者(データ中心)になじむ構文が必要になります。

C# 7では、タプルや型スイッチなどの機能が入ります。
これらは、将来的に、レコード型やパターン マッチングという、C# 8以降で検討されている機能につながっていきます。

### <a id="sec-generated-title-3"></a> <a id="tuple"></a>タプル

詳しくは「[名前のない複合型](../structured/st_anonymoustype.md)」で説明しますが、
型には常によい名前が付くわけではなく、名無しにしておきたいことがあります。
そういう場合に使うもののうちの1つがC# 7で導入されたタプル(tuple)です。

タプルの最大の用途は[多値戻り値](../structured/st_anonymoustype.md#multiple-returns)です。
関数の戻り値は引数と対になるものなので、タプルの書き心地は引数に近くなるように設計されています。

例えば以下のように書けます。

```csharp {title="タプルの例" highlight-text="(int count, int sum)"}
using System;
using System.Collections.Generic;

class Program
{
    // タプルを使って2つの戻り値を返す
    static (int count, int sum) Tally(IEnumerable<int> items)
    {
        var count = 0;
        var sum = 0;
        foreach (var x in items)
        {
            sum += x;
            count++;
        }

        return (count, sum);
    }

    static void Main()
    {
        var data = new[] { 1, 2, 3, 4, 5 };
        var t = Tally(data);
        Console.WriteLine($"{t.sum}/{t.count}");
    }
}
```

詳しくは「[タプル](../datatype/tuples.md)」で説明します。

### <a id="sec-generated-title-4"></a> <a id="deconstruction"></a>分解

タプルは、メンバー名だけ見ればその型が何を意味するか分かるからこそ型に名前が付かないわけです。 このとき、その型を受け取る変数にも、よい名前が浮かばなくなるはずです。
実際、前節では`t`という特に意味のない名前の変数で値を受け取っています。

であれば、最初から、中身の`count`、 `sum`に分解して戻り値を受け取りたいです。
C# 7では、そのための分解構文(deconstruction)も追加されました。
前節のコードを、分解を使うように書き換えると以下のようになります。

```csharp {title="タプルを分解して受け取る" highlight-text="var (count, sum)"}
var data = new[] { 1, 2, 3, 4, 5 };
var (count, sum) = Tally(data);
Console.WriteLine($"{sum}/{count}");
```

ちなみに、分解はタプル専用の構文ではなく、以下のように、`Deconstruct`という名前のメソッド(拡張メソッドでも可)を持っている型なら何にでも使うことができます。

```csharp {title="任意の型を分解する例" highlight-text="Deconstruct"}
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var (key, value) = new KeyValuePair<string, int>("one", 1);
    }
}

static class Extensions
{
    public static void Deconstruct<T, U>(this KeyValuePair<T, U> pair, out T key, out U value)
    {
        key = pair.Key;
        value = pair.Value;
    }
}
```


詳しくは「[分解](../datatype/deconstruction.md)
」で説明します。

### <a id="sec-generated-title-5"></a> <a id="out-var"></a>出力変数宣言

タプルが入るまで、複数の戻り値を返すためには[出力引数](../resource/sp_ref.md#out)を使っていました。

タプルが複数の戻り値を返す全く新しい手法なのに対して、
C# 7では既存の出力引数にも利便性向上のための機能が追加されました。
それが、出力変数宣言(out variable declaration。あるいは、略して out-var)です。

以下のように、out引数を受け取ると同時に、式の中で変数宣言できるようになりました。

```csharp {title="out-var" highlight-text="out var x, out var y"}
using System;

struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public void GetCoordinate(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}

class Program
{
    static void Main()
    {
        var p = new Point { X = 1, Y = 2 };
        p.GetCoordinate(out var x, out var y);

        // 以下のような書き方をしたのと同じ
        // int x, y;
        // p.GetCoordinate(out x, out y);

        Console.WriteLine($"{x}, {y}");
    }
}
```

タプルが入っても、出力引数の方が使いやすい場面は残ります(参考: [出力引数との比較](../datatype/tuples.md#out-params))。
そういう意味では、出力変数宣言はタプルと相補的な関係にあります。

また、この構文は、将来的には前節で出てきた「[分解](../datatype/deconstruction.md)
」や、次節で話す「[型スイッチ](../datatype/typeswitch.md)」と統合されて、
パターン マッチングというより大きな機能に発展するものです。

詳しくは「[出力変数宣言](../resource/sp_ref.md#out-var)」で説明します。

### <a id="sec-generated-title-6"></a> <a id="type-switch"></a>型スイッチ

C# 7で、[`is`演算子](../oop/oo_polymorphism.md#downcast)や[`switch`ステートメント](../structured/st_branch.md#switch)の`case`が拡張されて、以下のような機能が入りました。

- `case`でも、`is`演算子と同じように、インスタンスの型を見ての分岐ができるようになった
- `x is T t`や、`case T t`というように、型を調べつつ、型が一致してたらキャスト結果を変数`t`で受け取れるようになった

この機能を型スイッチ(type switch)と呼びます。
以下のような書き方ができます。

```csharp {title="isとcaseの拡張の例" highlight-ranges="1:19-1:20,11:10-11:26,15:10-15:15"}
if (obj is string s)
{
    Console.WriteLine("string #" + s.Length);
}

switch (obj)
{
    case 7:
        Console.WriteLine("7の時だけここに来る");
        break;
    case int n when n > 0:
        Console.WriteLine("正の数の時にここに来る " + n);
        // ただし、上から順に判定するので、7 の時には来なくなる
        break;
    case int n:
        Console.WriteLine("整数の時にここに来る" + n);
        // 同上、0 以下の時にしか来ない
        break;
    default:
        Console.WriteLine("その他");
        break;
}
```

詳しくは「[型スイッチ](../datatype/typeswitch.md)」で説明します。

#### <a id="sec-generated-title-7"></a> <a id="var-expressions"></a>式の中での変数宣言

[is 演算子の拡張](../datatype/typeswitch.md#is)と[出力変数宣言](../resource/sp_ref.md#out-var)では、式の中で変数宣言ができます。
式は割かしどこにでも書けるものなので、これまで変数宣言できなかったような場所(それも、かなり無制限)で変数を宣言できるようになりました。

そこで問題になるのは、式の中で宣言した変数のスコープがどうなるかです。
そのスコープは「式を囲うブロック、埋め込みステートメント、for、foreach、using、 case内」ということになっています。

詳しくは「[is演算子の拡張と出力変数宣言で作った変数のスコープ](../start/st_scope.md#csharp7)」を参照してください。

### <a id="sec-generated-title-8"></a> <a id="discard"></a>値の破棄

型スイッチや分解では、変数を宣言しつつ何らかの値を受け取るわけですが、 特に受け取る必要のない余剰の値が生まれたりします。

例えば、分解では、複数の値のうち、1つだけを受け取りたい場合があったとします。
こういう場合に、`_`を使うことで、値を受け取らずに無視することができます。

```csharp {title="_ で値を破棄"}
static (int quotient, int remainder) DivRem(int dividend, int divisor)
    => (Math.DivRem(dividend, divisor, out var remainder), remainder);

static void Deconstruct()
{
    // 商と余りを計算するメソッドがあるけども、ここでは商しか要らない
    // _ を書いたところでは、値を受け取らずに無視する
    var (q, _) = DivRem(123, 11);

    // 逆に、余りしか要らない
    // また、本来「var x」とか変数宣言を書くべき場所にも _ だけを書ける
    (_, var r) = DivRem(123, 11);
}
```

同様の機能は、型スイッチや出力変数宣言でも使えます。

詳しくは「[値の破棄](../datatype/patterns.md#discards)」を参照してください。

## <a id="sec-generated-title-9"></a> <a id="performance"></a>パフォーマンス改善

C#にとって一番重要視しているのは生産性の高さで、書きやすさ、読みやすさなどが一番大事です。
しかし、パフォーマンスへの配慮も大事です。いくら書きやすくても、出来上がったものがあまりにも遅いと言語の魅力は半減するでしょう。
そして、近年では、C#の用途も増え、パフォーマンスが求められる場面が増えています。
参考: [次期C#とパフォーマンス向上](http://www.buildinsider.net/column/iwanaga-nobuyuki/006)

それに、生産性を損なわずとも、パフォーマンス向上に関してできることもまだまだあるはずです。
そのため、C# 7では、パフォーマンス向上を目的とした新機能がいくつか入っています。

いくつかは、かつてマイクロソフト内で動いていた、[Midori](https://ja.wikipedia.org/wiki/Midori_(%E3%82%AA%E3%83%9A%E3%83%AC%E3%83%BC%E3%83%86%E3%82%A3%E3%83%B3%E3%82%B0%E3%82%B7%E3%82%B9%E3%83%86%E3%83%A0))という研究プロジェクトの成果から来ているようです。
Midoriは、マネージ コードをベースとしたOSを作ろうというプロジェクトでした。
OSのような低レイヤーのソフトウェアを作る以上、パフォーマンスは非常に重要です。
プロジェクト自体は閉じられましたが、その成果は、.NETやC#に取り込まれようとしています。

C# 7で入るものの多くが、値型と参照渡しを活用したメモリ管理の効率化になります。

### <a id="sec-generated-title-10"></a> <a id="ref-returns"></a>参照戻り値と参照ローカル変数

戻り値とローカル変数でも参照渡しを使えるようになりました。 書き方はほぼ参照引数と同じです。 戻り値の型の前、値を渡す側、値を受ける側それぞれに`ref`修飾子を付けます。

```csharp {title="参照戻り値と参照ローカル変数の例"}
using System;

class Program
{
    static void Main()
    {
        var x = 10;
        var y = 20;

        // x, y のうち、大きい方の参照を返す。この例の場合 y を参照。
        ref var m = ref Max(ref x, ref y);

        // 参照の書き換えなので、その先の y が書き換わる。
        m = 0;

        Console.WriteLine($"{x}, {y}"); // 10, 0
    }

    static ref int Max(ref int x, ref int y)
    {
        if (x < y) return ref y;
        else return ref x;
    }
}
```

詳しくは「[参照戻り値と参照ローカル変数](../resource/sp_ref.md#ref-returns)」を参照してください。

この機能によって、大き目の値型を無駄なコピーなく取り扱えるようになって、パフォーマンスの向上が期待できます(参考: 「[値型の参照渡し](../resource/sp_ref.md#value-type)」)。

### <a id="sec-generated-title-11"></a> <a id="local-functions"></a>ローカル関数

関数の中に入れ子で関数を書ける機能が追加されました。
入れ子の関数は、定義した関数の中でだけ使えます。
この機能をローカル関数と呼びます。

```csharp {title="ローカル関数の例" highlight-text="int f(int n) =&gt; n &gt;= 1 ? n * f(n - 1) : 1;"}
using System;

class Program
{
    static void Main()
    {
        // Main 関数の中で、ローカル関数 f を定義
        int f(int n) => n >= 1 ? n * f(n - 1) : 1;

        Console.WriteLine(f(10));
    }
}
```

詳しくは、[ローカル関数と匿名関数](../functional/fun_localfunctions.md)で説明します。

この機能はちょっとした利便性向上の機能ではありますが、
同時に、状況によってはラムダ式よりもパフォーマンスの良いコードが生成されるよう、最適化が掛かっています
(参考: [ローカル関数かつクロージャの場合](../functional/sp2_anonymousmethod.md#closure-local-function))。

### <a id="sec-generated-title-12"></a> <a id="tasklike"></a>非同期メソッドの戻り値に任意の型を使えるように

C# 6までは、[非同期メソッド](../async/sp5_async.md#async)の戻り値は、`void`か、`Task`、`Task<TResult>`クラス(`System.Threading.Tasks`名前空間)のいずれかである必要がありました。

これに対して、C# 7では、特定の条件を満たすように作れば、任意の型を非同期メソッドの戻り値として使えるようになりました。
最も有効と思われる例は、`ValueTask<TResult>`構造体(`System.Threading.Tasks`名前空間)です。
以下のようなコードが書けるようになります。

```csharp {title="ValueTaskの例"}
using System;
using System.Threading.Tasks;

class Program
{
    static async ValueTask<int> XAsync(Random r)
    {
        if (r.NextDouble() < 0.99)
        {
            // 99% ここを通る。
            // この場合、await が1度もなく、非同期処理にならない。
            // 非同期処理じゃないのに Task<int> のインスタンスが作られるのはもったいない
            return 1;
        }

        // こちら側は本当に非同期処理なので、Task<int> が必要。
        await Task.Delay(100);
        return 0;
    }
}
```

この例のように、大部分が実際には非同期処理を行わないような場合、都度`Task`クラスを作ってしまうと無駄なメモリ確保が必要になります。
それを避けるために使うのが`ValueTask`構造体です。

詳しくは、「[Task-like](../async/sp5_async.md#task-like)」を参照してください。

## <a id="sec-generated-title-13"></a>その他、利便性向上

その他、こまごまとした利便性向上がいくつかあります。
中には、C#チーム的に「要望があるのはわかるが、メリットは小さいので後回し」という位置づけになっていたものが、
チーム外からのPull Requestによって実装されたものもあります。

### <a id="sec-generated-title-14"></a> <a id="literals"></a>数値リテラルの改善

2進数リテラルが書けるようになりました。
また、数値リテラルの途中に、`_` を挟んで桁区切りに使えるようになりました。

```csharp {title="数値リテラルがらみの新機能の例"}
using System;

class Program
{
    static void M()
    {
        byte bitMask = 0b1100_0000;
        Console.WriteLine(bitMask); // 192

        uint magicNumber = 0xDEAD_BEEF;
        Console.WriteLine(magicNumber); // 3735928559
        Console.WriteLine(magicNumber.ToString("X")); // DEADBEEF 
    }
}
```

詳しくは、「[2進数リテラル](../start/stnumber.md#binary)」や「[数字区切り文字](../start/stnumber.md#digit-separator)」を参照してください。

### <a id="sec-generated-title-15"></a> <a id="throw-expression"></a>throw 式

以下の3つの場面に限ってですが、式の中に`throw`を書けるようになりました。

- [ラムダ式](../functional/sp3_lambda.md#lambda)や[式形式メンバー](../structured/st_function.md#sec-expression-bodied)の中(`=>` の後ろ)
- [null合体演算子](../resource/sp2_nullable.md#nullableType)(`??`)の後ろ
- [条件演算子](../structured/oo_exception.md)の第2、第3引数(条件式以外の部分。 `:` の前後)

```csharp {title="throw 式"}
// 式形式メンバーの中( => の直後)
static void A() => throw new NotImplementedException();

static string B(object obj)
{
    // null 合体演算子(??)の後ろ
    var s = obj as string ?? throw new ArgumentException(nameof(obj));

    // 条件演算子(?:)の条件以外の部分
    return s.Length == 0 ? "empty" :
        s.Length < 5 ? "short" :
        throw new InvalidOperationException("too long");
}
```

詳しくは、「[throw式](../structured/oo_exception.md#throwexpr)」を参照してください。

### <a id="sec-generated-title-16"></a> <a id="throw-expression"></a>式形式のメンバーの拡充

C# 6で、メソッド、演算子、プロパティとインデクサー(get-only)に対して、式が1つだけの場合に`=>`を使った略記法が追加されました。

これが、C# 7では、コンストラクター、ファイナライザー、プロパティとインデクサー(get/set それぞれ)、イベント(add/removeそれぞれ)でも使えるようになりました。
例えば、コンストラクターとファイナライザーであれば以下のように書けます。

```csharp {title="コンストラクター、ファイナライザーを式形式で定義する例。"}
class Counter
{
    static int x;

    // コンストラクター
    Counter() => x++;

    // ファイナライザー
    ~Counter() => x--;
}
```

詳しくは、「[expression-bodiedメンバーの拡充](../structured/st_function.md#sec-expression-bodied)」を参照してください。
