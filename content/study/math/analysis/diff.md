---
title: "常微分方程式"
source_url: "https://ufcpp.net/study/math/analysis/diff/"
content_type: "Article"
published_at: "2015-05-06T14:16:39"
updated_at: "2015-05-06T14:16:39"
tags: []
umbraco_id: 1465
parent_id: 1464
sort_order: 0
aliases:
  - "/analysis/diff"
  - "/analysis/diff.html"
  - "/math/analysis/diff/"
  - "/study/analysis/diff"
  - "/study/analysis/diff.html"
---

# 常微分方程式

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ひとくちに微分方程式といってもさまざまな形のものがあります。
一般的には、微分方程式は解析的に解を求めることができませんが、
特定のパターンの場合に限り解析解を求めることが出来ます。

微分方程式に関する数学分野の1つに、
微分方程式がどういうパターンの時に、
どういう方法で解析解を計算できるのかというものがあります。


## <a id="sec-generated-title-2"></a> <a id="plan"></a>執筆予定

```text
・まず、具体例を
力学の法則： ma = f
  速度抵抗
  単振動
  中心力： 重力、惑星の運動
放射性原子の崩壊： 原子の個数に比例して崩壊
電気回路


・解ける常微分方程式
- 変数分離形
- 定数係数線形
  - 基礎： 2階で斉次の場合 → 「[2階常微分方程式](/study/math/analysis/diffsecond)」
  - 一般論： 「[定数係数線形微分方程式](/study/math/analysis/difflinear)」
- 全微分形

↑の場合には確実に解けることがわかってる。

これ以外の場合でも、変数変換とかいろいろ式変形をすれば
↑のどれかの形に帰着できるものも多い。

・級数解法

・数値解法
解けない場合も多い、というか、方程式の形が複雑になるとまず解けない
→ コンピュータを使った近似解
```
もっとも単純な微分方程式について説明しつつ、いくつか用語の説明。

微分方程式の中で最も簡単なものというと、
間違いなく1変数定係数1階線形斉次微分方程式でしょう。
1変数定係数1階線形微斉次分方程式というのは、要するに、
<div class="math">
      <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> a x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">0</span>
    </div>
という形の微分方程式です。
言葉の意味は以下の通りです。

* 1変数 … 名前どおり、独立変数が1つだけ。（<span class="math">t</span>のみ。）

* 1階 … 方程式中に1次微分（<span class="math">
          <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </span>）までしかでてこない。

* 線形 …<span class="math">
          x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </span>に関して線形。（<span class="math">
          x<sup><span class="normal">2</span></sup>
        </span>とかが出てこない。）

* 定係数 …<span class="math">
          x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </span>の前に掛かっている係数<span class="math">a</span>が定数。

* 斉次 … 0次の項がない。（<span class="math">
          x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>, <table class="frac" summary="differential"><tr><td class="num"><span class="normal">d</span></td></tr><tr><td><span class="normal">d</span>t</td></tr></table>x<span class="paren" style="font-size:em;">(</span>t<span class="paren" style="font-size:em;">)</span>
        </span>の項しかない。）


このような微分方程式は、
速度抵抗を受ける物体の運動や、
抵抗とコンデンサのみからなる電気回路（RC回路）の電圧変化などに現れます。

この形の微分方程式は、変数分離系というパターンで解くことができて、
解は以下のようになります。
<div class="math">
      <span class="normal">−</span>a <span class="normal">d</span>t ＝ <table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table><span class="normal">d</span>x
    </div><div class="math">
      <span class="normal">−</span>a <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><span class="normal">d</span>t ＝ <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table><table class="frac" summary="fraction"><tr><td class="num"><span class="normal">1</span></td></tr><tr><td>x</td></tr></table><span class="normal">d</span>x
    </div><div class="math">
      <span class="normal">−</span>a t ＝ <span class="normal">log</span>x <span class="normal">+</span> C
    </div><div class="math">
      x ＝ A <span class="normal">e</span><sup><span class="normal">−</span>a t</sup>
    </div>
ただし、途中から現れる <span class="math">C, A</span> は積分定数です。
