---
title: "SystemかMicrosoftか、公式ライブラリの名前"
source_url: "https://ufcpp.net/blog/2015/12/systemormicrosoft/"
content_type: "BlogEntry"
published_at: "2015-12-14T12:40:03"
updated_at: "2015-12-14T12:40:03"
tags: []
umbraco_id: 1833
parent_id: 1816
sort_order: 6
aliases: []
---

# SystemかMicrosoftか、公式ライブラリの名前

Windows用として生まれて、クロスプラットフォームに育った.NETの宿命というか。`System`名前空間から始まるライブラリと、`Microsoft`名前空間から始まるライブラリの話。

## System or Microsoft

Systemなんて名前、基本的には標準ライブラリ用なわけですが。

「.NET系開発者はサードパーティ ライブラリにも平然と`System`の名前を付けることがある」みたいな問題もありますが、それは今回は置いておきます。今回は割かし標準に近いところ、マイクロソフトによる公式実装のライブラリの話です。

「.NET = マイクロソフト」(他のOSは「そのマイクロソフト.NETの移植」)だった頃には割かし何でも`System`名前空間にされていたわけですが、オープンソース、コミュニティによる推進、クロスプラットフォームなんかをうたうようになった最近はそれではまずいということで、`Microsoft`名前空間なライブラリが増えています。

## Systemやめました

Windowsにべったりなものが`System`名前空間だったんですよねぇ、長らく。それが徐々に、クロスプラットフォームに耐えうるものが`System`、そうでないものは別の名前空間へと変化。だいたい、2012年頃(Windows 8が出た頃、.NET的には.NET 4.5くらいの頃)が境目。

### GUIフレームワーク

「Windowsにべったり」というとGUIフレームワーク系のもので、以下のような感じ。わかりやすく、「.NETから分離します」「`System`やめます」の流れ。

