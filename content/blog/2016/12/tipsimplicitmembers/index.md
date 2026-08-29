---
title: "小ネタ 隠しメンバー"
source_url: "https://ufcpp.net/blog/2016/12/tipsimplicitmembers/"
content_type: "BlogEntry"
published_at: "2016-12-11T00:03:15"
updated_at: "2016-12-27T14:33:15"
tags: []
umbraco_id: 1990
parent_id: 1969
sort_order: 10
aliases: []
---

# 小ネタ 隠しメンバー

今日は、C# 上からは見えない隠しメンバーが作られるという話。
「覚えがないのになぜか『すでに定義があります』っていう名前被りのコンパイル エラーが出た」なんてこともあり得ます。

C# は .NET 向け言語の代表的な位置づけの言語ではありますが、だからと言って、C# の機能と .NET IL (中間言語)の機能は同じではありません。
C# コンパイラーによって、ILのレベルでは結構身に覚えのないメンバーが追加されます。

例えば以下のようなコードを書くだけで、自動的に追加されたメンバーがたくさん出てきます。

```csharp {title="全メンバーの列挙"}
using System;
using static System.Console;
using static System.Reflection.BindingFlags;

class C
{
    public int X { get; set; }
    public int this[int index] { get { return index; } set { } }
    public event Action E;
}

class Program
{
    static void Main()
    {
        foreach (var x in typeof(C).GetMembers(Public | NonPublic | Instance | DeclaredOnly))
        {
            WriteLine(x.Name);
        }
    }
}
```

実行結果は以下の通り。

```console {title="全メンバーの列挙"}
get_X
set_X
add_E
remove_E
get_Item
set_Item
.ctor
X
Item
E
<X>k__BackingField
E
```

今日はこれらについて説明して行きます。

## コンストラクター

これはまあ、わかりやすいですね。C# のクラスは、明示的にコンストラクターを書かなくても`new Class()`と書けるわけで、暗黙的にコンストラクターが1つ作られています(クラスで、コンストラクターを1つも書かなかった場合だけ)。

リフレクション的には、コンストラクターは`.ctor`という名前で見えます。
ちなみに、生成されるILを覗いてみると以下のような感じ。

```cil {title="コンストラクターの中身"}
.class private auto ansi beforefieldinit C
       extends [mscorlib]System.Object
{
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  nop
    IL_0007:  ret
  }
}
```

`rtspecialname`とかいう特別そうなフラグが付いているのと、
名前が変という以外はただのメソッドです。
中身も親クラスのコンストラクターを呼んでいるだけ。

## プロパティ

プロパティは、`get`、`set`に応じたメソッドと、
自動実装プロパティであればフィールドが1つ作られます。
今回の例では、`X`は自動実装プロパティで、`get`、`set`共に持っているので以下のようなILが生成されます。

```cil {title="プロパティの中身"}
  .field private int32 '<X>k__BackingField'
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 

  .property instance int32 X()
  {
    .get instance int32 C::get_X()
    .set instance void C::set_X(int32)
  }

  .method public hidebysig specialname instance int32 
          get_X() cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      int32 C::'<X>k__BackingField'
    IL_0006:  ret
  }

  .method public hidebysig specialname instance void 
          set_X(int32 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      int32 C::'<X>k__BackingField'
    IL_0007:  ret
  }
```

意味的には以下のような感じ。

- `<>`が含まれる読めた代物じゃないフィールドができる
- `get_`、`set_`から始まるメソッドができる
- フィールドとメソッドには`CompilerGenerated`属性が付いてる
- プロパティの定義自体は、メソッドを参照しているだけ

フィールドは、通常のC#では書けないような記号入りの名前なので特に問題を起こさないんですが、
メソッドの方は被りがあり得ます。つまり、以下のコードはコンパイル エラーを起こします。

```csharp {title="エラーを起こすコード" error-ranges="3:20-3:23"}
class C
{
    public int X { get; }
    int get_X() => 0;
}
```

しかもエラーを起こすのは `get` のところ。

## インデクサー

インデクサーなどというものは存在しない。いいね？

C#のインデクサーは、ILのレベルでは`Item`という名前のプロパティになっています。

```cil {title="インデクサーの中身"}
  .custom instance void [mscorlib]System.Reflection.DefaultMemberAttribute::.ctor(string) = ( 01 00 04 49 74 65 6D 00 00 ) // ...Item..

    .property instance int32 Item(int32)
  {
    .get instance int32 C::get_Item(int32)
    .set instance void C::set_Item(int32,
                                   int32)
  }

  .method public hidebysig specialname instance int32 
          get_Item(int32 index) cil managed
  {
    .maxstack  1
    .locals init ([0] int32 V_0)
    IL_0000:  nop
    IL_0001:  ldarg.1
    IL_0002:  stloc.0
    IL_0003:  br.s       IL_0005

    IL_0005:  ldloc.0
    IL_0006:  ret
  }

  .method public hidebysig specialname instance void 
          set_Item(int32 index,
                   int32 'value') cil managed
  {
    .maxstack  8
    IL_0000:  nop
    IL_0001:  ret
  }
```

