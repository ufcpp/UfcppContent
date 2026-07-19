---
title: "ref構造体"
source_url: "https://ufcpp.net/study/csharp/resource/refstruct/"
content_type: "Article"
published_at: "2017-11-18T00:00:00"
updated_at: "2024-06-22T00:00:00"
tags: []
umbraco_id: 2107
parent_id: 1286
sort_order: 7
aliases:
  - "/csharp/resource/refstruct/"
---

# ref構造体

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
[前項](span.md)では、C# 7.2 の新機能と深くかかわる `Span<T>` 構造体という型を紹介しました。
この型は、論理的には `(ref T Reference, int Length)` というような、「参照フィールド」と長さのペアを持つ構造体です。
「参照」を持っているので、参照戻り値や参照ローカル変数と同種の「出所の保証」が必要です。
また`Span<T>` には「[スタック](misc_heap.md)上に置かれている必要がある」(ヒープに置けない)という制限が必要です。

さらに、`Span<T>` に制限が掛かっている以上、「`Span<T>`を持つ型」にも再帰的に制限が掛かります。
「`Span<T>` を持つか持たないか」だけで挙動が変わるのでは影響範囲が大きすぎるため、
「`Span<T>` を持ちたければ `ref` という修飾が必要」という制約もあります。

ここでは、これらの `Span<T>` の「スタック上に置かれている必要がある」という制約や、「`ref` 構造体」について説明していきます。
(`ref`構造体という機能ではありますが、主用途が`Span<T>`に関するものなので、span safety ruleと呼ばれたりもします。)

##<a id="sec-generated-title-2"></a> <a id="ref-struct"></a>ref 構造体
`Span<T>` には制限が必要といっても、C# コンパイラーとしては `Span<T>` だけを特別扱いしたくはありません。
そこで、<strong id="key-refstruct" class="keyword">`ref`構造体</strong> (`ref struct`)というものを導入しました。

`ref`構造体は、名前通り、`ref` 修飾子が付いた構造体です。
`Span<T>` 構造体自身にも `ref` 修飾子がついています。
そして、`ref`構造体をフィールドとして持てるのは`ref`構造体だけです。

<pre class="source" title="ref構造体を持てるのはref構造体だけ">
<code><span class="comment">// Span&lt;T&gt; は ref 構造体になっている</span>
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">Span</span>&lt;<span class="type">T</span>&gt; { ... }

<span class="comment">// ref 構造体を持てるのは ref 構造体だけ</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefStruct</span>
{
    <span class="reserved">private</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; _span; <span class="comment">//OK</span>
}
</code></pre>

逆に言うと、`ref` 修飾子がついていない構造体や、クラスは`ref`構造体をフィールドとして持てません。

<pre class="source" title="">
<code><span class="comment">// NG。構造体以外を「ref 型」にはできない</span>
<span class="reserved">ref</span> <span class="reserved"><span class="error">class</span></span> <span class="type">InvalidClass</span> { }

<span class="comment">// ref がついていない普通の構造体は ref 構造体を持てない</span>
<span class="reserved">struct</span> <span class="type">NonRefStruct</span>
{
    <span class="reserved">private</span> <span class="error"><span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;</span> _span; <span class="comment">//NG</span>
}
</code></pre>

そして、以下で説明する制約は、`Span<T>` 構造体だけでなく、すべての `ref` 構造体に対して掛かります。

##<a id="sec-generated-title-3"></a> <a id="flow-analysis"></a>戻り値で返せるもの
`ref` 構造体を戻り値として使いたい場合、
[`ref` 戻り値・`ref` ローカル変数](sp_ref.md#ref-returns)と同様に、大元をたどって調べて(フロー解析して)、返していいものかどうかを判定します。
以下のようなルールがあります([`ref`戻り値と同じルール](sp_ref.md#flow-analysis)です)。

- 引数で受け取ったものは戻り値に返せます
- ローカルで確保したものは返せません
- 引数などを介して多段に参照している場合、コードをたどって大元が安全かまで調べます

<pre class="source" title="戻り値に返せるかどうか">
<code><span class="comment">// 引数で受け取ったものは戻り値で返せる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Success(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x) =&gt; x;

<span class="comment">// ローカルで確保したもの変数はダメ</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Error()
{
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];
    <span class="reserved">return</span> <span class="error">x</span>;
}

<span class="comment">// 多段の場合も元をたどって出所を調べてくれる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Success(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x, <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; y)
{
    <span class="reserved">var</span> r1 = x;
    <span class="reserved">var</span> r2 = y;
    <span class="reserved">var</span> r3 = r1.Length &gt;= r2.Length ? r1 : r2;

    <span class="comment">// r3 は出所をたどると引数の x か y</span>
    <span class="comment">// x も y も引数なので大丈夫</span>
    <span class="reserved">return</span> r3;
}

<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Error(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x, <span class="reserved">int</span> n)
{
    <span class="reserved">var</span> r1 = x;
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; r2 = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[n];
    <span class="reserved">var</span> r3 = r1.Length &gt;= r2.Length ? r1 : r2;

    <span class="comment">// r2 がローカルなのでダメ</span>
    <span class="reserved">return</span> <span class="error">r3</span>;
}
</code></pre>

ちなみに、上記の`Error`と似たようなコードでも、以下のコードはコンパイルできます。
ちゃんと「メモリ確保があったかどうか」を見ていて、「`default`であれば何も確保していない」という判定もしています。

<pre class="source" title="default は何も確保しない">
<code><span class="comment">// ちゃんと「メモリ確保」があったかどうかを見てる</span>
<span class="comment">// 同じようなコードでもこれは OK (default だと何も確保しない)</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; Success1()
{
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x = <span class="reserved">default</span>;
    <span class="reserved">return</span> x;
}
</code></pre>

このルールは、`ref`構造体と、`ref`引数・`ref`戻り値の間でも働きます。
例えば、引数由来の `Span<T>`から得た`ref T`な参照は戻り値にできますが、ローカル由来のものはできません。

<pre class="source" title="Span&gt;T&lt;とref T">
<code><span class="comment">// 引数で受け取った Span 由来の ref 戻り値は返せる</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Success(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x) =&gt; <span class="reserved">ref</span> x[0];

<span class="comment">// ローカルで確保した Span 由来の ref 戻り値はダメ</span>
<span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Error()
{
    <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];
    <span class="reserved">return</span> <span class="reserved">ref</span> <span class="error">x</span>[0];
}
</code></pre>

###<a id="sec-generated-title-4"></a> <a id="readonly-ref"></a>readonly ref
C# 7.2 で追加された構造体がらみの修飾子には[`readonly`](readonlyness.md#readonly-struct)というものもあります。
`readonly`修飾は、一見、参照がらみの機能とは無関係に見えますが、実はこれも「参照として返せるかどうか」の判定に関係しています。

例えば以下のコードを見てください。

<pre class="source" title="readonly修飾とref構造体">
<code><span class="reserved">using</span> System;

<span class="comment">// ref だけ</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefToSpan</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; _span;
    <span class="reserved">public</span> RefToSpan(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; span) =&gt; _span = span;

    <span class="comment">// 例え _span に readonly が付いていても、this 書き換えが可能</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Method(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; span) { <span class="reserved">this</span> = <span class="reserved">new</span> RefToSpan(span); }
}

