---
title: "文字列関係"
source_url: "https://ufcpp.net/study/csharp/lib/lib_string/"
content_type: "Article"
published_at: "2006-07-13T00:00:00"
updated_at: "2006-11-15T00:00:00"
tags: []
umbraco_id: 1351
parent_id: 1350
sort_order: 0
aliases:
  - "/csharp/lib/lib_string/"
  - "/csharp/lib_string"
  - "/csharp/lib_string.html"
  - "/study/csharp/lib_string"
  - "/study/csharp/lib_string.html"
---

# 文字列関係

## <a id="sec-generated-title-1"></a> <a id="string"></a>string

C# の組込み型 string の実体は System.String クラスです。
System.String クラスには以下のようなメソッドが標準で用意されています。
（以下の例に挙げるもの以外にも、いくつかのメソッドがあります。）

<pre class="source" title="System.String のメンバー" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> TestString
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">string</span> s = <span class="literal">"This Is a Program Which Allows You to Play It"</span>;

    <span class="comment">// ToUpper … アルファベットを大文字に変換</span>
    Console.Write(s.ToUpper() + <span class="literal">"\n"</span>);

    <span class="comment">// ToLower … アルファベットを小文字に変換</span>
    Console.Write(s.ToLower() + <span class="literal">"\n"</span>);

    <span class="comment">// Replace … 文字列の置換</span>
    Console.Write(s.Replace(<span class="literal">"Play"</span>, <span class="literal">"View"</span>) + <span class="literal">"\n"</span>);

    <span class="comment">// Split … 文字列を分割</span>
    <span class="reserved">int</span> i = 0;
    <span class="reserved">foreach</span>(<span class="reserved">string</span> word <span class="reserved">in</span> s.Split(' '))
    {
      <span class="reserved">if</span>((i % 2) == 0)
      {
        <span class="comment">// PadLeft … 左寄せで10文字分の幅にする。</span>
        Console.Write(<span class="literal">"/{0}/\n"</span>, word.PadLeft(10));
      }
      <span class="reserved">else</span>
      {
        <span class="comment">// PadRight … 右寄せで10文字分の幅にする。</span>
        Console.Write(<span class="literal">"/{0}/\n"</span>, word.PadRight(10));
      }
      ++i;
    }

    <span class="comment">// IndexOf … 文字列の検索</span>
    Console.Write(<span class="literal">"\"Program\" is found at {0}\n"</span>, s.IndexOf(<span class="literal">"Program"</span>));
  }
}
</code></pre>


<pre class="console" title="System.String のメンバー">
THIS IS A PROGRAM WHICH ALLOWS YOU TO PLAY IT
this is a program which allows you to play it
This Is a Program Which Allows You to View It
/      This/
/Is        /
/         a/
/Program   /
/     Which/
/Allows    /
/       You/
/to        /
/      Play/
/It        /
"Program" is found at 10
</pre>



## <a id="sec-generated-title-2"></a> <a id="format"></a>書式指定出力

String.Fomat メソッドや、Console.Write メソッドは、
数値などの体裁を整える書式指定機能を持っています。

詳細説明に別ページを儲けました: 「[文字列の書式設定](../../dotnet/bcl/bcl_format.md)」

