---
title: "経過日数の計算"
source_url: "https://ufcpp.net/study/algorithm/other/o_days/"
content_type: "Article"
published_at: "2015-05-06T14:05:28"
updated_at: "2015-05-06T14:05:28"
tags: []
umbraco_id: 1143
parent_id: 1142
sort_order: 0
aliases:
  - "/study/algorithm/o_days.html"
---

# 経過日数の計算

## <a id="sec-generated-title-1"></a> <a id="abstract"></a>概要

2つの日付（例えば、自分の誕生日と今日）の間の経過日数を求めたくなったとします。
ぱっとは出てきませんね。
原因は主に、毎月の日数がばらばらなのと、うるう年のせいなんですが。

まあ、2つの日付の差というとちょっと面倒なんで、
とりあえず、グレゴリウス暦1年1月1日を基準にして、経過日数を求めることにします。
（グレゴリウス暦施行（早い国で1582年）より前の日付も、
形式的にグレゴリウス暦とみなして計算します。）
この基準日からの経過日数が分かれば、その差を取ることで、2つの日付の差も分かります。

まあ、大筋だけ言うと、
1年1月1日から<span class="math">y</span>年<span class="math">m</span>月<span class="math">d</span>日までの経過日数は、

* <span class="math">dy</span>:<span class="math">
          <span class="paren" style="font-size:em;">(</span>
            y <span class="normal">−</span><span class="normal">1</span>
          <span class="paren" style="font-size:em;">)</span>
          <span class="normal">×</span>
          <span class="normal">365</span>
        </span>

* <span class="math">dl</span>:<span class="math">y</span>年までのうるう年の回数

* <span class="math">dm</span>: 1月1日から<span class="math">m</span>月1日までの日数

* <span class="math">d</span>


という4つに分けて考えて、その和
<span class="math">
        dy
        <span class="normal">+</span>
        dl
        <span class="normal">+</span>
        dm
        <span class="normal">+</span>
        d <span class="normal">−</span><span class="normal">1</span>
      </span>
で求まります。

最初と最後の項については説明するまでもないと思うんで、
<span class="math">dl</span>（うるう年）と
<span class="math">dm</span>（<span class="math">m</span> 月までの日数）に関してを説明します。


## <a id="sec-generated-title-2"></a> <a id="leap"></a>うるう年

うるう年かどうかの判定は、

* 4の倍数の年はうるう年。

* ただし、100の倍数の年はうるう年じゃない。

* でも、やっぱり400の倍数の年はうるう年。


でできるので、
1年から <span class="math">y</span> 年までのうるう年の回数は、

* ＋<span class="math">
          <span class="paren" style="font-size:em;">⌊</span>
            y <span class="normal">/</span> 4
          <span class="paren" style="font-size:em;">⌋</span>
        </span>← 4年に1回、うるう年。

* －<span class="math">
          <span class="paren" style="font-size:em;">⌊</span>
            y <span class="normal">/</span> 100
          <span class="paren" style="font-size:em;">⌋</span>
        </span>← でも、100年に1回、うるう年でない年がある。

* ＋<span class="math">
          <span class="paren" style="font-size:em;">⌊</span>
            y <span class="normal">/</span> 400
          <span class="paren" style="font-size:em;">⌋</span>
        </span>← でも、やっぱり400年に1回はうるう年。


（ただし、記号 <span class="math">
        <span class="paren" style="font-size:em;">⌊</span>x<span class="paren" style="font-size:em;">⌋</span>
      </span> は、
<span class="math">x</span> を超えない最大の整数）
を足して、
<span class="math">
        <span class="paren" style="font-size:em;">⌊</span>
          y <span class="normal">/</span> 4
        <span class="paren" style="font-size:em;">⌋</span>
        <span class="normal">−</span>
        <span class="paren" style="font-size:em;">⌊</span>
          y <span class="normal">/</span> 100
        <span class="paren" style="font-size:em;">⌋</span>
        <span class="normal">+</span>
        <span class="paren" style="font-size:em;">⌊</span>
          y <span class="normal">/</span> 400
        <span class="paren" style="font-size:em;">⌋</span>
      </span>
で計算可能です。
で、プログラム的には、整数同士の除算は、普通は余り切り捨てなので、

```csharp {title="y 年までのうるう年の回数"}
y / 4 - y / 100 + y / 400;
```


となります。
さらにちょっとプログラミング上の工夫をするなら、
除算を極力避けるために、
÷4 をシフト演算で書き換えて、
以下のように書くことも可能。

```csharp {title="y 年までのうるう年の回数"}
int c = y / 100;
int dl = (y >> 2) - c + (c >> 2);
```



