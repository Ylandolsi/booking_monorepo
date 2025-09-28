/**
 * Centralized Route Definitions
 * This file contains all route paths, param placeholders, and utilities for the application
 */

import type { ProductType } from '@/api/stores';
import { buildUrlWithParams } from '@/api/utils';

// Param placeholders
export const ROUTE_PARAMS = {
  // StoreSlug and ProductSlug are used in multiple places : todo
  USER_SLUG: '$userSlug',
  REQUEST_ID: '$requestId',
} as const;

// Route path constants
export const ROUTE_PATHS = {
  // Root
  ROOT: '/',

  // Auth routes
  AUTH: {
    LOGIN: '/auth/login',
    REGISTER: '/auth/register',
    FORGOT_PASSWORD: '/auth/forgot-password',
    RESET_PASSWORD: '/auth/reset-password',
    EMAIL_VERIFICATION: '/auth/email-verification',
    EMAIL_VERIFICATION_VERIFIED: '/auth/email-verified',
  },

  // App routes
  APP: {
    INDEX: '/app',
    LINKI: '/app/linki',

    STORE: {
      INDEX: '/app/store/', // show all products and store preview mobile with add product button
      PRODUCT: {
        VIEW: '/app/store/product/$productSlug', // view product details page
        // type : ?type=booking|digital
        INDEX: '/app/store/product', // add product flow : select type -> details -> specific fields
      },

      INFO_EDIT: '/app/store/edit', // store header info edit page
      CREATE_STORE: '/app/store/create', // create store flow
      SETUP_STORE: '/app/store/setup', // initial setup wizard after creating store
      //DASHBOARD: '/app/store/dashboard', // manage products, orders, settings

      // Public store preview
      PUBLIC_PREVIEW: '/store/$storeSlug',
      PUBLIC_SESSION_PRODUCT: '/store/$storeSlug/s/$productSlug',
    },

    // Profile routes
    PROFILE: {
      INTEGRATIONS: `/app/integration`,
    },

    // meets
    MEETS: {
      INDEX: `/app/meets`,
    },
    PAYMENT: {
      PAYOUT: `/app/payouts`,
    },

    // Admin routes
    ADMIN: {
      PAYOUT_REQUESTS: '/app/admin/payout-requests',
      PAYOUT_REQUEST_DETAILS: `/app/admin/payout-requests/${ROUTE_PARAMS.REQUEST_ID}`,
    },
  },

  // Error/Test routes
  ERROR_EXP: {
    SIMPLE_LOADING: '/error-exp/simple-loading-demo',
    ADVANCED_LOADING: '/error-exp/advanced-loading-examples',
  },

  TEST: {
    IMG: '/test/img',
    DASHBOARD: '/test/dashboard',
  },

  // Utility routes
  UNAUTHORIZED: '/unauthorized',
} as const;

// Type for route paths
export type RoutePath = typeof ROUTE_PATHS;

