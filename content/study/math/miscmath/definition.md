---
title: "1＋1"
source_url: "https://ufcpp.net/study/math/miscmath/definition/"
content_type: "Article"
published_at: "2015-05-06T14:18:37"
updated_at: "2015-05-06T14:18:37"
tags: []
umbraco_id: 1523
parent_id: 1521
sort_order: 1
aliases:
  - "/math/miscmath/definition/"
  - "/miscmath/definition"
  - "/miscmath/definition.html"
  - "/study/miscmath/definition"
  - "/study/miscmath/definition.html"
---

# 1＋1

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

小中学生がよく抱くらしい疑問、
「
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">2</span></span>
はなぜ？」
とか
「
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
はなぜ？」
とか、
答えられますか？
 
まあ、ぶっちゃけていうと、
これを定義にしてしまってもいいんですよね。
でもその一方で、他の定義からの導出も可能です。


## <a id="sec-generated-title-2"></a> <a id="two"></a>1＋1＝2

<span class="math">
        <span class="normal">1</span>
        <span class="normal">+</span>
        <span class="normal">1</span>
        <span class="normal">=</span>
        <span class="normal">2</span>
      </span>
は定義だから証明できないとか言う人もいますが、
本当にそう思います？
 
まず、以下の3つの条件を見てみてください。

1. 自然数 1 の次に小さい自然数を 2 とする

2. <span class="math">a</span>を自然数とすると、<span class="math">a <span class="normal">+</span><span class="normal">1</span></span>は<span class="math">a</span>の次に小さい自然数である

3. <span class="math">
          <span class="normal">1</span>
          <span class="normal">+</span>
          <span class="normal">1</span>
          <span class="normal">=</span>
          <span class="normal">2</span>
        </span>


条件 1 と 2 があれば、
「1の次に小さいのを2と呼ぶ」かつ
「
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span></span>
は1の次に小さい自然数」
なので、
結果として条件 3 が得られます。
 
一方で、
条件 2 と 3 を仮定すれば条件 1 がいえますし、
条件 1 と 3 を仮定すれば条件 2 がいえます。
要するに、3つの条件のうち、2つだけが仮定としなければならないもので、
残り1つは定理として導けます。
 
実を言うなら、
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">2</span></span>
を定義とする公理系があってもいいし、
定理として導ける公理系があってもいい。
前者の公理系を採用するなら、
当然、
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">2</span></span>
は定義であって証明はできない。
後者の公理系を採用するなら、もちろん証明可能な命題になる。
 
結局、
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">2</span></span>
を定義とするかどうか、選択権が残っているんですよね。
どれを選択するかは、
通常、
「もっとも条件数が少ない定義の仕方を選ぶ」か、
「一番直感的なものを選ぶ」のいずれかの方針で決定します。
 
この3つの条件だと、どれとどれを選んでも2つは必要なので、
直感的に分かりやすいのがどれかという問題になりますね。
個人的には1と2だと思うんですけど、人によるかも。


## <a id="sec-generated-title-3"></a> <a id="two2"></a>1＋1＝2（直感的な説明）

前節で、
1 とか 2 とか ＋ とかがどういう条件を満たすべきなのか、
以下のような3つの条件を出しました。

1. 自然数 1 の次に小さい自然数を 2 とする

2. <span class="math">a</span>を自然数とすると、<span class="math">a <span class="normal">+</span><span class="normal">1</span></span>は<span class="math">a</span>の次に小さい自然数である

3. <span class="math">
          <span class="normal">1</span>
          <span class="normal">+</span>
          <span class="normal">1</span>
          <span class="normal">=</span>
          <span class="normal">2</span>
        </span>


このうち、2つの条件があれば、残りの1個は導出可能なんですが、
じゃあ、どの2個を定義としてどの1個を定理にするのが一番直感的に分かりやすいでしょう。
 
