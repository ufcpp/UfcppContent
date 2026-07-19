---
title: "[雑記] O/R インピーダンスミスマッチ"
source_url: "https://ufcpp.net/study/csharp/data/sp3_ormismatch/"
content_type: "Article"
published_at: "2007-11-03T00:00:00"
updated_at: "2011-05-28T00:00:00"
tags:
  - "Ver. 3.0"
umbraco_id: 1307
parent_id: 1298
sort_order: 9
aliases:
  - "/csharp/data/sp3_ormismatch/"
  - "/csharp/sp3_ormismatch"
  - "/csharp/sp3_ormismatch.html"
  - "/study/csharp/sp3_ormismatch"
  - "/study/csharp/sp3_ormismatch.html"
---

# \[雑記\] O/R インピーダンスミスマッチ

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<h5 class="version version3">Ver. 3.0</h5>

「[LINQ](sp3_linq.md#linq)」 を用いることで、
IEnumerable や XML、リレーショナルデータベースなど、
様々なデータソースに対して、共通の構文で問い合わせなどの操作を行うことができます。

その中でも、リレーショナルデータベースへの問い合わせを可能とする LINQ to SQL や Entity Framework は、
オブジェクト指向プログラミングとリレーショナル データベースの間の溝（インピーダンスミスマッチ）を埋める技術として、非常に面白いものになっています。

* サンプル プログラム:[EntityFrameworkSample.zip](../../../../assets/source/EntityFrameworkSample.zip)



##<a id="sec-generated-title-2"></a> <a id="gui-data"></a>ほとんどのアプリケーション ＝ GUI ＋ データ処理
近年、ほとんどのアプリケーションは、何らかのデータに対する操作と表示が主な仕事となっています。
すなわち、データ処理（読み出しや更新）と表示用の GUI 構築がプログラムの行う処理です。

<figure>
	[![データに対する操作と表示](../../../../assets/media/ufcpp2000/csharp/fig/data1.png)](../../../../assets/media/ufcpp2000/csharp/fig/data1.png)
	<figcaption>データに対する操作と表示</figcaption>
</figure>


このうち、GUI の構築に関しては、オブジェクト指向とイベント駆動という考え方に基づいてプログラムが作られます。
C# は 1.0 の頃から、オブジェクト指向とイベント駆動に関する機能は充実していました。
（参考： 「[オブジェクト指向](../index.md#oop)」、「[イベント](../functional/sp_event.md)」 ）

一方、データ処理は、リレーショナル データベースなどに格納するわけですが、
オブジェクト指向プログラミング（OOP）とリレーショナル データベース（RDB）の間にはプログラミング モデルの差があって、
その差（インピーダンス ミスマッチと呼ばれたりします）が開発のハードルになるので問題とされています。

（本節以降で説明する 「[LINQ](sp3_linq.md#linq)」 は、
C# 3.0 で導入された機能で、のような OOP を基本とするプログラミング言語内で、RDB 的なデータ処理を実現するものです。）


##<a id="sec-generated-title-3"></a> <a id="impedance_mismatch"></a>インピーダンス ミスマッチ
インピーダンス ミスマッチ（impedance mismatch）という言葉は、元々は電気工学の言葉で、
直訳するなら「抵抗の不一致」ということになります。
図1に示すように、抵抗の異なる素材の間に電磁波を通そうとすると、
境界面で反射が起こって、電気的なエネルギーを効率よく伝達できないんですが、
そういう状況を思い浮かべての比喩表現です。

<figure>
	[![異なる素材間の抵抗の不一致](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch.png)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch.png)
	<figcaption>異なる素材間の抵抗の不一致</figcaption>
</figure>


（
電磁波とか言われてもよく分からないという人向けに蛇足的に説明すると、
音の反射も、音波が物質中を伝わる際の抵抗（音響インピーダンス）の異なる物質の境界面で起こります。
とにかく、
物質間で反射が起きてエネルギーがうまく伝わらない状況というのがインピーダンスミスマッチです。
）

インピーダンスのミスマッチが伝達ロスを招くわけですから、
このミスマッチを解消することで伝達率が上がることが期待されます。
実際、図2に示すように、物質の境界面をぼかすような処理をかけることで、
インピーダンス（物質の抵抗）の変化が緩やかになって、
光（電磁波）や音の反射を軽減できたりもします。

<figure>
	[![インピーダンス ミスマッチの解消による反射の軽減](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch2.png)](../../../../assets/media/ufcpp2000/csharp/fig/ormismatch2.png)
	<figcaption>インピーダンス ミスマッチの解消による反射の軽減</figcaption>
</figure>


（
ちなみに、
表面に微細な凸凹を付けることで境界面をぼかして、
インピーダンス ミスマッチの解消による反射の低減を行って、
きわめて透明なガラス/フィルムを作る技術があったりします。
蛾の目の構造はこういう凸凹になっていて実際に反射が抑えられていることから、
「モスアイ（moth eye）加工」と呼ばれたりします。
）


##<a id="sec-generated-title-4"></a> <a id="or_impedance_mismatch"></a>O/R インピーダンスミスマッチ
要するに、<em>コンセプトの異なる2つの分野を繋ごうとする際に起こる困難</em>をさして、
インピーダンス ミスマッチという言葉を使います。
そして、インピーダンス ミスマッチがあると「伝達ロス」が発生するので、このミスマッチは極力解消したいものです。

IT の分野で特に問題となるミスマッチは、
オブジェクト指向プログラミング（OOP）とリレーショナルデータベース（RDB）の間の不一致で、
<strong id="or_mismatch" class="keyword">O/R インピーダンス ミスマッチ</strong>（O/R は Object/Relational の略）と呼ばれます。

ここでは、
OOP と RDB でそれぞれどういうコンセプトでデータを表すかを説明した上で、
どういうミスマッチがあるのか、
LINQ でどう解決されるのかを説明したいと思います。


##<a id="sec-generated-title-5"></a> <a id="class_vs_table"></a>OOP のクラスと RDB のテーブル
ここでは例として、本のシリーズと作家のデータベースを考えます。
この例では、
シリーズは名前と出版社と作者を、
作家は名前・誕生日・ウェブサイト URL を持つものとします。

作家はいくつかのシリーズを持っていますし、シリーズにはそれぞれ作者がいるわけですが、
まあまず、最初はその両者の間の関連性はおいておいて別々に考えます。
（この段階では OOP と RDB の間の差は顕著には現れません。）


##### <a id="sec-generated-title-6"></a>OOP（クラス）
まず、OOP の例として C# のコードを挙げますが、
C# の場合、以下のようなクラスを定義して、
List や Dictionary を使ってデータを格納します。

<pre class="source" title="データを表現するクラス" lang="">
<code><span class="reserved">class</span> <span class="type">Author</span>
{
  <span class="reserved">public string</span> Name;
  <span class="reserved">public</span> <span class="type">DateTime</span> Birthday;
  <span class="reserved">public string</span> Url;
}

<span class="reserved">class</span> <span class="type">Series</span>
{
  <span class="reserved">public string</span> Name;
  <span class="reserved">public string</span> Publisher;
}
</code></pre>


<pre class="source" title="List でデータを格納" lang="">
<code><span class="type">List</span>&lt;<span class="type">Author</span>&gt; authors = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="type">Author</span>&gt; {
  <span class="reserved">new</span> <span class="type">Author</span> {
    Name = <span class="literal">"赤松健"</span>,
    Birthday = <span class="reserved">new</span> <span class="type">DateTime</span>(<span class="literal">1968</span>, <span class="literal">07</span>, <span class="literal">05</span>),
    Url = <span class="literal">"http://www.ailove.net/main.html"</span>
  },
  <span class="reserved">new</span> <span class="type">Author</span> {
    Name = <span class="literal">"久米田康治"</span>,
    Birthday = <span class="reserved">new</span> <span class="type">DateTime</span>(<span class="literal">1967</span>, <span class="literal">09</span>, <span class="literal">05</span>),
    Url = <span class="literal">"http://websunday.net/backstage/kumeta.html"</span>
  },
  <span class="reserved">new</span> <span class="type">Author</span> {
    Name = <span class="literal">"島本和彦"</span>,
    Birthday = <span class="reserved">new</span> <span class="type">DateTime</span>(<span class="literal">1961</span>, <span class="literal">04</span>, <span class="literal">26</span>),
    Url = <span class="literal">"http://simamoto.zenryokutei.com/"</span>
  },
  <span class="reserved">new</span> <span class="type">Author</span> {
    Name = <span class="literal">"藤田和日郎"</span>,
    Birthday = <span class="reserved">new</span> <span class="type">DateTime</span>(<span class="literal">1964</span>, <span class="literal">05</span>, <span class="literal">24</span>),
    Url = <span class="literal">"http://websunday.net/backstage/fujita.html"</span>
  },
};

<span class="type">List</span>&lt;<span class="type">Series</span>&gt; series = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="type">Series</span>&gt; {
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"魔法先生ネギま！"</span>, Publisher = <span class="literal">"講談社"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"ラブひな"</span>, Publisher = <span class="literal">"講談社"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"さよなら絶望先生"</span>, Publisher = <span class="literal">"講談社"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"かってに改蔵"</span>, Publisher = <span class="literal">"小学館"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"アニメ店長"</span>, Publisher = <span class="literal">"一迅社"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"新吼えろペン"</span>, Publisher = <span class="literal">"小学館"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"ゲキトウ"</span>, Publisher = <span class="literal">"講談社"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"からくりサーカス"</span>, Publisher = <span class="literal">"小学館"</span> },
  <span class="reserved">new</span> <span class="type">Series</span> { Name = <span class="literal">"うしおととら"</span>, Publisher = <span class="literal">"小学館"</span> },
};
</code></pre>



##### <a id="sec-generated-title-7"></a>RDB（テーブル）
一方、RDB では、
以下のように、テーブルとしてデータの構造定義・格納します。

<table summary="Authors テーブル">
	<caption>
		Authors テーブル
	</caption>
	<tr>
		<th>Name</th>
		<th>Birthday</th>
		<th>Url</th>
	</tr>
	<tr>
		<td markdown="1">赤松健</td>
		<td markdown="1">1968/07/05</td>
		<td markdown="1">http://www.ailove.net/main.html</td>
	</tr>
	<tr>
		<td markdown="1">久米田康治</td>
		<td markdown="1">1967/09/05</td>
		<td markdown="1">http://websunday.net/backstage/kumeta.html</td>
	</tr>
	<tr>
		<td markdown="1">島本和彦</td>
		<td markdown="1">1961/04/26</td>
		<td markdown="1">http://simamoto.zenryokutei.com/</td>
	</tr>
	<tr>
		<td markdown="1">藤田和日郎</td>
		<td markdown="1">1964/05/24</td>
		<td markdown="1">http://websunday.net/backstage/fujita.html</td>
	</tr>
</table>


<table summary="Series テーブル">
	<caption>
		Series テーブル
	</caption>
	<tr>
		<th>Name</th>
		<th>Publisher</th>
	</tr>
	<tr>
		<td markdown="1">魔法先生ネギま！</td>
		<td markdown="1">講談社</td>
	</tr>
	<tr>
		<td markdown="1">ラブひな</td>
		<td markdown="1">講談社</td>
	</tr>
	<tr>
		<td markdown="1">さよなら絶望先生</td>
		<td markdown="1">講談社</td>
	</tr>
	<tr>
		<td markdown="1">かってに改蔵</td>
		<td markdown="1">小学館</td>
	</tr>
	<tr>
		<td markdown="1">アニメ店長</td>
		<td markdown="1">一迅社</td>
	</tr>
	<tr>
		<td markdown="1">新吼えろペン</td>
		<td markdown="1">小学館</td>
	</tr>
	<tr>
		<td markdown="1">ゲキトウ</td>
		<td markdown="1">講談社</td>
	</tr>
	<tr>
		<td markdown="1">からくりサーカス</td>
		<td markdown="1">小学館</td>
	</tr>
	<tr>
		<td markdown="1">うしおととら</td>
		<td markdown="1">小学館</td>
	</tr>
</table>


まあ、本当は、出版社の情報も別テーブルに持ちたいところですが、
ここでは話を簡単にするために Series テーブル中に含めています。

最初にも少し触れましたが、
この時点では OOP と RDB には大きな差は生まれません。
見た目こそ違いますが、
いずれも、1行1行データが書かれているだけです。


##<a id="sec-generated-title-8"></a> <a id="hierarchy_vs_join"></a>OOP の階層的データ構造と RDB のテーブル結合
まあ、前節のように、データテーブルが独立しているうちは OOP と RDB にはそれほど大きな差は生まれません。
問題は、2つのテーブルの関係性を表すときに生じます。

引き続き、作家とシリーズのデータベースの例で説明しましょう。
作家はいくつかのシリーズを持っていますし、シリーズにはそれぞれ作者がいます。


##### <a id="sec-generated-title-9"></a>OOP（クラスの階層化）
OOP では、通常、階層的なデータ構造を持っています。
作家が複数のシリーズを持っているなら、作家クラスは以下のように書かれます。

<pre class="source" title="Author クラスには Series リストがある" lang="">
<code><span class="reserved">class</span> <span class="type">Author</span>
{
  <span class="reserved">public string</span> Name;
  <span class="reserved">public</span> <span class="type">DateTime</span> Birthday;
  <span class="reserved">public string</span> Url;

  <em><span class="reserved">public</span> <span class="type">List</span>&lt;<span class="type">Series</span>&gt; Series;</em>
}
</code></pre>


また、シリーズに作者があるなら、シリーズクラスは以下のようになります。
（もちろん、本当は1つの本に複数の作者（原作、作画、コンテ構成など）があったりしますが、
ここでは単純化のために、作家は1人だけとします。）

<pre class="source" title="Series クラスには Author フィールドがある" lang="">
<code><span class="reserved">class</span> <span class="type">Series</span>
{
  <span class="reserved">public string</span> Name;
  <span class="reserved">public string</span> Publisher;

  <em><span class="reserved">public</span> <span class="type">Author</span> Author;</em>
}
</code></pre>


で、例えば、各作家の著作一覧を取得したければ以下のように書きます。
階層的にデータを取得するために、2重ループなどを書きます。

<pre class="source" title="各作家の著作一覧を取得" lang="">
<code><span class="reserved">foreach</span> (<span class="type">Author</span> a <span class="reserved">in</span> authors)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}\n"</span>, a.Name);
  <span class="reserved">foreach</span> (<span class="type">Series</span> s <span class="reserved">in</span> a.Series)
  {
    <span class="type">Console</span>.Write(<span class="literal">"  - {0}\n"</span>, s.Name);
  }
}
</code></pre>


