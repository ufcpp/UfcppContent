---
title: "foreach の掛け方いろいろ"
source_url: "https://ufcpp.net/blog/2018/12/howtoenumerate/"
content_type: "BlogEntry"
published_at: "2018-12-20T10:06:39"
updated_at: "2018-12-20T10:06:39"
tags: []
umbraco_id: 2202
parent_id: 2177
sort_order: 19
aliases: []
---

# foreach の掛け方いろいろ

[IEnumerator の別実装](../fastenumerator/index.md)で、
インターフェイス越しの `foreach` には仮想呼び出しのコストが結構掛かっているという話を書きました。
(そちらでの主題は「なので、`MoveNext`/`Current`の2つに分かれているのはちょっともったいない」という話でした。
もちろん、それを気にしないといけないのは大体パフォーマンス最優先のエクストリームな状況だけです。)

あと、[配列のインデクサー](../arrayindexer/index.md)では、配列と[`Span<T>`構造体](../../../../study/csharp/resource/span.md)の列挙には C# のレベルでも JIT のレベルでも最適化が掛かっていて、かなり速いという話をしました。

今回はその辺りを踏まえて、列挙の仕方をいろいろ比較。

参考コード: [ArrayEnumeration](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/PerformanceTips/ArrayEnumeration)

## 内部的には配列なコレクション

`List<T>` (`System.Collections.Generic` 名前空間)とか、
`ImmutableArray<T>` (`System.Collections.Immutable` 名前空間)とか、
内部的に配列を持っていて、その上に何か機能を重ねたり(あるいは逆に書き換えを制限したり)している型は結構あります。
今回はその手の型の列挙について考えます。

とりあえず以下のような型を用意。参考にするために、配列を生列挙するコードも書いておきます。

<pre class="source" title="配列の生列挙">
<code><span class="reserved">using</span> BenchmarkDotNet.Attributes;
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 比較のために生列挙をしたいので public (本来は不要というかむしろダメ)</span>
    <span class="reserved">public</span> <span class="reserved">readonly</span> <span class="type">T</span>[] Array;
    <span class="reserved">public</span> ArrayWrapper(<span class="type">T</span>[] array) =&gt; Array = array;
}
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">ArrayEnumerationBenchmark</span>
{
    <span class="reserved">public</span> <span class="type">ArrayWrapper</span>&lt;<span class="reserved">int</span>&gt; _array;

    <span class="comment">// 比較のための生列挙。</span>
    [<span class="type">Benchmark</span>(Baseline = <span class="reserved">true</span>)]
    <span class="reserved">public</span> <span class="reserved">int</span> RawEnumeration()
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> _array.Array) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

とりあえず、結果:

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |

## IEnumerable の実装

`foreach`で使いたいというのが主題なので、とりあえず先ほどの型に `IEnumerable<T>` インターフェイスを実装してみます。

とはいえ、インターフェイスを介した `GetEnumerator`/`MoveNext`/`Current` はちょっとオーバーヘッドが掛かるので、以下のような作りにします。
(`List<T>` なんかはまさにこの作りになっています。)

<pre class="source" title="IEnumerable 化">
<code><span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 専用の型を作って、それを具象型のまま公開する</span>
    <span class="reserved">public</span> <span class="type">Enumerator</span> GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">Enumerator</span>(Array);
 
    <span class="comment">// インターフェイスは明示的実装にして別実装</span>
    <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt; <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;.GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">EnumeratorObject</span>(Array);
    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span>.GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">EnumeratorObject</span>(Array);
}
</code></pre>

### 専用実装(構造体)

まずは専用実装の方。
配列の全要素を列挙するような `IEnumerator<T>` 実装は以下のようになります。
無駄なアロケーションが発生しないように構造体製。

