---
title: "パターン マッチング"
source_url: "https://ufcpp.net/study/csharp/datatype/patterns/"
content_type: "Article"
published_at: "2018-11-24T00:00:00"
updated_at: "2021-09-20T00:00:00"
tags: []
umbraco_id: 2176
parent_id: 1940
sort_order: 3
aliases:
  - "/csharp/datatype/patternmatching"
  - "/csharp/datatype/patternmatching/"
  - "/csharp/datatype/patterns/"
  - "/study/csharp/datatype/patternmatching"
  - "/study/csharp/datatype/patternmatching/"
---

# パターン マッチング

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

[前項](typeswitch.md)で説明した通り、C# 7.0で、`is`演算子と`swtich`ステートメントが拡張されて、`is`/`case`の後ろにパターンを書けるようになりました。
パターンには以下のようなものがあります。

| パターン | バージョン | 概要 | 例 |
| --- | --- | --- | ------------- |
| [型パターン](#declaration) | C# 7.0 | 型の判定 | `int i`、`string s` |
| [定数パターン](#constant) | C# 7.0 | 定数との比較 | `null`、`1` |
| [var パターン](#var) | C# 7.0 | 何にでもマッチ・変数で受け取り | `var x` |
| [破棄パターン](#discard) | C# 8.0 | 何にでもマッチ・無視 | `_` |
| [位置パターン](#positional) | C# 8.0 | [分解](deconstruction.md)と同じ要領で、再帰的にマッチングする | `(1, var i, _)` |
| [プロパティ パターン](#property) | C# 8.0 | プロパティに対して再帰的にマッチングする | `{ A: 1, B: var i }` |
| [パターンの組み合わせ](#pattern-combintor) | C# 9.0 | `and` や `or` などでパターンの組み合わせができる | `int x and (x is 0 or 1)` |
| [関係演算パターン](#relational-patterns) | C# 9.0 | `<` や `>` などで数値の範囲を指定してマッチングする | `<= 0 and < 10` |
| [リスト パターン](#list) | C# 11.0 | 配列やリストなどにマッチ | `[]`, `[_, ..]` |

サンプル コード: [https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Data/Patterns](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Data/Patterns)

## <a id="sec-generated-title-2"></a> <a id="nonrecursive"></a>非再帰パターン

<h5 class="version version7">Ver. 7.0</h5>

C# の文法上の区別する意味はないんですが、
パターンのうち、C# 7.0 で入ったものと 8.0 で入ったものの一番の差は再帰があるかどうかです。
C# 7.0 からあるパターンは1層限り、8.0 で追加されたパターンは再帰的に何層もマッチできます。
(再帰がある方が難しいので後からの追加になりました。)

ここではまず、文法が簡単な再帰のないパターンから説明していきます。

### <a id="sec-generated-title-3"></a> <a id="declaration"></a>型パターン (宣言パターン)

C# 6.0以前から元々あった [`is` 演算子](../oop/oo_polymorphism.md#is-operator)の自然な拡張になっているのが型パターン(type pattern)です。
以下のように、型の後ろに続けて、マッチした結果を変数で受け取れます。

<pre class="source" title="型パターンの例">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <em><span class="reserved">int</span> <span class="variable">i</span></em>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;int &quot;</span> + <span class="variable">i</span>);
    <span class="control">else</span> <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <em><span class="reserved">string</span> <span class="variable">s</span></em>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;string &quot;</span> + <span class="variable">s</span>);
}
</code></pre>

`is` や `case` の後ろで変数宣言をしているような形なので、宣言パターン(declaration pattern)とも呼びます。
(というか、C# 8.0以降は宣言パターンの方が正式な呼び方に変わっていそうです。)

型パターンは、旧来からある `is` 演算子や `as` 演算子とほぼ同じ挙動です。
上記の例は、概ね以下のコードと同じ動作になります。

<pre class="source" title="型パターンの挙動">
<code><span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">int</span>)
{
    <span class="reserved">var</span> <span class="variable">i</span> = (<span class="reserved">int</span>)<span class="variable">x</span>;
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;int &quot;</span> + <span class="variable">i</span>);
}
<span class="control">else</span>
{
    <span class="reserved">string</span> <span class="variable">s</span> = <span class="variable">x</span> <span class="reserved">as</span> <span class="reserved">string</span>;
    <span class="control">if</span> (<span class="variable">s</span> != <span class="reserved">null</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;string &quot;</span> + <span class="variable">s</span>);
    }
}
</code></pre>

`as` + `!= null` になっていることからわかる通り、
型パターンは null にはマッチしません。
(以下のように、たとえ変数の型が一致していたとしても、null にはマッチしません。)

<pre class="source" title="型パターンは null にはマッチしない">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
{
    <span class="method">M</span>(<span class="string">&quot;abc&quot;</span>); <span class="comment">// matched abc</span>
    <span class="method">M</span>(<span class="reserved">null</span>);  <span class="comment">// 何も表示されない</span>
}
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">x</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">string</span> <span class="variable">s</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;matched &quot;</span> + <span class="variable">s</span>);
}
</code></pre>

#### <a id="sec-generated-title-4"></a> <a id="simplified-type-pattern"></a>型パターンの簡単化

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 で型パターンがちょっとだけシンプルになりました。

型パターンは元々 C# 1.0 からある `is` 演算子の延長として作られています。
ところが、`is` の場合は `x is T` と書けるのに、`switch` では `T _` のように変数宣言か `_` (破棄) を伴う必要がありました。
これが C# 9.0 で改善されています。

<pre class="source" title="型パターンの簡単化">
<code><span class="reserved">int</span> <span class="method">Is</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">string</span>)
    {
        <span class="control">return</span> 1;
    }
    <span class="control">return</span> 0;
}
 
<span class="reserved">int</span> <span class="method">Switch</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">switch</span> (<span class="variable">x</span>)
    {
        <span class="comment">// C# 8.0 までは string _ と書く必要あり</span>
        <span class="control">case</span> <span class="reserved">string</span>: <span class="control">return</span> 1;
    }
    <span class="control">return</span> 0;
}
 
<span class="reserved">int</span> <span class="method">SwitchExpr</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="comment">// C# 8.0 までは string _ と書く必要あり</span>
    <span class="reserved">string</span> =&gt; 1,
    <span class="reserved">_</span> =&gt; 0,
};
</code></pre>

C# 9.0 時点でこれが書けたなかったのは次節の[定数パターン](#constant)との混同を避けるためです。
例えば C# 9.0 では以下のようなコードが書けます。
こんなコードを書くこと自体少ないと思いますが、`is`の場合と`switch`の場合で、型と定数、どちらが優先されるかが違うので注意が必要です。

<pre class="source" title="型パターンと定数パターンの区別">
<code><span class="reserved">class</span> <span class="type">X</span> { }
 
<span class="reserved">class</span> <span class="type">Program1</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
    {
        <span class="type">X</span> =&gt; 1, <span class="comment">// これは x の型がクラス X</span>
        <span class="reserved">_</span> =&gt; 0,
    };
}
 
<span class="reserved">class</span> <span class="type">Program2</span>
{
    <span class="reserved">const</span> <span class="reserved">int</span> X = 1;
 
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M1</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
    {
        X =&gt; 1, <span class="comment">// これは定数 1</span>
        <span class="reserved">_</span> =&gt; 0,
    };
 
    <span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">M2</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="type">X</span>; <span class="comment">// でもこれはクラス X (C# 8.0 以前との互換性のため)</span>
}
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="constant"></a>定数パターン

`is`や`case`の後ろには定数も書けます。これを定数パターン(constant pattern)と言います。
単体で見ると普通に `==` を使えば済むことも多いわけですが、
定数パターンであれば他のパターンとの混在ができます。

<pre class="source" title="定数パターンの例">
<code><span class="control">switch</span> (<span class="variable">x</span>)
{
    <span class="comment">// 定数パターン</span>
    <span class="control">case</span> 0: <span class="control">return</span> 0;
    <span class="comment">// 型パターン</span>
    <span class="control">case</span> <span class="reserved">string</span> <span class="variable">s</span>: <span class="control">return</span> <span class="variable">s</span>.Length;
    <span class="control">default</span>: <span class="control">return</span> -1;
}
</code></pre>

名前通り定数しか使えません。
変数との値比較がしたければ、`when`句を使うなどが必要です。

<pre class="source" title="文字通り、定数パターンは定数のみ受け付ける">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">comparand</span>)
{
    <span class="control">switch</span> (<span class="variable">x</span>)
    {
        <span class="comment">// case comparand: とは書けない。</span>
        <span class="comment">// 型パターン + when 句を使う。</span>
        <span class="control">case</span> <span class="reserved">int</span> <span class="variable">i</span> <span class="reserved">when</span> <span class="variable">i</span> == <span class="variable">comparand</span>: <span class="control">return</span> 0;
        <span class="control">default</span>: <span class="control">return</span> -1;
    }
}
</code></pre>

ちなみに、定数パターンでは、[ユーザー定義演算子](../oop/oo_operator.md#udo)を見ません。
以下のように、`==`と`is`で挙動が違う場合があります。

<pre class="source" title="定数パターンはユーザー定義の演算子を見ない">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// 全てのインスタンスが等しいという挙動。</span>
    <span class="comment">// 当然、x == null も常に true。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> ==(<span class="type">X</span> <span class="variable">a</span>, <span class="type">X</span> <span class="variable">b</span>) =&gt; <span class="reserved">true</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> !=(<span class="type">X</span> <span class="variable">a</span>, <span class="type">X</span> <span class="variable">b</span>) =&gt; <span class="reserved">false</span>;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">X</span>();
 
        <span class="comment">// なんでも true なので、== null も true</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="method">==</span> <span class="reserved">null</span>);
 
        <span class="comment">// ユーザー定義の == は見ない。x が本当に null かどうかを見て、false になる</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">null</span>);
    }
}
</code></pre>

#### <a id="sec-generated-title-6"></a> <a id="pointer-null"></a>ポインターの null 比較

<h5 class="version version8">Ver. 8.0</h5>

細かい修正ですが、C# 8.0 からポインターに対してもパターン マッチングが使えるようになりました。
といってもプロパティや `Deconstruct` メソッドを持っているわけではないので、実質的には `is null` チェック用です。

<pre class="source" title="">
<code><span class="reserved">static</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>* <span class="variable">p</span>)
{
    <span class="comment">// 元々 OK。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">p</span> == <span class="reserved">null</span>);
 
    <span class="comment">// C# 8.0 から OK。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">p</span> <span class="reserved">is</span> <span class="reserved">null</span>);
}
</code></pre>

#### <a id="sec-generated-title-7"></a> <a id="span">ReadOnlySpan に対するパターンマッチ</a>

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`ReadOnlySpan<char>` に対して文字列リテラルによる定数パターンが使えるようになりました。

<pre class="source" title="">
<span class="comment">// string を渡せたところには ReadOnlySpan&lt;char&gt; を渡せるように。</span>
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">ReadLine</span></span>();

