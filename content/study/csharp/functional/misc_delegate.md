---
title: "[雑記] デリゲートの利用例"
source_url: "https://ufcpp.net/study/csharp/functional/misc_delegate/"
content_type: "Article"
published_at: "2009-11-22T00:00:00"
updated_at: "2021-10-16T20:14:33"
tags: []
umbraco_id: 1278
parent_id: 1275
sort_order: 2
aliases:
  - "/csharp/functional/misc_delegate/"
  - "/csharp/misc_delegate"
  - "/csharp/misc_delegate.html"
  - "/study/csharp/misc_delegate"
  - "/study/csharp/misc_delegate.html"
---

# \[雑記\] デリゲートの利用例

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
「デリゲートのイメージがつかめない」って人が思った以上に多いようなので、
利用例をいくつか挙げて、図示してみることに。

一言でいうと、「何か処理を外から挿す」というのがデリゲートの役割。


##<a id="sec-generated-title-2"></a> <a id="predicate"></a>述語： 条件式を外から挿す
「[デリゲート](sp_delegate.md)」で書いたことをさらりともう一度。

特定の条件を満たすものだけを抽出するようなメソッドを書きたいとき、条件式をデリゲートにして引数に渡します。
（こういう、外から与える条件式を述語（predicate）と言ったりします。）

例えば、与えられた条件を満たすものの和を求めるメソッドは以下のように書けます。

<pre class="source" title="条件を満たしたものの和を求める" lang="">
<code><span class="reserved">static int</span> Sum(<span class="reserved">int</span>[] a, <span class="type">Predicate</span>&lt;<span class="reserved">int</span>&gt; pred)
{
    <span class="reserved">int</span> sum = <span class="literal">0</span>;
    <span class="reserved">foreach</span> (<span class="reserved">int</span> x <span class="reserved">in</span> a)
        <span class="reserved">if</span> (pred(x))
            sum += x;
    <span class="reserved">return</span> sum;
}
</code></pre>


pred が「外から与える条件」です。
絵にすると以下のような感じ。

