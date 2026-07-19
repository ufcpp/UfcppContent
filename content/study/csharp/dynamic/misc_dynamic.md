---
title: "[雑記] 動的コード生成のパフォーマンス"
source_url: "https://ufcpp.net/study/csharp/dynamic/misc_dynamic/"
content_type: "Article"
published_at: "2010-08-14T00:00:00"
updated_at: "2015-08-26T09:12:49"
tags:
  - "Ver. 4.0"
umbraco_id: 1318
parent_id: 1312
sort_order: 5
aliases:
  - "/csharp/dynamic/misc_dynamic/"
  - "/csharp/misc_dynamic"
  - "/csharp/misc_dynamic.html"
  - "/study/csharp/misc_dynamic"
  - "/study/csharp/misc_dynamic.html"
---

# \[雑記\] 動的コード生成のパフォーマンス

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

.NET Framework のバージョンが上がるたびに色々と追加され、
今や、動的コード生成にもさまざまなやり方が。
ということで、並べて比較してみたいと思います。

「動的 ＝ リフレクション ＝ むちゃくちゃ遅い」というイメージをもたれる方も多いと思いますが、
実際のところ、1度生成したコードをキャッシュしておくなどの工夫をすれば、意外と許容範囲なパフォーマンスが得られます。

（GUI の描画部分やネットワーク通信部分の遅延と比べれば、演算部分の数倍程度の差は取るに足らない場合が多く、
過剰に気にする必要はありません（もちろん、状況次第）。）

比較コード含めたソースコード一式： 
[DynamicPerformance.zip](../../../../assets/media/ufcpp2000/csharp/source/DynamicPerformance.zip)



##### <a id="sec-generated-title-2"></a>ポイント

* 毎回リフレクションを呼び出すのはやっぱりかなり（2～3桁）遅い。

* キャッシュ機構を使えば、静的なコードの数倍程度までは速くできる。

* C# 4.0 の dynamic は適切にキャッシュしてくれているので、十分速い。



## <a id="sec-generated-title-3"></a> <a id="methodology"></a>比較の仕方

以下のような操作をベースに。

```csharp
public class Static
{
    public static int Sum(Point p)
    {
        return p.X + p.Y;
    }
}

public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
```


これを動的実行できるように変更していきます。
（Point 以外の型であっても、int 型の2つのプロパティ X, Y を持っていればなんでも受け付けるようにする。）


## <a id="sec-generated-title-4"></a> <a id="reflection"></a>リフレクション

リフレクションを使って、毎度 type.GetProperty するやり方。

```csharp
public class Refrection
{
    public static int Sum(object p)
    {
        var t = p.GetType();
        var propX = t.GetProperty("X");
        var propY = t.GetProperty("Y");
        var x = propX.GetValue(p, null);
        var y = propY.GetValue(p, null);
        return (int)x + (int)y;
    }
}
```



##### <a id="sec-generated-title-5"></a>利点

* .NET Framework 1.0 でも動く。

* 以上。



##### <a id="sec-generated-title-6"></a>欠点

* 遅い。文字通り、桁外れに遅い。

* 静的なバージョンと比べてだいたい2～3桁遅い。「倍」じゃなくて「桁」って単位で遅い。


正直、今この選択肢はないと思う。


## <a id="sec-generated-title-7"></a> <a id="ilgenerator"></a>IL 生成

<h5 class="version version2">Ver. 2.0</h5>

.NET Framework 2.0 で、DynamicMethod を使った動的コード生成ができるようになりました。
ただし、やってることは MSIL レベルでのコード生成なので、アセンブリ言語でコードを書くような感じになります。

ポイントは、一度生成したコードをキャッシュとして持っておくことです。
さすがに、動的コード生成自体はそれなりに重たい処理なので、
キャッシュしなければあまりいいパフォーマンスはでません。

```csharp
public class GenerateIL
{
    private static Dictionary<Type, Func<object, int>> cache = new Dictionary<Type,Func<object,int>>();

    public static int Sum(object p)
    {
        var t = p.GetType();
        Func<object, int> d;

        if (!cache.TryGetValue(t, out d))
        {
            d = CreateMethod(t);
            cache[t] = d;
        }

        return d(p);
    }

    private static Func<object, int> CreateMethod(Type t)
    {
        DynamicMethod dm = new DynamicMethod("Sum", typeof(int), new[] { typeof(object) });
        ILGenerator il = dm.GetILGenerator();

        LocalBuilder x = il.DeclareLocal(t);

        // var x = (Point)p;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Castclass, t);
        il.Emit(OpCodes.Stloc, x);

        // p.X
        il.Emit(OpCodes.Ldloc, x);
        il.EmitCall(OpCodes.Callvirt, t.GetProperty("X").GetGetMethod(), null);

        // p.Y
        il.Emit(OpCodes.Ldloc, x);
        il.EmitCall(OpCodes.Callvirt, t.GetProperty("Y").GetGetMethod(), null);

        // +
        // ちなみに、今回の場合は「p.X は必ず int」という前提なのでコード生成は楽だけど、
        // もしこれがユーザー定義型で、 + もユーザー定義の operator+ だったら、OpCodes.Add じゃなくて Call に変えなきゃいけない。
        // かなり面倒。
        il.Emit(OpCodes.Add);

        il.Emit(OpCodes.Ret);

        var f = typeof(Func<,>);
        var gf = f.MakeGenericType(typeof(object), typeof(int));
        var d = (Func<object, int>)dm.CreateDelegate(gf);

        return d;
    }
}
```



