---
title: "XML,XSL"
source_url: "https://ufcpp.net/study/xml/"
content_type: "Subject"
published_at: "2015-05-06T14:23:51"
updated_at: "2015-07-07T18:46:57"
tags: []
umbraco_id: 1644
parent_id: 1115
sort_order: 13
aliases:
  - "/study/ref/"
  - "/study/ref/index.html"
  - "/study/testxsl"
  - "/study/testxsl/"
  - "/study/textxsl/"
  - "/study/textxsl/index.html"
---

# XML,XSL

当初、このサイトは XML で書いたものを XSLT で変換して HTML ファイルを生成していました。ここではその XML, XSL の説明と、実際の XSLT ファイルの内容を記載しています。

    
XSL の現物の一覧↓。

    
<table summary="XSL 一覧">
	<caption>
		XSL 一覧
	</caption>
<thead>	<tr>
		<th>XSL ファイル</th>
		<th>説明</th>
	</tr>
</thead><tbody>	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/main.xsl">main.xsl</a></td>
		<td markdown="1">XML から直接参照するのはこの XSL ファイル。 他の XSL ファイルのインクルードと、 パラメータの設定を行う。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/general.xsl">general.xsl</a></td>
		<td markdown="1">未定義タグはそのまま出力。 HTML のタグをそのまま使うため。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/document.xsl">document.xsl</a></td>
		<td markdown="1">document タグ（一般ページの root 要素）を定義。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/index.xsl">index.xsl</a></td>
		<td markdown="1">index タグ（索引ページの root 要素）を定義。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/section.xsl">section.xsl</a></td>
		<td markdown="1">ドキュメント中の章割り・章の参照。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/keyword.xsl">keyword.xsl</a></td>
		<td markdown="1">キーワードの定義・参照。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/figure.xsl">figure.xsl</a></td>
		<td markdown="1">キャプションつきの図表の定義。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/source.xsl">source.xsl</a></td>
		<td markdown="1">ソースファイルやコンソール出力の標示用。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/exercise.xsl">exercise.xsl</a></td>
		<td markdown="1">演習問題の定義・参照・一覧作成。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/link.xsl">link.xsl</a></td>
		<td markdown="1">リンク集作成。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/qanda.xsl">qanda.xsl</a></td>
		<td markdown="1">Q＆A 作成。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/mathenv.xsl">mathenv.xsl</a></td>
		<td markdown="1">数式の記述用。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/variable.xsl">variable.xsl</a></td>
		<td markdown="1">数式中で使う変数の定義・参照用。</td>
	</tr>
	<tr>
		<td markdown="1"><a href="../../../assets/media/ufcpp2000/xml/xslfiles/amazon.xsl">amazon.xsl</a></td>
		<td markdown="1">Amazon アフィリエイト生成用</td>
	</tr>
</tbody></table>
    
<a href="../../../assets/media/ufcpp2000/xsd/xsd.zip">XSD ファイルをまとめた ZIP ファイル</a>

## 章

### <a id="xslsummary"></a>[XSL概要](xslsummary/index.md)

- [概要](xslsummary/about.md)
- [論理マークアップとデザインの変更](xslsummary/logical.md)
- [XML で数式を書こう](xslsummary/math.md)
- [XSD](xslsummary/xsd.md)

### <a id="summary"></a>[スタイルシートの説明](summary/index.md)

- [ドキュメント](summary/document.md)
- [ドキュメントのパラメータ](summary/nomenu.md)
- [未定義タグ](summary/general.md)
- [章の参照](summary/section.md)
- [キーワードの参照](summary/keyword.md)
- [変数](summary/variable.md)
- [図表](summary/figure.md)
- [ソースファイル](summary/source.md)

### <a id="links"></a>[リンク集](links/index.md)

- [リンク集（WEB関連、XML/XSLT）](links/link.md)

### <a id="ref"></a>[数式表現用XML](ref/index.md)

- [絶対値](ref/abs.md)
- [アレフ](ref/aleph.md)
- [偏角](ref/arg.md)
- [バー](ref/bar.md)
- [{}括弧](ref/brace.md)
- [括弧](ref/bracket.md)
- [条件分岐](ref/branch.md)
- [複素共役](ref/conjugate.md)
- [微分のd](ref/d.md)
- [時間微分](ref/ddt.md)
- [微分](ref/differential.md)
- [重積分記号](ref/doubleint.md)
- [指数関数の底](ref/e.md)
- [書体](ref/font.md)
- [フーリエ変換など](ref/fourier.md)
- [分数](ref/frac.md)
- [積分記号](ref/int.md)
- [lim](ref/limit.md)
- [指数・対数](ref/log.md)
- [行列](ref/matrix.md)
- [周回積分記号](ref/oint.md)
- [演算子](ref/operator.md)
- [()括弧](ref/paren.md)
- [時間偏微分](ref/pddt.md)
- [2階時間偏微分](ref/pddt_second.md)
- [Π](ref/pi.md)
- [実部・虚部](ref/re.md)
- [留数](ref/res.md)
- [∑](ref/sigma.md)
- [三角関数](ref/sin.md)
- [\[\]括弧](ref/sqbracket.md)
- [上付き・下付き文字](ref/subsup.md)
- [記号](ref/symbol.md)
- [3重積分記号](ref/tripleint.md)
- [ベクトル解析用記号](ref/va.md)
- [ベクトル](ref/vec.md)
- [縦ベクトル](ref/vervec.md)