- [Windows Forms](https://msdn.microsoft.com/ja-jp/library/system.windows.forms.aspx) (System.Windows.Forms.dll)
  - .NET 1.0時代からあるGUI
  - だいたいのクラスが`System.Windows.Forms`名前空間
  - Win32 API感丸出し。
  - それでも、[Monoががんばって移植](http://www.mono-project.com/docs/gui/winforms/)作業したのでなんとか「標準」の体は保ってる
- WPF (WindowsBase.dll, PresentationCore.dll, PresentationFramework.dll)
  - だいたいのクラスが`System.Windows`名前空間
  - .NET 3.0。この時代もまだまだ`System`名前空間
  - アセンブリ名からは`System`が外れる
  - さすがに高機能すぎて移植無理で、完全にWindows用
- [Windows Runtime, Windows.UI](https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.aspx)
  - ついに、GUIフレームワークは.NETから分離
     - WPF的なものをC++ネイティブ実装した上で、.NETから参照しやすいAPI用意したもの
  - .NETの世代的には.NET 4.5時代の出来事
  - 名前空間はさすがに`Windows`

### Web

ASP.NETも`System`名前空間をやめました。まあ「標準」に組み込むにはちょっとでかすぎる感じはあります。

境目は5。ASP.NET MVC 4までは`System.Web.Mvc`名前空間。ASP.NET MVC 5で`Microsoft.AspNet`に。Webフレームワークの1つにすぎないASP.NETが`Web`名前空間を名乗る仰々しさも軽減。

悪名高きSystem.Web.dll (IISにべったり)依存とかも切って、晴れてクロスプラットフォームに。

## Systemやめてませんでした

ここまではわかりやすく、「`System`やめました」なものなのでいいんですが。問題はここから。

### プレビュー版実装から標準に昇格

.NET 4辺りの頃からなんですが、プレビュー版の間は仮に`Microsoft`名前空間で実装しておいて、標準ライブラリに取り込めるとなった段階で初めて`System`名前空間に変更するというような開発フローを取っていました。

- dynamic関連
  - プレビューの頃は`Microsoft.Dynamic`
  - .NET 4では`System.Dynamic`
- Task関連
  - プレビューの頃は`Microsoft.Threading.Tasks`
  - .NET 4では`System.Threading.Tasks`
- async関連
  - プレビューの頃は`Microsoft.Runtime.CompilerServices`
         - (`Task`クラスなどの既存のクラスの拡張は`System`名前空間。`CompilerServices`だけが新規)
  - .NET 4.5では`System.Runtime.CompilerServices`

これは「`System`に正式配属されました」なので問題ないんですが、ちょっと困るのがこの先。

### バックポーティング実装

この手の新機能を、古いフレームワーク上でも使えるようにするバックポーティング実装があります。

- [`Microsoft.Bcl`](https://www.nuget.org/packages/Microsoft.Bcl/)
  - .NET 4.5の`CallerMemberNameAttribute`、`Tuple`、`IProgress`なんかを.NET 4で使えるようにするもの
- [`Microsoft.Bcl.Async`](https://www.nuget.org/packages/Microsoft.Bcl.Async/)
  - C# 5 (.NET 4.5)の非同期メソッドを.NET 4で使えるようにするもの
- [`Microsoft.Net.Http`](https://www.nuget.org/packages/Microsoft.Net.Http/)
  - .NET 4.5の`System.Net.Http.HttpClient`を.NET 4で使えるようにするもの

こいつら、中身の名前空間は`System`のくせに、パッケージ名は`Microsoft`。プレビュー時代の名残なのかなんなのか。(未来の)標準ライブラリなのに。

### やっぱりSystemダメでした

同系統で、一瞬「標準」に昇格した奴がいます。

- [System.Diagnostics.Tracing.EventSource](https://msdn.microsoft.com/ja-jp/library/system.diagnostics.tracing.eventsource.aspx)
  - ETW (Event Tracing for Windows)へのログ書き込みを.NETから簡単に行えるようにするもの。
  - .NET 4.5で追加

お気づきでしょうか。ETWのWの文字。「for Windows」。これまで「.NET 4.5のあたりでWindowsべったりな奴は`System`から外れた」とさんざん説明してきたところで、こいつの存在に頭抱えることに。

ちなみに、こいつ、その後も、`Microsoft`名前空間実装が生き残っていたりします。

- [Microsoft.Diagnostics.Tracing.EventSource](https://www.nuget.org/packages/Microsoft.Diagnostics.Tracing.EventSource/)
  - 曰く
        - 「`EventSource`クラスは標準にも含まれているけど、こっちの方が新しくて機能多い」
        - 「`System.Diagnostics.Tracing.EventSource`の方にとりこまれるまでのギャップ解消に使って」

「`System`の方にとりこまれるまで」と書かれているものの、何かこのまま`Microsoft`の方にすべきなんじゃないかという予感もひしひしと…

## 逆に、なぜMicrosoft…

逆に、「なぜそれを`Microsoft`名前空間にした」というものもあったりはします。

こいつ。

- [Microsoft.CSharp](https://www.nuget.org/packages/Microsoft.CSharp/)
  - C# 4のdynamicの中で使うやつ
        - C#のオーバーロード解決ルールに従って、実行時バインディングするためのライブラリ
  - この名前で、C#コンパイラーそのものを指すライブラリではないのでそこも要注意
        - コンパイラーそのものは[Microsoft.CodeAnalysis.CSharp](https://www.nuget.org/packages/Microsoft.CodeAnalysis.CSharp/)

C# 4のdynamicを使うのに必須のライブラリです。Mono版C#コンパイラーでも必須(Mono版の`Microsoft.CSharp`実装あり)です。確かに、C#に依存した機能なので`System`名前空間だと変なんですが。一方で、マイクロソフト実装でなくても必須なものに`Microsoft`名前空間というのも嫌な感じで。

## まとめ

最近のノリだと、

- .NET Core (オープンソースでクロスプラットフォームな.NET)でも「標準」となるべき基本ライブラリだけが`System`名前空間
- その他のマイクロソフト公式実装は`Microsoft`名前空間

になっているんですが、過去さかのぼると何でも間でも`System`名前空間だったし、その名残がいまだちらほらあったりします。特に、`System.Diagnostics.Tracing.EventSource`とかどうするんだろう…
