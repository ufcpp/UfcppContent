---
title: "[雑記] エントリーポイント"
source_url: "https://ufcpp.net/study/csharp/structured/miscentrypoint/"
content_type: "Article"
published_at: "2017-06-11T00:00:00"
updated_at: "2017-09-17T17:13:33"
tags: []
umbraco_id: 2072
parent_id: 1217
sort_order: 6
aliases:
  - "/csharp/structured/miscentrypoint/"
---

# \[雑記\] エントリーポイント

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

C# では通常、1つのプログラムは複数の C# ソースコードからなり、そのソースコード中には複数の関数が含まれています。
その、多数ある関数の中で、プログラム起動時に最初に呼ばれるものを<strong id="entry-point" class="keyword">エントリーポイント</strong>(entry point: 入場地点)と呼びます。

「[C# のプログラムの基本構造](../start/st_basis.md)」で例を出したように、
C# では、`Main`という名前の関数が自動的にエントリーポイントになります。

(「[関数](st_function.md)」内でも補足していますが、
正確にいうと、`Main`という名前のメソッドがエントリーポイントになります。)

### <a id="sec-generated-title-2"></a> <a id="cs-script"></a>[補足] C# スクリプト

[スクリプト実行](../cheatsheet/apscripting.md)の場合は関数で囲わなくてもどこにでも処理を書けます。
`Main`関数も不要です。

## <a id="sec-generated-title-3"></a> <a id="Main"></a>Main の引数、戻り値

`Main`の引数と戻り値は、以下のいずれかである必要があります。
これ以外のオーバーロードはエントリーポイントになりません。

<pre class="source" title="Main の引数と戻り値">
<code><span class="reserved">static</span> <span class="reserved">int</span> Main()
<span class="reserved">static</span> <span class="reserved">int</span> Main(<span class="reserved">string</span>[] args)
<span class="reserved">static</span> <span class="reserved">void</span> Main()
<span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
</code></pre>

(ただし、後述しますが、C# 7.1 からは戻り値として`Task`クラスが使えるようになりました。)

引数を持っている場合、引数にはコマンドライン引数が渡ってきます。
(引数なし版は、コマンドライン引数を受け取る必要がない時に使います。)

また、戻り値はプログラムの終了コードを返します。
[Windows の場合](https://msdn.microsoft.com/ja-jp/library/ms194959(v=vs.100).aspx)は0が正常終了、1が部分的な成功、…などの意味があるようです。
戻り値なし版の場合は常に0(正常終了)扱いです。

## <a id="sec-generated-title-4"></a> <a id="no-main"></a>Main がないタイプのプロジェクト

GUI アプリや Web アプリでは、`Main`関数を書かない場合があります。
この場合、以下のいずれかです。

- 他のプログラムから呼び出される。どの関数から呼び出すかは、呼び出し元次第
- 開発者に見えないところで自動的に`Main`が作られている

例えば、ASP.NETの場合は前者、WPF アプリの場合は後者になります。

## <a id="sec-generated-title-5"></a> <a id="explicit-entry-point"></a>エントリーポイントの指定

1つのプログラムの中に複数のクラスがあって、
複数のクラスの中に`Main`関数がある場合、そのままではエントリーポイントを決定できず、コンパイル エラーになります。

この場合、どの`Main`関数を使うかをオプション指定できます。

参考:

- [方法 : アプリケーションのスタートアップ オブジェクトを変更する](https://msdn.microsoft.com/ja-jp/library/17k74w0c.aspx)
- [/main (C# Compiler Options](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/main-compiler-option)

## <a id="sec-generated-title-6"></a> <a id="async-main"></a>非同期 Main

<h5 class="version version7_1">Ver. 7.1</h5>

C# 7.1で、以下のように、`Main`関数の戻り値に`Task`クラス(`System.Threading.Tasks`名前空間)を使えるようになりました。

<pre class="source" title="Main関数の引数と戻り値(C# 7.1 から)">
<code><span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; Main()
<span class="reserved">static</span> <span class="type">Task</span>&lt;<span class="reserved">int</span>&gt; Main(<span class="reserved">string</span>[] args)
<span class="reserved">static</span> <span class="type">Task</span> Main()
<span class="reserved">static</span> <span class="type">Task</span> Main(<span class="reserved">string</span>[] args)
</code></pre>

もちろん、[非同期メソッド](../async/sp5_async.md)を使えるようにするためです。
例えば以下のような`Main`関数が、ちゃんとエントリーポイントとして認識されます。

<pre class="source" title="非同期Mainの例">
<code><span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> Main()
{
    <span class="reserved">for</span> (<span class="reserved">int</span> i = 10; i &gt; 0; i--)
    {
        <span class="type">Console</span>.WriteLine(i);
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
    }

    <span class="type">Console</span>.WriteLine(<span class="string">"done."</span>);
}
</code></pre>

### <a id="sec-generated-title-7"></a> <a id="internal-async-main"></a>非同期 Main の仕組み

ちなみに、この機能は、コンパイラーが通常の(`void`/`int`戻り値の)エントリーポイントを別途自動生成することで実現しています。
例えば、先ほどの例のように、`Task Main()`を書くと、追加で以下のような関数が作られ、これが実際のエントリーポイントとして機能します。

<pre class="source" title="非同期Mainから自動生成される通常のMain">
<code><span class="comment">// 実際には &lt;Main&gt; というような、C# で本来使えない名前で生成される</span>
<span class="reserved">static</span> <span class="reserved">void</span> _Main_(<span class="reserved">string</span>[] args)
{
    Main().GetAwaiter().GetResult();
}
</code></pre>

中身は`GetAwaiter().GetResult()`を呼んでいるだけです。

### <a id="sec-generated-title-8"></a> <a id="compatibility"></a>通常の Main がすでにある場合

非同期 Main の仕様は C# 7.1 で追加されたものです。
そのため、これまでに書いたコードの中にすでに、エントリーポイントにするつもりがない `Task Main()` が含まれている場合に対する考慮が必要です。

C# 7.1 では、通常の(`void`/`int`戻り値の)`Main`関数がある場合、そちらだけをエントリーポイント扱いします。

<pre class="source" title="Main の優先度">
<code><span class="reserved">static</span> <span class="reserved">void</span> Main(<span class="reserved">string</span>[] args)
{
    <span class="type">Console</span>.WriteLine(<span class="string">"こちらがエントリーポイント扱い"</span>);
}

<span class="reserved">static</span> <span class="reserved">async</span> <span class="type">Task</span> Main()
{
    <span class="type">Console</span>.WriteLine(<span class="string">"void Main(string[]) がある限り、こちらは呼ばれない"</span>);
}
</code></pre>
