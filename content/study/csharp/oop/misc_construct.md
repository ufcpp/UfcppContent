---
title: "[雑記] コンストラクター内の仮想メソッド呼び出し"
source_url: "https://ufcpp.net/study/csharp/oop/misc_construct/"
content_type: "Article"
published_at: "2007-10-06T00:00:00"
updated_at: "2023-11-04T00:00:00"
tags: []
umbraco_id: 1266
parent_id: 1248
sort_order: 14
aliases:
  - "/csharp/misc_construct"
  - "/csharp/misc_construct.html"
  - "/csharp/oop/misc_construct/"
  - "/study/csharp/misc_construct"
  - "/study/csharp/misc_construct.html"
---

# \[雑記\] コンストラクター内の仮想メソッド呼び出し

## <a id="sec-generated-title-1"></a> <a id="abst">概要</a>

継承構造を持つクラスのコンストラクターの挙動と注意点の話を少々。


## <a id="sec-generated-title-2"></a> <a id="ctor-order">コンストラクターの実行順序</a>

派生クラスのインスタンスが生成される際、
派生クラスのコンストラクターの前に、基底クラスのコンストラクターが呼び出されます。

```csharp
// コンストラクター呼び出し。
new D();

class B
{
    public B() => Console.WriteLine("base");
}

class D : B
{
    public D() => Console.WriteLine("derived");
}
```


```console
base
derived
```


なので、派生クラスのコンストラクター内では、
基底クラスのメンバーはちゃんと初期化済みだと思って使えます。

```csharp
var d = new D();
Console.WriteLine(d.Y); // 25

class B
{
    public double X;
    public B() => X = 5;
}

class D : B
{
    public double Y;

    // B() の実行が先。X は 5 になってる。
    // ↓ ちゃんと y == 25 になる。
    public D() => Y = X * X;
}
```


```console
25
```



## <a id="sec-generated-title-3"></a> <a id="virtual">仮想メソッド呼び出し</a>

「派生クラスのコンストラクターの前に基底クラスのコンストラクターが呼ばれる」というルールは、
たいていどの言語でも同じルールです。
C++ でも Java でもそういうルールでコンストラクターを呼び出します。

でも、1つだけ注意すべき点があります。
コンストラクター中の仮想メソッド呼び出しの扱いに関して。
例えば、以下のような感じ。

```csharp
new D(); // C# のルールだと "derived" の方が表示される。

class B
{
    public B() => Console.WriteLine(Name());

    public virtual string Name() => "base";
}

class D : B
{
    public override string Name() => "derived";
}
```


この類のコードの挙動は C++ と C# で違います。
C++ で、このコードに相当するものを書いて実行すると、
base と表示されます。
派生クラス D のインスタンスを生成しているにもかかわらず、
基底クラス B の Name メソッドが呼ばれます。

```console
base
```


一方、C# では、以下のように、派生クラスの Name メソッドが呼ばれます。

```console
derived
```


仮想メソッド（あるいは、C++ では仮想関数と呼ぶ）の呼び出しは、
仮想メソッドテーブル（仮想関数テーブル）というものを通して行います。
Name というメソッドが呼ばれたときに、
実際にはどのメソッド（D.Name なのか B.Name なのか）を呼べばいいか、
テーブル中に参照情報が書かれていて、
それを見て実際のメソッド呼び出しが行われます。

で、C++ では、コンストラクターの頭で仮想関数テーブルが更新されます。
基底クラスのコンストラクター内では、まだ仮想関数テーブルが派生クラスのものに更新されていません。

一方、C# では、仮想メソッドテーブルの更新だけは先にして、
それから基底クラスのコンストラクター → 派生クラスのコンストラクターの順で処理が行われます。


### <a id="sec-generated-title-4"></a> <a id="order">余談： 初期化子とコンストラクターの実行順序</a>

