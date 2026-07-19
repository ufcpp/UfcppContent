---
title: "Extensions (拡張型)"
source_url: "https://ufcpp.net/blog/2024/3/extensions/"
content_type: "BlogEntry"
published_at: "2024-03-20T19:51:16"
updated_at: "2024-03-20T19:51:16"
tags: []
umbraco_id: 2495
parent_id: 2490
sort_order: 4
aliases: []
---

# Extensions (拡張型)

C# 3.0 から[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)が使えるわけですが、
もうちょっといろんな「拡張」をしたいという話が前々からあります。
例えば以下のような要求。

* 既存の型に静的メンバーも足したい
* プロパティや演算子も足したい
* インターフェイスの後付けもしたい

今では Extensions とか呼ばれていまして、以下の issue でトラッキング中。

* [Exploration: Shapes and Extensions #164](https://github.com/dotnet/csharplang/discussions/164)

ここからさかのぼって、かつては Extension everything とか呼ばれていたり、
個別に「インターフェイスを実装したい」「演算子を拡張したい」など個別の issue がありました。

* [Extension Everything](https://github.com/dotnet/roslyn/issues/11159)
  * [Extension classes with Interfaces](https://github.com/dotnet/roslyn/issues/3357)
  * [Extension operators](https://github.com/dotnet/roslyn/issues/4945)

2015年([Roslyn](https://github.com/dotnet/roslyn) が GitHub での公開に切り替わった年)にはすでにそんな話が出ています。

結構大きな機能なのでしり込みしていたみたいですが、
去年くらいから Working Group (この機能の追加を推進するメンバーを割り当てて、定期的にミーティング)を設けて作業を始めました。

* [Extensions がらみの議事録](https://github.com/dotnet/csharplang/tree/3cd77d5664281f6df4785a35d4b778c88ec3aa98/meetings/working-groups/roles)

うちのブログでも去年、1度取り上げています。

* [【C# 12 候補】 Extensions](../../../2023/3/extensions/index.md)

もう9年も経ってしまい、C# 12 でも入らなかったわけですが、
ついに今年、C# 13 には一部入りそう(インターフェイスの後付けだけは無理そう)な雰囲気になっています。

最近の話題のうちいくつかを取り上げると、以下のような話が出ています。

* [段階的に実装していく](https://github.com/dotnet/csharplang/blob/3cd77d5664281f6df4785a35d4b778c88ec3aa98/meetings/working-groups/roles/extensions-2024-01-25.md#scope-and-priorities-for-c13)
  * 静的メンバー → インスタンス メンバー → 継承のサポート (C# 13 でやれそうなのはここまで) → インターフェイスの後付け
* [普通の構造体でラッパー型を作って、利用時に Unsafe.As で変換してメンバーを呼ぶ](https://github.com/dotnet/csharplang/issues/7771)
  * 型消去な実装
* [インターフェイス実装は大変そう](https://github.com/dotnet/csharplang/blob/7a3990a2bec382871de3a0615746d274ae924b6b/proposals/extension-interfaces.md)
* [メンバーのルックアップ](https://github.com/dotnet/csharplang/blob/main/meetings/2024/LDM-2024-02-28.md)
  * クラスの継承時の挙動に準ずる
  * 旧拡張メソッドと新 Extensions は優先度をつけない(どちらにも同名メソッドがあった場合、コンパイル エラーにする)案が今のところ優勢

## extension 構文

ということで、改めて Extensions の話を。
今、以下のような構文を足そうとしています。

<pre class="source" title="extension 構文">
<span class="comment">// 拡張の構文例。</span>
<span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type">SomeExtension</span> <span class="reserved">for</span> <span class="type">SomeClass</span> : <span class="type">IEquatable</span>&lt;<span class="type">SomeExtension</span>&gt;
{
    <span class="comment">// 追加したいメンバーを書く。</span>

    <span class="comment">// 1. 静的メンバーも書ける。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="property"><span class="static">Y</span></span> <span class="operator">=&gt;</span> <span class="property"><span class="static">X</span></span> <span class="operator">*</span> <span class="property"><span class="static">X</span></span>;

    <span class="comment">// 2. メソッド以外も書ける。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="method">GetValue</span>();
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="method">SetValue</span>(<span class="reserved">value</span>);
    }

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>] <span class="operator">=&gt;</span> <span class="method">GetValue</span>(<span class="variable local">index</span>);

    <span class="comment">// 3. インターフェイスの実装を持てる。</span>
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type">SomeExtension</span><span class="operator">?</span> <span class="variable local">other</span>) <span class="operator">=&gt;</span> <span class="property">Property</span> <span class="operator">==</span> <span class="variable local">other</span><span class="operator">?</span><span class="operator">.</span><span class="property">Property</span>;
}

<span class="comment">// 拡張の対象の例。</span>
<span class="reserved">class</span> <span class="type">SomeClass</span>
{
    <span class="comment">// (中身は適当。)</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="field"><span class="static">X</span></span> <span class="operator">=</span> <span class="number">123</span>;

    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetValue</span>() <span class="operator">=&gt;</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetValue</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetValue</span>(<span class="reserved">int</span> <span class="variable local">index</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">*</span> <span class="variable local">index</span>;
}
</pre>

ちなみに、「インターフェイスの実装を持つ」には少し難題があって、
C# 13 時点では入らない可能性がかなり高いです。

## 普通の構造体 + Unsafe.As

拡張はラッパー構造体を使った実装になりそうです。
一時期は以下のような ref struct を使った実装になりそうだったんですが、
この案は結局没になりました。

<pre class="source" title="ref struct 案">
<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">SomeStruct</span>();
<span class="reserved">var</span> <span class="variable">extension</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">SomeExtension</span>(<span class="reserved">ref</span> <span class="variable">value</span>);

<span class="comment">// 拡張プロパティを呼び出す。</span>
<span class="variable">extension</span><span class="operator">.</span><span class="property">Property</span> <span class="operator">=</span> <span class="number">123</span>;

<span class="comment">// ちゃんと元インスタンスに値が反映。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">value</span><span class="operator">.</span><span class="method">GetValue</span>());

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">SomeExtension</span>(<span class="reserved">ref</span> <span class="type struct">SomeStruct</span> <span class="variable local">@this</span>)
{
    <span class="reserved">ref</span> <span class="type struct">SomeStruct</span> <span class="field">@this</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">@this</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">@this</span><span class="operator">.</span><span class="method">GetValue</span>(); <span class="comment">// ref で持ってるので、引数でもらった構造体に書き換えが反映される。</span>
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">@this</span><span class="operator">.</span><span class="method">SetValue</span>(<span class="reserved">value</span>);
    }
}

<span class="comment">// デモ用に構造体に変更。</span>
<span class="reserved">struct</span> <span class="type struct">SomeStruct</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetValue</span>() <span class="operator">=&gt;</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetValue</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetValue</span>(<span class="reserved">int</span> <span class="variable local">index</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">*</span> <span class="variable local">index</span>;
}
</pre>

この案に変わって、普通の構造体 + Unsafe.As を使う路線で考えているそうです。

<pre class="source" title="Unsafe.As 案">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">SomeStruct</span>();

<span class="comment">// Unsafe.As を使って、value 値が入っているの場所を無理やり SomeExtension で解釈。</span>
<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">extension</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="type"><span class="static">Unsafe</span></span><span class="operator">.</span><span class="static"><span class="method">As</span></span>&lt;<span class="type struct">SomeStruct</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="variable">value</span>);

<span class="comment">// 拡張プロパティを呼び出す。</span>
<span class="variable">extension</span><span class="operator">.</span><span class="property">Property</span> <span class="operator">=</span> <span class="number">123</span>;

<span class="comment">// extension の参照先が value なので、ちゃんと value が書き変わる。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">value</span><span class="operator">.</span><span class="method">GetValue</span>());

<span class="comment">// 普通の構造体。</span>
<span class="reserved">struct</span> <span class="type struct">SomeExtension</span>
{
    <span class="reserved">private</span> <span class="type struct">SomeStruct</span> <span class="field">@this</span>;

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Property</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">@this</span><span class="operator">.</span><span class="method">GetValue</span>();
        <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">@this</span><span class="operator">.</span><span class="method">SetValue</span>(<span class="reserved">value</span>);
    }
}

<span class="comment">// SomeStruct は先ほどと同じ。</span>
</pre>

## 型消去

Extensions は普通の型と同じように使えたりします。
(特に、`explicit` を付けた Extensions はむしろ「型を明示しないと使えない」状態になります。)
なのでこれを拡張型(extension types)と呼んだりもします。

で、前節の通り Extensions のコンパイル結果はラッパー構造体だったりするわけですが、
このラッパー構造体への変換(Unsafe.As)はあくまでメンバー参照のタイミングで行われます。
メソッドの引数などに拡張型を書くと、実際には「元の型 + 属性」(いわゆる「型消去」方式)になる予定です。
例えば、以下のようなメソッドを書いたとして、

<pre class="source" title="拡張型を引数に書く例">
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Sum</span></span>(<span class="type struct">SomeExtension</span> <span class="variable local">a</span>, <span class="type">List</span>&lt;<span class="type struct">SomeExtension</span>&gt; <span class="variable local">b</span>)
{
    <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="variable local">a</span><span class="operator">.</span><span class="property">Property</span>;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">b</span>) <span class="variable">sum</span> <span class="operator">+=</span> <span class="variable">x</span><span class="operator">.</span><span class="property">Property</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}
</pre>

以下のような類のコードに置き換わる予定です。

<pre class="source" title="拡張型を引数に書く例の展開結果">
<span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Sum</span></span>(
    <span class="comment">// SomeExtension は属性の中にしか残らない。</span>
    <span class="comment">// 元の、 SomeStruct に置き換わる。</span>
    [<span class="type">Extension</span>(<span class="reserved">typeof</span>(<span class="type struct">SomeExtension</span>))] <span class="type struct">SomeStruct</span> <span class="variable local">a</span>,
    [<span class="type">Extension</span>(<span class="reserved">typeof</span>(<span class="type struct">SomeExtension</span>))] <span class="type">List</span>&lt;<span class="type struct">SomeStruct</span>&gt; <span class="variable local">b</span>)
{
    <span class="comment">// メンバーアクセスするところで Unsafe.As</span>
    <span class="reserved">var</span> <span class="variable">sum</span> <span class="operator">=</span> <span class="type"><span class="static">Unsafe</span></span><span class="operator">.</span><span class="method"><span class="static">As</span></span>&lt;<span class="type struct">SomeStruct</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="variable local">a</span>)<span class="operator">.</span><span class="property">Property</span>;
    <span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">x</span> <span class="control">in</span> <span class="variable local">b</span>) <span class="variable">sum</span> <span class="operator">+=</span> <span class="static"><span class="type">Unsafe</span></span><span class="operator">.</span><span class="method"><span class="static">As</span></span>&lt;<span class="type struct">SomeStruct</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="static"><span class="type">Unsafe</span></span><span class="operator">.</span><span class="static"><span class="method">AsRef</span></span>(<span class="reserved">in</span> <span class="variable">x</span>))<span class="operator">.</span><span class="property">Property</span>;
    <span class="control">return</span> <span class="variable">sum</span>;
}
</pre>

[変性](../../../../study/csharp/oop/sp4_variance.md)を持っていない `List<T>` で、
`List<SomeStruct>` を `List<SomeExtension>` に変換する手段は通常全くありません。
型消去で `List<SomeExtension>` が `List<SomeStruct>` に置き換わることで、
`List<SomeStruct>` 型の変数を `List<SomeExtension>` 型の引数に渡せるようになっています。

## メンバーのルックアップ(継承)

拡張型は元となる型との間には、クラスの継承関係と似た関係が成り立ちます。
なので、メンバーのルックアップのルールも「クラスの継承に準ずる」で行きたいそうです。
例えば、派生クラスから基底クラスのメンバーを何の修飾もなしで(`this.` とか `base.` が必須ではなく)参照できるように、
拡張型から元となる型のメンバーも修飾なしで参照できます。

おさらい的に、「継承があるときのルックアップ」の例をいくつか紹介しておきます。
(拡張型中で元となる型と同名のメンバーを書くとこれに準ずることになると思われます。)

近い側優先:

<pre class="source" title="基底クラスと同名のメンバー参照">
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">new</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// 近い側優先なので、Derived.M が呼ばれる。</span>
        <span class="method">M</span>(<span class="number">1</span>);
    }
}
</pre>

もうちょっとわかりにくい例:

<pre class="source" title="基底クラスと同名で、引数の型が違うメンバー参照">
<span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable local">x</span>) { }
}

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="reserved">new</span> <span class="reserved">void</span> <span class="method"><span class="warning" title="CS0109">M</span></span>(<span class="reserved">object</span> <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>()
    {
        <span class="comment">// わかりにくいけども、Derived.M(object) の方が呼ばれる。</span>
        <span class="comment">// 引数の型を考えると Base.M(int) が呼ばれそうに見えるけども、そうはならない。</span>
        <span class="comment">// (「元々はなかったけど後から Base の方に M(int) が追加された」みたいな状況で破壊的変更にならないようにするため。)</span>
        <span class="method">M</span>(<span class="number">1</span>);
    }
}
</pre>

## メンバーのルックアップ(拡張同士)

あと、既存の拡張メソッドには以下のような優先度があります。

<pre class="source" title="インスタンス メソッド優先">
<span class="reserved">namespace</span> Ex1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> App1
{
    <span class="reserved">class</span> <span class="type">A</span>
    {
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Instance&quot;</span>);
    }

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Main</span></span>()
        {
            <span class="comment">// インスタンス メソッド優先。</span>
            <span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method">M</span>(); <span class="comment">// Instance</span>
        }
    }
}
</pre>

<pre class="source" title="同じ名前空間内の拡張メソッド優先">
<span class="reserved">namespace</span> Ex1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> App1
{
    <span class="reserved">class</span> <span class="type">A</span>;

    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> <span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Extension in App1&quot;</span>);
    }

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Main</span></span>()
        {
            <span class="comment">// 同じ名前空間内の拡張メソッド優先。</span>
            <span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method">M</span>(); <span class="comment">// in App1</span>
        }
    }
}
</pre>

<pre class="source" title="内側で using した方優先">
<span class="reserved">using</span> Ex1;

<span class="reserved">namespace</span> Ex1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> Ex2
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> App1
{
    <span class="reserved">using</span> Ex2;

    <span class="reserved">class</span> <span class="type">A</span>;

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Main</span></span>()
        {
            <span class="comment">// 内側で using した方優先。</span>
            <span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method">M</span>(); <span class="comment">// in Ex2</span>
        }
    }
}
</pre>

<pre class="source" title="優劣がない場合はコンパイル エラー">
<span class="reserved">namespace</span> Ex1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> Ex2
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;Extension in Ex1&quot;</span>);
    }
}

