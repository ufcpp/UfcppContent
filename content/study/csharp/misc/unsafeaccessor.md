---
title: "UnsafeAccessor"
source_url: "https://ufcpp.net/study/csharp/misc/unsafeaccessor/"
content_type: "Article"
published_at: "2026-02-15T17:23:26"
updated_at: "2026-02-15T17:23:26"
tags: []
umbraco_id: 2518
parent_id: 1338
sort_order: 11
aliases:
  - "/csharp/misc/unsafeaccessor/"
---

# UnsafeAccessor

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

.NET 8 から、[リフレクション](../dynamic/sp_reflection.md)なしで[アクセシビリティ](../oop/oo_conceal.md#level)を無視した(private や internal なメンバーにアクセス可能な)仕組みとして UnsafeAccessor というものが追加されました。

用途が狭いですし、unsafe と付く名前通り割とデメリットもある機能なので、使い心地はそれほどよくありません。
「全くできないと困るので口だけは用意した」系の機能になります。
例えば、フィールド1個にアクセスするだけでも以下のような書き方になります。

<pre class="source" title="UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="type">A</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span>();

<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">RefValue</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>; <span class="comment">// private であることを無視して a._value = 1; 相当の処理を実行。</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// A._value にアクセスするためのメソッド。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;_value&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="static"><span class="method">RefValue</span></span>(<span class="type">A</span> <span class="variable local">@this</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

`UnsafeAccessor` 属性(`System.Runtime.CompilerServices`)を付けた extern メソッドを書くと、.NET Runtime が静的に(JIT 時に) `_value` フィールド直参照と同等のコードを生成します。

## <a id="sec-generated-title-2"></a> <a id="ignore-accessibility">アクセシビリティ無視とリフレクション</a>

これまでアクセシビリティを無視したい場合、
たとえ型情報が静的に既知であってもリフレクションを使っていました。
例えば冒頭の例同様に `a._value = 1;` するためだけに、以下のようなコードが必要になっていました。

<pre class="source" title="リフレクションで private フィールドにアクセスする例">
<span class="type">A</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span>();

<span class="comment">// a._value = 1; 相当のコードをリフレクションでやるとこうなる。</span>
<span class="reserved">typeof</span>(<span class="type">A</span>)
    <span class="operator">.</span><span class="method">GetField</span>(<span class="string">&quot;_value&quot;</span>, System<span class="operator">.</span>Reflection<span class="operator">.</span><span class="type">BindingFlags</span><span class="operator">.</span>NonPublic <span class="operator">|</span> System<span class="operator">.</span>Reflection<span class="operator">.</span><span class="type">BindingFlags</span><span class="operator">.</span>Instance)<span class="operator">!</span>
    <span class="operator">.</span><span class="method">SetValue</span>(<span class="variable">a</span>, <span class="number">1</span>);

<span class="comment">// Type 型インスタンスが作られて、</span>
<span class="comment">// FieldInfo 型インスタンスが作られて、</span>
<span class="comment">// 動的な処理で a._value = 1; 相当のコードを実行。</span>
<span class="comment">// しかも、int 型の 1 は object にボックス化されるコストもかかる。</span>

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

コード中のコメントに書きましたが、この処理には様々なオーバーヘッドがかかります。
本当に動的な処理がしたい(本当に実行時まで型に関する情報を知らない)なら必要なコストですが、
この例の `A` のように型が既知の場合だいぶもったいないです。

すなわち、リフレクションの持つ以下の2つの側面があるわけですが、

1. 実行時までわからない型のメンバーを読み書きする
2. 本来アクセスできないメンバーを読み書きする

このうち後者だけを切り出したものが UnsafeAccessor です。

UnsafeAccessor を使った冒頭のコードなら、
パフォーマンス的には `a._value = 1;` と同水準になります。

##### <a id="sec-generated-title-3"></a> <a id="unsafe">余談: 「unsafe」</a>

名前に unsafe の文字が入っていますが、
メモリ安全性や型安全性の保証はあります(この意味では普通に safe)。
バッファー オーバー ランのようなメモリ脆弱性を起こせるような機能ではないですし、
型が合わないようなコードを書くと JIT 時にエラーになります。

UnsafeAccessor の「unsafe」は「private や internal な物に触れるので変更されても文句が言えない」くらいの意味です。

また、C# コンパイラーのレベルでは型チェックできない(JIT 時チェックになるので実行してみる必要はある)という不利益もあります。


## <a id="sec-generated-title-4"></a> <a id="motivation">利用場面</a>

UnsafeAccessor の主な用途は以下のようなものです。

1. シリアライザーや [DI](https://learn.microsoft.com/ja-jp/dotnet/core/extensions/dependency-injection/overview) などをリフレクションから Source Generator に移行したい
2. 元々あった大きな[アセンブリ](../package/project.md)を複数に分ける際などにやむを得ず使う
3. 単体テスト用途

元々の想定利用場面は 1. になります。
シリアライザーでは private なフィールドへの読み書きをリフレクションで行うことが多かったです。
ところがこれは、
リフレクションを避けたい実行環境
([AOT](https://learn.microsoft.com/ja-jp/dotnet/core/deploying/native-aot/?tabs=windows%2Cnet8) や、[Web Assembly](https://ja.wikipedia.org/wiki/WebAssembly) など)で困りました。
シリアライザーの場合まさに「型は静的に既知」な場合が多く、
リフレクションを使った動的コード生成から、
[Source Generator](analyzer-generator.md) を使った静的なコード生成への移行が進んでいます
(例: [System.Text.Json のソース生成利用](https://learn.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json/source-generation))。
これがまさに UnsafeAccessor で想定する場面になります。

ただ、多くの人にとって、
Source Generator は使う側にはなる一方で、作る側になることは希少です。
そのため 1. の用途では、UnsafeAccessor も「ライブラリで内部的に使われている」であって、直接の利用はあまりないでしょう。

続いて 2. ですが、
これもよほど大規模でよほど歴史のあるコードでしか見られないものです。
例えば .NET の標準ライブラリ内で、
[System.Net.Security](https://source.dot.net/#System.Net.Security/src/libraries/Common/src/System/Net/Http/X509ResourceClient.cs) から [System.Net.Http](https://source.dot.net/#System.Net.Http/System/Net/Http/GlobalHttpSettings.cs) 内の internal 型への参照が残っています。
元々大きな1つのアセンブリとして提供していたものを「Http」と「Security」で分けたときに困ったものと思われます。

結局のところ、多くの人にとっては 3. の単体テスト用途で使うことが多くなると思われます。
「private メンバー、internal メンバーの単体テストはすべきかどうか」ということ自体議論が分かれる問題ですが、まあ「private アクセスしたくなったことがある」くらいであれば多くの人が通ったことがある道だと思います。

例えば以下のようなライブラリ コードがあったとして、
「`Dispose` 後は中身が null になっていてほしい」というテストを書くみたいな用途が考えられます。

<pre class="source" title="">
<span class="reserved">namespace</span> Lib;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Class1</span>(<span class="reserved">object</span><span class="operator">?</span> <span class="variable local">resource</span>) : <span class="type">IDisposable</span>
{
    <span class="reserved">private</span> <span class="reserved">object</span><span class="operator">?</span> <span class="field">_someResource</span> <span class="operator">=</span> <span class="variable local">resource</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>()
    {
        <span class="field">_someResource</span> <span class="operator">=</span> <span class="reserved">null</span>;
    }
}
</pre>

<pre class="source" title="">
<span class="reserved">using</span> Lib;
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">namespace</span> TestProject1;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">UnitTest1</span>
{
    [<span class="type">Fact</span>]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose後は中身がnull</span>()
    {
        <span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">Class1</span>(<span class="string">&quot;&quot;</span>);

        <span class="reserved">using</span> (<span class="variable">x</span>)
        {
        }

        <span class="type">Assert</span><span class="operator">.</span>Null(<span class="static"><span class="method">GetSomeResourceRef</span></span>(<span class="variable">x</span>));
    }

    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;_someResource&quot;</span>)]
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">object</span><span class="operator">?</span> <span class="static"><span class="method">GetSomeResourceRef</span></span>(<span class="type">Class1</span> <span class="variable local">class1</span>);
}
</pre>

こういう単体テストがいいのかどうかという話はありますが…
(仕様に応じて「`Dispose` 後に何らかの操作をしても何も起きない」とか「`ObjectDisposedException` が出る」とか、別の形でのテストが望ましい可能性あり。)
「やりたいことは時々ある」の範疇かと思われます。

単体テストの場合、ライブラリと単体テストでプロジェクトは分かれているものの、同じ人がコードを書くことが非常に多いです。
そのため「private なものは変更されても文句が言えない」問題が軽微(変更して単体テストが失敗するようになっても、修正する義務を同じ人が負うのですぐに直すことになるだけ)なので、UnsafeAccessor とは相性がいいです。

## <a id="sec-generated-title-5"></a> <a id="how-to-use">UnsafeAccessor の書き方</a>

すでにいくつかの例を書いていますが、UnsafeAccessor を使うには
`UnsafeAccessor` 属性(`System.Runtime.CompilerServices` 名前空間)を付けた extern メソッドを書きます。

`UnsafeAccessor` 属性の第1引数に渡す `UnsafeAccessorKind` には以下の5つの値があります
(ほぼ名前通り):

* `Constructor`: コンストラクター
* `Method`: インスタンス メソッド
* `StaticMethod`: 静的メソッド
* `Field`: インスタンス フィールド
* `StaticField`: 静的フィールド

プロパティ、インデクサーや演算子などは「.NET の型システム上はメソッドになっている」という仕様があるので、`Method` か `StaticMethod` を使ってそのメソッドを呼び出すことになります。

ちなみに、型自体は public という場面では `A Accessor()` とか `void Accessor(A x)` みたいな素直な書き方で `A` の private メンバーにアクセスできるんですが、
型自体が internal の時はさらにちょっと面倒な書き方が必要になります。
この場合は、`UnsafeAccessorType` というもう1つの属性を使って、文字列で型名を指定することになります。

### <a id="sec-generated-title-6"></a> <a id="constructor">コンストラクター</a>

コンストラクターへのアクセスは以下のように書きます。

<pre class="source" title="コンストラクターに対する UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="static"><span class="method">CreateA</span></span>());
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="static"><span class="method">CreateA</span></span>(<span class="number">1</span>));

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// new A() 相当。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="type">A</span> <span class="static"><span class="method">CreateA</span></span>();

    <span class="comment">// new A(value) 相当。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="type">A</span> <span class="method"><span class="static">CreateA</span></span>(<span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="type">A</span>() { }
    <span class="reserved">private</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;

    <span class="reserved">private</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

「何の型のコンストラクターを呼ぶか」は戻り値の型を見ます。
メソッドの引数の型はアクセス先のコンストラクターの引数と一致させます。
(引数名は違っていても平気です。一致の必要があるのは型のみ。)

この機能は構造体に対しても使えます。

<pre class="source" title="構造体に対しても利用可能">
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="comment">// 構造体に対しても使えて、書き方はクラスの場合と同じ。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="type struct">A</span> <span class="static"><span class="method">CreateA</span></span>(<span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">private</span> <span class="type struct">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

### <a id="sec-generated-title-7"></a> <a id="method">インスタンス メソッド</a>

インスタンス メソッドへのアクセスは以下のように、
「先頭に1つ引数を足す」書き方をします。

<pre class="source" title="インスタンス メソッドに対する UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="method"><span class="static">Add</span></span>(<span class="variable">a</span>, <span class="number">1</span>);
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">var</span> <span class="variable">b</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">B</span>();
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">Add</span></span>(<span class="reserved">ref</span> <span class="variable">b</span>, <span class="number">1</span>);
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">b</span>); <span class="comment">// B(1)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// 第1引数をアクセス先の型にする。</span>
    <span class="comment">// (拡張メソッドと同じ感覚。)</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">Add</span></span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">value</span>);

    <span class="comment">// 構造体のインスタンス メソッドを呼びたい場合は ref 引数にする。</span>
    <span class="comment">// このメソッドの名前とアクセス先のメソッドの名前が違う場合は、Name で明示的に指定する。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;PrivateAdd&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">Add</span></span>(<span class="reserved">ref</span> <span class="type struct">B</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}

<span class="reserved">struct</span> <span class="type struct">B</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">PrivateAdd</span>(<span class="reserved">int</span> <span class="variable local">value</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">+=</span> <span class="variable local">value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">readonly</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">B(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

2つ目以降の引数の型はアクセス先のメソッドの引数と一致させます。
(コンストラクター同様、型のみの一致で OK。)
また、戻り値の型が一致している必要もあります。

アクセス先の型が構造体の場合、第1引数は `ref` もしくは `in` でないとアクセスできなくなります。
(実行時エラーを起こします。)

また、`UnsafeAccessor` 属性をつけるメソッドの名前とアクセス先のメソッドの名前が異なる場合には `Name` プロパティの明示が必要になります。

### <a id="sec-generated-title-8"></a> <a id="field">フィールド</a>

インスタンス フィールドへのアクセスは以下のように、
「`ref` 戻り値、引数なしのメソッド」で書きます。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="static"><span class="method">Value</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>;
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="comment">// 引数なし、ref 戻り値なメソッドでアクセス。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;_value&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">Value</span></span>(<span class="type">A</span> <span class="variable local">a</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

フィールドとメソッドでは命名規約が違う(`_value` と `Value` になる)ので、
自然な書き方をしようとすると `Name` の明示が必要になるかと思いますが、
メソッド名が変でもいいなら以下のように `Name` の省略もできます。

<pre class="source" title="メソッド名をフィールドに一致させると Name 省略可能">
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="comment">// あまりメソッドに _ 始まりの名前を付けないものの、気にしないのであればこれでも OK。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">_value</span></span>(<span class="type">A</span> <span class="variable local">a</span>);
}
</pre>

### <a id="sec-generated-title-9"></a> <a id="static">静的メソッド、静的フィールド</a>

静的メソッド、静的フィールドへのアクセスには `UnsafeAccessorKind` の `StaticMethod` と `StaticField` を使います。
インスタンス メソッド、インスタン フィールドの時と同様、引数の先頭にアクセス先の型を足します(静的メンバーなのでこの第1引数は使われず、ダミー引数になります)。

<pre class="source" title="静的メンバーに対する UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="static"><span class="method">Value</span></span>(<span class="reserved">null</span>) <span class="operator">=</span> <span class="number">2</span>;

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="reserved">null</span>, <span class="number">3</span>));

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>StaticField, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;_value&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="static"><span class="method">Value</span></span>(<span class="type">A</span><span class="operator">?</span> <span class="variable local">_</span>);

    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>StaticMethod)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">int</span> <span class="static"><span class="method">M</span></span>(<span class="type">A</span><span class="operator">?</span> <span class="variable local">_</span>, <span class="reserved">int</span> <span class="variable local">x</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="field">_value</span></span>;
    <span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="operator">*</span> <span class="static"><span class="field">_value</span></span>;
}
</pre>

