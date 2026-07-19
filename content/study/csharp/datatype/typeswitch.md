---
title: "is、switch の拡張 (型スイッチ)"
source_url: "https://ufcpp.net/study/csharp/datatype/typeswitch/"
content_type: "Article"
published_at: "2016-09-19T00:00:00"
updated_at: "2019-02-11T00:00:00"
tags: []
umbraco_id: 1956
parent_id: 1940
sort_order: 2
aliases:
  - "/csharp/datatype/typeswitch/"
---

# is、switch の拡張 (型スイッチ)

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
<h5 class="version version7">Ver. 7</h5>

C# 7.0で、[`is`演算子](../oop/oo_polymorphism.md#downcast)や[`switch`ステートメント](../structured/st_branch.md#switch)の`case`が拡張されました。

C# 6.0 以前では以下のような仕様でした。

- `is`演算子 … `x is T` と言うように、型の判定だけができた
- `switch`ステートメントの`case` … `case` の後ろには定数だけが指定で来た

これに対して、C# 7.0 以降では、`is`、`case`の後ろに「パターン」を指定できます。
「パターン」の詳細については[次項](patterns.md)で別途説明する予定ですが、
簡単に概要だけ表にすると以下のようなものがあります。

| パターン | バージョン | 概要 | 例 |
| --- | --- | --- | ------------- |
| 型パターン | C# 7.0 | 型の判定 | `int i`、`string s` |
| 定数パターン | C# 7.0 | 定数との比較 | `null`、`1` |
| var パターン | C# 7.0 | 何にでもマッチ・変数で受け取り | `var x` |
| 破棄パターン | C# 8.0 | 何にでもマッチ・無視 | `_` |
| 位置パターン | C# 8.0 | [分解](deconstruction.md)と同じ要領で、再帰的にマッチングする | `(1, var i, _)` |
| プロパティ パターン | C# 8.0 | プロパティに対して再帰的にマッチングする | `{ A: 1, B: var i }` |

C# 7.0 時点では「型パターン」が主だった機能だったため、
`is`や`switch`の拡張を指して「型スイッチ」(type switch)と呼ばれたりもしました。

本項では、まずは`is`や`switch`がC# 6.0以前と比べてどう変わったかについて焦点を当てます。
例なども、主に型パターン(C# 7.0)で説明していきます。
パターン自体の詳細については次項の「[パターン マッチング](patterns.md)」を参照してください。

##<a id="sec-generated-title-2"></a> <a id="is"></a>is演算子の拡張
C# 7では、`is`演算子で以下のような書き方ができるようになりました。

<pre class="source" title="is = 型判定">
<code><span class="input">型を調べたい変数</span> <span class="reserved">is</span> <span class="input">型</span> <span class="input">新しい変数</span>
</code></pre>

(正確に言うと`is`の後ろに新たに書けるようになったのは「パターン」で、
これはそのうちの「型パターン」と呼ばれるものです。)

C# 6以前の`is`演算子は少し使い勝手が悪い面がありました。型の一致を判定するだけならいいんですが、
型変換も絡むといまいちです。

例えば、以下のように型を判定するだけなら`is`演算子の出番です。

<pre class="source" title="is = 型判定">
<code><span class="comment">// 型判定のみなら、これまでの is 演算子でも十分</span>
<span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">string</span>) <span class="type">Console</span>.WriteLine(<span class="string">"string"</span>);
</code></pre>

ところが、型を判定したうえでダウンキャストしたいという場面では、以下のように、「2度手間」になって、コード量的にも実行効率的にもよくないです。

<pre class="source" title="ダウンキャストしたい場合、is はいまいち">
<code><span class="comment">// 型変換もしたい</span>
<span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">string</span>)
{
    <span class="reserved">var</span> s = (<span class="reserved">string</span>)obj;
    <span class="comment">//↑ isとキャストで2つの別命令を使う。二重処理になってるだけで無駄</span>
    <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + s.Length);
}
</code></pre>

結局、以下のように、`as`演算子を使うことが推奨されます。

<pre class="source" title="ダウンキャストにはasを使う">
<code><span class="comment">// 結局、as 演算子 + null チェックを使うことになる</span>
<span class="reserved">var</span> s = obj <span class="reserved">as</span> <span class="reserved">string</span>;
<span class="reserved">if</span> (s != <span class="reserved">null</span>)
{
    <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + s.Length);
}
</code></pre>

これに対して、C# 7では、`is`演算子で以下のような書き方ができるようになりました。

<pre class="source" title="">
<code><span class="comment">// C# 7での新しい書き方</span>
<span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">string</span> <em>s</em>)
{
    <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + s.Length);
}
</code></pre>

挙動的には、先ほどの`as`演算子を使ったものとまったく同じ挙動になります。
`is`演算子で型を判定しつつ(`bool`の戻り値を返しつつ)、その型への変換結果を新しい変数で受け取れます。

###<a id="sec-generated-title-3"></a> <a id="scope"></a>is演算子で宣言された変数のスコープ
`is`演算子の拡張によって、式の中で変数宣言ができるようになりました。
そこで問題になるのはこの変数のスコープです。

