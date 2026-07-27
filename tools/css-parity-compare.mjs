#!/usr/bin/env node
// Compares computed styles between the generated site and the original
// ufcpp.net, so CSS parity is checked against the live reference rather than by
// eye. Drives headless Chrome (or Edge) over the DevTools Protocol using only
// Node built-ins - no npm install, no browser automation dependency.
//
// Usage:
//   node tools/css-parity-compare.mjs cases.json
//   node tools/css-parity-compare.mjs --width 480 cases.json
//   node tools/css-parity-compare.mjs '[{"label":"...","path":"/...","selector":"..."}]'
//
// A case file is a JSON array of { label, path, selector } objects, where
// `path` is a site-relative URL present on both sides.
//
// Options:
//   --base <url>       generated site under test (default http://127.0.0.1:8791)
//   --reference <url>  original site (default https://ufcpp.net)
//   --width <px>       viewport width  (default 1200; use 480 for the narrow layout)
//   --height <px>      viewport height (default 900)
//   --max-diffs <n>    per-case diff lines printed (default 24)
//
// Serve the generated site first, for example:
//   dotnet run --project tools/Ufcpp.SiteGenerator -- --content content/ --assets assets/ --output _site/
//   cd _site && python -m http.server 8791 --bind 127.0.0.1
//
// Exit code is 1 when any case reports a difference, so this can gate a manual
// verification pass. See docs/css-parity.md.

import { spawn } from 'node:child_process';
import { existsSync, mkdtempSync, readFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';

const DEFAULTS = {
  base: 'http://127.0.0.1:8791',
  reference: 'https://ufcpp.net',
  width: 1200,
  height: 900,
  'max-diffs': 24,
};

function parseArgs(argv) {
  const options = { ...DEFAULTS };
  const rest = [];
  for (let i = 0; i < argv.length; i++) {
    const arg = argv[i];
    if (!arg.startsWith('--')) {
      rest.push(arg);
      continue;
    }
    const key = arg.slice(2);
    if (!(key in DEFAULTS)) throw new Error(`Unknown option --${key}`);
    const value = argv[++i];
    if (value === undefined) throw new Error(`--${key} needs a value`);
    options[key] = typeof DEFAULTS[key] === 'number' ? Number(value) : value;
  }
  if (rest.length !== 1) {
    throw new Error('Expected exactly one case-file path or inline JSON array.');
  }
  const [source] = rest;
  const json = existsSync(source) ? readFileSync(source, 'utf8') : source;
  return { options, cases: JSON.parse(json) };
}

const BROWSERS = [
  'C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe',
  'C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe',
  'C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe',
  'C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe',
  '/usr/bin/google-chrome',
  '/usr/bin/chromium',
];

function findBrowser() {
  const found = BROWSERS.find((path) => existsSync(path));
  if (!found) throw new Error(`No Chromium browser found. Looked in:\n  ${BROWSERS.join('\n  ')}`);
  return found;
}

// Properties whose colour/style only renders when the matching border side has
// a non-zero width. ufcpp.net sets the `border-color` shorthand on the version
// markers, which paints all four sides even though only the left one is drawn;
// comparing those invisible values reports differences nobody can see.
const BORDER_SIDES = ['Top', 'Right', 'Bottom', 'Left'];

const PROPS = [
  'color', 'backgroundColor', 'display', 'cursor', 'fontWeight', 'fontSize',
  'marginTop', 'marginRight', 'marginBottom', 'marginLeft',
  'paddingTop', 'paddingRight', 'paddingBottom', 'paddingLeft',
  'textDecorationLine',
  ...BORDER_SIDES.flatMap((side) => [
    `border${side}Width`, `border${side}Style`, `border${side}Color`,
  ]),
];

/** True when `property` describes a border side that neither side renders. */
function isInvisibleBorderDetail(property, ours, theirs) {
  const side = BORDER_SIDES.find((s) => property === `border${s}Style` || property === `border${s}Color`);
  if (!side) return false;
  const width = `border${side}Width`;
  const zero = (value) => value === '0px' || value === '0';
  return zero(ours[width]) && zero(theirs[width]);
}

const sleep = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

async function debuggerUrl(port) {
  for (let i = 0; i < 40; i++) {
    try {
      const list = await (await fetch(`http://127.0.0.1:${port}/json/list`)).json();
      const page = list.find((target) => target.type === 'page');
      if (page?.webSocketDebuggerUrl) return page.webSocketDebuggerUrl;
    } catch { /* the browser is not listening yet */ }
    await sleep(250);
  }
  throw new Error('Chrome DevTools endpoint never became available.');
}

async function connect(options) {
  const port = 9334;
  const browser = spawn(findBrowser(), [
    '--headless', '--disable-gpu', `--remote-debugging-port=${port}`,
    `--user-data-dir=${mkdtempSync(join(tmpdir(), 'css-parity-'))}`,
    '--no-first-run', `--window-size=${options.width},${options.height}`, 'about:blank',
  ], { stdio: 'ignore' });

  const socket = new WebSocket(await debuggerUrl(port));
  await new Promise((resolve) => socket.addEventListener('open', resolve, { once: true }));

  let nextId = 0;
  const pending = new Map();
  socket.addEventListener('message', (event) => {
    const message = JSON.parse(event.data);
    pending.get(message.id)?.(message.result);
    pending.delete(message.id);
  });
  const send = (method, params = {}) => new Promise((resolve) => {
    const id = ++nextId;
    pending.set(id, resolve);
    socket.send(JSON.stringify({ id, method, params }));
  });

  await send('Page.enable');
  return { send, close: () => { socket.close(); browser.kill(); } };
}

async function snapshot(send, url, selector) {
  await send('Page.navigate', { url });
  // ufcpp.net styles some elements from JavaScript, so give the page time to
  // settle before reading computed styles.
  await sleep(2600);
  const expression = `JSON.stringify([...document.querySelectorAll(${JSON.stringify(selector)})].map((el) => {
    const style = getComputedStyle(el);
    const snapshot = { tag: el.tagName.toLowerCase(), cls: el.getAttribute('class') || '' };
    for (const property of ${JSON.stringify(PROPS)}) snapshot[property] = style[property];
    return snapshot;
  }))`;
  const { result } = await send('Runtime.evaluate', { expression, returnByValue: true });
  return JSON.parse(result.value);
}

const { options, cases } = parseArgs(process.argv.slice(2));
const { send, close } = await connect(options);
let failed = false;

try {
  for (const { label, path, selector } of cases) {
    const ours = await snapshot(send, `${options.base}${path}`, selector);
    const theirs = await snapshot(send, `${options.reference}${path}`, selector);

    if (ours.length !== theirs.length) {
      failed = true;
      console.log(`\n## ${label} (${selector})`);
      console.log(`   element count differs: ours=${ours.length} ufcpp=${theirs.length}`);
      continue;
    }

    const diffs = [];
    for (let i = 0; i < ours.length; i++) {
      for (const property of PROPS) {
        if (ours[i][property] === theirs[i][property]) continue;
        if (isInvisibleBorderDetail(property, ours[i], theirs[i])) continue;
        diffs.push(
          `   [${i}] ${ours[i].tag}.${ours[i].cls} ${property}: `
          + `ours='${ours[i][property]}' ufcpp='${theirs[i][property]}'`);
      }
    }

    if (diffs.length > 0) failed = true;
    console.log(`\n## ${label} (${selector}) - ${ours.length} element(s), ${diffs.length} diff(s)`);
    if (diffs.length > 0) console.log(diffs.slice(0, options['max-diffs']).join('\n'));
  }
} finally {
  close();
}

process.exit(failed ? 1 : 0);
