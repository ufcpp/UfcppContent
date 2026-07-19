---
title: "特殊な文字列リテラル"
source_url: "https://ufcpp.net/study/csharp/start/st_string/"
content_type: "Article"
published_at: "2014-10-06T00:00:00"
updated_at: "2021-09-18T00:00:00"
tags:
  - "Ver. 6.0"
umbraco_id: 1202
parent_id: 1190
sort_order: 9
aliases:
  - "/csharp/st_string"
  - "/csharp/st_string.html"
  - "/csharp/start/st_string/"
  - "/study/csharp/st_string"
  - "/study/csharp/st_string.html"
---

# 特殊な文字列リテラル

## <a id="sec-generated-title-1"></a> <a id="abst">概要</a>

<h5 class="version version6">Ver. 6</h5>

C# 6 で、補間文字列と、nameof 演算子(nameof operator)という、2つの文字列関連機能が追加されました。

また、C# 11 で、生文字列リテラルという構文が追加されました。

## <a id="sec-generated-title-2"></a> <a id="string-interpolation">文字列補間</a>

クラスのメンバーを整形して文字列化するには、.NETでは<code>string</code>の<code>Format</code>メソッドを使います。

<pre class="source" title="" lang="">
<code><span class="reserved">var</span> formatted = <span class="reserved">string</span>.Format(<span class="literal">"({0}, {1})"</span>, x, y);
</code></pre>


<figure>
	[![string.Format メソッドの利用例](../../../../assets/media/ufcpp2000/csharp/fig/string-format.png)](../../../../assets/media/ufcpp2000/csharp/fig/string-format.png)
	<figcaption>string.Format メソッドの利用例</figcaption>
</figure>


しかし、Formatメソッドには、以下のような面倒事がありました。

* 頻出するわりに、string.Format という長めのタイピングが面倒

* 値を埋め込みたい場所と、埋め込む値を渡す場所が離れて読みにくい

* {0}とかの数と、渡す値の数が違っていても実行して見るまで気付かない


そこで、以下のような、Format用の専用構文が追加されました。

<pre class="source" title="文字列補間の例" lang="">
<code><span class="reserved">var</span> formatted = <span class="literal">$"({</span>x<span class="literal">}, {</span>y<span class="literal">})"</span>;
</code></pre>

このような書き方を<strong id="key-interpolated-string" class="keyword">補間文字列</strong>(interpolated string)、もしくは、<em>文字列補間</em>(string interpolation)といいます。
文字列補間の結果は、単純に `string.Format` メソッドの呼び出しに置き替えられます。
例えば、最初の例は以下のコードと同じ意味なります。

<pre class="source" title="文字列補間の展開結果" lang="">
<code><span class="reserved">var</span> formatted = <span class="reserved">string</span>.Format(<span class="literal">"({0}, {1})"</span>, x, y);
</code></pre>

### <a id="sec-generated-title-3"></a> <a id="csharp10-improvement">C# 10 でのパフォーマンス改善</a>

<h5 class="version version10">Ver. 10</h5>

`string.Format` を使った実装ではどうしてもパフォーマンス上の改善が難しく、
C# 10.0 では別の型を使って結構複雑なコードに変換する最適化が入りました。
条件を満たす場合、

<pre class="source" title="文字列補間の例">
<code><span class="reserved">var</span> formatted = <span class="literal">$"({</span>x<span class="literal">}, {</span>y<span class="literal">})"</span>;
</code></pre>

このコードは `string.Format` ではなく、以下のようなコードに展開されます。

<pre class="source" title="C# 10.0 での文字列補間の展開結果の例">
<code><span class="type">DefaultInterpolatedStringHandler</span> handler = <span class="reserved">new</span> <span class="type">DefaultInterpolatedStringHandler</span>(4, 2);
handler.<span class="method">AppendLiteral</span>(<span class="string">"("</span>);
handler.<span class="method">AppendFormatted</span>(x);
handler.<span class="method">AppendLiteral</span>(<span class="string">", "</span>);
handler.<span class="method">AppendFormatted</span>(y);
handler.<span class="method">AppendLiteral</span>(<span class="string">")"</span>);
<span class="reserved">string</span> s = handler.<span class="method">ToStringAndClear</span>();
</code></pre>

詳細な条件については「[C# 10.0 の補間文字列の改善](improvedinterpolatedstring.md)」で別途説明します。

とりあえず、簡単な条件としては、実行環境を .NET 6 以上(TargetFramework を net6.0 以上)にして再コンパイルするだけで文字列補間のパフォーマンスが上がると思ってください。

また、C# 10.0 ではこれと同時に、[一定の条件を満たす場合、文字列補間を const にできるようになりました](sp_const.md#constant-string-interpolation)。

### <a id="sec-generated-title-4"></a> <a id="escape">エスケープ</a>

エスケープ(`$""` の中で本来使えない文字を埋め込む方法)の方法は[通常の文字列](st_embeddedtype.md#escape-sequence)とほぼ同じです。
通常の文字列リテラルと同じく、`\` に続けることで、`"`記号(`\"`)や改行文字(`\n`)などが書けます。

少しだけ違うのは、`$""` の中では `{` や `}` も特別な意味を持っているので、これらに対するエスケープが別途必要になります。`{` や `}` は2つ重ねて`{{` や `}}` 書くことで、補間の意味ではなく、その場所に波括弧を表示する意味になります。

<pre class="source" title="エスケープ">
<code><reserved></span><span class="reserved">var</span> p = <span class="reserved">new</span> { X = 10, Y = 20 };
<span class="type">Console</span>.WriteLine(<span class="string">$"\"{{</span>{p.X}<span class="string">, </span>{p.Y}<span class="string">}}\""</span>);
</code></pre>

<pre class="console" title="エスケープ">
<code>"{10, 20}"
</code></pre>

### <a id="sec-generated-title-5"></a> <a id="formatting">書式指定</a>

書式指定もできます。

<pre class="source" title="文字列補間での書式指定" lang="">
<code><reserved></span><span class="reserved">var</span> formatted = <span class="string">$"(</span>{12300:<span class="string">c</span>}<span class="string">, </span>{12300:<span class="string">n</span>}<span class="string">, </span>{12300,4:<span class="string">x</span>}<span class="string">)"</span>;
</code></pre>

書式の書き方も`string.Format`に対して使えるものと同じです。

ただ、C#の構文化したことで、元々実行してみるまでエラーがわからなかったのが、コンパイル時に検出できるようになったりしています。

<pre class="source" title="">
<code><comment></span><span class="comment">// ほぼ同じ意味</span>
<span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Format(<span class="string">"{0,4:x}"</span>, x));
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x,4:<span class="string">x</span>}<span class="string">"</span>);

<span class="comment">// 書き方を忘れて、 , と : を間違えてしまうと…</span>

<span class="comment">// 実行時エラー</span>
<span class="type">Console</span>.WriteLine(<span class="reserved">string</span>.Format(<span class="string">"{0,x}"</span>, x));

<span class="comment">// コンパイル エラー</span>
<span class="type">Console</span>.WriteLine(<span class="string">$"</span>{x,x}<span class="string">"</span>);
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="conditional-in-string-interpolation">文字列補間と条件演算子</a>

`{}`の中には割と任意の式を書けます。
たとえば、以下のように、メソッドを呼び出したり、`{}`の中にさらに文字列リテラル`""`を含めることもできます。

<pre class="source" title="{} 内には割と任意の式を書ける">
<code><reserved></span><span class="reserved">var</span> data = <span class="reserved">new</span>[] { 1, 2, 3 };
<span class="reserved">var</span> s = <span class="string">$"</span>{<span class="reserved">string</span>.Join(<span class="string">", "</span>, data)}<span class="string"> =&gt; </span>{<span class="reserved">string</span>.Join(<span class="string">", "</span>, data.Select(i =&gt; i * i))}<span class="string">"</span>;
</code></pre>

ただ、1つだけ制限があって、条件演算子 `?:`は、`{}`中に直接書くことができません。
たとえば以下のコードでは、1行目(`s1`の行)がコンパイルエラーになります。

<pre class="source" title="">
<code><reserved></span><span class="reserved">var</span> s1 = <span class="string">$"p = </span>{p == <span class="reserved">null</span> ? <span class="string">"null"</span> :<span class="string"> p.ToString()</span>}<span class="string">"</span>; <span class="comment">// エラー</span>
<span class="reserved">var</span> s2 = <span class="string">$"p = </span>{(p == <span class="reserved">null</span> ? <span class="string">"null"</span> : p.ToString())}<span class="string">"</span>; <span class="comment">// 1段 () でくくればOK</span>
</code></pre>

前節の書式指定の `:` と認識されて、「書式エラー」になります。
(「`?`がある時だけ`:`の解釈を変える」というのが高コストすぎるそうで、こういう仕様になっています。)
一応、`s2`の行のように、1段階 `()`でくくればコンパイルできるようになります。

