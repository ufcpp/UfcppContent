---
title: "プロバイダとドライブ"
source_url: "https://ufcpp.net/study/powershell/misc/psdrive/"
content_type: "Article"
published_at: "2007-05-24T00:00:00"
updated_at: "2017-04-06T17:21:50"
tags: []
umbraco_id: 1595
parent_id: 1594
sort_order: 0
aliases:
  - "/powershell/misc/psdrive/"
  - "/powershell/psdrive"
  - "/powershell/psdrive.html"
  - "/study/powershell/psdrive"
  - "/study/powershell/psdrive.html"
---

# プロバイダとドライブ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

PowerShell では、
ファイルシステム、レジストリ、環境変数なんかの項目を、
どれも同じコマンドで操作できます。

Get-PSDrive Cmdlet

<pre class="console" title="">
<span class="prompt">&gt; </span>Get-PSDrive

Name       Provider      Root
----       --------      ----
Alias      Alias
C          FileSystem    C:\
cert       Certificate   \
D          FileSystem    D:\
Env        Environment
Function   Function
HKCU       Registry      HKEY_CURRENT_USER
HKLM       Registry      HKEY_LOCAL_MACHINE
Variable   Variable
</pre>


PowerShell 中で定義したエイリアス、関数、変数すらも、ファイルシステムと同じ構文でアクセス可能。

<pre class="console" title="">
<span class="prompt">&gt; </span>ls variable:*

Name                           Value
----                           -----
Error                          {}
DebugPreference                SilentlyContinue
<span class="input">後略</span>
</pre>


パスの書き方
[&lt;プロバイダ&gt;::]&lt;ドライブ&gt;:[\&lt;コンテナ&gt;[\&lt;サブコンテナ&gt;...]]\&lt;項目&gt;

特殊文字
. .. \

環境変数が取りたければ ls env:* で出来るし、
レジストリの項目を cd HKCU:\Software\Microsoft\Windows と探索可能。

環境変数とかは、以下のような構文で、あたかも変数のようにアクセス可能。

<pre class="console" title="">
<span class="prompt">&gt; </span>$env:windir
C:\WINDOWS
</pre>


${C:\Users\Public\test.txt} みたいなのでファイルの中身を読み書きできるのも同じ原理みたい。

こういう、ファイルシステム以外もファイルシステムと同様の扱いするための機構を提供するのがプロバイダらしい。

<pre class="console" title="">
<span class="prompt">&gt; </span>Get-PSProvider

Name                 Capabilities                            Drives
----                 ------------                            ------
Alias                ShouldProcess                           {Alias}
Environment          ShouldProcess                           {Env}
FileSystem           Filter, ShouldProcess                   {C, D}
Function             ShouldProcess                           {Function}
Registry             ShouldProcess                           {HKLM, HKCU}
Variable             ShouldProcess                           {Variable}
Certificate          ShouldProcess                           {cert}
</pre>
