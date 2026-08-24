# Code annotation migrator specification

## Status and lifetime

`Ufcpp.CodeAnnotationMigrator` is temporary, repository-local migration
infrastructure for restoring the code-block annotations tracked by Issues #4
and #5. It is intentionally independent from the retired
`Ufcpp.ContentConverter`: the old converter needs unavailable external inputs,
and its code-block normalization discarded the annotation data this migration
must recover.

The source of truth is the repository tree at commit
`eacf0d470a684771524fb04f710951d38a60cc74`, where the legacy `<pre>` blocks
still contain their editorial annotations. PR 1 is analysis-only. PR 2 adds the
Issue #4 title/highlight migration described below while retaining the same
stdout-only trust boundary. The tool never has an `--apply` mode. PR 3 handles
Issue #5 error/warning annotations, and PR 4 removes this tool.

## Inputs

The command accepts:

- `--repo-root <path>`: a local Git worktree root. The default is the current
  directory.
- `--source-commit <revision>`: a commit resolvable by the local repository.
  The default is the pinned full commit above. The report records the resolved
  full object ID.
- `--source-path <relative-path>`: the historical Markdown tree within the
  source commit. The default is `content`.
- `--current-path <relative-path>`: the current Markdown tree within the
  current `HEAD` commit. The default is `content`.
- `--report -`: deterministic JSON on standard output. `-` is the default and
  only accepted destination.
- `--dry-run`: optional and accepted for clarity. Dry run is the only mode.
- `--issue 4`: select the Issue #4 title/highlight contract. Omitting it retains
  the PR 1 all-annotation analysis.
- `--format <report|patch>`: select deterministic JSON or a deterministic
  unified patch. The default is `report`. `patch` requires `--issue 4`.

Unknown options, positional arguments, `--apply`, missing option values, rooted
source/current paths, `..` traversal, and every `--report` value other than `-`
are input errors. File output is categorically disabled: aliases, hard links,
mount/share namespaces, Git administration paths, associated worktrees, and
concurrent topology changes therefore cannot turn reporting into a repository
write. Callers that need a file capture standard output outside the tool.

Before analysis, the tool:

1. resolves and validates the worktree root with `git rev-parse`;
2. resolves both the requested source revision and current `HEAD` to full
   commit object IDs;
3. verifies that `<commit>:<source-path>` exists as a tree;
4. verifies that `<HEAD>:<current-path>` exists as a tree and that every
   worktree path component from the repository root to `current-path` is a
   normal directory rather than a symbolic link or junction;
5. rejects tracked, untracked, or ignored Markdown changes below the current
   content path, rejects `assume-unchanged`/`skip-worktree` index flags, and
   repeats the `HEAD` and cleanliness checks after all objects are read; and
6. reads historical and current Markdown only from the two resolved commit
   trees with `git ls-tree` and `git cat-file`.

Git is invoked only with read-only commands. Every subprocess disables
replacement objects, optional locks, lazy fetching, terminal prompts, file
system monitoring, automatic maintenance, and fetch commit-graph writes.
Before those explicit safe values are added, every inherited environment
variable whose name starts with `GIT_` is removed case-insensitively. This
includes alternate indexes, trace destinations, replacement/config injection,
object directories, and every numbered `GIT_CONFIG_*` field. Required objects
must already exist locally. The tool never checks out, updates, stages, fetches,
or writes a repository file.

## Document and block enumeration

Only `.md` files are considered. Paths are relative to the configured tree,
use `/` separators, and are ordered with ordinal string comparison.

Historical documents are read from the pinned commit. Every HTML `<pre>`
element is enumerated in source order, including `<pre>` elements nested in
raw HTML tables. A `<code>` wrapper inside `<pre>` is structural and is not a
separate block. Inline HTML or Markdown `<code>` outside `<pre>` is excluded.

Current documents are read from the resolved `HEAD` commit, not mutable
worktree files. The following are merged by source offset to produce one
document-order sequence:

