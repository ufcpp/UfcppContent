---
title: "ブログ表示（３）"
source_url: "https://ufcpp.net/study/dotnet/aspx/blog3/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-06T14:15:17"
tags: []
umbraco_id: 1422
parent_id: 1414
sort_order: 7
aliases:
  - "/study/aspx/blog3.html"
---

# ブログ表示（３）

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[ブログ表示（２）](blog2.md)」の続き。

利便性向上のためのおまけとして、
RSS フィードの出力と、
URL リライト・リダイレクトによる URL 短縮について説明します。
すなわち、以下のうちの 4、5 を説明。

1. Web コントロール

2. XSLT

3. クエリ文字列

4. HTTP リクエストのリライト・リダイレクト

5. RSS の作成



## <a id="sec-generated-title-2"></a> <a id="rewrite"></a>リライト・リダイレクト

「[ブログ表示（２）](blog2.md)」までの実装で、
BlogDate.aspx?year=2007&amp;month=6&amp;day=30 みたいな URL でリクエストを受けると、
2007年6月30日のブログを表示するようにできました。
でも、この長ったらしいクエリ文字列つきの URL がうっとうしいので、
2007/06/30.aspx というような URL を受け取って、
BlogDate.aspx?year=2007&amp;month=6&amp;day=30 にリダイレクトなりリライトするような仕組みを実装しましょう。

リダイレクトとリライトですが、
リダイレクトは要するに転送、
リライトの方は URL はそのままで中身だけ書き換えです。
例えば、2007/06/30.aspx というリクエストを受け取ったとき、
それぞれの動作は以下のようなものです。

* リダイレクト … クライアントに BlogDate.aspx?year=2007&amp;month=6&amp;day=30 にアクセスしなおしてもらう。

* リライト … クライアント側には 2007/06/30.aspx という URL が表示されたまま、BlogDate.aspx?year=2007&amp;month=6&amp;day=30 の結果を返す。



##### <a id="sec-generated-title-3"></a>Global.aspx を使ったリダイレクト/リライト

ASP.NET におけるリダイレクト/リライトの方法ですが、
Global.aspx 中の Application_BeginRequest イベントハンドラ内で、
Response.Redirect か Context.RewritePath メソッドを呼び出します。

URL の書き換えは、例えば、正規表現を使って以下のように行います。

```html
protected void Application_BeginRequest(object sender, EventArgs e)
{
  string url = Request.Url.AbsolutePath;

  Regex lookFor = new Regex(@"(\d{4})/(\d{2})/(\d{2})\.aspx");
  string sendTo = "BlogDate.aspx?mode=date&y=$1&m=$2&d=$3";

  if (!lookFor.IsMatch(url))
    continue;

  string result = lookFor.Replace(url, sendTo);
  Response.Redirect(result);
}
```


```html
protected void Application_BeginRequest(object sender, EventArgs e)
{
  string url = Request.Url.AbsolutePath;

  Regex lookFor = new Regex(@"(\d{4})/(\d{2})/(\d{2})\.aspx");
  string sendTo = "BlogDate.aspx?mode=date&y=$1&m=$2&d=$3";

  if (!lookFor.IsMatch(url))
    continue;

  string result = lookFor.Replace(url, sendTo);
  Context.RewritePath(result, false);
}
```



##### <a id="sec-generated-title-4"></a>汎用性を持たせる

もう少し汎用性を持たせてみましょう。
（元 URL, リダイレクト/リライト先 URL）のペアのリストを持っておいて、
リスト中の項目を1つ1つチェックしていくようにしてみます。

以下の例では、
2007/06/30.aspx を 20070630.aspx にリダイレクト、
20070630.aspx を BlogDate.aspx&amp;y=2007&amp;m=06&amp;d=30 にリライトします。

