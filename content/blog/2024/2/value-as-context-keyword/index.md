---
title: "field, value を文脈キーワード化"
source_url: "https://ufcpp.net/blog/2024/2/value-as-context-keyword/"
content_type: "BlogEntry"
published_at: "2024-02-24T22:20:18"
updated_at: "2024-02-24T23:08:00"
tags: []
umbraco_id: 2488
parent_id: 2480
sort_order: 7
aliases: []
---

# field, value を文脈キーワード化

C# 13 向けに検討されている機能の一つに、
「半自動プロパティ」とか「field キーワード」と呼ばれているものがあります。
元々は C# 12 向けに考えられていて、去年、うちのブログでも書いているやつです。

* [【C# 12 候補】半自動プロパティ](../../../2023/1/semi-auto-property/index.md)

簡単におさらいすると、
プロパティの `get`/`set` アクセサー内で、`field` を使って
[バッキング フィールド](../../../../study/csharp/oop/oo_property.md#auto)(自動プロパティの値を保存するためにコンパイラーが生成するフィールド)に明示的にアクセスするというものです。

```csharp {title="半自動プロパティ案"}
class A
{
    // 手動プロパティ (manual property)
    // (と、自前で用意したフィールド)。
    // こういう、プロパティからほぼ素通しで値を記録しているフィールドを「バッキング フィールド」(backing field)という。
    private int _x;
    public int X { get => _x; set => _x = value; }

    // 自動プロパティ (auto-property)。
    // 前述の X とほぼ一緒。
    // バッキング フィールドの自動生成。
    public int Y { get; set; }

    // 【C# 12 候補(改め、13 候補)】 半自動プロパティ (semi-auto-property)。
    // バッキング フィールドは自動生成。
    // 全自動の方と違って、バッキング フィールドの使い方は自由にできる。
    // field キーワードでバッキング フィールドを読み書き。
    public int Z { get => field; set => field = value; }
}
```

C# 12 時点では「これを破壊的変更なしで実装するのは大変」ということで見送りになりまして、
その結果検討されていたのが先日書いたブログの話。

* [C# での破壊的変更の今後の扱い (続報)](../breaking-changes/index.md)

ここで、「`field` の扱いで破壊的変更があるんだったら、`value` についても…」
という話が出ています。

* [[Proposal]: Field and value as contextual keywords #7964](https://github.com/dotnet/csharplang/issues/7964)

というのも、`value` (プロパティの `set` 内でだけ特別な意味を持つ)はちょっと C# 的には珍しく、
キーワードではなくて「暗黙に定義された引数」で、ちょっと浮いた挙動をします。

1つ目、[`@` で「脱キーワード化」](../../../../study/csharp/start/st_variable.md#identifier)ができない。

```csharp {title="キーワードじゃないので…"}
class A
{
    public int X
    {
        set
        {
            // value は @ を付けてもダメ。
            // 扱いが「暗黙定義された引数」なので、@value もその引数を指す。
            var @value = 1;

            // 普通、キーワードだったら @ を付けることで識別子に使える。
            var @this = 2;
        }
    }
}
```

2つ目、[`nameof`](../../../../study/csharp/start/st_string.md#nameof-operator)。

```csharp {title="nameof が使える"}
class A
{
    public int X
    {
        set
        {
            // 逆に、引数扱いゆえに nameof が使える。
            var n1 = nameof(value);

            // キーワードには nameof は使えない。
            var n2 = nameof(this);
        }
    }
}
```

3つ目、外側の識別子の参照。

```csharp {title="外にある「value フィールド」の参照ができない"}
class A
{
    int value;
    int @this;

    public int X
    {
        set
        {
            // 外にある「value フィールド」すら、@value では参照できない。
            // 暗黙の引数の方になる(@ を付けるだけ無駄)。
            // (ちなみに、 this.value = 1; と書けばフィールド参照になる。)
            @value = 1;

            // キーワードの場合は @this で外のフィールド参照になる。
            @this = 2;
        }
    }
}
```

`field` を足すことで軽微ながら破壊的変更が出るんなら、
`value` に軽微な破壊的変更がかかってもいいのではということで、
もうこの際 `value` もキーワード(`set` 内限定なので、[文脈キーワード](../../../../study/csharp/appendix/ap_reserved.md#context))してもいいのではという話になります。
どういう影響があるかというと、先ほどの例からわかる通りで、

* `var @value = 1;` みたいなのが書けるようになる
  * これは、できないことができるようにあるので破壊的ではない
* `nameof(value)` が書けなくなる
  * こう書いていた人が多数派とは思えない
  * ※追記: `if (value is null) throw new ArgumentNullException(nameof(value));` って書く人それなりにいる説あり
* `@value = 1;` みたいなのが暗黙的引数の上書きから、外のフィールドの上書きに変わる
  * 単に `value = 1;` でよかったわけで、もともと変

となります。

ちなみに、`field` は「暗黙の引数扱い」でも「文脈キーワード扱い」でもどちらにしろ破壊的変更になります。
「文脈キーワード扱い」の方が自然っぽいんですが、
そうなるとこの「`value` と何か挙動が違う」が気になるという懸念がありまして。
そこで出た対案が「`value` も文脈キーワードに変更」という感じかと思います。