### <a id="sec-generated-title-10"></a> <a id="property">プロパティ</a>

`UnsafeAccessorKind` にはプロパティを指定する方法はありませんが、
C# のプロパティは内部的にはメソッドになっているので、
そのメソッドに対する UnsafeAccessor を作ることでプロパティにアクセスできます。

プロパティ `T P` があったとすると、
`get`/`set` アクセサーにはそれぞれ
`T get_P()`、 `void set_P(T value)` というメソッドが対応します。
(元のプロパティ名 + `get_`/`set_` 接頭辞。)

<pre class="source" title="プロパティへの UnsafeAccessor は get_ / set_ メソッドで代用">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">SetValue</span></span>(<span class="variable">a</span>, <span class="number">1</span>);

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">GetValue</span></span>(<span class="variable">a</span>)); <span class="comment">// 1</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// Value プロパティの get アクセサーは get_Value。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;get_Value&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">int</span> <span class="static"><span class="method">GetValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>);

    <span class="comment">// Value プロパティの set アクセサーは set_Value。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;set_Value&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">SetValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="property">Value</span>{ <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

ちなみにこの[「プロパティ名 + `get_`/`set_` 接頭辞」ルールは C# の言語仕様で決まっています](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153102-member-names-reserved-for-properties)。
少なくとも C# コンパイラーで作ったプロパティの場合は必ずこのルールに基づくメソッド名になっています。
([IL アセンブラー](https://learn.microsoft.com/ja-jp/dotnet/framework/tools/ilasm-exe-il-assembler)を使えばこのルールを破れるはずですが、そんなことをやっている人は見たことがありません。)

### <a id="sec-generated-title-11"></a> <a id="indexer">インデクサー</a>

[プロパティ](#property)同様です。
[C# の仕様](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153104-member-names-reserved-for-indexers)上、インデクサーからは `get_Item` / `set_Item` というメソッドが作られているはずなので、これを経由してアクセスします。

<pre class="source" title="インデクサーへの UnsafeAccessor は get_Item / set_Item メソッドで代用">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="number">2</span>);
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">SetValue</span></span>(<span class="variable">a</span>, <span class="number">0</span>, <span class="number">1</span>);
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">SetValue</span></span>(<span class="variable">a</span>, <span class="number">1</span>, <span class="operator">-</span><span class="number">1</span>);

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A([1, -1])</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">GetValue</span></span>(<span class="variable">a</span>, <span class="number">0</span>)); <span class="comment">// 1</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="method"><span class="static">GetValue</span></span>(<span class="variable">a</span>, <span class="number">1</span>)); <span class="comment">// -1</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="comment">// インデクサーの get アクセサーは get_Item。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;get_Item&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">int</span> <span class="static"><span class="method">GetValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">i</span>);

    <span class="comment">// インデクサーの set アクセサーは set_Item。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;set_Item&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">SetValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">i</span>, <span class="reserved">int</span> <span class="variable local">value</span>);
}

<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">length</span>)
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span>[] <span class="field">_items</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">int</span>[<span class="variable local">length</span>];
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> <span class="variable local">i</span>]{ <span class="reserved">get</span> <span class="operator">=&gt;</span> <span class="field">_items</span>[<span class="variable local">i</span>]; <span class="reserved">set</span> <span class="operator">=&gt;</span> <span class="field">_items</span>[<span class="variable local">i</span>] <span class="operator">=</span> <span class="reserved">value</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A([</span>{<span class="reserved">string</span><span class="operator">.</span><span class="method"><span class="static">Join</span></span>(<span class="string">&quot;, &quot;</span>, <span class="field">_items</span>)}<span class="string">])</span><span class="string">&quot;</span>;
}
</pre>

(ちなみにこの `Item` の部分は `IndexerName` 属性を使って変更できたりします。
実際、`string` 型のインデクサーは `Chars` という名前です。
この場合、作られているメソッドの名前も `get_Chars` です。)


### <a id="sec-generated-title-12"></a> <a id="operator">演算子</a>

C# のユーザー定義の演算子は public でないといけない仕様なので、わざわざ UnsafeAccessor を使う場面はより一層少ないですが、一応触れておきます。
(後述する[型自体が internal な場合](#unsafe-accessor-type)に対して使えなくはないです。)

ユーザー定義の演算子も、内部的にはただのメソッドになります。
`+` 演算子の場合は `op_Addition` など、演算子ごとに名前が決まっています。
(参考: [演算子とメソッド名の対応関係](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#153106-method-names-reserved-for-operators))

<pre class="source" title="演算子に対するアクセスは op_ から始まるメソッドで代用">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="number">1</span>);

<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="method"><span class="static">Add</span></span>(<span class="variable">a</span>, <span class="number">1</span>); <span class="comment">// a += 1;</span>

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(2)</span>

<span class="variable">a</span> <span class="operator">=</span> <span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">Add</span></span>(<span class="reserved">null</span>, <span class="variable">a</span>, <span class="number">1</span>); <span class="comment">// a = a + 1;</span>

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(3)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="comment">// + は op_Addition。</span>
    <span class="comment">// 静的メソッドなのでダミーの第1引数が必要。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>StaticMethod, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;op_Addition&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="type">A</span> <span class="static"><span class="method">Add</span></span>(<span class="type">A</span><span class="operator">?</span> <span class="variable local">_</span>, <span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">x</span>);

    <span class="comment">// += は op_AdditionAssignment。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;op_AdditionAssignment&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="static"><span class="method">Add</span></span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">x</span>);
}

<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span> <span class="operator">=</span> <span class="variable local">value</span>;

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">A</span> <span class="reserved">operator</span> <span class="operator">+</span>(<span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">new</span>(<span class="variable local">a</span><span class="operator">.</span><span class="field">_value</span> <span class="operator">+</span> <span class="variable local">x</span>);

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="reserved">operator</span> <span class="operator">+=</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">_value</span> <span class="operator">+=</span> <span class="variable local">x</span>;

    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>


### <a id="sec-generated-title-13"></a> <a id="extension">拡張メンバーを UnsafeAccessor にする</a>

UnsafeAccessor の「静的メソッドにして第1引数を足す」という仕様が拡張メソッドと相性がよく、
拡張メソッドをそのまま UnsafeAccessor にすることができます。

<pre class="source" title="拡張メソッドに UnsafeAccessor 属性をつけるだけ">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="number">1</span>);

<span class="comment">// 拡張メソッド X.Add(A, int) を通して private な A.Add(int) を呼ぶ。</span>
<span class="variable">a</span><span class="operator">.</span><span class="method">Add</span>(<span class="number">1</span>);

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(2)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="static"><span class="method">Add</span></span>(<span class="reserved">this</span> <span class="type">A</span> <span class="variable local">a</span>, <span class="reserved">int</span> <span class="variable local">x</span>);
}

<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

これは C# 14 で入る `extension` ブロック形式の拡張メンバーでもできて、
以下のような書き方で UnsafeAccessor を書けたりします。

<pre class="source" title="extension ブロックで UnsafeAccessor を作る例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="number">1</span>);

<span class="comment">// 拡張メソッド X.Add を経由して、private な A.Add を呼ぶ。</span>
<span class="variable">a</span><span class="operator">.</span><span class="method">Add</span>(<span class="number">1</span>);

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(2)</span>

<span class="comment">// 拡張プロパティ X.Value を経由して、private な A.Value を取得・設定する。</span>
<span class="variable">a</span><span class="operator">.</span><span class="property">Value</span> <span class="operator">=</span> <span class="number">3</span>;
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span><span class="operator">.</span><span class="property">Value</span>); <span class="comment">// 3</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    <span class="reserved">extension</span>(<span class="type">A</span> <span class="variable local">a</span>)
    {
        <span class="comment">// コンパイル結果は void Add(this A, int x) と一緒。</span>
        <span class="comment">// そのまま UnsafeAccessor にできる。</span>
        [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
        <span class="reserved">public</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>);

        <span class="comment">// 拡張プロパティも通常のプロパティと同じ get_ / set_ という名前でメソッドを作る仕様で、</span>
        <span class="comment">// int get_Value(A) / void set_Value(A, int) というメソッドができてる。</span>
        <span class="comment">// 属性も伝搬されてて、 A.Value の UnsafeAccessor にできる。</span>
        <span class="reserved">public</span> <span class="reserved">extern</span> <span class="reserved">int</span> <span class="property">Value</span>
        {
            [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
            <span class="reserved">get</span>;

            [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
            <span class="reserved">set</span>;
        }
    }
}

<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; } <span class="operator">=</span> <span class="variable local">value</span>;
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Add</span>(<span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="property">Value</span> <span class="operator">+=</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

### <a id="sec-generated-title-14"></a> <a id="generics">ジェネリックな型やメンバーへのアクセス</a>

.NET 9 からはジェネリックな型に対して UnsafeAccessor を書けるようになりました。
型引数は以下のように「型は型に、メソッドはメソッドに」というルールで書けば大丈夫です。

<pre class="source" title="ジェネリックな UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>&lt;<span class="reserved">int</span>&gt;(<span class="number">1</span>);

<span class="static"><span class="type">X</span></span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="variable">a</span>); <span class="comment">// 1</span>
<span class="type"><span class="static">X</span></span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="method"><span class="static">M</span></span>(<span class="variable">a</span>, <span class="string">&quot;abc&quot;</span>); <span class="comment">// 1 abc</span>

<span class="comment">// 型の型引数は型に。</span>
<span class="comment">// A&lt;T&gt; の T はここに書く。</span>
<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>&lt;<span class="type param">T</span>&gt;
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="type">A</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">a</span>);

    <span class="comment">// メソッドの型引数はメソッドに。</span>
    <span class="comment">// A&lt;T&gt;.M&lt;U&gt; の U はここに書く。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Method)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">U</span>&gt;(<span class="type">A</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">a</span>, <span class="type param">U</span> <span class="variable local">x</span>);
}

