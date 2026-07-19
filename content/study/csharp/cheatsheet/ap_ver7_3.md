---
title: "C# 7.3 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver7_3/"
content_type: "Article"
published_at: "2018-04-14T00:00:00"
updated_at: "2025-01-01T18:46:38"
tags: []
umbraco_id: 2146
parent_id: 1174
sort_order: 12
aliases:
  - "/csharp/cheatsheet/ap_ver7_3/"
---

# C# 7.3 の新機能

<div class="version version7_1">Ver. 7.3</div>

<table>
<tr>
<th>リリース時期</th>
<td>2018/5</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2017 15.7</li>
<li>.NET Core 2.1</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>C# 7.0～7.2のちょっとした改善</li>
</ul>
</td>
</tr>
</table>

C# 7.0 以降の「小数点リリース」も3つ目となりました。
これまでのC# 7系リリースで追加されてきた、
[タプル](ap_ver7.md#tuple)や[構造体と参照の活用](ap_ver7_2.md#ref)、[式中での変数宣言](ap_ver7.md#var-expressions)になどに関する改善が含まれています。

## <a id="sec-generated-title-1"></a> <a id="tuple-equality"></a>タプルの ==, != 比較

タプル同士を `==`、`!=` 演算子で比較できるようになりました。
以下のように、メンバーごとの`==`を[`&&`](../start/st_operator.md#short-circuit)で繋いだものに展開されます。

<pre class="source" title="タプル ==">
<code><span class="reserved">void</span> M((<span class="reserved">int</span> a, (<span class="reserved">int</span> x, <span class="reserved">int</span> y) b) t)
{
    <span class="comment">// このタプル == 比較は、</span>
    <span class="type">Console</span>.WriteLine(t == (1, (2, 3)));
    <span class="comment">// こんな感じで、メンバーごとの == を &amp;&amp; で繋いだものに展開される。</span>
    <span class="type">Console</span>.WriteLine(t.a == 1 &amp;&amp; t.b.x == 2 &amp;&amp; t.b.y == 3);
}
</code></pre>

詳しくは「[==、!= での比較](../datatype/tuples.md#equality)」で説明します。

## <a id="sec-generated-title-2"></a> <a id="ref-reassignment"></a>ref 再代入

参照引数、参照ローカル変数に対して、
参照先の値の書き換えではなく、「どこを参照しているか」自体を書き換えることができるようになりました。

<pre class="source" title="ref 再代入">
<code><span class="reserved">int</span> x = 1;
<span class="reserved">int</span> y = 2;

<span class="comment">// x を参照。</span>
<span class="reserved">ref</span> var r = <span class="reserved">ref</span> x;

<span class="comment">// このとき、r に対する代入は x に反映される。</span>
r = 10; <span class="comment">// x が 10 になる。</span>

<span class="comment">// これが ref 再代入。</span>
<span class="comment">// r が y を参照するようになる。</span>
r = <span class="reserved"><em>ref</em></span> y;

<span class="comment">// 今度は、r に対する代入が y に反映される。</span>
r = 20; <span class="comment">// y が 20 になる。</span>

<span class="type">Console</span>.WriteLine((x, y)); <span class="comment">// (10, 20)</span>
</code></pre>

また、同時に、`for`ステートメントと`foreach`ステートメントのループ変数を参照ローカル変数にできるようになりました。

詳しくは
「[ref再代入](../resource/sp_ref.md#ref-reassignment)」、
「[for/foreach のループ変数を参照に](../resource/sp_ref.md#ref-for)」で説明します。


## <a id="sec-generated-title-3"></a> <a id="var-expressions"></a>式中での変数宣言(使える場所の拡充)

C# 7.0から式中で、
[is 演算子](../datatype/typeswitch.md#is)や[出力変数宣言](../resource/sp_ref.md#out-var)を使って、
式中でも変数宣言できるようになりましたが、
いくつか制限がありました。
C# 7.3で、これまではできなかった以下の個所でも変数宣言ができるようにありました。

- [クエリ式](../start/st_scope.md#query-expression)
- [初期化子](../start/st_scope.md#initializer)

<pre class="source" title="クエリ式中での変数宣言">
<code><span class="reserved">var</span> q =
    <span class="reserved">from</span> s <span class="reserved">in</span> <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"abc"</span>, <span class="string">"112"</span>, <span class="string">"132"</span>, <span class="string">"451"</span>, <span class="reserved">null</span> }
    <span class="reserved">where</span> s <span class="reserved">is</span> <span class="reserved">string</span> <em>x</em> &amp;&amp; x.Length &gt; 1
    <span class="reserved">where</span> <span class="reserved">int</span>.TryParse(s, <span class="reserved">out var</span> <em>x</em>) &amp;&amp; (x % 3) == 0
    <span class="reserved">select</span> s;
</code></pre>

<pre class="source" title="初期化子内での変数宣言">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Derived</span> : <span class="type">base</span>
{
    <span class="reserved">public</span> Derived(<span class="reserved">string</span> s) : <span class="reserved">this</span>(<span class="reserved">int</span>.TryParse(s, <span class="reserved">out var</span> <em>x</em>) ? x : -1)
    {
        <span class="comment">// コンストラクター初期化子中で宣言した x は、コンストラクター本体内で利用可能。</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="reserved">public</span> Derived(<span class="reserved">int</span> a) : <span class="reserved">base</span>(<span class="reserved">out var</span> <em>x</em>)
    {
        <span class="comment">// base の場合でも同様。</span>
        <span class="type">Console</span>.WriteLine(x);
    }

    <span class="comment">// フィールド初期化子、プロパティ初期化子中で宣言した x は、その初期化子内でのみ有効。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Field = <span class="reserved">int</span>.TryParse(<span class="string">"123"</span>, <span class="reserved">out var</span> <em>x</em>) ? x : -1;
    <span class="reserved">public</span> <span class="reserved">int</span> Property{ <span class="reserved">get</span>; <span class="reserved">set</span>; } = <span class="reserved">int</span>.TryParse(<span class="string">"123"</span>, <span class="reserved">out var</span> <em>x</em>) ? x : -1;
}
</code></pre>

詳しくは「[C# 7での新しいスコープ ルール](../start/st_scope.md#csharp7)」で説明します。

## <a id="sec-generated-title-4"></a> <a id="constraints"></a>ジェネリック型引数に対する Enum、Delegate、unmanaged 制約

3つほど指定できる制約が増えました。

<table summary="型引数に対する制約条件(C# 7.2まで)">
	<caption>
		型引数に対する制約条件
	</caption>
	<tr>
		<th>制約の与え方</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1"><code>where T : unmanaged</code></td>
		<td markdown="1">型<code>T</code>は「[アンマネージ型](../interop/sp_unsafe.md#unmanaged-types)」である</td>
	</tr>
	<tr>
		<td markdown="1"><code>where T : Enum</code></td>
		<td markdown="1">型<code>T</code>は「[列挙型](../structured/st_enum.md)」である</td>
	</tr>
	<tr>
		<td markdown="1"><code>where T : Delegate</code></td>
		<td markdown="1">型<code>T</code>は「[デリゲート型](../functional/sp_delegate.md)」である</td>
	</tr>
</table>

詳しくは「[ジェネリック](../oop/sp2_generics.md#cs7.3)」、
「[unsafe](../interop/sp_unsafe.md#unmanaged-constraints)」、
「[[余談] 暗黙的な派生](../oop/miscimplictinherit.md#constraints)」などで説明します。

## <a id="sec-generated-title-5"></a> <a id="overload-resolution"></a>オーバーロード解決の改善

オーバーロード解決が少し賢くなって、
これまでは呼び分けできなかったようなオーバーロードを呼び分けれるようになりました。

以下のようなものがあります。

- 静的メソッドかインスタンス メソッドかの違いで解決できるようになった
- ジェネリック型制約の違いで解決できるようになった
- &nbsp; [メソッド グループ](../structured/st_function.md#key-method-group)を引数にするとき、メソッドの戻り値を見るようになった

例えば、型制約だと、以下のような拡張メソッドの呼び分けができるようになりました。

<pre class="source" title="class 制約と struct 制約の呼び分け">
<code><span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.Linq;

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">ClassExtensions</span>
{
    <span class="comment">// クラスの場合は LINQ の FirstOrDefault そのまま。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span> FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
        =&gt; source.FirstOrDefault();
}

<span class="reserved">static</span> <span class="reserved">class</span> <span class="type">StructExtensions</span>
{
    <span class="comment">// 構造体の場合は null 許容型に変える必要がある。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span>? FirstOrNull&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt; source)
        <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>
        =&gt; source.Select(x =&gt; (<span class="type">T</span>?)x).FirstOrDefault();
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="comment">// ClassExtensions の方のが呼ばれる。</span>
        <span class="reserved">new</span>[] { <span class="string">"a"</span>, <span class="string">"b"</span>, <span class="string">"c"</span> }.FirstOrNull();

        <span class="comment">// StructExtensions の方のが呼ばれる。</span>
        <span class="reserved">new</span>[] { 1, 2, 3 }.FirstOrNull();
    }
}
</code></pre>

詳しくは「[[雑記]オーバーロード解決](../structured/miscoverloadresolution.md)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="stackalloc-initializer"></a>stackalloc 初期化子

`stackalloc`に対して、配列と同じような初期化子を使えるようになりました。
配列同様、初期化子中の要素の型からの推論も効きます。

<pre class="source" title="">
<code><span class="comment">// 初期化子。{ } を使って初期値を与えられる。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x1 = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[3] { 0xEF, 0xBB, 0xBF };

<span class="comment">// 初期化子があるとき、サイズは省略可能。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x2 = <span class="reserved">stackalloc</span> <span class="reserved">int</span>[] { 0xEF, 0xBB, 0xBF };

<span class="comment">// 初期化子から推論できるときは型名も省略可能。</span>
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; x3 = <span class="reserved">stackalloc</span>[] { 0xEF, 0xBB, 0xBF };
</code></pre>

## <a id="sec-generated-title-7"></a> <a id="custom-fixed"></a>ユーザー定義型の fixed ステートメント利用

所定のパターンを満たす型に対して `fixed` ステートメントが使えるようになりました。
以下のように、`GetPinnableReference`という名前のメソッドを用意すれば使えます。

<pre class="source" title="ユーザー定義型に対する fixed ステートメント">
<code><span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Array</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">T</span>[] _array;
    <span class="reserved">public</span> Array(<span class="reserved">int</span> length) =&gt; _array = <span class="reserved">new</span> <span class="type">T</span>[length];
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _array[index];
    <span class="reserved">public</span> <span class="reserved">int</span> Length =&gt; _array.Length;

    <span class="comment">// このメソッドがあれば fixed ステートメントを使えるようになる</span>
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> GetPinnableReference() =&gt; <span class="reserved">ref</span> _array[0];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="type">Array</span>&lt;<span class="reserved">int</span>&gt;(5);

        <span class="reserved">unsafe</span>
        {
            <span class="comment">// fixed (int* p = &amp;a.GetPinnableReference()) に展開される。</span>
            <span class="reserved">fixed</span> (<span class="reserved">int</span>* p = a)
            {
                <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
                    p[i] = i;
            }
        }

        <span class="reserved">for</span> (<span class="reserved">int</span> i = 0; i &lt; 5; i++)
            System.<span class="type">Console</span>.WriteLine(a[i]);
    }
}
</code></pre>

詳しくは「[ユーザー定義型の fixed ステートメント利用](../interop/sp_unsafe.md#custom-fixed)」で説明します。

## <a id="sec-generated-title-8"></a> <a id="others"></a>その他

その他、ほぼ「バグ修正」レベルの改善が2点あります。

### <a id="sec-generated-title-9"></a> <a id="field-attribute-on-auto-property"></a>自動プロパティのバック フィールドに対する field 属性指定

前者は、[自動プロパティ](../oop/oo_property.md#auto)に対して `field` 指定の属性が付けられるようになりました。

<pre class="source" title="自動プロパティが内部的に生成しているフィールドへの属性付け">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">XAttribute</span> : <span class="type">Attribute</span> { }

<span class="reserved">class</span> <span class="type">Sample</span>
{
    [<span class="reserved">field</span>:<span class="type">X</span>] <span class="comment">// 自動実装で生成されるフィールドに対する属性の指定</span>
    <span class="reserved">public</span> <span class="reserved">int</span> AutoProperty { <span class="reserved">get</span>; }
</code></pre>

詳しくは「[プロパティ、イベントと属性の対象](../dynamic/sp_attribute.md#auto-impl)」で説明します。

### <a id="sec-generated-title-10"></a> <a id="movable-fixed-buffer"></a>固定長バッファーの読み書きで、fixed ステートメント不要に

[固定長バッファー](../interop/sp_unsafe.md#fixed-buffer)の読み書きをする際、
[`fixed`ステートメント](../interop/sp_unsafe.md#fixed)が不要になる場面が増えたそうです。

<pre class="source" title="fixed なしで固定長バッファーの読み書き">
<code><span class="reserved">unsafe</span> <span class="reserved">struct</span> <span class="type">Buffer</span>
{
    <span class="reserved">public</span> <span class="reserved">fixed</span> <span class="reserved">byte</span> A[8];
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="type">Buffer</span> _buffer;

    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> buffer = <span class="reserved">new</span> <span class="type">Buffer</span>();
        buffer.A[0] = 1; <span class="comment">// 元々 OK</span>
        _buffer.A[0] = 2; <span class="comment">// C# 7.3 から OK</span>

        RefFixedBuffer(<span class="reserved">ref</span> buffer);

        System.<span class="type">Console</span>.WriteLine(buffer.A[0]);  <span class="comment">// 元々 OK</span>
        System.<span class="type">Console</span>.WriteLine(_buffer.A[0]); <span class="comment">// C# 7.3 から OK</span>
    }

    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> RefFixedBuffer(<span class="reserved">ref</span> <span class="type">Buffer</span> buffer)
    {
        buffer.A[1] = 3; <span class="comment">// C# 7.3 から OK</span>
    }
}
</code></pre>

[提案文書](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-7.3/indexing-movable-fixed-fields.md)にすら、「言語仕様上どうしてこの条件緩和が許されるのかを説明するのが難しい」とか書かれる始末な機能です…

本来はポインター操作になるので`fixed`ステートメントが必須なんですが、
C# コンパイラー的には[参照ローカル変数](../resource/sp_ref.md#ref-returns)と同じようなコード生成するらしく、
だったら`fixed`がなくても平気なはず、と言うことらしいです。
