/**
 * Navigation Utilities
 * This file provides utilities for programmatic navigation using the centralized routes
 */

import { useNavigate } from '@tanstack/react-router';
import { routes } from '@/config/routes';

// Custom hook for typed navigation
export function useAppNavigation() {
  const navigate = useNavigate();

  return {
    goTo: navigate,
    // Auth navigation
    goToLogin: (params?: { redirectTo?: string }) => {
      navigate({ to: routes.to.auth.login(params) });
    },

    goToRegister: (params?: { redirectTo?: string }) => {
      navigate({ to: routes.to.auth.register(params) });
    },

    goToForgotPassword: (params?: { redirectTo?: string }) => {
      navigate({ to: routes.to.auth.forgotPassword(params) });
    },

    goToResetPassword: (params: { redirectTo?: string; email?: string; token?: string }) => {
      navigate({ to: routes.to.auth.resetPassword(params) });
    },

    goToEmailVerification: (params?: { redirectTo?: string; email?: string; token?: string }) => {
      navigate({ to: routes.to.auth.emailVerification(params) });
    },

    goToEmailVerificationVerified: (params?: { redirectTo?: string }) => {
      navigate({ to: routes.to.auth.emailVerificationVerified(params) });
    },

    // App navigation
    goToApp: () => {
      navigate({ to: routes.to.store.index() + '/' });
    },

    goToPayout: () => {
      navigate({ to: routes.to.payment.payout() });
    },

    goToIntegrations: () => {
      navigate({ to: routes.to.integrations() });
    },

    // my meets
    goToMeets: () => {
      navigate({ to: routes.to.meets.index() });
    },

    // Admin navigation
    goToAdminPayoutRequests: () => {
      navigate({ to: routes.to.admin.payoutRequests() });
    },

    goToAdminPayoutRequestDetails: (requestId: string) => {
      navigate({ to: routes.to.admin.payoutRequestDetails(requestId) });
    },

    // Test navigation
    goToTestDashboard: () => {
      navigate({ to: routes.to.test.dashboard() });
    },

    goToTestImg: () => {
      navigate({ to: routes.to.test.img() });
    },

    // Utility navigation
    goToUnauthorized: () => {
      navigate({ to: routes.to.unauthorized() });
    },

    goToHome: () => {
      navigate({ to: routes.to.home() });
    },

    goToSupport: () => {
      // TODO : fix this
      navigate({ to: routes.to.home() });
    },
    // Generic navigation with replace option
    navigateToRoute: (routePath: string, options?: { replace?: boolean }) => {
      navigate({
        to: routePath,
        replace: options?.replace ?? false,
      });
    },

    // Navigate with state
    navigateWithState: (routePath: string, state: any, options?: { replace?: boolean }) => {
      navigate({
        to: routePath,
        replace: options?.replace ?? false,
        state,
      });
    },

    // Go back
    goBack: () => {
      window.history.back();
    },

    // External navigation
    goToExternal: (url: string, newTab = false) => {
      if (newTab) {
        window.open(url, '_blank', 'noopener,noreferrer');
      } else {
        window.location.href = url;
      }
    },
  };
}

// Utility functions for route checking
export const navigationUtils = {
  // Check if current path matches a route
  isCurrentRoute: (currentPath: string, targetRoute: string): boolean => {
    // Handle dynamic routes
    const routePattern = targetRoute.replace(/\$\w+/g, '[^/]+');
    const regex = new RegExp(`^${routePattern}$`);
    return regex.test(currentPath);
  },

  // Get breadcrumb trail for current route
  getBreadcrumbs: (currentPath: string): Array<{ label: string; path: string }> => {
    const pathSegments = currentPath.split('/').filter(Boolean);
    const breadcrumbs: Array<{ label: string; path: string }> = [{ label: 'Home', path: routes.to.home() }];

    let currentRoute = '';
    pathSegments.forEach((segment) => {
      currentRoute += `/${segment}`;

      // Skip dynamic segments for breadcrumb labels
      if (segment.startsWith('$')) return;

      // Capitalize and format segment for display
      const label = segment
        .split('-')
        .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
        .join(' ');

      breadcrumbs.push({
        label,
        path: currentRoute,
      });
    });

    return breadcrumbs;
  },

  // Extract route parameters from path
  extractRouteParams: (path: string, routePattern: string): Record<string, string> => {
    const pathSegments = path.split('/').filter(Boolean);
    const patternSegments = routePattern.split('/').filter(Boolean);
    const params: Record<string, string> = {};

    patternSegments.forEach((segment, index) => {
      if (segment.startsWith('$')) {
        const paramName = segment.slice(1);
        const paramValue = pathSegments[index];
        if (paramValue) {
          params[paramName] = paramValue;
        }
      }
    });

    return params;
  },
};

// Export both the hook and utilities
export default {
  useAppNavigation,
  navigationUtils,
};
