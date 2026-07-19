---
title: "例外処理"
source_url: "https://ufcpp.net/study/powershell/syntax/exception/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2007-05-23T00:00:00"
tags: []
umbraco_id: 1587
parent_id: 1577
sort_order: 9
aliases:
  - "/powershell/exception"
  - "/powershell/exception.html"
  - "/powershell/syntax/exception/"
  - "/study/powershell/exception"
  - "/study/powershell/exception.html"
---

# 例外処理

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
PowerShell では throw で例外を投げて、trap で例外を処理します。

シェルスクリプト言語の割には例外処理がきっちりしていると思います。
（微妙に挙動がつかめないところもあるんだけど・・・。
外部スクリプト内で 0 除算（1/0 とか）すると例外が trap できなかったり。）


##<a id="sec-generated-title-2"></a> <a id="exception"></a>例外
PowerShell では、（タイプミスしたりして）未定義のコマンドを入力したり、
不正な型変換をしたり、0 除算したり、
なんらかのエラーが起きたとき、例外を投げます。

例外は、特に何もしなければ、例外の内容を赤字で表示します。

<pre class="console" title="例外">
<span class="prompt">&gt; </span> UndefinedName
用語 'UndefinedName' は、コマンドレット、関数、操作可能なプ
ログラム、またはスクリプト ファイルとして認識されません。用
語を確認し、再試行してください。
発生場所 行:1 文字:13
+ UndefinedName &lt;&lt;&lt;&lt;
<span class="prompt">&gt; </span> 1/0
0 で除算しようとしました。
発生場所 行:1 文字:3
+ 1/0 &lt;&lt;&lt;&lt;
<span class="prompt">&gt; </span> [int]'test'
値 "test" を型 "System.Int32" に変換できません。エラー: "入
力文字列の形式が正しくありません。"
発生場所 行:1 文字:6
+ [int]' &lt;&lt;&lt;&lt; test'
</pre>


.NET Framework オブジェクトのメソッドが投げる例外も同様の扱いになります。

<pre class="console" title="例: int.Parse から生じた例外">
<span class="prompt">&gt; </span> [int]::Parse('test')
"1" 個の引数を指定して "Parse" を呼び出し中に例外が発生しました: 
"入力文字列の形式が正しくありません。"
発生場所 行:1 文字:13
</pre>



##<a id="sec-generated-title-3"></a> <a id="error"></a>$Error
ちなみに、例外の詳細は $Error 自動変数の中身を覗くことで分かります。
$Error には、過去に発生した例外のリストが格納されていて、
直前に発生した例外は $Error[0] に入っています。

$Error は ArrayList で、 System.Management.Automation.RuntimeException 型の値か System.Management.Automation.ErrorRecord 型が入っています。
「コマンドが見つからない」とか「型変換に失敗した」とか「0 除算」とかの、
PowerShell 内のエラーは RuntimeException に、
.NET Framework オブジェクトのメソッド中などで起きたエラーは ErrorRecord になります。

RuntimeException の場合、InnerException プロパティに例外の内容が格納されています。

<pre class="console" title="$Error、RuntimeException.InnerException">
<span class="prompt">&gt; </span> 1/0
0 で除算しようとしました。
発生場所 行:1 文字:3
+ 1/0 &lt;&lt;&lt;&lt;
<span class="prompt">&gt; </span> $Error[0].InnerException.GetType().Name
DivideByZeroException
</pre>


ErrorRecord 型の CategoryInfo プロパティに例外に関する情報が格納されています。

<pre class="console" title="$Error、ErrorRecord.Exception">
<span class="prompt">&gt; </span> [int]::Parse('test')
"1" 個の引数を指定して "Parse" を呼び出し中に例外が発生しました: 
"入力文字列の形式が正しくありません。"
発生場所 行:1 文字:13
+ [int]::Parse( &lt;&lt;&lt;&lt; 'test')
<span class="prompt">&gt; </span> $Error[0].CategoryInfo

Category   : NotSpecified
Activity   :
Reason     : MethodInvocationException
TargetName :
TargetType :
</pre>


