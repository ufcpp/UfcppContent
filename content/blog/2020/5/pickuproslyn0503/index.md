---
title: "ピックアップRoslyn: C# 9.0 向け Design Notes"
source_url: "https://ufcpp.net/blog/2020/5/pickuproslyn0503/"
content_type: "BlogEntry"
published_at: "2020-05-03T20:01:09"
updated_at: "2020-05-03T20:02:55"
tags: []
umbraco_id: 2293
parent_id: 2290
sort_order: 2
aliases: []
---

# ピックアップRoslyn: C# 9.0 向け Design Notes

C# Language Design Notes が3件に、

- [C# Language Design Notes for Apr 15, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-04-15.md)
- [C# Language Design Meeting for March 20, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-04-20.md)
 - [C# Language Design Meeting for April 27, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-04-27.md)

提案ドキュメントのアップロードが3件。

- [Extending Partial Methods](https://github.com/dotnet/csharplang/blob/master/proposals/extending-partial-methods.md)
- [covariant return types](https://github.com/dotnet/csharplang/blob/master/proposals/covariant-returns.md)
- [Module Initializers](https://github.com/dotnet/csharplang/blob/master/proposals/module-initializers.md)

特に partial メソッドの拡張の話は Source Generator 関連です。
最初、Source Generator ネタでまとめて投稿しようかと思っていたものの、ちょっとまとまり悪くなりそうだったので1日3件ブログに。

- 1個前: [.NET 5 への Xamarin 統合に向けて](../featureswitch/index.md)
- 2個前: [C# Source Generator (first preview)を試してみる](../sourcegenerator/index.md)

## partial メソッドの拡張

コンパイル時のソースコード生成ができるようになると、

- 手書きで `int M();` みたいなメソッド宣言だけを書く
- それに反応して、何らかの `M` の実装をコード生成したい

みたいな要件が出てきます。

こういう、コード生成物と手書きコードのつなぎ用の機能として、
C# には 2.0 の頃から [partial type](../../../../study/csharp/cheatsheet/ap_ver2.md#partial)、
3.0 の頃から [partial method](../../../../study/csharp/cheatsheet/ap_ver3.md#partial_method) という機能があったりします。
ですがこれは、

- Windows Forms とかが生成する中身が空っぽのメソッドが先にある
- 手書きコードを足す必要がないのであれば、一切痕跡を残さず消す
- 手書きコードを足した場合、コード生成物の中から呼ばれるようになる

みたいな機能で、以下のような制限があります。

- [アクセス修飾子](../../../../study/csharp/oop/oo_conceal.md#level)は付けれない(暗黙的に private)
- 戻り値は `void` 出ないとダメ
- [`out` 引数](../../../../study/csharp/resource/sp_ref.md#out)は使えない

これに対して、今出ている要件は逆方向のつなぎ(手書きが先にあって、コード生成物が後)になります。
微妙に挙動に差があるんですが、同じ `partial` というキーワードを使いまわしたいみたいです。

- 手書きコードで中身が空っぽのメソッドを先に書く
- コード生成でメソッドの中身を埋める、埋めないとコンパイル エラー
- アクセス修飾子を付けたら(private であっても)この挙動になる
- この場合は `void` 以外の戻り値と、`out` 引数を持てる

という感じ。
`partial void M();` と `private partial void M();` で挙動が違うのがなかなか気持ち悪いですけども、
新しいキーワードを足すよりはこの方がマシだろいうという判断みたいです。

## トップレベルのステートメント

`Program.Main` を書かなくても、ステートメントを直接ファイル直下に書けるようにするという話。
今回の議題は、

- スクリプト方言みたいに式だけ書く(`;` を付けない式を書く)とそれが戻り値になるべきか
  - → やらない
- トップレベルに書いた変数などの名前はどう扱うべきか
  - プログラム全域がその名前のスコープになる(= 同名別変数を定義するとエラーになる)
  - かといって、今の仕様だとその変数を参照して使おうとするとエラーになる
  - 使えない変数はスコープから外すべき？
  - → 今は使ってないけども将来はわからないのでいったんこの仕様で行きたい
- コマンドライン引数の受け取り方
  - `void Main(string[] args)` みたいな部分が消えるので、コマンドライン引数の `args` はどう受け取ればいいか
  - 案1: 暗黙の変数 (プロパティの `set` 中の `value` みたいなもの)を用意する(名前はおそらく `args` 変数になる)
  - 案2: `Environment.Args` なりなんなり、何らかの API を用意する
  - → もうちょっと要検討。どっちの案にするかはともかく、こういう手段は必要
- [`await`](../../../../study/csharp/async/sp5_async.md#async) の扱い
  - 今の仕様
      - トップレベル ステートメント中に `await` が1つでもあれば、生成されるのは `Task Main()`
      - 1つもなければ、生成されるのは `void Main()`
  - これでいい？常に `Task Main()` で生成するとかしなくていい？
  - → とりあえず今の仕様で行く

## Records がらみ

### with 式の Clone

`with` 式ではオブジェクトの Clone → 特定のプロパティだけ書き換え みたいな処理になります。
で、このクローンをどうするか。

- 現状、ユーザー定義の Clone メソッドには対応していない
- 将来的にはユーザー定義で Clone の挙動を自由に変えれるようにしたい
- C# 9.0 時点では、将来の拡張性だけつぶさないように気を付けつつ、いったん現状の実装で進める

### Records: positional

[4/13の Design Notes での検討](../../4/pickuproslyn0419/index.md)によれば、
positional な Records (プライマリ コンストラクター)の優先度は低めになったんですが。
それでも、これに関していろいろ検討しているみたいです。

### プライマリ コンストラクター

- プライマリ コンストラクターは常に呼ばれないといけない
  - 手書きのコンストラクターがある場合、`Point () : this(0, 0)` みたいにプライマリ コンストラクター呼び出しが必要
- positional な Records にはコピー コンストラクターも自動生成したい
  - `Point(Point other) : this(other.X, other.Y) { }` みたいなプライマリ コンストラクター呼び出しをしたい
- プライマリ コンストラクターでは、
  - `class MyClass(int x, int y) { public int P => x + y; }` みたいにキャプチャが発生したときには自動的にフィールドを生成する
  - そうでないときは何も生成しない(単なるコンストラクターの引数扱い)
- positional な Records では、init-only なプロパティを生成する
- Records でない普通のクラスでプライマリ コンストラクターを認める場合と、positional な Records とで不整合は起きないか
  - → 生成されるものがフィールドか init-only プロパティかの差があるけど、それくらいは許容する

### プライマリ コンストラクター本体とバリデーター

プライマリ コンストラクターに対してコンストラクター本体を持ちたい場合、
以下のような構文になるみたいです。

<pre class="source" title="プライマリ コンストラクター本体とバリデーター">
<code><span class="reserved">class</span> <span class="type">TypeName</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y) <span class="comment">// プライマリ コンストラクター</span>
{
    <span class="comment">// 引数リストなしの型名 = プライマリ コンストラクターの本体</span>
    <span class="reserved">public</span> <span class="type">TypeName</span>
    {
        <span class="comment">// new TypeName(x, y) の時点で呼ばれる処理</span>
    }
 
    <span class="comment">// init キーワード = バリデーター</span>
    init
    {
        <span class="comment">// new TypeName { X = x, Y = y } みたいな、初期化子での初期化の後に呼ばれる</span>
    }
}
</code></pre>

元々はこの2つを区別していなかったものの、結局は両方必要そうで、両方を認めそう。

### プライマリ コンストラクターからの基底クラス コストラクター呼び出し

2種類の書き方ができるけども、どちらも認めてしまえとのこと。

<pre class="source" title="">
<code><span class="reserved">class</span> <span class="type">TypeName</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y) : <span class="type">BaseType</span>(X, Y);
 
<span class="reserved">class</span> <span class="type">TypeName</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y) : BaseType
{
    <span class="reserved">public</span> <span class="type">TypeName</span> : <span class="reserved">base</span>(X, Y) { }
}
</code></pre>

## 共変戻り値

要は以下のようなやつ。

<pre class="source" title="共変戻り値">
<code><span class="reserved">class</span> <span class="type">Compilation</span> ...
{
    <span class="reserved">virtual</span> <span class="type">Compilation</span> <span class="method">WithOptions</span>(Options <span class="variable">options</span>)...
}
<span class="reserved">class</span> <span class="type">CSharpCompilation</span> : <span class="type">Compilation</span>
{
    <span class="comment">// 戻り値の型が Compilation.WithOptions と違う</span>
    <span class="comment">// けど、派生型で共変なので問題ないはず。</span>
    <span class="reserved">override</span> <span class="type">CSharpCompilation</span> <span class="method">WithOptions</span>(Options <span class="variable">options</span>)...
}
</code></pre>

これも需要は非常に高いものの、
「.NET ランタイム側の修正が必要なので C# だけでできなくて重たい」みたいに言われ続けてたやつ。

.NET 5 で実装するみたいです。今まさに作業中:

- [Covariant Returns Feature](https://github.com/dotnet/runtime/pull/35308)

ということで、C# 的にも C# 9.0 で入ります。
ただ、 .NET Core 3.1 以前のバージョンでは動かないです。

## モジュール初期化子

モジュール(dll とか)が読み込まれたタイミングで1回だけ呼ばれる処理を書きたいという要望が以前からあります。

似たようなものとして、クラスの静的コンストラクターがあるんですが…

<pre class="source" title="静的コンストラクター">
<code><span class="reserved">class</span> <span class="type">Initializer</span>
{
    <span class="reserved">static</span> <span class="type">Initializer</span>()
    {
        <span class="comment">// 初めてこのクラスの何らかのメンバーを使おうとしたタイミングで呼ばれる</span>
    }
}
</code></pre>

こいつだと、このクラスに一切触れなかった場合には全く呼ばれませんし、
「初めて触ったとき」という読めないタイミングでの呼び出しになります。

なのでモジュール読み込み時に確定で呼ばれるメソッドを用意したいというのがモジュール初期化子(module initializer)です。

今のところ、属性 + 静的コンストラクターで実装したいみたいです。

<pre class="source" title="モジュール初期化子">
<code><span class="reserved">using</span> System.Runtime.CompilerServices;
 
[<span class="reserved">module</span>: <span class="type">ModuleInitializer</span>(<span class="reserved">typeof</span>(<span class="type">MyModuleInitializer</span>))]
 
<span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">class</span> <span class="type">MyModuleInitializer</span>
{
    <span class="reserved">static</span> <span class="type">MyModuleInitializer</span>()
    {
        <span class="comment">// モジュール読み込み時に実行される</span>
    }
}
</code></pre>
