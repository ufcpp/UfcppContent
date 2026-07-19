---
title: "Roslynに提案issueを立てた話: nullの扱いに関して"
source_url: "https://ufcpp.net/blog/2016/11/nullproblem/"
content_type: "BlogEntry"
published_at: "2016-11-12T10:30:15"
updated_at: "2016-11-12T15:40:51"
tags: []
umbraco_id: 1967
parent_id: 1966
sort_order: 0
aliases: []
---

# Roslynに提案issueを立てた話: nullの扱いに関して

MVP Global Summit (グロサミ)に行ってきてたわけですが。

なんか、行きの飛行機内で思いついてしまって、そのまま向こうで頑張って issue 投稿、
せっかくだからグロサミ中に [Mads](https://github.com/MadsTorgersen) (C# のPM)を捕まえて「こんな問題見つけちゃって、昨日ちょうどissue立てたんだけどどうしよう？」みたいな話を振ってきたり。

(先に具体例を書いておけば、どれだけ英語がつたなくても結構意図は伝えられる。)

(ちなみに、元々は帰国後にゆっくりページ書くつもりだったんだけど、なんかグロサミ参加の妙なテンションに任せて、
[kekyoさん](https://twitter.com/kekyo2)、[藤原さん](https://twitter.com/yfakariya)、[室星さん](https://twitter.com/RyotaMurohoshi)とかと部屋の飲みの最中にこの方々も巻き込んでissue書きました。)

- [Proposal: user-defined null/default check (non-defaultable value types / nullable-like types) #15108](https://github.com/dotnet/roslyn/issues/15108)

勢いで立てたものなので整理できてないんですが… たぶん2パートに分けた方が良さそう。
以下、改めて要約した内容(英語に訳して再投稿予定)。

## 2つの提案

nullの取り扱いと関連して、以下の2つを提案する。

- 非default値型: `default(T)`の状態を認めない値型が必要ではないか
- nullable-like型: 参照型と`Nullable<T>`以外にも、`?.`や`??`が使える型をユーザー定義できるようにすべきではないか

## 非default値型

[Method Contracts](https://github.com/dotnet/roslyn/issues/119)、特に[参照型の非null保証](https://github.com/dotnet/roslyn/issues/5032)を入れようと思うと、確実な初期化処理が必須になる。

しかし、構造体の場合、`defautl(T)`など、既定値によって初期化処理を通らない「0/nullクリア」が発生する。
既定値の「0/nullクリア」のせいで、Contractsや非null保証のフロー解析が狂う可能性がある。

### 例1: 非null保証

パフォーマンスのために、参照型を1つだけ持つようなラッパーを構造体で作ることがある。
(例: [ImmutableArray](https://github.com/dotnet/corefx/blob/master/src/System.Collections.Immutable/src/System/Collections/Immutable/ImmutableArray_1.cs)、)
今、パフォーマンスはC#の1つの大きなテーマであり、こういうケースは今後より一層増えるだろう。

例として以下のような構造体を考える。

```csharp
struct Wrapper<T> where T : class
{
    public T Value { get; }
    public Wrapper(T value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}
```

[レコード型](https://github.com/dotnet/roslyn/blob/master/docs/features/records.md)や[非null保証](https://github.com/dotnet/roslyn/issues/5032)が入れば、単に以下のように書けるだろう。

```csharp
struct Wrapper<T>(T Value) whereT : class
```

単に`T`と書けば非nullとなり、nullを受け付けたければ`T?`と書くようになる。
問題は、この構造体を`default(Wrapper<T>)`で作ると、`T Value` (本来は非nullであるはず)がnullになってしまうことである。

### 例2: 値の制約付きの構造体

以下のような、値に制約の入った構造体を考える。この例は、正の数しか受け取れない数値型である。

```csharp
struct PositiveInt
{
    public int Value { get; }
    public PositiveInt(int value)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
        Value = value;
    }
}
```

C#に[レコード型](https://github.com/dotnet/roslyn/blob/master/docs/features/records.md)や[Method Contracts](https://github.com/dotnet/roslyn/issues/119)が入ると、この構造体はおそらく以下のように書ける。

```csharp
struct PositiveInt(int Value) requires Value > 0;
```

これで`Value`プロパティが常に0より大きいことが保証できているように見えるが、`default(PositiveInt)`のせいで、`Value`に0が入ることがあり得る。この値は無効なはずである。

### 提案: 非defaultフロー解析

現在提案されている参照型の[非null保証](https://github.com/dotnet/roslyn/issues/5032)は、フロー解析に基づいている。
値型が既定値でないことも、同じフロー解析で行えるはずである。

そこで、non-nullable reference typesに対して、non-defaultable value typesを提案したい。
何らかのアノテーション、例えば`[NonDefault]`属性を付けた構造体は既定値を取ってはいけないとするのはどうだろうか。

```csharp
[NonDefault]
struct Wrapper<T>(T Value) whereT : class

[NonDefault]
struct PositiveInt(int Value) requires Value > 0;
```

このとき、non-nullable reference typesに倣って、以下のように警告を出す。

```csharp
PositiveInt x = default(PositiveInt); // warning
PositiveInt? y = default(PositiveInt); // OK
PositiveInt z = y; // warning
PositiveInt w = y ?? new PositiveInt(1); // OK
```

non-defaultable value typesに対する`T?`は`Nullable<T>`を必要としない。
何故なら、`default(T)`は無効であり、`x.HasValue`を確かめなくても、`x == default(T)`で値を持っていないことが確認できるからである。
non-defaultable value typesに対しては`null`を`default(T)`と同一視してもいいかもしれない。

また、通常の構造体の中では、non-nullable reference typesのメンバーを持てないようにすべきだろう。
non-nullable reference typesを持てるのは、参照型か、non-defaultable value typesのみである。

## nullable-like型

現在、参照型と`Nullable<T>`構造体はC#コンパイラーによって特別な地位を与えられている。
すなわち、null条件演算子(`?.`)とnull合体演算子(`??`)の利用である。

しかし、これらの型以外にも、無効なインスタンスを`?.`や`??`で伝搬/差し替えしたいことがある。

この挙動はmonad的であり、[LINQ](http://mikehadlow.blogspot.jp/2011/01/monads-in-c-5-maybe.html)や[Task-like](https://github.com/ckimes89/arbitrary-async-return-nullable/)を使って無理やり解決しようとしている例もある。
しかし、悪用・乱用の類であり、決して読みやすいコードにはならないだろう。
無効なインスタンスの伝搬/差し替えは、やはり、`?.`や`??`を使うべきである。

### 例1: UnityEngine.Object

[Unity](http://japan.unity3d.com/)のゲーム中のオブジェクトの共通基底クラスとなる`UnityEngine.Object`は`operator ==`をオーバーロードしていて、オブジェクトが持っているネイティブ リソースがすでに破棄されているとき、オブジェクトをnull扱いする(`x == null`が真になる)。

しかし、参照型に対する`?.`や`??`では、オーバーロードした`==`は呼ばれない(`brtrue`命令によるnullチェックに展開される)。
そのため、以下のように、`?.`を使ったコードが正しく動かない。

```csharp
int? X(UnityEngine.Object obj)
{
    // OK
    if (obj == null) return null;
    return obj.GetInstanceID();
}

// runtime exception
int? Y(UnityEngine.Object obj) => obj?.GetInstanceID();
```

これまではUnityがC# 3.0にしか対応していなかったので問題にならなかったが、
Unity 5.5でC# 6.0に対応しようとしている。
この`?.`の挙動がはまるポイントになるだろう。

### 例2: Expected

無効な値としてnullを使うことを嫌う人が一定数いるが、その理由の1つが、「なぜ無効な値を返す必要があったのか」という原因に関する情報が消えることである。
そのため、`Nullable<T>`の代わりに、`T`と例外のunion型にあたる`Expected<T>`のような型を作って使おうとする人がいる。
例えば、[C++でそういう動きがみられる](http://www.open-std.org/jtc1/sc22/wg21/docs/papers/2014/n4015.pdf)。

```csharp
struct Expected<T>
{
    public T Value { get; }
    public Exception Exception { get; }
}
```

もしC#でもそういう型を作るのであれば、`?.`を使った例外の伝搬や、`??`を使った例外からの復帰があってもいいのではないだろうか。

```csharp
Expected<string> x = new Expected<string>(new Exception());
Expected<int> y = x?.Length;
string z = x ?? "";
```

### 提案: ユーザー定義のnullable-like型

所定のパターンを実装した型であれば`?.`および`??`を使えるようにすることを提案する。
[Task-like](https://github.com/dotnet/roslyn/issues/7169)に倣ってこれをnullable-likeと呼ぼう。

例えば、以下のようなパターンはどうだろうか。

```csharp
struct NullableLike<T>
{
    public T Value { get; }
    public bool HasValue { get; }
    // propagate a valid value
    public NullableLike<U> Propagate<U>(U value);
    // propagate an invalid value
    public NullableLike<T> Propagate();
}
```

これで、先ほどの`Expected<T>`の例であれば、以下のように展開する。

```csharp
Expected<string> x = new Expected<string>(new Exception());
Expected<int> y = x.HasValue ? x.Propagate(x.Value.Length) : x.Propagate<int>();
string z = x.HasValue ? x.Value : "";
```

ちなみに、このパターンに沿った`Expected<T>`の実装は以下のようになる。

```csharp
struct Expected<T>
{
    public T Value { get; }
    public Exception Exception { get; }

    public Expected(T value)
    {
        Value = value;
        Exception = null;
    }
    public Expected(Exception exception)
    {
        Value = default(T);
        Exception = exception;
    }

    public bool HasValue => Exception == null;
    public Expected<U> Propagate<U>() => new Expected<U>(Exception);
    public Expected<U> Propagate<U>(U value) => new Expected<U>(value);
}
```
