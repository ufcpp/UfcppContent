---
title: "変数"
source_url: "https://ufcpp.net/study/xml/summary/variable/"
content_type: "Article"
published_at: "2015-05-06T14:24:20"
updated_at: "2015-07-07T18:43:00"
tags: []
umbraco_id: 1656
parent_id: 1650
sort_order: 5
aliases:
  - "/study/summary/xml/variable"
  - "/study/summary/xml/variable/"
  - "/study/testxsl/variable.html"
---

# 変数

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

[<code>variable.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/variable.xsl) には、変数定義、参照、一覧表示のための template が記述されています。

var-group タグに、
変数の一覧を記述しておきます。
各変数 variable には文字、名前（日英）、単位（次元）、概要を記述します。

定義した変数は、use タグで参照することができます。
use タグを記述した場所には、変数の文字が表示されます。
文字にカーソルを合わせると、ポップアップで変数の意味が表示されます。
さらに、文字をクリックすると、変数の定義のある場所にジャンプします。


## <a id="sec-generated-title-2"></a> <a id="source"></a>ソース

```xml {title="ソース"}
<section title="結果" id="result">
  <p>
    変数の参照→ <math><use id="freq" /></math>、<math><use id="afreq" /></math>。
  </p>
</section>

<var-group>
  <variable id="freq">
    <letter>
      f
    </letter>
    <name>周波数</name>
    <ename>frequency</ename>
    <unit>Hz</unit>
    <summary>周期信号が、1秒間に何回繰り返されるか。</summary>
  </variable>
  <variable id="afreq">
    <letter>
      ω
    </letter>
    <name>角周波数</name>
    <ename>angular frequency</ename>
    <unit>rad</unit>
    <summary>
      <math>
        <use id="afreq" /> ＝ 2 π <use id="freq" />
      </math>
    </summary>
  </variable>
</var-group>
```

## <a id="sec-generated-title-3"></a> <a id="result"></a>結果

変数の参照→ <span class="math"><a href="#freq" title="周波数">
        f
      </a></span>、<span class="math"><a href="#afreq" title="角周波数">
        ω
      </a></span>。

<tbody>
<tr>
	<td>
<a id="freq"><span class="math"><nobr>
        f
      </nobr></span></a>
	</td>
	<td>
周波数
	</td>
	<td>
	frequency
	</td>
	<td>
	<unit>Hz</unit>
</td>
	<td>
	<summary>周期信号が、1秒間に何回繰り返されるか。</summary>
</td>
</tr>
<tr>
	<td>
<a id="afreq"><span class="math"><nobr>
        ω
      </nobr></span></a>
	</td>
	<td>
角周波数
	</td>
	<td>
	angular frequency
	</td>
	<td>
	<unit>rad</unit>
</td>
	<td>
	<summary>
        <span class="math">
          <a href="#afreq" title="角周波数">
        ω
      </a> ＝ 2 π <a href="#freq" title="周波数">
        f
      </a>
        </span>
      </summary>
</td>
</tr>
</tbody>
