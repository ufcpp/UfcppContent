---
title: "小ネタ string型のマーシャリング"
source_url: "https://ufcpp.net/blog/2016/12/tipsstringmarshal/"
content_type: "BlogEntry"
published_at: "2016-12-21T00:00:16"
updated_at: "2016-12-20T15:00:36"
tags: []
umbraco_id: 2004
parent_id: 1969
sort_order: 20
aliases: []
---

# 小ネタ string型のマーシャリング

数値や文字列の内部形式は、プログラミング言語ごとに違っています。プログラミング言語をまたいで値を受け渡しするには、その間に変換処理が必要になります。その変換処理のことをマーシャリング(marshalling: 整列する(特に、指揮官の指示で整列、集結、先導されるような意味あい))と言います。

## 無変換転送

といっても、変換処理はそれなりに重たい処理なので、異なるプログラミング言語間でも揃えられる限りには同じ形式を使って、そのまま値を渡せるようにしたくなるものです。C#では、Windows APIが使っている内部形式と揃えた形式にすることで、マーシャリング時の変換処理を極力減らしていたりします。

数値型は比較的簡単です。何せ、C#が動く環境は大体Little EndianのCPUですし、C#コンパイラーはアラインメントにも気を使った仕様になっていています。この辺りが一致しているなら、たいていの数値型は他のプログラミング言語にそのまま渡すことができます。こういう、そのまま渡せる型のことを[blittable](https://msdn.microsoft.com/ja-jp/library/75dwhxf7(v=vs.110).aspx)型というようです(blitはboundary block transferの略語から派生した「生データ転送する」という意味の単語)。

## 文字列のマーシャリング

問題は文字列です。文字列は、数値と同じくらい汎用的に使われるものですが、その内部形式は数値程単純ではありません。文字コードはどうなっているのかや、文字列の長さの管理などが、プログラミング言語ごとに異なります。

で、C#の文字列がどうなっているかというと、[Build Insiderの記事](http://www.buildinsider.net/language/csharpunicode/01)で書きましたが、COMの`BSTR`型互換です。そして、`BSTR`型も、C言語やC++でよく使われるUTF-16のnull終端文字列互換です。Windows APIはCやC++で書かれていて、たいていがnull終端文字列なので、ネイティブ側がUTF-16 (`wchar_t*`)を使っている限り、実は、C#側から変換なしで文字列を渡すことができます。

変換なしでというか、ポインターがそのまま渡ります。例えば、以下のようなネイティブ コードがあったとします。受け取った文字列をすべて「a」の文字で上書きしてしまう関数です。

```cpp
extern "C"
{
    // UTF-16 null終端文字列
    __declspec(dllexport) void __stdcall FillA16(wchar_t* str)
    {
        for (auto p = str; *p; p++)
        {
            *p = L'a';
        }
    }

    // ANSI null終端文字列
    __declspec(dllexport) void __stdcall FillA8(char* str)
    {
        for (auto p = str; *p; p++)
        {
            *p = 'a';
        }
    }
}
```

これを呼び出すC#コードは以下のようになります。

```csharp
using System;
using System.Runtime.InteropServices;

class Program
{
    // 対 UTF-16。無変換で(ポインター渡しで)呼び出せる。
    // CharSetで指定している「Unicode」はUTF-16のこと。
    [DllImport("Win32Dll.dll", CharSet = CharSet.Unicode)]
    extern static void FillA16(string s);

    // 対 ASCII。変換が必要。
    [DllImport("Win32Dll.dll", CharSet = CharSet.Ansi)]
    extern static void FillA8(string s);

    public static void Main()
    {
        Console.WriteLine(GetValue());

        // 変換が必要な方。
        // コピーが書き換わるだけなので、s1 には影響なし。
        var s1 = "awsedrftgyhu";
        FillA8(s1);
        Console.WriteLine(s1); // awsedrftgyhu

        // ポインターで渡る方。
        // s2 はネイティブ コード側での書き換えの影響を受ける。
        var s2 = "awsedrftgyhu";
        FillA16(s2);
        Console.WriteLine(s2); // aaaaaaaaaaaa
    }
}
```

UTF-16なnull終端文字列に対してC#側から文字列を渡す場合、ポインター渡しになって、ネイティブ コード側での書き換えの影響を受けます。
一方で、相手がANSI文字列(`char*`)の場合には、変換処理が走って、別途メモリが確保されてコピーするので、C++側で書き換えた結果は元の文字列に影響しません。

## 補足: ANSIとUnicode

ちなみに、Windows的には、ANSI、Unicodeというのは以下の意味です。

ANSI:

- 内部的に`char*` (C++の1バイト文字列)
- ANSIと言いつつ、ASCII互換でロケール依存の文字コードのこと
- 要するに、日本語Windowsの場合はShift-JIS

Unicode:

- 内部的に`wchar_t*` (C++の2バイト文字列)
- UTF-16のこと
- 昔(サロゲート ペアが生まれるまで)は、Unicode = UTF-16でした
