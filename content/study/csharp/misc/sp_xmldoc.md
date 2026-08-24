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

```csharp {title="doctest.cs"}
using System;
using System.Collections;

namespace DocumentTest
{
  /// <summary>
  /// 簡単なサンプルとして、リストを実装する。
  /// 片方向リストで、リストに値を加えることは出来るけど、削除は出来ない。
  /// </summary>
  public class List : IEnumerable
  {
    // リストのノード
    internal class Node
    {
      public object obj;
      public Node next;

      public Node(Node next)
      {
        this.next = next;
        this.obj = null;
      }

      public Node(Node next, object obj)
      {
        this.next = next;
        this.obj = obj;
      }
    }// Node

    // リストのEnumerator
    private class ListEnumrator : IEnumerator
    {
      public ListEnumrator(List list)
      {
        this.list = list;
        current = list.head;
      }

      public bool MoveNext()
      {
        current = current.next;
        return current != null;
      }

      public object Current
      {
        get{return current.obj;}
        set{current.obj = value;}
      }

      public void Reset()
      {
        current = this.list.head;
      }

      List list;
      Node current;
    }// ListEnumerator

    /// <summary>
    /// リストの作成
    /// </summary>
    public List()
    {
      head = new Node(null);
      tail = head;
    }

    /// <summary>
    /// リストに値を加える。
    /// </summary>
    /// <param name="obj">加えたい値</param>
    public void Add(object obj)
    {
      tail.next = new Node(null, obj);
      tail = tail.next;
    }

    /// <summary>
    /// リストのEnumeratorを返す
    /// </summary>
    /// <returns>リストのEnumerator</returns>
    public IEnumerator GetEnumerator()
    {
      return new ListEnumrator(this);
    }

    private Node head; // リストのダミーヘッダー
    private Node tail; // リストの最後尾
  }// List
}// DocumentTest
```


そして、以下のようなオプションを付けてコンパイルしてください。
Visual C#を使って作成する場合には、プロジェクトのプロパティを開いて、「構成プロパティ」→「ビルド」→「XML ドキュメント ファイル」という項目に、出力したいXMLファイルの名前を入れてビルドを行ってください。

```console {title="XML Documentを生成する場合、/doc オプションをつける。"}
csc /out:DocumentTest.dll /target:library doctest.cs /doc:doctest.xml
```


すると、以下のような内容のXMLファイルが生成されているはずです。


```xml {title="doctest.xml"}
<?xml version="1.0"?>
<doc>
    <assembly>
        <name>DocumentTest</name>
    </assembly>
    <members>
        <member name="T:DocumentTest.List">
            <summary>
            簡単なサンプルとして、リストを実装する。
            片方向リストで、リストに値を加えることは出来るけど、削除は出来ない。
            </summary>
        </member>
        <member name="M:DocumentTest.List.#ctor">
            <summary>
            リストの作成
            </summary>
        </member>
        <member name="M:DocumentTest.List.Add(System.Object)">
            <summary>
            リストに値を加える。
            </summary>
            <param name="obj">加えたい値</param>
        </member>
        <member name="M:DocumentTest.List.GetEnumerator">
            <summary>
            リストのEnumeratorを返す
            </summary>
            <returns>リストのEnumerator</returns>
        </member>
    </members>
</doc>
```
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
