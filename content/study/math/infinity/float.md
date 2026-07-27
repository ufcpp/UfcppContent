---
title: "計算機上では"
source_url: "https://ufcpp.net/study/math/infinity/float/"
content_type: "Article"
published_at: "2015-05-06T14:18:07"
updated_at: "2015-05-06T14:18:07"
tags: []
umbraco_id: 1508
parent_id: 1500
sort_order: 7
aliases:
  - "/study/infinity/float.html"
---

# 計算機上では

## <a id="sec-generated-title-1"></a> <a id="d74e4"></a>計算機上での数値の扱い

```text
計算机上では実数はおろか、整数ですら有限桁しか扱えない。
実数は浮動小数点数で表現 … 精度も桁数も有限
```

## <a id="sec-generated-title-2"></a> <a id="d74e10"></a>浮動小数点数における∞の定義

```text
計算機上では、

表現可能範囲が有限→あまりに大きな数は表現不能
→表現不能な大きさの数を全て∞扱いしてしまう
精度の問題から、非常に小さな数と0は区別が付かない
→非常に大きな数と∞も区別を付けない方が都合がいい

↓

計算途中でオーバーフローを起こした場合、∞に
1/0 → ∞に
1/∞→ 0 に
a+∞→ ∞
∞/∞ → NaN

NaN（Not A Number）… 存在しない数、
計算机上では扱えない数を表す特別な浮動小数点数。
実数計算ライブラリでは √(-1) なども NaN になるのが普通。

ちなみに、整数型の場合は 1/0 はエラーを起こす。
```
