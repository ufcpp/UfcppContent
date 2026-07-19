---
title: "ブログ表示（１）"
source_url: "https://ufcpp.net/study/dotnet/aspx/blog1/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-07-07T18:10:58"
tags: []
umbraco_id: 1420
parent_id: 1414
sort_order: 5
aliases:
  - "/aspx/blog1"
  - "/aspx/blog1.html"
  - "/dotnet/aspx/blog1/"
  - "/study/aspx/blog1"
  - "/study/aspx/blog1.html"
---

# ブログ表示（１）

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
ブログっぽいものを作成。

一般に“ブログ”と呼ばれてるようなものほどちゃんとしたツールを作るつもりはなくて、
以下のような仕様のものを。

* Web フォーム自体に書き込み機能は持たせない。

* 記事は、ローカル上で（XML 形式で）書いたものを FTP などでサーバにアップする。

* 記事は、20070630.xml というように、日付に応じて yyyyMMdd.xml 形式のファイル名で保存。

* 日付に応じた XML を XSLT して表示するだけ。

* トラックバック機能はおろか、コメント機能すら作らない。


今どき、ブログのデータは（記事、コメント、トラックバック含め全部）データベースに持つのがいいと思うんですが、
自分のサイトにそこまでの機能が必要ないのと、
過去に自分の書きためてるものが XML 形式だったのでこういう仕様に。

技術的な面でいうと、ここで説明する内容は以下の通り。

1. Web コントロール

2. XSLT

3. クエリ文字列

4. HTTP リクエストのリライト・リダイレクト

5. RSS の作成


長く成りそうなので、3つに分割。
このページではまず、最新の日付のブログを表示するところまで。
技術面で言うと、1 と 2 を説明。


##<a id="sec-generated-title-2"></a> <a id="spec"></a>仕様
ブログもどきの仕様ですが、以下のような感じ。

* XML 形式で保存しておいたデータを読んで、XSLT を掛けた結果を表示するだけ。

* コメント機能もトラックバック機能もなし。

* XML は、2007年7月1日のブログなら 20070701.xml というように、 yyyyMMdd.xml という形式のファイル名を付ける。


以下のような XML データに対して、
図1のような結果を表示します。


<pre class="xsource" title="日記データ（20060731.xml）">
<code><span class="bracket">&lt;?</span><span class="element">xml</span> <span class="attribute">version</span><span class="attvalue">="1.0"</span> <span class="attribute">encoding</span><span class="attvalue">="Shift_JIS"</span><span class="bracket">?&gt;</span>
<span class="bracket">&lt;</span><span class="element">content</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">blog</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
全く会話をしないまま１日が過ぎた。
見た目がカタい人間にうつるからかもしれないと思い、
馬車内で今日は兜をかぶらなかった。
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
誰もそれに気付かないようだった。
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">blog</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;/</span><span class="element">content</span><span class="bracket">&gt;</span>
</code></pre>
<figure>
	[![表示結果](../../../../assets/media/ufcpp2000/dotnet/resources/blog1.jpg)](../../../../assets/media/ufcpp2000/dotnet/resources/blog1.jpg)
	<figcaption>表示結果</figcaption>
</figure>


ちなみに、ここで使った XSL スタイルシートはこんな感じ → [main.xsl](../../../../assets/media/ufcpp2000/dotnet/resources/main.xsl)。
XSL “Transformation” と言うほど賢い処理はしていなくて、
ほとんど HTML タグを素通ししてるだけ。

で、以下のような3つの Web フォームを作ります。

1. BlogLatest.aspx … クエリ文字列で「何日前か」を指定してブログを表示。

2. BlogDate.aspx … クエリ文字列で日付を指定してブログを表示。

3. BlogSelect.aspx … ページ中にドロップダウンリストやカレンダーコントロールを配置して、日付を選択してブログを表示。


また、これらの3つのページの共通部品として、
XML に XSLT を掛けた物を表示するだけの Web コントロールを作ります。


##<a id="sec-generated-title-3"></a> <a id="control"></a>Web コントロールの作成
Web コントロールというのは、
asp:Button や asp:Label などのように、
Web フォームページ中に配置する部品です。
System.Web.UI.UserControl を継承したクラスを作ることで自作することも可能です。

