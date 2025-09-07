import {
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { PayoutKeys } from '@/features/app/payout/api/payout-keys';
import type { PayoutType } from '@/features/app/payout/types/payout';
import { MentorshipEndpoints } from '@/lib/api/mentor-endpoints';
import { api } from '@/lib';

const getHistoryPayout = async (): Promise<Array<PayoutType>> => {
  return await api.get<Array<PayoutType>>(
    MentorshipEndpoints.Payouts.PayoutHistory,
  );
};

export const useHistoryPayout = (
  overrides?: Partial<UseQueryOptions<Array<PayoutType>, Error>>,
): UseQueryResult<Array<PayoutType>, Error> => {
  const options: UseQueryOptions<Array<PayoutType>, Error> = {
    queryKey: PayoutKeys.history(),
    queryFn: getHistoryPayout,
    ...overrides,
  };

  return useQuery<Array<PayoutType>, Error>(options);
};
