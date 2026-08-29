---
title: "小ネタ C# と他の言語との差というと"
source_url: "https://ufcpp.net/blog/2016/12/tipsotherlangs/"
content_type: "BlogEntry"
published_at: "2016-12-23T15:19:37"
updated_at: "2016-12-23T07:47:09"
tags: []
umbraco_id: 2006
parent_id: 1969
sort_order: 22
aliases: []
---

# 小ネタ C# と他の言語との差というと

C#で、「他の言語との差というと」とか「他の言語から来たばかりの人が書きがちなコード」みたいなことを聞かれた場合、まず何が思い浮かぶでしょう。

C#に馴れちゃってる人だと、LINQとかasync/awaitとかの機能が最初に浮かんだりします。でも、この辺りは「大きな機能」過ぎて、知ってるか知らないかの二択、1度知れば検索してすぐに解説が出てくる類で、かえって問題にならないという印象。
案外、困るのはもうちょっと細かい部分じゃないかと思います。

みたいなのが今日の話題。

## 辞書(ハッシュテーブル)の列挙

`Dictionary<TKey, TValue>`の列挙を、キーも値も両方使うのに、`Keys`を使ってやろうとする人が結構いるらしいという話を聞きます。要するに以下のような書き方。

```csharp {title="C#っぽくない書き方" highlight-ranges="15:29-15:37,17:13-17:34"}
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var dic = new Dictionary<string, int>
        {
            { "one", 1 },
            { "two", 2 },
            { "three", 3 },
        };

        foreach (var key in dic.Keys)
        {
            var value = dic[key];
            Console.WriteLine($"{key} => {value}");
        }
    }
}
```

C#の`Dictionary`はキーと値をまとめて列挙できる(`IDictionary<TKey, TValue>`インターフェイスが`IEnumerable<KeyValuePair<TKey, TValue>>`インターフェイスから派生している)ので、以下のように書けます。

```csharp {title="C#の書き方"}
        foreach (var x in dic)
        {
            Console.WriteLine($"{x.Key} => {x.Value}");
        }
```

得られる結果が一緒だからどちらでもいいと思うかもしれないですけど、パフォーマンスが結構違います。この手のコレクション(他の言語で言うところの`map`とか`Hashtable`)のインデクサー アクセスはそこそこなコストです。
この例みたいなのだと、`Dictionary`内の要素の数にもよりますが、前者の`Keys`越しの方が2～3倍くらい遅いです。

## 文字列中の文字の列挙

`string`が`IEnumerable<char>`なのも案外気付いていない人がいるとか。

```csharp {title="C#っぽくない書き方"}
var s = "aáαあ亜😀";

for (int i = 0; i < s.Length; i++)
{
    var c = s[i];
    Console.WriteLine(c);
}
```

C#だと大体は`foreach`で列挙します。

```csharp {title="C#の書き方"}
foreach (var c in s)
{
    Console.WriteLine(c);
}
```

というか、[文字列からインデックス使って「N文字目」を取れると思うなよ](http://www.buildinsider.net/language/csharpunicode/01)。

上記の例でも、`foreach`の書き方含め、絵文字が2文字に割れちゃって正しく文字コードを取れません。
C#で正しくサロゲートペアを正しく扱うのはいまだにちょっと面倒なんですが…
いずれ、以下のように書けるようになるはずです。

```csharp {title="Utf8String"}
using System;
using System.Text.Utf8;

class Program
{
    static void Main()
    {
        var si = new Utf8String("aáαあ亜😀");

        foreach (var c in si.CodePoints)
        {
            Console.WriteLine(c);
        }
    }
}
```

逆に、この`Utf8String`からは、インデックスを使って「N文字目」を取る手段はなくなっています。

## `Format("{0} {0}", x)`

C# 6で[interpolation](../../../../study/csharp/start/st_string.md#string-interpolation)が入った今、あんまり使うものではなくなりましたが、`string.Format`の呼び方に関して。

interpolation でも書けない書き方なんですけども、以下のように、同じインデックスを複数回使う書き方ができたりします。

```csharp {title="C#の書き方"}
Console.WriteLine("({0} + {1}) × ({0} - {1}) = {0}^2 - {1}^2", "x", "y");
// (x + y) × (x - y) = x^2 - y^2
```

わざわざ、以下のような書き方をしてしまう人をちらほら見かけるとか

```csharp {title="C#っぽくない書き方"}
Console.WriteLine("({0} + {1}) × ({2} - {3}) = {4}^2 - {5}^2", "x", "y", "x", "y", "x", "y");
```

`printf`だとこんな感じで書いてましたもんね…

## 文字列の `+` 演算

以下のようなコードをC#で書くと、結果はどうなるでしょう。

```csharp {title="問題"}
string s1 = "abc";
object s2 = null;
Console.WriteLine(s1 + s2);
```

選択肢:

1. ぬるぽ(`NullReferenceException`発生)
1. `abc`が表示される
1. `abcnull`が表示される

答えは2番です。C#で、nullを文字列連結すると、空文字扱いになります。

Javaは3番になるんでしたっけ？nullが`"null"`に化けるっていう。

言われてみると、言語ごとに挙動が微妙に違ってちょっとめんどくさいですね、これ。

どっちもどっちというか、文字列連結に`+`演算子を使うって発想がまず、本当によかったのかどうかという疑問があります…

C#文化では、ガイドラインとして「演算子は、組み込み型のものと全然違う用途でオーバーロードするな」というものがあります。
となると、「組み込み型の`+`は足し算だろ、足し算として使えよ」と言われても仕方がなく。
「文字列連結は足し算といえるか」という命題ではあるんですが。
連結の結果、文字列長が足し算になるので足し算的な何かと言えなくもないですけど、きわどい。

ストリームの読み書きにシフト演算子(`<<`)を使われるよりは幾分かマシですけど、
文字列に対する`+`もやめといた方がよかったんじゃないかなぁ…
「顧客が本当に欲しかったものは[interpolation](../../../../study/csharp/start/st_string.md#string-interpolation)だった」説もありますし。

## `if (x)`

これはC言語方面から来た人がたまーにやらかして、ほんとみんな迷惑するやつなんですが…
`operator true`とかに変な実装を入れてしまうことがあります。

やらかす人は「[null関係の演算子](../tipsnulloperation/index.md)」の回で話した 「nullじゃないのに`x == null`がtrueになる」っていうコードとセットでやらかすんですが…

以下のようなコード。

```csharp {title="C#っぽくない書き方"}
class MyObject : IDisposable
{
    bool _isDisposed;

    public void Dispose()
    {
        // Dispose 後、もうこのオブジェクトは無効
        _isDisposed = true;
    }

    // 無効だったら if (x) { } で {} の中を通らなくする
    public static bool operator true(MyObject obj) => !obj._isDisposed;
    public static bool operator false(MyObject obj) => obj._isDisposed;
}
```

使う側は以下のような感じ。

```csharp {title="C#っぽくない書き方"}
static void M(MyObject obj)
{
    Console.WriteLine("----");
    if (obj) Console.WriteLine("有効");
}
```

C言語だと`if (x)`って結構書いてたましたもんね…
`bool`って概念を持っていなくて、0以外の値は全てtrue扱い(nullは0)で。
間違えて意図しない条件を`if`の中に書いてしまうので良くないと言われています。

良くないから、C#ではわざわざ書けなくしたものでして…
それをぶり返すような`operator`を書かれると結構困惑します。
