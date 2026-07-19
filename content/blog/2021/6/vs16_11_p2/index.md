---
title: "Visual Studio 16.11 Preview 2: record struct と global using"
source_url: "https://ufcpp.net/blog/2021/6/vs16_11_p2/"
content_type: "BlogEntry"
published_at: "2021-06-16T22:10:57"
updated_at: "2021-06-16T22:10:57"
tags: []
umbraco_id: 2350
parent_id: 2349
sort_order: 0
aliases: []
---

# Visual Studio 16.11 Preview 2: record struct と global using

[Visual Studio 16.11 Preview 2](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.11.0.pre.2.0) が来ていて、これに C# 10.0 の新機能が2つほど merge されています。
(いつも通り、[LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#new-options) を入れれば利用可能になっています。)

- [record struct](../../../../study/csharp/datatype/record.md#record-struct)
- [global using](../../3/usingimprovements/index.md)

ちなみに本当は [16.10 Preview 3 のとき](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/34)に sealed record ToString って機能もひっそりと入ってるんですが、
まあ下手すると誰も気づかないレベルの修正なので説明省略…
(先月全然ブログを書いてないことへの言い訳。)

## <a id="record-struct">record struct</a>

はい。[レコード型](../../../../study/csharp/datatype/record.md)を[値型](../../../../study/csharp/resource/oo_reference.md#valtype)(構造体)でも作れるようになりました。
C# 9.0 時点で、単に `record` キーワードを使って型定義すると必ず[参照型](../../../../study/csharp/resource/oo_reference.md#reftype)(クラス)になっていたんですが、C# 10.0 では `record struct` と `record class` で値型・参照型を選べるようになりました。

<pre class="source" title="record struct, record class">
<code><span class="comment">// こっちは構造体なのでヒープ アロケーション起きない。</span>
<span class="comment">// あんまりでかいデータを持たせるとコピーのコストが結構でかい。</span>
<span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">S</span>(1, 2);

<span class="comment">// こっちはクラスなのでアロケーション発生。</span>
<span class="reserved">var</span> c = <span class="reserved">new</span> <span class="type">C</span>(1, 2);

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type">S</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
<span class="reserved">record</span> <span class="reserved">class</span> <span class="type">C</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
</code></pre>

ちなみに、単なる `record` はこれまで通りクラスです。
`record` と `record class` は完全に同じ意味。

### struct と record struct

レコード型は元々「構造体的な扱いができる参照型」でした。
構造体みたいに、メンバーごとのクローン、メンバーごとの値比較ができるクラスみたいなものです。

じゃあ、`record struct` は普通の `struct` と何が違うかと言うと、以下のような点。

- プライマリ コンストラクターを持てる
- プライマリ コンストラクターの引数からプロパティが自動生成される
- 以下のメソッドが自動的に作られる
  - [`Deconstruct` メソッド](../../../../study/csharp/datatype/deconstruction.md)
  - `ToString`
  - `Equals`, `GetHashCode` (`IEqualtable<T>` インターフェイスの実装)
  - `==`, `!=` 演算子

### struct と with

あと、今回一緒に、普通の構造体に対しても [`with` 式](../../../../study/csharp/datatype/record.md#with)が使えるようになっています。

<pre class="source" title="普通の構造体に対して with ">
<code><span class="reserved">var</span> s1 = <span class="reserved">new</span> S { X = 1, Y = 2 };
<span class="reserved">var</span> s2 = s1 <span class="reserved">with</span> { X = 3 };

Console.WriteLine(s2); <span class="comment">// (3, 2)</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; (X, Y).ToString();
}
</code></pre>

構造体では、ある変数から別の変数に代入したとき、元から自動的にコピーを作っていたので、それをそのまま使っています。

## global using

`global using` を使うと、プロジェクト全体に対して有効な [using ディレクティブ](../../../../study/csharp/structured/sp_namespace.md#using)を書けます。

例えば、ある1ファイルに以下のようなコードを書いたとします。

<pre class="source" title="global using を書いたファイル">
<code><span class="reserved">global</span> <span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;
<span class="reserved">global</span> <span class="reserved">using</span> System.Linq;
<span class="reserved">global</span> <span class="reserved">using</span> System.Collections.Generic;
</code></pre>

そのプロジェクト内では、以下のようなコードが普通に書けます。

<pre class="source" title="global using の影響下にあるコードの例">
<code><span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt; { 1, 2, 3 };
<span class="reserved">var</span> y = x.<span class="method">Select</span>(i =&gt; i * i);
<span class="control">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> y) <span class="method">WriteLine</span>(i);
</code></pre>

[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)と合わせると、本当にこの3行だけで「コンパイルできて実行できるコード」になります。
「ネットで見かけたサンプル コードをコピペしたら動かない」というクレームが減るかと思われます。
(これが一番のメリット。)

あと、「[`DateOnly` なんて名前](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/)嫌だーーー」という方は以下のように書いておけます。一応。(別に推奨はしない。)

<pre class="source" title="">
<code><span class="reserved">global</span> <span class="reserved">using</span> Date = System.DateOnly;
</code></pre>

### 通常 using と同列

`global using` は、「そのプロジェクト内のすべてのファイルの先頭に `using` があるのと一緒」みたいな挙動をします。
つまり、「通常 `using` よりも外側のスコープ」みたいなことにはなりません。
あくまで「通常 `using` と同列」です。

例えばどこかのファイルに以下のような `System` への `global using` があったとします。

<pre class="source" title="System への global using">
<code><span class="reserved">global</span> <span class="reserved">using</span> System;
</code></pre>

で、これと同じプロジェクト内で通常の `using` を書く場合、以下のような挙動をします。

<pre class="source" title="global using System; 影響下のコード">
<code><span class="reserved">using</span> <span class="warning">System</span>; <span class="comment">// すでに global using System; があるので「重複」警告あり</span>

<span class="reserved">using</span> X = <span class="error">DateTime</span>; <span class="comment">// この行はコンパイル エラー。ここでは using System; ありきにはならない。</span>
<span class="reserved">using</span> <span class="type">Y</span> = System.<span class="type">DateTime</span>; <span class="comment">// こっちは OK</span>

<span class="reserved">namespace</span> A
{
    <span class="reserved">using</span> <span class="type">X</span> = <span class="type">DateTime</span>; <span class="comment">// これも OK。A の外に using System; があるので。</span>
}
</code></pre>

### 知らないところで using されてる問題

別に `global` かどうか以前の問題なんですが、「`using` しすぎ」は問題を起こすことがあります。
まず、同じ名前の型があった場合に「どっちかわからない」エラーを起こします。
単純に IDE 上での補完候補が増えすぎてうざいとかもあります。
それに、C# の場合、[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md#problem)という、`using` の有無で挙動が変わる機能があったりもします。

`global using` ではそれをプロジェクト全体にわたってできるわけですから、
嫌がらせしようと思えばいくらでも嫌がらせができます。
とりあえず名前被りの例:

<pre class="source" title="同名クラスを持つ別名前空間を global using">
<code><span class="comment">// JsonSerializer クラスがどれにもあるので、フルネームで書かないと弁別不能になる。</span>
<span class="reserved">global</span> <span class="reserved">using</span> Newtonsoft.Json;
<span class="reserved">global</span> <span class="reserved">using</span> Utf8Json;
<span class="reserved">global</span> <span class="reserved">using</span> System.Text.Json;
</code></pre>

ちなみに、`global using` は複数のファイルに書けます。
上記嫌がらせの3行を、それぞれ全く別のファイルに書いておくということもできます。

一方で、一応、<em>ファイルの先頭にしか書けない</em>という縛りはあります。

<pre class="source" title="先頭以外に global using を書くとさすがにエラー">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// 超絶長い Main 処理を延々と書いたりもありえなくはない</span>
    }
}

<span class="error"><span class="reserved">global</span> <span class="reserved">using</span> System.Linq;</span> <span class="comment">// さすがにこの行はコンパイル エラー</span>
</code></pre>

#### 問題を起こせる範囲

ただまあ、`global using` の影響範囲はプロジェクト内に限られるので、
嫌がらせができるとすれば基本的に「内部犯」になります。

「[global using で一番邪悪なことやった人が優勝](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/36)」とかいうひどいタイトルで配信してアイディアを募ろうとしていたり。

それで例として「`Where` 拡張メソッドの乗っ取り」を挙げてはいるんですが…
拡張メソッドで悪さをしたければ、トップ レベルのクラス(名前空間なしのグローバルなクラス)に拡張メソッドを書く方がはるかにたちが悪いです。

で、内部犯であれば、レビューや単体テストをちゃんとしていればある程度は防げるはずです。
悪意を持って攻めるなら「数千行のコミットにしれっと混ぜ込む」とかも考えられますけども。

たいてい以下のような [Analyzer](../../../../study/csharp/misc/analyzer-generator.md#analyzer) を書いてしまえば対処できちゃいそうなんですよねぇ…

- 複数のファイルに `global using` を書けなくする
- 拡張メソッドを含む名前空間を `global using` できなくする
- `global using` した名前空間中の型名の被りに対して警告を出す

あと、`global using` は [Source Generator](../../../../study/csharp/misc/analyzer-generator.md#analyzer) で生成することもできます。
これが唯一の「プロジェクト外に影響を及ぼせる `global using`」になるんですが…
こちらはこちらで、「信用ならないパッケージを参照するのが怖いのは元から」ですし、
Source Generator を書ける人自体が割合そんなに多くないですし。

なんかこう、レビューをうまくすり抜けたり、「嫌な予感しかしないんだけどメリットもありそうでやむなく使う」みたいな邪悪さを出せないものかと悩み中…
