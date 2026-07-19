---
title: "C# 8.0 switch 式"
source_url: "https://ufcpp.net/blog/2018/12/cs8switchexpr/"
content_type: "BlogEntry"
published_at: "2018-12-09T12:05:08"
updated_at: "2018-12-09T12:18:03"
tags: []
umbraco_id: 2190
parent_id: 2177
sort_order: 8
aliases: []
---

# C# 8.0 switch 式

今日は `switch` 式の話。
ステートメントではなく、式。
`var y = x switch { ... }` みたいに書ける構文です。

C# 8.0 候補の中でも割と早い段階に実装されていて、
「the patterns and ranges preview」とかいって[Preview 公開](https://github.com/dotnet/csharplang/wiki/vNext-Preview)もされていました。
(後述するように、`switch`式は「patterns」のおまけです。)

なのでてっきり、Visual Studio 2019 Preview でもまず真っ先に入ると思っていたんですが。
なぜか Preview 1 には入らなかったという…

(たぶん、パターン マッチングにまだもうちょっと調整したいことができたから？)

# 新しい switch

C# の `switch` ステートメント(C# 1.0 の頃からあるやつ)は、
C 言語系の影響を受けすぎている感じがあり、
使いにくいものでした。
C# 1.0 当時の情勢から言うと C 言語の `switch` を意識するのはしょうがなかったと思いますが、
それから15年以上経った今となってはちょっとしんどい文法です。

例えばこれまでだと、以下のような書き方を時々見ると思います。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">void</span> M(<span class="type">年号</span> e)
{
    <span class="reserved">int</span> y;
    <span class="reserved">switch</span> (e)
    {
        <span class="reserved">case</span> 明治:
            y = 45;
            <span class="reserved">break</span>;
        <span class="reserved">case</span> 大正:
            y = 15;
            <span class="reserved">break</span>;
        <span class="reserved">case</span> 昭和:
            y = 64;
            <span class="reserved">break</span>;
        <span class="reserved">case</span> 平成:
            y = 31;
            <span class="reserved">break</span>;
        <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
    }
    <span class="comment">// y を使って何か</span>
}
</code></pre>

しんどい理由は、

- それぞれの条件で1つずつ値を返したいだけなのにステートメントを求められる
- `break` が必須
- `case` ラベルもうざい

という辺り。
以下のように別メソッドを1段挟めば多少緩和するようなそうでもないような…

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">void</span> M(<span class="type">年号</span> e)
{
    <span class="reserved">int</span> lastYear()
    {
        <span class="reserved">switch</span> (e)
        {
            <span class="reserved">case</span> 明治: <span class="reserved">return</span> 45;
            <span class="reserved">case</span> 大正: <span class="reserved">return</span> 15;
            <span class="reserved">case</span> 昭和: <span class="reserved">return</span> 64;
            <span class="reserved">case</span> 平成: <span class="reserved">return</span> 31;
            <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>();
        }
    }
 
    <span class="reserved">var</span> y = lastYear();
    <span class="comment">// y を使って何か</span>
}
</code></pre>

一方、C# 8.0 で、`switch` に式として使えるバージョンが追加されます。
この「`switch` 式」を使えば、以下のように書き直せます。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">void</span> M(<span class="type">年号</span> e)
{
    <span class="reserved">var</span> y = e <span class="reserved">switch</span>
    {
        明治 =&gt; 45,
        大正 =&gt; 15,
        昭和 =&gt; 64,
        平成 =&gt; 31,
        _ =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidOperationException</span>()
    };
}
</code></pre>

`case`や`break`が消えてちょっとすっきり。

## 提案上はパターン マッチングの一部

今の C# の文法は、[csharplang リポジトリ](https://github.com/dotnet/csharplang)上で提案があって、採用することに決まったものは [Milestone](https://github.com/dotnet/csharplang/milestones) で管理されていたりします。

が、`switch` 式はぱっと見 [C# 8.0 Milestone](https://github.com/dotnet/csharplang/milestone/8) に並んでいない。
なぜかというと、パターン マッチングの一部として提案されたから。
[パターン マッチングの方の提案 issue](https://github.com/dotnet/csharplang/issues/45)を覗いてみれば、その中に「switch expression」の文字もあります。
実装も、[features/recursive-patterns](https://github.com/dotnet/roslyn/tree/features/recursive-patterns)ブランチ内で行われていたり。

けど、検索性は最悪なんですよね。
ただでさえ「issue の9割は重複か実現不能な提案」とか陰口言われるレベルの csharplang なのに、検索性が悪いとかちょっと…
実際、もう実装された後になって、「`switch` 式が欲しい」という重複提案が何度かありました…

## 後置き記法

書き方が `e switch {}` というちょっと変わった記法。

ステートメントの方の `switch` との弁別の意味もあるみたいです。`switch (e)` から初めてしまうとステートメントと混同してしまう。
かといって、新たに別キーワードを導入(例えば `match (e) ...`)するのもいまいち評判がよくなく。
(ちなみに、当初案はこの`match`キーワードの導入でした。
なので、長らくこの機能は「`match` 式」と呼ばれていました。)

あと、基本的に前置きの記法は評判が悪いです。
否定の`!`とかキャストとか。
csharplangのissueでも、「後置きキャスト文法が欲しい」という提案もよく見ます。
わざわざ `As` なんとかみたいな名前の拡張メソッドを生やすこともよくあります。
`!`の方も、`Not()`とか`Negate()`みたいな拡張メソッドを書いたことある人、結構いるんじゃないでしょうか。

なんか、前置きって書きにくいんですよねぇ。どうしても、「先に式を書いたうえで、カーソルを前に戻して、改めて`!`とかを入力する」みたいな書き方をしてしまい、そのカーソルを戻す作業がストレス。

そんな感じのこと、みんな思っているようで、`swtich`式も後置きの `e switch {}` になりました。

## `=>`

`case` の方も提案ではいろいろとバリエーションがありました。

`case` の区切り:

- `,` 区切り
- `;` 区切り

`case` の書き方

- `x: experssion`
- `x -> experssion`
- `x ~> experssion`
- `x => experssion`

以下のように、`when`句に条件演算子`?:`を書いたり、
返す値にラムダ式を書いたりできるので、`:`とか`=>`とかも案外弁別に悩む選択肢です。

<pre class="source" title="">
<code><span class="reserved">var</span> i = x <span class="reserved">switch</span>
{
    <span class="reserved">string</span> s <span class="reserved">when</span> (s.Length &gt;= 1 ? s[0] &lt;= 0x7F : <span class="reserved">true</span>) =&gt; () =&gt; <span class="reserved">true</span>,
    _ =&gt; (<span class="type">Func</span>&lt;<span class="reserved">bool</span>&gt;)(() =&gt; <span class="reserved">false</span>)
};
</code></pre>

(ちなみにこのコード、`()`がないとコンパイルが通りません。
`()`の入れ子弁別している模様。)

かといって見慣れない(ポインター用な)`->`とか、
ほんとに新たに追加する必要がある`~>`とかもだいぶ気持ち悪いです。

結局、アンケート的なのを取った結果、最終的に `x => expression,` に決まりました。
