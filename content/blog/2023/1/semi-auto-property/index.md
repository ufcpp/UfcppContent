---
title: "【C# 12 候補】半自動プロパティ"
source_url: "https://ufcpp.net/blog/2023/1/semi-auto-property/"
content_type: "BlogEntry"
published_at: "2023-01-16T22:15:04"
updated_at: "2023-01-16T22:15:04"
tags: []
umbraco_id: 2452
parent_id: 2449
sort_order: 2
aliases: []
---

# 【C# 12 候補】半自動プロパティ

今日は半自動プロパティの話。

* 提案 issue: [Proposal: Semi-Auto-Properties; field keyword #140](https://github.com/dotnet/csharplang/issues/140)

[約1年前にも書いてる](../../../2021/12/semi-auto-property/index.md)通り、場合によっては C# 11 で入っていたかもしれないものです。

需要はそれなりに高いんですが、
案外課題があって結局スケジュール的に11からははずれ、「その後どうなったの？」とか思われていそうな機能です。
(12候補としては結構有力。)

半自動プロパティの話自体は[去年度](../../../2021/12/semi-auto-property/index.md)にしているので、
今日書くのはその「課題」をつらつらと。

## 半自動プロパティ概要

去年の繰り返しになるので概要のみ。
要は、手動で書く通常のプロパティ(以下、手動プロパティ)と自動プロパティの中間で、
バッキング フィールドのアクセスに `field` というキーワードを使おうというものです。

<pre class="source" title="手動、(全)自動、半自動プロパティ">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 手動プロパティ (manual property)</span>
    <span class="comment">// (と、自前で用意したフィールド)。</span>
    <span class="comment">// こういう、プロパティからほぼ素通しで値を記録しているフィールドを「バッキング フィールド」(backing field)という。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_x</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">value</span>; }

    <span class="comment">// 自動プロパティ (auto-property)。</span>
    <span class="comment">// 前述の X とほぼ一緒。</span>
    <span class="comment">// バッキング フィールドの自動生成。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Y</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="comment">// 【C# 12 候補】 半自動プロパティ (semi-auto-property)。</span>
    <span class="comment">// バッキング フィールドは自動生成。</span>
    <span class="comment">// 全自動の方と違って、バッキング フィールドの使い方は自由にできる。</span>
    <span class="comment">// field キーワードでバッキング フィールドを読み書き。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Z</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved"><em>field</em></span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved"><em>field</em></span> <span class="operator">=</span> <span class="reserved">value</span>; }
}
</pre>

## field の “キーワード性”

半自動プロパティに類する提案は他にもありつつも、
現状はとりあえず「`field` キーワード」案で話が進んでいます。
キーワード追加。

ところが、この世に出ている C# コードの中には「`field` という名前のフィールドや変数」がそれなりにあって(オープンソースになっているコードとかを検索すると相当量出てくるそうで)、さすがに「`field` を文脈抜きに無条件にキーワード扱い」とかやるのは、破壊的変更としては許容できるレベルを超えていて、現実的ではないです。
`field` という単語は、最近提案されている新機能の中では断トツで(`record` や `required` すら霞むくらい)影響力が大きいかもしれません。

一方で、文脈キーワードの仕様はなかなかに複雑になりがちで、
今、[ちょっと単純化したいという話もあるくらい](../../../2021/2/lexicalkeywords/index.md)です。
そんな中、半自動プロパティでは早速苦戦しそうな雰囲気。

半自動プロパティの `field` は、極限まで突き詰めて「有効な時だけキーワード扱い」をやろうとすると `var` とか `record` とかよりもだいぶ難しいみたいです。
一例として挙がっているのは以下のようなコード。

