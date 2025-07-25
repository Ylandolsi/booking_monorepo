import {
  Alert,
  AlertDescription,
  Button,
  Label,
  MultiSelect,
  Spinner,
} from '@/components/ui';
import {
  useAllExpertises,
  useUpdateExpertises,
} from '@/features/profile/hooks';
import { useProfileEditStore } from '@/features/profile/stores';
import type { User } from '@/types/api';
import { Terminal } from 'lucide-react';
import { useEffect, useState } from 'react';

export function Expertise({ userData }: { userData: User }) {
  const { setSelectedExpertise, selectedExpertise } = useProfileEditStore();
  const updateExpertiseMutation = useUpdateExpertises();

  const { data: allExpertiseOptions, isLoading, error } = useAllExpertises();

  const [errorUpdate, setErrorUpdate] = useState<string | null>(null);

  // sync expertisestate with userExpertise
  useEffect(() => {
    setSelectedExpertise(userData.expertises.map((exp) => exp.id.toString()));
  }, []);

  if (isLoading) {
    return <Spinner />;
  }

  if (error) {
    return (
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Expertise</Label>
        <div className="text-red-500 text-sm">
          Failed to load expertise options
        </div>
      </div>
    );
  }

  const expertiseOptions =
    allExpertiseOptions?.map((expertise) => ({
      value: expertise.id.toString(),
      label: expertise.name,
    })) || [];

  const syncStateWithChoosenValues = (values: string[]) => {
    console.log(values);

    const idsExpertise: number[] = values.map((vl) => {
      return Number(vl);
    });
    setSelectedExpertise(idsExpertise);
  };

  const updateExpertise = () => {
    const length = selectedExpertise.length;
    console.log(length);
    if (length > 4) {
      setErrorUpdate('You cant select more than 4 expertises ');
      return;
    } else {
      updateExpertiseMutation.mutate({ expertises: selectedExpertise });
      setErrorUpdate(null);
    }
  };

  return (
    <div className="space-y-2">
      <div className="flex justify-between items-center">
        <Label className="block text-sm font-medium">Expertise</Label>
        <Button
          variant={'default'}
          className="p-2 w-fit h-6 mr-2 font-semibold"
          onClick={updateExpertise}
        >
          Save
        </Button>
      </div>
      <MultiSelect
        options={expertiseOptions}
        onValueChange={syncStateWithChoosenValues}
        defaultValue={userData?.expertises.map((exp) => exp.id.toString())}
        placeholder="Select up to 4 expertise areas"
        maxCount={4}
      />
      {errorUpdate && (
        <Alert variant="destructive">
          <Terminal />
          <AlertDescription>{errorUpdate}</AlertDescription>
        </Alert>
      )}
    </div>
  );
}
