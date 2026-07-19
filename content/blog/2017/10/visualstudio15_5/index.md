---
title: "Visual Studio 15.5 Preview"
source_url: "https://ufcpp.net/blog/2017/10/visualstudio15_5/"
content_type: "BlogEntry"
published_at: "2017-10-12T23:09:59"
updated_at: "2017-10-13T10:32:03"
tags: []
umbraco_id: 2088
parent_id: 2084
sort_order: 2
aliases: []
---

# Visual Studio 15.5 Preview

こないだ [Visual Studio 2017 Version 15.4](https://blogs.msdn.microsoft.com/visualstudio/2017/10/10/visual-studio-2017-version-15-4-released/)の正式リリースが出たところですが。
(主にUWP/Windows 10 Fall Creators Update絡みだったので個人的にはバグフィックス以外そこまで恩恵なし。)

翌日にもう[Visual Studio 2017 Version 15.5のプレビュー版]が。

「Stepping Back」デバッグ(1つ前のブレークポイントに状態を戻せる機能)とか割かし素敵そうな。

それはそれとして、15.5の告知ブログにはどこにも書かれていませんが、C# 7.2が含まれています。
大々的に書かない辺り、やっぱり[自信ない](../pickuproslyn1008/index.md)のかな…
まあ、こっそり出したところで、[Roslynリポジトリ](https://github.com/dotnet/roslyn)を見ていれば15.5で出すことだいぶ前からわかっていますが。

## C# 7.2

ということで、C# 7.2。

[C# 7.2で追加予定](https://github.com/dotnet/csharplang/milestone/6)となっている機能を一通り手元で試してみましたが、
「[ref local reassignment](https://github.com/dotnet/csharplang/issues/933)」以外は実装されていそう。
(※追記: ref local reassignment はそもそも 7.3 に延期されてそう。なので、予定されている機能は全部実装済み。)
あと、[先日報告を出したバグ](../pickuproslyn1008/index.md)の修正は、さすがに今回のバージョンには含まれていませんけども、
15.5の正式リリースまでには入ると思います。

試しに一通り書いてみたコードはうちのサンプル リポジトリに置いてあります。

- [Csharp7_2-1012](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2017/Csharp7_2-1012)

C# 7.1の時と同様に最初は Gist 辺りに書き捨てとこうかと思ったんですが、思ったよりも分量が多く。
結構なコード量だったのでちゃんとソリューションを作って複数のファイルに分けて書くことになったので GitHub リポジトリにコミット。

### `ref`

C# 7.2の当初予定だと「パフォーマンス関連の機能詰め込み」みたいな感じだったんですけど、やっぱ一部はもっと後に伸びました。
で、残ってるのが何かというと、もうほとんどが`ref`がらみ。

- [Span<T>, aka interior pointer, aka stackonly struct](https://github.com/dotnet/csharplang/issues/666)
  - [SpanSafety.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/SpanSafety.cs)
  - [RefStruct.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/RefStruct.cs)
  - [SafeStackalloc.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/SafeStackalloc.cs)
  - [SpanSample.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/SpanSample.cs)
- [conditional ref operator](https://github.com/dotnet/csharplang/issues/223)
  - [ConditionalRefOperator.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/ConditionalRefOperator.cs)
- [ref extension methods on structs](https://github.com/dotnet/csharplang/issues/186)
- [Readonly ref](https://github.com/dotnet/csharplang/issues/38)
  - [ReadOnlyStruct.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/ReadOnlyStruct.cs)
  - [RefExtensionRefOperator.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/RefExtensionRefOperator.cs)

C# 7.0の[参照戻り値・参照ローカル変数](../../../2016/6/cs7refreturns/index.md)の延長にあたる機能です。

(※ readonly structs とかは `ref`と関係なさそうにも見えますが、これがないと `ref` の安全性の保証ができないそうで。)

C# 7.0の参照戻り値の時点で、「9割方の人はおそらく使わない機能」、「残り1割(未満かも)の人が、ライブラリやフレームワークのパフォーマンス改善に使う」、「結果的に、全てのC#ユーザーがパフォーマンス改善の恩恵を受ける」的な機能なわけですが。
C# 7.2はもう、このバージョン全体がそんな感じ。

### 他

一応他にも新機能があるんですが、ものすっごい小粒です。

- [allow digit separator after 0b or 0x](https://github.com/dotnet/csharplang/issues/65)
  - [DigitSeparator.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/DigitSeparator.cs)
- [Non-trailing named arguments](https://github.com/dotnet/csharplang/issues/570)
  - [NonTrailingNamedArguments.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/NonTrailingNamedArguments.cs)
- [Private protected](https://github.com/dotnet/csharplang/milestone/6)
  - [ClassLibrary1/PrivateProtected.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ClassLibrary1/PrivateProtected.cs)
  - [ConsoleApp1/PrivateProtected.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-1012/ConsoleApp1/PrivateProtected.cs)
