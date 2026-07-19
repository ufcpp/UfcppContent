---
title: "file ローカル型"
source_url: "https://ufcpp.net/study/csharp/misc/file-local/"
content_type: "Article"
published_at: "2022-08-25T00:00:00"
updated_at: "2024-08-31T17:24:49"
tags: []
umbraco_id: 2431
parent_id: 1338
sort_order: 10
aliases:
  - "/csharp/misc/file-local/"
---

# file ローカル型

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`file` という修飾子を使って「書いたファイル内からだけアクセスできる型」を作れるようになりました。
これを <strong id="branch" class="key-file-local-type">file ローカル型</strong> (file-local type)と言います。

例えば、あるファイルに以下のようなコードを書いたとします。

<pre class="source" title="file 修飾付きの型を使う例">
<span class="number">1</span><span class="operator">.</span><span class="method">M</span>();

<em><span class="reserved">file</span></em> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=></span> <span class="type"><span class="static">Console</span><span class="operator">.<span class="method"><span class="static">WriteLine</span></span>(<span class="variable local">x</span>);
}
</pre>

これと同じプロジェクト内の別のファイルに以下のようなコードを書いてもエラーにはなりません。

<pre class="source" title="別のファイルに同名の file 修飾付きの型を定義">
<em><span class="reserved">file</span></em> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">this</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=></span> <span class="type"><span class="static">Console</span><span class="operator">.<span class="method"><span class="static">WriteLine</span></span>(<span class="string">"別ファイルの file-local Extensions"</span>);
}
</pre>

通常、global な場所(どの名前空間にも属さない場所)に、`Extensions` なんていうよくありそうな名前のクラスを作るとすぐに名前が衝突しますが、
`file` が付いていることによって、全くの同名の型があってもコンパイルできるようになります。

## <a id="sec-generated-title-1"></a> <a id="vs-internal">private や internal と比べて</a>

この手の「見える範囲を制限する」系の処理の用途の1つとして、
「派生クラス・インターフェイス実装クラスを隠す」というのがあります。

例えば、以下のようなコードを書いて、
`Disposable.FromAction` 越しに `IDisposable` でインスタンスを返し、
実装クラスである `ActionDisposable` は直接は使わせないというようなことがしたいことがあります。

<pre class="source" title="実装クラスを隠す例">
<span class="comment">// file 修飾子を付けると、このファイル内からしかアクセスできない。</span>
<span class="reserved">file</span> <span class="reserved">class</span> <span class="type">ActionDisposable</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">private</span> <span class="type">Action</span> <span class="field">_disposer</span>;
    <span class="reserved">public</span> <span class="type">ActionDisposable</span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="field">_disposer</span> <span class="operator">=</span> <span class="variable local">disposer</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() <span class="operator">=></span> <span class="field">_disposer</span>();
}

<span class="comment">// public クラスの、</span>
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Disposable</span></span>
{
    <span class="comment">// public メソッドで、</span>
    <span class="comment">// 戻り値は public interface なので大丈夫。</span>
    <span class="comment">// 内部でだけ file-local な型を使う。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IDisposable</span> <span class="method"><span class="static">FromAction</span></span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="reserved">new</span> <span class="type">ActionDisposable</span>(<span class="variable local">disposer</span>);
}
</pre>

こういう「隠す」用途であれば、
これまでも、`internal` や `private` でもある程度代用できました。

`private` の例:

