---
title: "ピックアップRoslyn 8/16 Close祭り"
source_url: "https://ufcpp.net/blog/2016/8/pickuproslyn0816/"
content_type: "BlogEntry"
published_at: "2016-08-16T03:33:52"
updated_at: "2016-08-16T04:57:55"
tags: []
umbraco_id: 1938
parent_id: 1932
sort_order: 3
aliases: []
---

# ピックアップRoslyn 8/16 Close祭り

実作業が一段落つくと始まるissue整理。

すごい数一気にコメントついたりCloseされたり。以下、そのまとめ。

## やらない

1行目: どういうものか

2行目: やらない理由

### [Proposal: Fixed-size buffers enhancements #126](https://github.com/dotnet/roslyn/issues/126)

構造体の内部に固定長配列を持てるようにしたい。

CLRのレベルで対応してもらいたい。C#コンパイラーでのコード生成はメリットの割に複雑すぎる。


### [infoof / propertyof / methodof (and reflection objects in attributes?) #128](https://github.com/dotnet/roslyn/issues/128)

`infoof`演算子(info-of、`typeof`のノリでMemberInfoを取得する)が欲しい。

リフレクションを言語に組み込みたくはない。`nameof`でもかなりのシナリオをカバーできる。


### [[Proposal] try-catch expressions #132](https://github.com/dotnet/roslyn/issues/132)

try-catch を式にしたい。

このページで提案されている構文だと問題がある。再提案があれば、取り組む価値があるかはまだ検討の必要があるかもしれないが、現状そこまでには思っていない。

### [Language support for the decorator design pattern #124](https://github.com/dotnet/roslyn/issues/124)

