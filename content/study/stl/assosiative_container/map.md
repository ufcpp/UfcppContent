---
title: "map, multimap"
source_url: "https://ufcpp.net/study/stl/assosiative_container/map/"
content_type: "Article"
published_at: "2015-05-06T14:23:42"
updated_at: "2015-05-06T14:23:42"
tags: []
umbraco_id: 1640
parent_id: 1638
sort_order: 1
aliases:
  - "/stl/assosiative_container/map/"
  - "/stl/map"
  - "/stl/map.html"
  - "/study/stl/map"
  - "/study/stl/map.html"
---

# map, multimap

##<a id="sec-generated-title-1"></a> <a id="d27e4"></a>map, multimapとは
Perlを勉強したことある人ならmapは連想配列に似ていると言えば分かると思います。
 
連想配列とはキーと値の組み合わせを記憶しておくものです。
 
普通の配列では、

<pre class="source" title="" lang="">
<code>phoneNumber[0] = <span class="literal">"06-633-****"</span>;
phoneNumber[1] = <span class="literal">"079-341-****"</span>;
phoneNumber[2] = <span class="literal">"078-852-****"</span>;
</code></pre>


というように添字には整数値しか使えません。
 
一方、連想配列では(C言語風に書くと)

<pre class="source" title="" lang="">
<code><span class="reserved">char</span> name[] = <span class="literal">"山田"</span>;
phoneNumber[name] = <span class="literal">"06-633-****"</span>;
phoneNumber[<span class="literal">"田中"</span>] = <span class="literal">"079-341-****"</span>;
phoneNumber[<span class="literal">"鈴木"</span>] = <span class="literal">"078-852-****"</span>;
</code></pre>


と言うように数値以外の添字を使うことができます。
このような添字のことをキーと言います。
 
map, multimapはこの連想配列のようにキーと値をペアにして記憶します。
名前で検索して値を調べるという操作は辞書を引くことに似ています。
そのため、mapはDictionary(辞書)とも呼ばれています。
mapはキーの重複が許されず、multimapはキーの重複が許されます。
 
STLでは、キーと値を組にするために<code>pair</code>というクラスが用意されています。

<pre class="source" title="" lang="">
<code>pair&lt;Key, Value&gt; <span class="comment">//Keyという型とValueという型を組にした型</span>
</code></pre>


そしてmapやmultimapはこのpair型を要素とする2分探索木で実装されています。
 
mapには<code>operator[]</code>が用意されていて、
連想配列と同じように操作することができます。例をあげると、

<pre class="source" title="" lang="">
<code>map&lt;string, string&gt; phoneNumber;
phoneNumber[<span class="literal">"山田"</span>] = <span class="literal">"06-633-****"</span>;
</code></pre>


と言うような操作ができます。
 
STLで2分探索木はxtreeというヘッダーファイル中に<code>tree</code>という名前のクラスとして用意されています。
この<code>tree</code>というクラスはset,multiset,map,multimapを実装するために用意されたクラスです。
したがってユーザーが直接このクラスを使う必要はありません。


##<a id="sec-generated-title-2"></a> <a id="d27e113"></a>mapの特徴
* 要素の追加、キーによる検索、削除が O(log n) で行える

* キーの重複を許さない

* キーの値の小さな要素から順にアクセスできる

* <code>operator[]</code>を用いてPerlの連想配列のように値にアクセスできる



##<a id="sec-generated-title-3"></a> <a id="d27e134"></a>multimapの特徴
* 要素の追加、キーによる検索、削除が O(log n) で行える

* キーの重複を許す

* キーの値の小さな要素から順にアクセスできる
