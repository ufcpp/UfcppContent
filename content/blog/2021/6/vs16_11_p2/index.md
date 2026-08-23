---
title: "Visual Studio 16.11 Preview 2: record struct と global using"
source_url: "https://ufcpp.net/blog/2021/6/vs16_11_p2/"
content_type: "BlogEntry"
published_at: "2021-06-16T22:10:57"
updated_at: "2021-06-16T22:10:57"
tags: []
umbraco_id: 2350
parent_id: 2349
sort_order: 0
aliases: []
---

# Visual Studio 16.11 Preview 2: record struct と global using

[Visual Studio 16.11 Preview 2](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview#16.11.0.pre.2.0) が来ていて、これに C# 10.0 の新機能が2つほど merge されています。
(いつも通り、[LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#new-options) を入れれば利用可能になっています。)

- [record struct](../../../../study/csharp/datatype/record.md#record-struct)
- [global using](../../3/usingimprovements/index.md)

ちなみに本当は [16.10 Preview 3 のとき](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/34)に sealed record ToString って機能もひっそりと入ってるんですが、
まあ下手すると誰も気づかないレベルの修正なので説明省略…
(先月全然ブログを書いてないことへの言い訳。)

## <a id="record-struct">record struct</a>

はい。[レコード型](../../../../study/csharp/datatype/record.md)を[値型](../../../../study/csharp/resource/oo_reference.md#valtype)(構造体)でも作れるようになりました。
C# 9.0 時点で、単に `record` キーワードを使って型定義すると必ず[参照型](../../../../study/csharp/resource/oo_reference.md#reftype)(クラス)になっていたんですが、C# 10.0 では `record struct` と `record class` で値型・参照型を選べるようになりました。

```csharp {title="record struct, record class"}
// こっちは構造体なのでヒープ アロケーション起きない。
// あんまりでかいデータを持たせるとコピーのコストが結構でかい。
var s = new S(1, 2);

// こっちはクラスなのでアロケーション発生。
var c = new C(1, 2);

record struct S(int X, int Y);
record class C(int X, int Y);
```

ちなみに、単なる `record` はこれまで通りクラスです。
`record` と `record class` は完全に同じ意味。

### struct と record struct

レコード型は元々「構造体的な扱いができる参照型」でした。
構造体みたいに、メンバーごとのクローン、メンバーごとの値比較ができるクラスみたいなものです。

じゃあ、`record struct` は普通の `struct` と何が違うかと言うと、以下のような点。

- プライマリ コンストラクターを持てる
- プライマリ コンストラクターの引数からプロパティが自動生成される
- 以下のメソッドが自動的に作られる
  - [`Deconstruct` メソッド](../../../../study/csharp/datatype/deconstruction.md)
  - `ToString`
  - `Equals`, `GetHashCode` (`IEqualtable<T>` インターフェイスの実装)
  - `==`, `!=` 演算子

### struct と with

あと、今回一緒に、普通の構造体に対しても [`with` 式](../../../../study/csharp/datatype/record.md#with)が使えるようになっています。

```csharp {title="普通の構造体に対して with "}
var s1 = new S { X = 1, Y = 2 };
var s2 = s1 with { X = 3 };

Console.WriteLine(s2); // (3, 2)

struct S
{
    public int X { get; init; }
    public int Y { get; init; }
    public override string ToString() => (X, Y).ToString();
}
```

構造体では、ある変数から別の変数に代入したとき、元から自動的にコピーを作っていたので、それをそのまま使っています。

## global using

`global using` を使うと、プロジェクト全体に対して有効な [using ディレクティブ](../../../../study/csharp/structured/sp_namespace.md#using)を書けます。

例えば、ある1ファイルに以下のようなコードを書いたとします。

```csharp {title="global using を書いたファイル"}
global using static System.Console;
global using System.Linq;
global using System.Collections.Generic;
```

そのプロジェクト内では、以下のようなコードが普通に書けます。

```csharp {title="global using の影響下にあるコードの例"}
var x = new List<int> { 1, 2, 3 };
var y = x.Select(i => i * i);
foreach (var i in y) WriteLine(i);
```

[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)と合わせると、本当にこの3行だけで「コンパイルできて実行できるコード」になります。
「ネットで見かけたサンプル コードをコピペしたら動かない」というクレームが減るかと思われます。
(これが一番のメリット。)

あと、「[`DateOnly` なんて名前](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/)嫌だーーー」という方は以下のように書いておけます。一応。(別に推奨はしない。)

```csharp
global using Date = System.DateOnly;
```

### 通常 using と同列

`global using` は、「そのプロジェクト内のすべてのファイルの先頭に `using` があるのと一緒」みたいな挙動をします。
つまり、「通常 `using` よりも外側のスコープ」みたいなことにはなりません。
あくまで「通常 `using` と同列」です。

例えばどこかのファイルに以下のような `System` への `global using` があったとします。

```csharp {title="System への global using"}
global using System;
```

で、これと同じプロジェクト内で通常の `using` を書く場合、以下のような挙動をします。

```csharp {title="global using System; 影響下のコード" error-ranges="sha256:8a66cef71416eeb306b97c19d36193e45cf3a20307edbccf8196df549035ea6b;3:11-3:19" warning-ranges="sha256:8a66cef71416eeb306b97c19d36193e45cf3a20307edbccf8196df549035ea6b;1:7-1:13"}
using System; // すでに global using System; があるので「重複」警告あり

using X = DateTime; // この行はコンパイル エラー。ここでは using System; ありきにはならない。
using Y = System.DateTime; // こっちは OK

namespace A
{
    using X = DateTime; // これも OK。A の外に using System; があるので。
}
```

### 知らないところで using されてる問題

別に `global` かどうか以前の問題なんですが、「`using` しすぎ」は問題を起こすことがあります。
まず、同じ名前の型があった場合に「どっちかわからない」エラーを起こします。
単純に IDE 上での補完候補が増えすぎてうざいとかもあります。
それに、C# の場合、[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md#problem)という、`using` の有無で挙動が変わる機能があったりもします。

`global using` ではそれをプロジェクト全体にわたってできるわけですから、
嫌がらせしようと思えばいくらでも嫌がらせができます。
とりあえず名前被りの例:

```csharp {title="同名クラスを持つ別名前空間を global using"}
// JsonSerializer クラスがどれにもあるので、フルネームで書かないと弁別不能になる。
global using Newtonsoft.Json;
global using Utf8Json;
global using System.Text.Json;
```

ちなみに、`global using` は複数のファイルに書けます。
上記嫌がらせの3行を、それぞれ全く別のファイルに書いておくということもできます。

一方で、一応、<em>ファイルの先頭にしか書けない</em>という縛りはあります。

```csharp {title="先頭以外に global using を書くとさすがにエラー" error-text="global using System.Linq;"}
using System;

class Program
{
    static void Main()
    {
        // 超絶長い Main 処理を延々と書いたりもありえなくはない
    }
}

global using System.Linq; // さすがにこの行はコンパイル エラー
```

#### 問題を起こせる範囲

ただまあ、`global using` の影響範囲はプロジェクト内に限られるので、
嫌がらせができるとすれば基本的に「内部犯」になります。

「[global using で一番邪悪なことやった人が優勝](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/36)」とかいうひどいタイトルで配信してアイディアを募ろうとしていたり。

それで例として「`Where` 拡張メソッドの乗っ取り」を挙げてはいるんですが…
拡張メソッドで悪さをしたければ、トップ レベルのクラス(名前空間なしのグローバルなクラス)に拡張メソッドを書く方がはるかにたちが悪いです。

で、内部犯であれば、レビューや単体テストをちゃんとしていればある程度は防げるはずです。
悪意を持って攻めるなら「数千行のコミットにしれっと混ぜ込む」とかも考えられますけども。

たいてい以下のような [Analyzer](../../../../study/csharp/misc/analyzer-generator.md#analyzer) を書いてしまえば対処できちゃいそうなんですよねぇ…

- 複数のファイルに `global using` を書けなくする
- 拡張メソッドを含む名前空間を `global using` できなくする
- `global using` した名前空間中の型名の被りに対して警告を出す

あと、`global using` は [Source Generator](../../../../study/csharp/misc/analyzer-generator.md#analyzer) で生成することもできます。
これが唯一の「プロジェクト外に影響を及ぼせる `global using`」になるんですが…
こちらはこちらで、「信用ならないパッケージを参照するのが怖いのは元から」ですし、
Source Generator を書ける人自体が割合そんなに多くないですし。

なんかこう、レビューをうまくすり抜けたり、「嫌な予感しかしないんだけどメリットもありそうでやむなく使う」みたいな邪悪さを出せないものかと悩み中…
