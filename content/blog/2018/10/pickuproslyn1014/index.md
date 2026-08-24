---
title: "ピックアップRoslyn 10/14: #nonnullディレクティブ、IAsyncEnumerable"
source_url: "https://ufcpp.net/blog/2018/10/pickuproslyn1014/"
content_type: "BlogEntry"
published_at: "2018-10-14T11:57:17"
updated_at: "2018-10-14T11:57:17"
tags: []
umbraco_id: 2172
parent_id: 2171
sort_order: 0
aliases: []
---

# ピックアップRoslyn 10/14: #nonnullディレクティブ、IAsyncEnumerable

[Design Notes 2件追加](https://github.com/dotnet/csharplang/issues/1925)。

- [C# Language Design Notes for Oct 1, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-10-01.md)
- [C# Language Design Notes for Oct 3, 2018](https://github.com/dotnet/csharplang/blob/master/meetings/2018/LDM-2018-10-03.md)

10/1 のは、nullable 参照型がジェネリクスに絡むときの話。
例えば以下のような、型推論とかについての検討。

```csharp {warning-ranges="sha256:ff3d79316cef40741c32943c6cea899fa1c7e6e8b08513ed90383f0b5126b556;18:30-18:38"}
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        IEnumerable<string> nonNull = new[] { "" };
        IEnumerable<string?> nullable = new[] { default(string) };

        // 配列の要素の型推論。
        // これは、IEnumerable<string?>[] になってるみたい。
        var array = new[] { nonNull, nullable };

        // ジェネリック メソッドの型引数の型推論。
        // 配列の要素と似たような推論になるはず。
        // が、201/9/11 版の実装では IEnumerable<string> で推論されてる。
        // この挙動が変だよね、と言うのが議題。
        var ret = M(nonNull, nullable);
    }

    static T M<T>(T x, T y) => x;
}
```

10/3 の方は、null チェックのコンテキスト切り替えの再検討と、
`IAsyncEnumerable`インターフェイスの実装方法の決定について。

## null チェックのコンテキスト切り替え

改めての説明になりますが、元々 null の存在を前提にしている C# にとって、
nullable 参照型(`T` だと非 null、`T?` で null 許容)の追加は、何も考えずにやると破壊的変更になってしまいます。
破壊的変更を極力避けている C# にとってそれはまずいので、null チェックの On/Off を切り替える仕組みを用意する予定です。

これまでのプレビュー版では、とりあえず属性ベースでコンテキスト切り替えが実装されていました。
`NonNullTypes`属性を付けたら On、付けていないかもしくは`NonNullTypes(Warning = false)`で Off。
が、そのやり方だと苦しそうということで、プリプロセッサ－でやる(`#nonnull`ディレクティブみたいなものを追加する)ことを改めて検討しているそうです。

(これまでの C# だと、属性の有無によってコンパイラーの挙動がガラッと変わるというような実装をしたことがなく、
そういう「モラル的な意味」でもあまり良くないのは元々良くないんですが、技術的にも苦しそうなことがわかってきたとか。)

ということで、やるとしたら以下の3つのうちのどれかになるだろうということで、これらをそれぞれ検討。

- 修飾子を作る
    - 非同期メソッドの `async` 修飾子みたいなの
- 属性でやるなら、かなり特別扱いした「疑似属性」的なものになる
    - `const` を受け付けない(`true`, `false` の直指定しか受け付けない)とか、名前付き引数を認めないとか
- ディレクティブを使う
    - `#pragma warning`と同じノリで、`#nonnull disable`、`#nonnull restore`で制御
    - プロジェクト全体の On/Off 制御のために、コンパイラー オプションも必要

で、とりあえず、ディレクティブを使ったアプローチで行ってみようという感じになっているみたいです。
(実際もう、pull request も出てて merge 済み。)

## IAsyncEnumerableインターフェイスの実装方法

非同期ストリーム(`await`と`yield`の混在と、非同期版 `foreach`)を実装するにあたって、インターフェイスをどうするかというのがずっと課題になっていました。
同期版だと、`IEnumerable<T>`(と、同じ名前のメソッドを持ってさえいればOK)を使うわけですが、
それの非同期版である`IAsyncEnumarable<T>`はどういうメソッドを持つべきか。

結局、以下のように、`IEnumerable<T>`とほぼ同じで単に`Async`語尾を付け、`ValueTask`を返す作りにしたいとのこと。

```csharp
public interface IAsyncEnumerable<out T>
{
    IAsyncEnumerator<T> GetAsyncEnumerator();
}

public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    ValueTask<bool> MoveNextAsync();
    T Current { get; }
}

public interface IAsyncDisposable
{
    ValueTask DisposeAsync();
}
```

他の選択肢としては、以下のようなものが検討されていました。

```csharp
public interface IAsyncEnumerator<out T> : IAsyncDisposable
{
    ValueTask<bool> WaitForNextAsync();
    T TryGetNext(out bool success);
}
```

というのも、パフォーマンス的には後者の方がだいぶ良いことがわかっています。
ただ、これは今回新たに追加する非同期版だけの話ではなくて、
既存の`IEnumerable<T>`についても同じ課題を抱えています。

[`IEnumerable<T>`の方で軽く試してみた感じ](https://gist.github.com/ufcpp/0b9a8a8d4ea6b8eb6505ec0c624b65f8)、かかるオーバーヘッドがほんとに倍くらい変わり得ます。
というのも、仮想メソッド呼び出しのコストはそこそこあるので、`MoveNext`/`Current`の2回の呼び出しに分かれているより、`TryGetNext`の1回で済む方が明らかに速くなります。

上記の`IAsyncEnumerator<T>`では、`WaitForNextAsync`と`TryGetNext`の2つのメソッドがありますが、`WaitForNextAsync`の方は呼び出しがかなり少なくなる想定なので、実質的にはこちらでも「`TryGetNext`の1個だけになるので速い」ということが言えます。

が、以下のようなデメリットもあります。

- (`foreach`とかのコンパイラー生成に頼らず手動で)`WaitForNextAsync`と`TryGetNext`を使ったコードを書くのはかなり大変になる
    - .NET の仕様上、`bool TryGetNext(out T)` だと[共変](../../../../study/csharp/oop/sp4_variance.md#covariance)にできなくて、`T TryGetNext(out bool)`なのがキモい
    - 特に、[`Zip`](https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.zip) みたいに複数のenumerableが絡むとかなり大変
- 同期版でもどの道同じ問題があるんだから、もしやるなら、同期版の方も含めて `foreach` の拡張を後から考えるべき
    - 同期版が `MoveNext`/`Current` なのに非同期版だけ `TryGetNext` にするのは差が大きすぎる

結局、デメリットがきつすぎるということで、同期版と同じ`MoveNext`/`Current`型のインターフェイスにこだわりたいということになったようです。

### ValueTask 実装

この`IAsyncEnumerable<T>`に関する検討を始めた当初は、`Task`に掛かるコストが特に懸念されていました。
が、`ValueTask`を使った最適化が進んだ結果、案外、`ValueTask<bool> MoveNextAsync()`にすれば低コストになりそうというのもわかってきた結果、上記の決断に至ったというのもありそうです。

そちらの検討も、corefx の方に issue が立っています。

- [Proposal: Public APIs for C# 8 async streams #32640](https://github.com/dotnet/corefx/issues/32640)
- [Proposal: Implement IAsyncDisposable on various BCL types #32665](https://github.com/dotnet/corefx/issues/32665)
- [Proposal: ManualResetValueTaskSource{Logic} types #32664](https://github.com/dotnet/corefx/issues/32664)

[前にちょっと書きました](../../3/pickuproslyn0323/index.md)が、.NET Core 2.1 世代で、`ValueTask`が`Task`だけじゃなくて、`IValueTaskSource`インターフェイスを受け付けるようになりました。
このインターフェイスを実装した独自のクラスを作ることで、非同期処理に掛かるコストを下げれる場合があります
(作ったインスタンスをキャッシュ・再利用したり)。
非同期ストリーム(`await`と`yield`の混在)の実装はまさにその場合に該当していて、
`IValueTaskSource`を使ったコード生成をしようという流れになっています。

あと、`IValueTaskSource`自体は.NET Core 2.1世代([NuGetパッケージ](https://www.nuget.org/packages/System.Threading.Tasks.Extensions/4.5.1)を参照すればそれ以外でも利用可能)で追加されましたが、このインターフェイスをちゃんと実装するのはそれなりに面倒です
(いくつか、これを実装したクラスは現在もあるんですが、全部internalだったりします)。
そこで、汎用に使える`ManualResetValueTaskSource`という実装クラスも、これを機にpublicにしたいという話もついでに出ています。
