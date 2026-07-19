---
title: "イベント"
source_url: "https://ufcpp.net/study/csharp/functional/sp_event/"
content_type: "Article"
published_at: "2015-05-06T14:10:25"
updated_at: "2015-01-02T00:00:00"
tags: []
umbraco_id: 1281
parent_id: 1275
sort_order: 7
aliases:
  - "/csharp/functional/sp_event/"
  - "/csharp/sp_event"
  - "/csharp/sp_event.html"
  - "/study/csharp/sp_event"
  - "/study/csharp/sp_event.html"
---

# イベント

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# には、イベント駆動型のプログラム作成を容易にするため、
イベント処理用の構文 event が用意されています。
event は、デリゲートに対する「[プロパティ](../oop/oo_property.md#property)」のようなもので、
以下のような特徴を持っています。

* デリゲート呼び出しはクラス内部からのみ可能。

* 外部からはデリゲートの追加/削除のみが可能。

##### <a id="sec-generated-title-2"></a>サンプル
[https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Event/EventDriven](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Event/EventDriven)


##### <a id="sec-generated-title-3"></a>ポイント
* イベント： プロパティのデリゲート版。イベント駆動処理に使われるのでこの名前になっています。

* イベント駆動処理には、単なるデリゲート型のプロパティでは機能が不十分で、 「呼び出しはクラス内からのみ、外部からできるのは登録・削除のみ」という制約が必要になります。

* C# には、この制約を満たすような専用の構文(event 構文)があります。

* 補足: といっても、event 構文には使いにくい面もあるので注意。参考:「[【雑記】イベントの購読とその解除](misceventsubscribe.md)」



##<a id="sec-generated-title-4"></a> <a id="event-driven"></a>イベント駆動型
「キーボードのボタンが押された」とか「マウスが移動した」等の、
コンピュータ上で発生するなんらかの事象のことを<strong id="event" class="keyword">イベント</strong>（event）といい、
イベントが発生したときに行う処理のことを<strong id="eventhandler" class="keyword">イベント ハンドラー</strong>（event handler）と呼びます。
このように、イベントとそれに対する処理により動作するようなプログラムのことを<strong id="edriven" class="keyword">イベント駆動型</strong>（event drive）プログラムと呼びます。

ポイントは、図1に示すように、イベントを発生させる側と受け取って何か処理をする側がわかれることです。

<figure>
	[![イベントの概要](../../../../assets/media/ufcpp2000/csharp/fig/ObservableObserver.png)](../../../../assets/media/ufcpp2000/csharp/fig/ObservableObserver.png)
	<figcaption>イベントの概要</figcaption>
</figure>


event source, observable, event sender, ... など、呼び方はいろいろありますが、流儀や文脈の差であって、だいたい同じものです。

イベント駆動の最たる例は、GUI アプリでしょう。
GUI アプリでは、ユーザからのマウスやキーボード、タッチなどの入力イベント発生を待ち、
それらに対して何らかの処理を行っていくことでプログラムが動作しています。

とはいえ、GUI アプリで例示するのは、他にもいろいろ説明しないといけないことが多いので、
ここではコンソール アプリで説明していきましょう。
コンソール アプリでも、「ユーザーからのキー入力を待つ」というのはイベント処理だと考えることができます。
（GUI アプリに関しては「[GUI アプリケーション](../lib/lib_forms.md)」や「[Windows Presentation Foundation](../../dotnet/index.md#wpf)」参照。）


##<a id="sec-generated-title-5"></a> <a id="ex"></a>イベント駆動型プログラムの例
イベント駆動の例として、
キーボードからの入力を受け取って処理を行うプログラムを作っていきます。
初めに、イベント発生側と受取側があまりわかれていないベタな例を示しましょう。
時節以降で、これを分離していきます。

簡単なサンプルとして、
1秒おきに時刻を表示するプログラムを作ります。
キーボードからの入力に応じて、
表示の一時停止や、表示形式の変更、プログラムの停止等を行います。

この最初のサンプルでは、
Main 関数内のループで時刻の表示を行い、
別のスレッドでイベント(ユーザからのキー入力)の発生を待ち続け、
同時にイベントの処理もこのスレッド内で行います。

<pre class="source" title="イベント処理の例 version 1" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 時刻の表示形式</span>
    <span class="reserved">const string</span> FULL = <span class="literal">"yyyy/dd/MM hh:mm:ss\n"</span>;
    <span class="reserved">const string</span> DATE = <span class="literal">"yyyy/dd/MM\n"</span>;
    <span class="reserved">const string</span> TIME = <span class="literal">"hh:mm:ss\n"</span>;

    <span class="reserved">static bool</span> isSuspended = <span class="reserved">true</span>;  <span class="comment">// プログラムの一時停止フラグ。</span>
    <span class="reserved">static string</span> timeFormat = TIME; <span class="comment">// 時刻の表示形式。</span>

    <span class="reserved">static void</span> Main()
    {
        WriteHelp();

        <span class="reserved">var</span> cts = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();

        <span class="type">Task</span>.WhenAll(
            <span class="type">Task</span>.Run(() =&gt; EventLoop(cts)),
            TimerLoop(cts.Token)
            ).Wait();
    }

    <span class="comment">// 毎秒時刻表示のループ</span>
    <span class="reserved">private static async</span> <span class="type">Task</span> TimerLoop(<span class="type">CancellationToken</span> ct)
    {
        <span class="reserved">while</span> (!ct.IsCancellationRequested)
        {
            <span class="reserved">if</span> (!isSuspended)
            {
                <span class="comment">// 1秒おきに現在時刻を表示。</span>
                <span class="type">Console</span>.Write(<span class="type">DateTime</span>.Now.ToString(timeFormat));
            }
            <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        }
    }

    <span class="comment">// キー受付のループ</span>
    <span class="reserved">static void</span> EventLoop(<span class="type">CancellationTokenSource</span> cts)
    {
        <span class="reserved">while</span> (!cts.IsCancellationRequested)
        {
            <span class="comment">// 文字を読み込む
            // (「キーが押される」というイベントの発生を待つ)</span>
            <span class="reserved">string</span> line = <span class="type">Console</span>.ReadLine();
            <span class="reserved">char</span> eventCode = line.Length == 0 ? <span class="literal">'\0'</span> : line[0];

            <span class="comment">// イベント処理</span>
            <span class="reserved">switch</span> (eventCode)
            {
                <span class="reserved">case</span> <span class="literal">'r'</span>: <span class="comment">// run</span>
                    isSuspended = <span class="reserved">false</span>;
                    <span class="reserved">break</span>;
                <span class="reserved">case</span> <span class="literal">'s'</span>: <span class="comment">// suspend</span>
                    isSuspended = <span class="reserved">true</span>;
                    <span class="reserved">break</span>;
                <span class="reserved">case</span> <span class="literal">'f'</span>: <span class="comment">// full</span>
                    timeFormat = FULL;
                    <span class="reserved">break</span>;
                <span class="reserved">case</span> <span class="literal">'d'</span>: <span class="comment">// date</span>
                    timeFormat = DATE;
                    <span class="reserved">break</span>;
                <span class="reserved">case</span> <span class="literal">'t'</span>: <span class="comment">// time</span>
                    timeFormat = TIME;
                    <span class="reserved">break</span>;
                <span class="reserved">case</span> <span class="literal">'q'</span>: <span class="comment">// quit</span>
                    cts.Cancel();
                    <span class="reserved">break</span>;
                <span class="reserved">default</span>: <span class="comment">// ヘルプ</span>
                    WriteHelp();
                    <span class="reserved">break</span>;
            }
        }
    }

    <span class="reserved">private static void</span> WriteHelp()
    {
        <span class="type">Console</span>.Write(
            <span class="literal">"使い方\n"</span> +
            <span class="literal">"r (run)    : 時刻表示を開始します。\n"</span> +
            <span class="literal">"s (suspend): 時刻表示を一時停止します。\n"</span> +
            <span class="literal">"f (full)   : 時刻の表示形式を“日付＋時刻”にします。\n"</span> +
            <span class="literal">"d (date)   : 時刻の表示形式を“日付のみ”にします。\n"</span> +
            <span class="literal">"t (time)   : 時刻の表示形式を“時刻のみ”にします。\n"</span> +
            <span class="literal">"q (quit)   : プログラムを終了します。\n"</span>
            );
    }
}
</code></pre>


このプログラムでは、<code>EventLoop</code> というメソッドの中で、
イベント(ユーザのキー入力)発生を待ち、
その処理を行っています。
ここで、イベントの発生を待つ部分は他のプログラムでも利用可能な汎用的な処理です。
そのため、次のステップとして、
イベント発生待ちの部分を取り出して、汎用ルーチン化することを考えます。
すなわち、イベント発生待受け部(イベントループ)とイベント処理部(イベント ハンドラー)を分けて実装することにします。


##<a id="sec-generated-title-6"></a> <a id="handler"></a>イベント ハンドラー
これまでで見てきたように、
イベント駆動型のプログラムは大きく分けて
<em>「イベント発生待受け部」（イベントループ）と「イベント処理部」（イベント ハンドラー）の2つの部分からなります</em>。
イベント処理部はプログラムごとに異なる処理を行うことになりますが、
イベント発生待受け部は汎用的な処理で、
どんなプログラムでも共通の処理になります。

そこで、イベント発生待受け部のみを独立させ、汎用ルーチン化することを考えます。
といっても、これの実現はそれほど難しい事ではなく、
単に[デリゲート](sp_delegate.md)を用いてイベント処理を他のメソッドに譲り渡してしまえばいいだけのことです。
したがって、イベント発生待受け用クラスは以下のようになります。

<pre class="source" title="キーボードからの入力イベント待受けクラス" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="comment">// イベント処理用のデリゲート</span>
<span class="reserved">delegate void</span> <span class="type">KeyboadEventHandler</span>(<span class="reserved">char</span> eventCode);

<span class="inactive">/// &lt;summary&gt;
///</span><span class="comment"> キーボードからの入力イベント待受けクラス。</span>
<span class="inactive">/// &lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">KeyboardEventLoop</span>
{
    <span class="type">KeyboadEventHandler</span> _onKeyDown;

    <span class="reserved">public</span> KeyboardEventLoop(<span class="type">KeyboadEventHandler</span> onKeyDown)
    {
        _onKeyDown = onKeyDown;
    }

    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> 待受け開始。</span>
    <span class="inactive">/// &lt;/summary&gt;
    /// &lt;param name="</span>ct<span class="inactive">"&gt;</span><span class="comment">待ち受けを終了したいときにキャンセルする。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="reserved">public</span> <span class="type">Task</span> Start(<span class="type">CancellationToken</span> ct)
    {
        <span class="reserved">return</span> <span class="type">Task</span>.Run(() =&gt; EventLoop(ct));
    }

    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> イベント待受けループ。</span>
    <span class="inactive">/// &lt;/summary&gt;</span>
    <span class="reserved">void</span> EventLoop(<span class="type">CancellationToken</span> ct)
    {
        <span class="comment">// イベントループ</span>
        <span class="reserved">while</span> (!ct.IsCancellationRequested)
        {
            <span class="comment">// 文字を読み込む
            // (「キーが押される」というイベントの発生を待つ)</span>
            <span class="reserved">string</span> line = <span class="type">Console</span>.ReadLine();
            <span class="reserved">char</span> eventCode = (line == <span class="reserved">null</span> || line.Length == 0) ? <span class="literal">'\0'</span> : line[0];

            <span class="comment">// イベント処理はデリゲートを通して他のメソッドに任せる。</span>
            _onKeyDown(eventCode);
        }
    }
}
</code></pre>


このようにしてイベント待受け部から独立させたイベント処理部(この例においては <code>onKeyDown</code> デリゲート)のことを<em>イベント ハンドラー</em>と呼びます。
そして、このクラスを用いて先ほどのサンプルプログラムを書き換えると以下のようになります。

<pre class="source" title="イベント処理の例 version 2" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// 時刻の表示形式</span>
    <span class="reserved">const string</span> FULL = <span class="literal">"yyyy/dd/MM hh:mm:ss\n"</span>;
    <span class="reserved">const string</span> DATE = <span class="literal">"yyyy/dd/MM\n"</span>;
    <span class="reserved">const string</span> TIME = <span class="literal">"hh:mm:ss\n"</span>;

    <span class="reserved">static</span> <span class="type">KeyboardEventLoop</span> eventLoop;
    <span class="reserved">static bool</span> isSuspended = <span class="reserved">true</span>;  <span class="comment">// プログラムの一時停止フラグ。</span>
    <span class="reserved">static string</span> timeFormat = TIME; <span class="comment">// 時刻の表示形式。</span>

    <span class="reserved">static void</span> Main()
    {
        WriteHelp();

        <span class="reserved">var</span> cts = <span class="reserved">new</span> <span class="type">CancellationTokenSource</span>();
        eventLoop = <span class="reserved">new</span> <span class="type">KeyboardEventLoop</span>(code =&gt; OnKeyDown(code, cts));

        <span class="type">Task</span>.WhenAll(
            eventLoop.Start(cts.Token),
            TimerLoop(cts.Token)
            ).Wait();
    }

    <span class="comment">// 毎秒時刻表示のループ</span>
    <span class="reserved">private static async</span> <span class="type">Task</span> TimerLoop(<span class="type">CancellationToken</span> ct)
    {
        <span class="reserved">while</span> (!ct.IsCancellationRequested)
        {
            <span class="reserved">if</span> (!isSuspended)
            {
                <span class="comment">// 1秒おきに現在時刻を表示。</span>
                <span class="type">Console</span>.Write(<span class="type">DateTime</span>.Now.ToString(timeFormat));
            }
            <span class="reserved">await</span> <span class="type">Task</span>.Delay(1000);
        }
    }

    <span class="comment">// イベント処理部。</span>
    <span class="reserved">static void</span> OnKeyDown(<span class="reserved">char</span> eventCode, <span class="type">CancellationTokenSource</span> cts)
    {
        <span class="reserved">switch</span> (eventCode)
        {
            <span class="reserved">case</span> <span class="literal">'r'</span>: <span class="comment">// run</span>
                isSuspended = <span class="reserved">false</span>;
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="literal">'s'</span>: <span class="comment">// suspend</span>
                <span class="type">Console</span>.Write(<span class="literal">"\n一時停止します\n"</span>);
                isSuspended = <span class="reserved">true</span>;
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="literal">'f'</span>: <span class="comment">// full</span>
                timeFormat = FULL;
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="literal">'d'</span>: <span class="comment">// date</span>
                timeFormat = DATE;
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="literal">'t'</span>: <span class="comment">// time</span>
                timeFormat = TIME;
                <span class="reserved">break</span>;
            <span class="reserved">case</span> <span class="literal">'q'</span>: <span class="comment">// quit</span>
                cts.Cancel();
                <span class="reserved">break</span>;
            <span class="reserved">default</span>: <span class="comment">// ヘルプ</span>
                WriteHelp();
                <span class="reserved">break</span>;
        }
    }

    <span class="reserved">private static void</span> WriteHelp()
    {
        <span class="type">Console</span>.Write(
            <span class="literal">"使い方\n"</span> +
            <span class="literal">"r (run)    : 時刻表示を開始します。\n"</span> +
            <span class="literal">"s (suspend): 時刻表示を一時停止します。\n"</span> +
            <span class="literal">"f (full)   : 時刻の表示形式を“日付＋時刻”にします。\n"</span> +
            <span class="literal">"d (date)   : 時刻の表示形式を“日付のみ”にします。\n"</span> +
            <span class="literal">"t (time)   : 時刻の表示形式を“時刻のみ”にします。\n"</span> +
            <span class="literal">"q (quit)   : プログラムを終了します。\n"</span>
            );
    }
}
</code></pre>



