import type { SocialLink, Store } from '@/api/stores';

export interface CreateStoreInput {
  title: string;
  slug: string;
  description?: string;
  picture?: File;
  socialLinks?: SocialLink[];
}

export interface CreateStoreResponse {
  slug: string;
}

export const createStore = async (data: CreateStoreInput): Promise<CreateStoreResponse> => {
  console.log(' createStore with data:', data);

  return { slug: 'slug-123' };
};

export const useCreateStore = () => {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: (data: CreateStoreInput) => createStore(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: [STORE_QUERY_KEY] });
    },
  });
};
