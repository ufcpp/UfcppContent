---
title: "Unity上でasync/await: はじめに"
source_url: "https://ufcpp.net/blog/2015/12/unityasync0introduction/"
content_type: "BlogEntry"
published_at: "2015-11-30T15:01:21"
updated_at: "2015-11-30T15:01:21"
tags: []
umbraco_id: 1817
parent_id: 1816
sort_order: 0
aliases: []
---

# Unity上でasync/await: はじめに

たまにはAdvent Calendar参加。

このブログは[Unity Advent Calendar 2015](http://qiita.com/advent-calendar/2015/unity)の12月1日の記事です。

7月に書いた「[Unity(ゲームエンジン)上で async/await](../../07/unityasyncbridge/index.md)」の続報というか進捗。
あと、補足説明いろいろ。

あれから4か月くらいたったわけでさすがに安定したというか。
むしろ、大して問題出なかったというか。

以下のコミット履歴を見てのとおり、4か月でコミット79個しかないものの、これでもう安定してたりします。

[https://github.com/OrangeCube/MinimumAsyncBridge/commits/master](https://github.com/OrangeCube/MinimumAsyncBridge/commits/master)

これが、「最初から安定してるライブラリは不活性に見えて不安がられる」というやつか…

むしろ、IL2CPPの安定を待ってるというか…

長くなりそうなので3部構成になっています:

- [背景](../unityasync1background/index.md)
- [現状](../unityasync2currentstatus/index.md)
- [課題と感想](../unityasync3retrospective/index.md)
