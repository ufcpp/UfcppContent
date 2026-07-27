---
title: "多項式環"
source_url: "https://ufcpp.net/study/math/infinity/polynomial/"
content_type: "Article"
published_at: "2015-05-06T14:18:01"
updated_at: "2015-05-06T14:18:01"
tags: []
umbraco_id: 1505
parent_id: 1500
sort_order: 4
aliases:
  - "/study/infinity/polynomial.html"
---

# 多項式環

## <a id="sec-generated-title-1"></a> <a id="d71e4"></a>多項式環

```text
f[x] ＝ Σ a_n x^n

などは環をなす。
```

## <a id="sec-generated-title-2"></a> <a id="d71e7"></a>多項式環の順序

```text
実数係数の多項式同士の順序関係を以下のように定義

・2つの多項式 f[x] と g[x] の次数が異なる場合、
  次数の大きい方が大きいものとする。
・高次のものから順に係数を比較し、1番最初に値の異なるものを比較し、
  値の大きい方が大きいものとする。

C#言語的に書くと、

	if(f.Order ＞ g.Order) return 1;
	if(f.Order ＜ g.Order) return -1;

	for(int n=f.Order; n＞0; --n)
		if(f[n] != g[n]) break;

	return f[n].CompareTo(g[n]);

こういう順序関係を「辞書式順序」という。

辞書式順序を導入すると、

任意の実数 ＜ x
任意の実数×x ＜ x^2
a ＜ b → ax ＜ bx

と言うような関係が成り立つ。
「任意の実数 ＜ x」と言う性質から、x を∞という。



これと同様に、無限小も作れる。
順序関係を、

// n ＞ f.Order のとき、f[n] == 0、n ＞ g.Order のとき、g[n] == 0 を返すものとする。
for(int n=0; n≦f.Order ＆＆ n≦g.Order; ++n)
{
	if(f[n] ＞ g[n]) return 1;
	if(f[n] ＜ g[n]) return -1;
}

にすると、「任意の正の実数 ＞ x」
```
