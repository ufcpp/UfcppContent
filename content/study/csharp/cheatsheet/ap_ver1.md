---
title: "C# 1.0"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver1/"
content_type: "Article"
published_at: "2016-06-11T00:00:00"
updated_at: "2019-02-18T19:37:33"
tags: []
umbraco_id: 1911
parent_id: 1174
sort_order: 3
aliases:
  - "/csharp/cheatsheet/ap_ver1/"
---

# C# 1.0

## <a id="sec-generated-title-1"></a>C# 1.0

<table>
<tr>
<th>リリース時期</th>
<td>2002/2</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio .NET 2002</li>
<li>.NET Framework 1.0</li>
<li>Visual Basic 7</li>
</td>
</tr>
</table>

C#は2000年6月に発表され、それから間もなくプレビュー版が公開されました。その後、2002年に正式リリースになっています。

当時、C#開発の指揮を取っていたAnders Hejlsbergは、元々Borland社でTurbo PascalやDelphiを作っていました。
マイクロソフトに移籍したのちはJ++ (Windows向けのJava拡張言語)に関わっています。
その後、大人の事情でマイクロソフトがJavaから撤退することになって、代わりに生まれたのがC#と言えます。

また、それ以前のWindows開発と言えば、主にC++かVisual Basicが用いられていました。
そこで、新言語であるC#の目標は、「C++の性能と、Visual Basicの生産性を兼ね備えた言語」でした。

このような背景から、C#は、Java、Delphi、Visual Basic、C++などから影響を受けた言語になっています。
C# 1.0時代は、Javaが一番近い言語でした。
そのJavaと比べてC#が特徴的だったのは以下のような機能です。

- 他の言語からの影響
  - [デリゲート](../functional/sp_delegate.md)
  - [プロパティ](../oop/oo_property.md)
  - [演算子](../oop/oo_operator.md)
- 性能面の向上
  - [値型](../resource/oo_reference.md)
  - [unsafe](../interop/sp_unsafe.md)
- 過去の資産との相互運用
  - [P/Invoke](../interop/sp_pinvoke.md)
  - [COM相互運用](../interop/sp4_cominterop.md)

また、Javaでいうところの「Java言語」と「Java仮想マシン」のレイヤーがはっきりと分かれています
(Java言語にあたるのがC#、Java仮想マシンにあたるのが.NET Frameworkです)。
これは、同じ仮想マシン上に異なるプログラミング言語(当時で言うと、Visual BasicとC++/CLI)を実装しているためです。
C++/CLIで書いたクラスを、Visual Basicで継承して、それをC#から使うといったようなことができます。

## <a id="sec-generated-title-2"></a>C# 1.1？ C# 1.2？

<table>
<tr>
<th>リリース時期</th>
<td>2003/4</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio .NET 2003</li>
<li>.NET Framework 1.1</li>
<li>Visual Basic 7.1</li>
</td>
</tr>
</table>

新しい技術にはバグがつきもので、Visual Studio .NET 2002から、たった1年で大幅なバグ修正が行われ、Visual Studio .NET 2003がリリースされました。
この際、C#にもほんの少しだけ手が入っています。

.NET Frameworkのバグ修正版が「.NET Framework 1.1」なので、それに合わせてC# 1.1と呼ばれることもあります。
一方で、仕様書的にはこれ以前に1度出し直しがあったようで、2003年の時点で「C# Language Specification 1.2」となっていることから、C# 1.2とも呼ばれます。
しかも、大した機能追加がないのでこのバージョンが言及される機会も少なく、正式にはC# 1.1なのかC# 1.2なのかははっきりしません<sup>※</sup>。

「2003年以前に1度出し直し」で、以下の2つが追加されています。

- `string` を `foreach` した時に最適化が掛かる(必ずしも `GetEnumerator`が呼ばれない)
- `foreach` の最後で `Dispose` メソッドが呼ばれる

また、「C# Language Specification 1.2」で追加されたのは以下の2つです。

- `#line hidden`ディレクティブ (参考: [プリプロセス命令](../misc/sp_preprocess.md#prepro))
- `/** */` 形式のドキュメント コメント (参考: [Documentation Comment](../misc/sp_xmldoc.md#doc))

<sup>※</sup> 1.1と1.2の両方の記述が見つかるし、「Visual Studio .NET 2003におけるC#の新機能」みたいな書き方でバージョンが明記されていなかったりするし。
