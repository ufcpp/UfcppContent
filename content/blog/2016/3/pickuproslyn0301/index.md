---
title: "ピックアップRoslyn 3/1: position-to-propertyマッチ"
source_url: "https://ufcpp.net/blog/2016/3/pickuproslyn0301/"
content_type: "BlogEntry"
published_at: "2016-03-01T12:59:21"
updated_at: "2016-03-01T12:59:21"
tags: []
umbraco_id: 1878
parent_id: 1877
sort_order: 0
aliases: []
---

# ピックアップRoslyn 3/1: position-to-propertyマッチ

久々にMads降臨(C# チームのLanguage Design Meetingの議事録投稿)。

- [C# Language Design Notes Feb 29, 2016](https://github.com/dotnet/roslyn/issues/9330)

3つの言語機能を紹介しているんですが、共通して「position-to-propertyマッチ」というのが必要になります。
このposition-to-propertyマッチを許す「主義」を採用することに決めたという報告です。

## position-to-propertyマッチ

簡単に言うと、例えば以下のようなクラスがあったとき、

<pre class="source" title="">
<code><reserved></span><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Person</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> FirstName { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">string</span> LastName { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Person(<span class="reserved">string</span> firstName, <span class="reserved">string</span> lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
</code></pre>

コンストラクター第1引数の`firstName`とプロパティの`FirstName`には1対1の対応があります。同様に第2に引数`lastName`とプロパティ`LastName`もです。
こういう、1対1の関係を、(先頭の1文字の大小の差は無視して)名前をベースに調べて、

- `FirstName`というプロパティと、コンストラクターの第1引数を同一視する
- `LastName`というプロパティと、コンストラクターの第2引数を同一視する

というような、引数位置(position)とプロパティ(property)の間の同一視を用いようという意味合い。

結構これを認めるのはC#としてはチャレンジになります。「頭文字の大小を無視」も、「名前の一致を見て同一視」も、これまで結構避ける主義でした。

ただ、以下に紹介する機能を、既存の型に対しても使えるようにしようとすると、どうしてもこういう仕組みが必要になります。

## 3つの新機能

position-to-propertyマッチを使う3つの新機能は以下の通りです。

- immutableオブジェクトに対するオブジェクト初期化子
- with式
- 位置指定のパターン(positional pattern)

### immutableオブジェクトに対するオブジェクト初期化子

先ほど出した`Person`クラスみたいなimmutableなオブジェクト(プロパティがget-onlyで、値の書き換え不能)を初期化したい場合、コンストラクター引数に値を渡す必要があります。でも、オブジェクト初期化子構文を使っての初期化を行いたいことがあります。なので、

<pre class="source" title="">
<code><reserved></span><span class="reserved">new</span> <span class="type">Person</span> { FirstName = <span class="string">"Mickey"</span>, LastName = <span class="string">"Mouse"</span> }
</code></pre>

というコードを、

<pre class="source" title="">
<code><reserved></span><span class="reserved">new</span> <span class="type">Person</span>(<span class="string">"Mickey"</span>, <span class="string">"Mouse"</span>)
</code></pre>

に置き換えようという機能を提供したい。

で、このためには、どのプロパティがどの引数に対応するか知る必要があります。

### with 式

immutableなオブジェクトは、値の変更も面倒になります。書き換え不能なわけで、わざわざ、一部の値を変更したうえで、新しいオブジェクトを作る必要があります。この面倒を軽減するために、with式という構文が提案されています。

以下のような書き方で、

<pre class="source" title="">
<code>p <span class="reserved">with</span>  { FirstName = <span class="string">"Minney"</span> }
</code></pre>

以下のようなコードを生成します。

<pre class="source" title="">
<code><reserved></span><span class="reserved">new</span> <span class="type">Person</span>(<span class="string">"Minney"</span>, p.LastName)
</code></pre>

こちらも同様に、プロパティに対応する引数を知る必要があります。

### 位置指定のパターン

C# 7に向けて実装中のパターン マッチング機能で、以下のようなコードが書けます。

<pre class="source" title="">
<code>p <span class="reserved">is</span> <span class="type">Person</span>(<span class="string">"Mickey"</span>, *)
</code></pre>

これは、`p`の型が`Person`かつ、`FirstName`が`"Mickey"`ということを調べる式です。

第1引数が`firstName`なので、対応するプロパティ`FirstName`の値を調べます。

こういう書き方は位置指定のパターン(positional pattern、あるいは、position-based pattern)といって、コンストラクターの逆操作です。

<pre class="source" title="">
<code>p <span class="reserved">is</span> <span class="type">Person</span>(<span class="string">"Mickey"</span>, *) ⇔ <span class="reserved">new</span> <span class="type">Person</span>(<span class="string">"Mickey"</span>, <span class="string">"..."</span>)
</code></pre>

一方、以下のようにプロパティ指定でのパターン(property pattern)もあって、こちらはオブジェクト初期化子の逆操作になります。

<pre class="source" title="">
<code>p <span class="reserved">is</span> <span class="type">Person</span> { FirstName <span class="reserved">is</span> <span class="string">"Mickey"</span> } ⇔ <span class="reserved">new</span> <span class="type">Person</span> { FirstName = <span class="string">"Mickey"</span> }
</code></pre>

そして再三になりますが、この位置指定のパターンにもposition-to-propertyマッチが必要になります。

もちろん、position-to-propertyマッチに頼らない明示的な挙動の上書きも用意する(`GetValues`メソッドや、`is`演算子のオーバーロードなどが候補になっている)つもりだそうですが、既定の挙動としてはposition-to-propertyマッチを使いたいとのことです。

## 既存の型への新構文の適用

position-to-propertyマッチを認めるというのは、結構、C#としては主義の変更というか、大幅な妥協ではあります。
(当然、issueページのコメント欄は、結構な割合でネガティブなコメントが埋まっていたりします。)

最初は、レコード型という新しい構文を用意して、その構文で作ったクラスや構造体で、上記の3つの構文を使えるようにする(使うための特殊なメソッドを生成する)つもりで考えていました。

が、その場合、既存の型には上記の便利な構文が使えないことになります。そして、すでに世に出回っている多くのコードに対して逐一「レコード型で書き換えろ」なんて言えるはずもありません。

なのでこの妥協、この主義変更ということになります。
実際のところ、世に出ているコードの多くが、コンストラクター引数とプロパティの名前を(頭文字の大小除いて)同じにしていたりするので、
実用性を考えるとよい妥協しどころではあります。
