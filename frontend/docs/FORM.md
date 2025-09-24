### React Hook Form && Zod

This guide covers integrating React Hook Form with Zod for form validation and schema management.

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

## React Hook Form Usage

### Basic Setup

```ts
import { useForm, UseFormReturn } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';

const form = useForm<CreateProductInput>({
  resolver: zodResolver(createProductSchema),
  defaultValues: {
    title: '',
    subtitle: '',
    description: '',
    price: 0,
    clickToPay: 'Buy Now',
    thumbnail: undefined,
    productType: ProductType.Session,
    isPublished: false,
    tags: [],
  },
  mode: 'onBlur', // Validate on blur for better UX
  criteriaMode: 'all', // Show all validation errors
  shouldFocusError: true, // Auto-focus on error fields
});

const onSubmit = async (data: CreateProductInput) => {
  try {
    setIsSubmitting(true);
    await createProductMutation.mutateAsync(data);
    toast.success('Product created successfully!');
    navigate({ to: ROUTE_PATHS.APP.PRODUCTS });
  } catch (error) {
    console.error('Failed to create product:', error);
    toast.error('Failed to create product. Please try again.');
  } finally {
    setIsSubmitting(false);
  }
};

// Form component
<Form {...form}>
  <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
    {/* Form fields */}
    <Button type="submit" disabled={isSubmitting || !form.formState.isValid}>
      {isSubmitting ? 'Creating...' : 'Create Product'}
    </Button>
  </form>
</Form>
```

###Reset
```ts
// with new values
// Reset form with new values
const resetForm = (newValues?: Partial<CreateProductInput>) => {
  form.reset(newValues);
};

// with default
  form.reset();
```

### Watching Values

```ts
const watchedValues = form.watch(); // watch all
const watchedSepcificValues = form.watch(['title', 'price']); // watch specific
const productType = form.watch('productType');
```

### Setting and Clearing Errors

```ts
form.setError('slug', { type: 'manual', message: 'Slug is already taken' });
form.clearErrors('slug');

const {
  formState: { errors, isValid, isSubmitting, isDirty, touchedFields, dirtyFields },
} = form;
```

### Form Field Example

```ts
<FormField
        control={form.control}
        name="title"
        render={({ field }) => (
                <FormItem>
                        <FormLabel className="text-foreground flex items-center gap-2">
                                <User className="h-4 w-4" />
                                Store Name *
                        </FormLabel>
                        <FormControl>
                                <Input
                                        placeholder="Your Amazing Store"
                                        className="border-border text-foreground py-3 text-lg"
                                        {...field}
                                        onChange={(e) => {
                                                field.onChange(e);
                                                if (!watchedValues.slug) generateSlug(e.target.value);
                                        }}
                                />
                        </FormControl>
                        <FormMessage />
                </FormItem>
        )}
/>
```

### Custom Form Components

For complex form sections like schedules, use a custom hook to manage state and integrate with react-hook-form. Example based on `useFormSchedule` hook:

```ts
// In a component like add-product.tsx
<FormScheduleComponent form={form} />

// FormScheduleComponent implementation
export function FormScheduleComponent({ form }: { form: UseFormReturn<ProductFormData> }) {
        const { schedule, selectedCopySource, actions, getScheduleSummary } = useFormSchedule(form);

        // Render schedule UI here, using actions to update form state

        // Form validation error display
        <FormField
                control={form.control}
                name="dailySchedule"
                render={() => (
                        <FormItem>
                                <FormMessage />
                        </FormItem>
                )}
        />
}

// useFormSchedule hook (excerpt)
export function useFormSchedule(form: UseFormReturn<CreateProductInput>): UseFormScheduleReturn {
        const formSchedule = form.watch('dailySchedule') as DailySchedule[] | undefined;

        useEffect(() => {
                if (!formSchedule || formSchedule.length === 0) {
                        form.setValue('dailySchedule', createDefaultSchedule());
                }
        }, [form, formSchedule]);

        const schedule = formSchedule || createDefaultSchedule();

        const updateSchedule = (day: DayOfWeek, updater: (ds: DailySchedule) => DailySchedule) => {
                const currentSchedule = (form.getValues('dailySchedule') as DailySchedule[]) || createDefaultSchedule();
                const newSchedule = currentSchedule.map((ds) => (ds.dayOfWeek === mapDayToNumber(day) ? updater(ds) : ds));
                form.setValue('dailySchedule', newSchedule, { shouldValidate: true });
        };

        // ... other actions

        return {
                schedule,
                selectedCopySource,
                actions: { /* actions */ },
                getScheduleSummary,
        };
}
```
````

---

### Field Wrapper

```ts
interface FormFieldWrapperProps<T extends FieldValues> {
  form: UseFormReturn<T>;
  name: Path<T>;
  label: string;
  description?: string;
  required?: boolean;
  children: (field: ControllerRenderProps<T, Path<T>>) => React.ReactNode;
}

export function FormFieldWrapper<T extends FieldValues>({
  form,
  name,
  label,
  description,
  required = false,
  children,
}: FormFieldWrapperProps<T>) {
  return (
    <FormField
      control={form.control}
      name={name}
      render={({ field }) => (
        <FormItem>
          <FormLabel className="text-foreground flex items-center gap-2">
            {label}
            {required && <span className="text-red-500">*</span>}
          </FormLabel>
          <FormControl>
            {children(field)}
          </FormControl>
          {description && (
            <FormDescription>{description}</FormDescription>
          )}
          <FormMessage />
        </FormItem>
      )}
    />
  );
}

// Usage
<FormFieldWrapper
  form={form}
  name="title"
  label="Product Title"
  required
>
  {(field) => (
    <Input
      placeholder="Enter product title"
      {...field}
      onChange={(e) => {
        field.onChange(e);
        generateSlug(e.target.value);
      }}
    />
  )}
</FormFieldWrapper>
```

### External validation ( like slug ) that requires api calls

```ts

// Usage in form field
function EmailField({ form }: { form: UseFormReturn<any> }) {
  const { validateAsync, validationStatus } = useAsyncValidation();

  return (
    <FormField
      control={form.control}
      name="email"
      render={({ field }) => (
        <FormItem>
          <FormLabel>Email</FormLabel>
          <FormControl>
            <div className="relative">
              <Input
                {...field}
                onChange={(e) => {
                  field.onChange(e);
                  validateAsync('email', e.target.value);
                }}
              />
              {validationStatus.email === 'validating' && (
                <Loader className="absolute right-2 top-1/2 -translate-y-1/2 h-4 w-4 animate-spin" />
              )}
              {validationStatus.email === 'valid' && (
                <Check className="absolute right-2 top-1/2 -translate-y-1/2 h-4 w-4 text-green-500" />
              )}
            </div>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
```
