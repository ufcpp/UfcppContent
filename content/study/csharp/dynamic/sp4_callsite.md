---
title: "dynamic の内部実装"
source_url: "https://ufcpp.net/study/csharp/dynamic/sp4_callsite/"
content_type: "Article"
published_at: "2009-05-24T00:00:00"
updated_at: "2009-06-21T00:00:00"
tags:
  - "Ver. 4.0"
umbraco_id: 1317
parent_id: 1312
sort_order: 4
aliases:
  - "/csharp/dynamic/sp4_callsite/"
  - "/csharp/sp4_callsite"
  - "/csharp/sp4_callsite.html"
  - "/study/csharp/sp4_callsite"
  - "/study/csharp/sp4_callsite.html"
---

# dynamic の内部実装

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
<h5 class="version version4">Ver. 4.0</h5>

dynamic って内部的にはどうなってるの？という話。

C# の dynamic は、「型が動的」というよりは、「静的な型に対する動的コード生成」と言った方が正確です。
動的に生成したコードはキャッシュされていて、2度目の呼び出しからはかなり効率よく実行されます。
このような手法はインラインメソッドキャッシュ（inline method cache）と呼ばれています。

注意：
内部的な話なので、C# のバージョンアップで実装方法が変わる可能性もあります（基本的な原理は変わらないと思いますが）。
（今このページに書かれている内容は、C# 4.0 の時点の実装に基づいています。）


##### <a id="sec-generated-title-2"></a>ポイント
* dynamic 型ってのは実はなくて、内部的には object。

* CallSite クラスを使って動的コード生成。



##<a id="sec-generated-title-3"></a> <a id="callsite"></a>動的 callsite
dynamic を使ったコードは、内部的には CallSite というクラスを使ったコードに展開されます。
（多分、「動的呼び出し（call）用の動的コードを生成するための用地（site）というような意味合い。）
例えば、以下のような C# 4.0 コードは、

<pre class="source" title="dynamic を使ったメソッド　X の動的呼び出し" lang="">
<code><span class="reserved">public static void</span> CallX(<span class="reserved">object</span> obj)
{
    <span class="reserved">dynamic</span> d = obj;
    d.X();
}

<span class="reserved">public static dynamic</span> GetX(<span class="reserved">dynamic</span> obj)
{
    <span class="reserved">return</span> obj.X;
}
</code></pre>


以下のようなコードに展開されます。

<pre class="source" title="CallX の展開結果" lang="">
<code><span class="comment">// ↓本当は、いかにもコンパイラが自動生成したような変な変数名になってる</span>
<span class="reserved">static</span> CallSite&lt;Action&lt;CallSite, <span class="reserved">object</span>&gt; site1;
<span class="reserved">static</span> CallSite&lt;Func&lt;CallSite, <span class="reserved">object</span>, <span class="reserved">object</span>&gt; site2;

<span class="reserved">public static void</span> CallX(<span class="reserved">object</span> obj)
{
    <span class="reserved">object</span> d = obj; <span class="comment">// (1) dynamic 型の変数は、実のところ単なる object 型になる</span>

    <span class="reserved">if</span> (site1 == <span class="reserved">null</span>)
    {
        <span class="comment">// d.X() 相当のコードを動的生成するための CallSite を作る。</span>
        site1 = CallSite&lt;Action&lt;CallSite, <span class="reserved">object</span>&gt;&gt;.Create(
            <span class="reserved">new</span> CSharpInvokeMemberBinder(
                CSharpCallFlags.None, <span class="literal">"X"</span>, <span class="reserved">typeof</span>(DynamicSample), <span class="reserved">null</span>,
                <span class="reserved">new</span> CSharpArgumentInfo[] {
                   <span class="reserved">new</span> CSharpArgumentInfo(CSharpArgumentInfoFlags.None, <span class="reserved">null</span>)
                }));
    }

    <span class="comment">// 動的生成したコードを呼んだり、新たに動的生成するのは、
    // 実際には Target デリゲートの中。</span>
    site1.Target.Invoke(site1, d);
}

