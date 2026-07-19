---
title: "[雑記] 継続と先物"
source_url: "https://ufcpp.net/study/csharp/async/misc_continuation/"
content_type: "Article"
published_at: "2011-05-10T00:00:00"
updated_at: "2015-08-26T09:13:49"
tags:
  - "Ver. 5.0"
umbraco_id: 1336
parent_id: 1326
sort_order: 9
aliases:
  - "/csharp/async/misc_continuation/"
  - "/csharp/misc_continuation"
  - "/csharp/misc_continuation.html"
  - "/study/csharp/misc_continuation"
  - "/study/csharp/misc_continuation.html"
---

# \[雑記\] 継続と先物

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

<h5 class="version version4">Ver. 4.0</h5>



## <a id="sec-generated-title-2"></a> <a id="plan"></a>予定

<strong id="key_future" class="keyword">先物</strong>（future）

<strong id="key_continuation" class="keyword">継続</strong>（continuation）
<pre>
・先物

「待ち時間を無駄にしない」という意味での非同期処理では、
いったん非同期に処理したうえで、処理結果の値を使って次の処理を始めたいことが多々ある

Task の場合、ジェネリック引数付きの Task&lt;T&gt; を使って、
非同期処理の結果を受け取ることができる


Task&lt;T&gt; みたいな、
「非同期に処理して、処理が終わったら値を取りたい」
みたいなものを先物（future: 将来的に受け取る物）って読んだりする。

（実際、他の言語だと Future って名前のクラスになってたりする
.NET 4 の Task でも、プレビュー版のころは Future&lt;T&gt; って名前になってた）

var t = Task.Factory.StartNew(() =&gt;
{
    Thread.Sleep(3000);
    return 10;
});
Console.WriteLine(t.Result);

みたいなコード書くと、
t.Result の時点でタスク終了待ちに入って、3秒後にタスクの戻り値（この場合10）が取れる。

（スレッド単位のシーケンスがどうなってるか絵にしたい）
main    別スレッド
|
|-------→|
|        |
|        |
|        |
|←-------|この間、メイン スレッド止まる
|

・継続
ただ、↑ みたいに3秒待ってしまうと非同期の意味ない。

実際には、「その処理終わったら引き続きこういう処理して」というようなデリゲートを渡す。

Task.Factory.StartNew(() =&gt;
{
    Thread.Sleep(3000);
    return 10;
})
.ContinueWith(t =&gt;
{
Console.WriteLine(t.Result);
});

こういう、「引き続き処理して」ってのを継続（continuation）って呼ぶ。

（スレッド単位のシーケンスがどうなってるか絵にしたい）
main    別スレッド
|
|-------→|
         |ここから先、メイン スレッドは自由
         |
         |
         |→| 継続
           |

・継続と戻り値

論理的にはほぼ一緒。
ただ、書き方がだいぶ煩わしく・・・

↓表にする
------------------------------
通常の戻り値
受け渡し側（関数内）: return x;
受け取り側（呼ぶ側）: var result = Method();

継続渡し
受け渡し側: continuation(x);
受け取り側: Method(x =&gt; ...);
------------------------------
戻り値を受け取る代わりに、継続処理をデリゲートで渡す
return で戻り値を返す代わりに、継続処理を呼び出す


再帰が深くなりそうに見えるけど、末尾呼び出しなので最適化可能。

・Combinator

戻り値⇔継続渡し の変換ができれば、非同期処理を同期っぽく書ける。

if とか while とかの制御構文も、If 関数とか While 関数作って、継続渡し化可能。
（こういうのを Combinator って言うらしい？関数型言語の用語。）

実際、F# の let! 非同期処理はこの 戻り値⇔継続渡し 変換で実現してるみたい。
http://tomasp.net/blog/async-compilation-internals.aspx

（この方法、正常フローの間はまだいいけども、例外処理絡むとどんどん面倒に）


・余談
.NET Framework は末尾呼び出しの最適化用の補助命令持ってる（Tail 命令）
メソッド呼び出し命令の前に Tail を付けると、スタック解放してから次のメソッド呼び出ししてくれる。

F# とかではこの Tail 命令入るし、
Tail 命令が吐いてなくても、.NET 4の64ビット版では自動的に末尾呼び出しの最適化してくれる。
（Release ビルド時のみ）

    </pre>
