---
title: "祝 .NET 5.0 リリース: .NET Core 3.1 からの移行話"
source_url: "https://ufcpp.net/blog/2020/11/net5_0ga/"
content_type: "BlogEntry"
published_at: "2020-11-16T00:29:37"
updated_at: "2020-11-16T00:33:08"
tags: []
umbraco_id: 2317
parent_id: 2316
sort_order: 0
aliases: []
---

# 祝 .NET 5.0 リリース: .NET Core 3.1 からの移行話

祝 .NET 5.0 GA。

- [Announcing .NET 5.0](https://devblogs.microsoft.com/dotnet/announcing-net-5-0/)
- [C# 9.0 on the record](https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/)
- [Visual Studio 2019 v16.8 and v16.9 Preview 1 Release Today](https://devblogs.microsoft.com/visualstudio/visual-studio-2019-v16-8/)
- [Announcing ASP.NET Core in .NET 5](https://devblogs.microsoft.com/aspnet/announcing-asp-net-core-in-net-5/)
- [Visual Studio 2019 16.8 リリースノート](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes#16.8.0)
- [Visual Studio 2019 18.9 Preview 1 リリースノート](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.9.0-pre-1.0)
- [.NET Conf 2019 - Day 1 ライブ配信](https://www.youtube.com/watch?v=mS6ykjdOVRg)

一応注釈なんですが、 .NET は以下のような状態です。

- .NET 5.0 からは単に「.NET」になります
  - .NET Framework, Standard, Core の統合結果です
  - TargetFramework 名、 net5.0 で、 netstandard2.1 と netcoreapp3.1 の後続扱い(後方互換あり)です
- 年次リリース＆偶数バージョンにだけ長期サポート(LTS: Long Term Support)あり
  - 5.0 は長期サポートなし(6.0 が出たら移行を推奨)
      - この状態を指して GA (General Availability) と呼んでる
  - 6.0 は来年同時期にリリース予定
  - LTS は「3年もしくは次の LTS リリース後1年間のうち、いずれかのより長い方」がサポート期間

一応リリースの日の夜に「記念雑談」をしてました(割かし本当に記念だけして、雑談です。話題それまくり)。

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/i02HL8g-D1w" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

## .NET Core 3.1 からの移行

前回、 .NET Core 2.2 から 3.0 の時は ASP.NET に破壊的変更があってそこでコード修正が必須だったりしたんですが。
今回、3.1 から 5.0 の以降では ASP.NET の破壊的変更もなさそうで、自分がかかわっているコードで大きな修正が必要なものはなかったです。

小さい修正はちょっとだけあって、自分のコードでも以下の2点は踏みました:

- WPF (WinForms でも同様のはず)で、csproj の書き方がちょっと変わる
- 警告がアグレッシブに追加される

あと、「コンパイルは通るけど挙動が変わっている可能性がある」という警戒を要する点もあります(自分では今のところは踏んでいない):

- 国際化対応が NLS (Windows が元々持っていたライブラリ)から ICU (Unicode 標準に沿ったオープンな実装)に切り替わった

### Windows 向けプロジェクト

元:

```xml {title="WPF プロジェクト(Windows 向け .NET プロジェクト) 修正前" highlight-text="WindowsDesktop"}
<Project Sdk="Microsoft.NET.Sdk.WindowsDesktop">
 
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFrameworks>netcoreapp3.1</TargetFrameworks>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
 
</Project>
```

後:

```xml {title="WPF プロジェクト(Windows 向け .NET プロジェクト) 修正後" highlight-text="-windows"}
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
    <OutputType>WinExe</OutputType>
    <TargetFramework>net5.0-windows</TargetFramework>
    <UseWPF>true</UseWPF>
  </PropertyGroup>
 
</Project>
```

### 警告がアグレッシブに追加される

C# はこれまで警告の追加も破壊的変更になりうるということで、追加には消極的でした。
今では「仕様の穴だった」と認識されている問題のあるコードでも、
かつて警告なしでコンパイルできてしまっていたものに新たに警告を足すということは避けていました。
(世の中にはコンパイルはできてしまう上でたまたま問題を踏まず動いてしまっていたコードがたくさんあります。)

.NET 5.0 世代ではこの方針に変更があって、以下のようになっています。

- TargetFramework を変えない限りには警告の追加はないものの、net5.0 に上げた場合には警告が出るようにする
-C# 言語バージョン(LangVersion) とは別に「警告バージョン」(AnalysisLevel)を指定できるようにする
- この条件下で、これまでだったら足さなかったようなレベルの警告を大幅に追加する

.NET 5.0 をターゲットにしたいし、C# 9.0 の新機能も使いたいけども警告だけは増やしたくないという人は、以下の2つのオプションを csproj に追加してください。

```xml {title="警告レベルを昔のまま維持するためのオプション"}
<Project Sdk="Microsoft.NET.Sdk">
 
  <PropertyGroup>
    <EnableNETAnalyzers>false</EnableNETAnalyzers>
    <AnalysisLevel>4.0</AnalysisLevel>
  </PropertyGroup>
 
</Project>
```

ちなみに、自分が踏んだ「追加警告」は以下のような警告だけでした(CS8881 の追加)。

```csharp {title=".NET 5.0 にすると増える警告"}
// エラーにならなくなってた条件
// - X と Y が別 DLL (別 csproj)
// - X が参照型だけを含む
public struct X<T>
{
    private string _x;
}

struct Y
{
    X<int> _x;
 
    public Y(bool x)
    {
        // 一定の条件下で、 _x を初期化しなくてもエラーが出なかった。
        // C# 9.0 (AnalysisLevel 5.0) では警告だけは出るようになった。
        if (x) return;
 
        _x = new X<int>();
    }
}
```

### ICU (International Components for Unicode) 化

もしかしたら踏むかもしれない地雷として話題になっているのが、国際化対応の ICU 化です。

- [.NET グローバリゼーションと ICU](https://docs.microsoft.com/ja-jp/dotnet/standard/globalization-localization/globalization-icu?WT.mc_id=DT-MVP-4028921)
- [.NET 5 以降で文字列を比較するときの動作の変更](https://docs.microsoft.com/ja-jp/dotnet/standard/base-types/string-comparison-net-5-plus?WT.mc_id=DT-MVP-4028921)
- [グローバリゼーションに関する破壊的変更](https://docs.microsoft.com/ja-jp/dotnet/core/compatibility/globalization?WT.mc_id=DT-MVP-4028921)
- [Improving the developer experience with regard to default string globalization](https://github.com/dotnet/runtime/issues/43956)

.NET Core では今まで以下のような状態でした。

- Windows 上では NLS (National Language Support) という Windows 組み込みの国際化ライブラリを使っていた
- 非 Windows 環境では ([ICU](http://site.icu-project.org/home))を使っていた
- なので、カルチャー依存の文字列処理などが、Windows とそうでない環境に差があった

.NET 5.0 では、Windows 上でも ICU を使う方針に変わりました。
「ICU を使えるオプション」(opt-in)にするか「デフォルトは ICU で、NLS に戻せるオプション」(opt-out)にするかは迷っていたみたいなんですが、結局は後者、デフォルト動作は ICU になりました。

「同じバージョンの .NET が Windows とその他で挙動が違う」という問題はなくなった一方で、
「Windows 上では .NET Core 3.1 と .NET 5.0 で挙動が違う」ということが一部起こります。

カルチャー依存な API を使わなければ問題ないんですが…
.NET の string は歴史的背景で、一部はカルチャー依存、一部は非依存みたいになっているので注意が必要です。
例えば、[`IndexOf` はカルチャー依存で、`Contains` は非依存](https://github.com/dotnet/runtime/issues/43736)みたいなことがあったりします。

#### ¥ 記号

日本語 Windows がやっている余計なお世話として有名なのが、「いまだに ¥ 記号と `\` (逆スラッシュ)を同一視する」というのがあります。

```csharp {title="ja-jp カルチャーでの破壊的変更"}
using System;
using System.Globalization;
using System.Threading;
 
var s1 = @"\";
var s2 = "¥";
 
var culture = CultureInfo.GetCultureInfo("ja-jp");
Thread.CurrentThread.CurrentCulture = culture;
Console.WriteLine(s1.Contains(s2)); // CurrentCulture 非依存。今までもこれからも False。
Console.WriteLine(s1.IndexOf(s2)); // CurrentCulture 依存。今まで 0。これから -1。
```

昔、逆スラッシュのコード(U+005C)は国ごとに解釈を変えてもいいという扱いになっていて、
日本語では¥(円記号)、韓国語では₩(ウォン記号)に使っていたという時代の名残りです。
というか、日本語 Windows 上ではフォントによってはいまだに U+005C が円記号で表示されますし…

今(Unicode)では、¥ (U+00A5)、₩ (U+20A9)にはちゃんと別コードが割当たっていますが、NLS は今でも `\` (U+005C) と同一視する処理が入っていたりします。

ちなみに、それぞれ日本語カルチャー(ja-jp)、韓国語カルチャー(ko-kr)でだけこの処理が起こります。

#### 改行

ICU では CR LF は「分割不可」らしく、CR LF と LF は別文字扱いになっています。
NLS はこの処理をしていないので、以下のコードは NLS と ICU で結果が変わります。

```csharp {title="改行の扱いの変化"}
using System;
 
var s1 = "\r\n";
var s2 = "\n";
 
Console.WriteLine(s1.IndexOf(s2)); // NLS だと 1。ICU だと -1。
```

#### 対処法

もしこの手の問題を踏んだ場合、対処方法としては2つあります。

- NLS に戻す
- カルチャー依存をやめる

NLS に戻すなら、csproj に以下の設定を追加します。

```xml {title="国際化対応を .NET 5.0 でも NLS でやるためのオプション"}
  <ItemGroup>
    <RuntimeHostConfigurationOption Include="System.Globalization.UseNls" Value="true" />
  </ItemGroup>
```

一方、カルチャー依存するようなメソッドは大体、第2引数にオプション指定できるので、それを `Ordinal` にしてしまえばカルチャー問題は踏まなくなります。

```xml {title="Ordinal 推奨"}
Console.WriteLine(s1.IndexOf(s2)); // CurrentCulture になってるのが問題
Console.WriteLine(s1.IndexOf(s2, StringComparison.CurrentCulture)); // \ と ￥ の問題を踏む
Console.WriteLine(s1.IndexOf(s2, StringComparison.InvariantCulture)); // \r\n と \n の問題を踏む
Console.WriteLine(s1.IndexOf(s2, StringComparison.Ordinal)); // カルチャー依存したくなければこれを指定すればいい
```

「デフォルトが `CurrentCulture` なことが問題」とは認識されていて、
近いうちにこの手の API に対して `StringComparison` を指定していなかった場合に警告を出すようなアナライザーを提供しようかという話になっていたりもします。
