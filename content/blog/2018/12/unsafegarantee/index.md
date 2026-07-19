---
title: "Unsafe クラス(保証のある利用例)"
source_url: "https://ufcpp.net/blog/2018/12/unsafegarantee/"
content_type: "BlogEntry"
published_at: "2018-12-28T09:40:24"
updated_at: "2018-12-28T09:40:24"
tags: []
umbraco_id: 2211
parent_id: 2177
sort_order: 28
aliases: []
---

# Unsafe クラス(保証のある利用例)

「[Unsafe クラス(保証外)](../unsafenogarantee/index.md)」ではわざわざ動作保証のない相当に邪悪なコードを紹介しました。

とはいえ、別に `Unsafe`クラスを使った瞬間に動作保証がなくなるわけではありません。
単に、開発者の裁量に任されるというだけで、正しく使えば問題は起こしません。

例えば、`Unsafe.As`メソッドは型チェックをせずに型を強制変換するメソッドですが、
最初から(`As`メソッドよりも前に予めチェックして)型がわかっているなら何も問題ありません。

## Union 型

例として、「`A` または `B` のどちらか」を表す型を作ってみましょう。
単なる例にそんなに凝っても仕方がないので、今回は「`string` または `char[]`」で作ります。

### is 演算子実装

素直に実装すると以下のようになります。

<pre class="source" title="object に対して is で型判定">
<code><span class="reserved">using</span> System;
 
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">StringOrCharArray</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">object</span> _value;

    <span class="reserved">public</span> StringOrCharArray(<span class="reserved">string</span> s) =&gt; _value = s;
    <span class="reserved">public</span> StringOrCharArray(<span class="reserved">char</span>[] array) =&gt; _value = array;

    <span class="reserved">public</span> <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; Span
        =&gt; _value <span class="reserved">is</span> <span class="reserved">string</span> s ? s.AsSpan() :
        _value <span class="reserved">is</span> <span class="reserved">char</span>[] a ? a.AsSpan() :
        <span class="reserved">default</span>;
}
</code></pre>

