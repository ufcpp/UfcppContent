---
title: "配列のダウンキャスト"
source_url: "https://ufcpp.net/blog/2018/12/arraydowncast/"
content_type: "BlogEntry"
published_at: "2018-12-15T12:21:16"
updated_at: "2018-12-15T12:24:23"
tags: []
umbraco_id: 2196
parent_id: 2177
sort_order: 14
aliases: []
---

# 配列のダウンキャスト

今日は [`Unsafe` クラス](../unsafe/index.md)を使った配列の最適化の話。

## `object[]`

.NET Framework 1.x 時代からある古い API の中にはいくつか、
本当は `T` 型の配列なのに `object[]` で戻り値を返してくるようなメソッドがいくつかあります。
`object[]`とまでは言わないものの、基底クラスの配列で返すメソッドは多いです。

1.x 時代には[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)がなかったせいなんですが、今となっては不便ではあります。

### 例: マルチキャスト デリゲート

例を1個。
C# のデリゲートは、複数のメソッドを `+=` で繋いで、一斉に呼び出すという機能があり、
これを[マルチキャスト デリゲート](../../../../study/csharp/functional/sp_delegate.md#malticast)と言います。
例えば以下のコードは、

<pre class="source" title="マルチキャスト デリゲート">
<code><span class="type">Action</span> f = <span class="reserved">null</span>;
 
<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 })
{
    f += () =&gt; <span class="type">Console</span>.WriteLine(<span class="string">$&quot;lambda </span>{i}<span class="string"> invoked&quot;</span>);
}
 
f();
</code></pre>

以下のような結果を出力します。

<pre class="source" title="実行結果">
<code>lambda 1 invoked
lambda 2 invoked
lambda 3 invoked
lambda 4 invoked
lambda 5 invoked
</code></pre>

基本的には[イベント](../../../../study/csharp/functional/sp_event.md)のための機能で、
戻り値は想定していません。`void`戻り値以外のメソッドに使おうとするとトラブります。
以下のようなコードを書いたとすると、

<pre class="source" title="マルチキャスト デリゲートの戻り値">
<code><span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; f = <span class="reserved">null</span>;
 
<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 })
{
    f += () =&gt;
    {
        <span class="type">Console</span>.WriteLine(<span class="string">$&quot;lambda </span>{i}<span class="string"> invoked&quot;</span>);
        <span class="reserved">return</span> i;
    };
}
 
<span class="type">Console</span>.WriteLine(<span class="string">$&quot;f returns </span>{f()}<span class="string">&quot;</span>);
</code></pre>

最後の行の出力は

<pre class="source" title="実行結果">
<code>f returns 5
</code></pre>

になります。要するに、最後の1個の戻り値以外は消えてなくなります。
全ての戻り値を取りたければ以下のように、
個々のデリゲートを配列で受け取って、1つ1つ呼び出すようなコードを書きます。

<pre class="source" title="マルチキャスト デリゲートから、個別のデリゲートを取り出す">
<code><span class="type">Delegate</span>[] list = f.GetInvocationList();
<span class="reserved">foreach</span> (<span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; item <span class="reserved">in</span> list)
    <span class="type">Console</span>.WriteLine(<span class="string">$&quot;f returns </span>{item()}<span class="string">&quot;</span>);
</code></pre>

`f` が `Func<int>` なんだから、`Func<int>[]` で一覧を取りたいところなんですが、
残念ながら `GetInvocationList` の結果は `Delegate[]` で帰ってきます。
それを再び `Func<int>` にダウンキャストして使うことになります。

