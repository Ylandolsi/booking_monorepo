import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import * as Endpoints from '@/lib/endpoints.ts';
import type { SocialLinksType } from '@/features/profile';
import { authQueryKeys } from '@/features/auth';

async function UpdateSocialLinks(data: SocialLinksType): Promise<void> {
  await api.put<void>(Endpoints.UpdateSocialLinks, data);
}

export function useUpdateSocialLinks() {
  return useMutation({
    mutationFn: ({ data }: { data: SocialLinksType }) =>
      UpdateSocialLinks(data),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Social links updated succesfully',
      errorMessage: 'Failed to update social links ',
    },
  });
}
