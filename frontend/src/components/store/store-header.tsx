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
    <div className={cn('bg-card border-b border-border p-6 text-center', className)}>
      {/* Profile Picture */}
      <div className="mb-4">
        <div className="w-16 h-16 mx-auto rounded-full overflow-hidden bg-muted">
          {store.profilePicture ? (
            <img src={store.profilePicture} alt={store.name} className="w-full h-full object-cover" />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-muted-foreground">
              <span className="text-2xl font-semibold">{store.name.charAt(0).toUpperCase()}</span>
            </div>
          )}
        </div>
      </div>

      {/* Store Name */}
      <h1 className="text-xl font-semibold text-foreground mb-2">{store.name}</h1>

      {/* Description */}
      {store.description && <p className="text-sm text-muted-foreground mb-4 leading-relaxed">{store.description}</p>}

      {/* Social Links */}
      {store.socialLinks && (
        <div className="flex justify-center space-x-3">
          {store.socialLinks.instagram && (
            <a
              href={store.socialLinks.instagram}
              className="w-8 h-8 rounded-full bg-muted flex items-center justify-center text-muted-foreground hover:bg-accent hover:text-accent-foreground transition-colors"
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="text-xs">IG</span>
            </a>
          )}
          {store.socialLinks.twitter && (
            <a
              href={store.socialLinks.twitter}
              className="w-8 h-8 rounded-full bg-muted flex items-center justify-center text-muted-foreground hover:bg-accent hover:text-accent-foreground transition-colors"
              target="_blank"
              rel="noopener noreferrer"
            >
              <span className="text-xs">X</span>
            </a>
          )}
          {store.socialLinks.website && (
            <a
              href={store.socialLinks.website}
              className="w-8 h-8 rounded-full bg-muted flex items-center justify-center text-muted-foreground hover:bg-accent hover:text-accent-foreground transition-colors"
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
