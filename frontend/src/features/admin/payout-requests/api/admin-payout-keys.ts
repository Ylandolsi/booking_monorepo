export const AdminPayoutKeys = {
  allPayouts: () => ['admin-payouts'] as const,
  payoutById: (id: number) => ['admin-payouts', id] as const,
};