---
title: "重要語句一覧"
source_url: "https://ufcpp.net/study/csharp/appendix/keywords/"
content_type: "Article"
published_at: "2015-05-06T14:13:49"
updated_at: "2016-06-23T06:41:11"
tags: []
umbraco_id: 1381
parent_id: 1377
sort_order: 3
aliases:
  - "/csharp/appendix/keywords/"
  - "/csharp/keywords"
  - "/csharp/keywords.html"
  - "/study/csharp/keywords"
  - "/study/csharp/keywords.html"
---

# 重要語句一覧

<div><table>
<thead><tr><th>タイトル</th><th>キーワード</th></tr></thead>
<tbody>
<tr><td colspan="2"><span id="1170">まえがき</span></td></tr>
<tr><td colspan="2"><span id="1174">C# の機能一覧（索引的なもの）</span></td></tr>
<tr><th><a href="../cheatsheet/ap_ver3.md" id="1179">C# 3.0 の新機能</a></th><td><ul>
<li><a href="../cheatsheet/ap_ver3.md#partial_method">パーシャルメソッド</a></li>
</ul></td></tr>
<tr><th><a href="../cheatsheet/ap_ver6.md" id="1182">C# 6 の新機能</a></th><td><ul>
<li><a href="../cheatsheet/ap_ver6.md#key-null-conditional">null 条件演算子</a></li>
<li><a href="../cheatsheet/ap_ver6.md#key-nameof">nameof 演算子</a></li>
<li><a href="../cheatsheet/ap_ver6.md#key-index-initializer">インデックス初期化子</a></li>
</ul></td></tr>
<tr><th><a href="../cheatsheet/ap_ver8.md" id="2232">C# 8.0 の新機能</a></th><td><ul>
<li><a href="../cheatsheet/ap_ver8.md#key-static-local-function">静的ローカル関数</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1185">C# の概要</span></td></tr>
<tr><th><a href="../abstract/ab_csharp.md" id="1186">C# とは</a></th><td><ul>
<li><a href="../abstract/ab_csharp.md#cs">C#</a></li>
</ul></td></tr>
<tr><th><a href="../abstract/ab_dotnet.md" id="1187">.NET とは</a></th><td><ul>
<li><a href="../abstract/ab_dotnet.md#dotnet">.NET Framework</a></li>
<li><a href="../abstract/ab_dotnet.md#il">IL</a></li>
<li><a href="../abstract/ab_dotnet.md#cli">CLI</a></li>
<li><a href="../abstract/ab_dotnet.md#csharp">C#</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1709">C#開発環境</span></td></tr>
<tr><td colspan="2"><span id="1190">基礎</span></td></tr>
<tr><th><a href="../start/st_compile.md" id="1192">プログラムの作成・実行</a></th><td><ul>
<li><a href="../start/st_compile.md#source">ソースファイル</a></li>
<li><a href="../start/st_compile.md#exec">実行ファイル</a></li>
<li><a href="../start/st_compile.md#compile">コンパイル</a></li>
<li><a href="../start/st_compile.md#compiler">コンパイラ</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_comment.md" id="1194">コメント</a></th><td><ul>
<li><a href="../start/st_comment.md#comment">コメント</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_variable.md" id="1198">変数と式</a></th><td><ul>
<li><a href="../start/st_variable.md#type">型</a></li>
<li><a href="../start/st_variable.md#var-decl">変数宣言</a></li>
<li><a href="../start/st_variable.md#literal">リテラル</a></li>
<li><a href="../start/st_variable.md#var">var</a></li>
<li><a href="../start/st_variable.md#identifier">識別子</a></li>
<li><a href="../start/st_variable.md#expression">式</a></li>
<li><a href="../start/st_variable.md#operator">演算子</a></li>
<li><a href="../start/st_variable.md#statement">文</a></li>
<li><a href="../start/st_variable.md#block">ブロック</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_embeddedtype.md" id="1201">組込み型</a></th><td><ul>
<li><a href="../start/st_embeddedtype.md#key-escape">エスケープ シーケンス</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_string.md" id="1202">特殊な文字列リテラル</a></th><td><ul>
<li><a href="../start/st_string.md#key-interpolated-string">補間文字列</a></li>
<li><a href="../start/st_string.md#key-nameof">nameof 演算子</a></li>
<li><a href="../start/st_string.md#key-raw-string">生文字列リテラル</a></li>
<li><a href="../start/st_string.md#key-utf8-literal">UTF-8 リテラル</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_operator.md" id="1203">組込み演算子</a></th><td><ul>
<li><a href="../start/st_operator.md#operator">演算子</a></li>
<li><a href="../start/st_operator.md#operand">オペランド</a></li>
<li><a href="../start/st_operator.md#shortcircuit">短絡評価</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_cast.md" id="1209">組込み型変換</a></th><td><ul>
<li><a href="../start/st_cast.md#cast">キャスト</a></li>
</ul></td></tr>
<tr><th><a href="../start/sp_checked.md" id="1213">オーバーフローのチェック</a></th><td><ul>
<li><a href="../start/sp_checked.md#overflow">オーバーフロー</a></li>
</ul></td></tr>
<tr><th><a href="../start/sp_const.md" id="1214">定数</a></th><td><ul>
<li><a href="../start/sp_const.md#constant">定数</a></li>
<li><a href="../start/sp_const.md#ro">読取り専用</a></li>
</ul></td></tr>
<tr><th><a href="../start/sp3_inference.md" id="1215">型推論(暗黙的型付け)と匿名型</a></th><td><ul>
<li><a href="../start/sp3_inference.md#type-inference">型推論</a></li>
<li><a href="../start/sp3_inference.md#anonytype">匿名型</a></li>
</ul></td></tr>
<tr><th><a href="../start/st_scope.md" id="1859">[雑記] 識別子のスコープとオブジェクトの寿命</a></th><td><ul>
<li><a href="../start/st_scope.md#identifier">識別子</a></li>
<li><a href="../start/st_scope.md#scope">スコープ</a></li>
</ul></td></tr>
<tr><th><a href="../start/misctyperesolution.md" id="2275">[雑記] 型の決定</a></th><td><ul>
<li><a href="../start/misctyperesolution.md#source-type">ソース型</a></li>
<li><a href="../start/misctyperesolution.md#target-type">ターゲット型</a></li>
</ul></td></tr>
<tr><th><a href="../start/miscreservedattribute.md" id="2361">[雑記] コンパイル結果に影響を及ぼす属性</a></th><td><ul>
<li><a href="../start/miscreservedattribute.md#key-reserved-attribute">予約属性</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1217">構造化</span></td></tr>
<tr><th><a href="../structured/st_branch.md" id="1220">条件分岐</a></th><td><ul>
<li><a href="../structured/st_branch.md#branch">条件分岐</a></li>
<li><a href="../structured/st_branch.md#if">if</a></li>
<li><a href="../structured/st_branch.md#switch">switch</a></li>
<li><a href="../structured/st_branch.md#goto">goto</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_loop.md" id="1225">反復処理</a></th><td><ul>
<li><a href="../structured/st_loop.md#while">while</a></li>
<li><a href="../structured/st_loop.md#do">do-while</a></li>
<li><a href="../structured/st_loop.md#for">for</a></li>
<li><a href="../structured/st_loop.md#foreach">foreach</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_array.md" id="1229">配列</a></th><td><ul>
<li><a href="../structured/st_array.md#array">配列</a></li>
<li><a href="../structured/st_array.md#key-initializer">初期化子</a></li>
<li><a href="../structured/st_array.md#multid">多次元配列</a></li>
<li><a href="../structured/st_array.md#jugged">配列の配列</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_function.md" id="1233">関数</a></th><td><ul>
<li><a href="../structured/st_function.md#function">関数</a></li>
<li><a href="../structured/st_function.md#function-member">関数メンバー</a></li>
<li><a href="../structured/st_function.md#method">メソッド</a></li>
<li><a href="../structured/st_function.md#return">戻り値</a></li>
<li><a href="../structured/st_function.md#paramter">引数</a></li>
<li><a href="../structured/st_function.md#return-value">戻り値</a></li>
<li><a href="../structured/st_function.md#actual-parameter">実引数</a></li>
<li><a href="../structured/st_function.md#formal%20parameter">仮引数</a></li>
<li><a href="../structured/st_function.md#overload">オーバーロード</a></li>
<li><a href="../structured/st_function.md#key-signature">シグネチャ</a></li>
<li><a href="../structured/st_function.md#key-method-group">メソッド グループ</a></li>
<li><a href="../structured/st_function.md#expression-bodied">expression-bodied (本体が式の)関数</a></li>
<li><a href="../structured/st_function.md#key-local">ローカル関数</a></li>
<li><a href="../structured/st_function.md#key-anonymous">匿名関数</a></li>
</ul></td></tr>
<tr><th><a href="../structured/miscentrypoint.md" id="2072">[雑記] エントリーポイント</a></th><td><ul>
<li><a href="../structured/miscentrypoint.md#entry-point">エントリーポイント</a></li>
</ul></td></tr>
<tr><th><a href="../structured/miscinlining.md" id="2110">[雑記] インライン化</a></th><td><ul>
<li><a href="../structured/miscinlining.md#key-inlining">インライン化</a></li>
</ul></td></tr>
<tr><th><a href="../structured/miscoverloadresolution.md" id="2147">[雑記] オーバーロード解決</a></th><td><ul>
<li><a href="../structured/miscoverloadresolution.md#overload-resolution">オーバーロード解決</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_library.md" id="1240">ライブラリ</a></th><td><ul>
<li><a href="../structured/st_library.md#library">ライブラリ</a></li>
<li><a href="../structured/st_library.md#stdlib">標準ライブラリ</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_enum.md" id="1241">列挙型</a></th><td><ul>
<li><a href="../structured/st_enum.md#enum">列挙型</a></li>
</ul></td></tr>
<tr><th><a href="../structured/st_struct.md" id="1242">データの構造化(複合型)</a></th><td><ul>
<li><a href="../structured/st_struct.md#class">クラス</a></li>
<li><a href="../structured/st_struct.md#struct">構造体</a></li>
<li><a href="../structured/st_struct.md#field">フィールド</a></li>
</ul></td></tr>
<tr><th><a href="../structured/sp_namespace.md" id="1244">名前空間</a></th><td><ul>
<li><a href="../structured/sp_namespace.md#namespace">名前空間</a></li>
<li><a href="../structured/sp_namespace.md#global-namespace">グローバル名前空間</a></li>
<li><a href="../structured/sp_namespace.md#key-file-scoped-namespace">ファイル スコープ名前空間</a></li>
<li><a href="../structured/sp_namespace.md#using">using ディレクティブ</a></li>
<li><a href="../structured/sp_namespace.md#key-global-using">global using ディレクティブ</a></li>
<li><a href="../structured/sp_namespace.md#alias">エイリアス</a></li>
</ul></td></tr>
<tr><th><a href="../structured/oo_exception.md" id="1245">例外処理</a></th><td><ul>
<li><a href="../structured/oo_exception.md#exc">例外</a></li>
<li><a href="../structured/oo_exception.md#exchandle">例外処理</a></li>
<li><a href="../structured/oo_exception.md#throw">throw 文</a></li>
<li><a href="../structured/oo_exception.md#try">try-catch-finally 文</a></li>
<li><a href="../structured/oo_exception.md#key-exception-filter">例外フィルター</a></li>
</ul></td></tr>
<tr><th><a href="../structured/misc_exception.md" id="1246">[雑記] 例外の使い方</a></th><td><ul>
<li><a href="../structured/misc_exception.md#tester_doer">Tester-Doer パターン</a></li>
<li><a href="../structured/misc_exception.md#tryparse">Try Parse パターン</a></li>
</ul></td></tr>
<tr><th><a href="../structured/misc_stacktrace.md" id="1247">[雑記] 例外のスタックトレース</a></th><td><ul>
<li><a href="../structured/misc_stacktrace.md#stacktrace">スタックトレース</a></li>
</ul></td></tr>
<tr><th><a href="../structured/miscexpressions.md" id="1962">[雑記] 式にまつわる補足</a></th><td><ul>
<li><a href="../structured/miscexpressions.md#key-expression">式</a></li>
<li><a href="../structured/miscexpressions.md#key-statement">ステートメント</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1248">オブジェクト指向</span></td></tr>
<tr><th><a href="../oop/oo_about.md" id="1249">オブジェクト指向とは</a></th><td><ul>
<li><a href="../oop/oo_about.md#proc">手続き</a></li>
<li><a href="../oop/oo_about.md#struct">データ構造</a></li>
<li><a href="../oop/oo_about.md#object">オブジェクト</a></li>
<li><a href="../oop/oo_about.md#oo">オブジェクト指向</a></li>
<li><a href="../oop/oo_about.md#class">クラス</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_class.md" id="1250">クラス</a></th><td><ul>
<li><a href="../oop/oo_class.md#class">クラス</a></li>
<li><a href="../oop/oo_class.md#instance">インスタンス</a></li>
<li><a href="../oop/oo_class.md#null">null</a></li>
<li><a href="../oop/oo_class.md#anonytype">匿名型</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_construct.md" id="1252">コンストラクター</a></th><td><ul>
<li><a href="../oop/oo_construct.md#key-primary-constructor">プライマリ コンストラクター</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_conceal.md" id="1254">実装の隠蔽</a></th><td><ul>
<li><a href="../oop/oo_conceal.md#level">アクセシビリティ</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_property.md" id="1255">プロパティ</a></th><td><ul>
<li><a href="../oop/oo_property.md#property">プロパティ</a></li>
<li><a href="../oop/oo_property.md#accessor">アクセサー</a></li>
<li><a href="../oop/oo_property.md#setter">setter</a></li>
<li><a href="../oop/oo_property.md#getter">getter</a></li>
<li><a href="../oop/oo_property.md#auto_prop">自動プロパティ</a></li>
<li><a href="../oop/oo_property.md#key-get-only">get-only プロパティ</a></li>
<li><a href="../oop/oo_property.md#key-required">required メンバー</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_static.md" id="1257">静的メンバー</a></th><td><ul>
<li><a href="../oop/oo_static.md#static-member">静的メンバー</a></li>
<li><a href="../oop/oo_static.md#stfield">静的フィールド</a></li>
<li><a href="../oop/oo_static.md#stmethod">静的メソッド</a></li>
<li><a href="../oop/oo_static.md#stconst">静的コンストラクター</a></li>
<li><a href="../oop/oo_static.md#stclass">静的クラス</a></li>
<li><a href="../oop/oo_static.md#key-using-static">using static</a></li>
</ul></td></tr>
<tr><th><a href="../oop/moduleinitializer.md" id="2329">モジュール初期化子</a></th><td><ul>
<li><a href="../oop/moduleinitializer.md#key-module-initializer">モジュール初期化子</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_operator.md" id="1259">演算子のオーバーロード</a></th><td><ul>
<li><a href="../oop/oo_operator.md#udt">ユーザー定義型</a></li>
<li><a href="../oop/oo_operator.md#opoverload">演算子のオーバーロード</a></li>
<li><a href="../oop/oo_operator.md#udo">ユーザ定義演算子</a></li>
<li><a href="../oop/oo_operator.md#cast">型変換</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_indexer.md" id="1261">インデクサー</a></th><td><ul>
<li><a href="../oop/oo_indexer.md#indexer">インデクサー</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_inherit.md" id="1262">継承</a></th><td><ul>
<li><a href="../oop/oo_inherit.md#derive">継承</a></li>
<li><a href="../oop/oo_inherit.md#supclass">基底クラス</a></li>
<li><a href="../oop/oo_inherit.md#subclass">派生クラス</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_polymorphism.md" id="1263">多態性</a></th><td><ul>
<li><a href="../oop/oo_polymorphism.md#statictype">静的な型</a></li>
<li><a href="../oop/oo_polymorphism.md#dynamictype">動的な型</a></li>
<li><a href="../oop/oo_polymorphism.md#typeof">typeof 演算子</a></li>
<li><a href="../oop/oo_polymorphism.md#upcast">アップキャスト</a></li>
<li><a href="../oop/oo_polymorphism.md#downcast">ダウンキャスト</a></li>
<li><a href="../oop/oo_polymorphism.md#is-operator">is 演算子</a></li>
<li><a href="../oop/oo_polymorphism.md#as-operator">as 演算子</a></li>
<li><a href="../oop/oo_polymorphism.md#virtual_method">仮想メソッド</a></li>
<li><a href="../oop/oo_polymorphism.md#override">オーバーライド</a></li>
<li><a href="../oop/oo_polymorphism.md#polymorphism">多態性</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_abstract.md" id="1267">抽象メソッド、抽象クラス</a></th><td><ul>
<li><a href="../oop/oo_abstract.md#abclass">抽象クラス</a></li>
<li><a href="../oop/oo_abstract.md#abmethod">抽象メソッド</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_interface.md" id="1269">インターフェース</a></th><td><ul>
<li><a href="../oop/oo_interface.md#contract">規約</a></li>
<li><a href="../oop/oo_interface.md#implementation">実装</a></li>
<li><a href="../oop/oo_interface.md#interface">インターフェース</a></li>
<li><a href="../oop/oo_interface.md#explicit-interface-method">インターフェイスの明示的実装</a></li>
</ul></td></tr>
<tr><th><a href="../oop/oo_vftable.md" id="1272">[雑記] 仮想関数テーブル</a></th><td><ul>
<li><a href="../oop/oo_vftable.md#vftable">仮想関数テーブル</a></li>
</ul></td></tr>
<tr><th><a href="../oop/sp2_generics.md" id="1273">ジェネリック</a></th><td><ul>
<li><a href="../oop/sp2_generics.md#generics">ジェネリック</a></li>
<li><a href="../oop/sp2_generics.md#typeparam">型引数</a></li>
</ul></td></tr>
<tr><th><a href="../oop/sp4_variance.md" id="1274">ジェネリクスの共変性・反変性</a></th><td><ul>
<li><a href="../oop/sp4_variance.md#covariance">共変性</a></li>
<li><a href="../oop/sp4_variance.md#contravariance">反変性</a></li>
<li><a href="../oop/sp4_variance.md#variance-annotation">変性注釈</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1940">データ型</span></td></tr>
<tr><th><a href="../datatype/tuples.md" id="1941">タプル</a></th><td><ul>
<li><a href="../datatype/tuples.md#key-tuple">タプル</a></li>
</ul></td></tr>
<tr><th><a href="../datatype/patterns.md" id="2176">パターン マッチング</a></th><td><ul>
<li><a href="../datatype/patterns.md#key-list-pattern">リスト パターン</a></li>
<li><a href="../datatype/patterns.md#key-slice-pattern">スライス パターン</a></li>
</ul></td></tr>
<tr><th><a href="../datatype/declarationexpressions.md" id="2009">特殊な変数宣言</a></th><td><ul>
<li><a href="../datatype/declarationexpressions.md#key-declaration-expression">変数宣言式</a></li>
<li><a href="../datatype/declarationexpressions.md#discard">破棄</a></li>
</ul></td></tr>
<tr><th><a href="../datatype/record.md" id="2348">レコード型</a></th><td><ul>
<li><a href="../datatype/record.md#key-record">レコード型</a></li>
<li><a href="../datatype/record.md#key-primary-constructor">プライマリ コンストラクター</a></li>
</ul></td></tr>
<tr><th><a href="../datatype/collection-expression.md" id="2475">コレクション式</a></th><td><ul>
<li><a href="../datatype/collection-expression.md#key-collection-expr">コレクション式</a></li>
<li><a href="../datatype/collection-expression.md#key-spread">スプレッド</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1275">関数指向</span></td></tr>
<tr><th><a href="../functional/sp_delegate.md" id="1277">デリゲート</a></th><td><ul>
<li><a href="../functional/sp_delegate.md#delegate">デリゲート</a></li>
<li><a href="../functional/sp_delegate.md#malticast">マルチキャストデリゲート</a></li>
<li><a href="../functional/sp_delegate.md#asynchronous">非同期呼び出し</a></li>
<li><a href="../functional/sp_delegate.md#anonymous-func">匿名関数</a></li>
<li><a href="../functional/sp_delegate.md#anonymous">匿名メソッド式</a></li>
<li><a href="../functional/sp_delegate.md#lambda">ラムダ式</a></li>
<li><a href="../functional/sp_delegate.md#covariance">covariance</a></li>
<li><a href="../functional/sp_delegate.md#contravariance">contravariance</a></li>
</ul></td></tr>
<tr><th><a href="../functional/fun_localfunctions.md" id="1929">ローカル関数と匿名関数</a></th><td><ul>
<li><a href="../functional/fun_localfunctions.md#key-local">ローカル関数</a></li>
<li><a href="../functional/fun_localfunctions.md#key-anonymous">匿名関数</a></li>
<li><a href="../functional/fun_localfunctions.md#key-lambda">ラムダ式</a></li>
<li><a href="../functional/fun_localfunctions.md#closure">クロージャ</a></li>
<li><a href="../functional/fun_localfunctions.md#key-static-local-function">静的ローカル関数</a></li>
<li><a href="../functional/fun_localfunctions.md#key-shadowing">シャドーイング</a></li>
</ul></td></tr>
<tr><th><a href="../functional/sp3_lambda.md" id="1280">ラムダ式</a></th><td><ul>
<li><a href="../functional/sp3_lambda.md#lambda">ラムダ式</a></li>
<li><a href="../functional/sp3_lambda.md#exp_tree">式木</a></li>
<li><a href="../functional/sp3_lambda.md#objectinit">オブジェクト初期化子</a></li>
<li><a href="../functional/sp3_lambda.md#collectioninit">コレクション初期化子</a></li>
<li><a href="../functional/sp3_lambda.md#key-index-initializer">インデックス初期化子</a></li>
</ul></td></tr>
<tr><th><a href="../functional/sp_event.md" id="1281">イベント</a></th><td><ul>
<li><a href="../functional/sp_event.md#event">イベント</a></li>
<li><a href="../functional/sp_event.md#eventhandler">イベント ハンドラー</a></li>
<li><a href="../functional/sp_event.md#edriven">イベント駆動型</a></li>
</ul></td></tr>
<tr><th><a href="../functional/sp3_extension.md" id="1284">拡張メソッド</a></th><td><ul>
<li><a href="../functional/sp3_extension.md#exmethod">拡張メソッド</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1298">データ列処理</span></td></tr>
<tr><th><a href="../data/sp_foreach.md" id="1299">foreach</a></th><td><ul>
<li><a href="../data/sp_foreach.md#foreach">foreach 文</a></li>
</ul></td></tr>
<tr><th><a href="../data/sp2_iterator.md" id="1300">イテレーター</a></th><td><ul>
<li><a href="../data/sp2_iterator.md#iterator">イテレーター</a></li>
</ul></td></tr>
<tr><th><a href="../data/sp3_linq.md" id="1303">LINQ</a></th><td><ul>
<li><a href="../data/sp3_linq.md#linq">LINQ</a></li>
<li><a href="../data/sp3_linq.md#std_query_op">標準クエリ演算子</a></li>
<li><a href="../data/sp3_linq.md#query">クエリ式</a></li>
</ul></td></tr>
<tr><th><a href="../data/sp3_stdquery.md" id="1304">標準クエリ演算子（クエリ式関係）</a></th><td><ul>
<li><a href="../data/sp3_stdquery.md#from">from 句</a></li>
<li><a href="../data/sp3_stdquery.md#select">select 句</a></li>
<li><a href="../data/sp3_stdquery.md#let">let 句</a></li>
<li><a href="../data/sp3_stdquery.md#transparent">透過識別子</a></li>
<li><a href="../data/sp3_stdquery.md#where">where 句</a></li>
<li><a href="../data/sp3_stdquery.md#join">join 句</a></li>
<li><a href="../data/sp3_stdquery.md#orderby">orderby 句</a></li>
<li><a href="../data/sp3_stdquery.md#groupby">group ... by 句</a></li>
</ul></td></tr>
<tr><th><a href="../data/sp3_lazylist.md" id="1306">[雑記] LINQ と遅延評価</a></th><td><ul>
<li><a href="../data/sp3_lazylist.md#delayed">遅延評価</a></li>
</ul></td></tr>
<tr><th><a href="../data/sp3_ormismatch.md" id="1307">[雑記] O/R インピーダンスミスマッチ</a></th><td><ul>
<li><a href="../data/sp3_ormismatch.md#or_mismatch">O/R インピーダンス ミスマッチ</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1286">メモリとリソース管理</span></td></tr>
<tr><th><a href="../resource/rm_gc.md" id="1287">C# のメモリ管理</a></th><td><ul>
<li><a href="../resource/rm_gc.md#garbage-collection">ガベージ コレクション</a></li>
<li><a href="../resource/rm_gc.md#finalizer">ファイナライザー</a></li>
<li><a href="../resource/rm_gc.md#finalize">ファイナライズ</a></li>
</ul></td></tr>
<tr><th><a href="../resource/oo_reference.md" id="1288">値型と参照型</a></th><td><ul>
<li><a href="../resource/oo_reference.md#valtype">値型</a></li>
<li><a href="../resource/oo_reference.md#reftype">参照型</a></li>
</ul></td></tr>
<tr><th><a href="../resource/readonlyness.md" id="2095">readonly の注意点</a></th><td><ul>
<li><a href="../resource/readonlyness.md#hidden-copy">隠れたコピー</a></li>
</ul></td></tr>
<tr><th><a href="../resource/rm_default.md" id="1289">既定値</a></th><td><ul>
<li><a href="../resource/rm_default.md#default-value">既定値</a></li>
</ul></td></tr>
<tr><th><a href="../resource/sp_ref.md" id="1290">参照渡し</a></th><td><ul>
<li><a href="../resource/sp_ref.md#byval">値渡し</a></li>
<li><a href="../resource/sp_ref.md#byref">参照渡し</a></li>
<li><a href="../resource/sp_ref.md#key-escape-analysis">エスケープ解析</a></li>
</ul></td></tr>
<tr><th><a href="../resource/refstruct.md" id="2107">ref構造体</a></th><td><ul>
<li><a href="../resource/refstruct.md#key-refstruct">`ref`構造体</a></li>
<li><a href="../resource/refstruct.md#key-ref-field">ref フィールド</a></li>
<li><a href="../resource/refstruct.md#key-escape-analysis">エスケープ解析</a></li>
</ul></td></tr>
<tr><th><a href="../resource/rmboxing.md" id="1292">ボックス化</a></th><td><ul>
<li><a href="../resource/rmboxing.md#key-boxing">ボックス化</a></li>
</ul></td></tr>
<tr><th><a href="../resource/sp2_nullable.md" id="1293">null許容値型(Nullable&amp;lt;T&amp;gt; 型)</a></th><td><ul>
<li><a href="../resource/sp2_nullable.md#nullableType">null 許容型</a></li>
</ul></td></tr>
<tr><th><a href="../resource/nullablereferencetype.md" id="2255">null 許容参照型</a></th><td><ul>
<li><a href="../resource/nullablereferencetype.md#key-nrt">null許容参照型</a></li>
<li><a href="../resource/nullablereferencetype.md#nullable-context">null 許容コンテキスト</a></li>
</ul></td></tr>
<tr><th><a href="../resource/rm_nullusage.md" id="1294">null の取り扱い</a></th><td><ul>
<li><a href="../resource/rm_nullusage.md#key-null-conditional">null条件演算子</a></li>
<li><a href="../resource/rm_nullusage.md#key-null-coalesce">null合体演算子</a></li>
</ul></td></tr>
<tr><th><a href="../resource/oo_dispose.md" id="1295">リソースの破棄</a></th><td><ul>
<li><a href="../resource/oo_dispose.md#using">using ステートメント</a></li>
</ul></td></tr>
<tr><th><a href="../resource/rmweakreference.md" id="1297">【雑記】弱参照</a></th><td><ul>
<li><a href="../resource/rmweakreference.md#key-weak-reference">弱参照</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1312">動的な処理</span></td></tr>
<tr><th><a href="../dynamic/sp_reflection.md" id="1313">実行時型情報</a></th><td><ul>
<li><a href="../dynamic/sp_reflection.md#metadata">メタデータ</a></li>
<li><a href="../dynamic/sp_reflection.md#reflection">リフレクション</a></li>
<li><a href="../dynamic/sp_reflection.md#rtti">実行時型情報</a></li>
</ul></td></tr>
<tr><th><a href="../dynamic/sp_attribute.md" id="1314">属性</a></th><td><ul>
<li><a href="../dynamic/sp_attribute.md#attribute">属性</a></li>
</ul></td></tr>
<tr><th><a href="../dynamic/sp3_expression.md" id="1315">式木（Expression Trees）</a></th><td><ul>
<li><a href="../dynamic/sp3_expression.md#expressiontree">式木</a></li>
</ul></td></tr>
<tr><th><a href="../dynamic/sp4_multipledispatch.md" id="1320">[雑記] 多重ディスパッチ</a></th><td><ul>
<li><a href="../dynamic/sp4_multipledispatch.md#dispatch">ディスパッチ</a></li>
<li><a href="../dynamic/sp4_multipledispatch.md#dynamic">動的ディスパッチ</a></li>
<li><a href="../dynamic/sp4_multipledispatch.md#multiple">多重ディスパッチ</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1321">相互運用</span></td></tr>
<tr><th><a href="../interop/sp_unsafe.md" id="1322">unsafe</a></th><td><ul>
<li><a href="../interop/sp_unsafe.md#unsafe">unsafe</a></li>
<li><a href="../interop/sp_unsafe.md#cppcli">C++/CLI</a></li>
</ul></td></tr>
<tr><th><a href="../interop/sp_pinvoke.md" id="1324">プラットフォーム呼び出し</a></th><td><ul>
<li><a href="../interop/sp_pinvoke.md#pinvoke">P/Invoke</a></li>
<li><a href="../interop/sp_pinvoke.md#extern-modifier">`extern`修飾子</a></li>
<li><a href="../interop/sp_pinvoke.md#key-marshaling">マーシャリング</a></li>
<li><a href="../interop/sp_pinvoke.md#rcw">RCW</a></li>
<li><a href="../interop/sp_pinvoke.md#ccw">CCW</a></li>
</ul></td></tr>
<tr><th><a href="../interop/memorylayout.md" id="1915">複合型のレイアウト</a></th><td><ul>
<li><a href="../interop/memorylayout.md#key-alignment">アラインメント</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1326">非同期処理</span></td></tr>
<tr><th><a href="../async/sp_thread.md" id="1327">マルチスレッド</a></th><td><ul>
<li><a href="../async/sp_thread.md#thread">スレッド</a></li>
<li><a href="../async/sp_thread.md#single">シングルスレッド</a></li>
<li><a href="../async/sp_thread.md#multi">マルチスレッド</a></li>
<li><a href="../async/sp_thread.md#exclusive">排他制御</a></li>
<li><a href="../async/sp_thread.md#cs">クリティカルセクション</a></li>
<li><a href="../async/sp_thread.md#lock">ロック</a></li>
</ul></td></tr>
<tr><th><a href="../async/misc_task.md" id="1332">[雑記] スレッド プールとタスク</a></th><td><ul>
<li><a href="../async/misc_task.md#key_task">タスク</a></li>
<li><a href="../async/misc_task.md#key_thread">スレッド</a></li>
<li><a href="../async/misc_task.md#key_thread_pool">スレッド プール</a></li>
</ul></td></tr>
<tr><th><a href="../async/misc_continuation.md" id="1336">[雑記] 継続と先物</a></th><td><ul>
<li><a href="../async/misc_continuation.md#key_future">先物</a></li>
<li><a href="../async/misc_continuation.md#key_continuation">継続</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1338">その他</span></td></tr>
<tr><th><a href="../misc/partial-type.md" id="2500">型の分割定義 (partial)</a></th><td><ul>
<li><a href="../misc/partial-type.md#partial_class">部分クラス</a></li>
<li><a href="../misc/partial-type.md#partial_method">部分メソッド</a></li>
</ul></td></tr>
<tr><th><a href="../misc/sp_preprocess.md" id="1339">プリプロセス</a></th><td><ul>
<li><a href="../misc/sp_preprocess.md#preprocess">プリプロセス命令</a></li>
</ul></td></tr>
<tr><th><a href="../misc/sp_xmldoc.md" id="1340">XML Document</a></th><td><ul>
<li><a href="../misc/sp_xmldoc.md#doccomment">ドキュメンテーションコメント</a></li>
</ul></td></tr>
<tr><th><a href="../misc/misc_roslyn.md" id="1343">[雑記] .NET Compiler Platform</a></th><td><ul>
<li><a href="../misc/misc_roslyn.md#compiler-platform">.NET Compiler Platform</a></li>
</ul></td></tr>
<tr><th><a href="../misc/miscpatternbased.md" id="2249">パターン ベースな構文</a></th><td><ul>
<li><a href="../misc/miscpatternbased.md#key-pattern-based">パターン ベース</a></li>
</ul></td></tr>
<tr><th><a href="../misc/miscentrypoint.md" id="2301">エントリー ポイント</a></th><td><ul>
<li><a href="../misc/miscentrypoint.md#key-entry-point">エントリー ポイント</a></li>
<li><a href="../misc/miscentrypoint.md#key-top-level-statements">トップ レベル ステートメント</a></li>
</ul></td></tr>
<tr><th><a href="../misc/file-local.md" id="2431">file ローカル型</a></th><td><ul>
<li><a href="../misc/file-local.md#branch">file ローカル型</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1717">パッケージ管理</span></td></tr>
<tr><th><a href="../package/project.md" id="1726">プロジェクトの分割</a></th><td><ul>
<li><a href="../package/project.md#assembly">アセンブリ</a></li>
</ul></td></tr>
<tr><th><a href="../package/toplevelaccessibility.md" id="1772">トップ レベルのアクセシビリティ</a></th><td><ul>
<li><a href="../package/toplevelaccessibility.md#key-nested">入れ子</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1344">フレームワーク / 実行環境</span></td></tr>
<tr><th><a href="../framework/fwjitcompilation.md" id="1820">JITコンパイル</a></th><td><ul>
<li><a href="../framework/fwjitcompilation.md#jit">JIT</a></li>
<li><a href="../framework/fwjitcompilation.md#aot">AOT</a></li>
</ul></td></tr>
<tr><th><a href="../framework/fwreferenceassemblies.md" id="1347">参照アセンブリ</a></th><td><ul>
<li><a href="../framework/fwreferenceassemblies.md#profile">プロファイル</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1350">標準ライブラリ</span></td></tr>
<tr><td colspan="2"><span id="1359">サンプルプログラム</span></td></tr>
<tr><th><a href="../sample/sp3_comprehensions.md" id="1363">[サンプル] クエリ式とリスト内包</a></th><td><ul>
<li><a href="../sample/sp3_comprehensions.md#comprehension">リスト内包</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1372">他のプログラミング言語経験者向け</span></td></tr>
<tr><th><a href="../cs4j/ab_csspec.md" id="1374">C# の特徴（C++、Java 利用者向け）</a></th><td><ul>
<li><a href="../cs4j/ab_csspec.md#gc">ガーベジコレクション</a></li>
<li><a href="../cs4j/ab_csspec.md#interface">インターフェース</a></li>
<li><a href="../cs4j/ab_csspec.md#property">プロパティ</a></li>
<li><a href="../cs4j/ab_csspec.md#delegate">デリゲート</a></li>
<li><a href="../cs4j/ab_csspec.md#attribute">属性</a></li>
</ul></td></tr>
<tr><td colspan="2"><span id="1377">付録</span></td></tr>
<tr><th><a href="ap_term.md" id="1378">その他の用語</a></th><td><ul>
<li><a href="ap_term.md#ducktype">ダック タイピング</a></li>
</ul></td></tr>
</tbody>
</table>
</div>