##<a id="sec-generated-title-7"></a> <a id="event-keyword"></a>C# の event 構文
ここまでの話をもう1歩推し進めて、
イベント ハンドラーの追加削除を自由にできるようにしたいと思います。
これはイベント ハンドラー用のデリゲート変数を public にしてしまえば簡単にできたりもしますが、
「[プロパティ](../oop/oo_property.md)」で説明したように、
メンバー変数を外部から直接取得/書換え可能にすべきではありません。
デリゲート変数も例外ではなく、取得/書換えはアクセッサを介して行うべきです。

それならば、デリゲート型のプロパティを用意すればいいのではないかと思われるかもしれませんが、それではまだ不十分です。
なぜかといいますと、イベント ハンドラー用のデリゲートには以下のような条件が求められるからです。

* デリゲート呼び出しはクラス内部からのみ可能。

* 外部からはデリゲートの追加/削除のみが可能。


すなわち、クラス内部からは通常のデリゲート変数と同様に扱え、
外部からは <code>+=</code>、<code>-=</code> 演算子によるデリゲートの追加/削除のみを行えるような仕組みが欲しいわけです。
プロパティではこのような仕組みは提供できません。

そこで、C# にはこの仕組みを実現するために <em>event</em> というキーワードが用意されています。
利用方法は簡単で、イベント ハンドラーとして使用したいデリゲート型の変数宣言時に <code>event</code> という修飾子を付けるだけです。

