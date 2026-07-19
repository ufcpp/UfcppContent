---
title: "ピックアップRoslyn: .NET 5 への Xamarin 統合に向けて"
source_url: "https://ufcpp.net/blog/2020/5/featureswitch/"
content_type: "BlogEntry"
published_at: "2020-05-03T18:24:27"
updated_at: "2020-05-03T18:24:27"
tags: []
umbraco_id: 2292
parent_id: 2290
sort_order: 1
aliases: []
---

# ピックアップRoslyn: .NET 5 への Xamarin 統合に向けて

1つ前のブログで話した [Source Generator](../sourcegenerator/index.md) の動機の1つは、
リフレクションが使えない環境でのコード生成をある程度カバーできるようにというものです。

今、 .NET Core と Xamarin の統合作業真っ最中で、
その結果、iOS とか Web Assembly とかで使いにくい機能をどうしようかという話になっていて、リフレクションもその1つです。

その他にもいくつか、対応プラットフォームが増えることで必要な作業がちらほらと。

## Feature Switch

iOS とかの AOT (事前ネイティブ コンパイル)シナリオだと使わない機能はコンパイル時に削ってしまいたいわけですが、
そのためのスイッチを用意したいみたいな話も出ていたりします。

- [Feature Switch](https://github.com/dotnet/designs/blob/master/accepted/2020/feature-switch.md)

`Regex` の `RegexOptions.Compiled` とか、めったに使わず、使わない場合削ってしまえればコード量が結構多いみたいです。
あと、暗号系のアルゴリズムとかは、アプリごとに1度「これ」と決めてしまえばほとんどの場合その1個だけしか使わないと思いますが、この場合、使わないものを軒並み決してしまいたかったり。

こういう特定機能のスイッチは、 Target Framework の亜種を増やすのではなくて、
以下のような感じの `bool` フラグでソースコード中で切り替え処理をしてほしそうです。
([Hardware Intrinsics](../../../2018/12/hdintrinsic/index.md)とかでやっているのと同じ方式です。)

```csharp
internal static class FeatureDefiningType
{
    [FeatureSwitch("System.Runtime.OptionalFeatureBehavior")]
    internal static bool IsOptionalFeatureEnabled { get; }
}
```

## 国際化対応

カレンダーや時刻とかの国ごとに違う書式(2020年5月3日 か 3 May, 2020 か)とか、
文字ごとの Unicode カテゴリー、文字列のどこで改行していいかとか、
アルファベットの大文字・小文字の対応付けとか、
どれも結構大きなテーブルデータを必要とします。

これに対して、.NET Core の国際化対応は [ICU](http://site.icu-project.org/home) (International Components for Unicode) に依存していたりします。

で、これも iOS とかでどうしようかという話に。

- [Globalization support in .NET 5](https://github.com/dotnet/designs/pull/102)

デスクトップやサーバーOSには今どき大体 ICU が同梱されていたりするんですが。
Android も、API level 24 移行は [ICU4J](https://developer.android.com/guide/topics/resources/internationalization#relation) を持っていたりします。
問題が iOS と Web Assembly で、この辺りは OS/プラットフォーム側に ICU がないのでどうしようかという状態。

案としては、いっそ国際化対応を捨てるという方法。
どのカルチャーを取得しようとしても常に [`InvariantCulture`](https://docs.microsoft.com/ja-jp/dotnet/api/system.globalization.cultureinfo.invariantculture)を返してしまうというモードがあるみたいです。

- [.NET Core Globalization Invariant Mode](https://github.com/dotnet/runtime/blob/master/docs/design/features/globalization-invariant-mode.md)

別案としては、ICU をアプリ単位で同梱する方法。
これは、「OS のバージョンに依らず、所望の ICU バージョン(≒ Unicode バージョン)に依存したい」という場合にも使えます。

- [Add app-local support for ICU](https://github.com/dotnet/runtime/pull/35383)

ちなみに、ICU はテーブルデータを全部持とうとすると16MBくらいのサイズになるらしく、
アプリ1個1個に全部含めてしまうには少々でかいです。
ICU 自体には「必要な分だけ残してテーブルを削る」みたいな仕組みもあるらしくて、
iOS とか Web Assembly みたいな AOT シナリオのコンパイルにもテーブル削減の仕組みを組み込みたいというような話もあります。

- [Prototype ICU size reduction tooling](https://github.com/dotnet/runtime/issues/35092)
