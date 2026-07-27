---
title: "周波数変換"
source_url: "https://ufcpp.net/study/sp/digital_filter/transform/"
content_type: "Article"
published_at: "2004-12-23T00:00:00"
updated_at: "2015-07-07T18:57:28"
tags: []
umbraco_id: 1615
parent_id: 1610
sort_order: 4
aliases:
  - "/study/digital_filter/transform.html"
---

# 周波数変換

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

カットオフ周波数 1 のローパスフィルタを設計すれば、
ある変換ルールを用いて任意のローパス・ハイパス・バンドパス・バンドストップフィルタを設計できます。
 
また、アナログ的手法で設計したフィルタをディジタル化
（アナログ伝達関数をディジタル伝達関数に変換）することができます。


## <a id="sec-generated-title-2"></a> <a id="analog"></a>アナログ→アナログ

執筆予定
```text
・アナログ

カットオフ周波数 1 のローパスから、

カットオフ周波数 ω<sub>0</sub> のローパス
s → s / ω<sub>0</sub>
↑
これは、区間 (－ω0, ω0) を (－1, 1) に移す写像。

カットオフ周波数 ω<sub>0</sub> のハイパス
s → ω<sub>0</sub> / s
↑
これは、区間 (－∞, －ω0), (ω0, ∞) を (－1, 1) に移す写像。

中心周波数 ω<sub>0</sub>、バンド幅 Δω のバンドパス
s → Δω s / (s<sup>2</sup> ＋ ω<sub>0</sub><sup>2</sup>)
↑
これは、区間 (ω0－Δω/2, ω0＋Δω/2) を (－1, 1) に移す写像。

中心周波数 ω<sub>0</sub>、バンド幅 Δω のバンドストップ
s → (s<sup>2</sup> ＋ ω<sub>0</sub><sup>2</sup>) / (Δω s)


・双2次バンドパスフィルタの零・極

双2次フィルタの一般形は

s<sup>2</sup> ＋ c s ＋ d
-------------------------
s<sup>2</sup> ＋ a s ＋ b

分母分子共に、共役複素解を持つものとして、

(s － β) (s － β<sup>*</sup>)
--------------------
(s － α) (s － α<sup>*</sup>)

これを元に、バンドパスフィルタ化するために、
s → Δω s / (s<sup>2</sup> ＋ ω<sub>0</sub><sup>2</sup>)
で変数変換すると、
極は以下のように変換される。

α ± √(α<sup>2</sup> － (ω<sub>0</sub> / Δω)<sup>2</sup>)
----------------------- ,
2 Δω

α<sup>*</sup> ± √(α<sup>*</sup><sup>2</sup> － (ω<sub>0</sub> / Δω)<sup>2</sup>)
-----------------------
2 Δω


α ＋ √(α<sup>2</sup> － (ω<sub>0</sub> / Δω)<sup>2</sup>)
-----------------------
2 Δω
と
α<sup>*</sup> － √(α<sup>*</sup><sup>2</sup> － (ω<sub>0</sub> / Δω)<sup>2</sup>)
-----------------------
2 Δω
が互いに共役。

元々の極 α を <span class="math">α ＝ a ＋ ib</span> で表すと、
変換後の極 α' は

α' ＝ a ＋ ib ± √(a<sup>2</sup> － b<sup>2</sup> － ω<sup>2</sup> ＋ i2ab)
       -----------------------------------
       2Δω
（ω ＝ ω<sub>0</sub>/Δω）

√(a<sup>2</sup> － b<sup>2</sup> － ω<sup>2</sup> ＋ i2ab) の部分を c ＋ id、
D ＝ a<sup>2</sup> － b<sup>2</sup> － ω<sup>2</sup> と置くと、

c ＝ √(√(D<sup>2</sup> ＋ 4) ＋ D)
d ＝ √(√(D<sup>2</sup> ＋ 4) － D)
```

## <a id="sec-generated-title-3"></a> <a id="sz"></a>アナログ→ディジタル

執筆予定
```text
インパルス不変法
双一次変換
双一次変換は別ページで詳しく（双1次変換 bilineartrans.xml ）。
```
