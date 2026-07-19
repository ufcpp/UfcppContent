---
title: "ピックアップ Roslyn 8/15"
source_url: "https://ufcpp.net/blog/2015/08/pickuproslyn0815/"
content_type: "BlogEntry"
published_at: "2015-08-14T19:02:49"
updated_at: "2015-08-29T02:49:53"
tags:
  - "ピックアップRoslyn"
umbraco_id: 1784
parent_id: 1775
sort_order: 3
aliases: []
---

# ピックアップ Roslyn 8/15

「バグでしょ？」からの「仕様です」が2件ほど。

そしてだいたいこの手の話題は、5・6年前に Stack Overflow で話題が出てて、
[Eric Lippert](http://stackoverflow.com/users/88656/eric-lippert)が回答済みという落ち。

## && の後ろ

[Change to definitive assignment in LINQ queries #4509](https://github.com/dotnet/roslyn/issues/4509)

`dynamic` が絡んだ時に、以下のコードで、「b が未初期化の可能性があります」エラーになるという話。

`decimal.TryParse(v, out a) && decimal.TryParse("15", out b) && a <= b`

1. `TryParse(v, out a)` で `a` が初期化される
1. `TryParse(v, out b)` で `b` が初期化される
1.  `a <= b` でその `a` と `b` を使う

`dynamic` が絡まなければ、`TryParse` の戻り値は `bool` で確定していてます。
そして、`bool` の `&&` であれば、1., 2. が評価された後でしか 3. が評価されない保証があるのでエラーは起こしません。

が、ユーザー定義型で、ユーザー定義の `true`, `false` `&` 演算子を用意していると、2. をすっ飛ばして3. が評価される場合があり得るというのがエラーの原因。`dynamic` が絡むと、戻り値も `dynamic` なわけで、`bool` に確定しない。そうなると、2. がすっ飛ばされる可能性があって、3. の時点で 'b' が初期化されいている保証が取れないという状態。

## 配列の共変性

[Invalid optimization performed by C# compiler for IEnumerable<T> on arrays #3830](https://github.com/dotnet/roslyn/issues/3830)

以下のコードで `b` は `false`

<pre class="source" title="b は false">
<code><reserved></span><span class="reserved">enum</span> <span class="type">Enum0</span> { First }
<span class="reserved">enum</span> <span class="type">Enum1</span> { First }
<span class="reserved">var</span> source = <span class="reserved">new</span> <span class="type">Enum0</span>[3];
<span class="inactive">...</span>
<span class="reserved">bool</span> b = source <span class="reserved">is</span> <span class="type">IEnumerable</span>&lt;<span class="type">Enum1</span>&gt;;
</code></pre>

これが、いったん `object` で受けるように変更するだけで `b` が `true` になる。

<pre class="source" title="b が true に">
<code><span class="reserved">object</span> source = <span class="reserved">new</span> <span class="type">Enum0</span>[3];
<span class="reserved">bool</span> b = source <span class="reserved">is</span> <span class="type">IEnumerable</span>&lt;<span class="type">Enum1</span>&gt;;
</code></pre>

なんか変じゃない？という話。

1個目は、C# のルールで型判定していて `false`。
2個目は、`object` を挟んだことで C# 上は判定せず、実行時に CLR が型判定するようになって、
CLR のルールでは `true` になる。
ということみたい。

配列は、ジェネリックがない頃に無理やり特殊対応な共変性を実装してるので、なんか微妙に変な挙動をすることがあったり。