<pre class="source" title="field の有効性の循環">
<span class="reserved">unsafe</span> <span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">object</span> <span class="property">Prop</span>
    {
        <span class="reserved">get</span>
        {
            <span class="type struct">S</span> <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">new</span>();

            <span class="comment">// このステートメントは「構造体 S が unmanaged のときだけ有効」</span>
            <span class="comment">// 言い換えると、「構造体 S が参照型のフィールドを持たないときだけ有効」</span>
            <span class="comment">// (C# 11 からは警告のみになったものの、元々はエラー。)</span>
            <span class="reserved">var</span> <span class="variable">ptr</span> <span class="operator">=</span> <span class="operator">&amp;</span><span class="variable">s</span>;

            <span class="comment">// field が「S とは無関係な定数とか」だと &amp;s が有効。</span>
            <span class="comment">// ところが、field がキーワードで、バッキング フィールドが自動的に作られると &amp;s が無効になる。</span>
            <span class="comment">// 「&amp;s が無効にならないようにこれは認めない」みたいなことまでやるのは解析が「循環」してしまう。</span>
            <span class="control">return</span> field;
        }
    }
}
</pre>

なのであんまり正確にやるのはやめておいた方がいいとして、
簡素化した案でいうと以下のようなものがあります。

* セマンティクスを見るのは「`field` という名前のクラスと、`field` という名前のフィールドがあるかどうか」だけ
* あとは、構文的にだけ解析して、「スコープ内に `field` という名前の識別子がいるかどうか」で判定

簡素化するために「スコープを無視して解析」みたいな案もあるみたいなんですが、
結局は、以下のように「スコープも考慮に入れる」、「内側のスコープやローカル関数でのシャドーイングは認める」という予定だそうです。

<pre class="source" title="field キーワード/識別子のスコープ">
<span class="reserved">object</span> <span class="property">Prop</span>
{
    <span class="reserved">get</span>
    {
        {
            <span class="comment">// この field は {} 内でだけ有効。</span>
            <span class="reserved">int</span> <span class="variable">field</span> <span class="operator">=</span> <span class="number">1</span>;
        }

        <span class="comment">// このフィールドは m の内側でだけ有効。</span>
        <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">m</span></span>(<span class="reserved">int</span> <span class="variable local">field</span>) { } 

        <span class="comment">// {} とかローカル関数の外側には &quot;field&quot; がいないので、</span>
        <span class="comment">// ここの field はキーワード。</span>
        <span class="control">return</span> <span class="reserved">field</span>;
    }
}
</pre>

というのも、同スコープ内の解析に限っても、それなりに解析が大変そうな文法がいくつかあって、「労力は変わらない」とのこと。

<pre class="source" title="field のキーワード性の解析が大変そうなやつら">
<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">int</span> <span class="property">Prop</span>
    {
        <span class="reserved">get</span>
        {
            <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> (<span class="field">field</span>: <span class="number">1</span>, <span class="number">2</span>); <span class="comment">// タプル要素名</span>
            <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="reserved">new</span> { <span class="property">field</span> <span class="operator">=</span> <span class="number">1</span> }; <span class="comment">// 匿名型のプロパティ</span>
            <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Foo</span>() { <span class="field">field</span> <span class="operator">=</span> <span class="number">1</span> }; <span class="comment">// オブジェクト初期化子でのフィールド/プロパティ参照</span>
            <span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> { <span class="field">field</span>: <span class="number">1</span> }) { } <span class="comment">// プロパティ パターンでのフィールド/プロパティ参照</span>

            <span class="comment">// 上記の field はいずれも、field という名前の変数が新たに導入されたりはしない。</span>
            <span class="comment">// このスコープ内に &quot;field&quot; はいないので、ここの field はキーワードでいいはず。</span>
            <span class="control">return</span> <span class="reserved">field</span>;
        }
    }
}

