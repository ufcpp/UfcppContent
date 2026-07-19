---
title: "ピックアップRoslyn: using がらみ (using エイリアスの改善と global using)"
source_url: "https://ufcpp.net/blog/2021/3/usingimprovements/"
content_type: "BlogEntry"
published_at: "2021-03-15T01:02:08"
updated_at: "2021-03-20T20:49:34"
tags: []
umbraco_id: 2338
parent_id: 2336
sort_order: 1
aliases: []
---

# ピックアップRoslyn: using がらみ (using エイリアスの改善と global using)

今日も「C# Language Design Meeting 議事録」の中から1個1個機能紹介。

今日は[2/10](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-02-10.md#global-usings)、[2/22](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-02-22.md#using-alias-improvements)辺りの話になります。

[using](../../../../study/csharp/structured/sp_namespace.md#using)がらみに色々更新が掛かるみたいです。
大まかに2点。

- using エイリアス改善: これまで書けてもよさそうなのに書けないエイリアスを書けるようにする
- global using: プロジェクト全域に対して有効な `using` ディレクティブ

[global using の方は提案ドキュメントが merge 済み](https://github.com/dotnet/csharplang/blob/main/proposals/GlobalUsingDirective.md)、
[using エイリアスの話はレビュー中](https://github.com/dotnet/csharplang/pull/4452)です。

## using エイリアス改善

これも細かく言うと3点。

- キーワードになってる型を直接使えるようにする
- 配列の `[]`、nullable の `?`、ポインターの `*`、タプルとかを使えるようにする
- 型引数を持てるようにする

今でも OK なパターンだと以下のような書き方ができます。

<pre class="source" title="これは C# 1.0 の頃から書ける">
<code><span class="reserved">using</span> <span class="type">Ok1</span> = System.<span class="type">Int32</span>;
<span class="reserved">using</span> <span class="type">Ok2</span> = System.<span class="type">Nullable</span>&lt;<span class="reserved">int</span>&gt;;
</code></pre>

ところが以下のようなやつは現状ではコンパイル エラー。
ジェネリック型引数の中なら `int` を書けるのに、直接は書けない。

<pre class="source" title="using エイリアスの右辺にキーワードは直接書けない">
<code><span class="reserved">using</span> Ng1 = <span class="reserved"><span class="type">int</span></span>;
<span class="reserved">using</span> Ng2 = <span class="reserved"><span class="type">int</span></span>?;
</code></pre>

以下のようなやつもコンパイル エラー。
配列の `[]`、nullable の `?`、ポインターの `*`は現状書けません。

<pre class="source" title="? [] * も付けれない">
<code><span class="reserved">using</span> <span class="type">Ng3</span> = System.<span class="type">Int32</span><span class="error">?</span>;
<span class="reserved">using</span> Ng4 = System.Int32<span class="error">[]</span>;
<span class="reserved">using</span> Ng5 = System.Int32<span class="error">*</span>;
</code></pre>

あと、頻出で出ている要望がタプルで、
以下のようなやつも「書きたいのに書けない」筆頭です。

<pre class="source" title="タプルのエイリアスを作りたいという要望は頻出">
<code><span class="reserved">using</span> Ng6 = (<span class="error">System.<span class="type">Int32</span></span>, <span class="error">System.<span class="type">String</span></span>); <span class="comment">// これがダメな時点でお察しだけど…</span>
<span class="reserved">using</span> Ng7 = (<span class="reserved"><span class="error">int</span></span>, <span class="reserved"><span class="error">string</span></span>); <span class="comment">// ほんとに書きたいのはこうだし、</span>
<span class="reserved">using</span> Ng8 = <span class="error">(<span class="reserved">int</span> <span class="variable">id</span>, <span class="reserved">string</span> <span class="variable">name</span>)</span>; <span class="comment">// 名前付きタプルも書きたい。</span>
</code></pre>

この辺り、C# 10.0 でまとめて解消しようという感じになっています。
ちなみに、似たような話だと、`enum` の基底にキーワードを書けるかどうかみたいなのが C# 6.0 の時に変わっています。

<pre class="source" title="enum の基底に int を書けるかどうかも C# 6.0 からの変更">
<code><span class="comment">// これなら C# 1.0 の頃から書けた。</span>
<span class="reserved">enum</span> <span class="type">A</span> : System.<span class="type">Int32</span> { }
 
<span class="comment">// これが書けるようになったのは C# 6.0 から。</span>
<span class="reserved">enum</span> <span class="type">B</span> : <span class="reserved">int</span> { }
</code></pre>

タプルのエイリアスを付けれるようにしようとなると、まあ、「ジェネリックなエイリアス」も作りたくなります。
これもこの際、C# 10.0 で一緒にやるそうです。

<pre class="source" title="ジェネリックなエイリアス">
<code><span class="reserved">using</span> <span class="error">Fix2&lt;T&gt;</span> = (T, T);
<span class="reserved">using</span> <span class="error">Fix3&lt;T&gt;</span> = (T, T, T);
<span class="reserved">using</span> <span class="error">Fix4&lt;T&gt;</span> = (T, T, T, T);
</code></pre>

もちろんタプル以外の「ジェネリックなエイリアス」も同じく C# 10.0 で取り組み。

<pre class="source" title="タプル以外のエイリアスものもジェネリックにしたい">
<code><span class="reserved">using</span> Option&lt;T&gt; = T ?;
</code></pre>

「部分適用」もできるようにしたいみたいです。
以下のような、「2引数のうち片方だけ確定」みたいな「ジェネリックなエイリアス」も作れるようにする予定です。

<pre class="source" title="部分適用なジェネリック エイリアス">
<code><span class="reserved">using</span> StringDictionary&lt;T&gt; = System.Collections.Generic.<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, T&gt;;
</code></pre>

arity (型引数の数)違いのエイリアスは並べられるようにする予定だそうです。

<pre class="source" title="arity 違いのエイリアスも OK になる予定">
<code><span class="reserved">using</span> <span class="type">MyDictionary</span> = System.Collections.Generic.<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">string</span>&gt;;
<span class="reserved">using</span> MyDictionary&lt;T&gt; = System.Collections.Generic.<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, T&gt;;
<span class="reserved">using</span> MyDictionary&lt;T1, T2&gt; = System.Collections.Generic.<span class="type">Dictionary</span>&lt;T1, T2&gt;;
</code></pre>

ちなみに、以下のようなオープン ジェネリック(引数なしの状態)は C# 10.0 でも書けません。

<pre class="source" title="これは C# 10.0 でもダメ">
<code><span class="comment">// これは引き続き今後もダメ。</span>
<span class="comment">// 空っぽの &lt;&gt; が許されるのは typeof(T&lt;&gt;) だけ</span>
<span class="reserved">using</span> <span class="type">OpenGeneric</span> = System.Collections.Generic.<span class="error"><span class="type">List</span>&lt;&gt;</span>;
</code></pre>

これをやるうえでちょっと悩ましいのが、「制約違反」みたいなのをどこで判定するか。
選択肢は2つあって、1つ目はエイリアスを作る時点 (`using T = ...;` の行) でエラーにする方法。
エイリアスを「実際にある型」に近い扱いにしようという感じ。
(現状あんまり乗り気ではなさげ。)

<pre class="source" title="選択肢1: エイリアス自体に where 制約">
<code><span class="reserved">using</span> Optional&lt;T&gt; = <span class="type">Nullable</span>&lt;<span class="error">T</span>&gt;; <span class="comment">// 「T に制約が付いてないのでダメ」扱いする</span>
<span class="reserved">using</span> Optional&lt;T&gt; = <span class="type">Nullable</span>&lt;T&gt; <span class="variable">where</span> T : <span class="reserved">struct</span> <span class="comment">// と言うことはここに型制約(where)を書けるようにする必要あり</span>
</code></pre>

もう1つの選択肢は、「エイリアスの時点では素通し」で、現状、こっちが有力みたいです。
「エイリアスはあくまでエイリアス」で、C 言語のマクロっぽい挙動というか。

<pre class="source" title="エイリアスの時点では素通し">
<code><span class="reserved">using</span> Optional&lt;T&gt; = <span class="type">Nullable</span>&lt;T&gt;; <span class="comment">// この時点では T のチェックしない。</span>
 
<span class="comment">// Nullable&lt;string&gt; とは書けないので、そのエイリアスの Optional&lt;string&gt; もダメ。</span>
<span class="reserved">void</span> <span class="method">m</span>(Optional&lt;<span class="reserved"><span class="error">string</span></span>&gt; <span class="variable">opt</span>) { }
</code></pre>

後者が有力なので、`using A<T> = T?;` が [null 許容値型](../../../../study/csharp/resource/sp2_nullable.md)になるか、
[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)になるか、
[defaultable](../../../../study/csharp/resource/nullablereferencetype.md#unconstrained-generics)になるかはおそらく利用側次第になります。

## global using

プロジェクト全域に影響を及ぼす `using` ディレクティブを書きたいという要望も昔からちらほらあります。

これはまあ、「全域に影響を及ぼす」ってのが怖くてやってなかっただけなんですが、
それももう今更なのかなぁという感じになっています。
と言うのも、

- [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)は `<Nullable>enable</Nullable` オプションを与えるとプロジェクト全体で有効・無効が切り替わる
- [SkipLocalsInit](../../../../study/csharp/cheatsheet/ap_ver9.md#skip-locals-init) は `[module:SkipLocalsInit]` と書けばプロジェクト全体に影響を及ぼせる

みたいな文法がすでにあります。

あと、 ASP.NET なプロジェクトを作るとテンプレート内に `_ViewImports.cshtml` っていうのが最初から存在しますが、その中身は以下のようになっています。

<pre class="source" title="テンプレート通りの _ViewImports.cshtml">
<code><span style="background:yellow;">@</span><span class="reserved">using</span> WebApplication1
<span style="background:yellow;">@</span><span class="reserved">using</span> WebApplication1.Models
<span style="background:yellow;">@addTagHelper</span> <span class="string">*, Microsoft.AspNetCore.Mvc.TagHelpers
</span>
</code></pre>

これ、やってることはまさに「プロジェクト中のすべての cshtml に影響を及ぼす `using`」になります。

あと例えば、最近の [C# 公式チュートリアル](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/hello-world?tutorial-step=1)では、普通に以下のコードがコンパイルできたりします。
どうも、暗黙的に、`System`、`System.Linq`、`System.Collections.Generic` 辺りがデフォルトで `using` されていそうな雰囲気。

<pre class="source" title="C# 公式チュートリアルではなぜか using System; が要らない">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello World!&quot;</span>);
</code></pre>

ちなみに、現時点での実装はどうも「ごり押し」っぽい雰囲気があります。
[Visual Studio Users Community Japan 勉強会 #6 質疑応答枠 1:12:18～](https://www.youtube.com/watch?v=yDrQ2nCPfR8&t=4338s)で話したことがあるですが、
おそらく、「書いたコードの前に以下のコードを追加」みたいな実装になっていると思います。

<pre class="source" title="テンプレコードを string.Concat してからコンパイルしてると思う、たぶん">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Linq;
<span class="reserved">using</span> System.Collections.Generic;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
</code></pre>

相当に気持ち悪い実装ですが、
こんな気持ち悪いことをしてまで、「`using` はおまじない」を消したいという状態になっています。

だったら認めようと。

問題は実現方法なんですが、これも初期案としては2案出ていました。

- `<Nullable>enable</Nullable>` みたいに、C# コンパイラーに渡すオプション/csproj ファイルに書く設定として提供
- C# ソースコード中に `global using N = int;` みたいなのを書けるようにする

ちなみに、後者が有力になっています。`global using N = int;` 支持になっているのは以下のような理由。

- Source Generator で使う場合に C# コードで書ける方が助かる
- dotnet コマンドから csc (C# コンパイラー)に素通ししてあげないといけないオプションがすでに大量にあってあんまりもう増やしたくない
- 「global using が欲しい」という要望も長年ずっと出続けてる

で、後は文法なわけですが、`global using` で行くみたいです。

<pre class="source" title="global using ディレクティブ">
<code><span class="reserved">global</span> <span class="reserved">using</span> System;
<span class="reserved">global</span> <span class="reserved">using</span> System.Linq.<span class="type">Enumerable</span>;
<span class="reserved">global</span> <span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">global</span> <span class="reserved">using static</span> System.Linq.<span class="type">Enumerable</span>;
</code></pre>

まあ、迷うとしたら語順くらいですかね。
普通に `global` という単語を名前空間にもクラス名にも使えてしまうので、`using global` だと「キーワードの `global` か名前空間の `global` か」の弁別が大変だそうで。

<pre class="source" title="global も文脈キーワード">
<code><span class="reserved">using</span> global;
 
<span class="reserved">namespace</span> global
{
    <span class="reserved">class</span> <span class="type">global</span> { }
}
</code></pre>

あと、さすがにファイル中散り散りに「プロジェクト全体に影響あり」なものが書かれるのは怖いということで、
`global using` を書けるのはファイルの先頭(普通の `using` ディレクティブよりも前)だけにするみたいです。