<span class="comment">// readonly ref</span>
<span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RORefToSpan</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; _span;
    <span class="reserved">public</span> <span class="reserved">void</span> Method(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; span) { }
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> LocalToRef(<span class="type">RefToSpan</span> r)
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];
        <span class="error">r.Method(local)</span>; <span class="comment">// ここでエラーになる。r の中身が書き換えられることで、local が外に漏れる可能性を危惧</span>

        <span class="comment">// 注: この例の場合は実際には漏れはしないものの、RefToSpan の作り次第なので保証はできない</span>
    }

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> LocalToRORef(<span class="type">RORefToSpan</span> r)
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];
        r.Method(local); <span class="comment">// readonly ref に対してなら OK</span>
    }
}
</code></pre>

ローカルで定義した`Span<T>`を、引数で渡ってきた`ref`構造体のメソッドに対して渡しています。
この場合、`readonly`がついている場合にだけコンパイルできます。
`readonly`がついていない方では、メソッドの中で`r`が書き換わる可能性があります。
その結果「ローカルの`Span<T>`が外に漏れる可能性がある」という判定を受けるため、コンパイル エラーになります。
`readonly`がついている方では「書き換えがあり得ない」ということで、「外にも漏れない」という判定になります。

###<a id="sec-generated-title-5"></a> <a id="unsafe"></a>余談: さすがに unsafe までは追えない
参照がらみのフロー解析は、あくまで`ref`ローカル変数や、`ref`構造体に対してだけ働きます。
`unsafe`を使って、ポインターなどを介するとさすがに追跡できません。

例えば、以下のコードは不正で、実行時エラーであったり、予期しない動作を招く可能性があります。
しかし、コンパイラーが不正を判定できず、コンパイル時にエラーにすることができません。

<pre class="source" title="unsafe な手段までは追えない">
<code><span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; X()
{
    <span class="comment">// ローカル</span>
    <span class="reserved">int</span> x = 10;

    <span class="comment">// unsafe な手段でローカルなものの参照を作って返す</span>
    <span class="comment">// これをやってしまうとまずいものの、コンパイル時にはエラーにできない</span>
    <span class="reserved">return</span> <span class="reserved">new</span> <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;(&amp;x, 1);
}
</code></pre>

##<a id="sec-generated-title-6"></a> <a id="stack-only"></a>「スタックのみ」制約
`ref`構造体はスタック上に置かれている必要があります。
この性質から、`ref`構造体は「stack-only 型」と呼ばれることもあります。
この制限が必要になるのは以下の2つの理由からです。

- そもそも参照自体がスタック上でしか働かない
- マルチスレッド動作時に安全性を保証できない

