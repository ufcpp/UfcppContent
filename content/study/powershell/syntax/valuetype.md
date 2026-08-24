---
title: "値型"
source_url: "https://ufcpp.net/study/powershell/syntax/valuetype/"
content_type: "Article"
published_at: "2007-05-20T00:00:00"
updated_at: "2015-05-06T14:20:53"
tags: []
umbraco_id: 1581
parent_id: 1577
sort_order: 3
aliases:
  - "/study/powershell/valuetype.html"
---

# 値型

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

ここでは、整数、小数、論理値などの値型について説明します。


## <a id="sec-generated-title-2"></a> <a id="numeric"></a>数値と算術演算

### <a id="sec-generated-title-3"></a> <a id="numeric_literal"></a>数値リテラル

とりあえず、
整数は System.Int32
実数は System.Double になるっぽいです。

```console {title="数値の型"}
>  (1).GetType().FullName
System.Int32
>  (1.1).GetType().FullName
System.Double
```


整数は、通常はもちろん10進数で書きますが、
0x を頭に付けると16進数になります。

```console {title="16進数リテラル"}
>  0xff
255
```


整数値なんだけど double で保持したい場合、
末尾に小数点を打って、
1. とすれば OK。
小数点の後ろの 0 は省略可能。

```console {title="double 型リテラル"}
>  (1.).GetType().FullName
System.Double
```


double は科学表記も可能です。
（例えば、1.234e6 で <span class="math">
          <span class="normal">1.234</span>
          <span class="normal">×</span>
          <span class="normal">10</span>
          <sup><span class="normal">6</span></sup>
        </span> の意味になる。）

```console {title="double の科学表記"}
>  1.234e6
1234000
```


それから、1l とか 1L（語尾に l, L）で long になります。

```console {title="long 型リテラル"}
>  (1L).GetType().FullName
System.Int64
```


あと、
キロバイト（<span class="math">
          <span class="normal">2</span>
          <sup>
            <span class="normal">10</span>
          </sup>
        </span> = 1024）、
メガバイト（<span class="math">
          <span class="normal">2</span>
          <sup>
            <span class="normal">20</span>
          </sup>
        </span> = 1048576）、
ギガバイト（<span class="math">
          <span class="normal">2</span>
          <sup>
            <span class="normal">30</span>
          </sup>
        </span> = 1073741824）
を表す 1kb, 1mb, 1gb という特殊な定数もあります。

```console {title="バイト定数"}
>  1kb
1024
>  1mb
1048576
>  1gb
1073741824
```



### <a id="sec-generated-title-4"></a> <a id="numeric_add"></a>加減乗除

+ - * / % で、それぞれ加算、減算、乗算、除算、剰余演算になります。
また、
++ -- でインクリメント、デクリメントもできます。
この辺りは C 言語や C# と同じです。
++ と -- に後置きと前置きがある辺りも同じです。

```console {title="+ - * / %"}
>  5 + 2
7
>  5 - 2
3
>  5 * 2
10
>  5 / 2
2.5
>  5 % 2
1
```


```console {title="++ --"}
>  $a = 1
>  $b = ++$a
>  $a,$b
2
2
>  $a = 1
>  $b = $a++
>  $a,$b
2
1
```


ここで、割り算結果の型に関してですが、
整数÷整数は、割り切れるときは int のまま、
割り切れないときは勝手に double になるようです。
まあ、流石に 2.5 * 2 が整数にはなったりはしません。

```console {title="割り算結果の型"}
>  (5 / 2).GetType().Name
Double
>  (4 / 2).GetType().Name
Int32
>  (5 / 2 * 2).GetType().Name
Double
```


あと、int の演算の結果が int で表現できる範囲を超えた場合、
勝手に double に変換されるようです。
（long なら表現できる桁であっても、double になる。）

```console {title="桁あふれ"}
>  1gb * 1gb
1.15292150460685E+18
>  [long]1gb * 1gb
1152921504606846976
>  (1gb * 1gb).GetType().Name
Double
>  ([long]1gb * 1gb).GetType().Name
Int64
```


ちなみに、
double → int への変換時には、四捨五入されます。
（挙動は System.Math.Round() メソッドと同じで、
いわゆる偶数丸め（round to even）。）

```console {title="double → int"}
>  [int]1.49
1
>  [int]1.5
2
>  [int]2.49
2
>  [int]2.5
2
>  [int]2.51
3
```



### <a id="sec-generated-title-5"></a> <a id="numeric_comp"></a>比較

比較演算子は
-eq
-ne
-lt
-gt
-le
-ge
となっています。
まあ、一応説明すると、前から順に、
equal, not equal, less than, greater than, less or equal, greater or equal
です。
C 系統の言語でいうところの、
==, !=, &lt;, &gt;, &lt;=, &gt;=
なんですが、
PowerShell では &lt; とかが他に特別な意味を持つので、
このような形になっています。

```console {title="比較演算子"}
>  $a = 1
>  if($a -eq 1) {echo '$a equals 1'}
$a equals 1
```



### <a id="sec-generated-title-6"></a> <a id="numeric_bitwise"></a>ビットごとの論理演算

ビットごとの論理演算もできて、
-bnot -band -bor -bxor
がそれぞれ、
ビット反転、ビットごとの AND、ビットごとの OR、ビットごとの XOR です。

```console {title="ビットごとの論理演算"}
>  1 -bor 2 -bor 3 -bor 4
7
>  0x1234 -band 0xff
52
>  0x0034
52
```


-and や -or 演算子（論理値の演算子）との違いに注意してください。
（
整数から論理値への自動型変換があるので 1 -and 2 とかの式もかけますが、
結果は $true -and $true、つまり $true になります。
）


## <a id="sec-generated-title-7"></a> <a id="bool"></a>論理値

整数が System.Int32、小数が System.Double になるのと同様に、
-eq 演算などの結果は System.Boolean になります（bool という名前でも参照可能）。

で、論理値の真と偽を表すための特殊な変数
$true, $false があります。

あと、PowerShell では、数値や文字列から論理値への自動型変換があります。
変換ルールは以下の通り。

* 数値: 非 0 なら真

* 文字列: "False" という文字列は偽扱い。"False" を除いて、長さ＞0 の文字列は真。

* 配列: 長さが 2 以上なら真。長さ1の場合は中身によって真偽を判断。長さ 0 の配列は偽。

* 任意のオブジェクト参照: null 値なら偽。



### <a id="sec-generated-title-8"></a> <a id="bool_operator"></a>論理演算

論理値用の演算子として、
! -not -and -or の4つがあります。
このうち、! と -not は同じ意味です。
-not -and -or は、まあ、名前どおり、論理否定、論理 AND、論理 OR です。

```console {title="論理演算"}
>  $true -and $true
True
>  $true -and $false
False
>  $false -and $false
False
>   $true -or $true
True
>   $true -or $false
True
>   $false -or $false
False
>  -not $true
False
>  -not $false
True
```


PowerShell では、他の型から bool への自動型変換があるので、
整数のビットごとの AND （-band）などのつもりで -and と書いてしまわないように注意してください。
