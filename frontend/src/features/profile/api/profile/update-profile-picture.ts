import { api } from '@/lib';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ProfilePictureType } from '@/features/profile/types';
import { authQueryKeys } from '@/features/auth';
import * as Endpoints from '@/lib/user-endpoints.ts';

async function updateProfilePicture(
  body: FormData,
): Promise<ProfilePictureType> {
  return await api.put<ProfilePictureType>(
    Endpoints.UpdateProfilePicture,
    body,
  );
}

export function useUpdateProfilePicture() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: ({ data }: { data: FormData }) => updateProfilePicture(data),
    meta: {
      successMessage: 'Profile picture updated successfully',
      errorMessage: 'Failed to update profile picture',
      successAction: (data: ProfilePictureType) => {
        console.log('Profile picture updated:', data);
        queryClient.setQueryData(authQueryKeys.currentUser(), (old: any) => {
          if (!old) return old;
          return {
            ...old,
            profilePicture: {
              ...old.profilePicture,
              profilePictureLink: data.profilePictureLink,
              thumbnailUrlPictureLink: data.thumbnailUrlPictureLink,
            },
          };
        });
      },
      errorAction: () => {
        queryClient.invalidateQueries({
          queryKey: authQueryKeys.currentUser(),
        });
      },
    },
  });
}
