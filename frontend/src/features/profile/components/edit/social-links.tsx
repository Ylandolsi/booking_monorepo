import {
  Button,
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
  Input,
} from '@/components/ui';
import { useUser } from '@/features/auth';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { socialLinksSchema, type SocialLinksFormValues } from '../../schemas';
import type { SocialLinksType } from '@/features/profile';
import { QueryWrapper } from '@/components';

const SOCIAL_FIELDS = [
  { key: 'linkedIn', label: 'LinkedIn', placeholder: 'Enter LinkedIn URL' },
  { key: 'portfolio', label: 'Website', placeholder: 'Enter Website URL' },
  { key: 'github', label: 'GitHub', placeholder: 'Enter GitHub URL' },
  { key: 'instagram', label: 'Instagram', placeholder: 'Enter Instagram URL' },
  { key: 'youtube', label: 'Youtube', placeholder: 'Enter Youtube URL' },
  { key: 'facebook', label: 'Facebook', placeholder: 'Enter Facebook URL' },
];

export function SocialLinksForm() {
  const userQuery = useUser();
  return (
    <QueryWrapper query={userQuery}>
      {(userData) => {
        const socialLinks: SocialLinksType = userData?.socialLinks ?? {};

        const form = useForm<SocialLinksFormValues>({
          resolver: zodResolver(socialLinksSchema),
          defaultValues: {
            linkedIn: socialLinks.linkedIn ?? '',
            portfolio: socialLinks.portfolio ?? '',
            github: socialLinks.github ?? '',
            instagram: socialLinks.instagram ?? '',
            youtube: socialLinks.youtube ?? '',
            facebook: socialLinks.facebook ?? '',
          },
        });

        const onSubmit = async (data: SocialLinksFormValues) => {
          console.log('Submitted social links:', data);
        };

        const sortedFields = [...SOCIAL_FIELDS].sort((a, b) => {
          const aDefined = !!socialLinks[a.key as keyof SocialLinksType];
          const bDefined = !!socialLinks[b.key as keyof SocialLinksType];
          return aDefined === bDefined ? 0 : aDefined ? -1 : 1;
        });

        return (
          <Form {...form}>
            <form className="space-y-4" onSubmit={form.handleSubmit(onSubmit)}>
              {sortedFields.map(({ key, label, placeholder }) => (
                <FormField
                  key={key}
                  control={form.control}
                  name={key as keyof SocialLinksFormValues}
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="text-sm font-medium">
                        {label}
                      </FormLabel>
                      <FormControl>
                        <Input
                          {...field}
                          type="url"
                          className="text-foreground/70"
                          placeholder={placeholder}
                          defaultValue={
                            socialLinks[key as keyof SocialLinksType] ?? ''
                          }
                        />
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              ))}

              <Button
                className="w-full"
                type="submit"
                // dirty = true => at least one field is changed from default

                disabled={!form.formState.isDirty || form.formState.isLoading}
              >
                {form.formState.isLoading ? 'Saving ...' : 'Save informations'}
              </Button>
            </form>
          </Form>
        );
      }}
    </QueryWrapper>
  );
}
