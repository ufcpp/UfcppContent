---
title: "オーバーロード解決優先度"
source_url: "https://ufcpp.net/blog/2024/2/overload-resolution-priority/"
content_type: "BlogEntry"
published_at: "2024-02-18T17:56:51"
updated_at: "2024-02-24T22:19:33"
tags: []
umbraco_id: 2487
parent_id: 2480
sort_order: 6
aliases: []
---

# オーバーロード解決優先度

今日は「負の遺産整理で消したいけども消せないメソッド対処」の話。
紆余曲折合って、現状、`OverloadResolutionPriority` 属性でオーバーロード解決に優先度をつけて、
優先度の高いものだけを見るようにするという案になっています。

最近のわかりやすい例だと、「パフォーマンス改善のために配列引数を `ReadOnlySpan` 引数に変えたい」というのをやりたいとします。

元々、配列引数で作っていたとして、

<pre class="source" title="元コード">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);

<span class="comment">// 元コード。</span>
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// これの引数を変えたい。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }
}
</pre>

暗黙的型変換があるものであれば、多少型を変えても「再コンパイルすれば大丈夫」という状態になることはあります。

<pre class="source" title="再コンパイルすれば大丈夫なこともある">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// int[] → ReadOnlySpan&lt;int&gt; の変更は、再コンパイルするならエラーにならず移行可能。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);

<span class="comment">// 変更後コード。</span>
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 引数、変えちゃった。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}
</pre>

ただ、「再コンパイル必須」というのは、
末端のアプリならともかく、ライブラリとかプラグインとかにとってはきついです。
過去にコンパイル済みバイナリの形でライブラリ参照すると、
先ほどの例は「`M(int[])` が見つからない」という実行時例外を起こします。

なので、現実的には「メソッドは追加する一方」になりがちなんですが、
非推奨にしたい古いメソッドによって利便性が損なわれることが多々あります。

<pre class="source" title="メソッドは追加する一方になりがち">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// 普通に書くと int[] の方に行っちゃう。</span>
<span class="comment">// パフォーマンスを理由に ReadOnlySpan&lt;int&gt; オーバーロードを足したのに無意味。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);

<span class="comment">// 変更後コード。</span>
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 元のメソッドは残しつつ、</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }
    <span class="comment">// オーバーロードを追加。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}
</pre>

非推奨にしたいものには `Obsolete` 属性を付けるという手段はありますが、
`Obsolete` 属性を付けたところでオーバーロード解決候補には残ってしまうのがかなり邪魔です。

<pre class="source" title="Obsolete でもオーバーロード解決候補になっちゃう">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// C# コンパイラーは Obsolete 属性が付いたメソッドも普通にオーバーロード解決候補にしちゃう。</span>
<span class="comment">// ReadOnlySpan&lt;int&gt; の方を呼んでほしくてやってるのに、</span>
<span class="comment">// 実際は int[] が選ばれたうえで警告が出るだけになる。</span>
<span class="warning" title="CS0618"><span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>)</span>;