- closed backtick or tilde fenced code blocks recognized by Markdig; and
- raw HTML `<pre>` elements, including those inside raw HTML tables.

Raw `<pre>` text appearing inside fenced, indented, or inline Markdown code is
excluded. A raw block records whether it is inside a `<table>`.

Each block records its one-based document ordinal, one-based source line,
original code text, normalized code, SHA-256 normalized-code hash, and kind.
The report never embeds whole source blocks.

## Legacy annotation extraction

The historical parser decodes HTML character and numeric entities in both
visible text and attributes. Markup nested inside annotations contributes its
visible decoded text.

For each historical `<pre>`:

- a non-whitespace `title` attribute is retained as title metadata;
- each `<em>` element is retained as a highlight selection;
- each `<span>` whose class token is exactly `error` is retained as an error
  selection;
- each `<span>` whose class token is exactly `warning` is retained as a
  warning selection; and
- all other tags, including syntax-color spans such as `reserved`, `literal`,
  `type`, `method`, and `comment`, affect neither metadata kind nor output
  text. Their visible text still contributes to code.

A structural `<code>...</code>` wrapper may be surrounded by whitespace inside
`<pre>`; only that wrapper boundary is excluded. A plain `<pre>` without the
wrapper preserves its complete original body bounds, including leading
whitespace before the first code line. Matching normalization, not parsing,
removes framing blank lines and common indentation.

Tag and attribute names and class tokens are compared case-insensitively.
Malformed or unbalanced `<pre>`, `<code>`, `<em>`, `error`, or `warning`
markup is an explicit diagnostic, never a partially parsed success. An orphan
`</pre>` outside a parsed block is also an explicit malformed historical case;
it does not invent a historical block or alter block coverage.

## Matching normalization

Original historical and current code text is retained for diagnostics and
conversion planning. Normalization is used only to build the matching hash.
It applies identically to both sides in this exact order:

1. decode HTML character and numeric entities;
2. convert CRLF and bare CR to LF;
3. remove spaces and tabs at the end of every line;
4. remove leading and trailing blank lines;
5. find the longest exact common prefix consisting only of spaces and tabs
   across all non-empty lines and remove it from every line; tabs are never
   expanded or considered equivalent to spaces; and
6. join lines with LF and no terminal LF.

Whitespace inside a non-empty line is otherwise significant. SHA-256 is
computed over the UTF-8 bytes of the normalized string and rendered as
lowercase hexadecimal.

## Exact matching

Blocks only match within the same exact relative document path. Fuzzy text,
edit distance, language, title, and neighboring prose are never matching
signals.

Matching has two deterministic passes:

1. **Ordinal and hash:** a historical and current block with the same
   one-based document ordinal and normalized hash match.
2. **Unique hash fallback:** among still-unmatched blocks in that path, a hash
   matches only when it occurs exactly once on each side. This safely handles
   an insertion or deletion that shifts later block ordinals.

For a hash duplicated on either side, ordinal matches are accepted only when
the complete ascending ordinal lists for that hash are identical. If duplicate
counts or ordinal lists differ, every historical occurrence of that hash is
ambiguous, even if one occurrence happens to retain its old ordinal. This
prevents an insertion from making one duplicate appear to match while swapping
the annotations of otherwise identical blocks. Occurrence proximity or
duplicate order is not used to guess.

If a remaining historical hash has no current candidate, it is unmatched. A
current-only block is reported separately and is not evidence of lost
historical metadata. Ambiguous and unmatched historical blocks are error
diagnostics even when they carry no annotation, so a successful exit cannot
hide partial block coverage.

A hash match is safe only when the target representation is also correct:
historical blocks outside tables must match current fenced blocks, while
historical blocks inside tables must match raw current `<pre>` elements that
remain inside a table. A kind or table-context mismatch is unrepresentable and
forces an error exit.

## Metadata conversion planning

PR 1 reports a plan; it does not edit Markdown. The planned contract is:

