---
title: "C# 9.0 in Visual Studio 16.7 preview 3"
source_url: "https://ufcpp.net/blog/2020/6/cs9vs16_7p3/"
content_type: "BlogEntry"
published_at: "2020-06-29T22:14:14"
updated_at: "2021-02-09T21:15:50"
tags:
  - "C# 9.0"
umbraco_id: 2300
parent_id: 2297
sort_order: 2
aliases: []
---

# C# 9.0 in Visual Studio 16.7 preview 3

先週の話にはなってしまうんですが、Visual Studio 16.7 が Preview 3.1 になっています。
.NET 5 も Preview 6 に。

- [Announcing .NET 5.0 Preview 6](https://devblogs.microsoft.com/dotnet/announcing-net-5-0-preview-6/)
- [リリース ノート: Visual Studio 2019 バージョン 16.7 Preview 3](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes-preview#16.7.0-pre.3.1)

で、今回も C# 9.0 の新機能がいくつか入っています。

- Records
- Top-level statements
- Function pointers

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/cxdZyFBxZws" frameborder="0" allow="accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

昨日、このネタでライブ配信してたりするんですが、
今回、ついに配信時間が3時間の大台に…

配信時間が長くなっているのはチャット欄での応答が盛り上がりすぎたというのが原因です。
(「新機能が多くて長引いた」みたいなのではなく。)
特に Function pointers の話とか、そもそも付いてくる人がいると思っていなかったので、
Function pointers だけで相当な時間しゃべることになったのがだいぶ意外…

インタラクションが欲しくて始めたライブ配信だし、
タイム キーピングを気にしなくていいのも楽なので、
うれしい悲鳴なんですけども、

## !is 問題

最初に取り上げるのは C# 9.0 でもなんでもないんですけども。

[VS 16.7 p3 のリリース ノート](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes-preview#16.7.0-pre.3.1)を見ていて、null 抑制演算子の `!` に対するリファクタリングが入っていてなんとも言えない気持ちになったという話。

以下のコード、解釈の仕方を間違う人があまりにも多いらしく。

<pre class="source" title="!is">
<code><span class="reserved">if</span> (x !<span class="reserved">is</span> 0) { }
</code></pre>

`!x` が「x の否定」、not x の意味なせいで、それで勘違いしちゃう人がいたりします。
上記のコードは正しくは `(x!) is 0` の意味で、この `!` は後置きの `x!`。
否定の意味の `!` ではなくて、[C# 8.0 の null 許容参照型](../../../../study/csharp/cheatsheet/ap_ver8.md#nullable-reference-type) がらみの機能で、効果としては[単なる null 警告の抑止](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving)です。

挙句の果てに、`is 0` と書いた時点で「null ではないことが確定」なので、null 警告の抑止をする意味すらないという。

つまるところ、`!is` を not is と勘違いして使ってしまうと、真逆の挙動になるというひどいバグを生む原因です。
なのでしょうがないので…

リファクタリング1: 意味がないので `!` を消す

<pre class="source" title="!is → is">
<code><span class="reserved">if</span> (x <span class="reserved">is</span> 0) { }
</code></pre>

リファクタリング2: “ちゃんと”真逆に直す

<pre class="source" title="!is → is">
<code><span class="reserved">if</span> (x <span class="reserved">is not</span> 0) { }
</code></pre>

## is not T x

[`not` パターン自体は先月の時点で入っていたんですが](../../5/cs9net5p4/index.md)、
微妙に今回の 16.7 Preview 3 リリースで入った修正もあります。
以下のようなコードが有効になりました。

<pre class="source" title="is not T x">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">object</span> x)
{
    <span class="comment">// not パターンでも変数宣言できる</span>
    <span class="reserved">if</span> (x <span class="reserved">is</span> <span class="reserved">not</span> <span class="reserved">string</span> s)
    {
        <span class="comment">// ちなみに、ここで s の中身を読もうとすると「未初期化」エラー</span>
 
        s = <span class="string">&quot;&quot;</span>;
 
        <span class="comment">// s を読めるのはこの行以降</span>
    }
 
    <span class="comment">// ここは絶対 s が初期化されている保証あり</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(s);
}
</code></pre>

## Top-level statements

`Program.Main` が要らなくなります。
C# スクリプト モードじゃなくても以下のように、Top-level  (クラスとか名前空間の外)にコードが書けます。

<pre class="source" title="Top-level にステートメントを書けるように">
<code><span class="reserved">using</span> System;
 
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hellow World!&quot;</span>);
</code></pre>

ちなみに、「C# スクリプト モード」とはまたちょっと挙動が違ったりします。

Top-level の場合

- あくまで `Main` メソッドの自動生成
  - 変数やメソッドを書くと、ローカル変数、ローカル関数の意味になる
  - `args` (暗黙的変数)でコマンドライン引数を受け取れるし、`return` で終了コードを返せる
- [`#r` や  `#load` などのスクリプト専用機能](../../../../study/csharp/cheatsheet/apscripting.md#script-syntax)は使えない

スクリプトの場合

- ラッパー クラスが作られる
  - 変数はフィールドに、メソッドはインスタンス メソッドになる
- 逆に、`return` とか `namespace` とか「通常モード」専用の構文は使えない

ちなみに、Top-level に普通に `await` も書けます。
`await` があれば `async Task Main`、なければ `void Main` みたいな扱いです。

Top-level ステートメントを書いた上で、さらにどこかのクラスに `Main` メソッドを書いてしまった場合、
Top-level ステートメントの方が優先されます(書いてしまった `Main` は呼ばれない)。
警告だけは出ます。

また、複数のファイルに Top-level ステートメントを書いたり、
名前空間やクラスよりも後ろに書いた場合はコンパイル エラーになります。
あくまで、1ファイルの先頭にだけ Top-level ステートメントを書けます。

## Function pointer

関数ポインターを C# 上で書けるようになりました。

まあ、正直、大多数の人にとって直接触れる機能ではないです。
実質的には P/Invoke 専用機能になると思います(どうひねり出そうと思っても他の用途が思いつかない)。

以下のように、`delegate*` で「関数ポインター型」を作って、
`&` でメソッドのアドレスを取得できる機能です。

<pre class="source" title="関数ポインター">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">unsafe</span> <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">delegate</span>*&lt;<span class="reserved">int</span>, <span class="reserved">void</span>&gt; f = &amp;<span class="method">M</span>;
        f(1);
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> x) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(x);
}
</code></pre>

.NET の仕様上は上記コードに相当する命令(ldftn, calli)が元々あったりします。
ただ、C# からこれらの命令を使う手段が全くなくて、
これまでは IL アセンブラーや [`Reflection.Emit`](https://docs.microsoft.com/ja-jp/dotnet/api/system.reflection.emit) を使う必要がありました。

今、iOS や WebAssembly 対応のために、
`Reflection.Emit` による実行時コード生成を、
[Source Generator](../../5/sourcegenerator/index.md) によるコンパイル時コード生成に置き換えたいという話もあったりします。
P/Invoke の類も、 .NET Runtime の中で特殊対応するよりも、事前にソースコード生成したいみたいな話もあって、
そのために必要になる機能です。

「ないと困るから入れた」という類であって、
「便利に使いたい、簡単に使いたい」という動機はないので、
構文的に結構複雑だったりします。

ただ、関数ポインターで `<void>` を認めてくれるんなら、普通のデリゲートでも同じように書きたい…
(書けないし、今後もたぶんずっと無理)

<pre class="source" title="要望としてはある(たぶん今後も無理)">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="error"><span class="reserved">void</span></span>&gt; f = <span class="method">M</span>; <span class="comment">// こう書きたい(無理)</span>
        <span class="type">Action</span>&lt;<span class="reserved">int</span>&gt; a = <span class="method">M</span>; <span class="comment">// 戻り値が void かどうかで型が違う</span>
    }
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> x) =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(x);
}
</code></pre>

