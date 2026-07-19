---
title: "[雑記] 非同期制御フロー"
source_url: "https://ufcpp.net/study/csharp/async/misc_asyncflow/"
content_type: "Article"
published_at: "2012-09-30T00:00:00"
updated_at: "2015-05-06T14:12:19"
tags: []
umbraco_id: 1333
parent_id: 1326
sort_order: 6
aliases:
  - "/csharp/async/misc_asyncflow/"
  - "/csharp/misc_asyncflow"
  - "/csharp/misc_asyncflow.html"
  - "/study/csharp/misc_asyncflow"
  - "/study/csharp/misc_asyncflow.html"
---

# \[雑記\] 非同期制御フロー

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
C# 5.0のasync/awaitがなかったころ、少し複雑目な非同期制御フローをどうやって実現していたかという話。

C# 5.0を使えない状況下で非同期処理を書くことになった場合の参考としてや、async/awaitがどうやって実現されているかを知るきっかけになると思います。

* 
[サンプル コード（ZIP 形式。proj/sln 含め一式。）](../../../../assets/media/ufcpp2000/csharp/source/ShowDialogAsyncSample.zip)




##### <a id="sec-generated-title-2"></a>ポイント
* C# 5.0（await演算子）便利だなー

* await演算子が内部的にやっていることは、イテレーターに近い

* なので、昔はイテレーターを使って非同期処理をすることが結構あった



##<a id="sec-generated-title-3"></a> <a id="requirement"></a>サンプルの要件
今回の例として使うのは、Figure 1に示すような、確認ダイアログ表示のフロー。

<figure>
	[![確認ダイアログを表示する例](../../../../assets/media/ufcpp2000/csharp/fig/asyncflow1.png)](../../../../assets/media/ufcpp2000/csharp/fig/asyncflow1.png)
	<figcaption>確認ダイアログを表示する例</figcaption>
</figure>


要は、何かを実行するにあたって、特定条件下では確認ダイアログの表示が必要で、すべてのダイアログで「OK」を押したときにだけ実行に移るという仕組みです。

たとえばゲームでも想像してもらって、「このアイテムはレアですが、本当に合成素材にしますか？」みたいなの。

* レアですよ？

* 合成して強化したアイテムですよ？

* これ以上合成しても上限に達してて変化しませんよ？


など、確認すべき項目がいくつかあります。


##<a id="sec-generated-title-4"></a> <a id="control-flow"></a>制御フロー
ダイアログ表示のコードを同期的に書ける場合、特に問題もなく書けると思います。

たとえばWPFだと、Windowクラス（System.Windows名前空間）のShowDialogメソッドで同期的にダイアログ表示できるので、それほど困りません（ダイアログを表示している間、呼び出し元のウィンドウは止まってしまいますが）。

以下のように書けます。

