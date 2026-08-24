---
title: "段階コンパイル (Tiered Compilation)"
source_url: "https://ufcpp.net/blog/2018/12/tieredcompilation/"
content_type: "BlogEntry"
published_at: "2018-12-30T00:43:31"
updated_at: "2018-12-30T01:01:08"
tags: []
umbraco_id: 2213
parent_id: 2177
sort_order: 30
aliases: []
---

# 段階コンパイル (Tiered Compilation)

今日は .NET Core 2.1 の頃に実装されて(有効にするには設定が必要)、
<strike>.NET Core 2.2 からは既定で有効になる</strike>ランタイムの最適化の話。

(※ Preview の頃にいったん規定で有効になったものの、結局、リリースでは opt-in に戻した模様。後述する gen0 最適化問題とかがあるせいかも。)

.NET Core 2.1 の頃に以下のようなブログが出ていました。

- [Tiered Compilation Preview in .NET Core 2.1](https://blogs.msdn.microsoft.com/dotnet/2018/08/02/tiered-compilation-preview-in-net-core-2-1/)

これも今までそんなにここで取り上げていないので、
.NET Core 2.2 が出た今、「在庫処分」的に取り上げようかと思います。

## JIT コンパイルの段階化

Java とか C# みたいな Just-In-Time コンパイル方式の言語では、常に以下のようなジレンマを持っています。

- 中間コードからネイティブ コードに実行時にコンパイルするので、あんまりコンパイルに時間をかけるとかえって遅くなる
- かといってコンパイル時の最適化がしょぼいと、出てくるネイティブ コードが遅い

長期的に動かすのであれば、最適化に時間をかけてでも良いネイティブ コードを生成する方が得になったりはします。
ただし、それは最終的にトータルでは速くなるという話で、起動にかかる時間は最適化を頑張ろうとするほど遅くなります。
また、そんなに頑張って最適化した結果が報われるような「よく通る経路」は、全体の一部分でしかありません。

そこで、以下のように、段階的な最適化を考えます。

- 初めて呼ばれるメソッドは一律「最適化なし」でJITコンパイルする
- 呼ばれた回数をカウントして、一定数を越えたら「最適化あり」でJITコンパイルしなおして差し替える

こういう手法を段階コンパイル(tiered compilaition)と言います。


## coreclr の段階コンパイル

段階コンパイルは、例えば Java は Java 7 の時に導入したそうで、そんなに目新しい手法でもありません。
.NET Core でも、2016 にはプロトタイプ実装があったそうです。
ただ、.NET はもともと NGEN (事前ネイティブ化)や MulticoreJit (並列コンパイル)などで起動時間の短縮に取り組んできていました。
それに、[`Span<T>`構造体](../../../../study/csharp/resource/span.md)による最適化とか、違う角度でのパフォーマンス改善などもいろいろあって、
なかなか段階コンパイルの優先度が上がらなかったみたいです。

それでも、.NET Core 2.1 には組み込まれて、.NET Core 2.2 ではついに既定動作として段階コンパイルが有効になりました。

技術的には以下のような実装になっているそうです。

- 肝となるのは、実行時に、すでにJITコンパイル済みのコードを改めてJITコンパイルしなおして差し替える機能
  - それ単体で [CodeVersionManager](https://github.com/dotnet/coreclr/blob/master/Documentation/design-docs/code-versioning.md) という名前がついていてドキュメントがある
  - 段階コンパイル以外にも、あまり使われていないコードを破棄することで省メモリ化したり、診断ログを採るためのコードを動的に差し込むのに使えます
- 初期状態を Tier0、頻繁に呼ばれていて最適化したいコードを Tier1 という名前で分類
  - ただ、現状では、元々デバッグ用にある最適化のオン・オフをそのまま使って、Tier0 なら最適化なし、Tier1 なら最適化ありでコンパイルしているだけ
- 差し替えは並列動作可能
  - メソッド A から別のメソッド B を呼んでいる最中に、A が Tier1 に昇格・再 JIT した場合、B からの戻り先アドレスに補正をかけるような処理がちゃんと働く

## 今後

[上記のブログ](https://blogs.msdn.microsoft.com/dotnet/2018/08/02/tiered-compilation-preview-in-net-core-2-1/)が書かれた頃から、
さらに改善も始まっているみたいです。

計画としては例えば、メソッドが呼ばれた回数のカウントは今の実装はあまり効率的なものじゃないことがわかっているそうなので、そこは直したいとのこと。
あと、Tier1 コンパイルはシングル スレッドで行われているそうなので、ここを並列化したいという話もあります。

### Tier0 最適化

単純な「Tier0 なら最適化なし」という実装に問題があることも分かっていて、修正が必要です。

例えば、ジェネリックなメソッド `M<T>` で、以下のように、`T` が値型かどうかを判別するハックがあったりします。

```csharp {title="型引数が値型かどうかを判別する方法"}
void M<T>()
{
    if (default(T) == null) Console.WriteLine("T は参照型");
    else Console.WriteLine("T は値型");
}
```

この C# コードで、`T` が値型のとき、IL 上は、`default(T) == null` のところでボックス化が起きるコードになっています。
`default(T)` を一度 `object` 型にした上で null と比較するような IL が生成されます。
しかし、最適化ありでJITコンパイルすると、このボックス化はちゃんと消える(それどころかJIT時定数扱いで条件分岐自体が消える)ので、
`default(T) == null` は `T` が値型かどうかを判別する最速の手段だったりします。

その一方で、今、Tier0 JIT ではこの最適化が働かないそうです。
その結果、Tier0 の時にはボックス化が発生して、かえって遅くなることがわかっています。
(それで、[せっかく入れた最適化を戻されかけたこともあったりします](https://github.com/dotnet/coreclr/pull/19904)。)
なので、[Tier0 でも、手間のかからない範囲の最低限の最適化はかけるべき](https://github.com/dotnet/coreclr/issues/14474)だろうという話になっています。

### AggressiveOptimization

ちなみに、NET Core 3.0 では、段階コンパイルを制御するためのオプションが追加されます。

今でも、`MethodImpl`属性(`System.Runtime.CompilerServices`名前空間)を使ってメソッドの最適化を制御できます。
例えばこれに、`MethodImplOptions.AggressiveInlining`オプションを指定すると、通常よりもインライン化されやすくなります。
.NET Core 3.0 では、この`MethodImplOptions`列挙型に`AggressiveOptimization`が追加されていて、
このオプションを指定すると最初から Tier1 扱いでJITコンパイルするようになるみたいです。
