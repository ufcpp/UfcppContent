---
title: "小ネタ 引数の個数の上限"
source_url: "https://ufcpp.net/blog/2016/12/tipsnumargs/"
content_type: "BlogEntry"
published_at: "2016-12-22T15:06:44"
updated_at: "2016-12-22T13:30:26"
tags: []
umbraco_id: 2005
parent_id: 1969
sort_order: 21
aliases: []
---

# 小ネタ 引数の個数の上限

引数の個数に制限があること、ご存じでしょうか。 むやみに多くても実装上の無駄が大きかったりしますし、上限が決まっていたりします。

C#は意外と大きくて、最大で65536個まで行けます。要するに2バイト分。
ということで、以下のC#コードはコンパイル可能です。
1バイトで収まらない、0～256までの257個の引数。

<pre class="source" title="257個の引数">
<code><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> M(
<span class="reserved">int</span> x0, <span class="reserved">int</span> x1, <span class="reserved">int</span> x2, <span class="reserved">int</span> x3, <span class="reserved">int</span> x4, <span class="reserved">int</span> x5, <span class="reserved">int</span> x6, <span class="reserved">int</span> x7, <span class="reserved">int</span> x8, <span class="reserved">int</span> x9, <span class="reserved">int</span> x10, <span class="reserved">int</span> x11, <span class="reserved">int</span> x12, <span class="reserved">int</span> x13, <span class="reserved">int</span> x14, <span class="reserved">int</span> x15,
<span class="reserved">int</span> x16, <span class="reserved">int</span> x17, <span class="reserved">int</span> x18, <span class="reserved">int</span> x19, <span class="reserved">int</span> x20, <span class="reserved">int</span> x21, <span class="reserved">int</span> x22, <span class="reserved">int</span> x23, <span class="reserved">int</span> x24, <span class="reserved">int</span> x25, <span class="reserved">int</span> x26, <span class="reserved">int</span> x27, <span class="reserved">int</span> x28, <span class="reserved">int</span> x29, <span class="reserved">int</span> x30, <span class="reserved">int</span> x31,
<span class="reserved">int</span> x32, <span class="reserved">int</span> x33, <span class="reserved">int</span> x34, <span class="reserved">int</span> x35, <span class="reserved">int</span> x36, <span class="reserved">int</span> x37, <span class="reserved">int</span> x38, <span class="reserved">int</span> x39, <span class="reserved">int</span> x40, <span class="reserved">int</span> x41, <span class="reserved">int</span> x42, <span class="reserved">int</span> x43, <span class="reserved">int</span> x44, <span class="reserved">int</span> x45, <span class="reserved">int</span> x46, <span class="reserved">int</span> x47,
<span class="reserved">int</span> x48, <span class="reserved">int</span> x49, <span class="reserved">int</span> x50, <span class="reserved">int</span> x51, <span class="reserved">int</span> x52, <span class="reserved">int</span> x53, <span class="reserved">int</span> x54, <span class="reserved">int</span> x55, <span class="reserved">int</span> x56, <span class="reserved">int</span> x57, <span class="reserved">int</span> x58, <span class="reserved">int</span> x59, <span class="reserved">int</span> x60, <span class="reserved">int</span> x61, <span class="reserved">int</span> x62, <span class="reserved">int</span> x63,
<span class="reserved">int</span> x64, <span class="reserved">int</span> x65, <span class="reserved">int</span> x66, <span class="reserved">int</span> x67, <span class="reserved">int</span> x68, <span class="reserved">int</span> x69, <span class="reserved">int</span> x70, <span class="reserved">int</span> x71, <span class="reserved">int</span> x72, <span class="reserved">int</span> x73, <span class="reserved">int</span> x74, <span class="reserved">int</span> x75, <span class="reserved">int</span> x76, <span class="reserved">int</span> x77, <span class="reserved">int</span> x78, <span class="reserved">int</span> x79,
<span class="reserved">int</span> x80, <span class="reserved">int</span> x81, <span class="reserved">int</span> x82, <span class="reserved">int</span> x83, <span class="reserved">int</span> x84, <span class="reserved">int</span> x85, <span class="reserved">int</span> x86, <span class="reserved">int</span> x87, <span class="reserved">int</span> x88, <span class="reserved">int</span> x89, <span class="reserved">int</span> x90, <span class="reserved">int</span> x91, <span class="reserved">int</span> x92, <span class="reserved">int</span> x93, <span class="reserved">int</span> x94, <span class="reserved">int</span> x95,
<span class="reserved">int</span> x96, <span class="reserved">int</span> x97, <span class="reserved">int</span> x98, <span class="reserved">int</span> x99, <span class="reserved">int</span> x100, <span class="reserved">int</span> x101, <span class="reserved">int</span> x102, <span class="reserved">int</span> x103, <span class="reserved">int</span> x104, <span class="reserved">int</span> x105, <span class="reserved">int</span> x106, <span class="reserved">int</span> x107, <span class="reserved">int</span> x108, <span class="reserved">int</span> x109, <span class="reserved">int</span> x110, <span class="reserved">int</span> x111,
<span class="reserved">int</span> x112, <span class="reserved">int</span> x113, <span class="reserved">int</span> x114, <span class="reserved">int</span> x115, <span class="reserved">int</span> x116, <span class="reserved">int</span> x117, <span class="reserved">int</span> x118, <span class="reserved">int</span> x119, <span class="reserved">int</span> x120, <span class="reserved">int</span> x121, <span class="reserved">int</span> x122, <span class="reserved">int</span> x123, <span class="reserved">int</span> x124, <span class="reserved">int</span> x125, <span class="reserved">int</span> x126, <span class="reserved">int</span> x127,
<span class="reserved">int</span> x128, <span class="reserved">int</span> x129, <span class="reserved">int</span> x130, <span class="reserved">int</span> x131, <span class="reserved">int</span> x132, <span class="reserved">int</span> x133, <span class="reserved">int</span> x134, <span class="reserved">int</span> x135, <span class="reserved">int</span> x136, <span class="reserved">int</span> x137, <span class="reserved">int</span> x138, <span class="reserved">int</span> x139, <span class="reserved">int</span> x140, <span class="reserved">int</span> x141, <span class="reserved">int</span> x142, <span class="reserved">int</span> x143,
<span class="reserved">int</span> x144, <span class="reserved">int</span> x145, <span class="reserved">int</span> x146, <span class="reserved">int</span> x147, <span class="reserved">int</span> x148, <span class="reserved">int</span> x149, <span class="reserved">int</span> x150, <span class="reserved">int</span> x151, <span class="reserved">int</span> x152, <span class="reserved">int</span> x153, <span class="reserved">int</span> x154, <span class="reserved">int</span> x155, <span class="reserved">int</span> x156, <span class="reserved">int</span> x157, <span class="reserved">int</span> x158, <span class="reserved">int</span> x159,
<span class="reserved">int</span> x160, <span class="reserved">int</span> x161, <span class="reserved">int</span> x162, <span class="reserved">int</span> x163, <span class="reserved">int</span> x164, <span class="reserved">int</span> x165, <span class="reserved">int</span> x166, <span class="reserved">int</span> x167, <span class="reserved">int</span> x168, <span class="reserved">int</span> x169, <span class="reserved">int</span> x170, <span class="reserved">int</span> x171, <span class="reserved">int</span> x172, <span class="reserved">int</span> x173, <span class="reserved">int</span> x174, <span class="reserved">int</span> x175,
<span class="reserved">int</span> x176, <span class="reserved">int</span> x177, <span class="reserved">int</span> x178, <span class="reserved">int</span> x179, <span class="reserved">int</span> x180, <span class="reserved">int</span> x181, <span class="reserved">int</span> x182, <span class="reserved">int</span> x183, <span class="reserved">int</span> x184, <span class="reserved">int</span> x185, <span class="reserved">int</span> x186, <span class="reserved">int</span> x187, <span class="reserved">int</span> x188, <span class="reserved">int</span> x189, <span class="reserved">int</span> x190, <span class="reserved">int</span> x191,
<span class="reserved">int</span> x192, <span class="reserved">int</span> x193, <span class="reserved">int</span> x194, <span class="reserved">int</span> x195, <span class="reserved">int</span> x196, <span class="reserved">int</span> x197, <span class="reserved">int</span> x198, <span class="reserved">int</span> x199, <span class="reserved">int</span> x200, <span class="reserved">int</span> x201, <span class="reserved">int</span> x202, <span class="reserved">int</span> x203, <span class="reserved">int</span> x204, <span class="reserved">int</span> x205, <span class="reserved">int</span> x206, <span class="reserved">int</span> x207,
<span class="reserved">int</span> x208, <span class="reserved">int</span> x209, <span class="reserved">int</span> x210, <span class="reserved">int</span> x211, <span class="reserved">int</span> x212, <span class="reserved">int</span> x213, <span class="reserved">int</span> x214, <span class="reserved">int</span> x215, <span class="reserved">int</span> x216, <span class="reserved">int</span> x217, <span class="reserved">int</span> x218, <span class="reserved">int</span> x219, <span class="reserved">int</span> x220, <span class="reserved">int</span> x221, <span class="reserved">int</span> x222, <span class="reserved">int</span> x223,
<span class="reserved">int</span> x224, <span class="reserved">int</span> x225, <span class="reserved">int</span> x226, <span class="reserved">int</span> x227, <span class="reserved">int</span> x228, <span class="reserved">int</span> x229, <span class="reserved">int</span> x230, <span class="reserved">int</span> x231, <span class="reserved">int</span> x232, <span class="reserved">int</span> x233, <span class="reserved">int</span> x234, <span class="reserved">int</span> x235, <span class="reserved">int</span> x236, <span class="reserved">int</span> x237, <span class="reserved">int</span> x238, <span class="reserved">int</span> x239,
<span class="reserved">int</span> x240, <span class="reserved">int</span> x241, <span class="reserved">int</span> x242, <span class="reserved">int</span> x243, <span class="reserved">int</span> x244, <span class="reserved">int</span> x245, <span class="reserved">int</span> x246, <span class="reserved">int</span> x247, <span class="reserved">int</span> x248, <span class="reserved">int</span> x249, <span class="reserved">int</span> x250, <span class="reserved">int</span> x251, <span class="reserved">int</span> x252, <span class="reserved">int</span> x253, <span class="reserved">int</span> x254, <span class="reserved">int</span> x255,
<span class="reserved">int</span> x256
        )
    { }
}
</code></pre>

