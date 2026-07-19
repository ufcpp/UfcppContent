---
title: "既定値"
source_url: "https://ufcpp.net/study/csharp/resource/rm_default/"
content_type: "Article"
published_at: "2014-10-06T00:00:00"
updated_at: "2017-08-15T00:00:00"
tags: []
umbraco_id: 1289
parent_id: 1286
sort_order: 4
aliases:
  - "/csharp/resource/rm_default/"
  - "/csharp/rm_default"
  - "/csharp/rm_default.html"
  - "/study/csharp/rm_default"
  - "/study/csharp/rm_default.html"
---

# 既定値

##<a id="sec-generated-title-1"></a> <a id="abst">概要</a>
C# はメモリ領域の未初期化を認めていません。
明示的な初期化を行わない場合、状況に応じて、コンパイル エラーになるか、既定値が入るかのどちらかです。

##<a id="sec-generated-title-2"></a> <a id="uninitialized">補足: 未初期化領域</a>
C# で気にする場面はほとんどありませんが、プログラミング言語によっては、未初期化の状態のメモリにアクセスできてしまう場合があります。
(特に、いわゆる低レイヤーな言語ほどそういうことが可能です。C# でも、「[unsafe](../interop/sp_unsafe.md#unsafe)」 コード内では起こり得ます。)

C++ を例に挙げてみましょう。
C++では、`new[]` で確保したばかりで初期化していないメモリ領域がどうなっているかは未定義(コンパイラーの裁量任せ)になっています。
以下のようなコードを見てください。

<pre class="source" title="" lang="">
<code><span class="reserved">#include</span> <span class="literal">&lt;stdio.h&gt;</span>

<span class="reserved">void</span> main()
{
    <span class="reserved">int</span>* x = <span class="reserved">new int</span>[1];
    x[0] = 0xFFFFFFFF; <span class="comment">// ちゃんと初期化</span>
    printf(<span class="literal">"%08x\n"</span>, x[0]);

    <span class="reserved">int</span>* px = x;
    <span class="reserved">delete</span> x;
    printf(<span class="literal">"%08x\n"</span>, px[0]); <span class="comment">// 削除済みの領域にアクセス</span>

    <span class="reserved">int</span>* y = <span class="reserved">new int</span>[1];
    printf(<span class="literal">"%08x\n"</span>, y[0]); <span class="comment">// 未初期化</span>
}
</code></pre>


この時、ちゃんと初期化してから使っている1つ目の printf 以外は、値がどうなっているか不定です。
例えば、Visual Studio 付属の C++ コンパイラー(以下、Visual C++/VC++)で実行した場合、
Debugビルド時とReleaseビルド時で挙動が違います。

<table summary="削除済み/未初期化領域のアクセス(Visual C++ の例)">
	<caption>
		削除済み/未初期化領域のアクセス(Visual C++ の例)
	</caption>
	<tr>
		<th rowspan="2">状態</th>
		<th rowspan="2">コード例</th>
		<th colspan="2">Debugビルド時</th>
		<th colspan="2">Releaseビルド時</th>
	</tr>
	<tr>
		<th>結果</th>
		<th>説明</th>
		<th>結果</th>
		<th>説明</th>
	</tr>
	<tr>
		<th>初期化済み</th>
		<td markdown="1">
<pre class="source" title="未初期化領域を読む" lang="">
<code><span class="reserved">int</span>* x = <span class="reserved">new int</span>[1];
x[0] = 0xFFFFFFFF;
printf(<span class="literal">"%08x\n"</span>, x[0]);
</code></pre>

</td>
		<td markdown="1">ffffffff</td>
		<td markdown="1">これは問題ないコード。常に同じ動作。</td>
		<td markdown="1">ffffffff</td>
		<td markdown="1">ビルド オプションで結果が変わったりもしない。</td>
	</tr>
	<tr>
		<th>削除済み</th>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">int</span>* px = x;
<span class="reserved">delete</span> x;
printf(<span class="literal">"%08x\n"</span>, px[0]);
</code></pre>

</td>
		<td markdown="1">dddddddd</td>
		<td markdown="1">削除済み領域を検知するためのパターンが入っている。<br></br>VC++ の場合は dd (ビット パターン 11011101)。</td>
		<td markdown="1">ffffffff<sup>※</sup></td>
		<td markdown="1">delete 前の値がそのまま残っている。</td>
	</tr>
	<tr>
		<th>未初期化</th>
		<td markdown="1">
<pre class="source" title="" lang="">
<code>    <span class="reserved">int</span>* y = <span class="reserved">new int</span>[1];
    printf(<span class="literal">"%08x\n"</span>, y[0]);
}
</code></pre>

