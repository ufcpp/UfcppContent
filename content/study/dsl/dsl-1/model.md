---
title: "ドメイン特化モデル"
source_url: "https://ufcpp.net/study/dsl/dsl-1/model/"
content_type: "Article"
published_at: "2007-09-15T00:00:00"
updated_at: "2015-05-06T14:15:38"
tags: []
umbraco_id: 1433
parent_id: 1426
sort_order: 6
aliases:
  - "/dsl/dsl-1/model/"
  - "/dsl/dsl/model"
  - "/dsl/model"
  - "/dsl/model.html"
  - "/study/dsl/dsl/model"
  - "/study/dsl/model"
  - "/study/dsl/model.html"
---

# ドメイン特化モデル

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

これまで、「[DSL 概要](dsl.md)」などのページでは、
DSL の「別言語を作ってから開発を始める」という部分、
すなわち言語指向プログラミングの視点からの話が多かったですが、
ここでは開発モデルの例を示したいと思います。


## <a id="sec-generated-title-2"></a> <a id="numerical"></a>数値計算

```text
Fortress … 数値計算がターゲット

- 数式をそのまま書ける
  - [] で下付き、^ で上付きとか、対応関係を覚えれば
    ほぼ数式をそのまま書く感覚
  - WYSIWYG 的レンダリング
- 並列計算
  - for ループが自動で並列化される
  - Parallel Do か Reduction かも自動で考えてくれてるっぽい
```

```text
  - 内包的集合定義みたいな書式
    [{X, Y} || X <- A, Y <- B, X + Y == 3]
（この書き方は Erlang 式）
    ↑
    数学の記法に沿って書くなら、
    {(x, y) s.t. x ∈ A, y ∈ B, x+y=3}
    手続き型プログラミング言語で書くなら、
    var list = new List<Point>();
    foreach (var x in A)
      foreach (var y in B)
        if (x + y == 3)
          list.Add(new Point(x, y));

  - 予約語が Unicode の数学記号でも書ける
    Σ[x ← 0:n] f(x)
    ↑
    IME 的なものが使えない・使いたくない場合、
    SUM[x <- 0:n] f(x)
    という書き方も可能。

    下付き文字が []、上付き文字が ^ とか、対応ルールだけ覚えていれば、
    数式と同じ感覚でプログラムが書ける。

    ↑こういうような機能、
      汎用言語の文法を拡張してまで取り入れるべきかと言われると微妙。
      あくまで数式処理用 DSL にしておいて、混在開発するのがいい気が。

  - レンダリング

  - 単位・次元を指定できる

  - 群・環

  - 並列処理
  シームレスな並行プログラミングってのはまだまだ難しい議題だけど、
  数値計算という用途に絞ればいい解決方法がありそう。

  Fortess だと、for ループが自動で並列化されたり
  reduction 演算子・reduction 変数も自動で判別してくれる

  演算子の可換性の有無によって生成するコード変えてくれるっぽいし。

  - mutable / immutable variable
    - 方程式（equation）の意味での等号と、
      代入（assignment）の意味の等号
    - 関数型言語では、変数は immutable
    - 命令型言語では、変数は mutable

    Fortress では
      =  使うと immutable 変数
      := 使うと mutable
```

## <a id="sec-generated-title-3"></a> <a id="plan"></a>予定

```text
- BPEL

ビジネスプロセス
  サービス指向アーキテクチャとかいう言葉がはやってる昨今、
  既存のサービスをつないで1つのソリューションに

  BPEL: どういう順番でどのサービスを呼び出すかとかを記述する DSL

  ぶっちゃけ、UML のアクティビティ図とかフローチャートみたいなもの。

  抽象度の高いモデルで、
  サービスが具体的に何で実装されてるのか
    単なるソフトウェアライブラリ
    ウェブサービス
      SOAP、CORBA など
  実装方法を意識せずに書けるようにするのが BPEL の目的？


- SystemC、TLM: Transaction Level Modeling

  - 抽象度
    - clock  : クロックを完全に意識
    - timed  : ある程度タイミングを意識
    - untimed: タイミングは意識しない

  - モデル
    - RTL （register transfar level）
      - clock を意識した設計モデル
    ↑
    ↓
    - ESL （electronic system level）
      - 完全にタイミングを意識しないハードウェア設計
      - ソフトウェアプログラミング言語を使ってハードウェア設計
  ↓
  - 両者の間のギャップが大きすぎ

  そこで、
  - モジュールとチャネルに分けて考える
    - モジュールは、
      ハードウェアモジュールであったり、
      ソフトウェアのプロセスであったり
    - チャネルは、
      ハードウェアモジュール間の結線
        直接
        レジスタをはさんで
        BUS 経由
        RAM 経由
      ソフトウェアからのハードウェア呼び出し

  - モジュールとチャネル毎に、
    clock, timed, untimed の抽象度を選択できるようなモデル

  → TLM: Transaction Level Modeling


- 異なる抽象度の混在開発
  - ソフトウェアだと、今まで高級言語・アセンブラ混在くらいしかなくて、
    今後はこれにシステムモデルレベルが加わるくらいだと思うけど、
    ハードウェアだと、
      システムレベル、動作レベル、RTL、ゲートレベル、アナログ…
      HW/SW 協調

× 信号処理関係は汎用言語中のライブラリの方がよさげ
   OOP パラダイムとかなりマッチしてて、
   それよりも抽象度の高いモデルの必要性はあまり感じない
```
