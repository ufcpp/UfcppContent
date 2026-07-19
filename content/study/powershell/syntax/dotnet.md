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
  - "/powershell/dotnet"
  - "/powershell/dotnet.html"
  - "/powershell/syntax/dotnet/"
  - "/study/powershell/dotnet"
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

<pre class="console" title="New-Object">
<span class="prompt">&gt; </span> $a = New-Object DateTime 2007, 4, 1
<span class="prompt">&gt; </span> $a

2007年4月1日 0:00:00
</pre>



## <a id="sec-generated-title-2"></a> <a id="dot"></a>プロパティ、メソッドのアクセス

あと、まあ、説明するまでもなく今まで散々つかっちゃっていますが、
. を使ってオブジェクトのプロパティやメソッドにアクセスできます。

<pre class="console" title="プロパティの取得">
<span class="prompt">&gt; </span> $a = New-Object DateTime 2007, 4, 1
<span class="prompt">&gt; </span> $a.DayOfWeek
Sunday
</pre>


<pre class="console" title="メソッド呼び出し">
<span class="prompt">&gt; </span> $a = "test"
<span class="prompt">&gt; </span> $a.ToUpper()
TEST
</pre>


それから、
static メソッドも
[型名]::メソッド名
という構文で呼び出すことができます。
例えば、System.Math クラスにはさまざまな数学関連の関数・定数があるわけですが、
以下のようにして呼び出すことができます。

<pre class="console" title="static メソッド呼び出し">
<span class="prompt">&gt; </span> [Math]::Atan(1) * 4
3.14159265358979
<span class="prompt">&gt; </span> [Math]::PI
3.14159265358979
</pre>



## <a id="sec-generated-title-3"></a> <a id="ref"></a>ref, out

.NET Framework には引数の参照渡しのために、
引数に ref とか out とかを指定できるんですが
（参考： 「[引数の参照渡し](../../csharp/resource/sp_ref.md)」）、
PowerShell では [ref] [out] というように、[] を使って指定します。

例えば、System.Int.TryParse は以下のようにして呼び出します。

<pre class="console" title="[ref]">
<span class="prompt">&gt; </span> $a = 0
<span class="prompt">&gt; </span> [int]::TryParse("128", [ref]$a)
True
<span class="prompt">&gt; </span> $a
128
</pre>



## <a id="sec-generated-title-4"></a> <a id="note_method"></a>メソッド呼び出しの注意点

1つ気をつけないといけない点があります。
コマンドや function、スクリプトブロック等では a b c と括弧なしのスペース区切りで引数を渡すのに対して、
メソッドコールは (a, b, c) と () 付き , 区切りで引数を渡します。
（関数やスクリプトブロックについては別項で説明。）

<pre class="console" title="メソッドの呼び出し">
<span class="prompt">&gt; </span> function Pow
{
  [Math]::Pow($args[0], $args[1])
}

<span class="prompt">&gt; </span> Pow 2 3
<span class="comment"># ↓ PowerShell の関数呼び出し</span>
8
<span class="prompt">&gt; </span> [Math]::Pow(2, 3)
<span class="comment"># ↓ メソッド呼び出し</span>
8
</pre>


<pre class="console" title="スクリプトブロックの呼び出し">
<span class="prompt">&gt; </span> $a = {$args[0] * $args[1]}
<span class="prompt">&gt; </span> &amp;$a 2 5
<span class="comment"># ↓ スクリプトブロック呼び出し</span>
10
<span class="prompt">&gt; </span> $b = @{}
<span class="prompt">&gt; </span> $b.test = {$args[0] * $args[1]}
<span class="prompt">&gt; </span> &amp;$b.test 2 5
<span class="comment"># ↓ メソッドっぽく見えるけど違う</span>
10
</pre>


たちが悪いことに、
PowerShell の関数に対して
「func(2, 3)」と書いてもエラーにはなりません。

<pre class="console" title="陥りがちなミス">
<span class="prompt">&gt; </span> function ToString
{
  foreach($x in $args){$x.ToString()}
}

<span class="prompt">&gt; </span> ToString 2 3
<span class="comment"># ↓ 2つの整数が引数</span>
2
3
<span class="prompt">&gt; </span> ToString(2, 3)
<span class="comment"># ↓ 1つの配列が引数</span>
System.Object[]
</pre>


要するに、ToString(2, 3) は以下のコードと同じ意味になります。

