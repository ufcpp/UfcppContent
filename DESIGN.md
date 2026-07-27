---
name: "++C++; // 未確認飛行 C"
description: "A practical technical reference room grounded in the current ufcpp.net identity."
colors:
  brand-lavender: "#ccccff"
  brand-navy: "#2a3869"
  brand-navy-hover: "#455282"
  page-bg: "#f3f3f3"
  content-bg: "#ffffff"
  text: "#4c4c4c"
  heading: "#483949"
  content-link: "#a35951"
  border: "#dddddd"
  code-keyword: "#0000e1"
  code-control: "#8f08c4"
  code-comment: "#008000"
  code-string: "#a31515"
  code-number: "#098658"
  code-type: "#2b91af"
  code-method: "#74531f"
  code-symbol: "#001080"
typography:
  headline:
    fontFamily: '"Hiragino Kaku Gothic Pro", "Meiryo", "Yu Gothic", YuGothic, "MS PGothic", sans-serif'
    fontSize: "28px"
    fontWeight: 400
    lineHeight: "1.25"
  title:
    fontFamily: '"Hiragino Kaku Gothic Pro", "Meiryo", "Yu Gothic", YuGothic, "MS PGothic", sans-serif'
    fontSize: "22px"
    fontWeight: 400
    lineHeight: "1.25"
  body:
    fontFamily: '"Hiragino Kaku Gothic Pro", "Meiryo", "Yu Gothic", YuGothic, "MS PGothic", sans-serif'
    fontSize: "16px"
    fontWeight: 400
    lineHeight: "1.5"
  code:
    fontFamily: 'Consolas, "Courier New", Courier, monospace'
    fontSize: "14px"
    fontWeight: 400
    lineHeight: "1.3"
rounded:
  compact: "0.2em"
  search: "10px"
spacing:
  compact: "8px"
  content: "16px"
  medium: "32px"
  wide: "48px"
components:
  navigation:
    backgroundColor: "{colors.brand-navy}"
    textColor: "{colors.content-bg}"
    typography: "{typography.body}"
    height: "50px"
  navigation-hover:
    backgroundColor: "{colors.brand-navy-hover}"
    textColor: "{colors.content-bg}"
  content-panel:
    backgroundColor: "{colors.content-bg}"
    textColor: "{colors.text}"
    rounded: "0"
    padding: "{spacing.content}"
  page-title:
    backgroundColor: "{colors.brand-navy}"
    textColor: "{colors.page-bg}"
    typography: "{typography.headline}"
    rounded: "{rounded.compact}"
    padding: "6px 8px"
---

# Design System: ++C++; // 未確認飛行 C

## Overview

**Creative North Star: "The Technical Reference Room"**

This system should feel like a well-used technical reference room: practical, approachable,
and dense with useful information. The live ufcpp.net identity is the source of truth, so the
lavender masthead, navy navigation, original logo, pale gray page, and white reading surfaces
remain immediately recognizable.

Modernization is structural rather than cosmetic. Semantic landmarks, keyboard focus, and
responsive spacing improve the experience without turning the archive into a generic SaaS
landing page, a dark-mode-first developer tool, or a sparse editorial redesign.

**Key Characteristics:**

- Original ufcpp.net logo on a pale lavender masthead.
- Compact navy navigation with high-contrast white labels.
- Information-dense white reading panels on a pale gray page.
- Restrained borders and shadows that separate surfaces without decoration.
- Code, tables, and formulas treated as first-class content within a full-width article.

## Colors

The palette is the established ufcpp.net lavender-and-navy identity supported by quiet,
high-contrast reading neutrals.

### Primary

- **Masthead Lavender** (`brand-lavender`): Owns the header and desktop footer so the original
  logo appears in its intended visual environment.
- **Navigation Navy** (`brand-navy`): Carries navigation, page-title plaques, and section markers.
- **Navigation Hover** (`brand-navy-hover`): Provides the only stronger navigation state.

### Secondary

- **Reference Link Brown** (`content-link`): Distinguishes links inside technical prose without
  introducing a generic bright web-blue accent.
- **Code Syntax Colors** (`code-keyword`, `code-control`, `code-comment`, `code-string`,
  `code-number`, `code-type`, `code-method`, `code-symbol`): Preserve the familiar Visual
  Studio Light syntax-highlighting vocabulary for both migrated and newly rendered examples.

### Neutral

- **Archive Gray** (`page-bg`): The continuous page background around all reading surfaces.
- **Reading White** (`content-bg`): Article, search, and code surfaces.
- **Technical Ink** (`text`): Default prose and navigation-supporting copy.
- **Heading Plum** (`heading`): Secondary heading color inherited from the live site.
- **Quiet Rule** (`border`): Hairline surface separation and restrained directional shadows.

