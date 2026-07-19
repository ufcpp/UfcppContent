---
title: "【C# 10.0 変更点】 構造体のフィールド初期化子にはコンストラクター必須"
source_url: "https://ufcpp.net/blog/2022/2/csharp10-breaking-change-field-init/"
content_type: "BlogEntry"
published_at: "2022-02-19T18:29:28"
updated_at: "2022-02-19T18:29:28"
tags: []
umbraco_id: 2422
parent_id: 2411
sort_order: 6
aliases: []
---

# 【C# 10.0 変更点】 構造体のフィールド初期化子にはコンストラクター必須

先日 Visual Studio 17.1.0 (正式リリース)と 17.2 Preview 1 が出たわけですが。

これをインストールすると、ちょこっと C# 10.0 の構造体のフィールド初期化子の挙動が変わります。
以下のようなコード、17.0/17.1 Preview 時代はコンパイルできていたんですが、17.1/17.2 Preview ではコンパイル エラーになります。

<pre class="source" title="しれっと破壊的変更が掛かった文法">
<code><span class="reserved">struct</span> <span class="type"><span class="error">S</span></span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1; <span class="comment">// ここが原因。<span>
}
</code></pre>

ちなみに、C# の言語バージョンが改まったわけではなく、
バグ修正とかと同じノリでサイレント修正です。

仕様: [Never synthesize parameterless struct constructor](https://github.com/dotnet/csharplang/pull/5637)

## 問題

上記のコードの 17.0/17.1 Preview 時代の挙動なんですが、
まあ、暗黙的に引数なしコンストラクターが追加されています。
以下のような挙動。

<pre class="source" title="17.0 時代の挙動">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">S</span>().X); <span class="comment">// 1</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="comment">// public S() { } これがある時と同じ挙動になってた。<span>
}
</code></pre>

問題は、このコードに引数ありコンストラクターを足したとき。
以下のようになっていたそうです。

<pre class="source" title="引数ありコンストラクターを手動で足すと、引数なしコンストラクターの自動生成がなくなる">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">S</span>().X); <span class="comment">// 0。 default(S).X 扱い…</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="reserved">public</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; X = <span class="variable">x</span>;
    <span class="comment">// public S() { } これが生成されなくなる。</span>
}
</code></pre>

この挙動が罠すぎるので、傷が浅いうちに不具合扱いして挙動を変えようということになりました。

## 案1: 現状維持

もちろん、現状維持も検討されたみたいなんですが、
C# 10.0 リリース後のユーザーの反応的には相当に強い懸念の声が出ていて、無視はできないレベルと判断されたそうです。

## 案2: 常に引数なしコンストラクターを生成する

以下のように直すのが自然な気がしなくもないわけですが…

<pre class="source" title="案2: 常に引数なしコンストラクターを生成する">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">S</span>().X); <span class="comment">// ちゃんと1になればいいわけで。</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="reserved">public</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; X = <span class="variable">x</span>;
    <span class="comment">// public S() { } これが生成されればいい。</span>
}
</code></pre>

これで問題になるのが、`record struct` の[プライマリ コンストラクター](../../../../study/csharp/datatype/record.md#primary-constructor)だそうで。

プライマリ コンストラクターがある場合、「全てのコンストラクターは最終的にプライマリ コンストラクターにたどり着く必要がある」ということになっています。

<pre class="source" title="必ず最終的にはプライマリ コンストラクターにたどり着く">
<code><span class="reserved">record</span> <span class="reserved">struct</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">X</span>)
{
    <span class="comment">// 必ず S(int X) にたどり着くように書かないとダメ。</span>
    <span class="reserved">public</span> <span class="type">S</span>() : <span class="reserved">this</span>(1) { }
    <span class="reserved">public</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">a</span>, <span class="reserved">int</span> <span class="variable">b</span>) : <span class="reserved">this</span>(<span class="variable">a</span> * <span class="variable">b</span>) { }
}
</code></pre>

ここで、じゃあ、先ほどの、フィールド初期化子があるときにどうするか。
コンパイラーが自動的に引数なしコンストラクターを追加するのであれば、プライマリ コンストラクターには何を渡すべきかという問題がでます。

<pre class="source" title="引数なしコンストラクターはどう実装されるべきか…">
<code><span class="reserved">record</span> <span class="reserved">struct</span> <span class="type">S</span>(<span class="reserved">string</span> <span class="variable">X</span>)
{
    <span class="reserved">public</span> <span class="reserved">int</span> Y = 1;

    <span class="comment">// public S() : this(null) { } を足す？</span>
    <span class="comment">// 非 null が期待される string に null が渡ってしまう…</span>
}
</code></pre>

これがあるから、当初、「引数ありコンストラクターがあるときにはむやみに引数なしコンストラクターを追加しない」という判断になったようです。

## 案3: コンストラクターが1つもないとき、フィールド初期化子をエラーに

ということで、今日のブログの冒頭の話に戻ります。

以下のコードがエラーになりました。

<pre class="source" title="しれっと破壊的変更が掛かった文法">
<code><span class="reserved">struct</span> <span class="type"><span class="error">S</span></span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
}
</code></pre>

ちなみに、Visual Studio 17.2 Preview 1 では、この状態の(エラーのある)コードに対して「引数なしコンストラクターを追加する」というリファクタリング機能が追加されています。

![Generate constructor リファクタリング](../../../../../assets/media/1211/generateconstructor.png)

ただ、最初から以下のようなコードを書くと罠っぽい挙動になるのは今と同じ。

<pre class="source" title="引数ありコンストラクターを手動で足すと new S() が default(S) 扱い">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">S</span>().X); <span class="comment">// 0。 default(S).X 扱い…</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="reserved">public</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; X = <span class="variable">x</span>;
    <span class="comment">// public S() { } これは生成されない。</span>
}
</code></pre>

ただ、「後から迂闊に引数ありコンストラクターを足してしまう」という状況は減るはずです。

エラーにならないようにするのは元々が以下のようなコードのはずで、

<pre class="source" title="エラーにならないコード">
<code><span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="reserved">public</span> <span class="type">S</span>() { }
}
</code></pre>

ここに引数ありコンストラクターを足すはずなので、
以下のような挙動が期待されます。

<pre class="source" title="コンストラクターが明示的にあれば解決">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="reserved">new</span> <span class="type">S</span>().X); <span class="comment">// ちゃんと1。</span>

<span class="reserved">struct</span> <span class="type">S</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X = 1;
    <span class="reserved">public</span> <span class="type">S</span>() { }
    <span class="reserved">public</span> <span class="type">S</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; X = <span class="variable">x</span>;
}
</code></pre>