概ね、「その式を含むブロック内」と考えていいんですが、`if`や`while`などの中で使ったときなど、いくつか特殊な場合があります。
詳細については「[式の中で変数宣言](../start/st_scope.md#declaration-expressions)」を参照してください。

###<a id="sec-generated-title-4"></a> <a id="null-check"></a>is演算子によるnullチェック
元々の`is`演算子の仕様でもあるんですが、`null`には型がなくて常に`is`に失敗します(`false`を返す)。

<pre class="source" title="nullは型を持たない">
<code><span class="reserved">string</span> x = <span class="reserved">null</span>;

<span class="reserved">if</span> (x <span class="reserved">is</span> <span class="reserved">string</span>)
{
    <span class="comment">// x の変数の型は string なのに、is string は false</span>
    <span class="comment">// is 演算子は変数の実行時の中身を見る ＆ null には型がない</span>
    <span class="type">Console</span>.WriteLine(<span class="string">"ここは絶対通らない"</span>);
}
</code></pre>

この仕様は、C# 7からの新しい構文でも引き継いでいて、`null`じゃないときだけだけ何かの処理をしたいときに使えます。
と言っても、参照型の場合にはあまり使い道はありませんが、以下のような書き方ができます。

<pre class="source" title="is演算子でnullチェック">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">string</span> nullable)
{
    <span class="reserved">if</span> (nullable <span class="reserved">is</span> <span class="reserved">string</span> nonNull)
    {
        <span class="comment">// nonNull には絶対に null が入らない</span>
        <span class="comment">// nullable をそのまま使っても、if の結果、null じゃない保証があるのであまり意味はないけども</span>
        <span class="type">Console</span>.WriteLine(nonNull.Length);
    }
}
</code></pre>

この書き方が役に立つのは、値型と[null許容型](../resource/sp2_nullable.md)を使う場合でしょう。
例えばC# 6以前だと、以下のような書き方になります。

<pre class="source" title="C# 6以前のnull許容型のnullチェック">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span>? x)
{
    <span class="comment">// C# 6以前の書き方</span>
    <span class="reserved">if</span> (x.HasValue)
    {
        <span class="comment">// この「.GetValueOrDefault()」をいちいち書くのが結構うっとおしい</span>
        <span class="comment">// x * x だと、(x.HasValue & x.HasValue) ? (int?)(x.GetValueOrDefault() * x.GetValueOrDefault()) : null みたいなコードに展開されてしまう</span>
        <span class="reserved">int</span> n = x.GetValueOrDefault();
        <span class="type">Console</span>.WriteLine(n * n);
    }
}
</code></pre>

これが、C# 7で以下のように書けるようになります。

<pre class="source" title="C# 7からのnull許容型のnullチェック">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">int</span>? x)
{
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="reserved">int</span> <em>n</em>)
    {
        <span class="type">Console</span>.WriteLine(n * n);
    }
}
</code></pre>

ただ、1つ注意が必要なのは、`is var` という似て非なる構文がある点です。
`is var` ([`var`パターン](patterns.md#var)と言って、[`is T`](patterns.md#declaration) とは別扱い)を使った場合、nullチェックはされません。
`var` は何でも受け取れる構文で、null も受け付けます。

ちなみに、C# 8.0 では、[再帰パターン](patterns.md#recursive)が暗黙的に null チェックも含んでいることを使って、手短に null チェックもできます
(参考: [非 null マッチング](patterns.md#non-null))。

<pre class="source" title="パターンを使って非 null チェック">
<code><span class="reserved">string</span> <span class="variable">s</span> = <span class="reserved">null</span>;
 
<span class="comment">// 型を明示した場合、null にマッチしない</span>
<span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">string</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;ここは通らない&quot;</span>);
 
<span class="comment">// var パターンは何にでも(null 含む)マッチする</span>
<span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">var</span> <span class="reserved">_</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;ここは通る&quot;</span>);
 
<span class="comment">// 再帰パターンで型を省略すると null チェックも含む</span>
<span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> { }) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;ここは通らない&quot;</span>);
</code></pre>

###<a id="sec-generated-title-5"></a> <a id="invariant-meaning"></a>余談: 変数の意味を変えない
プログラミング言語によっては、以下のように、`is`演算子で型を判定した後には、自動的にその型扱いしてくれる言語もあります。

<pre class="source" title="is によって変数の意味を変える">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">object</span> obj)
{
    <span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">string</span>)
    {
        <span class="comment">// この中では obj を string 扱いできる言語がある</span>
        <span class="comment">// C# ではコンパイル エラー</span>
        <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + obj.Length);
    }
    <span class="reserved">else</span> <span class="reserved">if</span> (obj <span class="reserved">is</span> <span class="reserved">int</span>)
    {
        <span class="comment">// 同上、int 扱いできる言語がある</span>
        <span class="comment">// C# ではコンパイル エラー</span>
        <span class="type">Console</span>.WriteLine(<span class="string">"int "</span> + (obj * obj));
    }
}
</code></pre>

C# では、こういう、「`object`だと思っていたものが一定範囲でだけ別の型になる」というようなことはやらない方針です。

また、以下のように、同名の別変数を導入できる言語もありますが、こちらもC#では認めていません。

<pre class="source" title="is 演算子で同名の別変数を導入">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">object</span> x)
{
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="reserved">string</span> x)
    {
        <span class="comment">// 引数の x とは別に、is 演算子で別の「x」を導入できる言語もある</span>
        <span class="comment">// C# ではコンパイル エラー</span>
        <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + x.Length);
    }
}
</code></pre>

C#では、変数はスコープ内で意味不変(invariant meaning)であるべきという方針を持っています。
上記の2つの例では、`obj`や`x`が部分的に(`if`の中でだけ)別の意味になるので、C#としては認めたくないものになります。

<!-- original-page-break -->


##<a id="sec-generated-title-6"></a> <a id="switch"></a>switchステートメントの拡張
C# 7では、`switch`ステートメントの`case`句に、値だけでなく、パターンを書けるようになりました。
パターンの書き方は、前節の`is`演算子と同様です。
また、型による条件に加えて、`when`句というものを付けて追加の条件式を書くこともできます。

