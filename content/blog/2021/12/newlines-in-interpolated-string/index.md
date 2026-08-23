---
title: "【C# 11 候補】 {} 中の改行"
source_url: "https://ufcpp.net/blog/2021/12/newlines-in-interpolated-string/"
content_type: "BlogEntry"
published_at: "2021-12-16T21:48:13"
updated_at: "2021-12-16T21:48:13"
tags: []
umbraco_id: 2382
parent_id: 2375
sort_order: 3
aliases: []
---

# 【C# 11 候補】 {} 中の改行

今日は「実は Visual Studio 17.1 Preview 1 (先月) の時点で既に入ってた」という機能の話。

C# 11 で、`$"{ここ}"` みたいな「補完穴」(interpolation hole: 補完文字列の `{}` の中)の改行に関する仕様がちょっと変わります。

## <a id="new-line-in-string">文字列リテラル中の改行</a>

C# の[文字列リテラル](../../../../study/csharp/start/st_embeddedtype.md#charl)は、`@` を付けると逐語的(`\` を使ったエスケープをしなくなる)になって、その中には改行を直接入れることができます。

```csharp {title="@ を付けると文字列内での改行 OK になる" error-ranges="sha256:186d3369fbec66bf908944d6753172c480ef4ec49908a8229d09f78974a7bfae;7:1-7:2"}
// @ を付けると文字列内での改行 OK になる。

var s1 = ""; // 改行入れれない。
var s2 = @"
"; // 改行 OK。
var s3 = "
"; // 当然これはコンパイル エラー。
```

この仕様、[補間文字列](../../../../study/csharp/start/st_string.md#key-interpolated-string)に対しても同様です。

```csharp {error-ranges="sha256:af5c58cb57e1a78c32805261d47fae88ef404b9b3bd18ff41e2f09196b278c3e;10:1-10:2"}
// @ を付けると文字列内での改行 OK になるのは $"" でも一緒。

var x = 123;

var s1 = $"{x}"; // 改行入れれない。
var s2 = @$"
{x}
"; // 改行 OK。
var s3 = $"{x}
"; // 当然これはコンパイル エラー。
```

## <a id="new-line-in-interpolation-hole">補間穴中の改行</a>

C# はほぼ全ての構文で改行の有無を問わないので、例えば以下の2つのコードは全く同じ意味になります。

```csharp {title="1行"}
var x = 123 + 987;
```

```csharp {title="改行を入れたもの"}
var
    x
    =
    123
    +
    987
    ;
```

で、補間穴 (`{}`)の中は普通の C# 構文になります。
前述のような「改行の有無を問わない」という常識に照らし合わせると、
以下のようなコードを書けていいはずです。
(C# 10 まではなぜかダメ。)

```csharp {title="{} 中の改行" error-ranges="sha256:aa47b506529b427f160f513c3d06f9f06685ad5524ba5224942546173ee5ccff;7:6-7:7"}
// なぜかダメだったコード。

var x = 123;

var s1 = $"{
    x
    }";
```

ちなみに、これに `@` を付けると C# 10 でもコンパイルできます。
というか、さらに言うと割かし何でも書けます。
`//` コメントすら書けます。

```csharp {title="@ を付ければなぜか OK"}
// @ を付ければなぜか OK。

var x = 123;

var s1 = $@"{
    x
    +
    987 // コメントすら OK
    }";
```

## <a id="new-line-in-interpolation-hole-11">C# 11 での変更</a>

で、まあ、`$"{}"` と `$@"{}"` で挙動が違うの、
[仕様](https://github.com/dotnet/csharpstandard/blob/draft-v6/standard/grammar.md)的にもそうなってるらしいんですが、
中の人曰く「[改行を禁止した実際の理由、覚えてない](https://github.com/dotnet/csharplang/blob/main/meetings/2021/LDM-2021-09-20.md#newlines-in-non-verbatim-interpolated-strings)」とのこと。

挙動が違うのも変なのでさらっと直したみたいです。
気づいたタイミング的に [C# 10](../../../../study/csharp/cheatsheet/ap_ver10.md) 正式リリースには間に合わなかったものの、
ほぼ修正は終わってたみたいで、即座に merge、実は 17.1 Preview 1 には入っていたみたいです。

ということで、実は [LangVersion preview](../../../../study/csharp/cheatsheet/langversionoption.md#langversion) を入れればもう動くらしい。

![LangVersion preview を入れればもう動くらしい](../../../../../assets/media/1197/newlineininterpolation.png)

(このスクショは Visual Studio 17.1.0 Preview 1.1 で撮影。)

さよなら、LangVersion default。おかえり、preview (1年ぶり2度目)。

ということで、以下のようなコード、C# 11 候補になっていて、
preview 指定すると現在でもコンパイルできたりします。

```csharp {title="C# 11 で有効になりそうなコード"}
// C# 11 候補。

var x = 123;

var s1 = $"こっちは C# 11 から OK {
    x
    +
    987 // コメントすら OK
    }";

var s2 = $@"こっちは元から OK
{
    x
    +
    987 // コメントすら OK
    }
def";
```

と言うのを[昨日の Pull Request](https://github.com/dotnet/roslyn/pull/58250) を見て初めて気づいたという話でした。
