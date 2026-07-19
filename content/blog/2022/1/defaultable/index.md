---
title: "【C# 11 候補】defaultable value type"
source_url: "https://ufcpp.net/blog/2022/1/defaultable/"
content_type: "BlogEntry"
published_at: "2022-01-07T21:48:20"
updated_at: "2022-01-07T21:48:20"
tags: []
umbraco_id: 2403
parent_id: 2401
sort_order: 1
aliases: []
---

# 【C# 11 候補】defaultable value type

[`ImmutableArray` に対してコレクション初期化子は使えないという話](../../../2021/12/immutable-array-init/index.md)でちょっと出しましたが、この問題の原因の1つは「[既定値](../../../../study/csharp/resource/rm_default.md)(`default`、0初期化)のまま放置してはいけない型がある」というものです。

`default` 放置問題は「null を null のまま放置してはいけない」という問題に直結するので、
[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)とも関連します。

ということで「[クラスの `null` 解析と同様に、構造体の `default` に関するフロー解析を行う](https://github.com/dotnet/csharplang/discussions/5337)」という提案が前々からあるんですが。
状況としては「提案のドラフトは書いてみたけど、まだ思い悩んでる点があって、Design Meeting に議題を上げる段階にない」みたいな感じです。

## default 放置問題

C# 8.0 で[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)(nullable reference type、通称 NRT)が入って、以下のように、null 参照例外が出そうな箇所にはコンパイル時に警告を出してくれるようになりました。

<pre class="source" title="C# 8.0 の NRT">
<code><span class="preprocess">#</span><span class="preprocess">nullable</span> <span class="preprocess">enable</span>

<span class="comment">// 警告: ? が付いてない変数に null を渡してる。</span>
<span class="reserved">string</span> <span class="variable">s</span> = <span class="reserved"><span class="warning">null</span></span>;

<span class="comment">// この行でも警告: s に null が入ってることを認識してる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable"><span class="warning">s</span></span>.Length);

<span class="comment">// OK</span>
<span class="reserved">string</span>? <span class="variable">n</span> = <span class="reserved">null</span>;

<span class="comment">// 警告: null かもしれないもののメンバー参照してる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable"><span class="warning">n</span></span>.Length);

<span class="comment">// これなら OK: not null 判定してるのでメンバー参照してももう大丈夫。</span>
<span class="control">if</span> (<span class="variable">n</span> <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">null</span>) <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">n</span>.Length);
</code></pre>

この解析は「できる範囲で、できることからやる」みたいな感じなので結構判定漏れもあるんですが。
その判定漏れの中で特に深刻なのが、構造体の `default` を挟んだ場合。

例えば以下のようなコードで、簡単に判定から漏れた null を残せます。

<pre class="source" title="default を介して null が紛れ込む例">
<code><span class="preprocess">#</span><span class="preprocess">nullable</span> <span class="preprocess">enable</span>

<span class="comment">// これは警告にしてもらえる: 非 null な S に null を渡した。</span>
<span class="type">A</span> <span class="variable">a1</span> = <span class="reserved">new</span>(<span class="reserved"><span class="warning"null</span></span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a1</span>.S.Length); <span class="comment">// OK</span>

<span class="comment">// これだと警告が出ない: default に対する解析がまだない(提案段階)。</span>
<span class="type">A</span> <span class="variable">a</span> = <span class="reserved">default</span>;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.S.Length); <span class="comment">// OK じゃないんだけど OK になる</span>

<span class="comment">// S は非 null なはず。</span>
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type">A</span>(<span class="reserved">string</span> <span class="variable">S</span>);
</code></pre>

この問題を一番深刻に踏み抜いてるのが、
最近のブログで何度か出て来た [`ImmutableArray`](../../../2021/12/immutable-array-init/index.md) なわけです。

<pre class="source" title="ImmutableArray の default によるぬるぽ">
<code><span class="preprocess">#</span><span class="preprocess">nullable</span> <span class="preprocess">enable</span>
<span class="reserved">using</span> System.Collections.Immutable;

<span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span> <span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt;();

<span class="comment">// コードのぱっと見の印象からすると 0 とか返ってきて欲しい。</span>
<span class="comment">// 実際にはぬるぽ発生。</span>
<span class="comment">// ぬるぽるんだったら、NRT 警告みたいなの出してほしい(これが課題)。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.Length);
</code></pre>

## defaultable value type

この問題に対する解決策、方向性としてはシンプルで、
「参照型に対して null を認めないようにフロー解析する」というのと同じノリで、「値型に対して `default` を認めないようにフロー解析する」というやり方で解決できるはずです。
それが今回説明する[defaultable value type](https://github.com/dotnet/csharplang/discussions/5337) (default 許容値型)。
nullable reference type (null 許容参照型)との対比でこんな名前になっています。

要は、

* `ImmutableArray` みたいな型に対して `default` を渡しているところには警告を出す
* あえて `default` を渡したい箇所には、NRT の `T?` に類する何か(仮に `T~` とか書く)みたいなアノテーションを付ける
  * これが defaultable value type

というもの。

### nullable と defaultable

ただ、まあ、ちょっとややこしいのが nullable と defaultable があるという点。
C# 2.0 の頃から null 許容値型があるので、
null → default → 有効な値 みたいな「2段の無効な値」ができてしまうという問題があります。

<pre class="source" title="2段の無効な値">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="comment">// null</span>
<span class="comment">// Nullable&lt;T&gt;.HasValue で null 判定。</span>
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt;? <span class="variable">a1</span> = <span class="reserved">null</span>;
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt;? <span class="variable">a2</span> = <span class="reserved">default</span>; <span class="comment">// これは null になる</span>

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a1</span>.HasValue); <span class="comment">// false</span>

