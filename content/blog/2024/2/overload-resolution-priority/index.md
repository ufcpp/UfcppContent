---
title: "オーバーロード解決優先度"
source_url: "https://ufcpp.net/blog/2024/2/overload-resolution-priority/"
content_type: "BlogEntry"
published_at: "2024-02-18T17:56:51"
updated_at: "2024-02-24T22:19:33"
tags: []
umbraco_id: 2487
parent_id: 2480
sort_order: 6
aliases: []
---

# オーバーロード解決優先度

今日は「負の遺産整理で消したいけども消せないメソッド対処」の話。
紆余曲折合って、現状、`OverloadResolutionPriority` 属性でオーバーロード解決に優先度をつけて、
優先度の高いものだけを見るようにするという案になっています。

最近のわかりやすい例だと、「パフォーマンス改善のために配列引数を `ReadOnlySpan` 引数に変えたい」というのをやりたいとします。

元々、配列引数で作っていたとして、

```csharp {title="元コード"}
int[] x = [1, 2, 3];

C.M(x);

// 元コード。
public class C
{
    // これの引数を変えたい。
    public static void M(int[] x) { }
}
```

暗黙的型変換があるものであれば、多少型を変えても「再コンパイルすれば大丈夫」という状態になることはあります。

```csharp {title="再コンパイルすれば大丈夫なこともある"}
int[] x = [1, 2, 3];

// int[] → ReadOnlySpan<int> の変更は、再コンパイルするならエラーにならず移行可能。
C.M(x);

// 変更後コード。
public class C
{
    // 引数、変えちゃった。
    public static void M(ReadOnlySpan<int> x) { }
}
```

ただ、「再コンパイル必須」というのは、
末端のアプリならともかく、ライブラリとかプラグインとかにとってはきついです。
過去にコンパイル済みバイナリの形でライブラリ参照すると、
先ほどの例は「`M(int[])` が見つからない」という実行時例外を起こします。

なので、現実的には「メソッドは追加する一方」になりがちなんですが、
非推奨にしたい古いメソッドによって利便性が損なわれることが多々あります。

```csharp {title="メソッドは追加する一方になりがち"}
int[] x = [1, 2, 3];

// 普通に書くと int[] の方に行っちゃう。
// パフォーマンスを理由に ReadOnlySpan<int> オーバーロードを足したのに無意味。
C.M(x);

// 変更後コード。
public class C
{
    // 元のメソッドは残しつつ、
    public static void M(int[] x) { }
    // オーバーロードを追加。
    public static void M(ReadOnlySpan<int> x) { }
}
```

非推奨にしたいものには `Obsolete` 属性を付けるという手段はありますが、
`Obsolete` 属性を付けたところでオーバーロード解決候補には残ってしまうのがかなり邪魔です。

```csharp {title="Obsolete でもオーバーロード解決候補になっちゃう" warning-text="C.M(x)" warning-diagnostics="sha256:2954556f99a9e2bb248fac9983f0bdbb3ed638e32b2e3403976930bd860edffc;CS0618@6:1-6:7"}
int[] x = [1, 2, 3];

// C# コンパイラーは Obsolete 属性が付いたメソッドも普通にオーバーロード解決候補にしちゃう。
// ReadOnlySpan<int> の方を呼んでほしくてやってるのに、
// 実際は int[] が選ばれたうえで警告が出るだけになる。
C.M(x);

// 変更後コード。
public class C
{
    // 古い方には Obsolete 属性を付ける。
    [Obsolete("Use M(ReadOnlySpan<int> x) instead.")]
    public static void M(int[] x) { }

    public static void M(ReadOnlySpan<int> x) { }
}
```

ということで、「バイナリ互換性のために残すけども、コンパイル時のオーバーロード解決候補には残さない」
(過去にコンパイルした DLL からは見えてるけども、ソースコードの再コンパイル時には見えない)
という状態を作りたいという要望があります。
これを「binary compat only」とか呼んでいます。

最初に思いつく案としては、`Obsolete` 属性に手を入れる方法。

```csharp {title="Obsolete 属性修正案"}
public class C
{
    // 最初に思いつく案として、Obsolete 属性を修正。
    [Obsolete("Use M(ReadOnlySpan<int> x) instead.", ObsoleteLevel.BinaryCompatOnly)]
    public static void M(int[] x) { }

    public static void M(ReadOnlySpan<int> x) { }
}
```

ただ、既存の `Obsolete` 属性に手を入れる案だと、例えば netstarndard2.0 向けライブラリとか、
ターゲットフレームワーク古いライブラリに対して使えなくなります。
なので新しい属性を足さざるを得ず。

当初案はまんま `BinaryCompatOnly` 属性でした。

* [Add proposal for BinaryCompatOnlyAttribute](https://github.com/dotnet/csharplang/pull/7707)

```csharp {title="BinaryCompatOnly 案"}
int[] x = [1, 2, 3];

// ReadOnlySpan<int> の方が選ばれるようになる予定。
C.M(x);

public class C
{
    // 新属性で「オーバーロード解決候補から外す」指定。
    [BinaryCompatOnly]
    public static void M(int[] x) { }

    public static void M(ReadOnlySpan<int> x) { }
}

public class BinaryCompatOnlyAttribute : Attribute;
```

ところが、じゃあ、「完全に候補から外す」だけでいいのかというと、そうでもなくて困ったみたいです。
例えば、インターフェイスの実装とかはどうするの？ということになりました。

```csharp {title="「完全に候補から外す」だけでいいのかどうか問題"}
public interface I
{
    // 新属性で「オーバーロード解決候補から外す」指定。
    [BinaryCompatOnly]
    void M(int[] x);

    // 新規追加メソッド。
    void M(ReadOnlySpan<int> x);
}

public class C : I
{
    // BinaryCompatOnly = コンパイル時には見えない
    // なわけで、 I.M(int[]) も「見えない」 = 実装できないのが正しい？
    //
    // こっちの M(int[]) にも BinaryCompatOnly 属性を付けることを義務付ける？
    public void M(int[] x) { }

    public void M(ReadOnlySpan<int> x) { }
}

public class BinaryCompatOnlyAttribute : Attribute;
```

そこで最終的に、

* オーバーロード解決に優先度をつけれるようにする
  * 何も指定がないときを 0 として、数字が大きいほど優先度を上げ、小さいほど下げる
* 優先度が一番高いものだけを候補にする

という案に修正されました。
属性名は `OverloadResolutionPriority`。

* [Add proposal for overload resolution priority](https://github.com/dotnet/csharplang/pull/7906)

```csharp {title="オーバーロード解決の優先度指定"}
int[] x = [1, 2, 3];

// ReadOnlySpan<int> の方が選ばれるようになる予定。
C.M(x);

public class C
{
    // 優先度を上げたければ priority の数字を増やす。
    [OverloadResolutionPriority(1)]
    public static void M(ReadOnlySpan<int> x) { }

    // 逆にこっちに priority = -1 とかを与えて優先度を下げるとかでも OK。
    [OverloadResolutionPriority(-1)]
    public static void M(int[] x) { }
}

public class OverloadResolutionPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; } = priority;
}
```

これなら先ほどのインターフェイスの例みたいな「見なくなりすぎる」問題は回避。
「高優先度のものが見つからなければ単に古い方を見に行く」みたいな挙動になります

まあ、具体化するには検討すべき項目はまだまだあるでしょうが
(例えば優先度は int で何でも受け付けるのでいいか？とか)、
方向性としては、C# チームも強く支持するし、
BCL 側もこれが入れば大々的に使いたい意向ありとのこと。
