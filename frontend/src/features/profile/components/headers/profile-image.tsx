import { LazyImage } from '@/utils';
import { MdEdit } from 'react-icons/md';
import { cn } from '@/lib/utils';

interface ProfileImageProps {
  className?: string;
  size?: 'sm' | 'lg';
  isCurrentUser: boolean;
}

export function ProfileImage({
  className,
  size = 'lg',
  isCurrentUser = false,
}: ProfileImageProps) {
  const sizeClasses = {
    sm: 'w-24 h-24',
    lg: 'w-28 h-28',
  };

  const editIconSize = {
    sm: 'w-3 h-3',
    lg: 'w-4 h-4',
  };

  return (
    <div className={cn('relative group', className)}>
      <LazyImage
        src="https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250"
        alt="profile-picture"
        placeholder="https://www.gravatar.com/avatar/2c7d99fe281ecd3bcd65ab915bac6dd5?s=250"
        className={cn(
          'rounded-2xl ring-4 ring-primary/20 object-cover transition-all group-hover:ring-primary/40',
          sizeClasses[size],
        )}
      />
      {isCurrentUser && (
        <div className="absolute -top-2 -right-2 rounded-full bg-primary p-2 shadow-lg hover:bg-primary/90 transition-colors cursor-pointer">
          <MdEdit
            className={cn('text-primary-foreground', editIconSize[size])}
          />
        </div>
      )}
    </div>
  );
}
