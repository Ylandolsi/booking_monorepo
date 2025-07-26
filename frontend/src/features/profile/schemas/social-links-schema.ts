import z from 'zod';

export const socialLinksSchema = z.object({
  linkedIn: z.string().url().optional().or(z.literal('')),
  portfolio: z.string().url().optional().or(z.literal('')),
  github: z.string().url().optional().or(z.literal('')),
  instagram: z.string().url().optional().or(z.literal('')),
  youtube: z.string().url().optional().or(z.literal('')),
  facebook: z.string().url().optional().or(z.literal('')),
});

export type SocialLinksFormValues = z.infer<typeof socialLinksSchema>;
