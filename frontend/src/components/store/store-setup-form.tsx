import { useState } from 'react';
import { cn } from '@/lib/cn';
import { SlugField } from './slug-field';
import { useCheckSlugAvailability } from '@/api/stores/stores/private/check-slug-availability';
import useDebounce from '@/hooks/use-debounce';

interface StoreFormData {
  name: string;
  slug: string;
  description: string;
  profilePicture: File | null;
}

interface StoreSetupFormProps {
  onSubmit: (data: StoreFormData) => void;
  isLoading?: boolean;
  className?: string;
}

export function StoreSetupForm({ onSubmit, isLoading = false, className }: StoreSetupFormProps) {
  const [formData, setFormData] = useState<StoreFormData>({
    name: '',
    slug: '',
    description: '',
    profilePicture: null,
  });

  // Debounce slug changes to avoid too many API calls
  const debouncedSlug = useDebounce(formData.slug, 500);

  // Real slug availability check
  const { data: slugAvailabilityResponse } = useCheckSlugAvailability(debouncedSlug, debouncedSlug.length >= 3);

  const slugAvailable = slugAvailabilityResponse?.isAvailable;

  const handleSlugChange = (slug: string) => {
    setFormData((prev) => ({ ...prev, slug }));
  };

  const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0] || null;
    setFormData((prev) => ({ ...prev, profilePicture: file }));
  };

  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    if (formData.name && formData.slug && slugAvailable) {
      onSubmit(formData);
    }
  };

  const isValid = formData.name && formData.slug && slugAvailable;

  return (
    <form onSubmit={handleSubmit} className={cn('space-y-6', className)}>
      {/* Store Name */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Store Name *</label>
        <input
          type="text"
          value={formData.name}
          onChange={(e) => setFormData((prev) => ({ ...prev, name: e.target.value }))}
          placeholder="My Awesome Store"
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
          required
        />
      </div>

      {/* Slug Field */}
      <SlugField value={formData.slug} onChange={handleSlugChange} isAvailable={slugAvailable} />

      {/* Description */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Description</label>
        <textarea
          value={formData.description}
          onChange={(e) => setFormData((prev) => ({ ...prev, description: e.target.value }))}
          placeholder="Tell people what you're all about..."
          rows={3}
          className="bg-input border-border text-foreground placeholder:text-muted-foreground focus:ring-ring w-full resize-none rounded-lg border px-3 py-2 focus:ring-2 focus:outline-none"
        />
      </div>

      {/* Profile Picture */}
      <div className="space-y-2">
        <label className="text-foreground block text-sm font-medium">Profile Picture</label>
        <div className="flex items-center space-x-4">
          <div className="bg-muted flex h-16 w-16 items-center justify-center overflow-hidden rounded-full">
            {formData.profilePicture ? (
              <img src={URL.createObjectURL(formData.profilePicture)} alt="Profile preview" className="h-full w-full object-cover" />
            ) : (
              <span className="text-muted-foreground text-xl">{formData.name ? formData.name.charAt(0).toUpperCase() : '?'}</span>
            )}
          </div>
          <label className="cursor-pointer">
            <input type="file" accept="image/*" onChange={handleFileChange} className="hidden" />
            <span className="bg-secondary text-secondary-foreground hover:bg-secondary/80 inline-flex items-center rounded-lg px-4 py-2 transition-colors">
              Choose Photo
            </span>
          </label>
        </div>
      </div>

      {/* Submit Button */}
      <button
        type="submit"
        disabled={!isValid || isLoading}
        className={cn(
          'w-full rounded-lg px-4 py-3 font-semibold transition-all',
          isValid && !isLoading ? 'bg-primary text-primary-foreground hover:opacity-90' : 'bg-muted text-muted-foreground cursor-not-allowed',
        )}
      >
        {isLoading ? (
          <span className="flex items-center justify-center">
            <div className="mr-2 h-5 w-5 animate-spin rounded-full border-2 border-current border-t-transparent"></div>
            Creating Store...
          </span>
        ) : (
          'Create Store'
        )}
      </button>
    </form>
  );
}