- `title`;
- `highlight-lines` and `highlight-text`;
- `error-lines` and `error-text`; and
- `warning-lines` and `warning-text`.

No character-range representation is introduced.

Selections are mapped to the matched current block. A selection that covers
one or more complete current lines is represented by one-based line numbers.
Adjacent numbers are collapsed to inclusive ranges, and disjoint ranges are
comma-separated in ascending order. Multiple whole-line selections of the same
kind share the one `*-lines` value.

After whole-line selections are removed, a kind may have at most one partial
selection. It is represented by `*-text` only when:

- the selected text is non-empty;
- it contains no CR or LF;
- it occurs exactly once, ordinally and case-sensitively, in the current code;
  and
- the occurrence maps exactly to the historical selection after newline
  normalization, including the same occurrence ordinal after matching entity
  normalization.

Planning uses the raw current fenced-code text, not the entity-decoded matching
form, because the future metadata attributes are evaluated against source
text. Line metadata uses physical current lines. A match that exists only after
entity decoding, or whose entity decoding changes the physical line layout, is
unrepresentable.

Multiple partial selections, multiline partial selections, missing selected
text, overlapping selections of different locations, or repeated selected
text are unrepresentable. They produce explicit diagnostics rather than a
guess. A matched block with any unrepresentable metadata remains a block match
but is unsafe for automatic application.

## Issue #4 migration contract

Issue #4 scope contains only legacy non-empty `title` attributes and `<em>`
selections. Error and warning annotations remain deliberately out of scope for
PR 2. Their unresolved PR 1 diagnostics cannot be presented as Issue #4
failures or silently migrated by the Issue #4 patch.

For fenced blocks, PR 2 preserves and prefers the existing representations in
this order:

1. `title` for a historical title;
2. `highlight-lines` for every selection that maps to one or more complete
   current lines;
3. `highlight-text` when the remaining partial selection is a non-empty,
   single-line, case-sensitive ordinal literal that occurs exactly once at the
   proven semantic position; and
4. `highlight-ranges` only when the remaining selections cannot be represented
   exactly by the preceding forms.

`highlight-text` and `highlight-ranges` are mutually exclusive in canonical
migrator output. `highlight-lines` may accompany either one. All metadata
properties are emitted in the order above, separated by one ASCII space.
Values always use double quotes. Before serialization, `&`, `"`, `<`, `>`, and
the backtick are replaced, in that order, by `&amp;`, `&quot;`, `&lt;`,
`&gt;`, and `&#96;`. The renderer HTML-decodes the attribute value exactly
once before title validation or literal matching. Encoding ampersand first
therefore preserves a literal entity spelling such as `&lt;` as
`&amp;lt;`, while `&#96;` keeps title text containing a backtick valid on a
backtick fence. Apostrophes, backslashes, and braces are literal. Existing
metadata is decoded once, verified, and rewritten to this canonical form. A
duplicate or conflicting existing property is an error; the migrator never
appends a second copy.

### Highlight range syntax

The exact grammar is:

```text
highlight-ranges = "sha256:" 64-lowercase-hex ";" range *("," range)
range            = position "-" position
position         = positive-decimal ":" positive-decimal
```

The first number in a position is a one-based logical line. The second is a
one-based Unicode scalar-value column. Column 1 is before the first scalar and
`scalar-count + 1` is immediately after the last scalar. UTF-16 code-unit
indexes, grapheme clusters, display cells, tab expansion, and culture never
affect columns. Unpaired UTF-16 surrogates are invalid.

The end position is exclusive. A range may cross logical lines and then
includes the intervening line break. Newline-only ranges are invalid. CRLF,
bare CR, and LF are logical line separators for coordinate mapping. The
physical source offsets used to insert `<mark>` elements retain the original
separator bytes. The line table is shared with `highlight-lines`; a terminal
line separator does not invent an additional addressable empty line.

