---
title: "Visual Studio 2019 GA"
source_url: "https://ufcpp.net/blog/2019/4/vs2019ga/"
content_type: "BlogEntry"
published_at: "2019-04-03T20:35:01"
updated_at: "2019-04-03T20:38:44"
tags: []
umbraco_id: 2238
parent_id: 2236
sort_order: 1
aliases: []
---

# Visual Studio 2019 GA

[Visual Studio 2019 の発表イベント](https://visualstudio.microsoft.com/vs2019-launch/)に合わせて、Visual Studio 2019 が GA (general available)になりました。

そういえば、Visual Studio も「GA」って言い方するようになったっぽいですね。
今まで「正式リリース」を指して RTM (Release To Manufacturing、ソフトウェアが CD とか DVD で売られてた頃の名残)とか言っていたんですが。

まあ、どうせドキュメントにもしばらく RTM と GA が混在すると思いますが。

## インストール

相変わらず日本語検索で「Visual Studio 2019」だけで GA 版のインストーラーにたどり着くのがちょっと大変臭く…

同僚は「[Publickey](https://www.publickey1.jp/blog/19/visual_studio_201941live_shareaiintellicodemac.html)経由で行けばダウンロードできた」と言ってました。

自分は先月すでに [RC (Release Candidate)版](../../2/vs2019rc/index.md)をインストールしていたので、アップデートの自動配信で GA 版にアップデートされました。

## 言いたいことは RC の時点で言った

毎度恒例なんですけども、「言いたいことは RC 版の時に行っちゃったので、GA 版で今更言うことはない」状態です。
RC ってのはそういうもの(バグ修正を除いて変更はしない)なので。

ということで、詳細は以前の RC 版のブログ

- [Visual Studio 2019 RC と Preview 4](../../2/vs2019rc/index.md)

を参照してください。

### Visual Studio の配信チャネル

先日の RC の時点でも書きましたが、今、Visual Studio Installer は「RC → GA」配信のチャネルと、Preview のチャネルに分かれています。

今は、

- RC だったもの → GA にアップデート
- Preview の方 → 16.0.0 Preview 5.0 が配信される(GA 相当と同じ機能)
- ついでに、Visual Studio 2017 の方にも 15.9.11 の配信あり

という状態。
Preview の方にはいずれ 16.1 Preview 1 の配信があると思います。

ちなみに、C# 8.0 にはいくつか 16.1 で初めて実装される機能があるんですが、
「[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)」を眺めている感じ、それが提供されるのは 16.1 Preview 2 かららしいです。
Preview 1 時点ではおそらく何の変化もありません。

## C# 8.0 有効化したった

ということで、自分にとっては「言うことは先月言った。インストールも RC からのアップデートだけ」という状態なわけですが。

一応、職場でチーム全員が Visual Studio 2019 になったのをいいことに、 C# 8.0 を仕事でも有効化してしまいました。

[いくつか破壊的変更も報告されています](https://github.com/dotnet/roslyn/blob/master/docs/compilers/CSharp/Compiler%20Breaking%20Changes%20-%20VS2019.md)が、職場のコードで踏み抜いた変更はなかったです。

ただし、その際には以下の注釈付き。

状況:

- Visua Studio 2019 (16.0)が GA (general available)になったけど、C# 8.0 は preview という扱い
- LangVersion preview とかを指定しないと使えない
- 正式リリースは .NET Core 3.0 に合わせるはずで、 .NET Core 3.0 は「今年後半」とだけ言われてる
- preview と言っても、現状で実装されてる機能は機能ごとにほんと安定度が違うので、使ってよさそうなのとダメそうなのが明確に分かれてる

以下の機能は遠慮なく使おうかと:

- [再帰パターン](../../../2018/12/cs8patterns/index.md)
- [switch 式](../../../2018/12/cs8switchexpr/index.md)
- [静的ローカル関数](../../1/vs2019p2/index.md)
- [`??=`](../../../2018/12/cs8misc/index.md)
- [using 改善](../../../2018/12/cs8notyet/index.md)

switch 式だけは注意が必要で、16.1 で演算子の優先順位が変わる予定。単品でだけ使う方が無難(`x + y switch { ... }` みたいな書き方は避ける)。

.NET Core 3.0 依存な機能([Range](../../../2018/12/cs8ranges/index.md)とか[非同期ストリーム](../../../2018/12/cs8asyncstreams/index.md)は当然使えないし、[null 許容参照型](../../../2018/12/cs8nrt/index.md)もまだ手を出さない。