意味的には以下のような感じ。

- `Item`という名前のプロパティが作られる
- プロパティは実は引数を取れる
  - (C#では無理だけど、VBなら引数付きプロパティも書ける)
- `DefaultMember`属性で`Item`プロパティを指定している

つまるところ、インデクサー = 「名前を省略していい引数付きプロパティ」です。

`Item`に展開されるので、もちろん、以下のコードは`this`のところでコンパイル エラー。

```csharp {title="エラーを起こすコード" error-text="this"}
class C
{
    public int this[int index] { get { return index; } }
    int Item { get; }
}
```

`get_Item`メソッドもダメです。`get`のところでエラー。

```csharp {title="エラーを起こすコード" error-ranges="3:34-3:37"}
class C
{
    public int this[int index] { get { return index; } }
    int get_Item(int index) => 0;
}
```

`Item`プロパティは普通に使いそうな名前なので、罠を踏むとしたらこれが一番頻出しそうなやつです。

ちなみに、回避方法も、まあ、あって、インデクサーから生成されるプロパティの名前は変更できます。

```csharp {title="インデクサーから生成されるプロパティの名前を明示的に指定"}
class C
{
    [System.Runtime.CompilerServices.IndexerName("Indexer")]
    public int this[int index] { get { return index; } }

    // ↑これで Item は生成されなくなるので、自前のもの↓と被らなくなる

    int Item { get; }
    int get_Item(int index) => 0;
}
```

ちなみに、C#コード上はインデクサーに`IndexerName`属性が付いていますが、
コンパイル結果的にはクラスに対する`DefaultMember`属性に変換されます。

## イベント

最後は、他の言語から来た人が困惑する機能ナンバー1、イベントです。
もっと難しい機能もたくさんありますけど、利用頻度の割に複雑という意味では断トツではないかと。
(使う頻度は多少あるけど、作る頻度はかなり低いんじゃないでしょうか。)

プロパティに近いんですが、`get`、`set`の代わりに`add`、`remove`です。
自動実装でフィールドが作れる部分は同じです。

```cil {title="イベントの中身"}
  .event [mscorlib]System.Action E
  {
    .addon instance void C::add_E(class [mscorlib]System.Action)
    .removeon instance void C::remove_E(class [mscorlib]System.Action)
  } // end of event C::E

  .field private class [mscorlib]System.Action E
  .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 

    .method public hidebysig specialname instance void 
          add_E(class [mscorlib]System.Action 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 結構長いのでさすがに省略
  }

  .method public hidebysig specialname instance void 
          remove_E(class [mscorlib]System.Action 'value') cil managed
  {
    .custom instance void [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor() = ( 01 00 00 00 ) 
    // 結構長いのでさすがに省略
  }
```

プロパティと似た感じで、

- フィールドができる
- `add_`、`remove_`から始まるメソッドができる
- フィールドとメソッドには`CompilerGenerated`属性が付いてる
- イベントの定義自体は、メソッドを参照しているだけ

という状態なんですが、ちょっと違うのは、以下の部分。

- フィールドの名前がイベントの名前とまったく同じ(この例の場合`E`)
- 自動生成されるメソッドの中身はほんと長い(参考: [補足: 自動イベント](../../../../study/csharp/functional/sp_event.md#auto-event))

C#では許されていませんが、ILレベルだと、メンバーの種類が違えば同じ名前を使えます。

そして、実は、イベントを触っているように見えて、実は裏で作られたフィールドを触っているという事態に。

```csharp {title="Eの参照の仕方"}
class C
{
    public event Action E;

    // 登録の側は add_E が呼ばれてるんだけど
    public void Register() => E += Handler;
    void Handler() { }

    // 呼び出し側では、実はイベントの E じゃなくて、フィールドの E
    public void Invoke() => E();
}
```

この`Invoke`メソッドの中を見てみると以下のような感じ。`ldfld`命令はフィールド読み込みのための命令です。

```cil {title="Invokeメソッドの中身"}
.method public hidebysig instance void  Call() cil managed
{
  .maxstack  8
  IL_0000:  ldarg.0
  IL_0001:  ldfld      class [mscorlib]System.Action C::E
  IL_0006:  callvirt   instance void [mscorlib]System.Action::Invoke()
  IL_000b:  nop
  IL_000c:  ret
}
```

つまり、イベントを明示的に実装すると、`E()`みたいな呼び出しはできなくなります。

```csharp {title="イベントを明示的実装に変えると、フィールドのEが消える"}
class C
{
    private Action _e;
    public event Action E
    {
        add { _e += value; }
        remove { _e -= value; }
    }

    // 明示的に add/remove を実装すると、自動実装なフィールドの E が消える
    // ↓このコードが書けなくなる
    public void Invoke() => E();
}
```

イベントの明示的な実装とかめったにするものじゃないのでそんなに踏まないと思いますが、一応注意が必要です。
