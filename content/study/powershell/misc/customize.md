---
title: "環境のカスタマイズ"
source_url: "https://ufcpp.net/study/powershell/misc/customize/"
content_type: "Article"
published_at: "2007-05-24T00:00:00"
updated_at: "2015-05-06T14:21:31"
tags: []
umbraco_id: 1596
parent_id: 1594
sort_order: 1
aliases:
  - "/powershell/customize"
  - "/powershell/customize.html"
  - "/powershell/misc/customize/"
  - "/study/powershell/customize"
  - "/study/powershell/customize.html"
---

# 環境のカスタマイズ

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
プロファイルディレクトリに色々ファイルを置くことでカスタマイズができるみたい。


##<a id="sec-generated-title-2"></a> <a id="dir"></a>プロファイルディレクトリ
PowerShell Home
C:\WINDOWS\system32\windowspowershell\v1.0
ここはユーザは直接触らない

$env:ALLUSERSPROFILE 以下の
\My Documents\WindowsPowerShell

$env:userprofile 以下の
\My Documents\WindowsPowerShell


##<a id="sec-generated-title-3"></a> <a id="d36e22"></a>profile.ps1
プロファイルディレクトリ内に profile.ps1 を置いておくと、
PowerShell のホスト起動時に読み込まれる。
<pre>
      profile.ps1 に書いておくと便利そうな TIPS

      ?eq など
      http://d.hatena.ne.jp/newpops/20070111/p1

      Windows.Forms をロードしておく
      http://d.hatena.ne.jp/newpops/20061229/p1
    </pre>

##<a id="sec-generated-title-4"></a> <a id="d36e31"></a>Display.xml
##<a id="sec-generated-title-5"></a> <a id="d36e33"></a>ホストの配色
<pre class="source" title="配色のカスタマイズ" lang="">
<code>$Host.UI.RawUI.BackgroundColor = 'White'
$Host.UI.RawUI.ForegroundColor = 'Black'
$Host.PrivateData.DebugBackgroundColor = 'Gray'
$Host.PrivateData.DebugForegroundColor = 'Yellow'
$Host.PrivateData.ErrorBackgroundColor = 'White'
$Host.PrivateData.ErrorForegroundColor = 'Red'
$Host.PrivateData.ProgressBackgroundColor = 'DarkCyan'
$Host.PrivateData.ProgressForegroundColor = 'Yellow'
$Host.PrivateData.VerboseBackgroundColor = 'Gray'
$Host.PrivateData.VerboseForegroundColor = 'Yellow'
$Host.PrivateData.WarningBackgroundColor = 'Gray'
$Host.PrivateData.WarningForegroundColor = 'Yellow'
</code></pre>



##<a id="sec-generated-title-6"></a> <a id="d36e42"></a>prompt 関数
プロンプトとして表示されてる「PS パス名 &gt;」の部分は、実は prompt 関数の出力。
prompt 関数を書き換えることで変更可能。


##<a id="sec-generated-title-7"></a> <a id="d36e48"></a>TabExpansion 関数
TabExpansion 関数で、タブ補完の挙動を変えれるみたい。

http://blogs.msdn.com/powershell/archive/2006/04/26/584551.aspx

頑張れば bash 風のタブ補完も可能？


##<a id="sec-generated-title-8"></a> <a id="d36e60"></a>types.ps1xml
"${PSHOME}\types.ps1xml"
で .NET Framework の型を拡張可能
System.Array に Count プロパティを足したり


##<a id="sec-generated-title-9"></a> <a id="d36e66"></a>Display.xml
Format-List での表示項目とかの設定。
