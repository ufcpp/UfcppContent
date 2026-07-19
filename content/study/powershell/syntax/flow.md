---
title: "制御構文"
source_url: "https://ufcpp.net/study/powershell/syntax/flow/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2007-05-23T00:00:00"
tags: []
umbraco_id: 1585
parent_id: 1577
sort_order: 7
aliases:
  - "/powershell/flow"
  - "/powershell/flow.html"
  - "/powershell/syntax/flow/"
  - "/study/powershell/flow"
  - "/study/powershell/flow.html"
---

# 制御構文

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# を意識して作ってるだけあって、
PowerShell の制御構文は C# の物と結構似ています。

C# と違うのは、以下の点。

* else if は elseif とつなげて書く必要あり。

* while, for, foreach にラベルを付けられる。

* 「if(cond) x = 0;」みたいな書き方は無理。単文であっても {} で囲わないとダメ。

* do until 文がある。

* switch 文の記法がちょっと違う。


制御構文のコード例はどうしても長めになるので、
以下このページでは、
test.ps1 という名前のスクリプト中に書いたものを呼び出す形で説明します。


##<a id="sec-generated-title-2"></a> <a id="if"></a>if, elseif, else
条件分岐には <strong id="if" class="keyword">if</strong>, <strong id="elseif" class="keyword">elseif</strong>, <strong id="else" class="keyword">else</strong> を使います。
"else if" とか elif という書き方はしません。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

if ($x -lt 5)
{
  'x ＜ 5'
}
elseif ($x -lt 10)
{
  '5 ≦ x ＜ 10'
}
else
{
  '10 ≦ x'
}
</code></pre>


<pre class="console" title="if 文の例">
<span class="prompt">&gt; </span> .\test.ps1 3
x ＜ 5
<span class="prompt">&gt; </span> .\test.ps1 7
5 ≦ x ＜ 10
<span class="prompt">&gt; </span> .\test.ps1 11
10 ≦ x
</pre>


if や else の後は、
例え1文だけであっても {} でくくる必要があります。


##<a id="sec-generated-title-3"></a> <a id="flow"></a>switch
<strong id="switch" class="keyword">switch</strong> 文は、ちょっと C# と書式が違います。
switch(条件) と書くところまでは一緒で、
その後ろが以下のような点で違います。

* case と書く必要がない。

* ラベルの後ろの {} が必須。

* フォールスルーがない。


switch 文はかなりいろいろできるっぽいんですが、
基本形は以下の通りです。
‘case’と書く必要はないし、値の後ろに : は不要です。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch ($x)
{
  1 {"a"}
  2 {"b"}
  default {"default"}
}
</code></pre>


<pre class="console" title="switch の例（１）">
<span class="prompt">&gt; </span> .\test.ps1 1
a
<span class="prompt">&gt; </span> .\test.ps1 2
b
<span class="prompt">&gt; </span> .\test.ps1 3
default
</pre>