The SHA-256 prefix is the lowercase hexadecimal digest of the UTF-8 encoding of
the exact `FencedCodeBlock.Lines.ToString()` value after only CRLF and bare-CR
are converted to LF. It includes every other character, space, tab, blank line,
entity spelling, and any terminal newline present in that Markdig value. The
migrator and renderer use the same Markdig version and shared implementation.
The digest guards the entire code block, so a coordinate cannot silently become
stale after an unrelated edit.

Ranges must be non-empty, in bounds, strictly increasing, disjoint, and
non-adjacent in logical source order. Decimal numbers have no sign, whitespace,
or leading zero. The digest and hexadecimal spelling are exact. The planner
sorts selections and merges overlapping or adjacent same-kind spans before
serialization. A hand-written duplicate, overlap, adjacency, alternate order,
uppercase digest, stale digest, or otherwise non-canonical value fails site
generation.

Range spans are unioned with spans from `highlight-lines` or `highlight-text`
before rendering. The existing structural renderer splits syntax-color spans
at source boundaries, wraps the selected runs in
`<mark class="code-highlight">`, and verifies before and after insertion that
the visible code text is unchanged. A range endpoint inside a Unicode scalar
or an HTML entity projection is unrepresentable and is never rounded.

### Exact historical-to-current projection

A normalized-code hash is a block identity check, not a positional mapping.
Before emitting a range, the migrator builds an explicit boundary projection
through the PR 1 normalization operations:

1. historical parser output and current Markdig code are split into physical
   lines while retaining source offsets;
2. HTML entity decoding records which source interval produced every decoded
   scalar boundary;
3. newline canonicalization, trailing-space removal, framing blank-line
   removal, and common-indent removal retain boundary maps into the normalized
   text;
4. equal normalized text aligns equal scalar boundaries; and
5. each historical selection boundary is projected back to an exact current
   source boundary and then converted to a range position.

The projection must prove that decoding the exact current source slice produces
the same newline-normalized scalar sequence as the historical selection. A
boundary removed by normalization, inside an entity, inside a surrogate pair,
or mapping to a different occurrence is an error. No fuzzy search, nearest
boundary, occurrence proximity, or visual guess is allowed.

### Raw table blocks

Raw `<pre>` blocks inside tables cannot carry Markdig fenced attributes.
Existing exact `title` attributes are retained; a missing title is added to the
`<pre>` opening tag with HTML attribute escaping. The audited Issue #4 raw
highlights are unique literal selections, so migration inserts
`<mark class="code-highlight">` at the exact guarded text-node boundaries
inside the existing `<code>` wrapper. It does not split an entity or element.
The generated HTML therefore uses the same permanent highlight element as
fenced blocks without enabling or broadening raw HTML.

After each raw edit, the migrator reparses the block and proves that its decoded
visible code text, wrapper kind, table context, and all non-annotation markup
are unchanged. An already-identical `title` or `mark` is verified and skipped.
Any other existing mark, non-unique source mapping, malformed HTML, or missing
`<code>` wrapper blocks patch emission.

### Exception catalog

Issue #4 has a checked-in deterministic exception catalog. Every entry is
guarded by exact relative path, pinned historical ordinal and normalized hash
(or an exact historical document/blob guard for malformed markup), expected
current ordinal and hash when it maps to live content, and a documented reason.
Allowed dispositions are:

- `mapped`: an exact live current target whose identity is proven by the
  catalog guards;
- `obsolete`: a historical annotation whose block was genuinely deleted or
  replaced, with nearby-content evidence recorded in the reason; and
- `malformed-resolved`: malformed legacy markup whose Issue #4 meaning is
  established explicitly from the pinned source.

A stale guard, missing candidate, reused target, unlisted ambiguous/unmatched
annotation, or `blocked` disposition is an Issue #4 error. Overrides never use
approximate text.

The baseline that the catalog must reconcile is:

| Metadata | Total | Exact match | Ambiguous | Unmatched | Range fallback |
|---|---:|---:|---:|---:|---:|
| title | 4,211 | 4,197 | 2 | 12 | n/a |
| highlight | 413 | 247 line/text | 0 | 4 | 162 |