<span class="reserved">namespace</span> App1
{
    <span class="reserved">using</span> Ex1;
    <span class="reserved">using</span> Ex2;

    <span class="reserved">class</span> <span class="type">A</span>;

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Main</span></span>()
        {
            <span class="comment">// 優劣がない場合はコンパイル エラー。</span>
            <span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method"><span class="error" title="CS0121">M</span></span>();
        }
    }
}
</pre>

新しい拡張型でも同様のルールになると思われます。

一方で、旧「拡張メソッド」と新「拡張型」に優劣をつけるかという議題もありますが、
現状は「優劣つけない」という方向で検討されています。
というか、新旧混在した時点でコンパイル エラーにしようかという話もあるみたいです。

<pre class="source" title="優劣がない場合はコンパイル エラー">
<span class="reserved">namespace</span> Ex1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">AExtension</span></span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">this</span> App1<span class="operator">.</span><span class="type">A</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;old extension method&quot;</span>);
    }
}

<span class="reserved">namespace</span> Ex2
{
    <span class="reserved">implicit</span> <span class="reserved">extension</span> <span class="type"><span class="static">AExtension</span></span> <span class="reserved">for</span> <span class="type">A</span>
    {
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;new extension type&quot;</span>);
    }
}

<span class="reserved">namespace</span> App1
{
    <span class="reserved">using</span> Ex1; <span class="comment">// これが外にあってもエラーにする案もあり</span>
    <span class="reserved">using</span> Ex2;

    <span class="reserved">class</span> <span class="type">A</span>;

    <span class="reserved">class</span> <span class="type">Program</span>
    {
        <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">Main</span></span>()
        {
            <span class="comment">// 優劣を付けない(コンパイル エラーになる)。</span>
            <span class="comment">// 何なら新旧混在している時点でコンパイル エラーにする可能性濃厚。</span>
            <span class="reserved">new</span> <span class="type">A</span>()<span class="operator">.</span><span class="method"><span class="error" title="CS0121">M</span></span>();
        }
    }
}
</pre>