<pre class="source" title="同期的にダイアログ表示" lang="">
<code><span class="reserved">private bool</span> CheckBlocking()
{
    <span class="reserved">if</span> (<span class="reserved">this</span>.Check1.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="type">Dialog</span>.ShowDialog(<span class="literal">"確認 1"</span>, <span class="literal">"1つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check2.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="type">Dialog</span>.ShowDialog(<span class="literal">"確認 2"</span>, <span class="literal">"2つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="type">Dialog</span>.ShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">return true</span>;
}
</code></pre>


問題は、非同期に書かざるを得ない場合です。Silverlightなんかはそうですし、最近実際に困ったのは[Unity](http://unity3d.com/)での話。

その「実際の話」では、ダイアログを表示するためのAPIが、引数にコールバック用のデリゲートを渡すタイプのAPIでした。

<pre class="source" title="" lang="">
<code><span class="inactive">/// &lt;summary&gt;
///</span><span class="comment"> コールバック型の非同期ダイアログ表示。</span>
<span class="inactive">/// &lt;/summary&gt;
/// &lt;param name="title"&gt;</span><span class="comment">ダイアログのタイトル文字列。</span><span class="inactive">&lt;/param&gt;
/// &lt;param name="message"&gt;</span><span class="comment">ダイアログの本文。</span><span class="inactive">&lt;/param&gt;
/// &lt;param name="onClose"&gt;</span><span class="comment">コールバック（OK が押されたら true、Cancel が押されたら false を渡す）。</span><span class="inactive">&lt;/param&gt;</span>
<span class="reserved">public static void</span> BeginShowDialog(<span class="reserved">string</span> title, <span class="reserved">string</span> message, <span class="type">Action</span>&lt;<span class="reserved">bool</span>&gt; onClose)
</code></pre>


で、これを使ってダイアログを表示する部分ですが、チーム開発の「後から継ぎ足し」の結果、気が付けば、以下のようなコードが出来上がっていました。

<span class="expand-button" title="展開/折畳">（クリックしてソースコードを表示（割と見るに堪えないので初期状態を非表示に））</span>
<div class="expand-panel" markdown="1" title="（クリックしてソースコードを表示（割と見るに堪えないので初期状態を非表示に））">
    
<pre class="source" title="コールバック型 API で無理な制御フローを書いた例" lang="">
<code><span class="reserved">private void</span> BeginCheck(<span class="type">Action</span>&lt;<span class="reserved">bool</span>&gt; onComplete)
{
    <span class="reserved">if</span> (<span class="reserved">this</span>.Check1.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 1"</span>, <span class="literal">"1つ目の確認作業"</span>, result =&gt;
        {
            <span class="reserved">if</span> (!result)
            {
                onComplete(<span class="reserved">false</span>);
                <span class="reserved">return</span>;
            }

            <span class="reserved">if</span> (<span class="reserved">this</span>.Check2.IsChecked ?? <span class="reserved">false</span>)
            {
                <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 2"</span>, <span class="literal">"2つ目の確認作業"</span>, result2 =&gt;
                {
                    <span class="reserved">if</span> (!result2)
                    {
                        onComplete(<span class="reserved">false</span>);
                        <span class="reserved">return</span>;
                    }

                    <span class="reserved">if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
                    {
                        <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>, result3 =&gt;
                        {
                            onComplete(result3);
                        });
                    }
                    <span class="reserved">else</span>
                        onComplete(<span class="reserved">true</span>);
                });
            }
            <span class="reserved">else if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
            {
                <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>, result3 =&gt;
                {
                    onComplete(result3);
                });
            }
            <span class="reserved">else</span>
                onComplete(<span class="reserved">true</span>);
        });
    }
    <span class="reserved">else if</span> (<span class="reserved">this</span>.Check2.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 2"</span>, <span class="literal">"2つ目の確認作業"</span>, result =&gt;
        {
            <span class="reserved">if</span> (!result)
            {
                onComplete(<span class="reserved">false</span>);
                <span class="reserved">return</span>;
            }

            <span class="reserved">if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
            {
                <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>, result3 =&gt;
                {
                    onComplete(result);
                });
            }
            <span class="reserved">else</span>
                onComplete(<span class="reserved">true</span>);
        });
    }
    <span class="reserved">else if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>, result3 =&gt;
        {
            onComplete(result3);
        });
    }
    <span class="reserved">else</span>
        onComplete(<span class="reserved">true</span>);
}
</code></pre>


    
</div>

条件分岐や途中で処理を打ち切ったりするのはコールバックで書くのが大変で、コピペ コードが散乱してしまっています。見るからにダメなコードですが、3つ目のダイアログ表示まではかろうじて「書けるには書けた」のでごまかしごまかしここまで来てしまった状態。

そして、仕様追加で4つ目のダイアログが必要になった時点でくじけることに。


##<a id="sec-generated-title-5"></a> <a id="csharp5"></a>C# 5.0
C# 5.0が使えるなら、つまり、Visual Studio 2012で、.NET Framework 4.5が入っていれば、非常に簡単な解決策があります。

Taskクラスを返す非同期APIを用意して、await演算子を使うだけ。


###<a id="sec-generated-title-6"></a> <a id="task-class"></a>Task クラス
コールバックを渡すタイプのAPIだとawait演算子を使えないので、まずはTaskクラス（System.Threading.Tasks名前空間）を返すタイプのAPIに変換します。以下のようになります。

<pre class="source" title="Task クラスを返すタイプの API" lang="">
<code><span class="reserved">public static</span> <span class="type">Task</span>&lt;<span class="reserved">bool</span>&gt; ShowDialogAsync(<span class="reserved">string</span> title, <span class="reserved">string</span> message)
{
    <span class="reserved">var</span> tcs = <span class="reserved">new</span> <span class="type">TaskCompletionSource</span>&lt;<span class="reserved">bool</span>&gt;();
    BeginShowDialog(title, message, result =&gt; { tcs.TrySetResult(result); });
    <span class="reserved">return</span> tcs.Task;
}
</code></pre>


（単純化のため、例外処理をさぼっています）

Taskクラス自体は.NET Framework 4の頃からあるので、それ以降のバージョンを使えるなら、このタイプのAPIを用意しておくといいでしょう。


###<a id="sec-generated-title-7"></a> <a id="await-op"></a>await 演算子
そして、ダイアログを表示する部分は以下のように書きます。

