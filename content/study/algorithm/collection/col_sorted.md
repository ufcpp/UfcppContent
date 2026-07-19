---
title: "ソート済み配列"
source_url: "https://ufcpp.net/study/algorithm/collection/col_sorted/"
content_type: "Article"
published_at: "2015-05-06T14:05:05"
updated_at: "2015-07-13T13:32:25"
tags: []
umbraco_id: 1134
parent_id: 1128
sort_order: 5
aliases:
  - "/algorithm/col_sorted"
  - "/algorithm/col_sorted.html"
  - "/algorithm/collection/col_sorted/"
  - "/study/algorithm/col_sorted"
  - "/study/algorithm/col_sorted.html"
---

# ソート済み配列

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

コレクションを使う際、
要素の並び順には意味がない場合があります。
例えば、辞書のような用途を考えてください。
辞書の中に要素が含まれているかを検索できればそれでよくて、
要素が挿入された順序などはあまり意味がありません。

一般に、「[配列リスト](col_array.md#array)」等の、順序構造を保つコレクションを使った場合、
コレクション中に要素が含まれているかどうかを検索するのには、
要素数 n に比例した計算量（O(n)）が必要です。
しかしながら、（挿入された順序を保てない代わりに）要素の検索に特化したコレクションを使うことで、
高速に（計算量 O(log n) で）検索が可能です。

例えば、単なる配列（あるいは「[配列リスト](col_array.md#array)」）であっても、
常にソート済みに保っておくならば、
2分検索法という高速な検索アルゴリズムが適用できます。
ここで説明する<strong id="sorted" class="keyword">ソート済み配列</strong>（sorted array）は、
その2分検索法を使ったコレクションです。


## <a id="sec-generated-title-2"></a> <a id="bin_search"></a>2分検索法

例えば、配列中の要素を検索しようと思うと、
普通は以下のようになります。

```csharp
/// <summary>
/// 要素を検索する。
/// </summary>
/// <param name="array">検索対象</param>
/// <param name="elem">検索したい要素</param>
/// <returns>要素の位置（見つからなかった場合は配列長）</returns>
static int Search(int[] array, int elem)
{
  int i;
  for (i = 0; i < array.Length; ++i)
    if (array[i] == elem)
      break;
  return i;
}
```


見ての通り、前から順に調べていって、見つかった所で処理を打ち切っています。
要素がどこに含まれているのか分からないので、
平均的には配列長÷2 くらいの計算量が必要です。

これに対して、
配列がソートされた状態にある場合に限り、
以下のようなアルゴリズムで高速な検索が可能です。

```csharp
/// <summary>
/// 要素を2分検索する。
/// </summary>
/// <param name="array">検索対象（ソートされている必要あり）</param>
/// <param name="elem">検索したい要素</param>
/// <returns>要素の位置（見つからなかった場合は配列長）</returns>
static int BinarySearch(int[] array, int elem)
{
  if (array.Length == 0) return 0;

  int l = 0;
  int r = array.Length - 1;
  while (l < r)
  {
    int m = (l + r) / 2;
    if (array[m] < elem) l = m + 1;
    else if (array[m] > elem) r = m - 1;
    else return m;
  }
  if (array[l] == elem) return l;
  return array.Length;
}
```


検索対象の配列がソートされた状態にあるんだから、
検索範囲のど真ん中にある値だけ見れば、
次に調べるべき範囲が真ん中よりも右にあるのか左にあるのかが分かります。
このことを利用して、検索範囲を2分の1ずつ縮めていっています。
したがって、
ループの回数は最悪でも log<sub>2</sub>(配列長) になり、
計算量 O(log n) になります。
このようなアルゴリズムを<strong id="binary" class="keyword">2分検索法</strong>（binary search）と呼びます。


## <a id="sec-generated-title-3"></a> <a id="sorted_array"></a>ソート済み配列

「[2分検索法](#binary)」を使えば、
検索が計算量 O(log n) を作ることができます。

要するに、要素を挿入・削除するさいに、
常に配列をソート済みの状態に保ちます。
ただし、ソート済みに保ったままで要素の挿入・削除をするためには O(n) 必要で、
ソート済み配列が高速なのはあくまで検索のみになります。


## <a id="sec-generated-title-4"></a> <a id="character"></a>特徴

ソート済み配列は以下のような利点を持っています。

* 検索が極めて高速。計算量 O(log n) の検索アルゴリズムの中でも、オーバーヘッドが少なく、特に高速です。

* 使用メモリ量に関しても、オーバーヘッドがほとんどなく、効率がいい。


ただし、以下のような欠点もあります。

* 要素の挿入・削除は O(n)。


このような特徴から、
要素数が比較的少ない場合、
あるいは、
要素の挿入・削除の機会が少なく検索の比率の高い場合に用いられます。


## <a id="sec-generated-title-5"></a> <a id="impl"></a>実装方法

要素の挿入・削除もありますし、
内部実装には「[配列リスト](col_array.md#array)」を使いましょう。

```csharp
class SortedArray<T> : IEnumerable<T>
  where T: IComparable<T>
{
  ArrayList<T> buffer;
}
```


要素の検索には「[2分検索法](#binary)」を使います。
ここでは、要素が含まれている位置を返すメソッド IndexOf を例示します。

```csharp
public int IndexOf(T elem)
{
  if (this.buffer.Count == 0)
    return 0;

  int r = this.buffer.Count - 1;
  int l = 0;
  while (l < r)
  {
    int m = (r + l) >> 1;
    int comp = this.buffer[m].CompareTo(elem);
    if (comp > 0) r = m - 1;
    else if (comp < 0) l = m + 1;
    else return m;
  }

  if(this.buffer[l].CompareTo(elem) == 0)
    return l;
  return this.buffer.Count;
}
```


要素の挿入時にも、2分検索を使って要素の挿入位置を探します。

```csharp
public void Insert(T elem)
{
  if (this.buffer.Count == 0)
  {
    this.buffer.InsertLast(elem);
    return;
  }

  int r = this.buffer.Count - 1;
  int l = 0;
  int comp;
  while (l < r)
  {
    int m = (r + l) >> 1;
    comp = this.buffer[m].CompareTo(elem);
    if (comp > 0) r = m - 1;
    else if (comp < 0) l = m + 1;
    else return; // 重複不可
  }

  comp = this.buffer[l].CompareTo(elem);
  if(comp < 0)
    this.buffer.Insert(l + 1, elem);
  else if(comp > 0)
    this.buffer.Insert(l, elem);
}
```


削除も同様です。
2分検索を使って要素の位置を探して、その要素を削除します。

```csharp
public void Erase(T elem)
{
  int i = this.IndexOf(elem);
  if (i < this.buffer.Count)
    this.buffer.Erase(i);
}
```


挿入・削除するさい、要素の位置を探すのは2分検索法が使えて高速ですが、
配列リストへの挿入・削除自体は O(n) です。


## <a id="sec-generated-title-6"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/SortedArray.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/SortedArray.cs)

（ISet 「[インターフェース](../../csharp/oop/oo_interface.md#interface)」 は、
[Set.cs](../../../../assets/src/Set.cs) で定義しています。）
