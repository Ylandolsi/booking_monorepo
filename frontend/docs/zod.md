## Zod Schemas

### Inferring Types from Schemas

```ts
export type CreateProductInput = z.infer<typeof createProductSchema>;
```

### Nested Schemas

```ts
export const availabilityRangeTypeSchema = z.object({
  id: z.union([z.number(), z.undefined()]),
  startTime: z.string(),
  endTime: z.string(),
});

export const dailyScheduleSchema = z.object({
  dayOfWeek: z.number(),
  isActive: z.boolean(),
  availabilityRanges: z.array(availabilityRangeTypeSchema),
});

export const createSessionProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.Session),
  duration: z.number().positive('Duration must be positive'),
  bufferTime: z.number().min(0, 'Buffer time cannot be negative'),
  meetingInstructions: z.string().optional(),
  timeZoneId: z.string().min(1, 'Time zone is required'),
  dailySchedule: z.array(dailyScheduleSchema).min(1, 'At least one day schedule is required'),
});
```

### Discriminated Unions for Multiple Schemas

```ts
export const createProductBaseSchema = z.object({
  title: z.string().min(3, 'Product title is required'),
  subtitle: z.string().optional(),
  description: z.string().optional(),
  price: z.number().min(0, 'Price cannot be negative'),
  clickToPay: z.string().min(1, 'Button text is required'),
  thumbnail: z.instanceof(File).optional(),
});

export const createSessionProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.Session), // literal is for distinguishing
  duration: z.number().positive('Duration must be positive'),
});

export const createDigitalProductSchema = createProductBaseSchema.extend({
  productType: z.literal(ProductType.DigitalDownload), // literal is for distinguishing
  files: z.array(z.instanceof(File)).min(1, 'At least one file is required'),
});

// Distinguish them by the product type
export const createProductSchema = z.discriminatedUnion('productType', [createSessionProductSchema, createDigitalProductSchema]);
export type CreateProductInput = z.infer<typeof createProductSchema>;
```

### Conditional Validation and Transforms

````ts
// Conditional validation based on other fields
const paymentSchema = z.object({
  paymentMethod: z.enum(['credit_card', 'paypal', 'bank_transfer']),
  creditCardNumber: z.string().optional(),
  paypalEmail: z.string().email().optional(),
  bankAccount: z.string().optional(),
}).refine((data) => {
  if (data.paymentMethod === 'credit_card') {
    return data.creditCardNumber && data.creditCardNumber.length >= 16;
  }
  if (data.paymentMethod === 'paypal') {
    return data.paypalEmail && data.paypalEmail.length > 0;
  }
  if (data.paymentMethod === 'bank_transfer') {
    return data.bankAccount && data.bankAccount.length > 0;
  }
  return true;
}, {
  message: "Payment details are required for the selected method",
  path: ["paymentMethod"],
});
```

### zod alternative
```ts
export interface BookingHookState {
  selectedDate: Date | undefined;
  selectedSlot: SessionSlotType | null;
  step: BookingStep;
  notes: string;
  title: string;
}

export function useBooking({ mentorSlug, iamTheMentor }: { mentorSlug?: string; iamTheMentor: boolean }) {
  const [state, setState] = useState<BookingHookState>({
    selectedDate: new Date(),
    selectedSlot: null,
    step: 'select',
    notes: '',
    title: '',
  });

  // Actions ...
const setSelectedDate = useCallback((date: Date | undefined) => {
  setState((prev: BookingHookState) => ({
    ...prev,
    selectedDate: date,
    selectedSlot: null, // Reset slot when date changes
  }));
}, []);

const setSelectedSlot = useCallback((slot: SessionSlotType | null) => {
  setState((prev: BookingHookState) => ({
    ...prev,
    selectedSlot: slot,
  }));
}, []);
```
````
