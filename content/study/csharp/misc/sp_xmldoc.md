---
title: "XML Document"
source_url: "https://ufcpp.net/study/csharp/misc/sp_xmldoc/"
content_type: "Article"
published_at: "2015-05-06T14:12:32"
updated_at: "2024-08-31T17:24:47"
tags: []
umbraco_id: 1340
parent_id: 1338
sort_order: 2
aliases:
  - "/csharp/misc/sp_xmldoc/"
  - "/csharp/sp_xmldoc"
  - "/csharp/sp_xmldoc.html"
  - "/study/csharp/sp_xmldoc"
  - "/study/csharp/sp_xmldoc.html"
---

# XML Document

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

ライブラリなどを作成し、公開する場合、ライブラリの内容を他人に理解してもらえるようにドキュメントを作成してやる必要があります。
ところが、プログラムのドキュメントを書く作業というのは結構面倒な作業です。
少しでも面倒な作業を減らせるように、C#コンパイラはC#のソースファイルをコンパイルする際に、一緒にXML形式のドキュメントを作成してくれます。

Javaをご存知の方はJavaのソースからドキュメントを生成するためのツール「javadoc」を使ったことがあるかもしれません。
C#のXML Documentationはこのjavadocと似たようなものです。
javadocとの違いは、コンパイラと別のツールとして提供されているのではなく、C#のXML DocumentationはC#コンパイラ自身に組み込まれていることと、出力形式がHTMLではなく、XMLであることです。


##### <a id="sec-generated-title-2"></a>ポイント

* /// から始まるコメントは、ソースコードからドキュメントを生成するための特別なコメントになります。

* 要するに、C#は、javadoc のような機能を標準で持っています。
    * 標準なので、コンパイラのチェックがかかります。

    * Visual Studio の支援を受けながら書くことができます。





## <a id="sec-generated-title-3"></a> <a id="ex"></a>XML Documentの例

XML Documentを理解するために、まずは実際にXML Documentを作成してみましょう。
以下のようなソースファイルをdoctest.csという名前で作成して見てください。

<pre class="source" title="doctest.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections;

<span class="reserved">namespace</span> DocumentTest
{
  <span class="comment">/// &lt;summary&gt;
  /// 簡単なサンプルとして、リストを実装する。
  /// 片方向リストで、リストに値を加えることは出来るけど、削除は出来ない。
  /// &lt;/summary&gt;</span>
  <span class="reserved">public class</span> List : IEnumerable
  {
    <span class="comment">// リストのノード</span>
    <span class="reserved">internal class</span> Node
    {
      <span class="reserved">public</span> object obj;
      <span class="reserved">public</span> Node next;

      <span class="reserved">public</span> Node(Node next)
      {
        <span class="reserved">this</span>.next = next;
        <span class="reserved">this</span>.obj = <span class="reserved">null</span>;
      }

      <span class="reserved">public</span> Node(Node next, object obj)
      {
        <span class="reserved">this</span>.next = next;
        <span class="reserved">this</span>.obj = obj;
      }
    }<span class="comment">// Node</span>

    <span class="comment">// リストのEnumerator</span>
    <span class="reserved">private class</span> ListEnumrator : IEnumerator
    {
      <span class="reserved">public</span> ListEnumrator(List list)
      {
        <span class="reserved">this</span>.list = list;
        current = list.head;
      }

      <span class="reserved">public bool</span> MoveNext()
      {
        current = current.next;
        <span class="reserved">return</span> current != <span class="reserved">null</span>;
      }

      <span class="reserved">public</span> object Current
      {
        <span class="reserved">get</span>{<span class="reserved">return</span> current.obj;}
        <span class="reserved">set</span>{current.obj = value;}
      }

      <span class="reserved">public void</span> Reset()
      {
        current = <span class="reserved">this</span>.list.head;
      }

      List list;
      Node current;
    }<span class="comment">// ListEnumerator</span>

    <span class="comment">/// &lt;summary&gt;
    /// リストの作成
    /// &lt;/summary&gt;</span>
    <span class="reserved">public</span> List()
    {
      head = <span class="reserved">new</span> Node(<span class="reserved">null</span>);
      tail = head;
    }

    <span class="comment">/// &lt;summary&gt;
    /// リストに値を加える。
    /// &lt;/summary&gt;
    /// &lt;param name="obj"&gt;加えたい値&lt;/param&gt;</span>
    <span class="reserved">public void</span> Add(object obj)
    {
      tail.next = <span class="reserved">new</span> Node(<span class="reserved">null</span>, obj);
      tail = tail.next;
    }

    <span class="comment">/// &lt;summary&gt;
    /// リストのEnumeratorを返す
    /// &lt;/summary&gt;
    /// &lt;returns&gt;リストのEnumerator&lt;/returns&gt;</span>
    <span class="reserved">public</span> IEnumerator GetEnumerator()
    {
      <span class="reserved">return new</span> ListEnumrator(<span class="reserved">this</span>);
    }

    <span class="reserved">private</span> Node head; <span class="comment">// リストのダミーヘッダー</span>
    <span class="reserved">private</span> Node tail; <span class="comment">// リストの最後尾</span>
  }<span class="comment">// List</span>
}<span class="comment">// DocumentTest</span>
</code></pre>


