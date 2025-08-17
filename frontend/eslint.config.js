import js from '@eslint/js';
import globals from 'globals';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import unicornPlugin from 'eslint-plugin-unicorn';
import importPlugin from 'eslint-plugin-import';
import prettierPlugin from 'eslint-plugin-prettier';
import checkFilePlugin from 'eslint-plugin-check-file';

export default tseslint.config(
  { ignores: ['dist'] },
  {
    files: ['**/*.{ts,tsx}'],
    settings: {
      'import/resolver': {
        typescript: {
          alwaysTryTypes: true,
          project: ['./tsconfig.json', './tsconfig.app.json'],
        },
      },
    },
    extends: [js.configs.recommended, ...tseslint.configs.recommended],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
      parserOptions: {
        project: ['./tsconfig.app.json', './tsconfig.node.json'],
        tsconfigRootDir: import.meta.dirname,
      },
    },
    plugins: {
      'react-hooks': reactHooks,
      'react-refresh': reactRefresh,
      import: importPlugin,
      prettier: prettierPlugin,
      'check-file': checkFilePlugin,
    },
    rules: {
      'import/no-unresolved': [
        'error',
        { commonjs: true, caseSensitive: false },
      ],
      'import/no-cycle': ['error', { maxDepth: 10 }],

      ...reactHooks.configs.recommended.rules,
      'react-refresh/only-export-components': [
        'warn',
        { allowConstantExport: true },
      ],
      '@typescript-eslint/naming-convention': [
        'error',
        {
          selector: 'class',
          format: ['PascalCase'],
          leadingUnderscore: 'forbid',
          trailingUnderscore: 'forbid',
        },
        {
          selector: 'interface',
          format: ['PascalCase'],
          leadingUnderscore: 'forbid',
          trailingUnderscore: 'forbid',
        },
        {
          selector: 'typeAlias',
          format: ['PascalCase'],
          leadingUnderscore: 'forbid',
          trailingUnderscore: 'forbid',
        },
      ],
      // 'import/no-restricted-paths': [
      //   'error',
      //   {
      //     zones: [
      //       // Prevent cross-feature imports - features should be isolated
      //       {
      //         target: './src/features/auth',
      //         from: './src/features',
      //         except: ['./auth'],
      //       },
      //       {
      //         target: './src/features/profile',
      //         from: './src/features',
      //         except: ['./profile'],
      //       },
      //       // Add more feature restrictions as needed
      //       // {
      //       //   target: './src/features/dashboard',
      //       //   from: './src/features',
      //       //   except: ['./dashboard'],
      //       // },

      //       // Enforce unidirectional architecture
      //       // Features and app can't import from each other directly
      //       {
      //         target: './src/features',
      //         from: './src/app',
      //       },

      //       // Shared modules should not import from features or app
      //       {
      //         target: [
      //           './src/components',
      //           './src/hooks',
      //           './src/lib',
      //           './src/types',
      //           './src/utils',
      //         ],
      //         from: ['./src/features', './src/app'],
      //       },

      //       // ENFORCE BARREL EXPORTS - Prevent direct internal imports
      //       // Block direct imports from feature internals (force barrel exports)
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/components',
      //       },
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/hooks',
      //       },
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/services',
      //       },
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/utils',
      //       },
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/types',
      //       },
      //       {
      //         target: ['./src/app', './src/features', './src/components'],
      //         from: './src/features/*/api',
      //       },

      //       // Block direct imports from shared module internals
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/components/ui',
      //       },
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/components/forms',
      //       },
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/hooks/use-*',
      //       },
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/lib/api',
      //       },
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/lib/utils',
      //       },
      //       {
      //         target: ['./src/app', './src/features'],
      //         from: './src/utils/helpers',
      //       },
      //     ],
      //   },
      // ],
      'import/no-cycle': 'error',
      'linebreak-style': ['error', 'unix'],
      'react/prop-types': 'off',

      // Alternative approach: Use import/no-restricted-paths with more specific patterns
      // This will catch direct internal imports that bypass barrel exports
      // 'no-restricted-imports': [
      //   'error',
      //   {
      //     patterns: [
      //       {
      //         group: [
      //           '@/features/*/components/*',
      //           '!@/features/*/components/index',
      //         ],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from components folder',
      //       },
      //       {
      //         group: ['@/features/*/hooks/*', '!@/features/*/hooks/index'],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from hooks folder',
      //       },
      //       {
      //         group: [
      //           '@/features/*/services/*',
      //           '!@/features/*/services/index',
      //         ],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from services folder',
      //       },
      //       {
      //         group: ['@/features/*/utils/*', '!@/features/*/utils/index'],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from utils folder',
      //       },
      //       {
      //         group: ['@/features/*/types/*', '!@/features/*/types/index'],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from types folder',
      //       },
      //       {
      //         group: ['@/features/*/api/*', '!@/features/*/api/index'],
      //         message:
      //           'Import from feature barrel export (@/features/[feature]) instead of directly from api folder',
      //       },
      //       // Enforce barrel exports for shared modules
      //       {
      //         group: [
      //           '@/components/ui/*',
      //           '!@/components/ui/index',
      //           '!@/components/index',
      //         ],
      //         message:
      //           'Import from component barrel export (@/components) instead of directly from ui folder',
      //       },
      //       {
      //         group: [
      //           '@/components/forms/*',
      //           '!@/components/forms/index',
      //           '!@/components/index',
      //         ],
      //         message:
      //           'Import from component barrel export (@/components) instead of directly from forms folder',
      //       },
      //       {
      //         group: ['@/lib/*/[!index]*', '!@/lib/index'],
      //         message:
      //           'Import from lib barrel export (@/lib) instead of directly from internal folders',
      //       },
      //       {
      //         group: ['@/utils/*/[!index]*', '!@/utils/index'],
      //         message:
      //           'Import from utils barrel export (@/utils) instead of directly from internal folders',
      //       },
      //     ],
      //   },
      // ],

      'import/default': 'off',
      'import/no-named-as-default-member': 'off',
      'import/no-named-as-default': 'off',
      'react/react-in-jsx-scope': 'off',
      'jsx-a11y/anchor-is-valid': 'off',
      '@typescript-eslint/no-unused-vars': ['error'],
      '@typescript-eslint/explicit-function-return-type': ['off'],
      '@typescript-eslint/explicit-module-boundary-types': ['off'],
      '@typescript-eslint/no-empty-function': ['off'],
      '@typescript-eslint/no-explicit-any': ['off'],

      // Enable prettier integration
      // 'prettier/prettier': ['error', {}, { usePrettierrc: true }],
      'check-file/filename-naming-convention': [
        'error',
        {
          '**/*.{ts,tsx}': 'KEBAB_CASE',
        },
        {
          ignoreMiddleExtensions: true,
          ignore: ['**/_*.*'], // ignore any file starting with _
        },
      ],
    },
  },
  {
    files: ['**/*.{ts,tsx,js,jsx}'],
    plugins: {
      unicorn: unicornPlugin,
    },
    rules: {
      'unicorn/filename-case': [
        'error',
        {
          case: 'kebabCase',
          ignore: [/vite-env\.d\.ts$/, /index\.ts$/, /index\.tsx$/],
          multipleFileExtensions: true,
        },
      ],
    },
  },
);