```html
public struct RewriteRule
{
  public Regex LookFor;
  public string SendTo;

  public RewriteRule(string lookFor, string sendTo)
  {
    this.LookFor = new Regex(lookFor);
    this.SendTo = sendTo;
  }
}

static RewriteRule[] rewriteRules = new RewriteRule[]
  {
    new RewriteRule(@"(\d{4})(\d{2})(\d{2})(\d+)\.aspx",
      "BlogDate.aspx?mode=date&y=$1&m=$2&d=$3&n=$4"),
    new RewriteRule(@"(\d{4})(\d{2})(\d{2})\.aspx",
      "BlogDate.aspx?mode=date&y=$1&m=$2&d=$3"),
  };
static RewriteRule[] redirectRules = new RewriteRule[]
  {
    new RewriteRule(@"(\d{4})/(\d{2})/(\d{2})/(\d+)\.aspx",
      "$1$2$3$4.aspx"),
    new RewriteRule(@"(\d{4})/(\d{2})/(\d{2})\.aspx",
      "$1$2$3.aspx"),
  };

protected void Application_BeginRequest(object sender, EventArgs e)
{
  string url = Request.Url.AbsolutePath;
  string result;

  foreach (RewriteRule rule in rewriteRules)
  {
    if (!rule.LookFor.IsMatch(url))
      continue;

    result = rule.LookFor.Replace(url, rule.SendTo);
    Context.RewritePath(result, false);
    return;
  }

  foreach (RewriteRule rule in redirectRules)
  {
    if (!rule.LookFor.IsMatch(url))
      continue;

    result = rule.LookFor.Replace(url, rule.SendTo);
    Response.Redirect(result);
    return;
  }
}
```


まあ、さらに汎用性を持たせたければ、
URL の変換ルールをソース中に埋め込むのではなく、
外部の設定ファイルに書いておくほうが望ましいです。

