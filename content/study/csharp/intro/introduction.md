---
title: "はじめに"
source_url: "https://ufcpp.net/study/csharp/intro/introduction/"
content_type: "Article"
published_at: "2000-12-24T00:00:00"
updated_at: "2008-06-28T00:00:00"
tags: []
umbraco_id: 1171
parent_id: 1170
sort_order: 0
aliases:
  - "/csharp/intro/introduction/"
  - "/csharp/introduction"
  - "/csharp/introduction.html"
  - "/study/csharp/introduction"
  - "/study/csharp/introduction.html"
---

# はじめに

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
* C# はいい言語ですよ。

* C# の文法を知りたいだけなら Microsoft の公式文書だけで十分。
    * ここではもう一歩踏み込んで「そもそもなぜそういう機能が必要なのか」という部分に焦点を当てた説明をします。



* プログラミング言語を覚えることは手段であって目的ではない。
    * なるべく簡単な言語を使った方がいいです。

    * 大切なのは「難しい言語を使えること」ではなく、「どんな言語を使ってでも、作りたいものを作れること」。

    * 楽に、いろんなことを幅広く実現できるのが C# ！





##<a id="sec-generated-title-2"></a> <a id="about"></a>当コンテンツの内容について
.NET Framework Technical Preview 版配布開始から約2年、
2001年末にようやく .NET Framework 正式版が発表されました。
また、2002年2月には日本語版も配布開始され、同4月には Visual C# .NET 日本語版も発売されました。
ここに来てようやく日本でも C# や .NET が話題になり始め、雑誌やウェブ上の掲示板などでの話題も盛り上がってきました。
そして、
C# は「C++ の柔軟性と Visual Basic(VB) の生産性を併せ持つ言語」として開発されていることもあり、
今まで VB を用いてプログラム開発を行っていた人や、
新規にプログラミングを始めようという人が C# の勉強をはじめようとしています。

Microsoft の提供する C# のドキュメントでは、
C# では「なにができるか」「どうすればできるか」という部分については詳細まで書かれていて非常に分かりやすいのですが、
「なぜこういった機能が必要なのか」という部分までは書かれていません。
そのため、C++ や Java などについての知識が十分にあれば、
Microsoft の提供するドキュメントだけでも容易に C# について理解することができるのですが、
プログラミングの初心者やこれからプログラミングを始めようという人にとっては、
このドキュメントだけではプログラミングの勉強をするには不十分です。

また、C# に関する解説書籍、ウェブサイトでも、
C 言語や Java などの知識を有することを前提とし、
1から C# を学ぶにはハードルの高いものが多いです。

そこで当サイトでは、これからプログラミングを始めようという人が C# の勉強をできるように、
C# で「何ができるか」「どうすればできるか」そして「なぜそのような機能が必要なのか」という部分まで説明していこうと思っています。

あるいは、プログラミング初心者の方でなくても、
C++ や Java と比べて追加された機能や、逆に削除された機能に関して、
「なぜ追加・削除されたのか」が分かってもらえるような説明を心がけるつもりです。


###<a id="sec-generated-title-3"></a> <a id="whycs"></a>C# によるプログラミング入門
よく訊かれる質問、
特に、主として C 言語を利用しているプログラマの方からの質問なのですが、
「C# は初入門の言語として適切か」と言うものを受けることがあります。

多いのは、
「C 言語 → C++ というように段階を経て学んでいくべきなのではないか」という意見です。
「Java や C# だと、class や static など、最初に説明しないといけない“おまじない”が多いし・・・」とか、
「Java や C# などのオブジェクト指向言語を学ぶにしても、
その基本は構造化プログラミング言語だし」というのが、これらの意見の理由です。

この質問に対する（僕個人の私的な）答えですが、
結論から言うと、初めから C# を学ぶ方がいいと思います。

例えば、C 言語なら最初から理解するのが難しい“おまじない”の部分がないかと言うとそんなはずもなく、
\#include などはやはり“おまじない”ですし、
Java や C# などの新参の言語は、構造化プログラミング言語としても見た場合でもかなり優秀な言語です。

むしろ、C 言語など、長い歴史を持つ言語は、
昔からの悪習を引きずっていて、
下手に学ぶと悪い癖が付いたまま抜けなくなる危険性の方が高いです。

習得難易度の低さの面からも、
悪習を身に着けないためという意味でも、
最初に C#、そして、必要に応じて C 言語などの抽象度の低い言語を学ぶ方がいいと思います。
（ちなみに、参考として抽象度の低い言語を学ぶという発想なら、
アセンブリ言語の勉強するのもお勧め。）

