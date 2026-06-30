/* =========================================================
   DEPARTURES BOARD — script.js
   Everything you see in .board is built at runtime from the
   `flights` array below. No row markup exists in index.html.
   ========================================================= */

// ---------------------------------------------------------
// 1. DATA MODEL
// ---------------------------------------------------------
// The single source of truth. Every render reads from this
// array; every update (status change, add, reset) mutates it
// and then re-renders the affected DOM.
const STARTING_FLIGHTS = [
  { id: 1, time: "08:05", flight: "AI 202", dest: "Mumbai",     gate: "A12", status: "ON TIME" },
  { id: 2, time: "08:20", flight: "6E 117", dest: "Delhi",      gate: "B04", status: "BOARDING" },
  { id: 3, time: "08:35", flight: "UK 955", dest: "Bengaluru",  gate: "A03", status: "ON TIME" },
  { id: 4, time: "08:50", flight: "SG 441", dest: "Hyderabad",  gate: "C09", status: "DELAYED" },
  { id: 5, time: "09:05", flight: "AI 870", dest: "Kolkata",    gate: "B11", status: "GATE CLOSED" },
  { id: 6, time: "09:20", flight: "QP 1422", dest: "Pune",      gate: "A07", status: "ON TIME" },
  { id: 7, time: "09:40", flight: "6E 663", dest: "Goa",        gate: "B02", status: "DEPARTED" },
];

// Working copy of the data the board currently renders.
// Cloned (deep-ish) from STARTING_FLIGHTS so Reset can restore
// the original without being mutated by reference.
let flights = cloneFlights(STARTING_FLIGHTS);

// Used to give every new flight (Add / custom form) a unique id
let nextId = flights.length + 1;

// Status cycle used by the "live status update" simulation
const STATUS_CYCLE = ["ON TIME", "BOARDING", "GATE CLOSED", "DEPARTED"];

// Optional: cap the board at N rows (stretch goal — oldest removed when full)
const MAX_ROWS = 12;

// ---------------------------------------------------------
// 2. DOM REFERENCES
// ---------------------------------------------------------
const boardEl = document.getElementById("board");
const summaryEl = document.getElementById("summary");
const clockEl = document.getElementById("clock");

const addBtn = document.getElementById("addBtn");
const resetBtn = document.getElementById("resetBtn");
const customForm = document.getElementById("customForm");

// ---------------------------------------------------------
// 3. HELPERS
// ---------------------------------------------------------

// Deep-clone the flight objects so the starting dataset is never
// mutated by reference when we edit the working `flights` array.
function cloneFlights(arr) {
  return arr.map((f) => ({ ...f }));
}

// Turn "GATE CLOSED" into the CSS class suffix "gate-closed"
function statusToClass(status) {
  return "status-" + status.toLowerCase().replace(/\s+/g, "-");
}

// Build a two-digit-padded HH:MM:SS string for the live clock
function formatClock(date) {
  const pad = (n) => String(n).padStart(2, "0");
  return `${pad(date.getHours())}:${pad(date.getMinutes())}:${pad(date.getSeconds())}`;
}

// ---------------------------------------------------------
// 4. RENDERING — build DOM nodes from data
//    (createElement + textContent + appendChild only)
// ---------------------------------------------------------

// Build ONE row <div> from a single flight object.
// This is the only place row markup is constructed, and it is
// done entirely with DOM APIs — no innerHTML string building.
function createRowElement(flightObj) {
  // Outer row container
  const row = document.createElement("div");
  row.className = "row";
  row.dataset.id = flightObj.id; // tag the element so we can find it later for live updates

  // --- Time cell ---
  const timeCell = document.createElement("span");
  timeCell.className = "cell cell-time";
  timeCell.textContent = flightObj.time;

  // --- Flight number cell ---
  const flightCell = document.createElement("span");
  flightCell.className = "cell cell-flight";
  flightCell.textContent = flightObj.flight;

  // --- Destination cell ---
  const destCell = document.createElement("span");
  destCell.className = "cell cell-dest";
  destCell.textContent = flightObj.dest;

  // --- Gate cell ---
  const gateCell = document.createElement("span");
  gateCell.className = "cell cell-gate";
  gateCell.textContent = flightObj.gate;

  // --- Status cell (wraps a colored badge <span>) ---
  const statusCell = document.createElement("span");
  statusCell.className = "cell cell-status";

  const statusBadge = document.createElement("span");
  statusBadge.className = "status " + statusToClass(flightObj.status);
  statusBadge.textContent = flightObj.status;
  statusCell.appendChild(statusBadge);

  // Attach all cells to the row, in column order
  row.appendChild(timeCell);
  row.appendChild(flightCell);
  row.appendChild(destCell);
  row.appendChild(gateCell);
  row.appendChild(statusCell);

  return row;
}

