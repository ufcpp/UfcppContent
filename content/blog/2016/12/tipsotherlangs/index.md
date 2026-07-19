---
title: "小ネタ C# と他の言語との差というと"
source_url: "https://ufcpp.net/blog/2016/12/tipsotherlangs/"
content_type: "BlogEntry"
published_at: "2016-12-23T15:19:37"
updated_at: "2016-12-23T07:47:09"
tags: []
umbraco_id: 2006
parent_id: 1969
sort_order: 22
aliases: []
---

# 小ネタ C# と他の言語との差というと

C#で、「他の言語との差というと」とか「他の言語から来たばかりの人が書きがちなコード」みたいなことを聞かれた場合、まず何が思い浮かぶでしょう。

C#に馴れちゃってる人だと、LINQとかasync/awaitとかの機能が最初に浮かんだりします。でも、この辺りは「大きな機能」過ぎて、知ってるか知らないかの二択、1度知れば検索してすぐに解説が出てくる類で、かえって問題にならないという印象。
案外、困るのはもうちょっと細かい部分じゃないかと思います。

みたいなのが今日の話題。

## 辞書(ハッシュテーブル)の列挙

`Dictionary<TKey, TValue>`の列挙を、キーも値も両方使うのに、`Keys`を使ってやろうとする人が結構いるらしいという話を聞きます。要するに以下のような書き方。

<pre class="source" title="C#っぽくない書き方">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> dic = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;
        {
            { <span class="string">"one"</span>, 1 },
            { <span class="string">"two"</span>, 2 },
            { <span class="string">"three"</span>, 3 },
        };

        <span class="reserved">foreach</span> (<span class="reserved">var</span> key <span class="reserved">in</span> <em>dic.Keys</em>)
        {
            <em><span class="reserved">var</span> value = dic[key];</em>
            <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{key}<span class="string"> =&gt; </span>{value}<span class="string">"</span>);
        }
    }
}
</code></pre>

C#の`Dictionary`はキーと値をまとめて列挙できる(`IDictionary<TKey, TValue>`インターフェイスが`IEnumerable<KeyValuePair<TKey, TValue>>`インターフェイスから派生している)ので、以下のように書けます。

<pre class="source" title="C#の書き方">
<code>        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> dic)
        {
            <span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x.Key}<span class="string"> =&gt; </span>{x.Value}<span class="string">"</span>);
        }
</code></pre>

得られる結果が一緒だからどちらでもいいと思うかもしれないですけど、パフォーマンスが結構違います。この手のコレクション(他の言語で言うところの`map`とか`Hashtable`)のインデクサー アクセスはそこそこなコストです。
この例みたいなのだと、`Dictionary`内の要素の数にもよりますが、前者の`Keys`越しの方が2～3倍くらい遅いです。

## 文字列中の文字の列挙

`string`が`IEnumerable<char>`なのも案外気付いていない人がいるとか。

<pre class="source" title="C#っぽくない書き方">
<code><span class="reserved">var</span> s = <span class="string">"aáαあ亜😀"</span>;

<span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; s.Length; i++)
{
    <span class="reserved">var</span> c = s[i];
    <span class="type">Console</span>.WriteLine(c);
}
</code></pre>

C#だと大体は`foreach`で列挙します。

<pre class="source" title="C#の書き方">
<code><span class="reserved">foreach</span> (<span class="reserved">var</span> c <span class="reserved">in</span> s)
{
    <span class="type">Console</span>.WriteLine(c);
}
</code></pre>

というか、[文字列からインデックス使って「N文字目」を取れると思うなよ](http://www.buildinsider.net/language/csharpunicode/01)。

上記の例でも、`foreach`の書き方含め、絵文字が2文字に割れちゃって正しく文字コードを取れません。
C#で正しくサロゲートペアを正しく扱うのはいまだにちょっと面倒なんですが…
いずれ、以下のように書けるようになるはずです。

<pre class="source" title="Utf8String">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Text.Utf8;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> si = <span class="reserved">new</span> <span class="type">Utf8String</span>(<span class="string">"aáαあ亜😀"</span>);

        <span class="reserved">foreach</span> (<span class="reserved">var</span> c <span class="reserved">in</span> si.CodePoints)
        {
            <span class="type">Console</span>.WriteLine(c);
        }
    }
}
</code></pre>

逆に、この`Utf8String`からは、インデックスを使って「N文字目」を取る手段はなくなっています。

## `Format("{0} {0}", x)`

C# 6で[interpolation](../../../../study/csharp/start/st_string.md#string-interpolation)が入った今、あんまり使うものではなくなりましたが、`string.Format`の呼び方に関して。