<pre class="source" title="switchステートメントの拡張" lang="">
<code><span class="reserved">switch</span>(<span class="input">変数</span>)
{
    <span class="reserved">case</span> <span class="input">型</span> <span class="input">変数</span>:
        <span class="comment">// 型が一致しているときにここに来る</span>
        <span class="comment">// その型に変換した結果が変数に入っている</span>
        <span class="reserved">break</span>;
    <span class="reserved">case</span> <span class="input">型</span> <span class="input">変数</span> <span class="reserved">when</span> <span class="input">条件式</span>:
        <span class="comment">// 型が一致していて、かつ、条件式満たしているときにここに来る</span>
        <span class="reserved">break</span>;
    <span class="reserved">case</span> <span class="input">値</span>:
        <span class="comment">// 通常の値による条件との混在も可能</span>
        <span class="reserved">break</span>;
      ・
      ・
      ・
    <span class="reserved">default</span>:
        <span class="comment">// どの条件も満たさない時に実行される</span>
        <span class="reserved">break</span>;
}
</code></pre>

例えば以下のような書き方ができます。

<pre class="source" title="型を見て分岐する switch ステートメントの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> F(<span class="reserved">object</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="reserved">string</span> s:
            <span class="type">Console</span>.WriteLine(<span class="string">"string #"</span> + s.Length);
            <span class="reserved">break</span>;
        <span class="reserved">case</span> 7:
            <span class="type">Console</span>.WriteLine(<span class="string">"7の時だけここに来る"</span>);
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="reserved">int</span> n <span class="reserved">when</span> n &gt; 0:
            <span class="type">Console</span>.WriteLine(<span class="string">"正の数の時にここに来る "</span> + n);
            <span class="comment">// ただし、上から順に判定するので、7 の時には来なくなる</span>
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="reserved">int</span> n:
            <span class="type">Console</span>.WriteLine(<span class="string">"整数の時にここに来る"</span> + n);
            <span class="comment">// 同上、0 以下の時にしか来ない</span>
            <span class="reserved">break</span>;
        <span class="reserved">default</span>:
            <span class="type">Console</span>.WriteLine(<span class="string">"その他"</span>);
            <span class="reserved">break</span>;
    }
}
</code></pre>

###<a id="sec-generated-title-7"></a> <a id="sequential"></a>上から逐次判定
C# 6までの、値による分岐しかなかった`switch`ステートメントとはちょっと違う部分があります。
以下の点に気を付けてください。

- 条件の範囲が被る場合がある
  - 値による分岐の場合は、各 `case` がそれぞれ排他だった
  - 型による分岐が入ったことで、上記の例でいう `7` ⊃ `int`かつ正の数 ⊃ `int` のように、被りが起こり得る
- 条件は上から順に判定する
  - 被りがない場合なら順序を気にする必要はなかった
      - なので、「ジャンプ テーブル化」(後述)という最適化手法が使えていた
  - 型による分岐を1つでも含むと、この前提が崩れて、ジャンプ テーブル化できない(逐次判定しかしない)

ジャンプ テーブル化の説明のために、以下のような`switch`を考えましょう。

<pre class="source" title="値による条件のみのswitchの例">
<code><span class="reserved">switch</span>(n)
{
    <span class="reserved">case</span> 0: <span class="reserved">return</span> <span class="string">"zero"</span>;
    <span class="reserved">case</span> 1: <span class="reserved">return</span> <span class="string">"one"</span>;
    <span class="reserved">case</span> 2: <span class="reserved">return</span> <span class="string">"two"</span>;
    <span class="reserved">case</span> 3: <span class="reserved">return</span> <span class="string">"three"</span>;
    <span class="reserved">case</span> 4: <span class="reserved">return</span> <span class="string">"four"</span>;
    <span class="reserved">case</span> 5: <span class="reserved">return</span> <span class="string">"five"</span>;
    <span class="reserved">case</span> 6: <span class="reserved">return</span> <span class="string">"six"</span>;
    <span class="reserved">case</span> 7: <span class="reserved">return</span> <span class="string">"seven"</span>;
    <span class="reserved">case</span> 8: <span class="reserved">return</span> <span class="string">"eight"</span>;
    <span class="reserved">case</span> 9: <span class="reserved">return</span> <span class="string">"nine"</span>;
    <span class="reserved">default</span>: <span class="reserved">return</span> <span class="string">"other"</span>;
}
</code></pre>

こういう`switch`であれば、以下のように、辞書を引いて結果を得ることもできるはずです。

<pre class="source" title="switchの辞書化">
<code><span class="reserved">var</span> map = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">string</span>&gt;
{
    { 0, <span class="string">"zero"</span> },
    { 1, <span class="string">"one"</span> },
    { 2, <span class="string">"two"</span> },
    { 3, <span class="string">"three"</span> },
    { 4, <span class="string">"four"</span> },
    { 5, <span class="string">"five"</span> },
    { 6, <span class="string">"six"</span> },
    { 7, <span class="string">"seven"</span> },
    { 8, <span class="string">"eight"</span> },
    { 9, <span class="string">"nine"</span> },
};

<span class="reserved">string</span> s;
<span class="reserved">if</span> (map.TryGetValue(n, <span class="reserved">out</span> s)) <span class="reserved">return</span> s;
<span class="reserved">else</span> <span class="reserved">return</span> <span class="string">"other"</span>;
</code></pre>

`case`の個数が少ないうちは普通に上から順に等値判定していく方が軽いんですが、
`case`数が増えれば増えるほど、辞書化した方が有利になります。

そこで、C# の`switch`ステートメント(というか、.NETの中間言語の`switch`命令)では、`case`の数が多い場合にこういう辞書を使った最適化を行うようになっています。
正確にいうと、辞書の値は条件分岐によるジャンプ先が入っていて、`goto`的な命令との組み合わせで実現されます。
そこで、「ジャンプ先のテーブルを引く」という意味で「ジャンプ テーブル化」と呼ばれます。

繰り返しになりますが、`case`に型による条件を書いてしまうと、こういうジャンプ テーブル化ができなくなります。
というより、コンパイル結果的には`switch`命令が使えず、`if-else`を繰り返すようなコードにコンパイルされます。
上から順に逐次判定になるので、`case`数があまりにも多いと実行性能的にあまりよくないので注意してください。

