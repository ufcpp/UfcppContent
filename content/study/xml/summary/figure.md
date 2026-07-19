---
title: "図表"
source_url: "https://ufcpp.net/study/xml/summary/figure/"
content_type: "Article"
published_at: "2015-05-06T14:24:22"
updated_at: "2015-07-07T18:26:39"
tags: []
umbraco_id: 1657
parent_id: 1650
sort_order: 6
aliases:
  - "/study/testxsl/figure"
  - "/study/testxsl/figure.html"
  - "/testxsl/figure"
  - "/testxsl/figure.html"
  - "/xml/summary/figure/"
---

# 図表

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

[<code>figure.xsl</code>](../../../../assets/media/ufcpp2000/xml/xslfiles/figure.xsl) には、図表用の template が記述されています。


## <a id="sec-generated-title-2"></a> <a id="source"></a>ソース

<pre class="xsource" title="ソース">
<code><span class="bracket">&lt;</span><span class="element">figure</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">image</span> <span class="attribute">src</span><span class="attvalue">="test.png"</span> <span class="attribute">width</span><span class="attvalue">="200"</span> <span class="attribute">height</span><span class="attvalue">="200"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">legend</span><span class="bracket">&gt;</span>テスト用の画像<span class="bracket">&lt;/</span><span class="element">legend</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">figure</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">figure</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">image</span> <span class="attribute">src</span><span class="attvalue">="test.png"</span> <span class="attribute">width</span><span class="attvalue">="100"</span> <span class="attribute">height</span><span class="attvalue">="100"</span> <span class="bracket">/&gt;</span>
  <span class="bracket">&lt;</span><span class="element">legend</span><span class="bracket">&gt;</span>←通し番号も付きます<span class="bracket">&lt;/</span><span class="element">legend</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">figure</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">table</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>i<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>ii<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>A<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>10<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>15<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>B<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>25<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>50<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">caption</span><span class="bracket">&gt;</span>表も書けます<span class="bracket">&lt;/</span><span class="element">caption</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;</span><span class="element">table</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">thead</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span><span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>あ<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>い<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">thead</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">tbody</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>イ<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>三<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>五<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;</span><span class="element">tr</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">th</span><span class="bracket">&gt;</span>ロ<span class="bracket">&lt;/</span><span class="element">th</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>八<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
      <span class="bracket">&lt;</span><span class="element">td</span><span class="bracket">&gt;</span>四<span class="bracket">&lt;/</span><span class="element">td</span><span class="bracket">&gt;</span>
    <span class="bracket">&lt;/</span><span class="element">tr</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;/</span><span class="element">tbody</span><span class="bracket">&gt;</span>
  <span class="bracket">&lt;</span><span class="element">caption</span><span class="bracket">&gt;</span>←表も同様に通し番号が付きます<span class="bracket">&lt;/</span><span class="element">caption</span><span class="bracket">&gt;</span>
<span class="bracket">&lt;/</span><span class="element">table</span><span class="bracket">&gt;</span>
</code></pre>

## <a id="sec-generated-title-3"></a> <a id="result"></a>結果

<figure>
	[![テスト用の画像](../../../../assets/media/ufcpp2000/xml/test.png)](../../../../assets/media/ufcpp2000/xml/test.png)
	<figcaption>テスト用の画像</figcaption>
</figure>


<figure>
	[![←通し番号も付きます](../../../../assets/media/ufcpp2000/xml/test.png)](../../../../assets/media/ufcpp2000/xml/test.png)
	<figcaption>←通し番号も付きます</figcaption>
</figure>


<table summary="表も書けます">
	<caption>
		表も書けます
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>i</th>
		<th>ii</th>
	</tr>
	<tr>
		<th>A</th>
		<td markdown="1">10</td>
		<td markdown="1">15</td>
	</tr>
	<tr>
		<th>B</th>
		<td markdown="1">25</td>
		<td markdown="1">50</td>
	</tr>
</table>


<table summary="←表も同様に通し番号が付きます">
	<caption>
		←表も同様に通し番号が付きます
	</caption>
<thead>	<tr>
		<td markdown="1"></td>
		<th>あ</th>
		<th>い</th>
	</tr>
</thead><tbody>	<tr>
		<th>イ</th>
		<td markdown="1">三</td>
		<td markdown="1">五</td>
	</tr>
	<tr>
		<th>ロ</th>
		<td markdown="1">八</td>
		<td markdown="1">四</td>
	</tr>
</tbody></table>