## インターフェイス実装

ここまでの話は割かし C# 13 で入りそうな話なんですが、
最後に1つ、13では入らなさそうなのがインターフェイス実装の後付けです。

これまでの話どおり、ラッパー構造体を作る方針で少し考えてみましょう。

インターフェイス実装に関する部分だけ残して、以下のようにしたとします。

<pre class="source" title="拡張型でインターフェイス実装">
<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> };
<span class="type struct">SomeExtension</span> <span class="variable">extension</span> <span class="operator">=</span> <span class="variable">value</span>;

<span class="variable">extension</span><span class="operator">.</span><span class="method">Equals</span>(<span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> });

<span class="reserved">explicit</span> <span class="reserved">extension</span> <span class="type">SomeExtension</span> <span class="reserved">for</span> <span class="type">SomeClass</span> : <span class="type">IEquatable</span>&lt;<span class="type">SomeExtension</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type">SomeExtension</span><span class="operator">?</span> <span class="variable local">other</span>) <span class="operator">=&gt;</span> <span class="field">Value</span> <span class="operator">==</span> <span class="variable local">other</span><span class="operator">?</span><span class="operator">.</span><span class="field">Value</span>;
}

<span class="reserved">class</span> <span class="type">SomeClass</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span>;
}
</pre>

ラッパー構造体で展開するとしたら以下のようになります。

