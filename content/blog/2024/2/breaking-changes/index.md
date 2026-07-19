---
title: "C# での破壊的変更の今後の扱い (続報)"
source_url: "https://ufcpp.net/blog/2024/2/breaking-changes/"
content_type: "BlogEntry"
published_at: "2024-02-10T19:55:38"
updated_at: "2024-02-10T19:55:38"
tags: []
umbraco_id: 2482
parent_id: 2480
sort_order: 1
aliases: []
---

# C# での破壊的変更の今後の扱い (続報)

[去年の3月にブログに書いた](../../../2023/3/csharp-breaking-change/index.md)ものの続報。

C# でも限定的に破壊的変更を許していこうかという話だったわけですが、
ちょっと具体化しました。

ある機能を実現するにあたって破壊的変更の原則と進め方についての話をしています。

## 破壊的変更の候補

C# 13 で導入したい `field` アクセス(自動プロパティのバッキングフィールドにアクセスするための `field` キーワード)と、
これまでに破壊的変更を避けるためにちょっと変な設計になっている `var` (型推論変数宣言)、`_` (discard)が検討の対象になっています。


## 破壊的変更を認める基準

1. あくまで控えめな破壊的変更で、エンドユーザーに明確なメリットがある
2. 破壊的変更を踏むようなコードは割かしレア
3. 破壊的変更を起こす予定のコードはどういう理由でどこが問題で、どう直せばいいかが明確に示せる
4. 破壊的変更を避けられるよう、完全に自動で、簡単で、堅牢で、局所的な code-fix が提供できる

### field の場合

`field` アクセスは以下のような話。

<pre class="source" title="field アクセス">
<span class="reserved">class</span> <span class="type">こういうのを</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_x</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">.</span><span class="static"><span class="method">Min</span></span>(<span class="reserved">value</span>, <span class="number">0</span>);
    }
}

<span class="reserved">class</span> <span class="type">こう書きたい</span> <span class="comment">// C# 13 候補</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Min</span></span>(<span class="reserved">value</span>, <span class="number">0</span>);
    }
}

<span class="reserved">class</span> <span class="type">こういうコードで困る</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">field</span>;
    <span class="comment">// ↑「このフィールドがないときだけ field をキーワード扱いする」みたいなことすると使い勝手が悪くなる。</span>

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">field</span>; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">field</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">.</span><span class="static"><span class="method">Min</span></span>(<span class="reserved">value</span>, <span class="number">0</span>);
    }
}
</pre>

これは以下のように、前述の基準を満たします。

1. `field` アクセスが欲しいという要望は多い。「`field` フィールドがないときだけ」とやると構文が複雑になるし、使い勝手も悪くなる
2. 「`field` という名前のフィールド」はなくはないだろうけども多くはないし、問題になるのはプロパティのアクセサー内だけ
3. 「`field` が将来キーワードになる」(から使うな)という明確な説明ができる
4. 型名や `this` を付けて `A.field` とか `this.field` と書くように変えればいい

### var の場合

[C# 3.0 の頃からある `var`](../../../../study/csharp/cheatsheet/ap_ver3.md#functional)ですが、
有名な話、「`class var { }` とかいう型をどこかに書いておけば、型推論の `var` を阻害できる」という問題があります。

<pre class="source" title="var">
<span class="comment">// 普通は型推論の var になるはず。</span>
<span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">1</span>;

<span class="comment">// が、こういうことをすると var x の意味が変わってしまう。</span>
<span class="reserved">class</span> <span class="type"><span class="warning" title="CS8981">var</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">var</span>(<span class="reserved">int</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
}
</pre>

嫌がらせでしかないんですが、
昔は「型推論とか怖いから嫌がらせしてやれ」と言っちゃう人が実際いたとか…

