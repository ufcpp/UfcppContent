---
title: "アクセスログを取ろう"
source_url: "https://ufcpp.net/study/dotnet/aspx/logging/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-18T08:58:44"
tags: []
umbraco_id: 1417
parent_id: 1414
sort_order: 2
aliases:
  - "/aspx/logging"
  - "/aspx/logging.html"
  - "/dotnet/aspx/logging/"
  - "/study/aspx/logging"
  - "/study/aspx/logging.html"
---

# アクセスログを取ろう

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
まず、
アクセスログの記録を例に、
以下の2点について説明します。

1. （ページ単位ではなく）Web アプリケーション全体を対象とした処理

2. サーバ上のファイル操作



##<a id="sec-generated-title-2"></a> <a id="global_asax"></a>Global.asax
Web アプリケーションとして構築した（ASP.NET などを実行する設定で作った）仮想ディレクトリの直下に、
Global.asax という名前のファイルを置くことで、
Web アプリケーション全体に対する処理を記述することができます。

ページが読み込まれた（Page_Load）とか描画される直前（Page_PreRender）とかのイベントは Web フォーム（.aspx ファイル）単位で記述しますが、
HTTP リクエストを処理し始める直前（Application_BeginRequest）とか認証完了（Application_AuthorizeRequest）などのイベント処理は Global.asax 中に記述します。
また、セッションの開始（Session_Start）、終了（Session_End）なども Global.asax 中で処理します。

Global.asax は、
以下のように &lt;script&gt; タグを1つだけ書いてその中で処理に記述するか、


<pre class="xsource" title="Global.asax（インラインコード）">
<code><span class="bracket">&lt;%@ </span><span class="element">Application</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="bracket">%&gt;</span>

<span class="bracket">&lt;</span><span class="element">script</span> <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="attribute">runat</span><span class="attvalue">="Server"</span><span class="bracket">&gt;</span>
void Application_OnBeginRequest(object sender, EventArgs e)
{
  // HTTP リクエスト処理開始時の処理
}

void Session_Start(object sender, EventArgs e)
{
  // セッション開始時の処理
}
<span class="bracket">&lt;/</span><span class="element">script</span><span class="bracket">&gt;</span>
</code></pre>
あるいは、以下のように、コードビハインドで処理を記述します。


<pre class="xsource" title="Global.asax（コードビハインド）">
<code><span class="bracket">&lt;%@ </span><span class="element">Application</span>
    <span class="attribute">Codebehind</span><span class="attvalue">="Global.asax.cs"</span>
    <span class="attribute">Inherits</span><span class="attvalue">="WebApplication1.Global"</span>
    <span class="attribute">Language</span><span class="attvalue">="C#"</span> <span class="bracket">%&gt;</span>
</code></pre>
<pre class="source" title="Glocal.asax.cs" lang="">
<code><span class="reserved">namespace</span> WebApplication1
{
  <span class="reserved">public class</span> Global : System.Web.HttpApplication
  {
    <span class="reserved">protected void</span> Application_OnBeginRequest(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="comment">// HTTP リクエスト処理開始時の処理</span>
    }

    <span class="reserved">protected void</span> Session_Start(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="comment">// セッション開始時の処理</span>
    }
  }
}
</code></pre>


