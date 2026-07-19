---
title: "配列のインデクサー"
source_url: "https://ufcpp.net/blog/2018/12/arrayindexer/"
content_type: "BlogEntry"
published_at: "2018-12-13T10:52:27"
updated_at: "2018-12-13T10:52:27"
tags: []
umbraco_id: 2194
parent_id: 2177
sort_order: 12
aliases: []
---

# 配列のインデクサー

C# 8.0 がらみの話も一段落してしまったので、
今日からしばらく予告通り、Gist に書き捨ててたもののブログ化になります。
ぶっちゃけ、在庫一掃処分セールみたいなものなので過度な期待はしないでください。

今日は C# コンパイラーと JIT レベルの最適化の話。

## 配列の範囲チェック

.NET の配列は、バッファオーバーランとかのメモリ破壊を避けるべく、範囲チェックがかかっています。

<pre class="source" title="">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = <span class="reserved">new</span> <span class="reserved">int</span>[4];
        <span class="reserved">var</span> x = a[5]; <span class="comment">// 範囲外なのでここで IndexOutOfRangeException が飛ぶ</span>
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

`a[5]` のところのコンパイル結果は以下のようになります。

IL:

<pre class="source">
<code><span style="color:purple">IL_0006:</span> <span style="color:blue">ldc.i4.5</span>  <span style="color:green"> // 5 をロード
</span><span style="color:purple">IL_0007:</span> <span style="color:blue">ldelem.i4</span> <span style="color:green"> // 配列要素の読み込み命令
</span></code></pre>

IL には配列の要素を読み書きする命令があります。
この時点では `ldelem` 命令が出力されているだけで、
例外を飛ばすコードはありません。
範囲チェックが挿入されるのはその次の、JIT でネイティブ コード化される段階になります。

x86 コード:

<pre class="source">
<code><span style="color:purple">L000f:</span> <span style="color:blue">cmp</span> dword [eax+0x4], 0x5<span style="color:green"> ; 配列長と 5 を比較
</span><span style="color:purple">L0013:</span> <span style="color:blue">jbe</span> L001e               <span style="color:green"> ; 例外を投げるコードにジャンプ
</span><span style="color:purple">L0015:</span> <span style="color:blue">mov</span> ecx, [eax+0x1c]     <span style="color:green"> ; a[5] の場所のデータを読み込み
</span></code></pre>

元のコードにはない比較・ジャンプ命令が挟まっています。

## 配列の列挙

今度は、全要素列挙することを見てみましょう。
以下のようなコードを考えます。

