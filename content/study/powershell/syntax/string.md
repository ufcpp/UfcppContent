---
title: "文字列"
source_url: "https://ufcpp.net/study/powershell/syntax/string/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2015-05-06T14:20:55"
tags: []
umbraco_id: 1582
parent_id: 1577
sort_order: 4
aliases:
  - "/powershell/string"
  - "/powershell/string.html"
  - "/powershell/syntax/string/"
  - "/study/powershell/string"
  - "/study/powershell/string.html"
---

# 文字列

## <a id="sec-generated-title-1"></a> <a id="string"></a>文字列

"" か '' でくくると文字列になります。

"" の場合、中に $a などの変数が出てくると中身を展開します。
'' の方は $ もそのまま文字として扱います。

```console
>  $a = 1
>  "test $a"
test 1
>  'test $a'
test $a
```


ちなみに、"" の中に " を、
'' の中に ' を書きたければ、
後述するエスケープ文字 ` （バッククオート）を使います。

```console
> "`"test`""
"test"
```



## <a id="sec-generated-title-2"></a> <a id="herestring"></a>here 文字列

もう1つ、複数行にわたる文字列を定義できる
<strong id="herestring" class="keyword">here string</strong> というものがあります。
@" と "@ ではさむか、
@' と '@ ではさみます。

@" "@ と @' '@ の違いは、"", '' と同様、変数を展開するかどうかです。
また、@" "@ と @' '@ の中には " や ' を書くこともできます。

```console
>  $a = @"
>  this is example of here string
>  $a
>  "test"
>  'test'
>  "@
>  
>  $a
this is example of here string
1
"test"
'test'
```


```console
>  $a = @'
>  this is example of here string
>  $a
>  "test"
>  'test'
>  '@
>  
>  $a
this is example of here string
$a
"test"
'test'
```



## <a id="sec-generated-title-3"></a> <a id="escape"></a>エスケープシーケンス

PowerShell では、
タブや改行などの特殊な文字を表すためのエスケープ（後ろに続く文字に特殊な意味を持たせるもの）に
`（バッククォート、逆アポストロフィ）を使います。

` は以下の3つの意味で使います。

1. 行末に書くと、その行と次の行を改行なしで繋いだのと同じ意味になる。

2. $, &amp; ` " ' などの特殊な意味を持つ記号の前に書くと、これらの記号をそのまま文字として扱う。

3. ` に続けて以下の表にある文字を書くと、特殊な意味を持つエスケープシーケンスになる。


<table summary="エスケープシーケンス">
	<caption>
		エスケープシーケンス
	</caption>
	<tr>
		<th>エスケープ文字</th>
		<th>意味</th>
	</tr>
	<tr>
		<td markdown="1">`0</td>
		<td markdown="1">(null)</td>
	</tr>
	<tr>
		<td markdown="1">`a</td>
		<td markdown="1">(alert)</td>
	</tr>
	<tr>
		<td markdown="1">`b</td>
		<td markdown="1">(backspace)</td>
	</tr>
	<tr>
		<td markdown="1">`f</td>
		<td markdown="1">(form feed)</td>
	</tr>
	<tr>
		<td markdown="1">`n</td>
		<td markdown="1">(new line)</td>
	</tr>
	<tr>
		<td markdown="1">`r</td>
		<td markdown="1">(carriage return)</td>
	</tr>
	<tr>
		<td markdown="1">`t</td>
		<td markdown="1">(tab)</td>
	</tr>
	<tr>
		<td markdown="1">`v</td>
		<td markdown="1">(vertical quote)</td>
	</tr>
</table>


C 言語とかみたいにエスケープに \ 記号を使わないのは、
多分、Windows のファイルシステムが \ をパスの区切りに使うからだと思います。


## <a id="sec-generated-title-4"></a> <a id="string_add"></a>結合・反復

文字列は、+ 演算子で結合、* 演算子で反復ができます。

```console
>  $a = "string 1" + "string 2"
>  $a.length
16
>  $a
string 1string 2
```


```console
>  $a = "string" * 3
>  $a.length
18
>  $a
stringstringstring
```



## <a id="sec-generated-title-5"></a> <a id="string_comp"></a>比較・置換

2つの文字列を比較する演算子として、
以下のようなものがあります。

