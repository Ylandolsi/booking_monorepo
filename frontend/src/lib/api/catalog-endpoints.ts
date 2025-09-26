/**
 * Catalog API Endpoints
 * Based on backend CatalogEndpoints.cs
 */

const BASE = 'api';

export const CatalogEndpoints = {
  Stores: {
    Create: `${BASE}/stores`,
    Update: `${BASE}/stores`,
    CheckSlugAvailability: (slug: string) => `${BASE}/stores/slug-availability/${slug}`,
    UpdatePicture: `${BASE}/stores/picture`,
    GetMy: `${BASE}/stores/me`,
    GetPublic: (slug: string) => `${BASE}/stores/${slug}`,
  },

  Products: {
    Sessions: {
      Create: `${BASE}/products/s/create`,
      Update: (productSlug: string) => `${BASE}/products/s/${productSlug}`,
      GetMy: (productSlug: string) => `${BASE}/products/s/${productSlug}/private`,

      GetSessions: `${BASE}/s`, // Get booked sessions for store owner
      // Public endpoints
      Get: (productSlug: string) => `${BASE}/products/s/${productSlug}`,
      Book: (productSlug: string) => `${BASE}/products/s/${productSlug}`,
      GetDailyAvailability: (productSlug: string) => `${BASE}/products/s/${productSlug}/availability`,
      GetMonthlyAvailability: (productSlug: string) => `${BASE}/products/s/${productSlug}/availability/month`,
    },
  },

  Orders: {
    Get: (orderId: number) => `${BASE}/orders/${orderId}`,
    GetMy: `${BASE}/orders/me`,
  },

  Payouts: {
    Payout: `${BASE}/payout`,
    PayoutHistory: `${BASE}/payout`,
    Admin: {
      ApprovePayout: `${BASE}/admin/payout/approve`,
      WebhookPayout: `${BASE}/admin/payout/webhook`,
      RejectPayout: `${BASE}/admin/payout/reject`,
      GetAllPayouts: `${BASE}/admin/payout`,
    },
  },

  Payment: {
    Create: `${BASE}/payments`,
    GetWallet: `${BASE}/payments/wallet`,
    Webhook: `${BASE}/payments/webhook`,
  },
} as const;

/**
 * Query parameter builders for API endpoints
 */
export const QueryBuilders = {
  Stores: {
    /**
     * Build query parameters for checking slug availability
     */
    slugAvailability: (slug: string) => ({ slug }),
  },

  Products: {
    Sessions: {
      /**
       * Build query parameters for daily availability
       */
      dailyAvailability: (date: string) => ({ date }),

      /**
       * Build query parameters for monthly availability
       */
      monthlyAvailability: (year: number, month: number, timeZoneId: string = 'Africa/Tunis') => ({
        year,
        month,
        timeZoneId,
      }),
    },
  },

  Payouts: {
    Admin: {
      /**
       * Build query parameters for getting all payouts
       */
      getAllPayouts: (status?: 'Pending' | 'Approved' | 'Rejected' | 'Completed', upToDate?: string, timeZoneId?: string) => ({
        ...(status && { status }),
        ...(upToDate && { upToDate }),
        ...(timeZoneId && { timeZoneId }),
      }),
    },
  },
} as const;
