---
title: "ピックアップRoslyn 9/3"
source_url: "https://ufcpp.net/blog/2017/9/pickuproslyn0903/"
content_type: "BlogEntry"
published_at: "2017-09-03T20:10:44"
updated_at: "2017-09-03T20:10:44"
tags: []
umbraco_id: 2081
parent_id: 2080
sort_order: 0
aliases: []
---

# ピックアップRoslyn 9/3

今日は以下の3本立て:

- 次のリリース(C# 7.2)の作業どのくらい進んでるのか久々に眺めてみた
- 1個、Champion(取り組むこと自体は確定)判定を受けた提案が出てた(return, break, continue 式)
- C# 8.0に向けて、「null許容参照型」の作業が最近活発化してる

## C# 7.2(次のマイナーリリース)を試してみる

最近ちょっと本腰を入れてC#コードでの速度最適化の作業をしてたりするんですけども、
そこでたどり着いた1つの境地が「[`stackalloc` 強すぎ](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/TypeRepositoryBenchmarks/StackallocDistinctBenchmark/DistinctBenchmark.cs#L7)」だったりします。

要するに、まあ .NET のヒープ確保は速いとは言われつつも、そもそもなくせるならなくす方が当然速い。
そのための手段として有効なのが構造体の活用と、この`stackalloc`。
ただ、まあ、unsafeコードだらけになります。

そのunsafeまみれを解消するのが [C# 7.2](https://github.com/dotnet/csharplang/milestone/6) (次のマイナーリリース)のテーマだったりするわけです。
で、そういえば、「[`Span<T>`を使えばsafeな文脈で `stackalloc` を使える](https://github.com/dotnet/csharplang/issues/535#issuecomment-299288113)」みたいな話を見かけたなぁとか思って、現状の実装状況を確認してみたりしました。

今、Visual Studio 2017 Previewでも提供されていないような新しい言語機能を試そうと思った場合、以下のようにすれば行けます。

- Roslyn の nightly build から所望のバージョンの[Microsoft.Net.Compilersパッケージ](https://dotnet.myget.org/feed/roslyn/package/nuget/Microsoft.Net.Compilers)を取ってくる
  - 今回の場合は、[2.6.0-rdonly-ref-62101-05](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-0902/ConsoleAppRefReadonly/ConsoleAppRefReadonly.csproj#L10)ってバージョンを参照
  - この NuGet パッケージを参照していると、ビルド時に使われる C# コンパイラーが NuGet パッケージ中のものに刺し変わる
  - ちなみに、刺し変わるのはビルドに使われるもののみで、Visual StudioのC# エディター上でリアルタイムに動いているコンパイラーは刺し変わらないみたいです(ビルドは通るけどVisual Studio上ではエラーが出まくるみたいな状態になります)
- [csproj に`<LangVersion>7.2</LangVersion>` タグを追加](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-0902/ConsoleAppRefReadonly/ConsoleAppRefReadonly.csproj#L6)

で試してみた結果が以下のような感じ。

- [ConsoleAppRefReadonly/Program.cs](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/Csharp7_2-0902/ConsoleAppRefReadonly/Program.cs)

このバージョンのC#コンパイラーは Roslynリポジトリの[readonly-ref](https://github.com/dotnet/roslyn/tree/features/readonly-ref)ブランチが元になっている物っぽいんですが、
名前通りではなくて radonly-ref 以外の機能も含まれています。
含まれているのは以下のようなもの:

- [ref readonly](https://github.com/dotnet/csharplang/issues/38) … [参照引数](../../../../study/csharp/resource/sp_ref.md#sec-byref)を書き換え不能にできる機能
  - (昔は「readonly ref」の語順だったものの、今は「ref readonly」の順になっています)
  - in … reaf readonlyの省略形。out引数の対比として、「入力専用の参照引数」という意味で in というキーワードが利用できる
- [ref拡張メソッド](https://github.com/dotnet/csharplang/issues/186) … 「`ref this`」あるいは「`in this`」という書き方で、[拡張メソッド](../../../../study/csharp/functional/sp3_extension.md)の第1引数を参照渡しにできる
- [stackonly struct](https://github.com/dotnet/csharplang/issues/666) … `Span<T>`構造体は[参照戻り値・参照ローカル変数](../../../../study/csharp/resource/sp_ref.md#ref-returns)と同じ扱いをしないといろいろ不具合が起こるので、それをコンパイル時にチェックする
  - `Span<T>`がらみの機能の一環として、「`Span<T>`の変数で受けるなら`stackalloc`は安全」というものもある

この辺りはもう割と実装済みみたいでした。コンパイルも実行も可能。

## return, break, continue 式

- [Champion "return, break and continue expressions" #867](https://github.com/dotnet/csharplang/issues/867)

[throw 式](../../../../study/csharp/structured/oo_exception.md#throwexpr)と同じ文脈で、`return`, `break`, `continue`も認めてもいいのではないかという話。(補足: throw 式が使える文脈ってのは、以下のように、`=>`とか`??`とか`?:`の後ろです。)

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Action</span> a = () =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>(); <span class="comment">// =&gt; の後ろ</span>
    }

    <span class="reserved">static</span> <span class="reserved">int</span> F(<span class="reserved">int</span>? n) =&gt; n ?? <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>(); <span class="comment">// ?? の後ろ</span>
    <span class="reserved">static</span> <span class="reserved">int</span> F(<span class="reserved">bool</span> b) =&gt; b ? 1 : <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>(); <span class="comment">// ?: の2、3項目</span>

    <span class="reserved">static</span> <span class="reserved">int</span> F() =&gt; <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>(); <span class="comment">// =&gt; の後ろ</span>
}
</code></pre>

throw式の実装の時点でreturn式とかは要望がでていましたし、
6月くらいには[Design Meeting](https://github.com/dotnet/csharplang/blob/master/meetings/2017/LDM-2017-06-27.md)の議題に挙がってたみたいですが、
この度、「Champion」(やること自体は確定)に昇格。
実装時期は未定(まだ分類されてない。タグ付けなし)。

## null許容参照型

最近ついに、待望の「null許容参照型」がらみの作業が実働に入ったっぽい雰囲気。そのうちの1つが以下のissue投稿。

- [Nullable reference types: Mads's current thoughts #863](https://github.com/dotnet/csharplang/issues/863)

Mads (C# のプロダクトマネージャー)の null 許容参照型に関する現在の考えだそうです。
これまでと方針変更があったとかではなく、単に改めてまとめとして考えを表明しただけですが。

「nullがあるもの」だった参照型に対してわざわざ「null許容参照型」なんて呼んでいるのからわかる通り、
参照型も「何も修飾を付けなければnullはないもの」「`?`を付けたときだけnullを想定」扱いしたいという方針になっています。

もちろん単純にそれをやると破壊的変更なので、on/offは切り替えれるようにする。
切り替えはアセンブリ単位で、「このアセンブリnull許容参照型を前提としたコンパイル時検査を掛けています」みたいなアセンブリ属性も付けるみたい。

あと、null検査に関して、言語仕様と静的解析機能は分けたいみたい。

- 言語機能としては、あくまで「nullを許すかどうかの注釈を付ける文法を用意」というだけ
  - 参照型`T`に対して単に`T`と書くと非null、`T?`と書くとnull許容
  - 後置き`!`演算子を付けることでnull検査の対象から外す
      - パフォーマンスの都合で一時的にnullになるのをわかっていてやっている場合とか
      - 注釈が付いていない(C# 8.0以前に書かれていたり、解析オプションをoffにしていたり)アセンブリを参照する場合とか
- それに対してどういう検査をするかは、概念上はC#コンパイラーの外、[アナライザー](../../../../study/csharp/package/pkgcodeawarelibrary.md#sec-generated-title-2)として実装する

ちなみに「概念上は」と言っているのは、C#コンパイラー内で実装しないと効率が悪い「解析」もあって実装上はC#コンパイラーに組み込むことになるから。とはいえ、「C#言語の仕様です」として厳密にいろいろと決めるには難しすぎる機能なので、「どこまで厳密に解析するかはアナライザーの実装に依存」としたいみたいです。解析の対象を段階的に広げていきたいみたいな思惑もあると思われます(「言語仕様」としてしまうと、後からの警告の追加も破壊的変更になるので)。

あと、「null検査」と言いつつ、C#としては、実際には「default検査」と表現するべきかも。
以下のようなものが静的解析の対象になります。

1. 参照型に対する null の代入
1. 参照型でインスタンス化される可能性がある[型引数](../../../../study/csharp/oop/sp2_generics.md#typeparam)に対するdefaultの代入
1. 参照型をフィールドに持つ構造体に対するdefaultの代入

これは「段階的に広げていきたい」の典型で、1は「必須」(at least)、2は「任意にできる」(could be optional)、3は「任意であるべきだろう」(should be optional)的に書かれていたり。
