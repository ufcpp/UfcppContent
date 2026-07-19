---
title: "【C# 11 候補】 UTF-8 リテラル"
source_url: "https://ufcpp.net/blog/2021/12/utf8-literal/"
content_type: "BlogEntry"
published_at: "2021-12-28T14:45:05"
updated_at: "2024-02-08T20:34:04"
tags: []
umbraco_id: 2394
parent_id: 2375
sort_order: 9
aliases: []
---

# 【C# 11 候補】 UTF-8 リテラル

.NET の UTF-8 対応がらみの続報。

## byte でやりくり

元々、`string` (UTF-16 でデータを持ってる)に加えて `Utf8String` みたいな名前で UTF-8 な型を追加しようか何て話もあったんですが。
`string` と `Utf8String` の2重管理がしんどいだろう、これだけ `string` 前提で .NET エコシステムが確立された状況で追加は無理だろうという雰囲気になっています。

`string` の中身を UTF-8 に変更した方が建設的かもしれないという話も出るくらいですが、さすがにそれをやりだすと大工事過ぎて短期では無理でしょう。
著者個人的にも「10年先ならわからないけども」くらいのお気持ちになりつつあります。

そうこうしているうちに、「生 byte 列で UTF-8 を扱う」と言うのが .NET エコシステム内でデファクトスタンダード化してしまいました(今ここ)。
例えば `System.Text.Unicode` 名前空間中のメソッドは以下のような感じになっています。

<pre class="source" title="">
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

文字なのかその他のバイナリ形式なのかがわからなくなるんであんまり親切設計ではないんですが…
型変換やオーバーロードをあんまり増やすのもしんどく、
「生 byte 列で UTF-8 を扱う」は結構定着しちゃうんじゃないかという感じ。

## リテラル問題

とはいえ。
UTF-8 扱いで `Span<byte>` とかを使うにあたって困るのが文字列リテラル。
今だと以下のように `byte` 定数的に `new byte[]` するしか方法がありません。

<pre class="source" title="UTF-8 代わりの byte 定数">
<code><span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_true</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'t'</span>, (<span class="reserved">byte</span>)<span class="string">'r'</span>, (<span class="reserved">byte</span>)<span class="string">'u'</span>, (<span class="reserved">byte</span>)<span class="string">'e'</span> };
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_false</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'f'</span>, (<span class="reserved">byte</span>)<span class="string">'a'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span>, (<span class="reserved">byte</span>)<span class="string">'s'</span>, (<span class="reserved">byte</span>)<span class="string">'e'</span> };
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">_null</span> = <span class="reserved">new</span> <span class="reserved">byte</span>[] { (<span class="reserved">byte</span>)<span class="string">'n'</span>, (<span class="reserved">byte</span>)<span class="string">'u'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span>, (<span class="reserved">byte</span>)<span class="string">'l'</span> };
</code></pre>

一応、これ、最適化はされて `new byte[]` のヒープ アロケーションは発生せず、
直接 DLL 中のデータ領域からデータが読まれます。

この3つくらいならいいんですけども、極まってくるとありとあらゆる文字列リテラルを UTF-8 byte 列化したくなり…

