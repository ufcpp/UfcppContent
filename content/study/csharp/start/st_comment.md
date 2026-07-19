---
title: "コメント"
source_url: "https://ufcpp.net/study/csharp/start/st_comment/"
content_type: "Article"
published_at: "2015-05-06T14:07:23"
updated_at: "2015-05-06T21:56:46"
tags: []
umbraco_id: 1194
parent_id: 1190
sort_order: 2
aliases:
  - "/csharp/st_comment"
  - "/csharp/st_comment.html"
  - "/csharp/start/st_comment/"
  - "/study/csharp/st_comment"
  - "/study/csharp/st_comment.html"
---

# コメント

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
プログラムには自然言語で注釈を入れましょう。


##### <a id="sec-generated-title-2"></a>ポイント
* コメント： プログラムとは関係ない、自然言語で書かれた注釈。

* <code>/\* 複数行にわたるコメント \*/</code>

* <code>// 行末までのコメント</code>

* 注意： 可能な限りコメントなんて書かなくても分かりやすいきれいなコードを書くのが理想的。



##<a id="sec-generated-title-3"></a> <a id="comment"></a>コメント
C# などの、自然言語に近い形で書けるプログラミング言語(このようなものを高級言語と呼ぶ)は、
人間が理解しやすい形でプログラムを記述するために作られたものですが、
やはり、自然言語による説明なしでは、理解のしやすさに限界があります。
プログラムのソースを理解しやすくするためには、人間の言葉で処理の概要や変数の意味などを書いておくのが一番です。

そのため、C# などの高級言語では、プログラムの流れとはまったく関係なく、人間の言葉で注釈を入れておくための仕組みを用意してあります。
このように、プログラム中に自然言葉で注釈を入れることを<strong id="comment" class="keyword">コメント</strong>といいます。

C#では、コメントの書き方には2通りあります。
1つは <em>
        <code>/\*</code> と <code>\*/</code> でコメントを囲う
      </em>方法で、もう1つは<em>
        <code>//</code> の後ろにコメントを書く
      </em>方法です。
<code>/\*</code> と <code>\*/</code> で囲われた文字列は、コメントとして扱われ、コンパイラに無視されます。
このコメントは複数行にわたって書くことも可能です。
ただし、<code>/\*</code> と <code>\*/</code> を2重にして使うことは出来ません。
また、<code>//</code> の後ろに続く文字列もコメントとして扱われます。
行末までがコメントとなります(そのため、複数行にわたるコメントは書けません)。
<code>/\* \*/</code> と違い、コメントを閉じ忘れるということが無いので便利です。

<pre class="source" title="コメントの例" lang="">
<code><span class="comment">/*
 この部分はコメントです。
 複数行にわたるコメントを書くことも可能です。
*/

// このようなコメントもかけます。
// 行末までがコメントになります。

/*
 でも、このコメントを
  /* こんな風に */
 2重に使っちゃだめ。
 このコメントはエラーになります。
*/
</span>
</code></pre>


どんなにプログラミングの得意な人でも、コメントのまったく入っていないソースファイルの内容を理解するのは困難です。
自分で書いたソースですら、数ヶ月も経つとどこに何を書いたのか分からなくなることがしばしばあります。
そういうことにならないためにも、ソースファイル中にはしっかりとコメントを入れるようにしましょう。

以下にコメントを挿入すべきポイントを示す例を挙げます。
プログラム中には現時点ではまだ説明していないことも使っていますので、
プログラムの内容自体は理解できないと思いますが、
ポイントとなる点は背景色を変えて強調してありますので、
とりあえずそこだけ流し読みしてみてください。


##### <a id="sec-generated-title-4"></a>サンプル
コメントの付け方の例を示します。
サンプルとはいえ、コメント入れ過ぎかも。

<em>
        ※おそらく、この例で「妥当な」コメントは /// から始まるドキュメンテーション コメントのみです。
      </em>

C# みたいな、割と意図どおりにプログラムコードを書ける言語において、
「コメントが付き過ぎ」あるいは「コメント書かなきゃ分からない」ってのは、
コードの出来が悪い可能性が高いです。
過剰なコメント（変数の説明や、処理の区切り）を入れたくなるようなときは、リファクタリングのしどきだと思ってください。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
</ul>
<div>

<pre class="source" title="コメントのつけ方の指針" lang="C#">
<code><span class="reserved">using</span> System;

