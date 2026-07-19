---
title: "名前のない複合型"
source_url: "https://ufcpp.net/study/csharp/structured/st_anonymoustype/"
content_type: "Article"
published_at: "2016-08-16T00:00:00"
updated_at: "2020-06-02T00:00:00"
tags: []
umbraco_id: 1939
parent_id: 1217
sort_order: 15
aliases:
  - "/csharp/structured/st_anonymoustype/"
---

# 名前のない複合型

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
型名がなくても、メンバー名だけでその型が何をしたいものなのか十分にわかる場合があります。
このとき、むしろ、良い型名が付かない(メンバー名と重複した名前にしかならない)こともあります。

そういう場合に、「名前のない複合型」を作りたくなります。
C#には、歴史的経緯から、[匿名型](../start/sp3_inference.md#anonymous)(anonymous type)と[タプル](../datatype/tuples.md#key-tuple)(tuple)という2種類の「名前のない複合型」があります。

##<a id="sec-generated-title-2"></a> <a id="use-case"></a>型に良い名前が付かない場合
一般的には、型にはちゃんとした名前を考えるべきです。
「型の名前だけを見れば、その型を使って何をしたいのかがわかる」というのが理想形です。
読みやすいプログラムを書くための1手法としても、「良い名前が付く単位でデータを1まとめにする」というのが非常に有効です。

しかし、常に良い名前が思いつくかというと、現実にはそうはいきません。
メンバー名だけ見ればその型が何をしたいのか十分にわかる場合、
型には良い名前が付きにくかったりします。

以下に2例ほど紹介しましょう。
それぞれ、タプルと匿名型が生まれた動機になります。

- 多値戻り値 → タプル
- 部分的なメンバー抜き出し → 匿名型

これらはタプル・匿名型の一番の動機ではありますが、別にこれ以外の用途でタプル・匿名型が使えないというわけではありません。

また、タプルと匿名型は似たような機能ですが、動機が異なれば実装はかなり変わります。

##<a id="sec-generated-title-3"></a> <a id="multiple-returns"></a>多値戻り値
関数を作るとき、複数の値を返したい場合があります。
例えば、「最小値、最大値、平均値を同時に求めるメソッド」があったとしましょう。

<pre class="source" title="最小値、最大値、平均値を同時に求めるメソッド">
<code><span class="reserved">static</span> <span class="type">X</span> Measure(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="reserved">var</span> count = 0;
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">var</span> min = <span class="reserved">int</span>.MaxValue;
    <span class="reserved">var</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
    {
        sum += x;
        count++;
        min = <span class="type">Math</span>.Min(x, min);
        max = <span class="type">Math</span>.Max(x, max);
    }

    <span class="reserved">return</span> <span class="reserved">new</span> <span class="type">X</span>(min, max, (<span class="reserved">double</span>)sum / count);
}
</code></pre>

この、戻り値の型`X`は、どういう名前であるべきでしょう。
メソッドがメソッドなので、「最小値と最大値と平均値」みたいな名前、すなわち、`MinMaxAverage`とかでしょうか？
`Measure`(計測)した結果なので、`MeasureResult`とかでしょうか？

どちらも、メンバー名やメソッド名を見ればわかります。
メンバー名やメソッド名と重複した名前です。
重複は後々プログラムを修正しにくくなるのであまりいいことではありません。
例えば、メソッド名を`Measure`から`Tally`(勘定、計算)に変えたくなったとします。`MeasureResult`も`TallyResult`に変えないといけないでしょう。
返したい値として、個数と分散、中央値も増やしたくなったとします。`CountMinMaxAverabeVarianceMedian`にすべきでしょうか？

こういう場合には、「名前のない型」を認めるべきです。例えば、以下のような書き方です。

<pre class="source" title="タプルを使った書き方">
<code><span class="reserved">static</span> <em>(<span class="reserved">int</span> min, <span class="reserved">int</span> max, <span class="reserved">double</span> average)</em> Measure(<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; items)
{
    <span class="reserved">var</span> count = 0;
    <span class="reserved">var</span> sum = 0;
    <span class="reserved">var</span> min = <span class="reserved">int</span>.MaxValue;
    <span class="reserved">var</span> max = <span class="reserved">int</span>.MinValue;
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> items)
    {
        sum += x;
        count++;
        min = <span class="type">Math</span>.Min(x, min);
        max = <span class="type">Math</span>.Max(x, max);
    }

    <span class="reserved">return</span> (min, max, (<span class="reserved">double</span>)sum / count);
}
</code></pre>

これで十分に、「itemsの最小値(min)、最大値(max)、平均値(average)を計って(measure)返す」という意図を書き表せています。

ちなみに、この、<code>(<span class="reserved">int</span> min, <span class="reserved">int</span> max, <span class="reserved">double</span> average)</code>という書き方をタプルと呼びます。
この機能については「[タプル](../datatype/tuples.md)」で説明します。