<pre class="source" title="event キーワード" lang="">
<code><span class="reserved">event</span> <span class="input">デリゲート型</span> <span class="input">イベント ハンドラー名</span>;
</code></pre>


このようにして宣言した変数は“<em>イベント</em>”と呼ばれ、
前述のように内部からは普通のデリゲートと同じように利用でき、
外部からは <code>+=</code>、<code>-=</code> のみが利用できるようになります。

また、イベントはプロパティの <code>get</code>/<code>set</code> と同じように、
<code>add</code>/<code>remove</code> というキーワードを用いて、
追加/削除時の処理を明示的に指定することもできます。
（省略可能。省略すると、デリゲートを格納するフィールドと、add/remove アクセサーをコンパイラーが自動生成してくれる。）

<pre class="source" title="イベントプロパティ" lang="">
<code><span class="reserved">event</span> <span class="input">デリゲート型</span> <span class="input">イベント ハンドラー名</span>
{
  <span class="reserved">add</span>
  {
<span class="comment">    // addアクセサ
    //  ここにイベント ハンドラー追加時の処理を書く。</span>
  }
  <span class="reserved">remove</span>
  {
<span class="comment">    // removeアクセサ
    //  ここにイベント ハンドラー削除時の処理を書く。</span>
  }
<span class="comment">  // add/remove アクセッサ共に、
  // 追加/削除したいイベント ハンドラーは value という名前の変数に格納されている。</span>
}
</code></pre>


