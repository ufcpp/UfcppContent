---
title: "C# vNext Preview"
source_url: "https://ufcpp.net/blog/2018/5/cs80preview/"
content_type: "BlogEntry"
published_at: "2018-05-12T20:00:42"
updated_at: "2018-05-12T20:05:45"
tags: []
umbraco_id: 2153
parent_id: 2150
sort_order: 2
aliases: []
---

# C# vNext Preview

以下のページで、C# 8.0のプレビュー公開を始めたみたです。

- [vNext Preview](https://github.com/dotnet/csharplang/wiki/vNext-Preview)

## インストール

これまででも、まだ Visual Studio プレビュー版にも組み込まれていないような機能の類も、VSIX (Visual Studio 拡張)や NuGet 参照でコンパイラーだけ差し替えることで使えたりはしました。
[roslyn のデイリー ビルド](https://dotnet.myget.org/gallery/roslyn)を「パッケージ ソース」にして、Microsoft.Net.Compilers パッケージを参照すれば行けます。

ただ、このやり方だと、IDEのC#エディターの IntelliSense は最新版になりません。
ビルドを実行するとコンパイルは通るんですが、エディター上ではエラーの赤線だらけになります。

で、今回公開された[vNext Preview](https://github.com/dotnet/csharplang/wiki/vNext-Preview)は、インストールとアンインストール用のスクリプトが入っています。基本的には中身は VSIX なんですが、たくさんの VSIX が入っていて、依存順が複雑だとかでそれぞれ個別に入れるのは無理っぽい感じです。
なので、

- Visual Studio を全て落とす
- インストール スクリプト(PowerShell)を実行する
- もし、Visual Studio のバージョンアップをする際には、一度 vNext Preview をアンインストールしてから

という手順を踏んでほしいとのこと。

## 少し先の機能

最近だと、Visual Studio 自体が、あるバージョンをリリースしてほとんどすぐに、[次のバージョンのプレビュー](https://www.visualstudio.com/ja/vs/preview/)を公開しています。
インストールも割と簡単で、正式リリース版との共存もできます。

そして、C# もそれでプレビューを体験できたりしました。
Visual Studio のリリース周期は最近3～4か月ごとなので、そのくらい先のものであればそんなに苦労することなく試せます。

一方、今回インストール用スクリプトを用意して提供しているのは、
要するに2バージョン以上先での提供予定のものを早めに試してもらいたいということでしょう。
久々のメジャー バージョンアップですし、ちょっと大き目な機能も入る予定です。

## C# 8.0 プレビューの現状

とはいえ、こないだ公開された「5月4日ビルド版」では、2個しか C# 8.0 の新機能が入っていなかったりはします。
入っているのは、

- 再帰パターン
- ranges

の2つ。

### 再帰パターン

C# 7.0 で[パターン マッチング](../../../../study/csharp/datatype/typeswitch.md)が入ったわけですが、
C# 7.0 時点では、元々計画に挙がってたうちの一部分(型パターン、型スイッチ)だけが実装されています。

C# 8.0 では、7.0 のときに先送りされた再帰パターンが入る予定です。

例えば以下のようなクラスがあったとして、

<pre class="source" title="例">
<code><span class="reserved">class</span> <span class="type">Base</span> { }
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> A(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);
}
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> B(<span class="reserved">string</span> name, <span class="reserved">int</span> value) =&gt; (Name, Value) = (name, value);
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">string</span> name) =&gt; name = Name;
    <span class="reserved">public</span> <span class="reserved">void</span> Deconstruct(<span class="reserved">out</span> <span class="reserved">string</span> name, <span class="reserved">out</span> <span class="reserved">int</span> value) =&gt; (name, value) = (Name, Value);
}
</code></pre>

以下のようなコードなら C# 7.0 でも書けました。

<pre class="source" title="C# 7.0 の型パターン">
<code><span class="reserved">static</span> <span class="reserved">int</span> M(<span class="type">Base</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="type">A</span> a: <span class="reserved">return</span> a.X * a.Y;
        <span class="reserved">case</span> <span class="type">B</span> b <span class="reserved">when</span> b.Name == <span class="string">"one"</span>: <span class="reserved">return</span> b.Value;
        <span class="reserved">case</span> <span class="type">B</span> b <span class="reserved">when</span> b.Name == <span class="string">"two"</span>: <span class="reserved">return</span> 2 * b.Value;
        <span class="reserved">case</span> <span class="type">B</span> b <span class="reserved">when</span> b.Name == <span class="string">"three"</span>: <span class="reserved">return</span> 3 * b.Value;
        <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>();
    }
}
</code></pre>

C# 8.0 では以下のような、再帰的なパターンが使えるようになります。

<pre class="source" title="C# 8.0 の再帰パターン">
<code><span class="reserved">static</span> <span class="reserved">int</span> M(<span class="type">Base</span> obj)
{
    <span class="reserved">switch</span> (obj)
    {
        <span class="reserved">case</span> <span class="type">A</span> { X: <span class="reserved">var</span> x, Y: <span class="reserved">var</span> y }: <span class="reserved">return</span> x * y;
        <span class="reserved">case</span> <span class="type">B</span> (<span class="string">"one"</span>) { Value: <span class="reserved">var</span> v }: <span class="reserved">return</span> v;
        <span class="reserved">case</span> <span class="type">B</span> (<span class="string">"two"</span>) { Value: <span class="reserved">var</span> v }: <span class="reserved">return</span> 2 * v;
        <span class="reserved">case</span> <span class="type">B</span> (<span class="string">"three"</span>) { Value: <span class="reserved">var</span> v }: <span class="reserved">return</span> 3 * v;
        <span class="reserved">default</span>: <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>();
    }
}
</code></pre>

`B("one")` みたいな、`()` の部分は位置指定パターンと言って、`Deconstruct` メソッドが呼ばれています(「[分解](../../../../study/csharp/datatype/deconstruction.md)」と同じ仕組み)。
残りの `{}` の部分はプロパティ パターンと言って、プロパティに対する `X is var x` などに展開されます。

#### switch 式

また、`switch` 式も追加されます。
式です。`=>` の後ろとかにも書けます。
今のところは以下のような構文になる予定。

<pre class="source" title="switch 式">
<code><span class="reserved">static</span> <span class="reserved">int</span> M(<span class="type">Base</span> obj)
    =&gt; obj <span class="reserved">switch</span>
    {
        <span class="type">A</span> { X: <span class="reserved">var</span> x, Y: <span class="reserved">var</span> y } =&gt; x * y,
        <span class="type">B</span> (<span class="string">"one"</span>) { Value: <span class="reserved">var</span> v } =&gt; v,
        <span class="type">B</span> (<span class="string">"two"</span>) { Value: <span class="reserved">var</span> v } =&gt; 2 * v,
        <span class="type">B</span> (<span class="string">"three"</span>) { Value: <span class="reserved">var</span> v } =&gt; 3 * v,
        _ =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">IndexOutOfRangeException</span>()
    };
</code></pre>

#### {} パターンで null チェック

ちなみに、プロパティ パターン (`{}` を使ったパターン)には null チェックが伴うそうです。

<pre class="source" title="{} パターンで null チェック">
<code><span class="reserved">string</span> s = <span class="reserved">null</span>;

<span class="comment">// null は型情報を持ってなかったり。たとえ、静的な型が一致していても is は常に false。</span>
<span class="reserved">if</span> (s <span class="reserved">is</span> <span class="reserved">string</span>) <span class="type">Console</span>.WriteLine(<span class="string">"ここは絶対通らない"</span>);

<span class="comment">// is string x みたいな変数宣言を伴ってても同じ。</span>
<span class="reserved">if</span> (s <span class="reserved">is</span> <span class="reserved">string</span> x) <span class="type">Console</span>.WriteLine(<span class="string">"ここも通らない"</span>);

<span class="comment">// が、var パターンは常に true。見た目 is string に似てるけど、結果が違う。</span>
<span class="reserved">if</span> (s <span class="reserved">is</span> <span class="reserved">var</span>  y) <span class="type">Console</span>.WriteLine(<span class="string">"ここは通る"</span>);

<span class="comment">// で、プロパティ パターンを使って、null チェック付きの var に近いことができる。</span>
<span class="reserved">if</span> (s <span class="reserved">is</span> { }) <span class="type">Console</span>.WriteLine(<span class="string">"ここは通らない"</span>);
</code></pre>

#### タプル switch

あと、タプルに対する switch では、`()` を1重に省略できます。

<pre class="source" title="タプル switch">
<code><span class="reserved">static</span> <span class="reserved">int</span> M(<span class="reserved">int</span> x, <span class="reserved">int</span> y)
{
    <span class="comment">// 本来は、switch ((x, y))</span>
    <span class="reserved">switch</span> (x, y)
    {
        <span class="reserved">case</span> (1, 1): <span class="reserved">return</span> 1;
        <span class="reserved">case</span> (1, 2): <span class="reserved">return</span> 2;
        <span class="reserved">case</span> (2, 1): <span class="reserved">return</span> 3;
        <span class="reserved">case</span> (2, 2): <span class="reserved">return</span> 4;
        <span class="reserved">default</span>: <span class="reserved">return</span> 0;
    }
}
</code></pre>

### ranges

ranges は、`1..3` みたいな書き方で「1から3まで(ただし3は含まない)のインデックス」みたいな範囲を表す記法です。
`Range`構造体と`Index`構造体に展開される予定で、
[この`range.cs`](https://raw.githubusercontent.com/dotnet/csharplang/master/proposals/ranges.cs)みたいな定義が必要です。

<pre class="source" title="ranges">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> data = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5, 6, 7, 8 };

        <span class="comment">// 1～4番目 → { 2, 3, 4 }</span>
        Write(data[1..4]);
        <span class="comment">// ↑は、↓と同じ結果</span>
        Write(data.AsSpan().Slice(1, 4 - 1));

        <span class="comment">// 2～(Length - 2)番目 = 最初と最後の2要素を飛ばす → { 3, 4, 5, 6 }</span>
        Write(data[2..^2]);
        <span class="comment">// ↑は、↓と同じ結果</span>
        Write(data.AsSpan().Slice(2, (data.Length - 2) - 2));

        <span class="comment">// 5～末尾 → { 6, 7, 8 }</span>
        Write(data[5..]);

        <span class="comment">// 先頭～3 → { 1, 2, 3 }</span>
        Write(data[..3]);

        <span class="comment">// 全体</span>
        Write(data[..]);
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Write(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; s)
    {
        <span class="reserved">foreach</span> (var x <span class="reserved">in</span> s)
        {
            <span class="type">Console</span>.Write(x);
            <span class="type">Console</span>.Write(<span class="string">" "</span>);
        }
        <span class="type">Console</span>.WriteLine();
    }
}
</code></pre>