##### <a id="sec-generated-title-8"></a>利点

* コード生成はそれなりに速い。

* .NET Framework 2.0 で使える。



##### <a id="sec-generated-title-9"></a>欠点

* アセンブリ言語を書ける人でないと無理。

* 自分でキャッシュの仕組みを書かなきゃいけない。


静的なバージョンと比べた場合、数倍程度の遅さで済みます。
（ただし、今回のようなメソッドの中身が単純なケースでは、静的なバージョンはインライン展開されることで相当に高速化されます。
この最適化まで見越すと、静的バージョンと動的バージョンで1桁程度の差になる場合もあります。）

ただ、アセンブリ言語で書くようなものなのが相当にきつく、かなり「最後の手段」。
「コード生成はそれなりに速い」といっても、コード生成の部分が呼ばれるのは最初の1回きりなので、
そこがいくら速かろうと、全体としては大した影響にはならず。
また、後述する式木と比べて「何倍」とかいうオーダーで速いわけではないです。
.NET Framework 3.5 以降が使えるのであれば、あえて使う必要性はないと思います。


## <a id="sec-generated-title-10"></a> <a id="expressiontree"></a>式木

<h5 class="version version3">Ver. 3.0</h5>

.NET Framework 3.5 （C# 3.0）で「[式木](sp3_expression.md#expressiontree)」というものが導入されました。
（ただし、一部の機能は .NET Framework 4 で追加されものなので、この例の場合は .NET 4 が必須になります。）

IL レベルではなく、構文木レベルでコードを生成できるので、IL Generator と比べればだいぶマシな動的コード生成ができます。
ただ、キャッシュの仕組みが必要なのは IL Generator と同様です。

```csharp
public class ExpressionTree
{
    private static Dictionary<Type, Func<object, int>> cache = new Dictionary<Type, Func<object, int>>();

    public static int Sum(object p)
    {
        var t = p.GetType();
        Func<object, int> d;

        if (!cache.TryGetValue(t, out d))
        {
            d = CreateMethod(t);
            cache[t] = d;
        }

        return d(p);
    }

    private static Func<object, int> CreateMethod(Type t)
    {
        var x = Expression.Parameter(typeof(object));
        var p = Expression.Parameter(t);

        /*
          * {
          *     T p;
          *     p = (T)x;
          *     return p.X + p.Y;
          * }
          */
        var exp = Expression.Lambda(
            // { T p;
            Expression.Block(
                new[] { p },
                // p = (T)x;
                Expression.Assign(p, Expression.Convert(x, t)),
                // p.X + p.Y
                // IL 生成と違って、ユーザー定義の operator+ でも正しくコード生成してくれる。
                Expression.Add(
                    Expression.Property(p, "X"),
                    Expression.Property(p, "Y"))),
            x
            );

        var d = (Func<object, int>)exp.Compile();
        return d;
    }
}
```



##### <a id="sec-generated-title-11"></a>利点

* IL 生成と比べれば随分素直に書ける。



##### <a id="sec-generated-title-12"></a>欠点

* .NET Framework 4 必須（限定的に、.NET 3.5 で実現可能な部分もあり）。

* 自分でキャッシュの仕組みを書かなきゃいけない。


おそらく、自前での動的コード生成を考えるなら、これが最も簡単な方法になります。

パフォーマンスもそれなりによく、IL 生成するものと比べてそう大きな差はありません。
静的なバージョンと比べた場合、数倍程度の遅さで済みます。


## <a id="sec-generated-title-13"></a> <a id="dynamic"></a>dynamic

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 で dynamic が導入されたわけですが、
内部的な挙動が見えないので色々誤解されがちだったり。

dynamic は、内部的には「式木による動的コード生成」＋「生成したコードのキャッシュ」を行っています。
したがって、式木版に近いパフォーマンス（静的なバージョンの数倍程度）が出ます。

