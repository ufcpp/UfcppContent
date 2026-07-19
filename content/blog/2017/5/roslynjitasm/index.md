---
title: "TryRoslyn 改め SharpLab で、JIT Asm表示"
source_url: "https://ufcpp.net/blog/2017/5/roslynjitasm/"
content_type: "BlogEntry"
published_at: "2017-05-18T11:42:28"
updated_at: "2017-05-18T11:42:28"
tags: []
umbraco_id: 2068
parent_id: 2059
sort_order: 2
aliases: []
---

# TryRoslyn 改め SharpLab で、JIT Asm表示

[いつの間にか](https://twitter.com/matarillo/status/864469038008631296)と話題に。
[去年の年末にちょこっと紹介したTryRoslyn](../../../2016/12/tipstryroslyn/index.md)がJIT ASM (JITの結果がどういうネイティブコードになっているかを見れる)機能に対応してたみたいです。

TryRoslyn、今は[SharpLab](https://github.com/ashmind/SharpLab)っていう名前に変わって、ドメインも取った見たいです:

- [https://sharplab.io/](https://sharplab.io/)

## ネイティブ コードの確認

例えばこんな感じのコードを書いて、

<pre class="source" title="">
<code><span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Math</span>;

<span class="reserved">public</span> <span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">double</span> M(<span class="reserved">double</span> x, <span class="reserved">double</span> y) =&gt; Exp(x) * Sin(y);
}
</code></pre>

[SharpLabに貼り付けて](https://sharplab.io/#f:>asmr/K4Zwlgdg5gBCAuBDeYDGMDKBPBBTAtgHQCyyAFgNwCwAULQA7ABGANmjKi4iCDAMK0A3rRiiYjVuwAmAe2YtcMYgApZ8xQA8ANDDWtFWAJQwAvAD4YAUQ31lG4wCpMkZUeo0AvrSA===)、
ページ内の「Decompiled」のコンボボックスで「JIT Asm」を選ぶと、以下のような結果が出ます
(現時点での結果)。

<pre class="source" title="">
<code><span class="comment">; This is an experimental implementation.
; Please report any bugs to https://github.com/ashmind/TryRoslyn/issues.

; Desktop CLR v4.6.1590.00 (clr.dll) on x86.</span>

C..ctor()
    L0000: <span class="reserved">ret</span>

C.M(Double, Double)
    L0000: <span class="reserved">fld</span> qword [esp+0xc]
    L0004: <span class="reserved">sub</span> esp, 0x8
    L0007: <span class="reserved">fstp</span> qword [esp]
    L000a: <span class="reserved">call</span> System.Math.Exp(Double)
    L000f: <span class="reserved">fld</span> qword [esp+0x4]
    L0013: <span class="reserved">fsin</span>
    L0015: <span class="reserved">fmulp</span> st1, st0
    L0017: <span class="reserved">ret</span> 0x10
</code></pre>

C#は、C# → IL にコンパイルする時点ではそこまで大きな最適化はしていなくて、
インライン展開とかの処理はIL → ネイティブコードに JIT コンパイルするタイミングで行っていたりします。
なので、本気で最適化に取り組みたかったらネイティブコードまで追わないと細かいところがわからなかったりします。

昔、[Visual Studio上でネイティブコードまで追う方法](../../../2016/12/tipsildasm/index.md)とかも書きましたけど、
そこそこ面倒な手順だったりしますし。
さらっとオンラインで確認して見れるのは結構ありがたいかも。

## 中身

なんか流れを見るに、

- .NETチームが、主に自分たちの動作確認用に使う目的でJIT結果を見れるライブラリ([jitutils](https://github.com/dotnet/jitutils))を去年くらいから作ってる
- .NETチームの中の人が[SharpLabの方に「これ使うとすげぇいいんじゃないか」みたいなissueを立てる](https://github.com/ashmind/SharpLab/issues/39)
- 最近ついにCharpLabに組み込まれた

という感じみたい。

ということで、最終的には.NETチームが作っているツールに行きつくみたいです。
