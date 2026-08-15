# Blog・記事の追加

この文書では、`content/` の Markdown を直接編集して、Blog エントリーや解説記事を
追加する手順を説明します。コマンドはリポジトリのルートから PowerShell で実行します。

> [!IMPORTANT]
> 現在は `content/` の Markdown を正として直接保守します。初回移行に使用した
> `Ufcpp.ContentConverter` は廃止済みで、必要な場合に履歴を確認できるよう
> Git tag `archive/content-converter` にのみ保存されています。

## ファイル構成

| 種別 | Markdown の配置先 | `content_type` | 追加先の索引 |
|---|---|---|---|
| Blog | `content/blog/<年>/<月>/<slug>/index.md` | `BlogEntry` | 月、年、Blog トップ |
| 解説記事 | `content/study/<分野>/<章>/<slug>.md` | `Article` | 章と分野 |

新しいファイル名と `slug` には、小文字の ASCII、数字、ハイフンを使うことを推奨します。
月のディレクトリ名は新規追加ではゼロ埋めせず、`1` から `12` を使います。既存 URL の
大文字小文字、Unicode 正規化、Windows の予約名と衝突する名前は使用できません。

## 共通準備

### 1. URL、日時、親ページを決める

- `source_url` は `https://ufcpp.net/` から始まる公開 URL とし、末尾に `/` を付けます。
- Blog の日時には公開日時、記事の日時には初回公開日時と最終更新日時を使います。
  新規追加では `2026-08-15T21:43:47+09:00` のようにタイムゾーンを含む ISO 8601
  形式を推奨します。
- `parent_id` には、親となる月ページまたは章ページの `umbraco_id` を指定します。
- `sort_order` には、同じ親を持つ既存ページの最大値に 1 を足した値を指定します。
  索引の表示順は Markdown のリスト順で決まるため、索引も別途更新します。

`source_url` が公開 URL の正です。たとえば
`content/blog/2026/8/example/index.md` の `source_url` は
`https://ufcpp.net/blog/2026/8/example/` にします。配置先と URL が異なる状態に
しないでください。

### 2. 一意な ID を採番する

`umbraco_id` は名前にかかわらず、SiteGenerator が全ページの一意なノード ID として
パンくずの構築に使用します。既存 ID の最大値を次のコマンドで確認し、新しく作る
各ページに最大値より大きい ID を順番に割り当てます。

```powershell
$ids = Get-ChildItem .\content -Recurse -Filter *.md |
  Select-String -Pattern '^umbraco_id:\s*(\d+)\s*$' |
  ForEach-Object { [int]$_.Matches[0].Groups[1].Value }

($ids | Measure-Object -Maximum).Maximum
```

年、月、エントリーを同時に作る場合は、それぞれに別の ID が必要です。`parent_id` が
参照するページを先に決めてから、親から子の順に採番すると間違いを防げます。

### 3. front matter を設定する

すべての Markdown は YAML front matter で始めます。

| 項目 | 指定内容 |
|---|---|
| `title` | ページタイトル。本文の `#` 見出しと一致させる |
| `source_url` | 末尾 `/` 付きの正規公開 URL |
| `content_type` | この文書で示す値を大文字小文字も含めて指定する |
| `published_at` | 初回公開日時 |
| `updated_at` | 最終更新日時。新規公開時は `published_at` と同じ値でよい |
| `tags` | Blog のカテゴリ。不要なら `[]` |
| `umbraco_id` | 全 Markdown で一意な整数 |
| `parent_id` | 親ページの `umbraco_id` |
| `sort_order` | 同じ親を持つページ内の順序 |
| `aliases` | 実在した旧 URL。新規ページでは通常 `[]` |

`aliases` に、推測した URL や単なる別表記を追加しないでください。各 alias は実際の
リダイレクトページとして公開されます。

### 4. 画像などのアセットを追加する

画像やダウンロード ファイルは、用途ごとに衝突しないディレクトリを `assets/` 配下へ
作って配置します。

```text
assets/media/blog/<年>/<月>/<slug>/...
assets/media/study/<分野>/<章>/<slug>/...
```

本文からは、深さに依存しないルート相対 URL で参照できます。