正直、`Slice(start, length)` みたいな記法との差が少なすぎて、便利さで言うとそこまで大きくはないんですが。
以下のような要件があるので、それなりに必要性はあります。

- `Slice(x, y)` みたいな書き方では、第2引数が「長さ」なのか「終端インデックス」なのかで迷う
- 「末尾から n 番目」みたいなのは`data.Length - n` みたいな書き方が必要でしんどい
- 特に多次元データの時に `data[a..b, c..d, e..f]` みたいに書きたい

現時点では、以下の実装はないみたいです。

1. start, length 型の ranges (「a を始点に長さ b」みたいなやつ)
1. inclusive ranges (「a～b まで(bも<em>含む</em>)」みたいなやつ)
1. ユーザー定義の `..` 演算子

このうち、C# 8.0 正式リリースまでに入るかもしれないのは1の start, length 型 ranges くらい。
残りは「その先また改めて検討」のはずです。

#### 一時的な「拡張インデクサー」

ちなみに、このプレビューでは、「拡張インデクサー」みたいなものが一時的に入っています
(`T[]`、`Span<T>`、`string`に対する`Range`型引数のインデクサーを拡張として追加しています)。
これはほんとに一時的な対処で、C# 8.0でこの文法で「拡張インデクサー」が使えるわけではありません。

正式には、「[Type Class](https://github.com/dotnet/csharplang/issues/110)」という別提案が出ていて、これ待ちです。
もしかしたらこれも C# 8.0 で入るかも。
