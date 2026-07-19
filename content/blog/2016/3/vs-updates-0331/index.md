---
title: "Visual Studio 2015 Update 2がRTM ＆ Visual Studio \"15\" Preview"
source_url: "https://ufcpp.net/blog/2016/3/vs-updates-0331/"
content_type: "BlogEntry"
published_at: "2016-03-31T04:32:52"
updated_at: "2016-03-31T04:32:52"
tags: []
umbraco_id: 1884
parent_id: 1877
sort_order: 3
aliases: []
---

# Visual Studio 2015 Update 2がRTM ＆ Visual Studio "15" Preview

昨日は夜更かししていたわけですが。

- [Build 2016 生中継](http://ascii.jp/elem/000/001/139/1139982/)

なんか今回は「本来はWindowsのイベント」らしさがあり、ほんとにWindowsの話題ばっかり。
開発ツール系の話が初日キーノートに出てこないという。
内心「やべぇ、しゃべることねぇ」とかおびえながらのひな壇芸人やってました。
危うく、ただ[池澤あやか](https://twitter.com/ikeay)さんに会いに行くだけの[ミーハー](https://twitter.com/ufcpp/status/715261117929365504)になるところでした。

開発ツール系は2日目の今晩のキーノートで話すんですかね。確かに最近そういう構成になってること多い。前の方にコンシューマー受けするもの、後ろの方に開発系。

そして、キーノートでまったく触れられないまま、さらっとMSDNブログで公開されるVisual Studio。

- [Visual Studio 2015 Update 2 RTM](https://blogs.msdn.microsoft.com/visualstudio/2016/03/30/visual-studio-2015-update-2-rtm/)
  - [Visual Studio ダウンロード ページ](https://www.visualstudio.com/downloads/download-visual-studio-vs)
  - [Visual Studio 2015 Update 2 リリースノート](https://www.visualstudio.com/news/vs2015-update2-vs)
- [Visual Studio "15" Preview](https://www.visualstudio.com/news/vs15-preview-vs)
  - [Visual Studio "15" Preview ダウンロード ページ](https://www.visualstudio.com/downloads/visual-studio-next-downloads-vs)
  - [Visual Studio "15" Preview リリースノート](https://www.visualstudio.com/news/vs15-preview-vs)
- [Announcing the .NET Framework 4.6.2 Preview](https://blogs.msdn.microsoft.com/dotnet/2016/03/30/announcing-the-net-framework-4-6-2-preview/)

## Visual Studio 2015 Update 2

そういえば、プレビュー版、RC版が出たときにあんまり取り上げてなかったなぁと。

C# に関連する新機能は4点ほど

- ソースコードを選択 → 右クリックメニューの「Execute in Interactive」から、C# Interactiveにコードを送れる
- Add Using (using ディレクティブの追加)に対するあいまいマッチング
- アナライザーAPIの性能改善
- 標準提供のリファクタリング機能にいくつかの新アクション

上の2つは簡単に触れておこうかと。

### Execute in Interactive

ソースコードの1部分の実行結果を今その場で知りたいときに便利。

ではあるんですが、これ、プロジェクトのコンテキストを取り込んでくれない(そのプロジェクト内で自分で定義した型は使えないし、参照してるライブラリもCV# Interactiveからは参照されない)ので微妙に不便。標準ライブラリ内のクラスしか使えないんですよね。「Json.NETのシリアライズ結果の挙動を知りたい！」とかもできない。計画上はNuGetパッケージの参照とかはC# Interactiveウィンドウ内からできるようにしたいって話があった気がするので、まあ、今後に期待ですかね。

### Add Using あいまいマッチング

「usingディレクティブってソースコードの上部にしか書けないからうざい。今まで使ってなかった名前空間の型を新たに使いたくなった場合、いちいちに上に移動するのがめんどくさい。」という不満にこたえる「Add Using」機能ですが、これまでだとクラス名を正確に打たないと「Add Using」できなくて、微妙に不便でした。

というのも、IntelliSenseであれば、

- 全部小文字で打っても補完でCamelCaseなメンバーを見つけられる
- 途中まで打てば補完で長い名前を打てる

という機能があって、もうさぼり癖がついてるわけです、C#開発者は。
それが、「Add Using」したければクラス名を正確に全部書けとか、無理。

挙動を見てる感じ、

- 全部小文字で打っても認識
- 文字が1・2文字まで間違ってる/足りてない状態でも認識
  - 単語の長さによるっぽい

という感じ。
streem(Stream)、dianostic（Diagnostics）とかそんなのがちゃんと補完されます。
スペルを正確に覚えるのが面倒なときに便利。
queueみたいな2文字違いどころじゃないレベルの単語が相手だと少々厳しいですが…

![全部小文字で打って、Add Using][1]

## Visual Studio "15" Preview

"15"はもちろん内部バージョンのことです。Visual Studio 2015が"14"。つまり、次期バージョンのプレビュー。

これまでだと、マイクロソフトの製品って2・3年サイクルでリリースしてました。直近のC#でも、2010でC# 4→2012でC# 5→2015でC# 6です。
最近徐々にリリースサイクルを縮めたがっているわけですが、なので、今年中のリリースが期待されます。

あと、インストーラーが2つあります。今まで通り、isoファイルで丸ごとの提供があるインストーラーと、「lightweight」なインストーラー。

C# 的には、まあ、[GitHubのRoslynリポジトリ](https://github.com/dotnet/roslyn)を眺めてると、[futureブランチ](https://github.com/dotnet/roslyn/tree/future)に結構C# 7の新機能がマージされだしておりまして、「おっ、ここまでは近々バイナリでのリリースがあるのかな」などと眺めておりました。

実際大体その通り。1・2週前のプルリクまではVS "15"に入った感じですね。

以下の機能が動きます。ただし、今、C#７の文法を試すにはちょっとしたトリック(条件コンパイルシンボルに `__DEMO__` を追加する)が必要になります。

- ローカル関数(nested local function)
- パターン マッチング(pattern matching)の一部
- ref戻り値/refローカル変数(ref returns/ref locals)
- 2進数リテラル(binary digit)、数値セパレーター(digit separator)

(ちなみに、リリースノートに書かれてないやつも、GitHub上で実装っぽいものがあったものは一通り試しました。その結果、2進数リテラルが動いてることを確認。)

この3/31リリースのプレビュー版で動くC# 7コード、GitHubにサンプルを上げておきました。

- [https://github.com/ufcpp/UfcppSample/tree/master/Demo/Csharp7/201603](https://github.com/ufcpp/UfcppSample/tree/master/Demo/Csharp7/201603)

### __DEMO__ 条件シンボル

今のところ、futureブランチにしかないような新文法は、コンパイラー オプションを追加しないと使えない状態になっています。
で、Visual Studio上からどうやってそのオプションを追加するの？という話なんですが…
追加できないです。対応追いついてないようで。

じゃあ、どうやって上記サンプルは動かしてるかというと、抜け道がありまして。
「プロジェクト設定で`__DEMO__`という名前の条件コンパイル シンボルを定義しておく」という、一時しのぎ感溢れる見事な対応。

![__DEMO__ 条件シンボル][2]

名前通り、デモで見せるときに使ったんでしょうね。

### ローカル関数

↓こんなやつ。

<pre class="source" title="ローカル関数">
<code><reserved></span><span class="reserved">private</span> <span class="reserved">static</span> <span class="reserved">void</span> LocalFunctions()
{
    <span class="reserved">int</span> F(<span class="reserved">int</span> n) =&gt; n &gt;= 1 ? n * F(n - 1) : 1;
    <span class="type">Console</span>.WriteLine(F(5));
}
</code></pre>

まあ、`public void X()`に対して`private void XInternal()`みたいなのを別途書くこと、たまにあったじゃないですか。
それが必要なくなります。

これまでも、ラムダ式を使えば似たようなことはできましたが、以下の点でローカル関数の方が有利になります。

- 再帰呼び出しが素直に書ける
- デリゲート変数への代入が省ける
- ローカル変数のキャプチャがちょっと賢い
  - ラムダ式: クラスのフィールドに昇格する。ヒープのアロケーションが発生
  - ローカル関数: 構造体のフィールドにして、ref引数で関数に渡される。ヒープ除け

### パターン マッチング(一部)

パターン マッチング自体は、最近[Build Insiderに寄稿](http://www.buildinsider.net/column/iwanaga-nobuyuki/004)したんでそちらを参照していただくとして。

このうち、位置指定パターンだけは未実装。
次回の記事で書く予定ですが、位置指定パターンだけはちょっと実装が面倒とういうか、
レコード型やタプル型など、他の新機能も合わせて詳細を詰めないといけないので、少し実装が後回しになっています。

まあ、型パターンだけでも使えれば結構便利なはず。

### ref戻り値

↓こんな感じ。

<pre class="source" title="">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Buffer</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> BaseIndex { <span class="reserved">get</span>; }

    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="type">T</span>[] _array;

    <span class="reserved">public</span> Buffer(<span class="reserved">int</span> baseIndex, <span class="reserved">int</span> count)
    {
        BaseIndex = baseIndex;
        _array = <span class="reserved">new</span> <span class="type">T</span>[count];
    }

    <span class="reserved">public</span> <span class="reserved">ref</span> <span class="type">T</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] =&gt; <span class="reserved">ref</span> _array[index - BaseIndex];
}
</code></pre>

まあ、ほとんどの開発者は直接は使わなさそうですかね。
パフォーマンス改善に期待できる機能です。
ライブラリ作者がこれを使うことでパフォーマンスが上がり、間接的には全ての開発者への恩恵が期待されます。
というか、C# コンパイラー自信のパフォーマンス改善が結構見込めそう。

これに限らず、パフォーマンス改善系の機能追加はちらほら追加されていきそうな雰囲気です。

### 2進数リテラル/数値セパレーター

↓これは大変わかりやすく。

<pre class="source" title="">
<code><reserved></span><span class="reserved">var</span> b = 0b1000_0001;
<span class="type">Console</span>.WriteLine(b);
</code></pre>

こいつはC# 6の頃から新機能候補に挙がってましたし、結構前から実装済みだったはず。[CoreFx](https://github.com/dotnet/corefx)の方のソースコード見てたらそこらじゅうですでに使われてたり。
前からあるから、今回リリースノートに書き忘れたのかなぁとか思ったり。
「そんなに強い関心があるわけじゃないけど実装が簡単だから実装済みです」、「`0b`でいいのかとか`_`でいいのかとか議論の余地もあるし、もう少し寝かせるか」みたいな立ち位置のはず。

### lightweightインストーラー

全部の機能はiso版のこれまで通りのインストーラーでないとまだ使えないらしいですが。
それとは別に、今後に期待がかかるlightweightインストーラーってのが提供されました。

これまでの「webインストーラー」と何が違うかというと、

- 最小構成だと300MB程度でインストールできる
  - 必要な機能は必要に応じて拡張マネージャーで入れる
- どうも、レジストリを汚さないみたい
  - 既存のVisual Studioを立ち上げっぱなしでもVS "15"をインストール可能
  - 1台に複数インストールとかもたぶんできる
      - プロジェクトごとに別の拡張入れた状態のVSを作るみたいなのもたぶん

という感じ。地味に、大きな変化だったり。

  [1]: ../../../../../assets/media/1069/smallcase.png
  [2]: ../../../../../assets/media/1070/__demo__.png