<span class="reserved">class</span> <span class="type">A</span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">value</span>)
{
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">M</span>() <span class="operator">=&gt;</span> <span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">value</span>);
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">M</span>&lt;<span class="type param">U</span>&gt;(<span class="type param">U</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">$&quot;</span>{<span class="variable local">value</span>}<span class="string"> </span>{<span class="variable local">x</span>}<span class="string">&quot;</span>);
}
</pre>

型引数の付け方は制限がかかっていて、
上記の例とは違う書き方、
例えば「クラス `X` にメソッド `M<T>` や `M<T, U>` を書く」みたいなことをすると実行時エラーになります。

### <a id="sec-generated-title-15"></a> <a id="unsafe-accessor-type">型自体が internal な場合</a>

.NET 10 から「型自体が internal で参照できない」という場合に対して UnsafeAccessor を使う手段が提供されるようになりました。

これまでの例で `A` 型の引数や戻り値を書いていた場所をとりあえず `object` 型にして、
その代わり、引数・戻り値に `UnsafeAccessorType` 属性を付けます。
`UnsafeAccessorType` 属性の引数に文字列で型名を書くことで、internal な型を参照できます。

例えばまず、`ClassLibrary1` という名前のプロジェクトに以下のようなクラスを用意したとします。

<pre class="source" title="ClassLibrary1 プロジェクト内に internal なクラスを用意">
<span class="reserved">namespace</span> Lib;

