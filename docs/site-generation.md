# Site Generation

`tools/Ufcpp.SiteGenerator` is a .NET 10 console application that converts the Markdown content in `content/` and static files in `assets/` into a fully static HTML site in `_site/`.

```
content/*.md + assets/
    ↓ Ufcpp.SiteGenerator
_site/**/*.html + _site/assets/ + sitemap.xml + rssfeed.xml
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10)

## Generating the site locally

From the repository root:

```bash
dotnet run --project tools/Ufcpp.SiteGenerator -- \
  --content content/ \
  --assets assets/ \
  --output _site/
```

After generation, the `_site/` directory contains:

| Path | Description |
|------|-------------|
| `_site/index.html` | Home page |
| `_site/<path>/index.html` | Content pages (directory URLs) |
| `_site/assets/` | Static assets copied verbatim |
| `_site/assets/css/site.css` | Site stylesheet |
| `_site/sitemap.xml` | XML sitemap |
| `_site/rssfeed.xml` | RSS 2.0 feed (latest 30 blog entries) |
| `_site/server.cs` | Optional .NET 10 file-based preview server |

The Google site-search approach is described in
[Static site search design](site-search.md).

Alias pages (meta-refresh redirects) are written for every `aliases` entry in front matter.
Rebuilding updates existing redirects while aliases that resolve to their own canonical
output leave the primary page intact.
Each build is generated and validated in a fresh sibling directory, then replaces the
previous output only after it succeeds. Deleted pages, aliases, and assets therefore do
not remain published, while a failed rebuild leaves the last successful output intact.
The generated layout includes the global header and footer, content-type-specific
metadata, and a single article column that uses the full available reading width at
every viewport size. Non-home pages receive breadcrumbs derived from their
`parent_id` hierarchy. Study articles also reproduce the original publication/update
line, nested table of contents, and keyword links on the page background above the
framed white body, without restoring a separate sidebar.

## Previewing locally

Generate the optional single-file .NET 10 preview server:

```bash
dotnet run --project tools/Ufcpp.SiteGenerator -- \
  --content content/ \
  --assets assets/ \
  --output _site/ \
  --include-preview-server

cd _site
dotnet run server.cs
```

Then open <http://localhost:8080/>. The `server.cs` file is omitted unless
`--include-preview-server` is specified.

You can instead use any other static file server:

```bash
# Python (built-in)
cd _site
python3 -m http.server 8080

# dotnet-serve (install once: dotnet tool install -g dotnet-serve)
dotnet serve -d _site -p 8080
```

## CLI options

| Option | Default | Description |
|--------|---------|-------------|
| `--content <dir>` | `content/` | Directory of Markdown source files |
| `--assets <dir>` | `assets/` | Directory of static assets |
| `--output <dir>` | `_site/` | Output directory |
| `--include-preview-server` | off | Write `server.cs` for `dotnet run server.cs` local preview |
| `--noindex` | off | Add `noindex, nofollow` robots metadata to generated pages and redirects |
| `--skip-validation` | off | Skip post-generation link/asset validation |

### Search indexing in preview deployments

The Azure Static Web Apps workflow generates its transient `_site/` output with `--noindex` and
copies `deploy/azure-static-web-apps/staticwebapp.preview.config.json` into the output as
`staticwebapp.config.json`. The generated HTML metadata and the configuration's global
`X-Robots-Tag` header both prevent compliant search engines from indexing the preview.

The preview configuration deliberately does not block crawling with `robots.txt`, because crawlers
must be able to read the `noindex` directive. A production build must omit `--noindex` and must not
copy the preview Static Web Apps configuration.

## Validation

By default, after generation the tool validates:

- Primary pages, aliases, generated artifacts, and copied assets do not claim the
  same output path
- URL-derived output paths cannot escape the output directory or use
  Windows-incompatible path segments
- Every expected page was written
- Root-relative page and generated-file links resolve to actual output files
- URL fragments match an `id` or legacy `<a name="…">` in the target page
- Asset references in `href`, `src`, `data`, and Silverlight
  `<param name="source" value="…">` attributes exist in the output

Copied HTML under `_site/assets/` is deliberately not parsed or modified during
validation, so archived files remain byte-identical. Broken internal links, missing
fragments or assets, and path collisions cause a non-zero exit code and print
diagnostic messages.

Pass `--skip-validation` to skip this step (useful for partial builds or debugging).

## Architecture

```
tools/Ufcpp.SiteGenerator/
├── Program.cs                 CLI entry point
├── SiteBuilder.cs             Orchestrates all generation steps
├── CliOptions.cs              Command-line options model
├── Loading/
│   └── PageLoader.cs          Parses YAML front matter, builds URL map, detects collisions
├── Models/
│   ├── FrontMatter.cs           Typed front-matter model (YamlDotNet)
│   ├── ContentPage.cs           Fully-loaded page model
│   ├── NavigationItem.cs        Breadcrumb and keyword link
│   ├── TableOfContentsItem.cs   Nested article heading link
│   └── RenderedContent.cs       HTML plus extracted article navigation
├── Rendering/
│   ├── MarkdigRenderer.cs     Markdig pipeline + AST link rewriting + HTML rendering
│   └── LinkRewriter.cs        Resolves relative .md and assets/ links to canonical URLs
├── Templates/
│   ├── SiteLayout.razor         Full HTML page layout (Razor Component)
│   ├── TableOfContentsList.razor Recursive nested heading list
│   └── _Imports.razor           Razor global imports
├── Output/
│   ├── OutputPathResolver.cs  source_url → output file path
│   ├── AssetCopier.cs         Copies assets/ and site CSS
│   ├── RedirectWriter.cs      Generates meta-refresh redirect pages for aliases
│   ├── SitemapWriter.cs       Writes sitemap.xml
│   ├── RssWriter.cs           Writes rssfeed.xml
│   └── PreviewServerWriter.cs Writes optional server.cs
├── Validation/
│   └── OutputValidator.cs     Post-generation validation
└── wwwroot/css/
    └── site.css               Site stylesheet
