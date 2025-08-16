import {
  Alert,
  alertIconMap,
  AlertTitle,
  AlertDescription,
  Button,
  Checkbox,
  Label,
} from '@/components';
import { useUser } from '@/features/auth';

import React from 'react';

export function CompletionStatus() {
  const { data: user } = useUser();

  const profileFields = Object.entries(
    user?.profileCompletionStatus ?? {},
  ).filter(([key]) => key !== 'completionStatus' && key !== 'totalFields');

  // Format field names for display
  const formatFieldName = (fieldName: string): string => {
    return fieldName
      .replace(/([A-Z])/g, ' $1')
      .replace(/^./, (str) => str.toUpperCase())
      .trim();
    // formatFieldName("emailAddress"); // "Email Address"
  };
  return (
    <div className="space-y-4">
      <Alert variant={'info'}>
        {React.createElement(alertIconMap['info'])}
        <AlertTitle>Complete your profile to become a mentor</AlertTitle>
        <AlertDescription>
          You need to fill out all required fields before you can offer
          mentoring services.
        </AlertDescription>
      </Alert>

      <div className="grid gap-3 md:grid-cols-2">
        {profileFields.map(([key, isCompleted]) => (
          <div
            key={key}
            className="flex items-center space-x-3 p-3 border rounded-lg"
          >
            <Checkbox
              id={key}
              checked={isCompleted === true}
              disabled
              className="data-[state=checked]:bg-green-600"
            />
            <Label
              htmlFor={key}
              className={`font-medium ${
                isCompleted ? 'text-green-700' : 'text-gray-600'
              }`}
            >
              {formatFieldName(key)}
            </Label>
          </div>
        ))}
      </div>

      <Button
        disabled={true}
        variant="secondary"
        className="w-full md:w-auto opacity-50 cursor-not-allowed"
      >
        Complete Profile First
      </Button>
    </div>
  );
}