参考：
[ASP.NET での URL 書き換え](http://www.microsoft.com/japan/msdn/net/aspnet/URLRewriting.aspx)。


##### <a id="sec-generated-title-5"></a>HttpModule、HttpHandler

ここで説明したような、Global.aspx に処理を書く方法の他に、
HttpModule や HttpHandler を使う方法もあります。

参考：
[URLのリダイレクト](http://uchukamen.com/ASPNET20/URLRedirect/index.htm)。


### <a id="sec-generated-title-6"></a> <a id="redirect_note1"></a>注意点1: 元 URL

「[アクセスログを取ろう](logging.md)」でも同じような話がありましたが、
元 URL は ASP.NET エンジンを通して表示する物でなければなりません。
（txt を ASP.NET を通して表示する設定にしていない限り）
2007/06/30.txt というような URL で BlogDate.aspx&amp;y=2007&amp;m=06&amp;d=30 にリダイレクト/リライトはできません。

まあ、もし、例えば、2007/06/30 という URL をリダイレクト/リライトしたければ、
2007/06/30/ というフォルダを予め作った上で、
その中に Default.aspx というダミー（中身が空の）ファイルを置いておくことで、
リダイレクト/リライトをする事もできます。
（いちいちフォルダを作るのは面倒ですが。）

あるいは、
「[アクセスログを取ろう](logging.md)」のときと同様に、
サーバ上の設定で、
仮想ディレクトリ中のありとあらゆるファイルを一度 ASP.NET エンジンを通してから表示するように設定するという手もあります。
（多少、サーバに掛かる負荷が増える物の、その増加量は小さいそうです。）


### <a id="sec-generated-title-7"></a> <a id="redirect_note2"></a>注意点2: リライト時の rebase

URL のリライトをする際、1つ気をつけるべきことがあります。

例えば、2007/06/30.aspx を BlogDate.aspx&amp;y=2007&amp;m=06&amp;d=30 にリライトする場合、
クライアントは表示されているページを「2007/06 ディレクトリ中の 30.aspx というファイル」だと思っているわけです。
ページ中に &lt;img src="logo.jpg"/&gt; と書かれていた場合、
それは 2007/06/logo.jpg を意味します。

ところが、表示されているページは実際には、BlogDate.aspx なわけで、
BlogDate.aspx としては、&lt;img src="logo.jpg"/&gt; といわれれば、
BlogDate.aspx と同じディレクトリ内の log.jpg であって欲しいはずです。

（まあ、&lt;img src="~/logo.jpg"/&gt; と言うように絶対パスで書いておけばこんな問題も起きないんですが。どうしても絶対パス指定したくない場合には困る。）

こういう問題を回避するため、ASP.NET では、
2007/06/30.aspx が呼ばれたときには &lt;img src="logo.jpg"/&gt; の代わりに &lt;img src="../../logo.jpg"/&gt; を表示するような機構も持っています。
この機構を働かせるためには、以下の2点を守る必要があります。

1つは、RewritePath メソッドを呼ぶ際に、
Context.RewritePath(result, false); と言うように、第2引数に false を指定すること。
（この第2引数は、パスの rebase をするかどうかのフラグ。
true の時には、&lt;img src="~/logo.jpg"/&gt; といわれれば、
BlogDate.aspx では logo.jpg、
2007/06/30.aspx では 2007/06/logo.jpg だと思う。
false の時には、どちらの場合でも logo.jpg だと思う。
）

もう1つは、
&lt;img src="~/logo.jpg"/&gt; などに runat="server" 属性を付けることです。
（要するに、サーバ上で処理を掛けた物を表示する設定にする。）
（HTML コントロールよりも、asp:Image などの ASP.NET Web コントロールを使うこと推奨。）

具体例を挙げてみましょう。
まず、先ほど作った Global.aspx のリライトルールに以下の物を追加します。

```html
static RewriteRule[] rewriteRules = new RewriteRule[]
  {
    new RewriteRule(@"Test/(RewriteTest.aspx)",
      "$1"),
    new RewriteRule(@"(\d{4})/(\d{2})/(\d{2})\.aspx",
      "BlogDate.aspx?mode=date&y=$1&m=$2&d=$3"),
  };
```


で、RewriteText.aspx という名前で、以下のような Web フォームページを作ります。


```html
<%@ Page Language="C#" %>

<html>
<head runat="server">
  <title>リダイレクトのテスト</title>
</head>
<body>
  <form id="form1" runat="server">
  <p>
    <a href="Default.aspx">html &lt;a&gt; tag</a>
  </p>
  <p>
    <asp:HyperLink runat="server"
      NavigateUrl="Default.aspx">
      asp:HyperLink
      </asp:HyperLink>
  </p>
  </form>
</body>
</html>
```
このファイルの表示結果なんですが、
まず、RewriteText.aspx で表示した場合には、
&lt;body&gt; の中身には以下のような HTML が生成されます。


```html
  <p>
    <a href="Default.aspx">html &lt;a&gt; tag</a>
  </p>
  <p>
    <a href="Default.aspx">asp:HyperLink</a>
  </p>
```
これに対して、Test/RewriteText.aspx という URL で要求を受けて、
リライトした場合には、以下のような HTML になります。


```html
  <p>
    <a href="Default.aspx">html &lt;a&gt; tag</a>
  </p>
  <p>
    <a href="../Default.aspx">asp:HyperLink</a>
  </p>
```
runat="server" なしの a タグの方では、
href 内のパスの修正が掛かっていない状態になります。
（意図して修正が掛からないようにしているならいいけども、
そうでない場合は注意。）

ということで、2007/06/30.aspx というような URL から BlogDate.aspx にリライト処理するなら、
「[メールフォーム](mailform.md)」で作った「[マスタページ](mailform.md#master)」も、
以下のように書き換える必要があります。


```xml
<%@ Master Language="C#" AutoEventWireup="true"
  CodeBehind="Site.master.cs" Inherits="WebsiteSample.Site" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

  <link rel="stylesheet" type="text/css" href="main.css" />

  <title>無題のページ</title>
</head>
<body>

<form id="form1" runat="server">

  <p class="head">
  <asp:Image runat="server" ImageUrl="logo.jpg"
    Width="320" Height="80" AlternateText="site logo" />
  </p>
  <div class="menu">
    <span class="menuItem">
      <asp:HyperLink runat="server"
        NavigateUrl="Default.aspx">TOP</asp:HyperLink>
    </span>
    <span class="menuItem">
      <asp:HyperLink runat="server"
        NavigateUrl="Mail.aspx">メール</asp:HyperLink>
    </span>
    <span class="menuItem">
      <asp:HyperLink runat="server"
        NavigateUrl="BlogLatest.aspx">日記</asp:HyperLink>
    </span>
    <span class="menuItem">
      <asp:HyperLink runat="server"
        NavigateUrl="Rss.aspx">RSS</asp:HyperLink>
    </span>
  </div>

  <p class="counter">
    総アクセス数: <%= Session["count"] %>
  </p>

  <div class="content">
    <asp:ContentPlaceHolder ID="ContentPlaceHolder1" runat="server">
    </asp:ContentPlaceHolder>
  </div>

  <div class="foot">
  <p>
    このサイトへのリンクはご自由にどうぞ
  </p>
  </div>
</form>

</body>
</html>
```

## <a id="sec-generated-title-8"></a> <a id="rss"></a>RSS

最後に、RSS フィードを生成してみます。

＠IT の記事「[サイトの更新情報を提供する標準言語RSS](http://www.atmarkit.co.jp/fdotnet/special/rss/rss_03.html)」あたりを参考に。
汎用性を持たせるために、RSS を出力する部分だけ独立させて、
クラス化してみました → 
[RssWriter.cs](../../../../assets/media/ufcpp2000/dotnet/resources/RssWriter.cs)


この RssWriter クラスは、以下のように使います。

```csharp
RssWriter writer = new RssWriter();
this.writer.SiteName = "My Site";
this.writer.AdministratorName = "admin name";
this.writer.Url = "http://my.domain.net/";

this.writer.Add(pageUrl, title, new DateTime(year, month, day), digest);

this.writer.Write(Request.Url.AbsoluteUri, Response.OutputStream);
```


このクラスを使って、
「[ブログ表示（２）](blog2.md)」で作ったブログもどきの RSS フィードを作ってみましょう。
まず、
「[画像カウンタ](counter.md#image)」のときと同じく、
.aspx ファイルは空っぽで、コードビハインド（.aspx.cs）のみの Web フォームを作ります。
名前は Rss.aspx にしておきます。

そして、コードビハインドファイル（Rss.aspx.cs）の内容は以下のような感じ。

```html
using System;
using System.Web;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace WebsiteSample
{
  public partial class Rss : System.Web.UI.Page
  {
    RssWriter writer = new RssWriter();

    protected void Page_Load(object sender, EventArgs e)
    {
      Response.ContentType = "application/xml";
      Response.ContentEncoding = System.Text.Encoding.UTF8;

      this.writer.Write(Request.Url.AbsoluteUri, Response.OutputStream);
    }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);

      this.writer.SiteName = "My Site";
      this.writer.AdministratorName = "admin name";
      this.writer.Url = URL;
      this.ReadItems();
    }

    const int DEFAULT_NUM = 15;
    const int DEFAULT_DIGEST_LENGTH = 128;

    const string URL = "http://my.domain.net/";

    static readonly Regex regYyyyMmDd =
      new Regex(@"(?<y>\d\d\d\d)(?<m>\d\d)(?<d>\d\d)",
      RegexOptions.Compiled);
    static readonly Regex regCDATA =
      new Regex(@"\<!\[CDATA\[.*?\]\]\>",
      RegexOptions.Compiled);
    static readonly Regex regTags =
     new Regex(@"\<.*?\>",
     RegexOptions.Compiled);

    /// <summary>
    /// データを読み出して、RssWriter に項目を追加。
    /// </summary>
    void ReadItems()
    {
      int num, digestLen;
      if (!int.TryParse(Request.QueryString["num"], out num))
        num = DEFAULT_NUM;
      if (!int.TryParse(Request.QueryString["len"], out digestLen))
        digestLen = DEFAULT_DIGEST_LENGTH;

      string PATH = Server.MapPath(@"~/App_Data");

      DirectoryInfo dataDir = new DirectoryInfo(PATH);
      FileInfo[] files = dataDir.GetFiles("*.xml");

      Array.Sort(files,
        delegate(FileInfo a, FileInfo b)
        {
          return b.Name.CompareTo(a.Name);
        });

      foreach (FileInfo file in files)
      {
        Match m = regYyyyMmDd.Match(file.Name);
        if (!m.Success)
          continue;
        int year = int.Parse(m.Groups["y"].Value);
        int month = int.Parse(m.Groups["m"].Value);
        int day = int.Parse(m.Groups["d"].Value);

        XmlDocument doc = new XmlDocument();
        using (Stream stream = new FileStream(
          file.FullName, FileMode.Open,
          FileAccess.Read, FileShare.ReadWrite))
        {
          doc.Load(stream);
        }

        XmlNodeList list = doc.GetElementsByTagName("blog");
        foreach (XmlNode node in list)
        {
          string pageUrl = URL +
            string.Format("{0}/{1:00}/{2:00}.aspx",
            year, month, day);

          XmlAttribute att;
          string title = string.Empty;
          att = node.Attributes["title"];
          if (att != null) title = att.Value;
          att = node.Attributes["category"];
          if (att != null)
          {
            title += " [" + att.Value + "]";
          }

          string digest = Digest(node.InnerXml, digestLen);

          this.writer.Add(
            pageUrl, title,
            new DateTime(year, month, day), digest);
        }

        if (this.writer.Count >= num) break;
      }
    }

    /// <summary>
    /// XML のダイジェストを作る。
    /// XML のタグを取り除いて、テキストのみにして、最初の num 文字だけを返す。
    /// </summary>
    /// <param name="xml">XML 文字列</param>
    /// <param name="num">最初何文字を返すか</param>
    /// <returns>タグを取り除いた結果</returns>
    static string Digest(string xml, int num)
    {
      xml = xml.Replace("\n", "");
      xml = xml.Replace("\r", "");

      string digest;
      digest = regCDATA.Replace(xml, "");
      digest = regTags.Replace(xml, "");

      if (num >= digest.Length)
        return digest;

      digest = digest.Substring(0, num);
      return digest + " …";
    }
  }
}
```
