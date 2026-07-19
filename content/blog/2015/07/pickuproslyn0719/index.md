---
title: "ピックアップRoslyn 7/19"
source_url: "https://ufcpp.net/blog/2015/07/pickuproslyn0719/"
content_type: "BlogEntry"
published_at: "2015-07-19T06:49:24"
updated_at: "2015-07-19T06:49:45"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1774
parent_id: 1756
sort_order: 2
aliases: []
---

# ピックアップRoslyn 7/19

Visual Studio 2015 関連作業が終わって、その次っぽい動きがちらほら。

## Make Features Layer Portable #3998

- [Make Features Layer Portable #3998](https://github.com/dotnet/roslyn/issues/3998)

Feature API = リファクタリングとか、コード解析からのコード修正とか、そのレイヤーのAPIのこと。

これを、[OmniSharp](http://www.omnisharp.net/)、[Visual Studio Code](https://code.visualstudio.com/)で使うためにポータブル化作業をしようという Issue ページが立ちました。

ほぼ、

- 今、internal で作ってあるものを public にするだけ(とはいえ、それが大変)
- 何年も保守していくものになるものをうかつに全部はpublicにできない
- public にするには耐ええない作りの箇所があるからその修正が必要

という感じの様子。

## Interactive Design Meeting Notes - 7/8/15 #3927

- [Interactive Design Meeting Notes - 7/8/15 #3927](https://github.com/dotnet/roslyn/issues/3927)

スクリプト向けC#方言の仕様レビューしてたみたい。その議事録なので、割かしC#スクリプトがどうなりそうかのまとめになってる。

- `#r`でDLLの参照、`#load`で他のスクリプトの取り込み。スクリプトの一番上にだけ書ける
- `#load`な、「その場所にそのスクリプトをコピー＆ペーストしてきたかのような挙動」。shとかでいうところの `.` の方(`&`の方じゃなく)
- トップ レベルに変数とか関数を書けるわけだけど、
  - static スクリプト変数には、`await` を含む初期化子を書けない
  - 普通のスクリプト変数(あるいは、トップ レベル変数って呼ぶ)にはそういう制限ない
    - interactive window とかで実行してる際には、`await` した時点でUIをブロックする(バックグラウンド実行にはしない)
- 「C#スクリプト」は、普通のC#ソースコードからコピー&ペーストして来たらそれがそのまま動くようにする
  - Copy/Paste Prinsible of Scripting (CPPoS: スクリプトのコピー&ペースト原則)って呼んでる

その他、最近、C# 7.0向けに[ローカル関数](https://github.com/dotnet/roslyn/issues/2930)の仕様が出始めているものの、それとトップ レベル関数は別物で、C#スクリプトは両方サポートすることになると思う。

スクリプトは常に `async` な文脈で実行される。どこにでも `await` 書ける。1つも`await`を書いてないときにはステートマシン(非同期メソッド内部実装の、仰々しいコード生成)は作らない最適化は掛ける。

static スクリプト変数は、スクリプトが何回実行されても1回だけ初期化が走るようにというのもあるけど、その主たる目的は CPPoS のため(どこからコピペしてもスクリプト実行できるようにするためには、staticフィールドもコピペで動く必要がある)。

トップ レベルのメソッド、プロパティ、型、スクリプト変数のスコープは既定で public (通常のC#は、アクセシビリティ修飾子を省略すると private/internal だけど、スクリプトの場合は public)。スクリプトの外(`#load`で呼び出す側)からは隠したい場合、「ファイルprivate」って概念を実装したい。現状でも、変数やメソッドを1段階 `{}` で覆ってしまえば外からアクセスできなくなる実装にはなってる。

`#load` するとその中のトップレベル要素は全部取り込まれる(トップ レベルの識別子には、特に何の修飾もなくアクセスできる)。`#load into`を使うと、そのスクリプトに名前を付けれるけども、これはエイリアス的な動作。2回、別名で`#load into`すると、その指す先は一緒。別インスタンスが作られたりはしない。

## Ability to classify fields, locals and parameters differently #3976

- [Ability to classify fields, locals and parameters differently #3976](https://github.com/dotnet/roslyn/issues/3976)

おまけ。

今、Visual Studioのコードの色分けでは、識別子にはあまり色が付かない(たぶん、色が付くのは型名だけ？)わけですが、フィールドとか変数の色分けには興味ある？という調査的な issue が立ったみたい。

これまで、大きく取り扱ってこなかったけども、おそらく実装しても実行性能的に目立った影響はないだろうし。とのこと。

例えば、以下のようなの。

- `readonly` かどうか(get-only自動プロパティも)での色分け欲しい？
- `event` も？
- (クラス/構造体直下の)定数と、ローカル定数の区別とか要る？
- 未初期化変数の警告にも色分けとか使うのどう？
- C#スクリプトができたら、トップ レベル変数とかトップ レベル メンバーもできるけども、それも別扱いしたい？
- 組み込み演算子と、オーバーロード(ユーザー定義演算子)の区別とか便利そうじゃない？
