import { useUser } from '@/features/auth';
import { Checkbox, Label, Progress , Button }  from '@/components/ui';

export function BecomeMentorPage() {
  const { data: user, isLoading, error } = useUser();
  const completion = user?.profileCompletionStatus?.completionStatus ?? 0;
  const total = user?.profileCompletionStatus?.totalFields ?? 1;
  const percentage = Math.floor((completion / total) * 100);
  return (
    <div className="py-10 mx-10 space-y-2">
      <div className="font-bold text-xl"> Become a Mentor </div>
      <Label> Profile Completion : {percentage} % </Label>
      <Progress value={percentage}></Progress>
      <div className="flex flex-col gap-4 mt-4">
        {Object.entries(user?.profileCompletionStatus ?? {})
          .filter(
            ([key]) => key !== 'completionStatus' && key !== 'totalFields',
          )
          .map(([key, value]) => (
            <div key={key} className="flex items-center space-x-2">
              <Checkbox id={key} disabled defaultChecked={value === true} />
              <Label htmlFor={key} className="font-semibold">{key}</Label>
            </div>
          ))}
      </div>
      <Button className="w-full mt-2" disabled={percentage != 100}>Become Mentor</Button>
    </div>
  );
}
