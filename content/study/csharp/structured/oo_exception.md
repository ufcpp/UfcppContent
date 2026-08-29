---
title: "例外処理"
source_url: "https://ufcpp.net/study/csharp/structured/oo_exception/"
content_type: "Article"
published_at: "2015-05-06T14:09:09"
updated_at: "2016-10-25T00:00:00"
tags:
  - "Ver. 6.0"
umbraco_id: 1245
parent_id: 1217
sort_order: 17
aliases:
  - "/study/csharp/oo_exception.html"
---

# 例外処理

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<strong id="exc" class="keyword">例外</strong>（exception）とは、
本来ならばプログラム中で起こってはいけないことが起こってしまうことをいいます。
堅牢なプログラムを作成するためには、
例外が起こったときでもプログラムが異常な動作をしないよう、
しっかりと<strong id="exchandle" class="keyword">例外処理</strong>（exception handling）を行う必要があります。

C# では、例外処理を行うための専用の構文が用意されていて、
プログラマが例外処理を容易に行えるようになっています。


##### <a id="sec-generated-title-2"></a>ポイント

* 例外: 「開こうとしたファイルが存在しなかった」など、特別な対処が必要な状況。

* 例外への対処には、例外用の構文があるのでそれを使いましょう。

* try { 例外が発生する可能性のあるコード } catch(例外) { 例外処理 }



## <a id="sec-generated-title-3"></a> <a id="exception"></a>例外処理とは

例外の例を挙げると、ユーザーが想定外の文字列を入力してきたときや、
プログラムに必要なファイルが開けなかったときなどがあります。

例えば、文字列を整数に変換することを考えてみます。
簡単化のため、とりあえず正の整数のみを扱うことにします。
想定外の文字列が来ないものと仮定するとプログラムは以下のようになります。

```csharp {title="文字列→整数変換 関数定義 (例外を想定せず)"}
// 文字→整数
static int CharToInt(char c)
{
  return c - '0';
}

// 文字列→整数
static int StringToInt(string str)
{
  int val = 0;
  foreach(char c in str)
  {
    int i = CharToInt(c);
    val = val * 10 + i;
  }
  return val;
}
```


当然、この関数に対して想定外の文字列を入力すると、おかしな結果が得られます。

```csharp {title="文字列→整数変換 利用側 (例外を想定せず)"}
static void Main()
{
  Console.Write("{0}\n", StringToInt("12345"));
  Console.Write("{0}\n", StringToInt("12a45")); // 途中に数字以外の文字が
}
```


```console
12345
16945    ←変な値が出力されてる
```


利用側が「想定外の文字列は絶対に入力しない」とか、
「変な結果が出てきても文句は言わない」とかいう風に開き直っているのなら、
別にこれでも問題はありません。
しかし、通常は想定外の文字列が入力されていないかどうか調べる手段が必要になると思います。

このような例外が起きたとき、
例外処理用の構文が用意されていない言語ではどのように対処していたかというと、
「通常はありえない値を返す」とか、
「関数の戻り値とは別に、関数が正しく終了したかどうかを示すフラグを返す」
といった手段を用いていました。

例えば、今回の場合、正の整数しか想定していないので、
想定外の文字列が来たときには負の数を返すことにしておけば、
例外が起きたかどうか調べることが出来ます。

```csharp {title="文字列→整数変換 関数定義 (例外処理構文未使用)"}
// 文字→整数
static int CharToInt(char c)
{
  if('0' <= c && c <= '9')
    return c - '0';
  else
    return -1; // 想定外の文字が入力された場合、-1 を返す。
}

// 文字列→整数
static int StringToInt(string str)
{
  int val = 0;
  foreach(char c in str)
  {
    int i = CharToInt(c);
    if(i == -1) return -1; // 想定外の文字列が入力された場合、-1 を返す。
    val = val * 10 + i;
  }
  return val;
}
```


関数の利用側のコードは以下のようになります。

```csharp {title="文字列→整数変換 利用側 (例外処理構文未使用)"}
static void Main()
{
  int i;

  i = StringToInt("12345");
  if(i == -1)
    Console.Write("想定外の文字列が入力されました");
  else
    Console.Write("{0}\n", i);

  i = StringToInt("12a45");
  if(i == -1)
    Console.Write("想定外の文字列が入力されました");
  else
    Console.Write("{0}\n", i);
}
```


