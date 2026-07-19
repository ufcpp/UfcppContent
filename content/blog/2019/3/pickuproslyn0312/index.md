---
title: "ピックアップ Roslyn 3/12"
source_url: "https://ufcpp.net/blog/2019/3/pickuproslyn0312/"
content_type: "BlogEntry"
published_at: "2019-03-12T22:28:03"
updated_at: "2019-03-12T22:35:16"
tags: []
umbraco_id: 2235
parent_id: 2233
sort_order: 1
aliases: []
---

# ピックアップ Roslyn 3/12

Visual Studio 2019 (16.0)が RC までいってちょっと落ち着いたのか、csharplang にちょっと動きが。
(もう、次に C# 8.0 絡みの新機能実装がマージされるとしたら 16.1 になるので、C# チーム的には今ちょっと落ち着ける時期のはず。)
[Designe Notes 3件](https://github.com/dotnet/csharplang/issues/2326)追加。

- [Feb 25th, 2019](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-02-25.md)
- [Feb. 27, 2019](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-02-27.md)
- [March 4th, 2019](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-03-04.md)

いくつかの話題はすでに個別の issue が立っています。

## インターフェイスのデフォルト実装

半分くらいはインターフェイスのデフォルト実装がらみ。
`base` 呼び出しをどうしようかという話と、アクセシビリティをどうしようかという話。

### base

`base` 呼び出しって言うのは以下のようなやつのこと。

<pre class="source" title="base 呼び出し">
<code><span class="reserved">using</span> System;
 
<span class="reserved">class</span> <span class="type">A</span>
{
    <span class="reserved">protected</span> <span class="reserved">virtual</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="string">&quot;A&quot;</span>);
}
 
<span class="reserved">class</span> <span class="type">B</span> : <span class="type">A</span>
{
    <span class="comment">// この、B の M は後から足したり消したり</span>
    <span class="comment">//protected override void M() =&gt; Console.WriteLine(&quot;B&quot;);</span>
}
 
<span class="comment">// A, B とは別アセンブリにあるとして</span>
<span class="reserved">class</span> <span class="type">C</span> : <span class="type">B</span>
{
    <span class="comment">// C から基底クラスの M() を呼ぶ</span>
    <span class="reserved">protected</span> <span class="reserved">override</span> <span class="reserved">void</span> <span class="method">M</span>() =&gt; <span class="reserved">base</span>.<span class="method">M</span>();
}
</code></pre>

この書き方で、`C.M` から基底クラスの `M` を呼び出せるわけですが、

- 基底クラスを1つずつたどっていって、最初に見つかったやつが呼ばれる
  - コンパイル時に `B.M` があったけど、実行時に読み込んだものからは消えていたら `A.M` が呼ばれる
  - コンパイル時に `B.M` はなかったけど、実行時に読み込んだものには足されていたら `B.M` が呼ばれる

みたいな挙動です。
これは C# の仕様というか、 .NET ランタイム([IL](../../../../study/il/index.md) 命令)のレベルでそういう仕様だそうです。

で、インターフェイスの場合はダイアモンド継承があり得るので、この仕様便りだと、規定をたどっていく経路が複数あって困る。
なので、`base(B).M()` みたいに、具体的にどの型の `M` を呼びたいのかを明示できる構文が導入される予定です。これについて、

- この書き方、(C# 8.0 でこれから実装される)インターフェイスのデフォルト実装だけじゃなく、クラスに対しても認める
- フィールドだろうがなんだろうが、この `base(BaseClass).Member` みたいな書き方が使える
  - ただし、(overrideさえなければ)元々 `this.Member` でアクセスできるものに限る
- `base(B).M()` と書いたら `B` だけを見る。実行時に消えて時たら実行時エラーを起こす
  - 「`B` になければ `B` から上をたどって探す」みたいなのは現状の .NET ランタイムでは不可能
  - 将来的に、 .NET ランタイム自体に改修を入れる余地はある

とのこと。

### アクセシビリティ

[アクセシビリティ](../../../../study/csharp/oop/oo_conceal.md#level)は `public` とか `private` とかのこと。

C# 7.3 までのインターフェイスは無条件に全部のメンバーが `public` (明示的に指定はできない)でしたが、
デフォルト実装とともに、アクセシビリティの指定ができるようになります。
これまでも、`public`、`protected`、`private` は提供するつもりでしたが、
残りの `internal`、`protected internal`、`private protected` も提供することに決めたそうです。

あと、インターフェイスの[明示的実装](../../../../study/csharp/oop/oo_interface.md#explicit-impl)、↓みたいなやつもあるわけですが。

<pre class="source" title="インターフェイスの明示的実装">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">M</span>();
}
 
<span class="reserved">class</span> <span class="type">A</span> : <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="type">I</span>.<span class="method">M</span>() { }
}
</code></pre>

デフォルト実装が入ることで、「インターフェイスが基底のメンバーを明示的実装」という状況が発生します。
この場合、その明示的実装は `protected` 扱いにするそうです。
(前節の「`base` は基底をたどって最初に見つかったものを呼ぶ」挙動との兼ね合いだそうです。)

## null 許容参照型

### 値型の default

null 許容“参照型”と言っているわけですから、名前通り、参照型に関する機能です。
でも、じゃあ、クラスとか参照型を含む構造体が絡んだとき、`default(T)` はどうするんだという問題が残ります。
(`default` [既定値](../../../../study/csharp/resource/rm_default.md)は 0/null での初期化になります。nullが絡む。)

なんか今のところ、`var y = default(参照型をフィールドとして持つ構造体)` みたいなのに対する警告は出さないみたいです。
(もちろん、「null を認めてないつもりのものに null が混ざる」という落とし穴でしかないので、相当な妥協。)

ただ、C# 8.0 では無理としても、構造体の “defaultability” については今後取り組みたい姿勢はある模様。
3年前(roslyn リポジトリ側に文法に関する提案も混ざってた頃含む)に自分が書いた「`default` を認めない値型を作らせてくれ」という issue:

- [Championed: Non-defaultable value types #146](https://github.com/dotnet/csharplang/issues/146)

が急に championed (C# チームの誰かが興味を持って取り組む)状態に変わりました。
ある意味こいつは「値型版の null 許容参照型」です。

### 利用調査

null 許容参照型のフロー解析をオンにしてどうなるか、
それなりの規模なライブラリを調べてみたみたいです(作者に直接聞いたのか、クローンしてきて自分たちでやってみたのか、ブログとかを見ただけなのかとかはわからず)。
[Telegram bot](https://www.nuget.org/packages/Telegram.Bot.Framework/)とか[Jil](https://www.nuget.org/packages/Jil/)とか。
そこで起きてた問題のまとめ。

- メンバー定義で困ることが多い。メソッド内部での問題はむしろ少ない
- インターフェイス側の定義を変えたときの、実装を全部変えて回る作業がやばい
- [初期化子](../../../../study/csharp/oop/oo_construct.md#member_initializer)での初期化を前提としているものに対して警告が消せない
- コンストラクター連鎖 (`A() : this(0) { }` みたいなやつ)もフロー解析しきれてない
- null 許容値型(既存の、値型に対する `?`) が null 許容参照型との挙動差でよく問題起こす
- 診断のクオリティがまだまだ。ジェネリックな型でよく混乱するし、特にタプルに対してつらい
- 自動 code fix 機能欲しい

## プロパティの get/set に Obsolete

- [Champion: "Allow Obsolete attribute on getters and setters" #2152](https://github.com/dotnet/csharplang/issues/2152)

プロパティの get/set [アクセサー](../../../../study/csharp/oop/oo_property.md#accessor)には、それぞれ属性が付けれます(メソッド扱い。`AttributeTargets.Method` が入ってる属性だけ)。
でも、`Obsolete`、`Conditional`、`CLSComliant` の3つは get/set に付けることは禁止されていました。

で、まあ、`Obsolete`だけは認めてもいいんじゃないかという話に。
(あと、Xamarin iOS のやつかな、たぶん、`Deprecated` 属性も。)
`Conditional`、`CLSComliant` は今後もノータッチとのこと。

## params と文字列補間の効率

- [Efficient Params and String Formatting #2302](https://github.com/dotnet/csharplang/issues/2302)

提案ドキュメントの背景説明に「MSBuildログ最小限にしてもstringで236MBメモリ食っててその半分がFormatがらみ」とか書かれてて、あっ、はい…そうですよね…

文字列の整形絡みは一時的な小さい文字列インスタンスが大量にできちゃって、結構遅かったりします。
なので、自分も結構仕事で、`string.Format` とかを避けて、`stackalloc` したりプールした `char[]` とかを使って自前で文字列整形することがあったり。

あと、[`params`](../../../../study/csharp/structured/sp_params.md) が必ず配列を `new` しちゃうのも、`string.Format` を重たくしている原因。

ということで、corefxlab の方で文字列整形をアロケーションなしでできないか、みたいな実験コードが出ていまして。

- [Protoype for nonallocating string formatting #2595](https://github.com/dotnet/corefxlab/pull/2595)

C# 側で対応しないといけないこともあるので Design Meeting でも議題に。
概ね、

- `params` に `Span<T>` を認めて、スタックに値を置いて可変長引数呼び出ししたい
- 値型の[ボックス化](../../../../study/csharp/resource/rmboxing.md)避けたいから `Variant` 型作るか

みたいな話。

まあでも、付いたコメント的には

- なんで [`stackalloc`](../../../../study/csharp/resource/span.md#safe-stackalloc) に参照型使えないの？ → それを認めようとすると[ガベコレ](../../../../study/computer/essential-software/memorymanagement.md#garbage-collection)のコードがだいぶ複雑になる(パフォーマンスにも悪影響)
- `Span<TypedReference>` を認められるようにしようよ

みたいな感じ。

## switch 式の優先度

今回の Designe Notes にはないんですけど、もう1個、割と最近立った提案 issue:

- [Propose to change precedence of switch expression from relational to primary #2331](https://github.com/dotnet/csharplang/issues/2331)

現時点 (Visual Studio 2019 RC でのことなので、たぶん、RC が外れても)での [`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)の結合優先度は関係演算子(`==` とか)と同じだそうです。
関係演算子って結構優先度が低くいですし、
`&` よりは上で `+` よりは下みたいな位置です。

- `x switch { ... } + 1` みたいなものが、`} + 1` の方を先に見ちゃってエラーに
- `a + b switch {...}` は `+` が先だけど、`a & b switch {...}` は `switch` が先

とかいう嫌な状態。

なので、プライマリな優先度(`x.M()` の `.` とか、`[]` とかと同じ)に変えようかという提案。

大体、`x switch { ... }` が後置き演算子みたいな見た目ですからね。
`[]` とか `++` とか `.` とか、後ろに置くものは大体がプライマリ。
それに合わせた方が自然だろうという意図もあり。

## Index/Range の実装再考

もう1つは C# 側じゃなくて、corefx 側から出ている要望。

- [Revisit Index/Range API requirements #35972](https://github.com/dotnet/corefx/issues/35972)

今現在の仕様だと、C# の姿勢としては、

- `^a` とか とか `a..b` とかの構文はそれぞれ `Index`/`Range` 構造体を作るだけ
- `x[a..b]` みたいなのを使いたければ、コレクション クラスの実装側に `Index`/`Range` を受け付けるオーバーロードを増やせ

です。

これに対して、要するに、

- インデクサーを持つコレクション クラス全部に対してオーバーロードを増やして回るのは大変だし、パフォーマンスが出ない実装になることがある
- それよりは、`collection[i.GetOffset(collection.Count)]` みたいな、`int` のオーバーロードを使うコードに展開する実装に変えてほしい

という流れ。
まあ、そりゃ、コレクション作ってる側からするとそうですよね。
corefx (中の人同士)ですらそうなんだから、ましてコミュニティ実装なコレクションだとなおのこと。
