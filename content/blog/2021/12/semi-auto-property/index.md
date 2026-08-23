---
title: "【C# 11 候補】 半自動プロパティ"
source_url: "https://ufcpp.net/blog/2021/12/semi-auto-property/"
content_type: "BlogEntry"
published_at: "2021-12-27T15:16:10"
updated_at: "2021-12-28T15:34:32"
tags: []
umbraco_id: 2393
parent_id: 2375
sort_order: 8
aliases: []
---

# 【C# 11 候補】 半自動プロパティ

[11月くらいからなんとか消化し始めた](../../11/2022-ga-soon/index.md)「[C# ライブ配信](https://www.youtube.com/channel/UCY-z_9mau6X-Vr4gk2aWtMQ)で口頭では言ったけどブログ化はしてなかったやつ」、
「C# 10.0 の補足」とか、文字コード・絵文字がらみの雑談話を抜けて、
やっと「C# 11.0 候補」の話になります。

こんな時間かかるかー…

## 半自動プロパティ

C# 11 目標で、自動プロパティにちょっと手が入りそうです。

[バッキング フィールド](../../../../study/csharp/oop/oo_property.md#auto)を `field` キーワードで読み書きできるようにするというもの。
俗称「半自動プロパティ」。

## おさらい: 初期 C# のプロパティ

C# 1.0 の頃からの一番煩雑な書き方だとプロパティは以下のように書いていました。追加でフィールドが1個必要。

```csharp {title="C# 初期のプロパティ"}
class A
{
    private int _x;
    public int X { get { return _x; } set { _x = value; } }
}
```

それに対して C# 3.0 で書けるようになった簡易記法が自動プロパティ(automatically implemented property、通称 auto-property)。
`get; set;` だけ書くと、上記の `_x` フィールド相当のものを自動的に作ってくれます。

```csharp {title="C# 3.0 の自動プロパティ"}
class A
{
    public int X { get; set; }
}
```

## 自動プロパティが使えなかったものの例

(この後もプロパティは細かく色々な改善があるんですが、それは置いておいて)

C# 3.0～10.0 までの “完全に自動な” プロパティだと一部の頻出する用途に使えなくて、結局は自前でフィールドを用意しないといけないことがありました。
特に有名な例を2つ挙げると、

1． PropertyChanged

```csharp {title="PropertyChanged のためにフィールドが必要"}
using System.ComponentModel;
using System.Runtime.CompilerServices;

class A : INotifyPropertyChanged
{
    private int _x;
    public int X { get => _x; set => SetProperty(ref _x, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void SetProperty<T>(ref T storage, T newValue, [CallerMemberName] string? propertyName = null)
    {
        storage = newValue;
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
```

2. 遅延初期化

```csharp {title="初回限りの重たい処理を、プロパティの初アクセス時に呼びたい"}
class A
{
    private string? _x;
    public string X => _x ?? GetX();

    private string GetX()
    {
        // 初回限りの重たい処理
    }
}
```

## field キーワードの追加

で、要望自体は結構昔からあったんですがようやく C# 11.0 で採用されそうなのが「`field` キーワード」。

例えば前節の例は以下のように書けます。

1． PropertyChanged

```csharp {highlight-ranges="sha256:851826c9564b993aabafe1e424d7009b13c15b3c72266e8b56066be3ab3f0546;3:27-3:32,3:57-3:62"}
class A : INotifyPropertyChanged
{
    public int X { get => field; set => SetProperty(ref field, value); }

    // 以下元と同じ
}
```

2. 遅延初期化

```csharp {title="field キーワードで遅延初期化" highlight-text="field"}
class A
{
    public string X => field ?? GetX();

    // 以下元と同じ
}
```

## 細々補足

以下のような補足あり。

* C# らしく(破壊的変更を避けて)、`field` は文脈キーワード
  * `field` と言う名前のフィールドがない場合だけキーワード扱い
* キーワード扱いを受けた場合、`nameof(value)` はコンパイルできないという仕様。
* `get` 側しかない場合は get-only プロパティと同様
  * コンストラクターでだけ `set` 可能
  * 生成されるフィールドは `readonly` 扱い

この新機能、俗称としては「半自動プロパティ」(semi-auto-property)なんですが、実装上・仕様書上は「自動プロパティの項目を修正」みたいです。

元:

* セミコロンのみの `get;` `set;` しかないプロパティを自動プロパティと呼ぶ

変更後:

* 以下の2つを自動プロパティと呼ぶ
  * セミコロンのみの `get;` `set;` アクセサーしかないプロパティ
  * アクセサー内で `field` キーワードを使っているプロパティ

## おまけ field はキーワードで value は変数？キーワード？

ちょっと余談。

field は明確に「文脈キーワード」です。補足説明の通り、`nameof(field)` 不可。

ところで、じゃあ、C# 1.0 の頃からある `value` はと言うと…
とりあえず、Visual Studio 上では「青」(キーワード扱いの色)です。
(↓ うちのサイトの色付けは Visual Studio 初期設定準拠。)

```csharp {title="value は青"}
class A
{
    private int _x;
    public int X { set => _x = value; }
    // ↑ Visual Studio 上、value の文字は青(キーワードの色)になってる。
}
```

ところで、この `value`、仕様書上は「`set` アクセサーには暗黙の引数 `value` がある」みたいな書かれ方になっています。
そして、結果的に `nameof(value)` は許されるという。

```csharp {title="nameof(value)"}
class A
{
    private string _x;
    public int X { set => _x = nameof(value); }
    // 意味あるコードではないものの、とりあえずコンパイル可能。
}
```

`nameof(int)` とかも許されておらず、`nameof` の中に「青」が来る(たぶん)唯一の例となります。

時代の名残りと言うかなんというか…
今なら `value` も文脈キーワードにしたかもしれないですね。

ちなみに、同じく仕様からして「暗黙の引数」とされている[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)の[コマンドライン引数の `args`](../../../../study/csharp/misc/miscentrypoint.md#args-returns) はちゃんと「群青」(変数・引数の色)です。

```csharp {title="args は群青"}
Console.WriteLine(args[0]);
```

まあ、`field` キーワードは最初から「キーワード扱い」の予定です。

```csharp {title="field は青"}
class A
{
    public int X { set => field = value; }
}
```
