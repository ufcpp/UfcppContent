---
title: "必ず、かの邪知暴虐の T4 を除かねばならぬと決意した"
source_url: "https://ufcpp.net/blog/2022/12/no-more-t4/"
content_type: "BlogEntry"
published_at: "2022-12-20T22:39:14"
updated_at: "2022-12-20T22:58:46"
tags: []
umbraco_id: 2446
parent_id: 2438
sort_order: 5
aliases: []
---

# 必ず、かの邪知暴虐の T4 を除かねばならぬと決意した

個人的に、前々から「[T4](https://learn.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates) は将来性が見えなさ過ぎてもう使うのやめたい」と言い続けていたわけですが、
最近ようやく自分が保守している T4 を全部別の手段で書き換えたので、
今日はそれの話。

## <a id="t4">T4 (Text Template Transformation Toolkit)</a>

テキスト テンプレートというと、ひな形的なテキストを簡易な文法で生成するようなものです。

例えば、

<pre class="source">
public static bool TryParse(this string s, out {{T}} x) =&gt; {{T}}.TryParse(s, out x);
</pre>

みたいな文字列の、`{{T}}` のところに `bool`, `byte`, `int`, `double` を与えて、

<pre class="source">
public static bool TryParse(this string s, out bool x) =&gt; bool.TryParse(s, out x);
public static bool TryParse(this string s, out byte x) =&gt; byte.TryParse(s, out x);
public static bool TryParse(this string s, out int x) =&gt; int.TryParse(s, out x);
public static bool TryParse(this string s, out double x) =&gt; double.TryParse(s, out x);
</pre>

とかを生成したいことがたまにあります。

今書いたみたいに4種・4行程度なら手書きでも全然かまわないんですが、
`sbyte`, `short`, `ushort`, ... と増やしていくとテキスト テンプレートに頼りたくなります。

C# でテキスト テンプレートというと、
[T4](https://learn.microsoft.com/ja-jp/visualstudio/modeling/code-generation-and-t4-text-templates) (Text Template Transfomration Toolkit)が有名ではあります。

T4 を使うと、上記の `Parse` は以下のように書けます。

<pre class="source">
&lt;#
var types = new[] { "bool", "byte", "int", "double" };

foreach (var t in types)
{
#&gt;
    public static bool TryParse(string s, out &lt;#= t #&gt; x) =&gt; &lt;#= t #&gt;.TryParse(s, out x);
&lt;#
}
#&gt;
</pre>

## <a id="t4-now">T4 の今</a>

元々 Entity Framework が内部で使っていたツールを公にしてしまったものですよね、確か。
今となっては本当に「してしまった」みたいな言い方にした方がいいと僕は本気で思っているんですけども。
どうも、.NET の中の人も、Entity Framework チーム以外あんまり乗り気で使っている風には見えず。
真面目に使う気があるのなら今時もうちょっと改良されててもよさそうなものなのに、
ちょっと塩漬け感があります。

例えば以下のような問題あり。

* Visual Studio でしか動かず、しかも、手作業で .tt ファイルを開いて保存したタイミングでしかテキスト生成が走らない
    * 今時あれば、[Roslyn Source Generator](../../../../study/csharp/misc/analyzer-generator.md) 化すれば dotnet build でテキスト生成できるのにやってない
    * Git とかで管理するなら、生成結果のテキストもコミットする運用になる
* Visual Studio 自体の表示言語に生成結果が依存する
    * 編集して保存した人の表示言語によって生成結果が変わる
    * 無駄に差分が出て、Git とかの差分が悲惨
* csproj 内にいろいろとゴミが残る
    * [`<Service Include="{508349b6-6b84-4df5-91f0-309beebad82d}" />`](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2022/NoMoreT4/ClassLibrary1/ClassLibrary1.csproj#L11) とか
    * [`System.CodeDom` の参照](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2022/NoMoreT4/ClassLibrary1/ClassLibrary1.csproj#L10)とか
    * [`<None Update="T4Generator.tt">` みたいなの](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2022/NoMoreT4/ClassLibrary1/ClassLibrary1.csproj#L14-L24)とか

さらに、中間的に作られる「テキスト生成するための generator クラス」がまたかなり悲惨だったりします。

一例: [元 tt ファイル](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2022/NoMoreT4/ClassLibrary1/T4Generator.tt) → [生成される generator クラス](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2022/NoMoreT4/ClassLibrary1/T4Generator.cs)

例えば、元 tt ファイルで `<#= t #>` みたいになっているところは
generator 内では `ToStringWithCulture(t)` みたいに展開されるんですが、
この `ToStringWithCulture` の中身は以下のようになっています。

<pre class="source" title="T4 が生成する ToStringWithCulture メソッド">
<span class="reserved">public</span> <span class="reserved">string</span> <span class="method">ToStringWithCulture</span>(<span class="reserved">object</span> <span class="variable local">objectToConvert</span>)
{
    <span class="control">if</span> ((<span class="variable local">objectToConvert</span> <span class="operator">==</span> <span class="reserved">null</span>))
    {
        <span class="control">throw</span> <span class="reserved">new</span> <span class="reserved">global</span><span class="operator">::</span>System<span class="operator">.</span><span class="type">ArgumentNullException</span>(<span class="string">&quot;objectToConvert&quot;</span>);
    }
    System<span class="operator">.</span><span class="type">Type</span> <span class="variable">t</span> <span class="operator">=</span> <span class="variable local">objectToConvert</span><span class="operator">.</span><span class="method">GetType</span>();
    System<span class="operator">.</span>Reflection<span class="operator">.</span><span class="type">MethodInfo</span> <span class="variable">method</span> <span class="operator">=</span> <span class="variable">t</span><span class="operator">.</span><span class="method">GetMethod</span>(<span class="string">&quot;ToString&quot;</span>, <span class="reserved">new</span> System<span class="operator">.</span><span class="type">Type</span>[] {
                <span class="reserved">typeof</span>(System<span class="operator">.</span><span class="type">IFormatProvider</span>)});
    <span class="control">if</span> ((<span class="variable">method</span> == <span class="reserved">null</span>))
    {
        <span class="control">return</span> <span class="variable local">objectToConvert</span><span class="operator">.</span><span class="method">ToString</span>();
    }
    <span class="control">else</span>
    {
        <span class="control">return</span> ((<span class="reserved">string</span>)(<span class="variable">method</span><span class="operator">.</span><span class="method">Invoke</span>(<span class="variable local">objectToConvert</span>, <span class="reserved">new</span> <span class="reserved">object</span>[] {
                    <span class="reserved">this</span><span class="operator">.</span>formatProviderField })));
    }
}
</pre>

任意の型に対してカルチャー(`IFromatProvider`)指定するためだけにリフレクション。
しかも、`MethodInfo` のキャッシュもせず、毎回律義に `GetMethod`。
さらに、常に `object` 引数で受け取っているので、`int` (おそらく最多で渡される)とかだと都度[ボックス化](../../../../study/csharp/resource/rmboxing.md)。

T4 が作られた当初ならしょうがなかったのかもしれないですけどねぇ。
今なら、単に [`string.Create`](https://learn.microsoft.com/ja-jp/dotnet/api/system.string.create#system-string-create(system-iformatprovider-system-runtime-compilerservices-defaultinterpolatedstringhandler@))とか[StringBuilder.Append](https://learn.microsoft.com/ja-jp/dotnet/api/system.text.stringbuilder.append#system-text-stringbuilder-append(system-iformatprovider-system-text-stringbuilder-appendinterpolatedstringhandler@))でカルチャー指定もできるのに。
というか、むしろ、[カルチャー依存やめろ、誰得](../../../2021/8/invariantculture/index.md)。

ちなみに、T4 生成の generator クラスと、自前で[文字列補間](../../../../study/csharp/start/st_string.md#string-interpolation)を使って書いた generator でベンチマークを比べると、一例として以下のテーブルくらいの差が出ます。

|        Method |        Mean |    Error |   StdDev |
|-------------- |------------:|---------:|---------:|
|            T4 | 19,247.7 ns | 74.46 ns | 69.65 ns |
| Interpolation |    330.6 ns |  5.94 ns |  5.55 ns |

2桁差。
2倍ではなく、<em>2桁</em>。
マイクロ秒とナノ秒の補助単位違いレベル。

という感じで、T4、
さすがに中身がグダグダすぎ、かつ、近代化される気配がまるっきり皆無でつらいかなと思います。
テンプレートの文法とかはそこまでおかしくもないんですけどね。
さすがにもう使っていられないかなと…

## <a id="no-more-t4">脱 T4</a>

ということで脱 T4 の話。

ただ、T4 の用途は2種類ありまして、それぞれ代替手段が異なります。

* TextTemplatingFilePreprocessor: 上記でいう「generator を作る」ところまでやるモード
* TextTemplatingFileGenerator: さらにその generator を実行して、最終結果を直接生成するモード

### <a id="file-preprocessor">TextTemplatingFilePreprocessor</a>

「generator を作るところまで」の方。

TextTemplatingFilePreprocessor な T4 はもう本当に存在意義がないですね。
先ほどすでに「文字列補間で自前で」とかやっていますが、
文字列補間で十分です。

特に、[C# 10 で文字列補間のパフォーマンスが劇的に向上](../../../../study/csharp/start/st_string.md#csharp10-improvement)していますし、
C# 11 で入った[生文字列リテラル](../../../../study/csharp/start/st_string.md#raw-string)によってテンプレートも書きやすくなっています。
本校冒頭で書いたテンプレートなら、普通に以下のように書けます。

<pre class="source" title="文字列補間でテンプレート">
<span class="reserved">using</span> System<span class="operator">.</span>Text;

<span class="reserved">var</span> <span class="variable">s</span> <span class="operator">=</span> <span class="reserved">new</span> <span class="type">StringBuilder</span>();

<span class="control">foreach</span> (<span class="reserved">var</span> <span class="variable">t</span> <span class="control">in</span> <span class="reserved">new</span>[] { <span class="string">&quot;bool&quot;</span>, <span class="string">&quot;byte&quot;</span>, <span class="string">&quot;int&quot;</span>, <span class="string">&quot;double&quot;</span> })
{
    <span class="variable">s</span><span class="operator">.</span><span class="method">Append</span>(<span class="string">$$&quot;&quot;&quot;
</span><span class="string">            public static bool TryParse(string s, out </span>{{<span class="variable">t</span>}}<span class="string"> x) =&gt; </span>{{<span class="variable">t</span>}}<span class="string">.TryParse(s, out x);
</span><span class="string">
        &quot;&quot;&quot;</span>);
}
</pre>

(カルチャー指定が必要なら `Append` メソッドの第1引数を追加。)

テキスト テンプレートとしては結構冗長ですけども、まあ、許容範囲で、
「素の C# だけで書けてる」ということを考慮すると十分満足の行くコードじゃないかと思います。

T4 からの移行も割かし簡単で、
[こんな感じのコード](https://gist.github.com/ufcpp/2262fe3f607974b68b849b5b47a4dc32)で置換を掛けるだけで行けます。

* `<#=` ～ `#>` を、`{{` ～ `}}` に置換
* `#>` を ` s.Append($$"""` に置換
* `<#` を `""");` に置換

(これだと不足もあるんですが、あとは手での書き換えでもなんとかなるレベル。)

### <a id="file-generator">TextTemplatingFileGenerator</a>

「generator を実行までして最終結果を直接得る」の方。

こっちはさすがに素の C# ではできないんですが。
ビルド時に何かしらのテキストを生成するものというと、最近だと Roslyn Source Generator です。

T4 も、テンプレートの文法自体に不満はそこまでないので、Source Generator 実装に置き換わっていたらそれを普通に使うんですけどね…
ここ数年定期的に「誰か T4 を Source Generator 実装しなおしてないかな」とか検索したりしてたんですが…
いないですね。一向に。

そして、「ないので自分で作ろう」ってなったときに、なかなかきれいな T4 エンジン ライブラリが見つからず。
「だったらもっと楽に使えそうな別のテンプレート エンジンを使いたい」となりまして。
結局、Source Generator で一番使いやすそうだったのが scriban でした。

* [scriban](https://github.com/scriban/scriban)

ということで作ったのがこれです:

* [ScribanSourceGenerator](https://github.com/ufcpp/ScribanSourceGenerator)
* [利用例](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2022/NoMoreT4/FileGenerator)

例えば以下のような拡張子 .scriban のファイルを置くか、

<pre class="source">
static class Extensions
{
{{
for $t in ["bool","byte","int","double"]
~}}
    public static bool TryParse(this string s, out {{$t}} x) =&gt; {{$t}}.TryParse(s, out x);
{{ end }}
}
</pre>

以下のようにクラスに属性を付けてコード生成できます。

<pre class="source" title="">
<span class="reserved">namespace</span> FileGenerator;

[ScribanSourceGeneretor<span class="operator">.</span><span class="type">ClassMember</span>(<span class="string">&quot;&quot;&quot;
    {{
    for $t in [&quot;bool&quot;,&quot;byte&quot;,&quot;int&quot;,&quot;double&quot;]
    ~}}
        public static bool TryParse(this string s, out {{$t}} x) =&gt; {{$t}}.TryParse(s, out x);
    {{ end }}
    &quot;&quot;&quot;</span>)]
<span class="reserved">internal</span> <span class="reserved">static</span> <span class="reserved">partial</span> <span class="reserved">class</span> <span class="type"><span class="static">Extensions</span></span>
{
}
</pre>
