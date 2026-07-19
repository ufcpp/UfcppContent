---
title: "COM 相互運用時の特別処理"
source_url: "https://ufcpp.net/study/csharp/interop/sp4_cominterop/"
content_type: "Article"
published_at: "2009-11-19T00:00:00"
updated_at: "2019-08-03T19:26:47"
tags:
  - "Ver. 4.0"
umbraco_id: 1325
parent_id: 1321
sort_order: 3
aliases:
  - "/csharp/interop/sp4_cominterop/"
  - "/csharp/sp4_cominterop"
  - "/csharp/sp4_cominterop.html"
  - "/study/csharp/sp4_cominterop"
  - "/study/csharp/sp4_cominterop.html"
---

# COM 相互運用時の特別処理

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>

.NET Framework には COM 相互運用機能があって、COM のクラスをあたかも .NET のクラスであるかのように扱うことができます。
ただ、COM が主流だった時代と今とでは大分設計思想に差があって、
.NET 的には不要だけども、COM 相互運用をする上では欲しい機能というのがいくつかあります。

そこで、C# 4.0 では、COM 相互運用用のクラス
（RCW（Runtime Callable Wrapper）といいます。.NET ランタイムから COM を呼び出せるようにしたラッパークラス）に対してだけ特別な処理をするようになりました。
COM への特別処理は以下の2点。

* ref 引数（「[引数の参照渡し](../resource/sp_ref.md)」参照）に対して、ref キーワードを付けなくても呼び出せるようになった。

* <code>get_X(index)</code>、<code>set_X(index, value)</code>というメソッドに対して、 インデックス付きプロパティ構文（<code>X[index]</code>という書き方）が使えるようになった。



## <a id="sec-generated-title-2"></a> <a id="refomit"></a>ref 省略

本来、「引数の参照渡しでは、呼び出し側からも参照渡しであることが一目でわかるべき」
というのが C# の流儀なので、ref キーワードの省略はあまりいい構文ではありません。
（なので、通常は「[参照渡し](../resource/sp_ref.md#byref)」では ref を省略できない。）

ですが、COM の場合、参照渡しにする必要のないようなものにまでやたらと ref が付きまくるので、
やむなく ref キーワードの省略を認めるようです。
（あくまで RCW （COM 相互運用クラス）に対してだけこの機構が働く。）

例えば、悪名高い Word の Document.SaveAs メソッドを見てみましょう。
C# 3.0 までは以下のような書き方をする必要がありました。

<pre class="source" title="悪名高い ref 地獄" lang="">
<code><span class="reserved">var</span> word = <span class="reserved">new</span> Microsoft.Office.Interop.Word.<span class="type">Application</span>();

<span class="reserved">object</span> missing = <span class="type">Type</span>.Missing;
<span class="reserved">object</span> filename = <span class="literal">"sample.docx"</span>;
word.ActiveDocument.SaveAs2(<span class="reserved">ref</span> filename,
    <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing,
    <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing,
    <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing,
    <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing, <span class="reserved">ref</span> missing);
</code></pre>


これが、C# 4.0 なら以下のように書けたりもします。

<pre class="source" title="ref 省略" lang="">
<code><span class="reserved">object</span> missing = <span class="type">Type</span>.Missing;
<span class="reserved">string</span> filename = <span class="literal">"sample.docx"</span>;
word.ActiveDocument.SaveAs2(filename,
    missing, missing, missing, missing,
    missing, missing, missing, missing,
    missing, missing, missing, missing,
    missing, missing, missing, missing);
</code></pre>


まあ、この場合、ref 省略よりも、オプション引数（「[オプション引数・名前付き引数](../structured/sp4_optional.md)」参照）が追加されたことの方がインパクトが大きいですが↓。

<pre class="source" title="オプション引数が入ったおかげで" lang="">
<code><span class="reserved">string</span> filename = <span class="literal">"sample.docx"</span>;
word.ActiveDocument.SaveAs2(filename);
</code></pre>


ただし、これも COM 特別処理のおかげです。
C# 的には本来、ref 引数に対して規定値は定義できないんですが、
RCW の場合には ref がついてても規定値が設定されるようになっています。


## <a id="sec-generated-title-3"></a> <a id="indexed"></a>インデックス付きプロパティ

C# は「インデックス付きプロパティじゃなくて、インデクサー持ちの型のプロパティを作れ」という設計思想です。
（あるいは、「[イテレーター](../data/sp2_iterator.md#iterator)」を使って IEnumerable を返すか。）
でも、COM の時代にはそういう思想がなくて、インデックス付きプロパティだらけなので、これもやむなく認めるようになりました。

例えば、Excel の Application.Range がインデックス付きプロパティになっています。
C# 3.0 までは、以下のようにアクセスする必要がありました。

<pre class="source" title="get_Range" lang="">
<code><span class="reserved">var</span> excel = <span class="reserved">new</span> Microsoft.Office.Interop.Excel.<span class="type">Application</span>();
<span class="reserved">var</span> range = excel.get_Range(<span class="literal">"A1"</span>, <span class="literal">"A2"</span>);
</code></pre>


これが、C# 4.0 では以下のように書けます。

<pre class="source" title="インデックス付きプロパティ" lang="">
<code><span class="reserved">var</span> excel = <span class="reserved">new</span> Microsoft.Office.Interop.Excel.<span class="type">Application</span>();
<span class="reserved">var</span> range = excel.Range[<span class="literal">"A1"</span>, <span class="literal">"A2"</span>];
</code></pre>


また、対 COM 限定で、インデクサーやインデックス付きプロパティに対する引数の省略（「[オプション引数](../structured/sp4_optional.md#optional)」参照）が可能です。
すなわち、以下のような記述が許されます。

<pre class="source" title="インデクサーに対する引数の省略" lang="">
<code>obj[];
</code></pre>


これらの処理は本当に対 RCW （COM 相互運用）専用です。
C# でインデックス付きプロパティが定義できるようになるわけではないです。

それどころか、VB.NET で作ったインデックス付きプロパティにすら、
C# からはインデックス付きプロパティ構文でアクセスできません。
（今まで通り get_X という書き方をする必要があります。）
例えば、VB で以下のように書いたとしても、

<pre class="source" title="VB のインデックス付きプロパティ" lang="">
<code><span class="reserved">Public Class</span> <span class="type">Class1</span>

    <span class="reserved">Dim</span> x_ <span class="reserved">As</span> <span class="type">Dictionary</span>(<span class="reserved">Of String</span>, <span class="reserved">Integer</span>)

    <span class="reserved">Public Property</span> X(<span class="reserved">ByVal</span> i <span class="reserved">As String</span>) <span class="reserved">As Integer
        Get
            Return</span> x_(i)
        <span class="reserved">End Get
        Set</span>(<span class="reserved">ByVal</span> value <span class="reserved">As Integer</span>)
            x_(i) = value
        <span class="reserved">End Set
    End Property

End Class</span>
</code></pre>


C# 側からは get_X でしか参照できません。（下図参照。）

<figure>
	[![C# から見たインデックス付きプロパティ](../../../../assets/media/ufcpp2000/csharp/fig/get_x.png)](../../../../assets/media/ufcpp2000/csharp/fig/get_x.png)
	<figcaption>C# から見たインデックス付きプロパティ</figcaption>
</figure>

## <a id="sec-generated-title-4"></a> <a id="no-pia"></a>No PIA

C# の機能ではなく、.NET Framework 4 の新機能ですが、No PIA と呼ばれる機能も追加されました。

詳しくは「[プラットフォーム呼び出し](sp_pinvoke.md#no-pia)」で説明します。
