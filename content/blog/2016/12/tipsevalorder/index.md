---
title: "小ネタ 式の評価順序"
source_url: "https://ufcpp.net/blog/2016/12/tipsevalorder/"
content_type: "BlogEntry"
published_at: "2016-12-04T01:10:43"
updated_at: "2016-12-04T01:10:43"
tags: []
umbraco_id: 1981
parent_id: 1969
sort_order: 3
aliases: []
---

# 小ネタ 式の評価順序

C#小ネタと言いつつ、IL小ネタになりがちだったので、今日はC#小ネタらしく。

最初にちょっとしたクイズ。
まず、中身は何でもいいんですが適当な2引数のメソッドを用意します。
例として、単純な足し算でも用意しておきましょう。

<pre class="source" title="2に引数のメソッドをまず用意">
<code><span class="reserved">static</span> <span class="reserved">int</span> F(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; x + y;
</code></pre>

以下の2つのコードの挙動は同じでしょうか？違うでしょうか？

1つ目: 一時変数を使用

<pre class="source" title="一時変数を使用して呼び出し">
<code><span class="reserved">var</span> temp = F(2, 3);
<span class="reserved">var</span> result = F(1, temp);
</code></pre>

2つ目: 1つの式で計算

<pre class="source" title="1つの式で呼び出し">
<code><span class="reserved">var</span> result = F(1, F(2, 3));
</code></pre>

まあ、同じですね。<em>副作用を残さない限りは。</em>

## オペランドの評価順序

ということで、今日はオペランドの評価順の話です。
上記の2つのコードを、わざと副作用付きに書き換えてみます。

そのためにとりあえず、副作用を起こすメソッドを追加。
`Console`にログ出力した後、引数を素通しするだけのメソッドです。
値渡し版と参照渡し版を用意。

<pre class="source" title="副作用を起こすメソッド">
<code><span class="comment">// WriteLine + 素通し</span>
<span class="reserved">static</span> <span class="type">T</span> Log&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) { <span class="type">Console</span>.WriteLine(x); <span class="reserved">return</span> x; }
<span class="reserved">static</span> <span class="reserved">ref</span> <span class="type">T</span> Log&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> x) { <span class="type">Console</span>.WriteLine(x); <span class="reserved">return</span> <span class="reserved">ref</span> x; }
</code></pre>

1つ目(一時変数を使用)を改めて:

<pre class="source" title="一時変数を使用 + 副作用あり">
<code><span class="reserved">var</span> temp = F(Log(2), Log(3));
<span class="reserved">var</span> result = F(Log(1), temp);
</code></pre>

<pre class="console" title="一時変数を使用 + 副作用あり">
<code>2
3
1
</code></pre>

2つ目(1つの式で計算)を改めて:

<pre class="source" title="1つの式で計算">
<code><span class="reserved">var</span> result = F(Log(1), F(Log(2), Log(3)));
</code></pre>

<pre class="console" title="1つの式で計算">
<code>1
2
3
</code></pre>

C#では、式の評価は、上から下へ、左から右へ逐次実行です。
なので、一時変数を導入すると結果が変わります。

1つの式の中でも、演算子の優先順位や結合方向とは無関係に、評価は一律左から右です。
代入(優先度が低い上に右から左に結合)が混ざっていようと、そのオペランドの評価は左から右です。

例えば以下の通り。

<pre class="source" title="演算子の優先順位や結合方向とは無関係に、オペランドは左から右に評価">
<code><span class="reserved">bool</span> x = <span class="reserved">false</span>, y = <span class="reserved">true</span>;
Log(<span class="reserved">ref</span> x) = Log(<span class="reserved">ref</span> y) = Log(1) + Log(2) * Log(3) &gt; Log(4) &amp; Log(5) &lt;= Log(6) - Log(7) | Log(8) == Log(9);
</code></pre>

<pre class="console" title="演算子の優先順位や結合方向とは無関係に、オペランドは左から右に評価">
<code>False
True
1
2
3
4
5
6
7
8
9
</code></pre>

## 名前付き引数のオペランド評価

前節の結果はそんなに不思議なことないでしょう。コードから挙動を予想しやすいって意味では書かれてる順番通りが一番です。それに、パフォーマンス的にも悪い選択ではありません。例えば、

<pre class="source" title="コンパイル元1">
<code><span class="reserved">var</span> result = F(1, F(2, 3));
</code></pre>

というようなコードであれば、コンパイル結果は以下のような感じになります(必要なところを抜粋)。

<pre class="source" title="コンパイル結果1">
<code>ldc.i4.1
ldc.i4.2
ldc.i4.3
call       F
call       F
</code></pre>

元のC#のオペランドと同じ、1, 2, 3の順で`ldc` (load constant)しています。
副作用を起こすために`Log`メソッドを挟む場合、この`ldc`のところが数命令に置き換わりますが、命令の並ぶ順序はこの場合と同じです。

