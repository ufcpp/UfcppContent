---
title: "配列リスト"
source_url: "https://ufcpp.net/study/algorithm/collection/col_array/"
content_type: "Article"
published_at: "2015-05-06T14:04:53"
updated_at: "2018-03-25T09:26:18"
tags: []
umbraco_id: 1130
parent_id: 1128
sort_order: 1
aliases:
  - "/algorithm/col_array"
  - "/algorithm/col_array.html"
  - "/algorithm/collection/col_array/"
  - "/study/algorithm/col_array"
  - "/study/algorithm/col_array.html"
---

# 配列リスト

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

「[配列](../../csharp/structured/st_array.md#array)」も、
同じ型のデータを複数集めた物ですから、
コレクションの一種です。
実際、C# の配列は <code>System.Collections.ICollection</code> という「[インターフェース](../../csharp/oop/oo_interface.md#interface)」を実装していて、コレクションとして扱うことができます。

ですが、単なる配列の場合、
コレクションとして使うには幾分か機能不足な部分があります。

* 最初に配列を作ったときに指定した長さ以上の要素を持てない。

* 新しい要素の挿入・削除ができない。


ということで、
配列に

* 要素数が足りなくなったら自動的に配列を確保しなおす。

* 新しい要素の挿入・削除。


という機能を追加したコレクションクラスを作ることがあります。

このような機能を持つクラスは、ライブラリによって名前がまちまちで、
C++ の STL では 「[vector](../../stl/seq_container/vector.md#vector)」、
C# （.NET Framework）では System.Collections.ArrayList や System.Collections.Generic.List&lt;T&gt; という名前になっています。
ここでは「<strong id="array" class="keyword">配列リスト</strong>」と呼ぶことにしましょう。


## <a id="sec-generated-title-2"></a> <a id="character"></a>特徴

配列リストは以下のような利点を持っています。

* 実装が単純（作るのが楽というだけではなく、動作が高速）。

* 「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」を使った要素へのランダムアクセスが、配列とほとんど変わらない速度（もちろんオーダーは O(1)）でできる。


しかしながら、所詮中身は配列なので、
以下のような欠点が残っています。

* 末尾以外への要素の挿入・削除が低速（要素数を n として平均 O(n)）。



## <a id="sec-generated-title-3"></a> <a id="impl"></a>実装方法

配列リストは、
単純に配列をラッピングするだけなので、実装が非常に簡単です。
まず、配列と、（確保した長さとは別に）実際に入っている要素の数を保持する変数を用意します。

```csharp
class ArrayList<T> : IEnumerable<T>
{
  T[] data;
  int count;
}
```


配列は、図1に示すように、実際に格納されている要素数（Count）よりも大きめに確保しておきます。

```csharp
public ArrayList(int capacity)
{
  this.data = new T[capacity];
  this.count = 0;
}
```


<figure>
	[![大きめに配列を確保](../../../../assets/media/ufcpp2000/algorithm/fig/col_array0.png)](../../../../assets/media/ufcpp2000/algorithm/fig/col_array0.png)
	<figcaption>大きめに配列を確保</figcaption>
</figure>


要素を追加していって、
配列の長さが足りなくなったら、配列を確保しなおします。
新たに確保しなおす配列の長さをどうするかは自由に決めれますが、
2倍ずつ長くする場合が多いです。

```csharp
/// <summary>
/// 配列を確保しなおす。
/// </summary>
/// <remarks>
/// 配列長は2倍ずつ拡張していきます。
/// </remarks>
void Extend()
{
  T[] data = new T[this.data.Length * 2];
  for (int i = 0; i < this.data.Length; ++i) data[i] = this.data[i];
  this.data = data;
}
```


中身は単なる配列なので、
末尾以外の位置の要素の挿入・削除は、
要素を1つずつずらして隙間を空ける/埋める作業が必要になります。
したがって、末尾以外への要素の挿入・削除は非常に低速です。

```csharp
/// <summary>
/// i 番目の位置に新しい要素を追加。
/// </summary>
/// <param name="i">追加位置</param>
/// <param name="elem">追加する要素</param>
public void Insert(int i, T elem)
{
  if (this.count >= this.data.Length)
    this.Extend();

  for (int n = this.count; n > i; --n)
  {
    this.data[n] = this.data[n - 1];
  }
  this.data[i] = elem;
  ++this.count;
}

/// <summary>
/// i 番目の要素を削除。
/// </summary>
/// <param name="i">削除位置</param>
public void Erase(int i)
{
  for (int n = i; n < this.count - 1; ++n)
  {
    this.data[n] = this.data[n + 1];
  }
  --this.count;
}
```



## <a id="sec-generated-title-4"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ArrayList.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/ArrayList.cs)
