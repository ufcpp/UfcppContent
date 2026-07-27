---
title: "Code-Awareなライブラリ"
source_url: "https://ufcpp.net/study/csharp/package/pkgcodeawarelibrary/"
content_type: "Article"
published_at: "2015-05-13T00:00:00"
updated_at: "2021-12-12T21:34:07"
tags:
  - "Ver. 6.0"
umbraco_id: 1718
parent_id: 1717
sort_order: 2
aliases: []
---

# Code-Awareなライブラリ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version6">Ver. 6</h5>

(C# 6の言語的な機能ではありませんが、C# 6と同世代(Visual Studio 2015世代)の技術なので、利用できるのはC# 6以降になります。)

[.NET Compiler Platform](../misc/misc_roslyn.md#compiler-platform)を使うことで、
コードを解析してその問題点の指摘やその修正方法を提供できる、コード アナライザーというものを作れます。

ライブラリを作る際、そのライブラリに特化したコード アナライザーを作って同梱して配ることで、
「ライブラリ自身が、ライブラリ利用側のコードを理解して、問題点の指摘やその修正方法まで提供」ということが実現できます。

こういう、「利用側のコードを理解するライブラリ」を、Code-Aware なライブラリと呼びます。

##### <a id="sec-generated-title-2"></a>サンプル

[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/DevEnv/CodeAwareLibrarySample](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/DevEnv/CodeAwareLibrarySample)

## <a id="sec-generated-title-3"></a>コード アナライザー

(アナライザー自体の説明はたぶんそのうち別ページに分ける。分けたあかつきには: 
1. C#はリアルタイムに、書いてるそばからエラー・警告出してもらえる言語。
2. Visual Studio以外もRoslyn対応そのうちするはず。Xamarinは近々？VSチームがOmniSharpにも協力してたはず。)

[C# 6 の新機能](../cheatsheet/ap_ver6.md)の冒頭で触れていますが、しばらくの間、C#コンパイラーの再実装に工数を割いていて、C# 6は言語機能としては大きなものがあまりありません。言語的に大きな機能追加がなかった代わりと言ってはなんですが、新しくなったコンパイラーには別の「売り」があります。それが、コード アナライザーの作成です。

コード アナライザーは、「コードを解析するもの」という名前通り、C#ソースコードを解析して、問題点を指摘したり、その修正機能を提供するものです。こういう解析機能を、誰でも自由に作れるようになりました。

その結果、C#コンパイラー自身では提供しにくいような、以下の様なコード解析が実現できます。

1. 経験則的な警告・エラーの提示
  - 機械的な判断だと間違う可能性のあるものは、コンパイラー機能としては実装しにくい
2. チーム内でのローカル ルールの解析
  - ルールはチームごとに違ったりするので、コンパイラーにはわからない
3. ライブラリ固有の事情
  - 特定のライブラリのためだけのコード解析は、標準では提供すべきではない

本稿の主題は3つ目の「ライブラリ固有の事情」になります。

## <a id="sec-generated-title-4"></a> <a id="libary-specific"></a>ライブラリ固有の事情の例

簡単な例を上げてみましょう。以下のようなコードをライブラリ化することを考えます。

```csharp
namespace FluentArithmetic
{
    public static class FluentExtensions
    {
        public static int Add(this int x, int y) => x + y;
        public static int Sub(this int x, int y) => x - y;
        public static int Mul(this int x, int y) => x * y;
        public static int Div(this int x, int y) => x / y;
    }
}
```

`a + b - c` などと書く代わりに、`a.Add(b).Sub(c)` と書けるようにするコードです。特に使い道はないんですが、「ライブラリ固有の事情」の説明にはなかなか手頃だと思います。

単純な四則演算なわけですから、いくつかの経験則が働くでしょう。ここでは2つほど挙げると、以下のようなものがあります。

- `1.Div(0)` というような、「0割り」は実行してみるまでもなくエラーになることがわかっている
- `1.Add(2)` というような、[リテラル](../start/st_variable.md#literal)同士の演算は、`3` などに置き換えて最適化できる

「`Div(0)` と書いたら常にエラーを起こす」というのは完全にこのライブラリ固有の事情になります。このライブラリのこの`Div`メソッドに限り、常に実行時エラーを起こすんだからもうコンパイル時にエラーにしたい。しかし、コンパイラーは、そんな特定のライブラリだけの特殊対応なんてできません。こんな時こそコード アナライザーの出番です。

## <a id="sec-generated-title-5"></a> <a id="libary-specific-analyzer"></a>ライブラリ固有のコード アナライザーの例

ということで、以下のような機能を提供するコード アナライザーを考えます。

- `Div(0)` を見たら問答無用でコンパイル エラーにする
- `1.Add(2)` とかのリテラル同士の演算は最適化できる旨、情報を出す
  - この最適化を自動的に行う「クイック アクション」を提供する

実際に実装してみた感じが以下の通り。

まず、`Div(0)`エラー。ちゃんとコンパイル エラーです。直さないとコンパイルできません。

![Div(0) エラー][1]

次が、`1.Add(2)` は最適化できるという情報。この場合、別にエラーにもならないし、警告も出ません。

![リテラル同士の演算][2]

エラーにも警告にもなりませんが、該当箇所には電球(lightbulb)マークがついています。この電球をクリックする(もしくは、`Ctrl+.` ショートカットキーを押す)と、コードの自動修正(code fix)が働きます。

![電球マークをクリック][3]

実行すると以下のようになるはずです。

![自動修正の結果][4]

ちなみに、Visual Studio的には、この「電球マークを起点として何か処理を掛ける」操作を「クイック アクション」といいます。

## <a id="sec-generated-title-6"></a> <a id="code-aware"></a>Code-Aware ライブラリ

[サンプル](https://github.com/ufcpp/UfcppSample/tree/master/DevEnv/CodeAwareLibrarySample)は実際にこの例のライブラリとそれ用コード アナライザーを実装したものです。

- FluentArithmetic.dll: ライブラリ本体。`a.Add(b).Sub(c)` という類の書き方で整数の四則演算をするためのライブラリ。
- FluentArithmeticAnalyzer.dll: FluentArithmetic 専用のコード アナライザー。

ライブラリ専用なわけですから、コード アナライザーはライブラリ本体とセットで配布すべきでしょう。
そうすることで、ライブラリ自身がライブラリ利用側のコードを見て、問題点の指摘やその修正方法まで提供できる状態になります。こういう状態のライブラリを、「利用側のコードまで理解する」、「利用側のコードを意識している」という意味で「Code-Aware」と言います。

要するに、以下の様なパッケージを作って配布します。

- ライブラリと、それ用コード アナライザーを同梱する
- パッケージ インストール時に走るスクリプトで、コード アナライザーへの参照を足す

（書きかけ）以下、作成手順の説明

- テンプレート通りに「Analyzer with Code Fix (NuGet + VSIX)」を作れば大体それっぽいものできてる
  - nuspec とか、install.ps1 最初からある
- これに、ライブラリ本体の参照を足す
- 今回の場合、nuspec とか install.ps1 とか、「ビルド後に実行するコマンドライン」でNuGetコマンドを呼ぶ処理は別プロジェクトに分離・移動
- コード アナライザーの動作確認は Test プロジェクトの実行で
  - Vsix プロジェクトを実行すると別 Visual Studio が立ち上がってデバッグ実行できるんだけど、重いし、エラーが出た時悲惨だし
- 一応 NuGet Gallary においてある: https://www.nuget.org/packages/FluentArithmetic/

（書きかけ）どういう場合に有効かの例をもう何例か列挙

- JSONパーサー ライブラリ同梱で、文字列リテラル中のJSONを解析
- 正規表現ライブラリ同梱で、`Regex("ここの解析")`

  [1]: ../../../../assets/media/1007/codeawarelibrary1.png
  [2]: ../../../../assets/media/1008/codeawarelibrary2.png
  [3]: ../../../../assets/media/1009/codeawarelibrary3.png
  [4]: ../../../../assets/media/1010/codeawarelibrary4.png
