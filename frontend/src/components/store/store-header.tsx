import type { Store } from '@/api/stores';
import { FALLBACK_PICTURE_STORE } from '@/assets';
import { Link } from '@/components/ui';
import { socialPlatforms } from '@/features/app/store';
import { cn } from '@/lib/cn';

interface StoreHeaderProps {
  store: Store;
  className?: string;
}

interface StoreHeaderProps {
  store: Store;
  className?: string;
}

export function StoreHeader({ store, className }: StoreHeaderProps) {
  store = { ...store };
  return (
    <header>
      <div className={cn('mt-2 flex flex-col items-center p-6', className)}>
        {/* TODO : change to LazyImage */}
        <img
          alt="Sophia Carter"
          className="border-background-light dark:border-background-dark size-28 rounded-full border-4 object-cover"
          src={store.picture?.mainLink || FALLBACK_PICTURE_STORE}
        />
        <h3 className="mt-4 text-2xl font-bold">{store.title}</h3>
        <p className="text-foreground text-sm">@{store.slug}</p>
        <p className="text-foreground mt-2 text-center text-sm break-all">{store.description}</p>
        {store.socialLinks && store.socialLinks.length > 0 && (
          <div className="mt-4 flex justify-center gap-4">
            {socialPlatforms.map(
              ({ key, icon: Icon }) =>
                store.socialLinks?.find((link: any) => link.platform === key)?.url && (
                  <Link
                    key={key}
                    href={store.socialLinks?.find((link: any) => link.platform === key)?.url}
                    target="_blank"
                    className="text-foreground hover:text-primary transition-colors"
                  >
                    <Icon className="h-5 w-5" />
                  </Link>
                ),
            )}
          </div>
        )}
      </div>
    </header>
  );
}
