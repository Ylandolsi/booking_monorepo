// /**
//  * ESLint Configuration for Route Constants Enforcement
//  *
//  * This configuration adds the custom rule to enforce ROUTE_PATHS usage
//  * and provides different presets for development and CI environments.
//  */

// const path = require('path');

// // Custom rule configuration
// const routeConstantsRule = {
//   'local/enforce-route-constants': [
//     'error',
//     {
//       allowStringLiterals: false,
//       enforceImport: true,
//     },
//   ],
// };

// module.exports = {
//   // Base configuration
//   base: {
//     plugins: ['local'],
//     rules: {
//       ...routeConstantsRule,
//     },
//     settings: {
//       'local-rules': {
//         'enforce-route-constants': path.resolve(
//           __dirname,
//           './enforce-route-constants.js',
//         ),
//       },
//     },
//   },

//   // Development mode (warnings only)
//   development: {
//     plugins: ['local'],
//     rules: {
//       'local/enforce-route-constants': [
//         'warn',
//         {
//           allowStringLiterals: false,
//           enforceImport: true,
//         },
//       ],
//     },
//     settings: {
//       'local-rules': {
//         'enforce-route-constants': path.resolve(
//           __dirname,
//           './enforce-route-constants.js',
//         ),
//       },
//     },
//   },

//   // CI mode (strict errors)
//   ci: {
//     plugins: ['local'],
//     rules: {
//       'local/enforce-route-constants': [
//         'error',
//         {
//           allowStringLiterals: false,
//           enforceImport: true,
//         },
//       ],
//     },
//     settings: {
//       'local-rules': {
//         'enforce-route-constants': path.resolve(
//           __dirname,
//           './enforce-route-constants.js',
//         ),
//       },
//     },
//   },

//   // Legacy mode (allow string literals during migration)
//   legacy: {
//     plugins: ['local'],
//     rules: {
//       'local/enforce-route-constants': [
//         'warn',
//         {
//           allowStringLiterals: true,
//           enforceImport: false,
//         },
//       ],
//     },
//     settings: {
//       'local-rules': {
//         'enforce-route-constants': path.resolve(
//           __dirname,
//           './enforce-route-constants.js',
//         ),
//       },
//     },
//   },
// };
