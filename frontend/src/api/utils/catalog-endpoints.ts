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
      GetAllSessionsMonthly: () => `${BASE}/products/s`, // queryParam :string? month ,string?year , string? timeZoneId

      // Public endpoints
      Get: (productSlug: string) => `${BASE}/products/s/${productSlug}`,
      Book: (productSlug: string) => `${BASE}/products/s/${productSlug}`,
      GetMonthlyAvailability: (productSlug: string) => `${BASE}/products/s/${productSlug}/availability/month`, // with query params year, month timeZoneId
    },

    Arrange: `${BASE}/produtcs/arrange`, // Note: typo in backend endpoint
    Delete: (productSlug: string) => `${BASE}/products/${productSlug}`,
    TogglePublished: (productSlug: string) => `${BASE}/products/toggle/${productSlug}`,
  },

  Orders: {
    Get: (orderId: number) => `${BASE}/orders/${orderId}`,
    GetMy: `${BASE}/orders/me`,
    GetOrders: `${BASE}/orders`, // query params: startsAt, endsAt
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

      /**
       * Build query parameters for getting all sessions
       */
      getAllSessions: (year: number, month: number, timeZoneId: string = 'Africa/Tunis') => ({
        year,
        month,
        timeZoneId,
      }),
    },
  },

  Orders: {
    /**
     * Build query parameters for getting orders
     */
    getOrders: (startsAt?: Date, endsAt?: Date) => ({
      ...(startsAt && { startsAt: startsAt.toISOString() }),
      ...(endsAt && { endsAt: endsAt.toISOString() }),
    }),
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
