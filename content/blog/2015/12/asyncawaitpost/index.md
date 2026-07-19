---
title: "async/await その後/追記"
source_url: "https://ufcpp.net/blog/2015/12/asyncawaitpost/"
content_type: "BlogEntry"
published_at: "2015-12-02T02:43:12"
updated_at: "2015-12-02T02:46:21"
tags: []
umbraco_id: 1831
parent_id: 1816
sort_order: 4
aliases: []
---

# async/await その後/追記

昨日、[Unity 上での async/await の話](../unityasync0introduction/index.md)と、そのついでに`Task`クラスでかすぎるよとか、async/awaitは`Task`依存強すぎかなぁとか書いたわけですが。

ちょうどその関連の話題が2件ほど出てたので追記。

ほんと、これがもう2年早く出ていれば[MinimumAsyncBridge](https://github.com/OrangeCube/MinimumAsyncBridge)の実装もっと楽だったのに。

## Unity カスタム コルーチン

Unity 5.3 で`CustomYieldInstruction`ってクラスが追加されて、これを実装すればコルーチン内で`yield return`で返して、「待ってもらえる」型を自作できるようになるとのこと。

- [カスタムコルーチン](http://blogs.unity3d.com/jp/2015/12/01/custom-coroutines/)

今まであった`YieldInstruction`クラスとか、メンバー何もなくて、お前何のために居るんだよ。C# 的にはそういうマーカー用の基底クラス作る文化ねぇよ。そういうのには属性使えよ。とかいう状態だったわけですが。

`YieldInstruction`なんて、まだ「待っててほしいかどうか」を`bool` 1個返すだけでカスタム処理掛けるし、awaitやら`Task`クラスやらと比べたらだいぶ簡素なんだから最初から`CustomYieldInstruction`提供してくれればいいのに… とずっと思っていたものがやっと実装されるそうです。

## 任意の「Task 風の型」

一方で、C# のawaitは`Task`クラスに依存しすぎって話も書いてたわけですが。ちょうど[Roslynリポジトリ](https://github.com/dotnet/roslyn/)にそれ関連のissueページが立ちました。

- [Proposal: arbitrary task-like types returned from async methods #7169](https://github.com/dotnet/roslyn/issues/7169)

任意の`Task`風の型を非同期メソッドの戻り値にできるようにする提案。

### 要求はずっとある

先日も言った通り、C# 5.0の仕様が公開された瞬間からさんざん「`Task`以外も使いたい」って要望は出てます。

Microsoft内部都合的にいっても、[UWP](http://www.atmarkit.co.jp/ait/articles/1506/23/news012.html)(WinRT)だと非同期処理には`IAsyncAction`インターフェイスを使わないと行けない(`Task`に対するラッパーを1段かまさないと行けなくて面倒)とかあったり。

[ValueTask](https://github.com/dotnet/corefxlab/blob/master/src/System.Threading.Tasks.Channels/src/System/Threading/Tasks/ValueTask.cs)なんかの話もある様子。非同期処理の結果をキャッシュとして持つにあたって、クラス(= [参照型](../../../../study/csharp/resource/oo_reference.md#reftype)、ヒープ圧迫)である`Task`は避けたくて、構造体(= [値型](../../../../study/csharp/resource/oo_reference.md#valtype))版がほしいって言う需要があり。C#は最近、パフォーマンス向上に向けて結構攻めてたりします。

あと、これがあれば例えば、「同期コンテキスト拾わない版`Task`」みたいなのを作れて、[`ConfigureAwait(false)`付けまくらないと死ぬ問題](http://neue.cc/2013/10/10_429.html)を多少マシにできるかも。

### 問題と対策、提案内容

そもそもC# 5.0導入当時の背景として、

- C# 5.0を出すときに、一度は検討したし、実際プロトタイプ実装はした
- ジェネリックな型に対して型推論がきれいにできなかった
- それを問題視してこの実装は正式版には取り入れなかった

ってのがあります。

とりあえず、複雑な型推論はやっぱりあきらめる必要がありそう。まあ、戻り値側を見ての推論が必要な型とかはやっぱり無理。そこさえあきらめれば、現状の`Task`クラス前提のasync/awaitと同程度の型推論はちゃんとできるっぽい。もしかしたら、C# 5.0当時よりも今の方が型推論が賢いので、今だからこそってのもあるのかもしれないですね。

非同期メソッドの戻り値として使いたい型(`Task`風の型)を作りたい場合は、以下のような型を書けとのこと。

```csharp
[TaskLike(typeof(FooBuilder))] struct Foo { … }
struct FooBuilder { … similar to AsyncVoidMethodBuilder … }

[TaskLike(typeof(FooBuilder<T>))] struct Foo<T> { … }
struct FooBuilder<T> { … similar to AsyncTaskMethodBuilder<T> … }
```