<span class="comment">// 変更後コード。</span>
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 古い方には Obsolete 属性を付ける。</span>
    [<span class="type">Obsolete</span>(<span class="string">&quot;Use M(ReadOnlySpan&lt;int&gt; x) instead.&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}
</pre>

ということで、「バイナリ互換性のために残すけども、コンパイル時のオーバーロード解決候補には残さない」
(過去にコンパイルした DLL からは見えてるけども、ソースコードの再コンパイル時には見えない)
という状態を作りたいという要望があります。
これを「binary compat only」とか呼んでいます。

最初に思いつく案としては、`Obsolete` 属性に手を入れる方法。

<pre class="source" title="Obsolete 属性修正案">
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 最初に思いつく案として、Obsolete 属性を修正。</span>
    [<span class="type">Obsolete</span>(<span class="string">&quot;Use M(ReadOnlySpan&lt;int&gt; x) instead.&quot;</span>, <span class="type">ObsoleteLevel</span><span class="operator">.</span>BinaryCompatOnly)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}
</pre>

ただ、既存の `Obsolete` 属性に手を入れる案だと、例えば netstarndard2.0 向けライブラリとか、
ターゲットフレームワーク古いライブラリに対して使えなくなります。
なので新しい属性を足さざるを得ず。

当初案はまんま `BinaryCompatOnly` 属性でした。

* [Add proposal for BinaryCompatOnlyAttribute](https://github.com/dotnet/csharplang/pull/7707)

<pre class="source" title="BinaryCompatOnly 案">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// ReadOnlySpan&lt;int&gt; の方が選ばれるようになる予定。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 新属性で「オーバーロード解決候補から外す」指定。</span>
    [<span class="type">BinaryCompatOnly</span>]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">BinaryCompatOnlyAttribute</span> : <span class="type">Attribute</span>;
</pre>

ところが、じゃあ、「完全に候補から外す」だけでいいのかというと、そうでもなくて困ったみたいです。
例えば、インターフェイスの実装とかはどうするの？ということになりました。

<pre class="source" title="「完全に候補から外す」だけでいいのかどうか問題">
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// 新属性で「オーバーロード解決候補から外す」指定。</span>
    [<span class="type">BinaryCompatOnly</span>]
    <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>[] <span class="variable local">x</span>);

    <span class="comment">// 新規追加メソッド。</span>
    <span class="reserved">void</span> <span class="method">M</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>);
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span> : <span class="type">I</span>
{
    <span class="comment">// BinaryCompatOnly = コンパイル時には見えない</span>
    <span class="comment">// なわけで、 I.M(int[]) も「見えない」 = 実装できないのが正しい？</span>
    <span class="comment">//</span>
    <span class="comment">// こっちの M(int[]) にも BinaryCompatOnly 属性を付けることを義務付ける？</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">BinaryCompatOnlyAttribute</span> : <span class="type">Attribute</span>;
</pre>

そこで最終的に、

* オーバーロード解決に優先度をつけれるようにする
  * 何も指定がないときを 0 として、数字が大きいほど優先度を上げ、小さいほど下げる
* 優先度が一番高いものだけを候補にする

という案に修正されました。
属性名は `OverloadResolutionPriority`。

* [Add proposal for overload resolution priority](https://github.com/dotnet/csharplang/pull/7906)

<pre class="source" title="オーバーロード解決の優先度指定">
<span class="reserved">int</span>[] <span class="variable">x</span> <span class="operator">=</span> [<span class="number">1</span>, <span class="number">2</span>, <span class="number">3</span>];

<span class="comment">// ReadOnlySpan&lt;int&gt; の方が選ばれるようになる予定。</span>
<span class="type">C</span><span class="operator">.</span><span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="comment">// 優先度を上げたければ priority の数字を増やす。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="number">1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>) { }

    <span class="comment">// 逆にこっちに priority = -1 とかを与えて優先度を下げるとかでも OK。</span>
    [<span class="type">OverloadResolutionPriority</span>(<span class="number">-1</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">int</span>[] <span class="variable local">x</span>) { }
}

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">OverloadResolutionPriorityAttribute</span>(<span class="reserved">int</span> <span class="variable local">priority</span>) : <span class="type">Attribute</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Priority</span> { <span class="reserved">get</span>; } <span class="operator">=</span> <span class="variable local">priority</span>;
}
</pre>

これなら先ほどのインターフェイスの例みたいな「見なくなりすぎる」問題は回避。
「高優先度のものが見つからなければ単に古い方を見に行く」みたいな挙動になります

まあ、具体化するには検討すべき項目はまだまだあるでしょうが
(例えば優先度は int で何でも受け付けるのでいいか？とか)、
方向性としては、C# チームも強く支持するし、
BCL 側もこれが入れば大々的に使いたい意向ありとのこと。