<span class="comment"><em>// クラスの前にはそのクラスの説明を書いたほうがいい。</em></span>

<span class="comment"><em>//「///」から始まるコメントはC#では特別な意味を持つ。
// 詳しくは「XML Documentation」で説明する。</em>

/// &lt;summary&gt;
/// コメント付けのサンプルプログラム。
/// ここでは例として配列で与えられたデータ列の平均値と分散を求めて表示する。
/// &lt;/summary&gt;</span>
<span class="reserved">class</span> CommentSample
{
  <span class="reserved">static void</span> Main()
  {
<em>    <span class="comment">// 変数名の後に変数の説明を書いたりすることも。
    // ほんとは、コメントが無くても意味の分かる変数名を付けるべき。</span></em>

    <span class="reserved">var</span> dataSource = <span class="reserved">new</span>[] {
      455,  58,   8,   7, 987,  56,   2,  64, 698,  79,
       98,  79,  45, 465, 167,  97,  94, 657, 237, 587,
      687, 654, 647,   4, 654, 984,   8, 489,   7,  22 }; <span class="comment">// データ列</span>
    <span class="reserved">double</span> mean; <span class="comment">// 平均値</span>
    <span class="reserved">double</span> variance;  <span class="comment">// 分散

<em>    // 処理の区切りごとに、処理の内容の簡単な説明を書いたり。
    // これも、できれば、コメントなんて書かなくても分かりやすい簡潔な処理を書く方がいい。
    // (「関数の前にだけ説明があれば十分」と言うのが理想。
    //   要するに、処理の区切りごとに関数に分かれてる方がいい。
    // コメントが必要そうな処理の区切りがあったら、そこを関数化する。)</em>

    // データ列 dataSource の平均値と分散を求める</span>
    CalcMean(dataSource, <span class="reserved">out</span> mean, <span class="reserved">out</span> variance);

    <span class="comment">// 結果の表示</span>
    Console.WriteLine(<span class="literal">"平均 : {0}, 分散 : {1}"</span>, mean, variance);
  }

  <span class="comment"><em>// 関数の前にはその関数の説明を書く。</em>

  /// &lt;summary&gt;
  /// 配列に入ったデータの平均値と分散を求める
  /// &lt;param name="data"&gt;与えられたデータ列&lt;/param&gt;
  /// &lt;param name="mean"&gt;dataの平均値(出力)&lt;/param&gt;
  /// &lt;param name="variance"&gt;dataの分散(出力)&lt;/param&gt;
  /// &lt;/summary&gt;</span>
  <span class="reserved">static void</span> CalcMean(<span class="reserved">int</span>[] data, <span class="reserved">out double</span> mean, <span class="reserved">out double</span> variance)
  {
    <span class="reserved">int</span> sum = 0;     <span class="comment">// 合計</span>
    <span class="reserved">int</span> sq_sum = 0;  <span class="comment">// 二乗の合計

    // データ列の合計と二乗の合計を求める</span>
    <span class="reserved">foreach</span>(<span class="reserved">int</span> n <span class="reserved">in</span> data)
    {
      sum += n;
      sq_sum += n*n;
    }
    <span class="comment">// 平均値と分散を計算</span>
    mean = (<span class="reserved">double</span>)sum / data.Length;
    variance = (<span class="reserved">double</span>)sq_sum / data.Length - mean*mean;
  }
}
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="comment">' VB の場合は ' の後ろがコメント。</span>
<span class="comment">' を3つ付けるとドキュメンテーション コメント。</span>

