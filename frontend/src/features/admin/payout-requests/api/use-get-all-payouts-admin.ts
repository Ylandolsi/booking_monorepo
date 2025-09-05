import {
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { AdminPayoutKeys } from './admin-payout-keys';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { api } from '@/lib';
import { toLocalISOString } from '@/utils/format';

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

const getAllPayoutsAdmin = async (
  status?: string,
  upToDate?: string,
  timeZoneId?: string,
): Promise<Array<AdminPayoutResponse>> => {
  return await api.get<Array<AdminPayoutResponse>>(
    MentorshipEndpoints.Payouts.Admin.GetAllPayouts,
    {
      params: { status, upToDate, timeZoneId },
    },
  );
};

export const useGetAllPayoutsAdmin = (
  status?: string,
  upToDate?: Date,
  timeZoneId?: string,
  overrides?: Partial<UseQueryOptions<Array<AdminPayoutResponse>, Error>>,
): UseQueryResult<Array<AdminPayoutResponse>, Error> => {
  
  const normalizedUpToDate = upToDate ? toLocalISOString(upToDate) : undefined;

  const dateOnly = normalizedUpToDate?.slice(0, 10); // used for cache

  const options: UseQueryOptions<Array<AdminPayoutResponse>, Error> = {
    queryKey: AdminPayoutKeys.allPayouts(status, dateOnly, timeZoneId),
    queryFn: () => getAllPayoutsAdmin(status, normalizedUpToDate, timeZoneId),
    ...overrides,
  };

  return useQuery<Array<AdminPayoutResponse>, Error>(options);
};