まず、`ref` 構造体以前に、参照自体がスタック上でしか使えません。
参照は、常にその参照の出所をトラッキングする必要があります。
例えば、出所がクラス(.NET の[ガベージ コレクション](rm_gc.md#garbage-collection)の管理下)の場合、
それを参照する方もガベージ コレクションのトラッキングの対象になります。
このトラッキング処理を低コストで行うためには、参照がスタック上になければなりません。

次に、マルチスレッド動作に関してですが、
`Span<T>` の中身が論理的には `(ref T Reference, int Length)` という2要素からなることによります。
安全に使うには、この2つが[アトミック](../async/sp_thread.md#lock)に読み書きされなければなりません。
もし、`Reference` だけが書き換わり、`Length` がまだ書き換わっていないタイミングで参照先を読み書きされてしまうと、
範囲チェックが正しく働かず、不正な領域を読み書きしてしまう危険性が出てきます。

ということで、「スタック上に置かれている必要がある」という制約が掛かります。
具体的には、以下のような制限があります。

- クラスのフィールドとして持てない(クラスに `ref` 修飾子を付けれない理由はこれ)
- [クラスのフィールドに昇格](../functional/sp2_anonymousmethod.md)する可能性があることができない
  - [ローカル関数](../functional/fun_localfunctions.md#key-local)や[ラムダ式](../functional/fun_localfunctions.md#key-anonymous)で[キャプチャ](../functional/fun_localfunctions.md#capture-local)できない
  - [イテレーター](../data/sp2_iterator.md)の引数には使えない
  - イテレーター内では、`yield return` をまたいで使えない
  - [非同期メソッド](../async/sp5_async.md)に対しては引数にもローカル変数にも使えない
      - ([C# 13 で緩和](../cheatsheet/ap_ver13.md#ref-in-async)。C# 13 からは、`await` をまたがない限り、ローカル変数に使えます)
- [ボックス化](rmboxing.md)できない
  - `object`や`dynamic`、インターフェイス型の変数に代入できない
  - `ToString` など、`object` 型のメソッドを呼べない
- ジェネリック型引数として使えない

<pre class="source" title="ref構造体は stack-only">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="comment">//❌ そもそもクラスに ref を付けれないのも stack-only を保証するため</span>
<span class="reserved">ref</span> <span class="reserved"><span class="error">class</span></span> <span class="type">Class</span> { }

<span class="comment">//❌ インターフェイス実装</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type">RefStruct</span> : <span class="type"><span class="error">IDisposable</span></span> { <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { } }

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">//❌ 非同期メソッドの引数</span>
    <span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> Async(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="error">x</span>)
    {
        <span class="comment">//❌ 非同期メソッドのローカル変数</span>
        <span class="error"><span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;</span> local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[10];
    }

    <span class="comment">//❌ イテレーターの引数</span>
    <span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; Iterator(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x)
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[10];
        local[0] = 1; <span class="comment">//⭕ yield return をまたがないならOK</span>
        <span class="reserved">yield</span> <span class="reserved">return</span> local[0];
        <span class="comment">//❌ yield をまたいだ読み書き</span>
        <span class="error">local</span>[0] = 2; <span class="comment">// ダメ</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; local = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[1];

        <span class="comment">//❌ box 化</span>
        <span class="reserved">object</span> obj = <span class="error">local</span>;

        <span class="comment">//❌ object のメソッド呼び出し</span>
        <span class="reserved">var</span> str = <span class="error">local</span>.ToString();

        <span class="comment">//❌ クロージャ</span>
        <span class="type">Func</span>&lt;<span class="reserved">int</span>&gt; a1 = () =&gt; <span class="error">local</span>[0];
        <span class="reserved">int</span> F() =&gt; <span class="error">local</span>[0];

        <span class="comment">//❌ 型引数にも渡せない</span>
        <span class="type">List</span>&lt;<span class="error"><span class="type">Span</span>&lt;<span class="reserved">int</span>&gt;</span>&gt; list;
    }
}
</code></pre>


###<a id="sec-generated-title-7"></a> <a id="TypedReference"></a>余談: TypedReference
「[型付き参照](../interop/sp_makeref.md)」で説明している`TypedReference`型も、内部的に参照を持っている型の1つです。
`TypedReference` は ref 構造体の仕様よりも古くからあって、昔はこの型だけに対して特殊対応をしていました。

その昔からある `TypedReference` に対する特殊対応は、本項で説明している C# 7.2 から入った ref 構造体に対する制約よりもだいぶ緩くて、実は「スタック上に置かれている必要がある」制約から割かし簡単に外れることができました。

ちなみに、C# 7.2 で ref 構造体を導入後、
.NET Core 2.1 からは `TypedReference` に対する特殊対応は止めて、単に `TypedReference` を ref 構造体に変更したようです。
結果的に元よりも制約が厳しくなっていて、昔は(バグっている可能性が非常に高いものの)一応コンパイルできていたコードがコンパイル エラーになる可能性があります。
(ただ、`TypedReference` 自体利用頻度が非常に低いので問題にはなっていません。)

##<a id="sec-generated-title-8"></a> <a id="ref-field">ref フィールド</a>
<h5 class="version version11">Ver. 11</h5>

C# 11 で、[ref 構造体](#key-refstruct)のフィールドを [`ref` (参照渡し)](sp_ref.md#byref)で持てるようになりました。
これを <strong id="key-ref-field" class="keyword">ref フィールド</strong>(ref field)と言います。

ref フィールドの書き方は参照引数や参照戻り値と同じく、型の前に `ref` 修飾を付けます。

<pre class="source" title="ref フィールド">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">ByReference</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="field">Value</span>;
}
</pre>

C# 7.2 に頃に [`Span<T>` 構造体の内部的な話](span.md#fast-span)で、「`Span<T>` はランタイム側で特殊処理を入れている」というような話を書いていましたが、
ref フィールドが入ったことで、通常の C# コードで同様のことができるようになりました。
実際、.NET 7 からはそういう実装に置き換わっていて、`Span<T>` の内部は晴れて以下のようなコードに変更されています。

<pre class="source" title=".NET 7 での Span の中身">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="reserved">internal</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="type param">T</span> <span class="field">_reference</span>;
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">_length</span>;
}
</pre>

ちなみに、ref フィールドを持てるのは ref 構造体だけです。
以下のコードはコンパイル エラーになります。

<pre class="source" title="">
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field"><span class="error" title="CS9059">_x</span></span>; <span class="comment">// class 中はダメ。</span>
}

<span class="reserved">struct</span> <span class="type struct">B</span>
{
    <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field"><span class="error" title="CS9059">_x</span></span>; <span class="comment">// struct も ref がついてないものの中はダメ。</span>
}
</pre>

###<a id="sec-generated-title-9"></a> <a id="readonly-ref">readonly ref</a>
C# 7.2 の頃に [`ref readonly`](sp_ref.md#ref-readonly) というものがありました。
これは、「参照先の値の変更不可」というものです。
一方で、ref フィールドになると、`ref readonly` と `readonly ref` の2種類の readonly ができます(あるいは両方付けて `readonly ref readonly` もできます)。

比較のためにまず、どちらの readonly もついていない状態ですが、
当然、「どこを参照するか変更」と「参照先の値の変更」のどちらもできます。

<pre class="source" title="✔「どこを参照するか変更」と✔「参照先の値の変更」">
<span class="reserved">scoped</span> <span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>();

<span class="reserved">int</span> <span class="variable">x1</span> <span class="operator">=</span> <span class="number">0</span>;
<span class="variable">a</span><span class="operator">.</span><span class="field">X</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x1</span>; <span class="comment">// どこを参照するかを変更。</span>

<span class="variable">a</span><span class="operator">.</span><span class="field">X</span> <span class="operator">=</span> <span class="number">2</span>; <span class="comment">// 参照先の値を変更</span>

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">public</span> <em><span class="reserved">ref</span></em> <span class="reserved">int</span> <span class="field">X</span>;
}
</pre>

で、`ref readonly` の方は C# 7.2 の頃からある意味と同じで、「参照先の値の変更不可」です。

<pre class="source" title="✔「どこを参照するか変更」と✖「参照先の値の変更」">
<span class="reserved">scoped</span> <span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>();

<span class="reserved">int</span> <span class="variable">x1</span> <span class="operator">=</span> <span class="number">0</span>;
<span class="variable">a</span><span class="operator">.</span><span class="field">X</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x1</span>; <span class="comment">// どこを参照するかを変更。</span>

<span class="variable"><span class="error" title="CS8331">a</span><span class="operator">.</span><span class="field">X</span></span> <span class="operator">=</span> <span class="number">2</span>; <span class="comment">// エラー: 参照先の値を変更不可。</span>

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">public</span> <em><span class="reserved">ref</span> <span class="reserved">readonly</span></em> <span class="reserved">int</span> <span class="field">X</span>;
}
</pre>

一方、C# 11 から書ける `readonly ref` は、要は、ref フィールド `ref T X` を readonly にするという意味なので、「どこを参照するか変更」の方ができなくなります。

<pre class="source" title="✖「どこを参照するか変更」と✔「参照先の値の変更」">
<span class="reserved">int</span> <span class="variable">x0</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// readonly フィールドはコンストラクターでしか初期化できないので引数で渡す。</span>
<span class="reserved">scoped</span> <span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>(<span class="reserved">ref</span> <span class="variable">x0</span>);

