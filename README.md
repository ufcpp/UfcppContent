# ufcpp.net content

This repository is the local, deterministic Markdown archive of the published ufcpp.net
content snapshot. It contains generated Markdown, referenced local assets, reversible content
and asset catalogs, and the converter used to regenerate them.

## Layout

- `content/`: 1,107 generated Markdown documents.
- `assets/`: only assets referenced by generated content.
- `catalog/`: source fingerprints, node/asset mappings, and validation results.
- `tools/Ufcpp.ContentConverter/`: .NET 8 converter.
- `tests/Ufcpp.ContentConverter.Tests/`: focused xUnit tests.
- `docs/`: format and regeneration details.

The raw Umbraco cache, acquisition archives, credentials, activity logs, and extracted source
trees do not belong in this repository.

## Local validation

```powershell
dotnet restore .\UfcppContent.slnx
dotnet build .\UfcppContent.slnx --no-restore
dotnet test .\UfcppContent.slnx --no-build
```

Regeneration requires the locally acquired inputs outside this repository:

```powershell
dotnet run --project .\tools\Ufcpp.ContentConverter -- `
  --snapshot <published-content.xml> `
  --media <extracted-media-root> `
  --sitemap <live-sitemap.xml> `
  --rewrite-maps .\tools\Ufcpp.ContentConverter\data\rewrite_rewritemaps.config `
  --legacy-root <legacy-source-repository> `
  --output .
```

See [docs/regeneration.md](docs/regeneration.md) for the complete offline procedure.

## Copyright

The repository intentionally provides no open-source or content-reuse license. See
[COPYRIGHT.md](COPYRIGHT.md).
