import type { Store } from '@/api/stores';
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
  store = { ...store, socialLinks: [{ platform: 'twitter', url: 'https://twitter.com/sophiacarter' } as any] }; // TODO : remove this
  return (
    <header>
      <div className={cn('mt-5 flex flex-col items-center p-6', className)}>
        {/* TODO : change to LazyImage */}
        <img
          alt="Sophia Carter"
          className="border-background-light dark:border-background-dark size-28 rounded-full border-4 object-cover"
          src={
            store.picture?.mainLink ||
            'https://lh3.googleusercontent.com/aida-public/AB6AXuAVVUW7VSR5qKZvyiFJrocy8LklzRipqoFqpbkZCKyOmfv7JmjP8JHP9m_1XYbqwN-KKIeIPmcZBUOmFRgjxbJHdC5HxztN7NS8ZUr4P-UF91F9iYl6yoixDD-R-ViQ3gAu8FL9ZOJOcYFX6hKNuNedrFWV5RTbYVLnMQLXa4NX2EE1Y5bG63rMkkjBvNqg1N4IPDHAru9vJ6BRLLMb39w2wqmL0RyA16I57e3Dby5l7DSSisuJvzlzRNcXi7R2ecY5NBo4CXAaoL85'
          }
        />
        <h3 className="mt-4 text-xl font-bold">{store.title}</h3>
        <p className="text-foreground text-sm">@{store.slug}</p>
        <p className="text-foreground mt-2 text-center text-sm">{store.description}</p>
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
