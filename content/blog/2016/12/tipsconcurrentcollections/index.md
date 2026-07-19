---
title: "小ネタ Concurrent コレクション"
source_url: "https://ufcpp.net/blog/2016/12/tipsconcurrentcollections/"
content_type: "BlogEntry"
published_at: "2016-12-24T00:23:54"
updated_at: "2016-12-30T00:45:45"
tags: []
umbraco_id: 2007
parent_id: 1969
sort_order: 23
aliases: []
---

# 小ネタ Concurrent コレクション

.NET 4以来、[`System.Collections.Concurrent`](https://msdn.microsoft.com/ja-jp/library/system.collections.concurrent.aspx)以下に、
Concurrentなコレクションがいくつか追加されました。

Concurrent、英単語の意味としては「同時に起こる」という意味の形容詞。
プログラミングにおいては、「複数のプログラムやスレッドから同時にアクセスされる」という意味で使われ、
「並行」とか「同時実行」とか訳されます。
たいてい、「Concurrentなんとか」みたいな名前のものは「同時実行があっても問題が起きない」という意味になります。

ただし、「問題を起こさない」って言ってもいろいろな意味があって、それぞれのコレクションの性質をちゃんとわかっておかないと困ったりします。
(.NET の`System.Collections.Concurrent`に限らず、たいていのプログラミング言語のたいていのライブラリで、Concurrentと名の付くものは同様の注意が必要です。)

ということで、今日は[`ConcurrentDictionary`](https://msdn.microsoft.com/ja-jp/library/dd287191.aspx)の`GetOrAdd`メソッドを例にとって挙動をちょっと説明。

## GetOrAdd

こいつ: [`GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)`](https://msdn.microsoft.com/ja-jp/library/ee378677.aspx)

名前通り、キーに応じた値がすでにあればその値を返し、なければ `valueFactory` を呼んで、新しい値を作って辞書に登録しつつ、その作った値を返します。

話を簡単にするために、まずちょっと、同時実行が必要ない状況で例を出しますが、以下のような挙動になります。

<pre class="source" title="GetOrAdd: 同時実行がない場合">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Concurrent;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">const</span> <span class="reserved">int</span> theKey = 1;
        <span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">ConcurrentDictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">string</span>&gt;();

        <span class="comment">// まず、GetOrAdd の同時実行が起こらない場合を見てみる</span>
        <span class="comment">// 普通の逐次実行なので、同時実行にはならない</span>
        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 4; i++)
        {
            <span class="reserved">var</span> item = d.GetOrAdd(theKey, key =&gt;
            {
                <span class="comment">// インスタンス新規作成</span>
                <span class="comment">// 単一のキーでアクセスしているので1回限り</span>
                <span class="type">Console</span>.WriteLine(<span class="string">$"Add: </span>{i}<span class="string">"</span>);
                <span class="reserved">return</span> i.ToString();
            });

            <span class="comment">// 同じインスタンスが返ってきているか確認</span>
            <span class="type">Console</span>.WriteLine(<span class="string">$"Get: </span>{item}<span class="string">"</span>);
        }
    }
}
</code></pre>

<pre class="source" title="実行結果">
<code>Add: 0
Get: 0
Get: 0
Get: 0
Get: 0
</code></pre>

この例では、同じキーで何度も `GetOrAdd` を呼んでいます。
値の生成(`$"Add: {i}"`と表示される部分)は最初の1回でしか通りません。

## 並列動作

この`for`ループを並列化することを考えます。

### ConcurrentDictionary の必要性

単に同時実行で問題を起こさないようにするなら、わざわざ`ConcurrentDictionary`なんていう新しいクラスを作らなくても、
`lock`ステートメントを掛ければ済む話です。

Concurrentを名乗らない普通の`Dictionary`を使って、
自前で`lock`を掛けるのであれば、例えば以下のように書けばいいでしょう。

<pre class="source" title="Dictionaryに対して自前でlockを掛ける GetOrAdd 実装">
<code><span class="reserved">static</span> <span class="reserved">class</span> <span class="type">DictionaryExtensions</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">TValue</span> GetOrAdd&lt;<span class="type">TKey</span>, <span class="type">TValue</span>&gt;(<span class="reserved">this</span> <span class="type">IDictionary</span>&lt;<span class="type">TKey</span>, <span class="type">TValue</span>&gt; d, <span class="type">TKey</span> key, <span class="type">Func</span>&lt;<span class="type">TKey</span>, <span class="type">TValue</span>&gt; valueFactory)
    {
        <span class="reserved">lock</span> (d)
        {
            <span class="type">TValue</span> value;
            <span class="reserved">if</span> (!d.TryGetValue(key, <span class="reserved">out</span> value))
            {
                value = valueFactory(key);
                d[key] = value;
            }
            <span class="reserved">return</span> value;
        }
    }
}
</code></pre>

このコードの何が嫌かというと、`lock`範囲が広すぎること。

- Getにも`lock`が掛かる。新規追加(Add)の頻度が低い時に完全に無駄
- Add のときに、`valueFactory`呼び出し中にもずっと`lock`が掛かっていて、`valueFactory`の中身次第では`lock`時間が長くなりすぎる

