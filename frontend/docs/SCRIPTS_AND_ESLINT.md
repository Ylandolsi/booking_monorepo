# Scripts & ESLint Rules Documentation

This documentation explains the utility scripts and ESLint rules that help maintain route synchronization in the centralized routing system.

## Overview

The routing system provides three main tools to ensure consistency between `ROUTE_PATHS` constants and actual route usage:

1. **Route Generator** - Creates a JSON snapshot of routes from TypeScript
2. **Route Checker** - Validates synchronization between constants and files
3. **Route Extractor** - Analyzes route usage in the codebase
4. **ESLint Rule** - Enforces route constants usage and prevents string literals

---

## Scripts

### 1. Route Generator (`scripts/generate-route-paths.cjs`)

**Purpose**: Creates a JSON snapshot of `ROUTE_PATHS` from `src/config/routes.ts` for use by other scripts.

**Why it exists**:

- Avoids TypeScript compilation issues when importing .ts files from Node.js scripts
- Provides a single source of truth for route validation scripts
- Works with both ESM and CommonJS environments

**How it works**:

1. Uses `esbuild` (preferred) or `ts-node` to compile TypeScript
2. Imports the compiled module to extract `ROUTE_PATHS`
3. Writes a JSON snapshot to `src/config/route-paths.generated.json`
4. Counts and reports the number of routes found

**Usage**:

```bash
npm run generate:routes
```

**Output**:

```json
{
  "ROOT": "/",
  "AUTH": {
    "LOGIN": "/auth/login",
    "REGISTER": "/auth/register"
  }
  // ... all other routes
}
```

**Dependencies**:

- `esbuild` (recommended) or `ts-node + typescript` (fallback)

---

### 2. Route Checker (`scripts/check-routes.js`)

**Purpose**: Validates that `ROUTE_PATHS` constants match actual `createFileRoute()` calls in the codebase.

**Why it exists**:

- Ensures routes stay synchronized as the app grows
- Catches missing or unused route constants
- Provides CI/CD integration for automated validation
- Suggests fixes for route mismatches

**How it works**:

1. Loads `ROUTE_PATHS` from generated JSON snapshot (calls generator if needed)
2. Scans `src/routes/**/*.{ts,tsx}` files using glob patterns
3. Extracts route usage via regex:
   - `createFileRoute(ROUTE_PATHS.AUTH.LOGIN)` → resolves constant
   - `createFileRoute('/auth/login')` → direct string literal
4. Compares the two sets and reports mismatches

**Usage**:

```bash
npm run check:routes          # Reports issues, exits 0
npm run check:routes:ci       # Strict mode, exits 1 on any issues
```

**Output Example**:

```
🔍 Checking route synchronization...

📋 Routes in ROUTE_PATHS: 26
📁 Routes in files: 24

❌ Routes found in files but missing from ROUTE_PATHS:
====================================================
- /new/route/not/in/constants

📝 Suggested additions to ROUTE_PATHS:
=====================================
NEW_ROUTE_NOT_IN_CONSTANTS: '/new/route/not/in/constants',

⚠️  Routes in ROUTE_PATHS but not found in files:
=================================================
- /unused/route

✅ All routes are properly synchronized!
📊 Route breakdown:
   auth: 6 routes
   booking: 4 routes
   app: 2 routes
```

**Regex Patterns**:

