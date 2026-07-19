---
title: ".csproj + project.json"
source_url: "https://ufcpp.net/blog/2015/12/csprojprojectjson/"
content_type: "BlogEntry"
published_at: "2015-12-09T02:13:21"
updated_at: "2015-12-09T02:13:21"
tags: []
umbraco_id: 1832
parent_id: 1816
sort_order: 5
aliases: []
---

# .csproj + project.json

Visual Studio 2015 Update 1で、`.csproj` + ` project.json` 構成のプロジェクトを、`.xproj` プロジェクトから参照しようとしたら素直にはできなったという話と、自力で何とかしたという話。

## 大雑把に言って

面倒ごとが起きる原因はおおむね、ASP.NET 系の作業が来年初頭に伸びてるのと、UWP 系の作業が今年7月に前倒したのとの合わせ技のせい。

たぶん、ASP.NET 5系/.NET CoreのRTM(来年の第1四半期内という予定)までには治るかなぁ。

## ASP.NET 系

オープンソース化とかクロスプラットフォーム路線を一番推し進めてるのはここ。.NET Coreのランタイム(CoreCLR)を一番欲してるのもここ。

### xproj 形式

ASP.NET 5や、.NET Core向けライブラリ、.NET Core向けコンソール アプリを書く場合、プロジェクトが`.xproj`という新しい形式になっています。

ASP.NET 系ではプロジェクト中のソースコードとかパッケージ管理を簡素化したいみたい。既存の`.csproj` (もしくはVBの場合は`.vbproj`)との違いは以下の表のような感じ。

<table>
<tr>
<td></td>
<th markdown="1">これまで(`.csproj`形式)</th>
<th markdown="1">新方式(`.xproj`形式)</th>
</tr>
<tr>
<th>ファイル管理</th>
<td>`.csproj`ファイルの中に、ビルドに含めたいファイルを全部1行1行書く</td>
<td>特定フォルダー以下のC#ファイル全部がビルド対象。除外したいものだけ`exclude`設定を書く</td>
</tr>
<tr>
<th>ライブラリ参照</th>
<td>プロジェクト参照、DLLの直接参照、NuGetパッケージ参照が分かれてる。特に、NuGetパッケージは`packages.config`という別ファイルに記述</td>
<td>この3つを一元化</td>
</tr>
<tr>
<th>ビルド処理</th>
<td>XAMLとか、ビルド時処理が必要なものを含めたり、汎用的な使い方ができる</td>
<td>基本的にWeb用で、HTMLやJavaScriptみたいにそのままデプロイしたいファイルか、C#ファイルのコンパイルくらい</td>
</tr>
</table>

### project.json

実際のところ、`.xproj`ファイルの中にはVisual Studioに関する設定しか入っていなくて、実際プロジェクトのビルドに必要な情報は全部`project.json`という名前のJSONファイルに入っています。コマンドライン ビルドするなら`.xproj`ファイルは不要。

`project.json`には、以下のような情報が入っています。

- (プロジェクトをNuGetパッケージ化するとき用の)名前、バージョン、作者など
- ターゲット
  - フレームワークのバージョン(.NET 4.5とか)
  - 実行環境(Windows とか)
- 依存関係
  - NuGetパッケージ
  - 他のプロジェクト
  - DLLを直接参照

## NuGet 3

.NET Coreは、Windowsデスクトップ向けの.NET Frameworkと違って、ライブラリの参照を、たとえ標準ライブラリでも、必要な分だけNuGetパッケージとして参照する仕組みになっています。一枚岩リリース/巨大インストーラーの撤廃。パッケージ リリース化。

ということでNuGetの利用が加速すると思われるわけですが、そこで、NuGetの利便性向上が必須。
これまでのNuget(2系、`packages.config`)には以下のような問題がありました。

- 1つのプロジェクトを複数のソリューションから参照するとビルドできなくなる
  - ダウンロードしてきたパッケージをソリューション単位でキャッシュする
  - `.csproj`側にキャッシュしたDLLへの相対パスが入る
  - ソリューションの場所が違うと相対パスが狂う
- 依存関係を芋ずる式に含む
  - 依存先のさらに依存先も、`packages.config`や`.csproj`に情報が追加される
  - 「依存先の依存先」とかも含めたバージョン管理がむちゃくちゃ大変に

