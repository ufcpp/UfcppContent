---
title: ".NET Framework オブジェクト"
source_url: "https://ufcpp.net/study/powershell/syntax/dotnet/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2007-05-29T00:00:00"
tags: []
umbraco_id: 1580
parent_id: 1577
sort_order: 2
aliases:
  - "/study/powershell/dotnet.html"
---

# .NET Framework オブジェクト

## <a id="sec-generated-title-1"></a> <a id="new"></a>オブジェクトの作成

これまでに説明した通り、
1 は System.Int32 だし、 "test" は System.String で、
全部 .NET Framework のオブジェクトです。
これらは、PowerShell 中で 1 とか "test" と書くだけで作れますが、
その他にも、New-Object Cmdlet を使って .NET Framework の任意のオブジェクトを作ることができます。
（ちなみに、New-Object は COM オブジェクトも同様に作れる。）

例えば、以下のようにして、System.DateTime クラスのインスタンスを作成できます。
（PowerShell では System. は省略可能ということになってる。）

```console {title="New-Object"}
>  $a = New-Object DateTime 2007, 4, 1
>  $a

2007年4月1日 0:00:00
```



## <a id="sec-generated-title-2"></a> <a id="dot"></a>プロパティ、メソッドのアクセス

あと、まあ、説明するまでもなく今まで散々つかっちゃっていますが、
. を使ってオブジェクトのプロパティやメソッドにアクセスできます。

```console {title="プロパティの取得"}
>  $a = New-Object DateTime 2007, 4, 1
>  $a.DayOfWeek
Sunday
```


```console {title="メソッド呼び出し"}
>  $a = "test"
>  $a.ToUpper()
TEST
```


それから、
static メソッドも
[型名]::メソッド名
という構文で呼び出すことができます。
例えば、System.Math クラスにはさまざまな数学関連の関数・定数があるわけですが、
以下のようにして呼び出すことができます。

```console {title="static メソッド呼び出し"}
>  [Math]::Atan(1) * 4
3.14159265358979
>  [Math]::PI
3.14159265358979
```



## <a id="sec-generated-title-3"></a> <a id="ref"></a>ref, out

.NET Framework には引数の参照渡しのために、
引数に ref とか out とかを指定できるんですが
（参考： 「[引数の参照渡し](../../csharp/resource/sp_ref.md)」）、
PowerShell では [ref] [out] というように、[] を使って指定します。

例えば、System.Int.TryParse は以下のようにして呼び出します。

```console {title="[ref]"}
>  $a = 0
>  [int]::TryParse("128", [ref]$a)
True
>  $a
128
```



## <a id="sec-generated-title-4"></a> <a id="note_method"></a>メソッド呼び出しの注意点

1つ気をつけないといけない点があります。
コマンドや function、スクリプトブロック等では a b c と括弧なしのスペース区切りで引数を渡すのに対して、
メソッドコールは (a, b, c) と () 付き , 区切りで引数を渡します。
（関数やスクリプトブロックについては別項で説明。）

```console {title="メソッドの呼び出し"}
>  function Pow
{
  [Math]::Pow($args[0], $args[1])
}

>  Pow 2 3
# ↓ PowerShell の関数呼び出し
8
>  [Math]::Pow(2, 3)
# ↓ メソッド呼び出し
8
```


```console {title="スクリプトブロックの呼び出し"}
>  $a = {$args[0] * $args[1]}
>  &$a 2 5
# ↓ スクリプトブロック呼び出し
10
>  $b = @{}
>  $b.test = {$args[0] * $args[1]}
>  &$b.test 2 5
# ↓ メソッドっぽく見えるけど違う
10
```


たちが悪いことに、
PowerShell の関数に対して
「func(2, 3)」と書いてもエラーにはなりません。

```console {title="陥りがちなミス"}
>  function ToString
{
  foreach($x in $args){$x.ToString()}
}

>  ToString 2 3
# ↓ 2つの整数が引数
2
3
>  ToString(2, 3)
# ↓ 1つの配列が引数
System.Object[]
```


要するに、ToString(2, 3) は以下のコードと同じ意味になります。