また、上の方の`case`にあるほど判定が速いことになります。
以下のように、一番上の`case`と一番下の`case`では、かなりパフォーマンスに差が出ます。
(なので、パフォーマンスが気になるなら、発生頻度が高いものほど上の方に書く必要があります。)

<pre class="source" title="逐次判定によるパフォーマンスの変化">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Diagnostics;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> sw = <span class="reserved">new</span> <span class="type">Stopwatch</span>();

        <span class="comment">// bool 型は一番先頭 = 速い</span>
        <span class="reserved">object</span> t = <span class="reserved">true</span>;
        sw.Start();
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 100000; i++) TypeSwitch(t);
        sw.Stop();
        <span class="type">Console</span>.WriteLine(<span class="string">"bool   "</span> + sw.Elapsed); <span class="comment">// かなり速いはず</span>

        <span class="comment">// double 型は一番末尾 = 遅い</span>
        <span class="reserved">object</span> d = 1.1;
        sw.Restart();
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 100000; i++) TypeSwitch(d);
        sw.Stop();
        <span class="type">Console</span>.WriteLine(<span class="string">"string "</span> + sw.Elapsed); <span class="comment">// 手元の環境では5倍くらい遅かった</span>

        <span class="comment">// どの case にもない型。default 句に行く</span>
        <span class="reserved">var</span> s = <span class="type">DateTime</span>.UtcNow;
        sw.Restart();
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 100000; i++) TypeSwitch(s);
        sw.Stop();
        <span class="type">Console</span>.WriteLine(<span class="string">"string "</span> + sw.Elapsed); <span class="comment">// 一番最後まで判定するので遅い</span>
    }

    <span class="reserved">static</span> <span class="reserved">int</span> TypeSwitch(<span class="reserved">object</span> x)
    {
        <span class="reserved">switch</span> (x)
        {
            <span class="reserved">default</span>: <span class="reserved">return</span> -1; <span class="comment">// ちなみに、default 句はどこに書こうと必ず一番最後</span>
            <span class="reserved">case</span> <span class="reserved">bool</span> <span class="reserved">_</span>: <span class="reserved">return</span> 0; <span class="comment">// 前から順に判定ということは、bool の時が一番早い</span>
            <span class="reserved">case</span> <span class="reserved">sbyte</span> <span class="reserved">_</span>: <span class="reserved">return</span> 1;
            <span class="reserved">case</span> <span class="reserved">byte</span> <span class="reserved">_</span>: <span class="reserved">return</span> 2;
            <span class="reserved">case</span> <span class="reserved">short</span> <span class="reserved">_</span>: <span class="reserved">return</span> 3;
            <span class="reserved">case</span> <span class="reserved">ushort</span> <span class="reserved">_</span>: <span class="reserved">return</span> 4;
            <span class="reserved">case</span> <span class="reserved">int</span> <span class="reserved">_</span>: <span class="reserved">return</span> 5;
            <span class="reserved">case</span> <span class="reserved">uint</span> <span class="reserved">_</span>: <span class="reserved">return</span> 6;
            <span class="reserved">case</span> <span class="reserved">long</span> <span class="reserved">_</span>: <span class="reserved">return</span> 7;
            <span class="reserved">case</span> <span class="reserved">ulong</span> <span class="reserved">_</span>: <span class="reserved">return</span> 8;
            <span class="reserved">case</span> <span class="reserved">float</span> <span class="reserved">_</span>: <span class="reserved">return</span> 9;
            <span class="reserved">case</span> <span class="reserved">double</span> <span class="reserved">_</span>: <span class="reserved">return</span> 10; <span class="comment">// 逆に double の時は凄く遅い</span>
        }
    }
}
</code></pre>

ちなみに、この例でも書いてありますが、逐次判定になっていたとしても`default`句にたどり着くのは必ず一番最後です。

<!-- original-page-break -->


##<a id="sec-generated-title-8"></a> <a id="usage"></a>型スイッチ(switch を使ったパターン マッチング)の用途
型によって分岐する方法としては、[仮想メソッド](../oop/oo_polymorphism.md#virtual)を使う方法があります。
オブジェクト指向プログラミング言語としては、仮想メソッドが相当に便利で、実行性能もよく、こちらが好まれます。
極端な意見では、「型を調べたら負け」、「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)が必要なのは筋が悪い」という人すらいます。

ここでは、この仮想メソッドと、本稿の主題である型スイッチの使い分けについて説明します。

例として、以下のようなクラス階層を考えます。

<pre class="source" title="式ノード">
<code><span class="reserved">public abstract class</span> <span class="type">Node</span> { }

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Var</span> : <span class="type">Node</span> { }

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Const</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Const(<span class="reserved">int</span> value) { Value = value; }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Add</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="type">Node</span> Left { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Node</span> Right { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Add(<span class="type">Node</span> left, <span class="type">Node</span> right)
    {
        Left = left;
        Right = right;
    }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Mul</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="type">Node</span> Left { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Node</span> Right { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Mul(<span class="type">Node</span> left, <span class="type">Node</span> right)
    {
        Left = left;
        Right = right;
    }
}
</code></pre>

説明都合で簡素化していますが、数式を扱うようなクラスです。
要するに、例えば、「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mi>x</mi><mo>+</mo><mn>1</mn></math>」というような式を、以下のようなコードで表すためのクラスです。

<pre class="source" title="x × x + 1">
<code><span class="reserved">var</span> expression = <span class="reserved">new</span> <span class="type">Add</span>(
    <span class="reserved">new</span> <span class="type">Mul</span>(
        <span class="reserved">new</span> <span class="type">Var</span>(),
        <span class="reserved">new</span> <span class="type">Var</span>()),
    <span class="reserved">new</span> <span class="type">Const</span>(1));
</code></pre>

![式を扱いためのクラス](../../../../assets/media/1094/expressions.png)

これに対して、「変数<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi></math>の値を与えて、式の計算結果を得る」というようなメソッドを、仮想メソッドと型スイッチの両方で作ってみましょう。

まず、仮想メソッドなら以下のようになるでしょう(必要な部分だけを抜き出してあります)。

<pre class="source" title="仮想メソッドで実装する例">
<code><span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">int</span> Calculate(<span class="reserved">int</span> x);
}

<span class="reserved">class</span> <span class="type">Var</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> Calculate(<span class="reserved">int</span> x) =&gt; x;
}

<span class="reserved">class</span> <span class="type">Const</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> Calculate(<span class="reserved">int</span> x) =&gt; Value;
}