## Records

待望(？)の Records。
概ね、[6月9日にブログに書いた](../record0609/index.md)状態で実装されていそうな感じ。

一番シンプルな書き方をすると以下のようになります。
プライマリ コンストラクター構文。
引数の順序(position, 位置)に意味があるので positional record と言ったりもします。

<pre class="source" title="プライマリ コンストラクターを使った “positional” record">
<code><span class="reserved">using</span> System;
 
<span class="comment">// 一番シンプルな書き方はこうなる</span>
<span class="reserved">record</span> Point(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
        <span class="type">Console</span>.<span class="method">WriteLine</span>(p.X);
    }
}
</code></pre>

`class` や `struct` と並んで、`record` という型定義用のキーワードが増えます。

ちなみに上記コードは以下のコードとほぼ同じ意味になります。

<pre class="source" title="プライマリ コンストラクターの展開結果">
<code><span class="comment">// プライマリ コンストラクターの展開結果</span>
<span class="reserved">record</span> Point
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">init</span>; }
 
    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y)
    {
        <span class="reserved">this</span>.X = X;
        <span class="reserved">this</span>.Y = Y;
    }
}
</code></pre>

残念ながら、今のところ(というか、おそらく C# 9.0 リリース時点では)、
プライマリ コンストラクターを書けるのは `record` だけになりそうです。

一方、`init` は `class` や `struct` でも使えます。
例えば、以下のようなコードは有効な C# 9.0 コードになります。

<pre class="source" title="init アクセサー">
<code><span class="comment">// init に関しては çlass でも struct でも使える</span>
<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">init</span>; }
 
    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y)
    {
        <span class="reserved">this</span>.X = X;
        <span class="reserved">this</span>.Y = Y;
    }
}
</code></pre>