<pre class="source" title="ラッパー構造体で展開">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> };
<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">extension</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="type"><span class="static">Unsafe</span></span><span class="operator">.</span><span class="method"><span class="static">As</span></span>&lt;<span class="type">SomeClass</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="variable">value</span>);

<span class="reserved">var</span> <span class="variable">temp</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> };

<span class="comment">// こういう風に直接インターフェイス メンバーを呼ぶ分には特に問題なさげ。</span>
<span class="variable">extension</span><span class="operator">.</span><span class="method">Equals</span>(<span class="static"><span class="type">Unsafe</span></span><span class="operator">.</span><span class="static"><span class="method">As</span></span>&lt;<span class="type">SomeClass</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="variable">temp</span>));

<span class="reserved">struct</span> <span class="type struct">SomeExtension</span> : <span class="type">IEquatable</span>&lt;<span class="type struct">SomeExtension</span>&gt;
{
    <span class="reserved">private</span> <span class="type">SomeClass</span> <span class="field">Value</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type struct">SomeExtension</span> <span class="variable local">other</span>) <span class="operator">=&gt;</span> <span class="field">Value</span><span class="operator">.</span><span class="field">Value</span> <span class="operator">==</span> <span class="variable local">other</span><span class="operator">.</span><span class="field">Value</span><span class="operator">?</span><span class="operator">.</span><span class="field">Value</span>;
}

