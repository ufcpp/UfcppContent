---
title: "コマンド実行"
source_url: "https://ufcpp.net/study/powershell/cmdlet/cmdlet/"
content_type: "Article"
published_at: "2007-05-19T00:00:00"
updated_at: "2007-05-24T00:00:00"
tags: []
umbraco_id: 1589
parent_id: 1588
sort_order: 0
aliases:
  - "/powershell/cmdlet.html"
  - "/powershell/cmdlet/cmdlet/"
  - "/study/powershell/cmdlet.html"
---

# コマンド実行

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（書きかけ）


## <a id="sec-generated-title-2"></a> <a id="param"></a>パラメータ

パラメータの渡し方

「位置パラメータ」と「名前付きパラメータ」

例: ls。
「ls .」← . は位置パラメータ。
「ls -Path .」← Path という名前の付いたパラメータ

パイプラインを通して値を渡す。
Where-Object とか ForEach-Object の -InputObject パラメータなんかはどの受け取り方もできる。

必須
位置
既定値
パイプライン入力を許可する
ワイルドカード文字を許可する

Parameter required?true
Parameter position?1
Parameter type        String
Default value
Accept multiple values?false
Accepts pipeline input?true
Accepts wildcard characters?true


## <a id="sec-generated-title-3"></a> <a id="commonparam"></a>common parameter

common parameter
man about_commonparameters
Verbose
Debug
ErrorAction
ErrorVariable
OutVariable
OutBuffer
共通パラメータに加えて、システムの状態を変更するコマンドレットがサポート
しているパラメータが 2 つあります。
WhatIf
Confirm


## <a id="sec-generated-title-4"></a> <a id="CmdletClass"></a>Cmdlet の実体

実体は Cmdlet クラスを継承したクラス

引数はプロパティで受け取る

名前付きパラメータの場合、プロパティ名がそのままパラメータ名に。
位置パラメータの場合、
PositionAttribute 属性を付けて位置を指定。

begin, process, end


## <a id="sec-generated-title-5"></a> <a id="pipeline"></a>パイプライン

パイプラインの挙動

```console
> 1,2,3,4 |
  %{Start-Sleep 0.3; Write-Warning 1; $_ } |
  %{Start-Sleep 0.5; Write-Warning 2; $_ } |
  %{Start-Sleep 0.7; Write-Warning 3; $_ }

警告: 1
警告: 2
警告: 3
1
警告: 1
警告: 2
警告: 3
2
警告: 1
警告: 2
警告: 3
3
警告: 1
警告: 2
警告: 3
4
```


↑きっちりこの順で出るってことは、
各 Cmdlet が Enumerator みたいな挙動してるはず。
感覚的にはほんと、LINQ だわ。


## <a id="sec-generated-title-6"></a> <a id="snapin"></a>SnapIn

SnapIn
