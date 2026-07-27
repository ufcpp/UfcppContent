# Offline regeneration

Regeneration is local-only. Do not contact the live site, Azure, Kudu, storage, or any other
network endpoint.

## Prerequisites

- The .NET SDK selected by `global.json`.
- The published Umbraco XML snapshot.
- The extracted Media directory.
- The matching sitemap snapshot.
- The local legacy source repository used only to recover referenced static files.

Keep all acquisition inputs outside this Git repository.

## Generate

From the repository root:

```powershell
dotnet run --project .\tools\Ufcpp.ContentConverter -- `
  --snapshot <published-content.xml> `
  --media <extracted-media-root> `
  --sitemap <live-sitemap.xml> `
  --rewrite-maps .\tools\Ufcpp.ContentConverter\data\rewrite_rewritemaps.config `
  --legacy-root <legacy-source-repository> `
  --output .
```

The converter deletes only `content`, `assets`, and `catalog` below `--output`. It never
deletes tooling, tests, or documentation.

The rewrite maps are parsed into a *routing* alias set that also contains derived paths
(`/study`-less and extension-less variants) so legacy links inside the source content can
be resolved. Only the subset returned by `AliasPolicy.SelectPublished` is written to the
front-matter `aliases` and `catalog/content-map.json`, because only those become published
redirects. `LegacyUrlCoverageTests` re-checks the committed content against the rewrite
maps offline, so regeneration cannot silently drop a legacy URL.

The XML parser uses `DtdProcessing.Ignore` and a null resolver. Generation fails on malformed
hierarchy, unknown document types or macros, malformed exercise JSON, unsafe output paths,
ambiguous aliases, unresolved internal files or fragments, missing assets, sitemap differences,
or required accounting differences.

Legacy `<pre>` elements whose class list contains `source` are preserved as raw HTML after
newline normalization. Their attributes and nested markup, including syntax-coloring spans and
editorial annotations such as `<em>`, are not converted to Markdown fences. Other preformatted
blocks and existing Markdown fences continue through language detection and normalization.

## Validate

```powershell
dotnet build .\UfcppContent.slnx --no-restore
dotnet test .\UfcppContent.slnx --no-build
```

For a deterministic comparison, generate once to the repository and once to a local
`.validation-output` directory using the same inputs, then compare `content`, `assets`, and
`catalog` byte-for-byte. Remove `.validation-output` afterward.

Expected accounting is recorded in `catalog/validation-report.json`:

- 1,150 source nodes
- 1,107 Markdown outputs
- 928 public content pages
- 179 structural indexes
- 34 integrated exercises
- 4 metadata-only subject groups
- 5 excluded runtime/configuration nodes
- 1 explicitly recorded missing required property in the source snapshot
- 928 sitemap URLs matched

The acquired sitemap contains the two runtime error URLs and omits the generated Search and
Sitemap utility pages. The converter validates that acquired 928-URL set exactly while retaining
the required Search and Sitemap Markdown outputs and excluding runtime error bodies.
