---
title: "C++とJavaとの組込み型の比較"
source_url: "https://ufcpp.net/study/csharp/cs4j/cmp_type/"
content_type: "Article"
published_at: "2015-05-06T14:13:37"
updated_at: "2015-05-06T14:13:37"
tags: []
umbraco_id: 1375
parent_id: 1372
sort_order: 2
aliases:
  - "/csharp/cmp_type"
  - "/csharp/cmp_type.html"
  - "/csharp/cs4j/cmp_type/"
  - "/study/csharp/cmp_type"
  - "/study/csharp/cmp_type.html"
---

# C++とJavaとの組込み型の比較

##<a id="sec-generated-title-1"></a> <a id="comp"></a>組込み型の比較
以下にC++とJavaとC#の組込み型の一覧を列挙します。

<table summary="">

	<tr>
		<th colspan="2">種類</th>
		<th>C++</th>
		<th>Java</th>
		<th>C#</th>
	</tr>
	<tr>
		<th colspan="2">論理型</th>
		<td markdown="1">bool</td>
		<td markdown="1">boolean</td>
		<td markdown="1">bool</td>
	</tr>
	<tr>
		<th rowspan="4">符号付き<br></br>整数</th>
		<th>1byte</th>
		<td markdown="1">signed char<sup>
            [*1](#a)
          </sup></td>
		<td markdown="1">byte</td>
		<td markdown="1">sbyte</td>
	</tr>
	<tr>
		<th>2byte</th>
		<td markdown="1">short, int<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1">short</td>
		<td markdown="1">short</td>
	</tr>
	<tr>
		<th>4byte</th>
		<td markdown="1">int, long<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1">int</td>
		<td markdown="1">int</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1">int, long<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1">long</td>
		<td markdown="1">long</td>
	</tr>
	<tr>
		<th rowspan="4">符号なし<br></br>整数</th>
		<th>1byte</th>
		<td markdown="1">unsigned char<sup>
            [*1](#a)
          </sup></td>
		<td markdown="1"></td>
		<td markdown="1">byte</td>
	</tr>
	<tr>
		<th>2byte</th>
		<td markdown="1">unsigned short, unsigned int<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1"></td>
		<td markdown="1">ushort</td>
	</tr>
	<tr>
		<th>4byte</th>
		<td markdown="1">unsigned int, unsigned long<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1"></td>
		<td markdown="1">uint</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1">unsigned long<sup>
            [*2](#b)
          </sup></td>
		<td markdown="1"></td>
		<td markdown="1">ulong</td>
	</tr>
	<tr>
		<th rowspan="2">浮動小数<br></br>点数</th>
		<th>4byte</th>
		<td markdown="1">float</td>
		<td markdown="1">float</td>
		<td markdown="1">float</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1">double</td>
		<td markdown="1">double</td>
		<td markdown="1">double</td>
	</tr>
	<tr>
		<th colspan="2">デシマル</th>
		<td markdown="1">なし</td>
		<td markdown="1">なし</td>
		<td markdown="1">decimal</td>
	</tr>
	<tr>
		<th colspan="2">文字</th>
		<td markdown="1">char (1byte)</td>
		<td markdown="1">char (2byte)</td>
		<td markdown="1">char (2byte)</td>
	</tr>
	<tr>
		<th colspan="2">文字列</th>
		<td markdown="1">string<sup>
            [*3](#c)
          </sup></td>
		<td markdown="1">String<sup>
            [*3](#c)
          </sup></td>
		<td markdown="1">string</td>
	</tr>
	<tr>
		<th colspan="2">オブジェクト型</th>
		<td markdown="1">なし</td>
		<td markdown="1">Object<sup>
            [*3](#c)
          </sup></td>
		<td markdown="1">object</td>
	</tr>
</table>


<a id="a">注1</a> :
C++ では、<code>signed</code> や <code>unsigned</code> の付かない <code>char</code> 型が符号付きか符合なしかは処理系に依存している。

<a id="b">注2</a> :
C++では、<code>int, short, long</code> のサイズは処理系に依存している。
通常、 <code>int</code> はその処理系でもっとも高速に処理を行えるサイズになっている。
<code>short</code> は <code>int</code> と同じか、それよりも小さいサイズ、
<code>long</code> は <code>int</code> と同じか、それよりも大きいサイズと決められている。

<a id="c">注3</a> :
C++ や Java の <code>string</code> や <code>Object</code> は言語に組み込まれた型ではなく、ライブラリで提供されているクラス。