<span class="reserved">class</span> <span class="type">Add</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> Calculate(<span class="reserved">int</span> x) =&gt; Left.Calculate(x) + Right.Calculate(x);
}

<span class="reserved">class</span> <span class="type">Mul</span>
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> Calculate(<span class="reserved">int</span> x) =&gt; Left.Calculate(x) * Right.Calculate(x);
}
</code></pre>

一方、型スイッチを使って書くなら以下のようになります。

<pre class="source" title="型スイッチで実装する例">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">NodeExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> Calculate(<span class="reserved">this</span> <span class="type">Node</span> n, <span class="reserved">int</span> x)
    {
        <span class="reserved">switch</span> (n)
        {
            <span class="reserved">case</span> <span class="type">Var</span> v: <span class="reserved">return</span> x;
            <span class="reserved">case</span> <span class="type">Const</span> c: <span class="reserved">return</span> c.Value;
            <span class="reserved">case</span> <span class="type">Add</span> a: <span class="reserved">return</span> Calculate(a.Left, x) + Calculate(a.Right, x);
            <span class="reserved">case</span> <span class="type">Mul</span> m: <span class="reserved">return</span> Calculate(m.Left, x) * Calculate(m.Right, x);
        }
        <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentOutOfRangeException</span>();
    }
}
</code></pre>

それぞれ、以下のような特徴があります。

- 性能:
  - 〇 仮想メソッドはかなり実行性能がいい
  - × 型スイッチでは性能面はかなわない
- 実装の強制
  - 〇 仮想メソッドなら、抽象メソッドにしておけば派生クラスでの実装漏れがあり得なくなる
  - × 型スイッチの場合、`case`への追加忘れがあり得る
- 実装を書ける場所
  - × 仮想メソッドはクラスの中にないとダメ
  - 〇 型スイッチなら拡張メソッドなど、クラスの外でも使える

基本的にはやっぱり仮想メソッドの方が性能・使い勝手の面で良かったりします。
ただ、仮想メソッド最大の問題は、クラスの中に書くのが必須ということです。
どうしてもクラスの中には書けない(クラスの作者自身が書けず、第三者が書く必要がある)場合というのはあって、
この場合は型スイッチを使う必要があります。

クラスの中に書くということは、そのクラスを使いたい人なら誰でも使うような汎用的な機能なはずです。
仮想メソッドはそういう汎用的な機能にしか使えないということになります。

一方で、使う人それぞれの固有の事情であれば、使う人の側が自分で書くことになるでしょう。

例えば、表示要件を考えてみます。
あるアプリでは、「`x * x + 1`」というように、プログラミング言語によくあるように、掛け算を`*`で表して文字列化したいかもしれません。
またあるアプリでは、「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mi>x</mi><mo>+</mo><mn>1</mn></math>」というように、ちゃんと数式フォントで、掛け算には×記号を使って表示したいかもしれません。
数式表示のためには、自前でレンダリングを行うべきかもしれませんし、
「`<math><mi>x</mi><mo>×</mo><mi>x</mi><mo>+</mo><mn>1</mn></math>`」というようなMathML文字列を作って、これを何らかのライブラリに解釈してもらうのがいいかもしれません。

数式データを使う用途もアプリごとに変わってくるでしょう。
あるアプリでは、数式を組版して表示すること自体が目的かもしれません。
またあるアプリでは、数式を微分したり方程式の解を求めたり、数学計算のために使うかもしれません。
あるいは、プログラミング言語を作っていて、式を計算するCPU命令を出力するための中間形式として使うかもしれません。

こういう、クラス作者側では用途が見えないものは、型スイッチを使って書くことになります。

###<a id="sec-generated-title-9"></a> <a id="performance"></a>補足: 型スイッチの性能
仮想メソッドと比べると遅いという話をしましたが、これは、仮想メソッドが性能よすぎるだけで、
型スイッチもそこまでひどい性能ではありません。
先ほどの`Calculate`の例でいうと、大まかに計測したところ4倍程度の差でした。

「型を見る」というと、[リフレクション](../dynamic/sp_reflection.md)を想像する人がいるようです。
リフレクションを使う場合、確かに、2・3桁(2・3倍じゃなくて、桁が変わる)遅くなる場合があります。
しかし、型スイッチに必要なのは「その型に代入できるかどうか」だけで、これはそこそこ高速な処理です。
リフレクションで遅いのは、「ある型がどういうメンバーを持っているか調べる」であるとか、
「メソッド名を文字列で渡してメソッドを探して、そのメソッドを実行する」であるとかです。

要するに、リフレクションで取れる型情報や、それの使い方には何段階かあって、それぞれ負荷の度合いも変わります。

![型情報の使い方と実行速度](../../../../assets/media/1095/typeinfo.png)

型識別だけなら大したコストは掛かりません。そして、型スイッチが使うのはこの型識別情報だけです。

