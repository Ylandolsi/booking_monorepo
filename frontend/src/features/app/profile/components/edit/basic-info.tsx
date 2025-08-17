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
  Alert,
  AlertDescription,
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
  type LanguageType,
} from '@/features/app/profile';
import { useUser } from '@/features/auth';
import { Spinner } from '@/components/ui';
import { useEffect, useMemo } from 'react';
import { AlertTriangle } from 'lucide-react';

const GENDER_OPTIONS = [
  { value: 'Male', label: 'Male' },
  { value: 'Female', label: 'Female' },
];

const MAX_LANGUAGES = 4;

interface LanguageOption {
  value: string;
  label: string;
}

export function BasicInfoForm() {
  const userQuery = useUser();
  const allLanguageQuery = useAllLanguages();
  const updateLanguageMutation = useUpdateLanguages();
  const updateBasicInfoMutation = useUpdateBasicInfo();

  const languageOptions: LanguageOption[] = useMemo(() => {
    if (!allLanguageQuery.data) return [];
    return allLanguageQuery.data.map((language: LanguageType) => ({
      value: language.id.toString(),
      label: language.name,
    }));
  }, [allLanguageQuery.data]);

  const form = useForm<BasicInfoFormValues>({
    resolver: zodResolver(basicInfoSchema),
    defaultValues: {
      firstName: userQuery.data?.firstName ?? '',
      lastName: userQuery.data?.lastName ?? '',
      gender: userQuery.data?.gender ?? '',
      languages:
        userQuery.data?.languages?.map((lang) => lang.id.toString()) ?? [],
      bio: userQuery.data?.bio ?? '',
    },
  });

  useEffect(() => {
    if (userQuery.data) {
      form.reset({
        firstName: userQuery.data.firstName ?? '',
        lastName: userQuery.data.lastName ?? '',
        gender: userQuery.data.gender ?? '',
        languages:
          userQuery.data.languages?.map((lang) => lang.id.toString()) ?? [],
        bio: userQuery.data.bio ?? '',
      });
    }
  }, [userQuery.data, form]);

  const handleSubmit = async (data: BasicInfoFormValues) => {
    try {
      const { languages, ...basicInfoData } = data;
      const userLanguages =
        userQuery.data?.languages?.map((lang) => lang.id.toString()) ?? [];

      const sortedLanguages = [...languages].sort();
      const sortedUserLanguages = [...userLanguages].sort();

      if (
        JSON.stringify(sortedLanguages) !== JSON.stringify(sortedUserLanguages)
      ) {
        await updateLanguageMutation.mutateAsync({
          languages: languages.map(Number),
        });
      }

      await updateBasicInfoMutation.mutateAsync({
        data: basicInfoData as BasicInfoType,
      });
    } catch (error) {
      console.error('Failed to update profile:', error);
    }
  };

  if (userQuery.isLoading || allLanguageQuery.isLoading) {
    return (
      <div className="flex items-center justify-center p-8">
        <Spinner />
        <span className="ml-2 text-sm text-gray-600">
          Loading profile data...
        </span>
      </div>
    );
  }

  if (userQuery.error || allLanguageQuery.error) {
    return (
      <Alert variant="destructive">
        <AlertTriangle className="h-4 w-4" />
        <AlertDescription>
          Failed to load profile data. Please refresh the page and try again.
        </AlertDescription>
      </Alert>
    );
  }

  const isSubmitting =
    updateLanguageMutation.isPending || updateBasicInfoMutation.isPending;
  const hasChanges = form.formState.isDirty;

  return (
    <div className="space-y-4">
      <Form {...form}>
        <form className="space-y-4" onSubmit={form.handleSubmit(handleSubmit)}>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <FormField
              control={form.control}
              name="firstName"
              render={({ field }) => (
                <FormItem>
                  <FormLabel className="text-sm font-medium">
                    First Name *
                  </FormLabel>
                  <FormControl>
                    <Input
                      {...field}
                      placeholder="Enter your first name"
                      disabled={isSubmitting}
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
                    Last Name *
                  </FormLabel>
                  <FormControl>
                    <Input
                      {...field}
                      placeholder="Enter your last name"
                      disabled={isSubmitting}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

          <FormField
            control={form.control}
            name="gender"
            render={({ field }) => (
              <FormItem>
                <FormLabel className="text-sm font-medium">Gender</FormLabel>
                <FormControl>
                  <Select
                    onValueChange={field.onChange}
                    value={field.value}
                    disabled={isSubmitting}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select your gender" />
                    </SelectTrigger>
                    <SelectContent>
                      {GENDER_OPTIONS.map((option) => (
                        <SelectItem key={option.value} value={option.value}>
                          {option.label}
                        </SelectItem>
                      ))}
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
                  Languages (up to {MAX_LANGUAGES})
                </FormLabel>
                <FormControl>
                  <MultiSelect
                    options={languageOptions}
                    onValueChange={field.onChange}
                    defaultValue={field.value}
                    placeholder={`Select up to ${MAX_LANGUAGES} languages`}
                    maxCount={MAX_LANGUAGES}
                    disabled={isSubmitting}
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
                <FormLabel className="text-sm font-medium">Bio</FormLabel>
                <FormControl>
                  <Textarea
                    {...field}
                    placeholder="Tell us about yourself..."
                    className="min-h-[100px] resize-none"
                    disabled={isSubmitting}
                  />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <div className="flex gap-2 pt-4">
            <Button
              type="button"
              variant="outline"
              onClick={() => form.reset()}
              disabled={isSubmitting || !hasChanges}
              className="flex-1"
            >
              Reset
            </Button>
            <Button
              type="submit"
              disabled={isSubmitting || !hasChanges}
              className="flex-1"
            >
              {isSubmitting ? 'Saving...' : 'Save Changes'}
            </Button>
          </div>
        </form>
      </Form>
    </div>
  );
}
