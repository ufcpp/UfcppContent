# CSS パリティ（ufcpp.net との突合）

`content/` の記事本文は移行元 ufcpp.net の HTML をほぼそのまま引き継いでいる。そのため本文には
`version13` や `pros-mark` のようなレガシー クラスが残っており、生成サイトのスタイルシート
`tools/Ufcpp.SiteGenerator/wwwroot/css/site.css` がそれらを定義していないと「クラスは付いて
いるのにスタイルが当たらない」要素が発生する。

このドキュメントは、その差分をどう突合し、どこまで一致させ、どこを意図的に変えているかを
記録する。

## 決定

**本文（`.content` 配下）は ufcpp.net の見た目に合わせる。サイトの外枠は合わせない。**

- 合わせる対象は、記事本文が実際に使っていて、かつ ufcpp.net の本文用 CSS が定義している
  クラスに限る
- ヘッダー、サイドバー、広告枠、ページャー、ブログ ウィジェットなどのサイト外枠は
  `DESIGN.md` / `PRODUCT.md` に基づく意図的な再設計であり、突合対象外とする
- JavaScript は導入しない。ufcpp.net が JavaScript 前提で動かしているコンポーネントは、
  **内容を常に表示したまま**、**操作できるようには見せない**方針で静的に描画する

## 参照スタイルシート

| 項目 | 値 |
|---|---|
| 取得元 | `https://ufcpp.net/css/bundle.min.css` |
| 取得日時 | 2026-07-27T06:13:22Z |
| SHA-256 | `9214DFC3D098809824785CBB7EA11AB477C6F24EC2910FA308008DD83CB97383` |
| サイズ | 35,534 バイト |

参照 CSS は**リポジトリにコミットしない**。ufcpp.net のスタイルシートは本リポジトリの
成果物ではないうえ、テストや CI がネットワークに依存すると再現性を失うため。突合時のみ
作業ディレクトリに取得する。

```powershell
$dir = "$env:TEMP\ufcpp-css-ref"
New-Item -ItemType Directory -Force -Path $dir | Out-Null
curl.exe -s -o "$dir\bundle.min.css" https://ufcpp.net/css/bundle.min.css
(Get-FileHash "$dir\bundle.min.css" -Algorithm SHA256).Hash

# 1 行 1 ルールに展開しておくと読みやすい
(Get-Content "$dir\bundle.min.css" -Raw) -replace '\}', "}`n" |
    Set-Content "$dir\bundle.expanded.css"
```

ハッシュが上表と異なる場合は元サイト側が更新されている。差分を確認したうえで、この表と
突合結果を更新すること。

## 突合手順

突合は 3 段階に分かれている。上から順に、粗い網羅 → 実際の描画 → 回帰防止。

### 1. クラスの網羅性を突合する

```powershell
pwsh -NoProfile -File .\tools\css-class-reconciliation.ps1
```

`content/**/*.md` と `Templates/**/*.razor` が使っているクラスを抽出し、`site.css` と
参照 CSS の双方に対して突合する。Markdown からの抽出時にはフェンス コード ブロック、
インライン コード、エンティティ エスケープされたマークアップ（`&lt;div class="..."&gt;`）を
除外する。これをしないと、解説文として書かれた HTML の例が「使用箇所」として誤検出される。

出力は 2 種類。

- **使われているのに未定義** — 修正候補。現在 11 件で、いずれも参照 CSS 側にも定義が無い
  （後述の許可リスト）
- **定義されているのに未使用** — 情報のみ。ビルド時に生成される Roslyn / ColorCode の
  トークン クラスが大半なので、CI を落とす材料にはしない

### 2. 計算値をブラウザーで突合する

クラス名の有無だけでは詳細度の問題を検出できない。実際にレンダリングしたうえで
`getComputedStyle` を突き合わせる。

```powershell
# 生成してローカルに配信する
dotnet run --project tools/Ufcpp.SiteGenerator -- --content content/ --assets assets/ --output _site/
cd _site; python -m http.server 8791 --bind 127.0.0.1

