---
title: "【C# 12 候補】 Extensions"
source_url: "https://ufcpp.net/blog/2023/3/extensions/"
content_type: "BlogEntry"
published_at: "2023-03-05T22:39:12"
updated_at: "2023-03-05T22:39:12"
tags: []
umbraco_id: 2458
parent_id: 2457
sort_order: 0
aliases: []
---

# 【C# 12 候補】 Extensions

今日は「拡張」(拡張メソッド的なものの改良)の話。
(今日のこれは、C# 12 で全て実装されるかどうか怪しく、
一部 13 以降になる可能性も結構高いです。)

* 提案ドキュメント: [Extension types](https://github.com/dotnet/csharplang/blob/main/proposals/extensions.md)
* Working Group 議事録
    * [2022/11/10](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2022-11-10.md)
    * [2023/1/23](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-01-23.md)
    * [2023/1/25](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-01-25.md)
    * [2023/2/15](https://github.com/dotnet/csharplang/blob/main/meetings/working-groups/roles/roles-2023-02-15.md)

結構昔から、

* [Extension everything](https://github.com/dotnet/roslyn/issues/11159): 拡張メソッドと同じような仕組みでプロパティ、インデクサー、演算子などを「拡張」したい
* [Roles](https://github.com/dotnet/csharplang/blob/main/meetings/2022/LDM-2022-09-26.md#roles--extensions): 「拡張」をある種の「型」扱いしたい

みたいな案があったんですが、結局、この Roles をベースに、Extensions とか Extension types という名称で実装が進みそうです。

原案で「Roles/Extensions」と呼ばれていたものは、「Explicit /Implicit extensions」となります。

## extension キーワード

提案されている現状の文法では、新たに `extension` キーワードを使った「型定義」できるようにするみたいです。

例えば、`int` に対する「拡張」を書くのなら、以下のような書き方をします。

<pre class="source" title="int に対する extension">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
}
</pre>

### なんでも拡張

現状の[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)の仕様では、名前通り、メソッドしか定義できません。
プロパティなどを「拡張」したいという要望は長らくあるんですが、
今の拡張メソッドの文法がプロパティなどに向いていなさ過ぎて、導入できずにいます。
また、静的メンバーにも対応していません。

<pre class="source" title="プロパティに向かない文法、静的メンバーにも未対応">
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="comment">// x.Method() と呼べる。</span>
    <span class="comment">// 第1引数を特別扱いしてる都合上…</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Method</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

    <span class="comment">// 引数のないプロパティとか、</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="property"><span class="error" title="CS0548">Property</span></span></span> { }

    <span class="comment">// インデクサーはどうするか悩ましい。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="error" title="CS0720"><span class="error" title="CS0548"><span class="error" title="CS0106"><span class="reserved">this</span></span></span></span>[<span class="reserved">int</span> <span class="variable local">index</span>] { }

    <span class="comment">// 元が static なものを拡張する手段もない。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="reserved">operator</span> <span class="error" title="CS1534"><span class="error" title="CS0715"><span class="error" title="CS0161"><span class="operator">+</span></span></span></span>() { }
}
</pre>

`extension` を使った定義では、インスタンス フィールドと[自動プロパティ](../../../../study/csharp/oop/oo_property.md#auto)・[自動イベント](../../../../study/csharp/functional/sp_event.md#auto-event)(暗黙的にフィールドが必要)を除いて、どのメンバーでも使えます。

<pre class="source" title="プロパティやインデクサー、静的メンバーにも対応">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>() { }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span> => <span class="reserved">this</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable">index</span>] => <span class="variable">index</span>;

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticMethod</span></span>() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Ex</span> <span class="reserved">operator</span>+ (<span class="type">Ex</span> <span class="variable">x</span>) => <span class="variable">x</span>;
}
</pre>

ちなみに、インターフェイスも実装できる予定です。
既存の(第3者が作っていて自分では手を入れられない)型にインターフェイスを後挿しできます。

<pre class="source" title="拡張インターフェイス実装">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="reserved">bool</span> : <span class="type">IFormattable</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">ToString</span>(<span class="reserved">string</span>? <span class="variable">format</span>, <span class="type">IFormatProvider</span>? <span class="variable">formatProvider</span>) =&gt; <span class="reserved">this</span> ? <span class="string">"true"</span> : <span class="string">"false"</span>;
}
</pre>

これで、以下のような呼び出しができるようになる予定です。

<pre class="source" title="拡張定義したプロパティ、インデクサー、静的メソッドを呼び出し">
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="variable">x</span><span class="operator">.</span><span class="method">Method</span>();
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">x</span><span class="operator">.</span><span class="property">Property</span>;
<span class="reserved">_</span> <span class="operator">=</span> <span class="variable">x</span>[<span class="number">1</span>];
<span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">StaticMethod</span></span>();

<span class="type">IFormattable</span> f = <span class="reserved">true</span>;
</pre>

### 拡張「型」

既存の拡張メソッドでも起こるんですが、
複数の拡張があるとき、同名のメソッドが被ってどちらを呼ぶべきか解決できない時があります。

<pre class="source" title="名前被りで解決できない拡張メソッド">
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// 2つ同名のメソッドがあって優先度解決できないのでコンパイル エラー。</span>
<span class="variable">x</span><span class="operator">.</span><span class="method"><span class="error" title="CS0121">Method</span></span>();

<span class="comment">// 解決するためには途端に「普通の静的メソッド」呼びに戻る。</span>
<span class="static"><span class="type">Ex1</span></span><span class="operator">.</span><span class="method"><span class="static">Method</span></span>(<span class="variable">x</span>);
<span class="type"><span class="static">Ex2</span></span><span class="operator">.</span><span class="static"><span class="method">Method</span></span>(<span class="variable">x</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex1</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Method</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Ex2</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Method</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }
}
</pre>

