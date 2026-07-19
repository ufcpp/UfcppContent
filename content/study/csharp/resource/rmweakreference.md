---
title: "【雑記】弱参照"
source_url: "https://ufcpp.net/study/csharp/resource/rmweakreference/"
content_type: "Article"
published_at: "2015-01-02T00:00:00"
updated_at: "2025-05-17T17:50:00"
tags: []
umbraco_id: 1297
parent_id: 1286
sort_order: 16
aliases:
  - "/csharp/RmWeakReference"
  - "/csharp/RmWeakReference.html"
  - "/csharp/resource/rmweakreference/"
  - "/study/csharp/RmWeakReference"
  - "/study/csharp/RmWeakReference.html"
---

# 【雑記】弱参照

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
ガベージ コレクションに関連して、弱参照というものがあります。
めったに使うものではありませんが、使い方・使い道を説明します。


##### <a id="sec-generated-title-2"></a>サンプル
[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Resource/WeakReference](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Resource/WeakReference)


##<a id="sec-generated-title-3"></a> <a id="weak-reference"></a>弱参照とは
「[ガベージ コレクション](rm_gc.md#garbage-collection)」(以下、GC)では、「他のオブジェクトから参照されているものは生きてる、誰からも参照されていないものはもう不要」という判定方法で、
不要なオブジェクトを削除します。
逆に言うと、誰か1つでも参照を持っているオブジェクトは削除されません。

一方で、「オブジェクトを使いたいんだけども、GC 的には参照していることにしないでほしい」、
「自分以外が全員参照を手放したらその時点で削除対象にしてほしい」というような要件がまれにあります。
こういう、GC の参照探索上は除外してほしい参照を、<strong id="key-weak-reference" class="keyword">弱参照</strong>(weak reference)といいます。

.NET では、WeakReference クラス(System 名前空間）を使うことで弱参照を扱えます。
例えば以下のように使います。

<pre class="source" title="WeakReference クラスの使い方" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main(<span class="reserved">string</span>[] args)
    {
        RunAsync().Wait();
    }

    <span class="reserved">private static async</span> <span class="type">Task</span> RunAsync()
    {
        <span class="reserved">var</span> obj = (<span class="reserved">object</span>)123;
        <span class="reserved">var</span> t = StartLoop(<span class="reserved">new</span> <span class="type">WeakReference</span>&lt;<span class="reserved">object</span>&gt;(obj));

        <span class="comment">// 2.5秒後にオブジェクトを消す</span>
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(2500);
        obj = <span class="reserved">null</span>;
        <span class="type">GC</span>.Collect();

        <span class="reserved">await</span> t;
    }

    <span class="comment">// 1秒に1回、「参照中」メッセージを表示</span>
    <span class="reserved">static async</span> <span class="type">Task</span> StartLoop(<span class="type">WeakReference</span>&lt;<span class="reserved">object</span>&gt; r)
    {
        <span class="reserved">while</span> (<span class="reserved">true</span>)
        {
            <span class="reserved">object</span> obj;
            <span class="reserved">if</span> (r.TryGetTarget(<span class="reserved">out</span> obj))
            {
                <span class="type">Console</span>.WriteLine(obj + <span class="literal">" を参照中"</span>);
            }
            <span class="reserved">else</span>
            {
                <span class="type">Console</span>.WriteLine(<span class="literal">"参照がなくなりました"</span>);
                <span class="reserved">break</span>;
            }

            <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        }
    }
}
</code></pre>


この例では、RunAsync 側で GC.Collect (ガベージ コレクションを強制起動)を呼んだ時点で、<code>(object)123</code> (整数を object 化(ボックス化)したもの)の参照が消えます。
つまり、<code>WeakReference&lt;object&gt; r</code> 越しに参照している分は、GC 的には参照扱いしていません。

そして、その後、StartLoop 側で TryGetTarget に失敗(false が返って、else 側に進む)して、ループが終了します。
GC により元のオブジェクト(Target)が削除されていたら、TryGetTarget が失敗します。


##<a id="sec-generated-title-4"></a> <a id="usage"></a>弱参照の用途
普通に C# を使っていて、WeakReference を見かけることはほとんどないと思います。
だいたいのプログラムでは、メモリ管理について気にすることはめったにありません(GC 任せ)。
弱参照を使うというのは、メモリ管理を自分で気にかけるということなので、当然、あまり出番はありません。

それに、弱参照を使うと、GC が掛かるタイミング(普通は制御しない。不定なタイミング)に依存することになるので、挙動が読めないという問題もあります。

ということで、弱参照を使う場面はほとんどありませんが、一応、いくつか用途を紹介しておきましょう。


###<a id="sec-generated-title-5"></a> <a id="weak-key-table"></a>用途1: 弱参照キーのテーブル
あるオブジェクトに、外から別のオブジェクトを紐づけたいとします。
これに対する手っ取り早い実現方法は Dictionary を使ったテーブル化です。

例えば、以下のようなクラス(名簿か何かで使う、「個人」型)があったとします。

<pre class="source" title="" lang="">
<code><span class="inactive">/// &lt;summary&gt;
///</span><span class="comment"> 仮に、このクラスが自作じゃなくて、どこか別のライブラリで定義されているものとする。</span>
<span class="inactive">///</span><span class="comment"> 自分のプログラムでは、ID と名前だけじゃなくて、住所も足したくなったとして…</span>
<span class="inactive">/// &lt;/summary&gt;</span>
<span class="reserved">public class</span> <span class="type">Person</span>
{
    <span class="reserved">public int</span> Id { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public string</span> Name { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
</code></pre>


元々の名簿管理では ID と名前くらいしか使っていなかったものに対して、追加で別の情報を足すことになったとしましょう。
一応、Dictionary を使えば、情報の関連付けはできます。
例えば、所在地を足すなら以下のようにします。

<pre class="source" title="" lang="">
<code><span class="reserved">var</span> people = <span class="reserved">new</span>[]
{
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 1, Name = <span class="literal">"Jurian Naul"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 2, Name = <span class="literal">"Thomas Bent"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 3, Name = <span class="literal">"Ellen Carson"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 4, Name = <span class="literal">"Katrina Lauran"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 5, Name = <span class="literal">"Monica Ausbach"</span> },
};

<span class="reserved">var</span> locations = <span class="reserved">new</span> <span class="type">Dictionary</span>&lt;<span class="type">Person</span>, <span class="reserved">string</span>&gt;();

locations[people[0]] = <span class="literal">"Shinon"</span>;
locations[people[1]] = <span class="literal">"Lance"</span>;
locations[people[2]] = <span class="literal">"Pidona"</span>;
locations[people[3]] = <span class="literal">"Loanne"</span>;
locations[people[4]] = <span class="literal">"Loanne"</span>;

<span class="reserved">foreach</span> (<span class="reserved">var</span> p <span class="reserved">in</span> people)
{
    <span class="reserved">var</span> location = locations[p];
    <span class="type">Console</span>.WriteLine(p.Name + <span class="literal">" at "</span> + location);
}
</code></pre>


ここで、この Person 情報は追加・削除が結構あるとしましょう。
削除された Person に対しては、一緒に所在地情報も消えてほしかったりします。
locations テーブルが Person をキーとして参照していることによって、Person が GC 対象から外れる(いつまでたっても削除されない)ようでは困ります。
こういう場合に弱参照が使えます。
Dictionary のキー側を弱参照にすればいいわけです。

というような、キー側が弱参照なテーブルは、実は最初からあって、
ConditionalWeakTable というクラス(System.Runtime.CompilerServices 名前空間)です。
(実際には WeakReference クラスを使って弱参照管理しているのではなく、ネイティブ実装で弱参照管理しているようですが。)

<pre class="source" title="" lang="">
<code><span class="reserved">var</span> people = <span class="reserved">new</span>[]
{
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 1, Name = <span class="literal">"Jurian Naul"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 2, Name = <span class="literal">"Thomas Bent"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 3, Name = <span class="literal">"Ellen Carson"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 4, Name = <span class="literal">"Katrina Lauran"</span> },
    <span class="reserved">new</span> <span class="type">Person</span> {Id = 5, Name = <span class="literal">"Monica Ausbach"</span> },
};

<span class="reserved">var</span> locations = <span class="reserved">new</span> <em><span class="type">ConditionalWeakTable</span>&lt;<span class="type">Person</span>, <span class="reserved">string</span>&gt;()</em>;

locations.Add(people[0], <span class="literal">"Shinon"</span>);
locations.Add(people[1], <span class="literal">"Lance"</span>);
locations.Add(people[2], <span class="literal">"Pidona"</span>);
locations.Add(people[3], <span class="literal">"Loanne"</span>);
locations.Add(people[4], <span class="literal">"Loanne"</span>);

<span class="reserved">foreach</span> (<span class="reserved">var</span> p <span class="reserved">in</span> people)
{
    <span class="reserved">string</span> location;
    <span class="reserved">if</span> (locations.TryGetValue(p, <span class="reserved">out</span> location))
        <span class="type">Console</span>.WriteLine(p.Name + <span class="literal">" at "</span> + location);
}
</code></pre>


これなら、locations テーブルがあっても、キーになっている Person は GC の対象になります。

もっとも、ほとんどの場合、「自分の要件にあった別の Person クラスを作りなおす」という方が正しい解決策でしょう。
弱参照キーの出番もそれほど多くないです。
ConditionalWeakTable クラスも、名前空間が CompilerServices になっている通り、元々、C# のコンパイラーが使うために作られた用途の狭いクラスです。
(Java なんかは、同系統のクラスである WeakHashMap が java.util 名前空間にあったりしますが…)


###<a id="sec-generated-title-6"></a> <a id="weak-event"></a>用途2: 弱イベント
GC を持っている(ので、めったなことではメモリ リークしないはずの)プログラミング言語で、メモリ リークの温床になっているのがイベント購読です。
「[【雑記】イベントの購読とその解除](../functional/misceventsubscribe.md)」で説明していますが、
イベント発生側と受取側の寿命が違う場合、イベント購読解除をしないとメモリ リークになります。

イベントの購読解除をきっちり行うためには、要は、Dispose 処理(参考: 「[using ステートメント](oo_dispose.md#using)」)をちゃんとすればいいわけですが… 
まれに、Dispose 処理がものすごく面倒(ちゃんとするためにはコードがかなり複雑化してしまう)ものがあります(参考: 例えば、「[Task クラス](rm_disposable.md#task)」)。

こういう、Dispose 処理が必須なのに Dispose しにくいものの救済策にも、弱参照が使えたりします。
弱参照を使って、イベント受取側が消えた時に自動的にイベント購読解除してしまうやり方を、
弱イベント(weak event)パターンとか、
弱購読(weak subscription)とか呼んだりします。

ちなみに、こういう要件が頻発する代表例は、GUI のデータ バインディングです(参考: [WPFの「データ・バインディング」を理解する](http://www.atmarkit.co.jp/ait/articles/1010/08/news123.html))。
なので、WPF は、弱イベント パターンを補助するために [WeakEventManager](http://msdn.microsoft.com/ja-jp/library/system.windows.weakeventmanager.aspx) というクラス(System.Windows 名前空間)を持っていたりします。
とはいえ、C# の event 構文に対する弱イベント パターン実装は結構めんどくさくて、この WeakEventManager もあんまり使いたい作りではなかったりします。

event 構文で弱イベントを使いにくい理由は、「[【雑記】イベントの購読とその解除](../functional/misceventsubscribe.md)」 で説明してるような、add/remove 型の購読開始/解除だからだと思います。
IDisposable Subscribe 型のイベント購読なら、意外と簡単に書けます。
例えば、[Reactive Extensions](https://rx.codeplex.com/) を使って実装するなら以下のような感じ。

<pre class="source" title="弱イベント購読" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Reactive.Disposables;

<span class="reserved">public static class</span> <span class="type">WeakEventExtensions</span>
{
    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> 弱イベント購読。</span>
    <span class="inactive">///</span><span class="comment"> 戻り値の </span><span class="inactive">&lt;see cref="</span><span class="type">IDisposable</span><span class="inactive">"/&gt;</span><span class="comment"> が誰からも参照されなくなったら自動的にイベント購読解除する。</span>
    <span class="inactive">/// &lt;/summary&gt;
    /// &lt;typeparam name="</span><span class="type">T</span><span class="inactive">"&gt;</span><span class="comment">イベント引数の型。</span><span class="inactive">&lt;/typeparam&gt;
    /// &lt;param name="</span>observable<span class="inactive">"&gt;</span><span class="comment">イベント発生側。</span><span class="inactive">&lt;/param&gt;
    /// &lt;param name="</span>onNext<span class="inactive">"&gt;</span><span class="comment">イベント受取側。</span><span class="inactive">&lt;/param&gt;
    /// &lt;returns&gt;</span><span class="comment">イベント購読解除用の disposable。</span><span class="inactive">&lt;/returns&gt;
    /// &lt;remarks&gt;
    ///</span><span class="comment"> 弱参照の性質上、</span><span class="inactive">&lt;see cref="</span><span class="type">GC</span><span class="inactive">"/&gt;</span><span class="comment"> がかかって初めて「誰も使ってない」判定を受ける。</span>
    <span class="inactive">///</span><span class="comment"> それまではイベント購読解除されず、イベントが届き続ける。</span>
    <span class="inactive">///</span><span class="comment"> GC タイミングに左右されるコードは推奨できないんで、可能な限り、</span>
    <span class="inactive">///</span><span class="comment"> 戻り値の </span><span class="inactive">&lt;see cref="</span><span class="type">IDisposable</span>.Dispose<span class="inactive">"/&gt;</span><span class="comment"> を明示的に呼ぶべき。</span>
    <span class="inactive">/// &lt;/remarks&gt;</span>
    <span class="reserved">public static</span> <span class="type">IDisposable</span> WeakSubscribe&lt;<span class="type">T</span>&gt;(<span class="reserved">this</span> <span class="type">IObservable</span>&lt;<span class="type">T</span>&gt; observable, <span class="type">Action</span>&lt;<span class="type">T</span>&gt; onNext)
    {
        <span class="type">WeakReference</span>&lt;<span class="type">IDisposable</span>&gt; weakSubscription = <span class="reserved">null</span>;
        <span class="type">IDisposable</span> subscription = <span class="reserved">null</span>;

        subscription = observable.Subscribe(x =&gt;
        {
            <span class="type">IDisposable</span> d;
            <span class="reserved">if</span> (!weakSubscription.TryGetTarget(<span class="reserved">out</span> d))
            {
                <span class="comment">// 弱参照のターゲットが消えてたらイベント購読解除。</span>
                subscription.Dispose();
                <span class="reserved">return</span>;
            }
            onNext(x);
        });

        <span class="comment">// subscription は↑のラムダ式が参照を持っちゃうことになるので、
        // 別の IDisposable を作ってラップ。</span>
        <span class="reserved">var</span> s = <span class="reserved">new</span> <span class="type">SingleAssignmentDisposable</span>();
        s.Disposable = subscription;

        <span class="comment">// 作った、外から呼ぶ用 IDisposable の弱参照を作る。</span>
        weakSubscription = <span class="reserved">new</span> <span class="type">WeakReference</span>&lt;<span class="type">IDisposable</span>&gt;(s);
        <span class="reserved">return</span> s;
    }
}
</code></pre>


利用例も示しましょう。以下のようになります。

<pre class="source" title="WeakSubcribe の利用例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Reactive.Subjects;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        RunAsync(<span class="reserved">true</span>).Wait();
        RunAsync(<span class="reserved">false</span>).Wait();
    }

    <span class="reserved">private const int</span> Interval = 100;

    <span class="reserved">private static async</span> <span class="type">Task</span> RunAsync(<span class="reserved">bool</span> manualDispose)
    {
        <span class="reserved">if</span> (manualDispose) <span class="type">Console</span>.WriteLine(<span class="literal">"ちゃんと Dispose"</span>);
        <span class="reserved">else</span> <span class="type">Console</span>.WriteLine(<span class="literal">"GC 任せ"</span>);

        <span class="comment">// イベントを、
        // d1: 通常のイベント購読
        // d2: 弱イベント購読</span>
        <span class="reserved">var</span> x = <span class="reserved">new</span> <span class="type">Subject</span>&lt;<span class="reserved">int</span>&gt;();
        <span class="reserved">var</span> d1 = x.Subscribe(i =&gt; <span class="type">Console</span>.WriteLine(<span class="literal">"subscribe "</span> + i));
        <span class="reserved">var</span> d2 = x.WeakSubscribe(i =&gt; <span class="type">Console</span>.WriteLine(<span class="literal">"weak subscribe "</span> + i));
        <span class="reserved">var</span> cts = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();
        <span class="reserved">var</span> t = EventSourceLoop(x, cts.Token);

        <span class="comment">// イベントが飛んでくる間隔の3倍待つ → 3回イベントが来る</span>
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(3 * Interval);

        <span class="reserved">if</span> (manualDispose)
        {
            <span class="comment">// ちゃんと Dispose。
            // 当たり前だけども、以後、イベントは受け取らなくなる。</span>
            d1.Dispose();
            d2.Dispose();
        }
        <span class="reserved">else</span>
        {
            <span class="comment">// Dispose 忘れたままオブジェクトを捨てる。
            // d1 は、Subscribe 内で参照を握っているので GC 対象にならない。メモリ リーク。
            // d2 は、WeakSubscribe 内は弱参照なので、こっちの参照なくせば GC 対象。
            // 以後、イベントは subscribe 側にだけ届く。</span>
            d1 = <span class="reserved">null</span>;
            d2 = <span class="reserved">null</span>;
            <span class="type">GC</span>.Collect();
        }

        <span class="comment">// 同じく3回分待つ</span>
        <span class="reserved">await</span> <span class="type">Task</span>.Delay(300);

        cts.Cancel();
        <span class="reserved">await</span> t;
    }

    <span class="comment">// イベントを飛ばし続けるループ</span>
    <span class="reserved">static async</span> <span class="type">Task</span> EventSourceLoop(<span class="type">IObserver</span>&lt;<span class="reserved">int</span>&gt; observer, <span class="type">CancellationToken</span> ct)
    {
        <span class="reserved">for</span> (<span class="reserved">var</span> i = 0; !ct.IsCancellationRequested; ++i)
        {
            observer.OnNext(i);
            <span class="reserved">await</span> <span class="type">Task</span>.Delay(Interval);
        }
    }
}
</code></pre>


<pre class="console" title="WeakSubcribe の利用例">
ちゃんと Dispose
subscribe 0
weak subscribe 0
subscribe 1
weak subscribe 1
subscribe 2
weak subscribe 2
GC 任せ
subscribe 0
weak subscribe 0
subscribe 1
weak subscribe 1
subscribe 2
weak subscribe 2
subscribe 3
subscribe 4
subscribe 5
</pre>


ちなみにこの例だと、戻り値の IDisposable が弱参照になっていますが、
弱参照な IObservable を作るような作り方もできます(例: [WeakEventExtensions.AsWeakObservable.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Resource/WeakReference/WeakEvent/WeakEventExtensions.AsWeakObservable.cs))。

あと、ファイナライザーを使う方法もなくはないです(例: [WeakEventExtensions.FinalizeDiposable.cs](https://github.com/ufcpp/UfcppSample/blob/master/Chapters/Resource/WeakReference/WeakEvent/WeakEventExtensions.FinalizeDiposable.cs) )が、
こっちの方は、コードはシンプルになるものの、実行性能上はあんまりよくないはず。

まあ、この弱イベント購読も、割かし「最終手段」なもので、本来はちゃんと自前で Dispose 処理すべきものです。
Dispose 処理をかけるのが著しく困難な場面はそこまで頻出しないでしょう。
(ただ、Dispose し忘れを検出するのには、この弱参照やファイナライザーを使ったパターンを使うのも悪くないです。
自動解除がかかるタイミングにデバッグ ログを仕込んでおけば、解除漏れを探すのに役立ちます。)


###<a id="sec-generated-title-7"></a> <a id="cache"></a>誤用: オブジェクト キャッシュ
最後に、弱参照の用途としてたまに上がるものの、そんなによい効果を得られないものについて触れておきます。

オブジェクト作成にそこそこ時間がかかるので、作成したものをしばらく保存しておきたい(キャッシュ)場面は結構あります。
ここで、「しばらく」というのを「積極的に破棄はしないけども、GC のタイミングでは削除してもらって構わない」と考えると、
弱参照の出番のように思えます。

ただ、GC の gen0 (参考: 「[C# のガベージ コレクション](rm_gc.md#garbage-collection)」)は GC 実行頻度が高すぎて想像以上にあっさりキャッシュが消えます。

逆に、gen2 が走るようなタイミングはメモリが不足している状況なので、
ページング(メモリの内容をハードディスクなどに退避して、見かけのメモリ容量を増やす)が発生するなど、
性能に深刻な影響が出たりします。
無頓着に gen2 に割り当てられるようなキャッシュを持ったりすると、かえって動作が遅くなったりするようです。
