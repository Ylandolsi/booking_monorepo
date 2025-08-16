/**
 * ESLint Rule: Enforce Route Constants
 * This rule ensures that createFileRoute() calls use ROUTE_PATHS constants
 * instead of string literals
 */

const { error } = require('console');

module.exports = {
  meta: {
    type: 'problem',
    docs: {
      description:
        'Enforce use of ROUTE_PATHS constants in createFileRoute calls',
      category: 'Best Practices',
      recommended: true,
    },
    fixable: 'code',
    schema: [
      {
        type: 'object',
        properties: {
          allowStringLiterals: {
            type: 'boolean',
            default: false,
          },
          enforceImport: {
            type: 'boolean',
            default: true,
          },
        },
        additionalProperties: false,
      },
    ],
    messages: {
      useRouteConstants:
        'Use ROUTE_PATHS.{{constantName}} instead of string literal "{{routePath}}"',
      missingImport:
        'Import ROUTE_PATHS from "@/config/routes" when using route constants',
      unknownRoute:
        'Route "{{routePath}}" not found in ROUTE_PATHS. Add it to the constants or use a valid route.',
    },
  },

  create(context) {
    const options = context.options[0] || {};
    const allowStringLiterals = options.allowStringLiterals || false;
    const enforceImport = options.enforceImport !== false;

    let hasRoutePathsImport = false;
    let routePathsImportName = 'ROUTE_PATHS';

    // Load ROUTE_PATHS dynamically from generated JSON file
    let ROUTE_PATHS = {};
    try {
      const fs = require('fs');
      const path = require('path');

      // Try to find the generated route paths JSON
      const workingDir = context.getCwd ? context.getCwd() : process.cwd();
      const generatedPath = path.join(
        workingDir,
        'src',
        'config',
        'route-paths.generated.json',
      );

      if (fs.existsSync(generatedPath)) {
        const content = fs.readFileSync(generatedPath, 'utf8');
        ROUTE_PATHS = JSON.parse(content);
      } else {
        throw error();
      }
    } catch (error) {
      console.error('ESLint rule error loading ROUTE_PATHS:', error.message);
      // Use empty object to prevent crashes
      ROUTE_PATHS = {};
    }

    /**
     * Flatten ROUTE_PATHS to create a path -> constant mapping
     */
    function flattenPaths(obj, prefix = '') {
      const result = {};
      for (const [key, value] of Object.entries(obj)) {
        const currentPath = prefix ? `${prefix}.${key}` : key;
        if (typeof value === 'string') {
          result[value] = currentPath;
        } else if (typeof value === 'object') {
          Object.assign(result, flattenPaths(value, currentPath));
        }
      }
      return result;
    }

    const pathToConstant = flattenPaths(ROUTE_PATHS);

    return {
      ImportDeclaration(node) {
        if (node.source.value === '@/config/routes') {
          const routePathsSpecifier = node.specifiers.find(
            (spec) => spec.imported && spec.imported.name === 'ROUTE_PATHS',
          );
          if (routePathsSpecifier) {
            hasRoutePathsImport = true;
            routePathsImportName = routePathsSpecifier.local.name;
          }
        }
      },

      CallExpression(node) {
        // Check if this is a createFileRoute call
        if (
          node.callee.name === 'createFileRoute' &&
          node.arguments.length === 1
        ) {
          const arg = node.arguments[0];

          // Check for string literal usage
          if (arg.type === 'Literal' && typeof arg.value === 'string') {
            if (!allowStringLiterals) {
              const routePath = arg.value;
              const constantName = pathToConstant[routePath];

              if (constantName) {
                context.report({
                  node: arg,
                  messageId: 'useRouteConstants',
                  data: {
                    routePath,
                    constantName: `${routePathsImportName}.${constantName}`,
                  },
                  fix(fixer) {
                    return fixer.replaceText(
                      arg,
                      `${routePathsImportName}.${constantName}`,
                    );
                  },
                });
              } else {
                context.report({
                  node: arg,
                  messageId: 'unknownRoute',
                  data: { routePath },
                });
              }
            }
          }

          // Check for missing import when using constants
          if (
            arg.type === 'MemberExpression' &&
            arg.object.name === 'ROUTE_PATHS' &&
            !hasRoutePathsImport &&
            enforceImport
          ) {
            context.report({
              node: arg,
              messageId: 'missingImport',
              fix(fixer) {
                const sourceCode = context.getSourceCode();
                const firstImport = sourceCode.ast.body.find(
                  (stmt) => stmt.type === 'ImportDeclaration',
                );

                if (firstImport) {
                  const importStatement = `import { ROUTE_PATHS } from '@/config/routes';\n`;
                  return fixer.insertTextBefore(firstImport, importStatement);
                }
                return null;
              },
            });
          }
        }
      },
    };
  },
};
