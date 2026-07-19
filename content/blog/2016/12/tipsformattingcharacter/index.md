---
title: "小ネタ フォーマット文字識別子"
source_url: "https://ufcpp.net/blog/2016/12/tipsformattingcharacter/"
content_type: "BlogEntry"
published_at: "2016-12-10T00:23:13"
updated_at: "2016-12-27T14:32:56"
tags: []
umbraco_id: 1989
parent_id: 1969
sort_order: 9
aliases: []
---

# 小ネタ フォーマット文字識別子

いい加減、小ネタらしい小ネタを書かないとタイトル詐欺臭いのでほんとに小ネタを。

C#では、以下のようなコードが書けたりします。変数`ab`を用意して、`a\u200db`って変数に書き込むと、`ab`の値が変わるという。
要するに、この2つは識別子としては同一扱いされます。

<pre class="source" title="Zero Width Joiner 識別子">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> ab = 0;
        a\u200db = 1; <span class="comment">// ab と同じ扱い。\u200d は Zero Width Joiner</span>
        <span class="type">Console</span>.WriteLine(ab); <span class="comment">// 1</span>
    }
}
</code></pre>

この挙動を説明するには、以下の2つの仕様が出てきます。

- Unicode エスケープ シーケンス
  - 参考: [2.4.1 Unicode 文字エスケープ シーケンス](https://msdn.microsoft.com/ja-jp/library/aa664669.aspx)
- 識別子におけるフォーマット文字の挙動
  - 参考: [2.4.2 識別子](https://msdn.microsoft.com/ja-jp/library/aa664670.aspx)

1つのUnicodeエスケープ シーケンスは、`\u`に続けて4桁の16進数を打つか、`\U`に続けて8桁の16進数を打つと、その番号に対応したUnicode文字に変換されるというものです。このエスケープ シーケンスは、文字列リテラルの外、どこででも有効です。例えば、以下のようなことも可能。aの文字は、UnicodeではU+61です。

<pre class="source" title="a と \u0061 は同じ">
<code><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> a = 0;
        \u0061 = 1;
        <span class="type">Console</span>.WriteLine(a);
    }
}
</code></pre>

もう1つは、フォーマット文字は識別子に含められるけど、除外して考えるという仕様。
フォーマット文字ってのは、文字を描画方法とかを指定するための不可視文字で、例えば以下のようなものがあります。

- Zero Width Joiner (U+200D): その左右の文字が不可分なことを表す。ゼロ幅接合子。略称 ZWJ。
- Left-to-Right Mark (U+200E): 文字を左から右に向かって描画すべきということを表す
- Right-to-Left Mark (U+200F): 同上、右から左

一部の自然言語でこの手の制御が必要だけども、見えない文字だからこの文字のあるなしで別識別子にはしたくないっていうことでしょう。

これら2つを組み合わせた結果が冒頭のコードです。`a\u200db`は、`a`と`b`の間にZWJを挟んだ状態で、結果的に、識別子としてはZWJが無視されて、`ab`として扱われます。

まあ、見えない文字とか無視すべきですよね。
普通、見えない文字はそもそも識別子として使えなくしてるものなんですが、Right-to-Left Markとかは、アラビア語プログラミングとかすると使うかもしれないですもんねぇ。「使う」というか、もしかしたらエディターによって勝手に挿入されるかもしれず。
その場合、無視すべき、ということなんでしょう。

[無視しないプログラミング言語もありますが](http://swiftlang.ng.bluemix.net/#/repl/584acaa094b17a360a54c2bc)…

## Unicode Consortium で規定

この挙動、どうも、[Unicode Consortium](http://www.unicode.org/)のレポートに基づいてるみたいです。

[Unicode Technical Report #15](http://www.unicode.org/reports/tr15/tr15-18.html)の、[Annex7: Annex 7: Programming Language Identifiers](http://www.unicode.org/reports/tr15/tr15-18.html#Programming%20Language%20Identifiers)のところ。
プログラミング言語の識別子に使える文字はどうあるべきか、みたいな話が結構詳細に書かれています。
これに沿っている言語は他にもありそうなので、試してみるといいかも。
