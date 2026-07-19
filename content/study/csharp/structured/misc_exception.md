---
title: "[雑記] 例外の使い方"
source_url: "https://ufcpp.net/study/csharp/structured/misc_exception/"
content_type: "Article"
published_at: "2015-05-06T14:09:11"
updated_at: "2018-04-14T23:45:54"
tags: []
umbraco_id: 1246
parent_id: 1217
sort_order: 18
aliases:
  - "/csharp/misc_exception"
  - "/csharp/misc_exception.html"
  - "/csharp/structured/misc_exception/"
  - "/study/csharp/misc_exception"
  - "/study/csharp/misc_exception.html"
---

# \[雑記\] 例外の使い方

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

言語構文的な意味での例外処理の方法は「[例外処理](oo_exception.md)」で説明しましたが、
実際のところ、どういう場合にどうやって例外を投げて、
どうやって処理すればいいのかは、
慣れるまでなかなか難しかったりします。

ということで、ここでは、例外の使い方について説明したいと思います。


## <a id="sec-generated-title-2"></a> <a id="basis"></a>例外の投げ方の基本方針

例外の投げ方に関する考え方は意外とシンプルで、
「<em>メソッドの定める結果を達成できないなら例外を投げる</em>」という方針で OK です。

いくつか例を挙げてみましょう。

<table summary="規約と例外">
	<caption>
		規約と例外
	</caption>
	<tr>
		<th>メソッド</th>
		<th>規約</th>
		<th>例外が起きる場面</th>
	</tr>
	<tr>
		<td markdown="1">File.Open</td>
		<td markdown="1">ファイルを開く。</td>
		<td markdown="1">開こうとしたファイルが存在しない。</td>
	</tr>
	<tr>
		<td markdown="1">Enumerable.Single</td>
		<td markdown="1">コレクションの中から条件を満たすただ1つの要素を選択する。</td>
		<td markdown="1">条件を満たす要素が1つも存在しないか、もしくは、2つ以上存在する。</td>
	</tr>
	<tr>
		<td markdown="1">int.Parse</td>
		<td markdown="1">文字列を整数に変換する。</td>
		<td markdown="1">変換できない文字列が渡された。</td>
	</tr>
	<tr>
		<td markdown="1">Dictionary&lt;T&gt;.Item</td>
		<td markdown="1">キーに対応する値を取得する。</td>
		<td markdown="1">キーが存在しない。</td>
	</tr>
</table>


