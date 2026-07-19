---
title: "“Safe C” としての C#"
source_url: "https://ufcpp.net/study/csharp/misc/ap_safec/"
content_type: "Article"
published_at: "2013-11-10T00:00:00"
updated_at: "2024-08-31T17:24:47"
tags: []
umbraco_id: 1341
parent_id: 1338
sort_order: 3
aliases:
  - "/csharp/ap_safec"
  - "/csharp/ap_safec.html"
  - "/csharp/misc/ap_safec/"
  - "/study/csharp/ap_safec"
  - "/study/csharp/ap_safec.html"
---

# “Safe C” としての C#

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

(書きかけ)

C# は、企画段階では “Safe C” (安全な C 言語)と呼ばれていた時期もあるそうです。
プログラマーが陥りがちなミスを避けるため、間違いそうな文法を避けたり、細かな規則を設けています。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>予定

```text
http://www.slideshare.net/Coverity/the-psychology-of-c-analysis-24025354

switch fall through
    これはでも、今の感覚からすると「breakを書かなくてもcaseをまたがない」にした方が好感度高そう。
    大体の人は「いちいちbreak書くのめんどくさい」って言うし。
    実際、最近の言語でbreak書く言語あまり見ないし。
    この辺りはC言語からの負の遺産。
    
unfase 制限
    
実装依存をなくす
x = ++x + x++; とかの、term の評価順
配列の境界外アクセス禁止、lengthを自分で知ってる
    
{
    {
        var x;
    }
    var x; // error
}
    
未初期化変数禁止、フィールド/配列の0クリア
    non-null 参照の考え方と相性悪いけど

数値系の暗黙的変換なし
    ポインター ⇔ 整数 ⇔ char, bool, enum
    符号

x + 1; みたいな、何も起こさない式だけを認めない

if (x = 0) エラー

return してないパスを認めない
```
