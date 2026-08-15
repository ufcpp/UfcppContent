# ufcpp.net content

This repository is the local, deterministic Markdown archive of the published ufcpp.net
content snapshot. It contains Markdown maintained as the current source of truth, referenced
local assets, reversible content and asset catalogs, and the static site generator.

## Layout

- `content/`: 1,107 generated Markdown documents.
- `assets/`: only assets referenced by generated content.
- `catalog/`: source fingerprints, node/asset mappings, and validation results.
- `tools/Ufcpp.SiteGenerator/`: .NET 10 static site generator.
- `tests/Ufcpp.SiteGenerator.Tests/`: focused xUnit tests.
- `docs/`: format and regeneration details.

The raw Umbraco cache, acquisition archives, credentials, activity logs, and extracted source
trees do not belong in this repository.

## Adding content

See [docs/adding-content.md](docs/adding-content.md) for the procedure to add blog entries and
articles directly as Markdown.

## Local validation

```powershell
dotnet restore .\UfcppContent.slnx
dotnet build .\UfcppContent.slnx --no-restore
dotnet test .\UfcppContent.slnx --no-build
```

## Historical conversion

The one-time `Ufcpp.ContentConverter`, its tests, and the original offline regeneration
instructions were retired after the Umbraco migration. Their final state is preserved in the
annotated Git tag [`archive/content-converter`](https://github.com/runceel/UfcppContent/tree/archive/content-converter).
They are not part of the current build or authoring workflow.

See [docs/regeneration.md](docs/regeneration.md) for archival access details.

## Copyright

The repository intentionally provides no open-source or content-reuse license. See
[COPYRIGHT.md](COPYRIGHT.md).
