import React from 'react';
import { Alert, AlertDescription, alertIconMap } from '@/components/ui';

interface SaveSuccessAlertProps {
  show: boolean;
}

export function SaveSuccessAlert({ show }: SaveSuccessAlertProps) {
  if (!show) return null;

  return (
    <Alert variant="success" className="mb-4">
      {React.createElement(alertIconMap['success'])}
      <AlertDescription>
        Your availability has been saved successfully!
      </AlertDescription>
    </Alert>
  );
}
