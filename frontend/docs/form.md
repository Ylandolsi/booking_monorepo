# React Hook Form Patterns and Documentation

This document outlines common patterns and best practices for using React Hook Form with Zod validation in the frontend application.

## Table of Contents

- [Basic Setup](#basic-setup)
- [Form Fields](#form-fields)
  - [Select Fields](#select-fields)
  - [Form Field Examples](#form-field-examples)
- [Form State Management](#form-state-management)
  - [Watching Values](#watching-values)
  - [Setting and Clearing Errors](#setting-and-clearing-errors)
  - [Resetting Forms](#resetting-forms)
- [Advanced Patterns](#advanced-patterns)
  - [Custom Form Components](#custom-form-components)
  - [Field Wrapper](#field-wrapper)
  - [External Validation](#external-validation)

## Basic Setup

### Form Initialization with Zod Resolver

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
```

### Form Submission Handler

```ts
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
```

### Form Component Structure

```tsx
<Form {...form}>
  <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
    {/* Form fields */}
    <Button type="submit" disabled={isSubmitting || !form.formState.isValid}>
      {isSubmitting ? 'Creating...' : 'Create Product'}
    </Button>
  </form>
</Form>
```

## Form Fields

### Select Fields

```tsx
<FormField
  control={form.control}
  name="bufferTime" // name of field
  render={(
    { field }, // properties of filed with name "bufferTime"
  ) => (
    <FormItem>
      <FormLabel className="text-foreground">Buffer Time (minutes)</FormLabel>
      <FormControl>
        <Select onValueChange={(value) => field.onChange(Number(value))} value={field.value?.toString()}>
          <SelectTrigger className="w-full">
            <SelectValue placeholder="Select duration" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="15">15</SelectItem>
            <SelectItem value="30">30</SelectItem>
            <SelectItem value="45">45</SelectItem>
            {/* only 30 minutes available for now */}
          </SelectContent>
        </Select>
      </FormControl>
      <FormMessage />
    </FormItem>
  )}
/>
```

### Form Field Examples

```tsx
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

## Form State Management

### Watching Values

```ts
const watchedValues = form.watch(); // watch all
const watchedSpecificValues = form.watch(['title', 'price']); // watch specific
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

### Resetting Forms

```ts
// Reset form with new values
const resetForm = (newValues?: Partial<CreateProductInput>) => {
  form.reset(newValues);
};

// Reset to default values
form.reset();
```

## Advanced Patterns

### Custom Form Components

For complex form sections like schedules, use a custom hook to manage state and integrate with react-hook-form. Example based on `useFormSchedule` hook:

#### Usage in Component

```tsx
// In a component like add-product.tsx
<FormScheduleComponent form={form} />
```

#### Component Implementation

```tsx
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
  />;
}
```

#### Custom Hook Implementation

```tsx
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
    actions: {
      /* actions */
    },
    getScheduleSummary,
  };
}
```

### Field Wrapper

```tsx
interface FormFieldWrapperProps<T extends FieldValues> {
  form: UseFormReturn<T>;
  name: Path<T>;
  label: string;
  description?: string;
  required?: boolean;
  children: (field: ControllerRenderProps<T, Path<T>>) => React.ReactNode;
}

export function FormFieldWrapper<T extends FieldValues>({ form, name, label, description, required = false, children }: FormFieldWrapperProps<T>) {
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
          <FormControl>{children(field)}</FormControl>
          {description && <FormDescription>{description}</FormDescription>}
          <FormMessage />
        </FormItem>
      )}
    />
  );
}

// Usage
<FormFieldWrapper form={form} name="title" label="Product Title" required>
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
</FormFieldWrapper>;
```

### External Validation

For validation that requires API calls (like checking if a slug is available):

```tsx
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
              {validationStatus.email === 'validating' && <Loader className="absolute right-2 top-1/2 h-4 w-4 -translate-y-1/2 animate-spin" />}
              {validationStatus.email === 'valid' && <Check className="absolute right-2 top-1/2 h-4 w-4 -translate-y-1/2 text-green-500" />}
            </div>
          </FormControl>
          <FormMessage />
        </FormItem>
      )}
    />
  );
}
```

```

```
