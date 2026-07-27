---
title: "関数、フィルタ、スクリプト"
source_url: "https://ufcpp.net/study/powershell/syntax/function/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2007-05-24T00:00:00"
tags: []
umbraco_id: 1586
parent_id: 1577
sort_order: 8
aliases:
  - "/study/powershell/function.html"
---

# 関数、フィルタ、スクリプト

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

他の多くのプログラミング言語と同様に、
PowerShell にも関数やサブルーチンと呼ばれる類の機能があります。

PowerShell の関数はオブジェクトパイプラインを強く意識して作られています。


## <a id="sec-generated-title-2"></a> <a id="function"></a>関数

PowerShell では、
<strong id="keyfunction" class="keyword">function</strong> キーワードを使って、
「function 関数名 {処理内容}」という構文で<strong id="function" class="keyword">関数</strong>（function）を定義します。

呼び出しは、単に関数名を記述するだけです。

```console
>  function f { pwd }
>  f

Path
----
C:\Users\Public
```



### <a id="sec-generated-title-3"></a> <a id="args"></a>引数の受け取り

PowerShell の関数は引数を持てるんですが、
引数の受け取り方には3パターンあります。

1. $args 自動変数を使う。

2. 引数リストを書く。

3. param キーワードを使う。


まずは、$args という自動変数を使って引数を受け取る方法です。
「f param1 param2 param3」というように引数を与えると、
param1, param2, param3 が自動的に配列 $args に格納されます。

```console
>  function f { $args.Length }
>  f a b c
3
>  f 1 2 3 4
4
>  function f { $args[0] * $args[1] }
>  f 2 3
6
```


