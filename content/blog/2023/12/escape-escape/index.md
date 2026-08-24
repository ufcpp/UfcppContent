---
title: "\\e (エスケープ文字のエスケープ シーケンス)"
source_url: "https://ufcpp.net/blog/2023/12/escape-escape/"
content_type: "BlogEntry"
published_at: "2023-12-05T22:42:39"
updated_at: "2023-12-05T22:45:16"
tags: []
umbraco_id: 2477
parent_id: 2476
sort_order: 0
aliases: []
---

# \\e (エスケープ文字のエスケープ シーケンス)

先々月書いた「[C# 13 向けトリアージ](../../10/triage2023/index.md)」で紹介してた C# 13 候補の1つ、「`\e` エスケープ シーケンス」が早々に実装されてたという話です。

[.NET 8 正式リリース記念](https://www.youtube.com/watch?v=1w-E4QgmAdg)の配信ではちょこっと触れてたんですが、そういえばブログには書いてなかったので紹介。

## <a id="escape-character">エスケープ文字</a>

キーボードで打てないような文字や、画面に表示されない文字を入力したりするために、
「`\n` と書いたら改行(U+000A, new line, line feed)に置き換える」みたいな仕様があり、これをエスケープ シーケンス(escape sequence, 回避用の一連の文字列)と言います。

C# をはじめ、C 言語の影響を受けて作られた言語の多くは `\` (reverse solidus, 逆スラッシュ)で始まる文字列によってエスケープします。
プログラミング言語だと他には <code>`</code> (逆引用符、グレイブ アクセント, grave accent)とかを使うものがあったりしますが、
要は、利用頻度があまりない文字をエスケープ シーケンスの開始文字にすることが多いです。

一方で、ASCII コードには古よりずっと、[エスケープ文字](https://ja.wikipedia.org/wiki/%E3%82%A8%E3%82%B9%E3%82%B1%E3%83%BC%E3%83%97%E6%96%87%E5%AD%97#ASCII%E3%82%A8%E3%82%B9%E3%82%B1%E3%83%BC%E3%83%97%E6%96%87%E5%AD%97)(U+001B, escape character)というものがあります。
名前通りエスケープ シーケンスの開始文字として使われるもので、
多くのターミナル アプリがこのエスケープ文字を使ったシーケンスに対応しています。
[ANSI (American National Standards Institute) によって策定された標準仕様](https://en.wikipedia.org/wiki/ANSI_escape_code)があって、大体のターミナルはこの仕様に基づいた実装を持っています。
(この仕様は ANSI X3.64 というそうです。)

例えば C# で以下のようなコードを書いて実行すると、たいていの環境で赤い文字が表示されるはずです。

```csharp {title="ANSI X3.64 を使って文字色を変える例"}
Console.WriteLine("\u001b[31mred text");
```

`\u001b` がエスケープ文字(以下、ESC と表記)で、ESC + `[31m` という文字列を Console に書き込むとそれ以降の文字色が変わります。

## <a id="escape-escape">エスケープのエスケープ</a>

そして C# 13 候補として、このエスケープ文字(U+001B)に対する C# のエスケープ シーケンスとして、`\e` が提案・承認されました。

C# 12 以前でも `\x` + 16進数2桁とか、`\u` + 16進数4桁とか、 `\U` + 16進数8桁とか、
任意の文字コードを直接打ち込むエスケープ手段があったので、別にそれほどなくて困るものでもなかったりはします。
以下のコードの `\x1b`, `\u001b`, `\U0000001b` はいずれもエスケープ文字です。

```csharp {title="\x, \u, \U"}
Console.WriteLine("\x1b[31mred text");
Console.WriteLine("\u001b[4munderlined text");
Console.WriteLine("\U0000001b[0mreset style");
```

古からある仕様ですが、
長らく C# の主戦場だった Windows では
「文字のスタイル変更は `Console.ForegroundColor` などの API 経由で行ってほしい」
みたいな感じで、あまり ANSI X3.64 を利用する文化ではありませんでした。

しかし最近は Linux 上での C# 利用も増え、
Windows も[今時っぽい新しいターミナル](https://apps.microsoft.com/detail/9N0DX20HK701?hl=ja-jp&gl=JP)を搭載するようになり、
ANSI X3.64 を積極的に使いたいという要望がちらほら増えてきました。

また、
Windows Terminal が新しくなった今となっては、
`Console.ForegroundColor` などの .NET の API を使って操作できるものよりも、
ANSI X3.64 でやれることの方が多くなっていたりします。

そこで出てきたのが[ `\e` でエスケープ文字を表せるようにしてほしい](https://github.com/dotnet/csharplang/issues/7400)という要望。

## <a id="proposal">\e 提案の検討</a>

この提案で得られるメリットや、かかるコストを考えてみましょう。

まずメリットの方。
前節で書いた通り、エスケープ文字を使いたいことはちらほらないこともなく、「あれば便利かも」とは思います。
とはいえ、毎回自分で ANSI X3.64 を書くかと言われると微妙。
「31番が赤」とかいちいち覚えないですからね。
C# でも、ANSI X3.64 出力用のライブラリを提供してくれている方がいらっしゃいます: [Kokuban](https://github.com/Cysharp/Kokuban) (安定の Cysharp)。

また、元から `\x1b` と書けたわけで、「`\e` と書けるようになって楽かどうか」と言われるとたった2文字の短縮です。
もちろん、「エスケープ文字の文字コードは何だったっけ？」というのを覚えるよりは「エスケープの頭文字をとって `e`」というのの方が覚えやすそうではあります。

コストに関しては、エスケープ シーケンスの解析用の `switch` ステートメントに1個 `case` を追加するだけです。
以下のたった3行の追加。

```csharp {title="\e 対応のためのコード"}
    case 'e':
        ch = '\u001b';
        break;
```

「C# 12 以下では使えない」みたいな判定を足すとしてもさらに追加で +3 行。
テストとかを足しても数百行程度の修正になります。
ここの `case` 1個くらいならコンパイル実行時のコストもほとんどなし。

要するに、割かし毒にも薬にもならない、低コスト低リターンな提案ということになります。

なので、[C# チームによる判定](https://github.com/dotnet/csharplang/discussions/7603)は「Any Time」。
この「Any Time」は、

* 自分たちで実装の労力は割かない
* コミュニティによる Pull Request が来た場合は受け付ける

みたいな温度感です。

## <a id="implemented">そして実装</a>

「Any Time」のわりにもうすでに実装されたものがあるわけですが。
以下のコード、Visual Studio 17.9 Preview 1 (11月15日にリリース) で動きます。

```csharp {title="\e エスケープ、もう動いてる"}
Console.WriteLine("\e[31mred text");
Console.WriteLine("\e[4munderlined text");
Console.WriteLine("\e[0mreset style");
```

![\e もう動いてる](../../../../../assets/media/1217/escapeescape.png)

普通、コミュニティ実装だとそこそこ時間がかかるんですけどね。
何せ、「専業でやっているわけじゃない外部の人のコードのレビュー」みたいなプロセスを経るので。

`\e` に関しては、

* [10月17日に「Any Time」で承認](https://github.com/dotnet/csharplang/issues/7400#issuecomment-1765078956)
* [10月21日に Pull Request 出る](https://github.com/dotnet/roslyn/pull/70497)
* 10月24日に Pull Request が通る
* [10月31日に main ブランチに merge](https://github.com/dotnet/roslyn/issues/70634)

という感じ。
「Any Time」とは…

Pull Request を作った方、C# チームの人ですしね。
定時後とかにさらっとやっちゃった感じかなぁと。
[ホリデーの飛行機の中で embedded language を実装しちゃうような人](https://github.com/dotnet/roslyn/pull/23984)なので。

ということで、「.NET 9 のプレビュー版もまだ出てないのにもう C# 13 候補機能の1つが実装されてリリースされてる」という面白状況に。