The final audited disposition totals and the six malformed legacy cases are
part of the deterministic Issue #4 report. Patch mode is unavailable until
every title and highlight is either mapped or explicitly obsolete and no
Issue #4 diagnostic remains.

### Rewrite and patch safety

The tool still reads only immutable Git objects and writes only to the supplied
standard-output stream. In Issue #4 report mode it includes the exact target
commit, exception resolutions, representation counts, per-block plans, and
rewrite summary. In patch mode it first computes every postimage in memory,
validates all postimages, and emits nothing unless the complete Issue #4 result
is safe.

The unified patch is UTF-8 without a byte-order mark and uses LF patch
newlines. Files remain LF with their existing final newline. Each file header
contains the exact preimage and postimage Git blob IDs. Hunks contain context
and can change only a fence opening metadata line, a raw `<pre>` opening tag,
or one of the audited raw highlight text nodes. The patch contains the resolved
target commit in a leading comment.

The explicit application procedure is:

1. capture stdout as UTF-8 without a byte-order mark;
2. verify `git rev-parse HEAD` equals the target commit recorded by the report
   and patch;
3. require both index and worktree to be clean;
4. run `git apply --check --index <patch>`; and
5. run `git apply --index --whitespace=nowarn <patch>`.

The `index` preimage guards and `--index` prevent a context-offset match against
a different file version. Git applies the complete checked patch or reports a
failure; the migrator itself never opens a worktree file for writing.

After application and commit, a second Issue #4 report must verify all existing
metadata, and patch mode must emit zero content bytes with exit code 0. A
different existing value is an error, not a second annotation. This is the
idempotency contract.

### Audited repository result

The PR 2 audit found no obsolete or blocked Issue #4 annotation.

The twelve baseline unmatched titles and two ambiguous titles were caused by a
Markdig HTML-block enumeration edge: a whitespace-only line after a legacy
`<div markdown="1">` did not terminate the HTML block, so a real opening fence
was hidden and a later closing fence was misread as an opening. Discovery now
normalizes whitespace-only Markdown separators outside fences while retaining
an exact source-offset map. This restores the original ordinal/hash matches
without an override.

The remaining catalog has seven entries:

- two exact changed-code mappings:
  `study/csharp/cheatsheet/ap_ver7_2.md` (legacy unescaped `<T>`) and
  `study/sp/dsp/frequency.md` (one literal legacy `<em>` pair); and
- five malformed historical blocks with exact document-blob, ordinal, line,
  and current-block hash guards. They recover five titles and one genuine
  highlight. The orphan `</pre>` in
  `blog/2026/3/sourcegeneratordemo/index.md` contains no annotation and is
  classified as benign.

The checked plan reconciles these totals:

| Result | Count |
|---|---:|
| Baseline parsed title blocks | 4,211 |
| Supplemental malformed titles | 5 |
| Restored title blocks | 4,216 |
| Baseline parsed highlight blocks | 413 |
| Supplemental malformed highlights | 1 |
| Restored highlight blocks | 414 |
| `highlight-lines` blocks | 52 |
| `highlight-text` blocks | 206 |
| `highlight-ranges` blocks | 164 (368 ranges) |
| Raw-table highlight blocks | 8 |
| Changed Markdown documents | 461 |
| Obsolete / blocked annotations | 0 / 0 |

The 164 range blocks include all 162 baseline line/text representation
failures plus two recovered multi-selection blocks hidden by the enumeration
bug. One baseline case intentionally selects four trailing spaces. Because
normalization removes that region, it uses a separate line-end anchor only
after proving the historical and current trailing runs are ordinally equal.
No fuzzy or occurrence-based fallback is used.

### Issue #4 acceptance

PR 2 is complete only when:

- all 4,211 historical title blocks and all 413 historical highlight blocks
  have an exact mapped or evidenced-obsolete disposition;
