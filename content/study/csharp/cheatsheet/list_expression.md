---
title: "C# の式と文の一覧"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/list_expression/"
content_type: "Article"
published_at: "2011-05-31T00:00:00"
updated_at: "2016-10-25T00:00:00"
tags: []
umbraco_id: 1175
parent_id: 1174
sort_order: 0
aliases:
  - "/csharp/cheatsheet/list_expression/"
  - "/csharp/list_expression"
  - "/csharp/list_expression.html"
  - "/study/csharp/list_expression"
  - "/study/csharp/list_expression.html"
---

# C# の式と文の一覧

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
機能別索引＆概要。
C# で使える式と文の一覧を先に示しておきます。


##<a id="sec-generated-title-2"></a> <a id="expression"></a>式
式: int x = ... とか f(...) の ... の部分に書けるもの。
x + y みたいな演算子適用が主。
その他、メンバー アクセスとかラムダ式とかクエリ式とか。

一覧（優先度順）:

<table summary="">

	<tr>
		<th>カテゴリー</th>
		<th>式</th>
		<th>参考</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="15">基本式</td>
		<td markdown="1"><code>x.m</code></td>
		<td markdown="1">メンバー アクセス:「[クラス](../oop/oo_class.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x(…)</code></td>
		<td markdown="1">メソッド呼び出し:「[関数](../structured/st_function.md)」「[デリゲート](../functional/sp_delegate.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x[…]</code></td>
		<td markdown="1">インデックス アクセス:「[配列](../structured/st_array.md)」「[インデクサー](../oop/oo_indexer.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x?.m</code><br/><code>x?[...]</code></td>
		<td markdown="1">null 条件演算子:「[null の取り扱い](../resource/rm_nullusage.md#null-conditional)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x++</code><br></br><code>x--</code></td>
		<td markdown="1">「[インクリメント・デクリメント](../start/st_operator.md#inc)」（後置き）</td>
	</tr>
	<tr>
		<td markdown="1"><code>x!</code></td>
		<td markdown="1">「[null 免除演算子](../resource/nullablereferencetype.md#null-forgiving)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>new T(…)</code><br></br><code>new T(…){…}</code><br></br><code>new {…}</code><br></br><code>new T[…]</code></td>
		<td markdown="1">オブジェクトの生成:「[クラス](../oop/oo_class.md)」「[コンストラクター](../oop/oo_construct.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>typeof(T)</code></td>
		<td markdown="1">「[実行時型情報](../dynamic/sp_reflection.md)」型情報の取得</td>
	</tr>
	<tr>
		<td markdown="1"><code>checked(x)</code><br></br><code>unchecked(x)</code></td>
		<td markdown="1">「[オーバーフローのチェック](../start/sp_checked.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>default(T)</code></td>
		<td markdown="1">「[既定値](../resource/rm_default.md#default-keyword)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>delegate{…}</code></td>
		<td markdown="1">「[匿名関数](../functional/sp_delegate.md#anonymous)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>nameof(x)</code></td>
		<td markdown="1">「[nameof演算子](../start/st_string.md#nameof-operator)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>stackalloc T[...]</code></td>
		<td markdown="1">「[スタック上への配列の確保](../interop/sp_unsafe.md#stackalloc)」、「[安全な stackalloc](../resource/span.md#safe-stackalloc)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>p-&gt;m</code></td>
		<td markdown="1">ポインター アクセス:「[unsafe](../interop/sp_unsafe.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>sizeof(T)</code></td>
		<td markdown="1">「[unsafe](../interop/sp_unsafe.md)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="10">単項演算</td>
		<td markdown="1"><code>+x</code><br></br><code>-x</code></td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>!x</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>~x</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>^x</code></td>
		<td markdown="1">「[Index](../data/dataranges.md#index)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>++x</code><br></br><code>--x</code></td>
		<td markdown="1">「[インクリメント・デクリメント](../start/st_operator.md#inc)」（前置き）</td>
	</tr>
	<tr>
		<td markdown="1"><code>(T)x</code></td>
		<td markdown="1">型変換（キャスト）:「[明示的な型変換](../start/st_cast.md#explicit)」「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>await x</code></td>
		<td markdown="1">非同期処理の完了待ち:「[非同期処理](../async/sp5_async.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x</code></td>
		<td markdown="1">true/false 式:「[演算子のオーバーロード](../oop/oo_operator.md#true-false)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>*p</code></td>
		<td markdown="1">ポインター間接参照:「[unsafe](../interop/sp_unsafe.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&amp;p</code></td>
		<td markdown="1">ポインター化:「[unsafe](../interop/sp_unsafe.md)」</td>
	</tr>
	<tr>
		<td markdown="1">範囲</td>
		<td markdown="1"><code>x..y</code></td>
		<td markdown="1">「[Range](../data/dataranges.md#range)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">switch 式</td>
		<td markdown="1"><code> x switch { ...  }</code></td>
		<td markdown="1">「[switch 式](../datatype/typeswitch.md#switch-expression)」</td>
	</tr>
	<tr>
		<td markdown="1"><code> x with { ...  }</code></td>
		<td markdown="1">「[with 式](../datatype/record.md#with)」</td>
	</tr>
	<tr>
		<td markdown="1">乗除算</td>
		<td markdown="1"><code> x * y</code><br></br><code>x / y</code><br></br><code>x % y</code></td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1">加減算</td>
		<td markdown="1"><code>x + y</code><br></br><code>x – y</code></td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1">シフト</td>
		<td markdown="1"><code>x &lt;&lt; y</code><br></br><code>x &gt;&gt; y</code></td>
		<td markdown="1">「[シフト](../start/st_operator.md#shift)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">関係式/型検査</td>
		<td markdown="1"><code>x &lt; y</code><br></br><code>x &gt; y</code><br></br><code>x &lt;= y</code><br></br><code>x &gt;= y</code></td>
		<td markdown="1">「[関係演算](../start/st_operator.md#relation)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x is T</code></td>
		<td markdown="1">「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x as T</code></td>
		<td markdown="1">「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1">等値比較</td>
		<td markdown="1"><code>x == y</code><br></br><code>x != y</code></td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="3">論理演算<sup>※1</sup></td>
		<td markdown="1"><code>x &amp; y</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x ^ y</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x | y</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="5">条件演算<sup>※1</sup></td>
		<td markdown="1"><code>x &amp;&amp; y</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」（短絡評価）</td>
	</tr>
	<tr>
		<td markdown="1"><code>x || y</code></td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」（短絡評価）</td>
	</tr>
	<tr>
		<td markdown="1"><code>x ?? y</code></td>
		<td markdown="1">「[null 合体演算子](../start/st_operator.md#null)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>throw x</code></td>
		<td markdown="1">「[throw 式](../structured/oo_exception.md#throwexpr)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x ? y : z</code></td>
		<td markdown="1">「[条件演算子](../start/st_operator.md#condition)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="4">クエリ式、 ラムダ式、 代入<sup>※2</sup></td>
		<td markdown="1"><code>from x in …</code></td>
		<td markdown="1">「[クエリ式](../data/sp3_linq.md#query)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>() =&gt; { }</code></td>
		<td markdown="1">「[匿名関数](../functional/sp_delegate.md#anonymous)」「[ラムダ式](../functional/sp3_lambda.md)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x = y</code></td>
		<td markdown="1">「[代入演算](../start/st_operator.md#substitute)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x op= y</code></td>
		<td markdown="1">+= など:「[代入演算](../start/st_operator.md#substitute)」</td>
	</tr>
</table>


※1 論理演算と条件演算に関しては、同列の優先順位ではなく、上から順に優先度が付いています。

※2 クエリ式、ラムダ式、代入は同列で、例えば、以下のような C# コードを書けます。

<pre class="source" title="クエリ式、ラムダ式、代入を含む式" lang="">
<code><span class="reserved">int</span> sum = 0;
<span class="type">Func</span>&lt;<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;&gt; q = () =&gt;
  <span class="reserved">from</span> x <span class="reserved">in</span> <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 }
  <span class="reserved">select</span> sum += x;
</code></pre>



##### <a id="sec-generated-title-3"></a>結合規則
優先順位が同じ場合、クエリ式、ラムダ式、代入の3つは右から、その他の式は左から結合します。

左結合は、例えば、<code>a + b + c + d</code> なら <code>((a + b) + c) + d</code> と同じ意味です。
右結合の例は <code>Func&lt;int&gt; f = x =&gt; s += x;</code> なら <code>Func&lt;int&gt; f = (x =&gt; (s += x));</code> になります。


##### <a id="sec-generated-title-4"></a>評価順
演算子のオペランドは、演算子の優先順位や結合規則によらず、常に左から順に評価されます。
例えば、以下のように、画面への出力を伴うメソッド Echo を呼ぶと、2, 3, 4 の順で出力されます。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> Echo(<span class="reserved">int</span> x)
    {
        <span class="type">Console</span>.WriteLine(x);
        <span class="reserved">return</span> x;
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> s = Echo(2) + Echo(3) * Echo(4);
        <span class="comment">// 演算子の優先順位に関係なく、Echo(2) → Echo(3) → Echo(4) の順に呼ばれる</span>
    }
}
</code></pre>


<pre class="console" title="">
2
3
4
</pre>



##<a id="sec-generated-title-5"></a> <a id="statement"></a>文
一覧:

<table summary="">

	<tr>
		<th>カテゴリー</th>
		<th>文</th>
		<th>例</th>
		<th>参考</th>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">宣言</td>
		<td markdown="1">ローカル変数</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">int</span> x;
<span class="reserved">string</span> s = <span class="literal">"sample"</span>;
<span class="reserved">var</span> a = 10;
</code></pre>

</td>
		<td markdown="1">「[変数と式](../start/st_variable.md)」</td>
	</tr>
	<tr>
		<td markdown="1">ローカル定数</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">const</span> <span class="reserved">int</span> x = 100;
<span class="reserved">const</span> <span class="reserved">double</span> e = 2.71828;
</code></pre>

</td>
		<td markdown="1">「[定数](../start/sp_const.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">式</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code>x = 1 + 2;
<span class="type">Math</span>.Sin(1);
</code></pre>

</td>
		<td markdown="1">式単体。 「式;」。「[変数と式](../start/st_variable.md)」「[式](#expression)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">オーバーフローのチェック</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">checked</span>
{
    <span class="reserved">int</span> z = x * y;
}
<span class="reserved">unchecked</span>
{
    <span class="reserved">int</span> z = x * y;
}
</code></pre>

</td>
		<td markdown="1">「[オーバーフローのチェック](../start/sp_checked.md)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">条件分岐</td>
		<td markdown="1">if 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">if</span> (条件) { } <span class="reserved">else</span> { }
</code></pre>

</td>
		<td markdown="1">「[制御フロー](../structured/st_control.md)」「[if 文](../structured/st_branch.md#if)」</td>
	</tr>
	<tr>
		<td markdown="1">switch 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">switch</span>(値)
{
    <span class="reserved">case</span> 0:
        <span class="reserved">break</span>;
    <span class="reserved">default</span>:
        <span class="reserved">break</span>;
}
</code></pre>

</td>
		<td markdown="1">「[switch 文](../structured/st_branch.md#switch)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="4">反復処理</td>
		<td markdown="1">while 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">int</span> n = 10;
<span class="reserved">while</span> (n &gt; 0)
{
    --n;
}
</code></pre>

</td>
		<td markdown="1">「[制御フロー](../structured/st_control.md)」「[while 文](../structured/st_loop.md#while)」</td>
	</tr>
	<tr>
		<td markdown="1">do 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">string</span> s;
<span class="reserved">do</span>
{
    s = <span class="type">Console</span>.ReadLine();
} <span class="reserved">while</span> (s.Length == 0);
</code></pre>

</td>
		<td markdown="1">「[制御フロー](../structured/st_control.md)」「[do-while 文](../structured/st_loop.md#dowhile)」</td>
	</tr>
	<tr>
		<td markdown="1">for 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 10; i++)
{

}
</code></pre>

</td>
		<td markdown="1">「[制御フロー](../structured/st_control.md)」「[for 文](../structured/st_loop.md#for)」</td>
	</tr>
	<tr>
		<td markdown="1">foreach 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">var</span> array = <span class="reserved">new</span>[] { 1, 2, 3 };
<span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
{

}
</code></pre>

</td>
		<td markdown="1">「[制御フロー](../structured/st_control.md)」「[foreach文](../structured/st_loop.md#foreach)」「[foreach](../data/sp_foreach.md)」</td>
	</tr>
	<tr>
		<td markdown="1" rowspan="2">反復の中断</td>
		<td markdown="1">break 文</td>
		<td markdown="1" rowspan="2">
<pre class="source" title="" lang="">
<code><span class="reserved">while</span> (<span class="reserved">true</span>)
{
    <span class="reserved">if</span> (nothingToDo)
        <span class="reserved">continue</span>;

    <span class="reserved">if</span> (!isActive)
        <span class="reserved">break</span>;
}
</code></pre>

</td>
		<td markdown="1">「[反復処理](../structured/st_loop.md)」</td>
	</tr>
	<tr>
		<td markdown="1">continue 文</td>
		<td markdown="1">「[反復処理](../structured/st_loop.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">goto 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">while</span> (<span class="reserved">true</span>)
{
    <span class="reserved">while</span> (<span class="reserved">true</span>)
    {
        <span class="reserved">goto</span> LOOP_END;
    }
}
LOOP_END: ;
</code></pre>

</td>
		<td markdown="1">「[goto 文](../structured/st_branch.md#goto)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">return 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">int</span> Add(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
{
    <span class="reserved">return</span> x + y;
}
</code></pre>

</td>
		<td markdown="1">「[関数](../structured/st_function.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">yield 文</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; ZeroTo(<span class="reserved">int</span> x)
{
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt;= x; i++)
    {
        <span class="reserved">yield</span> <span class="reserved">return</span> i;
    }
}
</code></pre>

</td>
		<td markdown="1">「[イテレーター](../data/sp2_iterator.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">例外処理</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">try</span>
{
}
<span class="reserved">catch</span> (<span class="type">IOException</span>)
{
}
<span class="reserved">finally</span>
{
}
</code></pre>

</td>
		<td markdown="1">「[例外処理](../structured/oo_exception.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">リソース破棄</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> r = <span class="type">File</span>.OpenText(<span class="literal">"a.txt"</span>))
{
    <span class="reserved">var</span> s = r.ReadLine();
}
</code></pre>

</td>
		<td markdown="1">「[リソースの破棄](../resource/oo_dispose.md)」</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">ロック</td>
		<td markdown="1">
<pre class="source" title="" lang="">
<code><span class="reserved">object</span> syncObj = <span class="reserved">new</span> <span class="reserved">object</span>();
<span class="type">Parallel</span>.ForEach(data, x =&gt;
{
    <span class="reserved">lock</span> (syncObj)
    {
        sum += x;
    }
});
</code></pre>

</td>
		<td markdown="1">「[lock 文](../async/sp_thread.md#lock)」</td>
	</tr>
</table>
