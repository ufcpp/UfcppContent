---
title: "UAX31: Unicode Identifier の話"
source_url: "https://ufcpp.net/blog/2021/2/uax31/"
content_type: "BlogEntry"
published_at: "2021-02-12T00:52:24"
updated_at: "2021-02-12T01:06:50"
tags: []
umbraco_id: 2328
parent_id: 2327
sort_order: 0
aliases: []
---

# UAX31: Unicode Identifier の話

今日はまた[去年の作業が元ネタ](https://github.com/ufcpp-live/UfcppLiveAgenda/issues/6)で、プログラミング言語の識別子に使える文字に関する話です。

## レターか数字

<em>「1文字目にはアルファベットか `_`、2文字目以降にはそれに加えて数字を使えます。」</em>

30年くらい前にはこれが「プログラミング言語の識別子(変数名など)に使える文字列」の定義でした。
`_` の部分はプログラミング言語次第ですが、「1文字目にアルファベット、2文字目以降に数字」の部分は結構いろんな言語でそうだったんじゃないかと思います。

まあ、昔のプログラミング言語は [ASCII コード](https://ja.wikipedia.org/wiki/ASCII)で書く物だったので、上記の条件は `[a-zA-Z]` とか `[0-9]` みたいな正規表現で書けたんですが。
[Unicode](https://ja.wikipedia.org/wiki/Unicode) の時代になると「アルファベットだけでいいのか」とか「アルファベットって何だ」という話になります。

### レター

まず、「アルファベット(alphabet)」というと母音と子音が分かれてる文字のことで、ラテン文字、ギリシャ文字、キリル文字なんかのことを指します。アラビア文字みたいに母音しか表記しないやつはアブジャド(abjad)、漢字は表意文字(ideogram)、ひらがな・カタカナみたいなのは音節文字(syllabary)と呼ぶらしく、「1文字目はアルファベット」と言ってしまうと一部の自然言語に偏ってしまいます。

Unicode 的にこの辺りの「記号や数字じゃない文字全般」を指してレター(letter)と呼ぶので、冒頭の条件は以下のように書き換わります。

<em>「1文字目にはレターか `_`、2文字目以降にはそれに加えて数字を使えます。」</em>

### レターとは…

Unicode では文字ごとにカテゴリーが決められているので「ある文字列がレターかどうか」を調べるのは簡単…

かというとそうでもなくて、確かに1文字1文字がレターかどうかを判定するのは素直なんですが、2文字以上がくっついて1文字になることがあってそれが面倒だったりします。

例えば「ら゚」の文字。
現代日本語では普通は使わない文字ですが、R 音の「ら」と L 音の「ら」を区別するために L 音の方を「ら゚」と書く用法が一時期あったそうです。今現在ほとんど流通していないレア文字なので、この文字を Unicode 1文字で表す方法はない(符号が割当たってない)んですが、普通の「ら」(U+3089)の後ろに[半濁点(U+309A)](https://www.compart.com/en/unicode/U+309A)を並べることで「ら゚」を表せます。

ちなみに、Unicode の文字カテゴリー的には

- ら(U+3089) は Letter, Other (Lo)
- ゚゚   (U+309A) は  Mark, NonSpacing (Mn)

となっています。マーク(mark)ってものが出てきましたが、「レターにくっついて修飾する系の文字」は大体この分類です。日本語の濁点・半濁点以外にも、ラテン文字に対する[ダイアクリティカルマーク](https://ja.wikipedia.org/wiki/%E3%83%80%E3%82%A4%E3%82%A2%E3%82%AF%E3%83%AA%E3%83%86%E3%82%A3%E3%82%AB%E3%83%AB%E3%83%9E%E3%83%BC%E3%82%AF)もマークの類です。

日本語とかラテン文字の場合はこういうレア文字を除いてほとんどの文字が、
わざわざレター + マークの組み合わせを使わなくても大体 Unicode の符号が割当たっているのでそこまで困りません。
一方で、[タイ文字](https://th.wikipedia.org/wiki/%E0%B8%AD%E0%B8%B1%E0%B8%81%E0%B8%A9%E0%B8%A3%E0%B9%84%E0%B8%97%E0%B8%A2)(อักษรไทย みたいなの)とか[サンスクリット](https://hi.wikipedia.org/wiki/%E0%A4%B8%E0%A4%82%E0%A4%B8%E0%A5%8D%E0%A4%95%E0%A5%83%E0%A4%A4_%E0%A4%AD%E0%A4%BE%E0%A4%B7%E0%A4%BE)(संस्कृत みたいなの)は普通に日常的に使う文字がレター + マーク構成になっています。

ということで、「人の認識上」でレターっぽいものは受け付けたいとなったとき、
「Unicode の処理の都合上」では「1文字目にレター、2文字目以降にマーク」になります。
その結果、冒頭の条件はさらに以下のように書き換わります。

<em>「1文字目にはレターか `_`、2文字目以降にはそれに加えて数字とマークを使えます。」</em>

## Unicode カテゴリーを使ったちゃんとした定義

この「1文字目にはレター、2文字目以降にはそれに加えて数字とマーク」という方向性、たぶん最初に採用したのは Java ですかね。[`isJavaIdentifierStart`](https://docs.oracle.com/javase/7/docs/api/java/lang/Character.html#isJavaIdentifierStart(char))、[isJavaIdentifierPart](https://docs.oracle.com/javase/7/docs/api/java/lang/Character.html#isJavaIdentifierPart(char)) というメソッドで判定してるみたいなんですが、ここに並んでいる条件がおおむね「レターと数字とマーク」です。

[C# の場合](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/lexical-structure#identifiers?WT.mc_id=DT-MVP-4028921)は Unicode カテゴリーがそのまま列挙されていて、

- 1文字目: Lu, Ll, Lt, Lm, Lo, Nl
- 2文字目以降: 上記に加えて、Mc, Nd, Pc, Cf

みたいになっていますが、これがだいたい「レターと数字とマーク」になります。

### カテゴリーの安定性問題

Unicode 曰く、「カテゴリーはできる限り安定させたいけど希に変わることがある」とのこと。

実際、日本語だと以下の文字のカテゴリーに変更がありました。

- ゛ と ゜ (U+309B と U+309C、単独の濁点・半濁点)
  - これとは別に、マーク(直前のレターにくっつく)扱いの濁点・半濁点がある(U+3099 と U+309A)
  - 昔はマークの方の濁点・半濁点との混同があった
  - 今は Symbol, Modifier で識別子として使えない
- ・ (U+30FB、[中黒](https://ja.wikipedia.org/wiki/%E4%B8%AD%E9%BB%92))
  - 昔は Punctuation, Connector で識別子の2文字目以降に使えた
  - 今は Punctuation, Other で識別子として使えない

特に・(U+30FB) の変更は比較的新しい話で、
Java 7 (2011年)とか C# 6.0 (2015年) の頃に「今までコンパイルできていたコードが急にコンパイルできなくなった」みたいな騒ぎがありました。

## UAX31

Unicode、「何番のコードに何の文字を割り当てるか」みたいな基本的な定義に加えて、例えば以下のような様々なレポートを出していたりします。

- [右書き・左書き](https://unicode.org/reports/tr9/)
- [文字列の照合順序](https://www.unicode.org/reports/tr10/)
- [全角・半角](https://www.unicode.org/reports/tr11/)

昔は Unicode Technical Report、今は Unicode Standard Annex (付録)みたいに呼んでいるようで、
後者は UAX と略したりします。

で、その中に「識別子として使える文字」の話もあります。通称 UAX31。

- [UNICODE IDENTIFIER AND PATTERN SYNTAX](https://www.unicode.org/reports/tr31/)

あくまで recommended defaults (推奨される既定動作)であって何か拘束力のある標準仕様ではないんですが、
「迷うくらいならこれに従っておけ」くらいの材料にはなります。

概ね Java/C# のものを踏襲していそうな感じで、以下の条件がベースです。

- 1文字目: Lu, Ll, Lt, Lm, Lo, Nl
- 2文字目以降: 上記に加えて、Mc, Nd, Pc, Cf

少なくとも[2003年のバージョン1](https://www.unicode.org/reports/tr31/tr31-1.html)はほぼこの条件。
違いというか、先ほどのカテゴリーの安定性問題を避けるためにいくつか付帯説明があります。

- 「[Alternative Identifier](https://www.unicode.org/reports/tr31/tr31-1.html#Alternative_Identifier_Syntax)」(代替案)として、一部の記号だけ避けてほぼすべての文字を識別子として使える案もある
- 後方互換性のため、4文字ほど、カテゴリー変更があった文字に追加で Other_ID_Start という属性を持たせて識別子として使えるようにしている

[2020年のバージョン](https://www.unicode.org/reports/tr31/tr31-33.html)ではもう少し複雑になっていますが、大体安定性のためです。
Other_ID_Start だけでは文字のカテゴリー変更に対応できなかったみたいで、追加で Other_ID_Continue という属性が定義されています。
また、Alternative Identifier 向けに定義している Pattern_Syntax (プログラミング言語の構文に使いそうな記号類)、Pattern_White_Space  (同、空白文字の類)を避けることが明言されています。
これら Other_ID_Start 、Other_ID_Continue、Pattern_Syntax、Pattern_White_Space は、カテゴリーと違って、今後破壊的変更を起こさないように運用するとのこと。

また、初期バージョンで「Alternative Identifier」と呼んでいたものは、現在は「[Immutable Identifier](https://www.unicode.org/reports/tr31/tr31-33.html#Immutable_Identifier_Syntax)」という呼び名に変わっています。
名前通り、Unicode のバージョンによらず常に不変な保証があります。
ただ、何の文字でも受け付けすぎるのであまり推奨はされていません。

## C++ の UAX31 採用

Java と C# は、Unicode のカテゴリー変更を受け入れる方向性になっています。
中黒(U+30FB)のカテゴリー変更のとき、そこまで大きな問題にしなかったので。
UAX31 の[オプションとして使ってもいい文字](https://www.unicode.org/reports/tr31/tr31-33.html#Table_Optional_Medial)のテーブルに中黒(KATAKANA MIDDLE DOT)が追加されたりはしましたが、
それだけです。

これに対して、C++ (かつては ASCII 文字しか受け付けなかった)が Unicode 識別子に対応しようとした際には UAX31 に従おうという話になったそうです。

### UAX31 Immutable Identifier

ただ、問題は UAX31 の安定性。
ちょうど Java が中黒(U+30FB)問題を踏んだ時期だったので、カテゴリー変更を警戒して、
Alternative Identifier (現在の Immutable Identifier)を採用しようとしました
([2010年発案](http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2010/n3146.html))。
(結局標準化はしてなさそう？ですが)いくつかの C++ コンパイラーはこの案で実装あり。
例えば[Clang は 3.3 で](https://releases.llvm.org/3.3/tools/clang/docs/ReleaseNotes.html)、
[gcc は 10 で](https://gcc.gnu.org/gcc-10/changes.html)対応。

と言うことで現在、たいていの C++ コンパイラーで以下のコードがコンパイルできます。

```cpp {title="Emoji C++"}
#include <iostream>
 
int main()
{
    int 😱 = 2;
    int 😇 = 3;
    int 🥺 = 5;
    std::cout << 😱 * 😇 * 🥺 << std::endl;
}
```

### UAX31 Default Identifier

その後、前述のとおり UAX31 にも手が入っていて、安定性が改善しました。

と言うことで改めて、C++ の標準仕様として、C++ の識別子を UAX31 に従うようにしようという話になっているみたいです。
今度はちゃんと Default Identifier (Java とか C# とかに近いやつ)で。

- [C++ Identifier Syntax using Unicode Standard Annex 31 (P1949R6)](http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2020/p1949r6.html)

[現状、賛成多数](https://github.com/cplusplus/papers/issues/688)で、C++ 23 で採用されそうな雰囲気。

ということで、Immutable Identifier (絵文字を含んでる)から Default Identifier (絵文字を含んでいない)に<em>変更</em>されそうです。前節の絵文字ソースコードは C++ 23 から<em>コンパイルできなくなる</em>予定。

### Immutable Identifier から Default Identifier への変更

これまで使えてた文字が使えなくなる(破壊的変更する)わけで、
この提案にはそれ相応の説得材料が必要になります。
なので、[P1949R6](http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2020/p1949r6.html) には結構詳細に、これまで(Immutable Identifier)の問題とか、変更の影響がまとめられています。

その中から2つほど紹介。

- 絵文字が使えなくなる問題
- 元から一部の絵文字は使えなかった問題

#### 絵文字が使えなくなる問題

まんま抜粋。

```cpp {title="Throwing Pile of Poo"}
class 💩 : public std::exception { };
```

> Throwing “PILE OF POO” becomes ill-formed. Conference slide-ware will be less entertaining.

> (絵文字が使えなくなることで) うんこ投げるコードは受け付けなくなった。スライド映えしなくなる。

基本的に破壊的変更をよしとしない C++ で、破壊的変更が賛成多数になるくらいですから…

絵文字識別子の扱い、やっぱり「スライド映え」ですよね。

#### 元から一部の絵文字は使えなかった問題

Immutable Identifier は

- 所定の文字を1つ1つリストアップして識別子として使えないように禁止してる
  - 基本的には記号類は禁止
- [サロゲートペア](https://ja.wikipedia.org/wiki/Unicode#%E6%8B%A1%E5%BC%B5%E9%A0%98%E5%9F%9F)は無条件に許可

みたいなことをしているので…
以下のように、使える絵文字と使えない絵文字があります(not valid コメントの行のものだけダメ)。

```csharp {title="Immutable Identifier での絵文字"}
int ⏰ = 0; //not valid
int 🕐 = 0;
 
int ☠ = 0; //not valid
int 💀 = 0;
 
int ✋ = 0; //not valid
int 👊 = 0;
 
int ✈ = 0; //not valid
int 🚀 = 0;
 
int ☹ = 0; //not valid
int 😀 = 0;
```

要するに、「基本的に記号を禁止しているのに、サロゲートペアなやつは禁止されない」という状態。
上から順に文字コードは以下のようになっています。

- ⏰ : U+23F0
-  🕐 : U+1F550
- ☠  : U+2620
-  💀  : U+1F480
- ✋  : U+270B
-  👊 :  U+1F44A
- ✈  : U+2708
-  🚀  : U+1F680
- ☹  : U+2639
-  😀  : U+1F600

この辺りはまあ、「なんか変だな」で済む話なんですが、1個、ポリコレ的な地雷を踏みそうな事案も発見されています。

```csharp {title="ポリコレ地雷を踏みそうな絵文字"}
bool 👷 = true; //  Construction Worker
bool 👷‍♀ = false; // Woman Construction Worker ({Construction Worker}{ZWJ}{Female Sign})
```

男の建築作業員はよくて女の建築作業員はダメなのか！

これ、[Emoji ZWJ Sequence](https://emojipedia.org/emoji-zwj-sequence/) というやつでして。
「絵文字が特定の性別に偏っている」という問題に対する解決策として、「絵文字をいくつか [ZWJ](https://ja.wikipedia.org/wiki/%E3%82%BC%E3%83%AD%E5%B9%85%E6%8E%A5%E5%90%88%E5%AD%90) でつなぐことで別の字形に変える」という対処をしています。
で、👷‍♀ の絵文字シーケンスが Female Sign (♀、U+2640)を含んでいて、これが「サロゲートペアじゃない記号」なので Immutable Identifier でも禁止されている文字になります。

当初は「スライド映えしなくなるくらい別にいいよね」というだけの問題だったものが「ポリコレ的に今のままの方がまずい」になったことでちょっと賛成票が増えたみたいです。

## まとめ

元々は C# の識別子について調べてる過程で、源流は Java っぽいという話だったんですが、
近い仕様が Unicode の推奨仕様([UAX31](https://www.unicode.org/reports/tr31/))になっていました。
ここまでは結構前から知っていたものの、UAX31 に類する仕様を採用しようとするプログラミング言語は少数派だと思っていました。

それが最近になって、C++ が実質的に破壊的変更になりえる状況で UAX31 Default Identifier を採用しそうな流れになっていて、提案文書に「<em>💩投げれなくなる</em>」のパワーワードが含まれていたという話でした。
