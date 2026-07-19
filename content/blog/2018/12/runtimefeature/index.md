---
title: "RuntimeFeature クラス"
source_url: "https://ufcpp.net/blog/2018/12/runtimefeature/"
content_type: "BlogEntry"
published_at: "2018-12-04T10:13:34"
updated_at: "2018-12-04T10:21:12"
tags: []
umbraco_id: 2182
parent_id: 2177
sort_order: 3
aliases: []
---

# RuntimeFeature クラス

先日 [C# 8.0 予告なブログ](../../11/cs80_net48/index.md)で書いた通り、
C# 8.0 で入る[インターフェイスのデフォルト実装](https://github.com/dotnet/csharplang/issues/52)は .NET ランタイム側の修正が必要な機能です。

今日は、そういう「ランタイム側機能」についての話を少し。

## ランタイム側機能

C# の言語機能は、C# コンパイラーがちょこっと頑張ってよい具合にコード生成して、
古い .NET Framework ランタイム上でも動くものが多いです。
「古いランタイム上では動かない新機能」というと、実は .NET Framework 2.0 での[ジェネリクス](../../../../study/csharp/oop/sp2_generics.md)の導入まで遡ります。
.NET Framework 2.0 は2005年リリースですし、
C# 8.0 には実に13年ぶりにランタイムの方に修正を求める機能が入ったことになります。

契機となったのはやっぱり .NET Core の存在です。
.NET Core は、オープンソースで開発ペースも速く、
side by side (1台のPCに複数バージョンを同時にインストール可能)なのでランタイムの更新がしやすいという利点があります。
ランタイム更新しやすいからこその、13年ぶりの新機能を追加です。

登場直後の .NET Core は .NET Framework の下位互換のような存在でしたが、
.NET Core 2.0 くらいから互換性が増してきて、
.NET Core 3.0 ではついに Windows 限定な WPF や UWP などの GUI フレームワークも .NET Core 上で動くようになりました。
一応、完全新規の案件であれば .NET Framework の方をわざわざ使う理由がないくらいにはなっています。
ようやく、次のステップに進む段階に入ったといえます。
これも、このタイミングで13年ぶりの新機能追加に至った要因でしょう。

## RuntimeFeature クラス

しかし、ランタイムの新旧によって使えない機能があるとなると、
それを検知・コンパイル時にエラーにする仕組みが必要になります。
コンパイルできたはいいけど、実際に動かそうとした段階で無理だったというのでは困ります。

その検知機構として用意されたのが、`RuntimeFeature`クラス(`System.Runtime.CompilerServices`名前空間)です。
以下のようなクラスになっていて、`const string`なメンバーが存在するかどうかで、その機能を使えるかどうかを判定します。

<pre class="source" title="RuntimeFeature クラス">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">RuntimeFeature</span>
{
<span class="inactive">#if</span> FEATURE_DEFAULT_INTERFACES
    <span class="reserved">public</span> <span class="reserved">const</span> <span class="reserved">string</span> DefaultImplementationsOfInterfaces = <span class="string">"DefaultImplementationsOfInterfaces"</span>;
<span class="inactive">#endif</span>
    <span class="reserved">public</span> <span class="reserved">const</span> <span class="reserved">string</span> PortablePdb = <span class="string">"PortablePdb"</span>;
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> IsSupported(<span class="reserved">string</span> feature);
}
</code></pre>

今のところ生えているメンバーは、

- `DefaultImplementationsOfInterfaces` … 今回追加されたインターフェイスのデフォルト実装の可否
- `PortablePdb` … 動的コード生成で Portable PDB を解釈できるかどうか

の2つです。

### PortablePdb

ちなみに、`PortablePdb`の方について補足。
まず、PDB はデバッグ情報が掛かれたファイルで、
Visual C++ の頃から同名の拡張子のファイルは作られていました。
C# でも、ビルド時に dll や exe と一緒に拡張子が pdb のファイルが作られていると思います。

拡張子自体はずっと同じ pdb ですが、内部の形式については最近ガラッと変わりました。
以前の pdb は、仕様がオープンになっておらず、
pdb を読み込めるデバッガーが Windows 依存でした(なので、通称 "Windows PDB")。
そこで、.NET Core では、せっかくなので仕様自体をオープンにした Portable な pdb 形式を作ることにしたそうです。
それが「Portable PDB」。

PDB は基本的に C# コンパイラーが生成するものなので、`RuntimeFeature` (ランタイム側)とは無関係そうに見えます。
では `PortablePdb` は何のためにあるかと言うと、動的コード生成です。
例えば C# スクリプト実行であっても、内部的にはちゃんと PDB を生成して、
デバッグ情報が取れるようにしてあります。
このとき、生成する PDB を Portable PDB にしていいか、クラシックな Windows PDB でないとダメかを判別するための機構がないと困るので、`RuntimeFeature.PortablePdb`があります。

## その他の RuntimeFeature 機能

その他に、`RuntimeFeature`クラスには
(.NET Standard 2.1 から)以下のような bool 型の静的プロパティもあります。

<pre class="source" title="DynamicCode">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">RuntimeFeature</span>
{
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> IsDynamicCodeSupported { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">bool</span> IsDynamicCodeCompiled { <span class="reserved">get</span>; }
}
</code></pre>

先ほどの2つとは違って、こちらは実行時に値を確認して使う用みたいです。
そのランタイムで、

- `IsDynamicCodeSupported` … そもそも動的コード実行は可能か
- `IsDynamicCodeCompiled` … 動的コード実行はコンパイルされているか(= インタープリター実行ではないか = パフォーマンスよく実行できるか)

の判別に使います。
例えば、動的コンパイルができない環境で[式ツリー](../../../../study/csharp/dynamic/sp3_expression.md)の `Compile` メソッドを使ったりすると、
高速化のためにやってることなのにかえって破滅的に遅いコードになってしまいます。
それを避けるために分岐に使うのがこれらのプロパティ。

## 今後入るかもしれないランタイム側機能

とりあえず、`DefaultImplementationsOfInterfaces` は最初の一歩です。
これからは定期的に、こういう .NET ランタイム側の修正が必要な機能が追加されていくものと思われます。
(おそらく、基本的にはメジャー バージョンアップのタイミングでの追加。
そんなに高頻度で追加はしないと思われます。)

例えば、以下のような issue ページがあります。
.NET ランタイム(CLR)に修正を入れれるなら実現できそうな C# 機能の一覧。

- [What language proposals would benefit from CLR changes?](https://github.com/dotnet/csharplang/issues/317)

挙がっている内容は、以下のようなものです。

### CLR unification of types across assemblies

別アセンブリで定義された「同名で同じメンバーを含む型」を同一視したいというもの。

DI 用途でほしかったり。
あと、匿名クラス(内部的に匿名クラスを生成してる)が public なところで使えない理由にもなっているので回避方法が欲しいという要望があります。

### Make void a first-class type

要するに、`Func<void>`とか`Task<void>`と書かせろ、
`Action`や非ジェネリック`Task`との分岐がめんどい、というあれ。

### Covariance and contravariance for classes

現状、[変性](../../../../study/csharp/oop/sp4_variance.md)が認められているのはインターフェイスとデリゲートだけなわけですが、どうにかしてクラスでも認めてほしいというやつ。

`Task<object>` に `Task<string>` を代入したいとか、そういうやつ。

### |, &, and ~ operators on a type parameter with the enum constraint

`where T : Enum` が付いてるとき、その型の変数に対してビット演算したいというやつ。
今、列挙型とジェネリクスの相性が悪すぎてつらく。

### Union and intersection types

最近 TypeScript に入ったあれ。
`string | int` で「`string` か `int` のどちらか」みたいな型を作ったり、
`IA & IB` で「2つのインターフェイス`IA`と`IB`の両方を実装した(両方のメソッドを使える)型」を作ったり。

ある程度は C# コンパイラーのレベルでできるんですが、
ランタイムに手を入れて型システムのレベルで対応した方がよいという話。

### Support generic indexers

インデクサーに型引数を取りたいと。
↓みたいな。(今は、この`T`がどうやっても使えない。)

<pre class="source" title="インデクサーに型引数">
<code><span class="reserved">public</span> <span class="reserved">interface</span> <span class="type">IOptions</span>
{
    <span class="type">T</span> <span class="reserved">this</span>&lt;<span class="type">T</span>&gt;[<span class="type">OptionKey</span>&lt;<span class="type">T</span>&gt; key]
    {
        <span class="reserved">get</span>;
        <span class="reserved">set</span>;
    }
}
</code></pre>

### Higher-kinded polymorphism

ジェネリクスの型制約に複雑な条件を付けたいというやつ。
例として挙がってるのは↓みたいなコード。

<pre class="source" title="複雑な型制約">
<code><span class="reserved">public</span> <span class="reserved">static</span> <span class="type">T</span>&lt;<span class="type">A</span>&gt; To&lt;<span class="type">T</span>, <span class="type">A</span>&gt;(<span class="reserved">this</span> <span class="type">IEnumerable</span>&lt;<span class="type">A</span>&gt; xs)
    <span class="reserved">where</span> <span class="type">T</span> : &lt;&gt;, <span class="reserved">new</span>(), <span class="type">ICollection</span>&lt;&gt;
{
    <span class="reserved">var</span> ta = <span class="reserved">new</span> <span class="type">T</span>&lt;<span class="type">A</span>&gt;();
    <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> xs)
    {
        ta.Add(x);
    }
    <span class="reserved">return</span> ta;
}
</code></pre>

### methods in enums

列挙型に直接メソッドを持ちたいというやつ。
拡張メソッドでの実装だとちょっとつらいこともあり。
