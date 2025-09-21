import { cn } from '@/lib/cn';

interface Store {
  name: string;
  description?: string;
  profilePicture?: string;
  socialLinks?: {
    instagram?: string;
    twitter?: string;
    website?: string;
  };
}

interface StoreHeaderProps {
  store: Store;
  className?: string;
}

export function StoreHeader({ store, className }: StoreHeaderProps) {
  return (
    <div className={cn('bg-card text-center', className)}>
      {/* Profile Picture */}
      <div className="mb-4">
        <div className="bg-muted mx-auto h-16 w-16 overflow-hidden rounded-full">
          {store.profilePicture ? (
            <img src={store.profilePicture} alt={store.name} className="h-full w-full object-cover" />
          ) : (
            <div className="text-muted-foreground flex h-full w-full items-center justify-center">
              <span className="text-2xl font-semibold">{store.name.charAt(0).toUpperCase()}</span>
            </div>
          )}
        </div>
      </div>

      {/* Store Name */}
      <h1 className="text-foreground mb-2 text-xl font-semibold">{store.name}</h1>

      {/* Description */}
      {store.description && <p className="text-muted-foreground mb-4 text-sm leading-relaxed">{store.description}</p>}

      {/* Social Links */}
      {store.socialLinks && (
        <div className="flex justify-center space-x-3">
          {store.socialLinks.instagram && (
            <a
              href={store.socialLinks.instagram}
              className="bg-muted text-muted-foreground hover:bg-accent hover:text-accent-foreground flex h-8 w-8 items-center justify-center rounded-full transition-colors"
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="text-xs">IG</span>
            </a>
          )}
          {store.socialLinks.twitter && (
            <a
              href={store.socialLinks.twitter}
              className="bg-muted text-muted-foreground hover:bg-accent hover:text-accent-foreground flex h-8 w-8 items-center justify-center rounded-full transition-colors"
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="text-xs">X</span>
            </a>
          )}
          {store.socialLinks.website && (
            <a
              href={store.socialLinks.website}
              className="bg-muted text-muted-foreground hover:bg-accent hover:text-accent-foreground flex h-8 w-8 items-center justify-center rounded-full transition-colors"
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="text-xs">🌐</span>
            </a>
          )}
        </div>
      )}
    </div>
  );
}
