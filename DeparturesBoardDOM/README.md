# Departures Board

A live airport/train departures board built entirely from JavaScript-driven
DOM manipulation — no row markup exists in the HTML.

## What I built

`index.html` contains only a static shell: header, toolbar, column labels,
an **empty** `<div id="board">`, and a summary footer. Every row inside the
board, every cell, and every status badge is created at runtime by
`script.js` from a `flights` array (the data model). The app also includes:

- **Add Departure** — generates a random flight and appends one new animated row.
- **Reset Board** — restores the original starting dataset and re-renders.
- **Live clock** — `setInterval` updates a header clock every second.
- **Live status simulation** — every 4 seconds, one in-progress flight advances
  through `ON TIME → BOARDING → GATE CLOSED → DEPARTED`, and only that flight's
  status badge is touched in the DOM (not a full re-render).
- **Live counter** — a footer summary recalculated from the data on every change.
- **Custom flight form** — type your own time/flight/destination/gate and submit
  it as a new row.
- **Row cap** — board caps at 12 rows; oldest row is dropped when a new one
  pushes past the limit.
- **localStorage persistence** — board state survives a page refresh.
- **Split-flap CSS animation** — rows flip in on add, and status badges flip
  when their text changes.

## How the DOM is created and updated

1. **Data model**: `flights` is an array of objects (`id, time, flight, dest,
   gate, status`). It's the single source of truth — the DOM always reflects it.
2. **Row creation** (`createRowElement`): for each flight object, I build a
   `<div class="row">` and five child `<span>` cells using
   `document.createElement`, set their text with `textContent`, and attach
   them with `appendChild`. No `innerHTML` is used anywhere. Each row gets a
   `data-id` attribute so I can find it again later for targeted updates.
3. **Full render** (`renderBoard`): clears the board (`textContent = ""`) and
   loops over `flights`, appending a fresh row for each — used on load and Reset.
4. **Incremental render** (`appendRow`): for Add/Submit, I don't rebuild the
   whole board — I create one row and `appendChild` it, with a CSS class that
   triggers the flap-in animation.
5. **Targeted update** (`updateStatusInDom`): for the live status simulation,
   I locate the existing row via `querySelector('.row[data-id="..."]')`, then
   update only its `.status` badge's `textContent` and `className` — the rest
   of the row's DOM nodes are never touched or recreated.
6. **Clock & status loops**: two `setInterval` timers — one repaints the clock
   text every second, the other advances one random flight's status every 4
   seconds and re-derives the summary counts.