- `createFileRoute\s*\(\s*ROUTE_PATHS\.([A-Z0-9_]+(?:\.[A-Z0-9_]+)*)\s*\)/g`
- `createFileRoute\s*\(\s*['"`]([^'"`]+)['"`]\s\*\)/g`

---

### 3. Route Extractor (`scripts/extract-routes.js`)

**Purpose**: Analyzes route usage to show migration progress and compliance.

**Why it exists**:

- Shows how many routes use constants vs string literals
- Helps track migration progress from hardcoded strings to constants
- Provides simple overview of routing system health

**How it works**:

1. Scans all route files for `createFileRoute()` calls
2. Categorizes usage into constants vs literals
3. Reports summary statistics

**Usage**:

```bash
npm run extract:routes
```

**Output Example**:

```
🔄 Extracting routes from createFileRoute calls...

📁 Found 26 route files

📊 Summary:
   Routes using constants: 24
   Routes using literals: 0

✅ All routes are using ROUTE_PATHS constants!
🎉 Your routing system is properly centralized.
```

---

## ESLint Rule

### Route Constants Enforcement (`eslint-rules/enforce-route-constants.js`)

**Purpose**: Prevents string literals in `createFileRoute()` calls and enforces `ROUTE_PATHS` usage.

**Why it exists**:

- Catches violations during development (before CI)
- Auto-fixes string literals to use constants
- Integrates with existing linting workflow
- Provides real-time feedback in editors

**How it works**:

1. Loads `ROUTE_PATHS` from generated JSON snapshot (with fallback to hardcoded paths)
2. Analyzes AST for `createFileRoute()` calls
3. Reports violations when string literals are used
4. Auto-fixes by replacing literals with constants
5. Auto-imports `ROUTE_PATHS` if missing

**Rule Behavior**:

- ❌ `createFileRoute('/auth/login')` → Reports error
- ✅ `createFileRoute(ROUTE_PATHS.AUTH.LOGIN)` → Passes
- 🔧 Auto-fix: Replaces string with constant and adds import

**Configuration Options**:

```javascript
'local/enforce-route-constants': [
  'error', // or 'warn'
  {
    allowStringLiterals: false,  // Strict mode
    enforceImport: true,         // Auto-add imports
  },
]
```

**Auto-fix Example**:

```typescript
// Before (triggers rule)
createFileRoute('/auth/login');

// After auto-fix
import { ROUTE_PATHS } from '@/config/routes';
createFileRoute(ROUTE_PATHS.AUTH.LOGIN);
```

**Integration** (`eslint-rules/route-config.js`):

- **Development**: Warnings only, doesn't break workflow
- **CI**: Strict errors, fails build on violations
- **Legacy**: Allows literals during migration period

---

## Workflow Integration

### Development Workflow

```bash
# 1. Developer adds new route file
touch src/routes/new/feature.tsx

# 2. Use constants in route file
createFileRoute(ROUTE_PATHS.NEW.FEATURE)

# 3. Add route to constants
# Edit src/config/routes.ts to add NEW.FEATURE: '/new/feature'

# 4. Validate (runs automatically on save with ESLint)
npm run check:routes
```

### CI/CD Integration

```yaml
# Add to GitHub Actions or similar
- name: Validate Routes
  run: |
    npm run generate:routes
    npm run check:routes:ci
    npm run lint  # includes ESLint rule
```

### Package.json Scripts

```json
{
  "scripts": {
    "generate:routes": "node scripts/generate-route-paths.cjs",
    "check:routes": "node scripts/check-routes.js",
    "check:routes:ci": "node scripts/check-routes.js --strict",
    "extract:routes": "node scripts/extract-routes.js",
    "prebuild": "npm run generate:routes && npm run check:routes"
  }
}
```

---

## Architecture Decisions

### Why JSON Snapshot Instead of Direct Import?

1. **ESM Compatibility**: Avoids import issues with TypeScript files in Node.js
2. **Performance**: JSON parsing is faster than TypeScript compilation
3. **CI Reliability**: No dependency on TypeScript compilation in scripts
4. **Simplicity**: One-time generation, multiple consumption

### Why Multiple Scripts Instead of One?

1. **Single Responsibility**: Each script has a focused purpose
2. **Flexibility**: Can run validation without generation, or extraction independently
3. **Debug-ability**: Easier to troubleshoot specific issues
4. **Performance**: Skip expensive operations when not needed

### Why Both Regex and AST Parsing?

1. **Speed**: Regex is fast for simple pattern matching
2. **Robustness**: AST would be more accurate but adds complexity
3. **Good Enough**: Current regex patterns cover 99% of real usage
4. **Maintainability**: Regex is easier to understand and modify

---

## Troubleshooting

### Common Issues

**"Failed to generate route paths"**

- Install dependencies: `npm install --save-dev esbuild`
- Check that `src/config/routes.ts` exports `ROUTE_PATHS`
- Ensure `ROUTE_PATHS` is a plain object (no functions/computed values)

**"Route found in files but missing from ROUTE_PATHS"**

- Add the missing route to `src/config/routes.ts`
- Run `npm run generate:routes` to update snapshot
- Re-run validation

**"ESLint rule not working"**

- Run `npm run generate:routes` to create the JSON snapshot
- Check ESLint configuration includes the custom rule
- Verify the rule is pointing to correct file path

**"TypeScript compilation errors"**

- Scripts work with source files, not compiled output
- Ensure `src/config/routes.ts` has valid TypeScript syntax
- Check for circular dependencies in route imports

---

## Future Improvements

### Potential Enhancements

1. **AST-based Parsing**: Replace regex with TypeScript compiler API for robustness
2. **Route Type Generation**: Auto-generate TypeScript types from route definitions
3. **IDE Integration**: VS Code extension for real-time route validation
4. **Route Tree Visualization**: Generate visual route hierarchy diagrams
5. **Dead Route Detection**: Find route files that are never referenced
6. **Dynamic Route Validation**: Validate parameterized routes match their definitions

### Integration Ideas

1. **Pre-commit Hooks**: Run route validation before each commit
2. **Hot Reload Integration**: Update route snapshot during development
3. **Bundle Analysis**: Report route bundle sizes and code splitting points
4. **Documentation Generation**: Auto-generate API docs from route definitions

---

## Summary

These tools provide a comprehensive system for maintaining route consistency:

- **Generator**: Creates machine-readable snapshot of routes
- **Checker**: Validates synchronization and suggests fixes
- **Extractor**: Reports migration progress and compliance
- **ESLint Rule**: Prevents violations during development

Together, they ensure your centralized routing system stays reliable as your application grows, catching issues early and providing clear guidance for fixes.
