---
title: "値の入出力"
source_url: "https://ufcpp.net/study/csharp/start/st_consoleio/"
content_type: "Article"
published_at: "2015-05-06T14:07:26"
updated_at: "2020-09-13T10:49:49"
tags: []
umbraco_id: 1195
parent_id: 1190
sort_order: 3
aliases:
  - "/csharp/st_consoleio"
  - "/csharp/st_consoleio.html"
  - "/csharp/start/st_consoleio/"
  - "/study/csharp/st_consoleio"
  - "/study/csharp/st_consoleio.html"
---

# 値の入出力

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

これから本格的に C# によるプログラミングを解説して行くことになりますが、
ただ文章で説明するよりも実際にサンプルプログラムを挙げて説明するほうが分かりやすいと思うので、
そうして行きたいと思います。
また、ただ単に計算を行うだけのプログラムよりも、
ユーザーからの入力を受け取って、計算結果を出力するようなもののほうが面白いでしょうから、
そのようなサンプルプログラムを挙げていきたいと思っています。

そのためにまず、C# の文字ベースプログラムにおける入出力の行い方について簡単に説明しておきます。
ただ、現時点ではまだ詳しい説明は出来ませんので、
「とりあえずこうすれば入出力が行える」ということだけ覚えておいてもらうことになります。

<figure>
	[![値の入出力](../../../../assets/media/ufcpp2000/csharp/fig/io.png)](../../../../assets/media/ufcpp2000/csharp/fig/io.png)
	<figcaption>値の入出力</figcaption>
</figure>



##### <a id="sec-generated-title-2"></a>ポイント

* 「まずは慣れろ」ということで、とりあえず今は詳しい説明省略。



## <a id="sec-generated-title-3"></a> <a id="input"></a>入力

C#でユーザーからの入力を受け取りたい場合、<em>
        <code>Console.ReadLine</code>
      </em> というものを使います。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

```csharp
var str = Console.ReadLine(); // ユーザーの入力した文字列を1行読み込む
```


</div>
<div>

```vbnet
Dim str = Console.ReadLine()
```


</div>
<div>

```fsharp
let str = Console.ReadLine()
```


</div>
<div>

```cpp
auto str = Console::ReadLine();
```


</div>
</div>


数値を入力したい場合には、さらに <code>Parse</code> というものを使って、以下のようにします。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

```csharp
var n    = int.Parse(Console.ReadLine());  // ユーザーの入力した整数を読み込む
var x = double.Parse(Console.ReadLine()); // ユーザーの入力した実数を読み込む
```


</div>
<div>

```vbnet
Dim n = Integer.Parse(Console.ReadLine())
Dim x = Double.Parse(Console.ReadLine())
```


</div>
<div>

```fsharp
let n = Int32.Parse(Console.ReadLine())
let x = Double.Parse(Console.ReadLine())
```


</div>
<div>

```cpp
auto n = Int32::Parse(Console::ReadLine());
auto x = double::Parse(Console::ReadLine());
```


</div>
</div>


