import { Button, ErrorComponenet, Link } from '@/components';
import { cn } from '@/utils';
import { useProfileBySlug, useProfileEditStore } from '@/features/app/profile';
import { routes } from '@/config';
interface ProfileActionsProps {
  variant?: 'horizontal' | 'vertical';
  className?: string;
}

export function ProfileActions({
  variant = 'vertical',
  className,
}: ProfileActionsProps) {
  const { isSlugCurrent, user } = useProfileBySlug();

  const { setDefaultSection, openDialog } = useProfileEditStore();

  const containerClasses = {
    horizontal: 'flex items-center gap-3',
    vertical: 'flex flex-col items-center gap-3',
  };

  if (!user)
    return (
      <ErrorComponenet
        title={'User not found'}
        message={'Failed to fetch the user or user doesnt exists .'}
      />
    );
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
        <Link
          to={routes.paths.APP.BOOKING.SESSION}
          params={{ mentorSlug: user.slug }}
        >
          <Button variant="outline" size="lg" className="rounded-xl w-full">
            Book a session
          </Button>
        </Link>
      )}
    </div>
  );
}
