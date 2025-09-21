import type { Store } from '@/api/stores';
import { queryOptions, useQuery, type UseQueryOptions, type UseQueryResult } from '@tanstack/react-query';

const initialStore: Store = {
  title: 'Linki Store',
  slug: 'linki-store',
  description: 'Welcome to my Linki store! Here you can find all my digital products and book 1:1 sessions.',
  isPublished: true,
  step: 2,
  socialLinks: [
    { platform: 'twitter', url: 'https://twitter.com/linki' },
    { platform: 'instagram', url: 'https://instagram.com/linki' },
  ],
  products: [],
  createdAt: new Date().toISOString(),
  picture: {
    mainLink: 'https://i.pravatar.cc/150?u=linki',
    thumbnailLink: 'https://i.pravatar.cc/150?u=linki',
  },
};

// Mock API implementations
export const getMyStore = async (): Promise<Store> => {
  console.log('mock: getMyStore');
  return initialStore;
};

export function useMyStore(overrides?: Partial<UseQueryOptions<any, Error>>): UseQueryResult<Store, Error> {
  return useQuery(
    queryOptions({
      queryKey: ['store', 'my-store'],
      queryFn: getMyStore,
      ...overrides,
    }),
  );
}