<span class="comment">// 引数や戻り値が dynamic の場合は、Dynamic 属性付きの object 型になる</span>
[return: Dynamic]
<span class="reserved">public static object</span> GetX([Dynamic] <span class="reserved">object</span> obj)
{
    <span class="reserved">if</span> (site2 == <span class="reserved">null</span>)
    {
        <span class="comment">// d.X 相当のコードを動的生成するための CallSite を作る。</span>
        site2 = CallSite&lt;Func&lt;CallSite, <span class="reserved">object</span>, <span class="reserved">object</span>&gt;&gt;.Create(
            <span class="reserved">new</span> CSharpGetMemberBinder(
                <span class="literal">"X"</span>, <span class="reserved">typeof</span>(DynamicSample), <span class="reserved">new</span> CSharpArgumentInfo[] {
                    <span class="reserved">new</span> CSharpArgumentInfo(CSharpArgumentInfoFlags.None, <span class="reserved">null</span>)
                }));
    }

    <span class="comment">// 動的生成したコードを呼んだり、新たに動的生成するのは、
    // 実際には Target デリゲートの中。</span>
    <span class="reserved">return</span> site2.Target.Invoke(site2, obj);
}
</code></pre>


要点は3つ。

1. dynamic 型ってのは本当はなくて、内部的には単なる objetct 型になる。 メソッドの引数が dynamic 型の場合、[Dynamic] 属性が付く。

2. CallSite 初期化時： どうやって動的コード生成するかを指定する。 CallSiteBinder（この例の場合、CSharpInvokeMemberBinder や CSharpGetMemberBinder がこのクラスを継承してる）を使う。

3. CallSite.Target： 動的に生成したコードの入っているデリゲート。 obj の中身がまだコード生成していない型だった場合、動的コード生成して Target デリゲート自身の中身を書きかえる。



##<a id="sec-generated-title-4"></a> <a id="DynamicAttribute"></a>Dynamic 属性
前述の通り、コンパイル結果的には dynamic 型ってものはなくて、
実際のところは単なる object 型の変数になります。
特に、ローカル変数の型を dynamic にした場合には、完璧に単なる object 型の変数になります。

メンバー変数やプロパティ、メソッドの引数や戻り値の型を dynamic にした場合には、
普通の object と区別するために、Dynamic 属性が付きます。

<pre class="source" title="メンバー変数、プロパティ、メソッドの引数・戻り値に dynamic" lang="">
<code><span class="reserved">dynamic</span> x;
<span class="reserved">public dynamic</span> X { <span class="reserved">get</span> { <span class="reserved">return</span> x; } <span class="reserved">set</span> { x = value; } }

<span class="reserved">public static dynamic</span> GetX(<span class="reserved">dynamic</span> obj)
{
    <span class="comment">// 中身省略</span>
}
</code></pre>


というコードは、以下のようなコードに変換されます。

<pre class="source" title="Dynamic属性" lang="">
<code>[Dynamic]
<span class="reserved">private object</span> x;

[Dynamic]
<span class="reserved">public object</span> X
{
    [return: Dynamic]
    <span class="reserved">get</span> { <span class="reserved">return this</span>.x; }
    [param: Dynamic]
    <span class="reserved">set</span> { <span class="reserved">this</span>.x = value; }
}

[return: Dynamic]
<span class="reserved">public static object</span> GetX([Dynamic] <span class="reserved">object</span> obj)
{
    <span class="comment">// 中身省略</span>
}
</code></pre>


