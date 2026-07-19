---
title: "IList とかを IReadOnlyList とかから派生させたい"
source_url: "https://ufcpp.net/blog/2024/3/ilist-inherites-from-ireadonlylist/"
content_type: "BlogEntry"
published_at: "2024-03-03T00:18:50"
updated_at: "2024-03-03T00:18:50"
tags: []
umbraco_id: 2492
parent_id: 2490
sort_order: 1
aliases: []
---

# IList とかを IReadOnlyList とかから派生させたい

.NET が長らく抱えている「なぜ `IList<T>` は `IReadOnlyList<T>` ではないのか」問題、 .NET 9 で解消するかもしれないみたい。

ちなみに、問題を抱えるに至った原因は `IReadOnlyList<T>` が後付けということです。
1から作り直すのであれば、誰がどう考えても `IList<T>` は `IReadOnlyList<T>` から派生させるのが自然です。
それがかえって、`IReadOnlyList<T>` 導入以降に .NET 利用を始めた人に混乱を招いているというのが現状になります。

## 当初設計: インターフェイスは増やしすぎない

インターフェイスを増やすというのは、
型情報で DLL サイズが増えるとか、
実行時にインターフェイスを検索するコストが増えるとか、
多少なりともコストを生じます。

一方で、.NET Framework の最初のβ版が出たのは2000年ごろ、正式版で2002年なわけですが、
この頃は read-only であることの重要性が過小評価されていたと思います。
なので、重要でない(と当時は思われていた)ものにコストは掛けたくないという話に。

(この辺りのことは「<a target="_blank" href="https://www.amazon.co.jp/dp/4296080040?&_encoding=UTF8&tag=cunflc-22&linkCode=ur2&linkId=c74d95cede34616d4c468ff921d42544&camp=247&creative=1211">.NETのクラスライブラリ設計</a>」で触れられていたりします。
ちなみにこの本、要は「.NET の初期設計に関する懺悔本」です。)

そこで当時の設計としては「read-only / writeable なインターフェイスを1個用意して、`IsReadOnly` プロパティで書き込み出来るかどうかを調べる」という作りでした。

<pre class="source" title="read-only かどうかはプロパティで調べる">
<span class="reserved">namespace</span> System<span class="operator">.</span>Collections;

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IList</span> : <span class="type">ICollection</span>, <span class="type">IEnumerable</span>
{
    <span class="reserved">object</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">bool</span> <span class="property">IsReadOnly</span> { <span class="reserved">get</span>; } <span class="comment">// ← これ</span>
    <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">object</span> <span class="variable local">value</span>);
    <span class="comment">// 以下略</span>
}
</pre>

.NET Framework 2.0 (2005年)に[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)が導入されてもまだこの思想は引き継がれます。
まあ、旧来インターフェイスとジェネリック インターフェイスで思想が違うのも混乱しそうですし。

<pre class="source" title="ジェネリック ICollection でも IsReadOnly プロパティ">
<span class="reserved">namespace</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic;

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">ICollection</span>&lt;<span class="type param">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;, <span class="type">IEnumerable</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
    <span class="reserved">bool</span> <span class="property">IsReadOnly</span> { <span class="reserved">get</span>; } <span class="comment">// ← これ</span>
    <span class="reserved">void</span> <span class="method">Add</span>(<span class="type param">T</span> <span class="variable local">value</span>);
    <span class="comment">// 以下略</span>
}
</pre>

問題になり始めたのは C# 4.0 (2010年)で共変性を得てからでして。
読み書き両方できてしまう `IList<T>` や `ICollection<T>` では、以下のような共変な代入ができません。

<pre class="source" title="書き込みがあると共変にできない">
<span class="type">IList</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">str</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">string</span>&gt;();
<span class="type">IList</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">obj</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS0266">str</span></span>; <span class="comment">// ダメ。</span>

<span class="comment">// そりゃ、こういうコード書かれたらまずいので当然。</span>
<span class="variable">obj</span><span class="operator">.</span><span class="method">Add</span>(<span class="number">1</span>);
</pre>

そこで .NET Framework 4.5 (2012年)では read-only 系のインターフェイスが導入されます。

<pre class="source" title="read-only 系インターフェイスは共変">
<span class="type">IReadOnlyList</span>&lt;<span class="reserved">string</span>&gt; <span class="variable">str</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">string</span>&gt; { <span class="string">&quot;abc&quot;</span> };
<span class="type">IReadOnlyList</span>&lt;<span class="reserved">object</span>&gt; <span class="variable">obj</span> <span class="operator">=</span> <span class="variable">str</span>; <span class="comment">// read-only なら共変。</span>

<span class="comment">// obj.Add(1); とか書かれる心配がない。</span>
<span class="comment">// 読むだけなら安全。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">obj</span>[<span class="number">0</span>]);
</pre>

## インターフェイスへの親インターフェイスの追加・メンバー移動は破壊的変更

2012年に追加された read-only 系インターフェイスですが、元々あったインターフェイスとは独立しています。
残念ながら「`IList<T>` は `IReadOnlyList<T>` ではない」という状態。

