---
title: "C# の null 判定の話"
source_url: "https://ufcpp.net/blog/2020/12/isnull/"
content_type: "BlogEntry"
published_at: "2020-12-13T20:20:17"
updated_at: "2020-12-13T20:20:17"
tags: []
umbraco_id: 2319
parent_id: 2318
sort_order: 0
aliases: []
---

# C# の null 判定の話

null、一般名詞としては「無効なもの」とか「0個」とかの意味の単語です。
zero も語源をたどるとアラビア語とかサンスクリット語の「空っぽ (nothing)」にあたる単語から来ていて、実のところ一般名詞としては出自が違うだけで null = zero だったり。

一方、C# (とそれに類するプログラミング言語)では、 `null` というキーワードを「無効なものを 0 を使って表す」という意味で使っていて、
一般名詞としての null が持つ2つの意味を同時に指していたりします。

とはいえ、別に null という英単語の意味を考慮して「無効なものを 0 を使って表す」にしたわけではなくて、
単に実装上「0 かどうかの判定は非常に高速なのでパフォーマンス的に都合がいい」という現実的な理由で 0 を使っています。

前置きが長くなりましたが、C# において null 判定をするというのは、内部的には単に 0 比較で、
大体の CPU 上で最速の部類に入る命令を使って実装できます。

## x == null

null 判定というとまずどういうコードを思い浮かべるでしょうか？
「昔から書けた」という意味で、まず `x == null` が真っ先に思い浮かぶ人が多いと思います。

<pre class="source" title="== null">
<code><span class="reserved">bool</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> == <span class="reserved">null</span>;
<span class="reserved">class</span> <span class="type">A</span> { }
</code></pre>

これも、この状態であれば単なる 0 比較になります。
実際、コンパイル結果を覗いてみればわかるんですが、以下のコードと同じコードが生成されます。

<pre class="source" title="== 0">
<code><span class="reserved">bool</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> == 0;
</code></pre>

ただ、ここで問題になるのが[演算子オーバーロード](../../../../study/csharp/oop/oo_operator.md)でして、これをやっちゃってると「単なる 0 比較」ではなくなってしまいます。
特に以下のように、`==` の中でそこそこ重たい処理をやっちゃっているときが問題になります。

<pre class="source" title="== の中でそこそこ重たい処理をやっちゃってる場合">
<code><span class="reserved">bool</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="method">==</span> <span class="reserved">null</span>;
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> ==(<span class="type">A</span> <span class="variable">x</span>, <span class="type">A</span> <span class="variable">y</span>) =&gt; そこそこ重たい処理;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> !=(<span class="type">A</span> <span class="variable">x</span>, <span class="type">A</span> <span class="variable">y</span>) =&gt; そこそこ重たい処理;
}
</code></pre>

`==` を使っている側、この例でいうと メソッド `M` の中身は最初にあげた「速い」コードと同じ見た目なのが罠で、「本当は 0 比較でいいはずなのにわざわざ重たい `operator ==` が呼ばれてしまう」という状況が往々にして発生します。

過激派な意見としては「`==` をオーバーロード(ユーザー裁量で中身を変更)可能にしてしまったことがよくなかった」という話もあるんですが、まあ、できるものは仕方がないとして。
本来の「無効かどうかの判定は単なる 0 比較で済む」という状態にしたければ、`==` を避けた方がいいということが多々あります。

## ReferenceEquals(x, null)

この罠にはまっちゃってるコードは案外世の中にあふれているというか、
.NET の標準ライブラリでも結構あったみたいです。

この問題は昔の C# でも簡単に解消する方法が1つあって、それが、`ReferenceEquals` を使うという案。

<pre class="source" title="ReferenceEquals(null)">
<code><span class="reserved">bool</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="method">ReferenceEquals</span>(<span class="variable">x</span>, <span class="reserved">null</span>);
</code></pre>

これで、ユーザー定義の `==` オーバーロードは呼ばれることなく、常に 0 比較で null 判定が走ります。

めでたしめでたし。

となるわけはなく、見栄えが悪すぎる…

ということで、「`ReferenceEquals` に書き換えて問題ないし、書き換えたら露骨にパフォーマンスがよくなるんだけど、この見栄えの悪さを許容するべき？」みたいな議題になっていました。

## x is null

そこに来て、C# 7.0 で[パターン マッチング](../../../../study/csharp/datatype/patterns.md)という文法が入りました。
この頃には「`== null` の罠」が周知の事実だったので、「`is null` と書いたときにはユーザー定義の `==` を呼ばない。常に 0 比較にする」という判断が下りました。

