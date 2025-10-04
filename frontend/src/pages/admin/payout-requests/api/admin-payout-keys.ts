export const AdminPayoutKeys = {
  allPayouts: (status?: string, upToDate?: string, timeZoneId?: string) => ['admin-payouts', status, upToDate, timeZoneId] as const,
  payoutById: (id: number) => ['admin-payouts', id] as const,
};