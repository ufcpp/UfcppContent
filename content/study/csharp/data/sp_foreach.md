---
title: "foreach"
source_url: "https://ufcpp.net/study/csharp/data/sp_foreach/"
content_type: "Article"
published_at: "2002-11-03T00:00:00"
updated_at: "2008-01-05T00:00:00"
tags: []
umbraco_id: 1299
parent_id: 1298
sort_order: 0
aliases:
  - "/csharp/data/sp_foreach/"
  - "/csharp/sp_foreach"
  - "/csharp/sp_foreach.html"
  - "/study/csharp/sp_foreach"
  - "/study/csharp/sp_foreach.html"
---

# foreach

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

foreachとは、コレクションのすべての要素を1回ずつ読み出すための構文です。


##### <a id="sec-generated-title-2"></a>ポイント

* 配列みたいに for (int i = 0; i &lt; array.Length; ++i) { array[i] ... } という形で要素の列挙ができないようなコレクションも、foreach なら列挙可能。

* foreach (変数 in コレクション) { ... }



## <a id="sec-generated-title-3"></a> <a id="collection"></a>コレクション

<em>コレクション</em>(「コンテナ」ともいいます)とは配列やリスト、辞書などの複数の要素をひとつにまとめるクラスのことです。
複数の要素をまとめておく方法にはさまざまな方法があり、
その方法によって呼び名が変わります。
以下にコレクションの例とその簡単な説明を列挙します。

<table summary="">

	<tr>
		<td markdown="1"></td>
		<th>データ格納方式</th>
		<th>長所</th>
		<th>欠点</th>
	</tr>
	<tr>
		<th>配列</th>
		<td markdown="1">要素を単純に横に並べて置いておく。</td>
		<td markdown="1">処理の効率もメモリの使用効率もよい。また、任意の場所にある要素にいつでもアクセスできる。</td>
		<td markdown="1">末尾以外の場所に要素を挿入することが出来ない(出来ても効率が悪い)。</td>
	</tr>
	<tr>
		<th>連結リスト</th>
		<td markdown="1">セルと呼ばれる要素を入れておく箱を繋げていく。</td>
		<td markdown="1">任意の場所の要素の追加・削除が効率的に行える。</td>
		<td markdown="1">配列と比べ効率が落ちる。また、配列と違って前から順に要素をたどっていくことしか出来ない。</td>
	</tr>
	<tr>
		<th>探査木</th>
		<td markdown="1">左右に枝の伸びる木構造にデータを格納。 「左側の枝には小さな値、右側の枝には大きな値を格納する」といった条件をつけておく。</td>
		<td markdown="1">要素の検索・挿入・削除が効率的に行える。</td>
		<td markdown="1">要素を挿入した順序が意味を成さなくなる。</td>
	</tr>
</table>


ここでは詳細には触れませんが、
当サイト上にある「[C++ STL](../../stl/index.md)」や「[アルゴリズムとデータ構造](../../algorithm/index.md)」でもコレクションについて簡単な説明がありますので、興味のある方はそちらをご覧ください。
また、コレクションについてより詳しく知りたい方は検索エンジンで「データ構造 アルゴリズム」などをキーワードにして検索してみてください。

ここでは例として連結リストを示します。
あくまで例として示すだけなので、単純な実装方法を取っています。
(本来はもう少しちゃんとした実装の仕方をしないとだめ。)

