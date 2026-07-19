---
title: "小ネタ privateメンバーはAPIの一部か"
source_url: "https://ufcpp.net/blog/2016/12/tipsapisurface/"
content_type: "BlogEntry"
published_at: "2016-12-20T15:07:11"
updated_at: "2016-12-22T13:30:27"
tags: []
umbraco_id: 2003
parent_id: 1969
sort_order: 19
aliases: []
---

# 小ネタ privateメンバーはAPIの一部か

## ことの発端

なんかぐらばくさんとこので、[エラーになるはずのコードがPCLなプロジェクトでだけビルド通ってしまって問題になってた](https://twitter.com/Grabacr07/status/808972608309866496)らしい。

要点を抜き出すと以下のような感じ。

<pre class="source" title="なぜかフィールドの初期化が不要になってしまう例">
<code><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">DateTimeWrapper</span>
{
    <span class="type">DateTimeOffset</span> t;

    <span class="reserved">public</span> DateTimeWrapper(<span class="reserved">int</span> i)
    {
        <span class="comment">// t を初期化しないとコンパイル エラーになるはず</span>
        <span class="comment">// でも、なぜか PCL プロジェクトではエラーにならない</span>
    }
}
</code></pre>

本来ダメなはずのコードが、PCL プロジェクトでだけコンパイルできてしまうという問題。
「ちゃんと初期化しないと怒られるはず」というのが常識のC#でこれをやられると、ほんと見つけられないバグになったりします。

プロジェクトの種類によって挙動が変わる謎の不具合…

csprojの中身を見てみても、どうも最終的に同じコンパイラーを使っていそう。
軽く[Process Explorer](http://forest.watch.impress.co.jp/library/software/prcsxplorer/)を眺めてみても、
ちゃんと同じコンパイラーが動作していそう。
コンパイラーが同じなのに、なぜ同じコードのコンパイル結果が変わってしまうのか、
謎は深まるばかり…

## 原因は参照アセンブリ

で、調べてみたら、どうも、参照しているアセンブリが違うせいみたい。

- [Reference assemblies need to include private struct fields #6185](https://github.com/dotnet/corefx/issues/6185)

ちなみに、再現用のサンプル コード: [PrivateField](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2016/PrivateField)

### 参照アセンブリ

問題の話をする前にまず簡単に、アセンブリの種類について補足。
今、[NuGet](https://www.nuget.org/)とかでライブラリを参照すると、開発時と実行時で別のDLLが参照されたりします。

- 実装アセンブリ: 実際に動くコードが入っているDLL。実行時に参照されるのはこれ。
- 参照アセンブリ: APIサーフェスだけが入っているDLL。開発時にはこっちが参照される。

これは、開発環境と実行環境が違っても問題なく開発できるようにするための処置です。

元々は .NET Framework 3.5の頃に、
クライアント プロファイルっていう、クライアント上では使わない機能を削ったバージョンの .NET Frameworkインストーラーを用意したことが発端で、
「開発環境ではつかえたクラスが、実行に TypeLoadException を起こした」みたいな自体を回避するために作られた仕組みです。
その後、[PCL](https://msdn.microsoft.com/ja-jp/library/gg597391(v=vs.100).aspx)でも同様の手法が使われるようになりました。

要するに、

- 実行環境の数だけ、開発環境にも別バージョンの .NET のインストールが必要になる
- それをすべてインストーラーに同梱していたらインストーラー サイズが大きくなりすぎる
- コンパイルに必要な情報(APIサーフェス)だけ残して、メソッドの中身とかはごっそり削ったバージョンのDLLを用意して、開発環境ではそのDLLを参照する

みたいな仕組み。
ここで言うAPIサーフェスっていうのは「APIとして外に公開されている表層の情報」という意味あいです。
見えない部分は削ってしまえと。

### どこまでが API サーフェスか

ここでちゃんと考えないといけないのが、どこまでを API サーフェスとみなすべきか。
すなわち、「開発時に参照するだけならどこまでの情報を残す必要があって、どこまでを削って大丈夫か」という話です。

publicやprotectedなメンバーはわかりやすくていいでしょう。外から見えるので、当然APIサーフェスに含まれるべきです。

ちょっと微妙なラインがinternalで、本来は外から見えないはずですが、
[`InternalsVisibleTo`属性](https://msdn.microsoft.com/ja-jp/library/bb385840(v=vs.90).aspx)なんてものもあるので、
外から見える可能性が残ります。
なので、APIサーフェスになりえます(`InternalsVisibleTo`属性があるときだけでいいんですが、参照ライブラリに残す必要があります)。

そして、private。
privateメンバーは、外から参照する手段がありません。
(リフレクションを使うと取れたりはしますけども、コンパイル時には関係ない話です。)
なので、APIサーフェスとはみなされない…
はず…？

と思いきや、privateメンバーがコンパイルに影響する場面が1つだけあります。
それが、構造体のprivateフィールド。

### 構造体のprivateフィールド

いくつか、構造体のprivateフィールドがコンパイル結果に影響を及ぼす例を挙げてみましょう。

- [確実な初期化](../../../../study/csharp/resource/rm_struct.md#definite-assignment)ルール
- ポインター型
- 再帰レイアウト

#### 確実な初期化

C#では、構造体のフィールドは、コンストラクター内で必ず初期化しないといけない、初期化するまでは他のメンバーを呼べないという制約があります。
初期化忘れによるバグを防ぐ意図があります。

でも、空っぽの構造体は初期化しなくてもいいらしい。

<pre class="source" title="空っぽの構造体の初期化は不要">
<code><reserved></span><span class="reserved">struct</span> <span class="type">EmptyStruct</span> { }
<span class="reserved">struct</span> <span class="type">Integer</span> { <span class="reserved">private</span> <span class="reserved">int</span> _x; }

<span class="reserved">struct</span> <span class="type">DefiniteAssignement</span>
{
    <span class="type">EmptyStruct</span> _e;
    <span class="type">Integer</span> _i;

    DefiniteAssignement(<span class="reserved">int</span> i)
    {
        <span class="comment">// 中身があるものは初期化必須</span>
        _i = <span class="reserved">new</span> <span class="type">Integer</span>();
        <span class="comment">// 一方で、EmptyStruct みたいに空っぽのものは初期化不要</span>
    }
}
</code></pre>

中身の有無によって挙動が変わります。

#### ポインター型

基本的に、[GC](../../../../study/csharp/resource/rm_gc.md#garbage-collection)管理下のオブジェクトのポインターを作るのは危険です。

そこで、C#では以下の条件を満たす型(非管理型(unmanaged type)と呼びます)でだけポインターを作ることを認めています

- 参照型ではない
- ジェネリックではない
- 上記2条件を再帰的に満たす(フィールドに1つ含まない)

例えば、もし仮にこの条件を満たさない(GC管理下にある)型のポインターを作れたとします。
そうすると、以下のような問題のあるコードが書けてしまいます。
(そうならないように、赤線の部分をコンパイル エラーにしている。)

<pre class="source" title="managedな型のポインターが作れた場合の問題">
<code><reserved></span><span class="reserved">using</span> System.Runtime.InteropServices;

<span class="comment">// 参照型を含む構造体</span>
<span class="reserved">struct</span> <span class="type">Wrapper</span> { <span class="reserved">object</span> _obj; }

<span class="reserved">class</span> <span class="type">ManagedPointer</span>
{
    <span class="reserved">public</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> X()
    {
        <span class="comment">// Wrapper みたいに内部的に参照型のフィールドを持っている型は、本来はポインター化できない</span>
        <span class="comment">// sizeof 取得も本来はできない</span>

        <span class="comment">// unmanaged なメモリを確保</span>
        <span class="comment">// AllocHGlobal で取得したメモリ領域は初期化されている保証がない</span>
        <span class="comment">// 実行するたびに違う値が入ってる</span>
        <span class="reserved">var</span> p = <span class="type">Marshal</span>.AllocHGlobal(<span class="error"><span class="reserved">sizeof</span>(<span class="type">Wrapper</span>)</span>);
        <span class="type">Wrapper</span> a = *(<span class="error"><span class="type">Wrapper</span>*</span>)p;

        <span class="comment">// ここで GC が発生したとすると、</span>
        <span class="comment">// GC が TaskAwaiter 中の Task のフィールド(未初期化)を参照する</span>
        <span class="comment">// 未初期化(= 意味のないランダムな値)な参照先を見に行こうとして死ぬ</span>

        <span class="type">Marshal</span>.FreeHGlobal(p);
    }
}
</code></pre>

こちらも、メンバーに参照型を含んでいるかどうかを追うのに、構造体の中身を追う必要があります。

#### 再帰レイアウト

構造体の中にそれ自身の型のフィールドを持とうとすると、当然ですが無限再帰を起こします。
無限に再帰する構造体のレイアウトなんて決定できない(オーバーフローする)ので、当然禁止事項です。

<pre class="source" title="無限に再帰する構造体レイアウト">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Container</span>&lt;<span class="type">T</span>&gt;
{
    <span class="reserved">public</span> <span class="type">T</span> Item;
}

<span class="reserved">struct</span> <span class="type">RecursiveLayout</span>
{
    <span class="comment">// 無限再帰するので、この構造体はレイアウトが確定できない</span>
    <span class="type">Container</span>&lt;<span class="type">RecursiveLayout</span>&gt; <span class="error">_x</span>;
}
</code></pre>

再帰していないかどうかを調べるために、構造体の中身の情報が必要です。

### privateフィールドを残していない問題

この、「構造体は、中身のprivateフィールドの情報も残さないとまずい」というのに気づいたのは、
参照アセンブリの仕組みを導入したのよりもちょっと後です。
リリースまでには気づいてなくて、リリース後に不具合報告を受けて気づいたようで。

PCLプロジェクトから参照しているいくつかの参照アセンブリが、構造体のprivateフィールドまで削除してしまっていて、問題を起こします。

ということで、本題に戻りますが、PCLプロジェクトでだけ起こせる問題の数々。
以下のコード、本来はコンパイル エラーになるべきですが、PCLではコンパイルできてしまします。

1つ目。確実な初期化に漏れるケース。

<pre class="source" title="本来は未初期化エラーになるはず">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">struct</span> <span class="type">DefiniteAssignment</span>
{
    <span class="comment">// DateTimeOffset には中身があるはずなのに…</span>
    <span class="type">DateTimeOffset</span> _x;

    <span class="reserved">public</span> <span class="error">DefiniteAssignment</span>(<span class="reserved">int</span> n) { } <span class="comment">// PCL ではエラーにならない</span>
}
</code></pre>

2つ目。ポインター化できるかどうかの判定をミスるケース。

<pre class="source" title="本来はポインター化できない型をポインター化">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
<span class="reserved">using</span> System.Runtime.InteropServices;

<span class="reserved">class</span> <span class="type">ManagedPointer</span>
{
    <span class="reserved">public</span> <span class="reserved">unsafe</span> <span class="reserved">void</span> X()
    {
        <span class="comment">// TaskAwaiter は内部的に Task クラスのフィールドを1個だけ持っている</span>
        <span class="comment">// 本来はポインター化できない</span>
        <span class="reserved">var</span> p = <span class="type">Marshal</span>.AllocHGlobal(<span class="error"><span class="reserved">sizeof</span>(<span class="type">TaskAwaiter</span>)</span>);

        <span class="comment">// PCL ではエラーにならない</span>
        <span class="type">TaskAwaiter</span> a = *(<span class="error"><span class="type">TaskAwaiter</span>*</span>)p;

        <span class="comment">// ここで GC が発生したとするとまずい</span>

        <span class="type">Marshal</span>.FreeHGlobal(p);
    }
}
</code></pre>

3つ目。無限再帰なレイアウトを作れてしまうケース。

<pre class="source" title="無限再帰レイアウト">
<code><span class="reserved">using</span> System.Collections.Generic;

<span class="reserved">struct</span> <span class="type">RecursiveLayout</span>
{
    <span class="comment">// 無限再帰するので、この構造体はレイアウトが確定できない</span>
    <span class="type">KeyValuePair</span>&lt;<span class="type">RecursiveLayout</span>, <span class="type">RecursiveLayout</span>&gt; <span class="error">_x</span>; <span class="comment">// PCL ではエラーにならない</span>
}
</code></pre>

どれも結構まずいんですが、今のところ、これがPCLではコンパイルできてしまっています。
`DateTimeOffset`、`KeyValuePair`、`TaskAwaiter`などの構造体で、PCLが参照している参照アセンブリでは中身がごっそり削られているのが原因。

### この問題を踏む可能性

この問題ですが、根本的には「参照アセンブリを作るときに消しちゃいけないところまで消しすぎた」というのが原因なわけで、
参照しているもの次第で起こるかどうかが決まります。

問題が起きるケース:

- PCL を使っていて、上記の`DateTimeOffset` などを参照する
- 同様に、[.NET Standard 向けのライブラリ](https://docs.microsoft.com/ja-jp/dotnet/articles/standard/library) プロジェクトでも、該当する型を参照すると問題が起きる

起きないケース:

- アプリなど、実行アセンブリを直接参照しているもの
- 実装アセンブリを直接提供しているライブラリなら問題が起きない
  - [`ValueTask`](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/)や[`ValueTuple`](https://www.nuget.org/packages/System.ValueTuple/)は実装アセンブリしか提供していないので、こいつらでは問題は起きない

### 問題への対処(検討中)

とりあえず、どこの問題かというと参照アセンブリを作るツールになります。

今は構造体のすべてのprivateフィールドを削ってしまっている挙動を、以下のように変更する必要があります。
(フィールドをすべて残すのではなく、以下のルールにするのは参照アセンブリのサイズ削減のため。
C#コンパイラーが誤動作しないようにするにはこのルールで十分。)

- 1つでも値型のフィールドを持っていれば、`int`型など適当な型のフィールドを1個だけ作って含める
- 1つでも参照型のフィールドを持っていれば、`object`型のフィールドを1個だけ作って含める
- ジェネリックな構造体の場合は、ジェネリック型引数で与えられた型のフィールドは消さずに残す

今は、C# コンパイラー自身が実装アセンブリリと同時に参照アセンブリを作る機能を持っているみたいなので、
基本的にはC# チームの仕事かも。
(昔からそうだったわけではなくて、割かし最近、そういう機能を持った。
それ以前は、オープンになっていないツールで、標準ライブラリの参照アセンブリ作りをしてた。)

とはいえ、現在問題を起こしている参照アセンブリとかを、ちゃんと治ったバージョンのツールで生成しなおして、
パッケージをNuGetサーバーに上げなおす作業は、たぶんC# チームの範疇外。

問題を起こす状況も限られているし、複数のチームが絡んでいるしで、ちょっと修正には時間が掛かりそうな雰囲気…