<pre class="source" title="is null">
<code><span class="reserved">bool</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>; <span class="comment">// operator == は呼ばない。常に ReferenceEquals(x, null) と同じ。</span>
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> ==(<span class="type">A</span> <span class="variable">x</span>, <span class="type">A</span> <span class="variable">y</span>) =&gt; そこそこ重たい処理;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> !=(<span class="type">A</span> <span class="variable">x</span>, <span class="type">A</span> <span class="variable">y</span>) =&gt; そこそこ重たい処理;
}
</code></pre>

これに「見栄え的に `ReferenceEquals` は NG」派が飛びつきました。
`== null` から `is null` への書き換えで救われたコードが結構あったみたいです。
(実際、僕が保守しているコードでもいくつかこの書き換えでパフォーマンス改善しています。)

めでたしめでたし？

## 非 null

めでたくなかった。

実際に多いのは以下のようなコードだったりします。

<pre class="source" title="null じゃないときだけ処理">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">a</span>.X; // プロパティ参照コストを避けるために変数に受ける。
 
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>) <span class="control">return</span>;
 
    <span class="comment">// x を使って何か処理をする。</span>
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// virtual がついていたり、いくつかの場面では X プロパティの参照に多少コストがかかる。</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">object</span>? X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

これはいわゆる early return (先頭で検査して不適切なら即 return)な書き方ですが、
判定を逆転させて同じ結果になるコードを以下のように書きたいこともあります。

<pre class="source" title="!(is null)">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">a</span>.X;
 
    <span class="control">if</span> (!(<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>))
    {
        <span class="comment">// x を使って何か処理をする。</span>
    }
}
</code></pre>

何にしてもポイントが2つあって、

- 1度変数 `x` で受けたい
- 「null である」判定じゃなく、「null じゃない」判定をしたい

という要件があって、`x is null` の導入だけだとまだちょっと面倒が残っている感じになっています。

## x is object (非 null)

「null じゃない」判定に使える書き方はいくつかあるんですが、前半で話した `x == null` の話と同様、`x != null` はユーザー定義の演算子オーバーロードを呼ばれて遅くなることがあります。
そこで `x is null` と同様、比較的新しめの文法であるパターン マッチングを使った「null じゃない」判定が欲しくなります。

C# の場合、「null は型を持っていない」という扱いになるので、すべての型の共通基底クラスである `object` 型にすらマッチしません。
なので、以下のように、`is object` というパターンを書くと「null じゃない」という判定になります。

<pre class="source" title="is object">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X <span class="reserved">is</span> <span class="reserved">object</span> <span class="variable">x</span>)
    {
        <span class="comment">// ここに来るのは a.X が null じゃなかった時だけ。</span>
        <span class="comment">// x を使って何か処理をする。</span>
    }
}
</code></pre>

### 注意: x is var (null 判定しない)

ここで注意すべきことが1点。結構な罠なんですが、上記のように `is object` が「null じゃない」判定になるのに対して、`is var` だと null / 非 null に関わらず常にマッチします(`is var` 単体だと常に true)。

<pre class="source" title="is var の場合は null 判定しないので注意">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X <span class="reserved">is</span> <span class="reserved">var</span> x)
    {
        <span class="comment">// ここは常に通る。</span>
        <span class="comment">// if なしで var x = a.X; と書くのとほぼ同じ意味なので非推奨。</span>
    }
}
</code></pre>

`var` パターンは `switch`-`case` の `default` 句みたいなもので、「他のどの条件も満たさないときの最後の受け口」みたいに使うものです。
なので、今回の主題の null 判定に限らず、`if` 単体で使うものではありません。

## x is { } (非 null)

もう1個、`is { }` という書き方でも「null じゃない」判定ができます。
知らないと何が何だかわからない謎な書き方ですが、
文法的にいうとこれは「[プロパティ パターン](../../../../study/csharp/datatype/patterns.md#property)」というものになります。

<pre class="source" title="is { } で null じゃない判定">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X <span class="reserved">is</span> { } x)
    {
        <span class="comment">// ここに来るのは a.X が null じゃなかった時だけ。</span>
        <span class="comment">// 起こる結果は is object x と同じ。</span>
        <span class="comment">// x を使って何か処理をする。</span>
    }
}
</code></pre>

本来は以下のように、再帰的にプロパティの中身を確認できる「パターン」です。