# 別のシェルから突合する
node tools/css-parity-compare.mjs tools/css-parity-cases.json
node tools/css-parity-compare.mjs --width 480 tools/css-parity-cases.json
```

`tools/css-parity-compare.mjs` はヘッドレス Chrome（または Edge）を DevTools Protocol で
操作し、同じパス・同じセレクターについてローカルと ufcpp.net の計算値を比較する。Node の
組み込み機能だけで動くので、npm install は不要。差分があれば終了コード 1 を返す。

`tools/css-parity-cases.json` には**一致していなければならない**ケースだけを載せている。
意図的に差がある箇所（後述）はここに入れず、必要に応じてセレクターを直接渡して確認する。

```powershell
node tools/css-parity-compare.mjs '[{"label":"expand","path":"/study/csharp/async/misc_asyncflow/","selector":".expand-panel"}]'
```

なお、幅 0 のボーダーの色とスタイルは比較対象から外している。ufcpp.net は `.versionN` で
`border-color` ショートハンドを使っており四辺すべてに色が付くが、描画されるのは左辺だけ
なので、残り三辺の色差は誰にも見えないためである。

### 3. 回帰をテストで固定する

`tests/Ufcpp.SiteGenerator.Tests/SiteCssParityTests.cs` が次を検証する。ネットワークにも
ブラウザーにも依存しない。

- `content/` が使うクラスが `site.css` に定義されているか、理由付きの許可リストに載っている
- 許可リストが陳腐化していない（もう使われていない項目が残っていない）
- `version` 系 22 クラスの色とボーダー スタイルが期待どおり
- `.content h3`〜`h6` が `margin` ショートハンドを使っていない（後述の詳細度バグの再発防止）
- `.expand-panel` が表示されたまま
- 操作できない要素に `cursor` や `:hover` が付いていない

## 詳細度の罠

今回の不具合は、単に「ルールが無い」だけではなかった。移植時に踏みやすい罠を残しておく。

### `margin` ショートハンドが `margin-left` を打ち消す

修正前の `site.css` は見出しに `margin: 16px 0` と**ショートハンド**で書いていた。これは
`margin-left: 0` を詳細度 (0,1,1) で宣言するため、`.version { margin-left: 8px }` (0,1,0) に
勝ってしまう。ufcpp.net は `margin-top` / `margin-bottom` の**ロングハンド**で書いており、
左マージンが素通りする。

```css
/* ufcpp.net */
.container h3, h4, h5, h6 { margin-top: 16px; margin-bottom: 16px; }
```

`site.css` も同じくロングハンドに変更した。h3〜h6 には他に左右マージンの供給源が無いので、
`.version` 以外の計算結果は変わらない。

### `:where()` で詳細度を上げずにスコープする

`.version` 系は `:where(.content) .version13` の形で書いている。`:where()` は詳細度に寄与
しないため、ufcpp.net と同じ (0,1,0) を保ったままスコープできる。ここを素直に
`.content .version13` (0,2,0) と書くと 2 つの問題が起きる。

1. `.content h5` (0,1,1) に勝ってしまい、`h5.version13` の文字色が水色になる。ufcpp.net では
   `.container h5` が勝つので **h5 のバージョン見出しは紺色のまま**であり、バージョン別の色が
   出るのは `div.version.versionN` だけ
2. `.version` と `.versionN` の詳細度が揃わなくなり、`.version` の `border-left`
   ショートハンドが `.versionN` の色指定に勝ってバージョン色が消える

`.version` と `.versionN` は**同じ詳細度で、`.versionN` を後に置く**必要がある。

### 移植しなかった宣言

- `display: flexbox` — 2011 年ドラフトの無効値でブラウザーが破棄する（実質 `block`）
- `.expand-button::before` の FontAwesome グリフ — アイコン フォントを配信していない

## 移植したクラス

| クラス | 内容 | 主な使用箇所 |
|---|---|---|
| `.version` + `.version2`〜`.version19`, `.version7_1/_2/_3` | 色付きの左ボーダー。8〜13 は `ridge`、14〜19 は `double` | 本文 300 箇所（h5 277 / div 23） |
| `pre.source` | 本文色 `#000` | 多数 |
| `table.layout` | セルの罫線と背景を消す | 6 ファイル |
| `table.variable` | 文字色 `#2c2e55`、`tbody` の上下に 4px の罫線 | `study/physics/em/variable.md` |
| `.pros-mark` / `.cons-mark` | 赤 / 青のマーカー | `fun_whyextensions.md` 17 箇所 |
| `.input` | 橙色の太字（入力箇所の強調） | 3 箇所 |
| `.expand-button` / `.expand-panel` | 折りたたみ（下記のとおり静的化） | 13 ファイル |
| `.tab-container` | 言語別タブ（下記のとおり静的化） | 7 ファイル |
| `.latest-posts` | 取り込んだ更新履歴リスト | ブログ 1 件 |

