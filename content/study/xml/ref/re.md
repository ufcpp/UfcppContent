---
title: "実部・虚部"
source_url: "https://ufcpp.net/study/xml/ref/re/"
content_type: "Article"
published_at: "2015-05-06T14:25:28"
updated_at: "2015-05-06T14:25:28"
tags: []
umbraco_id: 1688
parent_id: 1661
sort_order: 26
aliases:
  - "/study/ref/Re.html"
---

# 実部・虚部

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

実部を表す記号Reおよび、虚部を表す記号Imを表示


## <a id="sec-generated-title-2"></a> <a id="usage"></a>利用方法

```xml
<Re/> <Im/>
```

## <a id="sec-generated-title-3"></a> <a id="sample"></a>サンプル

```xml
<exp/>(z) = <exp/>(<Re/>(z))<paren size="1.2"><cos/><Im/>(z) + i<sin/><Im/>(z)</paren>
```
<div class="math"><span class="normal">exp</span>(z) = <span class="normal">exp</span>(<span class="script">Re</span>(z))<span class="paren" style="font-size:1.2em;">(</span><span class="normal">cos</span><span class="script">Im</span>(z) + i<span class="normal">sin</span><span class="script">Im</span>(z)<span class="paren" style="font-size:1.2em;">)</span>
</div>

## <a id="sec-generated-title-4"></a> <a id="xsl"></a>XSL template

```xml
<xsl:template match="ufcpp:Re">
  <span class="script">Re</span>
</xsl:template>

<xsl:template match="ufcpp:Im">
  <span class="script">Im</span>
</xsl:template>
```

## <a id="sec-generated-title-5"></a> <a id="css"></a>style sheet

```css
span.cursive
{
  font-family:cursive;
  font-style:italic;
  padding-right:0.2em;
}
```