- the 162 baseline line/text failures have exact guarded range plans or an
  explicitly evidenced different disposition;
- all two ambiguous titles, twelve unmatched titles, four unmatched
  highlights, and relevant malformed/current-only cases are cataloged without
  fuzzy matching;
- the deterministic report has no unexplained Issue #4 diagnostic and patch
  generation is all-or-nothing;
- the applied content diff changes annotation metadata only and reparsing proves
  every code block's visible text is unchanged;
- a second committed-tree run emits an empty patch;
- focused renderer and migrator tests cover canonical serialization, line/text
  preference, repeated and multiple partial selections, multiline ranges,
  Unicode scalar columns, CRLF/LF mapping, stale/bounds/canonical failures,
  syntax-span intersections, duplicates and explicit overrides, raw tables,
  deterministic patching, failure non-mutation, and idempotency; and
- the full solution build/tests, content validation, site generation, and
  representative generated-HTML inspection pass.

## Report

The report is UTF-8 without a byte-order mark, uses LF newlines, is
pretty-printed JSON with one final LF, and has schema version 2 after the
addition of `ranges` to selection plans. The PR 1 baseline was schema version
1. It contains no timestamp, absolute path, machine name, elapsed time, or
report destination.

Top-level sections are emitted in this order:

1. `schemaVersion` and `mode`;
2. resolved `source` and `target`, each with a full commit object ID and
   relative tree path;
3. the normalization and matching policy identifiers;
4. document and block totals;
5. coverage counters;
6. representable metadata plans; and
7. diagnostics.

Coverage has separate counters for `title`, `highlight`, `error`, `warning`,
historical blocks expected to become `fencedBlocks`, and historical raw
`rawTableBlocks` that must remain inside HTML tables. Every counter has `total`,
`matched`, `ambiguous`, `unmatched`, and `unrepresentable` fields. For metadata,
the unit is a historical block containing that metadata kind. For block kinds,
the unit is a historical block. Totals additionally distinguish current raw
`<pre>` blocks outside tables from raw `<pre>` blocks inside tables and record
the number of malformed historical cases. A malformed block contributes to the
appropriate block-kind `unrepresentable` count; an orphan closing tag does not
represent a block. Metadata that cannot be parsed safely is not guessed into a
metadata counter and is instead identified by its diagnostic.

Plans are ordered by path, historical block ordinal, and metadata kind.
Diagnostics are ordered by path, historical block ordinal, current block
ordinal, diagnostic code, and metadata kind. Each diagnostic includes a stable
code, severity, path, relevant source and target locations/ordinals, hash when
available, metadata kind when applicable, and an actionable message.

Two runs against identical trees and arguments must produce byte-identical
report bytes.

## Diagnostics and exit codes

- `0`: analysis completed with every historical block matched and every
  annotation representable.
- `1`: an unexpected process or I/O failure prevented a report.
- `2`: usage or input validation failed, including an invalid repository,
  commit, source path, current path, dirty current content tree, or forbidden
  report destination.
- `3`: analysis completed and emitted a report, but at least one historical
  block is ambiguous or unmatched, metadata is unrepresentable, or malformed
  markup prevents safe analysis.

Warnings may describe current-only files or blocks, but warnings alone do not
change exit code 0. Every unsafe partial result is an error and forces exit
code 3.

## Safety and PR 1 acceptance

The tool does not open current Markdown from the worktree; it reads the captured
`HEAD` tree by object ID and reports that target commit. It exposes no content
writer, no apply command, and no filesystem report destination. It writes only
to the caller-provided standard-output stream.

PR 1 acceptance requires:

- focused tests for parsing, enumeration, normalization, exact matching,
  planning, deterministic reporting, Git/input failures, and exit behavior;
- a full dry run against the pinned commit and the current branch;
- a second run whose report bytes equal the first run;
- no `content/` diff and no worktree changes caused by either run; and
- an inventory of all error diagnostics and an explicit assessment of whether
  the line/text contract is sufficient for Issues #4 and #5.