<span class="comment">// is も</span>
<span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="string">&quot;a&quot;</span>) { }

<span class="comment">// switch ステートメントも</span>
<span class="control">switch</span> (<span class="variable">s</span>)
{
    <span class="control">case</span> <span class="string">&quot;b&quot;</span>:
        <span class="control">break</span>;
}

<span class="comment">// switch 式も OK。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="variable">s</span> <span class="control">switch</span>
{
    <span class="string">&quot;c&quot;</span> <span class="operator">=&gt;</span> <span class="number">1</span>,
    <span class="reserved">_</span> <span class="operator">=&gt;</span> <span class="number">2</span>,
};
</pre>

文字列処理に対して `ReadOnlySpan<char>` を使う機会が多くなってきたので特殊対応したそうです。

(パターンに書かれているのは `""` みたいな「定数」ですが、
そこに `string` から `ReadOnlySpan<char>` の変換が挟まっていて定数とは言い切れない状態です。
C# チーム自身はそれほど実装に乗り気ではなく、外部からのコントリビューションで実装された機能になります。)

### <a id="sec-generated-title-8"></a> <a id="var"></a>var パターン

型パターンと似ていますが、具体的な型名の代わりに `var` キーワードを使うと、
任意の型にマッチするパターンになります。
これを var パターン (var pattern)と言います。

`switch` の最後に書いて「その他全部」な分岐に使ったりします。

<pre class="source" title="var パターンを「その他全部」の意味で使う例">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">switch</span>(<span class="variable">x</span>)
    {
        <span class="control">case</span> 0: <span class="control">return</span> 0;
        <span class="control">case</span> <span class="reserved">string</span> <span class="variable">s</span>: <span class="control">return</span> <span class="variable">s</span>.Length;
        <span class="control">case</span> <em><span class="reserved">var</span> other</em>: <span class="control">return</span> <span class="variable">other</span>.<span class="method">GetHashCode</span>();
        <span class="comment">// あるいは、変数で受け取る必要がないときは _ にしておけば破棄の意味なる</span>
        <span class="comment">// case var _:</span>
    }
}
</code></pre>

あと、少し悪用気味ではありますが、式中での変数宣言に使えたりします。

<pre class="source" title="式中での変数宣言代わりに var パターンを利用">
<code><span class="control">while</span> (<span class="type">Console</span>.<span class="method">ReadLine</span>() <span class="reserved">is</span> <em><span class="reserved">var</span> line</em> &amp;&amp; !<span class="reserved">string</span>.<span class="method">IsNullOrEmpty</span>(<span class="variable">line</span>))
{
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">line</span>);
}
</code></pre>

1つ注意が必要な点として、var パターンは型パターンと違って、null にもマッチします。

<pre class="source" title="var パターンは null にもマッチ">
<code><span class="reserved">string</span> <span class="variable">s</span> = <span class="reserved">null</span>;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">string</span> <span class="variable">x</span>); <span class="comment">// false</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">var</span> y);    <span class="comment">// true</span>
</code></pre>

null をはじきたい場合は、var ではなく、後述するプロパティ パターンを使って`x is {} nonNull`と書いたりします。

### <a id="sec-generated-title-9"></a> <a id="discards"></a><a id="discard"></a>破棄パターン

<h5 class="version version8">Ver. 8.0</h5>

何にでもマッチして、マッチ結果を受け取る必要がない場合、`_` を使って値を破棄できます。これを破棄パターン(discard pattern)と言います。

再帰はしないんですが、`switch`式の中と、再帰パターン内でしか使えないので C# 8.0 での実装になります。
`is`やステートメントの方の`switch`の`case`の後ろでは`var _`と書く必要がありますが、`switch`式の場合は`_`だけで値を破棄します。

<pre class="source" title="switch 式中では、_ だけで値を破棄できる">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
    =&gt; <span class="variable">x</span> <span class="reserved">switch</span>
    {
        0 =&gt; 0,
        <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length,
        <em><span class="reserved">_</span></em> =&gt; -1
    };
</code></pre>

### <a id="sec-generated-title-10"></a> <a id="breaking-change-in-discard"></a>余談: 破棄パターンが C# 8.0 からな理由

ちなみに、`is` や `switch`ステートメント内で `_` だけでの値の破棄ができないのは既存コードとの互換性のためです。
普通書かないようなコードですが、一応、以下のようなコードが元々合法なため、意味を変えることができませんでした。

<pre class="source" title="_ クラス、 _ 定数">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">_Type</span>
{
    <span class="reserved">class</span> <span class="type">_</span> { }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> <span class="type">_</span>); <span class="comment">// class _ とのマッチ</span>
    }
}
 
<span class="reserved">class</span> <span class="type">_Constant</span>
{
    <span class="reserved">const</span> <span class="reserved">int</span> _ = 0;
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
    {
        <span class="control">switch</span> (<span class="variable">x</span>)
        {
            <span class="control">case</span> _: <span class="comment">// 定数 _ とのマッチ</span>
                <span class="control">break</span>;
        }
    }
}
</code></pre>

(あまりにも紛らわしいので、このコードを C# 8.0 でコンパイルすると警告が出ます。)

<!-- original-page-break -->

## <a id="sec-generated-title-11"></a> <a id="recursive"></a>再帰パターン

<h5 class="version version8">Ver. 8.0</h5>

C# 7.0 の範囲で使えるものは、「パターン」と呼ぶのが仰々しいくらい単純なものでした。
C# 8.0 で、再帰的に使えるパターンが追加されて、ようやくパターン マッチングらしくなりました。

例えば以下のような感じです。

<pre class="source" title="再帰パターンの例">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="method">Point</span>(<span class="reserved">int</span> <span class="variable">x</span> = 0, <span class="reserved">int</span> <span class="variable">y</span> = 0) =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) = (X, Y);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>)
        =&gt; <span class="variable">obj</span> <span class="reserved">switch</span>
    {
        0 =&gt; 1,
        <span class="reserved">int</span> <span class="variable">i</span> =&gt; 2,
        <em><span class="type">Point</span> (1, <span class="reserved">_</span>)</em> =&gt; 4, <span class="comment">// new!</span>
        <em><span class="type">Point</span> { X: 2, Y: <span class="reserved">var</span> y }</em> =&gt; <span class="variable">y</span>, <span class="comment">// new!</span>
        <span class="reserved">_</span> =&gt; 0
    };
}
</code></pre>

### <a id="sec-generated-title-12"></a> <a id="positional"></a>位置パターン

位置パターン (positional pattern)は、
[分解](deconstruction.md)と同じ要領で再帰的なマッチングを行うパターンです。

分解と同様、`Deconstruct`メソッドを呼んでメンバーを取り出した上で、
それぞれのメンバーの値に対してマッチングを行います。
例えば、先ほど例として使った`Point`クラスを引き続き使うとして、以下のように書けます。

<pre class="source" title="位置パターンの例">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">Point</span> <span class="variable">p</span>)
    =&gt; <span class="variable">p</span> <span class="reserved">switch</span>
{
    (1, 2) =&gt; 0,
    (<span class="reserved">var</span> x, <span class="reserved">_</span>) <span class="reserved">when</span> x &gt; 0 =&gt; <span class="variable">x</span>,
    <span class="reserved">_</span> =&gt; -1
};
</code></pre>

このコードは概ね以下のような意味になります。

<pre class="source" title="位置パターンの展開結果">
<code><span class="variable">p</span>.<span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">y</span>);
<span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> 1 &amp;&amp; <span class="variable">y</span> <span class="reserved">is</span> 2) <span class="control">return</span> 0;
<span class="control">if</span> (<span class="variable">x</span> &gt; 0) <span class="control">return</span> <span class="variable">x</span>;
<span class="control">return</span> -1;
</code></pre>

サブパターンの順序に意味があるため「位置」パターンという呼び名になっています。

上記の例では元々型が`Point`だとわかっているので型名を省略していますが、
型の明示もできます。

<pre class="source" title="位置パターンでの型の明示">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>)
    =&gt; <span class="variable">obj</span> <span class="reserved">switch</span>
{
    <span class="reserved">int</span> <span class="variable">i</span> =&gt; <span class="variable">i</span>,
    <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length,
    <em><span class="type">Point</span>(<span class="reserved">var</span> x, <span class="reserved">var</span> y)</em> =&gt; 0,
    <span class="reserved">_</span> =&gt; -1
};
</code></pre>

また、後述しますが、プロパティ パターンとの混在や、
型パターンのように変数を付け足すこともできます。

<pre class="source" title="位置パターン、プロパティ パターン、型パターンの組み合わせ">
<code><span class="variable">obj</span> <span class="reserved">switch</span>
{
    <span class="type">Point</span> (<span class="reserved">var</span> x, <span class="reserved">_</span>) { Y: <span class="reserved">var</span> y } p =&gt; <span class="variable">x</span> * <span class="variable">y</span>
};
</code></pre>

位置パターンとか言いつつ、名前付き引数のノリで、名前付きなパターン マッチングもできます。

<pre class="source" title="名前付き位置パターン">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">NamedPattern</span>(<span class="type">Point</span> <span class="variable">p</span>)
    =&gt; <span class="variable">p</span> <span class="reserved">switch</span>
{
    (<em><span class="variable">x</span>:</em> 1, <em><span class="variable">y</span>:</em> 2) =&gt; 0,
    (<em><span class="variable">x</span>:</em> <span class="reserved">var</span> x, <em><span class="variable">y</span>:</em> <span class="reserved">_</span>) <span class="reserved">when</span> <span class="variable">x</span> &gt; 0 =&gt; <span class="variable">x</span>,
    <span class="reserved">_</span> =&gt; -1
};
</code></pre>

#### <a id="sec-generated-title-13"></a> <a id="constructor-vs-positional"></a>補足: コンストラクター呼び出しの逆

位置パターンは、コンストラクター呼び出し(`new`)の逆に当たる構文です。
書き方も、コンストラクターと対になっています。

<pre class="source" title="コンストラクター呼び出しと位置パターン">
<code><span class="comment">// 位置指定で構築できるんなら、位置指定でマッチングできるべき</span>
<span class="reserved">var</span> <span class="variable">p1</span> = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
<span class="reserved">var</span> <span class="variable">r1</span> = <span class="variable">p1</span> <span class="reserved">is</span> <span class="type">Point</span> (1, 2);
 
<span class="comment">// 名前指定で構築できるんなら、名前指定でマッチングできるべき</span>
<span class="reserved">var</span> <span class="variable">p2</span> = <span class="reserved">new</span> <span class="type">Point</span>(<span class="variable">x</span>: 1, <span class="variable">y</span>: 2);
<span class="reserved">var</span> <span class="variable">r2</span> = <span class="variable">p2</span> <span class="reserved">is</span> <span class="type">Point</span> (<span class="variable">x</span>: 1, <span class="variable">y</span>: 2);
 
