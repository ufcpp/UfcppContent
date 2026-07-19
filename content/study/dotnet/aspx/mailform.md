---
title: "メールフォーム"
source_url: "https://ufcpp.net/study/dotnet/aspx/mailform/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-06T14:15:11"
tags: []
umbraco_id: 1419
parent_id: 1414
sort_order: 4
aliases:
  - "/aspx/mailform"
  - "/aspx/mailform.html"
  - "/dotnet/aspx/mailform/"
  - "/study/aspx/mailform"
  - "/study/aspx/mailform.html"
---

# メールフォーム

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
ウェブサイト上にメールアドレスを掲載しちゃうと SPAM メールの餌食になるし、
ASP.NET ごしにメールしてもらうように、メールフォームを作成。

.NET Framework が、
System.Net.Mail の MailMessage とか SmtpClient とか、
メール送信機能も持っているので、
この辺りを利用。

ついでといってはなんですが、
マスタページを使って、他のページと見た目やメニューなどを統一してみる。


##<a id="sec-generated-title-2"></a> <a id="master"></a>マスタページ
サイト中の全てのページに対して、
そろえておきたい部分ってのがあります。
例えば、全ページにサイトのロゴを表示したいとか、
メニューを付けたいとか、そういうのです。

ASP.NET では、<strong id="master" class="keyword">マスタページ</strong>（master page）というものを使って、
複数のページに共通する部分をまとめておくことができます。

まあ、CGI アプリなんかでも、
以下のような HTML テンプレートを用意しておいて、
$$contents$$ の部分を置換して表示したりすることがありますが、
ASP.NET では、そういう仕組みが標準で用意されているわけです。


<pre class="xsource" title="CGI アプリでたまに使う HTML テンプレート">
<code><span class="bracket">&lt;</span><span class="element">html</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">link</span> <span class="attribute">rel</span><span class="attvalue">="stylesheet"</span> <span class="attribute">type</span><span class="attvalue">="text/css"</span> <span class="attribute">href</span><span class="attvalue">="main.css"</span> <span class="bracket">/&gt;</span>

 <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>$$title$$<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="head"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">img</span> <span class="attribute">src</span><span class="attvalue">="logo.jpg"</span> <span class="attribute">width</span><span class="attvalue">="320"</span> <span class="attribute">height</span><span class="attvalue">="80"</span> <span class="attribute">alt</span><span class="attvalue">="site logo"</span> <span class="bracket">/&gt;</span>
 <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

 <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="content"</span><span class="bracket">&gt;</span>
  $$contents$$
 <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

 <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="foot"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
  このサイトへのリンクはご自由にどうぞ
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
 <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>
ASP.NET のマスタページを作るには、
Visual Studio を使う場合、
ソリューションエクスプローラから [追加]→[新しい項目]→[マスタ ページ] を選びます。
まあ、結局中身はテキストファイルなので、
統合開発環境を使わないでも、
拡張子 .Master のファイルを作ってテキストエディタで中身を書いても OK。

例として、
「サイトのロゴ」「メニュー」「カウンタ」「フッタ」辺りを表示するマスタページを作ります。
ファイル名は仮に、Site.Master としておきます。


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
  <span class="bracket">&lt;</span><span class="element">img</span> <span class="attribute">src</span><span class="attvalue">="logo.jpg"</span> <span class="attribute">width</span><span class="attvalue">="320"</span> <span class="attribute">height</span><span class="attvalue">="80"</span> <span class="attribute">alt</span><span class="attvalue">="site logo"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="menu"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Default.aspx"</span><span class="bracket">&gt;</span>TOP<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">span</span> <span class="attribute">class</span><span class="attvalue">="menuItem"</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">a</span> <span class="attribute">href</span><span class="attvalue">="Mail.aspx"</span><span class="bracket">&gt;</span>メール<span class="bracket">&lt;/</span><span class="element">a</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">span</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">p</span> <span class="attribute">class</span><span class="attvalue">="counter"</span><span class="bracket">&gt;</span>
    総アクセス数: <span class="bracket">&lt;%</span>= Session["count"] <span class="bracket">%&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>

  <span class="bracket">&lt;</span><span class="element">div</span> <span class="attribute">class</span><span class="attvalue">="content"</span><span class="bracket">&gt;</span>
