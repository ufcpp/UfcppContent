---
title: "Java 開発者向けチート シート"
source_url: "https://ufcpp.net/study/csharp/cs4j/java_cheatsheet/"
content_type: "Article"
published_at: "2011-05-25T00:00:00"
updated_at: "2015-05-18T08:42:02"
tags: []
umbraco_id: 1373
parent_id: 1372
sort_order: 0
aliases:
  - "/csharp/cs4j/java_cheatsheet/"
  - "/csharp/java_cheatsheet"
  - "/csharp/java_cheatsheet.html"
  - "/study/csharp/java_cheatsheet"
  - "/study/csharp/java_cheatsheet.html"
---

# Java 開発者向けチート シート

## <a id="sec-generated-title-1"></a> <a id="abst"></a>概要

キーワードや演算子の対訳表。

参考: [Moving to C# and the .NET Framework, for Java Developers](http://msdn.microsoft.com/en-us/gg715299.aspx)。


## <a id="sec-generated-title-2"></a> <a id="keyword"></a>キーワード

<table summary="">

	<tr>
		<th>Java</th>
		<th>C#</th>
		<th>関連記事</th>
	</tr>
	<tr>
		<td markdown="1"><code>abstract</code></td>
		<td markdown="1"><code>abstract</code></td>
		<td markdown="1">「[抽象化](../oop/oo_abstract.md#abstraction)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>assert</code></td>
		<td markdown="1">なし</td>
		<td markdown="1">キーワードではなく、Debug.Assert メソッドとして提供。</td>
	</tr>
	<tr>
		<td markdown="1"><code>break</code></td>
		<td markdown="1"><code>break</code></td>
		<td markdown="1">「[while 文](../structured/st_loop.md#while)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>case</code></td>
		<td markdown="1"><code>case</code></td>
		<td markdown="1">「[switch 文](../structured/st_branch.md#switch)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>catch</code></td>
		<td markdown="1"><code>catch</code></td>
		<td markdown="1">「[例外処理構文](../structured/oo_exception.md#syntax)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>class</code></td>
		<td markdown="1"><code>class</code></td>
		<td markdown="1">「[クラス定義](../oop/oo_class.md#definition)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>continue</code></td>
		<td markdown="1"><code>continue</code></td>
		<td markdown="1">「[while 文](../structured/st_loop.md#while)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>default</code></td>
		<td markdown="1"><code>default</code></td>
		<td markdown="1">「[switch 文](../structured/st_branch.md#switch)」「[既定値](../oop/sp2_generics.md#default)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>do</code></td>
		<td markdown="1"><code>do</code></td>
		<td markdown="1">「[do-while 文](../structured/st_loop.md#dowhile)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>else</code></td>
		<td markdown="1"><code>else</code></td>
		<td markdown="1">「[if 文](../structured/st_branch.md#if)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>enum</code></td>
		<td markdown="1"><code>enum</code></td>
		<td markdown="1">「[列挙型とは](../structured/st_enum.md#about)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>extends</code></td>
		<td markdown="1"><code>:</code></td>
		<td markdown="1">「[クラスの継承](../oop/oo_inherit.md#inherit)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>false</code></td>
		<td markdown="1"><code>false</code></td>
		<td markdown="1">「[論理値型](../start/st_embeddedtype.md#bool)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>final</code></td>
		<td markdown="1"><code>sealed</code></td>
		<td markdown="1">「[sealed](../oop/oo_inherit.md#sealed)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>finally</code></td>
		<td markdown="1"><code>finally</code></td>
		<td markdown="1">「[例外処理構文](../structured/oo_exception.md#syntax)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>for</code></td>
		<td markdown="1"><code>for / foreach</code></td>
		<td markdown="1">「[for 文](../structured/st_loop.md#for)」「[foreach文](../structured/st_loop.md#foreach)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>if</code></td>
		<td markdown="1"><code>if</code></td>
		<td markdown="1">「[if 文](../structured/st_branch.md#if)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>implements</code></td>
		<td markdown="1"><code>:</code></td>
		<td markdown="1">「[C# のインターフェース](../oop/oo_interface.md#interface)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>import</code></td>
		<td markdown="1"><code>using</code></td>
		<td markdown="1">「[名前空間の使い方](../structured/sp_namespace.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>instanceof</code></td>
		<td markdown="1"><code>is</code></td>
		<td markdown="1">「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>interface</code></td>
		<td markdown="1"><code>interface</code></td>
		<td markdown="1">「[C# のインターフェース](../oop/oo_interface.md#interface)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>native</code></td>
		<td markdown="1"><code>extern</code></td>
		<td markdown="1">「[外部エイリアス](../structured/sp_namespace.md#extern)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>new</code></td>
		<td markdown="1"><code>new</code></td>
		<td markdown="1">「[クラスの利用](../oop/oo_class.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>null</code></td>
		<td markdown="1"><code>null</code></td>
		<td markdown="1">「[クラスの利用](../oop/oo_class.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>package</code></td>
		<td markdown="1"><code>namespace</code></td>
		<td markdown="1">「[名前空間の使い方](../structured/sp_namespace.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>private</code></td>
		<td markdown="1"><code>private</code></td>
		<td markdown="1">「[アクセスレベル](../oop/oo_conceal.md#level)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>protected</code></td>
		<td markdown="1"><code>protected</code></td>
		<td markdown="1">「[アクセスレベル](../oop/oo_conceal.md#level)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>public</code></td>
		<td markdown="1"><code>public</code></td>
		<td markdown="1">「[アクセスレベル](../oop/oo_conceal.md#level)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>return</code></td>
		<td markdown="1"><code>return</code></td>
		<td markdown="1">「[関数定義](../structured/st_function.md#definition)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>static</code></td>
		<td markdown="1"><code>static</code></td>
		<td markdown="1">「[静的メンバーの使い方](../oop/oo_static.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>strictfp</code></td>
		<td markdown="1">なし</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>super</code></td>
		<td markdown="1"><code>base</code></td>
		<td markdown="1">「[基底クラスのコンストラクタを明示的に呼び出す](../oop/oo_inherit.md#base_ctor)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>switch</code></td>
		<td markdown="1"><code>switch</code></td>
		<td markdown="1">「[switch 文](../structured/st_branch.md#switch)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>synchronized</code></td>
		<td markdown="1"><code>lock</code></td>
		<td markdown="1">「[lock 文](../async/sp_thread.md#lock)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>this</code></td>
		<td markdown="1"><code>this</code></td>
		<td markdown="1">「[コンストラクター](../oop/oo_construct.md#ctor)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>throw</code></td>
		<td markdown="1"><code>throw</code></td>
		<td markdown="1">「[例外処理構文](../structured/oo_exception.md#syntax)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>throws</code></td>
		<td markdown="1">なし</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<td markdown="1"><code>transient</code></td>
		<td markdown="1">なし</td>
		<td markdown="1">[Nonserialized]「[属性](../dynamic/sp_attribute.md#attribute)」を利用。</td>
	</tr>
	<tr>
		<td markdown="1"><code>true</code></td>
		<td markdown="1"><code>true</code></td>
		<td markdown="1">「[論理値型](../start/st_embeddedtype.md#bool)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>try</code></td>
		<td markdown="1"><code>try</code></td>
		<td markdown="1">「[例外処理構文](../structured/oo_exception.md#syntax)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>...</code></td>
		<td markdown="1"><code>params</code></td>
		<td markdown="1">「[params キーワード](../structured/sp_params.md#params)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>void</code></td>
		<td markdown="1"><code>void</code></td>
		<td markdown="1">「[引数が複数ある関数、引数のない関数、戻り値のない関数](../structured/st_function.md#void)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>volatile</code></td>
		<td markdown="1"><code>volatile</code></td>
		<td markdown="1">「[volatile](../async/sp_thread.md#volatile)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>while</code></td>
		<td markdown="1"><code>while</code></td>
		<td markdown="1">「[while 文](../structured/st_loop.md#while)」</td>
	</tr>
</table>



### <a id="sec-generated-title-3"></a> <a id="not-in-java-keyword"></a>Java にないキーワード

Java にはないか、あっても少し挙動の違うキーワードの一覧です。

<table summary="">

	<tr>
		<th>キーワード</th>
		<th>説明</th>
		<th>関連記事</th>
	</tr>
	<tr>
		<td markdown="1"><code>checked / unchecked</code></td>
		<td markdown="1">整数演算のオーバーフロー時に例外を発生させるかどうかを選択する。</td>
		<td markdown="1">「[checked キーワード](../start/sp_checked.md#checked)」「[unchecked キーワード](../start/sp_checked.md#unchecked)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>const</code></td>
		<td markdown="1">定数を作る。 Java はキーワードの予約のみで、実際は機能を持っていません。</td>
		<td markdown="1">「[const](../start/sp_const.md#const)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>delegate</code></td>
		<td markdown="1">メソッドを参照するための型を作る。</td>
		<td markdown="1">「[デリゲートの定義](../functional/sp_delegate.md#definition)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>event</code></td>
		<td markdown="1">イベント ハンドラーの登録口を作る。</td>
		<td markdown="1">「[イベント](../functional/sp_event.md#event)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>explicit / implicit</code></td>
		<td markdown="1">キャスト演算子用。暗黙の型変換を認めるかどうかを指定する。</td>
		<td markdown="1">「[演算子のオーバーロードの方法](../oop/oo_operator.md#overload)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>goto</code></td>
		<td markdown="1">いわゆる goto 文（無条件ジャンプ）。 Java はキーワードの予約のみで、実際は機能を持っていません。</td>
		<td markdown="1">「[goto 文](../structured/st_branch.md#goto)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>in / out</code></td>
		<td markdown="1">ジェネリクスの共変性/反変性を指定する。</td>
		<td markdown="1">「[ジェネリックの共変性・反変性](../oop/sp4_variance.md#variance)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>new</code></td>
		<td markdown="1">（new 演算子の他に） 基底クラスのメンバーを隠して再実装するために使う。</td>
		<td markdown="1">「[基底クラスのメンバーの隠蔽](../oop/oo_inherit.md#conceal)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>operator</code></td>
		<td markdown="1">演算子をオーバーロードする。</td>
		<td markdown="1">「[演算子のオーバーロードの方法](../oop/oo_operator.md#overload)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>out / ref</code></td>
		<td markdown="1">メソッドの引数を参照渡しする。</td>
		<td markdown="1">「[参照渡し](../resource/sp_ref.md#byref)」「[出力引数](../resource/sp_ref.md#out)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>override</code></td>
		<td markdown="1">C# では、仮想メソッドをオーバーライドする際に override 修飾子を付けます。 （スペル ミスによるオーバーライド失敗を検出できるように。）</td>
		<td markdown="1">「[仮想メソッド](../oop/oo_polymorphism.md#virtual)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>protected</code></td>
		<td markdown="1">Java の protected メンバーは、派生クラス内からだけでなく、パッケージ内の他のクラスから参照できます（C# で一番近いのは protected internal）。 C# の protected メンバーはクラス内、もしくは派生クラス内からのみ参照できます。</td>
		<td markdown="1">「[アクセスレベル](../oop/oo_conceal.md#level)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>readonly</code></td>
		<td markdown="1">読み取り専用メンバーを作る。 Java でいうところの、フィールドに対して final を付けた時と同様の挙動になります。</td>
		<td markdown="1">「[readonly](../start/sp_const.md#readonly)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>struct</code></td>
		<td markdown="1">値型を作る。</td>
		<td markdown="1">「[構造体とは](../structured/st_struct.md#about)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>unsafe</code></td>
		<td markdown="1">unsafe コードを有効にする。</td>
		<td markdown="1">「[unsafe コード](../interop/sp_unsafe.md#unsafe)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>virtual</code></td>
		<td markdown="1">Java では全てのメソッドが仮想メソッドになりますが、 C# では virtual 修飾子を付けたものだけが仮想メソッドになります。</td>
		<td markdown="1">「[仮想メソッド](../oop/oo_polymorphism.md#virtual)」</td>
	</tr>
</table>



### <a id="sec-generated-title-4"></a> <a id="not-in-java-context"></a>Java にないコンテキスト キーワード

C# のいくつかのキーワードはコンテキスト キーワードになっていて、特定の文脈でのみキーワード扱いされます。

<table summary="">

	<tr>
		<th>キーワード</th>
		<th>説明</th>
		<th>関連記事</th>
	</tr>
	<tr>
		<td markdown="1"><code>add / remove</code></td>
		<td markdown="1">イベントのハンドラー追加/削除用のアクセサーを定義。</td>
		<td markdown="1">「[イベント](../functional/sp_event.md#event)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>dynamic</code></td>
		<td markdown="1">動的な変数を定義。</td>
		<td markdown="1">「[動的型付け変数](../dynamic/sp4_dynamic.md#dynamic)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>get / set / value</code></td>
		<td markdown="1">プロパティ/インデクサーのアクセサーを定義。</td>
		<td markdown="1">「[プロパティとは](../oop/oo_property.md#about)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>from / select /where</code>など</td>
		<td markdown="1">クエリ式。</td>
		<td markdown="1">「[クエリ式](../data/sp3_linq.md#query)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>partial</code></td>
		<td markdown="1">部分クラスを作る。</td>
		<td markdown="1">「[クラスの分割定義](../oop/oo_class.md#partial)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>var</code></td>
		<td markdown="1">変数を定義（型推論）。</td>
		<td markdown="1">「[型推論](../start/sp3_inference.md#implicit)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>where</code></td>
		<td markdown="1">ジェネリクスの型制約を付ける。</td>
		<td markdown="1">「[制約条件](../oop/sp2_generics.md#where)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>yield</code></td>
		<td markdown="1">イテレーター ブロック中で値を返す。</td>
		<td markdown="1">「[イテレーター ブロック](../data/sp2_iterator.md#block)」</td>
	</tr>
</table>



## <a id="sec-generated-title-5"></a> <a id="operator"></a>演算子

<table summary="">

	<tr>
		<th>Java</th>
		<th>C#</th>
		<th>説明</th>
		<th>関連記事</th>
	</tr>
	<tr>
		<td markdown="1"><code>x.y</code></td>
		<td markdown="1"><code>x.y</code></td>
		<td markdown="1">メンバー参照。</td>
		<td markdown="1">「[クラスの利用](../oop/oo_class.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>f(x)</code></td>
		<td markdown="1"><code>f(x)</code></td>
		<td markdown="1">メソッド呼び出し。</td>
		<td markdown="1">「[関数](../structured/st_function.md#function)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>a[x]</code></td>
		<td markdown="1"><code>a[x]</code></td>
		<td markdown="1">配列の要素参照。</td>
		<td markdown="1">「[配列](../structured/st_array.md#array)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>++, --</code></td>
		<td markdown="1"><code>++, --</code></td>
		<td markdown="1">インクリメント/デクリメント。</td>
		<td markdown="1">「[インクリメント・デクリメント](../start/st_operator.md#inc)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>new</code></td>
		<td markdown="1"><code>new</code></td>
		<td markdown="1">インスタンス生成。</td>
		<td markdown="1">「[クラスの利用](../oop/oo_class.md#use)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>instanceof</code></td>
		<td markdown="1"><code>is</code></td>
		<td markdown="1">インスタンスの型を調べる。</td>
		<td markdown="1">「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>+, -</code></td>
		<td markdown="1"><code>+, -</code></td>
		<td markdown="1">（単項）数値の符号反転 /（2項）数値の加減算 / 文字列の連結。</td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>+</code></td>
		<td markdown="1"><code>+</code></td>
		<td markdown="1">文字列の連結。</td>
		<td markdown="1">「[文字列連結](../start/st_operator.md#concat)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>!</code></td>
		<td markdown="1"><code>!</code></td>
		<td markdown="1">論理否定。</td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&amp;&amp;, ||</code></td>
		<td markdown="1"><code>&amp;&amp;, ||</code></td>
		<td markdown="1">論理積 / 論理和（短絡評価版）。</td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&amp;, |, ^</code></td>
		<td markdown="1"><code>&amp;, |, ^</code></td>
		<td markdown="1">論理積 / 論理和 / 排他的論理和。</td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>~</code></td>
		<td markdown="1"><code>~</code></td>
		<td markdown="1">補数（ビット反転）。</td>
		<td markdown="1">「[論理演算子](../start/st_operator.md#logical)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&lt;&lt;, &gt;&gt;</code></td>
		<td markdown="1"><code>&lt;&lt;, &gt;&gt;</code></td>
		<td markdown="1">左右シフト。</td>
		<td markdown="1">「[シフト](../start/st_operator.md#shift)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&gt;&gt;&gt;</code></td>
		<td markdown="1">なし</td>
		<td markdown="1">C# では、符号付き整数の右シフト（<code>&gt;&gt;</code>）は算術シフトに、 符号なし整数の右シフトは論理シフトになります。</td>
		<td markdown="1">「[シフト](../start/st_operator.md#shift)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>*, /, %</code></td>
		<td markdown="1"><code>*, /, %</code></td>
		<td markdown="1">数値の乗除算、剰余。</td>
		<td markdown="1">「[算術演算子](../start/st_operator.md#arithmetic)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>==, !=</code></td>
		<td markdown="1"><code>==, !=</code></td>
		<td markdown="1">等値比較。</td>
		<td markdown="1">「[関係演算](../start/st_operator.md#relation)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>&lt;, &lt;=, &gt;, &gt;=</code></td>
		<td markdown="1"><code>&lt;, &lt;=, &gt;, &gt;=</code></td>
		<td markdown="1">大小比較。</td>
		<td markdown="1">「[関係演算](../start/st_operator.md#relation)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>x ? y : z</code></td>
		<td markdown="1"><code>x ? y : z</code></td>
		<td markdown="1">条件演算子。</td>
		<td markdown="1">「[条件演算子](../start/st_operator.md#condition)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>=</code></td>
		<td markdown="1"><code>=</code></td>
		<td markdown="1">代入。</td>
		<td markdown="1">「[代入演算](../start/st_operator.md#substitute)」</td>
	</tr>
</table>



### <a id="sec-generated-title-6"></a> <a id="not-in-java-operator"></a>Java にない演算子

<table summary="">

	<tr>
		<th>演算子</th>
		<th>説明</th>
		<th>関連記事</th>
	</tr>
	<tr>
		<td markdown="1"><code>??</code></td>
		<td markdown="1">値が null の時に別の値に置き換える。</td>
		<td markdown="1">「[null 合体演算子](../start/st_operator.md#null)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>=&gt;</code></td>
		<td markdown="1">ラムダ式を作る。</td>
		<td markdown="1">「[ラムダ式](../functional/sp3_lambda.md#lambda)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>as</code></td>
		<td markdown="1">例外を投げる代わりに null を返す型変換。</td>
		<td markdown="1">「[ダウンキャスト](../oop/oo_polymorphism.md#downcast)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>checked / unchecked</code></td>
		<td markdown="1">整数演算のオーバーフロー時に例外を発生させるかどうかを選択する。</td>
		<td markdown="1">「[checked キーワード](../start/sp_checked.md#checked)」「[unchecked キーワード](../start/sp_checked.md#unchecked)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>default(T)</code></td>
		<td markdown="1">T 型の規定値（値型なら 0 や false、参照型なら null）を返す。 ジェネリクスで利用。</td>
		<td markdown="1">「[既定値](../oop/sp2_generics.md#default)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>sizeof(T)</code></td>
		<td markdown="1">T 型のサイズを返す。</td>
		<td markdown="1">「[unsafe コード限定機能](../interop/sp_unsafe.md#function)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>stackalloc</code></td>
		<td markdown="1">スタック上に配列を確保する。</td>
		<td markdown="1">「[unsafe コード限定機能](../interop/sp_unsafe.md#function)」</td>
	</tr>
	<tr>
		<td markdown="1"><code>typeof(T)</code></td>
		<td markdown="1">T 型の型情報を取得。</td>
		<td markdown="1">「[実行時型情報の取得](../dynamic/sp_reflection.md#type)」</td>
	</tr>
</table>



## <a id="sec-generated-title-7"></a> <a id="type"></a>基礎的な型

参考: 「[組込み型](../start/st_embeddedtype.md)」

<table summary="">

	<tr>
		<th colspan="2">種類</th>
		<th>Java</th>
		<th>C#</th>
		<th>注釈</th>
	</tr>
	<tr>
		<th colspan="2">論理型</th>
		<td markdown="1">boolean</td>
		<td markdown="1">bool</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<th rowspan="4">符号付き<br></br>整数</th>
		<th>1byte</th>
		<td markdown="1">byte</td>
		<td markdown="1">sbyte</td>
		<td markdown="1" rowspan="8">Java の整数型は符号付きのみ。</td>
	</tr>
	<tr>
		<th>2byte</th>
		<td markdown="1">short</td>
		<td markdown="1">short</td>
	</tr>
	<tr>
		<th>4byte</th>
		<td markdown="1">int</td>
		<td markdown="1">int</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1">long</td>
		<td markdown="1">long</td>
	</tr>
	<tr>
		<th rowspan="4">符号なし<br></br>整数</th>
		<th>1byte</th>
		<td markdown="1"></td>
		<td markdown="1">byte</td>
	</tr>
	<tr>
		<th>2byte</th>
		<td markdown="1"></td>
		<td markdown="1">ushort</td>
	</tr>
	<tr>
		<th>4byte</th>
		<td markdown="1"></td>
		<td markdown="1">uint</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1"></td>
		<td markdown="1">ulong</td>
	</tr>
	<tr>
		<th rowspan="2">浮動小数<br></br>点数</th>
		<th>4byte</th>
		<td markdown="1">float</td>
		<td markdown="1">float</td>
		<td markdown="1" rowspan="2">いずれも IEEE 754 形式</td>
	</tr>
	<tr>
		<th>8byte</th>
		<td markdown="1">double</td>
		<td markdown="1">double</td>
	</tr>
	<tr>
		<th colspan="2">デシマル</th>
		<td markdown="1">なし</td>
		<td markdown="1">decimal</td>
		<td markdown="1">Java の場合は BigDecimal クラスを利用。 C# の decimal は IEEE 754-2008 形式ではない。</td>
	</tr>
	<tr>
		<th colspan="2">文字</th>
		<td markdown="1">char</td>
		<td markdown="1">char</td>
		<td markdown="1">UTF-16</td>
	</tr>
	<tr>
		<th colspan="2">文字列</th>
		<td markdown="1">String</td>
		<td markdown="1">string</td>
		<td markdown="1">いずれも不変なオブジェクト。</td>
	</tr>
	<tr>
		<th colspan="2">オブジェクト型</th>
		<td markdown="1">Object</td>
		<td markdown="1">object</td>
		<td markdown="1"></td>
	</tr>
	<tr>
		<th colspan="2">他</th>
		<td markdown="1">Date</td>
		<td markdown="1">DateTime</td>
		<td markdown="1">扱える範囲が異なるので注意。 Java は1970年1月1日以降（上限は未定義）。 C# は1年1月1日から9999年12月31日まで。</td>
	</tr>
</table>
