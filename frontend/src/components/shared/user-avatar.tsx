import { LazyImage } from '@/utils/lazy-image';
import { Button } from '@/components/ui/button';
import { cn } from '@/utils/cn';
import { MdEdit } from 'react-icons/md';
import type { User } from '@/types/api';

interface UserAvatarProps {
  user: User;
  size?: 'sm' | 'md' | 'lg' | 'xl';
  showEditButton?: boolean;
  showOnlineStatus?: boolean;
  className?: string;
  onEdit?: () => void;
  onClick?: () => void;
}

const sizeClasses = {
  sm: 'w-8 h-8',
  md: 'w-12 h-12',
  lg: 'w-20 h-20',
  xl: 'w-28 h-28',
};

const editButtonSizes = {
  sm: 'p-1',
  md: 'p-1.5',
  lg: 'p-2',
  xl: 'p-2.5',
};

export function UserAvatar({
  user,
  size = 'md',
  showEditButton = false,
  showOnlineStatus = false,
  className = '',
  onEdit,
  onClick,
}: UserAvatarProps) {
  const avatarUrl =
    user.profilePictureUrl ||
    `https://www.gravatar.com/avatar/${user.email}?s=250&d=identicon`;

  return (
    <div className={cn('relative group', className)}>
      <LazyImage
        src={avatarUrl}
        alt={`${user.firstName} ${user.lastName}`}
        placeholder={avatarUrl}
        className={cn(
          sizeClasses[size],
          'rounded-2xl ring-4 ring-primary/20 object-cover transition-all cursor-pointer',
          onClick && 'hover:ring-primary/40',
          className,
        )}
        onClick={onClick}
      />

      {/* {showOnlineStatus && (
        <div className={cn(
          'absolute bottom-0 right-0 rounded-full border-2 border-background',
          user.isOnline ? 'bg-green-500' : 'bg-gray-400',
          size === 'sm' ? 'w-3 h-3' : size === 'md' ? 'w-4 h-4' : 'w-5 h-5'
        )} />
      )} */}

      {showEditButton && onEdit && (
        <Button
          size="sm"
          onClick={onEdit}
          className={cn(
            'absolute -top-2 -right-2 rounded-full bg-primary shadow-lg hover:bg-primary/90 transition-colors',
            editButtonSizes[size],
          )}
        >
          <MdEdit className="text-primary-foreground w-4 h-4" />
        </Button>
      )}
    </div>
  );
}