<span class="reserved">int</span> <span class="variable">x1</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="variable"><span class="error" title="CS0191">a</span><span class="operator">.</span><span class="field">X</span></span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x1</span>; <span class="comment">// エラー: どこを参照するかを変更不可。</span>

<span class="variable">a</span><span class="operator">.</span><span class="field">X</span> <span class="operator">=</span> <span class="number">2</span>; <span class="comment">// 参照先の値を変更はできる。</span>

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">public</span> <em><span class="reserved">readonly</span> <span class="reserved">ref</span></em> <span class="reserved">int</span> <span class="field">X</span>;
    <span class="reserved">public</span> <span class="type struct">A</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">X</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
}
</pre>

当然、両方の `readonly` を付けると両方不可です。

<pre class="source" title="✖「どこを参照するか変更」と✖「参照先の値の変更」">
<span class="reserved">int</span> <span class="variable">x0</span> <span class="operator">=</span> <span class="number">0</span>;

<span class="comment">// readonly フィールドはコンストラクターでしか初期化できないので引数で渡す。</span>
<span class="reserved">scoped</span> <span class="type struct">var</span> <span class="variable">a</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">A</span>(<span class="reserved">ref</span> <span class="variable">x0</span>);

<span class="reserved">int</span> <span class="variable">x1</span> <span class="operator">=</span> <span class="number">1</span>;
<span class="variable"><span class="error" title="CS0191">a</span><span class="operator">.</span><span class="field">X</span></span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x1</span>; <span class="comment">// エラー: どこを参照するかを変更不可。</span>

<span class="variable"><span class="error" title="CS8331">a</span><span class="operator">.</span><span class="field">X</span></span> <span class="operator">=</span> <span class="number">2</span>; <span class="comment">// エラー: 参照先の値を変更不可。</span>

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">A</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">ref</span> <span class="reserved">readonly</span> <span class="reserved">int</span> <span class="field">X</span>;
    <span class="reserved">public</span> <span class="type struct">A</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">X</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
}
</pre>

##<a id="sec-generated-title-10"></a> <a id="escape-analysis">エスケープ解析</a>
参照を使う上では、「漏らしてはいけないものを漏らさない」ということが必要になります。
簡単に言うと、メソッド内のローカル変数はメソッドを抜けると消えるので、
その参照は外に漏らしてはいけません。

<pre class="source" title="ローカル変数への参照は外に漏らせない">
<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method"><span class="static">M</span></span>()
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>; <span class="comment">// メソッド内の変数はメソッド抜けると消える。</span>
    <span class="control">return</span> <span class="reserved">ref</span> <span class="variable"><span class="error" title="CS8168">x</span></span>; <span class="comment">// エラー: 消えるものと外には漏らせない。</span>
}
</pre>

こういう「漏れている」状態を「エスケープ(escape: 脱走)している」と言います。

上記の例の場合は単純ですが、
参照変数などがあるため、間接的に何段も追いかける必要があります。

<pre class="source" title="エスケープ阻止のため、多段に追う必要あり">
<span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="static"><span class="method">M</span></span>()
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>; <span class="comment">// メソッド内の変数はメソッド抜けると消える。</span>
    <span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">x</span>;
    <span class="reserved">ref</span> <span class="reserved">var</span> <span class="variable">z</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable">y</span>;
    <span class="control">return</span> <span class="reserved">ref</span> <span class="variable"><span class="error" title="CS8157">z</span></span>; <span class="comment">// エラー: 間に2段挟まっているものの、元は x なので外に漏らせない。</span>
}
</pre>

このように、間に何段か挟まっていようと、大本をたどってエスケープを避ける処理を「<strong id="key-escape-analysis" class="keyword">エスケープ解析</strong>」(escape analysis)と呼びます。

C# 7.2 で ref 構造体が、
C# 11 で ref フィールドが入ったわけですが、
エスケープ解析はこれらも考慮する必要があります。

例えばわざとちょっと複雑なことをすると、以下のように、いろいろなところに参照が伝搬するコードが書けます。

