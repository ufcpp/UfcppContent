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

<pre class="source" title="全メンバーの列挙">
<code><span class="reserved">using</span> System;
<span class="reserved">using</span> <span class="reserved">static</span> System.<span class="type">Console</span>;
<span class="reserved">using</span> <span class="reserved">static</span> System.Reflection.<span class="type">BindingFlags</span>;

<span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="reserved">get</span>; <span class="reserved">set</span>; }
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] { <span class="reserved">get</span> { <span class="reserved">return</span> index; } <span class="reserved">set</span> { } }
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">Action</span> E;
}

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">foreach</span> (<span class="reserved">var</span> x <span class="reserved">in</span> <span class="reserved">typeof</span>(<span class="type">C</span>).GetMembers(Public | NonPublic | Instance | DeclaredOnly))
        {
            WriteLine(x.Name);
        }
    }
}
</code></pre>

実行結果は以下の通り。

<pre class="console" title="全メンバーの列挙">
<code>get_X
set_X
add_E
remove_E
get_Item
set_Item
.ctor
X
Item
E
&lt;X&gt;k__BackingField
E
</code></pre>

今日はこれらについて説明して行きます。

## コンストラクター

これはまあ、わかりやすいですね。C# のクラスは、明示的にコンストラクターを書かなくても`new Class()`と書けるわけで、暗黙的にコンストラクターが1つ作られています(クラスで、コンストラクターを1つも書かなかった場合だけ)。

リフレクション的には、コンストラクターは`.ctor`という名前で見えます。
ちなみに、生成されるILを覗いてみると以下のような感じ。

<pre class="source" title="コンストラクターの中身">
<code>.class <span class="reserved">private</span> <span class="reserved">auto</span> <span class="reserved">ansi</span> <span class="reserved">beforefieldinit</span> C
       <span class="reserved">extends</span> [mscorlib]System.Object
{
  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">rtspecialname</span> 
          <span class="reserved">instance</span> <span class="reserved">void</span>  <span class="reserved">.ctor</span>() <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Object::<span class="reserved">.ctor</span>()
    IL_0006:  nop
    IL_0007:  ret
  }
}
</code></pre>

`rtspecialname`とかいう特別そうなフラグが付いているのと、
名前が変という以外はただのメソッドです。
中身も親クラスのコンストラクターを呼んでいるだけ。

## プロパティ

プロパティは、`get`、`set`に応じたメソッドと、
自動実装プロパティであればフィールドが1つ作られます。
今回の例では、`X`は自動実装プロパティで、`get`、`set`共に持っているので以下のようなILが生成されます。

<pre class="source" title="プロパティの中身">
<code>  .field <span class="reserved">private</span> <span class="reserved">int32</span> '&lt;X&gt;k__BackingField'
  .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 

  .property <span class="reserved">instance</span> <span class="reserved">int32</span> X()
  {
    .get <span class="reserved">instance</span> <span class="reserved">int32</span> C::get_X()
    .set <span class="reserved">instance</span> <span class="reserved">void</span> C::set_X(<span class="reserved">int32</span>)
  }

  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">int32</span> 
          get_X() <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      <span class="reserved">int32</span> C::'&lt;X&gt;k__BackingField'
    IL_0006:  ret
  }

  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">void</span> 
          set_X(<span class="reserved">int32</span> 'value') <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      <span class="reserved">int32</span> C::'&lt;X&gt;k__BackingField'
    IL_0007:  ret
  }
</code></pre>

意味的には以下のような感じ。

- `<>`が含まれる読めた代物じゃないフィールドができる
- `get_`、`set_`から始まるメソッドができる
- フィールドとメソッドには`CompilerGenerated`属性が付いてる
- プロパティの定義自体は、メソッドを参照しているだけ

フィールドは、通常のC#では書けないような記号入りの名前なので特に問題を起こさないんですが、
メソッドの方は被りがあり得ます。つまり、以下のコードはコンパイル エラーを起こします。