<span class="reserved">class</span> <span class="type">Foo</span> { <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">field</span>; }
</pre>

## 初期化子の挙動

C# の構造体には「すべてのフィールドを初期化しきるまで関数メンバー(メソッドやプロパティ)を呼べない」という仕様がありました。
(ただし、[C# 11 で緩和されました](../../../../study/csharp/cheatsheet/ap_ver11.md#auto-default)。)

<pre class="source" title="すべてのフィールドの初期化が必須">
<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">int</span> <span class="field">_x</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() { }

    <span class="reserved">public</span> <span class="type struct">S</span>()
    {
        <span class="comment">// C# 10 まではコンパイル エラーになってた。</span>
        <span class="method"><span class="error" title="CS0188">M</span></span>(); <span class="comment">// _x の初期化より前</span>
        <span class="field">_x</span> <span class="operator">=</span> <span class="number">0</span>;
    }
}
</pre>

そんな中、C# 6 で[ get-only プロパティ](../../../../study/csharp/cheatsheet/ap_ver6.md#getter-only)の導入とともに、
「[コンストラクター内での自動プロパティへの代入は、それのバッキング フィールドへの直接代入への最適化を認める](../../../../study/csharp/cheatsheet/ap_ver6.md#struct-property-init)」という仕様も入っています。

<pre class="source" title="バッキング フィールドへの代入に展開">
<span class="reserved">struct</span> <span class="type struct">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="type struct">Point</span>(<span class="reserved">int</span> <span class="variable local">x</span>)
    {
        <span class="comment">// C# 5.0まではエラーに。</span>
        <span class="property">X</span> <span class="operator">=</span> <span class="variable local">x</span>;

        <span class="comment">// これを認めるために、X = x の部分は「Xのバッキングフィールド = x」に展開される。</span>
    }
}
</pre>

その流れで、プロパティ初期化子も「バッキング フィールドへの代入に展開」されます。
例えば以下のようなコードを書いたとします。

<pre class="source" title="プロパティ初期化子">
<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; } <span class="operator">=</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="type struct">S</span>() { }
}

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">R</span>(<span class="reserved">int</span> <span class="variable local">X</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">private</span> <span class="reserved">set</span>; } <span class="operator">=</span> <span class="variable local">X</span>;
}
</pre>

このコードは、以下のようなコードとほぼ同じ挙動になります。

<pre class="source" title="バッキング フィールドへの代入に展開">
<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_x</span>; <span class="reserved">private</span> <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">value</span>; }
    <span class="reserved">public</span> <span class="type struct">S</span>()
    {
        <span class="field">_x</span> <span class="operator">=</span> <span class="number">1</span>; <span class="comment">// X = 1 ではなくて、_x = 1</span>
    }
}

<span class="reserved">struct</span> <span class="type struct">R</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_x</span>; <span class="reserved">private</span> <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">value</span>; }

    <span class="reserved">public</span> <span class="type struct">R</span>(<span class="reserved">int</span> <span class="variable local">X</span>)
    {
        <span class="field">_x</span> <span class="operator">=</span> <span class="variable local">X</span>; <span class="comment">// this.X = X ではなくて、_x = 1</span>
    }
}
</pre>

という背景の中、半自動プロパティの場合はどうしようかという問題があります。
例えば以下のようなコードを認めたいんですが、
じゃあ、初期化時に `OnXChanged` は呼ばれるのかどうか。

<pre class="source" title="半自動プロパティのプロパティ初期化子">
<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="comment">// 流れ的にはこういうプロパティ初期化子も認めたい。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>;
        <span class="reserved">private</span> <span class="reserved">set</span>
        {
            <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
            <span class="method">OnXChanged</span>();
        }
    } <span class="operator">=</span> <span class="number">1</span>;

    <span class="reserved">public</span> <span class="type struct">S</span>() { }

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">OnXChanged</span>()
    {
        <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;何か副作用起こす&quot;</span>);
    }
}
</pre>

C# 11 での変更前は「自動プロパティと同様にせざるを得ない」と言われていました。
つまるところ、プロパティ初期化子はバッキング フィールドへの直代入に展開されて、
結果的に、`OnXChanged` は呼ばれないということになります。

