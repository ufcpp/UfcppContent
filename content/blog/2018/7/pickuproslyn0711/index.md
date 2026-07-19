---
title: "ピックアップRoslyn 7/11: using patterns and declarations"
source_url: "https://ufcpp.net/blog/2018/7/pickuproslyn0711/"
content_type: "BlogEntry"
published_at: "2018-07-11T17:53:34"
updated_at: "2018-07-11T17:54:46"
tags: []
umbraco_id: 2160
parent_id: 2159
sort_order: 0
aliases: []
---

# ピックアップRoslyn 7/11: using patterns and declarations

C# 8.0での追加目標で、[`using` ステートメント](../../../../study/csharp/resource/oo_dispose.md#using)絡みの機能が2つほど。

- [Using patterns and declarations #1703](https://github.com/dotnet/csharplang/pull/1703)

1つは、パターン ベース(`IDiposable`インターフェイスの実装不要)で`using`が使えるようになるというもの。
もう1つは、変数宣言・変数のスコープに紐づいた`using`。

## パターン ベースで using

C# の言語機能のいくつかは、単にメソッド呼び出しに変換するだけのシンタックスシュガーが多いです。
`foreach`、`await`、クエリ式など、いろんなものが「この名前のメソッドさえ実装していればどんな型でも使える」という類の構文になっています。
ですが、微妙にものによって挙動が違ったり。

- 拡張メソッドでもいい
  - [クエリ式](../../../../study/csharp/data/sp3_stdquery.md)
  - [`await`](../../../../study/csharp/async/sp5_awaitable.md#awaiter)
  - [分解](../../../../study/csharp/datatype/deconstruction.md#arbitrary-types)
  - [コレクション初期化子](../../../../study/csharp/functional/sp3_lambda.md#collectioninit) (ただし、[C# 6.0 以降](../../../../study/csharp/cheatsheet/ap_ver6.md#add-extensions))
- インスタンス メソッドでないとダメ
  - [`foreach`](../../../../study/csharp/data/sp_foreach.md#ownmaking)
  - [`fixed`](../../../../study/csharp/interop/sp_unsafe.md#custom-fixed)
- インターフェイスの実装が必須
  - [`using`](../../../../study/csharp/resource/oo_dispose.md#using)

ということで、インターフェイスの実装が必須で使い勝手が悪かった`using`ステートメントですが、C# 8.0で、これを「`Dispose()`と言うメソッドさえ持っていれば何でも使える」というものに変えるようです。

というのも、[ref構造体](../../../../study/csharp/resource/refstruct.md)がインターフェイスを実装できないものの、ref構造体で`using`を使いたい場面が非常に多い状況になっているので。
パターン ベースな`using`の需要がかなり上がっているみたいです。

ちなみに、これは`using`に限った話ではなく、上記のような挙動の差をなくしたいという話でもあります。
C# 8.0では他にも「[非同期 `foreach`](https://github.com/dotnet/csharplang/blob/master/proposals/async-streams.md)」みたいな話もあって、これと関連して、上記の「`foreach`が拡張メソッドの`GetEnumerator`を受け付けないのは変じゃない？」みたいなことも言われています。こちらもセット。

## using 変数宣言

これまで、`using`を使うときには以下のような書き方でした。

<pre class="source" title="">
<code><span class="reserved">using</span> (<span class="reserved">var</span> d = someDisposable)
{
    <span class="comment">// このスコープ内を抜けたら Dispose</span>
}
</code></pre>

で、C# 8.0 では、以下のような書き方を認めようという話です。

<pre class="source" title="">
<code>{
    <span class="comment">// 変数宣言と同時に、その変数を using</span>
    <span class="reserved">using</span> <span class="reserved">var</span> d = someDisposable;

    <span class="comment">// 変数のスコープを抜けたら Dispose</span>
}
</code></pre>

C# の言語機能としては「`using`修飾付きの変数宣言」みたいになるようです。

この機能を追加する主な動機は、以下のような「`using`の入れ子」の解消です。

<pre class="source" title="">
<code><span class="comment">// 同寿命のリソースを何個も使うとき、こんな感じになる</span>
<span class="reserved">using</span> (<span class="reserved">var</span> a = someDisposable)
<span class="reserved">using</span> (<span class="reserved">var</span> b = anotherDisposable)
<span class="reserved">using</span> (<span class="reserved">var</span> c = oneMoreDisposable)
{
    <span class="comment">// ここを抜けたら Dispose</span>
}

<span class="comment">// それをこう変えたい</span>
<span class="reserved">using</span> <span class="reserved">var</span> a = someDisposable;
<span class="reserved">using</span> <span class="reserved">var</span> b = anotherDisposable;
<span class="reserved">using</span> <span class="reserved">var</span> c = oneMoreDisposable;

<span class="comment">// メソッドを抜けたら Dispose</span>
</code></pre>

これも、`fixed`ステートメントにも同じことが言えそう(同寿命で重ねることが結構ある)ということで、
同じような「`fixed`変数宣言」も考えているそうです(こちらはたぶんC# 8.0よりも将来の話)。