むしろ、型スイッチの遅さの原因は、
[前項](#sequential)で説明したような、逐次判定のせいです。
上から1つ1つ`case`の条件判定しているので、平均的には`case`の数に比例した処理量が必要になります。


<!-- original-page-break -->


##<a id="sec-generated-title-10"></a> <a id="generic-type-switch"></a>余談: ジェネリック型に対する型パターン
<h5 class="version version7_1">Ver. 7.1</h5>

C# 7.0の時点では、[ジェネリクス](../oop/sp2_generics.md)が絡む場合、
例えば以下のようなコードはコンパイル エラーになっていました。
(ジェネリックな型`T`の変数に対して`switch`できない。ちなみに、一度`object`にキャストすればできる。)

<pre class="source" title="C# 7.0ではコンパイルできないswitchの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> M&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
{
    <span class="reserved">switch</span> (x)
    {
        <span class="reserved">case</span> <span class="reserved">int</span> i:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="reserved">string</span> s:
            <span class="reserved">break</span>;
    }
}
</code></pre>

「`T`を`int`や`string`として処理できない」と言った旨のコンパイル エラーが出ます。

さらにいうと、以下のような需要が結構ありそうな場面でも、C# 7.0ではコンパイル エラーになりました。

<pre class="source" title="C# 7.0ではコンパイルできないswitchの例(型制約付き)">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived1</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived2</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">Derived3</span> : <span class="type">Base</span> { }

<span class="comment">// こういう、型制約付きのやつですら 7.0 ではダメだった</span>
<span class="reserved">static</span> <span class="reserved">void</span> N&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Base</span>
{
    <span class="reserved">switch</span> (x)
    {
        <span class="reserved">case</span> <span class="type">Derived1</span> d:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="type">Derived2</span> d:
            <span class="reserved">break</span>;
        <span class="reserved">case</span> <span class="type">Derived3</span> d:
            <span class="reserved">break</span>;
    }
}
</code></pre>

C# 7.0でも、以下のように、`as`演算子を使った場合にはちゃんとコンパイルできます。
型パターンは、内部的には`as`演算子に展開される機能で、`as`演算子にできて型パターンにできないことがあるのは不自然です。

<pre class="source" title="as 演算子での置き換え">
<code><span class="reserved">static</span> <span class="reserved">void</span> N&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x)
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">Base</span>
{
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived1</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived2</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
    { <span class="reserved">var</span> d = x <span class="reserved">as</span> <span class="type">Derived3</span>; <span class="reserved">if</span> (d != <span class="reserved">null</span>) { <span class="reserved">return</span>; } }
}
</code></pre>

そこで、C# 7.1では、上記コードのような、ジェネリックな型に対する型パターンを使えるようになりました。
(新機能というよりは、仕様漏れ・バグ修正の類です。)

##<a id="sec-generated-title-11"></a> <a id="generic-is-null"></a>余談: ジェネリック型に対する is null
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 から、
以下のコードがコンパイルできるようになりました。

<pre class="source" title="ジェネリック型に対する is null">
<code><span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">M</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>;
</code></pre>

