---
title: "接ベクトル・微分形式の内在的定義"
source_url: "https://ufcpp.net/study/math/manifold/intrinsic/"
content_type: "Article"
published_at: "2015-05-06T14:18:31"
updated_at: "2015-05-06T14:18:31"
tags: []
umbraco_id: 1520
parent_id: 1515
sort_order: 4
aliases:
  - "/manifold/intrinsic"
  - "/manifold/intrinsic.html"
  - "/math/manifold/intrinsic/"
  - "/study/manifold/intrinsic"
  - "/study/manifold/intrinsic.html"
---

# 接ベクトル・微分形式の内在的定義

##<a id="sec-generated-title-1"></a> <a id="abstract"></a>概要
具体的な座標をとって考えるなら、
接ベクトルは多様体上の微分演算子 <span class="math"><table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x</denom></td></tr></table></span>、
微分形式は微小変分 <span class="math"><span class="normal">d</span>x</span> と考えることができます。
 
しかし、
曲面や曲線の概念を、せっかく抽象化して多様体という概念を得たのに、
接ベクトルや微分形式に関しては具体的な座標に依存するような定義が必要なのはいまいちではないでしょうか。
 
そこで、現在では、
具体的な座標系の導入を必要としない、
多様体をユークリッド空間の中に埋め込んで考える必要もない、
多様体の外に何の過程もおかず多様体の内側だけで完結するような定義の仕方が考えられています。
このような定義の仕方を内在的定義といいます。


##<a id="sec-generated-title-2"></a> <a id="plan"></a>執筆予定
<pre>
・接ベクトル
接ベクトル ＝ 微分演算子で定義

<span class="math">
  <table class="frac" summary="differential"><tr><td class="num">∂</td></tr><tr><td>∂<denom>x</denom></td></tr></table> f ＝ lim_δx→0 {f(x ＋ δx) － f(x)} / δx
</span>
とかで定義すると内在的にできないので、

a を実数
f, g を多様体 M → <span class="bold">R</span> の C^∞ 級関数として、

v(f ＋ g) ＝ v(f)＋v(g)
v(a f) ＝ a v(f)
v(f g) ＝ v(f) g ＋ f v(g)

を満たすような C^∞(M) → <span class="bold">R</span> の写像を接ベクトル場として定義。
M 上点 p での接ベクトルを T_p M で表す。
M 上の接ベクトル場全体を T M で表す。

・微分形式
接ベクトルの双対空間として定義
→ なので、余接ベクトル場と呼ぶ

T_p M の双対 T*_p M: T_p M → <span class="bold">R</span> を p での余接ベクトル
その全体 T* M ＝ ∪T*_p M を余接ベクトル場と呼ぶ

また、p 本の接ベクトルから実数への写像
(T_p M)^p → <span class="bold">R</span>
で、多重線形性
T(D1, D2, ・・・, aDi ＋ bDi', ・・・, Dp)
＝
a T(D1, D2, ・・・, Di, ・・・, Dp)
＋
b T(D1, D2, ・・・, Di', ・・・, Dp)
を見たすものを p-共変テンソルと呼びます。

p-共変テンソル T(D1, D2, ・・・, Dp) （Di は接ベクトル）で、
さらに交代性
T(D1, D2, ・・・, Di, ・・・, Dj, ・・・, Dp)
＝
－T(D1, D2, ・・・, Dj, ・・・, Di, ・・・, Dp)
を満たすものを微分 p 形式と呼ぶ。

当然、微分 1 形式 ＝ 余接ベクトル
あと、微分 0 形式 ＝ スカラー

逆に、q 本の余接ベクトル → 実数の写像
(T*_p M)^q → <span class="bold">R</span>
で、多重線形性を満たすものを q-反変テンソルと呼ぶ。

p 本の接ベクトルと q 本の余接ベクトル → 実数のものは
p 共変, q 反変テンソル、もしくは、(p, q) テンソル

・外微分
リー微分 L_D と内部積 ι_D を使って定義。

任意の接ベクトル D に対して
L_D ＝ ι_D d ＋ d ι_D
を満たすような演算子 d があるんで、それを外微分とする。

ω が p 形式のとき、
ι_D ω は p－1 形式になるんで、
ι_D dω ＝ d(ι_Dω) － L_Dω
で、p－1 形式の外微分から p 形式の外微分が再帰的に定義される。
</pre>