<pre class="source" title="連結リストの例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="comment">/// &lt;summary&gt;
/// リストのノード
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Node
{
  <span class="reserved">public int</span> elem;
  <span class="reserved">public</span> Node next;

  <span class="reserved">public</span> Node() : <span class="reserved">this</span>(0, <span class="reserved">null</span>){}

  <span class="reserved">public</span> Node(<span class="reserved">int</span> val, Node next)
  {
    <span class="reserved">this</span>.elem = val;
    <span class="reserved">this</span>.next = next;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 連結リストクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> List
{
  <span class="reserved">public</span> Node head;

  <span class="reserved">public</span> List()
  {
    head = <span class="reserved">null</span>;
  }

  <span class="comment">/// &lt;summary&gt;
  /// リストに新しい要素を追加する。
  /// &lt;/summary&gt;
  /// &lt;param name="val"&gt;追加する値&lt;/param&gt;</span>
  <span class="reserved">public void</span> Add(<span class="reserved">int</span> val)
  {
    Node node = <span class="reserved">new</span> Node(val, <span class="reserved">this</span>.head);
    <span class="reserved">this</span>.head = node;
  }
}
</code></pre>



## <a id="sec-generated-title-4"></a> <a id="iEnumerable"></a>IEnumerable インターフェース

ここで1つ問題があります。
データの格納方式が違えば、当然データの読み出し方も変わってくるということです。
例えば、配列の場合、以下のようにすれば全ての要素を読み出せます。

<pre class="source" title="配列のデータ読み出し" lang="">
<code><span class="reserved">int</span>[] a = <span class="reserved">new int</span>[]{1, 3, 5, 7};
<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;a.Length; ++i)
  Console.Write(<span class="literal">"{0}\n"</span>, a[i]);
</code></pre>


しかし、上述の例に挙げたリストクラスに対して同じ操作を行おうとすると以下のようになります。

<pre class="source" title="" lang="">
<code>List list = <span class="reserved">new</span> List();
list.Add(7);
list.Add(5);
list.Add(3);
list.Add(1);
<span class="reserved">for</span>(Node n=list.head; n!=<span class="reserved">null</span>; n=n.next)
{
  Console.Write(<span class="literal">"{0}\n"</span>, n.elem);
}
</code></pre>


同じ「コレクション内のすべての要素を1回ずつ読み出す」という操作なのに全然違うコードを書く必要があります。
コレクションごとにコードを変更するのは面倒ですし、
仕様の変更に柔軟に対応できないなどの問題があります。

そこで、コレクションクラスは共通のインターフェースを実装するという決まりを作り、
要素へのアクセスはこのインターフェースを通して行うのが一般的です。
そのためのクラスとして .NET Framework には <em>
        <code>IEnumerable</code>
      </em> というインターフェースが用意されています。
もちろん、C# の配列は <code>IEnumerable</code> インターフェースを実装しています。

<code>IEnumerable</code> インターフェースの実装の仕方については後ほど述べることにして、
ここでは <code>IEnumerable</code> インターフェースを介した要素へのアクセスの仕方のみを説明します。
<code>IEnumerable</code> インターフェースを介した要素へのアクセスは以下のようにします。

<pre class="source" title="IEnamerable インターフェースを介したコレクションのアクセス" lang="">
<code><span class="reserved">int</span>[] array = <span class="reserved">new int</span>[]{1, 3, 5, 7};

IEnumerator e = array.GetEnumerator();
<span class="reserved">while</span>(e.MoveNext())
{
  <span class="reserved">int</span> val = (<span class="reserved">int</span>)e.Current;
  Console.Write(<span class="literal">"{0}\n"</span>, val);
}
</code></pre>


ここで、<code>IEnumerator</code> とは<em>列挙子</em>と呼ばれるクラスを作るためのインターフェースです。
<code>IEnumerator</code> インターフェースについては後ほど説明します。


## <a id="sec-generated-title-5"></a> <a id="foreach"></a>foreach文とは

<strong id="foreach" class="keyword">foreach 文</strong>を用いるとこで <code>IEnumerable</code> インターフェースを介した要素へのアクセスを簡単化することが出来ます。
以下のように、foreachを使うことでコレクションのすべての要素を1回ずつ読み出すことができます。

<pre class="source" title="foreachの使い方" lang="">
<code><span class="reserved">foreach</span>(<span class="input">型名</span> <span class="input">変数</span> <span class="reserved">in</span> <span class="input">コレクション</span>)
  <span class="input">文</span>
</code></pre>


このコードは以下のように展開されます。

<pre class="source" title="foreachの実態" lang="">
<code><span class="reserved">try</span>
{
  IEnumerator e = array.GetEnumerator();
  <span class="reserved">while</span>(e.MoveNext())
  {
    <span class="input">型名</span> <span class="input">変数</span> = (<span class="input">型名</span>)e.Current;
    <span class="input">文</span>
  }
} 
<span class="reserved">finally</span>
{
  <span class="input">Dispose処理</span>
}
</code></pre>

「Dispose処理」の部分は、コンパイル時点で`IDisposable`なことがわかっている型かどうかで実際に生成されるコードが変わります。
コンパイル時点で`IDisposable`なことがわかる場合は以下の通り。

<pre class="source" title="foreachのDispose処理(コンパイル時点でわかっている場合)" lang="">
<code>    ((<span class="type">IDisposable</span>)e).Dispose();
</code></pre>

逆に、わからない場合は以下のようになります。

<pre class="source" title="foreachのDispose処理(コンパイル時点でわかっている場合)" lang="">
<code>    <span class="type">IDisposable</span> d = e <span class="reserved">as</span> <span class="type">IDisposable</span>;
    <span class="reserved">if</span> (d != null) d.Dispose();
</code></pre>

例えば、<code>int</code>型の配列の要素を読み出して画面に表示するには以下のようにします。

<pre class="source" title="foreachの例" lang="">
<code><span class="reserved">int</span>[] array = <span class="reserved">new int</span>[10]{1, 2, 4, 8, 16, 32, 64, 128, 256, 512};

<em><span class="reserved">foreach</span>(<span class="reserved">int</span> n <span class="reserved">in</span> array)</em>
{
  Console.Write(n + <span class="literal">" "</span>);
}
</code></pre>


<pre class="console" title="">
1 2 4 8 16 32 64 128 256 512 
</pre>


foreach文の実態は<code>IEnumerable</code> インターフェースを介した要素へのアクセスですから、
<code>IEnumerable</code> インターフェースを実装しているならどんなコレクションクラスの要素でも読み出すことが出来ます。
例えば、.NET Framework標準ライブラリの<code>ArrayList</code>クラスは<code>IEnumrable</code>インターフェースを実装していますので、以下のようにforeach文を使ってコレクション内の要素を列挙することが出来ます。

<pre class="source" title="ArrayListに対してforeachを使う" lang="">
<code><em>ArrayList</em> list = <span class="reserved">new</span> ArrayList();

<span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;10; ++i)
{
  list.Add(i * (i + 1) / 2);
}