汎用性を重視していたり、コンパイラの自動生成にお任せする部分も多く、
自前で式木を組むよりも少々遅くなりがちです。
ただし、結構高度なキャッシュの仕組みを持っていて、
適用される型が多くなってくると、ひょっとすると自前でキャッシュ機構を持つよりも、
dynamic 任せの方が高速な場合も出て来ると思います。

```csharp
public class Dynamic
{
    public static int Sum(dynamic p)
    {
        // GenerateIL とか ExpressionTree 版が、「p.X の戻り値は int」みたいな前提でコード書いちゃったので、
        // 以下のように書かないと比較がフェアじゃないかなと。
        // （厳密には、これでもまだこちらの方が不利）
        return (int)p.X + (int)p.Y;
    }
}
```



##### <a id="sec-generated-title-14"></a>利点

* 超お手軽。

* 動的コード生成もキャッシュも全自動。



##### <a id="sec-generated-title-15"></a>欠点

* Type 型を（メソッドの引数で受け取るとか）明示的に渡せない。

* .NET Framework 4 必須。


この例の場合だと、パフォーマンス的には式木版と比べて数％程度の劣化でした。
これだけお手軽で、たった数％程度の劣化しかないならかなりの利便性の良さだと思います。

ただし、Type 型を引数として渡して動的コード生成したりはできないので、
そういう場合は後述する CallSite（dynamic を実現するための内部的なクラス）を直接利用することになります（結構面倒）。


## <a id="sec-generated-title-16"></a> <a id="callsite"></a>CallSite（dynamic の内部挙動）

<h5 class="version version4">Ver. 4.0</h5>

C# 4.0 の dynamic の内部挙動的には、CallSite というクラスを使っています。
参考までに、上述の dynamic の例を、CallSite を直接使って書き直したものも例示しておきます。

例示のついでに、
多少最適化を掛けています。
（if を1つにまとめたり、キャスト用の CallSite を1つにまとめたり。）
式木版と遜色ないというか、むしろ式木版よりもちょっと速いくらいになりました。

```csharp
public class DynamicInside
{
    static CallSite<Func<CallSite, object, int>> siteCast;
    static CallSite<Func<CallSite, object, object>> siteGetX, siteGetY;

    public static int Sum(object p)
    {
        if (siteCast == null)
        {
            const CSharpBinderFlags convert = CSharpBinderFlags.ConvertExplicit;
            const CSharpBinderFlags none = CSharpBinderFlags.None;
            var argInfo = new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) };

            siteGetX = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(none, "X", null, argInfo));
            siteGetY = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(none, "Y", null, argInfo));
            siteCast = CallSite<Func<CallSite, object, int>>.Create(Binder.Convert(convert, typeof(int), null));
        }
        return (siteCast.Target(siteCast, siteGetX.Target(siteGetX, p)) + siteCast.Target(siteCast, siteGetY.Target(siteGetY, p)));
    }
}
```



##### <a id="sec-generated-title-17"></a>利点

* やってみたら、動的実行版の中では最速だった（今回の例の場合）。



##### <a id="sec-generated-title-18"></a>欠点

* 結構複雑で、お手軽とはいかない。

* .NET Framework 4 必須。


ちなみに、作成手順的には、
「dynamic を使ったコードを書く」→「逆コンパイル」→「コードをコピーしてきてそのあと最適化」とやっています。
でないと書ける気しません。

とはいえ、IL 生成するよりはだいぶマシですし、
キャッシュ機構は CallSite にお任せできるので、
ありといえばありな選択肢。


## <a id="sec-generated-title-19"></a> <a id="dictionary"></a>おまけ： 辞書構造

参考実装。

動的にやるんであれば、CLR オブジェクトとか使わず最初から辞書（ハッシュテーブル）にでもすりゃいいんじゃね？
という短絡的実装。

これだけシンプルなメソッドですら、dynamic より倍くらい遅い。
プロパティの数が増えたり、文字数が長くなるとみるみるパフォーマンスが落ちるおまけ付き。

さすがに毎回リフレクション呼ぶよりは1桁以上速いですが。

```csharp
public class PropertyDictionary
{
    public static int Sum(Dictionary<string, int> p)
    {
        return p["X"] + p["Y"];
    }
}
```



##### <a id="sec-generated-title-20"></a>利点

* 単純と言えば単純。



##### <a id="sec-generated-title-21"></a>欠点

* CLR オブジェクトを渡せない。

* やっぱり遅い。


古来、動的言語のオブジェクトは基本的にこの辞書みたいな構造になっています。
もちろん、最近は最適化手法が考えられていて、
CallSite と同じような動的メソッドキャッシュの仕組みを実装しています。
（実装によって動的言語のパフォーマンスがずいぶんと変わるのは、こういう部分の差も幾分かあります。）
