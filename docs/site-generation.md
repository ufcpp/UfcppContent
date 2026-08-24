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

Alias pages are written for every `aliases` entry in front matter. Each alias page redirects
with a small inline script that carries the incoming fragment across
(`location.replace(target + location.hash)`), so legacy links such as
`/csharp/oo_interface.html?p=6#static-abstract` still land on their anchor in the single-page
output. The legacy `?p=` page-number query is intentionally dropped, and a `<noscript>`
meta-refresh keeps the redirect working without scripting.
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
framed white body, without restoring a separate sidebar. Previous and next links appear
after navigable chapter content (`Article` and `ExerciseList`) in subject tree order
(`sort_order` at each level). Crossing a chapter boundary prefixes the target title with
`【章名】`; the first and last page in each subject show only the available direction.

## Canonical URLs and legacy redirects

Every page is canonically served from its directory URL,
`/study/<subject>/<chapter>/<slug>/`. No page is canonically served from a legacy
`.html` path, and the trailing-slash-less form resolves to the same output file, so
the host serves it without a redirect.

Articles published before 2014 additionally had a flat legacy URL,
`/study/<subject>/<slug>.html`. Those paths are listed in the page's `aliases` and get a
fragment-preserving redirect page with `rel=canonical` pointing at the directory URL, so old
links and bookmarks keep working.

Published aliases intentionally exclude `/study`-less paths
(`/csharp/st_basis.html`) and extension-less variants of `.html` paths
(`/study/csharp/st_basis`) that were derived only for the original migration. Genuine
legacy URLs outside `/study/`, such as `/lecture/index.html`, remain published when no
`/study`-prefixed counterpart exists on the same page.

`LegacyUrlCoverageTests` loads the committed content through `PageLoader` and verifies it
offline against
`tests/Ufcpp.SiteGenerator.Tests/data/rewrite_rewritemaps.config`: every rewrite-map key
and value is served by some page, no canonical URL ends in `.html`, and no published alias
is a derived form. The retired conversion implementation that originally produced these
aliases is preserved in Git tag `archive/content-converter`.

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
- Every alias resolves to an existing output file, so no redirect is silently missing
- Page-relative page, generated-file, and asset links resolve to actual output files
- Generated pages contain no root-relative internal URLs, so the same output can be
  hosted at the origin root or beneath a path prefix
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
│   ├── NavigationItem.cs        Breadcrumb, keyword, and previous/next link
│   ├── TableOfContentsItem.cs   Nested article heading link
│   └── RenderedContent.cs       HTML plus extracted article navigation
├── Rendering/
│   ├── MarkdigRenderer.cs     Markdig pipeline + AST link rewriting + HTML rendering
│   └── LinkRewriter.cs        Resolves content links to page-relative public URLs
├── Templates/
│   ├── SiteLayout.razor         Full HTML page layout (Razor Component)
│   ├── TableOfContentsList.razor Recursive nested heading list
│   └── _Imports.razor           Razor global imports
├── Output/
│   ├── OutputPathResolver.cs  source_url → output file path
│   ├── SiteUrlResolver.cs     Root-relative target → portable page-relative URL
│   ├── AssetCopier.cs         Copies assets/ and site CSS
│   ├── RedirectWriter.cs      Generates fragment-preserving redirect pages for aliases
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

Two ufcpp.net components only worked with JavaScript, which this site does not ship.
`LegacyControlRewriter` rewrites their Markdown source into markup CSS alone can
drive, right after fenced code blocks have been replaced by placeholders and before
`markdown="1"` containers are expanded — so HTML that only appears inside a code
sample is never touched:

- `<span class="expand-button">` followed by `<div class="expand-panel">` becomes
  `<details class="expand-panel">` / `<summary class="expand-button">` around a
  `<div class="expand-panel-body" markdown="1">`. The panel starts closed, matching
  ufcpp.net.
- `<div class="tab-container">` gains one `<input type="radio">` per tab ahead of its
  `<ul>`, and each `<li>` body is wrapped in a `<label for>`. `site.css` switches
  panels with `:checked ~`. Names and IDs are `ufcpp-tab-{set}-{tab}`, skipped past
  any ID the page already uses, so the output stays deterministic and collision-free.

Markup that does not match the expected shape — a panel with no button, or a tab set
with more tabs than `LegacyControlRewriter.MaxSwitchableTabs` — is left untouched and
keeps the earlier static rendering, so no content can become unreachable.

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

Fenced code blocks are highlighted at build time. C# uses Roslyn
(`Microsoft.CodeAnalysis.Classification.Classifier`) with preview language parsing,
so current contextual keywords and semantic symbol categories such as record types,
methods, properties, parameters, and locals receive Visual Studio-style colors.
Other supported languages use ColorCode. Build-time highlighting keeps generated
pages deterministic and self-contained; unlike browser-side highlighting, it
requires no JavaScript or CDN request.

The renderer supports these canonical language names:

- `csharp`, `xml`, `html`, `css`, `powershell`, `cpp`
- `vbnet`, `fsharp`, `json`, `sql`, `java`, `python`
- `javascript`, `typescript`