見てほしいのは`Span`プロパティの部分です。
この中で使っている[`is`演算](../../../../study/csharp/datatype/typeswitch.md#is)は、
実際のところ、以下のような `as` + null チェックと等価です。

<pre class="source" title="is は as + null チェック">
<code><span class="reserved">var</span> s = _value <span class="reserved">as</span> <span class="reserved">string</span>;
<span class="reserved">if</span> (s != <span class="reserved">null</span>) ...
</code></pre>

で、`as` 演算子は IL 的には [isinst 命令](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.isinst)になってます。

isinst 命令は要は実行時型情報を調べる命令です。
実行時型情報と言っても、動的コード生成をしない(単に型を調べるだけ)ならそこまで高コストではありません。
なので、動的コード生成みたいに「静的なコードに比べて2桁遅い」みたいな事態にはなりません。

## 型弁別用の enum 値

しかし、今日は「ちょっとのコスト」も避けようという話なので、
この isinst 命令を消すことを考えます。

`object` 型のフィールドに加えて、型弁別用の enum 値を別途持ってみることにします。
ただ、素直な実装をしてしまうと「コスト避け」の試みは失敗します。

<pre class="source" title="型弁別用の enum を追加">
<code><span class="reserved">using</span> System;
 
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">StringOrCharArray</span>
{
    <span class="reserved">public</span> Discriminator Type { <span class="reserved">get</span>; }
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">object</span> _value;
 
    <span class="reserved">public</span> StringOrCharArray(<span class="reserved">string</span> s) =&gt; (Type, _value) = (Discriminator.String, s);
    <span class="reserved">public</span> StringOrCharArray(<span class="reserved">char</span>[] array) =&gt; (Type, _value) = (Discriminator.CharArray, array);
 
    <span class="reserved">public</span> <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; Span
    {
        <span class="reserved">get</span>
        {
            <span class="comment">// せっかく Type を見て switch してるのに…</span>
            <span class="reserved">switch</span> (Type)
            {
                <span class="reserved">default</span>: <span class="reserved">return</span> <span class="reserved">default</span>;
                <span class="comment">// この2行のキャストが余計。</span>
                <span class="reserved">case</span> Discriminator.String: <span class="reserved">return</span> ((<span class="reserved">string</span>)_value).AsSpan();
                <span class="reserved">case</span> Discriminator.CharArray: <span class="reserved">return</span> ((<span class="reserved">char</span>[])_value).AsSpan();
            }
        }
    }
}
</code></pre>

enum 値を見て switch していますが、分岐の先で結局キャストしています。
キャストの方は [caltclass 命令](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.opcodes.castclass)になるんですが、
この命令は内部的に isinst 命令と大差ないみたいで、実行時間もほとんど同じです。

これがこのクラスの失敗理由で、
「せっかく事前に enum 値で型を判定してるのに、castclass 命令で改めて型チェックをしてて、単に2重の負担がかかってるだけ」
という状態になっています。
結果的に、先ほどの is 演算子実装よりもちょっとだけよりも遅くなります。

## Unsafe 実装

ということで、`Unsafe`。
先ほどの `Span` プロパティを以下のように書き換えます。

<pre class="source" title="Unsafe.As はチェックをすっ飛ばすので高速">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.CompilerServices;
 
<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">StringOrCharArray</span>
{
    <span class="comment">// 先ほどと同じところは割愛</span>
 
    <span class="reserved">public</span> <span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; Span
    {
        <span class="reserved">get</span>
        {
            <span class="comment">// せっかく Type を見て switch してるんだから</span>
            <span class="reserved">switch</span> (<span class="type">Type</span>)
            {
                <span class="reserved">default</span>: <span class="reserved">return</span> <span class="reserved">default</span>;
                <span class="comment">// キャストを Unsafe.As で置き換えれば高速。</span>
                <span class="reserved">case</span> Discriminator.String: <span class="reserved">return</span> <span class="type">Unsafe</span>.As&lt;<span class="reserved">object</span>, <span class="reserved">string</span>&gt;(<span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef(_value)).AsSpan();
                <span class="reserved">case</span> Discriminator.CharArray: <span class="reserved">return</span> <span class="type">Unsafe</span>.As&lt;<span class="reserved">object</span>, <span class="reserved">char</span>[]&gt;(<span class="reserved">ref</span> <span class="type">Unsafe</span>.AsRef(_value)).AsSpan();
            }
        }
    }
}
</code></pre>

`Unsafe.As`メソッドは型チェックをすっとばしてるので高速です。
名前通り unsafe ではありますが、今回の場合、事前に enum 値で型を調べているので問題は起こしません。

この実装であれば、当初目的である isinst 命令を避けることができます。
前述の通り桁違いな速度差が出るわけではないんですが、
元の is 演算子実装より数割程度速くなります。

ベンチマーク: [DiscriminatedUnion](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/PerformanceTips/DiscriminatedUnion)

## フィールドが増えてる

今回の例では、型チェックの負担は減りますが、
代わりにフィールドが1つ増えています。
構造体サイズも倍になってしまい、コピーのコストが発生してしまいます。
(`Span`プロパティのアクセスよりも、変数のコピーの頻度が圧倒的に多い場合、むしろ遅くなる可能性があります。)

ということで使いどころには注意が必要です。
ただ、実装によってはこのコストは避けれます。
例えば、標準ライブラリ中の`Memory<T>`構造体(`System`名前空間)は以下のような構造になっています。

<pre class="source" title="Memory&lt;T&gt; 構造体の中身">
<code><span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type">Memory</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">object</span> _object;
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> _index;
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">int</span> _length;
}
</code></pre>

`Memory`構造体は、以下のような前提で、フィールドを増やさずに isinst 命令を避けたりしています。

- `_index`(配列の開始インデックス)も`_length`(そこから何要素抜き出すか)も、負にはならない
- 負にならないのなら、最上位ビットが 1 になることはあり得ない
- その最上位ビットを、型弁別用に使う
