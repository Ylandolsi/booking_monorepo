import * as React from 'react';
import { type FieldError } from 'react-hook-form';

import { Error } from './error';
import { Label } from '@/components/ui/label';
import { Textarea } from '../textarea';
import { Input } from '../input';

type FieldWrapperProps = {
  label?: string;
  className?: string;
  children: React.ReactNode;
  error?: FieldError | undefined;
};

export const FieldWrapper = (props: FieldWrapperProps) => {
  const { label, error, children } = props;
  return (
    <div className="space-y-2">
      <Label className="block text-sm font-medium">{label}</Label>
      {children}
      <Error errorMessage={error?.message} />
    </div>
  );
};

{
  /* <FieldWrapper 
  label="Email Address" 
  error={errors.email}
>
  <input type="email" name="email" />
</FieldWrapper> */
}

// function FormField({ label, inputType, ...props }: any) {
//   return (
//     <div className="space-y-2">
//       <Label className="block text-sm font-medium">{label}</Label>
//       {inputType === 'textarea' ? (
//         <Textarea {...props} />
//       ) : (
//         <Input type={inputType} {...props} />
//       )}
//       <Error errorMessage={error?.message} />
//     </div>
//   );
// }