また、拡張メソッドは元々あるインスタンス メソッドよりも優先度が低いので、
同名のメソッドで「上書き」することもできません。

<pre class="source" title="拡張メソッドでは同名インスタンス メソッドの上書きはできない">
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// インスタンス メソッドの方が優先度が高く、この書き方で Ex1.ToString は呼べない。</span>
<span class="variable">x</span><span class="operator">.</span><span class="method">ToString</span>();

<span class="comment">// 「普通の静的メソッド」呼びで一応解決は可能。</span>
<span class="type"><span class="static">Ex1</span></span><span class="operator">.</span><span class="static"><span class="method">ToString</span></span>(<span class="variable">x</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Ex1</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">ToString</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X2&quot;</span>);
}
</pre>

これらの例の通り、
名前被り時の解決方法は「普通の静的メソッドとして呼ぶ」という手段です。

一方、`extension` では、以下のように、キャスト的な文法で解決します。

<pre class="source" title="キャストで拡張を使う">
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// 「暗黙」にやろうとすると、extension を使ったやり方でも解決不能・元々あるメソッド優先。</span>
<span class="variable">x</span><span class="operator">.</span><span class="method">Method</span>();   <span class="comment">// これは解決不能。</span>
<span class="variable">x</span><span class="operator">.</span><span class="method">ToString</span>(); <span class="comment">// これは int.ToString が呼ばれる。</span>

<span class="comment">// キャスト構文で解決可能。</span>
((<span class="type">Ex1</span>)<span class="variable">x</span>)<span class="operator">.</span><span class="method">Method</span>();   <span class="comment">// Ex1.Method。</span>
((<span class="type">Ex2</span>)<span class="variable">x</span>)<span class="operator">.</span><span class="method">Method</span>();   <span class="comment">// Ex2.Method。</span>
((<span class="type">Ex2</span>)<span class="variable">x</span>)<span class="operator">.</span><span class="method">ToString</span>(); <span class="comment">// Ex1.ToString。</span>