Common aliases are normalized (`cs`/`c#`, `ps1`, `c++`, `vb`, `fs`, `py`, `js`,
and `ts`). Case is ignored. The frequently used `console`, `text`, `cil`, and
`shell` blocks intentionally remain plain text. A block with no language or an
unknown language is emitted as escaped plain code so its contents cannot be
interpreted as HTML or be lost. ColorCode failures also fall back to escaped
plain code.

Fenced blocks can carry editorial highlighting metadata in their generic
attributes:

````markdown
```csharp {title="Register the click handler" highlight-lines="2,5-7" error-text="missingName" warning-lines="9"}
// Code
```
````

`title` adds an escaped `title` attribute to the generated `<pre>` element, so
desktop browsers expose the same native hover tooltip as the legacy site. It
must be non-empty and cannot contain control characters. When it is omitted,
the renderer does not emit a `title` attribute. Metadata text is HTML-decoded
exactly once; canonical metadata encodes `&`, `"`, `<`, `>`, and backticks
so quote-rich titles and literal entity spellings remain unambiguous.
`highlight-lines` accepts
comma-separated, one-based whole-line numbers and inclusive ranges. Blank lines
count toward the numbering; the terminating line break after each selection stays
outside the generated highlight. `highlight-text` uses case-sensitive ordinal
literal matching and highlights every occurrence, including overlapping occurrences.
When both highlight attributes select the same source, their source ranges are
combined and overlapping or adjacent ranges are merged.

Legacy compiler annotations use parallel `error-lines`, `error-text`,
`error-ranges`, `warning-lines`, `warning-text`, and `warning-ranges`
properties. Error/warning text must occur exactly once; repeated text uses the
fingerprinted range form instead of guessing an occurrence. Within one kind,
overlapping or adjacent intervals are merged. Different kinds stay distinct
even at the same boundaries.

Compiler/analyzer IDs use selection-level `error-diagnostics` and
`warning-diagnostics` metadata rather than a block-level title:

````markdown
```csharp {error-ranges="sha256:…;1:1-1:6" error-diagnostics="sha256:…;CS0219@1:1-1:2,CS0453@1:1-1:6"}
// Code
```
````

Each entry is `CS####@range` or `CA####@range`; the fingerprint and one-based
Unicode-scalar/end-exclusive coordinates match `*-ranges`. Entry order
preserves legacy outer-to-inner opening order. Identical/nested ranges,
different IDs on one range, and duplicate same-ID occurrences remain separate;
crossing same-kind ranges and non-CS/CA titles are invalid.

Legacy highlights that cannot be expressed exactly by a whole line or a unique
literal use a fingerprinted positional fallback:

````markdown
```text {highlight-ranges="sha256:8f434346648f6b96df89dda901c5176b10a6d83961dd3c1ac88b59b2dc327aa4;1:1-1:3"}
hi
```
````

The formal grammar shared by all positional annotation properties is:

```text
highlight-ranges = range-value
error-ranges     = range-value
warning-ranges   = range-value
range-value      = "sha256:" 64-lowercase-hex ";" range *("," range)
range            = position "-" position
position         = positive-decimal ":" positive-decimal
```

`64-lowercase-hex` is exactly 64 lowercase hexadecimal digits.
`positive-decimal` is greater than zero and has no sign or leading zero; the
grammar contains no whitespace.

Each range is `startLine:startColumn-endLine:endColumn`, with one-based lines
and Unicode-scalar columns and an exclusive end. Multiple ranges are
comma-separated. CRLF and bare CR count as one logical line break; tabs count as
one scalar and are not expanded.

The fingerprint input is exactly the string consumed by the renderer from
`FencedCodeBlock.Lines.ToString()`: Markdig's parsed code content only, without
the opening or closing fence, language/info string, or attributes. Entity
spellings and all spaces, tabs, and blank content lines remain literal. The line
break that merely terminates the last code line before the closing fence is not
part of this value. An additional empty content line is significant, however:
the example above produces `hi`, while an empty line between `hi` and the
closing fence produces `hi\n`.

Before hashing, every CRLF pair and lone CR in that string is replaced with one
LF. The normalized string is encoded with UTF-8 and the resulting bytes are
passed directly to SHA-256; no byte-order mark or other prefix is included. A
minimal equivalent C# recipe is:

```csharp
using System;
using System.Security.Cryptography;
using System.Text;

static string Fingerprint(string markdigCodeValue)
{
    var normalized = markdigCodeValue
        .Replace("\r\n", "\n", StringComparison.Ordinal)
        .Replace('\r', '\n');
    var bytes = Encoding.UTF8.GetBytes(normalized);
    return Convert.ToHexString(SHA256.HashData(bytes)).ToLowerInvariant();
}

Console.WriteLine(Fingerprint("hi"));
// 8f434346648f6b96df89dda901c5176b10a6d83961dd3c1ac88b59b2dc327aa4
```

Editing any part of a range-annotated block makes stale metadata fail the build
rather than silently moving a highlight. Values must use the single canonical
ordering and spelling: ranges are non-empty, in bounds, strictly increasing,
disjoint, and non-adjacent.