<pre class="source" title="書式指定出力" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> TestConsoleWrite
{
  <span class="reserved">static void</span> Main()
  {
  Console.Write(
<span class="literal">@"
通常     {0:d}
通貨     {0:c}
, 区切り {0:n}
16進数   {0:x}
"</span>, 19980);
  Console.Write(
<span class="literal">@"
通常 {0:d}
4桁  {0:d4}
8桁  {0:d8}
"</span>, 196);
  Console.Write(
<span class="literal">@"
通常       {0:g}
固定桁     {0:f}
指数表記   {0:e}
パーセント {0:p}
"</span>, 0.012345678);
  Console.Write(
<span class="literal">@"
標準 {0:f}
4桁  {0:f4}
6桁  {0:f6}
"</span>, 365.242199);
  Console.Write(
<span class="literal">@"
桁数明示 {0:000.000}
"</span>, 3.14);
  Console.Write(
<span class="literal">@"
通常     {0}
千単位   {0:#,} 千
百万単位 {0:#,,}    百万
電話番号 {0:(000) 000-0000}
"</span>, 123456789);
  }
}
</code></pre>


<pre class="console" title="書式指定出力">
通常     19980
通貨     \19,980
, 区切り 19,980.00
16進数   4e0c

通常 196
4桁  0196
8桁  00000196

通常       0.012345678
固定桁     0.01
指数表記   1.234568e-002
パーセント 1.23%

標準 365.24
4桁  365.2422
6桁  365.242199

桁数明示 003.140

通常     121378376
千単位   121378 千
百万単位 121    百万
電話番号 (012) 137-8376
</pre>



## <a id="sec-generated-title-3"></a> <a id="regex"></a>正規表現

System.Text.RegularExpressions.Regex クラスにより、
正規表現による文字列探索を利用できます。

詳細説明に別ページを儲けました: 「[正規表現（文字列パターン マッチング）](../../dotnet/bcl/bcl_regex.md)」

Regex クラスの正規表現は、Perl や Awk などの有名な実装との互換性を意識して設計されています。

<pre class="source" title="Regex クラス" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">class</span> TestRexex
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">string</span> s =
<span class="literal">@"
This is a test program.
you can download it from the following page.
http://www.xxx.yyy/bin/test.exe
If you have any questions about this,
please contact us by sending e-mail to the following address.
support@xxx.yyy
"</span>;

    <span class="comment">// メールアドレス抽出</span>
    Regex email = <span class="reserved">new</span> Regex(<span class="literal">@"\w*@[\w\.]*"</span>);
    Console.Write(<span class="literal">"{0}\n"</span>, email.Match(s).Value);

    <span class="comment">// URL 抽出</span>
    Regex http = <span class="reserved">new</span> Regex(<span class="literal">@"http://(?&lt;domain&gt;[\w\.]*)/(?&lt;path&gt;[\w\./]*)"</span>);
    Match m = http.Match(s);
    Console.Write(<span class="literal">"{0}\n"</span>, m.Value);
    Console.Write(<span class="literal">"domain: {0}\npath  : {1}\n"</span>,
      m.Groups[<span class="literal">"domain"</span>].Value,
      m.Groups[<span class="literal">"path"</span>].Value);

    <span class="comment">// t の付く単語全部抜き出し</span>
    Regex wordIncludingT = <span class="reserved">new</span> Regex(<span class="literal">@"\w*[tT]\w*"</span>);
    <span class="reserved">for</span>(m = wordIncludingT.Match(s); m.Success; m = m.NextMatch())
      Console.Write(<span class="literal">"{0}\t"</span>, m.Value);
    Console.Write(<span class="literal">"\n"</span>);
  }
}
</code></pre>


<pre class="console" title="Regex クラス">
support@xxx.yyy
http://www.xxx.yyy/bin/test.exe
domain: www.xxx.yyy
path  : bin/test.exe
This    test    it      the     http    test    questions       about   this
contact to      the     support
</pre>



##### <a id="sec-generated-title-4"></a>サンプル

正規表現クラス Regex を使って、
XML の中身を HTML 中に貼り付けれる形に変換します。

<pre class="source" title="XmlToText" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Generic;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Text;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">namespace</span> XmlToText
{
  <span class="reserved">class</span> Program
  {
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
      Regex regComment = <span class="reserved">new</span> Regex(
        <span class="literal">"\\&lt;!--(?&lt;inner&gt;.*?)--\\&gt;"</span>,
        RegexOptions.Compiled
        );
      Regex regElem = <span class="reserved">new</span> Regex(
        <span class="literal">"\\&lt;(?&lt;inner&gt;/{0,1}[\\w:\\.]+\\s*"</span> +
        <span class="literal">"([\\w:\\.]+\\s*=\\s*\\\"[^\\\"]+\\\"\\s*)*/{0,1})\\&gt;"</span>,
        RegexOptions.Compiled
        );
      Regex regName = <span class="reserved">new</span> Regex(
        <span class="literal">"^/{0,1}(?&lt;name&gt;[\\w:\\.]+)"</span>,
        RegexOptions.Compiled
        );
      Regex regAttrName = <span class="reserved">new</span> Regex(
        <span class="literal">"(?&lt;attname&gt;[\\w:\\.]+)\\s*="</span>,
        RegexOptions.Compiled
        );
      Regex regAttrValue = <span class="reserved">new</span> Regex(
        <span class="literal">"(?&lt;attvalue&gt;=\\s*\\\"[^\\\"]+\\\")"</span>,
        RegexOptions.Compiled
        );
      
      <span class="reserved">string</span> target;

      <span class="reserved">using</span> (
        StreamReader reader = <span class="reserved">new</span>
          StreamReader(Console.OpenStandardInput(), Encoding.UTF8))
      {
        target = reader.ReadToEnd();
      }

      <span class="reserved">string</span> result = target;

      result = result.Replace(<span class="literal">"&amp;"</span>, <span class="literal">"&amp;amp;"</span>);
      result = result.Replace(<span class="literal">"\t"</span>, <span class="literal">"  "</span>);

      result = regComment.Replace(result,
        <span class="reserved">delegate</span>(Match mc)
        {
          <span class="reserved">return</span>
            <span class="literal">"$$$comment$$$&amp;lt;!--"</span> +
            mc.Groups[<span class="literal">"inner"</span>].Value.Replace(<span class="literal">"&lt;"</span>, <span class="literal">"&amp;lt;"</span>).Replace(<span class="literal">"&gt;"</span>, <span class="literal">"&amp;gt;"</span>) +
            <span class="literal">"--&amp;gt;$$$/comment$$$"</span>;
        });

      result = regElem.Replace(result,
        <span class="reserved">delegate</span>(Match m)
        {
          <span class="reserved">string</span> q = m.Groups[<span class="literal">"inner"</span>].Value;

          q = regName.Replace(q,
            <span class="reserved">delegate</span>(Match m0)
            {
              <span class="reserved">string</span> r = m0.Value;
              <span class="reserved">string</span> name = m0.Groups[<span class="literal">"name"</span>].Value;
              <span class="reserved">return</span> r.Replace(name, <span class="literal">"&lt;element&gt;"</span> + name + <span class="literal">"&lt;/element&gt;"</span>);
            });

          q = regAttrName.Replace(q,
            <span class="reserved">delegate</span>(Match m1)
            {
              <span class="reserved">string</span> s = m1.Value;
              <span class="reserved">string</span> attname = m1.Groups[<span class="literal">"attname"</span>].Value;
              <span class="reserved">return</span> s.Replace(attname, <span class="literal">"&lt;attribute&gt;"</span> + attname + <span class="literal">"&lt;/attribute&gt;"</span>);
            });

          q = regAttrValue.Replace(q,
            <span class="reserved">delegate</span>(Match m2)
            {
              <span class="reserved">string</span> t = m2.Value;
              <span class="reserved">string</span> attvalue = m2.Groups[<span class="literal">"attvalue"</span>].Value;
              <span class="reserved">return</span> t.Replace(attvalue, <span class="literal">"&lt;attvalue&gt;"</span> + attvalue + <span class="literal">"&lt;/attvalue&gt;"</span>);
            });

          <span class="reserved">return</span> <span class="literal">"&lt;lt/&gt;"</span> + q + <span class="literal">"&lt;gt/&gt;"</span>;
        });

      result = result.Replace(<span class="literal">"$$$comment$$$"</span>, <span class="literal">"&lt;comment&gt;"</span>);
      result = result.Replace(<span class="literal">"$$$/comment$$$"</span>, <span class="literal">"&lt;/comment&gt;"</span>);

      Console.Write(result);
    }
  }
}
</code></pre>


入力 XML ファイル。


<pre class="xsource" title="入力 XML ファイル">
<code><span class="bracket">&lt;</span><span class="element">Page</span>
  <span class="attribute">xmlns</span><span class="attvalue">=
    "http://schemas.microsoft.com/winfx/2006/xaml/presentation"</span>
  <span class="attribute">xmlns:x</span><span class="attvalue">=
    "http://schemas.microsoft.com/winfx/2006/xaml"</span>
  <span class="attribute">Background</span><span class="attvalue">="White"</span>
  <span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">FlowDocument</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Paragraph</span>
    <span class="attribute">FontSize</span><span class="attvalue">="32"</span>
    <span class="attribute">Foreground</span><span class="attvalue">="Blue"</span><span class="bracket">&gt;</span>
  Example
  <span class="bracket">&lt;</span>/<span class="element">Paragraph</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">Paragraph</span>
    <span class="attribute">FontSize</span><span class="attvalue">="24"</span><span class="bracket">&gt;</span>
  This is an example of a 
  <span class="bracket">&lt;</span><span class="element">Span</span>
    <span class="attribute">FontStyle</span><span class="attvalue">="Italic"</span>
    <span class="attribute">Foreground</span><span class="attvalue">="Red"</span><span class="bracket">&gt;</span>
    xaml
    <span class="bracket">&lt;</span>/<span class="element">Span</span><span class="bracket">&gt;</span>
    application.
  <span class="bracket">&lt;</span>/<span class="element">Paragraph</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span>/<span class="element">FlowDocument</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">Page</span><span class="bracket">&gt;</span>
</code></pre>
出力。
この例では、
&lt; 等を、
&lt;lt/&gt; という形に変換して、
これをさらに XSLT して利用することを想定しています。
（このサイトは、XML でドキュメントを記述して、XSLT で HTML 化しています。）


<pre class="xsource" title="出力。">
<code><span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Page<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>xmlns<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>=
    "http://schemas.microsoft.com/winfx/2006/xaml/presentation"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>xmlns:x<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>=
    "http://schemas.microsoft.com/winfx/2006/xaml"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>Background<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="White"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>FlowDocument<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Paragraph<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>FontSize<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="32"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>Foreground<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="Blue"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
  Example
  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span>/<span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Paragraph<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Paragraph<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>FontSize<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="24"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
  This is an example of a 
  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Span<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>FontStyle<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="Italic"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">attribute</span><span class="bracket">&gt;</span>Foreground<span class="bracket">&lt;</span>/<span class="element">attribute</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">attvalue</span><span class="bracket">&gt;</span>="Red"<span class="bracket">&lt;</span>/<span class="element">attvalue</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
  xaml
  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span>/<span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Span<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
  application.
  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span>/<span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Paragraph<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span>/<span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>FlowDocument<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">lt</span>/<span class="bracket">&gt;</span>/<span class="bracket">&lt;</span><span class="element">element</span><span class="bracket">&gt;</span>Page<span class="bracket">&lt;</span>/<span class="element">element</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">gt</span>/<span class="bracket">&gt;</span>
</code></pre>
