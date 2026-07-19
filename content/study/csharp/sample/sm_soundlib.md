---
title: "[サンプル] SoundLibrary"
source_url: "https://ufcpp.net/study/csharp/sample/sm_soundlib/"
content_type: "Article"
published_at: "2015-05-06T14:13:11"
updated_at: "2015-07-12T14:24:06"
tags: []
umbraco_id: 1361
parent_id: 1359
sort_order: 1
aliases:
  - "/csharp/sample/sm_soundlib/"
  - "/csharp/sm_soundlib"
  - "/csharp/sm_soundlib.html"
  - "/study/csharp/sm_soundlib"
  - "/study/csharp/sm_soundlib.html"
---

# \[サンプル\] SoundLibrary

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
Wave の読み書き、音声フィルタ、周波数解析などの機能を持つライブラリです。

* 学生の頃、研究用に使っていたもの。
    * 結構な規模
    * [音声信号処理ライブラリ ソースファイル(github)](https://github.com/ufcpp/UfcppSample/tree/master/Chapters/Old/SoundLibrary)



* 多少問題あり。
    * C# 1.0 の頃に書いたのでジェネリックスすら未使用。書き変えたいところも多々。
        * ジェネリックスでフィルタとかを固定小数点演算にも対応させたりしたいんだけど。

        * 波形データ生成のあたりもイテレータ使って生成したい。

        * その他、「[信号処理](../../sp/index.md)」の記事を書きつつ、 1から作り直したいくらいの気分だけど・・・。 そもそも記事書き自体止まっているので。



    * 権利の関係上、ライブラリを使ってる側（実際の研究内容に関係する部分）が公開できない。
        * なので、サンプル不足が否めない。

        * これを作った当時はテストプログラムもあまり作ってなかったし。
  * ソースコードが Shift JIS とかで、UTF8で保存しなおさないと今のコンパイラーだと「認識できないエスケープ シーケンス」エラーになる
