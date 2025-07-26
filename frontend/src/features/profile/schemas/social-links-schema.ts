import z from 'zod';

export const socialLinksSchema = z.object({
  linkedIn: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),

  portfolio: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),
  github: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),
  instagram: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),
  youtube: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),
  facebook: z
    .string()
    .url({ message: 'Please enter a valid URL (e.g., https://example.com)' })
    .optional()
    .or(z.literal('')),
});

export type SocialLinksFormValues = z.infer<typeof socialLinksSchema>;
