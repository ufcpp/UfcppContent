---
title: "ピックアップRoslyn 10/31: csharplang の運営方針とかトリアージとか"
source_url: "https://ufcpp.net/blog/2020/10/pickuproslyn1030/"
content_type: "BlogEntry"
published_at: "2020-10-31T00:17:21"
updated_at: "2020-10-31T00:17:21"
tags: []
umbraco_id: 2315
parent_id: 2311
sort_order: 3
aliases: []
---

# ピックアップRoslyn 10/31: csharplang の運営方針とかトリアージとか

また何件かまとめて、C# Language Design Meeting 議事録を紹介。

- [October 12th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-12.md)
- [October 14th, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-14.md)
- [October 26st, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-26.md)

主に、csharplang の運営方針に関する話と、こまごまとトリアージ話。

(この他に、
[October 21st, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-10-21.md)ではプライマリ コンストラクターの話があったり、
Meeting 議事録とは別に[派生型の網羅性の話](https://github.com/dotnet/csharplang/issues/4032)が出てたりするんですが、
またちょっと話が大きくなりそうなので別の回で改めて。)

## csharplang の運営方針

### C# コミュニティ大使

先週の配信:

<div>
<iframe width="560" height="315" src="https://www.youtube.com/embed/aDXHl3S8oik" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
</div>

これの割と冒頭で話してるんですけども、[csharplang](https://github.com/dotnet/csharplang)で、Community Ambassador (コミュニティ大使)を設けようという話が出ていました。
(実際、ほぼ即日、何名か任命。)

C# はオープンソース開発されているといっても、マイクロソフトの C# チームが責任を負ってどの機能をいつまでに実装するかなどは独裁的に決定しています。
バグ修正の類などは [roslyn](https://github.com/dotnet/roslyn/) (コンパイラー実装に関するリポジトリ)に直接 Pull Request を出して通ったりしますが、
言語仕様に関しては [csharplang](https://github.com/dotnet/csharplang/) (言語仕様だけに絞ったリポジトリ)でのディスカッションを経て、
C# チームが承認したものだけが受け付けられます。

あるいは、C# チームが「微妙なラインなので、もし実装してくれる人がいるなら受け付けるけども、C# チーム内では実装しない」と判定した言語機能であれば外部貢献が受け付けられたりはします。
ただ、これも、「例え外部貢献だろうとリジェクト」と判定される言語機能もあって、その場合は誰かがその機能を実装したとしても Pull Request が承認されることはありません。

とはいえ、そういう運営体制だとしてコミュニティ(要するにマイクロソフト外の協力者)側でサポートできることがあるだろうというのが今回の流れです。

csharplang には、Design Meeting でこの話題が出た12日時点で1500件を超える issue が立っていたわけですが…

- そもそも GitHub に Discussions 機能が追加される前からあるもので、今であれば discussion にすべきものが大半
- ほとんどが重複
- 残る issue も具体的なメリットや、逆にそれを実装した場合のリスクなどの観点が抜けている

というような状態です。

これに対して、

- issue から discussion への移行
- disucussion の answered 判定
- 重複 issue の close
- 具体性のある提案ドキュメント化(に向けた誘導)

などは、最終決定権を持たないコミュニティ メンバーでも可能なわけです。
そこで、csharplang 内で特にアクティブに活動していて、その辺りの整理作業を任せられそうな信頼のおける数名に「大使」として issue/discussion の編集権限を与えることになったそうです。

そして効果のほどなんですが、その後3週間弱となる現在、csharplang の issue はついに900ほどまで減っています。
ほぼ1日30個くらいの一定ペースで減少中。

### マイルストーンの整理

前節の通り、C# の言語機能に関しては C# チームの決定権が絶対的なんですが、
その決定結果は「やる/やらない」の2択ではなくて、以下のように積極度に段階があります。

- Working Set: やりたいし、C# チーム自身が活発に手がける
- Backlog: やりたいけども、ちょっと決め手に欠けていて、言語設計のレベルで何かいいアイディアが欲しい(いきなりコミュニティ実装を受け付けられるというレベルでもない)
- Any Time: やってもいいけども、優先度低めで C# チームのリソースを割けない(コミュニティ実装は受け付けられる)
- Likely Never: よっぽどのことがないとやらない

今まで 9.0 とか 10.0 とかの C# のバージョンをそのままマイルストーンにしていましたが、
「活発に手掛けているからと言っても短期間で完成するものではなくて数バージョン先になる」みたいな機能もあれば、
「そんなに優先度が高くなかったけども、コミュニティ貢献の質が良くて採用」みたいな機能もあるので、
取り組みの活発さとマイルストーンの温度感に差がありました。

ということで、Working Set と Backlog マイルストーンを新設したとのこと
(Any Time と Likely Never は元からあったものの、ここで改めて意図を明文化)。

これからはまず Working Set か Backlog かに分類された上で、
具体的に実装が進んできてリリースに含められそうかどうかが見えてきてから初めて C# バージョン番号を冠したマイルストーンに移動という流れになりそうです。
また、バージョン番号も実際にリリースされる(マイルストーンが close される)までは、「予定ではそのバージョンで入れるけども、重大な問題が発覚したらそのバージョンからは外すこともある」みたいな状態です。

## トリアージ

いくつか抜粋。

### Repeated Attributes in Partial Members

C# 9.0 で追加する新しい partial method ですが、以下のようなコードを書くとコンパイル エラーを起こします。

<pre class="source" title="AllowMultiple = false な属性">
<code><span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    [<span class="reserved">return</span>: <span class="type">MaybeNull</span>]
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">M</span>();
}
 
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">C</span>
{
    [<span class="reserved">return</span>: <span class="type">MaybeNull</span>]
    <span class="reserved">public</span> <span class="reserved">partial</span> <span class="reserved">string</span> <span class="method">M</span>() =&gt; <span class="string">&quot;&quot;</span>;
}
</code></pre>

`AttributeUsage` で重複不可(`AllowMultiple = false`) になっている属性が partial の宣言側と実装側の両方についている場合、「重複」判定を受けてしまっているという状態。
この例のように null 許容関連の属性は宣言側と実装側の両方に付けたいことが結構あって、これをエラーにされると結構困りそうです。
ということで、重複不可になっている属性でも、全く同じパラメーターで指定されている場合は両側に同じ属性が付いている状態を認めたいとのこと。
working set 判定。

### params Span

現状、[可変長引数](../../../../study/csharp/structured/sp_params.md#params)は配列が作られてしまうのでアロケーションが発生します。
これを、`Span<T>` で受け付けられるようにしてアロケーションをなくしたいという話は前々からあります。

[11日に書いた低水準機能改善](../pickuproslynlowlevel10/index.md)の一環として「safe な固定長バッファー」みたいな話もあって、
これがあればそんなに難なく `params Span<T>` ができるはずなので、今このタイミングで working set 判定。

### Sequence Expressions

`(var x = Read(); x * x)` みたいな書き方で、複数のステートメントを並べつつ、最後の値を返す「[式](../../../../study/csharp/structured/miscexpressions.md#key-expression)」にできる文法が欲しいという話。

[初期化子](../../../../study/csharp/oop/oo_construct.md#member_initializer)とか [`switch` 式](../../../../study/csharp/datatype/typeswitch.md#switch-expression)とか、
式しか受け付けない便利な文法が結構あります。

これと関連しそうないくつかの提案 ([3038](https://github.com/dotnet/csharplang/issues/3038)、[3037](https://github.com/dotnet/csharplang/issues/3037)、[3086](https://github.com/dotnet/csharplang/issues/3086))がすでに working set 判定済みなので、この Sequence Expressions も working set 入り。

### utf8 string literals

C# に UTF-8 関連の特殊対応文法を入れるよりもまず 「`Utf8String` 型の実装](https://github.com/dotnet/corefxlab/issues/2350)が先だろという話でして。

そっちは [nightly ビルドの NuGet パッケージを入れて試してみてフィードバックが欲しい](https://github.com/dotnet/corefxlab/issues/2350#issuecomment-718917142)らしいですよ。

ということで、C# 側としては Backlog 判定だそうです。

### File scoped namespaces

今までの、

<pre class="source" title="名前空間">
<code><span class="reserved">namespace</span> N
{
    <span class="reserved">class</span> <span class="type">C</span>
    {
    }
}
</code></pre>

これを、

<pre class="source" title="1ライン名前空間">
<code><span class="reserved">namespace</span> N;
 
<span class="reserved">class</span> <span class="type">C</span>
{
}
</code></pre>

こうじゃ。
(1ライン名前空間の導入。)

最近の C# は[トップ レベル ステートメント](../../../../study/csharp/misc/miscentrypoint.md#top-level-statements)しかり、「見栄えがすっきりすること」に割と前向きです。

もちろん、「主張が弱くなりすぎていて、書くのはいいとしても、読むときには逆に見逃してしまうリスクが高くて読みづらい」みたいなこともあります
(プログラムは書いている時間よりも読んでいる時間の方が長いので、「読みづらい」というのは思った以上のデメリット)。
とはいえ、この辺りは慣れの問題もあって、すっきり書けるプログラミング言語が増えてきた昨今、「読みづらい」判定の基準もずいぶん変わってきたと思います。

また、C# は基本的には空白文字の有無によって挙動を変えない言語なので、
その発想でいうと「インデント1段の差は些細な差」ではあります。
例えばまあ、別に、1ライン名前空間がなくても、以下のように書けばほぼ同じ見た目になります。

<pre class="source" title="名前空間">
<code><span class="reserved">namespace</span> N
{
<span class="reserved">class</span> <span class="type">C</span>
{
}
}
</code></pre>

ただ、最近は

- Visual Studio に限らずツールでの自動整形が当たり前
- 改行位置とか空白の数まで含めて、スタイルまできっちり決めて、スタイル違反を警告にすることもざら
- 何だったら CI テストでスタイル違反は merge できないようにはじく人までいる

みたいな状態なので、「挙動が変わらないはずの言語でも空白の有無が結構大きい」みたいな感じになっています。

ということで C# でもついに1ライン名前空間に前向きな判定が出ていて、working set 入り(というか、すでに 10.0 (バージョン番号マイルストーン)入り)。

### Efficient params and string formatting

[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)は便利な構文なんですけども、
アロケーションが掛かっちゃうタイミングが早すぎてロギングとかの用途では使えなかったりします。
(例えば、ログ レベルによってはログ書き出ししない場合が大部分になるのに、書き出しもしない文字列のアロケーションをしまくってしまう。)

[`ILogger.Log` メソッド](https://docs.microsoft.com/ja-jp/dotnet/api/microsoft.extensions.logging.ilogger.log?view=dotnet-plat-ext-3.1#Microsoft_Extensions_Logging_ILogger_Log__1_Microsoft_Extensions_Logging_LogLevel_Microsoft_Extensions_Logging_EventId___0_System_Exception_System_Func___0_System_Exception_System_String__)が `formatter` 引数を持つ長ったらしくて使いにくそうなシグネチャになってるのもそのせいでして。

前述の `params Span<T>` とかを使ったりでアロケーションを減らそう見たいな提案はいくつか出ているんですが、今あげたロギングの例とかを考えると多分不十分じゃないかなと思います。

ということで「working set としてキープし続ける。都度色々な提案を見ていく」みたいな空気感。

### readonly classes and records

[readonly struct](../../../../study/csharp/cheatsheet/ap_ver7_2.md#readonly-struct)の参照型版。
class, record に対しても readonly 修飾(フィールド全部が readonly であることを求める)を付けたいという話。

今までやってない理由は大体以下の2点。

- あくまで shallow な read-only 性しか担保していなくて、階層すべての immutability を保証できない
- 構造体ほど切羽詰まってない(構造体の場合は[隠れたコピー](../../../../study/csharp/resource/readonlyness.md#hidden-copy)発生問題がある)

とはいえ、あって困るものではなく、working set 入り。

### Target typed anonymous type initializers

以下のような話。

<pre class="source" title="() なし名前指定ターゲット型推論">
<code><span class="comment">// C# 9.0 で、ターゲットからの型推論で new() と書けるように</span>
<span class="type">Point</span> <span class="variable">a</span> = <span class="reserved">new</span>(1, 2);
 
<span class="comment">// 逆に(3.0 からある)ソース型推論だとこうなる</span>
<span class="reserved">var</span> <span class="variable">b</span> = <span class="reserved">new</span> <span class="type">Point</span>(1, 2);
<span class="reserved">var</span> <span class="variable">c</span> = <span class="reserved">new</span> <span class="type">Point</span> { X = 1, Y = 2 };
 
<span class="comment">// ターゲット型推論で nominal (プロパティ名指定)な初期化をするならこうなる。</span>
<span class="type">Point</span> <span class="variable">d</span> = <span class="reserved">new</span>() { X = 1, Y = 2 };
 
<span class="comment">// ↑ この () は邪魔じゃない？</span>
<span class="comment">// とはいえ…</span>
<span class="comment">// これは「匿名型」になる。</span>
<span class="reserved">var</span> <span class="variable">e</span> = <span class="reserved">new</span> { X = 1, Y = 2 };
 
<span class="comment">// ターゲット型があるときはターゲット型推論扱い</span>
<span class="comment">// (ターゲットが object とか dynamic、var の時だけ匿名型扱いするような分岐)</span>
<span class="comment">// も可能なんじゃないか？</span>
<span class="comment">// (C# 9.0 時点ではエラー。たぶん、破壊的変更にはならず上記挙動が可能)</span>
<span class="type">Point</span> <span class="variable">f</span> = <span class="reserved">new</span> { X = 1, Y = 2 };
 
<span class="reserved">record</span> Point(<span class="reserved">int</span> <span class="variable">X</span> = 0, <span class="reserved">int</span> <span class="variable">Y</span> = 0);
</code></pre>

`new() {}` の `()` が邪魔というか、ソース型推論の場合との整合性があんまりよくなくていまいちなのは確かだったり。

その一方で、[匿名型](../../../../study/csharp/start/sp3_inference.md#anonymous)と混ざる怖さを払しょくできるほど欲しい構文かと言われると微妙で。
Backlog 入り。

### Static local functions in base calls

以下のように、初期化子でローカル関数を呼びたいという話が出ていまして。

<pre class="source" title="初期化子でローカル関数を呼びたい">
<code><span class="reserved">class</span> <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">Base</span>(<span class="reserved">int</span> <span class="variable">x</span>) { }
}
 
<span class="reserved">class</span> <span class="type">Derive</span> : <span class="type">Base</span>
{
    <span class="reserved">public</span> <span class="type">C</span>() : <span class="reserved">base</span>(init())
    {
        <span class="reserved">static</span> <span class="reserved">int</span> <span class="method">init</span>()
        {
            <span class="comment">// 何かそれなりの処理</span>
        }
    }
}
</code></pre>

まあ割と「わからなくはない」という感じで Working set 入りしてるんですが。
ただ、C# チーム的にはもうちょっと汎用的に、「スコープの拡張」みたいなのを考えているみたいです。

例えば、

- 上記のようにローカル関数のスコープを広げるのであれば、メソッド内で定義した [`const`](../../../../study/csharp/start/sp_const.md#const) も同様に使いたい場面はある
- 初期化子だけじゃなくて、属性とかからも参照したい
  - `[return: NotNullIfNotNull(nameof(p))] string? M(string? p)` みたいなのとか、実際、スコープを広げたい要求は出てきてる