<span class="reserved">class</span> <span class="type">SomeClass</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="field">Value</span>;
}
</pre>

この例はインターフェイス実装しているといっても、そもそもメンバーを直接呼んでいるので問題がないだけです。
問題は以下の状況。

* インターフェイス型や `object` 型の変数で受けてボックス化する場合
* ジェネリック メソッドに渡す場合

まず、インターフェイス型の変数で受けてみましょう。
`ReferenceEquals` や `is` 判定であまり期待通りとは言えない挙動を起こします。

<pre class="source" title="インターフェイス型の変数で受けてみる">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> };
<span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">extension</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="static"><span class="type">Unsafe</span></span><span class="operator">.</span><span class="method"><span class="static">As</span></span>&lt;<span class="type">SomeClass</span>, <span class="type struct">SomeExtension</span>&gt;(<span class="reserved">ref</span> <span class="variable">value</span>);

<span class="comment">// インターフェイスに渡そうとすると、この実装だとボックス化が発生。</span>
<span class="type">IEquatable</span>&lt;<span class="type struct">SomeExtension</span>&gt; <span class="variable">boxedExtension</span> <span class="operator">=</span> <span class="variable">extension</span>;

<span class="comment">// インスタンスが一致しなくなる。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="method">ReferenceEquals</span></span>(<span class="variable">value</span>, <span class="variable">boxedExtension</span>)); <span class="comment">// false</span>