今でも「型推論は嫌」ということをもう人はいるとは思いますが、
その場合も今は[ソースコード分析](https://learn.microsoft.com/ja-jp/dotnet/fundamentals/code-analysis/overview)の設定を変えて警告なりエラーにできるようになっているので「`class var { }`」みたいな変なことをする必要はありません。

なので、もう今となってはこれも破壊的変更してでも「`var` は常に型推論」にしてしまっていいのではないかという話になります。

これについての前述の基準:

1. 「`class var { }`みたいなものは実用的じゃない。その割に `var` を常にキーワード扱いできないのは構文ハイライトとかで結構困る
2. 嫌がらせ以外で「`class var { }`」を書く人もいない
3. 「`var` という名前の型は作るな」と説明できる
4. もしどうしても「`var` 型」を作りたければ `@var` と書けばいい

### _ の場合

[C# 7 で discard が導入された](../../../../study/csharp/cheatsheet/ap_ver7.md#discard)わけですが、
これも「`_` を普通に変数として使っていないときに限り、`_` が discard の意味になる」という挙動になっています。

<pre class="source" title="_ (discard)">
<span class="reserved">void</span> <span class="method">m1</span>(<span class="reserved">int</span> <span class="variable local">i</span>, <span class="reserved">string</span> <span class="variable local">s</span>)
{
    <span class="comment">// これはいずれも discard。</span>
    (<span class="reserved">_</span>, <span class="reserved">string</span> <span class="reserved">_</span>) <span class="operator">=</span> (<span class="variable local">i</span>, <span class="variable local">s</span>);
    <span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">TryParse</span></span>(<span class="variable local">s</span>, <span class="reserved">out</span> <span class="reserved">_</span>);
}

<span class="reserved">void</span> <span class="method">m2</span>(<span class="reserved">int</span> <span class="variable local">i</span>, <span class="reserved">string</span> <span class="variable local">s</span>)
{
    <span class="reserved">var</span> <span class="variable">_</span> <span class="operator">=</span> <span class="variable local">i</span>; <span class="comment">// これがあるせいで…</span>

    (<span class="variable">_</span>, <span class="reserved">string</span> <span class="reserved">_</span>) <span class="operator">=</span> (<span class="variable local">i</span>, <span class="variable local">s</span>); <span class="comment">// ここの1個目の _ は変数。</span>
    <span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">TryParse</span></span>(<span class="variable local">s</span>, <span class="reserved">out</span> <span class="variable">_</span>); <span class="comment">// ここの _ は変数。</span>
}

<span class="reserved">void</span> <span class="method">m3</span>(<span class="reserved">int</span> <span class="variable local">i</span>, <span class="reserved">string</span> <span class="variable local">s</span>)
{
    <span class="reserved">var</span> <span class="variable">_</span> <span class="operator">=</span> <span class="variable local">i</span>;
    <span class="reserved">var</span> <span class="variable"><span class="error" title="CS0128">_</span></span> <span class="operator">=</span> <span class="variable local">s</span>; <span class="comment">// これは「同じ名前の変数がすでにある」エラー。</span>

    (<span class="variable">_</span>, <span class="reserved">string</span> <span class="reserved">_</span>) <span class="operator">=</span> (<span class="variable local">i</span>, <span class="variable local">s</span>);
    <span class="reserved">int</span><span class="operator">.</span><span class="static"><span class="method">TryParse</span></span>(<span class="variable local">s</span>, <span class="reserved">out</span> <span class="variable">_</span>);
}
</pre>

これについての前述の基準:

1. 今のままだといつ `_` が discard になるかわかりにくすぎる
2. 元々 `_` を変数・引数として使っていた人も、「値を特に読まない」(なのでほんとは discard にしたい)という意味でこの名前を使うことが多い
3. 「`_` が常に discard の意味になる」と説明できる
4. `@_` と書けば「`_` という名前の変数」を書ける

## 破壊的変更の影響を軽減

破壊的変更に対応しやすくするため、
C# *N* に対応したコンパイラーを使ったとき、
まだ C# *N - 1* 以下だった場合に警告と code-fix を提供したいとのこと。

現在、`LangVersion` を明示しなかった場合、
.NET SDK が「`TargetFramework` に応じた言語バージョンを自動選択する」という挙動になっています。

なので、例えば以下のような流れで比較的安全にバージョンアップができます。

* .NET 9 SDK をインストールすると C# 13 対応コンパイラーになる
* この時点で既存のプロジェクトは `net8.0` とかがターゲットになっているはずで、C# 12 が選ばれる
* 「C# 13 対応コンパイラーで C# 12 を利用」状態なので、`field` に関する警告が出る
* 警告を直してから `net9.0` ターゲットに上げると安全にバージョンアップができる

ただ、この手の警告の追加自体が破壊的変更
(警告は必ず取る方針であったり、なんなら WarnAsError オプションでエラーにできる)なので、
年に1回のメジャーバージョンアップ時以外には警告追加しないとのこと。

C# のバージョンアップを予定していない人向けに警告抑止の手段の提供や、
もしかしたら「先送りはするけどいつかバージョンアップしたい」人向けに「30日だけ警告を止める」みたいな手段を提供するのがいいかもしれないという話も出ています。

### LangVersion latest, preview

[LangVersion](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) latest にすると、
常に C# コンパイラーが対応している最新の C# バージョンになります。
こうなると先ほどの「C# *N* 対応コンパイラーで C# *N - 1*」という状態が起きなくなるので、
「言語バージョンアップ前に破壊的変更を修正」ということができません。
なので、latest は今後非推奨にして、使っていたら警告を出すことを検討しているそうです。

一方で、LangVersion preview はわかってて使っている人柱向けですし、
プレビュー提供している言語機能は普通にリリースまでに破壊的変更がかかることもあって、
元から破壊的変更は覚悟の上で使っているはずです。
なので、preview に対しては特に問題視はしないそうです。
