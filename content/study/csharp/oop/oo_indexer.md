---
title: "インデクサー"
source_url: "https://ufcpp.net/study/csharp/oop/oo_indexer/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2015-02-16T00:00:00"
tags: []
umbraco_id: 1261
parent_id: 1248
sort_order: 8
aliases:
  - "/csharp/oo_indexer"
  - "/csharp/oo_indexer.html"
  - "/csharp/oop/oo_indexer/"
  - "/study/csharp/oo_indexer"
  - "/study/csharp/oo_indexer.html"
---

# インデクサー

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# で利用できる基本型の1つに配列があります。
配列では i 番目の要素を読み書きする際、
<code>a[i]</code> というように <code>[]</code> を用います。

「[演算子のオーバーロード](oo_operator.md)」でも述べましたが、
ユーザー定義型の理想は、組込み型とまったく同じように扱えることです。
そこで、C# では、
ユーザー定義型が配列型と同様に <code>[]</code> を用いた要素の読み書きが行えるように<strong id="indexer" class="keyword">インデクサー</strong>という仕組みが用意されています。

インデクサーを定義することで、ユーザー定義型のオブジェクトでも、
配列と同じような <code>a[i]</code> という形での要素の読み書きができるようになります。


##### <a id="sec-generated-title-2"></a>ポイント

* 組み込み型（int や string など）とユーザー定義型（クラスや構造体）の区別をなくそう。

* ユーザー定義型にも、配列っぽく<code>[]</code>を使ったインデックスアクセスを定義できます（インデクサー）。

* 書き方は、T this [int index] { set { ... } get { ... } }



## <a id="sec-generated-title-3"></a> <a id="definition"></a>インデクサーの定義

インデクサーは以下のようにして定義します。

<pre class="source" title="" lang="">
<code><span class="input">アクセスレベル</span> <span class="input">戻り値の型</span> <span class="reserved">this</span>[<span class="input">添字の型</span> <span class="input">添字</span>]
{
  <span class="reserved">set</span>
  {
    <span class="comment">// setアクセサ
    //  ここに値の変更時の処理を書く。
    //  value という名前の変数に代入された値が格納される。
    //  添字が使える以外はプロパティと同じ。</span>
  }
  <span class="reserved">get</span>
  {
    <span class="comment">// getアクセサ
    //  ここに値の取得時の処理を書く。
    //  メソッドの場合と同様に、値はreturnキーワードを用いて返す。
    //  こっちも添字が使える以外はプロパティと同じ。</span>
  }
}
</code></pre>


インデクサーの定義の仕方はプロパティの定義の仕方に似ています。
プロパティ名の代わりに <code>this[]</code> を使うことと、
添字が使えること以外はプロパティと同じです。

例えば、以下のように添字の下限と上限の両方を指定できる配列を作ることが出来ます。

<pre class="source" title="インデクサーの例1" lang="">
<code><span class="reserved">using</span> System;

<span class="inactive">/// &lt;summary&gt;
///</span><span class="comment"> 添字の下限と上限を指定できる配列。</span>
<span class="inactive">/// &lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">BoundArray</span>
{
    <span class="reserved">int</span>[] array;
    <span class="reserved">int</span> lower;   <span class="comment">// 配列添字の下限</span>

    <span class="reserved">public</span> BoundArray(<span class="reserved">int</span> lower, <span class="reserved">int</span> upper)
    {
        <span class="reserved">this</span>.lower = lower;
        array = <span class="reserved">new int</span>[upper - lower + 1];
    }

    <span class="reserved">public int this</span>[<span class="reserved">int</span> i]
    {
        <span class="reserved">set</span> { <span class="reserved">this</span>.array[i - lower] = <span class="reserved">value</span>; }
        <span class="reserved">get</span> { <span class="reserved">return this</span>.array[i - lower]; }
    }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="type">BoundArray</span> a = <span class="reserved">new</span> <span class="type">BoundArray</span>(1, 9);

        <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt;= 9; ++i)
            a[i] = i;

        <span class="reserved">for</span> (<span class="reserved">int</span> i = 1; i &lt;= 9; ++i)
            <span class="type">Console</span>.Write(<span class="literal">"a[{0}] = {1}\n"</span>, i, a[i]);
    }
}
</code></pre>


<pre class="console" title="">
a[1] = 1
a[2] = 2
a[3] = 3
a[4] = 4
a[5] = 5
a[6] = 6
a[7] = 7
a[8] = 8
a[9] = 9
</pre>


インデクサーの添字は1つである必要はなく、
複数の添字を利用することが出来ます。

