# Content format

## Paths

Canonical directory URLs map to repository paths as follows:

- `/` → `content/index.md`
- Article-like pages → a leaf Markdown file, such as
  `/study/csharp/start/foo/` → `content/study/csharp/start/foo.md`
- Blog entries → `content/blog/<year>/<month>/<slug>/index.md`
- Structural pages → `index.md` inside their directory

The converter rejects case-insensitive and Unicode-normalized collisions, reserved Windows
names, invalid characters, and trailing spaces or periods.

## Front matter

Every generated Markdown file begins with deterministic YAML:

```yaml
---
title: "Page title"
source_url: "https://ufcpp.net/canonical/directory/url/"
content_type: "Article"
published_at: "2000-01-01T00:00:00"
updated_at: "2000-01-02T00:00:00"
tags: []
umbraco_id: 1234
parent_id: 1233
sort_order: 0
aliases: []
---
```

Strings use JSON-compatible YAML quoting. Arrays are sorted deterministically. Article dates
prefer `sinceSet` and `lastUpdatedSet`; blog publication dates use `firstPublishedDate`.

## Body conversion

- Original `bodyText` MarkdownDeep/Extra Markdown and embedded HTML are preserved except for
  legacy code blocks.
- ATX headings use a space after the marker and are followed by a blank line; fenced code, HTML
  comments, and front matter are otherwise left unchanged.
- Legacy `<pre>` blocks become fenced Markdown with GitHub Linguist language names. Explicit
  language metadata, filenames, titles, code syntax, and the page section are considered in that
  order. Known legacy code blocks default to C#; bare preformatted prose defaults to `text`.
- Existing fenced code blocks keep their language, or receive one by the same inference when the
  info string is empty.
- Code blocks inside HTML tables retain their HTML structure and receive the equivalent standard
  `language-*` class because fenced Markdown cannot be rendered inside an HTML table.
- `bodyTextParsed` is not used.
- HTML tables and MathML remain embedded HTML.
- Legacy heading anchors are normalized to valid empty HTML anchors for GitHub rendering.
- Legacy generated section IDs and known historical fragment aliases remain valid.
- Original page boundaries become `<!-- original-page-break -->`. The archive renders each
  article as a single page, so the marker only records where the legacy `?p=` pagination used
  to split the article and has no effect on the generated HTML.
- The legacy `?p=` page-number query is removed from internal links; only the fragment is kept,
  and every anchor that used to live on a later page remains reachable on the single page.
  Inbound external links that still carry `?p=` continue to work because the query is ignored.
- Exercises are embedded in their parent article and in the subject exercise list.
- The three known Umbraco macros are expanded to static content; any unknown macro fails the
  conversion.
- Deprecated Amazon Associates book widgets on AboutMe pages are converted to direct Amazon
  product links while preserving the affiliate tag.
- Internal content links become relative Markdown links. Runtime navigation queries become
  fragments.
- Every generated relative file target and Markdown fragment is validated.
- Internal assets become relative links below `assets/`; external URLs remain external.
- Files below `assets/` preserve their original bytes and encoding; Git does not normalize them.
- Source node 1410 is a recorded legacy anomaly: its absent `bodyText` is emitted as an empty
  value. Any other missing required body property fails generation.
- Archived Silverlight demo links point to their preserved `.xap` payload because the original
  runtime wrapper pages are not present in the local source.

## Catalogs

`catalog/content-map.json` contains all 1,150 source nodes, including generated, integrated,
metadata-only, and excluded nodes. `catalog/asset-map.json` records source-relative provenance,
hash, size, original URL, and output path for each copied asset. No catalog stores an absolute
source path.
