---
title: "ディジタルフィルタプログラミング"
source_url: "https://ufcpp.net/study/sp/digital_filter/program/"
content_type: "Article"
published_at: "2007-03-24T00:00:00"
updated_at: "2015-05-06T14:22:25"
tags: []
umbraco_id: 1612
parent_id: 1610
sort_order: 1
aliases:
  - "/study/digital_filter/program.html"
---

# ディジタルフィルタプログラミング

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

ディジタルフィルタの実装の例として、
C# 言語による実装を示します。
 
言語に C# を採用する理由は以下の通り。

* ディジタルフィルタとオブジェクト指向は相性がよい

* オブジェクト指向言語といえば、今の時代、Java か C#

* このサイトは C# サイトなので、C# で（参考：「[C# によるプログラミング入門](../../csharp/index.md)」）


まず、このページでは、
様々なディジタルフィルタに共通の「[インターフェース](../../csharp/oop/oo_interface.md#interface)」と、
ディジタルフィルタの中でも基礎中の基礎になる「増幅器」および「遅延器」の実装例を示します。
 
移行のページでは、ディジタルフィルタの説明とともに、
サンプルプログラムを掲載（予定）。


## <a id="sec-generated-title-2"></a> <a id="interface"></a>ディジタルフィルタインターフェース

まあ、とりあえず、先に C# のソースを示します。
（プログラミングの知識に関しては、
「[インターフェース](../../csharp/oop/oo_interface.md)」辺りを参照してください。）

```csharp
/// <summary>
/// ディジタルフィルタインターフェース。
/// </summary>
public interface IFilter : ICloneable
{
  /// <summary>
  /// フィルタリングを行い、その結果を返す。
  /// </summary>
  /// <param name="x">フィルタ入力。</param>
  /// <returns>フィルタ出力。</returns>
  double GetValue(double x);

  /// <summary>
  /// フィルタの内部状態をクリアする。
  /// </summary>
  void Clear();
}
```


完全版: 
[IFilter.cs](../../../../assets/media/ufcpp2000/sp/src/IFilter.cs)

線形フィルタの場合、
信号を1つ入力して、フィルタリング結果の信号を1つ得るようなモデルがよく使われます。
（「信号を1つ」というのは、
音声信号の場合なら「1サンプル」、
画像の場合なら「1画素」とかのことです。
このページでは、主に音声向けに説明しているので、
「1サンプル」という言い方を頻繁に使います。）
この、「1サンプルの入力を与え、1サンプルの出力を得る」という操作を行うのが、
GetValue メソッドです。
 
また、ディジタルフィルタは、内部状態を持っている（過去数サンプル分の入力を記憶していたり）する場合がありますが、
時折、この内部状態を初期状態に戻したい場合があります。
そのための操作用に Clear メソッドも用意しておきます。


## <a id="sec-generated-title-3"></a> <a id="amplifier"></a>増幅器

最も単純な作りのディジタルフィルタの例として、
増幅器という物を考えて見ます。
<strong id="amplifier" class="keyword">増幅器</strong>（amplifier）は、
本当に単純に、「入力信号の定数倍を出力する」というフィルタです。
 
この場合、状態を持つ（過去の信号を記憶したりする）必要も全くなく、
実装も非常に簡単です。
要点だけ抜き出すと、以下の通り。

```csharp
public class Amplifier : IFilter
{
  double amp; // 倍率

  /// <summary>
  /// 倍率
  /// </summary>
  public double Amplitude
  {
    get { return this.amp; }
    set { this.amp = value; }
  }

  public double GetValue(double x)
  {
    return this.amp * x;
  }

  public void Clear()
  {
  }
}
```


完全版: 
[Amplifier.cs](../../../../assets/media/ufcpp2000/sp/src/Amplifier.cs)


## <a id="sec-generated-title-4"></a> <a id="buffer"></a>過去の信号の記憶

もう1つの基本中の基本となるフィルタである遅延器の説明に入る前に、
過去の信号の記憶について実装例を示します。
 
遅延器というと、何サンプルか前の信号を出力するフィルタのことです。
したがって、遅延器を作るためには、
過去何サンプルかの信号を記憶しておく必要があります。
 
そのための記憶領域としては、
以下のような「[クラス](../../csharp/oop/oo_class.md#class)」を作っておくと便利です。

1. n サンプル前までの値を保持している。

2. n ＋ 1 サンプルより前の値は随時上書きされて消えていく。

3. <code>buf[i]</code>と言うような書き方で、i サンプル前の値を読み書きできる。


このようなクラスを、ここでは、<strong id="cbuffer" class="keyword">循環バッファ</strong>（circular buffer）と呼びましょう。
（一般用語としては確立した物ではない。
でも、この意味で循環バッファという言葉を使う人が多いはず。）
 
循環バッファの実装方法は、例えば以下のようになります。

```csharp
public class CircularBuffer : ICloneable
{
  double[] buf;

  // 中略

  /// <summary>
  /// n サンプル前の値の取得
  /// </summary>
  /// <param name="n">何サンプル前の値を読み書きするか</param>
  /// <returns>n サンプル前の値</returns>
  public double this[int n]
  {
    get { return this.buf[n]; }
    set { this.buf[n] = value; }
  }

  /// <summary>
  /// 値の挿入
  /// </summary>
  /// <param name="x">挿入したい値</param>
  public void Insert(double x)
  {
    for (int n = this.buf.Length - 1; n > 0; --n)
    {
      this.buf[n] = this.buf[n - 1];
    }
    this.buf[0] = x;
  }

  /// <summary>
  /// 要素数
  /// </summary>
  public double Count
  {
    get { return this.buf.Length; }
  }
}
```


完全版: 
[CircularBuffer1.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer1.cs)

この場合、値の読み出しは高速に行えますが、
新しい値を挿入する際に、いちいち全要素を1つずつずらす作業が必要で、
効率がよくありません。
そこで、以下のような実装方法もあります。

```csharp
public class CircularBuffer : ICloneable
{
  double[] buf;
  int top;

  // 中略

  /// <summary>
  /// n サンプル前の値の取得
  /// </summary>
  /// <param name="n">何サンプル前の値を読み書きするか</param>
  /// <returns>n サンプル前の値</returns>
  public double this[int n]
  {
    get { return this.buf[(n + this.top) % this.buf.Length]; }
    set { this.buf[(n + this.top) % this.buf.Length] = value; }
  }

  /// <summary>
  /// 値の挿入
  /// </summary>
  /// <param name="x">挿入したい値</param>
  public void Insert(double x)
  {
    --this.top;
    if (this.top < 0) this.top += this.buf.Length;
    this.buf[this.top] = x;
  }

  /// <summary>
  /// 要素数
  /// </summary>
  public int Count
  {
    get { return this.buf.Length; }
  }
}
```


完全版: 
[CircularBuffer2.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer2.cs)

これで、値の挿入時に、全要素を1つずつずらすといった重たい処理はなくなりました。
でも、剰余演算も結構重たい演算で、避けれるならば避けたいものです。
これに対して、バッファ長を2の冪に制限することで、
剰余演算を論理 AND 演算に置き換える方法もあります。

```csharp
public class CircularBuffer : ICloneable
{
  double[] buf;
  int length;
  int top;
  int mask;

  /// <summary>
  /// n サンプル前の値の取得
  /// </summary>
  /// <param name="n">何サンプル前の値を読み書きするか</param>
  /// <returns>n サンプル前の値</returns>
  public double this[int n]
  {
    get { return this.buf[(this.top + n) & this.mask]; }
    set { this.buf[(this.top + n) & this.mask] = value; }
  }

  /// <summary>
  /// 値の挿入
  /// </summary>
  /// <param name="x">挿入したい値</param>
  public void Insert(double x)
  {
    --this.top;
    this.top &= this.mask;
    this.buf[this.top] = x;
  }

  /// <summary>
  /// 要素数
  /// </summary>
  public int Count
  {
    get { return this.length; }
  }
}
```


完全版: 
[CircularBuffer3.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer3.cs)

バッファ長を2の冪に制限するために、
「n を以上の最小の整数」を返す関数 <code>CeilPower2(n)</code> を用意したりします。
実装例はこちら → 
[Util.cs](../../../../assets/media/ufcpp2000/sp/src/Util.cs)


## <a id="sec-generated-title-5"></a> <a id="delay"></a>遅延器

「[循環バッファ](#cbuffer)」さえ出来れば、
遅延器の実装は簡単です。
過去数サンプル分のデータを循環バッファに記憶しておいて、
所望の位置のデータを出力するだけです。

```csharp
public class Delay : IFilter
{
  CircularBuffer buf;

  public Delay(int delaytime)
  {
    this.buf = new CircularBuffer(delaytime);
  }

  public double GetValue(double x)
  {
    double y = this.buf[this.buf.Count - 1];
    this.buf.Insert(x);
    return y;
  }

  public void Clear()
  {
    for (int n = this.buf.Count; n > 0; --n)
      this.buf.Insert(0);
  }
```


完全版: 
[Delay.cs](../../../../assets/media/ufcpp2000/sp/src/Delay.cs)
### <a id="sec-generated-title-6"></a>ソースファイルリスト

* 
[IFilter.cs](../../../../assets/media/ufcpp2000/sp/src/IFilter.cs)
* 
[Amplifier.cs](../../../../assets/media/ufcpp2000/sp/src/Amplifier.cs)
* 
[CircularBuffer1.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer1.cs)
* 
[CircularBuffer2.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer2.cs)
* 
[CircularBuffer3.cs](../../../../assets/media/ufcpp2000/sp/src/CircularBuffer3.cs)
* 
[Util.cs](../../../../assets/media/ufcpp2000/sp/src/Util.cs)
* 
[Delay.cs](../../../../assets/media/ufcpp2000/sp/src/Delay.cs)
