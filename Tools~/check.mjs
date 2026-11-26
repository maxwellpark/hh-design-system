// Fails when an imported asset has no .meta, since Unity skips it in a package installed from git.
import { existsSync, readdirSync, readFileSync, statSync } from "node:fs";
import { join } from "node:path";

const roots = ["Runtime", "Editor", "package.json", "README.md", "CHANGELOG.md", "LICENSE.md"];
const problems = [];

// Unity ignores hidden names and names ending in ~, and never imports a .meta itself
const imported = (name) => !name.startsWith(".") && !name.endsWith("~") && !name.endsWith(".meta");

function walk(path) {
  if (!existsSync(`${path}.meta`)) {
    problems.push(`${path}: no .meta`);
  }
  if (statSync(path).isDirectory()) {
    for (const name of readdirSync(path).filter(imported)) {
      walk(join(path, name));
    }
  }
}

const guids = new Map();
function collectGuids(path) {
  if (statSync(path).isDirectory()) {
    for (const name of readdirSync(path)) {
      if (!name.startsWith(".") && !name.endsWith("~")) {
        collectGuids(join(path, name));
      }
    }
    return;
  }
  if (!path.endsWith(".meta")) {
    return;
  }
  const guid = readFileSync(path, "utf8").match(/^guid: ([0-9a-f]{32})$/m)?.[1];
  if (!guid) {
    problems.push(`${path}: no guid`);
  } else if (guids.has(guid)) {
    problems.push(`${path}: guid also used by ${guids.get(guid)}`);
  } else {
    guids.set(guid, path);
  }
}

for (const root of roots) {
  walk(root);
  collectGuids(`${root}.meta`);
  if (statSync(root).isDirectory()) {
    collectGuids(root);
  }
}

if (problems.length > 0) {
  console.error(problems.join("\n"));
  process.exit(1);
}
console.log(`${guids.size} .meta files ok`);
