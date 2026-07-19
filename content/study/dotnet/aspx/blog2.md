---
title: "ブログ表示（２）"
source_url: "https://ufcpp.net/study/dotnet/aspx/blog2/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-06T14:15:15"
tags: []
umbraco_id: 1421
parent_id: 1414
sort_order: 6
aliases:
  - "/aspx/blog2"
  - "/aspx/blog2.html"
  - "/dotnet/aspx/blog2/"
  - "/study/aspx/blog2"
  - "/study/aspx/blog2.html"
---

# ブログ表示（２）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[ブログ表示（１）](blog1.md)」の続き。

以下の3パターンの表示方法を実装します。

1. BlogLatest.aspx … クエリ文字列で「何日前か」を指定してブログを表示。

2. BlogDate.aspx … クエリ文字列で日付を指定してブログを表示。

3. BlogSelect.aspx … ページ中にドロップダウンリストやカレンダーコントロールを配置して、日付を選択してブログを表示。


ブログもどき自体はこれで完成です。
技術面でいうと、以下の内容のうちの 3 を説明。

1. Web コントロール

2. XSLT

3. クエリ文字列

4. HTTP リクエストのリライト・リダイレクト

5. RSS の作成



## <a id="sec-generated-title-2"></a> <a id="latest"></a>「何日前か」を指定して表示

まず、クエリ文字列で「何日前か」を指定してブログを表示するページを作ります。
「[Web コントロールの利用](blog1.md#use_control)」で作った BlogLatest.aspx を元に少し書き換え。

ASP.NET では、受け取ったクエリ文字列を Request.QueryString に格納します。
例えば、
“http://my.domain.jp/BlogLatest.aspx?days=3”と言うような URL で HTTP リクエストがあったとすると、
Request.QueryString["days"] とすることで、"3" という文字列が得られます。

で、Request.QueryString から値を取り出すために、
以下のようなクラスを用意しておきます。

<pre class="source" title="Util.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Collections.Specialized;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public class</span> Util
  {
    <span class="reserved">internal static</span> Regex yyyyMMdd =
      <span class="reserved">new</span> Regex(<span class="literal">@"(\d{4})(\d{2})(\d{2})\.xml"</span>);

    <span class="comment">/// &lt;summary&gt;
    /// NameValueCollection から値を取り出す。
    /// 「キーに year がなければ y も試す」というように、
    /// 複数のキーのうちの最初に1つ合致したキーに対応する値を取得。
    /// 1つも合致しなければ defaultValue を返す。
    /// &lt;/summary&gt;</span>
    <span class="reserved">public static int</span> GetIntFrom(
      NameValueCollection collection,
      <span class="reserved">int</span> defultValue,
      <span class="reserved">params string</span>[] keys)
    {
      <span class="reserved">foreach</span> (<span class="reserved">string</span> key <span class="reserved">in</span> keys)
      {
        <span class="reserved">int</span> val;
        <span class="reserved">string</span> str = collection[key];
        <span class="reserved">if</span> (!<span class="reserved">string</span>.IsNullOrEmpty(str))
        {
          <span class="reserved">int</span>.TryParse(str, <span class="reserved">out</span> val);
          <span class="reserved">return</span> val;
        }
      }
      <span class="reserved">return</span> defultValue;
    }
  }
}
</code></pre>


この GetIntFrom メソッドを使って、
BlogLatest の Page_Load イベントハンドラを以下のように書き換えます。

<pre class="source" title="BlogLatest.aspx.cs を書き換え" lang="">
<code><span class="reserved">protected void</span> Page_Load(<span class="reserved">object</span> sender, EventArgs e)
{
  <em><span class="reserved">int</span> days = Util.GetIntFrom(Request.QueryString, 0, <span class="literal">"days"</span>, <span class="literal">"d"</span>);</em>

  <span class="reserved">string</span> basePath = Context.Server.MapPath(<span class="literal">"~/App_Data"</span>);
  <span class="reserved">string</span>[] files = Directory.GetFiles(basePath, <span class="literal">"*.xml"</span>);
  Array.Sort(files);

  <em><span class="reserved">string</span> xmlFile = files[files.Length - 1 - days];</em>
  <span class="reserved">string</span> xslFile = basePath + <span class="literal">@"\main.xsl"</span>;

  <span class="reserved">this</span>.xmlContent.XmlFileName = xmlFile;
  <span class="reserved">this</span>.xmlContent.XslFileName = xslFile;

  Match match = Util.yyyyMMdd.Match(xmlFile);
  <span class="reserved">if</span> (match.Success)
  {
    <span class="reserved">this</span>.head.Text = <span class="reserved">string</span>.Format(<span class="literal">"{0}年{1}月{2}日"</span>,
      match.Groups[1],
      match.Groups[2].ToString().TrimStart(<span class="literal">'0'</span>),
      match.Groups[3].ToString().TrimStart(<span class="literal">'0'</span>));
  }
}
</code></pre>


