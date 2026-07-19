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
  - "/aspx/blog3"
  - "/aspx/blog3.html"
  - "/dotnet/aspx/blog3/"
  - "/study/aspx/blog3"
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

<pre class="source" title="リダイレクト（Global.aspx のコードビハインド中に追加）" lang="">
<code><span class="reserved">protected void</span> Application_BeginRequest(<span class="reserved">object</span> sender, EventArgs e)
{
  <span class="reserved">string</span> url = Request.Url.AbsolutePath;

  Regex lookFor = <span class="reserved">new</span> Regex(<span class="literal">@"(\d{4})/(\d{2})/(\d{2})\.aspx"</span>);
  <span class="reserved">string</span> sendTo = <span class="literal">"BlogDate.aspx?mode=date&amp;y=$1&amp;m=$2&amp;d=$3"</span>;

  <span class="reserved">if</span> (!lookFor.IsMatch(url))
    <span class="reserved">continue</span>;

  <span class="reserved">string</span> result = lookFor.Replace(url, sendTo);
  Response.Redirect(result);
}
</code></pre>


<pre class="source" title="リライト（Global.aspx のコードビハインド中に追加）" lang="">
<code><span class="reserved">protected void</span> Application_BeginRequest(<span class="reserved">object</span> sender, EventArgs e)
{
  <span class="reserved">string</span> url = Request.Url.AbsolutePath;

  Regex lookFor = <span class="reserved">new</span> Regex(<span class="literal">@"(\d{4})/(\d{2})/(\d{2})\.aspx"</span>);
  <span class="reserved">string</span> sendTo = <span class="literal">"BlogDate.aspx?mode=date&amp;y=$1&amp;m=$2&amp;d=$3"</span>;

  <span class="reserved">if</span> (!lookFor.IsMatch(url))
    <span class="reserved">continue</span>;

  <span class="reserved">string</span> result = lookFor.Replace(url, sendTo);
  Context.RewritePath(result, <span class="reserved">false</span>);
}
</code></pre>



##### <a id="sec-generated-title-4"></a>汎用性を持たせる

もう少し汎用性を持たせてみましょう。
（元 URL, リダイレクト/リライト先 URL）のペアのリストを持っておいて、
リスト中の項目を1つ1つチェックしていくようにしてみます。

以下の例では、
2007/06/30.aspx を 20070630.aspx にリダイレクト、
20070630.aspx を BlogDate.aspx&amp;y=2007&amp;m=06&amp;d=30 にリライトします。

<pre class="source" title="Global.aspx のコードビハインド中に追加" lang="">
<code><span class="reserved">public struct</span> RewriteRule
{
  <span class="reserved">public</span> Regex LookFor;
  <span class="reserved">public string</span> SendTo;

  <span class="reserved">public</span> RewriteRule(<span class="reserved">string</span> lookFor, <span class="reserved">string</span> sendTo)
  {
    <span class="reserved">this</span>.LookFor = <span class="reserved">new</span> Regex(lookFor);
    <span class="reserved">this</span>.SendTo = sendTo;
  }
}

<span class="reserved">static</span> RewriteRule[] rewriteRules = <span class="reserved">new</span> RewriteRule[]
  {
    <span class="reserved">new</span> RewriteRule(<span class="literal">@"(\d{4})(\d{2})(\d{2})(\d+)\.aspx"</span>,
      <span class="literal">"BlogDate.aspx?mode=date&amp;y=$1&amp;m=$2&amp;d=$3&amp;n=$4"</span>),
    <span class="reserved">new</span> RewriteRule(<span class="literal">@"(\d{4})(\d{2})(\d{2})\.aspx"</span>,
      <span class="literal">"BlogDate.aspx?mode=date&amp;y=$1&amp;m=$2&amp;d=$3"</span>),
  };
<span class="reserved">static</span> RewriteRule[] redirectRules = <span class="reserved">new</span> RewriteRule[]
  {
    <span class="reserved">new</span> RewriteRule(<span class="literal">@"(\d{4})/(\d{2})/(\d{2})/(\d+)\.aspx"</span>,
      <span class="literal">"$1$2$3$4.aspx"</span>),
    <span class="reserved">new</span> RewriteRule(<span class="literal">@"(\d{4})/(\d{2})/(\d{2})\.aspx"</span>,
      <span class="literal">"$1$2$3.aspx"</span>),
  };

