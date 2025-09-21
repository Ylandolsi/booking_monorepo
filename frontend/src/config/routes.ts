/**
 * Centralized Route Definitions
 * This file contains all route paths, param placeholders, and utilities for the application
 */

// Param placeholders
export const ROUTE_PARAMS = {
  MENTOR_SLUG: '$mentorSlug',
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

  // Store routes
  STORE: {
    ROOT: '/store',
    INDEX: '/store/index',
    DASHBOARD: '/store/dashboard',
    VIEW: '/store/$storeSlug',
    PRODUCT: '/store/$storeSlug/$productSlug',
  },

  // App routes
  APP: {
    INDEX: '/app',
    LINKI: '/app/linki',
    STORE: '/app/store',
    BOOKING: {
      SESSION: `/app/booking/session/${ROUTE_PARAMS.MENTOR_SLUG}`,
    },

    // Mentor routes
    MENTOR: {
      BECOME: '/app/mentor/become',
      SET_SCHEDULE: '/app/mentor/set-schedule',
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

  // Booking routes
  booking: {
    session: (mentorSlug: string) => ROUTE_PATHS.APP.BOOKING.SESSION.replace(ROUTE_PARAMS.MENTOR_SLUG, mentorSlug),
  },

  meets: {
    index: () => ROUTE_PATHS.APP.MEETS.INDEX,
  },

  // Mentor routes
  mentor: {
    become: () => ROUTE_PATHS.APP.MENTOR.BECOME,
    setSchedule: () => ROUTE_PATHS.APP.MENTOR.SET_SCHEDULE,
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
  isMentorRoute: (path: string) => path.startsWith('/mentor'),
  isProfileRoute: (path: string) => path.startsWith('/profile'),
  isTestRoute: (path: string) => path.startsWith('/test') || path.startsWith('/error-exp'),

  // is protected route (app, booking, mentor, profile)
  // isPublic route
};

// Export default for convenience
export default routes;
