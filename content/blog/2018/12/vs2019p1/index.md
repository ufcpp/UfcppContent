---
title: "Visual Studio 2019 Preview 1"
source_url: "https://ufcpp.net/blog/2018/12/vs2019p1/"
content_type: "BlogEntry"
published_at: "2018-12-05T09:22:13"
updated_at: "2018-12-06T10:07:22"
tags: []
umbraco_id: 2185
parent_id: 2177
sort_order: 4
aliases: []
---

# Visual Studio 2019 Preview 1

[Connect](https://channel9.msdn.com/Events/Connect/Microsoft-Connect--2018) やってましたね。

とりあえず、関連ブログ:

- [Making every developer more productive with Visual Studio 2019](https://blogs.msdn.microsoft.com/visualstudio/2018/12/04/making-every-developer-more-productive-with-visual-studio-2019/)
  - [Visual Studio 2019 Preview](https://visualstudio.microsoft.com/ja/vs/preview/)
- [Announcing .NET Core 2.2](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-2-2/)
  - [.NET Core 2.2 downloads](https://dotnet.microsoft.com/download/dotnet-core/2.2)
- [Announcing .NET Core 3 Preview 1 and Open Sourcing Windows Desktop Frameworks](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-net-core-3-preview-1-and-open-sourcing-windows-desktop-frameworks/)
- [Announcing Open Source of WPF, Windows Forms, and WinUI at Microsoft Connect(); 2018](https://blogs.windows.com/buildingapps/2018/12/04/announcing-open-source-of-wpf-windows-forms-and-winui-at-microsoft-connect-2018/)
- [Announcing WPF, WinForms, and WinUI are going Open Source](https://www.hanselman.com/blog/AnnouncingWPFWinFormsAndWinUIAreGoingOpenSource.aspx)
  - [.NET Core 3.0 Preview](https://dotnet.microsoft.com/download/dotnet-core/3.0)
  - [github WPF](https://github.com/dotnet/wpf)
  - [github WinForms](https://github.com/dotnet/winforms)
  - [github Windows UI Library](https://github.com/Microsoft/microsoft-ui-xaml)
- [Announcing ASP.NET Core 2.2, available today!](https://blogs.msdn.microsoft.com/webdev/2018/12/04/asp-net-core-2-2-available-today/)
- [Announcing Entity Framework Core 2.2](https://blogs.msdn.microsoft.com/dotnet/2018/12/04/announcing-entity-framework-core-2-2/)

とりあえず、.NET Core 2.2 正式リリース ＆ .NET Core 3.0 プレビュー提供開始。
Visual Studio 2019も preview 1 がダウンロードできるようになりました。

あと、WPF、WinForms 等がオープンソースになったみたいです。
(重ね重ねの注意になりますが、.NET Core 3.0 で動く/オープンソースになったといっても、Windows 限定です。)

## C# 8.0

大してアナウンスされていませんが、Visual Studio 2019 Preview 1 でひそかにちょこっとだけ C# 8.0 を試せるようになっていたり。

ただ、

- LangVersion default は C# 7.0 だし、latest は 7.3 のまま
  - C# 8.0 を試してみたければ LangVersion 8.0 を明示的に指定
  - LangVersion beta とか experimental みたいなモニカーもなさそう
- 実装されている機能は 「[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)」参照。現時点では以下のものだけっぽい
  - Nullable reference type
  - Ranges
  - Null-coalescing Assignment
  - Alternative interpolated verbatim strings
  - (※追記) Async streams
- どうも、C# 8.0 の正式サポート開始は Visual Studio 2019 の最初のリリースではやらず、その後、 .NET Core 3.0 が出るタイミングからにしたいらしい

### 今使える機能

[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md) の C# 8.0 のところに並んでいる15個のうち、「Merged to dev16 preview1」になっている4個だけが今試せるっぽいです。

(※追記: もう1個、Async streams も実装されてるっぽい。
ただ、バグっててちゃんとコンパイルできず。)

ちなみに、ちゃんと、C# 8.0 の機能を使おうとすると、「プロジェクトを C# 8.0 にアップグレードしますか？」と聞かれます。

![Upgrade this project to C# language version 8.0 *beta*](../../../../../assets/media/1165/upgradetocs8.png)

むっちゃ beta を強調されてますが。

以下、軽くサンプルを。([github にも上げてあります](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/Cs8InVs2019P1))

#### Nullable reference type

<pre class="source" title="">
<code><span class="comment">// 有効にするには #nullable ディレクティブが必要。</span>
#nullable enable

<span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        Console.WriteLine(LengthSum(<span class="string">"abc"</span>, <span class="string">"xyz"</span>));
        Console.WriteLine(LengthSum(<span class="string">"abc"</span>, <span class="reserved">null</span>));
    }

    <span class="reserved">static</span> <span class="reserved">int</span> LengthSum(<span class="reserved">string</span> a, <span class="reserved">string</span>? b)
    {
        <span class="comment">// こう書いてしまうと b のところで警告。</span>
        <span class="reserved">var</span> len0 = a.Length + <span class="warning">b</span>.Length;

        <span class="comment">// これなら OK。b?. なので、b の null チェック済み。</span>
        <span class="reserved">var</span> len1 = a.Length + b?.Length ?? 0;

        <span class="comment">// こんな感じで if で null チェックしても OK。</span>
        <span class="comment">// チェック済みな個所では b. で大丈夫。</span>
        <span class="reserved">var</span> len = a.Length;
        <span class="reserved">if</span>(b != <span class="reserved">null</span>) len += b.Length;

        <span class="reserved">return</span> len;
    }
}
</code></pre>

#### Ranges

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> data = <span class="reserved">new</span>[] { 0, 1, 2, 3, 4, 5 };

        <span class="comment">// 1～2要素目。2 は exclusive。なので、表示されるのは 1 だけ。</span>
        Write(Slice(data, 1..2));

        <span class="comment">// 先頭から1～末尾から1。表示されるのは 1, 2, 3, 4</span>
        Write(Slice(data, 1..^1));

        <span class="comment">// 先頭～末尾から1。表示されるのは 0, 1, 2, 3, 4</span>
        Write(Slice(data, ..^1));

        <span class="comment">// 先頭から1～末尾。表示されるのは 1, 2, 3, 4, 5</span>
        Write(Slice(data, 1..));
    }

    <span class="comment">// 最終的に、.NET Core 3.0 には Span&lt;int&gt; に Range 型を受け取るインデクサーが入るはず。</span>
    <span class="comment">// 今はその実装がないので自前で同じ機能を作る。</span>
    <span class="reserved">static</span> Span&lt;<span class="reserved">int</span>&gt; Slice(Span&lt;<span class="reserved">int</span>&gt; data, Range range)
    {
        <span class="reserved">int</span> getIndex(<span class="reserved">int</span> length, Index i) =&gt; i.FromEnd ? length - i.Value : i.Value;
        <span class="reserved">var</span> s = getIndex(data.Length, range.Start);
        <span class="reserved">var</span> e = getIndex(data.Length, range.End);
        <span class="reserved">return</span> data.Slice(s, e - s);
    }

    <span class="comment">// 表示確認用。Span の中身を , 区切り表示。</span>
    <span class="reserved">static</span> <span class="reserved">void</span> Write&lt;<span class="type">T</span>&gt;(Span&lt;T&gt; items)
    {
        <span class="reserved">var</span> first = <span class="reserved">true</span>;
        <span class="reserved">foreach</span> (var x <span class="reserved">in</span> items)
        {
            <span class="reserved">if</span> (first) first = <span class="reserved">false</span>;
            <span class="reserved">else</span> Console.Write(<span class="string">", "</span>);
            Console.Write(x);
        }
        Console.WriteLine();
    }
}
</code></pre>

#### Null-coalescing Assignment

`x ??= y` で、`if (x == null) x = y;` の意味に。

<pre class="source" title="">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        NullCoalescingAssignment(<span class="string">"abc"</span>); <span class="comment">// "abc" が表示される</span>
        NullCoalescingAssignment(<span class="reserved">null</span>);  <span class="comment">// "default string" が表示される</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> NullCoalescingAssignment(<span class="reserved">string</span> s)
    {
        s ??= <span class="string">"default string"</span>;
        Console.WriteLine(s);
    }
}
</code></pre>

#### Alternative interpolated verbatim strings

`$@` の順序しか受け付けなかったやつが、`@$` も認めるという話。

<pre class="source" title="">
<code><span class="comment">// こっちは C# 6.0 からあるやつ。</span>
<span class="reserved">var</span> s1 = $@"\\\ {x}";

<span class="comment">// これまでは $ と @ の順番逆にできなかった。</span>
<span class="comment">// C# 8.0 から @$ でも OK。</span>
<span class="reserved">var</span> s2 = @$"\\\ {x}";
</code></pre>

### サポートに関して

[Mads (C# チームの PM)の動画](https://channel9.msdn.com/Events/Connect/Microsoft-Connect--2018/D140)の説明欄には「included in Visual Studio 2019」とか書いてあるんですが…

C# チームの中の人が gitter で[以下のようなことを言っており](https://gitter.im/dotnet/csharplang?at=5c0599bd1c439034af12ba30):

> The C# 8 language support will not RTM with initial VS2019 release. The features and the language version will be there but as "beta", meaning some breaking language changes may still occur.
> C# 8 will RTM in an update of VS2019, aligned with .NET Core 3.

VS2019 リリースの時点ではRTMにはならない。ベータ扱いで、まだ破壊的変更の可能性残る。
C# 8のRTMはVS2019のアップデートで、.NET Core 3.0とそろえてやる。

とのこと。

「機能としては乗ってるけどまだベータ」とか、混乱されそうでちょっと怖いですが。
「default」が 8.0 に切り替わらない限りには大丈夫なのかな…

## おまけ: Regexパーサー

[今年の初めに紹介した Regex パーサー](../../1/pickuproslyn0103/index.md)が Visual Studio 2019 に組み込まれたみたいです。

![Regex パーサー](../../../../../assets/media/1166/regexparser.png)

構文ハイライトが付くのと、不正な正規表現の検出、訂正をある程度やってくれるます。
