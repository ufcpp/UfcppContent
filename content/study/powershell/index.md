---
title: "Windows PowerShell"
source_url: "https://ufcpp.net/study/powershell/"
content_type: "Subject"
published_at: "2015-05-06T14:20:28"
updated_at: "2015-05-06T14:37:48"
tags: []
umbraco_id: 1572
parent_id: 1115
sort_order: 3
aliases:
  - "/powershell/"
  - "/powershell/index"
  - "/powershell/index.html"
  - "/study/powershell/index"
  - "/study/powershell/index.html"
---

# Windows PowerShell

Windows Power Shell は、
Windows をコマンドラインから、あるいは、
スクリプトを使って管理するために作られた新しいシェル環境です。

    
作られた目的のせいか、
Power Shell を解説する書籍・ウェブサイトには、
「管理者向けの TIPS、実例集」
（レジストリの値の変え方とか、WMI の呼び出し方とか）
みたいな物が多かったりします。

    
でも、Power Shell を調べてみた感じ、
Power Shell 用のスクリプト言語や、
コマンドレットの動作・作り方など、
プログラミング的にも結構面白そうな感じなので、
その辺りを中心に話をしてみようかと思います。

    
（注：
コマンドの実行結果など、
ページ表示の収まりをよくするため、
ところどころ省略している部分があります。
）

## 章

### <a id="intro"></a>[前置き](intro/index.md)

- [Windows PowerShell 概要](intro/abstract.md)
- [スクリプト言語とは](intro/scriptlang.md)
- [準備](intro/prepare.md)

### <a id="syntax"></a>[PowerShel の構文](syntax/index.md)

- [基礎知識](syntax/basic.md)
- [変数](syntax/variable.md)
- [.NET Framework オブジェクト](syntax/dotnet.md)
- [値型](syntax/valuetype.md)
- [文字列](syntax/string.md)
- [配列](syntax/array.md)
- [XML](syntax/xml.md)
- [制御構文](syntax/flow.md)
- [関数、フィルタ、スクリプト](syntax/function.md)
- [例外処理](syntax/exception.md)

### <a id="cmdlet"></a>[Cmdlet](cmdlet/index.md)

- [コマンド実行](cmdlet/cmdlet.md)
- [変数操作関連の Cmdlet](cmdlet/cmd_variable.md)
- [オブジェクト操作 Cmdlet](cmdlet/cmd_object.md)

### <a id="interop"></a>[.NET 言語からの利用](interop/index.md)

- [C# 上で PowerShell スクリプトを実行](interop/interop.md)

### <a id="misc"></a>[その他](misc/index.md)

- [プロバイダとドライブ](misc/psdrive.md)
- [環境のカスタマイズ](misc/customize.md)
- [重要語句・Cmdlet 一覧](misc/keywords.md)