ここでは、
「[仕様](#spec)」で説明したような用途向けに、
XML 形式のデータを読み込んで、XSLT を掛けたものを表示するコントロールを作ってみます。

Web コントロール（拡張子 .ascx）の作り方は Web フォーム（.aspx）とほとんど同じです。
違いは、&lt;%@ Page %&gt; の変わりに &lt;%@ Control %&gt; ディレクティブを使うことと、
コードビハインド中のクラスの継承元が Page クラスから UserControl クラスに変わることです。

Visual Studio を使う場合、
ソリューションエクスプローラから、
[追加]→[新しい項目]→[Web ユーザコントロール] を選ぶことで雛形を作ってもらえます。

で、XML 表示コントロールですが、
まず、ascx ファイル中に、XSLT 結果を表示するための &lt;div&gt; タグを配置しておきます。
（ファイル名は ShowXml.ascx としておきます。）


<pre class="xsource" title="ShowXml.ascx">
<code><span class="bracket">&lt;%@ </span><span class="element">Control</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="ShowXml.ascx.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.ShowXml"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">id</span><span class="attvalue">="content"</span> <span class="bracket">/&gt;</span>
</code></pre>
コードビハインド側は以下のような感じ。
（例外処理とかはサボっています。）

<pre class="source" title="ShowXml.ascx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.Xml;
<span class="reserved">using</span> System.Xml.Xsl;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Text;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> ShowXml : System.Web.UI.UserControl
  {
    <span class="reserved">#region</span> プロパティ

    <span class="reserved">string</span> xmlFileName = <span class="reserved">null</span>;

    <span class="reserved">public string</span> XmlFileName
    {
      <span class="reserved">get</span> { <span class="reserved">return</span> xmlFileName; }
      <span class="reserved">set</span> { xmlFileName = value; }
    }

    <span class="reserved">string</span> xslFileName = <span class="reserved">null</span>;

    <span class="reserved">public string</span> XslFileName
    {
      <span class="reserved">get</span> { <span class="reserved">return</span> xslFileName; }
      <span class="reserved">set</span> { xslFileName = value; }
    }

    <span class="reserved">#endregion

    protected void</span> Page_PreRender(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="reserved">if</span> (<span class="reserved">this</span>.xmlFileName == <span class="reserved">null</span> || <span class="reserved">this</span>.xslFileName == <span class="reserved">null</span>
        || !File.Exists(<span class="reserved">this</span>.xmlFileName)
        || !File.Exists(<span class="reserved">this</span>.xslFileName)
        )
        <span class="reserved">return</span>;

      XslCompiledTransform xslt = <span class="reserved">new</span> XslCompiledTransform();
      xslt.Load(<span class="reserved">this</span>.XslFileName);
      XmlDocument xml = <span class="reserved">new</span> XmlDocument();
      xml.Load(<span class="reserved">this</span>.xmlFileName);

      XmlNodeReader reader = <span class="reserved">new</span> XmlNodeReader(xml);
      MemoryStream s = <span class="reserved">new</span> MemoryStream();
      XmlTextWriter writer = <span class="reserved">new</span> XmlTextWriter(s, Encoding.UTF8);
      xslt.Transform(reader, writer);

      s.Seek(0, SeekOrigin.Begin);
      StreamReader sr = <span class="reserved">new</span> StreamReader(s);

      <span class="reserved">string</span> result = sr.ReadToEnd();
      <span class="reserved">this</span>.content.InnerHtml = result;

      s.Dispose();
    }
  }
}
</code></pre>


XML のファイル名と XSL のファイル名を public なプロパティにしておきます。
ShowXml コントロールを使う側では、これらのプロパティにファイル名を指定して使います。

XML がらみの処理自体は、System.Xml 名前空間中のクラスを使って XML / XSLT の処理をしているだけなので、
詳しくは System.Xml や System.Xml.Xsl あたりのヘルプを参照してください。
XSLT 結果は、単純に &lt;div id="content" /&gt; の InnerHtml として表示します。

1つ注意が必要なのは、
表示処理を行っているイベントが、
今までよく使っていた Page_Load ではなく、
Page_PreRender なことです。

ASP.NET の処理の順番は、
Page_Load → プロパティの値の設定 → Page_PreRender の順番になっているようで、
Page_Load に処理を書いても、プロパティの値の設定結果が反映されません。

ちなみに、ASP.NET にはデータテンプレートという、
XML やデータベース中から取り出してきたデータを任意の形式で表示するための機構が用意されてたりするんで、
（使い方次第ではあるけど）そちらを使ってみることも推奨。


