---
title: "既定値"
source_url: "https://ufcpp.net/study/csharp/resource/rm_default/"
content_type: "Article"
published_at: "2014-10-06T00:00:00"
updated_at: "2017-08-15T00:00:00"
tags: []
umbraco_id: 1289
parent_id: 1286
sort_order: 4
aliases:
  - "/study/csharp/rm_default.html"
---

# 既定値

## <a id="sec-generated-title-1"></a> <a id="abst">概要</a>

C# はメモリ領域の未初期化を認めていません。
明示的な初期化を行わない場合、状況に応じて、コンパイル エラーになるか、既定値が入るかのどちらかです。

## <a id="sec-generated-title-2"></a> <a id="uninitialized">補足: 未初期化領域</a>

C# で気にする場面はほとんどありませんが、プログラミング言語によっては、未初期化の状態のメモリにアクセスできてしまう場合があります。
(特に、いわゆる低レイヤーな言語ほどそういうことが可能です。C# でも、「[unsafe](../interop/sp_unsafe.md#unsafe)」 コード内では起こり得ます。)

C++ を例に挙げてみましょう。
C++では、`new[]` で確保したばかりで初期化していないメモリ領域がどうなっているかは未定義(コンパイラーの裁量任せ)になっています。
以下のようなコードを見てください。

```cpp
#include <stdio.h>

void main()
{
    int* x = new int[1];
    x[0] = 0xFFFFFFFF; // ちゃんと初期化
    printf("%08x\n", x[0]);

    int* px = x;
    delete x;
    printf("%08x\n", px[0]); // 削除済みの領域にアクセス

    int* y = new int[1];
    printf("%08x\n", y[0]); // 未初期化
}
```


この時、ちゃんと初期化してから使っている1つ目の printf 以外は、値がどうなっているか不定です。
例えば、Visual Studio 付属の C++ コンパイラー(以下、Visual C++/VC++)で実行した場合、
Debugビルド時とReleaseビルド時で挙動が違います。

<table summary="削除済み/未初期化領域のアクセス(Visual C++ の例)">
	<caption>
		削除済み/未初期化領域のアクセス(Visual C++ の例)
	</caption>
	<tr>
		<th rowspan="2">状態</th>
		<th rowspan="2">コード例</th>
		<th colspan="2">Debugビルド時</th>
		<th colspan="2">Releaseビルド時</th>
	</tr>
	<tr>
		<th>結果</th>
		<th>説明</th>
		<th>結果</th>
		<th>説明</th>
	</tr>
	<tr>
		<th>初期化済み</th>
		<td markdown="1">

```cpp {title="未初期化領域を読む"}
int* x = new int[1];
x[0] = 0xFFFFFFFF;
printf("%08x\n", x[0]);
```

</td>
		<td markdown="1">ffffffff</td>
		<td markdown="1">これは問題ないコード。常に同じ動作。</td>
		<td markdown="1">ffffffff</td>
		<td markdown="1">ビルド オプションで結果が変わったりもしない。</td>
	</tr>
	<tr>
		<th>削除済み</th>
		<td markdown="1">

```cpp
int* px = x;
delete x;
printf("%08x\n", px[0]);
```

</td>
		<td markdown="1">dddddddd</td>
		<td markdown="1">削除済み領域を検知するためのパターンが入っている。<br></br>VC++ の場合は dd (ビット パターン 11011101)。</td>
		<td markdown="1">ffffffff<sup>※</sup></td>
		<td markdown="1">delete 前の値がそのまま残っている。</td>
	</tr>
	<tr>
		<th>未初期化</th>
		<td markdown="1">

```cpp
    int* y = new int[1];
    printf("%08x\n", y[0]);
}
```

</td>
		<td markdown="1">cdcdcdcd</td>
		<td markdown="1">未初期化領域を検知するためのパターンが入っている。<br></br>VC++ の場合は cd (ビット パターン 11001101)。</td>
		<td markdown="1">00000000<sup>※</sup></td>
		<td markdown="1">この例の場合は0詰め。</td>
	</tr>
</table>


<sup>※</sup> 常にこうなるわけじゃない。状況次第。

まだこの実行結果は値がわかりやすい方ですが、
場合によってはもっとランダムに意味不明の数値が得られたりします。
しかも、実行するたびに毎回結果が変わったりします。

こういう不定な動作は、「テスト実行時にはうまく動いていた(ように見えた)のに、本番環境では動かない」というようなバグになることもあります。
これは発見しにくい類のバグで、メモリ領域の未初期化を認めている言語ではよく問題になったりします。
そのため、C# は未初期化を認めていません。


## <a id="sec-generated-title-3"></a> <a id="sec-default-value">既定値</a>

とうことで、C# では、未初期化なメモリ領域へのアクセスを認めていません。
明示的な変数の初期化を怠った場合、状況に応じて、以下のいずれかになります(コンパイル エラー、もしくは、0埋め = 本項の主題となる「既定値」で初期化される)。

* 初期化しないと<em>コンパイル エラー</em>になる
    * ローカル変数
    * 構造体のフィールド(C# 10 以前)
* <em>0 埋め</em> (これを「既定値」と呼びます。後述)
    * クラスのフィールド(C# 11 以降は構造体のフィールドも)
    * 配列の要素
    * `default(T)` という式(後述)で作った値

<strong id="default-value" class="keyword">既定値</strong>(default value)というのは、その名の通り、明示的な初期化を怠った時に既定で代入される値です。
基本的に、既定値は「0 埋め」です。
型に応じて、`0`、`false`、`null` のどれか(全部、メモリ上の値としては 0 で表現される値)です。
「[構造体](../structured/st_struct.md#struct)」の場合は、すべてのフィールドを既定値で埋めたものになります。

`null` なんかは [10億ドルの間違い (billion-dollar mistake)](https://www.infoq.com/presentations/Null-References-The-Billion-Dollar-Mistake-Tony-Hoare/)とまで言われて忌み嫌われていますが、少なくとも未定義動作よりははるかにマシです。(参考: 「[null 許容参照型](nullablereferencetype.md)」)

以下に、既定値の例を示します(この例ではクラスのフィールドを明示的に初期化せず使うことで既定値を得ています)。

```csharp {title="既定値の例"}
// 初期化せずにフィールドを読んでみる(既定値が入っている)
var a = new DefaultValues();

Console.WriteLine(a.i);         // 0
Console.WriteLine(a.x);         // 0.0
Console.WriteLine((int)a.c);    // '\0' (ヌル文字)は表示できないので数値化して表示
Console.WriteLine(a.b);         // False
Console.WriteLine(a.s == null); // null は表示できないので比較で。True になる

class DefaultValues
{
    public int i;
    public double x;
    public char c;
    public bool b;
    public string s;
}
```

0 埋めなのは、主にパフォーマンス上の理由です。
(未定義動作よりはマシなので)何か決まった値で初期化するとするなら 0 が一番低コストです。
配列などで大きめのメモリ領域を確保した際でも、0 埋めならそこまで大きなコストをかけずに初期化できます。

```csharp {title="巨大配列の 0 埋めの例"}
using System.Runtime.InteropServices;

// 16 MB の巨大領域。
// 要素1個1個は初期化していないので、全部に既定値が入ってる。
var points = new Vector4[1024 * 1024];

// 中身が全部 0 なことを確認してみる。
// (無理やり byte 配列扱いして、1 byte ずつ確認。)
var bytes = MemoryMarshal.AsBytes<Vector4>(points);

foreach (var v in bytes)
{
    if (v != 0)
        Console.WriteLine("絶対通らないはず");
}

struct Vector4
{
    public float X, Y, Z, W;
}
```

### <a id="sec-generated-title-4"></a> <a id="word-default">余談: default という英単語</a>

ちなみに、既定値は英語だと default value なわけですが。
この「デフォルト」という言葉、IT 業界内では割かし基本単語っぽく感じるものの、
他の業界の人に通じないことがたまにあったりします。
というか、「既定で」、「標準で」みたいな意味で「デフォルト」という言葉を多用するのは IT 業界の用法みたいです。

default の元々の意味は「債務不履行」とか「怠慢」です。
2012年頃に某国の財務破たんで有名になった金融用語の「デフォルト」と同じ単語です。
単語の成り立ち的には de + fault で、「失敗(fault)に陥る(de)」とかになります。

なので、default value ＝ やるべきことやってない(初期化しないとまずいだろっていう変数を初期化してない)時に強制的に代入される値 ＝ 既定値 という感じ。

## <a id="sec-generated-title-5"></a> <a id="default-keyword">default(T)</a>

C# 1.0 の頃には、既定値を作るための構文がありませんでした。
数値の場合は 0 とか 0.0 とか、bool の場合には false とか、クラスの場合には null とかいったように、個別に既定値相当の値を与える必要がありました。

また、構造体 `T` に対して、<code>new T()</code> で既定値(0 埋め)を作るという仕様がありました。
(実際、C# 10 で引数なしコンストラクターの仕様が入るまでは、構造体の `new T()` は常に既定値でした。)

C# 2.0 で「[ジェネリック](../oop/sp2_generics.md#generics)」が導入されたことで、
どんな型でも一律既定値を作れる構文が必要になりました。
以下のような場面で困りました。

```csharp
T X<T>()
{
    return ????; // T の既定値を作りたいけども、null とか 0 とかは書けない
}
```

<h5 class="version version2">Ver. 2.0</h5>

そこで、ジェネリックと同時に入った仕様が、`default` キーワードを使った既定値の作成機能です。

```csharp
T X<T>()
{
    return default(T); // 型に応じて、null とか 0 とかになる
}
```

### <a id="sec-generated-title-6"></a> <a id="default-constructor">default(T) と構造体のコンストラクター</a>

前節で少し触れましたが、`default(T)` 構文が入るまで、
構造体の既定値は `new T()` で作っていました。
この仕様のせいで、C# では、構造体に引数なしのコンストラクターを定義できませんでした。
(ちなみに、.NET 的にはそんな制限はありません。あくまで C# の文法上の制限。)

しかし、C# 2.0 以降、`default(T)` で既定値を作れる仕様が入ったので、実は、「C# の構造体には引数なしのコンストラクターが定義できない」って仕様は今となっては不要だったりします。
つまり、以下のよう使い分けれていいはずです。

```csharp
void X<T>()
    where T : new()
{
    var x = new T();    // この場合はコンストラクターが呼ばれて欲しい
    var y = default(T); // こいつは既定値(0 埋め)
}
```

<h5 class="version version10">Ver. 10.0</h5>

この現状を鑑みて、
C# 10 から構造体に引数なしのコンストラクターを定義できるようになりました。

```csharp {title="C# 10 で引数なしコンストラクターが書けるようになって、new T() と default(T) が別の意味に"}
// new T() は S(1, 2) に、
// default(T) は S(0, 0) になる。
WriteNewAndDefault<S>();

static void WriteNewAndDefault<T>()
    where T : new()
{
    var x = new T();    // この場合はコンストラクターが呼ばれるようになった。
    var y = default(T); // こいつは既定値(0 埋め)。

    Console.WriteLine(x);
    Console.WriteLine(y);
}

struct S
{
    public int X, Y;
    public S() => (X, Y) = (1, 2);
    public override string ToString() => $"S({X}, {Y})";
}
```

ちなみに、引数なしコンストラクターの仕様は C# 6 で一度検討されたんですが、その時にはいくつかバグを踏んでしまって撤回されました。
この時踏んだバグは以下のようなものです。

- `new T() == default(T)` という前提での最適化をしているコードが多すぎて、`new T()`で正しくコンストラクターを呼ばれない場面があった。
  - .NETランタイムの中でそういうコードがあって、C#よりも上のレイヤーでの回避ができない。
  - `Activator`クラス(`System`名前空間)の`CreateInstance`とかがそう。

## <a id="sec-generated-title-7"></a> <a id="default-expr">default 式</a>

<h5 class="version version7_1">Ver. 7.1</h5>

これまでの`default(T)`という構文では、型名が長い時にかなり煩雑なコードになっていました。
これに対して、C# 7.1では、左辺(代入先)から推論できる場合に、`(T)`を省略して`default`だけで既定値を作れるようになりました。 

例えば、既定値をよく使う割に型名が長くてうっとおしいものの代表格に、`CancellationToken`構造体(`System.Threading`名前空間)があります。
以下のような感じのコードを書くことが結構あったりします。

```csharp {title="CancellationTokenの規定値をdefault(T)で作る例"}
static async Task DefaultExpression(CancellationToken c = default(CancellationToken))
{
    while (c != default(CancellationToken) && !c.IsCancellationRequested)
    {
        await Task.Delay(1000);
        Console.WriteLine(".");
    }
}
```

これに対して、C# 7.1では、以下のように書き直せます。

```csharp {highlight-ranges="1:59-1:66,3:17-3:24"}
static async Task DefaultExpression(CancellationToken c = default)
{
    while (c != default && !c.IsCancellationRequested)
    {
        await Task.Delay(1000);
        Console.WriteLine(".");
    }
}
```

1行目の引数の既定値と、3行目の `!=`演算子の右側に`default`とだけ書かれています。
いずれも、引数`c`の型から`CancellationToken`構造体であることが推論できるので、`(CancellationToken)`の部分を省略できます。

この書き方をdefault式(default expression)、あるいは、defaultリテラル(default literal)と呼びます。

## <a id="sec-generated-title-8"></a> <a id="constant">既定値は定数</a>

既定値 `default(T)` は常に定数扱いされます。

C# には定数(`readonly` の意味じゃなく、`const`)しか受け付けない文脈がいくつかあります。
要は、コンパイル時に確定してないといけない部分なんですが、例えば以下のようなものがあります。

* 属性に渡す値

* 引数の既定値

定数を求められるので、
`int` とか `string` なら任意のリテラル(1, 2, 3, ... "abc" 何でも)を渡せますが、
クラスと構造体は既定値(`null`、`default(T)`)しか渡せません。

## <a id="sec-generated-title-9"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>

<h5 class="version version11">Ver. 11.0</h5>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
クラスと同じく、明示的に代入しなかったフィールド・自動プロパティには既定値が入ります。

```csharp {title="構造体のフィールドが自動的に 0 初期化されるように"}
struct Sample
{
    public int X { get; } = 1;
    public int Y { get; }
    public string? Z { get; }

    // X には初期化子が付いてるので元々 OK。
    // C# 11 では Y, Z に何も入れなくても自動的に 0/null 初期化されるように。
    public Sample() { }

    // C# 11 では Y に何も入れなくても大丈夫。0 に。
    public Sample(string z) => Z = z;
}
```

今となっては「構造体の場合はフィールドの明示的な初期化が必須」という制限は、「出どころを誰も覚えていない」というレベルだったそうです。
おそらくは、「構造体のフィールドはローカル変数的に扱う」みたいな空気感だと思われます。

制限が残っていても役に立つわけでもなく、
不便なだけだったので今更ながら「クラスと同様」に変更することになりました。
