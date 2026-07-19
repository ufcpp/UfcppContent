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

## <a id="sec-generated-title-1"></a> <a id="setget"></a>Set-Variable, Get-Variable

$ を使って変数の読み書きをする以外に、
<strong id="set_variable" class="keyword">Set-Variable</strong> と <strong id="get_variable" class="keyword">Get-Variable</strong> という Cmdlet を使うことでも変数の読み書きができます。

```console
>  Set-Variable a 1
>  Get-Variable a

Name                           Value
----                           -----
a                              1

>  $a = 2
>  Get-Variable a

Name                           Value
----                           -----
a                              2
```


まあ、単に値を代入するだけなら Set-Variable は必要ないんですが、
Set-Variable を使うと、
ReadOnly / Constant 属性を付与することができます。

```console
>  Set-Variable a 1 -option ReadOnly
>  $a = 0
変数 a は読み取り専用または定数であるため、上書きできません。
```


また、
Get-Variable では任意のレベルのスコープの変数にアクセスしたりできます。

```console
>  Get-Variable a –scope 1  # 親スコープから値を取得
>  $Get-Variable a –scope 2  # 祖父
```


その他、<strong id="remove_variable" class="keyword">Remove-Variable</strong> で変数を削除したりもできます。

```console
>  $a = 0
>  Remove-Variable a
>  Get-Variable a
Get-Variable : 名前 'a' の変数が見つかりません。
```
