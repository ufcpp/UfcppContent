---
title: "ピックアップ Roslyn 5/14"
source_url: "https://ufcpp.net/blog/2015/5/pickuproslyn20150514/"
content_type: "BlogEntry"
published_at: "2015-05-14T14:49:52"
updated_at: "2015-07-07T19:32:30"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1723
parent_id: 1700
sort_order: 2
aliases: []
---

# ピックアップ Roslyn 5/14

[Build 2015][1]の資料に目を通すので忙しくてサボっていましたが。

まあ、もっとも、C#チーム、Visual Studioチーム的にもRC版が出たばかり、かつ、RTMも近いだろう状況で、GitHub上の動きを見ててもバグ修正で多忙な雰囲気を感じます。

そんな中、いくつかピックアップ。

## Unicode version number

[https://github.com/dotnet/roslyn/issues/2648](https://github.com/dotnet/roslyn/issues/2648)

「C#では、常に実行環境の最新のUnicodeバージョンを利用したいので、仕様書上はUnicodeバージョン番号を消そう」とのこと。

基本的にはUnicodeって追加される一方なので、そのほうがいいですよね。[レアケース][2]もあるにはあったんですが。

## csc2/vbc2 撤廃

[https://github.com/dotnet/roslyn/pull/2711](https://github.com/dotnet/roslyn/pull/2711)

Roslyn製の新コンパイラーは、2種類(C#, VBともに2種類ずつ)ありました。

- csc/vbc.exe: 以前の(Visual Studio 2013までの)コンパイラーと同じような動作のスタンドアローン動作版。
- csc2/vbc2.exe: 複数のプロジェクト間で標準ライブラリなどの読込結果を共有するために、Windowsサービスとして常時動いてるコンパイラーに仕事を投げるもの。

今は、csc/vbc 側が内部的に KeepAlive オプションとかを持っていて、旧 csc2/vbc2 的な動作もできるみたいです。

今の、Visual Studio 2015 RC版で、「"csc2.exe" はコード 1 を伴って終了しました。」とかいう謎エラー メッセージが出てくる問題、そろそろおさらばできそうかも。

## 2進数リテラル、_ 区切りの実装

[https://github.com/dotnet/roslyn/pull/2730](https://github.com/dotnet/roslyn/pull/2730)

そういえば、C# 6.0から漏れてたのよね、忘れてたけども。`0b101001` みたいな2進数リテラルを書ける機能と、`0xff_ff_ff_ff`みたいな、`_` で数値リテラルを区切れる機能。

割りかし実装は簡単な類の機能なわけですが、それでもこんな、C# 6.0 RTMの直前(冒頭で書いたとおり、今はRTMに向けてバグ修正で手一杯のはず)になんで？

と思って調べてみたら、このpull-req主、マイクロソフトにインターンで入って来た学生っぽい。確かに手頃な課題。

## メソッド名に `!` 記号を使わせて

[https://github.com/dotnet/roslyn/issues/2696](https://github.com/dotnet/roslyn/issues/2696)

要するに、Rubyな人がRubyの命名規約習いたいから、`Enlarge!` (記号の`!`までがメソッド名)みたいな識別子を認めてほしいと要求。

まあもちろん、そんな要求そのままは受け入れがたいわけですが。とはいえ、いくつかわからなくもない話の流れが。

- Rubyで末尾に`!`をつけるのは、「引数で受け取ったオブジェクトに対して副作用をかけますよ」というのを示す規約
  - 例えば `X M(X x)`はメソッド内で新しい`X`のインスタンスを作って返すのに対して、`void X!(X x)`はインスタンス`x`を書き換える
  - あくまで命名規約。守られる保証はない
- C#的にはちょっと
  - 守られる保証のない命名規約自体、C#は望まない
  - 語尾に`Pure`であるとか、多少煩雑になるけども、最初から使える文字を使うべきではないか
  - `[Pure]`属性をつけるとか、属性ベースでやるのがC#っぽいのではないか
- ただ、属性ベースだと…
  - 利用側、ぱっと見で区別が付かない
  - 確かに副作用を起こすかどうかはぱっと見で区別ついた方が良さそう
  - 特定の属性が付いている場合に、IDE上での表示(色など)が変わる機能でもあればいいんだけど
- IDE上の表示で区別するとして
  - それって、Visual Studioはともかく、今どきだったらXamarin Studio, Visual Studio Code, OmniSharpなどなど、いろいろ選択肢はあるけども、その全てが対応してくれなきゃ嫌だ

的な。

まあ、副作用起きるというのが目に見えてほしいという点に関してはわかる。

  [1]: http://channel9.msdn.com/events/build/2015
  [2]: ../../../../study/csharp/start/misc_unicode.md#katakana-middle-dot