## <a id="sec-generated-title-3"></a> <a id="month"></a>月ごとの日数

とりあえず、
「1月1日から <span class="math">m</span> 月1日までの経過日数」とかいう長い言葉を何度も言いたくないので、
記号を定義しておきます。

* <span class="math">
          d<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
        </span>:<span class="math">m</span>月の日数

* <span class="math">
          s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
        </span>: 1月1日から<span class="math">m</span>月1日までの経過日数 ＝ 先月までの<span class="math">
          d<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
        </span>の和


まずは、
<span class="math">
        d<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
      </span> と
<span class="math">
        s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
      </span> の値の一覧を見てみましょう
（表1）。
ここでは、うるう年は無視します。
14月まである理由は後述します。

<table summary="d(m), s(m)">
	<caption>
		d(m), s(m)
	</caption>
	<tr>
		<th>月<span class="math">m</span></th>
		<td markdown="1">1</td>
		<td markdown="1">2</td>
		<td markdown="1">3</td>
		<td markdown="1">4</td>
		<td markdown="1">5</td>
		<td markdown="1">6</td>
		<td markdown="1">7</td>
		<td markdown="1">8</td>
		<td markdown="1">9</td>
		<td markdown="1">10</td>
		<td markdown="1">11</td>
		<td markdown="1">12</td>
		<td markdown="1">13</td>
		<td markdown="1">14</td>
	</tr>
	<tr>
		<th><span class="math">
            d<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
          </span></th>
		<td markdown="1">31</td>
		<td markdown="1">28</td>
		<td markdown="1">31</td>
		<td markdown="1">30</td>
		<td markdown="1">31</td>
		<td markdown="1">30</td>
		<td markdown="1">31</td>
		<td markdown="1">31</td>
		<td markdown="1">30</td>
		<td markdown="1">31</td>
		<td markdown="1">30</td>
		<td markdown="1">31</td>
		<td markdown="1">31</td>
		<td markdown="1">28</td>
	</tr>
	<tr>
		<th><span class="math">
            s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
          </span></th>
		<td markdown="1">0</td>
		<td markdown="1">31</td>
		<td markdown="1">59</td>
		<td markdown="1">90</td>
		<td markdown="1">120</td>
		<td markdown="1">151</td>
		<td markdown="1">181</td>
		<td markdown="1">212</td>
		<td markdown="1">243</td>
		<td markdown="1">273</td>
		<td markdown="1">304</td>
		<td markdown="1">334</td>
		<td markdown="1">365</td>
		<td markdown="1">396</td>
	</tr>
</table>


まあ、たった12個の整数ですし、
この <span class="math">
        s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
      </span> を定数テーブルで持っておけば解決する話なんですけど、
それじゃ面白くないんで別の方法を紹介。
（無駄にテーブルを持ちたくないですし。）

まず、2月だけうるう年の問題があったり、
他の月と比べて極端に日数が少ないので、例外として扱いってしまいたいです。
そこで、1月と2月を「前年の13月と14月」とみなして、
3～14月にして考えます。

28日しかない2月を末尾に移したので、
<span class="math">
        s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
      </span> の差
<span class="math">
        s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span><span class="normal">−</span>
        s<span class="paren" style="font-size:em;">(</span>
          m <span class="normal">−</span><span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
は全て 30 か 31 のどちらかになります。
そうすると、
30 と 31 の出てくる順番は少々不規則ですが、
近似的になら直線で表せそうです。
（近似的に、というか、
格子点の隙間を通して、小数点以下切り捨てることで表すような感じ。）

すなわち、
<span class="math">
        s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="paren" style="font-size:em;">⌊</span>
          a m <span class="normal">+</span> b
        <span class="paren" style="font-size:em;">⌋</span>
      </span>
となるような直線 <span class="math">
        a m <span class="normal">+</span> b
      </span> を探してみることにします。
要は、<span class="math">m</span> ＝ 3～14 に対して、
<div class="math">
      s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span><span class="normal">≦</span>
      a m <span class="normal">+</span> b
      <span class="normal">&lt;</span>
      s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
    </div>
という条件を満たすような実数 <span class="math">a, b</span> を求めることになります。

まあ、
傾き <span class="math">a</span> は、
30 と 31 の間を取って、30.5 前後の値になることは直感的に分かると思います。
頑張っていろいろ計算すると、
<span class="math">a</span> の範囲が
<div class="math">
      <span class="normal">30.57143</span>
      <span class="normal">≅</span>
      <table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">214</span>
        </td></tr><tr><td>
          <span class="normal">7</span>
        </td></tr></table>
      <span class="normal">≦</span>
      a
      <span class="normal">&lt;</span><table class="frac" summary="fraction"><tr><td class="num">
          <span class="normal">245</span>
        </td></tr><tr><td>
          <span class="normal">8</span>
        </td></tr></table><span class="normal">=</span><span class="normal">30.625</span>
    </div>
のとき、上述の条件を満たすように出来ることが分かります。
（
214/7 という値は、
2点
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          m, s<span class="paren" style="font-size:em;">(</span>m<span class="paren" style="font-size:em;">)</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">5</span>, <span class="normal">120</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">12</span>, <span class="normal">334</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
の傾きで、
245/8 の方は
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">6</span>, <span class="normal">151</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
と
<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">14</span>, <span class="normal">396</span>
        <span class="paren" style="font-size:em;">)</span>
      </span>