```markdown
![画像の説明](/assets/media/blog/2026/8/example/diagram.png)
```

`catalog/asset-map.json` は移行元スナップショットの来歴情報なので、手動追加した
アセットのために編集しません。

## Blog エントリーを追加する

### 1. 年ページと月ページを用意する

対象の年と月がすでに存在する場合は、その `index.md` を使います。存在しない場合は
近い年・月のファイルをコピーし、次の関係になるように front matter と本文を変更します。

| ページ | 配置先 | `content_type` | `parent_id` |
|---|---|---|---|
| 年 | `content/blog/<年>/index.md` | `BlogYear` | `content/blog/index.md` の `umbraco_id` |
| 月 | `content/blog/<年>/<月>/index.md` | `BlogMonth` | 年ページの `umbraco_id` |

年ページの本文には月ごとの見出しとエントリー一覧、月ページの本文にはその月の
エントリー一覧を置きます。新しい年を作った場合は、`content/blog/index.md` の
「年別」にも新しい年へのリンクを追加します。

### 2. エントリーを作る

`content/blog/<年>/<月>/<slug>/index.md` を作り、`<...>` を実際の値に置き換えます。

```yaml
---
title: "<タイトル>"
source_url: "https://ufcpp.net/blog/<年>/<月>/<slug>/"
content_type: "BlogEntry"
published_at: "<公開日時>"
updated_at: "<更新日時>"
tags: []
umbraco_id: <一意な ID>
parent_id: <月ページの umbraco_id>
sort_order: <同じ月内の順序>
aliases: []
---

# <タイトル>

<本文>
```

タグを付ける場合は、決定的な差分になるよう名前順に記載します。

```yaml
tags:
  - "C# 14.0"
  - ".NET 10"
```

### 3. Blog の索引を更新する

公開日順になる位置へ、同じタイトルとリンクを追加します。

1. 月ページ `content/blog/<年>/<月>/index.md`

   ```markdown
   - 2026-08-15 [タイトル](<slug>/index.md)
   ```

2. 年ページ `content/blog/<年>/index.md`

   ```markdown
   - 2026-08-15 [タイトル](<月>/<slug>/index.md)
   ```

3. Blog トップ `content/blog/index.md` の「最新の投稿」

   ```markdown
   - 2026-08-15 [タイトル](<年>/<月>/<slug>/index.md)
   ```

「最新の投稿」は公開日時の降順で 20 件を維持します。21 件になった場合は最も古い
項目を外します。過去日付のエントリーを追加する場合も、日付順の位置へ挿入します。
RSS は `BlogEntry` を公開日時順に最大 30 件選んで自動生成するため、RSS ファイルを
手で編集する必要はありません。

### 4. 静的サイトマップを更新する

Blog の年とエントリーは静的サイトマップ `content/sitemap.md` にも掲載します。
既存の正規 URL 順を保ち、Blog 年の配下へエントリーを追加します。新しい年の場合は
年ページも追加します。月ページは静的サイトマップの掲載対象ではありません。

```markdown
    - [2026](blog/2026/index.md)
        - [タイトル](blog/2026/8/<slug>/index.md)
```

### 5. タグのカテゴリ索引を更新する

`tags` が空でなければ、`content/search.md` の同名カテゴリにもエントリーを公開日順で
追加します。新しいタグの場合は、UTF-8 のタグ名を SHA-256 でハッシュ化した先頭
12 文字をカテゴリ ID に使います。

```powershell
$tag = "C# 14.0"
$bytes = [Text.Encoding]::UTF8.GetBytes($tag)
$hash = [Convert]::ToHexString(
  [Security.Cryptography.SHA256]::HashData($bytes)
).ToLowerInvariant().Substring(0, 12)
$hash
```

```markdown
### <a id="blog-category-<ハッシュ>"></a>C# 14.0

- 2026-08-15 [タイトル](blog/<年>/<月>/<slug>/index.md)
```

新しいカテゴリを作った場合は、`content/blog/index.md` の「カテゴリ」にもリンクを
追加します。

```markdown
- [C# 14.0](../search.md#blog-category-<ハッシュ>)
```

## 解説記事を追加する

### 1. 記事を作る