ちなみに、関数の呼び出しはコマンド扱いです（引数の取り方は「[コマンドモード](basic.md#commandmode)」になる）。

自動変数 $args を使う方法以外に、
明示的に引数を宣言する構文もあります。
関数名の後ろに、「function f($x, $y)」というように引数リストを書きます。

```console
>  function f($x, $y) { $x * $y }
>  f 2 3
6
```


もう1つ、param キーワードを使う方法があります。
関数名の後とかではなくて、
関数本体内部に param($x, $y) というような書き方をします。

```console
>  function f { param($x, $y); $x * $y }
>  f 2 3
6
```


関数では、普通に引数リストを書けばいい話なので、
あまり param を使う必要もありませんが、
後述する外部スクリプトやスクリプトブロックでは、
この param キーワードを使って引数を受け取ることになります。


### <a id="sec-generated-title-4"></a> <a id="argstype"></a>引数の型指定

引数リストによる方法と、param キーワードを使う方法では、
引数の型を明示することもできます。

```console
>  function f([int]$x, [int]$y) { $x * $y }
>  f 2 3
6
>  f 2.2 3.4
# ↓ int に型変換される
6
```


型を明示する場合としない場合とで、
型変換によって挙動が代わる場合もあるので注意してください。
（できるだけ型は明示すべき。）

```console
>  function f($x, $y) { $x * $y }
>  f "2" 3
222
>  function f([int]$x, [int]$y) { $x * $y }
>  f "2" 3
6
```



### <a id="sec-generated-title-5"></a> <a id="ref"></a>ref 引数

関数の引数は参照渡しもできます。
（参照渡しに関しては、「[引数の参照渡し](../../csharp/resource/sp_ref.md)」参照。）
参照渡しをするには、引数の前に [ref] をつけます。

```console
> function swap([ref]$a, [ref]$b) {
  $t = $a.Value; $a.Value = $b.Value; $b.Value = $t
}

> $x = 1
> $y = 2
> $x, $y
1
2
> swap ([ref]$x) ([ref]$y)
> $x, $y
2
1
```


ただし、上の例を見ての通り、
参照先の変数には Value を通してアクセスする必要があります。
（この辺り、C# の ref キーワードとはちょっと違う。）
[ref] を付けた変数は System.Management.Automation.PSReference 型になります。


### <a id="sec-generated-title-6"></a> <a id="call"></a>引数の渡し方

既に少し触れましたが、
関数の呼び出しはコマンド扱いです。
引数の取り方が「[コマンドモード](basic.md#commandmode)」になる以外にも、
名前付きパラメータで（オプション的な書式）で引数を渡すこともできたりします。

例えば、「function f($x, $y)」というように、変数名 $x, $y で引数を定義した関数の場合、
「f -x 2 -y 3」と言うような書式でも関数の呼び出しが可能です。
（オプション的な渡し方と、並べる順番による渡し方の併用も可能。）

```console
>  function f($x, $y) { $x * $y }
>  f -x 2 -y 3
6
>  f 2 -y 3
6
>  f -x 2 3
6
```



### <a id="sec-generated-title-7"></a> <a id="default"></a>引数のデフォルト値

引数には、
デフォルト値（呼び出し側で引数を与えずに呼んだときに設定される値）を設定できます。

デフォルト値の設定の書式は、
引数リスト（もしくは param）で変数を宣言する際に、値を代入しておくだけです。

```console
>  function f($x = 3, $y = 5) { $x * $y }
>  f
15
>  f 2
10
>  f -x 2
10
>  f -y 2
6
```



### <a id="sec-generated-title-8"></a> <a id="return"></a>return

関数中に return と書くと、そこで関数の処理を終了します。

```console
>  function f($x) { if($x -lt 5) {return}; $x }
>  f 3
# ↓ 値を出力する前に return してる
>  f 5
5
```



### <a id="sec-generated-title-9"></a> <a id="returnval"></a>戻り値

値を出力するコマンドを書くと、
それが関数の戻り値になります。
値を出力するコマンドを複数並べた場合、
出力が配列になります。

```console
>  function f {1; "test"; pwd}
>  f
1
test

Path
----
C:\Users\Public
```


return の後ろに戻り値を書くこともできますが、
まあ、どこに書いても戻り値が出力されちゃうんで、あんまり意味もないです。

```console
>  function f { 1; 2; return 3}
>  f
1
2
3
```


で、気をつけないといけないのは、
関数内で戻り値を持った別の関数を呼ぶ場合。
（成功したかどうかを示すフラグを返すだけとかで）
戻り値を受け取る必要がない場合でも、
ちゃんと変数で戻り値を受け取るか、
[void] を書いておかないと不要な値が出力されてしまいます。

```console
>  function parse($str)
{
  [int] $local:a = 0;
  [int]::TryParse($str, [ref]$local:a)
# ↓ TryParse が結果を返すことを忘れると
  $str
}

>  parse "128"
True
# ↓ 不要な出力が混ざる
128
>  parse "test"
False
0
```


```console
>  function parse($str)
{
  [int] $local:a = 0;
  [void][int]::TryParse($str, [ref]$local:a)
  $str
}

>  parse "128"
128
>  parse "test"
0
```



### <a id="sec-generated-title-10"></a> <a id="pipeline"></a>パイプライン引数

PowerShell の関数の他のスクリプト言語の関数と異なるのは、
オブジェクトパイプラインを強く意識している所です。

PowerShell の関数は、コマンド（Cmdlet や 外部スクリプト）とほとんど同じ機能を持っていて、
| を使ってパイプラインで他のコマンドと繋ぐこともできます。

パイプラインから受け取った値は、
$input という自動変数に格納されます。
$input は Enumerator なので、foreach 文などを使って要素を参照します。

```console
>  function times([int] $a)
{
  foreach($x in $input)
  {
    $a * $x
  }
}

>  1,2,3 | times 2
2
4
6
```



### <a id="sec-generated-title-11"></a> <a id="process"></a>begin, process, end

PowerShell の関数には、パイプライン処理専用の以下のような構文も用意されています。

```console
>  function 関数名
{
  begin   { 最初に1回呼ばれる }
  process { パイプラインで受け取ったオブジェクトごとに呼ばれる }
  end     { 最後に1回呼ばれる }
}
```


パイプラインで受け取ったオブジェクトは $_ という自動変数に格納されます。
要するに、例えば、「1,2,3 | f」と書いて、配列 1,2,3 を f に渡すと、
以下のような動作をします。

* begin ブロックが呼ばれる

* $_ = 1 として process ブロックが呼ばれる

* $_ = 2 として process ブロックが呼ばれる

* $_ = 3 として process ブロックが呼ばれる

* end ブロックが呼ばれる


例えば、以下のように使います。

```console
>  function f()
{
  begin   { $local:count = 0 }
  process { $_; $local:count += 1 }
  end     { "total {0} objects" -f $local:count }
}

>  1,2,3 | f
1
2
3
total 3 objects
```



## <a id="sec-generated-title-12"></a> <a id="filter"></a>フィルタ

関数とは別に、パイプラインから受け取ったオブジェクトに対する処理専用の<strong id="filter" class="keyword">フィルタ</strong>（filter）というものもあります。
まあ、要するに、process 文だけを持つ function だと思ってください。

書式は関数とほとんど一緒なんですが、function の変わりに <strong id="keyfilter" class="keyword">filter</strong> キーワードを使って、
「filter フィルタ名 { 処理内容 }」と書きます。

```console
>  filter square { $_ * $_}
>  1,2,3 | square
1
4
9
```


関数と同様に、引数を取ったりもできます。

```console
>  filter times($a) { $a * $_}
>  1,2,3 | times 2
2
4
6
```


複数の戻り値を返すこともできます。

```console
>  filter duplicate { $_; $_ }
>  1,2,3 | duplicate
1
1
2
2
3
3
```



## <a id="sec-generated-title-13"></a> <a id="scriptblock"></a>スクリプトブロック

名前付きの関数・フィルタに加えて、
匿名関数のような物も作れます。
作り方は以下のような感じで、{} で囲ったコードを書くだけ。

```console
>  $block = { $a * $a }
```


これを、<strong id="scriptblock" class="keyword">スクリプトブロック</strong>と呼びます。

スクリプトブロックは、関数と同じように呼び出し可能なんですが、
呼び出すためにはスクリプトブロックを代入した変数の前に &amp; を付ける必要があります。

```console
>  $block = { $a * $a }
>  $block
# ↓ & を付けないと、中身が表示される
$a * $a
>  $a = 2
>  & $block
4
>  $a = 3
>  & $block
9
```


&amp; 演算子の詳細については「[実行演算子](#execute)」で説明します。

$args や param を使って引数を受け取ることもできます。

```console
>  $block = { param($a); $a * $a }
>  & $block 3
9
```


&amp; を使ったスクリプトブロックも、
コマンドと同じ扱いになります。
（引数は「[コマンドモード](basic.md#commandmode)」で渡す。）

ちなみに、
スクリプトブロックは System.Management.Automation.ScriptBlock 型になります。
ScriptBlock 型は、Invoke メソッドを持っていて、
この Invoke を呼び出すことでもスクリプトブロックの中身を実行可能です。

```console
>  $block = { $args[0] * $args[0] }
>  $block.Invoke(2)
4
```


ただし、この記法だと param を使った引数は受け取れないので注意。

あと、混乱を招きそうなんですが、
&amp; で呼ぶ際には引数は「[コマンドモード](basic.md#commandmode)」で呼ぶのに対して、
Invoke の方は () を使って , 区切りで与えるので注意してください。

```console
>  $block = { $args[0] * $args[1] }
>  & $block 2 3
6
>  $block.Invoke(2, 3)
6
```



## <a id="sec-generated-title-14"></a> <a id="extern"></a>外部スクリプト

「[スクリプトファイル](basic.md#extern)」で説明したとおり、
拡張子 .ps1 のファイルにスクリプトを書いておいて、
そのスクリプトファイルを呼び出すということもできます。
スクリプトの中からさらに別のスクリプトファイルを呼び出すこともできます。

スクリプトは、
書き方も呼び出し方も関数の中身とほとんど同じです。

書く側では、
param で引数を取ることが出来ますし、
begin, process, end ブロックを書くこともできます。
$args や $input も使えますし、
process ブロック内では $_ も使えます。

呼び出す側では、
引数を宣言順で渡すことも、オプション的な書式で渡すこともできますし、
パイプラインを通した呼び出しもできます。

例えば、
square.ps1 という名前のファイルに以下の内容を書いておいたとします。

```powershell
param($factor)

process
{
  $factor * $_ * $_
}
```


すると、以下のようなスクリプト呼び出しが可能です。

```console
>  1,2,3 | .\test.ps1 -factor 2
2
8
18
```



## <a id="sec-generated-title-15"></a> <a id="execute"></a>実行演算子

「[スクリプトブロック](#scriptblock)」のところで、
スクリプトブロックは &amp; を使って呼び出すと書きましたが、
ここではもう少し詳しく &amp; について説明します。

&amp; には、実はいくつかの機能があります。
（といっても、いずれもコマンドの実行がらみ。）

1. &amp; 外部スクリプト … 関数、外部スクリプトを呼び出す。

2. &amp; スクリプトブロック … スクリプトブロックを実行する。

3. &amp; "文字列" … 文字列をコマンドとして解釈する。


まあ、1番目は、&amp; をつけなくても、
関数名・スクリプトファイル名だけでも呼び出せるのであまり必要はないですね。
ただ、後述する . 演算子との対比のためにこういう書き方もできるようになっているんだと思います。

2番目は「[スクリプトブロック](#scriptblock)」で説明したとおりです。

3番目は以下のような感じ。

```console
>  & "pwd"

Path
----
C:\Users\Public
```


ちなみに、これは、文字列内全体が1つのコマンドとみなされます。
例えば、&amp; "cd .." とか書くと、
スペースも含めて「cd ..」全体で1つのコマンドだと思われて、
「そんな名前のコマンドはない」と怒られます。
（「&amp; "cd" ..」なら正しく実行できる。）


## <a id="sec-generated-title-16"></a> <a id="source"></a>ソース演算子

実行演算子 &amp; と似たような演算子に、
ソース演算子 . があります。
&amp; と同じく、以下の3つのことができます。

1. . 外部スクリプト … 関数、外部スクリプトを呼び出す。

2. . スクリプトブロック … スクリプトブロックを実行する。

3. . "文字列" … 文字列をコマンドとして解釈する。


で、&amp; との違いなんですが、
実行されるスコープが違います。
&amp; の方は、1段階下のレベルのスコープになるんですが、
. では、呼び出し元と同じスコープで実行されます。
（Unix 系コマンドの source と同じ動作。）

例えば以下のコードを見てください。

```console
>  function f { $x = 0 }
>  $x = 1
>  f
>  $x
1
```


関数 f 内の $x と、呼び出し元で 1 を代入している $x は別物です。
（関数内はレベルが1段階下。）
なので、関数内で $x の値を 0 に上書きしたつもりでも、
呼び出し元の方の $x には影響はありません。

これを . で呼び出すとどうなるかと言うと、
以下の通り。

```console
>  function f { $x = 0 }
>  $x = 1
>  . f
>  $x
0
```


関数の中身が、呼び出し元と同じスコープで実行されます。
従って、呼び出し元の $x が書き換えられます。

なので、“呼び出し”というよりは“取り込み”ということで、
ソース（source）演算子と呼びます。

外部スクリプトファイル中で定数や関数を定義して、
それを取り込みたい場合
（要するに、C 言語の #include 的な使い方をしたい場合）
には &amp; ではなくて . を使います。

ちなみに、
.test とかいうように書くと、
PowerShell は ".test" という名前のファイルを探しにいっちゃうので、
. 演算子の後ろには1スペース空ける必要があります。


## <a id="sec-generated-title-17"></a> <a id="priority"></a>コマンド名の解決順位

PowerShell では、
Cmdlet も関数もスクリプトも全部同じ書式でコマンドとして呼び出せます。
また、Cmdlet と同じ名前の関数やエイリアスを作ったりすることもできます。

同名のエイリアスや関数がある場合には、以下の優先順位で実行されます。

エイリアス → 関数 → Cmdlet → ps1 スクリプト → exe/bat ファイル → 通常ファイル

（通常のファイルをコマンドとして入力すると、
ファイルに関連付けられたアプリケーションが立ち上がります。）
