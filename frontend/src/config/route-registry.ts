/**
 * Route Registry
 * This file contains all route definitions mapped to their file paths
 * Use this to understand the route structure and ensure consistency
 */

import { ROUTE_PATHS } from '@/config/routes';

// Feature-based route grouping
export const ROUTE_GROUPS = {
  AUTH: [
    ROUTE_PATHS.AUTH.LOGIN,
    ROUTE_PATHS.AUTH.REGISTER,
    ROUTE_PATHS.AUTH.FORGOT_PASSWORD,
    ROUTE_PATHS.AUTH.RESET_PASSWORD,
    ROUTE_PATHS.AUTH.EMAIL_VERIFICATION,
    ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED,
  ],

  BOOKING: [
    ROUTE_PATHS.BOOKING.SESSION,
    ROUTE_PATHS.BOOKING.DEMO,
    ROUTE_PATHS.BOOKING.ENHANCED,
    ROUTE_PATHS.BOOKING.TEST,
  ],

  MENTOR: [ROUTE_PATHS.MENTOR.BECOME, ROUTE_PATHS.MENTOR.SET_SCHEDULE],

  PROFILE: [ROUTE_PATHS.PROFILE.USER],

  APP: [ROUTE_PATHS.APP.INDEX],

  TEST: [
    ROUTE_PATHS.ERROR_EXP.SIMPLE_LOADING,
    ROUTE_PATHS.ERROR_EXP.ADVANCED_LOADING,
    ROUTE_PATHS.TEST.MENTOR_REQUIRED,
    ROUTE_PATHS.TEST.IMG,
    ROUTE_PATHS.TEST.DASHBOARD,
    ROUTE_PATHS.TEST.BOOKING_DEMO,
    ROUTE_PATHS.TEST.ALREADY,
  ],
} as const;

// Protected routes (require authentication)
export const PROTECTED_ROUTES = [
  ROUTE_PATHS.APP.INDEX,
  ROUTE_PATHS.BOOKING.SESSION,
  ROUTE_PATHS.BOOKING.DEMO,
  ROUTE_PATHS.BOOKING.ENHANCED,
  ROUTE_PATHS.MENTOR.BECOME,
  ROUTE_PATHS.MENTOR.SET_SCHEDULE,
  ROUTE_PATHS.PROFILE.USER,
] as const;

// Public routes (no authentication required)
export const PUBLIC_ROUTES = [
  ROUTE_PATHS.ROOT,
  ROUTE_PATHS.AUTH.LOGIN,
  ROUTE_PATHS.AUTH.REGISTER,
  ROUTE_PATHS.AUTH.FORGOT_PASSWORD,
  ROUTE_PATHS.AUTH.RESET_PASSWORD,
  ROUTE_PATHS.AUTH.EMAIL_VERIFICATION,
  ROUTE_PATHS.AUTH.EMAIL_VERIFICATION_VERIFIED,
  ROUTE_PATHS.UNAUTHORIZED,
  ...ROUTE_GROUPS.TEST,
] as const;

// Route utilities
export const routeUtils = {
  isProtectedRoute: (path: string): boolean => {
    return PROTECTED_ROUTES.some((route) => {
      // Handle dynamic routes with parameters
      const routePattern = route.replace(/\$\w+/g, '[^/]+');
      const regex = new RegExp(`^${routePattern}$`);
      return regex.test(path);
    });
  },

  isPublicRoute: (path: string): boolean => {
    return PUBLIC_ROUTES.some((route) => {
      const routePattern = route.replace(/\$\w+/g, '[^/]+');
      const regex = new RegExp(`^${routePattern}$`);
      return regex.test(path);
    });
  },

  getRouteGroup: (path: string): keyof typeof ROUTE_GROUPS | null => {
    for (const [groupName, routes] of Object.entries(ROUTE_GROUPS)) {
      if (
        routes.some((route) => {
          const routePattern = route.replace(/\$\w+/g, '[^/]+');
          const regex = new RegExp(`^${routePattern}$`);
          return regex.test(path);
        })
      ) {
        return groupName as keyof typeof ROUTE_GROUPS;
      }
    }
    return null;
  },
};

export default {
  ROUTE_PATHS,
  ROUTE_GROUPS,
  PROTECTED_ROUTES,
  PUBLIC_ROUTES,
  routeUtils,
};
