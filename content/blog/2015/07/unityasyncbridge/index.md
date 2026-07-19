---
title: "Unity(ゲームエンジン)上で async/await"
source_url: "https://ufcpp.net/blog/2015/07/unityasyncbridge/"
content_type: "BlogEntry"
published_at: "2015-07-06T12:50:29"
updated_at: "2015-07-06T12:50:29"
tags: []
umbraco_id: 1757
parent_id: 1756
sort_order: 0
aliases: []
---

# Unity(ゲームエンジン)上で async/await

async/await が使えないC#とかちょっと。

で、最近、Unity上でasync/awaitを使えるかもしれないという希望が見えてみたので、現状報告。

## 背景

主にUnityの問題点。数年来文句を言い続けて、一向に解決してもらえていない…

### Unity上のC#は3.0

Unityが使っているC#は、結構古めの[Mono](http://www.mono-project.com/) (確か 2.8 系)で、普段普通に最新のC#を使っている人の感覚では、結構きつい制限がかかった状態にあります。

- C# 3.0相当
  - 引数の規定値・名前付き引数(C# 4.0から)だけ使えたりするものの、structやenumの値を規定値に指定できなかったり
- .NET 3.5相当(WPFとか除く)
  - `System.Linq`は使える

これで何がつらいかというと、async/awaitが使えないのが一番つらい。スマホゲームって非同期処理の塊になるわけですが、そこでawaitが使えない3.0時代の書き方をするのとかもはや苦行で。

一応、Unity自体がコルーチンの仕組みを持っていて、それを使うということになっているものの、コルーチンだと、

- 戻り値を返す仕組みを標準では持っていない
- `UnityEngine.dll` への依存が必要
  - しかもこのライブラリ、Unityの外から参照できない(実行時エラーを起こす)
- `UnityEngine.GameObject` クラスが債務持ちすぎで気持ち悪い

などのつらみがあります。

### DLL化で多少はマシに

C# の最新機能が使えない理由は、大まかに2つあります。

- C#コンパイラーが古い
- 使える.NET標準ライブラリの制限がきつい

前者の制限はまあなんとか回避する方法があったりします。C#の機能の多くは古い.NET Framework上でも動きます。
顕著なのは[C# 6.0](../../../../study/csharp/cheatsheet/ap_ver6.md)の新機能ですけども、ほとんどの機能が .NET Framework 2.0上でも動かせます(参考: [C#の言語バージョンと.NET Framework](../../../../study/csharp/cheatsheet/listfxlangversion.md#v6))。

UnityはDLLの参照もできるので、コアロジックをUnityプロジェクトから完全に分離して、DLL化してから、Unityプロジェクト上にコピーすればコンパイラーの制限はかかりません。普通にC# 6.0が使えます。

![コアロジックを分離・DLL化][1]

うちのプロジェクトの場合、Unityの外とのコード共有(サーバー側ロジックとか、ゲームデータの編集ツールとか)を考えて最初から割かし徹底してプロジェクトを分けているので、かなりの部分でC# 6.0が使えています。

#### DLLのコピーを楽に

ちなみに、いくつか面倒はありますが、対処用のライブラリとかUnityエディター拡張を用意しています。

- DLLのコピーがめんどくさい → [コピー用の仕組み(プロジェクト設定+スクリプト)のNuGet パッケージ](https://github.com/ufcpp/UnityTools/tree/master/CopyDllsAfterBuild)書いて公開してある
- DLL化したらその中身のソースコードが見えない → Unityエディター拡張で、DLL参照を、元のC#プロジェクト参照に置き替えるスクリプトを書いてる

### ライブラリの制限

問題はライブラリに依存したC#機能の場合。具体的には

- [dynamic](../../../../study/csharp/dynamic/sp4_dynamic.md)
- [async/await](../../../../study/csharp/async/sp5_async.md)
- [Caller Info](../../../../study/csharp/cheatsheet/ap_ver5.md#CallerInfo)
- [FormattableString](../../../../study/csharp/start/st_string.md#FormatableString)

の辺りです。まあ、async/await以外はなくても我慢ができますが…

で、[C#の言語バージョンと.NET Framework](../../../../study/csharp/cheatsheet/listfxlangversion.md#v6)でちょっと書いていますが、足りない分は自前で同名・同名前空間・同機能のクラスを実装してしまえば、実は動かすことができます。

Monoのクラスライブラリ部分はMITライセンスですし、今なら、マイクロソフトによる実装もMITライセンスで徐々にオープンソース化されて行ってる最中です([corefx](https://github.com/dotnet/corefx))。この辺りから移植すれば、Unityの古い環境でも、一応は最新機能を使えるはずだったりはします。

### AOT制限

※ただし、iOSは除く。

でした。

iOSの場合、.NETやJavaのような仮想マシンコード実行が認められていないので、AOT(Ahead Of Time)というコンパイル方法で、事前にネイティブ コード化してアプリ パッケージ化します。こいつが曲者というか、古いバージョンのMonoでは制限がきつくて、いろいろなコードが実行時エラーになって困ります。

日本語でまとまってるのだとノイエさんとこの記事: [Unity + iOSのAOTでの例外の発生パターンと対処法](http://neue.cc/2014/07/01_474.html)

絶望的なのが`Interlocked.CompareExchange<T>`を使えないことでして、async/awaitの中核たる`Task`クラスがこれを多用しています。結局、Unity上でasync/awaitを使う道は閉ざされていました。

## これまでの回避策

で、async/awaitが使えないなら。

### IteratorTasks

うちで作っちゃったのが`Task`クラスもどき。

- [IteratorTasks](https://github.com/OrangeCube/IteratorTasks)

Unityのコルーチンと同じく、yield returnベースで非同期処理をするためのライブラリです。コルーチンと違って、

- Unityプロジェクトの外で使える
- 戻り値持ってる
- .NET 4 以降標準の`Task`クラスとシグネチャそろえてある

という辺りが利点。

とはいえ、「awaitの代わりにyield returnを使う」という辺りが所詮「もどき」でしかなく。

あと、「いくらなんでもそのうちUnityもC# 5.0に対応するだろう」とか高を括っていたので特に表立ったアピールはしていなかったりします。気が付けば何年これ使ってるんだろう…

というか、もう何度だって言いますが、[「標準ライブラリの互換ライブラリなんてものは超バッド ノウハウ」「おかげさまで安定はしたけども、それは恥だと思っている」](http://www.slideshare.net/ufcpp/orange-cube-20153/18)。

### UniRx

で、もう1個は今流行りのRx。

- [UniRx](https://github.com/neuecc/UniRx)

これは、IteratorTasksみたいな「もどき」移植じゃなくて、結構そのまま[Rx](https://msdn.microsoft.com/en-us/data/gg577609.aspx)の移植。

async/awaitと違って、Rx的な非同期処理ならC# 3.0の範囲で書けるので、IteratorTasksと違って「無茶」をする必要がないのが利点。

あと、Rxはイベント処理にも使えます(というか、C# 5.0では単発の非同期処理は`Task`とasync/awaitを使って書いちゃうので、むしろイベント処理が主役というか)。

## それでもasync/await使いたい

そして最近ふと気づいたことがいくつか。

- `Task`クラス関連全体じゃなくて、async/awaitに必要な最低限の実装ならUnityの制限に引っかかりにくいんじゃないか
- [IL2CPP](http://blogs.unity3d.com/jp/2015/05/06/an-introduction-to-ilcpp-internals/)に置き変わったらコンパイラーも差し替えれるんじゃないか

### async/awaitに必要な最低限の実装

まあ、非同期処理自体には、標準の`Task`クラスがなくても[IteratorTasks](https://github.com/OrangeCube/IteratorTasks)とか[UniRx](https://github.com/neuecc/UniRx)を持っているわけですから、別にそこから移植しないでもいいんじゃないかと。

async/awaitを実行するためには、`IAsyncMethodBuilder`インターフェイスをはじめとするいくつかの型の実装が必要なんですが、実のところ、`TaskCompletionSource<TResult>`クラスの機能くらいしか使っていなくて、ここだけ自作すれば、フル機能の`Task`要らないんですよね。

先週、その`IAsyncMethodBuilder`の[.NET 3.5向けバックポーティング](https://github.com/rackerlabs/dotnet-threading)をしている人を見かけたのがきっかけなんですけども。これのコードをちょっと眺めていたら、`IAsyncMethodBuilder`等の実装は意外とUnityの制限に引っかかりそうなコードが少なくて。フル機能の`Task`実装さえ避ければ案外動くんじゃないかと。

#### MinimumAsyncBridge

ということで試しに作ってみました。

最初は、[IteratorTasks](https://github.com/OrangeCube/IteratorTasks)とか[UniRx](https://github.com/neuecc/UniRx)に直接手を加えるつもりで実装してしまったんですが。

- [IteratorTasks.Csharp5](https://github.com/OrangeCube/IteratorTasks/tree/master/IteratorTasks.Csharp5)
- [AwaitableUniRx](https://github.com/ufcpp/AwaitableUniRx)

よくよく考えてみたら、`TaskCompletionSource<TResult>`だけなら独立して実装した方がいいんじゃないかと思いなおして、作りなおしたのがこちら。

- [MinimumAsyncBridge](https://github.com/ufcpp/MinimumAsyncBridge)

まだ作ったばっかりで試験運用が足りてないのと、後述する「コンパイラー差し替え」のやり方も試してみたいのとで、今後こいつをどうするかはちょっと不透明ですが…

とりあえず今のところそれっぽくは動いています。

※あくまで、「Unityプロジェクトから分離、DLL化して使う」に限ってasync/awaitが使えます。

### コンパイラー差し替え

「Unityプロジェクトから分離、DLL化して使う」という制限が、うちはまあ最初からDLL分離してるので問題ないんですけども、一般には面倒事だろうから[「需要どのくらいあるかなぁ」とつぶやいてみた](https://twitter.com/ufcpp/status/616922280967344128)のが先週末。

そしたら、「[コンパイラーの差し替えして、Unityプロジェクト側でもC# 5.0/6.0使えるよ](https://twitter.com/movra_jr/status/616935458136846336)」などと教えていただきまして。

なるほど。その手が一応あるのか…

ちなみに、これ、「[IL2CPP](http://blogs.unity3d.com/jp/2015/05/06/an-introduction-to-ilcpp-internals/)がまともに動くなら」という前提がかかります。IL2CPPは、公称では「任意のILコードを実行できる」となっているので、コンパイラーを差し替えてもちゃんと動くはず。

IL2CPPさえまともなら… (ちなみに、今開発中のプロジェクトはいまだIL2CPPで動かず。)

#### Unity C# 5.0 and 6.0 Integration

その「コンパイラー差し替え」も紹介しておきます。

- [Unity C# 5.0 and 6.0 Integration](https://bitbucket.org/alexzzzz/unity-c-5.0-and-6.0-integration/src)

このリポジトリ自体にはコンパイラーの差し替えがらみのコードだけが入っています。async/awaitを動かすためには別途、

- [AsyncBridge](https://github.com/OmerMor/AsyncBridge)
- [Task Parallel Library for .NET 3.5](https://www.nuget.org/packages/TaskParallelLibrary/)

が必要です。

コンパイラー差し替えなのでそれなりに手順が要るんですが…

- このリポジトリ中の`cmcs.exe`をビルドして、Unityインストール フォルダーの`\Unity\Editor\Data\Mono\lib\mono\2.0`にコピーする
- Unityプロジェクトの設定で、`Project Settings/Player/API Compatibility Level`を`.NET 2.0`にする
- [Mono 4.0.0](http://www.mono-project.com/docs/about-mono/releases/4.0.0/) の C# 6.0コンパイラーか、[Roslyn](https://github.com/dotnet/roslyn)のコンパイラーを、Unityプロジェクト内にコピーする

「手順を踏むのが面倒」、「個人のプロジェクトじゃなくてUnityが公式にやってくれないと不安」、「IL2CPPほんとに大丈夫なの？」という懸念はあるものの、Unityプロジェクト内でもasync/awaitが使えるようになるのは魅力的だし… という感じで、迷いつつも導入の検討中。

## まとめ

主に、Monoのバージョンが古い(特にiOS向けのAOTコンパイル)のせいで、Unityはいろいろと制限がきついです。特にきついのがasync/awaitが使えないことですが、最近、Unity上でasync/await使える希望が見えてきました。

- [.NET 3.5向けバックポーティング](https://github.com/rackerlabs/dotnet-threading)の実装を見てみたら、意外とUnityの制限避けれるかもしれない雰囲気
- [IL2CPP](http://blogs.unity3d.com/jp/2015/05/06/an-introduction-to-ilcpp-internals/)がまともになりさえすれば、制限がだいぶ緩和される
- そしたらコンパイラーも差し替えできるはず

ということで、以下のものを紹介。

- [MinimumAsyncBridge](https://github.com/ufcpp/MinimumAsyncBridge): 非同期処理自体には[UniRx](https://github.com/neuecc/UniRx)などを使う前提で、async/awaitに必要なライブラリの最低ラインの実装
- [Unity C# 5.0 and 6.0 Integration](https://bitbucket.org/alexzzzz/unity-c-5.0-and-6.0-integration/src): Unityの使っているコンパイラーを差し替えて、Unityプロジェクト内でC# 5.0/6.0を使えるようにするもの

  [1]: ../../../../../assets/media/1021/separatedlls.png
