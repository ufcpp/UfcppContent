---
title: "スクリプト言語とは"
source_url: "https://ufcpp.net/study/powershell/intro/scriptlang/"
content_type: "Article"
published_at: "2007-05-19T00:00:00"
updated_at: "2015-05-06T14:20:36"
tags: []
umbraco_id: 1575
parent_id: 1573
sort_order: 1
aliases:
  - "/study/powershell/scriptlang.html"
---

# スクリプト言語とは

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

PowerShell の話の前に、
スクリプト言語の話を少し。

<strong id="script" class="keyword">スクリプト言語</strong>とは、
小規模なコード向けに、
（小規模かつ性能をあまり重視しないうちは）便利な機能を盛り込んだプログラミング言語です。

スクリプト言語の目的は様々ですが、
例えば、
文字列処理に長けた物もあれば、
HTML と組み合わせてリッチでインタラクティブなウェブページを作るための言語などもあります。
また、多くのシェル環境は、
バッチ処理（まとめて一括処理）などのための専用のスクリプト言語を持っています
（PowerShell は、このシェル用のスクリプト言語）。

スクリプト言語と、C# などのアプリケーション開発用の言語（スクリプト言語との対比で<strong id="system" class="keyword">システムプログラミング言語</strong>と呼んだりもするみたい）とは、
前提・目的の違いによって、
言語の特性が変わってきます。


## <a id="sec-generated-title-2"></a> <a id="character"></a>スクリプト言語に対する前提・要求と特性

小規模・性能度外視のコードを前提としたスクリプト言語と、
時には大規模なアプリケーション開発にも使われる非スクリプト言語では、
前提の違いから、言語に求められる要求が変わってきます。
要求が異なれば、当然、言語の特性も変わってきます。

<table summary="スクリプト言語と非スクリプト言語の比較">
	<caption>
		スクリプト言語と非スクリプト言語の比較
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>スクリプト言語</th>
		<th>非スクリプト言語</th>
	</tr>
	<tr>
		<th rowspan="3">前提</th>
		<td markdown="1">小規模</td>
		<td markdown="1">大規模</td>
	</tr>
	<tr>
		<td markdown="1">概ね、1人で</td>
		<td markdown="1">チーム開発</td>
	</tr>
	<tr>
		<td markdown="1">性能はあまり要求されない</td>
		<td markdown="1">高い性能が要求される</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3" style="text-align:center;">↓</td>
	</tr>
	<tr>
		<th rowspan="3">言語への要求</th>
		<td markdown="1">覚えにくくても、1度覚えてしまえば便利</td>
		<td markdown="1">言語自体はシンプルに</td>
	</tr>
	<tr>
		<td markdown="1">書きやすく</td>
		<td markdown="1">書きやすさ以上に、読みやすさ・チェックしやすさ優先</td>
	</tr>
	<tr>
		<td markdown="1">性能すらも度外視で書きやすく</td>
		<td markdown="1">性能に影響がない範囲で書きやすく</td>
	</tr>
	<tr>
		<td markdown="1" colspan="3" style="text-align:center;">↓</td>
	</tr>
	<tr>
		<th rowspan="3">言語の特性</th>
		<td markdown="1">動的型付け</td>
		<td markdown="1">静的型付け</td>
	</tr>
	<tr>
		<td markdown="1">インタープリット方式</td>
		<td markdown="1">コンパイル方式</td>
	</tr>
	<tr>
		<td markdown="1">便利そうな文法はなんでも取り入れる</td>
		<td markdown="1">トリッキーすぎるコードが書けてしまう文法は避ける</td>
	</tr>
</table>


まあ、本当は細かい目的・用途ごとにプログラミング言語があったっていいんですが、
実際には、表1の右側の非スクリプト言語は C 言語とその系譜である C++, Java, C# の独壇場です。
「読みやすさ優先」とか「トリッキーな文法は避ける」とかの制約が付くと、
誰が作ろうと大体似たような言語になっちゃうのかも。

一方で、左側のスクリプト言語は結構な種類のものが残っていたりします。


## <a id="sec-generated-title-3"></a> <a id="instance"></a>具体例

私的には、
スクリプト言語と非スクリプト言語の違いで、一番大きいのは
「書きやすさ優先」と「読みやすさ優先」かなぁと感じています。

具体的な説明のために、
以下のような C# コードを考えてみます。

```csharp {title="連想配列の例（C#）"}
Dictionary<string, int> dic = new Dictionary<string, int>();
dic.Add("test", 1);
Console.Write(dic["test"]);
```


まあ、多分、C# というプログラミング言語をしらなくても、
dictionary（辞書）という単語を知っている人なら大体このコードの意味が分かるんじゃないでしょうか。
キー（test）と値（1）のペアを管理する辞書があって、
辞書に (test, 1) というペアを追加して、
最後に test をキーとする値を参照。

で、これと似たようなことを PowerShell のスクリプトを使って書いてみると、以下のようになります。

```powershell {title="連想配列の例（PowerShell）"}
$dic = @{}
$dic.test = 1
$dic.test
```


これだけです。ほんとに。
これを見れば、
「書きやすさ優先」と「読みやすさ優先」の違いがなんとなく分かるんじゃないかと。
むちゃくちゃ書きやすいんですが、
PowerShell を知らない人からしたらほんとに意味の分からないコードだと思います。

C# の側が Dictionary という名前のクラスで辞書を作るのに対して、
PowerShell では <code>@{}</code> だけで辞書が作れてしまいます。
（正確に言うと、<code>@{}</code> で作られるのは System.Collections.Hashtable 型だけど。）

値の参照も、
C# では <code>dic["test"]</code> というように、
いかにも test というキーで値を取り出しているように見える書き方なのに対して、
PowerShell ではプロパティの参照と区別の付かない <code>$dic.test</code> という書き方をします。

ちなみに、PowerShell では <code>$dic["test"]</code> という書き方もできます。
このように、「同じものに対して複数の異なる書き方ができる」というのは、
「便利な文法は何でも取り入れる」スクリプト言語によくある特徴ですね。
非スクリプト言語の場合、「<code>dic["test"]</code> と書けばいいんだから <code>dic.test</code> と書ける必要はない、むしろそれをやると混乱の原因」と考えるのが普通です。

混乱の原因というのは、
例えば C# では、<code>dic.test</code> と書くと、
test はメンバー変数かプロパティのどちらかなわけです。
これに対して、PowerShell では、
test はオブジェクトのメンバー変数かもしれないし、
連想配列のキーかもしれないし、
XML の子要素かもしれない。
全然違うものを同じ文法で書けるというのも、
「書きやすさ優先」の結果です。
