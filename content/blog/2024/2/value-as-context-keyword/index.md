---
title: "field, value を文脈キーワード化"
source_url: "https://ufcpp.net/blog/2024/2/value-as-context-keyword/"
content_type: "BlogEntry"
published_at: "2024-02-24T22:20:18"
updated_at: "2024-02-24T23:08:00"
tags: []
umbraco_id: 2488
parent_id: 2480
sort_order: 7
aliases: []
---

# field, value を文脈キーワード化

C# 13 向けに検討されている機能の一つに、
「半自動プロパティ」とか「field キーワード」と呼ばれているものがあります。
元々は C# 12 向けに考えられていて、去年、うちのブログでも書いているやつです。

* [【C# 12 候補】半自動プロパティ](../../../2023/1/semi-auto-property/index.md)

簡単におさらいすると、
プロパティの `get`/`set` アクセサー内で、`field` を使って
[バッキング フィールド](../../../../study/csharp/oop/oo_property.md#auto)(自動プロパティの値を保存するためにコンパイラーが生成するフィールド)に明示的にアクセスするというものです。

<pre class="source" title="半自動プロパティ案">
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

    <span class="comment">// 【C# 12 候補(改め、13 候補)】 半自動プロパティ (semi-auto-property)。</span>
    <span class="comment">// バッキング フィールドは自動生成。</span>
    <span class="comment">// 全自動の方と違って、バッキング フィールドの使い方は自由にできる。</span>
    <span class="comment">// field キーワードでバッキング フィールドを読み書き。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Z</span> { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>; }
}
</pre>

C# 12 時点では「これを破壊的変更なしで実装するのは大変」ということで見送りになりまして、
その結果検討されていたのが先日書いたブログの話。

* [C# での破壊的変更の今後の扱い (続報)](../breaking-changes/index.md)

ここで、「`field` の扱いで破壊的変更があるんだったら、`value` についても…」
という話が出ています。

* [[Proposal]: Field and value as contextual keywords #7964](https://github.com/dotnet/csharplang/issues/7964)

というのも、`value` (プロパティの `set` 内でだけ特別な意味を持つ)はちょっと C# 的には珍しく、
キーワードではなくて「暗黙に定義された引数」で、ちょっと浮いた挙動をします。

1つ目、[`@` で「脱キーワード化」](../../../../study/csharp/start/st_variable.md#identifier)ができない。

<pre class="source" title="キーワードじゃないので…">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">set</span>
        {
            <span class="comment">// value は @ を付けてもダメ。</span>
            <span class="comment">// 扱いが「暗黙定義された引数」なので、@value もその引数を指す。</span>
            <span class="reserved">var</span> <span class="variable"><span class="warning" title="CS0219"><span class="error" title="CS0136">@value</span></span></span> <span class="operator">=</span> <span class="number">1</span>;

            <span class="comment">// 普通、キーワードだったら @ を付けることで識別子に使える。</span>
            <span class="reserved">var</span> <span class="variable">@this</span> <span class="operator">=</span> <span class="number">2</span>;
        }
    }
}
</pre>

2つ目、[`nameof`](../../../../study/csharp/start/st_string.md#nameof-operator)。

<pre class="source" title="nameof が使える">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">set</span>
        {
            <span class="comment">// 逆に、引数扱いゆえに nameof が使える。</span>
            <span class="reserved">var</span> <span class="variable">n1</span> <span class="operator">=</span> <span class="reserved">nameof</span>(<span class="reserved">value</span>);

            <span class="comment">// キーワードには nameof は使えない。</span>
            <span class="reserved">var</span> <span class="variable">n2</span> <span class="operator">=</span> <span class="reserved">nameof</span>(<span class="reserved"><span class="error" title="CS8081">this</span></span>);
        }
    }
}
</pre>

3つ目、外側の識別子の参照。

<pre class="source" title="外にある「value フィールド」の参照ができない">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">int</span> <span class="field">value</span>;
    <span class="reserved">int</span> <span class="field">@this</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">set</span>
        {
            <span class="comment">// 外にある「value フィールド」すら、@value では参照できない。</span>
            <span class="comment">// 暗黙の引数の方になる(@ を付けるだけ無駄)。</span>
            <span class="comment">// (ちなみに、 this.value = 1; と書けばフィールド参照になる。)</span>
            <span class="reserved">@value</span> <span class="operator">=</span> <span class="number">1</span>;

            <span class="comment">// キーワードの場合は @this で外のフィールド参照になる。</span>
            <span class="field">@this</span> <span class="operator">=</span> <span class="number">2</span>;
        }
    }
}
</pre>

`field` を足すことで軽微ながら破壊的変更が出るんなら、
`value` に軽微な破壊的変更がかかってもいいのではということで、
もうこの際 `value` もキーワード(`set` 内限定なので、[文脈キーワード](../../../../study/csharp/appendix/ap_reserved.md#context))してもいいのではという話になります。
どういう影響があるかというと、先ほどの例からわかる通りで、

* `var @value = 1;` みたいなのが書けるようになる
  * これは、できないことができるようにあるので破壊的ではない
* `nameof(value)` が書けなくなる
  * こう書いていた人が多数派とは思えない
  * ※追記: `if (value is null) throw new ArgumentNullException(nameof(value));` って書く人それなりにいる説あり
* `@value = 1;` みたいなのが暗黙的引数の上書きから、外のフィールドの上書きに変わる
  * 単に `value = 1;` でよかったわけで、もともと変

となります。

ちなみに、`field` は「暗黙の引数扱い」でも「文脈キーワード扱い」でもどちらにしろ破壊的変更になります。
「文脈キーワード扱い」の方が自然っぽいんですが、
そうなるとこの「`value` と何か挙動が違う」が気になるという懸念がありまして。
そこで出た対案が「`value` も文脈キーワードに変更」という感じかと思います。
