---
title: "C# での破壊的変更の今後の扱い(案)"
source_url: "https://ufcpp.net/blog/2023/3/csharp-breaking-change/"
content_type: "BlogEntry"
published_at: "2023-03-11T23:49:16"
updated_at: "2023-03-11T23:49:17"
tags: []
umbraco_id: 2459
parent_id: 2457
sort_order: 1
aliases: []
---

# C# での破壊的変更の今後の扱い(案)

C# は、進化していくにあたって、破壊的変更を極力起こさないようにかなり気を使っているプログラミング言語です。
細かい話をすると破壊的変更も皆無ではないんですが、
破壊的変更を認める(認めてでも追加したい新機能を実装する)ハードルは結構高めです。

そんな C# ですが、ちょっとそのハードルの基準を緩められないかというような話が出ています。

* [Dealing with limited breaking changes in C#](https://github.com/dotnet/csharplang/discussions/7033)
* [その後の Design Meeting 議事録](https://github.com/dotnet/csharplang/blob/main/meetings/2023/LDM-2023-03-08.md#limited-breaking-changes-in-c)

## 補足: 影響範囲と、影響力の軽減

補足として、
ハードルを緩めるといっても本当にちょっとです。
C# チームは、「GitHub の public リポジトリを検索して、実際に影響を受けたコードを探す」とかやって既存のコードに対する影響を評価してたりするんですが、

* これまで: 単体テストとかでわざと変なコードを書いているものを除いて、ほぼ影響皆無なら OK
* 提案: それほど多くはないものの、無視できると言えるほど皆無ではないものでも OK にしたい

みたいな感じ。

代わりといってはなんですが、影響を受ける人への負担を最小限にするために、以下のような仕組みを提供するのはどうか？という提案になっています。

* 言語バージョンを最新のものにアップグレードすると影響を受けるコードを識別する
* そういうコードに対して診断メッセージを出して、破壊的変更があることを知らせる
* 自動コード修正機能で、破壊的変更を受けないようなコードへの書き換えを提供する
* 早い段階でこれらの診断・コード修正を提供する

## これまでの破壊的変更の例

[件の discussion](https://github.com/dotnet/csharplang/discussions/7033)で触れられているわけではないですが、
補足的に、
これまでの「ほぼ影響皆無」な破壊的について紹介しておきましょう。
細かく言うともっといろいろとあるんですが、結構大きめのもののみ抜粋。

### ジェネリクスの <>

C# のジェネリクスは C# 2.0 からの導入なわけで、それ以前には `M<T>()` みたいな `<>` の用法はありませんでした。
ここで、多少工夫すると、C# 1.0 の頃でも合法そうな `<>` が書けます。
例えばこんな感じ:

<pre class="source" title="&lt;&gt;">
X(A&lt;B, C&gt;(D));
</pre>

* C# 1.0 の解釈: 2引数のメソッド `X` があって、式 `A<B` と `C>(D)` が引数
* C# 2.0 の解釈: 1引数のメソッド `X` と、引数1つで型引数2つのメソッド `A` がある

色が付くと多少わかりやすいですかね。

<pre class="source" title="&lt;&gt; 1.0 VS 2.0">
<span class="comment">// C# 1.0 解釈</span>
<span class="method">X</span>(<span class="variable">A</span> &lt; <span class="variable">B</span>, <span class="variable">C</span> &gt; (<span class="variable">D</span>));

<span class="comment">// C# 2.0 解釈</span>
<span class="method">X</span>(<span class="method">A</span>&lt;<span class="type">B</span>, <span class="type">C</span>&gt;(<span class="variable">D</span>));
</pre>

まあ、狙わないと踏めないですね。
C# 2.0 当時に踏んだ人はいないんじゃないでしょうか。
実際僕も確か、C# 5.0 辺りの時に「かつてこんなのあったけども誰も気にしなかったよ」的な話題で知りました。

### foreach 変数のキャプチャ

割かしちゃんとアナウンスがあった破壊的変更でいうと、C# 5.0 のときの [`foreach` の仕様変更](../../../../study/csharp/cheatsheet/ap_ver5.md#foreach)があります。
詳細はリンク先を見てもらうとして、
簡単に言うと以下のコードの実行結果が C# 4.0 以前と 5.0 以降で変わります。

<pre class="source" title="C# 5.0 の foreach の仕様変更">
<span class="reserved">var</span> <span class="variable">data</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>, <span class="number">4</span>, <span class="number">5</span> };

<span class="type">Action</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">null</span>;

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable">data</span>)
{
    <span class="variable">a</span> <span class="operator">+=</span> () <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">x</span>);
}

<span class="variable">a</span>();
</pre>

`x` のスコープが `foreach` の内側か外側かが変わっていて、
単一の変数 `x` が全てのループで共有されるか、ループごとに違う変数扱いになるかが変わります。
結果的に、(C# 4.0 以前)「5つの5が表示される」か(C# 5.0 以降)「1, 2, 3, 4, 5 が表示される」かという結構大きな差になります。

まあ、4.0以前の挙動の方をバグだと思う人もいたくらいです。
このコードを書いてみて5が5つ表示されたら、まあ、コードを書き換えますよね、普通。
なので、破壊的変更の影響を受ける人はほぼ皆無でした。

この当時はまだ「GitHub にあるコードをクロールして調べる」みたいな手段がなかったので、
C# チーム的には恐る恐る破壊的変更をリリースしていました。
ですが、まあ、結果的には「心配しすぎだった」と言われているくらい、不平不満の声はなかったはずです。
繰り返しになりますが、バグ修正とすら思われているレベルです。

### record, required, scoped, file

C# 9.0 で[`record`](../../../../study/csharp/cheatsheet/ap_ver9.md#record)が、
C# 11.0 で[`required`](../../../../study/csharp/cheatsheet/ap_ver11.md#required)、[`scoped`](../../../../study/csharp/resource/refstruct.md#scoped)、[`file`](../../../../study/csharp/cheatsheet/ap_ver11.md#file-local)が新たにキーワードになりました。

ただ、幸い、これらは(当然、[文脈キーワード](../../../../study/csharp/misc/ap_compatibility.md#contextual-keyword)で)「型名として使おうとする時だけまずい」という仕様になっています。

<pre class="source" title="record の破壊的変更の影響は型名に対してのみ">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="comment">// 全然平気。</span>
    <span class="reserved">int</span> <span class="field">record</span>;
    <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable local">record</span>) { }
    <span class="reserved">int</span> <span class="method">M</span>()
    {
        <span class="reserved">int</span> <span class="variable">record</span> <span class="operator">=</span> <span class="number">0</span>;
        <span class="control">return</span> <span class="variable">record</span>;
    }

    <span class="comment">// これがダメ。</span>
    <span class="comment">// 以前:  record という名前のクラスのフィールド x</span>
    <span class="comment">// C# 11: x という名前のレコード型宣言</span>
    <span class="reserved">record</span> <span class="type"><span class="warning" title="CS8981">x</span></span>;
}

<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8860">record</span></span> { }
</pre>

幸い、C# では「型名は大文字始まりにする」という文化が浸透していて、わざわざこの規約に反する型名を使う人もほとんどいません。

昔ならそれでも破壊的変更はしり込みしたんでしょうが、
今回は「GitHub にあるコードをクロールして調べる」が有効に機能したようです。
調べた結果、デモやテストでわざと変な名前をつけている人を除いて、問題を起こしそうなコードは見当たらなかったそうです。

実際、C# 9.0 リリース後にこれで困ったという人は見かけません。
それもあってか、C# 11.0 では、[そもそも `required`、`scoped`、`file` という名前の型宣言自体エラーに](../../../../study/csharp/cheatsheet/ap_ver11.md#CS9029)しました。
結構な破壊的変更ですが、これで困ったという人は、僕の知る限りは見かけたことはありません。

(1個だけ、native interop で、native 側に `file` という構造体がいて、
それに合わせて「C# でも意図的に小文字始まりの `file` を使う」みたいな判断をしていたコードは見たことがあります。それは `struct @file {}` と書けば解決。)

## 今懸念される新機能: 半自動プロパティ

今何で困っているかというと、1月にブログに書いた[半自動プロパティ](../../1/semi-auto-property/index.md)です。
`field` キーワードの追加。

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

`record` とかと違ってこれが危ないのは、「`field` という名前のフィールドがいたらアウト」という、割かしありそうなラインなせいです。
以下のコード、半自動プロパティが実装される前後で意味が変わる可能性が大きくなっています。
(回避できなくもないものの、コストが高すぎてできれば破壊的変更を認める方向で進めたい。)

<pre class="source" title="半自動プロパティで壊れる予定のコード">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">field</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }
}
</pre>

これはGitHubで調べたら、いるらしいです。
まあ、いそうですよね。

ただ、そんなに多くもない。
安直な `field` という名前のフィールドがそこまで多くないというのもありますが、
C# のコーディング規約上の派閥的な話もあります。
フィールドの命名規約として「`_` を付ける派」は影響を受けません。

<pre class="source" title="_ 派">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_field</span>; <span class="comment">// _ 派。影響を受けない。</span>

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }
}
</pre>

「インスタンス メンバーには常に `this.` を付ける派」も影響を受けません。

<pre class="source" title="this. 派">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">field</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="comment">// this. 派。影響を受けない。</span>
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">this</span><span class="operator">.</span><span class="field">field</span>;
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">this</span><span class="operator">.</span><span class="field">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
    }
}
</pre>

