import { createProductBaseSchema } from '@/api/stores/produtcs/base-schema';
import { ProductType } from '@/api/stores/produtcs/products-type';
import z from 'zod';

export const createDigitalProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.DigitalDownload),
  files: z.array(z.instanceof(File)).min(1, 'At least one file is required'),
  deliveryUrl: z.string().url('Invalid URL').optional(),
  previewImage: z.instanceof(File).optional(),
});
