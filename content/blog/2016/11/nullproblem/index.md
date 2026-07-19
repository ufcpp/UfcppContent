---
title: "Roslynに提案issueを立てた話: nullの扱いに関して"
source_url: "https://ufcpp.net/blog/2016/11/nullproblem/"
content_type: "BlogEntry"
published_at: "2016-11-12T10:30:15"
updated_at: "2016-11-12T15:40:51"
tags: []
umbraco_id: 1967
parent_id: 1966
sort_order: 0
aliases: []
---

# Roslynに提案issueを立てた話: nullの扱いに関して

MVP Global Summit (グロサミ)に行ってきてたわけですが。

なんか、行きの飛行機内で思いついてしまって、そのまま向こうで頑張って issue 投稿、
せっかくだからグロサミ中に [Mads](https://github.com/MadsTorgersen) (C# のPM)を捕まえて「こんな問題見つけちゃって、昨日ちょうどissue立てたんだけどどうしよう？」みたいな話を振ってきたり。

(先に具体例を書いておけば、どれだけ英語がつたなくても結構意図は伝えられる。)

(ちなみに、元々は帰国後にゆっくりページ書くつもりだったんだけど、なんかグロサミ参加の妙なテンションに任せて、
[kekyoさん](https://twitter.com/kekyo2)、[藤原さん](https://twitter.com/yfakariya)、[室星さん](https://twitter.com/RyotaMurohoshi)とかと部屋の飲みの最中にこの方々も巻き込んでissue書きました。)

- [Proposal: user-defined null/default check (non-defaultable value types / nullable-like types) #15108](https://github.com/dotnet/roslyn/issues/15108)

勢いで立てたものなので整理できてないんですが… たぶん2パートに分けた方が良さそう。
以下、改めて要約した内容(英語に訳して再投稿予定)。

## 2つの提案

nullの取り扱いと関連して、以下の2つを提案する。

- 非default値型: `default(T)`の状態を認めない値型が必要ではないか
- nullable-like型: 参照型と`Nullable<T>`以外にも、`?.`や`??`が使える型をユーザー定義できるようにすべきではないか

## 非default値型

[Method Contracts](https://github.com/dotnet/roslyn/issues/119)、特に[参照型の非null保証](https://github.com/dotnet/roslyn/issues/5032)を入れようと思うと、確実な初期化処理が必須になる。

しかし、構造体の場合、`defautl(T)`など、既定値によって初期化処理を通らない「0/nullクリア」が発生する。
既定値の「0/nullクリア」のせいで、Contractsや非null保証のフロー解析が狂う可能性がある。

### 例1: 非null保証

パフォーマンスのために、参照型を1つだけ持つようなラッパーを構造体で作ることがある。
(例: [ImmutableArray](https://github.com/dotnet/corefx/blob/master/src/System.Collections.Immutable/src/System/Collections/Immutable/ImmutableArray_1.cs)、)
今、パフォーマンスはC#の1つの大きなテーマであり、こういうケースは今後より一層増えるだろう。

例として以下のような構造体を考える。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Wrapper</span>&lt;<span class="type">T</span>&gt; <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> Wrapper(<span class="type">T</span> value)
    {
        Value = value ?? <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(value));
    }
}
</code></pre>

[レコード型](https://github.com/dotnet/roslyn/blob/master/docs/features/records.md)や[非null保証](https://github.com/dotnet/roslyn/issues/5032)が入れば、単に以下のように書けるだろう。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Wrapper</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> Value) <span class="reserved">where</span><span class="type">T</span> : <span class="reserved">class</span>
</code></pre>

単に`T`と書けば非nullとなり、nullを受け付けたければ`T?`と書くようになる。
問題は、この構造体を`default(Wrapper<T>)`で作ると、`T Value` (本来は非nullであるはず)がnullになってしまうことである。

### 例2: 値の制約付きの構造体

以下のような、値に制約の入った構造体を考える。この例は、正の数しか受け取れない数値型である。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">PositiveInt</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> PositiveInt(<span class="reserved">int</span> value)
    {
        <span class="reserved">if</span> (value &lt;= 0) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentOutOfRangeException</span>(<span class="reserved">nameof</span>(value));
        Value = value;
    }
}
</code></pre>

C#に[レコード型](https://github.com/dotnet/roslyn/blob/master/docs/features/records.md)や[Method Contracts](https://github.com/dotnet/roslyn/issues/119)が入ると、この構造体はおそらく以下のように書ける。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">PositiveInt</span>(<span class="reserved">int</span> Value) <span class="reserved">requires</span> Value &gt; 0;
</code></pre>

これで`Value`プロパティが常に0より大きいことが保証できているように見えるが、`default(PositiveInt)`のせいで、`Value`に0が入ることがあり得る。この値は無効なはずである。

### 提案: 非defaultフロー解析

現在提案されている参照型の[非null保証](https://github.com/dotnet/roslyn/issues/5032)は、フロー解析に基づいている。
値型が既定値でないことも、同じフロー解析で行えるはずである。

そこで、non-nullable reference typesに対して、non-defaultable value typesを提案したい。
何らかのアノテーション、例えば`[NonDefault]`属性を付けた構造体は既定値を取ってはいけないとするのはどうだろうか。

<pre class="source" title="">
<code>[<span class="type">NonDefault</span>]
<span class="reserved">struct</span> <span class="type">Wrapper</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> Value) <span class="reserved">where</span><span class="type">T</span> : <span class="reserved">class</span>

[<span class="type">NonDefault</span>]
<span class="reserved">struct</span> <span class="type">PositiveInt</span>(<span class="reserved">int</span> Value) <span class="reserved">requires</span> Value &gt; 0;
</code></pre>

このとき、non-nullable reference typesに倣って、以下のように警告を出す。

<pre class="source" title="">
<code><span class="type">PositiveInt</span> x = <span class="reserved">default</span>(<span class="type">PositiveInt</span>); <span class="comment">// warning</span>
<span class="type">PositiveInt</span>? y = <span class="reserved">default</span>(<span class="type">PositiveInt</span>); <span class="comment">// OK</span>
<span class="type">PositiveInt</span> z = y; <span class="comment">// warning</span>
<span class="type">PositiveInt</span> w = y ?? <span class="reserved">new</span> <span class="type">PositiveInt</span>(1); <span class="comment">// OK</span>
</code></pre>

non-defaultable value typesに対する`T?`は`Nullable<T>`を必要としない。
何故なら、`default(T)`は無効であり、`x.HasValue`を確かめなくても、`x == default(T)`で値を持っていないことが確認できるからである。
non-defaultable value typesに対しては`null`を`default(T)`と同一視してもいいかもしれない。

また、通常の構造体の中では、non-nullable reference typesのメンバーを持てないようにすべきだろう。
non-nullable reference typesを持てるのは、参照型か、non-defaultable value typesのみである。

## nullable-like型

現在、参照型と`Nullable<T>`構造体はC#コンパイラーによって特別な地位を与えられている。
すなわち、null条件演算子(`?.`)とnull合体演算子(`??`)の利用である。

しかし、これらの型以外にも、無効なインスタンスを`?.`や`??`で伝搬/差し替えしたいことがある。

この挙動はmonad的であり、[LINQ](http://mikehadlow.blogspot.jp/2011/01/monads-in-c-5-maybe.html)や[Task-like](https://github.com/ckimes89/arbitrary-async-return-nullable/)を使って無理やり解決しようとしている例もある。
しかし、悪用・乱用の類であり、決して読みやすいコードにはならないだろう。
無効なインスタンスの伝搬/差し替えは、やはり、`?.`や`??`を使うべきである。

### 例1: UnityEngine.Object

[Unity](http://japan.unity3d.com/)のゲーム中のオブジェクトの共通基底クラスとなる`UnityEngine.Object`は`operator ==`をオーバーロードしていて、オブジェクトが持っているネイティブ リソースがすでに破棄されているとき、オブジェクトをnull扱いする(`x == null`が真になる)。

しかし、参照型に対する`?.`や`??`では、オーバーロードした`==`は呼ばれない(`brtrue`命令によるnullチェックに展開される)。
そのため、以下のように、`?.`を使ったコードが正しく動かない。

<pre class="source" title="">
<code><span class="reserved">int</span>? X(UnityEngine.<span class="type">Object</span> obj)
{
    <span class="comment">// OK</span>
    <span class="reserved">if</span> (obj == <span class="reserved">null</span>) <span class="reserved">return</span> <span class="reserved">null</span>;
    <span class="reserved">return</span> obj.GetInstanceID();
}

<span class="comment">// runtime exception</span>
<span class="reserved">int</span>? Y(UnityEngine.<span class="type">Object</span> obj) =&gt; obj?.GetInstanceID();
</code></pre>

これまではUnityがC# 3.0にしか対応していなかったので問題にならなかったが、
Unity 5.5でC# 6.0に対応しようとしている。
この`?.`の挙動がはまるポイントになるだろう。

### 例2: Expected

無効な値としてnullを使うことを嫌う人が一定数いるが、その理由の1つが、「なぜ無効な値を返す必要があったのか」という原因に関する情報が消えることである。
そのため、`Nullable<T>`の代わりに、`T`と例外のunion型にあたる`Expected<T>`のような型を作って使おうとする人がいる。
例えば、[C++でそういう動きがみられる](http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2014/n4015.pdf)。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Expected</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Exception</span> Exception { <span class="reserved">get</span>; }
}
</code></pre>

もしC#でもそういう型を作るのであれば、`?.`を使った例外の伝搬や、`??`を使った例外からの復帰があってもいいのではないだろうか。

<pre class="source" title="">
<code><span class="type">Expected</span>&lt;<span class="reserved">string</span>&gt; x = <span class="reserved">new</span> <span class="type">Expected</span>&lt;<span class="reserved">string</span>&gt;(<span class="reserved">new</span> <span class="type">Exception</span>());
<span class="type">Expected</span>&lt;<span class="reserved">int</span>&gt; y = x?.Length;
<span class="reserved">string</span> z = x ?? <span class="string">""</span>;
</code></pre>

### 提案: ユーザー定義のnullable-like型

所定のパターンを実装した型であれば`?.`および`??`を使えるようにすることを提案する。
[Task-like](https://github.com/dotnet/roslyn/issues/7169)に倣ってこれをnullable-likeと呼ぼう。

例えば、以下のようなパターンはどうだろうか。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">NullableLike</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">bool</span> HasValue { <span class="reserved">get</span>; }
    <span class="comment">// propagate a valid value</span>
    <span class="reserved">public</span> <span class="type">NullableLike</span>&lt;<span class="type">U</span>&gt; Propagate&lt;<span class="type">U</span>&gt;(<span class="type">U</span> value);
    <span class="comment">// propagate an invalid value</span>
    <span class="reserved">public</span> <span class="type">NullableLike</span>&lt;<span class="type">T</span>&gt; Propagate();
}
</code></pre>

これで、先ほどの`Expected<T>`の例であれば、以下のように展開する。

<pre class="source" title="">
<code><span class="type">Expected</span>&lt;<span class="reserved">string</span>&gt; x = <span class="reserved">new</span> <span class="type">Expected</span>&lt;<span class="reserved">string</span>&gt;(<span class="reserved">new</span> <span class="type">Exception</span>());
<span class="type">Expected</span>&lt;<span class="reserved">int</span>&gt; y = x.HasValue ? x.Propagate(x.Value.Length) : x.Propagate&lt;<span class="reserved">int</span>&gt;();
<span class="reserved">string</span> z = x.HasValue ? x.Value : <span class="string">""</span>;
</code></pre>

ちなみに、このパターンに沿った`Expected<T>`の実装は以下のようになる。

<pre class="source" title="">
<code><span class="reserved">struct</span> <span class="type">Expected</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Value { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Exception</span> Exception { <span class="reserved">get</span>; }

    <span class="reserved">public</span> Expected(<span class="type">T</span> value)
    {
        Value = value;
        Exception = <span class="reserved">null</span>;
    }
    <span class="reserved">public</span> Expected(<span class="type">Exception</span> exception)
    {
        Value = <span class="reserved">default</span>(<span class="type">T</span>);
        Exception = exception;
    }

    <span class="reserved">public</span> <span class="reserved">bool</span> HasValue =&gt; Exception == <span class="reserved">null</span>;
    <span class="reserved">public</span> <span class="type">Expected</span>&lt;<span class="type">U</span>&gt; Propagate&lt;<span class="type">U</span>&gt;() =&gt; <span class="reserved">new</span> <span class="type">Expected</span>&lt;<span class="type">U</span>&gt;(Exception);
    <span class="reserved">public</span> <span class="type">Expected</span>&lt;<span class="type">U</span>&gt; Propagate&lt;<span class="type">U</span>&gt;(<span class="type">U</span> value) =&gt; <span class="reserved">new</span> <span class="type">Expected</span>&lt;<span class="type">U</span>&gt;(value);
}
</code></pre>