また、各シリーズの著者を取得するには以下のようにします。

<pre class="source" title="各シリーズの著者を取得" lang="">
<code><span class="reserved">foreach</span> (<span class="type">Series</span> s <span class="reserved">in</span> series)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}, {1}\n"</span>, s.Name, <em>s.Author.Name</em>);
}
</code></pre>



##### <a id="sec-generated-title-10"></a>RDB（テーブル間の関係）
一方、RDB では、階層的にデータを持つことはできません。
データ上は、以下のように、ID 情報（Series テーブルの Author_Id 列）だけを持っておきます。

<table summary="Authors テーブル">
	<caption>
		Authors テーブル
	</caption>
	<tr>
		<th>Id</th>
		<th>Name</th>
		<th>Birthday</th>
		<th>Url</th>
	</tr>
	<tr>
		<td markdown="1">1</td>
		<td markdown="1">赤松健</td>
		<td markdown="1">1968/07/05</td>
		<td markdown="1">http://www.ailove.net/main.html</td>
	</tr>
	<tr>
		<td markdown="1">2</td>
		<td markdown="1">久米田康治</td>
		<td markdown="1">1967/09/05</td>
		<td markdown="1">http://websunday.net/backstage/kumeta.html</td>
	</tr>
	<tr>
		<td markdown="1">3</td>
		<td markdown="1">島本和彦</td>
		<td markdown="1">1961/04/26</td>
		<td markdown="1">http://simamoto.zenryokutei.com/</td>
	</tr>
	<tr>
		<td markdown="1">4</td>
		<td markdown="1">藤田和日郎</td>
		<td markdown="1">1964/05/24</td>
		<td markdown="1">http://websunday.net/backstage/fujita.html</td>
	</tr>