このように明示的に追加/削除時の処理を追加したものをイベントプロパティと呼びます。

それでは、先ほど作成したイベント発生待受けクラスを event を用いて書き換えてみましょう。
（event キーワードを足して public にしただけ。）

<pre class="source" title="イベント発生待受けクラス 完成形" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;
<span class="reserved">using</span> System.Threading.Tasks;

<span class="comment">// イベント処理用のデリゲート</span>
<span class="reserved">delegate void</span> <span class="type">KeyboadEventHandler</span>(<span class="reserved">char</span> eventCode);

<span class="inactive">/// &lt;summary&gt;
///</span><span class="comment"> キーボードからの入力イベント待受けクラス。</span>
<span class="inactive">/// &lt;/summary&gt;</span>
<span class="reserved">class</span> <span class="type">KeyboardEventLoop</span>
{
    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> キー入力があった時に呼ばれるイベント。</span>
    <span class="inactive">/// &lt;/summary&gt;</span>
    <em><span class="reserved">public event</span> <span class="type">KeyboadEventHandler</span> OnKeyDown;</em>

    <span class="reserved">public</span> KeyboardEventLoop() { }
    <span class="reserved">public</span> KeyboardEventLoop(<span class="type">KeyboadEventHandler</span> onKeyDown)
    {
        OnKeyDown += onKeyDown;
    }

    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> 待受け開始。</span>
    <span class="inactive">/// &lt;/summary&gt;
    /// &lt;param name="</span>ct<span class="inactive">"&gt;</span><span class="comment">待ち受けを終了したいときにキャンセルする。</span><span class="inactive">&lt;/param&gt;</span>
    <span class="reserved">public</span> <span class="type">Task</span> Start(<span class="type">CancellationToken</span> ct)
    {
        <span class="reserved">return</span> <span class="type">Task</span>.Run(() =&gt; EventLoop(ct));
    }

    <span class="inactive">/// &lt;summary&gt;
    ///</span><span class="comment"> イベント待受けループ。</span>
    <span class="inactive">/// &lt;/summary&gt;</span>
    <span class="reserved">void</span> EventLoop(<span class="type">CancellationToken</span> ct)
    {
        <span class="comment">// イベントループ</span>
        <span class="reserved">while</span> (!ct.IsCancellationRequested)
        {
            <span class="comment">// 文字を読み込む
            // (「キーが押される」というイベントの発生を待つ)</span>
            <span class="reserved">string</span> line = <span class="type">Console</span>.ReadLine();
            <span class="reserved">char</span> eventCode = (line == <span class="reserved">null</span> || line.Length == 0) ? <span class="literal">'\0'</span> : line[0];

            <span class="comment">// イベント処理は event を通して他のメソッドに任せる。</span>
            OnKeyDown(eventCode);
        }
    }
}
</code></pre>



