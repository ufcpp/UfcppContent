---
title: "小ネタ コレクション初期化子"
source_url: "https://ufcpp.net/blog/2016/12/tipscollectioninitializer/"
content_type: "BlogEntry"
published_at: "2016-12-06T00:00:49"
updated_at: "2016-12-05T15:01:13"
tags: []
umbraco_id: 1983
parent_id: 1969
sort_order: 5
aliases: []
---

# 小ネタ コレクション初期化子

昨日のオブジェクト初期化子に続き、今日はコレクション初期化子の話。

コレクション初期化子ってのは、例えば以下のようなやつのことです。

```csharp
// この、{} の部分がコレクション初期化子。
var x = new List<int> { 1, 2, 3, 4, 5 };
```

このコレクション初期化を使える条件は、`Add` メソッドを持っていて、かつ、 `IEnumerable` を実装していることです。

最低限の実装をしてみると、以下のような感じ。

```csharp
class MyList : IEnumerable
{
    List<int> _list = new List<int>();
    public void Add(int value) => _list.Add(value);
    public IEnumerator GetEnumerator() => _list.GetEnumerator();
}

static void ListSample()
{
    var x = new MyList { 1, 2, 3, 4, 5 };

    foreach (var item in x)
        Console.WriteLine(item);
}
```

この、コレクション初期化子は以下のように展開されます。

```csharp
var x = new MyList();
x.Add(1);
x.Add(2);
x.Add(3);
x.Add(4);
x.Add(5);
```

ここで生じる疑問があります: `IEnumerable` の実装、要るの？

## 依存は避けれるなら避けるべきもの

だって、`Add`メソッドしか使ってなくない？`IEnumerable`は何にも使ってないよね？

だいたい、C#の文法が`IEnumerable`に依存しちゃうの？
例えば、`foreach`であれば`GetEnumeartor`メソッドさえ持っていれば、別に`IEnumerable`インターフェイスを実装していない型であっても使えます。
LINQもそうで、`Select`や`Where`など、所定のメソッドさえ持っていれば、クエリ式を使えます。

最近、Build Insiderで[Task-likeの話](http://www.buildinsider.net/column/iwanaga-nobuyuki/009)とかも書きましたけど、
言語の文法が何かの型に依存するというのはリスクを持ちます。
可能なら避けるべきものです。

で、コレクション初期化子、`IEnumerable` 要るの？

## たぶん、誤用の防止

まあ、問題になるとすると以下のような例ですかね。

```csharp
struct Adder
{
    public int Add(int x, int y) => x + y;
}

static void AdderSample()
{
    // こういう誤用を防ぎたかったのかなという気はする
    var x = new Adder
    {
        { 2, 1 },
        { 3, 4 },
        { 5, 9 },
    };
}
```

`Add`メソッドだけを条件にしてしまうと、こういうコードが書けてしまう。
で、この`Add`の呼ばれ方だと、何の役にも立たないわけです。
`Adder`の内部状態を変えたいわけじゃなてく、単なるオペレーターなわけでして。

もちろん、`IEnumerable`の実装を義務付けたところで、あえて濫用することはできます。
例えば、以下のような書き方なら現在の仕様でもできます。

```csharp
class Accumulator : IEnumerable
{
    public int Sum { get; set; }
    public int Add(int value) => Sum += value;

    // 空実装してしまえば、コレクション初期化子の乱用可能
    public IEnumerator GetEnumerator() => throw new NotSupportedException();
}

static void AccumulatorSample()
{
    // コレクションでもないんでもないけど、コレクション初期化子を使える
    var x = new Accumulator { 1, 2, 3, 4, 5 };
    Console.WriteLine(x.Sum); // 15
}
```

とりあえず空実装。

まあ、意図的にやってるので大して問題にはならないんですが。
`Adder`みたいなのが意図せずコレクション初期化子で使われるのだけは防止したかったんですかね…
そのために`GetEnumerator`の空実装しろと…