特に、[`Task`戻り値のデリゲートを `await` したいとき](https://gist.github.com/ufcpp/a72f63d11962f7a5a9a5981b6be31f74)とかに必須の手段です。

## 配列ダウンキャスト

そしてここからが本題。

基底クラスの配列から、元の派生クラスの要素を列挙したい場合、どうするのが最速でしょうか。
`string[]` と `object[]` でベンチマークを取ってみます。

ベンチマーク全体は [Gist](https://gist.github.com/ufcpp/efb726420adc6f8183a5c7a92ff17a61) に置いておきます。
要点を抜き出すと…

比較用データ: 同じ `string` 配列を、`string[]` のフィールドと `object[]` にフィールドに格納して使います。

<pre class="source" title="ベンチマークに使うデータ">
<code><span class="reserved">string</span>[] _stringData = <span class="reserved">new</span> <span class="reserved">string</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;ab&quot;</span>, <span class="string">&quot;abc&quot;</span>, <span class="string">&quot;abcd&quot;</span>, <span class="string">&quot;abcde&quot;</span>, <span class="string">&quot;abcdef&quot;</span>, <span class="string">&quot;abcdefg&quot;</span> };
<span class="reserved">object</span>[] _objectData = <span class="reserved">new</span> <span class="reserved">string</span>[] { <span class="string">&quot;a&quot;</span>, <span class="string">&quot;ab&quot;</span>, <span class="string">&quot;abc&quot;</span>, <span class="string">&quot;abcd&quot;</span>, <span class="string">&quot;abcde&quot;</span>, <span class="string">&quot;abcdef&quot;</span>, <span class="string">&quot;abcdefg&quot;</span> };
</code></pre>

これを、以下の3パターン(+ 参考までに1パターン)のコードに与えてみます。

(1) MemberwiseCast: 要素ごとにダウンキャスト

<pre class="source" title="MemberwiseCast">
<code><span class="reserved">foreach</span> (<span class="reserved">string</span> s <span class="reserved">in</span> _objectData)
    sum += s.Length;
</code></pre>

(2) ArrayCast: 最初に配列自体をダウンキャスト

<pre class="source" title="ArrayCast">
<code><span class="reserved">var</span> data = (<span class="reserved">string</span>[])_objectData;
<span class="reserved">foreach</span> (<span class="reserved">var</span> s <span class="reserved">in</span> data)
    sum += s.Length;
</code></pre>

(3) UnsafeStructCast: 謎の最適化

<pre class="source" title="謎の最適化に使う謎の構造体">
<code><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Wrap</span>&lt;<span class="type">T</span>&gt; { <span class="reserved">public</span> <span class="type">T</span> Value; }
</code></pre>
<pre class="source" title="UnsafeStructCast">
<code><span class="reserved">var</span> data = <span class="type">Unsafe</span>.As&lt;<span class="reserved">object</span>[], Wrap&lt;<span class="reserved">string</span>&gt;[]&gt;(<span class="reserved">ref</span> _objectData);
<span class="reserved">foreach</span> (<span class="reserved">var</span> s <span class="reserved">in</span> data)
    sum += s.Value.Length;
</code></pre>

(参考) Static: 最初から `string[]` の方を列挙

<pre class="source" title="Static">
<code><span class="reserved">foreach</span> (<span class="reserved">var</span> s <span class="reserved">in</span> _stringData)
    sum += s.Length;
</code></pre>

比較の結果、以下のような感じになります。

|           Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
|----------------- |---------:|----------:|----------:|-------:|---------:|
|   MemberwiseCast | 8.552 ns | 0.1170 ns | 0.1094 ns |   2.56 |     0.04 |
|        ArrayCast | 7.074 ns | 0.0952 ns | 0.0844 ns |   2.11 |     0.03 |
| UnsafeStructCast | 3.589 ns | 0.0200 ns | 0.0167 ns |   1.07 |     0.01 |
|           Static | 3.346 ns | 0.0249 ns | 0.0233 ns |   1.00 |     0.00 |

お分かりいただけるだろうか。
1要素ごとにキャストのオーバーヘッドが掛かっていそうな (1) が遅いのは当然として。
最初に1回だけオーバーヘッドが掛かってあとは大丈夫そうに見える (2) がだいぶ遅いという。
そして、「謎の最適化」を自称しているだけあって (3) が倍くらい速い。

## 謎の最適化 `Wrap<T>`

ということで、この謎の最適化が速くなる原理について。

[.NET の配列には共変性](../../../../study/csharp/oop/sp4_variance.md#covariant-array)があります。

<pre class="source" title="配列の共変性">
<code><span class="reserved">string</span>[] derivedItems = { <span class="string">&quot;Aleph&quot;</span>, <span class="string">&quot;Beth&quot;</span>, <span class="string">&quot;Gimel&quot;</span> };
<span class="reserved">object</span>[] baseItems = derivedItems; <span class="comment">// この代入は明示的なキャストなしでできる</span>
</code></pre>

だいたいこいつが犯人。

この状況で、`baseItems[i] = 10;` とか書いてしまうとまずいことになります。
なので、`baseItems[i]` に対していちいち型チェックが挿入されていて、
本来の型と違う型の値を代入しようとすると例外が飛びます。
その型チェックのコストが、前節の (2) が遅くなる原因。

ちなみに、共変性は参照型にしか働かないので、例えば以下のようなコードはコンパイル エラーになります。`int` は値型なので、共変ではなくなります。

<pre class="source" title="値型の配列には共変性が働かない">
<code><span class="reserved">int</span>[] derivedItems = { 1, 2, 3 };
<span class="reserved">object</span>[] baseItems = derivedItems; <span class="comment">// この代入は(キャストの有無によらず)認められない</span>
</code></pre>

謎の最適化 (3) が速くなる理由はここにあります。
`Wrap<T>`構造体を介することで、共変性がなくなっています。
そして、共変じゃないことがわかっているので、型チェックが挟まらなくなる。
結果的に速い。
ただし、本来変換できないはずの `object[]` から `Wrap<string>[]` への嘘ダウンキャストが必要になるので、`Unsafe` クラスが必須です。

(ちなみに、`Wrap<T>`構造体は`T`型のフィールド1つだけの構造体なので、
`object`と`Wrap<object>`のメモリ上での構造は全く同じになります。
なので、今回のような嘘ダウンキャストしても「メモリ上の配置が同じだから同じコードで動く」みたいな感じになります。)

割かしひどい話です。
`Unsafe` クラスを避けれるなら避けたいわけでして。
最初から「共変性は使わないから型チェックしないでくれ」と指示できるような、
配列に代わる手段が求められます。
ということで出ている提案が以下のようなもの。

- [Invariant Arrays](https://github.com/dotnet/corefx/issues/23689)

まあ、採用される気配ないんですけども…
普通に coreclr 内にさっきの「謎の最適化」と同種のコード入ってたりするんですけども。
