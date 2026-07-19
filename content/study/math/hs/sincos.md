---
title: "三角関数"
source_url: "https://ufcpp.net/study/math/hs/sincos/"
content_type: "Article"
published_at: "2015-05-06T14:16:19"
updated_at: "2015-05-06T14:16:19"
tags: []
umbraco_id: 1454
parent_id: 1445
sort_order: 8
aliases:
  - "/hs/sincos"
  - "/hs/sincos.html"
  - "/math/hs/sincos/"
  - "/study/hs/sincos"
  - "/study/hs/sincos.html"
---

# 三角関数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

教科書に公式が大量に並んでる割には、
丸暗記が必要なことはほとんどない、
実に素敵な単元。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>執筆予定

<pre>
　　　　／|
　１　／　|
　　／　　| sin
　／　　　|
∠＿＿＿＿|
   cos
って絵と、
tan = sin/cos
と、
(cos a + i sin a)(cos b + i sin b)
= cos(a+b) + i sin(a+b)
だけ覚えてれば何とかなる。

もちろん、他の細々した公式も覚えてるに越したことはないけど、
やろうと思えば全部この3つだけから導出可能。

細々としたものは忘れてしまいやすいけど、
このたった3つの単純な事柄と、導出手順だけならほぼ忘れない。


ド・モアブルの定理の
(cos a + i sin a)(cos b + i sin b)
= cos(a+b) + i sin(a+b)
という式は、三角関数を習う時点では出てこないものだけど、
「一度先に進んでから振り返ると見通しがいい」の典型。

まあ、証明手順的には、
幾何学的に加法定理を証明するのが先で、
それを使ってこのド・モアブルの定理を証明するんだけど、
公式を覚える上では順番逆の方が覚えることが少なくて楽。


あと、
(cos a + i sin a)(cos b + i sin b)
= cos(a+b) + i sin(a+b)
という公式、指数法則と一緒よね。

(exp ax)' = a exp ax
と、
(cos x + i sin x)' = i (cos x + i sin x)
を見比べてみよう。
cos x + i sin x == exp ix
が想像できる。

↑
大学で「テイラー展開」を習うと、これが本当に一致していることが分かります。
あと、「微分方程式」の解法を習ってもこの公式にたどり着くことができます。

|αβ| ＝ |α| |β|
arg(αβ) ＝ argα ＋ argβ
なわけだけど、
これも、|αβ| ＝ |α| |β| を両辺対数取ると、
log|αβ| ＝ log|α| ＋ log|β|
で、
log|x| と arg x に共通性あり。
実際、
log α ＝ log|α| ＋ i argα
なぜならば、
α ＝ |α| (cos(argα) + i sin(argα))
   ＝ |α| exp (i argα)
   ＝ exp log|α| × exp (i argα)
   ＝ exp(log|α| ＋ i argα)
</pre>
