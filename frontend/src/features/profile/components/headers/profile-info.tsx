import { useProfileBySlug } from '@/features/profile/hooks';
import { cn } from '@/utils';
import { FaMapMarkerAlt } from 'react-icons/fa';
import { MdVerified } from 'react-icons/md';

interface ProfileInfoProps {
  titleSize?: 'sm' | 'lg';
  className?: string;
}

export function ProfileInfo({ titleSize = 'lg', className }: ProfileInfoProps) {
  const { user } = useProfileBySlug();

  const titleClasses = {
    sm: 'text-2xl',
    lg: 'text-3xl',
  };

  const verifiedIconSize = {
    sm: 'w-5 h-5',
    lg: 'w-6 h-6',
  };

  return (
    <div className={cn('space-y-3', className)}>
      <div className="flex items-center gap-2">
        <h1
          className={cn('font-bold text-foreground', titleClasses[titleSize])}
        >
          {user?.firstName} {user?.lastName}
        </h1>
        <MdVerified
          className={cn('text-primary', verifiedIconSize[titleSize])}
        />
      </div>

      <div className="flex items-center gap-2 text-muted-foreground">
        <FaMapMarkerAlt className="w-4 h-4" />
        <span
          className={cn('font-medium', titleSize === 'sm' ? 'text-sm' : '')}
        >
          Software Engineer Student at Issat Sousse
        </span>
      </div>

      <div className="flex items-center gap-4 text-sm text-muted-foreground">
        <div className="flex items-center gap-1">
          <span className="font-semibold text-foreground">156</span>
          <span>Connections</span>
        </div>
        <div className="flex items-center gap-1">
          <span className="font-semibold text-foreground">42</span>
          <span>Reviews</span>
        </div>
      </div>
    </div>
  );
}
