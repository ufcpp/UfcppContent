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
  - "/algorithm/col_dic"
  - "/algorithm/col_dic.html"
  - "/algorithm/collection/col_dic/"
  - "/study/algorithm/col_dic"
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

<pre class="source" title="" lang="">
<code>IDictionary&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; dic;
<span class="reserved">string</span> word;

dic[word] = dic[word] + 1;
</code></pre>


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

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 辞書。
/// &lt;/summary&gt;
/// &lt;typeparam name="TKey"&gt;鍵の型&lt;/typeparam&gt;
/// &lt;typeparam name="TValue"&gt;値の型&lt;/typeparam&gt;</span>
<span class="reserved">public interface</span> IDictionary&lt;TKey, TValue&gt;
  : IEnumerable&lt;KeyValuePair&lt;TKey, TValue&gt;&gt;
{
  <span class="comment">/// &lt;summary&gt;
  /// 新しい要素の挿入。
  /// &lt;/summary&gt;</span>
  <span class="reserved">void</span> Insert(TKey key, TValue val);

  <span class="comment">/// &lt;summary&gt;
  /// 要素の削除。
  /// &lt;/summary&gt;</span>
  <span class="reserved">void</span> Erase(TKey key);

  <span class="comment">/// &lt;summary&gt;
  /// 要素を含むかどうか。
  /// &lt;/summary&gt;</span>
  <span class="reserved">bool</span> Contains(TKey key);

  <span class="comment">/// &lt;summary&gt;
  /// [] を使って値を取り出す。
  /// &lt;/summary&gt;</span>
  TValue <span class="reserved">this</span>[TKey key]
  {
    <span class="reserved">set</span>;
    <span class="reserved">get</span>;
  }

  <span class="comment">/// &lt;summary&gt;
  /// 鍵一覧取得
  /// &lt;/summary&gt;</span>
  IEnumerable&lt;TKey&gt; Keys { <span class="reserved">get</span>; }

  <span class="comment">/// &lt;summary&gt;
  /// 値一覧取得
  /// &lt;/summary&gt;</span>
  IEnumerable&lt;TValue&gt; Values { <span class="reserved">get</span>; }
}
</code></pre>


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

<pre class="source" title="" lang="">
<code><span class="comment">/// &lt;summary&gt;
/// 辞書のエントリー。
/// &lt;/summary&gt;</span>
<span class="reserved">internal class</span> Entry&lt;TKey, TValue&gt;
{
  <span class="reserved">internal</span> TKey key;
  <span class="reserved">internal</span> TValue val;

  <span class="reserved">internal</span> Entry(TKey key) : <span class="reserved">this</span>(key, <span class="reserved">default</span>(TValue)) { }

  <span class="reserved">internal</span> Entry(TKey key, TValue val)
  {
    <span class="reserved">this</span>.key = key;
    <span class="reserved">this</span>.val = val;
  }

  <span class="reserved">public override int</span> GetHashCode()
  {
    <span class="reserved">return this</span>.key.GetHashCode();
  }

  <span class="reserved">public override bool</span> Equals(<span class="reserved">object</span> obj)
  {
    Entry&lt;TKey, TValue&gt; ent = obj <span class="reserved">as</span> Entry&lt;TKey, TValue&gt;;
    <span class="reserved">if</span> (ent == <span class="reserved">null</span>) <span class="reserved">return false</span>;
    <span class="reserved">return this</span>.key.Equals(ent.key);
  }
}
</code></pre>


「[ハッシュテーブル](col_hash.md#hashtable)」を用いる場合にはこれで十分ですが、
「[ソート済み配列](col_sorted.md#sorted)」や
「[2分探索木](col_tree.md#bintree)」を用いる場合、
鍵の大小比較が必要なので、IComparable を実装して以下のようにします。

<pre class="source" title="" lang="">
<code><span class="reserved">internal class</span> ComparableEntry&lt;TKey, TValue&gt;
  : Entry&lt;TKey, TValue&gt;,
  IComparable&lt;ComparableEntry&lt;TKey, TValue&gt;&gt;
  <span class="reserved">where</span> TKey : IComparable&lt;TKey&gt;
{
  <span class="reserved">internal</span> ComparableEntry(TKey key) : <span class="reserved">base</span>(key) { }
  <span class="reserved">internal</span> ComparableEntry(TKey key, TValue val) : <span class="reserved">base</span>(key, val) { }

  <span class="reserved">public int</span> CompareTo(ComparableEntry&lt;TKey, TValue&gt; other)
  {
    <span class="reserved">return this</span>.key.CompareTo(other.key);
  }
}
</code></pre>


そして、辞書本体ですが、
この Entry クラスの「[ハッシュテーブル](col_hash.md#hashtable)」をメンバー変数として持ちます。

<pre class="source" title="" lang="">
<code><span class="reserved">public class</span> HashDictionary&lt;TKey, TValue&gt; : IDictionary&lt;TKey, TValue&gt;
{
  HashTable&lt;Entry&lt;TKey, TValue&gt;&gt; table;
}
</code></pre>


値の挿入・削除・検索などは、この table に丸投げすれば OK です。

<pre class="source" title="" lang="">
<code><span class="reserved">public void</span> Insert(TKey key, TValue val)
{
  <span class="reserved">this</span>.table.Insert(<span class="reserved">new</span> Entry&lt;TKey, TValue&gt;(key, val));
}

<span class="reserved">public void</span> Erase(TKey key)
{
  <span class="reserved">this</span>.table.Erase(<span class="reserved">new</span> Entry&lt;TKey, TValue&gt;(key));
}

<span class="reserved">public bool</span> Contains(TKey key)
{
  <span class="reserved">return this</span>.table.Contains(<span class="reserved">new</span> Entry&lt;TKey, TValue&gt;(key));
}
</code></pre>


最後に、「[インデクサー](../../csharp/oop/oo_indexer.md#indexer)」は、
検索と挿入の組み合わせで実装します。

<pre class="source" title="" lang="">
<code><span class="reserved">public</span> TValue <span class="reserved">this</span>[TKey key]
{
  <span class="reserved">get</span>
  {
    Entry&lt;TKey, TValue&gt; entry
      = <span class="reserved">this</span>.table.Find(<span class="reserved">new</span> Entry&lt;TKey, TValue&gt;(key));
    <span class="reserved">if</span> (entry == <span class="reserved">null</span>) <span class="reserved">return default</span>(TValue);
    <span class="reserved">return</span> entry.val;
  }
  <span class="reserved">set</span>
  {
    Entry&lt;TKey, TValue&gt; entry
      = <span class="reserved">this</span>.table.Find(<span class="reserved">new</span> Entry&lt;TKey, TValue&gt;(key));
    <span class="reserved">if</span> (entry == <span class="reserved">null</span>) <span class="reserved">this</span>.Insert(key, value);
    <span class="reserved">else</span> entry.val = value;
  }
}
</code></pre>



## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプルソース

C# サンプルソースを示します。
上述の HashDictionary に加え、
「[ソート済み配列](col_sorted.md#sorted)」、
「[2分探索木](col_tree.md#bintree)」を使った実装
SortedDictionary、TreeDictionary も実装しています。
（中身は HashDictionary とほとんど同じなので、説明は割愛。）

[https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Dictionary.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Algorithm/Collections/Dictionary.cs)
