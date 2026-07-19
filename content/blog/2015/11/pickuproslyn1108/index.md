---
title: "ピックアップRoslyn 11/8"
source_url: "https://ufcpp.net/blog/2015/11/pickuproslyn1108/"
content_type: "BlogEntry"
published_at: "2015-11-08T07:01:40"
updated_at: "2015-11-08T07:01:40"
tags: []
umbraco_id: 1814
parent_id: 1810
sort_order: 1
aliases: []
---

# ピックアップRoslyn 11/8

## Global Summit

[Global Summit](https://mvp.microsoft.com/ja-jp/Summit) に行っていたわけですが、最近の開発系製品の類はオープンソースになってて最初から全部見えていたり。というか、前日に「Summitでしゃべる内容はこんなの」みたいなIssueページが立ったりして、情報筒抜け。

ちなみに補足しておくと、MS MVP Global SummitはNDAの下でいろいろと聞いてこれる場なわけですが、

- NDA は、聞いた情報がプレスリリースやブログなどで公開された時点で切れる。その後は話せる
- 「あんまり NDA な話聞いても人に伝えられないのがかえって困ったりする」という要望あり

という2つの事情から、ほぼ同時、あるいは、せいぜい数週程度で情報公開するものが増えていたりします。せいぜい、MS内部の人から直接話を聞ける、直接会える、直接フィードバックを言える、他国のMVPのフィードバックや反応まで見えるという感じのもの。

まあ、製品によるんですが。.NET系は、[Roslyn](https://github.com/dotnet/roslyn)がGitHubになって以来、ほぼそんな状況です。

### Outline of C# 7 demo at MVP Summit 2015-11-02

[Outline of C# 7 demo at MVP Summit 2015-11-02 #6505](https://github.com/dotnet/roslyn/issues/6505)

ということで、前日リーク。

今、仕様が結構固まってきて、動く実装があるものは以下の2つ。

- Local Functions
- Pattern Matching

この2個のデモ。

ただ、デモとして見ることはできても、一般の人がこれを手元に持ってきて試すってのはまだみたい。
Visual Studio 2015 Update 1で、「単機能リリース」ができるようにVisual Studio自体に手を入れるみたなので、それ以降。

「単機能リリース」は、要するに、「C# vNext」みたいなでかい単位じゃなくて、「Pattern Matching単体」とか「Local Functions単体」とかを個別に、Visual Studio拡張として(Visual Studio自体を上書きしない、アンインストールできれいさっぱり汚れ残さず消える)提供するもの。現状のVisual Studioだとうまくできないみたい。

ちなみにこの2つ、仕様のドラフトが上がったみたいです(futureブランチにmerg済み)。

- [roslyn/docs/features/](https://github.com/dotnet/roslyn/tree/future/docs/features)
  - [https://github.com/dotnet/roslyn/blob/future/docs/features/local-functions.md](https://github.com/dotnet/roslyn/blob/future/docs/features/local-functions.md)
  - [https://github.com/dotnet/roslyn/blob/future/docs/features/patterns.md](https://github.com/dotnet/roslyn/blob/future/docs/features/patterns.md)

## RoslynQuoter

C# のコードを書いたら対応する SyntaxNode を作るコードを出力してくれるサービスが。要るよね、そりゃ…

- [RoslynQuoter](http://roslynquoter.azurewebsites.net/)
  - [ソースコードおいてある GitHub リポジトリ](https://github.com/KirillOsenkov/RoslynQuoter)

ちなみに、MS社員。Roslyn側じゃなくて、IDEチームっぽい。