<pre class="source" title="インデクサーの例2 複数の添字" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// jagged array を使った行列。
/// rectangular array のように[i, j]という添字で要素の読み書き可能。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Matrix
{
  <span class="reserved">int</span>[][] array;

  <span class="reserved">public</span> Matrix(<span class="reserved">int</span> rows, <span class="reserved">int</span> cols)
  {
    <span class="reserved">this</span>.array = <span class="reserved">new int</span>[rows][];
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;rows; ++i)
      <span class="reserved">this</span>.array[i] = <span class="reserved">new int</span>[cols];
  }

  <em><span class="reserved">public int this</span>[<span class="reserved">int</span> i, <span class="reserved">int</span> j]</em>
  {
    <span class="reserved">set</span>{<span class="reserved">this</span>.array[i][j] = value;}
    <span class="reserved">get</span>{<span class="reserved">return this</span>.array[i][j];}
  }
}

<span class="reserved">class</span> IndexerSample
{
  <span class="reserved">static void</span> Main()
  {
    Matrix a = <span class="reserved">new</span> Matrix(4, 4);

    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;4; ++i)
      <span class="reserved">for</span>(<span class="reserved">int</span> j=0; j&lt;4; ++j)
        a[i, j] = (i+1) * (j+3);

    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;4; ++i)
    {
      <span class="reserved">for</span>(<span class="reserved">int</span> j=0; j&lt;4; ++j)
        Console.Write(<span class="literal">"{0,4}"</span>, a[i, j]);
      Console.Write(<span class="literal">"\n"</span>);
    }
  }
}
</code></pre>


<pre class="console" title="">
   3   4   5   6
   6   8  10  12
   9  12  15  18
  12  16  20  24
</pre>


また、添字の型は整数型である必要はありません。
例えば、以下のように添字が <code>string</code> 型のインデクサーを持つ辞書クラスを作ることも出来ます。

<pre class="source" title="インデクサーの例2 string 型の添字" lang="">
<code><span class="reserved">using</span> System;

<span class="comment">/// &lt;summary&gt;
/// Dictionary クラスの項目。
/// &lt;/summary&gt;</span>
<span class="reserved">internal class</span> Item
{
  <span class="reserved">public string</span> key;
  <span class="reserved">public string</span> value;
  <span class="reserved">public</span> Item next;

  <span class="reserved">public</span> Item(<span class="reserved">string</span> key, <span class="reserved">string</span> value, Item next)
  {
    <span class="reserved">this</span>.key = key;
    <span class="reserved">this</span>.value = value;
    <span class="reserved">this</span>.next = next;
  }
}

<span class="comment">/// &lt;summary&gt;
/// 辞書クラス。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> Dictionary
{
  Item head;

  <span class="reserved">public</span> Dictionary()
  {
    <span class="reserved">this</span>.head = <span class="reserved">new</span> Item(<span class="reserved">null</span>, <span class="reserved">null</span>, <span class="reserved">null</span>);
  }

  <span class="reserved">public string this</span>[<span class="reserved">string</span> key]
  {
    <span class="reserved">set</span>
    {
      <span class="reserved">for</span>(Item item = <span class="reserved">this</span>.head.next; item != <span class="reserved">null</span>; item =item.next)
        <span class="reserved">if</span>(item.key == key)
        {
          item.value = value;
          <span class="reserved">return</span>;
        }
      <span class="reserved">this</span>.head.next = <span class="reserved">new</span> Item(key, value, <span class="reserved">this</span>.head.next);
    }
    <span class="reserved">get</span>
    {
      <span class="reserved">for</span>(Item item = <span class="reserved">this</span>.head.next; item != <span class="reserved">null</span>; item =item.next)
        <span class="reserved">if</span>(item.key == key)
          <span class="reserved">return</span> item.value;
      <span class="reserved">return null</span>;
    }
  }
}

<span class="reserved">class</span> IndexerSample
{
  <span class="reserved">static void</span> Main()
  {
    Dictionary dic = <span class="reserved">new</span> Dictionary();

    dic[<span class="literal">"ﾊｧ"</span>]    = <span class="literal">"( ﾟДﾟ)？"</span>;
    dic[<span class="literal">"ﾊｧﾊｧ"</span>]  = <span class="literal">"(;´Д｀)"</span>;
    dic[<span class="literal">"ﾎﾟｶｰﾝ"</span>] = <span class="literal">"( ﾟдﾟ)"</span>;
    dic[<span class="literal">"ｵﾏｴﾓﾅ"</span>] = <span class="literal">"(´∀｀)"</span>;

    Console.Write(dic[<span class="literal">"ﾊｧﾊｧ"</span>]);
  }
}
</code></pre>


<pre class="console" title="">
(;´Д｀)
</pre>



## <a id="sec-generated-title-4"></a> <a id="level"></a>set/get で異なるアクセスレベルを設定

<h5 class="version version2">Ver. 2.0</h5>

