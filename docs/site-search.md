# 静的サイト内検索の設計

## 決定

サイト自身に全文検索エンジンを持たず、`/search/` に通常の
HTML フォームを置いて Google の `ufcpp.net` 指定検索へ遷移させる。
Google Programmable Search Element や外部 JavaScript は埋め込まない。

この方式を採る理由は次のとおり。

- 日本語・英語・C# の識別子が混在する検索とランキングを検索エンジンに任せられる
- 初期表示時にも検索時にも、サイトからインデックスや検索ランタイムを配信しない
- サーバー、API キー、検索用ビルド依存、検索サービスの管理画面が不要
- JavaScript が無効でも同じフォームを利用できる
- Google への通信は、利用者が説明を読んでフォームを送信するまで発生しない

フォームは `GET https://www.google.com/search` に `q`（検索語）と
`as_sitesearch=ufcpp.net` を送る。検索ページには「結果は Google に移動して
表示される」ことと、Google のプライバシーポリシーへのリンクをフォームの直前に
表示する。URL パラメーターは Google が将来変更する可能性があるため、リリース前と
年 1 回の手動確認項目にする。

これは Google Programmable Search Engine の契約ではなく、通常の Google 検索への
導線である。費用は発生しない一方、クロールされていないページ、公開直後のページ、
検索エンジン側で除外されたページは見つからず、順位や可用性を保証できない。
この制約が問題になった時点で、後述の Pagefind 案へ移行する。

## 実データによる評価

2026-07-21、リビジョン `ec01ecd` の全 `content/` を既存ジェネレーターで生成して
評価した。入力は 1,107 Markdown ファイルで、`Search` と `Sitemap` を除く
1,105 ページが `search-index.json` に入った。

| 指標 | 結果 |
|---|---:|
| 現行 JSON（インデント、非 ASCII エスケープあり） | 17,763,881 bytes |
| 現行 JSON、gzip -9 | 2,810,928 bytes |
| 同じ項目を compact UTF-8 JSON にした場合 | 10,206,593 bytes |
| compact UTF-8 JSON、gzip -9 | 2,483,592 bytes |
| URL・タイトル・種類・タグだけの compact JSON | 122,869 bytes |
| 同メタデータ、gzip -9 | 21,978 bytes |
| Markdown 本文の合計 | 6,261,841 characters |
| 1 ページの最大 Markdown 本文 | 50,132 characters |

再現手順は次のとおり。

```bash
dotnet run --project tools/Ufcpp.SiteGenerator -- \
  --content content/ --assets assets/ --output /tmp/ufcpp-search-evaluation
gzip -9 -c /tmp/ufcpp-search-evaluation/search-index.json | wc -c
```

さらに、大小文字を無視した単純部分一致を実データに適用した。

| クエリ | タイトル一致 | 本文一致を含む候補数 | 上位タイトルの例 |
|---|---:|---:|---|
| `非同期` | 10 | 105 | `非同期メソッド`、`非同期ストリーム` |
| `ジェネリック` | 5 | 133 | `ジェネリック`、`ジェネリック型引数の部分型推論` |
| `LINQ` | 3 | 108 | `LINQ`、`LINQ と遅延評価` |
| `null` | 20 | 284 | `C# 8.0 null許容参照型` |
| `C# 12` | 7 | 46 | `C# 12.0 の新機能` |
| `Span` | 9 | 349 | `Span<T>構造体`、`First-class な Span 型` |

日本語は文字列部分一致でも目的の記事を拾えるが、英単語や識別子はコード例を含む
多数のページに一致する。タイトル一致を本文一致より強くするだけでも上位 5 件は
概ね妥当だったが、現行 JSON は Markdown、HTML、コードをそのまま含むため、
スニペットの品質と候補の絞り込みに追加処理が必要である。約 2.5 MB gzip の全量取得も
検索を使う利用者に対して大きい。このため、現行 JSON をそのままブラウザー検索する
方式は採用しない。

## 候補の比較