<pre class="source" title="非同期メソッド（await 演算子）を使ったダイアログ表示フロー" lang="">
<code><span class="reserved">private <em>async</em></span> <span class="type"><em>Task</em></span>&lt;<span class="reserved">bool</span>&gt; Check<em>Async</em>()
{
    <span class="reserved">if</span> (<span class="reserved">this</span>.Check1.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="reserved"><em>await</em></span> <span class="type">Dialog</span>.ShowDialog<em>Async</em>(<span class="literal">"確認 1"</span>, <span class="literal">"1つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check2.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="reserved"><em>await</em></span> <span class="type">Dialog</span>.ShowDialog<em>Async</em>(<span class="literal">"確認 2"</span>, <span class="literal">"2つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">var</span> result = <span class="reserved"><em>await</em></span> <span class="type">Dialog</span>.ShowDialog<em>Async</em>(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>);
        <span class="reserved">if</span> (!result) <span class="reserved">return false</span>;
    }

    <span class="reserved">return true</span>;
}
</code></pre>


同期呼び出しの場合と比べて、背景色を変えて強調している部分だけが変化しています。
違いは、以下の通りで、残りの部分は全く同じです。

* メソッドに async 修飾子が付く

* 非同期処理を行いたい部分に await 演算子が付く

* 命名規約上、非同期処理を行うメソッドの名前は、語尾に Async を付ける


async修飾子やawait演算子の詳細は「[非同期メソッド](sp5_async.md#async)」 を参照してください。


##<a id="sec-generated-title-8"></a> <a id="iterator"></a>イテレーター非同期
そう、C# 5.0ならね。

ということで、問題は、C# 5.0が使えない場合。

C# 5.0以前、割と常套手段として知られていたのが、イテレーター（詳しくは「[イテレーター](../data/sp2_iterator.md)」を参照）を使った非同期処理手法です。

上記の例を、この手法を使って書き直すと、以下のようになります。

<pre class="source" title="イテレーターを使った非同期処理の例" lang="">
<code><span class="reserved">private void</span> BeginCheckWithIterator(<span class="type">Action</span>&lt;<span class="reserved">bool</span>&gt; onComplete)
{
    <span class="reserved">var</span> e = CheckIterator(onComplete).GetEnumerator();

    <span class="type">Action</span> a = <span class="reserved">null</span>;

    a = () =&gt;
    {
        <span class="reserved">if</span> (!e.MoveNext()) <span class="reserved">return</span>;
        e.Current(a);
    };

    a();
}

<span class="reserved">private</span> <span class="type">IEnumerable</span>&lt;<span class="type">Action</span>&lt;<span class="type">Action</span>&gt;&gt; CheckIterator(<span class="type">Action</span>&lt;<span class="reserved">bool</span>&gt; onComplete)
{
    <span class="reserved">if</span> (<span class="reserved">this</span>.Check1.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">bool</span> result = <span class="reserved">false</span>;
        <span class="reserved">yield return</span> callback =&gt; <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 1"</span>, <span class="literal">"1つ目の確認作業"</span>, r =&gt; { result = r; callback(); });

        <span class="reserved">if</span> (!result)
        {
            onComplete(<span class="reserved">false</span>);
            <span class="reserved">yield break</span>;
        }
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check2.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">bool</span> result = <span class="reserved">false</span>;
        <span class="reserved">yield return</span> callback =&gt; <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 2"</span>, <span class="literal">"2つ目の確認作業"</span>, r =&gt; { result = r; callback(); });

        <span class="reserved">if</span> (!result)
        {
            onComplete(<span class="reserved">false</span>);
            <span class="reserved">yield break</span>;
        }
    }

    <span class="reserved">if</span> (<span class="reserved">this</span>.Check3.IsChecked ?? <span class="reserved">false</span>)
    {
        <span class="reserved">bool</span> result = <span class="reserved">false</span>;
        <span class="reserved">yield return</span> callback =&gt; <span class="type">Dialog</span>.BeginShowDialog(<span class="literal">"確認 3"</span>, <span class="literal">"3つ目の確認作業"</span>, r =&gt; { result = r; callback(); });

        <span class="reserved">if</span> (!result)
        {
            onComplete(<span class="reserved">false</span>);
            <span class="reserved">yield break</span>;
        }
    }

    onComplete(<span class="reserved">true</span>);
}
</code></pre>


行数は増えてしまっていますが、パターンとして、

* <code>await</code>⇔<code>yield return</code>

* <code>return 戻り値;</code>⇔<code>onComplete(戻り値); yield break;</code>


というような、機械的な置き換えが成り立ちます。一度覚えてしまえば「書けなくはない」でしょう。

非同期処理に必要なのは、要するに「中断と再開」で、実は、イテレーターがやっていることと同じです。なので、この例みたいに、イテレーターを使って非同期制御フローを書けるわけです。

実際、C# 5.0のawait演算子は、イテレーターがやっているのと同様のコード生成によって実現されています。


##<a id="sec-generated-title-9"></a> <a id="conclusion"></a>まとめ
C# 5.0で追加されたawait演算子が内部で行っていることは「中断と再開」で、イテレーターと同系統の技術です。

逆に言うと、イテレーターを使って、await演算子と同じようなことをする方法があります。実際、C# 5.0以前には、この方法で非同期処理を行っている人もいました。

ここでは、そのイテレーターを使った非同期処理の例を挙げました。C# 5.0が使えない環境での助けや、C# 5.0のawait演算子の挙動を知る助けになるかと思います。
