---
title: "正規表現（文字列パターン マッチング）"
source_url: "https://ufcpp.net/study/dotnet/bcl/bcl_regex/"
content_type: "Article"
published_at: "2012-01-23T00:00:00"
updated_at: "2017-05-13T15:23:21"
tags: []
umbraco_id: 1387
parent_id: 1385
sort_order: 1
aliases:
  - "/study/dotnet/bcl_regex.html"
---

# 正規表現（文字列パターン マッチング）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="regex" class="keyword">正規表現</strong>（regular expression）は、文字列のパターン マッチングに使う簡易言語です。
.NET の場合、Regex クラス（System.Text.RegularExpressions 名前空間）を使うことで、正規表現によるパターン マッチングができます。

Regex クラスが受け付ける正規表現の書き方は、Perl での書き方に準じます。
ネットで「正規表現」で検索すると、Perl や Java のものが多く見つかりますが、同じ書き方ができます。

参考:

* [MSDN: Regex クラス](http://msdn.microsoft.com/ja-jp/library/system.text.regularexpressions.regex.aspx)



## <a id="sec-generated-title-2"></a> <a id="regex-class"></a>Regex クラス

概要の通り、Regex クラスを使って文字列パターン マッチングを行います。

例えば、以下のように書くことで、ハイフンで区切られた単語を抜き出すことができます。
Match メソッド（最初の1件を得る）や、Matches メソッド（マッチした個所すべてを得る）を使います。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
</ul>
<div>

```csharp
var text = @"
C# (pronounced C sharp) is a programming language that
is designed for building a variety of applications that
run on the .NET Framework. C# is simple, powerful,
type-safe, and object-oriented. The many innovations
in C# enable rapid application development while
retaining the expressiveness and elegance of C-style
languages. ";

var withHyphen = new Regex(@"\w+-\w+", RegexOptions.Multiline);

var hyphenedWord =
    from Match m in withHyphen.Matches(text)
    select m.Value;

foreach (var item in hyphonedWords)
{
    Console.WriteLine(item);
}
```


</div>
<div>

```vbnet
Dim text = "C# (pronounced C sharp) is a programming language that" & vbCrLf &
    "is designed for building a variety of applications that" & vbCrLf &
    "run on the .NET Framework. C# is simple, powerful," & vbCrLf &
    "type-safe, and object-oriented. The many innovations" & vbCrLf &
    "in C# enable rapid application development while" & vbCrLf &
    "retaining the expressiveness and elegance of C-style" & vbCrLf &
    "languages. "

Dim withHyphen = New Regex("\w+-\w+", RegexOptions.Multiline)

Dim hyphenedWord = From m As Match In withHyphen.Matches(text).OfType(Of Match)()
                   Select m.Value

For Each item In hyphenedWord
    Console.WriteLine(item)
Next
```


</div>
<div>

```fsharp
open System
open System.Text.RegularExpressions

let text = "
C# (pronounced C sharp) is a programming language that
is designed for building a variety of applications that
run on the .NET Framework. C# is simple, powerful,
type-safe, and object-oriented. The many innovations
in C# enable rapid application development while
retaining the expressiveness and elegance of C-style
languages. "

let withHyphen = new Regex(@"\w+-\w+", RegexOptions.Multiline)

let hyphenedWord = seq {
    for m in withHyphen.Matches(text) do
        yield m.Value
        }

let e = seq {
    for x in 0..10 do
        for y in 0..10 do
            System.Threading.Thread.Sleep(100)
            yield x * y
            }

for x in hyphenedWord do Console.WriteLine x
```


</div>
</div>


```console
type-safe
object-oriented
C-style
```


もう1つ、単語の出現頻度を数える例を示しましょう。
単語の区切りを表すのに正規表現を使います。
Split メソッドで、単語の切り出しを行います。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
</ul>
<div>

```csharp
var text = @"
C# (pronounced C sharp) is a programming language that
is designed for building a variety of applications that
run on the .NET Framework. C# is simple, powerful,
type-safe, and object-oriented. The many innovations
in C# enable rapid application development while
retaining the expressiveness and elegance of C-style
languages. ";

var splitter = new Regex(@"[\s\(\)\.,\n\r]+", RegexOptions.Multiline);

var wordCount =
    from word in splitter.Split(text)
    where !string.IsNullOrEmpty(word)
    group word by word into g
    orderby g.Count()
    select new { Count = g.Count(), Word = g.Key };

foreach (var item in wordCount)
{
    Console.WriteLine(item);
}
```


</div>
<div>

```vbnet
Dim text = "C# (pronounced C sharp) is a programming language that" & vbCrLf &
    "is designed for building a variety of applications that" & vbCrLf &
    "run on the .NET Framework. C# is simple, powerful," & vbCrLf &
    "type-safe, and object-oriented. The many innovations" & vbCrLf &
    "in C# enable rapid application development while" & vbCrLf &
    "retaining the expressiveness and elegance of C-style" & vbCrLf &
    "languages. "

Dim splitter = New Regex("[\s\(\)\.,\n\r]+", RegexOptions.Multiline)

Dim wordCount = From word In splitter.Split(text)
                Where Not String.IsNullOrEmpty(word)
                Group By Word = word Into Group
                Order By Group.Count()
                Select New With {Group.Count(), Word}

For Each item In wordCount
    Console.WriteLine(item)
Next
```


</div>
</div>


```console
前略
{ Count = 2, Word = of }
{ Count = 2, Word = the }
{ Count = 2, Word = and }
{ Count = 3, Word = C# }
{ Count = 3, Word = is }    
```


これらの例では、Regex クラスのインスタンスを作っています。
作ったインスタンスを取っておけば、文字列で与えた正規表現を、内部的な表現にコンパイルする作業を1度限りにできて、実行効率が良くなります。
一方、実行効率を気にしない、もしくは、一度きりのパターン マッチングなら、静的メソッド版も使えます。

```csharp
var text = "abcde";
Console.WriteLine(Regex.Match(text, "a+"));
Console.WriteLine(Regex.Match(text, "a.*e"));
```


```console
a
abcde
```


以下では、正規表現の中身（Regex クラスに与える文字列）の説明をしていきましょう。


## <a id="sec-generated-title-3"></a> <a id="basic"></a>正規表現の基本: 文字をそのまま書く

いくつかの特別な意味を持った記号（. {} \ * + など）以外は、一致させたい文字をそのまま書きます。例えば、abという正規表現は、abを含む文字列に一致します。

<table summary="">

	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`ab`を含む文字列に一致します。`ab`の前後に別の文字があっても構いません。`a`と`b`の間に別の文字が入る場合には一致しません。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`abc`</td>
		<td markdown="1">`stab`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`a`</td>
		<td markdown="1">`acb`</td>
	</tr>
</table>


この例をC#で書くと、以下のようになります。

```csharp
var r = new Regex("ab");
Console.WriteLine(r.Match("abc").Success); // true
Console.WriteLine(r.Match("enable").Success); // true
Console.WriteLine(r.Match("a").Success); // false
Console.WriteLine(r.Match("acb").Success); // false
```



## <a id="sec-generated-title-4"></a> <a id="quantity"></a>数量指定

同じ文字の繰り返しを検出したい場合に使える、数量指定用の特殊記号として、 <code>*</code>（アスタリスク）、<code>+</code>（プラス）、<code>?</code> （はてな）、<code>{}</code> （波括弧）などがあります。

<table summary="">

	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab*a`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`*` (アスタリスク)で、0個以上の同じ文字を表します</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`aa`</td>
		<td markdown="1">`abbba`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`a`</td>
		<td markdown="1">`ab`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab+a`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`+` (プラス)で、1個以上の同じ文字を表します</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`aba`</td>
		<td markdown="1">`abbba`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`aa`</td>
		<td markdown="1">`aca`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab?a`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`?` (はてな)で、0個もしくは1個の文字を表します</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`aa`</td>
		<td markdown="1">`aba`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`abba`</td>
		<td markdown="1">`aca`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab{2}a`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`{}` で、連続する同じ文字を表します。数字を1つだけ入れると、その個数ぴったりを表します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`abba`</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`aba`</td>
		<td markdown="1">`abbba`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`ab{2,4}a`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`{}` に、コンマで区切って2つの数字を入れると、最小と最大の数指定できます。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`abba`</td>
		<td markdown="1">`abbbba`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`aba`</td>
		<td markdown="1">`abbbbba`</td>
	</tr>
</table>


通常、これらの数量指定は「最大一致」になります。一方、これらの記号の後ろに ? （はてな）をつけることで、「最小一致」パターンも作れます。

```csharp
var r1 = new Regex(@".*,");  // 任意の文字の後ろにコンマ
var r2 = new Regex(@".*?,"); // 同上。ただし、最小一致
var str = "aaa,aaa,aaa,";

Console.WriteLine(r1.Match(str)); // aaa,aaa,aaa, まで拾われる
Console.WriteLine(r2.Match(str)); // aaa, だけ拾われる
```



## <a id="sec-generated-title-5"></a> <a id="escape"></a>エスケープ

特殊な意味を持つ記号（<code>.</code> や <code>*</code>）自体を検索するためには、特殊記号の前に \ 記号（半角の円記号、フォントによっては逆スラッシュになります）をつけます。

<table summary="">

	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`\\\.\*`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`\` の直後に特殊記号を書くことで、特殊記号自身を検索できます。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`\.*`</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`\a*`</td>
		<td markdown="1">`\.`</td>
	</tr>
</table>


また、普通は見えない文字（改行やタブ文字）も、\ 記号に続けて n や t などの文字を書くことで表現します。主要なものを書くと、以下の通りです。

<table summary="">

	<tr>
		<td markdown="1">`\t`</td>
		<td markdown="1">タブ文字。</td>
	</tr>
	<tr>
		<td markdown="1">`\n`</td>
		<td markdown="1">改行文字。</td>
	</tr>
	<tr>
		<td markdown="1">`\r`</td>
		<td markdown="1">キャリッジ リターン（復帰）文字。</td>
	</tr>
	<tr>
		<td markdown="1">`\u`<span style="font-style:italic; font-family:serif">nnnn</span></td>
		<td markdown="1">Unicodeを直接指定します。<span style="font-style:italic; font-family:serif">nnnn</span>のところに、Unicodeを16進数で記述します。</td>
	</tr>
</table>


このような、特殊記号/不可視文字を入力するための記法をエスケープ（escape: 逃げ道、避難）と呼びます。


## <a id="sec-generated-title-6"></a> <a id="character-class"></a>文字クラス

特定の文字ではなく、ある範囲の文字（たとえば、算用数字全部など）と一致するようなパターンを作ることができます。

エスケープ同様、\ 記号に続けて d や s などの文字を書くことで、文字クラスを表現します。また、<code>[]</code> （角括弧）中に複数の文字を入れることで、そのいずれかの文字に一致します。

<table summary="">

	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`\d+`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`\` 記号は特別な意味を持ちます。`\d` や `\s` など、直後の文字によって意味が変わります。`\d` は任意の算用数字を表します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`1234`</td>
		<td markdown="1">`65536`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`abc`</td>
		<td markdown="1">`----`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`\sx+\s`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`\s` は任意の空白文字を表します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`y x y`</td>
		<td markdown="1">`y xxx y`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`yxy`</td>
		<td markdown="1">`yxxxxy`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`\w+\s+\w+`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`\w` は単語に使われる文字を表します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`abc xyz`</td>
		<td markdown="1">`あいう えお`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`abcdef`</td>
		<td markdown="1">`あいうえお`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`\p{Ps}\w+\p{Pe}`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`\p{}` で特定の Unicode カテゴリーに一致します。Ps は開き括弧、Pe は閉じ括弧です。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`(abc}`</td>
		<td markdown="1">`【abc]`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`|abc|`</td>
		<td markdown="1">`.abc.`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`a.*\.`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`.` （ピリオド）は任意の1文字を表します。ピリオド自信を表すためには、`\.` と書きます。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`abcd.`</td>
		<td markdown="1">`a(!#$%&amp;'().`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`abcd`</td>
		<td markdown="1">`a`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`[,\d]+`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`[]` （各括弧）中に含まれる任意の文字に一致します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`19,800`</td>
		<td markdown="1">`12,34,56`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`abcd`</td>
		<td markdown="1">`あいうえ`</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3"></td>
	</tr>
	<tr>
		<th>正規表現</th>
		<td markdown="1" colspan="2">`^[,\d]+$`</td>
	</tr>
	<tr>
		<th>説明</th>
		<td markdown="1" colspan="2">`^` は文字列の先頭、`$` は末尾を意味します。</td>
	</tr>
	<tr>
		<th>一致例</th>
		<td markdown="1">`19,800`</td>
		<td markdown="1">`12,34,56`</td>
	</tr>
	<tr>
		<th>不一致例</th>
		<td markdown="1">`-19,800`</td>
		<td markdown="1">`12,34.56`</td>
	</tr>
</table>



## <a id="sec-generated-title-7"></a> <a id="grouping"></a>グループ化

パターンの一部分だけ取り出したり、置換したりするために、正規表現内にグループを作ることができます。<code>()</code> （丸括弧）でくくった部分がグループになります。

例えば以下のようなコードを見てみましょう。

```csharp
var r = new Regex(@"(\d{4})/(\d{2})/(\d{2})");
var m = r.Match("2011/12/15");
 
foreach (var x in m.Groups)
{
    Console.WriteLine(x);
}
```


<code>()</code>が3か所あります。マッチ結果（m）のGroupsには、マッチした全体と、<code>()</code> でくくった3か所の結果が格納されています。したがって、実行結果は以下の通りです。

```console
2011/12/15
2011
12
15
```


グループには、名前を付けておくこともできます。<code>(?&lt;id&gt;パターン)</code> というように、<code>()</code> 内の先頭に <code>?&lt;&gt;</code> をつけます。

```csharp
var r = new Regex(@"(?<y>\d{4})/(?<m>\d{2})/(?<d>\d{2})");
var m = r.Match("2011/12/15");

Console.WriteLine(m.Groups["y"]); // 2011
Console.WriteLine(m.Groups["m"]); // 12
Console.WriteLine(m.Groups["d"]); // 15
```