<pre class="source" title="エラーを起こすコード">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> X { <span class="error"><span class="reserved">get</span></span>; }
    <span class="reserved">int</span> get_X() =&gt; 0;
}
</code></pre>

しかもエラーを起こすのは `get` のところ。

## インデクサー

インデクサーなどというものは存在しない。いいね？

C#のインデクサーは、ILのレベルでは`Item`という名前のプロパティになっています。

<pre class="source" title="インデクサーの中身">
<code>  .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Reflection.DefaultMemberAttribute::<span class="reserved">.ctor</span>(<span class="reserved">string</span>) = ( 01 00 04 49 74 65 6D 00 00 ) <span class="comment">// ...Item..
</span>
    .property <span class="reserved">instance</span> <span class="reserved">int32</span> Item(<span class="reserved">int32</span>)
  {
    .get <span class="reserved">instance</span> <span class="reserved">int32</span> C::get_Item(<span class="reserved">int32</span>)
    .set <span class="reserved">instance</span> <span class="reserved">void</span> C::set_Item(<span class="reserved">int32</span>,
                                   <span class="reserved">int32</span>)
  }

  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">int32</span> 
          get_Item(<span class="reserved">int32</span> index) <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .maxstack  1
    .locals init ([0] <span class="reserved">int32</span> V_0)
    IL_0000:  nop
    IL_0001:  ldarg.1
    IL_0002:  stloc.0
    IL_0003:  br.s       IL_0005

    IL_0005:  ldloc.0
    IL_0006:  ret
  }

  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">void</span> 
          set_Item(<span class="reserved">int32</span> index,
                   <span class="reserved">int32</span> 'value') <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .maxstack  8
    IL_0000:  nop
    IL_0001:  ret
  }
</code></pre>

意味的には以下のような感じ。

- `Item`という名前のプロパティが作られる
- プロパティは実は引数を取れる
  - (C#では無理だけど、VBなら引数付きプロパティも書ける)
- `DefaultMember`属性で`Item`プロパティを指定している

つまるところ、インデクサー = 「名前を省略していい引数付きプロパティ」です。

`Item`に展開されるので、もちろん、以下のコードは`this`のところでコンパイル エラー。

<pre class="source" title="エラーを起こすコード">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="error"><span class="reserved">this</span></span>[<span class="reserved">int</span> index] { <span class="reserved">get</span> { <span class="reserved">return</span> index; } }
    <span class="reserved">int</span> Item { <span class="reserved">get</span>; }
}
</code></pre>

`get_Item`メソッドもダメです。`get`のところでエラー。

<pre class="source" title="エラーを起こすコード">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] { <span class="error"><span class="reserved">get</span></span> { <span class="reserved">return</span> index; } }
    <span class="reserved">int</span> get_Item(<span class="reserved">int</span> index) =&gt; 0;
}
</code></pre>

`Item`プロパティは普通に使いそうな名前なので、罠を踏むとしたらこれが一番頻出しそうなやつです。

ちなみに、回避方法も、まあ、あって、インデクサーから生成されるプロパティの名前は変更できます。

<pre class="source" title="インデクサーから生成されるプロパティの名前を明示的に指定">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    [System.Runtime.CompilerServices.<span class="type">IndexerName</span>(<span class="string">"Indexer"</span>)]
    <span class="reserved">public</span> <span class="reserved">int</span> <span class="reserved">this</span>[<span class="reserved">int</span> index] { <span class="reserved">get</span> { <span class="reserved">return</span> index; } }

    <span class="comment">// ↑これで Item は生成されなくなるので、自前のもの↓と被らなくなる</span>

    <span class="reserved">int</span> Item { <span class="reserved">get</span>; }
    <span class="reserved">int</span> get_Item(<span class="reserved">int</span> index) =&gt; 0;
}
</code></pre>

ちなみに、C#コード上はインデクサーに`IndexerName`属性が付いていますが、
コンパイル結果的にはクラスに対する`DefaultMember`属性に変換されます。

