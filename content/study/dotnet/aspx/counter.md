---
title: "アクセスカウンタを作ろう"
source_url: "https://ufcpp.net/study/dotnet/aspx/counter/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-06T14:15:09"
tags: []
umbraco_id: 1418
parent_id: 1414
sort_order: 3
aliases:
  - "/aspx/counter"
  - "/aspx/counter.html"
  - "/dotnet/aspx/counter/"
  - "/study/aspx/counter"
  - "/study/aspx/counter.html"
---

# アクセスカウンタを作ろう

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

アクセスログ記録に続き、アクセスカウンタを。

せっかくだから色々やってみようということで、
以下のような作り方をします。

* カウント数は、サーバ上のファイルに書いておきます。

* カウントは Global.asax の Session_Start で行います。

* 前回のアクセス時のカウント数を Cookie で記録。リピート状況を見ます。

* カウント数の表示方法を何パターンか説明（Web フォーム中に表示 ＆ 画像カウンタ）


そのため、以下のような項目について解説。

* Session State。

* Cookie の利用。

* ASP.NET を使った画像カウンタ



## <a id="sec-generated-title-2"></a> <a id="count"></a>カウント処理

アクセスログ記録と同じく、
カウント処理は Global.asax の Session_Start イベントハンドラで行うことにします。

