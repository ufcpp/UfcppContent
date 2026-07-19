---
title: "指数関数の底"
source_url: "https://ufcpp.net/study/xml/ref/e/"
content_type: "Article"
published_at: "2015-05-06T14:24:53"
updated_at: "2015-05-06T14:24:53"
tags: []
umbraco_id: 1674
parent_id: 1661
sort_order: 12
aliases:
  - "/ref/e"
  - "/ref/e.html"
  - "/study/ref/e"
  - "/study/ref/e.html"
  - "/xml/ref/e/"
---

# 指数関数の底

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

指数関数の底eを表示する(eはブロック体で表記すべき)


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<e/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<e/><sup>iθ</sup> = <cos/>θ ＋ i<sin/>θ
```
<div class="math"><span class="normal">e</span><sup>iθ</sup> = <span class="normal">cos</span>θ ＋ i<span class="normal">sin</span>θ
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:e">
  <span class="normal">e</span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.normal
{
  font-weight:normal;
  font-style:normal;
}
```