<span class="comment">// 型推論が効く場合に new の後ろの型名は省略可能(になる予定)なら</span>
<span class="comment">// 型が既知なら型名を省略してマッチングできるべき</span>
<span class="type">Point</span> <span class="variable">p3</span> = <span class="reserved">new</span> (1, 2);
<span class="reserved">var</span> <span class="variable">r3</span> = <span class="variable">p3</span> <span class="reserved">is</span> (1, 2);
 
<span class="comment">// 階層的に new できるんなら、階層的にマッチングできるべき</span>
<span class="reserved">var</span> <span class="variable">line</span> = <span class="reserved">new</span> <span class="type">Line</span>(<span class="reserved">new</span> <span class="type">Point</span>(1, 2), <span class="reserved">new</span> <span class="type">Point</span>(3, 4));
<span class="reserved">var</span> <span class="variable">r4</span> = <span class="variable">line</span> <span class="reserved">is</span> ((1, 2), (3, 4));
</code></pre>

#### <a id="sec-generated-title-14"></a> <a id="how-to-deconstruct"></a>分解方法

位置パターンは [分解](deconstruction.md)と同じ要領でメンバーの値を取り出します。
分解もそうなんですが、[タプル](tuples.md)(C# のタプル構文を使って作る `ValueTuple` 構造体の値)の場合とそうでない場合で内部的な挙動が少し変わります。

まず、タプルの場合、コンパイラーの最適化によって、タプルのフィールドを直接参照するようなコードが生成されます。
例えば以下のようなコードを書いた場合、

<pre class="source" title="タプルに対する位置パターン">
<code><span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">TupleSyntax</span>((<span class="reserved">int</span> a, <span class="reserved">int</span> b) <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> (1, 2);
</code></pre>

以下のようなコードと同じような挙動をします。

<pre class="source" title="タプルに対する位置パターンの展開結果">
<code><span class="comment">// ValueTuple の場合は直接フィールドを参照する。</span>
<span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">TupleSyntax</span>((<span class="reserved">int</span> a, <span class="reserved">int</span> b) <span class="variable">x</span>)
{
    <span class="control">return</span> <span class="variable">x</span>.a == 1 &amp;&amp; <span class="variable">x</span>.b == 2;
}
</code></pre>

そうでない場合、まずはコンパイル時に `Deconstruct` メソッドを探します。
見つかった場合は、それを使うコードが生成されます。
例として以下のようなクラスを用意します。

<pre class="source" title="分解可能なクラス">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">class</span> <span class="type">X</span> : <span class="type">ITuple</span>
{
    <span class="reserved">public</span> <span class="reserved">object</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable">index</span>] =&gt; <span class="variable">index</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> Length =&gt; 2;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">a</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">b</span>) =&gt; (<span class="variable">a</span>, <span class="variable">b</span>) = (0, 1);
}
</code></pre>

この型に対して以下のようなコードを書いた場合、

<pre class="source" title="コンパイル時に Deconstruct メソッドが見つかる場合">
<code><span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Deconstruct</span>(<span class="type">X</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> (1, 2);
</code></pre>

以下のようなコードと同じような挙動をします。

<pre class="source" title="コンパイル時に Deconstruct メソッドが見つかる場合の展開結果">
<code><span class="comment">// コンパイル時に Deconstruct メソッドが見つかる場合はそれを使って分解。</span>
<span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Deconstruct</span>(<span class="type">X</span> <span class="variable">x</span>)
{
    <span class="variable">x</span>.<span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">a</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">b</span>);
    <span class="control">return</span> <span class="variable">a</span> == 1 &amp;&amp; <span class="variable">b</span> == 2;
}
</code></pre>

分解代入や分解変数宣言とは違って、位置パターンの場合はコンパイル時に `Deconstruct` メソッドが見つからない場合があります。
この場合、`ITuple`インターフェイス(`System.Runtime.CompilerServices`名前空間)を使って分解を試みます。
例えば以下のように`object`で値を渡すコードを書いた場合、

<pre class="source" title="コンパイル時に Deconstruct メソッドが見つからない場合">
<code><span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Object</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> (1, 2);
</code></pre>

以下のようなコードと同じような挙動をします。

<pre class="source" title="コンパイル時に Deconstruct メソッドが見つからない場合の展開結果">
<code><span class="comment">// コンパイル時の解決ができない場合、ITuple を実装しているかどうかを見る。</span>
<span class="comment">// Length とインデクサーを使ってマッチング。</span>
<span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Object</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">return</span> <span class="variable">x</span> <span class="reserved">is</span> <span class="type">ITuple</span> <span class="variable">t</span>
        &amp;&amp; <span class="variable">t</span>.Length == 2
        &amp;&amp; <span class="variable">t</span>[0] <span class="reserved">is</span> <span class="reserved">int</span> <span class="variable">a</span> &amp;&amp; <span class="variable">a</span> == 1
        &amp;&amp; <span class="variable">t</span>[1] <span class="reserved">is</span> <span class="reserved">int</span> <span class="variable">b</span> &amp;&amp; <span class="variable">b</span> == 1
        ;
}
</code></pre>

#### <a id="sec-generated-title-15"></a> <a id="tuple-switch"></a>タプル switch

位置パターンに伴って、`switch`ステートメントの `()` の中に、複数の値を `,` 区切りで書けるようになりました。

<pre class="source" title="複数の値に対する switch">
<code><span class="reserved">int</span> <span class="method">Compare</span>(<span class="reserved">int</span>? <span class="variable">a</span>, <span class="reserved">int</span>? <span class="variable">b</span>)
{
    <em><span class="control">switch</span> (<span class="variable">a</span>, <span class="variable">b</span>)</em>
    {
        <span class="control">case</span> (<span class="reserved">null</span>, <span class="reserved">null</span>): <span class="control">return</span> 0;
        <span class="control">case</span> (<span class="reserved">int</span> <span class="reserved">_</span>, <span class="reserved">null</span>): <span class="control">return</span> -1;
        <span class="control">case</span> (<span class="reserved">null</span>, <span class="reserved">int</span> <span class="reserved">_</span>): <span class="control">return</span> -1;
        <span class="control">case</span> (<span class="reserved">int</span> <span class="variable">a1</span>, <span class="reserved">int</span> <span class="variable">b1</span>): <span class="control">return</span> <span class="variable">a1</span>.<span class="method">CompareTo</span>(<span class="variable">b1</span>);
    }
}
</code></pre>

このコードは、まず `(a, b)` というタプルを作って、それを `switch` ステートメントに掛ける挙動になります。`case` の後ろに書かれているのは位置パターンです。

要するに、意味としては `switch ((a, b))` と書くのと同じです。
なので実体としては「複数の値に対する`switch`」というより、「タプルに限り、`()` を一段省略できる」という機能です。

#### <a id="sec-generated-title-16"></a> <a id="zero-or-one"></a>0、1要素の分解

タプル構築や分解代入・分解宣言では0、1要素のもの( `()` や `(x)`) は認められていませんが、
位置パターンでは認められるようになりました。
それぞれ、0、1引数の`Deconstruct`メソッドが調べられます。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>() { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">a</span>) =&gt; <span class="variable">a</span> = 0;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>() =&gt; <span class="method">M</span>(<span class="reserved">new</span> <span class="type">X</span>());
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type">X</span> <span class="variable">x</span>)
    {
        <span class="comment">// 0 引数の位置パターン。</span>
        <span class="comment">// Deconstruct() を持っていることが使える条件。</span>
        <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> ()) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Deconstruct()&quot;</span>);
 
        <span class="comment">// 1 引数の位置パターン。</span>
        <span class="comment">// Deconstruct(out T) を持っていることが使える条件。</span>
        <span class="comment">// ただ、キャストの () との区別が難しいらしく、素直に x is (int a) とは書けない。</span>
        <span class="comment">// 前後に余計な var や _ を付ける必要あり。</span>
        <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> (<span class="reserved">int</span> <span class="variable">a</span>) <span class="reserved">_</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;Deconstruct(</span>{<span class="variable">a</span>}<span class="string">)&quot;</span>);
    }
}
</code></pre>

0引数のものは単に `()` で OK です。

一方で、1引数のものは、キャストの `()` との区別が難しいそうで、
素直に `(constant)` とか `(T variable)` とかは書けません。
`var (subpattern)` とか `(subpattern) _` とか、前後に余計なものを付けることでキャストと区別します。

### <a id="sec-generated-title-17"></a> <a id="remove-deconstruct"></a>最適化での Deconstruct 削除

位置パターンでは、コンパイラーの最適化によって `Deconstruct` メソッドの呼び出しが消えることがあります。
以下のように、すべて `_` で値を破棄してしまう場合には `Deconstruct` メソッドを呼び出す必要がなく、
実際、呼び出しが消えてなくなります。

<pre class="source" title="Deconstruct が常に呼ばれる保証はない">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// Deconstruct に副作用を持たせる</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Deconstruct()&quot;</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">a</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Deconstruct(out int a)&quot;</span>);
        <span class="variable">a</span> = 0;
    }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">a</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">b</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Deconstruct(out int a, out int b)&quot;</span>);
        (<span class="variable">a</span>, <span class="variable">b</span>) = (0, 0);
    }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">X</span>();
 
        <span class="comment">// Deconstruct() がないとコンパイル エラーになるけど、</span>
        <span class="comment">// Deconstruct() は呼ばれない。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> ());
 
        <span class="comment">// Deconstruct(out int) がないとコンパイル エラーになるけど、</span>
        <span class="comment">// Deconstruct(out int) は呼ばれない。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">var</span> (<span class="reserved">_</span>));
 
        <span class="comment">// Deconstruct(out int, out int) がないとコンパイル エラーになるけど、</span>
        <span class="comment">// Deconstruct(out int, out int) は呼ばれない。</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> (<span class="reserved">_</span>, <span class="reserved">_</span>));
    }
}
</code></pre>

また、引数の数が同じ位置パターンをいくつか並べた際にも、`Deconstruct` メソッドの呼び出しは1回にまとめられます。

<pre class="source" title="引数の数が同じ位置パターンを並べる例">
<code><span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="method">X</span>(<span class="reserved">int</span> <span class="variable">value</span>) =&gt; Value = <span class="variable">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">value</span>) =&gt; <span class="variable">value</span> = Value;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">X</span> <span class="variable">x</span>)
        =&gt; <span class="variable">x</span> <span class="reserved">switch</span>
    {
        <span class="comment">// 引数の数が同じ位置パターンを3回。</span>
        <span class="comment">// この場合、Deconstruct(out int) の呼び出しは1回にまとめられる。</span>
        (0) <span class="reserved">_</span> =&gt; 1,
        (1) <span class="reserved">_</span> =&gt; 2,
        (2) <span class="reserved">_</span> =&gt; 0,
        <span class="reserved">_</span> =&gt; <span class="variable">x</span>.Value
    };
}
</code></pre>

ちなみに、仕様上は「必ず消える」という保証もないです(「消えることがある」という仕様)。
なので、`Deconstruct` メソッドは副作用を起こさないように作ることが推奨されます。

