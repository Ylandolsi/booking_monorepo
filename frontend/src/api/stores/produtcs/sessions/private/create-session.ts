import { patchPostSessionSchemaToFormData } from '@/api/stores/produtcs';
import type {
  CreateProductInput,
  CreateSessionProductRequest,
  PatchPostSessionResponse,
} from '@/api/stores/produtcs/sessions/private/schema-session';
import { storeKeys } from '@/api/stores/stores-keys';
import { api, CatalogEndpoints, validateFile } from '@/api/utils';
import { useMutation } from '@tanstack/react-query';

export const createSession = async ({ data }: { data: CreateProductInput }) => {
  if (data.thumbnailImage) {
    const validation = validateFile(data.thumbnailImage);

    if (!validation.isValid) {
      throw new Error(validation.error || 'Invalid file');
    }
  }

  // Create FormData for the request
  const formData = patchPostSessionSchemaToFormData(data as CreateSessionProductRequest);

  try {
    const response = await api.post<PatchPostSessionResponse>(CatalogEndpoints.Products.Sessions.Create, formData);

    return response;
  } catch (error) {
    console.error('Error creating session:', error);
    throw error;
  }
};

export const useCreateSession = () => {
  return useMutation<PatchPostSessionResponse, Error, { data: CreateProductInput }>({
    mutationFn: createSession,
    meta: {
      invalidatesQuery: [storeKeys.myStore()],
      successMessage: 'Session created successfully!',
    },
  });
};
