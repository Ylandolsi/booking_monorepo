import {
  Button,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  Input,
  MultiSelect,
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
  Textarea,
} from '@/components/ui';
import { LANGUAGES_OPTIONS } from '../../constants';
import { UserDataWrapper } from '@/features/auth';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { basicInfoSchema, type BasicInfoFormValues } from '@/features/profile';

export function BasicInfoForm() {
  const FIELDS = [
    {
      key: 'firstName',
      label: 'First Name',
      type: 'text',
      placeholder: 'Enter your first name',
    },
    {
      key: 'lastName',
      label: 'Second Name',
      type: 'text',
      placeholder: 'Enter your second name',
    },
    {
      key: 'gender',
      label: 'Gender',
      type: 'select',
      options: [
        { value: 'Male', label: 'Male' },
        { value: 'Female', label: 'Female' },
      ],
      placeholder: 'Select a gender',
    },
    {
      key: 'languages',
      label: 'Languages',
      type: 'multiselect',
      options: LANGUAGES_OPTIONS,
      placeholder: 'Select up to 4 languages',
      maxCount: 4,
      animation: 2,
    },
    {
      key: 'bio',
      label: 'Bio',
      type: 'textarea',
      placeholder: 'Write a short bio',
    },
  ];
  return (
    <UserDataWrapper>
      {(userData) => {
        const form = useForm<BasicInfoFormValues>({
          resolver: zodResolver(basicInfoSchema),
          defaultValues: {
            firstName: userData?.firstName ?? '',
            lastName: userData?.lastName ?? '',
            gender: userData?.gender ?? '',
            languages: userData?.languages?.map((ld) => ld.name) ?? [],
            bio: userData?.bio ?? '',
          },
        });

        const onSubmit = async (data: BasicInfoFormValues) => {
          console.log('Submitted basic info:', data);
        };

        return (
          <Form {...form}>
            <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
              {FIELDS.map((fieldConfig) => (
                <FormField
                  key={fieldConfig.key}
                  control={form.control}
                  name={fieldConfig.key as keyof BasicInfoFormValues}
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-sm font-medium">
                        {fieldConfig.label}
                      </FormLabel>
                      <FormControl>
                        {fieldConfig.type === 'text' && (
                          <Input
                            {...field}
                            type="text"
                            placeholder={fieldConfig.placeholder}
                          />
                        )}
                        {fieldConfig.type === 'select' && (
                          <Select
                            onValueChange={field.onChange}
                            value={field.value}
                            defaultValue={field.value}
                          >
                            <SelectTrigger className="w-full">
                              <SelectValue
                                placeholder={fieldConfig.placeholder}
                              />
                            </SelectTrigger>
                            <SelectContent>
                              {fieldConfig.options?.map((option) => (
                                <SelectItem
                                  key={option.value}
                                  value={option.value}
                                >
                                  {option.label}
                                </SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                        )}
                        {fieldConfig.type === 'multiselect' && (
                          <MultiSelect
                            options={fieldConfig.options!}
                            onValueChange={field.onChange}
                            defaultValue={field.value}
                            placeholder={fieldConfig.placeholder}
                            animation={fieldConfig.animation}
                            maxCount={fieldConfig.maxCount}
                          />
                        )}
                        {fieldConfig.type === 'textarea' && (
                          <Textarea
                            {...field}
                            placeholder={fieldConfig.placeholder}
                          />
                        )}
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              ))}
              <Button
                className="w-full"
                type="submit"
                disabled={!form.formState.isDirty || form.formState.isLoading}
              >
                {form.formState.isLoading ? 'Saving ...' : 'Save informations'}
              </Button>
            </form>
          </Form>
        );
      }}
    </UserDataWrapper>
  );
}
