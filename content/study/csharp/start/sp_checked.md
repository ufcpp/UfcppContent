---
title: "オーバーフローのチェック"
source_url: "https://ufcpp.net/study/csharp/start/sp_checked/"
content_type: "Article"
published_at: "2015-05-06T14:08:04"
updated_at: "2021-09-22T16:44:12"
tags: []
umbraco_id: 1213
parent_id: 1190
sort_order: 13
aliases:
  - "/csharp/sp_checked"
  - "/csharp/sp_checked.html"
  - "/csharp/start/sp_checked/"
  - "/study/csharp/sp_checked"
  - "/study/csharp/sp_checked.html"
---

# オーバーフローのチェック

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C#では<code>checked, unchecked</code>というキーワードを用いることで、
数値演算のオーバーフローをチェックするかどうかを明示的に選択することが出来ます。


##### <a id="sec-generated-title-2"></a>ポイント

* checked (式)： 式中でオーバーフローが発生したら例外を投げます。

* unchecked (式)： 式中で起きたオーバーフローはすべて無視します。

* C# のデフォルトでは unchecked と同じ状態。 コンパイラオプションで checked な状態に変更できます。



## <a id="sec-generated-title-3"></a> <a id="overflow"></a>オーバーフロー

「[組込み型](st_embeddedtype.md)」で説明したとおり、
コンピュータの内部で扱える値の範囲は限られています。
そのため、計算を行っている途中で計算結果がこの範囲を超えてしまうことがあり、
このような状況を<strong id="overflow" class="keyword">オーバーフロー</strong>（overflow）と言います。

計算の途中でオーバーフローが起こると、計算結果が大幅に狂うことになります。
例えば、以下のようなプログラムを実行すると、
「正の数同士を足し合わせているのに結果が負の数になる」という症状を引き起こします。

```csharp
sbyte a = 64;           // 2進表現では 0100 0000。
sbyte b = 65;           // 2進表現では 0100 0001。
sbyte c = (sbyte)(a + b); // 2進表現では 1000 0001 ←これは sbyte では -127 を表す。
Console.Write("{0} + {1} = {2}", a, b, c); // 64 + 65 = -127 と表示される。
```



## <a id="sec-generated-title-4"></a> <a id="checked"></a>checked キーワード

オーバーフローを起こされるとまずい場合、
何らかの方法でオーバーフローを検出する必要があります。
C#では、コンパイル時に <code>/checked+</code> というオプションを付けることで、
オーバーフローを起こしたときに例外をスローするようになります。
しかし、この方法では、特定の箇所でのみオーバーフローのチェックを行うということが出来ません。

そこで、コードの特定の箇所でのみオーバーフローのチェックを行うために、<code>checked</code> というキーワードが用意されています。
<code>checked</code> キーワードは以下のようにして使用します。

```csharp
checked
  ブロック
```


または、

```csharp
checked(式)
```


前者の書き方をchecked文、後者の書き方をchecked演算子と呼びます。
checked文中の式や、
checked演算子の後の式の中でオーバーフローが起きた場合、
<code>System.OverflowException</code> 例外が発生します。

```csharp
using System;

class CheckedSample
{
  static void Main()
  {
    try
    {
      checked
      {
        sbyte a = 64;
        sbyte b = 65;
        sbyte c = (sbyte)(a + b);
      }
    }
    catch(OverflowException ex)
    {
      Console.Write(ex.Message);
    }
  }
}
```


```console
演算操作の結果オーバーフローが発生しました。
```



## <a id="sec-generated-title-5"></a> <a id="unchecked"></a>unchecked キーワード

checked のときとは逆に、オーバーフローをあえて無視したい場合もあります。
そのため、<code>unchecked</code> というキーワードも用意されています。
<code>unchecked</code> キーワードは以下のようにして使用します。

```csharp
unchecked
  ブロック
```


または、

```csharp
unchecked(式)
```


使い方はcheckedと同様で、
unchecked文とunchecked演算子があります。

unchecked文中の式や、
unchecked演算子の後の式の中ではオーバーフローは無視され、
例外はスローされません。

```csharp
using System;

/// <summary>
/// 線形合同法っていう古典的な擬似乱数生成手法を使った乱数生成クラス。
/// </summary>
class Random
{
  uint seed;

  public Random(uint seed)
  {
    this.seed = seed;
  }

  public long Next()
  {
    seed = unchecked(seed * 1664525  + 1013904223);
    // ↑ここであえてオーバーフローは無視する。
    // 計算結果は (seed * 1664525  + 1013904223) を2の32乗で割ったあまりになる。
    return seed;
  }
}
```



## <a id="sec-generated-title-6"></a> <a id="float"></a>浮動小数点数型の場合のオーバーフロー

浮動小数点数型の場合、オーバーフローを起こした場合、値は無限大(infinity)になります。
また、絶対値が浮動小数点数で表せる値の範囲を下回った場合(このような状況を<em>アンダーフロー</em>と呼ぶ)、
値は0になります。

```csharp
float x = 1e30f;
float y = 1e-30f;
Console.Write("{0}, {1}", x*x, y*y); // +∞, 0 と表示される。
```


浮動小数点数の場合、例えchecked文中であっても、
オーバーフローによる例外は発生しません。
