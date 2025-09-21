import { Button, Form, FormControl, FormField, FormItem, FormLabel, FormMessage, Input } from '@/components/ui';
import { QueryStateWrapper } from '@/components/wrappers';
import { useUser } from '@/api/auth';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { socialLinksSchema, type SocialLinksFormValues } from '../../schemas';
import { useUpdateSocialLinks, type SocialLinksType } from '@/features/app/profile';
import { useMemo, useEffect } from 'react';
import { ExternalLink } from 'lucide-react';

interface SocialField {
  key: keyof SocialLinksFormValues;
  label: string;
  placeholder: string;
  icon?: string;
}

const SOCIAL_FIELDS: SocialField[] = [
  {
    key: 'linkedIn',
    label: 'LinkedIn',
    placeholder: 'https://linkedin.com/in/username',
    icon: '💼',
  },
  {
    key: 'portfolio',
    label: 'Portfolio Website',
    placeholder: 'https://yourwebsite.com',
    icon: '🌐',
  },
  {
    key: 'github',
    label: 'GitHub',
    placeholder: 'https://github.com/username',
    icon: '🐙',
  },
  {
    key: 'instagram',
    label: 'Instagram',
    placeholder: 'https://instagram.com/username',
    icon: '📸',
  },
  {
    key: 'youtube',
    label: 'YouTube',
    placeholder: 'https://youtube.com/@username',
    icon: '📺',
  },
  {
    key: 'facebook',
    label: 'Facebook',
    placeholder: 'https://facebook.com/username',
    icon: '📘',
  },
];

export function SocialLinksForm() {
  const userQuery = useUser();

  return (
    <QueryStateWrapper query={userQuery} loadingMessage="Loading social links..." loadingType="spinner" containerClassName="p-0" minHeight="200px">
      {(userData) => <SocialLinksFormContent userData={userData} />}
    </QueryStateWrapper>
  );
}

function SocialLinksFormContent({ userData }: { userData: any }) {
  const updateSocialLinksMutation = useUpdateSocialLinks();

  const socialLinks: SocialLinksType = useMemo(() => userData?.socialLinks ?? {}, [userData?.socialLinks]);

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

  // Reset form when user data changes
  useEffect(() => {
    if (userData?.socialLinks) {
      const links = userData.socialLinks;
      form.reset({
        linkedIn: links.linkedIn ?? '',
        portfolio: links.portfolio ?? '',
        github: links.github ?? '',
        instagram: links.instagram ?? '',
        youtube: links.youtube ?? '',
        facebook: links.facebook ?? '',
      });
    }
  }, [userData?.socialLinks, form]);

  // Sort fields to show filled ones first
  const sortedFields = useMemo(() => {
    return [...SOCIAL_FIELDS].sort((a, b) => {
      const aHasValue = !!socialLinks[a.key];
      const bHasValue = !!socialLinks[b.key];

      // Show filled fields first
      if (aHasValue && !bHasValue) return -1;
      if (!aHasValue && bHasValue) return 1;
      return 0;
    });
  }, [socialLinks]);

  const handleSubmit = async (data: SocialLinksFormValues) => {
    try {
      // Filter out empty URLs to avoid storing empty strings
      const filteredData = Object.entries(data).reduce((acc, [key, value]) => {
        if (value && value.trim()) {
          acc[key as keyof SocialLinksType] = value.trim();
        }
        return acc;
      }, {} as SocialLinksType);

      await updateSocialLinksMutation.mutateAsync({
        data: filteredData,
      });
    } catch (error) {
      console.error('Failed to update social links:', error);
    }
  };

  const isValidUrl = (url: string): boolean => {
    if (!url) return true;
    try {
      new URL(url);
      return true;
    } catch {
      return false;
    }
  };

  const isSubmitting = updateSocialLinksMutation.isPending;
  const hasChanges = form.formState.isDirty;

  return (
    <div className="space-y-4">
      <Form {...form}>
        <form className="space-y-4" onSubmit={form.handleSubmit(handleSubmit)}>
          <div className="space-y-4">
            {sortedFields.map(({ key, label, placeholder, icon }) => {
              const fieldValue = form.watch(key);
              const hasValue = !!socialLinks[key];

              return (
                <FormField
                  key={key}
                  control={form.control}
                  name={key}
                  render={({ field }) => (
                    <FormItem>
                      <FormLabel className="flex items-center gap-2 text-sm font-medium">
                        {icon && <span>{icon}</span>}
                        {label}
                        {hasValue && <span className="rounded bg-green-50 px-2 py-1 text-xs text-green-600">Connected</span>}
                      </FormLabel>
                      <FormControl>
                        <div className="relative">
                          <Input
                            {...field}
                            type="url"
                            placeholder={placeholder}
                            disabled={isSubmitting}
                            className={`pr-10 ${fieldValue && !isValidUrl(fieldValue) ? 'border-red-300 focus:border-red-500' : ''}`}
                          />
                          {fieldValue && isValidUrl(fieldValue) && (
                            <ExternalLink
                              onClick={(e) => {
                                e.preventDefault();
                                if (fieldValue && isValidUrl(fieldValue)) {
                                  window.open(fieldValue, '_blank', 'noopener,noreferrer');
                                }
                              }}
                              className="absolute top-1/2 right-3 h-4 w-4 -translate-y-1/2 transform cursor-pointer text-gray-400"
                            />
                          )}
                        </div>
                      </FormControl>
                      <FormMessage />
                    </FormItem>
                  )}
                />
              );
            })}
          </div>

          <div className="flex gap-2 pt-4">
            <Button type="button" variant="outline" onClick={() => form.reset()} disabled={isSubmitting || !hasChanges} className="flex-1">
              Reset
            </Button>
            <Button type="submit" disabled={isSubmitting || !hasChanges} className="flex-1">
              {isSubmitting ? 'Saving...' : 'Save Social Links'}
            </Button>
          </div>
        </form>
      </Form>
    </div>
  );
}
