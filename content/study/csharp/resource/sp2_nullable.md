---
title: "null許容値型(Nullable&lt;T&gt; 型)"
source_url: "https://ufcpp.net/study/csharp/resource/sp2_nullable/"
content_type: "Article"
published_at: "2015-05-06T14:10:51"
updated_at: "2019-07-27T16:47:03"
tags:
  - "Ver. 2.0"
umbraco_id: 1293
parent_id: 1286
sort_order: 10
aliases:
  - "/study/csharp/sp2_nullable.html"
---

# null許容値型(Nullable&lt;T&gt; 型)

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

通常、「[値型](oo_reference.md#valtype)」は `null` 値（無効な値）を取れません。
ところが、データベース等、一部のアプリケーションでは、
値型の通常の（有効な）値と `null`（無効な値）を取るような型が欲しいときがあります。
そこで、C# 2.0 では、null 許容型（Nullable 型）という特殊な型が用意されました。

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、[参照型](oo_reference.md#valtype)についても `?` の有無で null の可否を指定する機能が追加されました。
この機能を指して [null 許容参照型](nullablereferencetype.md)(nullable reference type)と言ったりします。

この null 許容参照型と区別する意味で、本項で説明している機能(C# 2.0 時代には唯一の null 許容型だった)を指して、null 許容値型(nullable value type)と呼ぶこともあります。

##### <a id="sec-generated-title-2"></a>ポイント

* 値型 T に対して、T? をいう書き方で null 許容型になります。

* null 許容型は、元となる値型の値か `null` を保持できる型です。


## <a id="sec-generated-title-3"></a> <a id="nullable"></a>null 許容型

<strong id="nullableType" class="keyword">null 許容型</strong>(nullable type)は、値型の型名の後ろに <code>?</code> を付ける事で、元の型の値または <code>null</code> の値を取れる型になるというものです。
<code>int</code> 型で例に取ると、以下のような書き方が出来ます。

```csharp {title="null 許容型の例"}
int? x = 123;
int? y = null;
```

### <a id="sec-generated-title-4"></a> <a id="non-nullable"></a>null 非許容型

(本項の意味、すなわち null 許容値型の場合)
null 許容型にできるのは「null 許容型を除く値型」のみです。

要するに、`int??` のように、「多重に null 許容」な型は作れないということです。
`int??` と書くとコンパイル エラーになります。

C#の仕様書上は、この「null 許容型を除く値型」を指して、null 非許容型(non-nullable type)と言ったりもします。日本語の場合は「null 非許容」よりも「非 null」とか書く方がわかりやすいかもしれません。

C# 8.0 以降では「null 非許容値型」や「非 null 値型」というように、値型であることを強調する呼び方もします。

### <a id="sec-generated-title-5"></a> <a id="nullable-reference"></a>null 許容参照型

C# 7.3 以前では、<code>string?</code> というのは定義できません（参照型には `?` を付けれない）。

C# 8.0 で、null 許容参照型と呼ばれる新しい機能が入って参照型でも `?` の有無で null の可否を指定できるようになりました。
ただ、「後入り」な機能なので、本項で説明している null 許容値型とは少し挙動が違ったりします。

詳しくは別項で説明予定です。

## <a id="sec-generated-title-6"></a> <a id="member"></a>null 許容型のメンバー

<code>T?</code> という書き方で得られる null 許容型は、
コンパイル結果的には、`Nullable<T>`構造体(`System`名前空間) と等価になります。
例えば、以下の2つの変数x と y は全く同じ型の変数になります。

```csharp {title="T? と Nullable&lt;T&gt;"}
int? x;
Nullable<int> y;
```

ちなみに、[リフレクション](../dynamic/sp_reflection.md#reflection)で型情報を取り出そうとした場合、null許容型は`Nullable<T>`構造体に見えます。

そして、この`Nullable<T>`構造体は、
`HasValue`という`bool`型のプロパティと、
`Value`という`T`型のプロパティを持っています。

<table summary="Nullable&lt;T&gt; 型のメンバー">
	<caption>
		Nullable&lt;T&gt; 型のメンバー
	</caption>
	<tr>
		<th>戻り値の型</th>
		<th>プロパティ名</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">`bool`</td>
		<td markdown="1">`HasValue`</td>
		<td markdown="1">有効な（`null` でない）値を持っていれば `true`、 値が `null` ならば `false` を返します。</td>
	</tr>
	<tr>
		<td markdown="1">`T`</td>
		<td markdown="1">`Value`</td>
		<td markdown="1">有効な値を返します。 もし、`HasValue` が `false`（値が `null`）だった場合、 例外 `InvalidOperationException` 投げます。</td>
	</tr>
</table>


また、<code>int? x = 123;</code> という書き方ができることから容易に想像が付くように、
<code>T?</code>型 と <code>T</code> 型の間には暗黙の型変換ができます。
<code>T</code> → <code>T?</code> の変換は常に可能で、
以下のようなコードの下2行は等価になります。

```csharp {title="int → int?"}
int? x;
x = 123;
x = new int?(123); // x = 123; と等価。
```


その逆、
<code>T?</code> → <code>T</code> の変換は、`HasValue` が `true` のときのみ可能で、
`HasValue` が `false` の時には `InvalidOperationException` がスローされます。

```csharp {title="int? → int"}
int? x = 123;
int? y = null;
int z;

z = (int)x; // OK。
z = (int)y; // 例外が発生。
```



## <a id="sec-generated-title-7"></a> <a id="operation"></a>null 許容型に対する演算

元となる型 <code>T</code> が持っている演算子は、
そのまま null 許容型 <code>T?</code> に対して利用できます。

<table summary="Nullable&lt;T&gt; 型に対する演算">
	<caption>
		Nullable&lt;T&gt; 型に対する演算
	</caption>
	<tr>
		<td markdown="1">単項演算</td>
		<td markdown="1"><code>+  ++  -  --  !  ~ </code></td>
		<td markdown="1">オペランドも計算結果も共に<code>T</code>型の単項演算子がある場合、<code>T?</code>に対してもその演算子を利用できます。<code>T?</code>型のオペランドが null の場合、計算結果も null になります。</td>
	</tr>
	<tr>
		<td markdown="1">二項演算</td>
		<td markdown="1"><code>+  -  *  /  %  &amp;  |  ^</code></td>
		<td markdown="1">（左右両方の）オペランドも計算結果も共に<code>T</code>型の二項演算子がある場合、<code>T?</code>に対してもその演算子を利用できます。<code>T?</code>型のオペランドのどちらか片方でも null だった場合、計算結果も null になります。 （ただし、bool 型に対する<code>&amp;</code>および<code>|</code>は例外で、 これらに関しては後述します。）</td>
	</tr>
	<tr>
		<td markdown="1">シフト演算</td>
		<td markdown="1"><code>&lt;&lt; &gt;&gt;</code></td>
		<td markdown="1">これらも二項演算と同様で、<code>T</code>型の演算子がある場合、<code>T?</code>に対してもその演算子を利用できます。 ただし、シフト演算ですので、右オペランドは int 型です。<code>T?</code>型の左オペランドが null だった場合、計算結果も null になります。</td>
	</tr>
	<tr>
		<td markdown="1">等値演算</td>
		<td markdown="1"><code>== !=</code></td>
		<td markdown="1"><code>T</code>型の等値演算がある場合、<code>T?</code>型の等値判定も可能です。<code>T?</code>型の オペランドが左右とも null の場合、比較結果は等しいと判定されます。 また、有効な（non-null の）値と null は等しくありません。 左右ともに有効な値の場合、<code>T</code>型の比較結果と同じになります。</td>
	</tr>
	<tr>
		<td markdown="1">関係演算</td>
		<td markdown="1"><code>&lt; &gt; &lt;= &gt;=</code></td>
		<td markdown="1"><code>T</code>型の比較演算がある場合、<code>T?</code>型の比較も可能です。<code>T?</code>型のオペランドのどちらか片方でも null だった場合、計算結果は false になります。 左右ともに有効な値の場合、<code>T</code>型の比較結果と同じになります。</td>
	</tr>
</table>


<code>bool?</code> 型に対する <code>&amp;</code> および <code>|</code> は以下のような結果になります。

<table summary="bool? に対する &amp;、|">
	<caption>
		bool? に対する &amp;、|
	</caption>
	<tr>
		<th>x</th>
		<th>y</th>
		<th>x &amp; y</th>
		<th>x | y</th>
	</tr>
	<tr>
		<td markdown="1">true</td>
		<td markdown="1">true</td>
		<td markdown="1">true</td>
		<td markdown="1">true</td>
	</tr>
	<tr>
		<td markdown="1">true</td>
		<td markdown="1">false</td>
		<td markdown="1">false</td>
		<td markdown="1">true</td>
	</tr>
	<tr>
		<td markdown="1">true</td>
		<td markdown="1">null</td>
		<td markdown="1">null</td>
		<td markdown="1">true</td>
	</tr>
	<tr>
		<td markdown="1">false</td>
		<td markdown="1">true</td>
		<td markdown="1">false</td>
		<td markdown="1">true</td>
	</tr>
	<tr>
		<td markdown="1">false</td>
		<td markdown="1">false</td>
		<td markdown="1">false</td>
		<td markdown="1">false</td>
	</tr>
	<tr>
		<td markdown="1">false</td>
		<td markdown="1">null</td>
		<td markdown="1">false</td>
		<td markdown="1">null</td>
	</tr>
	<tr>
		<td markdown="1">null</td>
		<td markdown="1">true</td>
		<td markdown="1">null</td>
		<td markdown="1">true</td>
	</tr>
	<tr>
		<td markdown="1">null</td>
		<td markdown="1">false</td>
		<td markdown="1">false</td>
		<td markdown="1">null</td>
	</tr>
	<tr>
		<td markdown="1">null</td>
		<td markdown="1">null</td>
		<td markdown="1">null</td>
		<td markdown="1">null</td>
	</tr>
</table>

## <a id="sec-generated-title-8"></a> <a id="coalescing"></a>null 合体演算子 (??)

null 許容型には、`??` 演算という特殊な演算子を使えます。
この`??`演算子は<strong id="nullableType" class="keyword">null合体演算子</strong><sup>※</sup>と呼ばれ、
値が <code>null</code> かどうかを判別し、<code>null</code> の場合には別の値を割り当てる演算子です。

```csharp {title="?? 演算子"}
// x, y は int? 型の変数
int? z = x ?? y; // x != null ? x : y
int i = z ?? -1; // z != null ? z.Value : -1
```

### <a id="sec-generated-title-9"></a> <a id="coalesce-translation"></a><sup>※</sup> coalesce

null合体演算子は、英語では null coalescing operator と言います。

coalesceという名前はSQLの同様の機能から来ているようです。SQLでも、「もし値がnullだったら、別の有効な値を返す」という機能を持ったCOALESCE関数というものがあります。

coalesceの元の英単語の意味は、合体・融合・癒着というような意味です。null coalescing operatorやCOALESCE関数の意味としては、「癒着」が一番近い気がします。SQLが由来ですので、歯抜け(テーブル中のnullの行 = 値が欠けている状態)をパテで埋めるようなイメージでしょうか。

### <a id="sec-generated-title-10"></a> <a id="null-coalescing-assignment"></a>null 合体代入 (??=)

C# 8.0 では、null合体演算子 (`??`)も複合代入に使えるようになりました(`??=`)。

例えば以下のような書き方ができます。

```csharp {title="null 合体代入" highlight-text="??="}
static void M(string s = null)
{
    s ??= "default string";
    Console.WriteLine(s);
}
```

意味としては、`if (s == null) s = ...;` と同じになります。[キャッシュ用途](rm_nullusage.md#cache)に便利だったりします。

### <a id="sec-generated-title-11"></a> <a id="assignment-result"></a>結果の型

C# では、代入や複合代入自体も式になっています。
なので、`var z = y += x;` みたいな感じでつないで掛けて、
`var z = (y += x);` という意味で評価されます。
この時、ほとんどの場合、`y += x` の部分の結果の型は `y` の型になります。

```csharp {title="複合代入の結果の型"}
byte x = 1;
byte y = 2;
var z = (y += x); // こう書くと y が byte なので z も byte に。
var w = y + x;    // この場合は int だったりする。C# の int 未満の整数の足し算結果は int になる。
```

この点に関して、null 合体代入は例外的な挙動をします。
というのも、`??` の最大の目的は「null だった時に何か有効な値に差し替える」というものなので、結果の型は非 null であってほしい場合がほとんどです。
なので、`y ??= x` の結果の型は `y` の側ではなく、`x` の側から推論されます。

```csharp {title="null 合体代入の戻り値は右辺から推論される"}
#nullable enable
string? s1 = null;
string s2 = s1 ??= ""; // s1 に ? が付いていても、s1 ??= "" の結果は string。
 
int? i1 = null;
int i2 = i1 ??= 0; // i1 に ? が付いていても、i1 ??= 0 の結果は int。
 
float? f1 = null;
float? f2 = null;
float? f3 = f2 ??= f1; // 右辺も null 許容なら結果の方も null 許容。
```

[キャッシュ用途](rm_nullusage.md#cache)で以下のような書き方をよくするため、こういう型決定ルールになっていないと使いにくくなります。

```csharp {title="??= の結果からは ? が消えてほしい"}
public T Property => _cache ??= GetValue();
private T? _cache;
 
private T GetValue()
{
    // 計算に時間がかかる処理
}
```
