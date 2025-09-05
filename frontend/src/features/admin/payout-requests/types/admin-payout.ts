// Admin payout types that match the backend PayoutResponse
export interface AdminPayoutResponse {
  id: number;
  userId: number;
  konnectWalletId: string;
  walletId: number;
  amount: number;
  paymentRef: string;
  status: 'Pending' | 'Approved' | 'Rejected' | 'Completed';
  createdAt: string;
  updatedAt: string;
}

// API response types
export interface ApprovePayoutAdminResponse {
  payUrl: string;
}

// Request payload types
export interface ApprovePayoutRequest {
  PayoutId: number;
}

export interface RejectPayoutRequest {
  PayoutId: number;
}

// UI-specific types for the admin payout pages
export type PayoutStatus = 'pending' | 'approved' | 'rejected' | 'completed';
export type TimeFilter = 'today' | 'last_hour' | 'last_3_days' | 'all';

// Helper function to convert backend status to frontend status
export const mapPayoutStatus = (backendStatus: AdminPayoutResponse['status']): PayoutStatus => {
  return backendStatus.toLowerCase() as PayoutStatus;
};