### <a id="sec-generated-title-18"></a> <a id="property"></a>プロパティ パターン

プロパティ パターン(property pattern)は、プロパティに対して再帰的なマッチングを行うパターンです。
(プロパティ パターンという名前に反して、フィールドも使えます。)

書き方は、`{ PropertyName: SubPattern, ... }` というように、
プロパティ名と、そのプロパティに対して掛けたいパターンを `:` でつなぎます。
複数のプロパティに対して使う場合はそれぞれを `,` で区切ります。
位置パターンとは違って、名前の省略はできません。

再び `Point` クラス(`int` 型の2つのプロパティ `X`、`Y` を持つ)を例に挙げます。
以下のような書き方ができます。

<pre class="source" title="プロパティ パターンの例">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">Point</span> <span class="variable">p</span>)
    =&gt; <span class="variable">p</span> <span class="reserved">switch</span>
{
    { X: 1, Y: 2 } =&gt; 0,
    { X: <span class="reserved">var</span> x, Y: <span class="reserved">_</span> } <span class="reserved">when</span> <span class="variable">x</span> &gt; 0 =&gt; <span class="variable">x</span>,
    <span class="reserved">_</span> =&gt; -1
};
</code></pre>

このコードは概ね以下のような意味になります。

<pre class="source" title="プロパティ パターンの展開結果">
<code><span class="reserved">var</span> <span class="variable">x</span> = <span class="variable">p</span>.X;
<span class="reserved">var</span> <span class="variable">y</span> = <span class="variable">p</span>.Y;
<span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> 1 &amp;&amp; <span class="variable">y</span> <span class="reserved">is</span> 2) <span class="control">return</span> 0;
<span class="control">if</span> (<span class="variable">x</span> &gt; 0) <span class="control">return</span> <span class="variable">x</span>;
<span class="control">return</span> -1;
</code></pre>

位置パターンと同様、型の明示もできます。

<pre class="source" title="プロパティ パターンでの型の明示">
<code><span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>)
    =&gt; <span class="variable">obj</span> <span class="reserved">switch</span>
{
    <span class="reserved">int</span> <span class="variable">i</span> =&gt; <span class="variable">i</span>,
    <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length,
    <em><span class="type">Point</span> { X: 0, Y: 0 }</em> =&gt; 0,
    <span class="type">Point</span> (<span class="reserved">_</span>, <span class="reserved">_</span>) =&gt; 1,
    <span class="reserved">_</span> =&gt; -1
};
</code></pre>

ちなみに、プロパティ パターンと言いつつ、フィールドも参照できます。

<pre class="source" title="プロパティ パターンでフィールドを参照する例">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// (外から見て) get-only なプロパティ</span>
    <span class="reserved">public</span> <span class="reserved">int</span> GetOnly { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }
 
    <span class="comment">// get/set 可能なプロパティ</span>
    <span class="reserved">public</span> <span class="reserved">int</span> GetSet { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="comment">// フィールド</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Field;
 
    <span class="comment">// set-only なプロパティ</span>
    <span class="reserved">public</span> <span class="reserved">int</span> SetOnly { <span class="reserved">set</span> =&gt; GetOnly = <span class="reserved">value</span>; }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// オブジェクト初期化子では、set が public なプロパティか readonly ではないフィールドを指定可能</span>
        <span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span> <span class="type">X</span> { GetSet = 1, Field = 2, SetOnly = 3 };
 
        <span class="comment">// プロパティ パターンでは、get が public なプロパティかフィールドを指定可能</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">x</span> <span class="reserved">is</span> { GetOnly: 3, GetSet: 1, Field: 2 });
    }
}
</code></pre>

#### <a id="sec-generated-title-19"></a> <a id="initializer-vs-property"></a>オブジェクト初期化子の逆

「位置パターンはコンストラクター呼び出しの逆」という話をしましたが、
同様に、プロパティ パターンは[オブジェクト初期化子](../oop/oo_construct.md#member_initializer)と対になるものです。

<pre class="source" title="オブジェクト初期化子とプロパティ パターン">
<code><span class="comment">// 初期化子でプロパティ指定できるんなら、プロパティ指定でマッチングできるべき</span>
<span class="reserved">var</span> <span class="variable">p1</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
<span class="reserved">var</span> <span class="variable">r1</span> = <span class="variable">p1</span> <span class="reserved">is</span> { X: 1, Y: 2 };
 
<span class="comment">// 混在で構築できるんなら、混在でマッチングできるべき</span>
<span class="reserved">var</span> <span class="variable">p2</span> = <span class="reserved">new</span> <span class="type">Point</span>(<span class="variable">x</span>: 1) { Y = 2 };
<span class="reserved">var</span> <span class="variable">r2</span> = <span class="variable">p2</span> <span class="reserved">is</span> (1, <span class="reserved">_</span>) { Y: 2 };
</code></pre>

ただ、`=` は代入の意味なのでパターンとしては使えず、代わりに `:` になっています。
`:` を使っているのは、位置パターンと構文を共通化できて実装が楽だからだそうです。

#### <a id="sec-generated-title-20"></a> <a id="no-order"></a>位置パターンとプロパティ パターンの順序

位置パターンとプロパティ パターンを混在して使う場合、
`Deconstruct`メソッドとプロパティのアクセサーの呼び出し順序には<em>保証がない</em>そうです。

残念ながら、以下のようなコードには動作保証がないそうです。

<pre class="source" title="順序保障がないとまずいコードの例">
<code><span class="reserved">using</span> System;
 
<span class="reserved">enum</span> <span class="type">Type</span> { A, B }
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="type">Type</span> Type { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="method">X</span>(<span class="type">Type</span> <span class="variable">type</span>) =&gt; Type = <span class="variable">type</span>;
 
    <span class="comment">// それぞれ Type が一致しているときだけ値を取り出せ、そうでなければ例外</span>
    <span class="reserved">public</span> <span class="reserved">int</span> A =&gt; Type == <span class="type">Type</span>.A ? 1 : <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
    <span class="reserved">public</span> <span class="reserved">int</span> B =&gt; Type == <span class="type">Type</span>.B ? 2 : <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
 
    <span class="comment">// 分解でタイプ判定</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="type">Type</span> <span class="variable">t</span>) =&gt; <span class="variable">t</span> = Type;
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>(<span class="reserved">new</span> <span class="type">X</span>(<span class="type">Type</span>.A)));
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">M</span>(<span class="reserved">new</span> <span class="type">X</span>(<span class="type">Type</span>.B)));
    }
 
    <span class="comment">// 以下のコードはたまたま動く可能性はあるものの、C# の言語使用としては保証がない。</span>
    <span class="comment">// Deconstruct よりも先にプロパティのアクセスがあると例外が出ることがある。</span>
    <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">M</span>(<span class="type">X</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">switch</span>
    {
        (<span class="type">Type</span>.A) { A: <span class="reserved">var</span> a } =&gt; <span class="variable">a</span>,
        (<span class="type">Type</span>.B) { B: <span class="reserved">var</span> b } =&gt; <span class="variable">b</span>,
        <span class="reserved">_</span> =&gt; 0
    };
}
</code></pre>

#### <a id="sec-generated-title-21"></a> <a id="non-null"></a>非 null マッチング

プロパティ パターンは、暗黙的にnullチェックが挟まって、非 null であることが保証されます。
しかも、`x is { }` というように、中身が空っぽであっても null チェックだけは挿入されるので、 `x is { }`を「`x`はnullではない」の意味で使えます。

C# 7.0 までのパターンだと、null チェックを楽に書く手段がなかったです。

<pre class="source" title="C# 7.0 時点のパターンでの非 null パターン">
<code><span class="reserved">struct</span> <span class="type">LongLongNamedStruct</span> { }
 
<span class="reserved">void</span> <span class="method">M1</span>(<span class="type">LongLongNamedStruct</span>? <span class="variable">x</span>)
{
    <span class="comment">// こういう書き方だと null チェックになる。</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="type">LongLongNamedStruct</span> <span class="variable">nonNull</span>)
    {
        <span class="comment">// obj が null じゃない時だけここが実行される。</span>
        <span class="comment">// でも、x の型が既知なのに、長いクラス名をわざわざ書くのはしんどい…</span>
    }
}
 
<span class="reserved">void</span> <span class="method">M2</span>(<span class="type">LongLongNamedStruct</span>? <span class="variable">x</span>)
{
    <span class="comment">// が、var パターンは null にもマッチしちゃう。</span>
    <span class="comment">// (var は「何にでもマッチ」。null でも true になっちゃう。)</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">var</span> nullable)
    {
        <span class="comment">// obj が null でもここが実行される。</span>
    }
}
</code></pre>

単に null チェックだけなら `!(x is null)` とか `x.HasValue` だけでいいんですけども、 値を使いたければその後ろで `var nonNull = x.GetValueOrDefault();` とかが必要で、何を使うにしても微妙に長くなりがちでした。

そこで先ほどの `x is { }` を使います。
以下のような書き方で、null 許容型の null チェックをしつつ、値を変数に受け取れます。

<pre class="source" title="C# 8.0 での非 null パターン">
<code><span class="reserved">void</span> <span class="method">M3</span>(<span class="type">LongLongNamedStruct</span>? <span class="variable">x</span>)
{
    <span class="comment">// (C# 8.0) プロパティ パターンであれば、null チェックを含む。</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> { } nonNull)
    {
        <span class="comment">// obj が null じゃない時だけここが実行される。</span>
    }
}
</code></pre>

#### <a id="sec-generated-title-22"></a> <a id="sub-pattern-name"></a>プロパティ パターンの拡張(入れ子のメンバー参照)

<h5 class="version version10">Ver. 10</h5>

C# 10.0 で、以下のように、入れ子のプロパティ・フィールド参照でプロパティ パターンを書けるようになりました。

<pre class="source" title="入れ子のプロパティ参照">
<code><span class="method">m</span>(<span class="reserved">null</span>);
<span class="method">m</span>(<span class="reserved">new</span> <span class="type">X</span> { Name = <span class="string">""</span> });
<span class="method">m</span>(<span class="reserved">new</span> <span class="type">X</span> { Name = <span class="string">"a"</span> });
<span class="method">m</span>(<span class="reserved">new</span> <span class="type">X</span> { Name = <span class="string">"abc"</span> });

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m</span>(<span class="type">X</span>? x)
{
    <span class="control">if</span> (x <span class="reserved">is</span> { <em>Name.Length: 1</em> })
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"single-char Name"</span>);
    }
}

<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span>? Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

この例でいう `{ Name.Length: 1 }` の部分は、`{ Name: { Length: 1 } }` と全く同じ意味になります。

ここで注意点というか、1つ、一瞬迷いそうな点として、
`Name.Length` と言う書き方でも `Name` の null チェックを含んでいます。
`{ Name: { Length: 1 } }` をさらに展開すると、以下のようなコードとほぼ同じ意味になります。