<span class="comment">// 「拡張型」の変数で1度受けるのでも解決可能。</span>
<span class="comment">// この場合は int のメソッドよりも extension のメソッドの方が優先。</span>
<span class="type">Ex1</span> <span class="variable">ex</span> <span class="operator">=</span> <span class="variable">x</span>;
<span class="variable">ex</span><span class="operator">.</span><span class="method">Method</span>();
<span class="variable">ex</span><span class="operator">.</span><span class="method">ToString</span>();

<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex1</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">ToString</span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X2&quot;</span>);
}

<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex2</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }
}
</pre>

### 実際に型として使える

`Ex1 ex` みたいな変数を定義できることからもわかる通り、
`extension` は普通に「型」という扱いです。
なので、拡張型 (extension types)と呼びます。

変数だけではなく、引数、型引数などにも使えます。

<pre class="source" title="拡張型引数">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// int → Ex1 の暗黙の変換。</span>
<span class="method"><span class="static">M1</span></span>(<span class="variable">x</span>);

<span class="comment">// IEnumerable&lt;int&gt; → IEnumerable&lt;Ex1&gt; の暗黙の変換。</span>
<span class="static"><span class="method">M2</span></span>(<span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span> });

<span class="comment">// 引数に拡張型を使う。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M1</span></span>(<span class="type">Ex1</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">x</span>);

<span class="comment">// 型引数に拡張型を使う。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M2</span></span>(<span class="type">IEnumerable</span>&lt;<span class="type">Ex1</span>&gt; <span class="variable local">x</span>)
{
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">item</span> <span class="control">in</span> <span class="variable local">x</span>) <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">item</span>);
}

<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex1</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
}
</pre>

### explicit extension

これまで説明なしで `implicit extension` という書き方をしてきましたが、
そこから察していただける通り、`explicit extension` もあります。
名前通り型の明示が必須になって、
`int` などの元の型のままでメンバーを呼ぶことができなくなります。

<pre class="source" title="explicit exntension">
<span class="comment">// (implicit なら呼べるけど) explicit extension では呼べない。</span>
<span class="number">1</span>.<span class="method">Method</span>();
<span class="reserved">int</span>.<span class="method"><span class="static">StaticMethod</span></span>();

<span class="comment">// こんな風に、型を明示して呼ぶ想定。</span>
<span class="type">Ex</span> <span class="variable">ex</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="variable">ex</span><span class="operator">.</span><span class="method">Method</span>();
<span class="type">Ex</span>.<span class="method"><span class="static">StaticMethod</span></span>();

<span class="reserved"><em>explicit</em></span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticMethod</span></span>() { }
}
</pre>

「`1.Method()` みたな呼び方ができないものが『extension』なのか？」みたいな話はあります。
なので、元々は role, view, shape (同じデータの別の役割・見え方・輪郭)みたいな言葉を使おうかという話も出ていました。
ただ、変に用語を増やすよりは、「暗黙的拡張」、「明示的拡張」と呼び分ける方がいいのではないかということになって、こちらにも `extension` を使おうという流れになっています。

ちなみに、同じ型に対する別の extension はお互い型変換させるつもりはないそうです。

<pre class="source" title="2つの異なる explicit exntension">
<span class="comment">// 基となる型から extension への変換は暗黙 OK。</span>
<span class="type">Ex1</span> <span class="variable">ex1</span> = 1;
<span class="type">Ex2</span> <span class="variable">ex2</span> = 2;

<span class="comment">// extension 同士の変換はダメ。</span>
<span class="type">Ex2</span> <span class="variable">ex3</span> = <span class="variable">ex1</span>;

<span class="reserved">explicit</span> <span class="reserved">extension</span> <span class="type">Ex1</span> <span class="reserved">for</span> <span class="reserved">int</span> { }
<span class="reserved">explicit</span> <span class="reserved">extension</span> <span class="type">Ex2</span> <span class="reserved">for</span> <span class="reserved">int</span> { }
</pre>