```console {title="陥りがちなミス"}
>  $a = @(2, 3)
>  ToString $a
```


あと、
New-Object Cmdlet では、以下のように、一見 , 区切りで引数を与えているように見えますが、
これも実は単に、New-Object が第2引数に配列を取るというだけです。
（, は配列化演算子。）

```console {title="New-Object"}
>  $a = New-Object DateTime 2007, 4, 1
```



## <a id="sec-generated-title-5"></a> <a id="load_assemblly"></a>アセンブリのロード

System.Reflection.Assembly の Load とか LoadFile を使ってアセンブリをロード。


##### <a id="sec-generated-title-6"></a>自作 DLL のロード

```csharp
using System;

namespace Ufcpp
{
  public class Test
  {
    string s;

    public Test() : this("test"){}

    public Test(string s) { this.s = s; }

    public string this[object o]
    {
      get { return this.s + o.ToString(); }
    }
  }
}
```


```console
> csc /target:library a.cs
> [void][Reflection.Assembly]::LoadFile($($(pwd).path + '\a.dll'))
```



##### <a id="sec-generated-title-7"></a>標準アセンブリのロード

標準アセンブリで、
PowerShell がデフォルトでロードしていないもののロード。

例えば、System.Drawing.Bitmap クラスを使いたければ、
System.Drawing のロードが必要で、

```console
> [void][Reflection.Assembly]::LoadWithPartialName('System.Drawing')
> $bmp = [Drawing.Image]::FromFile($((pwd).Path + '\' + $filename))
```



## <a id="sec-generated-title-8"></a> <a id="cast"></a>型変換

どうも、[型名] を使った型変換は、コンストラクタを呼んでるっぽい。

（
数値とか文字列とかの型変換は明らかに特殊なことをしてるけど、
.NET Framework オブジェクトの型変換は、多分、コンストラクタ呼び出しに置きかえられてると思う。
）

前節で作った Ufcpp.Test クラスは、string を引数とするコンストラクタを持っていますが、
その結果、文字列からの型変換が可能↓。

```console
> $a = [Ufcpp.Test]'abc'
> $a['test']
abstest
```


その他、Collection クラス同士の相互型変換が出来たり。


## <a id="sec-generated-title-9"></a> <a id="com"></a>COM の利用

.NET Framework では、
過去の資産を活用するために、
.NET から COM オブジェクトを利用する機構が用意されていました。

PowerShell でも同様に、COM を利用することができます。
COM オブジェクトは、New-Object コマンドに -Com オプションを付けるだけで作ることができます。
例えば、Excel

```console
> $a = New-Object -comobject Excel.Application
> $b = $a.Workbooks.Add()
> $c = $b.Worksheets.Item(1)
> $cell = $c.Cells.Item(1, 1)
> $cell.Interior.Color = 0xff0000
> $b.SaveAs($((pwd).Path + '\' + $filename))
```



##### <a id="sec-generated-title-10"></a>サンプル

ビットマップを読み出して、
ビットマップのドットに応じてExcel のセルの背景色塗りつぶしでドット絵を描く PowerShell スクリプト。

減色処理とかはしてないんで、
元々 Excel の色数に合わせて減色した画像を入力しないとまともな絵は出ない。

```powershell {title="exceldot.ps1"}
param([string]$inName, [string]$outName)

[void][Reflection.Assembly]::LoadWithPartialName('System.Drawing')
$bmp = [Drawing.Image]::FromFile($((pwd).Path + '\' + $inName))

$a = New-Object -comobject Excel.Application
$b = $a.Workbooks.Add()
$c = $b.Worksheets.Item(1)

for ($y = 0; $y -lt $bmp.Height; ++$y)
{
  for ($x = 0; $x -lt $bmp.Width; ++$x)
  {
    $color = $bmp.GetPixel($x, $y)
    $cell = $c.Cells.Item($y + 1, $x + 1)

    $cell.Interior.Color = $color.ToArgb()
  }
}
$c.UsedRange.RowHeight = 5
$c.UsedRange.ColumnWidth = 5 / 8.33

$b.SaveAs($((pwd).Path + '\' + $outName))
```