<pre class="source" title="構造体で専用実装">
<code><span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// 「仮想呼び出しは遅い」ということがわかっているわけで、</span>
    <span class="comment">// こんな感じで具象型を返す GetEnumerator を作った方が高速。</span>
    <span class="comment">// 構造体にした方が最適化が効く。</span>
    <span class="reserved">public</span> <span class="type">Enumerator</span> GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">Enumerator</span>(Array);
 
    <span class="reserved">public</span> <span class="reserved">struct</span> <span class="type">Enumerator</span> : <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt;
    {
        <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">T</span>[] _array;
        <span class="reserved">private</span> <span class="reserved">int</span> _i;
        <span class="reserved">internal</span> Enumerator(<span class="type">T</span>[] array) =&gt; (_array, _i) = (array, -1);
 
        <span class="reserved">public</span> <span class="type">T</span> Current =&gt; _array[_i];
        <span class="reserved">public</span> <span class="reserved">bool</span> MoveNext() =&gt; ((<span class="reserved">uint</span>)++_i) &lt; (<span class="reserved">uint</span>)_array.Length;
        <span class="comment">// 残りは省略</span>
    }
}
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">ArrayEnumerationBenchmark</span>
{
    <span class="comment">// 構造体の Enumerator 越しの列挙</span>
    <span class="comment">// 構造体で返してるとほんとにきっちり最適化が効くみたいで、</span>
    <span class="comment">// ほぼ配列生列挙と同じ速度が出る。</span>
    [<span class="type">Benchmark</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> StructEnumeration()
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> _array) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

これを使って `foreach (var x in _array)` とすると、
`MoveNext`も`Current`もインライン展開されて、
最適化でほとんど配列の生列挙と同じコードに展開されます。
誤差の範囲内で生列挙と同じ速度が出ます。


|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |

### インターフェイス実装

構造体実装なものだけでは `IEnumerable<T>` インターフェイスの要件を満たさないので、
別途明示的実装を足します。

このとき、実装要件を満たすだけなら `IEnumerator<T> IEnumerable<T> GetEnumerator() => GetEnumerator();` (構造体実装の `GetEnumerator` を素通しするだけ)でも構いません。
ただ、構造体をインターフェイス化して使うとかえって遅くて、
少しでもパフォーマンスを上げたりならクラスで作り直す方がよかったりします。

<pre class="source" title="インターフェイス実装">
<code><span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt; : <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;
{
    <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt; <span class="type">IEnumerable</span>&lt;<span class="type">T</span>&gt;.GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">EnumeratorObject</span>(Array);
    <span class="type">IEnumerator</span> <span class="type">IEnumerable</span>.GetEnumerator() =&gt; <span class="reserved">new</span> <span class="type">EnumeratorObject</span>(Array);
 
    <span class="comment">// 構造体の Enumerator と中身は全く同じで、ただクラスになってるだけ。</span>
    <span class="comment">// 構造体をインターフェイス越しに返すとかえって遅くなるので、こんなクラスが別途必要に…</span>
    <span class="reserved">public</span> <span class="reserved">class</span> <span class="type">EnumeratorObject</span> : <span class="type">IEnumerator</span>&lt;<span class="type">T</span>&gt;
    {
        <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">T</span>[] _array;
        <span class="reserved">private</span> <span class="reserved">int</span> _i;
        <span class="reserved">internal</span> EnumeratorObject(<span class="type">T</span>[] array) =&gt; (_array, _i) = (array, -1);
 
        <span class="reserved">public</span> <span class="type">T</span> Current =&gt; _array[_i];
        <span class="reserved">public</span> <span class="reserved">bool</span> MoveNext() =&gt; ((<span class="reserved">uint</span>)++_i) &lt; (<span class="reserved">uint</span>)_array.Length;
 
        <span class="reserved">object</span> <span class="type">IEnumerator</span>.Current =&gt; Current;
        <span class="reserved">public</span> <span class="reserved">void</span> Dispose() { }
        <span class="reserved">public</span> <span class="reserved">void</span> Reset() =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">NotImplementedException</span>();
    }
}
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">ArrayEnumerationBenchmark</span>
{
    <span class="comment">// インターフェイス越し列挙になるように、IEnumerable&lt;T&gt; にキャストして使ってる。</span>
    <span class="comment">// びっくりするくらい遅い。</span>
    <span class="comment">// StructEnumeration とかに比べて10倍遅い。</span>
    [<span class="type">Benchmark</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> InterfaceEnumeration()
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> (<span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt;)_array) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

構造体/具象型実装が配列生列挙とそん色ないのに対して、
こいつは10倍以上遅いです。
それでも、別途クラスで実装した方がちょっとだけマシ。

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |

## 出来合いの型

おまけで、出来合いの型を被せて返すのもやっておきます。

### ReadOnlyCollection

配列を生で返したくない状況の1つが、書き換えを認めたくない場合です。
そういう場合、
`ReadOnlyCollection<T>` クラス(`System.Collections.ObjectModel` 名前空間)を使ったりします。

ただ、このクラス、`IList<T>` 向けなので、
配列だけでいいときには余計(繰り返しますが、インターフェイス越しは遅い)ですし、
.NET Framework 2.0 時代からあってパフォーマンスへの考慮はあんまりない型です。
要するに、遅い…

<pre class="source" title="ReadOnlyCollection 越しの列挙">
<code><span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">ReadOnlyCollection</span>&lt;<span class="type">T</span>&gt; AsReadOnlyCollection() =&gt; <span class="reserved">new</span> <span class="type">ReadOnlyCollection</span>&lt;<span class="type">T</span>&gt;(Array);
}
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">ArrayEnumerationBenchmark</span>
{
    <span class="comment">// ReadOnlyCollection&lt;T&gt; 列挙。</span>
    <span class="comment">// InterfaceEnumeration 以上に遅い。とにかく遅い。</span>
    <span class="comment">// ReadOnlyCollection&lt;T&gt; は内部的に IList&lt;T&gt; 越しに配列アクセスするので、それがほんとに遅い。</span>
    [<span class="type">Benchmark</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> ReadOnlyCollectionEnumeration()
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> _array.AsReadOnlyCollection()) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |
|  ReadOnlyCollectionEnumeration | 5,199.8 ns | 21.591 ns | 20.1960 ns | 13.48 |    0.07 |

### Span

まあ、今なら、特に .NET Core を使えるのであれば、
`ReadOnlySpan<T>` 構造体 (`System` 名前空間)を使うのがいいと思います。
`Span<T>` と同様最適化が掛かるので、
書き換えを防止しつつ、配列の生列挙とそん色ない速度が出ます。

<pre class="source" title="ReadOnlySpan 越しの列挙">
<code><span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">ArrayWrapper</span>&lt;<span class="type">T</span>&gt;
{
    <span class="comment">// インデクサーも使いたいとき用、その2。</span>
    <span class="comment">// Span&lt;T&gt; を介してみる。</span>
    <span class="comment">// パフォーマンスに焦点が当たってた .NET Core 2.1 世代の型だけあって、かなり速い。</span>
    <span class="reserved">public</span> <span class="type">ReadOnlySpan</span>&lt;<span class="type">T</span>&gt; AsSpan() =&gt; Array;
}
 
<span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">ArrayEnumerationBenchmark</span>
{
    <span class="comment">// Span&lt;T&gt; 列挙</span>
    <span class="comment">// こいつも配列生列挙とほぼ同じ性能。速い。</span>
    [<span class="type">Benchmark</span>]
    <span class="reserved">public</span> <span class="reserved">int</span> SpanEnumeration()
    {
        <span class="reserved">var</span> sum = 0;
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> _array.AsSpan()) sum += x;
        <span class="reserved">return</span> sum;
    }
}
</code></pre>

|                         Method |       Mean |     Error |     StdDev | Ratio | RatioSD |
| ------------------------------ |-----------:|----------:|-----------:|------:|--------:|
|                 RawEnumeration |   385.6 ns |  1.031 ns |  0.9646 ns |  1.00 |    0.00 |
|              StructEnumeration |   386.3 ns |  1.100 ns |  0.9751 ns |  1.00 |    0.00 |
|           InterfaceEnumeration | 4,407.3 ns | 14.790 ns | 13.8350 ns | 11.43 |    0.05 |
|  ReadOnlyCollectionEnumeration | 5,199.8 ns | 21.591 ns | 20.1960 ns | 13.48 |    0.07 |
|                SpanEnumeration |   385.0 ns |  1.612 ns |  1.5079 ns |  1.00 |    0.00 |

## `Span<T>` の利用率アップ

ここまでの説明の通り下手すると10倍性能が違ったりするので、
.NET Core 2.1 が出て以降、
`Span<T>`や`ReadOnlySpan<T>`を引数に取るAPIが増えていたりします。
`IEnumerable<T>`や`IList<T>`が減って。

そうなると、既存のコレクションに対しても、「可能なもの(内部的に配列とか連続したデータになってるやつ)は`Span`で取りたい」という要求がかなり高くなっています。

が、既存の型を改修してもらえるまで待てないという人も… 
`Unsafe` な手段で無理やり中身の配列を取得して、
無理やり `Span` にしてしまったり…

- [Add Span and Memory conversion methods for ImmutableArray](https://github.com/dotnet/roslyn/pull/31785)

過渡的な手段とはいえ、結構邪悪です。
以下のようなコード。

<pre class="source" title="ImmurableArray を無理やり配列に変換">
<code>[<span class="type">StructLayout</span>(<span class="type">LayoutKind</span>.Sequential)]
<span class="reserved">private</span> <span class="reserved">struct</span> <span class="type">ImmutableArrayProxy</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">internal</span> <span class="type">T</span>[] MutableArray;
}
 
<span class="reserved">internal</span> <span class="reserved">static</span> <span class="type">T</span>[] DangerousGetUnderlyingArray&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">ImmutableArray</span>&lt;<span class="type">T</span>&gt; array)
     =&gt; <span class="type">Unsafe</span>.As&lt;<span class="type">ImmutableArray</span>&lt;<span class="type">T</span>&gt;, <span class="type">ImmutableArrayProxy</span>&lt;<span class="type">T</span>&gt;&gt;(<span class="reserved">ref</span> array).MutableArray;
</code></pre>

また、`Span`を使うと、中身を参照で外に漏らしちゃうことになるので、
ちょっと変な挙動をすることがあります。
以下のようなコードには注意を。

<pre class="source" title="Span で中身を返す場合の注意点">
<code><span class="reserved">using</span> System;
 
<span class="comment">// System.Collections.Generic.List&lt;T&gt; と同じような実装 + AsSpan</span>
<span class="reserved">class</span> <span class="type">List</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="type">T</span>[] _buffer;
    <span class="reserved">private</span> <span class="reserved">int</span> _count;
    <span class="reserved">public</span> List(<span class="reserved">int</span> capacity) =&gt; _buffer = <span class="reserved">new</span> <span class="type">T</span>[capacity];
 
    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _buffer[index];
    <span class="reserved">public</span> <span class="type">ReadOnlySpan</span>&lt;<span class="type">T</span>&gt; AsSpan() =&gt; _buffer.AsSpan(0, _count);
 
    <span class="reserved">public</span> <span class="reserved">void</span> Add(<span class="type">T</span> item)
    {
        <span class="reserved">if</span>(_count == _buffer.Length)
        {
            <span class="reserved">var</span> newBuffer = <span class="reserved">new</span> <span class="type">T</span>[_buffer.Length * 2];
            _buffer.AsSpan().CopyTo(newBuffer);
            _buffer = newBuffer;
        }
        _buffer[_count++] = item;
    }
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> list = <span class="reserved">new</span> <span class="type">List</span>&lt;<span class="reserved">int</span>&gt;(2);
        list.Add(1);
        list.Add(2);
        <span class="comment">// この時点で容量満杯</span>
 
        <span class="comment">// Span 取得してから…</span>
        <span class="reserved">var</span> span = list.AsSpan();
 
        list.Add(3);  <span class="comment">// Add で内部バッファーの再確保が発生</span>
        list[0] = 99; <span class="comment">// 新しいバッファーへの書き込み</span>
 
        <span class="type">Console</span>.WriteLine(span[0]); <span class="comment">// 古いバッファーを参照してるので 1 のまま</span>
        <span class="type">Console</span>.WriteLine(list[0]); <span class="comment">// 新しいバッファーを参照してるので 99</span>
    }
}
</code></pre>

## まとめ

- `GetEnumerator` を実装するなら、専用の構造体をまず考える
- それとは別に、`IEnumerable<T>.GetEnumerator` を明示的実装
- `Span<T>`/`ReadOnlySpan<T>` 速い
  - ので、パフォーマンス的には `Span` を使いたい
  - でも、参照が外に漏れるので注意