要は、strong-typedef 的なものに使えます。
(この辺りが「それは extension なのか？」と言われるゆえんです。
拡張するメンバーが一切なくても使い道があります。)

### 細かい文法話

extension は別の extension からの派生もOKで、
多重継承も認めるそうです。

インターフェイス実装もできるわけで、
`:` の後ろには他の extension とインターフェイスが並びます。
例えば以下のような感じ。
(`T` は通常の型、`I` 始まりのものがインターフェイス、`X` 始まりのものが extension。)

<pre class="source" title="extension 定義(文法まとめ)">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">X</span> <span class="reserved">for</span> <span class="type">T</span> : <span class="type">XA</span>, <span class="type">XB</span>, <span class="type">IA</span>, <span class="type">IB</span>
{
}
</pre>

ちなみに、ここでいう `T` (`for` の後ろの型)のことを「基になる型」(underlying type: 根底にある型、基礎となる型)と言います。
(C# 的には、`enum` なんかの `enum E : int { }` とかの `int` の部分も underlying type と言います。Microsoft の和訳では undelying type = 基になる型。)

クラスの場合は基底クラスとインターフェイスをあまり区別せず、`class Derived : Base, IA, IB` と書ける(ただし、基底クラスは先頭である必要あり)わけですが、
extension の場合は `for` を使って `:` とは分ける方向で考えているみたいです。
基底型をいくつも持てるし、ただでさえ基底型とインターフェイスの混在があるのに、さらに基になる型 `T` も並べた時に、「同じ `:` を使って、一番先頭という縛りを設ける」というのはいささか不安だったそうです。
特に、`partial` を認めるつもりなので、その場合に「一番先頭」があやふやになるのを懸念したみたいです。

<pre class="source" title="partial extension">
<span class="reserved">implicit</span> <span class="reserved">partial</span> <span class="reserved">extension</span> <span class="type">X</span> <span class="reserved">for</span> <span class="type">T</span> : <span class="type">XA</span>, <span class="type">IA</span>
{
}

<span class="reserved">implicit</span> <span class="reserved">partial</span> <span class="reserved">extension</span> <span class="type">X</span> : <span class="type">XB</span>, <span class="type">IB</span>
{
}
</pre>

また、既存の拡張メソッドがトップレベルの型での定義以外を認めていないのに対して、
新しい extension は入れ子を認めるそうです。

<pre class="source" title="partial extension">
<span class="reserved">using</span> <span class="reserved">static</span> <span class="type">Ex</span>;
<span class="reserved">using</span> <span class="reserved">static</span> <span class="type">C</span>;

<span class="comment">// ちゃんと呼べる。</span>
1.<span class="method">M1</span>();
2.<span class="method">M2</span>();

<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="type">T</span>
{
    <span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">NextedEx</span> <span class="reserved">for</span> <span class="reserved">int</span>
    {
        <span class="reserved">void</span> <span class="method">M1</span>() { }
    }
}

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">NextedEx</span> <span class="reserved">for</span> <span class="reserved">int</span>
    {
        <span class="reserved">void</span> <span class="method">M2</span>() { }
    }
}
</pre>

さらに、ジェネリックにもできるそうです。

<pre class="source" title="generic extension">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">X</span>&lt;<span class="type">T</span>&gt; <span class="reserved">for</span> <span class="type">T</span> : <span class="type">XA</span>, <span class="type">IA</span>
    <span class="reserved">where</span> <span class="type">T</span> : <span class="type">IT</span>
{
}
</pre>

派生 extension を作る際には、
基となる型の条件を強める方向でなら、基となる型の変更もできるみたいです。

<pre class="source" title="基となる型の変更">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">XBase</span> <span class="reserved">for</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">object</span>&gt;
{
}

<span class="comment">// IEnumerable&lt;object&gt; から IEnumerable&lt;string&gt; への変更はOK。</span>
<span class="comment">// (逆だとダメ。)</span>
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">XDerived1</span> <span class="reserved">for</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">string</span>&gt; : <span class="type">XBase</span>
{
}

