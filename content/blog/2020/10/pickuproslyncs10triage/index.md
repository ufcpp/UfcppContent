---
title: "ピックアップRoslyn: C# 10.0 トリアージ"
source_url: "https://ufcpp.net/blog/2020/10/pickuproslyncs10triage/"
content_type: "BlogEntry"
published_at: "2020-10-12T23:02:47"
updated_at: "2020-10-12T23:02:47"
tags:
  - "C# 10.0"
umbraco_id: 2314
parent_id: 2311
sort_order: 2
aliases: []
---

# ピックアップRoslyn: C# 10.0 トリアージ

[前回](../pickuproslynlowlevel10/index.md)、[前々回](../pickuproslynrecord10/index.md)の続きというか、大きくなりすぎたので分けたのの続き。

- [C# Language Design Meeting for September 23rd, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-23.md)
- [C# Language Design Meeting for September 28th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-28.md)
- [C# Language Design Meeting for September 30th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-09-30.md)
- [C# Language Design Meeting for October 5th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-05.md)
- [C# Language Design Meeting for October 7th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-07.md)

ここ数週、C# 10.0 向けの検討が続いていて、
そのうち大きなものは[前々回の record struct](../pickuproslynrecord10/index.md)と[前回の低水準機能](../pickuproslynlowlevel10/index.md)で、残りはこまごまとしたトリアージ作業になります。

今回でやっと最後、その残りのトリアージの話。

## NaN 比較

C# では、というか、IEEE 754 (浮動小数点数の標準規格)では、
NaN (Not a Number)との比較は常に false ということになっています。

<pre class="source" title="NaN との比較">
<code><span class="reserved">bool</span> <span class="method">m</span>(<span class="reserved">double</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> == <span class="reserved">double</span>.NaN;
 
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">m</span>(1.0)); <span class="comment">// 当然 false</span>
<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="method">m</span>(<span class="reserved">double</span>.NaN)); <span class="comment">// これですら false</span>
</code></pre>

最近の C# では「常に false な式」に対して警告を出すことが結構あるんで、
過去の文法に対しても「常に false 警告」を足してもいいんじゃないかという話があります。

ただ、これまでの C# だと、「警告であっても追加すると破壊的変更になりうる」ということで消極でした。

これに対して C# 9.0/.NET 5.0 では警告ウェーブ(AnalysisLevel オプション。[RC 1 記念ライブ配信](https://www.youtube.com/watch?v=VQLtwak8W0U&feature=youtu.be)のときに口頭説明はしてる)が入るので、今後は警告の追加もしていきたいということになっています。

で、NaN との比較の話に戻りますが、
実はすでに FxCop Analyzer (Roslyn 標準ではないものの、Visual Studio ではデフォルトで有効になっているアナライザー)が NaN 比較に対する修正を提案してきます。
「Roslyn 標準に置き換えるほどではない」ということで、「特に何もしない」とのこと。

## null 許容参照型の改善

C# 8.0 で [null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)が入りましたが、最初から完全なものを作るのは無理なので段階的に改善していくという計画になっていて、C# 9.0 でもいくつか改善が入っています。

- `MemberNotNull` 属性

<pre class="source" title="MemberNotNull 属性">
<code><span class="reserved">class</span> <span class="type">X</span>
{
    <span class="reserved">public</span> <span class="reserved">string</span> NotNull;
    <span class="reserved">public</span> <span class="type">X</span>() =&gt; <span class="method">Init</span>();
 
    <span class="comment">// このメソッドの呼び出し後、NotNull プロパティの非 null を保証</span>
    [<span class="type">MemberNotNull</span>(<span class="reserved">nameof</span>(NotNull))]
    <span class="reserved">private</span> <span class="reserved">void</span> <span class="method">Init</span>() =&gt; NotNull = <span class="string">&quot;&quot;</span>;
}
</code></pre>

- 制約なしジェネリック型に対する `T?`

<pre class="source" title="制約なしジェネリック型に対する T?">
<code><span class="inactive">#nullable</span> <span class="inactive">enable</span>
 
<span class="reserved">class</span> <span class="type">X</span>
{
    <span class="comment">// where T を書かないときも T? が利用できるように。</span>
    <span class="comment">// ただし、意味的には nullable というよりも &quot;defaultable&quot; で…</span>
    <span class="reserved">static</span> <span class="type">T</span>? <span class="method">M</span>&lt;<span class="type">T</span>&gt;() =&gt; <span class="reserved">default</span>;
 
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="reserved">string</span>? <span class="variable">s1</span> = <span class="method">M</span>&lt;<span class="reserved">string</span>?&gt;(); <span class="comment">// string? → string?</span>
        <span class="reserved">string</span>? <span class="variable">s2</span> = <span class="method">M</span>&lt;<span class="reserved">string</span>&gt;();  <span class="comment">// string → string?</span>
        <span class="reserved">int</span>?    <span class="variable">i1</span> = <span class="method">M</span>&lt;<span class="reserved">int</span>?&gt;();    <span class="comment">// int? → int?</span>
        <span class="reserved">int</span>     <span class="variable">i2</span> = <span class="method">M</span>&lt;<span class="reserved">int</span>&gt;();     <span class="comment">// int → int で 0 が返る</span>
    }
}
</code></pre>

で、C# 9.0 にも漏れたものがいくつかあって、引き続き 10.0 向けに検討していくとのこと。

- [`Task<T>` の改善](https://github.com/dotnet/csharplang/issues/3950)
  - 共変性を認めたい(`Task<T>` を `Task<T?>` に代入できるようにしたい>)
- [LINQ の改善](https://github.com/dotnet/csharplang/issues/3951)
  - 特に `source.Where(x => x != null).Select(x => xは非null扱い)` ができるようにできないものか
- 未初期フィールド(今のところ良案なし)

## required プロパティ

[前々回](../pickuproslynrecord10/index.md)、少し nominal record (オブジェクト初期化子で初期化する前提のレコード型)の話をしましたが、
C# 9.0 時点では nominal に(プロパティで)定義したメンバーは初期化を必須にできません。
常に省略可能で、省略した場合は 0/null に自動的に初期化されます。

<pre class="source" title="nominal record のプロパティは現状、明示的な初期化が不要">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span>
{
    <span class="comment">// X, Y ともに何も書かなくても別に構わない</span>
};
 
<span class="reserved">record</span> Point
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">init</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</code></pre>

これに対して、明示的な初期化を義務付けたいという話があって、
1案としては以下のような書き方が提案されています。
これを required プロパティといいます。

<pre class="source" title="required プロパティ(の1案)">
<code><span class="reserved">var</span> <span class="variable">p</span> = <span class="reserved">new</span> <span class="type">Point</span>
{
    X = 1, <span class="comment">// X は書かないとコンパイル エラー</span>
    <span class="comment">// Y はなくてもいい</span>
};
 
<span class="reserved">record</span> Point
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; req; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">init</span>; }
}
</code></pre>

元々「間に合う物なら C# 9.0 で」くらいの感じで提案が出ていたものなので、引き続き 10.0 候補として検討していくとのこと。

## 匿名型に対する with 式

これも[前々回](../pickuproslynrecord10/index.md)書きましたが、
レコード型は「名前付きの匿名型」という側面があります。

となると逆に、「匿名型は名前なしのレコード型」という扱いになっている方が自然で、
この一貫性を取るために、匿名型にも `with` 式を認めたいという話が出ています。

<pre class="source" title="匿名型に対する with 式">
<code><span class="reserved">var</span> <span class="variable">a</span> = <span class="reserved">new</span> { X = 1, Y = 2 };
<span class="reserved">var</span> <span class="variable">b</span> = <span class="variable">a</span> <span class="reserved">with</span> { X = 3 }; <span class="comment">// 9.0 時点ではできないものの、10.0 で検討</span>
</code></pre>

[discussion](https://github.com/dotnet/csharplang/discussions/3952)では「匿名型自体どうなの？」とか言われたりもしますが…

C# チーム的には前向き(たぶん、変更コストがそんなに高くなく、レコード型との一貫性を重要視してる)みたいで、10.0 候補になっています。
元々 `with` 式には 10.0 向け残作業(ユーザー定義の `Clone` メソッドとか)があるので、それと合わせて検討。

## shebang

C# でも shebang (Unix シェルでよくある、1行目に `#!` を書いてスクリプトを何で実行するか指定するやつ)を認めよう(C# コンパイラー的には単にコメント扱いで無視)という話があります。

<pre class="source" title="shebang">
<code><span class="inactive">#! dotnet run</span>
 
System.<span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hello&quot;</span>);
</code></pre>

ただ、これはどちらかというと [donet CLI](https://docs.microsoft.com/ja-jp/dotnet/core/tools/?WT.mc_id=DT-MVP-4028921)側の問題なので、C# チーム的には「X.0」(いつやるか未定)扱い。
「CLI 側が dotnet run でスクリプト実行できるようになったら本気出す」みたいな感じみたいです。

## リスト パターン

配列とか `List<T>` とか(あるいはもしかしたら汎用に `IEnumerable<T>` も)を `[]` を使った[パターン](../../../../study/csharp/datatype/patterns.md)でマッチングできるようにしたいという話があります。

<pre class="source" title="リスト パターン">
<code>
<span class="reserved">var</span> <span class="variable">x</span> = <span class="reserved">new</span>[] { 1, 2, 3 };
 
<span class="control">if</span> (<span class="variable">x</span> <span class="reserved">is</span> [1, 2, var i])
{
    ...
}
</code></pre>

(すでにコミュニティ貢献でプロトタイプ実装があったりします。)

これに対して C# チーム的には「リスト パターンは辞書パターンと一緒に考えたい」、
「ただ、プロパティ パターンの `{}` と明確に区別がついて、かつ、辞書らしい文法を思いつかない」という感じ。

「C# 10.0 に入れれる気はしないけども」という補足付きで「10.0 で検討」とのこと。

## global using

今、[マイクロソフトによる公式 C# チュートリアル](https://docs.microsoft.com/ja-jp/dotnet/csharp/tutorials/intro-to-csharp/hello-world?tutorial-step=1&WT.mc_id=DT-MVP-4028921)とかでは、「ブラウザーでコードを試す」という機能があって、ブラウザー内で C# コードを書いてその場で実行できたりします。

ここでは C# のスクリプト文法を使えるので、例えば、以下のような1行のコードが「実行」ボタン1つで実行できます。

<pre class="source" title="チュートリアル上ではこの1ライナーが有効">
<code><span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;Hellow World!&quot;</span>);
</code></pre>

これ、実は `using System;` なしで `Console` クラスにアクセスできたりします。
スクリプト文法限定なんですが、いくつかの名前空間は「デフォルトで `using` 済み扱い」みたいにする機能があるということだったりします。

一方で、C# 9.0 からは[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)という機能が入ります。
プロジェクト(csproj)を作ってコンパイルする通常の C# 文法とスクリプト文法の差を縮めたいという意図で、
ファイル直下にステートメントを書いて `Main` メソッドを省略できるという機能です。

ここで、「通常文法とスクリプト文法の差を縮めたい」という意図があるので、
前述の「デフォルトで `using` 済み扱い」も通常文法に入れたいという議題が上がります。
これを指して global using といっていて、コンパイラー オプションとか csproj 中のタグで、プロジェクト全体に「`using` した状態にする」というオプションを提供したいそうです。

[.NET Notebooks](https://devblogs.microsoft.com/dotnet/net-interactive-preview-3-vs-code-insiders-and-polyglot-notebooks/)とか、 .NET 6 辺りをターゲットにした「C# インタラクティブ実行環境」があったりするので、その辺りのユーザーの使用感のフィードバックをもらいつつ、C# 10.0 で検討とのこと。

## closed enum

enum 型に対して、「メンバー定義してない値は取らない」という保証を与えて、
`switch` の網羅性チェックが働くようにしたいという話があります。

例えば以下のコードは現状では警告が出るんですが、「警告をなくせる enum が欲しい」というのが closed enum です(ここでいう close (閉じる)というのは、「これ以上のメンバー追加はない」という意味です)。

<pre class="source" title="enum の網羅性">
<code><span class="reserved">int</span> <span class="method">m</span>(<span class="type">X</span> <span class="variable">x</span>) =&gt; <span class="variable">x</span> <span class="warning"><span class="control">switch</span></span>
{
    <span class="type">X</span>.A =&gt; 1,
    <span class="type">X</span>.B =&gt; 2,
    <span class="type">X</span>.C =&gt; 4,
    <span class="comment">// 今の enum の仕様だと (X)100 とか書けるので、A, B, C だけでは「網羅した」判定を受けない。</span>
    <span class="comment">// 警告が出る。</span>
};
 
<span class="reserved">enum</span> <span class="type">X</span>
{
    A, B, C
}
</code></pre>

この辺りの網羅性のロジックは、別途 C# 10.0 で検討されている [discriminated union](https://github.com/dotnet/csharplang/issues/113) でも同様なので、それと一緒に考えたいとのこと。

## トップ レベル関数

C# 9.0 で入った[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)で、トップ レベルにメソッドを書いた場合、
それはトップ レベルからのみアクセスできます。

<pre class="source" title="トップ レベルにメソッドを書いた場合の挙動">
<code><span class="reserved">using</span> System;
 
<span class="comment">// トップ レベルでメソッドを書く。</span>
<span class="reserved">void</span> <span class="method">m</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;m&quot;</span>);
 
<span class="comment">// トップ レベルから呼ぶのは OK。</span>
<span class="method">m</span>();
 
<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="comment">// トップじゃない場所から呼ぶとコンパイル エラー。</span>
    <span class="comment">// ちなみにエラー内容は「m が見つからない」じゃなくて、
    // 「トップ レベルの m はトップ レベルからだけ呼べる」。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="method">m</span>();
}
</code></pre>

少なくとも C# 9.0 時点では意図的にこういう仕様になっているんですが、
「将来、この `m` をプロジェクト内のどこからでも呼んでいい global 関数的なものとして認めてもいいんじゃないか」という議題は残っていました
(今エラーになるものを将来エラーじゃなくすというのは破壊的変更にはならないので検討の余地がある)。

とはいえ元々「可能性はある」と言っていただけなので、あまり積極的ではなく。
「もし C# を1から再設計するんなら入れるけど、今から入れるのはちょっと」みたいな意見の人が多いそうです。
今回やっぱりばっさりと「rejected」とのことです。

## プライマリ コンストラクター

[前々回](../pickuproslynrecord10/index.md)触れたとおり。
今、レコード型にだけ許されている `record Point(int X, int Y)` みたいな書き方(型名直後に `()` で引数リスト)をクラス、構造体にも認めようという話。

引き続き 10.0 目標で検討。

## パラメーターの null 検証の簡素化

null 許容参照型による null 検証はあくまでコンパイル時の検証で、
unsafe とか[抑止演算子の `!`](../../../../study/csharp/resource/nullablereferencetype.md#null-forgiving)とかを使うとコンパイル時検証をすり抜けられます。
また、構造体や配列要素の規定値とか、フロー解析がしにくくて、今のところ検証をすり抜けてしまう穴があります。

そこで、必要であればやっぱり実行時の検証、要するに以下のようなコードも必要だろうという空気感。

<pre class="source" title="実行時 null 検証">
<code><span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>)
{
    <span class="control">if</span> (<span class="variable">s</span> <span class="reserved">is</span> <span class="reserved">null</span>)
        <span class="control">throw</span> <span class="reserved">new</span> <span class="type">ArgumentNullException</span>(<span class="reserved">nameof</span>(<span class="variable">s</span>));
 
    ...
}
</code></pre>

これを、`string s!` とかで簡素化したいという案も出ています。
「文法は `!` でいいのか」みたいな部分で合意が取れておらず 9.0 では流れましたが、10.0 で再検討とのこと。

## generic type alias

`using` エイリアスで以下のような書き方をしたいという話はずっと昔からたびたび出ています。

<pre class="source" title="using エイリアスでジェネリック型引数を書きたい">
<code><span class="reserved">using</span> List&lt;T&gt; = System.Collections.Generic.<span class="type">List</span>&lt;T&gt;;
</code></pre>

「欲しいけど、他にたくさんある C# 10.0 候補を押しのけてまでは…」という感じみたいで、
「X.0」(いつやるか不明)行き。

## パラメーターに対する nameof

null 許容参照型の [`NutNullIfNotNull`](../../../../study/csharp/resource/nullablereferencetype.md#sec-generated-title-6) とかの登場で急に需要が高まったんですが、
属性内で、メソッドの引数を `nameof` 参照したいという要求があります。

<pre class="source" title="パラメーターを nameof 参照したい例">
<code><span class="reserved">using</span> System.Diagnostics.CodeAnalysis;
 
<span class="reserved">class</span> <span class="type">Path</span>
{
    <span class="comment">// 今、nameof(path) とは書けない。</span>
    [<span class="reserved">return</span>: <span class="type">NotNullIfNotNull</span>(<span class="string">&quot;path&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">string</span>? <span class="method">GetFileName</span>(<span class="reserved">string</span>? <span class="variable">path</span>);
}
</code></pre>

まあ、C# 8.0 時点でこれの需要が急増することはわかっていて、
単に優先度的に 9.0 に入らなかっただけです。
すでに実装は始めているそうなので、10.0 候補。

## Span<T> パターン

今や普通に `string` と `Span<char>`、`ReadOnlySpan<char>` を比較することがあるわけで、
だったら、`Span<chat>` を `switch` 式に掛けたいという要求が当然あります。

<pre class="source" title="Span に対して文字列リテラルで switch">
<code><span class="comment">// string に対してこんな感じの switch していたものを…</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">s</span>) =&gt; <span class="variable">s</span> <span class="control">switch</span>
{
    <span class="string">&quot;Id&quot;</span> =&gt; 1,
    <span class="string">&quot;Name&quot;</span> =&gt; 2,
    <span class="string">&quot;Age&quot;</span> =&gt; 3,
    <span class="reserved">_</span> =&gt; 0,
};
 
<span class="comment">// Span や ReadOnlySpan でもやりたい。</span>
<span class="reserved">int</span> <span class="method">M</span>(<span class="type">ReadOnlySpan</span>&lt;<span class="reserved">char</span>&gt; <span class="variable">s</span>) =&gt; <span class="variable">s</span> <span class="control">switch</span>
{
    <span class="string">&quot;Id&quot;</span> =&gt; 1,
    <span class="string">&quot;Name&quot;</span> =&gt; 2,
    <span class="string">&quot;Age&quot;</span> =&gt; 3,
    <span class="reserved">_</span> =&gt; 0,
};
</code></pre>

これは「Any Time」(C# チーム的には乗り気じゃないけど、コミュニティ貢献は受け付ける)扱いなんですが、
実際にコミュニティ貢献の Pull Request が出ていたりします。
それに対する細かい判断:

- `Span<char>`、`ReadOnlySpan<char>` に対する特殊対応なので気持ち悪いものの…
  - 実のところ `Span` に対しては `foreach` とかですでに特別扱いしているので今更
- 後から足すと破壊的変更にならないか…
  - `Span` は [ref 構造体](../../../../study/csharp/resource/refstruct.md)で `object` に代入できないとかの制限が幸いして、破壊的変更を避けれそう
- `ReadOnlySpan<char>` だけ？
  - `ReadOnlySpan<char>` を受け付けるんなら `Span<char>` も受け付けてよさそう
- `Memory<char>` と `ReadOnlyMemory<char>` は？
  - それはなしで。`m.Span` と書くだけでいいし、`Span` 限定で
- `switch` だけ認める？
  - パターンを掛ける任意のコンテキスト(`is` とかでも)で認めてよさそう
- ジャンプ テーブル化
  - 内部実装的なことをいうと、今、`string` に対する `switch` は `case` が6個以上あるときハッシュ値を使ったジャンプ テーブル化する最適化を掛けてる
  - `Span<char>`、`ReadOnlySpan<char>` でも同様の最適化が要る。アロケーション除けによるメリットを打ち消すくらい遅くなる実装は避けたい
