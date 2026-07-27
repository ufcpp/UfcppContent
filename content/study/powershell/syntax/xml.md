---
title: "XML"
source_url: "https://ufcpp.net/study/powershell/syntax/xml/"
content_type: "Article"
published_at: "2007-05-27T00:00:00"
updated_at: "2017-04-06T17:41:02"
tags: []
umbraco_id: 1584
parent_id: 1577
sort_order: 6
aliases:
  - "/study/powershell/xml.html"
---

# XML

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（書きかけ）

PowerShell では、
XML 内のデータを普通のオブジェクトと同じような構文で読み出すことができます。

例えば、このページ、XML で書いた物を HTML に変換しているんですが、
元の XML は以下のような感じになっています。

    <?xml version="1.0" encoding="UTF-8"?>
    <?xml-stylesheet type="text/xsl" href="main.xsl" ?>
    <document title="XML" since="2007/5/27">
    <section title="概要" id="abst">
    <p>
        PowerShell では、
        XML 内のデータを普通のオブジェクトと同じような構文で読み出すことができます。
    </p>
    </section>
    後略

で、PowerShell で
```console
> $xml = [xml](Get-Content xml.xml)
> $xml.document.title
XML
> $xml.document.section[0].title
概要
> $xml.document.section[0].id
abst
> $xml.document.section[0].p
```
という感じで中身にアクセス可能。
PowerShell では、
XML 内のデータを普通のオブジェクトと同じような構文で読み出すことができます。

応用例：
RSS を読んだり
