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

```xml
<figure>
  <image src="test.png" width="200" height="200" />
  <legend>テスト用の画像</legend>
</figure>
<figure>
  <image src="test.png" width="100" height="100" />
  <legend>←通し番号も付きます</legend>
</figure>
<table>
  <tr>
    <td></td>
    <th>i</th>
    <th>ii</th>
  </tr>
  <tr>
    <th>A</th>
    <td>10</td>
    <td>15</td>
  </tr>
  <tr>
    <th>B</th>
    <td>25</td>
    <td>50</td>
  </tr>
  <caption>表も書けます</caption>
</table>
<table>
  <thead>
    <tr>
      <td></td>
      <th>あ</th>
      <th>い</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th>イ</th>
      <td>三</td>
      <td>五</td>
    </tr>
    <tr>
      <th>ロ</th>
      <td>八</td>
      <td>四</td>
    </tr>
  </tbody>
  <caption>←表も同様に通し番号が付きます</caption>
</table>
```

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