<code>int</code> や <code>double</code> については「[変数と式](st_variable.md)」で、
<code>var</code> については「[型推論](st_variable.md#infer)」で、
<code>Console</code> については「[ライブラリ](../structured/st_library.md)」で説明します。


## <a id="sec-generated-title-4"></a> <a id="output"></a>出力

計算結果などを出力したい場合には <em>
        <code>Console.Write</code>
      </em> というものを使います。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

```csharp
int m = 1, n = 3;
Console.Write("m = {0}, n = {1}", m, n); // 文字や数値の出力
```


</div>
<div>

```vbnet
Dim m = 1, n = 3
Console.Write("m = {0}, n = {1}", m, n)
```


</div>
<div>

```fsharp
let m, n = 1, 3
Console.Write("m = {0}, n = {1}", m, n)
```


</div>
<div>

```cpp
int m = 1, n = 3;
Console::Write("m = {0}, n = {1}", m, n);
```


</div>
</div>


この出力の仕方はフォーマット出力といって、
<code>{0}</code> とある場所に <code>m</code> の値が、
<code>{1}</code> とある場所に <code>n</code> の値が書き込まれます。
例えば上述のサンプルの出力結果は以下のようなものになります。

```console
m = 1, n = 3
```



##### <a id="sec-generated-title-5"></a>サンプル

<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

```csharp
using System;

class Program
{
    static void Main()
    {
        // 入力を促すメッセージの表示して、文字を入力してもらう
        Console.Write("あなたのお名前は？ : ");
        var name = Console.ReadLine();

        // 入力を促すメッセージの表示して、数値を入力してもらう
        Console.Write("あなたのお年は？   : ");
        var age = int.Parse(Console.ReadLine());

        // メッセージの出力
        Console.WriteLine("{0} ({1}歳) さん、ようこそお越しくださいました。", name, age);
    }
}
```


</div>
<div>

```vbnet
Module Program

    Sub Main()
        Console.Write("あなたのお名前は？ : ")
        Dim name = Console.ReadLine()

        Console.Write("あなたのお年は？   : ")
        Dim age = Integer.Parse(Console.ReadLine())

        Console.WriteLine("{0} ({1}歳) さん、ようこそお越しくださいました。", name, age)
    End Sub

End Module
```


</div>
<div>

```fsharp
open System

Console.Write("あなたのお名前は？ : ")
let name = Console.ReadLine()

Console.Write("あなたのお年は？   : ")
let age = Int32.Parse(Console.ReadLine())

Console.WriteLine("{0} ({1}歳) さん、ようこそお越しくださいました。", name, age)
```


</div>
<div>

```cpp
Console::Write("あなたのお名前は？ : ");
auto name = Console::ReadLine();

Console::Write("あなたのお年は？   : ");
auto age = int::Parse(Console::ReadLine());

Console::WriteLine("{0} ({1}歳) さん、ようこそお越しくださいました。", name, age);
```


</div>
</div>


```console
あなたのお名前は？ : tiyu
あなたのお年は？   : 12
tiyu (12歳) さん、ようこそお越しくださいました。
```



## <a id="sec-generated-title-6"></a> <a id="gui"></a>GUI 雛形プログラム

GUI プログラム（Windows アプリ）を使って演習問題（の一部）を解いてもらうために、
演習用 GUI プログラムの雛形を用意しました。

[GUI 雛形プログラム1](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/UserInputSample)

プログラムは図1に示すような見た目で、
A ～ E のテキストボックスに値を入力し、
[実行] ボタンを押してプログラムを実行します。

<figure>
	[![GUI 雛形プログラム1](../../../../assets/media/ufcpp2000/csharp/fig/InputGui.png)](../../../../assets/media/ufcpp2000/csharp/fig/InputGui.png)
	<figcaption>GUI 雛形プログラム1</figcaption>
</figure>


GUI プログラムの大部分は、この時点までの知識では説明できませんが、
今はとりあえず、分からない大部分は無視してもらって、
InputData.cs 中の「TODO: ↓ここに演習問題の回答コードを書いてください」というコメントのある部分だけ書き換えてください。

<span class="expand-button" title="展開/折畳">（旧バージョン）</span>
<div class="expand-panel" markdown="1" title="（旧バージョン）">

(※旧ウェブサイトから未移植につきコードなし)      
      
プログラムは図1に示すような見た目で、
A ～ E のテキストボックスに値を入力し、
[Run] ボタンを押してプログラムを実行します。

      
<figure>
	[![GUI 雛形プログラム1](../../../../assets/media/ufcpp2000/csharp/fig/Form1.png)](../../../../assets/media/ufcpp2000/csharp/fig/Form1.png)
	<figcaption>GUI 雛形プログラム1</figcaption>
</figure>


      
GUI プログラムの大部分は、この時点までの知識では説明できませんが、
今はとりあえず、分からない大部分は無視してもらって、
Form1.cs 中の「TODO: ↓ここに演習問題の回答コードを書いてください」というコメントのある部分だけ書き換えてください。

    
</div>

演習問題の多くは基本的に CUI プログラム（コマンドプロンプト）を前提に作っていますが、
値を入力してもらって、何らかの計算を行って、結果を出力するタイプの演習問題には、
この雛形プログラムを利用できます。
## <a id="exercise"></a>演習問題

### <a id="exercise-console1"></a>問題 1


Console.Write を用いて、自分の名前を画面に表示せよ。


#### 解答例 1


```csharp
using System;

class Sample
{
  static void Main()
  {
    Console.Write("岩永信之");
  }
}
```



### <a id="exercise-console2"></a>問題 2


Console.ReadLine を用いて文字列を1行読み込み、
Console.Write を用いて読んだ文字列をそのまま鸚鵡返しするプログラムを作成せよ。

おまけ： 1度読み込んだ文字列を2度ずつ鸚鵡返しするものを作成せよ。


#### 解答例 1


```csharp
using System;

class Sample
{
  static void Main()
  {
    string line = Console.ReadLine();
    Console.Write(line);
  }
}
```



#### 解答例 2


```csharp
using System;

class Sample
{
  static void Main()
  {
    string line = Console.ReadLine();
    Console.Write(line);
    Console.Write(line);
  }
}
```