<pre class="source" title="入れ子のプロパティ パターンは null チェックを含む">
<code>    <span class="control">if</span> (x <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
    {
        <span class="reserved">var</span> name = x.Name;
        <span class="control">if</span> (name <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
        {
            <span class="reserved">var</span> length = name.Length;
            <span class="control">if</span> (length == 1)
            {
                <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">"single-char Name"</span>);
            }
        }
    }
</code></pre>

### <a id="sec-generated-title-23"></a> <a id="list">リスト パターン</a>

<h5 class="version version11">Ver. 11</h5>

C# 11で、`[]` を使ってリスト(配列や `List<T>` など)に対するパターン マッチングができるようになりました。
例えば以下のような `switch` を書けます。

<pre class="source" title="リストパターンの例">
<code><span class="reserved">var</span> <span class="variable">array</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> };

<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">array</span> <span class="control">switch</span>
{
    [] <span class="operator">=&gt;</span> <span class="string">&quot;空の配列&quot;</span>,
    [<span class="number">1</span>] <span class="operator">=&gt;</span> <span class="string">&quot;長さ1で、1要素目が1&quot;</span>,
    [<span class="reserved">_</span>] <span class="operator">=&gt;</span> <span class="string">&quot;長さ1の配列&quot;</span>,
    [<span class="number">1</span>, <span class="number">2</span>] <span class="operator">=&gt;</span> <span class="string">&quot;長さ2で、1要素目が1、2要素目が2&quot;</span>,
    [<span class="number">1</span>, <span class="reserved">_</span>] <span class="operator">=&gt;</span> <span class="string">&quot;長さ2で、1要素目が1&quot;</span>,
});
</code></pre>

このような `[]` を使ったパターンを<strong id="key-list-pattern" class="keyword">リスト パターン</strong>(list pattern)と言います。

#### <a id="sec-generated-title-24"></a> <a id="square-bracket">注意: 角カッコ</a>

C# で新文法を追加する際には、既存の文法と比べて違和感がないような選択をすることが多いです。

そういう意味ではリスト パターンの `[]` は珍しくちょっと見慣れない感じの選択でした。
これまで `[]` を使う文法というと、配列作成の `new T[N]` か、インデクサーの `x[i]` な分けですが、
これらはの場合 `[]` の内側には「個数」や「何番目か」の数値が入ります。
リスト パターンの `[]` の中に入るのは「要素に対するパターン」で、ちょっと方針が異なります。

初期案では、配列初期化子 `new[] { a, b, c }` からの類推ができるよう、リスト パターンには `{}` を使おうかという話もありました。
ただ、`is {}` だと[プロパティ パターン](#property)との弁別が難しかったようです。

これに対して、(C# 11 では入らなかったんですが、将来) 「コレクション リテラル」みたいな文法で `[]` を使う事を考えたりもしているようです。

<pre class="source" title="[] でコレクション初期化">
<code><span class="comment">// (C# 11 時点で提案段階)</span>
<span class="reserved">using</span> System.Collections.Immutable;

<span class="reserved">int</span>[] <span class="variable">array</span> = [ 1, 2 ];
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">span</span> = [ 1, 2 ];
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">ros</span> = [ 1, 2 ];
<span class="type">List</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">list</span> = [ 1, 2 ];
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">immutable</span> = [1, 2];
</code></pre>

これが入れば、初期化・生成側と、パターン マッチ・分解側の間の違和感が緩和されるかと思います。

#### <a id="sec-generated-title-25"></a> <a id="slice-pattern">.. (スライス パターン)</a>

パターンに対して `[a, b]` と書く場合、2要素ピッタリのリスト出ないとマッチしません。

<pre class="source" title="個数がピッタリでないとマッチしない">
<code><span class="reserved">var</span> <span class="variable">array</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> };

<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [<span class="number">1</span>, <span class="number">2</span>]); <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [<span class="number">1</span>]);    <span class="comment">// false。部分一致ではダメ。</span>
</code></pre>

部分一致させたい場合、余る部分に `..` を置けばマッチさせることができます。
例えば、以下のようなコードで、「1, 2 で始まって、長さ2以上のリスト」にマッチできます。

<pre class="source" title="「1, 2 で始まって、長さ2以上のリスト」にマッチ">
<code><span class="reserved">var</span> <span class="variable">array</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> };

<span class="method">match</span>(<span class="reserved">new</span>[] { <span class="number">1</span> }); <span class="comment">// false</span>
<span class="method">match</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> }); <span class="comment">// true (ちょうどでもOK)</span>
<span class="method">match</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> }); <span class="comment">// true (過剰でもOK)</span>
<span class="method">match</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> }); <span class="comment">// true</span>

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">match</span>(<span class="reserved">int</span>[] <span class="variable local">array</span>)
    <span class="operator">=&gt;</span> <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable local">array</span> <span class="reserved">is</span> [<span class="number">1</span>, <span class="number">2</span>, ..]);
</code></pre>

このような `..` を<strong id="key-slice-pattern" class="keyword">スライス パターン</strong>(slice pattern)と言います。

ちなみに、スライス パターンはリスト パターンの `[]` の内側にだけ書けます。
例えば `array is ..` みたいな書き方は認められていません。

`..` は先頭や中間にも書けます。

<pre class="source" title="先頭、中間の ..">
<code><span class="reserved">var</span> <span class="variable">a1</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> };
<span class="reserved">var</span> <span class="variable">a2</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">2</span> };
<span class="reserved">var</span> <span class="variable">a3</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">1</span>, <span class="number">2</span> };

<span class="comment">// 1で始まって2で終わる(長さは任意)。</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a1</span> <span class="reserved">is</span> [<span class="number">1</span>, .., <span class="number">2</span>]); <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a2</span> <span class="reserved">is</span> [<span class="number">1</span>, .., <span class="number">2</span>]); <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a3</span> <span class="reserved">is</span> [<span class="number">1</span>, .., <span class="number">2</span>]); <span class="comment">// true</span>

<span class="comment">// 末尾が 1, 2で終わる(長さは任意)。</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a1</span> <span class="reserved">is</span> [.., <span class="number">1</span>, <span class="number">2</span>]); <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a2</span> <span class="reserved">is</span> [.., <span class="number">1</span>, <span class="number">2</span>]); <span class="comment">// false</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">a3</span> <span class="reserved">is</span> [.., <span class="number">1</span>, <span class="number">2</span>]); <span class="comment">// true</span>
</code></pre>

ちなみに、2か所以上に `..` を置いてしまうとコンパイル エラーになります。

<pre class="source" title="2か所以上に .. を置くとコンパイル エラー">
<code><span class="reserved">var</span> <span class="variable">array</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> };

<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable">array</span> <span class="reserved">is</span> [.., <span class="error">..</span>]);
</code></pre>

#### <a id="sec-generated-title-26"></a> <a id="sub-pattern">リスト パターンの再帰</a>

[リスト パターン](#list)はカテゴライズするなら[再帰パターン](#recursive)の一種です。
`[]` の中の各要素には任意のパターンを書くことができます。

<pre class="source" title="リスト パターン中の再帰パターン">
<code><span class="reserved">using</span> System<span class="operator">.</span>Numerics;

<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">match1</span>(<span class="reserved">int</span>[] <span class="variable local">array</span>)
    <span class="operator">=&gt;</span> <span class="variable local">array</span> <span class="reserved">is</span> [<span class="number">0</span>, <span class="reserved">_</span>, <span class="operator">&gt;</span> <span class="number">0</span>, <span class="operator">&lt;</span> <span class="number">0</span>, <span class="reserved">var</span> x, ..] <span class="operator">&amp;&amp;</span> (<span class="variable">x</span> <span class="operator">%</span> <span class="number">2</span>) <span class="operator">==</span> <span class="number">1</span>;
<span class="comment">// 前から順に、</span>
<span class="comment">// 0 だけにマッチ(定数パターン)</span>
<span class="comment">// 任意 (破棄パターン)</span>
<span class="comment">// 0 より大きい(関係演算パターン)</span>
<span class="comment">// 0 より小さい(関係演算パターン)</span>
<span class="comment">// 任意 (var パターン)</span>
<span class="comment">// 残り読み飛ばし (スライス パターン)</span>

<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">match2</span>((<span class="reserved">int</span> x, <span class="reserved">int</span> y)[] <span class="variable local">points</span>)
    <span class="operator">=&gt;</span> <span class="variable local">points</span> <span class="reserved">is</span> [(<span class="number">1</span>, <span class="number">2</span>), (<span class="field">x</span>: <span class="number">3</span>, <span class="field">y</span>: <span class="number">4</span>), { <span class="field">x</span>: <span class="number">5</span>, <span class="field">y</span>: <span class="number">6</span> }];
<span class="comment">// 前から順に</span>
<span class="comment">// 位置パターン</span>
<span class="comment">// 位置パターン(名前付き)</span>
<span class="comment">// プロパティ パターン</span>
</code></pre>

また、スライス パターンも、`..` の後ろに続けてパターンを書くことができます。

<pre class="source" title=".. に再帰でパターンを付ける">
<code><span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">match1</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>) <span class="operator">=&gt;</span> <span class="variable local">span</span> <span class="control">switch</span>
{
    [<span class="operator">&gt;</span> <span class="number">0</span>, .. <span class="reserved">var</span> rest] <span class="operator">=&gt;</span> <span class="method">match1</span>(<span class="variable">rest</span>), <span class="comment">// 先頭が正の数で、残りを再帰的に判定</span>
    [] <span class="operator">=&gt;</span> <span class="reserved">true</span>,
    <span class="reserved">_</span> <span class="operator">=&gt;</span> <span class="reserved">false</span>,
};

<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">match2</span>(<span class="reserved">int</span>[] <span class="variable local">array</span>)
    <span class="operator">=&gt;</span> <span class="variable local">array</span> <span class="reserved">is</span> [<span class="number">1</span>, ..[<span class="number">2</span>, <span class="number">3</span>]]; <span class="comment">// あまり意味はなくて、[1, 2, 3] と同じ結果にしかならない</span>
</code></pre>

よく使いそうな例でいうと、「先頭数バイトが特定のパターンの時に読み飛ばし」みたいなことができます。

<pre class="source" title="UTF-8 の BOM 読み飛ばし">
<code><span class="reserved">var</span> <span class="variable">utf8</span> <span class="operator">=</span> <span class="type">File</span><span class="operator">.</span><span class="method">ReadAllBytes</span>(<span class="string">&quot;a.txt&quot;</span>);

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">b</span> <span class="control">in</span> <span class="method">removeBom</span>(<span class="variable">utf8</span>))
{
    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">b</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
}

<span class="reserved">static</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="method">removeBom</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">utf8</span>)
    <span class="operator">=&gt;</span> <span class="variable local">utf8</span> <span class="reserved">is</span> [<span class="number">0xEF</span>, <span class="number">0xBB</span>, <span class="number">0xBF</span>, .. <span class="reserved">var</span> noBom] <span class="operator">?</span> <span class="variable">noBom</span> <span class="operator">:</span> <span class="variable local">utf8</span>;
</code></pre>

#### <a id="sec-generated-title-27"></a> <a id="list-pattern-lowering">リスト パターンの展開結果</a>

リスト パターンやスライス パターンは、
割かしべたに長さ (`Length` もしくは `Count` プロパティ)、インデックス (`a[i]`) やスライス (`a[..]`) に展開されます。
例えば以下のようなリスト パターンを書いた場合、

<pre class="source" title="リスト パターンを使った回文判定の例">
<code><span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">0</span>]));              <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span> }));             <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span> }));          <span class="comment">// false</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">2</span> }));       <span class="comment">// false</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">1</span> }));       <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">1</span>, <span class="number">2</span>, <span class="number">1</span> })); <span class="comment">// true</span>
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="method">palindrome</span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">1</span>, <span class="number">2</span>, <span class="number">2</span> })); <span class="comment">// false</span>

