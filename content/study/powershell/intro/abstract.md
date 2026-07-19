---
title: "Windows PowerShell 概要"
source_url: "https://ufcpp.net/study/powershell/intro/abstract/"
content_type: "Article"
published_at: "2007-05-19T00:00:00"
updated_at: "2007-05-24T00:00:00"
tags: []
umbraco_id: 1574
parent_id: 1573
sort_order: 0
aliases:
  - "/powershell/abstract"
  - "/powershell/abstract.html"
  - "/powershell/intro/abstract/"
  - "/study/powershell/abstract"
  - "/study/powershell/abstract.html"
---

# Windows PowerShell 概要

##<a id="sec-generated-title-1"></a> <a id="abst"></a>PowerShell 概要
Windows <strong id="powershell" class="keyword">PowerShell</strong> は、
2006年末頃に登場した、
主にシステム管理者を対象としたシェルです。

シェルというと、要するに、
Unix 系 OS の csh, bash や、
MS-DOS プロンプトのようなもののことです。
コマンドラインインターフェースやスクリプト言語などを使って、
ファイル操作など OS 機能を実行します。
（シェル（shell）という言葉は、
OS の中心部分を貝殻（shell）のように覆う部分という意味からきているらしい。）

これまで、Unix に大きく後れを取っていたといわれる Windows のシェルですが、
PowerShell はまさに次世代のシェルといっていいほど洗練された高性能なシェル環境です。


##<a id="sec-generated-title-2"></a> <a id="character"></a>PowerShell の特徴
###<a id="sec-generated-title-3"></a> <a id="consistency"></a>一貫したインターフェース
PowerShell のコマンドは一貫した命名規則を持ちます。
また、引数の渡し方も一貫しています。

PowerShell のコマンドは
（従来の Windows ツールも PowerShell から呼び出し可能で、
これに関しては当てはまりませんが、
少なくとも PowerShell 用に作られた Cmdlet（コマンドレットと読む）と呼ばれるコマンドに関しては）
以下のような規則を持っています。

* 「動詞-名詞」をいう形の名前をしている。（例えば、Get-Help とか Set-Location とか。）

* Command -Name1 param1 -Name2 param2 ... というように、オプション的な書式で引数を与える。

* Command param1 param2 ... というように、位置による引数の与え方も可能（オプション的な書式と併用可能）。


例えば、以下のようになります。

<pre class="console" title="Cmdlet の例">
<span class="prompt">&gt; </span> Get-Command more