<figure>
	[![条件を満たしたものの和を求める。](../../../../assets/media/ufcpp2000/csharp/fig/delegate1.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate1.png)
	<figcaption>条件を満たしたものの和を求める。</figcaption>
</figure>


「条件を外から与える」っていうのは、例えば以下のようにします。

<pre class="source" title="条件「5 より小さい」を与える" lang="">
<code><span class="reserved">var</span> sum = Sum(
    <span class="reserved">new</span>[] { <span class="literal">1</span>, <span class="literal">5</span>, <span class="literal">3</span>, <span class="literal">8</span> },
    x =&gt; x &lt; <span class="literal">5</span>);
</code></pre>


この例の場合、「5より小さい」という条件を与えたことになります。
したがって、1, 5, 3, 8 の中から5より小さい 1, 3 だけが抽出され、その和である 4 が Sum の結果になります。
これも絵にすると以下のような感じ。

<figure>
	[![条件「5よりも小さい」を与える。](../../../../assets/media/ufcpp2000/csharp/fig/delegate2.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate2.png)
	<figcaption>条件「5よりも小さい」を与える。</figcaption>
</figure>



##<a id="sec-generated-title-3"></a> <a id="callback"></a>コールバック： 非同期処理の終了通知
マルチスレッドで複数の処理を同時に実行したりすると、
他のスレッドの終了のタイミングがつかめなくなります。
（「タイミングがつかめない」ってことを指して、こういう処理を非同期（acyncronous）処理と呼びます。）
（マルチスレッドに関しては「[マルチスレッド](../async/sp_thread.md)」参照。）

非同期処理の終了のタイミングで何かをしたい場合、
「処理が終わったらこのメソッドを呼んで欲しい」というものを他スレッド側に渡して呼び出してもらいます。
このような「他スレッドで呼び出して欲しいメソッド」をコールバック（callback）と言います。

例として、以下のようなプログラムを見てみましょう。
（<em>注意</em>: .NET 4 で Task クラスが導入されて以降、この例のような、BeginInvoke を使った非同期処理は書かなくなりました。
ただし、Task クラスを使う場合も、非同期処理や、そのコールバックにはデリゲートを使います。）

<pre class="source" title="非同期処理の例" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;

<span class="reserved">public class</span> <span class="type">Program</span>
{
    <span class="reserved">static void</span> Main()
    {
        <span class="comment">// 非同期処理を開始。</span>
        BeginAsyncWork(Callback);

        <span class="comment">// 同時に別の処理もする。</span>
        <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; <span class="literal">7</span>; i++)
        {
            <span class="comment">// 0.8秒おきにメッセージ表示。</span>
            System.Threading.<span class="type">Thread</span>.Sleep(<span class="literal">800</span>);
            <span class="type">Console</span>.WriteLine(<span class="literal">"メイン処理 {0}"</span>, i);
        }
    }
    <span class="reserved">static void</span> BeginAsyncWork(<span class="type">AsyncCallback</span> callback)
    {
        <span class="type">Action</span> async = AsyncWork;
        async.BeginInvoke(callback, <span class="reserved">null</span>);
    }
    <span class="reserved">static void</span> AsyncWork()
    {
        <span class="reserved">for</span> (<span class="reserved">int</span> i = <span class="literal">0</span>; i &lt; <span class="literal">5</span>; i++)
        {
            <span class="comment">// 1秒おきにメッセージ表示。</span>
            System.Threading.<span class="type">Thread</span>.Sleep(<span class="literal">1000</span>);
            <span class="type">Console</span>.WriteLine(<span class="literal">"非同期処理 {0}"</span>, i);
        }
    }
    <span class="reserved">static void</span> Callback(<span class="type">IAsyncResult</span> r)
    {
        <span class="type">Console</span>.WriteLine(<span class="literal">"終了！"</span>);
    }
}
</code></pre>


メインスレッド（Main 内の処理）では0.8秒に1回「メイン処理」の文字列を、
それとは別スレッド（AsyncWork 内）では1秒に1回「非同期処理」の文字列を表示しています。

BeginAsyncWork で、AsyncWork の非同期実行を開始しています。
前述のとおり、メインスレッド側ではいつ AsyncWork の処理が終わるのかわからないので、
コールバックを渡して、AsyncWork の実行が終わったら呼び出してもらいます。

<figure>
	[![BeginAsyncWork](../../../../assets/media/ufcpp2000/csharp/fig/delegate3.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate3.png)
	<figcaption>BeginAsyncWork</figcaption>
</figure>


今回の場合、コールバックとして、「終了！」という文字列を表示するメソッドを渡しています。

<figure>
	[![BeginAsyncWork に Callback を渡す。](../../../../assets/media/ufcpp2000/csharp/fig/delegate4.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate4.png)
	<figcaption>BeginAsyncWork に Callback を渡す。</figcaption>
</figure>


ということで、出力結果は以下のような感じになります。

<pre class="console" title="実行結果">
メイン処理 0
非同期処理 0
メイン処理 1
非同期処理 1
メイン処理 2
非同期処理 2
メイン処理 3
メイン処理 4
非同期処理 3
メイン処理 5
非同期処理 4
終了！
メイン処理 6
</pre>



##<a id="sec-generated-title-4"></a> <a id="event"></a>イベント処理
デリゲートといえばイベント処理（「[イベント](sp_event.md)」参照。）
詳しくは 「[イベント](sp_event.md)」 の方を読んでもらうとして、
ここでは GUI アプリのイベント処理がどんな感じで動いているかをイラストレーション。

ものすごい大雑把に模擬的な書き方をすると、GUI アプリってのは以下のような構造で動いています。
（メッセージループって言います。）

<pre class="source" title="GUI アプリのメッセージループ" lang="">
<code><span class="type">Message</span> msg;
<span class="reserved">while</span> (GetMessage(<span class="reserved">out</span> msg)) <span class="comment">// OS から「マウスクリック」とかのメッセージが来てないか調べる。</span>
{
    ProcessMessage(msg); <span class="comment">// メッセージを処理。</span>
}
</code></pre>


ProcessMessage の中身も模擬的に書くと、以下のような感じ。

<pre class="source" title="メッセージ処理" lang="">
<code><span class="reserved">void</span> ProcessMessage(<span class="type">Message</span> msg)
{
    <span class="reserved">if</span> (msg == マウスクリック) MouseClick();
    <span class="reserved">else if</span> (msg == キーを押した) KeyDown();
    <span class="reserved">else if</span> (msg == キーを離した) KeyUp();
    <span class="input">以下略</span>
}
</code></pre>


ここで出てきた MouseClick とか KeyDown とかはデリゲートです。
「[イベント](sp_event.md#event)」になっていて、GUI ライブラリ利用者が任意のイベントハンドラーを外から挿せるようになっています。
図にすると以下のような感じ。

<figure>
	[![メッセージループ。](../../../../assets/media/ufcpp2000/csharp/fig/delegate5.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate5.png)
	<figcaption>メッセージループ。</figcaption>
</figure>


で、例えば、クリックされたときにメッセージボックスを出したいとします。
Windows Forms を使うなら、以下のようなコードを書きます。

<pre class="source" title="クリックイベントを拾ってメッセージボックスを表示" lang="">
<code><span class="reserved">var</span> form = <span class="reserved">new</span> <span class="type">Form</span>();
form.Click += (sender, e) =&gt; { <span class="type">MessageBox</span>.Show(<span class="literal">"Click!"</span>); };
</code></pre>


その結果、メッセージループ内の Click のところで MessageBox.Show が呼び出されるようになります。

<figure>
	[![クリックイベントを処理。](../../../../assets/media/ufcpp2000/csharp/fig/delegate6.png)](../../../../assets/media/ufcpp2000/csharp/fig/delegate6.png)
	<figcaption>クリックイベントを処理。</figcaption>
</figure>