<em>    <span class="bracket">&lt;</span><span class="element">asp:ContentPlaceHolder</span> <span class="attribute">ID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">asp:ContentPlaceHolder</span><span class="bracket">&gt;</span></em>
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
&lt;asp:ContentPlaceHolder&gt; の部分に、ページごとのコンテンツが表示され、
残りの部分はこのマスタを適用する全ページに共通になります。

早速、このマスタを使う Web フォームページを作ってみましょう。
（Visual Studio を使うなら、
[追加]→[新しい項目]→[Web コンテンツフォーム] で雛形を作ってくれます。）
以下のように、
&lt;%@ Page %&gt; ディレクティブ中の
MasterPageFile のところに、上述のマスタページファイルの名前を書きます。


<pre class="xsource" title="Default.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span>
  <span class="attribute">MasterPageFile</span><span class="attvalue">="~/Site.Master"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="Default.aspx.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.Default"</span>
  <span class="attribute">Title</span><span class="attvalue">="My Website トップページ"</span> <span class="bracket">%&gt;</span>
<span class="bracket">&lt;</span><span class="element">asp:Content</span> <span class="attribute">ID</span><span class="attvalue">="Content1"</span>
  <span class="attribute">ContentPlaceHolderID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
Welcome to my website.
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
ASP.NET を使ったサイト構築の一例。
<span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>

<span class="bracket">&lt;/</span><span class="element">asp:Content</span><span class="bracket">&gt;</span>
</code></pre>
&lt;asp:Content&gt; の中身が、マスタページの &lt;asp:ContentPlaceHolder&gt; のところに展開されます。

ロゴとか CSS ファイルを用意して
（サンプル: [ロゴ](../../../../assets/resources/logo.jpg)、[CSS](../../../../assets/resources/main.css)）、
このページを表示すると、以下のようになります。

<figure>
	[![マスタページの適用結果の例](../../../../assets/media/ufcpp2000/dotnet/resources/Default_aspx.jpg)](../../../../assets/media/ufcpp2000/dotnet/resources/Default_aspx.jpg)
	<figcaption>マスタページの適用結果の例</figcaption>
</figure>



##<a id="sec-generated-title-3"></a> <a id="mailform"></a>メールフォーム
マスタページもできたところで、本題のメールフォームを作ってみましょう。

とりえあえず、Web コンテンツフォームの &lt;asp:Content&gt; の中に、
テキストボックスやボタンを適当に配置。


<pre class="xsource" title="Mail.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span>
  <span class="attribute">MasterPageFile</span><span class="attvalue">="~/Site.Master"</span> <span class="attribute">AutoEventWireup</span><span class="attvalue">="true"</span>
  <span class="attribute">CodeBehind</span><span class="attvalue">="Mail.aspx.cs"</span> <span class="attribute">Inherits</span><span class="attvalue">="WebsiteSample.Mail"</span>
  <span class="attribute">Title</span><span class="attvalue">="メールフォーム"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">asp:Content</span> <span class="attribute">ID</span><span class="attvalue">="Content1"</span>
  <span class="attribute">ContentPlaceHolderID</span><span class="attvalue">="ContentPlaceHolder1"</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">h2</span><span class="bracket">&gt;</span>メール送信フォーム<span class="bracket">&lt;/</span><span class="element">h2</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">table</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>お名前<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">asp:TextBox</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="textName"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">asp:TextBox</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>メールアドレス<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">asp:TextBox</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="textAddress"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">asp:TextBox</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>ホームページ(お持ちであれば)<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;</span><span class="element">asp:TextBox</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="textSiteUrl"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">asp:TextBox</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span> <span class="attribute">colspan</span><span class="attvalue">="2"</span><span class="bracket">&gt;</span>メッセージ<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span> <span class="attribute">colspan</span><span class="attvalue">="2"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">asp:TextBox</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="textMessage"</span>
      <span class="attribute">TextMode</span><span class="attvalue">="MultiLine"</span> <span class="attribute">Rows</span><span class="attvalue">="6"</span> <span class="attribute">Columns</span><span class="attvalue">="80"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">asp:TextBox</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span> <span class="attribute">colspan</span><span class="attvalue">="2"</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">asp:Button</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="buttonOk"</span> <span class="attribute">Text</span><span class="attvalue">="送信"</span>
      <span class="attribute">OnClick</span><span class="attvalue">="buttonOk_Click"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;</span><span class="element">asp:Button</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="buttonCancel"</span> <span class="attribute">Text</span><span class="attvalue">="取消"</span>
      <span class="attribute">OnClick</span><span class="attvalue">="buttonCancel_Click"</span> <span class="bracket">/&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">div</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">asp:Label</span> <span class="attribute">runat</span><span class="attvalue">="server"</span> <span class="attribute">ID</span><span class="attvalue">="labelResult"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">asp:Label</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">div</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">asp:Content</span><span class="bracket">&gt;</span>
