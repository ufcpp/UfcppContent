---
title: "仮想テーブルの中身をのぞき見"
source_url: "https://ufcpp.net/blog/2018/12/unsaferuntime/"
content_type: "BlogEntry"
published_at: "2018-12-29T10:38:59"
updated_at: "2018-12-29T10:38:59"
tags: []
umbraco_id: 2212
parent_id: 2177
sort_order: 29
aliases: []
---

# 仮想テーブルの中身をのぞき見

しばらくやってた `Unsafe` シリーズですが、今日は特に凶悪な奴です。

割かし最近なんですが、coreclr にこんなプルリクが出ていました。

- [Improve performance of `Memory<T>.Span` property getter](https://github.com/dotnet/coreclr/pull/20386)

これがまあ、なかなか凄いコードを含んでいます。仮想テーブルの中身を覗いて、「特定のビットが立っていたら配列」みたいなコードを書いています。

## 該当箇所

まず、[仮想テーブルのポインターを取得](https://github.com/dotnet/coreclr/blob/ef93a727984dbc5b8925a0c2d723be6580d20460/src/System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.cs#L222)

```csharp
private static IntPtr GetObjectMethodTablePointer(object obj)
{
    return Unsafe.Add(ref Unsafe.As<byte, IntPtr>(ref JitHelpers.GetPinningHelper(obj).m_data), -1);
}
```

- Managed なオブジェクトのアドレスを取得
- その場所の1ワード手前に仮想テーブルへのポインターが入っているはず

で、それを使って「配列かどうか」を判定。

```csharp
internal static unsafe bool ObjectHasComponentSize(object obj)
{
    return *(int*)GetObjectMethodTablePointer(obj) < 0;
}
```

- 仮想テーブルの最初の4バイトはヘッダーになっている
- ヘッダーの最上位ビットは「クラスが可変長かどうか」のフラグになっている
- .NET のクラスで可変長なのは配列と文字列だけ

とまあ、今現在の実装としてはこれで確かに「配列、もしくは、文字列かどうか」を判定できます。

## もちろん実装依存

当然ですが、今現在の実装としてできるからといって、将来もそうと言う保証はありません。
仕様として明言されているわけではなく、凄くきわどいコードです。

ギリギリ許されているのは、「coreclr 内の internal コードなので、もしランタイムに手を入れて仮想テーブルの実装が変わるようならその時に併せてここも直せばいい」という感じです。
coreclr の外で真似していいコードではないでしょう。

このプルリク内でも、「一旦はこれでマージしちゃっていいけど、`Unsafe`クラスを使った実装じゃなくて、ちゃんとランタイム側で判定用の intrinsic な API を提供すべき」という話の流れにはなっています。
さすがにいずれは消えると思われます。
