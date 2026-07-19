---
title: "using alias を任意の型に対応"
source_url: "https://ufcpp.net/blog/2023/1/using-alias-any-types/"
content_type: "BlogEntry"
published_at: "2023-01-18T22:07:04"
updated_at: "2023-01-18T22:07:04"
tags: []
umbraco_id: 2453
parent_id: 2449
sort_order: 3
aliases: []
---

# using alias を任意の型に対応

今日は using alias の話。

* 提案: [Allow using alias directive to reference any kind of Type](https://github.com/dotnet/csharplang/blob/main/proposals/using-alias-types.md)

これはちらほら実装が始まっているので近々触れるものが出てくるんじゃないでしょうか。

## 既存の using ディレクティブ

using alias は、using ディレクティブを書くときに `using T = System.DateOnly;` みたいに書いて、以後は `T` だけで型名を参照できるやつ。
現状何が問題かというと…

まず、以下のコードであれば現状でもコンパイルできるんですが…

<pre class="source" title="現状の C# でも書ける using alias">
<span class="reserved">using</span> <span class="type">List</span> <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;<span class="reserved">int</span>&gt;;
<span class="reserved">using</span> <span class="type">ListA</span> <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;<span class="reserved">int</span>[]&gt;;
<span class="reserved">using</span> <span class="type">ListN</span> <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;<span class="reserved">int</span><span class="operator">?</span>&gt;;
<span class="reserved">using</span> <span class="type">ListT</span> <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;(<span class="reserved">int</span>, <span class="reserved">int</span>)&gt;;
</pre>

そのくせ以下のコードはコンパイルできません。

<pre class="source" title="現状ではコンパイルできない using alias">
<span class="reserved">using</span> <span class="type">Primitive</span> <span class="operator">=</span>  <span class="reserved"><span class="error" title="CS1001">int</span></span>;
<span class="reserved">using</span> <span class="type">Array</span> <span class="operator">=</span> <span class="reserved"><span class="error" title="CS1001"><span class="error" title="CS1002">int</span></span></span>[<span class="error" title="CS0116">]</span>;
<span class="reserved">using</span> <span class="type">Nullable</span> <span class="operator">=</span> <span class="error" title="CS1002"><span class="reserved">int</span></span><span class="error" title="CS0116"><span class="operator">?</span></span>;
<span class="reserved">using</span> <span class="type">Tuple</span> <span class="operator">=</span> <span class="error" title="CS1002">(</span><span class="reserved"><span class="error" title="CS1525">int</span></span>, <span class="reserved"><span class="error" title="CS1525">int</span></span>);
</pre>

要するに、ジェネリック型引数なら制限がほとんどないのに、トップレベルの時にだけ、以下のものを書けないという制限がありました。

* `int` みたいにキーワードを使ったプリミティブ型 (⇔ `System.Int32` なら書ける)
* null 許容型 (`T?`) (⇔ `System.Nullable<T>` なら書ける)
* タプル (`(T1, T2)`) (⇔ `System.ValueTuple<T1, T2>` なら書ける)
* 配列 (`T[]`)

まあさすがにいい加減これを認めようという話になっています。

一番需要があるのはタプルですかね。
あと、最近では[関数ポインター](https://github.com/ufcpp/UfcppSample/issues/347)なんかも `delegate*<int, int, void>` みたいな感じで名前が長くなりがちなので、これに対しても使いたいみたいです。

## 微修正

`int` とか `int?` とかに対応するだけなら大した変更は要らないみたいです。
[構文的には1行書き変わるだけ](https://github.com/dotnet/csharplang/blob/main/proposals/using-alias-types.md)。

<pre>
using_alias_directive
-    : 'using' identifier '=' namespace_or_type_name ';'
+    : 'using' identifier '=' (namespace_name | type) ';'
    ;
</pre>

たぶん、「元々 using 専用に特殊処理していたけども、普通の型名参照と同じものに置き換える」みたいな感じでしょうか。

これは…
もっと早くから対応してくれててもよかった疑惑が…

## トップレベルの null 許容参照型

参照型に対しては、トップレベルでは `?` をつけれないようにするみたいです。
まあ、今でも、`typeof(string)` は書けても `typeof(string?)` とは書けないので、
それと同じです。

<pre class="source" title="トップレベルの NRT">
<span class="reserved">using</span> <span class="type">List</span> <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;<span class="reserved">string</span><span class="operator">?</span>&gt;; <span class="comment">// これは OK。</span>
<span class="reserved">using</span> <span class="type">S</span> <span class="operator">=</span> <span class="error"><span class="reserved">string</span><span class="operator">?</span></span>; <span class="comment">// これはダメ。</span>
</pre>

## ポインター

要望として関数ポインターのエイリアスを作りたいわけですが。
[unsafe](../../../../study/csharp/interop/sp_unsafe.md) なものを単に
`using T = int*;` とか書いていいのかどうかという議題がありました。

これに対しては結局、`using unsafe` という構文を導入するみたいです。

<pre class="source" title="using unsafe">
<span class="reserved">using</span> <span class="reserved">unsafe</span> <span class="type">T</span> <span class="operator">=</span> <span class="reserved">int</span><span class="operator">*</span>;
<span class="reserved">using</span> <span class="reserved">unsafe</span> <span class="type">F</span> <span class="operator">=</span> <span class="reserved">delegate</span><span class="operator">*</span>&lt;<span class="reserved">int</span>, <span class="reserved">int</span>, <span class="reserved">void</span>&gt;;
</pre>

## 今後の課題: 型引数

[エイリアスをジェネリックにして型引数を持たせたい](https://github.com/dotnet/csharplang/issues/1239)という話もあります。
以下のような、エイリアスの右辺にも `<T>` を付けたいというやつ。

<pre class="source" title="エイリアスに &lt;T&gt; を付けたい">
<span class="reserved">using</span> <span class="type">List</span>&lt;<span class="type">T</span>&gt; <span class="operator">=</span> System<span class="operator">.</span>Collections<span class="operator">.</span>Generic<span class="operator">.</span><span class="type">List</span>&lt;<span class="type">T</span>&gt;;
</pre>

これはこれで要望はあって、Backlog (すぐに手を付けるほどの優先度にはない)とはいえ、
Champion (C# チームの担当がついてる状態)にはなっています。

ただ、これの対応は「微修正」では済まないので、
C# 12 マイルストーンからは外れるみたいです。
