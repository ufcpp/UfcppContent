---
title: "ピックアップRoslyn 5/27"
source_url: "https://ufcpp.net/blog/2015/5/pickuproslyn27/"
content_type: "BlogEntry"
published_at: "2015-05-27T02:23:02"
updated_at: "2017-11-27T12:13:23"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1744
parent_id: 1700
sort_order: 4
aliases: []
---

# ピックアップRoslyn 5/27

引き続き、[Roslynリポジトリ](https://github.com/dotnet/roslyn)は「RTMに向けてバグ修正で手いっぱい」感漂って来てて、新しい話はあまりないんですが。

というか、割かし重複提案なissueが立って、速攻で「それ、これとの重複じゃない？」→「重複だった…」的な流れになってるものが多々。まあ、issueが800件超えっぱなしですからねぇ、このリポジトリ。

そんな中いくつか。

## Support the tadpole operators #3072

[https://github.com/dotnet/roslyn/issues/3072](https://github.com/dotnet/roslyn/issues/3072)

完全にネタというか、ネタに振り回されてるというか。

大元の火種はこれ: [New C++ experimental feature: The tadpole operators](http://blogs.msdn.com/b/oldnewthing/archive/2015/05/25/10616865.aspx)

「[The Old New Thing](http://blogs.msdn.com/b/oldnewthing/)」は、「Windows秘話」「Windows温故知新」的に、「Windowsのここがダメだ」とか言われまくってる辺りに対して、何でそんな風になってるのかとか、そんな話を面白おかしくネタにしているブログ。有名な奴だと、「『Windowsはいつも無駄に見た目ばっかり変えやがって』って文句言うけどさ、中身だけ無茶苦茶進化してて、見た目が変わってない電卓は、ずっと進化してること気づいてもらえなかったんだぞ」みたいな話(「[When you change the insides, nobody notices](http://blogs.msdn.com/b/oldnewthing/archive/2004/05/25/141253.aspx)」)とか。

で、今回やらかしたネタが、「オタマジャクシ演算子」。機能的に言うと、「副作用のない`++`, `--`」、「高優先度の単項`+1`, `-1`演算子」。

<table>
<tr>
<th>構文</th>
<th>意味</th>
<th>覚え方</th>
</tr>
<tr>
<td>`-~y`</td>
<td>`y + 1`</td>
<td>オタマジャクシが値の方に向かって泳いで、値を増やす。</td>
</tr>
<tr>
<td>`~-y`</td>
<td>`y - 1`</td>
<td>オタマジャクシが値から去るように泳いで、値を減らす。</td>
</tr>
</table>

これを、「Visual Studio 2015 RCで実装してみた実験的な試み」とか言って紹介（もちろん嘘）。The Old New Thingの方のコメントでも、Roslyn issueページの方のコメントでも、割かしみんな盛大に釣られてるっぽい。

[The Old New Thingの次のブログ エントリー](http://blogs.msdn.com/b/oldnewthing/archive/2015/05/26/10617079.aspx)でネタを明かしていますが、これ、別に「実験的に実装したもの」じゃなくて、現状の正規のC++文法で(C#ででも)認められた普通の演算です。[2の補数](../../../../study/computer/general/generalcomputercircuit.md#two-complement)とかを知ってれば簡単にわかるんですが、ビット反転 `~` と符号反転 `-` は1違いの数字になるので。単にこれを並べただけ。

なんというか。4月1日にやれよ…

## Provide simple syntax to create a weak-referenced eventhandler #101

[https://github.com/dotnet/roslyn/issues/101](https://github.com/dotnet/roslyn/issues/101)

弱イベント作れる専用構文ほしいという要望。

まあ、気持ちはわからなくはないけども。うちのサイトにも「[弱イベント](../../../../study/csharp/resource/rmweakreference.md#weak-event)」の話ありますが。

乱用すると性能面での影響が無視できない、かつ、何も知らないと簡単に乱用される機能なんですよねぇ。個人的にはあると楽になるけど、ほんとにあったらやばそうな機能すぎて、ちょっと。

## Compilation without generics #3064

[https://github.com/dotnet/roslyn/issues/3064](https://github.com/dotnet/roslyn/issues/3064)

[.NET Micro Framework](http://en.wikipedia.org/wiki/.NET_Micro_Framework)ではジェネリック使えないんだし、C#コンパイラー的に「ジェネリックを認めない」版を提供してくれないかという要望。

というか、[C# 4.0以降、自動実装イベントから生成されるコードが変わって](../../../../study/csharp/functional/sp_event.md#auto-event)、中身でジェネリック版`Interlocked.CompareExchange`を使うようになったせいで、.NET MFでイベントが使えないという問題と合わさって結構深刻みたい。

.NET MF自体が今後どうなの？という感じもあり。

まあ、この場合、本当の問題は、自動実装イベントが`Interlocked.CompareExchange`に依存してることな感じも。コンパイラー生成コードが、undocumentedに、何か特定のAPIに依存しているってのはあんまりよくない状態。