<pre class="source" title="残念ながら完全に別インターフェイス">
<span class="reserved">namespace</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic;

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IReadOnlyCollection</span>&lt;<span class="reserved">out</span> <span class="type param">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">ICollection</span>&lt;<span class="type param">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// IReadOnlyCollection とは独立に Count を持つ。</span>
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
    <span class="comment">// 以下略</span>
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IReadOnlyList</span>&lt;<span class="reserved">out</span> <span class="type param">T</span>&gt; : <span class="type">IReadOnlyCollection</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="type param">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] { <span class="reserved">get</span>; }
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IList</span>&lt;<span class="type param">T</span>&gt; : <span class="type">ICollection</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// IReadOnlyList とは独立に this[int] を持つ。</span>
    <span class="type param">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="comment">// 以下略</span>
}
</pre>

普通に考えて、1から作るのであれば以下のようにします。

<pre class="source" title="1からやり直せるならどう考えても ICollection : IReadOnlyCollection">
<span class="reserved">namespace</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic;

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IReadOnlyCollection</span>&lt;<span class="reserved">out</span> <span class="type param">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">ICollection</span>&lt;<span class="type param">T</span>&gt; : <span class="type">IReadOnlyCollection</span>&lt;<span class="type param">T</span>&gt; 
{
    <span class="comment">// 以下略</span>
}
</pre>

ところが、後付けでこういうことをするのは破壊的変更になります。

例えば以下のようなコードがあったとします。

<pre class="source" title="バージョン1">
<span class="comment">// バージョン1</span>

<span class="comment">// corelib.dll</span>
<span class="reserved">interface</span> <span class="type">ICollection</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="comment">// corelib とは別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// mylib.dll</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">ICollection</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
}
</pre>

ここに `IReadOnlyCollection` を「理想的な状態」で導入したくて `Count` を移動させると mylib を壊します。

<pre class="source" title="Count を IReadOnlyCollection">
<span class="comment">// バージョン2</span>

<span class="comment">// corelib.dll</span>
<span class="reserved">interface</span> <span class="type">IReadOnlyCollection</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="reserved">interface</span> <span class="type">ICollection</span> : <span class="type">IReadOnlyCollection</span>
{
    <span class="comment">// Count は IReadOnlyCollection に移した。</span>
}

<span class="comment">// corelib とは別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// mylib.dll</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">ICollection</span>
{
    <span class="comment">// 再コンパイルするなら平気。</span>
    <span class="comment">// ただ、古い dll のまま使うと「IReadOnlyCollection.Count がない」と怒られる。</span>
    <span class="comment">// 再コンパイルするまでは C が持ってるのは ICollection.Count。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
}
</pre>

ということでインターフェイスを独立。
これなら「再コンパイルするまでは `C` は `IReadOnlyCollection` にはならない」というだけなので、
DLL のロードに失敗したりはしません。

<pre class="source" title="機能がダブるけどもこれで妥協">
<span class="comment">// corelib.dll</span>
<span class="reserved">interface</span> <span class="type">IReadOnlyCollection</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="reserved">interface</span> <span class="type">ICollection</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; } <span class="comment">// IReadOnlyCollection と機能がダブってるけど許して</span>
}

<span class="comment">// corelib とは別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// mylib.dll</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">ICollection</span>, <span class="type">IReadOnlyCollection</span> <span class="comment">// 2個とも実装</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
}
</pre>

これが .NET のコレクション系インターフェイスの現状になります。

## インターフェイス メソッドのデフォルト実装

.NET Core 3.0 (2019年)に[デフォルト実装](../../../../study/csharp/cheatsheet/ap_ver8.md#default-imeplementation-of-interface)というものが導入されて、インターフェイスへのメンバー追加での破壊的変更を避けれるようになりました。

この機能を使えば先ほどの「既存クラスが `IReadOnlyCollection.Count` を実装していない」問題は解消できます。
(親インターフェイスの追加は、「メンバー追加」の一種なのでデフォルト実装で対処できます。)

<pre class="source" title="デフォルト実装で解決">
<span class="comment">// corelib.dll</span>
<span class="reserved">interface</span> <span class="type">IReadOnlyCollection</span>
{
    <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; }
}

<span class="reserved">interface</span> <span class="type">ICollection</span> : <span class="type">IReadOnlyCollection</span>
{
    <span class="reserved">new</span> <span class="reserved">int</span> <span class="property">Count</span> { <span class="reserved">get</span>; } <span class="comment">// IReadOnlyCollection.Count とは別の Count にはなっちゃう。</span>

    <span class="comment">// IReadOnlyCollection のことを知らない既存クラスのために、</span>
    <span class="comment">// 既存クラスに代わって ICollection 内で IReadOnlyCollection.Count を実装。</span>
    <span class="reserved">int</span> <span class="type">IReadOnlyCollection</span><span class="operator">.</span><span class="property">Count</span> <span class="operator">=&gt;</span> <span class="property">Count</span>;
}

