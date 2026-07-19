---
title: "ピックアップRoslyn 4/8"
source_url: "https://ufcpp.net/blog/2020/4/pickuproslyn0408/"
content_type: "BlogEntry"
published_at: "2020-04-09T09:45:42"
updated_at: "2020-04-09T09:45:42"
tags: []
umbraco_id: 2288
parent_id: 2286
sort_order: 1
aliases: []
---

# ピックアップRoslyn 4/8

[前回話したことの続き](../begginingcs9/index.md)。
[ライブ配信で話したことの後半](https://youtu.be/JXa0JbfiYpw)。

C# Language Design Meetingの議事録がいくつか上がっています。

- [March 23rd, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-03-23.md)
- [March 25, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-03-25.md)
- [March 30, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-03-30.md)
- [April 1, 2020](https://github.com/dotnet/csharplang/blob/master/meetings/2020/LDM-2020-04-01.md)

この前後に2月頃の議事録も一気に上がっていて、
C# 9.0 がらみのデザイン決定も大詰めなんだなぁという感じがしています。
(実装期間を考えると、デザイン決定は今の時期が最も活発。)

ちなみに、[パターン マッチングの改善](https://github.com/dotnet/csharplang/issues/2850)の話とかは全然出てきませんが、
この辺りはデザインがすでに割と固まってるという話であって、立ち消えたとかではありません。

## トリアージ

いくつか、そもそも採用するかどうかの検討。

### ref struct 制約

今、[ref 構造体](../../../../study/csharp/resource/refstruct.md)にはインターフェイスを実装できないわけですが、
これは、ref 構造体は [box 化](../../../../study/csharp/resource/rmboxing.md)してはいけないという制約を守るためです。

一方で、ジェネリック型引数に `ref struct` 制約を付けれるなら、ref 構造体がインターフェイスを実装していても box 化を避けれるんじゃないかという話が上がっています。

ただ、検討の結果、`ref struct` 制約は必要として、それだけでも不十分だろうとのこと。
.NET ランタイムの方にも手を入れないと安全性を確保できないし、
[Shapes](https://github.com/dotnet/csharplang/issues/164)っていう C# 10.0 (次の次)で検討している機能とも領域が被るので、
`ref struct` 制約は 10.0 のタイミングで改めて検討しようという流れ。

### デリゲートのオーバーロード解決の改善

[C# 7.3 でメソッドのオーバーロード解決の改善](../../../../study/csharp/cheatsheet/ap_ver7_3.md#overload-resolution)をやってるんですが、
デリゲートの場合にはこの手の解決をしてないみたいです。

とりあえず C# 9.0 でデリゲートに対しても同様の改善を入れたいとのこと。
あと、同じく C# 9.0 目標で作業が進んでいる[関数ポインター](https://github.com/dotnet/csharplang/blob/master/proposals/function-pointers.md) (この後、今日のブログでも少し話題あり)でも同様のオーバーロード解決をしたいとのこと。

### 拡張メソッドの GetEnumerator

C# の [`foreach`](../../../../study/csharp/data/sp_foreach.md)は `GetEnumerator` メソッドの呼び出しに展開されるわけですが、
現状だと、インスタンス メソッドしか認めていなかったりします。
[この手の構文一覧](../../../../study/csharp/misc/miscpatternbased.md#index)を見てもらえるとわかるんですが、新しめの構文との一貫性が取れなくなっていたりします。
(新しいほど、拡張メソッドでも OK になってる。)

ただ、検討に上がってなかったわけではなくて、
終始「破壊的変更になりかねないからうかつに触りたくない」という雰囲気。

今回もそういう姿勢です。
「破壊的変更を起こさない限りにおいて歓迎」。
(ちなみに、[外部からの Pull Request](https://github.com/dotnet/roslyn/pull/43050) が来ているので、これに関する検討だと思います。
で、今、破壊的変更を起こしてないかの確認中。)

## native int

前々から、いわゆる「native int」(32ビットCPU上では３２ビット、64ビットCPU上では64ビットになるような整数型)が欲しいという話があります。

実のところ、今でも[`IntPtr`](https://docs.microsoft.com/ja-jp/dotnet/api/system.intptr)型がそういう挙動をしているんですが、
こいつは名前通りポインター扱いなので、算術・論理演算とかを全面的には認めていなかったりします。
そこで、算術・論理演算をちゃんと認めた native int 型として `nint` と `nuint` を足す流れになっています。

ただ、内部的には `nint` は `IntPtr` 構造体で、`nuint` は `UIntPtr` 構造体に展開する
(似て非なる型を追加することはしない)という方向で決定済み。
破壊的変更を起こさないように、
`nint` とかのキーワードを使って宣言しているときは算術・論理演算を認めて、
そうでないときはこれまで通り認めないという、セマンティクスを見た挙動変化をします。

今回は、じゃあ、`nint` を LangVersion 8 以前で動かす(`IntPtr` に見える)ときにどういう挙動をすべきかという話。
まあ、普通にコンパイル エラーにすべきだろうという感じ。

あと、`nint` の[定数](../../../../study/csharp/start/sp_const.md)はどう扱うべきかという話も。
32ビットCPUでもちゃんと動くようにするために、以下のようにするみたいです。

- 32ビットに収まる値は const 扱い
- それを超える場合は readonly static 的なものに展開

## target-typed new

target-typed new、要するに、`List<T> x = new ();` みたいな書き方でコンストクター呼び出しできるようにしようという話です。

この構文、「ライブラリ側で新たにコンストラクターが足された時に、利用側に破壊的変更が起きる可能性がある」っていう意味ではちょっと怖いんですが。
それを言うと、今でも `null` とか `default` が似たような状態なのでしょうがないだろうと。

`new ()` 自体は型を持っていないものの、`default` とかと同じで、ターゲットの型を見て具体的な型を決定して、その型のコンストラクターを呼びたいとのこと。
機械的にこの作業をすると、`enum` に対して `new ()` も書けてしまうことになるけども、
それは認めようという感じ。

あと、[null 許容値型](../../../../study/csharp/resource/sp2_nullable.md)に対して `T? x = new ();` と書いたときには
`null` 扱いじゃなくて `new T()` 扱い(`T?` の元になる型(underlying type) `T` の既定値)扱いすべきだろうという話も。

あと、[`dynamic`](../../../../study/csharp/dynamic/sp4_dynamic.md)に関しては、
今も `new dynamic()` を認めていないんだし、`dynamic x = new ();` は認めないようです。

## Record

Record 型周りは、[以前書いたんですが](../../2/pickuproslyn0203/index.md)、
init-only プロパティというのを足す方向で議論が進んでいます。
これに対して、init-only よりも、ビルダーパターンを使った実装にしてほしいという話が出ていたのでそれを検討。
結局は、init-only で行くみたいです。

あと、Record 型の仕様には `Equals`や`GetHashCode`メソッドの自動実装も含まれるわけですが、どういうコードを生成すべきかというのも議題になっています。
等値比較は複雑なカスタマイズをしたい要件が結構あります。
(例えば、配列だったら `SequenceEquals` すべきかとか、
文字列だったらカルチャー配慮した比較をすべきかとか。)
まあ、Record 的には、「手書きの `Equals` があればそっちを使う」という仕様なので、
カスタマイズが必要なら手書きしてほしい。
コンパイラーが自動生成する比較はシンプルな shallow 比較にしたいとのこと。

## 関数ポインター

元々は [static delegate](https://github.com/dotnet/csharplang/blob/master/proposals/static-delegates.md) とか言われていたやつなんですが、
今はもう、完全に「関数ポインター」扱いになっています。
ポインターという名があらわす通り、[native 相互運用](../../../../study/csharp/interop/sp_pinvoke.md)向けで、[unsafe](../../../../study/csharp/interop/sp_unsafe.md) な機能になります。

相互運用の際には呼び出し規約(引数とか戻り値を、どういう順番でどこに置くかみたいなルール)の指定が必要になるんですが、`cdecl` とか `stdcall` とか `fastcall` みたいな、今現在明確に決まっていて名前があるものだけじゃなく、
将来の規約追加にも耐えれる作りにしたいので、属性での指定もできるようにしたいとのこと。

あと、関数ポインター自体に属性を付けたい場合、`delegate* cdecl[属性]<void> ptr;` みたいな文法にするようです。

## プロパティ内の field キーワード

以下のようなやつ。

```csharp
public int P
{
    get => field ;
    set
    {
        PropertyChanged();
        field  = value;
    }
}
```

プロパティに対する[バック フィールド](../../../../study/csharp/oop/oo_property.md#auto_prop)にアクセスするためのキーワードが欲しいと。

これ、[Source Generator](https://github.com/dotnet/roslyn/blob/master/docs/features/source-generators.md)が入れば需要が激減するだろうし、
キーワード追加する割にはそこまで利便性が上がるわけでもないしで、
提案が出ては「今のところ取り組むつもりはない」みたいな返答を繰り返してた機能なんですが。

「重複提案があまりにも多い」と言って、今回ついに Design Meeting の議題に乗ったという。
ちなみに、僕が把握してる範囲でも重複 issue は20個くらいあります。
(その中の一番古い issue: [#133](https://github.com/dotnet/csharplang/issues/133)。
「これと重複してるよ」リンクがあまりに多いので、「被リンク」通知だらけです。)

とはいえ、「取り組みます」と言っているわけではなくて、
「誰も取り組んでない機能の中で最も頻繁に要求が来るものであることを認めた」みたいなノリ。
