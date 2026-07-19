---
title: "変数操作関連の Cmdlet"
source_url: "https://ufcpp.net/study/powershell/cmdlet/cmd_variable/"
content_type: "Article"
published_at: "2007-05-19T00:00:00"
updated_at: "2015-05-06T14:21:15"
tags: []
umbraco_id: 1590
parent_id: 1588
sort_order: 1
aliases:
  - "/powershell/cmd_variable"
  - "/powershell/cmd_variable.html"
  - "/powershell/cmdlet/cmd_variable/"
  - "/study/powershell/cmd_variable"
  - "/study/powershell/cmd_variable.html"
---

# 変数操作関連の Cmdlet

##<a id="sec-generated-title-1"></a> <a id="setget"></a>Set-Variable, Get-Variable
$ を使って変数の読み書きをする以外に、
<strong id="set_variable" class="keyword">Set-Variable</strong> と <strong id="get_variable" class="keyword">Get-Variable</strong> という Cmdlet を使うことでも変数の読み書きができます。

<pre class="console" title="Set-Variable, Get-Variable">
<span class="prompt">&gt; </span> Set-Variable a 1
<span class="prompt">&gt; </span> Get-Variable a

Name                           Value
----                           -----
a                              1

<span class="prompt">&gt; </span> $a = 2
<span class="prompt">&gt; </span> Get-Variable a

Name                           Value
----                           -----
a                              2
</pre>


まあ、単に値を代入するだけなら Set-Variable は必要ないんですが、
Set-Variable を使うと、
ReadOnly / Constant 属性を付与することができます。

<pre class="console" title="Set-Variable -option">
<span class="prompt">&gt; </span> Set-Variable a 1 -option ReadOnly
<span class="prompt">&gt; </span> $a = 0
変数 a は読み取り専用または定数であるため、上書きできません。
</pre>


また、
Get-Variable では任意のレベルのスコープの変数にアクセスしたりできます。

<pre class="console" title="Get-Variable -scope">
<span class="prompt">&gt; </span> Get-Variable a –scope 1  # 親スコープから値を取得
<span class="prompt">&gt; </span> $Get-Variable a –scope 2  # 祖父
</pre>


その他、<strong id="remove_variable" class="keyword">Remove-Variable</strong> で変数を削除したりもできます。

<pre class="console" title="Remove-Variable">
<span class="prompt">&gt; </span> $a = 0
<span class="prompt">&gt; </span> Remove-Variable a
<span class="prompt">&gt; </span> Get-Variable a
Get-Variable : 名前 'a' の変数が見つかりません。
</pre>