ということで、Records の説明をする上での本質は以下の2点からなります。

- じゃあ、 `class` と `record` は何が違うか
- プロパティの `init` は何か

### 補足: IsExternalInit 属性

`init` プロパティを表現するために `IsExternalInit` という名前の属性を使っているんですが、
これは、

- 現時点では .NET 5 にも入っていない
- リリース時点では .NET 5 には入る予定
- 古い .NET ランタイムに対するポーティングとかは提供せず「C# 9.0 をフルにサポートするのは .NET 5 のみ」という扱いにしたい

ということになっています。
ただ、以下のコードを自前で用意すれば、現時点の .NET 5 Preview や古い .NET ランタイムでも `record` や `init` を使えます。

<pre class="source" title="IsExternalInit 属性">
<code><span class="reserved">namespace</span> System.Runtime.CompilerServices
{
    <span class="reserved">internal</span> <span class="reserved">class</span> <span class="type">IsExternalInit</span> { }
}
</code></pre>

ちなみにこの型、中身空っぽでマーカー的に用意された型ですが、
実際の使われ方は [modreq](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/4) になります。

後述する `init` が、古い C# コンパイラーから触られるとまずい機能なので、触れなくするために modreq を使っています。

### record と class

現状だと、プライマリ コンストラクターを書けるのは `record` だけなんですが、
`class` や `struct` に対しても後々追加される可能性は結構高いです。

で、前述の通り、`init` プロパティを使うのであれば、`class`、`struct`、`record` でほぼ同じ書き方ができます。
じゃあ、`class`、`struct` と `record` の何が違うかと言うと…

- `struct` みたいな、全フィールドの memberwise 比較を元にした `Equals`、`GetHashCode`、`Clone` がコンパイラー生成される
- `record` は参照型

みたいな感じです。
要するに、`struct` 的な「値セマンティクス」を持つ参照型が `record`。

これだけ書いてしまうと大したことをしていないように聞こえますけども、
immutable なデータ構造を簡単に書けるようにするという意義があります。

