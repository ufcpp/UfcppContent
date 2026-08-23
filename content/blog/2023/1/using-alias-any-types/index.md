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

```csharp {title="現状の C# でも書ける using alias"}
using List = System.Collections.Generic.List<int>;
using ListA = System.Collections.Generic.List<int[]>;
using ListN = System.Collections.Generic.List<int?>;
using ListT = System.Collections.Generic.List<(int, int)>;
```

そのくせ以下のコードはコンパイルできません。

```csharp {title="現状ではコンパイルできない using alias" error-ranges="sha256:2d05a6e619f5f95d9b37961dd3ceae9b0014ed717b9819274dedab017bf681e1;1:20-1:23,2:15-2:18,2:19-2:20,3:18-3:22,4:15-4:19,4:21-4:24"}
using Primitive =  int;
using Array = int[];
using Nullable = int?;
using Tuple = (int, int);
```

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

```text
using_alias_directive
-    : 'using' identifier '=' namespace_or_type_name ';'
+    : 'using' identifier '=' (namespace_name | type) ';'
    ;
```

たぶん、「元々 using 専用に特殊処理していたけども、普通の型名参照と同じものに置き換える」みたいな感じでしょうか。

これは…
もっと早くから対応してくれててもよかった疑惑が…

## トップレベルの null 許容参照型

参照型に対しては、トップレベルでは `?` をつけれないようにするみたいです。
まあ、今でも、`typeof(string)` は書けても `typeof(string?)` とは書けないので、
それと同じです。

```csharp {title="トップレベルの NRT" error-ranges="sha256:59ad0d397a5a4d879d0b212bd5b835b856e2198f0de02f43467404bbfda8a777;2:11-2:18"}
using List = System.Collections.Generic.List<string?>; // これは OK。
using S = string?; // これはダメ。
```

## ポインター

要望として関数ポインターのエイリアスを作りたいわけですが。
[unsafe](../../../../study/csharp/interop/sp_unsafe.md) なものを単に
`using T = int*;` とか書いていいのかどうかという議題がありました。

これに対しては結局、`using unsafe` という構文を導入するみたいです。

```csharp {title="using unsafe"}
using unsafe T = int*;
using unsafe F = delegate*<int, int, void>;
```

## 今後の課題: 型引数

[エイリアスをジェネリックにして型引数を持たせたい](https://github.com/dotnet/csharplang/issues/1239)という話もあります。
以下のような、エイリアスの右辺にも `<T>` を付けたいというやつ。

```csharp {title="エイリアスに &lt;T&gt; を付けたい"}
using List<T> = System.Collections.Generic.List<T>;
```

これはこれで要望はあって、Backlog (すぐに手を付けるほどの優先度にはない)とはいえ、
Champion (C# チームの担当がついてる状態)にはなっています。

ただ、これの対応は「微修正」では済まないので、
C# 12 マイルストーンからは外れるみたいです。
