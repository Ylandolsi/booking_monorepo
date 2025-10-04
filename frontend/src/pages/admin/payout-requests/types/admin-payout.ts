// Admin payout types that match the backend PayoutResponse

import type { AdminPayoutResponse } from "../api";


// UI-specific types for the admin payout pages
export type PayoutStatus = 'pending' | 'approved' | 'rejected' | 'completed';
export type TimeFilter = 'Last24Hours' | 'LastHour' | 'Last3Days' | 'All';

// Helper function to convert backend status to frontend status
export const mapPayoutStatus = (backendStatus: AdminPayoutResponse['status']): PayoutStatus => {
  return backendStatus.toLowerCase() as PayoutStatus;
};