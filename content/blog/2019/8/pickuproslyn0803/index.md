---
title: "ピックアップRoslyn 8/3: Records など"
source_url: "https://ufcpp.net/blog/2019/8/pickuproslyn0803/"
content_type: "BlogEntry"
published_at: "2019-08-03T19:16:49"
updated_at: "2019-08-03T19:21:53"
tags: []
umbraco_id: 2260
parent_id: 2259
sort_order: 0
aliases: []
---

# ピックアップRoslyn 8/3: Records など

Design Notes が3件ほど。

そのうち2件は7/10, 17のもので、C# 8.0 の最後の詰めっぽい感じ。

- [Added: LDM notes for July 10th and July 17th #2702](https://github.com/dotnet/csharplang/issues/2702)
  - [7/10](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-10.md)
  - [7/17](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-17.md)

残りは、7/22 のもので、C# 9.0 に向けて「今度こそ」という感じで Records の話。

- [Discussion: New records proposal #2699](https://github.com/dotnet/csharplang/issues/2699)
  - [7/22](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-22.md)
  - [Records v2](https://github.com/dotnet/csharplang/blob/856c335cc584eda2178f0604cc845ef200d89f97/proposals/recordsv2.md)

## 7/10

- [7/10](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-10.md)

議題は3点。

- Empty switch statement
- `[DoesNotReturn]`
- `params!`

### Empty switch statement

元々、[switch](../../../../study/csharp/datatype/typeswitch.md#switch-expression)式を式ステートメント(式1個だけ + `;` でステートメントを作るやつ)で使えるようにしたいという話があります。
セットで、戻り値が `void` な式を `switch` 式使えるようにしたいという話もあり。

<pre class="source" title="switch 式を式ステートメント化け &amp; void を認める">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">bool</span> <span class="variable">flag</span>)
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">a</span>() { }
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">b</span>() { }
 
    <span class="comment">// switch 式内で void なものを書けるようしたいという話あり。</span>
    <span class="comment">// (今 (少なくとも VS 16.3 Preview 1)は認められていない。)</span>
    <span class="variable">flag</span> <span class="control">switch</span>
    {
        <span class="reserved">true</span> =&gt; <span class="method">a</span>(),
        <span class="reserved">false</span> =&gt; <span class="method">b</span>(),
    };
}
</code></pre>

(ちなみに、これの実装はまだなく、[関連 issue](https://github.com/dotnet/roslyn/issues/30649)を今見ると[Compiler.Next](https://github.com/dotnet/roslyn/milestone/44)という謎のマイルストーンが付けられてた…
C# 8.0 を目指してたけどスケジュール的に無理で次に回ったやつですね、たぶん。)

で、7/10 の議題的には、以下のような「空 switch 式」を認めるかどうか。

<pre class="source" title="空 switch 式">
<code><span class="comment">// 式ステートメントに出来る前提では、空 switch を禁止する十分な理由が見当たらない。</span>
<span class="variable">flag</span> <span class="control">switch</span> { };
</code></pre>

意味のあるコードではないですけども、
まあ、わざわざ禁止する十分な理由が見当たらないとのこと。

### DoesNotReturn

`[DoesNotReturn]` 属性は、[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)に関連する属性です。
以下のメソッドのように、そのメソッドを呼んだ時点でそこから後ろは絶対に呼ばれないということを表すもの。

<pre class="source" title="">
<code><span class="comment">// 例外を出すんで、このメソッドからは絶対に正常に戻ってこない。</span>
[<span class="type">DoesNotReturn</span>]
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Throw</span>() =&gt; <span class="control">throw</span> <span class="reserved">new</span> <span class="type">Exception</span>();
 
<span class="comment">// 永久ループしてるんで、このメソッドからも戻ってこない。</span>
[<span class="type">DoesNotReturn</span>]
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">InfiniteLoop</span>() { <span class="control">while</span> (<span class="reserved">true</span>) ; }
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">int</span> <span class="variable">i</span>)
{
    <span class="reserved">string</span>? <span class="variable">s</span> = <span class="reserved">null</span>;
 
    <span class="comment">// 絶対に戻ってこない or 非 null な値の代入ありのどちらか。</span>
    <span class="control">if</span> (<span class="variable">i</span> == 1) <span class="method">Throw</span>();
    <span class="control">else</span> <span class="control">if</span>(<span class="variable">i</span> == 2) <span class="method">InfiniteLoop</span>();
    <span class="control">else</span> { <span class="variable">s</span> = <span class="string">&quot;abc&quot;</span>; }
 
    <span class="comment">// ここに来た時点で絶対に s = &quot;abc&quot; を通ってるので、s は非 null。</span>
    <span class="comment">// 警告は出さなくていいはず。</span>
    <span class="type">Console</span>.<span class="method">WriteLine</span>(<span class="variable">s</span>.Length);
}
</code></pre>

null 許容参照型のために導入される属性ですが、
原理的には[確実な初期化ルール](../../../../study/csharp/resource/rm_struct.md#definite-assignment)に対しても使えるはずです。
7/10 の議題はこの点についてで、とりあえずこの属性は null チェックにしか使わないという決断。

汎用な reachability (到達可能かどうか。到達可能なら null チェック、確実な初期化チェックが必要)判定は将来改めて考える。
その際にはおそらく別の仕組みを使うとのこと。

### param!

引数の後ろに `!` を書くことで、その引数の実行時 null チェックを自動挿入したいという提案があります。

- [Champion: Simplified parameter null validation code #2145](https://github.com/dotnet/csharplang/issues/2145)

ちなみに、[null 許容参照型](../../../../study/csharp/resource/nullablereferencetype.md)はコンパイル時の静的なフロー解析で、実行時には何もしません。
静的にチェックしても、null 許容参照型導入前の古いコードや、[`#nullable disable` なコンテキスト](../../../../study/csharp/resource/nullablereferencetype.md#opt-in)で書いたコード、unsafe なコードから実行時に null が紛れ込むことがあります。
なので、実行時 null チェックの挿入にも需要が残っています。

機能自体はぜひ採用したいものの、詳細を詰め切れていない(いくつか問題がある)ので C# 8.0 には入れないとのこと。

例えば現状の文法案(引数の後ろに `!`) は、「式の後ろの `!`」と期待するものが真逆になるのでまずいです。これに対して、適切な文法を考えている余裕はもう C# 8.0 のスケジュールにはありません。

<pre class="source" title="param! と expression!">
<code><span class="reserved">static</span> <span class="reserved">void</span> <span class="method">M</span>(<span class="reserved">string</span> <span class="variable">param</span>!)
{
    <span class="comment">// (C# 8.0 に入れないことが決まった。)</span>
    <span class="comment">// param! と書くと、以下のコードがコンパイラーによって挿入される</span>
    <span class="comment">// if (param is null) throw new ArgumentNullException(nameof(param));</span>
 
    <span class="comment">// つまり、暗に、本来非 null なところに null が来うることを期待してる。</span>
}
 
<span class="reserved">static</span> <span class="reserved">void</span> <span class="method">N</span>(<span class="reserved">string</span>? <span class="variable">nullable</span>)
{
    <span class="comment">// (これは C# 8.0 に入る。)</span>
    <span class="comment">// null が来てても完全に無視。</span>
    <span class="comment">// null forgiven (null の罪に目をつむる) 演算子って言ったりする。</span>
    <span class="reserved">string</span> <span class="variable">notnull</span> = <span class="variable">nullable</span>!;
 
    <span class="comment">// つまり、暗に、本来 null 許容なところに null が来ないことを期待してる。</span>
}
</code></pre>

## 7/17

- [7/17](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-17.md)

こっちの議題は2つ。と言っても片方は triage (細々と3つ、機能を入れるかどうか検討)。

- Nullability of events
- Triage
  - Support XML doc comments on local functions
  - Warn on obsoleting overrides of non-obsolete members
  - Proposals for ConfigureAwait with context

### Nullability of events

C# のイベント、特に[自動イベント](../../../../study/csharp/functional/sp_event.md#auto-event)は2つの側面を持ちます。

- 外から見て `+=`/`-=` できる (add/remove アクセサー)
- 中から見てデリゲート呼び出しができる(同名のフィールドとして参照できる)

これに対して null チェックをどうするかという話。
例えば、以下のようなチェックの仕方をしてほしいという要求は十分にあります。

- 外から見て、`+=`/`-=` に null を渡すとかは許容したくない。外から見ると非 null であってほしい
- 中からの呼び出しに関しては、null が来ることを前提としたコードを書くことが一般的
  - イベント `E` に対して `E?.Invoke(this, args)` とか (null 許容な前提だから `?.` を書く)

ということで、自動イベントに対するフロー解析で、アクセサーとフィールドを別扱いするべきかどうかというのが議題に。

結論的には、要求があることはわかるものの、イベントだけを特別扱いするのも混乱のもとなので、変なことはしないとのこと。

### Triage 3件

- ローカル関数に XML ドキュメント コメントを付けたい
  - 需要はわかるし、やりたい
  - でも、それを言ったらローカル関数だけじゃなくてローカルなもの全般(変数含む)にもドキュメント コメントの需要あって、合わせて考えたい
- [Obsolete](../../../../study/csharp/dynamic/sp_attribute.md#compiler_attribute)でないメンバーを、Obsolete 付きで[オーバーライド](../../../../study/csharp/oop/oo_polymorphism.md#override)したときに警告を出すかどうか
  - あると便利
  - でも、やるなら warning waves (既存コードを壊すような警告を足せるように、オプション指定で警告度合いを増やす)を入れる段階でやる
- ConfigureAwait 問題
  - 再三にわたって「[何もつけなくても `ConfigureAwait(false)` になるような機能](../../../2016/12/tipscontextfreetask/index.md)が欲しい」と言われる。いい加減検討が必要
  - 次のメジャーリリースに向けて考える

## 7/22 (record V2)

- [7/22](https://github.com/dotnet/csharplang/blob/master/meetings/2019/LDM-2019-07-22.md)

### records おさらい

records は、要は純粋なデータを表すような型のこと。
現状の C# で書くと、コンストラクター引数をプロパティでほぼ同じものを何度も繰り返し書かないといけなくてしんどいやつです。

<pre class="source" title="records">
<code><span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="comment">// public なプロパティでデータをまとめたいというのがこの手の型(レコード)の主目的。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
 
    <span class="comment">// 目的外のところで、存外書かないといけないコードが多い。</span>
    <span class="reserved">public</span> <span class="type">Records</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) = (X, Y);
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type">Records</span> <span class="variable">other</span>) =&gt; (X, Y) == (<span class="variable">other</span>.X, <span class="variable">other</span>.Y);
    <span class="comment">// その他、GetHashCode, == と !=, Equals(object) 等々…</span>
}
</code></pre>

うちのブログでも何度も何度も出ている話なのでずっと読んでくれている方ならわかると思いますが、初出は C# 6.0 の頃で、7 でも 8 でも流れて、9.0 で今度こそこれを主役にしたいという雰囲気。

### records V1

最初に提案された records (今回、records V1 とか positional records とか呼ばれるようになっています)は概ね、以下のような書き方から、上記のようなクラスを生成する機能です。

<pre class="source" title="Records V1">
<code>data <span class="reserved">class</span> <span class="type">Records</span>(<span class="reserved">int</span> X, <span class="reserved">int</span> Y);
</code></pre>

クラスの生成は、前述のような immutable (プロパティが get-only で、コンストラクターでの初期化が必須)なものになる(少なくともデフォルトではそうなる。get/set できるようにしたければ明示が必要にする)予定です。

こういう書き方自体はなくなったわけじゃなくて、これはこれで C# 9.0 のスコープに入っているんですが、問題もあって「V2」という別の書き方が提案されました。

問題は、V1 だとプロパティ初期化子が使えないこと。

<pre class="source" title="Records に対して初期化子を使いたい">
<code><span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Records</span>(<span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (X, Y) = (<span class="variable">x</span>, <span class="variable">y</span>);
    <span class="comment">// 以下略</span>
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// こうは書ける。</span>
        <span class="reserved">var</span> <span class="variable">r1</span> = <span class="reserved">new</span> <span class="type">Records</span>(1, 2);
 
        <span class="comment">// こう書きたいけど、これが V1 だと無理。</span>
        <span class="reserved">var</span> <span class="variable">r2</span> = <span class="reserved">new</span> <span class="type">Records</span> { X = 1, Y = 2 };
 
        <span class="comment">// こんな感じで「部分書き換え」もしたい。</span>
        <span class="comment">// X は r2.X を引き継ぎつつ、Y だけ書き換えた新しいインスタンスを作りたい。</span>
        <span class="reserved">var</span> <span class="variable">r3</span> = <span class="variable">r2</span> with { Y = 3};
    }
}
</code></pre>

### records V2

そこで今回提案されているのが records V2 (nominal records) で、
以下のように、「initonly」なプロパティを定義できるようにするのはどうかというものです。

<pre class="source" title="initonly プロパティ">
<code><span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="reserved">public</span> initonly <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> initonly <span class="reserved">int</span> Y { <span class="reserved">get</span>; }
}
 
<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> <span class="method">Main</span>()
    {
        <span class="comment">// こう書けるようにする。</span>
        <span class="reserved">var</span> <span class="variable">r2</span> = <span class="reserved">new</span> <span class="type">Records</span> { X = 1, Y = 2 };
        <span class="reserved">var</span> <span class="variable">r3</span> = <span class="variable">r2</span> with { Y = 3};
    }
}
</code></pre>

仕組み的には、以下のように、「`readonly`が付いてるんだけどコンストラクター以外から書き換えられる set メソッドを用意」を考えているそうです。

<pre class="source" title="readonly なフィールドを書き換える set メソッド">
<code><span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="comment">// public initonly int X { get; } に対して</span>
 
    <span class="comment">// コンパイラーは &lt;Backing&gt;_X みたいな C# では書けない名前でフィールドを生成してる。</span>
    <span class="comment">// ここでは、説明のために単に _X で書く。</span>
    <span class="reserved">private</span> <span class="reserved">int</span> _X;
 
    <span class="comment">// get アクセサーに相当するメソッド</span>
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">get_X</span>() =&gt; _X;
 
    <span class="comment">// set アクセサーに相当するメソッド</span>
    [<span class="type">initonly</span>]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">set_X</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; _X = <span class="variable">x</span>;
}
</code></pre>

`[initonly]` 属性のところは、単なる(C# コンパイラーだけが使う)属性じゃなくて、
.NET ランタイムが解釈して特別扱いできる属性値(modreq)にしたいそうです。

現状の .NET の仕様では、こういう `readonly` なものの強制書き換えは unverifiable(安全性を検証できなくなる) だけど、unsafe ではないとのこと。
verifiable にするためにも、`[initonly]` が付いたメソッドには「コンストラクターの直後以外で呼べない」などの制限を掛けるという方針。

`with` (部分書き換え)については `WithConstructor` という特別なメソッドを用意して、それに対してコンストラクター同様の制限を掛ける方式を検討中とのこと。

### 現状の records 要約

3つに分けて考えるとよさそうです。

- primary コンストラクター (positional records)
- initonly プロパティ (nominal records)
- data class/data struct

1つ目の primary コンストラクターは、以下のような書き方で、
コンストラクター(の引数)とプロパティを同時に定義する書き方。

<pre class="source" title="primary コンストラクター (positional records)">
<code><span class="reserved">class</span> <span class="type">Records</span>(<span class="reserved">int</span> X);
   
<span class="comment">// ↓解釈結果</span>
 
<span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; }
    <span class="reserved">public</span> <span class="type">Records</span>(<span class="reserved">int</span> <span class="variable">X</span>) =&gt; <span class="reserved">this</span>.X = <span class="variable">X</span>;
}
</code></pre>

2つ目が今日話した initonly プロパティ。
immutable なデータ構造に対してプロパティ初期化子が使えるようにするもの。

<pre class="source" title="initonly プロパティ">
<code><span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="reserved">public</span> initonly <span class="reserved">int</span> X { <span class="reserved">get</span>; }
}
 
<span class="comment">// ↓解釈結果</span>
 
<span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="reserved">private</span> <span class="reserved">int</span> _X;
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="method">get_X</span>() =&gt; _X;
    [<span class="type">InitOnly</span>]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">set_X</span>(<span class="reserved">int</span> <span class="variable">x</span>) =&gt; _X = <span class="variable">x</span>;
}
</code></pre>

3つ目は、`data class`/`data struct` と書くことで、プロパティから `Equals`や`GetHashCode`、`Deconstruct`などの関連メソッドを自動生成する機能。
要は、これまでの提案と比べると、primarily コンストラクター が別機能として独立したことになります。
(キーワードは仮。`data` じゃなくて `record` キーワードになったりはするかも。)

<pre class="source" title="data class">
<code><span class="comment">// data が付く。</span>
data <span class="reserved">class</span> <span class="type">Records</span>
{
    <span class="comment">// 中身自体は既存の C# コード。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
}
 
<span class="comment">// ↓解釈結果</span>
 
<span class="reserved">class</span> <span class="type">Records</span> : <span class="type">IEquatable</span>&lt;<span class="type">Records</span>&gt;
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> Y { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Deconstruct</span>(<span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">x</span>, <span class="reserved">out</span> <span class="reserved">int</span> <span class="variable">y</span>) =&gt; (<span class="variable">x</span>, <span class="variable">y</span>) = (X, Y);
    <span class="reserved">public</span> <span class="reserved">bool</span> <span class="method">Equals</span>(<span class="type">Records</span> <span class="variable">other</span>) =&gt; (X, Y) == (<span class="variable">other</span>.X, <span class="variable">other</span>.Y);
    <span class="comment">// その他、GetHashCode, == と !=, Equals(object) 等々…</span>
}
</code></pre>

primary コンストラクター を独立させたのと、今回 initonly プロパティを足したわけですが、
要するに、これらの混在もできる予定です。

<pre class="source" title="3つの機能の混在">
<code><span class="comment">// primary コンストラクターと、</span>
data <span class="reserved">class</span> <span class="type">Records</span>(<span class="reserved">int</span> X)
{
    <span class="comment">// initonly プロパティと、</span>
    <span class="reserved">public</span> initonly <span class="reserved">int</span> Y { <span class="reserved">get</span>; }

    <span class="comment">// 既存の普通のプロパティが混在。</span>
    <span class="reserved">public</span> <span class="reserved">int</span> Z { <span class="reserved">get</span>; <span class="reserved">set</span>; }
 
    <span class="comment">// data が付いてるので Equals とかがコンパイラー生成される。</span>
    <span class="comment">// (X, Y, Z から生成。)</span>
}
</code></pre>