そのあたりのことを考えるために、
一度、自然数という発想にいたる経緯を追いなおしてみましょう。
自然数という概念は、物の個数を数えることから始まったわけで、
1 とか 2 とかいう数字は以下の例のような感じで決められています。

<blockquote markdown="1">
部屋に自分だけがいるときの人数を1と数える。
部屋にもう1人入ってきたら2人、
さらにもう1人入ってきたら3人、・・・、
と数える。

</blockquote>
さて、これを論理的に言い表そうと思ったらどうすればいいんでしょう。
やっぱり、
「
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">2</span></span>
ですよ」としか言い表せないんでしょうか。
 
まあ、上述の文章もいくつかに分解して考えられますね。

* 物の数の増減には基本単位がある。その単位を 1 と表す。

* 数には「1個増える」という操作が可能である。

* ある数に「1個増える」という操作を施した物をその数の「次」と呼ぶものとして、 「1の次」を 2 で表す。


まあ、こんな所です。
これとあと、いちいち言葉で「次」とか言うと面倒なので、
記号を定義しておきましょう。

* <span class="math">a</span>の次を<span class="math">a <span class="normal">+</span><span class="normal">1</span></span>と書く。


このあたりのことを述べているのが、
前節の条件 1, 2 なわけですね。
ここまで決めれば、必然的に
<span class="math"><span class="normal">1</span><span class="normal">+</span><span class="normal">1</span><span class="normal">=</span><span class="normal">1の次</span><span class="normal">=</span><span class="normal">2</span></span>
となります。


## <a id="sec-generated-title-4"></a> <a id="minus"></a>(-1)×(-1)＝1

<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">×</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="normal">1</span>
      </span>
も同様に、定義にもできるし、他の定義から導くこともできる。
 
ここでは、以下のようにして負の数を定義してみましょう。

1. 任意の自然数<span class="math">a</span>に対して、<span class="math">a <span class="normal">+</span><span class="normal">0</span><span class="normal">=</span> a</span>となるような元 0 の存在を認める。

2. 自然数<span class="math">a</span>に対して、<span class="math">a <span class="normal">+</span> b <span class="normal">=</span><span class="normal">0</span></span>を満たす数<span class="math">b</span>を<span class="math">a</span>の加法に関する逆元と呼び、<span class="math"><span class="normal">−</span>a</span>と書く。

3. 自然数<span class="math">a</span>と、ここで定義した 0、<span class="math"><span class="normal">−</span>a</span>をあわせて、整数と呼ぶ。

4. 整数は（すなわち、<span class="math"><span class="normal">−</span>a</span>も）自然数と同じ和・差・積の公式に従う。


このとき、以下のような定理が逐次示されます。

1. 任意の整数<span class="math">m</span>に対して、<span class="math">m <span class="normal">×</span><span class="normal">0</span><span class="normal">=</span><span class="normal">0</span></span>。

2. <span class="math">
a <span class="normal">+</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span>a<span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span>a<span class="paren" style="font-size:em;">)</span><span class="normal">+</span> a
<span class="normal">=</span><span class="normal">0</span></span>なので、 定義から、<span class="math"><span class="normal">−</span>a</span>の逆元は<span class="math">a</span>。 すなわち、<span class="math"><span class="normal">−</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span>a<span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
a
</span>。

3. 自然数と同じ法則が成り立つという仮定から、 整数にも分配法則が成り立ち、<span class="math">
a <span class="normal">+</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span>a
<span class="normal">=</span>
a<span class="normal">×</span><span class="normal">1</span><span class="normal">+</span> a<span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span>
a <span class="normal">×</span><span class="paren" style="font-size:1.2em;">(</span><span class="normal">1</span><span class="normal">+</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="paren" style="font-size:1.2em;">)</span><span class="normal">=</span>
a<span class="normal">×</span><span class="normal">0</span><span class="normal">=</span><span class="normal">0</span></span>なので、<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span>a
<span class="normal">=</span><span class="normal">−</span>a
</span>。

