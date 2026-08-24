---
title: "ピックアップRoslyn 2/10 変数のshadowing、関数ポインター、実行時nullチェック、Index/Rangeの仕様変更"
source_url: "https://ufcpp.net/blog/2019/2/pickuproslyn0210/"
content_type: "BlogEntry"
published_at: "2019-02-10T19:59:01"
updated_at: "2019-02-10T20:06:27"
tags: []
umbraco_id: 2221
parent_id: 2220
sort_order: 0
aliases: []
---

# ピックアップRoslyn 2/10 変数のshadowing、関数ポインター、実行時nullチェック、Index/Rangeの仕様変更

しばらくちょっと忙しくて紹介できてなかった話をいくつかまとめて。

## 匿名関数の変数 shadowing

[こないだの VS 2019 Preview 2](../../1/vs2019p2/index.md) から、
1段外側の変数と同じ名前で、
ローカル関数内の引数・変数を宣言できるようになったみたいです。

外側の `x` を隠すので shadowing と呼ばれます。

```csharp {title="shadowing"}
static int M()
{
    int x = 1;
 
    // C# 8.0 で、1段外側の変数の x と同名の引数が使えるように
    int m(int x) => x * x;
 
    return m(x);
}
```

きっかけとしては、静的ローカル関数が入ったからみたいです。
要するに、

- 外の変数をキャプチャしてるのかどうかぱっと見で分かりにくくなるのは怖いから許してなかった
- 静的ローカル関数ならキャプチャすることを許さないのでその問題は解消する
- とはいえ、静的ローカル関数でだけ shadowing を認めるのも気持ち悪い

という流れ。
この決定自体は[結構前(去年の9月10日)にやってたみたい](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-09-10.md)です。

ちなみに、現時点ではローカル関数のみ。ラムダ式では shadowing されません。
けども、[1月16日の Designs Meeting](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-01-16.md)で、ラムダ式でも認めよう、クエリ式でも検討してみようという話が出ていたり。

## 関数ポインター

C# で[デリゲートではなく生で関数ポインターを使いたい](../../../2018/8/pickuproslyn0822/index.md)という話があったわけですが。
[最近ちょっと検討が進んだみたいで、ちょっと文法が具体化してきました](https://github.com/dotnet/csharplang/blob/master/proposals/function-pointers.md)。

`func* int(string);` みたいな書き方になるみたいで、
unsafe コード必須。

## 実行時の null チェックの挿入

C# 8.0 で導入される予定の[null 許容参照型](../../../2018/12/cs8nrt/index.md)は、
基本的にコンパイル時のチェックであって、
(unsafe とかを使って)コンパイル時に拾えないような null が来ても、実行時には何もしません。

一方で、[実行時の null チェックを挿入するような簡易文法も足したい](https://github.com/dotnet/csharplang/pull/2144)という話が出ているようです。

これまで、C# では結構以下のようなコードを書いたと思います。

```csharp {title="引数の null チェック"}
static int M(string x)
{
    if (x == null)
    {
        throw new ArgumentNullException(nameof(x));
    }
 
    return x.Length;
}
```

これを、以下のように書くだけで同様の実行時 null チェックを挿入するようにしたいというものです。

```csharp {title="実行時 null チェック構文"}
static int M(string x!)
{
    return x.Length;
}
```

ただ、null-forgiving (コンパイル時チェックも実行時チェックもなくす)の `x!` と、この実行時 null チェックの `x!` で同じ書き方なのが結構気持ち悪く…
その辺りはまだちょっと悩んでいるみたいです。

## Index 型、Range 型の仕様変更

[Range 構文](../../../2018/12/cs8ranges/index.md)の内部実装として使われる`Index`構造体と`Range`構造体ですが、
[coreclr 側の API レビューの結果、実装がだいぶ変更されそうです](https://github.com/dotnet/coreclr/pull/22331)

`Index`構造体

- `Start`, `End` 静的プロパティ追加
- `int GetOffset(int length)` メソッド追加

`Range` 構造体

- `FromStart`を`StartAt`に、`ToEnd`を`EndAt`にリネーム
- `Create(Index start, Index end)` 静的メソッドは削除(普通にコンストラクターを使う)
- `OffsetAndLength GetOffsetAndLength(int length)` を追加
  - `OffsetAndLength` は `(int offset, int length)` な構造体 

ここまでは確定。
で、これらを使う側(配列とか `Span<T>` とか)側は、
`Range` 構造体を受け付けるオーバーロードを足すのではダメなんじゃないかという話も。理由は大体、

- `int`版と`Index`版、`(int offset, int length)`版と`Range`版の2重保守が大変になる
- JIT時最適化を掛けにくくなる

みたいな感じ。
これに対して、以下のように `int` 引数なメソッドに属性を付けて、

```csharp {title="属性ベースの Index/Range 受付"}
interface ISomeCollection<T>
{
    [IndexMethod]
    T this[int index] { get; set; }
 
    [RangeMethod]
    ISomeCollection<T> Slice(int start, int length);

    int Length { get; }
}
```

以下のようなコードを、

```csharp
ISomeCollection<T> x;
var y = x[^1];
var z = x.Slice(1..^1);
```

コンパイラーが以下のように置き換える実装を提案しています。
(ただし、`Length` や `Count` をどうやって取るかが課題。)

```csharp
var y = x[^1.GetOffset(x.Length)];
var (offset, length) = 1..^1.GetOffsetLength(x.Length);
var z = x.Slice(offset, length);
```

## LangVersion default の変更

今、LangVersion のデフォルト値(default)は、「最新のメジャー バージョン」になっています。
要するに、現在(最新は 7.3)のところ、default を指定すると 7.0 が選択されます。

これを、今更なんですが、[以下のように変える pull request](https://github.com/dotnet/roslyn/pull/33089) が通っていたり。

- latest はこれまで通り「最新 (マイナー バージョン含む)」
- default は latest と同じ意味、つまり、最新
- 「最新のメジャー バージョン」を表す latestMajor を追加
- 「プレビュー版」を表す preview を追加
