import { Button, Link } from '@/components';
import { cn } from '@/utils';
import { useProfileBySlug, useProfileEditStore } from '@/features/app/profile';
interface ProfileActionsProps {
  variant?: 'horizontal' | 'vertical';
  className?: string;
}

export function ProfileActions({
  variant = 'vertical',
  className,
}: ProfileActionsProps) {
  const { isSlugCurrent } = useProfileBySlug();

  const containerClasses = {
    horizontal: 'flex items-center gap-3',
    vertical: 'flex flex-col items-center gap-3',
  };

  const { setDefaultSection, openDialog } = useProfileEditStore();

  return (
    <div className={cn(containerClasses[variant], className)}>
      {isSlugCurrent ? (
        <Button
          className="rounded-xl w-full"
          size="lg"
          onClick={() => {
            setDefaultSection('Basic Info');
            openDialog();
          }}
        >
          Edit Profile
        </Button>
      ) : (
        <Link to={'/booking/session/john-doe'}>
          <Button variant="outline" size="lg" className="rounded-xl w-full">
            Book a session
          </Button>
        </Link>
      )}
    </div>
  );
}