なので、以下のようなコードはコンパイルエラーを起こしたりします。
（dynamic 型と object 型でメソッドを「[オーバーロード](../structured/st_function.md#overload)」することはできません。）

<pre class="source" title="dynamic と object でオーバーロードはできない" lang="">
<code><span class="comment">// 同じパラメーター型の GetX が2個あるぞって怒られる。</span>
<span class="reserved">public static dynamic</span> GetX(<span class="reserved">dynamic</span> obj)
{
    <span class="reserved">return</span> obj.X;
}

<span class="reserved">public static object</span> GetX(<span class="reserved">object</span> obj)
{
    <span class="reserved">var</span> t = obj.GetType();
    <span class="reserved">return</span> t.GetMethod(<span class="literal">"X"</span>).Invoke(obj, <span class="reserved">new object</span>[0]);
}
</code></pre>


ジェネリック型の型引数を dynamic にした場合はどうなるかというと、

<pre class="source" title="型引数を dynamic に" lang="">
<code><span class="reserved">static void</span> GenericDynamic(
    IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">object</span>&gt; a,
    IDictionary&lt;<span class="reserved">dynamic</span>, <span class="reserved">object</span>&gt; b,
    IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">dynamic</span>&gt; c,
    IDictionary&lt;<span class="reserved">dynamic</span>, <span class="reserved">dynamic</span>&gt; d)
{
}
</code></pre>


この例の場合、以下のようなコードに変換されます。

<pre class="source" title="型引数を dynamix にした結果がこれだよ" lang="">
<code><span class="reserved">private static void</span> GenericDynamic(
    IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">object</span>&gt; a,
    [Dynamic(<span class="reserved">new bool</span>[] { <span class="reserved">false</span>, <span class="reserved">true</span>, <span class="reserved">false</span> })] IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">object</span>&gt; b,
    [Dynamic(<span class="reserved">new bool</span>[] { <span class="reserved">false</span>, <span class="reserved">false</span>, <span class="reserved">true</span> })] IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">object</span>&gt; c,
    [Dynamic(<span class="reserved">new bool</span>[] { <span class="reserved">false</span>, <span class="reserved">true</span>, <span class="reserved">true</span> })] IDictionary&lt;<span class="reserved">object</span>, <span class="reserved">object</span>&gt; d)
{
}
</code></pre>


要するに、型引数の少なくともどれか1つが dynamic 型だった場合、
bool[] の引数付きの Dynamic 属性が付きます。


##<a id="sec-generated-title-5"></a> <a id="CallSiteBinder"></a>CallSiteBinder
続いて CallSite の初期化部分。
上述のコードのうち、以下のようなコードの部分について。

<pre class="source" title="CallSite の初期化" lang="">
<code><span class="reserved">if</span> (site2 == <span class="reserved">null</span>)
{
    <span class="comment">// d.X 相当のコードを動的生成するための CallSite を作る。</span>
    site2 = CallSite&lt;Func&lt;CallSite, <span class="reserved">object</span>, <span class="reserved">object</span>&gt;&gt;.Create(
        <span class="reserved">new</span> CSharpGetMemberBinder(
            <span class="literal">"X"</span>, <span class="reserved">typeof</span>(DynamicSample), <span class="reserved">new</span> CSharpArgumentInfo[] {
                <span class="reserved">new</span> CSharpArgumentInfo(CSharpArgumentInfoFlags.None, <span class="reserved">null</span>)
            }));
}
</code></pre>


<code>d.X</code> などのメンバーアクセスに対して、どういう動的コード生成を行えばいいかは、CallSite クラス自身は知りません。
それを実際に担ってるのは、この例で言うと CSharpGetMemberBinder の部分です。
CSharpGetMemberBinder は System.Runtime.CompilerServices.CallSiteBinder というクラスを継承していて、
CallSiteBinder の抽象メソッドの Bind 内で式木を作っています。

CSharpGetMemberBinder というように、名前に CSharp という言葉が付いてることからもわかるように、
言語ごとに CallSiteBinder の実装を変えることができます。

（CallSite 自体は、C# 4.0 の dynamic のためだけに作られたクラスではなくて、
DLR に含まれているクラス。
IronPython などの動的言語の実装にも使われています。）

C# 4.0 の場合（要するに、CSharpGetMemberBinder の中の挙動としては）、以下のような動的コード生成を行います。

* IDynamicObject インターフェースを実装した型の場合、TryGetMember などのメソッド呼び出し

* COM オブジェクトの場合、COM Interop コード

* その他の場合、「[リフレクション](sp_reflection.md#reflection)」を使ってメンバーを持っているかどうか調べて、持っているならそのメンバーにアクセスするコードを生成。



##<a id="sec-generated-title-6"></a> <a id="DynamicCodeGeneration"></a>CallSite.Target 内での動的コード生成
最後に、実際に動的コード生成。
CallSite.Target デリゲートを呼んでいる部分について。

<pre class="source" title="CallSite.Target 呼び出し" lang="">
<code>    site1.Target.Invoke(site1, d);
</code></pre>


Target デリゲートの中身は、初期状態では以下のようなコードと同じ状態になっています。

<pre class="source" title="CallSite.Target の初期状態" lang="">
<code><span class="reserved">static  object</span> _anonymous(CallSite site, <span class="reserved">object</span> x)
{
    <span class="reserved">return</span> site.Update(site, x);
}
</code></pre>


この状態で、<code>GetX(new Point { X = 1, Y = 2});</code> というように、
Point 型のインスタンスを引数として Target が呼ばれたとします。
このとき、Target 内には Update の1行しかないので、
この Update が呼ばれて、動的コード生成が行われます。
その結果、Target が以下のような状態に更新されます。

<pre class="source" title="Point 型に対応" lang="">
<code><span class="reserved">static  object</span> _anonymous(CallSite site, <span class="reserved">object</span> x)
{
    <span class="reserved">if</span> (x <span class="reserved">is</span> Point)
        <span class="reserved">return</span> ((Point)x).X;
    <span class="reserved">else</span>
        <span class="reserved">return</span> site.Update(site, x);
}
</code></pre>


ここで、<code>((Point)x).X</code> の部分を生成するのが CallSiteBinder の役目です。

Target 内がこのような状態になったので、
以後、Point 型のインスタンスで GetX を呼べば、
そこそこいい実行速度が得られます。

実行速度に関してもう少し詳しく言うと、

* 初回（今まで使ったことのない型のインスタンスで GetX を呼んだりしたとき）には動的コード生成（Target 内を更新）する分、かなり重たい処理が入る。

* 2回目以降は、そこそこ高速。
    * 静的に<code>x.X</code>プロパティを呼ぶのと比べると、if 文とキャストの分だけ遅い。

    * 呼び出しのたびに毎回「[リフレクション](sp_reflection.md#reflection)」を使うよりは圧倒的に高速。




という感じになります。

（
ちなみに、こういう、動的コード生成してデリゲート化しておくような手法をインラインメソッドキャッシュ（inline method cache）と言うようです。
DLR や C# 4.0 以外（例えば JavaScript とか）でも、同様の手法はよく使われます。
）

Point 以外の型のインスタンスが来ると、当然また Target の更新がかかります。
例えば、Vector3D を使って <code>GetX(new Vector3D(1, 2, 3));</code> とかすると、

<pre class="source" title="Vector3D 型にも対応" lang="">
<code><span class="reserved">static  object</span> _anonymous(CallSite site, <span class="reserved">object</span> x)
{
    <span class="reserved">if</span> (x <span class="reserved">is</span> Point)
        <span class="reserved">return</span> ((Point)x).X;
    <span class="reserved">else if</span> (x <span class="reserved">is</span> Vector3D)
        <span class="reserved">return</span> ((Vector3D)x).X;
    <span class="reserved">else</span>
        <span class="reserved">return</span> site.Update(site, x);
}
</code></pre>


となります。


##<a id="sec-generated-title-7"></a> <a id="misc"></a>余談
その他、いくつか小ネタを。


##### <a id="sec-generated-title-8"></a>typeof(dynamic)
<code>typeof(dynamic)</code> はそもそもエラーになります。
<code>typeof(object)</code> が得られたりはしません。


##### <a id="sec-generated-title-9"></a>dynamic の ToString 呼び出し
dynamic の実体は object なわけですが、
じゃあ、ToString() や GetHashCode() 等の object 型のメソッドはどうなるかというと・・・、
CallSite を介した動的コード生成になります。
ToString() だけ特別扱いされて静的なコードになったりはしません。


##### <a id="sec-generated-title-10"></a>Dynamic 属性を直接使う
dynamic キーワードを使わず、
Dynamic 属性を直接付けようとするとコンパイルエラーになります。
「DynamicAttribute は直接は使えない。dynamic キーワードを使ってくれ」というような感じで怒られます。
