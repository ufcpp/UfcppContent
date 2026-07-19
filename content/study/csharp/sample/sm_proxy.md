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

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「[実行時型情報](../dynamic/sp_reflection.md)」のサンプルがちょっと不足してるなぁと思って作ったもの。

.NET Framework では、RealProxy というクラスを使って、
インターフェースのメソッド呼び出しを横取りして独自の処理に置き換えることができます。

* 
[ソース一式（ZIP 形式）](../../../../assets/media/ufcpp2000/csharp/source/MulticastProxy.zip)




##<a id="sec-generated-title-2"></a> <a id="realproxy"></a>RealProxy
例えば、マルチキャストデリゲートのようなことをインターフェースのメソッド呼び出しに対して行うようなプロキシ。

RealProxy クラスを継承して、Invoke メソッドをオーバーライドするだけ。

<pre class="source" title="RealProxy を継承" lang="">
<code><span class="reserved">public class</span> MulticastProxy&lt;Interface&gt; : RealProxy
{
    <span class="reserved">public</span> MulticastProxy(<span class="reserved">params</span> Interface[] interfaces)
        : <span class="reserved">base</span>(<span class="reserved">typeof</span>(Interface))
    {
        <span class="reserved">this</span>.interfaces = <span class="reserved">new</span> List&lt;Interface&gt;(interfaces);
    }

    <span class="reserved">public override</span> IMessage Invoke(IMessage msg)
    {
        IMethodMessage mm = msg <span class="reserved">as</span> IMethodMessage;

        MethodInfo method = (MethodInfo)mm.MethodBase;
        <span class="reserved">object</span>[] args = mm.Args;

        <span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in this</span>.interfaces)
        {
            method.Invoke(i, args);
        }

        <span class="reserved">return new</span> ReturnMessage(
            <span class="reserved">null</span>, <span class="reserved">null</span>, 0, mm.LogicalCallContext, (IMethodCallMessage)msg);
    }

    <span class="reserved">private</span> List&lt;Interface&gt; interfaces;
}
</code></pre>


使う側では、GetTransparentProxy を呼んでプロキシ生成。

<pre class="source" title="GetTransparentProxy" lang="">
<code><span class="reserved">interface</span> IAnimal
{
    <span class="reserved">void</span> Bark();
}

<span class="reserved">class</span> Cat : IAnimal
{
    <span class="reserved">public void</span> Bark() { Console.Write(<span class="literal">"にゃー\n"</span>); }
}

<span class="reserved">class</span> Dog : IAnimal
{
    <span class="reserved">public void</span> Bark() { Console.Write(<span class="literal">"わん\n"</span>); }
}

<span class="reserved">class</span> Mouse : IAnimal
{
    <span class="reserved">public void</span> Bark() { Console.Write(<span class="literal">"ちゅー\n"</span>); }
}

<span class="reserved">class</span> Program
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        <span class="comment">// 猫、犬、鼠を1匹ずつ登録。</span>
        <span class="reserved">var</span> proxy = <span class="reserved">new</span> MulticastProxy&lt;IAnimal&gt;(
            <span class="reserved">new</span> Cat(),
            <span class="reserved">new</span> Dog(),
            <span class="reserved">new</span> Mouse()
            );

        IAnimal animals = (IAnimal)proxy.GetTransparentProxy();

        animals.Bark(); <span class="comment">// ちゃんと3匹とも鳴く。</span>
    }
}
</code></pre>


要するに、以下のようなインスタンスメソッド呼び出しに相当する処理を自動で行ってくれるものです。

<pre class="source" title="foreach でインスタンスごとに Bark 呼び出し" lang="">
<code>IAnimal[] animals = <span class="reserved">new</span> IAnimal[] { <span class="reserved">new</span> Cat(), <span class="reserved">new</span> Dog(), <span class="reserved">new</span> Mouse() };

<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> animals)
{
    i.Bark();
}
</code></pre>


この例では、「IAnimal の Bark を呼ぶ」というのが事前に分かっているので簡単に書けますが、
任意のインターフェースに対してこれと同様のことをするのが MulticastProxy の役目です。


##<a id="sec-generated-title-3"></a> <a id="pre-created"></a>事前にデリゲート化
一般的に言って、リフレクション使いまくるとパフォーマンスがでないので、
パフォーマンスが必要なら[動的にアセンブリ言語を吐き出したり、
      かなり変態的なことする必要があったりします](http://d.hatena.ne.jp/NyaRuRu/20070925/p1)が。

幸い、今回の MulticastProxy の場合、
MethodInfo から事前にデリゲートを作っておくことが可能で、
これを使うことでそこまで難しいことをせずともかなりのパフォーマンス改善ができます。

前節で示した MulticastProxy では、
以下のように、登録したインターフェースのインスタンスごとに MethodInfo.Invoke を呼んでいました。

<pre class="source" title="インスタンスごとに MethodInfo.Invoke を呼び出す" lang="">
<code>IMethodMessage mm = msg <span class="reserved">as</span> IMethodMessage;

MethodInfo method = (MethodInfo)mm.MethodBase;
<span class="reserved">object</span>[] args = mm.Args;

<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in this</span>.interfaces)
{
    method.Invoke(i, args);
}
</code></pre>


透過プロキシの Invoke が呼ばれるたびに MethodInfo.Invoke を呼び出すのは非常に重たい処理になるので、
事前にデリゲート化して高速化してみます。

例えば、IAnimal.Bark の呼び出しの場合、
Delegate.CreateDelegate を使って以下のように書けます。

<pre class="source" title="" lang="">
<code>IAnimal[] animals = <span class="reserved">new</span> IAnimal[] { <span class="reserved">new</span> Cat(), <span class="reserved">new</span> Dog(), <span class="reserved">new</span> Mouse() };

Delegate d = <span class="reserved">null</span>;

<span class="reserved">foreach</span> (<span class="reserved">var</span> i <span class="reserved">in</span> animals)
{
    Delegate di = Delegate.CreateDelegate(<span class="reserved">typeof</span>(Action), i, <span class="literal">"Bark"</span>);

    <span class="reserved">if</span> (d == <span class="reserved">null</span>) d = di;
    <span class="reserved">else</span> d = Delegate.Combine(d, di);
}

d.DynamicInvoke();
</code></pre>


CreateDelegate の処理はかなり重たい処理なので、
毎度 CreateDelegate するとかえって遅くなるんですが、
事前に作ってキャッシュしておくことで高速化がみこめます。

<pre class="source" title="" lang="">
<code>IMethodMessage mm = msg <span class="reserved">as</span> IMethodMessage;

MethodInfo method = (MethodInfo)mm.MethodBase;

<span class="comment">// 上述の例のように、事前に CreateDelegate しておいて、
// MethodInfo → Delegate の辞書の格納しておく。</span>
Delegate d = <span class="reserved">this</span>.del[method];

<span class="comment">// RealProxy.Invoke 内では辞書から取り出したデリゲートを呼び出すだけ。</span>
d.DynamicInvoke(mm.Args);
</code></pre>