「[コンストラクター初期化子](oo_construct.md#initializer)」で説明したように、
初期化の順序は、

1. 派生クラスのメンバー初期化子
2. 基底クラスのメンバー初期化子
3. 基底クラスのコンストラクター本体
4. 派生クラスのコンストラクター本体

という順序になります。
以下のようなコードを書くと実行順序がはっきりします。

```csharp
// コンストラクター呼び出し。
new Derived();

class Member
{
    public Member(string s) => Console.WriteLine($"Member {s}");
}

class Base
{
    public Member X = new("base"); // 2.

    public Base() => Console.WriteLine("Base()"); // 3.
}

class Derived : Base
{
    public Member Y = new("derived"); // 1.

    public Derived() => Console.WriteLine("Derived()"); // 4.
}
```


```console
Member derived
Member base
Base()
Derived()
```


で、メンバー変数初期化子を使って値を設定した変数は、
基底クラスのコンストラクターが呼ばれた時点ですでにきちんと初期化済みな事が保証されます。
基底クラスから仮想メソッドを呼び出す場合、
このことに留意してコードを書くとトラブルになりにくいです。


## <a id="sec-generated-title-5"></a> <a id="problem">コンストラクター中での仮想メソッド呼び出しの問題点</a>

基底クラスのコンストラクター内から仮想メソッドを呼んだとき、
ちゃんと動的な型に基づいて派生クラスのメソッドが呼ばれるわけですが、
この動作には1つ問題があります。
基底関数のコンストラクター内で仮想メソッドが呼ばれた時点では、
派生クラスのメンバー変数は初期化されていない（派生クラスのコンストラクターはまだ呼ばれてない）んですよね。
例えば、以下のコードを見てください。

```csharp
// コンストラクター呼び出し。
new D("derived");

class B
{
    public B() => Console.WriteLine(Name()); // D() の中身より先に実行される。

    public virtual string Name() => "anonymous";
}

class D : B
{
    private string _name;

    public D(string name) => _name = name;

    public override string Name() => _name; // D() 実行前に呼ばれるとまだ _name の初期化が終わってない。
}
```


前節の内容と比べて何が違うかというと、D.Name メソッド内で派生クラスのメンバー変数である name の値を読み出しています。

B のコンストラクター内で Name メソッドが呼ばれた時点では、
まだ D のコンストラクターは実行されていません。
したがって、name 変数はまだ初期化されていない（null になっている）状態で、
結局、このコードの実行結果は何も出力されません。

### <a id="sec-generated-title-6"></a> <a id="primary-constructor">プライマリ コンストラクターでの解決</a>

<h5 class="version version12">Ver. 12</h5>

ちなみにこの問題はプライマリ コンストラクターで解決できたりします。

「メンバー初期化子は派生クラスの方が先に実行される」という仕様で、
プライマリ コンストラクターの場合はメンバー初期化子を使うことになるので、
初期化処理が実行されるタイミングが早くなります。

```csharp
// コンストラクター呼び出し。
new D("derived");

class B
{
    public B() => Console.WriteLine(Name()); // D のメンバー初期化子よりは後に実行される。

    public virtual string Name() => "anonymous";
}

// 先ほどのコードのコンストラクターをプライマリ コンストラクター形式に変更。
class D(string name) : B
{
    private string _name = name; // フィールド初期化子になったことで、実行タイミングが早くなる。

    public override string Name() => _name; // B() 実行前に _name = name が呼ばれてて、期待通りの動作になる。
}
```

```console
derived
```


## <a id="sec-generated-title-7"></a> <a id="summary">まとめ</a>

C# では、コンストラクター内での仮想メソッド呼び出しは、
動的な型に基づいて呼び出されます。

ただし、メンバー変数を読み出すような仮想メソッドをコンストラクター内から呼び出すと、
正しい値が読めないので注意が必要です。
（メンバー変数にアクセスしない場合や、値を書き込む方は OK。）

C# 12 で追加されたプライマリ コンストラクターでは、通常のコンストラクターと比べてメンバーの初期化タイミングが早くなる点にも注意が必要かもしれません。
