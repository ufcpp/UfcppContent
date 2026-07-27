---
title: "はじめての C# 実演編"
source_url: "https://ufcpp.net/study/csharp/devenv/getstarted_csharp/"
content_type: "Article"
published_at: "2009-05-14T00:00:00"
updated_at: "2019-05-01T17:08:18"
tags: []
umbraco_id: 1189
parent_id: 1709
sort_order: 2
aliases:
  - "/study/csharp/abstract/getstarted_csharp"
  - "/study/csharp/getstarted_csharp.html"
---

# はじめての C# 実演編

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

「[はじめてのプログラミング](../intro/getstarted.md)」でも書いているように、
C# を最速でマスターしたければ Visual Studio に頼りましょう！

ということで、「[基礎](../index.md#start)」辺りで説明する予定の文法を、
Visual Studio の補助を受けつつ実際に書いてみます（動画あり）。


## <a id="sec-generated-title-2"></a> <a id="beginWith"></a>手始めに

Visual Studio 自体については「[C# 開発環境](ab_devenv.md)」を参照してください。
このページは Visual Studio for Windows を元に撮った動画で説明していますが、
Visual Studio Code や Visual Studio for Mac でも似たような操作が可能です。

Visual Studio を使う準備ができたら、まずお約束の Hello World（というメッセージを表示するだけの簡単なプログラム）を作成。
<iframe width="480" height="390" src="https://www.youtube.com/embed/qxH6poyBxsQ" frameborder="0" allowfullscreen=""></iframe>
* プロジェクトの新規作成

* [Visual C#] → [コンソール アプリケーション] を選んで、適当な名前を付けて [OK]。
    * ひな形ができる。



* cw と入力した後、[Tab] を2回押す。
    * Console.WriteLine ってのが補完で出てくる。



* WriteLine の中に、"" でくくって適当な文字列を入れる。

* ビルド
    * メニューから [ビルド] → [ソリューションのビルド]。

    * もしくは、[F6] キーを押す。

    * もしくは [Ctrl] + [Shift] + b。



* 実行
    * メニューから [デバッグ] → [デバッグなしで開始]。

    * もしくは [Ctrl] + [F5]。



* 入力した文字列が表示されるはず。



## <a id="sec-generated-title-3"></a> <a id="variable"></a>変数

<iframe width="480" height="390" src="https://www.youtube.com/embed/y9jSQ0vC19o" frameborder="0" allowfullscreen=""></iframe>
* <code>int i = 10;</code>
    * 整数型の変数 i に 10 という値を代入。



* <code>double x = 1.0;</code>
    * 小数（正確には「倍精度浮動小数点数」）型の変数 x に 1.0 を代入。



<iframe width="480" height="390" src="https://www.youtube.com/embed/GVpKgSq0Azs" frameborder="0" allowfullscreen=""></iframe>
* var で型推論ができる。
    * <code>var i = 10;</code>だと、10 が整数なので i は int になる。

    * <code>var x = 1.0;</code>だと、1.0 が小数なので x は double になる。



* var にマウスカーソルを合わせると、推論結果の型が出る。
    * その後、変数にマウスカーソルを載せると、変数の型が出る。





## <a id="sec-generated-title-4"></a> <a id="arithmetic"></a>四則演算

<iframe width="480" height="390" src="https://www.youtube.com/embed/rTIs9fDFC0Y" frameborder="0" allowfullscreen=""></iframe>
* <code>x + y</code>とかで加減乗除ができる。
    * <code>+</code>で足し算。

    * <code>-</code>で引き算。

    * <code>*</code>で掛け算。

    * <code>/</code>で割り算。



* [Ctrl] + c で、今カーソルのある行を丸々コピー。
    * その状態で [Ctrl] + v で行の貼り付け。

    * その後、<code>+</code>のところだけ書き換え。





## <a id="sec-generated-title-5"></a> <a id="cast"></a>型変換

<iframe width="480" height="390" src="https://www.youtube.com/embed/nSgOcNoiHvQ" frameborder="0" allowfullscreen=""></iframe>
* 精度の高い型から低い型への変換は明示的に書く必要がある。
    * 逆は暗黙的に変換がかかる。

    * double を int に変換すると、小数点以下が失われる。

    * <code>int i = (int)x;</code>





## <a id="sec-generated-title-6"></a> <a id="string"></a>文字列

<iframe width="480" height="390" src="https://www.youtube.com/embed/ijUkS6GQt0g" frameborder="0" allowfullscreen=""></iframe>
* <code>string s = "サンプルテキスト";</code>
    * 文字列型の変数 s を定義。



* <code>+</code>演算子で2つの文字列を連結できる。

* string.Format で書式指定付きの文字列化。
    * <code>string.Format(“({0}, {1})”, 10, 20);</code>
        * {0} のところに 10 が、{1} のところに 20 が入る







## <a id="sec-generated-title-7"></a> <a id="loop"></a>配列と反復

<iframe width="480" height="390" src="https://www.youtube.com/embed/r3c4wOLPU64" frameborder="0" allowfullscreen=""></iframe>
* int[] で int 型の配列になる。
    * <code>int[] x = new[] { 1, 2, 3, 4, 5 };</code>で5要素の配列を作る。



* while まで入力したら [Tab] を2回押す
    * while (反復の継続条件)

    * ぶっちゃけ、あんまり while 使うことない



<iframe width="480" height="390" src="https://www.youtube.com/embed/hbCmjaNdjSM" frameborder="0" allowfullscreen=""></iframe>
* for まで入力したら [Tab] を2回押す。
    * for (初期値; 継続条件; 値の更新)
        * 「10回繰り返す」とかやりたい時に使う。

        * ぶっちゃけ、配列の中身の列挙には次の foreach の方を使う方がいい。





<iframe width="480" height="390" src="https://www.youtube.com/embed/utsIRi6jhSE" frameborder="0" allowfullscreen=""></iframe>
* foreach まで入力したら [Tab] を2回押す
    * 配列とかの、コレクションの列挙

    * foreach (var 変数 in 配列)





## <a id="sec-generated-title-8"></a> <a id="branch"></a>条件分岐

<iframe width="480" height="390" src="https://www.youtube.com/embed/75tx1_ipjhc" frameborder="0" allowfullscreen=""></iframe>
* if まで入力して [Tab] 2回もできるけど、それほどタイピング量減らない。
    * if (条件) 式;

    * 必要なら if (条件) 式1; else 式2;



* if (条件) 式 の後、 ; を打った時点でソースコードの自動整形がかかる。



## <a id="sec-generated-title-9"></a> <a id="function"></a>関数

<iframe width="480" height="390" src="https://www.youtube.com/embed/tDtwW3nJ9zw" frameborder="0" allowfullscreen=""></iframe>
* 意味のある単位で細かく関数化しましょうね。
    * 関数化したい範囲を選択して、右クリックして [リファクター] → [メソッド抽出]。





## <a id="sec-generated-title-10"></a> <a id="struct"></a>構造体

<iframe width="480" height="390" src="https://www.youtube.com/embed/vvFKdEpVYM0" frameborder="0" allowfullscreen=""></iframe>
* <code>struct Point { public int X; public int Y; }</code>
    * struct で構造体定義。

    * public とかの意味は「[オブジェクト指向](../index.md#oop)」以降で説明。



* <code>var p = new Point { X = 10 };</code>みたいな書き方でメンバーの初期化可能。

<iframe width="480" height="390" src="https://www.youtube.com/embed/DEMNNRywehQ" frameborder="0" allowfullscreen=""></iframe>
* Usage First 開発。
    * 「使い方」の方を先に書いて、そこからクラスや構造体を生成。

    * （Visual Studio 2010 からの新機能。）