interpolation でも書けない書き方なんですけども、以下のように、同じインデックスを複数回使う書き方ができたりします。

<pre class="source" title="C#の書き方">
<code><span class="type">Console</span>.WriteLine(<span class="string">"({0} + {1}) × ({0} - {1}) = {0}^2 - {1}^2"</span>, <span class="string">"x"</span>, <span class="string">"y"</span>);
<span class="comment">// (x + y) × (x - y) = x^2 - y^2</span>
</code></pre>

わざわざ、以下のような書き方をしてしまう人をちらほら見かけるとか

<pre class="source" title="C#っぽくない書き方">
<code><span class="type">Console</span>.WriteLine(<span class="string">"({0} + {1}) × ({2} - {3}) = {4}^2 - {5}^2"</span>, <span class="string">"x"</span>, <span class="string">"y"</span>, <span class="string">"x"</span>, <span class="string">"y"</span>, <span class="string">"x"</span>, <span class="string">"y"</span>);
</code></pre>

`printf`だとこんな感じで書いてましたもんね…

## 文字列の `+` 演算

以下のようなコードをC#で書くと、結果はどうなるでしょう。

<pre class="source" title="問題">
<code><span class="reserved">string</span> s1 = <span class="string">"abc"</span>;
<span class="reserved">object</span> s2 = <span class="reserved">null</span>;
<span class="type">Console</span>.WriteLine(s1 + s2);
</code></pre>

選択肢:

1. ぬるぽ(`NullReferenceException`発生)
1. `abc`が表示される
1. `abcnull`が表示される

答えは2番です。C#で、nullを文字列連結すると、空文字扱いになります。

Javaは3番になるんでしたっけ？nullが`"null"`に化けるっていう。

言われてみると、言語ごとに挙動が微妙に違ってちょっとめんどくさいですね、これ。

どっちもどっちというか、文字列連結に`+`演算子を使うって発想がまず、本当によかったのかどうかという疑問があります…

C#文化では、ガイドラインとして「演算子は、組み込み型のものと全然違う用途でオーバーロードするな」というものがあります。
となると、「組み込み型の`+`は足し算だろ、足し算として使えよ」と言われても仕方がなく。
「文字列連結は足し算といえるか」という命題ではあるんですが。
連結の結果、文字列長が足し算になるので足し算的な何かと言えなくもないですけど、きわどい。

ストリームの読み書きにシフト演算子(`<<`)を使われるよりは幾分かマシですけど、
文字列に対する`+`もやめといた方がよかったんじゃないかなぁ…
「顧客が本当に欲しかったものは[interpolation](../../../../study/csharp/start/st_string.md#string-interpolation)だった」説もありますし。

## `if (x)`

これはC言語方面から来た人がたまーにやらかして、ほんとみんな迷惑するやつなんですが…
`operator true`とかに変な実装を入れてしまうことがあります。

やらかす人は「[null関係の演算子](../tipsnulloperation/index.md)」の回で話した 「nullじゃないのに`x == null`がtrueになる」っていうコードとセットでやらかすんですが…

以下のようなコード。

<pre class="source" title="C#っぽくない書き方">
<code><span class="reserved">class</span> <span class="type">MyObject</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">bool</span> _isDisposed;

    <span class="reserved">public</span> <span class="reserved">void</span> Dispose()
    {
        <span class="comment">// Dispose 後、もうこのオブジェクトは無効</span>
        _isDisposed = <span class="reserved">true</span>;
    }

    <span class="comment">// 無効だったら if (x) { } で {} の中を通らなくする</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">true</span>(<span class="type">MyObject</span> obj) =&gt; !obj._isDisposed;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">false</span>(<span class="type">MyObject</span> obj) =&gt; obj._isDisposed;
}
</code></pre>

使う側は以下のような感じ。

<pre class="source" title="C#っぽくない書き方">
<code><span class="reserved">static</span> <span class="reserved">void</span> M(<span class="type">MyObject</span> obj)
{
    <span class="type">Console</span>.WriteLine(<span class="string">"----"</span>);
    <span class="reserved">if</span> (obj) <span class="type">Console</span>.WriteLine(<span class="string">"有効"</span>);
}
</code></pre>

C言語だと`if (x)`って結構書いてたましたもんね…
`bool`って概念を持っていなくて、0以外の値は全てtrue扱い(nullは0)で。
間違えて意図しない条件を`if`の中に書いてしまうので良くないと言われています。

良くないから、C#ではわざわざ書けなくしたものでして…
それをぶり返すような`operator`を書かれると結構困惑します。
