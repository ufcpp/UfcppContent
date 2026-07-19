---
title: "[雑記] GUI と非同期処理"
source_url: "https://ufcpp.net/study/csharp/async/misc_uithread/"
content_type: "Article"
published_at: "2010-11-20T00:00:00"
updated_at: "2015-08-26T09:14:49"
tags:
  - "Ver. 5.0"
umbraco_id: 1337
parent_id: 1326
sort_order: 10
aliases:
  - "/csharp/async/misc_uithread/"
  - "/csharp/misc_uithread"
  - "/csharp/misc_uithread.html"
  - "/study/csharp/misc_uithread"
  - "/study/csharp/misc_uithread.html"
---

# \[雑記\] GUI と非同期処理

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
（書きかけ）

「[非同期処理](sp5_async.md)」の話とからめて、
GUI アプリケーション開発と非同期処理の話。


##<a id="sec-generated-title-2"></a> <a id="dispatcher"></a>GUI とディスパッチャー
（書きかけ、清書時は別ページにするかも）
<pre>
0.5秒固まったら「使いにくい」、3秒固まったら「バグだ」、10秒固まったら「パソコンが壊れた」と言われる。

↑ 大げさかもしれないけど、かなり真実。

・メッセージ ループ
クリックとかキー ダウンのイベントは一度キューにたまってる（メッセージ ポンプ）
ループでキューを見ては GUI の処理してるものがある。
WinForms とか WPF では隠ぺいされてるけども、内部的には、

while (GetMessage(ref msg, null, 0, 0))
{
    TranslateMessage(ref msg);
    DispatchMessage(ref msg);
}

DispatchMessage の内部で、最終的に以下のようなメソッドが呼ばれてる。

private static IntPtr WndProc(
    IntPtr hwnd, int msg, IntPtr wParam,
    IntPtr lParam, ref bool handled)
{
    if (msg == WM_LBUTTONDOWN)
    {
        MessageBox.Show("左クリックされた");
        handled = true;
    }
    return IntPtr.Zero;
}

WndProc 内で重たい処理するとフリーズ。
（結局、イベントハンドラーで重たい処理をするとフリーズ）
もちろん避けなきゃいけない。

最初に言った通り、0.5秒超えたら（超える可能性あったら）同期処理すべきじゃない。


・UI スレッド
一方で、グラフィックがらみは、パフォーマンス上の理由からマルチスレッド不可。
UI 要素は作ったスレッドからしかアクセスしちゃダメ。

ということで、
スレッド立てる → 重たい処理をそっちのスレッドでやる → UI スレッドに制御戻す
ってやらないとダメ。

UI スレッドに制御戻すために使うのがディスパッチャー（dispatcher: 配送係）
別スレッドから UI スレッドに、「この処理して」っていうメッセージを配送するって意味。

</pre>