<pre class="source" title="private で実装を隠す例">
<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Disposable</span></span>
{
    <span class="comment">// private にしておけば Disposable クラスの外からは触れない。</span>
    <span class="reserved">private</span> <span class="reserved">class</span> <span class="type">ActionDisposable</span> : <span class="type">IDisposable</span>
    {
        <span class="reserved">private</span> <span class="type">Action</span> <span class="field">_disposer</span>;
        <span class="reserved">public</span> <span class="type">ActionDisposable</span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="field">_disposer</span> <span class="operator">=</span> <span class="variable local">disposer</span>;
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() <span class="operator">=></span> <span class="field">_disposer</span>();
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IDisposable</span> <span class="method"><span class="static">FromAction</span></span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="reserved">new</span> <span class="type">ActionDisposable</span>(<span class="variable local">disposer</span>);
}
</pre>

`internal` の例:

<pre class="source" title="internal で実装を隠す例">
<span class="comment">// internal にしておけば別プロジェクトからは触れない。</span>
<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">ActionDisposable</span> : <span class="type">IDisposable</span>
{
    <span class="reserved">private</span> <span class="type">Action</span> <span class="field">_disposer</span>;
    <span class="reserved">public</span> <span class="type">ActionDisposable</span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="field">_disposer</span> <span class="operator">=</span> <span class="variable local">disposer</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() <span class="operator">=></span> <span class="field">_disposer</span>();
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="static"><span class="type">Disposable</span></span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">IDisposable</span> <span class="method"><span class="static">FromAction</span></span>(<span class="type">Action</span> <span class="variable local">disposer</span>) <span class="operator">=></span> <span class="reserved">new</span> <span class="type">ActionDisposable</span>(<span class="variable local">disposer</span>);
}
</pre>

多くの場合はこれらで十分ですし、
C# 10 以前ではこれでしのいできました。

ただ、問題になったのが [Source Generator](analyzer-generator.md#analyzer) によるコード生成です。
コード生成でクラスを生成したい場合、

* 複数の Source Generator によって「名前の取り合い」が起きかねない
    * 1つのクラスに対して複数の Source Generator を掛けるとき、たとえ `private` でコード生成しても名前が被る可能性がある
    * もし異なる作者の Source Generator で名前が被った場合、解決のしようがない
* Source Generator のアップデート時にクラス名を変えたり、クラス自体を消したりしたいことがある

という懸念・要求が出てきました。
`file` 修飾子によって得られるのは、この「他とは絶対に名前の取り合いにならない型名」になります。

ということで、`file` 修飾子があって一番うれしい用途は Source Generator です。
実際これは、.NET 6 で追加された `Regex` の Source Generator 対応(`GeneratedRegex`)から出て来た要望で、
`GeneratedRegex` は .NET 7 で早速この `file` 修飾子を使ったコード生成をするようになりました。

<pre class="source" title="GeneratedRegex の例">
<span class="reserved">using</span> System<span class="operator">.</span>Text<span class="operator">.</span>RegularExpressions;

<span class="reserved">namespace</span> FileLocal;

<span class="reserved">internal</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">R</span>
{
    <span class="comment">// file 修飾子、Source Generator で使う需要が高い。</span>
    <span class="comment">// 例えば、GeneratedRegex は早速(.NET 7 から)使ってる。</span>
    [<span class="type">GeneratedRegex</span>(<span class="string">@"\d+"</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="type">Regex</span> <span class="method"><span class="static">M</span></span>();

    <span class="comment">// ↑このメソッドから、</span>
    <span class="comment">// file sealed class M_0 : Regex { } みたいなクラスが作られてる。</span>
}
</pre>

## <a id="sec-generated-title-2"></a> <a id="applicable">適用範囲</a>

`file` 修飾子は型にのみ適用できます。
以下のように、フィールドやメソッドなどに使おうとするとコンパイル エラーになります。

<pre class="source" title="file は型のみ">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">file</span> <span class="reserved">int</span> <span class="field"><span class="error" title="CS0106">_x</span></span>;

    <span class="reserved">file</span> <span class="reserved">int</span> <span class="method"><span class="error" title="CS0106">M</span></span>() <span class="operator">=></span> <span class="field">_x</span>;
}
</pre>

一方、型であれば何でもよくて、インターフェイス、列挙型、デリゲートなどにも使えます。
以下のコードはいずれも問題なくコンパイルできます。

<pre class="source" title="型なら何でも file を付けれる">
<span class="reserved">file</span> <span class="reserved">interface</span> <span class="type">IA</span> { }
<span class="reserved">file</span> <span class="reserved">enum</span> <span class="type">E</span> { }
<span class="reserved">file</span> <span class="reserved">delegate</span> <span class="reserved">void</span> <span class="type">D</span>();
<span class="reserved">file</span> <span class="reserved">struct</span> <span class="type struct">S</span> { }
<span class="reserved">file</span> <span class="reserved">record</span> <span class="type">R</span>;</span>
<span class="reserved">file</span> <span class="reserved">record</span> <span class="reserved">struct</span> <span class="type struct">RS</span>;
</pre>

インターフェイスであれば、file ローカルなインターフェイスを `public` な型で実装することもできます。
これを使って、「file ローカルなメソッド」の代用にはなったりします。

<pre class="source" title="file ローカルなインターフェイスを public なクラスで実装する例">
<span class="comment">// file ローカルなインターフェイスも OK だし、</span>
<span class="comment">// それを public な型で実装するのも OK。</span>

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">CX</span> : <span class="type">IX</span> <span class="comment">// OK</span>
{
    <span class="comment">// file ローカルなインターフェイス で明示的実装しておけば実質 file ローカルなメソッドになる。</span>
    <span class="comment">// (ちなみに、別に明示的実装でなく普通に実装しても OK)。</span>
    <span class="reserved">void</span> <span class="type">IX</span><span class="operator">.</span><span class="method">M</span>() { }
}

<span class="reserved">file</span> <span class="reserved">interface</span> <span class="type">IX</span>
{
    <span class="reserved">void</span> <span class="method">M</span>();
}
</pre>

また、`file` 修飾子は[アクセシビリティ修飾子](../oop/oo_conceal.md#level)と同時に使うことはできません。
例えば以下のコードはコンパイル エラーになります。

<pre class="source" title="アクセシビリティとの併用不可">
<em><span class="reserved">internal</span> <span class="reserved">file</span></em> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static"><span class="error" title="CS9052">X</span></span></span>
{
}
</pre>

さらに、 `file` 修飾子は top-level (global な場所、もしくは、名前空間直下)の型にしか使えません。
言い換えると、入れ子の型は file ローカルにできません。
以下のコードはコンパイル エラーになります。

<pre class="source" title="入れ子の型不可">
<span class="reserved">class</span> <span class="type">A</span>
{
    <em><span class="reserved">file</span></em> <span class="reserved">class</span> <span class="type"><span class="error" title="CS9054">NestedFileClass</span></span>
    {
    }
}</pre>

## <a id="sec-generated-title-3"></a> <a id="implementation">内部実装</a>

file ローカルな型のコンパイル結果は、
C# にはよくある「通常の C# からは参照できない名前」(unspeakable name)に変換されます。
名前付けのルールは仕様化されていなくて、「常に同じ名前で生成される保証はない」とされています。
(この辺りは unspeakable name を生成する他の言語機能も同じです。)

現在の file ローカル型の名前付けでは、
ファイル名と連番が入った「`<file_name>f1_ClassName`」みたいな名前で生成されています。
file ローカル型の存在意義的に、「プロジェクト全体で一意な名前」であれば十分なはずで、
連番だけでも目的は果たせていそうです。
型名にファイル名が入ってるのはおそらくデバッグ時にスタックトレースを見やすくするためなど、付加的な目的だと思われます。
