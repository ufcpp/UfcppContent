---
title: "[雑記] 明確な代入ルール"
source_url: "https://ufcpp.net/study/csharp/start/definiteassignment/"
content_type: "Article"
published_at: "2023-04-15T16:19:17"
updated_at: "2023-04-15T16:19:17"
tags: []
umbraco_id: 2462
parent_id: 1190
sort_order: 20
aliases: []
---

# \[雑記\] 明確な代入ルール

## <a id="sec-generated-title-1"></a> <a id="abstract">概要</a>

C# には「明確な代入(definite assignment)ルール」と呼ばれる、未初期化変数を避ける仕組みがあります。

## <a id="sec-generated-title-2"></a> <a id="undefined">未定義動作問題</a>

大昔のプログラミング言語では、
変数に対して誰も何の値も代入していないことで、不定な値が返ってくるということがありました。
不定な値が得られてしまうことで、[未定義な動作](../resource/rm_default.md#uninitialized)になります。
特にまずいのは、「テストの時にはたまたまうまくいっていた(うまくいく値が返っていた)けども、本番でだけ失敗する」みたいな状況です。

この未定義動作はかなりまずい状態なので、
最近のプログラミング言語では大体これを防いでいます。
大体以下のいずれかの手段を取ります。

* 既定値: ある決まった値([C# の場合は 0 や null](../resource/rm_default.md))を自動的に代入する
* 明確な代入: 開発者が明示的な代入をすることを義務付ける

C# では、[クラス](../oop/oo_class.md)のフィールドや[配列](../structured/st_array.md)の中身については前者の「既定値による初期化」を行っていて、ローカル変数については後者の「代入の義務付け」を行っています。
この「代入の義務付け」が「明確な代入ルール」です。

## <a id="sec-generated-title-3"></a> <a id="rule">ルールの例</a>

まずわかりやすい例から見ていきましょう。
分岐も何もなければ簡単です。以下のようなコードはコンパイル エラーになります。

```csharp {title="未初期化変数を使おうとしてエラーになる例" error-ranges="sha256:e9eb136ddf7a7b25df7f020c1f626584a8dfda6a0ba2cf6d217a6416abfe6b19;4:19-4:20"}
int x;

// x に何も代入しないまま値を取り出そうとした。
Console.WriteLine(x);
```

解決策は当然「ちゃんと代入すること」(definitely assigned)なんですが、
変数の宣言と同時に初期値を与えるのでもいいですし、
後からの代入でも構いません。

```csharp {title="ちゃんと代入"}
// 変数宣言と同時に初期値を与える。
int x = 1;

int y;

// ここで y を使うとまずいけども…

y = 2;

// 値の代入後なら大丈夫。

Console.WriteLine(x);
Console.WriteLine(y);
```

C# では、この明確な代入を判定する際、分岐も見てくれます。
全ての分岐先でちゃんと代入していれば OK です。

```csharp {title="if-else 両方で代入"}
// 大丈夫な例: if-else 両方で代入。
static void m(bool condition)
{
    int x;

    if (condition)
    {
        x = 1;
    }
    else
    {
        x = -1;
    }

    // 大丈夫。
    Console.WriteLine(x);
}
```

```csharp {title="if でだけ代入" error-ranges="sha256:d92d76fbf7c096c32489790d8c40e719b6d0f4e6c40c16eaaa56701830fd54b1;12:23-12:24"}
// ダメな例: if でだけ代入。
static void m(bool condition)
{
    int x;

    if (condition)
    {
        x = 1;
    }

    // エラー。
    Console.WriteLine(x);
}
```

`if` だけではなく、`switch` でも判定してくれます。

```csharp {title="case が全ての値を網羅"}
// 大丈夫な例: case が全ての値を網羅しているなら大丈夫。
static void m(byte condition)
{
    int x;

    switch (condition)
    {
        case 0: x = -1; break;
        case 1: x = 1; break;
        default: x = 0; break; // default は必須。
    }

    // 大丈夫。
    Console.WriteLine(x);
}
```

```csharp {title="case に漏れ" error-ranges="sha256:0325d9e4776d8b5b5044cf6ceefe2decd142907f1d7869af90d078c523f93280;15:23-15:24"}
// ダメな例: case に漏れがあるとダメ。
static void m(byte condition)
{
    int x;

    switch (condition)
    {
        case 0: x = -1; break;
        case 1: x = 1; break;
        case < 255: x = 1; break;
        // この条件だと、condition が 255 の時が漏れてる。
    }

    // エラー。
    Console.WriteLine(x);
}
```

```csharp {title="結構ちゃんと網羅性をチェックしてる"}
// 大丈夫な例: 結構ちゃんと網羅性をチェックしてる。
static void m(sbyte condition)
{
    int x;

    switch (condition)
    {
        case < 0: x = -1; break;
        case 0: x = 0; break;
        case > 0: x = 1; break;
        // 負、0、正 で全ての値を網羅。
    }

    // 大丈夫。
    Console.WriteLine(x);
}
```

ループも結構ちゃんと判定します。
例えば、`while (false)` や、`break` なども追ってくれます。


```csharp {title="通らないループ" error-ranges="sha256:50ed806e4eabe2b2e1c31cc5116ab76f2bec888221f5224e21140e3376231650;11:19-11:20" warning-ranges="sha256:50ed806e4eabe2b2e1c31cc5116ab76f2bec888221f5224e21140e3376231650;7:5-7:6"}
// ダメな例: 通らないループ。
int x;

while (false)
{
    // ここを通らないこともちゃんと判定される。
    x = 1;
}

// エラー。
Console.WriteLine(x);
```

```csharp {title="早すぎる break" error-ranges="sha256:a5f72f354119ad8dd3bb06cadc375c2da47618c88350d441e729ace6e07efa09;12:19-12:20" warning-ranges="sha256:a5f72f354119ad8dd3bb06cadc375c2da47618c88350d441e729ace6e07efa09;8:5-8:6"}
// ダメな例: 早すぎる break。
int x;

while (true)
{
    break;
    // ここを通らないこともちゃんと判定される。
    x = 1;
}

// エラー。
Console.WriteLine(x);
```

```csharp {title="break 前に代入"}
// 大丈夫な例: break 前に代入。
int x;

while (true)
{
    // これならここを通る。
    x = 1;
    break;
}

// 大丈夫。
Console.WriteLine(x);
```

```csharp {title="永久ループ" warning-text="Console"}
// 大丈夫な例: 永久ループの下。
int x;

while (true)
{
}

// 永久ループの下には来ないので、この行自体呼ばれない。
// その場合、「代入してない」エラーにはならない。
// 別途「絶対に通らない」警告は出る。
Console.WriteLine(x);
```

## <a id="sec-generated-title-4"></a> <a id="improved-rule">ルールの改善</a>

<h5 class="version version10">Ver. 10</h5>

長らく、`?.` や `??` が絡んだ時の明確な代入の判定はあまり賢くありませんでした。
明確に代入されているケースでも、判定漏れでコンパイル エラーになっていました。
(厳しめにエラーになっているので、未定義動作問題は起きません。不便なだけです。)

それが C# 10 で改善されました。
例えば以下のコードは C# 10 以降でだけコンパイルできます。

```csharp {title="?. == true"}
// C# 10 から大丈夫な例: ?. == true。
void m(Dictionary<int, int>? d)
{
    if (d?.TryGetValue(123, out var x) == true)
    {
        // C# 10 から大丈夫になった。
        // (前までは ?. からの == true は判定漏れでエラー。)
        Console.WriteLine(x);
    }
}
```

```csharp {title="?. ??"}
// C# 10 から大丈夫な例: ?. ??。
void m(Dictionary<int, int>? d)
{
    if (d?.TryGetValue(123, out var x) ?? false)
    {
        // C# 10 から大丈夫になった。
        // (前までは ?. からの ?? も同様。)
        Console.WriteLine(x);
    }
}
```
