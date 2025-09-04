import z from 'zod';

export const mentorCreateFormSchema = z.object({
  hourlyRate: z
    .string()
    .min(1, 'Hourly rate is required')
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Hourly rate must be a positive number',
    })
    .transform((val) => Number(val)),
  bufferTimeMinutes: z
    .string()
    .min(1, 'Buffer time is required')
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Buffer time must be a positive number',
    })
    .transform((val) => Number(val)),
});

export type MentorCreateFormData = z.infer<typeof mentorCreateFormSchema>;