<span class="reserved">protected void</span> Application_BeginRequest(<span class="reserved">object</span> sender, EventArgs e)
{
  <span class="reserved">string</span> url = Request.Url.AbsolutePath;
  <span class="reserved">string</span> result;

  <span class="reserved">foreach</span> (RewriteRule rule <span class="reserved">in</span> rewriteRules)
  {
    <span class="reserved">if</span> (!rule.LookFor.IsMatch(url))
      <span class="reserved">continue</span>;

    result = rule.LookFor.Replace(url, rule.SendTo);
    Context.RewritePath(result, <span class="reserved">false</span>);
    <span class="reserved">return</span>;
  }

  <span class="reserved">foreach</span> (RewriteRule rule <span class="reserved">in</span> redirectRules)
  {
    <span class="reserved">if</span> (!rule.LookFor.IsMatch(url))
      <span class="reserved">continue</span>;

    result = rule.LookFor.Replace(url, rule.SendTo);
    Response.Redirect(result);
    <span class="reserved">return</span>;
  }
}
</code></pre>


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

<pre class="source" title="Test/RewriteText.aspx を RewriteText.aspx にリライト" lang="">
<code><span class="reserved">static</span> RewriteRule[] rewriteRules = <span class="reserved">new</span> RewriteRule[]
  {
<em>    <span class="reserved">new</span> RewriteRule(<span class="literal">@"Test/(RewriteTest.aspx)"</span>,
      <span class="literal">"$1"</span>),</em>
    <span class="reserved">new</span> RewriteRule(<span class="literal">@"(\d{4})/(\d{2})/(\d{2})\.aspx"</span>,
      <span class="literal">"BlogDate.aspx?mode=date&amp;y=$1&amp;m=$2&amp;d=$3"</span>),
  };
</code></pre>


で、RewriteText.aspx という名前で、以下のような Web フォームページを作ります。


<pre class="xsource" title="RewriteText.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">html</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>リダイレクトのテスト<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">form</span> <span class="attribute">id</span><span class="attvalue">="form1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>html &amp;lt;a&amp;gt; tag<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">asp:HyperLink</span> <span class="attribute">runat</span><span class="attvalue">="server"</span>
      <span class="attribute">NavigateUrl</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>
      asp:HyperLink
      <span class="bracket">&lt;/</span><span class="element">asp:HyperLink</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">form</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>
このファイルの表示結果なんですが、
まず、RewriteText.aspx で表示した場合には、
&lt;body&gt; の中身には以下のような HTML が生成されます。


<pre class="xsource" title="RewriteText.aspx の要求結果">
<code>  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>html &amp;lt;a&amp;gt; tag<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>asp:HyperLink<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
</code></pre>
これに対して、Test/RewriteText.aspx という URL で要求を受けて、
リライトした場合には、以下のような HTML になります。


<pre class="xsource" title="RewriteText.aspx の要求結果">
<code>  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>html &amp;lt;a&amp;gt; tag<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    <em><span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="../Default.aspx"</span><span class="bracket">&gt;</span>asp:HyperLink<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span></em>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
</code></pre>
runat="server" なしの a タグの方では、
href 内のパスの修正が掛かっていない状態になります。
（意図して修正が掛からないようにしているならいいけども、
そうでない場合は注意。）