確かJavaだと、256個までだったはずです。1バイト分。

こういう制限、Javaや.NETの場合、何によって制約されるかというと、中間コードの命令セットに依ります。例えば、.NETの場合だと、引数参照のために以下のような命令を持っています。

命令 | op code | 概要 | 命令サイズ
---- | ---- | ---- | ----
`ldarg.0` | `02` | 最初の引数をスタックにロードする | 1バイト
`ldarg.1` | `03` | 2つ目の引数をスタックにロードする | 1バイト
`ldarg.2` | `04` | 3つ目の引数をスタックにロードする | 1バイト
`ldarg.3` | `05` | 4つ目の引数をスタックにロードする | 1バイト
`ldarg.s` | `0E <index>` | 1バイトのオペランドで指定したインデックスの引数をスタックにロードする | 命令1バイト+オペランド1バイト
`ldarg` | `FE 09 <index>` | 2バイトのオペランドで指定したインデックスの引数をスタックにロードする | 命令2バイト+オペランド2バイト

4つ目の引数まで(0～3番目)なら1バイトで参照できます。
4～255番目までなら2バイト、そして、それ以上になると4バイト必要になります。
というように、オペランドによってプログラム サイズがでかくならないように、よく使うものほど短く、そうでないものほど長くなるように、複数の命令が用意されています。

この場面で効いてくるJavaと.NETの最大の差は、中間コード(Javaの場合はbyte code、.NETの場合はILと呼ばれてるやつ)の命令長の差です。
.NETは可変長になっていて、多くの命令が1バイトですが、いくつか2バイト命令を持っています。
上記の`ldarg` (2バイト オペランドの方)もその1つで、めったに使わないであろう命令に2バイトのコードを割り当てています。

一方、Javaのbyte codeは1バイト固定長の命令セットになっています。
使える命令は最大で256個ですし、無駄な命令はあまり入れたくありません。

まあ、引数の数が257個以上になるというのはほとんどないでしょう…
と締めたいところですが、ごくまれに、「機械生成で作ったコードで257個超えてJavaでコンパイル エラーになった」なんていう恐ろしいことを言いだす人も見かけるので侮れません。
そういう人が実際にいたから、.NETは`ldarg`命令を用意したんでしょうかね…
