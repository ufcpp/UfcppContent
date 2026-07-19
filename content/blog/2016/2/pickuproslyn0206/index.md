---
title: "ピックアップRoslyn 2/6"
source_url: "https://ufcpp.net/blog/2016/2/pickuproslyn0206/"
content_type: "BlogEntry"
published_at: "2016-02-06T06:02:47"
updated_at: "2016-02-06T06:04:09"
tags: []
umbraco_id: 1874
parent_id: 1873
sort_order: 0
aliases: []
---

# ピックアップRoslyn 2/6

## コンストラクター引数を元にオブジェクトの分解(deconstruction)

[Proposal: Positional deconstruction based on existing constructors and properties #8415](https://github.com/dotnet/roslyn/issues/8415)

[パターン マッチング](https://github.com/dotnet/roslyn/blob/future/docs/features/patterns.md)で、現在提案されている範囲では、`is` 演算子みたいな特殊なメソッドを1個追加してやらないと、`o is Person("Alan", var last)` みたいな感じのマッチングができません。

これだと、今後追加する型(特にレコード型)に対してなら使えるけども、既存の型には全く使えなくて困る。一方で、現状でも、以下のコードみたいに、コンストラクター引数とプロパティに1対1の関係があるようなクラスを書く人は多いわけで、この規約ベースでオブジェクトの分解をできないかという提案。

<pre class="source" title="">
<code><span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">class</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">Person</span>
{
  <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">Person</span>(<span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-smi" style="box-sizing: border-box; color: rgb(51, 51, 51);">firstName</span>, <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-smi" style="box-sizing: border-box; color: rgb(51, 51, 51);">lastName</span>) 
  { 
    FirstName = firstName; 
    LastName = lastName; 
  }
  <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">FirstName</span> { <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">get</span>; }
  <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">public</span> <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">string</span> <span class="pl-en" style="box-sizing: border-box; color: rgb(121, 93, 163);">LastName</span> { <span class="pl-k" style="box-sizing: border-box; color: rgb(167, 29, 93);">get</span>; }
}
</code></pre>

確か自分もこのパターンでクラスを書いていることが多いんで、にこの機能が入れば、それらを1個1個レコード型に置き換えたりしなくてもパターン マッチングが使えて大変便利。

でも、引数の`firstName`とプロパティの`FirstName`の対応関係を規約ベースでやるのは、C#の文化(規約を嫌う、識別子は大文字小文字を区別する)的には合わないんで悩ましい感じ。ついてるコメントも賛否両論です。

## Swift 2.0

[Swift 2.0でdeferとguardが入った](http://nshipster.com/guard-and-defer/)わけですが、先月、それはC#には適するかどうか、議論用のissueページが立ちました。

- ["defer" statement #8115](https://github.com/dotnet/roslyn/issues/8115)
- [Proposal: Guard statement in C# #8181](https://github.com/dotnet/roslyn/issues/8181)

ついてるコメントからすると、賛否は半々くらいか、ちょっと否定が多いくらいかなぁ。個人的な予想では、採用されない気がする。

### defer

`try { 処理 } finally { 後始末 }` の代わりに `defer { 後始末 } 処理` と書くような構文。

<pre class="source" title="">
<code>    {
        SomeType thing = Whatever...;
        defer {
            thing.Free();
        }
        <span class="pl-c" style="box-sizing: border-box; color: rgb(150, 152, 150);">// some code code using thing</span>
    }
</code></pre>

メリットは以下のような感じ。

- `using` 相当の機能を、`IDisposable` 実装していなくてもできる
- `try`-`finally` と比べて、「処理」の部分がネスト深くならない
- `try`句内と、`finally`句内の両方で使いたい変数をわざわざその外側で宣言する必要がない

ただ、以下のような問題も。

- `defer` が複数並んでるとき、その実行順はどうすべき？
- `try`-`finally` でできることに対してわざわざ新構文増やすの？
- 1回り外側のブロックに対して影響を与えるような構文ってなかなか理解されにくい

### guard

`if (絶対満たすべき条件) ; else throw 例外;` みたいなよくあるパターンに対して使う構文。絶対満たすべき条件だし、満たしてなかったら必ず例外を投げる(そこから先を絶対に実行しない)ことを保証するために、`if` じゃなくて `guard` を使おうというもの。

まあ、そういうものがほしいこともあるというのはわかるものの、[コントラクト](https://github.com/dotnet/roslyn/issues/119)と被ってるし、そもそも`guard`の必要性を減らせそう(コンパイル時にチェック可能で、実行時に例外投げる必要がない)な構文もこれから増えるだろうし。
