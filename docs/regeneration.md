# Historical content conversion

`Ufcpp.ContentConverter` was a one-time offline migration tool that converted the acquired
Umbraco snapshot into this repository's Markdown, assets, and catalogs. The migration is
complete, and the converter is no longer part of the current source tree or build.

The final converter implementation, migration tests, and original regeneration instructions
are preserved in the annotated Git tag `archive/content-converter`. The tag points to commit
`333a2463b3c6858a1b4f68e2426856d0157d1581`.

To inspect that snapshot without changing the current worktree:

```powershell
git fetch origin tag archive/content-converter
git worktree add --detach ..\UfcppContent-content-converter archive/content-converter
```

The archived converter replaced `content/`, `assets/`, and `catalog/` below its output directory.
Do not use it for current authoring. Add and update content directly as described in
[adding-content.md](adding-content.md).

The archived IIS rewrite map remains in the current tree solely as input for
`LegacyUrlCoverageTests`, which protects the legacy redirects still published by the site.