既存の分野と章へ追加する場合は、
`content/study/<分野>/<章>/<slug>.md` を作ります。`<...>` を実際の値に
置き換えます。

```yaml
---
title: "<タイトル>"
source_url: "https://ufcpp.net/study/<分野>/<章>/<slug>/"
content_type: "Article"
published_at: "<公開日時>"
updated_at: "<更新日時>"
tags: []
umbraco_id: <一意な ID>
parent_id: <章ページの umbraco_id>
sort_order: <同じ章内の順序>
aliases: []
---

# <タイトル>

## <最初の節>

<本文>
```

本文内のリンクは、Markdown ファイルから見た相対パスまたは `/` から始まるサイト内
URL にします。既存ページの見出しへリンクする場合は、リンク先に存在する ID を使います。

### 2. 章と分野の索引を更新する

親となる章の `content/study/<分野>/<章>/index.md` に記事へのリンクを追加します。

```markdown
- [タイトル](<slug>.md)
```

分野ページにも章ごとの記事一覧がある場合は、同じ記事へのリンクを追加します。
たとえば C# の分野ページ `content/study/csharp/index.md` は、各章の配下に記事一覧を
持つため、章ページと分野ページの両方を更新します。

```markdown
- [タイトル](<章>/<slug>.md)
```

新しい章を作る場合は `content/study/<分野>/<章>/index.md` を `Chapter` として作り、
`parent_id` に分野ページの `umbraco_id` を設定して、分野ページからリンクします。
新しい分野を作る場合は `content/study/<分野>/index.md` を `Subject` として作り、
`parent_id` に `content/study/index.md` の `umbraco_id` を設定して、Study トップから
リンクします。Home の「学習コンテンツ」にも掲載する場合は `content/index.md` へ
同じ分野を追加します。構造ページにも、それぞれ一意な `umbraco_id` が必要です。

### 3. 静的サイトマップを更新する

`content/sitemap.md` にも、既存の正規 URL 順とインデントを保って記事へのリンクを
追加します。新しい分野の場合は分野ページも追加します。章ページは静的サイトマップの
掲載対象ではありません。

```markdown
        - [タイトル](study/<分野>/<章>/<slug>.md)
```

`content/sitemap.md` は手動で保守する公開ページですが、生成後の `_site/sitemap.xml` は
対象の front matter から自動生成されます。`_site/sitemap.xml` は編集しません。

## 検証する

### 1. build と test

初回または依存関係が変わった後は restore を含めて実行します。

```powershell
dotnet restore .\UfcppContent.slnx
dotnet build .\UfcppContent.slnx --no-restore
dotnet test .\UfcppContent.slnx --no-build
```

### 2. サイトを生成する

SiteGenerator は、ID の重複、存在しない親 ID、階層の循環、URL の衝突、内部リンク、
フラグメント、アセット参照を検証します。

```powershell
dotnet run --project .\tools\Ufcpp.SiteGenerator --no-build -- `
  --content .\content `
  --assets .\assets `
  --output .\_site `
  --include-preview-server
```

生成に成功したら `_site/` へ移動し、プレビュー サーバーを起動します。

```powershell
Set-Location .\_site
dotnet run server.cs
```

確認後は `Ctrl+C` で終了します。`_site/` は生成物であり `.gitignore` の対象なので、
コミットしません。

### 3. 公開前に確認する

Blog の場合:

- エントリー、月、年、Blog トップの各リンクから移動できる
- パンくず、公開日、更新日、タグが意図どおり表示される
- `_site/rssfeed.xml` の最新 30 件内に入る場合、タイトル、URL、公開日、カテゴリが正しい
- タグを使う場合、Blog トップからカテゴリ一覧へ移動できる

解説記事の場合:

- 章と分野の索引から移動できる
- パンくずが Study トップ、分野、章、記事の親子関係を反映している
- 目次、見出しリンク、本文内リンク、画像が正しく表示される

共通:

- `/sitemap/` の静的サイトマップから新しいページへ移動できる
- `_site/sitemap.xml` に正規 URL が含まれる
- `git status` に意図した Markdown とアセットだけが表示される
- `catalog/` と `_site/` が変更対象に含まれていない
