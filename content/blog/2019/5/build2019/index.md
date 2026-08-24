---
title: ".NET 5、Visual Studio 16.1 Preview 3"
source_url: "https://ufcpp.net/blog/2019/5/build2019/"
content_type: "BlogEntry"
published_at: "2019-05-07T20:32:59"
updated_at: "2019-05-07T20:37:39"
tags: []
umbraco_id: 2242
parent_id: 2241
sort_order: 0
aliases: []
---

# .NET 5、Visual Studio 16.1 Preview 3

今年の [build](https://www.microsoft.com/en-us/build)、思ってたよりも .NET がらみが盛沢山…

[Windows Terminal](https://www.publickey1.jp/blog/19/windows_terminalpowershellsshmicrosoft_build_2019.html)とか[Visual Studio Online](https://www.publickey1.jp/blog/19/webvisual_studio_onlinevscodeintellicodelive_sharemicrosoft_build_2019.html)とかの方がさらにインパクト強そう？
ですけど、 .NET がらみもだいぶ。
まあ、3.0 が今年こそ見えてきましたからね。

- [Introducing .NET 5](https://devblogs.microsoft.com/dotnet/introducing-net-5/)
  - .NET Core 4 は Framework 4.X と紛らわしいから欠番にして、次は「5」
  - 徐々に .NET Core に一本化して、名前も「.NET」に
      - [.NET Framework は 4.8 を最後のバージョンにすると明言](https://devblogs.microsoft.com/dotnet/net-core-is-the-future-of-net/)
      - Mono の機能は徐々に取り込むとのこと
          - 最初の目標は [corefx](https://github.com/dotnet/corefx/)(ライブラリ部分)を[coreclr](https://github.com/dotnet/coreclr/)と Mono VM で99%共有化とかからと言ってるので先は長そう
  - これからは年に1回、毎年11月にメジャー リリースする
      - 2019/9 に .NET Core 3.0
      - 2019/11 に .NET Core 3.1
      - 2020/11 に .NET 5、以降毎年6, 7, 8, ...
          - 5 以降、Long Term Support は偶数番だけ
- [Announcing .NET Core 3.0 Preview 5](https://devblogs.microsoft.com/dotnet/announcing-net-core-3-0-preview-5/)
  - Preview 4 との差分
  - 差分のみだけど今回結構大きそう
  - 令和対応とか混ざっててちょっと受ける
- [Visual Studio 2019 version 16.1 Preview 3](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-version-16-1-preview-3/)
  - [IntelliCode が GA](https://devblogs.microsoft.com/visualstudio/announcing-the-general-availability-of-intellicode-plus-a-sneak-peek/)だって
      - 何を持って GA なんだろう… 「16.1 Preview 3 からはオプション指定なしでデフォルトでオン」の意味？
  - IntelliSense、 using してない名前空間の型も補完候補に出てくるように
      - IntelliCode のおかげで候補が賢くなってるおかげか、候補が多過ぎてつらい問題はそこまでなさそう？
- [https://online.visualstudio.com/](https://online.visualstudio.com/) 発表
  - 今のところ上記 URL は[発表ブログ](https://devblogs.microsoft.com/visualstudio/intelligent-productivity-and-collaboration-from-anywhere/)に転送
  - 将来的には「ブラウザー版 Visual Studio Code」になる予定
      - 今もう、実は [try.dot.net](https://try.dot.net/)とかが [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/client) ベースで動いてたりするので、布石はあった
      - Blazor 自体も「プレビュー」に(早期開発版フェーズは通過)
  - 名前の再利用やめろ… Surface といい docs といい…
      - ALM とか Team Services とか DevOps とか呼ばれてるやつの名称が「Visual Studio Online」だった時期がある

「11月って言うと、確かにホリデーシーズン直前で、アメリカ製品はその時期に出るものが一番安定してるけど」とか、「build で毎年11月とかそんなタイトな公約掲げちゃって大丈夫？」とかは思ったり思わなかったり。

まあ、この辺りは他の人がまとめてるので概要のみにして。

## C# 8.0 in Visual Studio 16.1 Preview 3

最近はすっかり C# 中心の話ばっかりなわけですが。

今回の Preview 3 では、新機能は特に増えていないんですが、ちょこっと修正があります。
[前々から「.NET Core 3.0 依存だから安定してない」って言っていた機能](../../4/vs2019ga/index.md)に、予定通り変更あり。

- 非同期イテレーターに `CancellationToken` を渡せる手段ができました
  - 例を gist にアップロード: [EnumeratorCancellation.cs](https://gist.github.com/ufcpp/ae78b9e06d77a573cd5f2fcdbefb92cd)
  - `EnumeratorCancellation` 属性を付ければいいらしい
  - 後述しますがちょこっと挙動変更される予定がすでにあり
- Index/Range がらみのコード生成結果変更
  - [2月にブログにした](../../2/pickuproslyn0210/index.md)通り
  - 変更内容に関する提案ドキュメント: [Index and Range Changes](https://github.com/dotnet/csharplang/blob/master/proposals/index-range-changes.md)
      - 旧仕様: コレクション側が `Index`/`Range` 構造体を受け付けるオーバーロードを用意
      - 新仕様: C# コンパイラーが `Index.GetOffset` を呼んで `int` に変換

## ピックアップ Roslyn 5/7

で、3日ほど前に1個、C# の Design Notes のアップロードがありました。

- [Added: LDM Notes for April 29, 2019 #2485](https://github.com/dotnet/csharplang/issues/2485)

### base(T)

一昨日、[インターフェイスのデフォルト実装](../../../../study/csharp/oop/oo_interface.md#dim)の話を書いたところなんですよ。
その中には [`base(T)` アクセスの話](../../../../study/csharp/oop/oo_interface.md#multiple-inheritance)もあります。
これを書いてから上記の Desing Notes に気づいたわけですが。

「`base(T)`、C# 8.0 からは外して、ランタイムのサポート込みで C# 9.0 に回したい」ですって。

書き直さなきゃ…

### 非同期イテレーターのキャンセル

この話は前述の[gist に上げた例](https://gist.github.com/ufcpp/ae78b9e06d77a573cd5f2fcdbefb92cd)にも書いてあるんですけども。
以下のような非同期イテレーターを書いた場合、

```csharp {title="非同期イテレーター"}
async IAsyncEnumerable<int> X([EnumeratorCancellation]CancellationToken ct = default)
```

`CancellationToken` は、`X(ct)` というのと、`X().WithCancellation(ct)` というの、どちらで書いても最終的に `X` の引数に渡ってきます。

で、両方指定できちゃう。`X(ct1).WithCancellation(ct2)` が合法。
この時どういう挙動をすべきかという話です。

- 現状: `ct2` が優先で上書きされる
- 提案: `ct1` と `ct2` を[リンク](https://docs.microsoft.com/ja-jp/dotnet/api/system.threading.cancellationtokensource.createlinkedtokensource)させたものを新たに作る

`CancellationToken` だけじゃなくて `CancellationTokenSource` にも依存するとか、
コード生成が複雑になるとか、
`CancellationTokenSource` を持つためのフィールドも増えてメモリ的にも優しくないとか、
デメリットもあるんですが、さすがに上書き挙動はまずそう。

### トリアージュ

buildで、.NET Core 3.0 のリリースが今年の7月にRC、9月にGA、11月に3.1と明言されたわけですが。
C# 8.0 はこれに追従する予定です。
要するに C# 8.0 にもスケジュールが切られました。
逆算すると、「C# 8.0 に何を入れて何を入れないか」の決断のタイムリミットが今。

ということで、なんかむっちゃ仕分けされてました。
仕分け結果は [csharplang 内の GitHub Project](https://github.com/dotnet/csharplang/projects/4)にも、
[Roslyn 内の Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)にも反映済み。
多少この2つに不整合があるんで、たぶんまだもうちょっと整理中のはずです。

前々から「8.0 タグが付いてるけど怪しくない？」みたいに思ってたものはやっぱり外れました。
急ぎ、[自分用のタスク リスト](https://github.com/ufcpp/UfcppSample/issues/208)にも反映。

### ローカル関数に対する属性

非同期イテレーター、属性ベースで `CancellationToken` を渡すようになったわけですが。
そこで「ローカル関数にも属性付けれないとまずいじゃない」という話になったみたいです。

ということで、それも実装予定に。
