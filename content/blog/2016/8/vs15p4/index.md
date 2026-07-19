---
title: "Visual Studio \"15\" Preview 4"
source_url: "https://ufcpp.net/blog/2016/8/vs15p4/"
content_type: "BlogEntry"
published_at: "2016-08-23T03:19:11"
updated_at: "2016-08-23T03:19:11"
tags: []
umbraco_id: 1945
parent_id: 1932
sort_order: 4
aliases: []
---

# Visual Studio "15" Preview 4

Preview 4が出たみたいですね。

- [Visual Studio “15” Preview 4](https://blogs.msdn.microsoft.com/visualstudio/2016/08/22/visual-studio-15-preview-4/)

最近、まあ、Previeｗ の新しいのが出ても、C# vNext の進捗具合だけしか取り上げてなかったりするんですが。
今回も主にその話題で。

## インストーラー

1点だけ。新しいインストーラーがだいぶちゃんとしたものになってますね。
細かい機能を選択して入れるんじゃなくて、「UWP開発したい人はこのオプションを選択してください」みたいな感じのUIに(その結果、それに必要な機能に一通りチェックが入る)。

Visual Studioってものすごいインストールに時間が掛かるので有名ですが、結構な割合、エミュレーターとか仮想マシンのインストールに取られてる時間だったりします。
要するに、UWPとかモバイル開発が不要なら、ここ外すだけでかなりインストール早かったり。

## C# 7 進捗

C# 7で入るものの予定は以下のページ参照。

- [Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)

これを踏まえて、Preview 4でのC# 7実装の状況まとめ:

<div>
<script src="https://gist.github.com/ufcpp/637759fdd02409d9f9795e5b00ae0ee2.js"></script>
</div>

`ValueTask`は、ちょっと前に中の人が「Preview 4に入る予定のブランチにマージされたぜ」ってtwitterでつぶやいてましたが、
QA通らなかったのか、リバート食らってました。

他は、たぶんあと細かい調整だけですかね。
変数/ローカル関数のスコープとか、クエリ式中でも分解構文使えるようにするとか。

ちなみに、タプルを使うためには`System.ValueTuple`が、`ValueTask`を使うためには`System.Threading.Tasks.Extensions`が必要になりますが、
どっちももうNuGet.orgに並んでいます。

- [System.ValueTuple](https://www.nuget.org/packages/System.ValueTuple/) (prerelease version のみ)
- [System.Threading.Tasks.Extensions](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/)
