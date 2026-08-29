---
title: "ピックアップRoslyn: ラムダ式の改善"
source_url: "https://ufcpp.net/blog/2021/3/lambdaimprovements/"
content_type: "BlogEntry"
published_at: "2021-03-28T17:40:12"
updated_at: "2021-03-28T17:40:29"
tags: []
umbraco_id: 2345
parent_id: 2336
sort_order: 5
aliases: []
---

# ピックアップRoslyn: ラムダ式の改善

ラムダ式、これまでのバージョンでもこまごまと小さい改善があったりしたので今「lambda improvements」と言われてもタイトル的にはインパクト薄そうですが… C# 10.0 向けに結構大きな改善を入れようとしているみたいです。

- [Add proposal for lambda improvements #4451](https://github.com/dotnet/csharplang/pull/4451)

## <a id="background">背景</a>

C# 2.0 で[メソッドをデリゲート型の変数に代入するときに `new` が要らなくなった](../../../../study/csharp/functional/sp_delegate.md#definition)のとか、[匿名メソッド式](../../../../study/csharp/functional/sp_delegate.md#anonymous-method)が入り、C# 3.0 で[ラムダ式](../../../../study/csharp/functional/sp_delegate.md#lambda)が入って以来、C# 9.0 に至るまでずっと、デリゲートの型推論の向きはずっと[ターゲット型からの推論](../../../../study/csharp/start/misctyperesolution.md#target-type)になっています。

C# の文法にはソース型からの推論の方が多いので、デリゲート(特にラムダ式)のターゲット型推論の挙動を「何か変」と思う人は多いんじゃないかと思います。
一番多いのは、「以下のコードがコンパイルできないのは変じゃない？」みたいに思うこと。

```csharp {title="コンパイルできないことへの不満を目にすることが多いコード"}
var f = (int x) => x * x;
```

大体は、デリゲートが「同じ引数・戻り値でも別の型を作れるし、それらは互いに区別する」という仕様のせい。
ターゲットの方の型が決まらないとラムダ式の型を決定できません。

```csharp {title="同じ引数・戻り値で別の型なデリゲート" error-ranges="9:5-9:6,10:5-10:6,11:5-11:6"}
using System;
 
A a = (int x) => x * x;
B b = (int x) => x * x;
Func<int, int> f = x => x * x;
 
// A, B, Func<int, int> は同じ引数、同じ戻り値だけど別の型。
// 互いに代入不可。
a = b;
b = f;
f = a;
 
delegate int A(int x);
delegate int B(int x);
```

今でこそ「ターゲット型がわからないときは `Func<T, TResult>` にすればいいんじゃない？」みたいなことを言われますが…

確かに今なら「できる限りは `Func<T, TResult>` を使う」みたいな習慣ができていますが、
`Func<T, TResult>` が標準ライブラリに入ったの自体が C# 3.0 (後付け)だったりするので、
「デフォルトは `Func<T, TResult>`」というほど盤石な地位かと言うと微妙に悩んだりします。

あと、C# の[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)の制限のせいで、`Func` には以下の問題もあったりします。

- `Func<ref int, readonly ref int>` みたいな、`ref` (参照渡し)な型引数を作れない
- `Func<T1, ..., T16, TResult>` みたいな、引数の個数違いを1個1個書く必要があって、標準ライブラリで提供しているものは最大で16引数しかない

あと、まあ、ラムダ式の最大の用途である LINQ が「ターゲット型推論だけあれば十分」なのもあります。

```csharp {title="LINQ はターゲット型推論が効くのでラムダ式の型決定で困ることがない"}
using System.Linq;
 
var q = new[] { 1, 2, 3, 4, 5, 6 }
    .Where(x => (x % 2) == 0) // ターゲット型推論で Func<int, bool> に決定
    .Select(x => x * x);      // ターゲット型推論で Func<int, int> に決定
```

一方で、最近、 ASP.NET 方面から「[任意のデリゲートを受け付ける `Map` メソッドを作りたい](https://github.com/dotnet/aspnetcore/pull/30556)」という話が上がっています。

例えば、以下のような短い書き方で所定の URL に対するアクションを登録できるようにしたいそうです。

```csharp {title="任意デリゲートを受け付ける ASP.NET Map メソッド"}
builder.MapGet("/", () => { });
builder.MapGet("/category/{c}", (char c) => char.GetUnicodeCategory(c));
```

C# としてもこの路線は支持したいそうで、
そうなると3点ほどラムダ式に変更を入れたいとのこと。

- ラムダ式に属性を付けれるようにする
- ラムダ式の戻り値を(ターゲット型推論じゃなく、ラムダ式側で)明示できるようにする
- (ターゲット型がない時の) ラムダ式の「自然な型」を導入する

## <a id="attributes">属性</a>

[C# 9.0 でローカル関数に属性を付けれるようにした](../../../../study/csharp/cheatsheet/ap_ver9.md#local-function-attribute)わけで、ラムダ式にも属性を付けれるようにしてもいいんじゃないかという話があります。
あと、同じく C# 9.0 で[ラムダ式に `static` を付けても大丈夫だった](../../../../study/csharp/cheatsheet/ap_ver9.md#static-anonymous-function)ので、だったらさらに `[]` が付いても大丈夫っぽい。

名前通り「式」(どこにでも書ける構文)なのであんまり長いものは書きたくはないですが、
例えば以下のようなコードは十分に「書ける範囲」かと思います。

```csharp {title="ラムダ式に属性を付ける例"}
app.MapAction([HttpGet("/")]() => new Todo(Id: 0, Name: "Name"));
app.MapAction([HttpPost("/")]([FromBody] Todo todo) => todo);
```

C# 10.0 でこれらを認めたいそうです。

## <a id="explicit-returns">明示的な戻り値の型</a>

ラムダ式、C# 3.0 の頃からの構文でも、引数の型は明示できます。
一方で、戻り値の型は常に推論任せだったわけですが、いい加減明示する方法が欲しいそうです。

例えば戻り値の型を明示できなくて困る？例は以下の通り。

```csharp {title="戻り値の型を明示できなくて困る例" error-ranges="4:9-4:10"}
using System;
 
var a = m(x => x * x, 1); // これはターゲット型推論任せで int, int に決定。
var b = m((short x) => x * x, 1); // これが実は型決定できなかったり。
 
T2 m<T1, T2>(Func<T1, T2> f, T1 x) => f(x);
```

この例の場合は以下のように型引数を明示することで一応は解決しますが、これが書き心地いいかと言われると微妙な感じ。

```csharp {title="型引数を明示して型推論が働くように変更"}
var b = m<short, long>(x => x * x, 1);
```

ジェネリクスの場合は型引数の方を明示するという手段を取れますが、
後述する「自然な型」を導入しようと思うと戻り値の型の明示がないと困る場面が出てきます。
ということで、以下のような「引数リストの後ろに `: T` を書く」という文法で戻り値の型を明示できるようにしたいそうです。

```csharp {title="ラムダ式の戻り値の型を明示する例"}
var b = m((short x) : long => x * x, 1); // ラムダ式の戻り値の型を明示的に long にする。
```

## <a id="natural-type">デリゲートの自然な型</a>

最後が一番大きな変更になるんですが、ラムダ式を `var`、`object`、`Delegate` に代入できるようにするために、
「ターゲット型がわからないときはこの型を選択する」みたいなもの(natural delegate type: デリゲートの自然な型)を決めておくことにするそうです。

候補が搾れる分にはラムダ式やメソッドからデリゲートの型を自動決定。

ラムダ式の例:

```csharp {title="ラムダ式の自然な型"}
var f1 = () => default;        // error: no natural type (決定不能)
var f2 = x => { };             // error: no natural type (決定不能)
var f3 = x => x;               // error: no natural type (決定不能)
var f4 = () => 1;              // System.Func<int>
var f5 = () : string => null;  // System.Func<string>
```

メソッド グループの例:

```csharp {title="メソッド グループの自然な型"}
static void F1() { }
static void F1<T>(this T t) { }
static void F2(this string s) { }
 
var f6 = F1;    // error: multiple methods (F1() と F1<T>(T) の区別が付かない)
var f7 = "".F1; // System.Action
var f8 = F2;    // System.Action<string>
```

で、「自然な型」は以下のように作るそうです。

- `System.Action` か `System.Func` を使えるとき(`ref` とかが付いていなくて16引数以下のとき)はそれを選ぶ
- 非同期メソッドの場合、戻り値の型は `Task`/`Task<T>` (`System.Threading.Tasks` 名前空間)にする
- `System.Action` か `System.Func` を使えないときは、[匿名型](../../../../study/csharp/start/sp3_inference.md#anonymous)みたいに、 `internal` な匿名のデリゲートを作る

匿名の型を作ってしまうのは、ASP.NET の `MapAciton` みたいに `Delegate` に代入して `MethodInfo` を見て分岐する用途では十分そうです。

一方で、例えば以下のような `is` 分岐はできないはずなので、この用途には使えません。

```csharp {title="引数・戻り値の型が一致していても別デリゲート型になる"}
using System;
 
// これはターゲット型からの型推論で成り立っているので OK。
a((ref int x) => ref x);
 
void a(RefDelegate d) { }
 
// C# 10.0 でコンパイルできるようにしたい書き方。
// この行はコンパイルできるようになる。
// Func<ref int, ref int> とは書けないので、匿名型が作られる。
b((ref int x) => ref x);
 
void b(Delegate d)
{
    // (ref int x) => ref x は匿名のデリゲート型が作られてその型になる。
    // delegate ref int Anonymous1$(ref int x) みたいな「通常の C# コードでは書けない名前の型」になるはず。
    // 例え引数・戻り値の型が一致していても、RefDelegate とは違う型なので、is RefDelegate が true になることはない。
    if (d is RefDelegate r)
    {
    }
}
 
delegate ref int RefDelegate(ref int x);
```