### Named Rules

**The Recognition Rule.** Lavender and navy must appear together in the masthead and navigation;
using either as a generic accent elsewhere weakens the identity.

**The White Reading Surface Rule.** Long-form content always sits on white against archive gray.
Never put technical prose directly on the lavender or navy brand fields.

## Typography

**Display Font:** Hiragino Kaku Gothic Pro with Meiryo and Yu Gothic fallbacks  
**Body Font:** Hiragino Kaku Gothic Pro with Meiryo and Yu Gothic fallbacks  
**Label/Mono Font:** Consolas with Courier New and Courier fallbacks

**Character:** A Japanese system-sans stack keeps the archive practical and familiar. Consolas
gives code samples the same compact Windows-oriented voice as the live site.

### Hierarchy

- **Headline** (400, 28px, 1.25): Page titles on compact navy plaques.
- **Title** (400, 22px, 1.25): Major section headings.
- **Subheading** (700, 18px, 1.25): Technical subsections in navy.
- **Body** (400, 16px, 1.5): Dense long-form reference prose.
- **Code** (400, 14px, 1.3): Source listings and inline technical tokens.

### Named Rules

**The Reference Density Rule.** Use the established compact scale; oversized display typography
is prohibited because it reduces useful information per viewport.

## Elevation

The system is flat by default. One-pixel directional shadows separate the masthead and footer
from the page, while reading surfaces use quiet borders rather than floating card shadows.

### Shadow Vocabulary

- **Header Rule** (`0 1px 1px #dddddd`): Separates the lavender masthead from navigation and page.
- **Footer Rule** (`0 -1px 1px #dddddd`): Separates the desktop footer from the page.

### Named Rules

**The Archive Flatness Rule.** Surfaces never float. If a panel resembles a modern dashboard
card, its shadow is too strong or its corner treatment is too decorative.

## Components

### Masthead

- **Shape:** Full-width rectangular field with no corner radius.
- **Background:** Masthead Lavender.
- **Identity:** Use the archived 450x65 ufcpp.net logo at its natural desktop dimensions.
- **Mobile:** Scale the logo proportionally and keep it centered.

### Navigation

- **Shape:** Full-width navy strip with a 5px dark top rule.
- **Default:** White 18px labels in a compact 50px row.
- **Hover / Focus:** Shift only to Navigation Hover; retain white text and visible focus.
- **Mobile:** Keep links horizontally scrollable instead of hiding navigation behind JavaScript.

### Content Panels

- **Corner Style:** Square.
- **Background:** Reading White.
- **Border:** One quiet rule around the article body; title and navigation stay outside it.
- **Internal Padding:** 16px desktop, 8px mobile.

### Context Navigation

- **Breadcrumbs:** Place a compact, wrapping hierarchy directly below the page title. Use
  `TOP` for the home link and omit the generic study container from descendant trails.
- **Article Indexes:** Keep the table of contents and keyword list in the full-width reading
  column above the framed body. Keep them unframed on the archive-gray page, as on the
  original site; they must never recreate a separate related-pages sidebar.
- **Table of Contents:** Preserve heading hierarchy with nested links and prefer stable legacy
  anchors when available.
- **Keywords:** Present extracted terms as compact gray links with a key marker, matching the
  original site's dense reference treatment.

### Headings

- **Page Title:** Compact navy plaque with Reading White text and gently rounded corners.
- **Section Title:** Heading Plum text with an 8px navy marker rendered as a separate shape.
- **Subsections:** Navy or Code Type Cyan according to hierarchy.

### Code Blocks

- **Background:** Reading White for source and dark gray for console output.
- **Border:** 2px Masthead Lavender.
- **Overflow:** Horizontal scrolling is mandatory; code must never wrap destructively.
- **Syntax:** Preserve blue reserved words, cyan types, green comments, and brown methods.

## Do's and Don'ts

### Do:

- **Do** use the original archived ufcpp.net logo and the canonical `#ccccff` / `#2a3869` pairing.
- **Do** keep articles white on `#f3f3f3` and retain compact 16px technical prose.
- **Do** let each article use the full available reading width.
- **Do** keep breadcrumbs and article indexes in the reading column immediately below the title.
- **Do** keep code and wide tables horizontally scrollable.

### Don't:

- **Don't** create generic SaaS landing pages with oversized hero copy, card grids, gradients,
  or glass effects.
- **Don't** introduce a dark-mode-first developer-tool redesign that replaces the established
  ufcpp.net palette.
- **Don't** create a minimal editorial redesign that sacrifices navigation density or the site's
  familiar logo.
- **Don't** apply decorative modernization that makes historical technical content feel detached
  from ufcpp.net.
- **Don't** add floating cards, large radii, animated entrances, or decorative motion.
