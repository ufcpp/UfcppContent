---
title: "イテレーター"
source_url: "https://ufcpp.net/study/csharp/data/sp2_iterator/"
content_type: "Article"
published_at: "2005-09-19T00:00:00"
updated_at: "2010-11-03T00:00:00"
tags:
  - "Ver. 2.0"
umbraco_id: 1300
parent_id: 1298
sort_order: 1
aliases:
  - "/csharp/data/sp2_iterator/"
  - "/csharp/sp2_iterator"
  - "/csharp/sp2_iterator.html"
  - "/study/csharp/sp2_iterator"
  - "/study/csharp/sp2_iterator.html"
---

# イテレーター

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# の foreach 構文は、コレクションクラスの利用者側から見ると非常に便利な機能です。
しかしながら、実装側から見た場合、<code>IEnumerable</code>や<code>IEnumerator</code>インターフェースを実装する必要があり、結構面倒な作業が必要でした。

この実装側の労力を軽減するために、C# 2.0ではイテレーター構文というものが追加されました。


##### <a id="sec-generated-title-2"></a>ポイント
* イテレーター構文： IEnumerator を簡単に実装するための機能。

* return の代わりに yield return



##<a id="sec-generated-title-3"></a> <a id="block"></a>イテレーター ブロック
メソッドやプロパティのgetアクセサーなどを定義する際、ブロック中に`return`の代わりに`yield return`もしくは`yield break`を書くことで、通常のメソッドやプロパティとは違った動作が得られます。この、`yield return`もしくは`yield break`を含むブロックのことを<strong id="iterator" class="keyword">イテレーター</strong> ブロック（iterator block）と言いいます。

イテレーター ブロックを使うことで、「[foreach 文](sp_foreach.md#foreach)」で利用可能なコレクションを返すメソッドやプロパティを簡単に実装することができます。

<pre class="source" title="イテレーター ブロック" lang="">
<code><span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> TestEnumerable
{
  <span class="comment">// ↓これがイテレーター ブロック。IEnumerable を実装するクラスを自動生成してくれる。</span>
<em>  <span class="reserved">static public</span> IEnumerable&lt;<span class="reserved">int</span>&gt; FromTo(<span class="reserved">int</span> from, <span class="reserved">int</span> to)
  {
    <span class="reserved">while</span>(from &lt;= to)
      <span class="reserved">yield return</span> from++;
  }</em>

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <span class="comment">// ↓こんな感じで使う。</span>
    <em><span class="reserved">foreach</span>(<span class="reserved">int</span> i <span class="reserved">in</span> FromTo(10, 20))</em>
    {
      Console.Write(<span class="literal">"{0}\n"</span>, i);
    }
  }
}
</code></pre>


ちなみに、yield という単語は「譲る」という意味です
（車文化のアメリカでは、「車線を譲る」（他の車を通すために速度を落としたり、脇道に止めたり）の意味でよく使われます）。
イテレーター ブロックの場合、"yield control to another method"（制御フローを他の処理に譲る）というような意味合いになります。

通常のブロック(メソッドやプロパティgetアクセサーの本体)との違いは以下の通りです。

* 戻り値の型が以下のうちのいずれか
    * System.Collections.IEnumerator

    * System.Collections.Generic.IEnumerator&lt;T&gt;

    * System.Collections.IEnumerable

    * System.Collections.Generic.IEnumerable&lt;T&gt;



* return の変わりに yield return というキーワードを使う。

* break の変わりに yield break というキーワードを使う。


上述の例の通り、
イテレーター ブロック中で、yield return 文が呼ばれるたびに、
foreach 文中で使われる値を1つ得ます。
for 文や while 文を使わず、ベタに yield return を並べても OK です。

<pre class="source" title="イテレーター ブロック" lang="">
<code><span class="reserved">static public</span> IEnumerable GetEnumerable(<span class="reserved">int</span> from, <span class="reserved">int</span> to)
{
  <span class="reserved">yield return</span> 1;
  <span class="reserved">yield return</span> 3.14;
  <span class="reserved">yield return</span> <span class="literal">"文字列"</span>;
  <span class="reserved">yield return new</span> System.Drawing.Point(1, 2);
  <span class="reserved">yield return</span> 1.0f;
}
</code></pre>


また、yield break を記述した行まで処理が進むと、イテレーターの処理をそこで終了します。

イテレーター ブロックは静的（static）なものでもインスタンス（非 static）でも、
どちらでも定義できます。
また、プロパティ風の記述も可能です。
上述の例は static なメソッドですが、以下のような非 static なプロパティ風の定義も可能です。

