---
title: "C# 上で PowerShell スクリプトを実行"
source_url: "https://ufcpp.net/study/powershell/interop/interop/"
content_type: "Article"
published_at: "2009-01-14T00:00:00"
updated_at: "2018-03-25T09:16:29"
tags: []
umbraco_id: 1593
parent_id: 1592
sort_order: 0
aliases:
  - "/powershell/interop.html"
  - "/powershell/interop/interop/"
  - "/study/powershell/interop.html"
---

# C# 上で PowerShell スクリプトを実行

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# 上で PowerShell スクリプトを実行する方法を説明します。
その際、C# から PowerShell に引数を渡し、
PowerShell からの戻り値を C# で受け取る方法も説明します。


## <a id="sec-generated-title-2"></a> <a id="prepare"></a>下準備

PowerShell の機能を .NET 言語から利用するためには、
System.Management.Automation.dll を参照する必要があります。
この DLL は、Windows SDK をインストールすると、Program Files の下にある以下のパスに配置されます。

<blockquote markdown="1">
%PROGRAMFILES%\Reference Assemblies\Microsoft\WindowsPowerShell\v1.0

</blockquote>
あるいは、PowerShell だけインストールして、GAC（Global Assembly Cache）から取りだす方法もあるようです
（参考： [コマンドレットの作成方法 [C#と諸々]](http://csharper.blog57.fc2.com/blog-entry-55.html)）。


## <a id="sec-generated-title-3"></a> <a id="invoke"></a>RunspaceInvoke

.NET 言語から PowerShell スクリプトを実行するには System.Management.Automation.RunspaceInvoke クラスを使います。

<pre class="source" title="RunspaceInvoke" lang="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> invoker = <span class="reserved">new</span> RunspaceInvoke())
{
    <span class="reserved">var</span> results = invoker.Invoke(source, <span class="reserved">new object</span>[] { });

    <span class="reserved">foreach</span> (<span class="reserved">var</span> result <span class="reserved">in</span> results)
    {
        Console.Write(result);
    }
}
</code></pre>


Invoke メソッドの第2引数は、パイプライン入力としてスクリプトに渡されます。
同様に、スクリプト中でパイプラインに出力した結果が results として C# 側に返されます。


### <a id="sec-generated-title-4"></a> <a id="arguments"></a>PowerShell 側での引数の受け取り方

前述のとおり、C# から与えられた入力はパイプライン入力になるので、
PowerShell 側では $input 「[自動変数](../syntax/variable.md#auto_var)」を使って受け取ることができます。

例えば、以下のコードでは、パイプラインで与えられた入力を二乗して出力します。

<pre class="source" title="$input を使って入力を受け取る" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Management.Automation;

<span class="reserved">static void</span> Main()
{
    <span class="reserved">string</span> source = <span class="literal">@"foreach($x in $input) { $x * $x }"</span>;

    <span class="reserved">using</span> (<span class="reserved">var</span> invoker = <span class="reserved">new</span> RunspaceInvoke())
    {
        <span class="reserved">var</span> result = invoker.Invoke(source, <span class="reserved">new</span>[] { 1, 2, 3, 4 });

        <span class="reserved">foreach</span> (<span class="reserved">var</span> r <span class="reserved">in</span> result)
        {
            Console.WriteLine(r);
        }
    }
}
</code></pre>


<pre class="console" title="出力結果">
1
4
9
16
</pre>


要するに、これで以下のような PowerShell コマンドと同じような実行結果になります。

<pre class="console" title="上記と同様のことをする PowerShell コマンド">
<span class="prompt">&gt; </span>$source = { foreach($x in $input) { $x * $x } }
<span class="prompt">&gt; </span>$results = 1, 2, 3, 4 | &amp; $source
<span class="prompt">&gt; </span>$results
1
4
9
16
</pre>


ちなみに、$input は列挙子(IEnumerator) であって、リストや配列ではないので、
以下のような値の受け取り方はできません。

<pre class="source" title="$input " lang="">
<code>$arg1 = $input[0]   # エラー。[] が使えない。
$arg2 = $input[1]   # 同上。
</code></pre>


<pre class="source" title="$input " lang="">
<code>$arg1, $arg2 = $input   # エラー。この構文も、右辺がリストでないと使えない。
</code></pre>


ちょっとうざったいですが、以下のいずれかのような受け取り方をするのがてっとり早いと思います。

<pre class="source" title="$input " lang="">
<code>$count = 0
foreach($x in $input)
{
  switch ($count)
  {
    0 { $arg1 = $x }
    1 { $arg2 = $x }
  }
  $count++
}
</code></pre>

<pre class="source" title="$input " lang="">
<code>if ($input.MoveNext()) { $arg1 = $input.Current }
if ($input.MoveNext()) { $arg2 = $input.Current }
</code></pre>

LINQ の ToList を PowerShell からも使いたい・・・


#### <a id="sec-generated-title-5"></a>サンプル

2つの配列の要素ごとの積を求めます。

<pre class="source" title="要素ごとの積" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Management.Automation;

<span class="reserved">static void</span> Main()
{
    <span class="reserved">string</span> source = <span class="literal">@"
$count = 0
foreach($a in $input)
{
  switch ($count)
  {
    0 { $lhs = $a }
    1 { $rhs = $a }
  }
  $count++
}

$len = [Math]::Min($lhs.Length, $rhs.Length)

for($i = 0; $i -lt $len; $i++)
{
  $lhs[$i] * $rhs[$i]
}
"</span>;

    <span class="reserved">using</span> (<span class="reserved">var</span> invoker = <span class="reserved">new</span> RunspaceInvoke())
    {
        <span class="reserved">var</span> lhs = <span class="reserved">new</span>[] { 1, 2, 3, 4, 5 };
        <span class="reserved">var</span> rhs = <span class="reserved">new</span>[] { 2, 3, 4, 5, 6 };

        <span class="reserved">var</span> result = invoker.Invoke(source, <span class="reserved">new</span>[] { lhs, rhs });

        <span class="reserved">foreach</span> (<span class="reserved">var</span> r <span class="reserved">in</span> result)
        {
            Console.WriteLine(r);
        }
    }
}
</code></pre>


<pre class="console" title="出力結果">
2
6
12
20
30
</pre>