</td>
		<td markdown="1">cdcdcdcd</td>
		<td markdown="1">未初期化領域を検知するためのパターンが入っている。<br></br>VC++ の場合は cd (ビット パターン 11001101)。</td>
		<td markdown="1">00000000<sup>※</sup></td>
		<td markdown="1">この例の場合は0詰め。</td>
	</tr>
</table>


<sup>※</sup> 常にこうなるわけじゃない。状況次第。

まだこの実行結果は値がわかりやすい方ですが、
場合によってはもっとランダムに意味不明の数値が得られたりします。
しかも、実行するたびに毎回結果が変わったりします。

こういう不定な動作は、「テスト実行時にはうまく動いていた(ように見えた)のに、本番環境では動かない」というようなバグになることもあります。
これは発見しにくい類のバグで、メモリ領域の未初期化を認めている言語ではよく問題になったりします。
そのため、C# は未初期化を認めていません。


##<a id="sec-generated-title-3"></a> <a id="sec-default-value">既定値</a>
とうことで、C# では、未初期化なメモリ領域へのアクセスを認めていません。
明示的な変数の初期化を怠った場合、状況に応じて、以下のいずれかになります(コンパイル エラー、もしくは、0埋め = 本項の主題となる「既定値」で初期化される)。

* 初期化しないと<em>コンパイル エラー</em>になる
    * ローカル変数
    * 構造体のフィールド(C# 10 以前)
* <em>0 埋め</em> (これを「既定値」と呼びます。後述)
    * クラスのフィールド(C# 11 以降は構造体のフィールドも)
    * 配列の要素
    * `default(T)` という式(後述)で作った値

<strong id="default-value" class="keyword">既定値</strong>(default value)というのは、その名の通り、明示的な初期化を怠った時に既定で代入される値です。
基本的に、既定値は「0 埋め」です。
型に応じて、`0`、`false`、`null` のどれか(全部、メモリ上の値としては 0 で表現される値)です。
「[構造体](../structured/st_struct.md#struct)」の場合は、すべてのフィールドを既定値で埋めたものになります。

`null` なんかは [10億ドルの間違い (billion-dollar mistake)](https://www.infoq.com/presentations/Null-References-The-Billion-Dollar-Mistake-Tony-Hoare/)とまで言われて忌み嫌われていますが、少なくとも未定義動作よりははるかにマシです。(参考: 「[null 許容参照型](nullablereferencetype.md)」)

以下に、既定値の例を示します(この例ではクラスのフィールドを明示的に初期化せず使うことで既定値を得ています)。

<pre class="source" title="既定値の例">
<code><span class="comment">// 初期化せずにフィールドを読んでみる(既定値が入っている)</span>
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">DefaultValues</span>();

<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a</span><span class="operator">.</span><span class="field">i</span>);         <span class="comment">// 0</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a</span><span class="operator">.</span><span class="field">x</span>);         <span class="comment">// 0.0</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>((<span class="reserved">int</span>)<span class="variable">a</span><span class="operator">.</span><span class="field">c</span>);    <span class="comment">// '\0' (ヌル文字)は表示できないので数値化して表示</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a</span><span class="operator">.</span><span class="field">b</span>);         <span class="comment">// False</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a</span><span class="operator">.</span><span class="field">s</span> <span class="operator">==</span> <span class="reserved">null</span>); <span class="comment">// null は表示できないので比較で。True になる</span>

<span class="reserved">class</span> <span class="type">DefaultValues</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">i</span>;
    <span class="reserved">public</span> <span class="reserved">double</span> <span class="field">x</span>;
    <span class="reserved">public</span> <span class="reserved">char</span> <span class="field">c</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="field">b</span>;
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="field">s</span>;
}
</code></pre>

0 埋めなのは、主にパフォーマンス上の理由です。
(未定義動作よりはマシなので)何か決まった値で初期化するとするなら 0 が一番低コストです。
配列などで大きめのメモリ領域を確保した際でも、0 埋めならそこまで大きなコストをかけずに初期化できます。

<pre class="source" title="巨大配列の 0 埋めの例">
<code><span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>InteropServices;

<span class="comment">// 16 MB の巨大領域。</span>
<span class="comment">// 要素1個1個は初期化していないので、全部に既定値が入ってる。</span>
<span class="reserved">var</span> <span class="variable">points</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Vector4</span>[<span class="number">1024</span> <span class="operator">*</span> <span class="number">1024</span>];

<span class="comment">// 中身が全部 0 なことを確認してみる。</span>
<span class="comment">// (無理やり byte 配列扱いして、1 byte ずつ確認。)</span>
<span class="reserved">var</span> <span class="variable">bytes</span> <span class="operator">=</span> <span class="type">MemoryMarshal</span><span class="operator">.</span><span class="method">AsBytes</span>&lt;<span class="type struct">Vector4</span>&gt;(<span class="variable">points</span>);

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">v</span> <span class="control">in</span> <span class="variable">bytes</span>)
{
    <span class="control">if</span> (<span class="variable">v</span> <span class="operator">!=</span> <span class="number">0</span>)
        <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="string">&quot;絶対通らないはず&quot;</span>);
}

<span class="reserved">struct</span> <span class="type struct">Vector4</span>
{
    <span class="reserved">public</span> <span class="reserved">float</span> <span class="field">X</span>, <span class="field">Y</span>, <span class="field">Z</span>, <span class="field">W</span>;
}
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="word-default">余談: default という英単語</a>
ちなみに、既定値は英語だと default value なわけですが。
この「デフォルト」という言葉、IT 業界内では割かし基本単語っぽく感じるものの、
他の業界の人に通じないことがたまにあったりします。
というか、「既定で」、「標準で」みたいな意味で「デフォルト」という言葉を多用するのは IT 業界の用法みたいです。

default の元々の意味は「債務不履行」とか「怠慢」です。
2012年頃に某国の財務破たんで有名になった金融用語の「デフォルト」と同じ単語です。
単語の成り立ち的には de + fault で、「失敗(fault)に陥る(de)」とかになります。

なので、default value ＝ やるべきことやってない(初期化しないとまずいだろっていう変数を初期化してない)時に強制的に代入される値 ＝ 既定値 という感じ。

##<a id="sec-generated-title-5"></a> <a id="default-keyword">default(T)</a>
C# 1.0 の頃には、既定値を作るための構文がありませんでした。
数値の場合は 0 とか 0.0 とか、bool の場合には false とか、クラスの場合には null とかいったように、個別に既定値相当の値を与える必要がありました。

また、構造体 `T` に対して、<code>new T()</code> で既定値(0 埋め)を作るという仕様がありました。
(実際、C# 10 で引数なしコンストラクターの仕様が入るまでは、構造体の `new T()` は常に既定値でした。)

C# 2.0 で「[ジェネリック](../oop/sp2_generics.md#generics)」が導入されたことで、
どんな型でも一律既定値を作れる構文が必要になりました。
以下のような場面で困りました。

<pre class="source" title="" lang="">
<code><span class="type">T</span> X&lt;<span class="type">T</span>&gt;()
{
    <span class="reserved">return</span> <span class="input">????</span>; <span class="comment">// T の既定値を作りたいけども、null とか 0 とかは書けない</span>
}
</code></pre>

<h5 class="version version2">Ver. 2.0</h5>

そこで、ジェネリックと同時に入った仕様が、`default` キーワードを使った既定値の作成機能です。

<pre class="source" title="" lang="">
<code><span class="type">T</span> X&lt;<span class="type">T</span>&gt;()
{
    <span class="reserved">return</span> <span class="reserved">default</span>(<span class="type">T</span>); <span class="comment">// 型に応じて、null とか 0 とかになる</span>
}
</code></pre>

###<a id="sec-generated-title-6"></a> <a id="default-constructor">default(T) と構造体のコンストラクター</a>
前節で少し触れましたが、`default(T)` 構文が入るまで、
構造体の既定値は `new T()` で作っていました。
この仕様のせいで、C# では、構造体に引数なしのコンストラクターを定義できませんでした。
(ちなみに、.NET 的にはそんな制限はありません。あくまで C# の文法上の制限。)

しかし、C# 2.0 以降、`default(T)` で既定値を作れる仕様が入ったので、実は、「C# の構造体には引数なしのコンストラクターが定義できない」って仕様は今となっては不要だったりします。
つまり、以下のよう使い分けれていいはずです。

<pre class="source" title="" lang="">
<code><span class="reserved">void</span> X&lt;<span class="type">T</span>&gt;()
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>()
{
    <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">T</span>();    <span class="comment">// この場合はコンストラクターが呼ばれて欲しい</span>
    <span class="reserved">var</span> y = <span class="reserved">default</span>(<span class="type">T</span>); <span class="comment">// こいつは既定値(0 埋め)</span>
}
</code></pre>

<h5 class="version version10">Ver. 10.0</h5>

この現状を鑑みて、
C# 10 から構造体に引数なしのコンストラクターを定義できるようになりました。

<pre class="source" title="C# 10 で引数なしコンストラクターが書けるようになって、new T() と default(T) が別の意味に">
<code><span class="comment">// new T() は S(1, 2) に、</span>
<span class="comment">// default(T) は S(0, 0) になる。</span>
<span class="method">WriteNewAndDefault</span>&lt;<span class="type struct">S</span>&gt;();

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">WriteNewAndDefault</span>&lt;<span class="type param">T</span>&gt;()
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="reserved">new</span>()
{
    <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type param">T</span>();    <span class="comment">// この場合はコンストラクターが呼ばれるようになった。</span>
    <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="reserved">default</span>(<span class="type param">T</span>); <span class="comment">// こいつは既定値(0 埋め)。</span>

    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">x</span>);
    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">y</span>);
}

<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">X</span>, <span class="field">Y</span>;
    <span class="reserved">public</span> <span class="type struct">S</span>() <span class="operator">=&gt;</span> (<span class="field">X</span>, <span class="field">Y</span>) <span class="operator">=</span> (<span class="number">1</span>, <span class="number">2</span>);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">S(</span>{<span class="field">X</span>}<span class="string">, </span>{<span class="field">Y</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</code></pre>

ちなみに、引数なしコンストラクターの仕様は C# 6 で一度検討されたんですが、その時にはいくつかバグを踏んでしまって撤回されました。
この時踏んだバグは以下のようなものです。

- `new T() == default(T)` という前提での最適化をしているコードが多すぎて、`new T()`で正しくコンストラクターを呼ばれない場面があった。
  - .NETランタイムの中でそういうコードがあって、C#よりも上のレイヤーでの回避ができない。
  - `Activator`クラス(`System`名前空間)の`CreateInstance`とかがそう。

##<a id="sec-generated-title-7"></a> <a id="default-expr">default 式</a>
<h5 class="version version7_1">Ver. 7.1</h5>

これまでの`default(T)`という構文では、型名が長い時にかなり煩雑なコードになっていました。
これに対して、C# 7.1では、左辺(代入先)から推論できる場合に、`(T)`を省略して`default`だけで既定値を作れるようになりました。 

例えば、既定値をよく使う割に型名が長くてうっとおしいものの代表格に、`CancellationToken`構造体(`System.Threading`名前空間)があります。
以下のような感じのコードを書くことが結構あったりします。

<pre class="source" title="CancellationTokenの規定値をdefault(T)で作る例">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> DefaultExpression(<span class="type">CancellationToken</span> c = <span class="reserved">default</span>(<span class="type">CancellationToken</span>))
{
    <span class="reserved">while</span> (c != <span class="reserved">default</span>(<span class="type">CancellationToken</span>) &amp;&amp; !c.IsCancellationRequested)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        <span class="type">Console</span>.WriteLine(<span class="string">"."</span>);
    }
}
</code></pre>

これに対して、C# 7.1では、以下のように書き直せます。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> DefaultExpression(<span class="type">CancellationToken</span> c = <em><span class="reserved">default</span></em>)
{
    <span class="reserved">while</span> (c != <em><span class="reserved">default</span></em> &amp;&amp; !c.IsCancellationRequested)
    {
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        <span class="type">Console</span>.WriteLine(<span class="string">"."</span>);
    }
}
</code></pre>

1行目の引数の既定値と、3行目の `!=`演算子の右側に`default`とだけ書かれています。
いずれも、引数`c`の型から`CancellationToken`構造体であることが推論できるので、`(CancellationToken)`の部分を省略できます。

この書き方をdefault式(default expression)、あるいは、defaultリテラル(default literal)と呼びます。

##<a id="sec-generated-title-8"></a> <a id="constant">既定値は定数</a>
既定値 `default(T)` は常に定数扱いされます。

C# には定数(`readonly` の意味じゃなく、`const`)しか受け付けない文脈がいくつかあります。
要は、コンパイル時に確定してないといけない部分なんですが、例えば以下のようなものがあります。

* 属性に渡す値

* 引数の既定値

定数を求められるので、
`int` とか `string` なら任意のリテラル(1, 2, 3, ... "abc" 何でも)を渡せますが、
クラスと構造体は既定値(`null`、`default(T)`)しか渡せません。

##<a id="sec-generated-title-9"></a> <a id="auto-default">構造体のフィールドの既定値初期化</a>
<h5 class="version version11">Ver. 11.0</h5>

C# 11 では、構造体でもフィールドの明示的な初期化が不要になりました。
クラスと同じく、明示的に代入しなかったフィールド・自動プロパティには既定値が入ります。

<pre class="source" title="構造体のフィールドが自動的に 0 初期化されるように">
<code><span class="reserved">struct</span> <span class="type struct">Sample</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; } <span class="operator">=</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Z</span> { <span class="reserved">get</span>; }

    <span class="comment">// X には初期化子が付いてるので元々 OK。</span>
    <span class="comment">// C# 11 では Y, Z に何も入れなくても自動的に 0/null 初期化されるように。</span>
    <span class="reserved">public</span> <span class="type struct">Sample</span>() { }

    <span class="comment">// C# 11 では Y に何も入れなくても大丈夫。0 に。</span>
    <span class="reserved">public</span> <span class="type struct">Sample</span>(<span class="reserved">string</span> <span class="variable local">z</span>) <span class="operator">=&gt;</span> <span class="property">Z</span> <span class="operator">=</span> <span class="variable local">z</span>;
}
</code></pre>

今となっては「構造体の場合はフィールドの明示的な初期化が必須」という制限は、「出どころを誰も覚えていない」というレベルだったそうです。
おそらくは、「構造体のフィールドはローカル変数的に扱う」みたいな空気感だと思われます。

制限が残っていても役に立つわけでもなく、
不便なだけだったので今更ながら「クラスと同様」に変更することになりました。
