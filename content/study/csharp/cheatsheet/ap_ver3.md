---
title: "C# 3.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver3/"
content_type: "Article"
published_at: "2015-05-06T14:06:43"
updated_at: "2025-01-01T18:43:41"
tags:
  - "Ver. 3.0"
umbraco_id: 1179
parent_id: 1174
sort_order: 5
aliases:
  - "/csharp/ap_ver3"
  - "/csharp/ap_ver3.html"
  - "/csharp/cheatsheet/ap_ver3/"
  - "/study/csharp/ap_ver3"
  - "/study/csharp/ap_ver3.html"
---

# C# 3.0 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver3"></a>C# 3.0

<div class="version version3">Ver. 3.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2007/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2008</li>
<li>.NET Framework 3.5</li>
<li>Visual Basic 9</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>LINQ</li>
</ul>
</td>
</tr>
</table>

2005年9月、C# 2.0 の正式出荷を目前にして、
C# の次世代拡張 C# 3.0 の言語仕様が公開されました。

C# 3.0（そして、同時に発表された VB 9.0）の目玉となる機能は、
Language Integrated Query、略して LINQ と呼ばれるもので、
リレーショナルデータベースや XML に対する操作をプログラミング言語に統合するものです。
（データベースや XML 操作用のライブラリと、
プログラミング言語中にSQL 風の問い合わせ構文を埋め込めるようにする言語拡張から成ります。）

C# 3.0 に追加された機能の多くは、基本的にこの LINQ を使うために必要な機能、
あるいは、より便利に LINQ を使うための機能になります。

ちなみに、詳しくは「[小さな機能の組み合わせ](../data/datamodulararchitecture.md)」で説明していますが、
LINQという大きな目的を、小さな機能の組み合わせで実現しています。


## <a id="sec-generated-title-2"></a> <a id="functional"></a>関数型言語・動的言語的機能

C# 3.0 の新機能には、関数型言語や動的言語が由来と思われる機能がいくつかあります。

といっても、C# を関数型言語や動的言語にしようという話ではなくて、
後述する LINQ のために使えそうな機能を輸入したという感じです。

詳細は「[関数型言語・動的言語的な機能](../functional/sp3_functional.md)」で説明します。


##### <a id="sec-generated-title-3"></a>暗黙的型付け

var キーワードを用いて、
暗黙的に型付けされたローカル変数（Implicitly typed local variables）を定義できるようになりました。

<pre class="source" title="var" lang="">
<code><span class="reserved">var</span> n = <span class="literal">1</span>;
<span class="reserved">var</span> x = <span class="literal">1.0</span>;
<span class="reserved">var</span> s = <span class="literal">"test"</span>;
</code></pre>



##### <a id="sec-generated-title-4"></a> <a id="extension"></a>拡張メソッド

以下のような構文で、
クラスやインターフェースに対してインスタンスメソッドを擬似的に追加できるようになりました。

<pre class="source" title="拡張メソッドの定義" lang="">
<code><span class="reserved">static class</span> <span class="type">StringExtensions</span>
{
  <span class="reserved">public static string</span> ToggleCase(<span class="reserved"><em>this</em> string</span> s)
  <span class="input">中身省略</span>
}
</code></pre>


このようにして定義したメソッドは、
通常通り、静的メソッドとして呼び出すこともできますが、
あたかも string 型のインスタンスメソッドであるかのように呼び出せるようになります。

<pre class="source" title="拡張メソッドの呼び出し" lang="">
<code><span class="reserved">string</span> s = <span class="literal">"This Is a Test String."</span>;
<span class="reserved">string</span> s1 = <span class="type">StringExtensions</span>.ToggleCase(s); <span class="comment">// 通常の呼び出し方。</span>
<span class="reserved">string</span> s1 = <em>s.ToggleCase()</em>;                 <span class="comment">// 拡張メソッド呼び出し。</span>
</code></pre>



##### <a id="sec-generated-title-5"></a>ラムダ式

関数型言語でよく使うような記法で匿名メソッドを定義できるようになりました。
この機能をラムダ式と呼びます。

