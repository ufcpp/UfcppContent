---
title: "C# の型とメンバー"
source_url: "https://ufcpp.net/study/csharp/cheatsheet/list_type/"
content_type: "Article"
published_at: "2011-06-01T00:00:00"
updated_at: "2025-05-17T17:57:00"
tags: []
umbraco_id: 1176
parent_id: 1174
sort_order: 1
aliases:
  - "/csharp/cheatsheet/list_type/"
  - "/csharp/list_type"
  - "/csharp/list_type.html"
  - "/study/csharp/list_type"
  - "/study/csharp/list_type.html"
---

# C# の型とメンバー

##<a id="sec-generated-title-1"></a> <a id="abst"></a>概要
機能別索引＆概要。
C# で使える型とそのメンバーの一覧を先に示しておきます。


##<a id="sec-generated-title-2"></a> <a id="type"></a>型
大まかに分類すると以下の通り。

<table summary="C# の型の種類。">
	<caption>
		C# の型の種類。
	</caption>
	<tr>
		<td markdown="1"></td>
		<th>組み込み型</th>
		<th>ユーザー定義型</th>
		<th>他の型から合成する型</th>
	</tr>
	<tr>
		<th>値型</th>
		<td markdown="1">単純型</td>
		<td markdown="1">構造体<br></br>列挙型</td>
		<td markdown="1">Null 許容型</td>
	</tr>
	<tr>
		<th>参照型</th>
		<td markdown="1">文字列型<br></br>オブジェクト型</td>
		<td markdown="1">クラス<br></br>インターフェイス<br></br>デリゲート</td>
		<td markdown="1">配列</td>
	</tr>
	<tr>
		<th>その他</th>
		<td markdown="1"></td>
		<td markdown="1"></td>
		<td markdown="1">ポインター</td>
	</tr>
</table>



##### <a id="sec-generated-title-3"></a>縦軸分類
* 値型: 値をスタックに確保。代入時にはコピーが作られる。継承不可。

* 参照型: 値をヒープに確保。代入時には参照を渡すだけ。


参考: 「[値型と参照型](../resource/oo_reference.md)」「[[雑記] スタックとヒープ](../resource/misc_heap.md)」


##### <a id="sec-generated-title-4"></a>横軸分類
* 組み込み型: プログラミング言語にあらかじめ組み込まれている変数の型 →「[組込み型](../start/st_embeddedtype.md)」

* ユーザー定義型: プログラマーが自由に作ることができる型（後述）

* 他の型から合成する型: ある型<code>T</code>から<code>T[]</code>や<code>T?</code>などというような書き方で「合成」して作る型（後述）



###<a id="sec-generated-title-5"></a> <a id="user-defined"></a>ユーザー定義型
<table summary="C# のユーザー定義型一覧。">
	<caption>
		C# のユーザー定義型一覧。
	</caption>
	<tr>
		<th>型</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">クラス</td>
		<td markdown="1">いわゆる複合型。 最も基本的なユーザー定義型 →「[クラス](../oop/oo_class.md)」</td>
	</tr>
	<tr>
		<td markdown="1">構造体</td>
		<td markdown="1">同じく、複合型。 クラスとの違いは値型であること →「[値型と参照型](../resource/oo_reference.md)」</td>
	</tr>
	<tr>
		<td markdown="1">インターフェイス</td>
		<td markdown="1">抽象メンバーだけを持つ（要するに、型の規約（contract）だけを定める）型 →「[インターフェース](../oop/oo_interface.md)」</td>
	</tr>
	<tr>
		<td markdown="1">列挙型</td>
		<td markdown="1">特定の値だけを取ることができる型 →「[列挙型](../structured/st_enum.md)」</td>
	</tr>
	<tr>
		<td markdown="1">デリゲート</td>
		<td markdown="1">いわゆる fist-class 関数。 メソッドを参照するための型 →「[デリゲート](../functional/sp_delegate.md)」</td>
	</tr>
</table>


