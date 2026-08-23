---
title: "準備"
source_url: "https://ufcpp.net/study/powershell/intro/prepare/"
content_type: "Article"
published_at: "2007-05-26T00:00:00"
updated_at: "2007-05-28T00:00:00"
tags: []
umbraco_id: 1576
parent_id: 1573
sort_order: 2
aliases:
  - "/study/powershell/prepare.html"
---

# 準備

## <a id="sec-generated-title-1"></a> <a id="install"></a>インストール

Longhorn サーバ（コードネーム）には標準で入るらしいんですが。

PowerShell は Windows XP、Windows Server 2003 もしくは Vista にインストール可能です。
インストール方法は、
[公式サイト](http://www.microsoft.com/japan/technet/scriptcenter/hubs/msh.mspx)にインストーラが置いてあるので、
それをダウンロードして実行するだけです。
ただし、PowerShell のインストールには .NET Framework 2.0 以降が必要です。
（.NET Framework 2.0 は Microsoft Update でインストール可能。）


## <a id="sec-generated-title-2"></a> <a id="context_menu"></a>フォルダを右クリックで

エクスプローラで、フォルダを右クリックして、そのフォルダをカレントフォルダにして PowerShell を起動できるようにしておくと便利です。

レジストリをいじることになるんですが、
レジストリの値の設定も、いちいち .reg ファイルを書いたりしなくても、
PowerShell 上からコマンド1行でできるのが素敵。

```console {title="右クリックメニューに PowerShell を追加（Vista 用）"}
> New-Item HKLM:\SOFTWARE\Classes\Directory\shell -N psh -Va PowerShell
> New-Item HKLM:\SOFTWARE\Classes\Directory\shell\psh `
-N command -Va "`"C:\Windows\system32\WindowsPowerShell\v1.0\powershell.exe`" `
-NoExit -Command `"cd \`"%l\`"`""
```


Vista の場合、PowerShell を「管理者として実行」しないと権限の問題でエラーになるので注意。


## <a id="sec-generated-title-3"></a> <a id="script"></a>スクリプトファイルの実行

詳細は「[スクリプトの実行ポリシー](../syntax/basic.md#policy)」に書いていますが、
上述の右クリックメニュー登録のついでに、
以下のコマンドも1度実行しておくことをお勧めします。

```console {title="実行ポリシーを変更"}
> Set-ExecutionPolicy RemoteSigned
```