元々 `x == null` であればコンパイルできていたのに、`x is null` がコンパイルできないのは変だということで修正されました。
型引数 `T` が[非 null 値型](../resource/sp2_nullable.md#non-nullable)の時には常に false になります。


<!-- original-page-break -->


##<a id="sec-generated-title-12"></a> <a id="switch-expression"></a>switch 式
<h5 class="version version8">Ver. 8.0</h5>

C# 8.0 では、`switch` の[式](../structured/miscexpressions.md#term)版が追加されました。
式なので戻り値が必須ですが、どこにでも書けて便利です。
また、従来の `switch` ステートメントは C# の前身となるC言語のものの名残を強く残し過ぎていて使いにくいものでしたが、その辺りも解消されて使いやすくなりました。

例えば、以下のような列挙型を使った分岐を考えてみます。

<pre class="source" title="例として使う列挙型(改元で困るやつ)">
<code><span class="reserved">using</span> <span class="reserved">static</span> <span class="type">年号</span>;
 
<span class="reserved">enum</span> <span class="type">年号</span>
{
    明治, 大正, 昭和, 平成
}
</code></pre>

これまでだと、以下のような書き方をせざるを得ないことがあったかと思います。

<pre class="source" title="switch ステートメントの例">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">年号</span> <span class="variable">e</span>)
{
    <span class="reserved">int</span> <span class="variable">y</span>;
    <span class="control">switch</span> (<span class="variable">e</span>)
    {
        <span class="control">case</span> 明治:
            <span class="variable">y</span> = 45;
            <span class="control">break</span>;
        <span class="control">case</span> 大正:
            <span class="variable">y</span> = 15;
            <span class="control">break</span>;
        <span class="control">case</span> 昭和:
            <span class="variable">y</span> = 64;
            <span class="control">break</span>;
        <span class="control">case</span> 平成:
            <span class="variable">y</span> = 31;
            <span class="control">break</span>;
        <span class="control">default</span>: <span class="control">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
    }
    <span class="comment">// y を使って何か</span>
}
</code></pre>

こういう書き方は結構しんどいわけですが、しんどい理由は以下のような点にあります。

- それぞれの条件で1つずつ値を返したいだけなのにステートメントを求められる
- `break` が必須
- `case` ラベルもうざい

ちょこっとごまかす方法として、以下のように別メソッドを1段挟む方法もあるにはありますが、相変わらず`case`や`return`がうっとおしいです。

<pre class="source" title="1段メソッドを挟んでごまかす">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">年号</span> <span class="variable">e</span>)
{
    <span class="reserved">int</span> <span class="method">lastYear</span>()
    {
        <span class="control">switch</span> (<span class="variable">e</span>)
        {
            <span class="control">case</span> 明治: <span class="control">return</span> 45;
            <span class="control">case</span> 大正: <span class="control">return</span> 15;
            <span class="control">case</span> 昭和: <span class="control">return</span> 64;
            <span class="control">case</span> 平成: <span class="control">return</span> 31;
            <span class="control">default</span>: <span class="control">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
        }
    }
 
    <span class="reserved">var</span> <span class="variable">y</span> = <span class="method">lastYear</span>();
    <span class="comment">// y を使って何か</span>
}
</code></pre>

これは、C# 8.0 の `switch` 式を使うと、以下のように書き直すことができます。

<pre class="source" title="switch 式で書き直し">
<code><span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">年号</span> <span class="variable">e</span>)
{
    <span class="reserved">var</span> <span class="variable">y</span> = <span class="variable">e</span> <span class="control">switch</span>
    {
        明治 =&gt; 45,
        大正 =&gt; 15,
        昭和 =&gt; 64,
        平成 =&gt; 31,
        <span class="reserved">_</span> =&gt; <span class="control">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>()
    };
    <span class="comment">// y を使って何か</span>
}
</code></pre>

文法的には以下のようになります。

<pre class="source" title="switch式の書式">
<code><span class="input">変数</span> <span class="control">switch</span>
{
    <span class="input">パターン1</span> =&gt; <span class="input">式1</span>,
    <span class="input">パターン2</span> =&gt; <span class="input">式2</span>,
      ・
      ・
      ・
}
</code></pre>

ステートメントの方の`switch`との弁別のために、`switch`キーワードは後置きになっています。

最後の1個のコンマはあってもなくてもかまいません。
[配列](../structured/st_array.md)や[オブジェクト初期化子、コレクション初期化子](../functional/sp3_lambda.md#init)と同様です。

パターンの部分には「[パターン マッチング](patterns.md)」で説明している任意のパターンを書けます。
また、[`when`句](#switch)を付けることもできます。

<pre class="source" title="switch 式に型パターン、破棄パターン、when 句">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="control">when</span> <span class="variable">x</span> &gt; 0 =&gt; 1,
    <span class="reserved">int</span> <span class="reserved">_</span> =&gt; 2,
    <span class="reserved">_</span> =&gt; 3,
};
</code></pre>

###<a id="sec-generated-title-13"></a> <a id="switch-priority"></a>switch 式の優先度
`switch` 式の優先度は単項演算の下、乗除演算の上になります。
`++x` や `await x` は `switch` 式よりも先に評価されて、
`x * y` や `x + y` は `switch` 式よりも後に評価されます。

<pre class="source" title="switch 式の優先度の例">
<code><span class="comment">// これは (await b) switch { ... } の意味になって、</span>
<span class="comment">// bool を await できないのでコンパイル エラー。</span>
<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> <span class="method">M1</span>(<span class="reserved">bool</span> <span class="variable">b</span>, <span class="type">Task</span> <span class="variable">x</span>, <span class="type">Task</span> <span class="variable">y</span>)
    =&gt; <span class="reserved">await</span> <span class="variable">b</span> <span class="control">switch</span> { <span class="reserved">true</span> =&gt; <span class="variable">x</span>, <span class="reserved">false</span> =&gt; <span class="variable">y</span> };
 
<span class="comment">// これは (++x) switch { ... } の意味で、</span>
<span class="comment">// x に -1 を渡した時だけ false に。</span>
<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">M2</span>(<span class="reserved">int</span> <span class="variable">x</span>)
    =&gt; ++<span class="variable">x</span> <span class="control">switch</span> { 0 =&gt; <span class="reserved">false</span>, <span class="reserved">_</span> =&gt; <span class="reserved">true</span> };
 
<span class="comment">// これは y * (switch { ... }) の意味で、</span>
<span class="comment">// 0 か y が返る。</span>
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M2</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>)
    =&gt; <span class="variable">y</span> * <span class="variable">x</span> <span class="control">switch</span> { 0 =&gt; 0, <span class="reserved">_</span> =&gt; 1 };
</code></pre>

###<a id="sec-generated-title-14"></a> <a id="exhaustive"></a>網羅性
式であるからには、`switch` 式は必ず値を返す必要があります。
なので、パターンには網羅性(exhaustiveness)が求められます。
すなわち、「どのパターンも満たさず`switch`式を抜けてしまう」みたいな状態は許容されません。
ちゃんと C# コンパイラーが網羅性をチェックしていて、抜けがあるとコンパイル エラーになります。

多くの場合、末尾に[`var`パターン](patterns.md#var)か[破棄パターン](patterns.md#discard)を書いて漏れを防ぎます。

<pre class="source" title="var/破棄で「残り全部」を網羅">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    1 =&gt; 2,
    2 =&gt; 4,
    <span class="reserved">_</span> =&gt; 8, <span class="comment">// 破棄パターンで「残り全部」を受付</span>
};
 
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="reserved">int</span> <span class="variable">i</span> =&gt; <span class="variable">i</span>,
    <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length,
    <span class="reserved">var</span> other =&gt; <span class="variable">other</span>.<span class="method">GetHashCode</span>(), <span class="comment">// var パターンで「残り全部」を受付</span>
};
</code></pre>

今のところ、`bool`だけは網羅性を確実にチェックできます。

<pre class="source" title="bool の網羅性チェック">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="reserved">true</span> =&gt; 1,
    <span class="reserved">false</span> =&gt; 0,
    <span class="comment">// true/false で全パターン網羅できているので _ とかは不要</span>
};
 
<span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">x</span>, <span class="reserved">bool</span> <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) <span class="control">switch</span>
{
    (<span class="reserved">false</span>, <span class="reserved">false</span>) =&gt; 0,
    (<span class="reserved">true</span>, <span class="reserved">false</span>) =&gt; 1,
    (<span class="reserved">false</span>, <span class="reserved">true</span>) =&gt; 2,
    (<span class="reserved">true</span>, <span class="reserved">true</span>) =&gt; 4,
    <span class="comment">// 上記4パターンしかありえないので _ とかは不要</span>
};
</code></pre>

将来的には、`enum`型の網羅性や、派生クラスの網羅性もチェックしたいそうですが、
「後からのメンバー追加に弱くなる」など課題があるため、実装されるかどうかは不明瞭です。

####<a id="sec-generated-title-15"></a> <a id="bool-exhaustiveness"></a>余談: bool の網羅性
前節の`switch`式の網羅性チェックと関連して、ステートメントの方の`switch`でも、`bool`の網羅性チェックが働くようになりました。
C# 8.0 前後で挙動が変わるのでご注意ください。