これで、BlogLatest.aspx?days=2 という呼び出し方をすることで、
最新から2日前のブログを表示できるようになります。


## <a id="sec-generated-title-3"></a> <a id="date"></a>日付を指定して表示

次は、日付を指定して表示する Web フォームを作ります。
ファイル名は BlogDate.aspx としておきます。

Page_Load イベントハンドラ内の処理が違うだけで、
他はほとんど BlogLatest の方と同じなので、
コードビハインドのみを示します。

<pre class="source" title="BlogDate.aspx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.IO;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> BlogDate : System.Web.UI.Page
  {
    <span class="reserved">protected void</span> Page_Load(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="reserved">int</span> day = Util.GetIntFrom(Request.QueryString, 0, <span class="literal">"day"</span>, <span class="literal">"d"</span>);
      <span class="reserved">int</span> month = Util.GetIntFrom(Request.QueryString, 0, <span class="literal">"month"</span>, <span class="literal">"m"</span>);
      <span class="reserved">int</span> year = Util.GetIntFrom(Request.QueryString, 0, <span class="literal">"year"</span>, <span class="literal">"y"</span>);

      <span class="reserved">string</span> basePath = Context.Server.MapPath(<span class="literal">"~/App_Data"</span>);

      <span class="reserved">string</span> xmlFile = basePath +
        <span class="reserved">string</span>.Format(<span class="literal">@"\{0}{1:00}{2:00}.xml"</span>, year, month, day);

      <span class="reserved">if</span> (!File.Exists(xmlFile)) <span class="reserved">return</span>;

      <span class="reserved">string</span> xslFile = basePath + <span class="literal">@"\main.xsl"</span>;

      <span class="reserved">this</span>.xmlContent.XmlFileName = xmlFile;
      <span class="reserved">this</span>.xmlContent.XslFileName = xslFile;

      <span class="reserved">this</span>.head.Text = <span class="reserved">string</span>.Format(<span class="literal">"{0}年{1}月{2}日"</span>,
        year, month, day);
    }
  }
}
</code></pre>


これで例えば、BlogDate.aspx?year=1998&amp;month=5&amp;day=21 というような書き方で、
1998年5月21日の記事が表示されます。


## <a id="sec-generated-title-4"></a> <a id="control"></a>ドロップダウンリストやカレンダーコントロールで日付を選択

最後に、
クエリ文字列ではなく、
ページ中にドロップダウンリストやカレンダーコントロールを配置して、
選択した日付の記事を表示する Web フォームを作ります。

まず、年月日を選択するためのドロップダウンリスト・テキストボックスや、
カレンダーコントロールを aspx 中に記述します。


<pre class="xsource" title="BlogSelect.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span>
  <span class="attribute">MasterPageFile</span><span class="attvalue">="~/Site.Master"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="BlogSelect.aspx.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.BlogSelect"</span>
  <span class="attribute">Title</span><span class="attvalue">="日記"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;%@ </span><span class="element">Register</span> <span class="attribute">TagPrefix</span><span class="attvalue">="local"</span> <span class="attribute">TagName</span><span class="attvalue">="ShowXml"</span> <span class="attribute">Src</span><span class="attvalue">="~/ShowXml.ascx"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">asp:Content</span> <span class="attribute">ID</span><span class="attvalue">="Content1"</span>
  <span class="attribute">ContentPlaceHolderID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="blogHead"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">asp:DropDownList</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="listYear"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">asp:DropDownList</span><span class="bracket">&gt;</span>
年

<span class="bracket">&lt;</span><span class="element">asp:DropDownList</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="listMonth"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="1"</span> <span class="attribute">Value</span><span class="attvalue">="1"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="2"</span> <span class="attribute">Value</span><span class="attvalue">="2"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="3"</span> <span class="attribute">Value</span><span class="attvalue">="3"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="4"</span> <span class="attribute">Value</span><span class="attvalue">="4"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="5"</span> <span class="attribute">Value</span><span class="attvalue">="5"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="6"</span> <span class="attribute">Value</span><span class="attvalue">="6"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="7"</span> <span class="attribute">Value</span><span class="attvalue">="7"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="8"</span> <span class="attribute">Value</span><span class="attvalue">="8"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="9"</span> <span class="attribute">Value</span><span class="attvalue">="9"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="10"</span> <span class="attribute">Value</span><span class="attvalue">="10"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="11"</span> <span class="attribute">Value</span><span class="attvalue">="11"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:ListItem</span> <span class="attribute">Text</span><span class="attvalue">="12"</span> <span class="attribute">Value</span><span class="attvalue">="12"</span> <span class="bracket">/&gt;</span>
<span class="bracket">&lt;/</span><span class="element">asp:DropDownList</span><span class="bracket">&gt;</span>
月

