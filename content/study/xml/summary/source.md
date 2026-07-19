---
title: "ソースファイル"
source_url: "https://ufcpp.net/study/xml/summary/source/"
content_type: "Article"
published_at: "2015-05-06T14:24:24"
updated_at: "2015-07-07T18:34:23"
tags: []
umbraco_id: 1658
parent_id: 1650
sort_order: 7
aliases:
  - "/study/testxsl/source"
  - "/study/testxsl/source.html"
  - "/testxsl/source"
  - "/testxsl/source.html"
  - "/xml/summary/source/"
---

# ソースファイル

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

[<code>source.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/source.xsl) には、コンソール画面風の表示や、プログラムソース・XML ファイル表示のための template が記述されています。


## <a id="sec-generated-title-2"></a> <a id="source"></a>ソース

<pre class="xsource" title="">
<code><span class="bracket">&lt;</span><span class="element">source</span> <span class="attribute">xml:space</span><span class="attvalue">="preserve"</span> <span class="attribute">title</span><span class="attvalue">="C# ソースファイル"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">reserved</span><span class="bracket">&gt;</span>namespace<span class="bracket">&lt;/</span><span class="element">reserved</span><span class="bracket">&gt;</span> Test
{
  <span class="bracket">&lt;</span><span class="element">reserved</span><span class="bracket">&gt;</span>class<span class="bracket">&lt;/</span><span class="element">reserved</span><span class="bracket">&gt;</span> ConsoleApp1
  {
    <span class="bracket">&lt;</span><span class="element">reserved</span><span class="bracket">&gt;</span>public static void<span class="bracket">&lt;/</span><span class="element">reserved</span><span class="bracket">&gt;</span> Main(<span class="bracket">&lt;</span><span class="element">reserved</span><span class="bracket">&gt;</span>string<span class="bracket">&lt;/</span><span class="element">reserved</span><span class="bracket">&gt;</span>[] args)
    {
      <span class="bracket">&lt;</span><span class="element">comment</span><span class="bracket">&gt;</span>// お約束のあの文句を画面に表示。<span class="bracket">&lt;/</span><span class="element">comment</span><span class="bracket">&gt;</span>
      Console.Write(<span class="bracket">&lt;</span><span class="element">string</span><span class="bracket">&gt;</span>"Hello World!\n"<span class="bracket">&lt;/</span><span class="element">string</span><span class="bracket">&gt;</span>);
    }
  }
}
<span class="bracket">&lt;/</span><span class="element">source</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">xsource</span> <span class="attribute">xml:space</span><span class="attvalue">="preserve"</span> <span class="attribute">title</span><span class="attvalue">="XML"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;?<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>xml<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>version<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="1.0"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>encoding<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="utf-8"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>?&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>document<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>title<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="ソースファイル"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>xmlns<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="http://ufcpp.net/study/document"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>section<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>title<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="概要"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span> <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>id<span class="bracket">&lt;/</span><span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="abst"<span class="bracket">&lt;/</span><span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>p<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
      XML 用
    <span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;/<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>p<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;/<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>section<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;lt;/<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>document<span class="bracket">&lt;/</span><span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">symbol</span><span class="bracket">&gt;</span>&amp;gt;<span class="bracket">&lt;/</span><span class="element">symbol</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">xsource</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">console</span> <span class="attribute">xml:space</span><span class="attvalue">="preserve"</span> <span class="attribute">title</span><span class="attvalue">="コンソール画面"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">prompt</span><span class="bracket">/&gt;</span><span class="bracket">&lt;</span><span class="element">input</span><span class="bracket">&gt;</span>csc Test.cs<span class="bracket">&lt;/</span><span class="element">input</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">prompt</span><span class="bracket">/&gt;</span><span class="bracket">&lt;</span><span class="element">input</span><span class="bracket">&gt;</span>Test.exe<span class="bracket">&lt;/</span><span class="element">input</span><span class="bracket">&gt;</span>
Hello World!<span class="bracket">&lt;</span><span class="element">comment</span><span class="bracket">&gt;</span>お決まりのあれが表示される<span class="bracket">&lt;/</span><span class="element">comment</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">console</span><span class="bracket">&gt;</span>
</code></pre>
ちなみに、さすがに reserved とか commenet とかのタグは、
ソースファイルから自動生成するためのプログラムを作って使っています。


## <a id="sec-generated-title-3"></a> <a id="result"></a>結果

<pre class="source" title="C# ソースファイル" lang="">
<code><span class="reserved">namespace</span> Test
{
  <span class="reserved">class</span> ConsoleApp1
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      <span class="comment">// お約束のあの文句を画面に表示。</span>
      Console.Write(<span class="literal">"Hello World!\n"</span>);
    }
  }
}
</code></pre>



<pre class="xsource" title="XML">
<code><span class="bracket">&lt;?</span><span class="element">xml</span> <span class="attribute">version</span><span class="attvalue">="1.0"</span> <span class="attribute">encoding</span><span class="attvalue">="utf-8"</span><span class="bracket">?&gt;</span>
<span class="bracket">&lt;</span><span class="element">document</span> <span class="attribute">title</span><span class="attvalue">="ソースファイル"</span> <span class="attribute">xmlns</span><span class="attvalue">="http://ufcpp.net/study/document"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">section</span> <span class="attribute">title</span><span class="attvalue">="概要"</span> <span class="attribute">id</span><span class="attvalue">="abst"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
      XML 用
    <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">section</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">document</span><span class="bracket">&gt;</span>
</code></pre>
<pre class="console" title="コンソール画面">
<span class="prompt">&gt; </span><span class="input">csc Test.cs</span>
<span class="prompt">&gt; </span><span class="input">Test.exe</span>
Hello World!
<span class="comment"># ↓ お決まりのあれが表示される</span>
</pre>
