---
title: "サイトのシステム更新"
source_url: "https://ufcpp.net/blog/2015/9/replacement/"
content_type: "BlogEntry"
published_at: "2015-09-01T15:08:41"
updated_at: "2015-09-01T15:41:20"
tags: []
umbraco_id: 1791
parent_id: 1787
sort_order: 0
aliases: []
---

# サイトのシステム更新

[ufcpp.net](../../../../index.md) のシステム入れ替えてから気が付けば3・4か月ほど経過したわけですが。

いまだ微妙に直したいなと思いつつ時間が取れない部分もちらほら。

## 未

ちゃんと治せてるものもあるにはあるんですけども。

![未][1]

アイコン設定してなかったら、ロゴの真ん中あたりを自動的にとることで「未」になってたっていうやつ。ただでさえ、たまに「未確認の人」とか言われたりするのが、完全になんか未然な感じに。

まあ、たぶん、ヒツジです。干支の、ひつじ年の「未」。僕、ひつじ年生まれですし。Excel先生も、済/未 とか入れてこうとしたら自動補完で干支を出しますし。連続データ。

![干支][2]

  [1]: ../../../../../assets/media/1032/mi.png
  [2]: ../../../../../assets/media/1033/eto.png

## 数式

とかの確認で twitter で「ufcpp.net」で検索かけたりしたわけですが。ちらほら、やっぱり数式崩れを気にするコメントがありますよねぇ。

うちのサイト、現状は9割がたのアクセスがC#/.NET関連でして、その他の部分の移行作業は多少手を抜いたりしていたり。

数式が結構残念なのも、C#/.NET関連のページにはほとんどないからなんですよねぇ。移行作業が大変な割にPVそんなにないので、コスト パフォーマンス的に見合わないという判断。

↓こういうのとか(実際こういう表示崩れを起こしてるページが結構)。

<span class="math">
<table class="sigma" summary="limitation"><tr><td><span class="normal">lim</span></td></tr><tr><td class="sigmasub">x → 0</td></tr></table>
g<span class="paren" style="font-size:em;">(</span>x<span class="paren" style="font-size:em;">)</span>
＝ 0
</span>

これは、↓みたいな数式を表示したくて書いてたやつなんですが。

![word math](../../../../../assets/media/1034/math.png)

まあ、大昔、CSSで必死に調整してそれっぽく見えるように出してたんで、当時と今とでブラウザーのレンダリングが違いすぎて。

一応、新システムは [MathJax](http://genkuroki.web.fc2.com/) 対応を入れてもらってるので、ちまちまとMathJax対応形式への変換をかけていけば表示できるはず。

例えば、以下のようなMathMLを書けば、

```xml
<math xmlns="http://www.w3.org/1998/Math/MathML">
    <mrow>
        <mrow>
            <munder>
                <mrow><mi mathvariant="normal">lim</mi></mrow>
                <mrow><mi>x</mi><mo>→</mo><mn>0</mn></mrow>
            </munder>
        </mrow>
        <mo>⁡</mo>
        <mrow>
            <mi>g</mi><mfenced separators="|"><mrow><mi>x</mi></mrow></mfenced>
        </mrow>
    </mrow>
    <mo>=</mo>
    <mn>0</mn>
</math>
```

以下のような数式が一応表示される。

<math xmlns="http://www.w3.org/1998/Math/MathML"><mrow><mrow><munder><mrow><mi mathvariant="normal">lim</mi></mrow><mrow><mi>x</mi><mo>→</mo><mn>0</mn></mrow></munder></mrow><mo>⁡</mo><mrow><mi>g</mi><mfenced separators="|"><mrow><mi>x</mi></mrow></mfenced></mrow></mrow><mo>=</mo><mn>0</mn></math>

ただ、作業量が多すぎて…

バイトしてくれる人とかいないかな… 全体で何式くらいあるかとか、1式(あるいは1ページ)いくらなら「よい時給のバイト」になるかとか計算してみるかなぁ。