##<a id="sec-generated-title-8"></a> <a id="auto-event"></a>補足: 自動イベント
前節で構文を説明した通り、event 構文には、add/remove アクセサーを明示的に書く方法と、省略して書く方法があります。
省略して書く方では、add/remove がコンパイラーによって自動生成されています。
ちなみに、この、コンパイラーによって自動生成されるもののことを自動イベント(auto-event)と言ったりします。

補足的にはなりますが、この自動イベントの自動生成結果について少し話しておきます。

例えば、以下のようなイベントを書いたとします。

<pre class="source" title="自動イベント" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">EventSample</span>
{
    <span class="reserved">public event</span> <span class="type">EventHandler</span> X;
}
</code></pre>


C# 4.0 以降、コンパイラーによる自動生成の結果は以下のような意味合いのものになります。

<pre class="source" title="コンパイラーによる自動生成の結果" lang="">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> System.Threading;

<span class="reserved">class</span> <span class="type">EventSample</span>
{
    <span class="reserved">private</span> <span class="type">EventHandler</span> _X; <span class="comment">// 注意: コンパイラー自動生成結果的には X</span>

    <span class="reserved">public event</span> <span class="type">EventHandler</span> X
    {
        <span class="reserved">add</span>
        {
            <span class="type">EventHandler</span> x2;
            <span class="reserved">var</span> x1 = _X;
            <span class="reserved">do</span>
            {
                x2 = x1;
                <span class="reserved">var</span> x3 = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Combine(x2, <span class="reserved">value</span>);
                x1 = <span class="type">Interlocked</span>.CompareExchange(<span class="reserved">ref</span> _X, x3, x2);
            }
            <span class="reserved">while</span> (x1 != x2);
        }
        <span class="reserved">remove</span>
        {
            <span class="type">EventHandler</span> x2;
            <span class="reserved">var</span> x1 = _X;
            <span class="reserved">do</span>
            {
                x2 = x1;
                <span class="reserved">var</span> x3 = (<span class="type">EventHandler</span>)<span class="type">Delegate</span>.Remove(x2, <span class="reserved">value</span>);
                x1 = <span class="type">Interlocked</span>.CompareExchange(<span class="reserved">ref</span> _X, x3, x2);
            }
            <span class="reserved">while</span> (x1 != x2);
        }
    }
}
</code></pre>