<span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">palindrome</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">list</span>) <span class="operator">=&gt;</span> <span class="variable local">list</span> <span class="control">switch</span>
{
    [] <span class="reserved">or</span> [<span class="reserved">_</span>] <span class="operator">=&gt;</span> <span class="reserved">true</span>,
    [<span class="reserved">var</span> first, .. <span class="reserved">var</span> rest, <span class="reserved">var</span> last] <span class="operator">=&gt;</span> <span class="variable">first</span> <span class="operator">==</span> <span class="variable">last</span> <span class="operator">&amp;&amp;</span> <span class="method">palindrome</span>(<span class="variable">rest</span>),
};
</code></pre>

以下のようなコードとほぼ同じ意味になります。

<pre class="source" title="palindrome の展開結果">
<code><span class="reserved">static</span> <span class="reserved">bool</span> <span class="method">palindrome</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">list</span>) <span class="operator">=&gt;</span> <span class="variable local">list</span><span class="operator">.</span><span class="property">Length</span> <span class="control">switch</span>
{
    <span class="number">0</span> <span class="reserved">or</span> <span class="number">1</span> <span class="operator">=&gt;</span> <span class="reserved">true</span>,
    <span class="operator">&gt;=</span> <span class="number">2</span> <span class="operator">=&gt;</span> <span class="variable local">list</span>[<span class="number">0</span>] <span class="operator">==</span> <span class="variable local">list</span>[<span class="operator">^</span><span class="number">1</span>] <span class="operator">&amp;&amp;</span> <span class="method">palindrome</span>(<span class="variable local">list</span>[<span class="number">1</span>..<span class="operator">^</span><span class="number">1</span>]),
};
</code></pre>

`a[^i]` や `a[i..j]` が使えることが、そのままリスト パターンを使える条件になります。
(詳しい条件に付いては「[インデックス/範囲](../data/dataranges.md)」を参照。)

また、`list is [_, .. var rest, _]` みたいなものが `list[1..^1]` に展開される都合上、
`list[i..j]` がパフォーマンス的にいまいちなコードになっている場合、
リスト パターンも非効率になります。

<pre class="source" title="スライス パターンは文字通りスライスを作る">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m1</span>(<span class="reserved">int</span>[] <span class="variable local">array</span>)
{
    <span class="comment">// 配列に対するスライスは新しい配列を作っちゃう(= 遅い)。</span>
    <span class="reserved">var</span> <span class="variable">slice</span> <span class="operator">=</span> <span class="variable local">array</span>[<span class="number">1</span>..<span class="operator">^</span><span class="number">1</span>];

    <span class="comment">// その影響で、以下のコードも新しい配列がいちいち作られて遅い。</span>
    <span class="comment">// (string でも同じようなことが起きる)。</span>
    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable local">array</span> <span class="reserved">is</span> [<span class="reserved">_</span>, ..<span class="reserved">var</span> rest, <span class="reserved">_</span>]);
}

<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">m2</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">span</span>)
{
    <span class="comment">// Span の場合はそんな非効率な事は起きないので、</span>
    <span class="reserved">var</span> <span class="variable">slice</span> <span class="operator">=</span> <span class="variable local">span</span>[<span class="number">1</span>..<span class="operator">^</span><span class="number">1</span>];

    <span class="comment">// 以下のコードも遅くはならない。</span>
    <span class="comment">// (string に対しては ReadOnlySpan&lt;char&gt; にすると速い)。</span>
    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="variable local">span</span> <span class="reserved">is</span> [<span class="reserved">_</span>, .. <span class="reserved">var</span> rest, <span class="reserved">_</span>]);
}
</code></pre>

### <a id="sec-generated-title-28"></a> <a id="usage"></a>再帰パターンの利用例