4. 定理 3 で、特に<span class="math">a <span class="normal">=</span><span class="normal">−</span><span class="normal">1</span></span>を代入すれば、<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">−</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span></span>となり、 定理 2 から<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">−</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>。


はい、ちゃんと定理として
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
が得られました。
「整数も自然数と同じ演算法則に従う」というのを定義にしたらこの結果が得られたわけですから、
結局、整数を自然数の自然な拡張にしたければ、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
にならざるを得ないということです。
 
これも、1＋1 の時と同じく、順序を逆にすることも可能で、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
の方（と <span class="math"><span class="normal">−</span><span class="normal">1</span></span> が交換法則と結合法則を満たすこと）を定義としてしまえば、
整数が自然数と同じ演算法則に従う（分配法則も満たす）ことを示せます。
まあ、交換法則と結合法則は仮定するくせに、
分配法則を仮定しないというのも変な話なので、
これはあんまりいい定義だとは思えないですけど。


## <a id="sec-generated-title-5"></a> <a id="minus2"></a>(-1)×(-1)＝1 （直感的な説明）

<span class="math">
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">×</span>
        <span class="paren" style="font-size:em;">(</span>
          <span class="normal">−</span>
          <span class="normal">1</span>
        <span class="paren" style="font-size:em;">)</span>
        <span class="normal">=</span>
        <span class="normal">1</span>
      </span>
の方も直感的な数の概念と食い違っていないか検証してみましょう。
 
まず、0 に関しては、「誰もいない部屋の状態を 0 とする」とでもしておけばいいでしょう。
ただ、これだと、負の数は考え出すことができません。
これは、誰もいない状態よりも人を減らせないのが問題なので、
ちょっと捻りを加えてみましょう。

<blockquote markdown="1">
部屋の代わりにちょっと大きめの建物を考えます。
建物全体に何人いるかはわからないんですが、
入口付近を見張ることで、
何人入ってきて何人出ていくかを数えることができます。

今現在、建物内にいる人間の数を基準にして、増減だけを見てみましょう。
今の状態が 0 です。
こうすると、0 よりも人が減る可能性があります。
そして、人が増えるというのを正の数、
人が減るというのを負の数で表せそうですね。

<span class="math">a</span> 人増えて、さらに続けて <span class="math">a</span> 人減ると、
トータルの増減は 0 になります。

</blockquote>
最後の一文を持って負の数を定義しようというのが、
以下の条件になるわけです。

* 自然数<span class="math">a</span>に対して、<span class="math">a <span class="normal">+</span> b <span class="normal">=</span><span class="normal">0</span></span>を満たす数<span class="math">b</span>を<span class="math">a</span>の加法に関する逆元と呼び、<span class="math"><span class="normal">−</span>a</span>と書く。


前節の通り、この定義と、
「負の数も自然数と同じ演算法則に従って欲しい」という要請だけでも、
十分に
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
が導出できます。
 
一方で、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
の方を定義にしてしまってもいいということに正当性を持たせるために、
以下のようなシチュエーションを考えてみましょう。

<blockquote markdown="1">
「1分あたり <span class="math">a</span> 人の増減がある」
というような状況を考えます。
このとき、
<span class="math">b</span> 分“前”は今より何人多/少なかったでしょう。
また、
<span class="math">b</span> 分“後”は今より何人多/少なくなるでしょう。

人の出入りと同じく、時間にも向きが付きました。
ここで、
<span class="math">b</span> 分“後”の方を自然数 <span class="math">b</span> で、
<span class="math">b</span> 分“前”の方を負の数 <span class="math"><span class="normal">−</span>b</span> で表すことにしましょう。

「毎分 <span class="math">a</span> 人の増加で <span class="math">b</span> 分後」
なら、自然数同士の掛け算なので、答えは <span class="math">a<span class="normal">×</span>b</span> ですね。

