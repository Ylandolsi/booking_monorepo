#!/usr/bin/env node

/**
 * Route Path Generator
 * Generates a JSON snapshot of ROUTE_PATHS from src/config/routes.ts
 * This allows the validation scripts to work without needing TypeScript compilation
 */

const path = require('path');
const fs = require('fs');

const projectRoot = path.resolve(__dirname, '..');
const routesPath = path.join(projectRoot, 'src', 'config', 'routes.ts');
const outputPath = path.join(
  projectRoot,
  'src',
  'config',
  'route-paths.generated.json',
);

async function generateRoutePaths() {
  try {
    // Check if the routes file exists
    if (!fs.existsSync(routesPath)) {
      throw new Error(`Routes file not found: ${routesPath}`);
    }

    // First, try to use esbuild for faster compilation
    try {
      const esbuild = require('esbuild');

      // Build the TypeScript file to a temporary JS file
      const tempOutputPath = path.join(projectRoot, 'temp-routes.mjs');

      await esbuild.build({
        entryPoints: [routesPath],
        bundle: false,
        platform: 'node',
        format: 'esm',
        outfile: tempOutputPath,
        target: 'node18',
        allowOverwrite: true,
      });

      // Import the compiled module
      const { pathToFileURL } = require('url');
      const routesModule = await import(pathToFileURL(tempOutputPath));
      const ROUTE_PATHS = routesModule.ROUTE_PATHS;

      // Clean up temp file
      fs.unlinkSync(tempOutputPath);

      if (!ROUTE_PATHS || typeof ROUTE_PATHS !== 'object') {
        throw new Error(
          'ROUTE_PATHS not found or not an object in src/config/routes.ts',
        );
      }

      // Write the JSON snapshot
      fs.writeFileSync(
        outputPath,
        JSON.stringify(ROUTE_PATHS, null, 2),
        'utf8',
      );

      console.log(`✅ Generated route paths snapshot:`);
      console.log(`   Input: ${path.relative(projectRoot, routesPath)}`);
      console.log(`   Output: ${path.relative(projectRoot, outputPath)}`);
      console.log(
        `   Routes found: ${Object.keys(flattenRoutes(ROUTE_PATHS)).length}`,
      );

      process.exit(0);
    } catch (esbuildError) {
      console.log('📦 esbuild not available, trying ts-node...');

      // Fallback to ts-node with ESM support
      try {
        require('ts-node').register({
          transpileOnly: true,
          esm: true,
          compilerOptions: {
            module: 'ESNext',
            target: 'ES2020',
            moduleResolution: 'node',
          },
        });

        // Create a dynamic import wrapper for CommonJS
        const importDynamic = new Function(
          'specifier',
          'return import(specifier)',
        );
        const routesModule = await importDynamic(routesPath);
        const ROUTE_PATHS = routesModule.ROUTE_PATHS;

        if (!ROUTE_PATHS || typeof ROUTE_PATHS !== 'object') {
          throw new Error(
            'ROUTE_PATHS not found or not an object in src/config/routes.ts',
          );
        }

        // Write the JSON snapshot
        fs.writeFileSync(
          outputPath,
          JSON.stringify(ROUTE_PATHS, null, 2),
          'utf8',
        );

        console.log(`✅ Generated route paths snapshot:`);
        console.log(`   Input: ${path.relative(projectRoot, routesPath)}`);
        console.log(`   Output: ${path.relative(projectRoot, outputPath)}`);
        console.log(
          `   Routes found: ${Object.keys(flattenRoutes(ROUTE_PATHS)).length}`,
        );

        process.exit(0);
      } catch (tsNodeError) {
        throw new Error(
          `Both esbuild and ts-node failed:\n- esbuild: ${esbuildError.message}\n- ts-node: ${tsNodeError.message}`,
        );
      }
    }
  } catch (error) {
    console.error('❌ Failed to generate route paths:', error.message);
    console.error('\nTroubleshooting:');
    console.error(
      '1. Make sure src/config/routes.ts exists and exports ROUTE_PATHS',
    );
    console.error(
      '2. Install required dependencies: npm install --save-dev esbuild (recommended) or ts-node typescript',
    );
    console.error(
      '3. Check that ROUTE_PATHS is a plain object (no functions or computed values)',
    );
    process.exit(1);
  }
}

// Helper to flatten routes for counting
function flattenRoutes(obj, prefix = '') {
  const routes = {};
  for (const [key, value] of Object.entries(obj)) {
    if (typeof value === 'string') {
      routes[prefix ? `${prefix}.${key}` : key] = value;
    } else if (typeof value === 'object' && value !== null) {
      Object.assign(
        routes,
        flattenRoutes(value, prefix ? `${prefix}.${key}` : key),
      );
    }
  }
  return routes;
}

// Self-executing script
if (require.main === module) {
  generateRoutePaths();
}

module.exports = { generateRoutePaths };
