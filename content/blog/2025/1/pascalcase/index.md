---
title: "UTF8 か Utf8 か"
source_url: "https://ufcpp.net/blog/2025/1/pascalcase/"
content_type: "BlogEntry"
published_at: "2025-01-01T18:52:45"
updated_at: "2025-01-01T18:52:45"
tags: []
umbraco_id: 2507
parent_id: 2506
sort_order: 0
aliases: []
---

# UTF8 か Utf8 か

今日は C# 配信をやっててちょくちょく話題になるやつの話。

```csharp {title="Utf8? UTF8?"}
using System.Text;
using System.Text.Unicode;

var buffer = (stackalloc byte[3]);
Utf8.FromUtf16("abc", buffer, out var r, out var w);
Encoding.UTF8.GetString(buffer[..w]);
```

`Utf8` なの？
`UTF8` なの？

(昔1回同じ話題でブログ書いた気がしつつ、最近もまた話題に出たので。)

## .NET の命名ガイドライン

.NET には命名規約に関するガイドラインがありまして、以下の場所にドキュメントとして残っています。

* [Capitalization Rules for Identifiers](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/capitalization-conventions#capitalization-rules-for-identifiers)

おおむね以下のようなルール。

* クラス名などは PacalCase を使ってください
  * (2文字よりも長い) 頭文字略語も同様です
  * 2文字の頭文字略語(two-letter acronyms)は例外で、全て大文字

つまるところ、
`Utf8` か `UTF8` なら、前者の `Utf8` が正解。

ちなみに、 `Encoding.UTF8` に何か崇高な理由があるわけではなく、
単純に「初期はルールが徹底されてなかった。すまん。」という懺悔あり。

3文字略語で他に変なのがないかはちょっと検索してみたんですけども、`UTF8` 以外、自分には見つけられず。
ただ、何せ `Encoding.UTF8` の利用頻度が高く、どうしても目立ってしまいます。

## 特殊ルールやめてほしい問題

例外であるところの「2文字の略語」に該当するのは例えば以下のような名前。

* [`IO`](https://learn.microsoft.com/ja-jp/dotnet/api/system.io?view=net-8.0) 名前空間
* [`GC`](https://learn.microsoft.com/ja-jp/dotnet/api/system.gc?view=net-8.0) クラス
* `IL` ([`ILGenerator`](https://learn.microsoft.com/ja-jp/dotnet/api/system.reflection.emit.ilgenerator?view=net-8.0) クラスとか)

個人的な意見としては、正直、こんな特殊ルールはない方がよかったんじゃないかなぁと思っています。
「`System.IO` で懺悔案件やらかしちゃったのをあとからつじつま合わせの特殊ルール足しただろ」とか陰口言われてるのも見たことがあります
(個人的にはこれに1票)。

`System.Io`、`System.Gc` は確かになんか字面の違和感すごいですし、`Il` (L が小文字)の視認性の悪さもすごいんですけど、特殊ルールが招く混乱よりはだいぶマシというお気持ち。

↓とか結構キモいと思うんですよねぇ。

* [`Avx512BW`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.intrinsics.x86.avx512bw?view=net-8.0) (AVX-512 の byte and word 命令)
* [`Avx512Vbmi`](https://learn.microsoft.com/ja-jp/dotnet/api/system.runtime.intrinsics.x86.avx512vbmi?view=net-8.0) (同、Vector Byte Manipulation Instructions)

ルール上は確かに、 BW は全部大文字で、VBMI は先頭だけ大文字なんですけども…

.NET Runtime の中の人もしばしば混乱していまして、
public なものにこそあまりないものの、internal/private なものだとときどき「2文字略語だけど2文字目が小文字」があります。

* [`DiyFp`](https://github.com/dotnet/runtime/blob/6045e29bbefa63b58ccd5502f057ae452f3c83af/src/libraries/System.Private.CoreLib/src/System/Number.DiyFp.cs#L18) (floating point)
* [`NtDll`](https://github.com/dotnet/runtime/blob/511d26611c051c56e546404ea616c220cc78817c/src/libraries/Common/src/Interop/Windows/Interop.Libraries.cs#L25) (Windows NT の NT。New Technology?)

他に、「.NET Runtime 内のもの([`DBNull`](https://learn.microsoft.com/ja-jp/dotnet/api/system.dbnull?view=net-8.0))は DB だけど、ASP.NET チームが書いてるもの([`DbContext`](https://learn.microsoft.com/ja-jp/dotnet/api/system.data.entity.dbcontext?view=entity-framework-6.2.0))は Db」なんて事案も。
ASP.NET チームは「しがらみはリセット」路線で、やっぱ特殊ルールを快く思ってないんでしょうね。


### ID? OK?

[`PlatformID`](https://learn.microsoft.com/ja-jp/dotnet/api/system.platformid?view=net-8.0)…

こいつだけが「2文字とも大文字の ID」です。
[`ManagedThreadId `](https://learn.microsoft.com/ja-jp/dotnet/api/system.threading.thread.managedthreadid?view=net-8.0#system-threading-thread-managedthreadid)とか[`EventId`](https://learn.microsoft.com/ja-jp/dotnet/api/system.diagnostics.tracing.eventattribute.eventid?view=net-8.0#system-diagnostics-tracing-eventattribute-eventid)とかは Id。
これも懺悔案件でしょうね。`PlatformID` は最初期からある enum なので。

何だったら、ドキュメントには↓とか書かれてますからね。`ID` を `Id` に渡す。

> `PlatformID` の列挙型メンバーの整数値を SignTool.exe の `PlatformId` 引数に渡せる

しかし、id とは一体何なのか…

* identity の略？ → ガイドライン的には「略語は使うな」なので、Identity と書くべき
* id という英単語(元は identity だったとしても、もう id のスペルで普及)？ → 略語ではないので Id と書くべき
* identity document (本人確認書類)の略？ → なら ID
  * `PlatformID` がこの意味とは考えにくい…

他に、OK もぶれがちで、
[`OK`](https://learn.microsoft.com/ja-jp/dotnet/api/system.windows.messageboxresult?view=windowsdesktop-8.0)も[`Ok`](https://learn.microsoft.com/ja-jp/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.ok?view=aspnetcore-9.0)もあります(これも、後者は ASP.NET)。

ok とは一体何なのか…

* 元々 all correct がなまったスラングの oll korrect の略らしい → OK
* が、語源ももうあいまいで ok という英単語化してる → Ok
* なんなら okay というスペルも定着してる → Okay (略すな危険)

この辺りで悩むことになるのも「2文字略語は特別」とかやっちゃったからで、
この特殊ルールがなければ迷うことなく Id, Ok なんですけども。

## まとめ

ガイドライン上、`Utf8` が正しくて、`UTF8` は過去のしがらみです。
ガイドラインは以下の通り。

* 3文字以上の場合は略語も含め、PascalCase
* 2文字の略語だけ例外で IO とか GC とか全部大文字

ただ、2文字略語の特殊ルールも割と過去のしがらみ感があって、
ASP.NET とかは2文字の略語も含めて PascalCase を採用しているみたいです。