C# は結構「`private` なところのコーディング規約は口うるさく言わない」みたいなところがあるので、フィールドに関しては `_field`、`this.field`、`field` の3つとも結構います。

さて、このラインの「大した影響ではないものの、無視できるほどは皆無じゃない」をどう扱いましょうか。
というのが現在の課題。

## さかのぼって

「`field` フィールド」程度の破壊的変更を認めたいのであれば、
過去のさかのぼれば、同程度の以下の影響範囲だけどもちょっと特殊対応して破壊的変更を避けたものがあります。

* `var`: `var` という名前の型がないときに限りキーワード扱い
* `dynamic`: `dynamic` という名前の型がないときに限りキーワード扱い
* `_`: 1つも変数参照がないときに限り [discard](../../../../study/csharp/cheatsheet/ap_ver7.md#discard) 扱い

特に前2者なんて、`required` や `scopde` が型名として使えなくなった今、かなり不自然ですよね。

かつては「型推論の `var` を使わせないために、わざと `class var {}` を定義しておく」という嫌がらせのような規約を定めてしまう人も一部いたそうですが。
今では「そんなことやるのは推奨されていない」で一蹴していいと思います。

## 改めて、破壊的変更の影響軽減

とりあえず差し当たっては「`field` フィールド」問題、もしかするとさらに踏み込んで「`var` 型」問題を、今後、破壊的変更を認める方向で進めることになるかもしれません。

さすがにサイレントに行うには大きすぎる破壊的変更なので、以下のように進めたいとのこと。

* コンパイラーを最新にした場合、言語バージョンを更新しなくても(TargetFramework を最新にしなくても)、「最新の C# で破壊的変更になる」旨を警告する
    * 言語バージョンを上げるつもりのない人向けに、抑止オプションも提供する
* 自動コード修正を提供して、早期に修正してもらう
    * 半自動プロパティの例でいうと `field` を `this.field` に自動的に置き換える
* このコード修正は IDE 上でも、コマンドラインでの実行もできるようにする

[discussion](https://github.com/dotnet/csharplang/discussions/7033) での反応は「賛成多数」(👍100 対 👎4)。
むしろ、「他の言語はもっと破壊的変更してるだろ。やっちゃえ」発言も目立ちます。
ただ、この discussion に参加しに来る人はその時点で「積極的な人」のはずなので、
もう少しいろんな方面の調査は必要かと思われます。

また、1つ disucussion 内で挙げられた懸念として、
「StackOverflow とかからコピペしてくるコード問題」があります。
コード片のコピペの場合、「どのバージョンのコピーを、どのバージョンのコンパイラーにペーストするか」がわからないので、「事前に警告して、事前にコード修正をかけてもらう」戦略がやりにくいです。
(こういう問題も、[top-level statemsnt](../../../../study/csharp/cheatsheet/ap_ver9.md#top-level-statements)ですでに経験済み。)