// Clear the board and rebuild every row from the `flights` array.
// Used on first load, Reset, and Add Departure.
function renderBoard() {
  // Wipe existing children (safe full-rebuild for simplicity & correctness)
  boardEl.textContent = "";

  if (flights.length === 0) {
    const empty = document.createElement("div");
    empty.className = "empty-msg";
    empty.textContent = "No departures scheduled.";
    boardEl.appendChild(empty);
  } else {
    flights.forEach((f) => {
      const rowEl = createRowElement(f);
      boardEl.appendChild(rowEl);
    });
  }

  updateSummary();
}

// Append a single new row with an entrance animation, without
// rebuilding the whole board (used by Add Departure / custom form).
function appendRow(flightObj) {
  const rowEl = createRowElement(flightObj);
  rowEl.classList.add("row-enter"); // triggers the CSS flap-in animation
  boardEl.appendChild(rowEl);

  // Remove the empty-state message if it was showing
  const emptyMsg = boardEl.querySelector(".empty-msg");
  if (emptyMsg) emptyMsg.remove();

  updateSummary();
}

// ---------------------------------------------------------
// 5. LIVE STATUS UPDATES — update only the affected cell
// ---------------------------------------------------------

// Find the row's status badge in the live DOM by data-id and
// update its text + class WITHOUT touching the rest of the row.
function updateStatusInDom(flightId, newStatus) {
  const row = boardEl.querySelector(`.row[data-id="${flightId}"]`);
  if (!row) return;

  const badge = row.querySelector(".status");
  if (!badge) return;

  badge.textContent = newStatus;
  badge.className = "status " + statusToClass(newStatus);

  // Re-trigger the flip animation
  badge.classList.remove("status-flip");
  // Force reflow so the animation can restart even if class is reapplied
  void badge.offsetWidth;
  badge.classList.add("status-flip");
}

// Pick a random flight that hasn't already DEPARTED and advance
// its status one step along STATUS_CYCLE. Updates data + DOM.
function advanceRandomFlightStatus() {
  const candidates = flights.filter((f) => f.status !== "DEPARTED" && f.status !== "DELAYED" && f.status !== "CANCELLED");
  if (candidates.length === 0) return;

  const target = candidates[Math.floor(Math.random() * candidates.length)];
  const currentIndex = STATUS_CYCLE.indexOf(target.status);

  // If status isn't part of the normal cycle (e.g. already custom), start at 0
  const nextIndex = currentIndex === -1 ? 0 : Math.min(currentIndex + 1, STATUS_CYCLE.length - 1);
  const newStatus = STATUS_CYCLE[nextIndex];

  if (newStatus === target.status) return; // nothing changed (already DEPARTED)

  // Update the data model
  target.status = newStatus;

  // Update only that one cell in the DOM (no full re-render)
  updateStatusInDom(target.id, newStatus);

  // Counters need to reflect the status change too
  updateSummary();
}

// ---------------------------------------------------------
// 6. LIVE COUNTER / SUMMARY
// ---------------------------------------------------------
function updateSummary() {
  const total = flights.length;
  const counts = flights.reduce((acc, f) => {
    acc[f.status] = (acc[f.status] || 0) + 1;
    return acc;
  }, {});

  const boarding = counts["BOARDING"] || 0;
  const delayed = counts["DELAYED"] || 0;
  const departed = counts["DEPARTED"] || 0;
  const closed = counts["GATE CLOSED"] || 0;

  summaryEl.textContent =
    `${total} departures · ${boarding} boarding · ${delayed} delayed · ` +
    `${closed} gate closed · ${departed} departed`;
}

// ---------------------------------------------------------
// 7. LIVE CLOCK
// ---------------------------------------------------------
function tickClock() {
  clockEl.textContent = formatClock(new Date());
}

