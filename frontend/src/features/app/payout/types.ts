export interface PayoutRequest {
  id: string;
  amount: number;
  status: 'pending' | 'approved' | 'rejected' | 'completed';
  requestDate: string;
  method: string;
}

export interface PayoutHistory {
  id: string;
  amount: number;
  status: 'completed' | 'pending' | 'failed';
  date: string;
  transactionId: string;
  method: string;
}

export interface WalletBalance {
  balance: number;
  currency: string;
  lastUpdated: string;
}