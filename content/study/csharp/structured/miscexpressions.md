---
title: "[雑記] 式にまつわる補足"
source_url: "https://ufcpp.net/study/csharp/structured/miscexpressions/"
content_type: "Article"
published_at: "2016-09-26T00:00:00"
updated_at: "2019-02-17T16:19:06"
tags: []
umbraco_id: 1962
parent_id: 1217
sort_order: 20
aliases:
  - "/csharp/structured/miscexpressions/"
---

# \[雑記\] 式にまつわる補足

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
「[変数と式](../start/st_variable.md)」で少し言葉としては出しましたが、
プログラミング言語の構文には大きく分けて式(expression)とステートメント(statement: 文、平叙文)という2種類のものがあります。

最近のプログラミング言語ほど式の比率が高くなっています。
C#でも、バージョンを重ねるごとに、式になっている構文が増えています。

##<a id="sec-generated-title-2"></a> <a id="term"></a>式とステートメント
「[基礎](../index.md#start)」と「[構造化](../index.md#structured)」のセクションで、C#の式とステートメントの結構な割合を紹介しました。ここで1度、この式とステートメントの区別についての話をしておきます。

式とステートメントは、大まかに言うと以下のようなものです。

- <strong id="key-expression" class="keyword">式</strong>(expression): 割とどこにでも書ける代わりに戻り値が必須
- <strong id="key-statement" class="keyword">ステートメント</strong>(statement): 戻り値がなくてもいい代わりに書ける場所がブロック内に限られている

#####<a id="sec-generated-title-3"></a> <a id="expression"></a>式
式(expression: 表現、語句、(数学用語で)式)は、
以下のような特徴がある構文です。

- 書ける場所が多い
- 必ず何かしらの値を返す
- いろいろ組み合わせて書ける

![式](../../../../assets/media/1096/expressions.png)

名前からして数学用語の「式」がもとになっていますが、
名前通り、`x + 1` というような数式みたいなものが多いです。
いかにも数式っぽいもの以外にも、[`x.PropertyName`](../oop/oo_class.md#use)や[`await x`](../async/sp5_async.md) なども式です。

組み合わせて書けるというのは、例えば、
`await Task.Run(() => new[] { "abc" }[0].Length)` みたいなものも式になっています。
この式は、以下のような式を組み合わせた結果です。

- `"abc"` … [文字列リテラル](../start/st_variable.md#literal)
- "new [] { x }` [配列生成](st_array.md)
- `x[0]` … [インデックス アクセス](../oop/oo_indexer.md)
- `x.Length` … [メンバー アクセス](../oop/oo_class.md#use)
- `() => x` … [ラムダ式](../functional/sp3_lambda.md)
- `Task.Run` … [メンバー アクセス](../oop/oo_class.md#use)
- `x(y)` … [メソッド呼び出し](st_function.md)
- `await x` … [非同期処理の完了待ち](../async/sp5_async.md)

組み合わせて書ける分、1つ1つはシンプルなものが多いです。

#####<a id="sec-generated-title-4"></a> <a id="statement"></a>ステートメント
一方、ステートメント(statement: 文、声明)は、
以下のような特徴がある構文です。

- ブロックの内側にしか書けない
- 組み合わせが効かない
- 1つ1つが大きい
- 戻り値がない

![ステートメント](../../../../assets/media/1097/statements.png)

`if` などの条件分岐や、`for` などの反復処理をはじめ、[制御構文](st_control.md)が多いです。

式との比較をまとめます。

|式|ステートメント(文)|
|---|---|
|数式っぽい構文が多い|制御構文が多い|
|どこにでも書ける|ブロック内(関数本体の中など)にしか書けない|
|いろいろ組み合わせて書ける|そんなに組み合わせの幅はない|
|戻り値が必須|戻り値がない|

式やステートメント(文)の一覧は、「[C# の式と文の一覧](../cheatsheet/list_expression.md)」にまとめてあります。

##<a id="sec-generated-title-5"></a> <a id="increasing-expressions"></a>式は増加傾向にある
近年では、ステートメントよりも式の方が好まれる傾向があります。
C# でも、バージョンアップを重ねるたびに、式の比率が増えています。

ステートメントだとそもそも書けない場所も多くて困ることがあります。
なので、以前でもステートメントを使って書けたものを、
式で書き直せるような構文の追加が結構あります。
また、式を使いやすくするような構文も増えています。

以下に例を挙げていきます。

##### <a id="sec-generated-title-6"></a>C# 2.0
<table>
<tr>
<th>構文</th>
<th>新しい書き方</th>
<th>以前の書き方</th>
</tr>
<tr>
<td><a href="../resource/rm_nullusage.md#key-null-coalesce">null 合体演算子</a></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">y</span> = <span class="variable">x</span> ?? <span class="string">&quot;&quot;</span>;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">y</span> = <span class="variable">x</span>;
<span class="control">if</span> (<span class="variable">y</span> == <span class="reserved">null</span>) <span class="variable">y</span> = <span class="string">&quot;&quot;</span>;
</code></pre></td>
</tr>
<tr>
<td><a href="../functional/sp_delegate.md#anonymous">匿名メソッド式</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
{
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> = <span class="reserved">delegate</span> (<span class="reserved">int</span> <span class="variable">x</span>)
    {
        <span class="control">return</span> <span class="variable">x</span> * <span class="variable">x</span>;
    };
}
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
{
    <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> = <span class="method">M</span>;
}
 
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>)
{
    <span class="control">return</span> <span class="variable">x</span> * <span class="variable">x</span>;
}
</code></pre></td>
</tr>
</table>

##### <a id="sec-generated-title-7"></a>C# 3.0
<table>
<tr>
<th>構文</th>
<th>新しい書き方</th>
<th>以前の書き方</th>
</tr>
<tr>
<td><a href="../cheatsheet/ap_ver3.md#functional">オブジェクト初期化子</a></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span>();
<span class="variable">p</span>.X = 1;
<span class="variable">p</span>.Y = 2;
</code></pre></td>
</tr>
<tr>
<td><a href="../cheatsheet/ap_ver3.md#functional">コレクション初期化子</a></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">list</span> = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { 1, 2 };
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">list</span> = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;();
<span class="variable">list</span>.<span class="method">Add</span>(1);
<span class="variable">list</span>.<span class="method">Add</span>(2);
</code></pre></td>
</tr>
<tr>
<td><a href="../functional/sp3_lambda.md">ラムダ式</a></td>
<td><pre class="source" title="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> = <span class="variable">x</span> =&gt; <span class="variable">x</span> * <span class="variable">x</span>;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt; <span class="variable">f</span> = <span class="reserved">delegate</span> (<span class="reserved">int</span> <span class="variable">x</span>) { <span class="control">return</span> <span class="variable">x</span>; };
</code></pre></td>
</tr>
<tr>
</table>


##### <a id="sec-generated-title-8"></a>C# 6.0
<table>
<tr>
<th>構文</th>
<th>新しい書き方</th>
<th>以前の書き方</th>
</tr>
<tr>
<td><a href="../resource/rm_nullusage.md#key-null-conditional">null条件演算子</a></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">y</span> = <span class="variable">x</span>?.Length;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">int</span>? <span class="variable">y</span>;
<span class="control">if</span> (<span class="variable">x</span> == <span class="reserved">null</span>) <span class="variable">y</span> = <span class="reserved">null</span>;
<span class="control">else</span> <span class="variable">y</span> = <span class="variable">x</span>.Length;
</code></pre></td>
</tr>
<tr>
<td><a href="../cheatsheet/ap_ver6.md#index-initializer">インデックス初期化子</a></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">dic</span> = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;
{
    [<span class="string">&quot;one&quot;</span>] = 1,
    [<span class="string">&quot;two&quot;</span>] = 2
};
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">var</span> <span class="variable">dic</span> = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;();
<span class="variable">dic</span>[<span class="string">&quot;one&quot;</span>] = 1;
<span class="variable">dic</span>[<span class="string">&quot;two&quot;</span>] = 2;
</code></pre></td>
</tr>
<tr>
<td><a href="../cheatsheet/ap_ver6.md#sec-expression-bodied">式形式メンバー</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> * <span class="variable">x</span>;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>)
{
    <span class="control">return</span> <span class="variable">x</span> * <span class="variable">x</span>;
}
</code></pre></td>
</tr>
</table>

##### <a id="sec-generated-title-9"></a>C# 7.0
<table>
<tr>
<th>構文</th>
<th>新しい書き方</th>
<th>以前の書き方</th>
</tr>
<tr>
<td><a href="st_function.md#sec-expression-bodied">式形式メンバー(追加)</a></td>
<td><pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X, Y;
    <span class="reserved">public</span> <span class="method">Point</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>)
        =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
}
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X, Y;
    <span class="reserved">public</span> <span class="method">Point</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>)
    {
        X = <span class="variable">x</span>;
        Y = <span class="variable">y</span>;
    }
}
</code></pre></td>
</tr>
<tr>
<td><a href="oo_exception.md#throwexpr">throw 式</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">string</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>)
        =&gt; <span class="variable">s</span> ?? <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">string</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> == <span class="reserved">null</span>) <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
    <span class="control">return</span> <span class="variable">s</span>;
}
</code></pre></td>
</tr>
<tr>
<td><a href="../resource/sp_ref.md#out-var">出力変数宣言</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>)
         =&gt; <span class="reserved">int</span>.<span class="method">TryParse</span>(<span class="variable">s</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">x</span>) ? <span class="variable">x</span> : -1;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span>;
    <span class="control">if</span> (<span class="reserved">int</span>.<span class="method">TryParse</span>(<span class="variable">s</span>, <span class="reserved">out</span> <span class="variable">x</span>)) <span class="control">return</span> <span class="variable">x</span>;
    <span class="control">else</span> <span class="control">return</span> -1;
}
</code></pre></td>
</tr>
<tr>
<td><a href="../datatype/patterns.md#declaration">宣言パターン</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
        =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">string</span> <span class="variable">s</span> ? <span class="variable">s</span>.Length : 0;
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="reserved">var</span> <span class="variable">s</span> = <span class="variable">x</span> <span class="reserved">as</span> <span class="reserved">string</span>;
    <span class="control">if</span> (<span class="variable">s</span> != <span class="reserved">null</span>) <span class="control">return</span> <span class="variable">s</span>.Length;
    <span class="control">return</span> 0;
}
</code></pre></td>
</tr>
</table>