<span class="comment">// default</span>
<span class="comment">// HasValue は true。</span>
<span class="comment">// ImmutableArray.IsDefault みたいな別判定が必要。</span>
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt;? <span class="variable">a3</span> = <span class="reserved">new</span>();
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a4</span> = <span class="reserved">new</span>();
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a5</span> = <span class="reserved">default</span>; <span class="comment">// これは new() になる</span>

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a3</span>.HasValue); <span class="comment">// true</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a4</span>.IsDefault); <span class="comment">// true</span>

<span class="comment">// 有効な値</span>
<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a6</span> = <span class="type">ImmutableArray</span>.<span class="method">Create</span>&lt;<span class="reserved">int</span>&gt;();

<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a6</span>.IsDefault); <span class="comment">// false</span>
</code></pre>

これがあるので、defaltable value type に対して `T?` という記法は使えません。
なので提案では<em>仮に</em> `T~` としています。
当初は `T??` みたいな案も出ていたんですが、
[null 合体演算の `??`](../../../../study/csharp/resource/sp2_nullable.md#coalescing)との弁別が(構文解析が重たくなるという意味で)難しいとのこと。

この仮の `~` を使って話を進めると、とりあえず書きたいコードは以下のようなものになります。

<pre class="source" title="defaultable value type の例 (~ 案)">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="method">m1</span>(<span class="reserved"><span class="warning">default</span></span>); <span class="comment">// 警告</span>
<span class="method">m1</span>(<span class="type">ImmutableArray</span>.<span class="method">Create</span>&lt;<span class="reserved">int</span>&gt;()); <span class="comment">// OK</span>
m2(<span class="reserved">default</span>); <span class="comment">// OK</span>

<span class="reserved">void</span> <span class="method">m1</span>(<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span>)
{
    <span class="comment">// a に default が入ることはなく、a.Length が有効。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.Length);
}

<span class="reserved">void</span> <span class="method">m2</span>(<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt;<em>~</em> a)
{
    <span class="comment">// a に default が入る可能性があり、a.Length のところに警告を出したい。</span>
    <span class="type">Console</span>.WriteLine(<span class="warning">a</span>.Length);

    <span class="comment">// 非 default を保証するような仕組みも欲しい。</span>
    <span class="control">if</span> (!a.IsDefault)
    {
        <span class="type">Console</span>.WriteLine(a.Length); <span class="comment">// これは OK にしたい。</span>
    }
}
</code></pre>

### 参照型フィールドで自動判定

この defaultable value types の最大の目的は `ImmutableArray` みたいな、内部に参照型フィールドを持っている場合の null 解析です。

なので、

* 非 null 参照型フィールドを1つでも持っていると「`default` のまま放置してはいけない型」判定になる
* 非 null 参照型フィールドをすべて非 null 初期化した時点で「`default` 状態から脱した」判定になる

という判定を自動的にする予定です。

<pre class="source" title="非 null 参照型フィールドで自動判定">
<code><span class="type">A</span> <span class="variable">a</span> = <span class="reserved">default</span>;

<span class="comment">// 警告: default のまま使った。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable"><span class="warning">a</span></span>.S);

<span class="comment">// OK: S が非 null になった時点で a は非 default。</span>
<span class="variable">a</span>.S = <span class="string">&quot;&quot;</span>;
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span>.S);

<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type">A</span>(<span class="reserved">string</span> <span class="variable">S</span>);
</code></pre>

### opt-in

上記の通り、非 null 参照型フィールドを持っている値型は自動的に `default` 解析の対象になるわけですが、
それ以外の構造体でも「`default` 放置するとまずい」というものはあります。

例として挙がってるのは "ハンドル" の類ですが、
要は、ポインターに類する値を `int` とか `IntPtr` で持っているような構造体。
昔からの習慣で、null と同じく「0 なら無効なハンドル値」とすることが多いです。
こういう型は null 許容参照型とほぼ同じが扱いが必要。

こういう型に対して何らかの新構文を追加すべきか、
それとも属性か何かでアノテーションを付けるかはまだ検討の余地がありますが、
仮に属性を使う案でいうと以下のような感じになります。