デコレーター([Pythonのデコレーター](http://qiita.com/mtb_beta/items/d257519b018b8cd0cc2e)とか[Kotlinのclass delgation](https://kotlinlang.org/docs/reference/delegation.html)と同種の機能)が欲しい。

いくらかのメリットがあるのはわかるし、過去に似た機能の提案も見てきた。でも、コンパイラーが暗黙的にコード生成するような機能には心配事が多々ある。
今なら、[ソース ジェネレーター](https://github.com/dotnet/roslyn/issues/5561) (たぶんC# 8)で解決できるかもしれない。


### [Strongly-typed type aliases #58](https://github.com/dotnet/roslyn/issues/58)

同じ型なんだけど、互いに暗黙的型変換はできないエイリアスを作りたい。例えば、`EmailAddress`型(中身は`string`)と`string`を区別したい。

[ソース ジェネレーター](https://github.com/dotnet/roslyn/issues/5561) (たぶんC# 8)を使って構造体をコード生成して、同様のことを実現してほしい。ソース ジェネレーターで苦痛は減るはず。


### [Auto properties with setter blocks #123](https://github.com/dotnet/roslyn/issues/123)

プロパティで、`get`のみ自動実装で、`set`は手動実装できるようにしてほしい。

良いディスカッションはあった。ディスカッションの中で出てきた副次的な案は別のissueページに移った。
でも、このページで提案されている元々の提案はやらない。バッキング フィールドを参照するような機能は得られるものに対して労力が過剰。


### [Implement Units of Measure or a broader class of similar contracts #144](https://github.com/dotnet/roslyn/issues/144)

F# の[Units of Measure](http://qiita.com/adacola/items/b65752b678e81bc8e354)がC#にもほしい。

非常にクールな機能だと思うものの、同種のものをC#に追加するのはメリットの割にコストが大きい。

訳注: たぶん、上記のStrolngly-typed typle aliasesで十分な利用シナリオも多いと思う。


### [[Proposal] Better handling of multiple comparisons with the same variable (`a > min && a < max`) #136](https://github.com/dotnet/roslyn/issues/136)

`min < x < max` みたいな、数学でよく見る多項比較できるようにしてほしい。

C# ではすでに `a < b < c` が `(a < b) < c` の意味で有効な式になっているし、難しい。 
`e in (0 ... n)` みたいな文法も、`e < n` なのか `e <= n` なのか混乱の元。


### [Allow type parameters to be declared co/contra variant on classes. #171](https://github.com/dotnet/roslyn/issues/171)

クラスの型引数にも共変性・反変性を認めてほしい。

CLRレベルでの対応が必須。


### [Proposal: Virtual arguments in methods #176](https://github.com/dotnet/roslyn/issues/176)

引数の実行時の型でメソッドのオーバーロードを呼び分けれるようにしてほしい(いわゆる多重ディスパッチ)。

ちゃんとやるのであればCLRレベルでの対応が必須。
型スイッチ(C# 7で入る)があれば書くのが多少楽になるけども、多重ディスパッチは型スイッチと比べてかなり複雑。


### [Feature Request: Lighter syntax for doc comments #85](https://github.com/dotnet/roslyn/issues/85)

docコメントがXMLなの重たい。JavaDoc/JSDocみたいな文法ほしい。

現状に不服があるのはわかる。でも、新旧文法の混在・競合みたいなのは嫌だし、直近での検討はしない。


## 重複削除・もうやってる

1行目: どういうものか

2行目: 何と重複してるか

### [Allow local variables to be static and/or readonly (deeper scoping) #49](https://github.com/dotnet/roslyn/issues/49)

ローカル変数に`static`(スコープが関数内に限られた静的フィールドを作る)とか`readonly`(書き換え不能)とか付けれるようにしてほしい。

以下の2つに分割できる。

- [Proposal: 'readonly' for Locals and Parameters #115](https://github.com/dotnet/roslyn/issues/115)
- [Please allow to define static local variables in functions, like C/C++ #10552](https://github.com/dotnet/roslyn/issues/10552)


### [Allow C# to use anonymous iterators. #24](https://github.com/dotnet/roslyn/issues/24)

匿名関数でイテレーターを作らせてほしい。

C# 7で入るローカル関数で同じことができる。


### [Please provide the C# Specification in PDF instead of DOCX #31](https://github.com/dotnet/roslyn/issues/31)

C#の仕様書をPDF提供してほしい。

長期作業として、マイクロソフトC#仕様書とECMA標準規格書の統合作業をしている。ECMAの方のはPDFで提供されてる。

訳注: 現体制になってから全然仕様書が表に出てこないんだけども… Long termってどのくらいLongなんだろう…


### [Lambdas omitting parameters #20](https://github.com/dotnet/roslyn/issues/20)

ラムダ式の引数のうち、要らないものは無視できる構文が欲しい。`someFunction((_, foo) => fooだけ使う);` みたいなの。

パターン マッチング(たぶんC# 8)と一緒にやる。


### [Extension properties for classes #112](https://github.com/dotnet/roslyn/issues/112)

メソッドだけじゃなくて、プロパティも拡張で作れるようにしてほしい。

以下の提案の一部分になる。

- [https://github.com/dotnet/roslyn/issues/11159](https://github.com/dotnet/roslyn/issues/11159)


### [UnreachableAfterAttribute and unreachable code detecting #59](https://github.com/dotnet/roslyn/issues/59)

常に例外を出すことが分かっていて、呼び出しよりも後ろのコードには絶対に到達しないことが保証できるようなメソッドを作りたい。そのために`UnreachableAfter`みたいな属性を作ってほしい。

[`never`型](https://github.com/dotnet/roslyn/issues/1226)ってのを検討中。


## 引き続き検討

1行目: どういうものか

2行目: どう取り組んでいるか

### [[Proposal] Object initializers for factory methods #133](https://github.com/dotnet/roslyn/issues/133)

`new T { X = ... }` みたいなのを、`new`演算子ではなく、ファクトリー メソッドを使った場合でも使えるようにしてほしい。`T.Create() { X = ... }` みたいな。

[`with`式](https://github.com/dotnet/roslyn/issues/5172) (たぶんC# 8になる)と合わせて考えたい。


### [Do not require type specification for constructors when the type is known #35](https://github.com/dotnet/roslyn/issues/35)

`var` (右辺値から変数の型を推論)の逆で、`T x = new();`みたいに、左辺値から`new`の型推論をしたい。

分解(deconstruction)の対となる構文として魅力的になりそう。

訳注: 分解の方が`var (x, y) = tuple;`みたいに型推論が効くんだから、構築(`new`)の方にも型推論が効いても良さげ。


### [IAsyncDisposable, using statements, and async/await #114](https://github.com/dotnet/roslyn/issues/114)

非同期Disposeがしたい。`using`ステートメントで、`IAsyncDisposable`的な非同期版のインターフェイスを使えるようにしてほしい。

[Lucian](https://github.com/ljw1004)にタスク割り当て。

訳注: たぶん、[非同期シーケンス](https://github.com/dotnet/roslyn/issues/261)と一緒に検討。Lucian は[Task-like](https://github.com/dotnet/roslyn/issues/7169)とか含め、非同期処理がらみの新仕様に取り組んでる人。


### [Extend LINQ syntax #100](https://github.com/dotnet/roslyn/issues/100)

クエリ式の後に`ToList()`とか`Single()`とかの、シーケンス自体に対する操作を呼ぶのが、今の構文だとだるい。

バックログに入れておく。`from x in items where x.foo, select x.bar do Single()` みたいな構文がいいかも。


### [[Proposal] (Minor) Method groups for constructors #140](https://github.com/dotnet/roslyn/issues/140)

コンストラクターをメソッド グループ(デリゲートにメソッドを渡すために、`Action a = Method`とか書くやつ)として認めてほしい。

よくある要求だし、よいアイディア。