<span class="comment">// ダウンキャストが失敗する。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">boxedExtension</span> <span class="reserved">is</span> <span class="type">SomeClass</span>); <span class="comment">// false</span>
</pre>

ジェネリク メソッドでは、以下のように、元の型と拡張型の両方の型情報を使う必要がでてきます。

<pre class="source" title="ジェネリク メソッドに拡張型を渡す">
<span class="reserved">var</span> <span class="variable">value</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">SomeClass</span> { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> };
<span class="type">List</span>&lt;<span class="type">SomeClass</span>&gt; <span class="variable">list</span> <span class="operator">=</span> [<span class="reserved">new</span>() { <span class="field">Value</span> <span class="operator">=</span> <span class="number">2</span> }, <span class="reserved">new</span>() { <span class="field">Value</span> <span class="operator">=</span> <span class="number">1</span> }, <span class="reserved">new</span>() { <span class="field">Value</span> <span class="operator">=</span> <span class="number">0</span> }];

<span class="comment">// SomeClass のままだと IEquatable 制約を満たさなくて呼べない。</span>
<span class="reserved">var</span> <span class="variable">i1</span> <span class="operator">=</span> <span class="method"><span class="static">IndexOf</span></span><span class="operator">&lt;</span><span class="error" title="CS0119"><span class="type">SomeClass</span></span><span class="operator">&gt;&gt;</span>(<span class="variable">list</span>, <span class="variable">value</span>);

<span class="comment">// これなら呼べるようになるはず。</span>
<span class="comment">// ただ、list は List&lt;SomeClass&gt; なので、やっぱり型消去が必要。</span>
<span class="comment">// 型引数が暗黙的に SomeClass と SomeExtension の2つに増えるような処理が必要。</span>
<span class="reserved">var</span> <span class="variable">i2</span> <span class="operator">=</span> <span class="static"><span class="method">IndexOf</span></span>&lt;<span class="type struct">SomeExtension</span>&gt;(<span class="variable"><span class="error" title="CS1503">list</span></span>, <span class="error" title="CS1503"><span class="variable">value</span></span>);

<span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">IndexOf</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type">List</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">list</span>, <span class="type param">T</span> <span class="variable local">value</span>)
    <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IEquatable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// 今の型システムだと T が通常の型か拡張型かを知るすべはなく、Unsafe.As 展開ができない。</span>
    <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> <span class="operator">=</span> <span class="number">0</span>; <span class="variable">i</span> <span class="operator">&lt;</span> <span class="variable local">list</span><span class="operator">.</span><span class="property">Count</span>; <span class="variable">i</span><span class="operator">++</span>)
        <span class="control">if</span> (<span class="variable local">list</span>[<span class="variable">i</span>]<span class="operator">.</span><span class="method">Equals</span>(<span class="variable local">value</span>))
            <span class="control">return</span> <span class="variable">i</span>;
    <span class="control">return</span> <span class="operator">-</span><span class="number">1</span>;
}
</pre>

いずれも、C# コンパイラー上のトリックでは問題を解消できなさそうで、
.NET ランタイムの型システムに手を入れる必要が出てきそうです。
型システムに手を入れるとなると結構大ごとなので、C# 13 で実現する見込みは残念ながらほぼありません。
