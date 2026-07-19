---
title: "GetUnicodeCategory(int codePoint) を提案してみた"
source_url: "https://ufcpp.net/blog/2018/1/getunicodecategory/"
content_type: "BlogEntry"
published_at: "2018-01-21T00:40:00"
updated_at: "2018-01-21T02:43:05"
tags: []
umbraco_id: 2130
parent_id: 2125
sort_order: 2
aliases: []
---

# GetUnicodeCategory(int codePoint) を提案してみた

今日は、.NET で、U+10000 以上のコードが割り当たってる文字の Unicode カテゴリー判定をする方法について。
提案を出したらそのままプルリクを出すことになった話。

## 背景

### U+10000 以上の文字

Unicode について詳細は、昔書いた記事があるのでそちらを参照。

- [Unicodeとは？ その歴史と進化、開発者向け基礎知識](https://www.buildinsider.net/language/csharpunicode/01)
- [Unicodeと、C#での文字列の扱い](https://www.buildinsider.net/language/csharpunicode/02)

ここではさらっと。

U+10000 以上に割り当たってる文字は、要するに、以下のようなものです。

- Unicode 設計当初に想定していなくて、後から「追加面」(supplementary planes)として定義した
- UTF-16 だと1文字で表せない
  - なので、サロゲート ペア(surrogate pair: 代理対)っていう2文字1組のコードで表す
- UTF-8 だと4バイト文字になる
- 一部のマイナーな漢字、マイナー言語の文字、絵文字なんかが入ってる

C# にとって問題になるのは、C# が作られた時期にはまだこの追加面文字が普及していなかったということです。
C# の文字列は内部的に UTF-16 だし、.NET の標準ライブラリは追加面の文字を扱うためのものがいまいちそろっていなかったりします。

まあ昔は本当に追加面にはマイナーな文字しかなかったのでそれほど問題にはならなかったわけです。
そう、絵文字がASCII圏でも広まり出すまでは…

### Unicode カテゴリー判定

まあ、絵文字のおかげで、アメリカ産のソフトウェアでも「ASCII 文字しかまともに受け付けない」みたいな不具合は減ってきているそうですが。
その一方で、「追加面文字は全部『サロゲート ペア』扱い」、すなわち、絵文字と一部のマイナーな漢字もいっしょくたに扱われてしまったりする問題はいまだ結構あったりします。

例えば、実のところ、C# コンパイラーがまさにそうでして。
(昔、[ちょっと勉強会で話したこともある](https://www.slideshare.net/ufcpp/c-86447482/48)んですが)

C# は仕様上、Letter (書き言葉に使われる文字)なら何でも識別子として使えるはずなんですが、
今、追加面に入っている Letter は使えないという「仕様違反」があります。

```csharp
// U+FFFF 以下 → 識別子に使える
var ᚠ = 1; // ルーン文字(U+16A0, OtherLetter)
var ʬ = 2; // 国際発音記号(U+02AC, LowercaseLetter)
var ℏ = 3; // 文字様記号、プランク定数(U+210F, LowercaseLetter)

// U+10000 以上 → 識別子に使えない
var 𩸽 = 4; // 一部のマイナーな漢字、ほっけ(U+29E3D, OtherLetter)
var 𓀀 = 5; // ヒエログリフ(U+13000, OtherLetter)
```

まさかのルーン文字以下の扱いを受けてる漢字があるとかʬʬ
草生えるʬʬʬʬʬ

これ、既知の問題でして、[中国の方が報告](https://github.com/dotnet/roslyn/issues/13474)はしているんですが。
そんなに「私もこれで困ってる」的な反響もなく、低優先度でずっと放置状態です。
(ちなみに、僕は C# コンパイラーの中身をいじってこの問題を修正したこともあるので、その気になれば修正プルリク出せます。需要がなさ過ぎてやる気も起きない…)

どうしてこういう挙動になるかと言うと、追加面文字のカテゴリーを正しく見ていないから。
C# は内部的に UTF-16 で文字列を扱っているので、
追加面文字の文字カテゴリーを見ようとすると、単に[サロゲート](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.unicodecategory?view=netcore-2.0#System_Globalization_UnicodeCategory_Surrogate)という判定を受けます。

### .NET のカテゴリー判定メソッド

.NET では、[`CharUnicodeInfo`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.charunicodeinfo?view=netcore-2.0)っていうクラスに Unicode 絡みのメソッドが定義されています。
(昔は`char`型に定義されていて、互換性のために今も `char` にその手のメソッドはあるんですが。)

Unicode カテゴリー判定は以下の2つメソッドがあります。

- [`GetUnicodeCategory(Char)`](https://docs.microsoft.com/ja-jp/dotnet/api/system.char.getunicodecategory?view=netcore-2.0#System_Char_GetUnicodeCategory_System_Char_)
- [`GetUnicodeCategory(String, Int32)`](https://docs.microsoft.com/ja-jp/dotnet/api/system.char.getunicodecategory?view=netcore-2.0#System_Char_GetUnicodeCategory_System_String_System_Int32_)

使い方は以下のような感じ。

```csharp
using static System.Console;
using static System.Globalization.CharUnicodeInfo;

class Program
{
    static void Main()
    {
        // char 用
        WriteLine(GetUnicodeCategory('ᚠ'));
        WriteLine(GetUnicodeCategory('ʬ'));
        WriteLine(GetUnicodeCategory('ℏ'));

        // 追加面(= char で表せないもの)用
        WriteLine(GetUnicodeCategory("𩸽", 0));
        WriteLine(GetUnicodeCategory("𓀀", 0));
    }
}
```

お分かりいただけるだろうか…

現状の .NET に追加面文字なんてものはないので、`string` で判定します。
サロゲート ペアになっている2文字の UTF-16 からカテゴリー判定するメソッドになっています。

内部が UTF-16 な文字列しか持っていないこれまだとそんなに困らなかったかもしれません。
でも、今、.NET にも [`Utf8String`](../../../2017/12/fastutf8string/index.md) なんていうものが実装されようとしているわけでして、このままだと困ります。

というか、`Utf8String` の完成を待っていられなくて[自作](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2016/Unicode)したり[自作](https://github.com/neuecc/Utf8Json)したりしてると、今でも困っています。

```csharp
// 𩸽の UTF-8 バイト列
// ファイルとかネットとか、今時普通は UTF-8 で保存・送受信するでしょ
var utf8 = new byte[] { 240, 169, 184, 189 };

// UTF-8 をデコード
// 𩸽のコードポイント U+29E3D が得られてるはず
var c = Decode(utf8, 0);

WriteLine(c.ToString("X")); // 29E3D

// このコードポイントから直接カテゴリーを得る手段がない
var category = GetUnicodeCategory(c); // ここでコンパイル エラー
WriteLine(category);
```

(`Decode` メソッドの実体は [Gist](https://gist.github.com/ufcpp/0b1d03675739d624c6d53d8db316d0ea#file-utf8-cs-L23) に。)

これ、もし現状の API でカテゴリー判定をしたければ、以下のような無駄なコードが必要になります。

```csharp
// 一度 string に変換(= 無駄にヒープ アロケーションが発生)
var s = char.ConvertFromUtf32(c);
// string 版の GetUnicodeCategory を呼ぶ
var category = GetUnicodeCategory(s, 0);
WriteLine(category);
```

要するに、以下のようなメソッドをよこせと言いたい。

- `GetUnicodeCategory(int codePoint)`

ちなみに、他のプログラミング言語はというと…

- Java: 同様のメソッドの引数、ちゃんと int。[`getType(int)`](https://docs.oracle.com/javase/jp/7/api/java/lang/Character.html#getType(int))
- Go: 最初から[文字が32ビット](https://golang.org/pkg/builtin/#rune)
- Swift: 同上、[32ビット](https://developer.apple.com/documentation/swift/unicode.scalar)

### 実は最初から実装ある

で、ですよ。
上記の、`GetUnicodeCategory(String, Int32)`の[中身を覗く](https://github.com/dotnet/coreclr/blob/38cf93013c1dd1efc7137a6f4930cab7cc653411/src/mscorlib/shared/System/Char.cs#L859)じゃないですか。
だって、サロゲート ペアのままでテーブル引いてるわけないし。
絶対内部で一度コードポイントにデコードしてるだろうと。

そしたらやっぱり簡単に見つかるわけですよ、
[`InternalGetUnicodeCategory(int ch)`](https://github.com/dotnet/coreclr/blob/38cf93013c1dd1efc7137a6f4930cab7cc653411/src/mscorlib/shared/System/Globalization/CharUnicodeInfo.cs#L293)とかいう internal なメソッドが。

public にしろと。

## issue 立て、そしてプルリク

この話、いつから気付いていたかというと、[昔C# コンパイラーの中身を書き換えたとき](https://dotnetfringe-japan.connpass.com/event/35659/)なので、もう1年以上前だったりします。

まあ？標準ライブラリに`Utf8String`が入る頃までには？
ほっといてもそのうちたぶん入るだろうし？
入る…よね？
そもそも`Utf8String`自体がまだもうちょっと先っぽい？
あれ…？

みたいな。

### issue 立てました

ということで、観念して突っついてみることに。
issue 立てました。

- [API Propsal: char.GetUnicodeCategory(unicode scalar) #26173](https://github.com/dotnet/corefx/issues/26173)
  - コードポイントを引数に受け取る `GetUnicodeCategory` が欲しいんだけど
  - てか、内部的に最初からあるよね？

[roslyn](https://github.com/dotnet/roslyn) とか [csharplang](https://github.com/dotnet/csharplang) とかなら普段から割かし張り付いてるので勝手もわかるんですが。
[corefx](https://github.com/dotnet/corefx)は勝手がわからず結構悩んだり。

そもそも、こういうプリミティブ型絡みの機能は corefx (標準ライブラリ)の方じゃなくて、[coreclr](https://github.com/dotnet/coreclr) (実行環境)の方に含まれていますし。
それとは別に、最近は「設計に関する提案はこちら」みたいな [dotnet/designs](https://github.com/dotnet/designs)とかいうリポジトリもありますし。
どれに、どう書けばいいの…

結局、corefx に出したわけですけども、まあ、一応それでも受け付けてもらえたみたい。

ただ、corefx に対する提案のフォーマットとしてはダメだったみたいで、

- [このドキュメント](https://github.com/dotnet/corefx/blob/master/Documentation/project-docs/api-review-process.md)に沿って整形して
- あっ、こっちで提案文章を更新しといたわ、これでいいか？

みたいなコメントが寝ている間に書かれていてビビったり。
(あちらはアメリカ西海岸なので、日本時間で言うと深夜2時くらいから稼働開始。こっちが寝てる間に話が進む。)

### レビューされました

提案 issue が corefx チームの目に留まった場合、
当然ですがレビューが掛かります。
今は、レビューの様子も毎回 YouTube に投稿されていたりします。
([Immo](https://www.youtube.com/channel/UCaFP8iQMTuPXinXBMEXsSuw) (.NET 標準ライブラリ周りのプログラム マネージャー)が投稿。)

何気に今週のレビューの様子を見てみたら、割かし最初の方にその提案 issue が取り上げられてるじゃないですか。

<div>
<iframe width="1504" height="759" src="https://www.youtube.com/embed/rSVM5RmAhso" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>
</div>

(1分30秒頃から)

英語半分も聞き取れないんで大体ですけども…

- やっぱ ASCII 圏の人は追加面文字とか知らない
  - `string` 版のやつは何か特殊なことしてるのか？`char` ではダメなのか？みたいな
  - 詳しそうな人がチームメイトに向かって「サロゲート ペアとは」みたいな説明を開始
- public にしてリネームするだけだしいいだろう、ただし、引数名はちゃんとしないと
  - 提案では Unicode scalar とか code point とか書かれてるけど、どれがいいんだ
  - scalar とか code point とかの説明も開始
  - じゃあ、code point の方だな

みたいな感じで、10分程度でレビュー完了。
結果がこちらのコメント: [Looks good but we should rename the parameter](https://github.com/dotnet/corefx/issues/26173#issuecomment-358057543)

ちなみに補足。scalar と code point は以下の差らしい。

- scalar: サロゲートになっている文字コード(前半 U+D800 〜 U+DBFF、後半 U+DC00 〜 U+DFFF)部分は除く
- code point: サロゲートの範囲も含む

`GetUnicodeCategory`の場合は、サロゲート文字を食わせると[Surrogate](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.unicodecategory?view=netcore-2.0#System_Globalization_UnicodeCategory_Surrogate)を返してくるので、この範囲が除外されている scalar ではなく、含んでいる code point が正解とのこと。

### プルリクを出す権利をやろう

レビューが通ったわけですけども、そしたら[こんなこと言われた](https://github.com/dotnet/corefx/issues/26173#issuecomment-358120197)わけですよ。

> この API の実装に関して、ヘルプはほしい？そうする意思があるならガイドをできるけども。

えっ、あっ、はい。
こちらでプルリク作るんですか。

この作業、
どう考えても「中の人」がやるのが手っ取り早いわけですよ。
public にしてリネームするだけ。
それも、Visual Studio でリファクタリング機能を使えばほんとに一瞬で、
普段から関わっている人が作業すれば1分で済むやつ。

それに対して、英語もカタコト、時差もある相手にプルリクをオファー。
どう考えても、「提案者であることに免じてコミット履歴に名前を残す権利をやろう」的な接待モード。

というか、実際接待。
いただいたガイドも、「この行をリネームしたプルリクを出してくれ」。
作業指示がきわめて具体的な上、テストとか要らないんですか。
テストはそちらでやっていただけるんですか。
はい、プルリク出します。

- [CharUnicodeInfo.GetUnicodeCategory(int codePoint) #15911](https://github.com/dotnet/coreclr/pull/15911)

### 条件コンパイルの罠

もう1度言いますが、「Visual Studio でリファクタリング機能を使えばほんとに一瞬」。

しかし、そこには罠があったのです。

クロスプラットフォームなものに関わらないとあんまりはまることがない罠なんですが。
クロスプラットフォームなものを作ってると、結構、条件コンパイルだらけになります。
Windows 向けビルドでしか通らない場所、Unix でしか通らない場所…

はい、やらかしました。Unix ビルドを壊すやつ。
IDE のリファクタリングって、条件コンパイルの全条件までは追えないんですよねぇ。

世間一般にも、条件コンパイルはビルドを壊す原因ナンバー1。
だって、手元ではビルド通ってるんだもん。
そのままコミットしちゃうじゃない？

しかも今回に関しては、C# の`#if` プリプロセス分岐ですらなく。
csproj 中に、「[`ItemGroup Condition` で `TargetsUnix` っていうプロパティが定義されている場合だけこのソースコードを使う](https://github.com/ufcpp/coreclr/blob/44c0d3a6647a2df72b8f2ff1254a77bf74956725/src/mscorlib/System.Private.CoreLib.csproj#L581)」みたいな記述が書かれてるやつでした。
自分が立ち上げてた Visual Studio からはファイルが見えてすらいない…

そしてこれもまた、僕が寝ている間に「直しといたよ」コミットが追加されているという接待っぷり…

### その後

coreclr に出したプルリクはマージされたみたいですね。

ただ、その後も関連作業が続いているみたいです。
単に coreclr にある mscorlib の実装だけじゃなくて、それを corefx 側に公開したりする作業が必要だそうで。

そっちでも、寝ている間に「テスト通らないなぁ」→「あっ、このテスト失敗は別件だわ、大丈夫」みたいな会話がなされており。

## まとめ

- ASCII 圏の人、ほんとに Unicode 追加面を知らない
  - 「こんなのほっといてもすぐに追加されるだろう」とか甘い
  - たぶん、日本人か中国人が言い出さないと進まない
- プルリクを出す権利をやろう
  - 接待モード
  - 時差的に、寝てる間にいろいろ起こってる
  - 条件コンパイルの罠