| 方式 | 日本語・順位 | 配信と性能 | 運用・プライバシー | 判断 |
|---|---|---|---|---|
| 通常の Google サイト指定検索 | 検索エンジンに委任。クロール遅延あり | サイトから追加配信なし | 送信後は Google に検索語等が渡る。順位・仕様は外部依存 | **初期実装に採用** |
| Google Programmable Search Element | Google の検索品質 | 埋め込み JavaScript が必要 | 広告、利用条件、費用体系、同意表示を継続管理する必要がある | 単純な外部フォームより利点が少ないため不採用 |
| Pagefind extended | 静的 HTML から索引を生成し、CJK 対応あり。実データで精度確認が必要 | 分割インデックスを検索時に遅延取得できる | 検索語を外部送信しない。ビルド用バイナリと JS/Wasm の更新が必要 | Google の制約が問題になった場合の第一候補 |
| MiniSearch/FlexSearch/Lunr 等 | 日本語 tokenizer、部分一致、順位を自分で調整 | 現行方式では大きな JSON とランタイムを取得 | JS 依存と長期保守。ライブラリごとの差分も負担 | Pagefind より自前作業が多いため不採用 |
| 完全な自前実装 | n-gram 等を自由に設計可能 | 分割、圧縮、キャッシュもすべて実装が必要 | 脆弱性・互換性・検索品質を継続保守 | 要件に対して過剰なため不採用 |

Pagefind を導入する場合は、extended/CJK 対応版を固定バージョンでビルド時だけ実行し、
生成 HTML の `lang="ja"` を利用する。採用前に compound word、英数字、記号を含む
下記の評価セットを実機で再評価し、標準版との差も記録する。

参考:

- [Pagefind overview](https://pagefind.app/docs/overview/)
- [Pagefind multilingual search](https://pagefind.app/docs/multilingual/)
- [Google Programmable Search Engine versions](https://support.google.com/programmable-search/answer/9069107)
- [Google Programmable Search Engine terms](https://support.google.com/programmable-search/answer/1714300)

## `/search/` の基本 UX

初期実装の検索ページは次を満たす。

1. `h1`「サイト内検索」
2. 可視の `<label for="site-search-query">検索キーワード</label>`
3. `type="search"`、`name="q"` の入力欄と「Google で検索」ボタン
4. `type="hidden"` の `as_sitesearch=ufcpp.net`
5. 外部サイトへ移動する旨とプライバシーに関する説明
6. Google で見つからない場合の `/sitemap/` への導線

ページを開いた時点では入力欄へ自動フォーカスしない。Tab で入力、送信、サイトマップの
順に移動でき、Enter で送信できること、フォーカスリングが見えること、200% 拡大と
狭い画面で横スクロールしないことを要件とする。フォームには検索ページ内のライブ
リージョンを置かない。結果は Google 側に遷移するため、存在しない「読み込み中」や
「0 件」をサイト側で読み上げない。

JavaScript は不要であり、無効時も機能は同じである。Google を利用できない場合の
空状態はサイトマップで代替する。検索語をサイトのログや Web Storage に保存しない。

将来 Pagefind に移行した場合は、以下も必須とする。

- 2 文字以上で検索し、入力のたびに検索する場合は debounce する
- 検索中、件数、0 件、失敗を `aria-live="polite"` で通知する
- 結果は見出し付きリストとし、各項目にタイトル、URL、スニペットを表示する
- 上下キーで入力欄の通常のカーソル操作を壊さない。結果間移動を実装するなら
  WAI-ARIA combobox pattern を一式実装し、Escape で閉じる
- `<mark>` は文字列連結 HTML ではなく DOM API で生成し、本文を `innerHTML` に渡さない
- 取得・解析失敗時はサイトマップと Google 検索へのリンクを表示する

## URL と静的ホスティング

`/search/` の出力は通常のコンテンツページと同じく `search/index.html` とする。
検索フォームの送信先だけが絶対 HTTPS URL で、サイト内リンクは配置先の base path を
考慮して生成する。

現在のテンプレートには `/search/`、`/assets/...` などのルート相対 URL があるため、
サブパス配信対応は検索ページだけで解決しない。実装時には generator option として
`--base-path`（既定 `/`、例 `/UfcppContent/`）を追加し、次を共通 URL helper で
解決する。

- ヘッダー、ナビゲーション、フッター、CSS、画像、RSS、サイトマップへの URL
- Markdown から変換した内部 URL
- 検索ページと、将来の検索インデックス/chunk URL

canonical URL と sitemap/RSS の公開 URL は base path を含む絶対 URL とする。
文字列置換や `../` の手書きは行わない。base path は先頭と末尾が `/` の形式へ正規化し、
`.`、`..`、query、fragment、scheme-relative URL を拒否する。ルート `/` と
`/preview/` の両方をテスト fixture で生成し、リンク検証を通す。

静的ホストでは JSON、Wasm、JavaScript に正しい MIME type を設定し、fingerprint 付き
検索資産は長期 immutable cache、`search/index.html` は通常の短期 cache とする。
初期採用案は検索資産を持たないため、この cache 設定は Pagefind 移行時だけ必要になる。

## 将来のローカル検索用インデックス

`search-index.json` はブラウザーから取得せず、未使用だったため Google 検索フォームの
実装と同時に生成を停止した。

Pagefind を採用する場合、Pagefind が生成する分割 index を正とし、独自 JSON は作らない。
それでも独自実装が必要になった場合の論理 schema は次のとおりとする。

```json
{
  "version": 1,
  "generatedAt": "2026-07-21T00:00:00Z",
  "documents": [
    {
      "id": 123,
      "path": "study/csharp/async/",
      "title": "非同期メソッド",
      "contentType": "Article",
      "tags": ["C#"],
      "sections": [
        {
          "anchor": "概要",
          "heading": "概要",
          "text": "表示用のプレーンテキスト"
        }
      ]
    }
  ]
}
```

- URL は origin や先頭 `/` を含まない相対 `path` とし、base path と安全に結合する
- 粒度はページではなく見出し section。結果表示では同一ページをまとめる
- title 8、heading 4、tags 4、本文 1 の重みを初期値にする
- Unicode は NFC、英字は invariant lowercase でも索引する。表示文字列は保持する
- 日本語は 2-gram と原文部分一致、英数字/C# 識別子は単語単位と prefix 一致を評価する
- HTML、Markdown 記号、コードブロック、script/style は索引前に除去する
- スニペットは一致位置の前後 60 文字を、単語境界を優先して最大 160 文字で作る
- `generatedAt` は再現可能ビルドでは固定値または省略し、ビルド時刻で毎回変えない

サイズの合格基準は、初回検索で取得する JS/Wasm と index chunk の合計を gzip/Brotli 後
200 KB 以下、1 クエリの追加 chunk を 100 KB 以下、全 index を圧縮後 1.5 MB 以下とする。
低速 4G 相当で検索開始から最初の結果表示まで p75 1 秒以下、メインページでは検索資産を
一切取得しない。基準を超えた場合は content type または path prefix で chunk を分け、
コードを索引対象へ戻すことではなく本文の抽出と section サイズを見直す。

## セキュリティと運用制約

- 外部フォームへ認証情報、現在 URL、referrer 用の独自パラメーターを追加しない
- 検索ページに `Referrer-Policy: strict-origin-when-cross-origin` 以上を設定する
- 検索語を HTML として描画しない。将来表示する場合も必ず text node として扱う
- 検索 index は公開情報であり、非公開の front matter や draft を含めない
- index/chunk URL は同一 origin の HTTPS のみ許可し、ユーザー入力から組み立てない
- 外部スクリプトを後から導入する場合は、プライバシー、CSP、SRI、費用、利用条件、
  障害時 fallback を改めてレビューする
- Google の仕様・ポリシー変更、クロール漏れ、主要クエリの品質を年 1 回確認する

## 後続タスク

順序と完了条件を以下に固定する。

1. **外部検索ページ（完了）**
   - `content/search.md` を追加し、通常の GET form、説明、サイトマップ fallback を実装
   - ヘッダーリンク、フォーム送信、キーボード操作、JavaScript 無効時をテスト
   - `Search` ページが検索データに含まれないことを維持
2. **base path 対応**
   - `--base-path` と共通 URL helper を追加し、全テンプレートと link rewriting に適用
   - `/` と `/preview/` の生成物を integration test で検証
3. **未使用 index の廃止（完了）**
   - 利用者がいないことを確認し、`SearchIndexWriter`、呼び出し、テスト、文書を削除
   - 出力に `search-index.json` が残らないことをテスト
4. **検索品質の定期確認**
   - 上表の 6 クエリに `async/await`、`参照型`、`パターン マッチング` を加えて手動確認
   - 主要記事が上位 5 件にない、またはクロール漏れが 5% を超えたら次へ進む
5. **必要時のみ Pagefind proof of concept**
   - extended 版の固定バージョンを advisory/ライセンス確認後に CI へ追加
   - 実データで上記クエリ、サイズ目標、低速 4G、スクリーンリーダーを比較
   - 合格した場合だけ Google form を fallback として残して切り替える

以上により、初期実装に必要な検索方式、プライバシー上の境界、URL 解決、UX、
評価基準は確定しており、Pagefind の導入自体は現在の未決事項ではない。
