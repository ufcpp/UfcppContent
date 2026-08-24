---
title: "見た目で区別できない変数"
source_url: "https://ufcpp.net/blog/2020/5/variationselectoridentifier/"
content_type: "BlogEntry"
published_at: "2020-05-18T00:02:03"
updated_at: "2020-05-19T19:40:45"
tags: []
umbraco_id: 2295
parent_id: 2290
sort_order: 3
aliases: []
---

# 見た目で区別できない変数

ちょっとしたネタ投稿をしてみたわけですが。

[https://paiza.io/projects/WMu7W_PPTqkZRV5iGztJtg](https://paiza.io/projects/WMu7W_PPTqkZRV5iGztJtg)

```java {title="見た目で区別できない変数"}
import java.util.*;

public class Main {
    public static void main(String[] args) throws Exception {
        int a = 1;
        int a︀ = 2;
        int a︁ = 4;
        int a︂ = 8;
        int a︃ = 16;
        int a︄ = 32;
        int a︅ = 64;
        int a︆ = 128;
        int a︇ = 256;
        int a︈ = 512;
        int a︉ = 1024;
        int a︊ = 2048;
        int a︋ = 4096;
        int a︌ = 8192;
        int a︍ = 16384;
        int a︎ = 32768;
        int a️ = 65536;

        System.out.println(a + a︀ + a︁ + a︂ + a︃ + a︄ + a︅ + a︆ + a︇ + a︈ + a︉ + a︊ + a︋ + a︌ + a︍ + a︎ + a️);
    }
}
```

ぱっと見は同じ「`a`」という名前の変数が17個並んでいますが、これ、全部別変数です。
名前被りでコンパイル エラーになったりもしません。

まあ、種を明かすと[異体字セレクター](https://ja.wikipedia.org/wiki/%E7%95%B0%E4%BD%93%E5%AD%97%E3%82%BB%E3%83%AC%E3%82%AF%E3%82%BF)っていう不可視の文字をくっつけてるだけで、この異体字セレクターが16種類あるので(何もつけてない1個と併せて)17個の「`a`」を作れているという状態。

## 元々同じ見た目で別の文字

特に難しいことをしなくても、元々、見た目が同じ別の文字は結構あります。
分かりやすい例で言うと、

- [A (U+0041)](https://www.fileformat.info/info/unicode/char/0041/index.htm) : ラテン文字の大文字の a
- [Α (U+0391)](https://www.fileformat.info/info/unicode/char/0391/index.htm) : ギリシャ文字の大文字の α
- [А (U+0410)](https://www.fileformat.info/info/unicode/char/0410/index.htm) : キリル文字の大文字の а

とか。
全部出自が同じだし、発音も同系統(「あ」系統)なので似てて当然なんですが。
「出自が同じで字形も近い文字をまとめるかどうか」と言うのは結構根が深い問題になります。
Unicode ではこれらの文字を別文字扱いしています。

(別の文字なので当然字形が違ってもいいんですけども、
たいていのフォントでは同じ字形で表示されて区別がつかないと思います。)

こいつらを使えば、割と簡単に「ぱっと見で区別がつかない変数」を作れます。

```csharp {title="ラテン文字、ギリシャ文字、キリル文字な変数"}
using System;
 
class Program
{
    static void Main()
    {
        var A = 1;
        var Α = 2;
        var А = 4;
 
        Console.WriteLine(A + Α + А); // 7
        Console.WriteLine((int)nameof(A)[0]); // U+0041 (10進 65)
        Console.WriteLine((int)nameof(Α)[0]); // U+0391 (10進 913)
        Console.WriteLine((int)nameof(А)[0]); // U+0410 (10進 1040)
    }
}
```

今時、意図的に排除<sup>※</sup>でもしていない限り non-ASCII な文字も変数名に使えるので、たいていの言語で同じことができると思います。
まあ、わざわざ non-ASCII な文字でソースコードを書く人も少ないので、悪意を持ってやらない限りは起きないと思いますが。

(<sup>※</sup> 例えば [Rust](https://doc.rust-lang.org/reference/identifiers.html) は ASCII 文字しか受け付けない意思を感じる。)

## アクセント

もう少し悪意を高めてみます。

Unicode はそれ以前の各国個別の文字コードとの互換性のために、
いくつか、全く同じ文字を別のコードで表現できてしまいます。
分かりやすいのがアクセント記号の類([ダイアクリティカルマーク](https://ja.wikipedia.org/wiki/%E3%83%80%E3%82%A4%E3%82%A2%E3%82%AF%E3%83%AA%E3%83%86%E3%82%A3%E3%82%AB%E3%83%AB%E3%83%9E%E3%83%BC%E3%82%AF))で、
例えば、「á」の文字は以下の2通りの表現方法があります。

- á ([U+00E1](https://www.fileformat.info/info/unicode/char/00E1/index.htm)) : 元々西欧向け文字コードに入っていた文字
- á ([U+0061](https://www.fileformat.info/info/unicode/char/0061/index.htm) [U+0301](https://www.fileformat.info/info/unicode/char/0301/index.htm)) : 無印の a の上に acute アクセントを結合

Unicode にはこの手の「同じ文字の別表現」を互いに変換するための仕様も入っているんですが、
その手の変換まで書けるプログラミング言語はまずありません。

結果的に、この手の文字も「ぱっと見で区別がつかない変数」に使えます。

```csharp {title="アクセント記号を含む変数"}
using System;
 
class Program
{
    static void Main()
    {
        var á = 1;
        var á = 2;
 
        Console.WriteLine(á + á); // 3
        Console.WriteLine((int)nameof(á)[0]); // U+00E1 (10進 225)
        Console.WriteLine((int)nameof(á)[0]); // [0] は `a` が拾える。U+0061 (10進 97)
        Console.WriteLine(nameof(á).Length); // 実は2文字なので Length が 2
    }
}
```

アクセント記号(この例で言うと U+0301)を変数名に使えるかどうかはプログラミング言語によります。
例えば [Go は letter (a とかの可読文字)しか受け付けません](https://golang.org/ref/spec#Identifiers)。
[Java](https://docs.oracle.com/javase/jp/7/api/java/lang/Character.html#isJavaIdentifierPart(int)) とか [C#](https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/language-specification/lexical-structure#identifiers) とかは、2文字目以降にはアクセント記号の類を受け付けます。

## 0幅不可視文字

Unicode にはいくつか、以下のような、0幅でまったく見えない文字が存在します。

- [Zero Width Joiner (U+200D)](https://www.fileformat.info/info/unicode/char/200d/index.htm) : 前後の文字をくっつける(改行とかで分割されることが絶対起きないようにするための文字)
- [Right-to-Left Mark U+200F](https://www.fileformat.info/info/unicode/char/200f/index.htm) : この文字以降、右書きにする

まあでも、この手の文字はほとんどのプログラミング言語が受け付けません。
Java と C# はちょっと特殊で、「[受け付けるけど完全に無視](https://docs.oracle.com/javase/jp/7/api/java/lang/Character.html#isIdentifierIgnorable(int))」という挙動をしたりします。

いずれにしても、0幅不可視な文字がソースコードに紛れていて困ることはほとんどない…
と思っていたんですけども…

## 異体字セレクター

そういえばあったわ、Java と C# が変数名として受け付けて、無視もせず、0幅不可視の文字…
というのが[異体字セレクター](https://ja.wikipedia.org/wiki/%E7%95%B0%E4%BD%93%E5%AD%97%E3%82%BB%E3%83%AC%E3%82%AF%E3%82%BF)。

[数字の 0 を斜線付きで表示](https://ja.wikipedia.org/wiki/%E6%96%9C%E7%B7%9A%E4%BB%98%E3%81%8D%E3%82%BC%E3%83%AD)したりするために使う文字なんですけども、カテゴリーが [Nonspacing Mark](https://www.compart.com/en/unicode/category/Mn) (アクセント記号とかと同じカテゴリー)だったりします。

ほとんどの文字が異体字なんて持ってないので、異体字セレクターがくっついていても、テキスト レンダリング上は単に無視されます。
要するに、実質、0幅不可視文字。
なのに、カテゴリー的には変数として受け付けられるし、別の文字として扱われます。

このうち U+FE00〜U+FE0F の16文字を使って書いたコードが冒頭の Java コードになります。

再掲: [https://paiza.io/projects/WMu7W_PPTqkZRV5iGztJtg](https://paiza.io/projects/WMu7W_PPTqkZRV5iGztJtg)

ちなみにこの文字、後ろにいくらでもつなげられるので、16個と言わず、文字列長を増やしていけばいくらでも「`a`」を増やせます。

## ちなみに、背景

これ、[単体テストのテストケース](https://github.com/ufcpp/roslyn/commit/94557f7e5bc0804c308b0af98f734e1d3f99592a#diff-92ef870ff9496ca561e057580a939742R24-R55)を検討しているときに思いついたものの、
以下の2つの理由で没ったものだったりします。

- 簡単に入力できない
  - 普通の状況で絶対に現れない文字列
  - 日本語 IME を使って入力できちゃう「葛󠄀」(葛󠄀城市の葛󠄀。葛飾区の葛の異体字。U+845B U+E0100)の方が適任
- プログラム的には難しい処理をしていない
  - 厄介なのは人の目に見えないという点だけ
  - プログラム的には  á ([U+0061](https://www.fileformat.info/info/unicode/char/0061/index.htm) [U+0301](https://www.fileformat.info/info/unicode/char/0301/index.htm)) と同じパターンなので、á だけで十分

というか、「葛󠄀」と「葛」は区別されるのかどうかが気になったのが先で、
異体字セレクター (U+E0100 とか)のカテゴリーがアクセント記号の類と同じ(Nonspacing Mark)なことに気付いたというのが実際の流れ。
(しかも、この [Nonspacing Mark カテゴリー](https://www.fileformat.info/info/unicode/category/Mn/list.htm)の文字一覧を眺めてみてるに、完全に不可視なのは異体字セレクターだけっぽい。)

ちなみに、どうやって上記 Java コードを書いたかと言うと…
[`"a\uFE00a\uFE01a\uFE02a\uFE03..."` みたいなエスケープ シーケンスを使ってプログラムで出力して](https://paiza.io/projects/MN1P7UTSayHYhpkJ6Reuow)、それをコピペして書いています。
