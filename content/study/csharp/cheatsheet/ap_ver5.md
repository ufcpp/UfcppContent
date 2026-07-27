---
title: "C# 5.0 の新機能"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/ap_ver5/"
content_type: "Article"
published_at: "2010-10-31T00:00:00"
updated_at: "2025-01-01T18:44:11"
tags:
  - "Ver. 5.0"
umbraco_id: 1181
parent_id: 1174
sort_order: 7
aliases:
  - "/study/csharp/ap_ver5.html"
---

# C# 5.0 の新機能

## <a id="sec-generated-title-1"></a> <a id="ver5"></a>C# 5.0

<div class="version version5">Ver. 5.0</div>

<table>
<tr>
<th>リリース時期</th>
<td>2012/8</td>
</tr>
<tr>
<th>同世代技術</th>
<td>
<ul>
<li>Visual Studio 2012</li>
<li>.NET Framework 4.5</li>
<li>Visual Basic 11</li>
<li>Windows Runtime</li>
<li>.NET Core Profile (現在の.NET Coreの原型)</li>
</td>
</tr>
<tr>
<th>要約・目玉機能</ht>
<td>
<ul>
<li>非同期メソッド</li>
</ul>
</td>
</tr>
</table>

2010年10月、PDC 10 にて C# 5.0 がらみの情報が少し。

.NET 4 以降、ライブラリもツールも、拡張という形で小出しにされるようになりましたが、
言語機能も多分に漏れず、小出しに出て来ることになりました。
PDC 10 では、「C# 5.0 / VB 11」という形での発表ではなく、
「Asynchronous Programming for C# and Visual Basic」という名前で、非同期処理に関する部分だけが公開されました。

2011年9月の BUILD でも、少し C# 5.0 の追加機能、Caller Info 属性についての情報が公開されました。


## <a id="sec-generated-title-2"></a> <a id="async"></a>非同期処理

async/await キーワードを使うことで、非同期処理を簡単に行えるようになります。

* 「[非同期処理](../async/sp5_async.md)」


非同期処理には、処理の内容をシーケンス図やフローチャートに起こすと、同期の時と全く同じになるものがあります。
このようなタイプのものに対しては、async/await を使うことによって、同期処理とほぼ同じコードで非同期処理を行えます。


## <a id="sec-generated-title-3"></a> <a id="CallerInfo"></a>Caller Info 属性

C++ でいうところの __FILE__ や __LINE__ マクロに相当するデバッグ用診断を実現するための機能です。

以下のように、メソッドの引数に属性をつけておくと、その引数に対して、コンパイラーが診断情報を渡してくれます。

```csharp
public static class Trace
{
    public static void WriteLine(string message,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0,
        [CallerMemberName] string member = "")
    {
        var s = string.Format("{0}:{1} - {2}: {3}", file, line, member, message);
        Console.WriteLine(s);
    }
}
```


* CallerFilePath: 呼び出し元のファイル名。

* CallerLineNumber: 呼び出し元の行番号。

* CallerMemberName: 呼び出し元のメンバー名（メソッド名、プロパティ名、イベント名等々）。


ちなみに、コンパイル結果的には、定数やオプション引数と同様、コンパイル時のリテラル埋め込みになります。（実行時のコストは 0。ファイル名などを直接手書きで埋め込んだ場合と全く同じ結果になる。）

イテレーター ブロックや非同期メソッド内から呼び出しても、ちゃんとメソッド名を拾えます。
また、匿名関数（ラムダ式や匿名メソッド式）中で呼び出した場合も、匿名関数を書いているメソッドの名前が使われます。
（これらの機能は内部的に別のクラスやメソッドを生成していますが、そのコンパイラー生成名ではなく、元のメソッド名を拾います。）

詳しくは「[呼び出し元情報(caller info)](../start/miscreservedattribute.md#CallerInfo)」で説明します。


## <a id="sec-generated-title-4"></a> <a id="foreach"></a>foreach の仕様変更

C# 5.0 で、foreach の仕様が少しだけ変わるそうです。
（一応、破壊的変更になります（既存のコードが動かなくなる可能性が0ではない）。）

C# 4.0 までは、foreach ステートメントは以下のように展開されていました。

```csharp
static void ForeachSample<T>(IEnumerable<T> data)
{
    // 展開前
    foreach (var x in data)
    {
        Console.WriteLine(x);
    }

    // 展開後
    using (var e = data.GetEnumerator())
    {
        T x;
        while(e.MoveNext())
        {
            x = e.Current;
            Console.WriteLine(x);
        }
    }
}
```


これで何が嫌かというと、ラムダ式で、foreach のループ変数（上記の例でいう x）をキャプチャしたときの挙動がいまいちなことです。
例えば、以下のコードを実行したとします。

```csharp
var data = new[] { 1, 2, 3, 4, 5 };

Action a = null;

foreach (var x in data)
{
    a += () => Console.WriteLine(x);
}

a();
```


C# 4.0 まででは、以下のような結果が得られる仕様になっていました。

```console
5
5
5
5
5
```


C# 5.0 では foreach の展開結果が以下のように変わる予定です。ループ変数の位置が、while ループの中に移動しました。

```csharp
static void ForeachSample<T>(IEnumerable<T> data)
{
    // 展開前
    foreach (var x in data)
    {
        Console.WriteLine(x);
    }

    // 展開後
    using (var e = data.GetEnumerator())
    {
        while(e.MoveNext())
        {
            T x;
            x = e.Current;
            Console.WriteLine(x);
        }
    }
}
```


これで、先ほどのラムダ式の例の実行結果は、以下のように変わります。

```console
1
2
3
4
5
```


C# 4.0 までの仕様が前述のようになっていたのは、以下のような理由によるものです。

* x と data の登場順序が変わらないように展開したかった。

* for ステートメントのループ変数と挙動をそろえたかった。


C# 5.0 で、破壊的変更になってまでこの仕様を変えたのは、ラムダ式の利用が広まって、上記のようなループ変数のキャプチャでの問題が大きくなったからのようです。
破壊的変更にならないように、構文自体を少し変えるという案もあったようですが、影響範囲は狭いだろうということで、仕様変更に決まったようです。