<em><span class="reserved">foreach</span>(<span class="reserved">int</span> s <span class="reserved">in</span> list)</em>
{
  Console.Write(s + <span class="literal">" "</span>);
}
</code></pre>


<pre class="console" title="">
0 1 3 6 10 15 21 28 36 45 
</pre>



### <a id="sec-generated-title-6"></a> <a id="pattern-based"></a>余談： パターン ベース

余談になりますが、
foreach で使うコレクションは、実は IEnumerable を実装している必要はなくて、
GetEnumerator という名前のメソッドを持っていればどんな型でもよかったりします。
（要するに、「[パターン ベース](../misc/miscpatternbased.md)」。） 

### <a id="sec-generated-title-7"></a> <a id="extension-getenumerator"></a>拡張メソッドでの GetEnumerator 実装

<h5 class="version version9">Ver. 9</h5>

C# 8.0 まではパターン ベースと言っても、`GetEnumerator` メソッドはインスタンス メソッドである必要がありました。
これが C# 9.0 で緩和されて、[拡張メソッド](../functional/sp3_extension.md)での実装が認められました。

例えば、C# 8.0 で入った [`Range`](dataranges.md#range) に対して以下のような拡張メソッドを書くことで、`foreach (var i in x..y)` みたいな書き方ができるようになります。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
 
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">i</span> <span class="control">in</span> 5..10)
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">i</span>); <span class="comment">// 5, 6, 7, 8, 9</span>
}
 
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">RangeExtension</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">RangeEnumerator</span> <span class="method">GetEnumerator</span>(<span class="reserved">this</span> <span class="type">Range</span> <span class="variable">r</span>) =&gt; <span class="reserved">new</span>(<span class="variable">r</span>);
 
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">RangeEnumerator</span>
    {
        <span class="reserved">private</span> <span class="reserved">int</span> _i;
        <span class="reserved">private</span> <span class="reserved">int</span> _end;
 
        <span class="reserved">public</span> <span class="type">RangeEnumerator</span>(<span class="type">Range</span> <span class="variable">r</span>)
        {
            _i = <span class="variable">r</span>.Start.Value - 1;
            _end = <span class="variable">r</span>.End.Value;
        }
 
        <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() =&gt; ++_i &lt; _end;
        <span class="reserved">public</span> <span class="reserved">int</span> Current =&gt; _i;
    }
}
</code></pre>