`.version13` の確認用ページとして `content/study/misc/list/test.md` が全バリエーションを
並べている（`/study/misc/list/test/`）。目視確認はここが最も効率が良い。

`table.variable` の文字色は、参照 CSS の `.variable { color: #2c2e55 }` という**要素を選ばない
ルール**に由来する。本リポジトリのシンタックス ハイライターは `.variable` を出力しないため、
コード トークンへ漏れないよう `table.variable` に限定して移植した。

## 未定義のまま許容するクラス

次の 11 クラスは `content/` で使われているが `site.css` に定義が無い。いずれも
**参照 CSS 側にも定義が無い**、つまり ufcpp.net でもスタイルが当たっていないため、
差分ではない。`SiteCssParityTests` の許可リストに理由付きで登録している。

| クラス | 理由 |
|---|---|
| `color` | ASCIIMath の余白用スペーサー |
| `speakerdeck-embed` | 外部埋め込み（Speaker Deck 側の CSS） |
| `subject` | ブログ本文の見出し補助。元サイトでも未定義 |
| `language-console` / `language-xml` | Markdown の言語指定がそのまま残ったもの |
| `silverlightControlHost` | 廃止済みプラグインのホスト要素 |
| `key-file-local-type` | 記事内の一時的なマーカー |
| `twitter-tweet` | 外部埋め込み（X / Twitter 側の CSS） |
| `site-footer-links` | 本リポジトリ側。`.site-footer p` / `a` が既に賄っている |
| `version11*` | `st_operator.md:228` の誤記（末尾の `*`）。元サイトでも壊れている |
| `xsource` | `devmodel.md:50` の誤記。`source` の打ち間違いと思われる |

許可リストは「もう使われていない項目」も検出するので、コンテンツ側で誤記が直れば
テストが落ちて気付ける。

## 意図的な差分

以下は**一致させない**と決めた箇所である。`tools/css-parity-cases.json` には含めていない。

### サイト外枠

ヘッダー、サイドバー、広告枠、ページャー、ブログ ウィジェットは `DESIGN.md` に基づく
再設計であり、ufcpp.net とは別物。

### インライン コードの文字サイズ

`site.css` は `code { font-size: 0.9em }` を持つが ufcpp.net は持たない。そのため
`pre` / `code` の内側にある `.input` などは 14.4px、ufcpp.net では 16px になる。これは
コードを枠付きのチップとして見せる本サイト全体のタイポグラフィ設計であり、変更すると
全ページに波及する。

### テーブルの表示方法と狭い画面での文字サイズ

