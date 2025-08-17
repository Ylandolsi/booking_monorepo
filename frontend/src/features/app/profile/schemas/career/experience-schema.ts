import z from 'zod';

export const experienceSchema = z
  .object({
    title: z.string().min(1, 'Title is required'),
    description: z.string().optional(),
    startDate: z.string().regex(/^\d{4}$/, 'Start year must be four digits'),
    endDate: z
      .string()
      .regex(/^\d{4}$/, 'End year must be four digits')
      .optional()
      .or(z.literal('')),
    toPresent: z.boolean(),
    company: z.string().min(1, 'Company is required'),
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

export type ExperienceInput = z.infer<typeof experienceSchema>;

export function ExperienceForm() {}
