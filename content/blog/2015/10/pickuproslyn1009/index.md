---
title: "ピックアップRoslyn 10/9 C# Interactive Window"
source_url: "https://ufcpp.net/blog/2015/10/pickuproslyn1009/"
content_type: "BlogEntry"
published_at: "2015-10-09T02:49:39"
updated_at: "2015-10-09T02:49:39"
tags: []
umbraco_id: 1804
parent_id: 1800
sort_order: 1
aliases: []
---

# ピックアップRoslyn 10/9 C# Interactive Window

## C# Interactive Window

[Visual Studio 2015 Update 1 CTP](http://blogs.msdn.com/b/visualstudio/archive/2015/10/08/visual-studio-2015-update-1-ctp.aspx)が出ました。VS 2015の上書きになるみたいなので試しにくいかもしれませんが、C# な人には待望の C# Interactive Window が追加されています。

C# チーム(.NET コンパイラー チーム)による説明は、ブログじゃなくて GitHub の wiki ページにしたみたいです:

- [Interactive Window](https://github.com/dotnet/roslyn/wiki/Interactive-Window)
  - ちなみに、このページの 2015 SP1 CTP ダウンロードへのリンクは間違っているので、 [VS チームのブログの方](http://blogs.msdn.com/b/visualstudio/archive/2015/10/08/visual-studio-2015-update-1-ctp.aspx) を参照してください

### スクリプト独特の挙動

基本的には通常の C# と同じコードが REPL で動きます。一部、特殊なのは以下のような挙動:

- 非同期コンテキストを持っている
  - await したら Interactive Window 内で待機
  - バックグラウンド実行にはならないし、Interactive Window のスレッドに戻ってくる
- 変数やメソッドは既定(アクセス修飾子を付けない)で public
- 同じ名前の変数を再定義することで、古い定義を隠す(shadowing)
- `IEnumerable` の出力は自動的に中身を表示してくれる

### Interactive Window としての機能

で、かなり高機能な REPL でいろいろ GUI サポートを受けれるみたいです:

- IntelliSense (補完だけじゃなくてコード スニペットとかも)使える
- 'Alt+矢印キー'で入力履歴
  - 'Ctrl+Alt+矢印' で、プレフィックスでの履歴 (途中まで入力して、それにマッチする履歴を出す)
- Interactive Window 内で打った内容を、エディター的に矢印で移動、コピペ可能
- コードのコピペ
  - Interactive Window からのコピーでも、リッチテキストでクリップボードに入って、色付け情報も残る
- 複数行入力したいときは 'Shift+Enter'
-  エディター的に移動した先の行を再実行したいときは Ctrl+Enter
- 'Ctrl+A' の挙動は、1回で入力行の全選択、2回でInteractive Window内全選択

### ディレクティブ

スクリプト限定のディレクティブ(`#define` とか、`#` から始まるやつ)がいくつか:

- `#r`: DLL(パス指定) や NuGet パッケージ(パッケージ名指定)の参照
- `#load`: ファイルを指定してスクリプト読み込み
  - コピペでそこに張り付けたのと同じ挙動。別コンテキスト実行にはならない
- `#clear`, `#cls`: Interactive Window 内の文字列全消去
  - 文字だけ。コンテキスト(これまでに定義した変数とかメソッドとか)は残る
- `#help`: ヘルプ表示
- `#reset`: コンテキスト含めて全消去

### csi (コマンドライン ツール)

Visual Studio の外、コマンドプロンプト内でも C# REPL を使えるみたい。

Developer Command Prompt for VS2015 (Visual Studio 付属の、各種環境変数設定済みの cmd)中で、`csi` ってコマンドを打てば C# REPL が起動するとのこと。

ちなみに、ちょっと触ってみた感じの個人的な感想では、コマンドライン ツールとして使うと、IntelliSense が効かないとか、大文字小文字の区別(普段 IntelliSense 頼りで打ってる)とか、`;` 付けないと行けないの(普通のC#だと違和感ないけど、REPL だと結構な違和感)とか、結構きつそうな感じでした…


## コード生成拡張

[[Proposal] enable code generating extensions to the compiler #5561](https://github.com/dotnet/roslyn/issues/5561)

`CodeAnalyzer`、`CodeFix`みたいな感じで、`CodeInjector` みたいなクラスを用意するみたい。これを継承して独自のC#→C#コード生成(いわゆるinjection、既存メンバーに機能を「注入」)を作るようなものになりそう。

先日紹介した [supersedes](https://github.com/dotnet/roslyn/issues/5292) 機能との組み合わせでいろいろできそう。

## レコード型やパターン マッチングの現状

[Notes on Records and Pattern Matching for 2015-10-07 design review #5757](https://github.com/dotnet/roslyn/issues/5757)

しばらくドキュメント化されてなかったレコード型やパターン マッチングに関する現状説明。

レコード型とパターン マッチングは、C# 6.0 の時にちょっと作り掛けてた(6.0 リリース近くになって急に出てきた話なのでさすがに間に合わず、7.0 に持ち越しすることになった)ものがあったわけですが、そこから現状までの差分で書かれています。
