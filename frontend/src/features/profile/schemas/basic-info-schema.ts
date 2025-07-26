import z from 'zod';

export const basicInfoSchema = z
  .object({
    firstName: z.string().min(1, 'First name is required').max(50),
    lastName: z.string().min(1, 'Second name is required').max(50),
    gender: z.string(),
    languages: z
      .array(z.string())
      .min(1, 'Select at least one language')
      .max(4, 'You can select up to 4 languages'),
    bio: z.string().max(500, 'Bio must be at most 500 characters').optional(),
    // picture: z.any().optional(), // File upload handling is usually separate
  })
  .refine(
    (data) => {
      console.log(data.gender);
      if (!['Female', 'Male'].includes(data.gender)) {
        return false;
      }
      return true;
    },
    {
      message: 'Gender must be either Male or Female',
      path: ['gender'],
    },
  );

export type BasicInfoFormValues = z.infer<typeof basicInfoSchema>;