BeginReqest の他に、ASP.NET Web アプリケーションにどのようなイベントがあるかは、
MSDN ライブラリ中の[ASP.NET アプリケーションのライフ サイクルの概要](http://msdn2.microsoft.com/ja-jp/library/ms178473(VS.80).aspx)などを参照。
セッションに関しては、同サイト[セッション状態イベント](http://msdn2.microsoft.com/ja-jp/library/ms178583(VS.80).aspx)参照。


##### <a id="sec-generated-title-3"></a>ログを記録するタイミング
アクセスログを取るという用途を考えると、
Application_BeginRequest か Session_Start 内にログ記録コードを書くのがいいと思います。

Application_BeginRequest で処理すれば、
ASP.NET を介してページが表示される（.aspx ページなどが開かれる）たびにログを記録します。

一方、Session_Start で処理すれれば、
セッションごとにログを記録します。
（同一ホスト・同一ブラウザからの連続したアクセスは1度だけ記録。
一定時間（デフォルトでは20分。Web.config で設定変更可能）アクセスがないと、
セッションがタイムアウトして、次のアクセスでは再びログが記録される。）


##### <a id="sec-generated-title-4"></a>Global.asax によるログ記録の対象
Global.asax には Web アプリケーション全体に対する処理を記述するといっても、
Global.asax 中に記述したコードが呼ばれるのは、
あくまで ASP.NET エンジンを介して表示されるページだけです。
すなわち、Web フォーム（.aspx）や Web サービス（.asmx）はログ記録の対象になりますが、
普通の HTML ファイルや画像などは記録対象外になります。

画像なども含めてなんでも Global.asax によるログ記録の対象にしたければ、
サーバ上の設定で、
あらゆるファイルを一度 ASP.NET エンジンを通してからクライアントに渡すようにするなどの設定変更が必要です。

HTML に関しては、
CGI でよく行われるような、
画像カウンタを介したログ記録方法を取るという手もあります。
（HTML 中の &lt;img&gt; タグから ASP.NET で記述された画像カウンタを呼びだすとか）。


##<a id="sec-generated-title-5"></a> <a id="logging"></a>ログの記録
ログ記録のタイミングを決めたところで、
実際のログ記録の説明に入ります。

ログは、いまどきの流行で言うとデータベースサーバにでも記録するのがいいんだと思うんですが、
ここでは、とりあえずサーバ上のローカルファイルに記録する方式で説明します。
（
個人サイトごときのアクセスログにデータベースを使うのも仰々しいですし。
私的には、
データベース使って遊んでみるのは C# 3.0 の 「[LINQ](../../csharp/cheatsheet/ap_ver3.md#linq)」 待ちだと思ってる。
）

で、サーバ上のファイルを読み書きするときに気をつける点は2点。

* サーバ上のファイルパスの取得

* 排他制御


この2つだけ把握すれば、あとは .NET Framework の System.IO の機能を使うだけなので、
普通の Windows アプリケーションと全く同じ感覚でプログラミングできます。
（参考：「[ファイル操作](../../csharp/lib/lib_file.md)」。）


##### <a id="sec-generated-title-6"></a>サーバ上のファイルパスの取得
Web アプリケーションの物理パスは
Request.PhysicalApplicationPath で取得できます。
（Request は、System.Web.HttpApplication とか System.Web.UI.Page クラスのプロパティ。
System.Web.HttpRequest 型。）

例えば、仮想ディレクトリの物理パスを、
サーバ上の C:\Users\user1\wwwroot に設定したとすると、
Request.PhysicalApplicationPath プロパティの値は C:\Users\user1\wwwroot になります。

このとき、アクセスログを、例えば C:\Users\user1\wwwroot\accesslog\ 以下に保存したければ、
以下のようにします。
（この例では、日付に応じて、yyyyMM.csv という形式の名前で保存します。）

<pre class="source" title="PhysicalApplicationPath" lang="">
<code><span class="reserved">string</span> basePath = Request.PhysicalApplicationPath + <span class="literal">@"\accesslog\"</span>;
DateTime now = DateTime.Now;
<span class="reserved">string</span> filename = basePath +
  <span class="reserved">string</span>.Format(<span class="literal">"{0}{1:00}.csv"</span>, now.Year, now.Month);
</code></pre>



##### <a id="sec-generated-title-7"></a>排他制御
Web アプリケーションにいつ誰がアクセスしてくるかは分かりません。
当然、複数の人が同時にアクセスしてくることもありえます。
このような状況下では、ファイルの読み書きに排他制御を掛ける必要があります。

例えば、単に以下のようなコードを書いたとします。

<pre class="source" title="ログ記録（排他制御なし）" lang="">
<code><span class="reserved">using</span> (StreamWriter sw = <span class="reserved">new</span> StreamWriter(filename, <span class="reserved">true</span>))
{
  sw.Write(<span class="literal">"\""</span> + DateTime.Now.ToString() + <span class="literal">"\","</span>);
  sw.Write(<span class="literal">"\""</span> +
    System.Net.Dns.GetHostEntry(Request.UserHostName).HostName +
    <span class="literal">"\","</span>);
  sw.Write(<span class="literal">"\""</span> + Request.UserAgent + <span class="literal">"\","</span>);
  sw.Write(<span class="literal">"\""</span> + Request.Url + <span class="literal">"\","</span>);
  sw.Write(<span class="literal">"\""</span> + Request.UrlReferrer + <span class="literal">"\"\n"</span>);
}
</code></pre>


排他制御も何もしていないので、
このままだと、ごく希にですが、
「他のプロセスで使用中のためファイルを開くことができません」
というような内容の例外が発生して、
ページにエラーメッセージが表示されることがあります。

あるいは、ファイルを開くときに FileShare.Write を指定して開くなら、
時々ログファイルが変になる可能性があります。
例えば、以下のように記録されて欲しいときに、

<blockquote markdown="1">
"2007/06/30 16:01:36", "xxx.aaa.ne.jp", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506)", "http://my.domain.jp/user1/Default.aspx", ""<br></br>
"2007/06/30 16:12:09", "xxx.aaa.ne.jp", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50215)", "http://my.domain.jp/user1/Default.aspx". ""