「毎分 <span class="math">a</span> 人の減少で <span class="math">b</span> 分後」なら、
トータルで <span class="math">a<span class="normal">×</span>b</span> の減少なので、
減少は負で表すというルールから <span class="math"><span class="normal">−</span>a<span class="normal">×</span>b</span> 人と書く。

同様に、
「毎分 <span class="math">a</span> 人の増加で <span class="math">b</span> 分前」なら、
今よりも <span class="math">a<span class="normal">×</span>b</span> 人少なかったはずですから、
<span class="math"><span class="normal">−</span>a<span class="normal">×</span>b</span> 人。

「毎分 <span class="math">a</span> 人の減少で <span class="math">b</span> 分前」なら、
今よりも <span class="math">a<span class="normal">×</span>b</span> 人多かったはずですから、
<span class="math">a<span class="normal">×</span>b</span> 人。

</blockquote>
結果をまとめると以下の通りです。

<table summary="人の増減">
	<caption>
		人の増減
	</caption>
	<tr>
		<td markdown="1"></td>
		<th><span class="math">b</span>分後</th>
		<th><span class="math">b</span>分前</th>
	</tr>
	<tr>
		<th>毎分<span class="math">a</span>人の増</th>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span>人多い</td>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span>人少ない</td>
	</tr>
	<tr>
		<th>毎分<span class="math">a</span>人の減</th>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span>人少ない</td>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span>人多い</td>
	</tr>
</table>


<table summary="増は＋、減は－。後は＋、前は－のルール">
	<caption>
		増は＋、減は－。後は＋、前は－のルール
	</caption>
	<tr>
		<td markdown="1"></td>
		<th><span class="math">b</span></th>
		<th><span class="math">
            <span class="normal">−</span>b</span></th>
	</tr>
	<tr>
		<th>毎分<span class="math">a</span></th>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span></td>
		<td markdown="1"><span class="math">
            <span class="normal">−</span>a<span class="normal">×</span>b</span></td>
	</tr>
	<tr>
		<th>毎分<span class="math"><span class="normal">−</span>a</span></th>
		<td markdown="1"><span class="math">
            <span class="normal">−</span>a<span class="normal">×</span>b</span></td>
		<td markdown="1"><span class="math">a<span class="normal">×</span>b</span></td>
	</tr>
</table>


ちゃんと負の数同士の積が正の数になっていますね。
これで、
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
の方が定義だって言ってしまっても、
別に直感的に違和感のあるものではなくなりました。
 
また、前述の通り、
「負の数も自然数と同じ演算法則に従って欲しい」
という要請だけでも
<span class="math"><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">×</span><span class="paren" style="font-size:em;">(</span><span class="normal">−</span><span class="normal">1</span><span class="paren" style="font-size:em;">)</span><span class="normal">=</span><span class="normal">1</span></span>
が言えてしまうわけですが、
「この結論は直感に反するものではない」
ということも言えます。


## <a id="sec-generated-title-6"></a> <a id="plan"></a>執筆予定

```text
結局、「定義の仕方次第」

-a という記号を、「かけて 1 になるような元」（要するに乗法に関する逆元）に使っても、
（有益かどうかを別にするなら）誰も文句はいいません。

1＋1＝0 になるような ＋演算があってもかまいません。

そんなことして意味があるかどうかを考えなければ、
もっと奇抜な法則を持っていたってかまわない。
集合 {1, a, △} に対して、
1＋1＝a, a＋1＝△, △＋1＝1
1×1＝1, 1×a＝a, 1×△＝△
a×a＝1, a×△＝△, △×△＝△
と定義したり。
（こんな変なのでも矛盾なく加減乗除できるし、分配法則も成り立ってるはず）
```

```text
あと、
「証明のできない公理はどこまで減らすことができるだろう？」
ということも考えていくと面白い。
その1つの到達点が、現在の公理的集合論
```
→ 参考： 「[数学](../index.md)」。
