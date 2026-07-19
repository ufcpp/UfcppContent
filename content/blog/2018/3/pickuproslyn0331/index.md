---
title: "ピックアップRoslyn 3/31: C# 7.3"
source_url: "https://ufcpp.net/blog/2018/3/pickuproslyn0331/"
content_type: "BlogEntry"
published_at: "2018-03-31T23:49:38"
updated_at: "2018-03-31T23:49:38"
tags: []
umbraco_id: 2143
parent_id: 2134
sort_order: 6
aliases: []
---

# ピックアップRoslyn 3/31: C# 7.3

[C# Language Design Notes アップロード祭り](../pickuproslyn0321/index.md)の続き。

Notes のうち3・4割くらいは、C# 7.3の機能の「最後の詰め」みたいな感じの検討でした。

## C# 7.3 の状況

ちなみに、先日のブログでも書きましたが、C# 7.3に含める機能はもう概ね固まっているみたいです。[進捗状況](https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md)を見ると全項目Merged。

(大半は、[nightly build](https://dotnet.myget.org/gallery/roslyn)のパッケージに反映済み。まだなのは「Custom fixed」と「Indexing movable fixed buffers」の2つ。)

うちの[C# によるプログラミング入門](../../../../study/csharp/index.md)には、Visual Studio 15.7 Previewで拡張なしで C# 7.3を使えるようになる頃にはページを追加したいなぁとか思いながら、nightly build版でちらほら試しに触ってみています。
とりあえずは、今のnightly buildに含まれていない上記2機能を除いて、書きたい内容のサンプル コードを整備:

- [https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/Csharp7_3-0309](https://github.com/ufcpp/UfcppSample/tree/master/Demo/2018/Csharp7_3-0309)

## Design Notes で検討されていたこと

とりあえず具体的な機能説明は[C# によるプログラミング入門](../../../../study/csharp/index.md)の方にページ追加するときに頑張ります。

今日は、先日上がった Design Notes で触れられていた内容を軽く紹介。

### 式中の変数宣言

C# 7.0でいわゆる「[`out var`](../../../../study/csharp/cheatsheet/ap_ver7.md#var-expressions)」が入って、変数宣言を式中でできるようになりました。
C# 7.3では、7.0の頃には使えなかったいくつかの場所でこの機能を使えるよう拡充することになっています。

Notes での検討事項は、主にその変数のスコープをどうするか:

- コンストラクター初期化子: `C(int i) : base(out var x) { x }` ← この `x` は `{}` 内で有効
- フィールド初期化子: その初期化子内でだけ有効
    - `int X = M(out var x).N(x);` ← `N`のとこで`x`は有効
    - 続けて `int Y = x;` ← `X` の方で作った `x` は、`Y` の方では無効
    - C# スクリプト実行(変数っぽく宣言したものは、暗黙的に見えないクラスのフィールドになってる)の時の挙動とは一貫性ないんだけど
- クエリ式: 句単位でスコープ切れてる
    - `from x in e where M(x, out var y) select y;` ← `select`の方の`y`は無効
    - ラムダ式の挙動に準じてる。 `e.Where(x => M(x, out var y)).Select(x => y);` ← `Select`内の`y`は無効
    - 句をまたぎたければ `let` 句を使って

### ジェネリクスの型制約

#### Enum, Delegate

`where T : Enum` と `where T : Delegate` (または `where T : MulticastDelegate`)が指定できるように。

この`Enum`、`Delegate`は、それぞれ`System.Enum`クラス、`System.Delegate` クラスのことです。`using System;`がないと`where T : Enum`とは書けず。

また、キーワードの`enum`、`delegate`を使った`where T : enum` と `where T : delegate` は書けません。
将来的にそういうのも検討するかもしれないけども、少なくとも C# 7.3 では実装しません。

#### ValueType, Array

同じような「.NETのランタイムが特別扱いしてる型」に`Systam.ValueType`と`System.Array`がありますが、
これらは現状では有意義な用途が見つかっておらず、「もし用途が見つかれば改めて考える」とのこと。

#### unmanaged

もう1個、こっちは文脈キーワードの追加で、`where T : unmanaged` という制約が追加されます。
これは要するに、「ポインター化可能な型」。
「[unsafe コード限定機能](../../../../study/csharp/interop/sp_unsafe.md#function)」のとこで説明していますが、
ポインター化しても安全な型にはちょっとした制約があって、
その制約を満たす型を元々 unmanaged 型と呼びます。

これまで、ジェネリック型引数に対して unmanaged かどうかの判定はできなかったんですが、C# 7.3 でこの制約が入ったことで、ジェネリックなポインターを作れるようになります。

unmanaged の条件の1つが「構造体であること」なので、`where T : unmanaged` はこの時点で暗黙的に `where T : struct` の意味も含みます。
なので、`where T : struct, unmanaged` みたいな併用は認めません。

あと、「ポインター化可能かどうか」の判定には[参照アセンブリ問題](../../../2016/12/tipsapisurface/index.md)っていう問題があったりするんですが、
unmanaged 制約もまったく同じ問題を起こします。
(といっても、これは C# コンパイラーの問題ではないので C# チームとしてはどうしようもない。)

ちなみに、`var` の時にもそうだったんですが、「後からの文脈キーワードの追加」なので、互換性維持のために「`var`と言う名前の型、`unmanaged`という名前の型がすでに存在するときは、キーワードではなくてその型の方が優先される」という挙動になっています。
意図的に`class unmanaged { }` みたいなクラスを作ると、`where T : unmanaged` の `T` は「この `unmanaged` クラスの派生型」という扱いになります。

### ref reassignment

`ref var r` に対して、「参照先の振り替え」ができるようになります。
`ref var r = ref x;` とした後、`r = ref y;` で振り替え。その後は `r = value;` とすると、`y` の中身が書き換わります。

普通の代入と同じく、この `r = ref y` も式にできます。
例えば、連結リスト操作で `while ((l = ref l.Next) != null)` みたいなコードを書けます。

あと、C# 7.3 では `foreach (ref var x in source)` みたいな、`foreach` の反復変数も参照にできます。
これも、通常の反復変数が書き換え不可なので、参照の場合の「参照先の振り替え」は不可です。

`ref int r;` みたいに未初期化の状態で変数宣言した後、`r = ref x;` と後から参照先を割り当てることも考えられますが、C# 7.3 の時点ではやらないみたい。
「[安全性チェック](../../../../study/csharp/resource/sp_ref.md#flow-analysis)」(戻り値として返せるかどうか)が大変になるからとのこと。

### stackalloc

C# 7.2 で「[安全な stackalloc](../../../../study/csharp/resource/span.md#safe-stackalloc)」が入ります。
そのため、これまでなら「unsafe 限定のレア機能」だった`stackalloc`が、C# 7.2 以降利用頻度が上がる可能性があります。

ということで、`stackalloc`の使い勝手を上げてもいいだろうという話に。
具体的には、配列の`new`に対してできることは`stackalloc`にもできてもよいという話。
要するに、`stackalloc`に初期化子を付けれるようになります。
`stackalloc[] { 1, 2, 3 }`とか。

### パターン ベースの fixed ステートメント

[`Span<T>`](../../../../study/csharp/resource/span.md)はパフォーマンス向上のために導入された型で、
こいつに対してポインター操作をしたいという需要は結構高いようです。
しかし、`Span<T>`からポインター取得する際に`fixed` (GC によってポインターが指してる先が移動しないように、オブジェクトを「固定」する)をどうやって掛けようかと言うので悩んでいました。

今は、`fixed (T* p = MemoryMarshal.GetReference(span))`とか書いたり、
ちょっと前までは`fixed (T* p = span.DangerousGetPinnableReference())`とかだったりしました。
これを、`fixed (T* p = span)` と書いたら何らかのメソッド呼び出しに置き換えたいという話。

展開結果は、まあ、「Dangerous はねぇわ」という感じみたいで、
結局、今現在 marge されているコードを見るに`GetPinnableReference`と言う名前を使いそう。

`string`とかの標準ライブラリ中の型に関してはこの`GetPinnableReference`をインスタンス メソッドとして追加予定。
一方で、([`ref this`、`in this`](../../../../study/csharp/functional/sp3_extension.md#ref-extensions)を含む)拡張メソッドもOKにする予定。

safe な手段ではできないんですが「null 相当する参照」を返すことで、`fixed (T* p = s)` の結果の`p`がnullポインターになることも認めるとのこと。
nullチェックは使う側の責任。

### タプルの ==, !=

C# 7.3 から `(x, y) == (1, 2)` みたいな、`==`と`!=`を使ったタプルの等値判定ができるようになります。

C# のタプルの実体は`System.ValueTuple`型なわけですが、これの`==`演算子を呼べばいいという話ではないです。
タプルは「メンバーごとの暗黙の型変換」を認めているので、比較でも型変換を考慮する必要があるとのこと。
例えば、`(int x, int y)`なタプル変数`t`に対して、`t == (123L, 1E10D)` とかができます(それぞれ`long`、`double`との比較)。

ということで、タプルの`==`は、「メンバーごとの`==`呼び出しを`&&`で繋いだもの」として解釈。
ユーザー定義の`==`で、`bool`ではなくユーザー定義型を返す場合、それの`operator.true`と`operator.false`が呼ばれます。

### オーバーロード解決

C# 6.0くらいの頃からずっと「[bestest betterness](../../../2015/10/pickuproslyn1018/index.md)」とかいうふざけた言い方をしてたやつをついに実装するそうで。
ちなみに、betternessルールの改良はたびたびやっているので、
「better betterness」→「best betterness」→「bestest betterness」とかポケモン進化みたいなふざけた名前になりました。

今まで、「まず引数の型の一致を確認」→「制約等を満たすか調べて、満たしてなければコンパイル エラー」という順の処理をしていました。
制約を調べる方があとなので、解決できないオーバーロードが結構あったという。
それを、引数の一致を調べるより先に、以下のものを調べるように変わります。

- ジェネリック型制約
- 静的メンバーかインスタンス メンバーか
- (メソッドをデリゲートに渡すときに)メソッドの戻り値の型

これは bestest だわー。
(さすがにふざけた名前なので、最近、Proposal のタイトルも「Improve overload candidates」に変更されました。)
