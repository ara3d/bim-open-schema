# BIM Open Schema branding

Decided 2026-10-03 by Christopher Diggins. BIM Open Schema is one of five
products in the BIM Open family; the family guide, with the fonts, the
neutrals, the status colours, and the rules every mark follows, is
`docs/BRANDING.md` in the `bim-open-toolkit` repository. This file holds
only what is the schema's own.

## The mark: Hollow Rows

![Hollow Rows](mark.svg)

Three table rows drawn as frames, each with a key cell and a value cell cut
out of it: the shape of the data with no data inside. The other four marks
in the family are verbs (rows become a flow, a flow becomes a page, a node
becomes slabs, a model becomes rows); this is the noun they share, and the
only mark drawn in negative space, which is what sets the specification
apart from the tools.

`mark.svg` is a 24 by 24 viewBox, one path, one fill, with
`fill-rule="evenodd"`; code that pastes the path must keep that attribute.
`lockup.svg` sets the mark beside the wordmark.

Rules:

- One colour: slate (`#4f5d78`) on a light surface; white on slate or on a
  dark surface. No gradients, no outline, no shadow.
- Sizes: 16 px (favicon), 22 px (beside the wordmark), 48 px (a page
  heading), 96 px and up (papers, slides). Below 16 px use one frame with
  its two cells, not all three rows.
- Clear space of half the mark's width on every side.

## Wordmark

Three words, **BIM Open Schema**, in Instrument Sans: "BIM Open" in weight
600 in the dim colour (`#5a606c`), then "Schema" in weight 700 in the text
colour (`#171a1f`), with a 4 px gap at 15 px. Where a person reads the
name, write the three words; `bim-open-schema` and `BimOpenSchema` are
names for machines.

## Colours

| Token | Value | Use |
|---|---|---|
| Accent | `#4f5d78` | The mark, links and buttons on the specification's pages, the key badge |
| Accent soft | `#e9ecf2` | Selected row, badges |
| Text | `#171a1f` | Headings, body |
| Dim | `#5a606c` | Secondary text, the "BIM Open" half of the wordmark |
| Surface, background, border | `#ffffff`, `#f4f5f7`, `#e3e6ea` | As in the family guide |

## Type

Instrument Sans for the wordmark and headings, Public Sans for body text
and tables, Fira Code for field and type names. Prose at 14.5 px on a 1.55
line height and a 68-character measure; field tables at 12 to 13 px, so a
table of columns here looks like a table of rows in BIM Open Flow.

## Earlier marks

`img/BIM-open-schema-logo-proposal.png`, `img/BOS-32x32.png`, and
`img/bos-big.png` are the logo from before the family brand and are kept
for history.
