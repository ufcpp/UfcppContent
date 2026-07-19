---
title: "小ネタ null関係の演算子"
source_url: "https://ufcpp.net/blog/2016/12/tipsnulloperation/"
content_type: "BlogEntry"
published_at: "2016-12-13T00:37:03"
updated_at: "2016-12-27T14:33:44"
tags: []
umbraco_id: 1994
parent_id: 1969
sort_order: 12
aliases: []
---

# 小ネタ null関係の演算子

今日は、`?.`とか`??`での、nullの判定方法について。

C# 6で導入されたnull条件演算子(`?.`)ですが、以下の2つの式が**ほぼ**同じ意味になります。

<pre class="source" title="">
<code>x != <span class="reserved">null</span> ? x.M() : <span class="reserved">null
</code></pre>

<pre class="source" title="">
<code>x ?.M()
</code></pre>

「ほぼ」であって「完全に同じ」と言えないのは、`==`演算子を呼ぶか呼ばないかが変わってしまうせいです。
前者(自分で`==`を呼んでいるやつ)はオーバーロードされた`==`を呼び出しますが、
後者(`?.`を利用)は呼びません(直接nullかどうか調べます)。

例えば、以下のように、本当はnullじゃないのにnullを自称する(`x == null`がtrueになる)クラスを作ると、ちょっと変な挙動になります。

<pre class="source" title="null でないのに == nullなクラスを作る">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;

<span class="reserved">class</span> <span class="type">NonDefault</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> NonDefault(<span class="type">T</span> value) { Value = value; }

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> ToString() =&gt; Value.ToString();

    <span class="comment">// Value が既定値のときに null と同値扱いする</span>
    <span class="comment">// null でないものとの x == null が true になることがある</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> ==(<span class="type">NonDefault</span>&lt;<span class="type">T</span>&gt; x, <span class="type">NonDefault</span>&lt;<span class="type">T</span>&gt; y) =&gt;
        ReferenceEquals(x, <span class="reserved">null</span>) ? ReferenceEquals(y, <span class="reserved">null</span>) || Equals(y.Value, <span class="reserved">default</span>(<span class="type">T</span>)) :
        ReferenceEquals(y, <span class="reserved">null</span>) ? ReferenceEquals(x, <span class="reserved">null</span>) || Equals(x.Value, <span class="reserved">default</span>(<span class="type">T</span>)) :
        Equals(x.Value, y.Value);

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> !=(<span class="type">NonDefault</span>&lt;<span class="type">T</span>&gt; x, <span class="type">NonDefault</span>&lt;<span class="type">T</span>&gt; y) =&gt; !(x == y);
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// null の時には "null" と表示する ToString</span>
    <span class="reserved">static</span> <span class="reserved">string</span> A(<span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt; x) =&gt; (x != <span class="reserved">null</span> ? x.ToString() : <span class="reserved">null</span>) ?? <span class="string">"null"</span>;
    <span class="comment">// A とほぼ同じ意味に見えて…</span>
    <span class="reserved">static</span> <span class="reserved">string</span> B(<span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt; x) =&gt; x?.ToString() ?? <span class="string">"null"</span>;

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        WriteLine(A(<span class="reserved">new</span> <span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt;(1))); <span class="comment">// 1</span>
        WriteLine(B(<span class="reserved">new</span> <span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt;(1))); <span class="comment">// 1</span>

        WriteLine(A(<span class="reserved">null</span>));                   <span class="comment">// null</span>
        WriteLine(B(<span class="reserved">null</span>));                   <span class="comment">// null</span>

        <span class="comment">// == を呼ぶ呼ばないことによる差がここで出る</span>
        WriteLine(A(<span class="reserved">new</span> <span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt;(0))); <span class="comment">// null</span>
        WriteLine(B(<span class="reserved">new</span> <span class="type">NonDefault</span>&lt;<span class="reserved">int</span>&gt;(0))); <span class="comment">// 0</span>
    }
}
</code></pre>

まあ、普通、こんな`==`演算子オーバーロードの仕方はしないんですが。
というか、参照型に対する`==`オーバーロード自体めったにしないんですが。
(通常、`==`演算子を使うのは、`Dictionary`のキーにしたい不変なクラスくらいです。)

ちなみに、このメソッド`A`、`B`のコンパイル結果はそれぞれ以下のようになります。
比較のために表にして命令ごとに並べてみましょう。

A | B
---- | ----
`ldarg.0`                            | `ldarg.0`
`ldnull`                             | `brtrue.s IL_0006`
`call     NonDefault::op_Inequality` |
`brtrue.s IL_000c`                   |
`ldnull`                             | `ldnull`
`br.s     IL_0012`                   | `br.s     IL_000c`
`ldarg.0`                            | `ldarg.0`
`callvirt Object::ToString`          | `callvirt Object::ToString`
`dup`                                | `dup`
`brtrue.s IL_001b`                   | `brtrue.s IL_0015`
`pop`                                | `pop`
`ldstr    "null"`                    | `ldstr    "null"`
`ret`                                | `ret`

nullの判定方法(2行目～4行目)だけが違って、残りは全く同じです。
`==`演算子を呼ばずに直接nullを調べるなら`brtrue`命令1個でできます。

ちなみに、`brtrue`は"branch if true"の略で、
「直前の結果がtrueだったらジャンプする」という命令になります。
整数の0とか、参照型のnullとかはfalse扱い。

この挙動は[null合体演算子](../../../../study/csharp/resource/sp2_nullable.md#coalescing)(`??`)でも同様です。

## おまけ: `throw null`

話題は変わりますが、`?.`の中身をILレベルで覗いたついでと言ってはなんですが、ちょっとしたおまけ。

時々、「`throw null`と書くと、`throw new NullReferenceException()`と同じ意味になる」的な誤解(？)を見かけたりします。
コンパイル結果的には当然、全然違うんですよね。

以下のように書いた場合、

<pre class="source" title="throw null">
<code><span class="reserved">static</span> <span class="reserved">void</span> X() { <span class="reserved">throw</span> <span class="reserved">null</span>; }
<span class="reserved">static</span> <span class="reserved">void</span> Y() { <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">NullReferenceException</span>(); }
</code></pre>

コンパイル結果は以下の通り。

<pre class="source" title="throw null">
<code>.method <span class="reserved">private</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> <span class="reserved">void</span>  X() <span class="reserved">cil</span> <span class="reserved">managed</span>
{
  <span class="comment">// コード サイズ       2 (0x2)
</span>  .maxstack  8
  IL_0000:  ldnull
  IL_0001:  throw
} <span class="comment">// end of method Program::X
</span>
.method <span class="reserved">private</span> <span class="reserved">hidebysig</span> <span class="reserved">static</span> <span class="reserved">void</span>  Y() <span class="reserved">cil</span> <span class="reserved">managed</span>
{
  <span class="comment">// コード サイズ       6 (0x6)
</span>  .maxstack  8
  IL_0000:  newobj     <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.NullReferenceException::<span class="reserved">.ctor</span>()
  IL_0005:  throw
} <span class="comment">// end of method Program::Y
</span>
</code></pre>

割かしそのまんまなILコードです。
nullをロード(`ldnull`)して、`throw`命令を実行。
要するに、前者は(`throw`命令の実行に失敗して)実行エンジンが`NullReferenceException`を作って投げていて、
後者は自分自身で作った`NullReferenceException`を投げている。
まあ、結果的には同じような挙動をするんですが。
