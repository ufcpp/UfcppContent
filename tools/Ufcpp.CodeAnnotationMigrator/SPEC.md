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
still contain their editorial annotations. PR 1 is analysis-only. It must not
modify `content/`, and it has no `--apply` mode. Later stacked PRs may consume
the report to implement Issues #4 and #5; PR 4 removes this tool.

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
  worktree. The default is `content`.
- `--report <path|->`: deterministic JSON output. `-`, the default, means
  standard output. A file report must be outside the repository worktree.
- `--dry-run`: optional and accepted for clarity. Dry run is the only mode.

Unknown options, positional arguments, `--apply`, missing option values, rooted
source/current paths, `..` traversal, and a report path anywhere inside the
repository worktree are input errors. Report paths that use a Windows device
alias or traverse a symbolic link or junction are also rejected; directory
handles are resolved to canonical final paths on Windows to catch short-name
and substituted-drive aliases. File reports are written to a sibling temporary
file and atomically moved into place so an existing hard link is not written
through.

Before analysis, the tool:

1. resolves and validates the worktree root with `git rev-parse`;
2. verifies that the revision names a commit;
3. verifies that `<commit>:<source-path>` exists as a tree;
4. verifies that the current path exists as a directory;
5. rejects tracked, untracked, or ignored Markdown changes below the current
   content path, rejects `assume-unchanged`/`skip-worktree` index flags, and
   rejects linked Markdown files; and
6. reads historical blobs with `git ls-tree` and `git cat-file`.

Git is invoked only with read-only commands. The tool never checks out,
updates, stages, or writes a repository file.

## Document and block enumeration

Only `.md` files are considered. Paths are relative to the configured tree,
use `/` separators, and are ordered with ordinal string comparison.

Historical documents are read from the pinned commit. Every HTML `<pre>`
element is enumerated in source order, including `<pre>` elements nested in
raw HTML tables. A `<code>` wrapper inside `<pre>` is structural and is not a
separate block. Inline HTML or Markdown `<code>` outside `<pre>` is excluded.

Current documents are read from the worktree. The following are merged by
source offset to produce one document-order sequence:

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

Tag and attribute names and class tokens are compared case-insensitively.
Malformed or unbalanced `<pre>`, `<code>`, `<em>`, `error`, or `warning`
markup is an explicit diagnostic, never a partially parsed success.

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

## Report

The report is UTF-8 without a byte-order mark, uses LF newlines, is
pretty-printed JSON with one final LF, and has schema version 1. It contains no
timestamp, absolute path, machine name, elapsed time, or report destination.

Top-level sections are emitted in this order:

1. `schemaVersion` and `mode`;
2. resolved `source` and relative `target`;
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
the number of malformed historical blocks. A malformed block contributes to
the appropriate block-kind `unrepresentable` count; metadata that cannot be
parsed safely from that block is not guessed into a metadata counter and is
instead identified by its per-block diagnostic.

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

The tool opens current Markdown only for reading. It exposes no content writer
and no apply command. Report file output uses only the explicit destination
after validating that it is outside the repository worktree.

PR 1 acceptance requires:

- focused tests for parsing, enumeration, normalization, exact matching,
  planning, deterministic reporting, Git/input failures, and exit behavior;
- a full dry run against the pinned commit and the current branch;
- a second run whose report bytes equal the first run;
- no `content/` diff and no worktree changes caused by either run; and
- an inventory of all error diagnostics and an explicit assessment of whether
  the line/text contract is sufficient for Issues #4 and #5.
