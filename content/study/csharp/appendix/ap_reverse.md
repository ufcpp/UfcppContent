---
title: "逆引き辞書"
source_url: "https://ufcpp.net/study/csharp/appendix/ap_reverse/"
content_type: "Article"
published_at: "2002-05-19T00:00:00"
updated_at: "2007-06-24T00:00:00"
tags: []
umbraco_id: 1383
parent_id: 1377
sort_order: 5
aliases:
  - "/csharp/ap_reverse"
  - "/csharp/ap_reverse.html"
  - "/csharp/appendix/ap_reverse/"
  - "/study/csharp/ap_reverse"
  - "/study/csharp/ap_reverse.html"
---

# 逆引き辞書

##<a id="sec-generated-title-1"></a> <a id="folder"></a>フォルダ
* [プログラムを起動したフォルダを取得する](#folder_01)
* [プログラムを置いてあるフォルダを取得する](#folder_02)
* [特殊なフォルダのパスを取得する](#folder_03)

####<a id="sec-generated-title-2"></a> <a id="folder_01">プログラムを起動したフォルダを取得する</a>
パスは起動したフォルダを基準とした相対パスになります。
したがって、<code>new DirectoryInfo(".");</code>
でプログラムを起動したフォルダを取得できます。

####<a id="sec-generated-title-3"></a> <a id="folder_02">プログラムを置いてあるフォルダを取得する</a>
プログラムの実行ファイルを置いてあるフォルダは
<code>Application.StartupPath</code> で取得できます。

####<a id="sec-generated-title-4"></a> <a id="folder_03">特殊なフォルダのパスを取得する</a>
<code>System.Environment.GetFolderPath</code> を利用することで
デスクトップやマイドキュメントなどの特殊なフォルダのパスを取得できます。



##<a id="sec-generated-title-5"></a> <a id="file"></a>ファイル操作
* [シフトJISのテキストを読み書きしたい。](#file_01)

####<a id="sec-generated-title-6"></a> <a id="file_01">シフトJISのテキストを読み書きしたい。</a>
文字コードを指定してファイルを読み書きするには、
<code>StreamReader/StreamWriter</code> クラスのコンストラクタで <code>Encoding</code> クラスを渡してやります。

日本語 Windows 環境では、<code>Encoding.Default</code> でシフト JIS の Encoding が得られる。

<pre class="source" title="StreamReader/StreamWriter" lang="">
<code>StreamReader fin  = <span class="reserved">new</span> StreamReader(
  <span class="literal">"in file"</span>, Encoding.Default);
StreamWriter fout = <span class="reserved">new</span> StreamWriter(
  <span class="literal">"out file"</span>, <span class="reserved">false</span>, Encoding.Default);
</code></pre>


ちなみに、シフト JIS のコードページは 932。
（日本語 Windows では）以下のコードと上のコードは同じ結果に。

<pre class="source" title="StreamReader/StreamWriter" lang="">
<code>StreamReader fin  = <span class="reserved">new</span> StreamReader(
  <span class="literal">"in file"</span>, Encoding.GetEncoding(932));
StreamWriter fout = <span class="reserved">new</span> StreamWriter(
  <span class="literal">"out file"</span>, <span class="reserved">false</span>, Encoding.GetEncoding(932));
</code></pre>




##<a id="sec-generated-title-7"></a> <a id="string"></a>文字列操作
* [stringからシフトJISコードのbyte配列に変換したい](#string_01)
* [正規表現を使いたい](#string_02)
* [文字が平仮名・片仮名・漢字かどうか調べたい](#string_03)

####<a id="sec-generated-title-8"></a> <a id="string_01">stringからシフトJISコードのbyte配列に変換したい</a>
<code>Encoding</code> クラスの GetBytes メソッドを使います。

文字コードの指定は <code>Encoding</code> クラスを用いて行います。
シフトJISのコードページは 932 です。
また、日本語 Windows 環境では <code>Encoding.Default</code> によってシフトJISのエンコーディングクラスを取得できます。

<pre class="source" title="StreamReader/StreamWriter" lang="">
<code>Encoding.GetEncoding(932).GetBytes(str);
Encoding.Default.GetBytes(str);
</code></pre>


####<a id="sec-generated-title-9"></a> <a id="string_02">正規表現を使いたい</a>
<code>System.Text.RegularExpressions</code> クラスを用いることで文字列の正規表現検索が出来ます。
.NET Framework の正規表現は Perl5 の正規表現と親和性の高いデザインになっています。

参考： 「[文字列関係](../lib/lib_string.md)」

詳しくはヘルプの「[.NET Framework の正規表現](https://msdn.microsoft.com/ja-jp/library/hs600312(v=vs.110).aspx)」や「[正規表現言語要素](https://msdn.microsoft.com/library/az24scfc(v=vs.100).aspx)」をご覧ください。

####<a id="sec-generated-title-10"></a> <a id="string_03">文字が平仮名・片仮名・漢字かどうか調べたい</a>
正規表現を使って文字のクラスを調べられる。
平仮名は文字クラス IsHiraganaに、
片仮名は IsKatakana、漢字は IsCJKUnifiedIdeographs にマッチする。

<pre class="source" title="IsHiragana, IsKatakana, IsCJKUnifiedIdeographs" lang="">
<code><span class="comment">// 平仮名だけからなる単語にマッチ</span>
Regex hira  = <span class="reserved">new</span> Regex(<span class="literal">@"\b\p{IsHiragana}+\b"</span>);
<span class="comment">// 片仮名にマッチ</span>
Regex kata  = <span class="reserved">new</span> Regex(<span class="literal">@"\p{IsKatakana}"</span>);
<span class="comment">// 漢字にマッチ</span>
Regex kanji = <span class="reserved">new</span> Regex(<span class="literal">@"\p{IsCJKUnifiedIdeographs}"</span>);
</code></pre>


その他にも文字クラス名を指定することでさまざまな文字クラスが判定可能。
文字クラス名は[unicode.org](http://www.unicode.org/)にある[ブロック名一覧](http://www.unicode.org/Public/UNIDATA/Blocks.txt)のブロック名にIsをつけたもの。



##<a id="sec-generated-title-11"></a> <a id="datetime"></a>時刻
* [任意の形式で文字列化されている時刻を DateTime 型に変換したい](#datetime_01)

####<a id="sec-generated-title-12"></a> <a id="datetime_01">任意の形式で文字列化されている時刻を DateTime 型に変換したい</a>
<code>DateTime.ParseExact</code> メソッドで、
以下のようにしてフォーマットを指定。

<pre class="source" title="DateTime.ParseExact" lang="">
<code><span class="reserved">string</span> str = <span class="literal">"08/Jul/2006:03:28:50 +0900"</span>;

Date d = DateTime.ParseExact(str,
  <span class="literal">"d'/'MMM'/'yyyy':'HH':'mm':'ss zzz"</span>,
  System.Globalization.DateTimeFormatInfo.InvariantInfo,
  System.Globalization.DateTimeStyles.None);

</code></pre>




##<a id="sec-generated-title-13"></a> <a id="output"></a>出力
* [C の printf みたいにフォーマット出力したい](#output_01)

####<a id="sec-generated-title-14"></a> <a id="output_01">C の printf みたいにフォーマット出力したい</a>
<code>System.Console.Write(string, params object[])</code> を使えばフォーマット出力が可能です。
パラメータの書式は以下の通り

<pre class="source" title="Console.Write の書式指定" lang="">
<code>{N,M:format}
</code></pre>


* <code>N</code>: パラメータのインデックス

* <code>M</code>: 表示する幅。不足分はスペースで埋められる。正の数の場合右詰、負の数の場合左詰。

* <code>format</code>: 書式指定。パラメータが<code>IFormattable</code>インターフェースを実装している場合、この書式に従って整形される。


ちなみに、Console.Write の書式指定の仕方は、
string.Format メソッドの書式指定と同じです。
string.Format メソッドに関しては、
「[文字列関係](../lib/lib_string.md)」で概説しているので、そちらを参照。

また、詳細は <s>ms-help://MS.NETFrameworkSDK.JA/cpguidenf/html/cpconformattingoverview.htm</s>で見ることが出来ます。



##<a id="sec-generated-title-15"></a> <a id="input"></a>入力
* [C の scanf みたいなことをしたい](#input_01)

####<a id="sec-generated-title-16"></a> <a id="input_01">C の scanf みたいなことをしたい</a>
例)
<pre class="source" title="C の scanf" lang="">
<code>int width, height;
scanf("width %d height %d", &amp;width, &amp;height);
</code></pre>

1)正規表現を使う
<pre class="source" title="Regex を使った入力文字列解析" lang="">
<code><span class="reserved">const string</span> pattern = <span class="literal">@"height (?&lt;height&gt;\w+) width (?&lt;width&gt;\w+)"</span>; 

Regex x = <span class="reserved">new</span> Regex(pattern);
<span class="reserved">string</span> str = Console.ReadLine();
Match m = x.Match(str); 

<span class="reserved">int</span> width = m.Group(<span class="literal">"width"</span>);
<span class="reserved">int</span> height = m.Group(<span class="literal">"height"</span>);

</code></pre>

2) Split を使う
<pre class="source" title="string.Split を使った入力文字列解析" lang="">
<code><span class="reserved">string</span> str = Console.ReadLine();
sring strs = str.Split(<span class="literal">' '</span>);
<span class="reserved">int</span> width = <span class="reserved">int</span>.Parse(strs[1]);
<span class="reserved">int</span> height = <span class="reserved">int</span>.Parse(strs[3]);

</code></pre>




##<a id="sec-generated-title-17"></a> <a id="resource"></a>リソース
* [リソースファイルの作成](#d179e351_01)
* [リソースの利用](#d179e351_02)

####<a id="sec-generated-title-18"></a> <a id="d179e351_01">リソースファイルの作成</a>
<pre class="source" title="リソースファイルの作成" lang="">
<code><span class="reserved">using</span> System.Drawing;
<span class="reserved">using</span> System.Resources; 

<span class="reserved">class</span> CreateResource
{
  <span class="reserved">static void</span> Main()
  {
    <span class="reserved">using</span>(ResourceWriter writer =
      <span class="reserved">new</span> ResourceWriter(<span class="literal">"リソースファイル.resources"</span>))
    {
      writer.AddResource(<span class="literal">"リソース名"</span>, <span class="reserved">new</span> Icon(<span class="literal">"ファイル.ico"</span>));
      writer.Generate();
    }
  }
}

</code></pre>


後はコンパイル時に <code>/res</code> を指定するだけ。

####<a id="sec-generated-title-19"></a> <a id="d179e351_02">リソースの利用</a>
<pre class="source" title="リソースの利用" lang="">
<code>ResourceManager rm = <span class="reserved">new</span> ResourceManager(
  <span class="literal">"アセンブリ名"</span>, <span class="reserved">this</span>.GetType().Assembly);
<span class="reserved">this</span>.Icon = (System.Drawing.Icon)rm.GetObject(<span class="literal">"リソース名"</span>);
</code></pre>




##<a id="sec-generated-title-20"></a> <a id="interop"></a>ネイティブコードとの相互運用性
* [構造体のレイアウト](#interop_01)
* [構造体を C 言語の共用体のように使う](#interop_02)

####<a id="sec-generated-title-21"></a> <a id="interop_01">構造体のレイアウト</a>
<code>System.Runtime.InteropServices.StructLayout</code> アトリビュートを使って構造体のレイアウトを指定できます。

* <code>LayoutKind.Auto</code>… 自動レイアウト

* <code>LayoutKind.Explicit</code>…<code>FieldOffset</code>アトリビュートを使って明示的にレイアウト

* <code>LayoutKind.Sequential</code>… 宣言した順にレイアウト


<code>LayoutKind.Sequential</code> をコンストラクタに渡した場合、
<code>Pack</code> プロパティでパッキングサイズを調整できます。
デフォルトの <code>Pack</code> の値は 8 です。

####<a id="sec-generated-title-22"></a> <a id="interop_02">構造体を C 言語の共用体のように使う</a>
構造体のレイアウトを <code>LayoutKind.Explicit</code> にすることで
C 言語の共用体のような使い方が出来ます。

<pre class="source" title="LayoutKind.Explicit で C 言語の共用体ライクな構造体を作る" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
<span class="reserved">struct</span> Hoge
{
  [FieldOffset(0)] <span class="reserved">public byte</span> B;
  [FieldOffset(0)] <span class="reserved">public int</span> N;
} 

<span class="reserved">class</span> Test 
{ 
  <span class="reserved">static void</span> Main() { 
    Hoge h = <span class="reserved">new</span> Hoge(); 
    h.N = 257; 

    Console.WriteLine(h.B); <span class="comment">// 1 が表示される</span>
  } 
} 

</code></pre>




##<a id="sec-generated-title-23"></a> <a id="serialize"></a>シリアライズ
* [オブジェクトをシリアライズする](#serialize_01)

####<a id="sec-generated-title-24"></a> <a id="serialize_01">オブジェクトをシリアライズする</a>
* バイナリで … System.Runtime.Serialization.Formatters.Binary.BinaryFormatter

* SOAPメッセージで … System.Runtime.Serialization.Formatters.Soap.SoapFormatter

* XMで … System.Xml.Serialization.XmlSerializer




##<a id="sec-generated-title-25"></a> <a id="win32"></a>Win32
* [システムイベントを拾う](#win32_01)

####<a id="sec-generated-title-26"></a> <a id="win32_01">システムイベントを拾う</a>
<code>Microsoft.Win32.SystemEvents</code> クラスのパブリックイベントで表示設定変更、時刻の変更、メモリ不足などのイベントを拾える。



##<a id="sec-generated-title-27"></a> <a id="gui"></a>GUI
* [Windows XP のビジュアルスタイルを使用する](#d179e548_01)
* [フォームの再描写がちらつく](#d179e548_02)

####<a id="sec-generated-title-28"></a> <a id="d179e548_01">Windows XP のビジュアルスタイルを使用する</a>
マニフェストを使用して Comctl32.dll version 6 がバインディングされるように設定する。
具体的な方法は以下の通り。

以下のXMLを「[実行ファイル名].manifest」という名前で保存し、
実行ファイルと同じディレクトリに置く。


<pre class="xsource" title="マニフェスト">
<code><span class="bracket">&lt;</span>?xml version="1.0" encoding="UTF-8" standalone="yes"?<span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">assembly</span>
  <span class="attribute">xmlns</span><span class="attvalue">="urn:schemas-microsoft-com:asm.v1"</span>
  <span class="attribute">manifestVersion</span><span class="attvalue">="1.0"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">dependency</span><span class="bracket">&gt;</span>
   <span class="bracket">&lt;</span><span class="element">dependentAssembly</span><span class="bracket">&gt;</span>
     <span class="bracket">&lt;</span><span class="element">assemblyIdentity</span>
       <span class="attribute">type</span><span class="attvalue">="win32"</span>
       <span class="attribute">name</span><span class="attvalue">="Microsoft.Windows.Common-Controls"</span>
       <span class="attribute">version</span><span class="attvalue">="6.0.0.0"</span>
       <span class="attribute">processorArchitecture</span><span class="attvalue">="X86"</span>
       <span class="attribute">publicKeyToken</span><span class="attvalue">="6595b64144ccf1df"</span>
       <span class="attribute">language</span><span class="attvalue">="*"</span>
     /<span class="bracket">&gt;</span>
   <span class="bracket">&lt;</span>/<span class="element">dependentAssembly</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">dependency</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span>/<span class="element">assembly</span><span class="bracket">&gt;</span>
</code></pre>
マニフェストを実行ファイル中に埋め込むためには、
リソースタイプ「RT_MANIFEST」、ID「1」のリソースとして埋め込む。
Visual Studio .NET を使えば、以下の手順で簡単にマニフェストの埋め込みが出来る。

* メニューの[ファイル]から[ファイルを開く]を選び、マニフェストを埋め込みたい実行ファイルを開く。

* 開いた実行ファイルを右クリックし[リソースの追加]を選び、[インポート]をクリックする。

* 埋め込みたいマニフェストファイルを選ぶ。

* マニフェストタイプに[RT_MANIFEST]と入力する。

* 追加されたマニフェストリソースのプロパティを開き、IDを「1」に変更する。


####<a id="sec-generated-title-29"></a> <a id="d179e548_02">フォームの再描写がちらつく</a>
Paint イベントを使ってフォームの際描写を行うと描写がちらついてしまう。
(OnPait メソッドでは、背景の塗りつぶしを行ってから Paint イベントハンドラを呼び出すため。)
OnPaintBackground メソッドをオーバーライドして描写を行えばちらつきはなくなる。



##<a id="sec-generated-title-30"></a> <a id="tool"></a>ツール
* [PreJIT って出来ないの？](#d179e682_01)
* [フリーの開発環境はないの？](#d179e682_02)

####<a id="sec-generated-title-31"></a> <a id="d179e682_01">PreJIT って出来ないの？</a>
Ngen.exe (native image generater)で出来ます。
詳しくは<s>ms-help://MS.NETFrameworkSDK.JA/cptools/html/cpgrfnativeimagegeneratorngenexe.htm</s>を参照してください。

####<a id="sec-generated-title-32"></a> <a id="d179e682_02">フリーの開発環境はないの？</a>
https://sourceforge.jp/projects/sharpdevelop-jp

Visual C# 2005 Express Edision が無料で利用可能になった。