<pre class="source" title="{ } の本来の使い方は「再起プロパティ パターン」">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span> <span class="reserved">is</span> { X: <span class="reserved">object</span> <span class="variable">x</span> })
    {
        <span class="comment">// a の中身の X プロパティの中身をチェック。</span>
        <span class="comment">// ちなみにこの場合、a 自体の null チェックもかかるので、a != null &amp;&amp; a.X != null と似た処理。</span>
    }
}
</code></pre>

ただ、`{}` の中に何もなくても「null じゃない」判定だけはかかるので、その用途に流用できます。
ちょっと濫用・悪用気味ではありますが、「null じゃない」判定をしつつ変数で受ける手段としては一番短い書き方になります。

## is not null

`x is { }` は最も短く「null じゃない」判定を書ける手段ではあるんですが、
なにぶん濫用気味な書き方で、知らない人が見て理解しにくい、知っていても「null じゃない」という意図が伝わりにくいという問題があります。

そこで結局、`!(x is null)` という書き方の方がいいんじゃないかという話にもなるんですが…
これはこれで、`!()` も十分に見にくい(`()` が邪魔だし、意味を真逆にする割には `!` という記号は視認性が悪すぎて見逃す)という問題があります。

あと、以下のような「書き間違い」をする人が後を絶たないという問題も起こしました。

<pre class="source" title="!is 問題">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X !<span class="reserved">is</span> <span class="reserved">null</span>) <span class="comment">// is not のつもりで !is とか書く</span>
    {
        <span class="comment">// ちなみにこの ! は not の意味にならず、このコードは is null (意図と真逆)になる。</span>
    }
}
</code></pre>

この `!` は[null 判定の抑止](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving)、要するに、コンパイラーが正しくフロー解析できなさそうな微妙なコードで、コンパイラーの警告をもみ消すために使う演算子です。
フロー解析(あくまでコンパイラー内での処理)に使うだけであって、この `!` の有無はコンパイル結果には全く影響を及ぼしません。
なので、`x !is null` と `x is null` が全く同じ意味。

一方、C# 9.0 では `not` パターンというものが導入されて、今度こそ is not の意味のパターンが書けるようになりました。

<pre class="source" title="is not null">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
    {
        <span class="comment">// ちゃんと null じゃないときだけここを通る。</span>
        <span class="comment">// != null と違ってユーザー定義演算子は呼ばれず、単なる 0 比較。</span>
    }
}
</code></pre>

## is not { } (null の時に early return)

ここからは C# 9.0 のバグの話。
Visual Studio 16.8 (C# 9.0 の初期リリース。2020年11月リリース版)時点の C# には
`is not { } x` という書き方にバグがあります(`is not object x` でも同様にバグあり)。

`not { }` は「null じゃない」をさらに否定しているので結局「null である」という判定になります。
単に「null である」判定をしたいだけなら `is null` と書けばいい話なんですが、
「変数で受けつつ null である判定」という処理をしたいときに `is not { } x` という書き方をします。

<pre class="source" title="is not { } で null 時 early return">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="type">A</span> <span class="variable">a</span>)
{
    <span class="control">if</span> (<span class="variable">a</span>.X <span class="reserved">is</span> <span class="reserved">not</span> { } x) <span class="control">return</span>; <span class="comment">// null だったら early return。</span>
 
    <span class="comment">// x を使って何か処理をする。</span>
    <span class="comment">// ここでは x に非 null な値が入っているはず。</span>
}
</code></pre>

`is not { } x` や `is not object x` とい書き方はまさにこの「null のときに early return」のためにあって、null じゃなければその値が変数 `x` に入った上で `else` 側に流れます。

ですが、バグで、時々その「null じゃない値を変数 `x` で受ける」という処理が消えてしまうことがあるそうです。
上記コードはちゃんと動くんですが、例えば以下のコードだと `x` が null のままになっていて実行時例外を起こします。

<pre class="source" title="16.8 時点の is not { } のバグ">
<code><span class="reserved">using</span> System;
 
<span class="method">M</span>(<span class="string">&quot;abc&quot;</span>);
 
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">not</span> { } x) <span class="control">return</span>; <span class="comment">// null だったら early return。</span>
 
    <span class="comment">// x には s が代入されていないとおかしいはずなのに…</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span>.Length); <span class="comment">// ここでぬるぽ(バグ)。</span>
}
</code></pre>

バグです。
バグ報告済みというか、[報告されて早々に修正・ merge 済み](https://github.com/dotnet/roslyn/pull/49369)で、Visual Studio 16.9 では直る見込みです(16.8.1 とかにもこの修正が取り込まれるかは未定)。