<span class="bracket">&lt;</span><span class="element">asp:TextBox</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="textDay"</span> <span class="attribute">Width</span><span class="attvalue">="20"</span> <span class="bracket">/&gt;</span>
日の記事を
<span class="bracket">&lt;</span><span class="element">asp:Button</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="buttonShow"</span> <span class="attribute">Text</span><span class="attvalue">="表示"</span>
  <span class="attribute">OnClick</span><span class="attvalue">="buttonShow_Click"</span> <span class="bracket">/&gt;</span>
<span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="blogHead"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">asp:Calendar</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="calendar"</span>
  <span class="attribute">OnSelectionChanged</span><span class="attvalue">="calendar_SelectionChanged"</span> <span class="bracket">/&gt;</span>
<span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="blogHead"</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">asp:Label</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="head"</span> <span class="bracket">/&gt;</span>
<span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">local:ShowXml</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="xmlContent"</span> <span class="bracket">/&gt;</span>

<span class="bracket">&lt;/</span><span class="element">asp:Content</span><span class="bracket">&gt;</span>
</code></pre>
で、コードビハインド側では以下のような処理を行います。

* 初期化： ブログを付け始めた年から今年までをドロップダウンリストの項目として追加

* [表示] ボタン押下時の処理

* カレンダーで日付を選択したときの処理


ソースコードは以下のようになります。

<pre class="source" title="BlogSelect.aspx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.Web.UI;
<span class="reserved">using</span> System.Web.UI.WebControls;
<span class="reserved">using</span> System.IO;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> BlogSelect : System.Web.UI.Page
  {
    <span class="reserved">protected override void</span> OnInit(EventArgs e)
    {
      <span class="reserved">base</span>.OnInit(e);

      <span class="reserved">const int</span> startedYear = 1998;
      DateTime today = DateTime.Today;
      <span class="reserved">int</span> year = today.Year;
      <span class="reserved">int</span> month = today.Month;
      <span class="reserved">int</span> day = today.Day;

      ListItem li;
      <span class="reserved">for</span> (<span class="reserved">int</span> y = startedYear; y &lt;= year; ++y)
      {
        li = <span class="reserved">new</span> ListItem();
        li.Text = y.ToString();
        li.Value = y.ToString();
        <span class="reserved">this</span>.listYear.Items.Add(li);
      }
      <span class="reserved">this</span>.listYear.SelectedIndex = year - startedYear;

      <span class="reserved">this</span>.listMonth.SelectedIndex = month - 1;

      <span class="reserved">this</span>.textDay.Text = day.ToString();
    }

    <span class="reserved">protected void</span> buttonShow_Click(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="reserved">int</span> y = <span class="reserved">int</span>.Parse(<span class="reserved">this</span>.listYear.SelectedItem.Value);
      <span class="reserved">int</span> m = <span class="reserved">int</span>.Parse(<span class="reserved">this</span>.listMonth.SelectedItem.Value);
      <span class="reserved">int</span> d = <span class="reserved">int</span>.Parse(<span class="reserved">this</span>.textDay.Text);

      <span class="reserved">this</span>.Show(y, m, d);
    }

    <span class="reserved">protected void</span> calendar_SelectionChanged(<span class="reserved">object</span> sender, EventArgs e)
    {
      DateTime selected = <span class="reserved">this</span>.calendar.SelectedDate;

      <span class="reserved">this</span>.Show(selected.Year, selected.Month, selected.Day);
    }

    <span class="reserved">void</span> Show(<span class="reserved">int</span> year, <span class="reserved">int</span> month, <span class="reserved">int</span> day)
    {
      <span class="reserved">string</span> basePath = Context.Server.MapPath(<span class="literal">"~/App_Data"</span>);

      <span class="reserved">string</span> xmlFile = basePath +
        <span class="reserved">string</span>.Format(<span class="literal">@"\{0}{1:00}{2:00}.xml"</span>, year, month, day);

      <span class="reserved">if</span> (!File.Exists(xmlFile)) <span class="reserved">return</span>;

      <span class="reserved">string</span> xslFile = basePath + <span class="literal">@"\main.xsl"</span>;

      <span class="reserved">this</span>.xmlContent.XmlFileName = xmlFile;
      <span class="reserved">this</span>.xmlContent.XslFileName = xslFile;

      <span class="reserved">this</span>.head.Text = <span class="reserved">string</span>.Format(<span class="literal">"{0}年{1}月{2}日"</span>,
        year, month, day);
    }
  }
}
</code></pre>


以下のような感じで、選択した日付の記事が表示されるはずです。

<figure>
	[![ドロップダウンリストなどで日付を選択](../../../../assets/media/ufcpp2000/dotnet/resources/BlogSelect.png)](../../../../assets/media/ufcpp2000/dotnet/resources/BlogSelect.png)
	<figcaption>ドロップダウンリストなどで日付を選択</figcaption>
</figure>