<table summary="文字列の一致比較">
	<caption>
		文字列の一致比較
	</caption>
	<tr>
		<th colspan="2">演算子</th>
		<th rowspan="2">説明</th>
	</tr>
	<tr>
		<th>一致</th>
		<th>不一致</th>
	</tr>
	<tr>
		<td markdown="1">-eq</td>
		<td markdown="1">-ne</td>
		<td markdown="1">完全一致比較</td>
	</tr>
	<tr>
		<td markdown="1">-like</td>
		<td markdown="1">-notlike</td>
		<td markdown="1">ワイルドカード対応の比較</td>
	</tr>
	<tr>
		<td markdown="1">-match</td>
		<td markdown="1">-notmatck</td>
		<td markdown="1">正規表現対応の比較</td>
	</tr>
</table>


<table summary="文字列の辞書式順序比較">
	<caption>
		文字列の辞書式順序比較
	</caption>
	<tr>
		<th>演算子</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">-lt</td>
		<td markdown="1">＜</td>
	</tr>
	<tr>
		<td markdown="1">-gt</td>
		<td markdown="1">＞</td>
	</tr>
	<tr>
		<td markdown="1">-le</td>
		<td markdown="1">≦</td>
	</tr>
	<tr>
		<td markdown="1">-ge</td>
		<td markdown="1">≧</td>
	</tr>
</table>


<table summary="文字列の置換">
	<caption>
		文字列の置換
	</caption>
	<tr>
		<th>演算子</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">-replace</td>
		<td markdown="1">置換（正規表現対応）</td>
	</tr>
</table>


例えば、Get-Location で取得したファイル一覧の中から特定の名前のファイルを探したければ、
以下のようにします。

```console
>  ls C:\Windows | %{$_.Name} | ?{$_ -eq "web"}
Web
>  ls C:\Windows | %{$_.Name} | ?{$_ -like "we*"}
Web
>  ls C:\Windows | %{$_.Name} | ?{$_ -match "^we.*"}
Web
```


置換は、
「対象の文字列 -replace 置換したい部分, 置換後の文字列」
という書式で行います。
（3項演算に見えなくもないですが、
-replace の右オペランドが配列なだけ。）

```console
>  $a = "Windows の新しいコマンドライン シェルです。"
>  $a -replace "です。", "なのですよ。"
Windows の新しいコマンドライン シェルなのですよ。
```


正規表現の書き方は、
.NET Framework の System.Text.RegularExpressions.Regex クラスのものと同じなので、
詳しくは .NET Framework のヘルプを参照。

ちなみに、PowerShell では、
比較も置換もすべてアルファベットの大文字・小文字の違いを無視します（case insensitive）。
で、実は、無視する（case insensitive）・無視しない（case sensitive）を明示的に指定した演算子も用意されています。
-eq に対して、insensitive 版の -ieq と sensitive 版の -ceq というものがあります。
同様に、-lt なら -ilt -clt、
-like なら -ilike -clike というように、
頭に i を付ければ case insensitive、
c を付ければ case sensitive になります。

```console
>  "test" -eq "Test"
True
>  "test" -ieq "Test"
True
>  "test" -ceq "Test"
False
```


（文字列以外に対しても使えるみたい。
意味ないけど。）


## <a id="sec-generated-title-6"></a> <a id="format"></a>フォーマット演算子

あともう1つ、フォーマット演算子 -f というものもあります。
「書式文字列 -f オブジェクト1, オブジェクト2, ...」という書式で使います。
書式文字列の書き方は、
System.String.Format メソッドのものと同じです。
（詳しくは .NET Framework のヘルプを参照。）

```console
>  "{0:00}:{1:00}:{2:00}" -f 1,3,12
01:03:12
>  "|{0,8}|" -f 128
|     128|
```



## <a id="sec-generated-title-7"></a> <a id="expression"></a>式評価演算子

文字列に対する演算子ではないんですが、
式評価演算子 $ というものがあります。
まあ、要するに、変数名の頭についてる $ です。

$a とか $x というように、$ に続けてアルファベットを書くと変数になるんですが、
$(式) と書くと () の中身を式として計算します。

まあ、たいていの文脈では、$ を付けなくても () だけで中身を式扱いするので、
そんなに使う必要もありません。

```console
>  echo (1 + 1)
2
>  echo $(1 + 1)
2
```


まあ、使えそうな場面は2つ。
1つは、
$() というように () の中身を空っぽにすると null 値が得られる。
() だけだと「括弧の中身がない」って言って怒られますが、
$() なら OK。

```console
>  $() -eq $null
True
```


で、もう1つは、"" の中でも式を使えること。
"" でくくった文字列は、
変数を展開するわけですが、
変数同様、$() の中身も式の計算結果に展開されます。

```console
>  "1 + 1 = $(1 + 1)"
1 + 1 = 2
```