<pre class="source" title="非 static プロパティ風イテレーター ブロック" lang="">
<code><span class="reserved">class</span> FromTo
{
  <span class="reserved">int</span> from, to;
  <span class="reserved">public</span> FromTo(<span class="reserved">int</span> from, <span class="reserved">int</span> to){<span class="reserved">this</span>.from = from; <span class="reserved">this</span>.to = to;}

<em>  <span class="reserved">public</span> IEnumerable&lt;<span class="reserved">int</span>&gt; Enumerable
  {
    <span class="reserved">get</span>
    {
      <span class="reserved">while</span>(from &lt;= to)
        <span class="reserved">yield return</span> from++;
    }
  }</em>

  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    <em><span class="reserved">foreach</span>(<span class="reserved">int</span> i <span class="reserved">in new</span> FromTo(10, 20).Enumerable)</em>
    {
      Console.Write(<span class="literal">"{0}\n"</span>, i);
    }
  }
}
</code></pre>

##<a id="sec-generated-title-4"></a> <a id="restriction"></a>イテレーターの制限
イテレーター ブロックは、戻り値を返せるような関数メンバー(メソッド、演算子、プロパティのget、インデクサーのget)なら基本的には何に対してでも使えます。

ただし、いくつか制限があります。

まず、以下のような制限があります。

- [unsafe](../interop/sp_unsafe.md)にはできない。
  - 関数メンバーにunsafe修飾子は付けれない。
  - イテレーター ブロック内にunsafeステートメントは書けない<sup>※</sup>