すなわち、以下のような`switch`ステートメントを書いたとき、`default`句に関する扱いが変わります。

<pre class="source" title="bool の網羅性">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="control">switch</span> (<span class="variable">b</span>)
    {
        <span class="control">case</span> <span class="reserved">false</span>: <span class="control">return</span> 0;
        <span class="control">case</span> <span class="reserved">true</span>: <span class="control">return</span> 1;
        <span class="control">default</span>: <span class="control">return</span> -1;
    }
}
</code></pre>

- C# 7.3 以前: `default` が必須
- C# 8.0 以降: `default` が要らないというか、むしろ書くと警告(絶対に来ない条件があるという扱い)

C# 7.3 以前がどうしてそうなっていたかは以前ブログを書いたのでそちらを参照してください: 「[bool 型の false, true, それ以外](../../../blog/2019/1/falsetrueother/index.md)」。

###<a id="sec-generated-title-16"></a> <a id="target-typed"></a>ターゲットからの型決定
`switch` 式にはターゲットからの型推論が働きます。

ここでいうターゲットというのは結果を渡す先のことで、例えば以下のような書き方をした場合、
null を渡す先が `int?` 型の変数なので、この `int?` が「ターゲットの型」になります。

<pre class="source" title="ターゲット(渡す先)の型(この場合は int?)">
<code><span class="reserved">int</span>? <span class="variable">x</span> = <span class="reserved">null</span>;
</code></pre>

`switch` 式では、いろいろな条件でいろいろな値を返すわけですが、
値から「共通の型」を決定できない場合があります。
例えば、以下のように、(例え同じクラスから派生していたとしても)異なる型 `A` と `B` の「共通の型」は判定できず、
コンパイル エラーを起こします。

<pre class="source" title="共通の型を見つけられなくてエラーになる例">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span> { }
 
<span class="reserved">static</span> <span class="reserved">object</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">i</span>)
{
    <span class="comment">// 値が A と B で違う型なので、switch 式が返す型を決定できない。</span>
    <span class="comment">// コンパイル エラーになる。</span>
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">i</span> <span class="error"><span class="control">switch</span></span>
    {
        0 =&gt; <span class="reserved">new</span> <span class="type">A</span>(),
        <span class="reserved">_</span> =&gt; <span class="reserved">new</span> <span class="type">B</span>(),
    };
 
    <span class="control">return</span> <span class="variable">x</span>;
}
</code></pre>

これくらいならば `Base` が共通の型だと判定してほしくも思いますが、
多段派生していたり、インターフェイスも実装していたり複雑な場合のことを考えるとそんなに簡単な話ではありません。

<pre class="source" title="共通型の決定が難しい例">
<code><span class="comment">// 型 D と F の「共通型」といわれると何？</span>
<span class="comment">// インターフェイス J？ それともクラス A？</span>
<span class="reserved">interface</span> <span class="type">I</span> { }
<span class="reserved">interface</span> <span class="type">J</span> { }
<span class="reserved">class</span> <span class="type">A</span> { }
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>, <span class="type">I</span> { }
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">A</span> { }
<span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>, <span class="type">J</span> { }
<span class="reserved">class</span> <span class="type">E</span> : <span class="type">B</span> { }
<span class="reserved">class</span> <span class="type">F</span> : <span class="type">C</span>, <span class="type">J</span> { }
</code></pre>

この問題の回避策は2つあって、1つは特に難しいこともなく、「[キャスト](../start/st_cast.md#cast)しろ」というものです。
C# コンパイラーが理解できるところまでかみ砕いたコードを書いてあげなきゃいけないということで、ちょっと煩雑なコードになります。

<pre class="source" title="キャストで解決">
<code><span class="comment">// 片方を既定型にキャストしておくことで「共通型は Base」と判定できるようになる</span>
<span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">i</span> <span class="control">switch</span>
{
    0 =&gt; (<span class="type">Base</span>)<span class="reserved">new</span> <span class="type">A</span>(),
    <span class="reserved">_</span> =&gt; <span class="reserved">new</span> <span class="type">B</span>(),
};
</code></pre>

もう1つが本節の主題の「ターゲット型からの型決定」です。
先ほどの例では左辺が `var` (型推論)なのでコンパイルできませんが、
以下のように、ターゲット側の型を明示することで、`switch` 式の側の型を `Base` に決定できます。

<pre class="source" title="ターゲットからの型決定">
<code><span class="comment">// 左辺(Base 型の変数)から switch 式の型を Base に決定。</span>
<span class="comment">// コンパイルできるようになる。</span>
<span class="type">Base</span> <span class="variable">x</span> = <span class="variable">i</span> <span class="control">switch</span>
{
    0 =&gt; <span class="reserved">new</span> <span class="type">A</span>(),
    <span class="reserved">_</span> =&gt; <span class="reserved">new</span> <span class="type">B</span>(),
};
</code></pre>

特に役立つのは「1 と null」(`int?` になってほしい)とかでしょう。

<pre class="source" title="1 と null の共通型を判定できないので代わりにターゲット型で解決">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">b</span>)
{
    <span class="comment">// これはコンパイル エラー。1 と null の共通型は C# 8.0 時点では決定できない。</span>
    <span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">b</span> <span class="error"><span class="control">switch</span></span> { <span class="reserved">true</span> =&gt; 1, <span class="reserved">_</span> =&gt; <span class="reserved">null</span> };
 
    <span class="comment">// これはコンパイルできる。ターゲット型から int? に決定済みなので、1 も null も受け付ける。</span>
    <span class="reserved">int</span>? <span class="variable">y</span> = <span class="variable">b</span> <span class="control">switch</span> { <span class="reserved">true</span> =&gt; 1, <span class="reserved">_</span> =&gt; <span class="reserved">null</span> };
}
</code></pre>
