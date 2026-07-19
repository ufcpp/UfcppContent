---
title: "C# 14.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver14/"
content_type: "Article"
published_at: "2025-08-31T00:00:00"
updated_at: "2025-12-20T20:47:29"
tags: []
umbraco_id: 2514
parent_id: 1174
sort_order: 19
aliases:
  - "/csharp/cheatsheet/ap_ver14/"
---

# C# 14.0 の新機能

<div class="version version14">Ver. 14.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2025/11</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>.NET 10.0</li>
<li>Visual Studio 2022 18.0</li>
</td>
</tr>
</table>

執筆予定: [C# 14.0 トラッキング issue](https://github.com/ufcpp/UfcppSample/issues/487)

##<a id="sec-generated-title-1"></a> <a id="field-keyword">field キーワード</a>
`field` という文脈キーワードが追加されました。
プロパティの `get`/`set` の中に `field` と書くと、
バッキング フィールドを生成した上で、そのフィールドの読み書きができます。
例えば前述の例を `field` を使って書き直すと以下のようになります。

<pre class="source" title="field キーワードの例">
<span class="reserved">using</span> System<span class="operator">.</span>ComponentModel;
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">class</span> <span class="type">FieldBackedProperties</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="comment">// 遅延初期化: 最初のプロパティ アクセス時にインスタンスを生成。</span>
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">X</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??=</span> <span class="string">&quot;&quot;</span>;

    <span class="comment">// set 側だけ null 許容(get 側で ?? で非 null 化)。</span>
    [<span class="type">AllowNull</span>]
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="property">Y</span>
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="reserved">field</span> <span class="operator">??</span> <span class="string">&quot;&quot;</span>;
        <span class="reserved">set</span>;
    }

    <span class="comment">// INotifyPropertyChanged の実装: get 側だけ素通し。</span>
    <span class="reserved">public</span> <span class="reserved">string</span><span class="operator">?</span> <span class="property">Z</span>
    {
        <span class="reserved">get</span>;
        <span class="reserved">set</span>
        {
            <span class="control">if</span> (<span class="reserved">field</span> <span class="operator">!=</span> <span class="reserved">value</span>)
            {
                <span class="reserved">field</span> <span class="operator">=</span> <span class="reserved">value</span>;
                PropertyChanged<span class="operator">?</span><span class="operator">.</span><span class="method">Invoke</span>(<span class="reserved">this</span>, <span class="reserved">new</span>(<span class="reserved">nameof</span>(<span class="property">Z</span>)));
            }
        }
    }

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">PropertyChangedEventHandler</span><span class="operator">?</span> PropertyChanged;
}
</pre>

詳しくは「[field キーワード](../oop/oo_property.md#field-keyword)」で説明します。

##<a id="sec-generated-title-2"></a> <a id="null-conditional-assignment">null 条件代入</a>
代入演算の左側で `?.` や `?[]` を書くことで「null じゃないときだけ代入」ができるようになりました。
これを null 条件代入(null conditional assignment)といいます。

<pre class="source" title="null 条件代入の例">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">A</span><span class="operator">?</span> <span class="variable local">a</span>)
{
    <span class="comment">// if (a != null) a.X = 10; とほぼ同じ。</span>
    <span class="variable local">a</span><span class="operator">?</span><span class="operator">.</span><span class="property">X</span> <span class="operator">=</span> <span class="number">10</span>;

    <span class="comment">// if (a != null) a[0] = 10; とほぼ同じ。</span>
    <span class="variable local">a</span><span class="operator">?</span>[<span class="number">0</span>] <span class="operator">=</span> <span class="number">10</span>;

    <span class="comment">// if (a != null) a.Event += () =&gt; { }; とほぼ同じ。</span>
    <span class="variable local">a</span><span class="operator">?</span><span class="operator">.</span>Event <span class="operator">+=</span> () <span class="operator">=&gt;</span> { };
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">X</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">index</span>]
    {
        <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="number">0</span>;
        <span class="reserved">set</span> { }
    }

    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">Action</span><span class="operator">?</span> <span class="warning" title="CS0067">Event</span>;
}
</pre>

詳しくは「[null の取り扱い - null じゃないときだけ代入](../resource/rm_nullusage.md#null-conditional-assignment)」で説明します。

##<a id="sec-generated-title-3"></a> <a id="first-class-span">First-class Span</a>
`Span<T>`/`ReadOnlySpan<T>` 構造体を言語構文的に特別扱いするようなりました。

詳しくは「[First-class Span](../resource/span.md#first-class-span)」で説明します。
##<a id="sec-generated-title-4"></a> <a id="overload-compound">複合代入演算子のオーバーロード</a>
複合代入演算子を直接オーバーロードできるようになりました。

<pre class="source" title="複合代入演算子のオーバーロードの例">
<span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">X</span>(<span class="reserved">int</span> <span class="variable local">Value</span>)
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">-=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">-=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">*=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">*=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">/=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">/=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">%=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">%=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&amp;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&amp;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">|=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">|=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">^=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">^=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&lt;&lt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&lt;&lt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&gt;&gt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&gt;&gt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">&gt;&gt;&gt;=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">&gt;&gt;&gt;=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">-=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">*=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">/=</span>(<span class="reserved">int</span> <span class="variable local">value</span>) { <span class="reserved">checked</span> { <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">value</span>; }; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">++</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">++</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">--</span>() <span class="operator">=&gt;</span> <span class="property">Value</span><span class="operator">--</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">++</span>() { <span class="reserved">checked</span> { <span class="property">Value</span><span class="operator">++</span>; } }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="reserved">checked</span> <span class="operator">--</span>() { <span class="reserved">checked</span> { <span class="property">Value</span><span class="operator">--</span>; } }
}
</pre>

