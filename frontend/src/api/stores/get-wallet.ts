import { api, CatalogEndpoints } from '@/api/utils';
import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';

async function getWallet(): Promise<Wallet> {
  const res = await api.get<Wallet>(CatalogEndpoints.Payment.GetWallet);
  return res;
}

export const WalletKeys = {
  wallet: () => ['wallet'] as const,
};

export function useGetWallet(overrides?: Partial<UseQueryOptions<Wallet, Error>>): UseQueryResult<Wallet, Error> {
  return useQuery<Wallet, Error>(
    queryOptions({
      queryFn: getWallet,
      queryKey: WalletKeys.wallet(),
      ...overrides,
    }),
  );
}

export type Wallet = {
  balance: number;
  pendingBalance: number;
};
