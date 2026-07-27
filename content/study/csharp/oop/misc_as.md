---
title: "[雑記] キャストと as"
source_url: "https://ufcpp.net/study/csharp/oop/misc_as/"
content_type: "Article"
published_at: "2007-10-06T00:00:00"
updated_at: "2008-03-09T00:00:00"
tags: []
umbraco_id: 1265
parent_id: 1248
sort_order: 13
aliases:
  - "/study/csharp/misc_as.html"
---

# \[雑記\] キャストと as

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

キャスト演算子と as 演算子の実行速度に関しての話を少々。

ちなみに、
キャストと as には以下のような差もあります。

* as は変換なしで代入可能かどうかしか判定しない（ユーザ定義の型変換演算子は呼んでくれない）

* as は参照型（class）にしか使えない



## <a id="sec-generated-title-2"></a> <a id="compare"></a>キャストと as の実行速度

普通のキャストと as は、ちゃんと型変換できるなら得られる結果は一緒で、失敗時には、

* キャスト： InvalidCast 例外発生

* as: null を返す


という違いがあります。
（例外に関しては、「[例外処理](../structured/oo_exception.md)」参照。）

C# の例外処理機構は、
try catch を書くだけならほとんどコストはないんですが、
例外が throw された場合にはかなり重たい負荷がかかります。
なので、型変換に失敗する可能性があるときは as にする方がほぼ確実にパフォーマンスがよくなります。


##### <a id="sec-generated-title-3"></a>確実に型変換ができる場合

じゃあ、100％確実に型変換ができるとわかっている場合はどうでしょう。

ちなみに、キャスト演算子と as 演算子は、以下のような 「[IL](../abstract/ab_dotnet.md#il)」 にコンパイルされます。

* キャスト： castclass 命令

* as： isinst 命令


IL 上はどちらも1命令で、
命令の差でパフォーマンスの違い推測できないんで、ここは実測してみます。
for ループの中でキャストか as するだけの関数を書いて、

```csharp
Stopwatch sw = new Stopwatch();

sw.Reset();
sw.Start();
TestCast(N);
Console.Write("{0}\n", sw.ElapsedTicks);

sw.Reset();
sw.Start();
TestAs(N);
Console.Write("{0}\n", sw.ElapsedTicks);
```


として、時間を計ってみます。
結果、<em>キャスト演算の方が1割程度高速</em>でした。

（ちなみに、キャスト演算子を使って型変換に失敗した場合、
例外が発生するとパフォーマンスは2桁3桁余裕で悪化します。）


## <a id="sec-generated-title-4"></a> <a id="as_is"></a>as と is の実行速度

確実に型変換できる場合にキャストの方が早いなら、
以下のようなコードを書けば実行速度が速くなるかというと、
そんなことはない。

```csharp
B b = new D(); // D extends B

if (b is D)
{
  D d = (D)b;
}
```


なぜかというと、is は、内部的には as とまったく同じコードになるから。
以下のような2つのコードがほぼ同じコンパイル結果になります。

```csharp
B b = new D(); // D extends B
D d = b as D;
if (d != null)
  ...
```


```csharp
B b = new D(); // D extends B
if (b is D)
  ...
```


要するに、is 演算子は as ＋ null 比較相当のコードになります。
その結果、「isで型を調べてからキャスト」は単に2度手間なだけで、遅くなります。