// Route builder utilities
export const routeBuilder = {
  // Auth routes
  auth: {
    login: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo ? `?redirectTo=${encodeURIComponent(params.redirectTo)}` : '';
      return `${ROUTE_PATHS.AUTH.LOGIN}${query}`;
    },
    register: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo ? `?redirectTo=${encodeURIComponent(params.redirectTo)}` : '';
      return `${ROUTE_PATHS.AUTH.REGISTER}${query}`;
    },
    forgotPassword: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo ? `?redirectTo=${encodeURIComponent(params.redirectTo)}` : '';
      return `${ROUTE_PATHS.AUTH.FORGOT_PASSWORD}${query}`;
    },
    resetPassword: (params?: { redirectTo?: string; email?: string; token?: string }) => {
      const searchParams = new URLSearchParams();
      if (params?.redirectTo) searchParams.set('redirectTo', params.redirectTo);
      if (params?.email) searchParams.set('email', params.email);
      if (params?.token) searchParams.set('token', params.token);
      const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
      return `${ROUTE_PATHS.AUTH.RESET_PASSWORD}${query}`;
    },
    emailVerification: (params?: { redirectTo?: string; email?: string; token?: string }) => {
      const searchParams = new URLSearchParams();
      if (params?.redirectTo) searchParams.set('redirectTo', params.redirectTo);
      if (params?.email) searchParams.set('email', params.email);
      if (params?.token) searchParams.set('token', params.token);
      const query = searchParams.toString() ? `?${searchParams.toString()}` : '';
      return `${ROUTE_PATHS.AUTH.EMAIL_VERIFICATION}${query}`;
    },
    emailVerificationVerified: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo ? `?redirectTo=${encodeURIComponent(params.redirectTo)}` : '';
      return `${ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED}${query}`;
    },
  },

  // App routes
  app: {
    root: () => ROUTE_PATHS.APP.INDEX,
  },

  store: {
    index: () => ROUTE_PATHS.APP.STORE.INDEX,
    productView: ({ productSlug }: { productSlug: string }) => ROUTE_PATHS.APP.STORE.PRODUCT.VIEW.replace('$productSlug', productSlug),

    productAdd: (params: { type?: ProductType | undefined }) => {
      return buildUrlWithParams(ROUTE_PATHS.APP.STORE.PRODUCT.INDEX, params);
    },
    productEdit: (params: { type?: ProductType; productSlug: string }) => {
      return buildUrlWithParams(ROUTE_PATHS.APP.STORE.PRODUCT.INDEX, params);
    },
    editStoreInfo: () => ROUTE_PATHS.APP.STORE.INFO_EDIT,
    createStore: () => ROUTE_PATHS.APP.STORE.CREATE_STORE,
    setupStore: () => ROUTE_PATHS.APP.STORE.SETUP_STORE,

    // public store preview
    publicStorePreview: ({ storeSlug }: { storeSlug: string }) => ROUTE_PATHS.APP.STORE.PUBLIC_PREVIEW.replace('$storeSlug', storeSlug),
    publicSessionProduct: ({ storeSlug, productSlug }: { storeSlug: string; productSlug: string }) =>
      ROUTE_PATHS.APP.STORE.PUBLIC_SESSION_PRODUCT.replace('$storeSlug', storeSlug).replace('$productSlug', productSlug),
  },

  meets: {
    index: () => ROUTE_PATHS.APP.MEETS.INDEX,
  },

  profile: {
    integrations: () => ROUTE_PATHS.APP.PROFILE.INTEGRATIONS,
  },

  payment: {
    payout: () => ROUTE_PATHS.APP.PAYMENT.PAYOUT,
  },

  // Admin routes
  admin: {
    payoutRequests: () => ROUTE_PATHS.APP.ADMIN.PAYOUT_REQUESTS,
    payoutRequestDetails: (requestId: string) => ROUTE_PATHS.APP.ADMIN.PAYOUT_REQUEST_DETAILS.replace(ROUTE_PARAMS.REQUEST_ID, requestId),
  },

  // Error/Test routes
  errorExp: {
    simpleLoading: () => ROUTE_PATHS.ERROR_EXP.SIMPLE_LOADING,
    advancedLoading: () => ROUTE_PATHS.ERROR_EXP.ADVANCED_LOADING,
  },

  test: {
    img: () => ROUTE_PATHS.TEST.IMG,
    dashboard: () => ROUTE_PATHS.TEST.DASHBOARD,
  },

  // Utility routes
  unauthorized: () => ROUTE_PATHS.UNAUTHORIZED,
  home: () => ROUTE_PATHS.ROOT,
};

// Route navigation helpers
export const routes = {
  // Paths - direct path constants
  paths: ROUTE_PATHS,

  // Param placeholders
  params: ROUTE_PARAMS,

  // Builders - functions to generate routes with parameters
  to: routeBuilder,

  // Utility functions
  isAuthRoute: (path: string) => path.startsWith('/auth'),
  isAppRoute: (path: string) => path.startsWith('/app'),
  isBookingRoute: (path: string) => path.startsWith('/booking'),
  isProfileRoute: (path: string) => path.startsWith('/profile'),
  isTestRoute: (path: string) => path.startsWith('/test') || path.startsWith('/error-exp'),

  // is protected route (app, booking, mentor, profile)
  // isPublic route
};

// Export default for convenience
export default routes;