クラスとインターフェイスと構造体の3つは非常によく似ていて、いずれも次節「[クラス、インターフェイス、構造体のメンバー](#member)」で説明する「メンバー」を持てます。
違いは以下の通りです。

* クラスは参照型で、構造体は値型になります（参考:「[値型と参照型](../resource/oo_reference.md)」）。

* 構造体は継承（参考:「[継承](../oop/oo_inherit.md)」）ができません

* インターフェイスはデータ メンバーを持てず、関数メンバーも実装を持てません（全て abstract（参考:「[抽象メソッド、抽象クラス](../oop/oo_abstract.md)」）扱い）

* クラスは多重継承できませんが、インターフェイスなら複数継承（実装）することができます。



###<a id="sec-generated-title-6"></a> <a id="composed"></a>他の型から合成する型
<table summary="C# における他の型から合成する型一覧。">
	<caption>
		C# における他の型から合成する型一覧。
	</caption>
	<tr>
		<th>型</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">配列</td>
		<td markdown="1">複数のデータをひとまとめにして扱うための型 →「[配列](../structured/st_array.md)」</td>
	</tr>
	<tr>
		<td markdown="1">Null 許容型</td>
		<td markdown="1">値型でも null（無効な値）を認めたい場合に使います →「[Nullable 型](../resource/sp2_nullable.md)」</td>
	</tr>
	<tr>
		<td markdown="1">ポインター</td>
		<td markdown="1">C# では、基本的にポインター（メモリのアドレスを数値として扱う）を禁止していますが、 unsafe コンテキスト内でのみポインターを利用可能です →「[unsafe](../interop/sp_unsafe.md)」</td>
	</tr>
</table>



##<a id="sec-generated-title-7"></a> <a id="member"></a>クラス、インターフェイス、構造体のメンバー
##### <a id="sec-generated-title-8"></a>データ メンバー
データを表すメンバー。

<table summary="C# のクラスのデータ メンバー一覧">
	<caption>
		C# のクラスのデータ メンバー一覧
	</caption>
	<tr>
		<th>メンバー</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">フィールド</td>
		<td markdown="1">ユーザー定義型の内部に持つべきデータを格納するための変数 →「[データの構造化](../structured/st_struct.md)」「[クラス](../oop/oo_class.md)」</td>
	</tr>
	<tr>
		<td markdown="1">定数</td>
		<td markdown="1">ユーザー定義型に関連付けられた定数 →「[定数](../start/sp_const.md)」</td>
	</tr>
</table>


インターフェイスはデータ メンバーを持てません。


##### <a id="sec-generated-title-9"></a>関数メンバー
何らかの処理を行うメンバー。

<table summary="C# のクラスの関数メンバー一覧">
	<caption>
		C# のクラスの関数メンバー一覧
	</caption>
	<tr>
		<th>メンバー</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">メソッド</td>
		<td markdown="1">普通に関数らしい使い方をする関数メンバー →「[関数](../structured/st_function.md)」</td>
	</tr>
	<tr>
		<td markdown="1">コンストラクター</td>
		<td markdown="1">初期化処理 →「[コンストラクター](../oop/oo_construct.md#ctor)」</td>
	</tr>
	<tr>
		<td markdown="1">ファイナライザー</td>
		<td markdown="1">破棄処理 →「[ファイナライザー](../oop/oo_construct.md#dtor)」</td>
	</tr>
	<tr>
		<td markdown="1">プロパティ</td>
		<td markdown="1">いわゆるアクセサーを構文化したもの。 クラス外からはフィールド的に、 クラス内からはメソッド的に使える/作れるメンバー →「[プロパティ](../oop/oo_property.md)」</td>
	</tr>
	<tr>
		<td markdown="1">インデクサー</td>
		<td markdown="1">一種の「引数付きプロパティ」。 配列のように、インデックスを指定して要素を読み書きするためのメンバー →「[インデクサー](../oop/oo_indexer.md)」</td>
	</tr>
	<tr>
		<td markdown="1">イベント</td>
		<td markdown="1">いわゆるオブザーバー パターンを構文化したもの。 イベント ハンドラーの登録口を自動生成します →「[イベント](../functional/sp_event.md)」</td>
	</tr>
	<tr>
		<td markdown="1">演算子</td>
		<td markdown="1">ユーザー定義型向けに演算子をオーバーロードできます →「[演算子のオーバーロード](../oop/oo_operator.md)」</td>
	</tr>
</table>


このうち、ファイナライザーはクラスのみ、コンストラクターと演算子はクラスと構造体（インターフェイス以外）のみで定義できます。


##### <a id="sec-generated-title-10"></a>特殊なメソッド
メソッドに関しては、いくつか特殊な機能を持つものもあります。

<table summary="特殊なメソッド一覧">
	<caption>
		特殊なメソッド一覧
	</caption>
	<tr>
		<th>メソッド</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">イテレーター ブロック</td>
		<td markdown="1">データの列挙（IEnumerable の実装）を簡素化します →「[イテレーター](../data/sp2_iterator.md)」</td>
	</tr>
	<tr>
		<td markdown="1">拡張メソッド</td>
		<td markdown="1">既存の型に後からメソッドを追加（したかのように見せかけます）。静的メソッドをインスタンス メソッドと同様の構文で呼び出せるようにします →「[拡張メソッド](../functional/sp3_extension.md)」</td>
	</tr>
	<tr>
		<td markdown="1">部分メソッド</td>
		<td markdown="1">開発ツールとの連携用 →「[クラスの分割定義](../oop/oo_class.md#partial)」</td>
	</tr>
	<tr>
		<td markdown="1">非同期メソッド</td>
		<td markdown="1">非同期処理を簡素化します →「[非同期メソッド](../async/sp5_async.md#async)」</td>
	</tr>
</table>



###<a id="sec-generated-title-11"></a> <a id="accessibility"></a>メンバーのアクセス レベル
メンバーにアクセス可能な範囲を制御するためのアクセス修飾子。

<table summary="C# のアクセス修飾子一覧">
	<caption>
		C# のアクセス修飾子一覧
	</caption>
	<tr>
		<th>アクセス修飾子</th>
		<th>説明</th>
	</tr>
	<tr>
		<td markdown="1">public</td>
		<td markdown="1">どこからでもアクセス可能。</td>
	</tr>
	<tr>
		<td markdown="1">protected</td>
		<td markdown="1">その型、もしくは、派生クラス内からのみアクセス可能 （ファミリー スコープ（family scope））。</td>
	</tr>
	<tr>
		<td markdown="1">internal</td>
		<td markdown="1">アセンブリ内からのみアクセス可能 （アセンブリ スコープ（assembly scope））。</td>
	</tr>
	<tr>
		<td markdown="1">protected internal</td>
		<td markdown="1">protected または internal。 その型、派生クラス、もしくは、アセンブリ内からのみアクセス可能。</td>
	</tr>
	<tr>
		<td markdown="1">private protected</td>
		<td markdown="1">protected かつ internal。 その型、アセンブリ内にある派生クラスからのみアクセス可能。</td>
	</tr>
	<tr>
		<td markdown="1">private</td>
		<td markdown="1">その型の中からのみアクセス可能。</td>
	</tr>
</table>


クラスなどの型は、名前空間直下やグローバル スコープ中で定義できますが、
この場合、public もしくは internal のみが指定可能です。
この場合、アクセス修飾子を省略すると internal 扱いになります。

クラス内では全てのアクセス修飾子が利用可能です。
この場合、省略すると private 扱いになります。
