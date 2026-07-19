---
title: "C# でビットフィールド"
source_url: "https://ufcpp.net/blog/2017/4/bitfields/"
content_type: "BlogEntry"
published_at: "2017-04-28T20:24:16"
updated_at: "2017-04-28T20:24:16"
tags: []
umbraco_id: 2055
parent_id: 2050
sort_order: 2
aliases: []
---

# C# でビットフィールド

[csharplang](https://github.com/dotnet/csharplang/)に、

- [C++のビットフィールドみたいなの、C# にもほしい](https://github.com/dotnet/csharplang/issues/465)
- [(任意のビット数を表す)bit 型が欲しい](https://github.com/dotnet/csharplang/issues/457)

みたいなのが投稿されていまして。

「それ、ライブラリとアナライザー、ちょっとしたソースコード生成でできるよ。」という話。

## BitFields ライブラリ

ということで実装してみたのがこちら。

- [BitFields ライブラリのソースコード](https://github.com/ufcpp/BitFields)
- [利用例(double/floatの内部ビット操作とか、RGB555形式とか)](https://github.com/ufcpp/UfcppSample/blob/master/Demo/2017/BitFieldsSample/Program.cs)
- [他に、昔実際に仕事で書いたビットフィールドの例](https://gist.github.com/ufcpp/f1fd6a5acd7717565e4fddfa9431e9fa)

昔、ビットフィールド的なものを手作業実装してた時に、「これはコード生成でやりたい…」とか思ってて、
できる宛まではついてたんですが。
なんだかんだ言って[アナライザーを書くの](http://www.buildinsider.net/enterprise/roslynextension/03)は結構めんどくさいんで、放置してすでに数年。

まあ、いい機会だから久々に重い腰を上げてアナライザー書いてみるかと思って作ったのが上記の[BitFields](https://github.com/ufcpp/BitFields)です。

## できること

### Nビット整数

Nビットまでの整数を受け付ける`BitN`(`N`は1～64)型と、それに対するアナライザーがあります。

![BitN型とアナライザー](../../../../../assets/media/1130/bitn.png)

ちゃんと、ビット数を超える値(例えば`Bit1`に2)を代入しようとするとコンパイル エラーになります。

### ビットフィールドコード生成

例えば、[RGB555](http://www.webtech.co.jp/blog/optpix_labs/format/942/)とか、半端なビット数で色情報を扱う形式があります。
32bitカラーが当たり前な今時だと珍しいですけど、昔はこういう形式も割と使われていました。

で、それを、こう書く。

<pre class="source" title="Rgb555構造体定義">
<code><reserved></span><span class="reserved">struct</span> <span class="type">Rgb555</span>
{
    <span class="reserved">enum</span> <span class="type">BitFields</span>
    {
        B = 5,
        G = 5,
        R = 5,
    }
}
</code></pre>

コード生成都合で、「構造体の中に`BitFields`という名前のenumを定義、値としてビット数を与える」みたいな規約ベースの型情報を書きます。
このenumはあくまでメタデータ(型情報)であって、実行時には一切使いません。

で、以下のように、クイック アクション(電球アイコン)が出るので、生成メニュー(Generate bit-fields)を選択。

![ビットフィールド生成](../../../../../assets/media/1131/bitfieldgenerator.png)

以下のようなコードが生成されます。

<pre class="source" title="Rgb555の生成結果">
<code><reserved></span><span class="reserved">using</span> BitFields;

<span class="reserved">partial</span> <span class="reserved">struct</span> <span class="type">Rgb555</span>
{
    <span class="reserved">public</span> <span class="reserved">ushort</span> Value;

    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">int</span> BShift = 0;
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">ushort</span> BMask = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((1U &lt;&lt; 5) - (1U &lt;&lt; 0)));
    <span class="reserved">public</span> <span class="type">Bit5</span> B
    {
        <span class="reserved">get</span> =&gt; (<span class="type">Bit5</span>)((Value &amp; BMask) &gt;&gt; BShift);
        <span class="reserved">set</span> =&gt; Value = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((Value &amp; ~BMask) | ((((<span class="reserved">ushort</span>)<span class="reserved">value</span>) &lt;&lt; BShift) &amp; BMask)));
    }
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">int</span> GShift = 5;
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">ushort</span> GMask = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((1U &lt;&lt; 10) - (1U &lt;&lt; 5)));
    <span class="reserved">public</span> <span class="type">Bit5</span> G
    {
        <span class="reserved">get</span> =&gt; (<span class="type">Bit5</span>)((Value &amp; GMask) &gt;&gt; GShift);
        <span class="reserved">set</span> =&gt; Value = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((Value &amp; ~GMask) | ((((<span class="reserved">ushort</span>)<span class="reserved">value</span>) &lt;&lt; GShift) &amp; GMask)));
    }
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">int</span> RShift = 10;
    <span class="reserved">private</span> <span class="reserved">const</span> <span class="reserved">ushort</span> RMask = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((1U &lt;&lt; 15) - (1U &lt;&lt; 10)));
    <span class="reserved">public</span> <span class="type">Bit5</span> R
    {
        <span class="reserved">get</span> =&gt; (<span class="type">Bit5</span>)((Value &amp; RMask) &gt;&gt; RShift);
        <span class="reserved">set</span> =&gt; Value = <span class="reserved">unchecked</span>((<span class="reserved">ushort</span>)((Value &amp; ~RMask) | ((((<span class="reserved">ushort</span>)<span class="reserved">value</span>) &lt;&lt; RShift) &amp; RMask)));
    }
}
</code></pre>

現状だと1度は手作業で「クイック アクションの選択」が必要なので、使い勝手はいまいちなんですが。
そのうち、[正式にコード生成機能がC#に入るはず](https://github.com/dotnet/csharplang/issues/107)で、その暁ににはもう少し利便性がよくなります。

## ライブラリだけでできることはライブラリで

まあ、一般論として、ライブラリだけでできるならライブラリ提供でいいわけで。
ライブラリを作ってみた結果として、本当にライブラリだけだと不便ということになれば、そこで初めて言語文法の提案をすればいい。

実際、ライブラリありきで出ている提案もあったりします。

- [Midori](https://ja.wikipedia.org/wiki/Midori_(%E3%82%AA%E3%83%9A%E3%83%AC%E3%83%BC%E3%83%86%E3%82%A3%E3%83%B3%E3%82%B0%E3%82%B7%E3%82%B9%E3%83%86%E3%83%A0))の成果の1つとして、[Slice.NET](https://github.com/joeduffy/slice.net)ってのが作られる
- Slice.NETを標準ライブラリに取り入れるべく、[corefxlab](https://github.com/dotnet/corefxlab/blob/master/docs/specs/span.md)で検討される
- 現在、実際に[System.Memory](https://github.com/dotnet/corefx/tree/master/src/System.Memory)というパッケージ名で標準ライブラリ化してる
  - [dailyビルドなプレビュー版で良ければ、NuGetパッケージあり](https://dotnet.myget.org/feed/dotnet-core/package/nuget/System.Memory)
  - プレビューが外れるのはたぶん、「[C# 7.2](https://github.com/dotnet/csharplang/milestone/6)」のタイミング
- これが標準ライブラリに入るのであれば、C#としても検討していい文法がある: [slicing](https://github.com/dotnet/csharplang/issues/185)

特に、今回やったビットフィールド生成みたいなものは、ちょっと「コンパイラーの仕事」にするにはいまいちかなぁと思います。

- シフトやマスク演算のコードが見えている方が理解がしやすい
- エンディアンの問題とかあって、汎用化するには怖い
  - 個人でライブラリを作る限りには「ビッグ エンディアン？知らない子ですね。現存するエンディアンじゃないですよ？」とか敵を作りそうな発言もできるものの、標準に取り込む際にそれを言えるかというと無理
- 人によって求めるものが違う
  - immutable版が欲しい
  - `Value`フィールドがpublicなのは嫌
  - コンストラクター、デコンストラクターもほしい
  - `BitN`、コンパイル時チェックだけじゃなくて実行時チェックもほしい(あるいは逆に、そんな実行時コストは絶対避けたい)

しかも今のC#だと、アナライザーを書けば「`Bit1`なら0と1だけ代入で来てほしい。2以上はビット数オーバーでコンパイル時エラー」みたいなこともできます。
ライブラリだけでできちゃうことも増えているので、コンパイラーのバージョンアップとか待ってないでライブラリ作っちゃえば良かったりします。