例えば、[Rx-Main](https://www.nuget.org/packages/Rx-Main/)を参照したとします。まず、`packages.config`っていうXMLファイルができて、以下のような状態に。

```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
  <package id="Rx-Core" version="2.2.5" targetFramework="net46" />
  <package id="Rx-Interfaces" version="2.2.5" targetFramework="net46" />
  <package id="Rx-Linq" version="2.2.5" targetFramework="net46" />
  <package id="Rx-Main" version="2.2.5" targetFramework="net46" />
  <package id="Rx-PlatformServices" version="2.2.5" targetFramework="net46" />
</packages>
```

Rx-InterfacesとかRx-Linqは、Rx-Mainの依存先です。芋ずる式に全部。

そして、このファイルだけじゃなくて、`.csproj`ファイルにも以下のような行が追加されます。

```xml
    <Reference Include="System.Reactive.Core, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL">
      <HintPath>..\packages\Rx-Core.2.2.5\lib\net45\System.Reactive.Core.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Reference Include="System.Reactive.Interfaces, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL">
      <HintPath>..\packages\Rx-Interfaces.2.2.5\lib\net45\System.Reactive.Interfaces.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Reference Include="System.Reactive.Linq, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL">
      <HintPath>..\packages\Rx-Linq.2.2.5\lib\net45\System.Reactive.Linq.dll</HintPath>
      <Private>True</Private>
    </Reference>
    <Reference Include="System.Reactive.PlatformServices, Version=2.2.5.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL">
      <HintPath>..\packages\Rx-PlatformServices.2.2.5\lib\net45\System.Reactive.PlatformServices.dll</HintPath>
      <Private>True</Private>
    </Reference>
```

そこで、NuGet 3系(7月のVS 2015リリースの頃に3.0、今は3.3)では、パッケージの管理方式を変えました。

- ダウンロードしてきたパッケージはユーザー単位でキャッシュ(ユーザー フォルダー直下に`.nuget`フォルダーを作ってそこで一元管理)
- `.csproj`や`.xproj`はノータッチ、`project.json`にだけ情報を記録
- 直接の依存先だけを記録

例えば、同じくRx-Mainを参照すると、`project.json`ファイルが以下のような状態になります。

```json
{
  "frameworks": {
    "net46": {}
  },
  "runtimes": {
    "win": {},
    "win-anycpu": {}
  },
  "dependencies": {
    "Rx-Main": "2.2.5"
  }
}
```

だいぶすっきりして、管理が楽になりました。

そして、NuGet 3系は、これまでの`packages.config`を止めて、ASP.NET 5に合わせて`project.json`を使うようになりました。

## UWP 系

Universal Windows Platformアプリ。要するにWindows 10向け。

元々は.NET Coreと同じスケジュール感で動いていた雰囲気があるんですが、Windows 10の前倒しに合わせて、UWP系ツールのリリースも今年7月に前倒しになりました。

ライブラリに関しては、UWPと.NET Coreは共通のものを使います(.NET Core向けフレームワーク、略してCoreFX)。
その結果、.NET CoreはRC(7月時点ではベータ)なのに、UWPはUpdate 1(7月時点ではRTM)。Update 1なUWPが、RCなCoreFXに依存しているという状態になっていたりします。

ちなみに、CoreFXの祖先はWindows 8のストア アプリ(かつてメトロと呼ばれていた何か)向け.NETです。Windows 8の時点で、パッケージ リリース化・クロスプラットフォーム化を前提としたライブラリ整理が完了しています。
なので、.NET Core全体はRCといっても、CoreFXの部分は十分こなれてはいます。

### .csproj + project.json

ここで、UWP系ツールの苦悩が。

- 割かしWeb前提なところがある`.xproj`は使えない
- でも、NuGet 3系(`project.json`に移行)は使いたい

ということで、UWP系ツールは`.csproj` + `project.json`というハイブリッド構成になっています。

ここでおさらい。

- `.xproj`にはVisual Studioが必要とする少ない情報しか入っていなくて、プロジェクト関連の設定は全部`project.json`に入ってる
- `.csproj`にはプロジェクト関連の設定が全部入ってる
  - NuGet  2系では、NuGetパッケージ参照情報が`packages.config`と`.csproj`で2重管理になっていた

ということで、UWP系ツールでは、

- `.csproj`からはNuGetパッケージ参照情報を消した
  - NuGet 3系リリースと同時にVisual Studio側にも手が入って、`project.json`側だけ見ればよくなった
- `project.json`にはNuGetパッケージ参照情報だけが入ってる(`.xproj`で使う場合のサブセット)

という状態になっています。

## 任意のプロジェクトでNuGet 3

NuGet 3系は`project.json`を前提にしています。ASP.NET 5系は最初から`project.json`で、NuGet 3系です。UWP系は`.csproj` + `project.json`というハイブリッド構成でNuGet 3系に対応しました。さて、じゃあ、既存の`.csproj`で作っていたライブラリやコンソール アプリ、WPFアプリなどはどうか。

`.csproj` + `project.json`は、公式ドキュメントによれば以下のような状態です。

- 公式にプロジェクト テンプレートがあって、「マイクロソフトがサポートします」と言っているのはUWPだけ
- ただし、手動で`packages.config`を`project.json`に置き換えれば、実は任意の`.csproj`でNuGet 3系を使える

ということで、`packages.config`から`project.json`への移行プログラムを自分で書いたのが以下のものになります。

- [MigrateToProjectJson](https://github.com/ufcpp/UfcppSample/tree/master/Tools/DnxMigration/MigrateToProjectJson)

## .xprojから.csprojを参照

なにぶん`.xproj`は新しい試みなので、まだまだ足りてないところがあります。ASP.NET 5や.NET Core本体がだいぶ安定してきても、Visual Studio上の`.xproj`の挙動が安定するまではRTMにできないんだろうなぁという雰囲気。

その1つが`.xproj`から既存の`.csproj`で作ったライブラリをプロジェクト参照する方法。以下のようなラッパーとなる`project.json`を書いてやる必要があります。

```json
{
  "version": "1.0.0-*",
  "frameworks": {
    "net35": {
      "wrappedProject": "../../ClassLibraryNet35/ClassLibraryNet35.csproj",
      "bin": {
        "assembly": "../../ClassLibraryNet35/obj/{configuration}/ClassLibraryNet35.dll",
        "pdb": "../../ClassLibraryNet35/obj/{configuration}/ClassLibraryNet35.pdb"
      }
    }
  },
  "dependencies": { }
}
```

`.xproj`が出た当初はこいつの手書きが必須でした。Visual Studio 2015がRTMしたころには、一応Visual Studioがこいつを生成してくれるようにはなっています。

ただ、ここでまた問題があって、`.csproj` + `project.json`構成にしてしまった`.csproj`プロジェクトを参照すると挙動が狂います。
上記のようなラッパー`project.json`がまともに作られなくなったり、
`.csproj`で参照している側の`project.json`が変に書き換わってビルドできなくなったり。

公式には、UWP系以外の`.csproj` + `project.json`構成は「できるけどサポート対象外」なので、この問題は現状では「自己責任」扱いではあります。

ということで、結局、ラッパー`project.json`の生成も自前でプログラム書きました。

- [GenerateWrapJson](https://github.com/ufcpp/UfcppSample/tree/master/Tools/DnxMigration/GenerateWrapJson)

## まとめ

ASP.NET 5系は来年第1四半期にRTM予定、UWP系は今年7月にRTM済み。そんな中、ASP.NET 5系とUWP系で共通で使っている部分が結構カオス。

それ以外のタイプのプロジェクトでも、自力で`.csproj` + `project.json`構成にすればNuGet 3系の恩恵にあずかれます。ただし、現状は「できるけどマイクロソフトのサポート対象外、自己責任」。ということで自力で何とかするプログラム書きました。

- [MigrateToProjectJson](https://github.com/ufcpp/UfcppSample/tree/master/Tools/DnxMigration/MigrateToProjectJson)
- [GenerateWrapJson](https://github.com/ufcpp/UfcppSample/tree/master/Tools/DnxMigration/GenerateWrapJson)

おそらく、ASP.NET 5系RTMの来年第1四半期に合わせて、この辺りの挙動をやっと仕上げるんじゃないかなぁという気はしています。それまでの数か月間のつなぎに。
