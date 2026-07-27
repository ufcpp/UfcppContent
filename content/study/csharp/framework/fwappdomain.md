---
title: "AppDomain"
source_url: "https://ufcpp.net/study/csharp/framework/fwappdomain/"
content_type: "Article"
published_at: "2013-05-06T00:00:00"
updated_at: "2015-11-19T14:14:36"
tags: []
umbraco_id: 1348
parent_id: 1344
sort_order: 4
aliases:
  - "/study/csharp/FwAppDomain.html"
---

# AppDomain

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（書きかけ）

第三者コード（プラグインとか）が勝手なメモリ アクセスできないように。

明確にドメイン（domain: 領地、領域、占有権）を分ける。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>書く予定

ネイティブのセキュリティ。
メモリ アドレス空間がプロセスごとに違ったり
OS がいろいろ保護してる代わりにプロセス立てるの重たい。

AppDomain
ソフトウェア的なメモリ分離。
1つのプロセス内に複数の AppDomain を作れる。
他の AppDomain 中のメモリに直接触れる手段ない。

マーシャリング（marshal: 指揮官を立てて整列したり入場案内したり）
つまり、AppDomain 間はユーザー コードが勝手に行き来できない。
必ずフレームワーク側の干渉がかかる。

マーシャリングのされ方には2種類あって、

* 値によるマーシャリング（marshal by value）: シリアル化/逆シリアル化を経て、コピーを渡す。 Serializable 属性を付けたり、ISerializable インターフェイスを実装したりして、シリアル化可能にしておく必要あり。

* 参照によるマーシャリング（marshal by reference）: 値のコピーは作らないけど、透過プロキシってのが作られて、AppDomain 間の通信みたいなのが発生する。 MarshalByRefObject っていう特別なクラスを継承すると、こっち扱いされる。


このどっちか（シリアル化可能に作るか、MarshalByRefObject を継承するか）しないと、マーシャリングできない。

その他、AppDomain ごとに別のセキュリティ ポリシーを設定できたり。