<span class="comment">// corelib とは別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// mylib.dll</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">ICollection</span>
{
    <span class="comment">// 再コンパイルするまではあくまで ICollection.Count。</span>
    <span class="comment">// それでも、ICollection 側で IReadOnlyCollection.Count を実装してくれているので平気。</span>
    <span class="comment">//</span>
    <span class="comment">// ちなみに、再コンパイルするとこの Count をもって</span>
    <span class="comment">// ICollection.Count と IReadOnlyCollection.Count の両方を実装。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
}
</pre>

ということで、インターフェイスのデフォルト実装の導入後、
ついに `ICollection<T>` が `IReadOnlyCollection<T>` 派生に、
`IList<T>` が `IReadOnlyList<T>` 派生にできるのではないかと多くの期待が寄せられています。
実際、2019年に提案あり:

* [Make mutable generic collection interfaces implement read-only collection interfaces
](https://github.com/dotnet/runtime/issues/31001)

ただ、厳密にはこれも破壊的変更を起こす可能性はあったりします。
というのも、デフォルト実装には「ダイアモンド継承」問題というものがあります。
以下のような感じで、「分かれ道からの合流がある継承」をやると問題を起こすことがあります。

<pre class="source" title="ダイアモンド継承問題">
<span class="reserved">interface</span> <span class="type">IA</span>
{
    <span class="reserved">int</span> <span class="method">M</span>();
}

<span class="reserved">interface</span> <span class="type">IB</span> : <span class="type">IA</span>
{
    <span class="reserved">int</span> <span class="type">IA</span><span class="operator">.</span><span class="method">M</span>() <span class="operator">=&gt;</span> <span class="number">1</span>; <span class="comment">// デフォルト実装持ち</span>
}

<span class="reserved">interface</span> <span class="type">IC</span> : <span class="type">IA</span>
{
    <span class="reserved">int</span> <span class="type">IA</span><span class="operator">.</span><span class="method">M</span>() <span class="operator">=&gt;</span> <span class="number">2</span>; <span class="comment">// デフォルト実装持ち</span>
}

<span class="comment">// IA.M の実装をデフォルト実装に頼るとして、</span>
<span class="comment">// IB の実装と IC の実装のどちらを使えばいいか不明瞭。</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type"><span class="error" title="CS8705">IB</span></span>, <span class="type">IC</span>
{
}
</pre>

まあ、前述の `ICollection` に「分かれ道」はないので誰しもがこの問題を踏むわけではないんですが。
1段自作のインターフェイスとかを挟んでいると問題を踏む可能性が出てきます。
例えば以下のような感じ。

<pre class="source" title="IReadOnlyCollection.Count でダイアモンド継承問題を踏む例">
<span class="comment">// corelib とは別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// anotherlib.dll</span>
<span class="reserved">interface</span> <span class="type">ICustomReadonlyList</span> : <span class="type">IReadOnlyCollection</span>
{
    <span class="comment">// 何らかのデフォルト実装持ち</span>
    <span class="reserved">int</span> <span class="type">IReadOnlyCollection</span><span class="operator">.</span><span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
}

<span class="comment">// corelib とも anotherlib とも別のプロジェクトで、別の開発者が保守</span>
<span class="comment">// mylib.dll</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type"><span class="error" title="CS8705">ICollection</span></span>, <span class="type">ICustomReadonlyList</span>
{
    <span class="comment">// ICollection 更新前: </span>
    <span class="comment">//   ICollection.Count は明示的に実装</span>
    <span class="comment">//   IReadOnlyCollection.Count は ICustomReadonlyList 側のデフォルト実装を使用</span>
    <span class="comment">//</span>
    <span class="comment">// ICollection 更新後: </span>
    <span class="comment">//   ICollection.Count は明示してるから平気</span>
    <span class="comment">//   IReadOnlyCollection.Count は ICustomReadonlyList と ICollection のどちらのデフォルト実装を使えばいいかわからない</span>
    <span class="comment">//   (ソースコードも修正しないと再コンパイルも失敗)</span>
    <span class="reserved">int</span> <span class="type">ICollection</span><span class="operator">.</span><span class="property">Count</span> <span class="operator">=&gt;</span> <span class="number">1</span>;
}
</pre>

この辺りの懸念もあって、しばらく塩漬けが続きます。

## ついに動きが

そして時は流れること4年、ついに動きが。
.NET 9 でこの作業をやろうという検討に入ったみたいです。

* [API レビューをやった報告コメント](https://github.com/dotnet/runtime/issues/31001#issuecomment-1811013088)
  * 実験を試みる準備が整った
* [.NET チームのプロダクト マネージャーのコメント](https://github.com/dotnet/runtime/issues/31001#issuecomment-1813159725)
  * 指摘されている破壊的変更はレアケースで、考えを変えるものではないと思う
  * どの程度の破壊的変更になるか(許容できる範囲かどうか)、.NET 9 の初期に実験してみるのは十分妥当
* [修正 Pull Request](https://github.com/dotnet/runtime/pull/95830)