<span class="comment">// ClassLibrary1 というプロジェクト内にあるものとする。</span>
<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

この型を別プロジェクトから参照するには以下のように書きます。

<pre class="source" title="ClassLibaray1 内の Lib.A クラスにアクセスするための UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="static"><span class="method">CreateA</span></span>(); <span class="comment">// var a = new A();</span>
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">_value</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>;   <span class="comment">// a._value = 1;</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">X</span></span>
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    [<span class="reserved">return</span>: <span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A, ClassLibrary1&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">object</span> <span class="static"><span class="method">CreateA</span></span>();

    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="static"><span class="method">_value</span></span>([<span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A, ClassLibrary1&quot;</span>)] <span class="reserved">object</span> <span class="variable local">@this</span>);
}
</pre>

型名はフルネーム(`Lib.A`)で書き、
`,` でつなげて アセンブリ名(通常、プロジェクト名がそのままアセンブリ名になります。今回は `ClassLibrary1`)を書きます。

`UnsafeAccessorType` 属性は拡張メンバーでも使えます。
(`object` 型インスタンスに対する拡張になってしまって誤用が怖いという問題はあり。)

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="method"><span class="static">CreateA</span></span>(); <span class="comment">// var a = new A();</span>

<span class="comment">// 拡張メソッド呼び。</span>
<span class="variable">a</span><span class="operator">.</span><span class="method">_value</span>() <span class="operator">=</span> <span class="number">1</span>;   <span class="comment">// a._value = 1;</span>

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>);