##### <a id="sec-generated-title-10"></a>C# 8.0
<table>
<tr>
<th>構文</th>
<th>新しい書き方</th>
<th>以前の書き方</th>
</tr>
<tr>
<td><a href="#abstract">switch 式</a></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span>? <span class="variable">x</span>, <span class="reserved">int</span>? <span class="variable">y</span>)
    =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) <span class="reserved">switch</span>
    {
        (<span class="reserved">null</span>, <span class="reserved">null</span>) =&gt; 0,
        (<span class="reserved">null</span>, { }) =&gt; 1,
        ({ }, <span class="reserved">null</span>) =&gt; -1,
        (<span class="reserved">int</span> <span class="variable">i</span>, <span class="reserved">int</span> <span class="variable">j</span>) =&gt; <span class="variable">i</span>.<span class="method">CompareTo</span>(<span class="variable">j</span>),
    };
</code></pre></td>
<td><pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span>? <span class="variable">x</span>, <span class="reserved">int</span>? <span class="variable">y</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">int</span> <span class="variable">i</span>)
    {
        <span class="control">if</span> (<span class="variable">y</span> <span class="reserved">is</span> <span class="reserved">int</span> <span class="variable">j</span>) <span class="control">return</span> <span class="variable">i</span>.<span class="method">CompareTo</span>(<span class="variable">j</span>);
        <span class="control">else</span> <span class="control">return</span> -1;
    }
    <span class="control">else</span>
    {
        <span class="control">if</span> (<span class="variable">y</span> <span class="reserved">is</span> <span class="reserved">int</span>) <span class="control">return</span> 1;
        <span class="control">else</span> <span class="control">return</span> 0;
    }
}
</code></pre></td>
</tr>
</table>