* [HTTP のヘッダーとかで使う文字列](https://github.com/dotnet/aspnetcore/blob/8b30d862de6c9146f466061d51aa3f1414ee2337/src/Servers/Kestrel/Core/src/Internal/Http2/Http2Connection.Generated.cs)
* [HTTP のステータス コード](https://github.com/dotnet/aspnetcore/blob/52eff90fbcfca39b7eb58baad597df6a99a542b0/src/Shared/runtime/Http2/Hpack/StatusCodes.cs)

とかを見てもらえるとなかなかにつらみを感じてもらえるのではないかと思います。

`"100"` みたいなものすら `new byte[] { (byte)'1', (byte)'0', (byte)'0' }`。

## UTF-8 文字列リテラル

と言うことで着地点として、[リテラルだけ UTF-8 なものを用意しようか](https://github.com/dotnet/csharplang/blob/main/proposals/utf8-string-literals.md)という雰囲気になっています。

* `Span<byte>` や `ReadSpan<byte>` に対して文字列リテラルを渡すと自動的に上記のような UTF-8 byte 列を生成する
* オーバーロード解決や `var` 型推論用に `u8` 接尾辞を用意

例えば以下のように書けるようになります。

暗黙的変換:

<pre class="source" title="byte[] とかへの文字列リテラル代入は UTF-8 byte 列扱い">
<code><span class="reserved">byte</span>[] <span class="variable">array</span> = <span class="string">&quot;hello&quot;</span>;             <span class="comment">// new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20 }</span>
<span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">span</span> = <span class="string">&quot;dog&quot;</span>;            <span class="comment">// new byte[] { 0x64, 0x6f, 0x67 }</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">span</span> = <span class="string">&quot;cat&quot;</span>;    <span class="comment">// new byte[] { 0x63, 0x61, 0x74 }</span>
</code></pre>

`u8` 接尾辞:

<pre class="source" title="u8 を付けると UTF-8 リテラルに">
<code><span class="reserved">string</span> <span class="variable">s1</span> = <span class="string">&quot;hello&quot;</span>u8;      <span class="comment">// エラー。型が合ってない。</span>
<span class="reserved">var</span> <span class="variable">s2</span> = <span class="string">&quot;hello&quot;</span>u8;         <span class="comment">// Ok。型は ReadOnlySpan&lt;byte&gt;。</span>
<span class="type">Span</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">s3</span> = <span class="string">&quot;hello&quot;</span>u8;  <span class="comment">// Ok。</span>
<span class="reserved">byte</span>[] <span class="variable">s4</span> = <span class="string">&quot;hello&quot;</span>u8;      <span class="comment">// Ok。</span>
</code></pre>

UTF-8 として不正になる文字列リテラルはコンパイル エラーにするそうです。
.NET の文字列は UTF-16 というか実際には「古き良き Unicode」(2バイト固定長で行けると思ってた頃の Unicode)なので、「[サロゲート ペア](https://codezine.jp/article/detail/1592)の片割れ」みたいな今となってはダメなやつを受け付けてしまうので。

<pre class="source" title="不正な UTF-16 はコンパイル エラーに">
<code><span class="reserved">byte</span>[] <span class="variable">array</span> = <span class="string">&quot;\uD801&quot;</span>; <span class="comment">// ハイ サロゲートのみ。コンパイル エラーにする。</span>
</code></pre>

ちなみに、`const string` から UTF-8 リテラルも作れるし、
「不正な UTF-16 を `+` でつないで、その結果が有効な UTF-8 になるなら OK」だそうです。

<pre class="source" title="不正 UTF-16 な const string 2つ → 有効な UTF-8 リテラル">
<code><span class="reserved">const</span> <span class="reserved">string</span> first = <span class="string">&quot;\uD83D&quot;</span>;  <span class="comment">// ハイ サロゲート。</span>
<span class="reserved">const</span> <span class="reserved">string</span> second = <span class="string">&quot;\uDE00&quot;</span>; <span class="comment">// ロー サロゲート。</span>
<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">byte</span>&gt; <span class="variable">span</span> = first + second; <span class="comment">// これは OK</span>
</code></pre>

## Utf8String 型の可能性

前述の通り、今の `string` を置き換えるような `Utf8String` 型が追加される可能性はかなり低くなってきたんですが。

一応まだ可能性 0 とは断じない方がいいので、一応この仮定的な `Utf8String` の存在は考慮しているそうです。

もしも `Utf8String` が積極的に使いたい「良い型」になったとしても、
たぶん、`""` から `byte[]`、`Span<byte>`、`ReadOnlySpan<byte>` への暗黙的変換は対して問題にならなさそう。

後悔するとしたら `u8` 接尾辞の「自然な型」を `ReadOnlySpan<byte>` にしてしまう点で、これに関しては「やっぱり `Utf8String` に変えたい」となっても変えれるものではなくなります。
とはいえ、「なので今は自然な型を決めるのはやめておこう」と思うほどのものではない(ので、`ReadOnlySpan<byte>` な方針でいく)でしょう。