// ---------------------------------------------------------
// 8. ADD DEPARTURE (random sample flight)
// ---------------------------------------------------------
const SAMPLE_DESTS = ["Chennai", "Jaipur", "Lucknow", "Ahmedabad", "Kochi", "Indore", "Nagpur", "Patna"];
const SAMPLE_CARRIERS = ["AI", "6E", "UK", "SG", "QP", "I5"];

function randomFlightNumber() {
  const carrier = SAMPLE_CARRIERS[Math.floor(Math.random() * SAMPLE_CARRIERS.length)];
  const num = Math.floor(100 + Math.random() * 900);
  return `${carrier} ${num}`;
}

function randomTime() {
  const h = String(Math.floor(Math.random() * 14) + 6).padStart(2, "0"); // 06–19
  const m = String(Math.floor(Math.random() * 60)).padStart(2, "0");
  return `${h}:${m}`;
}

function randomGate() {
  const terminal = ["A", "B", "C"][Math.floor(Math.random() * 3)];
  const num = Math.floor(Math.random() * 20) + 1;
  return `${terminal}${num}`;
}

function addRandomDeparture() {
  const newFlight = {
    id: nextId++,
    time: randomTime(),
    flight: randomFlightNumber(),
    dest: SAMPLE_DESTS[Math.floor(Math.random() * SAMPLE_DESTS.length)],
    gate: randomGate(),
    status: "ON TIME",
  };

  addFlightToBoard(newFlight);
}

// Shared logic for adding any new flight (random or custom form),
// including the row cap stretch goal.
function addFlightToBoard(newFlight) {
  flights.push(newFlight);

  // Stretch goal: cap board at MAX_ROWS, drop the oldest
  if (flights.length > MAX_ROWS) {
    flights.shift(); // remove oldest from data
    renderBoard();   // full rebuild needed since the row set changed at the top
  } else {
    appendRow(newFlight); // just add the new row, animated
  }

  saveToStorage();
}

// ---------------------------------------------------------
// 9. RESET
// ---------------------------------------------------------
function resetBoard() {
  flights = cloneFlights(STARTING_FLIGHTS);
  nextId = flights.length + 1;
  renderBoard();
  saveToStorage();
}

// ---------------------------------------------------------
// 10. PERSISTENCE (stretch goal) — survive a page refresh
// ---------------------------------------------------------
const STORAGE_KEY = "departures-board-state";

function saveToStorage() {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify({ flights, nextId }));
  } catch (e) {
    // localStorage may be unavailable (private mode, quota) — fail silently
    console.warn("Could not save board state:", e);
  }
}

function loadFromStorage() {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return false;
    const parsed = JSON.parse(raw);
    if (!Array.isArray(parsed.flights)) return false;
    flights = parsed.flights;
    nextId = parsed.nextId || flights.length + 1;
    return true;
  } catch (e) {
    console.warn("Could not load saved board state:", e);
    return false;
  }
}

// ---------------------------------------------------------
// 11. CUSTOM FLIGHT FORM (stretch goal)
// ---------------------------------------------------------
function handleCustomFormSubmit(event) {
  event.preventDefault(); // stop normal page reload on submit

  const time = document.getElementById("inputTime").value.trim();
  const flight = document.getElementById("inputFlight").value.trim();
  const dest = document.getElementById("inputDest").value.trim();
  const gate = document.getElementById("inputGate").value.trim();

  if (!time || !flight || !dest || !gate) return; // required attrs already guard this, belt & suspenders

  const newFlight = {
    id: nextId++,
    time,
    flight,
    dest,
    gate,
    status: "ON TIME",
  };

  addFlightToBoard(newFlight);
  customForm.reset(); // clear the inputs
}

// ---------------------------------------------------------
// 12. EVENT LISTENERS (no inline onclick anywhere)
// ---------------------------------------------------------
addBtn.addEventListener("click", addRandomDeparture);
resetBtn.addEventListener("click", resetBoard);
customForm.addEventListener("submit", handleCustomFormSubmit);

// ---------------------------------------------------------
// 13. INIT — runs once on page load
// ---------------------------------------------------------
function init() {
  loadFromStorage(); // restore previous session if available, else keep starting data

  renderBoard();      // build all rows from data
  tickClock();         // show clock immediately, don't wait 1s for first tick
  setInterval(tickClock, 1000);                 // live clock, every second
  setInterval(advanceRandomFlightStatus, 4000);  // live status updates, every 4s
}

init();