以前から二項演算子(`+` など)のオーバーロードをすることで、それに対応する複合代入(`+=` など)ができていましたが、この実装だとコピーのコストが不可避でした。
複合代入演算子を直接オーバーロードすることでこのコストを削減できます。

詳しくは「[複合代入演算子のオーバーロード](../oop/oo_operator.md#overload-compound)」で説明します。

##<a id="sec-generated-title-5"></a> <a id="simple-param-with-modifier">修飾子付きの引数の型名省略</a>
`ref` や `out` などの修飾子が必須の引数でも、ラムダ式引数の型名を省略できるようになりました。

<pre class="source" title="修飾子が必須でも引数の型名を省略できるように">
<span class="comment">// C# 13 までは型名省略不可で、(string text, out int result) のように書く必要があった。</span>
<span class="type">TryParse</span>&lt;<span class="reserved">int</span>&gt; <span class="variable">m</span> <span class="operator">=</span> (<span class="variable local">text</span>, <span class="reserved">out</span> <span class="variable local">result</span>) <span class="operator">=&gt;</span> { <span class="variable local">result</span> <span class="operator">=</span> <span class="number">0</span>; <span class="control">return</span> <span class="reserved">true</span>; };

<span class="reserved">delegate</span> <span class="reserved">bool</span> <span class="type">TryParse</span>&lt;<span class="type param">T</span>&gt;(<span class="reserved">string</span> <span class="variable local">text</span>, <span class="reserved">out</span> <span class="type param">T</span> <span class="variable local">result</span>);
</pre>

詳しくは「[修飾子付きの引数の型名省略](../functional/fun_localfunctions.md#simple-param-with-modifier)」で説明します。

##<a id="sec-generated-title-6"></a> <a id="others">その他</a>
###<a id="sec-generated-title-7"></a> <a id="partial-event">部分イベントと部分コンストラクター</a>
[部分プロパティ](../misc/partial-type.md#partial_property) (C# 13)に続いて、
C# 14 では[イベント](../functional/sp_event.md)と[コンストラクター](../oop/oo_construct.md)も部分定義できるようになりました。

<pre class="source" title="部分イベントと部分コンストラクターの例">
<span class="comment">// 元コード(手書き想定)。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="comment">// 部分イベント。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">event</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> PartialEvent;

    <span class="comment">// 部分コンストラクター。</span>
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="type">PartialClass</span>();
}

<span class="comment">// コード生成で作ってもらう前提のコード。</span>
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">PartialClass</span>
{
    <span class="reserved">private</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> <span class="field">_partialEvent</span>;
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">event</span> <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">?</span> PartialEvent
    {
        <span class="reserved">add</span> <span class="operator">=&gt;</span> <span class="field">_partialEvent</span> <span class="operator">+=</span> <span class="reserved">value</span>;
        <span class="reserved">remove</span> <span class="operator">=&gt;</span> <span class="field">_partialEvent</span> <span class="operator">-=</span> <span class="reserved">value</span>;
    }

    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="type">PartialClass</span>() { }
}
</pre>

###<a id="sec-generated-title-8"></a> <a id="unbount-type-in-nameof">unbound な型に対する nameof</a>
`T<>` みたいに型引数を埋めていないジェネリック型(これを unbound (未束縛)とか open (開きっぱなし) な型といいます)に対して `nameof` 演算子を使えるようになりました。

<pre class="source" title="unbound なジェネリック型に対する nameof 演算子">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;&gt;)); <span class="comment">// &quot;List&quot;</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">Dictionary</span>&lt;,&gt;<span class="operator">.</span><span class="property">Keys</span>)); <span class="comment">// &quot;Keys&quot;</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;&gt;<span class="operator">.</span><span class="type struct">Enumerator</span><span class="operator">.</span><span class="method">MoveNext</span>)); <span class="comment">// &quot;MoveNext&quot;</span>
</pre>

詳しくは「[unbound な型に対する nameof](../start/st_string.md#unbount-type-in-nameof)」で説明します。

###<a id="sec-generated-title-9"></a> <a id="file-based-app">ファイル ベース実行</a>
.NET 10 (C# 14 と同世代)で単独の `.cs` ファイルだけで C# プログラムを実行できるようになりました。

それに伴って、C# 的にも `#!` と `#:` (無視ディレクティブ)という機能が追加されています。
例えば以下のようなコードが書けて、
Unix 系シェルの [shebang](https://ja.wikipedia.org/wiki/%E3%82%B7%E3%83%90%E3%83%B3_(Unix)) を書けたり、これまでであればプロジェクト(`.csproj` ファイル中)に書いていた設定の類を C# ソースコード中に書けるようになっています。

<pre class="source" title="shebang 入り .cs ファイル">
<span class="comment">#!/usr/bin/env dotnet</span>
<span class="preprocess">#</span><span class="preprocess">:</span><span class="preprocess">sdk</span><span class="string"> Microsoft.NET.Sdk.Web</span>

<span class="reserved">var</span> <span class="variable">app</span> <span class="operator">=</span> <span class="type">WebApplication</span><span class="operator">.</span>CreateBuilder(<span class="reserved">args</span>)<span class="operator">.</span>Build();
<span class="variable">app</span><span class="operator">.</span>MapGet(<span class="string">&quot;/&quot;</span>, () <span class="operator">=&gt;</span> <span class="string">&quot;Hello World!&quot;</span>);
<span class="variable">app</span><span class="operator">.</span>Run();
</pre>

詳しくは「[ファイル ベース実行](file-based-app.md)」で説明します。
