import { MentorRequired } from '@/components/errors';
import { LoadingState } from '@/components/ui';
import { useMentor } from '@/hooks';

interface MentorGuardProps {
  children: React.ReactNode;
  fallback?: React.ReactNode;
  customTitle?: string;
  customMessage?: string;
  actionDescription?: string;
}

export const MentorGuard = ({
  children,
  fallback,
  customTitle,
  customMessage,
  actionDescription,
}: MentorGuardProps) => {
  const { mentor, isLoading } = useMentor();

  if (isLoading) {
    return <LoadingState type="dots" message={'loading user info'} />;
  }

  if (!mentor) {
    return (
      fallback || (
        <MentorRequired
          title={customTitle}
          message={customMessage}
          actionDescription={actionDescription}
        />
      )
    );
  }

  return <>{children}</>;
};