<span class="comment">// ちなみに、基となる型に変更がないなら for は省略可。</span>
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">XDerived2</span> : <span class="type">XBase</span>
{
}
</pre>

## 実装方法

現状、文法面をどうするかが議論の中心で、
あんまり実装方法に関する決定はないみたいなんですが、
案として挙がっているのは以下のような方向性です。

例えば、前述の(以下に再掲) extension に対して、

<pre class="source" title="前述の extension">
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">Ex</span> <span class="reserved">for</span> <span class="reserved">int</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>() { }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span> => <span class="reserved">int</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable">index</span>] => <span class="variable">index</span>;

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticMethod</span></span>() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">Ex</span> <span class="reserved">operator</span>+ (<span class="type">Ex</span> <span class="variable">x</span>) => <span class="variable">x</span>;
}
</pre>

以下のようなラッパー構造体を作るのはどうかという案になっています。

<pre class="source" title="">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Ex</span>
{
    <span class="reserved">private</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field">@this</span>;
    <span class="reserved">public</span> <span class="type struct">Ex</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">@this</span>) <span class="operator">=&gt;</span> <span class="reserved">this</span><span class="operator">.</span><span class="field">@this</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">@this</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Method</span>() { }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span> <span class="operator">=&gt;</span> <span class="field">@this</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] <span class="operator">=&gt;</span> <span class="variable local">index</span>;

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">StaticMethod</span></span>() { }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type struct">Ex</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type struct">Ex</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span>;
}
</pre>

[ref 構造体](../../../../study/csharp/resource/refstruct.md)、[ref フィールド](../../../../study/csharp/resource/refstruct.md#ref-field)を使う想定なので、
別途以下のような機能(C# 11 時点で認められていない)が必要になります。

* ref 構造体の ref フィールドを持てるようにする
* ref 構造体をジェネリック型引数にする
* ref 構造体でインターフェイスを実装する

<pre class="source" title="C# 11 で無理なものの、extension の実装に欲しいもの">
<span class="comment">// 現状、ref 構造体はインターフェイス実装を持てない。</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="error" title="CS0535"><span class="error" title="CS0535"><span class="error" title="CS8343"><span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;</span></span></span>
{
    <span class="comment">// 現状、ref 構造体の ref フィールドはダメ。</span>
    <span class="error" title="CS9050"><span class="reserved">ref</span> <span class="type struct">S</span></span> <span class="warning" title="CS0169"><span class="field"><span class="error" title="CS0523">_refS</span></span></span>;

    <span class="comment">// 現状、ref 構造体を型引数に渡せない。</span>
    <span class="type">IEnumerable</span>&lt;<span class="type struct">S</span>&gt; <span class="method"><span class="error" title="CS0306">GetItems</span></span>()
    {
        <span class="control">yield</span> <span class="control">return</span> <span class="reserved">default</span>;
    }
}
</pre>

## 実装フェーズ

冒頭に「C# 12 で全て実装されるかどうか怪しい」という話をしましたが、
具体的には以下のような3つのフェーズに分かれています。

1. 静的メンバーの拡張だけ認める
2. インスタンス メンバーも認める
3. インターフェイス実装を認める

前節で説明したように、ref フィールドを使った実装にする可能性が濃厚なわけで、
これら3フェーズは要するに、

* 静的メンバー: 現状でもできる
* インスタンス メンバー: ref 構造体の ref フィールドを認めた上でやりたい
* インターフェイス実装: ref 構造体のインターフェイス実装を認めた上でやりたい

という区分だったりします。

1と2を分けるのは少々気持ち悪いので実際にはこの2つは同時に提供されるかもしれませんが、
実装都合でいうと結構な難易度の隔たりがあるそうです。

ちなみに、「[静的メソッドの拡張をしたい、既存の型に静的メソッドを追加したい](https://github.com/dotnet/csharplang/discussions/2505)」という要望もそれなりに昔からあるので、
1だけ先行実装というのもそこまで不自然でもないかもしれません。
