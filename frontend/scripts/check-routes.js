#!/usr/bin/env node
/**
 * Route Synchronization Checker (fixed)
 *
 * - Normalizes routes (leading slash, no trailing slash except root)
 * - Flattens ROUTE_PATHS into a map: normalizedPath -> [constantKeys...]
 * - Extracts routes from files (resolves ROUTE_PATHS.* references and literals)
 * - Compares unique normalized sets; reports missing and duplicate constants
 *
 * Usage:
 *  node scripts/check-routes.js
 *  node scripts/check-routes.js --strict   # exit non-zero on any mismatch
 */

import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';
import { glob } from 'glob';
import { execSync } from 'child_process';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const projectRoot = path.resolve(__dirname, '..');

async function loadRoutePaths() {
  const generatedPath = path.join(
    projectRoot,
    'src',
    'config',
    'route-paths.generated.json',
  );

  if (!fs.existsSync(generatedPath)) {
    console.log('📄 Generated route paths not found, creating...');
    try {
      execSync('node scripts/generate-route-paths.cjs', {
        cwd: projectRoot,
        stdio: 'inherit',
      });
    } catch (error) {
      throw new Error(
        'Failed to generate route paths. Please run: npm run generate:routes',
      );
    }
  }

  try {
    const content = fs.readFileSync(generatedPath, 'utf8');
    const ROUTE_PATHS = JSON.parse(content);
    if (!ROUTE_PATHS || typeof ROUTE_PATHS !== 'object') {
      throw new Error('Invalid ROUTE_PATHS in generated file');
    }
    return ROUTE_PATHS;
  } catch (error) {
    throw new Error(`Failed to load generated route paths: ${error.message}`);
  }
}

function normalizeRoute(p) {
  if (!p || typeof p !== 'string') return p;
  let out = p.startsWith('/') ? p : `/${p}`;
  if (out.length > 1 && out.endsWith('/')) out = out.slice(0, -1);
  return out;
}

/**
 * Build a map of normalizedPath -> [constantKeyPaths...]
 * e.g. "/auth/login" => ["AUTH.LOGIN"]
 */
function collectRoutePathsMap(obj, prefix = '') {
  const map = new Map();

  function walk(node, keyPath) {
    if (typeof node === 'string') {
      const normalized = normalizeRoute(node);
      const existing = map.get(normalized) || [];
      existing.push(keyPath || '<root>');
      map.set(normalized, existing);
      return;
    }
    if (node && typeof node === 'object') {
      for (const [k, v] of Object.entries(node)) {
        const nextKey = keyPath ? `${keyPath}.${k}` : k;
        walk(v, nextKey);
      }
    }
  }

  walk(obj, '');
  return map;
}

/**
 * Extract route paths used in createFileRoute(...) calls.
 * Supports:
 *   createFileRoute(ROUTE_PATHS.AUTH.LOGIN)
 *   createFileRoute('/auth/login')
 *
 * Notes:
 * - Accepts optional trailing commas and multi-line formatting.
 * - Warns when ROUTE_PATHS.* cannot be resolved from the generated snapshot.
 */