<pre class="source" title="参照がいろんなところに伝搬する例">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>(<span class="reserved">out</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">result</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">var</span> <span class="variable">span</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;(<span class="reserved">ref</span> <span class="variable">x</span>); <span class="comment">// x が span から参照される状態。</span>
    <span class="reserved">scoped</span> <span class="reserved">var</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">R</span>();

    <span class="reserved">var</span> <span class="variable">ret</span> <span class="operator">=</span> <span class="variable">r</span><span class="operator">.</span><span class="method">M</span>(<span class="variable">span</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">y</span>); <span class="comment">// x がいろんなところに伝搬。</span>

    <span class="variable local">result</span> <span class="operator">=</span> <span class="error" title="CS8352"><span class="variable">r</span><span class="operator">.</span><span class="field">Span</span></span>; <span class="comment">// エラー: x が r.Span に伝搬してるかもしれないのでダメ。</span>
    <span class="variable local">result</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS8352">y</span></span>;      <span class="comment">// エラー: x が y に伝搬してるかもしれないのでダメ。</span>
    <span class="variable local">result</span> <span class="operator">=</span> <span class="error" title="CS8352"><span class="variable">ret</span></span>;    <span class="comment">// エラー: x が ret に伝搬してるかもしれないのでダメ。</span>
}

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">R</span>
{
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">Span</span>;

    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="method">M</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">x</span>, <span class="reserved">out</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>)
    {
        <span class="field">Span</span> <span class="operator">=</span> <span class="variable local">x</span>; <span class="comment">// フィールドにも、</span>
        <span class="variable local">y</span> <span class="operator">=</span> <span class="variable local">x</span>;    <span class="comment">// out 引数にも、</span>
        <span class="control">return</span> <span class="variable local">x</span>; <span class="comment">// 戻り値にも x (が持ってる参照)が伝搬。</span>
    }
}
</pre>

コスト度外視でよければ、
「どの引数・フィールドが、他のどの引数・フィールド・戻り値に伝搬するか」を事細かに指定することで厳密なエスケープ解析ができます。
(C# では採用しなかったため)仮定的なコードにはなりますが、
先ほどのコードを以下のように書けるようにするという案はなくはないです。

<pre class="source" title="(仮定的なコードで) 参照の伝搬をすべて明示">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>(<span class="reserved">out</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">result</span>)
{
    <span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
    <span class="reserved">var</span> <span class="variable">span1</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;(<span class="reserved">ref</span> <span class="variable">x</span>); <span class="comment">// x が span から参照される状態。</span>
    <span class="reserved">var</span> <span class="variable">span2</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">int</span>[<span class="number">1</span>];           <span class="comment">// こちらは配列を参照しているので外に漏らしても大丈夫。</span>

    <span class="reserved">var</span> <span class="variable">r</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">R</span> { <span class="field">Span</span> <span class="operator">=</span> <span class="variable">span1</span> };

    <span class="reserved">var</span> <span class="variable">ret</span> <span class="operator">=</span> <span class="variable">r</span><span class="operator">.</span><span class="method">M</span>(<span class="variable">span2</span>, <span class="reserved">out</span> <span class="reserved">var</span> <span class="variable">y</span>); <span class="comment">// span2 → y, span1 → r.Span → ret と伝搬。</span>

    <span class="variable local">result</span> <span class="operator">=</span> <span class="variable">y</span>;      <span class="comment">// 出どころが y → span2 → 配列 なので外に漏らして大丈夫。</span>
    <span class="variable local">result</span> <span class="operator">=</span> <span class="error"><span class="variable">ret</span></span>;    <span class="comment">// 出どころが ret → r.Span → span1 → x なのでダメ。</span>
}

<span class="comment">// 仮定的な文法: ` で、参照の伝搬先を表現。</span>
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">R</span>
{
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;</span><em>`A</em> <span class="field">Span</span>;

    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;</span><em>`A</em> <span class="method">M</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;</span><em>`B</em> <span class="variable local">x</span>, <span class="reserved">out</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;<em>`B</em> <span class="variable local">y</span>)
    {
        <span class="comment">// 伝搬先の指定が違うので、以下のコードはダメ。</span>
        <span class="comment">// Span = x;</span>
        <span class="comment">// return x;</span>
        <span class="variable local">y</span> <span class="operator">=</span> <span class="variable local">x</span>;       <span class="comment">// `B 間の伝搬は OK。</span>
        <span class="control">return</span> <span class="field">Span</span>; <span class="comment">// `A 間の伝搬は OK。</span>
    }
}
</pre>

###<a id="sec-generated-title-11"></a> <a id="scoped"></a><a id="scoped-modifier">scoped 修飾子</a>
ただ、ここまで細かい指定に需要があるかというと微妙です。
そこで C# 11 では、以下の2種類だけに絞ることにしました。

* scoped: どこにも漏らさない。メソッドの中でだけ使う。
* unscoped: どこかに漏らす。

ref 構造体(`Span<T>` など)に関しては実際にこの2択で、
何もつかなかった場合は unscoped 扱いで、`scoped` という新しい修飾子を付けると scoped 扱いになります。

一方で、`ref T` (`ref` 引数・`ref` 変数)に関しては、
既存コードを壊さないように、何もつけないと「引数から戻り値への伝搬だけ認める」(通称 return-only)というわかりにくいルールになっています。
そして、`UnscopedRef` 属性(`System.Diagnostics.CodeAnalysis` 名前空間)を付けると unscoped 扱い、
`scoped` 修飾子を付けると scoped 扱いになります。
(またちょっとややこしいことに、コンストラクターの引数の場合だけ、`ref T` でも unscoped 扱いみたいです。)

実際のコードを見てみましょう。
まず、何もつけない場合(`ref T` は return-only、ref 構造体は unscoped):

<pre class="source" title="何もつけない: ref T は return-only、ref 構造体は unscoped">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Default</span>
{
    <span class="reserved">private</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_y</span>;

    <span class="comment">// OK なやつ。</span>
    <span class="reserved">public</span> <span class="type struct">Default</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">ReturnRef</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">GetRef</span>() <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="field">_x</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseRef</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

    <span class="reserved">public</span> <span class="type struct">Default</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="method">ReturnSpan</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="method">GetSpan</span>() <span class="operator">=&gt;</span> <span class="field">_y</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetSpan</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseSpan</span>(<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) { }

    <span class="comment">// エラーになるやつ。</span>
    <span class="comment">// 引数 → フィールドへの伝搬だけ、ref T と Span&lt;T&gt; の挙動が違う。</span>
    <span class="comment">// ref T は「引数 → 戻り値 だけは OK」(return-only)。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetRef</span>(<span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="error" title="CS9079"><span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span></span>;
}
</pre>

続いて、`scoped` 修飾子を付けた場合(いずれも scoped 扱い)、たいていのものがダメになります:

<pre class="source" title="scoped 修飾子を付けた場合">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Scoped</span>
{
    <span class="reserved">private</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_y</span>;

    <span class="comment">// OK なやつ。</span>
    <span class="comment">// フィールドにも戻りにも伝搬しない場合だけ OK。</span>
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseRef</span>(<span class="reserved">scoped</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseSpan</span>(<span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) { }

    <span class="comment">// エラーになるやつ。</span>
    <span class="comment">// たいていダメ。</span>
    <span class="reserved">public</span> <span class="type struct">Scoped</span>(<span class="reserved">scoped</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="error" title="CS8374"><span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span></span>;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">ReturnRef</span>(<span class="reserved">scoped</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="variable local"><span class="error" title="CS9075">x</span></span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetRef</span>(<span class="reserved">scoped</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="error" title="CS8374"><span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span></span>;

    <span class="reserved">public</span> <span class="type struct">Scoped</span>(<span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS8352">y</span></span>;
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="method">ReturnSpan</span>(<span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="error" title="CS8352"><span class="variable local">y</span></span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetSpan</span>(<span class="reserved">scoped</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local"><span class="error" title="CS8352">y</span></span>;
}
</pre>

最後に、`UnscopedRef` 属性を付けた場合、たいていのものが OK になります
(ただし、ref 構造体は何も付けなくても unscoped 扱いなので、追加で属性を付けようとするとエラーになります):

<pre class="source" title="">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Unscoped</span>
{
    <span class="reserved">private</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="field">_x</span>;
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="field">_y</span>;

    <span class="comment">// OK なやつ。</span>
    <span class="comment">// UnscopedRef 属性を付けるとなんでも OK に。</span>
    <span class="comment">// (といっても差が出るのは SetRef だけ。)</span>
    <span class="reserved">public</span> <span class="type struct">Unscoped</span>([<span class="type">UnscopedRef</span>] <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">ReturnRef</span>([<span class="type">UnscopedRef</span>] <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetRef</span>([<span class="type">UnscopedRef</span>] <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="field">_x</span> <span class="operator">=</span> <span class="reserved">ref</span> <span class="variable local">x</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseRef</span>([<span class="type">UnscopedRef</span>] <span class="reserved">ref</span> <span class="reserved">int</span> <span class="variable local">x</span>) { }

    <span class="comment">// Span の方は「デフォルトで UnscopedRef だから属性付けるな」とエラーになる。</span>
    <span class="reserved">public</span> <span class="type struct">Unscoped</span>([<span class="type"><span class="error" title="CS9063">UnscopedRef</span></span>] <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="method">ReturnSpan</span>([<span class="type"><span class="error" title="CS9063">UnscopedRef</span></span>] <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">SetSpan</span>([<span class="error" title="CS9063"><span class="type">UnscopedRef</span></span>] <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="field">_y</span> <span class="operator">=</span> <span class="variable local">y</span>;
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">UseSpan</span>([<span class="type"><span class="error" title="CS9063">UnscopedRef</span></span>] <span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt; <span class="variable local">y</span>) { }
}
</pre>

###<a id="sec-generated-title-12"></a> <a id="caller">呼び出し元の挙動</a>
この手の機能は、
「メソッド内でできることを制限する代わりに、呼び出し元でできることを増やす」というものです。

例えば、unscoped (何も修飾子を付けていない ref 構造体)の場合、以下のように、
`Builder.Replace` の中で制限がない代わり、それを呼んでいる場所でのエラーが増えます。

<pre class="source" title="unscoped な挙動">
<span class="reserved">var</span> <span class="variable">builder</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Builder</span>();

<span class="static"><span class="method">Replace</span></span>(<span class="reserved">ref</span> <span class="variable">builder</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Replace</span></span>(<span class="reserved">ref</span> <span class="type struct">Builder</span> <span class="variable local">builder</span>)
{
    <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">newBuffer</span> <span class="operator">=</span> <span class="reserved">stackalloc</span> <span class="reserved">char</span>[<span class="number">3</span>];
    <span class="error" title="CS8350"><span class="variable local">builder</span><span class="operator">.</span><span class="method">Replace</span>(<span class="error" title="CS8352"><span class="variable">newBuffer</span></span>)</span>; <span class="comment">// ダメ。stackalloc したものが builder 越しに外に漏れる。</span>
}

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Builder</span>(<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">initialBuffer</span>)
{
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="field">_buffer</span> <span class="operator">=</span> <span class="variable local">initialBuffer</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Replace</span>(<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">value</span>)
    {
        <span class="comment">// 参照先自体を書き換え。</span>
        <span class="comment">// 引数からフィールドに参照が伝搬。</span>
        <span class="field">_buffer</span> <span class="operator">=</span> <span class="variable local">value</span>;
    }
}
</pre>

一方、scoped (`scoped` 修飾子を付けている)の場合、以下のように、
`Builder.Replace` の中で制限が掛かる代わり、それを呼んでいる場所でのエラーがなくなります。

<pre class="source" title="">
<span class="reserved">var</span> <span class="variable">builder</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">Builder</span>();

<span class="static"><span class="method">Append</span></span>(<span class="reserved">ref</span> <span class="variable">builder</span>);

<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">Append</span></span>(<span class="reserved">ref</span> <span class="type struct">Builder</span> <span class="variable local">builder</span>)
{
    <span class="type struct">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">buffer</span> <span class="operator">=</span> [<span class="number">0x61</span>, <span class="number">0x62</span>, <span class="number">0x63</span>];
    <span class="variable local">builder</span><span class="operator">.</span><span class="method">Append</span>(<span class="variable">buffer</span>); <span class="comment">// 同じようなことをしていてもこれは OK。</span>
}


<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Builder</span>(<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">initialBuffer</span>)
{
    <span class="reserved">private</span> <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="field">_buffer</span> <span class="operator">=</span> <span class="variable local">initialBuffer</span>;

    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Append</span>(<span class="reserved">scoped</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">utf8</span>)
    {
        <span class="comment">// 中身を書き換え。参照先自体は元のまま。</span>
        <span class="comment">// 引数の参照はどこにも漏らさない。</span>
        System<span class="operator">.</span>Text<span class="operator">.</span><span class="type">Encoding</span><span class="operator">.</span><span class="static"><span class="property">UTF8</span></span><span class="operator">.</span><span class="method">GetChars</span>(<span class="variable local">utf8</span>, <span class="field">_buffer</span>);
    }
}
</pre>

ちなみに、内部的には `scoped` 修飾子の方も属性で表現されています。
`scoped` 修飾子を付けた引数には `ScopedRef` 属性が付きます。
(ユーザーが自分の手でこの属性を付けることは認められていません。)

###<a id="sec-generated-title-13"></a> <a id="ref-this">構造体の this</a>
構造体の `this` は参照になっています。
この参照はデフォルトで scoped 扱いになっていて、外に漏らすことができません。

<pre class="source" title="this は scoped 扱い">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type struct">S</span> <span class="method">RefThis</span>() <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="reserved"><span class="error" title="CS8170">this</span></span>;

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">RefX</span>() <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="field"><span class="error" title="CS8170">_x</span></span>;
}
</pre>

この挙動を変えるのにも `UnscopedRef` 属性が使えます。
メソッド自身に `UnscopedRef` 属性を付けることで、`this` が unscoped 扱いになります。

<pre class="source" title="this を unscoped 扱いに変更">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="reserved">struct</span> <span class="type struct">S</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> <span class="field">_x</span>;

    [<span class="type">UnscopedRef</span>]
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type struct">S</span> <span class="method">RefThis</span>() <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="reserved">this</span>;

    [<span class="type">UnscopedRef</span>]
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="reserved">int</span> <span class="method">RefX</span>() <span class="operator">=&gt;</span> <span class="reserved">ref</span> <span class="field">_x</span>;
}
</pre>

##<a id="sec-generated-title-14"></a> <a id="ref-struct-interface">ref 構造体のインターフェイス実装</a>
<h5 class="version version13">Ver. 13</h5>

C# 13 で、ref 構造体にインターフェイスを実装できるようになりました。
例えば以下のようなコードを書いてもエラーを起こしません。

<pre class="source" title="ref 構造体にインターフェイスを実装する例">
<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="type">IFormattable</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="method">ToString</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">format</span>, <span class="type">IFormatProvider</span><span class="operator">?</span> <span class="variable local">formatProvider</span>) <span class="operator">=&gt;</span> <span class="string">&quot;&quot;</span>;
}
</pre>

ただ、前述の[「スタックのみ」制約](#stack-only)のせいで直接インターフェイス型の変数に代入することは C# 13 でもできません。
以下のコードは引き続きエラーになります。

<pre class="source" title="インターフェイスを実装できるようになったのに、インターフェイスに代入できない">
<span class="type">IFormattable</span> <span class="variable">f</span> <span class="operator">=</span> <span class="error" title="CS0029"><span class="reserved">new</span> <span class="type struct">S</span>()</span>;

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="type">IFormattable</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="method">ToString</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">format</span>, <span class="type">IFormatProvider</span><span class="operator">?</span> <span class="variable local">formatProvider</span>) <span class="operator">=&gt;</span> <span class="string">&quot;&quot;</span>;
}
</pre>

[ボックス化](rmboxing.md#boxing)を起こさないようにインターフェイス活用しようと思うと[ジェネリクス](../oop/sp2_generics.md)が必要になります。

<pre class="source" title="ジェネリクスでボックス化回避">
<span class="reserved">int</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>; <span class="comment">// int は IFormattable を実装してる。</span>

<span class="comment">// これはボックス化を起こす。</span>
<span class="type">IFormattable</span> <span class="variable">f</span> <span class="operator">=</span> <span class="variable">x</span>;
<span class="variable">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// ジェネリックメソッドを介して、</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">f</span>) <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IFormattable</span>
    <span class="operator">=&gt;</span> <span class="variable local">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// こうやって IFormattable.ToString を呼べばボックス化を回避できる。</span>
<span class="static"><span class="method">M</span></span>(<span class="variable">x</span>);
</pre>

したがって、この機能の肝は「ref 構造体をジェネリクスで使えるようにする」ということになります。

###<a id="sec-generated-title-15"></a> <a id="allows-ref-struct">allows ref struct</a>
ref 構造体に課せられている「ボックス化できない」などの制限は、C# のジェネリクスにとっては後付けなので、
そのままでは「ref 構造体の制限を満たしている」ということを保証できません。
例えば以下のコードは C# 2 以来ずっと合法なわけですが、
ボックス化を起こすコードなので ref 構造体に適しません。

<pre class="source" title="">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">f</span>) <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IFormattable</span>
{
    <span class="comment">// object に代入するとボックス化。</span>
    <span class="reserved">object</span> <span class="variable">o</span> <span class="operator">=</span> <span class="variable local">f</span>;

    <span class="comment">// WriteLine(object) なので、これも「object への変換」でボックス化。</span>
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable local">f</span>);

    <span class="comment">// 何ならインターフェイスへの代入でもボックス化。</span>
    <span class="type">IFormattable</span> <span class="variable">f1</span> <span class="operator">=</span> <span class="variable local">f</span>;
}

<span class="static"><span class="method">M</span></span>(<span class="number">123</span>);
</pre>

そこで C# 13 で、`allows ref struct` というものが追加されました。
型制約の `where` 句にこの条件を書くと、型引数に ref 構造体を渡せるようになります。

<pre class="source" title="">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;() <span class="reserved">where</span> <span class="type param">T</span> : <em><span class="reserved">allows</span> <span class="reserved">ref</span> <span class="reserved">struct</span></em>
{
}

<span class="comment">// これまで使えていた型は引き続き使える。</span>
<span class="static"><span class="method">M</span></span>&lt;<span class="reserved">string</span>&gt;();
<span class="static"><span class="method">M</span></span>&lt;<span class="reserved">int</span>&gt;();

<span class="comment">// これまで使えなかった ref 構造体にも使えるようになる。</span>
<span class="static"><span class="method">M</span></span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">int</span>&gt;&gt;();
<span class="static"><span class="method">M</span></span>&lt;<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt;&gt;();
</pre>

その代わり、`allows ref struct` を付けると、メソッド内でボックス化を起こすようなコードを書けなくなります。

<pre class="source" title="allows ref struct な型の変数はボックス化できない">
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;() <span class="reserved">where</span> <span class="type param">T</span> : <span class="reserved">allows</span> <span class="reserved">ref</span> <span class="reserved">struct</span>
{
    <span class="comment">// 先ほどのボックス化を起こすコードはすべてエラーに。</span>
    <span class="reserved">object</span> <span class="variable">o</span> <span class="operator">=</span> <span class="error" title="CS0103">f</span>;
    <span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="error" title="CS0103">f</span>);
    <span class="type">IFormattable</span> <span class="variable">f1</span> <span class="operator">=</span> <span class="error" title="CS0103">f</span>;
}
</pre>

ちなみに、通常の制約が「メソッド内でできることが増える代わりに、渡せる型が減る」というものなのに対して、
`allows ref struct` は「メソッド内でできることを減らす代わりに、渡せるが型が増える」ものになっていて、
これを「[アンチ制約](../oop/sp2_generics.md#anti-constraint)」と呼びます。

これで、ボックス化を起こさないようにインターフェイスのメンバーを呼べるようになったので、
ref 構造体のインターフェイス実装を活用できるようになります。

<pre class="source" title="allows ref struct なジェネリック メソッドを介して、ref 構造体のインターフェイス実装を呼ぶ">
<span class="type struct">S</span> <span class="variable">x</span> <span class="operator">=</span> <span class="reserved">new</span>(); <span class="comment">// S は IFormattable を実装してる。</span>

<span class="comment">// これはボックス化を起こすから C# 13 でもエラーになる。</span>
<span class="type">IFormattable</span> <span class="variable">f</span> <span class="operator">=</span> <span class="variable"><span class="error" title="CS0029">x</span></span>;
<span class="variable">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// allows ref struct なジェネリックメソッドを介して、</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method"><span class="static">M</span></span>&lt;<span class="type param">T</span>&gt;(<span class="type param">T</span> <span class="variable local">f</span>) <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">IFormattable</span>, <span class="reserved">allows</span> <span class="reserved">ref</span> <span class="reserved">struct</span>
    <span class="operator">=&gt;</span> <span class="variable local">f</span><span class="operator">.</span><span class="method">ToString</span>(<span class="string">&quot;X&quot;</span>, <span class="reserved">null</span>);

<span class="comment">// こうやって IFormattable.ToString を呼べば大丈夫になった。</span>
<span class="method"><span class="static">M</span></span>(<span class="variable">x</span>);

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="type">IFormattable</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> <span class="method">ToString</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">format</span>, <span class="type">IFormatProvider</span><span class="operator">?</span> <span class="variable local">formatProvider</span>) <span class="operator">=&gt;</span> <span class="string">&quot;&quot;</span>;
}
</pre>

####<a id="sec-generated-title-16"></a> <a id="bcl-allows-ref-struct">標準ライブラリ中の allows ref struct</a>
C# 13 で `allows ref struct` が追加されると同時に、
.NET 9 では、標準ライブラリ中のジェネリックなデリゲート型の大部分と、一部のインターフェイスの型引数に `allows ref struct` が付きました。
以下のようなコードが書けるようになっています。

<pre class="source" title="多くのデリゲート、一部のインターフェイスに allows ref struct">
<span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// 多くのデリゲートの型引数に allows ref struct が付いた。</span>
<span class="type">Action</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;&gt; <span class="variable">a</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="string">&quot;123&quot;</span><span class="operator">.</span><span class="method">TryCopyTo</span>(<span class="variable local">x</span>);
<span class="type">Func</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;, <span class="reserved">int</span>&gt; <span class="variable">b</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="method">IndexOf</span>(<span class="string">'1'</span>);
<span class="type">Predicate</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;&gt; <span class="variable">c</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="method">Contains</span>(<span class="string">'1'</span>);
<span class="type">Comparison</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;&gt; <span class="variable">d</span> <span class="operator">=</span> (<span class="variable local">x</span>, <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span><span class="operator">.</span><span class="method">SequenceCompareTo</span>(<span class="variable local">y</span>);
<span class="type">Converter</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;, <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt;&gt; <span class="variable">e</span> <span class="operator">=</span> <span class="variable local">x</span> <span class="operator">=&gt;</span> <span class="variable local">x</span>;

<span class="comment">// 比較系のインターフェイスには大体 allows ref struct が付いた。</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">IComparer</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;&gt;, <span class="type">IEqualityComparer</span>&lt;<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt;&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">Compare</span>(<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">x</span>, <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">x</span>, <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">y</span>) <span class="operator">=&gt;</span> <span class="reserved">true</span>;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">GetHashCode</span>([<span class="type">DisallowNull</span>] <span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">obj</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
}

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">S</span> : <span class="type">IEquatable</span>&lt;<span class="type struct">S</span>&gt;, <span class="type">IComparable</span>&lt;<span class="type struct">S</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">CompareTo</span>(<span class="type struct">S</span> <span class="variable local">other</span>) <span class="operator">=&gt;</span> <span class="number">0</span>;
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type struct">S</span> <span class="variable local">other</span>) <span class="operator">=&gt;</span> <span class="reserved">true</span>;
}
</pre>

#####<a id="sec-generated-title-17"></a> <a id="ref-struct-delegate">余談: ref 構造体引数のデリゲートの自然な型</a>
C# 10 の頃にデリゲートに[自然な型](../functional/sp_delegate.md#natural-type)が入りましたが、
「可能であれば `Action`、`Action<T>`、`Func<T>` を使う」という仕様になっています。
これに対して、.NET 9 でこれらのデリゲートに `allows ref strcut` が付いたことで、「可能であれば」の範囲が広がっています。
これまでだと匿名のデリゲート型になっていたものが、`Action` や `Func` に変わることがあります。

<pre class="source" title=".NET 8 から 9 で型が変わる例">
<span class="reserved">var</span> <span class="variable">a</span> <span class="operator">=</span> (<span class="type struct">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">s</span>) <span class="operator">=&gt;</span> { };

<span class="comment">// .NET 8 以前だと: &lt;&gt;f__AnonymousDelegate0</span>
<span class="comment">// .NET 9 以降だと: Action`1</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="variable">a</span><span class="operator">.</span><span class="method">GetType</span>()<span class="operator">.</span><span class="property">Name</span>);
</pre>

####<a id="sec-generated-title-18"></a> <a id="ienumerable-not-allow">余談: IEnumerable 問題</a>
ref 構造体がらみで非常に多い要望の1つに、`Span<T>`、`ReadOnlySpan<T>` に対して LINQ を使いたいというものがあります。
しかし、ref 構造体にインターフェイスを実装できるようになっても、`Span<T>` に `IEnumerable<T>` は実装できなくて、この要望はかないません。
問題は、以下のように、`IEnumerator<T>` インターフェイスを戻り値に返す部分が ref 構造体と合いません。

<pre class="source" title="ref 構造体は IEnumerable と相性がよくない">
<span class="reserved">using</span> System<span class="operator">.</span>Collections;

<span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type param">T</span>&gt;
{
    <span class="comment">// res 構造体に IEnumerator を実装するのは可能。</span>
    <span class="reserved">ref</span> <span class="reserved">struct</span> <span class="type struct">Enumerator</span>(<span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="variable local">span</span>) : <span class="type">IEnumerator</span>&lt;<span class="type param">T</span>&gt;
    {
        <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type struct">Span</span>&lt;<span class="type param">T</span>&gt; <span class="field">_span</span> <span class="operator">=</span> <span class="variable local">span</span>;
        <span class="reserved">public</span> <span class="type param">T</span> <span class="property">Current</span> <span class="operator">=&gt;</span> <span class="reserved">default</span><span class="operator">!</span>;
        <span class="reserved">object</span> <span class="type">IEnumerator</span><span class="operator">.</span><span class="property">Current</span> <span class="operator">=&gt;</span> <span class="reserved">null</span><span class="operator">!</span>;
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Dispose</span>() { }
        <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">MoveNext</span>() <span class="operator">=&gt;</span> <span class="reserved">false</span>;
        <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Reset</span>() { }
    }

    <span class="comment">// 問題はここ。</span>
    <span class="comment">// (ジェネリックを介さず) 直接 IEnumerator&lt;T&gt; インターフェイスを返す必要があって、ref 構造体に合わない。</span>
    <span class="reserved">public</span> <span class="type">IEnumerator</span>&lt;<span class="type param">T</span>&gt; <span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="error" title="CS0029"><span class="reserved">new</span> <span class="type struct">Enumerator</span>(<span class="reserved">this</span>)</span>;
    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span><span class="operator">.</span><span class="method">GetEnumerator</span>() <span class="operator">=&gt;</span> <span class="method">GetEnumerator</span>();
}
</pre>

`IEnumerator<T>` の方であれば問題なく実装できるので、`IEnumerator<T>` 版の LINQ を用意した方がいいのかという話題も出ていたりします。
