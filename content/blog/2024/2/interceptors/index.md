---
title: "インターセプター"
source_url: "https://ufcpp.net/blog/2024/2/interceptors/"
content_type: "BlogEntry"
published_at: "2024-02-11T18:49:49"
updated_at: "2024-02-11T19:57:52"
tags: []
umbraco_id: 2484
parent_id: 2480
sort_order: 3
aliases: []
---

# インターセプター

「最近動きがあったもの」ブログをいくつか書いてて、
「[続報](../breaking-changes/index.md)」みたいなものも書いてるわけですが。

今日のも「まあ、去年から動く実装すでにあるんだけど」という意味では続報なものの、
今日のインターセプターはあまりうちのサイトで取り上げておらず、初めて説明を書く話。
([ライブ配信](https://github.com/ufcpp/UfcppSample/issues/456)では時々話に出てるんですが。)

## インターセプター

今日話すインターセプターは、まあ、[Source Generator](../../../../study/csharp/misc/analyzer-generator.md)向けの機能です。

既存の Source Generator でも、
クラスを丸ごと生成するとか、メソッドの中身を生成とかはできます。

.NET が標準で提供しているやつだと `GeneratedRegex` とか。
partial メソッドに属性を付けて、メソッドの中身をコード生成しています。

<pre class="source" title="GeneratedRegex">
<span class="reserved">partial</span> <span class="reserved">class</span> <span class="type">Reg</span>
{
    <span class="comment">// この属性を付けた partial メソッドに対して、</span>
    <span class="comment">// Sytem.Text.RegularExpressions.Generator でコード生成してる。</span>
    [<span class="type">GeneratedRegex</span>(<span class="string">@&quot;\d+&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="type">Regex</span> <span class="method"><span class="static">Digits</span></span>();
}
</pre>

これで困るのは、**メソッドの呼び出し箇所ごとに違う実装をコード生成したい場合**です。

一例として以下のようなコードを考えます。
要は「const string を `Parse` するならコンパイル時に全部やっちゃえるのでは」という話。

<pre class="source" title="呼び出し箇所ごとに別コード生成したいものの例">
<span class="reserved">using</span> <span class="reserved">static</span> System<span class="operator">.</span><span class="static"><span class="type">Console</span></span>;

<span class="comment">// const string の Parse はコンパイル時にできるのでは。</span>
<span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="string">&quot;123&quot;</span>));
<span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">int</span><span class="operator">.</span><span class="static"><span class="method">Parse</span></span>(<span class="string">&quot;456&quot;</span>));

<span class="comment">// こういうのは無理として。</span>
<span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="method"><span class="static">ReadLine</span></span>()<span class="operator">!</span>));
</pre>

