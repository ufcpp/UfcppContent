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
  - "/study/csharp/st_comment.html"
---

# コメント

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

プログラムには自然言語で注釈を入れましょう。


##### <a id="sec-generated-title-2"></a>ポイント

* コメント： プログラムとは関係ない、自然言語で書かれた注釈。

* <code>/\* 複数行にわたるコメント \*/</code>

* <code>// 行末までのコメント</code>

* 注意： 可能な限りコメントなんて書かなくても分かりやすいきれいなコードを書くのが理想的。



## <a id="sec-generated-title-3"></a> <a id="comment"></a>コメント

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

```csharp
/*
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
```


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

```csharp
using System;

// クラスの前にはそのクラスの説明を書いたほうがいい。

//「///」から始まるコメントはC#では特別な意味を持つ。
// 詳しくは「XML Documentation」で説明する。

/// <summary>
/// コメント付けのサンプルプログラム。
/// ここでは例として配列で与えられたデータ列の平均値と分散を求めて表示する。
/// </summary>
class CommentSample
{
  static void Main()
  {
    // 変数名の後に変数の説明を書いたりすることも。
    // ほんとは、コメントが無くても意味の分かる変数名を付けるべき。

    var dataSource = new[] {
      455,  58,   8,   7, 987,  56,   2,  64, 698,  79,
       98,  79,  45, 465, 167,  97,  94, 657, 237, 587,
      687, 654, 647,   4, 654, 984,   8, 489,   7,  22 }; // データ列
    double mean; // 平均値
    double variance;  // 分散

    // 処理の区切りごとに、処理の内容の簡単な説明を書いたり。
    // これも、できれば、コメントなんて書かなくても分かりやすい簡潔な処理を書く方がいい。
    // (「関数の前にだけ説明があれば十分」と言うのが理想。
    //   要するに、処理の区切りごとに関数に分かれてる方がいい。
    // コメントが必要そうな処理の区切りがあったら、そこを関数化する。)

    // データ列 dataSource の平均値と分散を求める
    CalcMean(dataSource, out mean, out variance);

    // 結果の表示
    Console.WriteLine("平均 : {0}, 分散 : {1}", mean, variance);
  }

  // 関数の前にはその関数の説明を書く。

  /// <summary>
  /// 配列に入ったデータの平均値と分散を求める
  /// <param name="data">与えられたデータ列</param>
  /// <param name="mean">dataの平均値(出力)</param>
  /// <param name="variance">dataの分散(出力)</param>
  /// </summary>
  static void CalcMean(int[] data, out double mean, out double variance)
  {
    int sum = 0;     // 合計
    int sq_sum = 0;  // 二乗の合計

    // データ列の合計と二乗の合計を求める
    foreach(int n in data)
    {
      sum += n;
      sq_sum += n*n;
    }
    // 平均値と分散を計算
    mean = (double)sum / data.Length;
    variance = (double)sq_sum / data.Length - mean*mean;
  }
}
```


</div>
<div>

```vbnet
' VB の場合は ' の後ろがコメント。
' を3つ付けるとドキュメンテーション コメント。

''' <summary>
''' コメント受けのサンプル プログラム
''' 「'''」から始まるコメントは VB では特別な意味を持つ。
''' 詳しくは「XML Documentation」で説明する。
''' </summary>
Module Program

    Sub Main()
        Dim dataSource = New Integer() {
            455, 58, 8, 7, 987, 56, 2, 64, 698, 79,
            98, 79, 45, 465, 167, 97, 94, 657, 237, 587,
            687, 654, 647, 4, 654, 984, 8, 489, 7, 22
        }

        Dim mean As Double
        Dim variance As Double

        CalcMean(dataSource, mean, variance)

        Console.WriteLine("平均 : {0}, 分散 : {1}", mean, variance)
    End Sub

    ''' <summary>
    ''' 配列に入ったデータの平均値と分散を求める。
    ''' </summary>
    ''' <param name="data">与えられたデータ。</param>
    ''' <param name="mean">data の平均値。</param>
    ''' <param name="variance">data の分散。</param>
    ''' <remarks></remarks>
    Sub CalcMean(ByVal data As Integer(), ByRef mean As Double, ByRef variance As Double)
        Dim sum = 0
        Dim squareSum = 0

        For Each x In data
            sum += x
            squareSum += x
        Next

        mean = CType(sum, Double) / data.Length
        variance = CType(squareSum, Double) / data.Length - mean * mean
    End Sub

End Module
```


</div>
</div>


```console
平均 : 303.2, 分散 : 99802.0266666667
```


ここで、<code>/**</code> もしくは <code>///</code> で始まるコメントには特殊な意味があります。
これらはドキュメンテーション コメントと呼ばれるもので、「[XML Document](../misc/sp_xmldoc.md)」で説明します。


## <a id="sec-generated-title-5"></a> <a id="whereToUse"></a>コメントの使いどころ

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