の傾き。
）

例えば、きり良く
<span class="math">
        a <span class="normal">=</span><span class="normal">30.6</span>,
      </span><span class="math">
        b <span class="normal">=</span><span class="normal">−</span><span class="normal">32.4</span>
      </span>
とかにして、
<div class="math">
      dm <span class="normal">=</span><span class="paren" style="font-size:em;">⌊</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">306</span> m <span class="normal">−</span><span class="normal">324</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">/</span>
        <span class="normal">10</span>
      <span class="paren" style="font-size:em;">⌋</span>
    </div>
で計算したり、
あるいは、
除算を避けるために、
<span class="math">
        a <span class="normal">=</span><span class="normal">979</span><span class="normal">/</span><span class="normal">32</span> ,
      </span><span class="math">
        b <span class="normal">=</span><span class="normal">−</span><span class="normal">1033</span><span class="normal">/</span><span class="normal">32</span>
      </span>
にして、
<div class="math">
      dm <span class="normal">=</span><span class="paren" style="font-size:em;">⌊</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">979</span> m <span class="normal">−</span><span class="normal">1033</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">/</span>
        <span class="normal">32</span>
      <span class="paren" style="font-size:em;">⌋</span>
    </div>
で計算します。
プログラム的に書くなら、÷32 はシフト演算に置き換えられて、以下のようになります。

```csharp {title="1月1日から m 月1日までの日数"}
int dm = (m * 979 - 1033) >> 5;
```



## <a id="sec-generated-title-4"></a> <a id="sample"></a>完成品

結局、これまでに説明した内容をまとめると、
1年1月1日からの経過日数を求めるプログラムは以下のようになります。

```csharp {title="グレゴリウス暦1年1月1日からの経過日数を求める"}
/// <summary>
/// グレゴリウス暦1年1月1日からの経過日数を求める。
/// （グレゴリウス暦施行前の日付も、
///   形式的にグレゴリウス暦と同じルールで計算。）
/// </summary>
/// <param name="y">年</param>
/// <param name="m">月</param>
/// <param name="d">日</param>
/// <returns>1年1月1日からの経過日数</returns>
static int GetDays(int y, int m, int d)
{
  // 1・2月 → 前年の13・14月
  if (m <= 2)
  {
    --y;
    m += 12;
  }
  int dy = 365 * (y - 1); // 経過年数×365日
  int c = y / 100;
  int dl = (y >> 2) - c + (c >> 2); // うるう年分
  int dm = (m * 979 - 1033) >> 5; // 1月1日から m 月1日までの日数
  return dy + dl + dm + d - 1;
}
```


ちなみに、
経過日数の計算ができれば、曜日の判定も可能です。
有名な曜日判定法に、[ツェラーの公式](http://ja.wikipedia.org/wiki/%E3%83%84%E3%82%A7%E3%83%A9%E3%83%BC%E3%81%AE%E5%85%AC%E5%BC%8F)ってのがあるんですが、
この公式は、このページで説明した内容と同様にして導出できます。

参考までに、ツェラーの公式による曜日判定プログラムを書いておくと、以下の通りです。

```csharp {title="ツェラーの公式に基づく曜日判定"}
/// <summary>
/// 曜日判定
/// </summary>
/// <param name="y">年</param>
/// <param name="m">月</param>
/// <param name="d">日</param>
/// <returns>0なら日曜、1: 月曜、…、6: 土曜</returns>
static int GetDayOfWeek(int y, int m, int d)
  int c = y / 100;
  y %= 100;
  int dow = d + 26 * (m + 1) / 10 + y + y / 4  + c / 4 - 2 * c;
  dow %= 7;
```


月 <span class="math">m</span> の前に 26 という謎の定数が出てきますが、
これは、「[月ごとの日数](#month)」で出てきた定数 306 を 70 ＝ 7×10 で割った余りです。