[PowerShell Memo](http://d.hatena.ne.jp/newpops/) で、
↑の説明に沿って発生した例外の型を調べる関数の例が示されています。
「[発生した例外クラス名を調べる](http://d.hatena.ne.jp/newpops/20051211)」


##<a id="sec-generated-title-4"></a> <a id="throw"></a>例外の throw
例外は throw キーワードを使って自分で投げることもできます。
throw は文字列、.Net Framework の例外クラス、ErrorRecord 型のいずれかを受け取ります。
（どれを throw しても、
$Error の中身は RuntimeException になるみたい。）

文字列を throw に渡した場合、
$Error の中身は、
渡した文字列を Message プロパティに格納した RuntimeException になります。

<pre class="console" title="throw Exception クラス">
<span class="prompt">&gt; </span> throw 'error message'
error message
発生場所 行:1 文字:6
+ throw  &lt;&lt;&lt;&lt; 'error message'
<span class="prompt">&gt; </span> $Error[0].InnerException.Message
error message
<span class="prompt">&gt; </span> $Error[0].InnerException.GetType().Fullname
System.Management.Automation.RuntimeException
</pre>


.Net Framework の例外クラスを throw した場合には、
throw した例外を InnerException として含む RuntimeException になります。

<pre class="console" title="">
<span class="prompt">&gt; </span> throw New-Object ArgumentException
値が有効な範囲にありません。
発生場所 行:1 文字:6
+ throw  &lt;&lt;&lt;&lt; New-Object ArgumentException
<span class="prompt">&gt; </span> $Error[0].InnerException.GetType().Fullname
System.ArgumentException
</pre>


ErrorRecord を throw するのはちょっと複雑なんですが、
例としては以下のような感じ。

<pre class="console" title="throw ErrorRecord">
<span class="prompt">&gt; </span> throw New-Object Management.Automation.ErrorRecord
  (New-Object ArgumentException), 'test',
  ([Management.Automation.ErrorCategory]::InvalidArgument), ($null)
値が有効な範囲にありません。
発生場所 行:1 文字:6
+ throw  &lt;&lt;&lt;&lt; New-Object Management.Automation.ErrorRecord (
New-Object ArgumentException), 'test', ([Management.Automat
ion.ErrorCategory]::InvalidArgument), ($null)
<span class="prompt">&gt; </span> $Error[0].ErrorRecord.FullyQualifiedErrorId
test
<span class="prompt">&gt; </span> $Error[0].ErrorRecord.CategoryInfo

Category   : InvalidArgument
Activity   :
Reason     : ArgumentException
TargetName :
TargetType :
</pre>



##<a id="sec-generated-title-5"></a> <a id="trap"></a>例外の trap
今までは例外を投げっぱなしの状態でしたが、
ちゃんと例外を拾って処理することもできます。
例外処理は trap キーワードを使って行います。

C# や Java なんかだと、
try ブロックを書いて、
try ブロック内で生じた例外を catch ブロックで拾うわけですが
（参考： 「[例外処理](../../csharp/structured/oo_exception.md)」）、
PowerShell の trap はちょっと書き方が違います。

PowerShell の trap は、C# や Java でいうところの catch の方に相当するもので、
例外を拾いたいブロック内のどこでもいいので書いておけば、ブロック内で生じた例外を拾います。

例えば、以下のようなスクリプトを書くと、
trap はスクリプト内で生じた例外を拾います。
（trap はスクリプト内のどこに書いても同じ）

<pre class="source" title="test.ps1" lang="">
<code>1
[int]'test'
2

trap { 'trap exception' }
</code></pre>


<pre class="console" title="trap">
1
<em>trap exception</em>
値 "test" を型 "System.Int32" に変換できません。エラー: "入力
文字列の形式が正しくありません。"
2
</pre>


まあ、結果を見ての通り、
例外を trap しても、その後、エラーメッセージを表示した上で処理は続行します。


###<a id="sec-generated-title-6"></a> <a id="trap_break"></a>trap 内の break, continue
ここで、trap ブロック内に break か continue を書くことで、
挙動を変更することができます。
まず、continue を書くと、
エラーメッセージは表示せず、trap 内に書かれた処理だけして、
あとは何事もなかったかのように処理を続行します。

<pre class="source" title="test.ps1" lang="">
<code>1
[int]'test'
2

trap { 'trap exception'; <em>continue</em> }
</code></pre>


<pre class="console" title="trap 内に break">
1
trap exception
2
</pre>


一方、break を書くと、
エラーメッセージを表示して、残った処理はせずにブロック（あるいはスクリプト）を抜けます。

<pre class="source" title="test.ps1" lang="">
<code>1
[int]'test'
2

trap { 'trap exception'; <em>break</em> }
</code></pre>


<pre class="console" title="trap 内に break">
1
trap exception
値 "test" を型 "System.Int32" に変換できません。エラー: "入力
文字列の形式が正しくありません。"
</pre>


ちなみに、この挙動は、要するに、break によって例外が再 throw されています。
上位のブロックで trap すればエラーメッセージは表示されなくなります。
（以下の例では、
function f 内の trap では break しているので、エラー発生後の 2 は表示されません。
一方、function 外の trap では continue しているので、エラーの発生源である f より後ろの 'b' が表示されます。）

<pre class="source" title="test.ps1" lang="">
<code>function f
{
  trap { 'trap in function'; break }
  1
  [int]'test'
  2
}

'a'
f
'b'

trap { 'trap in script'; continue }
</code></pre>


<pre class="console" title="例外の再 throw と、上位ブロックでの trap">
<span class="prompt">&gt; </span> .\test.ps1
a
1
trap in function
trap in script
b
</pre>



###<a id="sec-generated-title-7"></a> <a id="trap_certain"></a>特定の例外だけ trap
trap {ブロック} と書くことで任意の例外を拾っていましたが、
trap [型] {ブロック} と書くことで特定の型の例外だけを拾えます

<pre class="source" title="test.ps1" lang="">
<code>1
[int]'test'
2
UndefinedName
3

trap [InvalidCastException] { 'trap for invalid cast'; continue }
trap [SystemException] { 'trap for system'; continue }
</code></pre>


<pre class="console" title="特定の型だけ trap">
<span class="prompt">&gt; </span> .\test.ps1
1
trap for invalid cast
2
trap for system
3
</pre>
