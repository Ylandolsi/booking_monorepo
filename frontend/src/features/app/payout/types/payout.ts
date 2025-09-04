//@ts-expect-error : eslint
export enum PayoutStatus {
  Pending = 'Pending',
  Approved = 'Approved',
  Rejected = 'Rejected',
  Completed = 'Completed',
}

export type PayoutType = {
  id: number;
  userId: number;
  konnectWalletId: string;
  walletId: number;
  amount: number;
  paymentRef: string;
  status: PayoutStatus;
  createdAt: string; // utc dateTime
  updatedAt: string; // utc dateTime
};
