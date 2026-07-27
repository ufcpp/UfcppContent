---
title: "ハッシュテーブル"
source_url: "https://ufcpp.net/study/algorithm/collection/col_hash/"
content_type: "Article"
published_at: "2015-05-06T14:05:09"
updated_at: "2015-07-13T13:32:53"
tags: []
umbraco_id: 1135
parent_id: 1128
sort_order: 6
aliases:
  - "/study/algorithm/col_hash.html"
---

# ハッシュテーブル

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

<strong id="hashtable" class="keyword">ハッシュテーブル</strong>（hash table）は、
「ハッシュ値」という物を使うことによって、
要素の挿入・削除・検索を非常に高速に行うことの出来るコレクションです。

挿入する要素の数よりも、余裕を見て大きめのメモリを確保して置くならば、
要素の挿入・削除・検索をほぼ O(1) （要素数によらず一定時間）で行うことができます。


## <a id="sec-generated-title-2"></a> <a id="hash"></a>ハッシュ値

<strong id="hash" class="keyword">ハッシュ</strong>（hash）というのは、
文字列などの任意のデータから、そのデータを要約して得られる固定長の値のことです。
得られた値のことをハッシュ値（hash value）、
値を得る操作をハッシュ関数（hash function）といいます。

ネットワークなどを通してメッセージを送る際、
通信路でエラーが混入していないかどうかを確かめるために、
送信元と受信先の双方でハッシュ値を計算し、値を比較するのに使われたりします。

hash という言葉は、料理のハッシュドビーフなどについているハッシュと同じで、
「細切れ」という意味です。
「送信したメッセージを細切れにした一部分」というような感じの意味合いで、
元のメッセージと同じだけの情報は持っていませんが、
メッセージの真偽などの確認に使える値ということです。

元のメッセージよりも持っている情報量が少ないということは、
原理的に不可逆で、
ハッシュ値から元のメッセージを再生することはできません。
このような性質から、パスワード認証に使われたりもします。
（認証サーバ内では、パスワードのハッシュ値が記録されている。）

ちなみに、このような、メッセージの真偽確認やパスワード認証のために使われる場合には、
ハッシュ値のことを「メッセージダイジェスト」と呼ぶ場合もあります
（文字通り、メッセージの要約という意味で用いるので）。
これに対して、「ハッシュ値」という呼び方をする場合、
「任意のデータから固定長の値（ほとんどの場合整数値）を得る」という部分に意味があります。


## <a id="sec-generated-title-3"></a> <a id="hashtable"></a>ハッシュテーブル

通常、配列の [] の中には整数しか使えません。

```csharp
int[] x = new int[N];
x[0] = 0;        // これは OK
x["string"] = 0; // これういうことは無理
```


ここで、
前節で説明したように、ハッシュという考え方を用いれば、
任意のデータから（そのデータを要約する）整数値が得られます。
その整数値を使えば、
任意のデータ（文字列だろうが、自分で定義した構造体だろうがなんでも）を配列に格納できたりしないでしょうか。

```csharp
int[] x = new int[N];
x[Hash("string")] = 0; // ハッシュ値を使って文字列を整数化
```


ここではひとまず、ハッシュ関数（上のコード中でいう Hash）の作り方には触れずにおきます（のちほど）。
あと、説明の簡素化のために、割と適当なコードを書きましたが、
実用のためにはもう一工夫必要です。

ここでの主題は、こういうハッシュ値を使ったコレクションを作れないだろうか、ということです。
実際、この発想によって、
要素の挿入・削除・検索を高速に行うことの出来るコレクションが作れ、
これをハッシュテーブル（hash table）と呼びます。

先ほど、実用にはもう一工夫必要と書きましたが、
どういうことかと言うと、
1つは、異なるデータから同じハッシュ値が得られる可能性があるので、
同じハッシュ値を持つデータをリストとして持つ必要があるということと、
もう1つは、配列のサイズには制限があるので、
実際にはハッシュ値を配列サイズで割ったあまりを使うということです。

```csharp
List<string>[] x = new List<string>[N]; // とりあえず、標準ライブラリの List 使用
x[Hash("string") % N].Add("string"); // リスト化＆ N で剰余
```


ハッシュ関数に偏りがなく、
配列のサイズを十分に大きく取れるならば、
ハッシュ値が被る確率は非常に少なく、
結果として、要素の挿入・検索がほぼ一定時間で可能になります。
（配列のサイズを M、挿入する要素の数を N とすると、
挿入・削除・検索の平均計算量は O(N/M) です。）


## <a id="sec-generated-title-4"></a> <a id="character"></a>特徴

ハッシュテーブルは以下のような利点を持っています。

* ハッシュ関数に偏りがなく、十分なサイズの配列を取れるなら、ほぼ一定時間で要素の挿入・削除・検索ができます。


ただし、以下のような欠点もあります。

* ハッシュ関数の作り方や、配列サイズに大きく依存します。
    * ハッシュ関数に偏りがなくなるような工夫が必要。

    * 配列を大きめに取っておかないと検索効率が悪化。



* 要素の順序はばらばらになります。


このような特徴から、
偏りのないハッシュ関数が用意できて、かつ、
メモリがふんだんにある環境ではハッシュテーブルがよく用いられます。


## <a id="sec-generated-title-5"></a> <a id="impl"></a>実装方法

ハッシュテーブルで必要となる内部データは、
基本的に「リストの配列」です。
リストは、「[片方向連結リスト](col_flist.md#flist)」のような構造で十分です。
そのため、まず、リストのノードを定義します。

```csharp
class Node
{
  internal T val;
  internal Node next;

  internal Node(T val)
  {
    this.val = val;
    this.next = null;
  }
}
```


そして、このノードの配列を用意します。
また、剰余演算が必要なので、
「[循環バッファ](col_circular.md#circular)」のときと同様に
（剰余演算の変わりにマスク演算（論理 AND 演算）にしたいので）、
マスク用の変数も用意します。

```csharp
class HashTable<T> : IEnumerable<T>
{
  Node[] table;
  int mask;
}
```


肝心のハッシュ関数に関してですが、
C# / .NET Framework では、
（すべてのデータ型の基底となる）object 型に GetHashCode という関数が用意されています。
ここでは、これを使うことにしましょう。

このハッシュ関数を使って、要素の挿入は以下のように行います。

```csharp
public void Insert(T elem)
{
  int code = elem.GetHashCode() & this.mask;
  Node n = this.table[code];
  Node m = new Node(elem, n);
  m.next = n;
  this.table[code] = m;
}
```


削除や検索は以下の通りです。

```csharp
public void Erase(T elem)
{
  int code = elem.GetHashCode() & this.mask;
  Node n = this.table[code];

  if (n == null) return;
  if (n.next == null)
    this.table[code] = null;

  while (n.next != null && n.next.val.Equals(elem))
    n = n.next;
  if(n.next != null)
    n.next = n.next.next;
}
```


```csharp
public bool Contains(T elem)
{
  int code = elem.GetHashCode() & this.mask;
  Node n = this.table[code];
  while (n != null && !n.val.Equals(elem))
    n = n.next;
  return n != null;
}
```



## <a id="sec-generated-title-6"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/HashTable.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/HashTable.cs)

（ISet 「[インターフェース](../../csharp/oop/oo_interface.md#interface)」 は、
[Set.cs](../../../../assets/src/Set.cs) で定義しています。）