「[型スイッチの用途](typeswitch.md#usage)」と同じ題材で、再帰パターンの利用例も挙げておきます。

使った題材は、数式を扱うようなクラスです。
要するに、例えば、「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mi>x</mi><mo>+</mo><mn>1</mn></math>」というような式を、以下のようなクラスで表します。

<pre class="source" title="数式を表す Node クラス">
<code><span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">readonly</span> <span class="type">Node</span> X = <span class="reserved">new</span> <span class="type">Var</span>();
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">Node</span>(<span class="reserved">int</span> <span class="variable">value</span>) =&gt; <span class="reserved">new</span> <span class="type">Const</span>(<span class="variable">value</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Node</span> <span class="reserved">operator</span> +(<span class="type">Node</span> <span class="variable">left</span>, <span class="type">Node</span> <span class="variable">right</span>) =&gt; <span class="reserved">new</span> <span class="type">Add</span>(<span class="variable">left</span>, <span class="variable">right</span>);
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Node</span> <span class="reserved">operator</span> *(<span class="type">Node</span> <span class="variable">left</span>, <span class="type">Node</span> <span class="variable">right</span>) =&gt; <span class="reserved">new</span> <span class="type">Mul</span>(<span class="variable">left</span>, <span class="variable">right</span>);
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Var</span> : <span class="type">Node</span> { <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; <span class="string">&quot;x&quot;</span>; }
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Const</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="method">Const</span>(<span class="reserved">int</span> <span class="variable">value</span>) { Value = <span class="variable">value</span>; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">value</span>) =&gt; <span class="variable">value</span> = Value;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; Value.<span class="method">ToString</span>();
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Add</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="type">Node</span> Left { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Node</span> Right { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="method">Add</span>(<span class="type">Node</span> <span class="variable">left</span>, <span class="type">Node</span> <span class="variable">right</span>) =&gt; (Left, Right) = (<span class="variable">left</span>, <span class="variable">right</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="type">Node</span> <span class="variable">left</span>, <span class="reserved">out</span> <span class="type">Node</span> <span class="variable">right</span>) =&gt; (<span class="variable">left</span>, <span class="variable">right</span>) = (Left, Right);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; <span class="string">$&quot;(</span>{Left.<span class="method">ToString</span>()}<span class="string"> + </span>{Right.<span class="method">ToString</span>()}<span class="string">)&quot;</span>;
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Mul</span> : <span class="type">Node</span>
{
    <span class="reserved">public</span> <span class="type">Node</span> Left { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Node</span> Right { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="method">Mul</span>(<span class="type">Node</span> <span class="variable">left</span>, <span class="type">Node</span> <span class="variable">right</span>) =&gt; (Left, Right) = (<span class="variable">left</span>, <span class="variable">right</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="type">Node</span> <span class="variable">left</span>, <span class="reserved">out</span> <span class="type">Node</span> <span class="variable">right</span>) =&gt; (<span class="variable">left</span>, <span class="variable">right</span>) = (Left, Right);
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() =&gt; <span class="string">$&quot;</span>{Left.<span class="method">ToString</span>()}<span class="string"> * </span>{Right.<span class="method">ToString</span>()}<span class="string">&quot;</span>;
}
</code></pre>

こいつに対して「式の簡約化」をやってみます。
要は、
「<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>+</mo><mn>0</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi></math>に、
<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mn>1</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi></math>に、
<math xmlns="http://www.w3.org/1998/Math/MathML"><mi>x</mi><mo>×</mo><mn>0</mn></math>を<math xmlns="http://www.w3.org/1998/Math/MathML"><mn>0</mn></math>に直す」みたいなやつ。

こういう処理は、`switch`式と位置パターンを使って以下のように書けます。
(コード全体: [Expressions/Program.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Data/Patterns/Expressions/Program.cs))

<pre class="source" title="switch 式と位置パターンを使って式の簡約化">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Node</span> <span class="method">Simplify</span>(<span class="reserved">this</span> <span class="type">Node</span> <span class="variable">n</span>)
    =&gt; <span class="variable">n</span> <span class="reserved">switch</span>
{
    <span class="type">Add</span> (<span class="reserved">var</span> l, <span class="reserved">var</span> r) =&gt; (<span class="variable">l</span>.<span class="method">Simplify</span>(), <span class="variable">r</span>.<span class="method">Simplify</span>()) <span class="reserved">switch</span>
    {
        <span class="comment">// 0 を足しても変わらない</span>
        (<span class="type">Const</span>(0), <span class="reserved">var</span> r1) =&gt; <span class="variable">r1</span>,
        (<span class="reserved">var</span> l1, <span class="type">Const</span>(0)) =&gt; <span class="variable">l1</span>,
        <span class="comment">// 他</span>
        (<span class="reserved">var</span> l1, <span class="reserved">var</span> r1) =&gt; <span class="reserved">new</span> <span class="type">Add</span>(<span class="variable">l1</span>, <span class="variable">r1</span>)
    },
    <span class="type">Mul</span> (<span class="reserved">var</span> l, <span class="reserved">var</span> r) =&gt; (<span class="variable">l</span>.<span class="method">Simplify</span>(), <span class="variable">r</span>.<span class="method">Simplify</span>()) <span class="reserved">switch</span>
    {
        <span class="comment">// 0 を掛けたら 0</span>
        (<span class="type">Const</span>(0) c, <span class="reserved">_</span>) =&gt; <span class="variable">c</span>,
        (<span class="reserved">_</span>, <span class="type">Const</span>(0) c) =&gt; <span class="variable">c</span>,
        <span class="comment">// 1 を掛けても変わらない</span>
        (<span class="type">Const</span>(1), <span class="reserved">var</span> r1) =&gt; <span class="variable">r1</span>,
        (<span class="reserved">var</span> l1, <span class="type">Const</span>(1)) =&gt; <span class="variable">l1</span>,
        <span class="comment">// 他</span>
        (<span class="reserved">var</span> l1, <span class="reserved">var</span> r1) =&gt; <span class="reserved">new</span> <span class="type">Mul</span>(<span class="variable">l1</span>, <span class="variable">r1</span>)
    },
    <span class="reserved">_</span> =&gt; <span class="variable">n</span>
};
</code></pre>

C# 7.3 までだと、この処理は以下のように書くことになります。

<pre class="source" title="C# 7.3 以前での書き方">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Node</span> <span class="method">ClassicSimplify</span>(<span class="reserved">this</span> <span class="type">Node</span> <span class="variable">n</span>)
{
    <span class="control">if</span> (<span class="variable">n</span> <span class="reserved">is</span> <span class="type">Add</span> <span class="variable">a</span>)
    {
        <span class="reserved">var</span> (<span class="variable">l</span>, <span class="variable">r</span>) = <span class="variable">a</span>;
        <span class="reserved">var</span> <span class="variable">l1</span> = <span class="variable">l</span>.<span class="method">Simplify</span>();
        <span class="reserved">var</span> <span class="variable">r1</span> = <span class="variable">r</span>.<span class="method">Simplify</span>();
 
        { <span class="control">if</span> (<span class="variable">l1</span> <span class="reserved">is</span> <span class="type">Const</span> <span class="variable">c</span> &amp;&amp; <span class="variable">c</span>.Value == 0) <span class="control">return</span> <span class="variable">r1</span>; }
        { <span class="control">if</span> (<span class="variable">r1</span> <span class="reserved">is</span> <span class="type">Const</span> <span class="variable">c</span> &amp;&amp; <span class="variable">c</span>.Value == 0) <span class="control">return</span> <span class="variable">l1</span>; }
        <span class="control">return</span> <span class="reserved">new</span> <span class="type">Add</span>(<span class="variable">l1</span>, <span class="variable">r1</span>);
    }
    <span class="control">if</span> (<span class="variable">n</span> <span class="reserved">is</span> <span class="type">Mul</span> <span class="variable">m</span>)
    {
        <span class="reserved">var</span> (<span class="variable">l</span>, <span class="variable">r</span>) = <span class="variable">m</span>;
        <span class="reserved">var</span> <span class="variable">l1</span> = <span class="variable">l</span>.<span class="method">Simplify</span>();
        <span class="reserved">var</span> <span class="variable">r1</span> = <span class="variable">r</span>.<span class="method">Simplify</span>();
 
        {
            <span class="control">if</span> (<span class="variable">l1</span> <span class="reserved">is</span> <span class="type">Const</span> <span class="variable">c</span>)
            {
                <span class="control">if</span> (<span class="variable">c</span>.Value == 0) <span class="control">return</span> <span class="variable">c</span>;
                <span class="control">if</span> (<span class="variable">c</span>.Value == 1) <span class="control">return</span> <span class="variable">r1</span>;
            }
        }
        {
            <span class="control">if</span> (<span class="variable">r1</span> <span class="reserved">is</span> <span class="type">Const</span> <span class="variable">c</span>)
            {
                <span class="control">if</span> (<span class="variable">c</span>.Value == 0) <span class="control">return</span> <span class="variable">c</span>;
                <span class="control">if</span> (<span class="variable">c</span>.Value == 1) <span class="control">return</span> <span class="variable">l1</span>;
            }
        }
        <span class="control">return</span> <span class="reserved">new</span> <span class="type">Mul</span>(<span class="variable">l1</span>, <span class="variable">r1</span>);
    }
    <span class="control">return</span> <span class="variable">n</span>;
}
</code></pre>

<!-- original-page-break -->

## <a id="sec-generated-title-29"></a> <a id="pattern-combintor"></a>パターンの組み合わせ

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 で `and` や `or` などのキーワードを使ってパターンの組み合わせ(pattern combinators)ができるようになりました。

- `and`: 論理積パターン (conjunctive patterns)。両辺に書いたパターンの両方にマッチすることを求める
- `or`: 論理和パターン (disjunctive patterns)。両辺に書いたパターンの少なくとも一方にマッチすることを求める
- `not`: 否定パターン (negated patterns)。後ろに書いたパターンの否定を取る
- `()`: 括弧付きパターン (parenthesized patterns)。`and`, `or` などの結合優先度を指定するためにパターンを `()` でくくる

### <a id="sec-generated-title-30"></a> <a id="and-pattern"></a>and パターン

2つのパターンを `and` キーワードでつなぐことで、両方のパターンにマッチしたときだけマッチした扱いになります。
(論理積パターン(conjunctive patterns)と言ったりもします。)

例えば、複数のインターフェイスをすべて実装しているかを判定するとかに使えます。

<pre class="source" title="and パターンで複数のインターフェイスを実装しているか判定">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="comment">// 2つのインターフェイスを両方実装している場合にマッチ。</span>
    <span class="comment">// この時、パターン中で宣言した a, b にはちゃんと両方「初期化済み」判定を受ける。</span>
    <span class="type">IA</span> <span class="variable">a</span> <span class="reserved">and</span> <span class="type">IB</span> <span class="variable">b</span> =&gt; <span class="variable">a</span>.A * <span class="variable">b</span>.B,
    <span class="reserved">_</span> =&gt; 0,
};
 
<span class="reserved">interface</span> <span class="type">IA</span> { <span class="reserved">int</span> A { <span class="reserved">get</span>; } }
<span class="reserved">interface</span> <span class="type">IB</span> { <span class="reserved">int</span> B { <span class="reserved">get</span>; } }
</code></pre>

その他、後述する関係演算パターンと組み合わせて、「0～10まで」みたいな数値の範囲を表すことができます。

<pre class="source" title="数値の範囲指定パターン">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">byte</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    &gt;= 0 <span class="reserved">and</span> &lt; 10 =&gt; 0,
    &gt;= 10 <span class="reserved">and</span> &lt; 100 =&gt; 1,
    &gt;= 100 =&gt; 2,
};
</code></pre>

### <a id="sec-generated-title-31"></a> <a id="or-pattern"></a>or パターン

2つのパターンを `or` キーワードでつなぐことで、少なくともいずれか片方のパターンにマッチしたときにマッチした扱いになります。
(論理和パターン(disjunctive patterns)と言ったりもします。)

単純に複数の値にマッチさせたり、複数の型にマッチさせることができます。

<pre class="source" title="複数の値にマッチ">
<code><span class="reserved">bool</span> <span class="method">IsSmallPrime</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> 2 <span class="reserved">or</span> 3 <span class="reserved">or</span> 5 <span class="reserved">or</span> 7;
 
<span class="reserved">bool</span> <span class="method">IsTrue</span>(<span class="reserved">bool</span>? <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="reserved">true</span> =&gt; <span class="reserved">true</span>,
    <span class="comment">// _ (true 以外)と差はないものの、あり得る値を網羅していることがチェックできるという点で</span>
    <span class="comment">// true, false, null の3つの値を並べる意味はなくはない。</span>
    <span class="reserved">false</span> <span class="reserved">or</span> <span class="reserved">null</span> =&gt; <span class="reserved">false</span>,
};
</code></pre>

また、複数の型にマッチさせたりもできます。

<pre class="source" title="複数の型にマッチ">
<code><span class="reserved">bool</span> <span class="method">IsByte</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">byte</span> <span class="reserved">or</span> <span class="reserved">sbyte</span>;
</code></pre>

`and` と同様、後述する関係演算パターンとの組み合わせでも使えます。

<pre class="source" title="関係演算と or パターンの組み合わせ">
<code><span class="reserved">int</span> <span class="method">Triangular</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    &lt; -1 <span class="reserved">or</span> &gt; 1 =&gt; 0,
    <span class="reserved">_</span> =&gt; 1 - <span class="type">Math</span>.<span class="method">Abs</span>(<span class="variable">x</span>),
};
</code></pre>

#### <a id="sec-generated-title-32"></a> <a id="conditional-keyward-and-or"></a>文脈キーワードの and, or

C# のキーワード追加では恒例行事ですが、
既存コードをなるべく壊さないように、後付けな `and`、`or` などは[文脈キーワード](../appendix/ap_reserved.md#context)になっています。

例えば、あまり意味のあるコードではないものの以下のようなコードは有効な C# コードになります。

<pre class="source" title="and, or は文脈キーワード">
<code><span class="comment">// 水色の部分は型名の or, and。青色の部分はキーワードの or, and。</span>
<span class="reserved">bool</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="reserved">is</span> <span class="type">or</span> <span class="reserved">or</span> <span class="type">and</span> <span class="reserved">and</span> <span class="type">and</span>;
 
<span class="reserved">class</span> <span class="type">and</span> { }
<span class="reserved">class</span> <span class="type">or</span> { }
</code></pre>

### <a id="sec-generated-title-33"></a> <a id="not-pattern"></a>not パターン

パターンの前に `not` キーワードを置くことで、元のパターンの成否を反転させることができます。
(否定パターン(negated patterns)と言ったりもします。)

おそらく一番使い道があるのは `not null` だと思います。

<pre class="source" title="not null">
<code><span class="reserved">using</span> System;
 
<span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span>? <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
    }
}
</code></pre>

`string` 相手だと `x != null` と大差ないですが、[場合によってはパフォーマンスがよくなることもあります](../../../blog/2020/12/isnull/index.md)。
また、`!` の視認性があまりよくないので `!=` よりも `is not` の方を好む人もいるようです。

あと、いわゆる early return に使えます。
以下のように、特定条件を満たさないときに早々に `return` ステートメントで関数を抜けてしまうときに `not` パターンが使えます。

<pre class="source" title="not パターンで early return">
<code><span class="reserved">using</span> System;
 
<span class="reserved">void</span> <span class="method">PositivePattern</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">string</span> <span class="variable">s</span>)
    {
        <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
    }
}

<span class="comment">// ↑のメソッドを early return で書き直したもの。</span>
<span class="reserved">void</span> <span class="method">EarlyReturn</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="comment">// if の中に限り、not + 型パターンで変数宣言可能。</span>
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">string</span> <span class="variable">s</span>) <span class="control">return</span>;
 
    <span class="comment">// この場合、if 中(not string の時) には s が使えず、</span>
    <span class="comment">// その後ろ(string の時)でだけ s が使える。</span>
 
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
}
</code></pre>

### <a id="sec-generated-title-34"></a> <a id="parenthesized-patterns"></a>括弧付きパターン

`not`, `and`, `or` の結合順位は `!`, `&&`, `||` と同じで、`not` → `and` → `or` の順です。

例えば以下のような書き方をすると、`and` の結合が優先されます。

<pre class="source" title="and が優先">
<code><span class="reserved">bool</span> <span class="method">IsAsciiLetter</span>(<span class="reserved">char</span> <span class="variable">c</span>) =&gt; <span class="variable">c</span> <span class="reserved">is</span> &gt;= <span class="string">&#39;a&#39;</span> <span class="reserved">and</span> &lt;= <span class="string">&#39;z&#39;</span> <span class="reserved">or</span> &gt;= <span class="string">&#39;A&#39;</span> <span class="reserved">and</span> &lt;= <span class="string">&#39;Z&#39;</span>;
</code></pre>

`&&` と `||` でもよくある話ですが、優先度がわかりにくくて読むときにつらかったりします。
また、`or` の方を優先したいことも当然あります。

そこで、パターンを `()` で囲んで結合優先度を明示することができるようになりました。
(括弧付きパターン(parenthesized patterns)と言ったりもします。)
先ほどの `IsAsciiLetter` の例は以下のようにも書けます。