##<a id="sec-generated-title-4"></a> <a id="projection"></a>部分的なメンバー抜き出し
主に「[データ処理](#projection)」以降で説明して行きますが、
データ処理では、ある型のデータの中から、所定のメンバーだけを抜き出したいことがよくあります。

例えば、以下のようなデータがあったとします。
これは、「[疑似個人情報データ生成サービス](http://hogehoge.tk/personal/)」を使って作った架空の個人情報です。

<pre class="source">
1,奥野茉奈,オクノマナ,女,0288250107,1972/05/18
2,久保敏行,クボトシユキ,男,086288618,1984/10/13
3,長瀬由美,ナガセユミ,女,0548252320,1965/09/25
4,植田良子,ウエダヨシコ,女,0954083389,1977/03/18
・・・
</pre>

全データ: [personal_infomation.csv](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/StructuredProgramming/Tuples/personal_infomation.csv)

このデータを以下のような型で読み込んで使うとします。

<pre class="source" title="個人情報を表すクラス">
<code><span class="reserved">class</span> <span class="type">Person</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Id { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> Kana { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Sex</span> Sex { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> PhoneNumber { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">DateTime</span> BirthDay { <span class="reserved">get</span>; }
}
</code></pre>

このデータ列に対して、性別・年代ごとの人数構成を調べたいとします。
C# には、グループ化するための関数(`GroupBy`)や、個数を調べるための関数(`Count`)が備わっているのでそれを使いたいと思います。

<pre class="source" title="性別・年代ごとの人数調査">
<code><span class="reserved">var</span> persons = ReadAll(<span class="string">"personal_infomation.csv"</span>).ToArray();

<span class="comment">// 性別・年代(10年区切り)ごとに何人いるかを集計</span>
<span class="reserved">var</span> histgram = persons
    .GroupBy(p =&gt; <span class="reserved">new</span> <span class="type">X</span> { Sex = p.Sex, BirthDecade = p.BirthDay.Year / 10 })
    .Select(g =&gt; <span class="reserved">new</span> <span class="type">Y</span>{ Sex = g.Key.Sex, BirthDecade = g.Key.BirthDecade, Count = g.Count() })
    .OrderBy(x =&gt; x.BirthDecade)
    .ThenBy(x =&gt; x.Sex);
</code></pre>

ここで再び命名問題です。
グループ化のキーとして使っている`X`型と、結果をまとめるために使っている`Y`型は、どういう名前であるべきでしょう。
`Person`の一部分なので`PartOfPerson`とかでしょうか？別の情報を抜き出したくなった時との区別はどうしましょう。
グループ化のキーなわけで`GroupKey`とか？これも、グループ化の条件をいろいろ変えたいときは、条件ごとに`GroupKey`が必要になります。
多値戻り値の時と同様、「性別と年代の組み合わせ」であれば、メンバー名(`Sex`、`BirthDecade`)を見れば十分に意味が分かります。

こういう場合もやはり、「名前のない型」を認めるべきです。例えば以下のような書き方です。

<pre class="source" title="匿名型を使った書き方">
<code><span class="reserved">var</span> persons = ReadAll(<span class="string">"personal_infomation.csv"</span>).ToArray();

<span class="comment">// 性別・年代(10年区切り)ごとに何人いるかを集計</span>
<span class="reserved">var</span> histgram = persons
    .GroupBy(p =&gt; <span class="reserved">new</span> { p.Sex, BirthDecade = p.BirthDay.Year / 10 })
    .Select(g =&gt; <span class="reserved">new</span> { g.Key.Sex, g.Key.BirthDecade, Count = g.Count() })
    .OrderBy(x =&gt; x.BirthDecade)
    .ThenBy(x =&gt; x.Sex);
</code></pre>

コード全体: [AnonymousTypes.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/StructuredProgramming/Tuples/AnonymousTypes.cs)

<pre class="console" title="実行結果">
<code>{ Sex = Male, BirthDecade = 195, Count = 45 }
{ Sex = Female, BirthDecade = 195, Count = 43 }
{ Sex = Male, BirthDecade = 196, Count = 117 }
{ Sex = Female, BirthDecade = 196, Count = 115 }
{ Sex = Male, BirthDecade = 197, Count = 126 }
{ Sex = Female, BirthDecade = 197, Count = 131 }
{ Sex = Male, BirthDecade = 198, Count = 140 }
{ Sex = Female, BirthDecade = 198, Count = 133 }
{ Sex = Male, BirthDecade = 199, Count = 79 }
{ Sex = Female, BirthDecade = 199, Count = 71 }
</code></pre>

この、<code><span class="reserved">new</span> { p.Sex, BirthDecade = p.BirthDay.Year / 10 }</code>というような書き方を匿名型と言います。

##<a id="sec-generated-title-5"></a> <a id="summary"></a>まとめ
ここでは、型名を付けるに付けられない場合を2例ほど紹介しました。

- 多値戻り値
- 部分的なメンバー抜き出し

それぞれ、タプルと匿名型という機能がC#に入った動機にあたります。これらの機能の詳細については、別項で説明して行きます。

- [匿名型](../start/sp3_inference.md#anonymous)(anonymous type)
- [タプル](../datatype/tuples.md#key-tuple)(tuple)
