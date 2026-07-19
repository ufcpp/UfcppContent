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

```csharp
using System;
using System.IO;

class TestIO
{
  static void Main()
  {
    if(!Directory.Exists("test"))
      Directory.CreateDirectory("test");
    
    for(int i=0; i<5; ++i)
    {
      string fileName = string.Format(@"test\{0}.txt", i);
      string contents = string.Format("Test file No. {0}", i);
      File.WriteAllText(fileName, contents);
    }
  }
}
```


プログラムを実行した後のディレクトリ内の様子です。

```console
> ls
ConsoleApplication2.exe  ConsoleApplication2.vshost.exe
ConsoleApplication2.pdb  test
> cd test
> ls
0.txt  1.txt  2.txt  3.txt  4.txt
> cat 1.txt
Test file No. 1
```



## <a id="sec-generated-title-3"></a> <a id="fileinfo"></a>DirectoryInfo, FileInfo

System.IO.DirectoryInfo と System.IO.FileInfo は、
ディレクトリやファイルの情報を取り出すためのクラスです。

```csharp
using System;
using System.IO;

class TestRexex
{
  static void Main()
  {
    DirectoryInfo dir = new DirectoryInfo("test");

    foreach(FileInfo f in dir.GetFiles())
    {
      string name = f.Name;
      string ext  = f.Extension;
      DateTime t = f.CreationTime;
      Console.Write("{0}\next: {1}, time: {2}\n",
        name, ext, t.ToString("hh:mm:ss"));
    }
  }
  }
}
```


```console
0.txt
ext: .txt, time: 02:34:36
1.txt
ext: .txt, time: 02:34:36
2.txt
ext: .txt, time: 02:34:36
3.txt
ext: .txt, time: 02:34:36
4.txt
ext: .txt, time: 02:34:36
```



## <a id="sec-generated-title-4"></a> <a id="stream"></a>StreamReader, StreamWriter

System.IO.StreamReader, System.IO.StreamWriter を用いることで、
テキストファイルの読み書きができます。

```csharp
using System;
using System.IO;

class TestRexex
{
  static void Main()
  {
    // ファイルにテキストを書き出し。
    using(StreamWriter w = new StreamWriter(@"test\test.txt"))
    {
      w.WriteLine("基本的に、Console クラスの文字列出力メソッドと同じ。");
      w.WriteLine("WriteLine では末尾に改行文字が加えられます。");
      int n = 5;
      double x = 3.14;
      w.Write("書式指定出力もできます → n = {0}, x = {1}", n, x);
    }

    // ファイルからテキストを読み出し。
    using(StreamReader r = new StreamReader(@"test\test.txt"))
    {
      string line;
      while( (line = r.ReadLine()) != null) // 1行ずつ読み出し。
      {
        Console.WriteLine(line);
      }
    }
  }
}
```


```console
基本的に、Console クラスの文字列出力メソッドと同じ。
WriteLine では末尾に改行文字が加えられます。
書式指定出力もできます → n = 5, x = 3.14
```



## <a id="sec-generated-title-5"></a> <a id="binary"></a>BinaryReader, BinaryWriter

バイナリ形式での読み書きには、
System.IO.BinaryReader, System.IO.BinaryWriter クラスを用います。

```csharp
using System;
using System.IO;

class TestRexex
{
  static void Main()
  {
    // バイナリ形式でファイルに書き出し。
    using(BinaryWriter w = new BinaryWriter(File.OpenWrite(@"test\binary")))
    {
      w.Write(new byte[]{(byte)0x01, (byte)0x23, (byte)0x45, (byte)0x67, });
      w.Write((int)123456789);
      w.Write((float)3.14159);
    }

    // 1バイトずつ読み出し。
    using(BinaryReader w = new BinaryReader(File.OpenRead(@"test\binary")))
    {
      try
      {
        for(;;)
          Console.Write("{0:x2}", w.ReadByte());
      }
      catch(EndOfStreamException)
      {
        Console.Write("\n");
      }
    }

    // 書き出したときと同じ手順で読み出し。
    using(BinaryReader w = new BinaryReader(File.OpenRead(@"test\binary")))
    {
      Console.Write("{0:x2}, ", w.ReadByte());
      Console.Write("{0:x2}, ", w.ReadByte());
      Console.Write("{0:x2}, ", w.ReadByte());
      Console.Write("{0:x2}\n", w.ReadByte());
      Console.Write("{0:d}\n", w.ReadInt32());
      Console.Write("{0:g}\n", w.ReadSingle());
    }
  }
}
```


```console
0123456715cd5b07d00f4940
01, 23, 45, 67
123456789
3.14159
```