<pre class="source" title="">
<code><span class="reserved">void</span> M(<span class="reserved">int</span>[] array)
{
    <span class="reserved">for</span> (<span class="reserved">var</span> i = 0; i &lt; array.Length; ++i)
    {
        <span class="reserved">var</span> x = array[i];
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

この場合は、ループ付近が以下のようにコンパイルされます。

IL:

<pre class="source">
<code><span style="color:purple">IL_0004:</span> <span style="color:blue">ldarg.1</span>
<span style="color:purple">IL_0005:</span> <span style="color:blue">ldloc.0</span>
<span style="color:purple">IL_0006:</span> <span style="color:blue">ldelem.i4</span>                 <span style="color:green"> // array[i]</span>
<span style="color:purple">IL_0007:</span> <span style="color:blue">call</span> void WriteLine(int32)
<span style="color:purple">IL_000c:</span> <span style="color:blue">ldloc.0</span>
<span style="color:purple">IL_000d:</span> <span style="color:blue">ldc.i4.1</span>
<span style="color:purple">IL_000e:</span> <span style="color:blue">add</span>
<span style="color:purple">IL_000f:</span> <span style="color:blue">stloc.0</span>
<span style="color:purple">IL_0010:</span> <span style="color:blue">ldloc.0</span>
<span style="color:purple">IL_0011:</span> <span style="color:blue">ldarg.1</span>
<span style="color:purple">IL_0012:</span> <span style="color:blue">ldlen</span>
<span style="color:purple">IL_0013:</span> <span style="color:blue">conv.i4</span>                   <span style="color:green"> // i < Length</span>
<span style="color:purple">IL_0014:</span> <span style="color:blue">blt.s</span> IL_0004
</code></pre>

x86 コード:

<pre class="source">
<code><span style="color:purple">L0008:</span> <span style="color:blue">xor</span> esi, esi
<span style="color:purple">L000a:</span> <span style="color:blue">mov</span> ebx, [edi+0x4]
<span style="color:purple">L000d:</span> <span style="color:blue">test</span> ebx, ebx
<span style="color:purple">L000f:</span> <span style="color:blue">jle</span> L001f
<span style="color:purple">L0011:</span> <span style="color:blue">mov</span> ecx, [edi+esi*4+0x8] <span style="color:green"> ; array[i]</span>
<span style="color:purple">L0015:</span> <span style="color:blue">call</span> WriteLine(Int32)
<span style="color:purple">L001a:</span> <span style="color:blue">inc</span> esi
<span style="color:purple">L001b:</span> <span style="color:blue">cmp</span> ebx, esi             <span style="color:green"> ; i < Length</span>
<span style="color:purple">L001d:</span> <span style="color:blue">jg</span> L0011
</code></pre>

`for` ステートメント中の `i < Length` 相当のコードはありますが、
その他の比較はありません。
要するに、単品だと`array[i]`のところに挟まっていた範囲チェックが、このループでは消えています。

これは JIT が行っている最適化で、要するに、

- 何もしなければ、`array[i]` のところに暗黙的な範囲チェックを追加する
- 明示的な範囲チェックがあれば、余計な範囲チェックの追加はしない

という挙動になります。
なので、安全性は保たれつつ、ループの速度は落としません。

## foreach 最適化

次に以下のコードを考えます。
先ほどの `for` を使ったコードとやっていることは全く一緒です。
配列の全要素の列挙。

<pre class="source" title="">
<code><span class="reserved">void</span> M(<span class="reserved">int</span>[] array)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
    {
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

こいつは以下のようにコンパイルされます。

IL:

<pre class="source">
<code><span style="color:purple">IL_0006:</span> <span style="color:blue">ldloc.0</span>
<span style="color:purple">IL_0007:</span> <span style="color:blue">ldloc.1</span>
<span style="color:purple">IL_0008:</span> <span style="color:blue">ldelem.i4</span>                 <span style="color:green"> // array[i]</span>
<span style="color:purple">IL_0009:</span> <span style="color:blue">call</span> void WriteLine(int32)
<span style="color:purple">IL_000e:</span> <span style="color:blue">ldloc.1</span>
<span style="color:purple">IL_000f:</span> <span style="color:blue">ldc.i4.1</span>
<span style="color:purple">IL_0010:</span> <span style="color:blue">add</span>
<span style="color:purple">IL_0011:</span> <span style="color:blue">stloc.1</span>
<span style="color:purple">IL_0012:</span> <span style="color:blue">ldloc.1</span>
<span style="color:purple">IL_0013:</span> <span style="color:blue">ldloc.0</span>
<span style="color:purple">IL_0014:</span> <span style="color:blue">ldlen</span>
<span style="color:purple">IL_0015:</span> <span style="color:blue">conv.i4</span>                   <span style="color:green"> // i < Length</span>
<span style="color:purple">IL_0016:</span> <span style="color:blue">blt.s</span> IL_0006
</code></pre>

`ldloc` (ローカル変数読み込み)の後ろの番号とかが違うだけで、
他は先ほどの `for` のコードと全く同じです。
(要するに、「変数名が違うけど同じロジック」程度の差です。)

`GetEnumerator`とか`MoveNext`、`Current`は一切出てきません。
代わりにインデックスの `++i` とか `array[i]` 相当のコードが出てきます。

要するに、C# コンパイラーは、配列の `foreach` を見たら `for (var i ...` 相当のコードに変換します。

## 配列の一部分を列挙

配列全体の列挙に対して結構よい最適化がかかっていることはわかりました。
次は、一部分だけ列挙することを考えます。

<pre class="source" title="">
<code><span class="reserved">void</span> M(<span class="reserved">int</span>[] array, <span class="reserved">int</span> start, <span class="reserved">int</span> count)
{
    <span class="reserved">var</span> end = start + count;
    <span class="reserved">for</span> (<span class="reserved">var</span> i = start; i &lt; end; ++i)
    {
        <span class="reserved">var</span> x = array[i];
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

これを同じようにコンパイルすると…
ループ近辺だけ抜き出すと以下のようになります。

x86 コード:

<pre class="source">
<code><span style="color:purple">L0017:</span> <span style="color:blue">mov</span> eax, [edi+0x4]
<span style="color:purple">L001a:</span> <span style="color:blue">mov</span> [ebp-0x10], eax
<span style="color:purple">L001d:</span> <span style="color:blue">mov</span> eax, [ebp-0x10]
<span style="color:purple">L0020:</span> <span style="color:blue">cmp</span> esi, eax       <span style="color:green"> ; ここの比較は array[i] に対して暗黙的に追加されるもの</span>
<span style="color:purple">L0022:</span> <span style="color:blue">jae</span> L003a          <span style="color:green"> ; 例外を投げるコードへのジャンプ</span>
<span style="color:purple">L0024:</span> <span style="color:blue">mov</span> ecx, [edi+esi*4+0x8]
<span style="color:purple">L0028:</span> <span style="color:blue">call</span> System.Console.WriteLine(Int32)
<span style="color:purple">L002d:</span> <span style="color:blue">inc</span> esi
<span style="color:purple">L002e:</span> <span style="color:blue">cmp</span> esi, ebx       <span style="color:green"> ; ここの比較は i < end</span>
<span style="color:purple">L0030:</span> <span style="color:blue">jl</span> L001d           <span style="color:green"> ; これはループを抜ける・抜けないの分岐</span>
</code></pre>

さすがに、`array.Length` 以外のものまで見て最適化は掛けてくれないみたいです。
ちなみに、事前に `start`、`end`の範囲チェックをしてもダメ。

## そこで `Span<T>`

C# 7.2 では、[`Span<T>`](../../../../study/csharp/resource/span.md)がらみの対応がいろいろ入ったわけですが、このタイミングで、`Span<T>`に対する列挙の最適化も入っています。

まず、配列同様、`Span<T>`に対する`foreach`は`for`に最適化されます。
例えば、以下の2つのメソッドはほぼ同じ IL にコンパイルされます。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">void</span> M1(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; array)
{
    <span class="reserved">for</span> (<span class="reserved">var</span> i = 0; i &lt; array.Length; ++i)
    {
        <span class="reserved">var</span> x = array[i];
        <span class="type">Console</span>.WriteLine(x);
    }
}
 
<span class="reserved">public</span> <span class="reserved">void</span> M2(<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; array)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array)
    {
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

また、JIT 時の最適化で、「暗黙の範囲チェック」も消えてくれるようです。
要するに、先ほどの `for (var i = start; i < end; ++i)` なループよりも、
以下のような、`Span<T>`を介したコードの方が最適化がかかりやすいです。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">void</span> M2(<span class="reserved">int</span>[] array, <span class="reserved">int</span> start, <span class="reserved">int</span> count)
{
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> array.AsSpan(start, count))
    {
        <span class="type">Console</span>.WriteLine(x);
    }
}
</code></pre>

ちなみに、今のところこういう最適化がかかるのは配列と `Span<T>` だけです。
`Span<T>` だけ特別扱いするのもちょっと嫌な話で、
もっと汎用的に、所定のパターンを満たした型なら `foreach` を `for (var i ...` に変換できるような仕組みも一応検討はされています。
(`Span<T>`以外に対する需要はそこまで高くないので、優先度は低め。)
