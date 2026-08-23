---
title: "C#のプログラムの基本構造"
source_url: "https://ufcpp.net/study/csharp/start/st_basis/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2021-09-05T00:00:00"
tags: []
umbraco_id: 1191
parent_id: 1190
sort_order: 0
aliases:
  - "/study/csharp/st_basis.html"
---

# C#のプログラムの基本構造

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

##### <a id="sec-generated-title-2"></a>ポイント

* C# プログラムは Main から始まります。

* 「[クラス](../oop/oo_class.md#class)」とか「[名前空間](../structured/sp_namespace.md#namespace)」とかは、今のところおまじない（後々説明）。



## <a id="sec-generated-title-3"></a> <a id="sample"></a>C#の簡単なプログラム例

まずは C# を用いて書かれた簡単なプログラムを見てみましょう。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

```csharp {title="最も簡単なC#プログラム" highlight-lines="5-9"}
using System;
 
class Program
{
    static void Main(string[] args)
    {
        // 初めてC#を学ぶ方々にご挨拶
        Console.WriteLine("皆様、はじめまして");
    }
}
```


</div>
<div>

```vbnet
Module Program
 
    Sub Main()
        ' 初めてVisual Basicを学ぶ方々にご挨拶
        Console.WriteLine(("皆様、はじめまして")
    End Sub
 
End Module
```


</div>
<div>

```fsharp
// 初めてF#を学ぶ方々にご挨拶
open System
Console.Write "皆様、はじめまして"
```


</div>
<div>

```cpp
#include "stdafx.h"
 
using namespace System;
 
int main(array<System::String ^> ^args)
{
  // 初めてC++/CLIを学ぶ方々にご挨拶
    Console::WriteLine(L"皆様、はじめまして");
    return 0;
}
```


</div>
</div>


これからしばらくの間は <code>using</code> とか <code>class</code> という部分のことは忘れて、
背景色を変えて強調してある部分だけを注目してください。

<em>
        C#のプログラムは、すべてこの<code>Main</code>と書いてある部分から始まります
      </em>。
このプログラムは、画面(DOSプロンプト中)に“皆様、始めまして。”という文字を表示します。
<em>
        <code>Console.Write</code>は文字や数値を画面に出力するためのもの
      </em>で、詳しくは「[ライブラリ](../structured/st_library.md)」で説明します。
また、<em>
        <code>//</code>から始まる行はコメント
      </em>で、プログラムの動作とは関係ありません。詳しくは「[コメント](st_comment.md)」で説明します。

ちなみに、
<code>using</code>は「[名前空間](../structured/sp_namespace.md)」で、
<code>class</code>は「[クラス](../oop/oo_class.md)」で、
<code>public</code>は「[実装の隠蔽](../oop/oo_conceal.md)」で、
<code>static</code>は「[静的メンバー](../oop/oo_static.md)」で、
<code>void</code>は「[関数](../structured/st_function.md)」で説明していきます。

### <a id="sec-generated-title-4"></a> <a id="top-level-statements"></a>C# 9.0 から

<h5 class="version version9">Ver. 9.0</h5>

C# 9.0 からは、上記のコードを以下のように書くことができます。

```csharp
using System;
// 初めてC#を学ぶ方々にご挨拶
Console.WriteLine("皆様、はじめまして");
```

`namespace` とか `class` とかを飛ばして、書きたい処理を直接ファイル直下に書くことができるようになりました。
詳しくは「[トップ レベル ステートメント](../misc/miscentrypoint.md#top-level-statements)」で説明します。

### <a id="sec-generated-title-5"></a> <a id="global-using"></a>C# 10.0 から

<h5 class="version version10">Ver. 10.0</h5>

C# 10.0 からは、さらに、以下のように縮めて書くことができます。

```csharp
// 初めてC#を学ぶ方々にご挨拶
Console.WriteLine("皆様、はじめまして");
```

`using` も消えました。
詳しくは「[global using](../structured/sp_namespace.md#global-using)」で説明します。

## <a id="sec-generated-title-6"></a> <a id="gui"></a>GUIプログラム例

C# では GUI (Graphical User Interface: 要するに、Windowsなどのようにボタンやメニューなどをマウスで操作するようなもの)プログラミングも行えます。

GUI プログラムは文字ベース(CUI: Character User Interfaceという)のプログラムに比べて煩雑な処理が多く、難しいので、ここでは例を挙げるにとどめます。

<h5 class="version version3">Ver. 3.0</h5>
ちなみに、この例は、.NET Framework 3.0、C# 3.0 以降で動きます。
詳しくは、「[Windows Presentation Foundation](../../dotnet/index.md#wpf)」で説明します。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
</ul>
<div>

```csharp {title="GUI プログラム例（WPF）"}
using System;
using System.Windows;
using System.Windows.Controls;

public class Program
{
    [STAThread]
    static void Main()
    {
        var button = new Button { Content = "ここを押せ" };
        button.Click += (sender, e) => MessageBox.Show("ようこそ");

        var win = new Window
        {
            Title = "サンプルプログラム",
            Width = 300,
            Height = 200,
            Content = button,
        };

        var app = new Application();
        app.Run(win);
    }
}
```


</div>
<div>

```vbnet
Module VBSample

    Sub Main()
        Dim button = New Button With {.Content = "ここを押せ"}
        AddHandler button.Click, Function(sender, args) {MessageBox.Show("ようこそ")}

        Dim win = New Window With
                  {
                      .Title = "サンプルプログラム",
                      .Width = 300,
                      .Height = 200,
                      .Content = button
                  }

        Dim app = New Application()
        app.Run(win)
    End Sub

End Module
```


</div>
<div>

```fsharp
open System
open System.Windows
open System.Windows.Controls
 
let button = new Button(Content = "ここを押せ")
button.Click.Add(fun x -> MessageBox.Show("ようこそ") |> ignore)

let win = new Window(
                     Title = "サンプルプログラム",
                     Width = 300.0,
                     Height = 200.0,
                     Content = button)

[<STAThread>]
do
    let app = new Application()
    app.Run(win) |> ignore
```


</div>
</div>


<figure>

[![C# 3.0 WPF によるGUIプログラムの例](../../../../assets/media/ufcpp2000/csharp/fig/wpfwelcome.png)](../../../../assets/media/ufcpp2000/csharp/fig/wpfwelcome.png)

<figcaption>C# 3.0 WPF によるGUIプログラムの例</figcaption>
</figure>


<span class="expand-button" title="展開/折畳">（古いコード（Windows Forms））</span>
<div class="expand-panel" markdown="1" title="（古いコード（Windows Forms））">
      
.NET Framework 2.0 時代の古いコードも残しておきます。
詳しくは、「[GUI アプリケーション](../lib/lib_forms.md)」で説明します。

      
```csharp {title="C#によるGUIプログラムの例"}
namespace CsharpSample
{
  using System;
  using System.Windows.Forms;
  using System.Drawing;

  /// <summary>
  /// ボタンが1つ付いたウィンドウを作成し、
  /// ボタンを押したときに「ようこそ。」と書かれたメッセージボックスを表示
  /// </summary>
  class WelcomeForm : Form
  {
    Button button;

    WelcomeForm()
    {
      // ウィンドウ内にボタンをひとつ作成
      this.Text       = "サンプルプログラム";
      this.ClientSize = new Size(256, 64);

      this.button = new Button();
      this.button.Location = new Point(80, 16);
      this.button.Size     = new Size(96, 32);
      this.button.Text     = "ここを押せ";
      this.button.Click   += new EventHandler(button_Click);
      this.Controls.Add(this.button);
    }

    // ボタンが押されたときの処理
    private void button_Click(object sender, System.EventArgs e)
    {
      MessageBox.Show("ようこそ。");
    }

    static void Main() 
    {
      Application.Run(new WelcomeForm());
    }
  }
}
```


      
このサンプルプログラムでは、ボタンがひとつあるウィンドウが表示され、
ボタンを押すと“ようこそ。”というメッセージが表示されます。

      
<figure>

[![C#によるGUIプログラムの例](../../../../assets/media/ufcpp2000/csharp/fig/guiwelcome.png)](../../../../assets/media/ufcpp2000/csharp/fig/guiwelcome.png)

<figcaption>C#によるGUIプログラムの例</figcaption>
</figure>


      
ちなみに、
Visual Studio でこのソースをコンパイルする場合、
「Windows フォームアプリケーション」プロジェクトにしてください。
また、コマンドラインで csc を使ってコンパイルする場合、
（ソースファイルの名前を WelcomeForm.cs とすると）以下のようなコマンドでコンパイルします。

      
```console {title="コマンドラインで csc を使ってコンパイルする場合"}
csc /r:system.windows.forms.dll /r:system.drawing.dll /t:winexe WelcomeForm.cs
```
   
</div>

## <a id="sec-generated-title-7"></a> <a id="web"></a>Webアプリ例

<h5 class="version version10">Ver. 10</h5>

C# 10.0/ .NET 6 世代では、Webアプリ開発を以下のような十数行のコードから始められるようになりました。

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGet("/", () => "Hello World!");

app.Run();
```

![.NET 6 からの「最小限の Web アプリ」テンプレートの実行結果の例](../../../../assets/media/1190/dotnet6webapp.png)
