---
title: "タプル"
source_url: "https://ufcpp.net/blog/2016/8/tuples/"
content_type: "BlogEntry"
published_at: "2016-08-31T13:44:40"
updated_at: "2016-08-31T13:44:40"
tags:
  - "C# 7思い出話"
umbraco_id: 1947
parent_id: 1932
sort_order: 6
aliases: []
---

# タプル

思い出したかのように「C# 7思い出話」。

タプルがらみも先週末で一通り書き終わったはずかな。

- [名前のない複合型](../../../../study/csharp/structured/st_anonymoustype.md)
- [タプル](../../../../study/csharp/datatype/tuples.md)
- [複合型の分解](../../../../study/csharp/datatype/deconstruction.md)

## タプル

これ自体はおおむね当初予定通りですかね。
そんなに裏話的な面白話もなく。
文法的に変化が大きいんで実装は大変そうでしたが。

しいていうと、他の提案を差し置いてタプルの優先度が高かった的な話ですかね。
タプルは、[レコード型](https://github.com/dotnet/roslyn/blob/master/docs/features/records.md)とか、
C# 7のさらに先で入る予定の機能の基礎に使う可能性があって。
依存順的に先にないと行けないという、割かしシンプルな話。

タプルは、`(int x, int y)`みたいなメンバー名を持った型から、`ValueTuple`という完全に名前のないデータ構造を生成する機能なわけで、
型に緩い世界と厳しい世界の橋渡しとして期待されます。ロジックを書く上では厳しい型付けを持っている方が楽ですが、View (WPFやUWPのData Bindingみたいな世界)や、Wire Data (通信層でのデータのシリアライズ・デシリアライズ)とかでは緩い型の方が助かったりします。その間をつなぐ基礎に使えそうって意味で、タプルには期待感があります。

そして、レコード型をタプルをベースに作る、あるいは、レコード型からタプルへの変換を用意すれば、型名がある世界とない世界もつなげます。

レコード型をタプルをベースに作るっていうのは、例えば以下のようなコードを書いたとして(これがレコード型って呼ばれる機能)、

<pre class="source" title="レコード型の書き方の例">
<code><span class="reserved">class</span> <span class="type">Point</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
</code></pre>

以下のようなコードに展開するという案。

<pre class="source" title="上記コードの展開結果の案1: レコード型をタプルを使って作る">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    (<span class="reserved">int</span> x, <span class="reserved">int</span> y) Value;
    <span class="reserved">public</span> <span class="reserved">int</span> X =&gt; Value.x;
    <span class="reserved">public</span> <span class="reserved">int</span> Y =&gt; Value.y;
}
</code></pre>

レコード型からタプルへの変換を用意というのだと、以下のような感じ。

<pre class="source" title="上記コードの展開結果の案1: タプルへの変換">
<code><span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> (<span class="reserved">int</span> x, <span class="reserved">int</span> y) Value =&gt; (X, Y);

    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
}
</code></pre>

どちらにしろ、依存関係があるなら、依存されている側の方が先に落ち着いていないといけない。

レコード型は、その前身となる [primary constructor](http://blog.xin9le.net/entry/2014/06/16/144115)っていう機能提案から考えるとC# 6よりも前(一瞬、C# 6に入れる想定で話が進んでた)、2014年半ば頃からあります。それが、「もっと洗練してレコード型にしてから取り入れたい。そのためにはC# 6では無理」となり、「タプルやパターン マッチングと併せて考えたい。そのためにはC# 7よりも後」となり今に至ると。

## 分解

こちらは逆に、前倒し。
前倒しというか、パターン マッチングがらみの仕様のうち、一部分だけが先行してC# 7に入るわけですが、その一部。

パターン マッチングは結構大きな提案です。All or Nothing、「全部入れるか全部入れない」かとかで入れようとすると、たぶん3年くらい先になります。
こないだ[短期リリース サイクル化](http://www.buildinsider.net/column/iwanaga-nobuyuki/008)の話とか書きましたが、後への影響残さないなら、先に少しだけリリースする方が助かります。

元々は、以下のような構文で話が進んでいたんで、それからするとずいぶん、分解専用の構文になりました。多少、そこは、今後、パターン マッチングの全体が入るときに衝突しないのかな？とか思ったりはします(たぶん大丈夫という判断が付いたからC# 7で入ることになったはずですが)。

<pre class="source" title="パターン マッチングの一部としての分解構文">
<code><span class="reserved">let</span> <span class="input">パターン</span> = <span class="input">分解したいもの</span>;
</code></pre>