ファイルの読み書きに関しては、
やることはほとんど「[ログの記録](logging.md#logging)」と同じです。
カウント数を保存しておくファイルですが、
ここで説明する例では総アクセス数しか記録しませんが、
将来的に「今日だけのアクセス数」とかも記録することを考えて、
count\total.txt というファイルに書くことにします。

例えば、以下のような感じのコードを Global.asax.cs（Global.asax のコードビハインド）のクラス中に追加します。

<pre class="source" title="Global.asax.cs に追加" lang="">
<code><span class="reserved">string</span> GetTotalCount()
{
  <span class="reserved">string</span> filename = Request.PhysicalApplicationPath +
    <span class="literal">@"\count\total.txt"</span>;

  Application.Lock();

  <span class="reserved">int</span> num;
  <span class="reserved">using</span> (FileStream fs = <span class="reserved">new</span> FileStream(
    filename, FileMode.OpenOrCreate,
    FileAccess.ReadWrite, FileShare.None))
  {
    StreamReader sr = <span class="reserved">new</span> StreamReader(fs);
    <span class="reserved">string</span> line = sr.ReadLine();

    <span class="reserved">if</span> (<span class="reserved">string</span>.IsNullOrEmpty(line) || !<span class="reserved">int</span>.TryParse(line, <span class="reserved">out</span> num))
    {
      num = 0;
    }

    line = (num + 1).ToString();

    fs.Seek(0, SeekOrigin.Begin);

    StreamWriter sw = <span class="reserved">new</span> StreamWriter(fs);
    sw.WriteLine(line);
    sw.Flush();
  }
  Application.UnLock();

  <span class="reserved">return</span> num.ToString();
}

<span class="reserved">protected void</span> Session_Start(Object sender, EventArgs e)
{
  <span class="reserved">if</span> (CheckExcludeList())
    <span class="reserved">return</span>;

  AddAccessLog();

  <span class="reserved">string</span> count = GetTotalCount();
  Session[<span class="literal">"TotalCount"</span>] = count;
}
</code></pre>


この例中、最後の行の Session["TotalCount"] = count; なんですが、
Session はセッション状態を記憶しておくために使うプロパティです。
（HttpApplication や Page クラスのプロパティで、
HttpSessionState 型。）
ここで Session["TotalCount"] に代入した値は、
セッションがタイムアウトするまでずっと保持されます。


## <a id="sec-generated-title-3"></a> <a id="cookie"></a>Cookie の利用

ここで、カウント数もアクセスログに記録してみることにします。
また、前回のアクセス時のカウント数も記録して、
リピート状況をログに取れるようにします。
前回のアクセスカウント数は、Cookie を使ってクライアント側に記憶しておいてもらいます。

Cookie は、
Request.Cookies でクライアントから送られて来た Cookie を取得し、
Response.Cookies でサーバから送り返す Cookie を設定します。

ということで、
「[ログの記録](logging.md#logging)」で作った AddAccessLog メソッドを以下のように書き換えます。

<pre class="source" title="AddAccessLog を修正" lang="">
<code><span class="reserved">void</span> AddAccessLog(<span class="reserved">string</span> count)
{
      <span class="reserved">string</span> basePath = Request.PhysicalApplicationPath + <span class="literal">@"\accesslog\"</span>;
      DateTime now = DateTime.Now;
      <span class="reserved">string</span> filename = basePath
        + <span class="reserved">string</span>.Format(<span class="literal">"{0}{1:00}.csv"</span>, now.Year, now.Month);

<em>  <span class="reserved">string</span> prev;
  <span class="reserved">if</span> (Request.Cookies[<span class="literal">"PREV"</span>] != <span class="reserved">null</span>)
    prev = Request.Cookies[<span class="literal">"PREV"</span>].Value;
  <span class="reserved">else</span>
    prev = count;

  Response.Cookies[<span class="literal">"PREV"</span>].Value = count;</em>

  Application.Lock();

  <span class="reserved">using</span> (StreamWriter sw = <span class="reserved">new</span> StreamWriter(filename, <span class="reserved">true</span>))
  {
    sw.Write(<span class="literal">"\""</span> + DateTime.Now.ToString() + <span class="literal">"\","</span>);
    sw.Write(<span class="literal">"\""</span> +
      System.Net.Dns.GetHostEntry(Request.UserHostName).HostName +
      <span class="literal">"\","</span>);
    sw.Write(<span class="literal">"\""</span> + Request.UserAgent + <span class="literal">"\","</span>);
    sw.Write(<span class="literal">"\""</span> + Request.Url + <span class="literal">"\","</span>);
    sw.Write(<span class="literal">"\""</span> + Request.UrlReferrer + <span class="literal">"\"\n"</span>);
  }
  Application.UnLock();
}

<span class="reserved">protected void</span> Session_Start(Object sender, EventArgs e)
{
  <span class="reserved">if</span> (CheckExcludeList())
    <span class="reserved">return</span>;

  <span class="reserved">string</span> count = GetTotalCount();
  Session["count"] = count;
  AddAccessLog(count);
}
</code></pre>



## <a id="sec-generated-title-4"></a> <a id="display"></a>カウント数の表示（Web フォーム中）

Global.asax で Session 状態に記憶したカウント数を表示したいわけですが、
Web フォーム（.aspx）ページ中に表示するのは非常に簡単です。

1番簡単な方法でいうと、カウント数を表示したい位置に以下の1行を書くだけ。


<pre class="xsource" title="カウント数の表示">
<code>&lt;%= Session["count"] %&gt;
</code></pre>
例えば、以下のような感じ。


<pre class="xsource" title="Default.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">html</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>テストページ<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    総アクセス数: <span class="bracket">&lt;%</span>= Session["count"] <span class="bracket">%&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>

## <a id="sec-generated-title-5"></a> <a id="image"></a>画像カウンタ

普通の html ページ上にカウンタを設置しすることを考えて、
CGI などでよくやるような、画像カウンタを作ることもできます。

ASP.NET の Web フォームの出力を画像にしたければ、
Response.ClearContent や Response.ContentType などを使って、
Page_Load イベントハンドラに以下のようなコードを書きます。
（画像の作り方に関してはほんの一例。
要点は強調表示している4行。）

<pre class="source" title="画像カウンタ" lang="">
<code><span class="reserved">private void</span> Page_Load(<span class="reserved">object</span> sender, System.EventArgs e)
{
  <span class="reserved">string</span> text = (<span class="reserved">string</span>)Session[<span class="literal">"TotalCount"</span>];
  text = text.PadLeft(7, <span class="literal">'0'</span>);
  Font font = <span class="reserved">new</span> Font(<span class="literal">"ＭＳ ゴシック"</span>, 15);

  Bitmap bitmap = <span class="reserved">new</span> Bitmap(75, 20);
  Graphics graphics = Graphics.FromImage(bitmap); 

  graphics.FillRectangle(Brushes.White, 0, 0, 75 , 20);
  graphics.DrawString(text, font, Brushes.Black,0,2);

<em>  Response.ClearContent();
  Response.ContentType = <span class="literal">"image/gif"</span>;
  bitmap.Save(Response.OutputStream, ImageFormat.Gif);
  Response.End();</em>

  graphics.Dispose();
  bitmap.Dispose(); 
}
</code></pre>


.aspx ファイル中に何を書いていようと、
Response.ClearContent() メソッドを呼び出すことで、
一度出力結果がまっさらになるので、
.aspx には &lt;script&gt; タグだけを書くか、
何も書かず &lt;%@ Page %&gt; ディレクティブでコードビハインドの設定だけを書きます。

例えば以下のような感じ。
（ここでは、これのファイル名は Counter.aspx としておきます。）


<pre class="xsource" title="Counter.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">language</span><span class="attvalue">="c#"</span> <span class="bracket">%&gt;</span>
<span class="bracket">&lt;%@ </span><span class="element">import</span> <span class="attribute">Namespace</span><span class="attvalue">="System.Drawing.Imaging"</span> <span class="bracket">%&gt;</span>
<span class="bracket">&lt;%@ </span><span class="element">import</span> <span class="attribute">Namespace</span><span class="attvalue">="System.Drawing"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">script</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

private void Page_Load(object sender, System.EventArgs e)
{
  string text = (string)Session["count"];
  text = text.PadLeft(7, '0');
  Font font = new Font("ＭＳ ゴシック", 15);

  Bitmap bitmap = new Bitmap(75, 20);
  Graphics graphics = Graphics.FromImage(bitmap); 

  graphics.FillRectangle(Brushes.White, 0, 0, 75 , 20);
  graphics.DrawString(text, font, Brushes.Black,0,2);

  Response.ClearContent();
  Response.ContentType = "image/gif";
  bitmap.Save(Response.OutputStream, ImageFormat.Gif);
  Response.End();

  graphics.Dispose();
  bitmap.Dispose(); 
}

<span class="bracket">&lt;/</span><span class="element">script</span><span class="bracket">&gt;</span>
</code></pre>
この画像カウンタを呼び出す HTML 側には以下のような感じで &lt;img&gt; タグを書きます。


<pre class="xsource" title="カウンタ利用側の HTML">
<code><span class="bracket">&lt;</span><span class="element">html</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>テストページ<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    総アクセス数: 
    <span class="bracket">&lt;</span><span class="element">img</span> <span class="attribute">src</span><span class="attvalue">="Counter.aspx"</span> <span class="attribute">width</span><span class="attvalue">="75"</span> <span class="attribute">height</span><span class="attvalue">="20"</span><span class="bracket">/&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>

### <a id="sec-generated-title-6"></a> <a id="js"></a>おまけ（JavaScript 版）

同じ理屈で、JavaScript カウンタにしたりもできます。


<pre class="xsource" title="JsCounter.aspx">
<code><span class="bracket">&lt;%@ </span><span class="element">Page</span> <span class="attribute">language</span><span class="attvalue">="c#"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">script</span> <span class="attribute">runat</span><span class="attvalue">="server"</span><span class="bracket">&gt;</span>

private void Page_Load(object sender, System.EventArgs e)
{
  string text = (string)Session["count"];
  Response.ClearContent();
  Response.ContentType = "text/javascript";
  Response.Output.Write("document.write(" + text + ");");
}

<span class="bracket">&lt;/</span><span class="element">script</span><span class="bracket">&gt;</span>
</code></pre>
これを JsCounter.aspx という名前で保存したとすると、
呼び出し側の HTML では以下のような書きます。


<pre class="xsource" title="JavaScript カウンタ利用側の HTML">
<code><span class="bracket">&lt;</span><span class="element">html</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">head</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">title</span><span class="bracket">&gt;</span>テストページ<span class="bracket">&lt;/</span><span class="element">title</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">head</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">body</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">p</span><span class="bracket">&gt;</span>
    総アクセス数: 
    <span class="bracket">&lt;</span><span class="element">script</span> <span class="attribute">type</span><span class="attvalue">="text/javascript"</span> <span class="attribute">src</span><span class="attvalue">="JsCounter.aspx"</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">script</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">p</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">body</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">html</span><span class="bracket">&gt;</span>
</code></pre>