そして、以下のようなオプションを付けてコンパイルしてください。
Visual C#を使って作成する場合には、プロジェクトのプロパティを開いて、「構成プロパティ」→「ビルド」→「XML ドキュメント ファイル」という項目に、出力したいXMLファイルの名前を入れてビルドを行ってください。

<pre class="console" title="XML Documentを生成する場合、/doc オプションをつける。">
csc /out:DocumentTest.dll /target:library doctest.cs /doc:doctest.xml
</pre>


すると、以下のような内容のXMLファイルが生成されているはずです。


<pre class="xsource" title="doctest.xml">
<code><span class="bracket">&lt;?</span><span class="element">xml</span> <span class="attribute">version</span><span class="attvalue">="1.0"</span><span class="bracket">?&gt;</span>
<span class="bracket">&lt;</span><span class="element">doc</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">assembly</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">name</span><span class="bracket">&gt;</span>DocumentTest<span class="bracket">&lt;/</span><span class="element">name</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">assembly</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">members</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">member</span> <span class="attribute">name</span><span class="attvalue">="T:DocumentTest.List"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">summary</span><span class="bracket">&gt;</span>
            簡単なサンプルとして、リストを実装する。
            片方向リストで、リストに値を加えることは出来るけど、削除は出来ない。
            <span class="bracket">&lt;/</span><span class="element">summary</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">member</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">member</span> <span class="attribute">name</span><span class="attvalue">="M:DocumentTest.List.#ctor"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">summary</span><span class="bracket">&gt;</span>
            リストの作成
            <span class="bracket">&lt;/</span><span class="element">summary</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">member</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">member</span> <span class="attribute">name</span><span class="attvalue">="M:DocumentTest.List.Add(System.Object)"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">summary</span><span class="bracket">&gt;</span>
            リストに値を加える。
            <span class="bracket">&lt;/</span><span class="element">summary</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">param</span> <span class="attribute">name</span><span class="attvalue">="obj"</span><span class="bracket">&gt;</span>加えたい値<span class="bracket">&lt;/</span><span class="element">param</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">member</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;</span><span class="element">member</span> <span class="attribute">name</span><span class="attvalue">="M:DocumentTest.List.GetEnumerator"</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">summary</span><span class="bracket">&gt;</span>
            リストのEnumeratorを返す
            <span class="bracket">&lt;/</span><span class="element">summary</span><span class="bracket">&gt;</span>
            <span class="bracket">&lt;</span><span class="element">returns</span><span class="bracket">&gt;</span>リストのEnumerator<span class="bracket">&lt;/</span><span class="element">returns</span><span class="bracket">&gt;</span>
        <span class="bracket">&lt;/</span><span class="element">member</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">members</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">doc</span><span class="bracket">&gt;</span>