All three `*-ranges` properties share the same grammar and compose with their
kind's line selection. Error/warning text and range properties are mutually
exclusive; existing highlight text/range composition remains supported. The
renderer splits at every syntax and annotation boundary and
uses the fixed outer-to-inner order `<mark class="code-highlight">`,
error spans, warning spans, then the syntax-color span. Multiple active titled
spans of one kind are nested in diagnostic-list order. The renderer retains
common outer wrappers across inner boundaries; partial different-kind overlaps
are split into valid nested runs rather than crossing tags.
The named-property allowlist contains only `title`, the three line/text/range
triples, and the error/warning diagnostic identity lists; event handlers and
other named generic attributes fail site generation instead of reaching
generated HTML. Author-supplied generic IDs and classes on fenced code blocks
also fail generation. Markdig-generated attributes on its derived block
extensions are ignored rather than copied.

Annotation metadata is consumed by the renderer and is never copied into
generated HTML. Malformed attributes, incompatible text/range pairs, ambiguous
error/warning text, invalid line syntax, non-positive or out-of-range line
numbers, an empty literal, a literal with no match, a stale range fingerprint,
invalid Unicode, or non-canonical range coordinates fail site generation
explicitly. Raw table code retained as HTML uses the same permanent mark/error/
warning elements; migration verifies that decoded visible code and all
non-annotation markup remain unchanged. Before inserting fixed annotation
elements, the renderer parses the trusted syntax-highlighter fragment
structurally and verifies that its text maps exactly to the source code. This
preserves syntax-token spans and escaped plain code without rewriting generated
HTML with regular expressions.

Rendered diagnostic spans use the browser-native tooltip only:
`<span class="error|warning" title="CS####|CA####">`. They receive no event,
data, style, ARIA, role, ID, anchor, focus, or JavaScript attributes. Untitled
diagnostic spans remain class-only and retain the existing visual merging.

Source-code highlights reproduce the legacy effective style: `#e0ffff`
background, `0 2px` padding, normal font style, and bold text while nested syntax
token colors remain intact. Syntax keyword classifications inherit the base code
weight instead of using bold, matching the current ufcpp.net `.reserved` style.
Console highlights retain white text on `#606060`, normal weight, and the legacy
`1px solid #ff8080` bottom border.
Error annotations retain the legacy dotted medium red (`#f00`) underline and
warnings the dotted medium green (`#008000`) underline, scoped to
`.content pre code` so unrelated legacy classes are unaffected.

Roslyn classifications are emitted as scoped `roslyn-*` classes. Same-position
and overlapping classifications are merged by text interval so embedded
classifications cannot duplicate or drop source characters.

To add a non-C# language, add its canonical name and aliases to `LanguagesByName`
in `Rendering/SyntaxHighlightingExtension.cs`, add scoped token colors to
`wwwroot/css/site.css` when its token classes are new, and add a representative
case to `MarkdigRendererTests`.

### Stylesheet and CSS parity

The site ships a single stylesheet, `tools/Ufcpp.SiteGenerator/wwwroot/css/site.css`,
copied to `_site/assets/css/site.css` at build time.

Article bodies still carry the legacy HTML classes inherited from ufcpp.net
(`version13`, `pros-mark`, `table.layout`, …), so `site.css` must define them for
the content to render as intended. [docs/css-parity.md](css-parity.md) records how
that reconciliation is verified, which classes are deliberately left unstyled, and
where this site intentionally departs from the original — including the
JavaScript-free rebuild of ufcpp.net's expand panels and language tabs.

Two checks back it up, plus `SiteCssParityTests`:

```bash
pwsh -NoProfile -File ./tools/css-class-reconciliation.ps1   # class coverage
node tools/css-parity-compare.mjs tools/css-parity-cases.json # computed styles
```

### Link rewriting

Internal links are rewritten at the Markdig AST level (before HTML rendering):

- Relative `.md` links are resolved through canonical site paths and then made relative
  to the current public page
- Relative `assets/` paths are emitted as page-relative links into the copied
  `assets/` tree
- Query strings and fragments are retained while the path is rewritten
- Protocol-relative external URLs remain external
- Existing root-relative site links are made page-relative
- Absolute `ufcpp.net` links are made page-relative when they identify a generated
  page, alias, artifact, or copied asset; illustrative endpoints remain absolute
- Existing root-relative legacy media references are mapped to copied assets and made
  page-relative

Asset lookup uses the directory supplied through `--assets`; it does not assume the
directory is a sibling named `assets`.

Raw HTML `href`, `src`, and `data` attributes plus Silverlight source parameters in
the rendered output are rewritten with bounded regular expressions (max 2 048 chars
per attribute value, 5 s timeout) to catch links in raw HTML blocks.

The page shell and alias redirects use the same relative URL resolver. As a result, one
generated `_site/` tree works both at `https://ufcpp.net/` and at a project-site prefix
such as `https://ufcpp.github.io/UfcppContent/`; no deployment-specific base-path option
or HTML `<base>` element is required. Absolute canonical URLs, sitemap entries, and RSS
entry URLs continue to identify `https://ufcpp.net/`.

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
