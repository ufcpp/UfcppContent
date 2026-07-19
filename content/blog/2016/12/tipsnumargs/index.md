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

```csharp
class Program
{
    static void M(
int x0, int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, int x10, int x11, int x12, int x13, int x14, int x15,
int x16, int x17, int x18, int x19, int x20, int x21, int x22, int x23, int x24, int x25, int x26, int x27, int x28, int x29, int x30, int x31,
int x32, int x33, int x34, int x35, int x36, int x37, int x38, int x39, int x40, int x41, int x42, int x43, int x44, int x45, int x46, int x47,
int x48, int x49, int x50, int x51, int x52, int x53, int x54, int x55, int x56, int x57, int x58, int x59, int x60, int x61, int x62, int x63,
int x64, int x65, int x66, int x67, int x68, int x69, int x70, int x71, int x72, int x73, int x74, int x75, int x76, int x77, int x78, int x79,
int x80, int x81, int x82, int x83, int x84, int x85, int x86, int x87, int x88, int x89, int x90, int x91, int x92, int x93, int x94, int x95,
int x96, int x97, int x98, int x99, int x100, int x101, int x102, int x103, int x104, int x105, int x106, int x107, int x108, int x109, int x110, int x111,
int x112, int x113, int x114, int x115, int x116, int x117, int x118, int x119, int x120, int x121, int x122, int x123, int x124, int x125, int x126, int x127,
int x128, int x129, int x130, int x131, int x132, int x133, int x134, int x135, int x136, int x137, int x138, int x139, int x140, int x141, int x142, int x143,
int x144, int x145, int x146, int x147, int x148, int x149, int x150, int x151, int x152, int x153, int x154, int x155, int x156, int x157, int x158, int x159,
int x160, int x161, int x162, int x163, int x164, int x165, int x166, int x167, int x168, int x169, int x170, int x171, int x172, int x173, int x174, int x175,
int x176, int x177, int x178, int x179, int x180, int x181, int x182, int x183, int x184, int x185, int x186, int x187, int x188, int x189, int x190, int x191,
int x192, int x193, int x194, int x195, int x196, int x197, int x198, int x199, int x200, int x201, int x202, int x203, int x204, int x205, int x206, int x207,
int x208, int x209, int x210, int x211, int x212, int x213, int x214, int x215, int x216, int x217, int x218, int x219, int x220, int x221, int x222, int x223,
int x224, int x225, int x226, int x227, int x228, int x229, int x230, int x231, int x232, int x233, int x234, int x235, int x236, int x237, int x238, int x239,
int x240, int x241, int x242, int x243, int x244, int x245, int x246, int x247, int x248, int x249, int x250, int x251, int x252, int x253, int x254, int x255,
int x256
        )
    { }
}
```

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
