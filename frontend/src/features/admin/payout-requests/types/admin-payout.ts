// Admin payout types that match the backend PayoutResponse

import type { AdminPayoutResponse } from "../api";


// UI-specific types for the admin payout pages
export type PayoutStatus = 'pending' | 'approved' | 'rejected' | 'completed';
export type TimeFilter = 'today' | 'last_hour' | 'last_3_days' | 'all';

// Helper function to convert backend status to frontend status
export const mapPayoutStatus = (backendStatus: AdminPayoutResponse['status']): PayoutStatus => {
  return backendStatus.toLowerCase() as PayoutStatus;
};