<span class="comment">// 拡張プロパティ呼び。</span>
<span class="variable">a</span><span class="operator">.</span><span class="property">Value</span> <span class="operator">=</span> <span class="number">2</span>;   <span class="comment">// a._value = 2;</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    [<span class="reserved">return</span>: <span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A, ClassLibrary1&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">object</span> <span class="static"><span class="method">CreateA</span></span>();

    <span class="comment">// 拡張メソッドでも使える。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">_value</span></span>([<span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A, ClassLibrary1&quot;</span>)] <span class="reserved">this</span> <span class="reserved">object</span> <span class="variable local">@this</span>);

    <span class="comment">// extension ブロックでも使える。</span>
    <span class="reserved">extension</span>([<span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A, ClassLibrary1&quot;</span>)] <span class="reserved">object</span> <span class="variable local">@this</span>)
    {
        <span class="reserved">public</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="property">Value</span>
        {
            [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;_value&quot;</span>)]
            <span class="reserved">get</span>;
        }
    }
}
</pre>

[ジェネリックな型](#generics)の場合は `` `1 `` みたいな語尾をつける必要があります。
例として `ClassLibrary1` 側のクラス `A` を以下のようにジェネリック クラスにしてみます。

<pre class="source" title="internal なジェネリック クラスを用意">
<span class="reserved">namespace</span> Lib;

<span class="comment">// ClassLibrary1 というプロジェクト内にあるものとする。</span>
<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">A</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type param">T</span> <span class="field">_value</span>;
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="field">_value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

これを参照するためには以下のような書き方になります。

<pre class="source" title="ClassLibaray1 内の Lib.A&lt;T&gt; クラスにアクセスするための UnsafeAccessor の例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="type"><span class="static">X</span></span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="static"><span class="method">CreateA</span></span>(); <span class="comment">// var a = new A();</span>
<span class="type"><span class="static">X</span></span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="static"><span class="method">_value</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>;   <span class="comment">// a._value = 1;</span>

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>);

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>&lt;<span class="type param">T</span>&gt;
{
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Constructor)]
    [<span class="reserved">return</span>: <span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A`1, ClassLibrary1&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">object</span> <span class="static"><span class="method">CreateA</span></span>();

    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="method"><span class="static">_value</span></span>([<span class="type">UnsafeAccessorType</span>(<span class="string">&quot;Lib.A`1, ClassLibrary1&quot;</span>)] <span class="reserved">object</span> <span class="variable local">@this</span>);
}
</pre>

