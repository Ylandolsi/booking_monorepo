import { FormControl, FormDescription, FormField, FormItem, FormLabel, FormMessage, Input } from '@/components/ui';
import type { ControllerRenderProps, FieldValues, Path, UseFormReturn } from 'react-hook-form';

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
{
  /* <FormFieldWrapper form={form} name="title" label="Product Title" required>
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
</FormFieldWrapper>; */
}
