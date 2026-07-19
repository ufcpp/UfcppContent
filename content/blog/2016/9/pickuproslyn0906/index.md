---
title: "ピックアップ Roslyn 9/6: defaultの型推論"
source_url: "https://ufcpp.net/blog/2016/9/pickuproslyn0906/"
content_type: "BlogEntry"
published_at: "2016-09-06T11:43:27"
updated_at: "2016-09-06T11:43:27"
tags: []
umbraco_id: 1954
parent_id: 1948
sort_order: 2
aliases: []
---

# ピックアップ Roslyn 9/6: defaultの型推論

なんか1個、気になったプルリクが。

- [Prototype: target typed "default" #13603](https://github.com/dotnet/roslyn/pull/13603)

`default(T)`を、左辺から型が推論できる場合には`default`だけで書けるようにしようっていう作業。

ちょっと前に提案があって、前向きっぽい感じではあったんですけど、なんで今実作業やってんだろう。
今からC# 7リリース辺りまでは、C# 7に入る予定の機能のバグ修正・改善ばっかりになると思ってたんですけども。
これくらいの修正ならさっと実装してC# 7に入れれそう？

まあ、これに限らず、今後、左辺から右辺の型推論は今後増えそうな感じです。

## 左辺と右辺

簡単に言葉の説明をしておくと、
左辺とか右辺ってのは、代入文の `=` の左右から来てる言葉。
`x = y`だったら、`x`が左辺で`y`が右辺です。

転じて、値を受け取る側が左辺で、値を渡す側が右辺。
変数宣言だと、宣言する変数が左辺で、初期値として渡す値が右辺です。
見た目は左右に分かれませんが、メソッド呼びだしなんかでも、値を受け取る側(仮引数)が左辺で値を渡す側(実引数)が右辺と考えられます。

## 右辺から左辺の型推論

C#の型推論って、右辺から左辺の推論が多いです。`var`しかり、ジェネリック型引数しかり。

以下のような感じ。

<pre class="source" title="右辺からの左辺の型推論(var、ジェネリック)">
<code><reserved></span><span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="reserved">var</span> x = 1; <span class="comment">// 1 からの推論で、int x = 1; 扱い。</span>
        F(2); <span class="comment">// 2 からの推論で、F&lt;int&gt;(2); 扱い</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> F&lt;<span class="type">T</span>&gt;(<span class="type">T</span> x) =&gt; System.<span class="type">Console</span>.WriteLine(<span class="reserved">typeof</span>(<span class="type">T</span>).Name);
}
</code></pre>

## 左辺から右辺の型推論

逆に左辺から右辺の型推論なのは、現状ではラムダ式くらい。

<pre class="source" title="左辺から右辺の型推論(ラムダ式)">
<code><reserved></span><span class="reserved">using</span> System;

<span class="reserved">class</span> <span class="type">Program</span>
{
    <span class="reserved">static</span> <span class="reserved">void</span> Main()
    {
        <span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">object</span>&gt; f = x =&gt; x.ToString(); <span class="comment">// f の型が&lt;int, string&gt;なので、(int x) =&gt; (object)x.ToString() 扱い</span>
        X(x =&gt; x.ToString()); <span class="comment">// Xの引数が&lt;int, string&gt;なので、(int x) =&gt; (object)x.ToString() 扱い</span>
    }

    <span class="reserved">static</span> <span class="reserved">void</span> X(<span class="type">Func</span>&lt;<span class="reserved">int</span>, <span class="reserved">object</span>&gt; f) { }
}
</code></pre>

で、今、`default`と`new`の型推論を増やしたいという話が出ています。特に、`var`を使えないフィールドやプロパティに対する初期化子で有効そう。

<pre class="source" title="defaultやnewの型推論">
<code><reserved></span><span class="reserved">class</span> <span class="type">Sample</span>&lt;<span class="type">T</span>&gt;
    <span class="reserved">where</span> <span class="type">T</span> : <span class="reserved">new</span>()
{
    <span class="reserved">static</span> <span class="type">T</span> newValue = <span class="reserved">new</span>();       <span class="comment">// new T() の T を省略</span>
    <span class="reserved">static</span> <span class="type">T</span> defaultValue = <span class="reserved">default</span>; <span class="comment">// default(T) の T を省略</span>

    <span class="reserved">static</span> <span class="reserved">void</span> F(<span class="type">T</span> x = <span class="reserved">default</span>) <span class="comment">// default(T) の T を省略</span>
    {
    }
}
</code></pre>

で、なんか、`default`の方はプロトタイプ実装が始まったと。
ちなみに、`new`の方は実装の兆し全然なし。
C#チーム的に前向きに検討したいとは言ってるんですけども、実装コストはそこそこ高そう。
引数ありで、`new(1, 2)`みたいなのもできる予定です。

## 補足: `new()`と`default`

余談になりますが、「`null`じゃダメなの？」とか、「`new()`だけじゃダメなの？」とか言ってる人もまあ、います。
C#に不慣れな人には結構わかりにくいですよね…

以下のような差があるので、いずれも必要です。

- `new T()`だとインスタンスが作られる。`null`にはならない
- `null`は参照型にしか入らない。ジェネリック型引数`T`とかだと、`T`が参照型なのか値型なのか確定しないから`null`を使えない
  - `default(T)`だと、参照型なら`null`、値型なら「全メンバー0/`null`初期化」
- 現状、値型であれば`new T()`と`default(T)`はどちらも「全メンバー0/`null`初期化」で、同じものになるけど、将来的に変えたい
  - 構造体にも引数なしのコンストラクターを定義できるようにしたい
  - `new T()`は引数なしのコンストラクター呼び出し
  - `default(T)`は「全メンバー0/`null`初期化」

ちなみに、構造体の引数なしコンストラクターの話は、一度C# 6に入りかかったんですけど、
`Activator.CreateInstance`の内部に「構造体の`new T()`と`default(T)`は同じ意味」っていう前提で最適化を掛けちゃってるコードがあるらしくて、
これを修正してもらわないとダメってことでrevertされました。

参考:

- [`new()`と`default`は相補的](https://github.com/dotnet/roslyn/issues/13255#issuecomment-241025874)
- [構造体にも引数なしのコンストラクターを認めようとして当時できなかった話](https://github.com/dotnet/roslyn/issues/1029)
- ['new'の左辺からの型推論認めたいって話](https://github.com/dotnet/roslyn/issues/35#issuecomment-239876676)