int だと「最初から `123` と書け」と言われればそれまでなので役に立ちませんが、
例えば [`BigInteger`](https://learn.microsoft.com/ja-jp/dotnet/api/system.numerics.biginteger) とかなら意味がありそうです。

こういう場合に、メソッド呼び出し箇所を乗っ取って別のメソッドに差し替えてしまう仕組みを検討中で、
それがインターセプター(interceptor: 妨害者、途中で奪う・捕まえるもの)です。
C# 12 時点でプレビュー機能として実装されていて、後述するオプションを設定すると現在でも動かすことができます
(提案ドキュメント: [Interceptors](https://github.com/dotnet/roslyn/blob/main/docs/features/interceptors.md))。

現在の実装をベースに話すと、
先ほどのコードが F:/src/ConsoleApp1/ConsoleApp1/Program.cs というパスのファイルに書かれているものとして、
以下のようなコードを作ります(Source Generator で作ること前提)。

<pre class="source" title="インターセプターの例">
<span class="reserved">using</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

<span class="reserved">namespace</span> ConsoleApp1
{
    <span class="reserved">static</span> <span class="reserved">class</span> <span class="type"><span class="static">Interceptors</span></span>
    {
        [<span class="type">InterceptsLocation</span>(<span class="string">&quot;F:/src/ConsoleApp1/ConsoleApp1/Program.cs&quot;</span>, <span class="number">4</span>, <span class="number">15</span>)]
        <span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Parse123</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="number">123</span>;

        [<span class="type">InterceptsLocation</span>(<span class="string">&quot;F:/src/ConsoleApp1/ConsoleApp1/Program.cs&quot;</span>, <span class="number">5</span>, <span class="number">15</span>)]
        <span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">int</span> <span class="static"><span class="method">Parse456</span></span>(<span class="reserved">string</span> <span class="variable local">_</span>) <span class="operator">=&gt;</span> <span class="number">456</span>;
    }
}

<span class="reserved">namespace</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices
{
    [<span class="type">AttributeUsage</span>(<span class="type">AttributeTargets</span><span class="operator">.</span>Method, <span class="property">AllowMultiple</span> <span class="operator">=</span> <span class="reserved">true</span>)]
    <span class="reserved">file</span> <span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">InterceptsLocationAttribute</span> : <span class="type">Attribute</span>
    {
        <span class="reserved">public</span> <span class="type">InterceptsLocationAttribute</span>(<span class="reserved">string</span> <span class="variable local">filePath</span>, <span class="reserved">int</span> <span class="variable local">line</span>, <span class="reserved">int</span> <span class="variable local">column</span>) { }
    }
}
</pre>

`InterceptsLocation` 属性が付いたメソッドで、属性で指定したファイル・行・列にあるメソッド呼び出しを乗っ取ります。

この例の場合、

* `("略/Program.cs", 4, 15)` → `int.Parse("123")` の場所 → ここを `Parse123` で乗っ取る
* `("略/Program.cs", 5, 15)` → `int.Parse("456")` の場所 → ここを `Parse456` で乗っ取る

という挙動。
その結果、Program.cs の内容は以下のものに置き換わったものとしてコンパイルされます。

<pre class="source" title="インターセプターの適用結果">
<span class="reserved">using</span> <span class="reserved">static</span> System<span class="operator">.</span><span class="static"><span class="type">Console</span></span>;

<span class="comment">// const string の Parse はコンパイル時にできるのでは。</span>
<span class="static"><span class="method">WriteLine</span></span>(<span class="type">Interceptors</span><span class="operator">.</span><span class="method"><span class="static">Parse123</span></span>(<span class="string">&quot;123&quot;</span>));
<span class="static"><span class="method">WriteLine</span></span>(<span class="type">Interceptors</span><span class="operator">.</span><span class="static"><span class="method">Parse456</span></span>(<span class="string">&quot;456&quot;</span>));

<span class="comment">// こういうのは無理として。</span>
<span class="static"><span class="method">WriteLine</span></span>(<span class="reserved">int</span><span class="operator">.</span><span class="method"><span class="static">Parse</span></span>(<span class="method"><span class="static">ReadLine</span></span>()<span class="operator">!</span>));
</pre>

ちなみに、ファイル指定がフルパスなのが気持ち悪すぎていまいち取り上げる気になれず、今までブログ化していなかったり。
後述しますが、フルパスだと困るであろうことは課題として認識されていて、C# 13 正式リリースまでには対処が入ると思います。

## プレビュー オプション

インターセプターは「早めに動くものを提供して、ASP.​NET チーム辺りで実際に使ってもらってフィードバックをもらいたい」という意図で、かなり早い段階でプレビュー機能としてリリースされています。
当然作ってる側も自信をもってリリースしているわけがなく、
大幅に変更がかかる可能性があります。

(ちなみに、実際に試してもらってるコードはここ: [Http.Extensions](https://github.com/dotnet/aspnetcore/tree/c52a7abf3a8a08213dea461c7d97f421b9d862e6/src/Http/Http.Extensions/gen))

そんな感じの早期プレビューなのと、
先ほどの例の `int.Parse` みたいな何の変哲もない普通のメソッドを乗っ取れる仕組みが結構怖いので、
オプション指定しないとコンパイルできません。

当初は csproj に `<Features>InterceptorsPreview<Features>` というタグを入れておけば使える仕様でした。
単に「LangVersion preview」ではなくて、フィーチャースイッチを明示。

が、その後、「もっと制限をかけたい」ということになったみたいで、
`<InterceptorsPreviewNamespaces>ConsoleApp1</InterceptorsPreviewNamespaces>` みたいに、
「特定の名前空間にあるインターセプターのみを認める」というオプションの書き方に変わりました。
(コンパイラーがインターセプターを検索するコストを減らすためだそうです。
今は `InterceptorsPreviewNamespaces` という名前なものの、
プレビューが外れたら `InterceptorsNamespaces` にする予定。)

## 現在の open issue

ということで、最近あった検討内容:

* [Interceptors open issues for C# 13](https://github.com/dotnet/csharplang/pull/7835)

### 相対パス化

一番はやっぱり「フルパス問題」。

Source Generator はそれなりに重たい処理になることがあって、
コンパイルのたびに何度も走ってほしくないということがあります。
そういう場合ように、Source Generator の実行結果をファイル システムに書きこんじゃって、
手書きのコードと一緒にコミットしてしまうという運用も考えられます
([`EmitCompilerGeneratedFiles` オプション](https://learn.microsoft.com/ja-jp/dotnet/csharp/roslyn-sdk/source-generators-overview)でできます)。

フルパスはこのオプションを使うと一発で破綻します。
自分の手元では F:/src/ConsoleApp1 かもしれないものの、
Git とかで他人と共有すると、他の人のパスは F:/users/UserName/repos/ConsoleApp1 とかだったりします。
都度コード生成するなら問題ありませんが、コミットしようとするとパスの差で困ります。

なので、`InterceptsLocation` 属性に渡すパスは相対パスにできるようになると思います。
とはいえ、以下のような課題あり。

* 「インターセプトする側とされる側の保存先が別ドライブにある」みたいな場合、相対パス指定できない
  * 元からあるファイルシステムの問題で、`#line` ディレクティブとかでも同様の制限あり
* Source Generator が生成するファイルのパスがわからないと「そこからインターセプトされる側」の相対パスがわからない
  * Source Generator の生成結果の出力先パスを相対パス フレンドリーな場所に変更するつもりあり

### location specifier

もしくは、ファイル パスに依存すること自体をやめて、何らかの抽象的な「location specifier (場所指定子)」を受け付ける仕組みを用意するという案も出ています。
まずは `InterceptsLocation` 属性に以下のコンストラクター追加。

<pre class="source" title="locationSpecifier 引数のコンストラクター">
<span class="reserved">namespace</span> System<span class="operator">.</span>Runtime<span class="operator">.</span>CompilerServices;

[<span class="type">AttributeUsage</span>(<span class="type">AttributeTargets</span><span class="operator">.</span>Method, <span class="property">AllowMultiple</span> <span class="operator">=</span> <span class="reserved">true</span>)]
<span class="reserved">sealed</span> <span class="reserved">class</span> <span class="type">InterceptsLocationAttribute</span> : <span class="type">Attribute</span>
{
    <span class="reserved">public</span> <span class="type">InterceptsLocationAttribute</span>(<span class="reserved">string</span> <span class="variable local">filePath</span>, <span class="reserved">int</span> <span class="variable local">line</span>, <span class="reserved">int</span> <span class="variable local">column</span>) { }
    <em><span class="reserved">public</span> <span class="type">InterceptsLocationAttribute</span>(<span class="reserved">string</span> <span class="variable local">locationSpecifier</span>) { }</em>
}
</pre>

一例として以下のように、何らかの書式で「ソースコードの場所」がわかる文字列を指定。

<pre class="source" title="locationSpecifier 指定のインターセプター">
<span class="reserved">class</span> <span class="type">Interceptors</span>
{
    [<span class="type">InterceptsLocation</span>(<span class="string">&quot;v1:../../src/MyFile.cs(12,34)&quot;</span>)]
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">Interceptor</span>() { }
}
</pre>

独特な書式を覚えるのは大変でしょうが、
幸い、インターセプターは Source Generator 用の機能なわけで、
Source Generator 作者向けに location specifier を取得できる API を同時に提供するつもりだそうです
(なので独特な書式を覚える必要はないはず)。

<pre class="source" title="location specifier を取得できる API の追加">
<span class="reserved">namespace</span> Microsoft<span class="operator">.</span>CodeAnalysis;

<span class="reserved">public</span> <span class="reserved">readonly</span> <span class="reserved">struct</span> <span class="type struct">SourceProductionContext</span>
{
    <span class="reserved">public</span> <span class="reserved">void</span> <span class="method">AddSource</span>(<span class="reserved">string</span> <span class="variable local">hintName</span>, <span class="reserved">string</span> <span class="variable local">source</span>);
    <em><span class="reserved">public</span> <span class="reserved">string</span> <span class="method">GetInterceptsLocationSpecifier</span>(<span class="type">InvocationExpressionSyntax</span> <span class="variable local">intercepted</span>, <span class="reserved">string</span> <span class="variable local">interceptorFileHintName</span>);</em>
}
</pre>

Roslyn の構文木ノードを受け取って、
そこに相当する specifier を返してもらって、
この specifier をそのまま `InterceptsLocation` 属性に出力する想定です。

### もっと先の話

「たぶん C# 13 より後」と分類されている課題もいくつか。

* プロパティとかコンストラクターもインターセプトしたい
  * [`UnsafeAccessor`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafeaccessorattribute) と書き方そろえるかも
* インターセプトする側とされる側で完全にシグネチャが同じでないとダメなのを緩和したい
  * 「`Delegate` を `Func<T>` に変える」みたいなことはできていいはず