<span class="comment">''' </span><span class="inactive">&lt;summary&gt;</span>
<span class="comment">''' コメント受けのサンプル プログラム</span>
<span class="comment">''' 「'''」から始まるコメントは VB では特別な意味を持つ。</span>
<span class="comment">''' 詳しくは「XML Documentation」で説明する。</span>
<span class="comment">''' </span><span class="inactive">&lt;/summary&gt;</span>
<span class="reserved">Module</span> <span class="type">Program</span>

    <span class="reserved">Sub</span> Main()
        <span class="reserved">Dim</span> dataSource = <span class="reserved">New</span> <span class="reserved">Integer</span>() {
            455, 58, 8, 7, 987, 56, 2, 64, 698, 79,
            98, 79, 45, 465, 167, 97, 94, 657, 237, 587,
            687, 654, 647, 4, 654, 984, 8, 489, 7, 22
        }

        <span class="reserved">Dim</span> mean <span class="reserved">As</span> <span class="reserved">Double</span>
        <span class="reserved">Dim</span> variance <span class="reserved">As</span> <span class="reserved">Double</span>

        CalcMean(dataSource, mean, variance)

        <span class="type">Console</span>.WriteLine(<span class="literal">"平均 : {0}, 分散 : {1}"</span>, mean, variance)
    <span class="reserved">End</span> <span class="reserved">Sub</span>

    <span class="comment">''' </span><span class="inactive">&lt;summary&gt;</span>
    <span class="comment">''' 配列に入ったデータの平均値と分散を求める。</span>
    <span class="comment">''' </span><span class="inactive">&lt;/summary&gt;</span>
    <span class="comment">''' </span><span class="inactive">&lt;param name=</span><span class="inactive">"data"</span><span class="inactive">&gt;</span><span class="comment">与えられたデータ。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="comment">''' </span><span class="inactive">&lt;param name=</span><span class="inactive">"mean"</span><span class="inactive">&gt;</span><span class="comment">data の平均値。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="comment">''' </span><span class="inactive">&lt;param name=</span><span class="inactive">"variance"</span><span class="inactive">&gt;</span><span class="comment">data の分散。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="comment">''' </span><span class="inactive">&lt;remarks&gt;&lt;/remarks&gt;</span>
    <span class="reserved">Sub</span> CalcMean(<span class="reserved">ByVal</span> data <span class="reserved">As</span> <span class="reserved">Integer</span>(), <span class="reserved">ByRef</span> mean <span class="reserved">As</span> <span class="reserved">Double</span>, <span class="reserved">ByRef</span> variance <span class="reserved">As</span> <span class="reserved">Double</span>)
        <span class="reserved">Dim</span> sum = 0
        <span class="reserved">Dim</span> squareSum = 0

        <span class="reserved">For</span> <span class="reserved">Each</span> x <span class="reserved">In</span> data
            sum += x
            squareSum += x
        <span class="reserved">Next</span>

        mean = <span class="reserved">CType</span>(sum, <span class="reserved">Double</span>) / data.Length
        variance = <span class="reserved">CType</span>(squareSum, <span class="reserved">Double</span>) / data.Length - mean * mean
    <span class="reserved">End</span> <span class="reserved">Sub</span>

<span class="reserved">End</span> <span class="reserved">Module</span>
</code></pre>


</div>
</div>


<pre class="console" title="">
平均 : 303.2, 分散 : 99802.0266666667
</pre>


ここで、<code>/**</code> もしくは <code>///</code> で始まるコメントには特殊な意味があります。
これらはドキュメンテーション コメントと呼ばれるもので、「[XML Document](../misc/sp_xmldoc.md)」で説明します。


##<a id="sec-generated-title-5"></a> <a id="whereToUse"></a>コメントの使いどころ
コメントの理想形は、ドキュメンテーション コメントだけを残すような状態です。
コードで表現できるものはコードで表現すべきです。

その他、どうしてもコメントが必要になる状況として、以下のようなものがあります。


##### <a id="sec-generated-title-6"></a>理由を書く
コメント（自然言語での注釈）は、「プログラム コードとして表現できないこと」を書くために使います。
how（どうやってプログラムを動かす）はコードで表現できるので、コメントとして書きません。
最近の高級言語の表現力は高く、what（何がしたいか）もコメントに書く必要性は低いはずです。
一番コメントとして残しておきたいのは、why（なぜそういうコードを書くことにしたか）でしょう。

特に、泥臭い bad know-how 的なコメントが多くなるでしょう。
例えば、以下のようなものです。

* 本当はもう少しきれいに書く方法があるんだけども、パフォーマンス上の理由からわざと見づらいコードにしている。

* どうもライブラリのバグらしいので回避策として。

* あまりよくないコードだけどもとりあえず。余裕があるときに直す<sup>※</sup>。


※ こうならないように、余裕持たなきゃだめですよ。

ソース コード上のコメントではなく、
進行管理ツール（TFS（Team Foundation Server）や Jenkins など）のタスク化/チケット化すべきものも多々あります。
ツールが使えるなら、「後で直す」的なものはチケット化しましょう。
