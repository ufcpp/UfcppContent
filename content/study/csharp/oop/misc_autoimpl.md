---
title: "[雑記] 抽象定義と自動実装"
source_url: "https://ufcpp.net/study/csharp/oop/misc_autoimpl/"
content_type: "Article"
published_at: "2011-08-07T00:00:00"
updated_at: "2021-02-21T18:01:59"
tags: []
umbraco_id: 1271
parent_id: 1248
sort_order: 17
aliases:
  - "/study/csharp/misc_autoimpl.html"
---

# \[雑記\] 抽象定義と自動実装

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

C# の文法では、メソッドやプロパティなどの抽象定義（abstract 修飾子が付いているもの）と自動実装（コンパイラーが具体的な実装を生成してくれるもの）の見た目が似ているため、
少し混乱しやすいです。


##### <a id="sec-generated-title-2"></a>ポイント

* 抽象定義 … インターフェイスのメンバーや抽象メソッドのように、宣言のみの（規約だけ定めて、実装を持たない）もの。

* 具象定義 … 通常のメソッドやプロパティのように、実装まで書いてあるもの。

* 自動実装 … プロパティとイベントは、実装を省略して、コンパイラーに自動生成してもらうことが可能。この際、抽象定義と見た目が近いので注意。



## <a id="sec-generated-title-3"></a> <a id="auto-property"></a>自動実装プロパティ

C# では、プロパティとイベントの場合、実装を省略して書くことで、コンパイラー任せで自動的に実装を作ることができます。
参考:

* 「[自動プロパティ](oo_property.md#auto)」

* 「[イベント](../functional/sp_event.md#event)」


便利な機能ですが、抽象定義（abstract 修飾子が付いていたり、インターフェイス中に定義されるメンバー）の書き方と似ているため注意が必要です。

例えば、プロパティの場合、以下のようになります。

```csharp {title="抽象プロパティと自動実装プロパティ"}
interface ISample
{
    // 抽象定義: これは宣言のみで実装を持たない（規約のみを定める）
    int A { get; set; }
}

abstract class Sample
{
    // 抽象定義: これも宣言のみで、実装を持たない
    public abstract int A { get; set; }

    // 具象定義: これは自動実装プロパティ（コンパイラー生成の実体を持つ）
    public int X { get; set; }
}
```


単純に、abstract が付いているか、もしくはインターフェイス内にあれば抽象定義（宣言）です。
その他の場合は自動実装プロパティになります。

ちなみに、X の自動実装の展開（コンパイラーによる自動生成）結果は以下のようになります。
（A の側は抽象定義なので、このような展開は起こりません。）

```csharp {title="自動実装プロパティ X の展開結果"}
    public int X
    {
        get { return _X; }
        set { _X = value; }
    }

    // 実際には、プログラマーに見えない特殊な名前が与えられます
    private int _X;
```


自動的に生成されたフィールド（この例でいう <code>_X</code>）をバック フィールド（backing field）と呼びます。
通常、C# コードからバック フィールドは見えなくなっていますが、
「[リフレクション](../dynamic/sp_reflection.md#reflection)」を使うことで覗き見ることができます。

```csharp {title="自動実装プロパティのバック フィールド"}
using System;
using System.Reflection;

abstract class Sample
{
    public abstract int A { get; set; }
    public int X { get; set; }
}

class Program
{
    static void Main()
    {
        var fields = typeof(Sample).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            Console.WriteLine(field.Name);
        }
    }
}
```


結果として表示されるのは X プロパティのバック フィールドになります。
（繰り返しますが、A の方は抽象プロパティなので、同様のフィールドは生成されません。）

```console {title="自動実装プロパティのバック フィールド"}
<X>k__BackingField
```


見ての通り、通常の C# コードからは定義できない名前（&lt; から始まる名前）になっています。
（「[IL](../abstract/ab_dotnet.md#il)」的には有効な名前です。）


## <a id="sec-generated-title-4"></a> <a id="auto-event"></a>自動実装イベント

プロパティは get; set; を明示的に書くだけまだましで、イベント構文はより一層、混乱を招きます。

```csharp {title="抽象イベントと自動実装イベント"}
interface ISample
{
    // 抽象定義: 宣言のみ、実装ない
    public event Action<int> A;
}

class Sample
{
    // 抽象定義: 宣言のみ、実装ない
    public abstract event Action<int> A;

    // 具象定義: 自動実装イベント
    public event Action<int> X;

    // ↑パッと見、デリゲート型のフィールドに見えるのがまた（別物です）
    Action<int> x;
}
```


プロパティ同様、abstract が付いているか、インターフェイス中で定義されている場合には抽象定義、それ以外は自動実装イベントです。

デリゲート型のフィールドと似て見える点にも注意が必要ですが、
実際には以下のようなアクセサーが自動生成されています。

```csharp {title="自動実装イベントの展開結果"}
    // 簡易版。本当はこれに、スレッド安全性の保証用コードが入る。
    public event Action<int> X
    {
        add { _X = (Action<int>)Delegate.Combine(_X, value); }
        remove { _X = (Action<int>)Delegate.Remove(_X, value); }
    }

    // バック フィールド
    // C# の文法上は認められていないのでここでは _X で代用したものの、
    // 実際にはイベントと同名のフィールド（この例の場合 X）が作られる。
    // （IL 的には OK。）
    private Action<int> _X;
```


イベントの場合、クラス内から普通のデリゲート型のフィールドのように扱えますが、
これは実際には、自動実装によって生成されたバック フィールドへのアクセスになります。

```csharp {title="クラス内からのイベント利用"}
using System;

class Sample
{
    public event Action<int> X;

    public void RaiseX(int value)
    {
        var d = X; // 実はこの X は自動生成されたバック フィールドの X
        if (d != null) d(value);
    }
}

class Program
{
    static void Main()
    {
        var s = new Sample();
        s.X += x => { }; // この X はイベントの X
    }
}
```


実際、
「[リフレクション](../dynamic/sp_reflection.md#reflection)」を使うことでバック フィールドを確認できます。

```csharp
using System;
using System.Reflection;

abstract class Sample
{
    public abstract event Action<int> A;
    public event Action<int> X;
}

class Program
{
    static void Main()
    {
        var fields = typeof(Sample).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var field in fields)
        {
            Console.WriteLine(field.Name);
        }
    }
}
```


X とだけ表示されます（イベントと同名のフィールドが生成されている）。
（前述のプロパティの例と同様、A の方は抽象イベントなので、同様のフィールドは生成されません。）

```console {title="自動実装プロパティのバック フィールド"}
X
```
