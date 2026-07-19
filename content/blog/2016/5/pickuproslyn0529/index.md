---
title: "ピックアップRoslyn 5/29"
source_url: "https://ufcpp.net/blog/2016/5/pickuproslyn0529/"
content_type: "BlogEntry"
published_at: "2016-05-29T05:01:33"
updated_at: "2016-05-29T05:08:49"
tags: []
umbraco_id: 1904
parent_id: 1890
sort_order: 4
aliases: []
---

# ピックアップRoslyn 5/29

## 拡張メンバー

Design Meetingで拡張メンバーの検討をしてたみたい。
(拡張メンバー = 初期検討段階で「extension everything」(なんでも拡張)って言ってたやつ。拡張メソッド以外に、プロパティとかも拡張できるようにする構文)

- [C# Language Design Notes for May 10](https://github.com/dotnet/roslyn/issues/11516)

現状、以下のような感じ。

- `extension class`ってキーワードで、通常のクラスを継承したような構文で作る
  - 定義したメンバーは拡張メソッド同様、静的メソッド化される
  - プロパティは、「インデックス付きプロパティ」(ILレベルとかVBにはある。C#のレベルで使ってないだけ)に展開するのがよさそうだけど、現状のC#では認めてないものなのでちょっと悩ましい
- インスタンス メンバー風の拡張だけじゃなくて、静的メソッドの拡張も足せる
- コンストラクターも足せる
- 演算子をどうするかは悩ましい
  - 四則演算とかはいいと思うんだけど
  - `==` とかが怪しい
- `Person`クラスの拡張`Enrollee`があったとして、`Person`の通常のインスタンス メンバーと、拡張の方のメンバーを呼び分ける構文がほしい
  - 優先されるのは通常のインスタンス メンバー
  - たぶん`((Emrollee)person).Supervisor`みたいなキャスト構文でやる

## コンパイラー組み込み

「時々C#にもインライン アセンブラーがほしい」問題に対して、
別解法の提案。

- [Proposal: Compiler intrinsics #11475](https://github.com/dotnet/roslyn/issues/11475)

### C#では書けないもの

まず、「ILを使えば書けるけど、C#では書けない」みたいなのの例。

- CLRがやってるみたいなネイティブ コードとの相互運用をビルド時に静的に作ってしまおうと思うと、いくつかC#では書けない命令が必要
- `infoof`(information ofの略。`typeof`と同じノリで`MethodInfo`とかを取る)相当の機能をライブラリでやろうと思うと`ldtoken`命令が必要
- [スライス](http://www.buildinsider.net/column/iwanaga-nobuyuki/007)を作るのにC#では書けない機能が必要
  - 実際、[現状、ILを書いてる](https://github.com/joeduffy/slice.net/blob/master/src/PtrUtils.il)

### インライン アセンブラーはやらない

- 時々しかない要件のためにILインライン アセンブラーを実装するのはコストに見合わない
- というか、混ぜるのよくない。C#コンパイラーとILアセンブラーは別ツールとして、別チームが個別開発すべき

### 別解法の提案

で、提案してるのが「コンパイラー組み込み」(compiler intrinsics: コンパイラーが内在して持ってる実装)。

いくつかのC++コンパイラーは、プログラマーがSSE命令とかAVX命令みたいな特殊な命令を使えるように、[コンパイラー組み込みの特殊なメソッドを提供](https://msdn.microsoft.com/ja-jp/library/26td21ds.aspx)してたりします。
C#でもそういう特殊なメソッドを用意して、上記のような通常のC#では書けない場面に対処すればいいんじゃないかという提案を出してきています。

文法的には`extern`メソッドを使うだけ。`CompilerIntrinsic`属性を付けることで、ネイティブDLL中のネイティブ メソッドを呼ぶ代わりに、コンパイラーが直接その場所にIL命令を出力するというもの。
これなら今から新文法について考える必要もないし、簡単に目的が達成できそう。

## bool型やenum型のswitchの完備性

`bool`型には`true`と`false`しかないはずなんだから、以下の`default`句は不要にできないかという話。ご意見求む状態。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">bool</span> b = ...;
<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">switch</span> (b)
{
    <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">case</span> <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">true</span>:
    <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">case</span> <span class="pl-c1" style="box-sizing: border-box; color: rgb(0, 134, 179);">false</span>:
        <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">break</span>;
    <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">default</span>:
        <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">break</span>; <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// warning: unreachable code?</span>
}
</code></pre>

これ、昔から要望としては頻出なんですが、できない理由もあります。ILは結構低級機能を提供しているので、`bool`型の変数を無理やり書き換えて、`true`でも`false`でもない値を作れて、上記`default`句に来ちゃう場合があったり。
というか、C#でも、以下のような書き方でそういう値を作れます。

- [unsafeで値を書き換え](https://gist.github.com/ufcpp/fe6788898ca4b91f0067dd7017af6ca4)
- [StructLayoutで値を書き換え](https://gist.github.com/matarillo/0b6815729dd20fb17c0ee62210dfeb3c)

それに、後方互換性を考えると、既存の`switch`には警告も出せないです。

という背景の中、今後導入予定の`match`式(`switch`の式バージョン)ではこういう普通はあり得ない`default`句に対する警告がほしいですか？という質問。

## 言語サポート付きのobsolete

今、obsolete (廃止したい、もう使ってほしくない)なメンバーには`Obsolete`属性を付けて対処しています。
でも、この属性ベースのやり方じゃなくて、C#の言語機能としてobsoleteキーワードが必要かもという話が。

- [Proposal: language support for Obsolete #11583](https://github.com/dotnet/roslyn/issues/11583)

シナリオ的には以下のようなもの。

<pre class="source" title="">
<code><comment></span><span class="comment">// シナリオ 1:</span>
<span class="reserved">int</span> f(<span class="reserved">string</span> s);                 <span class="comment">// ライブラリv1がこいつを持ってる</span>
<span class="reserved">int</span> f(<span class="reserved">string</span> s, <span class="reserved">bool</span> b = <span class="reserved">false</span>); <span class="comment">// v2でこれに変えたい</span>
<span class="comment">// バイナリ互換性のためにf(string)は消せない</span>
<span class="comment">// でも、今後、オーバーロード解決時に、f("") で優先的に f(string, bool) の方を見てほしい(今は無理)</span>

<span class="comment">// シナリオ 2:</span>
<span class="reserved">float</span> g();  <span class="comment">// ライブラリv1がこいつを持ってる</span>
<span class="reserved">double</span> g(); <span class="comment">// v2でこれに変えたい</span>
<span class="comment">// バイナリ互換性のためにfloat g()は消せない</span>
<span class="comment">// でも、今後、double g()を使いたい(呼び分けどころか今はそもそも定義すら不可能)</span>
</code></pre>

これに対して、v1側に`obsolete`修飾子を付けることで、v2側の新メソッドの追加・オーバーロード解決できるようにしたいとのこと。

## TryRoslyn

[Roslynリポジトリ](https://github.com/dotnet/roslyn)の各ブランチの最新版でのコンパイルを、オンラインで試して見れるサイト作ってる人がいた。

- [http://tryroslyn.azurewebsites.net/](http://tryroslyn.azurewebsites.net/)

C#チームの公式提供じゃないし、まだまだ作業途中みたいですけども。

- Lucian (C#チームの中の人)が、自分のリポジトリで[async任意戻り値](https://github.com/dotnet/roslyn/issues/10902)の試験実装してる
- ashmind (TryRoslyn作ってる人)が、そのLucianの個人リポジトリのブランチをTryRoslynに追加してくれた
- Lucian、[早速利用](http://tryroslyn.azurewebsites.net/#b:ljw1004-features-async-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=)

みたいな感じになってた。