### <a id="sec-generated-title-7"></a> <a id="multi-line">複数行の文字列補間</a>

また、`$@` から始めることで、複数行の文字列補間もできます。

<pre class="source" title="複数行の文字列補間" lang="">
<code><span class="reserved">var</span> verbatim = <span class="literal">$@"
verbatim (here) string
{</span>x<span class="literal">}, {</span>y<span class="literal">}, {</span>x<span class="literal">:c}, {</span>x<span class="literal">:n}
"</span>;
</code></pre>

ちなみに、逆順、つまり、`@$`は、C# 8.0 以降でだけ使えます(C# 7.3 以前だとコンパイル エラーになります)。

<pre class="source" title="コンパイル エラー" lang="">
<code><span class="comment">// これは C# 7.3 以前ではコンパイル エラーになる</span>
<span class="reserved">var</span> verbatim = <span class="literal">@$"</span>
verbatim (here) string
{x}, {y}, {x:c}, {x:n}
";
</code></pre>

また、`$@`を使った場合、エスケープのルールは[逐語的文字列リテラル](st_embeddedtype.md#verbatim-string)と同じになります。
すなわち、`"` と書きたければ `""`と、ダブルクォーテーションを2つ重ねます。また、`\`から始めるエスケープはできません(`\`記号がそのまま表示される)。

<pre class="source" title="複数行文字列補間でのエスケープ">
<code><span class="type">Console</span>.WriteLine(<span class="literal">$@"
""
{{
{<span class="literal">p.X</span>}\{<span class="literal">p.Y</span>}
}}
""
"</span>);
</code></pre>

<pre class="console" title="複数行文字列補間でのエスケープ">
<code>
"
{
10\20
}
"
</code></pre>

### <a id="sec-generated-title-8"></a> <a id="FormatableString"></a><a id="FormattableString">FormattableString</a>

ちなみに、`Format`メソッドには、`IFormatProvider` インターフェイス(`System`名前空間)を与える(カルチャーなどの指定ができる)オーバーロードがあります(参考: 「[書式とカルチャー](../../dotnet/bcl/bcl_format.md#culture)」)。

C# 6 では、文字列補間機能を使いつつ、`IFormatProvider` を与える方法もちゃんと提供されます。
文字列補間でカルチャー指定するには、これから説明する `FormattableString` という型(`System`名前空間)を介します。

文字列補間構文では、以下のように、`IFormattable` インターフェイス(`System`名前空間)に代入すると、
一旦 `FormattableString` クラス(`System` 名前空間)のインスタンスが作られます。
(左辺の型を見て決定。右辺の書き方は直接文字列に整形する場合とまったく同じ。)

<pre class="source" title="" lang="">
<code><span class="comment">// 左辺の型が IFormattable の時、文字列補間の結果は string ではなく、FormattableString になる</span>
System.<span class="type">IFormattable</span> formatable = <span class="literal">$"({</span>x<span class="literal">}, {</span>y<span class="literal">})"</span>;
</code></pre>


`IFormattable` の `ToString` メソッドには、`IFormatProvider` を与えることで、整形の仕方を調整できます。

<pre class="source" title="FormattableString に対する書式プロバイダー指定" lang="">
<code><span class="type">IFormattable</span> f = <span class="literal">$"</span>{x :<span class="literal">c</span>}<span class="literal">, </span>{x :<span class="literal">n</span>}<span class="literal">"</span>;
<span class="type">Console</span>.WriteLine(f.ToString(<span class="reserved">null</span>, <span class="reserved">new</span> System.Globalization.<span class="type">CultureInfo</span>(<span class="literal">"en-us"</span>)));
</code></pre>


ちなみに、こちらは、`FormattableStringFactory` クラス(`System.Runtime.CompilerServices` 名前空間)の `Create` メソッド呼び出しに変換されます。

<pre class="source" title="" lang="">
<code>System.<span class="type">IFormattable</span> formatable = System.Runtime.CompilerServices.<span class="type">FormattableStringFactory</span>.Create(<span class="literal">"({0}, {1})", x, y</span>;
</code></pre>

### <a id="sec-generated-title-9"></a> <a id="FormattableString-overload">FormattableString のオーバーロード解決</a>

`string` 引数と `FormattableString` 引数のオーバーロードがあるとき、
`$""` リテラルを渡すと、常に `string` の方が優先されます。

例えば以下のようなメソッドを考えます。

<pre class="source" title="string と FormattableString のオーバーロード">
<code><span class="comment">// string が優先されるので、M1($&quot;&quot;) という書き方では呼び分けできない。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;string: &quot;</span> + <span class="variable">s</span>);
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M1</span>(<span class="type">FormattableString</span> <span class="variable">s</span>) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">$&quot;format: </span>{<span class="variable">s</span>.Format}<span class="string">, args: </span>{<span class="reserved">string</span>.<span class="method">Join</span>(<span class="string">&quot;, &quot;</span>, <span class="variable">s</span>.<span class="method">GetArguments</span>())}<span class="string">&quot;</span>);
</code></pre>

このとき、`M1($"")` という書き方では `M1(string)` の方が呼ばれてしまいます。

<pre class="source" title="string 優先">
<code><span class="comment">// string の方が呼ばれる</span>
<span class="method">M1</span>(<span class="string">&quot;&quot;</span>);
 
<span class="comment">// これでも、結局 string の方が呼ばれる</span>
<span class="method">M1</span>(<span class="string">$&quot;&quot;</span>);
 
<span class="comment">// FormattableString の方を呼びたければ明示的なキャストが必要</span>
<span class="method">M1</span>((<span class="type">FormattableString</span>)<span class="string">$&quot;&quot;</span>);
</code></pre>

`FormattableString` の方を優先的に呼んでほしい場合は、
以下のようなちょっとしたトリックが必要になります。

<pre class="source" title="FormattableString を優先してもらうためのトリック">
<code><span class="comment">// M2(&quot;&quot;) と M2($&quot;&quot;) で呼び分けできる。</span>
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="type">RawString</span> <span class="variable">s</span>) =&gt; <span class="method">M1</span>(<span class="variable">s</span>.Value);
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M2</span>(<span class="type">FormattableString</span> <span class="variable">s</span>) =&gt; <span class="method">M1</span>(<span class="variable">s</span>);
 
<span class="comment">// オーバーロード解決の優先度をごまかすために、string からの暗黙的型変換を持つ構造体を用意。</span>
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">RawString</span>
{
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">string</span> Value;
    <span class="reserved">public</span> <span class="method">RawString</span>(<span class="reserved">string</span> <span class="variable">value</span>) =&gt; Value = <span class="variable">value</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">RawString</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="reserved">new</span> <span class="type">RawString</span>(<span class="variable">s</span>);
 
    <span class="comment">// これがないとダメみたい</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">implicit</span> <span class="reserved">operator</span> <span class="type">RawString</span>(<span class="type">FormattableString</span> <span class="variable">s</span>) =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">InvalidCastException</span>();
}
</code></pre>

暗黙的型変換と比べれば `FormattableString` の方が優先度が高いので、
この `M2` であれば、ちゃんと `M2("")` で `string` の方が、
`M2($"")` で `FormattableString` の方が呼ばれます。

<pre class="source" title="暗黙的型変換よりは FormattableString の方が優先">
<code><span class="comment">// RawString (string) の方が呼ばれる</span>
<span class="method">M2</span>(<span class="string">&quot;&quot;</span>);
 
<span class="comment">// これなら FormattableString の方が呼ばれる</span>
<span class="method">M2</span>(<span class="string">$&quot;&quot;</span>);
 
<span class="comment">// ただ、 + とかを加えてしまうと string 扱いになってしまうので注意</span>
<span class="method">M2</span>(<span class="string">$&quot;&quot;</span> + <span class="string">$&quot;&quot;</span>);
</code></pre>

## <a id="sec-generated-title-10"></a> <a id="nameof-operator">nameof 演算子</a>

C# 6 で、<strong id="key-nameof" class="keyword">nameof 演算子</strong>(nameof operator: "name of X" (Xの名前)を1キーワード化したもの)というものが追加されました。
変数や、クラス、メソッド、プロパティなどの名前(識別子)を文字列リテラルとして取得できます。

<pre class="source" title="nameof 演算子の例" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">MyClass</span>
{
    <span class="reserved">public int</span> MyProperty =&gt; myField;
    <span class="reserved">private int</span> myField = 10;

    <span class="reserved">public void</span> MyMethod()
    {
        <span class="reserved">var</span> myLocal = 10;
        <span class="type">Console</span>.WriteLine(<em><span class="reserved">nameof</span>(<span class="type">MyClass</span>)</em>);
        <span class="type">Console</span>.WriteLine(<span class="reserved">nameof</span>(MyProperty) + <span class="literal">" = "</span> + MyProperty);
        <span class="type">Console</span>.WriteLine(<span class="reserved">nameof</span>(myField) + <span class="literal">" = "</span> + myField);
        <span class="type">Console</span>.WriteLine(<span class="reserved">nameof</span>(MyMethod));
        <span class="type">Console</span>.WriteLine(<span class="reserved">nameof</span>(myLocal) + <span class="literal">" = "</span> + myLocal);
    }
}
</code></pre>


<pre class="console" title="nameof 演算子の例">
<code>MyClass
MyProperty = 10
myField = 10
MyMethod
myLocal = 10
</code></pre>

(ちなみに、[nameof 演算子は const にできます](sp_const.md#constant-expressions)。)

こういう識別子名を文字列化したくなる場面の例としてC# で頻出するパターンは、
`INotifyPropertyChanged` の実装や、`ArgumentException`の引数などがあります。

例えば、C# 5.0までであれば、`ArgumentoException`は以下のようにメッセージを書くことになりました。

<pre class="source" title="ArgumentoExceptionのメッセージ">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">double</span> Sqrt(<span class="reserved">double</span> x)
{
    <span class="reserved">if</span> (x &lt; 0)
        <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentException</span>(<span class="string">"x は0以上でなければなりません"</span>);
    <span class="reserved">return</span> <span class="type">Math</span>.Sqrt(x);
}
</code></pre>

しかし、この例のように、普通の文字列リテラルとして識別子を書いてしまうと、それが識別子だという情報が失われて、ソースコード解析の対象から外れてしまう問題があります。例えばVisual Studioは、変数、引数、メソッド名など、識別子のリネーム機能を持っていますが、文字列中に埋め込んでしまったものは識別子としては認識されず、リネームできません。

そこで、C# 6で追加されたnameof 演算子を使います。

<pre class="source" title="ArgumentoExceptionのメッセージをnameofを使って書き替え">
<code><reserved></span><span class="reserved">static</span> <span class="reserved">double</span> Sqrt(<span class="reserved">double</span> x)
{
    <span class="reserved">if</span> (x &lt; 0)
        <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentException</span>(<span class="string">$"</span>{<span class="reserved">nameof</span>(x)}<span class="string"> は0以上でなければなりません"</span>);
    <span class="reserved">return</span> <span class="type">Math</span>.Sqrt(x);
}
</code></pre>

このようなリファクタリング機能を使った際、nameof 演算子であれば、その識別子を使っている個所全ての変更も全て行われます。

(ここから下、文章が古い。図も含めて要修正)

例えば、メソッド名などに一度適当な名前を付けて実装したあと、Visual Studioのリファクタリング機能を使ってちゃんとした名前にリネームしたいことがあります。
しかし、文字列にしてしまっている "" 内のメソッド名の部分はリファクタリングできず、元のまま残ります。

<figure>
	[![nameof 演算子をリファクタリングの対象にする](../../../../assets/media/ufcpp2000/csharp/fig/nameof-refactoring.png)](../../../../assets/media/ufcpp2000/csharp/fig/nameof-refactoring.png)
	<figcaption>nameof 演算子をリファクタリングの対象にする</figcaption>
</figure>

nameof 演算子の目的はここにあります。識別子名を文字列化するだけなんですが、ソースコード解析の対象にできます。

INotifyPropertyChanged の実装でもnameof 演算子を使う例を以下に挙げておきましょう。

<pre class="source" title="" lang="">
<code><span class="reserved">using</span> System.ComponentModel;
<span class="reserved">using</span> System.Runtime.CompilerServices;

<span class="reserved">class</span> <span class="type">Rect</span> : <span class="type">BindableBase</span>
{
    <span class="reserved">public int</span> Width
    {
        <span class="reserved">get</span> { <span class="reserved">return</span> _width; }
        <span class="reserved">set</span>
        {
            SetProperty(<span class="reserved">ref</span> _width, <span class="reserved">value</span>);
            <span class="comment">// Width が変化すると Area も変化するので、それを通知</span>
            OnPropertyChanged(<em><span class="reserved">nameof</span>(Area)</em>);
        }
    }
    <span class="reserved">private int</span> _width;

    <span class="reserved">public int</span> Height
    {
        <span class="reserved">get</span> { <span class="reserved">return</span> _height; }
        <span class="reserved">set</span>
        {
            SetProperty(<span class="reserved">ref</span> _height, <span class="reserved">value</span>);
            <span class="comment">// Height が変化すると Area も変化するので、それを通知</span>
            OnPropertyChanged(<em><span class="reserved">nameof</span>(Area)</em>);
        }
    }
    <span class="reserved">private int</span> _height;

    <span class="reserved">public int</span> Area =&gt; Width * Height;
}

<span class="reserved">public class</span> <span class="type">BindableBase</span> : <span class="type">INotifyPropertyChanged</span>
{
    <span class="reserved">protected void</span> SetProperty&lt;<span class="type">T</span>&gt;(<span class="reserved">ref</span> <span class="type">T</span> storage, <span class="type">T</span> value, [<span class="type">CallerMemberName</span>] <span class="reserved">string</span> propertyName = <span class="reserved">null</span>)
    {
        <span class="reserved">if</span> (!Equals(storage, value))
        {
            storage = value;
            OnPropertyChanged(propertyName);
        }
    }

    <span class="reserved">protected void</span> OnPropertyChanged([<span class="type">CallerMemberName</span>] <span class="reserved">string</span> propertyName = <span class="reserved">null</span>)
        =&gt; PropertyChanged?.Invoke(<span class="reserved">this</span>, <span class="reserved">new</span> <span class="type">PropertyChangedEventArgs</span>(propertyName));

    <span class="reserved">public event</span> <span class="type">PropertyChangedEventHandler</span> PropertyChanged;
}
</code></pre>。

### <a id="sec-generated-title-11"></a> <a id="nameof-parameter"></a>nameof(引数) のスコープ変更

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`nameof` にちょっとだけ変更が掛かりました。
以下のように、メソッドに対する属性の中で、そのメソッドの引数の名前が参照できるようになりました。

<pre class="source" title="nameof(引数名)">
<code><span class="reserved">using</span> System<span class="operator">.</span>Diagnostics<span class="operator">.</span>CodeAnalysis;

<span class="comment">// C# 10 までこの属性、 NotNullIfNotNull(&quot;x&quot;) と書かないといけなくて割かしつらかった。</span>
[<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="reserved">nameof</span>(x))]
<span class="reserved">static</span> <span class="reserved">string</span><span class="operator">?</span> <span class="method">m</span>(<span class="reserved">string</span><span class="operator">?</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span>;
</code></pre>

この例で使っているように、きっかけとしては[null 許容参照型](../resource/nullablereferencetype.md#sec-generated-title-6)で使う `NotNullIfNotNull` 属性などのために仕様変更されました。
これ以降にも、[`CallerArgumentExpression`](../cheatsheet/ap_ver10.md#CallerArgumentExpression) 属性や[`InterpolatedStringHandlerArgument`](improvedinterpolatedstring.md#InterpolatedStringHandlerArgument)属性など、
引数名を参照したい属性がじわじわと増えていたりします。

### <a id="sec-generated-title-12"></a> <a id="unbount-type-in-nameof">unbound な型に対する nameof</a>

<h5 class="version version14">Ver. 14</h5>

C# 14 から、`T<>` みたいに型引数を埋めていないジェネリック型(これを unbound (未束縛)とか open (開きっぱなし) な型といいます)に対して `nameof` 演算子を使えるようになりました。

<pre class="source" title="unbound なジェネリック型に対する nameof 演算子">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;&gt;)); <span class="comment">// &quot;List&quot;</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">Dictionary</span>&lt;,&gt;<span class="operator">.</span><span class="property">Keys</span>)); <span class="comment">// &quot;Keys&quot;</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;&gt;<span class="operator">.</span><span class="type struct">Enumerator</span><span class="operator">.</span><span class="method">MoveNext</span>)); <span class="comment">// &quot;MoveNext&quot;</span>
</pre>

`nameof` 演算子では元からどのみち型が引数の部分 (`<>` とその中身)は無視されていたので、
ここを埋めるかどうかは結果得られる文字列に何の影響もありません。
これまでできなかったのは「手間に対して需要が少ない」という実装上の都合で、
C# 14 でようやく着手という流れです。
(`typeof(T<>)` は昔から書けたのでそれの流用でできそうに見えますが、
`typeof` の場合は `typeof(T<>.Member)` みたいなメンバー参照がないので、
今回の `nameof` 対応はそれなりに新規実装の部分があります。)

C# 13 以前だと同じことをしたければ、意味もなく何か適当な型引数を埋めて書いていました。

<pre class="source" title="C# 13 以前は何か適当な型引数を埋めて問題回避していた">
<span class="comment">// int の部分には特に意味はないけども、埋めないとコンパイルが通らなかったので適当に int を採用。</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt;)); <span class="comment">// &quot;List&quot;</span>
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">Dictionary</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>&gt;<span class="operator">.</span><span class="property">Keys</span>)); <span class="comment">// &quot;Keys&quot;</span>
<span class="static"><span class="type">Console</span></span><span class="operator">.</span><span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">nameof</span>(<span class="type">List</span>&lt;<span class="reserved">int</span>&gt;<span class="operator">.</span><span class="type struct">Enumerator</span><span class="operator">.</span><span class="method">MoveNext</span>)); <span class="comment">// &quot;MoveNext&quot;</span>
</pre>

ただ、型引数にかかっている制約によっては「適当に `int` を渡す」みたいなことがかなり難しくなります。
場合によっては、以下のように「絶対に書けない」という状況も発生します。
(この場合、メソッド `M` が public なのがおかしいというのはありますが、原理的にはこういうことがありえます。)

<pre class="source" title="nameof が使えない状況を作ったもの">
<span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="comment">// static abstract があると M&lt;I&gt; と書けなくなる。</span>
    <span class="comment">// (実装したクラスでないと渡せない。)</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">abstract</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>();
}

<span class="reserved">public</span> <span class="reserved">abstract</span> <span class="reserved">class</span> <span class="type">B</span>
{
    <span class="comment">// アクセス制限がかなり厳しいコンストラクターを用意。</span>
    <span class="comment">// クラス自体は public であっても、別プロジェクトで派生クラスは作れない。</span>
    <span class="reserved">private</span> <span class="reserved">protected</span> <span class="type">B</span>() { }
}

<span class="comment">// 実装しているクラスは internal で、外からは使わせない。</span>
<span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">D</span> : <span class="type">B</span>, <span class="type">I</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>() { }
}

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">C</span></span>
{
    <span class="comment">// T : I のせいで派生クラスでないとダメ。</span>
    <span class="comment">// T : B のせいで派生クラスを作れない。</span>
    <span class="comment">// 唯一の実装クラス D は internal なので、外からは使えない。</span>
    <span class="comment">// 結果、C# 13 以前は nameof(M&lt;&gt;) が使えなかった。</span>
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="static"><span class="method">M</span></span>&lt;<span class="type param">T</span>&gt;() <span class="reserved">where</span> <span class="type param">T</span> : <span class="type">B</span>, <span class="type">I</span>
    {
    }
}
</pre>



<!-- original-page-break -->


## <a id="sec-generated-title-13"></a> <a id="raw-string"></a>生文字列リテラル

C# 11 で、3つ以上の連続した `"` を使うことで、「一切エスケープが必要ない文字列リテラル」を書けるようになりました。

<pre class="source" title="raw string literal">
<code><span class="comment">// &quot;&quot;&quot; から始まる文字列リテラル(raw string, 生文字列)。</span>
<span class="reserved">var</span> <span class="variable">quote</span> = <span class="string">&quot;&quot;&quot;
    &quot; はそのまま &quot; として使われて、
    \ も \ のままの意味。
    \\ は \ が2個。
    {} とかも特別な解釈はされない。
    &quot;&quot;&quot;</span>;
</code></pre>

これを<strong id="key-raw-string" class="keyword">生文字列リテラル</strong>(raw string literal)と言います。

最近は「[言語内言語](../../../blog/2022/2/embedded-languages/index.md)」みたいなものの需要が微妙に高まっている中、
こういう「エスケープ不要の文字列」への要望が強くなってきています。
本来ならば[逐語的文字列リテラル](st_embeddedtype.md#verbatim-string)(`@""`)がその役割に当たるんですが、この `@""` の構文が微妙に使いにくいので、それを置き換えるような新しい文法が導入されました。

### <a id="sec-generated-title-14"></a> <a id="normal-literal">背景: 通常の文字列リテラルや、逐語的リテラル</a>

多くのプログラミング言語で、通常、`"` や `'` などの記号で挟まられた部分が[文字列リテラル](st_embeddedtype.md#stringl)になります。
この「通常の文字列リテラル」で困るのは、その文字列中に `"` や `'` 自身を含む場合で、
C# ではそういう場合のために、`\` を使った[エスケープ](st_embeddedtype.md#escape-sequence)を行います。

<pre class="source" title="通常の文字列リテラル">
<code><span class="comment">// &quot; を含む文字列リテラル。</span>
<span class="reserved">var</span> <span class="variable">quote</span> = <span class="string">&quot;\&quot;&quot;</span>;
</code></pre>

エスケープが必要な文字が増えてくるとかなり煩雑です。
そこで C# では、`@""` という書き方で、以下のように、エスケープを<em>減らせる</em>ようにしました。
これを[逐語的文字列リテラル](st_embeddedtype.md#verbatim-string)(verbatim string literal)と言います。

* `\` は `\` としてそのまま使われる
* リテラル中に改行を含められる

<pre class="source" title="逐語的文字列リテラル">
<code><span class="comment">// @&quot;&quot; と書くと、\ と改行のエスケープが不要に。</span>
<span class="reserved">var</span> <span class="variable">quote</span> = <span class="string">@&quot;これで3行の文字列になる。
\ は \ のまま使われる。\\ も \ 2つ。
ただし、&quot;&quot; を使いたいときは &quot;&quot; を2個並べないとダメ。これでダブルクォーテーションマーク1つ扱い。&quot;</span>;
</code></pre>

「エスケープなしで書ける文字列」というのが逐語的文字列の存在意義なんですが、
もうこの時点で、「`"` にはエスケープが必要」となっています。
その他、[文字列補間](#conditional-in-string-interpolation)との組み合わせでは `{}` のエスケープも必要です。
また、もう1つの要望として、「複数行の文字列を書くとき、インデントを揃えたいけどできない」という問題もあります。

<pre class="source" title="逐語的文字列補間とその欠点">
<code><span class="reserved">var</span> <span class="variable">value</span> = 123;

<span class="comment">// $@&quot;&quot; で逐語的 + 文字列補間。</span>
<span class="comment">// - { を使いたければ {{ というように、そこそこ使いたくなりがちな文字に結局エスケープが必要</span>
<span class="comment">// - 最初と最後の行の改行も文字列に含まれる</span>
<span class="comment">// - インデントのスペース4つも文字列に含まれる</span>
<span class="reserved">var</span> <span class="variable">quote</span> = <span class="string">$@&quot;</span><span class="string">
    {{
      &quot;&quot;key&quot;&quot;: </span>{<span class="variable">value</span>}<span class="string">
    }}
    </span><span class="string">&quot;</span>;
</code></pre>

### <a id="sec-generated-title-15"></a> <a id="raw-string-syntax">新文法: 生文字列</a>

`"` や `'` を含め、あらゆる文字を一切エスケープなしで書けるようにしたいということで、
C# 11 で、`"""` というように、「3つ以上の `"` を並べる」という新しい文法を追加しました。

以下のように、単一行か複数行かと、文字列補間の有無によって4パターンあります。

<pre class="source" title="4種の生文字列の例">
<code><span class="reserved">var</span> <span class="variable">value</span> = 123;

<span class="reserved">var</span> <span class="variable">singleLine</span> = <span class="string">&quot;&quot;&quot;{ &quot;abc&quot;: 123 }&quot;&quot;&quot;</span>;

<span class="reserved">var</span> <span class="variable">mutiLine</span> = <span class="string">&quot;&quot;&quot;
    {
      &quot;abc&quot;: 123
    }
    &quot;&quot;&quot;</span>;

<span class="reserved">var</span> <span class="variable">singleLineInterpolation</span> = <span class="string">$&quot;&quot;&quot;</span><span class="string">abc: </span>{<span class="variable">value</span>}<span class="string">&quot;&quot;&quot;</span>;

<span class="reserved">var</span> <span class="variable">mutiLineInterpolation</span> = <span class="string">$&quot;&quot;&quot;
</span><span class="string">    abc: </span>{<span class="variable">value</span>}<span class="string">
    &quot;&quot;&quot;</span>;
</code></pre>

### <a id="sec-generated-title-16"></a> <a id="arbitrary-number">3つ以上の "</a>

生文字列の目的は「一切のエスケープが不要」というものです。
そこで通常問題になるのが、`"""` の内側で同じく `"""` を使いたい場合。

例えばの話、「自分自身を文字列リテラル化したい」みたいなことを考えてみましょう。
まず、以下のような C# 11 コードがあったとします。

<pre class="source" title="生文字列を使った C# コードの例">
<code><span class="reserved">var</span> <span class="variable">mutiLine</span> = <span class="string">&quot;&quot;&quot;
    {
      &quot;abc&quot;: 123
    }
    &quot;&quot;&quot;</span>;
</code></pre>

一切エスケープ不要というなら、「この C# コードを出力する C# コード」みたいなものもエスケープなしで書けるようにしたいです。
こういう場合に、以下のようなコードを書いてしまうと、最初の `"""` が出て来た時点で文字列リテラルを閉じようとしてしまって、コンパイル エラーになります。

<pre class="source" title="じゃあ、生文字列の中で &quot;&quot;&quot; を書きたい場合は？">
<code><span class="comment">// &quot;&quot;&quot; と &quot;&quot;&quot; の間に &quot;&quot;&quot; は書けない。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    var mutiLine = <span class="error">&quot;&quot;&quot;</span></span>
        {
          <span class="string">&quot;abc&quot;</span>: 123
        }
        <span class="string">&quot;&quot;&quot;;</span>
    <span class="string">&quot;&quot;&quot;);</span>
</code></pre>

そこでどうするかというと、生文字列リテラルの開始文字を `""""` と4つに増やします。
(同じ個数の `"` が出てくるまで文字列リテラルが終わりません。)

<pre class="source" title="&quot; を4つにすれば問題解決">
<code><span class="comment">// &quot; 4つで開始すれば、リテラルの中で &quot;&quot;&quot; (&quot; 3つ)を書いても問題ない。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string"><em>&quot;&quot;&quot;&quot;</em>
    var mutiLine = &quot;&quot;&quot;
        {
          &quot;abc&quot;: 123
        }
        &quot;&quot;&quot;;
    <em>&quot;&quot;&quot;&quot;</em></span>);
</code></pre>

これが、C# の生文字列リテラルの仕様が「3つ<em>以上</em>の `"` を並べる」になっている理由です。
もちろんさらに入れ子を増やして、`"""""` (5つ)の内側に `""""` を書くこともできます。

<pre class="source" title="入れ子を2重にして、&quot; を5つに">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;&quot;&quot;
    Console.WriteLine(&quot;&quot;&quot;&quot;
        var mutiLine = &quot;&quot;&quot;
            {
              &quot;abc&quot;: 123
            }
            &quot;&quot;&quot;;
        &quot;&quot;&quot;&quot;);
    &quot;&quot;&quot;&quot;&quot;</span>);
</code></pre>

逆に `"` 2つがダメなのは、`""` が既存の文法で有効なもの(空文字列になる)なので、
意味を変えるわけにはいかないからです。

<pre class="source" title="&quot; 2つはただの空文字列">
<code><span class="comment">// 生文字列の &quot;+&quot; ではなく、空文字列2つの結合(= 結局は空文字列)。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;</span>
    +
    <span class="string">&quot;&quot;</span>);
</code></pre>

### <a id="sec-generated-title-17"></a> <a id="single-or-multiple">単一行と複数行</a>

単一行リテラルか複数行リテラルかは、単純に `"""` の後ろに改行があるかどうかで変わります。

<pre class="source" title="">
<code><span class="comment">// 単一行生文字列。</span>
<span class="reserved">var</span> <span class="variable">singleLine</span> = <span class="string">&quot;&quot;&quot;この中身が文字列リテラル&quot;&quot;&quot;</span>;

<span class="comment">// 複数行生文字列。</span>
<span class="reserved">var</span> <span class="variable">multiLine</span> = <span class="string">&quot;&quot;&quot;
    この行が文字列リテラル。この前後には改行文字は残らない。
    &quot;&quot;&quot;</span>;

<span class="comment">// 以下の3行は全く同じ結果になる。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;a\&quot;b&quot;</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;a&quot;b&quot;&quot;&quot;</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    a&quot;b
    &quot;&quot;&quot;</span>);