こういう規約違反に対しては、基本的に例外を使います。
後述する 「[Try Parse パターン](#tryparse)」が適しているような状況以外では、戻り値によって正常・例外の区別をするような方法は好ましくありません。
（利用側に例外処理を強制できず、例外が発生したことに気づかないまま正常なつもりで処理を続けてしまい、後になって困る可能性がある。）


## <a id="sec-generated-title-3"></a> <a id="class"></a>例外の種類

一口に「規約を達成できない状況」と言っても、実はいくつかのパターンがあります。

* 利用法上の例外： 利用者側が正しい使い方をしていれば回避できる例外。

* 発生は避けれないが、復帰可能な例外。

* 対処のしようがない致命的な例外。



##### <a id="sec-generated-title-4"></a>利用法上の例外

多くの場合、事前のチェックなどを行うことによって例外が発生するような状況は回避できます。

<table summary="例外の発生状況と回避方法">
	<caption>
		例外の発生状況と回避方法
	</caption>
	<tr>
		<th>例外が発生する状況</th>
		<th>回避する方法</th>
	</tr>
	<tr>
		<td markdown="1">引数が null であってはならない。</td>
		<td markdown="1">どこか適切なレベルで null チェックを行う。</td>
	</tr>
	<tr>
		<td markdown="1">ファイルを Open するまえに Read してはならない。</td>
		<td markdown="1">ファイルを Open してから Read する。 Close した後には Read しない。</td>
	</tr>
	<tr>
		<td markdown="1">Dictionary では、存在しないキーに対して値の読み取りを行ってはならない。</td>
		<td markdown="1">キーが存在するかどうかを事前にチェックする。</td>
	</tr>
	<tr>
		<td markdown="1">読み取り専用コレクションに対して書き込み操作を行ってはならない。</td>
		<td markdown="1">事前に IsReadOnly プロパティを確認する。</td>
	</tr>
</table>


この手の例外に対しては、以下のような方針を取ります。

* 後述する「[Tester-Doer パターン](#tester_doer)」などを使って、事前に例外を避けるためのチェックを行えるようにする。

* 利用法上の例外はキャッチしない。例外が発生しなくなるまでテストとデバッグを行う。

* 引数の不正に対しては System.ArgumentException または System.ArgumentNullException を投げる。

* 不正な操作に対しては System.InvalidOperationException を投げる。



##### <a id="sec-generated-title-5"></a>発生は避けれないが、復帰可能な例外

例えば、File.Open を考えてみましょう。
以下のようなコードは、一応、ファイルの存在の有無を事前チェックしています。

```csharp
if (!File.Exists(filename))
{
    text = "デフォルトのテキスト";
}
else
{
    // Exists の後、↓を実行するまでの間にファイルが消される可能性はある。
    text = File.ReadAllText(filename);
}
```


ところが、ファイルというのはこのプログラム以外からも編集されるものなので、
Exists で確認してから ReadAllText で読み込みを行うわずかな間にファイルが消えてしまう可能性が残ります。

ですが、このコードの場合、ファイルが消えてたら消えてたでデフォルトの値を返すことでプログラムは続行可能なので、
以下のように対処します。

```csharp
try
{
    if (!File.Exists(filename))
    {
        text = "デフォルトのテキスト";
    }
    else
    {
        text = File.ReadAllText(filename);
    }
}
catch(FileNotFoundException)
{
    text = "ファイルがなくてもデフォルトのテキストがあれば OK";
}
```


この手の例外に対しては、以下のような方針を取ります。

* 完全に回避可能ではないにしても、「[Tester-Doer パターン](#tester_doer)」を用意しておくのは悪いことではない。

* 対処可能なら catch して対処してしまう。

* 対処できない（あるいは、対処をもっと上の階層にゆだねた方がいい）場合
    * catch しない。

    * あるいは、一度 catch して、いくつか必要な操作（中途半端な状態を残さないためにロールバックを行ったり、ログを記録したり）を行ってから例外を再度投げる。



* 標準の例外とは違う処理が必要な場合にのみ、自前で例外クラスを作成する。
    * 例えば、FileNotFoundException を catch せずにそのまま上位に流してもいい場面では、自作の例外でラップしたりはしない。





##### <a id="sec-generated-title-6"></a>対処のしようがない致命的な例外

.NET Framework 自体がエラーを起こしたり、どうあがいてもプログラムの続行が不可能な状況も、希にあります。

こういう場合、言えることは1つだけで、
<em>下手に例外を握りつぶさず、素直にプログラムを終了させてください</em>。
握りつぶしたがために後から不具合を起こすくらいなら、
例外が起きた瞬間に処理を止める方が幾分かマシです。


## <a id="sec-generated-title-7"></a> <a id="tester"></a>Tester-Doer パターン

前述のとおり、使用法上の例外はそもそも発生しないようにするのが好ましいです。
そのためには、メソッド呼び出し前に、インスタンスの状態や、引数の中身をチェックします。
引数の null チェックのように外で簡単にチェックできる条件もあれば、
特別なチェック用メソッドが必要な場合もあります。

後者、すなわち、
例外の発生しうるメソッド本体（Doer: do するもの）に対して、
事前チェック用のテストメソッド（Tester）を用意する方法のことを <strong id="tester_doer" class="keyword">Tester-Doer パターン</strong>と呼びます。
.NET Framework のクラスで、この Tester-Doer パターンになっているものをいくつか紹介します。

<table summary="Tester-Doer パターンを実装するクラス">
	<caption>
		Tester-Doer パターンを実装するクラス
	</caption>
	<tr>
		<th>クラス</th>
		<th>Doer</th>
		<th>Tester</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">Sytem.Collections.Generics.IDictionary</td>
		<td markdown="1">Item （インデクサー）</td>
		<td markdown="1">ContainsKey</td>
		<td markdown="1">キーが存在しない場合に例外を起こすんで、事前に存在の有無を確認。 （ちなみに、後述の「[Try Parse パターン](#tryparse)」を使う TryGetValue というメソッドもある。）</td>
	</tr>
	<tr>
		<td markdown="1">System.Collection.Generics.ICollection</td>
		<td markdown="1">Item （インデクサー）の setter</td>
		<td markdown="1">IsReadOnly</td>
		<td markdown="1">.NET では、読み取り専用コレクションと読み書き両用コレクションでインターフェースを分けない方針。 読み取り専用かどうかは IsReadOnly プロパティで確認してから使う。</td>
	</tr>
	<tr>
		<td markdown="1">System.IO.Stream</td>
		<td markdown="1">Read, Write</td>
		<td markdown="1">CanRead, CanWrite</td>
		<td markdown="1">同上、読み取り専用・書き込み専用・両用でインターフェースを分けない。</td>
	</tr>
</table>



## <a id="sec-generated-title-8"></a> <a id="try"></a>Try Parse パターン

処理の内容によっては、Tester の処理負荷が高すぎて、「[Tester-Doer パターン](#tester_doer)」を実装したくない場合があります。

例えば、int.Parse なんかがそうなんですが、
int.Parse に対して Tester を作ろうと思うと、結局のところ int.Parse と同じような処理が必要になります。
同じような処理を2度実行するのも馬鹿げた話なので、別の方法を考えることになります。

こういう場合に使うのが <strong id="tryparse" class="keyword">Try Parse パターン</strong>です。
「戻り値でエラーを返さない」という方針をあきらめて、bool の戻り値で処理の可否を返します。

* Parse: 文字列を解析して値に変換する。変換できない場合例外を投げる。

* TryParse: 文字列を解析して、変換できるかどうかを調べると同時に、できるならば値を返す。bool の戻り値で変換の可否を、out 引数（「[出力引数](../resource/sp_ref.md#out)」参照）で値を返す。


```csharp
// 通常の Parse。変換できない場合は FormatException が発生。
int x = int.Parse(text);

// TryParse。例外が発生しない代わりに、見てのとおり書き方がちょっとうっとおしい。
int y;
if (!int.TryParse(text, out y))
    y = 0;
```


out 引数も可能な限りは避けたい機能だったりするんで、
実は結構な苦肉の策だったりはします。
とはいえ、変換ができないときに例外を発生させるくらいなら、
多少の不恰好を覚悟でこのパターンを使うのもやむなしな感じです。

ちなみに、Tester-Doer パターンと Try Parse パターンの両方を実装しておくというのも考えられます。
例えば、IDictionary&lt;T&gt; では、Item（インデクサー）と ContainsKey では Tester-Doer パターンに、
TryGetValue では Try Parse パターンに基づいた実装になっています。
