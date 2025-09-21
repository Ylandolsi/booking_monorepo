import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import * as Endpoints from '@/api/auth/auth-endpoints';
import type { SocialLinksType } from '@/features/app/profile';
import { authQueryKeys } from '@/api/auth';

async function UpdateSocialLinks(data: SocialLinksType): Promise<void> {
  await api.put<void>(Endpoints.UpdateSocialLinks, data);
}

export function useUpdateSocialLinks() {
  return useMutation({
    mutationFn: ({ data }: { data: SocialLinksType }) => UpdateSocialLinks(data),
    meta: {
      invalidatesQuery: [authQueryKeys.currentUser()],
      successMessage: 'Social links updated succesfully',
      errorMessage: 'Failed to update social links ',
    },
  });
}