<pre class="source" title="ラムダ式" lang="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt; p = <em>n =&gt; n &gt; <span class="literal">0</span></em>;
</code></pre>


この式は、以下のような匿名メソッド同じ意味になります。

<pre class="source" title="C# 2.0 の匿名メソッド" lang="">
<code><span class="reserved">delegate</span>(<span class="reserved">int</span> n)
{
  <span class="reserved">return</span> n &gt; <span class="literal">0</span>;
}
</code></pre>


さらに、ラムダ式は式木データとしても利用可能です。
ラムダ式をデリゲートに代入すると匿名メソッド（実行コード）として、
Expression 型に代入すると式木データとしてコンパイルされます。

<pre class="source" title="ラムダ式をデータとして扱う" lang="">
<code><span class="type">Expression</span>&lt;<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">bool</span>&gt;&gt; e = n =&gt; n &gt; <span class="literal">0</span>;
<span class="type">BinaryExpression</span> lt = (<span class="type">BinaryExpression</span>)e.Body;
<span class="type">ParameterExpression</span> en = (<span class="type">ParameterExpression</span>)lt.Left;
<span class="type">ConstantExpression</span> zero = (<span class="type">ConstantExpression</span>)lt.Right;
</code></pre>



##### <a id="sec-generated-title-6"></a>初期化子

オブジェクトの初期化を以下のような記法でできるようになりました。

<pre class="source" title="オブジェクト初期化子" lang="">
<code><span class="type">Point</span> p = <span class="reserved">new</span> <span class="type">Point</span>{ X = <span class="literal">0</span>, Y = <span class="literal">1</span> };
</code></pre>


ちなみに、このコードは以下のようなコードと等価です。

<pre class="source" title="オブジェクト初期化子" lang="">
<code><span class="type">Point</span> p = <span class="reserved">new</span> <span class="type">Point</span>();
p.X = <span class="literal">0</span>;
p.Y = <span class="literal">1</span>;
</code></pre>


また、コレクションの初期化を以下のような記法でできるようになりました。

