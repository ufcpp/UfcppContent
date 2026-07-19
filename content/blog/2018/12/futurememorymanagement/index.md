---
title: "将来のメモリ管理"
source_url: "https://ufcpp.net/blog/2018/12/futurememorymanagement/"
content_type: "BlogEntry"
published_at: "2018-12-26T09:39:07"
updated_at: "2018-12-26T09:47:46"
tags: []
umbraco_id: 2209
parent_id: 2177
sort_order: 26
aliases: []
---

# 将来のメモリ管理

今日はまたちょっと将来の話。
リリース時期・本当にリリースされるか未定の機能で、メモリ管理がらみの話をまとめて。

## ヒープ確保の負担を減らしたい

メモリの管理方法には[スタック](../../../../study/computer/essential-software/memorymanagement.md#stack)と[ヒープ](../../../../study/computer/essential-software/memorymanagement.md#heap)があって、
一般的にはスタックの方が高速です。
スタックの方が制限がきついので、遅くてもしょうがなくヒープを使う場面がでてきます。

ヒープ管理は結構大きな負担なので、これを減らせば結構なパフォーマンス改善になります。
いくつか方向性があって、以下のような最適化が考えられます。

- ヒープ管理自体を賢くする
    - → [ガベージ コレクション](../../../../study/computer/essential-software/memorymanagement.md#garbage-collection)
    - → Arena Allocation (後述)
- ヒープを避ける
    - 手作業でヒープを避ける
        - C# が構造体とかの「[値型](../../../../study/csharp/resource/oo_reference.md)」を持っているのはこのため
        - → 先日書いた「[`Span<T>` 利用による最適化](../spanify/index.md)」とかもまさにこれ
    - 自動でヒープを避ける
        - → Object Stack Allocation (後述)

## ガベージ コレクション

一般的に、こまごまと個別に処理をするよりも、ある程度まとまった単位でごっそり処理する方が効率がよかったりします。
[ガベージ コレクション](../../../../study/computer/essential-software/memorymanagement.md#garbage-collection)というメモリ管理手法はまさにそんな感じで、
「ごみはしばらくほったらかし」→「定期的にごっそりまとめてゴミ集め」とかやっていて、
これがスループット的には結構有利だったりします。
「まとめてゴミ集め」の瞬間に負荷が集中するという問題はあるものの、
トータルでみると低負荷なメモリ管理手法です。

メモリ管理が手動(`alloc` したものは開発者が責任をもって `free` しないといけない)な言語でも、内部的にガベージ コレクション的な挙動をしているものがあるくらいです。
[gperftools](https://github.com/gperftools/gperftools)の tcmalloc なんかはそうみたいです。
小さいオブジェクトの場合だけですが、`free`した瞬間に即座にその領域を解放するのではなくて、
メモリが足りなくなってきたときにまとめて解放処理をします。

ガベージ コレクション以外のヒープ管理手法は、
たびたび提案されては、
そのたび「[世代別 GC](../../../../study/computer/essential-software/memorymanagement.md#generational)に性能で勝てなかった」的な結論に達したりします。
.NET でも、[Project Snowflake](https://www.infoq.com/jp/news/2017/10/snowflake)という研究プロジェクトがありました
([Rust](https://www.rust-lang.org/)みたいに、メモリの所有権をはっきりさせればヒープ管理が速くなるんじゃないかという原理に基づく提案)。
結構賢そうなことをやっているんですが、それでも結論は「[利益がコストに見合わない](https://github.com/dotnet/coreclr/issues/20074#issuecomment-423199088)」でした。

## Arena Allocation

Project Snowflake に代わって1つ有望視されているのは、
Arena Allocation という手法です。

[Protocol Buffers の C++ 版](https://developers.google.com/protocol-buffers/docs/reference/arenas)がこの手法によるメモリ管理を提供しているんですが、それを .NET にも導入できないかという調査をしているみたいです。
まだあんまりドキュメントがなく、[QConSF](https://qconsf.com/)の登壇で軽く紹介された程度ですが。

- [CLR/CoreCLR: How We Got Here & Where We're Going](https://qconsf.com/sf2018/presentation/clrcoreclr-how-we-got-here-where-were-going) ([ppt](https://qconsf.com/system/files/presentation-slides/coreclr_today_tomorrow.pptx))

これも、「ある程度まとまった単位でごっそり処理する方が高効率」という原理に則ったものです。
以下のように、「ごっそり消す」タイミングを明示するような方式。
メモリ放棄のまとまった単位を指して arena (舞台、競技場、界)と呼んでいます。

```csharp
Arena arena = new Arena();
using (arena.Activate())
{
    // この内側で new したものは「arena」内に確保される
}
// arena 内のオブジェクトは arena の Dispose 時にまとめて解放
arena.Dispose();
```

この方式はトランザクションのスコープがはっきりしているものに対して有効です。
Protocol Buffers が採用していることからわかるように、シリアライズ・デシリアライズが好例です。
シリアライズの途中でしか使わない一時的なバッファーを Arena 中に確保して、
シリアライズ完了時にまとめて解放すれば効率よくゴミを片付けられます。

## Object Stack Allocation

クラスのインスタンスは基本的にはヒープ上に確保するものなんですが、
「メソッド内で完結している」(引数にも渡さないし、戻り値にも返さない)という状況に限って、
スタック上に領域確保しても問題なく動いたりします。

そこで、JITコンパイラーが「メソッド内で完結している」かどうかを判定して、
完結していればクラスのインスタンスであってもスタック上に確保する最適化手法(Object Stack Allocation)があります。
「メソッド内から逃げ出していないかを解析する」という意味で、Escape Analysis と呼ばれたりもします。

Java SE では Java SE 6 の頃から Escape Analysis を実装しています。
Go なんかは最初から Escape Analysis ありきで作られています。
(要するに、結構昔からある最適化手法。)
それがこの度、.NET にも入りそうです。

- [Initial implementation of object stack allocation #20814](https://github.com/dotnet/coreclr/pull/20814)

Java と比べてずいぶん採用が遅いですが、
要は、C# は値型を持っているのでそもそも手作業でヒープを避ける手段があるからです。
挙句、「[`Span<T>` 利用による最適化](../spanify/index.md)」で説明したような手動最適化がの方が断然効果的なので、そっちの方が優先されています。
(Escape Analysis は「メソッド内から逃げ出していない」という条件が思いのほか厳しいので、適用できる割合はそんなに高くない。)

「コンパイラーとかランタイムが頑張るよりも手作業の方がまだまだ高効率」という話なので、
あんまり夢はない感じ…
(`Span<T>`も、「[ガベコレ](../../../../study/computer/essential-software/memorymanagement.md#garbage-collection)で追えるものを増やす」という作業は必要だったので、
ランタイムが何もしていないわけではないんですが。)