```console
12345
想定外の文字列が入力されました
```



## <a id="sec-generated-title-4"></a> <a id="syntax"></a>例外処理構文

上述したように、例外処理専用の構文を用いなくても例外処理を行えます。
しかし、上述したような方法にはいくつか欠点があります。
その欠点を以下に挙げます。

* 例外の検出が面倒。

* 正常動作部と例外処理部の区別が分かりにくい。

* 関数利用者に例外処理を行うことを強制出来ない。

* 関数を呼び出すたびに例外処理用のコードを書く必要がある。


例外処理構文を用いるとこれらの問題を解決することが出来ます。
ここでようやく本題に入るわけですが、
それでは、C# の例外処理構文について説明していきたいと思います。


##### <a id="sec-generated-title-5"></a>throw

まず、関数定義側、すなわち、例外が発生する可能性のある側では、
<strong id="throw" class="keyword">throw 文</strong>を使って例外が起こったことを利用側に知らせます。
throw 文は以下のようにして使用します。

```csharp {title="throw 文"}
throw 例外クラスのインスタンス
```


この throw 文は想定外のことが起こった場所に挿入します。
このような処理を「<em>例外を投げる</em>」といいます。
例外が投げられると、正常動作部の処理は中断され、例外処理部が呼び出されます。

throw 文によって投げられる例外は、
<code>System.Exception</code> クラスの派生クラスのインスタンスです。
それ以外のクラスのインスタンスを throw することは出来ません。
例えば、<code>throw new Exception();</code> というようにします。

例として先ほどの文字列→整数変換関数を throw 文を使って書き直してみましょう。

```csharp {title="文字列→整数変換 関数定義 (throw 使用)"}
// 文字→整数
static int CharToInt(char c)
{
  if(c < '0' || '9' < c)
    throw new FormatException(); // 不正な文字が入力された場合、例外を投げる

  return c - '0';
}

// 文字列→整数
static int StringToInt(string str)
{
  int val = 0;
  foreach(char c in str)
  {
    int i = CharToInt(c);
    val = checked(val * 10 + i);
  }
  return val;
}
```



##### <a id="sec-generated-title-6"></a>try-catch-finally

次に、関数利用側、すなわち、例外を処理する側では、
<strong id="try" class="keyword">try-catch-finally 文</strong>を使って例外を処理します。
try-catch-finally 文は以下のようにして使用します。

```csharp {title="try-catch-finally 文"}
try
{
  例外が投げられる可能性のあるコード
}
catch(例外の種類)
{
  例外処理コード
}
finally
{
  例外発生の有無にかかわらず実行したいコード
  リソースの破棄などを行う
}
```

`try`句は必須ですが、`catch`、`finally`はどちらか片方だけにできます。
というか、`catch`を使いたい範囲と`finally`を使いたい範囲は違っていることが多く、
片方だけ使うことは多いです。

```csharp {title="catch のみの try、finally のみの try"}
try
{
    (リソースの寿命の方が広い)

    try
    {
        例外が投げられる可能性のあるコード(範囲が狭い)
    }
    catch (例外の種類)
    {
        例外処理
    }
}
finally
{
    リソースの破棄など
}
```

本項では主に`catch`の方について説明して行きます。
`finally` については「[リソースの破棄](../resource/oo_dispose.md)」で改めて説明します。
ちなみに、try-finally には using ステートメントという短縮記法があるので、
`finally`が必要になる頻度は少なめです。

##### <a id="sec-generated-title-7"></a>複数の catch

`catch`句は複数並べて書けます。

こちらも例として、先ほどの文字列→整数変換関数利用側コードを try-catch 文を使って書き直してみましょう。

```csharp {title="文字列→整数変換 関数利用側 (try-catch使用)" highlight-text="try"}
static void Main()
{
    try
    {
        Console.Write("{0}\n", StringToInt("12345"));
        Console.Write("{0}\n", StringToInt("12a45"));
        //↑ ここで FormatException 例外が投げられる。
    }
    catch(FormatException)
    {
        Console.Write("想定外の文字列が入力されました");
    }
    catch(OverflowException)
    {
        Console.Write(桁あふれしました");
    }
}
```

ちなみに、同じ型の`catch`を複数並べるとエラーになります。

```csharp {title="同じ型のcatchを並べるとエラー" error-ranges="7:7-7:22"}
try
{
}
catch(FormatException)
{
}
catch(FormatException)
{
}
```

