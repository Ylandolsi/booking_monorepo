/**
 * Query keys for stores API
 */

export const STORE_QUERY_KEY = 'stores';
export const PRODUCT_QUERY_KEY = 'products';
export const ORDER_QUERY_KEY = 'orders';
export const PAYMENT_QUERY_KEY = 'payments';
export const PAYOUT_QUERY_KEY = 'payouts';

export const storeKeys = {
  all: [STORE_QUERY_KEY] as const,
  myStore: () => [STORE_QUERY_KEY, 'my'] as const,
  publicStore: (slug: string) => [STORE_QUERY_KEY, 'public', slug] as const,
  slugAvailability: (slug: string) => [STORE_QUERY_KEY, 'slug-availability', slug] as const,
} as const;

export const productKeys = {
  all: [PRODUCT_QUERY_KEY] as const,
  sessions: () => [PRODUCT_QUERY_KEY, 'sessions'] as const,
  session: (productSlug: string) => [PRODUCT_QUERY_KEY, 'sessions', productSlug] as const,
  availability: (productSlug: string, year: number, month: number) => {
    return ['monthly-availability', productSlug, year, month];
  },
} as const;

export const orderKeys = {
  all: [ORDER_QUERY_KEY] as const,
  my: () => [ORDER_QUERY_KEY, 'my'] as const,
  order: (orderId: number) => [ORDER_QUERY_KEY, orderId] as const,
} as const;

export const paymentKeys = {
  all: [PAYMENT_QUERY_KEY] as const,
  wallet: () => [PAYMENT_QUERY_KEY, 'wallet'] as const,
} as const;

export const payoutKeys = {
  all: [PAYOUT_QUERY_KEY] as const,
  history: () => [PAYOUT_QUERY_KEY, 'history'] as const,
  admin: {
    all: () => [PAYOUT_QUERY_KEY, 'admin'] as const,
    payouts: (status?: string) => {
      const key = [PAYOUT_QUERY_KEY, 'admin', 'payouts'];
      return status ? [...key, status] : key;
    },
  },
} as const;