ということで、素直に実装すればいいだけ…

でもなくて、まあ、ほとんどの式は素直に実装していいんですが、一部めんどくさい奴がいます。例えば、名前付き引数。
以下のようなコードを書いたとします。
`x`, `y`を逆に並べています。

<pre class="source" title="コンパイル元2">
<code>F(1, F(y: 2, x: 3));
</code></pre>

すると、コンパイル結果は以下の通り。C#ソースコード上は1, 2, 3だったものが、IL的には1, 3, 2になります。

<pre class="source" title="コンパイル結果2">
<code>ldc.i4.1
ldc.i4.<em>3</em>
ldc.i4.<em>2</em>
call       F
call       F
</code></pre>

この結果は副作用がないからこそ、C#コンパイラーの最適化が掛かってこうなっています。
副作用がないことがわかっている場合、評価順を並べ替えを行います。

一方で、副作用があるとそうはいきません。あくまで、C#では左から右への評価が必要です。

例えば以下のようなコードをでは、ちゃんと、1, 2, 3の順での評価が必要です。

<pre class="source" title="コンパイル元3">
<code>F(Log(1), F(y: Log(2), x: Log(3)));
</code></pre>

コンパイル結果は以下の通りです。これまでは必要のなかった一時変数(`stloc`: ローカル変数へのストア)が必要になります。

<pre class="source" title="コンパイル結果3">
<code>ldc.i4.1
call       Log
ldc.i4.2
call       Log
stloc.0    <span class="comment">// 一時変数！
</span>ldc.i4.3
call       Log
ldloc.0
call       F
call       F
</code></pre>

ちなみに、これ、C# 4.0の時にはバグってて評価順が狂ってた(逆順になってた)そうです。
C# 5.0でバグ修正した結果、[破壊的変更](https://msdn.microsoft.com/en-us/library/hh678682(v=vs.110).aspx)になっていたり(めったにこんなコード書かない上に、バグの修正なので特に問題にはならず)。

## タプルの要素の評価順序

もう1個変な例を挙げておきましょう。C# 7で導入されるタプルと分解で、以下のように、swapコードを書けるようになりました。

<pre class="source" title="タプルを使ったswap処理">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
(x, y) = (y, x);
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x}<span class="string">, </span>{y}<span class="string">"</span>);
</code></pre>

これ、同じ処理をタプルを使わず書くとすると、まあ、以下のようにしますよね。

<pre class="source" title="タプルを使わないswap処理">
<code><span class="reserved">var</span> temp = x;
x = y;
y = temp;
</code></pre>

こいつらにも副作用を加えてみましょう。

まず、タプルを使うもの。

<pre class="source" title="タプルを使ったswap処理 + 副作用">
<code><span class="reserved">var</span> x = 1;
<span class="reserved">var</span> y = 2;
(Log(<span class="reserved">ref</span> x), Log(<span class="reserved">ref</span> y)) = (Log(y), Log(x));
</code></pre>

ちゃんと、これも左から右に順に評価されます。すなわち、`ref x`, `ref y`, `y`, `x`の順。なので結果は以下の通り。

<pre class="console" title="タプルを使ったswap処理 + 副作用">
<code>1
2
2
1
</code></pre>

で、これをタプルなしで副作用も込みで全く同じ挙動にするためにはどうするか。

先ほどの類推で以下のように書いてしまうと、副作用の順序が変わります。

<pre class="source" title="タプルを使わないswap処理 + 副作用 1">
<code><span class="reserved">var</span> temp = Log(x);
Log(<span class="reserved">ref</span> x) = Log(y);
Log(<span class="reserved">ref</span> y) = temp;
</code></pre>

<pre class="console" title="タプルを使わないswap処理 + 副作用 1">
<code>1
1
2
2
</code></pre>

正しくは、以下のように書かないと同じにはなりません。

<pre class="source" title="タプルを使わないswap処理 + 副作用 2">
<code><span class="reserved">ref</span> var rx = <span class="reserved">ref</span> Log(<span class="reserved">ref</span> x);
<span class="reserved">ref</span> var ry = <span class="reserved">ref</span> Log(<span class="reserved">ref</span> y);
<span class="reserved">var</span> vy = Log(y);
<span class="reserved">var</span> vx = Log(x);
rx = vy;
ry = vx;
</code></pre>

## まとめ

副作用があっても常に一定の結果になるように、C#では、オペランドの評価順が常に左から右、書かれている通りの順序で行われます(まあ、割かし最近のプログラミング言語では大体同じで順序保証があります)。

ただ、順序保証がない場合に比べて、保証のためのコストがちょっとだけかかります(なので、古いプログラミング言語では「コンパイラーの実装ごとに変えていい」となっているものも結構あります)。

まあ、これだけ書いておいて身もふたもない結論で締めますが、副作用起こすような式を書くやつが悪い。