`lock`は、意外と重たい処理です。可能な限り避けて、可能な限り短くする必要があります。

`ConcurrentDictionary`は、`lock`範囲を極力小さくすることで、パフォーマンス向上を図っているクラスです。

### ConcurrentDictionaryの癖

ただし、`ConcurrentDictionary`の`GetOrAdd`には少々癖があります。

[ドキュメント](https://msdn.microsoft.com/ja-jp/library/ee378677.aspx)をちゃんと読むと書いてあるんですが、

- `valueFactory`は複数回呼ばれる可能性があります
- 返す値・辞書内に格納する値は必ず1つであることが保証されています

という挙動。

その結果、最初にあげた例で、`for`ループを`Parallel.For`に変えて並列化すると、以下のような挙動をします。

<pre class="source" title="GetOrAdd: 同時実行する場合">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Concurrent;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">const</span> <span class="reserved">int</span> theKey = 1;
        <span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">ConcurrentDictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">string</span>&gt;();

        <span class="comment">// 並列動作</span>
        <span class="comment">// 並列なので、ループの中身が複数のスレッドで同時に動くことがある</span>
        <span class="type">Parallel</span>.For(0, 4, i =&gt;
        {
            <span class="reserved">var</span> item = d.GetOrAdd(theKey, key =&gt;
            {
                <span class="comment">// 同時に来られると、ここは複数回動く可能性がある</span>
                <span class="type">Console</span>.WriteLine(<span class="string">$"Add: </span>{i}<span class="string">"</span>);
                <span class="reserved">return</span> i.ToString();
            });

            <span class="comment">// Add が複数回動いても、Get で帰ってくる値は必ず単一の保証あり</span>
            <span class="type">Console</span>.WriteLine(<span class="string">$"Get: </span>{item}<span class="string">"</span>);
        });
    }
}
</code></pre>

実行する環境によって/実行するたびに結果は異なりますが、一例としては以下のような実行結果になります。

<pre class="source" title="実行結果">
<code>Add: 0
Add: 3
Get: 0
Add: 1
Get: 0
Add: 2
Get: 0
Get: 0
</code></pre>

(この環境では)Addは4回動いています。
しかし、戻り値として返っているのはそのうち1つだけで、Getのところに表示されている値は全部同じです。

### 癖の回避: Lazyとの組み合わせ

`lock`を減らすためとはいえ、ちょっと癖のある挙動です。この癖を回避したければ一工夫要ります。
その工夫として、別途、[`Lazy`クラス](https://msdn.microsoft.com/ja-jp/library/dd642331.aspx)(`System`名前空間)と組み合わせる方法があります。

以下のような書き方をします。

<pre class="source" title="ConcurrentDictionary と Lazy の併用">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Concurrent;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">const</span> <span class="reserved">int</span> theKey = 1;
        <span class="reserved">var</span> d = <span class="reserved">new</span> <span class="type">ConcurrentDictionary</span>&lt;<span class="reserved">int</span>, <span class="type">Lazy</span>&lt;<span class="reserved">string</span>&gt;&gt;(); <span class="comment">// 値を Lazy&lt;string&gt; に変える</span>

        <span class="comment">// 並列動作</span>
        <span class="comment">// 並列なので、ループの中身が複数のスレッドで同時に動くことがある</span>
        <span class="type">Parallel</span>.For(0, 4, i =&gt;
        {
            <span class="reserved">var</span> lazy = d.GetOrAdd(theKey, key =&gt; <span class="reserved">new</span> <span class="type">Lazy</span>&lt;<span class="reserved">string</span>&gt;(() =&gt;
            {
                <span class="comment">// 複数個の Lazy インスタンスが作られることはあるけども、</span>
                <span class="comment">// Lazy が作られただけでは valueFactory は呼ばれない</span>
                <span class="type">Console</span>.WriteLine(<span class="string">$"Add: </span>{i}<span class="string">"</span>);
                <span class="reserved">return</span> i.ToString();
            }));

            <span class="comment">// lazy 自体は単一のインスタンスが返る保証あり</span>

            <span class="comment">// この時点で初めて Add: の行が呼ばれる</span>
            <span class="comment">// Lazy のデフォルトの挙動では、valueFactory が呼ばれるのは1回限りの保証あり</span>
            <span class="reserved">var</span> item = lazy.Value;

            <span class="type">Console</span>.WriteLine(<span class="string">$"Get: </span>{item}<span class="string">"</span>);
        });
    }
}
</code></pre>

結果は以下のようになります。

<pre class="source" title="実行結果">
<code>Add: 0
Get: 0
Get: 0
Get: 0
Get: 0
</code></pre>

値は0である保証はなくて、`Add: 1`とか`Add: 2`が表示されることもありますが、少なくとも

- `Add:`の行が表示されるのは1回限り
- `Add:`の行と`Get:`の行で表示されている値は同じ

という保証はされています。

これで、`GetOrAdd`全体を`lock`するよりはだいぶパフォーマンスのよいコードになります。
特に、Addがほとんどなく、Get頻度が高い場合にはかなり顕著な差になるでしょう。
