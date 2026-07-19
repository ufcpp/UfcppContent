---
title: "IL の概要"
source_url: "https://ufcpp.net/study/il/summary/il_about/"
content_type: "Article"
published_at: "2012-01-29T00:00:00"
updated_at: "2015-05-06T14:15:54"
tags: []
umbraco_id: 1442
parent_id: 1441
sort_order: 0
aliases:
  - "/il/summary/il_about/"
---

# IL の概要

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
（書きかけ）

C# などの、.NET Framework 上で動作するプログラミング言語は、
一度、.NET の中間言語（intermediate language）にコンパイルされます。

.NET の場合、中間言語のことを、単に略して IL と呼んだり、
MSIL（Microsoft Intermediate Language）や CIL（Common intermediate language）と呼んだりします。
（以後、IL という表記に統一します。）


##<a id="sec-generated-title-2"></a> <a id="mechanism"></a>.NET の仕組み
<pre>
いくつか、用語を説明

C# など → IL → ネイティブ コード
の流れ

JIT（Just-In-Time コンパイル）

Pre-JIT（NGen）
特に、Windows 8/.NET 4.5 では Auto-NGen

マシン語（数値化した IL）と、アセンブリ言語（human-readable な IL 表現）
</pre>
C#をはじめとする、.NET対応言語は、.NETの仮想マシンが直接解釈できるILマシン語にコンパイルされます。アプリやライブラリは、このILの状態で配布します。

一般的な（仮想マシンか実CPUか問わず）実行環境と同様に、ILには、マシン語と1対1に対応していて、ある程度人間でも読めるような言語（アセンブリ言語）が定義されています。

ILは、.NET Frameworkの仮想マシンによって、都度、ネイティブ コード（実CPUが直接解釈できるマシン語）にコンパイルしながら（Just-In-Time方式のコンパイル、略してJIT）実行されます。


##<a id="sec-generated-title-3"></a> <a id="why"></a>なぜ IL
<pre>
・コンパイラー作る側の都合

C# などの高級言語の専門家と、
x86, x64, ARM などの CPU に応じた最適化の専門家に分けたい

C# とかに新機能を追加しつつ、
CPU ごとに高度な最適化をしようと思うと、
一旦 IL を介さざるを得ない。


・セキュリティ的な検証できるメリット


・メタデータ
これは IL だからというわけでもなく、COM なんかでもそうだけど
メタデータを持ちたい
</pre>

##<a id="sec-generated-title-4"></a> <a id="assembly"></a>.NET アセンブリ
C#などのソース コードのコンパイル結果、つまり、.NET向けの実行可能形式（exe）やライブラリ（dll）を総称して、.NETアセンブリ（assembly）と言います。

アセンブリの中には、メタデータ（型情報や属性）と、ILコードが入っています。
<pre>
図を入れる
</pre>

##<a id="sec-generated-title-5"></a> <a id="metadata"></a>メタデータ
<pre class="source" title="C#" lang="">
<code><span class="reserved">public</span> <span class="reserved">class</span> <span class="type">Sample</span>
{
    <span class="reserved">private</span> <span class="reserved">readonly</span> <span class="reserved">string</span> _name;
    <span class="reserved">public</span> <span class="reserved">string</span> Name { <span class="reserved">get</span> { <span class="reserved">return</span> _name; } }

    <span class="reserved">public</span> <span class="reserved">int</span> Value { <span class="reserved">get</span>; <span class="reserved">set</span>; }

    <span class="reserved">public</span> Sample(<span class="reserved">string</span> name, <span class="reserved">int</span> value = 0)
    {
        _name = name;
        Value = value;
    }
}
</code></pre>


メタデータの例:

<pre class="source" title="IL" lang="">
<code>.class public auto ansi beforefieldinit Sample
       extends [mscorlib]System.Object
{
}

.field private initonly string _name

.property instance string Name()
{
  .get instance string Sample::get_Name()
}

.property instance int32 Value()
{
  .get instance int32 Sample::get_Value()
  .set instance void Sample::set_Value(int32)
}

<span class="input">...後略...</span>
</code></pre>


どのクラスがどういうメンバーを持っていてとか、そういう情報が全部アセンブリ内に残る。

これはアセンブリ言語表現の IL。
実際にアセンブリに入るのは、この情報をバイナリ化したもの。
<pre>
. から始まる行がメタデータ定義

TypeDef （リフレクションで使うような型全部の情報）と
TypeRef （型の弁別情報だけ）

デリゲートとか列挙型とかも、IL ネイティブ
（クラスとかが生成されてるわけじゃない。IL レベルにある。）

ジェネリック
</pre>

##<a id="sec-generated-title-6"></a> <a id="metadata"></a>IL 命令
IL コードの例:

<pre class="source" title="IL" lang="">
<code>.method public hidebysig specialname rtspecialname 
        instance void  .ctor(string name,
                             [opt] int32 'value') cil managed
{
  .param [2] = int32(0x00000000)
  .maxstack  8
  IL_0000:  ldarg.0
  IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
  IL_0006:  ldarg.0
  IL_0007:  ldarg.1
  IL_0008:  stfld      string Sample::_name
  IL_000d:  ldarg.0
  IL_000e:  ldarg.2
  IL_000f:  call       instance void Sample::set_Value(int32)
  IL_0014:  ret
}
</code></pre>


IL_0000 とかはラベル。制御フローに使う。
ldarg.0 とかが IL 命令。

ほとんどが1バイト命令。
ごく一部、2バイト命令、4バイト命令もあり。

いくつか紹介。
アセンブリ言語 ⇔ マシン語 対応表込みで。
例えば ldarg.0 なら0x02。
<pre>
スタック型マシン
任意の型を受け付けるスタック
（＋ ローカル変数テーブル、引数テーブル）

call 命令とかの後ろにメソッド名が入ってるけども、マシン語的には、Token と呼ばれる4バイトの数値になってる。
Token の値を元に、メタデータを検索して実際の型やメソッド名を得る。

プリミティブ
（int とかだけ、パフォーマンス的な理由でいろいろ特別扱い受けてる。
  専用命令がある。）

定数ロードとか、いくつかの命令はオペランドなし版がある。
（命令長的にお得）

オペランド付きの命令は、1バイト（short）版と4バイト（long）版がある。

if とか for に相当する命令はない。
br 命令で（高級言語でいうと if goto 相当）

switch だけは専用命令がある。
case の数次第で、if-else 相当コードか、ジャンプ テーブルに JIT


ガベコレ

ネイティブ interop
</pre>
