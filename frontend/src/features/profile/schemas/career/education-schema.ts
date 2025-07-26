import z from 'zod';

export const educationSchema = z
  .object({
    university: z.string().min(1, 'University is required'),
    field: z.string().min(1, 'Field of study is required'),
    description: z.string().optional(),
    startDate: z.string().regex(/^\d{4}$/, 'Start year must be four digits'),
    endDate: z
      .string()
      .regex(/^\d{4}$/, 'End year must be four digits')
      .optional()
      .or(z.literal('')),
    toPresent: z.boolean(),
  })
  .refine(
    (data) => {
      if (!data.toPresent && !data.endDate) {
        return false;
      }
      if (
        data.endDate &&
        data.startDate &&
        parseInt(data.endDate) < parseInt(data.startDate)
      ) {
        return false;
      }
      return true;
    },
    {
      message:
        "End year must be after start year, or select 'Currently studying'",
      path: ['endDate'],
    },
  );

export type EducationInput = z.infer<typeof educationSchema>;