</blockquote>
以下のように、2行分のログが変に混ざったりするかもしれません。

<blockquote markdown="1">
"2007/06/30 16:01:36", "xxx.aaa.ne.jp", "2007/06/30 16:12:09", "xxx.aaa.ne.jp", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50215)", "http://my.domain.jp/user1/Default.aspx". ""<br></br>"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0; SLCC1; .NET CLR 2.0.50727; .NET CLR 3.0.04506)", "http://my.domain.jp/user1/Default.aspx", ""

</blockquote>
そこで、排他制御が必要になります。
ファイルに対する排他制御は、
System.IO.FileStream.Lock / UnLock でやればよさそうなんですが、
ここでは、せっかく ASP.NET を使っているんだから ASP.NET の機能を使ってみます。

Application.Lock() / UnLock() を使って排他制御ができます。
（Appllcation は HttpApplication や Page クラスのプロパティで、
System.Web.HttpApplicationState 型。）
Lock() と UnLock() で囲まれた領域は、
複数のプロセスから同時には実行されなくなります。
（ロックとかは、必要最小限な分にだけ掛けるべきで、
そういう観点からすると、Application でロックするのは作法的にはあまりよくないかも。
でも、Application.Lock() は使いやすいから。）

<pre class="source" title="ログ記録（排他制御なし）" lang="">
<code>Application.Lock();
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
</code></pre>



##### <a id="sec-generated-title-8"></a>サンプル
まとめると、
Global.asax.cs（Global.asax のコードビハインド）に以下のようなコードを書くことでアクセスログの記録ができます。

<pre class="source" title="Global.asax.cs" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Web;
<span class="reserved">using</span> System.IO;

<span class="reserved">namespace</span> WebApplication1
{
  <span class="reserved">public class</span> Global : System.Web.HttpApplication
  {
    <span class="reserved">void</span> AddAccessLog()
    {
      <span class="reserved">string</span> basePath = Request.PhysicalApplicationPath + <span class="literal">@"\accesslog\"</span>;
      DateTime now = DateTime.Now;
      <span class="reserved">string</span> filename = basePath
        + <span class="reserved">string</span>.Format(<span class="literal">"{0}{1:00}.csv"</span>, now.Year, now.Month);

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

    <span class="reserved">protected void</span> Application_OnBeginRequest(<span class="reserved">object</span> sender, EventArgs e)
    {
      <span class="comment">// ↓もし、毎リクエストでログをとりたければ
      // AddAccessLog();</span>
    }

    <span class="reserved">protected void</span> Session_Start(<span class="reserved">object</span> sender, EventArgs e)
    {
      AddAccessLog();
    }
  }
}
</code></pre>



##<a id="sec-generated-title-9"></a> <a id="global_asax"></a>検索エンジンのクローラの除外
前節の状態だとおそらく、検索エンジンのクローラロボットでアクセスログが埋まります。
なので、クローラだった場合にはログを記録しない仕組みが必要になります。

まあ、単純に、ユーザエージェントを見て、それっぽい文字が含まれていたらログを記録しないだけなので、
以下のようなコードを Global.asax.cs に追加するだけで簡単に実現可能です。

<pre class="source" title="Global.asax.cs に追加" lang="">
<code><span class="reserved">static readonly string</span>[] excludeList = <span class="reserved">new string</span>[]
{
  <span class="literal">"Googlebot"</span>,
  <span class="literal">"Yahoo! Slurp"</span>,
  <span class="literal">"msnbot"</span>,
  <span class="literal">"MMCrawler"</span>,
  <span class="literal">"yetibot@naver.com"</span>,
  <span class="literal">"AMZNKAssocBot"</span>,
  <span class="literal">"Mediapartners-Google"</span>,
};

<span class="reserved">bool</span> CheckExcludeList()
{
  <span class="reserved">string</span> agent = Request.UserAgent;

  <span class="reserved">foreach</span> (<span class="reserved">string</span> exclude <span class="reserved">in</span> excludeList)
  {
    <span class="reserved">if</span> (agent.Contains(exclude))
      <span class="reserved">return true</span>;
  }
  <span class="reserved">return false</span>;
}

<span class="reserved">protected void</span> Session_Start(Object sender, EventArgs e)
{
  <span class="reserved">if</span> (CheckExcludeList())
    <span class="reserved">return</span>;

  AddAccessLog();
}
</code></pre>


（汎用性を持たせたければ、
除外リストはソースファイル中に埋め込むんじゃなくて、
外部設定ファイルに書くようにすべきですけど。）