上からそれぞれ、 $x == 1 のとき、$x == 2 のとき、それ以外のときの処理です。
1 にマッチしたあと、2 や default の方にフォールスルー（参考：「[switch 文](../../csharp/structured/st_branch.md#switch)」）したりしません。

ただし、同じ値を複数書くこともできて、
その場合は複数のブロックが実行されます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch ($x)
{
  1 {"a1"}
  1 {"a2"}
  2 {"b"}
  default {"default"}
}
</code></pre>


<pre class="console" title="switch の例（２）">
<span class="prompt">&gt; </span> .\test.ps1 1
a1
a2
</pre>


また、値には定数だけでなくて、変数も使えます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

$a = 1
$b = 2

switch ($x)
{
  $a {"a"}
  $b {"b"}
  default {"default"}
}
</code></pre>



###<a id="sec-generated-title-4"></a> <a id="switch_cond"></a>値の代わりに条件式を書く
値の代わりに条件式もかけたりします。
条件式は {} でくくります。
また、自動変数 $_ を使って式を書きます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch ($x)
{
  {$_ -lt 5} {'x ＜ 5'}
  {$_ -lt 10} {'x ＜ 10'}
  default {"default"}
}
</code></pre>


で、この場合、
条件を満たす全てのブロックが実行されます。

<pre class="console" title="条件式による switch 文">
<span class="prompt">&gt; </span> .\test.ps1 3
x ＜ 5
x ＜ 10
<span class="prompt">&gt; </span> .\test.ps1 7
x ＜ 10
<span class="prompt">&gt; </span> .\test.ps1 10
default
</pre>


break キーワードを使って、
1つマッチした時点で処理を打ち切ることもできます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch ($x)
{
  {$_ -lt 5} {'x ＜ 5'; break}
  {$_ -lt 10} {'x ＜ 10'; break}
  default {"default"}
}
</code></pre>


<pre class="console" title="break 付きの switch">
<span class="prompt">&gt; </span> .\test.ps1 3
x ＜ 5
<span class="prompt">&gt; </span> .\test.ps1 7
x ＜ 10
</pre>



###<a id="sec-generated-title-5"></a> <a id="option"></a>大文字・小文字の区別
switch 文にはいくつかオプションを付けることができます。

1つは、アルファベットの大文字・小文字を区別するかどうかで、
switch の後ろに -c を付けると区別するようになり、
何も付けなければ区別しません。
（case sensitive の c。）

（
ヘルプなどの文章によると、
正式には -casesensitive オプションらしいんですが、
どうも最初の1文字しか見ていないみたい。
-cwertyuio とか書いても -casesensitive オプションとみなされます。
）

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch ($x)
{
  "t*"  {'t*'}
  "*s*" {'*s*'}
  default {'no match'}
}
</code></pre>


<pre class="console" title="case insensitive">
<span class="prompt">&gt; </span> .\test.ps1 t*
t*
<span class="prompt">&gt; </span> .\test.ps1 T*
t*
</pre>


<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch <em>-c</em> ($x)
{
  "t*"  {'t*'}
  "*s*" {'*s*'}
  default {'no match'}
}
</code></pre>


<pre class="console" title="case sensitive">
<span class="prompt">&gt; </span> .\test.ps1 t*
t*
<span class="prompt">&gt; </span> .\test.ps1 T*
no match
</pre>



###<a id="sec-generated-title-6"></a> <a id="option"></a>正規表現、ワイルドカード、完全一致
もう1つ、文字列の一致判定を、
正規表現、ワイルドカード、完全一致のいずれで行うかオプション指定できます。
（-c オプションとの併用も可能。）

<table summary="正規表現、ワイルドカード、完全一致">
	<caption>
		正規表現、ワイルドカード、完全一致
	</caption>
	<tr>
		<th>オプション</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">e</td>
		<td markdown="1">exact。完全一致。（特に何もオプション指定しなければこれになる。）</td>
	</tr>
	<tr>
		<td markdown="1">w</td>
		<td markdown="1">wild card。ワイルドカードを使って一致判定。</td>
	</tr>
	<tr>
		<td markdown="1">r</td>
		<td markdown="1">regular expression。正規表現を使って一致判定。</td>
	</tr>
</table>


<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch <em>-w</em> ($x)
{
  "t*"  {'t*'}
  "*s*" {'*s*'}
  default {'no match'}
}
</code></pre>


<pre class="console" title="ワイルドカード版">
<span class="prompt">&gt; </span> .\test.ps1 test
t*
*s*
<span class="prompt">&gt; </span> .\test.ps1 text
t*
<span class="prompt">&gt; </span> .\test.ps1 est
*s*
</pre>


<pre class="source" title="test.ps1" lang="">
<code>param ($x)

switch <em>-r</em> ($x)
{
  "^t.*"  {'t*'}
  ".*?s.*" {'*s*'}
  default {'no match'}
}
</code></pre>


<pre class="console" title="正規表現版">
<span class="prompt">&gt; </span> .\test.ps1 test
t*
*s*
<span class="prompt">&gt; </span> .\test.ps1 text
t*
<span class="prompt">&gt; </span> .\test.ps1 est
*s*
</pre>


（
-c（-casesinsitive）オプションと同様、
正確にはそれぞれ -exact、-wildcard、-regex オプション。
これも、最初の1文字しかみてないみたい。
）


###<a id="sec-generated-title-7"></a> <a id="file"></a>ファイルの中身を見て switch
なんか、何に使うかよく分からないけども、
-f オプション（多分、file の f）で、
ファイルの中身を見て switch できるみたい。

<pre class="source" title="test.ps1" lang="">
<code>switch -f test.txt
{
  5 {'test.txt contains 5'}
  10 {'test.txt contains 10'}
  default {'default'}
}
</code></pre>


<pre class="console" title="ファイルの中身で switch">
<span class="prompt">&gt; </span> 3, 5, 10 | Set-Content test.txt
<span class="prompt">&gt; </span> .\test.ps1
test.txt contains 5
test.txt contains 10
</pre>



##<a id="sec-generated-title-8"></a> <a id="while"></a>while, for, until
<strong id="while" class="keyword">while</strong> と <strong id="for" class="keyword">for</strong> は C# とほぼ同じです。
while(条件式) は条件式が真の間だけ反復処理、
for(a; b; c){ブロック} は a; while(b){ブロック} c; とほぼ同じ意味です。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

while ($x -gt 0)
{
  $x
  --$x
}
</code></pre>


<pre class="console" title="while">
<span class="prompt">&gt; </span> .\test.ps1 3
3
2
1
</pre>


<pre class="source" title="test.ps1" lang="">
<code>param ($x)

for ($sum = 1; $x -gt 1; --$x)
{
  $sum += $x
}

$sum
</code></pre>


<pre class="console" title="for">
<span class="prompt">&gt; </span> .\test.ps1 3
6
<span class="prompt">&gt; </span> .\test.ps1 4
10
</pre>


<strong id="dowhile" class="keyword">do while</strong> 文もあって、これも C# とほぼ同じです。
do {ブロック} while(条件式) で、最低1回はブロックを実行、
それ以後は条件式が真の間だけブロックを反復します。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

do
{
  $x
  --$x
} while ($x -gt 0)
</code></pre>


<pre class="console" title="do while">
<span class="prompt">&gt; </span> .\test.ps1 3
3
2
1
<span class="prompt">&gt; </span> .\test.ps1 0
0
</pre>


あと、PowerShell には do while の逆の意味の <strong id="dountil" class="keyword">do until</strong> というものもあります。
do {ブロック} until(条件式) で、条件式が偽の間ブロックを反復します。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

do
{
  $x
  --$x
} <em>until ($x -le 0)</em>
</code></pre>


<pre class="console" title="do until">
<span class="prompt">&gt; </span> .\test.ps1 3
3
2
1
<span class="prompt">&gt; </span> .\test.ps1 0
0
</pre>



##<a id="sec-generated-title-9"></a> <a id="foreach"></a>foreach
<strong id="foreach" class="keyword">foreach</strong> もほとんど C# と同じで、
foreach ($x in $array) {ブロック} です。
Perl みたいな書き方（for ($array) {$_}）はできません。
<strong id="in" class="keyword">in</strong> が必須です。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

foreach ($a in $x)
{
  $a * $a
}
</code></pre>


<pre class="console" title="foreach">
<span class="prompt">&gt; </span> .\test.ps1 1, 2, 3
1
4
9
</pre>



##<a id="sec-generated-title-10"></a> <a id="break"></a>break, continue
C# などと同様に、
<strong id="brak" class="keyword">break</strong>, <strong id="continue" class="keyword">continue</strong> を使って、ループから抜けたり、次の反復処理に移ったりできます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

foreach ($a in $x)
{
  if ($a -eq 0) { break } # 0 が来たらループ終了
  if (!($a -band 1)) { continue } # 偶数は飛ばす
  "$a is odd number"
}
</code></pre>


<pre class="console" title="break, continue">
<span class="prompt">&gt; </span> .\test.ps1 1, 2, 3, 4, 5, 0, 6, 7, 8, 9
1 is odd number
3 is odd number
5 is odd number
</pre>


C# と違って、（goto がないので）多重ループから抜けるときには、
while や do の前に「: ラベル」をつけた上で、
「break ラベル」あるいは「continue ラベル」と書きます。

ただし、ラベルの付け方にちょっと注意。
Java や JavaScript とは違って、
「:ラベル while(条件式)」というように、
: が先です。

ラベルは、
while, do while, do until, for, foreach のいずれにもつけることができます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

:label1 while ($true)
{
  :label2 while ($true)
  {
    $x
    if ($x -eq 10) { break label1 }
    $x -= 3
    if ($x -le 0)  { break label2 }
  }
  $x += 10
}
</code></pre>


<pre class="console" title="break ラベル">
<span class="prompt">&gt; </span> .\test.ps1 4
4
1
8
5
2
9
6
3
10
</pre>


あと、ラベルを（文字列として）変数に格納して使ったりもできます。

<pre class="source" title="test.ps1" lang="">
<code>param ($x)

$l1 = "label1"
$l2 = "label2"

:label1 while ($true)
{
  :label2 while ($true)
  {
    $x
    if ($x -eq 10) { break $l1 }
    $x -= 3
    if ($x -le 0)  { break $l2 }
  }
  $x += 10
}
</code></pre>