<pre class="source" title="属性を使って defaultable value type opt-in">
<code>[<span class="type">MaybeDefault</span>] <span class="comment">// 「default 放置はダメ」を表す何らかの属性</span>
<span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">BlobHandle</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">nuint</span> _value;

    [<span class="type">AllowDefault</span>] <span class="comment">// 「このプロパティが true なら非 default」を表す何らかの属性</span>
    <span class="reserved">public</span> <span class="reserved">bool</span> IsNil =&gt; _value != 0;

    <span class="reserved">public</span> <span class="reserved">byte</span> <span class="method">Read</span>() =&gt; <span class="comment">// ...</span>
}

<span class="reserved">void</span> <span class="method">M1</span>(<span class="type">BlobHandle</span>~ handle)
{
    <span class="control">if</span> (!handle.IsNil)
    {
        handle.Read(); <span class="comment">// ok</span>
    }
}
M1(<span class="reserved">default</span>); <span class="comment">// ok</span>

<span class="reserved">void</span> <span class="method">M2</span>(<span class="type">BlobHandle</span> <span class="variable">handle</span>)
{
    <span class="variable">handle</span>.<span class="method">Read</span>();
}
<span class="method">M2</span>(<span class="reserved"><span class="warning">default</span></span>); <span class="comment">// warning</span>
</code></pre>

ちなみに、属性はこれ専用のものを用意すべきか、
それとも null 許容参照型で使っている `MaybeNull` などの属性をそのまま流用すべきかみたいな点も検討途中です。

### default 演算子

前述の `IsDefault` (`ImmutableArray` が今持ってるやつ)とか `IsNil` (前節の例に挙げた `BlobHandle` のやつ)とかじゃなくて、
`default` 判定専用の演算子定義も必要なんじゃないかという話もあります。

というのも、以下のようなコード(また `ImmutableArray` が起こす問題)を考えます。

<pre class="source" title="ImmutableArray に対してパターン マッチングでぬるぽる">
<code><span class="reserved">using</span> System.Collections.Immutable;

<span class="reserved">void</span> <span class="method">m</span>(<span class="type">ImmutableArray</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">a</span>)
{
    <span class="comment">// ImmutableArray に対してリスト パターンを使う。</span>
    <span class="comment">// パターンマッチングは暗黙的に非 null 判定を含んでいて、たいていの型に対してはぬるぽを起こさない。</span>
    <span class="comment">// ところが…</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">a</span> <span class="reserved">is</span> [1, ..]);
}

<span class="comment">// こういうのは大丈夫。</span>
<span class="method">m</span>(<span class="type">ImmutableArray</span>.<span class="method">Create</span>(1)); <span class="comment">// true</span>
<span class="method">m</span>(<span class="type">ImmutableArray</span>.<span class="method">Create</span>(2)); <span class="comment">// false</span>

<span class="comment">// これが例外を起こす。</span>
<span class="comment">// null チェックに代わる「default チェック」が必要…</span>
<span class="method">m</span>(<span class="reserved">default</span>);
</code></pre>

こんな感じで「`default` を放置しちゃダメ」な型に対するパターン マッチングをするにあたって、「null チェック代わりに何か `default` チェックを挟みたい」という要件があります。

で、「何か特定のプロパティを呼ぶ」とかよりは、以下のように、`operator default` みたいなものを書けるようにした方がいいのではないかという案も出ています。

<pre class="source" title="operator default">
<code><span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">ImmutableArray</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> <span class="reserved">operator</span> <span class="reserved">default</span>(<span class="type">ImmutableArray</span>&lt;<span class="type">T</span>&gt; <span class="variable">arr</span>) =&gt; <span class="variable">arr</span>._array <span class="reserved">is</span> <span class="reserved">null</span>;
}
</code></pre>

## 課題

NRT で問題を起こしている以上、defaultable value type みたいなフロー解析が必要なこと自体はもう分かっているわけですが。
話が進まないのはまだ悩ましい点が残っているから。

特に悩ましいとされるのが2点あって、以下のようなものです。

* プロパティはどうするか
* NRT 並みに「既存コードの移行作業」に手間が掛かる

### プロパティ

[`record struct`](../../../../study/csharp/cheatsheet/ap_ver10.md#record-struct)では、メンバーは(フィールドではなく)プロパティで作られます。
例えば、`record struct A(string S);` と書くと、`S` はプロパティです。

この場合、「すべての非 null 参照型フィールドを初期化していれば非 `default`」の判定をどうするかという問題があります。
プロパティ `S` 越しにそのバッキングフィールドを初期化することになるわけですが、プロパティとフィールドの紐づけができないとフロー解析できません。

### 既存コードの移行

null 許容参照型を導入するときもかなり苦労しました。
.NET の標準ライブラリに null アノテーションを付けて回る作業には2年くらい掛かっています。

しかも、既存コードを壊さないように、「null 解析をするかどうか」は [opt-in](../../../../study/csharp/resource/nullablereferencetype.md#opt-in) (明示的にオプション指定しない限り有効化されない)になっていて、「オプションの有無で2種類の C# がある」といってもいいような状況になっています。
(C# チームもこれを好ましいとは思っていないので、
null 許容参照型はそれだけ「無理してでも必要」とされる唯一の機能です。)

defaultable value type ではこの「アノテーション追加」と「opt-in」をもう1度やる必要があります。