##<a id="sec-generated-title-4"></a> <a id="use_control"></a>Web コントロールの利用
前節で作った XML 表示コントロールを使って、
ブログデータを表示する Web フォームを作ってみます。
とりあえず、まず最初に、
最新の日付のブログを表示するだけの物を作って見ます。

まず、
ブログデータや XSL ファイルですが、
App_Data フォルダに格納する事にします。

<figure>
	[![App_Data フォルダとその中身の例](../../../../assets/media/ufcpp2000/dotnet/resources/App_Data.png)](../../../../assets/media/ufcpp2000/dotnet/resources/App_Data.png)
	<figcaption>App_Data フォルダとその中身の例</figcaption>
</figure>


自作した Web コントロールの使い方ですが、
以下のようになります。

* &lt;%@ Register %&gt; でコントロールを登録。

* 登録したコントロールをページ中に配置。


自作した XML 表示コントロール（ファイル名 ShowXml.ascx）を使うなら、
aspx ファイル中に以下のように書きます。


<pre class="xsource" title="BlogLatest.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span>
  <span class="attribute">MasterPageFile</span><span class="attvalue">="~/Site.Master"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="BlogLatest.aspx.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.BlogLatest"</span>
  <span class="attribute">Title</span><span class="attvalue">="日記"</span> <span class="bracket">%&gt;</span>
<em><span class="bracket">&lt;%@ </span><span class="element">Register</span> <span class="attribute">TagPrefix</span><span class="attvalue">="local"</span> <span class="attribute">TagName</span><span class="attvalue">="ShowXml"</span> <span class="attribute">Src</span><span class="attvalue">="~/ShowXml.ascx"</span> <span class="bracket">%&gt;</span></em>

<span class="bracket">&lt;</span><span class="element">asp:Content</span> <span class="attribute">ID</span><span class="attvalue">="Content1"</span>
  <span class="attribute">ContentPlaceHolderID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="blogHead"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">asp:Label</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="head"</span> <span class="bracket">/&gt;</span>
<span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

<em><span class="bracket">&lt;</span><span class="element">local:ShowXml</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="xmlContent"</span> <span class="bracket">/&gt;</span></em>

<span class="bracket">&lt;/</span><span class="element">asp:Content</span><span class="bracket">&gt;</span>
</code></pre>
で、最新の記事を表示したいなら、
コードビハインドファイルは以下のようにします。

<pre class="source" title="BlogLatest.aspx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> BlogLatest : System.Web.UI.Page
  {
    <span class="reserved">internal static</span> Regex yyyyMMdd =
      <span class="reserved">new</span> Regex(<span class="literal">@"(\d{4})(\d{2})(\d{2})\.xml"</span>);

    <span class="reserved">protected void</span> Page_Load(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="reserved">string</span> basePath = Context.Server.MapPath(<span class="literal">"~/App_Data"</span>);
      <span class="reserved">string</span>[] files = Directory.GetFiles(basePath, <span class="literal">"*.xml"</span>);
      Array.Sort(files);

      <span class="reserved">string</span> xmlFile = files[files.Length - 1];
      <span class="reserved">string</span> xslFile = basePath + <span class="literal">@"\main.xsl"</span>;

<em>      <span class="reserved">this</span>.xmlContent.XmlFileName = xmlFile;
      <span class="reserved">this</span>.xmlContent.XslFileName = xslFile;</em>

      Match match = yyyyMMdd.Match(xmlFile);
      <span class="reserved">if</span> (match.Success)
      {
        <span class="reserved">this</span>.head.Text = <span class="reserved">string</span>.Format(<span class="literal">"{0}年{1}月{2}日"</span>,
          match.Groups[1],
          match.Groups[2].ToString().TrimStart(<span class="literal">'0'</span>),
          match.Groups[3].ToString().TrimStart(<span class="literal">'0'</span>));
      }
    }
  }
}
</code></pre>


やっていることは、
App_Data フォルダ中から最新の日付の XML ファイルを探してきて、
それを ShowXml コントロールの XmlFileName プロパティに渡しているだけです。
（20070701.xml とかいう形式でデータを保存しているので、
ファイル名でソートすればそれがそのまま日付順でのソートになる。）

ついでに、日付を &lt;asp:Label ID="head"&gt; 中に表示。
