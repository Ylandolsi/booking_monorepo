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
  type ExpertiseType,
} from '@/features/profile';
import { useProfileEditStore } from '@/features/profile/stores';
import type { User } from '@/types/api';
import { Terminal } from 'lucide-react';
import { useCallback, useEffect, useMemo, useState } from 'react';

const MAX_EXPERTISE_COUNT = 4;
const EXPERTISE_LIMIT_ERROR = `You can't select more than ${MAX_EXPERTISE_COUNT} expertise areas`;

interface ExpertiseProps {
  userData: User;
}

interface ExpertiseOption {
  value: string;
  label: string;
}

export function Expertise({ userData }: ExpertiseProps) {
  const { setSelectedExpertise, selectedExpertise } = useProfileEditStore();
  const { data: allExpertiseOptions, isLoading, error } = useAllExpertises();
  const [validationError, setValidationError] = useState<string | null>(null);

  const updateExpertiseMutation = useUpdateExpertises();

  const expertiseOptions: ExpertiseOption[] = useMemo(() => {
    return (allExpertiseOptions ?? []).map((expertise: ExpertiseType) => ({
      value: expertise.id.toString(),
      label: expertise.name,
    }));
  }, [allExpertiseOptions]);

  useEffect(() => {
    const userExpertiseIds = userData.expertises.map((exp) => exp.id);
    setSelectedExpertise(userExpertiseIds);
  }, [userData.expertises, setSelectedExpertise]);

  const handleSelectionChange = useCallback(
    (values: string[]) => {
      const expertiseIds = values.map(Number);

      if (values.length <= MAX_EXPERTISE_COUNT) {
        setValidationError(null);
      }

      setSelectedExpertise(expertiseIds);
    },
    [setSelectedExpertise],
  );

  const handleSave = useCallback(() => {
    if (selectedExpertise.length > MAX_EXPERTISE_COUNT) {
      setValidationError(EXPERTISE_LIMIT_ERROR);
      return;
    }

    setValidationError(null);
    updateExpertiseMutation.mutate({
      expertises: selectedExpertise,
    });
  }, [selectedExpertise, updateExpertiseMutation]);

  if (isLoading) {
    return (
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Expertise</Label>
        <div className="flex items-center justify-center p-4">
          <Spinner />
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="space-y-2">
        <Label className="block text-sm font-medium">Expertise</Label>
        <Alert variant="destructive">
          <Terminal className="h-4 w-4" />
          <AlertDescription>
            Failed to load expertise options. Please try again later.
          </AlertDescription>
        </Alert>
      </div>
    );
  }

  const hasChanges =
    selectedExpertise.length !== userData.expertises.length ||
    !selectedExpertise.every((id) =>
      userData.expertises.some((exp) => exp.id === id),
    );

  return (
    <div className="space-y-2">
      <div className="flex justify-between items-center">
        <Label className="block text-sm font-medium">Expertise</Label>
        <Button
          variant="default"
          size="sm"
          onClick={handleSave}
          disabled={updateExpertiseMutation.isPending || !hasChanges}
          className="font-semibold h-5"
        >
          {updateExpertiseMutation.isPending ? (
            <Spinner className="h-4 w-4" />
          ) : (
            'Save'
          )}
        </Button>
      </div>

      <MultiSelect
        options={expertiseOptions}
        onValueChange={handleSelectionChange}
        defaultValue={userData.expertises.map((exp) => exp.id.toString())}
        placeholder={`Select up to ${MAX_EXPERTISE_COUNT} expertise areas`}
        maxCount={MAX_EXPERTISE_COUNT}
      />

      {/* Validation Error */}
      {validationError && (
        <Alert variant="destructive">
          <Terminal className="h-4 w-4" />
          <AlertDescription>{validationError}</AlertDescription>
        </Alert>
      )}
    </div>
  );
}
