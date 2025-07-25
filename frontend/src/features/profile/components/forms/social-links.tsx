import { MainErrorFallback } from '@/components/errors/main';
import { Input, Spinner } from '@/components/ui';
import { useUser } from '@/features/auth';
import { Label } from '@radix-ui/react-label';
import type { SocialLinks } from '../../types';

const SOCIAL_FIELDS = [
  { key: 'linkedIn', label: 'LinkedIn', placeholder: 'Enter LinkedIn URL' },
  { key: 'portfolio', label: 'Website', placeholder: 'Enter Website URL' },
  { key: 'github', label: 'GitHub', placeholder: 'Enter GitHub URL' },
  { key: 'instagram', label: 'Instagram', placeholder: 'Enter Instagram URL' },
  { key: 'youtube', label: 'Youtube', placeholder: 'Enter Youtube URL' },
  { key: 'facebook', label: 'Facebook', placeholder: 'Enter Facebook URL' },
];

export function SocialLinksForm() {
  const { data: userData, error, isLoading } = useUser();
  if (isLoading) return <Spinner />;
  if (error) return <MainErrorFallback />;

  const socialLinks: SocialLinks = userData?.socialLinks ?? {};

  const sortedFields = [...SOCIAL_FIELDS].sort((a, b) => {
    const aDefined = !!socialLinks[a.key];
    const bDefined = !!socialLinks[b.key];
    return aDefined === bDefined ? 0 : aDefined ? -1 : 1;
  });

  return (
    <div className="space-y-4">
      {sortedFields.map(({ key, label, placeholder }) => (
        <div className="space-y-2" key={key}>
          <Label className="block text-sm font-medium">{label}</Label>
          <Input
            type="url"
            placeholder={placeholder}
            defaultValue={socialLinks[key] ?? ''}
          />
        </div>
      ))}
    </div>
  );
}
