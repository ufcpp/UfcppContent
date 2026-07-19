---
title: "ピックアップRoslyn 12/18"
source_url: "https://ufcpp.net/blog/2015/12/pickuproslyn1218/"
content_type: "BlogEntry"
published_at: "2015-12-18T14:15:14"
updated_at: "2015-12-18T14:15:14"
tags: []
umbraco_id: 1834
parent_id: 1816
sort_order: 7
aliases: []
---

# ピックアップRoslyn 12/18

## CoreCLR側との兼ね合い

[DNX](https://github.com/aspnet/dnx)向けに、ASP.NETチームの方が実装しちゃってるけども、Roslyn側の協力がほしそう・Roslyn側で取り組んでほしそうな話が2件。

### ICompileModule

[https://github.com/dotnet/roslyn/issues/5561#issuecomment-164688325](https://github.com/dotnet/roslyn/issues/5561#issuecomment-164688325)

DNX側で、`ICompileModule`インターフェイスを実装しておけばコンパイルの途中で独自処理を挟めるという実装があったりします。そのタイミングで、Roslynコンパイラーからの内部情報をもらいたいという要望が出ていて、「それはRoslyn側の仕事だ」と誘導されてきたのがこれ。確かに、今、C#でもCode Injector(コンパイル時に処理を挟む仕組み)の検討してるところだし、統合されてほしいところ。

### async Main

[Async Main [Speclet] #7476](https://github.com/dotnet/roslyn/issues/7476)

`Main`メソッド(プログラムのエントリー ポイント)に非同期メソッドを認めてほしいという話。

これ、DNXではいったん独自に実装したものの、既存の.NET Frameworkコードに対して破壊的変更になるというおしかりが多く、RC版で撤廃されたところなんですよね。

それが改めてRoslyn側で提案されたという。`void Main`があったらそっちを優先するみたいなルール設ければ破壊的変更にならないはずだし、改めて取り組んでもらいたいところ。

まあ、Roslyn側でも何度かこの話題出てたはず。その時問題になってたのは同期コンテキストをどうするか。といっても、非同期`Main`以前からの問題で、GUIだと同時実行制御いらない(`await`したときにUIスレッドに戻ってきて、単一スレッド実行が保証される)のに、コンソール アプリだとそういう仕組みが働かないので`lock`とか書けないとまずくなるプログラムが出てくるっていう問題。

## インライン アセンブラー

Roslyn側に提案が出ているわけじゃないんですが。[corefxlab](https://github.com/dotnet/corefxlab)上のコードでえげつないものを見てしまい。

[PtrUtils.cs](https://github.com/dotnet/corefxlab/blob/67d6fa0556d0835c2d779707904d7474ef9da81e/src/System.Slices/src/System/PtrUtils.cs)

`ILSub`属性内に文字列でILアセンブリ コードを書いて、それをビルド後処理でILコード刺し込みしてるみたい。何かいいツールがあるのかと思ったら、[その処理自体自作](https://github.com/dotnet/corefxlab/tree/67d6fa0556d0835c2d779707904d7474ef9da81e/src/System.Slices/tools/ILSub)。

こんな黒魔術やってる理由は、ガベコレがオブジェクトを追える状態のままポインター操作したいかららしい。それは要件からして大変黒魔術的。パフォーマンス改善のためなので黒魔術になりがちなのはしょうがないんですが、書いてる本人からしてコメントに「なんとか動く」、「きたないトリック」、「Very Bad Things」とか言っている状態。

これ見てると、C#にインラインILアセンブラーを追加してほしいような、追加したいと思うような場面がそもそも魔術的過ぎるような。

## プロトタイプとか仕様とか

いくつか、プロトタイプ実装が動きだしてたり、仕様が固まり始めてたりしてるみたい。

- https://github.com/dotnet/roslyn/issues/7445 非null参照型、プロトタイプ実装開始
- https://github.com/dotnet/roslyn/issues/357#issuecomment-162603202 covariant return types、詳細詰めれたっぽい
