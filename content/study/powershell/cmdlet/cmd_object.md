---
title: "オブジェクト操作 Cmdlet"
source_url: "https://ufcpp.net/study/powershell/cmdlet/cmd_object/"
content_type: "Article"
published_at: "2007-05-22T00:00:00"
updated_at: "2015-05-06T14:21:17"
tags: []
umbraco_id: 1591
parent_id: 1588
sort_order: 2
aliases:
  - "/powershell/cmd_object"
  - "/powershell/cmd_object.html"
  - "/powershell/cmdlet/cmd_object/"
  - "/study/powershell/cmd_object"
  - "/study/powershell/cmd_object.html"
---

# オブジェクト操作 Cmdlet

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
if, while, for などの制御構文に加えて、
パイプラインを通したオブジェクトの処理用に、
Where-Object や ForEach-Object などの Cmdlet があります。
慣れると制御構文の方の foreach や if などよりも便利かも。


##<a id="sec-generated-title-2"></a> <a id="foreach"></a>ForEach-Object
一番分かりやすいのは、制御構文に同じ名前の物がある
<strong id="foreach_object" class="keyword">ForEach-Object</strong> でしょうか。
パイプラインで受け取ったオブジェクトに対して処理を行うもので、
以下のように使います。

<pre class="console" title="ForEach-Object">
<span class="prompt">&gt; </span>1,2,3,4 | ForEach-Object { $_ * $_ }
1
4
9
16
</pre>


ForEach-Object は第1引数に「[スクリプトブロック](../syntax/function.md#scriptblock)」を受け取ります。
$_ 自動変数を使ってオブジェクトに対する処理を書きます。

ForEach-Object は頻繁に使うものなので、
利便性を考えて「[エイリアス](../syntax/basic.md#alias)」が設定されていて、
foreach という名前と、% という名前でも呼び出せます。

<pre class="console" title="foreach, %">
<span class="prompt">&gt; </span>2,3,4 | foreach { $_ * $_ }
4
9
16
<span class="prompt">&gt; </span>2,3,4 | %{ $_ * $_ }
4
9
16
</pre>


その他、オプションで、begin ブロックと end ブロックも受け取れます。
（挙動は関数の begin, process, end と同じ。
「[begin, process, end](../syntax/function.md#process)」参照。）

<pre class="console" title="ForEach-Object">
<span class="prompt">&gt; </span>2,3,4 | % -begin {$num = 0} -process {++$num; $_} -end {"total $num"}
2
3
4
total 3
</pre>



##<a id="sec-generated-title-3"></a> <a id="where"></a>Where-Object
ForEach-Object 以外に、
SQL クエリ的な使い方のできる <strong id="where_object" class="keyword">Where-Object</strong> などの Cmdlet もあります。

まず、Where-Object ですが、
パイプライン中のオブジェクトのうち、
指定した条件を満たすものだけを取り出します。
条件式は、やはり $_ を使って書きます。

<pre class="console" title="Where-Object">
<span class="prompt">&gt; </span>1,9,3,7,5 | Where-Object {$_ -le 5}
1
3
5
</pre>


Where-Object にも「[エイリアス](../syntax/basic.md#alias)」が設定されていて、
where と ? でも呼び出せます。

<pre class="console" title="where, ?">
<span class="prompt">&gt; </span>1,9,3,7,5 | where {$_ -le 5}
1
3
5
<span class="prompt">&gt; </span>1,9,3,7,5 | ?{$_ -le 5}
1
3
5
</pre>


まあ、要するに、以下の foreach と同じ挙動です。

<pre class="console" title="foreach で where と同じ処理">
<span class="prompt">&gt; </span>1,9,3,7,5 | %{if($_ -le 5) {$_}}
1
3
5
</pre>



##<a id="sec-generated-title-4"></a> <a id="d33e90"></a>Select-Object
<strong id="select_object" class="keyword">Select-Object</strong> Cmdlet は、パイプラインから受け取ったオブジェクトのうち、
特定のプロパティのみを取り出したオブジェクトを出力します。

<pre class="console" title="Select-Object">
<span class="prompt">&gt; </span>ls C:\WINDOWS\Web\*.gif | Select-Object Name, LastWriteTime

Name                                              LastWriteTime
----                                              -------------
bullet.gif                                        2004/08/05 21:00:00
exclam.gif                                        2004/08/05 21:00:00
tips.gif                                          2004/08/05 21:00:00
</pre>


これも select というエイリアスが付いています。
where と合わせて、SQL クエリチックな書き方ができます。

<pre class="console" title="where と select の組み合わせ">
<span class="prompt">&gt; </span>ls C:\WINDOWS |
  where {$_.Name -like "d*"} |
  select Name, LastWriteTime


Name                                              LastWriteTime
----                                              -------------
Debug                                             2007/05/10 16:15:20
DOCS                                              2005/03/24 13:22:35
DOTNETFX                                          2005/03/24 13:22:43
<span class="input">後略</span>
</pre>



##<a id="sec-generated-title-5"></a> <a id="group"></a>Group-Object
<strong id="group_object" class="keyword">Group-Object</strong> （エイリアス: group）は、
同じプロパティの値を持つオブジェクトをグループ化します。
（SQL の Group By に相当。）
例えば、以下の通り。

<pre class="console" title="Group-Object">
<span class="prompt">&gt; </span>ls C:\WINDOWS\Web | group Extension

Count Name                      Group
----- ----                      -----
    2                           {printers, Wallpaper}
    3 .gif                      {bullet.gif, exclam.gif, tips.gif}
    2 .htt                      {deskmovr.htt, safemode.htt}
    1 .htm                      {tip.htm}
</pre>


ちなみに、Group-Object の出力結果の型は
Microsoft.PowerShell.Commands.GroupInfo の配列です。
GroupInfo は Name や Values などのプロパティを持っています。

<pre class="console" title="GroupInfo のプロパティ">
<span class="prompt">&gt; </span>ls C:\WINDOWS\Web | group Extension | ?{$_.Name -like ".h*"}

Count Name                      Group
----- ----                      -----
    2 .htt                      {deskmovr.htt, safemode.htt}
    1 .htm                      {tip.htm}
</pre>



##<a id="sec-generated-title-6"></a> <a id="sort"></a>Sort-Object
もう1個、SQL の sort by に相当する <strong id="sort_object" class="keyword">Sort-Object</strong> （エイリアス: sort）もあります。

<pre class="console" title="Sort-Object">
<span class="prompt">&gt; </span>ls C:\WINDOWS\Web | sort LastWriteTime, Name | select Name, LastWriteTime

Name                                              LastWriteTime
----                                              -------------
bullet.gif                                        2004/08/05 21:00:00
deskmovr.htt                                      2004/08/05 21:00:00
exclam.gif                                        2004/08/05 21:00:00
safemode.htt                                      2004/08/05 21:00:00
tip.htm                                           2004/08/05 21:00:00
tips.gif                                          2004/08/05 21:00:00
printers                                          2005/03/24 12:50:06
Wallpaper                                         2005/03/24 13:07:57
</pre>