##### <a id="sec-generated-title-8"></a>try-catch 文の利点

try-catch 文を使った例外処理には以下のような利点があります。

* 正常動作部と例外処理部の区別が明確になります。
    * try の中には動作が正常な時の処理が、 catch の中には例外発生時の対処のみが書かれます。



* 関数利用側に例外処理させることを強制できます。
    * もし、正しく例外処理しなければ（throw された例外を catch しなければ）、プログラムは強勢終了されます。

    * 対処のしようのないエラーがあった場合、対処できないままプログラムが動き続けるよりは、強制終了される方が後々困ることが少なくなります。



* 例外処理部（catch 節）や、正常・例外問わず必ず実行する必要なある部分（finaly 節）が一か所にまとまります。
    * 同じような処理を何か所も書く必要がなくなります。


### <a id="sec-generated-title-9"></a> <a id="throwexpr"></a>throw 式

<h5 class="version version7">Ver. 7</h5>

`throw`はこれまでステートメントでしか書けませんでした。
C# 6ではバージョン アップとともに、式として書けるものや、式でだけ使える便利な文法が増えている中、
`throw`も式として書きたいという要望がありました。

限られてはいますが、確かに書きたい場面はあります。
そこで、C# 7では、`throw`式(throw expression)が追加されました。
以下の3カ所でに書けます。

