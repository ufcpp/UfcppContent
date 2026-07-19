---
title: "ファイル操作"
source_url: "https://ufcpp.net/study/csharp/lib/lib_file/"
content_type: "Article"
published_at: "2001-12-31T00:00:00"
updated_at: "2015-05-06T14:12:55"
tags: []
umbraco_id: 1352
parent_id: 1350
sort_order: 1
aliases:
  - "/csharp/lib/lib_file/"
  - "/csharp/lib_file"
  - "/csharp/lib_file.html"
  - "/study/csharp/lib_file"
  - "/study/csharp/lib_file.html"
---

# ファイル操作

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

System.IO 名前空間以下に、ディレクトリ（フォルダ）・ファイルの作成・読み書き等を行うためのクラスが用意されています。

ファイル操作などの処理は、
オブジェクト指向言語との相性もいいですし、
C# の 「[foreach 文](../data/sp_foreach.md#foreach)」 や 「[LINQ](../data/sp3_linq.md#linq)」 との親和性も高いので、
C# でのファイル操作の楽さには目を見張るものがあります。


## <a id="sec-generated-title-2"></a> <a id="file"></a>Directory, File

System.IO.Directory と System.IO.File には、
ディレクトリやファイルを読み書きするための static メソッドがあります。

<pre class="source" title="Directory, File" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> TestIO
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">if</span>(!Directory.Exists(<span class="literal">"test"</span>))
      Directory.CreateDirectory(<span class="literal">"test"</span>);
    
    <span class="reserved">for</span>(<span class="reserved">int</span> i=0; i&lt;5; ++i)
    {
      <span class="reserved">string</span> fileName = <span class="reserved">string</span>.Format(<span class="literal">@"test\{0}.txt"</span>, i);
      <span class="reserved">string</span> contents = <span class="reserved">string</span>.Format(<span class="literal">"Test file No. {0}"</span>, i);
      File.WriteAllText(fileName, contents);
    }
  }
}
</code></pre>


プログラムを実行した後のディレクトリ内の様子です。

<pre class="console" title="Directory, File">
<span class="prompt">&gt; </span><span class="input">ls</span>
ConsoleApplication2.exe  ConsoleApplication2.vshost.exe
ConsoleApplication2.pdb  test
<span class="prompt">&gt; </span><span class="input">cd test</span>
<span class="prompt">&gt; </span><span class="input">ls</span>
0.txt  1.txt  2.txt  3.txt  4.txt
<span class="prompt">&gt; </span><span class="input">cat 1.txt</span>
Test file No. 1
</pre>



## <a id="sec-generated-title-3"></a> <a id="fileinfo"></a>DirectoryInfo, FileInfo

System.IO.DirectoryInfo と System.IO.FileInfo は、
ディレクトリやファイルの情報を取り出すためのクラスです。

<pre class="source" title="DirectoryInfo, FileInfo" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> TestRexex
{
  <span class="reserved">static void</span> Main()
  {
    DirectoryInfo dir = <span class="reserved">new</span> DirectoryInfo(<span class="literal">"test"</span>);

    <span class="reserved">foreach</span>(FileInfo f <span class="reserved">in</span> dir.GetFiles())
    {
      <span class="reserved">string</span> name = f.Name;
      <span class="reserved">string</span> ext  = f.Extension;
      DateTime t = f.CreationTime;
      Console.Write(<span class="literal">"{0}\next: {1}, time: {2}\n"</span>,
        name, ext, t.ToString(<span class="literal">"hh:mm:ss"</span>));
    }
  }
  }
}
</code></pre>


<pre class="console" title="DirectoryInfo, FileInfo">
0.txt
ext: .txt, time: 02:34:36
1.txt
ext: .txt, time: 02:34:36
2.txt
ext: .txt, time: 02:34:36
3.txt
ext: .txt, time: 02:34:36
4.txt
ext: .txt, time: 02:34:36</pre>



## <a id="sec-generated-title-4"></a> <a id="stream"></a>StreamReader, StreamWriter

System.IO.StreamReader, System.IO.StreamWriter を用いることで、
テキストファイルの読み書きができます。