<pre class="console" title="陥りがちなミス">
<span class="prompt">&gt; </span> $a = @(2, 3)
<span class="prompt">&gt; </span> ToString $a
</pre>


あと、
New-Object Cmdlet では、以下のように、一見 , 区切りで引数を与えているように見えますが、
これも実は単に、New-Object が第2引数に配列を取るというだけです。
（, は配列化演算子。）

<pre class="console" title="New-Object">
<span class="prompt">&gt; </span> $a = New-Object DateTime 2007, 4, 1
</pre>



## <a id="sec-generated-title-5"></a> <a id="load_assemblly"></a>アセンブリのロード

System.Reflection.Assembly の Load とか LoadFile を使ってアセンブリをロード。


##### <a id="sec-generated-title-6"></a>自作 DLL のロード

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">namespace</span> Ufcpp
{
  <span class="reserved">public class</span> Test
  {
    <span class="reserved">string</span> s;

    <span class="reserved">public</span> Test() : <span class="reserved">this</span>(<span class="literal">"test"</span>){}

    <span class="reserved">public</span> Test(<span class="reserved">string</span> s) { <span class="reserved">this</span>.s = s; }

    <span class="reserved">public string this</span>[<span class="reserved">object</span> o]
    {
      <span class="reserved">get</span> { <span class="reserved">return this</span>.s + o.ToString(); }
    }
  }
}
</code></pre>


<pre class="console" title="">
<span class="prompt">&gt; </span>csc /target:library a.cs
<span class="prompt">&gt; </span>[void][Reflection.Assembly]::LoadFile($($(pwd).path + '\a.dll'))
</pre>



##### <a id="sec-generated-title-7"></a>標準アセンブリのロード

標準アセンブリで、
PowerShell がデフォルトでロードしていないもののロード。

例えば、System.Drawing.Bitmap クラスを使いたければ、
System.Drawing のロードが必要で、

<pre class="console" title="">
<span class="prompt">&gt; </span>[void][Reflection.Assembly]::LoadWithPartialName('System.Drawing')
<span class="prompt">&gt; </span>$bmp = [Drawing.Image]::FromFile($((pwd).Path + '\' + $filename))
</pre>



## <a id="sec-generated-title-8"></a> <a id="cast"></a>型変換

どうも、[型名] を使った型変換は、コンストラクタを呼んでるっぽい。

（
数値とか文字列とかの型変換は明らかに特殊なことをしてるけど、
.NET Framework オブジェクトの型変換は、多分、コンストラクタ呼び出しに置きかえられてると思う。
）

前節で作った Ufcpp.Test クラスは、string を引数とするコンストラクタを持っていますが、
その結果、文字列からの型変換が可能↓。

<pre class="console" title="">
<span class="prompt">&gt; </span>$a = [Ufcpp.Test]'abc'
<span class="prompt">&gt; </span>$a['test']
abstest
</pre>


その他、Collection クラス同士の相互型変換が出来たり。


## <a id="sec-generated-title-9"></a> <a id="com"></a>COM の利用

.NET Framework では、
過去の資産を活用するために、
.NET から COM オブジェクトを利用する機構が用意されていました。

PowerShell でも同様に、COM を利用することができます。
COM オブジェクトは、New-Object コマンドに -Com オプションを付けるだけで作ることができます。
例えば、Excel

<pre class="console" title="">
<span class="prompt">&gt; </span>$a = New-Object -comobject Excel.Application
<span class="prompt">&gt; </span>$b = $a.Workbooks.Add()
<span class="prompt">&gt; </span>$c = $b.Worksheets.Item(1)
<span class="prompt">&gt; </span>$cell = $c.Cells.Item(1, 1)
<span class="prompt">&gt; </span>$cell.Interior.Color = 0xff0000
<span class="prompt">&gt; </span>$b.SaveAs($((pwd).Path + '\' + $filename))
</pre>



##### <a id="sec-generated-title-10"></a>サンプル

ビットマップを読み出して、
ビットマップのドットに応じてExcel のセルの背景色塗りつぶしでドット絵を描く PowerShell スクリプト。

減色処理とかはしてないんで、
元々 Excel の色数に合わせて減色した画像を入力しないとまともな絵は出ない。

<pre class="source" title="exceldot.ps1" lang="">
<code>param([string]$inName, [string]$outName)

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
</code></pre>
