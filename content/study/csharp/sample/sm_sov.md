---
title: "[サンプル] SOV 型のメソッド呼び出し"
source_url: "https://ufcpp.net/study/csharp/sample/sm_sov/"
content_type: "Article"
published_at: "2008-03-09T00:00:00"
updated_at: "2015-05-06T14:13:24"
tags: []
umbraco_id: 1368
parent_id: 1359
sort_order: 8
aliases:
  - "/study/csharp/sm_sov.html"
---

# \[サンプル\] SOV 型のメソッド呼び出し

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

文章って、
動詞が最後にある方が自然らしいよ（眉つば）。

ということで、メソッド（動詞に相当）名が最後に来るような構文を C# で無理やり作ってみた。


## <a id="sec-generated-title-2"></a> <a id="cause"></a>ことの発端

きっかけは以下の記事。

* [「英語式語順は、自然な思考の順番に反する」研究結果](http://wiredvision.jp/news/200807/2008070921.html)


いわく、
「『少女がノブを回す』をジェスチャーで表してくれ」
というようなお願いをすると、
SOV 型の言語話者でも SVO 型の言語話者でも、
どちらでも「少女、ノブ、回す」の順序でジェスチャーするとか。

一応、
SOV とか SVO とかを補足しておくと、
「主語、目的語、述語」の語順か「主語、述語、目的語」の語順かという違い。
英語って「A girl turns a doorknob.」の語順になるはずだけど、
ジェスチャーすると「girl, doorknob, turn」の順になると。


### <a id="sec-generated-title-3"></a> <a id="programming"></a>OOP における語順

とりあえずこの論文を信じるとすると、
プログラミング言語における単語の語順ってどうなんでしょうか。

例えば、C 言語なんかでは以下のような書き方になります。

```csharp
Verb(subject);
Verb(subject, object);
```


たいてい、関数名が述語、そのあとに主語、目的語が来るので、VSO 型です。

これに対して、オブジェクト指向言語では以下のように書きます。

```csharp
subject.Verb();
subject.Verb(object);
```


SVO になりました。
VSO よりは述語が後ろにあります。
C# とかの OOP 言語は英語の文法に近いんですよね。
英語圏の人間が作ってるんだから当然？

さて、「SOV の方が自然」説を信じるなら、
以下のような構文がいいということになります。

```csharp
subject.Verb();
(subject, object).Verb();
```


この構文でダブルディスパッチできるなら確かに欲しい。

ダブルディスパッチってのは、subject と object の両方の動的型情報を使って「[多態性](../oop/oo_polymorphism.md#polymorphism)」することを言います。
例えば、図形の衝突判定を行いたい場合、
「丸と丸」「丸と四角」「四角と四角」でそれぞれ処理が変わります。
これは、OOP お得意の virtual 関数を使って <code>shape1.CollisionDetect(shape2)</code> と書くだけではできなかったりします。
上述のような SOV 構文で、<code>(shape1, shape2).CollisionDetect()</code> とか書けると結構素敵。


## <a id="sec-generated-title-4"></a> <a id="csharpsov"></a>C# で SOV 構文

完璧にネタなんですが、
C# で SOV 構文でプログラミングできるようにしてみました。

* 
[ソース一式（ZIP 圧縮）](../../../../assets/media/ufcpp2000/csharp/source/Sentence.zip)




### <a id="sec-generated-title-5"></a> <a id="english"></a>英語版

英語は語順に厳しい言語です。
これは、日本語でいうところの「てにをは」が欠落してしまったせいなんですけども、
ヨーロッパ言語ってものは本来、語順に自由の利く言語です。

逆に言うと、主語、目的語に by とか with を付けちゃえば、語順を自由にできるんですよ、実は。
例えば、「A girl turns a doorknob.」なら、

* by a girl, with a doorknob, turn.


にしてしまえば、語順が変わっても意味は通ります。

ということで、以下のような構文でメソッド呼び出しできるようなライブラリを書いてみました。

```csharp
Sentence
    .With("Hello World!\n")
    .To(Console.Out)
    .Write();

string result =
    Sentence
    .With("置換前の文字列\n")
    .From("置換前")
    .To("置換後")
    .Replace();
```


ちなみに、Write と Replace の実体は以下のような感じ。

```csharp
public static void Write(this Nominals n)
{
    var writer = n.To.Cast<System.IO.TextWriter>();
    writer.Write(n.With.Value);
}

public static string Replace(this Nominals n)
{
    var input = n.With.Cast<string>();
    var pattern = n.From.Cast<string>();
    var replacement = n.To.Cast<string>();

    var reg = new System.Text.RegularExpressions.Regex(pattern);

    return reg.Replace(input, replacement);
}
```


要するに、Nominals（体言リスト）と、Nominals を生成するための With, To などの拡張メソッドを定義しただけ。


##### <a id="sec-generated-title-6"></a>感想

VB の名前付き引数みたいになってきた。VSO 型になるけども。

```csharp
Write( \
  With := "Hello World!" \
  To   := Console.Write )

Replace( \
  With := "置換前の文字列" \
  From := "置換前" \
  To   := "置換後")
```



### <a id="sec-generated-title-7"></a> <a id="japanese"></a>日本語版

SOV 型言語と言えば日本語ですね、と。
日本語プログラミングしてみましょうか。

```csharp
Nominals.Make(
    "Hello World!\n".を(),
    Console.Out.に()
).出力();

string 置換結果 =
Nominals.Make(
    "置換前の文字列\n".の(),
    "置換前".を(),
    "置換後".に()
    ).置換();

string 繋いだ結果 =
Nominals.Make(
    "abc".と(),
    "def".と(),
    "ghi".を()
    ).繋ぐ();
```


出力、置換の定義は以下のような感じ。

```csharp
public static void 出力(this Nominals n)
{
    var writer = n.に.Cast<System.IO.TextWriter>();
    writer.Write(n.を.Value);
}

public static string 置換(this Nominals n)
{
    var input = n.の.Cast<string>();
    var pattern = n.を.Cast<string>();
    var replacement = n.に.Cast<string>();

    var reg = new System.Text.RegularExpressions.Regex(pattern);

    return reg.Replace(input, replacement);
}
```



##### <a id="sec-generated-title-8"></a>感想

皆様の言いたいことはわかります。

[ひまわり](http://ja.wikipedia.org/wiki/%E3%81%B2%E3%81%BE%E3%82%8F%E3%82%8A_(%E3%83%97%E3%83%AD%E3%82%B0%E3%83%A9%E3%83%9F%E3%83%B3%E3%82%B0%E8%A8%80%E8%AA%9E))？


## <a id="sec-generated-title-9"></a> <a id="in_fact"></a>そもそもほんとに SOV がいい？

実際のところどうなんでしょうね。
元ネタの記事、
そもそも「被験者は40人ってのは妥当？」とか「SOV 言語はトルコ語だけ？」とか、
少々疑問もありますし。

ちなみに、日本語は SOV、英語は SVO って区切りも実は結構あいまい。
文法なんていくらでも崩れます。
例えば、同じような文章でも・・・

* 私はそこで犬を見た

* 私がそこで見たものは犬だ

* 犬を見たの、そこで、私

* 見たよ、私、そこの犬


語順に厳しい英語ですら、形式主語とか関係代名詞で語順変え放題。

* I saw a dog there.

* It was a dog I saw there.

* There was a dog I saw

* What's seen by me was a dog.


書き方で意味が変わってくるし、
ちょっとどうかと思う文章もありますけど、
通じはすると思う、たぶん。

思うに、話の主題は前にあった方がいいってことなんだと思うんですが。
要するに、SOV でも SVO でもなく、

* 「話の主題となる名詞2・3個」 ＋ 「動詞」 ＋ 「付帯情報となる名詞」


というのがいいのではないかと。
実際、語順に自由の利く言語はそんな文章書く気がする。
SOVO？


### <a id="sec-generated-title-10"></a> <a id="powersheel"></a>それってつまり・・・

「話の主題が前、そして、付帯情報を後ろに付ける」っていうと、
実のところ、PowerShell のパイプライン構文がそうなってるかも。

```powershell
subject, object | Verb -Option complement
```
