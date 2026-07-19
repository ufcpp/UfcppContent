---
title: "ピックアップRoslyn & Visual Studio 16.1 Preview 1"
source_url: "https://ufcpp.net/blog/2019/4/pickuproslyn0420/"
content_type: "BlogEntry"
published_at: "2019-04-20T20:21:37"
updated_at: "2019-05-19T21:06:56"
tags: []
umbraco_id: 2239
parent_id: 2236
sort_order: 2
aliases: []
---

# ピックアップRoslyn & Visual Studio 16.1 Preview 1

## Visual Studio 16.1 Preview 1

[Visual Studio 16.1 Preview 1](https://docs.microsoft.com/ja-jp/visualstudio/releases/2019/release-notes-preview#16.1.0-pre.1.0) が来てたことに今更気付くなど…

先日の[Visual Studio 2019 GA の話](../vs2019ga/index.md)で書いた通り、これまで「Visual Studio 2019 Preview」をインストールしていた人のところには 16.1 Preview 1 が配信されているはずです。
GA 版にかまけてしばらく Preview チャネルの方を見てなかった…
(4/10 の配信)

[Language Feature Status](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)によれば、16.0 から 16.1 Preview 1 での差分は以下の1点のみです。

- [unmanaged generic structs](https://github.com/dotnet/csharplang/issues/1744)


ジェネリックな構造体でも条件さえ満たせば[ポインターとか stackalloc とか](../../../../study/csharp/interop/sp_unsafe.md)使えるようになりました。
C# によるプログラミング入門にも反映させてあります。

- [アンマネージなジェネリック構造体](../../../../study/csharp/cheatsheet/ap_ver8.md#unmanaged-generic-struct)

[using の話](../../../2018/12/cs8notyet/index.md)とか[非同期ストリームの話](../../../2018/12/cs8asyncstreams/index.md)とか、先に埋めた方がいいだろと思うものもちらほらあったりはするんですが、なかなか手付かずに…

## Design Notes

また一気に大量アップロード…

- [3月6日～4月1日分まとめて](https://github.com/dotnet/csharplang/compare/c0edd6fcb7...34e86a1702)
- [Added: LDM notes for April 3rd, 2019 #2414](https://github.com/dotnet/csharplang/issues/2414)
- [Added: LDM Notes from Apr 15, 2019 #2447](https://github.com/dotnet/csharplang/issues/2447)

ちょっと一気に来過ぎて自分も大筋しか見れてないんですが…
興味の引かれたところだけ抜粋:

### switch

- [switch 式](../../../../study/csharp/cheatsheet/ap_ver8.md#switch-expression)の優先度変えたいって
  - 今: 比較演算子の辺り。`<` とかの近く
  - 変更後: 単項演算子の直後。インデクサー、キャスト、`await` とかよりは下で、掛け算の `*` とかよりは上。
- やっぱ `{}` が暗黙的に「null ではない」の意味なの混乱しそう
  - でも、`is` での挙動と合わせないと変だし
- 将来の [or パターン](https://github.com/dotnet/csharplang/issues/1350)のために、今、`case X or` みたいなのは警告出すべき？
  - 今もう動くコードなので、破壊的変更になっちゃうから無理そう

### 非同期ストリーム

- 非同期イテレーターへの `CancellationToken` の渡し方どうしよう？
  - 引数に所定の属性を付けたら、生成されるイテレーターに伝搬するようにしたい

### ??= 演算の戻り値の型

以下のコードを書いたとき、 `c` の型はどうあるべきか

<pre class="source" title="">
<code><span class="reserved">int</span>? <span class="variable">b</span> = <span class="reserved">null</span>;
<span class="reserved">var</span> <span class="variable">c</span> = <span class="variable">b</span> ??= 5;
</code></pre>

- 今(16.1 Preview 1)の実装は `int?` になる
  - `b ??= 5` と `b = b ?? 5` が同じ意味になるように
- 今後、`int` になるように変える

### Index/Range

- [先日書いた](../pickuproslyn0402/index.md)「パターン ベースにする」という話、確定
- `Length` があればまずそれを、なければ `Count` を調べる
  - `Length` の戻り値が `int` でなければそれは無視して `Count` を調べる
  - `Count` の実装が O(1) じゃない場合の心配とかはしない(それは .NET のデザイン ガイドライン違反)
- `[Range r]` なインデクサーよりも、`Slice(int start, int length)` の方が優先される
- インスタンス メンバーしか追わない(拡張メソッドは調べない)
- `int` 以外のインデクサーは認めない(暗黙の型変換とかは追わない)し、引数も1個だけのしか調べない
- `Slice` も`int`が2引数のものしか調べない

### null 解析(null 許容参照型)

- 到達不能なコード(`return` の後ろとか)の null 解析はしない
  - 今ある「[確実な初期化](../../../../study/csharp/resource/rm_struct.md#definite-assignment)の解析」がそうだから
- 匿名型のメンバーの nullability は追うべき？ → yes
- 以下のコード、実装側で区別できなくて困らない？

<pre class="source" title="">
<code><span class="reserved">interface</span> <span class="type">I</span>
{
    <span class="reserved">void</span> <span class="method">Foo</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span> <span class="variable">value</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">class</span>;
    <span class="reserved">void</span> <span class="method">Foo</span>&lt;<span class="type">T</span>&gt;(<span class="type">T</span>? <span class="variable">value</span>) <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">struct</span>;
}
</code></pre>

→ 困る。実装側に `where` 制約を付けて区別できるようにしないといけない。

- [dynamic](../../../../study/csharp/dynamic/sp4_dynamic.md) な変数の nullability は追う？ → yes

### インターフェイスのデフォルト実装

- reabstraction は認めるか
  - `abstract` を付けて、デフォルト実装があるメソッドを再び「派生クラスでの実装が必須」の状態に戻す話
  - もし実装が楽そうならやるべき理由はある。実現性を要調査 → 十分できそう
- デフォルト実装ないから object のメンバー、特に `MemberwiseClone` のアクセスを認めるか
  - たぶん yes。でも問題起こしそう。object の protected メンバーはアクセスできないようにするかも → できなくするほうがだいぶ好ましそう
- クラス同様、[partial メソッド](../../../../study/csharp/oop/oo_class.md#partial)は暗黙的に private？ → yes

## Better Obsoletion (Obsolete 属性をよりよくしたい)

C# 側ではなくて [dotnet/designs](https://github.com/dotnet/designs) の方に出ている話ですがもう1個。

- [Better Obsoletion #62](https://github.com/dotnet/designs/pull/62)

.NET の基本ライブラリには、廃止予定にしてしまいたい(`Obsolete` 属性を付けてしまいたい) API がもう結構大量にあるわけですが。
今の `Obsolete` 属性の仕様だと「全部抑止」か「全部警告」の all or nothing で困るという話。

「ある特定の `Obsolete` 属性は無視して、それ以外の `Obsolete` 属性はちゃんと警告になる」みたいなグルーピング機能が欲しく、そのために `Obsolete` 属性に `DiagnosticId` プロパティを追加しようという提案になっています。

新しい属性を作ることも考えたけど、そしたら皮肉なことに `Obsolete` 属性自体に `Obsolete` 属性がついてしまうという問題。
