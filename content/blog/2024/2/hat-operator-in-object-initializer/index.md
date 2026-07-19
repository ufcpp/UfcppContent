---
title: "オブジェクト初期化子中の ^ 演算子"
source_url: "https://ufcpp.net/blog/2024/2/hat-operator-in-object-initializer/"
content_type: "BlogEntry"
published_at: "2024-02-08T21:36:47"
updated_at: "2024-02-08T21:36:47"
tags: []
umbraco_id: 2481
parent_id: 2480
sort_order: 0
aliases: []
---

# オブジェクト初期化子中の ^ 演算子

今日の C# 話はちょこっとした修正の話になります。
これまで `new C { [^1] = 1 };` がコンパイル通らなかったみたいで、これが最近修正されました。

(Visual Studio 17.9 Preview 3 (1月17日リリース済み)の時点で実装されていました。
気づいてはいたけども、小さすぎてブログにするかどうか迷ってるうちに3週間ほど経過。)

以下のコードで示すような修正内容です。

<pre class="source" title="オブジェクト初期化子中の ^ 演算子">
<span class="comment">// これがコンパイル エラーを起こす。</span>
<span class="comment">// (Visual Studio 17.9 Preview 3 以降を使うとコンパイルできるようになった。)</span>
<span class="reserved">var</span> <span class="variable">c</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">C</span> { [<span class="operator">^</span><span class="number">1</span>] <span class="operator">=</span> <span class="number">1</span> };

<span class="comment">// これなら昔からコンパイル通る。</span>
<span class="comment">// (オブジェクト初期化子はこれと同じコードに展開されるはずなのに。)</span>
<span class="variable">c</span>[<span class="operator">^</span><span class="number">1</span>] <span class="operator">=</span> <span class="number">1</span>;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// インデクサーと Length さえ持っていれば c[^i] と書けるようになる。</span>
    <span class="comment">// c[c.Length - i] 扱い。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Length</span> <span class="operator">=&gt;</span> <span class="number">1</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">i</span>] { <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="variable local">i</span>; <span class="reserved">set</span> { } }
}
</pre>

まあ、`^` を導入した時にオブジェクト初期化子は考慮漏れしてたんですかね。

こんなのでも一応悩むポイントはありまして。
1つは、例えば入れ子で `new C() { [^1] = { [2] = 42, [3] = 43 } }` とか書いたとき、

<pre class="source" title="2度 Length を評価">
<span class="comment">// 2行に分かれる = Length - 1 の計算が2度走る。</span>
<span class="reserved">var</span> <span class="variable">c</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">C</span>();
<span class="variable">c</span>[<span class="operator">^</span><span class="number">1</span>][<span class="number">2</span>] <span class="operator">=</span> <span class="number">42</span>;
<span class="variable">c</span>[<span class="operator">^</span><span class="number">1</span>][<span class="number">3</span>] <span class="operator">=</span> <span class="number">43</span>;
</pre>


か

<pre class="source" title="^i をキャッシュして Length は1回限り評価">
<span class="comment">// ^ の結果をキャッシュする。</span>
<span class="reserved">var</span> <span class="variable">c</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">C</span>();
<span class="reserved">var</span> <span class="variable">cachedIndexArgument</span> <span class="operator">=</span> <span class="operator">^</span><span class="number">1</span>;
<span class="variable">c</span>[<span class="variable">cachedIndexArgument</span>][<span class="number">2</span>] <span class="operator">=</span> <span class="number">42</span>;
<span class="variable">c</span>[<span class="variable">cachedIndexArgument</span>][<span class="number">3</span>] <span class="operator">=</span> <span class="number">43</span>;
</pre>


か、どちらがいいかという問題。

もう1つ、`new C() { [^1] = { } }` みたいに入れ子の部分が空っぽの場合、`Length` を評価する必要はあるかどうかとかも。


「`Length` が副作用を持っている」とか「`c[^1]` が副作用で `Length` を書き換える」みたいな変なことをしているとこの辺りの結果が変わるわけで。

結局、以下のような選択をしたそうです。

* 前者は、「`^` の結果をキャッシュする」の方を選択
* 後者は、`[^1] = { }` の時は評価しない(`Length - 1` の計算自体せず、`Length` の getter は呼ばない) を選択


## おまけ: ちょっと予告

あと、ちょこっと次回以降の予告。

しばらくブログ化していませんでしが C# 13 向けの作業がちらほら。
特に、2月に入ったくらいからアクティブで、結構検討が進んでいるみたいです。

最近見かけている話題を見出しだけ出しておくと以下の通り。

* コレクション式の改善
* ジェネリクスで ref struct を使えるように
* Extensions
* ジェネリクスの部分型推論 `_`
* partial プロパティ
* 破壊的変更がらみ

しばらくネタをため込んでしまったために大量に…
一気に書くと大変なので、次回以降、1個ずつブログにしようかと思います。