C# 2.0 では、
「[プロパティ](oo_property.md#property)」と同様に、
インデクサーの set/get アクセサそれぞれ異なるアクセスレベルを設定できるようになりました。

<pre class="source" title="set/get で異なるアクセスレベル" lang="">
<code><span class="reserved">int</span>[] x;
<span class="reserved">public int this</span>[<span class="reserved">int</span> i]
{
    <span class="reserved">get</span> { <span class="reserved">return</span> x[i]; }
    <span class="reserved"><em>private</em> set</span> { x[i] = <span class="reserved">value</span>; }
}
</code></pre>



## <a id="sec-generated-title-5"></a> <a id="indexed"></a>余談: VB のインデックス付きプロパティ

余談なんですが、
VB.NET なんかにはインデックス付きプロパティというものもあります。
（VB.NET の他、Delphi には配列プロパティという名前で同様な機能が。）
VB.NET にあって C# にない機能の代表格としてよく挙がるんですが、
C# にこの機能がない理由についてちょっと考えてみます。

（先に「[foreach](../data/sp_foreach.md)」とか「[コレクション](../lib/lib_other.md#collection)」、「[コレクション概要](../../algorithm/collection/collection.md)」の辺りを読んでおくといいかも。）

VB.NET のプロパティの構文は以下のような感じです。
（例として整数型のプロパティを作るなら）

<pre class="source" title="VB.NET のプロパティ（定義側）" lang="">
<code>Public Property X() As Integer
  Get
    Return x_
  End Get
  Set
    x_ = value
  End Set
End Property
</code></pre>


<pre class="source" title="VB.NET のプロパティ（利用側）" lang="">
<code>obj.X = 0
</code></pre>


で、VB の場合はプロパティが引数を取れます。

<pre class="source" title="VB.NET のインデックス付きプロパティ（定義側）" lang="">
<code>Public Property X(i As Integer) As Integer
  Get
    Return x_(i)
  End Get
  Set
    x_(i) = value
  End Set
End Property
</code></pre>


<pre class="source" title="VB.NET のインデックス付きプロパティ（利用側）" lang="">
<code>obj.X(0) = 0
</code></pre>


この構文、ある意味「名前付きインデクサー」ともいえます。
C# のインデクサーの構文は、なんか意味の分からない所に this が入って、
以下のような書き方をするわけですが、

<pre class="source" title="C# のインデクサー" lang="">
<code><span class="reserved">int</span>[] x;

<span class="reserved">public int this</span>[<span class="reserved">int</span> i]
  {
    <span class="reserved">get</span>{<span class="reserved">return this</span>.x[i];}
  }
}
</code></pre>


この、this の部分に自由な名前を書けるのが VB.NET のインデックス付きプロパティだと思ってください。
this を使うときには（要するにインデクサー） obj[i] という記法で、
名前を付けたときには obj.Name[i] という記法でインデクサーを使える。

まあ、便利そうな機能ではあるんですけど、
なぜか C# にはない。
「これだけは VB.NET が便利」なんてことも言われるんですが、
C# がインデックス付きプロパティ（あるいは、名前付きインデクサー）を採用しなかった理由ってのも、
想像付かなくはないんですよね。

多分なんですけど、
C# の言語設計者的には、
インデックス付きプロパティにするよりも、
コレクションクラス（配列とかのこと）を返すプロパティを使って欲しいんだと思います。
要するに、以下のような。

<pre class="source" title="C# でインデックス付きプロパティ相当のことをしたい場合" lang="">
<code><span class="reserved">int</span>[] x;

<span class="reserved">public int</span>[] X
  {
    <span class="reserved">get</span>{<span class="reserved">return this</span>.x;}
  }
}
</code></pre>


単なる配列じゃなくて、もうちょっと細かい挙動をちゃんと書きたければ、
自分で ICollection なり IList なりを実装した内部クラスを書けと。

結構面倒なんですけど、
C# 設計者がそうして欲しかった理由は、
名前付きインデクサーだと foreach とかが使えないから。
例えば、以下のような利用側コードは、
コレクションを返すプロパティなら OK なんですが、
インデックス付きプロパティではできない。
（参考： 「[foreach](../data/sp_foreach.md)」。）

<pre class="source" title="インデックス付きプロパティでは foreach が使えない" lang="">
<code><span class="reserved">foreach</span>(<span class="reserved">int</span> val <span class="reserved">in</span> obj.X)
{
  Console.Write(<span class="literal">"{0}\n"</span>, val);
}
</code></pre>


ちなみに、インデックス付きプロパティではなくて、
C# 2.0 で導入されたイテレータ構文を使ったプロパティなら、
簡単に作れて、かつ、foreach で使えます。
（参考： 「[イテレーター](../data/sp2_iterator.md)」。）

また勝手な推測が交じるんですが、
多分、C# の開発者は、C# 1.0 の設計段階からイテレータのアイディアをなんとなく持っていて、
そのためにあえてインデックス付きプロパティを C# に導入しなかったんじゃないかと。
