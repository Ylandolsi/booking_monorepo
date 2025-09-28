import { toFormData } from '@/api/utils';
import z from 'zod';

export const patchPostStoreSchema = z.object({
  title: z.string().min(3, 'Store name must be at least 3 characters'),
  slug: z
    .string()
    .min(3, 'Slug must be at least 3 characters')
    .regex(/^[a-z0-9-]+$/, 'Slug can only contain lowercase letters, numbers, and hyphens'),
  description: z.string().optional(),
  file: z.instanceof(File).optional(),
  socialLinks: z
    .array(
      z.object({
        platform: z.string(),
        url: z.string().url('Invalid URL'),
      }),
    )
    .optional(),
});

export type PatchPostStoreRequest = z.infer<typeof patchPostStoreSchema>;

export interface PatchPostStoreResponse {
  slug: string;
}

export const patchPostStoreSchemaToFormData = (data: PatchPostStoreRequest) => {
  const formData = toFormData({
    title: data.title,
    slug: data.slug,
    file: data.file || new File([], ''),
    description: data.description || '',
    socialLinksJson: data.socialLinks,
  });

  return formData;
};