<pre class="source" title="コレクション初期化子" lang="">
<code><span class="type">List</span>&lt;<span class="reserved">int</span>&gt; list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; {<span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">3</span>};
</code></pre>


こちらは以下のようなコードと等価です。

<pre class="source" title="コレクション初期化子" lang="">
<code><span class="type">List</span>&lt;<span class="reserved">int</span>&gt; list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
list.Add(<span class="literal">1</span>);
list.Add(<span class="literal">2</span>);
list.Add(<span class="literal">3</span>);
</code></pre>



##### <a id="sec-generated-title-7"></a>匿名型

匿名型（anonymous type）を作成できるようになりました。
匿名型の作り方は以下の通りです。

<pre class="source" title="匿名型" lang="">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> { FamilyName = <span class="literal">"糸色"</span>, FirstName=<span class="literal">"望"</span>};
</code></pre>



##### <a id="sec-generated-title-8"></a>暗黙型付け配列

new で配列を作成する際、
型を省略できるようになりました。

<pre class="source" title="配列の暗黙的型付け" lang="">
<code><span class="reserved">int</span>[] array = <em><span class="reserved">new</span>[] {<span class="literal">1</span>, <span class="literal">2</span>, <span class="literal">3</span>, <span class="literal">4</span>}</em>;
</code></pre>



## <a id="sec-generated-title-9"></a> <a id="linq"></a>LINQ

C# 3.0 の目玉となる新機能は、
Language Integrated Query、略して LINQ と呼ばれるもので、
リレーショナルデータベースや XML に対する操作をプログラミング言語に統合するものです。

LINQ の導入により、以下のような利点があります。

* オブジェクト指向言語らしい書き方でデータベースへの問い合わせができます。

* in-memory なオブジェクト、XML、リレーショナルデータベースに対して、同じ文法でデータの問い合わせができます。

* 問い合わせ時に、コンパイラによる文法チェックや、IntelliSense のようなツールの補助を受けることができます。


要するに、LINQ とは、
C# 等の言語に SQL ライクなデータベース操作構文を組み込む
（＋ データベースや XML 操作用のライブラリ）
というものです。
例えば、以下のような書き方ができます。

<pre class="source" title="C# 3.0 LINQ" lang="">
<code><span class="reserved">var</span> 学生名簿 =
<span class="reserved">new</span>[] {
  <span class="reserved">new</span> {学生番号 = <span class="literal">14</span>, 姓 = <span class="literal">"風浦"</span>, 名 = <span class="literal">"可符香"</span>},
  <span class="reserved">new</span> {学生番号 = <span class="literal">20</span>, 姓 = <span class="literal">"小森"</span>, 名 = <span class="literal">"霧"</span>    },
  <span class="reserved">new</span> {学生番号 = <span class="literal">22</span>, 姓 = <span class="literal">"常月"</span>, 名 = <span class="literal">"まとい"</span>},
  <span class="reserved">new</span> {学生番号 = <span class="literal">19</span>, 姓 = <span class="literal">"小節"</span>, 名 = <span class="literal">"あびる"</span>},
  <span class="reserved">new</span> {学生番号 = <span class="literal">18</span>, 姓 = <span class="literal">"木村"</span>, 名 = <span class="literal">"カエレ"</span>},
  <span class="reserved">new</span> {学生番号 = <span class="literal">16</span>, 姓 = <span class="literal">"音無"</span>, 名 = <span class="literal">"芽留"</span>  },
  <span class="reserved">new</span> {学生番号 = <span class="literal">17</span>, 姓 = <span class="literal">"木津"</span>, 名 = <span class="literal">"千里"</span>  },
  <span class="reserved">new</span> {学生番号 = <span class="literal"> 8</span>, 姓 = <span class="literal">"関内"</span>, 名 = <span class="literal">"マリア"</span>},
  <span class="reserved">new</span> {学生番号 = <span class="literal">28</span>, 姓 = <span class="literal">"日塔"</span>, 名 = <span class="literal">"奈美"</span>  },
};

<span class="reserved">var</span> 学籍番号前半名 =
  <span class="reserved">from</span> p <span class="reserved">in</span> 学生名簿
  <span class="reserved">where</span> p.学生番号 &lt;= <span class="literal">15</span>
  <span class="reserved">orderby</span> p.学生番号
  <span class="reserved">select</span> p.名;

<span class="reserved">foreach</span>(<span class="reserved">var</span> 名 <span class="reserved">in</span> 学籍番号前半名)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, 名);
}
</code></pre>


<pre class="console" title="C# 3.0 LINQ の例、実行結果">
マリア
可符香
</pre>


詳細は「[LINQ](../data/sp3_linq.md)」と「[標準クエリ演算子（クエリ式関係）](../data/sp3_stdquery.md)」で説明します。


## <a id="sec-generated-title-10"></a> <a id="etc"></a>その他

LINQ 関連の機能以外に、
後からさらに追加された新機能があるようです。


### <a id="sec-generated-title-11"></a> <a id="auto"></a>自動プロパティ