- 引数を[ref, out](../resource/sp_ref.md)にはできない。
- [ref ローカル変数](../resource/sp_ref.md#ref-returns)を書けない<sup>※</sup>

(<sup>※</sup> このうち unsafe ステートメントと ref ローカル変数は、[C# 13 で書けるように](../cheatsheet/ap_ver13.md#ref-in-async)なりました。)

また、以下の場所には`yield return`、`yield break`共に書けません。

- finally 句内
- [匿名関数](../functional/sp_delegate.md#anonymous)の中
  - 匿名なイテレーター ブロック自体作れません。

そして、以下の場所には`yield return`を書けません。

- catch 句を持つ try 句内
  - (finally 句のみを持つ try 句内には`yield return`を書けます)
- catch 句内

##<a id="sec-generated-title-5"></a> <a id="GetEnum"></a>GetEnumerator
「[コレクションクラスの自作](sp_foreach.md#ownmaking)」で説明したように、
通常、foreach 文で利用できるコレクションクラスを自作するには、
IEnumerable インターフェースを継承し、
GetEnumerator メソッドをオーバーライドします。

C# 2.0 ではこのような方法の他に、
GetEnumerator と言う名前のイテレーター ブロックを定義することでも
コレクションクラスを作成できます。
ここでは、「[ジェネリック](../oop/sp2_generics.md)」で例に挙げた Stack クラスにイテレーターを追加してみましょう。

<pre class="source" title="GetEnumerator イテレーター ブロック" lang="">
<code><span class="reserved">class</span> Stack&lt;Type&gt;
{
  Type[] buf;
  <span class="reserved">int</span> top;
  <span class="reserved">public</span> Stack(<span class="reserved">int</span> max) { <span class="reserved">this</span>.buf = <span class="reserved">new</span> Type[max]; <span class="reserved">this</span>.top = 0; }
  <span class="reserved">public void</span> Push(Type item) { <span class="reserved">this</span>.buf[<span class="reserved">this</span>.top++] = item; }
  <span class="reserved">public</span> Type Pop() { <span class="reserved">return this</span>.buf[--<span class="reserved">this</span>.top]; }

<em>  <span class="reserved">public</span> IEnumerator&lt;Type&gt; GetEnumerator()
  {
    <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="reserved">this</span>.top - 1; i &gt;= 0; --i)
      <span class="reserved">yield return</span> buf[i];
  }</em>
}
</code></pre>



##### <a id="sec-generated-title-6"></a>サンプル
「[foreach](sp_foreach.md)」で挙げた例を、
ジェネリックスとイテレーターを用いて書き直してみます。

<pre class="source" title="イテレーターの例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="comment">/// &lt;summary&gt;
/// 片方向連結リストクラス
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> LinearList&lt;T&gt;
{
  <span class="comment">/// &lt;summary&gt;
  /// 連結リストのセル
  /// &lt;/summary&gt;</span>
  <span class="reserved">private class</span> Cell
  {
    <span class="reserved">public</span> T value;
    <span class="reserved">public</span> Cell next;

    <span class="reserved">public</span> Cell(T value, Cell next)
    {
      <span class="reserved">this</span>.value = value;
      <span class="reserved">this</span>.next = next;
    }
  }

  <span class="reserved">private</span> Cell head;

  <span class="reserved">public</span> LinearList()
  {
    <span class="reserved">this</span>.head = <span class="reserved">null</span>;
  }

  <span class="comment">/// &lt;summary&gt;
  /// リストに新しい要素を追加
  /// &lt;/summary&gt;</span>
  <span class="reserved">public void</span> Add(T value)
  {
    <span class="reserved">this</span>.head = <span class="reserved">new</span> Cell(value, head);
  }

  <span class="comment">/// &lt;summary&gt;
  /// 列挙子を取得
  /// &lt;/summary&gt;</span>
  <span class="reserved">public</span> IEnumerator&lt;T&gt; GetEnumerator()
  {
    <span class="reserved">for</span>(Cell c = <span class="reserved">this</span>.head; c != <span class="reserved">null</span>; c = c.next)
    {
      <span class="reserved">yield return</span> c.value;
    }
  }
}

<span class="reserved">class</span> ForeachSample
{
  <span class="reserved">static void</span> Main()
  {
    LinearList&lt;<span class="reserved">int</span>&gt; list = <span class="reserved">new</span> LinearList&lt;<span class="reserved">int</span>&gt;();

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



##<a id="sec-generated-title-7"></a> <a id="complied"></a>イテレーターのコンパイル結果
イテレーターは、
コレクションクラスを実装する際の手間が大幅に削減できる、
非常に便利な機能です。
ですが、少々抽象度が高く、イテレーター ブロックのコンパイル結果がどうなるのか、
ちょっと想像しづらいと思います。

中には、
中身の分からないものを使うのが怖いという方もいらっしゃるでしょうし、
怖いとまでは言わないものの、少しでもプログラムの効率をよくするために、
コンパイル結果がどうなるかを知りたいと言う方は多いと思います。
なので、イテレーター ブロックのコンパイル結果について少し触れておきます。
（ちなみに、C# 2.0 の仕様書中にも、このコンパイル結果に関する記事があります。）

イテレーターのコンパイル結果ですが、コンパイラが頑張ってくれていて、
結構凄いことをしています。
一種の状態機械（state machine）を自動生成していて、
例えば、先ほど例に挙げた Stack なら以下のようなコードと等価になるそうです。

<pre class="source" title="イテレーターのコンパイル結果（と等価なコード）" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Collections;

<span class="reserved">class</span> Stack&lt;T&gt; : IEnumerable&lt;T&gt;
{
  T[] buf;
  <span class="reserved">int</span> top;
  <span class="reserved">public</span> Stack(<span class="reserved">int</span> max) { <span class="reserved">this</span>.buf = <span class="reserved">new</span> T[max]; <span class="reserved">this</span>.top = 0; }
  <span class="reserved">public void</span> Push(T item) { <span class="reserved">this</span>.buf[<span class="reserved">this</span>.top++] = item; }
  <span class="reserved">public</span> T Pop() { <span class="reserved">return this</span>.buf[--<span class="reserved">this</span>.top]; }

  <span class="reserved">public</span> IEnumerator&lt;T&gt; GetEnumerator() {
    <span class="reserved">return new</span> __Enumerator1(<span class="reserved">this</span>);
  }
  <span class="reserved">class</span> __Enumerator1: IEnumerator&lt;T&gt;, IEnumerator
  {
    <span class="reserved">int</span> __state;
    T __current;
    Stack&lt;T&gt; __this;
    <span class="reserved">int</span> i;

    <span class="reserved">public</span> __Enumerator1(Stack&lt;T&gt; __this)
    {
      <span class="reserved">this</span>.__this = __this;
    }

    <span class="reserved">public</span> T Current
    {
      <span class="reserved">get</span> { <span class="reserved">return</span> __current; }
    }

    <span class="reserved">object</span> IEnumerator.Current
    {
      <span class="reserved">get</span> { <span class="reserved">return</span> __current; }
    }

    <span class="reserved">public bool</span> MoveNext()
    {
      <span class="reserved">switch</span> (__state)
      {
        <span class="reserved">case</span> 1: <span class="reserved">goto</span> __state1;
        <span class="reserved">case</span> 2: <span class="reserved">goto</span> __state2;
      }
      i = __this.top - 1;

    __loop:
      <span class="reserved">if</span> (i &lt; 0) <span class="reserved">goto</span> __state2;
      __current = __this.buf[i];
      __state = 1;
      <span class="reserved">return true</span>;

    __state1:
      --i;
      <span class="reserved">goto</span> __loop;

    __state2:
      __state = 2;
      <span class="reserved">return false</span>;
    }
    <span class="reserved">public void</span> Dispose()
    {
      __state = 2;
    }

    <span class="reserved">void</span> IEnumerator.Reset()
    {
      <span class="reserved">throw new</span> NotSupportedException();
    }
  }
}
</code></pre>


C# 2.0 コンパイラは、
イテレーター ブロック内の for 文を、
この MoveNext メソッド内のようなコードに展開してくれるそうです。
やっていることを簡単に言うと、<code>yield return x;</code> の部分を以下のように置き換えています。

<pre class="source" title="yield return の置き換え" lang="">
<code>state = State1; <span class="comment">// 次に復帰するときのための状態の記録</span>
Current = x;    <span class="comment">// 戻り値を Current に保持</span>
<span class="reserved">return</span> true;    <span class="comment">// いったん処理終了</span>
<span class="reserved">case</span> State1:    <span class="comment">// 次に呼ばれたときに続きから処理するためのラベル</span>
</code></pre>

(疑似コードです。実際の C# では `case` に変数は使えないので、
「これに相当する `goto` が生成される」くらいのものだと思って読んでください。)

そして、最後に、これを switch 文で囲う(に相当する`goto`が挿入される)ことで、
処理の一時中断と再開を実現します。

ちなみに、このコードを見ての通り、
イテレーター ブロックによって得た IEnumerator は、
実は Reset メソッドをサポートしていません。
Reset を呼ぼうとすると NotSupportedException がスローされます。


##<a id="sec-generated-title-8"></a> <a id="dispose"></a>リソースの破棄
「[リソースの破棄](../resource/oo_dispose.md)」で説明したように、
ファイルなどの、.NET Framework の「[ガーベジコレクション](../cs4j/ab_csspec.md#gc)」の管理対象外のリソースは明示的な破棄が必要です。

リソースの破棄は、Dispose() メソッドなどを直接呼び出すことでもできますが、
以下のように、イテレーター ブロック中で Dispose() を呼び出しても、
正しく呼び出されない場合があります。

<pre class="source" title="不適切なリソース破棄" lang="">
<code><span class="reserved">static</span> IEnumerable&lt;<span class="reserved">string</span>&gt; Lines(<span class="reserved">string</span> path)
{
  System.IO.StreamReader sr = <span class="reserved">new</span> System.IO.StreamReader(path);

  <span class="reserved">string</span> line;
  <span class="reserved">while</span> ((line = sr.ReadLine()) != <span class="reserved">null</span>)
  {
    <span class="reserved">yield return</span> line;
  }

  <em>sr.Dispose(); <span class="comment">// この行は呼ばれないことがある</span></em>
}
</code></pre>

利用側の`foreach`ループに`break`などを書くと、`yield return`から後ろが実行されなくなります。
以下の例のように、`break`を1つ追加するだけで、イテレーター ブロック内の最後の1行が実行されなくなります。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Iterator()
{
    <span class="type">Console</span>.Write(<span class="string">"1の前 "</span>);
    <span class="reserved">yield</span> <span class="reserved">return</span> 1;
    <span class="type">Console</span>.Write(<span class="string">"1の後 "</span>);
}

<span class="reserved">static</span> <span class="reserved">void</span> Foreach()
{
    <span class="reserved">var</span> items = Iterator();

    <span class="comment">// こちらのループの結果: 1の前 1を消費 1の後</span>
    <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> items)
    {
        <span class="type">Console</span>.Write(<span class="string">$"</span>{item}<span class="string">を消費 "</span>);
    }

    <span class="comment">// こちらのループの結果: 1の前 1を消費</span>
    <span class="comment">// (yield return より後ろ(1の後)は実行されない)</span>
    <span class="reserved">foreach</span> (<span class="reserved">var</span> item <span class="reserved">in</span> items)
    {
        <span class="type">Console</span>.Write(<span class="string">$"</span>{item}<span class="string">を消費"</span>);
        <span class="reserved">break</span>; <span class="comment">// break 1つで挙動が変わる</span>
    }
}
</code></pre>

正しく sr.Dispose(); が呼ばれるようにしたければ、
イテレーター ブロック内で「[try-catch-finally 文](../structured/oo_exception.md#try)」や「[using ステートメント](../resource/oo_dispose.md#using)」を使います。

<pre class="source" title="using を使ったリソース破棄" lang="">
<code><span class="reserved">static</span> IEnumerable&lt;<span class="reserved">string</span>&gt; Lines(<span class="reserved">string</span> path)
{
  <span class="reserved">using</span> (System.IO.StreamReader sr = <span class="reserved">new</span> System.IO.StreamReader(path))
  {
    <span class="reserved">string</span> line;
    <span class="reserved">while</span> ((line = sr.ReadLine()) != <span class="reserved">null</span>)
    {
      <span class="reserved">yield return</span> line;
    }
  }
}
</code></pre>
