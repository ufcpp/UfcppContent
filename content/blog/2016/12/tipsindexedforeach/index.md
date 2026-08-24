---
title: "小ネタ インデックス付き foreach"
source_url: "https://ufcpp.net/blog/2016/12/tipsindexedforeach/"
content_type: "BlogEntry"
published_at: "2016-12-07T00:23:23"
updated_at: "2016-12-06T15:23:49"
tags: []
umbraco_id: 1984
parent_id: 1969
sort_order: 6
aliases: []
---

# 小ネタ インデックス付き foreach

`foreach` ステートメントで、インデックス付きで列挙したいことが時々あります。
今回は、そういうときの対処方法について。
というか、C# 7が待ち遠しくなる話。

配列や`List<T>`であれば以下のようにも書けます。

```csharp {title="やむなく for ステートメント"}
for (int i = 0; i < length; i++)
{
    var item = array[i];
    Console.WriteLine($"index: {i}, value: {item}");
}
```

`IEnumerable<T>`の場合にはこうは書けず、
現状だと、以下のようにループの外側に1個変数を作る必要があったりします。

```csharp {title="やむなく foreach ループの外に変数を置く"}
var i = 0;
foreach (var item in items)
{
    Console.WriteLine($"index: {i}, value: {item}");
    i++;
}
```

ループの外側に変数`i`が漏れるのが嫌なのと、
あと、`continue`が絡むと`i++`するのが大変になったりします。

`Select`のオーバーロードの1つを使って、以下のような書き方も一応できます。

```csharp {title="Select のオーバーロードの1つに、インデックスを拾えるものがある"}
foreach (var x in items.Select((item, index) => new { item, index }))
{
    Console.WriteLine($"index: {x.index}, value: {x.item}");
}
```

ただ、これだと無駄にオブジェクトが`new`されます(匿名型は参照型なのでヒープ確保が発生します)。ループの中でのヒープ確保はできれば避けたい負担です。
それに、`x.item`みたいな書き方がちょっと嫌な感じです。

[C# 7](../../../../study/csharp/cheatsheet/ap_ver7.md)であれば、[タプル](../../../../study/csharp/datatype/tuples.md)を使うのがいいかもしれません。ついでに、[分解構文](../../../../study/csharp/datatype/deconstruction.md)も使えば多少すっきりします。

```csharp {title="[C# 7] タプルがあれば"}
foreach (var (item, index) in items.Select((item, index) => (item, index)))
{
    Console.WriteLine($"index: {index}, value: {item}");
}
```

タプルは値型なので、いくらかヒープ確保が減ります。
また、[分解](../../../../study/csharp/datatype/deconstruction.md)があるおかげで`x.`とか書く必要がなくなりました。

でもまだちょっとうっとおしいですね。
`(item, index) => (item, index)`とか毎度書きたくないです。
拡張メソッドを用意しておきたいところ。

```csharp {title="Indexed 拡張メソッド"}
public static partial class TupleEnumerable
{
    public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        IEnumerable<(T item, int index)> impl()
        {
            var i = 0;
            foreach (var item in source)
            {
                yield return (item, i);
                ++i;
            }
        }

        return impl();
    }
}
```

これで、以下のように書けます。

```csharp {title="Indexed拡張メソッドの使い方"}
foreach (var (item, index) in items.Indexed())
{
    Console.WriteLine($"index: {index}, value: {item}");
}
```

これなら、まあ、悪くはなさそうです。
こういうメソッド、そこそこ使うことがありそう。

ちなみに、今回は[イテレーター](../../../../study/csharp/data/sp2_iterator.md)を使って`Indexed`メソッドを実装しましたが、ガチガチに最適化するなら、以下のように、構造体で実装してヒープ確保をなくすべきかもしれません。

- [Gist: index付きforeach](https://gist.github.com/ufcpp/2b3e1a5821169f6b21ded175ad05c752)
