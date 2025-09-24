import z from 'zod';

export const createProductBaseSchema = z.object({
  title: z.string().min(3, 'Product title is required'),
  subtitle: z.string().min(3, 'Subtitle must be at least 3 characters'),
  description: z.string().optional(),
  price: z.number().min(0, 'Price cannot be negative'),
  clickToPay: z.string().min(1, 'Button text is required'),
  thumbnail: z.instanceof(File).optional(),
});
