---
title: "【C# 11 候補】 引数の null チェック"
source_url: "https://ufcpp.net/blog/2022/2/parameter-null-check/"
content_type: "BlogEntry"
published_at: "2022-02-13T13:03:28"
updated_at: "2022-02-13T13:03:28"
tags: []
umbraco_id: 2416
parent_id: 2411
sort_order: 4
aliases: []
---

# 【C# 11 候補】 引数の null チェック

先日出た Visual Studio 17.1 Preview 3 で、引数 null チェックの簡素化構文が入りました。

<pre class="source" title="引数 null チェックの !!">
<code><span class="method">m</span>(<span class="reserved">null</span>); <span class="comment">// ArgumentNull 例外が出る。</span>

<span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> <span class="variable">x</span><em>!!</em>) { }
</code></pre>

## 展開結果

上記の `void m(string x!!)` は以下のように展開されます。
(クラス名は実際には通常の C# では書けない変な名前で生成されます。)

<pre class="source" title="!! の展開結果">
<code><span class="reserved">void</span> <span class="method">m</span>(<span class="reserved">string</span> <span class="variable">x</span>)
{
    <span class="type">Internal</span>.<span class="method">ThrowIfNull</span>(<span class="variable">x</span>, <span class="string">&quot;x&quot;</span>);
}

<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">Internal</span>
{
    <span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Throw</span>(<span class="reserved">string</span> <span class="variable">paramName</span>)
    {
        <span class="control">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="variable">paramName</span>);
    }

    <span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">ThrowIfNull</span>(<span class="reserved">object</span> <span class="variable">argument</span>, <span class="reserved">string</span> <span class="variable">paramName</span>)
    {
        <span class="control">if</span> (<span class="variable">argument</span> == <span class="reserved">null</span>)
        {
            <span class="method">Throw</span>(<span class="variable">paramName</span>);
        }
    }
}
</code></pre>

もしかしたら、C# 11.0 リリースまでには、internal なコンパイラー生成メソッドではなくて、
標準ライブラリ中の [`ArgumentNullException.ThrowIfNull`](https://docs.microsoft.com/ja-jp/dotnet/api/system.argumentnullexception.throwifnull) メソッドに置き変わったりするかもしれませんが、まあ、やってることは一緒です。

ちなみに、`ThrowIfNull` メソッドから `Throw` メソッドだけがさらに抽出されているのはその方がパフォーマンスがいいからです。
`throw` ステートメントがあるとインライン展開を阻害したりするので。

## NRT と相補的な機能

[初期提案](https://github.com/dotnet/csharplang/issues/2145)が出たのは2019年の1月頃で、
C# 8.0 (2019年9月リリース)の [null 許容参照型](../../../../study/csharp/cheatsheet/ap_ver8.md#nullable-reference-type) (略して NRT)と同時期に検討されていたものです。

下手に NRT と同時期だったので紛らわしいのは紛らわしいんですが、

| NRT | 引数!! |
| --- | --- |
| コンパイル時のチェック(警告) | 実行時のチェック(例外) |
| コンパイル結果には全く影響を及ぼさない | コンパイル結果が変わる |
| メタデータに残る(外から見える・区別がある) | 残らない(あくまで内部実装の簡単化) |

という感じで、相補的な機能です。

## 文法案

文法的にはちょっと悩ましいんですが… 例えば、

* 使う記号は何がいいか
  * 初期案では `!` だった
  * 他に `??`、`?? throw`、`?!` みたいな話は出てはいる
  * 結局現在は `!!`
* 場所
  * 内部実装にしか影響ないのにメソッド宣言部分にあっていいのか
  * `void m(string x) { x!! }` みたいにメソッドの中になくていいのか
  * NRT に合わせるなら `string?` に倣って `string! x` とかにしなくていいのか 
* 変に記号にせず、汎用的な contract として、`requires x is not null` とか書けないか
* `<Nullable>throw</Nullable>` みたいなオプションで、問答無用で全ての引数に実行時 null チェックを挟めないか
* `[DisallowNull]` みたいな属性ではどうか

とかさんざん言われています。

これに対して、

* `!` は [null 抑止](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving)と同じ記号なのがまずい
  * null 抑止は「何もせず警告を無視」、`!!` は「実行時に例外」で大きな差
* `??` は、[null 合体](../../../../study/csharp/resource/rm_nullusage.md#null-coalesce)との弁別が構文解析する上で大変
* `?!` も条件演算子 + 単項前置き `!`
* `?? throw` は単純に長い
* `string!` は NRT との混同しててちょっとずれてる。NRT はあくまでコンパイル時チェックだけしたい
* `void m(string x) { x!! }` は `x` を2度書くのが結局つらい
* 汎用 contract (`requres`) は、NRT 以前から何度か案が出て、プロトタイプ実装もされては何度も没ってる
* 全ての引数に実行時 null チェックを挟むのはパフォーマンス的に論外。コンパイル オプションで `throw` の有無が変わるのもあまり好ましくない
* 属性が実行時の挙動に大きな影響を及ぼすのはあんまり本望ではない

等々ありまして、結局は `void m(string x!!)` 案で実装されました。

## 再燃

で、`!!` の実装がリリースされたことで、
先日、dotnet/runtime 内のコードにそれを適用する pull request が出たわけですが。

* [Initial roll out of !!](https://github.com/dotnet/runtime/pull/64720)

約1200ファイル、2万行の差分。

そしてこれが Twitter 上で話題になり、今更ながら反発の声多数。

元々色々もめ気味の機能ですからねぇ。
まして、急に Twitter 上で話題になったことで、
普段 csharplang にいない人が急にわらわらと出てくる事態になり。
2019年にさんざんやった話を蒸し返し中…

* [Question: How is !! envisaged to be used?](https://github.com/dotnet/csharplang/discussions/5735)

ほんと、普段こんなに人いないのに…
みんなぬるぽが好きすぎ…
