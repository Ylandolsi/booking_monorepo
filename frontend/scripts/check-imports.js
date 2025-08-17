#!/usr/bin/env node
/**
 * Run TypeScript type check to surface import/module resolution errors
 * (TS2307 etc). This is the most reliable way to detect broken imports.
 */
import { spawnSync } from 'child_process';
import path from 'path';

const projectRoot = process.cwd();
const tscCmd = 'npx';
const args = ['tsc', '--noEmit', '-p', 'tsconfig.json'];

console.log('🔎 Running TypeScript check (tsc --noEmit)...');
const res = spawnSync(tscCmd, args, {
  cwd: projectRoot,
  stdio: 'inherit',
  shell: true,
});

process.exit(res.status ?? 1);
