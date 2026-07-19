---
title: "C# 8.0 の予告"
source_url: "https://ufcpp.net/blog/2018/11/cs80_net48/"
content_type: "BlogEntry"
published_at: "2018-11-14T18:40:47"
updated_at: "2018-11-15T00:22:32"
tags: []
umbraco_id: 2175
parent_id: 2174
sort_order: 0
aliases: []
---

# C# 8.0 の予告

一昨日、C# 8.0 に関するブログが出たわけですが。

- [Building C# 8.0](https://blogs.msdn.microsoft.com/dotnet/2018/11/12/building-c-8-0/)

個人的には「最近全然ブログ書かない C# チームが働いただと…」的な感想もあるんですが
(C# 7.3 のときとか「半年前にリリースしてたわ」みたいなブログでした)。
近々プレビュー版が公開されるであろう C# 8.0 の予告記事です。

Visual Studio 15.9 正式リリースに続いて近々、Visual Studio 16.0 のプレビュー版も公開されて、
それと一緒に .NET Core 3.0 と C# 8.0 もプレビュー公開になると思われます。

## .NET Framework 4.8 は未サポート？

で、「.NET Framework 4.8 は .NET Standard 2.1 に追従しないので、C# 8.0 に対応しない」みたいな感じのことが話題になっていますが。
これ、多少不正確でして。
正しくは、

- 一部、必要なライブラリが .NET Framework 4.8 には提供されない
  - [Ranges](https://github.com/dotnet/csharplang/issues/185) … [`Range`構造体と`Index`構造体](https://github.com/dotnet/coreclr/pull/20899)
  - [Async Streams](https://github.com/dotnet/csharplang/issues/43) … [`IAsyncEnumerable<T>`インターフェイスと`IAsyncDisposable`インターフェイス](https://github.com/dotnet/coreclr/pull/20628)
- ランタイム自体に手を入れないと実現できない機能があって、 .NET Framework 4.8 はそれを実装しない
  - [インターフェイスのデフォルト実装](https://github.com/dotnet/csharplang/issues/52)

という3つの機能に制限が掛かるだけ。他の機能は普通にどの TargetFramework でも動きます。
[null 許容参照型](https://github.com/dotnet/csharplang/issues/36)とか[パターン マッチング](https://github.com/dotnet/csharplang/issues/45)の完全版とか switch 式とかは .NET Framework 1.0 ですら動くと思われます。

### 必要ライブラリ

まあ、`Range`/`Index`構造体なんて結構小さい型なので、
例のごとく[自分で同じ名前・同じ機能の構造体を書いてしまえば](../../../../study/csharp/cheatsheet/listfxlangversion.md#how-to)普通に古いランタイムでも C# 8.0 の機能を使えます。

といっても、Ranges の機能は、配列とか `Span<T>` が対応していて初めて役に立つものです。
(以下のような書き方をするためには、配列側が対応している必要あり。)

<pre class="source" title="">
<code><span class="reserved">int</span>[] data = { 1, 2, 3, 4, 5, 6 };
<span class="type">Span</span>&lt;<span class="reserved">int</span>&gt; span = data[1..^1];
<span class="reserved">foreach</span> (var x <span class="reserved">in</span> span)
    Console.WriteLine(x); <span class="comment">// 2, 3, 4, 5</span>
</code></pre>

既存の型に `Range` 型対応を混ぜ込むのはちょっと無理なので、
その意味では .NET Standard 2.1 でないと大して役に立たない機能になります。

Async Streams の方も似たような感じ。
`IAsyncEnumerable<T>`インターフェイス自体の移植は簡単ですが、
それに対応したライブラリがないとあんまりおいしくないかもしれません。

### インターフェイスのデフォルト実装

本当にどうあがいても .NET Framework 4.8 では動かせないのはこちら。
インターフェイスのデフォルト実装だけです。

ちなみに、どんな感じで「動かせない」かというと、

- TargetFramework net48 でも、LangVersion 8.0 自体は選べる
  - 前述のとおり、大半の機能は普通に使えます
- TargetFramework net48 を選んだ場合、デフォルト実装を使ったところだけコンパイル エラーになる

みたいな感じ。
デフォルト実装自体そんなに使う機能でもないと思うので、
大抵の状況では特に問題にならないと思います。

### RuntimeFeature クラス

デフォルト実装の話、
要するに、LangVersion だけじゃなくて、TargetFramework によっても文法に分岐が掛かることになります。

ちなみに、正確にいうと、TargetFramework 自体を見て分岐しているのではなくて、
[`RuntimeFeature`クラス](https://source.dot.net/#System.Runtime/System.Runtime.cs,6753)のプロパティがあるかないかで分岐しています。

以下のようなクラスなんですが、この`DefaultImplementationsOfInterfaces`プロパティが存在するランタイムでだけデフォルト実装が使えます。

<pre class="source" title="">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">RuntimeFeature</span>
{
<span class="inactive">#if</span> FEATURE_DEFAULT_INTERFACES
        <span class="reserved">public</span> <span class="reserved">const</span> <span class="reserved">string</span> DefaultImplementationsOfInterfaces = <span class="string">"DefaultImplementationsOfInterfaces"</span>;
<span class="inactive">#endif</span>
    <span class="reserved">public</span> <span class="reserved">const</span> <span class="reserved">string</span> PortablePdb = <span class="string">"PortablePdb"</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> IsSupported(<span class="reserved">string</span> feature) { <span class="reserved">throw</span> <span class="reserved">null</span>; }
}
</code></pre>

## .NET Standard 2.1

ちなみに、ちょっとわかりにくいですが、

- 登場時期: .NET Core 3.0 = .NET Standard 2.1 = .NET Framework 4.8
- 持っている機能: .NET Core 3.0 > .NET Standard 2.1 ≒ .NET Core 2.1 > .NET Framework 4.8 = .NET Standard 2.0

みたいな感じ。
.NET Standard 2.1 の主だった新機能は`ValueTask` がらみと `Span<T>` がらみです。
.NET Core 2.1 ですでにおなじみ(？)のやつ。

(Ranges と Async Streams、最初は .NET Core 3.0 でないと使えないのでは疑惑も多少。
.NET Standard 2.1 に入るのかな…)

## .NET Framework の今後の扱い

ということで、C# 8.0 への対応という意味では .NET Framework でもそこまで大した問題にはならないと思います。

が、まあ、「.NET Framework 4.8 は最新のものに追従しない」というのは事実。

とりあえず、

- 同世代の .NET Core 3.0 では Windows 限定機能(WPF, UWP)にもついに対応する
  - もう .NET Framework の方でしか使えない機能が残っていないはず
  - 新規案件で .NET Framework を使うメリットがもう何もない
- 保守モードな既存のアプリにまで .NET Core への移行を要請しないだけ良心的
  - そのための「保守アップデート」が .NET Framework 4.8
  - セキュリティ パッチとかはきっちり当てて出す

という感じ。
この辺りに関しては、 C# 8.0 のブログ以前に、
今月初旬に出てる以下のブログの方ですでに告知済みだったり。

- [Announcing .NET Standard 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/11/05/announcing-net-standard-2-1/)

こっちを見る限り、.NET Framework 4.8 が追従しないのは、デフォルト実装だけじゃなくて、
`Span<T>`がらみの方が主みたいです。

「[`Span<T>`構造体](../../../../study/csharp/resource/span.md)」で書いているように、
`Span<T>`にはslow版(古いランタイムでも動く実装)とfast版(.NET Core 2.1 以降でないと動かない安全かつ高速な実装)の2種類あります。
その`Span<T>`を使ったライブラリの中には、fast版の安全性保証がないとまずいものもいくつかあって、
そういうものは .NET Standard 2.0 以前向けには提供されていません。

.NET Framework 4.8はこのfast `Span<T>`対応もできておらず、
なので、.NET Standard 2.0 のまま据え置きということになります。
