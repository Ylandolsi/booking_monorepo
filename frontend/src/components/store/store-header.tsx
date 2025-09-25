import type { Store } from '@/api/stores';
import { socialPlatforms } from '@/features/app/store';
import { FALLBACK_PROFILE_PICTURE } from '@/lib';
import { cn } from '@/lib/cn';
import { User } from 'lucide-react';

interface StoreHeaderProps {
  store: Store;
  className?: string;
}

interface StoreHeaderProps {
  store: Store;
  className?: string;
}

export function StoreHeader({ store, className }: StoreHeaderProps) {
  return (
    <div className={cn('text-center', className)}>
      {/* Profile Picture */}
      <div className="mx-auto mb-4 flex h-20 w-20 items-center justify-center rounded-full">
        {store.picture ? (
          // TODO : replace with LazyImage
          // and store.picture
          <img src={FALLBACK_PROFILE_PICTURE} alt="Profile" className="h-full w-full rounded-full object-cover" />
        ) : (
          <User className="text-primary-foreground h-8 w-8" />
        )}
      </div>

      <h2 className="text-foreground mb-2 text-xl font-bold">{store.title || 'Your Store Name'}</h2>

      <p className="text-muted-foreground mb-4 line-clamp-3 text-sm leading-relaxed text-wrap break-words">
        {store.description || 'Your store description will appear here...'}
      </p>

      {store.socialLinks && store.socialLinks.length > 0 && (
        <div className="mb-6 flex justify-center gap-4">
          {socialPlatforms.map(
            ({ key, icon: Icon }) =>
              store.socialLinks?.find((link: any) => link.platform === key)?.url && (
                <a
                  key={key}
                  href={store.socialLinks?.find((link: any) => link.platform === key)?.url}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="text-primary hover:text-accent transition-colors"
                >
                  <Icon className="h-5 w-5" />
                </a>
              ),
          )}
        </div>
      )}
    </div>
  );
}