（2008年追記：
最近、会社の同僚が「C++ ロストジェネレーション論」というのを唱え始めた。
C++ 全盛期、Java 登場以前の90年代にプログラミングを始めた人は、
C++ のせいで過剰にプログラミングを怖がったり、
過去の悪習にとらわれて変な癖がついたりしてかわいそう。
まるで就職氷河期の失われた世代のようだという話。
あながちトンデモな話でもない気がする。
）


##<a id="sec-generated-title-4"></a> <a id="content"></a>サンプル コードについて
##### <a id="sec-generated-title-5"></a>言語選択
ページ中のサンプル コードを C# 以外の言語にも対応させてみました。
VB（Visual Basic）、F#、C++/CLI などに対応します。
（タブ名が C++ になっているものは、ネイティブ（のみ）の C++ ではなく、C++/CLI だと思ってください。）

注意: 段階的に書き換え中です。
参考: 「[C#のプログラムの基本構造](../start/st_basis.md)」。


<div class="tab-container">
<ul>
	<li>C#</li>
	<li>VB</li>
	<li>F#</li>
	<li>C++</li>
</ul>
<div>

<pre class="source" title="最も簡単なC#プログラム" lang="C#">
<code><span class="comment">// C#</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"Hello World"</span>);
</code></pre>


</div>
<div>

<pre class="source" title="" lang="VB">
<code><span class="comment">' Visual Basic</span>
<span class="type">Console</span>.WriteLine(<span class="literal">"Hello World"</span>)
</code></pre>


</div>
<div>

<pre class="source" title="" lang="F#">
<code><span class="comment">// F#</span>
Console.Write <span class="literal">"Hello World"</span>
</code></pre>


</div>
<div>

<pre class="source" title="" lang="C++">
<code><span class="comment">// C++/CLI</span>
Console::WriteLine(L<span class="literal">"Hello World"</span>);
</code></pre>


</div>
</div>



##### <a id="sec-generated-title-6"></a>凡例
当コンテンツ中でサンプル コードは以下のようなスタイルで書いています。

<pre class="source" title="サンプルコードの例" lang="">
<code><span class="comment">// サンプルコード</span>
<span class="reserved">if</span>(<span class="input">条件文</span>)
{
  <span class="reserved">int</span> n = 0;
  <span class="reserved">string</span> s = <span class="literal">"sample"</span>;
}
</code></pre>


<span class="reserved">C# のキーワード</span>や、<span class="comment">コメント</span>、<span class="string">文字列</span>は色付けして強調表示してあります。
また、<span class="input">条件文</span>などその時々に応じて適当に書き換える必要のある部分は太字で強調表示してあります。

また、ユーザーからの入力や、サンプルコードの出力は以下のようなスタイルで書いています。

<pre class="console" title="サンプルコードの出力例">
sample input/output
<span class="comment"># ↓ この行に対するコメント</span>
<span class="input">ユーザーからの入力</span>
</pre>


サンプルコードとは異なる背景色で囲み、<span class="input">ユーザーからの入力</span>は太字で強調表示してあります。

さらに、本文中、サンプルコード中を問わず、<em>重要な部分</em>は背景色を色付けし強調表示してあります。


##<a id="sec-generated-title-7"></a> <a id="bsmark"></a>\ 記号についての注意
￥記号問題
（同じ文字コードに、日本では ￥、英語圏では ＼ の半角が割り当たっている）
のせいで、
「\」が「￥」の半角で表示される場合と「＼」の半角になる場合があるようですが、
どちらも同じものです。

￥記号問題に関する詳細は、
[Wikipedia の￥記号の記事](http://ja.wikipedia.org/wiki/%EF%BF%A5%E8%A8%98%E5%8F%B7)を参照してください。

##<a id="sec-generated-title-8"></a> <a id="variants"></a>表記ゆれ
このサイトはもう15年以上にわたって更新していますし、
C# 関連の用語の和訳が正式に決まる前から記事を書くことが多いので、
結構表記ゆれが多いです。
その点ご容赦願います。

例えば以下のようなものは表記ゆれしています

- expression tree: 式ツリー、式木
- statement: ステートメント、文
- string interpolation: 文字列補間、文字列挿入

極力元の英単語を併記するようにしているので、もし思った通りのものが見つからなかった場合、その元の英単語での検索も試みてください。

##<a id="sec-generated-title-9"></a> <a id="availability"></a>当コンテンツ複製・利用について
当コンテンツの内容は個人的・教育的目的での複製・利用はご自由にして頂いて構いません。
利用に際して、誤字等を見つけた場合や、ご意見・ご要望・ご不満等がございましたら、
その内容を教えていただければこちらとしても助かりますので、
そうして頂きますようにお願い致します。
