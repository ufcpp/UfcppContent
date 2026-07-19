---
title: "[雑記] 構文の進化"
source_url: "https://ufcpp.net/study/csharp/misc/misclanguageevolution/"
content_type: "Article"
published_at: "2016-05-05T00:00:00"
updated_at: "2024-08-31T17:24:48"
tags: []
umbraco_id: 1897
parent_id: 1338
sort_order: 6
aliases:
  - "/csharp/misc/misclanguageevolution/"
---

# \[雑記\] 構文の進化

この記事はソフトウェアデザインに寄稿した内容が元になっています。

> 初出： 技術評論社刊『ソフトウェアデザイン 2016 年 4 月 号<br>
> 　　　　今すぐ実践できる良いプログラムの書き方<br>
> 　　　　C#編 言語機能の進化から学ぶ「良いコードの書き方」

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

プログラムの「良い書き方」は時代とともに進歩しています。そして、それが定着するとともに、プログラミング言語にも「良い書き方」をサポートする新しい構文が徐々に追加されていきます。
ここでは、C#の[プロパティ](../oop/oo_property.md)構文を例に、「良い書き方」とともに言語構文が進歩していった過程を紹介します。

[プロパティ](../oop/oo_property.md)は、クラスの実装側にとってはメソッドのような振る舞いを書け、クラスの利用側にとっては[フィールド](../structured/st_struct.md#field)の読み書きのように書けるメンバーです。

C#のプロパティ構文は、C#のバージョンアップに伴い何度か機能追加されています。時とともに徐々に、より良い書き方や、典型的な書き方が明らかになった結果です。
そこでここでは、C#のプロパティ構文の変遷と、それがどういう要件に基づいているのかを説明していきます。

- [サンプル コード](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2016/GoodCode/src/PropertySample)

## <a id="sec-generated-title-2"></a> <a id="accessor"></a>getter/setter

C#以前からある良いとされる習慣の1つに、「クラスの持つデータは必ずメソッドを通して返せ、[フィールド](../structured/st_struct.md#field)を公開するな」というものがあります。メソッドを通すことで以下のような利点があるためです。

- 実装方法を選べる
- 単純なデータの読み書きだけではなく、追加の処理を挟める
- 実装方法を後から変えても、クラス利用側への影響が出ない
- virtualにすることで、派生クラスで挙動を変更できる

そこで、例えば`X`というデータを読み書きする際には、`GetX`, `SetX`というメソッドを介する習慣ができました。これらのメソッドをそれぞれgetter/setterといい、2つ合わせてアクセサー(accessor)と呼びます。単純なデータの読み書きであっても以下のように`Get`/`Set`メソッドを書くべきということです。

```csharp
class Sample
{
    private int _x;
    public int GetX() { return _x; }
    public void SetX(int x) { _x = x; }
}
```

このような書き方には前述のようなメリットがある一方で、クラス利用側のコードが煩雑になるという問題があります。例えば、`X`の値に1加えるだけでも以下のような書き方が必要になります。

```csharp
var s = new Sample();
s.SetX(s.GetX() + 1);
```

そこで、C#では、プロパティという構文を用意しました。以下のように書きます。

```csharp
class Sample
{
    private int _x;
    public int X
    {
        get { return _x; }
        set { _x = value; }
    }
}
```

`get`, `set`に続けて、メソッド的に振る舞いを書けます。一方で、利用側のコードは以下のように、フィールドの読み書きと同じように書けます。

```csharp
var s = new Sample();
s.X += 1;
```

## <a id="sec-generated-title-3"></a> <a id="set-accessibility"></a>getterとsetterで異なるアクセシビリティ(C# 2.0)

データに対して、読む(get)のと書く(set)のとではだいぶ重みが違います。一般に、書き込みの方が慎重に行う必要があります。多くの場合、読み取り(get)だけを公開(public)し、書き込み(set)はクラス内にとどめる(private)ことになるでしょう。

そこで、C# 2.0で、プロパティのgetとsetの[アクセシビリティ](../oop/oo_conceal.md#level)を[別々に設定できる](../oop/oo_property.md#level)ようになりました。例えばgetだけpublicにして、setをprivateにするには以下のような書き方をします。

```csharp
class Sample
{
    private int _x;
    public int X
    {
        get { return _x; }
        private set { _x = value; }
    }
    public Sample(int x) { _x = x; }
}
```

## <a id="sec-generated-title-4"></a> <a id="auto-property"></a>自動プロパティ(C# 3.0)

あとから実装方法を変更するかもしれないのでメソッド(のように振る舞えるプロパティ)を使うべきといっても、実際のところ、これまでいくつか挙げたコード例がそうですが、ほとんどのプロパティは単純なフィールドの読み書きです。これらのような書き方は、「あとから変えるかもしれない」という心配のためだけに書くには少し煩雑過ぎます。

そこで、C# 3.0では、自動的に上記のようなフィールドとそれに対する読み書きを生成する[自動プロパティ](../oop/oo_property.md#auto)(auto property)という機能が追加されました。以下のように、getやsetの後ろのブロックを省略することで自動プロパティになります。

```csharp
class Sample
{
    public int X { get; private set; }
    public Sample(int x) { X = x; }
}
```

単純なフィールドの読み書きでいい間はこの書き方をし、追加の処理が必要になった際には自動実装ではない以前通りのプロパティに変更します。

## <a id="sec-generated-title-5"></a> <a id="get-only-property"></a>get-onlyプロパティ(C# 6)

繰り返しになりますが、一般に、データの書き込みには慎重になるべきです。極端に言うと、コンストラクターでだけ値を代入して、他の場所では書き換えない方がいいことが多いです。こういう書き換え不能なデータのことをimmutable(変更不能)なデータと呼びます。

C# 5.0までは、immutableであることを確実に保証するためには以下のような書き方でプロパティを作る必要がありました。

```csharp
class Sample
{
    private readonly int _x;
    public int X { get { return _x; } }
    public Sample(int x) { _x = x; }
}
```

時代とともに、データをimmutableにしておくことの良さが一般に広まってきました。C#でも、プロパティをimmutableに書くことが増えています。

そこで、C# 6ではimmutableなプロパティを書きやすくするため、[get-onlyプロパティ](../oop/oo_property.md#get-only)という構文が追加されました。以下のように、getだけを書くことで、前述のようなreadonlyフィールドを自動生成してくれます。書き込みはコンストラクター内でだけ行えます。

```csharp
class Sample
{
    public int X { get; }
    public Sample(int x) { X = x; }
}
```

## <a id="sec-generated-title-6"></a> <a id="record-type"></a>レコード型(検討段階)

immutableなプロパティは、値の初期化をコンストラクターで行う都合上、プロパティに対応するコンストラクター引数が必ず必要になります。前節の例でいうと、プロパティXに対して引数xがあります。これは、大文字・小文字が違うくらいで、ほぼ同じものを何度も書かされている状態です。

そこで、以下のような書き方で、コンストラクターとimmutableなプロパティを自動生成する構文の追加が検討がされています(C# 7には入らず、そのさらに先)。

```csharp
class Sample(int X);
```

`X`以外のメンバーを書きたいときだけ追加でクラス本体を書きます。immutableなプロパティだけでいい場合には、この例のように;だけを書いて、本体を省略します。

この構文を<em>レコード型</em>(record type)と呼びます。ここでのレコード(record: 記録、1件のデータ)という言葉は、単純なプロパティしか持たず、純粋にデータを表現するための型という意味です。
