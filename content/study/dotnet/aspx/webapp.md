---
title: "Web アプリケーション"
source_url: "https://ufcpp.net/study/dotnet/aspx/webapp/"
content_type: "Article"
published_at: "2006-06-30T00:00:00"
updated_at: "2015-05-06T14:15:05"
tags: []
umbraco_id: 1416
parent_id: 1414
sort_order: 1
aliases:
  - "/aspx/webapp"
  - "/aspx/webapp.html"
  - "/dotnet/aspx/webapp/"
  - "/study/aspx/webapp"
  - "/study/aspx/webapp.html"
---

# Web アプリケーション

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

（書きかけ）
<pre>
ラウンドトリップ
- CGI プログラムなんかだと、
  - 次の処理に必要な情報は一度全部 HTML に書き出しておく
  - ユーザがボタンを押した際とかには、
    post されたデータの解析から処理をする必要がある
  というように、ラウンドトリップを意識したプログラミングが必要
↓
- ASP.NET なんかだと、
  そのあたりの面倒な処理はフレームワークが勝手にやってくれる。
  プログラマは意識する必要なし。

  - ASP.NET を使えば、Windows プログラムとほぼ同じ感覚で
    ウェブアプリ構築可能。
  - Page_Load とかのイベントハンドラを書くだけ。


Web フォーム
どういうイベントがどういう順で起こるか
http://msdn2.microsoft.com/ja-jp/library/ms178472(VS.80).aspx


Web アプリケーションの状態
- HTTP は状態を持たないプロトコル
  - ページからページに遷移したときに、プログラムの状態は持ち越されない。
  - ユーザの状態を保持したければ、ファイルに書き出したり Cookie を使ったり。
  ↓
  - ASP.NET では、Web アプリケーションの状態がサーバ上に残る

  - Web アプリケーションの設定
    - IIS の設定で「仮想ディレクトリ」を作る際に、
      「ASP などを実行する」をチェックして仮想ディレクトリを作ると、
      その仮想ディレクトリ全体が1つの Web アプリケーションになる。
    - 同一 Web アプリケーション内のページは同じ状態を共有する。
      - 一定時間以内の同一ユーザ・同一ブラウザからのアクセスに対して、
        1つのプロセスがずっと生き続ける。
      - 通常はメモリ上に状態が残る。
        設定によっては SQL サーバや Cookie を介した状態の共有も可能。
    ↓
    - 別の Web アプリ(仮想ディレクトリ)間でのデータ共有は無理
      - それをしたい場合は、ファイルや Cookie、DB サーバを介して

</pre>

## <a id="sec-generated-title-2"></a> <a id="d43e13"></a>

## <a id="sec-generated-title-3"></a> <a id="d43e18"></a>

## <a id="sec-generated-title-4"></a> <a id="d43e23"></a>