- ラムダ式や式形式メンバーの中(`=>` の後ろ)
- [null合体演算子](../resource/sp2_nullable.md#nullableType)(`??`)の後ろ
- [条件演算子]()の第2、第3引数(条件式以外の部分。 `:` の前後)

```csharp {title="throw 式"}
// 式形式メンバーの中( => の直後)
static void A() => throw new NotImplementedException();

static string B(object obj)
{
    // null 合体演算子(??)の後ろ
    var s = obj as string ?? throw new ArgumentException(nameof(obj));

    // 条件演算子(?:)の条件以外の部分
    return s.Length == 0 ? "empty" :
        s.Length < 5 ? "short" :
        throw new InvalidOperationException("too long");
}
```

これ以外の文脈で`throw`式を書くことはできません。
例えば、以下のコードはコンパイル エラーになります。

```csharp {title="=&gt;、??、?: 以外の場所に throw 式は書けない"}
static void C()
{
    // コンパイル エラー。この文脈に throw 式は書けない
    B(throw new InvalidOperationException());
}
```

ちなみに、式になっている以上「戻り値の型」を考えないといけないわけですが、
`throw`式の戻り値の型は「任意の型に変換可能」とみなされます。
`throw` 式単体で具体的な型を持っているわけではないので、以下のように、型を決めれない書き方をするとコンパイル エラーになります。

```csharp {title="型が決まらなくてコンパイル エラーになる例"}
// コンパイル エラー。null(型を持っていない)と並べると型が決まらない。
var x = true ? null : throw new Exception();

// コンパイル エラー。throw 式同士を並べると型が決まらない。
var y = true ? throw new InvalidOperationException() : throw new NotSupportedException();
```

### <a id="sec-generated-title-10"></a> <a id="std"></a>標準で用意されている例外クラス

.NET Frameowrk が標準で提供する例外クラスのうち、よく出てくるもの/よく使うものをいくつか例に挙げます。

<table summary="">

	<tr>
		<th>クラス名</th>
		<th>throw される状況</th>
	</tr>
	<tr>
		<td markdown="1" colspan="2">System 名前空間</td>
	</tr>
	<tr>
		<td markdown="1">ArgumentException</td>
		<td markdown="1">メソッドの引数が変な場合。ArgumentNullExceptionやArgumentOutOfRangeException以外の場合で変な時に使う。</td>
	</tr>
	<tr>
		<td markdown="1">ArgumentNullException</td>
		<td markdown="1">引数がnullの場合。</td>
	</tr>
	<tr>
		<td markdown="1">ArgumentOutOfRangeException</td>
		<td markdown="1">メソッドの許容範囲外の値が引数として渡された場合。</td>
	</tr>
	<tr>
		<td markdown="1">ArithmeticException</td>
		<td markdown="1">算術演算によるエラーの基本クラス。OverflowException, DivideByZeroException, NotFiniteNumberException以外の算術エラーを示したければ使う。</td>
	</tr>
	<tr>
		<td markdown="1">OverflowException</td>
		<td markdown="1">算術演算やキャストでオーバーフローが起きた場合。</td>
	</tr>
	<tr>
		<td markdown="1">DivideByZeroException</td>
		<td markdown="1">0で割ったときのエラー。</td>
	</tr>
	<tr>
		<td markdown="1">NotFiniteNumberException</td>
		<td markdown="1">浮動小数点値が無限大の場合。</td>
	</tr>
	<tr>
		<td markdown="1">FormatException</td>
		<td markdown="1">引数の書式が仕様に一致していない場合。</td>
	</tr>
	<tr>
		<td markdown="1">IndexOutOfRangeException</td>
		<td markdown="1">配列のインデックスが変な場合。</td>
	</tr>
	<tr>
		<td markdown="1">InvalidCastException</td>
		<td markdown="1">無効なキャストの場合。</td>
	</tr>
	<tr>
		<td markdown="1">InvalidOperationException</td>
		<td markdown="1">引数以外の原因でエラーが起きた場合。</td>
	</tr>
	<tr>
		<td markdown="1">ObjectDisposedException</td>
		<td markdown="1">Dispose済みのオブジェクトで操作が実行される場合。</td>
	</tr>
	<tr>
		<td markdown="1">NotImplementedException</td>
		<td markdown="1">メソッドが未実装の場合。</td>
	</tr>
	<tr>
		<td markdown="1">NotSupportedException</td>
		<td markdown="1">呼び出されたメソッドがサポートされていない場合、または呼び出された機能を備えていないストリームに対して読み取り、シーク、書き込みが試行された場合。</td>
	</tr>
	<tr>
		<td markdown="1">NullReferenceException</td>
		<td markdown="1">nullオブジェクト参照を逆参照しようとした場合。</td>
	</tr>
	<tr>
		<td markdown="1">PlatformNotSupportException</td>
		<td markdown="1">特定のプラットフォームで機能が実行されない場合。</td>
	</tr>
	<tr>
		<td markdown="1">TimeoutException</td>
		<td markdown="1">指定したタイムアウト時間が経過した場合。</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">System.Collections.Generics 名前空間</td>
	</tr>
	<tr>
		<td markdown="1">KeyNotFoundException</td>
		<td markdown="1">コレクションに該当するキーが無い場合。</td>
	</tr>
	<tr>
		<td markdown="1" colspan="2">System.IO 名前空間</td>
	</tr>
	<tr>
		<td markdown="1">DirectoryNotFoundException</td>
		<td markdown="1">ディレクトリが無い場合。</td>
	</tr>
	<tr>
		<td markdown="1">FileNotFoundException</td>
		<td markdown="1">ファイルが無い場合。</td>
	</tr>
	<tr>
		<td markdown="1">EndOfStreamException</td>
		<td markdown="1">ストリームの末尾を超えて読み込もうとしている場合。</td>
	</tr>
</table>

## <a id="sec-generated-title-11"></a> <a id="propagation"></a>例外の伝搬

例外は、`catch`句でキャッチされるまで、どんどん上位の呼び出し元に伝搬していきます。

```csharp {title="例外は上位に伝搬する"}
using System;

class Program
{
    // A で投げた例外が
    static void A() => throw new NotImplementedException();

    // B → C と伝搬して
    static void B() => A();
    static void C() => B();

    static void Main()
    {
        try
        {
            C();
        }
        catch(NotImplementedException ex)
        {
            // 最終的にここでキャッチされる
            Console.WriteLine(ex);
        }
    }
}
```

例外が最後までキャッチされなかった場合<sup>※</sup>、アプリ自体が停止します。

```csharp {title="未処理例外"}
using System;

class Program
{
    static void A() => throw new NotImplementedException();
    static void B() => A();
    static void C() => B();

    static void Main()
    {
        C(); // 例外が出るけども、誰もキャッチしていない
        // この時点でアプリが停止

        Console.WriteLine("ここは絶対に通らない");
    }
}
```

このとき、以下のような、例外で停止した旨を示すメッセージが表示されます。

```console {title="未処理例外"}
Unhandled Exception: System.NotImplementedException: The method or operation is not implemented.
   at Program.A() in C:\Projects\ConsoleApp1\Program.cs:line 5
   at Program.B() in C:\Projects\ConsoleApp1\Program.cs:line 6
   at Program.C() in C:\Projects\ConsoleApp1\Program.cs:line 7
   at Program.Main() in C:\Users\xii-h\Documents\Visual Studio 2017\Projects\ConsoleApp1\ConsoleApp1\Program.cs:line 11
```

例外の種類の他に、どのメソッドを経由して例外が発生したかや、メソッドがソースコード中のどこにあるかなどの情報が入っています。この情報を[スタックトレース](misc_stacktrace.md)と呼びます。

<sup>※</sup> GUI アプリの場合、GUI フレームワーク内で例外がキャッチされていて、未処理例外があっても即座にアプリが停止することはありません。代わりに、エラー ダイアログ画面が表示されたりします。

## <a id="sec-generated-title-12"></a> <a id="guide"></a>例外処理の指針

一般に、tyr-catch を用いた例外処理は、
if 文などを使った値のチェックに比べて、
実行速度が遅いといわれています。
try ブロックで囲んだだけでは（例外が発生しなければ）ほとんどオーバーヘッドはないのですが、
例外発生時には少し大き目のコストが発生します。

このコストを考えても、try-catch を用いるメリットの方が大きいのですが、
まあ、避けれるのならばコストの大きな処理は避けたいというのがプログラマの心情というものです。

ということで、例外の使い方（特に、避けれる例外を避ける方法）について別ページで説明をします → 「[[雑記] 例外の使い方](misc_exception.md)」。

## <a id="sec-generated-title-13"></a> <a id="exception-filter"></a>例外フィルター

<h5 class="version version6">Ver. 6</h5>

C# 6で、例外のcatch句に続けてwhenと書くことで、catchしたい例外の条件を書けるようになりました。
この機能を<strong id="key-exception-filter" class="keyword">例外フィルター</strong>(exception filter)といいます。

```csharp {title="例外フィルター" highlight-text="when (条件)"}
try
{
  例外が投げられる可能性のあるコード
}
catch(例外の種類) when (条件)
{
  例外処理コード
}
```

用途としては、例えば、以下のように、複数の種類の例外に対して、同じ例外処理を掛けたい場合があります。

```csharp
try
{
    F();
}
catch (DirectoryNotFoundException e)
{
    // DirectoryNotFoundException のときと FileNotFoundException の時で
    // 全く同じ例外処理の仕方をしたい場合がある。
    Console.WriteLine(e);
}
catch (FileNotFoundException e)
{
    // DirectoryNotFoundException のときと FileNotFoundException の時で
    // 全く同じ例外処理の仕方をしたい場合がある。
    Console.WriteLine(e);

    // コピペ コードになっちゃうので嫌！
}
```

これに対して、例外フィルターを使うと、以下のように書き直せます。

```csharp
try
{
    F();
}
catch (IOException e) when (e is DirectoryNotFoundException || e is FileNotFoundException)
{
    // DirectoryNotFoundException のときと FileNotFoundException の時で
    // 全く同じ例外処理の仕方をしたい場合がある。
    Console.WriteLine(e);
}
```

また、入れ子になっている例外の処理にも有効です。
例えば、Parallelクラス(System.Threading.Tasks名前空間)のメソッドを使って並列処理を行った場合、1段階ラップされた状態で例外が throw されます。この時、以下のように例外フィルターを使うことで、catch句が書きやすくなります。

```csharp
try
{
    Parallel.For(0, 10000, F);
}
catch (AggregateException e) when (e.InnerExceptions.Any(i => i is ArgumentException))
{
    // F が ArgumentException を throw する場合でも、
    // Parellel.For を通した結果、ここに来る例外は AggregateException で、
    // AggregateException.InnerExceptions の中に ArgumentException が入っている。
}
```

`when`句が付いている場合は、同じ型の`catch`を複数書くことができます。
この場合、書いた順に上から調べて最初に条件を満たした`catch`句が実行されます。

```csharp
try
{
}
catch (Exception e) when (e is FormatException || (e is AggregateException a && a.InnerException is FormatException))
{
}
catch (Exception e) when (e is OverflowException || (e is AggregateException a && a.InnerException is OverflowException))
{
}
catch (Exception e)
{
}
```

ちなみに、この例外フィルターは、[IL](../../il/summary/il_about.md)のレベルでは.NET 1.0の頃からある機能です。単に、それに対応するILコードをC#を使って書くすべが今までなく、C# 6で追加されたというものです。
