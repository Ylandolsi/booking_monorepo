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
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  basicInfoSchema,
  useAllLanguages,
  useUpdateBasicInfo,
  useUpdateLanguages,
  type BasicInfoFormValues,
  type BasicInfoType,
} from '@/features/profile';
import { useUser } from '@/features/auth';
import { QueryWrapper } from '@/components';
import { useMemo } from 'react';

export function BasicInfoForm() {
  const userQuery = useUser();

  return (
    <QueryWrapper query={userQuery}>
      {(userData) => {
        const allLanguageQuery = useAllLanguages();

        return (
          <QueryWrapper query={allLanguageQuery}>
            {(languages) => {
              const updateLanguageMutate = useUpdateLanguages();
              const updateBasicInfoMutate = useUpdateBasicInfo();

              const languageOptions = useMemo(() => {
                return (languages ?? []).map((language) => ({
                  value: language.id.toString(),
                  label: language.name,
                }));
              }, [languages]);

              const form = useForm<BasicInfoFormValues>({
                resolver: zodResolver(basicInfoSchema),
                defaultValues: {
                  firstName: userData?.firstName ?? '',
                  lastName: userData?.lastName ?? '',
                  gender: userData?.gender ?? '',
                  languages:
                    userData?.languages?.map((ld) => ld.id.toString()) ?? [],
                  bio: userData?.bio ?? '',
                },
              });

              const watchLangs = form.watch('languages');

              const onSubmit = async (data: BasicInfoFormValues) => {
                const { languages, ...basicInfoData } = data;

                console.log('Submitted basic info:', data);
                console.log(watchLangs);
                await updateLanguageMutate.mutateAsync({
                  languages: languages.map((lg) => Number(lg)),
                });
                await updateBasicInfoMutate.mutateAsync({
                  data: basicInfoData as BasicInfoType,
                });
              };

              return (
                <Form {...form}>
                  <form
                    className="space-y-4"
                    onSubmit={form.handleSubmit(onSubmit)}
                  >
                    <FormField
                      control={form.control}
                      name="firstName"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-sm font-medium">
                            First Name
                          </FormLabel>
                          <FormControl>
                            <Input
                              {...field}
                              type="text"
                              placeholder="Enter your first name"
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    <FormField
                      control={form.control}
                      name="lastName"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-sm font-medium">
                            Last Name
                          </FormLabel>
                          <FormControl>
                            <Input
                              {...field}
                              type="text"
                              placeholder="Enter your last name"
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    <FormField
                      control={form.control}
                      name="gender"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-sm font-medium">
                            Gender
                          </FormLabel>
                          <FormControl>
                            <Select
                              onValueChange={field.onChange}
                              value={field.value}
                            >
                              <SelectTrigger className="w-full">
                                <SelectValue placeholder="Select a gender" />
                              </SelectTrigger>
                              <SelectContent>
                                <SelectItem value="Male">Male</SelectItem>
                                <SelectItem value="Female">Female</SelectItem>
                              </SelectContent>
                            </Select>
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    <FormField
                      control={form.control}
                      name="languages"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-sm font-medium">
                            Languages
                          </FormLabel>
                          <FormControl>
                            <MultiSelect
                              options={languageOptions}
                              onValueChange={field.onChange}
                              defaultValue={field.value}
                              placeholder="Select up to 4 languages"
                              animation={2}
                              maxCount={4}
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    <FormField
                      control={form.control}
                      name="bio"
                      render={({ field }) => (
                        <FormItem>
                          <FormLabel className="text-sm font-medium">
                            Bio
                          </FormLabel>
                          <FormControl>
                            <Textarea
                              {...field}
                              placeholder="Write a short bio"
                            />
                          </FormControl>
                          <FormMessage />
                        </FormItem>
                      )}
                    />

                    <Button
                      className="w-full"
                      type="submit"
                      disabled={
                        !form.formState.isDirty || form.formState.isSubmitting
                      }
                    >
                      {form.formState.isSubmitting
                        ? 'Saving...'
                        : 'Save Information'}
                    </Button>
                  </form>
                </Form>
              );
            }}
          </QueryWrapper>
        );
      }}
    </QueryWrapper>
  );
}