</code></pre>
で、コードビハインド側に、[送信]・[取消]ボタンが押されたときのイベントハンドラを書きます。
.NET Framework では、
メールの送受信機能は System.Web.Mail 名前空間にまとまっています。

（メールアドレスや、SMTP サーバ名はソース中に書くんじゃなくて、
設定ファイルとかに書いておくのがいいんですが、
ちょっと手抜き。）

<pre class="source" title="Mail.aspx.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.Net.Mail;
<span class="reserved">using</span> System.Text.RegularExpressions;

<span class="reserved">namespace</span> WebsiteSample
{
  <span class="reserved">public partial class</span> Mail : System.Web.UI.Page
  {
    Regex regex = <span class="reserved">new</span> Regex(<span class="literal">@"^[-_\.a-zA-Z0-9]+\@[-_\.a-zA-Z0-9]+$"</span>);

    <span class="reserved">protected void</span> buttonOk_Click(<span class="reserved">object</span> sender, EventArgs e)
    {
      Match m = regex.Match(<span class="reserved">this</span>.textAddress.Text);
      <span class="reserved">if</span> (!m.Success)
      {
        <span class="reserved">this</span>.labelResult.Text = <span class="literal">"エラー: メールアドレスが不正です"</span>;
        <span class="reserved">return</span>;
      }

      <span class="reserved">if</span> (<span class="reserved">string</span>.IsNullOrEmpty(<span class="reserved">this</span>.textName.Text))
      {
        <span class="reserved">this</span>.labelResult.Text = <span class="literal">"エラー: お名前が入力されていまません"</span>;
        <span class="reserved">return</span>;
      }

      <span class="reserved">try</span>
      {
        MailAddress addrFrom = <span class="reserved">new</span> MailAddress(
          <span class="reserved">this</span>.textAddress.Text, <span class="reserved">this</span>.textName.Text);
        MailAddress addrTo = <span class="reserved">new</span> MailAddress(
          <span class="literal">"webmaster@xxx.aaa.jp"</span>);
        MailMessage msg = <span class="reserved">new</span> MailMessage(addrFrom, addrTo);

        msg.Subject = <span class="literal">"Chaos Dimension メールフォームからのメール"</span>;
        msg.Body =
          <span class="literal">"名前: "</span> + <span class="reserved">this</span>.textName.Text + <span class="literal">"\n"</span> +
          <span class="literal">"メールアドレス: "</span> + <span class="reserved">this</span>.textAddress.Text + <span class="literal">"\n"</span> +
          <span class="literal">"ウェブサイト: "</span> + <span class="reserved">this</span>.textSiteUrl.Text + <span class="literal">"\n\n"</span> +
          <span class="reserved">this</span>.textMessage.Text;

        SmtpClient client = <span class="reserved">new</span> SmtpClient(<span class="literal">"mail.xxx.aaa.jp"</span>);
        client.Send(msg);

        <span class="reserved">this</span>.labelResult.Text = <span class="literal">"送信完了"</span>;
      }
      <span class="reserved">catch</span> (Exception exc)
      {
        <span class="reserved">this</span>.labelResult.Text = <span class="literal">"送信エラー: "</span> + exc.Message;
      }
    }

    <span class="reserved">protected void</span> buttonCancel_Click(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="reserved">this</span>.textName.Text = <span class="reserved">string</span>.Empty;
      <span class="reserved">this</span>.textAddress.Text = <span class="reserved">string</span>.Empty;
      <span class="reserved">this</span>.textSiteUrl.Text = <span class="reserved">string</span>.Empty;
      <span class="reserved">this</span>.textMessage.Text = <span class="reserved">string</span>.Empty;
    }
  }
}
</code></pre>


これで、以下のようなページができあがるはずです。

<figure>
	[![メールフォームの例](../../../../assets/media/ufcpp2000/dotnet/resources/Mail_aspx.jpg)](../../../../assets/media/ufcpp2000/dotnet/resources/Mail_aspx.jpg)
	<figcaption>メールフォームの例</figcaption>
</figure>
