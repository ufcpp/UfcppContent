---
title: "講習用資料"
source_url: "https://ufcpp.net/study/miscprog/list/training/"
content_type: "Article"
published_at: "2007-06-12T00:00:00"
updated_at: "2019-11-24T11:06:10"
tags: []
umbraco_id: 1545
parent_id: 1542
sort_order: 2
aliases:
  - "/miscprog/list/training/"
  - "/miscprog/training"
  - "/miscprog/training.html"
  - "/study/miscprog/training"
  - "/study/miscprog/training.html"
---

# 講習用資料

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

プログラミングの新人講習のようなものをやる機会があったので、
せっかくなのでそのときの資料をアップ。
背景等は以下の通り。

* 言語は C/C++。

* 開発環境は特に問わず（GUI 開発関係だけは Visual Studio、C# を使用）。

* 基礎的な話は他の人がしているので、ちょっと変わった話をする。


で、実際やったのは以下の4つ。

* 「[C/C++ よくあるバグパターンと対策](#bug)」

* 「[非標準機能、処理系依存機能](#std)」

* 「[オブジェクト指向プログラミング概要](#oop)」

* 「[RAD デモ](#rad)」


まあ、反応は上々かと。
OOP 概要は、ちょっと急ぎ足過ぎて演習問題やるのはきつかった。
概要説明だけにとどめるか、演習もするなら C++ の説明がもっと必要。


## <a id="sec-generated-title-2"></a> <a id="bug"></a>C/C++ よくあるバグパターンと対策

C/C++ でやりがちなミスを列挙してみました。
実例のソースファイル中にコメントで説明を入れています。
C/C++ といいつつ、C 言語で書いています。

1. [= と ==](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/01equal.c)

2. [off-by-one](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/02offbyone.c)

3. [演算子の優先順位](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/03priority.c)

4. [&amp;&amp; と ||](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/04and.c)

5. [if と else の対応関係](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/05elseif.c)

6. [int, double、暗黙の型変換](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/06int.c)

7. [数値の範囲](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/07range.c)

8. [マクロの副作用](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/08define.c)

9. [全角文字の混入](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/09jpspace.c)

10. [配列関係](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/10array.c)

11. [その他諸々](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/bug/11other.c)



## <a id="sec-generated-title-3"></a> <a id="std"></a>非標準機能、処理系依存機能

C/C++ でプログラミングするときに、
「非標準機能を使うな」、
「可能な限り処理系に依存しないように」
というのに異存のある人はあんまりいないと思います。
でもそのためには、どういうのが非標準で、どういうのが処理系に依存しちゃうのか、
ちゃんと把握しておかなければなりません。
 
ということで、非標準機能や処理系依存機能の説明用のプレゼン資料を作りました。

* [非標準機能、処理系依存機能](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/std/presentation.ppt)（PowerPoint 形式）。



## <a id="sec-generated-title-4"></a> <a id="oop"></a>オブジェクト指向プログラミング概要

いまどき、オブジェクト指向的考え方は必須なわけですが、
事細かに説明するほどの時間はないので、
概要のみ。

「[オブジェクト指向](../../csharp/index.md#oop)」で書いてる内容を C++ 化して、
図を多めでプレゼン資料化。

* [オブジェクト指向プログラミング概要](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/oop/presentation.ppt)（PowerPoint 形式）。

* [演習雛形・回答例](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/oop/exercise.zip)（ZIP 形式で圧縮）。



## <a id="sec-generated-title-5"></a> <a id="rad"></a>RAD デモ

RAD （Rapid Application Development）のデモンストレーションをしてみた。
 
目的は、
「最新の話題に触れてもらう」、
「最近は GUI 開発もずいぶん簡単になっているということを知ってもらう」というもの。
 
デモのみで、演習等はなしです。
（受講者全員に Visual Studio を入れて来いというのはいいづらかったのと、
演習とかさせるなら、多分、Visual Studio の使い方の話から事細かにせざるを得なくなりそうだったので、
それを避けた。）
 
デモのみなので、C/C++ という制約からは離れて、C# やら XAML やらを使っています。
（参考： 「[C# によるプログラミング入門](../../csharp/index.md)」、「[クラスライブラリ](../../dotnet/index.md)」。）
まあ、C# の見た目は C++ に近いんで、デモだけならそこまで混乱は生じないかなぁと。
どうせメインは IDE（integrated development environment: 統合開発環境）を使った「ドラッグ＆ドロップ開発」なので。
 
完成までの手順をメモったテキストファイルを用意して、
それを見ながらライブで Windows.Forms を使った GUI プログラムを作りました。
（一部分、あらかじめ作っておいたコードをコピー＆ペーストしています。）
 
また、最近の話題に触れてもらおうということで、
「[Windows Presentation Foundation](../../dotnet/wpf/wpf_abst.md#wpf)」
を使った例も出しました。
（これはライブ開発ではなく、出来合いの物を使って説明。）
 
実際にやったデモは以下の通り。

* RAD 実演（ライブ）
    * ボタンを押したら Hello World （[手順書](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/01helloworld.txt)（txt）、[完成品](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/WindowsApplication1.zip)（ZIP） ）

    * 画像ビューア （[手順書](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/02viewer.txt)（txt）、[完成品](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/WindowsApplication2.zip)（ZIP） ）

    * ラインアート （[手順書](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/03lineart.txt)（txt）、[アイコン](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/App.ico)（ico）、[完成品](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/LineArt.zip)（ZIP） ）



* 最近の動向（WPF 開発）
    * XAML 版ラインアート （[ソース](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/LineArt.xaml)（XAML） ）

    * 3D 描写 （[ソース](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/viewport3d.xaml)（XAML） ）

    * WPF 版 Hello World （[ソース一式](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/ufcpp2000/miscprog/training/rad/XamlApplication.zip)（ZIP） ）