ということで、2007/06/30.aspx というような URL から BlogDate.aspx にリライト処理するなら、
「[メールフォーム](mailform.md)」で作った「[マスタページ](mailform.md#master)」も、
以下のように書き換える必要があります。


<pre class="xsource" title="Site.Master">
<code><span class="bracket">&lt;%@ </span><span class="element">Master</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="Site.master.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.Site"</span> <span class="bracket">%&gt;</span>

&lt;!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
  "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"&gt;

<span class="bracket">&lt;</span><span class="element">html</span> <span class="attribute">xmlns</span><span class="attvalue">="http://www.w3.org/1999/xhtml"</span> <span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">type</span><span class="attvalue">="text/css"</span> <span class="attribute">href</span><span class="attvalue">="main.css"</span> <span class="bracket">/&gt;</span>

  <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>無題のページ<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">form</span> <span class="attribute">id</span><span class="attvalue">="form1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">p</span> <span class="attribute">class</span><span class="attvalue">="head"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:Image</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ImageUrl</span><span class="attvalue">="logo.jpg"</span>
    <span class="attribute">Width</span><span class="attvalue">="320"</span> <span class="attribute">Height</span><span class="attvalue">="80"</span> <span class="attribute">AlternateText</span><span class="attvalue">="site logo"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="menu"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">asp:HyperLink</span> <span class="attribute">runat</span><span class="attvalue">="server"</span>
        <span class="attribute">NavigateUrl</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>TOP<span class="bracket">&lt;/</span><span class="element">asp:HyperLink</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">asp:HyperLink</span> <span class="attribute">runat</span><span class="attvalue">="server"</span>
        <span class="attribute">NavigateUrl</span><span class="attvalue">="Mail.aspx"</span><span class="bracket">&gt;</span>メール<span class="bracket">&lt;/</span><span class="element">asp:HyperLink</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">asp:HyperLink</span> <span class="attribute">runat</span><span class="attvalue">="server"</span>
        <span class="attribute">NavigateUrl</span><span class="attvalue">="BlogLatest.aspx"</span><span class="bracket">&gt;</span>日記<span class="bracket">&lt;/</span><span class="element">asp:HyperLink</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">asp:HyperLink</span> <span class="attribute">runat</span><span class="attvalue">="server"</span>
        <span class="attribute">NavigateUrl</span><span class="attvalue">="Rss.aspx"</span><span class="bracket">&gt;</span>RSS<span class="bracket">&lt;/</span><span class="element">asp:HyperLink</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">p</span> <span class="attribute">class</span><span class="attvalue">="counter"</span><span class="bracket">&gt;</span>
    総アクセス数: <span class="bracket">&lt;%</span>= Session["count"] <span class="bracket">%&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="content"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">asp:ContentPlaceHolder</span> <span class="attribute">ID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">asp:ContentPlaceHolder</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="foot"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    このサイトへのリンクはご自由にどうぞ
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">form</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>

## <a id="sec-generated-title-8"></a> <a id="rss"></a>RSS

最後に、RSS フィードを生成してみます。

＠IT の記事「[サイトの更新情報を提供する標準言語RSS](http://www.atmarkit.co.jp/fdotnet/special/rss/rss_03.html)」あたりを参考に。
汎用性を持たせるために、RSS を出力する部分だけ独立させて、
クラス化してみました → 
[RssWriter.cs](../../../../assets/media/ufcpp2000/dotnet/resources/RssWriter.cs)


この RssWriter クラスは、以下のように使います。

<pre class="source" title="RssWriter クラス" lang="">
<code>RssWriter writer = <span class="reserved">new</span> RssWriter();
<span class="reserved">this</span>.writer.SiteName = <span class="literal">"My Site"</span>;
<span class="reserved">this</span>.writer.AdministratorName = <span class="literal">"admin name"</span>;
<span class="reserved">this</span>.writer.Url = <span class="literal">"http://my.domain.net/"</span>;

<span class="reserved">this</span>.writer.Add(pageUrl, title, <span class="reserved">new</span> DateTime(year, month, day), digest);

<span class="reserved">this</span>.writer.Write(Request.Url.AbsoluteUri, Response.OutputStream);
</code></pre>


このクラスを使って、
「[ブログ表示（２）](blog2.md)」で作ったブログもどきの RSS フィードを作ってみましょう。
まず、
「[画像カウンタ](counter.md#image)」のときと同じく、
.aspx ファイルは空っぽで、コードビハインド（.aspx.cs）のみの Web フォームを作ります。
名前は Rss.aspx にしておきます。

そして、コードビハインドファイル（Rss.aspx.cs）の内容は以下のような感じ。

<pre class="source" title="Rss.aspx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.IO;
<span class="reserved">using</span> System.Xml;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> Rss : System.Web.UI.Page
  {
    RssWriter writer = <span class="reserved">new</span> RssWriter();

    <span class="reserved">protected void</span> Page_Load(<span class="reserved">object</span> sender, EventArgs e)
    {
      Response.ContentType = <span class="literal">"application/xml"</span>;
      Response.ContentEncoding = System.Text.Encoding.UTF8;

      <span class="reserved">this</span>.writer.Write(Request.Url.AbsoluteUri, Response.OutputStream);
    }

    <span class="reserved">protected override void</span> OnInit(EventArgs e)
    {
      <span class="reserved">base</span>.OnInit(e);

      <span class="reserved">this</span>.writer.SiteName = <span class="literal">"My Site"</span>;
      <span class="reserved">this</span>.writer.AdministratorName = <span class="literal">"admin name"</span>;
      <span class="reserved">this</span>.writer.Url = URL;
      <span class="reserved">this</span>.ReadItems();
    }

    <span class="reserved">const int</span> DEFAULT_NUM = 15;
    <span class="reserved">const int</span> DEFAULT_DIGEST_LENGTH = 128;

    <span class="reserved">const string</span> URL = <span class="literal">"http://my.domain.net/"</span>;

    <span class="reserved">static readonly</span> Regex regYyyyMmDd =
      <span class="reserved">new</span> Regex(<span class="literal">@"(?&lt;y&gt;\d\d\d\d)(?&lt;m&gt;\d\d)(?&lt;d&gt;\d\d)"</span>,
      RegexOptions.Compiled);
    <span class="reserved">static readonly</span> Regex regCDATA =
      <span class="reserved">new</span> Regex(<span class="literal">@"\&lt;!\[CDATA\[.*?\]\]\&gt;"</span>,
      RegexOptions.Compiled);
    <span class="reserved">static readonly</span> Regex regTags =
     <span class="reserved">new</span> Regex(<span class="literal">@"\&lt;.*?\&gt;"</span>,
     RegexOptions.Compiled);

    <span class="comment">/// &lt;summary&gt;
    /// データを読み出して、RssWriter に項目を追加。
    /// &lt;/summary&gt;</span>
    <span class="reserved">void</span> ReadItems()
    {
      <span class="reserved">int</span> num, digestLen;
      <span class="reserved">if</span> (!<span class="reserved">int</span>.TryParse(Request.QueryString[<span class="literal">"num"</span>], <span class="reserved">out</span> num))
        num = DEFAULT_NUM;
      <span class="reserved">if</span> (!<span class="reserved">int</span>.TryParse(Request.QueryString[<span class="literal">"len"</span>], <span class="reserved">out</span> digestLen))
        digestLen = DEFAULT_DIGEST_LENGTH;

      <span class="reserved">string</span> PATH = Server.MapPath(<span class="literal">@"~/App_Data"</span>);

      DirectoryInfo dataDir = <span class="reserved">new</span> DirectoryInfo(PATH);
      FileInfo[] files = dataDir.GetFiles(<span class="literal">"*.xml"</span>);

      Array.Sort(files,
        <span class="reserved">delegate</span>(FileInfo a, FileInfo b)
        {
          <span class="reserved">return</span> b.Name.CompareTo(a.Name);
        });

      <span class="reserved">foreach</span> (FileInfo file <span class="reserved">in</span> files)
      {
        Match m = regYyyyMmDd.Match(file.Name);
        <span class="reserved">if</span> (!m.Success)
          <span class="reserved">continue</span>;
        <span class="reserved">int</span> year = <span class="reserved">int</span>.Parse(m.Groups[<span class="literal">"y"</span>].Value);
        <span class="reserved">int</span> month = <span class="reserved">int</span>.Parse(m.Groups[<span class="literal">"m"</span>].Value);
        <span class="reserved">int</span> day = <span class="reserved">int</span>.Parse(m.Groups[<span class="literal">"d"</span>].Value);

        XmlDocument doc = <span class="reserved">new</span> XmlDocument();
        <span class="reserved">using</span> (Stream stream = <span class="reserved">new</span> FileStream(
          file.FullName, FileMode.Open,
          FileAccess.Read, FileShare.ReadWrite))
        {
          doc.Load(stream);
        }

        XmlNodeList list = doc.GetElementsByTagName(<span class="literal">"blog"</span>);
        <span class="reserved">foreach</span> (XmlNode node <span class="reserved">in</span> list)
        {
          <span class="reserved">string</span> pageUrl = URL +
            <span class="reserved">string</span>.Format(<span class="literal">"{0}/{1:00}/{2:00}.aspx"</span>,
            year, month, day);

          XmlAttribute att;
          <span class="reserved">string</span> title = <span class="reserved">string</span>.Empty;
          att = node.Attributes[<span class="literal">"title"</span>];
          <span class="reserved">if</span> (att != <span class="reserved">null</span>) title = att.Value;
          att = node.Attributes[<span class="literal">"category"</span>];
          <span class="reserved">if</span> (att != <span class="reserved">null</span>)
          {
            title += <span class="literal">" ["</span> + att.Value + <span class="literal">"]"</span>;
          }

          <span class="reserved">string</span> digest = Digest(node.InnerXml, digestLen);

          <span class="reserved">this</span>.writer.Add(
            pageUrl, title,
            <span class="reserved">new</span> DateTime(year, month, day), digest);
        }

        <span class="reserved">if</span> (<span class="reserved">this</span>.writer.Count &gt;= num) <span class="reserved">break</span>;
      }
    }

    <span class="comment">/// &lt;summary&gt;
    /// XML のダイジェストを作る。
    /// XML のタグを取り除いて、テキストのみにして、最初の num 文字だけを返す。
    /// &lt;/summary&gt;
    /// &lt;param name="xml"&gt;XML 文字列&lt;/param&gt;
    /// &lt;param name="num"&gt;最初何文字を返すか&lt;/param&gt;
    /// &lt;returns&gt;タグを取り除いた結果&lt;/returns&gt;</span>
    <span class="reserved">static string</span> Digest(<span class="reserved">string</span> xml, <span class="reserved">int</span> num)
    {
      xml = xml.Replace(<span class="literal">"\n"</span>, <span class="literal">""</span>);
      xml = xml.Replace(<span class="literal">"\r"</span>, <span class="literal">""</span>);

      <span class="reserved">string</span> digest;
      digest = regCDATA.Replace(xml, <span class="literal">""</span>);
      digest = regTags.Replace(xml, <span class="literal">""</span>);

      <span class="reserved">if</span> (num &gt;= digest.Length)
        <span class="reserved">return</span> digest;

      digest = digest.Substring(0, num);
      <span class="reserved">return</span> digest + <span class="literal">" …"</span>;
    }
  }
}
</code></pre>
