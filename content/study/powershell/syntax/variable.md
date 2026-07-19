---
title: "変数"
source_url: "https://ufcpp.net/study/powershell/syntax/variable/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2007-05-24T00:00:00"
tags: []
umbraco_id: 1579
parent_id: 1577
sort_order: 1
aliases:
  - "/powershell/syntax/variable/"
  - "/powershell/variable"
  - "/powershell/variable.html"
  - "/study/powershell/variable"
  - "/study/powershell/variable.html"
---

# 変数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

説明が長くなりそうなので分割。
ここでは、主に変数の取り扱いについて説明します。


## <a id="sec-generated-title-2"></a> <a id="variable"></a>変数

「[基礎知識](basic.md)」でも説明したように、
$ から始まる単語は変数になります。

```console
>  $a = 1
>  $a
1
```


「$ ＋ アルファベット」という書き方以外に、
「${任意の記号}」という書き方もできます。
${a} というように、中身がアルファベットなら $a と同じ意味になってあまり意味はないんですが、
${#$%&amp;'(} というような、任意の記号を含む変数名を付けることができます。


## <a id="sec-generated-title-3"></a> <a id="object"></a>Variant ではなく Object

変数にはどんな型の値でも代入できますが、
Variant ではなく Object です。

（Variant ってのは、要するに、
「型の定まってない型」、「どんな型にでもなれる型」です。
型があいまいになるのでプログラミングミスの原因。
あと、型判定とか型変換のためのオーバーヘッドも生じる。
Object の方は、
C 言語的にいうと (void *) みたいなもので、
どんな型の値も格納できるものの、
中身の型が変わることはありません。
）

Object の中身はちゃんと型を持っています。
1 を代入したなら System.Int32 ですし、
"test" を代入したなら System.String になります。
PowerShell 上のオブジェクトは全て .NET Framework のオブジェクトで、
GetType() などのメソッドやプロパティを使うことができます。

（
ただ、数値や文字列に対しては Variant 的な動作をしていて、
特殊な自動変換が働いて「どんな型にでもなれる型」になっています。
でも、特殊な型変換機構が働くのは、数値や文字列などの基本的な型のみ。
それ以外の型に関しては、暗黙的コンストラクタ呼び出しで型変換をしているみたい。
）

（
あと、C# でいうところの unboxing は自動的にやってくれるので、
object a = "test";
((string)a).ToUpper();
みたいなキャストは必要ありません。
）

```console
>  $a = 1
>  $a.GetType().Name
Int32
>  $a = "test"
>  $a.Length
4
>  $a.ToUpper()
TEST
>  $a.GetType().Name
String
```


オブジェクトが全部 .NET Framework のものなので、
.NET Framework SDK のヘルプを読むことで、
数値や文字列に対してどういう操作が可能なのかとかを調べることができます。


## <a id="sec-generated-title-4"></a> <a id="cast"></a>型変換

値や変数の前に [型名] を付けると、型変換ができます。
例えば、実数を整数に変換するには以下のようにします。

```console
>  $a = [int]1.2
>  $a
1
```


無理な変換をしようとするとエラーになります。

```console
>  $a = [int]"test"
値 "test" を型 "System.Int32" に変換できません。
```


でも、結構柔軟に型変換してくれます。
例えば、"128" というような文字列は、
C# なんかだと int.Parse メソッドを使って整数に変換する必要がありますが、
PowerShell では [int] で変換できます。

```console
>  $a = [int]"128"
>  $a
128
>  $a.GetType().Name
Int32
```



## <a id="sec-generated-title-5"></a> <a id="typespesific"></a>型の指定

変数の型は明示的に指定することもできます。
指定方法は、例えば以下のような感じで、
代入時に [型名] を変数の前に付けます。

```console
>  [int]$a = 1
```


型を指定すると、指定した型以外は代入できなくなります。

```console
>  [int]$a = 0
>  $a = "test" 
# ↓ エラー
値 "test" を型 "System.Int32" に変換できません。
>  $a = 1.1 
# ↓ 整数に型変換される
>  $a
1
```


一度型を指定すると、その変数はもうずっとその型の値しか代入できません。
別の型の値を代入したければ、
後述する 「[Remove-Variable](../cmdlet/cmd_variable.md#remove_variable)」 Cmdlet でいったん変数を削除する必要があります。


## <a id="sec-generated-title-6"></a> <a id="type"></a>型

先ほども説明したように、
PowerShell 上のオブジェクトは全て .NET Framework のオブジェクトになっています。
整数なら System.Int32 ですし、
小数なら System.Double、
文字列なら System.String です。

ただし、PowerShell では System 名前空間は省略可能です。
あと、大文字と小文字を区別しないので、
それぞれ、int32, double, string という名前で型を指定できます。

また、System.Int32 には int、
System.Int64 には long、
System.Boolean には bool という別名が付いています。
（どうもこの3つだけっぽい。
uint や short はない。
UInt32 や Int16 と書く必要あり。）

あと、特殊な型 void というものがあるようです。
（void は C 言語や C# などで、関数の戻り値がないことを示すキーワード。）
[void] を使うと、値を消してしまうことができるみたい。

```console
>  [void]1
>  [void]$a
>  [void]$a.GetType()
```


まあ、この例みたいな使い方にはあまり意味はありませんが、
関数（例えば、実行の成否を bool で返してくるような）の戻り値を無視したいときなどに使います。


## <a id="sec-generated-title-7"></a> <a id="scope"></a>スコープ

PowerShell の変数はスコープを持っています。
普通に宣言した変数はローカルスコープを持っていて、
関数やブロックの外部からは参照できません。

: をはさんで変数名の前にスコープ名を書くことで、
ローカル以外のスコープの変数を読み書きできます。
（例えば、グローバルスコープの変数 a には、
$global:a という書き方でアクセスする。）

スコープは以下の4種類あります。

<table summary="スコープの種類">
	<caption>
		スコープの種類
	</caption>
	<tr>
		<th>グローバル</th>
		<td markdown="1">global</td>
		<td markdown="1">どこからでも（スクリプトファイル外からでも）読み書き可能</td>
	</tr>
	<tr>
		<th>スクリプト</th>
		<td markdown="1">script</td>
		<td markdown="1">同一スクリプトファイル内なら、どこからでも読み書き可能</td>
	</tr>
	<tr>
		<th>ローカル</th>
		<td markdown="1">local</td>
		<td markdown="1">現在のブロック内か、子ブロック（ブロック中にさらにブロックを書く）から読み書き可能</td>
	</tr>
	<tr>
		<th>プライベート</th>
		<td markdown="1">private</td>
		<td markdown="1">現在のブロック内からのみ読み書き可能（子要素も除く）</td>
	</tr>
</table>


また、これらの名前付きスコープの他に、
別項で説明する 「[Set-Variable](../cmdlet/cmd_variable.md#set_variable)」, 「[Set-Variable](../cmdlet/cmd_variable.md#set_variable)」 Cmdlet を使うと、
「2レベル上の親ブロック中のスコープ」というように、
レベルを指定しての変数の読み書きも可能です。

あと、スコープとは違うんですが、
スコープと同じような「$env:変数名」という書式で環境変数を取得することもできます。
例えば、path 環境変数を取得したければ以下のように書きます。

```console
>  $env:path
C:\Windows\System32\WindowsPowerShell\v1.0\;C:\Wind....
```


↑どうも、env: はファイルシステムのドライブとかと同列の扱いらしい。
C ドライブを C: とか書くのと同じ。
で、「${C:\...\ファイル名}」みたいな記法で、ファイルの読み書きもできる模様。

```console
>  ${C:\Users\Public\test.txt} = "test"
>  ${C:\Users\Public\test.txt}
"test"
>  Get-Content C:\Users\Public\test.txt
"test"
```


ただし、
C:\ からフルパス書かないと駄目みたい。
（要するに、構文としては ${ドライブレター:パス} でないと駄目。）
まあ、あんまり便利な記法にも見えないし、
普通に Get-Content, Set-Content を使った方がいいかも。


## <a id="sec-generated-title-8"></a> <a id="operator"></a>演算子

整数の加減乗除・剰余に関しては、
C# と同じ
<code>+ - * / %</code>
という記号を使って演算が可能です。
また、文字列にも + や * 演算子が使えます。
（詳しくは次節で説明。）

これら5つの演算子に対しては、
対応する代入演算子
<code>+= -= *= /= %=</code>
も存在します。
（$a = $a + 1 と $a += 1 は同じ意味。）

ちなみに、代入演算子は複数並べて書くこともできます。

```console
>  $a = $b = $c = 1
>  $a,$b,$c
1
1
1
>  $a += $b += $c += 1
>  $a,$b,$c
4
3
2
```


一方、
&amp; や &gt; などの記号は特殊な意味を持っているので、
<code>+ - * / %</code> の5つ以外に関しては
「-eq」や「-lt」というように、
- から始まる文字列を使って演算子を表します。


### <a id="sec-generated-title-9"></a> <a id="typeoperator"></a>Object に対する演算子

整数や文字列、配列に対する演算子は、
それぞれの項で説明するとして、
ここでは任意の型に共通する演算だけ説明しておきます。

まず、変数が存在するかどうかを確認するために、
-eq 演算子を使って null 値との比較が可能です。
null というのは変数が空っぽの状態のことで、
PowerShell では、$null という名前の特殊な変数で表します。

```console
>  Remove-Variable a
>  $a
>  $a -eq $null
True
>  $a = 0
>  $a -eq $null
False
```


ちなみに、$() とか [void]0 でも null 値を作ることができたりします。

それから、-is と -isnot 演算子を使って、変数に格納されている値の型を確かめることができます。
（左辺に変数、右辺に [型名] を書きます。）

```console
>  $a = 1
>  $a -is [int]
True
>  $a -isnot [int]
False
>  $a -is [string]
False
>  $a -isnot [string]
True
```


また、-as で型変換もできます。
[型名] による型変換とちがって、
変換できない場合にはエラーを起こすのではなく null 値を返します。

```console
>  $a = "test" -as [int]
>  $a -eq $null
True
>  $a = 1.2 -as [int]
>  $a
1
```



### <a id="sec-generated-title-10"></a> <a id="priority"></a>演算子の優先順位

演算子には結合の優先度があります。
例えば、（まあ、多くのプログラミング言語がそうであるように、）
+ より * の方が優先度が上で、
1 + 2 * 3 + 1 と書くと 1 + (2 * 3) の意味になります。

まだ現時点で説明していない演算子もありますが、
とりあえずリファレンスに書いてある優先度一覧を示します。

<table summary="">

	<tr>
		<th>優先度</th>
		<th>演算子</th>
		<th>補足</th>
	</tr>
	<tr>
		<td markdown="1">高</td>
		<td markdown="1">( ) { }</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="17">　</td>
		<td markdown="1">@</td>
		<td markdown="1">配列生成</td>
	</tr>
	<tr>
		<td markdown="1">$</td>
		<td markdown="1">変数、式評価演算子</td>
	</tr>
	<tr>
		<td markdown="1">!</td>
		<td markdown="1">論理否定</td>
	</tr>
	<tr>
		<td markdown="1">[ ]</td>
		<td markdown="1">配列インデックス</td>
	</tr>
	<tr>
		<td markdown="1">.</td>
		<td markdown="1">メンバー参照の .</td>
	</tr>
	<tr>
		<td markdown="1">&amp; .</td>
		<td markdown="1">実行演算子、ソース演算子</td>
	</tr>
	<tr>
		<td markdown="1">++ --</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">単項 + -</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">,</td>
		<td markdown="1">配列化<sup>†</sup></td>
	</tr>
	<tr>
		<td markdown="1">..</td>
		<td markdown="1">配列化<sup>†</sup></td>
	</tr>
	<tr>
		<td markdown="1">* / %</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">2項 + -</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">比較演算子</td>
		<td markdown="1"><sup>††</sup></td>
	</tr>
	<tr>
		<td markdown="1">-band -bor -bxor</td>
		<td markdown="1"><sup>†</sup></td>
	</tr>
	<tr>
		<td markdown="1">-and -or</td>
		<td markdown="1">　</td>
	</tr>
	<tr>
		<td markdown="1">|</td>
		<td markdown="1">パイプライン</td>
	</tr>
	<tr>
		<td markdown="1">&gt; &gt;&gt;</td>
		<td markdown="1">リダイレクト</td>
	</tr>
	<tr>
		<td markdown="1">低</td>
		<td markdown="1">代入演算子</td>
		<td markdown="1">　</td>
	</tr>
</table>


<sup>†</sup>
リファレンスには書いてないけど、多分この位置

<sup>††</sup>
多分、-as -is -replace -contains -f は比較演算子のところに含まれてると思う。


## <a id="sec-generated-title-11"></a> <a id="shell_var"></a>シェル変数

PowerShell が既定で持っている変数（シェル変数）がいくつかあります。


### <a id="sec-generated-title-12"></a> <a id="auto"></a>自動変数

いくつか、PowerShell 自体が自動的に値を設定している<strong id="auto_var" class="keyword">自動変数</strong>があります。
（ユーザは変更できない。）

「[Get-Variable](../cmdlet/cmd_variable.md#get_variable)」 Cmdlet を引数なしで呼び出せば、
現在使われている変数一覧が取得できるので、
PowerShell を起動直後に Get-Variable すればどういう自動変数があるのかが分かります。

以下、いくつか代表的なものを挙げます。
（詳細は、Get-Help Cmdlet を使って「Get-Help about_automatic_variables」で見れます。）

<table summary="自動変数">
	<caption>
		自動変数
	</caption>
	<tr>
		<th>変数名</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">$$</td>
		<td markdown="1">前のコマンド ラインの最後のトークン。</td>
	</tr>
	<tr>
		<td markdown="1">$^</td>
		<td markdown="1">前のコマンド ラインの最初のトークン。</td>
	</tr>
	<tr>
		<td markdown="1">$?</td>
		<td markdown="1">最後のコマンドの論理値状態。</td>
	</tr>
	<tr>
		<td markdown="1">$_</td>
		<td markdown="1">現在のパイプライン オブジェクト。</td>
	</tr>
	<tr>
		<td markdown="1">$args</td>
		<td markdown="1">スクリプトまたは関数の引数。</td>
	</tr>
	<tr>
		<td markdown="1">$input</td>
		<td markdown="1">スクリプトにパイプで連結されているオブジェクトの列挙子。</td>
	</tr>
	<tr>
		<td markdown="1">$Matches</td>
		<td markdown="1">-match 演算子で検出された一致結果の連想配列。</td>
	</tr>
	<tr>
		<td markdown="1">$HOME</td>
		<td markdown="1">ユーザーのホーム ディレクトリ。</td>
	</tr>
	<tr>
		<td markdown="1">$Error</td>
		<td markdown="1">前のコマンドのエラーの配列。</td>
	</tr>
	<tr>
		<td markdown="1">$MyInvocation</td>
		<td markdown="1">スクリプトファイル自身に関する情報</td>
	</tr>
</table>



### <a id="sec-generated-title-13"></a> <a id="user"></a>ユーザ設定変数

履歴の最大保持数など、
ユーザが設定できるシェル変数（<strong id="user_var" class="keyword">ユーザ設定変数</strong>）もあります。
