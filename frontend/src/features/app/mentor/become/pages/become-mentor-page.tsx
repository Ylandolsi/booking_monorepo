import { useUser } from '@/api/auth';
import { Label, Progress } from '@/components/ui';
import { CompletionStatus, MentorPreferences } from '@/features/app/mentor/become/componenets';
import { useMentorDetails } from '@/features/app/mentor/become/api';
import { AlreadyMentorPage } from '@/features/app/mentor/become/pages/already-mentor-page';

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
    <div className="space-y-6">
      <div className="space-y-2">
        <h1 className="text-2xl font-bold">Become a Mentor</h1>
        <div className="space-y-2">
          <Label className="text-sm text-gray-600">Profile Completion: {percentage}%</Label>
          <Progress value={percentage} className="w-full" />
        </div>
      </div>

      {isProfileComplete ? <MentorPreferences /> : <CompletionStatus />}
    </div>
  );
}
