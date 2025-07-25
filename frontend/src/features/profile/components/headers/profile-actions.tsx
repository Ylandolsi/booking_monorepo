import { Button } from '@/components/ui';
import { cn } from '@/utils';
import { ProfileEditDialog } from '@/features/profile';

interface ProfileActionsProps {
  isSlugCurrent: boolean;
  variant?: 'horizontal' | 'vertical';
  className?: string;
}

export function ProfileActions({
  isSlugCurrent,
  variant = 'vertical',
  className,
}: ProfileActionsProps) {
  const containerClasses = {
    horizontal: 'flex items-center gap-3',
    vertical: 'flex flex-col items-center gap-3',
  };

  return (
    <div className={cn(containerClasses[variant], className)}>
      {isSlugCurrent && <ProfileEditDialog />}
      <Button variant="outline" size="lg" className="rounded-xl w-full">
        Message
      </Button>
    </div>
  );
}
