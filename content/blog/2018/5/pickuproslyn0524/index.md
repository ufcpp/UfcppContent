---
title: "ピックアップRoslyn 5/24"
source_url: "https://ufcpp.net/blog/2018/5/pickuproslyn0524/"
content_type: "BlogEntry"
published_at: "2018-05-24T17:21:59"
updated_at: "2018-05-24T17:21:59"
tags: []
umbraco_id: 2155
parent_id: 2150
sort_order: 4
aliases: []
---

# ピックアップRoslyn 5/24

珍しくまめに議事録の投稿が。

- [C# Language Design Notes for May 21. 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-05-21.md)

今回はこの21日の Design Meeting 議事録1件のみの追加。

さらっと内容紹介:

## target typed new

要はこんなやつ。

```csharp
// 今まで
M(new A(1));

// 提案: 左辺から推測できる場合、型名を省略可能
M(new (1));
```

プロトタイプ実装のプルリクが出ていて、いつでも通せる状態ではあるんですが、
ちょっと懸念事項が出てきたとのこと。

今回懸念しているのは以下のような状況。

```csharp
class A
{
    public A(int value) { }
}

class B { }

class Program
{
    static void M(A x) { }
    static void M(B x) { }

    static void Main()
    {
        // 今は、1引数コンストラクターを持っているのが A だけなので、A で推論すべき？
        // その場合、後から B に1引数コンストラクターを足してしまった場合どうあるべき？
        M(new (1));
    }
}
```

厳しくいくなら、「target typed new が使えるのは初期化子でのみ」みたいな制限にしてしまうという案もあり。
なんせ、「フィールドに対して `var` を使えないのがしんどい。`Dictionary<SomeLongNamedType, AnotherLongNamedType>` みたいな長い型を2度書きたくない」というのが最大の要件なので、「初期化子のみ」であってもこの最大要件だけは満たせる。

とはいえ、おそらく、一度はそれで実装したとしても結局後から制限を緩めたくなるだろうと思われる。
なので、まじめにこの「後からコンストラクターを足す」問題に向き合わないといけない。

似たものでいうと、「[`out var`](../../../../study/csharp/resource/sp_ref.md#out-var)を使った場合、オーバーロード解決しない」(引数の数が同じ複数のオーバーロードがある場合、`var`ではなく、具体的な型が必須)ってなっているので、今後 target typed new を足す際にも同様の制限を掛けるのがいいかもしれない。

## return/break/continue 式

`condition ? value : throw new Exception()` みたいに、`throw`が式にできたんだから、`return`、`break`、`continue`も式の中で使いたいという話。

こちらも、プロトタイプ実装のプルリクが既に(それもコミュニティ貢献で)出ていて、
やろうと思えばいつでも通せる状態。

が、懸念になっているのが `return` はどこまでさかのぼって `return` されるべきか。
(`throw`の場合はどの道「`catch`があるところまで上に伝搬」なので問題にならないが、
`return` は1段階だけ戻るものなので。)

例えば、「ブロック式」みたいなのも将来入れる可能性はあって、
以下のような書き方ができるようになったとして、この `return` はブロックを抜ける(`x` に 1 もしくは 2 が入る)ために使われることになる。

```csharp
var x = { if (condition) return 1; else return 2; };
```

今、「`return`式」を入れたとして、こういう将来考えうる文法との整合性がちゃんと保てるかという心配が強く、「現時点では保留」としておいた方がよさそう。

## 非同期 stream

- 生成: `await` と `yield` を混在させれるようにしたい
- 消費: `foreach await (...)` で列挙できるようにしたい

### 非同期 foreach はパターン ベースであるべきか

通常の `foreach` は、所定のパターンさえ満たせば使えます(`GetEnumerator`, `MoveNext`, `Current` などを持っていれば、インターフェイスなどは求めない)。

非同期 `foreach` も同様にすべき。する予定。

### 拡張メソッドで非同期 foreach

現状、通常の `foreach` では、`GetEnumerator` はインスタンス メソッドでないとダメ。
(初期設計がそうだったのと、そこまで拡張メソッドにしたいという要望が強くなかったため。)

非同期 `foreach` では拡張メソッドも認めたいし、
既存の通常 `foreach` でも拡張メソッドを認められるよう検討したい。

### ラムダ式中で await と yield の混在

現状の C# では、ラムダ式中で `yield` を使えない。
(「メソッド中に1つでも`yield`があれば[イテレーター](../../../../study/csharp/data/sp2_iterator.md)扱い」っていう仕様と相性が良くない。入れ子のせいで誤判定が怖い。)
ただし、「優先度低」で捨て置かれてるけども、害悪だとまでは思ってない。

非同期版(`await`と`yield`の混在)もしばらく考えないでおきたい
(やるなら、同期版(ラムダ式中での`yield`利用)と同時に考えたい)。

### イテレーター化する条件

今、

- 非同期メソッドは、メソッドに `async` 修飾子が必須
- イテレーターは、特に修飾子は必要なく、メソッド中に `yield` があるかどうかで判定

と、ちょっと一貫性が欠けた状態にはなっている。
(イテレーターのこの仕様は今となってはあまり良くないかもしれないけども、
破壊的変更や、「同じことを別の文法でもできる」状態にしてまでやることではない。)

`await`と`yield`を混在させるにあたってはどうするべきか。
現状、今の延長で行く予定。
すなわち、メソッドに`async`修飾子が付いていて、かつ、メソッド中に`yield`が1つでもあれば非同期 stream 扱い。

### 非同期 stream の戻り値はパターン ベースであるべきか

非同期メソッドは、当初は戻り値の型が `Task`/`Task<T>` 限定だった。
C# 7.0 移行は、所定のパターンを満たしていれば任意の型を使えるようになった。

一方で、イテレーターはいまだに `IEnumerator`/`IEnumerable`/`IEnumerator<T>`/`IEnumerable<T>`  限定。

非同期 stream では最初から所定のパターンを満たした任意の型を使えるようにしたい。
一緒に、イテレーターの戻り値もパターン ベースにできるように検討したい。

### dynamic に対して foeach await

`dynamic x` に対して `foreach (var i in x)` は使えるけども、非同期版の場合にも同様に dynamic を認めるべきか。

やらないつもり。