(これまでは単に C# 1.0 時代からある文法に下手に手を入れるのが怖くて認められていなかっただけです。)


## <a id="sec-generated-title-8"></a> <a id="ownmaking"></a>コレクションクラスの自作

<code>IEnumrable</code>インターフェースを実装することで、foreach文で利用できるコレクションクラスを自作できます。

<code>IEnumrable</code>インターフェースには<code>GetEnumerator</code>メソッドがあり、このメソッドは<code>IEnumerator</code>インターフェースを返します。
コレクションクラスを自作する場合、この<code>IEnumerator</code>インターフェースを実装する<em>列挙子</em>も自作する必要があります。

<code>IEnumerator</code>インターフェースには<code>Current</code>というプロパティと<code>MoveNext</code>、<code>Reset</code>という2つのメソッドがあります。
<code>Current</code>プロパティはコレクション内の現在の要素を取得するためのもので、
<code>MoveNext</code>メソッドは列挙子をコレクションの次の要素に進めます。
また、<code>Reset</code>メソッドは列挙子を初期位置、つまりコレクションの最初の要素の前に戻します。

<pre class="source" title="コレクションクラスと列挙子の自作の例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;

<span class="comment">/// &lt;summary&gt;
/// 片方向連結リストクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> LinearList : <em>IEnumerable</em>
{
  <span class="comment">/// &lt;summary&gt;
  /// 連結リストのセル
  /// &lt;/summary&gt;</span>
  <span class="reserved">private class</span> Cell
  {
    <span class="reserved">public</span> object value;
    <span class="reserved">public</span> Cell next;

    <span class="reserved">public</span> Cell(object value, Cell next)
    {
      <span class="reserved">this</span>.value = value;
      <span class="reserved">this</span>.next = next;
    }
  }

  <span class="comment">/// &lt;summary&gt;
  /// <em>LinearList の列挙子</em>
  /// &lt;/summary&gt;</span>
  <span class="reserved">private class</span> LinearListEnumerator : <em>IEnumerator</em>
  {
    <span class="reserved">private</span> LinearList list;
    <span class="reserved">private</span> Cell current;

    <span class="reserved">public</span> LinearListEnumerator(LinearList list)
    {
      <span class="reserved">this</span>.list = list;
      <span class="reserved">this</span>.current = <span class="reserved">null</span>;
    }

    <span class="comment">/// &lt;summary&gt;
    /// コレクション内の現在の要素を取得
    /// &lt;/summary&gt;</span>
    <span class="reserved">public</span> object <em>Current</em>
    {
      <span class="reserved">get</span>{<span class="reserved">return this</span>.current.value;}
    }

    <span class="comment">/// &lt;summary&gt;
    /// 列挙子をコレクションの次の要素に進める
    /// &lt;/summary&gt;</span>
    <span class="reserved">public bool</span> <em>MoveNext</em>()
    {
      <span class="reserved">if</span>(<span class="reserved">this</span>.current == <span class="reserved">null</span>)
        <span class="reserved">this</span>.current = <span class="reserved">this</span>.list.head;
      <span class="reserved">else
        this</span>.current = <span class="reserved">this</span>.current.next;

      <span class="reserved">if</span>(<span class="reserved">this</span>.current == <span class="reserved">null</span>)
        <span class="reserved">return false</span>;
      <span class="reserved">return true</span>;
    }

    <span class="comment">/// &lt;summary&gt;
    /// 列挙子を初期位置に戻す
    /// &lt;/summary&gt;</span>
    <span class="reserved">public void</span> <em>Reset</em>()
    {
      <span class="reserved">this</span>.current = <span class="reserved">null</span>;
    }
  }

  <span class="reserved">private</span> Cell head;

  <span class="reserved">public</span> LinearList()
  {
    head = <span class="reserved">null</span>;
  }

  <span class="comment">/// &lt;summary&gt;
  /// リストに新しい要素を追加
  /// &lt;/summary&gt;</span>
  <span class="reserved">public void</span> Add(object value)
  {
    head = <span class="reserved">new</span> Cell(value, head);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 列挙子を取得
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> IEnumerator <em>GetEnumerator</em>()
  {
    <span class="reserved">return new</span> LinearListEnumerator(<span class="reserved">this</span>);
  }
}

<span class="reserved">class</span> ForeachSample
{
  <span class="reserved">static void</span> Main()
  {
    LinearList list = <span class="reserved">new</span> LinearList();

    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;10; ++i)
    {
      list.Add(i * (i + 1) / 2);
    }

    <span class="reserved">foreach</span>(<span class="reserved">int</span> s <span class="reserved">in</span> list)
    {
      Console.Write(s + <span class="literal">" "</span>);
    }
  }
}
</code></pre>


<pre class="console" title="">
45 36 28 21 15 10 6 3 1 0 
</pre>


<h5 class="version version2">Ver. 2.0</h5>

このようなコレクションクラスを自作する作業は結構面倒なんですが、
C# 2.0 ではこの作業を簡単化するための「[イテレーター](sp2_iterator.md#iterator)」という機能が追加されました。
詳しくは、「[イテレーター](sp2_iterator.md)」で説明します。


## <a id="sec-generated-title-9"></a> <a id="performance"></a>foreach 文のパフォーマンス

「[foreach文とは](#foreach)」で説明したように、
一般には、foreach 文は以下のようなコードに展開されます。
（IDispose を実装しない場合。
IDispose を実装するクラスの場合には、
さらに「[using ステートメント](../resource/oo_dispose.md#using)」で囲ったのと同じ扱いになります。）

<pre class="source" title="foreachの実態" lang="">
<code>IEnumerator e = array.GetEnumerator();
<span class="reserved">while</span>(e.MoveNext())
{
  <span class="input">型名</span> <span class="input">変数</span> = (<span class="input">型名</span>)e.Current;
  <span class="input">文</span>
}
</code></pre>


このコードだと、
MoveNext() や Current などのメソッド呼び出しのオーバーヘッドが結構大きくて、
<code>for(int i; i &lt; array.Length; ++i) 文;</code>
というようなコードに比べると少し実行効率が悪くなります。

ただ、配列に対して foreach を使った場合、
最適化がかかって for 文相当のコードに変換されるようで、
そこまで大きな差はなくなるようです。

## <a id="sec-generated-title-10"></a> <a id="await-foreach"></a>非同期 foreach

<h5 class="version version8">Ver. 8.0</h5>

C# 8.0で非同期版の`foreach`が追加されました。
`await foreach` (`foreach`の前に`await`を付ける)という構文で、
[`IAsyncEnumerable<T>`](https://docs.microsoft.com/ja-jp/dotnet/api/system.collections.generic.iasyncenumerable-1)インターフェイス(`System.Collections.Generic`名前空間)か、それと同じ[パターン](../misc/miscpatternbased.md)を満たす型の列挙ができます。

<pre class="source" title="非同期 foreach">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">AsyncForeach</span>(<span class="type">IAsyncEnumerable</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">items</span>)
{
    <em><span class="reserved">await</span> <span class="control">foreach</span></em> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable">items</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">item</span>);
    }
}
</code></pre>

詳しくは「[非同期foreach](../async/asyncstream.md#await-foreach)」で説明します。
