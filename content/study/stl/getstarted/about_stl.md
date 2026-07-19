---
title: "STL とは"
source_url: "https://ufcpp.net/study/stl/getstarted/about_stl/"
content_type: "Article"
published_at: "2015-05-06T14:23:13"
updated_at: "2015-05-06T14:23:13"
tags: []
umbraco_id: 1628
parent_id: 1627
sort_order: 0
aliases:
  - "/stl/about_stl"
  - "/stl/about_stl.html"
  - "/stl/getstarted/about_stl/"
  - "/study/stl/about_stl"
  - "/study/stl/about_stl.html"
---

# STL とは

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

STL (Standard Template Library) とは、C++ の（1998年に標準化された）標準ライブラリの1つです。
STL は、template という機能を用いた直交性の高いライブラリです。


## <a id="sec-generated-title-2"></a> <a id="template"></a>template

<em>template</em>は型をパラメータとして与えることで、任意の型に対応したコンテナやアルゴリズムを記述できる機能です。
例えば、2つの値のうち大きいほうを取り出す関数<code>max()</code>を作りたいとします。<code>int</code>型に限定したものなら簡単に作れて以下のようになります。

```csharp
inline
int max(int x, int y)
{
  return x > y ? x : y;
}
```


しかし、<code>double</code>型や文字列に対して同じことをしたい場合、
改めて<code>double</code>型用のものと文字列用のものを作る必要があります。

templateを用いるとこの問題を解決できます。
template版の<code>max()</code>関数は以下のようになります。

```cpp
template<typename T>
inline
T max(const T& x, const T& y)
{
  return x > y ? x : y;
}
```


<code>template</code>というキーワードを用いてtemplate関数を定義します。
<code>template</code>に続く &lt;&gt; の中に、パラメータとなる型を書きます。
こうすることで、<code>int</code>型の変数を引数として呼び出せば<code>int</code>型に対応した<code>max()</code>関数が自動的に生成され、<code>double</code>型の変数を引数として呼び出せば<code>double</code>型に対応した<code>max()</code>関数が自動的に生成されます。

C/C++言語では、長い間このような汎用の機能を汎用ポインター(<code>void*</code>)や、マクロを使って行ってきました。
しかし、これらの機能は型安全性がなく、コンパイル時には発見できない潜在的なバグの原因になりやすいものです。
また、マクロにはデバッガによるデバッグを困難にするなどといった問題がありました。
しかし、templateを使うことで、型安全に汎用アルゴリズムを記述できるようになります。


## <a id="sec-generated-title-3"></a> <a id="orthogonal"></a>直交性

配列のように複数の値を一まとめにして管理するクラスを
<em>コンテナクラス</em>または<em>コレクションクラス</em>といいます。
コンテナクラスは、格納する型、格納する方式によってさまざまな種類があり、
整列、検索、置換などのさまざまな操作が考えられます。
以下に例を挙げてみます。

<table summary="">

	<tr>
		<th>格納する型</th>
		<td markdown="1"><code>int</code>型、<code>double</code>型、文字列型</td>
	</tr>
	<tr>
		<th>格納方式</th>
		<td markdown="1">配列、可変長配列、連結リスト</td>
	</tr>
	<tr>
		<th>操作</th>
		<td markdown="1">整列、検索、置換</td>
	</tr>
</table>


格納する型の種類が i 個、格納方式の数が j 個、操作の数が k 個あるとき、
これらのさまざまな種類のコンテナとその操作を個別に実装しようとすると、
全部で <em>i×j×k</em> 個のコードを書く必要があります。
それに対し、もし任意の型を格納できるコンテナがあり、任意の種類のコンテナを扱えるコンテナ操作関数があれば、<em>i＋j＋k</em> 個のコードを書くだけですみます。

前者は格納する型、格納方式、操作が相互に依存性を持っているため、i×j×k 個という大量のコードを書く必要があるわけです。
逆に、後者は格納する型、格納方式、操作に依存性がないため、i＋j＋k 個という少ないコードを書くだけですみます。
このようなコードの依存性のことを<em>直交性</em>といい、
依存性の少ないものを直交性が高いといいます。


## <a id="sec-generated-title-4"></a> <a id="stl"></a>STL

STLは、template機能を用いることで、直交性、汎用性が高く、型安全で高速なコンテナクラスおよびその操作を提供するライブラリです。

STLのコンテナクラスは <code>&gt;</code> などの演算子を適切に定義した任意の型を格納できます。
また、STLの規則に従って作ったコンテナクラスなら、任意のコンテナクラスがSTLの提供する操作関数を利用できます。

STL はその名前(Standard Template Library)が示すとおり、C++標準ライブラリに付属するライブラリです。
そのため、最新のC++の処理系には必ずSTLが付属しています。
ただし、STLが標準ライブラリとして採択されたのは1998年のことなので、古いC++処理系には付属していません。また、templateという機能自体が比較的新しいものなので、templateを完全に実装していない処理系も多々あります。
