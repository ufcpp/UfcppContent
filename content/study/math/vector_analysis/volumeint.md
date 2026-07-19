---
title: "体積積分"
source_url: "https://ufcpp.net/study/math/vector_analysis/volumeint/"
content_type: "Article"
published_at: "2015-05-06T14:17:40"
updated_at: "2015-05-06T14:17:40"
tags: []
umbraco_id: 1495
parent_id: 1491
sort_order: 3
aliases:
  - "/math/vector_analysis/volumeint/"
  - "/study/vector_analysis/volumeint"
  - "/study/vector_analysis/volumeint.html"
  - "/vector_analysis/volumeint"
  - "/vector_analysis/volumeint.html"
---

# 体積積分

## <a id="sec-generated-title-1"></a> <a id="volumeint"></a>体積積分とは

空間上のある領域<span class="math">V</span>上で定義されるスカラー場<span class="math">f</span>に対して
<div class="math">
      <em>
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table>f <span class="normal">d</span>V
      </em>
    </div>
を<span class="math">f</span>の<span class="math">V</span>上での<strong id="volumeint" class="keyword">体積積分</strong>という。
ここで<span class="math">
        <span class="normal">d</span>V
      </span>は微小体積素です。

イメージ的には<span class="math">f</span>は空間密度で、<span class="math">
        <span class="integral">∫</span><table class="integral" summary="integral"><tr><td class="intsup"> </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub">V</td></tr></table>f <span class="normal">d</span>V
      </span>は領域<span class="math">V</span>上での<span class="math">f</span>の総和という感じで捉えてください。


## <a id="sec-generated-title-2"></a> <a id="cartesian"></a>体積積分の直交座標系での表現

直交座標系における体積素<span class="math">
        <span class="normal">d</span>V
      </span>は図1のようなものをイメージしてもらってかまいません。
そして、体積積分を直交座標を用いて表現すると
<div class="math">
      <em>
        <span class="integral">∫<span style="margin-left:-0.5em;">∫</span><span style="margin-left:-0.5em;">∫</span></span><table class="integral" summary="integral"><tr><td class="intsup">  </td></tr><tr><td style="font-size:30%;"> </td></tr><tr><td class="intsub"></td></tr></table> f<span class="normal">d</span>x<span class="normal">d</span>y<span class="normal">d</span>z
      </em>
    </div>
となります。

<figure>
	[![体積素](../../../../assets/media/ufcpp2000/math/volumeint1.png)](../../../../assets/media/ufcpp2000/math/volumeint1.png)
	<figcaption>体積素</figcaption>
</figure>
