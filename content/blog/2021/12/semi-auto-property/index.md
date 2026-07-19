---
title: "【C# 11 候補】 半自動プロパティ"
source_url: "https://ufcpp.net/blog/2021/12/semi-auto-property/"
content_type: "BlogEntry"
published_at: "2021-12-27T15:16:10"
updated_at: "2021-12-28T15:34:32"
tags: []
umbraco_id: 2393
parent_id: 2375
sort_order: 8
aliases: []
---

# 【C# 11 候補】 半自動プロパティ

[11月くらいからなんとか消化し始めた](../../11/2022-ga-soon/index.md)「[C# ライブ配信](https://www.youtube.com/channel/UCY-z_9mau6X-Vr4gk2aWtMQ)で口頭では言ったけどブログ化はしてなかったやつ」、
「C# 10.0 の補足」とか、文字コード・絵文字がらみの雑談話を抜けて、
やっと「C# 11.0 候補」の話になります。

こんな時間かかるかー…

## 半自動プロパティ

C# 11 目標で、自動プロパティにちょっと手が入りそうです。

[バッキング フィールド](../../../../study/csharp/oop/oo_property.md#auto)を `field` キーワードで読み書きできるようにするというもの。
俗称「半自動プロパティ」。

## おさらい: 初期 C# のプロパティ

C# 1.0 の頃からの一番煩雑な書き方だとプロパティは以下のように書いていました。追加でフィールドが1個必要。

<pre class="source" title="C# 初期のプロパティ">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _x;
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span> { <span class="control">return</span> _x; } <span class="reserved">set</span> { _x = <span class="reserved">value</span>; } }
}
</code></pre>

それに対して C# 3.0 で書けるようになった簡易記法が自動プロパティ(automatically implemented property、通称 auto-property)。
`get; set;` だけ書くと、上記の `_x` フィールド相当のものを自動的に作ってくれます。

<pre class="source" title="C# 3.0 の自動プロパティ">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>

## 自動プロパティが使えなかったものの例

(この後もプロパティは細かく色々な改善があるんですが、それは置いておいて)

C# 3.0～10.0 までの “完全に自動な” プロパティだと一部の頻出する用途に使えなくて、結局は自前でフィールドを用意しないといけないことがありました。
特に有名な例を2つ挙げると、

1． PropertyChanged

<pre class="source" title="PropertyChanged のためにフィールドが必要">
<code><span class="reserved">using</span> System.ComponentModel;
<span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">class</span> <span class="type">A</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _x;
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span> =&gt; _x; <span class="reserved">set</span> =&gt; <span class="method">SetProperty</span>(<span class="reserved">ref</span> _x, <span class="reserved">value</span>); }

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">PropertyChangedEventHandler</span>? PropertyChanged;

    <span class="reserved">protected</span> <span class="reserved">void</span> <span class="method">SetProperty</span>&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> <span class="variable">storage</span>, <span class="type">T</span> <span class="variable">newValue</span>, [<span class="type">CallerMemberName</span>] <span class="reserved">string</span>? <span class="variable">propertyName</span> = <span class="reserved">null</span>)
    {
        <span class="variable">storage</span> = <span class="variable">newValue</span>;
        PropertyChanged?.<span class="method">Invoke</span>(<span class="reserved">this</span>, <span class="reserved">new</span>(<span class="variable">propertyName</span>));
    }
}
</code></pre>

2. 遅延初期化

<pre class="source" title="初回限りの重たい処理を、プロパティの初アクセス時に呼びたい">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">string</span>? _x;
    <span class="reserved">public</span> <span class="reserved">string</span> X =&gt; _x ?? <span class="method">GetX</span>();

    <span class="reserved">private</span> <span class="reserved">string</span> <span class="method">GetX</span>()
    {
        <span class="comment">// 初回限りの重たい処理</span>
    }
}
</code></pre>

## field キーワードの追加

で、要望自体は結構昔からあったんですがようやく C# 11.0 で採用されそうなのが「`field` キーワード」。

例えば前節の例は以下のように書けます。

1． PropertyChanged

<pre class="source" title="">
<code><span class="reserved">class</span> <span class="type">A</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span> =&gt; <em><span class="reserved">field</span></em>; <span class="reserved">set</span> =&gt; SetProperty(<span class="reserved">ref</span> <em><span class="reserved">field</span></em>, <span class="reserved">value</span>); }

    <span class="comment">// 以下元と同じ</span>
}
</code></pre>

2. 遅延初期化

<pre class="source" title="field キーワードで遅延初期化">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> X =&gt; <em><span class="reserved">field</span></em> ?? <span class="method">GetX</span>();

    <span class="comment">// 以下元と同じ</span>
}
</code></pre>

## 細々補足

以下のような補足あり。

* C# らしく(破壊的変更を避けて)、`field` は文脈キーワード
  * `field` と言う名前のフィールドがない場合だけキーワード扱い
* キーワード扱いを受けた場合、`nameof(value)` はコンパイルできないという仕様。
* `get` 側しかない場合は get-only プロパティと同様
  * コンストラクターでだけ `set` 可能
  * 生成されるフィールドは `readonly` 扱い

この新機能、俗称としては「半自動プロパティ」(semi-auto-property)なんですが、実装上・仕様書上は「自動プロパティの項目を修正」みたいです。

元:

* セミコロンのみの `get;` `set;` しかないプロパティを自動プロパティと呼ぶ

変更後:

* 以下の2つを自動プロパティと呼ぶ
  * セミコロンのみの `get;` `set;` アクセサーしかないプロパティ
  * アクセサー内で `field` キーワードを使っているプロパティ

## おまけ field はキーワードで value は変数？キーワード？

ちょっと余談。

field は明確に「文脈キーワード」です。補足説明の通り、`nameof(field)` 不可。

ところで、じゃあ、C# 1.0 の頃からある `value` はと言うと…
とりあえず、Visual Studio 上では「青」(キーワード扱いの色)です。
(↓ うちのサイトの色付けは Visual Studio 初期設定準拠。)

<pre class="source" title="value は青">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _x;
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">set</span> =&gt; _x = <span class="reserved">value</span>; }
    <span class="comment">// ↑ Visual Studio 上、value の文字は青(キーワードの色)になってる。</span>
}
</code></pre>

ところで、この `value`、仕様書上は「`set` アクセサーには暗黙の引数 `value` がある」みたいな書かれ方になっています。
そして、結果的に `nameof(value)` は許されるという。

<pre class="source" title="nameof(value)">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">string</span> _x;
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">set</span> =&gt; _x = <span class="reserved">nameof</span>(<span class="reserved">value</span>); }
    <span class="comment">// 意味あるコードではないものの、とりあえずコンパイル可能。</span>
}
</code></pre>

`nameof(int)` とかも許されておらず、`nameof` の中に「青」が来る(たぶん)唯一の例となります。

時代の名残りと言うかなんというか…
今なら `value` も文脈キーワードにしたかもしれないですね。

ちなみに、同じく仕様からして「暗黙の引数」とされている[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)の[コマンドライン引数の `args`](../../../../study/csharp/misc/miscentrypoint.md#args-returns) はちゃんと「群青」(変数・引数の色)です。

<pre class="source" title="args は群青">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">args</span>[0]);
</code></pre>

まあ、`field` キーワードは最初から「キーワード扱い」の予定です。

<pre class="source" title="field は青">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">set</span> =&gt; <span class="reserved">field</span> = <span class="reserved">value</span>; }
}
</code></pre>
