---
title: "小ネタ corefxlabとSystem.Memory"
source_url: "https://ufcpp.net/blog/2016/12/tipscorefxlab/"
content_type: "BlogEntry"
published_at: "2016-12-03T00:01:36"
updated_at: "2016-12-02T15:01:59"
tags:
  - "C# Tips 2016"
umbraco_id: 1980
parent_id: 1969
sort_order: 2
aliases: []
---

# 小ネタ corefxlabとSystem.Memory

## corefxlab

.NETチームは以下のリポジトリの1つに、[corefxlab](https://github.com/dotnet/corefxlab)というものがあります。
名前通り、[corefx](https://github.com/dotnet/corefx) (.NET の標準ライブラリ)入りを目指して試験的なコードを入れておく場所で、
安定したものからcorefxに移転になったりします。

corefxの方にいきなり入らず、一度「lab」を経ているわけで、
それなりに「これまでの.NETではあまり取り組んでなくてこなれていないもの」が含まれています。

その最たるものがパフォーマンス。
.NETは、これまでどちらかというと生産性や安全性が優先だったわけで、
「そんなコード書いてまでパフォーマンス求める？」みたいなものは少なかったんですが、
最近ではそういうコードが増えてきています。

もちろんこれまでもパフォーマンスは必要だったわけですが、
そういう場合はすぐにネイティブ コードに逃げていました。
.NETはネイティブ コードとの相互運用もしっかりしているわけで、それも1つの解決策です。

ですが、.NETもこなれてきて、.NET単体でのパフォーマンスに目を向ける余裕も出てきました。
それになんだかんだ言って、ネイティブ側、.NET側の双方で配慮しないとパフォーマンスが出ないことも多々あります。
それに取り組んでいるのがcorefxlabです。

といっても、そういう「そんなの書いてまで」的なコードは、型の中に閉じ込めています。
corefxlab中のライブラリの中身は結構な頑張り具合になっていますが、
単にビルド済みのライブラリを使うだけならそこまで変なライブラリでもないでしょう。

## System.Memory (旧 System.Slices)

そんなcorefxlab出身のライブラリの中から例として1つのライブラリを紹介します。

- [`System.Memory`](https://github.com/dotnet/corefx/tree/master/src/System.Memory)

昔は`System.Slices`という名前でcorefxlabにありました。
というか、まあ、さらに追加で試験的なコードを書く場所として、corefxlab側にも`System.Slices`が残ってはいます。(本当にやばそうなコードはcorefxlab側に取り残されています。[`Cast`](https://github.com/dotnet/corefxlab/blob/master/src/System.Slices/System/SpanExtensions_binary.cs#L22)とか。)

- [`System.Slices`](https://github.com/dotnet/corefxlab/tree/master/src/System.Slices)

Slices (切れ端、断片)という名前は歴史的経緯のようです。
現在`Span` (区間)と呼ばれている構造体も、昔は`Slice`という名前だったみたいです。
今となってはどこにも`Slice`という名前は残っていないし、corefxに移る際には`System.Memory`というパッケージ名に変わりました。

`Span<T>`構造体の持つ機能を単純化して書くと、以下のようなコードと同類の機能を提供するものです。

```csharp {title="単純化したSpan"}
struct Span
{
    public byte[] _data;
    public int _offset;
    // 本来は範囲チェックも必要なので length も必要

    public Span(byte[] data, int offset = 0)
    {
        _data = data;
        _offset = offset;
    }

    // C# 7登場に合わせて ref uint に変更予定
    public unsafe uint this[int index]
    {
        get
        {
            fixed (byte* p = _data)
            {
                var q = (uint*)(p + _offset);
                return q[index];
            }
        }
        set
        {
            fixed (byte* p = _data)
            {
                var q = (uint*)(p + _offset);
                q[index] = value;
            }
        }
    }

    public Span Slice(int startIndex) => new Span(_data, startIndex + _offset);
}
```

この単純化して示せている部分で言うと、`Span`の機能は以下の通りです。

- 配列の一部分だけを参照する
- 元の型(この例だと`byte`)とは違う型(同 `int`)の値を、配列に対して直接読み書きする

これに対してさらに、本物の `Span<T>` は以下のようなものです。

- 型を自由に変えれる
  - 任意の型の配列を受け取れる
      - ジェネリックを使える
  - `Cast<T, U>()`で、他の型に強制変換できる
      - `T`型の配列に、`U`の値を直接書き込める
- ヒープ(配列)、スタック(`stackalloc`)、ネイティブ バッファー(ポインター)を統一的に扱える
- `fixed` (それなりに負担がある操作)が不要でパフォーマンスがいい

たったこれだけのことなんですが、通常のC#では書けなかったりします(後述)。

まあ、とにかく、以下の例のようなことが、そこそこ高パフォーマンスで実行できます。

```csharp {title="Spanの利用例"}
using System;

struct Data
{
    public byte A;
    public byte B;
    public short C;
    public int D;
}

class Program
{
    static void Main(string[] args)
    {
        // データ書き込み用のバッファーを byte 配列で用意
        var data = new byte[24];

        // byte のまま書き込み
        var b = new Span<byte>(data);
        b[0] = 1;
        b[1] = 2;

        // short を書き込み
        var s = b.Slice(2).Cast<byte, ushort>();
        s[0] = 0x0403;
        s[1] = 0x0605;
        s[2] = 0x0807;

        // int
        var i = s.Slice(3).Cast<ushort, uint>();
        i[0] = 0x0C0B0A09;
        i[1] = 0x100F0E0D;

        // 構造体
        var l = i.Slice(2).Cast<uint, Data>();
        l[0] = new Data
        {
            A = 0x11,
            B = 0x12,
            C = 0x1413,
            D = 0x18171615,
        };

        // 1～24が並ぶはず
        Console.WriteLine(string.Join(" ", data));
    }
}
```

### 構造体の読み書き

特に、構造体を直接読み書きできるのは、使い方次第では非常に強力です。
構造体内部のレイアウトを気にしなければ、きわめて低コストなシリアライズが実現できる可能性があります。
(データ構造を値型のみの固定長にしないといけなかったり、
文字列を直接持てなかったり、
ARMで使うならバイト境界をまたげないはずだし、制限はきつい。
可能性があると言っても設計はそこそこ難しそう。)

レイアウトに関しては、以下の点に注意が必要です。

- エンディアン
  - サーバーもクライアントもIntel系かARM系のCPUを使う状況下ではリトル エンディアン固定で考えても特に問題は出ない
  - .NETが移植されてる先の大部分がリトル エンディアン(XBox？知らない子ですね…)
  - というか、ビッグ エンディアン自体がそこまでもう残ってない
      - 「ネットワーク エンディアン」とか言われてTCPとかの世代ではビッグ エンディアンが使われてるけども、最近だとQUICとかはリトル エンディアン
      - XBoxもXBox Oneで、PSもPS4でリトル エンディアンなCPUに変更
      - Java がビッグ エンディアンなのが唯一つらいところ…
- バイト境界
  - 32ビットCPUか64ビットCPUかで4バイト境界か8バイト境界かが変わるので注意
  - 心配なら[`StructLayout`属性](../../../../study/csharp/interop/memorylayout.md#layout-kind)を付ければどの環境でも同じレイアウトにできる

### C# で書けないコード

「通常の C# では書けない」と言いましたが、理由は、ポインターに対する制限が強いせい:

- ジェネリック型引数に対してポインターを作れない
- ポインターと[参照](../../../../study/csharp/resource/sp_ref.md#ref-returns)を相互変換できない
- ポインターだけでは[GC](../../../../study/csharp/resource/rm_gc.md#garbage-collection)的に安全に参照を持てない

じゃあ、どうやって`Span<T>`構造体を書いているかというと…
ILアセンブラーです。
ILが必要な部分は別の、[`System.Runtime.CompilerServices.Unsafe`](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)というライブラリに追い出していますが、こちらの中身はほぼ全部ILです。

- IL: [`Unsafe`クラス](https://github.com/dotnet/corefx/blob/master/src/System.Runtime.CompilerServices.Unsafe/src/System.Runtime.CompilerServices.Unsafe.il)
  - C#でコンパイル → ildasm で逆アセンブル → 属性中のコードでメソッドの中身を上書きするツール([ILSub](https://github.com/dotnet/corefxlab/tree/master/samples/ILsub))を通す → ilasm でアセンブル という黒魔術

IL を書くのは大変だし、ビルド手順も面倒になるしでなかなか保守がしんどそう。

まあ、こういうコードをC#だけで書けるようにしたいという提案は出ているんですが、
採用されるかどうかは不透明(少なくとも直近の作業には載ってない)。
参考: [Compiler intrinsics](../../5/pickuproslyn0529/index.md)