</table>


<table summary="Series テーブル">
	<caption>
		Series テーブル
	</caption>
	<tr>
		<th>Name</th>
		<th>Publisher</th>
		<th>Author_Id</th>
	</tr>
	<tr>
		<td markdown="1">魔法先生ネギま！</td>
		<td markdown="1">講談社</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">ラブひな</td>
		<td markdown="1">講談社</td>
		<td markdown="1">1</td>
	</tr>
	<tr>
		<td markdown="1">さよなら絶望先生</td>
		<td markdown="1">講談社</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<td markdown="1">かってに改蔵</td>
		<td markdown="1">小学館</td>
		<td markdown="1">2</td>
	</tr>
	<tr>
		<td markdown="1">アニメ店長</td>
		<td markdown="1">一迅社</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<td markdown="1">新吼えろペン</td>
		<td markdown="1">小学館</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<td markdown="1">ゲキトウ</td>
		<td markdown="1">講談社</td>
		<td markdown="1">3</td>
	</tr>
	<tr>
		<td markdown="1">からくりサーカス</td>
		<td markdown="1">小学館</td>
		<td markdown="1">4</td>
	</tr>
	<tr>
		<td markdown="1">うしおととら</td>
		<td markdown="1">小学館</td>
		<td markdown="1">4</td>
	</tr>