## イベント

最後は、他の言語から来た人が困惑する機能ナンバー1、イベントです。
もっと難しい機能もたくさんありますけど、利用頻度の割に複雑という意味では断トツではないかと。
(使う頻度は多少あるけど、作る頻度はかなり低いんじゃないでしょうか。)

プロパティに近いんですが、`get`、`set`の代わりに`add`、`remove`です。
自動実装でフィールドが作れる部分は同じです。

<pre class="source" title="イベントの中身">
<code>  .event [mscorlib]System.Action E
  {
    .addon <span class="reserved">instance</span> <span class="reserved">void</span> C::add_E(<span class="reserved">class</span> [mscorlib]System.Action)
    .removeon <span class="reserved">instance</span> <span class="reserved">void</span> C::remove_E(<span class="reserved">class</span> [mscorlib]System.Action)
  } <span class="comment">// end of event C::E
</span>
  .field <span class="reserved">private</span> <span class="reserved">class</span> [mscorlib]System.Action E
  .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 

    .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">void</span> 
          add_E(<span class="reserved">class</span> [mscorlib]System.Action 'value') <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 
    <span class="comment">// 結構長いのでさすがに省略
</span>  }

  .method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">specialname</span> <span class="reserved">instance</span> <span class="reserved">void</span> 
          remove_E(<span class="reserved">class</span> [mscorlib]System.Action 'value') <span class="reserved">cil</span> <span class="reserved">managed</span>
  {
    .custom <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Runtime.CompilerServices.CompilerGeneratedAttribute::<span class="reserved">.ctor</span>() = ( 01 00 00 00 ) 
    <span class="comment">// 結構長いのでさすがに省略
</span>  }
</code></pre>

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

<pre class="source" title="Eの参照の仕方">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">Action</span> E;

    <span class="comment">// 登録の側は add_E が呼ばれてるんだけど</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Register() =&gt; E += Handler;
    <span class="reserved">void</span> Handler() { }

    <span class="comment">// 呼び出し側では、実はイベントの E じゃなくて、フィールドの E</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Invoke() =&gt; E();
}
</code></pre>

この`Invoke`メソッドの中を見てみると以下のような感じ。`ldfld`命令はフィールド読み込みのための命令です。

<pre class="source" title="Invokeメソッドの中身">
<code>.method <span class="reserved">public</span> <span class="reserved">hidebysig</span> <span class="reserved">instance</span> <span class="reserved">void</span>  Call() <span class="reserved">cil</span> <span class="reserved">managed</span>
{
  .maxstack  8
  IL_0000:  ldarg.0
  IL_0001:  ldfld      <span class="reserved">class</span> [mscorlib]System.Action C::E
  IL_0006:  callvirt   <span class="reserved">instance</span> <span class="reserved">void</span> [mscorlib]System.Action::Invoke()
  IL_000b:  nop
  IL_000c:  ret
}
</code></pre>

つまり、イベントを明示的に実装すると、`E()`みたいな呼び出しはできなくなります。

<pre class="source" title="イベントを明示的実装に変えると、フィールドのEが消える">
<code><span class="reserved">class</span> <span class="type">C</span>
{
    <span class="reserved">private</span> <span class="type">Action</span> _e;
    <span class="reserved">public</span> <span class="reserved">event</span> <span class="type">Action</span> E
    {
        <span class="reserved">add</span> { _e += <span class="reserved">value</span>; }
        <span class="reserved">remove</span> { _e -= <span class="reserved">value</span>; }
    }

    <span class="comment">// 明示的に add/remove を実装すると、自動実装なフィールドの E が消える</span>
    <span class="comment">// ↓このコードが書けなくなる</span>
    <span class="reserved">public</span> <span class="reserved">void</span> Invoke() =&gt; E();
}
</code></pre>

イベントの明示的な実装とかめったにするものじゃないのでそんなに踏まないと思いますが、一応注意が必要です。