```

### Markdown rendering

Markdig processes each page with the following extensions:

- Abbreviations, auto-identifiers, citations, custom containers
- Definition lists, figures, footers, footnotes, grid tables
- Mathematics (MathML), media links, pipe tables, list extras
- Task lists, diagrams, auto-links, generic attributes

`EmphasisExtraExtension` (`++text++` / `~~text~~`) is **not** enabled to avoid
misinterpreting `++C++` (a C-related joke in the site name) as `<ins>C</ins>`.

Raw HTML blocks in Markdown are preserved verbatim. Legacy `markdown="1"` content
inside `blockquote`, `div`, `th`, and `td` elements is rendered recursively, including
nested raw tables. This keeps indented table rows as table markup instead of turning
them into Markdown code blocks.

After HTML rendering, the first `h1` is separated, including when it is inside a
wrapper, so contextual navigation can be placed directly below the page title. On
`Article` pages, `h2` through `h4` headings form the nested table of contents.
Explicit legacy anchor IDs are preferred over generated IDs, preserving stable links
from the original site. Headings without an anchor receive a collision-free generated
ID. When a reused ID would resolve to an earlier element, a target-specific generated
anchor keeps the table-of-contents or keyword link unambiguous. Elements with both an
`id` and a `keyword` class form the visible keyword list; duplicate keyword IDs are
emitted only once. Other content types keep their original body without article-only
indexes.

### Syntax highlighting

Fenced code blocks are highlighted at build time with ColorCode. Build-time
highlighting keeps generated pages deterministic and self-contained; unlike
browser-side highlighting, it requires no JavaScript or CDN request.

The renderer supports these canonical language names:

- `csharp`, `xml`, `html`, `css`, `powershell`, `cpp`
- `vbnet`, `fsharp`, `json`, `sql`, `java`, `python`
- `javascript`, `typescript`

Common aliases are normalized (`cs`/`c#`, `ps1`, `c++`, `vb`, `fs`, `py`, `js`,
and `ts`). Case is ignored. The frequently used `console`, `text`, `cil`, and
`shell` blocks intentionally remain plain text. A block with no language, an
unknown language, or a highlighting failure is also emitted as escaped plain
code so its contents cannot be interpreted as HTML or be lost.

To add a language, add its canonical name and aliases to `LanguagesByName` in
`Rendering/SyntaxHighlightingExtension.cs`, add scoped token colors to
`wwwroot/css/site.css` when its token classes are new, and add a representative
case to `MarkdigRendererTests`.

### Link rewriting

Internal links are rewritten at the Markdig AST level (before HTML rendering):

- Relative `.md` links → canonical site paths (e.g. `../foo.md` → `/study/csharp/foo/`)
- Relative `assets/` paths → root-relative `/assets/…` URLs
- Query strings and fragments are retained while the path is rewritten
- Protocol-relative external URLs remain external
- Existing root-relative legacy media references are mapped to copied assets

Asset lookup uses the directory supplied through `--assets`; it does not assume the
directory is a sibling named `assets`.

Raw HTML `href`, `src`, and `data` attributes plus Silverlight source parameters in
the rendered output are rewritten with bounded regular expressions (max 2 048 chars
per attribute value, 5 s timeout) to catch links in raw HTML blocks.

### Output determinism

The generator produces byte-identical output for identical input:

- Pages are processed in case-insensitive alphabetical order of their relative path
- Sitemap entries are ordered by canonical path
- RSS feed entries are ordered by `published_at` descending and formatted in UTC
- Percent-encoded URL segments remain encoded in public URLs but are decoded and
  validated for portable filesystem output

## Running tests

```bash
dotnet test tests/Ufcpp.SiteGenerator.Tests/
```
