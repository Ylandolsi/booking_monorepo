import { useUser } from '@/features/auth';
import { Label, Progress } from '@/components/ui';
import {
  CompletionStatus,
  MentorPreferences,
} from '@/features/mentor/become/componenets';
import { useMentorDetails } from '@/features/mentor/become/api';
import { AlreadyMentorPage } from '@/features/mentor/become/pages/already-mentor-page';

export function BecomeMentorPage() {
  const { data: user } = useUser();
  const { data: mentor } = useMentorDetails(user?.slug, { enabled: !!user });

  const completion = user?.profileCompletionStatus?.completionStatus ?? 0;
  const total = user?.profileCompletionStatus?.totalFields ?? 1;
  const percentage = Math.floor((completion / total) * 100);
  const isProfileComplete = percentage === 100;

  if (mentor) {
    return <AlreadyMentorPage />;
  }
  return (
    <div className="py-10 mx-10 space-y-6">
      <div className="space-y-2">
        <h1 className="font-bold text-2xl">Become a Mentor</h1>
        <div className="space-y-2">
          <Label className="text-sm text-gray-600">
            Profile Completion: {percentage}%
          </Label>
          <Progress value={percentage} className="w-full" />
        </div>
      </div>

      {isProfileComplete ? <MentorPreferences /> : <CompletionStatus />}
    </div>
  );
}