</code></pre>
このように、C#コンパイラでは /doc オプションを指定してやることによって、ソースファイルからXML形式のドキュメントを自動で生成することが出来ます。


## <a id="sec-generated-title-4"></a> <a id="doc"></a>Documentation Comment

上述のサンプル中には、<code>///</code> というように、 <code>/</code> 3つで始まるコメントがあります。
C#では、この <code>///</code> で始まるコメントは特別な意味を持ち、<strong id="doccomment" class="keyword">ドキュメンテーションコメント</strong>と呼ばれています。
クラスやメソッドの前にこのドキュメンテーションコメントを入れておくと、そのクラスやメソッドに関する説明をXMLファイルに書き出してくれます。

また、C# 1.2（Visual Studio 2003 と同時期に出たバージョン）では <code>/** コメント \*/</code> というように、
<code>/\*\*</code> から始まる複数行コメントもドキュメンテーションコメントとして扱われるようになりました。

ここで、ドキュメンテーションコメントの内容はXML形式で書きます。
例えば、上述のサンプルでは<code>&lt;summary&gt;</code>というXML要素が出てきますが、この要素中には、そのクラスやメソッドの概要を書きます。
以下にドキュメンテーションコメント用のタグの一覧を示します。

<table summary="">

	<tr>
		<th>タグ名</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">&lt;c&gt;</td>
		<td markdown="1">コード(summaryなどの文中に書くもの)</td>
	</tr>
	<tr>
		<td markdown="1">&lt;code&gt;</td>
		<td markdown="1">コード(複数行にわたるもの)</td>
	</tr>
	<tr>
		<td markdown="1">&lt;example&gt;</td>
		<td markdown="1">サンプル コードの説明(codeと組み合わせて使う)</td>
	</tr>
	<tr>
		<td markdown="1">&lt;exception&gt;</td>
		<td markdown="1">例外クラスの説明</td>
	</tr>
	<tr>
		<td markdown="1">&lt;include&gt;</td>
		<td markdown="1">別のファイルの内容を取り込む</td>
	</tr>
	<tr>
		<td markdown="1">&lt;list&gt;</td>
		<td markdown="1">アイテマイズしたいときに使う</td>
	</tr>
	<tr>
		<td markdown="1">&lt;param&gt;</td>
		<td markdown="1">そのメソッドの引数に関する説明</td>
	</tr>
	<tr>
		<td markdown="1">&lt;paramref&gt;</td>
		<td markdown="1">summaryなどの文中で引数を参照したいときに使う</td>
	</tr>
	<tr>
		<td markdown="1">&lt;permission&gt;</td>
		<td markdown="1">メンバーへのアクセスのパーミッションを指定する</td>
	</tr>
	<tr>
		<td markdown="1">&lt;remarks&gt;</td>
		<td markdown="1">クラスの説明</td>
	</tr>
	<tr>
		<td markdown="1">&lt;returns&gt;</td>
		<td markdown="1">戻り値の説明</td>
	</tr>
	<tr>
		<td markdown="1">&lt;see&gt;</td>
		<td markdown="1">他のメンバーを参照したいときに使う</td>
	</tr>
	<tr>
		<td markdown="1">&lt;seealso&gt;</td>
		<td markdown="1">他に参照して欲しいものがあるときに使う</td>
	</tr>
	<tr>
		<td markdown="1">&lt;summary&gt;</td>
		<td markdown="1">そのクラスやメソッドの概要</td>
	</tr>
	<tr>
		<td markdown="1">&lt;value&gt;</td>
		<td markdown="1">プロパティの説明</td>
	</tr>
</table>
