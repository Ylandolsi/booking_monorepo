import { api } from '@/lib';
import { useMutation } from '@tanstack/react-query';
import * as Endpoints from '@/lib/endpoints.ts';
import type { BasicInfoType } from '../../types';

async function updateBasicInfo(data: BasicInfoType): Promise<void> {
  await api.put<void>(Endpoints.UpdateBasicInfo, data);
}

export function useUpdateBasicInfo() {
  return useMutation({
    mutationFn: ({ data }: { data: BasicInfoType }) => updateBasicInfo(data),
    meta: {
      invalidatesQuery: [['user']],
      successMessage: 'Basic info updated succesfully',
      errorMessage: 'Failed to update basic info ',
    },
  });
}