</table>


そして、問い合わせの際に、
ID を元に2つのテーブルを結合してから所望のデータを取り出します。

例えば、OOP の例と同じく、
各作家のシリーズ一覧を取得したければ、以下のような SQL 文を書きます。

<pre class="source" title="INNER JOIN で結合" lang="">
<code><span class="reserved">SELECT</span> <span class="type">[a]</span>.<span class="type">[Name]</span> <span class="reserved">AS</span> <span class="type">[AuthorName]</span>, <span class="type">[s]</span>.<span class="type">[Name]</span>
  <span class="reserved">FROM</span> <span class="type">[Authors]</span> <span class="reserved">AS</span> <span class="type">[a]</span>
  <span class="inactive">INNER JOIN</span> <span class="type">[Series]</span> <span class="reserved">AS</span> <span class="type">[s]</span> <span class="reserved">ON</span> <span class="type">[a]</span>.<span class="type">[Id]</span> = <span class="type">[s]</span>.<span class="type">[Author_Id]</span>
</code></pre>


このように、OOP と RDB には、階層的データ構造とテーブル結合という方法論の差があります。


##<a id="sec-generated-title-11"></a> <a id="orm"></a>O/R マッパー
前節のおさらいになりますが、
OOP では階層的データ構造を、

<pre class="source" title="OOP の階層的データ構造" lang="">
<code><span class="reserved">class</span> <span class="type">Author</span>
{
  <span class="reserved">public string</span> Name;
  <span class="reserved">public</span> <span class="type">DateTime</span> Birthday;
  <span class="reserved">public string</span> Url;

  <em><span class="reserved">public</span> <span class="type">List</span>&lt;<span class="type">Series</span>&gt; Series;</em>
}
</code></pre>


