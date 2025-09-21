import type { SocialLink } from '@/api/stores/stores';
import type { Picture } from '@/api/stores/types';
import { useMutation } from '@tanstack/react-query';

interface UpdateStoreInput {
  Title: string;
  Orders?: Record<string, number>;
  Description?: string;
  Picture?: Picture;
  SocialLinks?: readonly SocialLink[];
}

interface UpdateStoreResponse {
  slug: string;
}

export const updateStore = async (data: UpdateStoreInput): Promise<UpdateStoreResponse> => {
  console.log('mock: updateStore with data:', data);
  return { slug: 'slug-123' };
};

export const useUpdateStore = () => {
  return useMutation({
    mutationFn: (data: UpdateStoreInput) => updateStore(data),
    meta: {
      invalidatesQuery: ['store', 'my-store'],
      successMessage: 'Store updated successfully!',
    },
  });
};
