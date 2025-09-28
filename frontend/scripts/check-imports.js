#!/usr/bin/env node
/**
 * Run TypeScript type check to surface ONLY import/module resolution errors
 * Filters for TS2307, TS1192, TS2305, TS2304, TS2614, TS2306 error codes
 */
import { spawnSync } from 'child_process';
import path from 'path';

const projectRoot = process.cwd();
const tscCmd = 'npx';
const args = ['tsc', '--noEmit', '-p', 'tsconfig.app.json'];

// Import-related error codes to filter for
const importErrorCodes = ['TS2307', 'TS1192', 'TS2305', 'TS2304', 'TS2614', 'TS2306'];

console.log('🔎 Running TypeScript check (filtering for import issues only)...');
const res = spawnSync(tscCmd, args, {
  cwd: projectRoot,
  stdio: 'pipe', // Capture output instead of inheriting
  shell: true,
  encoding: 'utf8',
});

// Filter output for import-related errors
const lines = res.stdout.split('\n');
const importErrors = lines.filter((line) => {
  return importErrorCodes.some((code) => line.includes(`error ${code}:`));
});

if (importErrors.length > 0) {
  console.log('❌ Found import/module resolution errors:');
  importErrors.forEach((line) => console.log(line));
  console.log(`\nFound ${importErrors.length} import-related error(s)`);
  process.exit(1);
} else {
  console.log('✅ No import/module resolution errors found');
  process.exit(0);
}