RDB ではテーブル結合という方法を用いて関連性のあるデータにアクセスします。

<pre class="source" title="RDB のテーブル結合" lang="">
<code><span class="reserved">SELECT</span> <span class="type">[a]</span>.<span class="type">[Name]</span> <span class="reserved">AS</span> <span class="type">[AuthorName]</span>, <span class="type">[s]</span>.<span class="type">[Name]</span>
  <span class="reserved">FROM</span> <span class="type">[Authors]</span> <span class="reserved">AS</span> <span class="type">[a]</span>
  <span class="inactive">INNER JOIN</span> <span class="type">[Series]</span> <span class="reserved">AS</span> <span class="type">[s]</span> <span class="reserved">ON</span> <span class="type">[a]</span>.<span class="type">[Id]</span> = <span class="type">[s]</span>.<span class="type">[Author_Id]</span>
</code></pre>


近年、プログラミング言語からリレーショナルデータベースにアクセスする機会が増え、
この OOP と RDB の方法論の差、
すなわち、このページの冒頭で話をした O/R インピーダンスミスマッチを解消したいという要望が強くなっています。
下位のデータベースを意識せず、普通に OOP の作法でプログラミングするだけで RBD とのデータのやり取りがしたいわけです。

このような、OOP の作法でデータベース アクセスするための仕組みを O/R マッパー（O/R mapper）と呼びます。


