---
title: "ASP.NET 概要"
source_url: "https://ufcpp.net/study/dotnet/aspx/aspx_abstract/"
content_type: "Article"
published_at: "2006-06-28T00:00:00"
updated_at: "2015-05-06T14:15:03"
tags: []
umbraco_id: 1415
parent_id: 1414
sort_order: 0
aliases:
  - "/study/aspx/abstract.html"
---

# ASP.NET 概要

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="aspdotnet" class="keyword">ASP.NET</strong> は、Microsoft が開発したサーバサイトアプリケーションフレームワークです。

動的な処理はウェブサーバ上で行い、
クライアントには（普通の HTML などの）結果のみが表示されます。

ASP.NET のフレームワークを用いることで、
（人が直接操作することを前提に）結果を HTML で表示するウェブフォームや、
（プログラムから呼び出して使うことが前提の）SOAP と呼ばれる通信プロトコルでデータを返す XML ウェブサービスを簡単に作ることができます。

ここでは、ウェブフォームを中心に説明していきます。


## <a id="sec-generated-title-2"></a> <a id="sample"></a>例

ASP.NET のウェブフォームでは、HTML （ただし、拡張子は .html ではなく、.aspx にする）の中にプログラムを埋め込んだり、
「HTML ＋ プログラムソースファイル」で開発ができます。

例として、現在時刻を表示するだけの単純な ASP.NET ウェブフォームを示します。
以下のような内容を、拡張子 .aspx を付けて保存するだけで OK です。


```html {title="now.aspx"}
<%@ Page Language="C#" %>

<html lang="ja">
<body>
<p style="color:#0000ff;">

<%= DateTime.Now.ToString() %>

</p>
</body>
</html>
```
こうすると、
&lt;% %&gt; という特殊なタグでくくられた部分がサーバ上で解釈されて、
結果を該当箇所に埋め込んだ HTML が出力されます。

特にコンパイル作業なども必要なく、
この .aspx ファイルをサーバ上に置くだけで OK。
（サーバ側で拡張子 .aspx を ASP.NET アプリケーションとして認識する設定は必要。
「ASP.NET を使える」という触れ込みのサーバなら最初から設定済みのはず。）
ページが始めて表示される際に、
自動的にコンパイルされて、
以後は .aspx ファイルや関連するファイルの中身が修正されるまでの間、
ページを表示するたびにコンパイル済みのバイナリが呼び出されます。

ちなみに、
この &lt;% %&gt; でコードを囲って書く方式は、
（ビジュアルとロジックが混ざるのがあまりよくないと言われていて）
非推奨とされています。
代わりに、以下のように、&lt;script&gt; タグを使ってロジックを分離します。


```html {title="now.aspx その2"}
<%@ Page Language="C#" %>

<script runat="server" type="text/C#">
  void Page_Load(object sender, EventArgs e)
  {
    this.label1.Text = DateTime.Now.ToString();
  }
</script>

<html>
<body>
<form id="form1" runat="server">

<p>
<asp:Label runat="server" ID="label1" />
</p>

</form>
</body>
</html>
```
Page_Load というのは、このページが表示される際に呼び出される特殊な関数です。
属性に runat="server" と書いたタグの箇所には、サーバ側での処理の結果が表示されます。

先ほどの &lt;% %&gt; を使ったものや、
この .aspx ファイル中に直接 &lt;script&gt; タグを書くような方式は、
インラインコード（inline code）と呼ばれています。
これに対して、ASP.NET では、
.aspx ファイルとプログラムコードを完全に分離して書くことができます。
例えば、以下の通り。

```html {title="now.aspx.cs"}
using System;

namespace WebApplication1
{
  public partial class _Default : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      this.label1.Text = DateTime.Now.ToString();
    }
  }
}
```



```html {title="now.aspx コードビハインド版"}
<%@ Page Language="C#"
  CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>

<html>
<body>
<form id="form1" runat="server">

<p>
<asp:Label runat="server" ID="label1" />
</p>

</form>
</body>
</html>
```
このような方式を<strong id="codebehind" class="keyword">コードビハインド</strong>（code-behind: ニュアンスとしては code behind the page、aspx ページの後ろに隠れたコード。「分離コード」と訳すことも。）といいます。
こうすることで、
完全にビジュアル（見た目・視覚的なデザイン）とロジック（処理内容・論理的なデザイン）を分離できます。
ビジュアルとロジックの分離によって、
ビジュアルデザイナーとプログラマーの分業がしやすくなるなどのメリットがあって、
ASP.NET では、このコードビハインド方式での開発が推奨されています。

ただし、この場合、コードの方はコンパイル作業が必要です。
（Visual Studio などを使えばコンパイルも簡単。）

ちなみに、
コードビハインドを使ってビジュアルとロジックを分離するという思想は、
「[Windows Presentation Foundation](../wpf/wpf_abst.md#wpf)」にも引き継がれています。


## <a id="sec-generated-title-3"></a> <a id="devenv"></a>開発環境

ASP.NET は、ウェブフォームを作るにしろウェブサービスを作るにしろ、
全てテキスト形式のソースファイルで書くことができます。
なので、やろうと思えば、テキストエディタ一つあれば開発ができます。
（実際、前節の例はテキストエディタで書いてる。）

まあ、でも、普通は統合開発環境を使って開発します。
[Visual Studio](http://www.microsoft.com/japan/msdn/vstudio/) や、
その入門者向け無料サブセット版の[Visual Web Developer 2005 Express Edition](http://www.microsoft.com/japan/msdn/vstudio/express/vwd/) を使うのが一般的です。

<iframe style="width:120px;height:240px;" scrolling="no" marginwidth="0" marginheight="0" frameborder="0" src="http://rcm-jp.amazon.co.jp/e/cm?t=cunflc-22&amp;o=9&amp;p=8&amp;l=as1&amp;asins=B000CSRIYG&amp;fc1=000000&amp;IS2=1&amp;lt1=_blank&amp;lc1=0000ff&amp;bc1=000000&amp;bg1=ffffff&amp;f=ifr">
asin 番号: B000CSRIYG</iframe>


## <a id="sec-generated-title-4"></a> <a id="server"></a>ウェブサーバ

ウェブアプリケーションを使うためには、当然、ウェブサーバが必要になります。
「自宅でサーバを稼動させて・・・」なんてやる人はそう多くないので、
レンタルサーバを少し紹介。

というか、Microsoft のサイト中に [ASP.NET ホスティングサービス情報](http://www.microsoft.com/japan/msdn/asp.net/hosting/)があったりします。
[無料で利用できるもの](http://www.fsdotnet.jp/index.shtml)とか、
[ドメイン取得費無料・月額525円](http://www.activeweb.jp/)とかいう安いのもあるので、
気軽に試してみたい方はこのあたりを。


## <a id="sec-generated-title-5"></a> <a id="link"></a>リンク

本格的に ASP.NET を勉強してみるつもりなら、
以下の辺りを。
<!-- 以下、reflinks 要素の変換後の出力です。参照先に同じリンクが記述されている可能性があります。-->

[ASP.NET Developer Center](http://www.microsoft.com/japan/msdn/asp.net/)
: 日本語公式サイト。

[ASP.NET クイック スタート チュートリアル](http://ja.gotdotnet.com/quickstart/aspplus/)
: Microsoft が提供するチュートリアル。 Microsoft はドキュメントの多言語対応に結構力をいれているので、 日本語的にも割りと安心して読めると思います。

[＠IT 連載　 プログラミングASP.NET](http://www.atmarkit.co.jp/fdotnet/aspnet/index/index.html)
: こちらは＠ITの記事。