<pre class="source" title="StreamReader, StreamWriter" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> TestRexex
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// ファイルにテキストを書き出し。</span>
    <span class="reserved">using</span>(StreamWriter w = <span class="reserved">new</span> StreamWriter(<span class="literal">@"test\test.txt"</span>))
    {
      w.WriteLine(<span class="literal">"基本的に、Console クラスの文字列出力メソッドと同じ。"</span>);
      w.WriteLine(<span class="literal">"WriteLine では末尾に改行文字が加えられます。"</span>);
      <span class="reserved">int</span> n = 5;
      <span class="reserved">double</span> x = 3.14;
      w.Write(<span class="literal">"書式指定出力もできます → n = {0}, x = {1}"</span>, n, x);
    }

    <span class="comment">// ファイルからテキストを読み出し。</span>
    <span class="reserved">using</span>(StreamReader r = <span class="reserved">new</span> StreamReader(<span class="literal">@"test\test.txt"</span>))
    {
      <span class="reserved">string</span> line;
      <span class="reserved">while</span>( (line = r.ReadLine()) != <span class="reserved">null</span>) <span class="comment">// 1行ずつ読み出し。</span>
      {
        Console.WriteLine(line);
      }
    }
  }
}
</code></pre>


<pre class="console" title="StreamReader, StreamWriter">
基本的に、Console クラスの文字列出力メソッドと同じ。
WriteLine では末尾に改行文字が加えられます。
書式指定出力もできます → n = 5, x = 3.14
</pre>



## <a id="sec-generated-title-5"></a> <a id="binary"></a>BinaryReader, BinaryWriter

バイナリ形式での読み書きには、
System.IO.BinaryReader, System.IO.BinaryWriter クラスを用います。

<pre class="source" title="BinaryReader, BinaryWriter" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.IO;

<span class="reserved">class</span> TestRexex
{
  <span class="reserved">static void</span> Main()
  {
    <span class="comment">// バイナリ形式でファイルに書き出し。</span>
    <span class="reserved">using</span>(BinaryWriter w = <span class="reserved">new</span> BinaryWriter(File.OpenWrite(<span class="literal">@"test\binary"</span>)))
    {
      w.Write(<span class="reserved">new byte</span>[]{(<span class="reserved">byte</span>)0x01, (<span class="reserved">byte</span>)0x23, (<span class="reserved">byte</span>)0x45, (<span class="reserved">byte</span>)0x67, });
      w.Write((<span class="reserved">int</span>)123456789);
      w.Write((<span class="reserved">float</span>)3.14159);
    }

    <span class="comment">// 1バイトずつ読み出し。</span>
    <span class="reserved">using</span>(BinaryReader w = <span class="reserved">new</span> BinaryReader(File.OpenRead(<span class="literal">@"test\binary"</span>)))
    {
      <span class="reserved">try</span>
      {
        <span class="reserved">for</span>(;;)
          Console.Write(<span class="literal">"{0:x2}"</span>, w.ReadByte());
      }
      <span class="reserved">catch</span>(EndOfStreamException)
      {
        Console.Write(<span class="literal">"\n"</span>);
      }
    }

    <span class="comment">// 書き出したときと同じ手順で読み出し。</span>
    <span class="reserved">using</span>(BinaryReader w = <span class="reserved">new</span> BinaryReader(File.OpenRead(<span class="literal">@"test\binary"</span>)))
    {
      Console.Write(<span class="literal">"{0:x2}, "</span>, w.ReadByte());
      Console.Write(<span class="literal">"{0:x2}, "</span>, w.ReadByte());
      Console.Write(<span class="literal">"{0:x2}, "</span>, w.ReadByte());
      Console.Write(<span class="literal">"{0:x2}\n"</span>, w.ReadByte());
      Console.Write(<span class="literal">"{0:d}\n"</span>, w.ReadInt32());
      Console.Write(<span class="literal">"{0:g}\n"</span>, w.ReadSingle());
    }
  }
}
</code></pre>


<pre class="console" title="BinaryReader, BinaryWriter">
0123456715cd5b07d00f4940
01, 23, 45, 67
123456789
3.14159
</pre>