「[イベント](../functional/sp_event.md#event)」は、
「[デリゲート](../functional/sp_delegate.md#delegate)」に対する「[プロパティ](../oop/oo_property.md#property)」のようなものなわけですが、
イベントの場合、
add/remove などの定義部分を省略して書けば、
コンパイラが自動的に add/remove に相当するものを生成してくれていました。

（2006年11月にひっそりと追加されたみたいなんですが）
これに対して、
C# 3.0 では、
プロパティの get/set の中身の省略もできるようになりました。

例えば、

<pre class="source" title="プロパティの set/get の省略" lang="">
<code><span class="reserved">public string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
</code></pre>


というように、
<code>get; set;</code> とだけ書いておくと、

<pre class="source" title="set/get の自動生成結果" lang="">
<code><span class="reserved">private string</span> __name;
<span class="reserved">public string</span> Name
{
  <span class="reserved">get</span> { <span class="reserved">return this</span>.__name; }
  <span class="reserved">set</span> { <span class="reserved">this</span>.__name = value; }
}
</code></pre>


というようなコードに相当するものが自動的に生成されます。
（<code>__name</code> という変数名はプログラマが参照できるものではありません。）


### <a id="sec-generated-title-12"></a> <a id="partial_method"></a>パーシャルメソッド

もう1つ、
（VS 2008 β2（2007年5月公開）で追加されたみたいなんですが）
<strong id="partial_method" class="keyword">パーシャルメソッド</strong>（partial method）という機能があります。

どういうものかというと、
「[パーシャルクラス](../oop/oo_class.md#partial_class)」内限定で、
メソッドに partial を付けることでメソッドの宣言と定義を分けれるというものです。
（ただし、private メソッド限定。戻り値も void 以外不可。）

例えば、まずクラスの部分定義で以下のようなコードを書いたとします。

<pre class="source" title="パーシャルメソッドの宣言" lang="">
<code><span class="reserved">partial class</span> <span class="type">Program</span>
{
  <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
  {
    OnBeginProgram();

    <span class="type">Console</span>.Write(<span class="literal">"program body\n"</span>);

    OnEndProgram();
  }

  <span class="reserved">static partial void</span> OnBeginProgram();
  <span class="reserved">static partial void</span> OnEndProgram();
}
</code></pre>


この状態でプログラムをコンパイル → 実行すると、「program body」の文字だけが表示されます。

ここで、以下のような部分定義を追加して、
パーシャルメソッドに実装を与えます。

<pre class="source" title="パーシャルメソッドに実装を追加" lang="">
<code><span class="reserved">partial class</span> Program
{
  <span class="reserved">static partial void</span> OnBeginProgram()
  {
    <span class="type">Console</span>.Write(<span class="literal">"check pre-condition\n"</span>);
  }

  <span class="reserved">static partial void</span> OnEndProgram()
  {
    <span class="type">Console</span>.Write(<span class="literal">"check post-condition\n"</span>);
  }
}
</code></pre>


すると、OnBeginProgram、OnEndProgram が呼ばれるようになります。
実行結果は以下の通り。

<pre class="console" title="パーシャルメソッド実装追加後の実行結果">
check pre-condition
program body
check post-condition
</pre>


利用場面としては、
宣言側は人手で書いて、
定義側はツールで自動生成というようなものを想定しているようです。

「[メソッドの実装の分離](../oop/oo_class.md#partial_method)」にもう少し詳しく書いていますが、
この機能は制限が多くて利用場面が限られていますし、
あまり好ましくない副作用もあります。
あくまで、人手での記述とツールでの自動生成の混在開発で使うものだと思ったほうがいいです。


## <a id="sec-generated-title-13"></a> <a id="summary"></a>まとめ

C# 3.0 には LINQ（Language Integrated Query）に関連した新機能がいくつか追加されました。
大別すると、以下のような機能です。

* SQL ライクな問い合わせ構文の統合

* 関数型言語・動的言語的機能
    * 型の省略・推論機構

    * 拡張メソッド

    * ラムダ式

C# 3.0 は、
問い合わせ構文の統合により、
データベースとオブジェクトの間の溝を埋めてくれます。
すなわち、
オブジェクト指向プログラミングにより実現される多彩なデータ構造を使って、
データベースの構築・問い合わせが可能になります。

ただし、
C# 3.0 では、言語そのものがデータベースへのアクセス機能を持ったわけではなく、
問い合わせ構文を適当なメソッド/拡張メソッド呼び出しに変換することで実現しています。
逆に言うと、適切なメソッドさえ定義されていれば、
何でも問い合わせ構文の対象となりえます。
SQL Server などのデータベースサーバーに対しても、
配列などで確保したメモリ上のデータに対しても、
また、XML データに対しても、
全く同じ構文で問い合わせが可能です。

型の省略・推論機構は、
型名を省略（あるいは var キーワードだけ記述）しても、
コンパイラが自動で適切な型を選択してくれるというものです。
これは、C# が動的型付け言語や型付けのゆるい言語になったということではありません。
型推論機構の導入によって、
C# の厳格な型付けを守ったまま、
型付けのゆるい言語の利便性に歩み寄ったものといえます。

ラムダ式などの機能追加も、
C# が関数型言語になったというわけではなく、
関数型言語から便利そうな構文をいくつか拝借したというものです。