<pre class="source" title="() を付けて優先度を明示">
<code><span class="comment">// () を付けて優先度を明示。</span>
<span class="reserved">bool</span> <span class="method">IsAsciiLetter</span>(<span class="reserved">char</span> <span class="variable">c</span>) =&gt; <span class="variable">c</span> <span class="reserved">is</span> (&gt;= <span class="string">&#39;a&#39;</span> <span class="reserved">and</span> &lt;= <span class="string">&#39;z&#39;</span>) <span class="reserved">or</span> (&gt;= <span class="string">&#39;A&#39;</span> <span class="reserved">and</span> &lt;= <span class="string">&#39;Z&#39;</span>);
</code></pre>

前述の「複数のインターフェイスをすべて実装しているかを判定」と「`not` パターンを使った early return」の組み合わせもできます。

<pre class="source" title="not (and)">
<code><span class="reserved">using</span> System;
 
<span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">x</span>)
{
    <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> <span class="reserved">not</span> (<span class="type">IA</span> <span class="variable">a</span> <span class="reserved">and</span> <span class="type">IB</span> <span class="variable">b</span>)) <span class="control">return</span>;
 
    <span class="comment">// a, b ともに使える。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.A * <span class="variable">b</span>.B);
}
 
<span class="reserved">interface</span> <span class="type">IA</span> { <span class="reserved">int</span> A { <span class="reserved">get</span>; } }
<span class="reserved">interface</span> <span class="type">IB</span> { <span class="reserved">int</span> B { <span class="reserved">get</span>; } }
</code></pre>

## <a id="sec-generated-title-35"></a> <a id="relational-patterns"></a>関係演算パターン

<h5 class="version version9">Ver. 9.0</h5>

`<`, `<=`, `>`, `>=` の4つの関係演算子を使って数値の大小をパターンの中に書けます。
(関係演算パターン(relational patterns)と言ったりします。)

<pre class="source" title="">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">byte</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    &lt; 10 =&gt; 1, <span class="comment">// 0～9</span>
    &gt;= 10 <span class="reserved">and</span> &lt;= 99 =&gt; 2, <span class="comment">// 10～99</span>
    &gt; 99 =&gt; 3, <span class="comment">// 100～255</span>
};
</code></pre>

初期の案では、C# 8.0 で[範囲アクセス用に `..` 演算子を導入](../cheatsheet/ap_ver8.md#range)したのに対して、「範囲パターン」も用意したいというものでした。
ただ、`x..y` みたいな範囲パターンだと、両端(この場合 `x`と`y`)を含むかどうかがわかりにくくて困るだろうということで不採用になっていました。
( `..` 演算子は[インデックス用途](../data/dataranges.md#index-usage)に絞ったことで、先頭`x`は含む、末尾`y`は含まないというルールにできましたが、「範囲パターン」の場合はあまり用途を絞れないので同じルールだと使いにくいという問題があります。)

他のプログラミング言語だと、範囲を表すために `<..`, `=..`, `..<`, `..=` など `..` の前後に `<` や `=` を付けることで両端の含む・含まない問題を解決していたりします。
しかし、C# ではもういっそ、`<`, `<=`, `>`, `>=` と `and` パターンの組み合わせで範囲を表そうということになりました。

<!-- original-page-break -->

## <a id="sec-generated-title-36"></a> <a id="compile-time-validation"></a>コンパイル時検査

パターン マッチングでは、値の網羅性を満たしているかどうかと、書いたパターンが重複していないかをコンパイル時に検査してくれる仕組みがあります。

### <a id="sec-generated-title-37"></a> <a id="exhaustive"></a>網羅性チェック

いくつかの型は決まった値しかとりません。例えば `bool` なら `true` か `false` の2値ですし、
`bool?` でも `true`, `false`, `null` の3値だけです。
`byte` も高々256個の値しか持ちません。
[型スイッチのページにも書いていますが](typeswitch.md#exhaustive)、パターン マッチングではこれらの値をすべて網羅しているかどうか(exhaustiveness: 網羅性)の検査をしてくれます。

<pre class="source" title="bool, bool? の網羅性検査">
<code><span class="comment">// 無警告</span>
<span class="reserved">int</span> <span class="method">A</span>(<span class="reserved">bool</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="reserved">true</span> =&gt; 1,
    <span class="reserved">false</span> =&gt; 0,
};
 
<span class="comment">// 警告あり(CS8655: 条件に null が足りていない)</span>
<span class="reserved">int</span> <span class="method">B</span>(<span class="reserved">bool</span>? <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="warning"><span class="control">switch</span></span>
{
    <span class="reserved">true</span> =&gt; 1,
    <span class="reserved">false</span> =&gt; 0,
};
 
<span class="comment">// 無警告</span>
<span class="reserved">int</span> <span class="method">C</span>(<span class="reserved">bool</span>? <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    <span class="reserved">true</span> =&gt; 1,
    <span class="reserved">false</span> <span class="reserved">or</span> <span class="reserved">null</span> =&gt; 0,
};
</code></pre>

また、数値型に対しては、[関係演算パターン](#relational-patterns)を使って「100未満」と「100以上」というように相補的に値を網羅しているかを検査できます。
例えば以下のコードには条件漏れがあって警告を起こします。

<pre class="source" title="実は条件漏れがあるコード">
<code><span class="comment">// 警告を起こす</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">byte</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    &lt; 10 =&gt; 1,
    &gt;= 10 <span class="reserved">and</span> &lt; 100 =&gt; 2,
    <span class="comment">// &lt; 100 と &gt; 100 (どちらも 100 は含まない)しかないので、実は 100 が漏れてる</span>
    &gt; 100 =&gt; 3,
};
</code></pre>

値パターンや `or` パターンとの組み合わせでも網羅性の検査がかかります。

<pre class="source" title="値パターンや or パターンとの組み合わせでの網羅性検査">
<code><span class="comment">// 整数の場合は値パターンとかその or パターン、関係演算パターンの組み合わせでも網羅性検査がかかる</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">uint</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="control">switch</span>
{
    0 <span class="reserved">or</span> 2 <span class="reserved">or</span> 4 <span class="reserved">or</span> 6 <span class="reserved">or</span> 8 =&gt; 0,
    1 <span class="reserved">or</span> 3 <span class="reserved">or</span> 5 <span class="reserved">or</span> 7 <span class="reserved">or</span> 9 =&gt; 1,
    &gt;= 10 =&gt; -1, <span class="comment">// この行がなかったり、条件が &gt; 10 とかだったりすると警告</span>
};
</code></pre>

一般の型に対しても、「null か非 null か」みたいな条件が相補的で、これに対しても網羅性の検査がかかります。

<pre class="source" title="null か非 null かの網羅性">
<code><span class="comment">// null か非 null かで網羅性検査がかかっていて、どれか1行でも欠けていると警告</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">int</span>? <span class="variable">x</span>, <span class="reserved">int</span>? <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) <span class="control">switch</span>
{
    (<span class="reserved">null</span>, <span class="reserved">null</span>) =&gt; 0,
    ({ }, <span class="reserved">null</span>) =&gt; -1,
    (<span class="reserved">null</span>, { }) =&gt; 1,
    ({ } x1, { } y1) =&gt; <span class="variable">x1</span>.<span class="method">CompareTo</span>(<span class="variable">y1</span>),
};
</code></pre>

### <a id="sec-generated-title-38"></a> <a id="case-duplicate"></a>条件の重複チェック

`switch` ステートメント/`switch` 式中に絶対に到達できない条件があるとき、
ある程度はコンパイル時に検知してコンパイル エラーにしてもらえます。

パターンを使った `switch` の条件は[上から逐次判定](typeswitch.md#sequential)なので、要するに、上の方に下にある条件の上位互換な条件があるとコンパイル エラーになります。

一番わかりやすいのは[破棄パターン](#discard)で、これは「何にでも一致するパターン」なので、その下に何かを書くとエラーになります。

<pre class="source" title="破棄パターンの下に別条件を書いても絶対に到達できない">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="reserved">_</span> =&gt; 0,
    <span class="error"><span class="reserved">string</span> <span class="reserved">_</span></span> =&gt; 1,
};
</code></pre>

当然ですが、全く同じ条件が2つ以上ある場合にも、1つ目以外には絶対に到達しないのでエラーになります。

<pre class="source" title="同じ条件が並ぶとエラー">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="reserved">string</span> <span class="variable">s</span> =&gt; <span class="variable">s</span>.Length,
    <span class="error"><span class="reserved">string</span> <span class="reserved">_</span></span> =&gt; 1,
};
</code></pre>

ちなみに、[`when`句](typeswitch.md#switch)だと重複チェックが漏れることがあります。
一方、同じような条件でも、[再帰パターン](#recursive)を使うとチェックが働きやすいです。

<pre class="source" title="再帰パターンの方が重複チェックが正確">
<code><span class="reserved">int</span> <span class="method">M1</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="comment">// when 句を使うと「同じ条件」判定ができなくなる。コンパイルできてしまう。</span>
    <span class="reserved">string</span> <span class="variable">s</span> <span class="control">when</span> <span class="variable">s</span>.Length == 0 =&gt; 0,
    <span class="reserved">string</span> <span class="variable">s</span> <span class="control">when</span> <span class="variable">s</span>.Length == 0 =&gt; 1,
    <span class="reserved">_</span> =&gt; -1,
};
 
<span class="reserved">int</span> <span class="method">M2</span>(<span class="reserved">object</span> <span class="variable">obj</span>) =&gt; <span class="variable">obj</span> <span class="control">switch</span>
{
    <span class="comment">// 同じことを再帰パターンでやるとちゃんと重複チェックが掛かる。2つ目でコンパイル エラーに。</span>
    <span class="reserved">string</span> { Length: 0 } =&gt; 0,
    <span class="error"><span class="reserved">string</span> { Length: 0 }</span> =&gt; 1,
    <span class="reserved">_</span> =&gt; -1,
};
</code></pre>

また、前節の[網羅性](#exhaustive)とも関連して、
全ての値を網羅済みのところの後ろに条件を足しても、その行には絶対に来ないのでエラーにできます。
例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="網羅済みのところの後ろに追加の条件を足すとエラー">
<code><span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">a</span>, <span class="reserved">bool</span> <span class="variable">b</span>) =&gt; (<span class="variable">a</span>, <span class="variable">b</span>) <span class="control">switch</span>
{
    (<span class="reserved">false</span>, <span class="reserved">false</span>) =&gt; 0,
    (<span class="reserved">true</span>, <span class="reserved">false</span>) =&gt; 1,
    (<span class="reserved">false</span>, <span class="reserved">true</span>) =&gt; 2,
    (<span class="reserved">true</span>, <span class="reserved">true</span>) =&gt; 3,
    <span class="comment">// bool の場合上記4つ以外は絶対にないことがわかるので、この行でコンパイル エラーになる。</span>
    <span class="error"><span class="reserved">_</span></span> =&gt; 4,
};
</code></pre>
