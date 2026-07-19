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

```xml
<source xml:space="preserve" title="C# ソースファイル">
<reserved>namespace</reserved> Test
{
  <reserved>class</reserved> ConsoleApp1
  {
    <reserved>public static void</reserved> Main(<reserved>string</reserved>[] args)
    {
      <comment>// お約束のあの文句を画面に表示。</comment>
      Console.Write(<string>"Hello World!\n"</string>);
    }
  }
}
</source>

<xsource xml:space="preserve" title="XML">
<symbol>&lt;?</symbol><element>xml</element> <attribute>version</attribute><attvalue>="1.0"</attvalue> <attribute>encoding</attribute><attvalue>="utf-8"</attvalue><symbol>?&gt;</symbol>
<symbol>&lt;</symbol><element>document</element> <attribute>title</attribute><attvalue>="ソースファイル"</attvalue> <attribute>xmlns</attribute><attvalue>="http://ufcpp.net/study/document"</attvalue><symbol>&gt;</symbol>
  <symbol>&lt;</symbol><element>section</element> <attribute>title</attribute><attvalue>="概要"</attvalue> <attribute>id</attribute><attvalue>="abst"</attvalue><symbol>&gt;</symbol>
    <symbol>&lt;</symbol><element>p</element><symbol>&gt;</symbol>
      XML 用
    <symbol>&lt;/</symbol><element>p</element><symbol>&gt;</symbol>
  <symbol>&lt;/</symbol><element>section</element><symbol>&gt;</symbol>
<symbol>&lt;/</symbol><element>document</element><symbol>&gt;</symbol>
</xsource>

<console xml:space="preserve" title="コンソール画面">
<prompt/><input>csc Test.cs</input>
<prompt/><input>Test.exe</input>
Hello World!<comment>お決まりのあれが表示される</comment>
</console>
```
ちなみに、さすがに reserved とか commenet とかのタグは、
ソースファイルから自動生成するためのプログラムを作って使っています。


## <a id="sec-generated-title-3"></a> <a id="result"></a>結果

```csharp
namespace Test
{
  class ConsoleApp1
  {
    static void Main(string[] args)
    {
      // お約束のあの文句を画面に表示。
      Console.Write("Hello World!\n");
    }
  }
}
```



```xml
<?xml version="1.0" encoding="utf-8"?>
<document title="ソースファイル" xmlns="http://ufcpp.net/study/document">
  <section title="概要" id="abst">
    <p>
      XML 用
    </p>
  </section>
</document>
```
```console
> csc Test.cs
> Test.exe
Hello World!
# ↓ お決まりのあれが表示される
```