function extractRoutesFromFiles(ROUTE_PATHS) {
  const routeFiles = glob.sync('src/routes/**/*.{ts,tsx}', {
    cwd: projectRoot,
  });
  const extracted = new Set();

  // Allow optional trailing comma and whitespace/newlines between tokens
  const patterns = [
    // matches createFileRoute( ROUTE_PATHS.A.B.C )  OR with trailing comma
    /createFileRoute\s*\(\s*ROUTE_PATHS\.([A-Za-z0-9_]+(?:\.[A-Za-z0-9_]+)*)\s*,?\s*\)/g,
    // matches createFileRoute('/some/path') OR with trailing comma
    /createFileRoute\s*\(\s*['"`]([^'"`]+)['"`]\s*,?\s*\)/g,
  ];

  for (const file of routeFiles) {
    const fullPath = path.resolve(projectRoot, file);
    if (!fs.existsSync(fullPath)) continue;
    const content = fs.readFileSync(fullPath, 'utf8');

    for (const pattern of patterns) {
      let match;
      while ((match = pattern.exec(content)) !== null) {
        const token = match[1];
        if (!token) continue;
        // literal route -> normalize and add
        if (token.startsWith('/')) {
          extracted.add(normalizeRoute(token));
          continue;
        }
        // ROUTE_PATHS constant reference -> resolve from ROUTE_PATHS snapshot
        const keys = token.split('.');
        let value = ROUTE_PATHS;
        let resolved = true;
        for (const k of keys) {
          if (value && typeof value === 'object' && k in value) {
            value = value[k];
          } else {
            resolved = false;
            break;
          }
        }
        if (resolved && typeof value === 'string') {
          extracted.add(normalizeRoute(value));
        } else {
          // More permissive warning: include file & token so you can fix formatting/typo
          console.warn(
            `⚠️  Could not resolve ROUTE_PATHS.${token} referenced in ${file}`,
          );
        }
      }
    }
  }

  return Array.from(extracted).sort();
}

function findDuplicatesFromMap(map) {
  const duplicates = [];
  for (const [route, keys] of map.entries()) {
    if (keys.length > 1) duplicates.push({ route, keys });
  }
  return duplicates;
}

async function validateRoutes() {
  console.log('🔍 Checking route synchronization...\n');

  try {
    const ROUTE_PATHS = await loadRoutePaths();

    const routeMap = collectRoutePathsMap(ROUTE_PATHS); // Map normalized -> [keys]
    const constantRoutes = Array.from(routeMap.keys()).sort();
    const extractedRoutes = extractRoutesFromFiles(ROUTE_PATHS);

    console.log(`📋 Routes in ROUTE_PATHS: ${constantRoutes.length}`);
    console.log(`📁 Routes in files: ${extractedRoutes.length}\n`);

    const missingFromConstants = extractedRoutes.filter(
      (r) => !constantRoutes.includes(r),
    );
    const missingFromFiles = constantRoutes.filter(
      (r) => !extractedRoutes.includes(r),
    );
    const duplicates = findDuplicatesFromMap(routeMap);

    let hasErrors = false;

    if (missingFromConstants.length > 0) {
      hasErrors = true;
      console.log('❌ Routes found in files but missing from ROUTE_PATHS:');
      console.log('====================================================');
      missingFromConstants.forEach((r) => console.log(`- ${r}`));
      console.log('');
      console.log('📝 Suggested additions:');
      missingFromConstants.forEach((route) => {
        const segments = route.split('/').filter(Boolean);
        const constantName = segments
          .map((s) => s.replace(/\$/, '').replace(/[^a-zA-Z0-9]/g, '_'))
          .join('_')
          .toUpperCase();
        console.log(`${constantName}: '${route}',`);
      });
      console.log('');
    }

    if (missingFromFiles.length > 0) {
      hasErrors = true;
      console.log('⚠️  Routes in ROUTE_PATHS but not found in files:');
      console.log('=================================================');
      missingFromFiles.forEach((r) => console.log(`- ${r}`));
      console.log('');
    }

    if (duplicates.length > 0) {
      hasErrors = true;
      console.log(
        '🔄 Duplicate routes in ROUTE_PATHS (same path used by multiple constants):',
      );
      console.log('===================================');
      duplicates.forEach((d) => {
        console.log(`- ${d.route}`);
        d.keys.forEach((k) => console.log(`   • ${k}`));
      });
      console.log('');
    }

    if (!hasErrors) {
      console.log('✅ All routes are properly synchronized!');
      console.log('🎉 ROUTE_PATHS constants match createFileRoute calls.');

      // summary by category
      const categories = {};
      for (const route of constantRoutes) {
        const category = route.split('/')[1] || 'root';
        categories[category] = (categories[category] || 0) + 1;
      }
      console.log('\n📊 Route breakdown:');
      for (const [cat, count] of Object.entries(categories)) {
        console.log(`   ${cat}: ${count} routes`);
      }
    }

    const isStrict = process.argv.includes('--strict');
    if (isStrict && hasErrors) {
      console.log(
        '\n💥 Strict mode: Exiting with error due to synchronization issues.',
      );
      process.exit(1);
    }

    if (hasErrors) {
      console.log(
        '\n💡 Run "npm run generate:routes" to refresh the route snapshot.',
      );
      process.exit(1);
    }

    return !hasErrors;
  } catch (error) {
    console.error('❌ Error during route validation:', error.message);
    process.exit(1);
  }
}

(async () => {
  try {
    await validateRoutes();
  } catch (err) {
    console.error('💥 Fatal error:', err.message);
    process.exit(1);
  }
})();
