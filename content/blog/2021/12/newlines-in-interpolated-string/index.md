---
title: "【C# 11 候補】 {} 中の改行"
source_url: "https://ufcpp.net/blog/2021/12/newlines-in-interpolated-string/"
content_type: "BlogEntry"
published_at: "2021-12-16T21:48:13"
updated_at: "2021-12-16T21:48:13"
tags: []
umbraco_id: 2382
parent_id: 2375
sort_order: 3
aliases: []
---

# 【C# 11 候補】 {} 中の改行

今日は「実は Visual Studio 17.1 Preview 1 (先月) の時点で既に入ってた」という機能の話。

C# 11 で、`$"{ここ}"` みたいな「補完穴」(interpolation hole: 補完文字列の `{}` の中)の改行に関する仕様がちょっと変わります。

## <a id="new-line-in-string">文字列リテラル中の改行</a>

C# の[文字列リテラル](../../../../study/csharp/start/st_embeddedtype.md#charl)は、`@` を付けると逐語的(`\` を使ったエスケープをしなくなる)になって、その中には改行を直接入れることができます。

<pre class="source" title="@ を付けると文字列内での改行 OK になる">
<code><span class="comment">// @ を付けると文字列内での改行 OK になる。</span>

<span class="reserved">var</span> <span class="variable">s1</span> = <span class="string">&quot;&quot;</span>; <span class="comment">// 改行入れれない。</span>
<span class="reserved">var</span> <span class="variable">s2</span> = <span class="string">@&quot;
&quot;</span>; <span class="comment">// 改行 OK。</span>
<span class="reserved">var</span> <span class="variable">s3</span> = <span class="string">&quot;</span>
<span class="string"><span class="error">&quot;</span>; // 当然これはコンパイル エラー。</span>
</code></pre>

この仕様、[補間文字列](../../../../study/csharp/start/st_string.md#key-interpolated-string)に対しても同様です。

<pre class="source" title="">
<code><span class="comment">// @ を付けると文字列内での改行 OK になるのは $&quot;&quot; でも一緒。</span>

<span class="reserved">var</span> <span class="variable">x</span> = 123;

<span class="reserved">var</span> <span class="variable">s1</span> = <span class="string">$&quot;</span>{<span class="variable">x</span>}<span class="string">&quot;</span>; <span class="comment">// 改行入れれない。</span>
<span class="reserved">var</span> <span class="variable">s2</span> = <span class="string">@$&quot;</span><span class="string">
</span>{<span class="variable">x</span>}<span class="string">
</span><span class="string">&quot;</span>; <span class="comment">// 改行 OK。</span>
<span class="reserved">var</span> <span class="variable">s3</span> = <span class="string">$&quot;</span>{<span class="variable">x</span>}
<span class="string"><span class="error">&quot;</span>; // 当然これはコンパイル エラー。</span>
</code></pre>

## <a id="new-line-in-interpolation-hole">補間穴中の改行</a>

C# はほぼ全ての構文で改行の有無を問わないので、例えば以下の2つのコードは全く同じ意味になります。

<pre class="source" title="1行">
<code><span class="reserved">var</span> <span class="variable">x</span> = 123 + 987;
</code></pre>

<pre class="source" title="改行を入れたもの">
<code><span class="reserved">var</span>
    <span class="variable">x</span>
    =
    123
    +
    987
    ;
</code></pre>

で、補間穴 (`{}`)の中は普通の C# 構文になります。
前述のような「改行の有無を問わない」という常識に照らし合わせると、
以下のようなコードを書けていいはずです。
(C# 10 まではなぜかダメ。)

<pre class="source" title="{} 中の改行">
<code><span class="comment">// なぜかダメだったコード。</span>

<span class="reserved">var</span> <span class="variable">x</span> = 123;

<span class="reserved">var</span> <span class="variable">s1</span> = <span class="string">$&quot;</span>{
    <span class="variable">x</span>
    }<span class="string"><span class="error">&quot;</span></span>;
</code></pre>

ちなみに、これに `@` を付けると C# 10 でもコンパイルできます。
というか、さらに言うと割かし何でも書けます。
`//` コメントすら書けます。

<pre class="source" title="@ を付ければなぜか OK">
<code><span class="comment">// @ を付ければなぜか OK。</span>

<span class="reserved">var</span> <span class="variable">x</span> = 123;

<span class="reserved">var</span> <span class="variable">s1</span> = <span class="string">$@&quot;</span>{
    <span class="variable">x</span>
    +
    987 <span class="comment">// コメントすら OK</span>
    }<span class="string">&quot;</span>;
</code></pre>

## <a id="new-line-in-interpolation-hole-11">C# 11 での変更</a>

で、まあ、`$"{}"` と `$@"{}"` で挙動が違うの、
[仕様](https://github.com/dotnet/csharpstandard/blob/draft-v6/standard/grammar.md)的にもそうなってるらしいんですが、
中の人曰く「[改行を禁止した実際の理由、覚えてない](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-09-20.md#newlines-in-non-verbatim-interpolated-strings)」とのこと。

挙動が違うのも変なのでさらっと直したみたいです。
気づいたタイミング的に [C# 10](../../../../study/csharp/cheatsheet/ap_ver10.md) 正式リリースには間に合わなかったものの、
ほぼ修正は終わってたみたいで、即座に merge、実は 17.1 Preview 1 には入っていたみたいです。

ということで、実は [LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) を入れればもう動くらしい。

![LangVersion preview を入れればもう動くらしい](../../../../../assets/media/1197/newlineininterpolation.png)

(このスクショは Visual Studio 17.1.0 Preview 1.1 で撮影。)

さよなら、LangVersion default。おかえり、preview (1年ぶり2度目)。

ということで、以下のようなコード、C# 11 候補になっていて、
preview 指定すると現在でもコンパイルできたりします。

<pre class="source" title="C# 11 で有効になりそうなコード">
<code><span class="comment">// C# 11 候補。</span>

<span class="reserved">var</span> <span class="variable">x</span> = 123;

<span class="reserved">var</span> <span class="variable">s1</span> = <span class="string">$&quot;</span><span class="string">こっちは C# 11 から OK </span>{
    <span class="variable">x</span>
    +
    987 <span class="comment">// コメントすら OK</span>
    }<span class="string">&quot;</span>;

<span class="reserved">var</span> <span class="variable">s2</span> = <span class="string">$@&quot;</span><span class="string">こっちは元から OK
</span>{
    <span class="variable">x</span>
    +
    987 <span class="comment">// コメントすら OK</span>
    }<span class="string">
def</span><span class="string">&quot;</span>;
</code></pre>

と言うのを[昨日の Pull Request](https://github.com/dotnet/roslyn/pull/58250) を見て初めて気づいたという話でした。