##### <a id="sec-generated-title-12"></a>LINQ で O/R マッピング
「[LINQ](sp3_linq.md#linq)」は、様々な種類のデータに対して、統一的な問い合わせを行うための仕組みです。
この「さまざまな種類のデータ」にはデータベースも含まれています。
すなわち、O/R マッピング用の LINQ が存在します。

歴史的背景から、LINQ O/R マッパーには、.NET Framework 標準搭載のものだけで、2系統あります。

* LINQ to SQL: いわば、LINQ（IQueryable）の参考実装（C# コンパイラー チームが作成したもの）で、ある意味「簡易実装」な O/R マッパー。 今後の機能改善はされない予定。

* ADO.NET Entity Framework: ちゃんとデータベース フレームワーク（ADO.NET）チームが開発している O/R マッパー。 以下、単に Entity Framework と表記します。


LINQ to SQL の方が成熟が早かった（LINQ 自体と同時に開発していたので当然）ため、
当初は LINQ to SQL の方がよく利用されていました。
今（※2011年執筆）では Entity Framework もだいぶ成熟しているので、
こちらを使うべきでしょう。

ただし、LINQ to SQL の方が仕組みがシンプルな分、移植がしやすく、
Entity Framework が使えない環境でも LINQ to SQL なら使える（移植できる）ということもあります。
例えば、Windows Phone 7 向けの O/R マッパーは LINQ to SQL がベースとなっています。


##<a id="sec-generated-title-13"></a> <a id="linq"></a>LINQ の利用
###<a id="sec-generated-title-14"></a> <a id="entity"></a>エンティティ
まず、データベース上のテーブルに相当するクラス（これをエンティティ（entity: 本質、実体）と呼びます）を定義します。

<em>※以下、ADO.NET Entity Framework 4.1（2011年時点の最新版）を使った説明をします。</em>
（「過去の履歴」として、LINQ to SQL 版の説明も残してあります。）

Entity Framework では、何の変哲もないただのクラスを使ってデータベースのテーブルを生成/参照できます。

前節から引き続き、作家・シリーズ テーブルを例に取って説明しましょう。
まず、テーブル間の関係を抜きにすると、以下のような感じになります。

<pre class="source" title="エンティティ クラスの定義" lang="">
<code><span class="reserved">using</span> System.ComponentModel.DataAnnotations;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">namespace</span> CodeFirst.Models
{
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Author</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> Id { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">Required</span>]
        [<span class="type">StringLength</span>(100)]
        <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <span class="reserved">public</span> <span class="type">DateTime</span>? Birthday { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">StringLength</span>(512)]
        <span class="reserved">public</span> <span class="reserved">string</span> Url { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    }

    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Series</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> Id { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">Required</span>]
        [<span class="type">StringLength</span>(512)]
        <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    }
}
</code></pre>


次に、
定義したエンティティ クラスを使ってデータベースを生成/テーブル参照するためのクラスを作ります。
Entity Framework では、以下のように、DbContext クラスを継承したクラスを作ります。

<pre class="source" title="エンティティ クラスの定義" lang="">
<code><span class="reserved">using</span> System.Data.Entity;

<span class="reserved">namespace</span> CodeFirst.Models
{
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">ComicDatabase</span> : <span class="type">DbContext</span>
    {
        <span class="reserved">public</span> <span class="type">DbSet</span>&lt;<span class="type">Author</span>&gt; Authors { <span class="reserved">get</span>; <span class="reserved">set</span>; }
        <span class="reserved">public</span> <span class="type">DbSet</span>&lt;<span class="type">Series</span>&gt; Series { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    }
}
</code></pre>


<span class="expand-button" title="展開/折畳">（LINQ to SQL 版）</span>
<div class="expand-panel" markdown="1" title="（LINQ to SQL 版）">
                
エンティティクラスには Table 属性を、
エンティティのメンバー（テーブルの列に相当）には Column 属性を付けます。

                
例えば、前節までの説明で使った作家/シリーズ テーブルの場合、
以下のような感じになります。

                
<pre class="source" title="エンティティ定義" lang="">
<code><span class="reserved">using</span> System.Data.Linq.Mapping;

[<span class="type">Table</span>(Name = <span class="literal">"Authors"</span>)]
<span class="reserved">public class</span> <span class="type">Author</span>
{
  [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsPrimaryKey = <span class="reserved">true</span>, IsDbGenerated = <span class="reserved">true</span>)]
  <span class="reserved">public int</span> Id;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Name;
  [<span class="type">Column</span>]
  <span class="reserved">public</span> <span class="type">DateTime</span>? Birthday;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Url;
}

[<span class="type">Table</span>(Name = <span class="literal">"Series"</span>)]
<span class="reserved">public class</span> <span class="type">Series</span>
{
  [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsPrimaryKey = <span class="reserved">true</span>, IsDbGenerated = <span class="reserved">true</span>)]
  <span class="reserved">public int</span> Id;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Name;
  [<span class="type">Column</span>]
  <span class="reserved">public int</span> AuthorId;
}
</code></pre>


                
LINQ to SQL では、これらの属性をみて、
クラスのメンバーアクセスを RDB への SQL 問い合わせに変換します。

                
この例では、Author と Series の Id の Column 属性には、AutoSync などのパラメータが付いています。
これは、「データベース側で自動的に生成されるユニークな ID で、自動的にデータベース側と同期します」という意味になります。
通常、重複のない一意的な整数値がほしい場合、このような設定をします。


                

##### <a id="sec-generated-title-15"></a>DataContext
次に、データベースサーバに接続するためのクラス（DataContext）を作ります。

                
先ほどの Author と Series テーブルにアクセスするためには、
以下のようなクラスを作ります。

                
<pre class="source" title="ComicDataContext" lang="">
<code><span class="reserved">using</span> System.Data.Linq;

<span class="reserved">public class</span> <span class="type">ComicDataContext</span> : <span class="type">DataContext</span>
{
  <span class="reserved">public</span> ComicDataContext(<span class="reserved">string</span> connectionString)
    : <span class="reserved">base</span>(connectionString)
  {
  }

  <span class="reserved">public</span> <span class="type">Table</span>&lt;<span class="type">Author</span>&gt; Author;
  <span class="reserved">public</span> <span class="type">Table</span>&lt;<span class="type">Series</span>&gt; Series;
}
</code></pre>


                
DataContext を継承するクラスに、Table 型のメンバーを書くだけです。
各 Table の初期化は、親クラスの DataContext のコンストラクタ中で、
「[リフレクション](../dynamic/sp_reflection.md#reflection)」機能を使って行われます。
なので、最低限、コンストラクタと Table メンバーだけ書けば LINQ to SQL で利用可能です。

            
</div>


###<a id="sec-generated-title-16"></a> <a id="query"></a>IQueryable とクエリ式
例えば、Author テーブルに対するクエリは以下のように書けます。

<pre class="source" title="クエリの例" lang="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">ComicDatabase</span>())
{
    <span class="reserved">var</span> q =
        <span class="reserved">from</span> a <span class="reserved">in</span> db.Authors
        <span class="reserved">where</span> a.Name == <span class="literal">"島本和彦"</span> || a.Name == <span class="literal">"赤松健"</span>
        <span class="reserved">select</span> a;

    <span class="reserved">foreach</span> (<span class="reserved">var</span> a <span class="reserved">in</span> q)
    {
        <span class="type">Console</span>.Write(<span class="literal">"{0}, {1:yyyy/M/d}, {2}\n"</span>, a.Name, a.Birthday, a.Url);
    }
}
</code></pre>


<pre class="console" title="実行結果">
赤松健, 1968/7/5, http://www.ailove.net/main.html
島本和彦, 1961/4/26, http://simamoto.zenryokutei.com/
</pre>


<span class="expand-button" title="展開/折畳">（LINQ to SQL 版）</span>
<div class="expand-panel" markdown="1" title="（LINQ to SQL 版）">
                
<pre class="source" title="" lang="">
<code><span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">ComicDataContext</span>(ConnectionString);

<span class="reserved">var</span> q =
  <span class="reserved">from</span> a <span class="reserved">in</span> db.Author
  <span class="reserved">where</span> a.Name == <span class="literal">"島本和彦"</span> || a.Name == <span class="literal">"赤松健"</span>
  <span class="reserved">select</span> a;

<span class="reserved">foreach</span> (<span class="reserved">var</span> a <span class="reserved">in</span> q)
{
  <span class="type">Console</span>.Write(<span class="literal">"{0}, {1:yyyy/M/d}, {2}\n"</span>, a.Name, a.Birthday, a.Url);
}
</code></pre>


                
Table クラスは IQueryable インターフェースを実装していて、
このクエリ式は IQueryable に対する操作になります。
「[クエリ式](sp3_linq.md#query)」で説明したように、
C# 3.0 のクエリ式は、実際には Select や Where などといった名前のメソッド（あるいは拡張メソッド）呼び出しになります。
この例の場合、以下のメソッド呼び出しと同じ意味です。


                
<pre class="source" title="クエリ式の解釈結果" lang="">
<code><span class="type">IQueryable</span>&lt;<span class="type">Author</span>&gt; q = db.Author.Where(
  a =&gt; a.Name == <span class="literal">"島本和彦"</span> || a.Name == <span class="literal">"赤松健"</span>);
</code></pre>


            
</div>

この時、クエリ式の結果（この例では変数 q）の型は IQueryable インターフェイスになります。

IQueryable は、このようなクエリ式から SQL 文を生成し、データベースサーバに問い合わせを行います。
ちなみに、IQueryable を ToString すると、生成された SQL 文を確認することができます。

<pre class="source" title="クエリ式の解釈結果の確認" lang="">
<code><span class="reserved">var</span> q =
  <span class="reserved">from</span> a <span class="reserved">in</span> db.Author
  <span class="reserved">where</span> a.Name == <span class="literal">"島本和彦"</span> || a.Name == <span class="literal">"赤松健"</span>
  <span class="reserved">select</span> a;

<span class="type">Console</span>.WriteLine(q.ToString());
</code></pre>


<pre class="console" title="実行結果">
SELECT
[Extent1].[Id] AS [Id],
[Extent1].[Name] AS [Name],
[Extent1].[Kana] AS [Kana],
[Extent1].[Birthday] AS [Birthday],
[Extent1].[Url] AS [Url]
FROM [dbo].[Authors] AS [Extent1]
WHERE [Extent1].[Name] IN (N'島本和彦',N'赤松健')
</pre>



###<a id="sec-generated-title-17"></a> <a id="association"></a>エンティティ間の関係性
それでは次に、
Author と Series エンティティ間の関係性を記述します。

Entity Framework を使うと、ただ単に他のエンティティを参照するプロパティを定義するだけで、
データベース テーブルの関係性を表現できます。
例えば、先ほどの Author / Series クラスに以下のような修正を加えます。

<pre class="source" title="テーブル間の関係性を、エンティティ クラスの階層構造で表現" lang="">
<code>    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Author</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> Id { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">Required</span>]
        [<span class="type">StringLength</span>(100)]
        <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <span class="reserved">public</span> <span class="type">DateTime</span>? Birthday { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">StringLength</span>(512)]
        <span class="reserved">public</span> <span class="reserved">string</span> Url { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <em><span class="reserved">public</span> <span class="reserved">virtual</span> <span class="type">IList</span>&lt;<span class="type">Series</span>&gt; Series { <span class="reserved">get</span>; <span class="reserved">set</span>; }</em>
    }

    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Series</span>
    {
        <span class="reserved">public</span> <span class="reserved">int</span> Id { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        [<span class="type">Required</span>]
        [<span class="type">StringLength</span>(512)]
        <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }

        <em><span class="reserved">public</span> <span class="type">Author</span> Author { <span class="reserved">get</span>; <span class="reserved">set</span>; }</em>
    }
</code></pre>


このような、エンティティ間の参照関係を表すプロパティをナビゲーション プロパティ（navigation property）と呼びます。
ナビゲーション プロパティは、データベース上は ID 情報だけ記録され、
参照時に適宜、テーブルの JOIN が行われます。

<span class="expand-button" title="展開/折畳">（LINQ to SQL 版）</span>
<div class="expand-panel" markdown="1" title="（LINQ to SQL 版）">
                
<pre class="source" title="エンティティ間の関連性" lang="">
<code><span class="reserved">using</span> System.Data.Linq.Mapping;

[<span class="type">Table</span>(Name = <span class="literal">"Authors"</span>)]
<span class="reserved">public class</span> <span class="type">Author</span>
{
  [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsPrimaryKey = <span class="reserved">true</span>, IsDbGenerated = <span class="reserved">true</span>)]
  <span class="reserved">public int</span> Id;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Name;
  [<span class="type">Column</span>]
  <span class="reserved">public</span> <span class="type">DateTime</span>? Birthday;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Url;

  [<span class="type">Association</span>(OtherKey = <span class="literal">"AuthorId"</span>)]
  <span class="reserved">public</span> <span class="type">EntitySet</span>&lt;<span class="type">Series</span>&gt; Series;
}

[<span class="type">Table</span>(Name = <span class="literal">"Series"</span>)]
<span class="reserved">public class</span> <span class="type">Series</span>
{
  [<span class="type">Column</span>(AutoSync = <span class="type">AutoSync</span>.OnInsert, IsPrimaryKey = <span class="reserved">true</span>, IsDbGenerated = <span class="reserved">true</span>)]
  <span class="reserved">public int</span> Id;
  [<span class="type">Column</span>]
  <span class="reserved">public string</span> Name;
  [<span class="type">Column</span>]
  <span class="reserved">public int</span> AuthorId;

  [<span class="type">Association</span>(Storage = <span class="literal">"_Author"</span>, ThisKey = <span class="literal">"AuthorId"</span>)]
  <span class="reserved">public</span> <span class="type">Author</span> Author
  {
    <span class="reserved">get</span> { <span class="reserved">return this</span>._Author.Entity; }
    <span class="reserved">set</span> { <span class="reserved">this</span>._Author.Entity = <span class="reserved">value</span>; }
  }
  <span class="reserved">private</span> <span class="type">EntityRef</span>&lt;<span class="type">Author</span>&gt; _Author;
}
</code></pre>


                
EntitySet や EntityRef クラスのデータへのアクセスは、
必要になったときに初めてデータベースサーバに問い合わせを行います。
すなわち、初回アクセス時にのみサーバからデータをロードし、
取得済みのデータがすでにあるならその値を返します。

                
Author は複数の Series を持っているので EntitySet、
Series は（今回の例では）ただ1人の Author を持つので EntityRef を使います。

                
OOP における階層的データ構造は、
RDB ではテーブル結合で行うわけですが、
結合の際のキーを Association 属性のパラメータに与えます。
この例では、
Author の主キー（IsPrimaryKey = true）である Id と Series の AuthorId の値によってテーブルを結合するので、
Author 側には OtherKey = "AuthorId" を、
Series 側には ThisKey = "AuthorId" を指定します。

            
</div>

これで、Author.Series や Series.Author の値が必要になった際に、
自動的にテーブル結合を行うような SQL 文が生成されます。

<pre class="source" title="自動的にテーブル結合を行うクエリが作られる" lang="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> db = <span class="reserved">new</span> <span class="type">ComicDatabase</span>())
{
    <span class="reserved">var</span> q =
        <span class="reserved">from</span> s <span class="reserved">in</span> db.Series
        <span class="reserved">where</span> s.Name.Contains(<span class="literal">"先生"</span>)
        <span class="reserved">select</span> <span class="reserved">new</span> { Title = s.Name, Author = s.Author.Name };

    <span class="reserved">foreach</span> (<span class="reserved">var</span> s <span class="reserved">in</span> q)
    {
        <span class="type">Console</span>.Write(<span class="literal">"{0}, {1}\n"</span>, s.Title, s.Author);
    }

    <span class="type">Console</span>.Write(<span class="literal">"\n{0}\n"</span>, q);
}
</code></pre>


<pre class="console" title="実行結果">
魔法先生ネギま！, 赤松健
さよなら絶望先生, 久米田康治

SELECT
1 AS [C1],
[Extent1].[Name] AS [Name],
[Extent2].[Name] AS [Name1]
FROM  [dbo].[Series] AS [Extent1]
<em>LEFT OUTER JOIN [dbo].[Authors] AS [Extent2] ON [Extent1].[Author_Id] = [Extent2].[Id]</em>
WHERE [Extent1].[Name] LIKE N'%先生%'
</pre>



##<a id="sec-generated-title-18"></a> <a id="summary"></a>まとめ
* オブジェクト指向プログラミング（OOP）言語とリレーショナルデータベース（RDB）の間には、
    * OOP： 階層的データ構造

    * RDB： テーブル結合

という方法論の差があります（O/R インピーダンスミスマッチ）。

* Entity Framework を利用する際に使う物:
    * エンティティ: データベースのテーブルに相当するクラスを定義。

    * 関連性: 他のエンティティを参照するプロパティ（ナビゲーション プロパティ）を定義することでテーブル間の関係性を定義。

    * データベース コンテキスト: 定義したエンティティを使ってデータベースのテーブル生成/参照するためのクラスを定義。




<span class="expand-button" title="展開/折畳">（LINQ to SQL 版）</span>
<div class="expand-panel" markdown="1" title="（LINQ to SQL 版）">
            
* LINQ to SQL では、OOP の階層的データアクセスから、テーブル結合を行うような SQL クエリを自動的に生成します。

* LINQ to SQL を利用する際には、
    * エンティティ： Table 属性付きのクラスに、Column 属性付きのメンバーを定義

    * 関連性： Association 属性を付けた EntityRef / EntitySet 型のメンバーを定義

    * DataContext： DataContext クラスに Table 型のメンバーを定義

を作る。


        
</div>

次章の「[[雑記] LINQ to SQL 実践編](sp3_linqtosql.md)」では、
Visual Studio を使ってデータベースのテーブル定義 → LINQ to SQL クラス化 → クエリ式を使ったプログラム作成という一連の作業を具体的に説明します。