UnsafeAccessor 定義側のクラス(この例だと `X<T>`)の書き方は[前節](#generics)と同じです。
一方、 `UnsafeAccessorType` 属性に渡す型名は、
[`Type` 型](https://learn.microsoft.com/ja-jp/dotnet/api/system.type) の [`FullName` プロパティ](https://learn.microsoft.com/ja-jp/dotnet/api/system.type.fullname)で得られる文字列と同じです。
`A<T>` であれば `` Lib.A`1 `` になります。
`` ` `` はバッククオートで、日本語キーボードの場合 shift + `@` で入力するやつです。
また、`1` の部分は型引数の個数で、
これが `A<T1, T2>` の場合だと `` `2 ``、
`A<T1, T2, T3>` の場合だと `` `3 `` になります。


### <a id="sec-generated-title-16"></a> <a id="compiler-generated-field">コンパイラー生成のフィールド</a>

ここからは C# の言語仕様にはない話になります。
(現在の Roslyn と呼ばれる C# コンパイラーの実装ではそうなっているけども、
将来もずっと同じ実装が続くかとかの保証はない話です。)

[自動プロパティ](../oop/oo_property.md#auto)
([`field` キーワード](../oop/oo_property.md#field-keyword)を使ったものも含む)からはフィールドが生成されています。
現在の Roslyn の場合、このフィールドの命名ルールは「プロパティ `P` に対して `<P>k__BackingField`」みたいになります。
この挙動を使えばプロパティのバッキング フィールドを読み書きすることができます。

<pre class="source" title="バッキング フィールドを UnsafeAccessor を使って読み書き">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>();
<span class="static"><span class="type">X</span></span><span class="operator">.</span><span class="static"><span class="method">RefValue</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>;

<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// これで Value プロパティのバッキング フィールドを参照できる。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;&lt;Value&gt;k__BackingField&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">RefValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>);
}

<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="property">Value</span> { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="property">Value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>

また、[プライマリ コンストラクター引数のキャプチャ](../oop/oo_construct.md#capture)で作られるフィールドは、引数 `x` に対して `<x>P` という名前になります。

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">A</span>(<span class="number">0</span>);
<span class="type"><span class="static">X</span></span><span class="operator">.</span><span class="method"><span class="static">RefValue</span></span>(<span class="variable">a</span>) <span class="operator">=</span> <span class="number">1</span>; <span class="comment">// 書き換え。</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="variable">a</span>); <span class="comment">// A(1)</span>

<span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">X</span></span>
{
    <span class="comment">// プライマリ コンストラクター引数から作られるフィールドは &lt;&gt;P。</span>
    [<span class="type">UnsafeAccessor</span>(<span class="type">UnsafeAccessorKind</span><span class="operator">.</span>Field, <span class="property">Name</span> <span class="operator">=</span> <span class="string">&quot;&lt;value&gt;P&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">extern</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">RefValue</span></span>(<span class="type">A</span> <span class="variable local">a</span>);
}

<span class="reserved">class</span> <span class="type">A</span>(<span class="reserved">int</span> <span class="variable local">value</span>)
{
    <span class="reserved">public</span> <span class="reserved">override</span> <span class="reserved">string</span> <span class="method">ToString</span>() <span class="operator">=&gt;</span> <span class="string">$&quot;</span><span class="string">A(</span>{<span class="variable local">value</span>}<span class="string">)</span><span class="string">&quot;</span>;
}
</pre>
