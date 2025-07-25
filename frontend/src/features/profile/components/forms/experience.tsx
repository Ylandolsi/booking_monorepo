import { Button, MultiSelect, Spinner, Textarea } from '@/components/ui';
import { Label } from '@radix-ui/react-label';
import { EXPERTISE_OPTIONS } from '../../constants';
import { useUser } from '@/features/auth';
import { MainErrorFallback } from '@/components/errors/main';
import { MdOutlineWork } from 'react-icons/md';
import { formatDate } from '@/utils';
import { FaBookReader } from 'react-icons/fa';

export function ExperienceForm({
  selectedExpertise,
  setSelectedExpertise,
}: {
  selectedExpertise: string[];
  setSelectedExpertise: (value: string[]) => void;
}) {
  const { data: userData, error, isLoading } = useUser();
  if (isLoading) return <Spinner />;
  if (error) return <MainErrorFallback />;
  return (
    <div className="space-y-4">
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Expertise</Label>
        <MultiSelect
          options={[...EXPERTISE_OPTIONS]}
          onValueChange={setSelectedExpertise}
          defaultValue={userData?.expertises.map((exp) => exp.name)}
          placeholder="Select up to 4 expertise areas"
          maxCount={4}
        />
      </div>
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Work Experience</Label>
        <div className="flex flex-col">
          {userData?.experiences.map((exp) => (
            <div
              key={exp.id}
              className="flex mb-2 p-2 border border-border rounded-xl justify-between gap-3 relative items-center"
            >
              <div className="flex flex-shrink-0 w-10 h-10 bg-primary/10 rounded-xl items-center justify-center">
                <MdOutlineWork className="w-6 h-6 text-primary" />
              </div>
              <div className="flex-1 ">
                <div className="font-semibold text-sm text-foreground">
                  {exp.title}
                </div>
                <div className="text-xs text-muted-foreground">
                  {exp.companyName}
                  <br></br>
                  {formatDate(exp.startDate)} -{' '}
                  {exp.endDate ? formatDate(exp.endDate) : 'Present'}
                </div>
              </div>
              <Button
                variant="ghost"
                className="rounded-md hover:bg-border/20 "
                onClick={() => {
                  // Handle edit action
                }}
              >
                {' '}
                Edit{' '}
              </Button>
            </div>
          ))}
          <Button variant={'outline'} className="border-border">
            Add new experience
          </Button>
        </div>
        <div className="space-y-2">
          <Label className="block text-sm font-medium">Education</Label>
          <div className="flex flex-col  gap-2">
            {userData?.educations.map((ed) => (
              <div className="border border-border flex justify-between items-center p-2 rounded-xl gap-3">
                <div className="bg-border w-10 h-10 flex items-center justify-center rounded-xl">
                  <FaBookReader className="w-6 h-6 text-primary" />
                </div>
                <div className="flex-1">
                  <div className="font-semibold text-sm">{ed.field}</div>
                  <div className="text-xs text-muted-foreground">
                    {ed.university}
                    <br></br>
                    {formatDate(ed.startDate)} -{' '}
                    {ed.endDate ? formatDate(ed.endDate) : 'Present'}
                  </div>
                </div>
                <Button variant="ghost" className="hover:bg-border/20">
                  Edit
                </Button>
              </div>
            ))}
            <Button variant={'outline'} className="border-border">
              Add new Education
            </Button>
          </div>
        </div>
      </div>
    </div>
  );
}
