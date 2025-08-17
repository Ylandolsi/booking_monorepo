/**
 * Centralized Route Definitions
 * This file contains all route paths, param placeholders, and utilities for the application
 */

// Param placeholders
export const ROUTE_PARAMS = {
  MENTOR_SLUG: '$mentorSlug',
  USER_SLUG: '$userSlug',
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
    EMAIL_VERIFICATION_VERIFIED: '/auth/email-verification/verified',
  },

  // App routes
  APP: {
    INDEX: '/app',
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
      USER: `/app/profile/${ROUTE_PARAMS.USER_SLUG}`,
    },
  },

  // Error/Test routes
  ERROR_EXP: {
    SIMPLE_LOADING: '/error-exp/simple-loading-demo',
    ADVANCED_LOADING: '/error-exp/advanced-loading-examples',
  },

  TEST: {
    MENTOR_REQUIRED: '/test/mentor-required',
    IMG: '/test/img',
    DASHBOARD: '/test/dashboard',
    BOOKING_DEMO: '/test/booking-demo',
    ALREADY: '/test/already',
  },

  // Utility routes
  UNAUTHORIZED: '/unauthorized',
} as const;

// Type for route paths
export type RoutePath = typeof ROUTE_PATHS;

// Route parameter types
export interface RouteParams {
  mentorSlug?: string;
  userSlug?: string;
}

// Query parameter types
export interface QueryParams {
  redirectTo?: string;
  email?: string;
  token?: string;
}

// Route builder utilities
export const routeBuilder = {
  // Auth routes
  auth: {
    login: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo
        ? `?redirectTo=${encodeURIComponent(params.redirectTo)}`
        : '';
      return `${ROUTE_PATHS.AUTH.LOGIN}${query}`;
    },
    register: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo
        ? `?redirectTo=${encodeURIComponent(params.redirectTo)}`
        : '';
      return `${ROUTE_PATHS.AUTH.REGISTER}${query}`;
    },
    forgotPassword: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo
        ? `?redirectTo=${encodeURIComponent(params.redirectTo)}`
        : '';
      return `${ROUTE_PATHS.AUTH.FORGOT_PASSWORD}${query}`;
    },
    resetPassword: (params?: {
      redirectTo?: string;
      email?: string;
      token?: string;
    }) => {
      const searchParams = new URLSearchParams();
      if (params?.redirectTo) searchParams.set('redirectTo', params.redirectTo);
      if (params?.email) searchParams.set('email', params.email);
      if (params?.token) searchParams.set('token', params.token);
      const query = searchParams.toString()
        ? `?${searchParams.toString()}`
        : '';
      return `${ROUTE_PATHS.AUTH.RESET_PASSWORD}${query}`;
    },
    emailVerification: (params?: {
      redirectTo?: string;
      email?: string;
      token?: string;
    }) => {
      const searchParams = new URLSearchParams();
      if (params?.redirectTo) searchParams.set('redirectTo', params.redirectTo);
      if (params?.email) searchParams.set('email', params.email);
      if (params?.token) searchParams.set('token', params.token);
      const query = searchParams.toString()
        ? `?${searchParams.toString()}`
        : '';
      return `${ROUTE_PATHS.AUTH.EMAIL_VERIFICATION}${query}`;
    },
    emailVerificationVerified: (params?: { redirectTo?: string }) => {
      const query = params?.redirectTo
        ? `?redirectTo=${encodeURIComponent(params.redirectTo)}`
        : '';
      return `${ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED}${query}`;
    },
  },

  // App routes
  app: {
    root: () => ROUTE_PATHS.APP.INDEX,
  },

  // Booking routes
  booking: {
    session: (mentorSlug: string) =>
      ROUTE_PATHS.APP.BOOKING.SESSION.replace(
        ROUTE_PARAMS.MENTOR_SLUG,
        mentorSlug,
      ),
  },

  // Mentor routes
  mentor: {
    become: () => ROUTE_PATHS.APP.MENTOR.BECOME,
    setSchedule: () => ROUTE_PATHS.APP.MENTOR.SET_SCHEDULE,
  },

  // Profile routes
  profile: {
    user: (userSlug: string) =>
      ROUTE_PATHS.APP.PROFILE.USER.replace(ROUTE_PARAMS.USER_SLUG, userSlug),
  },

  // Error/Test routes
  errorExp: {
    simpleLoading: () => ROUTE_PATHS.ERROR_EXP.SIMPLE_LOADING,
    advancedLoading: () => ROUTE_PATHS.ERROR_EXP.ADVANCED_LOADING,
  },

  test: {
    mentorRequired: () => ROUTE_PATHS.TEST.MENTOR_REQUIRED,
    img: () => ROUTE_PATHS.TEST.IMG,
    dashboard: () => ROUTE_PATHS.TEST.DASHBOARD,
    bookingDemo: () => ROUTE_PATHS.TEST.BOOKING_DEMO,
    already: () => ROUTE_PATHS.TEST.ALREADY,
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
  isTestRoute: (path: string) =>
    path.startsWith('/test') || path.startsWith('/error-exp'),
};

// Export default for convenience
export default routes;
