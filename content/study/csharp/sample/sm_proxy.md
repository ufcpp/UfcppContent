---
title: "[サンプル] 透過プロキシ"
source_url: "https://ufcpp.net/study/csharp/sample/sm_proxy/"
content_type: "Article"
published_at: "2008-03-09T00:00:00"
updated_at: "2015-05-06T14:13:26"
tags: []
umbraco_id: 1369
parent_id: 1359
sort_order: 9
aliases:
  - "/csharp/sample/sm_proxy/"
  - "/csharp/sm_proxy"
  - "/csharp/sm_proxy.html"
  - "/study/csharp/sm_proxy"
  - "/study/csharp/sm_proxy.html"
---

# \[サンプル\] 透過プロキシ

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[実行時型情報](../dynamic/sp_reflection.md)」のサンプルがちょっと不足してるなぁと思って作ったもの。

.NET Framework では、RealProxy というクラスを使って、
インターフェースのメソッド呼び出しを横取りして独自の処理に置き換えることができます。

* 
[ソース一式（ZIP 形式）](../../../../assets/media/ufcpp2000/csharp/source/MulticastProxy.zip)




## <a id="sec-generated-title-2"></a> <a id="realproxy"></a>RealProxy

例えば、マルチキャストデリゲートのようなことをインターフェースのメソッド呼び出しに対して行うようなプロキシ。

RealProxy クラスを継承して、Invoke メソッドをオーバーライドするだけ。

```csharp
public class MulticastProxy<Interface> : RealProxy
{
    public MulticastProxy(params Interface[] interfaces)
        : base(typeof(Interface))
    {
        this.interfaces = new List<Interface>(interfaces);
    }

    public override IMessage Invoke(IMessage msg)
    {
        IMethodMessage mm = msg as IMethodMessage;

        MethodInfo method = (MethodInfo)mm.MethodBase;
        object[] args = mm.Args;

        foreach (var i in this.interfaces)
        {
            method.Invoke(i, args);
        }

        return new ReturnMessage(
            null, null, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
    }

    private List<Interface> interfaces;
}
```


使う側では、GetTransparentProxy を呼んでプロキシ生成。

```csharp
interface IAnimal
{
    void Bark();
}

class Cat : IAnimal
{
    public void Bark() { Console.Write("にゃー\n"); }
}

class Dog : IAnimal
{
    public void Bark() { Console.Write("わん\n"); }
}

class Mouse : IAnimal
{
    public void Bark() { Console.Write("ちゅー\n"); }
}

class Program
{
    static void Main(string[] args)
    {
        // 猫、犬、鼠を1匹ずつ登録。
        var proxy = new MulticastProxy<IAnimal>(
            new Cat(),
            new Dog(),
            new Mouse()
            );

        IAnimal animals = (IAnimal)proxy.GetTransparentProxy();

        animals.Bark(); // ちゃんと3匹とも鳴く。
    }
}
```


要するに、以下のようなインスタンスメソッド呼び出しに相当する処理を自動で行ってくれるものです。

```csharp
IAnimal[] animals = new IAnimal[] { new Cat(), new Dog(), new Mouse() };

foreach (var i in animals)
{
    i.Bark();
}
```


この例では、「IAnimal の Bark を呼ぶ」というのが事前に分かっているので簡単に書けますが、
任意のインターフェースに対してこれと同様のことをするのが MulticastProxy の役目です。


## <a id="sec-generated-title-3"></a> <a id="pre-created"></a>事前にデリゲート化

一般的に言って、リフレクション使いまくるとパフォーマンスがでないので、
パフォーマンスが必要なら[動的にアセンブリ言語を吐き出したり、
      かなり変態的なことする必要があったりします](http://d.hatena.ne.jp/NyaRuRu/20070925/p1)が。

幸い、今回の MulticastProxy の場合、
MethodInfo から事前にデリゲートを作っておくことが可能で、
これを使うことでそこまで難しいことをせずともかなりのパフォーマンス改善ができます。

前節で示した MulticastProxy では、
以下のように、登録したインターフェースのインスタンスごとに MethodInfo.Invoke を呼んでいました。

```csharp
IMethodMessage mm = msg as IMethodMessage;

MethodInfo method = (MethodInfo)mm.MethodBase;
object[] args = mm.Args;

foreach (var i in this.interfaces)
{
    method.Invoke(i, args);
}
```


透過プロキシの Invoke が呼ばれるたびに MethodInfo.Invoke を呼び出すのは非常に重たい処理になるので、
事前にデリゲート化して高速化してみます。

例えば、IAnimal.Bark の呼び出しの場合、
Delegate.CreateDelegate を使って以下のように書けます。

```csharp
IAnimal[] animals = new IAnimal[] { new Cat(), new Dog(), new Mouse() };

Delegate d = null;

foreach (var i in animals)
{
    Delegate di = Delegate.CreateDelegate(typeof(Action), i, "Bark");

    if (d == null) d = di;
    else d = Delegate.Combine(d, di);
}

d.DynamicInvoke();
```


CreateDelegate の処理はかなり重たい処理なので、
毎度 CreateDelegate するとかえって遅くなるんですが、
事前に作ってキャッシュしておくことで高速化がみこめます。

```csharp
IMethodMessage mm = msg as IMethodMessage;

MethodInfo method = (MethodInfo)mm.MethodBase;

// 上述の例のように、事前に CreateDelegate しておいて、
// MethodInfo → Delegate の辞書の格納しておく。
Delegate d = this.del[method];

// RealProxy.Invoke 内では辞書から取り出したデリゲートを呼び出すだけ。
d.DynamicInvoke(mm.Args);
```
