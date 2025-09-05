import {
  useQuery,
  type UseQueryOptions,
  type UseQueryResult,
} from '@tanstack/react-query';
import { AdminPayoutKeys } from './admin-payout-keys';
import { MentorshipEndpoints } from '@/lib/mentor-endpoints';
import { api } from '@/lib';

import type { AdminPayoutResponse } from '../types';

const getAllPayoutsAdmin = async (): Promise<Array<AdminPayoutResponse>> => {
  return await api.get<Array<AdminPayoutResponse>>(
    MentorshipEndpoints.Payouts.Admin.GetAllPayouts,
  );
};

export const useGetAllPayoutsAdmin = (
  overrides?: Partial<UseQueryOptions<Array<AdminPayoutResponse>, Error>>,
): UseQueryResult<Array<AdminPayoutResponse>, Error> => {
  const options: UseQueryOptions<Array<AdminPayoutResponse>, Error> = {
    queryKey: AdminPayoutKeys.allPayouts(),
    queryFn: getAllPayoutsAdmin,
    ...overrides,
  };

  return useQuery<Array<AdminPayoutResponse>, Error>(options);
};