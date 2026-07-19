---
title: "ピックアップ Roslyn 7/21"
source_url: "https://ufcpp.net/blog/2016/7/pickuproslyn0721/"
content_type: "BlogEntry"
published_at: "2016-07-21T09:53:30"
updated_at: "2016-07-21T09:53:30"
tags: []
umbraco_id: 1931
parent_id: 1927
sort_order: 2
aliases: []
---

# ピックアップ Roslyn 7/21

## PDBファイルへのソースコード埋め込み

[Proposal: Embed sources in PDBs #12625](https://github.com/dotnet/roslyn/issues/12625)

提案されたの自体はだいぶ前だし、[実作業を開始したのも2週間くらい前](https://github.com/dotnet/roslyn/pull/12353)からなんですが、PRの方で出たフィードバックをまとめて1 issueにしたみたい。結構おもしろい。

Visual Studio上で、参照しているライブラリのソースコードにF12キーとかで飛びたいわけですが。PDBみたいなデバッグ用情報にソースコードがまとまってないと、ライブラリ(+ PDB)を公開する側としても、参照する側としてもかなり面倒。issueページ中に背景シナリオがいくつか書かれてますが、例えば、

- ビルド時にソースコード生成していて、その生成される分についてはバージョン管理に含めていない。ビルド時生成分のソースコードも含めてパッキングしてくれる仕組みがないとやってられない
- 会社的にソースコードを出すこと自体はOKだけど、ソースコードを置いてあるリポジトリはプライベート。ライブラリ利用者にアクセス権は与えられない
- GitHubとかでソースコードを公開しているけども、そのGitHub上のコードをVisual Studioから参照できるようにする設定までは面倒すぎてやってられない

などなど。

あと、さらに、[PDBをライブラリ自体に埋め込みたい](https://github.com/dotnet/roslyn/issues/12390)みたいな話も。

## Task-like型を返す非同期メソッド

[5/4のブログ](../../5/pickuproslyn0504/index.md)でちょっと書きましたが、非同期メソッドの戻り値に、`Task`(`System.Threading.Tasks`名前空間)以外の型を返せるようにしたいという話が出ています。

で、なんかマージされてる。

[Allow async methods to return Task-like types #12518](https://github.com/dotnet/roslyn/pull/12518)

[次のVisual Studio ”15” Preview (次は preview 4になるはず)には入る予定っぽい](https://twitter.com/jaredpar/status/755903209189748737)。

この機能なんですけど、割かし最近まで形になってなかったんですよね。なので当然のように、「C# 7/VB 15に入るのかどうか」議論のターゲットにもなってなかった。
4月くらいから[Lucian](https://github.com/ljw1004) (今、VBのPMやってる。元々の専門が非同期処理な方らしく、C# 5.0のasync/awaitにもがっつり関わってたみたい)が作業を開始。この時点でも、「まだLanguage Design Meetingの議題に上げる段階に至ってない」とか言ってたんですが。

その後どうもプロトタイピングがうまくいったようで、気が付いたら[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)のリストに並び、気が付いたらマージされてました。

結構有用な応用例があるんで、C# 7に間に合いそうなのは結構うれしいかも。次の[Build Insiderオピニオン](http://www.buildinsider.net/column/iwanaga-nobuyuki)、この辺りの話にしようかなぁ。