結構大げさなコードが生成されていますが、これは、マルチスレッド動作で正しく動くことを保証するためにこうなっています。
(こういうマルチスレッド動作保証の方法については、別途、非同期処理がらみのページで説明予定。)
マルチスレッド動作を気にしなくていいなら、意味的には以下のコードと同じです。

<pre class="source" title="マルチスレッド動作を気にしないならこれでいい" lang="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">EventSample</span>
{
    <span class="reserved">private</span> <span class="type">EventHandler</span> _X; <span class="comment">// 注意: コンパイラー自動生成結果的には X</span>

    <span class="reserved">public event</span> <span class="type">EventHandler</span> X
    {
        <span class="reserved">add</span> { _X += <span class="reserved">value</span>; }
        <span class="reserved">remove</span> { _X -= <span class="reserved">value</span>; }
    }
}
</code></pre>


1点、注意があります。
event とは別に、普通のデリゲート型のフィールドが作られますが、
実際のコンパイラー生成結果的には、event 名とフィールド名はまったく同じ名前で、どちらも X になります。
(C# の言語仕様上は、同名のメンバーを2つ持つことはできませんが、
.NET の中間言語仕様上は、event とフィールドみたいに、異種メンバーであれば同じ名前であっても構いません。)
クラスの外から <code>+=</code> / <code>-=</code> しているのは event の方の X で、
クラスの中からデリゲート呼び出ししているのはフィールドの方の X だったりします。
