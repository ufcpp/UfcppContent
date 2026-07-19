---
title: "ピックアップRoslyn: using がらみ (using エイリアスの改善と global using)"
source_url: "https://ufcpp.net/blog/2021/3/usingimprovements/"
content_type: "BlogEntry"
published_at: "2021-03-15T01:02:08"
updated_at: "2021-03-20T20:49:34"
tags: []
umbraco_id: 2338
parent_id: 2336
sort_order: 1
aliases: []
---

# ピックアップRoslyn: using がらみ (using エイリアスの改善と global using)

今日も「C# Language Design Meeting 議事録」の中から1個1個機能紹介。

今日は[2/10](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-02-10.md#global-usings)、[2/22](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-02-22.md#using-alias-improvements)辺りの話になります。

[using](../../../../study/csharp/structured/sp_namespace.md#using)がらみに色々更新が掛かるみたいです。
大まかに2点。

- using エイリアス改善: これまで書けてもよさそうなのに書けないエイリアスを書けるようにする
- global using: プロジェクト全域に対して有効な `using` ディレクティブ

[global using の方は提案ドキュメントが merge 済み](https://github.com/dotnet/csharplang/blob/main/proposals/GlobalUsingDirective.md)、
[using エイリアスの話はレビュー中](https://github.com/dotnet/csharplang/pull/4452)です。

## using エイリアス改善

これも細かく言うと3点。

- キーワードになってる型を直接使えるようにする
- 配列の `[]`、nullable の `?`、ポインターの `*`、タプルとかを使えるようにする
- 型引数を持てるようにする

今でも OK なパターンだと以下のような書き方ができます。

```csharp
using Ok1 = System.Int32;
using Ok2 = System.Nullable<int>;
```

ところが以下のようなやつは現状ではコンパイル エラー。
ジェネリック型引数の中なら `int` を書けるのに、直接は書けない。

```csharp
using Ng1 = int;
using Ng2 = int?;
```

以下のようなやつもコンパイル エラー。
配列の `[]`、nullable の `?`、ポインターの `*`は現状書けません。

```csharp
using Ng3 = System.Int32?;
using Ng4 = System.Int32[];
using Ng5 = System.Int32*;
```

あと、頻出で出ている要望がタプルで、
以下のようなやつも「書きたいのに書けない」筆頭です。

```csharp
using Ng6 = (System.Int32, System.String); // これがダメな時点でお察しだけど…
using Ng7 = (int, string); // ほんとに書きたいのはこうだし、
using Ng8 = (int id, string name); // 名前付きタプルも書きたい。
```

この辺り、C# 10.0 でまとめて解消しようという感じになっています。
ちなみに、似たような話だと、`enum` の基底にキーワードを書けるかどうかみたいなのが C# 6.0 の時に変わっています。

```csharp
// これなら C# 1.0 の頃から書けた。
enum A : System.Int32 { }
 
// これが書けるようになったのは C# 6.0 から。
enum B : int { }
```

タプルのエイリアスを付けれるようにしようとなると、まあ、「ジェネリックなエイリアス」も作りたくなります。
これもこの際、C# 10.0 で一緒にやるそうです。

```csharp
using Fix2<T> = (T, T);
using Fix3<T> = (T, T, T);
using Fix4<T> = (T, T, T, T);
```

もちろんタプル以外の「ジェネリックなエイリアス」も同じく C# 10.0 で取り組み。

```csharp
using Option<T> = T ?;
```

「部分適用」もできるようにしたいみたいです。
以下のような、「2引数のうち片方だけ確定」みたいな「ジェネリックなエイリアス」も作れるようにする予定です。

```csharp
using StringDictionary<T> = System.Collections.Generic.Dictionary<string, T>;
```

arity (型引数の数)違いのエイリアスは並べられるようにする予定だそうです。

```csharp
using MyDictionary = System.Collections.Generic.Dictionary<string, string>;
using MyDictionary<T> = System.Collections.Generic.Dictionary<string, T>;
using MyDictionary<T1, T2> = System.Collections.Generic.Dictionary<T1, T2>;
```

ちなみに、以下のようなオープン ジェネリック(引数なしの状態)は C# 10.0 でも書けません。

```csharp
// これは引き続き今後もダメ。
// 空っぽの <> が許されるのは typeof(T<>) だけ
using OpenGeneric = System.Collections.Generic.List<>;
```

これをやるうえでちょっと悩ましいのが、「制約違反」みたいなのをどこで判定するか。
選択肢は2つあって、1つ目はエイリアスを作る時点 (`using T = ...;` の行) でエラーにする方法。
エイリアスを「実際にある型」に近い扱いにしようという感じ。
(現状あんまり乗り気ではなさげ。)

```csharp
using Optional<T> = Nullable<T>; // 「T に制約が付いてないのでダメ」扱いする
using Optional<T> = Nullable<T> where T : struct // と言うことはここに型制約(where)を書けるようにする必要あり
```

もう1つの選択肢は、「エイリアスの時点では素通し」で、現状、こっちが有力みたいです。
「エイリアスはあくまでエイリアス」で、C 言語のマクロっぽい挙動というか。

```csharp
using Optional<T> = Nullable<T>; // この時点では T のチェックしない。
 
// Nullable<string> とは書けないので、そのエイリアスの Optional<string> もダメ。
void m(Optional<string> opt) { }
```

後者が有力なので、`using A<T> = T?;` が [null 許容値型](../../../../study/csharp/resource/sp2_nullable.md)になるか、
[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)になるか、
[defaultable](../../../../study/csharp/resource/nullablereferencetype.md#unconstrained-generics)になるかはおそらく利用側次第になります。

## global using

プロジェクト全域に影響を及ぼす `using` ディレクティブを書きたいという要望も昔からちらほらあります。

これはまあ、「全域に影響を及ぼす」ってのが怖くてやってなかっただけなんですが、
それももう今更なのかなぁという感じになっています。
と言うのも、

- [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)は `<Nullable>enable</Nullable` オプションを与えるとプロジェクト全体で有効・無効が切り替わる
- [SkipLocalsInit](../../../../study/csharp/cheatsheet/ap_ver9.md#skip-locals-init) は `[module:SkipLocalsInit]` と書けばプロジェクト全体に影響を及ぼせる

みたいな文法がすでにあります。

あと、 ASP.NET なプロジェクトを作るとテンプレート内に `_ViewImports.cshtml` っていうのが最初から存在しますが、その中身は以下のようになっています。

```razor
@using WebApplication1
@using WebApplication1.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

これ、やってることはまさに「プロジェクト中のすべての cshtml に影響を及ぼす `using`」になります。

あと例えば、最近の [C# 公式チュートリアル](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/tutorials/hello-world?tutorial-step=1)では、普通に以下のコードがコンパイルできたりします。
どうも、暗黙的に、`System`、`System.Linq`、`System.Collections.Generic` 辺りがデフォルトで `using` されていそうな雰囲気。

```csharp
Console.WriteLine("Hello World!");
```

ちなみに、現時点での実装はどうも「ごり押し」っぽい雰囲気があります。
[Visual Studio Users Community Japan 勉強会 #6 質疑応答枠 1:12:18～](https://www.youtube.com/watch?v=yDrQ2nCPfR8&t=4338s)で話したことがあるですが、
おそらく、「書いたコードの前に以下のコードを追加」みたいな実装になっていると思います。

```csharp
using System;
using System.Linq;
using System.Collections.Generic;
 
class Program
{
    static void Main()
    {
```

相当に気持ち悪い実装ですが、
こんな気持ち悪いことをしてまで、「`using` はおまじない」を消したいという状態になっています。

だったら認めようと。

問題は実現方法なんですが、これも初期案としては2案出ていました。

- `<Nullable>enable</Nullable>` みたいに、C# コンパイラーに渡すオプション/csproj ファイルに書く設定として提供
- C# ソースコード中に `global using N = int;` みたいなのを書けるようにする

ちなみに、後者が有力になっています。`global using N = int;` 支持になっているのは以下のような理由。

- Source Generator で使う場合に C# コードで書ける方が助かる
- dotnet コマンドから csc (C# コンパイラー)に素通ししてあげないといけないオプションがすでに大量にあってあんまりもう増やしたくない
- 「global using が欲しい」という要望も長年ずっと出続けてる

で、後は文法なわけですが、`global using` で行くみたいです。

```csharp
global using System;
global using System.Linq.Enumerable;
global using System.Collections.Generic;
global using static System.Linq.Enumerable;
```

まあ、迷うとしたら語順くらいですかね。
普通に `global` という単語を名前空間にもクラス名にも使えてしまうので、`using global` だと「キーワードの `global` か名前空間の `global` か」の弁別が大変だそうで。

```csharp
using global;
 
namespace global
{
    class global { }
}
```

あと、さすがにファイル中散り散りに「プロジェクト全体に影響あり」なものが書かれるのは怖いということで、
`global using` を書けるのはファイルの先頭(普通の `using` ディレクティブよりも前)だけにするみたいです。