`site.css` は `table { display: block; overflow-x: auto }` で、狭い画面でも横スクロールで
表を読めるようにしている（ufcpp.net は `display: table`）。加えて 640px 以下では
`table { font-size: 0.875rem }` を当てるため、幅 480px で比較すると表内の `font-size` と
それに比例する `em` 由来のパディングだけが一致しない。色・罫線・レイアウトは一致する。

### 数式テーブルのセル パディング

ufcpp.net は `td.intsup { padding: 0 }` のように数式セルの余白を 0 にしようとしているが、
`.container table td { padding: .2em .3em }` (0,1,2) に詳細度で負けており効いていない。
本サイトは `.content table.integral td` (0,2,2) で書いているため、**作者の意図どおり**
余白 0 で描画される。分数や積分が ufcpp.net よりわずかに詰まって見えるが、これは
`.version` の不具合とまったく同じ詳細度バグを元サイト側が踏んでいるだけなので、
描画結果ではなく意図に合わせている。

なお分数の横線は、ufcpp.net が `#4c4c4c` をリテラルで指定しているのに合わせ、
`currentColor` ではなく `var(--color-text)`（同じ値）を使う。こうしないと
`table.variable` のような色付きブロックの中で線の色がずれる。

### JavaScript 前提のコンポーネント

本サイトは JavaScript を配信しない（`docs/site-search.md`、`DESIGN.md` を参照）。そのため
次の 2 つは「内容は常に見える」「操作できるようには見せない」方針で静的に描画する。

| | ufcpp.net | 本サイト |
|---|---|---|
| `.expand-panel` | `display: none` で始まり JS で開閉 | 常に `display: block` |
| `.expand-button` | `cursor: pointer`、`:hover` 背景、`::before` の FontAwesome アイコン | どれも無し。本文と同じ体裁のラベル |
| `.tab-container > ul li` | `cursor: pointer`、`:hover`、選択中タブの `.current` を JS が付与 | 凡例として並べるだけ。選択状態は無し |
| `.tab-container > div` | JS が `.view` を付けて 1 つだけ表示 | すべて枠線で区切って表示 |

`.expand-button` の紫のチップは ufcpp.net では `::before` 疑似要素（アイコン）に付いており、
ラベル文字そのものには付かない。アイコンを持たない本サイトでチップだけ再現すると
ラベル全体が押せそうなボタンに見えてしまうため、再現しない。

JavaScript 無しで同等の操作性を得る案（`<details>` / `<summary>`、radio + label によるタブ）は
生成側の HTML 変換が必要になるため、別 Issue (#34) とする。

### シンタックス ハイライト

ufcpp.net はレガシーな `span.reserved` / `span.type` などでコードを色付けしているが、
本サイトは Roslyn / ColorCode が出力する `roslyn-*` クラスを使う。レガシー側の span も
`site.css` に残してあるため、移行前の記法が混在していても表示は崩れない。

## コンテンツ側の既知の不整合（本 Issue では直さない）

移行元から引き継いだ記述の揺れ。CSS の問題ではないので記録のみ。

- `content/study/csharp/start/st_operator.md:228` — `class="version version11*"`（末尾に `*`）
- `content/study/dotnet/silverlight/devmodel.md:50` — `<pre class="xsource">`
- `ap_ver7_2.md:18`、`ap_ver7_3.md:16`、`stnumber.md:131`、`sp4_optional.md:187` —
  本文が「Ver. 7.2」「Ver. 7.3」なのにクラスが `version7_1`

## 元サイトが更新されたとき

1. 参照 CSS を取得し直し、SHA-256 が上表と違うことを確認する
2. `tools/css-class-reconciliation.ps1` で新しく使われ始めたクラスが無いか確認する
3. `tools/css-parity-compare.mjs` を広い幅と狭い幅の両方で実行する
4. 差分が意図的なら本ドキュメントの「意図的な差分」に追記し、そうでなければ `site.css` を
   直したうえで `SiteCssParityTests` に不変条件を追加する
5. 上表の取得日時と SHA-256 を更新する
