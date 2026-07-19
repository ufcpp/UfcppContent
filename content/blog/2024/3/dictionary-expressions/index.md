---
title: "C# 13 でのコレクション式 - ディクショナリ式"
source_url: "https://ufcpp.net/blog/2024/3/dictionary-expressions/"
content_type: "BlogEntry"
published_at: "2024-03-16T18:17:03"
updated_at: "2024-03-16T18:17:03"
tags: []
umbraco_id: 2494
parent_id: 2490
sort_order: 3
aliases: []
---

# C# 13 でのコレクション式 - ディクショナリ式

C# 13でのコレクション式関連、量が多いのでちょっとずつ取り上げシリーズ。

* [[Proposal]: Collection Expressions Next (C#13 and beyond)](https://github.com/dotnet/csharplang/issues/7913)

今日はディクショナリ式の話を。

* ディクショナリ式 ← 今日はこれ
* 自然な型
* インラインなコレクション式
* コレクションに対する拡張メソッド
* 現状でコレクション式に対応してない型
* 非ジェネリックなコレクションのサポート
* [制限の緩和](../colexpr13-relax-restriction/index.md)

## ディクショナリ式

C# 12 でコレクション式が入りましたが、`Dictionary<TKey, TValue>` などのディクショナリ系の型に対しては使えませんでした。

<pre class="source" title="ディクショナリに対するコレクション式">
<span class="comment">// C# 12 でも空っぽのディクショナリは作れるのに…</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d</span> <span class="operator">=</span> [];

<span class="comment">// 要素があるものは書く手段がない(以下はいずれもエラー)。</span>
<span class="comment">// スケジュールの都合で意図的に「C# 13 でやる」計画。</span>

<span class="comment">// KeyValuePair とかタプルも受け付けず。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d1</span> <span class="operator">=</span> <span class="error" title="CS9215">[<span class="static"><span class="type">KeyValuePair</span></span><span class="operator">.</span><span class="method"><span class="static">Create</span></span>(<span class="string">&quot;&quot;</span>, <span class="number">1</span>)]</span>;
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d2</span> <span class="operator">=</span> <span class="error" title="CS9215">[<span class="error" title="CS0029">(<span class="string">&quot;&quot;</span>, <span class="number">1</span>)</span>]</span>;

<span class="comment">// コレクション初期化子/オブジェクト初期化子みたいな構文も受け付けず。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d3</span> <span class="operator">=</span> [<span class="error" title="CS1001">{</span><span class="string">&quot;&quot;</span>, <span class="number">1</span><span class="error" title="CS1022"><span class="error" title="CS1003"><span class="error" title="CS1002">}</span></span></span><span class="error" title="CS1022">]</span>;
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d4</span> <span class="operator">=</span> [<span class="error" title="CS0131">[<span class="string">&quot;&quot;</span>]</span> <span class="operator">=</span> <span class="number">1</span>];
</pre>

C# 12 時点で[コレクション式](../../../../study/csharp/datatype/collection-expression.md)に対する背景と同じく、
ディクショナリについても以下の需要が見込まれます。

* 簡素に書きたい
* いろいろな種類のディクショナリ系の型に対して共通で使える構文にしたい
* 特に、既存のコレクション初期化子では使えない immutable な型にも対応したい

まあ、
GitHub を軽くクロールしてみて利用頻度を調べると、
リストや配列と比べたらディクショナリの利用率は10%くらいらしいです。
とはいえ、10%もそこそこ大きな数字。
C# 12 時点では後回しになりましたが、13候補としては有力です。

提案ドキュメント、関連デザインミーティング等:

* [[Proposal]: Dictionary expressions #7822](https://github.com/dotnet/csharplang/issues/7822)
* [Dictionary Expressions](https://github.com/dotnet/csharplang/blob/main/proposals/dictionary-expressions.md)
* [C# Language Design Meeting for March 11th, 2024](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-03-11.md)

まあ検討が始まったばかりなので、まだまだ結論の出ていない検討事項も多数。
とりあえず今日は3月11日のミーティング議事録をベースにした話を書こうかと思います。

## 構文の候補

まだ構文をどうするかも決定ではないんですが、現状の最有力候補は `[key: value]` みたいな書き方です。

<pre class="source" title="ディクショナリ式の候補文法">
<span class="comment">// 「ディクショナリ式」の最有力候補の文法:</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d</span> <span class="operator">=</span> [
    <span class="string">&quot;one&quot;</span>: <span class="number">1</span>,
    <span class="string">&quot;two&quot;</span>: <span class="number">2</span>,
    ];
</pre>

もちろん、「JavaScript では `{}` を使うけども」みたいな別案もあるんですが、
まあ、C# 12 のコレクション式に合わせて `[]` になると思われます。

ちなみに、最初期には「`[]` の外でも `key: value` で `KeyValuePair` を作れるようにするべきか？」みたいな見当もありましたが、
現状それには否定的で、 `[]` の中限定の構文になりそうです。

<pre class="source" title="没案">
<span class="comment">// 没案「KeyValuePair 式」。</span>
<span class="type struct">KeyValuePair</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">kvp</span> <span class="operator">=</span> <span class="string">&quot;one&quot;</span>: <span class="number">1</span>;
</pre>

## 検討事項1: KeyValuePair を並べる

ディクショナリ式中では、`key: value` みたいな形式のみを受け付けるか、それとも、`KeyValuePair` であれば直接書けるようにするかという話があります。

<pre class="source" title="KeyValuePair を直接書けるようにする案">
<span class="comment">// key: value のみ。これは問題ない。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d</span> <span class="operator">=</span> [<span class="string">&quot;one&quot;</span>: <span class="number">1</span>];

<span class="reserved">var</span> <span class="variable">kvp</span> <span class="operator">=</span> <span class="type"><span class="static">KeyValuePair</span></span><span class="operator">.</span><span class="method"><span class="static">Create</span></span>(<span class="string">&quot;two&quot;</span>, <span class="number">2</span>);

<span class="comment">// KeyValuePair をいちいち展開する必要はあるかどうか。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d1</span> <span class="operator">=</span> [<span class="string">&quot;one&quot;</span>: <span class="number">1</span>,</span></span> <span class="variable">kvp</span><span class="operator">.</span><span class="property">Key</span>: <span class="variable">kvp</span><span class="operator">.</span><span class="property">Value</span>];

<span class="comment">// こう書きたい需要はある。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d2</span> <span class="operator">=</span> [<span class="string">&quot;one&quot;</span>: <span class="number">1</span>, <span class="variable">kvp</span>];
</pre>

`["one": 1, kvp]` と書けるようにする案には肯定的な人が多く、承認されそうです。

## 検討事項2: KeyValuePair のリストを Spread する

検討事項1と似たような話ですが、`IEnumerable<KeyValuePair<TKey, TValue>>` とかをディクショナリ式中に含められるかという話もあります。

<pre class="source" title="ディクショナリ式中で KeyValuePair のリストを Spread">
<span class="reserved">var</span> <span class="variable">kvps</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="static"><span class="type">KeyValuePair</span></span><span class="operator">.</span><span class="static"><span class="method">Create</span></span>(<span class="string">&quot;two&quot;</span>, <span class="number">2</span>) };

<span class="comment">// .. で展開すると KeyValuePair になるわけで、</span>
<span class="comment">// KeyValuePair を認めるのなら、 ..(KeyValuePair のリスト) も認めたい。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d1</span> <span class="operator">=</span> [..<span class="variable">kvps</span>];

<span class="comment">// 混在も需要あり。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d2</span> <span class="operator">=</span> [<span class="string">&quot;one&quot;</span>: <span class="number">1</span>, ..><span class="variable">kvps</span>];

<span class="comment">// 特に、「複数のディクショナリのマージ」みたいな用途で以下のように書きたい。</span>
<span class="reserved">var</span> <span class="variable">kvps1</span> <span class="operator">=</span> <span class="reserved">new</span>[] { <span class="type"><span class="static">KeyValuePair</span></span><span class="operator">.</span><span class="static"><span class="method">Create</span></span>(<span class="string">&quot;three&quot;</span>, <span class="number">3</span>) };
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d3</span> <span class="operator">=</span> [..<span class="variable">kvps</span>, ..<span class="variable">kvps1</span>];
</pre>

これも認める方向で検討されています。

## 検討事項3: ディクショナリじゃなくて KeyValuePair のリスト

`[]` 中の `key: value` は「`KeyValuePair` を作るための簡易記法」みたいなものになっているわけですが、
だったら以下のような「ディクショナリじゃないただのコレクションに対して使えるか」という話が出てきます。

<pre class="source" title="KeyValuePair のリストに対してディクショナリ式">
<span class="comment">// 「ディクショナリ式」の最有力候補の文法:</span>
<span class="type">List</span>&lt;<span class="type">KeyValuePair</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt;&gt; <span class="variable">d</span> <span class="operator">=</span> [
    <span class="string">&quot;one&quot;</span>: <span class="number">1</span>,
    <span class="string">&quot;two&quot;</span>: <span class="number">2</span>,
    ];
</pre>

これも需要がそれなりにありそうです。
.NET の BCL とか、 Roslyn 中のコードでもオプションとかをディクショナリではなくて `IEnumerable<KeyValuePair<TKey, TValue>>` 引数で受け取っているものがそれなりにあるそうで。

それに、先ほどの `Dictionary<string, int> d3 = [..kvps, ..kvps1];` みたいなもので、マージ元になる `kvps` などはディクショナリではなくて `KeyValuePair` のリストということは十分ありそうな話です。

ということで、これも承認されそうです。

## 検討事項4: KeyValuePair 以外の要素は認められるか

`Dictionary<TValue, TKey>` とかでは要素の列挙などに `KeyValuePair<TValue, TKey>` を使うことが多いですが、
ディクショナリ式を作るにあたって `KeyValuePair` だけに絞るか、それとも他の型も使えるようにするかという問題もあります。

例えば、タプル導入時にも、`(TKey key, TValue value)` はほぼ `KeyValuePair<TKey, TValue>` と同等」みたいなことを言われています。
割かし最近 BCL に追加された [`PriorityQueue`](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.generic.priorityqueue-2) なんかは、`(TElement Element, TPriority Priority)` で要素とその優先度の列挙をします。
[`Zip`](https://learn.microsoft.com/ja-jp/dotnet/api/system.linq.enumerable.zip?view=net-8.0#system-linq-enumerable-zip-2(system-collections-generic-ienumerable((-0))-system-collections-generic-ienumerable((-1)))) なんかも `(TFirst First, TSecond Second)` で結果を列挙します。
こういうものを直接 `[]` の中で `..` で展開したかったりはします。

あとは、`KeyValuePair` を特別扱いするとしても、暗黙の型変換を認めるかどうか。

<pre class="source" title="">
<span class="reserved">struct</span> <span class="type struct">Pair</span>&lt;<span class="type param">X</span>, <span class="type param">Y</span>&gt;(<span class="type param">X</span> <span class="variable local"><span class="warning" title="CS9113">x</span></span>, <span class="type param">Y</span> <span class="variable local"><span class="warning" title="CS9113">y</span></span>)
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type struct">KeyValuePair</span>&lt;<span class="type param">X</span>, <span class="type param">Y</span>&gt;(<span class="type struct">Pair</span>&lt;<span class="type param">X</span>, <span class="type param">Y</span>&gt; <span class="variable local">pair</span>) <span class="operator">=&gt;</span> </span>..<span class="operator">.;
}

<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d</span> <span class="operator">=</span> 
[
    <span class="reserved">new</span> <span class="type struct">Pair</span>(<span class="string">&quot;one&quot;</span>, <span class="number">1</span>),
    .. <span class="reserved">new</span>[] { <span class="reserved">new</span> <span class="type struct">Pair</span>(<span class="string">&quot;two&quot;</span>, <span class="number">2</span>) }
];
</pre>

これについては結論はまだ出ていないみたいです。

## 検討事項5: Add か、インデクサーか

まず、ディクショナリ式ではキーの重複を認めるかどうかという話があります。
例えば、`ToDictionary` なんかでは、キーが重複していると例外を出します。

<pre class="source" title="ToDictionary はキーの重複ダメ">
<span class="reserved">var</span> <span class="variable">d</span> <span class="operator">=</span> <span class="reserved">new</span>[] { (<span class="number">1</span>, <span class="number">10</span>), (<span class="number">1</span>, <span class="number">20</span>) }
    <span class="operator">.</span><span class="method">ToDictionary</span>(<span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="field">Item1</span>); <span class="comment">// ArgumentException</span>
</pre>

が、まあ、前述の2個のディクショナリをマージするようなケースでは重複を認める方がよかったりします。
オプション指定とかだと「デフォルト設定と、ユーザーごとの設定をマージ、後で追加した方を優先」みたいな使い方を結構しますし。

ただ、「重複を認めるかどうか」という観点だと、結局は「ターゲットにする型によって挙動が違う」ということになります。
例えば、以下のような感じ。

* `Dictionary<TKey, TValue>` の `Add` は重複を認めていない
* `ImmutableDictionary<TKey, TValue>` の `Add` は上書き(上書きした新しいインスタンスを作って返す)
* `FrozenDictionary<TKey, TValue>` の `Add` (`ICollection` インターフェイス越しに呼べちゃう) は `NotSupported` 例外を出す

なので結局は「どういう動作にするか」は決めれなくて、「`Add` とインデクサーのどちらを使うか」という話になります。

<pre class="source" title="ディクショナリ式の初期化はどちらにすべきか">
<span class="comment">// Add で初期化。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d1</span> <span class="operator">=</span> <span class="reserved">new</span>();
<span class="variable">d1</span><span class="operator">.</span><span class="method">Add</span>(<span class="string">&quot;a&quot;</span>, <span class="number">1</span>);
<span class="variable">d1</span><span class="operator">.</span><span class="method">Add</span>(<span class="string">&quot;b&quot;</span>, <span class="number">2</span>);

<span class="comment">// インデクサで初期化。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d2</span> <span class="operator">=</span> <span class="reserved">new</span>();
<span class="variable">d2</span>[<span class="string">&quot;a&quot;</span>] <span class="operator">=</span> <span class="number">1</span>;
<span class="variable">d2</span>[<span class="string">&quot;b&quot;</span>] <span class="operator">=</span> <span class="number">2</span>;
</pre>

ちなみにこれらは、現状のコレクション初期化子・オブジェクト初期化子を使うと以下のように書けるやつです。

<pre class="source" title="Dictionary に対するコレクション初期化子・オブジェクト初期化子">
<span class="comment">// Add での初期化になるコレクション初期化子。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d1</span> <span class="operator">=</span> <span class="reserved">new</span>()
{
    { <span class="string">&quot;a&quot;</span>, <span class="number">1</span> },
    { <span class="string">&quot;b&quot;</span>, <span class="number">2</span> }
};

<span class="comment">// インデクサでの初期化になるオブジェクト初期化子。</span>
<span class="type">Dictionary</span>&lt;<span class="reserved">string</span>, <span class="reserved">int</span>&gt; <span class="variable">d2</span> <span class="operator">=</span> <span class="reserved">new</span>()
{
    [<span class="string">&quot;a&quot;</span>] <span class="operator">=</span> <span class="number">1</span>,
    [<span class="string">&quot;b&quot;</span>] <span class="operator">=</span> <span class="number">2</span>
};
</pre>

`["a": 1, "b": 2]` はどちらになるかという話なわけですが、
現状はインデクサー案が有力みたいです。
コレクション初期化子(`Add` になる)と食い違うという懸念もありますが、
インデクサーの方が都合がよさどうという判断になっています。
例えば先ほど例に挙げた `[..defaultSettings, ..userSettings]` みたいなケースで重複を認めている方がよさそうで、
`Dictionary<TKey, TValue>` の場合は「`Add` は重複不可、インデクサーは可」ですし。
