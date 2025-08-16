#!/usr/bin/env node

/**
 * Find Navigation Patterns Script
 * This script helps identify remaining hardcoded navigation patterns that need refactoring
 */

import fs from 'fs';
import path from 'path';
import { glob } from 'glob';

const projectRoot = process.cwd();

console.log('🔍 Scanning for hardcoded navigation patterns...\n');

// Patterns to search for
const patterns = [
  { name: 'Hardcoded to prop', regex: /to=["'`]\/[^"'`]*["'`]/g },
  { name: 'Hardcoded href prop', regex: /href=["'`]\/[^"'`]*["'`]/g },
  { name: 'useNavigate direct usage', regex: /useNavigate\(\)/g },
  { name: 'navigate({ to:', regex: /navigate\(\s*{\s*to:/g },
  { name: 'window.location.href', regex: /window\.location\.href\s*=/g },
  { name: 'String route literals', regex: /["'`]\/[a-zA-Z][^"'`]*["'`]/g },
];

// Files to scan
const files = glob.sync('src/**/*.{ts,tsx}', {
  cwd: projectRoot,
  ignore: [
    'src/**/*.test.{ts,tsx}',
    'src/**/*.spec.{ts,tsx}',
    'src/routeTree.gen.ts',
    'src/config/routes.ts',
    'src/config/route-registry.ts',
    'src/hooks/use-navigation.ts',
    'src/hooks/use-navigation-links.ts',
  ],
});

const results = {};

for (const file of files) {
  const fullPath = path.resolve(projectRoot, file);
  const content = fs.readFileSync(fullPath, 'utf8');

  for (const pattern of patterns) {
    const matches = [...content.matchAll(pattern.regex)];
    if (matches.length > 0) {
      if (!results[file]) results[file] = {};
      if (!results[file][pattern.name]) results[file][pattern.name] = [];

      matches.forEach((match, index) => {
        const lines = content.substring(0, match.index).split('\n');
        const lineNumber = lines.length;
        const lineContent = lines[lines.length - 1] + match[0];

        results[file][pattern.name].push({
          match: match[0],
          line: lineNumber,
          context: lineContent.trim(),
        });
      });
    }
  }
}

// Display results
let totalIssues = 0;
for (const [file, patterns] of Object.entries(results)) {
  console.log(`📁 ${file}`);
  for (const [patternName, matches] of Object.entries(patterns)) {
    console.log(`   ${patternName}:`);
    matches.forEach((match) => {
      console.log(`     Line ${match.line}: ${match.match}`);
      totalIssues++;
    });
  }
  console.log('');
}

if (totalIssues === 0) {
  console.log('✅ No hardcoded navigation patterns found!');
} else {
  console.log(
    `⚠️  Found ${totalIssues} patterns that could be refactored to use centralized navigation.`,
  );
}

console.log('\n💡 Refactoring suggestions:');
console.log('   - Replace useNavigate() with useAppNavigation()');
console.log('   - Replace hardcoded to="/path" with routes.to.*');
console.log('   - Replace hardcoded href="/path" with navigationLinks.*');
console.log(
  '   - Replace window.location.href with nav.goToExternal() or nav.navigateToRoute()',
);
