---
title: "その他のライブラリ"
source_url: "https://ufcpp.net/study/csharp/lib/lib_other/"
content_type: "Article"
published_at: "2001-12-31T00:00:00"
updated_at: "2019-05-26T17:37:05"
tags: []
umbraco_id: 1356
parent_id: 1350
sort_order: 4
aliases:
  - "/csharp/lib/lib_other/"
  - "/csharp/lib_other"
  - "/csharp/lib_other.html"
  - "/study/csharp/lib_other"
  - "/study/csharp/lib_other.html"
---

# その他のライブラリ

##<a id="sec-generated-title-1"></a> <a id="math"></a>数学関数
System.Math クラスに、数学用の関数・定数などが定義されています。
表1に Math クラスのメンバーを示します（全て static）。

<table summary="Math クラスのメンバー">
	<caption>
		Math クラスのメンバー
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>メンバー名</th>
		<th>意味</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">定数</td>
		<td markdown="1"><code>PI</code></td>
		<td markdown="1">円周率。</td>
	</tr>
	<tr>
		<td markdown="1"><code>E</code></td>
		<td markdown="1">自然対数の底</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="5">指数・対数関数</td>
		<td markdown="1"><code>Exp(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">exp</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Pow(x, y)</code></td>
		<td markdown="1"><span class="math">
            x<sup>y</sup>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Log(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">log</span>
            <sub>e</sub> x
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Log(x, y)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">log</span>
            <sub>y</sub> x
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Log10(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">log</span>
            <sub><span class="normal">10</span></sub> x
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">三角関数</td>
		<td markdown="1"><code>Sin(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">sin</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Cos(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">cos</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Tan(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">tan</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="4">逆三角関数</td>
		<td markdown="1"><code>Asin(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">sin</span>
            <sup><span class="normal">−1</span></sup>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Acos(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">cos</span>
            <sup><span class="normal">−1</span></sup>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Atan(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">tan</span>
            <sup><span class="normal">−1</span></sup>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Atan2(y, x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">tan</span>
            <sup><span class="normal">−1</span></sup>
            <span class="paren" style="font-size:2em;">(</span>
              <table class="frac" summary="fraction"><tr><td class="num">y</td></tr><tr><td>x</td></tr></table>
            <span class="paren" style="font-size:2em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">双曲線関数</td>
		<td markdown="1"><code>Sinh(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">sinh</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Cosh(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">cosh</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1"><code>Tanh(x)</code></td>
		<td markdown="1"><span class="math">
            <span class="normal">tanh</span>
            <span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
          </span></td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">整数化</td>
		<td markdown="1"><code>Floor(x)</code></td>
		<td markdown="1">x の床（x 以下の最大の整数）。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Ceiling(x)</code></td>
		<td markdown="1">x の天井（x 以上の最小の整数）。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Round(x)</code></td>
		<td markdown="1">x を四捨五入。</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">その他の数学関数</td>
		<td markdown="1"><code>Abs(x)</code></td>
		<td markdown="1">x の絶対値。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Sign(x)</code></td>
		<td markdown="1">x 符号。x が正ならば1、負ならば－1、0ならば0。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Sqrt(x)</code></td>
		<td markdown="1">x の平方根。</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">最大・最小</td>
		<td markdown="1"><code>Max(x, y)</code></td>
		<td markdown="1">x, y のうち、大きい方を帰す。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Min(x, y)</code></td>
		<td markdown="1">x, y のうち、小さい方を帰す。</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">その他</td>
		<td markdown="1"><code>BigMul(x, y)</code></td>
		<td markdown="1"><code>int</code>×<code>int</code>で<code>long</code>を帰す乗算を行う。</td>
	</tr>
	<tr>
		<td markdown="1"><code>DivRem(x, y, out res)</code></td>
		<td markdown="1">商と剰余を同時に計算する。 res に<code>x % y</code>を代入し、<code>x / y</code>を帰す。</td>
	</tr>
	<tr>
		<td markdown="1"><code>IEEERemainder(x, y)</code></td>
		<td markdown="1">剰余を計算する。<code>x % y</code>が<code>x - Math.Truncate(x / y) * y</code>なのに対して、 この関数は<code>x - Math.Round(x / y) * y</code>を帰す。</td>
	</tr>
</table>


<pre class="source" title="数学関数の例" lang="">
<code>Console.Write(<span class="literal">"{0}\n"</span>, Math.Sin(2.0 / 3.0 * Math.PI));
Console.Write(<span class="literal">"{0}\n"</span>, Math.Log10(10000));
Console.Write(<span class="literal">"{0}\n"</span>, Math.Pow(2, 8));
</code></pre>


<pre class="console" title="数学関数の例">
0.866025403784439
4
256
</pre>


以下、何点か補足。


##### <a id="sec-generated-title-2"></a>Log と Log10
自然対数と常用対数については、
「[常用対数と自然対数](../../math/hs/m2.md#log_e)」を参照。


##### <a id="sec-generated-title-3"></a>Atan2
Math.Atan2、
C 言語にも atan2 という関数があるんですが、
意外と知らない人が多いみたい。

直交座標 <span class="math">
        <span class="paren" style="font-size:em;">(</span>x, y<span class="paren" style="font-size:em;">)</span>
      </span> → 極座標 <span class="math">
        <span class="paren" style="font-size:em;">(</span>r, θ<span class="paren" style="font-size:em;">)</span>
      </span> の変換とか、
複素数 <span class="math">
        z <span class="normal">=</span> x <span class="normal">+</span> i y
      </span> の偏角 <span class="math">
        <span class="normal">arg</span> z
      </span> とかを求めたいときに使う。
（
atan(y / x) だと、<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">1</span>, <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> も <span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span><span class="normal">1</span>, <span class="normal">−</span><span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
      </span> も atan(1) になっちゃって、π/4 になってしまうので。
）

数学っぽく書くなら、<code>atan2(y, x)</code> ＝ <span class="math">
        <span class="normal">arg</span>
        <span class="paren" style="font-size:em;">(</span>x + i y<span class="paren" style="font-size:em;">)</span>
      </span> です。


##### <a id="sec-generated-title-4"></a>Round
上の表では“四捨五入”と説明しましたが、
正確には、ぴったり真ん中（例えば 0.5, 1.5, 2.5, ・・・）のときの動作は四捨五入ではありません。

Round 関数は、第2引数に「ぴったり真ん中のときの丸めをどうするか」を指定することが出来て、
通常は MidpointRounding.ToEven になっています。
これは“偶数丸め”と呼ばれているもので、
0.5 → 0、1.5 → 2、2.5 → 2、3.5 → 4、 4.5 → 4 ・・・
というように、必ず偶数に向かって丸めます。

なぜこんなことをするかというと、
この方式が一番誤差の蓄積が少ないから。
“切り上げ”と“切り下げ”が半々なので、
丸めた数値を足し合わせていったとき、
丸め誤差が打ち消しあってくれる確率が高くなります。

一方、日本語の文字通りの四捨五入（ぴったり真ん中のときは切り上げ）をしたければ、
MidpointRounding.AwayFromZero を指定します。
こちらの方が演算量は小さくて、
精度よりも演算量優先の場合はこちらを指定します。
（要するに、0.5 を足して切り捨てるだけなので。
ToEven の場合は、0.5 のときに条件分岐したりテーブル参照したりが必要。）


##<a id="sec-generated-title-5"></a> <a id="datetime"></a>時刻
時刻は System.DateTime で、
時刻の差、すなわち、経過時間は System.TimeSpan クラスで表されます。

<pre class="source" title="DateTime と TimeSpan" lang="">
<code>DateTime t = DateTime.Now;
Console.Write(<span class="literal">"{0}\n"</span>, t);
Console.Write(<span class="literal">"{0}/{1,2}/{2,2} ({3}) {4,2}:{5:d02}:{6:d02}\n"</span>,
  t.Year, t.Month, t.Day, t.DayOfWeek,
  t.Hour, t.Minute, t.Second);

Console.Write(<span class="literal">"エンターキーを押して"</span>);
Console.ReadLine();

TimeSpan ts = DateTime.Now - t;
Console.Write(<span class="literal">"キーを押すまでの時間: {0}[ms]"</span>, ts.TotalSeconds);
</code></pre>


<pre class="console" title="DateTime と TimeSpan">
2005/09/21 16:51:44
2005/ 9/21 (Wednesday) 16:51:44
エンターキーを押して
キーを押すまでの時間: 2.6738448[ms]
</pre>



##<a id="sec-generated-title-6"></a> <a id="collection"></a>コレクション
System.Collections 名前空間以下に、
さまざまなコレクションクラスがあります。

詳細説明に別ページを儲けました: 「[コレクション](../../dotnet/bcl/bcl_collection.md)」

どのコレクションがどういう動作をするかは、
「[コレクション概要](../../algorithm/collection/collection.md)」も参照。

<table summary="コレクションクラス">
	<caption>
		コレクションクラス
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>クラス名</th>
		<th>概要</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">シーケンス</td>
		<td markdown="1"><code>ArrayList</code></td>
		<td markdown="1">配列で実装されたリストです。「[インデクサー](../oop/oo_indexer.md#indexer)」による要素のランダムアクセスが可能です。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Stack</code></td>
		<td markdown="1">FILO（first in last out：先入れ後出し）式のコレクション。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Queue</code></td>
		<td markdown="1">FIFO（first in fast out：先入れ先出し）式のコレクション。</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">辞書</td>
		<td markdown="1"><code>Hashtable</code></td>
		<td markdown="1">名前の通り、ハッシュテーブルで実装された辞書。 (キー, 値)のペアの順序は完全に失われます。 値の挿入も、キーによる検索も高速です。 （十分に大きなキャパシティにしておけば、非常に高速）</td>
	</tr>
	<tr>
		<td markdown="1"><code>SortedList</code></td>
		<td markdown="1">整列済みの配列で実装された辞書。 (キー, 値)のペアは、 キーの大小によってソートされた状態になります。 値の挿入には時間がかかりますが、 キーによる検索は非常に高速です。 （二分探索アルゴリズムによる検索を行います。）</td>
	</tr>
	<tr>
		<td markdown="1">ビット配列</td>
		<td markdown="1"><code>BitArray</code></td>
		<td markdown="1">例えば、ある変数 x の n ビット目が1か0かを調べるには、<code>(x &amp; (1 &lt;&lt; (n - 1))) != 0</code>と言うように書きますが、このビット配列を用いると、<code>BitArray a; a[n]</code>というように書けます。</td>
	</tr>
</table>


少し補足すると、
シーケンスと言うのは順番に意味のあるコレクションの事をいいます。
int 型で番号を指定して、インデクサで <code>a[i]</code> と言うようにアクセスできたり、
「先に入れた値ほど先に出てくる」、
「後に入れた値ほど先に出てくる」など、値の追加・取り出しに順序があります。

一方、辞書というのは、値とキーのペアを持っていて、
キーによって値を検索できるものです。
例えば、キーの型を string、値の型を int とすると、
<code>a["keyword"] = 5;</code> というように、
キーの型を引数とするインデクサによる値の読み書きができます。

<h5 class="version version2">Ver. 2.0</h5>

.NET Framework 2.0 では、「[ジェネリック](../oop/sp2_generics.md#generics)」の導入に伴い、
ジェネリック版のコレクションクラスが追加されました。
ジェネリックコレクションクラスは
System.Collections.Generic 名前空間以下にあります。

<table summary="ジェネリックコレクション">
	<caption>
		ジェネリックコレクション
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>クラス名</th>
		<th>概要</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="4">シーケンス</td>
		<td markdown="1"><code>List</code></td>
		<td markdown="1">非ジェネリック版の ArrayList に相当。 配列で実装されたリストです。</td>
	</tr>
	<tr>
		<td markdown="1"><code>LinkedList</code></td>
		<td markdown="1">連結リストです。 要素のランダムアクセスはできませんが、 シーケンスの末尾以外への要素の挿入が高速に行えます。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Stack</code></td>
		<td markdown="1">非ジェネリック版と同様。 FILO 式のコレクションです。</td>
	</tr>
	<tr>
		<td markdown="1"><code>Queue</code></td>
		<td markdown="1">非ジェネリック版と同様。 FIFO 式のコレクションです。</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">辞書</td>
		<td markdown="1"><code>Dictionary</code></td>
		<td markdown="1">非ジェネリック版の Hashtable に相当。 ハッシュテーブルで実装された辞書です。</td>
	</tr>
	<tr>
		<td markdown="1"><code>SortedDictionary</code></td>
		<td markdown="1">二分探索木（赤黒木）で実装された辞書。 (キー, 値)のペアは、 キーの大小によってソートされた状態になります。 値の挿入も、キーによる検索も高速です。 （できること自体はハッシュテーブルと二分探索木に大きな差はありませんが、 演算量やメモリ使用量などの点でそれぞれ一長一短あります。）</td>
	</tr>
	<tr>
		<td markdown="1"><code>SortedList</code></td>
		<td markdown="1">非ジェネリック版と同様。 整列済みの配列で実装された辞書。</td>
	</tr>
</table>
