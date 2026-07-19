---
title: "アナライザーのプロジェクト テンプレートとか dotnet new のテンプレートの話"
source_url: "https://ufcpp.net/blog/2017/12/analyzerconvention/"
content_type: "BlogEntry"
published_at: "2017-12-09T23:34:00"
updated_at: "2017-12-09T23:34:00"
tags: []
umbraco_id: 2117
parent_id: 2112
sort_order: 2
aliases: []
---

# アナライザーのプロジェクト テンプレートとか dotnet new のテンプレートの話

今日は `dotnet new`向けのテンプレートを書いてみたという話。

Visual Studio 標準の "Analyzer  with Code Fix" テンプレートにちょこっと手を入れたやつです。

- [AnalyzerConvention](https://github.com/ufcpp/AnalyzerConvention)

## 背景

### 自作アナライザー

昔、結構[アナライザー](../../../../study/csharp/package/pkgcodeawarelibrary.md)を書いてたんですよ。

- [ValueChangedGanerator](https://github.com/ufcpp/ValueChangedGanerator)
- [RecordConstructorGenerator](https://github.com/ufcpp/RecordConstructorGenerator)
- [LazyMixin](https://github.com/ufcpp/LazyMixin)
- [BitFields](https://github.com/ufcpp/BitFields)

### アナライザーに対する規約ベースのテスト

で、アナライザーって、標準のテンプレートだとテストを書くのが結構面倒なので、
規約ベースで

- 所定のパスに解析対象のソースコードを置く
- 別のあるパスに Code Fix 結果のソースコードを置く

みたいな仕組みを作ったことがあります。
それがこれ。

- [ConventionCodeFixVerifier](https://github.com/ufcpp/ConventionCodeFixVerifier)

NuGet パッケージにしてあって、パッケージ参照すると標準テンプレートのファイルをいくつか上書きします。

### 新 NuGet

ConventionCodeFixVerifier を書いたのはもうだいぶ昔でして、その当時は NuGet も csproj の形式も古かった時代です。

NuGet はまだ packages.confing とか使ってた頃。
NuGet はその後、project.json 形式 → csproj 中に PackageReference タグを書くように変化しまして。
packages.config 時代とは、「プロジェクト中のファイルを上書き」みたいなのをやりたいときの挙動が違います。
ということで、ConventionCodeFixVerifier も更新しないと今はちゃんと動かないんですが…

その一方で、NuGet でプロジェクト中のファイルを上書きするのも嫌な運用だなぁとかずっと思っていました。
むしろ、プロジェクト テンプレートを作るべき事案だろうと。

### SDK-based csproj

あと、Visual Studio 2017で[csprojの形式も新しくなったじゃないですか](../../5/newcsproj/index.md)。
SDK-based csproj とか呼ぶみたいなんですけども。
csproj の中身がかなりすっきりしてたり、この形式を使った場合だけオンになるビルド機能がちらほらあったり。

なんですけども、つい最近まで、標準の "Analyzer  with Code Fix"テンプレートは旧型式のままだったんですよね。
しかも未だに packages.config を使ってるという古臭い作り。

それが嫌すぎて、ここ1年くらいアナライザーを書くのを面倒がって、作りたいものはあったものの放置になったり、
上記の自作アナライザーの更新も止まっていたり。

それが、どうも 15.5 で SDK-based な形式に移行したみたい。
なので僕のやる気も復活した次第です。

### そうだ、プロジェクト テンプレート作ろう

とはいえ、前述の通り、NuGet でプロジェクト中のファイルを上書きするやり方はやめたいわけです。
でも、Visual Studio のプロジェクト テンプレートって何か作るのめんどくさくて。

と、迷っていたところで、「`dotnet` コマンドのやつならマシになってるのでは？」と思って検索した結果、

- [dotnet new のカスタム テンプレート](https://docs.microsoft.com/ja-jp/dotnet/core/tools/custom-templates)

これを読んでみて、「これなら案外行けそうかも」という気分になったので、こっちで作りました。

## 作ったもの

という背景の元作ったのが冒頭の「[AnalyzerConvention](https://github.com/ufcpp/AnalyzerConvention)」。

### 導入方法

以下のコマンドでテンプレートをインストールできます。

    dotnet new -i AnalyzerProject.Convention

`dotnet`コマンドで使えるテンプレート、NuGet パッケージを作ればいいらしく、[nuget.org](https://www.nuget.org/)からのインストールもできます。
`-i` オプションがその「nuget.org とかからパッケージを取って来てインストールする」というオプション。

ちなみに、パッケージ化してnuget.orgに上げたの物:

- [https://www.nuget.org/packages/AnalyzerProject.Convention/](https://www.nuget.org/packages/AnalyzerProject.Convention/)

これをインストールすれば、その後、以下のコマンドでプロジェクトを生成できます。

    dotnet new AnalyzerConvention

### 内容物

作ったテンプレートは、

- Visual Studio 15.5 の標準の"Analyzer  with Code Fix"テンプレート通りにプロジェクト新規作成
- それに[ConventionCodeFixVerifier](https://github.com/ufcpp/ConventionCodeFixVerifier)を上書き
  - ただし、もう V1 (古い規約)対応は切りました
- テスト プロジェクトを [XUnit](https://www.nuget.org/packages/xunit/) に移行

というもの。

標準テンプレートがベースなので、「クラス名を全部大文字にする」っていうクソ Code Fix のままです。

### おまけ: SourceName

この`dotnet`コマンドのテンプレート、[設定ファイル](https://github.com/ufcpp/AnalyzerConvention/blob/master/src/content/.template.config/template.json)の"`sourceName`"のところに書いた文字列を、`new`したときのフォルダー名(もしくは`-n`/`--name`オプションで指定した名前)で置換する仕組みみたいです。

この`sourceName`を何にしようか迷った結果…

Ạṇạḷỵẓẹṛ

に。

Analyzer に見えますけども、よく見ると全文字、下にドットが付いています。
Unicode の「Latin Extended Additional」辺りに入っている文字。
これなら普通のソースコードと混ざらないだろうと。

「混ざらない」って目的は無事果たしたんですけども、ファイル名とかフォルダー名も Unicode になっちゃってて…
nupkgって実体はただのZIPファイルなわけですけども、文字化けしてないかが多少不安…
日本語Windows以外で動かなくなってたりしたらどうしよう…
