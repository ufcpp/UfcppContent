---
title: "辞書"
source_url: "https://ufcpp.net/study/algorithm/collection/col_dic/"
content_type: "Article"
published_at: "2015-05-06T14:05:22"
updated_at: "2015-07-13T13:36:32"
tags: []
umbraco_id: 1141
parent_id: 1128
sort_order: 12
aliases:
  - "/study/algorithm/col_dic.html"
---

# 辞書

## <a id="sec-generated-title-1"></a> <a id="dic"></a>辞書

「辞書」というと、
例えば国語辞書などがその代表例なわけですが、
「項目名」と「項目の説明」等がペアになっています。
この例の場合、「項目名」で検索し、「項目の説明」を得たいわけです。

プログラミングの分野においては、
<strong id="dictionary" class="keyword">辞書</strong>（dictionary）というと、
鍵と値のペアを持っていて、
鍵による検索が可能なデータ構造のことをいいます。
先ほどの国語辞書の例でいうと、「項目名」が鍵で、「項目の説明」が値になります。

例えば、文章中に出てくる単語がそれぞれ何個ずつあるかをカウントしたいような場合を考えて見ましょう。
単語（string）を鍵として、個数（int）を値とするような辞書を用意して、
以下のような感じでカウントできます。

```csharp
IDictionary<string, int> dic;
string word;

dic[word] = dic[word] + 1;
```


辞書には、要素の順序は関係なく、要素の検索が高速なデータ構造があれば実装できるので、
「[セット](col_set.md#set)」を使って実装することができます。
要は、鍵と値のペアを持つクラスを用意して、
そのペアクラスの「[セット](col_set.md#set)」を作るだけです。

したがって、辞書の実装方法には、
「[ソート済み配列](col_sorted.md#sorted)」、
「[ハッシュテーブル](col_hash.md#hashtable)」、
「[2分探索木](col_tree.md#bintree)」等、
いくつか選択肢があります。
いずれの実装方法を採った場合でも、
必要な操作は同じなので、
以下のような「[インターフェース](../../csharp/oop/oo_interface.md#interface)」  IDictionary を定義しておきます。

```csharp
/// <summary>
/// 辞書。
/// </summary>
/// <typeparam name="TKey">鍵の型</typeparam>
/// <typeparam name="TValue">値の型</typeparam>
public interface IDictionary<TKey, TValue>
  : IEnumerable<KeyValuePair<TKey, TValue>>
{
  /// <summary>
  /// 新しい要素の挿入。
  /// </summary>
  void Insert(TKey key, TValue val);

  /// <summary>
  /// 要素の削除。
  /// </summary>
  void Erase(TKey key);

  /// <summary>
  /// 要素を含むかどうか。
  /// </summary>
  bool Contains(TKey key);

  /// <summary>
  /// [] を使って値を取り出す。
  /// </summary>
  TValue this[TKey key]
  {
    set;
    get;
  }

  /// <summary>
  /// 鍵一覧取得
  /// </summary>
  IEnumerable<TKey> Keys { get; }

  /// <summary>
  /// 値一覧取得
  /// </summary>
  IEnumerable<TValue> Values { get; }
}
```


IDictionary で定義しているのは、
値の挿入・削除・検索に加え、
「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」による値の参照と、
鍵・値の一覧取得です。

ちなみに、辞書構造は、
Perl 等いくつかの言語では「連想配列（associated array）」と呼ばれたりもします。
鍵と関連付けられていて（associated）、
配列のように「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」で値にアクセスできるため、
こう呼ばれます。

また、Perl では、「ハッシュ」と呼ぶ場合もあります。
おそらく、Perl の連想配列は「[ハッシュテーブル](col_hash.md#hashtable)」を使って実装されているのでしょう。


## <a id="sec-generated-title-2"></a> <a id="impl"></a>実装方法

ここでは、
「[ハッシュテーブル](col_hash.md#hashtable)」を使った実装で説明したいと思います。
名前は HashDictionary としておきましょう。

まずは、鍵と値のペアを持つデータ構造 Entry を定義します。
ハッシュ関数や、等値判定は鍵の物をそのまま使います。
（すなわち、値は無視して、鍵が等しいかどうかだけを見る。）

```csharp
/// <summary>
/// 辞書のエントリー。
/// </summary>
internal class Entry<TKey, TValue>
{
  internal TKey key;
  internal TValue val;

  internal Entry(TKey key) : this(key, default(TValue)) { }

  internal Entry(TKey key, TValue val)
  {
    this.key = key;
    this.val = val;
  }

  public override int GetHashCode()
  {
    return this.key.GetHashCode();
  }

  public override bool Equals(object obj)
  {
    Entry<TKey, TValue> ent = obj as Entry<TKey, TValue>;
    if (ent == null) return false;
    return this.key.Equals(ent.key);
  }
}
```


「[ハッシュテーブル](col_hash.md#hashtable)」を用いる場合にはこれで十分ですが、
「[ソート済み配列](col_sorted.md#sorted)」や
「[2分探索木](col_tree.md#bintree)」を用いる場合、
鍵の大小比較が必要なので、IComparable を実装して以下のようにします。

```csharp
internal class ComparableEntry<TKey, TValue>
  : Entry<TKey, TValue>,
  IComparable<ComparableEntry<TKey, TValue>>
  where TKey : IComparable<TKey>
{
  internal ComparableEntry(TKey key) : base(key) { }
  internal ComparableEntry(TKey key, TValue val) : base(key, val) { }

  public int CompareTo(ComparableEntry<TKey, TValue> other)
  {
    return this.key.CompareTo(other.key);
  }
}
```


そして、辞書本体ですが、
この Entry クラスの「[ハッシュテーブル](col_hash.md#hashtable)」をメンバー変数として持ちます。

```csharp
public class HashDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
  HashTable<Entry<TKey, TValue>> table;
}
```


値の挿入・削除・検索などは、この table に丸投げすれば OK です。

```csharp
public void Insert(TKey key, TValue val)
{
  this.table.Insert(new Entry<TKey, TValue>(key, val));
}

public void Erase(TKey key)
{
  this.table.Erase(new Entry<TKey, TValue>(key));
}

public bool Contains(TKey key)
{
  return this.table.Contains(new Entry<TKey, TValue>(key));
}
```


最後に、「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」は、
検索と挿入の組み合わせで実装します。

```csharp
public TValue this[TKey key]
{
  get
  {
    Entry<TKey, TValue> entry
      = this.table.Find(new Entry<TKey, TValue>(key));
    if (entry == null) return default(TValue);
    return entry.val;
  }
  set
  {
    Entry<TKey, TValue> entry
      = this.table.Find(new Entry<TKey, TValue>(key));
    if (entry == null) this.Insert(key, value);
    else entry.val = value;
  }
}
```



## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。
上述の HashDictionary に加え、
「[ソート済み配列](col_sorted.md#sorted)」、
「[2分探索木](col_tree.md#bintree)」を使った実装
SortedDictionary、TreeDictionary も実装しています。
（中身は HashDictionary とほとんど同じなので、説明は割愛。）

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Dictionary.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Dictionary.cs)