C# 11 でこの要件は必然ではなくなったわけですが、
それでも「自動プロパティと同様」の仕様(`OnXChanged` は呼ばれない)になりそうな雰囲気です。

## override

override したときの挙動をどうしようかという問題もあります。
というのも、例えば以下のコードを考えます。

<pre class="source" title="自動プロパティの override">
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="comment">// 自動プロパティなので、バッキング フィールドが作られる。</span>
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// override してる時点で Base.Prop とは別物。</span>
    <span class="comment">// それをまた自動プロパティにすると、Base.Prop のものとは別に追加でバッキング フィールドができる。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</pre>

自動プロパティの作るバッキング フィールドは `Base` と `Derived` で独立しています。
さらに、virtual なプロパティは「`get` だけ override」みたいなことができます。

<pre class="source" title="get だけ override">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span> { <span class="property">Prop</span> <span class="operator">=</span> <span class="number">2</span> }; <span class="comment">// set は base.Prop のものがそのまま呼ばれる。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="property">Prop</span>);        <span class="comment">// get は Derived.Prop が呼ばれて、4 になる。</span>

<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// get だけ override して、base のものの二乗を返す。</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">base</span><span class="operator">.</span><span class="property">Prop</span> <span class="operator">*</span> <span class="reserved">base</span><span class="operator">.</span><span class="property">Prop</span>; }
}
</pre>

そんな中、半自動プロパティでの override はどうしよう？という話になります。

<pre class="source" title="半自動プロパティでの override">
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Derived</span> { <span class="property">Prop</span> <span class="operator">=</span> <span class="number">2</span> };
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span><span class="operator">.</span><span class="property">Prop</span>);

<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">virtual</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="comment">// get だけ override して(全)自動プロパティというのはできない。</span>
    <span class="comment">// じゃあ、get だけ &quot;半&quot;自動プロパティは？</span>
    <span class="comment">// これは Base.Prop とは別のバッキング フィールドになる？</span>
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">int</span> <span class="property">Prop</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">*</span> <span class="reserved">field</span>; }
}
</pre>

これはさすがにどう転んでもわかりにくいので、
いっそのこと、「半自動プロパティでの override はすべてのアクセサー(get/set 両方)の override が必須」とするそうです。

## nullability

半自動プロパティの導入の動機の1つに遅延初期化、
すなわち、以下のようなコードを書きたいというものがあります。

<pre class="source" title="遅延初期化目的の半自動プロパティ">
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">LazyInit</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Value</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="method"><span class="static">ComputeValue</span></span>();
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">string</span> <span class="method"><span class="static">ComputeValue</span></span>() { <span class="comment">/*...*/</span> }
}
</pre>

この用途の場合、バッキング フィールドの型は `string?` であるべきなんですよね。

ところが、現状は「半自動プロパティから作られるバッキング フィールドの型はプロパティの型と同じ」という仕様なので、`string` になります。

参照型に関しては元から [`?` の有無はフロー解析](../../../../study/csharp/resource/nullablereferencetype.md)の差だけなのでそこまで問題ではないんですが、
値型の場合は困ります。

<pre class="source" title="">
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">LazyInit</span>
{
    <span class="comment">// field も int なので、 ?? が意味をなさない。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Value</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="method"><span class="static">ComputeValue</span></span>();
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">ComputeValue</span></span>() { <span class="comment">/*...*/</span> }
}
</pre>

これは、「`field` キーワード」路線でやる以上は解決しようがなさそうで、
それとは別に「[プロパティ スコープ フィールド](https://github.com/dotnet/csharplang/issues/133)」(半自動プロパティと同じ要件に対する別案)が必要かもしれません。
とはいえ、とりあえず「`field` キーワード」優先で、
プロパティ スコープ フィールドはやるとしてもその後ということになっています。