<span class="comment">// 以下の3行も全く同じ結果。</span>
<span class="comment">// (C# ソースコードの改行コード次第。この例の場合は LF。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;abc\ndef&quot;</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">@&quot;abc
def&quot;</span>);
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    abc
    def
    &quot;&quot;&quot;</span>);
</code></pre>

ちょっと変わっているのは、複数行リテラルの場合、`"""` と改行の間にスペースが挟まっていても複数行生文字列リテラルと認識されます。

<pre class="source" title="&quot;&quot;&quot; の後ろのスペースは無視される">
<code><span class="comment">// &quot;&quot;&quot; の後ろに実はスペースが4つあるけど、それは無視される。</span>
<span class="comment">// (ファイルの改行コード次第で 7 か 8。</span>
<span class="comment">// abcdef の6文字 + \r\n (改行)。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;<em>    </em>
    abc
    def
    &quot;&quot;&quot;</span>.Length);
</code></pre>

今のところは開き `"""` の後ろに書いても OK (ただし無視される)なのは空白文字だけですが、
生文字列の仕様のインスパイア元が Markdown の ```` ``` ```` なので、
もしかしたら以下のような「文字列の中身が何かの注釈を付ける」みたいな仕様は将来認められる可能性はあります。

<pre class="source" title="Markdown みたいに、生文字列に注釈を付けれるようにするかも？">
<code><span class="comment">// C# 11 としては不正。</span>
<span class="comment">// 「将来もしかしたら」程度の構文案。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;json</span>
    {
      <span class="string">&quot;id&quot;</span>: 123,
      <span class="string">&quot;name&quot;</span>: <span class="string">&quot;abc&quot;</span>
    }
    <span class="string">&quot;&quot;&quot;.Length);</span>
</code></pre>


また、複数行生文字列では、以下のように、「1行たりとも中身がないリテラル」は書けません。

<pre class="source" title="">
<code><span class="comment">// 先頭・末尾の改行は無視されるので、これが空文字列。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;

    &quot;&quot;&quot;</span>);

<span class="comment">// じゃあ、これは？…</span>
<span class="comment">// 「空文字列よりも短い文字列リテラル」というのも変で、単にコンパイル エラーに。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    <span class="error">&quot;&quot;&quot;</span></span>);
</code></pre>


### <a id="sec-generated-title-18"></a> <a id="multiline-indent">複数行生文字列とインデント</a>

元々インデントが深い場所で逐語的文字列リテラルを書いた場合、
以下のように、普段の C# コードと同じようなインデントを付けれないという問題があります。

<pre class="source" title="逐語的文字列リテラルの中にインデントを入れるわけにはいかない">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">flag</span>, <span class="reserved">int</span> <span class="variable">count</span>)
    {
        <span class="control">if</span> (<span class="variable">flag</span>)
        {
            <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; <span class="variable">count</span>; <span class="variable">i</span>++)
            {
                <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">@&quot;
インデントが崩れる。
左寄せにしないとリテラルにスペースが含まれちゃう。
&quot;</span>);
            }
        }
    }
}
</code></pre>

一方、生文字列では自由にインデントを入れられます。
以下のように、閉じ `"""` の行のインデントを基準にして、それよりも左側のスペースはコンパイル結果には残りません。

<pre class="source" title="生文字列のインデント">
<code><span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">flag</span>, <span class="reserved">int</span> <span class="variable">count</span>)
    {
        <span class="control">if</span> (<span class="variable">flag</span>)
        {
            <span class="control">for</span> (<span class="reserved">int</span> <span class="variable">i</span> = 0; <span class="variable">i</span> &lt; <span class="variable">count</span>; <span class="variable">i</span>++)
            {
                <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
                    インデントして大丈夫。
                    ここよりも左側のスペースはコンパイル結果の文字列には含まれない。
                    &quot;&quot;&quot;</span>); <span class="comment">// この行のインデントが基準で、そこから前のスペースが消える。</span>
            }
        }
    }
}
</code></pre>

ただ、これはこれで逆に、以下のようなコードには注意が必要です。

<pre class="source" title="閉じ &quot;&quot;&quot; のインデントには注意">
<code><span class="comment">// 1</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    a
    &quot;&quot;&quot;</span>.Length);

<span class="comment">// 5</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
    a
&quot;&quot;&quot;</span>.Length); <span class="comment">// 犯人はこの行。インデントがずれてる。</span>
</code></pre>

ちなみに、以下のように、閉じ `"""` の行よりもインデントが少ないコードを書くとコンパイル エラーになります。

<pre class="source" title="インデントが足りなくてエラーになる例">
<code><span class="comment">// インデントが不正(足りない)なのでエラーに。</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;&quot;&quot;
<span class="error">a</span>
    &quot;&quot;&quot;</span>);
</code></pre>

#### <a id="sec-generated-title-19"></a> <a id="mixed-whitespace">空白文字の混在</a>

C# は通常の(ASCII 文字の)スペース(文字コード U+0020)以外にも、以下のような文字を空白文字とみなします(通常スペースと同じ扱いになります)。

* Unicode の文字カテゴリーが Zs (Space Separator)の文字
* 水平タブ(U+0009)
* 垂直タブ(U+000B)
* フォーム フィード(U+000C)

これらの空白文字を閉じ `"""` の行に使った場合、途中の行にも全く同じ順序で同じ文字を並べなければなりません。
見えない文字なので少しわかりにくいですが、以下のコードでは1つ目の生文字列はOKで、2つ目(意図的に違う文字を混ぜたもの)はコンパイル エラーになります。

<pre class="source" title="">
<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;&quot;&quot;
    この行は OK
    &quot;&quot;&quot;</span>); <span class="comment">// U+1680 Ogam Space (見える空白文字。古アイルランドで使ってたらしい)</span>

<span class="type"><span class="static">Console</span></span><span class="operator">.</span><span class="method"><span class="static">WriteLine</span></span>(<span class="string">&quot;&quot;&quot;
<span class="error" title="CS9003">    </span>違う空白文字を混ぜてしまうとコンパイル エラー。
    &quot;&quot;&quot;</span>);
</pre>

(幾分かわかりやすくするために、「見える空白文字」である Ogam Space という文字を使っています。
ちなみに、エラーになっている行はこの Ogam Space と通常スペースの混在です。)



### <a id="sec-generated-title-20"></a> <a id="priority-verbatim">注意: @"" 優先</a>

1つ非常に紛らわしい書き方がありまして…
以下のコード、出力はどうなるでしょう？

<pre class="source" title="@&quot;&quot;&quot;">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">@&quot;&quot;&quot;abc&quot;&quot;&quot;</span>);
</code></pre>

答えは `"abc"` です。両端に `"` が付いてきます。

これ、`@"` から始まっているので逐語的文字列リテラルの方になります。
で、`@""` の中では「`"` を書きたければ `""` と書く」というエスケープをしますので、
「`@"""abc"""` は `"abc"` として解釈される逐語的文字列リテラル」ということになります。

`@` は見落としがちな文字なので多少注意が必要です。

## <a id="sec-generated-title-21"></a> <a id="raw-string-interpolation">生文字列、かつ、文字列補間</a>

「生文字列で文字列補間をしたい」という要望もそれなりにあります。
例えば以下のような感じのコードは、そのものはないにしても似たようなコードは書きたいことがあると思います。

<pre class="source" title="生文字列で文字列補間">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">format</span>(123, <span class="string">&quot;abc&quot;</span>));

<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">format</span>(<span class="reserved">int</span> <span class="variable">id</span>, <span class="reserved">string</span> <span class="variable">name</span>) =&gt; <span class="string">$&quot;&quot;&quot;
</span><span class="string">    id: </span>{<span class="variable">id</span>}<span class="string">
    name: &quot;</span>{<span class="variable">name</span>}<span class="string">&quot;</span><span class="string">
    &quot;&quot;&quot;</span>;
</code></pre>

補間をやるなら「`{` を含めたいときにエスケープが必要になってしまう」という懸念があって、
当初は前向きに検討されていませんでした。
ただ、最終的に、「`"` と同じく `$` の個数も可変にして解決」という手段を採りました。
「`$` の個数と同じ数の `{` と `}` を書いたときだけ補間あつかい、それ以下の場合は普通の文字列として `{` と `}` を解釈」となります。

例えば、「文字列補間で JSON を作る」みたいなことをしたい場合、`{` を多用することになるわけですが、
この場合は `$` を2個にすることで、`{` と `}` 1個はただの文字になって、`{{}}` が文字列補間になります。

<pre class="source" title="$ を2個にすれば、{ 1個はエスケープなしで書ける">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">format</span>(123, <span class="string">&quot;abc&quot;</span>));

<span class="reserved">static</span> <span class="reserved">string</span> <span class="method">format</span>(<span class="reserved">int</span> <span class="variable">id</span>, <span class="reserved">string</span> <span class="variable">name</span>) =&gt; <span class="string">$$&quot;&quot;&quot;
</span><span class="string">    {
      &quot;id&quot;: </span>{{<span class="variable">id</span> <span class="comment">/* ここは補間 */</span> }}<span class="string">,
      &quot;name&quot;: &quot;</span>{{<span class="variable">name</span> <span class="comment">/* ここも補間 */</span>}}<span class="string">&quot;
    }</span><span class="string">
    &quot;&quot;&quot;</span>;
</code></pre>

<pre class="console" title="$ を2個にすれば、{ 1個はエスケープなしで書ける">
<code>{
  "id": 123,
  "name": "abc"
}
</code></pre>


<!-- original-page-break -->

## <a id="sec-generated-title-22"></a> <a id="utf8-literal"></a>UTF-8 リテラル

<h5 class="version version11">Ver. 11</h5>

C# 11 で、`"abc"u8` みたいに、文字列リテラルの後ろに `u8` 接尾辞を付けることで、UTF-8 な byte 列を文字列リテラルの形で書けるようになりました。

<pre class="source" title="u8 リテラルの例">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">hex</span> <span class="operator">=</span> <em><span class="string">&quot;0123456789ABCDEF&quot;u8</span></em>;
</code></pre>

<strong id="key-utf8-literal" class="keyword">UTF-8 リテラル</strong>(UTF-8 literal)、もしくは語尾を取って u8リテラル(u8 literal)と呼びます。
ちなみに、UTF-8 リテラルの型は `ReadOnlySpan<byte>` になります。
(`var` による型推論も使えます。)

<pre class="source" title="u8 リテラルの型は ReadOnlySpan&lt;byte&gt;">
<code><span class="reserved">var</span> <span class="variable">hex</span> <span class="operator">=</span> <span class="string">&quot;0123456789ABCDEF&quot;u8</span>;
<span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="warning"><span class="variable">hex</span> <span class="reserved">is</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt;</span>); <span class="comment">// 「常に true」警告が出る</span>
</code></pre>

### <a id="sec-generated-title-23"></a> <a id="utf8-in-csharp">補足: C# と UTF-8</a>

UTF-8 のリテラルの話をもう少し掘り下げる前に、C# における文字コードの話を少し補足しておきます。

#### <a id="sec-generated-title-24"></a> <a id="history">時代背景</a>

今となっては、文字コードと言えばほぼ Unicode で、
その他の文字コードは互換性のために残っていると言っても過言ではないと思います。
Unicode に関する話は昔、Build Insider に寄稿したことがあるのでそちらも参照してください。

* [Unicodeとは？ その歴史と進化、開発者向け基礎知識](https://www.buildinsider.net/language/csharpunicode/01)
* [Unicodeと、C#での文字列の扱い](https://www.buildinsider.net/language/csharpunicode/02)

また、Unicode でも、符号化方式として、主に UTF-8 と UTF-16 という形式があります。
2000年代頃から徐々に UTF-8 の方が主流になってきています。

ただ、C# くらいの世代(2000年発表、2002年正式リリース)のプログラミング言語では、
結構昔の文字コードを引きずっていますし、
UTF-16 が主流になると思われていた時代の名残りが大きいです。

そのため、C# の文字(`char`)や文字列(`string`)は UTF-16 前提で、16ビット整数になっています。
(同じような方針になってしまっているプログラミング言語に Java や JavaScript があります。)

<pre class="source" title="char は16ビット">
<code><span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="reserved">sizeof</span>(<span class="reserved">char</span>)); <span class="comment">// 16</span>
</code></pre>

ところが、時代は UTF-8 一色になりました。
それにそもそも、プログラムの中で文字列操作する際にはほとんど ASCII コードに収まる文字しか使わない場面も多いです。
(UTF-8 は ASCII コードと完全互換です。
一方で、UTF-16 の場合は「1バイトを2バイトに引き延ばす」みたいな変換処理が必要で、この負担が案外大きいです。)

その結果、ここ数年、C# で「文字が UTF-16」というのが結構な負担になっていました。

#### <a id="sec-generated-title-25"></a> <a id="utf8-bytes">byte でやりくり</a>

この文字コード問題に対して、一時、
`Utf8String` みたいな名前で UTF-8 な型を追加しようか何て話もありました。
しかし、その方向性だと、`string` と `Utf8String` の2重管理がしんどい(これだけ `string` 前提で .NET エコシステムが確立された状況で追加は無理だろう)という雰囲気になっています。

そうこうしているうちに、「生 `byte` 列で UTF-8 を扱う」と言うのが .NET エコシステム内でデファクトスタンダード化してしまいました(今ここ)。
例えば `System.Text.Unicode` 名前空間中のメソッドは以下のような感じになっています。

<pre class="source" title="System.Text.Unicode.Utf8 クラスのメソッドの一部">
<code><span class="reserved">using</span> System.Buffers;

<span class="reserved">namespace</span> System.Text.Unicode;

<span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">Utf8</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">OperationStatus</span> <span class="method">FromUtf16</span>(
        <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">source</span>, <em><span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">destination</span></em>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">charsRead</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">bytesWritten</span>,
        <span class="reserved">bool</span> <span class="variable">replaceInvalidSequences</span> = <span class="reserved">true</span>, <span class="reserved">bool</span> <span class="variable">isFinalBlock</span> = <span class="reserved">true</span>);

    <span class="reserved">public</span> <span class="reserved">static</span> <span class="type">OperationStatus</span> <span class="method">ToUtf16</span>(
        <em><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">source</span></em>, <span class="type">Span</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">destination</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">bytesRead</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">charsWritten</span>,
        <span class="reserved">bool</span> <span class="variable">replaceInvalidSequences</span> = <span class="reserved">true</span>, <span class="reserved">bool</span> <span class="variable">isFinalBlock</span> = <span class="reserved">true</span>);
}
</code></pre>

`Span<byte>` と `ReadOnlySpan<byte>` で UTF-8 文字列を扱っています。

#### <a id="sec-generated-title-26"></a> <a id="literal-bytes">C# 10 までの課題: 文字列リテラルの byte 配列化</a>

一応、`Span<byte>` で UTF-8 文字列を扱えるとはいえ、
問題は文字列リテラルです。
`"true"` とか `" HTTP/1.0\r\n"` とか、 UTF-8 文字列 (ほとんどの場合、ASCII 文字列)を定数でプログラム中に埋め込みたい場面は結構あります。

今だと以下のように `byte` 定数を並べた配列を `new byte[]` するしか方法がありません。

<pre class="source" title="UTF-8 代わりの byte 定数">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_true</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'t'</span>, (<span class="reserved">byte</span>)<span class="string">'r'</span>, (<span class="reserved">byte</span>)<span class="string">'u'</span>, (<span class="reserved">byte</span>)<span class="string">'e'</span> };
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_false</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'f'</span>, (<span class="reserved">byte</span>)<span class="string">'a'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span>, (<span class="reserved">byte</span>)<span class="string">'s'</span>, (<span class="reserved">byte</span>)<span class="string">'e'</span> };
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_null</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'n'</span>, (<span class="reserved">byte</span>)<span class="string">'u'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span> };
</code></pre>

一応、これ、[最適化はされて `new byte[]` のヒープ アロケーションは発生せず](../../../blog/2022/2/span-optimization/index.md)、
直接 DLL 中のデータ領域からデータが読まれます。

とはいえ明らかに煩雑で、`true` などの文字列から上記のような `byte` 配列を生成してもらいたくなります。
その結果、C# 11 で UTF-8 リテラルが入ることになりました。

#### <a id="sec-generated-title-27"></a> <a id="utf8-literal-usage">UTF-8 リテラルの利用例</a>

[.NET の標準ライブラリ中のコード](https://github.com/dotnet/runtime)にも、前述のような「本当は文字列リテラルとして埋め込みたいのに仕方がなく `new byte[]` にしていた」というものが山ほどありました。
C# 11 化に伴い、大量のコードが UTF-8 リテラル化されています。
以下のような Pull Request が出ています。

* [#68334](https://github.com/dotnet/runtime/pull/68334)
* [#69995](https://github.com/dotnet/runtime/pull/69995)
* [#70568](https://github.com/dotnet/runtime/pull/70568)
* [#70894](https://github.com/dotnet/runtime/pull/70894)
* [#71417](https://github.com/dotnet/runtime/pull/71417)
* [#71992](https://github.com/dotnet/runtime/pull/71992)

これらの中には、例えば以下のような文字列が含まれています。

<pre class="source" title="UTF-8 リテラル化された文字列の例">
<code><span class="comment">// HTTP のステータス コード</span>
<span class="reserved">var</span> <span class="variable">ok</span> <span class="operator">=</span> <span class="string">&quot;200&quot;u8</span>;
<span class="reserved">var</span> <span class="variable">notFound</span> <span class="operator">=</span> <span class="string">&quot;404&quot;u8</span>;

<span class="comment">// CR LF</span>
<span class="reserved">var</span> <span class="variable">eol</span> <span class="operator">=</span> <span class="string">&quot;\r\n&quot;u8</span>;

<span class="comment">// 既知の型名</span>
<span class="reserved">var</span> <span class="variable">boolName</span> <span class="operator">=</span> <span class="string">&quot;Boolean&quot;u8</span>;
<span class="reserved">var</span> <span class="variable">byteName</span> <span class="operator">=</span> <span class="string">&quot;Byte&quot;u8</span>;
<span class="reserved">var</span> <span class="variable">in32Name</span> <span class="operator">=</span> <span class="string">&quot;Int32&quot;u8</span>;

<span class="comment">// 変換用テーブル</span>
<span class="reserved">var</span> <span class="variable">base64Table</span> <span class="operator">=</span> <span class="string">&quot;ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/&quot;u8</span>;
<span class="reserved">var</span> <span class="variable">base32Table</span> <span class="operator">=</span> <span class="string">&quot;abcdefghijklmnopqrstuvwxyz012345&quot;u8</span>;
<span class="reserved">var</span> <span class="variable">hexTable</span> <span class="operator">=</span> <span class="string">&quot;0123456789ABCDEF&quot;u8</span>;

<span class="comment">// Culture 名</span>
<span class="reserved">var</span> <span class="variable">cultureNames</span> <span class="operator">=</span> <span class="comment">// 一部抜粋</span>
    <span class="string">&quot;en-us&quot;u8</span> <span class="operator">+</span>
    <span class="string">&quot;fr-fr&quot;u8</span> <span class="operator">+</span>
    <span class="string">&quot;it-it&quot;u8</span>; <span class="comment">// 以下略</span>
</code></pre>

### <a id="sec-generated-title-28"></a> <a id="utf8-literal-detail">UTF-8 リテラルの詳細</a>

とうことで、改めて UTF-8 リテラルの話に戻りましょう。

[本節冒頭](#utf8-literal)で書いた通り、文字列リテラルの後ろに `u8` 接尾辞を付けることで UTF-8 リテラルになり、`ReadOnlySpan<byte>` を得ることができます。

<pre class="source" title="u8 リテラルの例">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <em><span class="string">&quot;abc&quot;u8</span></em>;
</code></pre>

ちなみに、初期案としては、`u8` 接尾辞がなしの通常の文字列リテラルも、
ターゲット型を見て自動的に UTF-8 リテラルに変換する話も出ていましたが、
オーバーロード解決がうまくいかず、没になりました。

<pre class="source" title="没案">
<code><span class="comment">// 初期案では OK だった(今はエラー)。</span>
<span class="reserved">byte</span>[] <span class="variable">s1</span> <span class="operator">=</span> <span class="error"><span class="string">&quot;abc&quot;</span></span>;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s2</span> <span class="operator">=</span> <span class="error"><span class="string">&quot;abc&quot;</span></span>;

<span class="comment">// u8 接尾辞ありで、byte[] への変換も元々は認めてた(今はエラー)。</span>
<span class="reserved">byte</span>[] <span class="variable">s3</span> <span class="operator">=</span> <span class="error"><span class="string">&quot;abc&quot;u8</span></span>;
</code></pre>

#### <a id="sec-generated-title-29"></a> <a id="utf8-literal-lowaring">UTF-8 リテラルの展開結果</a>

UTF-8 リテラルは、その文字列を UTF-8 として符号化した byte 列に展開されます。
例えば、前述の `"abc"u8` は、以下のようなコードとほぼ同じ意味になります。

<pre class="source" title="u8 リテラルの展開結果の例">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">97</span>, <span class="number">98</span>, <span class="number">99</span> };
</code></pre>

この手のコードは、C# コンパイラーによって、以下のようなコードに最適化されます。

<pre class="source" title="u8 リテラルの展開結果の最適化結果の例">
<code><span class="reserved">byte</span><span class="operator">*</span> <span class="variable">p</span> <span class="operator">=</span> DLL中のデータが格納されている領域へのポインター;
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt;(<span class="variable">p</span>, <span class="number">3</span>);
</code></pre>

ちなみに、最近の .NET は `Span<T>`, `ReadOnlySpan<T>` に対する最適化が結構よく掛かって、
例えば、`"abc"u8.Length` は JIT 時に単なる `3` に展開されたりします。

#### <a id="sec-generated-title-30"></a> <a id="utf8-literal-concat">+ での結合</a>

UTF-8 リテラル同士は `+` 演算子で結合できます。
例えば、以下の2変数には同じ結果が代入されます。

<pre class="source" title="UTF-8 リテラルの結合の例">
<code><span class="reserved">var</span> <span class="variable">singleLine</span> <span class="operator">=</span> <span class="string">&quot;ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/&quot;u8</span>;

<span class="reserved">var</span> <span class="variable">concatenated</span> <span class="operator">=</span> 
    <span class="string">&quot;ABCDEFGHIJKLMNOPQRSTUVWXYZ&quot;u8</span> <span class="operator">+</span>
    <span class="string">&quot;abcdefghijklmnopqrstuvwxyz&quot;u8</span> <span class="operator">+</span>
    <span class="string">&quot;0123456789&quot;u8</span> <span class="operator">+</span>
    <span class="string">&quot;+/&quot;u8</span>;
</code></pre>

これは、UTF-8 リテラルに対する特殊対応で、
一般の `ReadOnlySpan<byte>` に対しては `+` 結合はできません。

<pre class="source" title="+ 結合ができるのは UTF-8 リテラル同士の場合だけ">
<code><span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">abc</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">97</span>, <span class="number">98</span>, <span class="number">99</span> };
<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">def</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="reserved">byte</span>[] { <span class="number">100</span>, <span class="number">101</span>, <span class="number">102</span> };

<span class="reserved">var</span> <span class="variable">s1</span> <span class="operator">=</span> <span class="error"><span class="variable">abc</span> <span class="operator">+</span> <span class="variable">def</span></span>; <span class="comment">// エラー。</span>
<span class="reserved">var</span> <span class="variable">s2</span> <span class="operator">=</span> <span class="error"><span class="variable">abc</span> <span class="operator">+</span> <span class="string">&quot;def&quot;u8</span></span>; <span class="comment">// 片方が u8 リテラルでもダメ。エラー。</span>
</code></pre>

#### <a id="sec-generated-title-31"></a> <a id="utf8-literal-non-const">注意: 非 const</a>

(少なくとも C# 11 時点では) UTF-8 リテラルは [const](sp_const.md#const) 扱いにはなりません。
const しか書けない場所で使うとエラーになります。
具体的には、例えば、[`switch` や `is`](../datatype/typeswitch.md) に使えません。

<pre class="source" title="UTF-8 は const にはなれない">
<code><span class="comment">// これは OK。</span>
<span class="reserved">bool</span> <span class="method">str</span>(<span class="reserved">string</span> <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="reserved">is</span> <span class="string">&quot;abc&quot;</span>;

<span class="comment">// C# 11 で、これは OK になった。</span>
<span class="reserved">bool</span> <span class="method">charSpan</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="reserved">is</span> <span class="string">&quot;abc&quot;</span>;

<span class="comment">// これはダメ。</span>
<span class="reserved">bool</span> <span class="method">u8</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="reserved">is</span> <span class="error"><span class="string">&quot;abc&quot;u8</span></span>;

<span class="comment">// ちなみに、同じく C# 11 で入ったリスト パターンで、こんな風には書ける(つらい)。</span>
<span class="reserved">bool</span> <span class="method">listPattern</span>(<span class="type struct">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable local">x</span>) <span class="operator">=&gt;</span> <span class="variable local">x</span> <span class="reserved">is</span> [ <span class="number">97</span>, <span class="number">98</span>, <span class="number">99</span> ];
</code></pre>

#### <a id="sec-generated-title-32"></a> <a id="utf8-raw-string">UTF-8 生文字列</a>

[生文字列リテラル](#raw-string)との組み合わせもできます。
この場合も、`"""` の後ろに `u8` 接尾辞を付けます。

<pre class="source" title="UTF-8 生文字列の例">
<code><span class="reserved">var</span> <span class="variable">utf8Json</span> <span class="operator">=</span> <span class="string">&quot;&quot;&quot;
    {
      &quot;id&quot;: 123,
      &quot;name&quot;: &quot;abc&quot;,
      &quot;flag&quot;: true
    }
    &quot;&quot;&quot;u8</span>;
</code></pre>

結果が UTF-8 符号化された `ReadOnlySpan<byte>` になる以外は生文字列リテラルと同じです。

一方で、(少なくとも C# 11 では) 文字列補間との併用はできません。

<pre class="source" title="UTF-8 文字列補間は無理">
<code><span class="reserved">var</span> <span class="variable">x</span> <span class="operator">=</span> <span class="number">123</span>;
<span class="reserved">var</span> <span class="variable">y</span> <span class="operator">=</span> <span class="string">&quot;abc&quot;</span>;

<span class="comment">// これは OK。</span>
<span class="reserved">var</span> <span class="variable">s</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">id: </span>{<span class="variable">x</span>}<span class="string">, name: </span>{<span class="variable">y</span>}<span class="string">&quot;</span>;

<span class="comment">// これはダメ。</span>
<span class="reserved">var</span> <span class="variable">u8</span> <span class="operator">=</span> <span class="string">$&quot;</span><span class="string">id: </span>{<span class="variable">x</span>}<span class="string">, name: </span>{<span class="variable">y</span>}<span class="string">&quot;</span><span class="error">u8</span>;
</code></pre>


#### <a id="sec-generated-title-33"></a> <a id="utf8-literal-invalid-error">注意: 不正な Unicode 文字</a>

UTF-8 リテラルでは、UTF-8 にしたときに不正になるものはコンパイル エラーになります。

「UTF-8 リテラルでは」という前置きがあるのは、
C# の `string` は UTF-16 として不正なものを受け付けてしまうからです。
(この辺りも時代の影響で、昔は今よりも Unicode の扱いがかなり緩かったです。)

具体的には「[サロゲート ペア](https://codezine.jp/article/detail/1592)の片割れ」みたいなやつで、
現代的にはこういう「片割れ」を残すのはよくないと言われていますが、
C# の `char` や `string` は受け付けます。

<pre class="source" title="古き良きガバガバ Unicode の例">
<code><span class="comment">// サロゲート ペアの片割れだけの文字列。</span>
<span class="comment">// 現代的にはエラーにしたい。C# ができた頃にはそんなにうるさく言われなかった。</span>
<span class="reserved">var</span> <span class="variable">highSurrogate</span> <span class="operator">=</span> <span class="string">&quot;\uD801&quot;</span>;

<span class="comment">// ちなみに、 System.Text.Encoding では不正な Unicode 文字列を ? (U+FFFD) に置き換える処理あり。</span>

<span class="comment">// C# でいうところの Unicode は UTF-16 のこと。</span>
<span class="reserved">var</span> <span class="variable">utf16</span> <span class="operator">=</span> System<span class="operator">.</span>Text<span class="operator">.</span><span class="type">Encoding</span><span class="operator">.</span><span class="property">Unicode</span>;

<span class="comment">// 一度符号化して、複号すると…</span>
<span class="reserved">var</span> <span class="variable">encoded</span> <span class="operator">=</span> <span class="variable">utf16</span><span class="operator">.</span><span class="method">GetBytes</span>(<span class="variable">highSurrogate</span>);
<span class="reserved">var</span> <span class="variable">decoded</span> <span class="operator">=</span> <span class="variable">utf16</span><span class="operator">.</span><span class="method">GetString</span>(<span class="variable">encoded</span>);

<span class="comment">// U+FFFD に置き換わってる。</span>
<span class="comment">// この文字は replacement character と言って、</span>
<span class="comment">// 不正な文字を残さないために、認識できなかった文字を置き換えるための文字。</span>
<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">c</span> <span class="control">in</span> <span class="variable">decoded</span>)
{
    <span class="type">Console</span><span class="operator">.</span><span class="method">WriteLine</span>(<span class="string">$&quot;</span>{<span class="variable">c</span>}<span class="string">: </span>{(<span class="reserved">int</span>)<span class="variable">c</span>:<span class="string">X</span>}<span class="string">&quot;</span>);
}
</code></pre>

ですが、C# 11 の時代(2022年)に生まれた UTF-8 リテラルは、
ちゃんと不正な文字列をはじきます。

<pre class="source" title="不正な UTF-8 は受け付けない">
<code><span class="comment">// UTF-8 リテラルの場合は「サロゲート ペアの片割れ」を受け付けない。</span>
<span class="comment">// コンパイル エラーを起こす。</span>
<span class="reserved">var</span> <span class="variable">highSurrogate</span> <span class="operator">=</span> <span class="error"><span class="string">&quot;\uD801&quot;u8</span></span>;
</code></pre>

ちなみに、以下のように、最終的に有効な Unicode 文字列になるものであればちゃんとコンパイルできます。

<pre class="source" title="有効な並びでサロゲート ペアが並んでいればちゃんとコンパイル できる">
<code><span class="reserved">var</span> <span class="variable">surrogatePair</span> <span class="operator">=</span> <span class="string">&quot;\uD801\uDE00&quot;u8</span>;
</code></pre>

一方で、以下のように「`+` で結合すれば最終的には有効になるはずの2つの UTF-8 リテラル」みたいなものはコンパイル エラーになります。

<pre class="source" title="+ で結合する場合、個別にチェック">
<code><span class="reserved">var</span> <span class="variable">surrogatePair</span> <span class="operator">=</span>
    <span class="error"><span class="string">&quot;\uD801&quot;u8</span></span> <span class="operator">+</span>
    <span class="error"><span class="string">&quot;\uDE00&quot;u8</span></span>;
</code></pre>
