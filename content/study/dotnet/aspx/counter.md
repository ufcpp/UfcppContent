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

```csharp
string GetTotalCount()
{
  string filename = Request.PhysicalApplicationPath +
    @"\count\total.txt";

  Application.Lock();

  int num;
  using (FileStream fs = new FileStream(
    filename, FileMode.OpenOrCreate,
    FileAccess.ReadWrite, FileShare.None))
  {
    StreamReader sr = new StreamReader(fs);
    string line = sr.ReadLine();

    if (string.IsNullOrEmpty(line) || !int.TryParse(line, out num))
    {
      num = 0;
    }

    line = (num + 1).ToString();

    fs.Seek(0, SeekOrigin.Begin);

    StreamWriter sw = new StreamWriter(fs);
    sw.WriteLine(line);
    sw.Flush();
  }
  Application.UnLock();

  return num.ToString();
}

protected void Session_Start(Object sender, EventArgs e)
{
  if (CheckExcludeList())
    return;

  AddAccessLog();

  string count = GetTotalCount();
  Session["TotalCount"] = count;
}
```


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

```csharp
void AddAccessLog(string count)
{
      string basePath = Request.PhysicalApplicationPath + @"\accesslog\";
      DateTime now = DateTime.Now;
      string filename = basePath
        + string.Format("{0}{1:00}.csv", now.Year, now.Month);

  string prev;
  if (Request.Cookies["PREV"] != null)
    prev = Request.Cookies["PREV"].Value;
  else
    prev = count;

  Response.Cookies["PREV"].Value = count;

  Application.Lock();

  using (StreamWriter sw = new StreamWriter(filename, true))
  {
    sw.Write("\"" + DateTime.Now.ToString() + "\",");
    sw.Write("\"" +
      System.Net.Dns.GetHostEntry(Request.UserHostName).HostName +
      "\",");
    sw.Write("\"" + Request.UserAgent + "\",");
    sw.Write("\"" + Request.Url + "\",");
    sw.Write("\"" + Request.UrlReferrer + "\"\n");
  }
  Application.UnLock();
}

protected void Session_Start(Object sender, EventArgs e)
{
  if (CheckExcludeList())
    return;

  string count = GetTotalCount();
  Session["count"] = count;
  AddAccessLog(count);
}
```



## <a id="sec-generated-title-4"></a> <a id="display"></a>カウント数の表示（Web フォーム中）

Global.asax で Session 状態に記憶したカウント数を表示したいわけですが、
Web フォーム（.aspx）ページ中に表示するのは非常に簡単です。

1番簡単な方法でいうと、カウント数を表示したい位置に以下の1行を書くだけ。


```xml
<%= Session["count"] %>
```
例えば、以下のような感じ。


```html
<%@ Page Language="C#" %>

<html>
<head>
  <title>テストページ</title>
</head>
<body>
  <p>
    総アクセス数: <%= Session["count"] %>
  </p>
</body>
</html>
```

## <a id="sec-generated-title-5"></a> <a id="image"></a>画像カウンタ

普通の html ページ上にカウンタを設置しすることを考えて、
CGI などでよくやるような、画像カウンタを作ることもできます。

ASP.NET の Web フォームの出力を画像にしたければ、
Response.ClearContent や Response.ContentType などを使って、
Page_Load イベントハンドラに以下のようなコードを書きます。
（画像の作り方に関してはほんの一例。
要点は強調表示している4行。）

```csharp
private void Page_Load(object sender, System.EventArgs e)
{
  string text = (string)Session["TotalCount"];
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
```


.aspx ファイル中に何を書いていようと、
Response.ClearContent() メソッドを呼び出すことで、
一度出力結果がまっさらになるので、
.aspx には &lt;script&gt; タグだけを書くか、
何も書かず &lt;%@ Page %&gt; ディレクティブでコードビハインドの設定だけを書きます。

例えば以下のような感じ。
（ここでは、これのファイル名は Counter.aspx としておきます。）


```html
<%@ Page language="c#" %>
<%@ import Namespace="System.Drawing.Imaging" %>
<%@ import Namespace="System.Drawing" %>

<script runat="server">

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

</script>
```
この画像カウンタを呼び出す HTML 側には以下のような感じで &lt;img&gt; タグを書きます。


```html
<html>
<head>
  <title>テストページ</title>
</head>
<body>
  <p>
    総アクセス数: 
    <img src="Counter.aspx" width="75" height="20"/>
  </p>
</body>
</html>
```

### <a id="sec-generated-title-6"></a> <a id="js"></a>おまけ（JavaScript 版）

同じ理屈で、JavaScript カウンタにしたりもできます。


```html
<%@ Page language="c#" %>

<script runat="server">

private void Page_Load(object sender, System.EventArgs e)
{
  string text = (string)Session["count"];
  Response.ClearContent();
  Response.ContentType = "text/javascript";
  Response.Output.Write("document.write(" + text + ");");
}

</script>
```
これを JsCounter.aspx という名前で保存したとすると、
呼び出し側の HTML では以下のような書きます。


```html
<html>
<head>
  <title>テストページ</title>
</head>
<body>
  <p>
    総アクセス数: 
    <script type="text/javascript" src="JsCounter.aspx"></script>
  </p>
</body>
</html>
```
