import { useState } from 'react';
import { cn } from '@/lib/cn';
import { SlugField } from './slug-field';

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

  // Mock slug availability check - replace with actual API call
  const [slugAvailable, setSlugAvailable] = useState<boolean | undefined>(undefined);

  // Simulate slug validation
  const handleSlugChange = (slug: string) => {
    setFormData((prev) => ({ ...prev, slug }));

    if (slug.length >= 3) {
      // Simulate API call delay
      setTimeout(() => {
        // Mock: slugs starting with 'taken' are unavailable
        setSlugAvailable(!slug.startsWith('taken'));
      }, 500);
    } else {
      setSlugAvailable(undefined);
    }
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
        <label className="block text-sm font-medium text-foreground">Store Name *</label>
        <input
          type="text"
          value={formData.name}
          onChange={(e) => setFormData((prev) => ({ ...prev, name: e.target.value }))}
          placeholder="My Awesome Store"
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring"
          required
        />
      </div>

      {/* Slug Field */}
      <SlugField value={formData.slug} onChange={handleSlugChange} isAvailable={slugAvailable} />

      {/* Description */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Description</label>
        <textarea
          value={formData.description}
          onChange={(e) => setFormData((prev) => ({ ...prev, description: e.target.value }))}
          placeholder="Tell people what you're all about..."
          rows={3}
          className="w-full px-3 py-2 bg-input border border-border rounded-lg text-foreground placeholder:text-muted-foreground focus:outline-none focus:ring-2 focus:ring-ring resize-none"
        />
      </div>

      {/* Profile Picture */}
      <div className="space-y-2">
        <label className="block text-sm font-medium text-foreground">Profile Picture</label>
        <div className="flex items-center space-x-4">
          <div className="w-16 h-16 rounded-full overflow-hidden bg-muted flex items-center justify-center">
            {formData.profilePicture ? (
              <img src={URL.createObjectURL(formData.profilePicture)} alt="Profile preview" className="w-full h-full object-cover" />
            ) : (
              <span className="text-muted-foreground text-xl">{formData.name ? formData.name.charAt(0).toUpperCase() : '?'}</span>
            )}
          </div>
          <label className="cursor-pointer">
            <input type="file" accept="image/*" onChange={handleFileChange} className="hidden" />
            <span className="inline-flex items-center px-4 py-2 bg-secondary text-secondary-foreground rounded-lg hover:bg-secondary/80 transition-colors">
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
          'w-full py-3 px-4 rounded-lg font-semibold transition-all',
          isValid && !isLoading ? 'bg-primary text-primary-foreground hover:opacity-90' : 'bg-muted text-muted-foreground cursor-not-allowed',
        )}
      >
        {isLoading ? (
          <span className="flex items-center justify-center">
            <div className="w-5 h-5 border-2 border-current border-t-transparent rounded-full animate-spin mr-2"></div>
            Creating Store...
          </span>
        ) : (
          'Create Store'
        )}
      </button>
    </form>
  );
}