参照型の「値比較」(memberwise に `Equals`、`GetHashCode` 実装)は [immutable に作らないとまずい](https://gist.github.com/ufcpp/6336a4c1033e431c6732e82e03029bc7)です。
一方で、immutable なクラスを真面目に書くのはすさまじく大変で、その負担を減らしてくれるのが `record` です。

あと、派生が絡んだ時の `Equals` 実装も意外とめんどくさくて、そこも `record` からのコード生成が頑張ってくれています。

### with 式

immutable なクラスの部分書き換えをする場合、基本的には Clone してから所望のメンバーだけを上書きと言う処理が必要になります。

それをやってくれるのが `with` 式で、`record` に対して以下のような書き方ができます。

<pre class="source" title="with 式">
<code><span class="reserved">using</span> System;
 
<span class="reserved">record</span> Point(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
 
        <span class="comment">// p を部分書き換え(この場合 X だけ書き換え)</span>
        <span class="reserved">var</span> p1 = p <span class="reserved">with</span> { X = 3 };
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>((p.X, p.Y));   <span class="comment">// (1, 2)。元の p は不変</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>((p1.X, p1.Y)); <span class="comment">// (3, 2)。p を Clone した上で X だけ書き換えてる</span>
    }
}
</code></pre>

これと同じことを `class` の手書きでやろうとすると、以下のように、Clone 後の書き換えで immutable であることが破たんします。

<pre class="source" title="with 式を使わず手書きしようとすると…">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">Point</span>
{
    <span class="comment">// 本当は set できるとまずいけど、Clone 後の書き換えのために必須になってしまう。</span>
    <span class="comment">// これを回避するために init アクセサー(後述)がある。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="type">Point</span>(<span class="reserved">int</span> x, <span class="reserved">int</span> y) =&gt; (X, Y) = (x, y);
    <span class="reserved">public</span> <span class="type">Point</span> <span class="method">Clone</span>() =&gt; <span class="reserved">new</span> <span class="type">Point</span>(X, Y);
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
 
        <span class="comment">// p を部分書き換え(この場合 X だけ書き換え)</span>
        <span class="reserved">var</span> p1 = p.<span class="method">Clone</span>();
 
        <span class="comment">// Clone 後の書き換えのためにやむを得ず public set。</span>
        <span class="comment">// init とか with とか、新構文が必要になる理由。</span>
        p1.X = 3;
 
        <span class="type">Console</span>.<span class="method">WriteLine</span>((p.X, p.Y));   <span class="comment">// (1, 2)。元の p は不変</span>
        <span class="type">Console</span>.<span class="method">WriteLine</span>((p1.X, p1.Y)); <span class="comment">// (3, 2)。p を Clone した上で X だけ書き換えてる</span>
    }
}
</code></pre>

(もっと複雑で、実行時コストも高い方法でよければもうちょっとやり様はあるんですが。
後述する `init` プロパティで一応、実行時コストは掛けずにこの問題を解決できるので、それを採用することになりました。)

ちなみに、原理的には `with` 式は `init` プロパティを持つ `class` や `struct` に対しても使えるはずなんですが、
C# 9.0 時点では `record` 専用構文になりそうです。
(スケジュールの問題。あとで `class` や `struct` に対する `wth` 式追加が検討される。)

### init アクセサー

プロパティのアクセサーに、`set` の代わりに `init` を使うことで、
初期化子や `with` 式でだけ書き換え可能なプロパティができます。

<pre class="source" title="init アクセサー">
<code><span class="reserved">class</span> <span class="type">InitOnly</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">var</span> p = <span class="reserved">new</span> <span class="type">InitOnly</span>
        {
            X = 1, <span class="comment">// 初期化子を使える</span>
        };
 
        <span class="error">p.X</span> = 2; <span class="comment">// これはコンパイル エラー</span>
 
        <span class="comment">// with 式での書き換え(Clone 後の書き換え)もできる</span>
        <span class="reserved">var</span> p1 = p <span class="reserved">with</span> { X = 3 };
    }
}
</code></pre>

`init` アクセサーは以下の場所からだけ呼び出せる制限付きの `set` みたいなものです。

1. そのクラスのコンストラクター内
2. オブジェクト初期化子
3. `with` 式
4. 他の `init` アクセサー内

1だけでよければ [get-only プロパティ](../../../../study/csharp/oop/oo_property.md#get-only)でも実現できるんですが、残りの3つのために `init` アクセサーが新設されました。

ちなみに、`set` と `init` を同時に書くことはできません。
というか、`init` アクセサーは内部的には「特殊な属性を付けた `set` アクセサー」として実現されています。