CommandType     Name         Definition
-----------     ----         ----------
Function        more         param([string[]]$paths);  if(($paths -ne $null) ...
Application     more.com     C:\Windows\system32\more.com
Application     more.exe     C:\cygwin\bin\more.exe

<span class="prompt">&gt; </span> Get-Alias -Name p* -Exclude ps

CommandType     Name         Definition
-----------     ----         ----------
Alias           popd         Pop-Location
Alias           pushd        Push-Location
Alias           pwd          Get-Location
</pre>



###<a id="sec-generated-title-4"></a> <a id="pipeline"></a>オブジェクトパイプライン
多くのシェルには、
あるコマンドの出力を別のあるコマンドの入力につなぐパイプラインという機能があります。

PowerShell でも、（他の多くのシェルがそうであるように）
| 記号を使って2つのコマンドをパイプラインでつなぐことができます。

<pre class="console" title="Cmdlet の例">
<span class="prompt">&gt; </span> Get-ChildItem | Select-Object {$_.Name}

$_.Name
-------
doc
sample
test.ps1
</pre>


ここで、他のシェルと大きく異なるのは、
あるコマンドから他のコマンドに渡されるのが文字列ではなく、
オブジェクトだということです。
（このような仕組みを<strong id="pipeline" class="keyword">オブジェクトパイプライン</strong>といいます。）

Unix 系のシェルを含め、
これまでのシェルの大半は、
コマンドの出力も入力も全て文字列として受け渡ししていました。
パイプライン元ではオブジェクトを一度文字列に整形して出力し、
パイプライン先では文字列を解釈しなおすという余計な処理が必要になります。

これに対して、PowerShell はオブジェクトのまま
（正確に言うと、.NET Framework のオブジェクト）
でパイプラインが行われます。
例えば、上の例の Get-ChildItem は、
System.IO.FileInfo 型の配列を出力します。
Select-Object はこの出力を FileInfo 型のまま受け取って処理を行っています。
（上の例では、FileInfo 型の Name プロパティを読み出しています。）


###<a id="sec-generated-title-5"></a> <a id="script"></a>新しいスクリプト言語
これまで Windows では、
WSH というスクリプトベースのシステム管理環境がありましたが、
元々シェル用のスクリプト言語ではない VB Script や JavaScript を使っていたので、
コマンドの呼び出しなどの部分で利便性に不満がありました。

また、既存のシェル環境では、
パイプライン処理が文字列ベースで行われていたことからも想像が付くかと思いますが、
シェルスクリプト言語がオブジェクトを管理することを念頭に入れて作られていないものが多いです。

そこで、PowerShell では、
簡便な記法でコマンドの呼び出しができ、さらに、
.NET Framework のオブジェクトの扱いにも長けた新しいスクリプト言語を用意しています。

この新しいスクリプト言語は、
以下のような点で、C# の文法を少し意識しているようです。

* スクリプトブロックは begin/end ではなく {} で囲う。

* 配列のインデックスには () や &lt;&gt; ではなく [] を使う。

* 連想配列でも {} とかではなく [] を使う。

* オブジェクトのプロパティ/メソッド参照には -&gt; や : ではなく . を使う。

* for each の構文は foreach ($x in $array) { ... } 。


まあ、PowerShell のユーザガイドには「C# と似た構文とキーワードを使用」と書かれているものの、
変数名の頭に $ を付けたり、
コメントが # で始まったり、
Perl 的な雰囲気もあったりします。
（あと、JavaScript 的な雰囲気も少し。）


##<a id="sec-generated-title-6"></a> <a id="forDeveloper"></a>開発者にとっての PowerShell
「PowerShell の主なターゲットはシステム管理者」といっても、
それ以外の層にとってのメリットがないわけではありません。
.NET 開発者にとっても大きなメリットを持っていると思います。

参考記事：「[The Virtuous Cycle: .NET Developers using PowerShell](http://blogs.msdn.com/powershell/archive/2007/05/18/the-virtuous-cycle-net-developers-using-to-powershell.aspx)」。
←こちらに .NET 開発者にとってのメリットが書かれています。
要約すると以下のような感じ。

例えば、Unix プログラマの場合、
最低でも3つの言語を使い分けて開発を行っています。

1. システム（アプリケーションやサービス）プログラミング言語： C, C++, Java, ...

2. シェル： ksh, bash, ...

3. スクリプト言語： Perl, Python, ...


で、この3つはお互い一貫性がない。
「Perl に慣れたからと言って bash が使えるようになるわけじゃないし」という感じ。

これに対して、PowerShell はシェルであると同時に、
強力なスクリプト言語も持っています。
また、シェル内で .NET Framework のオブジェクトを自由に操作できるので、
C# などの .NET システムプログラミング言語との一貫性があります。
PowerShell に慣れれば .NET Framework のオブジェクトに慣れることになるし、
その逆もまた然り。
（さらに言うと、PowerShell のスクリプト言語は C# を意識した構文を持っているので、
C# プログラマーなら尚のこと相性がいいです。）

ということで、PowerShell は .NET 開発者にとって非常に魅力的なシェル環境になると思います。


##<a id="sec-generated-title-7"></a> <a id="link"></a>リンク
[Windows PowerShell でのスクリプティング](http://www.microsoft.com/japan/technet/scriptcenter/hubs/msh.mspx)
: 日本語公式サイト。

[次世代Windowsシェル「Windows PowerShell」を試す](http://www.atmarkit.co.jp/fdotnet/special/powershell01/powershell01_01.html)
: ＠IT の特集記事。

[PowerShell FAQ](http://newpops.wankuma.com/)
: PowerShell 関連書籍「PowerShell 宣言！」の著者のサイト「PowerShell Memo」から、 PowerShell の Tips を抽出して整理したもの。
