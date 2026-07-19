---
title: "ピックアップRoslyn 10/9"
source_url: "https://ufcpp.net/blog/2017/10/pickuproslyn1009/"
content_type: "BlogEntry"
published_at: "2017-10-09T17:50:55"
updated_at: "2017-10-09T17:50:55"
tags: []
umbraco_id: 2087
parent_id: 2084
sort_order: 1
aliases: []
---

# ピックアップRoslyn 10/9

今日は、新たにChampion (取り組むこと確定) が2件と、面白そうな提案1件。

- [Champion: ref local reassignment #933](https://github.com/dotnet/csharplang/issues/933) … ref ローカル変数の再代入
- [Champion "Declaration Expressions" #973](https://github.com/dotnet/csharplang/issues/973) … 宣言式
- [ValueEnumerator (fast to code and run) #982](https://github.com/dotnet/csharplang/issues/982) … 値型 enumerator

## ref ローカル変数の再代入

- [Champion: ref local reassignment #933](https://github.com/dotnet/csharplang/issues/933)

[C# 7.0](../../../../study/csharp/cheatsheet/ap_ver7.md#ref-returns) で、参照ローカル変数が使えるようになっていましたが、
参照ローカル変数の再代入はできませんでした。

<pre class="source" title="参照ローカル変数の再代入">
<code><span class="reserved">static</span> <span class="reserved">ref</span> <span class="reserved">int</span> Max(<span class="reserved">int</span>[] array)
{
    <span class="reserved">if</span> (array.Length == 0) <span class="reserved">throw</span> <span class="reserved">new</span> <span class="type">ArgumentException</span>();

    <span class="reserved">ref</span> <span class="reserved">var</span> max = <span class="reserved">ref</span> array[0];

    <span class="reserved">for</span> (<span class="reserved">var</span> i = 0; i &lt; array.Length; ++i)
    {
        <span class="reserved">if</span> (max &lt; array[i])
        {
            <span class="comment">// max = x; だと、array[0] の内容を上書きしちゃうのでダメ</span>
            <span class="comment">// こう書きたい(C# 7.0 では無理)</span>
            <span class="reserved">ref</span> max = <span class="reserved">ref</span> array[i];
        }
    }

    <span class="reserved">return</span> <span class="reserved">ref</span> max;
}
</code></pre>

一方、C# 7.2でつかされる機能として前々から決まっていたものとして、[ref-like 型](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-7.2/span-safety.md)というものがあります。
これまでの .NET では認められていなかった「フィールドとして参照を持てる構造体」を認めるための仕様です。
その手の構造体を安全に使うには`ref` (参照引数、参照ローカル変数、参照戻り値)と同じようなフロー解析が必要で、
その仕様が C# 7.2 で追加されます。

で、C# 7.2のref-like型では、変数への「参照の再代入」を認めている(認められるようにフロー解析を賢く実装した)ので、
だったら、C# 7.0までの参照ローカル変数でも再代入を認められるはず。
ということで、これもC# 7.2で実装しようという流れになっています。

## 宣言式

- [Champion "Declaration Expressions" #973](https://github.com/dotnet/csharplang/issues/973)

こちらは C# 6.0 の頃から提案に上がっていたもの。
以下のように、式の途中で変数宣言ができるという機能。

<pre class="source" title="宣言式">
<code><span class="reserved">var</span> square = (var x = <span class="reserved">int</span>.Parse(<span class="type">Console</span>.ReadLine()) * x;
</code></pre>

「パターンマッチと併せて練り直したい」、「パターンマッチ同様、変数`x`のスコープをどうするかちょっと迷う」、「大変な割には需要は低め(やらないとは言わないけど優先度低)」みたいな状態だったものに、ついに「Chanpion」タグが付きました。

まあ、ただし、マイルストーンが決まっていないので相変わらず優先度低めです。

## 値型 enumerator

- [ValueEnumerator (fast to code and run) #982](https://github.com/dotnet/csharplang/issues/982)

今の仕様だと、[イテレーター](../../../../study/csharp/data/sp2_iterator.md)を以下のように書きます。

<pre class="source" title="現在のイテレーター構文">
<code><span class="reserved">static</span> <span class="type">IEnumerable</span>&lt;<span class="reserved">int</span>&gt; X()
{
    <span class="reserved">yield</span> <span class="reserved">return</span> 1;
    <span class="reserved">yield</span> <span class="reserved">return</span> 2;
    <span class="reserved">yield</span> <span class="reserved">return</span> 3;
}
</code></pre>

これで何が問題かというと、必ずインターフェイスを介して列挙子を返すことになるので、
[ヒープ](../../../../study/csharp/resource/misc_heap.md)確保が避けれないという点です。

で、この問題を避けるために、結局、イテレーター構文は使わず、構造体な列挙子を1つ1つ作ったりするというつらい最適化作業が待っていたりします。
まあ昔からですが、[`List<T>`の`GetEnumerator`](http://source.dot.net/#System.Private.CoreLib/src/System/Collections/Generic/List.cs,675)なんかがそういう実装になっています。

書きやすさとパフォーマンスのトレードオフは常にあるものなのでしょうがないと言えばしょうがないんですが、
やっぱり最初から「構造体を生成してくれるイテレーターが欲しい」という提案が出てきたという状態。
